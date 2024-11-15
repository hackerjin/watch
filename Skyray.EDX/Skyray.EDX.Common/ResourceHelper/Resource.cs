using System;
using System.Resources;
namespace Skyray.EDX.Common
{
    public static class Resource
    {
        #region 资源定义
        /// <summary>
        /// 关于
        /// About
        /// <summary>
        public static string About;
        /// <summary>
        /// 确定
        /// OK
        /// <summary>
        public static string Accept;
        /// <summary>
        /// 添加
        /// Add
        /// <summary>
        public static string Add;
        /// <summary>
        /// 新建工作谱
        /// Add New Spectrum
        /// <summary>
        public static string AddNewSpectrum;
        /// <summary>
        /// 添加峰标识
        /// Add Peak Label
        /// <summary>
        public static string AddPeak;
        /// <summary>
        /// 调节计数率
        /// Adjust Count Rate
        /// <summary>
        public static string AdjustCountRate;
        /// <summary>
        /// 校正系数
        /// Adjust coefficient
        /// <summary>
        public static string Ajustcoefficient;
        /// <summary>
        /// 全部结果
        /// All information
        /// <summary>
        public static string AllInfo;
        /// <summary>
        /// 分析
        /// Analysis
        /// <summary>
        public static string Analysis;
        /// <summary>
        /// 确认要退出吗？
        /// Are you sure?
        /// <summary>
        public static string AskClose;
        /// <summary>
        /// 背右界
        /// Base Right
        /// <summary>
        public static string BaseHigh;
        /// <summary>
        /// 背左界
        /// Base Left
        /// <summary>
        public static string BaseLow;
        /// <summary>
        /// 初始道
        /// Channel Begin
        /// <summary>
        public static string BeginChannel;
        /// <summary>
        /// 亮度
        /// Brightness
        /// <summary>
        public static string Brightness;
        /// <summary>
        /// 计算
        /// Calculate
        /// <summary>
        public static string Calculate;
        /// <summary>
        /// 校正曲线
        /// Calibration
        /// <summary>
        public static string Calibration;
        /// <summary>
        /// 摄像头
        /// Camera
        /// <summary>
        public static string Camera;
        /// <summary>
        /// 取消
        /// Cancel
        /// <summary>
        public static string Cancel;
        /// <summary>
        /// 画面
        /// Canves
        /// <summary>
        public static string Canves;
        /// <summary>
        /// 样品腔电机
        /// Motor with chamber 
        /// <summary>
        public static string ChamberMotor;
        /// <summary>
        /// 通道
        /// Channel
        /// <summary>
        public static string Channel;
        /// <summary>
        /// 清空
        /// ClearAll
        /// <summary>
        public static string ClearAll;
        /// <summary>
        /// 关闭
        /// Close
        /// <summary>
        public static string Close;
        /// <summary>
        /// 粗调
        /// Amplification(C)
        /// <summary>
        public static string CoarseCode;
        /// <summary>
        /// 准直器
        /// Collimator
        /// <summary>
        public static string Collimator;
        /// <summary>
        /// 准直器电机
        /// Motor with Collimator
        /// <summary>
        public static string ColliMotor;
        /// <summary>
        /// 颜色
        /// Color
        /// <summary>
        public static string Color;
        /// <summary>
        /// 测试条件编号
        /// Condition ID
        /// <summary>
        public static string ConditionID;
        /// <summary>
        /// 提示
        /// Hint
        /// <summary>
        public static string Confirm;
        /// <summary>
        /// Tel:0512-57017022 Mail:pme@skyray-instrument.com
        /// Tel:0512-57017022 Mail:pme@skyray-instrument.com
        /// <summary>
        public static string Contect;
        /// <summary>
        /// 含量
        /// Content
        /// <summary>
        public static string Content;
        /// <summary>
        /// 误差
        /// Error
        /// <summary>
        public static string ContentError;
        /// <summary>
        /// 含量校正
        /// Content Revise
        /// <summary>
        public static string ContentRevise;
        /// <summary>
        /// 对比度
        /// Contrast
        /// <summary>
        public static string Contrast;
        /// <summary>
        /// 影响系数
        /// Correlation Coefficient
        /// <summary>
        public static string CorrelationCoefficient;
        /// <summary>
        /// 影响元素
        /// Correlation Element
        /// <summary>
        public static string CorrelationElement;
        /// <summary>
        /// 计数
        /// Count
        /// <summary>
        public static string Count;
        /// <summary>
        /// 计数率太高，请降低管压！
        /// Counts are to high, please select lower HV and Current!
        /// <summary>
        public static string CountHighWarn;
        /// <summary>
        /// 计数率
        /// Count Rate
        /// <summary>
        public static string CountRate;
        /// <summary>
        /// 管流
        /// Current
        /// <summary>
        public static string Current;
        /// <summary>
        /// 当前测量信息
        /// Currently infotmation
        /// <summary>
        public static string CurrentlyInfo;
        /// <summary>
        /// 自定义
        /// CustomField
        /// <summary>
        public static string CustomField;
        /// <summary>
        /// 数据
        /// Data
        /// <summary>
        public static string Data;
        /// <summary>
        /// 只能输入数字！
        /// It must be a number!
        /// <summary>
        public static string DataWrong;
        /// <summary>
        /// 默认
        /// Default
        /// <summary>
        public static string Default;
        /// <summary>
        /// 等级
        /// Degree
        /// <summary>
        public static string Degree;
        /// <summary>
        /// 探测器
        /// Detector
        /// <summary>
        public static string Detector;
        /// <summary>
        /// 设备
        /// Device
        /// <summary>
        public static string Device;
        /// <summary>
        /// 设备型号
        /// Type
        /// <summary>
        public static string DeviceType;
        /// <summary>
        /// 差额
        /// Difference
        /// <summary>
        public static string Difference;
        /// <summary>
        /// 方向
        /// Direction
        /// <summary>
        public static string Direction;
        /// <summary>
        /// 显示元素
        /// Display Qualitative Elements
        /// <summary>
        public static string DisplayElement;
        /// <summary>
        /// 显示峰标识
        /// Display Peak Label
        /// <summary>
        public static string DisplayPeak;
        /// <summary>
        /// DLL类型
        /// DllType
        /// <summary>
        public static string DllType;
        /// <summary>
        /// 下移
        /// Down
        /// <summary>
        public static string Down;
        /// <summary>
        /// 设置影响元素
        /// Edit　Correlation　Element
        /// <summary>
        public static string EditCorrelationElement;
        /// <summary>
        /// 编辑数据
        /// Edit Data
        /// <summary>
        public static string EditData;
        /// <summary>
        /// 编辑元素
        /// Element
        /// <summary>
        public static string EditElement;
        /// <summary>
        /// 设置纯元素谱
        /// Edit Element Spectrum 
        /// <summary>
        public static string EditElementSpectrum;
        /// <summary>
        /// 校正元素谱库
        /// Spec Cal Lib
        /// <summary>
        public static string ElemAdjustSpecData;
        /// <summary>
        /// 元素
        /// Element
        /// <summary>
        public static string Element;
        /// <summary>
        /// 元素个数不能大于%s个
        /// element number can't bigger than %s 
        /// <summary>
        public static string ElementCountLimit;
        /// <summary>
        /// '%s'不是有效的元素名
        /// '%s' is Invalid 
        /// <summary>
        public static string ElementInvalid;
        /// <summary>
        /// 元素周期表
        /// Element Table
        /// <summary>
        public static string ElementTable;
        /// <summary>
        /// 元素谱库
        /// Spec Lib
        /// <summary>
        public static string ElemSpecData;
        /// <summary>
        /// 椭圆
        /// Ellipse
        /// <summary>
        public static string Ellipse;
        /// <summary>
        /// 出射角
        /// Emer.
        /// <summary>
        public static string EmergentAngle;
        /// <summary>
        /// 激活
        /// Enabled
        /// <summary>
        public static string Enabled;
        /// <summary>
        /// 结束道
        /// Channel End
        /// <summary>
        public static string EndChannel;
        /// <summary>
        /// 能量
        /// Energy
        /// <summary>
        public static string Energy;
        /// <summary>
        /// 提示
        /// Error
        /// <summary>
        public static string Error;
        /// <summary>
        /// 退出
        /// Exit
        /// <summary>
        public static string Exit;
        /// <summary>
        /// 该软件授权结束，如继续使用请与江苏天瑞仪器股份有限公司联系。
        /// Expriation date, please contact Skyray ASAP and extend your expiration date.
        /// <summary>
        public static string ExpriationDate;
        /// <summary>
        /// 滤光片
        /// Filter
        /// <summary>
        public static string Filter;
        /// <summary>
        /// 滤光片电机
        /// Motor with Filter
        /// <summary>
        public static string FilterMotor;
        /// <summary>
        /// 精调
        /// Amplification(F)
        /// <summary>
        public static string FineCode;
        /// <summary>
        /// 拟合元素
        /// Fit Element
        /// <summary>
        public static string FitElem;
        /// <summary>
        /// 焦班
        /// Focus
        /// <summary>
        public static string Focus;
        /// <summary>
        /// 焦点
        /// FocusSpot
        /// <summary>
        public static string FocusSpot;
        /// <summary>
        /// 表达式
        /// Formula
        /// <summary>
        public static string Formula;
        /// <summary>
        /// 表达式不正确！
        /// Formula is Invalid
        /// <summary>
        public static string FormulaInvalid;
        /// <summary>
        /// 分辨率
        /// FWHM
        /// <summary>
        public static string FWHM;
        /// <summary>
        /// 图表
        /// Graphic
        /// <summary>
        public static string Graphic;
        /// <summary>
        /// 历史记录
        /// History Record
        /// <summary>
        public static string HistoryRecord;
        /// <summary>
        /// 编号
        /// ID
        /// <summary>
        public static string ID;
        /// <summary>
        /// 入射角
        /// Inc.
        /// <summary>
        public static string IncidenceAngle;
        /// <summary>
        /// 信息
        /// Information
        /// <summary>
        public static string Information;
        /// <summary>
        /// 初始化通道
        /// Initalize Channel
        /// <summary>
        public static string InitalChann;
        /// <summary>
        /// 初始化管流
        /// Initalize Current
        /// <summary>
        public static string InitalCurrent;
        /// <summary>
        /// 初始化元素
        /// Initalize Element
        /// <summary>
        public static string InitalElem;
        /// <summary>
        /// 初始化误差道
        /// Initalize Error
        /// <summary>
        public static string InitalError;
        /// <summary>
        /// 初始化管压
        /// Initalize voltage
        /// <summary>
        public static string InitalVoltage;
        /// <summary>
        /// 初始化失败！
        /// Initial failed!
        /// <summary>
        public static string InitialFaile;
        /// <summary>
        /// 初始化
        /// Initialization
        /// <summary>
        public static string Initialization;
        /// <summary>
        /// 初始化成功！
        /// Initialization succeeded!
        /// <summary>
        public static string InitialSucceed;
        /// <summary>
        /// 输入数据错误
        /// 'Value is wrong.
        /// <summary>
        public static string InputDataErr;
        /// <summary>
        /// 值必须是小数
        /// ' Value must be float.
        /// <summary>
        public static string InputDataErrFloat;
        /// <summary>
        /// 值必须是整数
        /// ' Value must be integer.
        /// <summary>
        public static string InputDataErrInt;
        /// <summary>
        /// 插值
        /// Insert
        /// <summary>
        public static string Insert;
        /// <summary>
        /// 仪器运行，是否强制退出？
        /// Instrument is still Running, are you sure to quit?
        /// <summary>
        public static string InstrementIsRunning;
        /// <summary>
        /// 智能模式
        /// Intellectualized
        /// <summary>
        public static string Intellectualized;
        /// <summary>
        /// 强度
        /// Intensity
        /// <summary>
        public static string Intensity;
        /// <summary>
        /// 强度校正
        /// Intensity Revise
        /// <summary>
        public static string IntensityRevise;
        /// <summary>
        /// 强度计算方法
        /// Intensity Way
        /// <summary>
        public static string IntensityWay;
        /// <summary>
        /// 文件格式错误！
        /// Invalid File.
        /// <summary>
        public static string InvalidFile;
        /// <summary>
        /// 名称不正确！
        /// Name is Invalid.
        /// <summary>
        public static string InvalidName;
        /// <summary>
        /// 语言
        /// Language
        /// <summary>
        public static string Language;
        /// <summary>
        /// 离授权结束日还有{0}天
        /// There is {0} day period end of the authorization.
        /// <summary>
        public static string LeftDay;
        /// <summary>
        /// 该软件离授权结束日还有{0}天，为免影响您的正常工作，请尽快与江苏天瑞仪器股份有限公司联系。
        /// In order not to affect your narmal use of software,there is {0} day period end of the authorization , please contact Jiangsu Skyray Instrument Co.,Ltd. ASAP.
        /// <summary>
        public static string LeftDayHint;
        /// <summary>
        /// 离授权结束日还有{0}小时
        /// There is {0} hour period end of the authorization.
        /// <summary>
        public static string LeftHour;
        /// <summary>
        /// 该软件离授权结束日还有{0}小时，为免影响您的正常工作，请尽快与江苏天瑞仪器股份有限公司联系。
        /// In order not to affect your narmal use of software,there is {0} hour period end of the authorization , please contact Jiangsu Skyray Instrument Co.,Ltd. ASAP.
        /// <summary>
        public static string LeftHourHint;
        /// <summary>
        /// 剩余时间
        /// Time Left
        /// <summary>
        public static string LeftTime;
        /// <summary>
        /// 一次曲线
        /// Linear
        /// <summary>
        public static string Linear;
        /// <summary>
        /// 中文
        /// Local
        /// <summary>
        public static string Local;
        /// <summary>
        /// 登录
        /// Login
        /// <summary>
        public static string Login;
        /// <summary>
        /// 批号
        /// Lot No.
        /// <summary>
        public static string LotNo;
        /// <summary>
        /// 电磁铁
        /// ElectroMagnet
        /// <summary>
        public static string MagnetExist;
        /// <summary>
        /// 主元素
        /// Main Element
        /// <summary>
        public static string MainElem;
        /// <summary>
        /// 主元素含量或强度不能为零！
        /// The content or intensity of the main can not be zero
        /// <summary>
        public static string MainElementValueZero;
        /// <summary>
        /// 匹配
        /// Matching
        /// <summary>
        public static string Matching;
        /// <summary>
        /// 匹配时间
        /// Matching Time
        /// <summary>
        public static string MatchTime;
        /// <summary>
        /// 匹配值
        /// Value
        /// <summary>
        public static string MatchValue;
        /// <summary>
        /// 最大计数率
        /// Max Count Rate
        /// <summary>
        public static string MaxCountRate;
        /// <summary>
        /// 最大值
        /// Max.
        /// <summary>
        public static string MaxValue;
        /// <summary>
        /// 平均值
        /// Mean.
        /// <summary>
        public static string MeanValue;
        /// <summary>
        /// 测量日期
        /// Date
        /// <summary>
        public static string MeasureDate;
        /// <summary>
        /// 测量信息
        /// Measure information
        /// <summary>
        public static string MeasureInfo;
        /// <summary>
        /// 测量次数
        /// Number of Readings
        /// <summary>
        public static string MeasureNumber;
        /// <summary>
        /// 测量条件
        /// Measurement
        /// <summary>
        public static string MeasureParam;
        /// <summary>
        /// 测量时间
        /// Measure Time
        /// <summary>
        public static string MeasureTime;
        /// <summary>
        /// 最小计数率
        /// Min Count Rate
        /// <summary>
        public static string MinCountRate;
        /// <summary>
        /// 最小值
        /// Min.
        /// <summary>
        public static string MinValue;
        /// <summary>
        /// 模式
        /// Mode
        /// <summary>
        public static string Mode;
        /// <summary>
        /// 更改
        /// Modify
        /// <summary>
        public static string Modify;
        /// <summary>
        /// 带样品腔
        /// Motor with Chamber Exist
        /// <summary>
        public static string MotorChamberExists;
        /// <summary>
        /// 带准直器电机
        /// Motor with collimator exists
        /// <summary>
        public static string MotorColliExists;
        /// <summary>
        /// 带滤光片电机
        /// Motor with filter exists
        /// <summary>
        public static string MotorFilterExists;
        /// <summary>
        /// 样品腔调节
        /// Sample Chamber Adjust
        /// <summary>
        public static string MotorFineAdjust;
        /// <summary>
        /// 步长
        /// Step
        /// <summary>
        public static string MotorStep;
        /// <summary>
        /// 位置
        /// Tag
        /// <summary>
        public static string MotorTag;
        /// <summary>
        /// 位置个数
        /// Tag Count
        /// <summary>
        public static string MotorTagCount;
        /// <summary>
        /// 请先打开一条曲线！
        /// Must be open a Curve!
        /// <summary>
        public static string MustOpenCur;
        /// <summary>
        /// 名称
        /// Name
        /// <summary>
        public static string Name;
        /// <summary>
        /// 否
        /// No
        /// <summary>
        public static string No;
        /// <summary>
        /// 不要再问我！
        /// Do not ask me again!
        /// <summary>
        public static string NoAskMe;
        /// <summary>
        /// 不能打印，记录不存在！
        /// Can't print,the record has not exist!
        /// <summary>
        public static string NoRecords;
        /// <summary>
        /// 正常模式
        /// Normal
        /// <summary>
        public static string Normal;
        /// <summary>
        /// 不能删除当前用户！
        /// Can not delete the running user!
        /// <summary>
        public static string NotDeleteUser;
        /// <summary>
        /// 不能为空！
        /// can not be null!
        /// <summary>
        public static string Notnull;
        /// <summary>
        /// 测量条件不存在！
        /// Test condition is not exist.
        /// <summary>
        public static string NullCondition;
        /// <summary>
        /// 用户名与密码均不能为空！
        /// Username and Password is null!
        /// <summary>
        public static string NullUserPwd;
        /// <summary>
        /// 次数
        /// Number
        /// <summary>
        public static string Number;
        /// <summary>
        /// 关闭
        /// Off
        /// <summary>
        public static string Off;
        /// <summary>
        /// 打开
        /// On
        /// <summary>
        public static string On;
        /// <summary>
        /// 打开
        /// Open
        /// <summary>
        public static string Open;
        /// <summary>
        /// 操作员
        /// Operator
        /// <summary>
        public static string Operator;
        /// <summary>
        /// 因子
        /// Factor
        /// <summary>
        public static string OptFactor;
        /// <summary>
        /// 数据优化
        /// Optimization
        /// <summary>
        public static string Optimization;
        /// <summary>
        /// 范围
        /// Range
        /// <summary>
        public static string OptRange;
        /// <summary>
        /// 值
        /// Value
        /// <summary>
        public static string OptValue;
        /// <summary>
        /// 其他设置
        /// Other
        /// <summary>
        public static string OtherSet;
        /// <summary>
        /// 口令
        /// Password
        /// <summary>
        public static string Password;
        /// <summary>
        /// 密码错误！
        /// Password is error!
        /// <summary>
        public static string PasswordError;
        /// <summary>
        /// 峰通道
        /// Channel of Peak
        /// <summary>
        public static string PeakChann;
        /// <summary>
        /// 峰背比
        /// Peak//Base
        /// <summary>
        public static string PeakDivBase;
        /// <summary>
        /// 峰高
        /// Peak Height
        /// <summary>
        public static string PeakHeight;
        /// <summary>
        /// 峰右界
        /// Peak Right
        /// <summary>
        public static string PeakHigh;
        /// <summary>
        /// 峰
        /// Line
        /// <summary>
        public static string PeakLine;
        /// <summary>
        /// 峰左界
        /// Peak left
        /// <summary>
        public static string PeakLow;
        /// <summary>
        /// 峰偏移
        /// Peak Offset
        /// <summary>
        public static string PeakOffset;
        /// <summary>
        /// 峰值
        /// Peak Value
        /// <summary>
        public static string PeakValue;
        /// <summary>
        /// 峰宽
        /// Peak Width
        /// <summary>
        public static string PeakWidth;
        /// <summary>
        /// 接口
        /// Port
        /// <summary>
        public static string Port;
        /// <summary>
        /// 位置
        /// Position
        /// <summary>
        public static string Position;
        /// <summary>
        /// 打印
        /// Print
        /// <summary>
        public static string Print;
        /// <summary>
        /// 打印报告
        /// Print Report
        /// <summary>
        public static string PrintReport;
        /// <summary>
        /// 打印屏幕
        /// Print Screen
        /// <summary>
        public static string PrintScreen;
        /// <summary>
        /// 抽真空与吸电磁铁不能同时存在
        /// Pumb and Magnet can't existed together!
        /// <summary>
        public static string PumbMagnetBothExist;
        /// <summary>
        /// 带真空泵
        /// Pump  Exists
        /// <summary>
        public static string PumpExists;
        /// <summary>
        /// 请放入%s标样！
        /// Place %s sample in chamber
        /// <summary>
        public static string PutSample;
        /// <summary>
        /// 请放入标样！
        /// Place standard sample in chamber
        /// <summary>
        public static string PutStdSample;
        /// <summary>
        /// 定性分析
        /// Qualitative
        /// <summary>
        public static string Qualitative;
        /// <summary>
        /// 分析参数
        /// Qualitative Parameter
        /// <summary>
        public static string QualitativeParam;
        /// <summary>
        /// 真实值
        /// Real Value
        /// <summary>
        public static string RealValue;
        /// <summary>
        /// 长方形
        /// Rectangle
        /// <summary>
        public static string Rectangle;
        /// <summary>
        /// 对比谱
        /// Reference Spectrum
        /// <summary>
        public static string ReferenceSpec;
        /// <summary>
        /// 删除
        /// Remove
        /// <summary>
        public static string Remove;
        /// <summary>
        /// 删除峰标识
        /// Remove Peak Label
        /// <summary>
        public static string RemovePeak;
        /// <summary>
        /// 报告
        /// Report
        /// <summary>
        public static string Report;
        /// <summary>
        /// 报告设置
        /// Custom
        /// <summary>
        public static string ReportSetting;
        /// <summary>
        /// 提示：列元素被行元素影响
        /// Prompt:Element of column will be effected by the element of rwo
        /// <summary>
        public static string ReviseElemPrompt;
        /// <summary>
        /// 单位长度（mm）
        /// Rule Unit(mm)
        /// <summary>
        public static string RuleUnit;
        /// <summary>
        /// 样品图
        /// Sample Grahic
        /// <summary>
        public static string SampGrahic;
        /// <summary>
        /// 样品信息栏
        /// Sample information 
        /// <summary>
        public static string SampleInfo;
        /// <summary>
        /// 样品个数太少！
        /// Samples are not enough !
        /// <summary>
        public static string SampleIsNotEnough;
        /// <summary>
        /// 样品名称
        /// Sample Name
        /// <summary>
        public static string SampleName;
        /// <summary>
        /// 饱和度
        /// Saturation
        /// <summary>
        public static string Saturation;
        /// <summary>
        /// 保存
        /// Save
        /// <summary>
        public static string Save;
        /// <summary>
        /// 另存为
        /// Save As
        /// <summary>
        public static string SaveAs;
        /// <summary>
        /// 是否保存初始化参数？
        /// Save initalize parameter?
        /// <summary>
        public static string SaveInitalParam;
        /// <summary>
        /// 保存路径
        /// Save Path
        /// <summary>
        public static string SavePath;
        /// <summary>
        /// 方差
        /// S.D.
        /// <summary>
        public static string SDValue;
        /// <summary>
        /// 查询
        /// Search
        /// <summary>
        public static string Search;
        /// <summary>
        /// 按日期查询
        /// SearchByDate
        /// <summary>
        public static string SearchByDate;
        /// <summary>
        /// 按名称查询
        /// SearchByName
        /// <summary>
        public static string SearchByName;
        /// <summary>
        /// 全选
        /// SelectAll
        /// <summary>
        public static string SelectAll;
        /// <summary>
        /// 设置
        /// Setting
        /// <summary>
        public static string Setting;
        /// <summary>
        /// 形状
        /// Shape
        /// <summary>
        public static string Shape;
        /// <summary>
        /// 天瑞Logo
        /// Skyray Logo
        /// <summary>
        public static string SkyrayLogo;
        /// <summary>
        /// 天瑞报表打印专用模板(中文)
        /// Skyray print template(CN)
        /// <summary>
        public static string SkyrayPrintTemplateCN;
        /// <summary>
        /// 天瑞报表打印专用模板(英文)
        /// Skyray print template(EN)
        /// <summary>
        public static string SkyrayPrintTemplateEN;
        /// <summary>
        /// 斜率
        /// Slope
        /// <summary>
        public static string Slope;
        /// <summary>
        /// 源设置
        /// SourceSetting
        /// <summary>
        public static string SourceSetting;
        /// <summary>
        /// 谱图
        /// Spectrum Graphic
        /// <summary>
        public static string SpecGraphic;
        /// <summary>
        /// 谱信息
        /// Spectrum Info
        /// <summary>
        public static string SpecInfo;
        /// <summary>
        /// 工作谱
        /// Spectrum
        /// <summary>
        public static string Spectrum;
        /// <summary>
        /// 谱文件
        /// Spectrum
        /// <summary>
        public static string SpectrumFile;
        /// <summary>
        /// 焦斑高度（mm）
        /// Spot Height(mm)
        /// <summary>
        public static string SpotHeight;
        /// <summary>
        /// 焦斑形状
        /// Spot Sharp
        /// <summary>
        public static string SpotSharp;
        /// <summary>
        /// 焦斑宽度（mm）
        /// Spot Width(mm)
        /// <summary>
        public static string SpotWidth;
        /// <summary>
        /// 焦斑X点（mm）
        /// Spot X(mm)
        /// <summary>
        public static string SpotX;
        /// <summary>
        /// 焦斑Y点（mm）
        /// Spot Y(mm)
        /// <summary>
        public static string SpotY;
        /// <summary>
        /// 统计信息
        /// Statistic
        /// <summary>
        public static string Statistic;
        /// <summary>
        /// 停止
        /// Stop
        /// <summary>
        public static string Stop;
        /// <summary>
        /// 字段
        /// Field
        /// <summary>
        public static string StrDataField;
        /// <summary>
        /// 标签
        /// Label
        /// <summary>
        public static string StrDataLabel;
        /// <summary>
        /// 表格
        /// DataTable
        /// <summary>
        public static string StrDataTable;
        /// <summary>
        /// 线条
        /// Graph
        /// <summary>
        public static string StrGraph;
        /// <summary>
        /// 图表
        /// Image
        /// <summary>
        public static string StrImage;
        /// <summary>
        /// 横线
        /// Horizontal Line
        /// <summary>
        public static string StrLineX;
        /// <summary>
        /// 竖线
        /// Vertical Line
        /// <summary>
        public static string StrLineY;
        /// <summary>
        /// 描述
        /// Summary
        /// <summary>
        public static string Summary;
        /// <summary>
        /// 供应商
        /// Supplier
        /// <summary>
        public static string Supplier;
        /// <summary>
        /// 系统设置
        /// Setting
        /// <summary>
        public static string SystemSet;
        /// <summary>
        /// 靶材角度
        /// Tube Angle
        /// <summary>
        public static string TargetAngle;
        /// <summary>
        /// 靶材原子序号
        /// Tube Number
        /// <summary>
        public static string TargetNumber;
        /// <summary>
        /// 测试
        /// Measurement
        /// <summary>
        public static string Test;
        /// <summary>
        /// 测试完成！
        /// Test complete
        /// <summary>
        public static string TestComplete;
        /// <summary>
        /// 测试结果
        /// Result
        /// <summary>
        public static string TestResult;
        /// <summary>
        /// 理论值
        /// Theory Value
        /// <summary>
        public static string TheoryValue;
        /// <summary>
        /// 峰刺宽
        /// Thorn Width
        /// <summary>
        public static string ThornWidth;
        /// <summary>
        /// 归一含量
        /// Add all elements content
        /// <summary>
        public static string TotalContent;
        /// <summary>
        /// 类型
        /// Type
        /// <summary>
        public static string Type;
        /// <summary>
        /// 上移
        /// Up
        /// <summary>
        public static string Up;
        /// <summary>
        /// 用户名
        /// User
        /// <summary>
        public static string User;
        /// <summary>
        /// 用户管理
        /// User 
        /// <summary>
        public static string UserMag;
        /// <summary>
        /// 真空度
        /// VacuumDegree
        /// <summary>
        public static string VacuumDegree;
        /// <summary>
        /// 
        /// Pump
        /// <summary>
        public static string Vacuumize;
        /// <summary>
        /// 真空度抽真空
        /// PumpByDegree
        /// <summary>
        public static string VacuumizeByDegree;
        /// <summary>
        /// 时间抽真空
        /// PumpByTime
        /// <summary>
        public static string VacuumizeByTime;
        /// <summary>
        /// 抽真空时间
        /// VacuumTime
        /// <summary>
        public static string VacuumTime;
        /// <summary>
        /// 版本
        /// Version
        /// <summary>
        public static string Version;
        /// <summary>
        /// 视频格式
        /// Video Format
        /// <summary>
        public static string VideoFormat;
        /// <summary>
        /// 视频参数
        /// Video Parameter
        /// <summary>
        public static string VideoParam;
        /// <summary>
        /// 视频大小
        /// VideoSize
        /// <summary>
        public static string VideoSize;
        /// <summary>
        /// 视频源
        /// Video Source
        /// <summary>
        public static string VideoSource;
        /// <summary>
        /// 画面高度（mm）
        /// View Hight(mm)
        /// <summary>
        public static string ViewHight;
        /// <summary>
        /// 画面宽度（mm）
        /// view Width(mm)
        /// <summary>
        public static string ViewWidth;
        /// <summary>
        /// 管压
        /// Voltage
        /// <summary>
        public static string Voltage;
        /// <summary>
        /// 元素已经存在！
        /// Element Exists!
        /// <summary>
        public static string WarmElemExist;
        /// <summary>
        /// 校正曲线正在使用，不能删除！
        /// Calibration in use!
        /// <summary>
        public static string WarmRemCond;
        /// <summary>
        /// 确认要删除吗？
        /// Are you sure you want to Delete?
        /// <summary>
        public static string WarmRemove;
        /// <summary>
        /// 重量
        /// Weight
        /// <summary>
        public static string Weight;
        /// <summary>
        /// 窗口材料
        /// Window Material
        /// <summary>
        public static string WindowMaterial;
        /// <summary>
        /// 窗口厚度
        /// Window Thick
        /// <summary>
        public static string WindowThick;
        /// <summary>
        /// %s已经存在，要覆盖吗？
        /// %s exists，overwrite?
        /// <summary>
        public static string XExist;
        /// <summary>
        /// X光管
        /// X-Ray Tube
        /// <summary>
        public static string XRayTube;
        /// <summary>
        /// 大小还原
        /// Default
        /// <summary>
        public static string XYRevert;
        /// <summary>
        /// 水平放大
        /// X Zoom In
        /// <summary>
        public static string XZoomIn;
        /// <summary>
        /// 水平缩小
        /// X ZoomOut
        /// <summary>
        public static string XZoomOut;
        /// <summary>
        /// 是
        /// Yes
        /// <summary>
        public static string Yes;
        /// <summary>
        /// 垂直放大
        /// Y Zoom In
        /// <summary>
        public static string YZoomIn;
        /// <summary>
        /// 垂直缩小
        /// Y Zoom Out
        /// <summary>
        public static string YZoomOut;
        /// <summary>
        /// Z轴电机下移
        /// Z_Motor move down
        /// <summary>
        public static string ZDown;
        /// <summary>
        /// Z轴电机
        /// ZMotor
        /// <summary>
        public static string ZMotor;
        /// <summary>
        /// Z轴电机向上停止
        /// Z_Motor stop move up
        /// <summary>
        public static string ZStop;
        /// <summary>
        /// Z轴电机上移
        /// Z_Motor move up
        /// <summary>
        public static string ZUp;
        /// <summary>
        /// 找不到打印机
        /// Can't find the printer
        /// <summary>
        public static string NullPrinter;
        #endregion
        /// <summary>
        /// 检测语言文件是否全部不为空
        /// </summary>
        /// <returns>true:   全部不为空
        ///          false:  某些字段为空
        ///                              </returns>
        public static bool CheckXmlRes()
        {
            bool ret = true;
            Type type = typeof(Skyray.EDX.Common.Resource);
            foreach(System.Reflection.FieldInfo field in  type.GetFields())
            {
                if(String.IsNullOrEmpty(field.GetValue(type).ToString()))
                {
                    ret = false;
                    break;
                }
            }
             return ret;
         }
        /// <summary>
        /// 数据加载方法
        /// </summary>
        /// <param name="fileName"></param>
        public static void LoadXmlRes(string fileName)
        {
            ResXResourceSet res = new ResXResourceSet(fileName);
          About = res.GetString("About");
          Accept = res.GetString("Accept");
          Add = res.GetString("Add");
          AddNewSpectrum = res.GetString("AddNewSpectrum");
          AddPeak = res.GetString("AddPeak");
          AdjustCountRate = res.GetString("AdjustCountRate");
          Ajustcoefficient = res.GetString("Ajustcoefficient");
          AllInfo = res.GetString("AllInfo");
          Analysis = res.GetString("Analysis");
          AskClose = res.GetString("AskClose");
          BaseHigh = res.GetString("BaseHigh");
          BaseLow = res.GetString("BaseLow");
          BeginChannel = res.GetString("BeginChannel");
          Brightness = res.GetString("Brightness");
          Calculate = res.GetString("Calculate");
          Calibration = res.GetString("Calibration");
          Camera = res.GetString("Camera");
          Cancel = res.GetString("Cancel");
          Canves = res.GetString("Canves");
          ChamberMotor = res.GetString("ChamberMotor");
          Channel = res.GetString("Channel");
          ClearAll = res.GetString("ClearAll");
          Close = res.GetString("Close");
          CoarseCode = res.GetString("CoarseCode");
          Collimator = res.GetString("Collimator");
          ColliMotor = res.GetString("ColliMotor");
          Color = res.GetString("Color");
          ConditionID = res.GetString("ConditionID");
          Confirm = res.GetString("Confirm");
          Contect = res.GetString("Contect");
          Content = res.GetString("Content");
          ContentError = res.GetString("ContentError");
          ContentRevise = res.GetString("ContentRevise");
          Contrast = res.GetString("Contrast");
          CorrelationCoefficient = res.GetString("CorrelationCoefficient");
          CorrelationElement = res.GetString("CorrelationElement");
          Count = res.GetString("Count");
          CountHighWarn = res.GetString("CountHighWarn");
          CountRate = res.GetString("CountRate");
          Current = res.GetString("Current");
          CurrentlyInfo = res.GetString("CurrentlyInfo");
          CustomField = res.GetString("CustomField");
          Data = res.GetString("Data");
          DataWrong = res.GetString("DataWrong");
          Default = res.GetString("Default");
          Degree = res.GetString("Degree");
          Detector = res.GetString("Detector");
          Device = res.GetString("Device");
          DeviceType = res.GetString("DeviceType");
          Difference = res.GetString("Difference");
          Direction = res.GetString("Direction");
          DisplayElement = res.GetString("DisplayElement");
          DisplayPeak = res.GetString("DisplayPeak");
          DllType = res.GetString("DllType");
          Down = res.GetString("Down");
          EditCorrelationElement = res.GetString("EditCorrelationElement");
          EditData = res.GetString("EditData");
          EditElement = res.GetString("EditElement");
          EditElementSpectrum = res.GetString("EditElementSpectrum");
          ElemAdjustSpecData = res.GetString("ElemAdjustSpecData");
          Element = res.GetString("Element");
          ElementCountLimit = res.GetString("ElementCountLimit");
          ElementInvalid = res.GetString("ElementInvalid");
          ElementTable = res.GetString("ElementTable");
          ElemSpecData = res.GetString("ElemSpecData");
          Ellipse = res.GetString("Ellipse");
          EmergentAngle = res.GetString("EmergentAngle");
          Enabled = res.GetString("Enabled");
          EndChannel = res.GetString("EndChannel");
          Energy = res.GetString("Energy");
          Error = res.GetString("Error");
          Exit = res.GetString("Exit");
          ExpriationDate = res.GetString("ExpriationDate");
          Filter = res.GetString("Filter");
          FilterMotor = res.GetString("FilterMotor");
          FineCode = res.GetString("FineCode");
          FitElem = res.GetString("FitElem");
          Focus = res.GetString("Focus");
          FocusSpot = res.GetString("FocusSpot");
          Formula = res.GetString("Formula");
          FormulaInvalid = res.GetString("FormulaInvalid");
          FWHM = res.GetString("FWHM");
          Graphic = res.GetString("Graphic");
          HistoryRecord = res.GetString("HistoryRecord");
          ID = res.GetString("ID");
          IncidenceAngle = res.GetString("IncidenceAngle");
          Information = res.GetString("Information");
          InitalChann = res.GetString("InitalChann");
          InitalCurrent = res.GetString("InitalCurrent");
          InitalElem = res.GetString("InitalElem");
          InitalError = res.GetString("InitalError");
          InitalVoltage = res.GetString("InitalVoltage");
          InitialFaile = res.GetString("InitialFaile");
          Initialization = res.GetString("Initialization");
          InitialSucceed = res.GetString("InitialSucceed");
          InputDataErr = res.GetString("InputDataErr");
          InputDataErrFloat = res.GetString("InputDataErrFloat");
          InputDataErrInt = res.GetString("InputDataErrInt");
          Insert = res.GetString("Insert");
          InstrementIsRunning = res.GetString("InstrementIsRunning");
          Intellectualized = res.GetString("Intellectualized");
          Intensity = res.GetString("Intensity");
          IntensityRevise = res.GetString("IntensityRevise");
          IntensityWay = res.GetString("IntensityWay");
          InvalidFile = res.GetString("InvalidFile");
          InvalidName = res.GetString("InvalidName");
          Language = res.GetString("Language");
          LeftDay = res.GetString("LeftDay");
          LeftDayHint = res.GetString("LeftDayHint");
          LeftHour = res.GetString("LeftHour");
          LeftHourHint = res.GetString("LeftHourHint");
          LeftTime = res.GetString("LeftTime");
          Linear = res.GetString("Linear");
          Local = res.GetString("Local");
          Login = res.GetString("Login");
          LotNo = res.GetString("LotNo");
          MagnetExist = res.GetString("MagnetExist");
          MainElem = res.GetString("MainElem");
          MainElementValueZero = res.GetString("MainElementValueZero");
          Matching = res.GetString("Matching");
          MatchTime = res.GetString("MatchTime");
          MatchValue = res.GetString("MatchValue");
          MaxCountRate = res.GetString("MaxCountRate");
          MaxValue = res.GetString("MaxValue");
          MeanValue = res.GetString("MeanValue");
          MeasureDate = res.GetString("MeasureDate");
          MeasureInfo = res.GetString("MeasureInfo");
          MeasureNumber = res.GetString("MeasureNumber");
          MeasureParam = res.GetString("MeasureParam");
          MeasureTime = res.GetString("MeasureTime");
          MinCountRate = res.GetString("MinCountRate");
          MinValue = res.GetString("MinValue");
          Mode = res.GetString("Mode");
          Modify = res.GetString("Modify");
          MotorChamberExists = res.GetString("MotorChamberExists");
          MotorColliExists = res.GetString("MotorColliExists");
          MotorFilterExists = res.GetString("MotorFilterExists");
          MotorFineAdjust = res.GetString("MotorFineAdjust");
          MotorStep = res.GetString("MotorStep");
          MotorTag = res.GetString("MotorTag");
          MotorTagCount = res.GetString("MotorTagCount");
          MustOpenCur = res.GetString("MustOpenCur");
          Name = res.GetString("Name");
          No = res.GetString("No");
          NoAskMe = res.GetString("NoAskMe");
          NoRecords = res.GetString("NoRecords");
          Normal = res.GetString("Normal");
          NotDeleteUser = res.GetString("NotDeleteUser");
          Notnull = res.GetString("Notnull");
          NullCondition = res.GetString("NullCondition");
          NullUserPwd = res.GetString("NullUserPwd");
          Number = res.GetString("Number");
          Off = res.GetString("Off");
          On = res.GetString("On");
          Open = res.GetString("Open");
          Operator = res.GetString("Operator");
          OptFactor = res.GetString("OptFactor");
          Optimization = res.GetString("Optimization");
          OptRange = res.GetString("OptRange");
          OptValue = res.GetString("OptValue");
          OtherSet = res.GetString("OtherSet");
          Password = res.GetString("Password");
          PasswordError = res.GetString("PasswordError");
          PeakChann = res.GetString("PeakChann");
          PeakDivBase = res.GetString("PeakDivBase");
          PeakHeight = res.GetString("PeakHeight");
          PeakHigh = res.GetString("PeakHigh");
          PeakLine = res.GetString("PeakLine");
          PeakLow = res.GetString("PeakLow");
          PeakOffset = res.GetString("PeakOffset");
          PeakValue = res.GetString("PeakValue");
          PeakWidth = res.GetString("PeakWidth");
          Port = res.GetString("Port");
          Position = res.GetString("Position");
          Print = res.GetString("Print");
          PrintReport = res.GetString("PrintReport");
          PrintScreen = res.GetString("PrintScreen");
          PumbMagnetBothExist = res.GetString("PumbMagnetBothExist");
          PumpExists = res.GetString("PumpExists");
          PutSample = res.GetString("PutSample");
          PutStdSample = res.GetString("PutStdSample");
          Qualitative = res.GetString("Qualitative");
          QualitativeParam = res.GetString("QualitativeParam");
          RealValue = res.GetString("RealValue");
          Rectangle = res.GetString("Rectangle");
          ReferenceSpec = res.GetString("ReferenceSpec");
          Remove = res.GetString("Remove");
          RemovePeak = res.GetString("RemovePeak");
          Report = res.GetString("Report");
          ReportSetting = res.GetString("ReportSetting");
          ReviseElemPrompt = res.GetString("ReviseElemPrompt");
          RuleUnit = res.GetString("RuleUnit");
          SampGrahic = res.GetString("SampGrahic");
          SampleInfo = res.GetString("SampleInfo");
          SampleIsNotEnough = res.GetString("SampleIsNotEnough");
          SampleName = res.GetString("SampleName");
          Saturation = res.GetString("Saturation");
          Save = res.GetString("Save");
          SaveAs = res.GetString("SaveAs");
          SaveInitalParam = res.GetString("SaveInitalParam");
          SavePath = res.GetString("SavePath");
          SDValue = res.GetString("SDValue");
          Search = res.GetString("Search");
          SearchByDate = res.GetString("SearchByDate");
          SearchByName = res.GetString("SearchByName");
          SelectAll = res.GetString("SelectAll");
          Setting = res.GetString("Setting");
          Shape = res.GetString("Shape");
          SkyrayLogo = res.GetString("SkyrayLogo");
          SkyrayPrintTemplateCN = res.GetString("SkyrayPrintTemplateCN");
          SkyrayPrintTemplateEN = res.GetString("SkyrayPrintTemplateEN");
          Slope = res.GetString("Slope");
          SourceSetting = res.GetString("SourceSetting");
          SpecGraphic = res.GetString("SpecGraphic");
          SpecInfo = res.GetString("SpecInfo");
          Spectrum = res.GetString("Spectrum");
          SpectrumFile = res.GetString("SpectrumFile");
          SpotHeight = res.GetString("SpotHeight");
          SpotSharp = res.GetString("SpotSharp");
          SpotWidth = res.GetString("SpotWidth");
          SpotX = res.GetString("SpotX");
          SpotY = res.GetString("SpotY");
          Statistic = res.GetString("Statistic");
          Stop = res.GetString("Stop");
          StrDataField = res.GetString("StrDataField");
          StrDataLabel = res.GetString("StrDataLabel");
          StrDataTable = res.GetString("StrDataTable");
          StrGraph = res.GetString("StrGraph");
          StrImage = res.GetString("StrImage");
          StrLineX = res.GetString("StrLineX");
          StrLineY = res.GetString("StrLineY");
          Summary = res.GetString("Summary");
          Supplier = res.GetString("Supplier");
          SystemSet = res.GetString("SystemSet");
          TargetAngle = res.GetString("TargetAngle");
          TargetNumber = res.GetString("TargetNumber");
          Test = res.GetString("Test");
          TestComplete = res.GetString("TestComplete");
          TestResult = res.GetString("TestResult");
          TheoryValue = res.GetString("TheoryValue");
          ThornWidth = res.GetString("ThornWidth");
          TotalContent = res.GetString("TotalContent");
          Type = res.GetString("Type");
          Up = res.GetString("Up");
          User = res.GetString("User");
          UserMag = res.GetString("UserMag");
          VacuumDegree = res.GetString("VacuumDegree");
          Vacuumize = res.GetString("Vacuumize");
          VacuumizeByDegree = res.GetString("VacuumizeByDegree");
          VacuumizeByTime = res.GetString("VacuumizeByTime");
          VacuumTime = res.GetString("VacuumTime");
          Version = res.GetString("Version");
          VideoFormat = res.GetString("VideoFormat");
          VideoParam = res.GetString("VideoParam");
          VideoSize = res.GetString("VideoSize");
          VideoSource = res.GetString("VideoSource");
          ViewHight = res.GetString("ViewHight");
          ViewWidth = res.GetString("ViewWidth");
          Voltage = res.GetString("Voltage");
          WarmElemExist = res.GetString("WarmElemExist");
          WarmRemCond = res.GetString("WarmRemCond");
          WarmRemove = res.GetString("WarmRemove");
          Weight = res.GetString("Weight");
          WindowMaterial = res.GetString("WindowMaterial");
          WindowThick = res.GetString("WindowThick");
          XExist = res.GetString("XExist");
          XRayTube = res.GetString("XRayTube");
          XYRevert = res.GetString("XYRevert");
          XZoomIn = res.GetString("XZoomIn");
          XZoomOut = res.GetString("XZoomOut");
          Yes = res.GetString("Yes");
          YZoomIn = res.GetString("YZoomIn");
          YZoomOut = res.GetString("YZoomOut");
          ZDown = res.GetString("ZDown");
          ZMotor = res.GetString("ZMotor");
          ZStop = res.GetString("ZStop");
          ZUp = res.GetString("ZUp");
          NullPrinter = res.GetString("NullPrinter");
          res.Close();
        }
    }
}
