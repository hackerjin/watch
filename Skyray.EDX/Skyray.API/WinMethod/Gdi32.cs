using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;


namespace Skyray.API
{
    public partial class WinMethod
    {
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCA(
            [MarshalAs(UnmanagedType.LPStr)]string lpszDriver,
            [MarshalAs(UnmanagedType.LPStr)]string lpszDevice,
            [MarshalAs(UnmanagedType.LPStr)]string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth,
           int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, PatBltTypes dwRop);

        [DllImport("gdi32")]
        public static extern bool BitBlt(
            IntPtr hdcDest,     // handle to destination DC (device context)
            int nXDest,         // x-coord of destination upper-left corner
            int nYDest,         // y-coord of destination upper-left corner
            int nWidth,         // width of destination rectangle
            int nHeight,        // height of destination rectangle
            IntPtr hdcSrc,      // handle to source DC
            int nXSrc,          // x-coordinate of source upper-left corner
            int nYSrc,          // y-coordinate of source upper-left corner
            System.Int32 dwRop  // raster operation code
            );

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(String driverName, String deviceName, String output, IntPtr lpInitData);

        [DllImport("gdi32.dll")]
        static public extern bool StretchBlt(IntPtr hDCDest, int XOriginDest, int YOriginDest, int WidthDest, int HeightDest,
        IntPtr hDCSrc, int XOriginScr, int YOriginSrc, int WidthScr, int HeightScr, uint Rop);


        [DllImport("gdi32.dll")]
        static public extern bool BitBlt(IntPtr hDCDest, int XOriginDest, int YOriginDest, int WidthDest, int HeightDest,
        IntPtr hDCSrc, int XOriginScr, int YOriginSrc, uint Rop);

        [DllImport("gdi32.dll")]
        static public extern bool PatBlt(IntPtr hDC, int XLeft, int YLeft, int Width, int Height, uint Rop);

        [DllImport("gdi32.dll")]
        static public extern uint GetPixel(IntPtr hDC, int XPos, int YPos);
        [DllImport("gdi32.dll")]
        static public extern int SetMapMode(IntPtr hDC, int fnMapMode);
        [DllImport("gdi32.dll")]
        static public extern int GetObjectType(IntPtr handle);
        [DllImport("gdi32")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO_FLAT bmi,
        int iUsage, ref int ppvBits, IntPtr hSection, int dwOffset);
        [DllImport("gdi32")]
        public static extern int GetDIBits(IntPtr hDC, IntPtr hbm, int StartScan, int ScanLines, int lpBits, BITMAPINFOHEADER bmi, int usage);
        [DllImport("gdi32")]
        public static extern int GetDIBits(IntPtr hdc, IntPtr hbm, int StartScan, int ScanLines, int lpBits, ref BITMAPINFO_FLAT bmi, int usage);
        [DllImport("gdi32")]
        public static extern IntPtr GetPaletteEntries(IntPtr hpal, int iStartIndex, int nEntries, byte[] lppe);
        [DllImport("gdi32")]
        public static extern IntPtr GetSystemPaletteEntries(IntPtr hdc, int iStartIndex, int nEntries, byte[] lppe);
        [DllImport("gdi32")]
        public static extern uint SetDCBrushColor(IntPtr hdc, uint crColor);
        [DllImport("gdi32")]
        public static extern IntPtr CreateSolidBrush(uint crColor);
        [DllImport("gdi32")]
        public static extern int SetBkMode(IntPtr hDC, BackgroundMode mode);
        [DllImport("gdi32")]
        public static extern int SetViewportOrgEx(IntPtr hdc, int x, int y, int param);
        [DllImport("gdi32")]
        public static extern uint SetTextColor(IntPtr hDC, uint colorRef);
        [DllImport("gdi32")]
        public static extern int SetStretchBltMode(IntPtr hDC, int StrechMode);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool MoveToEx(IntPtr hDC, int x, int y, ref POINT lpPoint);

        [DllImport("gdi32.dll")]
        public static extern IntPtr LineTo(IntPtr hdc, IntPtr x, IntPtr y);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool LineTo(IntPtr hDC, int x, int y);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern drawingMode SetROP2(IntPtr hdc, drawingMode fnDrawMode);


        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreatePen(int nPenStyle, int nWidth, int crColor);

    }
}
