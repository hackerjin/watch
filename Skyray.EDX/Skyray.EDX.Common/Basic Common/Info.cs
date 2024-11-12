using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Skyray.Controls;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 多语言组件
    /// </summary>
    public class Info
    {
        public static string Remarks = "注： ND代表含量小于等于";//+WorkCurveHelper.NDValue.ToString()+"ppm";
        public static string Subface = "X荧光仪器分析测得的数据为表面测试";
        public static string TotalContent = "Cr,Br为测得该元素的总含量，如果其显示超标并不代表VI价Cr和PBB，PBDE超标。";
        public static string Count = "计数";
        public static string Channel = "通道";
        public static string Energy = "能量";
        public static string Lushu = "卤素";
        public static string Ka = "Ka";
        public static string Kb = "Kb";
        public static string La = "La";
        public static string Lb = "Lb";
        public static string Lr = "Lr";
        public static string Le = "Le";
        public static string Description = "描述信息";
        public static string WorkingCurve = "工作曲线";
        public static string CaculateTime = "计算日期";
        public static string CurveExport = "曲线导入";
        public static string DeleteSpecFile = "选择谱文件已经删除！";
        public static string DeleteSpecListsFail = "谱已打开删除操作无效";
        public static string TemplateNoExists = "指定模板不存在，无法进行导出操作。";
        public static string ExcelInstallHint = "未安装Excel，无法操作。";
        public static string TotalPassReslt = "总判定";
        public static string DataDelete = "数据库中已经删除或者移除谱文件！";
        public static string WorkCurveDelete = "工作曲线已经删除，无法操作！";
        public static string NoLoadSource = "数据源加载失败！";
        public static string ExportMaxCount = "超过数据导出记录，请确认数据(最大记录数目为{0})";
        public static string ComputeMatching = "计算匹配度";
        public static string DataOptimization1 = "数据优化1";
        public static string DataOptimization2 = "数据优化2";
        public static string DataOptimization3 = "数据优化3";
        public static string DataOptimization4 = "数据优化4";
        public static string DataOptimization5 = "数据优化5";
        public static string PeakChannel = "峰通道";
        public static string CountTimes = "次数";
        public static string VacuumDegree = "真空度";
        public static string VacuumDegree2 = "真空度2";
        public static string Summary = "描述";
        public static string SelectICOFile = "请选择.ico文件！";
        public static string CaptionMaxLength = "化合物名称不能超过20个字符！";
        public static string About = "关于";
        public static string ConditionCanotDel = "智能条件与匹配条件不能删除！";
        public static string ConditionCanotBe = "测量条件不能取名为：";
        public static string InitParam = "初始化参数";
        public static string MAXELT = "感兴趣元素不能超出35个！";
        public static string InfoPnl = "信息栏";
        public static string NaviPnl = "导航栏";
        public static string SetSuccess = "设置成功！";
        public static string OnlySingleInstance = "软件已打开！";
        public static string PWDError = "密码错误！";
        public static string RemoveAll = "您确定要删除全部记录吗？";
        public static string SelectConditionExist = "被选条件已经存在";
        public static string SelectCurveExist = "被选曲线已经存在！";
        public static string IncludingAu = "Karat";
        public static string IsExitApplication = " 是否退出应用程序？";
        public static string ChannelError = "初始化误差道";
        public static string FileFormatError = "文件格式错误！";
        public static string TemplatePathEmpty = "模板路径不能为空！";
        public static string SpecDate = "测量日期";
        public static string Operator = "操作员";
        public static string SelectChamber = "请选择样品杯！";
        public static string PauseStop = "暂停";
        public static string NoSelectTemplate = "当前没有选择默认的打印模板！";
        public static string ExcelNoPath = "请选择导出Excel路径！";
        public static string ResolveCaculate = "测量分辨率";
        public static string Resolve = "分辨率";
        public static string CaculateAfterPrint = "请计算后再进行打印！";
        public static string ComputeResolve = "计算分辨率";
        public static string SelectError = "请正确选择元素和测量条件。";
        public static string DistrubAlert = "{0}元素受到{1}元素的干扰！";
        public static string ErrFileFormat = "文件格式有误！";
        public static string SpecificationsCategory = "规格类别";
        public static string SampleSpecMatch = "标样谱匹配";
        public static string NormalCondition = "正常";
        public static string PeakMove = "此数据可能不可靠，需要重新初始化？";
        public static string MatchParamsSuccess = "根据当前能量刻度，重新计算主要元素的边界修改成功！";
        public static string VoltageInfo = "钥匙未打开或者高压未启动！";
        public static string MathInfo = "当前匹配度为{0}小于设置匹配度下限{1}，匹配失败！";
        public static string NoDeleteSpec = "不能删除当前谱，当前谱已经在使用！";
        public static string DeviceNoPort = "请确定仪器端口设置。";
        public static string Delete = "删除";
        public static string OpenWorkSpec = "打开工作谱";
        public static string OpenVirtualSpec = "打开对比谱";
        public static string MustAddDeviceInfo = "必须添加当前仪器信息！";
        public static string AddDeviceInfo = "添加当前仪器信息？";
        public static string TestNoProcess = "测试次数为零！";
        public static string PleaseCorrectWeight = "请正确输入重量信息！";
        public static string MeasureConditionInvalidate = "测量条件中的子条件个数为零，请添加后再操作！";
        public static string LoadLibrayFailure = "加载动态库失败！";
        public static string CoverOpen = "请关闭仪器上盖！";
        public static string suspendTest = "已暂停测试！";
        public static string Weight = "重量";
        public static string HistoryConditionNotNull = "名称不能为空！";
        public static string SelectElementNotTable = "输入的元素名称有误！";
        public static string WorkCurveMeasureCondition = "当前谱的条件个数小于测量条件个数！";
        public static string CoverSilimarSpecList = "数据库中存在相同的谱名称，是否替换？";
        public static string SelectHistoryRecord = "没有选择历史记录！";
        public static string CurrentIntArea = "当前感兴趣区的面积是：";
        public static string ElementName = "元素";
        public static string CurveType = "曲线类型";
        public static string ConditionName = "测试条件";
        public static string NotFindDogy = "没有找到加密狗！";
        public static string PasswardDogyExpire = "密码狗到期！";
        public static string SuplusDogyTime = "剩余小时：";
        public static string LeftHour = "离授权结束日还有{0}小时";
        public static string LeftDay = "离授权结束日还有{0}天";
        public static string SuplusDayTime = "剩余天数：";
        public static string Start = "开始";
        public static string NoExistDecrobate = "当前曲线不存在能量刻度，请添加后再操作！";
        public static string MainElementsIntensity = "主元素在当前谱中不存在数据，请确认该元素的计算范围。";
        public static string NoRegionSelect = "没有区域被选中！";
        public static string CrClSample = "正在测量的样品可能是塑料！";
        public static string FeSample = "正在测量的样品可能是钢铁！";
        public static string FeCrSample = "正在测量的样品可能是不锈钢！";
        public static string CuZnSample = "正在测量的样品可能是铜及其它有色金属！";
        public static string SnSample = "正在测量的样品可能是焊锡！";
        public static string PlasticSample = "正在测量的样品可能是塑胶及其它！";
        public static string MgAlSample = "正在测量的样品可能是镁铝！";
        public static string TargetMotor = "靶材电机正在移动";
        public static string ChamberMotor = "样品转盘电机正在移动";
        public static string TargetMotorEnd = "靶材电机移动结束！";
        public static string ChamberMotorEnd = "样品转盘电机移动结束！";
        public static string CollimatMotor = "准直器电机正在移动到目标位置：";
        public static string CollimatMotorEnd = "准直器电机移动结束！";
        public static string FilterMotor = "滤光片电机正在移动...";
        public static string FilterMotorEnd = "滤光片电机移动结束！";
        public static string TimePumpOpen = "真空泵打开，正按照时间抽真空！";
        public static string SpacePumpOpen = "真空泵打开，正按照真空度抽真空！";
        public static string MotorStopMove = "电机停止移动.";
        public static string SpectrumTest = "正在测试过程中...";
        public static string OpenVoltagePreHeat = "正在预热过程中";
        public static string SpectrumEnd = "当前测试结束！";
        public static string SpectrumStop = "终止测试！";
        public static string continueSuspendTest = "继续被暂停测试？";
        public static string NetDeviceDisConnection = "连接已经断开，请重新连接";
        public static string NetDeviceConnection = "设备已连接";
        public static string NoDeviceConnect = "设备未连接，请连接设备";
        public static string DeviceConnecting = "设备连接中...";
        public static string SpectrumInitialize = "正在初始化过程中";
        public static string ConnectionDevice = "设备未连接！";
        public static string OpenVoltagePreHeatEnd = "预热前阶段结束";
        public static string InitailizeEnd = "初始化结束.";
        public static string Gain = "粗调码";
        public static string FineGain = "细调码";
        public static string AddressContext = "地址：江苏省昆山市玉山镇中华园西路1888号天瑞产业园   邮编：215300";
        public static string Address = "地址";
        public static string TelephoneContext = "电话：800-9993-800      传真： 0512－57017060";
        public static string Telephone = "联系方式";
        public static string MailContext = "E-Mail:service@skyray-instrument.com";
        public static string Mail = "邮箱地址";
        public static string NetWorkAddressContext = "网址: http://www.skyray-instrument.com";
        public static string NetWorkAddress = "网址";
        public static string CompanyNameContext = "江苏天瑞仪器有限公司";
        public static string CompanyName = "公司名称";
        public static string Administrator = "管理员";
        public static string StandandUser = "技术员";
        public static string CommonUser = "操作员";
        public static string NetWorkParams = "网格参数";
        public static string AdjustDistance = "调整距离";
        public static string TestSampleNameValidate = "请填写样品信息！";
        public static string PleaseWriteTestTimes = "请正确填写测试次数信息！";
        public static string PleaseWriteNameAndCode = "请选择样品杯编号及样品名称！";
        public static string MoveWorkStation = "移动平台";
        public static string ChamberMove = "样品转盘";
        public static string DeviceNotExistsState = "设备不在空闲状态！";
        public static string SubmitMotorInformation = "请确认电机相关信息!";
        public static string ExpressionInvalidate = "表达式无效！请重新输入。";
        public static string WorkCurveNoSpecList = "当前谱没有谱数据！";
        public static string PleaseSelectSpecType = "请选择测试谱的类型！";
        public static string StandandSampleSpecInfo = "在条件列表中不存在对应的匹配条件，请加入固定的匹配条件再操作！";
        public static string InitializeSuccess = "初始化成功！";
        public static string StandandSample = "标样谱";
        public static string PureElement = "纯元素";
        public static string Test = "测试";
        public static string XaisMove = "X轴 移动";
        public static string YaisMove = "Y轴 移动";
        public static string ZaisMove = "Z轴 移动";
        public static string MessageBoxTextProgrammableMeasureIsSureAdjust = "确定要校正吗？";
        public static string SpecifiedConditionNoExits = "指定的条件不存在当前的数据库中，请添加后再操作！";
        public static string StartInitFailure = "初始化失败";
        public static string WarningInitInfo = "初始化信息";
        public static string Thick = "厚度";
        public static string ConfirmDel = "确认删除吗？";
        public static string InitialInformation = "初始化信息提示";
        public static string MaxValue = "最大值";
        public static string MinValue = "最小值";
        public static string MeanValue = "平均值";
        public static string SDValue = "标准偏差";
        public static string RSDValue = "相对标准偏差";
        public static string Number = "测量次数";
        public static string MeasureTime = "测量时间";
        public static string Voltage = "管压";
        public static string Current = "管流";
        public static string Filter = "滤光片";
        public static string Collimator = "准直器";
        public static string VacuumizeByTime = "时间抽真空";
        public static string AdjustCountRate = "调节计数率";
        public static string MaxCountRate = "最大计数率";
        public static string MinCountRate = "最小计数率";
        public static string CountRate = "计数率";
        public static string CountBits = "计算参数";
        public static string CurrentCountRate = "实时";
        public static string AverageCountRate = "平均";
        public static string InitalElem = "初始化元素";
        public static string InitalChann = "初始化通道";
        public static string PutInitSample = "请放入标样";
        public static string PleaseCurveCalibrationSample = "请放入标样：";
        public static string InitialFaile = "初始化失败";
        public static string Save = "保存";
        public static string Cancel = "取消";
        public static string Normal = "正常模式";
        public static string Intelligent = "智能模式";
        public static string IntelligentCondition = "智能";
        public static string Match = "匹配";
        public static string Settings = "系统设置";
        public static string SysSettings = "系统参数设置";
        public static string ChoiceSetting = "保存设置";
        public static string DBSetting = "数据库设置";
        public static string SerialSetting = "串口设置";
        public static string DetectPointsSetting = "多点检测参数设置";
        public static string Condition = "测量条件";
        public static string SettingMeasureTime = "设置测量时间";
        public static string Curve = "工作曲线";
        public static string Workgion = "工作区";
        public static string Quality = "定性分析";
        public static string ExploreAnalysis = "智能分析";
        public static string Spec = "工作谱";
        public static string UserManage = "用户管理";
        public static string Tools = "选项";
        public static string Report = "分析报告";
        public static string TemplateModel = "模板类型";//Add by Strong 2012/10/15
        public static string OldTemplate = "模板选择";
        public static string PrintSetting = "打印设置";
        public static string ReportSetting = "保存路径";
        public static string NormalTemplate = "标准模板";//Add by Strong 2012/10/15
        public static string DefiniteTemplate = "自定义模板";
        public static string Print = "打印报告";
        public static string StorePrint = "保存报告";
        public static string Style = "风格";
        public static string Language = "语言";
        public static string Helper = "帮助";
        public static string Others = "其他";
        public static string Shape = "形状";
        public static string Supplier = "供应商";
        public static string OpenCondition = "打开条件";
        public static string EditWorkgion = "编辑工作区";
        public static string OpenCurve = "打开曲线";
        public static string EditElement = "编辑元素";
        public static string ElementRef = "影响元素";
        public static string DataOptimization = "数据优化";
        public static string EditData = "编辑数据";
        public static string CustomFiled = "自定义";
        public static string CreateIntRegion = "建立感兴趣区";
        public static string CaculateIntRegion = "计算感兴趣区面积";
        public static string OpenSpec = "打开谱";
        //public static string CoeeParamSet = "谱处理参数设置";
        public static string AnalysisParam = "分析参数";
        public static string AnalysisReport = "定性分析结果";
        public static string CalcIntensity = "计算强度";
        public static string CalcResolution = "计算分辨率";
        public static string CheckCurve = "校正曲线";
        public static string DefineAnalysis = "定性分析";
        public static string WorkElement = "工作谱";
        public static string TestResult = "测试结果";
        public static string ConnectTestResult = "连测结果";
        public static string ConnectStaticsResult = "连测统计";
        public static string StatisticsInfo = "统计信息";
        public static string ThickStatisticsInfo = "测厚统计";
        public static string Suggestion = "提示";
        public static string Statics = "统计";
        public static string MainPage = "主页";
        public static string PreHeat = "预热";
        public static string HistoryRecord = "历史记录";
        public static string TestWarning = "测试提醒信息";
        public static string WarningTestContext = "请打开工作曲线后再操作！";
        public static string NoSelect = "未选择！";
        public static string WorkCurveBeUsed = "当前曲线使用中，不可删除！";
        public static string IsNull = "内容不能为空！";
        public static string BeUsed = "被使用,不能删除！";
        public static string LayNum = "所在层";
        public static string ValidateUserInput = "当前输入中存在非法输入，请重新编辑。";
        public static string WarningUserInput = "用户管理输入信息";
        public static string TestSetting = "测试设置";
        public static string Initialization = "初始化";
        public static string StopTest = "停止";
        public static string Caculate = "计算";
        public static string MatchParams = "匹配元素边界校正";
        public static string SetMatchborder = "匹配元素边界校正";
        public static string ConnectDevice = "连接仪器";
        public static string AutoDemarcateEnergy = "自动峰标";
        public static string Device = "设备";
        public static string ElementMain = "主元素";
        public static string PeakDivBase = "峰背比";
        public static string BaseLow = "背景左界";
        public static string BaseHigh = "背景右界";
        public static string Difference = "差额";
        public static string Pattern = "模式";
        public static string Substrate = "基材";
        public static string FirstLayer = "第一层";
        public static string SecondLayer = "第二层";
        public static string ThirdLayer = "第三层";
        public static string ForthLayer = "第四层";
        public static string FifthLayer = "第五层";
        public static string NewTestSampleInfo = "请正确填写样品信息。";
        public static string InputInvalidate = "输入验证";
        public static string NewTestCurve = "请正确填写工作曲线信息。";
        public static string SelectSampleType = "请选择样品类型";
        public static string SelectSampleWarning = "样品类型选择信息。";
        //public static string NewTestMatch = "请正确选择模式。";
        public static string DataError = "数据错误！";
        //public static string HavePresence = "已存在！";
        public static string NoExistsWorkCurve = "请打开工作曲线，再执行此操作！";
        public static string OpenWordCurve = "警告信息";
        public static string DisplayPeakFlag = "显示峰标识";
        public static string AutoAnalysis = "自动分析";
        public static string ManualAnalysis = "手动分析";
        public static string DisplayElement = "显示元素";
        public static string AddVirtualSpec = "对比谱";
        public static string OpenRohs3 = "打开(RoHS3谱)";
        public static string OpenRohs4 = "打开(RoHS4谱)";
        public static string DisappearBk = "削本底";
        //public static string Samplelog = "对数谱";
        public static string ComputeSampleIntensity = "计算样品强度";
        public static string VirtualSpec = "对比谱";
        public static string NameIsNull = "名称不能为空！";
        public static string NormalMode = "正常模式";
        public static string SmartMode = "智能模式";
        public static string ChangeLanguage = "切换语言";
        public static string EditLanguage = "编辑语言";
        public static string PureElementSpecData = "纯元素谱";
        public static string DeleteDevice = "删除设备将删除该设备下所有信息，确定删除？";
        public static string ExistName = "当前名称已使用，请选择其它名称！";
        public static string NameRepeat = "名称重复！";
        public static string StyleError = "数据格式错误！";
        public static string OutOfRange = "数据超出范围！";
        public static string NeedConicPoint = "二次曲线至少需要三个不同的点！";
        public static string NeedThirdPoint = "至少需要四个不同的点！";
        public static string NoDataToSave = "无数据可以保存！";
        public static string CanotDelCurrentDevice = "不能删除当前设备！";
        public static string DeleteCurve = "确定删除当前工作曲线？";
        public static string ExistCustomName = "自定义中含有同名项，添加失败！";

        /// <summary>
        /// 复制到剪贴板出错
        /// </summary>
        public static string MessageBoxTextSystemCopyToClipboardFailed = "复制到剪贴板出错！";

        public static string StandardStone = "标准库";
        public static string Specifications = "规格标准";
        public static string SelectElement = "请选择元素";
        public static string SelectSpec = "请选择谱";
        public static string SelectBackSpec = "请选择本底谱";
        public static string CanotDel = "不能删除！";
        public static string PleaseSaveLang = "请先保存之前添加的语言！";
        public static string SelectLayer = "请选择层级！";
        public static string SingleLayer = "单层";
        public static string MultiLayer = "多层";

        public static string CurveName = "曲线名称";
        public static string SampleInfo = "样品信息";
        public static string SampleName = "样品名称";
        public static string SpecName = "谱名称";
        /// <summary>
        /// 报告名称设置
        /// </summary>
        public static string ReportName = "报告名称";
        //
        public static string SampleGraph = "谱图";
        public static string SampleImage = "样品图";
        public static string Intensity = "强度";
        public static string Content = "含量";
        //public static string Weight = "重量";

        //public static string SelectSampleFirst = "请先选择标准！";
        public static string IllegalInput = "输入含有非法字符！";
        //public static string UnNeedReference = "只有纯元素拟合和Fixed纯元素拟合算法需要拟合元素！";
        public static string NeedReference = "纯元素拟合算法需要拟合元素！";
        //public static string NeedSpecData = "拟合元素需要纯元素谱数据！";
        //public static string MaxMustBigThanMin = "最大记数率不得小于最小记数率！";
        public static string CustomNameOrExprissionNull = "自定义名称和表达式不能为空！";
        //public static string IsVacuumSelectBoth = "不可同时选择时间抽真空和真空度抽真空！";
        //public static string CurrentStandardExistElement = "当前标准中已包含该元素！";
        public static string IsOrNotCompareIntensity = "强度比较";
        //public static string ComparisonCoefficient = "阈值";
        public static string BPeakLow = "α/β峰左界";
        public static string BPeakHigh = "α/β峰右界";
        //public static string ThicknessUnit = "厚度单位";
        public static string HaveNoSuitSpectrum = "没有相匹配的纯元素谱！";
        public static string fitElements = "拟合元素";
        public static string NeedSpecs = "需要纯元素谱数据！";
        //public static string ElementCountOutRange = "超出元素最大个数！";
        //public static string AtLeastOneCondition = "至少需要一条测试条件，删除失败！";
        //public static string BeUsedCannotUpdate = "被使用，名称不可修改！";
        public static string VisualSpecOutRange = "对比谱个数超出最大{0}个范围！";
        public static string CustomNameMustDifferenceWithElement = "自定义名称不可与元素名称相同！";
        public static string CustomNameOrExprissionRepeat = "自定义名称和表达式不能重复！";
        public static string OptimizationValue = "值";
        //public static string DeviceParamterOutRange = "测量条件数量超出最大值！";
        public static string EditContent = "含量";
        //public static string CaculateIntensityFalse = "强度计算失败！";
        public static string SelectWorkRegion = "请选择工作区！";
        public static string DeleteDefaultWorkRegion = "不能删除默认工作区！";
        public static string NotDeleteCurrentCategory = "不能删除当前类别！";
        public static string EnterFullExampleName = "请输入完整规格名称！";
        public static string Atmospheric = "SMC压力表";
        public static string VacuumSi = "真空硅管";
        public static string Fixed = "真空硅2";
        public static string ChinawareDataBase = "陶瓷数据库";
        public static string ReportSpecification = "报规格";
        public static string BrassCurve = "黄铜曲线";
        public static string BrassStatics = "黄铜统计结果";
        public static string IPSettings = "网口设置";
        public static string SurfaceSourceSettings = "面光源设置";
        public static string SilimarSampleInfo = "样品名称重名，请重新输入！";
        public static string SilimarSpectrum = "谱名称重名！";
        public static string CreateTitle = "标题设置";
        public static string RohsTemplate = "Rohs模板";

        public static string ExportSuccess = "导出成功,是否打开查看？";

        public static string FunctionConfig = "功能配置";

        public static string CompanyOtherInformation = "样品其他信息";

        public static string UIConfig = "界面配置";
        public static string FpSpecCalibrate = "谱处理参数设置";
        public static string RExcusionCalibrate = "强度偏移校正";
        public static string OpenExcelOrNot = "是否打开";
        public static string OrignalIntensity = "原始强度";
        public static string CalibrateIntensity = "校正强度";

        public static string containerObjectMain = "主窗口";
        public static string containerObjectRight = "右侧窗口";
        public static string containerObjectLeft = "左侧窗口";
        public static string containerObjectGraphAndSample = "谱图和样品图";
        public static string containerObjectGraph = "谱图";
        public static string containerObjectCameral = "样品图";
        public static string containerObjectProcessBarAndStart = "时间条、开始结束按钮和小谱图";
        public static string containerObjectProcessBar = "时间条";
        public static string containerObjectXLine = "元素信息";
        public static string containerObjectEnergy = "能量信息";
        public static string containerObjectCurve = "曲线信息";
        public static string containerObjectSpecAndTubeStatus = "管状态和谱线信息";
        public static string containerObjectTubeStatus = "管状态";
        public static string containerObjectSpec = "谱线";
        public static string containerObjectButtonsAndDrop = "按钮组和滴水图";
        public static string containerObjectDrop = "滴水图";
        public static string containerObjectButtons = "按钮组";

        public static string strIntensityIsNull = "主元素标样强度不能为0！";
        public static string strContentIsNull = "主元素标样含量不能为0！";

        public static string strGainSet = "调节放大倍数";

        public static string SysConfig = "系统配置";
        public static string SampleType = "样品类型";
        public static string SpecType = "谱类型";

        public static string NameSetting = "存储名称规则";

        public static string strHistoryRecordContinuoust = "连测历史记录";
        public static string strHistoryRecordContinuoustList = "连测历史记录明细";
        public static string strSameHotKeys = "不能选择相同的快捷键！";
        public static string CurveCalibrate = "曲线校正";
        public static string MoveStationUp = "移动平台向上";
        public static string MoveStationDown = "移动平台向下";
        public static string MoveStationStop = "移动平台停止";

        public static string strIpAddress = "只能输入数字0-9和“.”如：(192.168.1.1)";
        public static string strSpecifications = "数据保存成功！";
        public static string strErrorSpecLength = "导入谱文件长度和当前设备谱长度不对应！";
        public static string strSeleWorkCurveCurrent = "请选择当前工作曲线！";
        public static string strIsRemoveBk = "请先选择是否削本底！";
        public static string strSpecListInfo = "请选择当前谱文件！";
        public static string strDropTime = "缓冲时间不能大于测量时间！";
        public static string strOpenExcel = "当前文件正在使用，请先关闭！";
        public static string strJudgeMatchAndFile = "匹配和匹配谱存在没有对应的记录！";
        public static string strIsSeleElement = "是否选择感兴趣元素！";
        public static string strAsc = "升序";
        public static string strDesc = "降序";
        public static string strAdjustCountFail = "调节计数率失败，当前最大管流为{0}";
        public static string strExpunction = "消去值";
        public static string strLayerNumber = "基础层元素不参加测量！";
        public static string MatchLevelSetting = "匹配度设定";
        public static string NitonAlloyGrade = "牌号库管理";
        public static string ShowDialog = "是否弹出测量结果";

        public static string strstandardname = "标准库名";
        //20110615 何晓明 备份还原
        public static string BackUpAndRestore = "备份还原";
        public static string BackUpSuccessed = "备份成功，是否打开？";
        public static string NeedReStartApplication = "进行还原后需要重启软件，是否继续？";
        public static string RestoreSuccessed = "还原成功，程序即将退出！";
        public static string BackUpAndRestoreFailed = "操作失败，原因：";
        public static string strNoExitspecInfo = "当前谱文件可能不存在！";

        public static string strLevelInfo = "EC非基材层，必须存在单层标样参与曲线！";

        public static string strCameraControlInfo = "不存在网口摄像头设备！";
        public static string strCameraControlErrorInfo = "IP设置失败，请重新设置！";

        public static string SpecOutRange = "谱个数超出10个的最大范围！";

        public static string strThickAccuracy = "计算精度值不在范围之内！";

        public static string strGotoInfo = "值不在范围之内！";

        public static string CrClinPlastic = "测塑料中CrCl";
        public static string CrCdPbHginSteel = "测钢铁中CrCdPbHg";
        public static string CrCdPbHginBrassZinc = "测有色金属中CrCdPbHg";
        public static string CrCdPbHginSolder = "测焊锡中CrCdPbHg";
        public static string Polyethylene = "测塑胶及其它";
        public static string CrCdPbHginMagnalium = "测镁铝中CrCdPbHg";

        public static string strOther = "其他";
        //何晓明 20110713 热键开放
        public static string HotKeyExists = "已启用该热键，是否覆盖？";
        //何晓明 20100715 Rohs中达模板多语言切换
        public static string OneTimeTemplate = "ExcelReportModelCN.xls";
        public static string ManyTimeTemplate = "ExcelRetestReportModelCN.xls";

        public static string isExitMatchSpecList = "选择的谱已经在其他工作曲线中为匹配谱！";

        public static string strTargetModelSelect = "模式选择";

        public static string strTotalCount = "总计数";

        public static string strArea = "面积";

        public static string strError = "误差";

        public static string strSimpleName = "样品名称";

        public static string strSubmissionDate = "送检日期";

        public static string strLotNo = "来料批次";

        public static string strStandard = "标准";

        public static string strUserColumnManage = "用户列管理";
        public static string strUserAddColumn = "列名";


        public static string strSeleWorkCurveName = "不能选择多个工作曲线！";

        public static string strBasicInfo = "基本信息";
        public static string strPassReslt = "判定";
        public static string strRestrictStandard = "限定标准";
        public static string strLonNo = "批号";
        public static string strTotalPassReslt = "总判定";
        public static string strElementAllName = "元素全名称";
        public static string strElementName = "元素名称";
        public static string strHistoryRecordCode = "编号";
        public static string strCorrectViewFailed = "画面宽高不能低于5mm，校正失败！";

        public static string OptModeStr = "测量模式";
        public static string strNormal = "正常";
        public static string strAbnormity = "异常";
        public static string strHVLock = "高压开关";
        public static string strHighVoltage = "高压";
        public static string strDetector = "探测器";
        public static string strVacuumPump = "真空泵";
        public static string strPleaseInspect = "请检查！";
        public static string strDetectionFaild = "仪器自检失败！";
        public static string strDetectionSuccess = "仪器自检成功。";
        public static string strDetectionDemarcateFail = "自检标定失败！";
        public static string strDetectionDemarcateSuccess = "自检标定成功，请点击应用生效！";
        public static string strDetection = "自检";
        public static string strDetecting = "检测中...";
        public static string strContainsCn = "内容包含中文";

        public static string strCoverSpecName = "数据库中存在相同的样品名称，是否递增？";
        public static string strUnit = "单位";

        public static string strOxide = "氧化物";
        public static string strOtherInfo = "报表信息，不能在此添加！";
        public static string strErrorLoss = "烧失量不能大于100！";
        //public static string TotalSelected = "全选样品腔";
        public static string TargetIndex = "靶材";
        public static string TargetMode = "靶材模式";
        public static string TubCurrentRatio = "管流比例因子";
        public static string MatchSpec = "匹配谱";

        public static string RhfactorErrorInfo = "Rh是镀层输入值为0至1";
        //public static string DropTimeBeyondAdjustTime = "丢包时间不能";

        public static string OpenPDFOrNot = "是否打开PDF";
        public static string OpenXMLOrNot = "是否打开XML";
        public static string OpenImageOrNot = "是否打开图片";

        public static string Order = "顺序";

        public static string Exit = "退出";

        public static string ExploreCaculateError = "智能模式计算出错";

        public static string AddCondition = "请增加测量条件！";
        public static string ImportInvalid = "导入无效！";
        public static string ConditionCountInvalid = "条件个数不一致！";
        public static string CreateSimilarCondition = "创建相同条件名称！条件名称为：";
        public static string Auto = "自动";
        public static string AverageMode = "平均选项";
        public static string ConcluteMode = "校正模式";
        public static string Manual = "手动";
        public static string ShowResultInMain = "弹出结果";

        public static string IsLogOut = "  是否注销？";
        public static string PWDLock = "密码保护";
        public static string RangePlus = "范围+";
        public static string RangeSub = "范围-";
        public static string ManySampleTest = "测试完成！请重新选择下一个点，点击确定测试继续。";

        public static string BrassSpecification = "规格";
        public static string BrassReportName = "光谱分析测试报告";
        public static string EditCurveName = "修改曲线";
        public static string ParamsConfig = "参数配置";
        public static string AddNode = "添加";
        public static string NotFindDevice = "没有找到设备！";

        public static string Total = "共计";
        public static string Article = "条";

        public static string UnKownSpecOtherSpecSameName = "待分析谱名称和标样谱或纯元素谱名称相同";

        public static string ActualVoltage = "实际管压";

        public static string ActualCurrent = "实际管流";

        public static string HistoryRecordCode = "流水号";

        public static string OpenOldSpec = "打开文件谱";
        public static string LogSpectrum = "对数谱";
        public static string SaveScreenshots = "保存截屏";
        public static string PrintScreenshots = "打印截屏";


        public static string CurrentSpec = "新格式谱";
        public static string OldSpec = "旧格式谱";
        public static string XRFCommon = "XRF通用";
        public static string XRFCopper = "XRF黄铜";
        public static string ExtendParams = "扩展参数";
        public static string DemoMath = "模拟匹配";
        public static string AutoMatch = "自动匹配";
        //public static string FindData = "发现老版谱数据，是否升级？";

        public static string SaveResults = "保存结果";
        public static string PrintResults = "打印结果";
        public static string PrintHeader = "打印标题";
        public static string MainPeakLeft = "主峰左界";
        public static string MainPeakRight = "主峰右界";
        public static string BaseArea = "背景面积";
        public static string MainArea = "主峰面积";
        public static string ContextNotify = "备注：1为大于，0为小于";
        public static string IronOrNoIron = "金属与非金属：";
        public static string Iron = "金属";
        public static string NoIron = "非金属";
        public static string FolderName = "备份路径或者文件夹名称不能空！";
        public static string SetSuccessParams = "设置成功，程序需重启生效，确认退出！";

        //public static string sAdd = "新增";
        //public static string sCover = "覆盖";

        public static string sCompressDatabaseInfo = "压缩成功！";
        public static string Continue = "继续";
        public static string Stock = "库存";
        public static string Status = "状态";
        public static string Storage = "入库";
        public static string Extraction = "出库";
        public static string Spin = "自旋";

        public static string SerialNumber = "序号";

        public static string MatchFail = "匹配失败!";

        public static string ElectricalRepeat = "请检查电机编号！";

        public static string IsPrintReport = "是否保存报告？";


        public static string NoStandardAlertInfo = "注意！此样品不是一个标准样品，它可能包含未知的元素。";
        //public static string NoStandardAlertInfo = "注意！此检测物为非正规标样,元素熔点差异过大并可能存在分布不均匀现象,检测结果仅供参考。";
        //public static string NoStandardAlertInfo = "Atention!This sample is not a formal standared sample,elements melting point difference is too large and the 
        //                                           possible distribution of uneven phenomenon,test results for reference only "; //英文

        public static string strTextBox = "文本";
        public static string strComboBox = "下拉列表";
        public static string strDateTimePicker = "时间";
        public static string strInitCondition = "初始化条件";
        public static string strAreaDensity = "面密度";
        public static string strAreaDensityUnit = "g/cm^2";

        public static string strWarningSettings = "报警设置";

        //打印模板
        public static string strPrintModel1 = "Rohs4模板";
        public static string strPrintModel2 = "新版的打印方式";
        public static string strPrintModel3 = "全国花";
        public static string strPrintModel4 = "新马德";
        public static string strPrintModel5 = "浩川金属";
        public static string strPrintModel6 = "印度模板";
        public static string strPrintModel7 = "黄铜模板";
        public static string strPrintModel8 = "XRF模板";
        public static string strPrintModel9 = "贵金属模板";
        public static string strPrintModel10 = "ROHSLeSi";
        public static string strPrintModel11 = "刘永清模板";
        public static string strPrintModel12 = "Thick模板";
        public static string strPrintModel13 = "Test_Report_WithSpecModel";
        public static string strPrintModel14 = "Test_Report_WithoutSpecModel";
        public static string strPrintModel15 = "印度模板2";

        //Camara
        public static string TestInfoMsg1 = "第{0}次测试,共{1}次测试。";
        public static string TestInfoMsg2 = "请单击[确定]后继续测试!";

        public static string DataBaseBackUP = "请在导出之前备份SQL数据库，数据库的内容重新同步,会将之前的数据删除。是否继续？";

        public static string CorrectCompleted = "强度校正成功!";

        public static string SelectAll = "选择所有记录";
        public static string CancelSelectAll = "取消选择记录";
        public static string ChamberIndex = "样品腔杯位";
        public static string Error = "错误!";
        public static string StandLine = "标准线性";
        //public static string DetailResult = "详细结果";
        //public static string SmartAnalyze = "贵金属一键智能测试";

        public static string PeakLeft = "峰左界";
        public static string PeakRight = "峰右界";
        public static string PureAu = "高纯金";
        public static string Range = "极差";
        public static string SoftManual = "软件说明";
        public static string MachineManual = "硬件说明";
        public static string Uncertainty = "不确定度";
        public static string TestSample = "测试样";
        public static string Combined = "合成";
        public static string Expanded = "扩展";
        public static string PrinHistory = "历史记录打印";
        public static string ExportHistory = "按模板导出";
        /// <summary>
        /// 计算出错
        /// </summary>
        public static string MessageBoxTextCalculateWrong = "计算过程异常，请确认样品与标定曲线是否匹配！";
        public static string JudgeStandard = "判断标准";
        public static string BackColor = "背景颜色";
        public static string ForeColor = "字体颜色";
        public static string FontSize = "字体大小";
        public static string Display = "显示内容";
        public static string CheckLine = "检量线";
        public static string Laser = "激光";
        public static string Shell = "保护罩";
        public static string On = "打开";
        public static string Off = "关闭";
        public static string UncertaintyMsg = "至少需要三次测试结果！";
        public static string UncertaintyStandard = "计算方法依据 GB18043-2013";



        ///硬件加密狗        

        public static string NetWorkFalse = "无法验证，网络连接失败,请插拔网线之后重启仪器。";
        public static string SNFalse = "设备 SN 验证失败，请联系天瑞仪器。";
        public static string PartSNFalse = "部件 SN 验证失败，请联系天瑞仪器，输入重授权码。";
        public static string RestTimeFalse = "获取设备剩余时间失败，请联系天瑞仪器。";
        public static string VerFalse = "获取设备版本号失败，请联系天瑞仪器。";
        public static string TimeOut = "已过期，请联系天瑞仪器。";

        public static string Rest = "还剩下";
        public static string DayOut = "天过期";
        public static string HVer = "硬件版本";
        public static string SVer = "软件版本";

        public static string ActiveSucceed = "授权完毕，重启仪器。";
        public static string ActiveFailed = "授权失败，请重开软件，重授权。";
        public static string HardwareDogcbItemsDPP = "多道";
        public static string HardwareDogcbItemsKV = "高压电源";
        public static string HardwareDogcbItemsXRay = "X射线管";
        public static string HardwareDogcbItemsTime = "过期时间";

        public static string Forever = "永久有效";
        public static string NewDeviceName = "新设备名称";
        public static string NewName = "名称";
        public static string ConnectDatabaseError = "连接数据库异常";
        public static string SaveError = "保存出错，请检查";
        public static string HighStandSample = "高标";
        public static string LowStandSample = "低标";
        public static string TrueValue = "真实值:";
        public static string TheoryValue = "理论值:";
        public static string LowestSampleArea = "所测样品的计数太低或测量时间太短，计算值不可靠！";
        public static string VacummLess = "真空度过低，待释放！";
        public static string ChangeSampleInfo = "修改样品信息";
        public static string DeviceName = "设备名称：";
        public static string ActiveDecode = "重授权码：";
        public static string DeviceState = "设备状态：";
        public static string ActualTime = "实际时间：";
        public static string EndTime = "过期时间：";
        public static string DeviceVersion = "设备版本：";
        public static string DppSn = "多道SN：";
        public static string VoltageSn = "高压SN：";
        public static string XraySn = "X射线SN：";
        public static string Active = "激活";
        public static string Verify = "验证";
        public static string ContactTel = "联系电话：";
        public static string ContactMail = "联系邮箱：";
        public static string ContactDer = "请将设备名称发到上述邮箱，并注明贵司全称，请在邮件发出后拨打联系电话确认，我司收到邮件后会发送一个解码文件给您！";
        public static string ResultJudgeRange = "注：除Br、Cr外,判断标准范围是上下";
        public static string FileNotExist = "文件不存在";
        public static string StandSampleLibrary = "标准样品库";
        public static string PleaseSelectGradeName = "请选择牌号名称:";
        public static string PleaseSelectGradeType = "请选择牌号类型:";
        public static string PleaseSelectType = "请选择查询类型:";
        public static string ParamFileBroken = "参数文件损坏，请重新安装软件！";
        public static string Grade = "牌号";
        public static string Type = "型号";
        public static string MatchDegree = "匹配度";
        public static string Weighting = "权重";
        public static string AccordGradeName = "按牌号名称";
        public static string AccordGradeType = "按牌号类型";
        public static string AccordElemContent = "按元素含量";
        public static string RangeScreen = "范围筛选";
        public static string SingleQuery = "单个查询";
        public static string BatchQuery = "批量查询";
        public static string ContentRange = "含量范围";
        public static string ShowGradenamedataInfo = "自该版本开始，生成的gradename.dat格式有变化（若用于之前版本或其它软件则无需选择）";
        public static string ShowNitonLabInfo = "用于编辑NitonLab.xls后重新读取";
        public static string ShowGradeExportInfo = "导出完成后请将param.dat/gradename.dat/GradeDll.dll覆盖到软件对应位置";
        public static string FileEditCloseExcel = "文件正在编辑状态，请先关闭Excel！";
        public static string FileBuildError = "生成文件有误";
        public static string GradeFileExportSuccess = "导出成功";
        public static string GradeType = "类型";
        public static string QueryResult = "查询结果";
        public static string QueryRecord = "牌号记录";

        #region SetHistoryRecordInfo Members
        //前缀shri为SetHistoryRecordInfo缩写
        public static string Shri_Setting = "设置";
        public static string Shri_SetRangeColor = "设定区间和颜色";
        public static string Shri_Query = "查询";
        public static string Shri_Modify = "修改";
        public static string Shri_Add = "新增";
        public static string Shri_Delete = "删除";
        public static string Shri_Cancel = "取消";
        public static string Shri_AddColumns = "添加列";
        public static string Shri_Save = "保存";
        public static string Shri_WorkRegion = "工作区";
        public static string Shri_Element = "元素";
        public static string Shri_MinValue = "区间最小值(>=)";
        public static string Shri_MaxValue = "区间最大值(<)";
        public static string Shri_FillColor = "背景颜色";
        public static string Shri_TextColor = "字体颜色";
        public static string Shri_Content = "元素说明";
        public static string Shri_Name = "列名";
        public static string Shri_Value = "值";
        public static string Shri_Position = "位置(从0开始倒数第几列)";
        public static string Shri_LoadXmlFailed = "加载Xml文件失败";

        #endregion

        public static string BlueSetting = "蓝牙设置";
        public static string BluePrint = "蓝牙打印";
        public static string NoBlueTooth = "没有蓝牙设备";
        public static string ConnectError = "连接过程有中断，请重新打印。";
        public static string BlueScaning = "蓝牙搜索中....";
        public static string BlueScanComplete = "搜索完成。";
        public static string NoPrinter = "未连接打印机";
        public static string PrintWaitInfo = "正在打印，请稍等...";

        public static string ThinDatabase = "清理数据库";
        public static string UnderProgress = "正在处理...";
        public static string Done = "已完成";
        public static string UnderProgressPlsWait = "当前或其他任务正在执行中，请等待";

        //关盖自动完成单次测量
        public static string TestOnCoverClosed = "关盖测试";
        public static string TestOnCoverClosedEnabled = "允许关盖测试";

        //一键测试
        public static string TestOnButtonPressed = "一键测试";
        public static string TestOnButtonPressedEnabled = "允许一键测试";

        public static string Canceled = "已取消";
        public static string UnderReleaseSpace = "正在压缩数据库, 请等待...";

        //数据库添加密码
        public static string DataBaseInvalid = "数据库不可用!";
        public static string FailedToGetDevInfo = "获取设备信息失败";
        public static string DataBaseNotExist = "数据库不存在!";
        public static string SmartAnalyze = "贵金属一键智能测试";

        public static string DeviceQualified = "仪器合格";
        public static string DeviceUnqualified = "仪器不合格。请再次检查选择的工作曲线或校验仪器";


        public static string BatchTest = "批量测试";
        public static string BTUnderBatchTest = "任务正在进行中, 暂时不能进行此操作";
        public static string BTSampleNameCanNotBeEmpty = "样品名称不能为空";
        public static string BTOrderNumber = "序号";
        public static string BTState = "状态";
        public static string BTSampleName = "样品名称";
        public static string BTSupplier = "供应商";
        public static string BTColumnCountLessThanCommand = "列数不能小于所需字段数";
        public static string BTColumnsNotIncludeInExcel = "Excel不包含以下列, 请检查";
        public static string BTOr = "或者";
        public static string BTFailedToLoadRecordInfo = "打开界面时加载RecordInfo失败";
        public static string BTSucceedToInput = "导入成功";
        public static string BTFailedToInput = "读取Excel失败";
        public static string BTSucceedToOutput = "导出成功";
        public static string BTFailedToOutput = "导出失败";
        public static string BTPlsInputDataFirst = "请先导入数据";
        public static string BTErrorOccurredContinueOrNot = "样品测试错误, 是否继续下一个测试";
        public static string BTCurrrentFinishedContinueOrNot = "样品测试已完成, 请更换下一个样品";
        public static string BTAllFinished = "测试已完成(点击我,关闭此提示)";
        public static string BTSureToExit = "确定退出吗?";
        public static string BTNoDataInDgv = "表格中没有数据";

        public static string CustomMsgBoxYes = "是(&Y)";
        public static string CustomMsgBoxNo = "否(&N)";
        public static string CustomMsgBoxCancel = "取消(&C)";
        public static string CustomMsgBoxOK = "确定(&O)";
        public static string CustomMsgBoxIgnore = "忽略(&I)";
        public static string CustomMsgBoxAbort = "放弃(&A)";
        public static string CustomMsgBoxRetry = "重试(&R)";
        public static string HideZeroContentElements = "隐藏零含量元素";
        public static string DataBase = "数据库";

        public static string CustomDataTip = "使用曲线自定义项的名字，例如Cl+Br,CaO等";
        public static string Reset = "复位";
        public static string SetReset = "设置复位点";
        public static string NotFocusPoint = "没有聚焦的点";

        public static string MotorError = "电机移动错误";
        public static string Dpp100Error = "Dpp 错误：";
        public static string CheckSerialNumber = "请检查Dpp的序列号。";
        public static string CheckSpiLine = "探测器上电加载失败，请重启仪器!";
        public static string CurrentWorkCurve = "当前工作曲线: ";
        public static string EncoderValue = "编码器";
        public static string PureSpecLib = "纯元素谱库";
        public static string Height = "高度";
        public static string ThickCalibrate = "厚度补偿库";
        public static string Expression = "公式";
        public static string DefinePureElem = "自定义纯元素";
        public static string PureSpecLibNoData = "需要纯元素谱库";
        public static string CalcAngleHeight = "高度值";
        public static string AdjustPureSpecLib = "校正谱库";
        public static string CoeffNotNumber = "非数字";
        public static string Authorization = "权限授权";
        public static string InSample = "进样";
        public static string OutSample = "出样";
        public static string TabParams = "平台参数设置";
        public static string AdjustInitial = "校正初始化";
        public static string AutoCheck = "自动校正";
        public static string AutoFocus = "自动对焦";
        public static string AdjustFinshed = "校正已成功！";
        public static string AutoCheckSucc = "自动校正完成！";
        public static string ShowDefinition = "清晰度";
        public static string StopDefinition = "停止清晰度";
        public static string NotFocus = "没有找到焦点";
        public static string NotInAtoms = "不在元素周期表范围,请重新设置";
        public static string Camera = "影像";
        public static string FocusArea = "摄像头聚焦区域";
        public static string Min = "聚焦区域（小）";
        public static string Middle = "聚焦区域（中）";
        public static string Max = "聚焦区域（大）";
        public static string CalcParams = "计算设置";
        public static string Edit = "编辑";
        public static string Complete = "完成";
        public static string DeleteSure = "确认删除吗？";
        public static string CameraZoomIn = "放大";
        public static string CameraZoomOut = "缩小";
        public static string CameraZoom = "恢复";
        public static string BaseAdjust = "基材校正";
        public static string HeatMap = "热力图";
        public static string PleasePutInBase = "请放入基材标样";
        public static string programMode = "编程模式";
        public static string ChangeUser = "切换用户";
        
    }
}
