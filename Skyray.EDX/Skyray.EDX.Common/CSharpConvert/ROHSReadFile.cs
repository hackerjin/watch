using System;
using System.Collections.Generic;
using Skyray.EDXRFLibrary;
using System.Xml;
using System.IO;
using System.Drawing;
using Skyray.EDX.Common;
using Skyray.API;
using System.Text;
using System.Linq;
using Lephone.Data.Common;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using Skyray.EDX.Common.Library;

namespace Skyray.EDX.Common.CSharpConvert
{
    public class ROHSReadFile : IDevice, ISpectrums, IWorkCurve
    {

        public string GetDefaultDeviceName(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            { return ""; }
            StringBuilder temp = new StringBuilder(255);
            string section = "System";
            int size = 255;
            WinMethod.GetPrivateProfileString(section, "DeviceName", "", temp, size, filePath);
            return temp.ToString();
        }

        public void SaveParamsIniToXml(string toXmlPath,string fromIni)
        {
            if (!System.IO.File.Exists(fromIni))
            { return; }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(toXmlPath);
            StringBuilder temp = new StringBuilder(255);
            string section = "System";
            int size = 255;
            WinMethod.GetPrivateProfileString(section, "PbCoeffA", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter/System/PbCoeffA").InnerText = temp.ToString();
            WinMethod.GetPrivateProfileString(section, "PbCoeffB", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter/System/PbCoeffB").InnerText = temp.ToString();
            WinMethod.GetPrivateProfileString(section, "CdCoeffA", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter/System/CdCoeffA").InnerText = temp.ToString();
            WinMethod.GetPrivateProfileString(section, "CdCoeffB", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter/System/CdCoeffB").InnerText = temp.ToString();
            WinMethod.GetPrivateProfileString(section, "PbLimit", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter/System/PbLimit").InnerText = temp.ToString();

            WinMethod.GetPrivateProfileString(section, "CdSnCoeff", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter/System/CdSnCoeff").InnerText = temp.ToString();

            WinMethod.GetPrivateProfileString(section, "PbabCoeff", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter/System/PbabCoeff").InnerText = temp.ToString();
            section = "Gauss";
            WinMethod.GetPrivateProfileString(section, "HalfWidth", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter/System/HalfWidth").InnerText = temp.ToString();
            WinMethod.GetPrivateProfileString(section, "PeakChannel", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter/System/PeakChannel").InnerText = temp.ToString();

            section = "Matter";
            WinMethod.GetPrivateProfileString(section, "BaseLeft", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter").Attributes["bLeft"].InnerText = temp.ToString();

            WinMethod.GetPrivateProfileString(section, "BaseRight", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter").Attributes["bRight"].InnerText = temp.ToString();

            WinMethod.GetPrivateProfileString(section, "ratio", "", temp, size, fromIni);
            xmlDoc.SelectSingleNode("Parameter").Attributes["TotalRatio"].InnerText = temp.ToString();

            XmlNodeList xmlList = xmlDoc.SelectNodes("Parameter/Element");
            foreach (XmlNode mode in xmlList)
            {
                int length = WinMethod.GetPrivateProfileString(section, mode.SelectSingleNode("ElementName").InnerText+"Left", "", temp, size, fromIni);
                if (length !=0)
                   mode.SelectSingleNode("Left").InnerText = temp.ToString();
                length = WinMethod.GetPrivateProfileString(section, mode.SelectSingleNode("ElementName").InnerText+"Right", "", temp, size, fromIni);
                  if (length !=0)
                   mode.SelectSingleNode("Right").InnerText = temp.ToString();
            }
            xmlDoc.Save(toXmlPath);
        }

        public int GetDefaultSmoothsTimes(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            { return 5; }
            StringBuilder temp = new StringBuilder(255);
            string section = "System";
            int size = 255;
            WinMethod.GetPrivateProfileString(section, "specSmoothTimes", "", temp, size, filePath);
            return int.Parse(string.IsNullOrEmpty(temp.ToString()) ? "5" : temp.ToString());
        }

        public int GetDefaultStands(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            { return 5; }
            StringBuilder temp = new StringBuilder(255);
            string section = "System";
            int size = 255;
            WinMethod.GetPrivateProfileString(section, "DefaultStandard", "", temp, size, filePath);
            return int.Parse(string.IsNullOrEmpty(temp.ToString()) ? "0" : temp.ToString());
        }

        public string GetDevicePath(string paramsPath)
        {
            if (!System.IO.File.Exists(paramsPath))
            { return null; }
            StringBuilder temp = new StringBuilder(255);
            string section = "System";
            int size = 255;
            int len = WinMethod.GetPrivateProfileString(section, "devicePath", "", temp, size, paramsPath);
            return len == 0 ? null : temp.ToString();
        }


        public double GetFixGaussDelta(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            { return 0; }
            double HalfWidth = 0;
            double PeakChannel = 1;
            StringBuilder temp = new StringBuilder(255);
            string section = "Gauss";
            int size = 255;
            WinMethod.GetPrivateProfileString(section, "HalfWidth", "", temp, size, filePath);
            HalfWidth = Convert.ToDouble(temp.ToString());
            WinMethod.GetPrivateProfileString(section, "PeakChannel", "", temp, size, filePath);
            PeakChannel = Convert.ToDouble(temp.ToString());
            return HalfWidth / PeakChannel;
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
            device.PortType = DllType.DLL4;
            XmlNode node = rootNode.SelectSingleNode("Name");
            device.Name = node.InnerText;
            node = rootNode.SelectSingleNode("PumpExist");
            device.HasVacuumPump = Convert.ToBoolean(node.InnerText);
            node = rootNode.SelectSingleNode("FilterMotorExist");
            device.HasFilter = Convert.ToBoolean(node.InnerText);
            node = rootNode.SelectSingleNode("CollMotorExist");
            device.HasCollimator = Convert.ToBoolean(node.InnerText);
            Detector detector = Detector.New.Init(Skyray.EDXRFLibrary.DetectorType.Si, 5.895, 170);
            node = rootNode.SelectSingleNode("Detector");
            XmlNode detectorNode = node.SelectSingleNode("DetectorType");
            if (detectorNode.InnerText.ToString().Trim().ToLower() == "Dp5".ToLower())
                device.IsDP5 = true;
            device.Dp5Version = Dp5Version.Dp5_CommonUsb;
            //detector.Type = (DetectorType)Enum.Parse(typeof(DetectorType), detectorNode.InnerText.ToString());
            //if (detector.Type == DetectorType.Dp5) 
            detectorNode = node.SelectSingleNode("Energy");
            detector.Energy = Convert.ToDouble(detectorNode.InnerText);
            detectorNode = node.SelectSingleNode("Fwhm");
            detector.Fwhm = Convert.ToDouble(detectorNode.InnerText);
            device.Detector = detector;
        }

        public void ReadFileCollimator(string filePath, Skyray.EDXRFLibrary.Device device)
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
            node = rootNode.SelectSingleNode("TargetCount");
            int targetCount = Convert.ToInt32(node.InnerText);
            if (targetCount > 0)
            {
                device.CollimatorMaxNum = targetCount;
                int[] Target = new int[targetCount];
                for (int i = 0; i < targetCount; ++i)
                {
                    Collimator filter = Collimator.New;
                    filter.Num = i+1;
                    node = rootNode.SelectSingleNode("Target" + i.ToString());
                    filter.Step = Convert.ToInt32(node.InnerText);
                    device.Collimators.Add(filter);
                }
            }
        }

        public void ReadFileFilter(string filePath, Skyray.EDXRFLibrary.Device device)
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
            node = rootNode.SelectSingleNode("TargetCount");
            int targetCount = Convert.ToInt32(node.InnerText);
            if (targetCount > 0)
            {
                device.FilterMaxNum = targetCount;
                int[] Target = new int[targetCount];
                for (int i = 0; i < targetCount; ++i)
                {
                    Filter filter = Filter.New;
                    filter.Num = i+1;
                    node = rootNode.SelectSingleNode("Target" + i.ToString());
                    filter.Step = Convert.ToInt32(node.InnerText);
                    device.Filter.Add(filter);
                }
            }
        }


        

        public void ReadFileRaytube(string filePath, Skyray.EDXRFLibrary.Device device)
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


        public Condition ReadDeviceCondition(string initPath, string paramPath, Skyray.EDXRFLibrary.Device device, string conditionName,List<DemarcateEnergy> listEnergy)
        {
            if (!System.IO.File.Exists(initPath) || !System.IO.File.Exists(paramPath))
            {
                return null;
            }
            Condition condition = Condition.New;
            DeviceParameter parameter = Default.GetDeviceParameter(device.SpecLength,1);
            InitParameter InitParam = Default.GetInitParameter(device.SpecLength);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(initPath);
            XmlNode xmlNode = xmldoc.SelectSingleNode("InitParameter");
            XmlNode innerNode = xmlNode.SelectSingleNode("TubeVoltage");
            InitParam.TubVoltage = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("TubeCurrent");
            InitParam.TubCurrent = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("Gain");
            InitParam.Gain = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("FineGain");
            InitParam.FineGain = int.Parse(innerNode.InnerText);
            DbObjectList<PreHeatParams> preList = PreHeatParams.FindAll();
            if (preList.Count > 0)
            {
                preList[0].FineGain = InitParam.FineGain;
                preList[0].Save();
            }
            innerNode = xmlNode.SelectSingleNode("Channel");
            InitParam.Channel = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("ChannelErr");
            InitParam.ChannelError = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("ElemName");
            InitParam.ElemName = innerNode.InnerText;
            condition.InitParam = InitParam;
            xmldoc.Load(paramPath);
            xmlNode = xmldoc.SelectSingleNode("DeviceParameter");
            innerNode = xmlNode.SelectSingleNode("TubeVoltage");
            parameter.TubVoltage = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("TubeCurrent");
            parameter.TubCurrent = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("PreTime");
            parameter.PrecTime = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("CollimatorId");
            if (innerNode.InnerText!="")
            parameter.CollimatorIdx = int.Parse(innerNode.InnerText)+1;
            innerNode = xmlNode.SelectSingleNode("FilterId");
            if (innerNode.InnerText!="")
            parameter.FilterIdx = int.Parse(innerNode.InnerText)+1;
            innerNode = xmlNode.SelectSingleNode("IsVacuumed");
            parameter.IsVacuum = Convert.ToBoolean(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("IsAdjustCountRate");
            parameter.IsAdjustRate = Convert.ToBoolean(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("MaxRate");
            parameter.MaxRate = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("MinRate");
            parameter.MinRate = int.Parse(innerNode.InnerText);
            parameter.Name = device.Name;
            condition.DeviceParamList.Add(parameter);
            //condition.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg());
            //condition.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu());
            listEnergy.ForEach(wc => { condition.DemarcateEnergys.Add(wc); });
            condition.Device = device;
            condition.Name = conditionName;
            condition.Save();
            return condition;
        }

        public List<Skyray.EDXRFLibrary.DemarcateEnergy> ReadDeviceDemarcatEnergy(string path)
        {
            List<DemarcateEnergy> list = new List<DemarcateEnergy>();
            if (!System.IO.File.Exists(path))
            {
                return list;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode rootNode = doc.SelectSingleNode("DemarcateEnergy");
            foreach (XmlElement elementItem in rootNode.ChildNodes)
            {
                DemarcateEnergy energyList = DemarcateEnergy.New;
                energyList.ElementName = elementItem.Attributes["Name"].Value.ToString();
                energyList.Energy = double.Parse(elementItem.Attributes["Energy"].Value.ToString());
                energyList.Line = (XLine)Enum.Parse(typeof(XLine),elementItem.Attributes["Line"].Value.ToString());
                energyList.Channel = int.Parse(elementItem.Attributes["Channel"].Value.ToString());
                list.Add(energyList);
            }
            return list;
        }

        public Condition ReadDeviceCondition(string initPath, string paramPath, Skyray.EDXRFLibrary.Device device)
        {
            if (!System.IO.File.Exists(initPath) || !System.IO.File.Exists(paramPath))
            {
                return null;
            }
            Condition condition = Condition.New;
            DeviceParameter parameter = Default.GetDeviceParameter(device.SpecLength,1);
            InitParameter InitParam = Default.GetInitParameter(device.SpecLength);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(initPath);
            XmlNode xmlNode = xmldoc.SelectSingleNode("InitParameter");
            XmlNode innerNode = xmlNode.SelectSingleNode("TubeVoltage");
            InitParam.TubVoltage = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("TubeCurrent");
            InitParam.TubCurrent = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("Gain");
            InitParam.Gain = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("FineGain");
            InitParam.FineGain = int.Parse(innerNode.InnerText);
            DbObjectList<PreHeatParams> preList = PreHeatParams.FindAll();
            if (preList.Count > 0)
            {
                preList[0].FineGain = InitParam.FineGain;
                preList[0].Save();
            }
            innerNode = xmlNode.SelectSingleNode("Channel");
            InitParam.Channel = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("ChannelErr");
            InitParam.ChannelError = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("ElemName");
            InitParam.ElemName = innerNode.InnerText;
            condition.InitParam = InitParam;
            xmldoc.Load(paramPath);
            xmlNode = xmldoc.SelectSingleNode("DeviceParameter");
            innerNode = xmlNode.SelectSingleNode("TubeVoltage");
            parameter.TubVoltage = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("TubeCurrent");
            parameter.TubCurrent = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("PreTime");
            parameter.PrecTime = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("CollimatorId");
            parameter.CollimatorIdx = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("FilterId");
            parameter.FilterIdx = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("IsVacuumed");
            parameter.IsVacuum = Convert.ToBoolean(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("IsAdjustCountRate");
            parameter.IsAdjustRate = Convert.ToBoolean(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("MaxRate");
            parameter.MaxRate = int.Parse(innerNode.InnerText);
            innerNode = xmlNode.SelectSingleNode("MinRate");
            parameter.MinRate = int.Parse(innerNode.InnerText);
            parameter.Name = device.Name;
            condition.DeviceParamList.Add(parameter);
            condition.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
            condition.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
            condition.Device = device;
            condition.Name = device.Name;
            condition.Save();
            return condition;
        }

        public void LoadCurrentCurves(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                return;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            currentCurveName.Clear();
            XmlNodeList curves = xmlDoc.SelectSingleNode("CurrentCurves").ChildNodes;
            try
            {
                foreach (XmlNode node in curves)
                {
                    XmlNode temp = node.SelectSingleNode("Name");
                    XmlNode tempId = node.SelectSingleNode("ID");
                    currentCurveName.Add(int.Parse(tempId.InnerText), temp.InnerText);
                }
            }
            catch { }
        }

        public Dictionary<int, string> currentCurveName = new Dictionary<int, string>();

        public void CreateCurve(string path, WorkRegion workRegion,Condition condition)
        {
            if (path.IsNullOrEmpty())
                return;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] curveInfo = dir.GetDirectories();
            if (curveInfo == null || curveInfo.Length == 0)
                return;
            int i = 0;
            foreach (DirectoryInfo info in curveInfo)
            {
                if (info.Name.Equals("PureElement")) continue;
                Console.WriteLine(info.Name);
                ElementList elementList = GetIntestedElementList(path + "\\ElemList.xml", condition);
                GetStandardSample(elementList, info.FullName + "\\Data.xml");
                WorkCurve workCurve = WorkCurve.New;
                workCurve.ElementList = elementList;
                workCurve.FuncType = FuncType.Rohs;
                workCurve.CalcType = CalcType.EC;
                workCurve.Name = info.Name;
                workCurve.WorkRegion = workRegion;
                workCurve.Condition = condition;
                workCurve.ConditionName = condition.Name;
                int index = (int)workRegion.RohsSampleType;
                string name = string.Empty;
                if (currentCurveName.TryGetValue(index, out name) && workCurve.Name==name)
                    workCurve.IsDefaultWorkCurve = true;
                else if (currentCurveName.Count == 0 && i == 0)
                    workCurve.IsDefaultWorkCurve = true;
                i++;
                workCurve.Save();
            }
        }

        public List<string> GetDefaultCurve(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNode rootNode = xmlDoc.SelectSingleNode("CurrentCurves");
            XmlNodeList nodeList = rootNode.ChildNodes;
            List<string> curve = new List<string>();
            foreach (XmlNode node in nodeList)
            {
                if (curve.Count >= 6) break;
                XmlNode innerNode = node.SelectSingleNode("Name");
                curve.Add(innerNode.InnerText);
            }
            return curve;
        }

        public Skyray.EDXRFLibrary.ElementList GetIntestedElementList(string filePath, Condition condition)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNode rootNode = xmlDoc.SelectSingleNode("ElemList");
            XmlNodeList nodeList = rootNode.ChildNodes;
            ElementList elementList = Default.GetElementList();
            foreach (XmlNode node in nodeList)
            {
                CurveElement curveElement;
                XmlNode innerNode = node.SelectSingleNode("Name");
                if (elementList.Items.ToList().Find(a => a.Caption.Equals(innerNode.InnerText)) != null)
                {
                    curveElement = elementList.Items.ToList().Find(a => a.Caption.Equals(innerNode.InnerText));
                    curveElement.IntensityComparison = true;
                    innerNode = node.SelectSingleNode("PeakLeft");
                    curveElement.BPeakLow = int.Parse(innerNode.InnerText);
                    innerNode = node.SelectSingleNode("PeakRight");
                    curveElement.BPeakHigh = int.Parse(innerNode.InnerText);
                    curveElement.Save();
                    continue;
                }
                else
                {
                    curveElement = CurveElement.New.Init("", true, "", 0, 0, "", XLine.Ka, 0, 0, 0, 0, true, 0, 0, 0, 0.00,
                                IntensityWay.FullArea, false, 0, 0, 0, CalculationWay.Insert, FpCalculationWay.LinearWithAnIntercept, ElementFlag.Calculated, LayerFlag.Calculated,
                                    ContentUnit.ppm, ThicknessUnit.um, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", false, Color.Red.ToArgb(), " ", 0, "", false, true, true, false, 1, 0, "", "", "",false,"");
                }
                curveElement.Caption = innerNode.InnerText;
                innerNode = node.SelectSingleNode("ID");
                curveElement.AtomicNumber = int.Parse(innerNode.InnerText);
                innerNode = node.SelectSingleNode("Line");
                curveElement.AnalyteLine = (XLine)(Enum.Parse(typeof(XLine), innerNode.InnerText));
                innerNode = node.SelectSingleNode("PeakLeft");
                curveElement.PeakLow = int.Parse(innerNode.InnerText);
                innerNode = node.SelectSingleNode("PeakRight");
                curveElement.PeakHigh = int.Parse(innerNode.InnerText);
                innerNode = node.SelectSingleNode("BaseLeft");
                curveElement.BaseLow = int.Parse(innerNode.InnerText);
                innerNode = node.SelectSingleNode("BaseRight");
                curveElement.BaseHigh = int.Parse(innerNode.InnerText);
                innerNode = node.SelectSingleNode("FitElems");
                curveElement.SReferenceElements = innerNode.InnerText;
                innerNode = node.SelectSingleNode("ReviseElems");
                if (innerNode.InnerText != null && innerNode.InnerText != "")
                {
                    string newInflu = innerNode.InnerText.Replace(';', ',');
                    curveElement.SInfluenceElements = newInflu;
                    string[] arrRef = newInflu.Split(',');
                    innerNode = node.SelectSingleNode("ReviseCoeffs");
                    string newCoe = innerNode.InnerText.Replace(';', ',');
                    curveElement.SInfluenceCoefficients = newCoe;
                    string[] arrCoe = newCoe.Split(',');
                    for (int i = 0; i < arrRef.Length; i++)
                    {
                        ElementRef er = ElementRef.New.Init("", true, 0);
                        er.Name = arrRef[i];
                        er.RefConf = Convert.ToDouble(arrCoe[i]);
                        curveElement.ElementRefs.Add(er);
                    }
                }

                innerNode = node.SelectSingleNode("IntWay");
                switch (innerNode.InnerText)
                {
                    case "Gauss":
                        curveElement.IntensityWay = IntensityWay.FixedGauss;
                        break;
                    case "Total":
                        curveElement.IntensityWay = IntensityWay.FullArea;
                        break;
                    case "Net":
                        curveElement.IntensityWay = IntensityWay.NetArea;
                        break;
                    case "FPGauss":
                        curveElement.IntensityWay = IntensityWay.FPGauss;
                        break;
                    case "Element":
                        curveElement.IntensityWay = IntensityWay.Reference;
                        break;
                    default: curveElement.IntensityWay = IntensityWay.FullArea;
                        break;
                }
                innerNode = node.SelectSingleNode("ContWay");
                switch (innerNode.InnerText)
                {
                    case "Insert":
                        curveElement.CalculationWay = CalculationWay.Insert;
                        break;
                    case "Linear":
                        curveElement.CalculationWay = CalculationWay.Linear;
                        break;
                    case "Square":
                        curveElement.CalculationWay = CalculationWay.Conic;
                        break;
                    default: curveElement.CalculationWay = CalculationWay.Insert;
                        break;
                }
                innerNode = node.SelectSingleNode("CompoundFormula");
                curveElement.Formula = innerNode.InnerText;
                curveElement.Formula = curveElement.Formula == "" ? curveElement.Caption : curveElement.Formula;
                innerNode = node.SelectSingleNode("Color");
                curveElement.Color = int.Parse(innerNode.InnerText);
                innerNode = node.SelectSingleNode("IsDisplay");
                curveElement.IsDisplay = Convert.ToBoolean(innerNode.InnerText);
                curveElement.IsShowElement = curveElement.IsDisplay;
                curveElement.ConditionID = 0;
                curveElement.DevParamId = condition.DeviceParamList[0].Id;
                curveElement.Save();
                elementList.Items.Add(curveElement);
            }
            return elementList;
        }

        public void GetStandardSample(ElementList elementList, string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNode rootNode = xmlDoc.SelectSingleNode("NewDataSet");
            XmlNodeList nodeList = rootNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "StdDataTable")
                {
                    StandSample ss = StandSample.New.Init("", "0","0", "0", "0", "0", true, "", 0, 0, "", 0, "0");
                    XmlNode innerNode = node.SelectSingleNode("SampleName");

                    ss.SampleName = innerNode.InnerText;
                    innerNode = node.SelectSingleNode("Intensity");
                    ss.X = innerNode.InnerText;
                    innerNode = node.SelectSingleNode("Content");
                    ss.Y = (float.Parse(innerNode.InnerText) / 10000).ToString();
                    innerNode = node.SelectSingleNode("Enabled");
                    ss.Active = Convert.ToBoolean(innerNode.InnerText);
                    innerNode = node.SelectSingleNode("ElemName");
                    ss.ElementName = innerNode.InnerText;
                    CurveElement element = elementList.Items.ToList().Find(l => l.Caption == innerNode.InnerText);
                    if (null == element) continue;//20130321 cyq 导入的标样数据中有曲线元素找不到的元素
                    ss.Density = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(ss.ElementName.ToUpper())).AtomDensity;
                    ss.Save();
                    element.Samples.Add(ss);
                }
            }
            CurveElement ce = elementList.Items.OrderByDescending(i => i.Samples.Count).ToList()[0];
            foreach (var ces in ce.Samples)
            {
                foreach (var c in elementList.Items)
                {
                    if (c.Samples.ToList().Find(ss => ss.SampleName.Equals(ces.SampleName)) != null)
                    { continue; }
                    else
                    {
                        StandSample ss = StandSample.New.Init(ces.SampleName, "0","0", "0", "0", "0", false, c.Caption, 0, 0, "", 0, "0");
                        c.Samples.Add(ss);
                    }
                }
            }
        }

        public void GetSpectrum(FileInfo filePath, List<Device> deviceList, SpecType specType)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath.FullName);
            DeviceParameterEntity deviceParams = new DeviceParameterEntity();
            string deviceName = ReadHelper.LoadDeviceInfo(xmlDoc, deviceParams);
            string name = filePath.Name.Replace(".Spe", "");
            Device d = deviceList.Find(delegate(Device w) { return w.IsDefaultDevice == true; });
            if (d == null)
                return;
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
            specList.SpecType = specType;
            spec.RemarkInfo = "";
            spec.DeviceParameter = deviceParams;
            specList.Specs = new SpecEntity[1];
            specList.Specs[0] = spec;
            specList.InitParam = d.Conditions[0].InitParam.ConvertToNewEntity();
            WorkCurveHelper.DataAccess.Save(specList);
            ReadHelper.LoadImage(xmlDoc, specList,2);
        }

        public void GetCustomStandard(string path)
        {
            if (path.IsNullOrEmpty())
                return;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DirectoryInfo dir = new DirectoryInfo(path);

            FileInfo[] fineInfo = dir.GetFiles("*.xml");

            if (fineInfo == null || fineInfo.Length == 0)
                return;
            foreach (FileInfo info in fineInfo)
            {
                switch (info.Name)
                {
                    case "Standard0.xml":
                        CreateStandard("Rohs",path + "\\Standard0.xml");
                        break;
                    case "Standard1.xml":
                        CreateStandard("玩具一", path + "\\Standard1.xml");
                        break;
                    case "Standard2.xml":
                        CreateStandard("玩具二", path + "\\Standard2.xml");
                        break;
                    case "Standard3.xml":
                        CreateStandard("自定义", path + "\\Standard3.xml");
                        break;
                    default: break;
                }
            }
        }

        private void CreateStandard(string name, string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            CustomStandard standard = CustomStandard.New;
            standard.StandardName = name;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNode rootNode = xmlDoc.SelectSingleNode("ArrayOfContentRang");
            XmlNodeList nodeList = rootNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                StandardData data = StandardData.New;
                XmlNode innerNode = node.SelectSingleNode("Name");
                data.ElementCaption = innerNode.InnerText;
                innerNode = node.SelectSingleNode("MaxValue");
                data.StandardContent = Convert.ToDouble(innerNode.InnerText);
                standard.StandardDatas.Add(data);
            }
            standard.Save();
        }

        public void CreateCurve(Skyray.EDXRFLibrary.WorkCurve curve, string filePath, List<Skyray.EDXRFLibrary.Condition> conditions)
        {
            throw new NotImplementedException();
        }

        #region IWorkCurve 成员

        public Skyray.EDXRFLibrary.ElementList GetIntestedElementList(string filePath, string workCurveDir)
        {
            throw new NotImplementedException();
        }

        public Skyray.EDXRFLibrary.Compounds ReadFileCompounds(string filePath)
        {
            throw new NotImplementedException();
        }

        public void ReadElementsSample(string filePath, Skyray.EDXRFLibrary.CurveElement elements)
        {
            throw new NotImplementedException();
        }

        public List<Skyray.EDXRFLibrary.DemarcateEnergy> GetConditionDemarcate(string filePath)
        {
            throw new NotImplementedException();
        }

        public Skyray.EDXRFLibrary.QualeElement GetQualeElement(string filePath)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISpectrums 成员

        public void GetSpectrum(System.IO.FileInfo filePath, Skyray.EDXRFLibrary.SpecType type, Skyray.EDXRFLibrary.Spectrum.SpecListEntity specList, List<Skyray.EDXRFLibrary.Device> deviceList)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDevice 成员

        public void ReadFileDetector(string filePaht, Skyray.EDXRFLibrary.Device device)
        {
            throw new NotImplementedException();
        }

        public void ReadFileChamber(string filePath, Skyray.EDXRFLibrary.Device device)
        {
            throw new NotImplementedException();
        }

        public void IsExistMoveAis(string filePath, Skyray.EDXRFLibrary.Device device)
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
