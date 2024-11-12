using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.Controls
{
    public class FormLayer : Form
    {
        private FormCustom _main;
        public FormLayer(FormCustom main)
        {
            ApplyStyles();
            this._main = main;
            main.Owner = this;
            AutoScaleMode = AutoScaleMode.None;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
        }

        private void ApplyStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var p = base.CreateParams;
                p.ExStyle |= 0x00080000; // WS_EX_LAYERED
                p.Style = p.Style | 0x00020000;   // WS_MINIMIZEBOX
                return p;
            }
        }

        public GraphicsPath GetRoundCornerGraphicsPath(Rectangle rect, float radius)
        {
            var gp = new GraphicsPath();
            if (radius == 0f)
            {
                gp.AddRectangle(rect);
                return gp;
            }
            int x = rect.X, y = rect.Y, w = rect.Width, h = rect.Height;
            if (radius > w / 2f)
                radius = w / 2f;
            if (radius > h / 2f)
                radius = h / 2f;
            var d = radius * 2;
            gp.AddArc(x, y, d, d, 180, 90);
            gp.AddArc(x + w - d - 1, y, d, d, 270, 90);
            gp.AddArc(x + w - d - 1, y + h - d - 1, d, d, 0, 90);
            gp.AddArc(x, y + h - d - 1, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        public void SetBits(byte alpha)
        {
            if (BackgroundImage == null)
                return;
            //绘制绘图层背景
            using (var bitmap = new Bitmap(BackgroundImage, Width, Height))
            {
                if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                    throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
                var oldBits = IntPtr.Zero;
                var screenDC = Win32.GetDC(IntPtr.Zero);
                var hBitmap = IntPtr.Zero;
                var memDc = Win32.CreateCompatibleDC(screenDC);

                try
                {
                    var topLoc = new Win32.Point(Left, Top);
                    var bitMapSize = new Win32.Size(Width, Height);
                    var blendFunc = new Win32.BLENDFUNCTION();
                    var srcLoc = new Win32.Point(0, 0);

                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    oldBits = Win32.SelectObject(memDc, hBitmap);

                    blendFunc.BlendOp = Win32.AC_SRC_OVER;
                    blendFunc.SourceConstantAlpha = alpha;
                    blendFunc.AlphaFormat = Win32.AC_SRC_ALPHA;
                    blendFunc.BlendFlags = 0;

                    Win32.UpdateLayeredWindow(Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, Win32.ULW_ALPHA);
                }
                finally
                {
                    if (hBitmap != IntPtr.Zero)
                    {
                        Win32.SelectObject(memDc, oldBits);
                        Win32.DeleteObject(hBitmap);
                    }
                    Win32.ReleaseDC(IntPtr.Zero, screenDC);
                    Win32.DeleteDC(memDc);
                }
            }
        }
    }
}
