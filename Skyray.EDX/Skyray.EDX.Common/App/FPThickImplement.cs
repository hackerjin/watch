using System.Collections.Generic;
using System.Linq;
using System.IO;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System;
using System.Xml;
using System.Drawing;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using Skyray.EDX.Common.CSharpConvert;
using Skyray.EDX.Common.Library;

namespace Skyray.EDX.Common.App
{
    public class FPThickImplement : IFactory
    {
        public FPThickReadFile readFile = new FPThickReadFile();

        #region IFactory 成员
        public void LoadDeviceFactory(string directoryDev, string curvedir)
        {
            if (directoryDev.IsNullOrEmpty()||!Directory.Exists(directoryDev))
                return;
            DirectoryInfo dir = new DirectoryInfo(directoryDev);
            if (dir == null)
                return;
            try
            {
                DirectoryInfo sampleDir = new DirectoryInfo(Environment.CurrentDirectory + "\\Image\\SampleImage\\EDXT");
                if (sampleDir.Exists)
                    sampleDir.Delete(true);
                sampleDir.Create();
            }
            catch { }
            DirectoryInfo[] deviceInfo = dir.GetDirectories();
            if (deviceInfo == null || deviceInfo.Length == 0)
                return;
            string defaultDevice = readFile.GetDefaultDeviceName(dir.Parent.FullName + "\\Data\\Parameter.conf");
            int deviceCount = 0;
            foreach (DirectoryInfo info in deviceInfo)
            {
                if (info.Name == ".svn")
                    continue;
                Device device = Device.New.Init("", "", DllType.DLL3, 1, 1, "", SpecLength.Normal, 1, 0, 2, 120, 120, 120, 120, 120, 120, 50, 600, 3, 120,0,0,"x");
                FPGAParams fpgaParams = FPGAParams.New.Init(OFFON.ON, OFFON.ON, OFFON.ON, 0, 0, 0, 0, 0, 0, "192.168.3.7", 100000,1);
                device.FPGAParams = fpgaParams;
                device.IsDP5 = false;
                device.Dp5Version = Dp5Version.Dp5_CommonUsb;
                readFile.ReadFileDeviceInfo(info.FullName + "\\Device.conf", device);

                readFile.ReadFileCollimator(info.FullName + "\\CollMotor.conf", device);

                readFile.ReadFileFilter(info.FullName + "\\FilterMotor.conf", device);

                readFile.ReadFileRaytube(info.FullName + "\\RayTub.conf", device);

                readFile.GetXAxesMotor(info.FullName + "\\XAxesMotor.conf", device);

                readFile.GetYAxesMotor(info.FullName + "\\YAxesMotor.conf", device);

                readFile.GetZAxesMotor(info.FullName + "\\ZAxesMotor.conf", device);
                device.Name = info.Name;
                device.FPGAParams = FPGAParams.New;
                //修改：何晓明 20111124 IP不能为空
                device.FPGAParams.IP = "192.168.3.97";
                //
                device.DeviceID = "1111";
                List<Condition> tempDeviceCondition = readFile.ReadFileCondition(info.FullName + "\\Condition.conf", device);
                if (tempDeviceCondition == null)
                    continue;
                if (device.Name.Equals(defaultDevice))
                {
                    device.IsDefaultDevice = true;
                }
                tempDeviceCondition.ToList().ForEach(w => device.Conditions.Add(w));
                device.Save();
                deviceCount++;
            }
            Device deviceDefault = Device.FindOne(w => w.IsDefaultDevice == true);
            if (deviceDefault != null && deviceDefault.Conditions.Count > 0)
                LoadCurveFactory(curvedir, deviceDefault.Conditions.ToList());
            else if (deviceCount > 0)
                LoadCurveFactory(curvedir, Device.FindAll()[0].Conditions.ToList());
          
        }

        public void LoadCurveFactory(string directoryDev, List<Condition> listCondition)
        {
            if (directoryDev.IsNullOrEmpty())
                return;
            try
            {
                if (!Directory.Exists(directoryDev))
                    Directory.CreateDirectory(directoryDev);
                DirectoryInfo dir = new DirectoryInfo(directoryDev);

                DirectoryInfo[] curveInfo = dir.GetDirectories();

                if (curveInfo == null || curveInfo.Length == 0)
                    return;

                foreach (DirectoryInfo info in curveInfo)
                {
                    Console.WriteLine(info.Name);
                    WorkCurve workCurve = WorkCurve.New;
                    workCurve.FuncType = FuncType.Thick;
                    workCurve.Name = info.Name;
                    readFile.CreateCurve(workCurve, info.FullName + "\\WorkCurve.conf", listCondition);
                    ElementList elementList = readFile.GetIntestedElementList(info.FullName + "\\ElemList.Data", info.FullName);
                    if (elementList == null)
                        return;
                    CurveElement ce = elementList.Items.OrderByDescending(l => l.LayerNumber).ToList()[0];
                    ce.LayerNumBackUp = Info.Substrate;
                    ce.LayerFlag = LayerFlag.Fixed;
                    workCurve.ElementList = elementList;
                    if (workCurve.ElementList.Items == null || workCurve.ElementList.Items.Count == 0)
                        return;
                    foreach (CurveElement curveElement in workCurve.ElementList.Items)
                    {
                        //curveElement.SReferenceElements = "";
                        foreach (var ele in workCurve.ElementList.Items)
                        {
                            curveElement.SReferenceElements += ele.Caption + ",";
                        }
                        if (curveElement.SReferenceElements != "")
                        {
                            curveElement.SReferenceElements.TrimEnd(',');
                        }
                        CheckFitElement(curveElement, workCurve);
                        readFile.ReadPureElementSample(info.FullName + "\\References", curveElement);//读纯元素谱
                        readFile.ReadElementsSample(info.FullName + "\\Standards", curveElement);//读标样谱
                        curveElement.DevParamId = workCurve.Condition.DeviceParamList[0].Id;
                    }
                    workCurve.Save();
                }
            }
            catch { }
        }

        private void CheckFitElement(CurveElement ce, WorkCurve workCurve)
        {
            if (ce.IntensityWay == IntensityWay.Reference || ce.IntensityWay == IntensityWay.FixedReference)
            {
                string[] refElements = ce.ReferenceElements;
                foreach (var refe in refElements)
                {
                    if (ce.References.ToList().Find(r => r.ReferenceElementName == refe) == null)
                    {
                        var findElem = workCurve.ElementList.Items.ToList().Find(e => e.Caption == refe);
                        if (findElem == null) continue;

                        ReferenceElement re = ReferenceElement.New.Init(ce.Caption, refe,50, 2000, findElem.BaseLow, findElem.BaseHigh, findElem.PeakDivBase);

                        ce.References.Add(re);

                        ce.Save();
                    }
                }
            }
        }

        #endregion


        public int[] changeSpecData(int[] data, float coeff)
        {
            coeff = Math.Abs(coeff);
            if (coeff == 1)
                return data;

            int[] newData = (int[])data.Clone();

            //压缩
            if (coeff > 1)
            {
                int splitIndex = (int)(data.Length / coeff);
                for (int i = 0; i < data.Length; i++)
                {
                    if (i < splitIndex)
                    {
                        int lowIndex = (int)(i * coeff);
                        int highIndex = lowIndex + 1;
                        newData[i] = (int)(data[lowIndex] + (highIndex < data.Length ? (data[highIndex] - data[lowIndex]) * (i * coeff - lowIndex) : 0));

                    }
                    else
                        newData[i] = 0;
                }
            }
            //拉伸
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    int lowIndex = (int)(i * coeff);
                    int highIndex = lowIndex + 1;
                    newData[i] = (int)(data[lowIndex] + (highIndex < data.Length ? (data[highIndex] - data[lowIndex]) * (i * coeff - lowIndex) : 0));
                }
            }

            return newData;
        }

        public string changeData(string value)
        {
            int[] intSpec = Helper.ToInts(value);
            intSpec = changeSpecData(intSpec, 1.1f);
            string[] strs = Array.ConvertAll(intSpec, element => element.ToString());
            string specData = string.Join(",", strs);
            return specData;
        }


        #region IFactory 成员
        public SpecListEntity LoadSpecFactory(string fileName)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                FileInfo info = new FileInfo(fileName);
                DeviceParameterEntity deviceParams = new DeviceParameterEntity();
                string deviceName = ReadHelper.LoadDeviceInfo(xmlDoc, deviceParams);
                Device deviceDefault = Device.FindOne(w => w.IsDefaultDevice == true);
                if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition != null & WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count > 0)
                {
                    deviceParams.Name = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].Name;
                    deviceDefault = WorkCurveHelper.DeviceCurrent;
                }
                else
                    deviceParams.Name = deviceDefault.Conditions[0].DeviceParamList[0].Name;
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
                spec.SpecData =str;
           
                spec.TubCurrent = deviceParams.TubCurrent;
                spec.TubVoltage = deviceParams.TubVoltage;
                spec.Name = info.Name.Replace(".spe", "");
                specList.Name = info.Name.Replace(".spe", "");
                specList.SpecType = SpecType.StandSpec;
                spec.RemarkInfo = "";
                specList.DemarcateEnergys = Default.ConvertFormOldToNew(new List<DemarcateEnergy>(), deviceDefault.SpecLength);
                spec.DeviceParameter = deviceParams;
                specList.Color = Color.Blue.ToArgb();
                specList.InitParam = Default.GetInitParameter(deviceDefault.SpecLength).ConvertToNewEntity();
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

        public void RepeatCaculateSampleIntensity(string dataPath)
        {
        }
        #endregion
    }
}
