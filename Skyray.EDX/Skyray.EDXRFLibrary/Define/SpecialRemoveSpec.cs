using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class SpecialRemoveSpec : DbObjectModel<SpecialRemoveSpec>, ICloneable
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
        public abstract HasMany<SpecialRemoveItem> RemoveItems { get; set; }

        /// <summary>
        /// 背左界
        /// </summary>
        public abstract int BaseLow { get; set; }
        /// <summary>
        /// 背右界 
        /// </summary>
        public abstract int BaseHigh { get; set; }
         /// <summary>
        /// 初始化
        /// </summary>        
        public abstract SpecialRemoveSpec Init(
           int BaseLow,
           int BaseHigh);

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }

        #endregion

    }

    [Serializable]
    public abstract class SpecialRemoveItem : DbObjectModel<SpecialRemoveItem>, ICloneable
    {
        /// <summary>
        /// 所属工作曲线
        /// </summary>
        [BelongsTo, DbColumn("SpecialRemoveSpec_Id")]
        public abstract SpecialRemoveSpec SpecialRemoveSpec { get; set; }

        /// <summary>
        /// 元素或化合物名称
        /// </summary>
        [LengthAttribute(ColLength.CurveElementCaption)]
        public abstract String Caption { get; set; }
        /// <summary>
        /// 峰左界
        /// </summary>
        public abstract int PeakLow { get; set; }
        /// <summary>
        /// 峰右界
        /// </summary>
        public abstract int PeakHigh { get; set; }

        /// <summary>
        /// 峰右界
        /// </summary>
        public abstract double AreaLimit { get; set; }

        public abstract SpecialRemoveItem Init(
           String Caption,
           int PeakLow,
           int PeakHigh,
           double AreaLimit);

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }

        #endregion
 
    }
}
