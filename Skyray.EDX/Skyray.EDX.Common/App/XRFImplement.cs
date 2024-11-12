using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.Xml;
using System.Drawing;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using Skyray.EDX.Common.CSharpConvert;
using Skyray.EDX.Common.Library;

namespace Skyray.EDX.Common.App
{
    public class XRFImplement:IFactory
    {
        public XRFReadFile readFile = new XRFReadFile();
        public List<Device> ListDevice = new List<Device>();
        public static string QualitativePath = string.Empty;

        #region IFactory 成员
        public void LoadDeviceFactory(string directoryDev,string curvedir)
        {
            if (directoryDev.IsNullOrEmpty()||!Directory.Exists(directoryDev))
                return;
            try
            {
                DirectoryInfo sampleDir = new DirectoryInfo(Environment.CurrentDirectory + "\\Image\\SampleImage\\EDXX");
                if (sampleDir.Exists)
                    sampleDir.Delete(true);
            }
            catch { }
            DirectoryInfo dir = new DirectoryInfo(directoryDev);
            if (dir == null)
                return;
            DirectoryInfo[] deviceInfo = dir.GetDirectories();

            if (deviceInfo == null || deviceInfo.Length == 0)
                return;
            bool flag = false;
            Device.DeleteAll();
            Chamber.DeleteAll();
            Filter.DeleteAll();
            Collimator.DeleteAll();
            FPGAParams.DeleteAll();
            StandSample.DeleteAll();
            DemarcateEnergy.DeleteAll();
            Condition.DeleteAll();
            WorkCurve.DeleteAll();
            //Spec.DeleteAll();
            //SpecList.DeleteAll();
            CurveElement.DeleteAll();
            foreach (DirectoryInfo info in deviceInfo)
            {
                if (Device.FindOne(w => w.Name == info.Name) != null)
                    continue;
                Device device = Device.New;
                readFile.ReadFileChamber(info.FullName + "\\ChamberMotor.xml", device);
                readFile.ReadFileCollimator(info.FullName + "\\CollMotor.xml", device);
                readFile.ReadFileFilter(info.FullName + "\\FilterMotor.xml", device);
                readFile.ReadFileRaytube(info.FullName + "\\RayTub.xml", device);
                readFile.ReadFileDetector(info.FullName + "\\Device.xml", device);
                readFile.IsExistMoveAis(info.FullName + "\\zMotor.xml", device);
                device.Name = info.Name;
                device.IsDP5 = false;
                device.Dp5Version = Dp5Version.Dp5_CommonUsb;
                device.DeviceID = "";
                device.SpecLength = SpecLength.Normal;
                device.VoltageScaleFactor = 1;
                device.CurrentScaleFactor = 1;
                List<Condition> conditionList = readFile.ReadFileCondition(info.FullName + "\\Condition.xml");
                if (conditionList == null)
                    continue;
                foreach (Condition conditon in conditionList)
                {
                    if (conditon.Name != "Intellectualized")
                    {
                        device.Conditions.Add(conditon);
                    }
                }
                //device.Save();
                //查找测量条件为匹配类型的条件
                Condition condi = device.Conditions.ToList().Find(w => w.Type == ConditionType.Match);
                if (condi == null)
                {
                    condi = Condition.New.Init(Info.Match);
                    condi.Type = ConditionType.Match;
                    condi.Device = device;
                    condi.InitParam = Default.GetInitParameter(device.SpecLength);//加载默认值
                    condi.DeviceParamList.Add(Default.GetDeviceParameter(device.SpecLength,1));
                    condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                    condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                    device.Conditions.Add(condi);
                }
                condi = device.Conditions.ToList().Find(w => w.Type == ConditionType.Intelligent);
                if (condi == null)
                {
                    Condition temp = conditionList.Find(w => w.Name == "Intellectualized");
                    if (temp == null)
                    {
                        condi = Condition.New.Init(Info.IntelligentCondition);
                        condi.Type = ConditionType.Intelligent;
                        condi.Device = device;
                        condi.InitParam = Default.GetInitParameter(device.SpecLength);//加载默认值
                        condi.DeviceParamList.Add(Default.GetDeviceParameter(device.SpecLength, 1));
                        condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                        condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                        device.Conditions.Add(condi);
                        
                    }
                    else
                    {
                        condi = Condition.New.Init(Info.IntelligentCondition);
                        condi.Type = ConditionType.Intelligent;
                        condi.Device = device;
                        condi.InitParam = temp.InitParam;
                        condi.DeviceParamList.Add(temp.DeviceParamList[0]);
                        condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                        condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                        device.Conditions.Add(condi);
                    }
                }
               
                foreach (Condition tt in device.Conditions)
                {
                    List<DemarcateEnergy> demacateEnergy = readFile.GetConditionDemarcate(QualitativePath + "\\DemarcateEnergy.xml");
                    if (demacateEnergy != null && demacateEnergy.Count > 0)
                    {
                       tt.DemarcateEnergys.Clear();
                       demacateEnergy.ForEach(w =>
                       {
                           tt.DemarcateEnergys.Add(w);
                       });
                    }
                }
                if (!flag)
                    device.IsDefaultDevice = true;
                device.Save();
                LoadCurveFactory(curvedir,device.Conditions.ToList());
                flag = true;
                ListDevice.Add(device);
            }
        }

        public void LoadCurveFactory(string directoryDev,List<Condition> listCondition)
        {
            if (directoryDev.IsNullOrEmpty())
                return;
            if (!Directory.Exists(directoryDev))
                Directory.CreateDirectory(directoryDev);
            DirectoryInfo dir = new DirectoryInfo(directoryDev);

            DirectoryInfo[] curveInfo = dir.GetDirectories();

             if (curveInfo == null || curveInfo.Length == 0)
                return;
             foreach (DirectoryInfo info in curveInfo)
             {
                 if (WorkCurve.FindOne(w => w.Name == info.Name) != null)
                     continue;
                 Console.Write("XRF工作曲线" + info.Name + "导入！\r\n");
                 WorkCurve workCurve = WorkCurve.New;
                 workCurve.Name = info.Name;
                 readFile.CreateCurve(workCurve, info.FullName + "\\WorkCurve.xml", listCondition);
                 ElementList elementList = readFile.GetIntestedElementList(info.FullName + "\\ElemList.xml", info.FullName);
                 workCurve.ElementList = elementList;
                 if (workCurve.ElementList != null && workCurve.ElementList.Items != null && workCurve.ElementList.Items.Count > 0)
                 {
                     foreach (CurveElement curveElement in workCurve.ElementList.Items)
                     {
                         curveElement.DevParamId = workCurve.Condition.DeviceParamList[0].Id;
                         curveElement.DistrubThreshold = "";
                         readFile.ReadElementsSample(info.FullName + "\\Data.xml", curveElement);
                     }
                 }
                 #region
                 //if (workCurve.Condition != null)
                 //{
                 //    workCurve.Condition.DemarcateEnergys.Clear();
                 //    List<DemarcateEnergy> listEnergy = readFile.GetConditionDemarcate(info.FullName + "\\DemarcateEnergy.xml");
                 //    if (listEnergy != null && listEnergy.Count > 0)
                 //    {
                 //        foreach (DemarcateEnergy enery in listEnergy)
                 //        {
                 //            enery.Condition = workCurve.Condition;
                 //            workCurve.Condition.DemarcateEnergys.Add(enery);
                 //        }
                 //    }
                 //}
                 #endregion
                 workCurve.Save();
             }
        }

        #endregion

        #region IFactory 成员

        public void RepeatCaculateSampleIntensity(string dataPath)
        {

        }
       
        public SpecListEntity LoadSpecFactory(string fileName)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                SpecListEntity specList = new SpecListEntity();
                DeviceParameterEntity deviceParams = new DeviceParameterEntity();
                string deviceName = ReadHelper.LoadDeviceInfo(xmlDoc, deviceParams);
                SpecEntity spec = new SpecEntity();
                ReadHelper.LoadSampleInfo(xmlDoc, specList, spec);
                FileInfo info = new FileInfo(fileName);
                XmlNode xmlElemRoot = xmlDoc.SelectSingleNode("Spectrum");
                XmlNode xmlElem = xmlElemRoot.SelectSingleNode("Data");
                char[] sep = new char[] { ',' };
                string[] dataStr = xmlElem.InnerText.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                string str = string.Empty;
                for (int i = 0; i < dataStr.Length; ++i)
                {
                    str += Convert.ToInt32(dataStr[i]) + ",";
                }
                spec.SpecData = str;
                spec.TubCurrent = deviceParams.TubCurrent;
                spec.TubVoltage = deviceParams.TubVoltage;
                spec.Name = info.Name;
                spec.RemarkInfo = "";
                spec.IsSmooth = false;
                Device device = Device.FindOne(w => w.IsDefaultDevice == true);
                if (device != null && device.Conditions.Count > 0)
                    specList.DemarcateEnergys = Default.ConvertFormOldToNew(device.Conditions[0].DemarcateEnergys, device.SpecLength);
                else
                {
                    List<DemarcateEnergy> listDemarcate = new List<DemarcateEnergy>();
                    listDemarcate.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                    listDemarcate.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                    specList.DemarcateEnergys = Default.ConvertFormOldToNew(listDemarcate, device.SpecLength);
                }
                spec.DeviceParameter = deviceParams;
                specList.InitParam = Default.GetInitParameter(device.SpecLength).ConvertToNewEntity();
                specList.Color = Color.Blue.ToArgb();
                specList.VirtualColor = Color.Blue.ToArgb();
                specList.Name = info.Name;
                specList.SpecType = SpecType.StandSpec;
                specList.Specs = new SpecEntity[1];
                specList.Specs[0] = spec;
                return specList;
            }
            catch { }
            return null;
        }
        #endregion
    }
}
