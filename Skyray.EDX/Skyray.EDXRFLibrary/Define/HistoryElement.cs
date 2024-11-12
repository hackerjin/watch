using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public abstract class HistoryElement : DbObjectModel<HistoryElement>
    {
        [BelongsTo, DbColumn("HistoryRecord_Id")]
        public abstract HistoryRecord HistoryRecord { get; set; }

        public abstract string elementName { set; get; }

        public abstract string contextelementValue { set; get; }

        /// <summary>
        /// 元素全面积
        /// </summary>
        public abstract double ElementIntensity { get; set; }

        //计算强度
        public abstract double CaculateIntensity { get; set; }

        //[AllowNull]
        public abstract long customstandard_Id { set; get; }

        //[AllowNull]
        public abstract int unitValue { set; get; }

        public abstract int thickunitValue { set; get; }

        public abstract string thickelementValue { set; get; }

        public abstract string densityelementValue { set; get; }

        public abstract string densityunitValue { set; get; }

        /// <summary>
        /// 误差
        /// </summary>
        public abstract double Error { get; set; }

        public abstract double AverageValue { get; set; }

        //真实值
        public abstract string contentRealelemValue { get; set; } 
    }
}