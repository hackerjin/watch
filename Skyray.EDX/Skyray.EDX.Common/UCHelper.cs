using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Skyray.EDXRFLibrary;
using Skyray.Controls;
using ZedGraph;
using Skyray.Controls.Extension;
using System.Data;
using Skyray.EDX.Common.Component;
using Lephone.Data.Definition;
using Lephone.Util;
using Lephone.Data.Common;
using Skyray.Language;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.CompilerServices;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.Dog;
using System.Reflection;
using Modbus.Data;
using System.Diagnostics;
namespace Skyray.EDX.Common
{

    public enum MessageType
    {
        Start,
        End,
        Refresh
    }

    public class VColor
    {
        public Color Color { get; set; }
        public bool BeSelected { get; set; }
    }

    public class DirectRunTest
    {
        public bool IsDirectRun { get; set; }
        public bool IsKeyCall { get; set; }
    }

    public class NiCuNiParams
    {
        public double limit { get; set; }
        public double aValue { get; set; }
        public double nValue { get; set; }
        public double kValue { get; set; }
        public double bValue { get; set; }
    }
    public class WorkCurveHelper
    {

        static WorkCurveHelper()
        {
            Thread.AllocateNamedDataSlot("decimals");
            Thread.AllocateNamedDataSlot("ndValue");
            Thread.AllocateNamedDataSlot("ndValid");
            Thread.AllocateNamedDataSlot("ndText");
        }

        private static int PeakLowHightSpacing = 10;

        public static string SaveSamplePath;

        public static string SaveGraphicPath;
        //{
        //    get { 
        //          if (_SaveGraphicPath!=string.Empty&&Directory.Exists(_SaveGraphicPath)) return _SaveGraphicPath;
        //          else return Application.StartupPath + "\\Image\\SpecImage"+"\\" + (DifferenceDevice.IsXRF ? "EDXX" : (DifferenceDevice.IsRohs ? "EDXR" : (DifferenceDevice.IsThick ? "EDXT" : "EDX3000")));
        //    }
        //}
        // private static string _SaveGraphicPath;

        public static string SaveSpectrumPath = Application.StartupPath + "\\Spectrum";
        public static string snFilePath = Application.StartupPath + "\\sn.skyray";
        public static string SampleCalPath = Application.StartupPath + "\\ConSampleCal.cal";

        //public static bool IsSaveRecorder = true;

        public static bool IsSaveSpectrumImage = false;

        public static bool IsSaveHistory = true;

        public static bool IsOpenSpecByHistoryRecord = false;

        public static bool TestOnButtonPressedEnabled = false;

        public static bool TestOnCoverClosedEnabled = false;

        public static bool ButtonDirectRun = false;

        public static bool IsSaveSpecData = true;

        public static bool IsManualTest = false;//默认手动测试不勾选，手动测试时每次测量后自动等待用户点击确定后再进行测量(Fpthick)。

        public static bool isShowND = true;

        public static int SelectShowType = 0;

        public static int AdjustType = 0;

        public static bool Is3200L = false;

        public static int VaccumDiffer = 3;

        public static int HistoryDefaultCurveType = 0;

        public static int CurrentProcessType = 1;
        #region 北京时间控制

        public static bool IsAutoIncrease = false;
        public static bool DropData = false;

        public static int ReportVkOrVe = 1;  //报告格式 默认1 ：WPS   0:EXCEL

        #endregion

        public static double NDValue;

        public static int SpecType;//报告谱图类型 0:默认， 1:韩国三星

        public static int ErrorType;//误差计算公式 0 ：默认误差 1：以3西格玛形式计算误差


        public static ISpectrumDAL DataAccess = new SQliteDAL();

        //public static ISpectrumDAL DataPureAccess = new SQliteDAL(); // 用作纯元素谱的库查询，纯元素谱不进入文件。

        public static TotalEditionType EditionType = TotalEditionType.Default;

        public static string SelectSpectrumPath;

        //public static bool IsPdRh;

        public static int SaveType = 0;

        public static double CamRatio = 1;

        //修改：何晓明 20111019 RoSH结果判断是1.含量判断,2.含量+误差判断
        public static int iResultJudgingType = 1;
        public static bool IsResultEdit = true;
        public static int isThickLimit = 0;
        public static int RohsElemCountLimit = 0;
        public static bool CheckRohsElemCount = false;

        //PK开启标志
        public static bool IsCarrayMatchPK = false;
        public static bool IsCarrayMatchPKSetting = false;
        public static bool IsMatchAlways = false;
        public static double CarrayMatchPKValue = 99.99;
        public static double ResolveFactor = 1;
        public static bool IsPureAuTest = false;
        public static double intResolve = 135.0;  //分辨率限值
        public static bool IsReoprtShowQualityElem = false;  // pf by 20150616 是否在保存的报告中显示定性分析元素
        public static bool IsReoprtShowAxias = false;  //报告中放大部数与谱图一致
        public static bool ReportSaveScreen = false;
        public static double KaratTranslater = 99.995;
        public static Keys SaveHistoryKeys;
        public static bool IsSaveToDataBase = false;//pfby 2015 0923 保存测量结果至SQL 周大福
        public static int SetCoverOpen = 0;
        public static double VacummNo = 0;
        public static bool IsShowEncryptButton = false;//是否显示硬件加密按钮
        public static bool IsLiteralityToDay = false;//精确到天
        public static bool IsEditRefCoefficient = false;  // 是否只读
        //public static bool IsCalcKaratWithMainElement = false; //是否以主元素结果来计算K值
        public static int isShowXRFStandard = 0;  //XRF是否显示标准
        public static string PrintName = string.Empty; //默认蓝牙打印机名称
        //public static bool isShowBase = true;
        public static int SaveReportType = 0;//0,EXCEl,1.PDF,2.CSV
        public static int PrinterType = 0;//0 普通打印机，1 蓝牙打印机

        public volatile static bool AskToClose = true;
        //public volatile static bool HasMainFormShown = false;
        public volatile static bool IsBatchTest = false;
        public volatile static bool IsTestNormal = false;
        public static bool HideZeroContentElements = false;
        public static int HistoryAverageRows = 1;  //做平均值条数
        public static bool IsResetMotor = false;
        public static bool bInitialize = false;
        public static bool isShowEncoder = false;
        public static int EncoderCalcWay = 0;
        public static List<SpecListEntity> PureSpecListEntiry = new List<SpecListEntity>();
        public static List<double> listPurIntensity = new List<double>();
        public static bool IsCurrentUnify = false;  //是否纯元素管流归一
        public static bool IsPureElemCurrentUnify = false;
        public static string TubeAngle = "19.5,33.2";
        public static bool bCurrentInfluenceGain = true;
        public static bool bShowInitParam = false;
        public static bool bCustomDevice = false;
        public static bool bMotorRestart = false;
        public static bool bOpenOutSample = false;
        public static double cx = 0.6;
        public static double cy = 0.1;
        public static bool bHeightLayer = false;
        public static int backStep = 1000;
        public static bool IsShowXYMove = false;  //XY设置显示 ，默认不显示
        public static bool IsDBOpen = false;
        public static object ucHistoryRecord1 = null;
        public static object ucCameraControl1 = null;
        public static DataStore dataStore = null;
        public static uint startDoing = 0;
        public static uint stopDoing = 0;
        public static uint initDoing = 0;
        public static bool FixedXY = false;
        public static bool FixedZ = false;
        public static bool FixedDisXY = true;
        public static bool FixedDisZ = true;
        public static object curCamera = null;
        public static Type thickType = null;
        public static object curThick = null;
        public static Type curFrmThickType = null;
        public static Form curFrmThick = null;
        public static bool AdminPass = false;
        public static Type cameraType = null;
        public static object camera = null;
        public static bool zSpeedMode = false;
        public static string smallCameraName = "";
        public static string largeCameraName = "";
        public static bool testingOpenCover = false;
        public static bool suspendTest = false;
        public static TestDevicePassedParams testParamsBackup = null;
        public static int contiOffsetInTemp = 0;
        public static int testedRows = 0;

        public static int actualTubVoltage = 0;

        public static List<long> testIds = new List<long>();


        public static bool zFixable = false;

        public static bool multiReset = false;
        public static bool filterReset = false;


        //电机步长与毫米换算关系
        public static float XCoeff = (float)(128.0 * 200 / 5);
        public static float YCoeff = (float)(128.0 * 200 / 5);
        public static float Y1Coeff = (float)(256.0 * 200 / 5);
        public static float ZCoeff = (float)(4992);
        public static float encoderCoeff = (float)(3.5);
        public static bool hasMotorEncoder = false;
        public static int encoderMotorCode = 0;

        //平台复位高度，单位为毫米
        public static float TabResetHeight = 40.0f;
        //进出样距离，单位为毫米
        public static float InOutDis = 103.47f;
        //远景摄像头距离近景摄像头的距离，单位为毫米
        public static float TwoCameraDis = 81.17f;
        //平台进样处距离远景摄像头的距离，单位为毫米
        public static float LargeViewDis = 22.3f;   
        //测试时，Y1轴向里移动距离，单位为毫米
        public static float TestDis = 68.7f;



        

        //平台实际大小，单位为毫米
        public static float TabWidth = 246.0f;
        public static float TabHeight = 246.0f;

        public static float TabWidthStep
        {

            get
            {

                return TabWidth * XCoeff;
            }
        }


        public static float TabHeightStep
        {

            get
            {

                return TabHeight * YCoeff;
            }
        }

        //平台的复位位置，单位为步长
        public static float ResetX      
        {
            get
            {   
                return TabWidth / 2.0f * XCoeff;
            }

        }


        public static float ResetY 
        {
           get
           {
               if (WorkCurveHelper.DeviceCurrent != null)
               {
                   if (WorkCurveHelper.DeviceCurrent.HasMotorSpin || WorkCurveHelper.DeviceCurrent.MotorYMaxStep < 300)
                       return TabHeight / 2.0f * YCoeff;
                   else
                       return 160 * YCoeff; //复位点与最外点的距离
               }
               else
                   return 0;

            }
        }
        
        
        
        public static float ResetZ
        {
           get
           {

               return  TabResetHeight * ZCoeff;

            }
        }

        //近景摄像头下的当前平台位置，单位为步长
        public static float X = ResetX;
        public static float Y = ResetY;
        public static float Z = ResetZ;

        //当前远景图像的拍摄高度，单位为步长，与Z保持一致，且两者都仅在拍摄远景图像时更新，运动时不更新
        public static float largeViewCatchHeight = 0;

        //远景摄像头高度与拍摄范围的公式系数
        public static float squareCoeff = 0;

        public static float multiCoeff = 1.2f;

        public static float baseCoeff = 182f;

        public static float heightWidthRatio = 0.75f;


        //远景摄像头下的可见平台范围大小，单位为毫米
        public static float largeViewTabWidth
        {

            get
            {
                return  squareCoeff*  (float)Math.Pow(largeViewCatchHeight/ZCoeff,2)  +  multiCoeff* largeViewCatchHeight / ZCoeff + baseCoeff ;
                
            }

        }
        public static float largeViewTabHeight
        {

            get
            {
                return largeViewTabWidth * heightWidthRatio;
            }

        }


        //远景摄像头拍摄的远景图像中心点在平台上的位置，单位为步长
        public static float largeViewX = ResetX;

        public static float largeViewY = ResetY;

        public static bool motorMoving = false;

        public static bool contiMoving = false;

        public static bool goingToDest = false;

        public static bool isSampleIn = true;

        public static int testNum = 0;
        public static Type ucCameraType = null;
        public static object ucCamera = null;
        public static string sPortName = "COM4";
        public static int iFrequency = 9600;
        public static int iTimeOut = 1000;

        public static ulong moveNo = 0;
        public static ulong nextNo = 1;
   
        public static bool isIn = false;
        public static bool testDemo = false;
        public static bool testDemoing = false;

        public static ushort testDemoMode = 0;
        public static string matrixMode = "dotDot";


        public static bool detectPoints = false;
        public static object firstCircle = null;

        public static int roiRadius = 4;
        public static int maxPixelErr = 15;
        public static int maxDetectNum = 2;

        public static int halfFociWidth = 24;
        public static int halfFociHeight = 24;

        public static int testTimes = 0;
        public static int curDeviceNum = 0;

        public static bool selfCheck = false;
        public static bool loopTestDemo = false;
        public static bool normalizeSteps = false;
        public static int xSteps = 0;
        public static int ySteps = 0;

        public static Mutex motorMutex = new Mutex();

        public static Bitmap reportImage = null;

        public static int FocusArea = 1; //1大 2中 3小;

        public static int CamerReturnStep = 1000;

        public static bool bSetHeight = false;

        public static NiCuNiParams NiCuNiParam = new NiCuNiParams();

        /// <summary>
        ///  是否使用电磁阀门
        /// </summary>
        public static bool IsUseElect = false;

        /// <summary>
        /// 有电磁阀门时是否继续开高压
        /// </summary>
        public static bool IsContinueVol = false;

        /// <summary>
        /// 全局线程集合 线程队列
        /// </summary>
        public static List<System.Threading.Thread> lsThread = new List<System.Threading.Thread>();

        public static DirectRunTest DirectRun = new DirectRunTest();

        /// <summary>
        /// 点击开始时，选择的公司其它信息
        /// </summary>
        public static Dictionary<string, string> SeleCompanyOthersInfo = new Dictionary<string, string>();

        public static void waitMoveStop()
        {
            ulong moveNo = WorkCurveHelper.moveNo;
            while (moveNo >= WorkCurveHelper.nextNo) ;
        }

        public static string LanguageShortName
        {

            get
            {
                string Language = "";
                Languages CurrentLang = Languages.FindOne(l => l.IsCurrentLang == true);
                Language = CurrentLang.ShortName;
                if (Language.ToLower() == "english") Language = "EN";
                else if (Language.ToLower() == "chinese") Language = "CN";
                return Language;
            }
        }

        private static IExcelFormat EfiValue = null;
        public static IExcelFormat Efi
        {
            get
            {
                if (EfiValue != null)
                    return EfiValue;
                string path = Application.StartupPath + @"\Plugin\ExcelFormat";
                if (!Directory.Exists(path)) return null;
                List<string> files = Directory.GetFiles(path, "*.plug").ToList();
                if (files == null || files.Count <= 0) return null;
                foreach (string f in files)
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFile(f);
                        Type[] types = asm.GetTypes();
                        foreach (Type t in types)
                        {
                            if (t.GetInterface("IExcelFormat") != null)
                            {
                                EfiValue = (IExcelFormat)Activator.CreateInstance(t);
                                return EfiValue;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        continue;
                    }

                }
                return null;
            }
        }

        private static Skyray.EDX.Common.UIHelper.MeasureTimeType measureTimeType;
        /// <summary>
        /// 测试时间显示类型
        /// </summary>
        public static Skyray.EDX.Common.UIHelper.MeasureTimeType MeasureTimeType
        {
            get
            {
                return measureTimeType;
            }
            set
            {
                measureTimeType = value;
            }
        }


        public static ISpectrumDAL GetExportType(string path)
        {
            ISpectrumDAL dal = null;
            switch (SpecExportType)
            {
                case 0:
                    dal = new FileDAL(path);
                    break;
                case 2:
                    dal = new PlainTextDAL(path);
                    break;
                default:
                    break;
            }
            return dal;
        }

        /// <summary>
        /// 线程队列 按顺序出队列并按顺序执行
        /// </summary>      
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public static void PopThreadList()
        {
            while (true)
            {
                int i = WorkCurveHelper.lsThread.IndexOf(Thread.CurrentThread);
                if ((i == 0 || i == -1) && Thread.CurrentThread.IsAlive)//如果为队列第一个线程或非队列线程调用方法 执行但不出队列
                {
                    break;
                }
                else if (i != -1 && i != 0 && WorkCurveHelper.lsThread[i - 1].IsAlive)//如果当前线程的前一个线程未完成 合并前一线程执行
                {
                    WorkCurveHelper.lsThread[i - 1].Join();
                }
                else if (i != -1 && i != 0 && !WorkCurveHelper.lsThread[i - 1].IsAlive)//如果前一线程已完成 当前线程出队列执行
                {
                    WorkCurveHelper.lsThread[i - 1].Abort();
                    WorkCurveHelper.lsThread.RemoveAt(i - 1);
                    if (WorkCurveHelper.lsThread.Count == 1)//当前队列只剩自己最后一个线程 清空队列
                        WorkCurveHelper.lsThread.Clear();
                    break;
                }
            }
        }

        /// 判断是否安装了excel
        /// </summary>
        /// <returns>true 安装了/false 未安装</returns>
        public static bool IsExcelInstall()
        {
            Type type;
            if (WorkCurveHelper.ReportVkOrVe == 1)
            {
                type = Type.GetTypeFromProgID("ET.Application");

            }
            else
            {
                type = Type.GetTypeFromProgID("Excel.Application");
            }
            return type != null;
        }

        public static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in info)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /// <summary>
        /// 设置默认元素边界
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool SetDefaultPeak(string elementName, out int left, out int right)
        {
            left = 0;
            right = 0;
            double energy = 0;
            Atom atom = Skyray.EDXRFLibrary.Atoms.AtomList.Find(a => a.AtomName.ToLower() == elementName.Trim().ToLower());
            int Id = 0;
            if (atom != null)
            {
                Id = atom.AtomID;
                if (Id <= 56)
                {
                    energy = Skyray.EDXRFLibrary.Atoms.AtomList.Find(a => a.AtomName == elementName).Ka;
                }
                else
                {
                    energy = Skyray.EDXRFLibrary.Atoms.AtomList.Find(a => a.AtomName == elementName).La;
                }
                int channel = DemarcateEnergyHelp.GetChannel(energy);
                int specLength = (int)WorkCurveHelper.DeviceCurrent.SpecLength;
                left = channel - PeakLowHightSpacing < 0 ? 0 : channel - PeakLowHightSpacing;
                left = left > specLength ? specLength : left;
                right = channel + PeakLowHightSpacing > specLength ? specLength : channel + PeakLowHightSpacing;
                right = right < 0 ? 0 : right;
                if (left > right) left = right;
                return true;
            }
            return false;
        }



        public static void OpenUC(UCMultiple control, bool flag)
        {
            OpenUC(control, flag, "", true);
        }


        /// <summary>
        /// 打开标准样品
        /// </summary>
        /// <param name="control"></param>
        /// <param name="flag">是否最大化</param>
        public static void OpenUC(UCMultiple control, bool flag, string TitleName, bool isModel)
        {
            control.OpenUC(flag, TitleName, isModel, false);
            //if (!control.LoadConditionAnalyser())
            //    return;
            //Form form = new Form();
            //form.BackColor = Color.White;
            //form.MinimizeBox = false;
            //form.ShowInTaskbar = false;
            //int padSpace = 0;
            //form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
            //form.Controls.Add(control);
            //form.MaximizeBox = flag;
            //form.Text = TitleName;
            //if (!flag)
            //{
            //    form.FormBorderStyle = FormBorderStyle.FixedSingle;
            //}
            //form.ClientSize = new Size(control.Width + padSpace * 2, control.Height + padSpace * 2);
            //form.ShowIcon = false;
            //control.Dock = DockStyle.Fill;
            //form.StartPosition = FormStartPosition.CenterScreen;
            //if (DialogResult.OK == form.ShowDialog())
            //    control.ExcuteEndProcess(null);
            //else
            //{
            //    control.ExcuteCloseProcess(null);
            //}
        }

        /// <summary>
        /// 打开标准样品
        /// </summary>
        /// <param name="control"></param>
        /// <param name="flag">是否最大化</param>
        public static void OpenUC(UCMultiple control, bool flag, string TitleName, bool isModel, bool noneStyle)
        {
            control.OpenUC(flag, TitleName, isModel, noneStyle);
            //if (control.IsSignlObject) return;
            //if (!control.LoadConditionAnalyser())
            //    return;
            //Form form = new Form();
            //form.BackColor = Color.White;
            //form.MinimizeBox = false;
            //form.ShowInTaskbar = false;
            //int padSpace = 0;
            //form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
            //form.Controls.Add(control);
            //form.MaximizeBox = flag;
            //form.Text = TitleName;
            ////form.FormClosing += (s, ex) =>
            ////{
            ////    control.IsSignlObject = false;
            ////};
            //if (!flag)
            //{
            //    form.FormBorderStyle = FormBorderStyle.FixedSingle;
            //}
            //form.ClientSize = new Size(control.Width + padSpace * 2, control.Height + padSpace * 2);
            //form.ShowIcon = false;
            //control.Dock = DockStyle.Fill;
            //form.StartPosition = FormStartPosition.CenterScreen;
            //if (isModel)
            //{
            //    if (DialogResult.OK == form.ShowDialog())
            //        control.ExcuteEndProcess(null);
            //    else
            //    {
            //        control.ExcuteCloseProcess(null);
            //    }
            //}
            //else
            //{
            //    form.Show();
            //}
            //control.IsSignlObject = true;
        }

        //static void form_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
        public static int PanelSpecIndex = 0;
        public static string[] Atoms;
        public static int[] Lines;

        /// <summary>
        /// 当前工作曲线
        /// </summary>
        private static WorkCurve _WorkCurveCurrent;

        public static WorkCurve WorkCurveCurrent
        {

            get { return _WorkCurveCurrent; }
            set
            {
                _WorkCurveCurrent = value;
                SpecHelper.CURRENTWorkCurveTemp = value;
                if (value != null)
                {
                    CalcType = value.CalcType;
                    _WorkCurveCurrent.SetFpCalibrated(false);

                    _WorkCurveCurrent.SetIsAdjustInit(WorkCurveHelper.IsAdjustInit);
                    _WorkCurveCurrent.SetAngle(WorkCurveHelper.TubeAngle, WorkCurveHelper.isShowEncoder);
                    if (_WorkCurveCurrent.IsBaseAdjust)
                        _WorkCurveCurrent.SetBasePureSpec(GetbasePureSpec());
                }
            }
        }

        private static WorkCurve _BeforePureAuCurveCurrent;
        public static WorkCurve BeforePureAuCurveCurrent
        {
            get { return _BeforePureAuCurveCurrent; }
            set
            {
                _BeforePureAuCurveCurrent = value;
                SpecHelper.CURRENTWorkCurveTemp = value;
                if (value != null)
                    CalcType = value.CalcType;
            }
        }

        public static SysConfig SysConfig;

        public static SpecificationsCategory CategoryCurrent;

        public static FuncType DeviceFunctype = FuncType.XRF;

        private static string excelPath = string.Empty;
        public static string ExcelPath
        {
            get
            {
                if (string.IsNullOrEmpty(excelPath))
                    excelPath = Application.StartupPath + "\\Report";
                if (!Directory.Exists(excelPath))
                    Directory.CreateDirectory(excelPath);
                return excelPath;
            }
            set
            {
                excelPath = value;
            }
        }

        public static bool DemoInstance = false;

        public static CalcType CalcType;



        //public static WorkRegion CurrentWorkRegion = WorkRegion.FindAll()[0];
        public static WorkRegion CurrentWorkRegion = null;

        public static ARMInfo ArmInfo;

        //public static Color DefaultSpecColor = Color.Blue;

        public static CustomStandard CurrentStandard;

        public static DeviceMeasure deviceMeasure;

        public static InterfaceType type = InterfaceType.Usb;

        //public static TargetType TargetType;

        //public static MachineType MachineType;
        //public static XRFChart theWholeChart;

        public static SpecMessage specMessage;

        //public static string sTestStopInfo = string.Empty;

        public static double Volumngreen;

        public static DataTable currentDataTable;

        public static DataTable staticsTable;

        public static string stringAu = "";

        public static int SoftWareContentBit = 4;
        public static int SoftWareIntensityBit = 4;
        public static int ThickBit = 4;
        //public static int ContentBit = 4;
        public static double AreaDensity = 1;
        public static double AreaDensityCoef = 1;

        public static bool IndiaAuWarning = true;
        public static double IndiaAuWarningMax = 0.7;
        public static double IndiaAuWarningMin = 0;
        public static int IndiaAuWarningTime = 5;

        public static bool ZeroMainElemAlertShow = true;
        public static int SpecExportType = 0;//谱导出格式 0，二进制 1，fp格式 2,明文格式 3，MCA 
        public static bool DelAfterExport = false;
        public static bool IPeakCalibration = false;//是否自动校正峰飘

        public static bool IShowQuickInfo = false;//是否显示快慢成型等信息

        public static bool IsExistsLock = false;
        public static int IsInnerMatch = 0;
        public static List<string> deviceNameList = new List<string>();
        public static double MoveStationVisible = 0d;
        public static int InitTime = 10;
        public static List<int> InitCurrentList = new List<int>();  //初始化管流集
        public static bool IsOpenRegister = true;

        public static int IDemarcateTest = 0;//0时恢复做，1自动峰标不做峰飘校正,2计算截距时也不做峰飘校正，但是fpga的截距值为0值， 

        public static int PeakErrorChannel = 0;//校正峰飘误差道

        public static int FastLimit = 0;//快成型上限（成型时间6.4）

        public static double InitChannelError = 0.2;//自制数字多道初始化误差

        public static bool IsAutoDetection = false;//是否自检
        public static int DetectionType = 0;//自检类型
        public static bool IsOnStartDetedtion = false;//是否开机自检
        public static string EDXRFAlertExceptElement = "Ni,Cu,Zn,Au,Ag,Pd";//报警排除元素

        public static bool IsAutoFocus = true;
        public static bool IsAutoAscend = false;
        public static int AscendStepZ = 10000;
        public static bool IsAutoUpload = false; //@CYR180815 测量结束后, 是否自动上传数据
        public static bool AutoUploadVisible = false;
        public static bool IsSetXRange = false;
        public static double[] XRange = new double[2];
        public static bool IsShowPureSpec = false;

        public static bool WriteNetportLog = false;


        public static int PrintExcelCount = 20;

        public static int AdjustTime = 5;
        public static int AdjustInterval = 30;
        public static bool PumpShowProgress = false;
        public static int AdjustExceptTime = 3;
        public static bool IsMatchAuto = true;

        public static int MatchType = 0;//0--谱匹配，1--三元素匹配
        public static int MatchBaseLeft = 700;//背景左边界
        public static int MatchBaseRight = 1300;//背景右边界
        public static double MatchBaseRatio = 0.15;//背景面积占总面积的比重
        public static double MatchBaseKRatio = 1;//k系因子
        public static double MatchBaseLRatio = 2;//L系因子
        public static int MatchHighSubstrateLeft = 50;//背景左边界
        public static int MatchHighSubstrateRight = 400;//背景右边界
        public static double MatchMinorElemRatio = 0.2;//主元素的高度占多少

        //2014-02-08
        public static int DelElemType = 0;//匹配时筛选元素的类别 0，强度比，1，含量
        public static double DelElemThreshold = 0.1;//匹配时筛选元素的阈值
        public static bool IsDelElemByQuale = false;//匹配时是否根据定性分析

        public static double ResultLowLimit = 0.0;

        public static string DeviceTypeForChamber = string.Empty;
        public static StandardService JapanStandard = new StandardService();
        public static JapanStyle OKStyle;
        public static JapanStyle NGStyle;
        //2013-10-25针对光闸由滤光片代替的功能
        public static bool IsCloseShutterAfterTest = false;
        public static bool IsNewDp5NetUse = false;
        public static int ResponseFilterIndex = 1;

        public static bool IsDirectCaculate = false;

        public static int AxisZSpeedMode = 0; //0:自定义设置模式，1:快中慢模式
        public static double ThicknessLimit = 100;//Fpthick 中厚度计算的最大值um
        public static int ZFast = 150;
        public static int ZMiddle = 90;
        public static int ZSlow = 20;
        public static int DoorCloseType = 0;
        public static int Growstyle = 0;
        private static Device _deviceCurrent;

        public static bool RohsShowCamerInMain = false; //rohs主界面是否需要摄像头区域
        public static bool IsPopUpReportOpen = true;   //报存完报告是否需要打开，默认保存后打开
        public static bool IsCreateHistory = false;

        //判断是否成功获取设备信息
        public volatile static bool IsDeviceInfoGot = false;
        public volatile static bool UnderReconnecting = false;
        public volatile static int RetryGetVersionTimes = 5;
        public volatile static int CurrentReconnectTime = 0;
        public volatile static int ReConnectCondition = 3; //0:不重连; 1:只要加密版本号获取失败就重连;2:只要接口板版本号获取失败就重连;3:任何一个版本号获取失败都重连
        public volatile static EventWaitHandle Ewh = new AutoResetEvent(false);

        public static Point positionP = new Point(0, 0);
        public static int PureCalcType = 1;  //纯元素谱库计算方式，插值or二次
        public static double PureAdjustCoeff = 1;
        public static string DppMachineId = string.Empty;
        public static bool IsDppValidate = true;   //dpp验证判定是否开放权限
        public static string RegisterId = string.Empty;
        public static bool IsFirstInfluenceGain = true;// 改放大倍数
        public static int FindCamerSecond = 10;
        public static bool bXYMotorSetp = false;
        public static int ZmotorStep = 1000;
        public static bool IsAdjustInit = false; //是否使用初始化强度校正
        public static double IntensityLimit = 0.01;
        public static double POptimization = 3;


        /// <summary>
        /// 当前设备
        /// </summary>
        public static Device DeviceCurrent
        {
            get { return _deviceCurrent; }
            set
            {
                _deviceCurrent = value;
                SpecHelper.CurrentDevice = value;
                DataAccess.SetCurDeviceName(_deviceCurrent.Name);
            }
        }

        public static bool IsStandSample;

        public static List<NaviItem> NaviItems = new List<NaviItem>();

        public static Color[] DefaultElementColor;

        public static List<VColor> DefaultVirtualColor;

        /// <summary>
        /// 点击停止扫谱，是否进行保存谱文件和计算
        /// </summary>
        public static bool StopTestIsSave = false;

        /// <summary>
        /// 当前实谱List
        /// </summary>
        private static SpecListEntity mainSpecList;

        public static SpecListEntity MainSpecList
        {
            get { return WorkCurveHelper.mainSpecList; }
            set
            {
                if (WorkCurveHelper.mainSpecList != value)
                {
                    //VirtualSpecList = new List<SpecList>();
                    mainSpecList = value;
                }
            }
        }

        private static SpecEntity currentSpec;

        public static SpecEntity CurrentSpec
        {
            get { return WorkCurveHelper.currentSpec; }
            set { WorkCurveHelper.currentSpec = value; }
        }

        public static List<SpecListEntity> VirtualSpecList;
        public static List<SpecListEntity> NoActiveVirtualSpecList;

        public static Dictionary<string, bool> VirtualSpecListAdditional;

        public static SpecListEntity DemoSpecList;

        public static void LoadConfig(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            List<MenuConfig> lstConfig = new List<MenuConfig>();
            if (File.Exists(filePath))
            {
                using (FileStream _FileStream = new System.IO.FileStream(filePath,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read,
                    System.IO.FileShare.None
                    ))
                {
                    _FileStream.Position = 0;
                    _FileStream.Seek(0, SeekOrigin.Begin);
                    lstConfig = (List<MenuConfig>)formatter.Deserialize(_FileStream);
                }

            }
            if (lstConfig.Count == 0)
                return;
            foreach (MenuConfig config in lstConfig)
            {
                NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == config.ItemName);
                if (item == null)
                    continue;
                item.Enabled = config.Enable;
                item.ShowInMain = config.ShowInMain;
                item.ShowInToolBar = config.ShowInTools;
                item.FlagType = config.FlagType;
                item.EnabledControl = config.EnableControl;
                item.Text = config.ItemText;
                if (item.Name == "MainPage")
                    item.ShowInMain = true;
            }
            NaviItem itemConnect = WorkCurveHelper.NaviItems.Find(w => w.Name == "ConnectDevice");
            if (itemConnect != null)
            {
                itemConnect.Enabled = true;
                itemConnect.EnabledControl = true;
                itemConnect.ShowInToolBar = true;
            }
        }

        public static SpecListEntity GetbasePureSpec()
        {
            if (WorkCurveCurrent == null || WorkCurveCurrent.ElementList == null || WorkCurveCurrent.ElementList.Items == null || WorkCurveCurrent.ElementList.Items.Count <= 0)
                return null;
            //int baselayer = WorkCurveCurrent.ElementList.Items.ToList().Max(w => w.LayerNumber) - 1 > 0 ? WorkCurveCurrent.ElementList.Items.ToList().Max(w => w.LayerNumber)-1 : 0; 
            int baselayer = WorkCurveCurrent.ElementList.Items.ToList().FindIndex(w => w.LayerNumber == 1);
            SqlParams params1 = new SqlParams("Name", WorkCurveCurrent.ElementList.Items[baselayer].ElementSpecName, false);
            List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params1 });

            if (lstSpecList.Count > 0)
                return lstSpecList[0];
            else
                return null;


        }


        /// <summary>
        /// 谱数据
        /// </summary>
        public static List<SpecData> mainSpecData;

        public static List<ToolStripW> GetToolStripW(int width)
        {
            if (NaviItems == null || NaviItems.Count == 0)
                return null;
            List<ToolStripW> listTools = new List<ToolStripW>();
            ToolStripW toolStripW = new ToolStripW();
            listTools.Add(toolStripW);
            foreach (NaviItem items in WorkCurveHelper.NaviItems)
            {
                if (items.ShowInToolBar)
                {
                    int controlWidth = 0;
                    switch (items.FlagType)
                    {
                        case 0:
                            if (items.BtnDropDown != null)
                            {
                                toolStripW.Items.Add(items.BtnDropDown);
                                controlWidth = items.BtnDropDown.Size.Width;
                            }
                            break;
                        case 1:
                            if (items.LabelStrip != null)
                            {
                                toolStripW.Items.Add(items.LabelStrip);
                                controlWidth = items.LabelStrip.Width;
                            }
                            break;
                        case 2:
                            if (items.ComboStrip != null)
                            {
                                toolStripW.Items.Add(items.ComboStrip);
                                controlWidth = items.ComboStrip.Width;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return listTools;
        }

        public static DataTable DGVToDataTable(DataGridView dgv2)
        {
            DataTable dtbl = new DataTable();
            List<string> columnName = new List<string>();
            foreach (DataGridViewColumn col in dgv2.Columns)
            {
                var list = from test in columnName where String.Compare(col.Name, test, true) == 0 select test;
                if (list.Count() == 0 && col.Visible)
                {
                    columnName.Add(col.Name);
                    dtbl.Columns.Add(col.HeaderText);
                }
            }
            foreach (DataGridViewRow row in dgv2.Rows)
            {
                object[] objs = new object[columnName.Count];
                for (int i = 0; i < columnName.Count; i++)
                {
                    objs[i] = row.Cells[columnName[i]].Value;
                }
                dtbl.Rows.Add(objs);
            }
            dtbl.AcceptChanges();
            return dtbl;
        }

        public static SpecificationsExample MatchSpecifications(ElementList elementList)
        {
            SpecificationsExample returnExample = null;
            if (CategoryCurrent.Specifications.Count > 0)
            {
                double minValue = double.MaxValue;
                foreach (SpecificationsExample examples in CategoryCurrent.Specifications)
                {
                    double totalValue = 0;
                    foreach (SpecificationElement specificationElements in examples.IncludeElements)
                    {
                        CurveElement curveElement = elementList.Items.FirstOrDefault(w => w.Caption == specificationElements.ElementName);
                        if (curveElement != null)
                            totalValue += BetweenSpecifications(curveElement, specificationElements);
                    }
                    if (totalValue < minValue)
                    {
                        minValue = totalValue;
                        returnExample = examples;
                    }
                }
                if (returnExample == null)
                    returnExample = CategoryCurrent.Specifications[0];
            }
            return returnExample;
        }

        public static double BetweenSpecifications(CurveElement curveElement, SpecificationElement specificationElements)
        {
            if (specificationElements.MaxValue == 0 && specificationElements.MinValue == 0)
                return 1;
            if (curveElement.Content > specificationElements.MaxValue)
                return (curveElement.Content - specificationElements.MaxValue) / specificationElements.MaxValue;
            else if (curveElement.Content < specificationElements.MinValue)
                return (specificationElements.MinValue - curveElement.Content) / specificationElements.MinValue;
            else
                return 0;
        }

        public static List<WorkCurve> GetAllCurves()
        {
            List<WorkCurve> allcurves = WorkCurve.FindBySql(@"select distinct a.* from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c 
                                    on b.Device_Id = c.Id where b.type==0 and b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and a.FuncType =" + (int)DeviceFunctype);
            return allcurves;
        }
        public static SpecListEntity GetSpecListByName(string specName)
        {
            SpecListEntity specOld = DataBaseHelper.QueryByEdition(specName, string.Empty, TotalEditionType.Default);
            return specOld;
        }

        public static void ReloadHardwareLanguage()
        {
            if (HardwareDog.HardwareLangStrings.Count > 0)
            {
                HardwareDog.HardwareLangStrings.Clear();
            }
            HardwareDog.HardwareLangStrings.Add("Info.Off", Info.Off);
            HardwareDog.HardwareLangStrings.Add("Info.NetWorkFalse", Info.NetWorkFalse);
            HardwareDog.HardwareLangStrings.Add("Info.SNFalse", Info.SNFalse);
            HardwareDog.HardwareLangStrings.Add("Info.PartSNFalse", Info.PartSNFalse);
            HardwareDog.HardwareLangStrings.Add("Info.RestTimeFalse", Info.RestTimeFalse);
            HardwareDog.HardwareLangStrings.Add("Info.VerFalse", Info.VerFalse);
            HardwareDog.HardwareLangStrings.Add("Info.TimeOut", Info.TimeOut);
            HardwareDog.HardwareLangStrings.Add("Info.Rest", Info.Rest);
            HardwareDog.HardwareLangStrings.Add("Info.DayOut", Info.DayOut);
            HardwareDog.HardwareLangStrings.Add("Info.HVer", Info.HVer);
            HardwareDog.HardwareLangStrings.Add("Info.SVer", Info.SVer);

            HardwareDog.HardwareLangStrings.Add("Info.ActiveSucceed", Info.ActiveSucceed);
            HardwareDog.HardwareLangStrings.Add("Info.ActiveFailed", Info.ActiveFailed);
            HardwareDog.HardwareLangStrings.Add("Info.HardwareDogcbItemsDPP", Info.HardwareDogcbItemsDPP);
            HardwareDog.HardwareLangStrings.Add("Info.HardwareDogcbItemsKV", Info.HardwareDogcbItemsKV);
            HardwareDog.HardwareLangStrings.Add("Info.HardwareDogcbItemsXRay", Info.HardwareDogcbItemsXRay);
            HardwareDog.HardwareLangStrings.Add("Info.HardwareDogcbItemsTime", Info.HardwareDogcbItemsTime);
            HardwareDog.HardwareLangStrings.Add("Info.ActiveDecode", Info.ActiveDecode);
            HardwareDog.HardwareLangStrings.Add("Info.DeviceState", Info.DeviceState);
            HardwareDog.HardwareLangStrings.Add("Info.ActualTime", Info.ActualTime);
            HardwareDog.HardwareLangStrings.Add("Info.EndTime", Info.EndTime);
            HardwareDog.HardwareLangStrings.Add("Info.DeviceVersion", Info.DeviceVersion);
            HardwareDog.HardwareLangStrings.Add("Info.DppSn", Info.DppSn);
            HardwareDog.HardwareLangStrings.Add("Info.VoltageSn", Info.VoltageSn);
            HardwareDog.HardwareLangStrings.Add("Info.XraySn", Info.XraySn);
            HardwareDog.HardwareLangStrings.Add("Info.Active", Info.Active);
            HardwareDog.HardwareLangStrings.Add("Info.Verify", Info.Verify);
            HardwareDog.HardwareLangStrings.Add("Info.ContactTel", Info.ContactTel);
            HardwareDog.HardwareLangStrings.Add("Info.ContactMail", Info.ContactMail);
            HardwareDog.HardwareLangStrings.Add("Info.ContactDer", Info.ContactDer);
            HardwareDog.HardwareLangStrings.Add("Info.Forever", Info.Forever);
            HardwareDog.HardwareLangStrings.Add("Info.DeviceName", Info.DeviceName);
            HardwareDog.HardwareLangStrings.Add("Info.On", Info.On);
        }

        public static bool FunctionEnabled(string functionName)
        {
            return NaviItems.Exists(item => item.Name == functionName && (item.Enabled || item.ShowInToolBar));
        }

        public static void SafeCall(Control con, Action act)
        {
            if (con == null || act == null)
                throw new ArgumentNullException("con or act");
            if (con.InvokeRequired)
            {
                con.Invoke(act);
            }
            else
            {
                act();
            }
        }

        public static void SafeCallAsync(Control con, Action act)
        {
            if (con == null || act == null)
                throw new ArgumentNullException("con or act");
            if (con.InvokeRequired)
            {
                con.BeginInvoke(act);
            }
            else
            {
                act();
            }
        }


    }

    public class MenuLoadHelper
    {

        /// <summary>
        /// 添加菜单项列表
        /// </summary>
        public static List<ToolStripControls> MenuStripCollection = new List<ToolStripControls>();


        /// 获取主菜单
        /// </summary>
        /// <param name="toolsStrip"></param>
        /// <returns></returns>
        public static MenuStripW GetMainFormMenuStrip()
        {
            if (MenuStripCollection == null || MenuStripCollection.Count == 0)
                return null;
            MenuStripW menuStrip = new MenuStripW();
            int maxUnitsInStock = (from prod in MenuStripCollection
                                   select prod.Postion).Max();

            for (int i = 0; i <= maxUnitsInStock; i++)
                AddMenuBar(menuStrip, i);
            return menuStrip;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuStrip"></param>
        /// <param name="position"></param>
        private static void AddMenuBar(MenuStripW menuStrip, int position)
        {
            if (menuStrip == null || position < 0)
                return;
            ToolStripControls toolsRoot = MenuStripCollection.Find(delegate(ToolStripControls ss)
            { return ss.Postion == position && ss.preToolStripMeauItem == null && ss.isRoot; });
            if (toolsRoot == null || toolsRoot.CurrentNaviItem == null || toolsRoot.CurrentNaviItem.MenuStripItem == null)
                return;
            menuStrip.Items.Add(toolsRoot.CurrentNaviItem.MenuStripItem);
            ToolStripControls toolsRootPre = MenuStripCollection.Find(delegate(ToolStripControls ss)
            { return ss.Postion == position && ss.preToolStripMeauItem == toolsRoot; });
            RecurveMenuItem(toolsRootPre, position, toolsRoot);
        }

        private static bool RecurveMenuItem(ToolStripControls preControls, int position, ToolStripControls rootControls)
        {
            if (preControls == null || preControls.CurrentNaviItem == null ||
                preControls.CurrentNaviItem.MenuStripItem == null)
                return false;
            bool flag = false;
            //if (preControls.CurrentNaviItem.Enabled)
            //{
            rootControls.CurrentNaviItem.MenuStripItem.DropDownItems.Add(preControls.CurrentNaviItem.MenuStripItem);
            if (preControls.isExistsChild)
            {
                ToolStripControls toolsControl = MenuStripCollection.Find(w => w.Postion == position && w.parentStripMeauItem == preControls && w.preToolStripMeauItem == null);
                flag = RecurveMenuItem(toolsControl, position, preControls);
            }

            if (!flag)
            {
                ToolStripControls toolsControl = MenuStripCollection.Find(w => w.Postion == position && w.preToolStripMeauItem == preControls);
                flag = RecurveMenuItem(toolsControl, position, rootControls);
            }
            //}
            return flag;
        }

    }

    public class ToolStripControls
    {
        /// <summary>
        /// 在菜单栏中的位置
        /// </summary>
        public int Postion { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public NaviItem CurrentNaviItem;

        public bool isRoot { get; set; }

        public bool isExistsChild { get; set; }

        public string[] Version { get; set; }

        /// <summary>
        public ToolStripControls preToolStripMeauItem { set; get; }

        public ToolStripControls parentStripMeauItem { set; get; }

        public ToolStripControls()
        {
        }

    }

    public class ARMInfo
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public bool RohsEnabled { get; set; }
        public bool ThickEnabled { get; set; }
        public bool XRFEnabled { get; set; }
        public bool FPEnabled { get; set; }
        public bool ECEnabled { get; set; }
        public string Key { get; set; }
    }
}
