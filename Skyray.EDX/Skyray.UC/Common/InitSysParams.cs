using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Lephone.Util.Setting;
using ZedGraph;
using Skyray.Language;
using Skyray.EDXRFLibrary.Define;
using System.Threading;
using System.Data;
using System.Data.SQLite;
using System.Xml;
using Microsoft.Win32;
using Lephone.Data.Common;
using System.Text.RegularExpressions;
using System.Configuration;
using Skyray.EDX.Common.ReportHelper;
using Skyray.EDXRFLibrary.Spectrum;


namespace Skyray.UC
{
    /// <summary>
    /// 软件初始化功能
    /// </summary>
    public class InitSysParams
    {
       
        private static void ReadUIFile(ref string font,ref string fontStyle,ref string fontSize,ref string imageSize,string name)
        {
            string strFilePath = AppDomain.CurrentDomain.BaseDirectory + "UI.xml";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(strFilePath);
            font = xmldoc.SelectSingleNode("UI/"+name+"/Font").InnerText;
            fontStyle = xmldoc.SelectSingleNode("UI/" + name + "/FontStyle").InnerText;
            fontSize = xmldoc.SelectSingleNode("UI/" + name + "/FontSize").InnerText;
            imageSize = xmldoc.SelectSingleNode("UI/" + name + "/ImageSize").InnerText;
        }

        /// <summary>
        /// 是否保存数据库2   addby 150923 pf
        /// </summary>
        public static void LoadDataApp()
        {
            string strPath = Application.StartupPath + "\\DBConnection.ini";
            string strData = "0";
            if (File.Exists(strPath))
            {
                System.Text.StringBuilder tempbuilder = new System.Text.StringBuilder(255);
                Skyray.API.WinMethod.GetPrivateProfileString("Param", "ExportToSQL", "0", tempbuilder, 255, strPath);
                strData = tempbuilder.ToString();
            }
            //WorkCurveHelper.IsSaveToDataBase = Convert.ToInt32(strData) == 2 ? true : false; // Convert.ToBoolean(strData);
            WorkCurveHelper.IsSaveToDataBase = (strData == "2" || strData == "3");
            WorkCurveHelper.AutoUploadVisible = strData == "3";
        }

        public static void InitLog()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(CustomExceptionHandler.OnThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CustomExceptionHandler.CurrentDomain_UnhandledException);
            Log.InitLog();//日志记录
        }

        public static void InitLang()
        {
            string font = string.Empty;
            string fontStyle = string.Empty;
            string fontSize = string.Empty;
            string imageSize = string.Empty;
            ReadUIFile(ref font, ref fontStyle, ref fontSize, ref imageSize, "FrmThick");
            //if (DifferenceDevice.IsAnalyser)
            //    ReadUIFile(ref font, ref fontStyle, ref fontSize, ref imageSize, "Frm3000");
            //else if (DifferenceDevice.IsThick)
            //    ReadUIFile(ref font, ref fontStyle, ref fontSize, ref imageSize, "FrmThick");
            //else if (DifferenceDevice.IsRohs)
            //    ReadUIFile(ref font, ref fontStyle, ref fontSize, ref imageSize, "FormRohs");
            //else
            //    ReadUIFile(ref font, ref fontStyle, ref fontSize, ref imageSize, "FormXRF");
            if (!string.IsNullOrEmpty(font) && !string.IsNullOrEmpty(fontStyle) && !string.IsNullOrEmpty(fontSize))
            {
                Font newFont = new Font(font, int.Parse(fontSize), (FontStyle)Enum.Parse(typeof(FontStyle), fontStyle));
                LanguageModel.newFont = newFont;
            }
            if (!string.IsNullOrEmpty(imageSize))
                LanguageModel.imageSize = int.Parse(imageSize);
            Skyray.Language.Lang.Model = new LanguageModel();//实例多语言对象
            Skyray.Language.Lang.Model.InfoTypes.AddRange(new Type[]
            {
                typeof(Skyray.EDX.Common.Info),
                typeof(Skyray.EDXRFLibrary.LibraryInfo),
                typeof(Skyray.Print.PrintInfo),
                typeof(Skyray.Controls.CommonsInfo),
                typeof(Skyray.Controls.Extension.XRFInfo),
                typeof(Skyray.Camera.CameraInfo)
            });
            if (!Skyray.Language.Param.SaveTextToDB)
            {
                Skyray.Language.Lang.Model.LangData = Func.GetLanguageData(Languages.FindOne(w => w.IsCurrentLang == true).Id);
                Skyray.Language.Lang.Model.ChangeLanguage();
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            bool bStatus = true;
            #region InitLog
            //Application.ThreadException += new ThreadExceptionEventHandler(CustomExceptionHandler.OnThreadException);
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CustomExceptionHandler.CurrentDomain_UnhandledException);
            //Log.InitLog();//日志记录
            #endregion
            //20110601 paul 旧表新增字段，进行字段添加
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            LoadAppParamsInit(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml");  //11ms
           
            //LoadDataApp(); 211110
            DataBaseHelper.UpdateDatabase(Lephone.Data.DbEntry.Context);   //70ms
           
            Ranges ranges = new Ranges();//界面输入控件边界信
            string[] dbFiles = new string[] { "AppParams.xml", "Parameter.xml", "Camera.xml", "EDXT.sdb", "Language.sdb" }; 
            foreach (var file in dbFiles)
            {
                if (File.Exists(file)) File.SetAttributes(file, FileAttributes.Normal);//检查数据库属性
            }
           
            Atoms.AtomList = Atom.FindAll();//加载原子信息
           
            WorkCurveHelper.ArmInfo = GetARMInfo();//获取设备信息
            WorkCurveHelper.DefaultElementColor = GetElementColor();//获取感兴趣元素的颜色信息
            WorkCurveHelper.DefaultVirtualColor = GetVisualColor();//获取对比谱的颜色信息
            if (DifferenceDevice.IsRohs || WorkCurveHelper.isThickLimit == 1 ||WorkCurveHelper.isShowXRFStandard ==1)
            {
                List<CustomStandard> lc = CustomStandard.Find(c => c.CurrentSatadard == true);
                //List<CustomStandard> lc = CustomStandard.FindAll(delegate(CustomStandard v) { return v.CurrentSatadard == true; });
                WorkCurveHelper.CurrentStandard = (lc == null || lc.Count == 0 ? null : lc[0]);//获取当前标准
            }
            Device d = Device.FindOne(x => x.IsDefaultDevice == true);//取得设备
            int iCount = 0;
            if (d == null && Msg.Show(Info.AddDeviceInfo) == DialogResult.OK)//添加新设备
            {
                while (d == null)
                {
                    TargetDic tar = new TargetDic(false);
                    //打开设备设置界面
                    WorkCurveHelper.OpenUC(new FrmDevice(), false);
                    //当前设备
                    d = Device.FindOne(x => x.IsDefaultDevice == true);
                    iCount++;
                    if (iCount >= 3)
                    {
                        Msg.Show(Info.MustAddDeviceInfo, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                }
            }
           
            if(d==null)
               d = Device.FindAll()[0];
            WorkCurveHelper.DeviceCurrent = d;//设置当前设备
            LoadRohsParams(Application.StartupPath + "\\Parameter.xml");
            TargetDic atar = new TargetDic();

            ///获取历史记录参数
            ReportTemplateHelper.GetParameter();
            ExcelTemplateParams.LoadExcelTemplateParams(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml");  //pf 已减少时间 0.5ms
            WorkCurveHelper.VirtualSpecList = new List<SpecListEntity>();//实例对比谱
            WorkCurveHelper.VirtualSpecListAdditional =new Dictionary<string,bool>();
            InitCondition(WorkCurveHelper.DeviceCurrent,false);
            ReadNTFile();
            ReadTitleIco();
            GetSurfaceSource();
            WorkCurveHelper.RegisterId = ReadRegister();
            InitTipSoundAndHotKey(WorkCurveHelper.DeviceCurrent);
            return bStatus;
        }

        public static string ReadRegister()
        {
            string cipher = string.Empty;
            try
            {
                string filename = Application.StartupPath + @"/Register.lic";
                if (System.IO.File.Exists(filename))
                {
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        cipher = sr.ReadToEnd();
                    }
                }
            }
            catch
            {
                cipher = string.Empty;
            }
             return cipher;
        }

    

        private static void LoadRohsParams(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode node = doc.SelectSingleNode("Parameter/System/HalfWidth");
                double HalfWidth = node == null ? 18 : double.Parse(node.InnerText);
                node = doc.SelectSingleNode("Parameter/System/PeakChannel");
                double PeakChannel = node == null ? 1105 : double.Parse(node.InnerText);
                double FixGaussDelta = (HalfWidth * 1.0) / (PeakChannel * 1.0);


                XmlNodeList xmlCalibrationList = doc.SelectNodes("Parameter/DataParam/NiFormulaParam");
                if (xmlCalibrationList != null)
                {
                    foreach (XmlNode xTemp in xmlCalibrationList)
                    {
                        if (xTemp.Name == "NiFormulaParam")
                        {
                            WorkCurveHelper.NiCuNiParam.aValue = xTemp.Attributes["aValue"].InnerText ==null ? 0.8226 : double.Parse(xTemp.Attributes["aValue"].InnerText);
                            WorkCurveHelper.NiCuNiParam.nValue = xTemp.Attributes["nValue"].InnerText == null ? -2.307 : double.Parse(xTemp.Attributes["nValue"].InnerText);
                            WorkCurveHelper.NiCuNiParam.kValue = xTemp.Attributes["kValue"].InnerText == null ? 5.747 : double.Parse(xTemp.Attributes["kValue"].InnerText);
                            WorkCurveHelper.NiCuNiParam.bValue = xTemp.Attributes["bValue"].InnerText == null ? -1.644 : double.Parse(xTemp.Attributes["bValue"].InnerText);
                            WorkCurveHelper.NiCuNiParam.limit = xTemp.Attributes["limit"].InnerText == null ? 0.5 : double.Parse(xTemp.Attributes["limit"].InnerText);
                            break;
                        }
                       
                    }
                }
                else
                {
                    WorkCurveHelper.NiCuNiParam.aValue = 0.8226;
                    WorkCurveHelper.NiCuNiParam.nValue = -2.307;
                }


                if (!Double.IsNaN(FixGaussDelta) && FixGaussDelta > 0 && WorkCurveHelper.DeviceCurrent.Detector.FixGaussDelta != FixGaussDelta)
                {
                    WorkCurveHelper.DeviceCurrent.Detector.FixGaussDelta = FixGaussDelta;
                    string sql = "update Detector set FixGaussDelta =" + FixGaussDelta + " where Id=" + WorkCurveHelper.DeviceCurrent.Detector.Id;
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                }
            }
            catch
            {
                WorkCurveHelper.NiCuNiParam.aValue = 0.8226;
                WorkCurveHelper.NiCuNiParam.nValue = -2.307;
            }
        }

        public static void LoadAppParamsInit(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode node = doc.SelectSingleNode("application/SoftWareCaculate/ContentBit");
                int.TryParse(node == null ? "0" : node.InnerText, out WorkCurveHelper.SoftWareContentBit);
                Thread.SetData(Thread.GetNamedDataSlot("decimals"), WorkCurveHelper.SoftWareContentBit);

                node = doc.SelectSingleNode("application/SoftWareCaculate/IntensityBit");
                int.TryParse(node == null ? WorkCurveHelper.SoftWareContentBit.ToString() : node.InnerText, out WorkCurveHelper.SoftWareIntensityBit);//追加强度的计算精度

                node = doc.SelectSingleNode("application/SoftWareCaculate/ThickBit");
                int.TryParse(node == null ? WorkCurveHelper.SoftWareContentBit.ToString() : node.InnerText, out WorkCurveHelper.ThickBit);//追加强度的计算精度

                node = doc.SelectSingleNode("application/SpecExport/Type");
                int.TryParse(node == null ? "0" : node.InnerText, out WorkCurveHelper.SpecExportType);

                node = doc.SelectSingleNode("application/SpecExport/DelAfterExport");
                WorkCurveHelper.DelAfterExport = node == null ? false : Convert.ToBoolean(node.InnerText);

                XmlNode xmlTemp = doc.SelectSingleNode("application/IGetGradeContent");//武大牌号库数据优化

                Helper.IGetGradeContent = Convert.ToBoolean(xmlTemp == null ? 0 : Convert.ToInt32(xmlTemp.InnerText));

                XmlNodeList xmlCalibrationList = doc.SelectNodes("application/PeakCalibrate/Item");
                if (xmlCalibrationList != null)
                {
                    foreach (XmlNode xTemp in xmlCalibrationList)
                    {
                        WorkCurveHelper.IPeakCalibration = Convert.ToBoolean(int.Parse(xTemp.Attributes["iCalibration"] == null ? "0" : xTemp.Attributes["iCalibration"].InnerText));
                        WorkCurveHelper.FastLimit = int.Parse(xTemp.Attributes["FastLimiter"] == null ? "0" : xTemp.Attributes["FastLimiter"].InnerText);
                        WorkCurveHelper.PeakErrorChannel = int.Parse(xTemp.Attributes["ErrorChannel"] == null ? "0" : xTemp.Attributes["ErrorChannel"].InnerText);
                        WorkCurveHelper.InitChannelError = double.Parse(xTemp.Attributes["InitChannelError"] == null ? "0" : xTemp.Attributes["InitChannelError"].InnerText);
                        WorkCurveHelper.IShowQuickInfo = Convert.ToBoolean(int.Parse(xTemp.Attributes["ShowQuickInfo"] == null ? "0" : xTemp.Attributes["ShowQuickInfo"].InnerText));
                        break;
                    }
                }
                node = doc.SelectSingleNode("application/ExcelPath/Path");
                WorkCurveHelper.ExcelPath = node == null ? "" : node.InnerText;

                node = doc.SelectSingleNode("application/IsExistsKey/Key");
                WorkCurveHelper.IsExistsLock = node == null ? false : Convert.ToBoolean(int.Parse(node.InnerText));

                node = doc.SelectSingleNode("application/MoveStationVisible/Distance");
                WorkCurveHelper.MoveStationVisible = node == null ? 0 : double.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/DeviceName");
                WorkCurveHelper.deviceNameList = node == null ? null : node.InnerText.Split(',').ToList();

                node = doc.SelectSingleNode("application/TestParams/ExcelCount");
                WorkCurveHelper.PrintExcelCount = node == null ? WorkCurveHelper.PrintExcelCount : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/AdjustTime");
                WorkCurveHelper.AdjustTime = node == null ? 5 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/AdjustInterva");
                WorkCurveHelper.AdjustInterval = node == null ? 30 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/PumpShowProgress");
                WorkCurveHelper.PumpShowProgress = node == null ? false : Convert.ToBoolean(int.Parse(node.InnerText));

                node = doc.SelectSingleNode("application/TestParams/MatchMode");
                WorkCurveHelper.IsInnerMatch = node == null ? WorkCurveHelper.IsInnerMatch : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/IsUseProcessData/Key");
                SpecHelper.IsSmoothProcessData = node == null ? SpecHelper.IsSmoothProcessData : Convert.ToBoolean(int.Parse(node.InnerText));

                node = doc.SelectSingleNode("application/InitParams/InitTime");
                WorkCurveHelper.InitTime = node == null ? WorkCurveHelper.InitTime : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/InitParams/InitCurrents");
                WorkCurveHelper.InitCurrentList = node == null ? Array.ConvertAll("100,300,600".Split(','), p => Convert.ToInt32(p)).ToList() : Array.ConvertAll(node.InnerText.Split(','), p => Convert.ToInt32(p)).ToList();  //node.InnerText.Split(',').ToList(int);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/InitParams/InitCurrents", new Dictionary<string, string>()
                    {
                        {
                            "InitCurrents", "100,300,600"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/GradeDataBase/MatchingLowerBound");
                DifferenceDevice.MatchNum = node == null ? DifferenceDevice.MatchNum : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/IsOpenRegister/Key");
                WorkCurveHelper.IsOpenRegister = node == null ? WorkCurveHelper.IsOpenRegister : Convert.ToBoolean(int.Parse(node.InnerText));

                node = doc.SelectSingleNode("application/Match/AutoMatch");
                WorkCurveHelper.IsMatchAuto = node == null ? true : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/Match/MatchType");
                WorkCurveHelper.MatchType = node == null ? 0 : int.Parse(node.InnerText);//追加三元素匹配

                node = doc.SelectSingleNode("application/Match/MatchBaseLeft");
                WorkCurveHelper.MatchBaseLeft = node == null ? 0 : int.Parse(node.InnerText);//追加三元素匹配

                node = doc.SelectSingleNode("application/Match/MatchBaseRight");
                WorkCurveHelper.MatchBaseRight = node == null ? 0 : int.Parse(node.InnerText);//追加三元素匹配

                node = doc.SelectSingleNode("application/Match/MatchBaseRatio");
                WorkCurveHelper.MatchBaseRatio = node == null ? 0 : double.Parse(node.InnerText);//追加三元素匹配

                node = doc.SelectSingleNode("application/Match/MatchBaseKRatio");
                WorkCurveHelper.MatchBaseKRatio = node == null ? 1 : double.Parse(node.InnerText);//追加三元素匹配

                node = doc.SelectSingleNode("application/Match/MatchBaseLRatio");
                WorkCurveHelper.MatchBaseLRatio = node == null ? 2 : double.Parse(node.InnerText);//追加三元素匹配

                node = doc.SelectSingleNode("application/Match/MatchHighSubstrateLeft");
                WorkCurveHelper.MatchHighSubstrateLeft = node == null ? 50 : int.Parse(node.InnerText);//追加三元素匹配

                node = doc.SelectSingleNode("application/Match/MatchHighSubstrateRight");
                WorkCurveHelper.MatchHighSubstrateRight = node == null ? 400 : int.Parse(node.InnerText);//追加三元素匹配

                node = doc.SelectSingleNode("application/Match/MatchPrimaryElemRatio");
                WorkCurveHelper.MatchMinorElemRatio = node == null ? 0.2 : double.Parse(node.InnerText);//追加三元素匹配

                //2014-02-08
                node = doc.SelectSingleNode("application/Match/DelElemType");
                WorkCurveHelper.DelElemType = node == null ? 0 : int.Parse(node.InnerText);//改进三元素匹配
                node = doc.SelectSingleNode("application/Match/DelElemThreshold");
                WorkCurveHelper.DelElemThreshold = node == null ? 0.1 : double.Parse(node.InnerText);//改进三元素匹配
                node = doc.SelectSingleNode("application/Match/IsDelElemByQuale");
                WorkCurveHelper.IsDelElemByQuale = node == null ? false : Convert.ToBoolean(node.InnerText);//改进三元素匹配

                node = doc.SelectSingleNode("application/DeviceTypeForChamber");
                WorkCurveHelper.DeviceTypeForChamber = node == null ? string.Empty : node.InnerText;//追加NewEdx6000B

                node = doc.SelectSingleNode("application/ShutterSetting/IsCloseShutterAfterTest");
                WorkCurveHelper.IsCloseShutterAfterTest = node == null ? false : bool.Parse(node.InnerText);//追加滤光片代替光闸
                node = doc.SelectSingleNode("application/ShutterSetting/ResponseFilterIndex");
                WorkCurveHelper.ResponseFilterIndex = node == null ? 1 : int.Parse(node.InnerText);//追加滤光片代替光闸  IsNewDp5NetUse

                node = doc.SelectSingleNode("application/NewDp5Net/IsUse");
                WorkCurveHelper.IsNewDp5NetUse = node == null ? false : bool.Parse(node.InnerText);//追加新Dp5功能

                node = doc.SelectSingleNode("application/Match/DirectCaculate");
                WorkCurveHelper.IsDirectCaculate = node == null ? false : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/StopTestIsSave");
                WorkCurveHelper.StopTestIsSave = node == null ? false : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/AutoFocus");
                WorkCurveHelper.IsAutoFocus = node == null ? false : Convert.ToBoolean(int.Parse(node.InnerText));

                SpecHelper.SmoothTimes = int.Parse(ConfigurationSettings.AppSettings["SmoothTimes"]);

                node = doc.SelectSingleNode("application/TestParams/AutoAscend");
                WorkCurveHelper.IsAutoAscend = node == null ? false : Convert.ToBoolean(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/AscendStepZ");
                WorkCurveHelper.AscendStepZ = node == null ? WorkCurveHelper.AscendStepZ : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/ResultLowLimit");
                WorkCurveHelper.ResultLowLimit = node == null ? 0.0 : double.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/AutoUpload");
                WorkCurveHelper.IsAutoUpload = node == null ? false : Convert.ToBoolean(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/SetXRange/IsSet");
                WorkCurveHelper.IsSetXRange = node == null ? false : Convert.ToBoolean(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/TestParams/SetXRange/IsSet", new Dictionary<string, string>()
                    {
                        {
                            "IsSet", "False"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/TestParams/SetXRange/Min");
                WorkCurveHelper.XRange[0] = node == null ? 0d : double.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/TestParams/SetXRange/Min", new Dictionary<string, string>()
                    {
                        {
                            "Min", "0"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/TestParams/SetXRange/Max");
                WorkCurveHelper.XRange[1] = node == null ? 4096d : double.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/TestParams/SetXRange/Max", new Dictionary<string, string>()
                    {
                        {
                            "Max", "4096"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/WriteNetportLog");
                WorkCurveHelper.WriteNetportLog = (node == null ? false : bool.Parse(node.InnerText));
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/WriteNetportLog", new Dictionary<string, string>()
                    {
                        {
                            "WriteNetportLog", "False"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/ReConnectionSettings/ReConnectCondition");
                int.TryParse(node == null ? "3" : node.InnerText, out WorkCurveHelper.ReConnectCondition);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/ReConnectionSettings/ReConnectCondition", new Dictionary<string, string>()
                    {
                        {
                            "ReConnectCondition", "3"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/ReConnectionSettings/RetryGetVersionTimes");
                int.TryParse(node == null ? "5" : node.InnerText, out WorkCurveHelper.RetryGetVersionTimes);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/ReConnectionSettings/RetryGetVersionTimes", new Dictionary<string, string>()
                    {
                        {
                            "RetryGetVersionTimes", "5"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/AutoDetection/IsDetection");
                WorkCurveHelper.IsAutoDetection = node == null ? false : Convert.ToBoolean(int.Parse(node.InnerText));

                node = doc.SelectSingleNode("application/AutoDetection/OnStartDetection");
                WorkCurveHelper.IsOnStartDetedtion = node == null ? false : Convert.ToBoolean(int.Parse(node.InnerText));

                node = doc.SelectSingleNode("application/AutoDetection/DetectionType");
                WorkCurveHelper.DetectionType = node == null ? 0 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/EDXRFAlert/ExceptElement");
                WorkCurveHelper.EDXRFAlertExceptElement = node == null ? WorkCurveHelper.EDXRFAlertExceptElement : node.InnerText;

                node = doc.SelectSingleNode("application/AutoSaveSpectrumImage/DefaultSave");
                WorkCurveHelper.IsSaveSpectrumImage = node == null ? true : Convert.ToBoolean(int.Parse(node.InnerText));

                node = doc.SelectSingleNode("application/SoftWareCaculate/AreaDensity");
                WorkCurveHelper.AreaDensity = node == null ? 1 : double.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/SoftWareCaculate/AreaDensityCoef");
                WorkCurveHelper.AreaDensityCoef = node == null ? 1 : double.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/IndiaAuWarning/Warning");
                WorkCurveHelper.IndiaAuWarning = node == null ? true : Convert.ToBoolean(int.Parse(node.InnerText));

                node = doc.SelectSingleNode("application/IndiaAuWarning/Max");
                WorkCurveHelper.IndiaAuWarningMax = node == null ? 0.7 : double.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/IndiaAuWarning/Min");
                WorkCurveHelper.IndiaAuWarningMin = node == null ? 0 : double.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/IndiaAuWarning/WarningTime");
                WorkCurveHelper.IndiaAuWarningTime = node == null ? 5 : int.Parse(node.InnerText);


                node = doc.SelectSingleNode("application/SavePath/IsSaveSpecData");
                WorkCurveHelper.IsSaveSpecData = (node == null || string.IsNullOrEmpty(node.InnerText) ? true : (node.InnerText == "1" ? true : false));

                node = doc.SelectSingleNode("application/SavePath/SaveSpecDataPath");
                WorkCurveHelper.SaveSpectrumPath = (node == null || string.IsNullOrEmpty(node.InnerText) ? Application.StartupPath + "\\Spectrum" : node.InnerText);

                if (!Directory.Exists(WorkCurveHelper.SaveSpectrumPath)) WorkCurveHelper.SaveSpectrumPath = Application.StartupPath + "\\Spectrum";
                //node = doc.SelectSingleNode("application/SavePath/IsSaveSampleImage");
                //WorkCurveHelper.IsSaveSample = (node == null || string.IsNullOrEmpty(node.InnerText) ? true : (node.InnerText == "1" ? true : false));

                node = doc.SelectSingleNode("application/SavePath/SaveSampleImagePath");
                WorkCurveHelper.SaveSamplePath = (node == null || string.IsNullOrEmpty(node.InnerText) ? Application.StartupPath + "\\Image\\SampleImage" + "\\" + (DifferenceDevice.IsXRF ? "EDXX" : (DifferenceDevice.IsRohs ? "EDXR" : (DifferenceDevice.IsThick ? "EDXT" : "EDX3000"))) : node.InnerText);
                if (!Directory.Exists(WorkCurveHelper.SaveSamplePath)) WorkCurveHelper.SaveSamplePath = Application.StartupPath + "\\Image\\SampleImage" + "\\" + (DifferenceDevice.IsXRF ? "EDXX" : (DifferenceDevice.IsRohs ? "EDXR" : (DifferenceDevice.IsThick ? "EDXT" : "EDX3000")));

                node = doc.SelectSingleNode("application/SavePath/IsSaveSpecImage");
                WorkCurveHelper.IsSaveSpectrumImage = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);

                node = doc.SelectSingleNode("application/SavePath/SaveSpecImagePath");
                WorkCurveHelper.SaveGraphicPath = (node == null || string.IsNullOrEmpty(node.InnerText) ? Application.StartupPath + "\\Image\\SpecImage" + "\\" + (DifferenceDevice.IsXRF ? "EDXX" : (DifferenceDevice.IsRohs ? "EDXR" : (DifferenceDevice.IsThick ? "EDXT" : "EDX3000"))) : node.InnerText);
                if (!Directory.Exists(WorkCurveHelper.SaveGraphicPath)) WorkCurveHelper.SaveGraphicPath = Application.StartupPath + "\\Image\\SpecImage" + "\\" + (DifferenceDevice.IsXRF ? "EDXX" : (DifferenceDevice.IsRohs ? "EDXR" : (DifferenceDevice.IsThick ? "EDXT" : "EDX3000")));

                node = doc.SelectSingleNode("application/SavePath/IsSaveHistoryRecord");
                WorkCurveHelper.IsSaveHistory = (node == null || string.IsNullOrEmpty(node.InnerText) ? true : int.Parse(node.InnerText) == 1 ? true : false);


                node = doc.SelectSingleNode("application/SpecExport/SaveType");
                WorkCurveHelper.SaveType = (node == null || string.IsNullOrEmpty(node.InnerText)) ? 0 : Convert.ToInt32(node.InnerText); ;
                if (WorkCurveHelper.SaveType == 0)
                {
                    WorkCurveHelper.DataAccess = new SQliteDAL();
                    if (WorkCurveHelper.DeviceCurrent != null)
                        WorkCurveHelper.DataAccess.SetCurDeviceName(WorkCurveHelper.DeviceCurrent.Name);
                }
                else
                {

                    DirectoryInfo curentApp = new DirectoryInfo(WorkCurveHelper.SaveSpectrumPath);//保存谱路径
                    if (!curentApp.Exists)
                        curentApp.Create();
                    if (WorkCurveHelper.SpecExportType == 0)
                        WorkCurveHelper.DataAccess = new FileDAL(WorkCurveHelper.SaveSpectrumPath);
                    else
                        WorkCurveHelper.DataAccess = new PlainTextDAL(WorkCurveHelper.SaveSpectrumPath);
                }

                node = doc.SelectSingleNode("application/Report/IsPopUpReportOpen");
                WorkCurveHelper.IsPopUpReportOpen = node == null ? true : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/Report/ReportStyle");
                WorkCurveHelper.ReportVkOrVe = node == null ? 1 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/Report/SpecType");
                WorkCurveHelper.SpecType = node == null ? 0 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/Report/IsReoprtShowQuality");
                WorkCurveHelper.IsReoprtShowQualityElem = node == null ? false : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/Report/IsReoprtShowAxias");
                WorkCurveHelper.IsReoprtShowAxias = node == null ? false : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/Report/ReportSaveScreen");
                WorkCurveHelper.ReportSaveScreen = node == null ? false : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/Report/PrintName");
                WorkCurveHelper.PrintName = node == null ? "HDT312A" : node.InnerText;

                node = doc.SelectSingleNode("application/Report/PrinterType");
                WorkCurveHelper.PrinterType = node == null ? 0 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/AdjustType");
                WorkCurveHelper.AdjustType = node == null ? 0 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/IsEditRefCoefficient");
                WorkCurveHelper.IsEditRefCoefficient = node == null ? false : bool.Parse(node.InnerText);


                node = doc.SelectSingleNode("application/TestParams/AdjustExceptTime");
                WorkCurveHelper.AdjustExceptTime = node == null ? 3 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/EDX3200L/Is3200L");
                WorkCurveHelper.Is3200L = node == null ? false : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/EDX3200L/VaccumDiffer");
                WorkCurveHelper.VaccumDiffer = node == null ? 3 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/OpenHistoryRecordType/DefaultCurveType");
                WorkCurveHelper.HistoryDefaultCurveType = node == null ? 0 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/CurrentProcess/ShowType");
                WorkCurveHelper.CurrentProcessType = node == null ? 1 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/MainElementInfo/ZeroContAlert");
                WorkCurveHelper.ZeroMainElemAlertShow = node == null ? true : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/FPThick/AxisZSpeedMode");
                WorkCurveHelper.AxisZSpeedMode = node == null ? 0 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/FPThick/ThicknessLimit");
                WorkCurveHelper.ThicknessLimit = node == null ? 100 : double.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/FPThick/Fast");
                WorkCurveHelper.ZFast = node == null ? 150 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/FPThick/Middle");
                WorkCurveHelper.ZMiddle = node == null ? 90 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/FPThick/Slow");
                WorkCurveHelper.ZSlow = node == null ? 20 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/Chart/GrowStyle");
                WorkCurveHelper.Growstyle = node == null ? 20 : int.Parse(node.InnerText);


                //追加PK功能
                node = doc.SelectSingleNode("application/PKValue/IsCarrayMatchPK");
                WorkCurveHelper.IsCarrayMatchPKSetting = node == null ? false : bool.Parse(node.InnerText);
                node = doc.SelectSingleNode("application/PKValue/MatchPKValue");
                WorkCurveHelper.CarrayMatchPKValue = node == null ? 99.99 : double.Parse(node.InnerText);
                //PK总是开启
                node = doc.SelectSingleNode("application/PKValue/IsMatchAlways");
                WorkCurveHelper.IsMatchAlways = node == null ? false : bool.Parse(node.InnerText);
                WorkCurveHelper.IsCarrayMatchPK = WorkCurveHelper.IsCarrayMatchPKSetting && WorkCurveHelper.IsMatchAlways;

                node = doc.SelectSingleNode("application/PureAu/ResolveFactor");
                WorkCurveHelper.ResolveFactor = node == null ? 1 : (double.Parse(node.InnerText) > 1 ? 1 : double.Parse(node.InnerText));
                node = doc.SelectSingleNode("application/PureAu/ResolveCount");
                WorkCurveHelper.intResolve = node == null ? 135 : double.Parse(node.InnerText);
                node = doc.SelectSingleNode("application/DoorSetting/CloseType");
                WorkCurveHelper.DoorCloseType = node == null ? 0 : int.Parse(node.InnerText);
                //追加热键保存
                node = doc.SelectSingleNode("application/SaveHistoryToDatabase/SaveHotKey");
                WorkCurveHelper.SaveHistoryKeys = (node == null || string.IsNullOrEmpty(node.InnerText) ? Keys.None : (Keys)Enum.Parse(typeof(Keys), node.InnerText));
                node = doc.SelectSingleNode("application/IsResultEdit");
                WorkCurveHelper.IsResultEdit = node == null ? true : bool.Parse(node.InnerText);
                node = doc.SelectSingleNode("application/Report/ErrorType");
                WorkCurveHelper.ErrorType = node == null ? 0 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/RohsShowCamerInMain");
                WorkCurveHelper.RohsShowCamerInMain = node == null ? false : bool.Parse(node.InnerText);

                ///开盖判断真空度
                node = doc.SelectSingleNode("application/CoverSetting/VacummOpen");
                WorkCurveHelper.SetCoverOpen = node == null ? 0 : int.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/CoverSetting/VacummNumber");
                WorkCurveHelper.VacummNo = node == null ? 0 : double.Parse(node.InnerText);

                //加密显示
                node = doc.SelectSingleNode("application/ShowEncryptButton");
                WorkCurveHelper.IsShowEncryptButton = node == null ? false : Convert.ToBoolean(int.Parse(node.InnerText));

                //编号精确到天
                node = doc.SelectSingleNode("application/LiteralityToDay");
                WorkCurveHelper.IsLiteralityToDay = node == null ? false : Convert.ToBoolean(int.Parse(node.InnerText));

                //以主元素的结果计算K值
                //node = doc.SelectSingleNode("application/CalcKaratWithMainElement");
                //WorkCurveHelper.IsCalcKaratWithMainElement = node == null ? false : Convert.ToBoolean(int.Parse(node.InnerText));


                node = doc.SelectSingleNode("application/Report/IsAutoSaveReport");
                WorkCurveHelper.SaveReportType = node.Attributes["ReportEXCEL"].Value.ToString() == "1"
                                                     ? 0
                                                     : (node.Attributes["ReportPDF"].Value.ToString() == "1" ? 1 : 0);

                node = doc.SelectSingleNode("application/TestParams/TestOnButtonPressedEnabled");
                WorkCurveHelper.TestOnButtonPressedEnabled = node == null ? false : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/TestParams/ButtonDirectRun");
                WorkCurveHelper.ButtonDirectRun = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, "TestParams", "ButtonDirectRun", "false");
                }
                node = doc.SelectSingleNode("application/OpenHistoryRecordType/HistoryAverageRows");
                WorkCurveHelper.HistoryAverageRows = node == null ? 1 : int.Parse(node.InnerText);


                //是否启动重置
                node = doc.SelectSingleNode("application/IsResetMotor");
                WorkCurveHelper.IsResetMotor = node == null ? false : bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("application/EncoderCoeff/isShowEncoder");
                WorkCurveHelper.isShowEncoder = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/EncoderCoeff/isShowEncoder", new Dictionary<string, string>()
                    {
                        {
                            "isShowEncoder", "false"
                        }
                    }, true);
                }
                node = doc.SelectSingleNode("application/EncoderCoeff/EncoderCalcWay");
                WorkCurveHelper.EncoderCalcWay = node == null ? 0 : int.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/EncoderCoeff/EncoderCalcWay", new Dictionary<string, string>()
                    {
                        {
                            "EncoderCalcWay", "0"
                        }
                    }, true);
                }
                node = doc.SelectSingleNode("application/EncoderCoeff/CalcType");
                WorkCurveHelper.PureCalcType = node == null ? 3 : int.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/EncoderCoeff/CalcType", new Dictionary<string, string>()
                    {
                        {
                            "CalcType", "3"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/EncoderCoeff/PureAdjustCoeff");
                WorkCurveHelper.PureAdjustCoeff = node == null ? 1 : double.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/EncoderCoeff/PureAdjustCoeff", new Dictionary<string, string>()
                    {
                        {
                            "PureAdjustCoeff", "1"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/IsUserElect");
                WorkCurveHelper.IsUseElect = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/IsUseElect", new Dictionary<string, string>()
                    {
                        {
                            "IsUseElect", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/bInitialize");
                WorkCurveHelper.bInitialize = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/bInitialize", new Dictionary<string, string>()
                    {
                        {
                            "bInitialize", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/bShowInitParam");
                WorkCurveHelper.bShowInitParam = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/bShowInitParam", new Dictionary<string, string>()
                    {
                        {
                            "bShowInitParam", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/bCurrentInfluenceGain");
                WorkCurveHelper.bCurrentInfluenceGain = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/bCurrentInfluenceGain", new Dictionary<string, string>()
                    {
                        {
                            "bCurrentInfluenceGain", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/IsPureElemCurrentUnify");
                WorkCurveHelper.IsPureElemCurrentUnify = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/IsPureElemCurrentUnify", new Dictionary<string, string>()
                    {
                        {
                            "IsPureElemCurrentUnify", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/TubeAngle");
                WorkCurveHelper.TubeAngle = node == null ? "19.5,33.2" : node.InnerText;
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/TubeAngle", new Dictionary<string, string>()
                    {
                        {
                            "TubeAngle", "19.5,33.2"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/bCustomDevice");
                WorkCurveHelper.bCustomDevice = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/bCustomDevice", new Dictionary<string, string>()
                    {
                        {
                            "bCustomDevice", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/bHeightLayer");
                WorkCurveHelper.bHeightLayer = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/bHeightLayer", new Dictionary<string, string>()
                    {
                        {
                            "bHeightLayer", "false"
                        }
                    }, true);
                }



                //node = doc.SelectSingleNode("application/FindCamerSecond");
                //WorkCurveHelper.FindCamerSecond = node == null ? 10 : int.Parse(node.InnerText);
                //if (node == null)
                //{
                //    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/FindCamerSecond", new Dictionary<string, string>()
                //    {
                //        {
                //            "FindCamerSecond", "10"
                //        }
                //    }, true);
                //}

                node = doc.SelectSingleNode("application/bXYMotorSetp");
                WorkCurveHelper.bXYMotorSetp = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/bXYMotorSetp", new Dictionary<string, string>()
                    {
                        {
                            "bXYMotorSetp", "false"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/ZmotorStep");
                WorkCurveHelper.ZmotorStep = node == null ? 1000 : int.Parse(node.InnerText);
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/ZmotorStep", new Dictionary<string, string>()
                    {
                        {
                            "ZmotorStep", "1000"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/bOpenOutSample");
                WorkCurveHelper.bOpenOutSample = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/bOpenOutSample", new Dictionary<string, string>()
                    {
                        {
                            "bOpenOutSample", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/bAdjustInit");
                WorkCurveHelper.IsAdjustInit = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/bAdjustInit", new Dictionary<string, string>()
                    {
                        {
                            "bAdjustInit", "false"
                        }
                    }, true);
                }

                ///有电磁阀时是否继续开高压
                node = doc.SelectSingleNode("application/IsContinueVol");
                WorkCurveHelper.IsContinueVol = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/IsContinueVol", new Dictionary<string, string>()
                    {
                        {
                            "IsContinueVol", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/backStep");
                WorkCurveHelper.backStep = node == null ? 1000 : int.Parse(node.InnerText);
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/backStep", new Dictionary<string, string>()
                    {
                        {
                            "backStep", "1000"
                        }
                    }, true);
                }

                ///有电磁阀时是否继续开高压
                node = doc.SelectSingleNode("application/IsShowXYMove");
                WorkCurveHelper.IsShowXYMove = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/IsShowXYMove", new Dictionary<string, string>()
                    {
                        {
                            "IsShowXYMove", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/IntensityLimit");
                WorkCurveHelper.IntensityLimit = node == null ? 0.1 : double.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/IntensityLimit", new Dictionary<string, string>()
                    {
                        {
                            "IntensityLimit", "0.1"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/POpt");
                WorkCurveHelper.POptimization = node == null ? 3 : double.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/POpt", new Dictionary<string, string>()
                    {
                        {
                            "POpt", "3"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/Camera/FocusArea");
                WorkCurveHelper.FocusArea = node == null ? 1 : int.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/Camera/FocusArea", new Dictionary<string, string>()
                    {
                        {
                            "FocusArea", "1"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/Camera/CamerReturnStep");
                WorkCurveHelper.CamerReturnStep = node == null ? 1000 : int.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/Camera/CamerReturnStep", new Dictionary<string, string>()
                    {
                        {
                            "CamerReturnStep", "1000"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/bSetHeight");

                WorkCurveHelper.bSetHeight = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/bSetHeight", new Dictionary<string, string>()
                    {
                        {
                            "bSetHeight", "false"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/EncoderCoeff/isShowEncoder");
                WorkCurveHelper.isShowEncoder = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/EncoderCoeff/isShowEncoder", new Dictionary<string, string>()
                    {
                        {
                            "isShowEncoder", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/IsDBOpen");
                WorkCurveHelper.IsDBOpen = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/IsDBOpen", new Dictionary<string, string>()
                    {
                        {
                            "IsDBOpen", "false"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/matrixMode");
                WorkCurveHelper.matrixMode = node == null ? "dotDot" : node.InnerText.Trim();
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/matrixMode", new Dictionary<string, string>()
                    {
                        {
                            "matrixMode", "dotDot"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/detectPoints");
                WorkCurveHelper.detectPoints = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/detectPoints", new Dictionary<string, string>()
                    {
                        {
                            "detectPoints", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/roiRadius");
                WorkCurveHelper.roiRadius = node == null ? 10 : int.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/roiRadius", new Dictionary<string, string>()
                    {
                        {
                            "roiRadius", "10"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/maxPixelErr");
                WorkCurveHelper.maxPixelErr = node == null ? 10 : int.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/maxPixelErr", new Dictionary<string, string>()
                    {
                        {
                            "maxPixelErr", "10"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/maxDetectNum");
                WorkCurveHelper.maxDetectNum = node == null ? 10 : int.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/maxDetectNum", new Dictionary<string, string>()
                    {
                        {
                            "maxDetectNum", "10"
                        }
                    }, true);
                }

                //毫米与步长转换参数在运行过程中不会变化，只可在后台配置文件更改，此处进行一次初始化

                node = doc.SelectSingleNode("application/XCoeff");
                WorkCurveHelper.XCoeff = node == null ? 10 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/XCoeff", new Dictionary<string, string>()
                    {
                        {
                            "XCoeff", "10"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/YCoeff");
                WorkCurveHelper.YCoeff = node == null ? 10 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/YCoeff", new Dictionary<string, string>()
                    {
                        {
                            "YCoeff", "10"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/Y1Coeff");
                WorkCurveHelper.Y1Coeff = node == null ? 10 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/Y1Coeff", new Dictionary<string, string>()
                    {
                        {
                            "Y1Coeff", "10"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/ZCoeff");
                WorkCurveHelper.ZCoeff = node == null ? 10 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/ZCoeff", new Dictionary<string, string>()
                    {
                        {
                            "ZCoeff", "10"
                        }
                    }, true);
                }



                //平台行程参数在运行过程中可以在用户界面进行更改
                node = doc.SelectSingleNode("application/TabResetHeight");
                WorkCurveHelper.TabResetHeight = node == null ? 40 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/TabResetHeight", new Dictionary<string, string>()
                    {
                        {
                            "TabResetHeight", "40"
                        }
                    }, true);
                }

                //平台参数设置
                node = doc.SelectSingleNode("application/InOutDis");
                WorkCurveHelper.InOutDis = node == null ? 108 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/InOutDis", new Dictionary<string, string>()
                    {
                        {
                            "InOutDis", "108"
                        }
                    }, true);
                }



                node = doc.SelectSingleNode("application/TwoCameraDis");
                WorkCurveHelper.TwoCameraDis = node == null ? 78 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/TwoCameraDis", new Dictionary<string, string>()
                    {
                        {
                            "TwoCameraDis", "78"
                        }
                    }, true);
                }

                //下方为远景图像大小公式的各种系数的一次初始化
                node = doc.SelectSingleNode("application/squareCoeff");
                WorkCurveHelper.squareCoeff = node == null ? 0 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/squareCoeff", new Dictionary<string, string>()
                    {
                        {
                            "squareCoeff", "0"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/multiCoeff");
                WorkCurveHelper.multiCoeff = node == null ? 1 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/multiCoeff", new Dictionary<string, string>()
                    {
                        {
                            "multiCoeff", "1"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/baseCoeff");
                WorkCurveHelper.baseCoeff = node == null ? 170 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/baseCoeff", new Dictionary<string, string>()
                    {
                        {
                            "baseCoeff", "170"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/heightWidthRatio");
                WorkCurveHelper.heightWidthRatio = node == null ? 0.75f : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/heightWidthRatio", new Dictionary<string, string>()
                    {
                        {
                            "heightWidthRatio", "0.75"
                        }
                    }, true);
                }



                node = doc.SelectSingleNode("application/FixedXY");

                WorkCurveHelper.FixedXY = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/FixedXY", new Dictionary<string, string>()
                    {
                        {
                            "FixedXY", "false"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/FixedZ");

                WorkCurveHelper.FixedZ = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/FixedZ", new Dictionary<string, string>()
                    {
                        {
                            "FixedZ", "false"
                        }
                    }, true);
                }



                node = doc.SelectSingleNode("application/zSpeedMode");

                WorkCurveHelper.zSpeedMode = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/zSpeedMode", new Dictionary<string, string>()
                    {
                        {
                            "zSpeedMode", "false"
                        }
                    }, true);
                }


                node = doc.SelectSingleNode("application/multiReset");
                WorkCurveHelper.multiReset = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/multiReset", new Dictionary<string, string>()
                    {
                        {
                            "multiReset", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/filterReset");
                WorkCurveHelper.filterReset = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/filterReset", new Dictionary<string, string>()
                    {
                        {
                            "filterReset", "false"
                        }
                    }, false);
                }


                node = doc.SelectSingleNode("application/zFixable");
                WorkCurveHelper.zFixable = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/zFixable", new Dictionary<string, string>()
                    {
                        {
                            "zFixable", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/hasMotorEncoder");
                WorkCurveHelper.hasMotorEncoder = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/hasMotorEncoder", new Dictionary<string, string>()
                    {
                        {
                            "hasMotorEncoder", "false"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/encoderCoeff");
                WorkCurveHelper.encoderCoeff = node == null ? (float)3.5 : float.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/encoderCoeff", new Dictionary<string, string>()
                    {
                        {
                            "encoderCoeff", "3.5"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/encoderMotorCode");
                WorkCurveHelper.encoderMotorCode = node == null ? 4 : int.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/encoderMotorCode", new Dictionary<string, string>()
                    {
                        {
                            "encoderMotorCode", "4"
                        }
                    }, true);
                }

                node = doc.SelectSingleNode("application/loopTestDemo");
                WorkCurveHelper.loopTestDemo = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/loopTestDemo", new Dictionary<string, string>()
                    {
                        {
                            "loopTestDemo", "false"
                        }
                    }, false);
                }

                node = doc.SelectSingleNode("application/normalizeSteps");
                WorkCurveHelper.normalizeSteps = node == null ? false : bool.Parse(node.InnerText);
                if (node == null)
                {
                    ReportTemplateHelper.SaveSpecifiedValue(path, doc, "application/normalizeSteps", new Dictionary<string, string>()
                    {
                        {
                            "normalizeSteps", "false"
                        }
                    }, false);
                }

                string filepath = "电机自检步长.txt";
                // 检查文件是否存在
                if (File.Exists(filepath))
                {
                    using (StreamReader reader = new StreamReader(filepath))
                    {
                        WorkCurveHelper.xSteps = int.Parse(reader.ReadLine());
                        WorkCurveHelper.ySteps = int.Parse(reader.ReadLine());
                    }
                }

            }
            catch
            {

            }
        }

        public static void ReadTitleIco()
        {
            DifferenceDevice.TitleIco = new Skyray.EDXRFLibrary.Define.TitleIco();

            Object obj = SerializeHelper.FileToObj(Application.StartupPath + "\\TitleIco");

            if (obj != null)
            {
                DifferenceDevice.TitleIco = obj as TitleIco;
            }
        }

        public static void InitTipSoundAndHotKey(Device device)
        {
            if (device.SysConfig == null)
            {
                SysConfig sysConfig = SysConfig.New.Init(false);
                sysConfig.Device = device;
                HotKeys key = HotKeys.New.Init(KeyModifiers.None, Keys.F2, "Start", SysFeatures.Start, "");
                sysConfig.HotKeys.Add(key);
                key = HotKeys.New.Init(KeyModifiers.None, Keys.F3, "Init", SysFeatures.Init, "");
                sysConfig.HotKeys.Add(key);
                key = HotKeys.New.Init(KeyModifiers.None, Keys.F4, "Energy", SysFeatures.Energy, "");
                sysConfig.HotKeys.Add(key);
                key = HotKeys.New.Init(KeyModifiers.None, Keys.F5, "Print", SysFeatures.Print, "");
                sysConfig.HotKeys.Add(key);
                sysConfig.Save();
            }
        }

        /// <summary>
        /// 获取设备ID
        /// </summary>
        /// <returns></returns>
        public static ARMInfo GetARMInfo()
        {
            return new ARMInfo
            {
                DeviceId = "",//从下位机获取到的设备ID写到这里
                DeviceName = "EDX3600B",//设备名称
                XRFEnabled = true
            };
        }

        /// <summary>
        /// 获取强度计算方法
        /// </summary>
        /// <param name="funcType"></param>
        /// <returns></returns>
        public static object[] GetIntensityMethod(FuncType funcType)
        {
            object[] objs = null;
            switch (funcType)
            {
                case FuncType.XRF:
                    objs = new object[]
                    { 
                        IntensityWay.FullArea,//净面积
                        IntensityWay.NetArea, //净面积                  
                        IntensityWay.Gauss, //高斯拟合
                        IntensityWay.FPGauss,//Fp高斯拟合
                        IntensityWay.Reference,//纯元素拟合
                    };
                    break;
                case FuncType.Rohs:
                    objs = new object[]
                    { 
                        IntensityWay.FullArea,//净面积
                        IntensityWay.NetArea, //净面积
                        IntensityWay.FixedGauss,//高斯拟合
                        IntensityWay.FPGauss,//Fp高斯拟合
                        IntensityWay.Reference,//纯元素拟合
                    };

                    break;
                case FuncType.Thick:
                    objs = new object[]
                    {                      
                        IntensityWay.Reference ,
                       // IntensityWay.FixedReference 
                        IntensityWay.FPGauss     
                    };
                    break;
                default: break;
            }
            return objs;
        }

        public static List<VColor> GetVisualColor()
        {
            return new List<VColor>
            {
                new VColor{ Color = Color.Chocolate,BeSelected = false},
                new VColor{ Color = Color.DeepPink,BeSelected = false},
                new VColor{ Color = Color.DarkRed,BeSelected = false},
                new VColor{ Color = Color.Yellow,BeSelected = false},
                new VColor{ Color = Color.Green,BeSelected = false},
                new VColor{ Color = Color.YellowGreen,BeSelected = false}
            };
        }

        public static Color[] GetElementColor()
        {
            return new Color[] 
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
        }

        /// <summary>
        ///Rohs当前设备增加条件和工作曲线
        /// </summary>
        /// <param name="device"></param>
        /// <param name="addDevice">增加设备标志，true增加设备，false为对当前设备进行验证</param>
        public static void InitCondition(Device device,bool addDevice)
        {
            Condition condi = null;
            condi = Skyray.EDXRFLibrary.Condition.FindOne(w => w.Type == ConditionType.Detection && w.Device.Id == device.Id);
            if (condi == null)
            {
                condi = Condition.New.Init(Info.strDetection);
                condi.Type = ConditionType.Detection;
                condi.Device = device;
                condi.InitParam = Default.GetInitParameter(device.SpecLength);//加载默认值
                condi.InitParam.FineGain = device.ComType == ComType.FPGA ? 1 : (device.ComType == ComType.USB && device.IsDP5 ? (device.Dp5Version==Dp5Version.Dp5_CommonUsb?8200:8.0f) : 120);
                condi.InitParam.Gain = device.ComType == ComType.FPGA ? 66 : (device.ComType == ComType.USB && device.IsDP5 ? 13 : 60);
               // condi.InitParam.ExpressionFineGain = "x";
                condi.DeviceParamList.Add(Default.GetDeviceParameter(device.SpecLength, 1));
                condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                
                condi.Save();
            }
            if (DifferenceDevice.IsXRF || DifferenceDevice.IsAnalyser)
            {
                //查找测量条件为匹配类型的条件
                condi = Skyray.EDXRFLibrary.Condition.FindOne(w => w.Type == ConditionType.Match && w.Device.Id == device.Id);
                if (condi == null)
                {
                    condi = Condition.New.Init(Info.Match);
                    condi.Type = ConditionType.Match;
                    condi.Device = device;
                    condi.InitParam = Default.GetInitParameter(device.SpecLength);//加载默认值
                    condi.InitParam.FineGain = device.ComType == ComType.FPGA ? 1 : (device.ComType == ComType.USB && device.IsDP5 ? (device.Dp5Version == Dp5Version.Dp5_CommonUsb ? 8200 : 8.0f) : 120);
                    condi.InitParam.Gain = device.ComType == ComType.FPGA ? 66 : (device.ComType == ComType.USB && device.IsDP5 ? 13 : 60);
                    condi.DeviceParamList.Add(Default.GetDeviceParameter(device.SpecLength,1));
                    condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                    condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                    condi.Save();
                }

                condi = Condition.FindOne(w => w.Type == ConditionType.Intelligent && w.Device.Id == device.Id);
                if (condi == null)
                {
                    condi = Condition.New.Init(Info.Intelligent);
                    condi.Type = ConditionType.Intelligent;
                    condi.Device = device;
                    condi.InitParam = Default.GetInitParameter(device.SpecLength);//加载默认值
                    condi.InitParam.FineGain = device.ComType == ComType.FPGA ? 1 : (device.ComType == ComType.USB && device.IsDP5 ? (device.Dp5Version == Dp5Version.Dp5_CommonUsb ? 8200 : 8.0f) : 120);
                    condi.InitParam.Gain = device.ComType == ComType.FPGA ? 66 : (device.ComType == ComType.USB && device.IsDP5 ? 13 : 60);
                    condi.DeviceParamList.Add(Default.GetDeviceParameter(device.SpecLength, 1));
                    condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                    condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                    condi.Save();
                    
                    WorkCurve workCurve = WorkCurve.New.Init(Info.Intelligent, Info.Intelligent, CalcType.FP, FuncType.XRF, false, 0,false,false,false,false,false,0,"",false,Info.strAreaDensityUnit,40,true);
                    workCurve.Condition = condi;
                    workCurve.Save();
                    ElementList elementList = ElementList.New.Init(true, 100, 0, false, 0, 0, false);
                    elementList.WorkCurve = workCurve;
                    elementList.MatchSpecListIdStr = "";
                    elementList.RefSpecListIdStr = "";
                    elementList.MainElementToCalcKarat = "Au";
                    elementList.LayerElemsInAnalyzer = "Rh";
                    elementList.Save();
                }
                condi = Condition.FindOne(w => w.Type == ConditionType.Match2 && w.Device.Id == device.Id);
                if (condi == null)
                {
                    condi = Condition.New.Init(Info.Match+2);
                    condi.Type = ConditionType.Match2;
                    condi.Device = device;
                    condi.InitParam = Default.GetInitParameter(device.SpecLength);//加载默认值
                    condi.InitParam.FineGain = device.ComType == ComType.FPGA ? 1 : (device.ComType == ComType.USB && device.IsDP5 ? (device.Dp5Version == Dp5Version.Dp5_CommonUsb ? 8200 : 8.0f) : 120);
                    condi.InitParam.Gain = device.ComType == ComType.FPGA ? 66 : (device.ComType == ComType.USB && device.IsDP5 ? 13 : 60);
                    condi.DeviceParamList.Add(Default.GetDeviceParameter(device.SpecLength, 1));
                    condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                    condi.DemarcateEnergys.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                    condi.Save();
                }

            }

           
        }


        public static void ReadNTFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\" + "param.dat";
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "param.dat"), FileMode.Open);

                BinaryReader reader = new BinaryReader(fs);
                DifferenceDevice.gradeNTNum = reader.ReadInt32();
                DifferenceDevice.param = new double[DifferenceDevice.gradeNTNum * 16 * 3];
                for (int i = 0; i < DifferenceDevice.gradeNTNum * 16 * 3; i++) DifferenceDevice.param[i] = reader.ReadDouble();
                reader.Close();
                fs.Close();
            }

            string pathGradeName = AppDomain.CurrentDomain.BaseDirectory + "\\" + "gradename.dat";
            if (File.Exists(pathGradeName))
            {
                DifferenceDevice.gradeNTName = new string[DifferenceDevice.gradeNTNum];
                FileStream fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gradename.dat"), FileMode.Open);
                BinaryReader reader = new BinaryReader(fs);
                for (int i = 0; i < DifferenceDevice.gradeNTNum; i++) DifferenceDevice.gradeNTName[i] = reader.ReadString();
                reader.Close();
                fs.Close();
            }
        }

        public static void GetSurfaceSource()
        {
            DbObjectList<SurfaceSourceLight> ss = SurfaceSourceLight.FindAll();
            if (ss.Count == 0)
            {
                SurfaceSourceLight surfaceSource = SurfaceSourceLight.New.Init(1000, 1000, 1000, 1000);
                surfaceSource.Save();
            }
        }

        /// <summary>
        /// 加密数据库
        /// </summary>
        private static int EncryptDB()
        {
            string sqliteCon, sqliteConCopy, dataSource, dataSouceHeader;
            sqliteCon = sqliteConCopy = Lephone.Data.DbEntry.Context.Driver.ConnectionString;
            dataSouceHeader = @"Data Source=";
            string[] arr = sqliteConCopy.Split(new char[] { ';' });
            if (arr == null || arr.Length <= 0)
            {
                //Msg.Show("ConnectionString Error");
                return 3;
            }
            dataSource = arr.ToList().Find(str => str.StartsWith(dataSouceHeader));
            if (string.IsNullOrEmpty(dataSource))
            {
                //Msg.Show("ConnectionString Error");
                return 3;
            }
            dataSource = dataSource.Replace(dataSouceHeader, "");
            if (!File.Exists(dataSource))
            {
                //Msg.Show(Info.DataBaseNotExist);
                return 4;
            }
            try
            {
                System.Data.SQLite.SQLiteConnection cnn = new System.Data.SQLite.SQLiteConnection(sqliteCon);
                cnn.Open();
                cnn.Close();
                return 0;//若能正常打开, 说明是已加密数据库, 无需任何操作
            }
            catch
            {

            }

            try
            {
                //有异常则可能是旧数据库无密码, 需要更改密码
                string str = ";password=";
                int idx = sqliteCon.IndexOf(str);
                idx = idx >= 0 ? idx : 0;
                string str2 = sqliteCon.Substring(idx);
                if (str2 == null || str2.Length <= 0 || str2 == sqliteCon)
                    return 3;
                string pwd = str2.Replace(str, "");
                sqliteCon = sqliteCon.Replace(str2, ""); //密码置空, 尝试打开数据库
                System.Data.SQLite.SQLiteConnection cnn = new System.Data.SQLite.SQLiteConnection(sqliteCon);
                cnn.Open();
                cnn.ChangePassword(pwd);
                cnn.Close();
                return 2;//更改密码完成
            }
            catch
            {
                return 1;//数据库密码错误, 不是空密码
            }
        }

        public static bool EncryptDBControl()
        {
            int value = EncryptDB();
            if (value == 1)
            {
                Log.Info("Database is Invalid : Password is not empty!");
                Msg.Show(Info.DataBaseInvalid);
                return false;
            }
            else if (value == 2)
            {
                Log.Info("Database has been upgraded!");
            }
            else if (value == 3)
            {
                Log.Info("ConnectionString has not been properly set");
                Msg.Show("ConnectionString has not been properly set");
                return false;
            }
            else if (value == 4)
            {
                Msg.Show(Info.DataBaseNotExist);
                return false;
            }
            return true;
        }

        public static bool ComTypeControl()
        {
            if (WorkCurveHelper.type != InterfaceType.NetWork)
            {
                Msg.Show(Info.FailedToGetDevInfo);
                return false;
            }
            return true;
        }
    }

}
