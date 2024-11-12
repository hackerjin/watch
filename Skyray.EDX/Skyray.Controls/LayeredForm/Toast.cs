using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Skyray.Controls
{
    public class Toast : Form
    {
        #region Fields
        private string _msg = string.Empty;
        private Color _foreColor = Color.Black;
        private Color _backColor = Color.WhiteSmoke;
        private int _radius;
        private int? _stayTime;
        private int? _gradientTime;
        private double _startOpacity;
        private double _endOpacity;
        private Timer _timer;
        private static Toast _instance;
        #endregion

        #region Properties
        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Ctor
        public Toast()
        {
            ApplyStyles();
            this.Click += (s, e) => this.Dispose();
        }
        #endregion

        #region Methods

        private void ApplyStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            //强制分配样式重新应用到控件上
            UpdateStyles();
        }

        public static void Show(Control owner, string msg)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.White, 20, 2000, 1000, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    owner.FindForm() == null ? null : owner.FindForm().Font);
        }

        public static void Show(Control owner, string msg, int? stayTime, int? gradientTime)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.White, 20, stayTime, gradientTime, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    owner.FindForm() == null ? null : owner.FindForm().Font);
        }

        public static void Show(Control owner, string msg, Color foreColor, Color backColor, int? stayTime, int? gradientTime)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, foreColor, backColor, 20, stayTime, gradientTime, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    owner.FindForm() == null ? null : owner.FindForm().Font);
        }

        public static void Show(Control owner, string msg, Color foreColor, Color backColor, int? stayTime, int? gradientTime, Font font)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, foreColor, backColor, 20, stayTime, gradientTime, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    font);
        }

        public static void ShowSuccess(Control owner, string msg)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Green, 20, 2000, 1000, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    owner.FindForm() == null ? null : owner.FindForm().Font);
        }

        public static void ShowSuccess(Control owner, string msg, Font f)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Green, 20, 2000, 1000, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    f);
        }

        public static void ShowSuccess(Control owner, string msg, int? stayTime, int? gradientTime)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Green, 20, stayTime, gradientTime, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    owner.FindForm() == null ? null : owner.FindForm().Font);
        }

        public static void ShowSuccess(Control owner, string msg, int? stayTime, int? gradientTime, Font f)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Green, 20, stayTime, gradientTime, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    f);
        }

        public static void ShowError(Control owner, string msg)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Red, 20, 2000, 1000, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    owner.FindForm() == null ? null : owner.FindForm().Font);
        }

        public static void ShowError(Control owner, string msg, Font f)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Red, 20, 2000, 1000, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    f);
        }

        public static void ShowError(Control owner, string msg, int? stayTime, int? gradientTime)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Red, 20, stayTime, gradientTime, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    owner.FindForm() == null ? null : owner.FindForm().Font);
        }

        public static void ShowWarning(Control owner, string msg)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Yellow, 20, 2000, 1000, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    owner.FindForm() == null ? null : owner.FindForm().Font);
        }

        public static void ShowWarning(Control owner, string msg, Font f)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Yellow, 20, 2000, 1000, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    f);
        }

        public static void ShowWarning(Control owner, string msg, int? stayTime, int? gradientTime)
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Dispose();
            }
            _instance = new Toast();
            _instance.ShowMsg(owner, msg, Color.Black, Color.Yellow, 20, stayTime, gradientTime, 0.9, 0.1, new Size(120, 50), new Size(600, 450), new Padding(10, 14, 10, 10),
                    owner.FindForm() == null ? null : owner.FindForm().Font);
        }

        private void ShowMsg(Control owner, string msg, Color foreColor, Color backColor, int radius, int? stayTime, int? gradientTime, double startOpacity, double endOpacity, Size minSize, Size maxSize, Padding padding, Font font)
        {
            this._msg = msg;
            this._foreColor = foreColor;
            this._backColor = backColor;
            this._radius = radius;
            this._stayTime = stayTime;
            this._gradientTime = gradientTime;
            this._startOpacity = startOpacity;
            this._endOpacity = endOpacity;
            this.MinimumSize = minSize;
            this.MaximumSize = maxSize;
            this.Padding = padding;
            if (font != null)
                this.Font = font;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.ShowInTaskbar = false;
            this.Size = MeasureText(this._msg, this.Font, this.MinimumSize, this.MaximumSize, this.Padding);
            this.Owner = owner == null ? null : owner.FindForm();
            if (this.Owner != null)
            {
                this.Location = new Point(this.Owner.Location.X + (this.Owner.Width - this.Width) / 2, this.Owner.Location.Y + (this.Owner.Height - this.Height) / 2);
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            CreateBitmap();

            this.Show();

            if (_timer == null && this._stayTime.HasValue)
            {
                var gt = this._gradientTime.HasValue && this._gradientTime.Value > 0 ? this._gradientTime.Value : 1000;
                var diff = this.Opacity - this._endOpacity;
                diff = diff > 0 && diff <= 1 ? diff : 1d;
                var per = diff / gt;
                _timer = new Timer() { Interval = this._stayTime.HasValue && this._stayTime.Value > 0 ? this._stayTime.Value : 2000 };
                _timer.Tick += (s, e) =>
                {

                    _timer.Interval = 200;
                    this._startOpacity -= (per * _timer.Interval);
                    if (this._startOpacity <= 0d)
                    {
                        this.Dispose();
                    }
                    else
                    {
                        SetBits();
                    }

                };
                _timer.Enabled = true;
            }

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cParms;
            }
        }

        public void SetBits()
        {
            if (BackgroundImage == null)
                return;
            //绘制绘图层背景
            using (var bitmap = new Bitmap(BackgroundImage, Width, Height))
            {
                if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                    throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
                IntPtr oldBits = IntPtr.Zero;
                IntPtr screenDC = Win32.GetDC(IntPtr.Zero);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr memDc = Win32.CreateCompatibleDC(screenDC);

                try
                {
                    Win32.Point topLoc = new Win32.Point(Left, Top);
                    Win32.Size bitMapSize = new Win32.Size(Width, Height);
                    Win32.BLENDFUNCTION blendFunc = new Win32.BLENDFUNCTION();
                    Win32.Point srcLoc = new Win32.Point(0, 0);

                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    oldBits = Win32.SelectObject(memDc, hBitmap);

                    blendFunc.BlendOp = Win32.AC_SRC_OVER;
                    blendFunc.SourceConstantAlpha = (Byte)(255 * (_startOpacity < 0 ? 0 : _startOpacity));
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

        private void CreateBitmap()
        {
            this.BackgroundImageLayout = ImageLayout.Stretch;
            var bitmap = new Bitmap(this.Width, this.Height);
            bitmap.MakeTransparent();
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                var rectText = new Rectangle(this.Padding.Left, this.Padding.Top, this.Width - this.Padding.Right - this.Padding.Left, this.Height - this.Padding.Bottom - this.Padding.Top);
                var rect = this.ClientRectangle;
                using (var gp = GetRoundCornerGraphicsPath(rect, this._radius))
                using (var bf = new SolidBrush(_foreColor))
                using (var bb = new SolidBrush(_backColor))
                using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.FillPath(bb, gp);
                    if (!string.IsNullOrEmpty(this._msg))
                    {
                        g.DrawString(this._msg, this.Font, bf, rectText, sf);
                    }
                }
                this.BackgroundImage = bitmap;
            }



        }

        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);
            SetBits();
        }

        private Size MeasureText(string msg, Font font, Size minSize, Size maxSize, Padding padding)
        {
            using (var g = this.CreateGraphics())
            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                var sizef = g.MeasureString(msg, font, maxSize.Width, sf);
                var size = new Size((int)(sizef.Width + padding.Left + padding.Right + 1),
                                    (int)(sizef.Height + padding.Top + padding.Bottom + 1));
                size.Width = size.Width < minSize.Width ? minSize.Width : size.Width;
                size.Height = size.Height < minSize.Height ? minSize.Height : size.Height;
                return size;
            }
        }

        private static GraphicsPath GetRoundCornerGraphicsPath(Rectangle rect, float radius)
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

        protected override void Dispose(bool disposing)
        {
            if (this._timer != null)
            {
                this._timer.Enabled = false;
                this._timer.Dispose();
            }

            if (this.BackgroundImage != null)
            {
                this.BackgroundImage.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}

