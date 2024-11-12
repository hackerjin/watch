using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public abstract class SpecificationsExample : DbObjectModel<SpecificationsExample>
    {
        /// <summary>
        /// 规格名称
        /// </summary>
        public abstract string ExampleName { get; set; }

        /// <summary>
        /// 规格开始日期
        /// </summary>
        public abstract DateTime? CreateExampleDate { get; set; }

        /// <summary>
        /// 更新规格日期
        /// </summary>
        public abstract DateTime? UpdateExampleDate { get; set; }

        /// <summary>
        /// 规格类型
        /// </summary>
        public abstract string ExampleType { get; set; }

        /// <summary>
        /// 所属规格类别
        /// </summary>
        [BelongsTo, DbColumn("SpecificationsCategory_Id")]
        public abstract SpecificationsCategory Category { get; set; }

        /// <summary>
        /// 包含
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<SpecificationElement> IncludeElements { get; set; }
    }
}
