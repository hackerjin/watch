using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Print
{
    /// <summary>
    /// 分组信息记录类
    /// </summary>
    public class GroupInfo
    {
        /// <summary>
        /// IndexInfo类型属性 记录索引起始
        /// </summary>
        public IndexInfo CtrlIndex { get; set; }

        /// <summary>
        /// 起始纵坐标
        /// </summary>
        public int StartY { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 顶点坐标
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// 底部坐标
        /// </summary>
        public int Bottom { get; set; }
        /// <summary>
        /// PrintTable集合属性
        /// </summary>
        public List<PrintTable> Tables { get; set; }

        /// <summary>
        /// 构造函数初始化PrintTable集合
        /// </summary>
        public GroupInfo()
        {
            Tables = new List<PrintTable>();
        }
    }
}
