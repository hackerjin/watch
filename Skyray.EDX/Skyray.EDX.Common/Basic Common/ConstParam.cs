using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    public class ConstParam
    {
        /// <summary>
        /// 最大通道数
        /// </summary>
        private const int McaBin = 2048;
        /// <summary>
        /// 能量窗口 
        /// </summary>
        public const double EnergyWindow = 0.1;

        /// <summary>
        /// 测试条件最多5个
        /// </summary>
        public const int MAXCC = 5;
        /// <summary>
        /// 测试元素最多35个
        /// </summary>
        public const int MAXELT = 35;
        /// <summary>
        /// 镀层最多6层
        /// </summary>
        public const int MAXLAYER = 6;
        /// <summary>
        /// 测试的化合物最多35个
        /// </summary>
        public const int MAXCMP = 35;
        /// <summary>
        /// 标准样品最多30个
        /// </summary>
        public const int MAXSTD = 30;
        /// <summary>
        /// 一个化合物中最多包含7个元素
        /// </summary>
        public const int MAXCMPELT = 7;
        /// <summary>
        /// 样品的ID不能超过24个字符
        /// </summary>
        public const int MAXSIDCHRS = 24;
        /// <summary>
        /// 化合物的名称不能超过18个字符
        /// </summary>
        public const int MAXCMPCHRS = 18;
        /// <summary>
        /// 去卷积中元素不能大于25个
        /// </summary>
        public const int MAXELTS = 25;
        /// <summary>
        /// 通道不能大于4096
        /// </summary>
        public const int SPECSIZE = 4096;

        /// <summary>
        /// 标样最小个数
        /// </summary>
        public const int StandSampleMinCount = 2;
    }
}
