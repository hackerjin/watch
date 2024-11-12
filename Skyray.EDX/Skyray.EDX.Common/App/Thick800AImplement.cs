using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.Component;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using Skyray.EDX.Common.DelphiConvert;
using Skyray.EDX.Common.Library;

namespace Skyray.EDX.Common.App
{
    public class Thick800AImplement:IFactory
    {
        public Thick800A readFile = new Thick800A();
        private string devicePath;
        #region IFactory 成员

        public void LoadDeviceFactory(string directoryDev, string curvedir)
        {
            if (directoryDev.IsNullOrEmpty())
                return;
            if (!Directory.Exists(directoryDev))
                Directory.CreateDirectory(directoryDev);
            DirectoryInfo dir = new DirectoryInfo(directoryDev);
            if (dir == null)
                return;
            devicePath = dir.FullName;
            FileInfo[] infoes = dir.GetFiles(@"*MotorParameter.ini");
            Device.DeleteAll();
            bool flag = false;
            foreach (FileInfo info in infoes)
            {
                if ((Device.FindOne(w => w.Name == info.Name) != null))
                    continue;
                Device device = Device.New.Init("", "", DllType.DLL4, 1, 1, "", SpecLength.Normal, 1, 0, 2, 120, 120, 120, 120, 120, 120, 50, 600, 3, 120, 0, 0, "x");
                readFile.ReadFileCollimator(info.FullName, device);
                readFile.ReadFileFilter(info.FullName,device);
                readFile.ReadFileRaytube(info.FullName + "\\RayTub.xml", device);
                readFile.ReadFileDetector(info.FullName + "\\Device.xml", device);
                readFile.IsExistMoveAis(info.FullName, device);

                device.Name = info.Name.Substring(0,info.Name.IndexOf(".")) ;
                device.DeviceID = "";
                device.SpecLength = SpecLength.Normal;
                device.VoltageScaleFactor = 1;
                device.CurrentScaleFactor = 1;

                List<Condition> conditionList = readFile.ReadFileCondition("");
                device.Conditions.Add(conditionList[0]);
                if (!flag)
                    device.IsDefaultDevice = true;
                device.Save();
                DirectoryInfo curveInfo = new DirectoryInfo(curvedir);
                FileInfo[] curveFiles = curveInfo.GetFiles("*.xml");
                if (curveFiles == null || curveFiles.Length == 0)
                    return;
                foreach (FileInfo tempCurve in curveFiles)
                {
                    LoadCurveFactory(device, tempCurve);
                }
                flag = true;
            }
        }
        private ImputData data = new ImputData();

        private void LoadCurveFactory(Device device, FileInfo curveFile)
        {
            //构造曲线从xml文件中
            data.CurveImport(curveFile.FullName, device, true, true, string.Empty, string.Empty);
        }
        public void LoadCurveFactory(string directoryDev, List<Skyray.EDXRFLibrary.Condition> conditions)
        {
            //data.CurveImport(curveFile.FullName, device, true, true, string.Empty, string.Empty);
            //if (directoryDev.IsNullOrEmpty())
            //    return;
            //if (!Directory.Exists(directoryDev))
            //    Directory.CreateDirectory(directoryDev);
            //DirectoryInfo dir = new DirectoryInfo(directoryDev);

            //DirectoryInfo[] curveInfo = dir.GetDirectories();

            //if (curveInfo == null || curveInfo.Length == 0)
            //    return;
            //foreach (DirectoryInfo info in curveInfo)
            //{
            //    WorkCurve workCurve = WorkCurve.New;
            //    workCurve.Name = info.Name;
            //    //DirectoryInfo[] samplesDir = info.GetDirectories("Standard Sample Spectrum");
            //    //List<FileInfo> curveSampleFiles = new List<FileInfo>();
            //    //FileInfo[] files = null;
            //    //if (samplesDir != null && samplesDir.Length > 0)
            //    //      files = samplesDir[0].GetFiles("*.Spe");
            //    //if (files != null && files.Length>0)
            //    //    curveSampleFiles = files.ToList(); ;
            //    readFile.CreateCurve(workCurve, null, conditions);
            //    ElementList elementList = readFile.GetIntestedElementList(info.FullName + "\\ElemData", info.FullName);
            //    workCurve.ElementList = elementList;
            //    if (workCurve.ElementList != null && workCurve.ElementList.Items != null && workCurve.ElementList.Items.Count > 0)
            //    {
            //        List<CurveElement> curveList = workCurve.ElementList.Items.OrderByDescending(w => w.LayerNumber).ToList();
            //        foreach (CurveElement curveElement in workCurve.ElementList.Items)
            //        {
            //            ReadSystemFile(devicePath + "\\parameter.ini", curveElement);
            //            curveElement.DevParamId = workCurve.Condition.DeviceParamList[0].Id;
            //            curveElement.CalculationWay = GetCaculationWay(info.FullName + "\\WorkCurveParam.ini");
            //            workCurve.ElementList.ThCalculationWay = (curveElement.CalculationWay == CalculationWay.Linear ? ThCalculationWay.ThLinear : ThCalculationWay.ThInsert);
            //            if (String.Compare(curveElement.Caption, curveList[0].Caption, true) != 0)
            //            {
            //                readFile.ReadElementsSample(info.FullName + "\\RecordData", curveElement);
            //            }
            //            //if (curveSampleFiles.Count == 0)
            //            //    continue;
            //            CheckFitElement(curveElement, workCurve);
            //            //curveElement.Samples.ToList().ForEach(w =>
            //            //{
            //            //    FileInfo tt = curveSampleFiles.Find(t => t.Name == w.SampleName);
            //            //    if (tt != null)
            //            //    {
            //            //        workCurve.CaculateIntensity(LoadSpecFactory(tt.FullName));
            //            //        w.X = curveElement.Intensity.ToString();
            //            //    }
            //            //});
            //        }
            //    }
            //    workCurve.Save();
            //}
        }

        private void CheckFitElement(CurveElement ce, WorkCurve workCurve)
        {
            if (ce.IntensityWay == IntensityWay.Reference || ce.IntensityWay == IntensityWay.FixedReference)
            {
                ce.References.Clear();
                ce.SReferenceElements=string.Empty;
                foreach (var element in workCurve.ElementList.Items)
                {
                    ReferenceElement re = ReferenceElement.New.Init(ce.Caption, element.Caption, element.PeakLow, element.PeakHigh, element.BaseLow, element.BaseHigh, element.PeakDivBase);
                    ce.References.Add(re);
                    ce.SReferenceElements += element.Caption + ",";
                }
                ce.SReferenceElements = ce.SReferenceElements.Substring(0, ce.SReferenceElements.Length - 1);
                ce.Save();
                //string[] refElements = ce.ReferenceElements;
                //foreach (var refe in refElements)
                //{
                //    if (ce.References.ToList().Find(r => r.ReferenceElementName == refe) == null)
                //    {
                //        var findElem = workCurve.ElementList.Items.ToList().Find(e => e.Caption == refe);
                //        if (findElem == null) continue;

                //        ReferenceElement re = ReferenceElement.New.Init(ce.Caption, refe, findElem.PeakLow,findElem.PeakHigh, findElem.BaseLow, findElem.BaseHigh, findElem.PeakDivBase);

                //        ce.References.Add(re);

                //        ce.Save();
                //    }
                //}
            }
        }

        public CalculationWay GetCaculationWay(string filePath)
        {
            CalculationWay way = CalculationWay.Insert;
            if (!System.IO.File.Exists(filePath))
            {
                return way;
            }
            StringBuilder temp = new StringBuilder(255);
            int size = 255;
            string caption = "WorkCurve";
            TxtFile.GetPrivateProfileString(caption, "YWay", "", temp, size, filePath);
            if (!temp.ToString().IsNullOrEmpty())
            {
                int flag = Convert.ToInt32(temp.ToString());
                switch (flag)
                {

                    case 0:
                        way = CalculationWay.Insert;
                        break;
                    case 1:
                        way = CalculationWay.Linear;
                        break;
                    default:
                        break;
                }
            }
            return way;
        }

        public void ReadSystemFile(string filePath,CurveElement elements)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            StringBuilder temp = new StringBuilder(255);
            int size = 255;
            string caption = "System";
            TxtFile.GetPrivateProfileString(caption, "ThickUnit", "", temp, size, filePath);
            elements.ThicknessUnit = (Convert.ToInt32(temp.ToString()) == 1 ? ThicknessUnit.ur : ThicknessUnit.um);

            TxtFile.GetPrivateProfileString(caption, "RemoveGround", "", temp, size, filePath);
            elements.IntensityWay = (Convert.ToInt32(temp.ToString()) == 1 ? IntensityWay.FixedReference : IntensityWay.Reference);
        }


        public void RepeatCaculateSampleIntensity(string dataPath)
        {

        }


        public SpecListEntity LoadSpecFactory(string fileName)
        {
            SpecListEntity entity = new SpecListEntity();
            if(DephiHelper.GetDeviceParamsFromSpec(new FileInfo(fileName), entity))
                return entity;
            return null;
        }
        #endregion
    }
}
