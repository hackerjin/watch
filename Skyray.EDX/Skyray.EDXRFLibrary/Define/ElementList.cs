using Lephone.Data.Definition;
using Lephone.Util;
using System;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class ElementList : DbObjectModel<ElementList>,ICloneable
    {
        /// <summary>
        /// 所属工作曲线
        /// </summary>
        [BelongsTo, DbColumn("WorkCurve_Id")]
        public abstract WorkCurve WorkCurve { get; set; }
        /// <summary>
        /// 包含元素信息
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<CurveElement> Items { get; set; }
        /// <summary>
        /// 包含自定义项
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<CustomField> CustomFields { get; set; }
        [Auto("归一含量")]
        public abstract bool IsUnitary { get; set; }
        public abstract double UnitaryValue { get; set; }  //<归一值,<=0则不归一 
        public abstract double TubeWindowThickness { get; set; }  //<归一值,<=0则不归一 
        public abstract bool RhIsLayer { get; set; }       //Rh是镀层
        public abstract double RhLayerFactor { get; set; }   //Rh镀层因子     modify by chuyaqin
        public abstract bool RhIsMainElementInfluence { get; set; }       //Rh是镀层时，是否只影响主元素
        public abstract bool IsAbsorption { get; set; }
        public abstract ThCalculationWay ThCalculationWay { get; set; }//测厚是否用吸收法
        public abstract double DBlLimt { get; set; }   //极限值     modify by chuyaqin  
        public abstract bool IsRemoveBk { get; set; }
        [AllowNull]
        public abstract string SpecListName { get; set; }
        public abstract bool IsReportCategory { get; set; }//是否报规格
        public abstract string MatchSpecListIdStr { get; set; }
        public abstract string RefSpecListIdStr { get; set; } //对比谱的添加
        /// <summary>
        /// 纯元素谱作为无限厚点加入
        /// </summary>
        public abstract bool PureAsInfinite { get; set; }
        public abstract bool NoStandardAlert { get; set; }

        public abstract double AreaDensity { get; set; }

        //2014-01-20PK
        public static bool IsPKCatchValue = false;

        /// <summary>
        /// 选择某一元素作为主元素来计算K值
        /// </summary>
        [AllowNull]
        public abstract string MainElementToCalcKarat { get; set; } //@CYR180428

        /// <summary>
        /// 选择选择某元素为镀层元素 元素以逗号隔开（贵金属），仅适用于单层
        /// </summary>
        public abstract string LayerElemsInAnalyzer { get; set; } //20190409

        /// <summary>
        /// 初始化
        /// </summary>        
        public abstract ElementList Init(
           bool IsUnitary,
           double UnitaryValue,
           double TubeWindowThickness,
           bool RhIsLayer,
           double RhLayerFactor,
           double DBlLimt,
           bool IsReportCategory);

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }

        #endregion

        public double GetAreaDensity(int layNumber,string sampleName)
        {
            double dbldensity = 0;
            double dblSumY = 0;
            double tempCal = 0;
            for (int tempi = 0; tempi < this.Items.Count; tempi++)
            {
                if (this.Items[tempi].LayerNumber == layNumber)
                {
                    int tempsi=0;
                    for(tempsi=0;tempsi<this.Items[tempi].Samples.Count;tempsi++)
                    {
                        if(this.Items[tempi].Samples[tempsi].SampleName.Equals(sampleName))
                        {
                            break;
                        }
                    }
                    StandSample stemp = this.Items[tempi].Samples[tempsi];
                    if (stemp != null && stemp.Density > 0 && stemp.Y != null && double.Parse(stemp.Y) > 0)
                    {
                        tempCal += double.Parse(stemp.Y) / stemp.Density;
                        dblSumY += double.Parse(stemp.Y);
                    }
                }
            }
            if (tempCal > 0)
                dbldensity = dblSumY / tempCal;
            return dbldensity;
        }
        
        public double GetAreaDensity(int layNumber)
        {
            //计算密度
            double dblSumY = 0;
            double tempCal = 0;
            double dblThickness=0;
            for (int tempi = 0; tempi < this.Items.Count; tempi++)
            {
                //设置了层密度的情况
                //if (this.Items[tempi].LayerNumber == layNumber && this.Items[tempi].LayerDensity > 0.000001)
                //{
                //    dblThickness = this.Items[tempi].Thickness;
                //    return dblThickness * this.Items[tempi].LayerDensity;
                //}
               // if (this.Items[tempi].LayerNumber == layNumber && this.Items[tempi].LayerDensity > 0.000001)
               // {
               //     dblThickness = this.Items[tempi].Thickness;
               //     return dblThickness * this.Items[tempi].LayerDensity;
               // }
               //else
                    if (this.Items[tempi].LayerNumber == layNumber)//未设置层密度，则算默认密度
                {
                    double AtomDensity=Atoms.AtomList.Find(w=>w.AtomID==this.Items[tempi].AtomicNumber).AtomDensity;
                    tempCal += this.Items[tempi].Content / AtomDensity;
                    dblSumY += this.Items[tempi].Content;
                    dblThickness=this.Items[tempi].Thickness;
                    Console.WriteLine("content :{0}--{1}", Items[tempi].Content, this.Items[tempi].Content);
                    Console.WriteLine("tempCal: {0} , dblsumY: {1} , dblThickness: {2} ---- AtomDensity:{3}", tempCal, dblSumY, dblThickness, AtomDensity);
                }
            }
            //计算面密度
           // return tempCal <= 0 ? 0 : dblThickness * dblSumY / (tempCal*10000);  //元素密度：g/cm^3  um*g/cm^3-->g/cm^2
          //  dblSumY = dblSumY > 100 ? 100 : dblSumY;
            Console.WriteLine("dblThickness: " + (dblThickness * dblSumY / (tempCal)).ToString());
            return tempCal <= 0 ? 0 : dblThickness * dblSumY / (tempCal);  //元素密度：g/cm^3  um*g/cm^3-->g/cm^2
        }

    }
}