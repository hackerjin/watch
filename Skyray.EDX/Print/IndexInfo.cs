using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Print
{
    /// <summary>
    /// 起始索引记录类
    /// </summary>
    public class IndexInfo
    {
        /// <summary>
        /// 开始索引
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 结束索引
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public IndexInfo()
        {

        }
        /// <summary>
        /// 索引数量
        /// </summary>
        public int Count
        {
            get { return End - Start + 1; }
        }
        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public IndexInfo(int startIndex, int endIndex)
        {
            Start = startIndex;
            End = endIndex;
        }
    }
}
