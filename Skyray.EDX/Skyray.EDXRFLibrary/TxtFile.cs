using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace Skyray.EDXRFLibrary
{
    public static class TxtFile
    {
        /// <summary>
        /// 写ini文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// 读ini文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="refVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder refVal, int size, string filePath);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);


        public static void GetStringsFromBuffer(Byte[] Buffer, int bufLen, List<string> Strings)
        {
            Strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((Buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }

        /// <summary>
        /// 判断该字符串是否是合法的浮点数
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static bool IsValidFloat(string text)
        {
            string textValidCode = "[-+]?\\b[0-9]+(\\.[0-9]+)?\\b";//"^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
            Regex regExp = new Regex(textValidCode);
            MatchCollection matchs = regExp.Matches(text);
            if (matchs.Count == 0)
            {
                return false;
            }
            return matchs[0].ToString().Equals(text);
        }

        /// <summary>
        /// 判断输入的字符是否合法（整数、浮点数）
        /// </summary>
        /// <param name="txtExist">现有的字符串</param>
        /// <param name="keyChar">新输入的字符</param>
        /// <param name="bInteger">true:判断合法整数;false:判断合法整数浮点数</param>
        /// <param name="bSign">是否允许负号</param>
        /// <returns></returns>
        public static bool IsValidCharacter(string txtExist, char keyChar, bool bInteger, bool bSign)
        {
            bool bAllow = IsValidFloat(txtExist + keyChar.ToString());
            if (keyChar == '\b')
            {
                bAllow = true;
            }
            if (!bInteger)
            {
                if ((txtExist.Length > 0) && (txtExist.IndexOf('.') < 0) && (keyChar == '.'))
                {
                    bAllow = true;
                }
                if (bSign && (txtExist.Length == 0) && (keyChar == '-'))
                {
                    bAllow = true;
                }
                if ((txtExist.Length == 0) && (keyChar == '.'))
                {
                    bAllow = false;
                }
            }
            return !bAllow;
        }

        /// <summary>
        /// 检查文件名是否合法
        /// </summary>
        /// <param name="name">要检查的字符串</param>
        /// <returns></returns>
        public static bool CheckName(string name)
        {
            bool bResult = true;
            // if (name.IndexOfAny(new char[] { '?', '/', '\\', '<', '>', ':', '*', '\"', '|' }) >= 0 || name == "")
            if ((name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0) || string.IsNullOrEmpty(name) || (name.IndexOfAny(new char[] { '\'', '.', '&' }) >= 0)) //'在数据库查询时会解析错误
            {
                bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// 限制输入文本输入长度
        /// </summary>
        /// <param name="text">现有文本</param>
        /// <param name="keyChar">下一个输入的字符</param>
        /// <param name="length">限制的最大长度</param>
        /// <returns>是否允许输入</returns>
        public static bool IsInStringLengthLimit(string text, char keyChar, int length)
        {
            if (text != null)
            {
                if ((text.Length > length) && (keyChar != '\b'))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 判断是否安装了excel
        /// </summary>
        /// <returns>true 安装了/false 未安装</returns>
        public static bool IsExcelInstall()
        {


            Type type = Type.GetTypeFromProgID("ET.Application");
            if(type ==null)
                type = Type.GetTypeFromProgID("Excel.Application");
           
            return type != null;
        }
    }
}
