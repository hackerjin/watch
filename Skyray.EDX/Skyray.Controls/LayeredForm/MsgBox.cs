using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;
using SolidBrush = System.Drawing.SolidBrush;
using StringAlignment = System.Drawing.StringAlignment;
using StringFormat = System.Drawing.StringFormat;
using System.Threading;
using System.Runtime.InteropServices;

namespace Skyray.Controls
{
    public class MsgBox : FormCustom
    {
        #region Fields
        private string _msg = string.Empty;
        private Color _foreColor = Color.Black;
        private Color _backColor = Color.WhiteSmoke;
        private int _radius;
        private Size _funcAreaSize;
        private Size _textSize;
        private int _btnBottomToBorder;
        private bool _isWarning = false;
        private bool _isExit = false;
        private bool _isShown = false;

        public static string Yes = "Yes(&Y)";
        public static string No = "No(&N)";
        public static string Cancel = "Cancel(&C)";
        public static string OK = "OK(&O)";
        public static string Retry = "Retry(&R)";
        public static string Ignore = "Ignore(&I)";
        public static string Abort = "Abort(&A)";
        #endregion

        #region Properties
        private Color _warningColor = Color.Red;
        public Color WarningColor
        {
            get { return _warningColor; }
            set { _warningColor = value; }
        }

        private int _warningShakingTimes = 6;

        public int WarningShakingTimes
        {
            get { return _warningShakingTimes; }
            set { _warningShakingTimes = value; }
        }
        #endregion

        #region Ctor
        public MsgBox()
        {
            ApplyStyles();
            this.Disposed += (s, e) =>
            {
                _isExit = true;
            };
            this.FormClosed += (s, e) =>
            {
                _isExit = true;
            };
            this.Shown += (s, e) =>
            {
                _isShown = true;
            };
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
            UpdateStyles();
        }

        public static DialogResult Show(Control owner, string msg, Color foreColor, Color backColor, int radius, byte alpha, Size minSize, Size maxSize, Padding padding, Font font, MessageBoxButtons msgBoxButtons, Size buttonSize, int buttonMargin, int buttonBottomDistanceToBorder, bool isEnglish)
        {
            return new MsgBox().ShowMsg(owner, msg, foreColor, backColor, radius, alpha, minSize, maxSize, padding, font, msgBoxButtons, buttonSize, buttonMargin, buttonBottomDistanceToBorder, isEnglish);
        }

        private DialogResult ShowMsg(Control owner, string msg, Color foreColor, Color backColor, int radius, byte alpha, Size minSize, Size maxSize, Padding padding, Font font, MessageBoxButtons msgBoxButtons, Size buttonSize, int buttonMargin, int buttonBottomDistanceToBorder, bool isEnglish)
        {
            this._msg = msg;
            this._foreColor = foreColor;
            this._backColor = backColor;
            this._radius = radius;
            this.Padding = padding;
            if (font != null)
                this.Font = font;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this._btnBottomToBorder = buttonBottomDistanceToBorder;
            _funcAreaSize = MeasureFuncArea(msgBoxButtons, buttonSize, buttonMargin, buttonBottomDistanceToBorder);
            if (_funcAreaSize.Width > maxSize.Width)
            {
                maxSize.Width = _funcAreaSize.Width;
            }
            _textSize = MeasureText(this._msg, this.Font, minSize, maxSize, this.Padding);

            var newSize = new Size(_funcAreaSize.Width > _textSize.Width ? _funcAreaSize.Width : _textSize.Width,
                                   _funcAreaSize.Height + _textSize.Height);
            if (maxSize.Height < newSize.Height)
            {
                maxSize.Height = newSize.Height;
            }
            this.MinimumSize = minSize;
            this.MaximumSize = maxSize;
            this.Size = newSize;
            GenerateFuncArea(msgBoxButtons, buttonSize, buttonMargin, isEnglish);
            var frm = owner.FindForm();
            this.StartPosition = FormStartPosition.Manual;
            this.Owner = frm;
            this.Location = new Point(frm.Location.X + (frm.Width - this.Width) / 2, frm.Location.Y + (frm.Height - this.Height) / 2);
            this.Radius = radius;
            this.BackColor = backColor;
            this.Alpha = alpha;
            this.Sizable = false;
            //this.Focus();
            //this.BringToFront();
            var r = this.ShowDialog();
            this.Dispose();
            return r;

        }

        private void GenerateFuncArea(MessageBoxButtons msgBoxButtons, Size buttonSize, int buttonMargin, bool custom)
        {
            switch (msgBoxButtons)
            {
                case MessageBoxButtons.OK:
                    var btn = new ButtonEx() { Text = OK, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange, BorderColor = Color.Transparent, Radius = buttonSize.Width / 4f };
                    btn.Location = new Point((this.Width - btn.Width) / 2, this.Height - this._btnBottomToBorder - btn.Height);
                    btn.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                    this.Controls.Add(btn);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    var btnArr = new Control[3]{ 
                        new ButtonEx(){ Text = Yes, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange,BorderColor = Color.Transparent,Radius = buttonSize.Width / 4f },
                        new ButtonEx(){ Text = No, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange,BorderColor = Color.Transparent,Radius = buttonSize.Width / 4f },
                        new ButtonEx(){ Text = Cancel, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange,BorderColor = Color.Transparent,Radius = buttonSize.Width / 4f  }
                    };
                    var left = (this.Width - buttonSize.Width * 3 - buttonMargin * 2) / 2;
                    var h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) =>
                    {
                        this.DialogResult = DialogResult.Yes;
                        //this.Dispose();
                        this.Close();
                    };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) =>
                    {
                        this.DialogResult = DialogResult.No;
                        this.Close();
                    };
                    btnArr[2].Location = new Point(left + buttonSize.Width * 2 + buttonMargin * 2, h);
                    btnArr[2].Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
                    this.Controls.AddRange(btnArr);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    btnArr = new Control[3]{ 
                        new ButtonEx(){ Text = Abort, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange,BorderColor = Color.Transparent,Radius = buttonSize.Height / 4f  },
                        new ButtonEx(){ Text = Retry, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange,BorderColor = Color.Transparent,Radius = buttonSize.Height / 4f },
                        new ButtonEx(){ Text = Ignore, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange,BorderColor = Color.Transparent,Radius = buttonSize.Height / 4f }
                    };
                    left = (this.Width - buttonSize.Width * 3 - buttonMargin * 2) / 2;
                    h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.Abort; this.Close(); };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };
                    btnArr[2].Location = new Point(left + buttonSize.Width * 2 + buttonMargin * 2, h);
                    btnArr[2].Click += (s, e) => { this.DialogResult = DialogResult.Ignore; this.Close(); };
                    this.Controls.AddRange(btnArr);
                    break;
                case MessageBoxButtons.OKCancel:
                    btnArr = new Control[2]{ 
                        new ButtonEx(){ Text = OK, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange, BorderColor = Color.Transparent,Radius = buttonSize.Height / 4f },
                        new ButtonEx(){ Text = Cancel, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange,BorderColor = Color.Transparent,Radius = buttonSize.Height / 4f  }
                    };
                    left = (this.Width - buttonSize.Width * 2 - buttonMargin) / 2;
                    h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
                    this.Controls.AddRange(btnArr);
                    break;
                case MessageBoxButtons.YesNo:
                    btnArr = new Control[2]{ 
                        new ButtonEx(){ Text = Yes, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue,FocusedColor = Color.DarkOrange, BorderColor = Color.Transparent,Radius = buttonSize.Height / 4f  },
                        new ButtonEx(){ Text = No, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue,FocusedColor = Color.DarkOrange, BorderColor = Color.Transparent,Radius = buttonSize.Height / 4f }
                    };
                    left = (this.Width - buttonSize.Width * 2 - buttonMargin) / 2;
                    h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };
                    this.Controls.AddRange(btnArr);
                    break;
                case MessageBoxButtons.RetryCancel:
                    btnArr = new Control[2]{ 
                        new ButtonEx(){ Text = Retry, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange,BorderColor = Color.Transparent,Radius = buttonSize.Height / 4f  },
                        new ButtonEx(){ Text = Cancel, Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.SteelBlue, FocusedColor = Color.DarkOrange,BorderColor = Color.Transparent,Radius = buttonSize.Height / 4f  }
                    };
                    left = (this.Width - buttonSize.Width * 2 - buttonMargin) / 2;
                    h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
                    this.Controls.AddRange(btnArr);
                    break;
            }
        }

        private void DrawButtons(Graphics g, MessageBoxButtons msgBoxButtons, Size buttonSize, int buttonMargin, bool isEnglish)
        {
            Control[] btnArr = { };
            switch (msgBoxButtons)
            {
                case MessageBoxButtons.OK:
                    btnArr = new Control[1]
                                     {
                                        new Button() { Text = isEnglish ? "OK(&O)" : "确定(&O)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent } 
            };
                    btnArr[0].Location = new Point((this.Width - btnArr[0].Width) / 2, this.Height - this._btnBottomToBorder - btnArr[0].Height);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Dispose(); };
                    this.Controls.Add(btnArr[0]);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    btnArr = new Control[3]{ 
                        new Button(){ Text = isEnglish? "Yes(&Y)":"是(&Y)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent},
                        new Button(){ Text = isEnglish? "No(&N)":"否(&N)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent},
                        new Button(){ Text = isEnglish? "Cancel(&C)":"取消(&C)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent }
                    };
                    var left = (this.Width - buttonSize.Width * 3 - buttonMargin * 2) / 2;
                    var h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Dispose(); };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) => { this.DialogResult = DialogResult.No; this.Dispose(); };
                    btnArr[2].Location = new Point(left + buttonSize.Width * 2 + buttonMargin * 2, h);
                    btnArr[2].Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Dispose(); };
                    this.Controls.AddRange(btnArr);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    btnArr = new Control[3]{ 
                        new Button(){ Text = isEnglish? "Abort(&A)":"放弃(&A)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent },
                        new Button(){ Text = isEnglish? "Retry(&R)":"重试(&R)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent },
                        new Button(){ Text = isEnglish? "Ignore(&I)":"忽略(&I)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent }
                    };
                    left = (this.Width - buttonSize.Width * 3 - buttonMargin * 2) / 2;
                    h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Dispose(); };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) => { this.DialogResult = DialogResult.No; this.Dispose(); };
                    btnArr[2].Location = new Point(left + buttonSize.Width * 2 + buttonMargin * 2, h);
                    btnArr[2].Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Dispose(); };
                    this.Controls.AddRange(btnArr);
                    break;
                case MessageBoxButtons.OKCancel:
                    btnArr = new Control[2]{ 
                        new Button(){ Text = isEnglish? "OK(&O)": "确定", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent},
                        new Button(){ Text = isEnglish? "Cancel(&C)":"取消(&C)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent }
                    };
                    left = (this.Width - buttonSize.Width * 2 - buttonMargin) / 2;
                    h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Dispose(); };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Dispose(); };
                    this.Controls.AddRange(btnArr);
                    break;
                case MessageBoxButtons.YesNo:
                    btnArr = new Control[2]{ 
                        new Button(){ Text = isEnglish?"Yes(&Y)" : "是(&Y)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent },
                        new Button(){ Text = isEnglish?"No(&N)" : "否(&N)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent}
                    };
                    left = (this.Width - buttonSize.Width * 2 - buttonMargin) / 2;
                    h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Dispose(); };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) => { this.DialogResult = DialogResult.No; this.Dispose(); };
                    this.Controls.AddRange(btnArr);
                    break;
                case MessageBoxButtons.RetryCancel:
                    btnArr = new Control[2]{ 
                        new Button(){ Text = isEnglish?"Retry(&R)" : "重试(&R)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent },
                        new Button(){ Text = isEnglish?"Cancel(&C)" : "取消(&C)", Size = buttonSize, FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent }
                    };
                    left = (this.Width - buttonSize.Width * 2 - buttonMargin) / 2;
                    h = this.Height - this._btnBottomToBorder - buttonSize.Height;
                    btnArr[0].Location = new Point(left, h);
                    btnArr[0].Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Dispose(); };
                    btnArr[1].Location = new Point(left + buttonSize.Width + buttonMargin, h);
                    btnArr[1].Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Dispose(); };
                    this.Controls.AddRange(btnArr);
                    break;
            }

            using (var bb = new SolidBrush(Color.Green))
            using (var bf = new SolidBrush(_foreColor))
            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                for (int i = 0; i < btnArr.Length; i++)
                {
                    var rect = new Rectangle(btnArr[i].Location, btnArr[i].Size);
                    var text = btnArr[i].Text.Replace("&", "");
                    using (var gp = GetRoundCornerGraphicsPath(rect, btnArr[i].Width / 2f + 10))
                    {
                        g.FillPath(bb, gp);
                        g.DrawString(text, this.Font, bf, rect, sf);
                    }

                }

            }
        }

        private Size MeasureFuncArea(MessageBoxButtons button, Size buttonSize, int buttonMargin, int distance)
        {
            switch (button)
            {
                case MessageBoxButtons.OK:
                    return new Size(buttonSize.Width + 20, buttonSize.Height + distance);
                case MessageBoxButtons.YesNoCancel:
                case MessageBoxButtons.AbortRetryIgnore:
                    return new Size(buttonSize.Width * 3 + buttonMargin * 2 + 20, buttonSize.Height + distance);
                default:
                    return new Size(buttonSize.Width * 2 + buttonMargin + 20, buttonSize.Height + distance);
            }
        }

        private Size MeasureText(string msg, System.Drawing.Font font, Size minSize, Size maxSize, Padding padding)
        {
            using (var g = this.CreateGraphics())
            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                var sizef = g.MeasureString(msg, font, maxSize.Width, sf);
                var size = new Size((int)(sizef.Width + padding.Left + padding.Right + 1),
                                    (int)(sizef.Height + padding.Top + padding.Bottom + 1));
                //size.Width = size.Width < minSize.Width ? minSize.Width : size.Width;
                //size.Height = size.Height < minSize.Height ? minSize.Height : size.Height;
                return size;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            var rect = this.ClientRectangle;
            var rectText = new Rectangle(this.Padding.Left, this.Padding.Top, this.Width - this.Padding.Right - this.Padding.Left,
                                         this.Height - this.Padding.Bottom - this.Padding.Top - _funcAreaSize.Height);
            using (var gp = GetRoundCornerGraphicsPath(rect, this._radius))
            using (var bf = new SolidBrush(_isWarning ? _warningColor : _foreColor))
            using (var bb = new SolidBrush(_backColor))
            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.FillPath(bb, gp);
                if (!string.IsNullOrEmpty(this._msg))
                {
                    g.DrawString(this._msg, this.Font, bf, rectText, sf);
                }
            }
        }

        private const int WM_NCACTIVATE = 0x0086;
        private volatile bool executFlag = true;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCACTIVATE)
            {
                if (executFlag && _isShown && (!_isExit))
                    StartWarning();
            }
            base.WndProc(ref m);

        }

        private void StartWarning()
        {
            //Console.WriteLine("Running");
            executFlag = false;
            new Action(() =>
            {
                try
                {
                    for (int i = 0; i < _warningShakingTimes && !_isExit; i++)
                    {
                        _isWarning = !_isWarning;
                        this.SafeCall(() =>
                        {
                            this.Refresh();
                        });
                        Thread.Sleep(100);
                    }
                }
                catch (Exception)
                {
                    //Console.WriteLine("exception");
                }

            }).BeginInvoke(ar =>
            {
                Thread.Sleep(1000);
                executFlag = true;
            }, null);
        }
        #endregion


    }
}
