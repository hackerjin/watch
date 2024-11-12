using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Skyray.Controls
{
    public class ButtonEx : Button
    {
        private Color _bgColor;
        private Color _borderColor;
        private Color _enteredColor;
        private Color _pressedColor;
        private Color _focusedColor;
        private float _radius;
        private bool _entered;
        private bool _pressed;
        private bool _enabled;


        public new Color BackColor
        {
            get
            {
                return _bgColor;
            }
            set
            {
                if (value == _bgColor)
                    return;
                _bgColor = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                if (value == _borderColor)
                    return;
                _borderColor = value;
                Invalidate();
            }
        }

        public Color EnteredColor
        {
            get
            {
                return _enteredColor;
            }
            set
            {
                _enteredColor = value;
            }
        }

        public Color PressedColor
        {
            get
            {
                return _pressedColor;
            }
            set
            {
                _pressedColor = value;
            }
        }

        public Color FocusedColor
        {
            get
            {
                return _focusedColor;
            }
            set
            {
                _focusedColor = value;
            }
        }

        public float Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                if (value == _radius)
                    return;
                if (value > Width / 2f)
                {
                    value = Width / 2f;
                }
                if (value > Height / 2f)
                {
                    value = Height / 2f;
                }
                _radius = value;
                Invalidate();
            }
        }


        public ButtonEx()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.SupportsTransparentBackColor
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw
                , true);
            this.Width = 100;
            this.Height = 32;
            base.BackColor = Color.Transparent;
            _borderColor = Color.LightBlue;
            _pressedColor = Color.Orange;
            _enteredColor = Color.Khaki;
            _bgColor = Color.SteelBlue;
            _radius = 8;
            _focusedColor = Color.Empty;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            var rectText = new Rectangle(this.Padding.Left, this.Padding.Top, this.Width - this.Padding.Right - this.Padding.Left, this.Height - this.Padding.Bottom - this.Padding.Top);
            var rect = this.ClientRectangle;
            using (var b = new SolidBrush(_pressed ? _pressedColor : (_entered ? _enteredColor : _bgColor)))
            using (var b2 = new SolidBrush(this.ForeColor))
            using (var gp = GetRoundCornerGraphicsPath(rect, _radius))
            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.FillPath(b, gp);
                if (_borderColor != Color.Transparent)
                {
                    using (Pen p = new Pen(_borderColor))
                    {
                        g.DrawPath(p, gp);
                    }
                }
                if (!string.IsNullOrEmpty(this.Text))
                {
                    g.DrawString(this.Text.Replace("&", ""), this.Font, b2, rectText, sf);
                }

                if (this.Focused && _focusedColor != Color.Empty)
                {
                    var rect2 = new Rectangle((int)(_radius + 1), this.Height - 4, (int)(this.Width - _radius * 2), 2);
                    using (var gp2 = GetRoundCornerGraphicsPath(rect2, 1))
                    using (var b3 = new SolidBrush(_focusedColor))
                    {
                        g.FillPath(b3, gp2);
                    }
                }
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

        
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _entered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _entered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                _pressed = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            _pressed = false;
            Invalidate();
        }

    }
}
