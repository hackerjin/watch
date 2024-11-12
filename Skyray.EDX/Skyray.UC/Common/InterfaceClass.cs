using System;
using System.Collections.Generic;
using System.Linq;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Define;
using System.Windows.Forms;
using System.Drawing;
using Skyray.EDX.Common;
using Skyray.Controls;
using Skyray.Camera;
using Skyray.Controls.Extension;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using Skyray.EDX.Common.Component;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using Lephone.Data.Definition;
using System.Threading;

using Skyray.EDX.Common.ReportHelper;
using Skyray.Print;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.Xml;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using Skyray.Print.ReportThreadManage;
using Skyray.EDXRFLibrary.Spectrum;
using System.Drawing.Printing;
using Lephone.Data.Common;
using Aspose.Cells;
using Skyray.API;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Diagnostics;
using InTheHand.Windows.Forms;
using System.Threading;
using System.Windows.Forms;
namespace Skyray.UC
{
    /// <summary>
    /// 软件主要功能接口
    /// </summary>
    public abstract class InterfaceClass
    {

        #region 全局变量
        public string StrTestResult = ExcelTemplateParams.PassResults;
        public int ICurveCalibrateWorkRegion = -1;
        public string SCurveCalibrateWorkCurve = "";
        public string SCurveCalibrateStandSample = "";
        public bool HasAutoDetection = false;
        public bool IsExistHistory = false;

        public bool IsHighSubstrate = false;

        public DeviceMeasure deviceMeasure = new DeviceMeasure();

        public FormWindowState grobleState = FormWindowState.Normal;

        public event EventHandler<BoolEventArgs> OnSateChanged = null;

        /// <summary>
        /// 表示当前状态是否可以进行关盖测试
        /// </summary>
        public bool CanDoTestOnCoverClosed = false;

        /// <summary>
        /// 表示定时器检测到盖子已关的次数, 用于关盖测试
        /// </summary>
        public int TimesCheckCoverClosed = 0;

        /// <summary>
        /// 表示定时器检测到盖子已关的次数, 用于一键测试
        /// </summary>
        public int TimesCheckCoverClosed2 = 0;


        /// <summary>
        /// 表示定时器检测到盖子已关的最大次数, 达到该值时, 开始进行测试
        /// </summary>
        const int MaxTimesCheckCoverClosed = 2;

        public Action ActionBeforeConnectStart;

        public Action ActionAfterTestFinished;

        /// <summary>
        /// 设备条件列表
        /// </summary>
        public List<DeviceParameter> deviceParamsList;
        public Dictionary<int, DeviceParameter> FirstDeviceParamsList = new Dictionary<int, DeviceParameter>();

        //用于由于自动调节计算率，造成修改条件的情况。
        public Dictionary<long, int> currentDeviceParamsList = new Dictionary<long, int>();
        public BlueToothPrinter bluePrint = new BlueToothPrinter();

        public List<HistoryElement> listHistoryElement = new List<HistoryElement>();

        /// <summary>
        /// 摄像头对象
        /// </summary>
        public SkyrayCamera skyrayCamera;

        /// <summary>
        /// 初始化条件
        /// </summary>
        public InitParameter initParams;

        /// <summary>
        /// 
        /// </summary>
        public QualeElementOperation qualeElement;

        /// <summary>
        /// 当前扫描的工作谱
        /// </summary>
        public SpecEntity spec
        {
            set
            {
                _spec = value;
                if (value != null && this.openWorkCurve && !string.IsNullOrEmpty(value.SpecData))
                    BackGroundHelper.ChangeIntVisible(true);

            }
            get { return _spec; }
        }

        private SpecEntity _spec;

        /// <summary>
        /// 当前工作谱列表
        /// </summary>
        public SpecListEntity specList;

        /// <summary>
        /// 菜单项
        /// </summary>
        public List<MenuConfig> lstConfig;

        /// <summary>
        /// 当前选择的模式
        /// </summary>
        public int currentSelectMode;

        /// <summary>
        /// 自动分析出来的元素集合
        /// </summary>
        public string[] ElemsOfQuale;

        /// <summary>
        /// 定性分析中对应元素的类型0为k系，1为L系
        /// </summary>
        public int[] lineStype;

        /// <summary>
        /// 主界面
        /// </summary>
        public Form MainForm;
        private XRFChart chart;
        /// <summary>
        /// 主谱图对象
        /// </summary>
        public XRFChart XrfChart
        {
            set
            {
                chart = value;
                string IsShow = ReportTemplateHelper.LoadSpecifiedValue("Radiation", "ShowIcon");
                if (IsShow == "1")
                {
                    chart.IsRadiation = true;
                }
                else
                {
                    chart.IsRadiation = false;
                }
            }
            get
            {
                return chart;
            }
        }

        /// <summary>
        /// 当前选中的设备条件列表索引
        /// </summary>
        public int deviceParamSelectIndex;

        /// <summary>
        /// 扫描附加信息
        /// </summary>
        public TestDevicePassedParams testDevicePassedParams;

        /// <summary>
        /// 最大的偏移量
        /// </summary>
        public int maxOffSet;

        /// <summary>
        /// 当前扫描的次数
        /// </summary>
        public int currentTestTimes;

        /// <summary>
        /// 如果出现相同名称，是否递增
        /// </summary>
        public bool IsIncrement;

        ///// <summary>
        ///// 判断是否选择了网格
        ///// </summary>
        //public static bool bInIsNetwork;

        ///// <summary>
        ///// 判断是否选择了多点
        ///// </summary>
        //public static bool bInIsMulti;


        /// <summary>
        /// 判断是否是摄像头控件开始测试
        /// </summary>
        public bool bIsCameraStartTest;



        public SpecMessage messageProcess = new SpecMessage();

        /// <summary>
        /// 测量数据容器点击的元素名称
        /// </summary>
        public string elementName;

        // private DataGridViewW dataGridView;

        /// <summary>
        /// 当前选择的模式
        /// </summary>
        public OptMode optMode;


        public Demarcate demacateMode;

        public Resolve deviceResolve = new Resolve();

        /// <summary>
        /// 主要为获取谱图对象，避免注册事件重复
        /// </summary>
        public bool displayFlag;
        public ProgressInfo progressInfo = new ProgressInfo();

        public ToolStripW currentStrip;
        public ExceptClientGrobal uc;

        public bool openWorkCurve = false;
        public bool ConnectState = false;

        public ReportThreadManage reportThreadManage;

        public int iCameraPointCount = 0;
        public int iCurrCameraPointCount = 0;

        public string PreviousRefSpecListIdStr = string.Empty;  //上条曲线默认对比谱名称

        public CustomStandard CurrentStandard; //当前标样数据

        private int InitCurrentTimes = 1;   //初始化走管流次数， 默认三次

        private List<PointF> lstCurCountRate = new List<PointF>();  //细调码与计数率

        public bool bAdjustInitCount = false;
        #endregion


        #region 条码扫描变量
        /// <summary>
        /// 是否进行条码扫码功能
        /// </summary>
        //public bool IsBarcodeScanning = false;
        /// <summary>
        /// 条码扫描后得到的条码样品名称
        /// </summary>
        public string BarcodeScanningSampleName = string.Empty;
        /// <summary>
        /// 条码扫描功能中的样品重量
        /// </summary>
        public string BarcodeScanningWeight = string.Empty;
        /// <summary>
        /// 接收条码重量后是否自动测试
        /// </summary>
        public bool IsAutoMeasure = false;
        /// <summary>
        /// 接收条码重量后自动测试延迟时间
        /// </summary>
        public int IMeasureDelay = 2;
        #endregion

        private List<DemarcateEnergy> CalInterceptEnergys = new List<DemarcateEnergy>();
        /// <summary>
        /// 初始化全局变量，并对扫描谱对象进行赋值
        /// </summary>
        /// <param name="progressBar"></param>
        public void InitalizeInterface()
        {
            specList = new SpecListEntity();
            this.qualeElement = new QualeElementOperation();
            displayFlag = false;
            maxOffSet = 5;
            this.deviceParamsList = new List<DeviceParameter>();
            this.deviceMeasure = WorkCurveHelper.deviceMeasure;

            WorkCurveHelper.deviceMeasure.CreateInitalize();
            RefreshDeviceInitialize(WorkCurveHelper.DeviceCurrent);
            if (WorkCurveHelper.type != InterfaceType.NetWork && WorkCurveHelper.type != InterfaceType.BlueTeeth)
                MotorInstance.LoadDLL(MotorInstance.UpdateKeyFile, WorkCurveHelper.DeviceCurrent);
            if (this.deviceMeasure.interfacce != null)
            {
                this.deviceMeasure.interfacce.OwnerHandle = this.handle;
                this.deviceMeasure.interfacce.Spec = new SpecEntity();
            }
            MotorAdvance.Device = WorkCurveHelper.DeviceCurrent;
            WorkCurveHelper.specMessage = this.messageProcess;
            //this.deviceMeasure.interfacce.CloseDevice();
            //if (this.deviceMeasure.interfacce.port!=null)
            //this.deviceMeasure.interfacce.port.ClosePump();
            this.deviceMeasure.interfacce.InitTime = WorkCurveHelper.InitTime;
            DataBaseInfo();
            SqlConnection.ConnectionString = constr;
        }

        public void WorkStationProcess()
        {
            if (
                GP.CurrentUser != null
                && GP.CurrentUser.Role != null
                && (GP.CurrentUser.Role.RoleType == 0 || GP.CurrentUser.Role.RoleType == 1)
                )
            {
                NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == "CreateToolsAppConfig");
                if (item != null)
                    item.Enabled = true;
            }

            NaviItem itemConnect = WorkCurveHelper.NaviItems.Find(w => w.Name == "ConnectDevice");
            if (itemConnect != null && (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA
                || (WorkCurveHelper.DeviceCurrent.ComType == ComType.USB && WorkCurveHelper.DeviceCurrent.IsDP5 && WorkCurveHelper.DeviceCurrent.Dp5Version == Dp5Version.Dp5_FastNet)))
                itemConnect.Enabled = true;
            else
                itemConnect.Enabled = false;

            NaviItem itemMoveStation = WorkCurveHelper.NaviItems.Find(w => w.Name == "ChamberMove");
            if (itemMoveStation != null)
            {
                if (WorkCurveHelper.DeviceCurrent.HasChamber && WorkCurveHelper.DeviceCurrent.Chamber.Count > 0)
                    itemMoveStation.Enabled = true;
                else
                    itemMoveStation.Enabled = false;
            }
        }

        public void LoadToolsFile(string filePath)
        {
            if (DifferenceDevice.IsAnalyser) ReportTemplateHelper.Edition = "EDXRF";
            else if (DifferenceDevice.IsRohs) ReportTemplateHelper.Edition = "Rohs";
            else if (DifferenceDevice.IsThick) ReportTemplateHelper.Edition = "FPThick";
            else if (DifferenceDevice.IsXRF) ReportTemplateHelper.Edition = "XRF";
            lstConfig = (List<MenuConfig>)MTlSToDragHelper.GetPassedUserDataInitialization(Application.StartupPath + "\\" + filePath);
            if (lstConfig == null || lstConfig.Count == 0)
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
                //item.Text = config.ItemText;
                //修改：何晓明 20110831 增加快捷键
                if (config.ShortcutKeys != null && config.ShortcutKeys.Contains(","))
                    item.MenuStripItem.ShortcutKeys = (Keys)Enum.Parse(typeof(Keys), config.ShortcutKeys.Split(',')[1]) | (Keys)Enum.Parse(typeof(Keys), config.ShortcutKeys.Split(',')[0]);
                else if (config.ShortcutKeys != null && config.ShortcutKeys != "None" && !config.ShortcutKeys.Contains(","))
                    item.MenuStripItem.ShortcutKeys = (Keys)Enum.Parse(typeof(Keys), config.ShortcutKeys);
                //
                if (item.Name == "MainPage")
                    item.ShowInMain = true;
            }
            List<DragDropMenuItem> lstDrapItem = (List<DragDropMenuItem>)MTlSToDragHelper.GetPassedUserDataInitialization(Application.StartupPath + "\\" + Path.GetFileNameWithoutExtension(filePath));
            if (lstDrapItem != null && lstDrapItem.Count > 0)
            {
                lstDrapItem = lstDrapItem.OrderByDescending(w => w.ShowToolPositoin).ToList();
                lstDrapItem.ForEach(w =>
                {
                    MTlSToDragHelper.TranslateToOrignMenuObj(MenuLoadHelper.MenuStripCollection, w);
                    //对WorkCurveHelper.NaviItems集合进行排序
                    var findItem = WorkCurveHelper.NaviItems.Find(wc => wc.Name == w.CurrentItemName);
                    if (findItem != null)
                    {
                        WorkCurveHelper.NaviItems.Remove(findItem);
                        WorkCurveHelper.NaviItems.Insert(0, findItem);
                    }
                });
            }

            string isShowDialog = ReportTemplateHelper.LoadSpecifiedValue("ShowDialog", "IsShow");
            this.IsShowDialog = string.IsNullOrEmpty(isShowDialog) ? false : (int.Parse(isShowDialog) == 1 ? true : false);
            NaviItem ShowDialogData = WorkCurveHelper.NaviItems.Find(w => w.Name == "ShowResultInMain");
            if (ShowDialogData != null)
            {
                ShowDialogData.MenuStripItem.Checked = ShowDialogData.BtnDropDown.Checked = this.IsShowDialog;
                this.refreshFillinof.RefreshMeasureDialog();
            }

            NaviItem testOnButtonPressedItem = WorkCurveHelper.NaviItems.Find(w => w.Name == "TestOnButtonPressedEnabled");
            if (testOnButtonPressedItem != null)
            {
                //testOnButtonPressedItem.MenuStripItem.Checked = testOnButtonPressedItem.BtnDropDown.Checked = WorkCurveHelper.TestOnButtonPressedEnabled;
                testOnButtonPressedItem.MenuStripItem.Checked = WorkCurveHelper.TestOnButtonPressedEnabled;
            }
            ReportTemplateHelper.LoadDirctoryTemplate();

            NaviItem focusItem;
            if (WorkCurveHelper.FocusArea == 1)
            {
                focusItem = WorkCurveHelper.NaviItems.Find(w => w.Name == "FocusMax");
                if (focusItem != null)
                {
                    //testOnButtonPressedItem.MenuStripItem.Checked = testOnButtonPressedItem.BtnDropDown.Checked = WorkCurveHelper.TestOnButtonPressedEnabled;
                    focusItem.MenuStripItem.Checked = true;
                }

            }
            else if (WorkCurveHelper.FocusArea == 2)
            {
                focusItem = WorkCurveHelper.NaviItems.Find(w => w.Name == "FocusMiddle");
                if (focusItem != null)
                {
                    //testOnButtonPressedItem.MenuStripItem.Checked = testOnButtonPressedItem.BtnDropDown.Checked = WorkCurveHelper.TestOnButtonPressedEnabled;
                    focusItem.MenuStripItem.Checked = true;
                }
            }
            else
            {
                focusItem = WorkCurveHelper.NaviItems.Find(w => w.Name == "FocusMin");
                if (focusItem != null)
                {
                    //testOnButtonPressedItem.MenuStripItem.Checked = testOnButtonPressedItem.BtnDropDown.Checked = WorkCurveHelper.TestOnButtonPressedEnabled;
                    focusItem.MenuStripItem.Checked = true;
                }
            }

            //string isShowSelectType = ReportTemplateHelper.LoadSpecifiedValue("SpecExport", "ShowSelectType");
            //WorkCurveHelper.SelectShowType = string.IsNullOrEmpty(isShowSelectType) ? 0 : Convert.ToInt32(isShowSelectType);
            //NaviItem ShowOpenOldSpec = WorkCurveHelper.NaviItems.Find(w => w.Name == "OpenOldSpec");
            //if (ShowOpenOldSpec != null)
            //    ShowOpenOldSpec.MenuStripItem.Checked = ShowOpenOldSpec.BtnDropDown.Checked = WorkCurveHelper.SelectShowType == 1 ? true : false;
            WorkStationProcess();
        }



        //private bool registerSuccess = false;

        /// <summary>
        /// 计算强度或者含量及厚度
        /// </summary>
        /// <param name="workCurve">当前工作曲线</param>
        /// <param name="currentTestTimes">当前的次数</param>
        /// <param name="localTaskList">任务列表</param>
        /// <returns></returns>
        public void PaintSpecData(SpecListEntity specList, int currentTestTimes)
        {
            if (WorkCurveHelper.MainSpecList.Specs.Length == 1)
            {
                WorkCurveHelper.CurrentSpec = WorkCurveHelper.MainSpecList.Specs[0];
                this.spec = WorkCurveHelper.CurrentSpec;
                this.refreshFillinof.RefreshSpec(this.specList, this.spec);
                XrfChart.ShowAloneSpec(WorkCurveHelper.WorkCurveCurrent, this.spec, WorkCurveHelper.VirtualSpecList, false, DifferenceDevice.DefaultSpecColor.ToArgb());

            }
            else if (WorkCurveHelper.MainSpecList.Specs.Length > 1)
            {
                XrfChart.MultiPanel(WorkCurveHelper.WorkCurveCurrent, WorkCurveHelper.MainSpecList, WorkCurveHelper.VirtualSpecList, this.spec, DifferenceDevice.DefaultSpecColor.ToArgb(), XrfChart.GraphPane.Chart.Fill.Color);
            }
        }

        public abstract void LoadToolsConfig();

        /// <summary>
        /// 软件特有的模式处理
        /// </summary>
        /// <param name="optMode">特定的模式</param>

        public abstract void MessageStopMove(OptMode optMode);


        ///<summary>
        /// 扫描未知样品处理逻辑
        /// </summary>
        /// <param name="workCurve">当前工作谱</param>
        /// <param name="testDeviceParams">扫描参数对象</param>
        public abstract void MeasureUnkownSpecProcess(WorkCurve workCurve, TestDevicePassedParams testDeviceParams);

        /// <summary>
        /// 扫描终止处理函数
        /// </summary>
        /// <param name="optMode">当前模式</param>
        /// <param name="usedTime">使用的时间</param>
        /// <param name="workcurve">当前工作曲线</param>
        public abstract void CallTerminateTestFun(OptMode optMode, int usedTime, WorkCurve workcurve);

        /// <summary>
        /// 智能模式处理
        /// </summary>
        public abstract void ExploreModeCaculate();

        /// <summary>
        /// 对摄像头对象进行查找
        /// </summary>
        /// <param name="clientPannel">主客户区的panel</param>
        /// <returns></returns>
        public abstract SkyrayCamera FindCameralUserControl(ContainerObject clientPannel);
        /// <summary>
        /// 收到谱图数据同时更新谱控件
        /// </summary>
        /// <param name="usedTime">使用的时间</param>
        public virtual void UpdateSpecGraphic(int usedTime)
        {
            if (usedTime <= 0.000001)
                return;
            progressInfo.Maximum = (this.optMode != OptMode.Initalize ? this.deviceParamsList[this.deviceParamSelectIndex].PrecTime : this.deviceMeasure.interfacce.InitTime);
            if (progressInfo.Maximum == progressInfo.Value && WorkCurveHelper.IsAutoIncrease)
            {
                progressInfo.LabelMeasureTime.Text = progressInfo.Maximum + "s";
            }
            if (usedTime >= progressInfo.Minimum && usedTime <= this.progressInfo.Maximum)
                progressInfo.Value = usedTime;
            int differenceUsedTime = (this.optMode != OptMode.Initalize ? this.deviceParamsList[this.deviceParamSelectIndex].PrecTime : this.deviceMeasure.interfacce.InitTime) - usedTime;
            progressInfo.SurplusTime = differenceUsedTime + "s";
            progressInfo.MeasureTime = (this.optMode != OptMode.Initalize ? this.deviceParamsList[this.deviceParamSelectIndex].PrecTime : this.deviceMeasure.interfacce.InitTime) + "s";
            if (this.XrfChart != null && this.spec != null && this.spec.SpecData != null)
            {
                this.spec.UsedTime = this.deviceMeasure.interfacce.usedTime;
                this.spec.DeviceParameter = this.deviceParamsList[this.deviceParamSelectIndex].ConvertFrom();
                WorkCurveHelper.MainSpecList = null;
                WorkCurveHelper.CurrentSpec = this.spec;
                int[] specDataInt = this.spec.SpecDatas;
                List<SpecData> listSpec = new List<SpecData>();
                if (specDataInt.Length == 0) return;
                //int[] specDataInt, BaseSpecDataInt = null;
                //if (WorkCurveHelper.isShowBase)
                //{
                //    BaseSpecDataInt = this.spec.SpecDatas;
                //    specDataInt = this.spec.OriginSpecData;
                //    BaseSpecDataInt = this.spec.BaseData;
                //}
                //else
                //    specDataInt = this.spec.SpecDatas;
                //List<SpecData> listSpec = new List<SpecData>();
                //List<SpecData> viceListSpec = new List<SpecData>();
                if (specDataInt == null || specDataInt.Length == 0) return;
                for (int i = 0; i < specDataInt.Length; i++)
                {
                    SpecData specData = new SpecData(i, specDataInt[i]);
                    listSpec.Add(specData);
                }
                //if (WorkCurveHelper.isShowBase)
                //{
                //    for (int m = 0; m < BaseSpecDataInt.Length; m++)
                //    {
                //        SpecData specData = new SpecData(m, BaseSpecDataInt[m]);
                //        viceListSpec.Add(specData);
                //    }

                //}
                XrfChart.CurrentSpecPanel = deviceParamSelectIndex + 1;
                XrfChart.SpecDataDic[deviceParamSelectIndex] = listSpec;
                //if (WorkCurveHelper.isShowBase)
                //{
                //    XrfChart.ViceSpecDataDic.Remove(deviceParamSelectIndex);
                //    XrfChart.ViceSpecDataDic.Add(deviceParamSelectIndex, viceListSpec);
                //}
                XrfChart.SpecDataDic.Remove(deviceParamSelectIndex);
                XrfChart.SpecDataDic.Add(deviceParamSelectIndex, listSpec);
                if (RatioVisual)
                    RatioVisualData(usedTime);
                XrfChart.SetSpecData(WorkCurveHelper.WorkCurveCurrent, WorkCurveHelper.VirtualSpecList, this.spec, true, DifferenceDevice.DefaultSpecColor.ToArgb(), WorkCurveHelper.Growstyle);
                bool refresh = true;
                try
                {
                    refresh = bool.Parse(ConfigurationSettings.AppSettings["RefreshBySencond"]);
                }
                catch { }

                if (optMode == OptMode.Test && refresh && this.deviceParamsList != null && this.deviceParamsList.Count == 1
    && WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0
                  && (WorkCurveHelper.WorkCurveCurrent.FuncType == FuncType.Thick || WorkCurveHelper.WorkCurveCurrent.ElementList.Items[0].Samples.Count > 0))
                {
                    List<SpecListEntity> listTemp = new List<SpecListEntity>();
                    listTemp.Add(this.specList);

                    if (!(DifferenceDevice.IsThick && bIsCameraStartTest && this.skyrayCamera.ContiTestPoints.Count > 0))//Thick中，如果使用多点或网格测试时，不进行每秒结果显示
                        CaculateContent(listTemp, this.currentTestTimes, false);
                }

                if (optMode == OptMode.Explore && refresh && this.specList != null)
                {
                    AtomNamesDic.Clear();
                    AtomLinesDic.Clear();
                    IsExistHistory = false;
                    this.specList.DemarcateEnergys = Default.ConvertFormOldToNew(WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys, WorkCurveHelper.DeviceCurrent.SpecLength);
                    WorkCurveHelper.MainSpecList = this.specList;
                    AutoAnalysisProcess(null);
                    ExploreModeCaculate();
                }
                if (DoOtherFormReceive != null)
                {
                    DoOtherFormReceive(WorkCurveHelper.WorkCurveCurrent.ElementList, progressInfo.Maximum, usedTime);
                }
            }

        }

        public static List<SpecEntity> recordLstSpec = new List<SpecEntity>();

        public void AddExcelData()
        {

            string path = Application.StartupPath + @"\ApplicationData.xls";

            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            Aspose.Cells.Cells cells = workbook.Worksheets[0].Cells;
            for (int i = 0; i < recordLstSpec.Count; i++)
            {
                var cell = cells[i, 0];
                cell.PutValue(recordLstSpec[i].Name);
                //for (int j = 0; j < recordLstSpec[i].SpecDatas.Length; j++)
                //{
                //    cell = cells[i, j + 1];
                //    cell.PutValue(recordLstSpec[i].SpecDatas[j]);
                //}
                cell = cells[i, 1];
                int count = 0;
                for (int j = 0; j < recordLstSpec[i].SpecDatas.Length; j++)
                {
                    count += recordLstSpec[i].SpecDatas[j];
                }
                cell.PutValue(count);
            }

            workbook.Save(path);

            //int k = 0;
            //bool hasRecord = false;
            //for (int j = k; j < this.ColumnCount; j++)
            //{
            //    var col = this.Columns[j];
            //    if (col.Visible)
            //    {
            //        var cell = cells[0, k];
            //        cell.PutValue(col.HeaderText);
            //        for (int i = 0; i < this.RowCount; i++)
            //        {
            //            cell = cells[i + 1, k];
            //            var obj = this[j, i].Value;
            //            if (obj != null)
            //            {
            //                var typ = obj.GetType();
            //                if (typ == typeof(DateTime))
            //                {
            //                    cell.Style.Custom = "yyyy-mm-dd hh:mm:ss";
            //                    cell.PutValue(obj.ToString());
            //                }
            //                else if (typ == typeof(string))
            //                {
            //                    var str = obj.ToString();
            //                    if (str.IsNum())
            //                        cell.PutValue(double.Parse(str));
            //                    else
            //                        cell.PutValue(str);
            //                }
            //                else
            //                {
            //                    cell.PutValue(obj);
            //                }
            //                hasRecord = true;
            //            }
            //        }
            //        k++;
            //    }
            //}
            //if (hasRecord)
            //{
            //    SaveFileDialog sdlg = new SaveFileDialog();
            //    sdlg.Filter = "Excel File(*.xls)|*.xls";
            //    if (sdlg.ShowDialog() == DialogResult.OK)
            //    {
            //        workbook.Save(sdlg.FileName);
            //    }
            //}
            //Excel.Application app = new Excel.Application();
            //Excel.Workbooks workBooks = app.Workbooks;
            //Excel.Workbook workBook = workBooks.Open(path, 3, false, 5, "", "", true, "", "", true, false, 0, true, false, false);
            //Excel.Sheets sheets = workBook.Sheets;
            //Excel._Worksheet sheet = (Excel._Worksheet)sheets.get_Item(1);
            //int nRowIndex = 1;

            //string s = "1,2,3\r\n4,5,6\r\n";
            //string[] s1 = s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (string s2 in s1)
            //{
            //    int nColIndex = 1;

            //    string[] s3 = s2.Split(',');
            //    foreach (string s4 in s3)
            //    {
            //        sheet.Cells[nRowIndex, nColIndex] = s4;
            //        nColIndex++;
            //    }

            //    nRowIndex++;
            //}

            //System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
            //workBook.Save();
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
            //workBooks.Close();
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(workBooks);
            //app.Workbooks.Close();
            //app.Quit();
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            //Excel.Application excel = new Excel.ApplicationClass();
            //excel.Visible = false;
            //Microsoft.Office.Interop.Excel.Workbook wb = (Microsoft.Office.Interop.Excel._Workbook)excel.Application.Workbooks.Add(true);
            //Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel._Worksheet)wb.Worksheets.Add("miss", "m", "m", "m");
            //ws = (Microsoft.Office.Interop.Excel._Worksheet)wb.Worksheets.Add("miss", "m", "m", "m");
            //int row = ws.UsedRange.Rows.Count;
            //((Microsoft.Office.Interop.Excel.Range)ws.Cells[row, 1]).Value2 = "1";
        }

        /// <summary>
        /// 根据谱控件的名称找到对象实例
        /// </summary>
        /// <param name="pannel">主客户区对象</param>
        public virtual void GetSpecGraphicObject(ContainerObject panel)
        {
            this.XrfChart.IXMaxChannel = (int)WorkCurveHelper.DeviceCurrent.SpecLength;
            if (this.XrfChart == null)
                return;
            messageProcess.StartThread(this.XrfChart);
            this.XrfChart.OnAddPeakFlag += new EventHandler(xrfChart_OnAddPeakFlag);
            this.XrfChart.OnDelPeakFlag += new EventHandler(xrfChart_OnDelPeakFlag);
            this.XrfChart.OnLeftBorder += new EventHandler<GraphEventArgs>(xrfChart_OnLeftBorder);
            this.XrfChart.OnRightBorder += new EventHandler<GraphEventArgs>(xrfChart_OnRightBorder);
            this.XrfChart.OnLeftBase += new EventHandler<GraphEventArgs>(xrfChart_OnLeftBase);
            this.XrfChart.OnRightBase += new EventHandler<GraphEventArgs>(xrfChart_OnRightBase);
            this.XrfChart.OnVirtualSpec += new EventHandler(xrfChart_OnVirtualSpec);
            if (this.XrfChart.GraphPane.XAxis.Scale.Max == 0)  //yuzhao20150611：为了是第一次打开谱图的时候显示的比例与小谱图一致，限制设置X轴放大比例与谱长度一致的功能
                this.XrfChart.MasterPane.PaneList[0].XAxis.Scale.Max = WorkCurveHelper.DeviceCurrent == null ? 2048 : (int)WorkCurveHelper.DeviceCurrent.SpecLength;
        }

        void xrfChart_OnVirtualSpec(object sender, EventArgs e)
        {
            UCVirtualSelect selectVirtual = new UCVirtualSelect();
            WorkCurveHelper.OpenUC(selectVirtual, false, Info.OpenVirtualSpec, true);
        }

        void xrfChart_OnRightBase(object sender, GraphEventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0 && this.elementName != null)
            {
                CurveElement elements = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => String.Compare(elementName, w.Caption, true) == 0 || (w.IsOxide && String.Compare(elementName, w.Formula, true) == 0));
                elements.BaseHigh = e.Channel;
                //elements.Save();
                string stringSql = "update CurveElement set BaseHigh =" + e.Channel + " where id=" + elements.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
            }
        }

        void xrfChart_OnLeftBase(object sender, GraphEventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0 && this.elementName != null)
            {
                CurveElement elements = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => String.Compare(elementName, w.Caption, true) == 0 || (w.IsOxide && String.Compare(elementName, w.Formula, true) == 0));
                elements.BaseLow = e.Channel;
                //elements.Save();
                string stringSql = "update CurveElement set BaseLow =" + e.Channel + " where id=" + elements.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
            }
        }

        void xrfChart_OnRightBorder(object sender, GraphEventArgs e)
        {
            CurveElement element = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => String.Compare(elementName, w.Caption, true) == 0 || (w.IsOxide && String.Compare(elementName, w.Formula, true) == 0));
            if (e.Channel <= element.PeakLow)
            {
                element.PeakLow = e.Channel;
            }
            element.PeakHigh = e.Channel;
            //element.Save();
            string stringSql = "update CurveElement set PeakLow =" + element.PeakLow + ",PeakHigh=" + element.PeakHigh + "  where id=" + element.Id;
            Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
            DrawBorderArea(element.PeakLow, element.PeakHigh, Color.FromArgb(element.Color));
        }

        void xrfChart_OnLeftBorder(object sender, GraphEventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0 && this.elementName != null)
            {
                CurveElement element = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => String.Compare(elementName, w.Caption, true) == 0 || (w.IsOxide && String.Compare(elementName, w.Formula, true) == 0));
                if (e.Channel >= element.PeakHigh)
                {
                    element.PeakHigh = e.Channel;
                }
                element.PeakLow = e.Channel;
                //element.Save();
                string stringSql = "update CurveElement set PeakLow =" + element.PeakLow + ",PeakHigh=" + element.PeakHigh + "  where id=" + element.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
                DrawBorderArea(element.PeakLow, element.PeakHigh, Color.FromArgb(element.Color));
            }
        }
        void DrawBorderArea(int left, int right, Color color)
        {
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.CurrentSpec != null)
            {
                XrfChart.SetSpecData(WorkCurveHelper.WorkCurveCurrent, WorkCurveHelper.VirtualSpecList, WorkCurveHelper.CurrentSpec, false, DifferenceDevice.DefaultSpecColor.ToArgb(), WorkCurveHelper.Growstyle);
            }

        }
        void xrfChart_OnDelPeakFlag(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent == null)
                return;
            DemarcateEnergy enery = this.XrfChart.ReadyDeleteDE;
            XrfChart.DemarcateEnergys.Remove(enery);
            enery.Delete();
            DelPeakFlag(enery);
        }

        void xrfChart_OnAddPeakFlag(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent == null)
                return;
            DemarcateEnergy en = this.XrfChart.ReadyAddDE;
            DemarcateEnergy de = XrfChart.DemarcateEnergys.ToList().Find(d => d.ElementName.Equals(en.ElementName) && d.Line == en.Line);
            double dchannel = en.Channel;
            if (double.Parse(sender.ToString()) > 0)
            {
                dchannel = double.Parse(sender.ToString());
            }
            else if (this.spec != null && this.spec.SpecDatas.Length > 10)
            {
                dchannel = SpecHelper.FitChannOfMaxValue((int)en.Channel - 15, (int)en.Channel + 15, this.spec.SpecDatas);
            }
            if (de != null)
            {
                de.Energy = en.Energy;
                de.Channel = dchannel;
                string stringSql = "update DemarcateEnergy set Energy =" + en.Energy + ",Channel=" + dchannel + " where id=" + de.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
            }
            else
            {
                en.Condition = WorkCurveHelper.WorkCurveCurrent.Condition;
                string stringSql = "insert into DemarcateEnergy(ElementName, Line, Energy, Channel, Condition_Id) values('" + en.ElementName + "'," + (int)en.Line + "," + en.Energy + "," + dchannel + "," + en.Condition.Id + ")";
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
                stringSql = "select Max(Id) from DemarcateEnergy";
                object obj = Lephone.Data.DbEntry.Context.ExecuteScalar(stringSql);
                int id = 1;
                try
                {
                    id = int.Parse(obj.ToString());
                }
                catch
                {
                    id = 1;
                }
                DemarcateEnergy newFind = DemarcateEnergy.FindById(id);
                XrfChart.DemarcateEnergys.Add(newFind);
            }
            AddPeakFlag(en);
        }

        public virtual void AddPeakFlag(DemarcateEnergy energy)
        {
            WorkCurveHelper.WorkCurveCurrent.Condition = Condition.FindById(WorkCurveHelper.WorkCurveCurrent.Condition.Id);
        }

        public virtual void DelPeakFlag(DemarcateEnergy energy)
        {
            WorkCurveHelper.WorkCurveCurrent.Condition = Condition.FindById(WorkCurveHelper.WorkCurveCurrent.Condition.Id);
        }
        /// <summary>
        /// 全部条件结束
        /// </summary>
        private bool IsInitialAllComplete = false;
        private bool IsInitialSucceed = false;
        /// <summary>
        /// 初始化结束处理函数
        /// </summary>
        /// <param name="success">是否成功</param>
        public virtual void TestInitilizeEnd(bool success)
        {

            IsInitialSucceed = success;
            if (!success)
            {
                if (progressInfo.Maximum == progressInfo.Value && WorkCurveHelper.IsAutoIncrease)
                {
                    progressInfo.LabelMeasureTime.Text = progressInfo.Maximum + "s";
                }

                Msg.Show(Info.InitialFaile, Info.InitialInformation, MessageBoxButtons.OK, MessageBoxIcon.Error);

                TestStartAfterControlState(true);
                progressInfo.Value = 0;

                if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                {

                    MotorOperator.MotorOperatorY1Thread((int)(WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));

                    DifferenceDevice.CurCameraControl.skyrayCamera1.Start();

                }
                return;
            }
            if (bAdjustInitCount)
            {
                this.initParams.InitCalibrateRatio = DifferenceDevice.interClassMain.deviceMeasure.interfacce.CountRating > 0 ? this.initParams.InitFistCount / DifferenceDevice.interClassMain.deviceMeasure.interfacce.CountRating : 1;
                //更新校正强度参数
                string sql = "Update InitParameter Set InitCalibrateRatio = " + this.initParams.InitCalibrateRatio;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);

            }


            lstCurCountRate.Add(new PointF((float)deviceMeasure.interfacce.ReturnCountRate, this.initParams.FineGain));
            if (WorkCurveHelper.bCurrentInfluenceGain && InitCurrentTimes < WorkCurveHelper.InitCurrentList.Count && (!bAdjustInitCount))
                DeviceInterface.PostMessage(this.handle, DeviceInterface.Wm_InitAddCurrent, true, 0);
            else
            {
                if (this.deviceMeasure.interfacce != null)
                    this.deviceMeasure.interfacce.CloseDevice();  //最后再关高压
                //放大倍数根据管流变化
                if (WorkCurveHelper.bCurrentInfluenceGain)
                {

                    //放大倍数的计算参数
                    double[] coeff = new double[3];
                    var pp = lstCurCountRate.Distinct().ToArray();
                    DifferenceDevice.CalculateCurve(pp, 2, false, coeff);
                    initParams.ExpressionFineGain = coeff[0].ToString("f20") + "*x*x" + (coeff[1] >= 0 ? "+" : "") + coeff[1].ToString("f20") + "*x" + (coeff[2] >= 0 ? "+" : "") + coeff[2].ToString("f20");
                    if (WorkCurveHelper.bShowInitParam)
                    {
                        using (FileStream fileStream = new FileStream(Application.StartupPath + @"\TestInit.txt", FileMode.Append))
                        {
                            using (StreamWriter sw = new StreamWriter(fileStream))
                            {//+ "校正后计数总和：" + calibrationData
                                foreach (var p in pp)
                                {
                                    sw.WriteLine("初始化中：  px:" + p.X.ToString() + "py:" + p.Y.ToString());
                                }
                                sw.WriteLine("初始化Expression：  " + initParams.ExpressionFineGain);
                            }
                        }
                    }
                }
                else
                {
                    initParams.ExpressionFineGain = "x";
                }
                if (!WorkCurveHelper.bInitialize)
                {
                    if (optMode == OptMode.Initalize)
                    {
                        if (progressInfo.Maximum == progressInfo.Value && WorkCurveHelper.IsAutoIncrease)
                        {
                            progressInfo.LabelMeasureTime.Text = progressInfo.Maximum + "s";
                        }
                        if (!bAdjustInitCount)
                        {
                            Msg.Show(Info.InitializeSuccess);
                            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                            {
                                MotorOperator.MotorOperatorY1Thread((int)(WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                                DifferenceDevice.CurCameraControl.skyrayCamera1.Start();
                            }
                        }
                        else
                            Msg.Show(Info.AdjustFinshed);
                    }
                    string sql = "Update InitParameter Set Gain = " + this.initParams.Gain + ", FineGain = "
                                 + this.initParams.FineGain + ", ExpressionFineGain = '" + this.initParams.ExpressionFineGain + "'";
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);

                    //修改预热参数，根据初始化后得到的粗调码和精调码
                    string preheatparamssql = "Update preheatparams Set Gain = " + this.initParams.Gain + ", FineGain = " + this.initParams.FineGain;
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(preheatparamssql);


                    Condition condition = Condition.FindOne(w => w.Type == ConditionType.Intelligent && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    if (condition != null)
                    {
                        sql = "Update InitParameter Set Gain = " + this.initParams.Gain + ", FineGain = " + this.initParams.FineGain + ", ElemName = '"
                            + this.initParams.ElemName + "', Channel = " + this.initParams.Channel + ", ExpressionFineGain = '" + this.initParams.ExpressionFineGain + "'";
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    }

                    condition = Condition.FindOne(w => w.Type == ConditionType.Match && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    if (condition != null)
                    {
                        sql = "Update InitParameter Set Gain = " + this.initParams.Gain + ", FineGain = " + this.initParams.FineGain + ", ElemName = '"
                            + this.initParams.ElemName + "', Channel = " + this.initParams.Channel + ", ExpressionFineGain = '" + this.initParams.ExpressionFineGain + "'";
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    }
                    condition = Condition.FindOne(w => w.Type == ConditionType.Detection && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    if (condition != null)
                    {
                        sql = "Update InitParameter Set Gain = " + this.initParams.Gain + ", FineGain = " + this.initParams.FineGain + ", ElemName = '"
                            + this.initParams.ElemName + "', Channel = " + this.initParams.Channel + ", ExpressionFineGain = '" + this.initParams.ExpressionFineGain + "'";
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    }
                    condition = Condition.FindOne(w => w.Type == ConditionType.Match2 && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    if (condition != null)
                    {
                        sql = "Update InitParameter Set Gain = " + this.initParams.Gain + ", FineGain = " + this.initParams.FineGain + ", ElemName = '"
                            + this.initParams.ElemName + "', Channel = " + this.initParams.Channel + ", ExpressionFineGain = '" + this.initParams.ExpressionFineGain + "'";
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    }

                    progressInfo.Value = 0;
                    deviceParamSelectIndex = 0;
                    TestStartAfterControlState(true); //对界面中的控件进行恢复
                }
                else
                {
                    if (success && WorkCurveHelper.bInitialize)
                    {
                        string sqlTemp = "select * from Condition where Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + "";
                        List<Condition> currentConditon = Condition.FindBySql(sqlTemp);
                        if (currentConditon != null && currentConditon.Count > 0)
                        {
                            foreach (Condition temp in currentConditon)
                            {
                                if (temp.InitParam.ElemName == initParams.ElemName)  //相同初始化元素情况下全部更新
                                {
                                    string sqltemp = "Update InitParameter Set TubVoltage=" + this.initParams.TubVoltage + ",TubCurrent=" + this.initParams.TubCurrent + ", Gain = " + this.initParams.Gain + ", FineGain = "
                                                + this.initParams.FineGain + ", ExpressionFineGain = '" + this.initParams.ExpressionFineGain + "',Channel=" + this.initParams.Channel + ",Filter=" + this.initParams.Filter + ",Collimator=" + this.initParams.Collimator + ",ChannelError=" + this.initParams.ChannelError + ",ElemName='" + this.initParams.ElemName + "' Where Id = " + temp.InitParam.Id;
                                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqltemp);
                                }
                            }
                            //重新赋值当前曲线的初始化参数
                            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
                        }
                    }
                    //CalcFwhm.Calc((int)this.deviceMeasure.interfacce.MaxChannelRealTime, this.spec.SpecDatas);
                    if (IsInitialAllComplete)
                    {
                        if (optMode == OptMode.Initalize)
                        {
                            if (progressInfo.Maximum == progressInfo.Value && WorkCurveHelper.IsAutoIncrease)
                            {
                                progressInfo.LabelMeasureTime.Text = progressInfo.Maximum + "s";
                            }
                            if (!bAdjustInitCount)
                            {
                                Msg.Show(Info.InitializeSuccess);
                                if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                                {
                                    MotorOperator.MotorOperatorY1Thread((int)(WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                                    DifferenceDevice.CurCameraControl.skyrayCamera1.Start();
                                }
                            }
                            else
                                Msg.Show(Info.AdjustFinshed);
                        }
                        progressInfo.Value = 0;
                        deviceParamSelectIndex = 0;
                        TestStartAfterControlState(true); //对界面中的控件进行恢复
                    }
                    else
                    {
                        DeviceInterface.PostMessage(this.handle, DeviceInterface.Wm_NextInitializationElem, true, 0);
                    }
                }
            }
            bAdjustInitCount = false;



        }


        //public virtual void TestInitilizeEnd()
        //{
        //    string sql = "Update InitParameter Set Gain = " + this.initParams.Gain + ", FineGain = "
        //                     + this.initParams.FineGain + " Where Id = " + this.initParams.Id;
        //    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);

        //    //修改预热参数，根据初始化后得到的粗调码和精调码
        //    string preheatparamssql = "Update preheatparams Set Gain = " + this.initParams.Gain + ", FineGain = " + this.initParams.FineGain;
        //    Lephone.Data.DbEntry.Context.ExecuteNonQuery(preheatparamssql);


        //    Condition condition = Condition.FindOne(w => w.Type == ConditionType.Intelligent && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
        //    if (condition != null)
        //    {
        //        sql = "Update InitParameter Set Gain = " + this.initParams.Gain + ", FineGain = " + this.initParams.FineGain + ", ElemName = '"
        //            + this.initParams.ElemName + "', Channel = " + this.initParams.Channel + " Where Id = " + condition.InitParam.Id;
        //        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
        //    }

        //    condition = Condition.FindOne(w => w.Type == ConditionType.Match && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
        //    if (condition != null)
        //    {
        //        sql = "Update InitParameter Set Gain = " + this.initParams.Gain + ", FineGain = " + this.initParams.FineGain + ", ElemName = '"
        //            + this.initParams.ElemName + "', Channel = " + this.initParams.Channel + " Where Id = " + condition.InitParam.Id;
        //        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
        //    }

        //    CalcFwhm.Calc((int)this.deviceMeasure.interfacce.MaxChannelRealTime, this.spec.SpecDatas);
        //    progressInfo.Value = 0;
        //    deviceParamSelectIndex = 0;
        //    TestStartAfterControlState(true); //对界面中的控件进行恢复
        //}


        public List<SpecListEntity> selectSpeclist = new List<SpecListEntity>();
        /// <summary>
        /// 打开谱
        /// </summary>
        /// <param name="specList">谱数据</param>
        public virtual void OpenWorkSpec(List<SpecListEntity> specList, bool caculate)
        {
            if (this.XrfChart == null)
                return;
            if (specList == null || specList.Count == 0)
                return;
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE, 0, 0);

            OpenWorkCurve(specList[0]);
            this.ElemsOfQuale = null;
            this.lineStype = null;
            openSpecFlag = false;
            WorkCurveHelper.MainSpecList = specList[0];
            this.specList = specList[0];
            this.selectSpeclist = specList;
            if (this.XrfChart.DemarcateEnergys == null)
            {
                List<DemarcateEnergy> tt = new List<DemarcateEnergy>();
                this.XrfChart.DemarcateEnergys = Default.ConvertFromNewOld(specList[0].DemarcateEnergys.ToList(), WorkCurveHelper.DeviceCurrent.SpecLength);
            }
            XrfChart.SpecDataDic.Clear();
            if (this.IsShowLogSpectrum)
            {
                this.CheckLogSpectrum(true);
            }
            else
            {
                if (WorkCurveHelper.MainSpecList.Specs.Length == 1)
                {
                    WorkCurveHelper.CurrentSpec = WorkCurveHelper.MainSpecList.Specs[0];
                    this.spec = WorkCurveHelper.CurrentSpec;
                    this.refreshFillinof.RefreshSpec(this.specList, this.spec);
                    List<SpecData> listSpec = new List<SpecData>();
                    int[] specDataInt = this.spec.SpecDatas;
                    //List<SpecData> viceListSpec = new List<SpecData>();
                    //int[] specDataInt, BaseSpecDataInt = null;
                    //if (WorkCurveHelper.isShowBase == true)
                    //{
                    //    BaseSpecDataInt = this.spec.SpecDatas;
                    //    specDataInt = this.spec.OriginSpecData;
                    //    BaseSpecDataInt = this.spec.BaseData;
                    //}
                    //else
                    //    specDataInt = this.spec.SpecDatas;
                    for (int m = 0; m < specDataInt.Length; m++)
                    {
                        SpecData specData = new SpecData(m, specDataInt[m]);
                        listSpec.Add(specData);
                    }
                    //if (WorkCurveHelper.isShowBase)
                    //{
                    //    for (int m = 0; m < BaseSpecDataInt.Length; m++)
                    //    {
                    //        SpecData specData = new SpecData(m, BaseSpecDataInt[m]);
                    //        viceListSpec.Add(specData);
                    //    }

                    //}
                    XrfChart.CurrentSpecPanel = 1;
                    XrfChart.SpecDataDic.Remove(0);
                    XrfChart.SpecDataDic.Add(0, listSpec);
                    //if (WorkCurveHelper.isShowBase)
                    //{
                    //    XrfChart.ViceSpecDataDic.Remove(0);
                    //    XrfChart.ViceSpecDataDic.Add(0, viceListSpec);
                    //}
                    XrfChart.ShowAloneSpec(WorkCurveHelper.WorkCurveCurrent, this.spec, WorkCurveHelper.VirtualSpecList, false, WorkCurveHelper.MainSpecList.Color == 0 ? DifferenceDevice.DefaultSpecColor.ToArgb() : WorkCurveHelper.MainSpecList.Color);
                    //XrfChart.ShowAloneSpec(WorkCurveHelper.WorkCurveCurrent, this.spec, WorkCurveHelper.VirtualSpecList, false, DifferenceDevice.DefaultSpecColor.ToArgb());
                }
                else if (WorkCurveHelper.MainSpecList.Specs.Length > 1)
                {
                    WorkCurveHelper.CurrentSpec = WorkCurveHelper.MainSpecList.Specs[0];
                    this.spec = WorkCurveHelper.CurrentSpec;
                    this.refreshFillinof.RefreshSpec(this.specList, this.spec);
                    WorkCurveHelper.MainSpecList.Color = WorkCurveHelper.MainSpecList.Color == 0 ? DifferenceDevice.DefaultSpecColor.ToArgb() : WorkCurveHelper.MainSpecList.Color;
                    XrfChart.MultiPanel(WorkCurveHelper.WorkCurveCurrent, WorkCurveHelper.MainSpecList, WorkCurveHelper.VirtualSpecList, this.spec, DifferenceDevice.DefaultSpecColor.ToArgb(), XrfChart.GraphPane.Chart.Fill.Color);
                }

                if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null && caculate)
                {
                    if (this.currentSelectMode == 1 || WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                        CaculateExcute(false, !WorkCurveHelper.IsOpenSpecByHistoryRecord);
                }

                //  匹配功能2013-05-22-------------------------------------------------------------------------
                if (WorkCurveHelper.IShowQuickInfo && WorkCurveHelper.MatchType == 1 && WorkCurveHelper.IsMatchAuto)//存放于块成型的文档
                {
                    string matchInfos = string.Empty;
                    for (int i = 0; i < specList.Count; i++)
                    {
                        string MatchElements = string.Empty;
                        List<DemarcateEnergy> temp = new List<DemarcateEnergy>();
                        if (specList[i].DemarcateEnergys != null)
                            specList[i].DemarcateEnergys.ToList().ForEach(w => temp.Add(w.ConvertFrom()));
                        bool bIsHighSubstrate = false;
                        WorkCurve ss = null;
                        if (CatchBGTypge(specList[i].Specs[0], temp) == 2 && specList[i].Specs.Length >= 2)//轻基体
                        {
                            bIsHighSubstrate = true;
                            ss = ToCatchCurveByThreeMainElements((int)FuncType.XRF, specList[i].Specs[1], temp, ref MatchElements, bIsHighSubstrate);

                        }
                        else
                            ss = ToCatchCurveByThreeMainElements((int)FuncType.XRF, specList[i].Specs[0], temp, ref MatchElements, bIsHighSubstrate);
                        matchInfos += specList[i].Name + "(" + MatchElements + ")" + "---" + (ss == null ? "NoMatch" : ss.Name) + "(" + (ss == null ? "NULL" : ss.MainElements) + ")" + ",\r\n";
                    }

                    //System.Windows.Forms.MessageBox.Show(matchInfos);
                    using (FileStream fileStream = new FileStream(Application.StartupPath + @"\InfoMatch_Curve.txt", FileMode.Append))
                    {
                        using (StreamWriter sw = new StreamWriter(fileStream))
                        {
                            sw.WriteLine(matchInfos);
                        }
                    }
                }

                //  匹配功能2013-05-22-------------------------------------------------------------------------
            }
            this.SetAxisXRange(this.XrfChart, WorkCurveHelper.XRange[0], WorkCurveHelper.XRange[1]);
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE_HIDE, 0, 0);

        }

        /// <summary>
        /// 添加感兴趣元素，同时更新统计数据等。
        /// </summary>
        public void AddInterestedElemenetUpdateProcess()
        {
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null)// && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
            {
                this.elementName = string.Empty;
                this.XrfChart.BoundaryElement = elementName;
                this.XrfChart.IUseBoundary = false;
                this.XrfChart.IUseBase = false;
                this.refreshFillinof.ContructMeasureRefreshData(1, WorkCurveHelper.WorkCurveCurrent.ElementList);//构造刷新测量数据
                this.refreshFillinof.CreateContructStatis(WorkCurveHelper.WorkCurveCurrent.ElementList); //构造统计信息
                if (WorkCurveHelper.CurrentSpec != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                {
                    XrfChart.SetSpecData(WorkCurveHelper.WorkCurveCurrent, WorkCurveHelper.VirtualSpecList, WorkCurveHelper.CurrentSpec, false, DifferenceDevice.DefaultSpecColor.ToArgb(), WorkCurveHelper.Growstyle);
                }
            }
        }

        public virtual void DisappearBk()
        {
            if (WorkCurveHelper.WorkCurveCurrent == null)
            {
                Msg.Show(Info.strSeleWorkCurveCurrent);
                return;
            }

            if (WorkCurveHelper.WorkCurveCurrent.ElementList == null)
            {
                Msg.Show(Info.strIsSeleElement);
                return;
            }
            if (!WorkCurveHelper.WorkCurveCurrent.ElementList.IsRemoveBk)
            {
                //Msg.Show(Info.strIsRemoveBk);
                return;
            }
            if (this.specList == null)
            {
                Msg.Show(Info.strSpecListInfo);
                return;
            }
            if (this.specList != null && WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.IsRemoveBk)
            {
                SpecListEntity removeBk = WorkCurveHelper.DataAccess.Query(WorkCurveHelper.WorkCurveCurrent.ElementList.SpecListName);
                BackGroundHelper.BGDisappear(removeBk, this.specList);
                //OpenWorkSpec(this.specList);
            }
        }

        public virtual void CreateIntRegion()
        {
            if (this.XrfChart != null)
                this.XrfChart.InterestAreaEnable = true;
        }

        public virtual void CaculateIntRegion()
        {
            if (this.XrfChart != null)
            {
                SpecSplitInfo info = this.XrfChart.currentInterestArea;
                if (info == null)
                {
                    Msg.Show(Info.NoRegionSelect);
                }
                else if (this.spec != null)
                {
                    double totalArea = BackGroundHelper.CaculateArea(this.spec, info.X2 > info.X1 ? info.X1 : info.X2, info.X2 > info.X1 ? info.X2 : info.X1);
                    Msg.Show(Info.CurrentIntArea + " " + totalArea.ToString());
                }
            }
        }

        public virtual ElementList CaclIntensityReport(out string sampleName, out bool IsExplore)
        {
            int SetCurrent = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].TubCurrent;
            WorkCurveHelper.WorkCurveCurrent.SetCurrent(SetCurrent);
            sampleName = string.Empty;
            IsExplore = false;
            if (this.currentSelectMode == 1)
            {
                IsExplore = true;
                if (this.ElemsOfQuale == null || this.lineStype == null
                    || this.ElemsOfQuale.Length == 0 || this.lineStype.Length == 0)
                    AutoAnalysisProcess(null);//自动分析处理
                ElementList elementList = ElementList.New;
                //ElementList elementList = WorkCurveHelper.WorkCurveCurrent.ElementList;
                elementList.IsUnitary = true;
                elementList.UnitaryValue = 100;
                if (this.ElemsOfQuale == null || this.lineStype == null
                     || this.ElemsOfQuale.Length == 0 || this.lineStype.Length == 0)
                    return null;

                selePrintObjectL.Clear();
                int Elem_i = 0;
                foreach (string str in this.ElemsOfQuale)
                {
                    CurveElement CurveElement = CurveElement.New;
                    Atom atom = Atoms.AtomList.Find(w => w.AtomName == str);
                    CurveElement.Caption = atom.AtomName;
                    CurveElement.Formula = atom.AtomName;
                    CurveElement.AtomicNumber = atom.AtomID;
                    CurveElement.LayerNumber = 1;
                    CurveElement.LayerNumBackUp = "";
                    CurveElement.ContentUnit = ContentUnit.per;
                    CurveElement.Flag = ElementFlag.Calculated;
                    CurveElement.IntensityWay = IntensityWay.FPGauss;
                    CurveElement.SSpectrumData = "";
                    CurveElement.SInfluenceCoefficients = "";
                    // CurveElement.AnalyteLine = atom.DefaultLine;
                    CurveElement.AnalyteLine = (XLine)(this.lineStype[Elem_i]);
                    CurveElement.LayerFlag = LayerFlag.Fixed;
                    CurveElement.IsShowElement = true;
                    elementList.Items.Add(CurveElement);
                    Elem_i++;
                }
                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Clear();
                WorkCurveHelper.WorkCurveCurrent.ElementList = elementList;

                WorkCurveHelper.WorkCurveCurrent.CaculateIntensity(this.specList);
                sampleName = this.specList.Name;// .SampleName;
                return WorkCurveHelper.WorkCurveCurrent.ElementList;
            }
            else
            {
                if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null
                    && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0 && this.specList != null && this.specList.Specs != null && this.specList.Specs.Length > 0)
                {
                    WorkCurveHelper.WorkCurveCurrent.CaculateIntensity(this.specList);
                    sampleName = this.specList.Name;// .SampleName;
                    return WorkCurveHelper.WorkCurveCurrent.ElementList;
                }
            }

            return null;
        }

        public virtual string[] QualityResult(out int[] lines)
        {
            lines = new int[1];
            if (this.ElemsOfQuale != null && this.ElemsOfQuale.Length > 0 && this.lineStype != null
                && this.lineStype.Length > 0 && this.lineStype.Length == this.ElemsOfQuale.Length)
            {
                lines = this.lineStype;
                return this.ElemsOfQuale;
            }
            return null;
        }

        public virtual void DisplayLogData()
        {
            if (this.specList != null && this.specList.Specs != null && this.specList.Specs.Length == 1)
            {
                int[] data = Helper.ToInts(this.specList.Specs[0].SpecData);
                int[] newData = new int[data.Length];
                for (int i = 0; i < newData.Length; i++)
                {
                    if (data[i] >= 1)
                        newData[i] = Convert.ToInt32(Math.Log10(10));
                    else
                        newData[i] = 0;
                }
                int maxLog = 0;
                for (int i = 0; i < newData.Length; i++)
                    if (newData[i] > maxLog) maxLog = newData[i];
                if (maxLog == 0)
                    maxLog = 1;
                double logRate = (this.XrfChart.Height - 10) / maxLog;
                for (int j = 0; j < newData.Length; j++)
                {
                    int height = (int)Math.Round(newData[j] * logRate);
                    if (height > this.XrfChart.Height || height < 0)
                        height = this.XrfChart.Height;
                    newData[j] = this.XrfChart.Height - height;
                }
                this.specList.Specs[0].SpecData = Helper.ToStrs(newData);
                this.selectSpeclist.Clear();
                this.selectSpeclist.Add(this.specList);
                OpenWorkSpec(this.selectSpeclist, true);
            }
        }

        public virtual void PauseStart()
        {
            NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == "TestSetting");
            if (item != null)
            {
                item.BtnDropDown.Enabled = true;
                item.BtnDropDown.Image = Properties.Resources.Pause;
                item.TT = null;
                item.Text = Info.PauseStop;
                item.excuteRequire = ExcuteStartProcess;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        private void StateChange(bool flag)
        {
            if (MenuLoadHelper.MenuStripCollection.Count == 0)
                return;
            if (this.XrfChart != null)
                this.XrfChart.UnSpecing = flag;
            //菜单栏及工具栏等状态变化
            if (MenuLoadHelper.MenuStripCollection.Count > 0)
            {
                foreach (ToolStripControls toolstrip in MenuLoadHelper.MenuStripCollection)
                {
                    if (toolstrip.parentStripMeauItem != null && toolstrip.parentStripMeauItem.CurrentNaviItem.Name != "Spec" && toolstrip.CurrentNaviItem.Enabled
                        && toolstrip.parentStripMeauItem.CurrentNaviItem.Name != "Quality" && toolstrip.CurrentNaviItem.EnabledControl && toolstrip.parentStripMeauItem.CurrentNaviItem.Name != "Others")
                    //if (toolstrip.parentStripMeauItem != null && toolstrip.parentStripMeauItem.CurrentNaviItem.Name != "Spec" && toolstrip.CurrentNaviItem.Enabled
                    //    && toolstrip.parentStripMeauItem.CurrentNaviItem.Name != "Quality" && toolstrip.parentStripMeauItem.CurrentNaviItem.Name != "Others")
                    {
                        toolstrip.CurrentNaviItem.BtnDropDown.Enabled = flag;
                        //toolstrip.CurrentNaviItem.EnabledControl = flag;
                        toolstrip.CurrentNaviItem.MenuStripItem.Enabled = flag;
                        toolstrip.CurrentNaviItem.Btn.Visible = flag;
                        (toolstrip.CurrentNaviItem.Btn.Tag as Label).Visible = flag;
                    }
                    //if (toolstrip.parentStripMeauItem != null && toolstrip.CurrentNaviItem.Enabled && toolstrip.parentStripMeauItem.CurrentNaviItem.Name == "Spec"
                    //    && toolstrip.CurrentNaviItem.Name != "AddVirtualSpec" && toolstrip.preToolStripMeauItem != null)
                    if (toolstrip.parentStripMeauItem != null && toolstrip.CurrentNaviItem.Enabled && toolstrip.CurrentNaviItem.EnabledControl && toolstrip.parentStripMeauItem.CurrentNaviItem.Name == "Spec"
                        && toolstrip.CurrentNaviItem.Name != "AddVirtualSpec" && toolstrip.preToolStripMeauItem != null)
                    {
                        toolstrip.CurrentNaviItem.BtnDropDown.Enabled = flag;
                        //toolstrip.CurrentNaviItem.EnabledControl = flag;
                        toolstrip.CurrentNaviItem.MenuStripItem.Enabled = flag;
                        toolstrip.CurrentNaviItem.Btn.Visible = flag;
                        (toolstrip.CurrentNaviItem.Btn.Tag as Label).Visible = flag;
                    }
                }
            }
            if (WorkCurveHelper.NaviItems.Count == 0)
                return;
            //工具栏及导航栏设置
            foreach (NaviItem naviItem in WorkCurveHelper.NaviItems)
            {
                //if ((naviItem.FlagType == 2 || naviItem.FunctionType == 2) && naviItem.Enabled)
                if ((naviItem.FlagType == 2 || naviItem.FunctionType == 2) && naviItem.Enabled && naviItem.EnabledControl)
                {
                    naviItem.BtnDropDown.Enabled = flag;
                    //naviItem.EnabledControl = flag;
                    naviItem.MenuStripItem.Enabled = flag;
                    naviItem.Btn.Visible = flag;
                    (naviItem.Btn.Tag as Label).Visible = flag;
                    naviItem.ComboStrip.Enabled = flag;
                }
            }
            //if (libSerialize.containerObjTemp != null && libSerialize.clientPannel != null)
            //    libSerialize.GetNaviButtonsAll(libSerialize.containerObjTemp, 30, libSerialize.clientPannel, true);
        }

        /// <summary>
        /// 预热处理过程。
        /// </summary>
        /// <param name="heatParams"></param>
        public virtual void PreHeatProcess(PreHeatParams heatParams)
        {
            this.heatParams = heatParams;
            this.XrfChart.CurrentSpecPanel = 1;
            DeviceParameter deviceParams = DeviceParameter.New.Init("PreDeviceParams", heatParams.PreHeatTime, heatParams.TubCurrent, heatParams.TubVoltage,
               heatParams.FilterIdx, heatParams.CollimatorIdx, 1, false, 0, false, 0, false, 0, 0, 50, 2000, false, false, 0, 0, 0, 0, 1, TargetMode.OneTarget, heatParams.CurrentRate);
            this.deviceParamsList = new List<DeviceParameter>();
            this.deviceParamsList.Add(deviceParams);
            this.deviceParamSelectIndex = 0;
            this.deviceMeasure.interfacce.ExistMagnet = WorkCurveHelper.DeviceCurrent.HasElectromagnet;
            this.deviceMeasure.interfacce.heatParams = this.heatParams;
            this.deviceMeasure.interfacce.InitParam = InitParameter.New.Init(heatParams.TubVoltage, heatParams.TubCurrent,
                                                   heatParams.Gain, heatParams.FineGain, 0, 0, 1105, 0, "Ag", heatParams.FilterIdx, heatParams.CollimatorIdx, heatParams.Target, heatParams.TargetMode, heatParams.CurrentRate, "x", 1);
            progressInfo.MeasureTime = this.deviceParamsList[this.deviceParamSelectIndex].PrecTime + "s";
            progressInfo.SurplusTime = this.deviceParamsList[this.deviceParamSelectIndex].PrecTime + "s";
            progressInfo.Value = 0;
            this.deviceMeasure.interfacce.StopFlag = false;
            this.optMode = OptMode.PreHeat;
            TestStartAfterControlState(false);
            this.deviceMeasure.StartPreHeat();
        }

        private PreHeatParams heatParams;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public virtual void ReceiveOpenVoltageTime(int time)
        {
            if (time < 0 || this.deviceMeasure.interfacce.StopFlag)
                return;
            progressInfo.Maximum = this.heatParams.PreHeatTime;
            if (time >= progressInfo.Minimum && time <= this.progressInfo.Maximum)
            {
                progressInfo.Value = time;
            }
            int differenceUsedTime = this.heatParams.PreHeatTime - time;
            progressInfo.SurplusTime = differenceUsedTime + "s";
        }

        public void BlueTeethFreshResult(int usedTime)
        {
            if (this.testDevicePassedParams != null && this.testDevicePassedParams.Spec.SpecType == SpecType.UnKownSpec)
            {
                SpecListEntity specTemp = new SpecListEntity();
                //specTemp.Condition = WorkCurveHelper.WorkCurveCurrent.Condition;
                specTemp.SampleName = this.testDevicePassedParams.Spec.SampleName;
                this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
                this.specList.Specs[0] = this.spec;
                this.spec.UsedTime = usedTime;
                this.selectSpeclist.Add(specTemp);
                CaculateContent(this.selectSpeclist, 1, true);//蓝牙
            }
        }

        /// <summary>
        /// 获取配置文件中的设置的格式生成报表名称
        /// </summary>
        /// 
        /// <returns></returns>
        public string GetDefineReportName(SpecListEntity speclist, WorkCurve workcurve, long hisRecordId)
        {
            string str = string.Empty;
            string splitStr = string.Empty;

            string reportName = string.Empty;
            try
            {
                HistoryRecord hisRecord = HistoryRecord.FindById(hisRecordId);
                Dictionary<string, string> dicHistoryInfos = new Dictionary<string, string>().AddRangs(GetHistoryRecordValuePair(hisRecord), GetCompanyOtherInfosPair(hisRecord));
                //UnionDictionaryStringString(GetHistoryRecordValuePair(hisRecord), GetCompanyOtherInfosPair(hisRecord));
                string otherpath = Application.StartupPath + "\\AppParams.xml";
                XElement xele = XElement.Load(otherpath);
                if (xele == null) return str;
                var names = xele.Elements("NameSetting").Elements("Name").ToList();
                for (int i = 0; i < names.Count; i++)
                {
                    if (names[i].Attribute(XName.Get("Flag")).Value.ToString() == "ReportName")
                    {
                        string[] judge = names[i].Value.ToString().Split(("_").ToCharArray());
                        if (judge != null && judge.Count() > 0)
                        {
                            for (int j = 0; j < judge.Count(); j++)
                            {
                                switch (judge[j])
                                {
                                    case "bSpecListName":
                                        reportName += speclist == null ? "" : (speclist.Name + "_");
                                        break;
                                    case "bSpecType":
                                        reportName += (/*str*/speclist.SpecType.ToString() + "_");
                                        break;
                                    case "bInitConditionName":
                                        reportName += (/*DifferenceDevice.interClassMain.initParams.Condition.Name*/workcurve.Condition.InitParam.Condition.Name + "_");
                                        break;
                                    case "bCurrentWorkCurve":
                                        reportName += (workcurve.Name + "_");
                                        break;
                                    case "bOptMode":
                                        reportName += (/*splitStr*/workcurve.Condition.Type.ToString() + "_");
                                        break;
                                    case "bTestTime":
                                        //reportName += (System.DateTime.Now.ToString("HHmmss") + "_");
                                        reportName += speclist == null ? "_" : (DateTime.Parse(speclist.SpecDate.ToString()).ToString("HHmmss") + "_");
                                        break;
                                    case "bTestDate":
                                        //reportName += (System.DateTime.Now.ToString("yyyyMMdd") + "_");
                                        reportName += speclist == null ? "_" : (DateTime.Parse(speclist.SpecDate.ToString()).ToString("yyyyMMdd") + "_");
                                        break;
                                    case "bWorks":
                                        reportName += (workcurve.WorkRegion == null ? "" : (workcurve.WorkRegion.Name.Replace(",", "") + "_"));
                                        break;
                                    case "bCollimator":
                                        reportName += (workcurve.Condition == null
                                                        || workcurve.Condition.DeviceParamList == null
                                                        || workcurve.Condition.DeviceParamList.Count <= 0
                                                        || !WorkCurveHelper.DeviceCurrent.HasCollimator
                                                        || WorkCurveHelper.DeviceCurrent.Collimators.Count < workcurve.Condition.DeviceParamList[0].CollimatorIdx ? "" : WorkCurveHelper.DeviceCurrent.Collimators[workcurve.Condition.DeviceParamList[0].CollimatorIdx - 1].Diameter.ToString() + "mm_");
                                        break;
                                    default:
                                        if (dicHistoryInfos.Keys.Contains(judge[j]) && !string.IsNullOrEmpty(dicHistoryInfos[judge[j]]) /*&& !reportName.Contains("_"+dicHistoryInfos[judge[j]]+"_")*/)
                                            reportName += dicHistoryInfos[judge[j]] + "_";
                                        break;
                                }
                            }
                        }
                        else
                        {
                            return str;
                        }
                    }
                }
                if (reportName != "_" && reportName != "")
                {
                    reportName = reportName.Replace("/", Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("∕")));
                    reportName = reportName.Replace("\\", Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("﹨")));
                    reportName = reportName.Replace("<", Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("〈")));
                    reportName = reportName.Replace(">", Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("＞")));
                    reportName = reportName.Replace("?", Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("﹖")));
                    reportName = reportName.Replace(":", Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("∶")));
                    reportName = reportName.Replace("|", Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("︳")));
                    reportName = reportName.Replace("*", Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("﹡")));
                    reportName = reportName.Replace("\"", Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("“")));
                    var sArray = reportName.Split(" /\\<>?:|*\"[]".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    StringBuilder builder = new StringBuilder();
                    sArray.All(w => { builder.Append(w); return true; });
                    reportName = builder.ToString().PadRight(110, ' ');
                    reportName = reportName.Substring(0, reportName.Substring(0, 110).LastIndexOf('_') + 1).TrimEnd('_');//防止出现索引-1
                    if (System.Diagnostics.Debugger.IsAttached) reportName = ReportTemplateHelper.ExcelModeType.ToString() + "#" + reportName;
                    //reportName = GetFileSuffix(reportName);

                }
                else
                {
                    reportName = DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                return reportName;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                reportName = DateTime.Now.ToString("yyyyMMddHHmmss");
                return reportName;
            }
        }

        private static string GetFileSuffix(string reportName)
        {
            if (!Directory.Exists(WorkCurveHelper.ExcelPath))
                Directory.CreateDirectory(WorkCurveHelper.ExcelPath);
            DirectoryInfo dir = new DirectoryInfo(WorkCurveHelper.ExcelPath);

            long files = 0;//dir.GetFiles(reportName + ".xls").Count() + dir.GetFiles(reportName + "_Retry.xls").Count();
            string suffix = string.Empty;
        Suffix://查找数字后缀
            suffix = files <= 0 ? "" : "_" + files.ToString();
            if (dir.GetFiles(reportName + suffix + ".xls").Count() + dir.GetFiles(reportName + suffix + "_Retry.xls").Count() > 0)
            {
                if (files == long.MaxValue) goto TrimLast_;
                files++;
                goto Suffix;
            }
        TrimLast_://去除超出长度的最后部分名称项
            if ((reportName + suffix + "_Retry.xls").Length > 128)
            {
                reportName = reportName.Substring(0, reportName.LastIndexOf('_') + 1).TrimEnd('_');//防止出现-1
                goto TrimLast_;
            }
            reportName += suffix;
            if (string.IsNullOrEmpty(reportName))
                reportName = DateTime.Now.ToString("yyyyMMddHHmmss");
            return reportName;
        }

        public Dictionary<string, string> GetHistoryRecordValuePair(HistoryRecord hisRecord)
        {
            Dictionary<string, string> dicBack = new Dictionary<string, string>();
            object obj = hisRecord;
            if (obj == null) return dicBack;
            Type objType = hisRecord.GetType();
            PropertyInfo[] pros = objType.GetProperties();
            object value = null;
            foreach (var pro in pros)
            {
                value = pro.GetValue(obj, null);
                dicBack.Add(pro.Name, value == null ? "" : value.ToString());
            }
            return dicBack;
        }

        public Dictionary<string, string> GetCompanyOtherInfosPair(HistoryRecord hisRecord)
        {
            Dictionary<string, string> dicBack = new Dictionary<string, string>();
            //XmlNodeList xmlNodeList = ExcelTemplateParams.GetCompanyOtherInfoTitles();
            //HistoryCompanyOtherInfo value = null;
            //foreach (XmlNode l in xmlNodeList)
            //{
            //    foreach (XmlNode n in l.ChildNodes)
            //    {
            //        string sd = n.Attributes[WorkCurveHelper.LanguageShortName].Value;
            //        string sTarget = n.Attributes["Target"].Value;
            //        string strsql = "select * from historycompanyotherinfo a " +
            //                                                       " left outer join companyothersinfo b on b.id=a.companyothersinfo_id " +
            //                                                       " where a.workcurveid='" + hisRecord.WorkCurveId + "' and a.history_id='" + hisRecord.Id + "' and b.name='" + sd + "' and b.isreport=1";
            //        List<HistoryCompanyOtherInfo> hisCompanyOtherInfoList = HistoryCompanyOtherInfo.FindBySql(strsql);
            //        value = hisCompanyOtherInfoList.FirstOrDefault();
            //        dicBack.Add(sTarget, value == null ? "" : value.ListInfo);
            //    }
            //}
            if (hisRecord == null) return dicBack;
            List<HistoryCompanyOtherInfo> lstHisComOthInfo = HistoryCompanyOtherInfo.Find(w => w.WorkCurveId == hisRecord.WorkCurveId && w.HistoryRecord.Id == hisRecord.Id);
            lstHisComOthInfo.ForEach(w =>
            {
                dicBack.Add(w.CompanyOthersInfo.Name, w.ListInfo);
            });
            return dicBack;
        }
        /// <summary>
        /// 获取配置文件中的设置的格式生成报表名称
        /// </summary>
        /// <returns></returns>
        public string GetDefineReportName()
        {
            return GetDefineReportName(this.specList, WorkCurveHelper.WorkCurveCurrent, this.recordList.LastOrDefault());
        }

        public string GenerateGenericReport(ElementList elements, List<long> selectLong)
        {
            ExcelTemplateParams.GetExcelTemplateParams();
            string sFileTemplateName = AppDomain.CurrentDomain.BaseDirectory + "/HistoryExcelTemplate/" +
                    (selectLong.Count > 1 ? ExcelTemplateParams.ManyTimeTemplate : ExcelTemplateParams.OneTimeTemplate);
            return GenerateGenericReport(elements, selectLong, sFileTemplateName);
        }

        public string GenerateGenericReport(ElementList elements, List<long> selectLong, string fileTemplateName)
        {
            string sReturn = string.Empty;
            if (this.XrfChart != null)
            {

                ReportTemplateHelper.ReoprtxAxisScale = (int)this.XrfChart.MasterPane.PaneList[0].XAxis.Scale.Min;
                ReportTemplateHelper.ReportXAxisScaleMax = (int)this.XrfChart.MasterPane.PaneList[0].XAxis.Scale.Max;
                ReportTemplateHelper.ReportXStep = (int)this.XrfChart.MasterPane.PaneList[0].XAxis.Scale.MajorStep;
                ReportTemplateHelper.IsSpecShowElem = this.XrfChart.BShowElement;
                if (WorkCurveHelper.ReportSaveScreen)
                    DifferenceDevice.interClassMain.SaveSreenShot(true, specList.Name);

            }
            if (ReportTemplateHelper.IsCurrentUsed && ReportTemplateHelper.ExcelModeType != 0 && ReportTemplateHelper.ExcelModeType != 2 && ReportTemplateHelper.ExcelModeType != 7)
            {
                Report report = new Report();
                report.Elements = elements;//FilterElementsByDGV(dgvHistoryRecord, selectLong.FirstOrDefault());
                report.InterestElemCount = report.Elements.Items.ToList().FindAll(w => w.IsShowElement).Count;
                report.historyRecordid = selectLong.FirstOrDefault().ToString();
                var reportName = GetDefineReportName(this.specList, WorkCurveHelper.WorkCurveCurrent, selectLong.FirstOrDefault());//GetDefineReportName();
                report.ListStatisticsMethods.Add(SUM);
                string sFileTemplateName = fileTemplateName;
                report.specList = specList;
                report.Spec = spec;
                //sReturn = report.GenerateGenericReport(sFileTemplateName, reportName, selectLong);
                report.TempletFileName = sFileTemplateName;
                sReturn = report.GenerateRetestReport(reportName, true, selectLong);
            }
            return sReturn;
        }

        private double SUM(object obj)
        {
            double d = 0;
            List<double> lst = obj as List<double>;
            d = lst == null ? 0 : lst.Sum();
            return d;
        }

        public virtual void SaveExcel(List<TreeNodeInfo> list, double dWeight)
        {
            if (WorkCurveHelper.WorkCurveCurrent == null || WorkCurveHelper.WorkCurveCurrent.ElementList == null) return;
            string valid = GenerateGenericReport(WorkCurveHelper.WorkCurveCurrent.ElementList, recordList);//采用当前感兴趣元素
            if (!string.IsNullOrEmpty(valid))
            {
                OpenPathThread(valid);
                return;
            }
            if (ReportTemplateHelper.ExcelModeType == 0)
            {
                if (EDXRFHelper.LoadLoadSourceEvent())
                    EDXRFHelper.DirectPrintHelper();
                else
                {
                    Msg.Show(Info.NoLoadSource);
                }
            }
            else if (ReportTemplateHelper.ExcelModeType == 2)
            {

                if (InterfaceClass.SetPrintTemplate(null, null))
                    EDXRFHelper.NewPrintDirectPrintHelper(InterfaceClass.seledataFountain);
                else
                {
                    Msg.Show(Info.NoLoadSource);
                }
            }
            else if (ReportTemplateHelper.ExcelModeType == 6 && DifferenceDevice.IsAnalyser)
            {
                if (DifferenceDevice.interClassMain.reportThreadManage == null) return;

                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string SaveReportPath = DifferenceDevice.interClassMain.reportThreadManage.GetHistoryRecordReport(hisRecordidList, dWeight, true, false);
                if (SaveReportPath == "") return;

                OpenPathThread(SaveReportPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 7)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string excelPath = BrassReport(hisRecordidList);
                if (excelPath == "") return;
                OpenPathThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 8)
            {
                List<long> hisRecordidList = new List<long>();
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);
                if (hisRecordidList.Count == 0) return;
                string excelPath = GetXRFRecordReport(hisRecordidList);
                if (excelPath == "") return;
                OpenPathThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 9 || ReportTemplateHelper.ExcelModeType == 21 || ReportTemplateHelper.ExcelModeType == 24)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetHistoryRecordReport(false, hisRecordidList);
                if (excelPath.Length == 0) return;
                OpenPathThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 26)
            {
                List<long> hisRecordidList = new List<long>();
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetGSHistoryRecord(hisRecordidList);
                if (excelPath == string.Empty || excelPath.Length == 0) return;
                OpenPathThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 30 || ReportTemplateHelper.ExcelModeType == 31)
            {

                var hrList = new List<long>();
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hrList.Add(hisId);
                if (hrList.Count == 0) return;
                GetBengalHistoryRecord(hrList, false);
            }
            else if (ReportTemplateHelper.ExcelModeType == 11)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);
                if (hisRecordidList.Count == 0) return;
                string excelPath = GetHistoryRecordEleven(hisRecordidList);
                if (excelPath == string.Empty || excelPath.Length == 0) return;
                OpenPathThread(excelPath);
                //GetHistoryRecordReport(hisRecordidList, false);//反射实现
            }
            else if (ReportTemplateHelper.ExcelModeType == 12)
            {
                List<long> hisRecordidList = new List<long>();


                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetThickHistoryRecordReport(hisRecordidList);
                if (excelPath.Length == 0) return;
                OpenPathThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 19)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetThickHistoryRecordReport19(hisRecordidList);
                if (excelPath.Length == 0) return;
                OpenPathThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 13 || ReportTemplateHelper.ExcelModeType == 14)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetHistoryRecordTailorReport(hisRecordidList);
                if (excelPath.Length == 0) return;
                OpenPathThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 15 && DifferenceDevice.IsAnalyser)
            {
                if (DifferenceDevice.interClassMain.reportThreadManage == null) return;

                List<long> hisRecordidList = new List<long>();
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string SaveReportPath = DifferenceDevice.interClassMain.reportThreadManage.GetHistoryRecordReport(hisRecordidList, dWeight, true, false);
                if (SaveReportPath == "") return;

                OpenPathThread(SaveReportPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 16)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetHistoryRecordReport(false, hisRecordidList);
                if (excelPath.Length == 0) return;
                OpenPathThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 20)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetHistoryRecordReport(false, hisRecordidList);
                if (excelPath.Length == 0) return;
                OpenPathThread(excelPath);
            }
            else
            {
                this.refreshFillinof.SaveExcel();
            }
        }

        private string GetXRFRecordReport(List<long> selectLong)
        {
            #region
            try
            {
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return string.Empty;
                }
                else
                {
                    #region 初始化表格
                    string workCurveName = string.Empty;
                    string WorkCurveID = string.Empty;
                    string historyRecordid = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    SpecListEntity tempList = new SpecListEntity();
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;

                        WorkCurveID = workCurve.Id.ToString();
                        workCurveName = workCurve.Name;
                        historyRecordid = selectLong[i].ToString();
                        report.ReadingNo = record.HistoryRecordCode;
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName, record.FilePath, record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                                spec = tempList.Specs[0];
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return "";
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return (curveElement.Caption == element.elementName || curveElement.Formula == element.elementName) && curveElement.ElementList != null && curveElement.ElementList.WorkCurve != null && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            if (temp != null && !temp.IsShowElement) continue;
                            if (temp == null)//感兴趣元素外的历史记录元素
                            {
                                temp = CurveElement.New;
                                temp.Caption = element.elementName;
                                temp.IsShowElement = true;
                                temp.ContentUnit = (ContentUnit)element.unitValue;
                                temp.RowsIndex = -1;
                                //temp.Intensity = element.CaculateIntensity;
                                //temp.Error = element.Error;
                                //if (element.unitValue == 2)
                                //    temp.Error = temp.Error / 10000;
                                //else
                                //    temp.Error = temp.Error / 10;
                                //double elecontent = 0.0;
                                //double.TryParse(element.contextelementValue, out elecontent);
                                //if (element.unitValue == 1)
                                //    temp.Content = elecontent;
                                //else if (element.unitValue == 2)
                                //    temp.Content = elecontent / 10000;
                                //else
                                //    temp.Content = elecontent / 10;
                                //elementList.Items.Add(temp);
                            }
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            if (element.unitValue == 2)
                                temp.Error = temp.Error / 10000;
                            else
                                temp.Error = temp.Error / 10;

                            if (element.unitValue == 1)
                                temp.Content = content;
                            else if (element.unitValue == 2)
                                temp.Content = content / 10000;
                            else
                                temp.Content = content / 10;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    #endregion

                    #region
                    if (elementList.Items.Count == 0)
                    {
                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        return "";
                    }
                    #endregion
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.Elements = elementList;
                    report.WorkCurveName = workCurveName;
                    report.WorkCurveID = WorkCurveID;
                    report.historyRecordid = historyRecordid;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    string reportName = GetDefineReportName();
                    //
                    string StrSavePath = string.Empty;
                    if (selectLong.Count == 1)
                    {
                        report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                        report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        List<HistoryRecord> records = HistoryRecord.FindBySql("select * from HistoryRecord where Id in (" + report.historyRecordid + ")");
                        //report.Specification = records.Count > 0 ? records[0].Specifications : "";
                        report.Specification = records.Count > 0 ? (System.Text.RegularExpressions.Regex.Replace((records[0].Specifications != null ? records[0].Specifications : ""), "%.?%|%.+%", "%")) : "";
                        StrSavePath = report.GenerateReport(reportName, true);
                        //
                    }
                    else
                    {
                        DataTable dt = CreateReTestTable(selectLong);
                        int cont = 0;
                        for (int j = 0; j < selectLong.Count; j++)
                        {
                            DataRow rowNew = dt.NewRow();
                            rowNew["Time"] = ++cont;
                            foreach (DataColumn column in dt.Columns)
                            {
                                HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.Caption && w.HistoryRecord.Id == selectLong[j]);
                                if (element != null)
                                {
                                    string valueStr = element.contextelementValue;
                                    if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                                    {
                                        double Value = double.Parse(valueStr);
                                        if (element.unitValue == 1)
                                            Value = Value * 10000;
                                        else if (element.unitValue == 3)
                                            Value = Value * 1000;

                                        if (element.unitValue == 1)
                                        {
                                            rowNew[column.Caption] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                                        }
                                        else if (element.unitValue == 2)
                                        {
                                            rowNew[column.Caption] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                                        }
                                        else
                                        {
                                            rowNew[column.Caption] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                                        }
                                    }
                                }
                            }
                            dt.Rows.Add(rowNew);
                        }

                        bool flag = false;
                        bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);

                        if (flag)
                            AddStaticsRows(dt, "time", null);

                        report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                        if (File.Exists(Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate.TrimEnd(".xls".ToCharArray()) + "_Horizontal.xls"))
                            ExcelTemplateParams.ManyTimeTemplate = ExcelTemplateParams.ManyTimeTemplate.TrimEnd(".xls".ToCharArray()) + "_Horizontal.xls";
                        report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        StrSavePath = report.GenerateRetestReport(reportName, dt, true, false);
                        //
                    }
                    if (!File.Exists(StrSavePath))
                        return "";
                    else
                        return StrSavePath;
                }
            }
            catch
            {
                return "";
            }
            #endregion
        }

        private void GetHistoryRecordReport(List<long> selectLong, bool print)
        {
            UCHistoryRecord uc = new UCHistoryRecord();//贵金属无历史记录界面 否则可以直接调用现有历史记录控件
            MethodInfo load = uc.GetType().GetMethod("UCHistoryRecord_Load", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            load.Invoke(uc, new object[] { null, null });
            FieldInfo field = uc.GetType().GetField("selectLong", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            selectLong.Reverse();//按最近顺序排列 
            field.SetValue(uc, selectLong);
            MethodInfo method = uc.GetType().GetMethod("btwTemplateExcel_Click", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (print)
                uc.OnPrintTemplate += new UCHistoryRecord.Print(PrintThread);//(PrintExcelByPath);
            else uc.OnPrintTemplate += new UCHistoryRecord.Print(OpenPathThread);
            method.Invoke(uc, new object[] { null, null });
        }
        private string GetThickHistoryRecordReport19(List<long> selectLong)
        {
            #region
            try
            {
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return string.Empty;
                }
                else
                {
                    #region 初始化表格
                    string workCurveName = string.Empty;
                    string WorkCurveID = string.Empty;
                    string historyRecordid = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    SpecListEntity tempList = new SpecListEntity();
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;

                        WorkCurveID = workCurve.Id.ToString();
                        workCurveName = workCurve.Name;
                        historyRecordid = selectLong[i].ToString();
                        report.ReadingNo = record.HistoryRecordCode;
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName, record.FilePath, record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                                spec = tempList.Specs[0];
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return "";
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList != null && curveElement.ElementList.WorkCurve != null && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            if (temp == null)//感兴趣元素外的历史记录元素
                            {
                                temp = CurveElement.New;
                                temp.Intensity = element.CaculateIntensity;
                                temp.Error = element.Error;
                                if (element.unitValue == 2)
                                    temp.Error = temp.Error / 10000;
                                else
                                    temp.Error = temp.Error / 10;
                                double elecontent = 0.0;
                                double.TryParse(element.contextelementValue, out elecontent);
                                if (element.unitValue == 1)
                                    temp.Content = elecontent;
                                else if (element.unitValue == 2)
                                    temp.Content = elecontent / 10000;
                                else
                                    temp.Content = elecontent / 10;
                                elementList.Items.Add(temp);
                            }
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            if (element.unitValue == 2)
                                temp.Error = temp.Error / 10000;
                            else
                                temp.Error = temp.Error / 10;

                            if (element.unitValue == 1)
                                temp.Content = content;
                            else if (element.unitValue == 2)
                                temp.Content = content / 10000;
                            else
                                temp.Content = content / 10;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    #endregion

                    #region
                    if (elementList.Items.Count == 0)
                    {
                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        return "";
                    }
                    #endregion
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.Elements = elementList;
                    report.WorkCurveName = workCurveName;
                    report.WorkCurveID = WorkCurveID;
                    report.historyRecordid = historyRecordid;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    string reportName = GetDefineReportName();

                    DataTable dt = this.GetReportDT19(report, elementList, selectLong);//获取打印数据
                    bool flag = false;
                    bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
                    if (flag && selectLong.Count >= 2)
                        // AddStaticsRows(dt, Info.SerialNumber);
                        AddThickStaticsRows(dt, Info.SerialNumber);                //yuzhaomodify:Thick专用模板
                    report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                    report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                    report.ReportPath = WorkCurveHelper.ExcelPath;
                    //report.GenerateRetestReport(reportName, dt, true);
                    //report.GenerateThickRetestReport(reportName, dt, true);   //yuzhaomodify:Thick专用模板
                    string strFileName = report.GenerateRetestReport(reportName, dt, true, false);
                    //}
                    return strFileName;
                }
            }
            catch
            {
                return "";
            }
            #endregion
        }

        private string GetThickHistoryRecordReport(List<long> selectLong)
        {
            #region
            try
            {
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return string.Empty;
                }
                else
                {
                    #region 初始化表格
                    string workCurveName = string.Empty;
                    string WorkCurveID = string.Empty;
                    string historyRecordid = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    SpecListEntity tempList = new SpecListEntity();
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;

                        WorkCurveID = workCurve.Id.ToString();
                        workCurveName = workCurve.Name;
                        historyRecordid = selectLong[i].ToString();
                        report.ReadingNo = record.HistoryRecordCode;
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName, record.FilePath, record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                                spec = tempList.Specs[0];
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return "";
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList != null && curveElement.ElementList.WorkCurve != null && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            if (temp == null)//感兴趣元素外的历史记录元素
                            {
                                temp = CurveElement.New;
                                temp.Intensity = element.CaculateIntensity;
                                temp.Error = element.Error;
                                if (element.unitValue == 2)
                                    temp.Error = temp.Error / 10000;
                                else
                                    temp.Error = temp.Error / 10;
                                double elecontent = 0.0;
                                double.TryParse(element.contextelementValue, out elecontent);
                                if (element.unitValue == 1)
                                    temp.Content = elecontent;
                                else if (element.unitValue == 2)
                                    temp.Content = elecontent / 10000;
                                else
                                    temp.Content = elecontent / 10;
                                elementList.Items.Add(temp);
                            }
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            if (element.unitValue == 2)
                                temp.Error = temp.Error / 10000;
                            else
                                temp.Error = temp.Error / 10;

                            if (element.unitValue == 1)
                                temp.Content = content;
                            else if (element.unitValue == 2)
                                temp.Content = content / 10000;
                            else
                                temp.Content = content / 10;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    #endregion

                    #region
                    if (elementList.Items.Count == 0)
                    {
                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        return "";
                    }
                    #endregion
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.Elements = elementList;
                    report.WorkCurveName = workCurveName;
                    report.WorkCurveID = WorkCurveID;
                    report.historyRecordid = historyRecordid;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    string reportName = GetDefineReportName();

                    DataTable dt = this.GetReportDT12(report, elementList, selectLong);//获取打印数据
                    bool flag = false;
                    bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
                    if (flag && selectLong.Count >= 2)
                        // AddStaticsRows(dt, Info.SerialNumber);
                        AddThickStaticsRows(dt, Info.SerialNumber);                //yuzhaomodify:Thick专用模板
                    report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                    report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                    report.ReportPath = WorkCurveHelper.ExcelPath;


                    string tempStr = "";
                    int loc = 0;



                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].Caption.Contains("厚度") || dt.Columns[i].Caption.Contains("面密度"))
                        {

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {

                                tempStr = dt.Rows[j][i].ToString();
                                loc = tempStr.IndexOf('(');


                                if (loc > 0)
                                {
                                    dt.Rows[j][i] = tempStr.Substring(0, loc).Trim();

                                }

                            }


                            if (dt.Columns[i].Caption.Contains("厚度"))
                            {
                                for (int index = 0; index < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; index++)
                                {
                                    if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].IsShowElement)
                                    {
                                        if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.ur)
                                        {
                                            dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + "u''" + ")";
                                        }
                                        else if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.um)
                                        {
                                            dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + "um" + ")";

                                        }
                                        else
                                        {
                                            dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + "g/L" + ")";

                                        }

                                        break;
                                    }
                                }


                            }
                            else
                            {
                                if (WorkCurveHelper.WorkCurveCurrent.AreaThickType == "g/m^2")
                                    dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + "g/m^2" + ")";
                                else
                                    dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + "mg/cm^2" + ")";
                            }
                        }


                        if (dt.Columns[i].Caption.Contains("样品名称"))
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {

                                if (dt.Rows[j][i].ToString().Contains("#"))
                                {
                                    dt.Rows[j][i] = dt.Rows[j][i].ToString().Split(new char[] { '#' })[0];

                                }
                            }

                        }
                    }



                    string strFileName = report.GenerateRetestReport(reportName, dt, true, false);
                    return strFileName;
                }
            }
            catch
            {
                return "";
            }
            #endregion
        }

        /// <summary>
        /// 9 贵金属Excel模板
        /// </summary>
        /// <param name="selectLong"></param>
        /// <returns></returns>
        private string GetHistoryRecordReport(bool IsPrint, List<long> selectLong)
        {
            #region
            try
            {
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return string.Empty;
                }
                else
                {
                    #region 初始化表格
                    string StrSaveReportPath = string.Empty;
                    string workCurveName = string.Empty;
                    string WorkCurveID = string.Empty;
                    string historyRecordid = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    SpecListEntity tempList = new SpecListEntity();
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;
                        elementList.RhIsLayer = workCurve.ElementList != null ? workCurve.ElementList.RhIsLayer : false;
                        elementList.LayerElemsInAnalyzer = workCurve.ElementList != null ? workCurve.ElementList.LayerElemsInAnalyzer : "";
                        WorkCurveID = workCurve.Id.ToString();
                        workCurveName = workCurve.Name;
                        historyRecordid = selectLong[i].ToString();
                        report.ReadingNo = record.HistoryRecordCode;
                        report.dWeight = record.Weight;
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName, record.FilePath, record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                                spec = tempList.Specs[0];
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return StrSaveReportPath;
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList != null && curveElement.ElementList.WorkCurve != null && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            if (temp != null && !temp.IsShowElement) continue;
                            if (temp == null)//感兴趣元素外的历史记录元素
                            {
                                temp = CurveElement.New;
                                temp.Caption = element.elementName;
                                temp.Intensity = element.CaculateIntensity;
                                temp.Error = element.Error;
                                if (element.unitValue == 2)
                                    temp.Error = temp.Error / 10000;
                                else
                                    temp.Error = temp.Error / 10;
                                double elecontent = 0.0;
                                double.TryParse(element.contextelementValue, out elecontent);
                                if (element.unitValue == 1)
                                    temp.Content = elecontent;
                                else if (element.unitValue == 2)
                                    temp.Content = elecontent / 10000;
                                else
                                    temp.Content = elecontent / 10;
                                double.TryParse(element.thickelementValue, out elecontent);
                                temp.Thickness = elecontent;
                                temp.IsShowElement = true;
                                temp.ContentUnit = (ContentUnit)element.unitValue;
                                elementList.Items.Add(temp);
                                report.historyStandID = element.customstandard_Id;
                                continue;
                            }
                            if (elementList.Items.ToList().FindIndex(w => w.Formula == element.elementName) >= 0) continue;
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            if (element.unitValue == 2)
                                temp.Error = temp.Error / 10000;
                            else
                                temp.Error = temp.Error / 10;

                            if (element.unitValue == 1)
                                temp.Content = content;
                            else if (element.unitValue == 2)
                                temp.Content = content / 10000;
                            else
                                temp.Content = content / 10;
                            double.TryParse(element.thickelementValue, out content);
                            temp.Thickness = content;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    #endregion

                    #region
                    if (elementList.Items.Count == 0)
                    {
                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        return StrSaveReportPath;
                    }
                    #endregion
                    report.ReportFileType = IsPrint ? 0 : report.ReportFileType;//直接打印时用excel
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.Elements = elementList;
                    report.WorkCurveName = workCurveName;
                    report.WorkCurveID = WorkCurveID;
                    report.historyRecordid = historyRecordid;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    report.IsAnalyser = (ReportTemplateHelper.ExcelModeType == 21 || ReportTemplateHelper.ExcelModeType == 9 || ReportTemplateHelper.ExcelModeType == 6 || ReportTemplateHelper.ExcelModeType == 15) ? true : false;
                    string reportName = GetDefineReportName();
                    //

                    if (selectLong.Count == 1)
                    {
                        report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                        report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        StrSaveReportPath = report.GenerateReport(reportName, true);
                        //
                    }
                    else
                    {
                        DataTable dt;
                        //if (WorkCurveHelper.IsPdRh)
                        //    dt = CreateReTestTable(selectLong, new string[] { "Pd", "Rh" });
                        //else
                        dt = CreateReTestTable(selectLong);
                        if (DifferenceDevice.IsShowKarat && dt.Columns.Contains("Au")) dt.Columns.Add("Karat");//新增K值列
                        int cont = 0;
                        for (int j = 0; j < selectLong.Count; j++)
                        {
                            DataRow rowNew = dt.NewRow();
                            rowNew["Time"] = ++cont;
                            double dKValue = 0;
                            foreach (DataColumn column in dt.Columns)
                            {
                                HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.Caption && w.HistoryRecord.Id == selectLong[j]);
                                if (element != null)
                                {
                                    report.elementListPDF.Add(element);
                                    string valueStr = element.contextelementValue;
                                    if (!string.IsNullOrEmpty(valueStr) && !valueStr.Equals("-"))//如果为空将导致dt为空
                                    {
                                        double Value = double.Parse(valueStr);
                                        //if (element.unitValue == 1)
                                        //    Value = Value * 10000;
                                        //else if (element.unitValue == 3)
                                        //    Value = Value * 1000;

                                        //if (element.unitValue == 1)
                                        //{
                                        //    rowNew[column.Caption] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                                        //}
                                        //else if (element.unitValue == 2)
                                        //{
                                        //    rowNew[column.Caption] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                                        //}
                                        //else
                                        //{
                                        //    rowNew[column.Caption] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                                        //}
                                        rowNew[column.Caption] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + (element.unitValue == 1 ? "(%)" : (element.unitValue == 2 ? "(ppm)" : "(‰)"));
                                        if (element.unitValue == 2)
                                            Value = Value / 10000;
                                        else if (element.unitValue == 3)
                                            Value = Value / 10;
                                        if (element.elementName.Equals("Au")) dKValue = Value * 24 / WorkCurveHelper.KaratTranslater;
                                    }

                                }
                            }
                            if (dt.Columns.Contains("Karat")) rowNew["Karat"] = dKValue.ToString("F3");
                            dt.Rows.Add(rowNew);
                        }

                        bool flag = false;
                        bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
                        string strStaticContent = ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStaContent");
                        bool bFullName = false;
                        try
                        {
                            bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestFullElemName"), out bFullName);
                        }
                        catch
                        {
                            bFullName = false;
                        }
                        bool bNeedTimesResult = true;
                        try
                        {
                            bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestNeedMulResults"), out bNeedTimesResult);
                        }
                        catch
                        {
                            bNeedTimesResult = true;
                        }
                        List<string> statics = (strStaticContent == null || strStaticContent.Trim() == string.Empty) ? null : strStaticContent.Split(',').ToList();
                        if (flag)
                            AddStaticsRows(dt, "time", statics);
                        if (bFullName)
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {

                                Atom atom = Atoms.AtomList.Find(s => s.AtomName == dc.ColumnName);
                                if (atom == null) continue;
                                string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                                string atomNameEN = (atom == null) ? "" : atom.AtomNameEN;
                                dc.Caption = Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN : atomNameEN;
                            }
                        }
                        if (ReportTemplateHelper.ExcelModeType == 21)
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {
                                Atom atom = Atoms.AtomList.Find(s => s.AtomName == dc.ColumnName);
                                if (atom == null) continue;
                                string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                                dc.Caption = Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN + "(" + dc.ColumnName + ")" : dc.ColumnName;
                            }
                        }
                        report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                        report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        report.NeedMultiResults = bNeedTimesResult;
                        report.selectLong = selectLong;
                        StrSaveReportPath = report.GenerateRetestReport(reportName, dt, true, true);
                        //
                    }
                    if (!File.Exists(StrSaveReportPath))
                        return string.Empty;
                    else
                        return StrSaveReportPath;
                }
            }
            catch
            {
                return "";
            }
            #endregion
        }

        /// <summary>
        /// 26号模板 冈山精工
        /// </summary>
        /// <param name="IsPrint"></param>
        /// <param name="selectLong"></param>
        /// <returns></returns>
        private string GetGSHistoryRecord(List<long> selectLong)
        {
            //ExcelTemplateReportBase eleven = new ElevenExcelTemplate("11", "刘永清要求模板");
            try
            {
                string strSavePath = string.Empty;
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                report.historyRecordid = selectLong.FirstOrDefault().ToString();
                HistoryRecord recordr = HistoryRecord.FindById(selectLong.FirstOrDefault());
                report.specList = DataBaseHelper.QueryByEdition(recordr.SpecListName, recordr.FilePath, recordr.EditionType);
                report.Spec = specList.Specs.FirstOrDefault();//替换谱信息
                report.operateMember = report.specList.Operater;//操作员信息，公开控制可以自定义显示不显示
                WorkCurve historyCurve = WorkCurve.FindById(recordr.WorkCurveId);//WorkCurve.FindOne(w=>w.Name==report.specList.WorkCurveName);//查找当时测量时的工作曲线
                report.Elements = historyCurve.ElementList;//画谱图时用到感兴趣元素边界
                var tt = historyCurve.ElementList.Items.ToList().OrderBy(w => w.RowsIndex);//行号排序
                report.Elements.Items.Clear();//清空所有记录以前
                tt.ToList().ForEach(w => report.Elements.Items.Add(w));//重新添加排序后的记录
                report.InterestElemCount = report.Elements.Items.ToList().FindAll(w => w.IsShowElement).Count;//画感兴趣元素时 选择的个数 为0不画
                report.WorkCurveName = historyCurve.Name;//当前工作曲线名称
                report.ReportPath = WorkCurveHelper.ExcelPath;//Excel保存路径
                report.RetestFileName = Application.StartupPath + "/HistoryExcelTemplate/" + ExcelTemplateParams.ManyTimeTemplate;//模板路径
                string FileName = GetDefineReportName();//报告名称
                bool showUnit = false;
                bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestIsShowUnit"), out showUnit);
                DataTable dt = CreateReTestTable(selectLong, showUnit);//表格内容
                bool flag = false;
                bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
                bool bFullName = false;
                try
                {
                    bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestFullElemName"), out bFullName);
                }
                catch
                {
                    bFullName = false;
                }
                string strStaticContent = ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStaContent");
                List<string> statics = (strStaticContent == null || strStaticContent.Trim() == string.Empty) ? null : strStaticContent.Split(',').ToList();
                if (flag && selectLong.Count > 1)
                {
                    AddStaticsRows(dt, "time", statics);//增加统计信息

                }
                if (bFullName)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        //if (ReportTemplateHelper.ExcelModeType == 21)
                        //{
                        //    Atom atom = Atoms.AtomList.Find(s => s.AtomName == curEle.Caption);
                        //    string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        //    showElem = Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN + "(" + curEle.Caption + ")" : curEle.Caption;
                        //}
                        //else
                        //    showElem = curEle.Caption;

                        Atom atom = Atoms.AtomList.Find(s => s.AtomName == dc.ColumnName);
                        if (atom == null) continue;
                        string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        string atomNameEN = (atom == null) ? "" : atom.AtomNameEN;
                        dc.Caption = Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN + "(" + dc.ColumnName + ")" : dc.ColumnName;
                    }
                }
                strSavePath = report.GenerateRetestReport(FileName, dt, true, true);//生成多次报告

                return strSavePath;
            }
            catch
            {
                return string.Empty;
            }
        }


        private void GetBengalHistoryRecord(List<long> selectLong, bool print)
        {
            var uc = new UCHistoryRecord();//贵金属无历史记录界面 否则可以直接调用现有历史记录控件
            var miLoad = uc.GetType().GetMethod("UCHistoryRecord_Load", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            miLoad.Invoke(uc, new object[] { null, null });
            var fiSelectLong = uc.GetType().GetField("selectLong", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            fiSelectLong.SetValue(uc, selectLong);
            MethodInfo method = uc.GetType().GetMethod("btwTemplateExcel_Click", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (print)
            {
                var fiPrint = uc.GetType().GetField("chkPrintHistory", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (fiPrint != null)
                {
                    var pi = fiPrint.FieldType.GetProperty("Checked");
                    if (pi != null)
                    {
                        var con = fiPrint.GetValue(uc);
                        if (con != null)
                            pi.SetValue(con, true, null);
                    }
                }

            }
            method.Invoke(uc, new object[] { null, null });

        }

        /// <summary>
        /// 16 三亚贵金属Excel模板
        /// </summary>
        /// <param name="selectLong"></param>
        /// <returns></returns>
        private string GetHistoryRecordSanYaReport(List<long> selectLong)
        {
            #region
            try
            {
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return string.Empty;
                }
                else
                {
                    #region 初始化表格
                    string workCurveName = string.Empty;
                    string WorkCurveID = string.Empty;
                    string historyRecordid = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    SpecListEntity tempList = new SpecListEntity();
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;
                        elementList.RhIsLayer = workCurve.ElementList != null ? workCurve.ElementList.RhIsLayer : false;
                        WorkCurveID = workCurve.Id.ToString();
                        workCurveName = workCurve.Name;
                        historyRecordid = selectLong[i].ToString();
                        report.ReadingNo = record.HistoryRecordCode;
                        report.dWeight = record.Weight;
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName, record.FilePath, record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                                spec = tempList.Specs[0];
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return "";
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList != null && curveElement.ElementList.WorkCurve != null && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            if (temp != null && !temp.IsShowElement) continue;
                            if (temp == null)//感兴趣元素外的历史记录元素
                            {
                                temp = CurveElement.New;
                                temp.Caption = element.elementName;
                                temp.Intensity = element.CaculateIntensity;
                                temp.Error = element.Error;
                                if (element.unitValue == 2)
                                    temp.Error = temp.Error / 10000;
                                else
                                    temp.Error = temp.Error / 10;
                                double elecontent = 0.0;
                                double.TryParse(element.contextelementValue, out elecontent);
                                if (element.unitValue == 1)
                                    temp.Content = elecontent;
                                else if (element.unitValue == 2)
                                    temp.Content = elecontent / 10000;
                                else
                                    temp.Content = elecontent / 10;
                                double.TryParse(element.thickelementValue, out elecontent);
                                temp.Thickness = elecontent;
                                temp.IsShowElement = true;
                                temp.ContentUnit = (ContentUnit)element.unitValue;
                                elementList.Items.Add(temp);
                                report.historyStandID = element.customstandard_Id;
                                continue;
                            }
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            if (element.unitValue == 2)
                                temp.Error = temp.Error / 10000;
                            else
                                temp.Error = temp.Error / 10;

                            if (element.unitValue == 1)
                                temp.Content = content;
                            else if (element.unitValue == 2)
                                temp.Content = content / 10000;
                            else
                                temp.Content = content / 10;
                            double.TryParse(element.thickelementValue, out content);
                            temp.Thickness = content;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    #endregion

                    #region
                    if (elementList.Items.Count == 0)
                    {
                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        return "";
                    }
                    #endregion
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.Elements = elementList;
                    report.WorkCurveName = workCurveName;
                    report.WorkCurveID = WorkCurveID;
                    report.historyRecordid = historyRecordid;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    string reportName = GetDefineReportName();
                    //
                    string StrSavePath = string.Empty;
                    if (selectLong.Count == 1)
                    {
                        report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                        report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        StrSavePath = report.GenerateReport(reportName, true);
                        //
                    }
                    else
                    {
                        DataTable dt = CreateReTestTable(selectLong);
                        int cont = 0;
                        for (int j = 0; j < selectLong.Count; j++)
                        {
                            DataRow rowNew = dt.NewRow();
                            rowNew["Time"] = ++cont;
                            foreach (DataColumn column in dt.Columns)
                            {
                                HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.Caption && w.HistoryRecord.Id == selectLong[j]);
                                if (element != null)
                                {
                                    string valueStr = element.contextelementValue;
                                    if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                                    {
                                        double Value = double.Parse(valueStr);
                                        if (element.unitValue == 1)
                                            Value = Value * 10000;
                                        else if (element.unitValue == 3)
                                            Value = Value * 1000;

                                        if (element.unitValue == 1)
                                        {
                                            rowNew[column.Caption] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                                        }
                                        else if (element.unitValue == 2)
                                        {
                                            rowNew[column.Caption] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                                        }
                                        else
                                        {
                                            rowNew[column.Caption] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                                        }
                                    }
                                }
                            }
                            dt.Rows.Add(rowNew);
                        }

                        bool flag = false;
                        bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);

                        if (flag)
                            AddStaticsRows(dt, "time", null);

                        report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                        report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        StrSavePath = report.GenerateRetestReport(reportName, dt, true, false);
                        //
                    }
                    if (!File.Exists(StrSavePath))
                        return "";
                    else
                        return StrSavePath;
                }
            }
            catch
            {
                return "";
            }
            #endregion
        }

        /// <summary>
        /// 13,14 贵金属Excel定制模板
        /// </summary>
        /// <param name="selectLong"></param>
        /// <returns></returns>
        private string GetHistoryRecordTailorReport(List<long> selectLong)
        {
            #region
            try
            {
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return string.Empty;
                }
                else
                {
                    #region 初始化表格
                    string workCurveName = string.Empty;
                    string WorkCurveID = string.Empty;
                    string historyRecordid = string.Empty;
                    SpecEntity spec = null;
                    SpecListEntity tempList = null;
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;

                        WorkCurveID = workCurve.Id.ToString();
                        workCurveName = workCurve.Name;
                        historyRecordid = selectLong[i].ToString();
                        report.ReadingNo = record.HistoryRecordCode;
                        report.dWeight = record.Weight;
                        if (tempList == null)
                            tempList = DataBaseHelper.QueryByEdition(record.SpecListName, record.FilePath, record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData) && spec == null)
                                spec = tempList.Specs[0];
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return "";
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList != null && curveElement.ElementList.WorkCurve != null && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            if (temp != null && !temp.IsShowElement) continue;
                            if (temp == null)//感兴趣元素外的历史记录元素
                            {
                                temp = CurveElement.New;
                                temp.Intensity = element.CaculateIntensity;
                                temp.Error = element.Error;
                                if (element.unitValue == 2)
                                    temp.Error = temp.Error / 10000;
                                else
                                    temp.Error = temp.Error / 10;
                                double elecontent = 0.0;
                                double.TryParse(element.contextelementValue, out elecontent);
                                if (element.unitValue == 1)
                                    temp.Content = elecontent;
                                else if (element.unitValue == 2)
                                    temp.Content = elecontent / 10000;
                                else
                                    temp.Content = elecontent / 10;
                                elementList.Items.Add(temp);
                            }
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            if (element.unitValue == 2)
                                temp.Error = temp.Error / 10000;
                            else
                                temp.Error = temp.Error / 10;

                            if (element.unitValue == 1)
                                temp.Content = content;
                            else if (element.unitValue == 2)
                                temp.Content = content / 10000;
                            else
                                temp.Content = content / 10;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    #endregion

                    #region
                    if (elementList.Items.Count == 0)
                    {
                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        return "";
                    }
                    #endregion

                    report.IsAnalyser = true;
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.Elements = elementList;
                    report.WorkCurveName = workCurveName;
                    report.WorkCurveID = WorkCurveID;
                    report.historyRecordid = selectLong.Count > 0 ? selectLong[0].ToString() : historyRecordid;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    string reportName = GetDefineReportName();
                    //
                    string StrSavePath = string.Empty;
                    if (selectLong.Count == 1)
                    {
                        report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                        report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        StrSavePath = report.GenerateReport(reportName, true);
                        //
                    }
                    else
                    {
                        DataTable dt = CreateReTestTable(selectLong, 13);
                        for (int j = 0; j < selectLong.Count; j++)
                        {
                            DataRow rowNew = dt.NewRow();
                            HistoryRecord HR = HistoryRecord.FindById(selectLong[j]);
                            if (HR != null)
                                rowNew["NAMES"] = HR.SampleName;
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (column.ColumnName.Equals("KARAT (kt)"))
                                {
                                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == "Au" && w.HistoryRecord.Id == selectLong[j]);
                                    if (element != null)
                                        rowNew[column.Caption] = (double.Parse(element.contextelementValue) * 24 / WorkCurveHelper.KaratTranslater).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                                }
                                else
                                {
                                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.ColumnName && w.HistoryRecord.Id == selectLong[j]);
                                    if (element != null)
                                    {
                                        string valueStr = element.contextelementValue;
                                        if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                                        {
                                            double Value = double.Parse(valueStr);
                                            if (element.unitValue == 1)
                                                Value = Value * 10000;
                                            else if (element.unitValue == 3)
                                                Value = Value * 1000;

                                            if (element.unitValue == 1)
                                            {
                                                rowNew[column.ColumnName] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                                            }
                                            else if (element.unitValue == 2)
                                            {
                                                rowNew[column.ColumnName] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                                            }
                                            else
                                            {
                                                rowNew[column.ColumnName] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                                            }
                                        }
                                    }
                                }
                            }
                            dt.Rows.Add(rowNew);
                        }
                        //if (dt.Rows.Count < 3)
                        //{
                        //    dt.Rows.Add(dt.NewRow());
                        //}
                        bool flag = false;
                        bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);

                        if (flag)
                            AddStaticsRows(dt, "NAMES", 13);

                        report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                        report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        StrSavePath = report.GenerateRetestReport(reportName, dt, true, false);
                        //
                    }
                    if (!File.Exists(StrSavePath))
                        return "";
                    else
                        return StrSavePath;
                }
            }
            catch
            {
                return "";
            }
            #endregion
        }

        /// <summary>
        ///1、测试结果单位由ppm改为mg/kg。
        ///2、全分析报告模板修改表头、表格格式、将测试结果和统计结果合二为一，去掉最小值和最大值，保留均值和相对标准偏差，且单位和数据值分开，
        ///3、多元素显示一行。
        ///4、谱图中用不同彩色定义感兴趣元素。
        ///5、能否横向设置
        /// </summary>
        /// <param name="selectLong"></param>
        /// <returns></returns>
        private string GetHistoryRecordEleven(List<long> selectLong)
        {
            //ExcelTemplateReportBase eleven = new ElevenExcelTemplate("11", "刘永清要求模板");
            try
            {
                string strSavePath = string.Empty;
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                report.historyRecordid = selectLong.FirstOrDefault().ToString();
                HistoryRecord recordr = HistoryRecord.FindById(selectLong.FirstOrDefault());
                report.specList = DataBaseHelper.QueryByEdition(recordr.SpecListName, recordr.FilePath, recordr.EditionType);
                report.Spec = specList.Specs.FirstOrDefault();//替换谱信息
                report.operateMember = report.specList.Operater;//操作员信息，公开控制可以自定义显示不显示
                WorkCurve historyCurve = WorkCurve.FindById(recordr.WorkCurveId);//WorkCurve.FindOne(w=>w.Name==report.specList.WorkCurveName);//查找当时测量时的工作曲线
                report.Elements = historyCurve.ElementList;//画谱图时用到感兴趣元素边界
                var tt = historyCurve.ElementList.Items.ToList().OrderBy(w => w.RowsIndex);//行号排序
                report.Elements.Items.Clear();//清空所有记录以前
                tt.ToList().ForEach(w => report.Elements.Items.Add(w));//重新添加排序后的记录
                report.InterestElemCount = report.Elements.Items.ToList().FindAll(w => w.IsShowElement).Count;//画感兴趣元素时 选择的个数 为0不画
                report.WorkCurveName = historyCurve.Name;//当前工作曲线名称
                report.ReportPath = WorkCurveHelper.ExcelPath;//Excel保存路径
                report.RetestFileName = Application.StartupPath + "/HistoryExcelTemplate/" + ExcelTemplateParams.ManyTimeTemplate;//模板路径
                string FileName = GetDefineReportName();//报告名称
                DataTable dt = CreateReTestTable(selectLong, false);//表格内容
                bool flag = false;
                bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);

                if (flag && selectLong.Count > 1)
                {
                    AddStaticsRows(dt, "time", null);//增加统计信息

                    if (!System.Diagnostics.Debugger.IsAttached)
                    {
                        dt.Rows.RemoveAt(dt.Rows.Count - 3);//最小值移出
                        dt.Rows.RemoveAt(dt.Rows.Count - 3);//最大值移出
                        dt.Rows.RemoveAt(dt.Rows.Count - 3);//SD移出
                    }
                }
                strSavePath = report.GenerateRetestReport(FileName, dt, true, false);//生成多次报告

                return strSavePath;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 创建连续测量结果数据库
        /// </summary>
        /// <param name="selectLong">历史记录Id集合</param>
        /// <param name="Extend">可选值，是否扩展显示感兴趣元素不包含但历史记录中有得元素</param>
        public DataTable CreateReTestTable(List<long> selectLong, params string[] hideElement)
        {
            DataTable reTestTable = new DataTable();
            reTestTable.Columns.Clear();
            reTestTable.Columns.Add("Time", typeof(string));
            var sSelectLong = selectLong.ConvertAll<string>(w => w.ToString()).Aggregate((w, r) => r + "," + w);//数字数组转换为字符数字 通过","合并
            var elements = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id in (" + sSelectLong.ToString() + ")").Select(w => w.elementName).Distinct();
            //var orders = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex).Select(w => w.Caption);//排序去取元素名 所有感兴趣元素
            var orders = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex).Select(w => w.Formula);
            var left = elements.Except(orders);//感兴趣元素外的元素            
            orders = orders.Concat(left);
            orders = orders.Except(hideElement);//移除需要隐藏的列
            foreach (var element in orders)
            {
                if (!reTestTable.Columns.Contains(element))
                {
                    reTestTable.Columns.Add(element, typeof(string));
                }


            }

            return reTestTable;
        }

        public DataTable CreateReTestTable(List<long> selectLong, int count, params string[] hideElement)
        {
            DataTable reTestTable = new DataTable();
            reTestTable.Columns.Clear();
            reTestTable.Columns.Add("NAMES", typeof(string));
            var sSelectLong = selectLong.ConvertAll<string>(w => w.ToString()).Aggregate((w, r) => r + "," + w);//数字数组转换为字符数字 通过","合并
            var elements = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id in (" + sSelectLong.ToString() + ")").Select(w => w.elementName).Distinct();
            var orders = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex).Select(w => w.Caption);//排序去取元素名 所有感兴趣元素
            var left = elements.Except(orders);//感兴趣元素外的元素            
            orders = orders.Concat(left);
            orders = orders.Except(hideElement);//移除需要隐藏的列
            foreach (var element in orders)
            {
                string elem = "";
                Atom ato = Atoms.AtomList.Find(s => s.AtomName == element);
                elem = (ato == null) ? elem : ato.AtomNameEN + "(" + ato.AtomName + ")";
                if (!reTestTable.Columns.Contains(elem))
                {
                    DataColumn dc = new DataColumn(element);
                    dc.Caption = elem;
                    reTestTable.Columns.Add(dc);
                    //reTestTable.Columns.Add(elem, typeof(string));
                }
            }
            reTestTable.Columns.Add("KARAT (kt)", typeof(string));
            return reTestTable;
        }

        private DataTable CreateReTestTable(List<long> selectLong, bool showUnit)
        {
            DataTable dt = CreateReTestTable(selectLong);
            int cont = 0;
            string sContentBit = WorkCurveHelper.SoftWareContentBit.ToString();
            for (int j = 0; j < selectLong.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                rowNew["Time"] = ++cont;
                foreach (DataColumn column in dt.Columns)
                {
                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.Caption && w.HistoryRecord.Id == selectLong[j]);
                    if (element != null)
                    {
                        string valueStr = element.contextelementValue;
                        if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                        {
                            double Value = double.Parse(valueStr);
                            if (showUnit)
                            {
                                if (element.unitValue == 1)
                                    Value = Value * 10000;
                                else if (element.unitValue == 3)
                                    Value = Value * 1000;

                                if (element.unitValue == 1)
                                {
                                    rowNew[column.Caption] = (Value / 10000).ToString("f" + sContentBit) + "(%)";
                                }
                                else if (element.unitValue == 2)
                                {
                                    rowNew[column.Caption] = Value.ToString("f" + sContentBit) + "(ppm)";
                                }
                                else
                                {
                                    rowNew[column.Caption] = (Value / 1000).ToString("f" + sContentBit) + "(‰)";
                                }
                            }
                            else
                                rowNew[column.Caption] = Value.ToString("f" + sContentBit);
                        }
                    }
                    else
                    {
                        if (column.Caption != "Time")
                            rowNew[column.Caption] = default(double).ToString("f" + sContentBit);
                    }
                }
                dt.Rows.Add(rowNew);
            }
            return dt;
        }

        //public List<DataRow> AddStaticsRows(DataTable dt, string HeadColumnName)
        public void AddStaticsRows(DataTable dt, string HeadColumnName, List<string> staticsShow)
        {

            //计算带单位的历史记录的最大值、最小值、平均值及标准偏差
            var lr = dt.Select();

            DataRow drMax = dt.NewRow();//新建最大值行
            DataRow drMin = dt.NewRow();//新建最小值行
            DataRow drAva = dt.NewRow();//新建平均值行
            DataRow drSD = dt.NewRow();//新建标准偏差行
            DataRow drRSD = dt.NewRow();//新建相对标准偏差

            foreach (DataColumn column in dt.Columns)
            {
                if (string.Compare(column.ColumnName, HeadColumnName, true) == 0)
                {
                    //首列填充列头说明信息
                    drMax[column] = Info.MaxValue;
                    drMin[column] = Info.MinValue;
                    drAva[column] = Info.MeanValue;
                    drSD[column] = Info.SDValue;
                    //drRSD[column] = Info.RSDValue;
                    if (DifferenceDevice.IsShowWeight) drRSD[column] = Info.Total + Info.Weight;
                    else drRSD[column] = Info.RSDValue;
                    continue;
                }
                if (column.DataType != typeof(string))
                //if (column.DataType != typeof(string) ||
                //    (!column.ColumnName.Contains(Info.Content)
                //    && !column.ColumnName.Contains(Info.EditContent)
                //    && !column.ColumnName.Contains(Info.Thick)
                //    && !column.ColumnName.Contains(Info.Intensity)
                //    && !column.ColumnName.Contains(Info.strAreaDensity)
                //    && !column.ColumnName.Contains(Info.Weight)
                //    && !column.ColumnName.Contains(Info.IncludingAu)
                //    && !column.ColumnName.Contains(Info.Weight)
                //    && !column.ColumnName.Contains(Info.Count)
                //    && !column.ColumnName.Contains(Info.Resolve)
                //    && (Atoms.AtomList.Find(w => w.AtomName == column.ColumnName) == null)))//2013-03-29追加 cyq
                {

                    continue;
                }
                //Atom a = Atoms.AtomList.Find(w => w.AtomName == column.ColumnName);
                //获取该列单位类型个数
                var majorUnit = from l in lr
                                where (l[column] != System.DBNull.Value && l[column].ToString() != "")//过滤空白项
                                group l by l[column].ToString().Split('(').Length > 1 ? l[column].ToString().Split('(')[1] : "" into g
                                orderby g.Count() descending
                                select new
                                {
                                    Key = g.Key,
                                    Count = g.Count()
                                };
                //只有一个类型的单位直接计算
                var sMajorUnit = (majorUnit.FirstOrDefault() == null || majorUnit.FirstOrDefault().Key == "") ? "" : "(" + majorUnit.FirstOrDefault().Key;//获取单位个数最多的那个单位作为统计结果平均值和标准偏差的单位
                if (majorUnit.Count() <= 0 || sMajorUnit == string.Empty)//2013-03-26追加 cyq
                {
                    try
                    {
                        Convert.ToDouble(lr[0][column]);
                    }
                    catch (System.Exception)
                    {
                        continue;
                    }
                }
                if (majorUnit.Count() <= 1)
                {
                    var value = from l in lr
                                where (l[column] != System.DBNull.Value && l[column].ToString() != "")//过滤空白项
                                group l by 1 into g
                                let dAva = g.Average(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()))
                                let dSD = Math.Sqrt(g.Aggregate(0.0, (result, p) => result +
                                       Math.Pow(Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()) -
                                       dAva
                                       , 2)
                                       / (g.Count() - 1))
                                       )
                                select new
                                {//获取最大值、最小值、平均值、标准偏差数值
                                    Max = g.Max(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
                                    Min = g.Min(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
                                    Ava = dAva,//g.Average(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
                                    SD = dSD,
                                    //SD = Math.Sqrt(g.Aggregate(0.0, (result, p) => result +
                                    //   Math.Pow(Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()) -
                                    //   g.Average(w => Convert.ToDouble(p[column] == DBNull.Value ? "0" : w[column].ToString().Split('(').FirstOrDefault()))
                                    //   , 2)
                                    //   / (g.Count() - 1))
                                    //   ),
                                    //    RSD = (dAva <= 0 || dSD <= 0) ? dSD : (Math.Round(dSD, ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity))? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero)
                                    //       /Math.Round(dAva,((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity))? WorkCurveHelper.ThickBit :WorkCurveHelper.SoftWareContentBit),MidpointRounding.AwayFromZero)*100)
                                    //};
                                    RSD = DifferenceDevice.IsShowWeight ? dAva * g.Count() : (((dAva <= 0 || dSD <= 0)) ? dSD : (Math.Round(dSD, ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero)
                                           / Math.Round(dAva, ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero) * 100))
                                };
                    drMax[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().Max.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;//附加计算结果单位
                    drMin[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().Min.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
                    //drAva[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().Ava.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
                    //drSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().SD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
                    drAva[column] = DifferenceDevice.IsShowWeight && column.ColumnName.Contains("(" + Info.Weight + ")") ? "" : (value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().Ava.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit);
                    drSD[column] = DifferenceDevice.IsShowWeight && column.ColumnName.Contains("(" + Info.Weight + ")") ? "" : (value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().SD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit);
                    //drRSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().RSD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + "%";//
                    //continue;
                    //更改为元素总重量
                    if (DifferenceDevice.IsShowWeight)
                    {
                        if (column.ColumnName.Contains("(" + Info.Weight + ")"))
                            drRSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().RSD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
                    }
                    else drRSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().RSD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + "%";
                    continue;
                }

                //多于一个单位的转换成ppm统一计算再由ppm转回主单位 可以兼容一个单位 但效率低
                double Max = double.MinValue, Min = double.MaxValue, Ava = 0.0, SD = 0.0, RSD = 0.0;
                foreach (DataRow row in dt.Rows)
                {
                    string value = row[column].ToString();
                    if (string.IsNullOrEmpty(value)) continue;//过滤空白项
                    string[] values = value.Split('(');
                    double valueDgt = Convert.ToDouble(values.FirstOrDefault());
                    valueDgt *= values.LastOrDefault().Contains("%") ? 10000 : values.LastOrDefault().Contains("‰") ? 1000 : 1;//不同单位转换为ppm
                    if (Max < valueDgt)
                    {
                        drMax[column] = value;
                        Max = valueDgt;
                    }

                    if (Min > valueDgt)
                    {
                        drMin[column] = value;
                        Min = valueDgt;
                    }
                    Ava += valueDgt;
                }
                Ava /= dt.Rows.Count;
                int iUnitConvert = sMajorUnit.Contains("%") ? 10000 : sMajorUnit.Contains("‰") ? 1000 : 1;//换算回主单位对应的系数
                drAva[column] = (Ava / iUnitConvert).ToString("f" + (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;//ppm转换回主单位
                foreach (DataRow row in dt.Rows)
                {
                    string value = row[column].ToString();
                    if (string.IsNullOrEmpty(value)) continue;//过滤空白项
                    string[] values = value.Split('(');
                    double valueDgt = Convert.ToDouble(values.FirstOrDefault());
                    valueDgt *= values.LastOrDefault().Contains("%") ? 10000 : values.LastOrDefault().Contains("‰") ? 1000 : 1;
                    SD += Math.Pow(valueDgt - Ava, 2);
                }
                SD = Math.Sqrt(SD / (dt.Rows.Count - 1));
                if (Ava <= 0 || SD <= 0)
                    RSD = SD;
                else
                    RSD = (Math.Round(SD, (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero)
                        / Math.Round(Ava, (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero)) * 100;
                drSD[column] = (SD / iUnitConvert).ToString("f" + (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
                drRSD[column] = RSD.ToString("f" + (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + "%";
            }
            if (staticsShow == null || staticsShow.Count <= 0)
            {
                dt.Rows.Add(drMax);
                dt.Rows.Add(drMin);
                dt.Rows.Add(drSD);
                dt.Rows.Add(drAva);
                dt.Rows.Add(drRSD);
                return;
            }
            if (staticsShow.Find(w => w.ToUpper().Equals("MAX")) != null)
                dt.Rows.Add(drMax);
            if (staticsShow.Find(w => w.ToUpper().Equals("MIN")) != null)
                dt.Rows.Add(drMin);
            if (staticsShow.Find(w => w.ToUpper().Equals("SD")) != null)
                dt.Rows.Add(drSD);
            if (staticsShow.Find(w => w.ToUpper().Equals("AVERAGE")) != null)
                dt.Rows.Add(drAva);
            if (staticsShow.Find(w => w.ToUpper().Equals("RSD")) != null)
                dt.Rows.Add(drRSD);
            //foreach(DataColumn column in dt.Columns)
            //{
            //    if (column.Caption == "time")
            //        continue;
            //    double sd = 0.0, ava = 0.0;
            //    if (double.TryParse(drAva[column].ToString(), out ava) || double.TryParse(drSD[column].ToString(), out sd) || ava == 0)
            //        drRSD[column] = 0;
            //    else
            //        drRSD[column] = (sd / ava).ToString("f" + WorkCurveHelper.SoftWareContentBit);
            //}
            //List<DataRow> lstRows = new List<DataRow>();
            //lstRows.Add(drMax);
            //lstRows.Add(drMin);
            //lstRows.Add(drSD);
            //lstRows.Add(drAva);
            //lstRows.Add(drRSD);
            //return lstRows;

        }

        //public List<DataRow> AddStaticsRows(DataTable dt, string HeadColumnName, string EndColumnName)
        //{

        //    //计算带单位的历史记录的最大值、最小值、平均值及标准偏差
        //    var lr = dt.Select();

        //    DataRow drMax = dt.NewRow();//新建最大值行
        //    DataRow drMin = dt.NewRow();//新建最小值行
        //    DataRow drAva = dt.NewRow();//新建平均值行
        //    DataRow drSD = dt.NewRow();//新建标准偏差行
        //    DataRow drRSD = dt.NewRow();//新建相对标准偏差

        //    foreach (DataColumn column in dt.Columns)
        //    {
        //        if (string.Compare(column.ColumnName, HeadColumnName, true) == 0)
        //        {
        //            //首列填充列头说明信息
        //            drMax[column] = Info.MaxValue;
        //            drMin[column] = Info.MinValue;
        //            drAva[column] = Info.MeanValue;
        //            drSD[column] = Info.SDValue;
        //            //drRSD[column] = Info.RSDValue;
        //            if (DifferenceDevice.IsShowWeight) drRSD[column] = Info.Total + Info.Weight;
        //            else drRSD[column] = Info.RSDValue;
        //            continue;
        //        }
        //        if (column.DataType != typeof(string) ||
        //            (!column.ColumnName.Contains(Info.Content)
        //            && !column.ColumnName.Contains(Info.EditContent)
        //            && !column.ColumnName.Contains(Info.Thick)
        //            && !column.ColumnName.Contains(Info.Intensity)
        //            && !column.ColumnName.Contains(Info.strAreaDensity)
        //            && !column.ColumnName.Contains(Info.Weight)
        //            && !column.ColumnName.Contains(Info.IncludingAu)
        //            && !column.ColumnName.Contains(Info.Weight)
        //            && !column.ColumnName.Contains(Info.Count)
        //            && !column.ColumnName.Contains(Info.Resolve)
        //            && (Atoms.AtomList.Find(w => w.AtomName == column.ColumnName) == null)))//2013-03-29追加 cyq
        //        {

        //            continue;
        //        }
        //        Atom a = Atoms.AtomList.Find(w => w.AtomName == column.ColumnName);
        //        //获取该列单位类型个数
        //        var majorUnit = from l in lr
        //                        where (l[column] != System.DBNull.Value && l[column].ToString() != "")//过滤空白项
        //                        group l by l[column].ToString().Split('(').Length > 1 ? l[column].ToString().Split('(')[1] : "" into g
        //                        orderby g.Count() descending
        //                        select new
        //                        {
        //                            Key = g.Key,
        //                            Count = g.Count()
        //                        };
        //        //只有一个类型的单位直接计算
        //        var sMajorUnit = (majorUnit.FirstOrDefault() == null || majorUnit.FirstOrDefault().Key == "") ? "" : "(" + majorUnit.FirstOrDefault().Key;//获取单位个数最多的那个单位作为统计结果平均值和标准偏差的单位
        //        if (majorUnit.Count() <= 0 || sMajorUnit == string.Empty)//2013-03-26追加 cyq
        //        {
        //            try
        //            {
        //                Convert.ToDouble(lr[0][column]);
        //            }
        //            catch (System.Exception)
        //            {
        //                continue;
        //            }
        //        }
        //        if (majorUnit.Count() <= 1)
        //        {
        //            var value = from l in lr
        //                        where (l[column] != System.DBNull.Value && l[column].ToString() != "")//过滤空白项
        //                        group l by 1 into g
        //                        let dAva = g.Average(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()))
        //                        let dSD = Math.Sqrt(g.Aggregate(0.0, (result, p) => result +
        //                               Math.Pow(Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()) -
        //                               dAva
        //                               , 2)
        //                               / (g.Count() - 1))
        //                               )
        //                        select new
        //                        {//获取最大值、最小值、平均值、标准偏差数值
        //                            Max = g.Max(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
        //                            Min = g.Min(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
        //                            Ava = dAva,//g.Average(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
        //                            SD = dSD,
        //                            //SD = Math.Sqrt(g.Aggregate(0.0, (result, p) => result +
        //                            //   Math.Pow(Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()) -
        //                            //   g.Average(w => Convert.ToDouble(p[column] == DBNull.Value ? "0" : w[column].ToString().Split('(').FirstOrDefault()))
        //                            //   , 2)
        //                            //   / (g.Count() - 1))
        //                            //   ),
        //                            //    RSD = (dAva <= 0 || dSD <= 0) ? dSD : (Math.Round(dSD, ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity))? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero)
        //                            //       /Math.Round(dAva,((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity))? WorkCurveHelper.ThickBit :WorkCurveHelper.SoftWareContentBit),MidpointRounding.AwayFromZero)*100)
        //                            //};
        //                            RSD = DifferenceDevice.IsShowWeight ? dAva * g.Count(k) : (((dAva <= 0 || dSD <= 0)) ? dSD : (Math.Round(dSD, ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero)
        //                                   / Math.Round(dAva, ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero) * 100))
        //                        };
        //            drMax[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().Max.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;//附加计算结果单位
        //            drMin[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().Min.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
        //            //drAva[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().Ava.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
        //            //drSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().SD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
        //            drAva[column] = DifferenceDevice.IsShowWeight && column.ColumnName.Contains("(" + Info.Weight + ")") ? "" : (value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().Ava.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit);
        //            drSD[column] = DifferenceDevice.IsShowWeight && column.ColumnName.Contains("(" + Info.Weight + ")") ? "" : (value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().SD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit);
        //            //drRSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().RSD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + "%";//
        //            //continue;
        //            //更改为元素总重量
        //            if (DifferenceDevice.IsShowWeight)
        //            {
        //                if (column.ColumnName.Contains("(" + Info.Weight + ")"))
        //                    drRSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().RSD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
        //            }
        //            else drRSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) : value.FirstOrDefault().RSD.ToString("f" + ((column.ColumnName.Contains(Info.Thick) || column.ColumnName.Contains(Info.strAreaDensity)) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + "%";
        //            continue;
        //        }

        //        //多于一个单位的转换成ppm统一计算再由ppm转回主单位 可以兼容一个单位 但效率低
        //        double Max = double.MinValue, Min = double.MaxValue, Ava = 0.0, SD = 0.0, RSD = 0.0;
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            string value = row[column].ToString();
        //            if (string.IsNullOrEmpty(value)) continue;//过滤空白项
        //            string[] values = value.Split('(');
        //            double valueDgt = Convert.ToDouble(values.FirstOrDefault());
        //            valueDgt *= values.LastOrDefault().Contains("%") ? 10000 : values.LastOrDefault().Contains("‰") ? 1000 : 1;//不同单位转换为ppm
        //            if (Max < valueDgt)
        //            {
        //                drMax[column] = value;
        //                Max = valueDgt;
        //            }

        //            if (Min > valueDgt)
        //            {
        //                drMin[column] = value;
        //                Min = valueDgt;
        //            }
        //            Ava += valueDgt;
        //        }
        //        Ava /= dt.Rows.Count;
        //        int iUnitConvert = sMajorUnit.Contains("%") ? 10000 : sMajorUnit.Contains("‰") ? 1000 : 1;//换算回主单位对应的系数
        //        drAva[column] = (Ava / iUnitConvert).ToString("f" + (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;//ppm转换回主单位
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            string value = row[column].ToString();
        //            if (string.IsNullOrEmpty(value)) continue;//过滤空白项
        //            string[] values = value.Split('(');
        //            double valueDgt = Convert.ToDouble(values.FirstOrDefault());
        //            valueDgt *= values.LastOrDefault().Contains("%") ? 10000 : values.LastOrDefault().Contains("‰") ? 1000 : 1;
        //            SD += Math.Pow(valueDgt - Ava, 2);
        //        }
        //        SD = Math.Sqrt(SD / (dt.Rows.Count - 1));
        //        if (Ava <= 0 || SD <= 0)
        //            RSD = SD;
        //        else
        //            RSD = (Math.Round(SD, (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero)
        //                / Math.Round(Ava, (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit), MidpointRounding.AwayFromZero)) * 100;
        //        drSD[column] = (SD / iUnitConvert).ToString("f" + (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + sMajorUnit;
        //        drRSD[column] = RSD.ToString("f" + (column.ColumnName.Contains(Info.Thick) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit)) + "%";
        //    }
        //    dt.Rows.Add(drMax);
        //    dt.Rows.Add(drMin);
        //    dt.Rows.Add(drSD);
        //    dt.Rows.Add(drAva);
        //    dt.Rows.Add(drRSD);
        //    //foreach(DataColumn column in dt.Columns)
        //    //{
        //    //    if (column.Caption == "time")
        //    //        continue;
        //    //    double sd = 0.0, ava = 0.0;
        //    //    if (double.TryParse(drAva[column].ToString(), out ava) || double.TryParse(drSD[column].ToString(), out sd) || ava == 0)
        //    //        drRSD[column] = 0;
        //    //    else
        //    //        drRSD[column] = (sd / ava).ToString("f" + WorkCurveHelper.SoftWareContentBit);
        //    //}
        //    List<DataRow> lstRows = new List<DataRow>();
        //    lstRows.Add(drMax);
        //    lstRows.Add(drMin);
        //    lstRows.Add(drSD);
        //    lstRows.Add(drAva);
        //    lstRows.Add(drRSD);
        //    return lstRows;

        //}

        public List<DataRow> AddStaticsRows(DataTable dt, string HeadColumnName, int count)
        {

            //计算带单位的历史记录的最大值、最小值、平均值及标准偏差
            var lr = dt.Select();

            //DataRow drMax = dt.NewRow();//新建最大值行
            //DataRow drMin = dt.NewRow();//新建最小值行
            DataRow drAva = dt.NewRow();//新建平均值行
            //DataRow drSD = dt.NewRow();//新建标准偏差行
            //DataRow drRSD = dt.NewRow();//新建相对标准偏差

            foreach (DataColumn column in dt.Columns)
            {
                if (string.Compare(column.ColumnName, HeadColumnName, true) == 0)
                {
                    //首列填充列头说明信息
                    //drMax[column] = Info.MaxValue;
                    //drMin[column] = Info.MinValue;
                    drAva[column] = Info.MeanValue;
                    //drSD[column] = Info.SDValue;
                    //drRSD[column] = Info.RSDValue;
                    continue;
                }
                //获取该列单位类型个数
                var majorUnit = from l in lr
                                where (l[column] != System.DBNull.Value && l[column].ToString() != "")//过滤空白项
                                group l by l[column].ToString().Split('(').Length > 1 ? l[column].ToString().Split('(')[1] : "" into g
                                orderby g.Count() descending
                                select new
                                {
                                    Key = g.Key,
                                    Count = g.Count()
                                };
                //只有一个类型的单位直接计算
                var sMajorUnit = (majorUnit.FirstOrDefault() == null || majorUnit.FirstOrDefault().Key == "") ? "" : "(" + majorUnit.FirstOrDefault().Key;//获取单位个数最多的那个单位作为统计结果平均值和标准偏差的单位
                if (majorUnit.Count() <= 1)
                {
                    var value = from l in lr
                                where (l[column] != System.DBNull.Value && l[column].ToString() != "")//过滤空白项
                                group l by 1 into g
                                let dAva = g.Average(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()))
                                let dSD = Math.Sqrt(g.Aggregate(0.0, (result, p) => result +
                                       Math.Pow(Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()) -
                                       dAva
                                       , 2)
                                       / (g.Count() - 1))
                                       )
                                select new
                                {//获取最大值、最小值、平均值、标准偏差数值
                                    Max = g.Max(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
                                    Min = g.Min(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
                                    Ava = dAva,//g.Average(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
                                    SD = dSD,
                                    //SD = Math.Sqrt(g.Aggregate(0.0, (result, p) => result +
                                    //   Math.Pow(Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()) -
                                    //   g.Average(w => Convert.ToDouble(p[column] == DBNull.Value ? "0" : w[column].ToString().Split('(').FirstOrDefault()))
                                    //   , 2)
                                    //   / (g.Count() - 1))
                                    //   ),
                                    RSD = (dAva <= 0 || dSD <= 0) ? dSD : (Math.Round(dSD, WorkCurveHelper.SoftWareContentBit, MidpointRounding.AwayFromZero)
                                    / Math.Round(dAva, WorkCurveHelper.SoftWareContentBit, MidpointRounding.AwayFromZero) * 100)
                                };
                    //drMax[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + WorkCurveHelper.SoftWareContentBit) : value.FirstOrDefault().Max.ToString("f" + WorkCurveHelper.SoftWareContentBit) + sMajorUnit;//附加计算结果单位
                    //drMin[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + WorkCurveHelper.SoftWareContentBit) : value.FirstOrDefault().Min.ToString("f" + WorkCurveHelper.SoftWareContentBit) + sMajorUnit;
                    drAva[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + WorkCurveHelper.SoftWareContentBit) : value.FirstOrDefault().Ava.ToString("f" + WorkCurveHelper.SoftWareContentBit) + sMajorUnit;
                    //drSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + WorkCurveHelper.SoftWareContentBit) : value.FirstOrDefault().SD.ToString("f" + WorkCurveHelper.SoftWareContentBit) + sMajorUnit;
                    //drRSD[column] = value.FirstOrDefault() == null ? default(double).ToString("f" + WorkCurveHelper.SoftWareContentBit) : value.FirstOrDefault().RSD.ToString("f" + WorkCurveHelper.SoftWareContentBit) + "%";
                    continue;
                }

                //多于一个单位的转换成ppm统一计算再由ppm转回主单位 可以兼容一个单位 但效率低
                //double Max = double.MinValue, Min = double.MaxValue, Ava = 0.0, SD = 0.0, RSD = 0.0;
                double Ava = 0.0;
                foreach (DataRow row in dt.Rows)
                {
                    string value = row[column].ToString();
                    if (string.IsNullOrEmpty(value)) continue;//过滤空白项
                    string[] values = value.Split('(');
                    double valueDgt = Convert.ToDouble(values.FirstOrDefault());
                    valueDgt *= values.LastOrDefault().Contains("%") ? 10000 : values.LastOrDefault().Contains("‰") ? 1000 : 1;//不同单位转换为ppm
                    //if (Max < valueDgt)
                    //{
                    //    drMax[column] = value;
                    //    Max = valueDgt;
                    //}

                    //if (Min > valueDgt)
                    //{
                    //    drMin[column] = value;
                    //    Min = valueDgt;
                    //}
                    Ava += valueDgt;
                }
                Ava /= dt.Rows.Count;
                int iUnitConvert = sMajorUnit.Contains("%") ? 10000 : sMajorUnit.Contains("‰") ? 1000 : 1;//换算回主单位对应的系数
                drAva[column] = (Ava / iUnitConvert).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sMajorUnit;//ppm转换回主单位
                //foreach (DataRow row in dt.Rows)
                //{
                //    string value = row[column].ToString();
                //    if (string.IsNullOrEmpty(value)) continue;//过滤空白项
                //    string[] values = value.Split('(');
                //    double valueDgt = Convert.ToDouble(values.FirstOrDefault());
                //    valueDgt *= values.LastOrDefault().Contains("%") ? 10000 : values.LastOrDefault().Contains("‰") ? 1000 : 1;
                //    SD += Math.Pow(valueDgt - Ava, 2);
                //}
                //SD = Math.Sqrt(SD / (dt.Rows.Count - 1));
                //if (Ava <= 0 || SD <= 0)
                //    RSD = SD;
                //else
                //    RSD = (Math.Round(SD, WorkCurveHelper.SoftWareContentBit, MidpointRounding.AwayFromZero)
                //        / Math.Round(Ava, WorkCurveHelper.SoftWareContentBit, MidpointRounding.AwayFromZero)) * 100;
                //drSD[column] = (SD / iUnitConvert).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sMajorUnit;
                //drRSD[column] = RSD.ToString("f" + WorkCurveHelper.SoftWareContentBit) + "%";
            }
            //dt.Rows.Add(drMax);
            //dt.Rows.Add(drMin);
            //dt.Rows.Add(drSD);
            dt.Rows.Add(drAva);
            //dt.Rows.Add(drRSD);
            List<DataRow> lstRows = new List<DataRow>();
            //lstRows.Add(drMax);
            //lstRows.Add(drMin);
            //lstRows.Add(drSD);
            lstRows.Add(drAva);
            //lstRows.Add(drRSD);
            return lstRows;

        }




        public List<DataRow> AddThickStaticsRows(DataTable dt, string HeadColumnName)                  //yuzhaomodify:Thick专用模板
        {
            //DataTable dt = dtable.Copy();  //用于计算的dt
            //string columnName = null;

            //foreach (DataColumn col in dt.Columns)
            //{
            //    if (col.ColumnName.Contains(Info.Thick))
            //    {
            //        for (int i = dt.Rows.Count - 1; i > 0; i--)
            //        {
            //            if (dt.Rows[i][col.ColumnName].ToString().Contains("--"))
            //            {
            //                dt.Rows[i].Delete();
            //            }
            //        }
            //        dt.AcceptChanges();
            //    }
            //}


            //int dtRows = dt.Rows.Count;

            //计算带单位的历史记录的最大值、最小值、平均值及标准偏差
            var lr = dt.Select();
            DataRow drMax = dt.NewRow();//新建最大值行
            DataRow drMin = dt.NewRow();//新建最小值行
            DataRow drAva = dt.NewRow();//新建平均值行
            DataRow drSD = dt.NewRow();//新建标准偏差行
            DataRow drRSD = dt.NewRow();//新建相对标准偏差
            DataRow drRange = dt.NewRow();//新建极差

            foreach (DataColumn column in dt.Columns)
            {
                if (string.Compare(column.ColumnName, HeadColumnName, true) == 0)
                {
                    //首列填充列头说明信息
                    drMax[column] = Info.MaxValue;
                    drMin[column] = Info.MinValue;
                    drAva[column] = Info.MeanValue;
                    drSD[column] = Info.SDValue;
                    drRSD[column] = Info.RSDValue;
                    drRange[column] = Info.Range;
                    continue;
                }
                if (column.DataType != typeof(string) ||
                    (!column.ColumnName.Contains(Info.Content)
                    && !column.ColumnName.Contains(Info.Thick)
                    && !column.ColumnName.Contains(Info.Intensity)
                    && !column.ColumnName.Contains(Info.strAreaDensity)
                    && (Atoms.AtomList.Find(w => w.AtomName == column.ColumnName) == null)) || column.ColumnName.Contains(Info.EditContent))//2013-03-29追加 cyq
                {

                    continue;
                }



                if (drMax != null)
                    drMax[column] = "0";
                if (drMin != null)
                    drMin[column] = "0";
                if (drAva != null)
                    drAva[column] = "0";
                if (drSD != null)
                    drSD[column] = "0";
                if (drRSD != null)
                    drRSD[column] = "0";
                if (drRange != null)
                    drRange[column] = "0";

            }
            if (drAva != null)
                dt.Rows.Add(drAva);
            if (drSD != null)
                dt.Rows.Add(drSD);
            if (drRSD != null)
                dt.Rows.Add(drRSD);
            if (drMax != null)
                dt.Rows.Add(drMax);
            if (drMin != null)
                dt.Rows.Add(drMin);
            if (drRange != null)
                dt.Rows.Add(drRange);

            //// dt2.ImportRow(dt.Rows[0]);//这是加入的是第一行

            // for (int i = 0; i < dtRows; i++)
            // {
            //     dt.Rows[i].Delete();
            // }
            // dt.AcceptChanges();


            // DataRow[] dr = dt.Select();
            // for (int i = 0; i < dt.Rows.Count; i++)
            // {
            //     dtable.ImportRow(dr[i]);
            // }

            List<DataRow> lstRows = new List<DataRow>();
            if (drAva != null)
                lstRows.Add(drAva);
            if (drSD != null)
                lstRows.Add(drSD);
            if (drRSD != null)
                lstRows.Add(drRSD);
            if (drMax != null)
                lstRows.Add(drMax);
            if (drMin != null)
                lstRows.Add(drMin);
            if (drRange != null)
                lstRows.Add(drRange);
            return lstRows;

        }


        public void OpenPathThread(string excelPath)
        {
            if (WorkCurveHelper.IsBatchTest)
                return;
            if (WorkCurveHelper.IsPopUpReportOpen)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(ShowExcel));
                thread.Priority = ThreadPriority.Highest;
                thread.Start(excelPath);
            }
        }

        public void PrintThread(string excelPath)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(PrintExcelByPath), excelPath);
        }

        private void ShowExcel(Object obj)
        {
            if (!File.Exists(obj.ToString())) return;
            if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Help.ShowHelp(null, obj.ToString());
            }
            //var vk_missing = Missing.Value;
            //if (obj.ToString().ToLower().Contains(".xls"))
            //{
            //    if (WorkCurveHelper.ReportVkOrVe == 1)
            //    {
            //        WPS.Application app = new ET.Application();
            //        WPS._Workbook workBook = app.Workbooks.Open(
            //      obj.ToString(), vk_missing, vk_missing, vk_missing, vk_missing,
            //      vk_missing, vk_missing, vk_missing,
            //      vk_missing, vk_missing, vk_missing, vk_missing,vk_missing);
            //        app.Visible = true;
            //    }
            //    else
            //    {
            //        EXCELL.Application app = new Microsoft.Office.Interop.Excel.Application();
            //        EXCELL.Workbook workBook = app.Workbooks.Open(
            //        obj.ToString(), vk_missing, vk_missing, vk_missing, vk_missing,
            //        vk_missing, vk_missing, vk_missing,
            //        vk_missing, vk_missing, vk_missing, vk_missing, vk_missing,
            //        vk_missing, vk_missing);
            //        app.Visible = true;
            //    }

            //}
            //else
            //    try {

            //        System.Diagnostics.Process.Start(obj.ToString());
            //    }
            //    catch (Exception e)
            //    {
            //        Msg.Show(e.Message);
            //    }

        }

        public void SaveSelfCheckReport()
        {
            string excelPath = GetSelfCheckReport(SelfCheckObject);
            if (excelPath.Length == 0) return;
            OpenPathThread(excelPath);
        }

        private string GetSelfCheckReport(SelfCheckObject SelfCheckObject)
        {
            try
            {
                Report report = new Report();
                SelfCheckObject.Operator = GP.UserName;
                SelfCheckObject.Date = DateTime.Now.ToString();
                SelfCheckObject.Device = WorkCurveHelper.DeviceCurrent.Name;

                if (this.deviceMeasure.interfacce.DeviceParam != null && this.deviceMeasure.interfacce.DeviceParam.Condition.Type == ConditionType.Detection)
                {
                    SelfCheckObject.SpecTimes = this.deviceMeasure.interfacce.DeviceParam.PrecTime.ToString();
                    SelfCheckObject.CollimatorId = WorkCurveHelper.DeviceCurrent.HasCollimator ? this.deviceMeasure.interfacce.DeviceParam.CollimatorIdx.ToString() : "No";
                    SelfCheckObject.FilterId = WorkCurveHelper.DeviceCurrent.HasFilter ? this.deviceMeasure.interfacce.DeviceParam.FilterIdx.ToString() : "No";
                    SelfCheckObject.Voltage = this.deviceMeasure.interfacce.DeviceParam.TubVoltage.ToString("f0");
                    SelfCheckObject.Current = this.deviceMeasure.interfacce.DeviceParam.TubCurrent.ToString("f0");
                }
                report.Spec = spec;
                report.specList = specList;
                string reportName = DateTime.Now.ToString("yyyyMMddHHmmss");
                report.TempletFileName = Application.StartupPath + (WorkCurveHelper.DetectionType == 0 ? "\\HistoryExcelTemplate\\AutoSelfCheckReportCN.xls" : "\\HistoryExcelTemplate\\AutoSelfCheckReportCN2.xls");
                report.ReportPath = WorkCurveHelper.ExcelPath;
                report.SelfCheckReport(reportName, true, SelfCheckObject);
                if (!File.Exists(report.ReportPath + reportName + ".xls"))
                    return "";
                else
                    return report.ReportPath + reportName + ".xls";
            }
            catch
            {
                return "";
            }
        }

        public virtual void Exist()
        {
            this.refreshFillinof.Exist();
        }

        public virtual void LogOut()
        {
            this.refreshFillinof.LogOut();
        }


        public virtual void SaveSreenShot(bool isSave, string fileName)
        {
            var chat = this.XrfChart;
            var bitmap = new Bitmap(chat.Width, chat.Height);
            bool b1 = chat.IsShowHScrollBar;
            bool b2 = chat.IsShowVScrollBar;
            chat.IsShowHScrollBar = false;
            chat.IsShowVScrollBar = false;
            chat.DrawToBitmap(bitmap, chat.Bounds);
            chat.IsShowHScrollBar = b1;
            chat.IsShowVScrollBar = b2;

            string sFolderPath = Application.StartupPath + "\\Screenshots";
            if (!Directory.Exists(sFolderPath)) Directory.CreateDirectory(sFolderPath);
            string imgPath = sFolderPath + "\\" + fileName + ".jpg";

            //byte[] bytes = null;




            if (isSave)
            {
                //保存位图
                if (bitmap != null)
                {
                    bitmap.Save(imgPath, ImageFormat.Jpeg);
                }
            }
        }

        /// <summary>
        /// 截屏功能
        /// </summary>
        /// <param name="isSave"></param>
        public virtual void Screenshots(bool isSave)
        {
            ////屏幕宽
            //int iWidth = Screen.PrimaryScreen.Bounds.Width;
            ////屏幕高
            //int iHeight = Screen.PrimaryScreen.Bounds.Height;
            ////按照屏幕宽高创建位图
            //Image img = new Bitmap(iWidth, iHeight);
            ////从一个继承自Image类的对象中创建Graphics对象
            //Graphics gc = Graphics.FromImage(img);
            ////抓屏并拷贝到myimage里
            //gc.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(iWidth, iHeight));


            //string sFolderPath = Application.StartupPath + "\\Screenshots";
            //if (!Directory.Exists(sFolderPath)) Directory.CreateDirectory(sFolderPath);
            //string imgPath = sFolderPath + "\\" + Guid.NewGuid().ToString() + ".jpg";


            var chat = this.XrfChart;
            var bitmap = new Bitmap(chat.Width, chat.Height);
            bool b1 = chat.IsShowHScrollBar;
            bool b2 = chat.IsShowVScrollBar;
            chat.IsShowHScrollBar = false;
            chat.IsShowVScrollBar = false;
            chat.DrawToBitmap(bitmap, chat.Bounds);
            chat.IsShowHScrollBar = b1;
            chat.IsShowVScrollBar = b2;

            string sFolderPath = Application.StartupPath + "\\Screenshots";
            if (!Directory.Exists(sFolderPath)) Directory.CreateDirectory(sFolderPath);
            string imgPath = sFolderPath + "\\" + Guid.NewGuid().ToString() + ".jpg";

            //byte[] bytes = null;




            if (isSave)
            {
                //保存位图
                if (bitmap != null)
                {
                    bitmap.Save(imgPath, ImageFormat.Jpeg);
                }

                //img.Save(imgPath);
                if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenImageOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Help.ShowHelp(null, imgPath);
                }
            }
            else
            {

                printImage = bitmap;
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler
                   (this.pd_PrintPage);
                pd.Print();

                File.Delete(imgPath);
            }
        }
        private Image printImage;
        private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            System.Drawing.Image image = printImage;
            int x = e.MarginBounds.X;
            int y = e.MarginBounds.Y;
            int width = image.Width;
            int height = image.Height;
            if ((width / e.MarginBounds.Width) > (height / e.MarginBounds.Height))
            {
                width = e.MarginBounds.Width;
                height = image.Height * e.MarginBounds.Width / image.Width;
            }
            else
            {
                height = e.MarginBounds.Height;
                width = image.Width * e.MarginBounds.Height / image.Height;
            }
            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, width, height);
            e.Graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel);
        }
        int iTemplateType = 0;
        [System.Diagnostics.Conditional("Test")]
        public void ReLoadExcelTempalteParams()
        {
            if (System.Diagnostics.Debugger.IsAttached && iTemplateType < 12)
            {
                ReportTemplateHelper.ExcelModeType = iTemplateType;
                ExcelTemplateParams.LoadExcelTemplateParams(Application.StartupPath + "\\AppParams.xml");
                ExcelTemplateParams.GetExcelTemplateParams();
            }
        }
        [System.Diagnostics.Conditional("Test")]
        private void AddTemplat(List<TreeNodeInfo> lsit)
        {

            if (System.Diagnostics.Debugger.IsAttached && iTemplateType < 12)
            {
                iTemplateType++;
                PrintExcel(lsit);
            }
            else
            {
                iTemplateType = 0;
                return;
            }
        }
        public virtual void PrintExcel(List<TreeNodeInfo> lsit)
        {
            //if (WorkCurveHelper.PrinterType == 1)
            //{
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(PrinterBlueThread), null);
            //    return;
            //}

            ReLoadExcelTempalteParams();
            //判断打印机是否存在
            if (!PrintHelper.IsPrinterExist())
            {
                SkyrayMsgBox.Show(PrintInfo.NoPrinter);
                return;
            }
            if (WorkCurveHelper.WorkCurveCurrent == null || WorkCurveHelper.WorkCurveCurrent.ElementList == null) return;
            string valid = GenerateGenericReport(WorkCurveHelper.WorkCurveCurrent.ElementList, recordList);
            if (!string.IsNullOrEmpty(valid))
            {
                PrintThread(valid);
                return;
            }

            if (ReportTemplateHelper.ExcelModeType == 0)
            {
                if (EDXRFHelper.LoadLoadSourceEvent())
                    EDXRFHelper.DirectPrint();
                else
                {
                    Msg.Show(Info.NoLoadSource);
                }
            }
            else if (ReportTemplateHelper.ExcelModeType == 2)
            {
                if (InterfaceClass.SetPrintTemplate(null, null))
                    EDXRFHelper.NewDirectPrint(InterfaceClass.seledataFountain);
                //EDXRFHelper.DirectPrint();
                else
                {
                    Msg.Show(Info.NoLoadSource);
                }
            }
            else if (ReportTemplateHelper.ExcelModeType == 6 && DifferenceDevice.IsAnalyser/*&&System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Contains("Skyray.India")*/)
            {


                if (DifferenceDevice.interClassMain.reportThreadManage == null || DifferenceDevice.interClassMain.recordList == null ||
                    DifferenceDevice.interClassMain.recordList.Count == 0) return;

                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisid in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisid);

                string SaveReportPath = DifferenceDevice.interClassMain.reportThreadManage.GetHistoryRecordReport(hisRecordidList, 0, true, false);


                if (SaveReportPath == "") return;

                //DirectPrintLibcs.DirectPrint(SaveReportPath);

                //PrintExcelByPath(SaveReportPath);
                PrintThread(SaveReportPath);

            }
            else if (ReportTemplateHelper.ExcelModeType == 7)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisid in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisid);

                string excelPath = BrassReport(hisRecordidList);
                if (excelPath == "") return;
                //DirectPrintLibcs.DirectPrint(excelPath);
                //PrintExcelByPath(excelPath);
                PrintThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 8)
            {
                List<long> hisRecordidList = new List<long>();
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);
                if (hisRecordidList.Count == 0) return;
                string excelPath = GetXRFRecordReport(hisRecordidList);
                if (excelPath == "") return;
                PrintThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 9 || ReportTemplateHelper.ExcelModeType == 21 || ReportTemplateHelper.ExcelModeType == 24)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisid in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisid);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetHistoryRecordReport(true, hisRecordidList);
                if (excelPath.Length == 0) return;
                //PrintExcelByPath(excelPath);
                PrintThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 11)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisid in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisid);
                if (hisRecordidList.Count == 0) return;
                GetHistoryRecordReport(hisRecordidList, true);
            }

            else if (ReportTemplateHelper.ExcelModeType == 12)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisid in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisid);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetThickHistoryRecordReport(hisRecordidList);
                if (excelPath.Length == 0) return;
                //PrintExcelByPath(excelPath);
                PrintThread(excelPath);
            }
            //else if (ReportTemplateHelper.ExcelModeType == 13 && DifferenceDevice.IsAnalyser)//贵重金属新模板
            //{
            //    if (DifferenceDevice.interClassMain.reportThreadManage == null || DifferenceDevice.interClassMain.recordList == null ||
            //        DifferenceDevice.interClassMain.recordList.Count == 0) return;

            //    List<long> hisRecordidList = new List<long>();

            //    foreach (long hisid in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisid);
            //    string SaveReportPath = DifferenceDevice.interClassMain.reportThreadManage.GetHistoryRecordReport(hisRecordidList, 0, true, false);

            //    if (SaveReportPath == "") return;

            //    PrintThread(SaveReportPath);

            //}
            else if (ReportTemplateHelper.ExcelModeType == 13 || ReportTemplateHelper.ExcelModeType == 14)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisid in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisid);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetHistoryRecordTailorReport(hisRecordidList);
                if (excelPath.Length == 0) return;
                //PrintExcelByPath(excelPath);
                PrintThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 15 && DifferenceDevice.IsAnalyser)
            {

                if (DifferenceDevice.interClassMain.reportThreadManage == null || DifferenceDevice.interClassMain.recordList == null ||
                    DifferenceDevice.interClassMain.recordList.Count == 0) return;

                List<long> hisRecordidList = new List<long>();

                foreach (long hisid in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisid);

                string SaveReportPath = DifferenceDevice.interClassMain.reportThreadManage.GetHistoryRecordReport(hisRecordidList, 0, true, false);


                if (SaveReportPath == "") return;

                PrintThread(SaveReportPath);

            }
            else if (ReportTemplateHelper.ExcelModeType == 16 || ReportTemplateHelper.ExcelModeType == 20)
            {
                List<long> hisRecordidList = new List<long>();
                //foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);
                foreach (long hisid in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisid);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetHistoryRecordReport(true, hisRecordidList);
                if (excelPath.Length == 0) return;
                //PrintExcelByPath(excelPath);
                PrintThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 26)
            {
                List<long> hisRecordidList = new List<long>();
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(hisId);

                if (hisRecordidList.Count == 0) return;

                string excelPath = GetGSHistoryRecord(hisRecordidList);
                if (excelPath == string.Empty || excelPath.Length == 0) return;
                OpenPathThread(excelPath);
            }
            else if (ReportTemplateHelper.ExcelModeType == 30 || ReportTemplateHelper.ExcelModeType == 31)
            {

                var hrList = new List<long>();
                foreach (long hisId in DifferenceDevice.interClassMain.recordList) hrList.Add(hisId);
                if (hrList.Count == 0) return;
                GetBengalHistoryRecord(hrList, true);
            }
            else
            {
                this.refreshFillinof.PrintExcel();
            }
            AddTemplat(lsit);
            #region 备份
            //if (!ExcelTemplateParams.TemplateType)
            //{
            //    if (EDXRFHelper.LoadLoadSourceEvent())
            //        EDXRFHelper.DirectPrint();
            //    else
            //    {
            //        Msg.Show(Info.NoLoadSource);
            //    }
            //}
            //else
            //    this.refreshFillinof.PrintExcel();

            #endregion
        }

        public virtual void PrintBlueExcel()
        {
            this.refreshFillinof.PrintBlueExcel();
        }

        //public virtual void PrintBlueExcel()
        //{
        //    this.refreshFillinof.PrintBlueExcel();
        //}

        //public void PrintExcelByPath(string path)
        //{
        //    if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path) || string.Compare(new FileInfo(path).Extension, ".xls") != 0)
        //    {
        //        return;
        //    }
        //    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
        //    excel.Visible = false;
        //    Microsoft.Office.Interop.Excel.Workbook work = excel.Workbooks.Open(path, Type.Missing,
        //                                         Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        //                                         Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        //                                         Type.Missing, Type.Missing, Type.Missing);
        //    try
        //    {
        //        object vk_missing = System.Reflection.Missing.Value;
        //        object vk_visible = true;
        //        object vk_false = false;
        //        object vk_true = true;
        //        work.PrintOut(vk_missing, vk_missing, vk_missing, vk_false, vk_missing, vk_false, vk_false, vk_missing);     
        //    }
        //    catch
        //    { }
        //    finally
        //    {
        //        excel.Quit();
        //        //VkExcel.KillExcel(excel);
        //        excel = null;
        //        work = null;
        //        //System.GC.Collect();
        //    }
        //}

        //public void PrintWPSByPath(string path)
        //{
        //    if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path) || string.Compare(new FileInfo(path).Extension, ".xls") != 0)
        //    {
        //        return;
        //    }
        //    WPS.Application excel = new WPS.Application();
        //    excel.Visible = false;
        //    WPS._Workbook work = excel.Workbooks.Open(path, Type.Missing,
        //                                         Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        //                                         Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        //                                         Type.Missing);
        //    try
        //    {
        //        object vk_missing = System.Reflection.Missing.Value;
        //        object vk_visible = true;
        //        object vk_false = false;
        //        object vk_true = true;
        //        work.PrintOut(vk_missing, vk_missing, vk_missing, vk_missing, vk_missing, vk_missing, vk_missing,
        //          vk_missing, false, 0, 0, 0, 0, false, ET.ETPaperTray.etPrinterAutomaticSheetFeed, false, ET.ETPaperOrder.etPrinterDownThenOver);

        //    }
        //    catch
        //    { }
        //    finally
        //    {
        //        excel.Quit();
        //        //VkEt.KillExcel("et");
        //        excel = null;
        //        work = null;
        //        //System.GC.Collect();
        //    }
        //}

        public void PrintExcelByPath(Object path)
        {
            if (path == null) return;
            bool printerValid = PrintHelper.IsDefaultPrinterValid();
            if (!printerValid)
            {
                SkyrayMsgBox.Show(PrintInfo.PrintExceptionMessage);
                return;
            }
            Workbook wb = new Workbook();
            wb.Open(path.ToString());
            Aspose.Cells.Rendering.ImageOrPrintOptions Io = new Aspose.Cells.Rendering.ImageOrPrintOptions();
            Io.HorizontalResolution = 200;
            Io.VerticalResolution = 200;
            Io.IsCellAutoFit = true;
            Io.IsImageFitToPage = true;
            Io.ChartImageType = System.Drawing.Imaging.ImageFormat.Png;
            Io.ImageFormat = System.Drawing.Imaging.ImageFormat.Tiff;
            Io.OnePagePerSheet = false;
            Io.PrintingPage = PrintingPageType.IgnoreStyle;
            Aspose.Cells.Rendering.WorkbookRender ss = new Aspose.Cells.Rendering.WorkbookRender(wb, Io);
            System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
            string printerName = doc.PrinterSettings.PrinterName;
            ss.ToPrinter(printerName);
        }

        public bool IsShowDialog = false;
        public bool IsPopupDialog = true;
        public bool IsAverage = false;
        public bool IsConclute = false;// Add by Strong 2013-3-18


        public void ShowMeasureDialog(NaviItem item)
        {
            IsShowDialog = item.BtnDropDown.Checked = item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
            ReportTemplateHelper.SaveSpecifiedValue("ShowDialog", "IsShow", IsShowDialog ? "1" : "0");
            this.refreshFillinof.RefreshMeasureDialog();
        }

        public void PumpStartProcess(int pumpTime)
        {
            progressInfo.Maximum = this.deviceParamsList[this.deviceParamSelectIndex].VacuumTime;
            if (pumpTime > 0 && pumpTime >= progressInfo.Minimum && pumpTime <= this.progressInfo.Maximum)
            {
                progressInfo.Value = pumpTime;
            }
            progressInfo.SurplusTime = this.deviceParamsList[this.deviceParamSelectIndex].VacuumTime - pumpTime + "s";
            progressInfo.MeasureTime = this.deviceParamsList[this.deviceParamSelectIndex].VacuumTime + "s";
        }

        public void PumpEndProcess()
        {
            progressInfo.SurplusTime = this.deviceParamsList[this.deviceParamSelectIndex].PrecTime + "s";
            progressInfo.MeasureTime = this.deviceParamsList[this.deviceParamSelectIndex].PrecTime + "s";
            progressInfo.Value = 0;
        }

        public void InitProcessBar()
        {
            if (this.deviceMeasure.interfacce.Pump.Exist && this.deviceMeasure.interfacce.DeviceParam.IsVacuum && WorkCurveHelper.PumpShowProgress)
            {
                progressInfo.SurplusTime = this.deviceParamsList[this.deviceParamSelectIndex].VacuumTime + "s";
                progressInfo.MeasureTime = this.deviceParamsList[this.deviceParamSelectIndex].VacuumTime + "s";
            }
            else
            {
                progressInfo.SurplusTime = this.deviceParamsList[this.deviceParamSelectIndex].PrecTime + "s";
                progressInfo.MeasureTime = this.deviceParamsList[this.deviceParamSelectIndex].PrecTime + "s";
            }
            progressInfo.Value = 0;
        }

        public virtual void PreHeatOpenVoltageEnd()
        {
            if (this.deviceMeasure.interfacce.StopFlag)
            {
                TestStartAfterControlState(true);
                return;
            }
            int deviceTime = heatParams.FinalHeatTime;
            DeviceParameter deviceParams = DeviceParameter.New.Init("PreDeviceParams", deviceTime, heatParams.TubCurrent, heatParams.TubVoltage,
                heatParams.FilterIdx, heatParams.CollimatorIdx, heatParams.Target, false, 0, false, 0, false, 0, 0, 50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, false, false, 0, 0, 0, 0, 1, heatParams.TargetMode, heatParams.CurrentRate);
            this.deviceParamsList = new List<DeviceParameter>();
            this.deviceParamsList.Add(deviceParams);
            this.deviceParamSelectIndex = 0;
            this.deviceMeasure.interfacce.DropTime = 0;
            this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];
            this.initParams = InitParameter.New.Init(heatParams.TubVoltage, heatParams.TubCurrent,
                                                      heatParams.Gain, heatParams.FineGain, 0, 0, 1105, 0, "Ag", heatParams.FilterIdx, heatParams.CollimatorIdx, heatParams.Target, heatParams.TargetMode, heatParams.CurrentRate, "x", 1);
            this.deviceMeasure.interfacce.InitParam = this.initParams;
            this.spec = new SpecEntity();
            this.spec.IsSmooth = true;
            this.deviceMeasure.interfacce.Spec = this.spec;
            this.deviceMeasure.interfacce.ExistMagnet = WorkCurveHelper.DeviceCurrent.HasElectromagnet;
            RefreshDeviceInitialize(WorkCurveHelper.DeviceCurrent);
            this.optMode = OptMode.PreHeat;
            TestStartAfterControlState(false);
            this.deviceMeasure.interfacce.StopFlag = false;
            //更新工作曲线信息
            //progressInfo.MeasureTime = this.deviceParamsList[this.deviceParamSelectIndex].PrecTime + "s";
            //progressInfo.SurplusTime = this.deviceParamsList[this.deviceParamSelectIndex].PrecTime + "s";
            //progressInfo.Value = 0;
            InitProcessBar();
            //deviceMeasure.interfacce.SetDp5Cfg();

            WorkCurveHelper.testNum = 0;
            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
            {
                DifferenceDevice.CurCameraControl.skyrayCamera1.BackgroundImageLayout = ImageLayout.Stretch;

                Bitmap testDemoImg = DifferenceDevice.CurCameraControl.skyrayCamera1.GrabImage();

                DifferenceDevice.CurCameraControl.skyrayCamera1.BackgroundImage = testDemoImg;

                DifferenceDevice.CurCameraControl.skyrayCamera1.Stop();

                MotorOperator.MotorOperatorY1Thread((int)(-WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                WorkCurveHelper.waitMoveStop();
            }

            this.deviceMeasure.interfacce.MotorMove();
        }

        /// <summary>
        /// 暂停测试更新控件状态
        /// </summary>
        /// <param name="flag"></param>
        public virtual void TestStartPauseState(bool flag)
        {
            if (MenuLoadHelper.MenuStripCollection.Count == 0)
                return;
            if (this.XrfChart != null)
                this.XrfChart.UnSpecing = flag;
            //菜单栏及工具栏等状态变化
            ToolStripControls tools = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "Tools");
            foreach (ToolStripControls toolstrip in MenuLoadHelper.MenuStripCollection)
            {
                if (toolstrip.Postion == tools.Postion)
                    continue;
                if (toolstrip.parentStripMeauItem != null && toolstrip.parentStripMeauItem.CurrentNaviItem.Name != "Spec" && toolstrip.CurrentNaviItem.Enabled
                   && toolstrip.parentStripMeauItem.CurrentNaviItem.Name != "Quality" && toolstrip.CurrentNaviItem.EnabledControl)
                {
                    toolstrip.CurrentNaviItem.BtnDropDown.Enabled = flag;
                    toolstrip.CurrentNaviItem.MenuStripItem.Enabled = flag;
                    toolstrip.CurrentNaviItem.Btn.Visible = flag;
                    (toolstrip.CurrentNaviItem.Btn.Tag as Label).Visible = flag;
                }

                if (toolstrip.parentStripMeauItem != null && toolstrip.CurrentNaviItem.EnabledControl && toolstrip.CurrentNaviItem.Enabled && toolstrip.parentStripMeauItem.CurrentNaviItem.Name == "Spec"
                    && toolstrip.CurrentNaviItem.Name != "AddVirtualSpec" && toolstrip.preToolStripMeauItem != null)
                {
                    toolstrip.CurrentNaviItem.BtnDropDown.Enabled = false;
                    toolstrip.CurrentNaviItem.MenuStripItem.Enabled = false;
                    toolstrip.CurrentNaviItem.Btn.Visible = false;
                    (toolstrip.CurrentNaviItem.Btn.Tag as Label).Visible = false;
                }
            }
            if (WorkCurveHelper.NaviItems.Count == 0)
                return;

            //工具栏及导航栏设置
            foreach (NaviItem naviItem in WorkCurveHelper.NaviItems)
            {
                if ((naviItem.FlagType == 2 || naviItem.FunctionType == 2) && naviItem.Enabled && naviItem.EnabledControl)
                {
                    naviItem.BtnDropDown.Enabled = flag;
                    naviItem.MenuStripItem.Enabled = flag;
                    naviItem.Btn.Visible = flag;
                    (naviItem.Btn.Tag as Label).Visible = flag;
                    naviItem.ComboStrip.Enabled = flag;
                }
            }
            //if (libSerialize.containerObjTemp != null && libSerialize.clientPannel != null)
            //    libSerialize.GetNaviButtonsAll(libSerialize.containerObjTemp, 30, libSerialize.clientPannel, true);
            //EDXRFHelper.DisplayWorkCurveControls();
            NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == "TestSetting");
            item.BtnDropDown.Enabled = true;
            item = WorkCurveHelper.NaviItems.Find(w => w.Name == "StopTest");
            if (item != null && item.Enabled && item.EnabledControl)
            {
                //item.BtnDropDown.Enabled = !flag;
                item.EnabledControl = !flag;
            }
        }

        public virtual void PreheatTestState(bool flag)
        {
            if (MenuLoadHelper.MenuStripCollection.Count == 0)
                return;
            if (this.XrfChart != null)
                this.XrfChart.UnSpecing = flag;
            //菜单栏及工具栏等状态变化
            foreach (ToolStripControls toolstrip in MenuLoadHelper.MenuStripCollection)
            {
                if (toolstrip.CurrentNaviItem.EnabledControl && toolstrip.CurrentNaviItem.Enabled)
                {
                    toolstrip.CurrentNaviItem.BtnDropDown.Enabled = flag;
                    toolstrip.CurrentNaviItem.MenuStripItem.Enabled = flag;
                    toolstrip.CurrentNaviItem.Btn.Visible = flag;
                    (toolstrip.CurrentNaviItem.Btn.Tag as Label).Visible = flag;
                }
            }
            if (WorkCurveHelper.NaviItems.Count == 0)
                return;

            //工具栏及导航栏设置
            foreach (NaviItem naviItem in WorkCurveHelper.NaviItems)
            {
                if ((naviItem.FlagType == 2 || naviItem.FunctionType == 2) && naviItem.Enabled && naviItem.EnabledControl)
                {
                    naviItem.BtnDropDown.Enabled = flag;
                    naviItem.MenuStripItem.Enabled = flag;
                    naviItem.Btn.Visible = flag;
                    (naviItem.Btn.Tag as Label).Visible = flag;
                    naviItem.ComboStrip.Enabled = flag;
                }
            }
            //if (libSerialize.containerObjTemp != null && libSerialize.clientPannel != null)
            //    libSerialize.GetNaviButtonsAll(libSerialize.containerObjTemp, 30, libSerialize.clientPannel, true);
            //EDXRFHelper.DisplayWorkCurveControls();

            NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == "TestSetting");
            if (item != null)
            {
                if (flag)
                {
                    item.Image = Properties.Resources.StartTest;
                    item.excuteRequire = null;
                    item.Text = Info.Start;
                    item.TT = this.uc.CreateNewSpec;
                }
            }
            item = WorkCurveHelper.NaviItems.Find(w => w.Name == "StopTest");
            if (item != null && item.Enabled)
            {
                //item.BtnDropDown.Enabled = !flag;
                item.EnabledControl = !flag;
            }
        }

        /// <summary>
        /// 测量开始及结束后界面中的控件状态
        /// <param name="flag">标记状态</param>
        /// </summary>
        public virtual void TestStartAfterControlState(bool flag)
        {
            startTest = !flag;

            if (OnSateChanged != null)
            {
                OnSateChanged(null, new BoolEventArgs(flag));
                // return;
            }
            if (optMode == OptMode.PreHeat)
            {
                PreheatTestState(flag);
            }
            else
            {
                StateChange(flag);
                //if (flag)
                //    startTest = true;
                NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == "TestSetting");
                if (item != null)
                {
                    if (flag)
                    {
                        item.Image = Properties.Resources.StartTest;
                        item.excuteRequire = null;
                        item.Text = Info.Start;
                        item.TT = this.uc.CreateNewSpec;
                        item.BtnDropDown.AutoToolTip = true;
                    }
                    else
                    {
                        item.BtnDropDown.AutoToolTip = false;
                        item.BtnDropDown.ToolTipText = "";
                    }
                }
                item = WorkCurveHelper.NaviItems.Find(w => w.Name == "StopTest");
                if (item != null && item.Enabled)
                {
                    //item.BtnDropDown.Enabled = !flag;
                    item.EnabledControl = !flag;
                    if (flag)
                    {
                        item.BtnDropDown.AutoToolTip = false;
                        item.BtnDropDown.ToolTipText = "";
                    }
                    else
                    {
                        item.BtnDropDown.AutoToolTip = true;
                    }
                }
                item = WorkCurveHelper.NaviItems.Find(w => w.Name == "ChangeSampleInfo");
                if (item != null && item.Enabled)
                {
                    //item.BtnDropDown.Enabled = !flag;
                    item.EnabledControl = !flag;
                    if (flag)
                    {
                        item.BtnDropDown.AutoToolTip = false;
                        item.BtnDropDown.ToolTipText = "";
                    }
                    else
                    {
                        item.BtnDropDown.AutoToolTip = true;
                    }
                }

                item = WorkCurveHelper.NaviItems.Find(w => w.Name == "programMode");
                item.Enabled = true;
                item.EnabledControl = true;

            }
            if (UCComponentMotor.zMotor != null)
                UCComponentMotor.zMotor.Enable = flag;
        }

        //private Thread netConnect = null;

        public virtual void FinishFocusState(bool flag)
        {


        }

        public void ConnectDevice()
        {
            if (deviceMeasure.interfacce.port != null && !this.deviceMeasure.interfacce.port.ConnectState && this.deviceMeasure.interfacce != null && this.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
            {
                //netConnect = new Thread(new ThreadStart(ThreadFunStart));
                //netConnect.Start();
                if (ActionBeforeConnectStart != null)
                    ActionBeforeConnectStart();
                ThreadFunStart();
            }
            else if (deviceMeasure.interfacce.port != null
                && this.deviceMeasure.interfacce != null
                && this.deviceMeasure.interfacce.port != null
                && this.deviceMeasure.interfacce.dp5Device != null
                && WorkCurveHelper.DeviceCurrent.ComType == ComType.USB
                && WorkCurveHelper.DeviceCurrent.IsDP5
                && WorkCurveHelper.DeviceCurrent.Dp5Version == Dp5Version.Dp5_FastNet
                && !this.deviceMeasure.interfacce.dp5Device.IsConnected())
            {
                //netConnect = new Thread(new ThreadStart(ThreadFunStart));
                //netConnect.Start();
                if (ActionBeforeConnectStart != null)
                    ActionBeforeConnectStart();
                ThreadFunStart();
            }
            if (deviceMeasure.interfacce != null)
            {
                deviceMeasure.interfacce.CloseDevice();
            }
        }


        public void SetSurfaceSource()
        {
            if (this.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
            {
                SurfaceSourceLight sour = SurfaceSourceLight.FindAll()[0];
                deviceMeasure.interfacce.port.SetSurfaceSource(sour.FirstLight, sour.SecondLight, sour.ThirdLight, sour.FourthLight);
            }
        }

        private void ThreadFunStart()
        {
            bool connectState = this.deviceMeasure.interfacce.port.Connect();
            if (WorkCurveHelper.DeviceCurrent.IsDP5 && WorkCurveHelper.DeviceCurrent.Dp5Version == Dp5Version.Dp5_FastNet)
            {
                connectState = WorkCurveHelper.deviceMeasure.interfacce.dp5Device.ConnectDevice(WorkCurveHelper.DeviceCurrent.FPGAParams.IP, "10001");
            }

            if (connectState)
            {
                MessageFormat message = new MessageFormat(Info.DeviceConnecting, 0);
                WorkCurveHelper.specMessage.localMesage.Add(message);
            }
            else
            {
                MessageFormat message = new MessageFormat(Info.NoDeviceConnect, 0);
                WorkCurveHelper.specMessage.localMesage.Add(message);
            }
        }

        public bool IPSettings(string IP, string SubNet, string GateWay, string DNS)
        {
            this.deviceMeasure.interfacce.port.IPSettings(IP, SubNet, GateWay, DNS);
            return true;
        }

        public bool SetSurfaceSource(ushort firstLight, ushort secondLight, ushort thirdLight, ushort fourthLight)
        {
            return deviceMeasure.interfacce.port.SetSurfaceSource(firstLight, secondLight, thirdLight, fourthLight);
        }

        /// <summary>
        /// 打开测量相应的逻辑处理
        /// </summary>
        /// <param name="workCurve">当前工作曲线</param>
        /// <param name="testDeviceParams">传递过来的参数</param>
        public void SubmitMeasureTest(WorkCurve workCurve, TestDevicePassedParams testDeviceParams)
        {
            //if (deviceMeasure.interfacce.State != DeviceState.Idel)
            //{
            //    Msg.Show(Info.DeviceNotExistsState);
            //    return;
            //}
            deviceMeasure.interfacce.State = DeviceState.Motoring;
            deviceMeasure.interfacce.InitParam = workCurve.Condition.InitParam;
            deviceMeasure.interfacce.DeviceParam = workCurve.Condition.DeviceParamList[0];
            deviceMeasure.interfacce.SetDp5Cfg();
            switch (testDeviceParams.SpecType)
            {
                case SpecType.UnKownSpec:  //未知谱扫描

                    MeasureUnkownSpecProcess(workCurve, testDeviceParams);
                    break;
                case SpecType.PureSpec:  //纯元素谱扫描
                    MeasurePureElementProcess(workCurve, testDeviceParams);
                    break;
                case SpecType.StandSpec://标样扫描
                    MeasureStandSample(workCurve, testDeviceParams);
                    break;
                default:
                    break;
            }
        }



        public abstract void MeasureStandSample(WorkCurve workCurve, TestDevicePassedParams testDeviceParams);
        public abstract void MeasurePureElementProcess(WorkCurve workCurve, TestDevicePassedParams testDeviceParams);

        /// <summary>
        /// 匹配及智能测试参数初始化
        /// </summary>
        /// <param name="conditionType"></param>
        public void StartExceptTestModeInitialize(ConditionType type, TestDevicePassedParams testDeviceParams)
        {
            Condition condition = Condition.FindOne(w => w.Type == type && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id); //操作指定类型的条件
            if (condition == null)
            {
                Msg.Show(Info.SpecifiedConditionNoExits);
                return;
            }
            this.initParams = condition.InitParam;
            if (WorkCurveHelper.WorkCurveCurrent != null)
            {
                this.initParams.Gain = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.Gain;
                this.initParams.FineGain = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.FineGain;
            }
            this.deviceParamsList = condition.DeviceParamList.ToList();
            if (this.deviceParamsList.Count == 0)
            {
                Msg.Show(Info.MeasureConditionInvalidate);
                return;
            }
            DeviceMeasurePassingInitialize(testDeviceParams, true);
        }

        /// <summary>
        /// 正常模式测试参数初始化
        /// </summary>
        /// <param name="workCurve"></param>
        /// <param name="testDeviceParams"></param>
        public void StartTestModeInitialize(WorkCurve workCurve, TestDevicePassedParams testDeviceParams)
        {
            List<DeviceParameter> listParams = workCurve.Condition.DeviceParamList.ToList();
            if (workCurve.ElementList != null && workCurve.ElementList.Items.Count > 0)
                DeviceParameterByElementList(listParams);
            else
                this.deviceParamsList = listParams;
            if (this.deviceParamsList.Count == 0)
            {
                Msg.Show(Info.MeasureConditionInvalidate);
                return;
            }
            this.initParams = workCurve.Condition.InitParam;
            DeviceMeasurePassingInitialize(testDeviceParams, true);
        }

        public void RefreshDeviceInitialize(Device device)
        {
            deviceMeasure.interfacce.ChamberMotor.Exist = device.HasChamber;
            deviceMeasure.interfacce.ChamberMotor.ID = device.ChamberElectricalCode;
            deviceMeasure.interfacce.ChamberMotor.DirectionFlag = device.ChamberElectricalDirect;
            deviceMeasure.interfacce.CollimatMotor.Exist = device.HasCollimator;
            deviceMeasure.interfacce.CollimatMotor.ID = device.CollimatorElectricalCode;
            deviceMeasure.interfacce.CollimatMotor.DirectionFlag = device.CollimatorElectricalDirect;
            deviceMeasure.interfacce.FilterMotor.Exist = device.HasFilter;
            deviceMeasure.interfacce.FilterMotor.DirectionFlag = device.FilterElectricalDirect;
            deviceMeasure.interfacce.FilterMotor.ID = device.FilterElectricalCode;
            deviceMeasure.interfacce.TargetMotor.Exist = device.HasTarget;
            deviceMeasure.interfacce.TargetMotor.ID = device.TargetElectricalCode;
            deviceMeasure.interfacce.TargetMotor.DirectionFlag = device.TargetElectricalDirect;

            //判断设备是否含有振动器
            if (device.HasCollimator)
            {
                if (deviceMeasure.interfacce.CollimatMotor.Target != null && deviceMeasure.interfacce.CollimatMotor.Target.Length > 0)
                    deviceMeasure.interfacce.CollimatMotor.Target = null;
                List<Collimator> listColli = device.Collimators.ToList();
                deviceMeasure.interfacce.CollimatMotor.Target = new int[listColli.Count];
                for (int i = 0; i < listColli.Count; i++)
                {
                    deviceMeasure.interfacce.CollimatMotor.Target[i] = listColli[i].Step;
                }
            }
            //判断设备是否含有过滤器
            if (device.HasFilter)
            {
                if (deviceMeasure.interfacce.FilterMotor.Target != null && deviceMeasure.interfacce.FilterMotor.Target.Length > 0)
                    deviceMeasure.interfacce.FilterMotor.Target = null;
                List<Filter> listColli = device.Filter.ToList();
                deviceMeasure.interfacce.FilterMotor.Target = new int[listColli.Count];
                for (int i = 0; i < listColli.Count; i++)
                {
                    deviceMeasure.interfacce.FilterMotor.Target[i] = listColli[i].Step;
                }
            }
            //含有靶材
            if (device.HasTarget)
            {
                if (deviceMeasure.interfacce.TargetMotor.Target != null && deviceMeasure.interfacce.TargetMotor.Target.Length > 0)
                    deviceMeasure.interfacce.TargetMotor.Target = null;
                List<Target> listTar = device.Target.ToList();
                deviceMeasure.interfacce.TargetMotor.Target = new int[listTar.Count];
                for (int i = 0; i < listTar.Count; i++)
                {
                    deviceMeasure.interfacce.TargetMotor.Target[i] = listTar[i].Step;
                }
            }
        }



        protected void NewReplaceDeleteData()
        {
            if (!IsIncrement)
            {
                int specType = 0;
                bool exist = WorkCurveHelper.DataAccess.ExistsRecord(this.specList.Name, out specType);
                if (exist)
                {
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery("delete from historycompanyotherinfo where history_id in (select id from historyrecord where speclistName in ('" + this.specList.Name + "'));" +
                                       "delete from historyelement where historyrecord_id in (select id from historyrecord where speclistName in ('" + this.specList.Name + "'));" +
                                       "delete from historyrecord where speclistName in ('" + this.specList.Name + "')");
                    WorkCurveHelper.DataAccess.DeleteRecord(this.specList.Name);
                }
            }
        }


        public bool TempIsKey = false;


        public bool SpeclistNameValidate()
        {
            //判断数据库中是否存在指定的样品名称
            int specType = 0;
            bool exist = WorkCurveHelper.DataAccess.ExistsRecord(this.specList.Name, out specType);

            if (exist && (testDevicePassedParams.SpecType == SpecType.PureSpec || testDevicePassedParams.SpecType == SpecType.StandSpec))
            {

                var result = ((WorkCurveHelper.DirectRun.IsDirectRun && TempIsKey) || (WorkCurveHelper.IsBatchTest)) ? DialogResult.Yes : Msg.Show(Info.strCoverSpecName, Info.Suggestion, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    IsIncrement = true;
                    if (WorkCurveHelper.testNum == 1)
                    {
                        MotorOperator.MotorOperatorY1Thread((int)(-WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                    }
                }
                else if (result == DialogResult.No)
                {

                    UCPWDLock uc = new UCPWDLock(true);
                    WorkCurveHelper.OpenUC(uc, false, Info.PWDLock, true);
                    if (uc.DialogResult == DialogResult.No)
                    {
                        return false;
                    }

                    IsIncrement = false;
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery("delete from historycompanyotherinfo where history_id in (select id from historyrecord where speclistName in ('" + this.specList.Name + "'));" +
                                      "delete from historyelement where historyrecord_id in (select id from historyrecord where speclistName in ('" + this.specList.Name + "'));" +
                                      "delete from historyrecord where speclistName in ('" + this.specList.Name + "')");
                    WorkCurveHelper.DataAccess.DeleteRecord(this.specList.Name);

                    if (WorkCurveHelper.testNum == 1)
                    {
                        MotorOperator.MotorOperatorY1Thread((int)(-WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                    }
                }
                else
                {
                    deviceMeasure.interfacce.State = DeviceState.Idel;
                    deviceMeasure.interfacce.connect = DeviceConnect.DisConnect;
                    DifferenceDevice.IsConnect = false;
                    DifferenceDevice.CurCameraControl.skyrayCamera1.Open();
                    return false;
                }
            }
            else
            {
                if (WorkCurveHelper.testNum == 1)
                {
                    MotorOperator.MotorOperatorY1Thread((int)(-WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                }
            }

            return true;
        }


        public bool NoLoadDeviceInitialize(TestDevicePassedParams testDeviceParams, Device device)
        {
            if (WorkCurveHelper.DeviceCurrent == null)
                return false;
            this.specList = new SpecListEntity(CurveTest.Spec.Name, CurveTest.Spec.SampleName, CurveTest.Spec.Height, CurveTest.Spec.CalcAngleHeight, CurveTest.Spec.Supplier,
                CurveTest.Spec.Weight, CurveTest.Spec.Shape, CurveTest.Spec.Operater, DateTime.Now, CurveTest.Spec.SpecSummary, testDeviceParams.SpecType, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb());
            this.specList.Loss = CurveTest.Spec.Loss;
            //this.specList.Condition = initParams.Condition;
            this.testDevicePassedParams = testDeviceParams;
            this.deviceParamSelectIndex = 0;
            this.deviceMeasure.interfacce.PauseStop = false;
            this.spec = new SpecEntity();
            this.spec.IsSmooth = true;
            this.spec.RemarkInfo = testDeviceParams.RemarkInformation;
            if (this.testDevicePassedParams.MeasureParams.MeasureNumber > 1)
                this.ExcuteFlag = true;
            else
                this.ExcuteFlag = false;
            this.currentTestTimes = 1;
            this.deviceMeasure.interfacce.Spec = this.spec;
            this.spec.DeviceParameter = this.deviceParamsList[deviceParamSelectIndex].ConvertFrom();
            this.demacateMode = Demarcate.None;
            this.specList.SpecDate = DateTime.Now;
            this.FirstDeviceParamsList.Clear();
            WorkCurveHelper.SelectSpectrumPath = "";
            WorkCurveHelper.EditionType = TotalEditionType.Default;
            IsIncrement = false;
            string constStr = GetDefineSpectrumName(specList.SampleName, testDevicePassedParams.IsRuleName);
            this.specList.Name = constStr;
            if (!SpeclistNameValidate())
            {
                deviceMeasure.interfacce.State = DeviceState.Idel;
                return false;
            }
            //if (WorkCurveHelper.DirectRun.IsKeyCall)//清除键盘调用标记 判断弹出名称冲突窗口后清除
            //    WorkCurveHelper.DirectRun.IsKeyCall = false; 
            this.refreshFillinof.UpdateWorkSpec(this.deviceParamsList[this.deviceParamSelectIndex], this.specList);
            //更新工作曲线信息
            this.refreshFillinof.RefreshCurve(this.initParams, this.deviceParamsList[this.deviceParamSelectIndex]);
            #region 多条件处理
            TempNetDataSmooth.Clear();
            List<int[]> temp = new List<int[]>();
            TempNetDataSmooth.Add(this.deviceParamsList[this.deviceParamSelectIndex].Id, temp);
            #endregion
            //判断数据库中是否存在指定的谱名称
            deviceMeasure.interfacce.InitParam = this.initParams;
            deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[0];
            this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
            this.specList.Specs[0] = this.spec;
            recordList.Clear();
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().ForEach(w => { w.CumulativeValue = 0; });
            int dt = -1;
            if (!this.currentDeviceParamsList.TryGetValue(deviceMeasure.interfacce.DeviceParam.Id, out dt))
                this.currentDeviceParamsList.Add(deviceMeasure.interfacce.DeviceParam.Id, deviceMeasure.interfacce.DeviceParam.TubCurrent);
            deviceMeasure.interfacce.StopFlag = false;
            deviceMeasure.interfacce.Pump.Exist = device.HasVacuumPump;
            deviceMeasure.interfacce.ExistMagnet = device.HasElectromagnet;
            deviceMeasure.interfacce.DropTime = testDeviceParams.MeasureParams.DiscardTime;
            deviceMeasure.interfacce.RecordDropTime = testDeviceParams.MeasureParams.DiscardTime;
            InitProcessBar();
            TestStartAfterControlState(false); //使相应的界面控件变灰
            return true;
        }


        //public List<HistoryRecord> recordList = new List<HistoryRecord>();
        public List<long> recordList = new List<long>();


        /// <summary>
        /// 设备初始化变量，便于以后测试结果。
        /// </summary>
        /// <param name="workCurve"></param>
        /// <param name="testDeviceParams"></param>
        public void DeviceMeasurePassingInitialize(TestDevicePassedParams testDeviceParams, bool flag)
        {
            Device device = WorkCurveHelper.DeviceCurrent;
            if (!NoLoadDeviceInitialize(testDeviceParams, device)) return;
            this.deviceMeasure.interfacce.StopFlag = false;
            //对测量设备进行相应的初始化
            RefreshDeviceInitialize(device);
            //判断设备是否含有样品腔
            if (device.HasChamber && testDeviceParams.WordCureTestList != null && testDeviceParams.WordCureTestList.Count > 0)
            {
                int chamberCount = device.Chamber.ToList().Count;
                deviceMeasure.interfacce.CellStates = new ChamberCellState[chamberCount];
                List<Chamber> chamberList = device.Chamber.ToList();
                deviceMeasure.interfacce.ChamberMotor.Target = new int[chamberCount];
                for (int i = 0; i < testDeviceParams.WordCureTestList.Count; i++)
                {
                    Chamber tempChamber = chamberList.Find(delegate(Chamber cc) { return cc.Num == int.Parse(testDeviceParams.WordCureTestList[i].SerialNumber); });
                    if (tempChamber != null)
                    {
                        deviceMeasure.interfacce.CellStates[tempChamber.Num - 1] = ChamberCellState.Waitting;
                        deviceMeasure.interfacce.ChamberMotor.Target[tempChamber.Num - 1] = tempChamber.Step;
                    }
                }
                int index = deviceMeasure.interfacce.GetCellIndex(ChamberCellState.Waitting);
                if (index < 0)
                    return;
                deviceMeasure.interfacce.CellStates[index] = ChamberCellState.Testing;

                deviceMeasure.interfacce.MotorMove(index + 1);


            }
            else
            {
                deviceMeasure.interfacce.MotorMove();
            }

        }

        /// <summary>
        /// 当前曲线下存在感兴趣元素，对当前的测量条件进行抽取
        /// </summary>
        /// <param name="listParams">当前条件下的小测量条件</param>
        public void DeviceParameterByElementList(List<DeviceParameter> listParams)
        {
            this.deviceParamsList = listParams;

            //if (WorkCurveHelper.WorkCurveCurrent.ElementList == null || WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count == 0)
            //    return;
            //List<CurveElement> listCurveElement = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList();
            //if (listCurveElement.Count == 0)
            //    return;
            //this.deviceParamsList = new List<DeviceParameter>();
            //foreach (DeviceParameter deviceParams in listParams)
            //{
            //    CurveElement curveElement = listCurveElement.Find(w => w.DevParamId == deviceParams.Id);
            //    DeviceParameter tempParamster = this.deviceParamsList.Find(w => w.Name == deviceParams.Name);
            //    if (tempParamster == null && curveElement != null)
            //        this.deviceParamsList.Add(deviceParams);
            //}
        }

        /// <summary>
        /// 电机停止移动处理函数
        /// </summary>
        /// <param name="optMode"></param>
        public void MotorStop(OptMode optMode)
        {
            deviceMeasure.interfacce.BeginSound();
            int id = this.deviceParamsList.Count > this.deviceParamSelectIndex && this.deviceParamsList[this.deviceParamSelectIndex].CollimatorIdx > 0 ? this.deviceParamsList[this.deviceParamSelectIndex].CollimatorIdx : 1;
            if (skyrayCamera != null)
            {
                //edit by chuyaqin 2011-07-23 光斑设置从文件读取的值
                //skyrayCamera.SetCameralFocus(id);
                skyrayCamera.FociIndex = id - 1;
            }
            switch (optMode)//电机移动结束
            {
                case OptMode.Test://测试
                    deviceMeasure.Test();
                    break;
                case OptMode.Initalize://初始化
                    deviceMeasure.Initialize();
                    break;
                case OptMode.Demarcate://能量刻度
                case OptMode.CalFPGAIntercept:
                    deviceMeasure.Test();
                    break;
                case OptMode.StandSample: //标样
                    deviceMeasure.Test();
                    break;
                case OptMode.SpecialOnlySave:
                    deviceMeasure.Test();
                    break;
                case OptMode.PureSample:
                    deviceMeasure.Test();
                    break;
                case OptMode.UnknownSave:
                    deviceMeasure.Test();
                    break;
                case OptMode.PreHeat:
                    deviceMeasure.Test();
                    break;
                case OptMode.Resolve:
                    deviceMeasure.Test();
                    break;
                case OptMode.Detection:
                    deviceMeasure.Test();
                    break;
                default: break;
            }
        }


        public void RefreshOptModeAferMathching()
        {
            switch (this.testDevicePassedParams.SpecType)
            {
                case SpecType.UnKownSpec:  //未知谱扫描
                    //this.optMode = OptMode.Test;
                    break;
                case SpecType.PureSpec:  //纯元素谱扫描
                    this.optMode = OptMode.PureSample;
                    break;
                case SpecType.StandSpec://标样扫描
                    this.optMode = OptMode.StandSample;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///扫描终止共同处理函数
        /// </summary>
        /// <param name="optMode">当前工作模式</param>
        /// <param name="usedTime">消耗的时间</param>
        /// <param name="workCurve">当前工作曲线</param>
        public void TerminateTest(OptMode optMode, int usedTime, WorkCurve workCurve)
        {
            if (DifferenceDevice.interClassMain.skyrayCamera != null)
            {
                DifferenceDevice.interClassMain.skyrayCamera.FociIndex = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].CollimatorIdx - 1;
            }

            RefreshCameraText(-1, 0, false);
            if (deviceMeasure.interfacce.StopFlag)
            {
                deviceMeasure.interfacce.StopSound();
                WorkCurveHelper.IDemarcateTest = 0;
                TestStop();
                if (optMode == OptMode.Detection)
                {
                    DetectionDataNormal = false;
                    GetDetectionDetectState = true;
                }
                if (WorkCurveHelper.StopTestIsSave) StopTest(usedTime, true, optMode);
                return;
            }
            else
            {
                deviceMeasure.interfacce.EndSound();
            }

            switch (optMode)
            {
                case OptMode.Demarcate: //能量刻度
                case OptMode.CalFPGAIntercept:
                    optModeDemarcate(workCurve, optMode);
                    break;
                case OptMode.Matching:                     //修正匹配不保存谱文件bug
                case OptMode.Explore:                      //修正匹配不保存谱文件bug
                    if (WorkCurveHelper.IsDirectCaculate)                  //修正匹配不保存谱文件bug
                        optModeTest(usedTime, true, optMode);                  //修正匹配不保存谱文件bug
                    //else optModeSample(usedTime, workCurve);
                    break;
                case OptMode.Test:
                    optModeTest(usedTime, true, optMode);
                    break;
                case OptMode.StandSample: //标样
                    optModeSample(usedTime, workCurve);
                    break;
                case OptMode.PureSample:
                    optModePure(usedTime, workCurve);
                    break;
                case OptMode.SpecialOnlySave:
                    optModeTest(usedTime, false, optMode);
                    break;
                case OptMode.UnknownSave:
                    optModeTest(usedTime, false, optMode);
                    break;
                case OptMode.PreHeat:
                    TestStartAfterControlState(true);
                    if (this.deviceMeasure.interfacce != null)
                        this.deviceMeasure.interfacce.CloseDevice();
                    progressInfo.Value = 0;
                    break;
                case OptMode.Resolve:
                    if (this.deviceMeasure.interfacce != null)
                        this.deviceMeasure.interfacce.CloseDevice();
                    DemarcateEnergyHelp.CalParam(this.XrfChart.DemarcateEnergys);
                    if (!this.deviceMeasure.interfacce.StopFlag && deviceResolve != null)
                    {
                        Msg.Show(Info.Resolve + ": " + deviceResolve.CalculateResolve().ToString("f1"));
                    }
                    this.progressInfo.Value = 0;
                    int tubCurrent = 0;
                    DeviceParameter tempParams = this.deviceParamsList[this.deviceParamSelectIndex];
                    bool success = this.currentDeviceParamsList.TryGetValue(tempParams.Id, out tubCurrent);
                    if (success)
                    {
                        string sql = "Update DeviceParameter Set TubCurrent= "
                                   + tubCurrent + " Where Id = " + tempParams.Id;
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    }
                    this.currentDeviceParamsList.Clear();
                    TestStartAfterControlState(true);
                    break;
                case OptMode.Detection:
                    DetectionDetectOver();
                    break;
            }
        }

        public abstract void optModeSample(int usedTime, WorkCurve workCurve);

        public abstract void optModePure(int usedTime, WorkCurve workCurve);

        public virtual string GetSpecTypeName(string specListName)
        {
            string str = string.Empty;
            string splitStr = string.Empty;
            if (this.testDevicePassedParams != null && this.testDevicePassedParams.IsAdditionSpec)
            {
                if (this.testDevicePassedParams.SpecType == SpecType.StandSpec && this.optMode == OptMode.StandSample)
                    str = Info.SampleSpecMatch;
                else if (this.testDevicePassedParams.SpecType == SpecType.StandSpec && this.optMode == OptMode.SpecialOnlySave)
                    str = Info.StandandSample;
                splitStr = Info.NormalCondition;
            }
            else
            {
                if (this.testDevicePassedParams.SpecType == SpecType.PureSpec && this.optMode == OptMode.PureSample)
                {
                    str = Info.PureElement;
                    splitStr = Info.IntelligentCondition;
                }
                else if (this.testDevicePassedParams.SpecType == SpecType.PureSpec && this.optMode == OptMode.SpecialOnlySave)
                {
                    str = Info.PureElement;
                    splitStr = Info.NormalCondition;
                }
                else if (this.testDevicePassedParams.SpecType == SpecType.UnKownSpec)
                {
                    str = Info.Test;
                    splitStr = Info.NormalCondition;
                }
                else if (this.testDevicePassedParams.SpecType == SpecType.StandSpec && this.optMode == OptMode.SpecialOnlySave)
                {
                    str = Info.StandandSample;
                    splitStr = Info.NormalCondition;
                }
            }
            if (testDevicePassedParams != null)
                str = specListName + "_" + str + "_" + initParams.Condition.Name + "_" + WorkCurveHelper.WorkCurveCurrent.Name + "_" + splitStr;
            return str;
        }

        /// <summary>
        /// 得到用户自定义的谱文件名
        /// </summary>
        /// <param name="specListName"></param>
        /// <param name="flag">标示开始测量取名，还是在测量过程中取名</param>
        /// <returns></returns>
        public string GetDefineSpectrumName(string specListName, bool isRuleName)
        {
            string samplename = string.Empty;
            if (isRuleName)
            {
                #region ----
                string str = string.Empty;
                string splitStr = string.Empty;
                if (this.testDevicePassedParams != null && this.testDevicePassedParams.IsAdditionSpec)
                {
                    if (this.testDevicePassedParams.SpecType == SpecType.StandSpec && this.optMode == OptMode.StandSample)
                        str = Info.SampleSpecMatch;
                    else if (this.testDevicePassedParams.SpecType == SpecType.StandSpec && this.optMode == OptMode.SpecialOnlySave)
                        str = Info.StandandSample;
                    splitStr = Info.NormalCondition;
                }
                else
                {
                    if (this.testDevicePassedParams.SpecType == SpecType.PureSpec && this.optMode == OptMode.PureSample)
                    {
                        str = Info.PureElement;
                        //splitStr = Info.IntelligentCondition;
                    }
                    else if (this.testDevicePassedParams.SpecType == SpecType.PureSpec && this.optMode == OptMode.SpecialOnlySave)
                    {
                        str = Info.PureElement;
                        //splitStr = Info.NormalCondition;
                    }
                    else if (this.testDevicePassedParams.SpecType == SpecType.UnKownSpec)
                    {
                        str = Info.Test;
                        //splitStr = Info.NormalCondition;
                    }
                    else if (this.testDevicePassedParams.SpecType == SpecType.StandSpec && this.optMode == OptMode.SpecialOnlySave)
                    {
                        str = Info.StandandSample;
                        //splitStr = Info.NormalCondition;
                    }
                    if (WorkCurveHelper.NaviItems.Find(w => w.Name == "cboMode") != null)
                        splitStr = WorkCurveHelper.NaviItems.Find(w => w.Name == "cboMode").ComboStrip.SelectedIndex == 0 ? Info.NormalCondition : Info.IntelligentCondition;
                }
                #endregion
                if (testDevicePassedParams == null) return str;

                //读取用户自定义名字格式
                string otherpath = Application.StartupPath + "\\AppParams.xml";
                XElement xele = XElement.Load(otherpath);
                if (xele == null) return str;
                var names = xele.Elements("NameSetting").Elements("Name").ToList();
                try
                {
                    for (int i = 0; i < names.Count; i++)
                    {
                        if (names[i].Attribute(XName.Get("Flag")).Value.ToString() == "SpectrumName" && !string.IsNullOrEmpty(names[i].Attribute(XName.Get("Flag")).Value))
                        {
                            string[] judge = names[i].Value.ToString().Split(("_").ToCharArray());
                            if (judge != null && judge.Length > 0)
                            {
                                for (int j = 0; j < judge.Length; j++)
                                {
                                    switch (judge[j])
                                    {
                                        case "bSpecListName":
                                            samplename += string.IsNullOrEmpty(specListName) ? "" : (specListName + "_");
                                            break;
                                        case "bSpecType":
                                            //samplename += (specList.SpecType.ToString() + "_");
                                            samplename += string.IsNullOrEmpty(str) ? "" : (str + "_");
                                            break;
                                        case "bInitConditionName":
                                            //samplename += string.IsNullOrEmpty(initParams.Condition.Name) ? "" : (initParams.Condition.Name + "_");
                                            samplename += initParams.Condition.Type == ConditionType.Intelligent ? (Info.IntelligentCondition + "_") : (string.IsNullOrEmpty(initParams.Condition.Name) ? "" : (initParams.Condition.Name + "_"));
                                            break;
                                        case "bCurrentWorkCurve":
                                            //samplename += string.IsNullOrEmpty(WorkCurveHelper.WorkCurveCurrent.Name) ? "" : (WorkCurveHelper.WorkCurveCurrent.Name + "_");
                                            samplename += WorkCurveHelper.WorkCurveCurrent.Condition.Type == ConditionType.Intelligent ? (Info.Intelligent + "_") : (string.IsNullOrEmpty(WorkCurveHelper.WorkCurveCurrent.Name) ? "" : (WorkCurveHelper.WorkCurveCurrent.Name + "_"));
                                            break;
                                        case "bOptMode":
                                            //samplename += (optMode.ToString() + "_");
                                            samplename += string.IsNullOrEmpty(splitStr) ? "" : (splitStr + "_");
                                            break;
                                        case "bTestTime":
                                            samplename += (specList.SpecDate.Value.ToString("HHmmss") + "_");
                                            break;
                                        case "bTestDate":
                                            samplename += (specList.SpecDate.Value.ToString("yyyyMMdd") + "_");
                                            break;
                                        case "bWorks":
                                            samplename += (WorkCurveHelper.WorkCurveCurrent.WorkRegion.Name + "_");
                                            break;
                                        case "bCollimator":
                                            samplename += (WorkCurveHelper.WorkCurveCurrent.Condition == null
                                                            || WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList == null
                                                            || WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count <= 0
                                                            || !WorkCurveHelper.DeviceCurrent.HasCollimator
                                                            || WorkCurveHelper.DeviceCurrent.Collimators.Count < WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].CollimatorIdx ? "" : WorkCurveHelper.DeviceCurrent.Collimators[WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].CollimatorIdx - 1].Diameter.ToString() + "mm_");
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                catch
                {

                }
                finally
                {
                    if (string.IsNullOrEmpty(samplename))
                        samplename = specListName;
                    else
                    {
                        string[] strNames = samplename.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        samplename = "";
                        for (int i = 0; i < strNames.Length; i++)
                        {
                            samplename += strNames[i] + "_";
                        }
                        samplename = samplename.Remove(samplename.Length - 1);
                    }


                }
            }
            else
            {
                if (testDevicePassedParams.SpecType == SpecType.PureSpec && DifferenceDevice.IsXRF)
                {
                    specListName += "_" + WorkCurveHelper.WorkCurveCurrent.Name;
                }
                samplename = specListName;
            }

            //谱名称后缀
            int iSuffix = currentTestTimes;
            if (IsIncrement)
            {
                //string stringSql="";
                if (DifferenceDevice.IsThick && bIsCameraStartTest)
                {
                    //iCameraPointCount = this.skyrayCamera.ContiTestPoints.Count
                    //currentTestTimes < testDevicePassedParams.MeasureParams.MeasureNumber
                    //                    stringSql = @"select a.name from SpecList a inner join Condition b 
                    //                                on a.Condition_Id=b.Id inner join Device c on b.Device_Id=c.Id where 1=1 and a.Name like '" + samplename + "_%_%'and a.NameType=" + (isRuleName ? "1" : "0") + " and b.Device_Id="
                    //                                     + WorkCurveHelper.DeviceCurrent.Id;
                    //if (specList != null) stringSql += " and a.id<>" + specList.Id;
                    //var testv = EDXRFHelper.GetData(stringSql);
                    SqlParams param0 = new SqlParams("Name", samplename, true, "", "_%_%", false);
                    SqlParams param1 = new SqlParams("NameType", (isRuleName ? "1" : "0"), true);
                    List<SpecListEntity> testv = WorkCurveHelper.DataAccess.Query(new SqlParams[] { param0, param1 });
                    List<int> sNameList = new List<int>();
                    foreach (var row in testv)
                    {
                        string strName = row.Name.Replace(samplename + "_", "").Split('_')[0];
                        if (!sNameList.Contains(int.Parse(strName))) sNameList.Add(int.Parse(strName));

                    }
                    if (iCurrCameraPointCount == iCameraPointCount)
                        iSuffix = sNameList.Max() + 1;
                    else
                    {
                        iSuffix = sNameList.Max() - (testDevicePassedParams.MeasureParams.MeasureNumber - currentTestTimes);
                    }

                }
                else
                {
                    //                    stringSql = @"select a.Id from SpecList a inner join Condition b 
                    //                                on a.Condition_Id=b.Id inner join Device c on b.Device_Id=c.Id where 1=1 and a.Name like '" + specListName + "%'and a.NameType=" + (isRuleName ? "1" : "0") + " and b.Device_Id="
                    //                                     + WorkCurveHelper.DeviceCurrent.Id;
                    //                    if (specList != null) stringSql += " and a.id<>" + specList.Id;
                    //                    var testv = EDXRFHelper.GetData(stringSql);
                    //var testv = EDXRFHelper.GetData(stringSql);
                    SqlParams param0 = new SqlParams("Name", specListName, true, "", "%", false);
                    SqlParams param1 = new SqlParams("NameType", (isRuleName ? "1" : "0"), true);
                    List<SpecListEntity> testv = WorkCurveHelper.DataAccess.Query(new SqlParams[] { param0, param1 });
                    iSuffix = testv.Count + 1;
                }
            }
            if (DifferenceDevice.IsThick && bIsCameraStartTest) samplename += "_" + iSuffix;
            else
                samplename += ((iSuffix > 1) ? "_" + iSuffix : "");
            //&& testDevicePassedParams.MeasureParams.MeasureNumber>1
            return samplename;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        public const int SW_RESTORE = 9;
        //key为小条件Id,value为小条件进行的数据
        public Dictionary<long, List<int[]>> TempNetDataSmooth = new Dictionary<long, List<int[]>>();
        /// <summary>
        /// 根据条件次数计算相应的含量和统计信息结果
        /// </summary>
        /// <param name="usedTime">当前条件使用的时间</param>
        /// <param name="flag">标记域，要来区别一般扫描还是指定扫描</param>
        /// <param name="optMode">当前选择的模式</param>
        public virtual void optModeTest(int usedTime, bool flag, OptMode optMode)
        {
            //if (this.skyrayCamera == null)
            //    return;
            if (deviceParamSelectIndex < deviceParamsList.Count)
            {
                //每个测量小条件进行扫描
                if (usedTime <= 0.000001)
                    return;
                this.spec.UsedTime = this.deviceMeasure.interfacce.usedTime;
                this.spec.SpecTime = deviceParamsList[deviceParamSelectIndex].PrecTime;
                string middleStr = this.specList.SampleName + "_" + deviceParamsList[deviceParamSelectIndex].Name + "_"
                    + initParams.Condition.Name + "_" + currentTestTimes;
                if (testDevicePassedParams.RemarkInformation != null)
                    this.spec.RemarkInfo = testDevicePassedParams.RemarkInformation;
                else this.spec.RemarkInfo = "";//spec.RemarkInfo不允许为null
                this.spec.DeviceParameter = this.deviceParamsList[deviceParamSelectIndex].ConvertFrom();
                this.spec.TubCurrent = this.spec.TubCurrent > 0 ? this.spec.TubCurrent : this.spec.DeviceParameter.TubCurrent;
                this.spec.TubVoltage = this.spec.TubVoltage > 0 ? this.spec.TubVoltage : this.spec.DeviceParameter.TubVoltage;
                DeviceParameter dt = null;
                if (!this.FirstDeviceParamsList.TryGetValue(deviceParamSelectIndex, out dt))
                    this.FirstDeviceParamsList.Add(this.deviceParamSelectIndex, this.deviceMeasure.interfacce.DeviceParam);
                this.spec.Name = middleStr;
                if (SpecHelper.IsSmoothProcessData)
                {
                    List<int[]> ttOutput = new List<int[]>();
                    if (TempNetDataSmooth.TryGetValue(this.deviceParamsList[deviceParamSelectIndex].Id, out ttOutput))
                    {
                        int[] smooth = Helper.ToInts(this.spec.SpecData);
                        ttOutput.Add(smooth);
                        if (ttOutput.Count > 5)
                        {
                            ttOutput.RemoveAt(0);
                        }
                        else
                        {
                            int[] ttTemp = ttOutput[0];
                            int count = ttOutput.Count;
                            for (int m = count; m < 5; m++)
                                ttOutput.Add(ttTemp);
                        }
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < this.deviceMeasure.interfacce.backData.Length; i++)
                        {
                            int te = 0;
                            foreach (var arr in ttOutput)
                            {
                                te += arr[i];
                            }
                            int temp = (int)Math.Round((te * 1.0 / ttOutput.Count), MidpointRounding.AwayFromZero);
                            sb.Append(temp.ToString() + ",");
                        }
                        TempNetDataSmooth.Remove(this.deviceParamsList[deviceParamSelectIndex].Id);
                        TempNetDataSmooth.Add(this.deviceParamsList[deviceParamSelectIndex].Id, ttOutput);
                        this.spec.SpecData = sb.ToString();
                    }
                }
            }
            if (deviceParamSelectIndex == deviceParamsList.Count - 1)
            {
                //当前小条件进行完毕
                string conditionName = string.Empty;
                string constStr = GetDefineSpectrumName(specList.SampleName, testDevicePassedParams.IsRuleName);
                specList.Name = constStr;
                NewReplaceDeleteData();
                specList.ImageShow = true;
                //specList.Condition = initParams.Condition;
                specList.Color = DifferenceDevice.DefaultSpecColor.ToArgb();
                specList.VirtualColor = specList.Color;
                specList.SpecDate = DateTime.Now;
                specList.DeviceName = WorkCurveHelper.DeviceCurrent.Name;
                specList.WorkCurveName = WorkCurveHelper.WorkCurveCurrent.Name;
                specList.ActualVoltage = this.deviceMeasure.interfacce.ReturnVoltage;
                specList.ActualCurrent = this.deviceMeasure.interfacce.ReturnCurrent;
                specList.CountRate = this.deviceMeasure.interfacce.ReturnCountRate;
                specList.PeakChannel = double.Parse(this.deviceMeasure.interfacce.MaxChannelRealTime.ToString("f1"));
                specList.TotalCount = (long)this.deviceMeasure.interfacce.TestTotalCount;
                specList.DemarcateEnergys = Default.ConvertFormOldToNew(WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys, WorkCurveHelper.DeviceCurrent.SpecLength);
                specList.InitParam = this.initParams.ConvertToNewEntity();
                #region yuzhaomodify
                specList.CompanyInfoList = CompanyOthersInfo.FindBySql("select * from companyothersinfo where  Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "'");
                #endregion
                if (this.XrfChart != null)
                    DemarcateEnergyHelp.CalParam(this.XrfChart.DemarcateEnergys);
                if (deviceResolve != null)
                {
                    deviceResolve.Spec = this.spec;
                    specList.Resole = deviceResolve.CalculateResolve();
                }
                if (WorkCurveHelper.IsSaveSpecData)
                {
                 

                   
                    
                    WorkCurveHelper.DataAccess.Save(this.specList);
                    #region 纯元素
                    if (WorkCurveHelper.isShowEncoder && this.specList.SpecType == SpecType.PureSpec)
                    {
                        string sql = "select * from PureSpecParam where DeviceName ='" + specList.DeviceName + "' and name ='" + specList.Name + "'";
                        List<PureSpecParam> pureList = PureSpecParam.FindBySql(sql);
                        if (pureList != null)
                            PureSpecParam.DeleteAll(w => w.Name == specList.Name);
                        PureSpecParam purr = PureSpecParam.New;
                        // PureSpecParam pur = PureSpecParam.New.Init(specList.Name, specList.Height, specList.DeviceName,
                        //specList.TotalCount, specList.WorkCurveName, specList.SpecType, specList.SampleName, obj, specList.SpecDate);
                        purr.Name = specList.Name;
                        purr.Height = specList.Height;
                        purr.DeviceName = specList.DeviceName;
                        //if (WorkCurveHelper.IsPureElemCurrentUnify)
                        //    purr.TotalCount = specList.CountRate / specList.ActualCurrent;    //计数率/管流
                        //else
                        //    purr.TotalCount = specList.CountRate; //计数率
                        purr.TotalCount = specList.CountRate;
                        purr.CurrentUnifyCount = specList.CountRate / specList.ActualCurrent;
                        purr.WorkCurveName = specList.WorkCurveName;
                        purr.SpecTypeValue = specList.SpecType;
                        purr.SampleName = specList.SampleName.Split('-').Length > 0 ? specList.SampleName.Split('-')[0] : specList.SampleName;
                        //purr.Data = obj;
                        purr.SpecDate = specList.SpecDate;
                        purr.UsedTime = usedTime;
                        purr.Condition = WorkCurveHelper.WorkCurveCurrent.Condition;
                        purr.ElementName = purr.SampleName;
                        purr.Current = spec.TubCurrent;
                        purr.Save();
                        //WorkCurveHelper.WorkCurveCurrent.Condition.PureSpecParamList.Add(purr);
                    }
                    #endregion

                }
                //判断当前谱文件是否存在空或null，如果则进行修改谱名 
                if (this.skyrayCamera != null && skyrayCamera.AutoSaveSamplePic)
                {
                    FileInfo file = new FileInfo(WorkCurveHelper.SaveSamplePath + "\\" + this.specList.Name + ".jpg");
                    //if (!file.Exists)
                    //this.skyrayCamera.GetGrabImageBytes(WorkCurveHelper.SaveSamplePath + "\\" + this.specList.Name + ".jpg");
                    this.skyrayCamera.GetImage(WorkCurveHelper.SaveSamplePath, this.specList.Name);
                }

                //主要针对Thick
                if (this.XrfChart != null && WorkCurveHelper.IsSaveSpectrumImage)
                {
                    if (grobleState == FormWindowState.Normal)
                        ShowWindow(this.MainForm.Handle, SW_RESTORE);
                    else
                        ShowWindow(this.MainForm.Handle, 3);
                    var chat = this.XrfChart;
                    var bitmap = new Bitmap(chat.Width, chat.Height);
                    bool b1 = chat.IsShowHScrollBar;
                    bool b2 = chat.IsShowVScrollBar;
                    chat.IsShowHScrollBar = false;
                    chat.IsShowVScrollBar = false;
                    chat.DrawToBitmap(bitmap, chat.Bounds);
                    chat.IsShowHScrollBar = b1;
                    chat.IsShowVScrollBar = b2;
                    //byte[] bytes = null;
                    if (bitmap != null)
                    {
                        FileInfo file = new FileInfo(WorkCurveHelper.SaveGraphicPath + "\\" + this.specList.Name + ".jpg");
                        //if (!file.Exists)
                        bitmap.Save(WorkCurveHelper.SaveGraphicPath + "\\" + this.specList.Name + ".jpg", ImageFormat.Jpeg);
                        bitmap.Dispose();
                    }

                }
                if (optMode != OptMode.Matching & optMode != OptMode.Explore)                    //修正匹配不保存谱文件bug
                    //进行判断是否有多次测量等或者有多个样品腔
                    TestFinalDeviceParamsAfter(flag, WorkCurveHelper.WorkCurveCurrent);//进行判断是否有多次测量等或者有多个样品腔

            }
            else
            {
                //对设备扫描对象谱进行重新赋值
                spec = new SpecEntity();
                spec.IsSmooth = true;
                deviceMeasure.interfacce.Spec = spec;
                deviceParamSelectIndex++;
                spec.DeviceParameter = this.deviceParamsList[this.deviceParamSelectIndex].ConvertFrom();
                List<int[]> tempInt = new List<int[]>();

                if (!this.TempNetDataSmooth.TryGetValue(this.deviceParamsList[this.deviceParamSelectIndex].Id, out tempInt))
                    this.TempNetDataSmooth.Add(this.deviceParamsList[this.deviceParamSelectIndex].Id, new List<int[]>());

                this.refreshFillinof.UpdateWorkSpec(this.deviceParamsList[this.deviceParamSelectIndex], this.specList);
                //更新工作曲线信息
                this.refreshFillinof.RefreshCurve(this.initParams, this.deviceParamsList[this.deviceParamSelectIndex]);
                int dt = -1;
                if (!this.currentDeviceParamsList.TryGetValue(this.deviceParamsList[this.deviceParamSelectIndex].Id, out dt))
                    this.currentDeviceParamsList.Add(this.deviceParamsList[this.deviceParamSelectIndex].Id, this.deviceParamsList[this.deviceParamSelectIndex].TubCurrent);
                DeviceParameter deviceTemp = null;
                if (this.FirstDeviceParamsList.TryGetValue(deviceParamSelectIndex, out deviceTemp))
                    this.deviceMeasure.interfacce.DeviceParam = deviceTemp;
                else
                    this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];
                progressInfo.MeasureTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                progressInfo.SurplusTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                progressInfo.Value = 0;
                this.specList.Specs[deviceParamSelectIndex] = this.spec;
                this.XrfChart.CurrentSpecPanel = deviceParamSelectIndex + 1;
                deviceMeasure.interfacce.MotorMove();
            }

        }

        //public delegate void AscendZAutoEndEventHandler();
        //public static event AscendZAutoEndEventHandler AscendZAutoEndEvent;

        public void AscendZAutoEnd()
        {
            TestEndCurrentProcess();
        }
        private void StopTest(int usedTime, bool flag, OptMode optMode)
        {
            if (deviceParamSelectIndex < deviceParamsList.Count)
            {
                //每个测量小条件进行扫描
                if (usedTime == 0)
                    return;
                this.spec.UsedTime = this.deviceMeasure.interfacce.usedTime;
                this.spec.SpecTime = deviceParamsList[deviceParamSelectIndex].PrecTime;
                string middleStr = this.specList.SampleName + "_" + deviceParamsList[deviceParamSelectIndex].Name + "_"
                    + initParams.Condition.Name + "_" + currentTestTimes;
                if (testDevicePassedParams.RemarkInformation != null)
                    this.spec.RemarkInfo = testDevicePassedParams.RemarkInformation;
                else this.spec.RemarkInfo = "";//spec.RemarkInfo不允许为null
                this.spec.DeviceParameter = this.deviceParamsList[deviceParamSelectIndex].ConvertFrom();
                this.spec.TubCurrent = this.spec.TubCurrent > 0 ? this.spec.TubCurrent : this.spec.DeviceParameter.TubCurrent;
                this.spec.TubVoltage = this.spec.TubVoltage > 0 ? this.spec.TubVoltage : this.spec.DeviceParameter.TubVoltage;
                DeviceParameter dt = null;
                if (!this.FirstDeviceParamsList.TryGetValue(deviceParamSelectIndex, out dt))
                    this.FirstDeviceParamsList.Add(this.deviceParamSelectIndex, this.deviceMeasure.interfacce.DeviceParam);
                this.spec.Name = middleStr;
                if (SpecHelper.IsSmoothProcessData)
                {
                    List<int[]> ttOutput = new List<int[]>();
                    if (TempNetDataSmooth.TryGetValue(this.deviceParamsList[deviceParamSelectIndex].Id, out ttOutput))
                    {
                        int[] smooth = Helper.ToInts(this.spec.SpecData);
                        ttOutput.Add(smooth);
                        if (ttOutput.Count > 5)
                        {
                            ttOutput.RemoveAt(0);
                        }
                        else
                        {
                            int[] ttTemp = ttOutput[0];
                            int count = ttOutput.Count;
                            for (int m = count; m < 5; m++)
                                ttOutput.Add(ttTemp);
                        }
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < this.deviceMeasure.interfacce.backData.Length; i++)
                        {
                            int te = 0;
                            foreach (var arr in ttOutput)
                            {
                                te += arr[i];
                            }
                            int temp = (int)Math.Round((te * 1.0 / ttOutput.Count), MidpointRounding.AwayFromZero);
                            sb.Append(temp.ToString() + ",");
                        }
                        TempNetDataSmooth.Remove(this.deviceParamsList[deviceParamSelectIndex].Id);
                        TempNetDataSmooth.Add(this.deviceParamsList[deviceParamSelectIndex].Id, ttOutput);
                        this.spec.SpecData = sb.ToString();
                    }
                }
            }
            //当前小条件进行完毕
            string conditionName = string.Empty;
            string constStr = GetDefineSpectrumName(specList.SampleName, testDevicePassedParams.IsRuleName);
            specList.Name = constStr;
            NewReplaceDeleteData();
            specList.ImageShow = true;
            //specList.Condition = initParams.Condition;
            specList.Color = DifferenceDevice.DefaultSpecColor.ToArgb();
            specList.VirtualColor = specList.Color;
            specList.SpecDate = DateTime.Now;
            specList.DeviceName = WorkCurveHelper.DeviceCurrent.Name;
            specList.WorkCurveName = WorkCurveHelper.WorkCurveCurrent.Name;
            specList.ActualVoltage = this.deviceMeasure.interfacce.ReturnVoltage;
            specList.ActualCurrent = this.deviceMeasure.interfacce.ReturnCurrent;
            specList.CountRate = this.deviceMeasure.interfacce.ReturnCountRate;
            specList.PeakChannel = double.Parse(this.deviceMeasure.interfacce.MaxChannelRealTime.ToString("f1"));
            specList.TotalCount = (long)this.deviceMeasure.interfacce.TestTotalCount;
            specList.DemarcateEnergys = Default.ConvertFormOldToNew(WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys, WorkCurveHelper.DeviceCurrent.SpecLength);
            specList.InitParam = this.initParams.ConvertToNewEntity();
            specList.Height = this.deviceMeasure.interfacce.ReturnEncoderValue;
            specList.CalcAngleHeight = this.deviceMeasure.interfacce.ReturnEncoderHeight;

            // 2018 样品其它信息在停止时保存
            specList.CompanyInfoList = CompanyOthersInfo.FindBySql("select * from companyothersinfo where  Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "'");
            if (this.XrfChart != null)
                DemarcateEnergyHelp.CalParam(this.XrfChart.DemarcateEnergys);
            if (deviceResolve != null)
            {
                deviceResolve.Spec = this.spec;
                specList.Resole = deviceResolve.CalculateResolve();
            }
            if (WorkCurveHelper.IsSaveSpecData)
                WorkCurveHelper.DataAccess.Save(this.specList);
            //判断当前谱文件是否存在空或null，如果则进行修改谱名
            if (this.skyrayCamera != null && skyrayCamera.AutoSaveSamplePic)
            {
                FileInfo file = new FileInfo(WorkCurveHelper.SaveSamplePath + "\\" + this.specList.Name + ".jpg");
                if (!file.Exists)
                    //this.skyrayCamera.GetGrabImageBytes(WorkCurveHelper.SaveSamplePath + "\\" + this.specList.Name + ".jpg");
                    this.skyrayCamera.GetImage(WorkCurveHelper.SaveSamplePath, this.specList.Name);

            }

            //主要针对Thick
            if (this.XrfChart != null && WorkCurveHelper.IsSaveSpectrumImage)
            {
                if (grobleState == FormWindowState.Normal)
                    ShowWindow(this.MainForm.Handle, SW_RESTORE);
                else
                    ShowWindow(this.MainForm.Handle, 3);
                var chat = this.XrfChart;
                var bitmap = new Bitmap(chat.Width, chat.Height);
                bool b1 = chat.IsShowHScrollBar;
                bool b2 = chat.IsShowVScrollBar;
                chat.IsShowHScrollBar = false;
                chat.IsShowVScrollBar = false;
                chat.DrawToBitmap(bitmap, chat.Bounds);
                chat.IsShowHScrollBar = b1;
                chat.IsShowVScrollBar = b2;
                //byte[] bytes = null;
                if (bitmap != null)
                {
                    FileInfo file = new FileInfo(WorkCurveHelper.SaveGraphicPath + "\\" + this.specList.Name + ".jpg");
                    if (!file.Exists)
                        bitmap.Save(WorkCurveHelper.SaveGraphicPath + "\\" + this.specList.Name + ".jpg", ImageFormat.Jpeg);
                    bitmap.Dispose();
                }
            }

            this.selectSpeclist.Clear();
            this.selectSpeclist.Add(this.specList);
            CaculateContentBefore();
            CaculateContent(this.selectSpeclist, currentTestTimes, true);//flag true
            CaculateContentEnd();

        }



        /// <summary>
        /// 根据是否存在当前工作曲线是否存在感兴趣元素的处理过程。
        /// </summary>
        private void TestInitializationElements(bool InitialAll, int id)
        {
            IsInitialAllComplete = false;
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            List<DeviceParameter> listParams = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.ToList();
            if (WorkCurveHelper.WorkCurveCurrent.ElementList == null || WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count == 0 || this.currentSelectMode == 1)
                this.deviceParamsList = listParams;
            else
                DeviceParameterByElementList(listParams);


            //if (WorkCurveHelper.bInitialize && InitialAll)  20220216
            if (InitialAll)
            {
                //List<Condition> lstCondition = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Normal);
                List<Condition> lstCondition = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Normal);
                List<Condition> lstConditionT = lstCondition.FindAll(w => w.InitParam.IsJoinInit == true);
                // List<Condition> lstCondition = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Normal && c.InitParam.IsJoinInit);
                if (lstConditionT != null && lstConditionT.Count > 0)
                {
                    if (id <= lstConditionT.Count - 1)
                        this.initParams = lstConditionT[id].InitParam;
                    if (id == lstConditionT.Count - 1)
                        IsInitialAllComplete = true;
                }
                else
                    this.initParams = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam;
                //  string sqlTemp = "select * from InitParameter group by ElemName";
                //List<InitParameter> currentInitParameters = InitParameter.FindBySql(sqlTemp);
                //if (currentInitParameters != null && currentInitParameters.Count > 0)
                //{
                //    if (id <= currentInitParameters.Count-1)
                //    {
                //        this.initParams = currentInitParameters[id];
                //    }
                //    if (id == currentInitParameters.Count - 1)
                //        IsInitialAllComplete = true;

                //}
            }
            else
            {

                this.initParams = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag">更新能量刻度</param>
        /// <param name="IsCalIntercept"></param>
        /// <param name="InitialAll">初始化所有条件</param>
        /// <param name="id">初始化元素集序号</param>
        public void TestInitalize(bool flag, bool IsCalIntercept, bool InitialAll, int id)
        {
            if (WorkCurveHelper.WorkCurveCurrent == null)
            {
                Msg.Show(Info.WarningTestContext, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (DeviceDisConnection()) return;
            if (!deviceMeasure.interfacce.IsConnectDevice() || !deviceMeasure.interfacce.port.ConnectState)
            {
                Msg.Show(Info.NetDeviceDisConnection);
                return;
            }

            DifferenceDevice.interClassMain.AutoIncrease();
            deviceMeasure.interfacce.IsSpin = DifferenceDevice.Spin;
            TestInitializationElements(InitialAll, id);
            this.deviceMeasure.interfacce.StopFlag = false;
            if (this.deviceMeasure.interfacce.State != DeviceState.Idel)
                return;
            this.XrfChart.CurrentSpecPanel = 1;
            InitCurrentTimes = 1;
            if (WorkCurveHelper.bCurrentInfluenceGain)
            {
                if (WorkCurveHelper.InitCurrentList.Count > 0)
                    this.initParams.TubCurrent = WorkCurveHelper.InitCurrentList[0];
                else
                    this.initParams.TubCurrent = 100; //强制从100开始;  100 ,300 ,600
            }
            lstCurCountRate.Clear();


            if (Msg.Show(Info.PutInitSample + " " + this.initParams.ElemName, Info.InitialInformation, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                WorkCurveHelper.testNum = 0;

                if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                {
                    DifferenceDevice.CurCameraControl.skyrayCamera1.BackgroundImageLayout = ImageLayout.Stretch;

                    Bitmap testDemoImg = DifferenceDevice.CurCameraControl.skyrayCamera1.GrabImage();

                    DifferenceDevice.CurCameraControl.skyrayCamera1.BackgroundImage = testDemoImg;
                    DifferenceDevice.CurCameraControl.skyrayCamera1.Stop();

                    MotorOperator.MotorOperatorY1Thread((int)(-WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                    WorkCurveHelper.waitMoveStop();
                }

                if (!flag)
                {
                    this.specList = new SpecListEntity();
                    if (IsCalIntercept)
                    {
                        optMode = OptMode.CalFPGAIntercept;
                        WorkCurveHelper.IDemarcateTest = 2;
                    }
                    else
                    {
                        optMode = OptMode.Demarcate;//能量刻度
                        WorkCurveHelper.IDemarcateTest = 1;
                    }
                    demacateMode = Demarcate.None;
                    this.spec = new SpecEntity();
                    this.spec.IsSmooth = true;
                    this.specList.Specs = new SpecEntity[1];
                    this.specList.Specs[0] = this.spec;
                    this.deviceMeasure.interfacce.ExistMagnet = WorkCurveHelper.DeviceCurrent.HasElectromagnet;
                    this.deviceMeasure.interfacce.Spec = this.spec;
                    DeviceParameter deviceParams = DeviceParameter.New.Init("", this.deviceMeasure.interfacce.InitTime,
                                                                           this.initParams.TubCurrent, this.initParams.TubVoltage,
                                                                           this.initParams.Filter, this.initParams.Collimator, 0,
                                                                           false, 0, false, 0, false, 0,
                                                                           0, 50, ((int)WorkCurveHelper.DeviceCurrent.SpecLength) - 50,
                                                                           false, false, 0, 0, 0, 0, 0, this.initParams.TargetMode, this.initParams.CurrentRate);
                    this.deviceParamsList = new List<DeviceParameter>();
                    this.deviceParamsList.Add(deviceParams);
                    this.deviceParamSelectIndex = 0;
                    this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[0];
                    this.deviceMeasure.interfacce.InitParam = this.initParams;
                    progressInfo.MeasureTime = this.deviceParamsList[0].PrecTime.ToString() + "s";
                    progressInfo.SurplusTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                    RefreshDeviceInitialize(WorkCurveHelper.DeviceCurrent);
                    TestStartAfterControlState(false);
                    this.deviceMeasure.interfacce.MotorMove();//移动电机并初始化
                }
                else
                {
                    this.specList = new SpecListEntity();
                    optMode = OptMode.Initalize;//初始化
                    this.spec = new SpecEntity();
                    this.spec.IsSmooth = true;
                    this.specList.Specs = new SpecEntity[1];
                    this.specList.Specs[0] = this.spec;
                    this.deviceMeasure.interfacce.InitParam = this.initParams;//初始化条件
                    this.deviceParamSelectIndex = 0;
                    this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];
                    this.deviceMeasure.interfacce.Spec = this.spec;
                    this.deviceMeasure.interfacce.ExistMagnet = WorkCurveHelper.DeviceCurrent.HasElectromagnet;
                    progressInfo.MeasureTime = this.deviceMeasure.interfacce.InitTime + "s";
                    progressInfo.SurplusTime = this.deviceMeasure.interfacce.InitTime + "s";
                    RefreshDeviceInitialize(WorkCurveHelper.DeviceCurrent);
                    TestStartAfterControlState(false);
                    deviceMeasure.interfacce.SetDp5Cfg();
                    this.deviceMeasure.interfacce.InitMotorMove();//移动电机并初始化
                }
            }
        }

        public void TestInitializeAddCurrent()
        {
            InitCurrentTimes++;
            this.deviceMeasure.interfacce.InitParam.TubCurrent = WorkCurveHelper.InitCurrentList[InitCurrentTimes - 1];  //this.deviceMeasure.interfacce.InitParam.TubCurrent + (InitCurrentTimes * 100);
            this.deviceMeasure.interfacce.InitMotorMove();//移动电机并初始化
        }

        //public void TestInitalize()
        //{
        //    if (WorkCurveHelper.WorkCurveCurrent == null)
        //    {
        //        Msg.Show(Info.WarningTestContext, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
        //    if (DeviceDisConnection()) return;



        //    TestInitializationElements();
        //    this.deviceMeasure.interfacce.StopFlag = false;
        //    if (this.deviceMeasure.interfacce.State != DeviceState.Idel)
        //        return;

        //    optMode = OptMode.Initalize;//初始化
        //    this.spec = new SpecEntity();
        //    this.spec.IsSmooth = true;
        //    this.deviceMeasure.interfacce.InitParam = this.initParams;//初始化条件
        //    this.deviceParamSelectIndex = 0;
        //    this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];
        //    this.deviceMeasure.interfacce.Spec = this.spec;
        //    this.deviceMeasure.interfacce.ExistMagnet = WorkCurveHelper.DeviceCurrent.HasElectromagnet;
        //    progressInfo.MeasureTime = this.deviceMeasure.interfacce.InitTime + "s";
        //    progressInfo.SurplusTime = this.deviceMeasure.interfacce.InitTime + "s";
        //    RefreshDeviceInitialize(WorkCurveHelper.DeviceCurrent);
        //    TestStartAfterControlState(false);
        //    this.deviceMeasure.interfacce.InitMotorMove();//移动电机并初始化
        //}


        HardwareClass hc = new HardwareClass();
        private bool DeviceDisConnection()
        {
            if (WorkCurveHelper.DemoInstance)
                return false;
            bool IsDeviceDisConnection = false;
            if (WorkCurveHelper.DeviceCurrent.ComType != ComType.USB)
                return false;
            if (WorkCurveHelper.deviceNameList != null && WorkCurveHelper.deviceNameList.Count > 0)
            {
                foreach (string str in WorkCurveHelper.deviceNameList)
                {
                    if (!hc.List.ToList().Exists(delegate(string v) { return v.StartsWith(str); }))
                    {
                        Msg.Show(Info.NoDeviceConnect);
                        return true;
                    }
                }
            }
            return IsDeviceDisConnection;
        }

        public abstract void TestStop();


        public abstract void RebackZero(Point destPoint);

        /// <summary>
        /// 显示峰值
        /// </summary>
        /// <param name="item">菜单项等</param>
        public void DisplayPeak(NaviItem item)
        {
            if (this.XrfChart != null)
            {
                this.XrfChart.IsShowPeakFlagAuto = !item.MenuStripItem.Checked;
                item.MenuStripItem.Checked = this.XrfChart.IsShowPeakFlagAuto;
                this.XrfChart.setElementPeakFlag();
                this.XrfChart.Refresh();
            }
        }

        public void ReportSpecification(NaviItem item)
        {
            DifferenceDevice.Specification = item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
        }

        public virtual void ExcuteStartProcess(NaviItem item)
        {
            if (item != null)
            {
                if (item.Text.Equals(Info.PauseStop))
                {
                    this.deviceMeasure.interfacce.PauseStop = true;
                    item.BtnDropDown.Image = Properties.Resources.StartTest;
                    item.Text = Info.Start;
                    TestStartPauseState(false);
                }
                else
                {
                    this.deviceMeasure.interfacce.PauseStop = false;
                    item.BtnDropDown.Image = Properties.Resources.Pause;
                    item.Text = Info.PauseStop;
                    TestStartPauseState(false);
                }
            }
        }

        /// <summary>
        /// 根据测量条件进行扫描
        /// </summary>
        /// <param name="testDevicePassedParams">传递过来的扫描信息</param>
        public virtual void StartTestProcess(TestDevicePassedParams testDevicePassedParams)
        {
            if (testDevicePassedParams == null)
            {
                return;
            }

            if (!deviceMeasure.interfacce.IsConnectDevice() || !deviceMeasure.interfacce.port.ConnectState)
            {
                Msg.Show(Info.NetDeviceDisConnection);
                return;
            }
            this.testDevicePassedParams = testDevicePassedParams;
            //if (!Lephone.Data.DbEntry.Context.GetTableNames().Contains("Spec"))
            //{
            //    Spec.FindAll();
            //}
            if (testDevicePassedParams.WordCureTestList.Count == 0)
                return;
            CurveTest = testDevicePassedParams.WordCureTestList[0];
            string sql = string.Empty;
            if (this.currentSelectMode == 0)
            {
                if (CurveTest.WordCurveID == null
                    || CurveTest.WordCurveID == string.Empty)
                {
                    sql = @"select * from WorkCurve as w left join Condition as c on w.Condition_Id = c.Id left join Device as d 
                       on c.Device_Id = d.Id where w.Name = '" + CurveTest.WordCurveName + "' and d.Id =" + WorkCurveHelper.DeviceCurrent.Id;
                }
                else
                {
                    sql = "select * from WorkCurve as w left join Condition as c on w.Condition_Id = c.Id left join Device as d ";
                    sql += "on c.Device_Id = d.Id where w.ID = " + CurveTest.WordCurveID + " and d.Id =" + WorkCurveHelper.DeviceCurrent.Id;

                }


            }
            else
            {
                sql = @"select * from WorkCurve as a join Condition as b on a.Condition_Id = b.Id join Device as d on d.Id=b.Device_Id where d.Id =" + WorkCurveHelper.DeviceCurrent.Id + " and b.Type = 2";
            }
            List<WorkCurve> listWorK = WorkCurve.FindBySql(sql);
            if (listWorK != null && listWorK.Count > 0)
            {
                //this.skyrayCamera.ShowCameraMenu = false;
                XrfChart.CurrentSpecPanel = 1;
                this.SetAxisXRange(this.XrfChart, WorkCurveHelper.XRange[0], WorkCurveHelper.XRange[1]);
                deviceMeasure.interfacce.OpenDevice();
                deviceMeasure.interfacce.IsSpin = DifferenceDevice.Spin;
                WorkCurveHelper.WorkCurveCurrent = listWorK[0];
                this.refreshFillinof.RefreshWorkRegion();
                bPlasticTemp = true;
                openSpecFlag = true;
                openWorkCurve = false;
                OpenWorkCurveLog(WorkCurveHelper.WorkCurveCurrent, this.testDevicePassedParams.MeasureParams.MeasureNumber);
                SubmitMeasureTest(WorkCurveHelper.WorkCurveCurrent, testDevicePassedParams);
            }
        }

        private bool bShowElement;
        public WordCureTest CurveTest;

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="item">菜单项等</param>
        public void DisplayElement(NaviItem item)
        {

            if (this.XrfChart != null)
            {
                item.MenuStripItem.Checked = !item.MenuStripItem.Checked;
                bShowElement = item.MenuStripItem.Checked;
                AutoAnalysisClass = new AutoAnalysisClass(bShowElement);
            }
        }

        /// <summary>
        /// 手动分析
        /// </summary>
        /// <param name="index">元素的系别</param>
        /// <param name="atoms">元素的名称</param>
        public void ManAnalysis(int[] index, string[] atoms)
        {
            WorkCurveHelper.Atoms = atoms;
            WorkCurveHelper.Lines = index;
            this.XrfChart.clearElementTable();//清除原元素
            this.ElemsOfQuale = atoms;
            this.lineStype = index;

            AtomNamesDic.Remove(WorkCurveHelper.PanelSpecIndex);
            AtomNamesDic.Add(WorkCurveHelper.PanelSpecIndex, atoms);
            AtomLinesDic.Remove(WorkCurveHelper.PanelSpecIndex);
            AtomLinesDic.Add(WorkCurveHelper.PanelSpecIndex, index);
            //AutoAnalysisClass = new AutoAnalysisClass(AtomNamesDic, AtomLinesDic, this.bShowElement);
            AutoAnalysisClass = new AutoAnalysisClass(AtomNamesDic, AtomLinesDic, true);
        }

        private AutoAnalysisClass classObj = null;

        public AutoAnalysisClass AutoAnalysisClass
        {
            get { return classObj; }
            set
            {
                if (value != null)
                {
                    //this.xrfChart.Atomnames = this.ElemsOfQuale;
                    //this.xrfChart.Atomlines = this.lineStype;
                    this.XrfChart.AtomNamesDic = this.AtomNamesDic;
                    this.XrfChart.AtomLinesDic = this.AtomLinesDic;
                    this.XrfChart.BShowElement = value.bShow;
                    WorkCurveHelper.NaviItems.Find(w => w.Name == "DisplayElement").MenuStripItem.Checked = this.XrfChart.BShowElement;
                }
            }
        }

        public Dictionary<int, string[]> AtomNamesDic = new Dictionary<int, string[]>();

        public Dictionary<int, int[]> AtomLinesDic = new Dictionary<int, int[]>();

        /// <summary>
        /// 自动分析处理函数
        /// </summary>
        public void AutoAnalysisProcess(NaviItem item)
        {
            if (this.specList == null || this.specList.Specs == null) return;
            if (this.specList.Specs.Length == 1) this.XrfChart.CurrentSpecPanel = 1;
            AtomNamesDic.Clear();
            AtomLinesDic.Clear();
            for (int m = 0; m < specList.Specs.Length; m++)
            {
                if (specList.Specs[m] == null) break;
                this.spec = specList.Specs[m];
                if (this.spec != null && this.spec.SpecData != null)
                {
                    this.qualeElement.Spec = this.spec;
                    if (this.currentSelectMode == 1)
                    {
                        this.qualeElement.IsIntelligence = true;
                        Condition condition = Condition.FindOne(w => w.Type == ConditionType.Intelligent && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                        if (condition == null || condition.DemarcateEnergys == null || condition.DemarcateEnergys.Count == 0)
                            return;
                        if (specList.DemarcateEnergys != null && specList.DemarcateEnergys.Length > 0)
                            DemarcateEnergyHelp.CalParam(specList.DemarcateEnergys);
                        else
                            DemarcateEnergyHelp.CalParam(this.XrfChart.DemarcateEnergys);
                    }
                    else
                    {
                        this.qualeElement.IsIntelligence = false;
                        if (this.XrfChart.DemarcateEnergys == null || this.XrfChart.DemarcateEnergys.Count == 0)
                            return;
                        if (specList.DemarcateEnergys != null && specList.DemarcateEnergys.Length > 0)
                            DemarcateEnergyHelp.CalParam(specList.DemarcateEnergys);
                        else
                            DemarcateEnergyHelp.CalParam(this.XrfChart.DemarcateEnergys);
                    }
                    this.qualeElement.ToChannel = DemarcateEnergyHelp.GetChannel;
                    this.qualeElement.ToEnergy = DemarcateEnergyHelp.GetEnergy;
                    this.ElemsOfQuale = this.qualeElement.Quale(); ;
                    this.lineStype = this.qualeElement.listInt.ToArray();
                    if (lineStype != null && lineStype.Length > 0)
                    {
                        for (int i = 0; i < lineStype.Length; i++)
                        {
                            lineStype[i] = lineStype[i] == 1 ? 2 : 0;
                        }
                    }
                    WorkCurveHelper.PanelSpecIndex = m;
                    WorkCurveHelper.Atoms = ElemsOfQuale;
                    WorkCurveHelper.Lines = lineStype;
                    AtomNamesDic.Add(m, ElemsOfQuale);
                    AtomLinesDic.Add(m, lineStype);
                }
            }
            AutoAnalysisClass = new AutoAnalysisClass(AtomNamesDic, AtomLinesDic, true);
        }

        private List<SpecListEntity> orignalSpecList = new List<SpecListEntity>();

        private void RatioVisualData(int useTime)
        {
            List<SpecListEntity> orignal = WorkCurveHelper.VirtualSpecList;
            if (orignal != null && orignal.Count > 0)
            {
                for (int i = 0; i < orignal.Count; i++)
                {
                    if (orignal[i].Specs == null || orignal[i].Specs.Length == 0)
                        continue;
                    for (int j = 0; j < orignal[i].Specs.Length; j++)
                    {
                        int[] data = Helper.ToInts(orignalSpecList[i].Specs[j].SpecData);
                        //int[] dataOr = Helper.ToInts(orignalSpecList[i].Specs[j].SpecOrignialData == null ?  orignalSpecList[i].Specs[j].SpecData : orignalSpecList[i].Specs[j].SpecOrignialData);
                        int virtualMax = SpecHelper.GetHighSpecValue(orignalSpecList[i].Specs[j].SpecDatas);
                        //修改：何晓明 20111126 没有打开谱的情况下增加比例虚谱报错
                        int currentMax = 0;
                        try
                        {
                            if (this.specList.Specs.Length < orignal[i].Specs.Length)
                            {
                                currentMax = SpecHelper.GetHighSpecValue(this.spec.SpecDatas);
                            }
                            else
                            {
                                currentMax = SpecHelper.GetHighSpecValue(this.specList.Specs[j].SpecDatas);
                            }
                        }
                        catch
                        {
                            currentMax = virtualMax;
                        }
                        //int currentMax = SpecHelper.GetHighSpecValue(this.spec.SpecDatas);
                        //
                        var temp = from dd in data select (int)(dd * ((double)currentMax / virtualMax));
                        //var tempa = from dd in dataOr select (int)(dd * ((double)currentMax / virtualMax));
                        orignal[i].Specs[j].SpecData = Helper.ToStrs(temp.ToArray());
                        //orignal[i].Specs[j].SpecOrignialData = Helper.ToStrs(tempa.ToArray());
                    }
                }
            }
        }

        private bool RatioVisual = false;
        public bool RatioVisualSpec
        {
            get { return RatioVisual; }
        }

        public virtual void SelectRatioVirtualSpec(List<SpecListEntity> splist)
        {
            setRateVirtualSpec();
            RatioVisualData(0);
            AddVirutulSpec(splist);
            RatioVisual = true;
        }

        /// <summary>
        /// 增加对比谱
        /// </summary>
        /// <param name="splist">谱数据</param>
        public virtual void AddVirutulSpec(List<SpecListEntity> splist)
        {
            RatioVisual = false;
            //WorkCurveHelper.VirtualSpecList = splist;
            if (WorkCurveHelper.MainSpecList == null)
            {
                int col = DifferenceDevice.DefaultSpecColor.ToArgb();
                //int col = (WorkCurveHelper.VirtualSpecList == null || WorkCurveHelper.VirtualSpecList.Count == 0) ? DifferenceDevice.DefaultSpecColor.ToArgb() : WorkCurveHelper.VirtualSpecList[0].VirtualColor;
                if (this.spec != null && this.spec.SpecData != null)
                {
                    XrfChart.ShowAloneSpec(WorkCurveHelper.WorkCurveCurrent, this.spec, WorkCurveHelper.VirtualSpecList, false, col);
                }
                else
                {

                    XrfChart.MultiPanel(WorkCurveHelper.WorkCurveCurrent, WorkCurveHelper.MainSpecList, WorkCurveHelper.VirtualSpecList, this.spec, col, XrfChart.GraphPane.Chart.Fill.Color);
                }
            }
            else if (WorkCurveHelper.MainSpecList.Specs.Length == 1)
            {

                XrfChart.ShowAloneSpec(WorkCurveHelper.WorkCurveCurrent, this.spec, WorkCurveHelper.VirtualSpecList, false, WorkCurveHelper.MainSpecList.Color);
            }
            else if (WorkCurveHelper.MainSpecList.Specs.Length > 1 && XrfChart.MasterPane.PaneList.Count == 1)
            {
                XrfChart.ShowAloneSpec(WorkCurveHelper.WorkCurveCurrent, this.spec, WorkCurveHelper.VirtualSpecList, false, WorkCurveHelper.MainSpecList.Color);
            }
            else if (WorkCurveHelper.MainSpecList.Specs.Length > 1 && XrfChart.MasterPane.PaneList.Count > 1)
            {
                XrfChart.MultiPanel(WorkCurveHelper.WorkCurveCurrent, WorkCurveHelper.MainSpecList, WorkCurveHelper.VirtualSpecList, this.spec, WorkCurveHelper.MainSpecList.Color, XrfChart.GraphPane.Chart.Fill.Color);
            }

        }

        private void setRateVirtualSpec()
        {
            List<SpecListEntity> Temp = WorkCurveHelper.VirtualSpecList;
            orignalSpecList.Clear();
            if (Temp != null && Temp.Count > 0)
            {
                RatioVisual = true;
                for (int i = 0; i < Temp.Count; i++)
                {
                    SpecListEntity newList = new SpecListEntity();
                    if (Temp[i].Specs == null || Temp[i].Specs.Length == 0)
                        continue;
                    newList.Specs = new SpecEntity[Temp[i].Specs.Length];
                    for (int j = 0; j < Temp[i].Specs.Length; j++)
                    {
                        SpecEntity tempSpec = new SpecEntity();
                        tempSpec.IsSmooth = true;
                        tempSpec.SpecData = Temp[i].Specs[j].SpecData;
                        newList.Specs[j] = tempSpec;
                    }
                    orignalSpecList.Add(newList);
                }
            }
        }

        public bool ExcuteFlag = false;

        public void SetXRFChartBoundary()
        {
            if (GP.CurrentUser.Role.RoleType == 0)
            {
                if (WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.EC)
                {
                    this.XrfChart.IUseBoundary = true;
                    this.XrfChart.IUseBase = true;
                }
                else
                {
                    this.XrfChart.IUseBoundary = true;
                }
            }
        }

        public void OpenWorkSpecCommon(SpecListEntity specList)
        {
            if (this.XrfChart == null)
                return;
            OpenWorkCurve(specList);
            this.ElemsOfQuale = null;
            this.lineStype = null;
            WorkCurveHelper.MainSpecList = specList;
            this.specList = specList;
            if (this.XrfChart.DemarcateEnergys == null) this.XrfChart.DemarcateEnergys = Default.ConvertFromNewOld(specList.DemarcateEnergys.ToList(), WorkCurveHelper.DeviceCurrent.SpecLength);
        }

        /// <summary>
        /// 是否扫谱
        /// </summary>
        public bool openSpecFlag = true;

        public virtual void ExploreModeDec()
        {
            if (this.currentSelectMode == 1)
            {
                Condition condition = Condition.FindOne(w => w.Type == ConditionType.Intelligent && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                if (condition != null)
                    this.XrfChart.DemarcateEnergys = condition.DemarcateEnergys.ToList();
                if (!displayFlag)
                {
                    if (this.XrfChart.GraphPane.XAxis.Scale.Max == 0) //yuzhao20150611：为了是第一次打开谱图的时候显示的比例与小谱图一致，限制设置X轴放大比例与谱长度一致的功能
                        this.XrfChart.GraphPane.XAxis.Scale.Max = WorkCurveHelper.DeviceCurrent == null ? 2048 : (int)WorkCurveHelper.DeviceCurrent.SpecLength;
                    this.XrfChart.Atoms = Atoms.AtomList;
                    this.XrfChart.MouseMoveEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(xrfChart_MouseMoveEvent);
                    this.XrfChart.MouseDoubleClick += new MouseEventHandler(xrfChart_MouseDoubleClick);
                }
                displayFlag = true;
                string sql = "select * from WorkCurve as a join Condition as b on a.Condition_Id = b.Id join Device as d on d.Id=b.Device_Id where d.Id =" + WorkCurveHelper.DeviceCurrent.Id + " and b.Type = 2";
                WorkCurve current = WorkCurve.FindBySql(sql).Count == 0 ? null : WorkCurve.FindBySql(sql)[0];
                if (current == null)
                {
                    Condition condi = Condition.FindOne(w => w.Type == ConditionType.Intelligent && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    current = WorkCurve.New.Init(Info.Intelligent, Info.Intelligent, CalcType.FP, FuncType.XRF, false, 0, false, false, false, false, false, 0, "", false, Info.strAreaDensityUnit, condi.DeviceParamList[0].PrecTime, true);
                    current.Condition = condi;
                    current.Save();
                }
                WorkCurveHelper.WorkCurveCurrent = current;
                WorkCurveHelper.WorkCurveCurrent.Name = Info.Intelligent;
                DifferenceDevice.MediumAccess.OpenCurveSubmit();
                EDXRFHelper.DisplayWorkCurveControls();
            }
            else
            {
                if (WorkCurveHelper.WorkCurveCurrent == null || WorkCurveHelper.WorkCurveCurrent.Condition.Type == ConditionType.Intelligent)
                {
                    string sql = "select * from WorkCurve as a join Condition as b on a.Condition_Id = b.Id join Device as d on d.Id=b.Device_Id where d.Id =" + WorkCurveHelper.DeviceCurrent.Id + " and a.IsDefaultWorkCurve = 1";
                    WorkCurve current = WorkCurve.FindBySql(sql).Count == 0 ? null : WorkCurve.FindBySql(sql)[0];
                    WorkCurveHelper.WorkCurveCurrent = current;
                    if (current != null)
                    {
                        //WorkCurveHelp er.WorkCurveCurrent = current;
                        DifferenceDevice.MediumAccess.OpenCurveSubmit();
                        EDXRFHelper.DisplayWorkCurveControls();
                    }
                }
            }
        }

        public abstract bool CaculateContent(List<SpecListEntity> specList, int currentTestTimes, bool IsAddHistory);

        public virtual void CaculateContentEnd()
        {

        }

        public virtual void CaculateContentBefore()
        {

        }


        public virtual void OUTSample()
        { }

        public virtual void INSample()
        { }


        public virtual void SetCameraCoeff(int coeff)
        {

        }

        public bool bPlasticTemp = false;


        public virtual bool TestManyConditionData(WorkCurve workCurve) { return false; }

        /// <summary>
        /// 测量中最后一个测量条件进行强度，含量及厚度计算
        /// <param name="flag">该标记用于是否进行计算等操作</param>
        /// </summary>
        protected void TestFinalDeviceParamsAfter(bool flag, WorkCurve workCurve)
        {
            bool bCaculted = false;
            if (flag)
            {
                this.selectSpeclist.Clear();
                this.selectSpeclist.Add(this.specList);
                CaculateContentBefore();
                bCaculted = CaculateContent(this.selectSpeclist, currentTestTimes, true);//flag true
                CaculateContentEnd();
            }
            bool result = TestManyConditionData(workCurve);
            if (!result)
            {
                result = TestNextMeasureTime();
                if (!result)
                {
                    this.currentTestTimes = 1;
                    result = TestContiuous();
                    if (!result)
                    {
                        string sIsAutoSaveReport = ReportTemplateHelper.LoadSpecifiedValue("Report", "IsAutoSaveReport");
                        //if (sIsAutoSaveReport == "0") return;
                        if (DifferenceDevice.IsRohs || DifferenceDevice.IsXRF)
                        {
                            if (bCaculted && sIsAutoSaveReport == "1" && ReportTemplateHelper.ExcelModeType != 2 && DoOtherFormEndTest == null)
                            {
                                //Thread thread = new Thread(new ThreadStart(SaveExcel));//SavePDF()
                                //thread.SetApartmentState(ApartmentState.STA);
                                ////thread.ApartmentState = ;
                                //thread.Start();
                                SaveExcel();
                            }

                            if (DifferenceDevice.IsRohs && WorkCurveHelper.Efi != null && DifferenceDevice.interClassMain.recordList.Count > 0)
                            {
                                bool IsPlasticContinuous = (ReportTemplateHelper.LoadByParameterSpecifiedValue("System", "PlasticContinuous") == "0") ? false : true;
                                List<long> selectLong = new List<long>();

                                if (IsPlasticContinuous)
                                {
                                    string StrIDs = DifferenceDevice.interClassMain.recordList[0].ToString();
                                    for (int i = 1; i < DifferenceDevice.interClassMain.recordList.Count; i++)
                                    {
                                        StrIDs += "," + DifferenceDevice.interClassMain.recordList[i].ToString();
                                    }
                                    List<ContinuousResult> ContinuousResultlist = ContinuousResult.FindBySql(@"select * from continuousresult where historyid in ( " + StrIDs + ")");
                                    //判断历史记录中最大ID是否存在连测历史记录中
                                    List<ContinuousResult> MaxHisRecordContinuousResultlist = ContinuousResult.FindBySql("select * from continuousresult where historyid in (select max(id) from historyrecord)");
                                    if (MaxHisRecordContinuousResultlist.Count > 0)
                                    {
                                        foreach (ContinuousResult continuousResult in ContinuousResultlist)
                                            selectLong.Add(continuousResult.Id);

                                    }

                                    ContinuousResult tempResult = ContinuousResult.FindById(selectLong[0]);
                                    List<ContinuousResult> continuousResults = ContinuousResult.Find(w => w.ContinuousNumber == tempResult.ContinuousNumber);
                                    List<HistoryRecord> historyRecords = new List<HistoryRecord>();
                                    for (int i = 0; i < continuousResults.Count; i++)
                                    {
                                        historyRecords.Add(HistoryRecord.FindById(continuousResults[i].HistoryId));
                                    }

                                    WorkCurveHelper.Efi.Execute(historyRecords);
                                }
                                else
                                {
                                    HistoryRecord historyRecord = HistoryRecord.FindById(DifferenceDevice.interClassMain.recordList[DifferenceDevice.interClassMain.recordList.Count - 1]);
                                    WorkCurveHelper.Efi.Execute(new List<HistoryRecord>() { historyRecord });
                                }

                            }
                        }
                        //else if (DifferenceDevice.IsXRF)
                        //{
                        //    if (sIsAutoSaveReport == "1" && ReportTemplateHelper.ExcelModeType != 2)
                        //    {
                        //        //Thread thread = new Thread(new ThreadStart(SaveExcel));//SavePDF()
                        //        //thread.SetApartmentState(ApartmentState.STA);
                        //        ////thread.ApartmentState = ApartmentState.STA;
                        //        //thread.Start();
                        //        SaveExcel();
                        //    }
                        //}


                        result = MoveToNextChamperProcess();
                        if (!result)
                        {
                            if (WorkCurveHelper.IsAutoAscend && WorkCurveHelper.AscendStepZ != 0 && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                            {
                                this.deviceMeasure.interfacce.CloseDevice();
                                Thread.Sleep(200);
                                deviceMeasure.interfacce.AscendZAuto();
                            }
                            else
                            {
                                TestEndCurrentProcess();
                            }
                            MessageFormat format = new MessageFormat(Info.SpectrumEnd, 0);
                            WorkCurveHelper.specMessage.localMesage.Add(format);

                            //string sIsAutoSaveReport = ReportTemplateHelper.LoadSpecifiedValue("Report", "IsAutoSaveReport");
                            //if (sIsAutoSaveReport == "0") return;
                            //if (DifferenceDevice.IsRohs)
                            //{
                            //    if (sIsAutoSaveReport == "1" && ReportTemplateHelper.ExcelModeType != 2)
                            //    {
                            //        Thread thread = new Thread(new ThreadStart(SaveExcel));//SavePDF()
                            //        thread.SetApartmentState(ApartmentState.STA);
                            //        //thread.ApartmentState = ;
                            //        thread.Start();
                            //    }
                            //}
                            //else if (DifferenceDevice.IsXRF)
                            //{
                            //    if (sIsAutoSaveReport == "1" && ReportTemplateHelper.ExcelModeType != 2)
                            //    {
                            //        Thread thread = new Thread(new ThreadStart(SaveExcel));//SavePDF()
                            //        thread.SetApartmentState(ApartmentState.STA);
                            //        //thread.ApartmentState = ApartmentState.STA;
                            //        thread.Start();
                            //    }
                            //}
                        }
                    }
                }
            }
        }

        public virtual bool TestContiuous()
        {
            return false;
        }

        public virtual void TestEndCurrentProcess()
        {
            DifferenceDevice.IsConnect = false;
            WorkCurveHelper.MainSpecList = this.specList;
            this.deviceMeasure.interfacce.State = DeviceState.Idel;
            this.deviceMeasure.interfacce.connect = DeviceConnect.DisConnect;
            TestStartAfterControlState(true);
            progressInfo.Value = 0;
            DifferenceDevice.IsConnect = false;
            this.deviceMeasure.interfacce.CloseDevice();
            this.FirstDeviceParamsList.Clear();
            if (deviceParamsList == null || deviceParamsList.Count == 0)
                return;
            #region yuzhao20150609_begin:根据戴晓玲需求更改为保存调节计数率后的条件，故删除此处代码

            //foreach (KeyValuePair<long, int> tempParams in this.currentDeviceParamsList)   
            //{
            //    string sql = "Update DeviceParameter Set TubCurrent= "
            //                    + tempParams.Value + " Where Id = " + tempParams.Key;
            //    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            //}

            #endregion yuzhao20150609_end

            //如果多条件情况下，存在扫描完一次，而停止，则删除已保存的谱及其打谱文件
            //if (deviceParamSelectIndex < deviceParamsList.Count - 1 && deviceParamsList.Count > 1 && this.specList != null)
            //{
            //    //this.specList.Delete();
            //    WorkCurveHelper.DataAccess.DeleteRecord(this.specList.Name);
            //}
            //else if (deviceParamsList.Count > 1 && this.specList != null && deviceParamSelectIndex == deviceParamsList.Count - 1 && this.specList.Specs.ToList().Exists(delegate(SpecEntity v) { return v.Name == null; }))
            //{
            //    WorkCurveHelper.DataAccess.DeleteRecord(this.specList.Name);
            //}

            this.currentDeviceParamsList.Clear();

            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0 && DifferenceDevice.Specification)
            {
                //string grade = GetNTGrade(WorkCurveHelper.WorkCurveCurrent, DifferenceDevice.param, DifferenceDevice.gradeNTName, DifferenceDevice.gradeNTNum, DifferenceDevice.MatchNum);
                string grade = QueryGrade();
                Msg.Show(grade, Info.ReportSpecification);
            }

            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null
                && WorkCurveHelper.WorkCurveCurrent.ElementList.NoStandardAlert
                && WorkCurveHelper.EDXRFAlertExceptElement != "")
            {
                //string[] eles = WorkCurveHelper.EDXRFAlertExceptElement.Split(',');
                //判断是否报警"Ni,Cu,Zn,Au,Ag,Pd"
                if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                {
                    foreach (var el in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                    {
                        if (el.IsAlert && el.Content > 0)
                        {
                            Msg.Show(Info.NoStandardAlertInfo, MessageBoxIcon.Warning);
                            break;
                        }
                    }
                }
            }
            // 纯元素测量结束后，替换纯元素谱
            if ((this.specList != null && this.specList.SpecType == SpecType.PureSpec) && (DifferenceDevice.interClassMain.IsConclute))
            {
                PureElementSpecChange(this.specList);
            }
            if (WorkCurveHelper.IsPureAuTest)
            {
                WorkCurveHelper.WorkCurveCurrent = WorkCurveHelper.BeforePureAuCurveCurrent;
                WorkCurveHelper.IsPureAuTest = false;
            }

            if (this.ActionAfterTestFinished != null)
                this.ActionAfterTestFinished();
        }


        /// <summary>
        /// 测量结束后，替换纯元素谱
        /// </summary>
        /// <param name="spec"></param>
        private void PureElementSpecChange(SpecListEntity specs)
        {
            //string elementName = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "SampleName");
            for (int i = 0; i < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; i++)
            {
                if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].IntensityWay == IntensityWay.FixedReference && WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption.Equals(specs.SampleName))
                {
                    if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ElementSpecName != null)
                    {
                        WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ElementSpecName = specs.Name;

                        long dgvId = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].DevParamId;
                        SpecEntity spec = specs.Specs.ToList().Find(s => s.DeviceParameter.Name == DeviceParameter.FindById(dgvId).Name);
                        // WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].SSpectrumData = Helper.TransforToDivTime(spec.SpecData, spec.UsedTime);
                        //element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime, lstSpecListmin[0].ActualCurrent);
                        WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].SSpectrumData = Helper.TransforToDivTimeAndCurrent(spec.SpecData, spec.UsedTime, spec.TubCurrent);
                    }
                }

            }


            WorkCurveHelper.WorkCurveCurrent.Save();
        }

        /// <summary>
        /// 合金牌号查询
        /// </summary>
        /// <param name="content">规定元素的含量数组</param>
        /// <returns>匹配度最接近的两个牌号</returns>
        public unsafe string QueryGrade()
        {
            ReadNTFile();
            // fill content[]
            //double[] content = new double[GradeElements.Length];

            //double[] content = new double[WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count];
            //string[] names = new string[WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count];
            double[] content = new double[GradeElements.Length];

            //Element e = null;
            CurveElement e;
            int ind = 0;
            for (int i = 0; i < GradeElements.Length; ++i)
            {
                e = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(a => a.Caption == GradeElements[i]);
                //e = elements[GradeElements[i]];
                content[i] = 0;
                if (e == null)
                    continue;
                content[i] = e.Content;
                //ind++;
            }
            //for (int i = 0; i < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; i++)
            //{
            //    e= WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i];
            //    content[i] = e.Content;
            //    names[i] = e.Caption;
            //}

            char[,] gradeElemNames = GetElementNames(GradeElements);

            //var pname = from p in WorkCurveHelper.WorkCurveCurrent.ElementList.Items select p.Caption;

            // char[,] testElemNames = GetElementNames(names);
            int[] ret = new int[6];
            fixed (double* pconts = &content[0])
            fixed (int* pret = &ret[0])
            fixed (double* pparams = &Params[0])
            {
                GetGrade(pret, GradeCount, pparams, pconts, GradeElements.Length);
                //GetGrade2(pret, pparams, GradeCount,
                //          gradeElemNames, GradeElements.Length,
                //          testElemNames, content.Length,
                //          pconts);
            }

            string result = string.Empty;
            if (ret[1] / 100 > DifferenceDevice.MatchNum && ret[0] >= 0)
                result += GradeNames[ret[0]].ToString() + "(" + (ret[1] * 1.0 / 100).ToString("F2") + "%)";
            if (ret[3] / 100 > DifferenceDevice.MatchNum && ret[2] >= 0)
                result += " | " + GradeNames[ret[2]].ToString() + "(" + (ret[3] * 1.0 / 100).ToString("F2") + "%)";
            if (result.Length == 0)
                result = "No Match";
            return result;
        }

        /// <summary>
        /// Return two-dimentional array representation of element name array
        /// </summary>
        private char[,] GetElementNames(string[] elemNames)
        {
            char[,] ret = new char[elemNames.Length, 3];
            string elemName = string.Empty;
            for (int i = 0; i < elemNames.Length; ++i)
            {
                elemName = elemNames[i].Trim();
                if (elemName.Length > 1)
                {
                    for (int j = 0; j < 2; ++j)
                    {
                        ret[i, j] = elemName[j];
                    }
                    ret[i, 2] = '\0';
                }
                else
                {
                    ret[i, 0] = elemName[0];
                    ret[i, 1] = '\0';
                    ret[i, 2] = '\0';
                }
            }
            return ret;
        }

        public bool ReadNTFile()
        {
            bool ret = false;
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\" + "param.dat";
            string pathGradeName = AppDomain.CurrentDomain.BaseDirectory + "\\" + "gradename.dat";

            if (!File.Exists(path) || !File.Exists(pathGradeName))
                return ret;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        GradeCount = reader.ReadInt32();
                        GradeElements = reader.ReadString().Split(',');
                        int totalCnt = GradeCount * GradeElements.Length * 3;
                        Params = new double[totalCnt];
                        for (int i = 0; i < totalCnt; ++i)
                            Params[i] = reader.ReadDouble();
                    }
                }

                GradeNames = new string[GradeCount];
                using (FileStream fs = new FileStream(pathGradeName, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        for (int i = 0; i < GradeCount; ++i)
                            GradeNames[i] = reader.ReadString();
                    }
                }
            }
            catch
            {
                return ret;
            }

            ret = true;
            return ret;
        }

        public bool IsDetect = false;
        private int GradeCount;
        private string[] GradeElements;
        private string[] GradeNames;
        private double[] Params;

        /// <summary>
        /// 获取牌号匹配结果(元素个数变长)
        /// </summary>
        /// <param name="ret">[Out]:下标-匹配度, 长度为3x2</param>
        /// <param name="param">[In]:min-max-w数组 length = gradeCnt x elemCnt * 3</param>
        /// <param name="gradeCnt">[In]:牌号总数</param>
        /// <param name="gradeElemNames">[In]:牌号库内部元素顺序</param>
        /// <param name="gradeElemCnt">[In]:牌号库内部元素个数</param>
        /// <param name="curveElemNames">[In]:工作曲线中元素顺序</param>
        /// <param name="curveElemCnt">[In]:工作曲线中元素个数</param>
        /// <param name="curveElemCont">[In]:元素含量数组</param>
        [DllImport(@"GradeDll.dll", EntryPoint = "GetGrade2")]
        public static extern unsafe void GetGrade2(
              int* ret,
              double* param,
              int gradeCnt,
              char[,] gradeElemNames,
              int gradeElemCnt,
              char[,] curveElemNames,
              int curveElemCnt,
              double* curveElemCont
             );

        [DllImport("GradeDll.dll", EntryPoint = "GetGrade", CallingConvention = CallingConvention.StdCall)]
        private static extern unsafe void GetGrade(int* grade, int grade_num, double* param, double* elements, int element_num);
        unsafe public string GetNTGrade(WorkCurve WorkCurve, double[] param, string[] gradeNTName, int gradeNtNum, int MatchNum)
        {
            string nearbyGradeId = string.Empty;
            double[] setElem = new double[16];
            string[] setElemName = new string[] { "Co", "Cr", "Fe", "Mn", "Mo", "Ni", "W", "Ta", "Zr", "Ti", "V", "Nb", "Cu", "Zn", "Hf", "Al" };
            int[] getGradeResult = new Int32[6];
            for (int i = 0; i < WorkCurve.ElementList.Items.Count; i++)
            {
                for (int j = 0; j < setElemName.Length; j++)
                {
                    if (WorkCurve.ElementList.Items[i].Caption != null)
                    {
                        if (WorkCurve.ElementList.Items[i].Caption.ToLower().Equals(setElemName[j].ToLower().ToString()))
                        {
                            setElem[j] = WorkCurve.ElementList.Items[i].Content;
                            break;
                        }
                    }
                }
                int sss = i;
            }
            fixed (double* test = &setElem[0])
            fixed (int* grade = &getGradeResult[0])
            fixed (double* prm = &param[0])
            {
                GetGrade(grade, gradeNtNum, prm, test, 16);
            }
            if (getGradeResult[1] / 100 > MatchNum)
            {
                nearbyGradeId += gradeNTName[getGradeResult[0]];
            }
            if (getGradeResult[3] / 100 > MatchNum)
            {
                nearbyGradeId += "/" + gradeNTName[getGradeResult[2]];
            }
            if (nearbyGradeId.Equals(string.Empty))
            {
                nearbyGradeId = "No Match";
            }
            return nearbyGradeId;
        }

        public bool startTest = false;

        /// <summary>
        /// 判断当前测量次数，并进行处理
        /// </summary>
        private bool TestNextMeasureTime()
        {
            if (currentTestTimes < testDevicePassedParams.MeasureParams.MeasureNumber)
            {
                bool result = false;
                if (!this.IsAverage)
                    result = true;
                else if (this.IsAverage)
                {
                    this.deviceMeasure.interfacce.CloseDevice();
                    if (DialogResult.OK == Msg.Show(Info.ManySampleTest, MessageBoxButtons.OK, MessageBoxIcon.Information))
                        result = true;
                }
                if (result)
                {
                    WorkCurveHelper.IsFirstInfluenceGain = false;   //第二次开始不需要计算gain
                    if (!testDevicePassedParams.MeasureParams.IsManualTest)
                    {
                        StartNextTest();
                        RefreshCameraText(currentTestTimes, testDevicePassedParams.MeasureParams.MeasureNumber, testDevicePassedParams.MeasureParams.IsManualTest);
                    }
                    else
                    {
                        this.deviceMeasure.interfacce.CloseDevice();
                        RefreshCameraText(currentTestTimes + 1, testDevicePassedParams.MeasureParams.MeasureNumber, testDevicePassedParams.MeasureParams.IsManualTest);
                    }


                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        //更改Camera上的信息
        public virtual void RefreshCameraText(int nextNum, int totalNum, bool IsShowButton)
        {
        }

        public void StartNextTest()
        {
            currentTestTimes++;
            spec = new SpecEntity();
            spec.IsSmooth = true;
            deviceMeasure.interfacce.Spec = spec;
            deviceParamSelectIndex = 0;
            progressInfo.Value = 0;
            this.spec.DeviceParameter = this.deviceParamsList[deviceParamSelectIndex].ConvertFrom();
            TestStartAfterControlState(false);
            double loss = specList.Loss;
            specList = new SpecListEntity("", specList.SampleName, specList.Height, specList.CalcAngleHeight, specList.Supplier, specList.Weight, specList.Shape, specList.Operater, specList.SpecDate,
                specList.SpecSummary, specList.SpecType, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb());
            specList.Loss = loss;
            //specList.Specs.Clear();
            //if (this.skyrayCamera != null)
            //    this.specList.Image = this.skyrayCamera.GetGrabImageBytes();
            //this.specList.Condition = this.initParams.Condition;
            this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
            this.specList.Specs[0] = spec;
            this.refreshFillinof.UpdateWorkSpec(this.deviceParamsList[this.deviceParamSelectIndex], this.specList);
            //更新工作曲线信息
            this.refreshFillinof.RefreshCurve(this.initParams, this.deviceParamsList[this.deviceParamSelectIndex]);
            DeviceParameter dt = null;
            if (this.FirstDeviceParamsList.TryGetValue(deviceParamSelectIndex, out dt))
                this.deviceMeasure.interfacce.DeviceParam = dt;
            else
                this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];
            if (!WorkCurveHelper.DeviceCurrent.IsDP5 && !this.testDevicePassedParams.MeasureParams.IsManualTest)
                this.deviceMeasure.interfacce.DropTime = 0;
            //progressInfo.MeasureTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
            //progressInfo.SurplusTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
            InitProcessBar();
            this.XrfChart.CurrentSpecPanel = 1;
            this.deviceMeasure.interfacce.InitParam = this.initParams;
            //this.deviceMeasure.interfacce.ImmediacyDotest();
            if (WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count == 1 && !this.testDevicePassedParams.MeasureParams.IsManualTest)
            {
                this.deviceMeasure.interfacce.HasVauccum = true;
            }
            this.deviceMeasure.interfacce.MotorMove();
        }
        public virtual void NextChamperOptionsOper()
        {
            List<DeviceParameter> listParams = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.ToList();
            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                DeviceParameterByElementList(listParams);
            else
                this.deviceParamsList = listParams;
            this.initParams = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam;
        }

        /// <summary>
        /// 如果存在下一个样品，移动到指定位置，打开相应曲线，初始化测量参数
        /// </summary>
        private bool MoveToNextChamperProcess()
        {
            int cellIndex = this.deviceMeasure.interfacce.GetCellIndex(ChamberCellState.Testing);
            if (cellIndex < 0)
                return false;
            this.deviceMeasure.interfacce.CellStates[cellIndex] = ChamberCellState.Finish;
            cellIndex = this.deviceMeasure.interfacce.GetCellIndex(ChamberCellState.Waitting);
            if (cellIndex < 0 || this.testDevicePassedParams.WordCureTestList.Count == 0)
                return false;
            CurveTest = this.testDevicePassedParams.WordCureTestList.Find(w => w.SerialNumber == (cellIndex + 1).ToString());
            string workCurveName = CurveTest.WordCurveName;
            spec = new SpecEntity();
            spec.IsSmooth = true;
            deviceMeasure.interfacce.Spec = spec;
            this.specList = CurveTest.Spec;
            string sql = @"select distinct a.* from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c 
                                    on b.Device_Id = c.Id where b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and a.Name ='" + workCurveName + "'";
            List<WorkCurve> currentCurve = WorkCurve.FindBySql(sql);
            if ((currentCurve != null) && (currentCurve.Count != 0))
            {
                WorkCurveHelper.WorkCurveCurrent = currentCurve[0];
                OpenWorkCurveLog(currentCurve[0], this.testDevicePassedParams.MeasureParams.MeasureNumber);
                if (WorkCurveHelper.WorkCurveCurrent != null)
                {
                    this.initParams = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam;
                    //this.initParams.Gain = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.Gain;
                    //this.initParams.FineGain = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.FineGain;
                }
                this.deviceParamsList = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.ToList();

                if (this.testDevicePassedParams.MatchChecked)
                {
                    this.optMode = OptMode.Matching;
                    Condition condition = Condition.FindOne(wc => wc.Type == ConditionType.Match && wc.Device.Id == WorkCurveHelper.DeviceCurrent.Id); //操作指定类型的条件
                    if (condition == null)
                    {
                        Msg.Show(Info.SpecifiedConditionNoExits);
                        return false;
                    }
                    this.initParams = condition.InitParam;
                    if (WorkCurveHelper.WorkCurveCurrent != null)
                    {
                        this.initParams.Gain = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.Gain;
                        this.initParams.FineGain = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.FineGain;
                    }
                    this.deviceParamsList = condition.DeviceParamList.ToList();
                }
                else
                {
                    NextChamperOptionsOper();
                }
                if (this.deviceParamsList.Count == 0)
                {
                    Msg.Show(Info.MeasureConditionInvalidate);
                    return false;
                }
                //this.specList.Condition = this.initParams.Condition;
                this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
                this.specList.SpecDate = DateTime.Now;
                this.specList.Specs[0] = spec;
                this.deviceParamSelectIndex = 0;

                TempNetDataSmooth.Clear();
                List<int[]> temp = new List<int[]>();
                TempNetDataSmooth.Add(this.deviceParamsList[this.deviceParamSelectIndex].Id, temp);

                this.refreshFillinof.UpdateWorkSpec(this.deviceParamsList[this.deviceParamSelectIndex], this.specList);
                //更新工作曲线信息
                this.refreshFillinof.RefreshCurve(this.initParams, this.deviceParamsList[this.deviceParamSelectIndex]);
                this.refreshFillinof.RefreshWorkRegion();
                TestStartAfterControlState(false);
                bPlasticTemp = true;
                this.currentTestTimes = 1;
                //progressInfo.MeasureTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                //progressInfo.SurplusTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                InitProcessBar();
                this.deviceMeasure.interfacce.InitParam = this.initParams;
                this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];
                deviceMeasure.interfacce.CellStates[cellIndex] = ChamberCellState.Testing;
                this.deviceMeasure.interfacce.MotorMove(cellIndex + 1);
            }
            return true;
        }

        /// <summary>
        /// 更新和插入能量刻度
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public long UpdateDemarcateByConditiion(Condition condition, string elementName)
        {
            if (condition == null)
                return 0;
            DemarcateEnergy existsEnergy = condition.DemarcateEnergys.ToList().Find(delegate(DemarcateEnergy w) { return w.Line == XLine.Ka && w.ElementName == elementName && w.Condition.Id == condition.Id; });
            if (existsEnergy != null)
            {
                existsEnergy.Channel = SpecHelper.FitChannOfMaxValue(deviceParamsList[deviceParamSelectIndex].BeginChann, deviceParamsList[deviceParamSelectIndex].EndChann,
                                                                  spec.SpecDatas);
                string stringSql = "update DemarcateEnergy set Channel=" + existsEnergy.Channel + " where id=" + existsEnergy.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
                return 0;
            }
            else
            {
                double Energy = Atoms.AtomList.Find(w => w.AtomName == elementName).Ka;
                double Channel = SpecHelper.FitChannOfMaxValue(deviceParamsList[deviceParamSelectIndex].BeginChann, deviceParamsList[deviceParamSelectIndex].EndChann,
                                                                   spec.SpecDatas);
                //string stringSql = "select Max(Id) from DemarcateEnergy";
                //object obj = Lephone.Data.DbEntry.Context.ExecuteScalar(stringSql);
                //int id = 1;
                //try
                //{
                //    id = int.Parse(obj.ToString()) + 1;
                //}
                //catch
                //{
                //    id = 1;
                //}
                //stringSql = "insert into DemarcateEnergy(Id,ElementName,Line,Energy,Channel,Condition_Id) values(" + id + ",'" + elementName + "'," + (int)XLine.Ka + "," + Energy + "," + Channel + "," + condition.Id + ")";
                //Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
                //return id;
                string stringSql = "insert into DemarcateEnergy(ElementName, Line, Energy, Channel, Condition_Id) values('" + elementName + "'," + (int)XLine.Ka + "," + Energy + "," + Channel + "," + condition.Id + ")";
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
                stringSql = "select Max(Id) from DemarcateEnergy";
                object obj = Lephone.Data.DbEntry.Context.ExecuteScalar(stringSql);
                int id = 1;
                try
                {
                    id = int.Parse(obj.ToString());
                }
                catch
                {
                    id = 1;
                }
                return id;
            }
        }

        public virtual void DemarcateFirstStage() { }

        public virtual void DemarcateSencondStage() { }

        /// <summary>
        /// 初始化能量刻度并保存
        /// <param name="workCurve">当前工作曲线</param>
        /// </summary>
        public virtual void optModeDemarcate(WorkCurve workCurve, OptMode optMode)
        {
            if (workCurve == null)
                return;
            string elementName = string.Empty;
            if (demacateMode == Demarcate.None)
            {
                string sampleName = "Cu";
                if (optMode == OptMode.CalFPGAIntercept)//计算斜率
                {
                    sampleName = "Fe";
                    CalInterceptEnergys.Clear();
                    double Energy = Atoms.AtomList.Find(w => w.AtomName == this.initParams.ElemName).Ka;
                    double Channel = SpecHelper.FitChannOfMaxValue(deviceParamsList[deviceParamSelectIndex].BeginChann, deviceParamsList[deviceParamSelectIndex].EndChann,
                                                                   spec.SpecDatas);
                    DemarcateEnergy de = DemarcateEnergy.New.Init(this.initParams.ElemName, XLine.Ka, Energy, Channel);
                    CalInterceptEnergys.Add(de);
                    progressInfo.Value = 0;
                    DemarcateFirstStage();
                    TestStartAfterControlState(true);
                    if (this.deviceMeasure.interfacce != null)
                        this.deviceMeasure.interfacce.CloseDevice();
                }
                else//能量刻度
                {
                    Condition condition = workCurve.Condition;
                    long id = UpdateDemarcateByConditiion(condition, this.initParams.ElemName);
                    if (id != 0)
                    {
                        DemarcateEnergy newFind = DemarcateEnergy.FindById(id);
                        workCurve.Condition.DemarcateEnergys.Add(newFind);
                    }
                    condition = Condition.FindOne(w => w.Type == ConditionType.Intelligent && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    UpdateDemarcateByConditiion(condition, this.initParams.ElemName);
                    condition = Condition.FindOne(w => w.Type == ConditionType.Match && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    UpdateDemarcateByConditiion(condition, this.initParams.ElemName);
                    condition = Condition.FindOne(w => w.Type == ConditionType.Match2 && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    UpdateDemarcateByConditiion(condition, this.initParams.ElemName);
                    progressInfo.Value = 0;
                    DemarcateFirstStage();
                    TestStartAfterControlState(true);
                    if (this.deviceMeasure.interfacce != null)
                        this.deviceMeasure.interfacce.CloseDevice();
                    this.XrfChart.DemarcateEnergys = workCurve.Condition.DemarcateEnergys.ToList();
                    this.XrfChart.Refresh();
                }

                if (progressInfo.Maximum == progressInfo.Value && WorkCurveHelper.IsAutoIncrease)
                {
                    progressInfo.LabelMeasureTime.Text = progressInfo.Maximum + "s";
                }

                FrmDialog dialog = new FrmDialog(Info.Suggestion, Info.PleaseCurveCalibrationSample + sampleName, false, MessageBoxIcon.Information);
                dialog.OnSubmit += new EventDelegate.EventHandleDialog(dialog_OnSubmit);
                dialog.StartPosition = FormStartPosition.CenterScreen;
                dialog.TopMost = true;
                dialog.Show();
            }
            else if (demacateMode == Demarcate.Test)
            {
                if (optMode == OptMode.CalFPGAIntercept)//计算斜率
                {
                    double Energy = Atoms.AtomList.Find(w => w.AtomName == "Fe").Ka;
                    double Channel = SpecHelper.FitChannOfMaxValue(deviceParamsList[deviceParamSelectIndex].BeginChann, deviceParamsList[deviceParamSelectIndex].EndChann,
                                                                   spec.SpecDatas);
                    DemarcateEnergy de = DemarcateEnergy.New.Init("Fe", XLine.Ka, Energy, Channel);
                    CalInterceptEnergys.Add(de);
                    if (CalInterceptEnergys.Count > 1
                        && CalInterceptEnergys[1].Energy != CalInterceptEnergys[0].Energy
                        && CalInterceptEnergys[1].Channel != CalInterceptEnergys[0].Channel)
                    {
                        double e1 = CalInterceptEnergys[0].Energy;
                        double e2 = CalInterceptEnergys[1].Energy;
                        double ch1 = CalInterceptEnergys[0].Channel;
                        double ch2 = CalInterceptEnergys[1].Channel;
                        WorkCurveHelper.DeviceCurrent.FPGAParams.Intercept = (e1 * ch2 - e2 * ch1) / (e1 - e2);
                        WorkCurveHelper.DeviceCurrent.FPGAParams.Save();
                    }
                    progressInfo.Value = 0;
                    TestStartAfterControlState(true);
                    if (this.deviceMeasure.interfacce != null)
                        this.deviceMeasure.interfacce.CloseDevice();
                }
                else
                {
                    Condition condition = workCurve.Condition;
                    long Id = UpdateDemarcateByConditiion(condition, "Cu");
                    if (Id != 0)
                    {
                        DemarcateEnergy newFind = DemarcateEnergy.FindById(Id);
                        workCurve.Condition.DemarcateEnergys.Add(newFind);
                    }
                    condition = Condition.FindOne(w => w.Type == ConditionType.Intelligent && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    UpdateDemarcateByConditiion(condition, "Cu");
                    condition = Condition.FindOne(w => w.Type == ConditionType.Match && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    UpdateDemarcateByConditiion(condition, "Cu");
                    condition = Condition.FindOne(w => w.Type == ConditionType.Match2 && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                    UpdateDemarcateByConditiion(condition, "Cu");
                    progressInfo.Value = 0;
                    DemarcateSencondStage();
                    TestStartAfterControlState(true);
                    if (this.deviceMeasure.interfacce != null)
                        this.deviceMeasure.interfacce.CloseDevice();
                    //MessageFormat format = new MessageFormat(Info.SpectrumEnd, 0);
                    //WorkCurveHelper.specMessage.localMesage.Add(format);
                    this.XrfChart.DemarcateEnergys = workCurve.Condition.DemarcateEnergys.ToList();
                    this.XrfChart.Refresh();
                }
                WorkCurveHelper.IDemarcateTest = 0;

            }
        }

        void dialog_OnSubmit(bool flag)
        {
            DeviceParameter deviceParams = DeviceParameter.New.Init("", this.deviceMeasure.interfacce.InitTime,
                                                                           this.initParams.TubCurrent, this.initParams.TubVoltage,
                                                                           this.initParams.Filter, this.initParams.Collimator, 0,
                                                                           false, 0, false, 0, false, 0,
                                                                           0, 50, ((int)WorkCurveHelper.DeviceCurrent.SpecLength) - 50,
                                                                           false, false, 0, 0, 0, 0, 0, this.initParams.TargetMode, this.initParams.CurrentRate);
            this.deviceParamsList = new List<DeviceParameter>();
            this.deviceParamsList.Add(deviceParams);
            this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[0];
            this.deviceMeasure.interfacce.InitParam = this.initParams;
            progressInfo.MeasureTime = this.deviceParamsList[0].PrecTime.ToString() + "s";
            progressInfo.SurplusTime = this.deviceParamsList[0].PrecTime.ToString() + "s";
            this.deviceMeasure.interfacce.StopFlag = false;
            demacateMode = Demarcate.Test;
            TestStartAfterControlState(false);
            progressInfo.Value = 0;
            this.deviceMeasure.Test();
        }

        /// <summary>
        /// 打开工作曲线处理函数
        /// </summary>
        /// <param name="workCurve">当前工作曲线</param>
        public virtual void OpenWorkCurveLog(WorkCurve workCurve, int count)
        {
            this.elementName = string.Empty;
            OpenWorkCurve(null);
            //if (!registerSuccess)
            //IsRegisterChinawareData();
            if (DifferenceDevice.IsThick)  //thick增加判定绑定到曲线
            {
                DbObjectList<CustomStandard> lstStandard = CustomStandard.FindAll();//获取所有标准
                if (lstStandard != null)
                    WorkCurveHelper.CurrentStandard = lstStandard.Find(S => S.StandardName == workCurve.ThickStandardName);//查找同名标准
                else
                    WorkCurveHelper.CurrentStandard = null;
            }
            this.refreshFillinof.ChangeCheckedCurve();//更改界面上被选择的曲线的操作
            this.refreshFillinof.RefreshTarget();

            this.refreshFillinof.ContructMeasureRefreshData(count, workCurve.ElementList);
            this.deviceMeasure.interfacce.HasVauccum = false;
            //if ((bInIsMulti || bInIsNetwork) && this.skyrayCamera.ContiTestPoints.Count > 0)
            //    this.refreshFillinof.ContructMeasureRefreshData(count * this.skyrayCamera.ContiTestPoints.Count, workCurve.ElementList);
            //else
            //    this.refreshFillinof.ContructMeasureRefreshData(count, workCurve.ElementList);
            this.refreshFillinof.CreateContructStatis(workCurve.ElementList);
            this.refreshFillinof.RefreshCurve(workCurve.Condition.InitParam, workCurve.Condition.DeviceParamList[0]);
            this.openWorkCurve = true;
            if (this.spec != null && !string.IsNullOrEmpty(this.spec.SpecData))
                BackGroundHelper.ChangeIntVisible(true);
            EDXRFHelper.DisplayWorkCurveControls();
            //NaviItem item = WorkCurveHelper.NaviItems.Find(w=>w.Name == "cboMode");
            //if (item != null)
            //{
            //    if (WorkCurveHelper.WorkCurveCurrent.Name.Equals(Info.Intelligent))
            //    {
            //        item.ComboStrip.SelectedIndex = 1;
            //    }
            //    else
            //    {
            //        item.ComboStrip.SelectedIndex = 0;
            //    }
            //}
            CheckFitElement();
            //测量条件的时候赋值为当前曲线时间
            WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].PrecTime = WorkCurveHelper.WorkCurveCurrent.TestTime;

        }


        public void ReloadVirtualSpecByWorkCurve()
        {
            //上一条曲线有且只有一条默认对比谱时，清空那条对比谱
            if (PreviousRefSpecListIdStr != string.Empty && WorkCurveHelper.VirtualSpecList.Count == 1)
            {
                if (WorkCurveHelper.VirtualSpecList[0].Name.ToString().Equals(PreviousRefSpecListIdStr))
                {
                    WorkCurveHelper.VirtualSpecList.Clear();
                    WorkCurveHelper.VirtualSpecListAdditional.Clear();
                }
            }
            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr != null && WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr != string.Empty)
            {
                WorkCurveHelper.VirtualSpecList.Clear();
                WorkCurveHelper.VirtualSpecListAdditional.Clear();
                SpecListEntity spectemp = WorkCurveHelper.DataAccess.Query(WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr);
                if (spectemp != null)
                {
                    spectemp.VirtualColor = spectemp.VirtualColor == 0 ? WorkCurveHelper.DefaultVirtualColor[0].Color.ToArgb() : spectemp.VirtualColor;
                    WorkCurveHelper.VirtualSpecList.Add(spectemp);
                    WorkCurveHelper.VirtualSpecListAdditional.Add(spectemp.Name, true);
                }
            }
            if (DifferenceDevice.interClassMain.RatioVisualSpec)  //比例对比谱
                DifferenceDevice.MediumAccess.SelectRatioVirtualSpec(WorkCurveHelper.VirtualSpecList);
            else
                DifferenceDevice.MediumAccess.OpenVirtualWorkSpectrum(WorkCurveHelper.VirtualSpecList);
        }


        public void ReloadVirtualByWorkCurve()
        {
            WorkCurveHelper.VirtualSpecList.Clear();
            WorkCurveHelper.VirtualSpecListAdditional.Clear();
            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr != null && WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr != string.Empty)
            {

                SpecListEntity spectemp = WorkCurveHelper.DataAccess.Query(WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr);
                if (spectemp != null)
                {
                    spectemp.VirtualColor = spectemp.VirtualColor == 0 ? WorkCurveHelper.DefaultVirtualColor[0].Color.ToArgb() : spectemp.VirtualColor;
                    WorkCurveHelper.VirtualSpecList.Add(spectemp);
                    WorkCurveHelper.VirtualSpecListAdditional.Add(spectemp.Name, true);
                }
            }
            if (DifferenceDevice.interClassMain.RatioVisualSpec)  //比例对比谱
                DifferenceDevice.MediumAccess.SelectRatioVirtualSpec(WorkCurveHelper.VirtualSpecList);
            else
                DifferenceDevice.MediumAccess.OpenVirtualWorkSpectrum(WorkCurveHelper.VirtualSpecList);
        }

        private void CheckFitElement()
        {
            if (WorkCurveHelper.WorkCurveCurrent.ElementList == null)
            { return; }
            foreach (var item in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
            {
                if (item.IntensityWay == IntensityWay.Reference || item.IntensityWay == IntensityWay.FixedReference)
                {
                    string[] refElements = item.ReferenceElements;
                    foreach (var refe in refElements)
                    {
                        if (item.References.ToList().Find(r => r.ReferenceElementName == refe) == null)
                        {
                            var findElem = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(e => e.Caption == refe);
                            if (findElem == null) continue;

                            ReferenceElement re = ReferenceElement.New.Init(item.Caption, refe, findElem.PeakLow, findElem.PeakHigh, findElem.BaseLow, findElem.BaseHigh, findElem.PeakDivBase);

                            item.References.Add(re);

                            item.Save();
                        }
                    }
                }
            }
        }

        public void InitXRFChart()
        {
            if (this.XrfChart == null)
                return;
            if (WorkCurveHelper.WorkCurveCurrent != null)
            {
                this.XrfChart.DemarcateEnergys = WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys.ToList();
            }
            else
            {
                this.XrfChart.DemarcateEnergys = Default.ConvertFromNewOld(specList.DemarcateEnergys.ToList(), WorkCurveHelper.DeviceCurrent.SpecLength);
            }
            if (!displayFlag)
            {
                if (this.XrfChart.GraphPane.XAxis.Scale.Max == 0) //yuzhao20150611：为了是第一次打开谱图的时候显示的比例与小谱图一致，限制设置X轴放大比例与谱长度一致的功能
                    this.XrfChart.GraphPane.XAxis.Scale.Max = WorkCurveHelper.DeviceCurrent == null ? 2048 : (int)WorkCurveHelper.DeviceCurrent.SpecLength;
                this.XrfChart.Atoms = Atoms.AtomList;
                this.XrfChart.MouseMoveEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(xrfChart_MouseMoveEvent);
                this.XrfChart.MouseDoubleClick += new MouseEventHandler(xrfChart_MouseDoubleClick);
            }
            messageProcess.StartThread(this.XrfChart);
            this.XrfChart.OnAddPeakFlag += new EventHandler(xrfChart_OnAddPeakFlag);
            this.XrfChart.OnDelPeakFlag += new EventHandler(xrfChart_OnDelPeakFlag);
            this.XrfChart.OnLeftBorder += new EventHandler<GraphEventArgs>(xrfChart_OnLeftBorder);
            this.XrfChart.OnRightBorder += new EventHandler<GraphEventArgs>(xrfChart_OnRightBorder);
            this.XrfChart.OnLeftBase += new EventHandler<GraphEventArgs>(xrfChart_OnLeftBase);
            this.XrfChart.OnRightBase += new EventHandler<GraphEventArgs>(xrfChart_OnRightBase);
            this.XrfChart.OnVirtualSpec += new EventHandler(xrfChart_OnVirtualSpec);
            if (this.XrfChart.GraphPane.XAxis.Scale.Max == 0) //yuzhao20150611：为了是第一次打开谱图的时候显示的比例与小谱图一致，限制设置X轴放大比例与谱长度一致的功能
                this.XrfChart.MasterPane.PaneList[0].XAxis.Scale.Max = WorkCurveHelper.DeviceCurrent == null ? 2048 : (int)WorkCurveHelper.DeviceCurrent.SpecLength;
        }

        /// <summary>
        /// 打开工作曲线相应的谱图控件进行初始化
        /// </summary>
        /// <param name="specList">谱数据</param>
        public void OpenWorkCurve(SpecListEntity specList)
        {
            if (this.XrfChart == null)
                return;
            this.XrfChart.BoundaryElement = elementName;
            this.XrfChart.IUseBoundary = false;
            this.XrfChart.IUseBase = false;
            if (WorkCurveHelper.WorkCurveCurrent != null)
            {
                this.XrfChart.DemarcateEnergys = WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys.ToList();
            }
            else
            {
                List<DemarcateEnergy> tt = new List<DemarcateEnergy>();
                specList.DemarcateEnergys.ToList().ForEach(w => tt.Add(w.ConvertFrom()));
                this.XrfChart.DemarcateEnergys = tt;
            }
            if (!displayFlag)
            {
                if (this.XrfChart.GraphPane.XAxis.Scale.Max == 0) //yuzhao20150611：为了是第一次打开谱图的时候显示的比例与小谱图一致，限制设置X轴放大比例与谱长度一致的功能
                    this.XrfChart.GraphPane.XAxis.Scale.Max = WorkCurveHelper.DeviceCurrent == null ? 2048 : (int)WorkCurveHelper.DeviceCurrent.SpecLength;
                this.XrfChart.Atoms = Atoms.AtomList;
                this.XrfChart.MouseMoveEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(xrfChart_MouseMoveEvent);
                this.XrfChart.MouseDoubleClick += new MouseEventHandler(xrfChart_MouseDoubleClick);
            }
            displayFlag = true;
        }

        public void AutoDetection()
        {
            if (WorkCurveHelper.IsAutoDetection)
            {
                new FrmAutoDetection().ShowDialog();
            }
        }

        public bool DeviceConnection()
        {

            if (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA && deviceMeasure.interfacce.port.ConnectState)
            {
                return true;
            }
            bool conn = deviceMeasure.interfacce.port.Connect();
            if (conn && WorkCurveHelper.DeviceCurrent.HasElectromagnet)
            {
                deviceMeasure.interfacce.port.OpenPump();
            }
            return conn;
        }

        public void LoadDetectionParam()
        {
            DbObjectList<QualeElement> QualeElementlIST = QualeElement.FindAll();
            if (QualeElementlIST.Count > 0)
            { QualeElement = QualeElementlIST[0]; }
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml");
            XmlNodeList xmlCalibrationList = doc.SelectNodes("application/Detection/Item");
            try
            {
                foreach (XmlNode xTemp in xmlCalibrationList)
                {
                    DetectionResollve = xTemp.Attributes["Resolve"] == null ? 0d : double.Parse(xTemp.Attributes["Resolve"].InnerText);
                    DetectionResollveError = xTemp.Attributes["ResolveError"] == null ? 0d : double.Parse(xTemp.Attributes["ResolveError"].InnerText);
                    DetectionPeakChannel = xTemp.Attributes["PeakChannel"] == null ? 0d : double.Parse(xTemp.Attributes["PeakChannel"].InnerText);
                    DetectionPeakError = xTemp.Attributes["PeakError"] == null ? 0d : double.Parse(xTemp.Attributes["PeakError"].InnerText);
                    DetectionMotorTime = xTemp.Attributes["MotorDelayTime"] == null ? 0 : int.Parse(xTemp.Attributes["MotorDelayTime"].InnerText);
                    DetectionDetecTime = xTemp.Attributes["DetectorDelayTime"] == null ? 0 : int.Parse(xTemp.Attributes["DetectorDelayTime"].InnerText);
                    DetectionPeakSecChannel = xTemp.Attributes["PeakSecChannel"] == null ? 0d : double.Parse(xTemp.Attributes["PeakSecChannel"].InnerText);
                    DetectionPeakSecError = xTemp.Attributes["PeakSecError"] == null ? 0d : double.Parse(xTemp.Attributes["PeakSecError"].InnerText);
                    DetectionCountRate = xTemp.Attributes["CountRate"] == null ? 0d : double.Parse(xTemp.Attributes["CountRate"].InnerText);
                    DetectionCountRateError = xTemp.Attributes["CountRateError"] == null ? 0d : double.Parse(xTemp.Attributes["CountRateError"].InnerText);
                    DetectionHalfWidth = xTemp.Attributes["HalfWidth"] == null ? 0d : double.Parse(xTemp.Attributes["HalfWidth"].InnerText);
                    DetectionHalfWidthError = xTemp.Attributes["HalfWidthError"] == null ? 0d : double.Parse(xTemp.Attributes["HalfWidthError"].InnerText);
                    break;
                }
            }
            catch { }
        }

        public bool DetectionHVLock()
        {
            int intVolgate = 0, intCurrent = 0, iTemp = 0, iVacuum = 0;
            bool iCoverClose = false;
            deviceMeasure.interfacce.port.GetParams(ref intVolgate, ref intCurrent, ref iTemp, ref iVacuum, ref iCoverClose);
            if (iVacuum > 0)
                return true;
            else
                return false;
        }

        public bool DetectionMotor(int code, int direct, int speed)
        {
            int dir = direct == 0 ? 1 : 0;
            DateTime begin;
            DateTime end;
            TimeSpan ts;
            try
            {
                begin = DateTime.Now;
                do
                {
                    Thread.Sleep(200);
                    end = DateTime.Now;
                    ts = end - begin;
                    if (ts.Seconds > DetectionMotorTime) return false;
                } while (!deviceMeasure.interfacce.port.MotorIsIdel(code));
                //先复位
                deviceMeasure.interfacce.port.MotorControl(code, dir, 65535, true, speed);
                do
                {
                    Thread.Sleep(200);
                    end = DateTime.Now;
                    ts = end - begin;
                    if (ts.Seconds > DetectionMotorTime) return false;
                } while (!deviceMeasure.interfacce.port.MotorIsIdel(code));
                //正向走动1000步
                deviceMeasure.interfacce.port.MotorControl(code, direct, 1000, true, speed);
                do
                {
                    Thread.Sleep(200);
                    end = DateTime.Now;
                    ts = end - begin;
                    if (ts.Seconds > DetectionMotorTime) return false;
                } while (!deviceMeasure.interfacce.port.MotorIsIdel(code));
                //反向走动1010步
                deviceMeasure.interfacce.port.MotorControl(code, dir, 1010, true, speed);
                do
                {
                    Thread.Sleep(200);
                    end = DateTime.Now;
                    ts = end - begin;
                    if (ts.Seconds > DetectionMotorTime) return false;
                } while (!deviceMeasure.interfacce.port.MotorIsIdel(code));
                return true;
            }
            catch
            { return false; }
        }

        public bool DetectionCollimator(int code, int direct, int speed)
        {
            return true;
        }

        public bool DetectionFilter(int code, int direct, int speed)
        {
            return true;
        }

        public bool DetectionHV()
        {
            //deviceMeasure.interfacce.port.OpenPump();
            //deviceMeasure.interfacce.port.OpenVoltage();
            //Thread.Sleep(100);
            //deviceMeasure.interfacce.port.OpenVoltageLamp();
            if (!HVSettings(10))
            {
                //DetectionClosePumpLock();
                return false;
            }
            if (!HVSettings(40))
            {
                //DetectionClosePumpLock();
                return false;
            }
            //DetectionClosePumpLock();
            return true;
        }

        public void DetectionClosePumpLock()
        {
            //deviceMeasure.interfacce.port.CloseVoltageLamp();
            //Thread.Sleep(100);
            //deviceMeasure.interfacce.port.CloseVoltage();
            //deviceMeasure.interfacce.port.ClosePump();
            deviceMeasure.interfacce.CloseDevice();
        }

        public bool DetectionDetectorDemarcate(Device device)
        {
            Condition condi = Condition.FindOne(w => w.Type == ConditionType.Detection && w.Device.Id == device.Id);
            GetDetectionDetectState = false;
            DetectionDataNormal = false;
            if (condi == null) return false;
            optMode = OptMode.Detection;
            List<DeviceParameter> listParams = condi.DeviceParamList.ToList();
            this.deviceParamsList = listParams;
            initParams = condi.InitParam;
            this.deviceParamSelectIndex = 0;
            this.currentDeviceParamsList.Clear();
            this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];
            this.XrfChart.CurrentSpecPanel = 1;
            int dt = -1;
            if (!this.currentDeviceParamsList.TryGetValue(deviceMeasure.interfacce.DeviceParam.Id, out dt))
                this.currentDeviceParamsList.Add(this.deviceMeasure.interfacce.DeviceParam.Id, this.deviceMeasure.interfacce.DeviceParam.TubCurrent);
            this.deviceMeasure.interfacce.InitParam = this.initParams;
            this.deviceMeasure.interfacce.ExistMagnet = device.HasElectromagnet;
            currentTestTimes = 1;
            this.spec = new SpecEntity();
            this.spec.IsSmooth = true;
            this.specList = new SpecListEntity();
            this.specList.WorkCurveName = "";
            this.specList.Name = this.specList.SampleName = DateTime.Now.ToString("yyyyMMddHHmmss");
            this.specList.Weight = 0;
            this.specList.Operater = GP.UserName;
            this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
            this.specList.Specs[0] = spec;
            this.deviceMeasure.interfacce.Spec = this.spec;
            deviceResolve.Spec = this.spec;
            RefreshDeviceInitialize(device);
            this.deviceMeasure.interfacce.StopFlag = false;
            deviceMeasure.interfacce.OpenDevice();
            this.deviceMeasure.interfacce.KillThread(false);
            this.deviceMeasure.interfacce.MotorMove();
            for (int i = 0; i < this.deviceParamsList[deviceParamSelectIndex].PrecTime + DetectionDetecTime; i++)
            {
                if (this.deviceMeasure.interfacce.StopFlag)
                    return false;
                Thread.Sleep(1000);
                if (GetDetectionDetectState)
                {
                    return true;
                }
            }
            this.deviceMeasure.Stop();
            return false;
        }

        public void StopDetection()
        {
            this.deviceMeasure.Stop();
        }

        public bool DetectionDetector(Device device)
        {
            SelfCheckObject.Resolve = "False";
            SelfCheckObject.Peak = "False";
            SelfCheckObject.CountRate = "False";
            SelfCheckObject.PeakSec = "False";
            SelfCheckObject.HalfWidth = "False";
            Condition condi = Condition.FindOne(w => w.Type == ConditionType.Detection && w.Device.Id == device.Id);
            GetDetectionDetectState = false;
            DetectionDataNormal = false;
            if (condi == null) return false;
            optMode = OptMode.Detection;
            List<DeviceParameter> listParams = condi.DeviceParamList.ToList();
            this.deviceParamsList = listParams;
            initParams = condi.InitParam;
            this.deviceParamSelectIndex = 0;
            this.currentDeviceParamsList.Clear();
            this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];
            this.XrfChart.CurrentSpecPanel = 1;
            int dt = -1;
            if (!this.currentDeviceParamsList.TryGetValue(deviceMeasure.interfacce.DeviceParam.Id, out dt))
                this.currentDeviceParamsList.Add(this.deviceMeasure.interfacce.DeviceParam.Id, this.deviceMeasure.interfacce.DeviceParam.TubCurrent);
            this.deviceMeasure.interfacce.InitParam = this.initParams;
            this.deviceMeasure.interfacce.ExistMagnet = device.HasElectromagnet;
            currentTestTimes = 1;
            this.spec = new SpecEntity();
            this.spec.IsSmooth = true;
            this.specList = new SpecListEntity();
            this.specList.WorkCurveName = "";
            this.specList.Name = this.specList.SampleName = DateTime.Now.ToString("yyyyMMddHHmmss");
            this.specList.Weight = 0;
            this.specList.Operater = GP.UserName;
            this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
            this.specList.Specs[0] = spec;
            this.deviceMeasure.interfacce.Spec = this.spec;
            deviceResolve.Spec = this.spec;
            RefreshDeviceInitialize(device);
            this.deviceMeasure.interfacce.StopFlag = false;
            deviceMeasure.interfacce.OpenDevice();
            this.deviceMeasure.interfacce.KillThread(false);
            this.deviceMeasure.interfacce.MotorMove();
            for (int i = 0; i < this.deviceParamsList[deviceParamSelectIndex].PrecTime + DetectionDetecTime; i++)
            {
                if (this.deviceMeasure.interfacce.StopFlag)
                    return false;
                Thread.Sleep(1000);
                if (GetDetectionDetectState)
                {
                    if (DetectionDataNormal)
                        return true;
                    else
                        return false;
                }
            }
            this.deviceMeasure.Stop();
            return false;
        }

        private bool GetDetectionDetectState = false;

        private bool DetectionDataNormal = false;
        private double DetectionResollve = 0d;
        private double DetectionResollveError = 0d;
        private double DetectionPeakChannel = 0d;
        private double DetectionPeakError = 0d;
        private int DetectionMotorTime = 0;
        private int DetectionDetecTime = 0;
        private double DetectionPeakSecChannel = 0d;
        private double DetectionPeakSecError = 0d;
        private double DetectionCountRate = 0d;
        private double DetectionCountRateError = 0d;
        private double DetectionHalfWidth = 0d;
        private double DetectionHalfWidthError = 0d;

        public SelfCheckObject SelfCheckObject = null;
        private void DetectionDetectOver()
        {
            if (WorkCurveHelper.DetectionType == 0)
            {
                double resolveReal = deviceResolve.CalculateResolve();//计算分辨率
                double peakReal = this.deviceMeasure.interfacce.MaxChannelRealTime;//峰通道
                if (Math.Abs(resolveReal - DetectionResollve) < DetectionResollveError)
                    SelfCheckObject.Resolve = "Pass";
                if (Math.Abs(peakReal - DetectionPeakChannel) < DetectionPeakError)
                    SelfCheckObject.Peak = "Pass";
                if (Math.Abs(resolveReal - DetectionResollve) < DetectionResollveError && Math.Abs(peakReal - DetectionPeakChannel) < DetectionPeakError)
                {
                    DetectionDataNormal = true;
                }
                GetDetectionDetectState = true;
                deviceMeasure.interfacce.port.ClosePump();
                deviceMeasure.interfacce.port.CloseVoltage();
                deviceMeasure.interfacce.port.CloseVoltageLamp();
                WorkCurveHelper.deviceMeasure.interfacce.IsDropTime = true;
            }
            else
            {
                double halfwidthReal = double.Parse(calcResolve());//计算分辨率(半高宽)
                double peakReal = this.deviceMeasure.interfacce.MaxChannelRealTime;//峰通道
                double countrateReal = deviceMeasure.interfacce.ReturnCountRate;//计数率
                double peakSecReal = double.Parse(peakSec());//次高峰
                if (Math.Abs(halfwidthReal - DetectionHalfWidth) < DetectionHalfWidthError)
                    SelfCheckObject.HalfWidth = "Pass";
                if (Math.Abs(peakReal - DetectionPeakChannel) < DetectionPeakError)
                    SelfCheckObject.Peak = "Pass";
                if (Math.Abs(countrateReal - DetectionCountRate) < DetectionCountRateError)
                    SelfCheckObject.CountRate = "Pass";
                if (Math.Abs(peakSecReal - DetectionPeakSecChannel) < DetectionPeakSecError)
                    SelfCheckObject.PeakSec = "Pass";
                if ((Math.Abs(halfwidthReal - DetectionHalfWidth) < DetectionHalfWidthError || DetectionHalfWidth == 0 || DetectionHalfWidthError == 0)
                    && (Math.Abs(peakReal - DetectionPeakChannel) < DetectionPeakError || DetectionPeakChannel == 0 || DetectionPeakError == 0)
                    && (Math.Abs(countrateReal - DetectionCountRate) < DetectionCountRateError || DetectionCountRate == 0 || DetectionCountRateError == 0)
                    && (Math.Abs(peakSecReal - DetectionPeakSecChannel) < DetectionPeakSecError || DetectionPeakSecChannel == 0 || DetectionPeakSecError == 0))
                {
                    DetectionDataNormal = true;
                }
                GetDetectionDetectState = true;
                deviceMeasure.interfacce.port.ClosePump();
                deviceMeasure.interfacce.port.CloseVoltage();
                deviceMeasure.interfacce.port.CloseVoltageLamp();
                WorkCurveHelper.deviceMeasure.interfacce.IsDropTime = true;
            }
        }

        public string calcResolve()
        {
            if (this.spec.SpecDatas == null || this.spec.SpecDatas.Length == 0) return "0";
            int low = 0;
            int high = 0;
            int[] spec = this.spec.SpecDatas;
            double ch = SpecHelper.FitChannOfMaxValue(0, spec.Length - 1, spec, ref low, ref high);
            double halfValue = spec[(int)ch] * 1.0 / 2;
            //double slope = DemarcateEnergyHelp.k1 * 1000;
            ////求半高宽的精确边界
            if ((low - 1) < 0 || low < 0 || (high - 1) < 0 || high < 0) return "0";
            double L = low + (halfValue - spec[low]) / (spec[low + 1] - spec[low]);
            double H = high - (halfValue - spec[high]) / (spec[high - 1] - spec[high]);
            //return ((H - L) * slope);
            return (H - L).ToString("f1");
        }
        public string peakSec()
        {
            int[] specData = DifferenceDevice.interClassMain.deviceMeasure.interfacce.Spec.SpecDatas;
            if (specData == null || specData.Length == 0) return "0";
            double[] data = Helper.ToDoubles(specData);
            int[] peakPositions = QualeElementOperation.Find(data, QualeElement.ChannFWHM, QualeElement.WindowWidth, QualeElement.Trh1, QualeElement.ValleyDistance, QualeElement.AreaLimt);
            int peakFirst = StorelyPoisition(data, peakPositions);
            if (peakFirst == -1) return "0";
            data[peakFirst] = 0;
            int peakSec = StorelyPoisition(data, peakPositions);
            if (peakSec == -1) return "0";
            double fitpeak = fitPeakSec(peakSec, specData);
            return fitpeak.ToString("f1");
        }
        private QualeElement QualeElement;
        private int StorelyPoisition(double[] data, int[] peakPositions)
        {
            int storelyPosition = -1;
            for (int i = 0; i < peakPositions.Length; i++)
            {
                if (data[peakPositions[i]] <= 0)
                    continue;
                if (storelyPosition < 0)
                {
                    storelyPosition = peakPositions[i];
                    continue;
                }
                if (data[peakPositions[i]] > data[storelyPosition])
                {
                    storelyPosition = peakPositions[i];
                }
            }
            return storelyPosition;
        }

        private double fitPeakSec(int peak, int[] Data)
        {
            if (Data == null || peak > Data.Length - 1 || peak <= 0)
                return 0;
            int value = Data[peak] / 2;
            int high = peak;
            int low = peak;
            for (int i = peak + 1; i < Data.Length; i++)
            {
                if (Data[i] <= value)
                {
                    high = i;
                    break;
                }
            }
            for (int i = peak - 1; i > 0; i--)
            {
                if (Data[i] <= value)
                {
                    low = i;
                    break;
                }
            }
            Int64 pa = 0;
            Int64 pb = 0;
            for (int i = low; i <= high; i++)
            {
                pa += Data[i] * i;
                pb += Data[i];
            }
            if (pb <= 0)
            {
                return 0;
            }
            else
            {
                return pa * 1.0 / pb;
            }
        }

        public bool DetectionVacuumPump(Device device)
        {
            Condition condi = Condition.FindOne(w => w.Type == ConditionType.Detection && w.Device.Id == device.Id);
            if (condi == null) return false;
            DateTime start;
            DateTime end;
            TimeSpan ts;
            deviceMeasure.interfacce.port.OpenPump();
            start = DateTime.Now;
            do
            {
                end = DateTime.Now;
                ts = end - start;
                if (ts.Seconds > 30)
                {
                    return false;
                }
                Thread.Sleep(1000);
                deviceMeasure.interfacce.GetReturnParams();

            } while (deviceMeasure.interfacce.ReturnVacuum > condi.DeviceParamList[0].VacuumDegree);

            return true;
        }

        public bool HVSettings(int hvValue)
        {
            int intVolgate = 0, intCurrent = 0, iTemp = 0, iVacuum = 0, iCover = 0;
            double uVoltage = 0, uCurrent = 0;
            bool iCoverClose = false;
            int returnVol = 0;
            if (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
                deviceMeasure.interfacce.port.setParam(hvValue / WorkCurveHelper.DeviceCurrent.VoltageScaleFactor, 50 / WorkCurveHelper.DeviceCurrent.CurrentScaleFactor);
            else deviceMeasure.interfacce.port.SetParam((int)(hvValue / WorkCurveHelper.DeviceCurrent.VoltageScaleFactor), 0, 0, 0);
            deviceMeasure.interfacce.port.OpenVoltage();
            Thread.Sleep(100);
            deviceMeasure.interfacce.port.OpenVoltageLamp();
            Thread.Sleep(900);
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                if (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
                {
                    deviceMeasure.interfacce.port.getParam(out uVoltage, out uCurrent, out iCover);
                    returnVol += (int)(uVoltage * WorkCurveHelper.DeviceCurrent.VoltageScaleFactor);
                }
                else
                {
                    deviceMeasure.interfacce.port.GetParams(ref intVolgate, ref intCurrent, ref iTemp, ref iVacuum, ref iCoverClose);
                    returnVol += (int)((intVolgate * 50 / 255) * WorkCurveHelper.DeviceCurrent.VoltageScaleFactor);
                }
            }
            if (!((returnVol / 5) >= hvValue * 0.9 && (returnVol / 5) <= hvValue * 1.05))
            {
                return false;
            }
            return true;
        }

        public virtual void DeviceChangeProcess()
        {
            this.specList = new SpecListEntity();
            this.selectSpeclist.Clear();
            if (DifferenceDevice.IsXRF) this.refreshFillinof.RefreshDevice();
            if (WorkCurveHelper.WorkCurveCurrent == null || WorkCurveHelper.WorkCurveCurrent.ElementList == null)
            {
                this.refreshFillinof.RefreshTarget();
                this.refreshFillinof.ContructMeasureRefreshData(1, null);
                this.refreshFillinof.CreateContructStatis(null);
            }
            MotorInstance.prePath = null;
            if (this.XrfChart != null)
            {
                this.XrfChart.Lssi = null;
                this.XrfChart.ClearInformation();
                this.XrfChart.ClearCurve();
                this.XrfChart.IXMaxChannel = (int)WorkCurveHelper.DeviceCurrent.SpecLength;
                this.XrfChart.Reduction();
            }

        }

        /// <summary>
        /// 双击谱图执行函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrfChart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (WorkCurveHelper.MainSpecList == null) return;
                if (this.XrfChart.MasterPane.PaneList.Count == 1 && WorkCurveHelper.MainSpecList.Specs.Length > 1)
                {
                    XrfChart.MultiPanel(WorkCurveHelper.WorkCurveCurrent, WorkCurveHelper.MainSpecList, WorkCurveHelper.VirtualSpecList, this.spec, DifferenceDevice.DefaultSpecColor.ToArgb(), XrfChart.GraphPane.Chart.Fill.Color);
                    this.XrfChart.CurrentSpecPanel = 0;
                }
                else if (this.XrfChart.MasterPane.PaneList.Count == 1 && WorkCurveHelper.MainSpecList.Specs.Length == 1)
                {

                }
                else
                {
                    Graphics g = this.MainForm.CreateGraphics();
                    for (int i = 0; i < this.XrfChart.MasterPane.PaneList.Count; i++)
                    {
                        float scaleFactor = this.XrfChart.MasterPane.CalcScaleFactor();
                        RectangleF rect = this.XrfChart.MasterPane.PaneList[i].CalcClientRect(g, scaleFactor);
                        PointF pf = new PointF((float)e.X, (float)e.Y);
                        MultiPannelDoubleProcess(rect, pf, i, g);
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.Show(ex.Message);
            }
        }

        /// <summary>
        /// 当前谱图中存在多个谱数据图，选择其中一个激活
        /// <param name="g"></param>
        /// <param name="pf"></param>
        /// <param name="i"></param>
        /// <param name="rect"></param>
        /// </summary>
        private void MultiPannelDoubleProcess(RectangleF rect, PointF pf, int i, Graphics g)
        {
            if (rect.Contains(pf))
            {
                WorkCurveHelper.CurrentSpec = WorkCurveHelper.MainSpecList.Specs[i];
                List<SpecEntity> lsp = new List<SpecEntity>();
                for (int j = 0; j < WorkCurveHelper.VirtualSpecList.Count; j++)
                {
                    try
                    {
                        SpecEntity spec = WorkCurveHelper.VirtualSpecList[j].Specs.First(x => x.DeviceParameter.Name == WorkCurveHelper.CurrentSpec.DeviceParameter.Name);
                        lsp.Add(spec);
                    }
                    catch (Exception) { continue; }
                }
                this.spec = WorkCurveHelper.CurrentSpec;
                this.XrfChart.CurrentSpecPanel = i + 1;

                string[] names;
                WorkCurveHelper.PanelSpecIndex = i;
                AtomNamesDic.TryGetValue(i, out names);
                int[] lines;
                AtomLinesDic.TryGetValue(i, out lines);
                WorkCurveHelper.Atoms = names;
                WorkCurveHelper.Lines = lines;
                this.XrfChart.DemarcateEnergys = (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys.Count > 0) ? WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys.ToList() : Default.ConvertFromNewOld(this.specList.DemarcateEnergys.ToList(), WorkCurveHelper.DeviceCurrent.SpecLength);
                this.refreshFillinof.RefreshSpec(this.specList, this.spec);
                this.XrfChart.ShowAloneSpec(WorkCurveHelper.WorkCurveCurrent, WorkCurveHelper.MainSpecList.Specs[i], WorkCurveHelper.VirtualSpecList, false, DifferenceDevice.DefaultSpecColor.ToArgb());
            }
        }

        public IRefreshFillInfo refreshFillinof;

        //public bool IsClickWorkregion = false;
        /// <summary>
        /// 谱图移动相应的容器变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool xrfChart_MouseMoveEvent(ZedGraph.ZedGraphControl sender, MouseEventArgs e)
        {
            refreshFillinof.RefreshQuality();
            return true;
        }

        public abstract void CaculateExcute(bool flag, bool IsAddHistory);

        public virtual void RefreshHistoryRecord(SpecListEntity specList, WorkCurve workCurve)
        {
            if (specList == null || workCurve == null)
                return;
            HistoryRecord history = HistoryRecord.New;
            string sorder = "1";

            if (WorkCurveHelper.IsLiteralityToDay)
            {
                string sql = "select case when ifnull(max(historyrecordcode),0)=0 then '1' else substr(max(historyrecordcode),-4,4)+1 end code " +
                          " from historyrecord where  historyrecordcode like '" + DateTime.Now.ToString("yyyyMMdd") + "-%'";
                DataSet ds = Lephone.Data.DbEntry.Context.ExecuteDataset(sql);
                if (ds != null && ds.Tables.Count == 1) sorder = ds.Tables[0].Rows[0][0].ToString();
                history.HistoryRecordCode = DateTime.Now.ToString("yyyyMMdd") + "-" + sorder.PadLeft(4, '0');
            }
            else
            {
                string sql = "select case when ifnull(max(historyrecordcode),0)=0 then '1' else max(substr(historyrecordcode,8,10)*1)+1 end code " +
                             " from historyrecord where  historyrecordcode like '" + DateTime.Now.ToString("yyyyMM") + "-%'";
                DataSet ds = Lephone.Data.DbEntry.Context.ExecuteDataset(sql);
                if (ds != null && ds.Tables.Count == 1) sorder = ds.Tables[0].Rows[0][0].ToString();
                history.HistoryRecordCode = DateTime.Now.ToString("yyyyMM") + "-" + sorder;

            }
            //区别选择谱文件，保存到历史记录时，样品名称最后全部为0
            string strcurrentTestTimes = currentTestTimes.ToString();
            if (currentTestTimes == 0 && this.selectSpeclist != null && this.selectSpeclist.Count > 0)
            {
                foreach (SpecListEntity slist in this.selectSpeclist)
                {
                    if (slist == specList && specList.Name.IndexOf("_") != -1)
                    {
                        try
                        {
                            strcurrentTestTimes = Convert.ToString(int.Parse(specList.Name.Split('_')[specList.Name.Split('_').Length - 1]));
                        }
                        catch
                        {
                            strcurrentTestTimes = currentTestTimes.ToString();
                        }
                        //strcurrentTestTimes = specList.Name.Split('_')[specList.Name.Split('_').Length - 1];
                        break;
                    }
                }
            }
            //history.SampleName = (DifferenceDevice.interClassMain.openSpecFlag)?specList.Name:specList.SampleName + ((int.Parse(strcurrentTestTimes) > 1) ? "_" + strcurrentTestTimes : "");
            history.SampleName = specList.SampleName;

            //history.SpecListId = specList.Id;
            history.SpecListName = specList.Name;
            if (specList.SpecDate.HasValue)
                history.SpecDate = specList.SpecDate.Value;

            history.SpecSummary = specList.SpecSummary;
            history.Shape = specList.Shape;
            history.Operater = specList.Operater;
            history.Supplier = specList.Supplier;

            if (specList.Weight.HasValue)
                history.Weight = specList.Weight.Value;
            history.ActualCurrent = specList.ActualCurrent;
            history.ActualVoltage = specList.ActualVoltage;
            history.CountRate = specList.CountRate;
            history.PeakChannel = specList.PeakChannel;
            history.Resole = specList.Resole;
            history.TotalCount = specList.TotalCount;
            history.WorkCurveId = workCurve.Id;
            history.DeviceName = workCurve.Condition.Device.Name;
            history.CaculateTime = DateTime.Now;
            history.EditionType = WorkCurveHelper.EditionType;
            history.FilePath = WorkCurveHelper.SelectSpectrumPath;
            history.DeviceName = workCurve.Condition.Device.Name;
            history.CaculateTime = DateTime.Now;
            history.Height = specList.Height;
            history.CalcAngleHeight = specList.CalcAngleHeight;
            // history.AreaDensity = workCurve.ElementList.AreaDensity.Round(WorkCurveHelper.ThickBit);
            //if (WorkCurveHelper.DeviceFunctype == FuncType.XRF && WorkCurveHelper.CategoryCurrent != null && workCurve.ElementList.IsReportCategory)
            //{
            //    SpecificationsExample examples = WorkCurveHelper.MatchSpecifications(workCurve.ElementList);
            //    history.Specifications = examples.ExampleName;
            //}
            if (WorkCurveHelper.DeviceFunctype == FuncType.XRF && workCurve.ElementList.IsReportCategory)
            {
                //string grade = GetNTGrade(WorkCurveHelper.WorkCurveCurrent, DifferenceDevice.param, DifferenceDevice.gradeNTName, DifferenceDevice.gradeNTNum, DifferenceDevice.MatchNum);
                string grade = QueryGrade();
                history.Specifications = grade;
            }

            SpecEntity spec = specList.Specs[0] == null ? null : specList.Specs[0];

            if (openSpecFlag) history.IsScan = true;
            //历史记录保存前，先取值
            listHistoryElement.Clear();
            if ((WorkCurveHelper.HistoryAverageRows > 1) && (currentTestTimes > 1))
            {

                int rows = WorkCurveHelper.HistoryAverageRows - 1;
                if (rows >= currentTestTimes)
                    rows = currentTestTimes - 1;
                listHistoryElement = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id in (select id from historyrecord  where workcurveid =" + workCurve.Id.ToString() + " order by specdate desc limit " + rows.ToString() + ")");
            }
            //if (WorkCurveHelper.IsCarrayMatchPKSetting && currentTestTimes == 2)
            //{
            //    lstFirstResultByPk = HistoryElement.FindBySql("select * from historyelement where HistoryRecord_Id =( select id from historyrecord order by specdate desc limit 1)");
            //}
            history.Save();
            SaveElementsMeasure(history, workCurve, spec);
            selePrintObjectL.Add(new PrintObject(specList, workCurve, 1, history.Id));



            SaveCompanyOthersInfo(history, this.CurveTest);


            if (DifferenceDevice.CurCameraControl.ChkRetest.Checked)
            {
                MethodInfo myMethod = WorkCurveHelper.curFrmThickType.GetMethod("delelteRetestRecord");
                myMethod.Invoke(WorkCurveHelper.curFrmThick, new object[] { history.Id });
                recordList.Clear();
                foreach (long id in WorkCurveHelper.testIds) recordList.Add(id);
            }
            else
                recordList.Add(history.Id);
            //对历史记录重新进行赋值
            SaveCurrentMesureResultToExcel(history);
            if ((openSpecFlag && DifferenceDevice.interClassMain.testDevicePassedParams.MeasureParams.MeasureNumber == currentTestTimes)
                || (!openSpecFlag && DifferenceDevice.interClassMain.selectSpeclist.Count == recordList.Count))
            {

                string sSql = "";
                foreach (long hisId in recordList) sSql += hisId.ToString() + ",";
                if (sSql != "")
                {
                    sSql = "update historyelement set averagevalue=( " +
                           " select averagevalues from (select elementname,total(contextelementvalue)/count(*) as averagevalues " +
                           " from historyelement where historyrecord_id  in (" + sSql.Substring(0, sSql.Length - 1) + ") group by elementname) b where b.elementname=historyelement.elementname " +
                           " ) where   historyrecord_id in (" + sSql.Substring(0, sSql.Length - 1) + ")";
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sSql);
                }

            }

            //if (WorkCurveHelper.Efi != null && !WorkCurveHelper.IsOpenSpec)
            //{
            //    WorkCurveHelper.Efi.Execute(history);
            //}


            //recordList.Add(history);
            ////对历史记录重新进行赋值
            //foreach (var his in recordList)
            //{
            //    his.HistoryElement.ToList().ForEach(w =>
            //    {
            //        CurveElement elements = workCurve.ElementList.Items.ToList().Find(wc => w.elementName.ToLower() == ((wc.IsOxide) ? wc.Formula.ToLower() : wc.Caption.ToLower()));
            //        if (w != null && elements != null)
            //        {
            //            w.AverageValue = elements.CumulativeValue / recordList.Count;
            //            w.Save();
            //        }
            //    });
            //}
        }

        public void SaveCurrentMesureResultToExcel(HistoryRecord hr)
        {
            string folderPath = Application.StartupPath + @"\TestResult";
            if (!Directory.Exists(folderPath))
            {
                // Directory.CreateDirectory(folderPath);
                return;
            }
            Thread trd = null;
            trd = new Thread(new ThreadStart(() =>
            {
                string path = folderPath + @"\data.xls";
                int count = 0;
                bool succeed = false;
                while (!succeed && count < 100)
                {
                    try
                    {
                        if (!File.Exists(path)) File.Create(path).Close();
                        Workbook workbook = new Workbook(path);
                        workbook.Settings.Shared = true;
                        //succeed = dataGridViewW2.ExportExcel_Public_SpecialRow(path, out workbook);
                        //写入结果
                        SpecListEntity spec = DataBaseHelper.QueryByEdition(hr.SpecListName, hr.FilePath, hr.EditionType);
                        Worksheet ws = workbook.Worksheets[0];
                        WorkCurve wc = WorkCurve.FindById(hr.WorkCurveId);
                        Cells cells = ws.Cells;
                        cells.Clear();
                        cells[0, 0].PutValue(Info.SampleName);
                        cells[1, 0].PutValue(hr.SampleName);
                        //cells[0, 1].PutValue(Info.MeasureTime);
                        //cells[1, 1].PutValue(spec!=null&&spec.Specs!=null&&spec.Specs.Length>0?spec.Specs[0].UsedTime+"S":"");
                        cells[0, 1].PutValue(Info.Operator);
                        cells[1, 1].PutValue(hr.Operater);
                        cells[0, 2].PutValue(Info.WorkingCurve);
                        cells[1, 2].PutValue(wc != null ? wc.Name : "");
                        cells[0, 3].PutValue(Info.SpecDate);
                        cells[1, 3].PutValue(spec != null && spec.Specs != null ? spec.SpecDate.ToString() : "");
                        //string[] elementName = new string[wc.ElementList.Items.Count];
                        //string[] valuse = new string[wc.ElementList.Items.Count];
                        int i = 4;
                        foreach (var em in hr.HistoryElement)
                        {

                            cells[0, i].PutValue(em.elementName + (em.unitValue == 3 ? "(‰)" : (em.unitValue == 2 ? "(ppm)" : "(%)")));
                            double dblContent = 0;
                            dblContent = double.Parse(em.contextelementValue);
                            cells[1, i].PutValue(Math.Round(dblContent, WorkCurveHelper.SoftWareContentBit));
                            i++;
                            //if (wc.ElementList.CustomFields.ToList().Find(w => w.Name == em.elementName) != null || elementName.Contains(em.elementName))
                            //{
                            //    continue;
                            //}
                            //elementName[i - 4] = em.elementName ;
                            //valuse[i - 4] = em.unitValue == 3 ? (dblContent / 10).ToString() : (em.unitValue == 2 ? (dblContent / 10000).ToString() : dblContent.ToString());

                        }
                        //foreach (var custom in wc.ElementList.CustomFields)
                        //{
                        //    double value = double.Epsilon;
                        //    TabControlHelper.CustomFieldByFortum(custom.Expression, elementName, valuse, 0, out value);
                        //    cells[0, i].PutValue(custom.Name+"(%)");
                        //    cells[1, i].PutValue(Math.Round(value, WorkCurveHelper.SoftWareContentBit));
                        //    i++;
                        //}
                        List<HistoryCompanyOtherInfo> hisCompanyOtherInfoList = HistoryCompanyOtherInfo.FindBySql("select * from historycompanyotherinfo where history_id = " + hr.Id);
                        foreach (var name in hisCompanyOtherInfoList)
                        {
                            cells[0, i].PutValue(name.CompanyOthersInfo.Name);
                            cells[1, i].PutValue(name.ListInfo);
                            i++;
                        }

                        cells[0, i].PutValue(Info.Supplier);
                        cells[1, i].PutValue(hr.Supplier);
                        i++;
                        cells[0, i].PutValue(Info.Shape);
                        cells[1, i].PutValue(hr.Shape);
                        i++;
                        cells[0, i].PutValue(Info.Weight);
                        cells[1, i].PutValue(hr.Weight.ToString());
                        i++;
                        if (workbook != null)
                            workbook.Save(path);
                        break;
                    }
                    catch (IOException ex)
                    {
                        count++;
                        Thread.Sleep(100);
                    }
                }
            }));
            trd.Start();
        }
        #region 自动保存报表功能
        private void SaveExcel()
        {
            DifferenceDevice.IsAutoSaveReport = true;
            List<TreeNodeInfo> lsit = new List<TreeNodeInfo>();
            DifferenceDevice.interClassMain.SaveExcel(lsit, 0);
            DifferenceDevice.IsAutoSaveReport = false;
        }

        //public void AnalyserAutoSaveReport(string SpecListName)
        //{
        //    if (DifferenceDevice.interClassMain.reportThreadManage == null) return;            
        //    Thread thread = new Thread(new ParameterizedThreadStart(DifferenceDevice.interClassMain.reportThreadManage.GetRecordReport));
        //    WorkCurveHelper.lsThread.Add(thread);
        //    thread.ApartmentState = ApartmentState.STA;
        //    thread.Start(SpecListName);
        //}

        #endregion

        //private void SaveCompanyOthersInfo(HistoryRecord history)
        //{
        //    //新增公司其它信息
        //    if (WorkCurveHelper.SeleCompanyOthersInfo.Count > 0)
        //    {
        //        foreach (string skey in WorkCurveHelper.SeleCompanyOthersInfo.Keys)
        //        {
        //            CompanyOthersInfo comOthersInfo = CompanyOthersInfo.FindById(long.Parse(skey));

        //            CompanyOthersListInfo comOthersListInfo = null;
        //            if (comOthersInfo != null)
        //            {
        //                if (comOthersInfo.ControlType == 1 && comOthersInfo != null)
        //                {
        //                    List<CompanyOthersListInfo> comOthersInfolist = CompanyOthersListInfo.FindBySql("select * from companyotherslistinfo where companyothersinfo_id='" + comOthersInfo.Id + "' and listinfo='" + WorkCurveHelper.SeleCompanyOthersInfo[skey] + "'");
        //                    if (comOthersInfolist.Count > 0) comOthersListInfo = comOthersInfolist[0];
        //                }
        //            }

        //            string strsql = "";
        //            if (comOthersListInfo != null)
        //            {
        //                strsql = "insert into historycompanyotherinfo(workcurveid,listinfo,history_id,companyothersinfo_id,companyotherslistinfo_id) " +
        //                                " values('" + history.WorkCurveId + "','" + WorkCurveHelper.SeleCompanyOthersInfo[skey].ToString() + "','" + history.Id + "','" + comOthersInfo.Id + "','" + comOthersListInfo.Id + "')";
        //            }
        //            else
        //            {
        //                strsql = "insert into historycompanyotherinfo(workcurveid,listinfo,history_id,companyothersinfo_id,companyotherslistinfo_id) " +
        //                                " values('" + WorkCurveHelper.WorkCurveCurrent.Id + "','" + WorkCurveHelper.SeleCompanyOthersInfo[skey].ToString() + "','" + history.Id + "','" + comOthersInfo.Id + "','-1')";
        //            }

        //            Lephone.Data.DbEntry.Context.ExecuteNonQuery(strsql);
        //        }
        //    }
        //}

        private void SaveCompanyOthersInfo(HistoryRecord history, WordCureTest CurveTest)
        {
            List<CompanyOthersInfo> CompanyInfoList = null;
            //新增公司其它信息
            if (CurveTest != null && CurveTest.CompanyInfoList != null && CurveTest.CompanyInfoList.Count > 0)
            {
                CompanyInfoList = CurveTest.CompanyInfoList;
            }
            else
            {
                CompanyInfoList = CompanyOthersInfo.FindBySql("select * from companyothersinfo where 1=1 and Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "' ");
                foreach (var name in CompanyInfoList)
                {
                    name.DefaultValue = "";
                }
            }
            foreach (var comOthersInfo in CompanyInfoList)
            {
                CompanyOthersListInfo comOthersListInfo = null;
                if (comOthersInfo != null)
                {
                    if (comOthersInfo.ControlType == 1 && comOthersInfo != null)
                    {
                        List<CompanyOthersListInfo> comOthersInfolist = CompanyOthersListInfo.FindBySql("select * from companyotherslistinfo where companyothersinfo_id='" + comOthersInfo.Id + "' and listinfo='" + comOthersInfo.DefaultValue + "'");
                        if (comOthersInfolist.Count > 0) comOthersListInfo = comOthersInfolist[0];
                    }
                }
                string strsql = "";
                if (comOthersListInfo != null)
                {
                    strsql = "insert into historycompanyotherinfo(workcurveid,listinfo,history_id,companyothersinfo_id,companyotherslistinfo_id) " +
                                    " values('" + history.WorkCurveId + "','" + comOthersInfo.DefaultValue + "','" + history.Id + "','" + comOthersInfo.Id + "','" + comOthersListInfo.Id + "')";
                }
                else
                {
                    strsql = "insert into historycompanyotherinfo(workcurveid,listinfo,history_id,companyothersinfo_id,companyotherslistinfo_id) " +
                                    " values('" + WorkCurveHelper.WorkCurveCurrent.Id + "','" + comOthersInfo.DefaultValue + "','" + history.Id + "','" + comOthersInfo.Id + "','-1')";
                }

                Lephone.Data.DbEntry.Context.ExecuteNonQuery(strsql);
            }
        }

        public abstract void SaveElementsMeasure(HistoryRecord history, WorkCurve workCurve, SpecEntity spec);
        public abstract void RefrenshMoveStation(NaviItem item, ContainerObject panel);
        public IntPtr handle;
        public event EventHandler DeviceSaveMove;
        public void CommonMoveStation(NaviItem item, ContainerObject panel)
        {

            WorkCurveHelper.deviceMeasure.CreateInitalize();
            if (WorkCurveHelper.type != InterfaceType.NetWork && WorkCurveHelper.type != InterfaceType.BlueTeeth)
                MotorInstance.LoadDLL(MotorInstance.UpdateKeyFile, WorkCurveHelper.DeviceCurrent);
            RefreshDeviceInitialize(WorkCurveHelper.DeviceCurrent);
            if (DeviceSaveMove != null)
                DeviceSaveMove(null, null);
            NaviItem itemMoveStation = WorkCurveHelper.NaviItems.Find(w => w.Name == "ChamberMove");
            if (itemMoveStation != null)
            {
                if (WorkCurveHelper.DeviceCurrent.HasChamber && WorkCurveHelper.DeviceCurrent.Chamber.Count > 0)
                    itemMoveStation.Enabled = true;
                else
                    itemMoveStation.Enabled = false;
            }
            NaviItem itemConnect = WorkCurveHelper.NaviItems.Find(w => w.Name == "ConnectDevice");
            if (itemConnect != null && (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA
                || (WorkCurveHelper.DeviceCurrent.ComType == ComType.USB && WorkCurveHelper.DeviceCurrent.IsDP5 && WorkCurveHelper.DeviceCurrent.Dp5Version == Dp5Version.Dp5_FastNet)))
            {
                itemConnect.Enabled = true;
                ConnectDevice();
            }
            else
            {
                itemConnect.Enabled = false;
                this.XrfChart.ClearInformation();
            }

            // WorkCurveHelper.deviceMeasure.CreateInitalize();
            // RefreshDeviceInitialize(WorkCurveHelper.DeviceCurrent);
            this.deviceMeasure.interfacce.OwnerHandle = handle;
            this.deviceMeasure.interfacce.Spec = this.spec;
            MotorAdvance.Device = WorkCurveHelper.DeviceCurrent;
            this.refreshFillinof.RefreshTarget();

        }

        public void ReturnMainPage(ContainerObject panel)
        {
            IEnumerator ienumer = panel.Controls.GetEnumerator();
            while (ienumer.MoveNext())
            {
                Control control = ienumer.Current as Control;
                if (control.Name == "MainPage")
                    control.Visible = true;
                else
                    control.Visible = false;
            }
        }

        public void CopyCurrentWorkCurve()
        {
            if (WorkCurveHelper.WorkCurveCurrent == null)
                return;

        }

        public void SetStyle(Skyray.Controls.Style style)
        {
            this.refreshFillinof.SetStyle(style);
        }

        public System.Windows.Forms.OpenFileDialog openDialogue = new OpenFileDialog();
        public void UpdateTitleICO()
        {
            if (DifferenceDevice.TitleIco != null)
            {
                MainForm.Text = DifferenceDevice.TitleIco.Text;
                MainForm.Icon = DifferenceDevice.TitleIco.Ico;
            }
        }

        public void RefreshHistory()
        {
            this.refreshFillinof.RefreshHistory();
        }



        //是否显示对数谱
        public bool IsShowLogSpectrum = false;

        public virtual void CheckLogAdditionDraw(SpecListEntity entity) { }

        public void CheckLogSpectrum(bool flag)
        {
            IsShowLogSpectrum = flag;
            if (this.specList != null && this.specList.Specs != null && this.specList.Specs.Length == 1)
            {
                SpecListEntity LogSpecList = null;
                if (!flag)
                    LogSpecList = this.specList;
                else
                {
                    LogSpecList = (SpecListEntity)this.specList.DeepClone();
                    int[] ints = Helper.ToInts(LogSpecList.Specs[0].SpecData);
                    int[] temp = new int[ints.Length];
                    for (int i = 0; i < ints.Length; i++)
                    {
                        if (ints[i] > 0)
                            temp[i] = (int)Math.Round(Math.Log10(ints[i]) * 100);
                        else
                            temp[i] = 0;
                    }
                    LogSpecList.Specs[0].SpecData = Helper.ToStrs(temp);
                }
                XrfChart.SpecDataDic.Clear();
                if (LogSpecList.Specs.Length == 1)
                {
                    List<SpecData> listSpec = new List<SpecData>();
                    int[] specDataInt = LogSpecList.Specs[0].SpecDatas;
                    for (int m = 0; m < specDataInt.Length; m++)
                    {
                        SpecData specData = new SpecData(m, specDataInt[m]);
                        listSpec.Add(specData);
                    }
                    XrfChart.CurrentSpecPanel = 1;
                    XrfChart.SpecDataDic.Remove(0);
                    XrfChart.SpecDataDic.Add(0, listSpec);
                    XrfChart.ShowAloneSpec(WorkCurveHelper.WorkCurveCurrent, LogSpecList.Specs[0], WorkCurveHelper.VirtualSpecList, false, LogSpecList.Color);
                }
                CheckLogAdditionDraw(LogSpecList);
                this.XrfChart.Reduction();
                this.XrfChart.Refresh();
            }
        }

        //chuyaqin 2012-04-12
        /// <summary>
        /// 匹配曲线
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="spec"></param>
        /// <param name="demarcateEnergys"></param>
        /// <param name="MatchElements"></param>
        /// <param name="bIsHighSubstrate"></param>
        /// <returns></returns>
        public WorkCurve ToCatchCurveByThreeMainElements(int flag, SpecEntity spec, List<DemarcateEnergy> demarcateEnergys, ref string MatchElements, bool bIsHighSubstrate)
        {
            //AutoAnalysisProcess(null);
            //WorkCurve curve = WorkCurve.FindOne(w => w.Name == spec.WorkCurveName && w.Condition.Device.Id== WorkCurveHelper.DeviceCurrent.Id);
            //Condition condition = null;
            if (demarcateEnergys == null || demarcateEnergys.Count <= 0)
            {
                Condition condition = Condition.FindOne(w => w.Type == ConditionType.Match && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                if (condition == null || condition.DemarcateEnergys == null || condition.DemarcateEnergys.Count == 0)
                    return null;
                demarcateEnergys = condition.DemarcateEnergys.ToList();
            }


            DemarcateEnergyHelp.CalParam(demarcateEnergys);
            this.qualeElement.ToChannel2 = DemarcateEnergyHelp.GetChannel2;

            List<double> dblPeakHeights = new List<double>();
            List<string> MainElements = this.qualeElement.GetSpectrumThreeElements(ref dblPeakHeights, spec, this.XrfChart.DemarcateEnergys.ToList(), bIsHighSubstrate);

            List<WorkCurve> allWorkCurve = WorkCurve.FindBySql(@"select distinct a.* from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c 
                                    on b.Device_Id = c.Id where b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and a.FuncType =" + flag);

            if (allWorkCurve == null || allWorkCurve.Count == 0)
                return null;
            WorkCurve CurrentCurve = allWorkCurve[0];
            WorkCurve DefaultCurve = allWorkCurve.Find(w => w.IsDefaultWorkCurve == true);
            int SetCurrent = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].TubCurrent;
            WorkCurveHelper.WorkCurveCurrent.SetCurrent(SetCurrent);
            SpecListEntity temp = new SpecListEntity();
            temp.Specs = new SpecEntity[1];
            temp.Specs[0] = spec;

            // int elementCount = MainElements.Count;
            // MatchElements = string.Empty;
            //string MatchElements2 = string.Empty;
            //for (int i = 0; i < elementCount; i++)
            //{
            //    MatchElements += MainElements[i];
            //    if (i < elementCount - 1)
            //    {
            //        MatchElements += ",";
            //    }
            //}
            //MatchElements2 = MatchElements;
            List<WorkCurve> MatchWorkCurves = new List<WorkCurve>();
            if (!bIsHighSubstrate)
            {
                List<string> allElems = new List<string>();
                if (DefaultCurve != null)
                {
                    if (WorkCurveHelper.IsDelElemByQuale)
                    {
                        foreach (var ei in DefaultCurve.ElementList.Items)
                        {
                            if (!MainElements.Contains(ei.Caption))
                            {
                                DefaultCurve.ElementList.Items.Remove(ei);//删除曲线里没有被分析出来的元素。
                            }
                        }
                    }
                    //默认曲线作为通用曲线作元素筛选
                    DefaultCurve.CaculateIntensity(temp);//求强度
                    switch (WorkCurveHelper.DelElemType)
                    {
                        case 0:
                            foreach (var ei in DefaultCurve.ElementList.Items)
                            {
                                double eiIn = ei.Samples.ToList().Find(w => double.Parse(w.Y) > 0) != null ? double.Parse(ei.Samples.OrderByDescending(w => double.Parse(w.Y)).ToList()[0].X) : -1;
                                ei.Content = eiIn > 0 ? ei.Intensity / eiIn : 0;
                            }
                            break;
                        case 1:
                            DefaultCurve.CacultateContent(temp);//求含量
                            break;
                        default: break;
                    }
                    foreach (var ei in DefaultCurve.ElementList.Items)
                    {
                        if (ei.Content > WorkCurveHelper.DelElemThreshold)
                            allElems.Add(ei.Caption);
                    }
                }
                for (int i = allWorkCurve.Count - 1; i < allWorkCurve.Count && i >= 0; i--)
                {
                    if (!allWorkCurve[i].IsJoinMatch
                        || allWorkCurve[i].MainElements == null
                        || allWorkCurve[i].MainElements.Trim() == string.Empty
                        || allWorkCurve[i].MainElements.ToUpper().Contains("BG"))
                    {
                        allWorkCurve.RemoveAt(i);
                        continue;
                    }
                    foreach (var em in allElems)
                    {
                        var tempem = allWorkCurve[i].ElementList.Items.ToList().Find(w => w.Caption == em);
                        if (tempem != null) continue;
                        allWorkCurve.RemoveAt(i);
                        break;
                    }
                }
                CurrentCurve = null;
            }
            else
            {
                foreach (WorkCurve workCurve in allWorkCurve)
                {
                    if (!workCurve.IsJoinMatch || workCurve.MainElements == null || workCurve.MainElements == string.Empty)
                    {
                        continue;
                    }
                    string[] curveMainElemnt = workCurve.MainElements.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!curveMainElemnt.Contains("BG"))
                    {
                        continue;
                    }
                    MatchWorkCurves.Add(workCurve);
                }
                allWorkCurve.Clear();
                for (int j = 0; j < MatchWorkCurves.Count; j++)
                {
                    allWorkCurve.Add(MatchWorkCurves[j]);
                }
                MatchWorkCurves.Clear();
                CurrentCurve = null;
            }
            // 次要元素的筛选
            double MinorContent = 0;
            for (int count = 0; count < dblPeakHeights.Count; count++)
            {
                dblPeakHeights[count] /= dblPeakHeights[0];
                if (count != 0)
                {
                    MinorContent += dblPeakHeights[count];
                }
            }

            if (dblPeakHeights.Count > 1 && MinorContent / dblPeakHeights[0] < WorkCurveHelper.MatchMinorElemRatio && !bIsHighSubstrate)
            {
                for (int count = dblPeakHeights.Count - 1; count > 0; count--)
                {
                    dblPeakHeights.RemoveAt(count);
                    MainElements.RemoveAt(count);
                }
            }
            //删除多余三个的元素
            int eli = MainElements.Count - 1;
            while (eli > 2)
            {
                MainElements.RemoveAt(eli);
                dblPeakHeights.RemoveAt(eli);
                eli--;
            }

            int elementCount = MainElements.Count;
            MatchElements = string.Empty;
            for (int i = 0; i < elementCount; i++)
            {
                MatchElements += MainElements[i];
                if (i < elementCount - 1)
                {
                    MatchElements += ",";
                }
            }
            if (allWorkCurve.Count <= 0)
            {
                return DefaultCurve;
            }

            List<double> dblCurveMathCoefs = new List<double>();
            int[] ElemntT = { 5, 2, 1 };
            int maxid = 0;
            int curveCount = 0;
            foreach (WorkCurve workCurve in allWorkCurve)
            {
                if (!workCurve.IsJoinMatch)
                {
                    continue;
                }
                string[] curveMainElemnt = workCurve.MainElements.Replace("BG;", "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                //string[] curveMainElemnt = workCurve.MainElements.Replace("BG,", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                double curveCoefs = 0;
                for (int i = 0; i < elementCount; i++)
                {
                    for (int j = 0; j < curveMainElemnt.Length; j++)
                    {
                        string[] elementInfo = curveMainElemnt[j].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        //if (curveMainElemnt[j].Length > 0 && MainElements[i].ToUpper().CompareTo(curveMainElemnt[j].ToUpper()) == 0)
                        if (elementInfo.Length > 0 && MainElements[i].ToUpper().CompareTo(elementInfo[0].ToUpper()) == 0)
                        {
                            //curveCoefs += dblPeakHeights[i] * ElemntT[j];
                            //curveCoefs += dblPeakHeights[i] * (elementInfo.Length > 1 && elementInfo[1].Trim() != string.Empty ? double.Parse(elementInfo[1]) : (5-j));
                            //curveCoefs += (3 - i) * (4.5- j*1.5);
                            //curveCoefs += ElemntT[i] * ElemntT[j];
                            double tempElementT = ElemntT[j];//防止输入字符串错误
                            try
                            {
                                tempElementT = elementInfo.Length > 1 && elementInfo[1].Trim() != string.Empty ? double.Parse(elementInfo[1]) : ElemntT[j];
                            }
                            catch
                            {
                                tempElementT = ElemntT[j];
                            }
                            curveCoefs += ElemntT[i] * tempElementT;
                        }
                    }
                }
                MatchWorkCurves.Insert(curveCount, workCurve);
                dblCurveMathCoefs.Insert(curveCount, curveCoefs);
                if (dblCurveMathCoefs[curveCount] >= dblCurveMathCoefs[maxid])
                {
                    maxid = curveCount;
                }
                curveCount++;
            }
            for (int i = 0; i < curveCount; i++)
            {
                string[] curveMainElemnt = MatchWorkCurves[i].MainElements.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (!bIsHighSubstrate
                    && dblCurveMathCoefs[maxid] == dblCurveMathCoefs[i]
                    && curveMainElemnt.Length == MainElements.Count
                    && dblCurveMathCoefs[i] > 0)//特征元素完全相同的情况
                {
                    CurrentCurve = MatchWorkCurves[i];
                    return CurrentCurve;
                }
                else if (bIsHighSubstrate && curveMainElemnt.Length == 1 && curveMainElemnt[0].Contains("BG") && dblCurveMathCoefs[maxid] <= 0)//塑料
                {
                    CurrentCurve = MatchWorkCurves[i];
                    return CurrentCurve;
                }
            }
            if (MatchWorkCurves.Count > 0 && dblCurveMathCoefs[maxid] > 0)
            {
                CurrentCurve = MatchWorkCurves[maxid];
            }
            if (CurrentCurve == null)
            {
                CurrentCurve = DefaultCurve;
            }
            return CurrentCurve;
        }

        /// <summary>
        /// 判断有无背景
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="demarcateEnergys"></param>
        /// <returns>返回-1是求面积错误，1,合金(没背景)。2,轻金属(有背景)</returns>
        public int CatchBGTypge(SpecEntity spec, List<DemarcateEnergy> demarcateEnergys)
        {
            if (demarcateEnergys == null || demarcateEnergys.Count <= 0)
            {
                Condition condition = Condition.FindOne(w => w.Type == ConditionType.Match && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                if (condition == null || condition.DemarcateEnergys == null || condition.DemarcateEnergys.Count == 0)
                    return -1;
                demarcateEnergys = condition.DemarcateEnergys.ToList();
            }

            DemarcateEnergyHelp.CalParam(demarcateEnergys);
            this.qualeElement.ToChannel2 = DemarcateEnergyHelp.GetChannel2;
            return this.qualeElement.IsHaveBG(demarcateEnergys, spec) == true ? 2 : 1;
        }
        //public SpecListEntity OpenFileDialog(bool isCaculate)
        //{
        //    SpecListEntity initEntity = null;
        //    if (this.openDialogue.ShowDialog() == DialogResult.OK)
        //    {
        //        SpecListEntity tt = WorkCurveHelper.DataAccess.Query(this.openDialogue.SafeFileName.Replace(".Spec",""));
        //        if (tt == null)
        //            return initEntity;
        //        if (isCaculate)
        //        {
        //            List<SpecListEntity> returnTT = new List<SpecListEntity>();
        //            returnTT.Add(tt);
        //            OpenWorkSpec(returnTT);
        //        }
        //        return tt;
        //    }
        //    return initEntity;
        //}

        //public S void OpenOldSpec()
        //{
        //    if (WorkCurveHelper.WorkCurveCurrent == null)
        //        return;
        //    if (openDialogue.ShowDialog() == DialogResult.OK)
        //    {
        //ImputData data = new ImputData();
        //SpecList openSpec = data.SampleImport(openDialogue.FileName, WorkCurveHelper.WorkCurveCurrent.Condition);
        //SpecList openSpec = DataInputHelper.CreateNewSpecListFromOld(openDialogue.FileName);
        //this.selectSpeclist.Clear();
        //this.selectSpeclist.Add(openSpec);
        //OpenWorkSpec(selectSpeclist);
        //}
        ////}

        //public static void CaculateStaticsDataByDt(ref double MinValue, ref double MaxValue, ref double AvgValue,
        //            ref double SdValue, DataTable dt, int beginRow, int testCurrentTimes, string colName)
        //{
        //    if (dt == null)
        //        return;
        //    double cell, sum, sum2, min, max, warp;
        //    if (dt.Rows[beginRow][colName] != null)
        //    {
        //        cell = Convert.ToDouble(dt.Rows[beginRow][colName].ToString());
        //        min = cell;
        //        max = min;
        //        sum = min;
        //        sum2 = cell * cell;
        //        for (int row = beginRow + 1; row < testCurrentTimes + beginRow; row++)
        //        {
        //            cell = Convert.ToDouble(dt.Rows[row][colName].ToString());
        //            if (min > cell)
        //            {
        //                min = cell;
        //            }
        //            else if (max < cell)
        //            {
        //                max = cell;
        //            }
        //            sum += cell;
        //            sum2 += cell * cell;
        //        }
        //        MinValue = min; //最小值
        //        MaxValue = max; //最大值
        //        sum /= testCurrentTimes;
        //        AvgValue = sum; //求和
        //        if (testCurrentTimes > 1)
        //        {
        //            warp = Math.Sqrt(Math.Abs(sum2 - sum * sum * testCurrentTimes) / (testCurrentTimes - 1));
        //            SdValue = warp;
        //        }
        //        else
        //        {
        //            SdValue = 0d;
        //        }
        //    }
        //}

        //public static void CaculateStaticsData(ref string MinValue, ref string MaxValue, ref string AvgValue,
        //            ref string SdValue, double Measure, int testCurrentTimes, string colName)
        //{
        //    //if (Measure == null)
        //    //    return;
        //    double cell, sum, sum2, min, max, warp;
        //    //if (Measure != null)
        //    //{
        //        cell = Convert.ToDouble(Measure.ToString());
        //        min = cell;
        //        max = min;
        //        sum = min;
        //        sum2 = cell * cell;
        //        for (int row = 1; row < testCurrentTimes; row++)
        //        {
        //            cell = Convert.ToDouble(Measure.ToString());
        //            if (min > cell)
        //            {
        //                min = cell;
        //            }
        //            else if (max < cell)
        //            {
        //                max = cell;
        //            }
        //            sum += cell;
        //            sum2 += cell * cell;
        //        }
        //        MinValue = min.ToString("f"+WorkCurveHelper.SoftWareContentBit.ToString()); //最小值
        //        MaxValue = max.ToString("f"+WorkCurveHelper.SoftWareContentBit.ToString()); //最大值
        //        sum /= testCurrentTimes;
        //        AvgValue = sum.ToString("f"+WorkCurveHelper.SoftWareContentBit.ToString()); //求和
        //        if (testCurrentTimes > 1)
        //        {
        //            warp = Math.Sqrt(Math.Abs(sum2 - sum * sum * testCurrentTimes) / (testCurrentTimes - 1));
        //            SdValue = warp.ToString("f"+WorkCurveHelper.SoftWareContentBit.ToString());
        //        }
        //        else
        //        {
        //            SdValue = string.Empty;
        //        }
        //    //}


        //}

        #region 打印新模板功能
        public static List<PrintObject> selePrintObjectL = new List<PrintObject>();//存放选择的需要打印的对象

        public static List<DataFountain> dataFountainList = null;//生成的打印模板对象

        public static int seledataFountain = 0;//根据此参数，获取是单次模板还是多次模板，如果等于1则为单次模板，如果大于1则采用多次模板

        public static bool isMulitTest = false;//判断是否为多次连测，如果为多次连测，则为true

        private static List<string> interSeleElement = null;//保存历史记录中，如果显示10种元素，而客户只显示3种元素，在打印中只需要显示3种元素


        public static bool SetPrintTemplate(List<string> seleElement, List<PrintObject> seleHistoryPrintObjectL)
        {
            bool isSucceed = true;

            List<PrintObject> CurryselePrintObject = (seleHistoryPrintObjectL == null) ? selePrintObjectL : seleHistoryPrintObjectL;

            if (CurryselePrintObject.Count == 0) return false;

            //修改：何晓明 20111129 报表名称设置 加载当前第一条记录谱对象和工作曲线
            EDXRFHelper.FirstSpectList = CurryselePrintObject[0].specList;
            EDXRFHelper.FirstWorkCurve = CurryselePrintObject[0].workCurve;
            EDXRFHelper.FirstHistoryRecordId = CurryselePrintObject.FirstOrDefault() == null ? -1 : CurryselePrintObject.FirstOrDefault().historyRecordId;
            //
            interSeleElement = seleElement;

            string strseleelement = "";
            //if (interSeleElement != null && interSeleElement.Count > 0)
            //foreach (string strele in interSeleElement) strseleelement += "'"+strele+"',";
            //strseleelement = "'" + string.Join("',", interSeleElement.ToArray());
            dataFountainList = new List<DataFountain>();
            int order = 0;
            foreach (PrintObject printObject in CurryselePrintObject)
            {
                if (DifferenceDevice.IsRohs && seleHistoryPrintObjectL == null
                    && WorkCurveHelper.WorkCurveCurrent.WorkRegion.Id != printObject.workCurve.WorkRegion.Id)
                    continue;
                List<int> orderl = new List<int>();
                if (printObject.printType == 1)
                {
                    DataFountain dataFountain = new DataFountain();
                    HistoryRecord historyRecord = HistoryRecord.FindById(printObject.historyRecordId);
                    if (historyRecord == null) return false;
                    //List<HistoryElement> historyElementList = new List<HistoryElement>();
                    List<CurveElement> lc = printObject.workCurve.ElementList.Items.OrderBy(a => a.RowsIndex).ToList();
                    var test = from tt in lc select tt.Caption;
                    strseleelement = "'" + string.Join("','", test.ToArray()) + "'";
                    List<HistoryElement> thel = HistoryElement.FindBySql(
                        " select a.* from  historyelement a where a.historyrecord_id=" + historyRecord.Id + ((strseleelement == "") ? "" : " and a.elementname in (" + strseleelement + ")"));
                    //foreach (var ele in lc)
                    //{
                    //    historyElementList.Add(thel.Find(w => w.elementName == ele.Caption));
                    //foreach (var he in thel)
                    //{
                    //    if (ele.Caption.Equals(he.elementName))
                    //    {
                    //        historyElementList.Add(he);
                    //    }
                    //}

                    //}
                    order++;
                    orderl.Add(order);
                    SetRecord(historyRecord, thel, order, printObject.specList, printObject.workCurve, orderl, ref dataFountain);
                    dataFountainList.Add(dataFountain);
                }
                else if (printObject.printType == 2)
                {
                    List<HistoryRecord> historyRecordList = HistoryRecord.FindBySql("select * from historyrecord where id in( select HistoryId from continuousresult where continuousNumber in(select continuousNumber from continuousresult where id=" + printObject.continuousresultId.ToString() + "))");
                    if (historyRecordList == null) return false;

                    int iorder = order;
                    for (int i = 0; i < historyRecordList.Count; i++)
                    {
                        iorder++;
                        orderl.Add(iorder);
                    }
                    foreach (HistoryRecord historyRecord in historyRecordList)
                    {
                        DataFountain dataFountain = new DataFountain();
                        //string sql = "select * from SpecList a inner join Condition b on a.Condition_Id = b.Id inner join Device d on b.Device_Id=d.Id where a.Name='" + historyRecord.SpecListName + "' and b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id;
                        SpecListEntity tempList = DataBaseHelper.QueryByEdition(historyRecord.SpecListName, historyRecord.FilePath, historyRecord.EditionType);
                        if (tempList == null) return false;
                        //SpecList curremtSpec = tempList;

                        WorkCurve workCurve = WorkCurve.FindById(historyRecord.WorkCurveId);
                        if (workCurve == null) return false;
                        List<HistoryElement> historyElementList = new List<HistoryElement>();
                        List<CurveElement> lc = workCurve.ElementList.Items.OrderBy(a => a.RowsIndex).ToList();
                        List<HistoryElement> thel = HistoryElement.FindBySql(" select * from  historyelement where historyrecord_id=" + historyRecord.Id.ToString() + ((strseleelement == "") ? "" : " and elementname in (" + strseleelement + ")"));
                        foreach (var ele in lc)
                        {
                            foreach (var he in thel)
                            {
                                if (ele.Caption.Equals(he.elementName))
                                {
                                    historyElementList.Add(he);
                                }
                            }
                        }
                        order++;
                        SetRecord(historyRecord, historyElementList, order, tempList, workCurve, orderl, ref dataFountain);
                        dataFountainList.Add(dataFountain);
                    }
                }
            }
            GetDataFountain getDataFountain = new GetDataFountain(dataFountainList);
            DirectPrintLibcs.lst = getDataFountain.GetSource();

            if (DirectPrintLibcs.lst == null) return false;

            var query = from cont in dataFountainList
                        group cont by cont.ContinuousList;
            seledataFountain = 0;
            foreach (var contdf in query)
                seledataFountain++;

            isMulitTest = false;
            return isSucceed;
        }


        private static void SetRecord(HistoryRecord historyRecord, List<HistoryElement> historyElementList,
            int order, SpecListEntity specList, WorkCurve workCurve, List<int> orderl, ref DataFountain dataFountain)
        {
            dataFountain.workcCurrent = workCurve;
            //string Edition = "";
            //if (DifferenceDevice.IsAnalyser) Edition = "EDXRF";
            //else if (DifferenceDevice.IsRohs) Edition = "Rohs";
            //else if (DifferenceDevice.IsThick) Edition = "FPThick";
            //else if (DifferenceDevice.IsXRF) Edition = "XRF";
            dataFountain.curreEdition = ReportTemplateHelper.Edition;
            dataFountain.specList = specList;
            dataFountain.order = order;
            dataFountain.ContinuousList = orderl;
            dataFountain.historyRecord = historyRecord;
            dataFountain.historyElementList = historyElementList;
            CustomStandard customStandard = null;
            #region 标准库
            if ((DifferenceDevice.IsRohs || WorkCurveHelper.isShowXRFStandard == 1 || DifferenceDevice.IsThick) && historyElementList.Exists(delegate(HistoryElement v) { return v.customstandard_Id != 0; }))
            {
                dataFountain.LStandardData = StandardData.FindBySql("select * from standarddata  where  customstandard_id=" + historyElementList.Find(delegate(HistoryElement v) { return v.customstandard_Id != 0; }).customstandard_Id.ToString());
                customStandard = CustomStandard.FindById(historyElementList.Find(delegate(HistoryElement v) { return v.customstandard_Id != 0; }).customstandard_Id);
            }
            #endregion

            #region 获取测量结果
            List<ElemTestResult> ElemTestResultL = new List<ElemTestResult>();



            if (DifferenceDevice.IsThick)
            {
                GetThickTestResult(workCurve, customStandard, specList, ref ElemTestResultL, historyRecord, historyElementList);
            }

            dataFountain.ElemTestResultL = ElemTestResultL;

            #endregion

            #region 获取统计信息
            //List<StatInfo> LStatInfo = new List<StatInfo>();
            //GetStatInfo(dataFountain.workcCurrent, ref LStatInfo);
            //dataFountain.LStatInfo = LStatInfo;
            #endregion


            #region 谱图
            //SpecAdditional additianlImage = SpecAdditional.FindOne(w => w.SpecListId == specList.Id);
            //if (additianlImage != null)
            //    dataFountain.ByteSpecData = additianlImage.GraphicData;
            if (dataFountain.specList != null && !dataFountain.specList.ImageShow)
            {
                string fileNameFull = WorkCurveHelper.SaveGraphicPath + "\\" + dataFountain.specList.Name + ".jpg";
                FileInfo infoIf = new FileInfo(fileNameFull);
                if (infoIf.Exists)
                {
                    Image bm = Image.FromFile(fileNameFull);
                    using (var ms = new System.IO.MemoryStream())
                    {
                        bm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = ms.GetBuffer();
                        dataFountain.ByteSpecData = bytes;
                    }
                }
            }
            //if (additianlImage != null)
            //    dataFountain.ByteSpecData = additianlImage.GraphicData;
            //else if (dataFountain.specList != null && !dataFountain.specList.ImageShow)
            //{
            //    string fileNameFull = WorkCurveHelper.SaveGraphicPath + "\\" + dataFountain.specList.Id + ".jpg";
            //    FileInfo infoIf = new FileInfo(fileNameFull);
            //    if (infoIf.Exists)
            //    {
            //        Image bm = Image.FromFile(fileNameFull);
            //        using (var ms = new System.IO.MemoryStream())
            //        {
            //            bm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //            byte[] bytes = ms.GetBuffer();
            //            dataFountain.ByteSpecData = bytes;
            //        }
            //    }
            //}
            //修改：何晓明 20110831 dataFountain.specList.Specs.Count == 0
            if (dataFountain.specList.Specs.Length > 0)
                dataFountain.SpecData = dataFountain.specList.Specs.ToList();
            //dataFountain.SpecData = dataFountain.specList.Specs[0];


            #endregion


            #region 样品图
            if (dataFountain.specList != null && dataFountain.specList.ImageShow)
            {
                string fileNameFull = WorkCurveHelper.SaveSamplePath + "\\" + dataFountain.specList.Name + ".jpg";
                FileInfo infoIf = new FileInfo(fileNameFull);
                if (infoIf.Exists)
                {
                    Image bm = Image.FromFile(fileNameFull);
                    using (var ms = new System.IO.MemoryStream())
                    {
                        bm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = ms.GetBuffer();
                        dataFountain.SampleImage = bytes;
                    }
                }
            }
            //else
            //    dataFountain.SampleImage = dataFountain.specList.Image;
            #endregion




            #region 获取元素信息
            List<Atom> AtomList = Atom.FindAll();
            if (AtomList != null && AtomList.Count > 0) dataFountain.atomList = AtomList;

            #endregion
        }

        #region 各套软件测量结果方法


        private static void GetThickTestResult(WorkCurve workcCurrent, CustomStandard CurrentStandard, SpecListEntity splist, ref List<ElemTestResult> ElemTestResultL
            , HistoryRecord historyRecord, List<HistoryElement> historyElementList)
        {
            #region Thick
            foreach (HistoryElement historyElement in historyElementList)
            {
                if (interSeleElement != null && !interSeleElement.Exists(delegate(string v) { return v == historyElement.elementName; })) continue;

                ElemTestResult currElemTestResult = new ElemTestResult();

                currElemTestResult.ElementName = historyElement.elementName;
                currElemTestResult.ThickValue = double.Parse(historyElement.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit);
                currElemTestResult.ThickUnit = historyElement.thickunitValue == 1 ? "(u〞)" : "(um)";

                currElemTestResult.IntensityVaule = historyElement.CaculateIntensity.ToString("f2");

                Atom atom = Atoms.AtomList.Find(s => s.AtomName == historyElement.elementName);
                if (atom != null)
                {
                    currElemTestResult.ElementNameCN = atom.AtomNameCN;
                    currElemTestResult.ElementNameEN = atom.AtomNameEN;
                }

                if (workcCurrent.ElementList.Items.ToList().Find(a => a.Caption == historyElement.elementName && a.IsShowContent) != null)
                //if (workcCurrent.IsThickShowContent)
                {
                    if (historyElement.unitValue == 1)
                    {
                        currElemTestResult.ContextUnit = "(%)";
                    }
                    else if (historyElement.unitValue == 2)
                    {
                        currElemTestResult.ContextUnit = "(ppm)";
                    }
                    else
                    {
                        currElemTestResult.ContextUnit = "(‰)";
                    }
                    //currElemTestResult.ContextUnit = historyElement.unitValue == 1 ? "(%)" : "(ppm)";
                    double temop = double.Parse(historyElement.contextelementValue) >= 100 ? 100 : double.Parse(historyElement.contextelementValue);
                    currElemTestResult.ContextValue = temop.ToString("f" + WorkCurveHelper.SoftWareContentBit);

                }

                ElemTestResultL.Add(currElemTestResult);
            }
            #endregion
        }




        #endregion

        public class PrintObject
        {
            public SpecListEntity specList { get; set; }
            public WorkCurve workCurve { get; set; }
            public long historyRecordId { get; set; }
            public int printType { get; set; }//1:历史记录，2:连测历史记录
            public int continuousresultId { get; set; }//连测历史记录的continuousresultId
            public PrintObject(SpecListEntity specList, WorkCurve workCurve, int printType, long historyRecordId)
            {
                this.specList = specList;
                this.workCurve = workCurve;
                this.printType = printType;
                this.historyRecordId = historyRecordId;

            }
            public PrintObject(SpecListEntity specList, WorkCurve workCurve, int printType, int continuousresultId)
            {
                this.specList = specList;
                this.workCurve = workCurve;
                this.printType = printType;
                this.continuousresultId = continuousresultId;

            }
        }

        #endregion


        #region 黄铜模板
        public string BrassReport(List<long> selectLong)
        {
            #region //
            //string Language = "";
            //Skyray.Language.Languages CurrentLang = Skyray.Language.Languages.FindOne(l => l.IsCurrentLang == true);
            //Language = CurrentLang.ShortName;
            //if (Language.ToLower() == "english") Language = "EN";
            //else if (Language.ToLower() == "chinese") Language = "CN";

            //string reportName = GetDefineReportName();
            //Report report = new Report();
            //List<BrassReport> BrassReportList = new List<BrassReport>();

            //DataTable dt_BrassStatistics = new DataTable();

            //ElementList elementStatisticsList = null;

            //foreach (long hisid in selectLong)
            //{
            //    #region
            //    string SampleName = "";
            //    DateTime TestTime = DateTime.Now;
            //    string WorkCurveName = "";
            //    string Specification = "";
            //    string SupplierName = "";
            //    string Weight = "";
            //    string Operater = "";
            //    string Address = "";
            //    string appAddress = ReportTemplateHelper.LoadSpecifiedValue("Excel", "Address");
            //    Address = appAddress;
            //    DataTable dt_Brass = null;
            //    List<SpecList> lSpecList = SpecList.FindBySql("select * from speclist where id in (select speclistid from historyrecord where id=" + hisid + " )");
            //    if (lSpecList.Count == 0) return "";

            //    SampleName = lSpecList[0].Name;
            //    TestTime = DateTime.Parse(lSpecList[0].SpecDate.ToString());
            //    WorkCurve workCurve = WorkCurve.FindById(lSpecList[0].WorkCurveId);
            //    WorkCurveName = workCurve.Name;
            //    SupplierName = lSpecList[0].Supplier;
            //    Weight = lSpecList[0].Weight.ToString();
            //    Operater = lSpecList[0].Operater;
            //    Specification = lSpecList[0].SpecSummary;
            //    ElementList elementList = ElementList.New;
            //    var elements = HistoryElement.Find(w => w.HistoryRecord.Id == hisid);
            //    foreach (var element in elements)
            //    {
            //        var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
            //        if (temp == null)
            //            continue;
            //        double content = 0.0;
            //        double.TryParse(element.contextelementValue, out content);
            //        temp.Intensity = element.CaculateIntensity;
            //        temp.Error = element.Error;
            //        if (element.unitValue == 1)
            //            temp.Content = content;
            //        else if (element.unitValue == 2)
            //            temp.Content = content / 10000;
            //        else
            //            temp.Content = content / 10;
            //        elementList.Items.Add(temp);
            //    }
            //    if (elementStatisticsList != null && elementList.Items.Count > elementStatisticsList.Items.Count)
            //        elementStatisticsList = elementList;
            //    else if (elementStatisticsList == null) elementStatisticsList = elementList;

            //    dt_Brass = SetColumns(elementList);
            //    dt_Brass.Columns.RemoveAt(0);

            //    for (int i = 1; i <= 3; i++)
            //    {
            //        DataRow dr = dt_Brass.NewRow();
            //        dt_Brass.Rows.Add(dr);
            //    }



            //    foreach (CurveElement curele in elementList.Items)
            //    {
            //        Atom atom = Atoms.AtomList.ToList().Find(w => w.AtomName == curele.Caption);
            //        dt_Brass.Rows[0][curele.Caption] = (atom == null) ? "" : ((Language == "CN") ? atom.AtomNameCN : atom.AtomNameEN);
            //        dt_Brass.Rows[1][curele.Caption] = (atom == null) ? "" : atom.AtomName;
            //        dt_Brass.Rows[2][curele.Caption] = curele.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
            //    }

            //    BrassReport BrassReport = new BrassReport(SampleName, TestTime, WorkCurveName, Specification,
            //        SupplierName, Weight, Operater, Address, dt_Brass);

            //    BrassReportList.Add(BrassReport);

            //    #endregion
            //}

            //if (BrassReportList.Count > 1)
            //{

            //    //获取统计信息
            //    dt_BrassStatistics = SetColumns(elementStatisticsList);
            //    for (int i = 1; i <= 5; i++)
            //    {
            //        DataRow dr = dt_BrassStatistics.NewRow();
            //        dt_BrassStatistics.Rows.Add(dr);
            //    }

            //    for (int i = 0; i < 5; i++)
            //    {
            //        switch (i)
            //        {
            //            case 0:
            //                dt_BrassStatistics.Rows[i][0] = Info.Statics;
            //                break;
            //            case 1:
            //                dt_BrassStatistics.Rows[i][0] = Info.MeanValue;
            //                break;
            //            case 2:
            //                dt_BrassStatistics.Rows[i][0] = Info.SDValue;
            //                break;
            //            case 3:
            //                dt_BrassStatistics.Rows[i][0] = Info.MaxValue;
            //                break;
            //            case 4:
            //                dt_BrassStatistics.Rows[i][0] = Info.MinValue;
            //                break;
            //        }

            //    }


            //    foreach (DataColumn col in dt_BrassStatistics.Columns)
            //    {
            //        if (col.Caption.ToLower() == "time") continue;
            //        double sMean = 0;
            //        double sVariance = 0;
            //        double sMaximum = 0;
            //        double sMinimum = 0;
            //        GetStatisticsByEele(BrassReportList, col.Caption, ref sMean, ref sVariance, ref sMaximum, ref sMinimum);
            //        dt_BrassStatistics.Rows[0][col.Caption] = col.Caption;// "统计"; 
            //        dt_BrassStatistics.Rows[1][col.Caption] = sMean.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());// "平均值";
            //        dt_BrassStatistics.Rows[2][col.Caption] = sVariance.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());// "方差";
            //        dt_BrassStatistics.Rows[3][col.Caption] = sMaximum.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());// "最大值";
            //        dt_BrassStatistics.Rows[4][col.Caption] = sMinimum.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());// "最小值";
            //    }
            //}


            //report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ((selectLong.Count > 1) ? ExcelTemplateParams.ManyTimeTemplate : ExcelTemplateParams.OneTimeTemplate);
            //report.ReportPath = WorkCurveHelper.ExcelPath;
            ////修改：何晓明 20111129 报告命名设置 赋值设置名称
            //report.GenerateRetestReport_Brass(reportName, BrassReportList, dt_BrassStatistics, true);

            //////修改：何晓明 20110715 按模板导出打开Excel
            ////var pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
            ////if (pProcess != null && pProcess.Length > 0) try { pProcess[0].Kill(); }
            ////    catch { };
            #endregion

            ExcelTemplateParams.GetExcelTemplateParams();
            string reportName = GetDefineReportName();
            Report report = new Report();
            report.bProtect = false;

            report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + (selectLong.Count > 1 ? ExcelTemplateParams.ManyTimeTemplate : ExcelTemplateParams.OneTimeTemplate);
            report.ReportPath = WorkCurveHelper.ExcelPath;
            report.historyRecordid = selectLong.FirstOrDefault().ToString();
            string StrExcelPath = report.GenerateRepeaterReportBrass(reportName, null, true, selectLong);


            if (!File.Exists(StrExcelPath))
                return "";
            return StrExcelPath;
        }

        private void GetStatisticsByEele(List<BrassReport> BrassReportList, string sEelName,
            ref double sMean, ref double sVariance, ref double sMaximum, ref double sMinimum)
        {
            sMaximum = 0;
            sMinimum = -1;
            double dEelContent = 0;
            foreach (BrassReport brassReport in BrassReportList)
            {
                dEelContent = double.Parse((brassReport.dt_Brass.Rows[2][sEelName] == null || brassReport.dt_Brass.Rows[2][sEelName].ToString() == "") ? "0" : brassReport.dt_Brass.Rows[2][sEelName].ToString());
                sMean += dEelContent;
                if (dEelContent > sMaximum) sMaximum = dEelContent;
                if (dEelContent < sMinimum || sMinimum == -1) sMinimum = dEelContent;

            }
            sMean /= BrassReportList.Count;


            sMean = Math.Round(sMean, 4);
            double powTotal = 0;
            foreach (BrassReport brassReport in BrassReportList)
            {
                dEelContent = double.Parse((brassReport.dt_Brass.Rows[2][sEelName] == null || brassReport.dt_Brass.Rows[2][sEelName].ToString() == "") ? "0" : brassReport.dt_Brass.Rows[2][sEelName].ToString());
                powTotal += Math.Pow((dEelContent - sMean), 2);

            }
            sVariance = Math.Sqrt(powTotal / (BrassReportList.Count - 1));
        }


        private DataTable SetColumns(ElementList elementList)
        {
            DataTable reTestTable = new DataTable();
            reTestTable.Columns.Clear();
            reTestTable.Columns.Add("Time", typeof(string));
            foreach (CurveElement curele in elementList.Items.ToList().OrderBy(d => d.RowsIndex).ToList())
                reTestTable.Columns.Add(curele.Caption, typeof(string));

            return reTestTable;

        }



        #endregion

        #region 选择模板路径，在进行打印或保存
        public virtual void SeleTemplatePrint(bool isPrint)
        {
            if (recordList.Count == 0) return;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string valid = GenerateGenericReportNew(WorkCurveHelper.WorkCurveCurrent.ElementList, recordList, ofd.FileName);
                if (!string.IsNullOrEmpty(valid))
                {
                    if (!isPrint)
                        OpenPathThread(valid);
                    else
                    {
                        //判断打印机是否存在
                        if (!PrintHelper.IsPrinterExist())
                        {
                            SkyrayMsgBox.Show(PrintInfo.NoPrinter);
                            return;
                        }

                        PrintThread(valid);
                    }

                    return;
                }
            }

        }

        public string GenerateGenericReportNew(ElementList elements, List<long> selectLong, string fileTemplateName)
        {
            string sReturn = string.Empty;
            Report report = new Report();
            report.Elements = elements;//FilterElementsByDGV(dgvHistoryRecord, selectLong.FirstOrDefault());
            report.InterestElemCount = report.Elements.Items.ToList().FindAll(w => w.IsShowElement).Count;
            report.historyRecordid = selectLong.FirstOrDefault().ToString();
            var reportName = GetDefineReportName();
            report.ListStatisticsMethods.Add(SUM);
            //sReturn = report.GenerateGenericReport(sFileTemplateName, reportName, selectLong);
            report.TempletFileName = fileTemplateName;
            sReturn = report.GenerateRetestReport(reportName, true, selectLong);
            return sReturn;
        }

        #endregion


        #region 北京时间
        public void AutoIncrease()
        {
            WorkCurveHelper.IsAutoIncrease = WorkCurveHelper.MeasureTimeType == Skyray.EDX.Common.UIHelper.MeasureTimeType.AutoIncrease;
            if (!WorkCurveHelper.IsAutoIncrease) return;
            bool isStart = false;
            if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.Test
                || DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.Idel
                || DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.Motoring
                || DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.Resume)
            {
                isStart = true;
            }

            DifferenceDevice.interClassMain.progressInfo.AutoIncrease(isStart);
        }

        #endregion

        public DataTable GetReportDT12(Report report, ElementList elementList, List<long> selectLong)   //yuzhaoadd:获取打印数据
        {
            #region

            WorkCurve workcurve = WorkCurve.FindById(HistoryRecord.FindById(selectLong[0]).WorkCurveId);
            bool IsShowError = (ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "IsShowError") == "0") ? false : true;
            DataTable dt = new DataTable();
            dt.Columns.Clear();
            dt.Columns.Add(Info.SerialNumber, typeof(string));
            dt.Columns.Add(Info.SampleName, typeof(string));


            // var orders = elementList.Items.OrderBy(w => w.LayerNumber).Select(w => w.Caption);//排序去取元素名 所有感兴趣元素
            var orders = elementList.Items.OrderBy(w => w.LayerNumber);      //yuzhaomodify:加入层数显示
            IList<string> ThickCollection = new List<string>();
            IList<string> OtherCollection = new List<string>();
            IList<string> ResultCollection = new List<string>();
            CustomStandard standard = null;
            string custonStandName = null;
            if (WorkCurveHelper.CurrentStandard != null && WorkCurveHelper.CurrentStandard.Id > -1)
            {
                standard = CustomStandard.FindById(WorkCurveHelper.CurrentStandard.Id);
            }

            foreach (var element in orders)
            {
                string StrNewText = string.Empty;
                if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                {
                    StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                    {
                        return string.Compare(w.ElementCaption, element.Caption, true) == 0;
                    });
                    if (standSample != null)
                        StrNewText = "\r\n" + "(" + standSample.StandardThick.ToString() + " - " + standSample.StandardThickMax.ToString() + ")";
                    else
                        StrNewText = "";

                }

                if (
                    !ThickCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.Thick + " " + StrNewText) &&
                    !OtherCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.EditContent) &&
                    !OtherCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity))
                {
                    if (elementList.Items.ToList().Find(a => a.Caption == element.Caption && a.IsShowElement) != null)
                    {
                        if (workcurve != null && workcurve.IsThickShowAreaThick)
                        {

                            // OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity);   //yuzhaomodify:加入层数显示
                            if (OtherCollection.Count > 0 && OtherCollection[OtherCollection.Count - 1].Contains(element.LayerNumber.ToString()))
                            {
                                if (OtherCollection[0].Split(' ')[1] == element.LayerNumber.ToString())
                                {
                                    OtherCollection[0] = OtherCollection[0].Split(' ')[0] + "|" +
                                                                                 element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity + " " + StrNewText;

                                }
                                else
                                    OtherCollection[OtherCollection.Count - 1] = OtherCollection[OtherCollection.Count - 1].Split(' ')[0] + "|" +
                                                                                     element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity + " " + StrNewText;

                            }
                            else
                            {
                                OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity);
                            }
                        }
                        else
                        {
                            if (ThickCollection.Count > 0 && ThickCollection[ThickCollection.Count - 1].Contains(element.LayerNumber.ToString()))
                            {
                                ThickCollection[ThickCollection.Count - 1] = ThickCollection[ThickCollection.Count - 1].Split(' ')[0] + "|" +
                                                                              element.Caption + " " + element.LayerNumber + " " + Info.Thick + " " + StrNewText;
                            }
                            else
                                ThickCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.Thick + " " + StrNewText);            //yuzhaomodify:加入层数显示
                        }
                        if (WorkCurveHelper.CalcType != CalcType.PeakDivBase)
                        {
                            if (elementList.Items.ToList().Find(a => a.Caption == element.Caption && a.IsShowContent) != null)
                                OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.EditContent);  //yuzhaomodify:加入层数显示
                        }
                        if (IsShowError) OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strError);  //yuzhaomodify:加入层数显示

                    }
                }

            }
            if (orders != null && orders.Count() >= 1)
            {
                int max = orders.ElementAt<CurveElement>(orders.Count() - 1).LayerNumber;
                for (int i = 1; i < max + 1; i++)
                {
                    foreach (string s1 in ThickCollection)
                    {
                        if (s1.Split(' ')[1] == i.ToString())
                            dt.Columns.Add(s1, typeof(string));
                    }
                    foreach (string s2 in OtherCollection)
                    {
                        if (s2.Split(' ')[1] == i.ToString())
                            dt.Columns.Add(s2, typeof(string));

                    }

                }
                if (standard != null)
                    dt.Columns.Add(Info.strPassReslt, typeof(string));
            }

            int cont = 0;
            for (int j = 0; j < selectLong.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                rowNew[Info.SerialNumber] = ++cont;
                HistoryRecord itemHistory = HistoryRecord.FindOne(w => w.Id == selectLong[j]);
                rowNew[Info.SampleName] = itemHistory.SampleName;
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.Caption == Info.SerialNumber || column.Caption == Info.SampleName) continue;
                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.Caption.Split(' ')[0].Split('|')[0] && w.HistoryRecord.Id == selectLong[j]);
                    if (element != null)
                    {
                        if (column.Caption.Split(' ')[2] == Info.EditContent)
                        {
                            string valueStr = element.contextelementValue;

                            if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                            {
                                double Value = double.Parse(valueStr);
                                if (WorkCurveHelper.WorkCurveCurrent.IsThickShowAreaThick)
                                {
                                    rowNew[column.Caption] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(" + (element.densityunitValue == string.Empty ? Info.strAreaDensityUnit : element.densityunitValue) + ")" : "");
                                }
                                else
                                {

                                    if (element.unitValue == 1)
                                        Value = Value * 10000;
                                    else if (element.unitValue == 3)
                                        Value = Value * 1000;

                                    if (element.unitValue == 1)
                                    {
                                        rowNew[column.Caption] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(%)" : "");
                                    }
                                    else if (element.unitValue == 2)
                                    {
                                        rowNew[column.Caption] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(ppm)" : "");
                                    }
                                    else
                                    {
                                        rowNew[column.Caption] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(‰)" : "");
                                    }
                                }
                            }



                        }
                        else if (column.Caption.Split(' ')[2] == Info.Thick)
                        {
                            double outvalue = 0;
                            string outThickValue = string.Empty;
                            bool isdouble = double.TryParse(element.thickelementValue, out outvalue);

                            if (element.thickunitValue == 1)
                                outvalue = outvalue * 0.0254;

                            if (outvalue > WorkCurveHelper.ThicknessLimit)
                            {
                                rowNew[column.Caption] = "--";
                            }
                            else
                            {

                                if (element.thickunitValue == 1)
                                {
                                    rowNew[column.Caption] = (outvalue / 0.0254).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(u〞)" : "");
                                    //   rowNew[column.Caption] = outvalue.ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(u〞)";
                                }
                                else if (element.thickunitValue == 3)
                                {
                                    rowNew[column.Caption] = outvalue.ToString("f" + WorkCurveHelper.ThickBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(g/L)" : ""); //"(g/L)";
                                }
                                else
                                {
                                    rowNew[column.Caption] = outvalue.ToString("f" + WorkCurveHelper.ThickBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(um)" : "");
                                }

                            }

                        }
                        else if (column.Caption.Split(' ')[2] == Info.strError)
                        {
                            rowNew[column.Caption] = element.Error.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                        }
                        else if (column.Caption.Split(' ')[2] == Info.strAreaDensity)
                        {
                            rowNew[column.Caption] = double.Parse(element.densityelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(" + (element.densityunitValue == string.Empty ? Info.strAreaDensityUnit : element.densityunitValue) + ")" : "");
                        }


                    }
                    else if (column.Caption == Info.strPassReslt)
                    {
                        string StrNewText = ExcelTemplateParams.TestRetultForThick(itemHistory.Id.ToString(), out custonStandName);
                        rowNew[column.Caption] = StrNewText;

                    }
                    else
                    {
                        rowNew[column.Caption] = double.Parse("0").ToString("f" + ((column.Caption.Split(' ')[2] == Info.Thick) ? WorkCurveHelper.ThickBit.ToString() : WorkCurveHelper.SoftWareContentBit.ToString())) + " ";

                    }
                }
                dt.Rows.Add(rowNew);
            }
            #endregion


            foreach (DataColumn dctemp in dt.Columns)
            {
                string[] value = dctemp.ColumnName.Split(' ');
                if (value != null && value.Length > 1)
                {
                    CurveElement cueT = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == value[0]);
                    if (cueT != null)
                        dctemp.ColumnName = dctemp.ColumnName.Replace(cueT.Caption, cueT.DefineElemName);
                }
            }

            if (FpWorkCurve.thickMode == ThickMode.NiNi || FpWorkCurve.thickMode == ThickMode.NiCuNi)
            {
                string colName = string.Empty;
                foreach (DataColumn dctemp in dt.Columns)
                {
                    if (dctemp.ColumnName.Contains("Fe 3 " + Info.Thick))
                    {
                        dctemp.ColumnName = "Ni 2 " + Info.Thick;
                        break;
                    }

                }


            }


            return dt;
        }
        public DataTable GetReportDT25(Report report, ElementList elementList, List<HistoryElement> elements, List<long> selectLong)   //yuzhaoadd:获取打印数据
        {
            #region

            WorkCurve workcurve = WorkCurve.FindById(HistoryRecord.FindById(selectLong[0]).WorkCurveId);
            bool IsShowError = (ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "IsShowError") == "0") ? false : true;
            DataTable dt = new DataTable();
            dt.Columns.Clear();
            dt.Columns.Add(Info.SerialNumber, typeof(string));
            dt.Columns.Add(Info.SampleName, typeof(string));


            // var orders = elementList.Items.OrderBy(w => w.LayerNumber).Select(w => w.Caption);//排序去取元素名 所有感兴趣元素
            var orders = elementList.Items.OrderBy(w => w.LayerNumber);      //yuzhaomodify:加入层数显示
            IList<string> ThickCollection = new List<string>();
            IList<string> OtherCollection = new List<string>();
            IList<string> CustomFieldCollection = new List<string>();
            foreach (var e in elements)
            {
                if (!CustomFieldCollection.Contains(e.elementName + " " + " " + Info.Thick))
                {
                    Atom atom = Atoms.AtomList.Find(s => s.AtomName == e.elementName);
                    if (atom == null)
                    {
                        CustomFieldCollection.Add(e.elementName + " " + " " + Info.Thick);
                    }
                }
            }
            foreach (var element in orders)
            {
                if (
                    !ThickCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.Thick) &&
                    !OtherCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.EditContent) &&
                    !OtherCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity))
                {
                    if (elementList.Items.ToList().Find(a => a.Caption == element.Caption && a.IsShowElement) != null)
                    {
                        if (workcurve != null && workcurve.IsThickShowAreaThick)
                        {
                            // OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity);   //yuzhaomodify:加入层数显示
                            if (OtherCollection.Count > 0 && OtherCollection[OtherCollection.Count - 1].Contains(element.LayerNumber.ToString()))
                            {
                                if (OtherCollection[0].Split(' ')[1] == element.LayerNumber.ToString())
                                {
                                    OtherCollection[0] = OtherCollection[0].Split(' ')[0] + "|" +
                                                                                 element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity;

                                }
                                else
                                    OtherCollection[OtherCollection.Count - 1] = OtherCollection[OtherCollection.Count - 1].Split(' ')[0] + "|" +
                                                                                     element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity;

                            }
                            else
                            {
                                OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity);
                            }
                        }
                        else
                        {
                            if (ThickCollection.Count > 0 && ThickCollection[ThickCollection.Count - 1].Contains(element.LayerNumber.ToString()))
                            {
                                ThickCollection[ThickCollection.Count - 1] = ThickCollection[ThickCollection.Count - 1].Split(' ')[0] + "|" +
                                                                              element.Caption + " " + element.LayerNumber + " " + Info.Thick;
                            }
                            else
                                ThickCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.Thick);            //yuzhaomodify:加入层数显示
                        }
                        if (WorkCurveHelper.CalcType != CalcType.PeakDivBase)
                        {
                            if (elementList.Items.ToList().Find(a => a.Caption == element.Caption && a.IsShowContent) != null)
                                OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.EditContent);  //yuzhaomodify:加入层数显示
                        }
                        if (IsShowError) OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strError);  //yuzhaomodify:加入层数显示
                    }
                }

            }
            //foreach (string s1 in ThickCollection)
            //    dt.Columns.Add(s1,typeof(string));
            //foreach (string s2 in OtherCollection)
            //    dt.Columns.Add(s2,typeof(string));
            if (orders != null && orders.Count() >= 1)
            {
                int max = orders.ElementAt<CurveElement>(orders.Count() - 1).LayerNumber;
                for (int i = 1; i < max + 1; i++)
                {
                    foreach (string s1 in ThickCollection)
                    {
                        if (s1.Split(' ')[1] == i.ToString())
                            dt.Columns.Add(s1, typeof(string));
                    }
                    foreach (string s2 in OtherCollection)
                    {
                        if (s2.Split(' ')[1] == i.ToString())
                            dt.Columns.Add(s2, typeof(string));
                    }

                }
                foreach (string s3 in CustomFieldCollection)
                {
                    dt.Columns.Add(s3, typeof(string));
                }
            }

            int cont = 0;
            for (int j = 0; j < selectLong.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                rowNew[Info.SerialNumber] = ++cont;
                HistoryRecord itemHistory = HistoryRecord.FindOne(w => w.Id == selectLong[j]);
                rowNew[Info.SampleName] = itemHistory.SampleName;
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.Caption == Info.SerialNumber || column.Caption == Info.SampleName) continue;
                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.Caption.Split(' ')[0].Split('|')[0] && w.HistoryRecord.Id == selectLong[j]);
                    if (element != null)
                    {
                        if (column.Caption.Split(' ')[2] == Info.EditContent)
                        {
                            string valueStr = element.contextelementValue;
                            if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                            {
                                double Value = double.Parse(valueStr);
                                if (element.unitValue == 1)
                                    Value = Value * 10000;
                                else if (element.unitValue == 3)
                                    Value = Value * 1000;

                                if (element.unitValue == 1)
                                {
                                    rowNew[column.Caption] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(%)" : "");
                                }
                                else if (element.unitValue == 2)
                                {
                                    rowNew[column.Caption] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(ppm)" : ""); //"(ppm)";
                                }
                                else
                                {
                                    rowNew[column.Caption] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(‰)" : "");//"(‰)";
                                }


                            }
                        }
                        else if (column.Caption.Split(' ')[2] == Info.Thick)
                        {
                            if (element.thickunitValue == 1)
                            {
                                rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(u〞)" : ""); //"(u〞)";
                            }
                            else if (element.thickunitValue == 3)
                            {
                                rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(g/L)" : "");//"(g/L)";
                            }
                            else
                            {
                                rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(um)" : "");//"(um)";
                            }
                        }
                        else if (column.Caption.Split(' ')[2] == Info.strError)
                        {
                            rowNew[column.Caption] = element.Error.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                        }
                        else if (column.Caption.Split(' ')[2] == Info.strAreaDensity)
                        {
                            rowNew[column.Caption] = double.Parse(element.densityelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + (ReportTemplateHelper.ShowUnitType ? "(" + (element.densityunitValue == string.Empty ? Info.strAreaDensityUnit : element.densityunitValue) + ")" : "");
                        }
                        //else
                        //{
                        //    rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(" + (element.densityunitValue == string.Empty ? Info.strAreaDensityUnit : element.densityunitValue) + ")";
                        //}

                    }
                    else
                    {
                        rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";

                    }
                }
                dt.Rows.Add(rowNew);
            }
            #endregion
            return dt;
        }
        public DataTable GetReportDT19(Report report, ElementList elementList, List<long> selectLong)   //yuzhaoadd:获取打印数据
        {
            #region

            WorkCurve workcurve = WorkCurve.FindById(HistoryRecord.FindById(selectLong[0]).WorkCurveId);
            bool IsShowError = (ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "IsShowError") == "0") ? false : true;
            DataTable dt = new DataTable();
            dt.Columns.Clear();
            dt.Columns.Add(Info.SerialNumber, typeof(string));
            dt.Columns.Add(Info.SampleName, typeof(string));
            int elemcout = 0;
            //foreach (var elem in elementList.Items)
            //{
            //    if (elem.LayerNumBackUp == Info.Substrate || elem.LayerNumBackUp == "基材")
            //        elemcout++;
            //}
            // var orders = elementList.Items.OrderBy(w => w.LayerNumber).Select(w => w.Caption);//排序去取元素名 所有感兴趣元素


            var elemOrder = elementList.Items.OrderByDescending(w => w.LayerNumber);
            foreach (var a in elemOrder)
            {
                if ((a.LayerNumBackUp == "基材" || a.LayerNumBackUp == Info.Substrate) && !a.IsShowElement)
                    elemcout = a.LayerNumber - 1;
                else
                    elemcout = a.LayerNumber;
                break;
            }
            //if (elemOrder.LayerNumBackUp == "基材" || elemOrder.LayerNumBackUp == Info.Substrate)
            //{
            //    elemcout = elementList.Items[0].LayerNumber - 1;
            //}
            //else
            //{
            //    elemcout = elementList.Items[0].LayerNumber;
            //}
            var orders = elementList.Items.OrderBy(w => w.LayerNumber);      //yuzhaomodify:加入层数显示
            IList<string> ThickCollection = new List<string>();
            IList<string> OtherCollection = new List<string>();
            foreach (var element in orders)
            {
                if (
                    !ThickCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.Thick) &&
                    !OtherCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.EditContent) &&
                    !OtherCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity))
                {
                    if (elementList.Items.ToList().Find(a => a.Caption == element.Caption && a.IsShowElement) != null)
                    {
                        if (workcurve != null && workcurve.IsThickShowAreaThick)
                        {
                            OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity);   //yuzhaomodify:加入层数显示
                        }
                        else
                        {
                            if (ThickCollection.Count > 0 && ThickCollection[ThickCollection.Count - 1].Contains(element.LayerNumber.ToString()))
                            {
                                ThickCollection[ThickCollection.Count - 1] = ThickCollection[ThickCollection.Count - 1].Split(' ')[0] + "|" +
                                                                              element.Caption + " " + element.LayerNumber + " " + Info.Thick;
                                if (OtherCollection[OtherCollection.Count - 1].Contains(Info.strPassReslt))
                                {
                                    OtherCollection.RemoveAt(OtherCollection.Count - 1);
                                    OtherCollection.RemoveAt(OtherCollection.Count - 1);
                                }
                                elemcout++;
                            }
                            else
                                ThickCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.Thick);            //yuzhaomodify:加入层数显示
                        }
                        //if (WorkCurveHelper.CalcType != CalcType.PeakDivBase)
                        //{
                        //    if (elementList.Items.ToList().Find(a => a.Caption == element.Caption && a.IsShowContent) != null)
                        //        OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.EditContent);  //yuzhaomodify:加入层数显示
                        //}
                        // if (IsShowError) OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strError);  //yuzhaomodify:加入层数显示
                        //if (WorkCurveHelper.isThickLimit == 1 && elemcout == 1 )
                        if (WorkCurveHelper.isThickLimit == 1 && element.LayerNumber == 1)
                        {
                            OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strRestrictStandard);
                            OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strPassReslt);
                        }
                    }
                }

            }
            //foreach (string s1 in ThickCollection)
            //    dt.Columns.Add(s1,typeof(string));
            //foreach (string s2 in OtherCollection)
            //    dt.Columns.Add(s2,typeof(string));
            if (orders != null && orders.Count() >= 1)
            {
                int max = orders.ElementAt<CurveElement>(orders.Count() - 1).LayerNumber;
                for (int i = 1; i < max + 1; i++)
                {
                    foreach (string s1 in ThickCollection)
                    {
                        if (s1.Split(' ')[1] == i.ToString())
                            dt.Columns.Add(s1, typeof(string));
                    }
                    foreach (string s2 in OtherCollection)
                    {
                        if (s2.Split(' ')[1] == i.ToString())
                            dt.Columns.Add(s2, typeof(string));
                    }
                }
            }

            int cont = 0;
            for (int j = 0; j < selectLong.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                rowNew[Info.SerialNumber] = ++cont;
                HistoryRecord itemHistory = HistoryRecord.FindOne(w => w.Id == selectLong[j]);
                rowNew[Info.SampleName] = itemHistory.SampleName;
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.Caption == Info.SerialNumber || column.Caption == Info.SampleName) continue;
                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.Caption.Split(' ')[0].Split('|')[0] && w.HistoryRecord.Id == selectLong[j]);
                    if (element != null)
                    {
                        if (column.Caption.Split(' ')[2] == Info.EditContent)
                        {
                            //string valueStr = element.contextelementValue;
                            //if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                            //{
                            //    double Value = double.Parse(valueStr);
                            //    if (element.unitValue == 1)
                            //        Value = Value * 10000;
                            //    else if (element.unitValue == 3)
                            //        Value = Value * 1000;

                            //    if (element.unitValue == 1)
                            //    {
                            //        rowNew[column.Caption] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                            //    }
                            //    else if (element.unitValue == 2)
                            //    {
                            //        rowNew[column.Caption] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                            //    }
                            //    else
                            //    {
                            //        rowNew[column.Caption] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                            //    }


                            //}
                        }
                        else if (column.Caption.Split(' ')[2] == Info.Thick)
                        {
                            if (element.thickunitValue == 1)
                            {
                                rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(u〞)";
                            }
                            else if (element.thickunitValue == 3)
                            {
                                rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(g/L)";
                            }
                            else
                            {
                                rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";
                            }
                        }
                        else if (column.Caption.Split(' ')[2] == Info.strError)
                        {
                            rowNew[column.Caption] = element.Error.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                        }
                        else if (column.Caption.Split(' ')[2] == Info.strAreaDensity)
                        {
                            rowNew[column.Caption] = double.Parse(element.densityelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(" + (element.densityunitValue == string.Empty ? Info.strAreaDensityUnit : element.densityunitValue) + ")";
                        }
                        else if (column.Caption.Split(' ')[2] == Info.strRestrictStandard)
                        {
                            string StrNewText = string.Empty;

                            if (WorkCurveHelper.CurrentStandard != null && WorkCurveHelper.CurrentStandard.Id > -1)
                            {
                                CustomStandard standard = CustomStandard.FindById(WorkCurveHelper.CurrentStandard.Id);
                                if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                                {
                                    StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                                    {
                                        return string.Compare(w.ElementCaption, element.elementName, true) == 0;
                                    });
                                    if (standSample != null)
                                        StrNewText = standSample.StandardThick.ToString() + " - " + standSample.StandardThickMax.ToString();
                                    else
                                        StrNewText = "0-0";
                                    if (element.thickunitValue == 1)
                                    {
                                        StrNewText += "(u〞)";
                                    }
                                    else
                                    {
                                        StrNewText += "(um)";
                                    }
                                }
                            }
                            rowNew[column.Caption] = StrNewText;

                        }
                        else if (column.Caption.Split(' ')[2] == Info.strPassReslt)
                        {
                            string StrNewText = string.Empty;
                            if (WorkCurveHelper.CurrentStandard != null && WorkCurveHelper.CurrentStandard.Id > -1)
                            {
                                CustomStandard standard = CustomStandard.FindById(WorkCurveHelper.CurrentStandard.Id);
                                if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                                {
                                    StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                                    {
                                        return string.Compare(w.ElementCaption, element.elementName, true) == 0;
                                    });
                                    if (standSample != null)
                                    {
                                        if (double.Parse(element.thickelementValue) >= standSample.StandardThick && double.Parse(element.thickelementValue) <= standSample.StandardThickMax)
                                            StrNewText = "PASS";
                                        else
                                            StrNewText = "Fail";
                                    }
                                }
                            }
                            rowNew[column.Caption] = StrNewText;
                        }

                    }
                    else
                    {
                        rowNew[column.Caption] = double.Parse("0").ToString("f" + ((column.Caption.Split(' ')[2] == Info.Thick) ? WorkCurveHelper.ThickBit.ToString() : WorkCurveHelper.SoftWareContentBit.ToString())) + " ";

                    }
                }
                dt.Rows.Add(rowNew);
            }
            #endregion
            return dt;
        }

        //public bool GetUncertainty(DataGridView dgvResults, string strSimilarSample)
        //{
        //    if (WorkCurveHelper.WorkCurveCurrent == null) return false;
        //    FrmSelToCertainty frmSelToCertainty = new FrmSelToCertainty();
        //    frmSelToCertainty.WorkCurve = WorkCurveHelper.WorkCurveCurrent;
        //    if (frmSelToCertainty.ShowDialog() != DialogResult.OK)
        //    {
        //        return false;
        //    }
        //    List<double> dblTestResults = new List<double>();
        //    int colIndex = -1;
        //    for (int i = 0; i < dgvResults.ColumnCount; i++)
        //    {
        //        if (dgvResults.Columns[i].Name.Equals(frmSelToCertainty.Element.Caption))
        //        {
        //            colIndex = i;
        //            break;
        //        }
        //    }
        //    for (int i = 0; i < dgvResults.RowCount; i++)
        //    {
        //        double resultTemp = 0;
        //        try
        //        {
        //            //转换成%
        //            switch (frmSelToCertainty.Element.ContentUnit)
        //            {
        //                case ContentUnit.permillage:
        //                    resultTemp = double.Parse(dgvResults.Rows[i].Cells[colIndex].Value.ToString()) / 10;
        //                    break;
        //                case ContentUnit.ppm:
        //                    resultTemp = double.Parse(dgvResults.Rows[i].Cells[colIndex].Value.ToString()) / 10000;
        //                    break;
        //                default:
        //                    resultTemp = double.Parse(dgvResults.Rows[i].Cells[colIndex].Value.ToString());
        //                    break;
        //            }
        //        }
        //        catch
        //        {
        //            resultTemp = 0;
        //        }
        //        dblTestResults.Add(resultTemp);
        //    }
        //    double Ua, Ub, Uc, Ud, Ux, U, AvgTest;
        //    StandSample similarSamp = frmSelToCertainty.Element.Samples.ToList().Find(w => w.SampleName == strSimilarSample);
        //    Ub = similarSamp == null ? 0 : double.Parse(similarSamp.Uncertainty);
        //    //Ub=frmSelToCertainty.StandUncertainty;
        //    Ud = frmSelToCertainty.UnknownUncertainty;
        //    AvgTest = Ua = Uc = Ux = U = 0;
        //    ////测试计算方法
        //    //dblTestResults.Clear();
        //    //dblTestResults.Add(99.58);
        //    //dblTestResults.Add(99.57);
        //    //dblTestResults.Add(99.58);
        //    //dblTestResults.Add(99.39);
        //    //dblTestResults.Add(99.56);
        //    //dblTestResults.Add(99.57);
        //    //Ub = 0.07;//编辑数据里面填写的是扩展不确定度
        //    //////
        //    bool result = WorkCurveHelper.WorkCurveCurrent.GetUncertainty(dblTestResults, frmSelToCertainty.Element.Caption, Ub / 2, Ud / 2, ref Uc, ref Ua, ref Ux, ref U, ref AvgTest);
        //    if (result)
        //    {
        //        string ShowInfomation = string.Empty;
        //        ShowInfomation += Info.TestResult + Info.Uncertainty + "Ua=" + Ua.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%\r\n";
        //        ShowInfomation += Info.StandandSample + Info.Uncertainty + "Ub=" + (Ub / 2).ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%(" + similarSamp.SampleName + ")\r\n";
        //        ShowInfomation += Info.WorkingCurve + Info.Uncertainty + "Uc=" + Uc.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%\r\n";
        //        ShowInfomation += Info.TestSample + Info.Uncertainty + "Ud=" + (Ud / 2).ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%\r\n";
        //        ShowInfomation += Info.Combined + Info.Uncertainty + "Ux=" + Ux.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%\r\n";
        //        ShowInfomation += Info.Expanded + Info.Uncertainty + "U=2*Ux=" + U.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%(k=2)\r\n";
        //        //转换成%
        //        string strUnit = "%";
        //        switch (frmSelToCertainty.Element.ContentUnit)
        //        {
        //            case ContentUnit.permillage:
        //                U *= 10;
        //                AvgTest *= 10;
        //                strUnit = "‰";
        //                break;
        //            case ContentUnit.ppm:
        //                U *= 10000;
        //                AvgTest *= 10000;
        //                strUnit = "ppm";
        //                break;
        //            default:
        //                break;
        //        }
        //        ShowInfomation += Info.TestSample + Info.Content + "(" + frmSelToCertainty.Element.Caption + ":" + AvgTest.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "±" + U.ToString("F" + WorkCurveHelper.SoftWareContentBit) + ")" + strUnit + ",k=2\r\n";
        //        ShowInfomation += Info.UncertaintyStandard;
        //        Msg.Show(ShowInfomation, Info.Uncertainty + "(" + frmSelToCertainty.Element.Caption + ")", MessageBoxButtons.OK, MessageBoxIcon.None);
        //    }
        //    return result;

        //}

        public bool GetUncertainty(DataGridView dgvResults, string strSimilarSample)
        {
            if (WorkCurveHelper.WorkCurveCurrent == null) return false;
            FrmSelToCertainty frmSelToCertainty = new FrmSelToCertainty();
            frmSelToCertainty.TopMost = true;
            frmSelToCertainty.WorkCurve = WorkCurveHelper.WorkCurveCurrent;
            if (frmSelToCertainty.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            //bool isNotPrint = true;
            //if (frmSelToCertainty.DialogResult == DialogResult.OK)
            //    isNotPrint = true;
            //else if (frmSelToCertainty.DialogResult == DialogResult.Yes)
            //    isNotPrint = false;
            //else
            //return false ;
            List<double> dblTestResults = new List<double>();
            int colIndex = -1;
            for (int i = 0; i < dgvResults.ColumnCount; i++)
            {
                if (dgvResults.Columns[i].Name.Split('(')[0].Equals(frmSelToCertainty.Element.Caption))
                {
                    colIndex = i;
                    break;
                }
            }
            for (int i = 0; i < dgvResults.RowCount; i++)
            {
                double resultTemp = 0;
                try
                {
                    //转换成%
                    switch (frmSelToCertainty.Element.ContentUnit)
                    {
                        case ContentUnit.permillage:
                            resultTemp = double.Parse(dgvResults.Rows[i].Cells[colIndex].Value.ToString()) / 10;
                            break;
                        case ContentUnit.ppm:
                            resultTemp = double.Parse(dgvResults.Rows[i].Cells[colIndex].Value.ToString()) / 10000;
                            break;
                        default:
                            resultTemp = double.Parse(dgvResults.Rows[i].Cells[colIndex].Value.ToString());
                            break;
                    }
                }
                catch
                {
                    resultTemp = 0;
                }
                dblTestResults.Add(resultTemp);
            }
            double Ua, Ub, Uc, Ud, Ux, U, AvgTest;
            StandSample similarSamp = frmSelToCertainty.Element.Samples.ToList().Find(w => w.SampleName == strSimilarSample);
            Ub = similarSamp == null ? 0 : double.Parse(similarSamp.Uncertainty);
            //Ub=frmSelToCertainty.StandUncertainty;
            Ud = frmSelToCertainty.UnknownUncertainty;
            AvgTest = Ua = Uc = Ux = U = 0;
            List<StandSample> sample = null;
            bool result = WorkCurveHelper.WorkCurveCurrent.GetUncertainty(dblTestResults, frmSelToCertainty.Element.Caption, Ub / 2, Ud / 2, ref Uc, ref Ua, ref Ux, ref U, ref AvgTest, ref sample);

            List<UncertaintySample> sampleList = UncertaintySample.LoadUnSmple(frmSelToCertainty.Element.Caption, sample);
            Uncertainty ucty = new Uncertainty(Ua, Ub / 2, Uc, Ud / 2, Ux, U);
            UncertaintyResult ur = new UncertaintyResult(ucty, AvgTest);
            UncertaintySample ucs = sampleList.Find(sl => sl.CurveName == similarSamp.SampleName);

            if (result)
            {
                string ShowInfomation = string.Empty;
                ShowInfomation += Info.TestResult + Info.Uncertainty + "Ua=" + Ua.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%\r\n\r\n";
                ShowInfomation += Info.StandandSample + Info.Uncertainty + "Ub=" + (Ub / 2).ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%(" + similarSamp.SampleName + ")\r\n\r\n";
                ShowInfomation += Info.Curve + Info.Uncertainty + "Uc=" + Uc.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%\r\n\r\n";
                ShowInfomation += Info.TestSample + Info.Uncertainty + "Ud=" + (Ud / 2).ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%\r\n\r\n";
                ShowInfomation += Info.Combined + Info.Uncertainty + "Ux=" + Ux.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%\r\n\r\n";
                ShowInfomation += Info.Expanded + Info.Uncertainty + "U=2*Ux=" + U.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "%(k=2)\r\n\r\n";
                //转换成%
                string strUnit = "%";
                switch (frmSelToCertainty.Element.ContentUnit)
                {
                    case ContentUnit.permillage:
                        U *= 10;
                        AvgTest *= 10;
                        strUnit = "‰";
                        break;
                    case ContentUnit.ppm:
                        U *= 10000;
                        AvgTest *= 10000;
                        strUnit = "ppm";
                        break;
                    default:
                        break;
                }
                ShowInfomation += Info.TestSample + Info.Content + "(" + frmSelToCertainty.Element.Caption + ":" + AvgTest.ToString("F" + WorkCurveHelper.SoftWareContentBit) + "±" + U.ToString("F" + WorkCurveHelper.SoftWareContentBit) + ")" + strUnit + ",k=2\r\n\r\n";
                ShowInfomation += Info.UncertaintyStandard;

                FrmUncertainty fuc = new FrmUncertainty(sampleList, ur, dblTestResults, ucs, sample, frmSelToCertainty.Element.Caption, ucs == null ? ucs.SampleName : similarSamp.SampleName, ShowInfomation);
                fuc.ShowDialog();


                //    Msg.Show(ShowInfomation, Info.Uncertainty + "(" + frmSelToCertainty.Element.Caption + ")", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            return result;

        }


        //追加不确定度的的匹配相似标样
        public string GetSimilarStandSample(SpecListEntity specUnKnown, out string StringMatch)
        {
            List<string> matchspecs = new List<string>();
            List<double> matchcoefs = new List<double>();
            string strMaxMatchSpName = string.Empty;
            double MaxLevel = 0d;
            StringMatch = string.Empty;

            #region//2014-04-29
            int ManyElemIndex = 0;
            int ManyElemConditon = 0;
            for (int i = 0; i < specUnKnown.Specs.Length; i++)
            {
                int elemCountTemp = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().FindAll(w => w.ConditionID == i).Count;
                if (elemCountTemp > ManyElemConditon)
                {
                    ManyElemIndex = i;
                    ManyElemConditon = elemCountTemp;
                }
            }
            foreach (var sp in WorkCurveHelper.WorkCurveCurrent.ElementList.Items[0].Samples)
            {
                if (!sp.Active) continue;
                SpecListEntity sptemp = WorkCurveHelper.DataAccess.Query(sp.SampleName);
                double tempLevel = 0d;
                if (sptemp != null)
                {
                    tempLevel = TabControlHelper.Matching(specUnKnown.Specs[ManyElemIndex].SpecDatac, this.maxOffSet, SpecHelper.GetHighSpecChannel(0, sptemp.Specs[ManyElemIndex].SpecDatac.Length - 1, sptemp.Specs[ManyElemIndex].SpecDatac), sptemp.Specs[ManyElemIndex].SpecDatac);
                    //strMaxMatchSpName = tempLevel > MaxLevel ? sptemp.SampleName : strMaxMatchSpName;
                    strMaxMatchSpName = tempLevel > MaxLevel ? sp.SampleName : strMaxMatchSpName;//修改成谱名 编辑数据里的标样名同谱名
                    MaxLevel = tempLevel > MaxLevel ? tempLevel : MaxLevel;
                    //matchspecs.Add(sptemp.SampleName);
                    matchspecs.Add(sp.SampleName);
                    matchcoefs.Add(tempLevel);
                    StringMatch += sp.SampleName + "  " + tempLevel.ToString() + "\r\n";
                }
            }
            #endregion
            return strMaxMatchSpName;
        }



        public void AddToHistorySQL()
        {
            try
            {
                if (SqlConnection.State != ConnectionState.Open)
                {
                    Msg.Show(Info.ConnectDatabaseError);
                }
                else
                {

                    if (specList == null || WorkCurveHelper.WorkCurveCurrent == null) return;

                    var elements = HistoryElement.Find(w => w.HistoryRecord.Id == Convert.ToInt64(DifferenceDevice.interClassMain.recordList[0]));
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        int currentNum = 1;
                        string sampleName = string.Empty;
                        HistoryRecord hRecord = HistoryRecord.FindById(Convert.ToInt64(DifferenceDevice.interClassMain.recordList[0]));
                        WorkCurve workCurve = WorkCurve.FindById(hRecord.WorkCurveId);
                        List<HistoryCompanyOtherInfo> hisCompanyOtherInfoList = HistoryCompanyOtherInfo.FindBySql("select * from historycompanyotherinfo where history_id = " + DifferenceDevice.interClassMain.recordList[0]);
                        string sReplaceText = "";
                        if (hisCompanyOtherInfoList != null && hisCompanyOtherInfoList.Count > 0)
                            sReplaceText = hisCompanyOtherInfoList[0].ListInfo;
                        sampleName = hRecord.SampleName;
                        if (hRecord.SampleName.IndexOf('_') != -1)
                        {
                            string currentNo = hRecord.SampleName.Substring(hRecord.SampleName.LastIndexOf('_') + 1);
                            try
                            {

                                currentNum = Convert.ToInt32(currentNo);
                                sampleName = hRecord.SampleName.Substring(0, hRecord.SampleName.LastIndexOf('_'));
                            }
                            catch
                            {
                                currentNum = 1;
                            }
                        }

                        cmd.Connection = SqlConnection;
                        cmd.CommandText = INSERTSample;
                        //  SQLiteParameter param1 = new SQLiteParameter("@Id", id);
                        SqlParameter param1 = new SqlParameter("@SampleName", sampleName);
                        SqlParameter param2 = new SqlParameter("@Operater", hRecord.Operater);
                        SqlParameter param3 = new SqlParameter("@SpecDate", hRecord.SpecDate);
                        SqlParameter param4 = new SqlParameter("@Supplier", hRecord.Supplier);
                        SqlParameter param5 = new SqlParameter("@DeviceName", hRecord.DeviceName);
                        SqlParameter param6 = new SqlParameter("@WorkCurveName", workCurve.Name);
                        SqlParameter param7 = new SqlParameter("@SampleNameId", currentNum);
                        SqlParameter param8 = new SqlParameter("@OperaterNum", sReplaceText);
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);
                        cmd.Parameters.Add(param3);
                        cmd.Parameters.Add(param4);
                        cmd.Parameters.Add(param5);
                        cmd.Parameters.Add(param6);
                        cmd.Parameters.Add(param7);
                        cmd.Parameters.Add(param8);

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                    }


                    int id = 0;
                    object maxID = ExecuteSelect("select max(Id) from HistoryRecord");
                    if (maxID != DBNull.Value)
                        id = Convert.ToInt32(maxID);




                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = INSERT_ELEMVALUE;
                        cmd.Connection = SqlConnection;
                        // SQLiteParameter param1 = new SQLiteParameter("@Id", id);
                        SqlParameter param1 = new SqlParameter("@elementName", string.Empty);
                        SqlParameter param2 = new SqlParameter("@contextelementValue", '0');
                        SqlParameter param3 = new SqlParameter("@HistoryRecord_Id", id);
                        SqlParameter param4 = new SqlParameter("@UnitValue", 0);
                        SqlParameter param5 = new SqlParameter("@Error", 1);
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);
                        cmd.Parameters.Add(param3);
                        cmd.Parameters.Add(param4);
                        cmd.Parameters.Add(param5);
                        //cmd.Parameters.Add(param6);


                        //for (int i = 0; i < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; i++)
                        //{
                        //    //param1.Value = id;
                        //    param1.Value = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption; //WorkCurve.Elements[i].Oxide.Length == 0 ? WorkCurve.Elements[i].Caption : WorkCurve.Elements[i].Oxide;
                        //    param2.Value = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Content.ToString();//WorkCurve.Elements[i].Content;
                        //    param3.Value = id;//WorkCurve.Elements[i].Error;
                        //    param4.Value = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].ContentUnit; //WorkCurve.Elements[i].ElemLimit;
                        //    param5.Value = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Error;//(int)WorkCurve.Elements[i].ElemUnit;
                        //    cmd.ExecuteNonQuery();
                        //}

                        foreach (var element in elements)
                        {
                            param1.Value = element.elementName;
                            param2.Value = element.contextelementValue;
                            param3.Value = id;//WorkCurve.Elements[i].Error;
                            param4.Value = element.unitValue;
                            param5.Value = element.Error;
                            cmd.ExecuteNonQuery();
                        }


                    }
                    if (!WorkCurveHelper.IsAutoUpload)
                        Msg.Show(Info.strSpecifications);
                }
            }
            catch
            {
                Msg.Show(Info.SaveError);
            }
            finally
            {
                CloseSQL();
            }
        }

        public void AddToStaticToSQL()
        {
            try
            {
                if (SqlConnection.State != ConnectionState.Open)
                {
                    Msg.Show(Info.ConnectDatabaseError);
                }
                else
                {

                    if (specList == null || WorkCurveHelper.WorkCurveCurrent == null) return;

                    // for (int n = 0; n < selectSpeclist.Count; n++)
                    for (int n = 0; n < DifferenceDevice.interClassMain.recordList.Count; n++)
                    {
                        int currentNum = 1;
                        string sampleName = string.Empty;
                        HistoryRecord hRecord = HistoryRecord.FindById(Convert.ToInt64(DifferenceDevice.interClassMain.recordList[n]));
                        WorkCurve workCurve = WorkCurve.FindById(hRecord.WorkCurveId);
                        List<HistoryCompanyOtherInfo> hisCompanyOtherInfoList = HistoryCompanyOtherInfo.FindBySql("select * from historycompanyotherinfo where history_id = " + DifferenceDevice.interClassMain.recordList[n]);
                        string sReplaceText = "";
                        if (hisCompanyOtherInfoList != null && hisCompanyOtherInfoList.Count > 0)
                            sReplaceText = hisCompanyOtherInfoList[0].ListInfo;
                        sampleName = hRecord.SampleName;

                        if (hRecord.SampleName.IndexOf('_') != -1)
                        {
                            string currentNo = hRecord.SampleName.Substring(hRecord.SampleName.LastIndexOf('_') + 1);
                            try
                            {

                                currentNum = Convert.ToInt32(currentNo);
                                sampleName = hRecord.SampleName.Substring(0, hRecord.SampleName.LastIndexOf('_'));
                            }
                            catch
                            {
                                currentNum = 1;
                            }
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == Convert.ToInt64(DifferenceDevice.interClassMain.recordList[n]));
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = SqlConnection;
                            cmd.CommandText = INSERTSample;

                            //  SQLiteParameter param1 = new SQLiteParameter("@Id", id);
                            SqlParameter param1 = new SqlParameter("@SampleName", sampleName);
                            SqlParameter param2 = new SqlParameter("@Operater", hRecord.Operater);
                            SqlParameter param3 = new SqlParameter("@SpecDate", hRecord.SpecDate);
                            SqlParameter param4 = new SqlParameter("@Supplier", hRecord.Supplier);
                            SqlParameter param5 = new SqlParameter("@DeviceName", hRecord.DeviceName);
                            SqlParameter param6 = new SqlParameter("@WorkCurveName", workCurve.Name);
                            SqlParameter param7 = new SqlParameter("@SampleNameId", currentNum);
                            SqlParameter param8 = new SqlParameter("@OperaterNum", sReplaceText);
                            cmd.Parameters.Add(param1);
                            cmd.Parameters.Add(param2);
                            cmd.Parameters.Add(param3);
                            cmd.Parameters.Add(param4);
                            cmd.Parameters.Add(param5);
                            cmd.Parameters.Add(param6);
                            cmd.Parameters.Add(param7);
                            cmd.Parameters.Add(param8);

                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                        }


                        int id = 0;
                        object maxID = ExecuteSelect("select max(Id) from HistoryRecord");
                        if (maxID != DBNull.Value)
                            id = Convert.ToInt32(maxID);




                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = INSERT_ELEMVALUE;
                            cmd.Connection = SqlConnection;
                            // SQLiteParameter param1 = new SQLiteParameter("@Id", id);
                            SqlParameter param1 = new SqlParameter("@elementName", string.Empty);
                            SqlParameter param2 = new SqlParameter("@contextelementValue", '0');
                            SqlParameter param3 = new SqlParameter("@HistoryRecord_Id", id);
                            SqlParameter param4 = new SqlParameter("@UnitValue", 0);
                            SqlParameter param5 = new SqlParameter("@Error", 1);
                            cmd.Parameters.Add(param1);
                            cmd.Parameters.Add(param2);
                            cmd.Parameters.Add(param3);
                            cmd.Parameters.Add(param4);
                            cmd.Parameters.Add(param5);
                            //cmd.Parameters.Add(param6);


                            //  for (int i = 0; i < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; i++)
                            foreach (var element in elements)
                            {
                                //param1.Value = id;

                                //param1.Value = WorkCurveHelper.tempElemList[n].Items[i].Caption; //WorkCurve.Elements[i].Oxide.Length == 0 ? WorkCurve.Elements[i].Caption : WorkCurve.Elements[i].Oxide;
                                //param2.Value = WorkCurveHelper.tempElemList[n].Items[i].Content.ToString();//WorkCurve.Elements[i].Content;
                                //param3.Value = id;//WorkCurve.Elements[i].Error;
                                //param4.Value = WorkCurveHelper.tempElemList[n].Items[i].ContentUnit; //WorkCurve.Elements[i].ElemLimit;
                                //param5.Value = WorkCurveHelper.tempElemList[n].Items[i].Error;//(int)WorkCurve.Elements[i].ElemUnit;
                                param1.Value = element.elementName;
                                param2.Value = element.contextelementValue;
                                param3.Value = id;//WorkCurve.Elements[i].Error;
                                param4.Value = element.unitValue;
                                param5.Value = element.Error;
                                cmd.ExecuteNonQuery();
                            }


                        }
                    }
                    if (!WorkCurveHelper.IsAutoUpload)
                        Msg.Show(Info.strSpecifications);
                }
            }
            catch (Exception ex)
            {
                Msg.Show(Info.SaveError);
            }
            finally
            {
                CloseSQL();
            }

        }


        /// <summary>
        /// 加载数据库信息 pf by 20150807
        /// </summary>
        /// <returns></returns>
        public string DataBaseInfo()
        {
            string strPath = Application.StartupPath + "\\DBConnection.ini";
            constr = string.Empty;
            StringBuilder tempbuilder = new StringBuilder(255);
            string strDataSource = string.Empty;
            WinMethod.GetPrivateProfileString("Param", "DataSource", "Local", tempbuilder, 255, strPath);
            strDataSource = tempbuilder.ToString();
            constr += "Server=" + strDataSource + ";";

            WinMethod.GetPrivateProfileString("Param", "InitialCatalog", "HISTORYDATA", tempbuilder, 255, strPath);
            constr += "Initial Catalog=" + tempbuilder.ToString() + ";";

            WinMethod.GetPrivateProfileString("Param", "UserID", "sa", tempbuilder, 255, strPath);
            constr += "uid=" + tempbuilder.ToString() + ";";

            WinMethod.GetPrivateProfileString("Param", "password", "123456", tempbuilder, 255, strPath);
            constr += "pwd=" + tempbuilder.ToString();
            //constr += ";Connection Timeout=5";
            return constr;
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        public void OpenSQL()
        {
            try
            {

                //DataBaseInfo();
                //SqlConnection.ConnectionString = constr;
                Stopwatch sw = new Stopwatch();
                bool connectSuccess = false;
                Thread t = new Thread(delegate()
                {
                    try
                    {
                        sw.Start();
                        SqlConnection.Open();
                        connectSuccess = true;

                    }
                    catch { }
                }
                    );

                t.IsBackground = true;
                t.Start();
                while (10000 > sw.ElapsedMilliseconds)
                    if (t.Join(1)) break;
            }
            catch (SqlException ex)
            {
                // return false;
            }
            //return true;
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void CloseSQL()
        {
            if (SqlConnection.State != ConnectionState.Closed)
                SqlConnection.Close();
        }

        public object ExecuteSelect(string sql)
        {
            object rtn = null;
            if (SqlConnection.State == ConnectionState.Open)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = SqlConnection;
                    rtn = cmd.ExecuteScalar();
                }
            }
            return rtn;
        }


        public static string DBPATH = "Data Source=HistoryData.sdb";
        private const string INSERT_HISTORY = @"INSERT INTO HistoryRecord(SampleName,Operater,SpecDate,Supplier,DeviceName,WorkCurveName)
          values(@SampleName,@Operater,@SpecDate,@Supplier,@DeviceName,@WorkCurveName)";
        private const string INSERT_ELEMVALUE = @"INSERT INTO HistoryElement(elementName,contextelementValue,HistoryRecord_Id,UnitValue,Error)
          values (@elementName,@contextelementValue,@HistoryRecord_Id,@UnitValue,@Error)";
        private const string INSERTSample = " INSERT INTO HistoryRecord(SampleName ,Operater ,SpecDate ,Supplier,DeviceName,WorkCurveName,SampleNameId,OperaterNum) values (@SampleName ,@Operater ,@SpecDate ,@Supplier,@DeviceName,@WorkCurveName,@SampleNameId,@OperaterNum)";
        public string constr = "server=(Local);Initial Catalog=HISTORYDATA;uid=sa;pwd=123456";
        public SqlConnection SqlConnection = new SqlConnection();

        public void PrinterBlueThread(object Obj)
        {
            PrinterBlue(DifferenceDevice.interClassMain.recordList);
        }

        public void PrinterBlue(List<long> histoyIDs)
        {
            if (histoyIDs.Count <= 0) return;
            if (WorkCurveHelper.PrintName.Equals(string.Empty))
            {
                SelectBluetoothDeviceDialog dialog = new SelectBluetoothDeviceDialog();
                dialog.ShowRemembered = true;
                dialog.ShowAuthenticated = true;//显示认证过的蓝牙设备   
                dialog.ShowUnknown = true;//显示位置蓝牙设备   
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    WorkCurveHelper.PrintName = dialog.SelectedDevice.DeviceAddress.ToString();//获取选择的远程蓝牙地址   
                    bluePrint.SelectDevice(dialog.SelectedDevice.DeviceAddress);
                    ReportTemplateHelper.SaveSpecifiedValueandCreate("Report", "PrintName", dialog.SelectedDevice.DeviceAddress.ToString());
                }

            }
            if (!bluePrint.Connect())
            {
                SkyrayMsgBox.Show(Info.NoPrinter);
                return;
            }
            bool isSuccess = PrintHistoryIDsByBlue(histoyIDs);
            if (!isSuccess)
            {
                SkyrayMsgBox.Show(Info.ConnectError);
                return;

            }
        }
        private bool PrintHistoryIDsByBlue(List<long> histoyIDs)
        {
            bool bResult = false;
            int rowsCount = histoyIDs.Count;

            //if ((WorkCurveHelper.isShowXRFStandard == 1) && WorkCurveHelper.CurrentStandard != null && WorkCurveHelper.CurrentStandard.StandardDatas != null && WorkCurveHelper.CurrentStandard.StandardDatas.Count > 0)
            //    dt.Columns.Add(Info.TestResult);
            List<string> strInfos = new List<string>();
            List<string> strConts = new List<string>();
            strInfos.Add(Info.SampleName);
            strInfos.Add(Info.SpecName);
            strInfos.Add(Info.SpecDate);
            for (int i = 0; i < rowsCount; i++)
            {
                strConts.Clear();
                DataTable dt = new DataTable();
                dt.Columns.Add(Info.ElementName);
                dt.Columns.Add(Info.Content);
                HistoryRecord historyRecord = HistoryRecord.FindById(histoyIDs[i]);
                strConts.Add(historyRecord.SampleName);
                strConts.Add(historyRecord.SpecListName);
                strConts.Add(historyRecord.SpecDate.ToString());

                WorkCurve wc = WorkCurve.FindById(historyRecord.WorkCurveId);
                var workEles = WorkCurve.FindById(historyRecord.WorkCurveId)
                    .ElementList.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex).ToList();
                bool IsExsitAu = false;
                double dblAuContent = 0;
                string LayerElems = "";
                if (wc.ElementList.LayerElemsInAnalyzer != null && wc.ElementList.RhIsLayer) LayerElems = wc.ElementList.LayerElemsInAnalyzer;
                string[] strElemsLayer = Helper.ToStrs(LayerElems);
                foreach (var em in workEles)
                {
                    HistoryElement he = historyRecord.HistoryElement.ToList().Find(w => w.elementName == em.Caption);
                    if (he == null) continue;
                    if (he.elementName.ToLower().Equals("au") && DifferenceDevice.IsAnalyser)
                    {
                        IsExsitAu = true;
                        dblAuContent = double.Parse(he.contextelementValue);
                        dblAuContent = he.unitValue == 1 ? dblAuContent : (he.unitValue == 3 ? dblAuContent / 10 : dblAuContent / 10000);
                    }
                    string sunit = (he.unitValue.ToString() == "1") ? "%" : ((he.unitValue.ToString() == "3") ? "‰" : "ppm");
                    if (strElemsLayer.Contains(em.Caption)) sunit = "um";
                    DataRow dr = dt.NewRow();
                    string strHeader = Skyray.Language.Lang.Model.CurrentLang.FullName != "中文" ? em.Caption : Atoms.AtomList.Find(w => w.AtomName == em.Caption).AtomNameCN + "(" + em.Caption + ")";
                    dr[0] = strHeader;
                    dr[1] = strElemsLayer.Contains(em.Caption) ? double.Parse(he.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(" + sunit + ")" : double.Parse(he.contextelementValue).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(" + sunit + ")";
                    dt.Rows.Add(dr);
                }
                if (IsExsitAu)
                {
                    //var Au = (from l in Elements.Items where string.Compare(l.Caption, "Au", true) == 0 select l.Content).FirstOrDefault();
                    double dKValue = dblAuContent * 24 / WorkCurveHelper.KaratTranslater;
                    strConts.Add(dKValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(k)");
                    strInfos.Add("Karat");

                }
                bResult = PrinteBlueOneID(dt, strInfos, strConts);
                if (strInfos.Contains("Karat")) strInfos.Remove("Karat");
                if (!bResult) break;

            }
            return bResult;

        }
        private bool PrinteBlueOneID(DataTable dt, List<string> strInfos, List<string> strConts)
        {
            bool Result = false;

            try
            {
                // 原来
                bluePrint.Write(ESC);
                bluePrint.Write(GS);
                bluePrint.Write(ReportResult(dt, strInfos, strConts));
                bluePrint.Write(CR);
                bluePrint.Write(CR);


                Result = true;

            }
            catch
            {
                bluePrint.IsAlive = false;
                Result = false;
                //   SkyrayMsgBox.Show(Info.ConnectError);
            }
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bPrintAll"></param>
        /// <param name="dt"></param>
        /// <param name="StrSampleName">样品名称</param>
        /// <param name="strTimes">测量时间</param>
        /// <param name="strTestDate">测量日期</param>
        /// <param name="strCompany">检测单位</param>
        /// <param name="strOperater">操作员</param>
        public void PrinteBlue(bool bPrintAll, DataTable dt, string StrSampleName, string strTimes, string strTestDate, string strCompany, string strOperater)
        {
            if (!bluePrint.IsExsitPrint())
            {
                SkyrayMsgBox.Show(Info.NoBlueTooth);
                return;
            }

            //if (printlogo == null)
            //{
            //    printlogo = global::Skyray.UC.Properties.Resources.printlogo;
            //}
            if (WorkCurveHelper.PrintName.Equals(string.Empty))
            {
                SelectBluetoothDeviceDialog dialog = new SelectBluetoothDeviceDialog();
                dialog.ShowRemembered = true;
                dialog.ShowAuthenticated = true;//显示认证过的蓝牙设备   
                dialog.ShowUnknown = true;//显示位置蓝牙设备   
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    WorkCurveHelper.PrintName = dialog.SelectedDevice.DeviceAddress.ToString();//获取选择的远程蓝牙地址   
                    bluePrint.SelectDevice(dialog.SelectedDevice.DeviceAddress);
                    ReportTemplateHelper.SaveSpecifiedValueandCreate("Report", "PrintName", dialog.SelectedDevice.DeviceAddress.ToString());
                }

            }
            if (!bluePrint.Connect())
            {
                SkyrayMsgBox.Show(Info.NoPrinter);
                return;
            }
            if (WorkCurveHelper.WorkCurveCurrent == null || WorkCurveHelper.WorkCurveCurrent.ElementList == null) return;



            bool isSuccess = PrintExcelBlue(dt, StrSampleName, strTimes, strTestDate, strCompany, strOperater);
            if (!isSuccess)
            {
                SkyrayMsgBox.Show(Info.ConnectError + "。");
                return;

            }

        }

        //  private Bitmap printlogo;

        /// <summary>
        /// 清空缓冲区数据，复位所有参数
        /// </summary>
        private readonly static byte[] ESC = { 0x1B, 0x40, 0x00 };
        /// <summary>
        /// 设置左边距
        /// </summary>
        private readonly static byte[] GS = { 0x1D, 0x4C, 25, 0x00 };
        /// <summary>
        /// 打印缓冲区数据，并换行
        /// </summary>
        private readonly static byte[] LF = { 0x0A };
        /// <summary>
        /// 打印缓冲区数据，并回车
        /// </summary>
        private readonly static byte[] CR = { 0x0D };
        /// <summary>
        /// 调用字库
        /// </summary>
        private readonly static byte[] CHARS = { 0x1B, 0x38 };

        /// <summary>
        /// 行间距
        /// </summary>
        private readonly static byte[] LINESPACE = { 0x1b, 0x33, 32 };
        /// <summary>
        /// 居左
        /// </summary>
        private readonly static byte[] ALIGNL = { 0x1b, 0x61, 0 };
        /// <summary>
        /// 居中
        /// </summary>
        private readonly static byte[] ALIGNC = { 0x1b, 0x61, 49 };
        /// <summary>
        /// 居右
        /// </summary>
        private readonly static byte[] ALIGNR = { 0x1b, 0x61, 50 };



        /// <summary>
        /// /蓝牙打印
        /// </summary>
        // public void PrintExcelBlue(bool bPrintAll, DataTable dt, string StrSampleName, string StrDateTime, string strPassReslt)
        public bool PrintExcelBlue(DataTable dt, string StrSampleName, string strTimes, string strDate, string strCompany, string strOperater)
        {
            bool Result = false;

            try
            {
                // 原来
                bluePrint.Write(ESC);
                bluePrint.Write(GS);
                bluePrint.Write(ReportResult(dt, StrSampleName, strTimes, strDate, strCompany, strOperater));
                bluePrint.Write(CR);
                bluePrint.Write(CR);
                bluePrint.Write(CR);
                bluePrint.Write(CR);

                Result = true;

            }
            catch
            {
                bluePrint.IsAlive = false;
                Result = false;
                //   SkyrayMsgBox.Show(Info.ConnectError);
            }
            return Result;

        }


        private void GetReportInfo(ref Dictionary<string, string> dReportOtherInfo)
        {
            List<CompanyOthersInfo> listOtherInfo = CompanyOthersInfo.FindBySql("select * from companyothersinfo where 1=1 and Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "' ");
            HistoryRecord historyRecord = (DifferenceDevice.interClassMain.recordList.Count > 0) ? HistoryRecord.FindById(DifferenceDevice.interClassMain.recordList[DifferenceDevice.interClassMain.recordList.Count - 1]) : null;
            if (listOtherInfo != null)
            {
                foreach (var name in listOtherInfo)
                {
                    string strsql = "select * from historycompanyotherinfo a " +
                                                                  " left outer join companyothersinfo b on b.id=a.companyothersinfo_id " +
                                                                  " where a.workcurveid in (select workcurveid from historyrecord where id='" + historyRecord.Id + "') and a.history_id='" + historyRecord.Id + "' and b.ExcelModeTarget='" + name.ExcelModeTarget + "'and b.Display =1 and b.isreport=1";
                    List<HistoryCompanyOtherInfo> hisCompanyOtherInfoList = HistoryCompanyOtherInfo.FindBySql(strsql);
                    string sReplaceText = "";
                    if (hisCompanyOtherInfoList != null && hisCompanyOtherInfoList.Count > 0)
                        sReplaceText = hisCompanyOtherInfoList[0].ListInfo;
                    if (name.Display)
                        dReportOtherInfo.Add(name.Name, sReplaceText);
                }
            }


        }

        public string ReportResult(DataTable dt, string strSampleName, string strTimes, string strDate, string strCompany, string strOperater)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}: {1}", Info.SampleName, strSampleName));
            sb.Append(Environment.NewLine);
            sb.Append(string.Format("{0}: {1}", Info.MeasureTime, strTimes));
            sb.Append(Environment.NewLine);
            sb.Append(string.Format("{0}: {1}", Info.SpecDate, strDate));
            sb.Append(Environment.NewLine);
            sb.Append(string.Format("{0}: {1}", Info.Curve, WorkCurveHelper.WorkCurveCurrent.Name));
            sb.Append(Environment.NewLine);

            //公司其它信息
            Dictionary<string, string> dReportOtherInfo = new Dictionary<string, string>();
            GetReportInfo(ref dReportOtherInfo);
            foreach (var otherinfo in dReportOtherInfo)
            {
                sb.Append(string.Format("{0}: {1}", otherinfo.Key, otherinfo.Value));
                sb.Append(Environment.NewLine);
            }
            sb.Append("-------------------------");
            sb.Append(Environment.NewLine);


            sb.Append(string.Format("{0}", Info.TestResult));
            sb.Append(Environment.NewLine);
            sb.Append(string.Format("{0,-10} {1}", Info.ElementName, Info.Content));
            sb.Append(Environment.NewLine);
            int allColCount = dt.Columns.Count;
            string str = string.Empty;
            int cnt = 1;
            for (int m = 0; m < dt.Rows.Count; m++)
            {
                sb.Append(string.Format("{0,-10} {1}", dt.Rows[m][0].ToString() + dt.Rows[m][2].ToString(), dt.Rows[m][1]).ToString());
                sb.Append(Environment.NewLine);
                //for (int i = 0; i < allColCount - cnt; i++)
                //{
                //    sb.Append(string.Format("{0,-10} {1}", dt.Columns[i + 1].Caption, dt.Rows[m][i + 1]).ToString());
                //    sb.Append(Environment.NewLine);
                //}
                //if ((WorkCurveHelper.CurrentStandard != null) && !string.IsNullOrEmpty(WorkCurveHelper.CurrentStandard.StandardName))
                //{
                //    sb.Append(string.Format("{0}: {1}", Info.strPassReslt, dt.Rows[m][dt.Columns.Count - 1].ToString()));
                //}
                //sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
        public string ReportResult(DataTable dt, List<string> PrinterStringInfos, List<string> PrinterStringCons)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            int count = PrinterStringInfos.Count;
            for (int i = 0; i < count; i++)
            {
                sb.Append(string.Format("{0}: {1}", PrinterStringInfos[i], PrinterStringCons[i]));
                sb.Append(Environment.NewLine);
            }
            sb.Append("-------------------------");
            sb.Append(Environment.NewLine);

            int allColCount = dt.Columns.Count;
            string str = string.Empty;
            for (int i = 0; i < allColCount; i++)
            {
                if (i == 0 || i == allColCount - 1) str += String.Format("{0,-8}", dt.Columns[i].Caption);
                else str += String.Format("{0,-8}", dt.Columns[i].Caption);
            }
            sb.Append(str);
            sb.Append(Environment.NewLine);
            int AllrowCount = dt.Rows.Count;
            for (int i = 0; i < AllrowCount; i++)
            {
                str = string.Empty;
                for (int j = 0; j < allColCount; j++)
                {
                    if (j == 0 || i == allColCount - j) str += String.Format("{0,-8}", dt.Rows[i][j].ToString());
                    else str += String.Format("{0,-8}", dt.Rows[i][j].ToString());
                }
                sb.Append(str);
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        //public string ReportResult(DataTable dt, string strSampleName, string strTimes, string strDate, string strCompany, string strOperater)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(string.Format("{0}: {1}", Info.SampleName, strSampleName));
        //    sb.Append(Environment.NewLine);
        //    sb.Append(string.Format("{0}: {1}", Info.MeasureTime, strTimes));
        //    sb.Append(Environment.NewLine);
        //    sb.Append(string.Format("{0}: {1}", Info.SpecDate, strDate));
        //    sb.Append(Environment.NewLine);
        //    sb.Append(string.Format("{0}: {1}", Info.Curve, WorkCurveHelper.WorkCurveCurrent.Name));
        //    sb.Append(Environment.NewLine);

        //    //公司其它信息
        //    Dictionary<string, string> dReportOtherInfo = new Dictionary<string, string>();
        //    GetReportInfo(ref dReportOtherInfo);
        //    foreach (var otherinfo in dReportOtherInfo)
        //    {
        //        sb.Append(string.Format("{0}: {1}", otherinfo.Key, otherinfo.Value));
        //        sb.Append(Environment.NewLine);
        //    }
        //    sb.Append("-------------------------");
        //    sb.Append(Environment.NewLine);

        //    int allColCount = dt.Columns.Count;
        //    string str = string.Empty;
        //    int cnt = 1;
        //    if ((WorkCurveHelper.CurrentStandard != null) && !string.IsNullOrEmpty(WorkCurveHelper.CurrentStandard.StandardName))
        //        cnt = 2;

        //    for (int m = 0; m < dt.Rows.Count; m++)
        //    {
        //        sb.Append(string.Format("{0}: {1}", Info.TestResult, (m + 1).ToString()));
        //        sb.Append(Environment.NewLine);
        //        sb.Append(string.Format("{0,-10} {1}", Info.ElementName, Info.Content));
        //        sb.Append(Environment.NewLine);
        //        for (int i = 0; i < allColCount - cnt; i++)
        //        {
        //            sb.Append(string.Format("{0,-10} {1}", dt.Columns[i + 1].Caption, dt.Rows[m][i + 1]).ToString());
        //            sb.Append(Environment.NewLine);
        //        }
        //        if ((WorkCurveHelper.CurrentStandard != null) && !string.IsNullOrEmpty(WorkCurveHelper.CurrentStandard.StandardName))
        //        {
        //            sb.Append(string.Format("{0}: {1}", Info.strPassReslt, dt.Rows[m][dt.Columns.Count - 1].ToString()));
        //        }
        //        sb.Append(Environment.NewLine);
        //    }

        //    return sb.ToString();
        //}
        #region 根据配置文件，放大xrfChart的X轴指定区间
        private bool SetAxisXRange(XRFChart target, double min, double max)
        {
            if (target == null || !WorkCurveHelper.IsSetXRange || min > max)
                return false;
            if (min < 0)
                min = 0;
            if (max > 4096)
                max = 4096;
            target.SetXRange(min, max);
            return true;
        }
        #endregion

        #region 关盖测试
        ///// <summary>
        ///// 满足条件则开始测量
        ///// </summary>
        //public void DoTestOnCoverClosed()
        //{
        //    if (!WorkCurveHelper.TestOnCoverClosedEnabled)
        //        return;
        //    if (deviceMeasure.interfacce.State != DeviceState.Idel)
        //        return;
        //    if (CanDoTestOnCoverClosed && !deviceMeasure.interfacce.ReturnCoverClosed)
        //    {
        //        if (TimesCheckCoverClosed < MaxTimesCheckCoverClosed)
        //            TimesCheckCoverClosed++;
        //        if (TimesCheckCoverClosed >= MaxTimesCheckCoverClosed)
        //        {
        //            CanDoTestOnCoverClosed = false;
        //            TimesCheckCoverClosed = 0;
        //            StartTest(DateTime.Now.ToString("yyyyMMddHHmmss"), string.Empty);
        //        }
        //    }
        //    else if (deviceMeasure.interfacce.ReturnCoverClosed)
        //    {
        //        CanDoTestOnCoverClosed = true;
        //        TimesCheckCoverClosed = 0;
        //    }
        //}



        int staticTime = 0;
        private bool isFirstChangeCoverState = true;
        private bool isLastCover = true;
        private bool isAutoCloseVoltage = true;
        private CoverState CurrentCoverState = CoverState.None;
        private CoverState LastCoverState = CoverState.None;
        /// <summary>
        /// 打开盖子时，电磁阀门使用工作
        /// </summary>
        public void DoOpenCoverStartElect()
        {
            if (deviceMeasure.interfacce.State == DeviceState.Idel)
            {
                if (deviceMeasure.interfacce.ReturnCoverClosed)
                    CurrentCoverState = CoverState.OpenCover;
                else
                    CurrentCoverState = CoverState.CloseCover;


                if (CurrentCoverState == CoverState.OpenCover && LastCoverState != CoverState.OpenCover) //盖子打开
                {
                    staticTime = 0;
                    //deviceMeasure.interfacce.Pump.TOpen();//电磁阀置1

                    //  deviceMeasure.interfacce.Pump.TClose(); //电磁阀置0
                    isFirstChangeCoverState = false;
                    LastCoverState = CoverState.OpenCover;
                }
                else if (CurrentCoverState == CoverState.CloseCover && LastCoverState != CoverState.CloseCover) //盖子关闭
                {
                    staticTime = 0;
                    //  deviceMeasure.interfacce.Pump.TClose(); //电磁阀置0

                    //  deviceMeasure.interfacce.Pump.TOpen();//电磁阀置1
                    if (isAutoCloseVoltage)
                    {
                        if (WorkCurveHelper.IsUseElect && WorkCurveHelper.IsContinueVol)
                        {
                            if (this.deviceMeasure != null && this.deviceMeasure.interfacce != null)
                                this.deviceMeasure.interfacce.SetOpenCurrent();
                        }
                        isFirstChangeCoverState = false;
                        LastCoverState = CoverState.CloseCover;
                    }
                }

                if (staticTime >= 360 && isAutoCloseVoltage) //500ms刷新一次 //超过300s，isAutoCloseVoltage为true时半闭一次
                {
                    //超过三分钟关高压
                    deviceMeasure.interfacce.port.CloseVoltage();
                    deviceMeasure.interfacce.port.CloseVoltageLamp();
                    WorkCurveHelper.deviceMeasure.interfacce.IsDropTime = true;
                    staticTime = 0;
                    isAutoCloseVoltage = false;
                }
                else if (isAutoCloseVoltage)
                {
                    staticTime++;
                    ////开始高压
                    //deviceMeasure.interfacce.port.OpenVoltage();
                    //Thread.Sleep(100);
                    //deviceMeasure.interfacce.port.OpenVoltageLamp();

                    //超过三分钟关高压
                    //deviceMeasure.interfacce.port.CloseVoltage();
                    //deviceMeasure.interfacce.port.CloseVoltageLamp();
                }
                else
                {
                    staticTime = 0;
                }


            }
            else
            {
                isFirstChangeCoverState = true;  //开始测试后就需要重置状态
                isLastCover = true;
                isAutoCloseVoltage = true;
                staticTime = 0;
            }

        }
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();


        public void DoOpenCoverInOutSample()
        {
            //sw.Stop();
            //string showtime = sw.Elapsed.TotalMilliseconds.ToString();
            //Console.WriteLine("-------刷新间隔：{0}ms", sw.Elapsed.TotalMilliseconds);
            //sw.Reset();
            //sw.Start();
            if (deviceMeasure.interfacce.State == DeviceState.Idel)
            {
                if (deviceMeasure.interfacce.ReturnCoverClosed)
                    CurrentCoverState = CoverState.OpenCover;
                else
                    CurrentCoverState = CoverState.CloseCover;

                if (WorkCurveHelper.bMotorRestart || !WorkCurveHelper.IsResetMotor)
                {
                    if (CurrentCoverState == CoverState.OpenCover && LastCoverState == CoverState.CloseCover) //盖子打开
                    {
                        //开盖出样
                        OUTSample();
                        LastCoverState = CoverState.OpenCover;
                    }
                    else if (CurrentCoverState == CoverState.CloseCover && LastCoverState == CoverState.OpenCover) //盖子关闭
                    {

                        //关盖进样
                        INSample();
                        LastCoverState = CoverState.CloseCover;
                    }
                    else if (CurrentCoverState == CoverState.OpenCover && LastCoverState == CoverState.None) //盖子打开
                    {
                        //复位后第一次开盖
                        //开盖出样
                        OUTSample();
                        LastCoverState = CoverState.OpenCover;
                    }
                    else if (CurrentCoverState == CoverState.CloseCover && LastCoverState == CoverState.None) //盖子关闭
                    {
                        LastCoverState = CoverState.CloseCover;
                    }

                }


            }






        }




        public void StartTest(string sampleName, string supplier)
        {
            StartTest(sampleName, supplier, null);
        }

        /// <summary>
        /// 创建参数并执行测量
        /// </summary>
        public void StartTest(string sampleName, string supplier, List<CompanyOthersInfo> list)
        {
            List<WordCureTest> localWorkCurve = new List<WordCureTest>();
            string strSampleName = sampleName;
            SpecListEntity specList = new SpecListEntity
                (
                strSampleName,
                strSampleName,
                deviceMeasure.interfacce.ReturnEncoderValue,
                0.0,
                supplier,
                0,
                "",
                FrmLogon.userName,
                DateTime.Now,
                "",
                SpecType.UnKownSpec,
                DifferenceDevice.DefaultSpecColor.ToArgb(),
                DifferenceDevice.DefaultSpecColor.ToArgb()
                )
            {
                Loss = 0.0
            };


            WordCureTest test = new WordCureTest()
            {
                WordCurveName = WorkCurveHelper.WorkCurveCurrent.Name,
                Spec = specList,
                SerialNumber = "0"
            };

            if (list != null)
            {
                test.CompanyInfoList = list;
            }

            localWorkCurve.Add(test);
            int dropTime = 0;
            int.TryParse(ReportTemplateHelper.LoadSpecifiedValueNoWait("TestParams", "DropTime"), out dropTime);
            MeasureParams MeasureParams = new MeasureParams
                (
                1,
                dropTime,
                false
                );
            TestDevicePassedParams testDevicePassedParams = new TestDevicePassedParams
                (
                false,
                MeasureParams,
                localWorkCurve,
                WorkCurveHelper.DeviceCurrent.IsAllowOpenCover,
                SpecType.UnKownSpec,
                "",
                false,
                true
                );
            DifferenceDevice.MediumAccess.ExcuteTestStart(testDevicePassedParams);
        }


        public void clickStart()
        {




            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
            {
                DifferenceDevice.CurCameraControl.skyrayCamera1.BackgroundImageLayout = ImageLayout.Stretch;

                Bitmap testDemoImg = DifferenceDevice.CurCameraControl.skyrayCamera1.GrabImage();

                DifferenceDevice.CurCameraControl.skyrayCamera1.BackgroundImage = testDemoImg;

                DifferenceDevice.CurCameraControl.skyrayCamera1.Stop();

                MotorOperator.MotorOperatorY1Thread((int)(-WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));

            }

            WorkCurveHelper.testNum = 1;
            WorkCurveHelper.testTimes = 1;
            WorkCurveHelper.curDeviceNum = 0;

            string sampleName = "";
            if (DifferenceDevice.CurCameraControl.ChkRetest.Checked && this.specList.SampleName != null)
                sampleName = (this.specList.SampleName.Contains("#") ? this.specList.SampleName.Split(new char[] { '#' })[0] : this.specList.SampleName) + "#" + DateTime.Now.ToString("yyyyMMddHHmmss");
            else
                sampleName = DateTime.Now.ToString("yyyyMMddHHmmss");
            StartTest(sampleName, string.Empty);
        }

        public void clickStop()
        {


            this.deviceMeasure.Stop();
            CameraRefMotor.CancelAll();
            TestStartAfterControlState(true);
            skyrayCamera.ClearContiMeasurePoint();
            progressInfo.Value = 0;
            this.XrfChart.ClearInformation();
            ModbusTcp.slave.DataStore.CoilDiscretes[1] = false;
            ModbusTcp.slave.DataStore.CoilDiscretes[3] = false;
            ModbusTcp.slave.DataStore.InputDiscretes[2] = false;
            WorkCurveHelper.stopDoing = 0;
            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
            {
                MotorOperator.MotorOperatorY1Thread((int)(WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                this.skyrayCamera.Start();
            }

        }


        #region 一键测试:按下硬件按钮,进行测试
        public void DoTestOnButtonPressed()
        {


            if (deviceMeasure.interfacce.State != DeviceState.Idel)
            {
                //如果正在测量中，按下了测试按钮，则清空按键信息，防止测量完成后继续测量
                if (MotorOperator.returnCmdState != CMDState.StopTest)
                {
                    MotorOperator.returnCmdState = CMDState.None;
                    return;
                }
                if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.TestButtonPressed)
                {
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.TestButtonPressed = false;
                    return;
                }
            }

            if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && deviceMeasure.interfacce.ReturnCoverClosed)
            {
                TimesCheckCoverClosed2 = 0;
                return;
            }
            else
            {
                if (TimesCheckCoverClosed2 < MaxTimesCheckCoverClosed)
                    TimesCheckCoverClosed2++;
            }
            if ((TimesCheckCoverClosed2 >= MaxTimesCheckCoverClosed))
            {
                if (MotorOperator.returnCmdState == CMDState.StartTest || deviceMeasure.interfacce.TestButtonPressed)
                {
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.TestButtonPressed = false;
                    Bitmap testDemoImg = DifferenceDevice.CurCameraControl.skyrayCamera1.GrabImage();
                    WorkCurveHelper.reportImage = testDemoImg;

                    clickStart();
                }
                else if (MotorOperator.returnCmdState == CMDState.StopTest)
                {
                    clickStop();
                }
                TimesCheckCoverClosed2 = 0;
                MotorOperator.returnCmdState = CMDState.None;

            }

        }

        #endregion


        #region 追加Rohs点检功能
        public delegate void ReceiveDataElse(ElementList elems, float TotalTime, float UsedTime); //能量转换为道
        public delegate void EndTestElse(ElementList elems); //到转换为能量
        public ReceiveDataElse DoOtherFormReceive = null;
        public EndTestElse DoOtherFormEndTest = null;
        #endregion

        #region 用于连接时, 禁用控件, 防止多次连接等问题
        public void SetNavEnabled(Form owner, List<string> navNameList, bool enabled)
        {
            if (owner == null
                || navNameList == null
                || navNameList.Count <= 0
                || WorkCurveHelper.NaviItems == null
                || WorkCurveHelper.NaviItems.Count <= 0)
                return;
            WorkCurveHelper.SafeCallAsync(owner, () =>
            {
                navNameList.ForEach(name =>
                {
                    var nav = WorkCurveHelper.NaviItems.Find(item => item.Name == name);
                    if (nav != null)
                    {
                        nav.EnabledControl = enabled;
                    }
                });
            });

        }

        public void DoAfterDevInfoNotGot(Form frm)
        {
            if (frm == null)
                throw new ArgumentNullException("frm");
            WorkCurveHelper.SafeCallAsync(frm, () =>
            {
                Msg.Show(Info.FailedToGetDevInfo);
                WorkCurveHelper.AskToClose = false;
                frm.Close();
            });
        }

        #endregion

        #region 批量测试
        private List<CompanyOthersInfo> SetOtherInfo(RecordInfo ri)
        {
            if (ri == null || ri.OtherInfoDictionary == null || ri.OtherInfoDictionary.Count <= 0)
                return null;
            var listOtherInfo = CompanyOthersInfo.FindBySql("select * from companyothersinfo where 1=1 and Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "' ");
            WorkCurveHelper.SeleCompanyOthersInfo.Clear();
            foreach (var name in listOtherInfo)
            {
                if (ri.OtherInfoDictionary.Contains(name.Name))
                {
                    name.DefaultValue = ri.OtherInfoDictionary[name.Name].ToString();
                    WorkCurveHelper.SeleCompanyOthersInfo.Add(name.Id.ToString(), name.DefaultValue);
                    name.Save();
                }
            }
            return listOtherInfo;
        }
        public bool StartTest(RecordInfo ri, ref string msg)
        {
            if (ri == null)
            {
                msg = "RecordInfo is null";
                return false;
            }
            WorkCurveHelper.IsBatchTest = true;
            WorkCurveHelper.IsTestNormal = false;
            if (this.ActionAfterTestFinished == null)
            {
                this.ActionAfterTestFinished = () =>
                {
                    WorkCurveHelper.IsTestNormal = !this.deviceMeasure.interfacce.StopFlag;
                    if (WorkCurveHelper.IsBatchTest && DifferenceDevice.interClassMain.MainForm is IBatchTest)
                    {
                        var test = DifferenceDevice.interClassMain.MainForm as IBatchTest;
                        if (test.ActionAfterTestFinished != null)
                        {
                            test.ActionAfterTestFinished();
                        }
                    }
                    WorkCurveHelper.IsBatchTest = false;
                };
            }

            StartTest(ri.SampleName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"), ri.Supplier, SetOtherInfo(ri));
            return true;
        }
        #endregion
    }


}

        #endregion