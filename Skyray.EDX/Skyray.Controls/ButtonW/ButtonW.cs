using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace Skyray.Controls
{
    public class ButtonW : Button, ISkyrayStyle
    {
        #region About Constructor

        public ButtonW()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor |
                          ControlStyles.UserPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.Opaque, false);
        }

        #endregion

        #region About Image Settings
        private e_imagelocation _imagelocation;
        public enum e_imagelocation
        {
            Center, LeftCenter, Top, Bottom, Left, Right, None
        }
        [DefaultValue(e_imagelocation.Center)]
        public e_imagelocation ImageLocation
        {
            get { return _imagelocation; }
            set { _imagelocation = value; this.Refresh(); }
        }
        private int _imageoffset = 2;
        [DefaultValue(2)]
        public int ImageOffset
        {
            get { return _imageoffset; }
            set { _imageoffset = value; }
        }
        private Point _maximagesize;
        public Point MaxImageSize
        {
            get { return _maximagesize; }
            set { _maximagesize = value; }
        }

        #endregion

        #region About Button Settings
        private e_showbase _showbase;
        private e_showbase _tempshowbase;
        public enum e_showbase
        {
            Yes, No
        }
        [DefaultValue(e_showbase.Yes)]
        public e_showbase ShowBase
        {
            get { return _showbase; }
            set
            {
                _showbase = value;
                this.Refresh();
            }
        }
        private int _radius = 4;
        [DefaultValue(4)]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (value >= 2)
                {
                    _radius = value;
                    this.Refresh();
                }
            }
        }
        private e_groupPos _grouppos;

        [DefaultValue(e_groupPos.None)]
        public e_groupPos GroupPos
        {
            get { return _grouppos; }
            set { _grouppos = value; this.Refresh(); }
        }

        private e_arrow _arrow;
        public enum e_arrow
        {
            None, ToRight, ToDown
        }
        [DefaultValue(e_arrow.None)]
        public e_arrow Arrow
        {
            get { return _arrow; }
            set { _arrow = value; this.Refresh(); }
        }
        private e_splitbutton _splitbutton;
        public enum e_splitbutton
        {
            No, Yes
        }
        [DefaultValue(e_splitbutton.No)]
        public e_splitbutton SplitButton
        {
            get { return _splitbutton; }
            set { _splitbutton = value; this.Refresh(); }
        }
        private int _splitdistance = 0;
        [DefaultValue(0)]
        public int SplitDistance
        {
            get { return _splitdistance; }
            set { _splitdistance = value; this.Refresh(); }
        }
        private string _title = "";
        [DefaultValue("")]
        public string Title
        {
            get { return _title; }
            set { _title = value; this.Refresh(); }
        }
        private bool _keeppress = false;
        [DefaultValue(false)]
        public bool KeepPress
        {
            get { return _keeppress; }
            set { _keeppress = value; }
        }
        private bool _ispressed = false;
        [DefaultValue(false)]
        public bool IsPressed
        {
            get { return _ispressed; }
            set
            {
                _ispressed = value;
                this.Invalidate();
            }
        }


        #endregion

        #region Menu Pos
        private Point _menupos = Point.Empty;
        [DefaultValue(typeof(Point))]
        public Point MenuPos
        {
            get { return _menupos; }
            set { _menupos = value; }
        }
        #endregion

        protected override void OnPaint(PaintEventArgs pevent)
        {
            #region Variables & Conf
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.High;

            #endregion

            #region Paint
            FillGradients(g);
            DrawImage(g);
            DrawString(g);
            DrawArrow(g);
            if (ToFocused) FillSplit(g);

            //DrawGradientItem(g, new Rectangle(Point.Empty, this.Bounds.Size), colors);
            #endregion
        }

        //private void DrawGradientItem(Graphics g,
        //                             Rectangle backRect,
        //                             GradientItemColors colors)
        //{
        //    // Cannot paint a zero sized area
        //    if ((backRect.Width > 0) && (backRect.Height > 0))
        //    {
        //        // Draw the background of the entire item
        //        DrawGradientBack(g, backRect, colors);

        //        // Draw the border of the entire item
        //        DrawGradientBorder(g, backRect, colors);
        //    }
        //}


        #region Paint Methods

        public bool bSilver { get; set; }

        public void FillGradients(Graphics gr)
        {
            int origin = this.Height / 3;
            int end = this.Height;
            int oe = (end - origin) / 2;
            GraphicsPath pa;

            LinearGradientBrush lgbrush;
            Rectangle rect;
            if (_showbase == e_showbase.Yes)
            {
                rect = new Rectangle(new Point(0, 0), new Size(this.Width - 1, this.Height - 1));
                pa = new GraphicsPath();
                DrawArc(rect, pa);

                #region Main Gradient

                ColorBlend mix = new ColorBlend();
                Color[] colors = null;

                switch (i_mode)
                {
                    case 0:
                        if (bSilver)
                        {
                            colors = ColorHelper.colors3;
                        }
                        else
                        {
                            colors = ColorHelper.colors0;
                        }

                        break;
                    case 1:
                        colors = ColorHelper.colors1;
                        break;
                    case 2:
                        colors = ColorHelper.colors2;
                        break;
                }

                mix.Colors = new Color[] { colors[0], colors[1], colors[2], colors[3] };

                mix.Positions = new float[] { 0.0F, 0.3F, 0.35F, 1.0F };

                lgbrush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                lgbrush.InterpolationColors = mix;

                gr.FillPath(lgbrush, pa);

                #endregion

                #region Fill Band

                rect = new Rectangle(new Point(0, 0), new Size(this.Width, this.Height / 3));
                pa = new GraphicsPath();
                int _rtemp = _radius;
                _radius = _rtemp - 1;
                DrawArc(rect, pa);

                if (colors[0].A > 80)
                {
                    gr.FillPath(new SolidBrush(Color.FromArgb(60, 255, 255, 255)), pa);//wzw
                }
                _radius = _rtemp;

                #endregion

                #region SplitFill
                if (_splitbutton == e_splitbutton.Yes & mouse)
                {
                    FillSplit(gr);
                }


                #endregion

                #region SplitLine

                if (_splitbutton == e_splitbutton.Yes)
                {
                    if (_imagelocation == e_imagelocation.Top)
                    {
                        switch (i_mode)
                        {
                            case 1:
                                gr.DrawLine(new Pen(ColorHelper.colors1[4]), new Point(1, this.Height - _splitdistance), new Point(this.Width - 1, this.Height - _splitdistance));
                                break;
                            case 2:
                                gr.DrawLine(new Pen(ColorHelper.colors2[4]), new Point(1, this.Height - _splitdistance), new Point(this.Width - 1, this.Height - _splitdistance));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (_imagelocation == e_imagelocation.Left)
                    {
                        switch (i_mode)
                        {
                            case 1:
                                gr.DrawLine(new Pen(ColorHelper.colors1[4]), new Point(this.Width - _splitdistance, 0), new Point(this.Width - _splitdistance, this.Height));
                                break;
                            case 2:
                                gr.DrawLine(new Pen(ColorHelper.colors2[4]), new Point(this.Width - _splitdistance, 0), new Point(this.Width - _splitdistance, this.Height));
                                break;
                            default:
                                break;
                        }
                    }

                }
                #endregion

                rect = new Rectangle(new Point(0, 0), new Size(this.Width - 1, this.Height - 1));
                pa = new GraphicsPath();
                DrawArc(rect, pa);
                gr.DrawPath(new Pen(colors[4], 0.9F), pa);

                pa.Dispose();
                lgbrush.Dispose();


            }
        }

        int offsetx = 0, offsety = 0, imageheight = 0, imagewidth = 0;
        public void DrawImage(Graphics gr)
        {
            if (this.Image != null)
            {
                offsety = _imageoffset; offsetx = _imageoffset;
                if (_imagelocation == e_imagelocation.Left | _imagelocation == e_imagelocation.Right)
                {
                    imageheight = this.Height - offsety * 2;
                    if (imageheight > _maximagesize.Y & _maximagesize.Y != 0)
                    { imageheight = _maximagesize.Y; }
                    imagewidth = (int)((Convert.ToDouble(imageheight) / this.Image.Height) * this.Image.Width);
                }
                else if (_imagelocation == e_imagelocation.Top | _imagelocation == e_imagelocation.Bottom)
                {
                    imagewidth = this.Width - offsetx * 2;
                    if (imagewidth > _maximagesize.X & _maximagesize.X != 0)
                    { imagewidth = _maximagesize.X; }
                    imageheight = (int)((Convert.ToDouble(imagewidth) / this.Image.Width) * this.Image.Height);

                }
                switch (_imagelocation)
                {
                    case e_imagelocation.Left:
                        gr.DrawImage(this.Image, new Rectangle(offsetx, offsety, imagewidth, imageheight));
                        break;
                    case e_imagelocation.Right:
                        gr.DrawImage(this.Image, new Rectangle(this.Width - imagewidth - offsetx, offsety, imagewidth, imageheight));
                        break;
                    case e_imagelocation.Top:
                        offsetx = this.Width / 2 - imagewidth / 2;
                        gr.DrawImage(this.Image, new Rectangle(offsetx, offsety, imagewidth, imageheight));
                        break;
                    case e_imagelocation.Bottom:
                        gr.DrawImage(this.Image, new Rectangle(offsetx, this.Height - imageheight - offsety, imagewidth, imageheight));
                        break;
                    default:
                        break;
                }
            }
        }

        public void DrawString(Graphics gr)
        {
            if (this.Text != "")
            {
                int textwidth = (int)gr.MeasureString(this.Text, this.Font).Width;
                int textheight = (int)gr.MeasureString(this.Text, this.Font).Height;

                int extraoffset = 0;
                Font fontb = new Font(this.Font, FontStyle.Bold);
                if (_title != "")
                {
                    extraoffset = textheight / 2;
                }

                string s1 = this.Text; string s2 = "";
                int jump = this.Text.IndexOf("\\n");

                if (jump != -1)
                {
                    s2 = s1.Substring(jump + 3); s1 = s1.Substring(0, jump);
                }

                switch (_imagelocation)
                {
                    case e_imagelocation.Left:
                        if (this.Title != "")
                        {
                            gr.DrawString(this.Title, fontb, new SolidBrush(ForeColor), new PointF(offsetx + imagewidth + 4, this.Font.Size / 2));
                            gr.DrawString(s1, this.Font, new SolidBrush(ForeColor), new PointF(offsetx + imagewidth + 4, 2 * this.Font.Size + 1));
                            gr.DrawString(s2, this.Font, new SolidBrush(ForeColor), new PointF(offsetx + imagewidth + 4, 3 * this.Font.Size + 4));
                        }
                        else
                        {
                            gr.DrawString(s1, this.Font, new SolidBrush(ForeColor),
                                new PointF(offsetx + imagewidth + 4,
                                    this.Height / 2 - this.Font.Size + 2.5f));
                        }
                        break;

                    case e_imagelocation.Right:
                        gr.DrawString(this.Title, fontb, new SolidBrush(ForeColor), new PointF(offsetx, this.Height / 2 - this.Font.Size + 1 - extraoffset));
                        gr.DrawString(this.Text, this.Font, new SolidBrush(ForeColor), new PointF(offsetx, extraoffset + this.Height / 2 - this.Font.Size + 1));
                        break;

                    case e_imagelocation.Top:
                        gr.DrawString(this.Text, this.Font, new SolidBrush(ForeColor), new PointF(this.Width / 2 - textwidth / 2 - 1, offsety + imageheight));
                        break;

                    case e_imagelocation.Bottom:
                        gr.DrawString(this.Text, this.Font, new SolidBrush(ForeColor), new PointF(this.Width / 2 - textwidth / 2 - 1, this.Height - imageheight - textheight - 1));
                        break;

                    case e_imagelocation.Center:
                        gr.DrawString(this.Text, this.Font, new SolidBrush(ForeColor),
                            new PointF(2 + this.Width / 2 - textwidth / 2 - 1,
                                3.1f + extraoffset + this.Height / 2 - this.Font.Size));
                        break;

                    case e_imagelocation.LeftCenter:
                        gr.DrawString(this.Text, this.Font, new SolidBrush(ForeColor), new PointF(2 + offsetx + imagewidth + 4, 2 + extraoffset + this.Height / 2 - this.Font.Size));
                        break;

                    default:
                        break;
                }

                fontb.Dispose();

            }
        }

        public void DrawArc(Rectangle re, GraphicsPath pa)
        {
            int _radiusX0Y0 = _radius,
                _radiusXFY0 = _radius,
                _radiusX0YF = _radius,
                _radiusXFYF = _radius;

            switch (_grouppos)
            {
                case e_groupPos.Left:
                    _radiusXFY0 = 1; _radiusXFYF = 1;
                    break;
                case e_groupPos.Center:
                    _radiusX0Y0 = 1; _radiusX0YF = 1; _radiusXFY0 = 1; _radiusXFYF = 1;
                    break;
                case e_groupPos.Right:
                    _radiusX0Y0 = 1; _radiusX0YF = 1;
                    break;
                case e_groupPos.Top:
                    _radiusX0YF = 1; _radiusXFYF = 1;
                    break;
                case e_groupPos.Bottom:
                    _radiusX0Y0 = 1; _radiusXFY0 = 1;
                    break;
            }
            pa.AddArc(re.X, re.Y, _radiusX0Y0, _radiusX0Y0, 180, 90);
            pa.AddArc(re.Width - _radiusXFY0, re.Y, _radiusXFY0, _radiusXFY0, 270, 90);
            pa.AddArc(re.Width - _radiusXFYF, re.Height - _radiusXFYF, _radiusXFYF, _radiusXFYF, 0, 90);
            pa.AddArc(re.X, re.Height - _radiusX0YF, _radiusX0YF, _radiusX0YF, 90, 90);
            pa.CloseFigure();

        }

        public void DrawShadow(Rectangle re, GraphicsPath pa)
        {
            int _radiusX0Y0 = _radius, _radiusXFY0 = _radius, _radiusX0YF = _radius, _radiusXFYF = _radius;
            switch (_grouppos)
            {
                case e_groupPos.Left:
                    _radiusXFY0 = 1; _radiusXFYF = 1;
                    break;
                case e_groupPos.Center:
                    _radiusX0Y0 = 1; _radiusX0YF = 1; _radiusXFY0 = 1; _radiusXFYF = 1;
                    break;
                case e_groupPos.Right:
                    _radiusX0Y0 = 1; _radiusX0YF = 1;
                    break;
                case e_groupPos.Top:
                    _radiusX0YF = 1; _radiusXFYF = 1;
                    break;
                case e_groupPos.Bottom:
                    _radiusX0Y0 = 1; _radiusXFY0 = 1;
                    break;
            }
            pa.AddArc(re.X, re.Y, _radiusX0Y0, _radiusX0Y0, 180, 90);
            pa.AddArc(re.Width - _radiusXFY0, re.Y, _radiusXFY0, _radiusXFY0, 270, 90);
            pa.AddArc(re.Width - _radiusXFYF, re.Height - _radiusXFYF, _radiusXFYF, _radiusXFYF, 0, 90);
            pa.AddArc(re.X, re.Height - _radiusX0YF, _radiusX0YF, _radiusX0YF, 90, 90);
            pa.CloseFigure();

        }

        public void DrawArrow(Graphics gr)
        {
            int _size = 1;
            switch (_arrow)
            {
                case e_arrow.ToDown:
                    if (_imagelocation == e_imagelocation.Left)
                    {
                        Point[] points = new Point[3];
                        points[0] = new Point(this.Width - 8 * _size - _imageoffset, this.Height / 2 - _size / 2);
                        points[1] = new Point(this.Width - 2 * _size - _imageoffset, this.Height / 2 - _size / 2);
                        points[2] = new Point(this.Width - 5 * _size - _imageoffset, this.Height / 2 + _size * 2);
                        gr.FillPolygon(new SolidBrush(ForeColor), points);
                    }
                    else if (_imagelocation == e_imagelocation.Top)
                    {
                        Point[] points = new Point[3];
                        points[0] = new Point(this.Width / 2 + 8 * _size - _imageoffset, this.Height - _imageoffset - 5 * _size);
                        points[1] = new Point(this.Width / 2 + 2 * _size - _imageoffset, this.Height - _imageoffset - 5 * _size);
                        points[2] = new Point(this.Width / 2 + 5 * _size - _imageoffset, this.Height - _imageoffset - 2 * _size);
                        gr.FillPolygon(new SolidBrush(ForeColor), points);
                    }
                    break;
                case e_arrow.ToRight:
                    if (_imagelocation == e_imagelocation.Left)
                    {
                        int arrowxpos = this.Width - _splitdistance + 2 * _imageoffset;
                        Point[] points = new Point[3];
                        points[0] = new Point(arrowxpos + 4, this.Height / 2 - 4 * _size);
                        points[1] = new Point(arrowxpos + 8, this.Height / 2);
                        points[2] = new Point(arrowxpos + 4, this.Height / 2 + 4 * _size);
                        gr.FillPolygon(new SolidBrush(ForeColor), points);
                    }
                    break;
                default:
                    break;
            }

        }

        public void FillSplit(Graphics gr)
        {
            Color _tranp = Color.FromArgb(200, 255, 255, 255);
            int x1 = this.Width - _splitdistance; int x2 = 0;
            int y1 = this.Height - _splitdistance; int y2 = 0;
            SolidBrush btransp = new SolidBrush(_tranp);

            #region Horizontal
            if (_imagelocation == e_imagelocation.Left)
            {
                if (xmouse < this.Width - _splitdistance & mouse) //Small button
                {
                    Rectangle _r = new Rectangle(x1 + 1, 1, this.Width - 2, this.Height - 1);
                    GraphicsPath p = new GraphicsPath();
                    int _rtemp = _radius; _radius = 4;
                    DrawArc(_r, p);
                    _radius = _rtemp;
                    gr.FillPath(btransp, p);

                }
                else if (mouse) //Big Button
                {
                    Rectangle _r = new Rectangle(x2 + 1, 1, this.Width - _splitdistance - 1, this.Height - 1);
                    GraphicsPath p = new GraphicsPath();
                    int _rtemp = _radius; _radius = 4;
                    DrawArc(_r, p);
                    _radius = _rtemp;
                    gr.FillPath(btransp, p);
                }

            }
            #endregion

            #region Vertical
            else if (_imagelocation == e_imagelocation.Top)
            {
                if (ymouse < this.Height - _splitdistance & mouse) //Small button
                {
                    Rectangle _r = new Rectangle(1, y1 + 1, this.Width - 1, this.Height - 1);
                    GraphicsPath p = new GraphicsPath();
                    int _rtemp = _radius; _radius = 4;
                    DrawArc(_r, p);
                    _radius = _rtemp;
                    gr.FillPath(btransp, p);
                }
                else if (mouse) //Big Button
                {
                    Rectangle _r = new Rectangle(1, y2 + 1, this.Width - 1, this.Height - _splitdistance - 1);
                    GraphicsPath p = new GraphicsPath();
                    int _rtemp = _radius; _radius = 4;
                    DrawArc(_r, p);
                    _radius = _rtemp;
                    gr.FillPath(btransp, p);
                }
            }
            #endregion

            btransp.Dispose();
        }

        #endregion

        #region About Fading

        int i_factor = 0;
        [DefaultValue(0)]
        public int FadingSpeed
        {
            get { return i_factor; }
            set
            {
                if (value > -1)
                {
                    i_factor = value;
                }
            }
        }

        #endregion

        #region Mouse Events
        int i_mode = 0; //0 Entering, 1 Out,2 Press
        int xmouse = 0, ymouse = 0;
        bool mouse = false;

        protected override void OnMouseEnter(EventArgs e)
        {
            mouse = true;
            i_mode = 1;
            xmouse = PointToClient(Cursor.Position).X;
            _tempshowbase = _showbase;
            _showbase = e_showbase.Yes;

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            UpdateLeave();
        }

        public void UpdateLeave()
        {
            if (_keeppress == false | (_keeppress == true & _ispressed == false))
            {
                _showbase = _tempshowbase;
                i_mode = 0;
                mouse = false;

            }
        }
        public bool ToFocused { get; set; }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _showbase = e_showbase.Yes;
            i_mode = 2;
            xmouse = PointToClient(Cursor.Position).X;
            ymouse = PointToClient(Cursor.Position).Y;
            mouse = true;

            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _showbase = e_showbase.Yes;
            i_mode = 1;
            mouse = true;

            #region ClickSplit
            if (_imagelocation == e_imagelocation.Left & xmouse > this.Width - _splitdistance & _splitbutton == e_splitbutton.Yes)
            {
                if (_arrow == e_arrow.ToDown)
                {
                    if (this.ContextMenuStrip != null)
                    {
                        this.ContextMenuStrip.Opacity = 1.0;
                        this.ContextMenuStrip.Show(this, 0, this.Height);
                    }

                }
                else if (_arrow == e_arrow.ToRight)
                {
                    if (this.ContextMenuStrip != null)
                    {
                        ContextMenuStrip menu = this.ContextMenuStrip;
                        this.ContextMenuStrip.Opacity = 1.0;
                        if (this.MenuPos.Y == 0)
                        {
                            this.ContextMenuStrip.Show(this, this.Width + 2, -this.Height);
                        }
                        else
                        {
                            this.ContextMenuStrip.Show(this, this.Width + 2, this.MenuPos.Y);
                        }
                    }

                }
            }
            else if (_imagelocation == e_imagelocation.Top & ymouse > this.Height - _splitdistance & _splitbutton == e_splitbutton.Yes)
            {
                if (_arrow == e_arrow.ToDown)
                {
                    if (this.ContextMenuStrip != null)
                        this.ContextMenuStrip.Show(this, 0, this.Height);
                }
            }
            #endregion
            else
            {
                base.OnMouseUp(mevent);

                #region Keep Press
                if (_keeppress)
                {
                    _ispressed = true;

                    try
                    {
                        foreach (Control _control in this.Parent.Controls)
                        {
                            if (typeof(ButtonW) == _control.GetType() & _control.Name != this.Name)
                            {
                                ((ButtonW)(_control))._ispressed = false;
                                ((ButtonW)(_control)).UpdateLeave();
                                ((ButtonW)(_control)).Refresh();
                            }
                        }
                    }
                    catch { }
                }
                #endregion

            }
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (mouse & this.SplitButton == e_splitbutton.Yes)
            {
                xmouse = PointToClient(Cursor.Position).X;
                ymouse = PointToClient(Cursor.Position).Y;
                this.Refresh();
            }
            base.OnMouseMove(mevent);
        }

        #endregion

        #region ISkyrayStyle ≥…‘±
        public void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:
                    this.bSilver = false;
                    this.Refresh();
                    break;
                case Style.Office2007Sliver:
                    this.bSilver = true;
                    this.Refresh();
                    break;

                default: break;
            }
        }

        private Style _Style = Style.Office2007Blue;

        //[DefaultValue(Style.Office2007Blue)]
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

        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                bSilver = !value;
            }
        }
    }
}

