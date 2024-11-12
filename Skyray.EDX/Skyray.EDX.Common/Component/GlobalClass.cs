using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common.Component
{
    class GlobalClass
    {
        //最小计数率
        public static int smallCount = 200;
        //第一次反映时间
        public static int firstTime = 10;
        //是否自动匹配
        public static int isAuto = 0;
        //最大管流
        public static int MaxCurrent = 100;
        //分析因子
        public static List<string> ListAnalysisFactor = new List<string>();

        public static double ErrorRate = 100;//牌号分析中的被除数

        public static bool isMultTest = false;

        public static List<object[]> listRefereceSpec = new List<object[]>();  //虚拟存放


        public static int MatchNum = 30;//qjl by 20101122

        public static int SmoothNum = 5;//平滑次数

        //public static string version;//软件版本号
    }

}
