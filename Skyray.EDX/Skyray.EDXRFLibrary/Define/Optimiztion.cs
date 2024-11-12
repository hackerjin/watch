using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class Optimiztion : DbObjectModel<Optimiztion>
    {
        /// <summary>
        /// 所属元素
        /// </summary>
        [BelongsTo, DbColumn("Element_Id")]
        public abstract CurveElement element { get; set; }

        public abstract double OptimiztionValue { get; set; }//优化值

        public abstract double OptimiztionRange { get; set; }//范围

        public abstract double OptimiztionFactor { get; set; }//因子

        public abstract int OptimizetionType { get; set; } //优化1为0，优化2为1,优化3为2

        //优化最大值
        public abstract double OptimiztionMax { get; set; }

        //优化最小值
        public abstract double OptimiztionMin { get; set; }

        public abstract string OptExpression { get; set; }

        /// <summary>
        /// 是否参与影响计算强度
        /// </summary>
        public abstract bool IsJoinIntensity { get; set; }


        public abstract Optimiztion Init(double OptimiztionValue, double OptimiztionRange, double OptimiztionFactor, double OptimiztionMax, double OptimiztionMin, string OptExpression, bool IsJoinIntensity);
    }
}
