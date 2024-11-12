using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDX.Common;
using System.IO;
using Skyray.EDXRFLibrary;
using System.Configuration;
using System.Xml;
using System.Drawing;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using Skyray.EDX.Common.CSharpConvert;
using Skyray.EDX.Common.Library;

namespace Skyray.EDX.Common.App
{
    public class ROHSImplement : IFactory
    {
        public ROHSReadFile readFile = new ROHSReadFile();


        #region IFactory 成员
        public void RepeatCaculateSampleIntensity(string dataPath)
        {
            List<SpecListEntity> list = new List<SpecListEntity>();
            DirectoryInfo info = new DirectoryInfo(dataPath);
            List<FileInfo> files = FileFoundHelper.GetDirAllFiles(dataPath, "Spe");
            var listCuve = WorkCurve.FindAll().ToList().FindAll(delegate(WorkCurve tt) { return tt.Condition.Device.IsDefaultDevice == true; });

            Helper.ExperienceCoeffs.LoadExperienceCoeffs(AppDomain.CurrentDomain.BaseDirectory + "Parameter.xml");
            string exePath = System.IO.Path.Combine(
      Environment.CurrentDirectory, "Skyray.RoHS.exe");
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(exePath);
            try
            {
                Skyray.EDXRFLibrary.SpecHelper.SmoothTimes = int.Parse(cfa.AppSettings.Settings["SmoothTimes"].Value);
            }
            catch { };
            foreach (var temp in listCuve)
            {
                if (temp.ElementList != null && temp.ElementList.Items.Count > 0)
                {
                    StandSamplesIntensity(temp, files);
                }
                temp.Save();
            }
        }

        private void StandSamplesIntensity(WorkCurve curve,List<FileInfo> files)
        {
            if (curve.ElementList != null && curve.ElementList.Items.Count > 0)
            {
                foreach (CurveElement ttElement in curve.ElementList.Items.ToList())
                {
                    ttElement.Samples.ToList().ForEach(w => {
                        FileInfo tt = files.Find(t => Path.GetFileNameWithoutExtension(t.Name) == w.SampleName);
                        if (tt != null)
                        {
                            curve.CaculateIntensity(LoadSpecFactory(tt.FullName));
                            w.X = ttElement.Intensity.ToString();
                        }
                    });
                }
            }
        }

        public void LoadDeviceFactory(string directoryDev, string curvedir)
        {
            if (directoryDev.IsNullOrEmpty()||!Directory.Exists(directoryDev))
                return;
            try
            {
                DirectoryInfo sampleDir = new DirectoryInfo(Environment.CurrentDirectory + "\\Image\\SampleImage\\EDXR");
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
            List<Condition> conditionList = new List<Condition>();
            string defaultDevice = readFile.GetDefaultDeviceName(dir.Parent.FullName + "\\Parameter\\Parameter.ini");
            double FixGaussDelta = readFile.GetFixGaussDelta(dir.Parent.FullName + "\\Parameter\\Parameter.ini");
            int defaultStandard = readFile.GetDefaultStands(dir.Parent.FullName + "\\Parameter\\Parameter.ini");
            readFile.LoadCurrentCurves(dir.Parent.FullName + "\\Parameter\\CurrentCurves.xml");
            List<CustomStandard> standareds = CustomStandard.FindAll();
            if (standareds != null && standareds.Count > defaultStandard)
            {
                standareds[defaultStandard].CurrentSatadard = true;
                standareds[defaultStandard].Save();
            }
            int smoothsTimes = readFile.GetDefaultSmoothsTimes(dir.Parent.FullName + "\\Parameter\\Parameter.ini");
            string exePath = System.IO.Path.Combine(
      Environment.CurrentDirectory, "Skyray.RoHS.exe");
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(exePath);
            cfa.AppSettings.Settings["SmoothTimes"].Value = smoothsTimes.ToString();
            cfa.Save();
            readFile.SaveParamsIniToXml(AppDomain.CurrentDomain.BaseDirectory + "Parameter.xml", dir.Parent.FullName + "\\Parameter\\Parameter.ini");

           DirectoryInfo info= deviceInfo.First(w => w.Name == defaultDevice);
            //foreach (DirectoryInfo info in deviceInfo)
            //{
           Device device = Device.New.Init("", "", DllType.DLL4, 1, 1, "", SpecLength.Normal, 1, 0, 2, 120, 120, 120, 120, 120, 120, 50, 600, 3, 120, 0, 0, "x");
                FPGAParams fpgaParams = FPGAParams.New.Init(OFFON.ON, OFFON.ON, OFFON.ON, 0, 0, 0, 0, 0, 0, "192.168.3.7", 100000,1);
                device.FPGAParams = fpgaParams;
                readFile.ReadFileDeviceInfo(info.FullName + "\\Device.xml", device);
                readFile.ReadFileCollimator(info.FullName + "\\CollMotor.xml", device);
                readFile.ReadFileFilter(info.FullName + "\\FilterMotor.xml", device);
                readFile.ReadFileRaytube(info.FullName + "\\RayTub.xml", device);
                device.Name = info.Name;
                device.DeviceID = "1111";
                //if (device.Name.Equals(defaultDevice))
                device.IsDefaultDevice = true;
                device.Detector.FixGaussDelta = FixGaussDelta;
                device.Save();
                LoadCurveFactory(curvedir, device, info.FullName + "\\InitParam.xml", info.FullName + "\\DemarcateEnergy.xml");
            //}
           
        }



        private void LoadCurveFactory(string directoryDev, Device device, string initPath, string EnergyPath)
        {
            if (directoryDev.IsNullOrEmpty()||!Directory.Exists(directoryDev))
                return;
            DirectoryInfo dir = new DirectoryInfo(directoryDev);
            DirectoryInfo[] areaInfo = dir.GetDirectories();
            if (areaInfo == null || areaInfo.Length == 0)
                return;
            Condition condition = null;
            foreach (DirectoryInfo info in areaInfo)
            {
                List<Skyray.EDXRFLibrary.DemarcateEnergy> energyPath = GetDemarcateEnergy(EnergyPath, device.SpecLength);
                switch (info.Name)
                {
                    case "Cr,Cl in Plastic":
                        condition = readFile.ReadDeviceCondition(initPath, info.FullName + "\\DeviceParam.xml", device, "Plastic", energyPath);
                        if (condition != null)
                            readFile.CreateCurve(info.FullName, WorkRegion.FindOne(w => w.Name == "Cr,Cl in Plastic"), condition);
                        break;
                    case "CrCdPbHg in Brass,Zinc":
                        condition = readFile.ReadDeviceCondition(initPath, info.FullName + "\\DeviceParam.xml", device, "BrassZinc", energyPath);
                        if (condition != null)
                            readFile.CreateCurve(info.FullName, WorkRegion.FindOne(w => w.Name == "CrCdPbHg in Brass,Zinc"), condition);
                        break;
                    case "CrCdPbHg in Magnalium":
                        condition = readFile.ReadDeviceCondition(initPath, info.FullName + "\\DeviceParam.xml", device, "Magnalium", energyPath);
                        if (condition != null)
                            readFile.CreateCurve(info.FullName, WorkRegion.FindOne(w => w.Name == "CrCdPbHg in Magnalium"), condition);
                        break;
                    case "CrCdPbHg in Solder":
                        condition = readFile.ReadDeviceCondition(initPath, info.FullName + "\\DeviceParam.xml", device, "Solder", energyPath);
                        if (condition != null)
                            readFile.CreateCurve(info.FullName, WorkRegion.FindOne(w => w.Name == "CrCdPbHg in Solder"), condition);
                        break;
                    case "CrCdPbHg in Steel":
                        condition = readFile.ReadDeviceCondition(initPath, info.FullName + "\\DeviceParam.xml", device, "Steel", energyPath);
                        if (condition != null)
                            readFile.CreateCurve(info.FullName, WorkRegion.FindOne(w => w.Name == "CrCdPbHg in Steel"), condition);
                        break;
                    case "Polyethylene":
                        condition = readFile.ReadDeviceCondition(initPath, info.FullName + "\\DeviceParam.xml", device, "Polyethylene", energyPath);
                        if (condition != null)
                            readFile.CreateCurve(info.FullName, WorkRegion.FindOne(w => w.Name == "Polyethylene"), condition);
                        break;
                    default: break;
                }
            }

            Condition condi = Condition.FindOne(w => w.Type == ConditionType.Match && w.Device.Id == device.Id);
            if (condi == null)
            {
                
                condi = Condition.New.Init(Info.Match);
                condi.Type = ConditionType.Match;
                condi.Device = device;
                if (condition != null)
                    condi.InitParam = InitParameter.New.Init(condition.InitParam.TubVoltage, condition.InitParam.TubCurrent, condition.InitParam.Gain, condition.InitParam.FineGain, condition.InitParam.ActGain, condition.InitParam.ActFineGain, condition.InitParam.Channel, condition.InitParam.ChannelError, condition.InitParam.ElemName, condition.InitParam.Filter, condition.InitParam.Collimator, condition.InitParam.Target, condition.InitParam.TargetMode, condition.InitParam.CurrentRate,condition.InitParam.ExpressionFineGain,condition.InitParam.InitCalibrateRatio);//加载默认值
                else
                    condi.InitParam = Default.GetInitParameter(device.SpecLength);
                condi.DeviceParamList.Add(Default.GetDeviceParameter(device.SpecLength, 1));
                List<Skyray.EDXRFLibrary.DemarcateEnergy> listDemarcate = GetDemarcateEnergy(EnergyPath, device.SpecLength);
                if (listDemarcate != null && listDemarcate.Count > 0)
                    listDemarcate.ForEach(wc => { condi.DemarcateEnergys.Add(wc); });
                condi.Save();
            }
          
        }

        private List<DemarcateEnergy> GetDemarcateEnergy(string path,SpecLength specLength)
        {
            List<Skyray.EDXRFLibrary.DemarcateEnergy> listDemarcate = readFile.ReadDeviceDemarcatEnergy(path);
            if (listDemarcate == null || listDemarcate.Count == 0)
            {
                listDemarcate.Add(Default.GetDemarcateEnergyAg(specLength));
                listDemarcate.Add(Default.GetDemarcateEnergyCu(specLength));
            }
            return listDemarcate;
        }

        public void LoadCurveFactory(string directoryDev, List<Skyray.EDXRFLibrary.Condition> conditions)
        {

        }

        public SpecListEntity LoadSpecFactory(string fileName)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                FileInfo info = new FileInfo(fileName);
                xmlDoc.Load(fileName);
                DeviceParameterEntity deviceParams = new DeviceParameterEntity();
                string deviceName = ReadHelper.LoadDeviceInfo(xmlDoc, deviceParams);
                string name = info.Name.Replace(".Spe", "");
                SpecListEntity specList = new SpecListEntity();
                SpecEntity spec = new SpecEntity();
                ReadHelper.LoadSampleInfo(xmlDoc, specList, spec);
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
                spec.Name = name;
                spec.IsSmooth = true;
                specList.Name = name;
                specList.SpecType = SpecType.StandSpec;
                spec.RemarkInfo = "";
                Device device = Device.FindOne(w=>w.IsDefaultDevice == true);
                if (device != null && device.Conditions.Count > 0)
                    specList.DemarcateEnergys = Default.ConvertFormOldToNew(device.Conditions[0].DemarcateEnergys, device.SpecLength);
                else
                {
                    if (WorkCurveHelper.DeviceCurrent==null)
                    {
                        Msg.Show(Info.NotFindDevice);
                        return null;
                    }
                    device = WorkCurveHelper.DeviceCurrent;
                    List<DemarcateEnergy> listDemarcate = new List<DemarcateEnergy>();
                    listDemarcate.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                    listDemarcate.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                    specList.DemarcateEnergys = Default.ConvertFormOldToNew(listDemarcate, device.SpecLength);
                }
                spec.DeviceParameter = deviceParams;
                specList.InitParam = Default.GetInitParameter(device.SpecLength).ConvertToNewEntity();
                specList.Color = Color.Blue.ToArgb();
                specList.VirtualColor = Color.Blue.ToArgb();
                specList.Specs = new SpecEntity[1];
                specList.Specs[0] = spec;
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    FileInfo[] jpgInfo = file.Directory.GetFiles(specList.Name + ".jpeg");
                    if (jpgInfo != null && jpgInfo.Length > 0)
                        jpgInfo[0].CopyTo(WorkCurveHelper.SaveSamplePath, true);
                }
                return specList;
            }
            catch { }
            return null;
        }
        
        #endregion
    }
}
