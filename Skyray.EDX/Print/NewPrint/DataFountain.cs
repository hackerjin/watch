using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.Print;
using System.Data;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Configuration;
using System.Windows.Forms;
using Lephone.Data.Definition;
using Skyray.EDX.Common.ReportHelper;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Define;
using System.IO;
using Skyray.Language;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.Print
{
    #region 设置数据源
    public partial class DataFountain
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// 工作曲线
        /// </summary>
        public WorkCurve workcCurrent { get; set; }

        /// <summary>
        /// 谱文件
        /// </summary>
        public SpecListEntity specList { get; set; }

        /// <summary>
        /// 测量结果
        /// </summary>
        public List<TestResult> LTestResult { get; set; }

        public List<ElemTestResult> ElemTestResultL { get; set; }

        public TestDeterminant TestDeterminant { get; set; }

        /// <summary>
        /// 统计信息
        /// </summary>
        public List<StatInfo> LStatInfo { get; set; }

        /// <summary>
        /// 样品数据信息
        /// </summary>
        public Byte[] SampleImage { get; set; }

        /// <summary>
        /// 谱图数据信息
        /// </summary>
        public Byte[] ByteSpecData { get; set; }

        public List<SpecEntity> SpecData { get; set; }

        /// <summary>
        /// 总判定，如果存在标准库，同时勾选了总判定，及其总值，则使用此功能
        /// </summary>
        public string[,] TotalDeterminant { get; set; }

        /// <summary>
        /// 当前标准库信息
        /// </summary>
        public List<StandardData> LStandardData { get; set; }


        /// <summary>
        /// 历史记录
        /// </summary>
        public HistoryRecord historyRecord { get; set; }

        /// <summary>
        /// 历史记录元素
        /// </summary>
        public List<HistoryElement> historyElementList { get; set; }

        /// <summary>
        /// 元素信息
        /// </summary>
        public List<Atom> atomList { get; set; }

        /// <summary>
        /// 保存连测记录相关信息，如果个数大于1则表明为连测，如果为一个记录，则为非连测情况
        /// </summary>
        public List<int> ContinuousList { get; set; }

        /// <summary>
        /// 当前软件版本
        /// </summary>
        public string curreEdition = "";//当前软件版本,


    }

    public class TestDeterminant
    {
        public string DeterminantName { get; set; }
        public string DeterminantResult { get; set; }
        public string DeterminantColor { get; set; }
        public TestDeterminant(string DeterminantName,string DeterminantResult, string DeterminantColor)
        {
            this.DeterminantName = DeterminantName;
            this.DeterminantResult = DeterminantResult;
            this.DeterminantColor = DeterminantColor;
        }
    }

    /// <summary>
    /// 测量结果
    /// </summary>
    public class TestResult
    {
        public string Order { get; set; }
        public string[,] ElementResult { get; set; }
        public TestResult(string Order, string[,] ElementResult)
        {
            this.Order = Order;
            this.ElementResult = ElementResult;
        }
        public TestResult(string Order)
        {
            this.Order = Order;
        }
    }

    /// <summary>
    /// 元素测量结果
    /// </summary>
    public class ElemTestResult
    {

        /// <summary>
        /// 元素名称
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// 元素中文名称
        /// </summary>
        public string ElementNameCN { get; set; }

        /// <summary>
        /// 元素英文名称
        /// </summary>
        public string ElementNameEN { get; set; }

        /// <summary>
        /// 含量
        /// </summary>
        public string ContextValue { get; set; }

        /// <summary>
        /// 含量单位
        /// </summary>
        public string ContextUnit { get; set; }

        /// <summary>
        /// 含量颜色
        /// </summary>
        public string ContextColor { get; set; }

        /// <summary>
        /// 强度
        /// </summary>
        public string IntensityVaule { get; set; }

        /// <summary>
        /// 厚度
        /// </summary>
        public string ThickValue { get; set; }

        /// <summary>
        /// 厚度单位
        /// </summary>
        public string ThickUnit { get; set; }

        /// <summary>
        /// 误差
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 判定名称
        /// </summary>
        public string DeterminantName { get; set; }

        /// <summary>
        /// 判定标准值
        /// </summary>
        public string DeterminantValue { get; set; }

        /// <summary>
        /// 判定结果
        /// </summary>
        public string DeterminantResult { get; set; }

        ///// <summary>
        ///// 样品名称
        ///// </summary>
        //public string SampleName { get; set; }

        ///// <summary>
        ///// 工作曲线
        ///// </summary>
        //public string WorkCurveName { get; set; }

        public ElemTestResult()
        { }

        /// <summary>
        /// Rohs使用的测量结果
        /// </summary>
        /// <param name="ElementName"></param>
        /// <param name="ContextValue"></param>
        /// <param name="ContextUnit"></param>
        /// <param name="ContextColor"></param>
        /// <param name="IntensityVaule"></param>
        /// <param name="Error"></param>
        /// <param name="DeterminantName"></param>
        /// <param name="DeterminantValue"></param>
        /// <param name="DeterminantResult"></param>
        public ElemTestResult(string ElementName, string ContextValue, string ContextUnit, string ContextColor
            , string IntensityVaule, string Error, string DeterminantName, string DeterminantValue, string DeterminantResult)
        { 
            this.ElementName=ElementName;
            this.ContextValue=ContextValue;
            this.ContextUnit=ContextUnit;
            this.ContextColor=ContextColor;
            this.IntensityVaule=IntensityVaule;
            this.Error=Error; 
            this.DeterminantName=DeterminantName;
            this.DeterminantValue=DeterminantValue;
            this.DeterminantResult = DeterminantResult;
        }

        /// <summary>
        /// FPThick使用的测量结果
        /// </summary>
        /// <param name="ElementName"></param>
        /// <param name="ContextValue"></param>
        /// <param name="ContextUnit"></param>
        /// <param name="IntensityVaule"></param>
        /// <param name="ThickValue"></param>
        /// <param name="ThickUnit"></param>
        public ElemTestResult(string ElementName, string ContextValue, string ContextUnit
            , string IntensityVaule, string ThickValue, string ThickUnit)
        {
            this.ElementName = ElementName;
            this.ContextValue = ContextValue;
            this.ContextUnit = ContextUnit;
            this.IntensityVaule = IntensityVaule;
            this.ThickValue = ThickValue;
            this.ThickUnit = ThickUnit;
        }

        public ElemTestResult(string ElementName, string ContextValue, string ContextUnit
            , string IntensityVaule)
        {
            this.ElementName = ElementName;
            this.ContextValue = ContextValue;
            this.ContextUnit = ContextUnit;
            this.IntensityVaule = IntensityVaule;
        }

        
    }

    /// <summary>
    /// 统计信息
    /// </summary>
    public class StatInfo
    {
        /// <summary>
        /// 元素
        /// </summary>
        public string StatColumns { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public string MaxValue { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public string MinValue { get; set; }
        /// <summary>
        /// 平均值
        /// </summary>
        public string MeanValue { get; set; }
        /// <summary>
        /// 标准偏差
        /// </summary>
        public string SDValue { get; set; }

        /// <summary>
        /// 相对标准偏差
        /// </summary>
        public string RSDValue { get; set; }

        public string Unit { get; set; }
        public StatInfo(string StatColumns, string MaxValue, string MinValue, string MeanValue, string SDValue, string RSDValue, string Unit)
        {
            this.StatColumns = StatColumns;
            this.MaxValue = MaxValue;
            this.MinValue = MinValue;
            this.MeanValue = MeanValue;
            this.SDValue = SDValue;
            this.RSDValue = RSDValue;
            this.Unit = Unit;
        }
    }



    #endregion

    #region 报表需要的对象

    /// <summary>
    /// 初始化参数
    /// </summary>
    public class PrintInitParam
    {
        /// <summary>
        /// 管压
        /// </summary>
        public int TubVoltage { get; set; }//<初始化电压

        /// <summary>
        /// 管流
        /// </summary>
        public int TubCurrent { get; set; }//初始化电流、

        /// <summary>
        /// 初始化元素
        /// </summary>
        public string ElemName { get; set; }//<用来初始化的元素

        /// <summary>
        /// 粗  调  码
        /// </summary>
        public float Gain { get; set; }    //<粗调码

        /// <summary>
        /// 精  调  码
        /// </summary>
        public float FineGain { get; set; } //<细调码

        /// <summary>
        /// 实际粗调码
        /// </summary>
        public float ActGain { get; set; }

        /// <summary>
        /// 实际细调码
        /// </summary>
        public float ActFineGain { get; set; }

        /// <summary>
        /// 初始化通道
        /// </summary>
        public int Channel { get; set; }   //<初始化通道

        /// <summary>
        /// 滤  光  片
        /// </summary>
        public int Filter { get; set; }    //<滤光片

        /// <summary>
        /// 准  直  器
        /// </summary>
        public int Collimator { get; set; }  //准直器

        /// <summary>
        /// 误  差  道
        /// </summary>
        public int ChannelError { get; set; }//<初始化误差道

        public PrintInitParam(int TubVoltage, int TubCurrent, string ElemName,
           float Gain, float FineGain, float ActGain, float ActFineGain
            , int Channel, int Filter, int Collimator, int ChannelError)
        {
            this.TubVoltage = TubVoltage;
            this.TubCurrent = TubCurrent;
            this.ElemName = ElemName;
            this.Gain = Gain;
            this.FineGain = FineGain;
            this.ActGain = ActGain;
            this.ActFineGain = ActFineGain;
            this.Channel = Channel;
            this.Filter = Filter;
            this.Collimator = Collimator;
            this.ChannelError = ChannelError;
        }
    }

    /// <summary>
    /// 测量条件
    /// </summary>
    public class PrintDeviceParameter
    {
        /// <summary>
        /// 所属测量条件名称
        /// </summary>
        public string DeviceParameterName { get; set; }
        /// <summary>
        /// 测量时间
        /// </summary>
        public int PrecTime { get; set; }

        /// <summary>
        /// 管流
        /// </summary>
        public int TubCurrent { get; set; }

        /// <summary>
        /// 管压
        /// </summary>
        public int TubVoltage { get; set; }

        /// <summary>
        /// 滤光片
        /// </summary>
        public int FilterIdx { get; set; }

        /// <summary>
        /// 滤光片材质
        /// </summary>
        public string FilterMaterial { get; set; }

        /// <summary>
        /// 准直器
        /// </summary>
        public int CollimatorIdx { get; set; }

        /// <summary>
        /// 是否抽真空
        /// </summary>
        public bool IsVacuum { get; set; }

        /// <summary>
        /// 抽真空时间
        /// </summary>
        public int VacuumTime { get; set; }

        /// <summary>
        /// 真空度抽真空
        /// </summary>
        public bool IsVacuumDegree { get; set; }

        /// <summary>
        /// 真空度
        /// </summary>
        public double VacuumDegree { get; set; }
        /// <summary>
        /// 是否调节计数
        /// </summary>
        public bool IsAdjustRate { get; set; }
        /// <summary>
        /// 最小計數率
        /// </summary>
        public double MinRate { get; set; }
        /// <summary>
        /// 最大計數率
        /// </summary>
        public double MaxRate { get; set; }

        public int BeginChann { get; set; }
        public int EndChann { get; set; }

        /// <summary>
        /// 是否干扰报警
        /// </summary>
        public bool IsDistrubAlert { get; set; }

        /// <summary>
        /// 是否判断峰飘
        /// </summary>
        public bool IsPeakFloat { get; set; }

        /// <summary>
        /// 峰飘左界
        /// </summary>
        public int PeakFloatLeft { get; set; }

        /// <summary>
        /// 峰飘右界
        /// </summary>
        public int PeakFloatRight { get; set; }

        /// <summary>
        /// 峰道址
        /// </summary>
        public int PeakFloatChannel { get; set; }

        /// <summary>
        /// 误差
        /// </summary>
        public int PeakFloatError { get; set; }
        public int PeakCheckTime { get; set; }

        


        public PrintDeviceParameter(
            string DeviceParameterName,
            int PrecTime,
            int TubCurrent,
            int TubVoltage,
            int FilterIdx,
            string FilterMaterial,
            int CollimatorIdx,
            bool IsVacuum,
            int VacuumTime,
            bool IsVacuumDegree,
            double VacuumDegree,
            bool IsAdjustRate,
            double MinRate,
            double MaxRate,
            int BeginChann,
            int EndChann,
            bool IsDistrubAlert,
            bool IsPeakFloat,
            int PeakFloatLeft,
            int PeakFloatRight,
            int PeakFloatChannel,
            int PeakFloatError,
            int PeakCheckTime
        )
        {
            this.DeviceParameterName = DeviceParameterName;
            this.PrecTime = PrecTime;
            this.TubCurrent = TubCurrent;
            this.TubVoltage = TubVoltage;
            this.FilterIdx = FilterIdx;
            this.FilterMaterial = FilterMaterial;
            this.CollimatorIdx = CollimatorIdx;
            this.IsVacuum = IsVacuum;
            this.VacuumTime = VacuumTime;
            this.IsVacuumDegree = IsVacuumDegree;
            this.VacuumDegree = VacuumDegree;
            this.IsAdjustRate = IsAdjustRate;
            this.MinRate = MinRate;
            this.MaxRate = MaxRate;
            this.BeginChann = BeginChann;
            this.EndChann = EndChann;
            this.IsDistrubAlert = IsDistrubAlert;
            this.IsPeakFloat = IsPeakFloat;
            this.PeakFloatLeft = PeakFloatLeft;
            this.PeakFloatRight = PeakFloatRight;
            this.PeakFloatChannel = PeakFloatChannel;
            this.PeakFloatError = PeakFloatError;
            this.PeakCheckTime = PeakCheckTime;
        }
    }


    /// <summary>
    /// 工作曲线
    /// </summary>
    public class PrintWorkCurve
    {
        /// <summary>
        /// 曲线名称
        /// </summary>
        public string WorkCurveName { get; set; }

        /// <summary>
        /// 测量名称
        /// </summary>
        public string ConditionName { get; set; }

        ///// <summary>
        ///// 扫谱时间
        ///// </summary>
        //private double SpecTime { get; set; }
        ///// <summary>
        ///// 所用时间
        ///// </summary>
        //private int UsedTime { get; set; }
        ///// <summary>
        ///// 管压
        ///// </summary>
        //private int TubVoltage { get; set; }
        ///// <summary>
        ///// 管流
        ///// </summary>
        //private int TubCurrent { get; set; }


        //public PrintWorkCurve(string WorkCurveName, string ConditionName, double SpecTime, int UsedTime, int TubVoltage, int TubCurrent) {
        //    this.WorkCurveName = WorkCurveName;
        //    this.ConditionName = ConditionName;
        //    this.SpecTime = SpecTime;
        //    this.UsedTime = UsedTime;
        //    this.TubVoltage = TubVoltage;
        //    this.TubCurrent = TubCurrent;
        //}

        public PrintWorkCurve(string WorkCurveName, string ConditionName)
        {
            this.WorkCurveName = WorkCurveName;
            this.ConditionName = ConditionName;
        }
    }

    /// <summary>
    /// 样品信息
    /// </summary>
    public class PrintSample
    {
        /// <summary>
        /// 样品名称
        /// </summary>
        public string SampleName { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// 形狀
        /// </summary>
        public string Shape { get; set; }

        /// <summary>
        /// 操作員
        /// </summary>
        public string Operater { get; set; }

        /// <summary>
        /// 扫谱日期
        /// </summary>
        public DateTime? SpecDate { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        private string SpecSummary { get; set; }

        public PrintSample(string SampleName, string Supplier, double? Weight,
            string Shape, string Operater, DateTime? SpecDate, string SpecSummary)
        {
            this.SampleName = SampleName;
            this.Supplier = Supplier;
            this.Weight = Weight;
            this.Operater = Operater;
            this.SpecDate = SpecDate;
            this.SpecSummary = SpecSummary;
        }
    }


    /// <summary>
    /// 样品图信息
    /// </summary>
    public class PrintSampleImage
    {
        private Bitmap SampleImage { get; set; }
        public PrintSampleImage(Bitmap SampleImage)
        { this.SampleImage = SampleImage; }
    }


    /// <summary>
    /// 谱图信息
    /// </summary>
    public class PrintSpecImage
    {
        public Bitmap SpecImage { get; set; }

        public Dictionary<string, Bitmap> ElementSpecImage { get; set; }
        public PrintSpecImage(Bitmap SpecImage, Dictionary<string, Bitmap> ElementSpecImage)
        {
            this.SpecImage = SpecImage;
            this.ElementSpecImage = ElementSpecImage;
        }
    }

    public class BaseInfoCls
    {
        /// <summary>
        /// 设备名
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 样品名称
        /// </summary>
        public string SimpleName { get; set; }
        /// <summary>
        /// 测试时间
        /// </summary>
        public string TestTime { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// 管压
        /// </summary>
        public string Voltage { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 管流
        /// </summary>
        public string Current { get; set; }
        /// <summary>
        /// 测量日期
        /// </summary>
        public string TestDate { get; set; }
        /// <summary>
        /// 工作曲线
        /// </summary>
        public string WorkCurve { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string LotNo { get; set; }
        /// <summary>
        /// 仪器型号
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 待检单号
        /// </summary>
        public string PendNo { get; set; }
        /// <summary>
        /// 送测单位
        /// </summary>
        public string SubmittedUnit { get; set; }
        /// <summary>
        /// 编号 
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 准直器
        /// </summary>
        public string CollimatorIdx { get; set; }
        /// <summary>
        /// 滤光片
        /// </summary>
        public string FilterIdx { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 测试点
        /// </summary>
        public string TestPoints { get; set; }

        /// <summary>
        /// 生产商
        /// </summary>
        public string Manufacturers { get; set; }

        /// <summary>
        /// 零部件商
        /// </summary>
        public string Partssupplier { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// 测试环境
        /// </summary>
        public string TestEnvironment { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public string ProcessingTime { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 原料名称
        /// </summary>
        public string NameOfRawMaterials { get; set; }

        /// <summary>
        /// 机种名/委托工厂
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 进料日期
        /// </summary>
        public string FeedDate { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string FanProducts { get; set; }

        /// <summary>
        /// 检验编号
        /// </summary>
        public string CheckCode { get; set; }

        /// <summary>
        /// 原始编号
        /// </summary>
        public string RawCode { get; set; }


        /// <summary>
        /// 印记
        /// </summary>
        public string Sigil { get; set; }
    }

    /// <summary>
    /// 总判定
    /// </summary>
    public class TotalDeterminant
    {
        public string TotalDeterminant1 { get; set; }
    }


    #endregion

    #region 获取数据


    /// <summary>
    /// 获取数据源
    /// </summary>
    public class GetDataFountain
    {
        private bool isShowUnit = true;//报表元素是否显示单位
        private bool isShowCStandard = true;//报表元素是否显示
        private string [] OtherMust=null;//其他必须信息
        private string [] OtherNoMust =null;//其他不必信息，但是存在字段，没有值
        private string Arrangement = "";//Arrangement:表明数据源的元素排列方式（0：为横向排列，1：为纵向排列）
        private bool isOriginal = true;//是否为元素表还是复合表

        private string CurrentLanguage = "";
        private string CurrentTemplate = "";


        private List<DataFountain> dataFountainList;
        private DataFountain dataFountain = null;
        Skyray.Language.LanguageModel languageModel;
        public GetDataFountain(List<DataFountain> dataFountainList)
        {
            if (languageModel == null)
            {
                languageModel = new Skyray.Language.LanguageModel();
            }
            this.dataFountainList = dataFountainList;
            dataFountain = dataFountainList[0];
        }

        public List<TreeNodeInfo> GetSource()
        {
            //初始化参数
            Condition condition = dataFountain.workcCurrent.Condition;

            PrintInitParam printInitParam = new PrintInitParam(dataFountain.specList.Specs[0].DeviceParameter.TubVoltage
               , dataFountain.specList.Specs[0].DeviceParameter.TubCurrent, condition.InitParam.ElemName
               , condition.InitParam.Gain, condition.InitParam.FineGain
               , condition.InitParam.ActGain, condition.InitParam.ActFineGain
               , condition.InitParam.Channel, condition.InitParam.Filter
               , condition.InitParam.Collimator, condition.InitParam.ChannelError);
            //工作曲线
            WorkCurve workcCurrent = dataFountain.workcCurrent;
            PrintWorkCurve printWorkCurve = new PrintWorkCurve(workcCurrent.Name, workcCurrent.ConditionName);

            //样品信息
            SpecListEntity specList = dataFountain.specList;
            PrintSample printSample = new PrintSample(specList.SampleName, specList.Supplier
                , specList.Weight, specList.Shape, specList.Operater, specList.SpecDate, specList.SpecSummary);

            //样品图数据信息
            PrintSampleImage printSampleImage = new PrintSampleImage(GetSampleImage(dataFountain.SampleImage));

            //谱图数据信息
            #region
            Bitmap SpecImage = null;
            List<DataFountain> dataFountainConList = null;
            if (dataFountainList.Exists(delegate(DataFountain v) { return v.ContinuousList.Count > 1; }))//连测记录处理
            {
                DataFountain dFountain = dataFountainList.Find(delegate(DataFountain v) { return v.ContinuousList.Count > 1; });
                dataFountainConList = new List<DataFountain>();
                foreach (int cOrder in dFountain.ContinuousList)
                    dataFountainConList.Add(dataFountainList.Find(delegate(DataFountain v) { return v.order == cOrder; }));

                List<int> FirstContIntr = new List<int>();
                ElementList elementList = ElementList.New;

                foreach (DataFountain df in dataFountainConList)
                {

                    ElementList tempElement = ElementList.New;
                    foreach (CurveElement elementCurve in df.workcCurrent.ElementList.Items)
                    {
                        tempElement.Items.Add(elementCurve);
                    }
                    FirstContIntr.Add(tempElement.Items.Count);


                    foreach (CurveElement elementCurve in tempElement.Items)
                    {
                        elementList.Items.Add(elementCurve);
                    }
                }


                SpecImage = GetSpecImage(dataFountainConList[0].SpecData, dataFountainConList[1].SpecData, 2, FirstContIntr, elementList);
            }
            else
            {
                if (dataFountain.SpecData != null && dataFountain.ByteSpecData == null)
                {
                    SpecImage = GetSpecImage(dataFountain.SpecData, null, 1, null, null);
                }
                else SpecImage = GetSampleImage(dataFountain.ByteSpecData);
            }
            #endregion



            //获取谱图元素数据信息
            Dictionary<string, Bitmap> ElementSpecImage = new Dictionary<string, Bitmap>();
            PrintSpecImage printSpecImage = new PrintSpecImage(SpecImage, ElementSpecImage);

            //测量结果

            #region

            //获取元素列之和
            Dictionary<string, int> tempElementD = new Dictionary<string, int>();
            foreach (DataFountain df in dataFountainList)
                foreach (HistoryElement historyElement in df.historyElementList)
                    if (!tempElementD.Keys.Contains(historyElement.elementName)) tempElementD.Add(historyElement.elementName, tempElementD.Count);

            DataTable dt = null;
            string TotalDeterminant = "";//总判定结果
            GetTestResult(ref dt, dataFountain, tempElementD, ref TotalDeterminant);



            #endregion

            //统计信息
            #region
            List<StatInfo> statInfoList = new List<StatInfo>();
            GetStatInfo(tempElementD, dt, ref statInfoList,false);
            if (dataFountain.curreEdition == "FPThick") GetStatInfo(tempElementD, dt, ref statInfoList, true);
            #endregion

            //总判定
            #region

            #endregion

            //获取当前模板参数
            GetTemplateAndAttribute(dt);

            Dictionary<string, string> DCompanyOtherInfo = new Dictionary<string, string>();
            GetCompanyOtherInfo(ref DCompanyOtherInfo);

            

            //测量条件信息
            List<PrintDeviceParameter> LprintDeviceParameter = new List<PrintDeviceParameter>();
            foreach (DeviceParameter deviceParameter in condition.DeviceParamList)
            {
                PrintDeviceParameter printDeviceParameter =
                    new PrintDeviceParameter(deviceParameter.Name, deviceParameter.PrecTime, deviceParameter.TubCurrent
                        , deviceParameter.TubVoltage, deviceParameter.FilterIdx, (condition.Device.Filter.Count > 0 && deviceParameter.FilterIdx>0) ? condition.Device.Filter[deviceParameter.FilterIdx - 1].Caption : "", deviceParameter.CollimatorIdx
                        , deviceParameter.IsVacuum, deviceParameter.VacuumTime, deviceParameter.IsVacuumDegree
                        , deviceParameter.VacuumDegree, deviceParameter.IsAdjustRate, deviceParameter.MinRate
                        , deviceParameter.MaxRate, deviceParameter.BeginChann, deviceParameter.EndChann
                        , deviceParameter.IsDistrubAlert, deviceParameter.IsPeakFloat, deviceParameter.PeakFloatLeft
                        , deviceParameter.PeakFloatRight, deviceParameter.PeakFloatChannel, deviceParameter.PeakFloatError
                        , deviceParameter.PeakCheckTime);
                LprintDeviceParameter.Add(printDeviceParameter);
            }

            


            BaseInfoCls baseInfocls = new BaseInfoCls();
            baseInfocls.DeviceName = WorkCurveHelper.DeviceCurrent.Name;
            baseInfocls.SimpleName = specList.SampleName;
            baseInfocls.TestTime = LprintDeviceParameter[0].PrecTime.ToString()+"(s)";
            baseInfocls.Supplier = printSample.Supplier;
            baseInfocls.Voltage = printInitParam.TubVoltage.ToString()+"(KV)";
            baseInfocls.Operator = printSample.Operater;
            baseInfocls.Current = printInitParam.TubCurrent.ToString() + "(μA)";
            baseInfocls.TestDate = printSample.SpecDate.ToString();
            baseInfocls.WorkCurve = workcCurrent.Name;

            foreach (string strKey in DCompanyOtherInfo.Keys)
            {
                switch (strKey.Split('-')[1])
                {
                    case "Operator": baseInfocls.Operator = DCompanyOtherInfo[strKey]; break;
                    case "Batch": baseInfocls.Batch = DCompanyOtherInfo[strKey]; break;
                    case "Name": baseInfocls.Name = DCompanyOtherInfo[strKey]; break;
                    case "NameOfRawMaterials": baseInfocls.NameOfRawMaterials = DCompanyOtherInfo[strKey]; break;
                    case "ModelName": baseInfocls.ModelName = DCompanyOtherInfo[strKey]; break;
                    case "FeedDate": baseInfocls.FeedDate = DCompanyOtherInfo[strKey]; break;
                    case "Material": baseInfocls.Material = DCompanyOtherInfo[strKey]; break;
                    case "TestEnvironment": baseInfocls.TestEnvironment = DCompanyOtherInfo[strKey]; break;
                    case "ProcessingTime": baseInfocls.ProcessingTime = DCompanyOtherInfo[strKey]; break;
                    case "LotNo": baseInfocls.LotNo = DCompanyOtherInfo[strKey]; break;
                    case "PendNo": baseInfocls.PendNo = DCompanyOtherInfo[strKey]; break;
                    case "SubmittedUnit": baseInfocls.SubmittedUnit = DCompanyOtherInfo[strKey]; break;
                    case "FanProducts": baseInfocls.FanProducts = DCompanyOtherInfo[strKey]; break;
                    case "CheckCode": baseInfocls.CheckCode = DCompanyOtherInfo[strKey]; break;
                    case "RawCode":baseInfocls.RawCode =DCompanyOtherInfo[strKey]; break;
                    case "Sigil": baseInfocls.Sigil = DCompanyOtherInfo[strKey]; break;


                       
                }
            }

            //string strLotNo = "";
            //DCompanyOtherInfo.TryGetValue("批号", out strLotNo);
            //baseInfocls.LotNo = (strLotNo == null) ? "" : strLotNo;
            //baseInfocls.Mode = "";

            //string strPendNo = "";
            //DCompanyOtherInfo.TryGetValue("待检单号", out strPendNo);
            //baseInfocls.PendNo = (strPendNo == null) ? "" : strPendNo;

            //string strSubmittedUnit = "";
            //DCompanyOtherInfo.TryGetValue("送检单位", out strSubmittedUnit);
            //baseInfocls.SubmittedUnit = (strSubmittedUnit == null) ? "" : strSubmittedUnit;
            //baseInfocls.Number = "";

            //string strBatch = "";
            //DCompanyOtherInfo.TryGetValue("批次", out strBatch);
            //baseInfocls.Batch = (strBatch == null) ? "" : strBatch;
            //string strTestPoints = "";
            //DCompanyOtherInfo.TryGetValue("测试点", out strTestPoints);
            //baseInfocls.TestPoints = (strTestPoints == null) ? "" : strTestPoints;
            //string strManufacturers = "";
            //DCompanyOtherInfo.TryGetValue("生产商", out strManufacturers);
            //baseInfocls.Manufacturers = (strManufacturers == null) ? "" : strManufacturers;
            //string strPartssupplier = "";
            //DCompanyOtherInfo.TryGetValue("零部件商", out strPartssupplier);
            //baseInfocls.Partssupplier = (strPartssupplier == null) ? "" : strPartssupplier;
            //string strMaterial = "";
            //DCompanyOtherInfo.TryGetValue("材料", out strMaterial);
            //baseInfocls.Material = (strMaterial == null) ? "" : strMaterial;


            //string strTestEnvironment = "";//测试环境
            //DCompanyOtherInfo.TryGetValue("测试环境", out strTestEnvironment);
            //baseInfocls.TestEnvironment = (strTestEnvironment == null) ? "" : strTestEnvironment;
            //string strProcessingTime = "";//处理时间
            //DCompanyOtherInfo.TryGetValue("处理时间", out strProcessingTime);
            //baseInfocls.ProcessingTime = (strProcessingTime == null) ? "" : strProcessingTime;


            if (LprintDeviceParameter.Count > 0)
            {
                baseInfocls.CollimatorIdx = LprintDeviceParameter[0].CollimatorIdx.ToString();
                baseInfocls.FilterIdx = LprintDeviceParameter[0].FilterIdx.ToString() + "(" + LprintDeviceParameter[0].FilterMaterial + ")";
            }

            //画各个元素图
            #region
            if (dataFountain.curreEdition != "XRF")
            {
                if (dataFountainList.Exists(delegate(DataFountain v) { return v.ContinuousList.Count > 1; }) && dataFountainConList != null)//连测记录处理
                {
                    List<int> FirstContIntr = new List<int>();
                    ElementList elementList = ElementList.New;

                    foreach (DataFountain df in dataFountainConList)
                    {

                        ElementList tempElement = ElementList.New;
                        //foreach (CurveElement elementCurve in df.workcCurrent.ElementList.Items)
                        foreach (HistoryElement HElement in df.historyElementList)
                        {
                            CurveElement elementCurve = df.workcCurrent.ElementList.Items.ToList().Find(delegate(CurveElement v) { return v.Caption == HElement.elementName; });
                            if (elementCurve != null) tempElement.Items.Add(elementCurve);
                        }
                        FirstContIntr.Add(tempElement.Items.Count);


                        foreach (CurveElement elementCurve in tempElement.Items)
                        {
                            elementList.Items.Add(elementCurve);
                        }
                    }
                    ElementImage(dataFountainConList[0].SpecData[0], dataFountainConList[1].SpecData[0], 2, FirstContIntr, elementList, "全元素", dataFountainConList, ref ElementSpecImage);
                }
                else
                {
                    ElementList elementList = ElementList.New;

                    ElementList tempElement = ElementList.New;
                    foreach (HistoryElement HElement in dataFountain.historyElementList)
                    {
                        CurveElement elementCurve = dataFountain.workcCurrent.ElementList.Items.ToList().Find(delegate(CurveElement v) { return v.Caption == HElement.elementName; });
                        if (elementCurve != null) tempElement.Items.Add(elementCurve);
                    }


                    foreach (CurveElement elementCurve in tempElement.Items)
                    {
                        elementList.Items.Add(elementCurve);
                    }

                    Bitmap bmp = new Bitmap(120, 60);
                    Graphics g = Graphics.FromImage(bmp);
                    //int width = (int)Math.Round(Convert.ToInt32(bmp.Width) * g.DpiX / 72.0);
                    //int height = (int)Math.Round(Convert.ToInt32(bmp.Height) * g.DpiY / 72.0);

                    int width = 400;
                    int height = 200;

                    bmp = new Bitmap(width, height);

                    Report report = new Report();
                
                    report.isShowND = WorkCurveHelper.isShowND;
                    report.Spec = dataFountain.specList.Specs[0];
                    //report.operateMember = FrmLogon.userName;
                    report.Elements = elementList;
                    report.WorkCurveName = dataFountain.workcCurrent.Name;
                    //report.FirstContIntr.Add(elementList.Items.Count);
                    report.InterestElemCount = dataFountain.historyElementList.Count;
                    report.DrawInterstringElems(ref bmp);
                    //DrawInterstringElems(ref bmp, (dataFountain.SpecData == null) ? dataFountain.specList.Specs[0] : dataFountain.SpecData, curveElement);
                    ElementSpecImage.Add("全元素", bmp);


                }
            }

            #endregion

            if (dt.Rows.Count > 1 && Arrangement == "1") return null;

            return GetZhongDa(dataFountainList, printInitParam, printWorkCurve, printSample, SpecImage, printSpecImage, dt, LprintDeviceParameter, baseInfocls, statInfoList, TotalDeterminant, DCompanyOtherInfo);
        }

        

        #region 获取模板及其模板参数

        private void GetTemplateAndAttribute(DataTable dt_TestResult)
        {

            CurrentLanguage = WorkCurveHelper.LanguageShortName;
            try
            {
                XElement xEAppXml = XElement.Load(Application.StartupPath + "\\AppParams.xml");
                string defaultTemplete = xEAppXml.Element("Report").Element("ReportName").Value;
                CurrentTemplate = defaultTemplete;
                


                XmlDocument doc = new XmlDocument();
                string path = Application.StartupPath + "\\PrintTemplate\\template.xml";
                if (File.Exists(path))
                {
                    doc.Load(path);
                    XmlNodeList rootNodeList = doc.SelectNodes("application/ReportTemplate/" + dataFountain.curreEdition + "/" + CurrentLanguage + "/TemplateName");
                    if (rootNodeList == null || rootNodeList.Count == 0) return;
                    foreach (XmlNode xmlnode in rootNodeList)
                    {
                        if (xmlnode.InnerText == defaultTemplete && dt_TestResult.Rows.Count == 1)
                        {
                            for (int i = 0; i < xmlnode.Attributes.Count; i++)
                            {
                                if (xmlnode.Attributes[i].Name == "ShowUnit") isShowUnit = xmlnode.Attributes[i].Value == "true" ? true : false;
                                if (xmlnode.Attributes[i].Name == "ShowStandard") isShowCStandard = xmlnode.Attributes[i].Value == "true" ? true : false;
                                if (xmlnode.Attributes[i].Name == "IsOriginal") isOriginal = xmlnode.Attributes[i].Value == "true" ? true : false;
                                //if (xmlnode.Attributes[i].Name == "IsDeterminant") isDeterminant = xmlnode.Attributes[i].Value == "true" ? true : false;
                                if (xmlnode.Attributes[i].Name == "OtherMust") OtherMust = xmlnode.Attributes[i].Value.ToString().Split(',');
                                if (xmlnode.Attributes[i].Name == "OtherNoMust") OtherNoMust = xmlnode.Attributes[i].Value.ToString().Split(',');
                                if (xmlnode.Attributes[i].Name == "Arrangement") Arrangement = xmlnode.Attributes[i].Value.ToString();
                            }
                        }
                        else if (xmlnode.InnerText == defaultTemplete && dt_TestResult.Rows.Count > 1)
                        {
                            string mulitdefaultTemplete = xmlnode.Attributes["Mulit"].Value;
                            if (defaultTemplete == mulitdefaultTemplete || mulitdefaultTemplete=="")
                            {
                                for (int i = 0; i < xmlnode.Attributes.Count; i++)
                                {
                                    if (xmlnode.Attributes[i].Name == "ShowUnit") isShowUnit = xmlnode.Attributes[i].Value == "true" ? true : false;
                                    if (xmlnode.Attributes[i].Name == "ShowStandard") isShowCStandard = xmlnode.Attributes[i].Value == "true" ? true : false;
                                    if (xmlnode.Attributes[i].Name == "IsOriginal") isOriginal = xmlnode.Attributes[i].Value == "true" ? true : false;
                                    //if (xmlnode.Attributes[i].Name == "IsDeterminant") isDeterminant = xmlnode.Attributes[i].Value == "true" ? true : false;
                                    if (xmlnode.Attributes[i].Name == "OtherMust") OtherMust = xmlnode.Attributes[i].Value.ToString().Split(',');
                                    if (xmlnode.Attributes[i].Name == "OtherNoMust") OtherNoMust = xmlnode.Attributes[i].Value.ToString().Split(',');
                                    if (xmlnode.Attributes[i].Name == "Arrangement") Arrangement = xmlnode.Attributes[i].Value.ToString();
                                }
                            }
                            else if (defaultTemplete != mulitdefaultTemplete && mulitdefaultTemplete != "")
                            {
                                XmlNodeList rootNodeListmulit = doc.SelectNodes("application/ReportTemplate/" + dataFountain.curreEdition + "/" + CurrentLanguage + "/TemplateName");
                                if (rootNodeListmulit == null || rootNodeListmulit.Count == 0) return;

                                CurrentTemplate = mulitdefaultTemplete;
                                foreach (XmlNode xmlnodemulit in rootNodeListmulit)
                                    {
                                        if (xmlnodemulit.InnerText == mulitdefaultTemplete)
                                        {
                                            for (int i = 0; i < xmlnodemulit.Attributes.Count; i++)
                                            {
                                                if (xmlnodemulit.Attributes[i].Name == "ShowUnit") isShowUnit = xmlnodemulit.Attributes[i].Value == "true" ? true : false;
                                                if (xmlnodemulit.Attributes[i].Name == "ShowStandard") isShowCStandard = xmlnodemulit.Attributes[i].Value == "true" ? true : false;
                                                if (xmlnodemulit.Attributes[i].Name == "IsOriginal") isOriginal = xmlnodemulit.Attributes[i].Value == "true" ? true : false;
                                                //if (xmlnodemulit.Attributes[i].Name == "IsDeterminant") isDeterminant = xmlnodemulit.Attributes[i].Value == "true" ? true : false;
                                                if (xmlnodemulit.Attributes[i].Name == "OtherMust") OtherMust = xmlnodemulit.Attributes[i].Value.ToString().Split(',');
                                                if (xmlnodemulit.Attributes[i].Name == "OtherNoMust") OtherNoMust = xmlnodemulit.Attributes[i].Value.ToString().Split(',');
                                                if (xmlnodemulit.Attributes[i].Name == "Arrangement") Arrangement = xmlnodemulit.Attributes[i].Value.ToString();
                                            }
                                        }
                                    }
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Msg.Show(e.Message);

            }
        }
        #endregion

        #region 获取测量结果
        private void GetTestResult(ref DataTable dt, DataFountain dFountain, Dictionary<string, int> tempElementD,ref string strstandardContext)
        {
            if (dataFountain.curreEdition == "Rohs")
            {
                dt = new DataTable("Rohs");
                DataColumn dc0 = dt.Columns.Add("Order", Type.GetType("System.String"));
                foreach (string elename in tempElementD.Keys)
                {
                    DataColumn dc1 = dt.Columns.Add(elename + "_ElementName", Type.GetType("System.String"));
                    DataColumn dc2 = dt.Columns.Add(elename + "_ContextValue", Type.GetType("System.String"));
                    DataColumn dc3 = dt.Columns.Add(elename + "_ContextUnit", Type.GetType("System.String"));
                    DataColumn dc4 = dt.Columns.Add(elename + "_ContextColor", Type.GetType("System.String"));
                    DataColumn dc5 = dt.Columns.Add(elename + "_IntensityVaule", Type.GetType("System.String"));
                    DataColumn dc6 = dt.Columns.Add(elename + "_Error", Type.GetType("System.String"));
                    DataColumn dc7 = dt.Columns.Add(elename + "_DeterminantName", Type.GetType("System.String"));
                    DataColumn dc8 = dt.Columns.Add(elename + "_DeterminantValue", Type.GetType("System.String"));
                    DataColumn dc9 = dt.Columns.Add(elename + "_DeterminantResult", Type.GetType("System.String"));
                    DataColumn dc10 = dt.Columns.Add(elename + "_ElementNameCN", Type.GetType("System.String"));
                    DataColumn dc11 = dt.Columns.Add(elename + "_ElementNameEN", Type.GetType("System.String"));
                }
                DataColumn dc12 = dt.Columns.Add("TotalDeterminantResult", Type.GetType("System.String"));
                DataColumn dc13 = dt.Columns.Add("TotalDeterminantColor", Type.GetType("System.String"));
            }
            else if (dataFountain.curreEdition == "FPThick")
            {
                dt = new DataTable("FPThick");
                DataColumn dc0 = dt.Columns.Add("Order", Type.GetType("System.String"));
                foreach (string elename in tempElementD.Keys)
                {
                    DataColumn dc1 = dt.Columns.Add(elename + "_ElementName", Type.GetType("System.String"));
                    DataColumn dc2 = dt.Columns.Add(elename + "_ContextValue", Type.GetType("System.String"));
                    DataColumn dc3 = dt.Columns.Add(elename + "_ContextUnit", Type.GetType("System.String"));
                    DataColumn dc5 = dt.Columns.Add(elename + "_IntensityVaule", Type.GetType("System.String"));
                    DataColumn dc6 = dt.Columns.Add(elename + "_ThickValue", Type.GetType("System.String"));
                    DataColumn dc7 = dt.Columns.Add(elename + "_ThickUnit", Type.GetType("System.String"));
                    DataColumn dc8 = dt.Columns.Add(elename + "_ElementNameCN", Type.GetType("System.String"));
                    DataColumn dc9 = dt.Columns.Add(elename + "_ElementNameEN", Type.GetType("System.String"));
                }
            }
            else
            {
                dt = new DataTable("XRF");
                DataColumn dc0 = dt.Columns.Add("Order", Type.GetType("System.String"));
                foreach (string elename in tempElementD.Keys)
                {
                    DataColumn dc1 = dt.Columns.Add(elename + "_ElementName", Type.GetType("System.String"));
                    DataColumn dc2 = dt.Columns.Add(elename + "_ContextValue", Type.GetType("System.String"));
                    DataColumn dc3 = dt.Columns.Add(elename + "_ContextUnit", Type.GetType("System.String"));
                    DataColumn dc5 = dt.Columns.Add(elename + "_IntensityVaule", Type.GetType("System.String"));
                    DataColumn dc6 = dt.Columns.Add(elename + "_ElementNameCN", Type.GetType("System.String"));
                    DataColumn dc7 = dt.Columns.Add(elename + "_ElementNameEN", Type.GetType("System.String"));
                }
            }

            //添加记录其他信息
            DataColumn dc14 = dt.Columns.Add("SampleName", Type.GetType("System.String"));//样品名称
            DataColumn dc15 = dt.Columns.Add("WorkCurveName", Type.GetType("System.String"));//工作曲线
            DataColumn dc16 = dt.Columns.Add("DeviceName", Type.GetType("System.String"));//设备
            DataColumn dc17 = dt.Columns.Add("TestDate", Type.GetType("System.String"));//测量日期
            DataColumn dc18 = dt.Columns.Add("TestTime", Type.GetType("System.String"));//测量时间
            DataColumn dc19 = dt.Columns.Add("Voltage", Type.GetType("System.String"));//测量管压
            DataColumn dc20 = dt.Columns.Add("Current", Type.GetType("System.String"));//测量管流
            DataColumn dc21 = dt.Columns.Add("Supplier", Type.GetType("System.String"));//供应商
            DataColumn dc22 = dt.Columns.Add("Operator", Type.GetType("System.String"));//操作员


            //如果存在连测历史记录，则将所有连测记录即两条记录的测量结果进行组合，不是连测历史记录的则进行次数添加
            List<DataFountain> ContDataFountain = dataFountainList.FindAll(delegate(DataFountain v) { return v.ContinuousList.Count > 1; });
            List<DataFountain> NotContDataFountain = dataFountainList.FindAll(delegate(DataFountain v) { return v.ContinuousList.Count == 1; });
            int iOrder = 0;//测量次数

            double totalContext = 0f;
            double standardContext = 0f; 
            if (ContDataFountain != null && ContDataFountain.Count > 0)
            {

                #region
                var query = from cont in ContDataFountain
                            group cont by cont.ContinuousList;
                foreach (var contdf in query)//IGrouping<List<int>, DataFountain>
                {
                    if (contdf.Key == null || contdf.Key.Count < 2) continue;
                    iOrder++;
                    DataFountain dfone = ContDataFountain.Find(delegate(DataFountain v) { return v.order == contdf.Key[0]; });
                    DataFountain dftwo = ContDataFountain.Find(delegate(DataFountain v) { return v.order == contdf.Key[1]; });
                    EveryRecord(iOrder, dfone, dftwo, ref dt, ref totalContext);

                    if (dfone.LStandardData != null && dfone.LStandardData.Count > 0
                        && dfone.LStandardData[0].CustomStandard != null && dfone.LStandardData[0].CustomStandard.IsSelectTotal)
                        standardContext += dfone.LStandardData[0].CustomStandard.TotalContentStandard;

                    if (dftwo.LStandardData != null && dftwo.LStandardData.Count > 0
                        && dftwo.LStandardData[0].CustomStandard != null && dftwo.LStandardData[0].CustomStandard.IsSelectTotal)
                        standardContext += dftwo.LStandardData[0].CustomStandard.TotalContentStandard;

                }
                #endregion
            }
            if (NotContDataFountain != null && NotContDataFountain.Count > 0)
            {
                foreach (DataFountain df in NotContDataFountain)
                {
                    iOrder++;
                    EveryRecord(iOrder, df, null, ref dt, ref totalContext);
                    if (df.LStandardData != null && df.LStandardData.Count > 0
                        && df.LStandardData[0].CustomStandard != null && df.LStandardData[0].CustomStandard.IsSelectTotal)
                        standardContext += df.LStandardData[0].CustomStandard.TotalContentStandard;
                    
                }
            }

            //判定本次打印的总判定
            strstandardContext = (totalContext > standardContext) ? ExcelTemplateParams.FalseResults : ExcelTemplateParams.PassResults;

        }

        private void EveryRecord(int iOrder, DataFountain dfone, DataFountain dftwo, ref DataTable dt, ref double totalContext)
        {
            DataRow row = dt.NewRow();
            row["Order"] = iOrder;
            bool isDeterminantResult = false;
            foreach (ElemTestResult eresult in dfone.ElemTestResultL)
            {
                row[eresult.ElementName + "_ElementName"] = eresult.ElementName;
                row[eresult.ElementName + "_ContextValue"] = eresult.ContextValue;
                row[eresult.ElementName + "_ContextUnit"] = eresult.ContextUnit;
                row[eresult.ElementName + "_IntensityVaule"] = eresult.IntensityVaule;
                row[eresult.ElementName + "_ElementNameCN"] = eresult.ElementNameCN;
                row[eresult.ElementName + "_ElementNameEN"] = eresult.ElementNameEN;

                if (dataFountain.curreEdition == "Rohs")
                {
                    row[eresult.ElementName + "_ContextColor"] = eresult.ContextColor;
                    row[eresult.ElementName + "_Error"] = eresult.Error;
                    row[eresult.ElementName + "_DeterminantName"] = eresult.DeterminantName;
                    row[eresult.ElementName + "_DeterminantValue"] = eresult.DeterminantValue;
                    row[eresult.ElementName + "_DeterminantResult"] = eresult.DeterminantResult;


                    if (dfone.TestDeterminant.DeterminantResult!=null && dfone.TestDeterminant.DeterminantResult.ToLower() == "false") isDeterminantResult = true;
                }
                else if (dataFountain.curreEdition == "FPThick")
                {
                    row[eresult.ElementName + "_ThickValue"] = eresult.ThickValue;
                    row[eresult.ElementName + "_ThickUnit"] = eresult.ThickUnit;
                }

                if (eresult.ContextValue != "ND" && eresult.ContextValue != "" && eresult.ContextValue!=null)
                totalContext += double.Parse(eresult.ContextValue);
            }
            if (dftwo != null)
            {
                foreach (ElemTestResult eresult in dftwo.ElemTestResultL)
                {
                    row[eresult.ElementName + "_ElementName"] = eresult.ElementName;
                    row[eresult.ElementName + "_ContextValue"] = eresult.ContextValue;
                    row[eresult.ElementName + "_ContextUnit"] = eresult.ContextUnit;
                    row[eresult.ElementName + "_IntensityVaule"] = eresult.IntensityVaule;
                    row[eresult.ElementName + "_ElementNameCN"] = eresult.ElementNameCN;
                    row[eresult.ElementName + "_ElementNameEN"] = eresult.ElementNameEN;
                    if (dataFountain.curreEdition == "Rohs")
                    {
                        row[eresult.ElementName + "_ContextColor"] = eresult.ContextColor;
                        row[eresult.ElementName + "_Error"] = eresult.Error;
                        row[eresult.ElementName + "_DeterminantName"] = eresult.DeterminantName;
                        row[eresult.ElementName + "_DeterminantValue"] = eresult.DeterminantValue;
                        row[eresult.ElementName + "_DeterminantResult"] = eresult.DeterminantResult;

                        if (dftwo.TestDeterminant.DeterminantResult!=null && dftwo.TestDeterminant.DeterminantResult.ToLower() == "false") isDeterminantResult = true;
                    }
                    else if (dataFountain.curreEdition == "FPThick")
                    {
                        row[eresult.ElementName + "_ThickValue"] = eresult.ThickValue;
                        row[eresult.ElementName + "_ThickUnit"] = eresult.ThickUnit;
                    }

                    if (eresult.ContextValue != "ND" && eresult.ContextValue != "" && eresult.ContextValue!=null) 
                    totalContext += double.Parse(eresult.ContextValue);
                }
            }

            if (dataFountain.curreEdition == "Rohs")
            {
                row["TotalDeterminantResult"] = isDeterminantResult ? ExcelTemplateParams.FalseResults : ExcelTemplateParams.PassResults;
                row["TotalDeterminantColor"] = isDeterminantResult ? "Red" : "Green";
            }

            row["SampleName"] = dfone.specList.Name;
            row["WorkCurveName"] = dfone.workcCurrent.Name;
            row["DeviceName"] = WorkCurveHelper.DeviceCurrent.Name;//设备
            row["TestDate"] = dfone.specList.SpecDate;//测量日期
            row["TestTime"] = dfone.specList.Specs[0].DeviceParameter.PrecTime;//测量时间
            row["Voltage"] = dfone.specList.Specs[0].DeviceParameter.TubVoltage;//测量管压
            row["Current"] = dfone.specList.Specs[0].DeviceParameter.TubCurrent;//测量管流
            row["Supplier"] = dfone.specList.Supplier;//供应商
            row["Operator"] = dfone.specList.Operater;//操作员


            dt.Rows.Add(row);
        }

        #endregion

        #region 统计信息方法
        private void GetStatInfo(Dictionary<string, int> tempElementD, DataTable dt, ref List<StatInfo> LStatInfo,bool isFPThick)
        {
            string strcolname = "_ContextValue";
            if (isFPThick) strcolname = "_ThickValue";

            if (!isFPThick && dataFountain.curreEdition == "FPThick") return;

            foreach (string elementName in tempElementD.Keys)
            {
                if (this.dataFountain.workcCurrent.ElementList.Items.ToList().Find(a => a.Caption == elementName && a.IsShowElement && a.IsShowContent) == null)
                    continue;
                if (isFPThick && this.dataFountain.workcCurrent.ElementList.Items.ToList().Exists(delegate(CurveElement v) { return v.LayerNumBackUp.ToString() == "基材" && v.Caption == elementName; }) && elementName.ToLower() != "rh") continue;
                StatInfo statinfo = null;
                Dictionary<int, double>  contentL=new Dictionary<int,double>();

                string unit = (dt!=null && dt.Rows.Count>0)?((isFPThick) ? dt.Rows[0][elementName + "_ThickUnit"].ToString() : dt.Rows[0][elementName + "_ContextUnit"].ToString()):"";
                
                double maxValue = 0d;
                double minValue = 0d;
                double avaValue = 0d;
                double sdValue = 0d;
                double redValue = 0d;

                double totalValue=0d;//当前元素含量之和
                bool cancleOperate = false;
                int testResultList = 0;
                foreach (DataRow row in dt.Rows)//_ContextUnit
                {

                    if (row[elementName + strcolname] != null && row[elementName + strcolname].ToString() == "ND")
                    {
                        cancleOperate = true; 
                    }
                    else if (row[elementName + strcolname] != null && row[elementName + strcolname].ToString() != "ND" && row[elementName + strcolname].ToString() != "")
                    {
                        testResultList++;
                        double val = double.Parse(row[elementName + strcolname].ToString());

                        //val = (isFPThick) ? double.Parse(val.ToString("f" + WorkCurveHelper.ContentBit)) : val;
                        contentL.Add(contentL.Count + 1, val);

                        maxValue.ToString("f" + WorkCurveHelper.SoftWareContentBit);
                        //获取最大值和最小值
                        if (val > maxValue || testResultList==1) maxValue = val;
                        if (val < minValue || testResultList == 1) minValue = val;
                        totalValue+=val;
                    }
                }
                if (cancleOperate)
                {
                    //修改：何晓明 20110927 ND改为数字与ND兼容
                    //statinfo = new StatInfo(elementName, "ND","ND","ND","ND","");
                    if(WorkCurveHelper.isShowND)
                    {
                        statinfo = new StatInfo(elementName, "ND", "ND", "ND", "ND", "", unit);
                    }
                    else 
                    {
                        statinfo = new StatInfo(elementName, maxValue.ToString(), minValue.ToString(), avaValue.ToString(), sdValue.ToString(), redValue.ToString(), unit);
                    }
                    //
                    LStatInfo.Add(statinfo);
                    continue;
                }
                //获取平均值
                avaValue = (testResultList==0)?0:totalValue / testResultList;
                //获取多次测量的平方之和，每次测试的某元素减去平均值的平方
                double squareTotal = 0d;
                foreach (int val in contentL.Keys)
                {
                    squareTotal += ((contentL[val] - avaValue) * (contentL[val] - avaValue));
                }
                squareTotal = ((testResultList - 1)==0)?0:squareTotal / (testResultList - 1);

                if (testResultList > 1)
                {
                    sdValue = Math.Sqrt(squareTotal);
                }
                else
                {
                    sdValue = 0d;
                }

                redValue = (avaValue==0)?0:(sdValue / avaValue)*100;

                if (dataFountain.curreEdition == "FPThick" && !isFPThick)
                {
                    maxValue = double.Parse(maxValue.ToString("f" + WorkCurveHelper.SoftWareContentBit));
                    minValue = double.Parse(minValue.ToString("f" + WorkCurveHelper.SoftWareContentBit));
                    avaValue = double.Parse(avaValue.ToString("f" + WorkCurveHelper.SoftWareContentBit));
                    sdValue = double.Parse(sdValue.ToString("f" + WorkCurveHelper.SoftWareContentBit));
                    redValue = double.Parse(redValue.ToString("f" + WorkCurveHelper.SoftWareContentBit));
                }
                else if (dataFountain.curreEdition == "FPThick" && isFPThick)
                {
                    maxValue = double.Parse(maxValue.ToString("f" + WorkCurveHelper.ThickBit));
                    minValue = double.Parse(minValue.ToString("f" + WorkCurveHelper.ThickBit));
                    avaValue = double.Parse(avaValue.ToString("f" + WorkCurveHelper.ThickBit));
                    sdValue = double.Parse(sdValue.ToString("f" + WorkCurveHelper.ThickBit));
                    redValue = double.Parse(redValue.ToString("f" + WorkCurveHelper.ThickBit));
                }

                statinfo = new StatInfo((isFPThick) ? elementName + "'" : elementName, maxValue.ToString(), minValue.ToString(), avaValue.ToString(), sdValue.ToString(), redValue.ToString(), unit);

                LStatInfo.Add(statinfo);
            }

        }

            /// <summary>
        /// 根据测量次数信息统计各个元素的各个参数
        /// </summary>
        /// <param name="MinValue">最小值</param>
        /// <param name="MaxValue">最大值</param>
        /// <param name="AvgValue">平均值</param>
        /// <param name="SdValue">方差</param>
        /// <param name="dataGridView">当前测量含量容器</param>
        /// <param name="testCurrentTimes">当前扫描次数</param>
        /// <param name="colName">列名</param>
        public static void CaculateStaticsData(ref double MinValue, ref double MaxValue, ref double AvgValue,
                    ref double SdValue, DataGridView dataGridView, int testCurrentTimes, string colName)
        {
            if (dataGridView == null)
                return;
            double cell, sum, sum2, min, max, warp;
            if (dataGridView[colName, 0].Value != null)
            {
                cell = Convert.ToDouble(dataGridView[colName, 0].Value.ToString());
                min = cell;
                max = min;
                sum = min;
                sum2 = cell * cell;
                for (int row = 1; row < testCurrentTimes; row++)
                {
                    cell = Convert.ToDouble(dataGridView[colName, row].Value.ToString());
                    if (min > cell)
                    {
                        min = cell;
                    }
                    else if (max < cell)
                    {
                        max = cell;
                    }
                    sum += cell;
                    sum2 += cell * cell;
                }
                MinValue = min; //最小值
                MaxValue = max; //最大值
                sum /= testCurrentTimes;
                AvgValue = sum; //求和
                if (testCurrentTimes > 1)
                {
                    warp = Math.Sqrt(Math.Abs(sum2 - sum * sum * testCurrentTimes) / (testCurrentTimes - 1));
                    SdValue = warp;
                }
                else
                {
                    SdValue = 0d;
                }
            }
        }

        #endregion

        private void ElementImage(SpecEntity tempSpec, SpecEntity unitSpec, int specCount, List<int> FirstContIntr, ElementList Elements, string ElementName, List<DataFountain> dataFountainConList, ref Dictionary<string, Bitmap> ElementSpecImage)
        {
            Bitmap bmp = new Bitmap(120, 60);
            Graphics g = Graphics.FromImage(bmp);
            //int width = (int)Math.Round(Convert.ToInt32(bmp.Width) * g.DpiX / 72.0);
            //int height = (int)Math.Round(Convert.ToInt32(bmp.Height) * g.DpiY / 72.0);

            int width = 400;
            int height = 200;

            bmp = new Bitmap(width, height);

            Report report = new Report();
            report.isShowND = WorkCurveHelper.isShowND;
            report.Spec = tempSpec;
            //report.operateMember = FrmLogon.userName;
            report.Elements = Elements;
            //report.WorkCurveName = vdataFountain.workcCurrent.Name;
            //report.FirstContIntr.Add(elementList.Items.Count);
            report.InterestElemCount = Elements.Items.Count;

            if (dataFountainConList != null)
                report.curElemCount = dataFountainConList[0].historyElementList.Count - 1;
            report.unitSpec = unitSpec;
            report.FirstContIntr = FirstContIntr;
            report.specCount = specCount;


            report.DrawInterstringElems(ref bmp);
            //DrawInterstringElems(ref bmp, (dataFountain.SpecData == null) ? dataFountain.specList.Specs[0] : dataFountain.SpecData, curveElement);
            ElementSpecImage.Add(ElementName, bmp);

        }

        private void GetCompanyOtherInfo(ref Dictionary<string,string> DCompanyOtherInfo)
        {
            string sReportPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml//CompanyInfo.xml";
            XmlDocument xmlDocReport = new XmlDocument();
            xmlDocReport.Load(sReportPath);

            string strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.LoadReportName().Replace("EN.", ".").Replace("En.", ".") + "']";


            XmlNodeList xmlnodelist = xmlDocReport.SelectNodes(strWhere);
            if (xmlnodelist == null || xmlnodelist.Count == 0) return;
            foreach (XmlNode xmlnode in xmlnodelist)
            {
                //获取支节点信息
                foreach (XmlNode childxmlnode in xmlnode.ChildNodes)
                {
                    string sd = childxmlnode.Attributes[WorkCurveHelper.LanguageShortName].Value;
                    string sTarget = childxmlnode.Attributes["Target"].Value;

                    string strsql = "select * from historycompanyotherinfo a " +
                                                               " left outer join companyothersinfo b on b.id=a.companyothersinfo_id " +
                                                               " where a.workcurveid='" + dataFountain.workcCurrent.Id + "' and a.history_id='" + dataFountain.historyRecord.Id + "' and b.name='" + sd + "' and b.isreport=1";
                    List<HistoryCompanyOtherInfo> hisCompanyOtherInfoList = HistoryCompanyOtherInfo.FindBySql(strsql);
                    string sReplaceText = "";
                    if (hisCompanyOtherInfoList != null && hisCompanyOtherInfoList.Count > 0)
                        sReplaceText = hisCompanyOtherInfoList[0].ListInfo;


                    DCompanyOtherInfo.Add(sd + "-" + sTarget, sReplaceText);

                }
            }
        }



        #region 画图

        ///<summary>
        /// 获取谱图
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public static Bitmap GetSpecImage(List<SpecEntity> tempSpec, List<SpecEntity> unitSpec, int specCount, List<int> FirstContIntr, ElementList Elements)
        {
            Bitmap bmpt = new Bitmap(480, 240);
            Graphics gt = Graphics.FromImage(bmpt);
            int bmpHeight = (240 / tempSpec.Count) - (5 * tempSpec.Count);
            int i = 0;
            foreach (SpecEntity tepSpec in tempSpec)
            {
                
                Bitmap bmp = new Bitmap(60, 60);
                Graphics g = Graphics.FromImage(bmp);
                //int width = (int)Math.Round(Convert.ToInt32(bmp.Width) * g.DpiX / 72.0);
                //int height = (int)Math.Round(Convert.ToInt32(bmp.Height) * g.DpiY / 72.0);
                int width = 480;
                int height = bmpHeight;
                bmp = new Bitmap(width, height);
                Report report = new Report();
                report.isShowND = WorkCurveHelper.isShowND;
                report.Spec = tepSpec;
                report.unitSpec = (unitSpec != null) ? unitSpec[0] : null;
                report.FirstContIntr = FirstContIntr;
                report.specCount = specCount;
                report.Elements = Elements;
                report.specList = new SpecListEntity();
                report.specList.Specs = new SpecEntity[1];
                report.specList.Specs[0] = tepSpec;
                report.DrawSpec(ref bmp,false);
                


                gt.DrawImageUnscaled(bmp, 1, i * bmpHeight + 5);
                i++;

            }
            //DrawSpec(ref bmp, tempSpec);
            gt.Dispose();
            return bmpt;


            //Bitmap bmp = new Bitmap(120, 60);
            //Graphics g = Graphics.FromImage(bmp);
            ////int width = (int)Math.Round(Convert.ToInt32(bmp.Width) * g.DpiX / 72.0);
            ////int height = (int)Math.Round(Convert.ToInt32(bmp.Height) * g.DpiY / 72.0);
            //int width = 400;
            //int height = 200;
            //bmp = new Bitmap(width, height);

            //Report report = new Report();
            //report.Spec = tempSpec[1];
            //report.unitSpec = (unitSpec != null) ? unitSpec[0] : null;
            //report.FirstContIntr = FirstContIntr;
            //report.specCount = specCount;
            //report.Elements = Elements;
            //report.DrawSpec(ref bmp);
            //return bmp;
        }

        ///<summary>
        /// 获取样品图
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public static Bitmap GetSampleImage(byte[] Image)
        {
            Bitmap b = null;
            if (Image == null) return null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Image))
            {
                b = new Bitmap(ms);
                return b;
            }
        }



        /// <summary>
        /// 清空点数据
        /// </summary>
        /// <param name="p"></param>
        private void ClearPoint(ref Point[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                p[i].X = 0;
                p[i].Y = 0;
            }
        }

        #endregion

        public List<TreeNodeInfo> GetZhongDa(List<DataFountain> dataFountainList, PrintInitParam printInitParam
            , PrintWorkCurve printWorkCurve, PrintSample printSample, Bitmap SpecImage, PrintSpecImage printSpecImage
            , DataTable dt_TestResult, List<PrintDeviceParameter> LprintDeviceParameter, BaseInfoCls baseInfocls, List<StatInfo> statInfoList, string TotalDeterminant, Dictionary<string, string> DCompanyOtherInfo)
        {
            var template = new List<TreeNodeInfo>();
            XElement xele = XElement.Load(Application.StartupPath + "\\printxml\\data.xml");


            XElement xeLabel = XElement.Load(Application.StartupPath + "\\printxml\\label" + WorkCurveHelper.LanguageShortName + ".xml");

            var label = xeLabel.Elements("Label").ToList();
            foreach (XElement xe in label)
            {
                template.Add(new TreeNodeInfo
                {
                    Type = CtrlType.Label,
                    Name = xe.Element("Name").Value,
                    Text = xe.Element("Text").Value,
                    Caption = xe.Element("Caption").Value
                });
            }

            #region 初始化参数
            var lbl = xele.Elements("Label").ToList();
            foreach (XElement xe in lbl)
            {
                List<object> lSouce = new List<object>();
                List<DataTable> lDT = new List<DataTable>();
                #region
                DataTable dt = new DataTable();
                dt.Columns.Add("a");
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = "11111111";
                dt.Rows.Add(dr);
                #endregion

                if (xe.Attribute("id").Value == "基本信息")
                {
                    Type t = ClassHelper.BuildType("TempCls");
                    List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
                    ClassHelper.CustPropertyInfo cpi;
                    foreach (XElement a in xe.Nodes().ToList())
                    {
                        cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
                        lcpi.Add(cpi);
                    }
                    t = ClassHelper.AddProperty(t, lcpi);
                    object obj = ClassHelper.CreateInstance(t);
                    BaseInfoCls model = baseInfocls;
                    Do(model, xe, obj);
                    lDT.Add(dt);
                    lSouce.Add(obj);
                    GetNote(template, lSouce, lDT,"基本信息", Info.strBasicInfo);
                }
                else if (xe.Attribute("id").Value == "工作曲线")
                {
                    Type t = ClassHelper.BuildType("TempCls");
                    List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
                    ClassHelper.CustPropertyInfo cpi;
                    foreach (XElement a in xe.Nodes().ToList())
                    {
                        cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
                        lcpi.Add(cpi);
                    }
                    t = ClassHelper.AddProperty(t, lcpi);
                    object obj = ClassHelper.CreateInstance(t);
                    PrintWorkCurve model = printWorkCurve;
                    Do(model, xe, obj);
                    lDT.Add(dt);
                    lSouce.Add(obj);
                    GetNote(template, lSouce, lDT, "工作曲线",Info.WorkingCurve);
                }
                else if (xe.Attribute("id").Value == "样品信息")
                {
                    Type t = ClassHelper.BuildType("TempCls");
                    List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
                    ClassHelper.CustPropertyInfo cpi;
                    foreach (XElement a in xe.Nodes().ToList())
                    {
                        cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
                        lcpi.Add(cpi);
                    }
                    t = ClassHelper.AddProperty(t, lcpi);
                    object obj = ClassHelper.CreateInstance(t);
                    PrintSample model = printSample;
                    Do(model, xe, obj);
                    lDT.Add(dt);
                    lSouce.Add(obj);
                    GetNote(template, lSouce, lDT, "样品信息",Info.SampleInfo);
                }
                else if (xe.Attribute("id").Value == "测量条件")
                {
                    Type t = ClassHelper.BuildType("TempCls");
                    List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
                    ClassHelper.CustPropertyInfo cpi;
                    foreach (XElement a in xe.Nodes().ToList())
                    {
                        cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
                        lcpi.Add(cpi);
                    }
                    t = ClassHelper.AddProperty(t, lcpi);
                    object obj = ClassHelper.CreateInstance(t);
                    PrintDeviceParameter model = LprintDeviceParameter[0];
                    Do(model, xe, obj);
                    lDT.Add(dt);
                    lSouce.Add(obj);
                    GetNote(template, lSouce, lDT, "测量条件",Info.Condition);
                }
                else if (xe.Attribute("id").Value == "总判定")
                {
                    #region
                    //Type t = ClassHelper.BuildType("TempCls");
                    //List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
                    //ClassHelper.CustPropertyInfo cpi;
                    //foreach (XElement a in xe.Nodes().ToList())
                    //{
                    //    cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
                    //    lcpi.Add(cpi);
                    //}
                    //t = ClassHelper.AddProperty(t, lcpi);
                    //object obj = ClassHelper.CreateInstance(t);
                    //TotalDeterminant model = new TotalDeterminant();
                    //model.TotalDeterminant1 = TotalDeterminant;
                    //Do(model, xe, obj);
                    //lDT.Add(dt);
                    //lSouce.Add(obj);
                    //GetNote(template, lSouce, lDT, Info.TotalPassReslt);
                    #endregion
                    template.Add(new TreeNodeInfo
                    {
                        Type = CtrlType.Label,
                        Name = "总判定",
                        Text = Info.strTotalPassReslt+ ":" + TotalDeterminant,
                        Caption = Info.strTotalPassReslt
                    });
                }
                else if (xe.Attribute("id").Value == "K值" && dataFountain.curreEdition=="EDXRF" && this.dataFountainList.Count==1)
                {
                    //获取金的含量
                    ElemTestResult auElemTestResult=dataFountain.ElemTestResultL.Find(delegate(ElemTestResult v) { return v.ElementName.ToLower() == "au"; });
                    double dKValue = 0;
                    if (auElemTestResult != null)
                    {
                        //stringAu = (auElemTestResult.ContextValue * 24 / 99.995).ToString("f4") + "K";

                        dKValue = (double.Parse(auElemTestResult.ContextValue) * 24) / WorkCurveHelper.KaratTranslater;
                    }

                        template.Add(new TreeNodeInfo
                        {
                            Type = CtrlType.Label,
                            Name = "K值",
                            Text = dKValue.ToString("f"+WorkCurveHelper.SoftWareContentBit.ToString()) + "K",
                            Caption = Info.IncludingAu
                        });
                }
                else if (xe.Attribute("id").Value == "Remarks" && dataFountain.curreEdition == "Rohs")
                {
                    template.Add(new TreeNodeInfo
                    {
                        Type = CtrlType.Label,
                        Name = "Remarks",
                        Text = (WorkCurveHelper.isShowND? Info.Remarks + WorkCurveHelper.NDValue.ToString() + "ppm":""),
                        Caption = Info.Remarks
                    });
                }
                //else if (xe.Attribute("id").Value == "接受" && dataFountain.curreEdition == "Rohs")
                //{
                //    string strCOtherInfoValue = "";
                //    DCompanyOtherInfo.TryGetValue("接受", out strCOtherInfoValue);
                //    if (strCOtherInfoValue == null || strCOtherInfoValue == "") strCOtherInfoValue = ""; 
                //    template.Add(new TreeNodeInfo
                //    {
                //        Type = CtrlType.Field,
                //        Name = "接受",
                //        Text = strCOtherInfoValue,
                //        Caption = "接受"
                //    });
                //}
                else if (xe.Attribute("id").Value == "测试日期" && dataFountain.curreEdition == "Rohs")
                {
                    string strCOtherInfoValue = "";
                    strCOtherInfoValue = DCompanyOtherInfo.ToList().Find(l => l.Key.Contains("DateOfTest")).Value;
                    if (strCOtherInfoValue == null || strCOtherInfoValue == "") strCOtherInfoValue = "";
                    template.Add(new TreeNodeInfo
                    {
                        Type = CtrlType.Field,
                        Name = "测试日期",
                        Text = strCOtherInfoValue,
                        Caption = Info.SpecDate
                    });
                }
                //else if (xe.Attribute("id").Value == "供给" && dataFountain.curreEdition == "Rohs")
                //{
                //    string strCOtherInfoValue = "";
                //    DCompanyOtherInfo.TryGetValue("供给", out strCOtherInfoValue);
                //    if (strCOtherInfoValue == null || strCOtherInfoValue == "") strCOtherInfoValue = "";
                //    template.Add(new TreeNodeInfo
                //    {
                //        Type = CtrlType.Field,
                //        Name = "供给",
                //        Text = strCOtherInfoValue,
                //        Caption = "供给"
                //    });
                //}
                else if (xe.Attribute("id").Value == "测试者" && dataFountain.curreEdition == "Rohs")
                {
                    string strCOtherInfoValue = "";
                    strCOtherInfoValue = DCompanyOtherInfo.ToList().Find(l => l.Key.Contains("Operator")).Value;
                    //DCompanyOtherInfo.TryGetValue("测试者", out strCOtherInfoValue);
                    if (strCOtherInfoValue == null || strCOtherInfoValue == "") strCOtherInfoValue = "";
                    template.Add(new TreeNodeInfo
                    {
                        Type = CtrlType.Field,
                        Name = "测试者",
                        Text = strCOtherInfoValue,
                        Caption = Info.Operator
                    });
                }


            }




            #endregion

            #region 复合表测量结果和统计信息
            var table = xele.Elements("Table").ToList();
            foreach (XElement xe in table)
            {
                List<DataTable> lDT = new List<DataTable>();
                List<object> lSouce = new List<object>();
                var DT = new TempDt();
                DataTable dt1 = null;
                if (xe.Attribute("id").Value == "测试结果")
                {

                    //设置数据源列头
                    Dictionary<string, string> ElemD = new Dictionary<string, string>();
                    Dictionary<string, string> ElemFPThick = new Dictionary<string, string>();
                    Dictionary<string, string> DOtherMust = new Dictionary<string, string>();
                    SetTableColumn(ref dt1, ref ElemD, ref DOtherMust, ref ElemFPThick, dt_TestResult);

                    if (dt_TestResult.Rows.Count > 1 || dataFountain.curreEdition == "XRF" || isOriginal)
                    {
                        #region 多次结果

                        //增加列名称记录
                        if (!isOriginal)
                        {
                            DataRow newrowC = dt1.NewRow();
                            foreach (string selen in ElemD.Keys)
                            {
                                newrowC[ElemD[selen]] = ElemD[selen];
                            }
                            foreach (string colname in DOtherMust.Keys)
                            {
                                if (colname == "样品名称")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "工作曲线")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "强度")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "误差")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "限定标准")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "判定")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];

                                else if (colname == "设备")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "测量日期")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "测量时间")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "测量管压")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "测量管流")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "供应商")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                                else if (colname == "操作员")
                                    newrowC[DOtherMust[colname]] = DOtherMust[colname];
                            }

                            //if (isDeterminant) newrowC["Determinant"] = Info.strPassReslt;
                            dt1.Rows.Add(newrowC);
                        }
                        
                        foreach (DataRow row in dt_TestResult.Rows)
                        {

                            DataRow newrow = dt1.NewRow();
                            newrow[Info.Order] = dt1.Rows.Count;
                            foreach (string selen in ElemD.Keys)
                            {
                                string sContextValue = row[selen + "_ContextValue"].ToString();
                                if (isShowUnit && dataFountain.curreEdition != "Rohs")
                                    sContextValue += row[selen + "_ContextUnit"].ToString();
                                newrow[ElemD[selen]] = sContextValue + ((dataFountain.curreEdition == "Rohs" && row[selen + "_ContextColor"].ToString() != "" && !isOriginal) ? "~" + row[selen + "_ContextColor"] : "");

                            }

                            if (dataFountain.curreEdition == "FPThick")
                            {
                                foreach (string selen in ElemFPThick.Keys)
                                {
                                    string sThickValue = row[selen.Replace("'", "") + "_ThickValue"].ToString();
                                    if (isShowUnit && dataFountain.curreEdition != "Rohs")
                                        sThickValue += row[selen.Replace("'", "") + "_ThickUnit"].ToString();

                                    newrow[ElemFPThick[selen]] = sThickValue;
                                }

                            }


                            foreach (string colname in DOtherMust.Keys)
                            {
                                if (colname == "样品名称")
                                    newrow[DOtherMust[colname]] = row["SampleName"];
                                else if (colname == "工作曲线")
                                    newrow[DOtherMust[colname]] = row["WorkCurveName"];
                                else if (colname == "判定")
                                    newrow[DOtherMust[colname]] = row["TotalDeterminantResult"] + (isOriginal ? "" : "~" + row["TotalDeterminantColor"]);
                                else if (colname == "设备")
                                    newrow[DOtherMust[colname]] = row["DeviceName"];
                                else if (colname == "测量日期")
                                    newrow[DOtherMust[colname]] = row["TestDate"];
                                else if (colname == "测量时间")
                                    newrow[DOtherMust[colname]] = row["TestTime"];
                                else if (colname == "测量管压")
                                    newrow[DOtherMust[colname]] = row["Voltage"];
                                else if (colname == "测量管流")
                                    newrow[DOtherMust[colname]] = row["Current"];
                                else if (colname == "供应商")
                                    newrow[DOtherMust[colname]] = row["Supplier"];
                                else if (colname == "操作员")
                                    newrow[DOtherMust[colname]] = row["Operator"];
                            }
                            dt1.Rows.Add(newrow);
                        }
                        

                        int colMun = 8;
                        if (dt1.Columns.Count > colMun && dataFountain.curreEdition == "XRF")
                        {
                            DataTable dt = new DataTable();
                            for (int i = 1; i <= colMun; i++)
                            {
                                DataColumn dc0 = dt.Columns.Add(i.ToString(), Type.GetType("System.String"));
                            }


                            int col = 0;
                            int row = 0;
                            for (int i = 0; i < dt1.Columns.Count; i++)
                            {
                                col++;
                                if (col == colMun || i == dt1.Columns.Count - 1)
                                {
                                    GetRow(colMun * row, i, ref dt, dt1);
                                    row++;
                                    col = 0;
                                }


                            }
                            dt1 = dt;
                        }
                       

                        #endregion
                    }
                    else if (dt_TestResult.Rows.Count == 1 && Arrangement == "1")
                    {
                        #region 单次纵向

                        DataRow dtrow = dt_TestResult.Rows[0];
                        foreach (string elename in ElemD.Keys)
                        {
                            DataRow newrow = dt1.NewRow();
                            newrow["ElementName"] = ElemD[elename];

                            string sContextValue = dtrow[elename + "_ContextValue"].ToString();
                            if (isShowUnit && dataFountain.curreEdition != "Rohs")
                                sContextValue +=  dtrow[elename + "_ContextUnit"].ToString() ;


                            newrow["ContextValue"] = sContextValue + ((dataFountain.curreEdition == "Rohs" && dtrow[elename + "_ContextColor"].ToString() != "") ? "~" + dtrow[elename + "_ContextColor"] : ""); ;
               


                            foreach (string colname in DOtherMust.Keys)
                            {
                                if (colname == "样品名称")
                                    newrow[DOtherMust[colname]] = dtrow[elename + "_IntensityVaule"];
                                else if (colname == "工作曲线")
                                    newrow[DOtherMust[colname]] = dtrow[elename + "_IntensityVaule"];
                                else if (colname == "强度")
                                    newrow[DOtherMust[colname]] = dtrow[elename + "_IntensityVaule"];
                                else if (colname == "误差")
                                    newrow[DOtherMust[colname]] = dtrow[elename + "_Error"];
                                else if (colname == "限定标准")
                                    newrow[DOtherMust[colname]] = dtrow[elename + "_DeterminantValue"];
                                else if (colname == "判定")
                                    newrow[DOtherMust[colname]] = dtrow["TotalDeterminantResult"] + "~" + dtrow["TotalDeterminantColor"];
                            }


                            if (dataFountain.curreEdition == "FPThick")
                            {
                                string sThickValue = dtrow[elename + "_ThickValue"].ToString();
                                if (isShowUnit && dataFountain.curreEdition != "Rohs")
                                    sThickValue += dtrow[elename + "_ThickUnit"].ToString();
                                newrow["ThickValue"] = sThickValue;
                            }

                            


                            //if (isDeterminant) newrow["Determinant"] = dtrow["TotalDeterminantResult"] + "~" + dtrow["TotalDeterminantColor"];
                            dt1.Rows.Add(newrow);
                        }


                        #endregion
                    }
                    else if (dt_TestResult.Rows.Count == 1 && Arrangement == "0")
                    {
                        #region 单次横向
                        DataRow dtrow = dt_TestResult.Rows[0];
                        for (int i = 0; i < 6; i++)
                        {
                            if (dataFountain.curreEdition != "Rohs" && i > 3) continue;
                            DataRow newrow = dt1.NewRow();
                            switch (i)
                            {
                                case 0: newrow["order"] = Info.strElementAllName;
                                    foreach (string elename in ElemD.Keys)
                                        newrow[ElemD[elename]] = (CurrentLanguage == "CN") ? dtrow[elename + "_ElementNameCN"] : dtrow[elename + "_ElementNameEN"];
                                    break;
                                case 1: newrow["order"] = Info.strElementName;
                                    foreach (string elename in ElemD.Keys)
                                        newrow[ElemD[elename]] = ElemD[elename];
                                    break;
                                case 2: newrow["order"] = Info.EditContent;
                                    foreach (string elename in ElemD.Keys)
                                    {
                                        string sContextValue = dtrow[elename + "_ContextValue"].ToString();
                                        if (isShowUnit && dataFountain.curreEdition != "Rohs")
                                            sContextValue +=  dtrow[elename + "_ContextUnit"].ToString() ;


                                        newrow[ElemD[elename]] = sContextValue + ((dataFountain.curreEdition == "Rohs" && dtrow[elename + "_ContextColor"].ToString() != "") ? "~" + dtrow[elename + "_ContextColor"] : "");
                                    }
                                    break;
                                case 3: newrow["order"] = Info.Intensity;
                                    foreach (string elename in ElemD.Keys)
                                        newrow[ElemD[elename]] = dtrow[elename + "_IntensityVaule"];
                                    break;
                                case 4: newrow["order"] = Info.strStandard;
                                    foreach (string elename in ElemD.Keys)
                                        newrow[ElemD[elename]] = dtrow[elename + "_DeterminantValue"];
                                    break;
                                case 5: newrow["order"] = Info.strPassReslt;
                                    foreach (string elename in ElemD.Keys)
                                        newrow[ElemD[elename]] = dtrow[elename + "_DeterminantResult"];
                                    break;
                            }
                            dt1.Rows.Add(newrow);

                        }
                        #endregion
                    }
                    DT.DT = dt1;
                    lSouce.Add(DT);
                    lDT.Add(dt1);
                    if (isOriginal)
                        GetNote(template, lSouce, lDT[0], "测试结果", Info.TestResult);
                    else
                        GetNote(template, lSouce, lDT, "测试结果", Info.TestResult);
                }
                else if (xe.Attribute("id").Value == "统计信息")
                {
                    dt1 = new DataTable();
                    foreach (XElement a in xe.Nodes().ToList())
                    {
                        if (dataFountain.curreEdition != "FPThick" && a.Value == "相对标准偏差") continue;
                        dt1.Columns.Add(a.Value);
                    }
                    List<StatInfo> ldt1 = statInfoList;
                    if (dataFountain.curreEdition != "FPThick")
                    {
                        DataRow dr;
                        foreach (StatInfo model in ldt1)
                        {
                            dr = dt1.NewRow();
                            dt1.Rows.Add(GetDr(dt1, dr, xe, model, dataFountain.curreEdition));
                        }
                    }
                    else
                    {
                        DataRow dr;
                        List<string> layernumberList = new List<string>();//当为FPThick时，存储已经选择的层
                        foreach (StatInfo model in ldt1)
                        {
                            if (model.StatColumns.IndexOf("'") != -1)
                            {
                                string elecolFP = "";
                                CurveElement curveElement = dataFountain.workcCurrent.ElementList.Items.ToList().Find(delegate(CurveElement v) { return v.Caption == model.StatColumns.Replace("'", ""); });
                                if (curveElement != null &&  !layernumberList.Contains(curveElement.LayerNumber.ToString()))
                                {
                                    layernumberList.Add(curveElement.LayerNumber.ToString());
                                    List<CurveElement> CurveElementList = dataFountain.workcCurrent.ElementList.Items.ToList().FindAll(delegate(CurveElement v) { return v.LayerNumber == curveElement.LayerNumber; });
                                    foreach (CurveElement cElement in CurveElementList)
                                        elecolFP += cElement.Caption + "|";
                                    if (elecolFP != "") elecolFP = elecolFP.Substring(0, elecolFP.Length - 1);

                                    dr = dt1.NewRow();
                                    dt1.Rows.Add(GetDr(dt1, dr, xe, model, dataFountain.curreEdition, elecolFP, model.Unit));

                                }
                            }
                            else
                            {


                                dr = dt1.NewRow();
                                dt1.Rows.Add(GetDr(dt1, dr, xe, model, dataFountain.curreEdition, "", model.Unit));
                            }
                        }
                    }
                    DT.DT = dt1;
                    lSouce.Add(DT);
                    lDT.Add(dt1);
                    GetNote(template, lSouce, lDT, "统计信息", Info.StatisticsInfo);
                }
            }
            #endregion

            #region 谱图
            var image = xele.Elements("Image").ToList();
            foreach (XElement xe in image)
            {
                List<object> lSouce = new List<object>();
                List<DataTable> lDT = new List<DataTable>();
                DataTable dt = new DataTable();
                dt.Columns.Add("a");
                if (xe.Attribute("id").Value == "谱图")
                {
                    Image imagee = null;
                    //if (dataFountain.ByteSpecData == null)
                    //    imagee = GetSpecImage(dataFountain.SpecData);
                    //else
                    imagee = SpecImage;//GetSampleImage(dataFountain.ByteSpecData);
                    template.Add(new TreeNodeInfo
                    {
                        Type = CtrlType.Image,
                        Name = "谱图",
                        Text = Info.SampleGraph,
                        Image = imagee
                    });
                }
                else if (xe.Attribute("id").Value == "样品图")
                {
                    template.Add(new TreeNodeInfo
                    {
                        Type = CtrlType.Image,
                        Name = "样品图",
                        Text = "",
                        Image = (GetSampleImage(dataFountain.SampleImage) == null) ? Image.FromFile(Application.StartupPath + "\\default.jpg") : GetSampleImage(dataFountain.SampleImage)
                    });
                }
                else if (xe.Attribute("id").Value == "元素图")
                {
                    Dictionary<string, Bitmap> d = printSpecImage.ElementSpecImage;
                    foreach (KeyValuePair<string, Bitmap> a in d)
                    {
                        template.Add(new TreeNodeInfo
                        {
                            Type = CtrlType.Image,
                            Name = "元素",
                            Text = Info.ElementName,
                            Image = a.Value
                        });
                        //if (languageModel.CurrentLangId == 2)//中文
                        //{
                        //    template.Add(new TreeNodeInfo
                        //    {
                        //        Type = CtrlType.Image,
                        //        Name = "元素",
                        //        Text = Info.ElementName,
                        //        Image = a.Value
                        //    });
                        //}
                        //else
                        //{
                        //    template.Add(new TreeNodeInfo
                        //    {
                        //        Type = CtrlType.Image,
                        //        Name = "Element",
                        //        Text = "Element",
                        //        Image = a.Value
                        //    });
                        //}
                        break;
                    }

                }
            }

            #endregion
            return template;
        }

        private void GetRow(int ibef,int iend,ref DataTable dt,DataTable dtold)
        {
            int icol=0;
            foreach (DataRow row in dtold.Rows)
            {
                icol = 0;
                DataRow newrow = dt.NewRow();
                for (int i = ibef; i <= iend; i++)
                {
                    newrow[icol] = row[i];
                    icol++;
                }
                dt.Rows.Add(newrow);
            }
        }

        private void SetTableColumn(ref DataTable dt, ref Dictionary<string, string> ElemD,ref Dictionary<string, string> DOtherMust,ref Dictionary<string, string> ElemFPThick, DataTable dt_TestResult)
        {
            dt = new DataTable("TestResult");
            if (dt_TestResult.Rows.Count > 1 || dataFountain.curreEdition == "XRF" || isOriginal || (dt_TestResult.Rows.Count == 1 && Arrangement == "0"))
            {
                DataColumn dc0 = dt.Columns.Add(Info.Order, Type.GetType("System.String"));
            }

            if (dataFountain.curreEdition != "Rohs") SetOtherColumns(ref dt, ref DOtherMust);
            
            if (Arrangement == "0")
            {
                List<string> elenameList = new List<string>();
                foreach (DataColumn col in dt_TestResult.Columns)
                {
                    if (col.ColumnName.IndexOf("_") == -1 || elenameList.Contains(col.ColumnName.Split('_')[0])) continue;
                    elenameList.Add(col.ColumnName.Split('_')[0]);
                }
                if (dataFountain.curreEdition == "FPThick")
                {
                    Dictionary<string,int> delenameList = new Dictionary<string,int>(); //int为层，string为在这层的元素
                    foreach (string s in elenameList)
                    {
                        CurveElement curveElement = dataFountain.workcCurrent.ElementList.Items.ToList().Find(delegate(CurveElement v) { return v.Caption == s; });
                        if (curveElement == null || curveElement.LayerNumber == -1) continue;
                        delenameList.Add(s, curveElement.LayerNumber);

                    }

                    var LayerNumberobjec = from elelist in delenameList orderby elelist.Value group elelist by elelist.Value into LayerNumberelelist select LayerNumberelelist;
                    foreach (var tt in LayerNumberobjec)
                    {
                        string elename = "";
                        string layerelename = "";
                        foreach (string strname in delenameList.Keys)
                        {
                            if (delenameList[strname] != tt.Key) continue;
                            elename = strname;
                            layerelename += strname + "|";
                            if (dataFountain.workcCurrent.ElementList.Items.ToList().Find(a => a.Caption == strname && a.IsShowElement && a.IsShowContent) != null)
                            //if (dataFountain.workcCurrent.IsThickShowContent)
                            {
                                string elecol = elename + Info.EditContent;
                                if (isShowUnit && dataFountain.curreEdition == "Rohs") elecol += "(ppm)";//是否显示单位
                                if (isShowCStandard)//是否显示标准
                                {
                                    elecol += "\r\n(" + Info.strStandard + dt_TestResult.Rows[0][elename + "_DeterminantValue"].ToString() + "ppm)";
                                }
                                DataColumn dc3 = dt.Columns.Add(elecol, Type.GetType("System.String"));
                                ElemD.Add(elename, elecol);
                            }
                        }
                        if (layerelename != "") layerelename = layerelename.Substring(0, layerelename.Length - 1) + Info.Thick;
                        DataColumn dc13 = dt.Columns.Add(layerelename, Type.GetType("System.String"));
                        ElemFPThick.Add(elename, layerelename);

                    }

                }
                else
                {
                    foreach (string s in elenameList)
                    {
                        string elecol = s;
                        if (isShowUnit && dataFountain.curreEdition == "Rohs") elecol += "(ppm)";//是否显示单位
                        if (isShowCStandard)//是否显示标准
                        {
                            elecol += "\r\n(" + Info.strStandard + dt_TestResult.Rows[0][s + "_DeterminantValue"].ToString() + "ppm)";
                        }
                        DataColumn dc3 = dt.Columns.Add(elecol, Type.GetType("System.String"));
                        ElemD.Add(s, elecol);
                    }
                }


                //List<string> layernumberList = new List<string>();//当为FPThick时，存储已经选择的层
                //foreach (DataColumn col in dt_TestResult.Columns)
                //{
                //    if (col.ColumnName.IndexOf("_") != -1)
                //    {
                //        string elename = col.ColumnName.Substring(0, col.ColumnName.IndexOf("_"));
                //        if (!ElemD.Keys.Contains(elename) && !ElemFPThick.Keys.Contains(elename+"'"))
                //        {
                //            //如果是FPThick，并且不显示含量，则不添加含量列
                //            if (dataFountain.curreEdition == "FPThick")
                //            {
                //                if (dataFountain.workcCurrent.IsThickShowContent)
                //                {
                //                    string elecol = elename + Info.EditContent;
                //                    if (isShowUnit && dataFountain.curreEdition == "Rohs") elecol += "(ppm)";//是否显示单位
                //                    if (isShowCStandard)//是否显示标准
                //                    {
                //                        elecol += "\r\n(" + Info.strStandard + dt_TestResult.Rows[0][elename + "_DeterminantValue"].ToString() + "ppm)";
                //                    }
                //                    DataColumn dc3 = dt.Columns.Add(elecol, Type.GetType("System.String"));
                //                    ElemD.Add(elename, elecol);
                //                }
                //                //elename += "'";

                //                string elecolFP = "";
                //                CurveElement curveElement = dataFountain.workcCurrent.ElementList.Items.ToList().Find(delegate(CurveElement v) { return v.Caption == elename; });
                //                if (curveElement != null && curveElement.LayerNumber != null && !layernumberList.Contains(curveElement.LayerNumber.ToString()))
                //                {
                //                    layernumberList.Add(curveElement.LayerNumber.ToString());
                //                    List<CurveElement> CurveElementList = dataFountain.workcCurrent.ElementList.Items.ToList().FindAll(delegate(CurveElement v) { return v.LayerNumber == curveElement.LayerNumber; });
                //                    foreach (CurveElement cElement in CurveElementList)
                //                        elecolFP += cElement.Caption + "|";
                //                    if (elecolFP != "") elecolFP = elecolFP.Substring(0, elecolFP.Length - 1) + Info.Thick;

                //                    DataColumn dc13 = dt.Columns.Add(elecolFP, Type.GetType("System.String"));
                //                    ElemFPThick.Add(elename, elecolFP);

                //                }

                                
                //            }
                //            else
                //            {

                //                string elecol = elename;
                //                if (isShowUnit && dataFountain.curreEdition == "Rohs") elecol += "(ppm)";//是否显示单位
                //                if (isShowCStandard)//是否显示标准
                //                {
                //                    elecol += "\r\n(" + Info.strStandard + dt_TestResult.Rows[0][elename + "_DeterminantValue"].ToString() + "ppm)";
                //                }
                //                DataColumn dc3 = dt.Columns.Add(elecol, Type.GetType("System.String"));
                //                ElemD.Add(elename, elecol);
                //            }


                //        }

                //    }
                //}
            }
            else if (Arrangement == "1")
            {
                foreach (DataColumn col in dt_TestResult.Columns)
                {
                    if (col.ColumnName.IndexOf("_") != -1)
                    {
                        string elename = col.ColumnName.Substring(0, col.ColumnName.IndexOf("_"));
                        if (!ElemD.Keys.Contains(elename))
                        {
                            string elecol = elename;
                            if (isShowUnit && dataFountain.curreEdition == "Rohs") elecol += "(ppm)";//是否显示单位
                            if (isShowCStandard)//是否显示标准
                            {
                                elecol += "\r\n("+Info.strStandard+  dt_TestResult.Rows[0][elename + "_DeterminantValue"].ToString() + " ppm)";
                            }
                            ElemD.Add(elename, elecol);
                        }

                    }
                }

                DataColumn dc5 = dt.Columns.Add("ElementName", Type.GetType("System.String"));
                DataColumn dc7 = dt.Columns.Add("ContextValue", Type.GetType("System.String"));
                if (dataFountain.curreEdition == "FPThick")
                {
                    DataColumn dc11 = dt.Columns.Add("ThickValue", Type.GetType("System.String"));
                }
            }

            if (dataFountain.curreEdition == "Rohs") SetOtherColumns(ref dt, ref DOtherMust);


          
            

        }

        private void SetOtherColumns(ref DataTable dt, ref Dictionary<string, string> DOtherMust)
        {
            //其他不必信息，但是存在字段，没有值
            if (OtherNoMust != null)
            {
                for (int i = 0; i < OtherNoMust.Length; i++)
                {
                    if (OtherNoMust[i].ToString() == "") continue;

                    string colname = OtherNoMust[i].ToString();
                    if (colname == "送检日期")
                        colname = Info.strSubmissionDate;
                    else if (colname == "来料批次")
                        colname = Info.strLotNo;


                    DataColumn dc1 = dt.Columns.Add(colname, Type.GetType("System.String"));
                }
            }

            //其他必须信息
            if (OtherMust != null)
            {
                for (int i = 0; i < OtherMust.Length; i++)
                {
                    if (OtherMust[i].ToString() == "") continue;
                    string colname = "";
                    if (OtherMust[i] == "样品名称")
                        colname = Info.SampleName;
                    else if (OtherMust[i] == "工作曲线")
                        colname = Info.WorkingCurve;
                    else if (OtherMust[i] == "强度")
                        colname = Info.Intensity;
                    else if (OtherMust[i] == "误差")
                        colname = Info.strError;
                    else if (OtherMust[i] == "限定标准")
                        colname = Info.strRestrictStandard;
                    else if (OtherMust[i] == "判定")
                        colname = Info.strPassReslt;

                    else if (OtherMust[i] == "设备")
                        colname = Info.Device;
                    else if (OtherMust[i] == "测量日期")
                        colname = Info.SpecDate;
                    else if (OtherMust[i] == "测量时间")
                        colname = Info.MeasureTime;
                    else if (OtherMust[i] == "测量管压")
                        colname = Info.Voltage;
                    else if (OtherMust[i] == "测量管流")
                        colname = Info.Current;
                    else if (OtherMust[i] == "供应商")
                        colname = Info.Supplier;
                    else if (OtherMust[i] == "操作员")
                        colname = Info.Operator;

                    DOtherMust.Add(OtherMust[i], colname);

                    DataColumn dc2 = dt.Columns.Add(colname, Type.GetType("System.String"));
                }
            }

        }





        private static DataRow GetDr(DataTable dt1, DataRow dr, XElement xe, object model, string curreEdition)
        {
            List<object> lo = new List<object>();
            int rowIndex = 0;

            foreach (PropertyInfo p in model.GetType().GetProperties())
            {
                foreach (XElement a in xe.Nodes().ToList())
                {
                    if (curreEdition != "FPThick" && a.Value == "相对标准偏差") continue;
                    if (p.Name.Equals(a.Attribute("id").Value))
                    {
                        object ooo = model.GetType().InvokeMember(p.Name, BindingFlags.GetProperty, null, model, null);
                        if (ooo == null)
                        {
                            ooo = new object();
                            ooo = "";
                        }
                        dr[rowIndex++] = ooo.ToString();
                        break;
                    }
                }
            }
            return dr;
        }
        private static DataRow GetDr(DataTable dt1, DataRow dr, XElement xe, object model, string curreEdition,string NewName,string unit)
        {
            List<object> lo = new List<object>();
            int rowIndex = 0;

            foreach (PropertyInfo p in model.GetType().GetProperties())
            {
                foreach (XElement a in xe.Nodes().ToList())
                {
                    if (curreEdition != "FPThick" && a.Value == "相对标准偏差") continue;
                    if (p.Name.Equals(a.Attribute("id").Value))
                    {
                        object ooo = model.GetType().InvokeMember(p.Name, BindingFlags.GetProperty, null, model, null);
                        if (ooo == null)
                        {
                            ooo = new object();
                            ooo = "";
                        }
                        if (p.Name == "StatColumns")
                        {
                            dr[rowIndex++] = ((ooo.ToString().IndexOf("'") == -1) ? ooo.ToString() : NewName) + unit;
                        }
                        else
                            dr[rowIndex++] = ooo.ToString();
                        break;
                    }
                }
            }
            return dr;
        }

        private static void Do(object model, XElement xe, object obj)
        {
            foreach (var p in model.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (XElement a in xe.Nodes().ToList())
                {
                    if (p.Name.Equals(a.Attribute("id").Value))
                    {
                        object ooo = model.GetType().InvokeMember(p.Name, BindingFlags.GetProperty, null, model, null);
                        if (ooo == null)
                        {
                            ooo = new object();
                        }
                        ClassHelper.SetPropertyValue(obj, p.Name, ooo.ToString());

                        break;
                    }
                }
            }
        }

        public static List<TreeNodeInfo> GetNote(List<TreeNodeInfo> template, List<object> lSource, List<DataTable> lDT , string Name,string text)
        {
            template.Add(new TreeNodeInfo
            {
                Type = CtrlType.ComposeTable,
                contextType = CompositeContextType.Original,
                Name = Name,
                Text = text,
                Tables = lDT,
                ObjSource = lSource
            });
            return template;
        }

        /// <summary>
        /// 表格
        /// </summary>
        /// <returns></returns>
        public static List<TreeNodeInfo> GetNote(List<TreeNodeInfo> template, List<object> lSource, DataTable dt, string Name,string text)
        {
            template.Add(new TreeNodeInfo
            {
                Type = CtrlType.Grid,
                Name = Name,
                Text = text,
                Table = dt,
                ObjSource = lSource
            });
            return template;
        }

    }



    public class TempDt
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable DT { get; set; }
    }

    public class ImageDt
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public PictureBox ImageDT { get; set; }
    }
    #endregion




    #region 备注

    //#region 设置数据源
    //public partial class DataFountain
    //{
    //    /// <summary>
    //    /// 序号
    //    /// </summary>
    //    public int order { get; set; }
    //    /// <summary>
    //    /// 工作曲线
    //    /// </summary>
    //    public WorkCurve workcCurrent { get; set; }

    //    /// <summary>
    //    /// 谱文件
    //    /// </summary>
    //    public SpecList specList { get; set; }

    //    /// <summary>
    //    /// 测量结果
    //    /// </summary>
    //    public List<TestResult> LTestResult { get; set; }

    //    /// <summary>
    //    /// 统计信息
    //    /// </summary>
    //    public List<StatInfo> LStatInfo { get; set; }

    //    /// <summary>
    //    /// 样品数据信息
    //    /// </summary>
    //    public Byte[] SampleImage { get; set; }

    //    /// <summary>
    //    /// 谱图数据信息
    //    /// </summary>
    //    public Byte[] ByteSpecData { get; set; }

    //    public Spec SpecData { get; set; }

    //    /// <summary>
    //    /// 总判定，如果存在标准库，同时勾选了总判定，及其总值，则使用此功能
    //    /// </summary>
    //    public string[,] TotalDeterminant { get; set; }

    //    /// <summary>
    //    /// 当前标准库信息
    //    /// </summary>
    //    public List<StandardData> LStandardData { get; set; }


    //    /// <summary>
    //    /// 历史记录
    //    /// </summary>
    //    public HistoryRecord historyRecord { get; set; }

    //    /// <summary>
    //    /// 历史记录元素
    //    /// </summary>
    //    public List<HistoryElement> historyElementList { get; set; }

    //    /// <summary>
    //    /// 元素信息
    //    /// </summary>
    //    public List<Atom> atomList { get; set; }

    //    /// <summary>
    //    /// 保存连测记录相关信息，如果个数大于1则表明为连测，如果为一个记录，则为非连测情况
    //    /// </summary>
    //    public List<int> ContinuousList { get; set; }


    //}

    ///// <summary>
    ///// 测量结果
    ///// </summary>
    //public class TestResult
    //{
    //    public string Order { get; set; }
    //    public string[,] ElementResult { get; set; }
    //    public TestResult(string Order, string[,] ElementResult)
    //    {
    //        this.Order = Order;
    //        this.ElementResult = ElementResult;
    //    }
    //}

    ///// <summary>
    ///// 统计信息
    ///// </summary>
    //public class StatInfo
    //{
    //    /// <summary>
    //    /// 元素
    //    /// </summary>
    //    public string StatColumns { get; set; }
    //    /// <summary>
    //    /// 最大值
    //    /// </summary>
    //    public string MaxValue { get; set; }
    //    /// <summary>
    //    /// 最小值
    //    /// </summary>
    //    public string MinValue { get; set; }
    //    /// <summary>
    //    /// 平均值
    //    /// </summary>
    //    public string MeanValue { get; set; }
    //    /// <summary>
    //    /// 标准偏差
    //    /// </summary>
    //    public string SDValue { get; set; }

    //    /// <summary>
    //    /// 相对标准偏差
    //    /// </summary>
    //    public string RSDValue { get; set; }
    //    public StatInfo(string StatColumns, string MaxValue, string MinValue, string MeanValue, string SDValue, string RSDValue)
    //    {
    //        this.StatColumns = StatColumns;
    //        this.MaxValue = MaxValue;
    //        this.MinValue = MinValue;
    //        this.MeanValue = MeanValue;
    //        this.SDValue = SDValue;
    //        this.RSDValue = RSDValue;
    //    }
    //}



    //#endregion

    //#region 报表需要的对象

    ///// <summary>
    ///// 初始化参数
    ///// </summary>
    //public class PrintInitParam
    //{
    //    /// <summary>
    //    /// 管压
    //    /// </summary>
    //    public int TubVoltage { get; set; }//<初始化电压

    //    /// <summary>
    //    /// 管流
    //    /// </summary>
    //    public int TubCurrent { get; set; }//初始化电流、

    //    /// <summary>
    //    /// 初始化元素
    //    /// </summary>
    //    public string ElemName { get; set; }//<用来初始化的元素

    //    /// <summary>
    //    /// 粗  调  码
    //    /// </summary>
    //    public float Gain { get; set; }    //<粗调码

    //    /// <summary>
    //    /// 精  调  码
    //    /// </summary>
    //    public float FineGain { get; set; } //<细调码

    //    /// <summary>
    //    /// 实际粗调码
    //    /// </summary>
    //    public float ActGain { get; set; }

    //    /// <summary>
    //    /// 实际细调码
    //    /// </summary>
    //    public float ActFineGain { get; set; }

    //    /// <summary>
    //    /// 初始化通道
    //    /// </summary>
    //    public int Channel { get; set; }   //<初始化通道

    //    /// <summary>
    //    /// 滤  光  片
    //    /// </summary>
    //    public int Filter { get; set; }    //<滤光片

    //    /// <summary>
    //    /// 准  直  器
    //    /// </summary>
    //    public int Collimator { get; set; }  //准直器

    //    /// <summary>
    //    /// 误  差  道
    //    /// </summary>
    //    public int ChannelError { get; set; }//<初始化误差道

    //    public PrintInitParam(int TubVoltage, int TubCurrent, string ElemName,
    //       float Gain, float FineGain, float ActGain, float ActFineGain
    //        , int Channel, int Filter, int Collimator, int ChannelError)
    //    {
    //        this.TubVoltage = TubVoltage;
    //        this.TubCurrent = TubCurrent;
    //        this.ElemName = ElemName;
    //        this.Gain = Gain;
    //        this.FineGain = FineGain;
    //        this.ActGain = ActGain;
    //        this.ActFineGain = ActFineGain;
    //        this.Channel = Channel;
    //        this.Filter = Filter;
    //        this.Collimator = Collimator;
    //        this.ChannelError = ChannelError;
    //    }
    //}

    ///// <summary>
    ///// 测量条件
    ///// </summary>
    //public class PrintDeviceParameter
    //{
    //    /// <summary>
    //    /// 所属测量条件名称
    //    /// </summary>
    //    public string DeviceParameterName { get; set; }
    //    /// <summary>
    //    /// 测量时间
    //    /// </summary>
    //    public int PrecTime { get; set; }

    //    /// <summary>
    //    /// 管流
    //    /// </summary>
    //    public int TubCurrent { get; set; }

    //    /// <summary>
    //    /// 管压
    //    /// </summary>
    //    public int TubVoltage { get; set; }

    //    /// <summary>
    //    /// 滤光片
    //    /// </summary>
    //    public int FilterIdx { get; set; }

    //    /// <summary>
    //    /// 准直器
    //    /// </summary>
    //    public int CollimatorIdx { get; set; }

    //    /// <summary>
    //    /// 是否抽真空
    //    /// </summary>
    //    public bool IsVacuum { get; set; }

    //    /// <summary>
    //    /// 抽真空时间
    //    /// </summary>
    //    public int VacuumTime { get; set; }

    //    /// <summary>
    //    /// 真空度抽真空
    //    /// </summary>
    //    public bool IsVacuumDegree { get; set; }

    //    /// <summary>
    //    /// 真空度
    //    /// </summary>
    //    public double VacuumDegree { get; set; }
    //    /// <summary>
    //    /// 是否调节计数
    //    /// </summary>
    //    public bool IsAdjustRate { get; set; }
    //    /// <summary>
    //    /// 最小計數率
    //    /// </summary>
    //    public double MinRate { get; set; }
    //    /// <summary>
    //    /// 最大計數率
    //    /// </summary>
    //    public double MaxRate { get; set; }

    //    public int BeginChann { get; set; }
    //    public int EndChann { get; set; }

    //    /// <summary>
    //    /// 是否干扰报警
    //    /// </summary>
    //    public bool IsDistrubAlert { get; set; }

    //    /// <summary>
    //    /// 是否判断峰飘
    //    /// </summary>
    //    public bool IsPeakFloat { get; set; }

    //    /// <summary>
    //    /// 峰飘左界
    //    /// </summary>
    //    public int PeakFloatLeft { get; set; }

    //    /// <summary>
    //    /// 峰飘右界
    //    /// </summary>
    //    public int PeakFloatRight { get; set; }

    //    /// <summary>
    //    /// 峰道址
    //    /// </summary>
    //    public int PeakFloatChannel { get; set; }

    //    /// <summary>
    //    /// 误差
    //    /// </summary>
    //    public int PeakFloatError { get; set; }
    //    public int PeakCheckTime { get; set; }

    //    public PrintDeviceParameter(
    //        string DeviceParameterName,
    //        int PrecTime,
    //        int TubCurrent,
    //        int TubVoltage,
    //        int FilterIdx,
    //        int CollimatorIdx,
    //        bool IsVacuum,
    //        int VacuumTime,
    //        bool IsVacuumDegree,
    //        double VacuumDegree,
    //        bool IsAdjustRate,
    //        double MinRate,
    //        double MaxRate,
    //        int BeginChann,
    //        int EndChann,
    //        bool IsDistrubAlert,
    //        bool IsPeakFloat,
    //        int PeakFloatLeft,
    //        int PeakFloatRight,
    //        int PeakFloatChannel,
    //        int PeakFloatError,
    //        int PeakCheckTime
    //    )
    //    {
    //        this.DeviceParameterName = DeviceParameterName;
    //        this.PrecTime = PrecTime;
    //        this.TubCurrent = TubCurrent;
    //        this.TubVoltage = TubVoltage;
    //        this.FilterIdx = FilterIdx;
    //        this.CollimatorIdx = CollimatorIdx;
    //        this.IsVacuum = IsVacuum;
    //        this.VacuumTime = VacuumTime;
    //        this.IsVacuumDegree = IsVacuumDegree;
    //        this.VacuumDegree = VacuumDegree;
    //        this.IsAdjustRate = IsAdjustRate;
    //        this.MinRate = MinRate;
    //        this.MaxRate = MaxRate;
    //        this.BeginChann = BeginChann;
    //        this.EndChann = EndChann;
    //        this.IsDistrubAlert = IsDistrubAlert;
    //        this.IsPeakFloat = IsPeakFloat;
    //        this.PeakFloatLeft = PeakFloatLeft;
    //        this.PeakFloatRight = PeakFloatRight;
    //        this.PeakFloatChannel = PeakFloatChannel;
    //        this.PeakFloatError = PeakFloatError;
    //        this.PeakCheckTime = PeakCheckTime;
    //    }
    //}


    ///// <summary>
    ///// 工作曲线
    ///// </summary>
    //public class PrintWorkCurve
    //{
    //    /// <summary>
    //    /// 曲线名称
    //    /// </summary>
    //    public string WorkCurveName { get; set; }

    //    /// <summary>
    //    /// 测量名称
    //    /// </summary>
    //    public string ConditionName { get; set; }

    //    ///// <summary>
    //    ///// 扫谱时间
    //    ///// </summary>
    //    //private double SpecTime { get; set; }
    //    ///// <summary>
    //    ///// 所用时间
    //    ///// </summary>
    //    //private int UsedTime { get; set; }
    //    ///// <summary>
    //    ///// 管压
    //    ///// </summary>
    //    //private int TubVoltage { get; set; }
    //    ///// <summary>
    //    ///// 管流
    //    ///// </summary>
    //    //private int TubCurrent { get; set; }


    //    //public PrintWorkCurve(string WorkCurveName, string ConditionName, double SpecTime, int UsedTime, int TubVoltage, int TubCurrent) {
    //    //    this.WorkCurveName = WorkCurveName;
    //    //    this.ConditionName = ConditionName;
    //    //    this.SpecTime = SpecTime;
    //    //    this.UsedTime = UsedTime;
    //    //    this.TubVoltage = TubVoltage;
    //    //    this.TubCurrent = TubCurrent;
    //    //}

    //    public PrintWorkCurve(string WorkCurveName, string ConditionName)
    //    {
    //        this.WorkCurveName = WorkCurveName;
    //        this.ConditionName = ConditionName;
    //    }
    //}

    ///// <summary>
    ///// 样品信息
    ///// </summary>
    //public class PrintSample
    //{
    //    /// <summary>
    //    /// 样品名称
    //    /// </summary>
    //    public string SampleName { get; set; }

    //    /// <summary>
    //    /// 供应商
    //    /// </summary>
    //    public string Supplier { get; set; }

    //    /// <summary>
    //    /// 重量
    //    /// </summary>
    //    public double? Weight { get; set; }

    //    /// <summary>
    //    /// 形狀
    //    /// </summary>
    //    public string Shape { get; set; }

    //    /// <summary>
    //    /// 操作員
    //    /// </summary>
    //    public string Operater { get; set; }

    //    /// <summary>
    //    /// 扫谱日期
    //    /// </summary>
    //    public DateTime? SpecDate { get; set; }

    //    /// <summary>
    //    /// 描述信息
    //    /// </summary>
    //    private string SpecSummary { get; set; }

    //    public PrintSample(string SampleName, string Supplier, double? Weight,
    //        string Shape, string Operater, DateTime? SpecDate, string SpecSummary)
    //    {
    //        this.SampleName = SampleName;
    //        this.Supplier = Supplier;
    //        this.Weight = Weight;
    //        this.Operater = Operater;
    //        this.SpecDate = SpecDate;
    //        this.SpecSummary = SpecSummary;
    //    }
    //}


    ///// <summary>
    ///// 样品图信息
    ///// </summary>
    //public class PrintSampleImage
    //{
    //    private Bitmap SampleImage { get; set; }
    //    public PrintSampleImage(Bitmap SampleImage)
    //    { this.SampleImage = SampleImage; }
    //}


    ///// <summary>
    ///// 谱图信息
    ///// </summary>
    //public class PrintSpecImage
    //{
    //    public Bitmap SpecImage { get; set; }

    //    public Dictionary<string, Bitmap> ElementSpecImage { get; set; }
    //    public PrintSpecImage(Bitmap SpecImage, Dictionary<string, Bitmap> ElementSpecImage)
    //    {
    //        this.SpecImage = SpecImage;
    //        this.ElementSpecImage = ElementSpecImage;
    //    }
    //}

    //public class BaseInfoCls
    //{
    //    /// <summary>
    //    /// 样品名称
    //    /// </summary>
    //    public string SimpleName { get; set; }
    //    /// <summary>
    //    /// 测试时间
    //    /// </summary>
    //    public string TestTime { get; set; }
    //    /// <summary>
    //    /// 供应商
    //    /// </summary>
    //    public string Supplier { get; set; }
    //    /// <summary>
    //    /// 管压
    //    /// </summary>
    //    public string Voltage { get; set; }
    //    /// <summary>
    //    /// 操作员
    //    /// </summary>
    //    public string Operator { get; set; }
    //    /// <summary>
    //    /// 管流
    //    /// </summary>
    //    public string Current { get; set; }
    //    /// <summary>
    //    /// 测量日期
    //    /// </summary>
    //    public string TestDate { get; set; }
    //    /// <summary>
    //    /// 工作曲线
    //    /// </summary>
    //    public string WorkCurve { get; set; }
    //    /// <summary>
    //    /// 批号
    //    /// </summary>
    //    public string LotNo { get; set; }
    //    /// <summary>
    //    /// 仪器型号
    //    /// </summary>
    //    public string Mode { get; set; }
    //    /// <summary>
    //    /// 待检单号
    //    /// </summary>
    //    public string PendNo { get; set; }
    //    /// <summary>
    //    /// 送测单位
    //    /// </summary>
    //    public string SubmittedUnit { get; set; }
    //    /// <summary>
    //    /// 编号 
    //    /// </summary>
    //    public string Number { get; set; }
    //    /// <summary>
    //    /// 准直器
    //    /// </summary>
    //    public string CollimatorIdx { get; set; }
    //    /// <summary>
    //    /// 滤光片
    //    /// </summary>
    //    public string FilterIdx { get; set; }
    //}

    ///// <summary>
    ///// 总判定
    ///// </summary>
    //public class TotalDeterminant
    //{
    //    public string TotalDeterminant1 { get; set; }
    //}


    //#endregion

    //#region 获取数据
    ///// <summary>
    ///// 获取数据源
    ///// </summary>
    //public class GetDataFountain
    //{
    //    private List<DataFountain> dataFountainList;
    //    private DataFountain dataFountain = null;
    //    Skyray.Language.LanguageModel languageModel;
    //    public GetDataFountain(List<DataFountain> dataFountainList)
    //    {
    //        if (languageModel == null)
    //        {
    //            languageModel = new Skyray.Language.LanguageModel();
    //        }
    //        this.dataFountainList = dataFountainList;
    //        dataFountain = dataFountainList[0];
    //    }

    //    public List<TreeNodeInfo> GetSource()
    //    {
    //        //初始化参数
    //        Condition condition = dataFountain.workcCurrent.Condition;

    //        PrintInitParam printInitParam = new PrintInitParam(dataFountain.specList.Specs[0].DeviceParameter.TubVoltage
    //           , dataFountain.specList.Specs[0].DeviceParameter.TubCurrent, condition.InitParam.ElemName
    //           , condition.InitParam.Gain, condition.InitParam.FineGain
    //           , condition.InitParam.ActGain, condition.InitParam.ActFineGain
    //           , condition.InitParam.Channel, condition.InitParam.Filter
    //           , condition.InitParam.Collimator, condition.InitParam.ChannelError);
    //        //工作曲线
    //        WorkCurve workcCurrent = dataFountain.workcCurrent;
    //        PrintWorkCurve printWorkCurve = new PrintWorkCurve(workcCurrent.Name, workcCurrent.ConditionName);

    //        //样品信息
    //        SpecList specList = dataFountain.specList;
    //        PrintSample printSample = new PrintSample(specList.SampleName, specList.Supplier
    //            , specList.Weight, specList.Shape, specList.Operater, specList.SpecDate, specList.SpecSummary);

    //        //样品图数据信息
    //        PrintSampleImage printSampleImage = new PrintSampleImage(GetSampleImage(dataFountain.SampleImage));


    //        //谱图数据信息
    //        Bitmap SpecImage = null;
    //        if (dataFountain.SpecData != null && dataFountain.ByteSpecData == null)
    //        {
    //            SpecImage = GetSpecImage(dataFountain.SpecData);
    //        }
    //        else SpecImage = GetSampleImage(dataFountain.ByteSpecData);


    //        //获取谱图元素数据信息
    //        Dictionary<string, Bitmap> ElementSpecImage = new Dictionary<string, Bitmap>();
    //        PrintSpecImage printSpecImage = new PrintSpecImage(SpecImage, ElementSpecImage);

    //        //测量次数
    //        List<TestResult> lTestResult = dataFountainList[0].LTestResult;

    //        //测量条件信息
    //        List<PrintDeviceParameter> LprintDeviceParameter = new List<PrintDeviceParameter>();
    //        foreach (DeviceParameter deviceParameter in condition.DeviceParamList)
    //        {
    //            PrintDeviceParameter printDeviceParameter =
    //                new PrintDeviceParameter(deviceParameter.Name, deviceParameter.PrecTime, deviceParameter.TubCurrent
    //                    , deviceParameter.TubVoltage, deviceParameter.FilterIdx, deviceParameter.CollimatorIdx
    //                    , deviceParameter.IsVacuum, deviceParameter.VacuumTime, deviceParameter.IsVacuumDegree
    //                    , deviceParameter.VacuumDegree, deviceParameter.IsAdjustRate, deviceParameter.MinRate
    //                    , deviceParameter.MaxRate, deviceParameter.BeginChann, deviceParameter.EndChann
    //                    , deviceParameter.IsDistrubAlert, deviceParameter.IsPeakFloat, deviceParameter.PeakFloatLeft
    //                    , deviceParameter.PeakFloatRight, deviceParameter.PeakFloatChannel, deviceParameter.PeakFloatError
    //                    , deviceParameter.PeakCheckTime);
    //            LprintDeviceParameter.Add(printDeviceParameter);
    //        }


    //        BaseInfoCls baseInfocls = new BaseInfoCls();
    //        baseInfocls.SimpleName = specList.SampleName;
    //        baseInfocls.TestTime = LprintDeviceParameter[0].PrecTime.ToString();
    //        baseInfocls.Supplier = printSample.Supplier;
    //        baseInfocls.Voltage = printInitParam.TubVoltage.ToString();
    //        baseInfocls.Operator = printSample.Operater;
    //        baseInfocls.Current = printInitParam.TubCurrent.ToString();
    //        baseInfocls.TestDate = printSample.SpecDate.ToString();
    //        baseInfocls.WorkCurve = workcCurrent.Name;
    //        baseInfocls.LotNo = "";
    //        baseInfocls.Mode = "";
    //        baseInfocls.PendNo = "";
    //        baseInfocls.SubmittedUnit = "";
    //        baseInfocls.Number = "";
    //        if (LprintDeviceParameter.Count > 0)
    //        {
    //            baseInfocls.CollimatorIdx = LprintDeviceParameter[0].CollimatorIdx.ToString();
    //            baseInfocls.FilterIdx = LprintDeviceParameter[0].FilterIdx.ToString();
    //        }

    //        foreach (CurveElement curveElement in dataFountain.workcCurrent.ElementList.Items)
    //        {
    //            Bitmap bmp = new Bitmap(120, 60);
    //            Graphics g = Graphics.FromImage(bmp);
    //            //int width = (int)Math.Round(Convert.ToInt32(bmp.Width) * g.DpiX / 72.0);
    //            //int height = (int)Math.Round(Convert.ToInt32(bmp.Height) * g.DpiY / 72.0);

    //            int width = 400;
    //            int height = 200;

    //            bmp = new Bitmap(width, height);

    //            Report report = new Report();
    //            report.Spec = dataFountain.specList.Specs[0];
    //            //report.operateMember = FrmLogon.userName;
    //            report.Elements = dataFountain.workcCurrent.ElementList;
    //            report.WorkCurveName = dataFountain.workcCurrent.Name;
    //            //report.FirstContIntr.Add(elementList.Items.Count);
    //            report.InterestElemCount = dataFountain.workcCurrent.ElementList.Items.Count;
    //            report.DrawInterstringElems(ref bmp);
    //            //DrawInterstringElems(ref bmp, (dataFountain.SpecData == null) ? dataFountain.specList.Specs[0] : dataFountain.SpecData, curveElement);
    //            ElementSpecImage.Add(curveElement.Caption, bmp);
    //        }



    //        XmlDocument xml = new XmlDocument();
    //        xml.Load(Application.StartupPath + "\\AppParams.xml");
    //        if (xml.SelectSingleNode("application/Excel/Company").FirstChild.Value == "1")
    //        {
    //            return GetOther(dataFountainList, printInitParam, printWorkCurve, printSample, SpecImage, printSpecImage, lTestResult, LprintDeviceParameter, baseInfocls);
    //        }
    //        else
    //        {
    //            return GetZhongDa(dataFountainList, printInitParam, printWorkCurve, printSample, SpecImage, printSpecImage, lTestResult, LprintDeviceParameter, baseInfocls);
    //        }
    //    }


    //    private List<TreeNodeInfo> GetOther(List<DataFountain> dataFountainList, PrintInitParam printInitParam
    //        , PrintWorkCurve printWorkCurve, PrintSample printSample, Bitmap SpecImage, PrintSpecImage printSpecImage
    //        , List<TestResult> lTestResult, List<PrintDeviceParameter> LprintDeviceParameter, BaseInfoCls baseInfocls)
    //    {
    //        var template = new List<TreeNodeInfo>();
    //        XElement xele = XElement.Load(Application.StartupPath + "\\printxml\\data.xml");

    //        XElement xeLabel = XElement.Load(Application.StartupPath + "\\printxml\\label.xml");

    //        var label = xeLabel.Elements("Label").ToList();
    //        foreach (XElement xe in label)
    //        {
    //            template.Add(new TreeNodeInfo
    //            {
    //                Type = CtrlType.Label,
    //                Name = xe.Element("Name").Value,
    //                Text = xe.Element("Text").Value,
    //                Caption = xe.Element("Caption").Value
    //            });
    //        }

    //        #region 初始化参数
    //        var lbl = xele.Elements("Label").ToList();
    //        foreach (XElement xe in lbl)
    //        {
    //            List<object> lSouce = new List<object>();
    //            List<DataTable> lDT = new List<DataTable>();
    //            #region
    //            DataTable dt = new DataTable();
    //            dt.Columns.Add("a");
    //            DataRow dr;
    //            dr = dt.NewRow();
    //            dr[0] = "11111111";
    //            dt.Rows.Add(dr);
    //            #endregion

    //            if (xe.Attribute("id").Value == "基本信息")
    //            {
    //                Type t = ClassHelper.BuildType("TempCls");
    //                List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
    //                ClassHelper.CustPropertyInfo cpi;
    //                foreach (XElement a in xe.Nodes().ToList())
    //                {
    //                    cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
    //                    lcpi.Add(cpi);
    //                }
    //                t = ClassHelper.AddProperty(t, lcpi);
    //                object obj = ClassHelper.CreateInstance(t);
    //                BaseInfoCls model = baseInfocls;
    //                Do(model, xe, obj);
    //                lDT.Add(dt);
    //                lSouce.Add(obj);
    //                GetNote(template, lSouce, lDT, "基本信息");
    //            }
    //            else if (xe.Attribute("id").Value == "总判定")
    //            {
    //                Type t = ClassHelper.BuildType("TempCls");
    //                List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
    //                ClassHelper.CustPropertyInfo cpi;
    //                foreach (XElement a in xe.Nodes().ToList())
    //                {
    //                    cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
    //                    lcpi.Add(cpi);
    //                }
    //                t = ClassHelper.AddProperty(t, lcpi);
    //                object obj = ClassHelper.CreateInstance(t);
    //                TotalDeterminant model = new TotalDeterminant();
    //                model.TotalDeterminant1 = dataFountainList[0].TotalDeterminant[0, 0] + "~" + dataFountainList[0].TotalDeterminant[0, 1];
    //                Do(model, xe, obj);
    //                lDT.Add(dt);
    //                lSouce.Add(obj);
    //                GetNote(template, lSouce, lDT, "总判定");
    //            }
    //        }

    //        #endregion

    //        #region 复合表测量结果和统计信息
    //        var table = xele.Elements("Table").ToList();
    //        foreach (XElement xe in table)
    //        {
    //            List<DataTable> lDT = new List<DataTable>();
    //            List<object> lSouce = new List<object>();
    //            var DT = new TempDt();
    //            DataTable dt1 = new DataTable();

    //            if (xe.Attribute("id").Value == "测试结果")
    //            {
    //                // if (dataFountainList.Count == 1)
    //                {
    //                    #region
    //                    //if (dataFountainList[0].LTestResult.Count == 1)
    //                    //{
    //                    //    #region
    //                    //    string strUnit = "";
    //                    //    string strElement = "";

    //                    //    int rowsIndex = 0;
    //                    //    TestResult ldt1 = lTestResult[0];

    //                    //    dt1.Columns.Add("元素");
    //                    //    dt1.Columns.Add("A");
    //                    //    dt1.Columns.Add("B");
    //                    //    dt1.Columns.Add("C");
    //                    //    dt1.Columns.Add("D");

    //                    //    if (ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] == "False" || ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] == "Pass")
    //                    //    {
    //                    //        dt1.Columns.Add(ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 0]);
    //                    //        rowsIndex = ldt1.ElementResult.Length / 6 - 1;
    //                    //    }
    //                    //    else
    //                    //    {
    //                    //        rowsIndex = ldt1.ElementResult.Length / 6;
    //                    //    }
    //                    //    DataRow dr;
    //                    //    for (int i = 0; i < rowsIndex; i++)
    //                    //    {
    //                    //        dr = dt1.NewRow();

    //                    //        strUnit = dataFountainList[0].LTestResult[0].ElementResult[i, 0].Substring(dataFountainList[0].LTestResult[0].ElementResult[i, 0].IndexOf("(") + 1);
    //                    //        strElement = dataFountainList[0].LTestResult[0].ElementResult[i, 0].Substring(0, dataFountainList[0].LTestResult[0].ElementResult[i, 0].IndexOf("("));
    //                    //        IEnumerable<StandardData> cc = from c in dataFountain.LStandardData
    //                    //                                       where c.ElementCaption == strElement
    //                    //                                       select c;
    //                    //        if (cc.Count() > 0)
    //                    //        {
    //                    //            dr[0] = strElement + "(标准" + cc.Single().StandardContent + " " + strUnit;
    //                    //        }
    //                    //        else
    //                    //        {
    //                    //            dr[0] = strElement + "(标准" + strUnit;
    //                    //        }
    //                    //        dr[1] = ldt1.ElementResult[i, 3] + "~" + ldt1.ElementResult[i, 2];
    //                    //        //dr[2] = ldt1.ElementResult[i, 2];
    //                    //        dr[2] = ldt1.ElementResult[i, 1];
    //                    //        dr[3] = ldt1.ElementResult[i, 4];
    //                    //        dr[4] = ldt1.ElementResult[i, 5];

    //                    //        if (ldt1.ElementResult[i, 2] == null)
    //                    //        {
    //                    //            dr[5] = "Pass~Green";
    //                    //        }
    //                    //        else
    //                    //        {
    //                    //            dr[5] = "False~Red";
    //                    //        }
    //                    //        //if (ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] == "False" || ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] == "Pass")
    //                    //        //{
    //                    //        //    dr[5] = ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] + "~" + ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 2];
    //                    //        //}
    //                    //        dt1.Rows.Add(dr);
    //                    //    }
    //                    //    DT.DT = dt1;
    //                    //    lSouce.Add(DT);
    //                    //    lDT.Add(dt1);
    //                    //    GetNote(template, lSouce, lDT, "测试结果");
    //                    //    #endregion
    //                    //}
    //                    //else
    //                    //{
    //                    //    #region
    //                    //    string strUnit = "";
    //                    //    string strElement = "";
    //                    //    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6; i++)
    //                    //    {
    //                    //        if (dataFountainList[0].LTestResult[0].ElementResult[i, 1] == "False" || dataFountainList[0].LTestResult[0].ElementResult[i, 1] == "Pass")
    //                    //        {
    //                    //            dt1.Columns.Add(dataFountainList[0].LTestResult[0].ElementResult[i, 0]);
    //                    //        }
    //                    //        else
    //                    //        {
    //                    //            strUnit = dataFountainList[0].LTestResult[0].ElementResult[i, 0].Substring(dataFountainList[0].LTestResult[0].ElementResult[i, 0].IndexOf("(") + 1);
    //                    //            strElement = dataFountainList[0].LTestResult[0].ElementResult[i, 0].Substring(0, dataFountainList[0].LTestResult[0].ElementResult[i, 0].IndexOf("("));
    //                    //            IEnumerable<StandardData> cc = from c in dataFountain.LStandardData
    //                    //                                           where c.ElementCaption == strElement
    //                    //                                           select c;
    //                    //            if (cc.Count() > 0)
    //                    //            {
    //                    //                dt1.Columns.Add(strElement + "(标准" + cc.Single().StandardContent + " " + strUnit);
    //                    //            }
    //                    //            else
    //                    //            {
    //                    //                dt1.Columns.Add(strElement + "(标准" + strUnit);
    //                    //            }
    //                    //        }
    //                    //    }
    //                    //    DataRow dr;

    //                    //    for (int i = 1; i <= dataFountainList[0].LTestResult.Count; i++)
    //                    //    {
    //                    //        dr = dt1.NewRow();
    //                    //        for (int j = 0; j < dataFountainList[0].LTestResult[0].ElementResult.Length / 6; j++)
    //                    //        {
    //                    //            if (dataFountainList[0].LTestResult[i - 1].ElementResult[j, 1].ToString() == "False" ||
    //                    //                dataFountainList[0].LTestResult[i - 1].ElementResult[j, 1].ToString() == "Pass")
    //                    //            {
    //                    //                dr[j] = dataFountainList[0].LTestResult[i - 1].ElementResult[j, 1];
    //                    //            }
    //                    //            else
    //                    //            {
    //                    //                dr[j] = dataFountainList[0].LTestResult[i - 1].ElementResult[j, 1];
    //                    //            }
    //                    //        }
    //                    //        dt1.Rows.Add(dr);
    //                    //    }
    //                    //    DT.DT = dt1;
    //                    //    lSouce.Add(DT);
    //                    //    lDT.Add(dt1);
    //                    //    GetNote(template, lSouce, lDT[0], "测试结果");
    //                    //    #endregion
    //                    //}
    //                    #endregion

    //                }
    //                //else
    //                {
    //                    #region
    //                    string strUnit = "";
    //                    string strElement = "";
    //                    dt1.Columns.Add(Skyray.EDX.Common.Info.strSimpleName);
    //                    dt1.Columns.Add(Skyray.EDX.Common.Info.strSubmissionDate);
    //                    dt1.Columns.Add(Skyray.EDX.Common.Info.strLotNo);

    //                    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6; i++)
    //                    {
    //                        if (dataFountainList[0].LTestResult[0].ElementResult[i, 1] == "False" || dataFountainList[0].LTestResult[0].ElementResult[i, 1] == "Pass")
    //                        {
    //                            dt1.Columns.Add(Skyray.EDX.Common.Info.strWorkCurve);
    //                            dt1.Columns.Add(dataFountainList[0].LTestResult[0].ElementResult[i, 0]);
    //                        }
    //                        else
    //                        {
    //                            strUnit = dataFountainList[0].LTestResult[0].ElementResult[i, 0].Substring(dataFountainList[0].LTestResult[0].ElementResult[i, 0].IndexOf("(") + 1);
    //                            strElement = dataFountainList[0].LTestResult[0].ElementResult[i, 0].Substring(0, dataFountainList[0].LTestResult[0].ElementResult[i, 0].IndexOf("("));
    //                            IEnumerable<StandardData> cc = from c in dataFountain.LStandardData
    //                                                           where c.ElementCaption == strElement
    //                                                           select c;
    //                            if (cc.Count() > 0)
    //                            {
    //                                dt1.Columns.Add(strElement + "(" + Skyray.EDX.Common.Info.strStandard + cc.Single().StandardContent + " " + strUnit);
    //                            }
    //                            else
    //                            {
    //                                dt1.Columns.Add(strElement + "(" + Skyray.EDX.Common.Info.strStandard + strUnit);
    //                            }
    //                        }
    //                    }

    //                    DataRow dr;
    //                    #region 复合表格时使用
    //                    //dr = dt1.NewRow();
    //                    //dr[0] = "样品名称";
    //                    //dr[1] = "送检日期";
    //                    //dr[2] = "来料批次";
    //                    //for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6; i++)
    //                    //{
    //                    //    if (dataFountainList[0].LTestResult[0].ElementResult[i, 1] == "False" || dataFountainList[0].LTestResult[0].ElementResult[i, 1] == "Pass")
    //                    //    {
    //                    //        dr[3 + i] = "工作曲线";
    //                    //        dr[3 + i+1] = dataFountainList[0].LTestResult[0].ElementResult[i, 0];
    //                    //    }
    //                    //    else
    //                    //    {
    //                    //        strUnit = dataFountainList[0].LTestResult[0].ElementResult[i, 0].Substring(dataFountainList[0].LTestResult[0].ElementResult[i, 0].IndexOf("(") + 1);
    //                    //        strElement = dataFountainList[0].LTestResult[0].ElementResult[i, 0].Substring(0, dataFountainList[0].LTestResult[0].ElementResult[i, 0].IndexOf("("));
    //                    //        IEnumerable<StandardData> cc = from c in dataFountain.LStandardData
    //                    //                                       where c.ElementCaption == strElement
    //                    //                                       select c;
    //                    //        if (cc.Count() > 0)
    //                    //        {
    //                    //            dr[3+i]=strElement + "(标准" + cc.Single().StandardContent + " " + strUnit;
    //                    //        }
    //                    //        else
    //                    //        {
    //                    //            dr[3+i]=strElement + "(标准" + strUnit;
    //                    //        }
    //                    //    }
    //                    //}
    //                    //dt1.Rows.Add(dr);
    //                    #endregion
    //                    for (int i = 0; i < dataFountainList.Count; i++)
    //                    {
    //                        dr = dt1.NewRow();
    //                        dr[0] = dataFountainList[i].historyRecord.SampleName;
    //                        dr[1] = "";
    //                        dr[2] = "";
    //                        for (int j = 3; j < (dataFountainList[i].LTestResult[0].ElementResult.Length / 6) + 3; j++)
    //                        {
    //                            if (dataFountainList[i].LTestResult[0].ElementResult[j - 3, 1].ToString() == "False" ||
    //                                dataFountainList[i].LTestResult[0].ElementResult[j - 3, 1].ToString() == "Pass")
    //                            {
    //                                dr[j] = dataFountainList[i].workcCurrent.Name;
    //                                dr[j + 1] = dataFountainList[i].LTestResult[0].ElementResult[j - 3, 1];
    //                            }
    //                            else
    //                            {
    //                                dr[j] = dataFountainList[i].LTestResult[0].ElementResult[j - 3, 1];
    //                            }
    //                        }
    //                        dt1.Rows.Add(dr);
    //                    }
    //                    DT.DT = dt1;
    //                    lSouce.Add(DT);
    //                    lDT.Add(dt1);
    //                    GetNote(template, lSouce, lDT[0], "测试结果");
    //                    #endregion
    //                }
    //            }
    //            else if (xe.Attribute("id").Value == "统计信息")
    //            {
    //                foreach (XElement a in xe.Nodes().ToList())
    //                {
    //                    dt1.Columns.Add(a.Value);
    //                }
    //                List<StatInfo> ldt1 = dataFountain.LStatInfo;
    //                DataRow dr;
    //                foreach (StatInfo model in ldt1)
    //                {
    //                    dr = dt1.NewRow();
    //                    dt1.Rows.Add(GetDr(dt1, dr, xe, model));
    //                }
    //                DT.DT = dt1;
    //                lSouce.Add(DT);
    //                lDT.Add(dt1);
    //                GetNote(template, lSouce, lDT, "统计信息");
    //            }

    //        }
    //        #endregion

    //        #region 谱图
    //        var image = xele.Elements("Image").ToList();
    //        foreach (XElement xe in image)
    //        {
    //            List<object> lSouce = new List<object>();
    //            List<DataTable> lDT = new List<DataTable>();
    //            DataTable dt = new DataTable();
    //            dt.Columns.Add("a");
    //            if (xe.Attribute("id").Value == "谱图")
    //            {
    //                Image imagee = null;
    //                //if (dataFountain.ByteSpecData == null)
    //                //    imagee = GetSpecImage(dataFountain.SpecData);
    //                //else
    //                imagee = SpecImage;//GetSampleImage(dataFountain.ByteSpecData);
    //                template.Add(new TreeNodeInfo
    //                {
    //                    Type = CtrlType.Image,
    //                    Name = "谱图",
    //                    Text = "",
    //                    Image = imagee
    //                });
    //            }
    //            else if (xe.Attribute("id").Value == "样品图")
    //            {
    //                template.Add(new TreeNodeInfo
    //                {
    //                    Type = CtrlType.Image,
    //                    Name = "样品图",
    //                    Text = "",
    //                    Image = (GetSampleImage(dataFountain.SampleImage) == null) ? null : GetSampleImage(dataFountain.SampleImage)
    //                });
    //            }
    //            else if (xe.Attribute("id").Value == "元素图")
    //            {
    //                Dictionary<string, Bitmap> d = printSpecImage.ElementSpecImage;
    //                foreach (KeyValuePair<string, Bitmap> a in d)
    //                {
    //                    template.Add(new TreeNodeInfo
    //                    {
    //                        Type = CtrlType.Image,
    //                        Name = "元素" + a.Key,
    //                        Text = "元素" + a.Key,
    //                        Image = a.Value
    //                    });
    //                }

    //            }
    //        }

    //        #endregion
    //        return template;
    //    }



    //    #region 画图

    //    ///<summary>
    //    /// 获取谱图
    //    /// </summary>
    //    /// <param name="Image"></param>
    //    /// <returns></returns>
    //    public static Bitmap GetSpecImage(Spec tempSpec)
    //    {
    //        Bitmap bmp = new Bitmap(120, 60);
    //        Graphics g = Graphics.FromImage(bmp);
    //        //int width = (int)Math.Round(Convert.ToInt32(bmp.Width) * g.DpiX / 72.0);
    //        //int height = (int)Math.Round(Convert.ToInt32(bmp.Height) * g.DpiY / 72.0);
    //        int width = 400;
    //        int height = 200;
    //        bmp = new Bitmap(width, height);
    //        Report report = new Report();
    //        report.Spec = tempSpec;
    //        report.DrawSpec(ref bmp);
    //        //DrawSpec(ref bmp, tempSpec);
    //        return bmp;

    //        //Bitmap bmp = new Bitmap(120, 60);
    //        //Graphics g = Graphics.FromImage(bmp);
    //        //int width = (int)Math.Round(Convert.ToInt32(bmp.Width) * g.DpiX / 72.0);
    //        //int height = (int)Math.Round(Convert.ToInt32(bmp.Height) * g.DpiY / 72.0);
    //        //bmp = new Bitmap(width, height);
    //        //DrawSpec(ref bmp, tempSpec);
    //        //return bmp;
    //    }

    //    ///<summary>
    //    /// 获取样品图
    //    /// </summary>
    //    /// <param name="Image"></param>
    //    /// <returns></returns>
    //    public static Bitmap GetSampleImage(byte[] Image)
    //    {
    //        Bitmap b = null;
    //        if (Image == null) return null;
    //        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Image))
    //        {
    //            b = new Bitmap(ms);
    //            return b;
    //        }
    //        return b;
    //    }

    //    public static int specCount = 1;
    //    public static Spec unitSpec; ///连测谱
    //    /// <summary>
    //    /// 画谱图
    //    /// </summary>
    //    /// <param name="bitmap"></param>
    //    private static void DrawSpec(ref Bitmap bitmap, Spec tSpec)
    //    {
    //        Spec tempSpec;
    //        int intSpace = 20;//间距
    //        Graphics g = Graphics.FromImage(bitmap);
    //        Brush backBrush = new SolidBrush(Color.White);
    //        Font font = new Font("Tahoma", 8, FontStyle.Regular);
    //        Pen linePen = new Pen(Color.Black);
    //        Brush textBrush = new SolidBrush(Color.Black);
    //        Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
    //        g.FillRectangle(backBrush, rect);
    //        Rectangle rectLine = new Rectangle(1, 1, bitmap.Width - 2, bitmap.Height - 2);
    //        g.DrawRectangle(linePen, rectLine);
    //        Size textSize = g.MeasureString("123456", font).ToSize(); //字符串大小
    //        int wMargin = Convert.ToInt32(2 * bitmap.Width / 100.0);//左右边距
    //        int hMargin = Convert.ToInt32(bitmap.Height / 100.0) + 2;//上下边距
    //        int textWidth = textSize.Width;
    //        int textHeight = textSize.Height;

    //        int tempWidth = Convert.ToInt32((bitmap.Width - 2 * wMargin - (specCount - 1) * intSpace - specCount * textWidth) / specCount * 1.0);// 谱图实际宽度
    //        int tempHeight = bitmap.Height - 2 * (hMargin + textHeight);//谱图实际高度

    //        for (int n = 1; n <= specCount; n++)
    //        {
    //            if (n == 1)
    //            {
    //                tempSpec = tSpec;
    //                //Spec.SpecDatas.CopyTo(tempSpec.SpecDatas, 0);
    //            }
    //            else
    //            {
    //                //unitSpec.SpecDatas.CopyTo(tempSpec.SpecDatas, 0);
    //                tempSpec = unitSpec;
    //            }
    //            int maxValue = SpecHelper.GetHighSpecValue(tempSpec.SpecDatas);
    //            double pixX = tempWidth / 1600.0;
    //            double pixY = tempHeight / (maxValue * 1.0);
    //            if (maxValue == 0)
    //            {
    //                pixY = 0;
    //            }
    //            int orgX = wMargin + n * textWidth + (n - 1) * (tempWidth + intSpace);
    //            g.DrawLine(linePen, orgX, tempHeight + hMargin + textHeight, orgX + tempWidth, tempHeight + hMargin + textHeight); //画X轴
    //            g.DrawLine(linePen, orgX, tempHeight + hMargin + textHeight, orgX, hMargin + textHeight);//画Y轴

    //            int titleW = 0;
    //            for (int i = 0; i <= 5; i++) //画Y轴刻度
    //            {
    //                int k = (int)Math.Round(maxValue * (5 - i) / 5.0);
    //                titleW = g.MeasureString(k.ToString(), font).ToSize().Width;
    //                int y = hMargin + textHeight + (int)Math.Round(tempHeight * i / 5.0);
    //                g.DrawLine(linePen, orgX, y, orgX - 4, y);
    //                g.DrawString(k.ToString(), font, textBrush, orgX - 4 - titleW, y - textHeight / 2);
    //            }

    //            Point[] point = new Point[2000];
    //            for (int i = 0; i < 2000; i++)
    //            {
    //                point[i].X = orgX + Convert.ToInt32(Math.Round(pixX * i));
    //                point[i].Y = hMargin + textHeight + tempHeight - Convert.ToInt32(Math.Round(pixY * tempSpec.SpecDatas[i]));
    //                if (point[i].Y < hMargin + textHeight)
    //                {
    //                    point[i].Y = hMargin + textHeight;
    //                }
    //                if (i % 500 == 0)
    //                {
    //                    g.DrawLine(linePen, point[i].X, hMargin + textHeight + tempHeight, point[i].X, hMargin + textHeight + tempHeight + 4);
    //                    titleW = g.MeasureString(i.ToString(), font).ToSize().Width / 2;
    //                    g.DrawString(i.ToString(), font, textBrush, point[i].X - titleW, hMargin + textHeight + tempHeight + 4);
    //                }
    //                else if (i % 100 == 0)
    //                {
    //                    g.DrawLine(linePen, point[i].X, hMargin + textHeight + tempHeight, point[i].X + 2, hMargin + textHeight + tempHeight);
    //                }
    //            }
    //            //g.FillPolygon(backBrush, point);
    //            g.DrawPolygon(linePen, point);


    //        }



    //    }


    //    /// <summary>
    //    /// 画感兴趣元素图
    //    /// </summary>
    //    /// <param name="bitmap"></param>
    //    private void DrawInterstringElems(ref Bitmap bitmap, Spec tSpec, CurveElement curveElement)
    //    {
    //        Spec tempSpec = tSpec;
    //        int InterestElemCount = 1;
    //        int curElemCount = 0;
    //        int OFFSET = 50;

    //        int maxCh = 300;
    //        Graphics g = Graphics.FromImage(bitmap);
    //        Brush backBrush = new SolidBrush(Color.White);
    //        Font font = new Font("Tahoma", 8, FontStyle.Regular);
    //        Pen linePen = new Pen(Color.Black);
    //        Brush textBrush = new SolidBrush(Color.Black);
    //        Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
    //        g.FillRectangle(backBrush, rect);
    //        Rectangle rectLine = new Rectangle(1, 1, bitmap.Width - 2, bitmap.Height - 2);
    //        g.DrawRectangle(linePen, rectLine);
    //        Size textSize = g.MeasureString("123456", font).ToSize(); //字符串大小
    //        int wMargin = Convert.ToInt32(2 * bitmap.Width / 100.0);//左右边距
    //        int hMargin = Convert.ToInt32(bitmap.Height / 100.0);//上下边距
    //        int textWidth = textSize.Width;
    //        int textHeight = textSize.Height;

    //        int tempWidth = (bitmap.Width - 2 * wMargin) / (int)Math.Round((InterestElemCount + 0.2) / 2);//根据元素个数分列
    //        int tempHeight = bitmap.Height / 2;
    //        tempHeight -= 2 * hMargin;
    //        int maxH = tempHeight - 2 * textHeight;
    //        int tempY = hMargin + textHeight;
    //        Point[] p = new Point[maxCh];
    //        int tempLeft = 0;
    //        int tempRight = 0;
    //        int row = 0;
    //        int col = 0;
    //        int tempCol = 0;
    //        int x = 0;
    //        int y = 0;
    //        int titleW = 0;
    //        string tempEnergy = string.Empty;
    //        double pixX = 0f;
    //        double pixY = 0f;
    //        int[] maxValue = new int[InterestElemCount];//每个感兴趣元素区域的最大值
    //        for (int i = 0; i < InterestElemCount; i++)
    //        {
    //            if (curveElement.PeakLow == 0)
    //            {
    //                continue;
    //            }
    //            ClearPoint(ref p);

    //            if (specCount > 1) //连测则有2个谱
    //            {
    //                if (i <= curElemCount)
    //                {
    //                    tempSpec = tSpec;
    //                }
    //                else
    //                {
    //                    tempSpec.SpecData = unitSpec.SpecData;
    //                }
    //            }
    //            else
    //            {
    //                tempSpec = tSpec;
    //            }
    //            tempLeft = curveElement.PeakLow - OFFSET;
    //            tempRight = curveElement.PeakHigh + OFFSET;

    //            //获取每个感兴趣元素区域的最大值
    //            try
    //            {
    //                maxValue[i] = tempSpec.SpecDatas[tempLeft];
    //            }
    //            catch (System.Exception ex)
    //            {
    //                //    MessageBoxEx.Show(Info.PeakSetError, Info.Hint, MessageBoxButtons.OK, MessageBoxIcon.Error);
    //                //    return;
    //            }
    //            for (int j = tempLeft; j < tempRight; j++)
    //            {
    //                if (tempSpec.SpecDatas[j] > maxValue[i])
    //                {
    //                    maxValue[i] = tempSpec.SpecDatas[j];
    //                }
    //            }
    //            if (maxValue[i] < 5)
    //            {
    //                maxValue[i] = 5;
    //            }

    //            //判断元素属于哪一行、列
    //            if (i >= InterestElemCount / 2.0)
    //            {
    //                row = 1;
    //                col = i - (int)Math.Round((InterestElemCount + 0.2) / 2);
    //            }
    //            else
    //            {
    //                row = 0;
    //                col = i;
    //            }

    //            tempCol = col * tempWidth;
    //            pixX = tempWidth / 200.0;
    //            pixY = maxH * 1.0 / maxValue[i];

    //            //画Y轴
    //            x = wMargin + textWidth + tempCol;
    //            y = tempY + maxH + tempHeight * row;
    //            g.DrawLine(linePen, x, hMargin + tempHeight * row, x, y);

    //            //画X轴
    //            g.DrawLine(linePen, x, y, x + tempWidth - textWidth, y);
    //            g.DrawString("KeV", font, textBrush, x + tempWidth - textWidth - 20, y + 4);

    //            for (int k = 0; k <= 3; k++)
    //            {
    //                int m = (int)Math.Round(maxValue[i] * (3 - k) / 3.0);
    //                titleW = g.MeasureString(m.ToString(), font).ToSize().Width;
    //                y = tempY + row * tempHeight + (int)Math.Round(maxH * k / 3.0);
    //                g.DrawLine(linePen, x, y, x - 4, y);
    //                g.DrawString(m.ToString(), font, textBrush, x - 4 - titleW, y - textHeight / 2);
    //            }
    //            int count = 0;
    //            for (int j = 0; j < maxCh; j++)
    //            {
    //                if ((j <= tempRight - tempLeft))
    //                {
    //                    p[j].X = x + (int)Math.Round(pixX * j);
    //                    p[j].Y = y - (int)Math.Round(pixY * tempSpec.SpecDatas[j + tempLeft]);
    //                }
    //                else
    //                {
    //                    p[j].X = x + (int)Math.Round(pixX * (tempRight - tempLeft));
    //                    p[j].Y = y;
    //                }
    //                if (p[j].Y < tempY + row * tempHeight)
    //                {
    //                    p[j].Y = tempY + row * tempHeight;
    //                }

    //                if ((j % 50 == 0) && (j <= tempRight - tempLeft) && (count < 3))
    //                {
    //                    count++;
    //                    g.DrawLine(linePen, p[j].X, y, p[j].X, y + 4);
    //                    tempEnergy = DemarcateEnergyHelp.GetEnergy(j + tempLeft).ToString("f1"); //通道转化为能量
    //                    titleW = g.MeasureString(tempEnergy, font).ToSize().Width / 2;
    //                    g.DrawString(tempEnergy, font, textBrush, p[j].X - titleW, y + 4); ;
    //                }
    //            }

    //            //设置起始端Y值相等
    //            p[0].Y = y;
    //            p[1].Y = y;
    //            p[0].X = p[1].X;
    //            p[maxCh - 1].Y = y;

    //            g.DrawPolygon(linePen, p); //画谱

    //            titleW = g.MeasureString("Ag-Ka", font).ToSize().Width / 2;
    //            int l = curveElement.PeakLow;
    //            int r = curveElement.PeakHigh;
    //            int w = r - l;
    //            Point[] boundPoints = new Point[w + 3];
    //            for (int j = 0; j <= w; j++)
    //            {
    //                boundPoints[j].X = p[j + OFFSET].X;
    //                boundPoints[j].Y = p[j + OFFSET].Y;
    //            }
    //            boundPoints[w + 1].X = boundPoints[w].X;
    //            boundPoints[w + 1].Y = y;
    //            boundPoints[w + 2].X = boundPoints[0].X;
    //            boundPoints[w + 2].Y = boundPoints[w + 1].Y;


    //            g.FillPolygon(new SolidBrush(Color.FromArgb(curveElement.Color)), boundPoints);

    //            //标识峰的线系 0722
    //            string[] lines = { "Ka", "Kb", "La", "Lb" };
    //            int index = (int)curveElement.AnalyteLine;
    //            int peakCh = w / 2 + OFFSET;

    //            g.DrawString(curveElement.Caption + "-" + lines[index], font, new SolidBrush(Color.FromArgb(curveElement.Color)), p[peakCh].X - titleW, p[peakCh].Y - 16);
    //        }
    //    }

    //    /// <summary>
    //    /// 清空点数据
    //    /// </summary>
    //    /// <param name="p"></param>
    //    private void ClearPoint(ref Point[] p)
    //    {
    //        for (int i = 0; i < p.Length; i++)
    //        {
    //            p[i].X = 0;
    //            p[i].Y = 0;
    //        }
    //    }

    //    #endregion


    //    public List<TreeNodeInfo> GetZhongDa(List<DataFountain> dataFountainList, PrintInitParam printInitParam
    //        , PrintWorkCurve printWorkCurve, PrintSample printSample, Bitmap SpecImage, PrintSpecImage printSpecImage
    //        , List<TestResult> lTestResult, List<PrintDeviceParameter> LprintDeviceParameter, BaseInfoCls baseInfocls)
    //    {
    //        var template = new List<TreeNodeInfo>();
    //        XElement xele = XElement.Load(Application.StartupPath + "\\printxml\\data.xml");

    //        XElement xeLabel = XElement.Load(Application.StartupPath + "\\printxml\\label.xml");

    //        var label = xeLabel.Elements("Label").ToList();
    //        foreach (XElement xe in label)
    //        {
    //            template.Add(new TreeNodeInfo
    //            {
    //                Type = CtrlType.Label,
    //                Name = xe.Element("Name").Value,
    //                Text = xe.Element("Text").Value,
    //                Caption = xe.Element("Caption").Value
    //            });
    //        }

    //        #region 初始化参数
    //        var lbl = xele.Elements("Label").ToList();
    //        foreach (XElement xe in lbl)
    //        {
    //            List<object> lSouce = new List<object>();
    //            List<DataTable> lDT = new List<DataTable>();
    //            #region
    //            DataTable dt = new DataTable();
    //            dt.Columns.Add("a");
    //            DataRow dr;
    //            dr = dt.NewRow();
    //            dr[0] = "11111111";
    //            dt.Rows.Add(dr);
    //            #endregion

    //            if (xe.Attribute("id").Value == "基本信息")
    //            {
    //                Type t = ClassHelper.BuildType("TempCls");
    //                List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
    //                ClassHelper.CustPropertyInfo cpi;
    //                foreach (XElement a in xe.Nodes().ToList())
    //                {
    //                    cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
    //                    lcpi.Add(cpi);
    //                }
    //                t = ClassHelper.AddProperty(t, lcpi);
    //                object obj = ClassHelper.CreateInstance(t);
    //                BaseInfoCls model = baseInfocls;
    //                Do(model, xe, obj);
    //                lDT.Add(dt);
    //                lSouce.Add(obj);
    //                GetNote(template, lSouce, lDT, "基本信息");
    //            }
    //            else if (xe.Attribute("id").Value == "工作曲线")
    //            {
    //                Type t = ClassHelper.BuildType("TempCls");
    //                List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
    //                ClassHelper.CustPropertyInfo cpi;
    //                foreach (XElement a in xe.Nodes().ToList())
    //                {
    //                    cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
    //                    lcpi.Add(cpi);
    //                }
    //                t = ClassHelper.AddProperty(t, lcpi);
    //                object obj = ClassHelper.CreateInstance(t);
    //                PrintWorkCurve model = printWorkCurve;
    //                Do(model, xe, obj);
    //                lDT.Add(dt);
    //                lSouce.Add(obj);
    //                GetNote(template, lSouce, lDT, "工作曲线");
    //            }
    //            else if (xe.Attribute("id").Value == "样品信息")
    //            {
    //                Type t = ClassHelper.BuildType("TempCls");
    //                List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
    //                ClassHelper.CustPropertyInfo cpi;
    //                foreach (XElement a in xe.Nodes().ToList())
    //                {
    //                    cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
    //                    lcpi.Add(cpi);
    //                }
    //                t = ClassHelper.AddProperty(t, lcpi);
    //                object obj = ClassHelper.CreateInstance(t);
    //                PrintSample model = printSample;
    //                Do(model, xe, obj);
    //                lDT.Add(dt);
    //                lSouce.Add(obj);
    //                GetNote(template, lSouce, lDT, "样品信息");
    //            }
    //            else if (xe.Attribute("id").Value == "测量条件")
    //            {
    //                Type t = ClassHelper.BuildType("TempCls");
    //                List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
    //                ClassHelper.CustPropertyInfo cpi;
    //                foreach (XElement a in xe.Nodes().ToList())
    //                {
    //                    cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
    //                    lcpi.Add(cpi);
    //                }
    //                t = ClassHelper.AddProperty(t, lcpi);
    //                object obj = ClassHelper.CreateInstance(t);
    //                PrintDeviceParameter model = LprintDeviceParameter[0];
    //                Do(model, xe, obj);
    //                lDT.Add(dt);
    //                lSouce.Add(obj);
    //                GetNote(template, lSouce, lDT, "测量条件");
    //            }
    //            else if (xe.Attribute("id").Value == "总判定")
    //            {
    //                Type t = ClassHelper.BuildType("TempCls");
    //                List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
    //                ClassHelper.CustPropertyInfo cpi;
    //                foreach (XElement a in xe.Nodes().ToList())
    //                {
    //                    cpi = new ClassHelper.CustPropertyInfo("System.String", a.Attribute("id").Value);
    //                    lcpi.Add(cpi);
    //                }
    //                t = ClassHelper.AddProperty(t, lcpi);
    //                object obj = ClassHelper.CreateInstance(t);
    //                TotalDeterminant model = new TotalDeterminant();
    //                model.TotalDeterminant1 = (dataFountainList[0].TotalDeterminant == null) ? "" : dataFountainList[0].TotalDeterminant[0, 0] + "~" + dataFountainList[0].TotalDeterminant[0, 1];
    //                Do(model, xe, obj);
    //                lDT.Add(dt);
    //                lSouce.Add(obj);
    //                GetNote(template, lSouce, lDT, "总判定");
    //            }
    //        }

    //        #endregion

    //        #region 复合表测量结果和统计信息
    //        var table = xele.Elements("Table").ToList();
    //        foreach (XElement xe in table)
    //        {
    //            List<DataTable> lDT = new List<DataTable>();
    //            List<object> lSouce = new List<object>();
    //            var DT = new TempDt();
    //            DataTable dt1 = new DataTable();
    //            if (xe.Attribute("id").Value == "测试结果")
    //            {
    //                #region
    //                //if (dataFountainList.Count == 1)
    //                //{

    //                //    if (dataFountainList[0].LTestResult.Count == 1)
    //                //    {
    //                //        int rowsIndex = 0;
    //                //        TestResult ldt1 = lTestResult[0];

    //                //        dt1.Columns.Add("元素");
    //                //        dt1.Columns.Add("A");
    //                //        dt1.Columns.Add("B");
    //                //        dt1.Columns.Add("C");
    //                //        dt1.Columns.Add("D");

    //                //        if (ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] == "False" || ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] == "Pass")
    //                //        {
    //                //            dt1.Columns.Add(ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 0]);
    //                //            rowsIndex = ldt1.ElementResult.Length / 6 - 1;
    //                //        }
    //                //        else
    //                //        {
    //                //            rowsIndex = ldt1.ElementResult.Length / 6;
    //                //        }
    //                //        DataRow dr;
    //                //        for (int i = 0; i < rowsIndex; i++)
    //                //        {
    //                //            dr = dt1.NewRow();
    //                //            dr[0] = ldt1.ElementResult[i, 0];
    //                //            dr[1] = ldt1.ElementResult[i, 3] + "~" + ldt1.ElementResult[i, 2];
    //                //            //dr[2] = ldt1.ElementResult[i, 2];
    //                //            dr[2] = ldt1.ElementResult[i, 1];
    //                //            dr[3] = ldt1.ElementResult[i, 4];
    //                //            dr[4] = ldt1.ElementResult[i, 5];

    //                //            if (ldt1.ElementResult[i, 2] == null)
    //                //            {
    //                //                dr[5] = "Pass~Green";
    //                //            }
    //                //            else
    //                //            {
    //                //                dr[5] = "False~Red";
    //                //            }
    //                //            //if (ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] == "False" || ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] == "Pass")
    //                //            //{
    //                //            //    dr[5] = ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 1] + "~" + ldt1.ElementResult[ldt1.ElementResult.Length / 6 - 1, 2];
    //                //            //}
    //                //            dt1.Rows.Add(dr);
    //                //        }
    //                //    }

    //                //}
    //                //else
    //                //{
    //                //    dt1.Columns.Add("A");
    //                //    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6; i++)
    //                //    {
    //                //        dt1.Columns.Add(i.ToString());
    //                //    }
    //                //    DataRow dr;
    //                //    dr = dt1.NewRow();
    //                //    dr[0] = "";
    //                //    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6; i++)
    //                //    {
    //                //        dr[i + 1] = dataFountainList[0].LTestResult[0].ElementResult[i, 0];
    //                //    }
    //                //    dt1.Rows.Add(dr);
    //                //    for (int i = 1; i <= dataFountainList.Count; i++)
    //                //    {
    //                //        dr = dt1.NewRow();
    //                //        dr[0] = i.ToString();
    //                //        for (int j = 0; j < dataFountainList[0].LTestResult[0].ElementResult.Length / 6; j++)
    //                //        {
    //                //            dr[j + 1] = dataFountainList[i - 1].LTestResult[0].ElementResult[j, 1];
    //                //        }
    //                //        dt1.Rows.Add(dr);
    //                //    }
    //                //}
    //                #endregion

    //                if (dataFountainList.Count > 0)
    //                {
    //                    dt1.Columns.Add("元素名称");
    //                    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6 - 1; i++)
    //                    {
    //                        dt1.Columns.Add(dataFountainList[0].LTestResult[0].ElementResult[i, 0]);
    //                    }
    //                    #region 全元素名称
    //                    DataRow dr = dt1.NewRow();
    //                    if (languageModel.CurrentLangId == 2)//中文
    //                    {
    //                        dr[0] = "元素全名称";
    //                        for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6 - 1; i++)
    //                        {
    //                            var atom = dataFountainList[0].atomList.Find(c => c.AtomName ==
    //                                dataFountainList[0].LTestResult[0].ElementResult[i, 0].ToString().Substring(0, dataFountainList[0].LTestResult[0].ElementResult[i, 0].ToString().IndexOf("(")));
    //                            if (atom != null)
    //                            {
    //                                dr[i + 1] = atom.AtomNameCN;
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        dr[0] = "Element Name";
    //                        for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6 - 1; i++)
    //                        {
    //                            var atom = dataFountainList[0].atomList.Find(c => c.AtomName ==
    //                                dataFountainList[0].LTestResult[0].ElementResult[i, 0].ToString().Substring(0, dataFountainList[0].LTestResult[0].ElementResult[i, 0].ToString().IndexOf("(")));
    //                            if (atom != null)
    //                            {
    //                                dr[i + 1] = atom.AtomNameEN;
    //                            }
    //                        }
    //                    }

    //                    dt1.Rows.Add(dr);

    //                    #endregion
    //                    #region 增加元素名称列
    //                    dr = dt1.NewRow();
    //                    if (languageModel.CurrentLangId == 2)//中文
    //                    {
    //                        dr[0] = "元素名称";
    //                    }
    //                    else
    //                    {
    //                        dr[0] = "Element Name";
    //                    }
    //                    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6 - 1; i++)
    //                    {
    //                        dr[i + 1] = dataFountainList[0].LTestResult[0].ElementResult[i, 0];
    //                    }
    //                    dt1.Rows.Add(dr);
    //                    #endregion

    //                    #region 含量
    //                    dr = dt1.NewRow();
    //                    if (languageModel.CurrentLangId == 2)
    //                    {
    //                        dr[0] = "含量";
    //                    }
    //                    else
    //                    {
    //                        dr[0] = "Content";
    //                    }
    //                    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6 - 1; i++)
    //                    {
    //                        dr[i + 1] = dataFountainList[0].LTestResult[0].ElementResult[i, 1];
    //                    }
    //                    dt1.Rows.Add(dr);
    //                    #endregion

    //                    #region 强度
    //                    dr = dt1.NewRow();
    //                    if (languageModel.CurrentLangId == 2)
    //                    {
    //                        dr[0] = "强度";
    //                    }
    //                    else
    //                    {
    //                        dr[0] = "Strength";
    //                    }
    //                    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6 - 1; i++)
    //                    {
    //                        dr[i + 1] = dataFountainList[0].LTestResult[0].ElementResult[i, 3] + "~" + dataFountainList[0].LTestResult[0].ElementResult[i, 2];
    //                    }
    //                    dt1.Rows.Add(dr);
    //                    #endregion

    //                    #region 标准
    //                    dr = dt1.NewRow();
    //                    if (languageModel.CurrentLangId == 2)
    //                    {
    //                        dr[0] = "标准";
    //                    }
    //                    else
    //                    {
    //                        dr[0] = "Standard";
    //                    }
    //                    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6 - 1; i++)
    //                    {
    //                        dr[i + 1] = dataFountainList[0].LTestResult[0].ElementResult[i, 5];
    //                    }
    //                    dt1.Rows.Add(dr);
    //                    #endregion

    //                    #region 判定
    //                    dr = dt1.NewRow();
    //                    if (languageModel.CurrentLangId == 2)
    //                    {
    //                        dr[0] = "判定";
    //                    }
    //                    else
    //                    {
    //                        dr[0] = "Determine";
    //                    }
    //                    for (int i = 0; i < dataFountainList[0].LTestResult[0].ElementResult.Length / 6 - 1; i++)
    //                    {
    //                        if (dataFountainList[0].LTestResult[0].ElementResult[i, 2] == null)
    //                        {
    //                            dr[i + 1] = "Pass~Green";
    //                        }
    //                        else
    //                        {
    //                            dr[i + 1] = "False~Red";
    //                        }
    //                    }
    //                    dt1.Rows.Add(dr);
    //                    #endregion

    //                    DT.DT = dt1;
    //                    lSouce.Add(DT);
    //                    lDT.Add(dt1);
    //                    GetNote(template, lSouce, lDT, "测试结果");
    //                }
    //            }
    //            else if (xe.Attribute("id").Value == "统计信息")
    //            {
    //                foreach (XElement a in xe.Nodes().ToList())
    //                {
    //                    dt1.Columns.Add(a.Value);
    //                }
    //                List<StatInfo> ldt1 = dataFountain.LStatInfo;
    //                DataRow dr;
    //                foreach (StatInfo model in ldt1)
    //                {
    //                    dr = dt1.NewRow();
    //                    dt1.Rows.Add(GetDr(dt1, dr, xe, model));
    //                }
    //                DT.DT = dt1;
    //                lSouce.Add(DT);
    //                lDT.Add(dt1);
    //                GetNote(template, lSouce, lDT, "统计信息");
    //            }
    //        }
    //        #endregion

    //        #region 谱图
    //        var image = xele.Elements("Image").ToList();
    //        foreach (XElement xe in image)
    //        {
    //            List<object> lSouce = new List<object>();
    //            List<DataTable> lDT = new List<DataTable>();
    //            DataTable dt = new DataTable();
    //            dt.Columns.Add("a");
    //            if (xe.Attribute("id").Value == "谱图")
    //            {
    //                Image imagee = null;
    //                //if (dataFountain.ByteSpecData == null)
    //                //    imagee = GetSpecImage(dataFountain.SpecData);
    //                //else
    //                imagee = SpecImage;//GetSampleImage(dataFountain.ByteSpecData);
    //                template.Add(new TreeNodeInfo
    //                {
    //                    Type = CtrlType.Image,
    //                    Name = "谱图",
    //                    Text = "",
    //                    Image = imagee
    //                });
    //            }
    //            else if (xe.Attribute("id").Value == "样品图")
    //            {
    //                template.Add(new TreeNodeInfo
    //                {
    //                    Type = CtrlType.Image,
    //                    Name = "样品图",
    //                    Text = "",
    //                    Image = (GetSampleImage(dataFountain.SampleImage) == null) ? null : GetSampleImage(dataFountain.SampleImage)
    //                });
    //            }
    //            else if (xe.Attribute("id").Value == "元素图")
    //            {
    //                Dictionary<string, Bitmap> d = printSpecImage.ElementSpecImage;
    //                foreach (KeyValuePair<string, Bitmap> a in d)
    //                {
    //                    if (languageModel.CurrentLangId == 2)//中文
    //                    {
    //                        template.Add(new TreeNodeInfo
    //                        {
    //                            Type = CtrlType.Image,
    //                            Name = "元素",
    //                            Text = "元素",
    //                            Image = a.Value
    //                        });
    //                    }
    //                    else
    //                    {
    //                        template.Add(new TreeNodeInfo
    //                        {
    //                            Type = CtrlType.Image,
    //                            Name = "Element",
    //                            Text = "Element",
    //                            Image = a.Value
    //                        });
    //                    }
    //                    break;
    //                }

    //            }
    //        }

    //        #endregion
    //        return template;
    //    }




    //    private static DataRow GetDr(DataTable dt1, DataRow dr, XElement xe, object model)
    //    {
    //        List<object> lo = new List<object>();
    //        int rowIndex = 0;

    //        foreach (PropertyInfo p in model.GetType().GetProperties())
    //        {
    //            foreach (XElement a in xe.Nodes().ToList())
    //            {
    //                if (p.Name.Equals(a.Attribute("id").Value))
    //                {
    //                    object ooo = model.GetType().InvokeMember(p.Name, BindingFlags.GetProperty, null, model, null);
    //                    if (ooo == null)
    //                    {
    //                        ooo = new object();
    //                        ooo = "";
    //                    }
    //                    dr[rowIndex++] = ooo.ToString();
    //                    break;
    //                }
    //            }
    //        }
    //        return dr;
    //    }

    //    private static void Do(object model, XElement xe, object obj)
    //    {
    //        foreach (var p in model.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
    //        {
    //            foreach (XElement a in xe.Nodes().ToList())
    //            {
    //                if (p.Name.Equals(a.Attribute("id").Value))
    //                {
    //                    object ooo = model.GetType().InvokeMember(p.Name, BindingFlags.GetProperty, null, model, null);
    //                    if (ooo == null)
    //                    {
    //                        ooo = new object();
    //                    }
    //                    ClassHelper.SetPropertyValue(obj, p.Name, ooo.ToString());

    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    public static List<TreeNodeInfo> GetNote(List<TreeNodeInfo> template, List<object> lSource, List<DataTable> lDT, string text)
    //    {
    //        template.Add(new TreeNodeInfo
    //        {
    //            Type = CtrlType.ComposeTable,
    //            contextType = CompositeContextType.Original,
    //            Name = text,
    //            Text = text,
    //            Tables = lDT,
    //            ObjSource = lSource
    //        });
    //        return template;
    //    }

    //    /// <summary>
    //    /// 表格
    //    /// </summary>
    //    /// <returns></returns>
    //    public static List<TreeNodeInfo> GetNote(List<TreeNodeInfo> template, List<object> lSource, DataTable dt, string text)
    //    {
    //        template.Add(new TreeNodeInfo
    //        {
    //            Type = CtrlType.Grid,
    //            Name = text,
    //            Text = text,
    //            Table = dt,
    //            ObjSource = lSource
    //        });
    //        return template;
    //    }

    //}



    //public class TempDt
    //{
    //    /// <summary>
    //    /// 数据源
    //    /// </summary>
    //    public DataTable DT { get; set; }
    //}

    //public class ImageDt
    //{
    //    /// <summary>
    //    /// 数据源
    //    /// </summary>
    //    public PictureBox ImageDT { get; set; }
    //}
    //#endregion
    #endregion
}
