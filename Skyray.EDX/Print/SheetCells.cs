using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.Print
{
    /// <summary>
    /// CellCotrol类
    /// </summary>
    public class CellControl
    {
        /// <summary>
        /// PrintCtrl类属性
        /// </summary>
        public PrintCtrl PrintCtrl { get; set; }
        /// <summary>
        /// 控件类型属性数组
        /// </summary>
        public CtrlType[] Types { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Point Point { get; set; }
        /// <summary>
        /// Label矩形
        /// </summary>
        public Rectangle LabelRect { get; set; }
        /// <summary>
        /// Field矩形
        /// </summary>
        public Rectangle FieldRect { get; set; }
        /// <summary>
        /// Client矩形
        /// </summary>
        public Rectangle ClientRect { get; set; }

        //public int DataIndex { set; get; }
        /// <summary>
        /// 行索引
        /// </summary>
        public IndexInfo RowIndexs { get; set; }
        /// <summary>
        /// 列索引
        /// </summary>
        public IEnumerable<int> ColIndexs { get; set; }

        /// <summary>
        /// 相对纵坐标值
        /// </summary>
        public int RelativeY { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public CellControl()
        {
            Types = new CtrlType[2];
        }
    }
}
