using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common.Component
{
    public class ResultStandard
    {

        /// <summary>
        /// 曲线名称
        /// </summary>
        public string CurveName { get; set; }

        /// <summary>
        /// 判定元素
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// 显示内容
        /// </summary>
        public string DisplayText { get; set; }

        public static ResultStandard Default
        {
            get
            {
                return new ResultStandard()
                {
                    CurveName = "Au_Curve",
                    Caption = "Au",
                    Max = 100,
                    Min = 0,
                    DisplayText = "OK"
                };
            }
        }

        public string ParseToString()
        {
            return CurveName + "," + Caption + "," + Min + "," + Max + "," + DisplayText;
        }

        public static ResultStandard ParseFromString(string str)
        {
            try
            {
                string[] strArray = str.Split(',');
                return new ResultStandard()
                {
                    CurveName = strArray.ElementAt(0),
                    Caption = strArray.ElementAt(1),
                    Min = int.Parse(strArray.ElementAt(2)),
                    Max = int.Parse(strArray.ElementAt(3)),
                    DisplayText = strArray.ElementAt(4)
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }

    //public class Range
    //{

    //    public override string ToString()
    //    {
    //        return Min.ToString() + "-" + Max.ToString();
    //    }
    //}
}
