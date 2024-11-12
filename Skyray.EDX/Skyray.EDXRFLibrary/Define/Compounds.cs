using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data.Definition;
using Lephone.Util;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class Compounds : DbObjectModel<Compounds>,ICloneable
    {
        /// <summary>
        /// 所属工作曲线
        /// </summary>
        [BelongsTo, DbColumn("WorkCurve_Id")]
        public abstract WorkCurve WorkCurve { get; set; }

        [Length(ColLength.CompoundsName)]
        public abstract string Name { get; set; }   //化合物名称

        public abstract Compounds Init(string name);

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }

        #endregion
    }
}
