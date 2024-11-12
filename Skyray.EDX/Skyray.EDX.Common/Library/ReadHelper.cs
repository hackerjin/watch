using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Skyray.EDXRFLibrary;
using System.IO;
using System.Drawing;
using System.Data;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common.Library
{
    public class ReadHelper
    {
       
        /// <summary>
        /// 从文件中读取参数
        /// </summary>
        /// <param name="fileName">文件名</param>
        public Device Load(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            XmlNode xmlRootElem = xmlDoc.SelectSingleNode("Device");
            XmlNode xmlElem = xmlRootElem.SelectSingleNode("Name");
            Device device = Device.New;
            device.Name = xmlElem.InnerText;
            xmlElem = xmlRootElem.SelectSingleNode("DllType");
            device.PortType = (DllType)Enum.Parse(typeof(DllType), xmlElem.InnerText);
            xmlElem = xmlRootElem.SelectSingleNode("IsCoverOpenedTest");
            if (xmlElem != null)
            {
                device.IsAllowOpenCover = bool.Parse(xmlElem.InnerText);
            }
          
            xmlElem = xmlRootElem.SelectSingleNode("PumpExist");
            device.HasVacuumPump = Convert.ToBoolean(xmlElem.InnerText);
            xmlElem = xmlRootElem.SelectSingleNode("FilterMotorExist");
            device.HasFilter = Convert.ToBoolean(xmlElem.InnerText);
            xmlElem = xmlRootElem.SelectSingleNode("CollMotorExist");
            device.HasCollimator = Convert.ToBoolean(xmlElem.InnerText);
            xmlElem = xmlRootElem.SelectSingleNode("ChamberMotorExist");
            if (xmlElem == null)
            {
                device.HasChamber = false;
            }
            else
            {
                device.HasChamber = Convert.ToBoolean(xmlElem.InnerText);
            }
            xmlElem = xmlRootElem.SelectSingleNode("ExistMagnet");
            if (xmlElem == null)
            {
                device.HasElectromagnet = false;
            }
            else
            {
                device.HasElectromagnet = (Boolean)Convert.ToBoolean(xmlElem.InnerText);
            }
            return device;
        }

        /// <summary>
        /// 载入测试条件信息
        /// </summary>
        /// <param name="xmlDoc"></param>
        public static string LoadDeviceInfo(XmlDocument xmlDoc, DeviceParameterEntity DevParam)
        {
            XmlNode xmlElemRoot = xmlDoc.SelectSingleNode("Spectrum");
            XmlNode xmlElem = xmlElemRoot.SelectSingleNode("DeviceParam");
            XmlNodeList nodeList = xmlElem.ChildNodes;
            string deviceName = string.Empty;
            foreach (XmlNode Node in nodeList)
            {
                switch (Node.Name)
                {
                    case "DeviceName":
                        deviceName = Node.InnerText;
                        break;
                    case "PrecTime":
                        DevParam.PrecTime = Convert.ToInt32(Node.InnerText);
                        break;
                    case "TubCurrent":
                        DevParam.TubCurrent = Convert.ToInt32(Node.InnerText);
                        break;
                    case "TubVoltage":
                        DevParam.TubVoltage = Convert.ToInt32(Node.InnerText);
                        break;
                    case "FilterIdx":
                        DevParam.FilterIdx = Convert.ToInt32(Node.InnerText);
                        break;
                    case "CollimatorIdx":
                        DevParam.CollimatorIdx = Convert.ToInt32(Node.InnerText);
                        break;
                    case "IsVacuum":
                        DevParam.IsVacuum = Convert.ToBoolean(Node.InnerText);
                        break;
                    case "IsAdjustRate":
                        DevParam.IsAdjustRate = Convert.ToBoolean(Node.InnerText);
                        break;
                    case "MinRate":
                        DevParam.MinRate = Convert.ToDouble(Node.InnerText);
                        break;
                    case "MaxRate":
                        DevParam.MaxRate = Convert.ToDouble(Node.InnerText);
                        break;
                    case "BeginChann":
                        DevParam.BeginChann = Convert.ToInt32(Node.InnerText);
                        break;
                    case "EndChann":
                        DevParam.EndChann = Convert.ToInt32(Node.InnerText);
                        break;
                    default:
                        break;
                }
            }
            return deviceName;
        }
       

        /// <summary>
        /// 载入样品信息
        /// </summary>
        /// <param name="xmlDoc"></param>
        public static void LoadSampleInfo(XmlDocument xmlDoc,SpecListEntity specList,SpecEntity spec)
        {
            XmlNode xmlElemRoot = xmlDoc.SelectSingleNode("Spectrum");
            XmlNode xmlElem = xmlElemRoot.SelectSingleNode("SampleInfo");
            XmlNodeList nodeList = xmlElem.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                switch (node.Name)
                {
                    case "Supplier":
                        specList.Supplier = node.InnerText;
                        break;
                    case "Weight":
                        specList.Weight = Convert.ToSingle(node.InnerText);
                        break;
                    case "Shape":
                        specList.Shape = node.InnerText;
                        break;
                    case "Operator":
                        specList.Operater = node.InnerText;
                        break;
                    case "SpecDate":
                        specList.SpecDate = DateTime.Parse(node.InnerText);
                        break;
                    case "Caption":
                        specList.SampleName = node.InnerText;
                        break;
                    case "UsedTime":
                        spec.UsedTime = Convert.ToInt32(node.InnerText);
                        spec.SpecTime = Convert.ToDouble(node.InnerText);
                        break;
                    case "SpecSummary": //样品描述信息
                        specList.SpecSummary = Convert.ToString(node.InnerText);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="specList"></param>
        /// <param name="softType">0:EDXRF,1:XRF,2:Rohs,3:Thick</param>
        public static void LoadImage(XmlDocument xmlDoc, SpecListEntity specList,int softType)
        {
            XmlNode xmlElemRoot = xmlDoc.SelectSingleNode("Spectrum");
            XmlNode xmlElem = xmlElemRoot.SelectSingleNode("SampleImage");
            if (xmlElem != null)
            {
                byte[] byteData = Convert.FromBase64String(xmlElem.InnerText);
                if (byteData.Length > 0)
                {
                    string samplePath =string.Empty;
                    switch (softType)
                    {
                        case 0:
                            samplePath = Environment.CurrentDirectory + "\\Image\\SampleImage\\EDX3000\\" + specList.Name + ".jpg";
                            break;
                        case 1:
                            samplePath = Environment.CurrentDirectory + "\\Image\\SampleImage\\EDXX\\" + specList.Name + ".jpg";
                            break;
                        case 2:
                            samplePath = Environment.CurrentDirectory + "\\Image\\SampleImage\\EDXR\\" + specList.Name + ".jpg";
                            break;
                        case 3:
                            samplePath = Environment.CurrentDirectory + "\\Image\\SampleImage\\EDXT\\" + specList.Name + ".jpg";
                            break;
                        default: break;
                    }
                    if (string.IsNullOrEmpty(samplePath))
                        return;
                    MemoryStream stream = new MemoryStream();
                    stream.Write(byteData, 0, byteData.Length);
                    System.Drawing.Image.FromStream(stream).Save(samplePath);
                    stream.Close();
                }
            }
        }

        public static List<Condition> LoadCondition(string fileName)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fileName);
            XmlNode xmlNode = xmldoc.SelectSingleNode("ConditionList");//测试条件
            List<Condition> conditionList = new List<Condition>();
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                Condition condition = Condition.New;
                XmlNode nodeFirst = node.FirstChild;
                condition.Name = nodeFirst.InnerText;//ID
                InitParameter InitParam = InitParameter.New;
                nodeFirst = nodeFirst.NextSibling;
                foreach (XmlNode nodeOfInit in nodeFirst)
                {
                    switch (nodeOfInit.Name)
                    {
                        case "Channel"://通道
                            InitParam.Channel = Convert.ToInt32(nodeOfInit.InnerText);
                            break;
                        case "ChannelError"://通道误差
                            InitParam.ChannelError = Convert.ToInt32(nodeOfInit.InnerText);
                            break;
                        case "ElemName"://元素名称
                            InitParam.ElemName = nodeOfInit.InnerText;
                            break;
                        case "FineGain"://细调
                            InitParam.FineGain = Convert.ToInt32(nodeOfInit.InnerText);
                            break;
                        case "Gain"://粗调
                            InitParam.Gain = Convert.ToInt32(nodeOfInit.InnerText);
                            break;
                        case "TubCurrent"://管流
                            InitParam.TubCurrent = Convert.ToInt32(nodeOfInit.InnerText);
                            break;
                        case "TubVoltage"://管压
                            InitParam.TubVoltage = Convert.ToInt32(nodeOfInit.InnerText);
                            break;
                        case "Filter"://滤光片
                            InitParam.Filter = Convert.ToInt32(nodeOfInit.InnerText);
                            break;
                        case "Collimator"://准直器
                            InitParam.Collimator = Convert.ToInt32(nodeOfInit.InnerText);
                            break;
                        default:
                            break;
                    }
                }
                condition.InitParam = InitParam;
                //Test
                DeviceParameter parameter;// = new DeviceParameter();
                nodeFirst = nodeFirst.NextSibling;
                while (nodeFirst != null)
                {
                    //add by 田春华                    
                    foreach (XmlNode nodeOfTestList in nodeFirst.ChildNodes)
                    {
                        #region DeviceParameter
                        parameter = DeviceParameter.New;
                        foreach (XmlNode nodeOfTest in nodeOfTestList)
                        {
                            switch (nodeOfTest.Name)
                            {
                                case "BeginChann":
                                    parameter.BeginChann = Convert.ToInt32(nodeOfTest.InnerText);
                                    break;
                                case "CollimatorIdx":
                                    parameter.CollimatorIdx = Convert.ToInt32(nodeOfTest.InnerText);
                                    break;
                                case "DeviceName":
                                    parameter.Name = nodeOfTest.InnerText;
                                    break;
                                case "EndChann":
                                    parameter.EndChann = Convert.ToInt32(nodeOfTest.InnerText);
                                    break;
                                case "FilterIdx":
                                    parameter.FilterIdx = Convert.ToInt32(nodeOfTest.InnerText);
                                    break;
                                case "IsAdjustRate":
                                    parameter.IsAdjustRate = Convert.ToBoolean(nodeOfTest.InnerText);
                                    break;
                                case "IsVacuum":
                                    parameter.IsVacuum = Convert.ToBoolean(nodeOfTest.InnerText);
                                    break;
                                // add 2009-8-26
                                case "VacuumTime":
                                    parameter.VacuumTime = Convert.ToInt32(nodeOfTest.InnerText);
                                    break;
                                // 修改：11-30-2009 田春华
                                // 目的：   真空度
                                case "VacuumDegree":
                                    parameter.VacuumDegree = Convert.ToDouble(nodeOfTest.InnerText);
                                    break;
                                case "IsVacuumDegree":
                                    parameter.IsVacuumDegree = Convert.ToBoolean(nodeOfTest.InnerText);
                                    break;

                                case "MaxRate":
                                    parameter.MaxRate = Convert.ToDouble(nodeOfTest.InnerText);
                                    break;
                                case "MinRate":
                                    parameter.MinRate = Convert.ToDouble(nodeOfTest.InnerText);
                                    break;
                                case "PrecTime":
                                    parameter.PrecTime = Convert.ToInt32(nodeOfTest.InnerText);
                                    break;
                                case "TubCurrent":
                                    parameter.TubCurrent = Convert.ToInt32(nodeOfTest.InnerText);
                                    break;
                                case "TubVoltage":
                                    parameter.TubVoltage = Convert.ToInt32(nodeOfTest.InnerText);
                                    break;
                                default:
                                    break;
                            }
                        }
                        condition.DeviceParamList.Add(parameter);
                        #endregion
                    }
                    nodeFirst = nodeFirst.NextSibling;
                }

                condition.DemarcateEnergys.Add(DemarcateEnergy.New.Init("Ag", XLine.Ka, 22.1, 1105));
                condition.DemarcateEnergys.Add(DemarcateEnergy.New.Init("Cu", XLine.Ka, 8.041, 412));
                conditionList.Add(condition);
                //条件队列里添加此默认的条件
            }
            return conditionList;
        }


        /// <summary>
        /// 从文件中载入参数
        /// </summary>
        public static ElementList loadElementList(string fileName,string workPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            int number;
            xmlDoc.Load(fileName);
            XmlNodeList nodeChildList;
            XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
            ElementList elementList = ElementList.New;
            elementList.MatchSpecListIdStr = "";
            elementList.RefSpecListIdStr = "";
            double Asrat = 0d;
            double Loi = 0d;
            foreach (XmlNode node in nodeList)
            {
                nodeChildList = node.ChildNodes;
                switch (node.Name)
                {
                    case "isUnitary":
                        elementList.IsUnitary = Convert.ToBoolean(node.InnerText);
                        break;
                    case "UnitaryValue":
                        elementList.UnitaryValue = Convert.ToDouble(node.InnerText);
                        break;
                    case "Asrat":
                        Asrat = Convert.ToDouble(node.InnerText);  //添加剂比率
                        break;
                    case "Loi":
                        Loi = Convert.ToDouble(node.InnerText);//烧失量比率
                        break;
                    case "Element": //读取元素信息
                        #region
                        number = Convert.ToInt32(node.Attributes[0].InnerText);
                        CurveElement element = CurveElement.New;
                        foreach (XmlNode childNode in nodeChildList)
                        {
                            switch (childNode.Name)
                            {
                                // 修改：1-6-2010 田春华
                                // 目的：一次因子
                                //case "ReviseK0":
                                //    element.K0 = Convert.ToDouble(childNode.InnerText);
                                //    break;
                                //case "ReviseK1":
                                //    element.K1 = Convert.ToDouble(childNode.InnerText);
                                //    break;
                                case "ID":
                                    element.AtomicNumber = Convert.ToInt32(childNode.InnerText);
                                    break;
                                case "Caption":
                                    element.Caption = childNode.InnerText;
                                    element.Formula = element.Caption;
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
                                    element.IntensityWay = GetAccordingWay(childNode.InnerText);
                                    break;
                                case "nMode":
                                    element.FpCalculationWay = GetAccordingFpCalculationWay(childNode.InnerText);
                                    break;
                                case "units":
                                    element.ContentUnit = (childNode.InnerText == "percent"?ContentUnit.per:ContentUnit.ppm);
                                    break;
                                ///属性 0613
                                case "property":
                                    element.Flag = (ElementFlag)Enum.Parse(typeof(ElementFlag), childNode.InnerText);
                                    break;
                                //case "ReviseParam":
                                //    element.ReviseParam = Convert.ToDouble(childNode.InnerText);
                                //    break;
                                //case "OriginalData":
                                //    Item[number].OriginalData = Convert.ToDouble(childNode.InnerText);
                                //    break;
                                //case "ReviseData":
                                //    Item[number].ReviseData = Convert.ToDouble(childNode.InnerText);
                                //    break;
                                //消去值
                                //case "ExpunctionValue":
                                //    Item[number].ExpunctionValue = Convert.ToDouble(childNode.InnerText);
                                //    break;
                                case "Line":
                                    element.AnalyteLine = (XLine)Enum.Parse(typeof(XLine), childNode.InnerText);
                                    break;
                                case "ReviseElem":
                                    element.SInfluenceElements = childNode.InnerText;
                                    break;
                                case "Coefs":
                                    string[] strCoefs =
                                        childNode.InnerText.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    element.K0 = Convert.ToDouble(strCoefs[0]);
                                    element.K1 = Convert.ToDouble(strCoefs[1]);
                                    break;
                                case "FitElem":
                                    element.SReferenceElements = childNode.InnerText.TrimEnd(',');
                                    break;
                                case "Flag":
                                    element.Flag = GetElementFlag(childNode.InnerText);
                                    break;
                                case "LayerNumber":
                                    element.LayerNumber = Convert.ToInt32(childNode.InnerText);
                                    break;
                                //case "CompoundFormula":
                                //    Item[number].CompoundFormula = childNode.InnerText;
                                //    break;
                                //case "CompoundRate":
                                //    Item[number].CompoundRate = Convert.ToDouble(childNode.InnerText);
                                //    break;
                                //case "ConditionCode":
                                //    Item[number].ConditionCode = Convert.ToInt32(childNode.InnerText);
                                //    break;
                                case "CuvWay":
                                    element.CalculationWay = GetCalculationWay(childNode.InnerText);
                                    break;
                            }
                        }
                        element.Asrat = Asrat;
                        element.Loi = Loi;
                        element.LayerNumBackUp = "";
                        element.SSpectrumData = "";
                        element.SInfluenceCoefficients = "";
                        element.ElementSpecName = "";
                        if (!element.SReferenceElements.IsNullOrEmpty())
                            GetElementByPure(workPath + "\\PureSpec", element);
                        element.ColorHelper = "";
                        elementList.Items.Add(element);
                        #endregion
                        break;
                    case "CustomField":
                        int index = Convert.ToInt32(node.Attributes[0].InnerText);
                        CustomField customField = CustomField.New;
                        foreach (XmlNode childNode in nodeChildList)
                        {
                            switch (childNode.Name)
                            {
                                case "Name":
                                   customField.Name = childNode.InnerText;
                                    break;
                                case "Formula":
                                    customField.Expression = childNode.InnerText;
                                    break;
                            }
                        }
                        elementList.CustomFields.Add(customField);
                        break;
                }
            }
            return elementList;
        }

        public static void GetElementByPure(string pureElementPath, CurveElement element)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string path = pureElementPath + "\\" + element.Caption + ".spe";
            if (!File.Exists(path))
                return;
            xmlDoc.Load(path);
            XmlNode xmlElemRoot = xmlDoc.SelectSingleNode("Spectrum");
            XmlNode xmlElem = xmlElemRoot.SelectSingleNode("Data");
            char[] sep = new char[] { ',' };
            string[] dataStr = xmlElem.InnerText.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            string str = string.Empty;
            for (int i = 0; i < dataStr.Length; ++i)
            {
                str += Convert.ToInt32(dataStr[i]) + ",";
            }
            element.SSpectrumData = str;
            element.ElementSpecName = element.Caption + ".spe_1";
        }

        public static ElementFlag GetElementFlag(string str)
        {
            ElementFlag way = ElementFlag.Internal;
            switch (str)
            {
                case "MainElem":
                    way = ElementFlag.Internal;
                    break;
                case "Calculated":
                    way = ElementFlag.Calculated;
                    break;
                case "Fixed":
                    way = ElementFlag.Fixed;
                    break;
                case "Difference":
                    way = ElementFlag.Difference;
                    break;
                case "Added":
                    way = ElementFlag.Added;
                    break;
                default:
                    break;
            }
            return way;
        }

        public static IntensityWay GetAccordingWay(string str)
        {
            IntensityWay way = IntensityWay.FullArea;
            switch (str)
            {
                case "Total":
                    way = IntensityWay.FullArea;
                    break;
                case "Net":
                    way = IntensityWay.NetArea;
                    break;
                case "Gauss":
                    way = IntensityWay.Gauss;
                    break;
                case "Fit":
                    way = IntensityWay.Reference;
                    break;
                case "FPGauss":
                    way = IntensityWay.FPGauss;
                    break;
                default:
                    break;
            }
            return way;
        }


        public static FpCalculationWay GetAccordingFpCalculationWay(string str)
        {
            FpCalculationWay way = FpCalculationWay.LinearWithAnIntercept;
            switch (str)
            {
                case "linearWithoutAnIntercept":
                    way = FpCalculationWay.LinearWithoutAnIntercept;
                    break;
                case "linearWithAnIntercept":
                    way = FpCalculationWay.LinearWithAnIntercept;
                    break;
                case "quadraticWithoutAnIntercept":
                    way = FpCalculationWay.SquareWithoutAnIntercept;
                    break;
                case "quadraticWithAnIntercept":
                    way = FpCalculationWay.SquareWithAnIntercept;
                    break;
                default:
                    break;
            }
            return way;
        }


        public static CalculationWay GetCalculationWay(string str)
        {
            CalculationWay way = CalculationWay.Insert;
            switch (str)
            {
                case "Insert":
                    way = CalculationWay.Insert;
                    break;
                case "Linear":
                    way = CalculationWay.Linear;
                    break;
                case "Square":
                    way = CalculationWay.Conic;
                    break;
                case "IntCorr":
                    way = CalculationWay.IntensityCorrect;
                    break;
                case "FPGauss":
                    way = CalculationWay.ContentContect;
                    break;
                default:
                    break;
            }
            return way;
        }

        public static void LoadElementSamples(string filePath,CurveElement element)
        {
            DataTable dataTable = new DataTable();
            dataTable.ReadXml(filePath);
            DataRow[] rows = dataTable.Select();
            List<string> sampCaptionList = new List<string>(rows.Length);
            int index;
            string sampCaption;
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i]["Sample"].GetType() == typeof(System.DBNull))
                {
                    continue;
                }
                sampCaption = (string)rows[i]["Sample"];
                index = sampCaptionList.IndexOf(sampCaption);
                if (index < 0) sampCaptionList.Add(sampCaption);
            }

            int sampCount = sampCaptionList.Count;

            for (int i = 0; i < sampCount; i++)
            {
                rows = dataTable.Select("Sample='" + sampCaptionList[i] + "'");
                //for (int col = 0; col < Elements.NumberOfConstituents; ++col)
                //{
                StandSample sampleData = StandSample.New;
                sampleData.SampleName = sampCaptionList[i];
                sampleData.ElementName = element.Caption;
                sampleData.Layer = 0;
                sampleData.TotalLayer = 0;
                sampleData.Level = "";
                sampleData.X = "0";
                sampleData.Y = "0";
                sampleData .Active = true;

                for (int rec = 0; rec < rows.Length; ++rec)
                {
                    if (element.Caption.Equals((string)rows[rec]["Element"]))
                    {
                        sampleData.X = rows[rec]["XValue"].ToString();
                        sampleData.Y = rows[rec]["YValue"].ToString();
                        sampleData.Active = (bool)rows[rec]["Enabled"];
                    }
                }
                element.Samples.Add(sampleData);
            }
        }

        /// <summary>
        /// 从文件载入能量刻度的元素
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static List<DemarcateEnergy> LoadDemarcateEnergy(string fileName)
        {
            XmlDocument doc = new XmlDocument();//创建文件流
            doc.Load(fileName);//加载文件
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            List<DemarcateEnergy> listEnergy = new List<DemarcateEnergy>();
            foreach (XmlNode node in nodes)
            {
                DemarcateEnergy elem = DemarcateEnergy.New;
                elem.ElementName = node.Attributes["Name"].Value;
                elem.Line = (XLine)Enum.Parse(typeof(XLine), node.Attributes["Line"].Value);
                elem.Energy = Convert.ToDouble(node.Attributes["Energy"].Value);
                elem.Channel = Convert.ToInt32(node.Attributes["Channel"].Value);
                listEnergy.Add(elem);//添加
            }
            DemarcateEnergyHelp.CalParam(listEnergy);//计算斜率与参数
            return listEnergy;
        }

        /// <summary>
        /// 载入参数
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static QualeElement LoadQualeElement(string fileName)
        {
            XmlTextReader tr = new XmlTextReader(fileName);
            QualeElement qualeElement = QualeElement.New;
            try
            {
              
                tr.WhitespaceHandling = WhitespaceHandling.None;
                tr.MoveToContent();
                //按顺序读取各子节点的信息
                qualeElement.ChannFWHM = Convert.ToInt32(tr.ReadElementString());
                qualeElement.WindowWidth = Convert.ToInt32(tr.ReadElementString());
                qualeElement.Trh1= Convert.ToDouble(tr.ReadElementString());
                qualeElement.ValleyDistance = Convert.ToDouble(tr.ReadElementString());
                qualeElement.AreaLimt = Convert.ToDouble(tr.ReadElementString());
                qualeElement.AvoidElem = tr.ReadElementString();
            }
            finally
            {
                if (tr != null)
                    tr.Close();
            }
            return qualeElement;
        }

        public static void LoadWorkcurveParam(string fileName)
        {
            if (!System.IO.File.Exists(fileName)) return;
            CalibrationParam paramsCal = CalibrationParam.New;
            XmlTextReader reader = new XmlTextReader(fileName);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "IsEscProc":
                            paramsCal.IsEscapePeakProcess = Convert.ToBoolean(reader.ReadString());
                            break;
                        case "IsSumProc":
                            paramsCal.IsSumPeakProcess = Convert.ToBoolean(reader.ReadString());
                            break;
                        case "IsBkgAutoProc1":
                            paramsCal.IsRemoveBackGroundOne = Convert.ToBoolean(reader.ReadString());
                            break;
                        case "IsBkgAutoProc2":
                            paramsCal.IsRemoveBackGroundTwo = Convert.ToBoolean(reader.ReadString());
                            break;
                        case "IsBkgAutoProc3":
                            paramsCal.IsRemoveBackGroundThree = Convert.ToBoolean(reader.ReadString());
                            break;
                        //case "EscProckevch":
                        //    EscProckevch = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "EscProcfDetAngle":
                        //    EscProcfDetAngle = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "EscProcampFac":
                        //    EscProcampFac = Convert.ToDouble(reader.ReadString());
                        //    break;

                        //case "SumProckevch":
                        //    SumProckevch = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "SumProcChanOff":
                        //    SumProcChanOff = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "SumProcPpr":
                        //    SumProcPpr = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "SumProcAcqTime":
                        //    SumProcAcqTime = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "SumProcAdjFac":
                        //    SumProcAdjFac = Convert.ToDouble(reader.ReadString());
                        //    break;

                        //case "BkgAutoProc1Kevch":
                        //    BkgAutoProc1Kevch = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "BkgAutoProc2AmpFac":
                        //    BkgAutoProc2AmpFac = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "BkgAutoProc1Times":
                        //    BkgAutoProc1Times = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "BkgAutoProc2Times":
                        //    BkgAutoProc2Times = Convert.ToDouble(reader.ReadString());
                        //    break;
                        //case "BkgAutoProc3Points":
                        //    BkgAutoProc3Points = (reader.ReadString()).ToString();
                        //    break;
                        ////add by jialiujie
                        //case "ChinawareFileName":
                        //    this.ChinawareFileName = reader.ReadString();
                        //    break;
                        //case "ChinawareShowMaxCount":
                        //    this.ChinawareShowMaxCount = Convert.ToInt32(reader.ReadString());
                        //    break;
                        //case "StumerValue":
                        //    this.StumerValue = Convert.ToDouble(reader.ReadString());
                        //    break;
                    }
                }
            }
            reader.Close();
        }

    }


}
