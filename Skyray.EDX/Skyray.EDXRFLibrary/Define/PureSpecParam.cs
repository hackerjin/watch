using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;


namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class PureSpecParam : DbObjectModel<PureSpecParam>
    {
        /// <summary>
        /// 所属测量条件
        /// </summary>
        [BelongsTo, DbColumn("Condition_Id"), Auto("Condition", "所属条件")]
        public abstract Condition Condition { get; set; }

        /// <summary>
        /// 谱名称
        /// </summary>
        public abstract string Name { get; set; }


        /// <summary>
        /// 谱测量的高度
        /// </summary>
        public abstract double Height { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public abstract string DeviceName { get; set; }


        public abstract double TotalCount { get; set; }
        /// <summary>
        /// 工作曲线名称
        /// </summary>
        [AllowNull]
        public abstract string WorkCurveName { get; set; }

        /// <summary>
        /// 谱标识
        /// </summary>
        public abstract SpecType SpecTypeValue { get; set; }

        /// <summary>
        /// 样品名称
        /// </summary>
        public abstract string SampleName { get; set; }

        ///// <summary>
        ///// 保存的二进制流
        ///// </summary>
        //public abstract Byte[] Data { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public abstract DateTime? SpecDate { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public abstract double UsedTime { get; set; }

        

        public abstract string ElementName { get;set; }

        public abstract double Current { get; set; }

        public abstract double CurrentUnifyCount { get; set; }

        public abstract PureSpecParam Init(string Name, double Height, string DeviceName, double TotalCount,double CurrentUnifyCount,string WorkCurveName,
         SpecType SpecTypeValue, string SampleName, DateTime? SpecDate);

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }
        #endregion
    }
}
