using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Linq
{
    public static class BasicTypeExtension
    {
        //public static List<int> GetNumList(this int maxNum, int startNum)
        //{
        //    var lst = new List<int>();
        //    while (startNum < maxNum)
        //    {
        //        lst.Add(startNum++);
        //    }
        //    return lst;
        //}

        //public static List<int> GetNumList(this int maxNum)
        //{
        //    return maxNum.GetNumList(0);
        //}
        /// <summary>
        /// 判断字符串数据是否为数字
        /// </summary>
        /// <param name="value">传入字符串数据本身</param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        /// <summary>
        /// 判断字符串数据是否为整数部分为一位小数位为一位以上的数据
        /// </summary>
        /// <param name="str">传入字符串数据本身</param>
        /// <returns></returns>
        public static bool IsNum(this string str)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+\b$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 判断整型数据是否在指定的整型数据区段内
        /// </summary>
        /// <param name="value">传入整型需要验证的数据本身</param>
        /// <param name="min">传入整型区段最小值</param>
        /// <param name="max">传入整型区段最大值</param>
        /// <returns></returns>
        public static bool IsBetween(this int value, int min, int max)
        {
            return value > min && value < max;
        }

        /// <summary>
        /// 判断是否为整型数据
        /// </summary>
        /// <param name="value">传入字符串数据</param>
        /// <returns></returns>
        public static bool IsInt(this string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        /// <summary>
        /// 判断是否为无符号型数据
        /// </summary>
        /// <param name="value">传入字符串数据</param>
        /// <returns></returns>
        public static bool IsUnsign(this string value)
        {
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }

        /// <summary>
        /// 对浮点型数据四舍五入为整数
        /// </summary>
        /// <param name="f">传入浮点型数据本身</param>
        /// <returns></returns>
        public static int GetNearInt(this float f)
        {
            return (int)Math.Round(f, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 对双精度数据四舍五入为整数
        /// </summary>
        /// <param name="d">传入双精度数据本身</param>
        /// <returns></returns>
        public static int GetNearInt(this double d)
        {
            return (int)Math.Round(d, 2, MidpointRounding.AwayFromZero);
        }
    }
}
