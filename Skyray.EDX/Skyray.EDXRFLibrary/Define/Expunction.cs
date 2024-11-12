using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 消去值
    /// </summary>
    [Serializable]
    public abstract class Expunction : DbObjectModel<Expunction>
    {
        /// <summary>
        /// 所属元素
        /// </summary>
        [BelongsTo, DbColumn("CurveElement_Id")]
        public abstract CurveElement CurveElement { get; set; }
        public abstract string ElementName { get; set; }
        public abstract double Ka { get; set; }
        public abstract double Kb { get; set; }
        public abstract double La { get; set; }
        public abstract double Lb { get; set; }
        public abstract double Lr { get; set; }
        public abstract double Le { get; set; }
    }
}
