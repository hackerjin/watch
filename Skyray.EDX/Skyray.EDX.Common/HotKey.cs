using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common
{
    public class HotKey
    {

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd,
            int id,
            KeyModifiers fsKeyModifiers,
            Keys vk
            );

        [DllImport("user32.dll",SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,
            int id
            );

    }
} 
