using Lephone.Data.Definition;
using System;
using System.Drawing;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 元素类
    /// </summary>
    [Serializable]
    public abstract class CurveElement : DbObjectModel<CurveElement>,ICloneable
    {
        /// <summary>
        /// 所属元素列表
        /// </summary>
        [BelongsTo, DbColumn("ElementList_Id")]
        public abstract ElementList ElementList { get; set; }
        /// <summary>
        /// 包含标准样品
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<StandSample> Samples { get; set; }

        [HasMany(OrderBy = "Id")]
        public abstract HasMany<ReferenceElement> References { get; set; }
        /// <summary>
        /// 消去值
        /// </summary>
        [HasOne(OrderBy = "Id")]
        public abstract Expunction Expunction { get; set; }
        /// <summary>
        /// 包含优化
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<Optimiztion> Optimiztion { get; set; }
        /// <summary>
        /// 包含影响元素
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<ElementRef> ElementRefs { get; set; }

        public abstract long DevParamId { get; set; }//小条件ID

        /// <summary>
        /// 元素或化合物名称
        /// </summary>
        [LengthAttribute(ColLength.CurveElementCaption)]
        public abstract String Caption { get; set; }
        /// <summary>
        /// 在计算结果和报告中是否显示结果 默认显示
        /// </summary>
        public abstract bool IsDisplay { get; set; }
        /// <summary>
        /// 化学式
        /// </summary>
        [LengthAttribute(ColLength.Formula)]
        public abstract String Formula { get; set; }
        /// <summary>
        /// 测量元素的原子序号
        /// </summary>
        public abstract int AtomicNumber { get; set; }
        /// <summary>
        /// 所在的层数
        /// </summary>
        public abstract int LayerNumber { get; set; } //
        /// <summary>
        /// 层数中文表示
        /// </summary>
        [LengthAttribute(ColLength.LayerNumBackUp)]
        public abstract string LayerNumBackUp { get; set; }
        /// <summary>
        /// 特征线  0-Ka, 1-Kb,2-La,3-Lb,4-Lr  5-Le
        /// </summary>
        public abstract XLine AnalyteLine { get; set; }
        /// <summary>
        /// 峰左界
        /// </summary>
        public abstract int PeakLow { get; set; }
        /// <summary>
        /// 峰右界
        /// </summary>
        public abstract int PeakHigh { get; set; }
        /// <summary>
        /// 背左界
        /// </summary>
        public abstract int BaseLow { get; set; }
        /// <summary>
        /// 背右界 
        /// </summary>
        public abstract int BaseHigh { get; set; }
        /// <summary>
        /// 是否采用峰背比
        /// </summary>
        public abstract bool PeakDivBase { get; set; }

        /// <summary>
        /// 背景强度计算方法
        /// </summary>
        public abstract BaseIntensityWay BaseIntensityWay { get; set; }

        /// <summary>
        /// 层密度
        /// </summary>
        public abstract double LayerDensity { get; set; }
        /// <summary>
        /// 强度
        /// </summary>
        public abstract double Intensity { get; set; }

        public abstract double BgIntensity { get; set; }


        public abstract double Content { get; set; }

        public abstract double CumulativeValue { get; set; }

        //private double _Content;
        ///// <summary>
        ///// 含量
        ///// </summary>
        //public double Content
        //{
        //    get { return 0.0d; }

        //    set { _Content = value; }
        //}


        //private double _ActContent;
        //public double ActContent
        //{
        //    get
        //    {
        //        //_ActContent = Content / 1000;

        //        return _ActContent;
        //    }
        //    set
        //    {

        //        _ActContent = value;
        //    }
        //}

        /// <summary>
        /// 厚度
        /// </summary>
        public abstract double Thickness { get; set; }



        /// <summary>
        /// 强度计算方法 1-FullArea  2-NetArea  3- Reference 4 Gauss 5 FixedGauss  6 FPGauss
        /// </summary>
        public abstract IntensityWay IntensityWay { get; set; }
        /// <summary>
        /// 是否比较a，b系的强度
        /// </summary>
        public abstract bool IntensityComparison { get; set; }
        /// <summary>
        /// a/b的阈值
        /// </summary>
        public abstract double ComparisonCoefficient { get; set; } 
        /// <summary>
        /// b峰的左边界
        /// </summary>
        public abstract int BPeakLow { get; set; }
        /// <summary>
        /// b峰的右边界
        /// </summary>
        public abstract int BPeakHigh { get; set; }
        /// <summary>
        /// 含量计算方法 1-Insert插值 2-linear一次曲线 3 conic二次曲线  4 IntensityCorrect强度校正  5 ContentContect含量校正
        /// </summary>
        public abstract CalculationWay CalculationWay { get; set; }
        /// <summary>
        ///  fp含量计算方法 1-一次不强制过原点 2-一次强制过原点 3-两次不强制过原点  4 -两次强制过原点
        /// </summary>
        public abstract FpCalculationWay FpCalculationWay { get; set; }
        /// <summary>
        /// 元素标志 1-Calculated计算, 2-Fixed不计算, 3-Difference,差额，4-Added添加剂  5-Internal 内标法；
        /// </summary>
        public abstract ElementFlag Flag { get; set; }
        /// <summary>
        /// 镀层标志  1-Calculated, 2-Fixed
        /// </summary>
        public abstract LayerFlag LayerFlag { get; set; }
        /// <summary>
        /// 含量单位 1 –%, 2 - ppm
        /// </summary>
        public abstract ContentUnit ContentUnit { get; set; }
        /// <summary>
        /// 厚度单位 1—mu  2—u’’
        /// </summary>
        public abstract ThicknessUnit ThicknessUnit { get; set; }

        /// <summary>
        /// 拟合元素名称
        /// </summary>
        public abstract string SReferenceElements { get; set; }
        public string[] ReferenceElements
        {
            get { return Helper.ToStrs(SReferenceElements); }
        }
        /// <summary>
        /// 纯元素普数据;如果是纯元素拟合需要这些数据,假定这些数据已经除以时间
        /// </summary>
        /// 
#if SqlServer
       [Length(ColLength.SpecDataByte)]
        public abstract byte[] SpecDataByte { get; set; }

        private string _SpecData = string.Empty;
        /// <summary>
        ///子谱数据
        /// </summary>
        public string SSpectrumData
        {
            get
            {
                if (_SpecData == string.Empty)
                {
                    _SpecData = Helper.ToString(SpecDataByte);
                }
                return _SpecData;
            }
            set
            {
                _SpecData = value;
                SpecDataByte = Helper.ToBytes(_SpecData);
            }
        }
#else
        public abstract string SSpectrumData { get; set; }
#endif


        /// <summary>
        /// 存放多条件时的纯元素谱图
        /// </summary>
        public abstract string SSpectrumDataNotFilter { get; set; }
       

        public double[] SpectrumData
        {
            get { return Helper.ToDoubles(SSpectrumData); }
        }
        /// <summary>
        /// 影响元素
        /// </summary>
        [LengthAttribute(ColLength.SInfluenceElements)]
        public abstract string SInfluenceElements { get; set; }

        public string[] InfluenceElements
        {
            get { return Helper.ToStrs(SInfluenceElements); }
            //set { SInfluenceElements = string.Join(",", InfluenceElements); }
        }


        /// <summary>
        /// 干扰阈值
        /// </summary>
        [LengthAttribute(ColLength.DistrubThreshold)]
        public abstract string DistrubThreshold { get; set; }

        public double[] DistrubThresholds
        {
            get { return Helper.ToDoubles(DistrubThreshold); }
        }

        /// <summary>
        /// 是否影响
        /// </summary>
        public abstract bool IsInfluence { get; set; }
        /// <summary>
        /// 影响系数 
        /// </summary>
        [LengthAttribute(ColLength.SInfluenceCoefficients)]
        public abstract string SInfluenceCoefficients { get; set; }


        public double[] InfluenceCoefficientLists
        {
            get { return Helper.ToDoubles(SInfluenceCoefficients); }
            //set { SInfluenceCoefficients = Helper.ToStrs(value); }
        }
        //下面三个数据只有在测量粉末或液体的时候才用到
        /// <summary>
        /// 添加剂比率
        /// </summary>
        public abstract Double Asrat { get; set; }
        /// <summary>
        /// 质量厚度
        /// </summary>
        public abstract Double Msthk { get; set; }
        /// <summary>
        /// 灼烧损失率
        /// </summary>
        public abstract Double Loi { get; set; }

        /// <summary>
        /// 如果测量的结果小于Limit,则含量=含量*K1+K0
        /// </summary>
        public abstract double Limit { get; set; }
        /// <summary>
        ///  含量系数1
        /// </summary>
        public abstract double K1 { get; set; }
        /// <summary>
        ///  含量系数2
        /// </summary>
        public abstract double K0 { get; set; }
        /// <summary>
        /// 误差
        /// </summary>
        public abstract double Error { get; set; }
        /// <summary>
        /// 计算误差的时候rohs用到的两个系数
        /// </summary>
        public abstract double ErrorK1 { get; set; }
        /// <summary>
        ///  //计算误差的时候rohs用到的两个系数
        /// </summary>
        public abstract double ErrorK0 { get; set; }
        /// <summary>
        /// 在元素列表中的位置
        /// </summary>
        public abstract int RowsIndex { get; set; }

        /// <summary>
        /// 纯元素谱
        /// </summary>
        [LengthAttribute(ColLength.ElementSpecName)]
        public abstract string ElementSpecName { get; set; }



        /// <summary>
        /// 纯元素谱2 多条件空滤时存放  nip nicuni，多条件时使用
        /// </summary>
        [LengthAttribute(ColLength.ElementSpecName)]
        public abstract string ElementSpecNameNoFilter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public abstract bool DifferenceHelp { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public abstract int Color { get; set; }

        [LengthAttribute(ColLength.ColorHelper)]
        public abstract string ColorHelper { get; set; }

        /// <summary>
        /// 斜率优化因子
        /// </summary>
        public abstract double SlopeOptimalFactor { get; set; }

        /// <summary>
        /// 截距优化因子
        /// </summary>
        public abstract double InterceptOptimalFactor { get; set; }



        /// <summary>
        /// 是否为氧化物
        /// </summary>
        public abstract bool IsOxide { get; set; }


        /// <summary>
        /// 是否显示元素测量结果
        /// </summary>
        public abstract bool IsShowElement { get; set; }

        /// <summary>
        /// 是否显示含量结果
        /// </summary>
        public abstract bool IsShowContent { get; set; }

        public abstract bool IsAlert { get; set; }

        public abstract double Contentcoeff { get; set; }

        public abstract double ContentRealValue { get; set; }

        /// <summary>
        /// 在距离补正情况下的纯元素谱，存放谱库中计算的谱名称
        /// </summary>
        [LengthAttribute(ColLength.ElementSpecName)]
        public abstract string ElementEncoderSpecName { get; set; }

        #region Add By Cyq
        public abstract int ConditionID { get; set; }//测量条件ID cyq
        #endregion
        /// <summary>
        ///  影响纯元素边界
        /// </summary>
        public abstract bool IsBorderlineElem { get; set; }

        /// <summary>
        /// 显示自定义元素名
        /// </summary>
        public abstract bool IsShowDefineName { get; set; }

        /// <summary>
        /// 自定义元素名
        /// </summary>
        [LengthAttribute(ColLength.CurveElementCaption)]
        public abstract string DefineElemName { get; set; }


        ///// <summary>
        ///// 最小标准值
        ///// </summary>
        //public abstract int MinStandard { get; set; }

        ///// <summary>
        ///// 最大标准值
        ///// </summary>
        //public abstract int MaxStandard { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>       
        public abstract CurveElement Init(
          String Caption,
          bool IsDisplay,
          String Formula,
          int AtomicNumber,
          int LayerNumber,
          string LayerNumBackUp,
          XLine AnalyteLine,
          int PeakLow,
          int PeakHigh,
          int BaseLow,
          int BaseHigh,
          bool PeakDivBase,
          Double LayerDensity,
          Double Intensity,
          Double Content,
          Double Thickness,
          IntensityWay IntensityWay,
          bool IntensityComparison,
          double ComparisonCoefficient,
          int BPeakLow,
          int BPeakHigh,
          CalculationWay CalculationWay,
          FpCalculationWay FpCalculationWay,
          ElementFlag Flag,
          LayerFlag LayerFlag,
          ContentUnit ContentUnit,
          ThicknessUnit ThicknessUnit,
          string SReferenceElements,
          string SSpectrumData,
          string SInfluenceElements,
          string SInfluenceCoefficients,
          double Asrat,
          double Msthk,
          double Loi,
          double Limit,
          double K1,
          double K0,
          double Error,
          double ErrorK1,
          double ErrorK0,
            int RowsIndex,
            string ElementSpecName,
            bool DifferenceHelp,
            int Color,
            string ColorHelper,
            int ConditionID,
            string DistrubThreshold,
            bool IsOxide,
            bool IsShowElement,
            bool IsShowContent,
            bool IsAlert,
            double Contentcoeff,
            double ContentRealValue,
            string ElementEncoderSpecName,
            string ElementSpecNameNoFilter,
            string SSpectrumDataNotFilter,
            bool IsShowDefineName,
            string DefineElemName
        );

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }

        #endregion
    }
    [Serializable]
    public abstract class ElementRef : DbObjectModel<ElementRef>,ICloneable
    {
        /// <summary>
        /// 所属元素
        /// </summary>
        [BelongsTo, DbColumn("Element_Id")]
        public abstract CurveElement Element { get; set; }

        [LengthAttribute(ColLength.ElementRefName)]
        public abstract string Name { get; set; }

        public abstract bool IsRef { get; set; }

        public abstract double RefConf { get; set; }

        public abstract double DistrubThreshold { get; set; }

        public abstract ElementRef Init(string Name, bool IsRef, double RefConf);

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);

        }

        #endregion
    }
}