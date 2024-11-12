using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public abstract class SpecificationsCategory : DbObjectModel<SpecificationsCategory>
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        public abstract string CategoryName { get; set; }

        /// <summary>
        /// 是否为当前类别
        /// </summary>
        public abstract bool IsCurrentCategory { get; set; }

        ///<summary>
        /// 包含规格
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<SpecificationsExample> Specifications { get; set; }
    }
}
