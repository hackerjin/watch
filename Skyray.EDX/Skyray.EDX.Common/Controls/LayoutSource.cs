using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;


namespace Skyray.EDX.Common
{
    public abstract class LayoutSource : DbObjectModel<LayoutSource>
    {
        /// <summary>
        /// 是否显示保存和取消按钮
        /// </summary>
        public abstract bool ShowSaveCancelButton { get; set; }
        /// <summary>
        /// 是否显示标题框
        /// </summary>
        public abstract bool ShowGroupBox { get; set; }
        /// <summary>
        /// 是否为排除模式
        /// </summary>
        public abstract bool BExclude { get; set; }
        /// <summary>
        /// 排除字段名，用,隔开
        /// </summary>
        public abstract string ExcludeFields { get; set; }
        /// <summary>
        /// 包含字段名，用,隔开
        /// </summary>
        public abstract string IncludeFields { get; set; }
        /// <summary>
        /// 列数
        /// </summary>
        public abstract int ColCount { get; set; }

        public abstract LabelPosition LabelPos { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ShowSaveCancelButton"></param>
        /// <param name="ShowGroupBox"></param>
        /// <param name="BExclude"></param>
        /// <param name="ExcludeFields"></param>
        /// <param name="IncludeFields"></param>
        /// <param name="ColCount"></param>
        /// <returns></returns>
        public abstract LayoutSource Init(bool ShowSaveCancelButton,
            bool ShowGroupBox,
            bool BExclude,
            string ExcludeFields,
            string IncludeFields,
            int ColCount,
            LabelPosition LabelPos);
    }
}
