using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class WorkRegion : DbObjectModel<WorkRegion>,ICloneable
    {
        /// <summary>
        /// 工作区名称
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public abstract string Caption { get; set; }
        /// <summary>
        /// 工作区类型
        /// </summary>
        public abstract RohsSampleType RohsSampleType { get; set; }
        /// <summary>
        /// 包含工作曲线
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<WorkCurve> WorkCurves { get; set; }
        /// <summary>
        /// 是否默认工作区
        /// </summary>
        public abstract bool IsDefaultWorkRegion { get; set; }

        public abstract WorkRegion Init(string Name, RohsSampleType RohsSampleType, bool IsDefaultWorkRegion, string Caption);

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }

        #endregion
    }
}
