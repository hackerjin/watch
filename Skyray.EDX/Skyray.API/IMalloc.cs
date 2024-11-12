using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.API
{
    // C# representation of the IMalloc interface.
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000002-0000-0000-C000-000000000046")]
    public interface IMalloc
    {
        [PreserveSig]
        IntPtr Alloc([In] int cb);
        [PreserveSig]
        IntPtr Realloc([In] IntPtr pv, [In] int cb);
        [PreserveSig]
        void Free([In] IntPtr pv);
        [PreserveSig]
        int GetSize([In] IntPtr pv);
        [PreserveSig]
        int DidAlloc(IntPtr pv);
        [PreserveSig]
        void HeapMinimize();
    }
}
