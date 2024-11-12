using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDX.Common.Component;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.Library;
using System.Xml;
using System.IO;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using System.Drawing;

namespace Skyray.EDX.Common.DelphiConvert
{
    public class Thick800A:IDevice,IWorkCurve,ISpectrums
    {
        #region IDevice 成员
        private  Color[] colors=
            {
                Color.BlueViolet,
                Color.Brown,
                Color.DarkOrange,
                Color.Aqua,
                Color.DeepSkyBlue,
                Color.DimGray,
                Color.Chartreuse,
                Color.Chocolate,
                Color.DodgerBlue,
                Color.Black,
                Color.DarkBlue,
                Color.DarkRed,
                Color.Aquamarine,
                Color.CornflowerBlue,
                Color.DarkCyan,
                Color.DarkGoldenrod,
                Color.DarkGray,
                Color.DarkGreen,
                Color.DarkKhaki,
                Color.DarkMagenta,
                Color.DarkOliveGreen,
                Color.DarkOrchid,
                Color.DarkSalmon,
                Color.DarkSeaGreen,
                Color.DarkSlateBlue,
                Color.DarkSlateGray,
                Color.DarkTurquoise,
                Color.DarkViolet,
                Color.DeepPink,
                Color.Azure,
                Color.Beige,
                Color.Bisque,
                Color.BlanchedAlmond,
                Color.BurlyWood,
                Color.CadetBlue,
                Color.Coral,
                Color.Cornsilk,
                Color.Crimson,
                Color.Cyan,
                Color.AliceBlue,
                Color.AntiqueWhite,
                Color.HotPink
            };

        public void ReadFileCollimator(string filePath, Skyray.EDXRFLibrary.Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = TxtFile.GetPrivateProfileString(null, null, null, Buffer,
             Buffer.GetUpperBound(0), filePath);
            List<string> sectionList = new List<string>();
            TxtFile.GetStringsFromBuffer(Buffer, bufLen, sectionList);
            if (!sectionList.Contains("CollMotor"))
                return;
            StringBuilder temp = new StringBuilder(255);
            string caption = "CollMotor";
            int size = 255;
            TxtFile.GetPrivateProfileString(caption, "Exist", "",temp, size, filePath);
            device.HasCollimator = temp.ToString()=="0"?false:true;
            TxtFile.GetPrivateProfileString(caption, "MotorID", "",temp, size, filePath);
            device.CollimatorElectricalCode = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString(caption, "MotorDir", "", temp, size, filePath);
            device.CollimatorElectricalDirect = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString(caption, "Speed", "", temp, size, filePath);
            device.CollimatorSpeed = Convert.ToInt32((temp.ToString()));
            TxtFile.GetPrivateProfileString(caption, "TagCount", "", temp, size, filePath);
            int tagCount = Convert.ToInt32(temp.ToString());
            if (tagCount > 0)
            {
                int[] Target = new int[tagCount];
                for (int i = 0; i < tagCount; ++i)
                {
                    Collimator collimator = Collimator.New;
                    collimator.Num = i;
                    TxtFile.GetPrivateProfileString(caption, "Tag"+(i+1), "", temp, size, filePath);
                    collimator.Step = Convert.ToInt32(temp.ToString());
                    device.Collimators.Add(collimator);
                }
            }
        }

        public void ReadFileFilter(string filePath, Skyray.EDXRFLibrary.Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = TxtFile.GetPrivateProfileString(null, null, null, Buffer,
             Buffer.GetUpperBound(0), filePath);
            List<string> sectionList = new List<string>();
            TxtFile.GetStringsFromBuffer(Buffer, bufLen, sectionList);
            if (!sectionList.Contains("FilterMotor"))
                return;

            StringBuilder temp = new StringBuilder(255);
            string caption = "FilterMotor";
            int size = 255;
            TxtFile.GetPrivateProfileString(caption, "Exist", "", temp, size, filePath);
            device.HasCollimator = temp.ToString() == "0" ? false : true;
            TxtFile.GetPrivateProfileString(caption, "MotorID", "", temp, size, filePath);
            device.CollimatorElectricalCode = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString(caption, "MotorDir", "", temp, size, filePath);
            device.CollimatorElectricalDirect = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString(caption, "Speed", "", temp, size, filePath);
            device.CollimatorSpeed = Convert.ToInt32((temp.ToString()));
            TxtFile.GetPrivateProfileString(caption, "TagCount", "", temp, size, filePath);
            int tagCount = Convert.ToInt32(temp.ToString());
            if (tagCount > 0)
            {
                int[] Target = new int[tagCount];
                for (int i = 0; i < tagCount; ++i)
                {
                    Filter filter = Filter.New;
                    filter.Num = i;
                    TxtFile.GetPrivateProfileString(caption, "Tag" + (i + 1), "", temp, size, filePath);
                    filter.Step = Convert.ToInt32(temp.ToString());
                    device.Filter.Add(filter);
                }
            }
        }

        public void ReadFileDetector(string filePaht, Skyray.EDXRFLibrary.Device device)
        {
            device.Detector = Detector.New.Init(Skyray.EDXRFLibrary.DetectorType.Si, 5.895, 170);
        }

        public void ReadFileChamber(string filePath, Skyray.EDXRFLibrary.Device device)
        {

        }

        public void ReadFileRaytube(string filePath, Skyray.EDXRFLibrary.Device device)
        {
            device.Tubes = Tubes.New.Init(74, 19, 1.9, "SiO2", 40, 35, 14);
        }

        public void IsExistMoveAis(string filePath, Skyray.EDXRFLibrary.Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = TxtFile.GetPrivateProfileString(null, null, null, Buffer,
             Buffer.GetUpperBound(0), filePath);
            List<string> sectionList = new List<string>();
            TxtFile.GetStringsFromBuffer(Buffer, bufLen, sectionList);
            StringBuilder temp = new StringBuilder(255);
            int size = 255;
            string caption = string.Empty;
            if (sectionList.Contains("XMotor"))
            {
                caption = "XMotor";
                TxtFile.GetPrivateProfileString(caption, "Exist", "", temp, size, filePath);
                device.HasMotorX = temp.ToString() == "0" ? false : true;
                TxtFile.GetPrivateProfileString(caption, "MotorID", "", temp, size, filePath);
                device.MotorXCode = Convert.ToInt32(temp.ToString());
                TxtFile.GetPrivateProfileString(caption, "MotorDir", "", temp, size, filePath);
                device.MotorXDirect = Convert.ToInt32(temp.ToString());
                TxtFile.GetPrivateProfileString(caption, "Speed", "", temp, size, filePath);
                device.MotorXSpeed = Convert.ToInt32(temp.ToString());
            }
            if (sectionList.Contains("YMotor"))
            {
                caption = "YMotor";
                TxtFile.GetPrivateProfileString(caption, "Exist", "", temp, size, filePath);
                device.HasMotorY = temp.ToString() == "0" ? false : true;
                TxtFile.GetPrivateProfileString(caption, "MotorID", "", temp, size, filePath);
                device.MotorYCode = Convert.ToInt32(temp.ToString());
                TxtFile.GetPrivateProfileString(caption, "MotorDir", "", temp, size, filePath);
                device.MotorYDirect = Convert.ToInt32(temp.ToString());
                TxtFile.GetPrivateProfileString(caption, "Speed", "", temp, size, filePath);
                device.MotorYSpeed = Convert.ToInt32(temp.ToString());
            }
            if (sectionList.Contains("ZMotor"))
            {
                caption = "ZMotor";
                TxtFile.GetPrivateProfileString(caption, "Exist", "", temp, size, filePath);
                device.HasMotorZ = temp.ToString() == "0" ? false : true;
                TxtFile.GetPrivateProfileString(caption, "MotorID", "", temp, size, filePath);
                device.MotorZCode = Convert.ToInt32(temp.ToString());
                TxtFile.GetPrivateProfileString(caption, "MotorDir", "", temp, size, filePath);
                device.MotorZDirect = Convert.ToInt32(temp.ToString());
                TxtFile.GetPrivateProfileString(caption, "Speed", "", temp, size, filePath);
                device.MotorZSpeed = Convert.ToInt32(temp.ToString());
            }

        }

     


        #endregion

        #region IWorkCurve 成员

        public ElementList GetIntestedElementList(string filePath, string workCurveDir)
        {
            if (DllExport.saveToXML(filePath) == 1)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath.Trim()+".xml");
                ElementList elementList = ElementList.New;
                elementList.MatchSpecListIdStr = "";
                elementList.RefSpecListIdStr = "";
                XmlNodeList elemList = xmlDoc.GetElementsByTagName("Row");
                int maxLayer = 0;
                for (int j = 0; j < elemList.Count; j++)
                {
                    XmlNode tep = elemList[j].SelectSingleNode("Layer");
                    int level = Convert.ToInt32(tep.InnerText);
                    if (level > maxLayer)
                        maxLayer = level;
                }

                for (int i = 0; i < elemList.Count; i++)
                {
                    CurveElement elements = CurveElement.New.Init("", true, "", 0, 0, "", XLine.Ka, 0, 0, 0, 0, false, 0, 0, 0, 0.00,
                             IntensityWay.Reference, false, 0, 0, 0, CalculationWay.Insert, FpCalculationWay.LinearWithoutAnIntercept, ElementFlag.Calculated, LayerFlag.Calculated,
                                 ContentUnit.per, ThicknessUnit.um, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", false, Color.Red.ToArgb(), " ", 0, "", false, true, true, false, 1, 0, "", "", "", false, "");
                    XmlNode node = elemList[i];
                    XmlNode tep = node.SelectSingleNode("Caption");
                    elements.Caption = tep.InnerText;
                    tep = node.SelectSingleNode("Layer");
                    int level = Convert.ToInt32(tep.InnerText);

                    tep = node.SelectSingleNode("PeakLeft");
                    elements.PeakLow = Convert.ToInt32(tep.InnerText);
                    tep = node.SelectSingleNode("PeakRight");
                    elements.PeakHigh = Convert.ToInt32(tep.InnerText);
                    tep = node.SelectSingleNode("Color");
                    elements.Color = colors[i].ToArgb();
                    elements.SSpectrumData = "";
                    int useTime=0;
                    string str = DephiHelper.GetSpecInfo(workCurveDir + "\\PureSpec" + "\\" + elements.Caption + @".spe",out useTime);
                    if (str != null && useTime > 0)
                    {
                        elements.SSpectrumData = Helper.TransforToDivTime(str,useTime);
                    }
                    elements.ElementSpecName = elements.Caption + @".Spe";
                    elements.Formula = "";
                    switch (level)
                    {
                        case 0:
                            elements.LayerNumBackUp = Info.Substrate;
                            elements.LayerNumber = maxLayer+1;
                            break;
                        case 1:
                            elements.LayerNumBackUp = Info.FirstLayer;
                            elements.LayerNumber = 1;
                            break;
                        case 2:
                            elements.LayerNumBackUp = Info.SecondLayer;
                            elements.LayerNumber = 2;
                            break;
                        case 3:
                            elements.LayerNumBackUp = Info.ThirdLayer;
                            elements.LayerNumber = 3;
                            break;
                        case 4:
                            elements.LayerNumBackUp = Info.ForthLayer;
                            elements.LayerNumber = 4;
                            break;
                        case 5:
                            elements.LayerNumBackUp = Info.FirstLayer;
                            elements.LayerNumber = 5;
                            break;
                    }
                    elements.SReferenceElements = elements.Caption;


                    elementList.Items.Add(elements);
                }
                return elementList;
            }
            return null;
        }

        public List<Condition> ReadFileCondition(string filePath)
        {
            List<Condition> condition = new List<Condition>();
            Condition newCondition = Condition.New;
            newCondition.Name = "11";
            #region
            //StringBuilder temp = new StringBuilder(255);
            //int size = 255;
            //DeviceParameter deviceParams = DeviceParameter.New;
            //string caption = "DeviceParam";
            //TxtFile.GetPrivateProfileString(caption, "PrecTime", "", temp, size, filePath);
            //deviceParams.PrecTime = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "TubCurrent", "", temp, size, filePath);
            //deviceParams.TubCurrent = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "TubVoltage", "", temp, size, filePath);
            //deviceParams.TubVoltage = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "FilterIdx", "", temp, size, filePath);
            //deviceParams.FilterIdx = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "MinRate", "", temp, size, filePath);
            //deviceParams.MinRate = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "MaxRate", "", temp, size, filePath);
            //deviceParams.MaxRate = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "Vacuumed", "", temp, size, filePath);
            //deviceParams.IsVacuum = Convert.ToBoolean(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "AdjustRated", "", temp, size, filePath);
            //deviceParams.IsAdjustRate = Convert.ToBoolean(temp.ToString());

            //TxtFile.GetPrivateProfileString(caption, "BeginChann", "", temp, size, filePath);
            //deviceParams.BeginChann = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "EndChann", "", temp, size, filePath);
            //deviceParams.EndChann = Convert.ToInt32(temp.ToString());

            //InitParameter initParams = InitParameter.New;
            //caption = "InitParam";
            //TxtFile.GetPrivateProfileString(caption, "Elem", "", temp, size, filePath);
            //initParams.ElemName = temp.ToString();
            //TxtFile.GetPrivateProfileString(caption, "Chann", "", temp, size, filePath);
            //initParams.Channel = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "TubCurrent", "", temp, size, filePath);
            //initParams.TubCurrent = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "TubVoltage", "", temp, size, filePath);
            //initParams.TubVoltage = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "CoarseCode", "", temp, size, filePath);
            //initParams.Gain = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "FineCode", "", temp, size, filePath);
            //initParams.FineGain = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "Error", "", temp, size, filePath);
            //initParams.ChannelError = Convert.ToInt32(temp.ToString());

            //newCondition.DeviceParamList.Add(deviceParams);
            //newCondition.InitParam = initParams;
            #endregion
            DeviceParameter deviceParams = DeviceParameter.New;
            deviceParams.Name = "Condition1";
            deviceParams.PrecTime = 40;
            deviceParams.TubCurrent = 600;
            deviceParams.TubVoltage = 45;
            deviceParams.FilterIdx = 1;
            deviceParams.MinRate = 1000;
            deviceParams.MaxRate = 9000;
            deviceParams.IsVacuum = false;
            deviceParams.IsAdjustRate =false;

            deviceParams.BeginChann =20;
            deviceParams.EndChann =2000;

            InitParameter initParams = InitParameter.New;
            initParams.ElemName = "Ag";
            initParams.Channel = 1105;
            initParams.TubCurrent = 600;
            initParams.TubVoltage = 45;
            initParams.Gain = 60;
            initParams.FineGain =120;
            initParams.ChannelError = 0;

            newCondition.DeviceParamList.Add(deviceParams);
            newCondition.InitParam = initParams;
            newCondition.DemarcateEnergys.Add(DemarcateEnergy.New.Init("Ag", XLine.Ka, 22.1, 1105));
            newCondition.DemarcateEnergys.Add(DemarcateEnergy.New.Init("Cu", XLine.Ka, 8.041, 412));
            newCondition.Type = ConditionType.Normal;
            condition.Add(newCondition);
            return condition;

        }

        public Compounds ReadFileCompounds(string filePath)
        {
            throw new NotImplementedException();
        }

        public void ReadElementsSample(string filePath, CurveElement elements)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            int layLevel = elements.LayerNumber;
            //if (layLevel > 1)
            //{
                string filePath2 = filePath + "2";
                if (DllExport.saveToXML(filePath2) == 1)
                {

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath2 + ".xml");
                    ElementList elementList = ElementList.New;
                    XmlNodeList elemList = xmlDoc.GetElementsByTagName("Row");
                    int maxLevel = 0;
                    for (int i = 0; i < elemList.Count; i++)
                    {
                        XmlNode tep = elemList[i].SelectSingleNode("Layer");
                        int level = (!tep.InnerText.IsNullOrEmpty())?Convert.ToInt32(tep.InnerText):0;
                        if (level > maxLevel) 
                            maxLevel = level;
                    }
                    for (int i = 0; i < elemList.Count; i++)
                    {
                        XmlNode tep = elemList[i].SelectSingleNode("ElemCaption");

                        if (elements.Caption == tep.InnerText && Convert.ToInt32(elemList[i].SelectSingleNode("Layer").InnerText) == elements.LayerNumber)
                        {
                            StandSample standsample = StandSample.New;
                            tep = elemList[i].SelectSingleNode("Caption");
                            standsample.SampleName = tep.InnerText;
                             tep = elemList[i].SelectSingleNode("XValue");
                             standsample.X = 
                            (!tep.InnerText.IsNullOrEmpty())?tep.InnerText:"0";
                             tep = elemList[i].SelectSingleNode("YValue");
                            standsample.Z = (!tep.InnerText.IsNullOrEmpty())?tep.InnerText:"0";
                             tep = elemList[i].SelectSingleNode("Layer");
                             standsample.Layer = Convert.ToInt32(tep.InnerText);
                             standsample.ElementName = elements.Caption;
                             standsample.TotalLayer = maxLevel+1;
                             standsample.Active = true;
                             standsample.Level = Info.MultiLayer;
                             elements.Samples.Add(standsample);
                        }
                    }
                }

            //}
            //else if (layLevel == 1)
            //{
                  if (DllExport.saveToXML(filePath) == 1)
                  {
                      XmlDocument xmlDoc = new XmlDocument();
                      xmlDoc.Load(filePath + ".xml");
                      ElementList elementList = ElementList.New;
                      XmlNodeList elemList = xmlDoc.GetElementsByTagName("Row");
                      for (int i = 0; i < elemList.Count; i++)
                      {
                          XmlNode tep = elemList[i].SelectSingleNode("ElemCaption");
                          if (elements.Caption == tep.InnerText && Convert.ToInt32(elemList[i].SelectSingleNode("Layer").InnerText) == 1)
                          {
                              StandSample standsample = StandSample.New;
                              tep = elemList[i].SelectSingleNode("Caption");
                              standsample.SampleName = tep.InnerText;
                              tep = elemList[i].SelectSingleNode("XValue");
                              standsample.X = 
                              (!tep.InnerText.IsNullOrEmpty()) ? tep.InnerText : "0";
                              tep = elemList[i].SelectSingleNode("YValue");
                              standsample.Y = "0";
                              standsample.Z = 
                              (!tep.InnerText.IsNullOrEmpty()) ? tep.InnerText : "0";
                              tep = elemList[i].SelectSingleNode("Layer");
                              standsample.Layer = 
                              (!tep.InnerText.IsNullOrEmpty())?Convert.ToInt32(tep.InnerText):0;
                              standsample.ElementName = elements.Caption;
                              standsample.TotalLayer = 2;
                              standsample.Active = true;
                              standsample.Level = Info.SingleLayer;
                              elements.Samples.Add(standsample);
                          }
                      }

                  //}

                  }
        }

        public List<DemarcateEnergy> GetConditionDemarcate(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
             if (DllExport.saveToXML(filePath) == 1)
             {
                 XmlDocument xmlDoc = new XmlDocument();
                 xmlDoc.Load(filePath+".xml");
               
                   List<DemarcateEnergy> listEnergy = new List<DemarcateEnergy>();
                 XmlNodeList elemList = xmlDoc.GetElementsByTagName("Row");
                  for (int i = 0; i < elemList.Count; i++)
                  {
                      DemarcateEnergy energyDema = DemarcateEnergy.New;

                      XmlNode tep = elemList[i].SelectSingleNode("Caption");
                      energyDema.ElementName = tep.InnerText;

                      tep = elemList[i].SelectSingleNode("Chann");
                      energyDema.Channel = Convert.ToInt32((tep.InnerText));

                      tep = elemList[i].SelectSingleNode("Energy");
                      energyDema.Energy = Convert.ToDouble((tep.InnerText));
                       energyDema.Line  = XLine.Ka;
                       listEnergy.Add(energyDema);
                  }
                  DemarcateEnergyHelp.CalParam(listEnergy);//计算斜率与参数
                  return listEnergy;
             }
             return null;

        }

        public QualeElement GetQualeElement(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            StringBuilder temp = new StringBuilder(255);
            int size = 255;
            string caption = "Quale";
            QualeElement qualElement = QualeElement.New;
            TxtFile.GetPrivateProfileString(caption, "PeakWidth", "", temp, size, filePath);
            qualElement.WindowWidth = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString(caption, "RootDiff", "", temp, size, filePath);
            qualElement.ChannFWHM = Convert.ToInt32(temp.ToString());

            TxtFile.GetPrivateProfileString(caption, "PeakOffset", "", temp, size, filePath);
            qualElement.AreaLimt = Convert.ToInt32(temp.ToString());

            TxtFile.GetPrivateProfileString(caption, "RootRate", "", temp, size, filePath);
            qualElement.Trh1 = Convert.ToInt32(temp.ToString());

            TxtFile.GetPrivateProfileString(caption, "PeakThorn", "", temp, size, filePath);
            qualElement.ValleyDistance = Convert.ToInt32(temp.ToString());
            return qualElement;
        }

        public void CreateCurve(WorkCurve curve, string filePath, List<Condition> conditions)
        {
            //if (curve == null || !System.IO.File.Exists(filePath))
            //{
            //    return;
            //}
            curve.CalcType = CalcType.EC;
            curve.FuncType = FuncType.Thick;
            curve.Condition = conditions[0];
            curve.ConditionName = curve.Condition.Name;
        }

        #endregion

        #region ISpectrums 成员

        public void GetSpectrum(System.IO.FileInfo filePath, SpecType type, SpecListEntity specList, List<Device> deviceList)
        {
            //DephiHelper.GetDeviceParamsFromSpec(filePath, specList, type);
        }
        public void GetSpectrum(System.IO.FileInfo filePath, SpecType type, SpecListEntity specList, Condition condition)
        {
            DephiHelper.GetDeviceParamsFromSpec(filePath, specList);
            specList.InitParam = condition.InitParam.ConvertToNewEntity();
            WorkCurveHelper.DataAccess.Save(specList);
        }
        #endregion
    }
}
