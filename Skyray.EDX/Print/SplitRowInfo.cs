using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Print
{
    /// <summary>
    /// 行信息记录类
    /// </summary>
    public class SplitRowInfo
    {
        public SplitTableType SplitTableType { get; set; }
        public IndexInfo Indexs { get; set; }
    }
}
