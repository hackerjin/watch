using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class ReferenceElement : DbObjectModel<ReferenceElement>
    {

        [BelongsTo, DbColumn("Element_Id")]
        public abstract CurveElement Element { get; set; }

        /// <summary>
        /// 主元素
        /// </summary>
        public abstract string MainElementName { get; set; }

        /// <summary>
        /// 拟合元素
        /// </summary>
        public abstract string ReferenceElementName { get; set; }

        /// <summary>
        /// 拟合左界
        /// </summary>
        public abstract int ReferenceLeftBorder { get; set; }

        /// <summary>
        /// 拟合右界
        /// </summary>
        public abstract int ReferenceRightBorder { get; set; }

        /// <summary>
        /// 背左界
        /// </summary>
        public abstract int ReferenceBackLeft { get; set; }

        /// <summary>
        /// 背右界
        /// </summary>
        public abstract int ReferenceBackRight { get; set; }

        /// <summary>
        /// 峰背比
        /// </summary>
        public abstract bool PeakToBack { get; set; }

        public abstract ReferenceElement Init(string MainElementName, string ReferenceElementName, int ReferenceLeftBorder, int ReferenceRightBorder,
            int ReferenceBackLeft, int ReferenceBackRight, bool PeakToBack);

    }
}
