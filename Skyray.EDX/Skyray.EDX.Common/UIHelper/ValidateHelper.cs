using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Skyray.EDX.Common
{
    public  class ValidateHelper
    {
        public static readonly string[] IllegalChars = new string[] { "/", @"\", "?",  ":", "<", ">",  "|","*","''"};
        /// <summary>
        /// 非法验证
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static bool IllegalCheck(Control ctrl)
        {
            bool res = true;
            string InputCharacters = ctrl.Text.TrimEnd().TrimStart();
            if (string.IsNullOrEmpty(InputCharacters))
            {
                res = false;
                Msg.Show(Info.IsNull);
            }
            else
            {
                foreach (string c in IllegalChars)
                {
                    if (InputCharacters.Contains(c))
                    {
                        Msg.Show(Info.IllegalInput);
                        res = false;
                        break;
                    }
                }
            }
            return res;
        }

        protected bool IsChineseLetter(string input, int index)
        {
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            if (input != "")
            {
                //获得字符串input中指定索引index处字符unicode编码
                code = Char.ConvertToUtf32(input, index);

                if (code >= chfrom && code <= chend)
                {
                    return true; //当code在中文范围内返回true
                }
                else
                {
                    return false; //当code不在中文范围内返回false
                }
            }
            return false;
        }

        public bool IsChina(string CString)
        {
            bool BoolValue = false;
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                {
                    BoolValue = false;
                }
                else
                {
                    return BoolValue = true;
                }
            }
            return BoolValue;
        }
        /// <summary>
        /// 判断字符串包含中文
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool StringContainCn(string str)
        {
            bool b = false;
            Regex rx = new Regex("^[\u4e00-\u9fa5]$");
            for (int i = 0; i < str.Length; i++)
            {
                b = b || rx.IsMatch(str[i].ToString());
                if (b) break;
            }
            return b;
        }

        /// <summary>
        /// 判断字符串全部为中文
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool StringIsCn(string str)
        {
            bool b = true;
            Regex rx = new Regex("^[\u4e00-\u9fa5]$");
            for (int i = 0; i < str.Length; i++)
            {
                b = b && rx.IsMatch(str[i].ToString());
                if (!b) break;
            }
            return b;
        }

        ///// <summary>
        /// 判断句子中是否含有中文
        /// </summary>
        /// <param >字符串</param>
        public static bool WordsIsCN(string words)
        {
            string TmmP;
            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);
                byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);
                if (sarr.Length == 2)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 给定一个字符串，判断其是否只包含有汉字
        /// </summary>
        /// <param name="testStr"></param>
        /// <returns></returns>
        public static bool IsOnlyContainsChinese(string testStr)
        {
            char[] words = testStr.ToCharArray();
            foreach (char word in words)
            {
                if (IsGBCode(word.ToString()) || IsGBKCode(word.ToString()))  // it is a GB2312 or GBK chinese word
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// 判断一个word是否为GB2312编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(word);
            if (bytes.Length <= 1)
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                //判断是否是GB2312
                if (byte1 >= 176 && byte1 <= 247 && byte2 >= 160 && byte2 <= 254)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBKCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254)     //判断是否是GBK编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// 判断一个word是否为Big5编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsBig5Code(string word)
        {
            byte[] bytes = Encoding.GetEncoding("Big5").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if ((byte1 >= 129 && byte1 <= 254) && ((byte2 >= 64 && byte2 <= 126) || (byte2 >= 161 && byte2 <= 254)))  //判断是否是Big5编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsValidatePath(string path)
        {
            bool bBack = false;
            if (string.IsNullOrEmpty(path)) return bBack;
            char[] chJudge = path.ToCharArray();
            char[] chInvalidate = System.IO.Path.GetInvalidPathChars();
            foreach (var c in chJudge)
                if (chInvalidate.Contains(c))
                    return bBack;
            bBack = true;
            return bBack;
        }
    }
}
