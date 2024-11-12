using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public abstract class SpecificationElement : DbObjectModel<SpecificationElement>
    {
        /// <summary>
        /// 元素记录所属的规格
        /// </summary>
        [BelongsTo, DbColumn("SpecificationsExample_Id")]
        public abstract SpecificationsExample SpecificationsExample { get; set; }

        /// <summary>
        /// 元素名称
        /// </summary>
        public abstract string ElementName { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public abstract double MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public abstract double MinValue { get; set; }
    }
}
