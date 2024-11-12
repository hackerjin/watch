using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public abstract class SpecAdditional : DbObjectModel<SpecAdditional>
    {
        /// <summary>
        /// 工作曲线的ID
        /// </summary>
        public abstract long SpecListId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract Byte[] GraphicData { get; set; }

        [AllowNull]
        public abstract string Additional0 { get; set; }
        [AllowNull]
        public abstract string Additional1 { get; set; }
        [AllowNull]
        public abstract string Additional2 { get; set; }
        [AllowNull]
        public abstract string Additional3 { get; set; }
        [AllowNull]
        public abstract string Additional4 { get; set; }
        [AllowNull]
        public abstract string Additional5 { get; set; }
        [AllowNull]
        public abstract string Additional6 { get; set; }
    }
}
