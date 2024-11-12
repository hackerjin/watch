using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.API
{
    public partial class WinMethod
    {
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder refVal, int size, string filePath);

        [DllImport("kernel32.dll")]
        public static extern int WritePrivateProfileString(string section, string key, string value, string filePath);
    }
}
