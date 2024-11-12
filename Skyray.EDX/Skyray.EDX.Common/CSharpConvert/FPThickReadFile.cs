using System;
using System.Collections.Generic;
using Skyray.EDXRFLibrary;
using System.Xml;
using System.IO;
using System.Drawing;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using Skyray.EDX.Common.Library;
using System.Data;

namespace Skyray.EDX.Common.CSharpConvert
{
    public class FPThickReadFile : IDevice, ISpectrums, IWorkCurve
    {
        #region IDevice Members 设备信息

        public string GetDefaultDeviceName(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return "";
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode rootNode = doc.SelectSingleNode("System");
            XmlNode node = rootNode.SelectSingleNode("DeviceName");
            return node.InnerText;
        }

        public void ReadFileDeviceInfo(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode rootNode = doc.SelectSingleNode("Device");
            XmlNode node = rootNode.SelectSingleNode("DllType");

            //修改：何晓明 20111124 DLLType 字符串不符合枚举类型
            //if (node.InnerText.Length > 4)
            //    device.PortType = (DllType)Enum.Parse(typeof(DllType), node.InnerText.ToString().Substring(0,4));
            //else if(node.InnerText.Length==4)
            //    device.PortType = (DllType)Enum.Parse(typeof(DllType), node.InnerText.ToString());
            //
            if (!string.IsNullOrEmpty(node.InnerText.ToString()))
            {
                string[] str = node.InnerText.ToString().Split('_');
                if (str.Length>0)
                    device.PortType = (DllType)Enum.Parse(typeof(DllType), str[0],true);
                if (str.Length>1)
                    device.UsbVersion = (UsbVersion)Enum.Parse(typeof(UsbVersion), str[1],true);
                if (str.Length > 2)
                    device.IsPassward = (str[2] == "0" ? true : false);
            }
            node = rootNode.SelectSingleNode("PumpExist");
            device.HasVacuumPump = Convert.ToBoolean(node.InnerText);
            node = rootNode.SelectSingleNode("FilterMotorExist");
            device.HasFilter = Convert.ToBoolean(node.InnerText);
            node = rootNode.SelectSingleNode("CollMotorExist");
            device.HasCollimator = Convert.ToBoolean(node.InnerText);
            node = rootNode.SelectSingleNode("XAxesMotorExist");
            device.HasMotorX = Convert.ToBoolean(node.InnerText);
            node = rootNode.SelectSingleNode("YAxesMotorExist");
            device.HasMotorY = Convert.ToBoolean(node.InnerText);
            node = rootNode.SelectSingleNode("ZAxesMotorExist");
            device.HasMotorZ = Convert.ToBoolean(node.InnerText);
            Detector detector = Detector.New.Init(Skyray.EDXRFLibrary.DetectorType.Si, 5.895, 170);
            node = rootNode.SelectSingleNode("Detector");
            XmlNode detectorNode = node.SelectSingleNode("DetectorType");

            //修改：何晓明 20111124 DP5设备大小写区分
            if (detectorNode.InnerText.ToUpper() == "DP5")
                detector.Type = (DetectorType)Enum.Parse(typeof(DetectorType), "Si");
            else
                detector.Type = (DetectorType)Enum.Parse(typeof(DetectorType), detectorNode.InnerText.ToString());
            //
            detectorNode = node.SelectSingleNode("Energy");
            detector.Energy = Convert.ToDouble(detectorNode.InnerText);
            detectorNode = node.SelectSingleNode("Fwhm");
            detector.Fwhm = Convert.ToDouble(detectorNode.InnerText);
            device.Detector = detector;
        }

        public void ReadFileCollimator(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode rootNode = doc.SelectSingleNode("Motor");
            XmlNode node = rootNode.SelectSingleNode("DirectionFlag");
            device.CollimatorElectricalDirect = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("ID");
            device.CollimatorElectricalCode = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("DefSpeed");
            if (node != null)
            {
                device.CollimatorSpeed = Convert.ToInt32(node.InnerText);
            }
            node = rootNode.SelectSingleNode("TargetCount");
            int targetCount = Convert.ToInt32(node.InnerText);
            if (targetCount > 0)
            {
                int[] Target = new int[targetCount];
                for (int i = 0; i < targetCount; ++i)
                {
                    Collimator filter = Collimator.New;
                    filter.Num = i;
                    node = rootNode.SelectSingleNode("Target" + i.ToString());
                    filter.Step = Convert.ToInt32(node.InnerText);
                    device.Collimators.Add(filter);
                }
            }
        }

        public void ReadFileFilter(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode rootNode = doc.SelectSingleNode("Motor");
            XmlNode node = rootNode.SelectSingleNode("DirectionFlag");
            device.FilterElectricalDirect = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("ID");
            device.FilterElectricalCode = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("DefSpeed");
            if (node != null)
            {
                device.FilterSpeed = Convert.ToInt32(node.InnerText);
            }
            node = rootNode.SelectSingleNode("TargetCount");
            int targetCount = Convert.ToInt32(node.InnerText);
            if (targetCount > 0)
            {
                int[] Target = new int[targetCount];
                for (int i = 0; i < targetCount; ++i)
                {
                    Filter filter = Filter.New;
                    filter.Num = i;
                    node = rootNode.SelectSingleNode("Target" + i.ToString());
                    filter.Step = Convert.ToInt32(node.InnerText);
                    device.Filter.Add(filter);
                }
            }
        }

        public void ReadFileDetector(string filePaht, Device device)
        {
            if (!System.IO.File.Exists(filePaht))
            {
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePaht);
            Detector detector = Detector.New;
            XmlNode xmlRootElem = xmlDoc.SelectSingleNode("Device");
            XmlNode xmlElem = xmlRootElem.SelectSingleNode("Detector");
            XmlNode xmlElemSub = xmlElem.SelectSingleNode("DetectorType");
            detector.Type = (DetectorType)Enum.Parse(typeof(DetectorType), xmlElemSub.InnerText);
            xmlElemSub = xmlElem.SelectSingleNode("Energy");
            detector.Energy = Convert.ToDouble(xmlElemSub.InnerText);
            xmlElemSub = xmlElem.SelectSingleNode("Fwhm");
            detector.Fwhm = Convert.ToDouble(xmlElemSub.InnerText);
            device.Detector = detector;
        }

        public void ReadFileChamber(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode rootNode = doc.SelectSingleNode("Motor");
            XmlNode node = rootNode.SelectSingleNode("DirectionFlag");
            device.ChamberElectricalDirect = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("ID");
            device.ChamberElectricalCode = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("DefSpeed");
            if (node != null)
            {
                device.ChamberSpeed = Convert.ToInt32(node.InnerText);
            }
            node = rootNode.SelectSingleNode("TargetCount");
            int targetCount = Convert.ToInt32(node.InnerText);
            if (targetCount > 0)
            {
                int[] Target = new int[targetCount];
                for (int i = 0; i < targetCount; ++i)
                {
                    Chamber filter = Chamber.New;
                    filter.Num = i;
                    node = rootNode.SelectSingleNode("Target" + i.ToString());
                    filter.Step = Convert.ToInt32(node.InnerText);
                    filter.StepCoef1 = 0;
                    filter.StepCoef2 = 0;
                    device.Chamber.Add(filter);
                }
            }
        }

        public void ReadFileRaytube(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            Tubes tubes = Tubes.New.Init(74, 19, 1.9, "SiO2", 40, 35, 14);
            XmlNode xmlRootElem = xmlDoc.SelectSingleNode("XRayTub");
            XmlNode xmlElem = xmlRootElem.SelectSingleNode("TargetAtomicNumber");
            tubes.AtomNum = Convert.ToInt32(xmlElem.InnerText);
            xmlElem = xmlRootElem.SelectSingleNode("TargetTakeOffAngle");
            tubes.Angel = Convert.ToInt32(xmlElem.InnerText);
            xmlElem = xmlRootElem.SelectSingleNode("WindowThickness");
            tubes.Thickness = Convert.ToDouble(xmlElem.InnerText);

            xmlElem = xmlRootElem.SelectSingleNode("WindowFormula");
            tubes.Material = xmlElem.InnerText;

            xmlElem = xmlRootElem.SelectSingleNode("IncidentAngle");
            tubes.Incident = int.Parse(Math.Round(Convert.ToDouble(xmlElem.InnerText)).ToString());
            xmlElem = xmlRootElem.SelectSingleNode("EmergentAngle");
            tubes.Exit = int.Parse(Math.Round(Convert.ToDouble(xmlElem.InnerText)).ToString());
            device.Tubes = tubes;
        }

        public void IsExistMoveAis(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode rootNode = doc.SelectSingleNode("Motor");

            XmlNode node = rootNode.SelectSingleNode("Exist");
            bool exists = Convert.ToBoolean(node.InnerText);
            if (exists)
            {
                device.HasMotorZ = exists;
                node = rootNode.SelectSingleNode("ID");
                device.MotorZCode = Convert.ToInt32(node.InnerText);

                node = rootNode.SelectSingleNode("DirectionFlag");
                device.MotorZDirect = Convert.ToInt32(node.InnerText);

                node = rootNode.SelectSingleNode("MaxStep");
                device.MotorZMaxStep = Convert.ToInt32(node.InnerText);

                node = rootNode.SelectSingleNode("DefSpeed");
                if (node != null)
                {
                    device.MotorZSpeed = Convert.ToInt32(node.InnerText);
                }
            }
        }

        public void GetXAxesMotor(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode rootNode = doc.SelectSingleNode("OfXAxesMotor");

            XmlAttributeCollection Attributes = rootNode.Attributes;// SelectSingleNode("Exist");
            XmlNode node = Attributes.GetNamedItem("Exist");
            device.HasMotorX = Convert.ToBoolean(node.InnerText);

            node = Attributes.GetNamedItem("ID");
            device.MotorXCode = Convert.ToInt32(node.InnerText);

            node = Attributes.GetNamedItem("DirectionFlag");
            device.MotorXDirect = Convert.ToInt32(node.InnerText);

            node = Attributes.GetNamedItem("MaxStep");
            device.MotorXMaxStep = Convert.ToInt32(node.InnerText);

            node = Attributes.GetNamedItem("RepositionStep");
            if (node != null)
            {
                device.MotorXSpeed = Convert.ToInt32(node.InnerText);
            }

        }

        public void GetYAxesMotor(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode rootNode = doc.SelectSingleNode("OfYAxesMotor");

            XmlAttributeCollection Attributes = rootNode.Attributes;// SelectSingleNode("Exist");
            XmlNode node = Attributes.GetNamedItem("Exist");
            device.HasMotorY = Convert.ToBoolean(node.InnerText);

            node = Attributes.GetNamedItem("ID");
            device.MotorYCode = Convert.ToInt32(node.InnerText);

            node = Attributes.GetNamedItem("DirectionFlag");
            device.MotorYDirect = Convert.ToInt32(node.InnerText);

            node = Attributes.GetNamedItem("MaxStep");
            device.MotorYMaxStep = Convert.ToInt32(node.InnerText);

            node = Attributes.GetNamedItem("RepositionStep");
            if (node != null)
            {
                device.MotorYSpeed = Convert.ToInt32(node.InnerText);
            }
        }

        public void GetZAxesMotor(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode rootNode = doc.SelectSingleNode("OfZAxesMotor");

            XmlAttributeCollection Attributes = rootNode.Attributes;// SelectSingleNode("Exist");
            XmlNode node = Attributes.GetNamedItem("Exist");
            device.HasMotorZ = Convert.ToBoolean(node.InnerText);

            node = Attributes.GetNamedItem("ID");
            device.MotorZCode = Convert.ToInt32(node.InnerText);

            node = Attributes.GetNamedItem("DirectionFlag");
            device.MotorZDirect = Convert.ToInt32(node.InnerText);

            node = Attributes.GetNamedItem("MaxStep");
            device.MotorZMaxStep = Convert.ToInt32(node.InnerText);

            node = Attributes.GetNamedItem("RepositionStep");
            if (node != null)
            {
                device.MotorZSpeed = Convert.ToInt32(node.InnerText);
            }
        }

        #endregion

        #region ISpectrums Members 谱信息

        public void GetSpectrum(FileInfo filePath, SpecType type, SpecListEntity specList, List<Device> deviceList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath.FullName);
            DeviceParameterEntity deviceParams = new DeviceParameterEntity();
            string deviceName = ReadHelper.LoadDeviceInfo(xmlDoc, deviceParams);
            SpecEntity spec = new SpecEntity();
            ReadHelper.LoadSampleInfo(xmlDoc, specList, spec);
            //ReadHelper.LoadImage(xmlDoc, specList);
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
            spec.Name = filePath.Name;
            //spec.SpecOrignialData = spec.SpecData;
            specList.Name = filePath.Name;
            specList.SpecType = type;
            spec.RemarkInfo = "";
            if (deviceList == null || deviceList.Count == 0)
                return;
            
            //Device device = deviceList.Find(w => w.Name == deviceName);
            //if (device != null && device.Conditions != null && device.Conditions.Count > 0)
            //    specList.Condition = device.Conditions[0];
            //else
            //    specList.Condition = deviceList[0].Conditions[0];
            spec.DeviceParameter = deviceParams;
            specList.Specs = new SpecEntity[1];
            specList.Specs[0] = spec;
            //specList.Save();
        }

        public void GetSpectrum(FileInfo filePath, List<Device> deviceList, SpecType specType)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath.FullName);
            DeviceParameterEntity deviceParams = new DeviceParameterEntity();
            string deviceName = ReadHelper.LoadDeviceInfo(xmlDoc, deviceParams);
            Device deviceDefault = deviceList.Find(w => w.IsDefaultDevice == true);
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
            //spec.SpecOrignialData = spec.SpecData;
            spec.TubCurrent = deviceParams.TubCurrent;
            spec.TubVoltage = deviceParams.TubVoltage;
            spec.Name = filePath.Name.Replace(".spe", "");
            if ((int)specType == 0)
            { 
                specList.Name = filePath.Name.Replace(".spe", ""); 
            }
            else
            {
                specList.Name = filePath.Name.Replace(".spe", "") + "_" + (int)specType;
            }
            specList.SpecType = specType;
            spec.RemarkInfo = "";
            spec.DeviceParameter = deviceParams;
            specList.Specs = new SpecEntity[1];
            specList.Specs[0] = spec;
            specList.InitParam = deviceDefault.Conditions[0].InitParam.ConvertToNewEntity();
            specList.Height = 0;
            WorkCurveHelper.DataAccess.Save(specList);
            ReadHelper.LoadImage(xmlDoc, specList,3);
        }

        //public void GetSpectrum(FileInfo filePath, SpecType type, SpecList specList, List<Device> deviceList)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(filePath.FullName);
        //    DeviceParameter deviceParams = DeviceParameter.New;
        //    string deviceName = ReadHelper.LoadDeviceInfo(xmlDoc, deviceParams);
        //    Spec spec = Spec.New;
        //    ReadHelper.LoadSampleInfo(xmlDoc, specList, spec);
        //    ReadHelper.LoadImage(xmlDoc, specList);
        //    XmlNode xmlElemRoot = xmlDoc.SelectSingleNode("Spectrum");
        //    XmlNode xmlElem = xmlElemRoot.SelectSingleNode("Data");
        //    char[] sep = new char[] { ',' };
        //    string[] dataStr = xmlElem.InnerText.Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //    string str = string.Empty;
        //    for (int i = 0; i < dataStr.Length; ++i)
        //    {
        //        str += Convert.ToInt32(dataStr[i]) + ",";
        //    }
        //    spec.SpecData = str;
        //    spec.TubCurrent = deviceParams.TubCurrent;
        //    spec.TubVoltage = deviceParams.TubVoltage;
        //    spec.Name = filePath.Name;

        //    specList.Name = filePath.Name;
        //    specList.SpecType = type;
        //    spec.RemarkInfo = "";
        //    if (deviceList == null || deviceList.Count == 0)
        //        return;
        //    Device device = deviceList.Find(w => w.Name == deviceName);
        //    if (device != null && device.Conditions != null && device.Conditions.Count > 0)
        //        specList.Condition = device.Conditions[0];
        //    else
        //        specList.Condition = deviceList[0].Conditions[0];
        //    spec.DeviceParameter = specList.Condition.DeviceParamList[0];
        //    specList.Specs.Add(spec);
        //    //specList.Save();
        //}

        #endregion

        #region IWorkCurve Members 工作曲线信息

        public ElementList GetIntestedElementList(string filePath,string workCurvePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            XmlDocument xmlDoc = new XmlDocument();
            //int number;
            xmlDoc.Load(filePath);
            ElementList elementList = Default.GetElementList();
            elementList.MatchSpecListIdStr = "";
            XmlNodeList nodeChildList;
            XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
            int maxLayer = 1;
            foreach (XmlNode node in nodeList)
            {
                nodeChildList = node.ChildNodes;
                CalculationWay way = CalculationWay.Insert;
                switch (node.Name)
                {
                    case "UnitaryValue":
                        elementList.UnitaryValue = Convert.ToDouble(node.InnerText);
                        break;
                    case "CuvWay":
                        way = (CalculationWay)Enum.Parse(typeof(CalculationWay), node.InnerText);
                        if (way == CalculationWay.Insert)
                            elementList.ThCalculationWay = ThCalculationWay.ThInsert;
                        else
                            elementList.ThCalculationWay = ThCalculationWay.ThLinear;
                        break;
                    case "Element": //读取元素信息
                        #region
                        CurveElement element = CurveElement.New.Init("", true, "", 0, 0, "", XLine.Ka, 0, 0, 0, 0, false, 0, 0, 0, 0.00,
                            IntensityWay.Reference, false, 0, 0, 0, way, FpCalculationWay.LinearWithoutAnIntercept, ElementFlag.Calculated, LayerFlag.Calculated,
                                ContentUnit.per, ThicknessUnit.um, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", false, Color.Red.ToArgb(), " ", 0, "", false, true, true, false, 1, 0, "", "", "",false,"");
                        //number = Convert.ToInt32(node.Attributes[0].InnerText);
                        foreach (XmlNode childNode in nodeChildList)
                        {
                            switch (childNode.Name)
                            {
                                case "ID":
                                    element.AtomicNumber = Convert.ToInt32(childNode.InnerText);
                                    break;
                                case "Caption":
                                    element.Caption = childNode.InnerText;
                                    break;
                                case "Color":
                                    element.Color = Convert.ToInt32(childNode.InnerText);
                                    break;
                                case "BaseLeft":
                                    element.BaseLow = Convert.ToInt32(childNode.InnerText);
                                    break;
                                case "BaseRight":
                                    element.BaseHigh = Convert.ToInt32(childNode.InnerText);
                                    break;
                                case "PeakLeft":
                                    element.PeakLow = Convert.ToInt32(childNode.InnerText);
                                    break;
                                case "PeakRight":
                                    element.PeakHigh = Convert.ToInt32(childNode.InnerText);
                                    break;
                                case "PeakDivBase":
                                    element.PeakDivBase = Convert.ToBoolean(childNode.InnerText);
                                    break;
                                case "PeakAreaWay":
                                    element.IntensityWay = IntensityWay.Reference;//(AreaWay)Enum.Parse(typeof(AreaWay), childNode.InnerText);
                                    break;
                                case "Line":
                                    element.AnalyteLine = (XLine)Enum.Parse(typeof(XLine), childNode.InnerText);
                                    if (element.AnalyteLine == XLine.Kb)
                                        element.AnalyteLine = XLine.Ka;
                                    else if (element.AnalyteLine == XLine.La)
                                        element.AnalyteLine = XLine.Kb;
                                    else if (element.AnalyteLine == XLine.Lb)
                                        element.AnalyteLine = XLine.La;
                                    else if (element.AnalyteLine == XLine.Lr)
                                        element.AnalyteLine = XLine.Lb;
                                    break;
                                case "ReviseElem":
                                    element.SInfluenceElements = childNode.InnerText;
                                    break;
                                case "FitElem":
                                    element.SReferenceElements = childNode.InnerText;
                                    break;
                                case "Flag":
                                    element.LayerFlag = (LayerFlag)Enum.Parse(typeof(LayerFlag), childNode.InnerText);
                                    break;
                                case "LayerNumber":
                                    element.LayerNumber = Convert.ToInt32(childNode.InnerText);
                                    break;
                                case "CompoundFormula":
                                    element.Formula = childNode.InnerText;
                                    break;
                                case "ConditionCode":
                                    element.ConditionID = Convert.ToInt32(childNode.InnerText);
                                    break;
                            }
                        }
                        switch (element.LayerNumber)
                        {
                            case 1:
                                element.LayerNumBackUp = Info.FirstLayer; break;
                            case 2:
                                element.LayerNumBackUp = Info.SecondLayer; break;
                            case 3:
                                element.LayerNumBackUp = Info.ThirdLayer; break;
                            case 4:
                                element.LayerNumBackUp = Info.ForthLayer; break;
                            case 5:
                                element.LayerNumBackUp = Info.FifthLayer; break;
                            case 6:
                                element.LayerNumBackUp = Info.Substrate; break;
                        }
                        if (element.LayerNumber > maxLayer)
                        {
                            maxLayer = element.LayerNumber;
                        }
                        //element.SReferenceElements = element.Caption;
                        element.Formula = element.Formula == "" ? element.Caption : element.Formula;
                        element.Flag =  ElementFlag.Calculated;
                        element.Save();
                        elementList.Items.Add(element);
                        #endregion
                        break;
                }
            }
            foreach (CurveElement elem in elementList.Items)
            {
                if (elem.LayerNumber==maxLayer)
                {
                    elem.LayerNumBackUp = Info.Substrate;
                    elem.Save();
                }
            }
            return elementList;
        }

        public List<Skyray.EDXRFLibrary.Condition> ReadFileCondition(string filePath, Device Device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filePath);
            XmlNode xmlNode = xmldoc.SelectSingleNode("ConditionList");//测试条件
            List<Condition> conditionList = new List<Condition>();
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                Condition condition = Condition.New;
                DeviceParameter parameter = Default.GetDeviceParameter(Device.SpecLength, 1);
                InitParameter InitParam = Default.GetInitParameter(Device.SpecLength);
                XmlAttributeCollection attributes = node.Attributes;
                XmlNode innerNode = attributes.GetNamedItem("ID");
                condition.Name = innerNode.InnerText;
                parameter.Name = innerNode.InnerText;
                innerNode = attributes.GetNamedItem("PrecTime");
                parameter.PrecTime = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("Voltage");
                parameter.TubVoltage = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("Current");
                parameter.TubCurrent = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("FilterIndex");
                parameter.FilterIdx = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("ColliIndex");
                parameter.CollimatorIdx = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("IsPump");
                parameter.IsVacuum = Convert.ToBoolean(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("PumpTime");
                parameter.VacuumTime = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("IsAdjustCount");
                parameter.IsAdjustRate = Convert.ToBoolean(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("MaxCount");
                parameter.MaxRate = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("MinCount");
                parameter.MinRate = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("InitElem");
                InitParam.ElemName = innerNode.InnerText;
                innerNode = attributes.GetNamedItem("InitChann");
                InitParam.Channel = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("InitVoltage");
                InitParam.TubVoltage = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("InitCurrent");
                InitParam.TubCurrent = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("CoarseCode");
                InitParam.Gain = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("FineCode");
                InitParam.FineGain = int.Parse(innerNode.InnerText);
                innerNode = attributes.GetNamedItem("ChannError");
                InitParam.ChannelError = int.Parse(innerNode.InnerText);
                condition.InitParam = InitParam;
                condition.DeviceParamList.Add(parameter);
                condition.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg(Device.SpecLength));
                condition.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu(Device.SpecLength));
                //condition.Device = Device;
                //condition.Save();
                conditionList.Add(condition);
            }
            return conditionList;
        }

        public void ReadPureElementSample(string filePath, CurveElement elements)
        {
            if (filePath.IsNullOrEmpty())
                return;
            if (!Directory.Exists(filePath))
            {
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(filePath);
            FileInfo[] fileInfo = dir.GetFiles("*.spe");
            if (fileInfo == null || fileInfo.Length == 0)
                return;
            foreach (FileInfo info in fileInfo)
            {
                string simpleName = info.Name.Split('.')[0];
                if (!simpleName.Equals(elements.Caption)) continue;
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(info.FullName);
                XmlNode xmlNode = xmldoc.SelectSingleNode("Spectrum");
                XmlNode node = xmlNode.SelectSingleNode("Data");
                XmlNode DeviceParamNode = xmlNode.SelectSingleNode("DeviceParam");
                XmlNode timeNode = DeviceParamNode.SelectSingleNode("PrecTime");
                //Helper.Smooth(Helper.ToDoubles
                elements.SSpectrumData = Helper.TransforToDivTime(node.InnerText, int.Parse(timeNode.InnerText));

                elements.ElementSpecName = simpleName +"_" + elements.ElementList.WorkCurve.Name;
                //elements.Save();
                //SpecList specList = Default.GetSpecList();
                //specList.SpecType = SpecType.PureSpec;
                //specList.Condition = elements.ElementList.WorkCurve.Condition;
                //specList.WorkCurveId = elements.ElementList.WorkCurve.Id;
                //specList.Name = elements.ElementSpecName;
                //specList.SampleName = elements.ElementSpecName;
                //specList.VirtualColor = Color.Blue.ToArgb();
                //Spec spec = Spec.New;
                //spec.DeviceParameter = elements.ElementList.WorkCurve.Condition.DeviceParamList[0];
                //spec.Name = specList.Name;
                //spec.SpecData = node.InnerText;
                //spec.RemarkInfo = "";
                //spec.UsedTime = int.Parse(timeNode.InnerText);
                //specList.Specs.Add(spec);
                //specList.Save();
            }
        }

        public void ReadElementsSample(string filePath, CurveElement element)
        {
            if (filePath.IsNullOrEmpty())
                return;
            if (!Directory.Exists(filePath))
            {
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(filePath);
            FileInfo[] fileInfo = dir.GetFiles("*.dt");
            if (fileInfo == null || fileInfo.Length == 0)
                return;
            foreach (FileInfo info in fileInfo)
            {
                DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(info.Name));
                ds.ReadXml(info.FullName, XmlReadMode.ReadSchema);
               
                StandSample sample = StandSample.New;
                sample.Active = true;
                sample.Element = element;
                sample.SampleName = Path.GetFileNameWithoutExtension(info.Name);
                sample.ElementName = element.Caption;
                sample.Level = Info.MultiLayer;
                sample.TotalLayer = ds.Tables["tableLayer"].Rows.Count;
                foreach (DataRow node in ds.Tables["tableConstituent"].Rows)
                {
                    if (node["colConstituentFormula"].ToString().Equals(element.Caption))
                    {
                        sample.X = node["colAnalyteIntensity"].ToString();
                        sample.Y = node["colConstituentConcentration"].ToString();
                        element.ContentUnit = int.Parse(node["colConstituentConcentrationUnit"].ToString()) == 1 ? ContentUnit.per : ContentUnit.ppm;
                        sample.Layer = int.Parse(node["colConstituentLayerNumber"].ToString());
                        DataRow find = ds.Tables["tableLayer"].Rows[sample.Layer - 1];
                        sample.Z = find["colLayerThickness"].ToString();
                    }
                }
                element.Samples.Add(sample);
            }
        }

        //public void ReadPureElementSample(string filePath, CurveElement elements)
        //{
        //    if (!Directory.Exists(filePath))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        string[] stdPaths = Directory.GetFiles(filePath);
        //        System.Collections.ArrayList PureList = new System.Collections.ArrayList();
        //        try
        //        {
        //            foreach (string path in stdPaths)
        //            {
        //                DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(path));
        //                ds.ReadXml(path, XmlReadMode.ReadSchema);
        //                PureList.Add(ds);
        //            }
        //        }
        //        catch { }
        //        //elements.ElementSpecName = 
        //    }
        //}

        public Compounds ReadFileCompounds(string filePath)
        {
            throw new NotImplementedException();
        }

        //public void ReadElementsSample(string filePath, CurveElement elements)
        //{
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return;
        //    }
        //    ReadHelper.LoadElementSamples(filePath, elements);
        //}

        public List<DemarcateEnergy> GetConditionDemarcate(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            return ReadHelper.LoadDemarcateEnergy(filePath);
        }

        public QualeElement GetQualeElement(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            return ReadHelper.LoadQualeElement(filePath);
        }

        public void CreateCurve(WorkCurve curve, string filePath, List<Skyray.EDXRFLibrary.Condition> conditions)
        {
            if (curve == null || !System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode rootNode = doc.SelectSingleNode("WorkCurve");

            XmlNode node = rootNode.SelectSingleNode("Type");
            curve.CalcType = (node.InnerText == "FP" ? CalcType.FP : CalcType.EC);
            DirectoryInfo info = new DirectoryInfo(filePath);
            string deviceName = GetWorkcurveDeviceName(info.Parent.FullName + "\\DeviceParam.conf");
            node = rootNode.SelectSingleNode("ConditionID");
            curve.Condition = conditions.Find(w => w.Name == node.InnerText && w.Device.Name == deviceName.Trim());
            curve.Condition = curve.Condition == null ? conditions[0] : curve.Condition;
            curve.ConditionName = curve.Condition == null ? conditions[0].Name : curve.Condition.Name;
            curve.Save();
        }

        private string GetWorkcurveDeviceName(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return "";
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode rootNode = doc.SelectSingleNode("DeviceParameter");
            XmlNode node = rootNode.SelectSingleNode("DeviceName");
            return node.InnerText;
        }
        #endregion

        #region ISpectrums 成员

        void ISpectrums.GetSpectrum(FileInfo filePath, SpecType type, SpecListEntity specList, List<Device> deviceList)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IWorkCurve 成员


        public List<Condition> ReadFileCondition(string filePath)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
