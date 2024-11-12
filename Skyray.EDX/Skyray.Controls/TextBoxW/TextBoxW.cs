using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Skyray.API;

namespace Skyray.Controls
{
    public class TextBoxW : TextBox, ISkyrayStyle
    {
        
        //public TextBoxW()
        //{ }

        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    if (((m.Msg == WinMsgs.WM_NCPAINT) || (m.Msg == WinMsgs.WM_ERASEBKGND)) || (m.Msg == WinMsgs.WM_PAINT))
        //    {
        //        IntPtr wParam = m.WParam;
        //        IntPtr hdc = WinMethod.GetDCEx(m.HWnd, wParam, 0x21);
        //        if (hdc != IntPtr.Zero)
        //        {
        //            Graphics graphics = Graphics.FromHdc(hdc);
        //            Rectangle bounds = new Rectangle(0, 0, base.Width, base.Height);
        //            ControlPaint.DrawBorder(graphics, bounds, this.borderColor, ButtonBorderStyle.Solid);
        //            m.Result = (IntPtr)1;
        //            WinMethod.ReleaseDC(m.HWnd, hdc);
        //        }
        //    }
        //}

        

        private Color borderColor;

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        #region ISkyrayStyle 成员
        public void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:
                    this.borderColor = Color.FromArgb(121, 153, 194);
                    this.Refresh();
                    break;
                case Style.Office2007Sliver:
                    this.borderColor = Color.FromArgb(111, 112, 116);
                    this.Refresh();
                    break;

                default: break;
            }
        }

        private Style _Style = Style.Office2007Blue;
        public Style Style
        {
            get
            {
                return _Style;
            }
            set
            {
                _Style = value;
                SetStyle(_Style);
            }
        }

        #endregion

    }
}
