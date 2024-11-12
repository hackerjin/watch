using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SourceGrid;

namespace Skyray.Print
{
    /// <summary>
    /// 列信息记录类
    /// </summary>
    [Serializable]
    public class ColInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Caption
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visiable { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 单元格字体
        /// </summary>
        public Font CellFont { get; set; }
        /// <summary>
        /// 表头字体
        /// </summary>
        public Font HeaderFont { get; set; }
        /// <summary>
        /// 异常字体
        /// </summary>
        public Color ExceptionColor { get; set; }

        //public int ColIndex { get; set; }
        ///// <summary>
        ///// 本列自动宽度
        ///// </summary>
        //public bool AutoWidth { get; set; }

        /// <summary>
        /// 对应Grid Column 信息
        /// </summary>
        [NonSerialized]
        public ColumnInfo GridColInfo;
        /// <summary>
        /// 是否需要重新排版Grid
        /// </summary>
        [NonSerialized]
        public bool NeedReLayout = false;
    }
}
