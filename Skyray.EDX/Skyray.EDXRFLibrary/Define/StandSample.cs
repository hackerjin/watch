using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;


namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class StandSample : DbObjectModel<StandSample>
    {
        [BelongsTo, DbColumn("Element_Id")]
        public abstract CurveElement Element { get; set; }

        /// <summary>
        /// 标样名称
        /// </summary>
        [LengthAttribute(ColLength.SampleName)]
        public abstract string SampleName { get; set; }


        public abstract string Height { get; set; }


        public abstract string CalcAngleHeight { get; set; }
        /// <summary>
        /// X 强度
        /// </summary>
        public abstract string X { get; set; }

        /// <summary>
        /// Y 含量
        /// </summary>
        public abstract string Y { get; set; }

        /// <summary>
        /// 厚度
        /// </summary>
        public abstract string Z { get; set; }
        /// <summary>
        /// 校正强度
        /// </summary>
        public abstract double TheoryX { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public abstract bool Active { get; set; }

        /// <summary>
        /// 元素名称
        /// </summary>
        [LengthAttribute(ColLength.ElementName)]
        public abstract string ElementName { get; set; }

        //public abstract long SpecListId { get; set; }
        //public abstract string SpecListName { get; set; }

        /// <summary>
        /// 所在层
        /// </summary>
        public abstract int Layer { get; set; }

        /// <summary>
        /// 总层数
        /// </summary>
        public abstract int TotalLayer { get; set; }

        public abstract string Level { get; set; }

        //chuyaqin add 2010-0513
        public abstract double Density { get; set; }
        //chuyaqin add 2010-0513
        public abstract double Thickness { get; set; }
        //chuyaqin add 2010-0513
        /// <summary>
        /// 理论强度
        /// </summary>
        public abstract double IntensityC { get; set; }

        public abstract bool IsMatch { get; set; }//标样是否参与匹配
        //public abstract long MatchSpecListId { get; set; }//参与匹配的匹配谱
        [AllowNull]
        public abstract string MatchSpecName { get; set; }//名称

        public abstract string Uncertainty { get; set; }//不确定度

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleName"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public abstract StandSample Init(string SampleName,
                                            string Height,
                                            string CalcAngleHeight,
                                            string X,
                                            string Y,
                                            string Z,
                                            bool Active,
                                            string ElementName,
            int Layer, int TotalLayer, string Level,double Density,string Uncertainty
                                            );
    }
}
