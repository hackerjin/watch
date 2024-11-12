using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data;

using Skyray.EDXRFLibrary;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract partial class WorkCurve : DbObjectModel<WorkCurve>,ICloneable
    {
        /// <summary>
        /// 所属测量条件
        /// </summary>
        [BelongsTo, DbColumn("Condition_Id")]
        public abstract Condition Condition { get; set; }
        /// <summary>
        /// 所属工作区
        /// </summary>
        [BelongsTo, DbColumn("WorkRegion_Id")]
        public abstract WorkRegion WorkRegion { get; set; }
        /// <summary>
        /// 包含元素列表
        /// </summary>
        [HasOne(OrderBy = "Id")]
        public abstract ElementList ElementList { get; set; }
        /// <summary>
        /// 包含校正参数
        /// </summary>
        [HasOne(OrderBy = "Id")]
        public abstract CalibrationParam CalibrationParam { get; set; }

        /// <summary>
        /// 包含校正参数
        /// </summary>
        [HasOne(OrderBy = "Id")]
        public abstract SpecialRemoveSpec SpecialRemoveParam { get; set; }
        /// <summary>
        /// 包含自定义化合物
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<Compounds> Compounds { get; set; }

        /// <summary>
        /// 包含强度偏移校正的数据
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<IntensityCalibration> IntensityCalibration { get; set; }
        /// <summary>
        /// 曲线名称
        /// </summary>
        [Length(ColLength.WorkCurveName)]
        public abstract string Name { get; set; }
        /// <summary>
        /// 测量名称
        /// </summary>
        [Length(ColLength.ConditionName)]
        public abstract string ConditionName { get; set; }
        /// <summary>
        /// 计算方法
        /// </summary>
        public abstract CalcType CalcType { get; set; }
        /// <summary>
        /// 功能类型
        /// </summary>
        public abstract FuncType FuncType { get; set; }
        /// <summary>
        /// 是否使用吸收法
        /// </summary>
        public abstract bool IsAbsorb { get; set; }
        /// <summary>
        /// 测厚是否显示含量
        /// </summary>
        public abstract bool IsThickShowContent { get; set; }
        /// <summary>
        /// 极限厚度
        /// </summary>
        public abstract double LimitThickness { get; set; }
        /// <summary>
        /// 扣背景
        /// </summary>
        public abstract bool RemoveBackGround { get; set; }
        /// <summary>
        /// 去和峰
        /// </summary>
        public abstract bool RemoveSum { get; set; }
        /// <summary>
        /// 去逃逸峰
        /// </summary>
        public abstract bool RemoveEscape { get; set; }

        public abstract bool IsDefaultWorkCurve { get; set; }
        /// <summary>
        /// 参与匹配
        /// </summary>
        public abstract bool IsJoinMatch { get; set; }

        /// <summary>
        /// 相似曲线
        /// </summary>
        public abstract int SimilarCurveId { get; set; }

        /// <summary>
        /// 相似曲线名
        /// </summary>
        [AllowNull]
        public abstract string SimilarCurveName { get; set; }

        /// <summary>
        /// 测厚是否显示质量密度（mg/cm^2）2013-06-24
        /// </summary>
        public abstract bool IsThickShowAreaThick { get; set; }
       
        ///// <summary>
        ///// 面密度的基单位 1,mg/cm^2  ，2 自定义
        ///// </summary>
        public abstract string AreaThickType { get; set; }

        /// <summary>
        /// 测厚是否显示质量密度（mg/cm^2）2013-06-24
        /// </summary>
        public abstract bool IsCurrentNormalize { get; set; }


        [AllowNull]
        public abstract string MainElements { get; set; }

        public abstract int InCalType { get; set; }//测量校正类型
        [AllowNull]
        public abstract string InCalSampName { get; set; }//校正的样品名,高标 1,α，2，β
        [AllowNull]
        public abstract string InCalSampNameL { get; set; }//校正的样品名，低标

        
        public double ContRatio = 35;//rohs用的固定的经验系数
        [AllowNull]
        public abstract string RemarkName { get; set; } // thick 用作名称备注

        [AllowNull]
        public abstract string ThickStandardName { get; set; }  //thick用作标准绑定曲线

        public abstract int TestTime { get; set; }

        public abstract bool IsShowMain { get; set; }  //是否在主界面显示工作曲线


        public abstract bool IsNiP2 { get; set; }

        public abstract bool IsBaseAdjust { get; set; }  //是否启用基材校正

        public abstract WorkCurve Init(string Name,
            string ConditionName,
            CalcType CalcType,
            FuncType FuncType,
            bool IsAbsorb,
            double LimitThickness, bool RemoveBackGround, bool RemoveSum, bool RemoveEscape, bool IsDefaultWorkCurve, bool IsJoinMatch, int SimilarCurveId, string SimilarCurveName, bool IsThickShowAreaThick, string AreaThickType, int TestTime, bool IsShowMain);
        public override string ToString()
        {
            //return base.ToString();
            return Name;
            
        }

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }

        public byte[] Serialize()
        {
            return BaseObject.Serialize(this);
        }

        #endregion
    }
}
