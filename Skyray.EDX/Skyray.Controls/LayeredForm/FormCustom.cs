using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace Skyray.Controls
{
    public class FormCustom : Form
    {
        private FormLayer _layer;

        private float _radius;
        public float Radius
        {
            get { return _radius; }
            set
            {
                if (value == _radius)
                    return;
                LastRadius = _radius;
                if (value < 0f)
                {
                    _radius = 0f;
                }
                else
                {
                    _radius = value;
                    if (_radius > this.Height / 2f)
                        _radius = this.Height / 2f;
                    if (_radius > this.Width / 2f)
                        _radius = this.Width / 2f;
                }
                DrawBackgroundImage();
            }
        }

        public float LastRadius { get; set; }

        private byte _alpha = 255;
        public byte Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value;
            }
        }

        private Form _owner;
        public new Form Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                if (_layer != null)
                {
                    base.Owner = _layer;
                    _layer.Owner = _owner;
                }
            }
        }

        private FormWindowState _state;
        public new FormWindowState WindowState
        {
            get { return _state; }
            set
            {
                _state = value;
                if (_state == FormWindowState.Minimized)
                {
                    SetOwnedForms(this, false);
                }
                base.WindowState = _state;

            }
        }

        private bool _movable = true;
        public bool Movable
        {
            get { return _movable; }
            set
            {
                _movable = value;
            }
        }

        private bool _sizable = true;
        public bool Sizable
        {
            get { return _sizable; }
            set
            {
                _sizable = value;
            }
        }



        public Action<Graphics> DrawOthers;

        public FormCustom()
        {
            ApplyStyles();
            AutoScaleMode = AutoScaleMode.None;
            FormBorderStyle = FormBorderStyle.None;
            if (!DesignMode && _layer == null)
            {
                _layer = new FormLayer(this); //先实例化, 等FormCustom Load时一起显示
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var p = base.CreateParams;
                p.Style = p.Style | 0x00020000;   // WS_MINIMIZEBOX
                return p;
            }
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

        private void SetOwnedForms(Form frm, bool flag)
        {
            if (frm.OwnedForms != null && frm.OwnedForms.Length > 0)
            {
                for (int i = 0; i < frm.OwnedForms.Length; i++)
                {
                    var f = frm.OwnedForms[i];
                    f.Visible = flag;
                    SetOwnedForms(f, flag);
                }

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode && _layer != null && _radius > 0f)
            {
                this.TransparencyKey = BackColor;
                if (_sizable)
                    this.MakeSizable();
                if (_movable)
                    this.MakeMovable();
                this.LocationChanged += (s, a) =>
                {
                    if (_layer == null)
                        return;
                    _layer.Location = this.Location;
                };
                this.SizeChanged += (s, a) =>
                {
                    if (_layer == null)
                        return;
                    _layer.ClientSize = this.ClientSize;
                    DrawBackgroundImage();
                    //_layer.SetBits(_alpha);
                    SetRegion();
                };
                this.Activated += (s, a) =>
                {
                    base.Owner = _layer;
                };
                this.Disposed += (s, a) =>
                {
                    if (_layer.BackgroundImage != null)
                        _layer.BackgroundImage.Dispose();
                    _layer.Dispose();
                };
                _layer.ClientSize = this.ClientSize;
                _layer.Location = this.Location;
                _layer.BackgroundImageLayout = ImageLayout.Stretch;
                _layer.BackgroundImageChanged += (s, a) =>
                {
                    if (_layer == null)
                        return;
                    _layer.SetBits(_alpha);
                };
                _layer.FormClosed += (s, a) =>
                {
                    if (_layer.BackgroundImage != null)
                        _layer.BackgroundImage.Dispose();
                    _layer.Dispose();
                };
                DrawBackgroundImage();
                SetRegion();
                _layer.Show();

            }
            base.OnLoad(e);
        }

        private void SetRegion()
        {
            using (var gp = GetClipRegionPath())
            {
                this.Region = new Region(gp);
            }

        }

        private GraphicsPath GetClipRegionPath()
        {
            var gp = new GraphicsPath();
            if (_radius == 0f)
            {
                gp.AddRectangle(this.ClientRectangle);
                return gp;
            }
            int w = this.Width, h = this.Height;
            if (_radius > w / 2f)
                _radius = w / 2f;
            if (_radius > h / 2f)
                _radius = h / 2f;
            var d = _radius * 2;

            var tmp = (float)((1 - Math.Sqrt(2.0) / 2) * _radius);
            gp.AddLines(new PointF[] 
            {
                new PointF(1, _radius + 1),
                new PointF(tmp + 1, tmp + 1),
                new PointF(_radius + 1, 1),
                new PointF(w - _radius - 1, 1),
                new PointF(w - tmp - 1, tmp + 1),
                new PointF(w - 1, _radius + 1),
                new PointF(w - 1, h - _radius - 1),
                new PointF(w - tmp - 1, h - tmp - 1),
                new PointF(w - _radius - 1, h - 1),
                new PointF(_radius + 1, h - 1),
                new PointF(tmp + 1, h - tmp - 1),
                new PointF(1, h - _radius - 1)
            });
            gp.CloseFigure();
            return gp;
        }

        const int WM_SYSCOMMAND = 0x112;
        //窗体关闭消息
        const int SC_CLOSE = 0xf060;
        //窗体最小化消息
        const int SC_MINIMIZE = 0xf020;
        //窗体最大化消息
        const int SC_MAXIMIZE = 0xf030;
        //窗体还原消息
        const int SC_RESTORE = 0xf120;

        //窗体按钮的拦截函数
        protected override void WndProc(ref Message m)
        {

            if (m.Msg == WM_SYSCOMMAND)
            {

                if (m.WParam.ToInt32() == SC_RESTORE)
                {
                    //拦截还原按钮
                    SetOwnedForms(this, true);
                }

                if (m.WParam.ToInt32() == SC_MINIMIZE)
                {
                    //拦截还原按钮
                    SetOwnedForms(this, false);
                }
            }
            base.WndProc(ref m);



        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_layer != null)
            {
                base.Owner = null;
                _layer.Close();
            }
            base.OnFormClosed(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DesignMode)
            {
                var g = e.Graphics;
                DrawImpl(g, this.ClientRectangle);
            }
        }

        private void DrawBackgroundImage()
        {
            if (_layer == null)
                return;
            if (_layer.BackgroundImage != null)
                _layer.BackgroundImage.Dispose();
            var bmp = new Bitmap(_layer.Width, _layer.Height);
            bmp.MakeTransparent();
            using (var g = Graphics.FromImage(bmp))
            {
                var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                DrawImpl(g, rect);
            }
            _layer.BackgroundImage = bmp;
        }

        private void DrawImpl(Graphics g, Rectangle rect)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            using (var gp = GetRoundCornerGraphicsPath(rect, this._radius))
            using (var bb = new SolidBrush(this.BackColor))
            {
                g.FillPath(bb, gp);
            }

            if (DrawOthers != null)
                DrawOthers(g);
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
    }
}

