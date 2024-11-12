using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;


namespace Skyray.API
{
    public partial class WinMethod
    {
        [DllImport("Shell32.DLL")]
        public static extern int SHGetMalloc(out IMalloc ppMalloc);

        [DllImport("Shell32.DLL")]
        public static extern int SHGetSpecialFolderLocation(
                    IntPtr hwndOwner, int nFolder, out IntPtr ppidl);

        [DllImport("Shell32.DLL")]
        public static extern int SHGetPathFromIDList(
                    IntPtr pidl, StringBuilder Path);

        [DllImport("Shell32.DLL", CharSet = CharSet.Auto)]
        public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO bi);

        [DllImport("shell32.dll")]
        public static extern System.UInt32 SHAppBarMessage(System.UInt32 dwMessage, ref APPBARDATA data);
    }
}
