using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.API
{
    public partial class WinMethod
    {
        [DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        public static extern bool PlaySound(string szSound, System.IntPtr hMod, PlaySoundFlags flags);

    }
}
