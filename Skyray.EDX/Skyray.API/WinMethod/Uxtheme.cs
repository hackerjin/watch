using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.API
{
    public partial class WinMethod
    {
        [DllImport("uxtheme.dll")]
        public static extern int SetWindowTheme(IntPtr hwnd, String pszSubAppName, String pszSubIdList);

        //[DllImport("uxtheme.dll")]
        //static public extern int SetWindowTheme(IntPtr hWnd, string AppID, string ClassID);
    }
}
