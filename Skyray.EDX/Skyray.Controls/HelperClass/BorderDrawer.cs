using System;
using System.Drawing;
using System.Windows.Forms;
using Skyray.API;
namespace Skyray.Controls
{

    internal class BorderDrawer
    {
        private static int WM_ERASEBKGND = 20;
        private static int WM_NCPAINT = 0x85;
        private static int WM_PAINT = 15;

        public void DrawBorder(ref Message message, Color borderColor, int width, int height)
        {
            if (((message.Msg == WM_NCPAINT) || (message.Msg == WM_ERASEBKGND)) || (message.Msg == WM_PAINT))
            {
                IntPtr wParam = message.WParam;
                IntPtr hdc = WinMethod.GetDCEx(message.HWnd, wParam, 0x21);
                if (hdc != IntPtr.Zero)
                {
                    Graphics graphics = Graphics.FromHdc(hdc);
                    Rectangle bounds = new Rectangle(0, 0, width, height);
                    ControlPaint.DrawBorder(graphics, bounds, borderColor, ButtonBorderStyle.Solid);
                    message.Result = (IntPtr)1;
                    WinMethod.ReleaseDC(message.HWnd, hdc);
                }
            }
        }
    }
}

