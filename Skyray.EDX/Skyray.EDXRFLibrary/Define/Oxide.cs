using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Skyray.EDXRFLibrary.Define;

namespace Skyray.EDXRFLibrary.Define
{
    [Serializable]
    public abstract class Oxide : DbObjectModel<Oxide>
    {
        [BelongsTo, DbColumn("Atom_Id")]
        public abstract Atom Atom { get; set; }


        /// <summary>
        /// 氧化物名称
        /// </summary>
        [Length(ColLength.OxideName)]
        public abstract string OxideName { get; set; }

        /// <summary>
        /// 氧化物英文名称
        /// </summary>
        [Length(ColLength.OxideNameEN)]
        public abstract string OxideNameEN { get; set; }

        /// <summary>
        /// 氧化物中文名称
        /// </summary>
        [Length(ColLength.OxideNameCN)]
        public abstract string OxideNameCN { get; set; }

        public abstract Oxide Init(Atom Atom,string OxideName, string OxideNameEN, string OxideNameCN);
    }
}
