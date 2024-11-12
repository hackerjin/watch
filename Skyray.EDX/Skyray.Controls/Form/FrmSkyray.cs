using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Skyray.API;
using System.Drawing.Text;
namespace Skyray.Controls
{
    public class FrmSkyray : Form, ISkyrayStyle
    {
        #region Fields

        private SkinFormRenderer _renderer;
        private RoundStyle _roundStyle = RoundStyle.All;
        private int _radius = 4;
        private int _captionHeight = 26;
        private Font _captionFont = SystemFonts.CaptionFont;
        private int _borderWidth = 6;
        private Size _minimizeBoxSize = new Size(32, 18);
        private Size _maximizeBoxSize = new Size(32, 18);
        private Size _closeBoxSize = new Size(32, 18);
        private Point _controlBoxOffset = new Point(6, 3);
        private int _controlBoxSpace = 2;
        private bool _active;
        private ControlBoxManager _controlBoxManager;
        private Padding _padding;
        private bool _canResize = false;
        private bool _inPosChanged;
        private ToolTip _toolTip;
        private static readonly object EventRendererChanged = new object();
        public static Icon IconMain { get; set; }

        #endregion

        #region Constructors

        public FrmSkyray()
            : base()
        {
            SetStyles();
            Init();
        }

        #endregion

        #region Events

        public event EventHandler RendererChangled
        {
            add { base.Events.AddHandler(EventRendererChanged, value); }
            remove { base.Events.RemoveHandler(EventRendererChanged, value); }
        }

        #endregion

        #region Properties

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SkinFormRenderer Renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = new SkinFormProfessionalRenderer();
                }
                return _renderer;
            }
            set
            {
                _renderer = value;
                OnRendererChanged(EventArgs.Empty);
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                base.Invalidate(new Rectangle(
                    0,
                    0,
                    Width,
                    CaptionHeight + 1));
            }
        }

        [DefaultValue(typeof(RoundStyle), "1")]
        public RoundStyle RoundStyle
        {
            get { return _roundStyle; }
            set
            {
                if (_roundStyle != value)
                {
                    _roundStyle = value;
                    SetReion();
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(8)]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value < 4 ? 4 : value;
                    SetReion();
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(24)]
        public int CaptionHeight
        {
            get { return _captionHeight; }
            set
            {
                if (_captionHeight != value)
                {
                    _captionHeight = value < _borderWidth ?
                                    _borderWidth : value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(3)]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                if (_borderWidth != value)
                {
                    _borderWidth = value < 1 ? 1 : value;
                }
            }
        }

        [DefaultValue(typeof(Font), "CaptionFont")]
        public Font CaptionFont
        {
            get { return _captionFont; }
            set
            {
                if (value == null)
                {
                    _captionFont = SystemFonts.CaptionFont;
                }
                else
                {
                    _captionFont = value;
                }
                base.Invalidate(CaptionRect);
            }
        }

        [DefaultValue(typeof(Size), "32, 18")]
        public Size MinimizeBoxSize
        {
            get { return _minimizeBoxSize; }
            set
            {
                if (_minimizeBoxSize != value)
                {
                    _minimizeBoxSize = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Size), "32, 18")]
        public Size MaximizeBoxSize
        {
            get { return _maximizeBoxSize; }
            set
            {
                if (_maximizeBoxSize != value)
                {
                    _maximizeBoxSize = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Size), "32, 18")]
        public Size CloseBoxSize
        {
            get { return _closeBoxSize; }
            set
            {
                if (_closeBoxSize != value)
                {
                    _closeBoxSize = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Point), "6, 0")]
        public Point ControlBoxOffset
        {
            get { return _controlBoxOffset; }
            set
            {
                if (_controlBoxOffset != value)
                {
                    _controlBoxOffset = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(-1)]
        public int ControlBoxSpace
        {
            get { return _controlBoxSpace; }
            set
            {
                if (_controlBoxSpace != value)
                {
                    _controlBoxSpace = value < 0 ? 0 : value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(true)]
        public bool CanResize
        {
            get { return _canResize; }
            set { _canResize = value; }
        }

        [DefaultValue(typeof(Padding), "0")]
        public new Padding Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;
                base.Padding = new Padding(
                    BorderWidth + _padding.Left,
                    CaptionHeight + _padding.Top,
                    BorderWidth + _padding.Right,
                    BorderWidth + _padding.Bottom);
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = FormBorderStyle.None; }
        }
        protected override Padding DefaultPadding
        {
            get
            {
                return new Padding(
                    BorderWidth,
                    CaptionHeight,
                    BorderWidth,
                    BorderWidth);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                if (!DesignMode)
                {
                    cp.Style |= (int)WindowStyles.WS_THICKFRAME;

                    if (ControlBox)
                    {
                        cp.Style |= (int)WindowStyles.WS_SYSMENU;
                    }

                    if (MinimizeBox)
                    {
                        cp.Style |= (int)WindowStyles.WS_MINIMIZEBOX;
                    }

                    if (!MaximizeBox)
                    {
                        cp.Style &= ~(int)WindowStyles.WS_MAXIMIZEBOX;
                    }

                    if (_inPosChanged)
                    {
                        cp.Style &= ~((int)WindowStyles.WS_THICKFRAME |
                            (int)WindowStyles.WS_SYSMENU);
                        cp.ExStyle &= ~((int)WindowExStyles.WS_EX_DLGMODALFRAME |
                            (int)WindowExStyles.WS_EX_WINDOWEDGE);
                    }
                }

                return cp;
            }
        }

        internal Rectangle CaptionRect
        {
            get { return new Rectangle(0, 0, Width, CaptionHeight); }
        }

        internal ControlBoxManager ControlBoxManager
        {
            get
            {
                if (_controlBoxManager == null)
                {
                    _controlBoxManager = new ControlBoxManager(this);
                }
                return _controlBoxManager;
            }
        }

        internal Rectangle IconRect
        {
            get
            {
                if (base.ShowIcon && base.Icon != null)
                {
                    int width = SystemInformation.SmallIconSize.Width;
                    if (CaptionHeight - BorderWidth - 4 < width)
                    {
                        width = CaptionHeight - BorderWidth - 4;
                    }
                    return new Rectangle(
                        BorderWidth,
                        BorderWidth + (CaptionHeight - BorderWidth - width) / 2,
                        width,
                        width);
                }
                return Rectangle.Empty;
            }
        }

        internal ToolTip ToolTip
        {
            get { return _toolTip; }
        }

        #endregion

        #region Override Methods

        protected virtual void OnRendererChanged(EventArgs e)
        {
            Renderer.InitSkinForm(this);
            EventHandler handler =
                base.Events[EventRendererChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
            base.Invalidate();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SetReion();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetReion();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            ControlBoxManager.ProcessMouseOperate(
                e.Location, MouseOperate.Move);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ControlBoxManager.ProcessMouseOperate(
                e.Location, MouseOperate.Down);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ControlBoxManager.ProcessMouseOperate(
                e.Location, MouseOperate.Up);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlBoxManager.ProcessMouseOperate(
                Point.Empty, MouseOperate.Leave);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            ControlBoxManager.ProcessMouseOperate(
                PointToClient(MousePosition), MouseOperate.Hover);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = ClientRectangle;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            SkinFormRenderer renderer = Renderer;
            renderer.DrawSkinFormBackground(
                new SkinFormBackgroundRenderEventArgs(
                this, g, rect));

            renderer.DrawSkinFormCaption(
                new SkinFormCaptionRenderEventArgs(
                this, g, CaptionRect, _active));

            renderer.DrawSkinFormBorder(
                new SkinFormBorderRenderEventArgs(
                this, g, rect, _active));

            if (ControlBoxManager.CloseBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(
                    new SkinFormControlBoxRenderEventArgs(
                    this,
                    g,
                    ControlBoxManager.CloseBoxRect,
                    _active,
                    ControlBoxStyle.Close,
                    ControlBoxManager.CloseBoxState));
            }

            if (ControlBoxManager.MaximizeBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(
                    new SkinFormControlBoxRenderEventArgs(
                    this,
                    g,
                    ControlBoxManager.MaximizeBoxRect,
                    _active,
                    ControlBoxStyle.Maximize,
                    ControlBoxManager.MaximizeBoxState));
            }

            if (ControlBoxManager.MinimizeBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(
                    new SkinFormControlBoxRenderEventArgs(
                    this,
                    g,
                    ControlBoxManager.MinimizeBoxRect,
                    _active,
                    ControlBoxStyle.Minimize,
                    ControlBoxManager.MinimizeBoxState));
            }

            int dis = 2;
            Brush brush = new SolidBrush(SkinFormColorTable.InnerFill);
            g.FillRectangle(brush, dis,
                this.CaptionHeight - 1,
                BorderWidth - dis,
                this.Height - CaptionHeight - BorderWidth + 1);//填充左空白

            g.FillRectangle(brush, this.Width - BorderWidth - 1,
                            this.CaptionHeight - 1, BorderWidth - dis,
                            this.Height - CaptionHeight - BorderWidth + 1);//填充右空白

            g.FillRectangle(brush, dis,
                this.Height - BorderWidth - 1,
                this.Width - dis * 2 - 1, BorderWidth - dis);//填充下边空白

            //g.DrawRectangle(new Pen(Color.LightGray), BorderWidth+2, CaptionHeight+2,
            //    this.Width - BorderWidth * 2-5,
            //    this.Height - CaptionHeight - BorderWidth-5);//内边框
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WinMsgs.WM_NCHITTEST:
                    WmNcHitTest(ref m);
                    break;
                case (int)WinMsgs.WM_NCPAINT:
                case (int)WinMsgs.WM_NCCALCSIZE:
                    break;
                case (int)WinMsgs.WM_WINDOWPOSCHANGED:
                    _inPosChanged = true;
                    base.WndProc(ref m);
                    _inPosChanged = false;
                    break;
                case (int)WinMsgs.WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(ref m);
                    break;
                case (int)WinMsgs.WM_NCACTIVATE:
                    WmNcActive(ref m);
                    break;
                //case (int)WinMsgs.WM_CLOSE:
                //    this.Hide();
                //    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_controlBoxManager != null)
                {
                    _controlBoxManager.Dispose();
                    _controlBoxManager = null;
                }

                _renderer = null;
                _toolTip.Dispose();
            }
        }

        #endregion

        #region Message Methods

        private void WmNcHitTest(ref Message m)
        {
            int wparam = m.LParam.ToInt32();
            Point point = new Point(
                MethodEx.LOW_ORDER(wparam),
                MethodEx.HIGH_ORDER(wparam));
            point = PointToClient(point);

            if (IconRect.Contains(point))
            {
                m.Result = new IntPtr(
                    (int)HitTest.HTSYSMENU);
                return;
            }

            if (_canResize)
            {
                if (point.X < 5 && point.Y < 5)
                {
                    m.Result = new IntPtr(
                        (int)HitTest.HTTOPLEFT);
                    return;
                }

                if (point.X > Width - 5 && point.Y < 5)
                {
                    m.Result = new IntPtr(
                        (int)HitTest.HTTOPRIGHT);
                    return;
                }

                if (point.X < 5 && point.Y > Height - 5)
                {
                    m.Result = new IntPtr(
                        (int)HitTest.HTBOTTOMLEFT);
                    return;
                }

                if (point.X > Width - 5 && point.Y > Height - 5)
                {
                    m.Result = new IntPtr(
                        (int)HitTest.HTBOTTOMRIGHT);
                    return;
                }

                if (point.Y < 3)
                {
                    m.Result = new IntPtr(
                        (int)HitTest.HTTOP);
                    return;
                }

                if (point.Y > Height - 3)
                {
                    m.Result = new IntPtr(
                        (int)HitTest.HTBOTTOM);
                    return;
                }

                if (point.X < 3)
                {
                    m.Result = new IntPtr(
                       (int)HitTest.HTLEFT);
                    return;
                }

                if (point.X > Width - 3)
                {
                    m.Result = new IntPtr(
                       (int)HitTest.HTRIGHT);
                    return;
                }
            }

            if (point.Y < CaptionHeight)
            {
                if (!ControlBoxManager.CloseBoxRect.Contains(point) &&
                    !ControlBoxManager.MaximizeBoxRect.Contains(point) &&
                    !ControlBoxManager.MinimizeBoxRect.Contains(point))
                {
                    m.Result = new IntPtr(
                      (int)HitTest.HTCAPTION);
                    return;
                }
            }
            m.Result = new IntPtr(
                     (int)HitTest.HTCLIENT);
        }

        private void WmGetMinMaxInfo(ref Message m)
        {
            MINMAXINFO minmax = (MINMAXINFO)Marshal.PtrToStructure(
                m.LParam, typeof(MINMAXINFO));

            if (MaximumSize != Size.Empty)
            {
                minmax.maxTrackSize = MaximumSize;
            }
            else
            {
                Rectangle rect = Screen.GetWorkingArea(this);

                minmax.maxPosition = new Point(
                    rect.X - BorderWidth,
                    rect.Y);
                minmax.maxTrackSize = new Size(
                    rect.Width + BorderWidth * 2,
                    rect.Height + BorderWidth);
            }

            if (MinimumSize != Size.Empty)
            {
                minmax.minTrackSize = MinimumSize;
            }
            else
            {
                minmax.minTrackSize = new Size(
                    CloseBoxSize.Width + MinimizeBoxSize.Width +
                    MaximizeBoxSize.Width + ControlBoxOffset.X +
                    ControlBoxSpace * 2 + SystemInformation.SmallIconSize.Width +
                    BorderWidth * 2 + 3,
                    CaptionHeight);
            }

            Marshal.StructureToPtr(minmax, m.LParam, false);
        }

        private void WmNcActive(ref Message m)
        {
            if (m.WParam.ToInt32() == 1)
            {
                _active = true;
            }
            else
            {
                _active = false;
            }
            m.Result = KnownParam.TRUE;

            base.Invalidate();
        }

        #endregion

        #region Private Methods

        private void SetStyles()
        {
            SetStyle(
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint |//用于自身有着特别绘制的控件
                ControlStyles.AllPaintingInWmPaint | //将绘制阶段折叠入Paint事件
                ControlStyles.OptimizedDoubleBuffer | //直到Paint返回,再显示绘制对象              
                ControlStyles.ResizeRedraw, //当调整控件大小时使整个工作区无效
                true);
            if (IconMain != null) this.Icon = IconMain;
            this.StartPosition = FormStartPosition.CenterScreen;
            // base.Opacity = 0.9;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            UpdateStyles();
        }

        private void SetReion()
        {
            if (base.Region != null)
            {
                base.Region.Dispose();
            }
            base.Region = Renderer.CreateRegion(this);
        }

        private void Init()
        {
            _toolTip = new ToolTip();
            base.FormBorderStyle = FormBorderStyle.None;
            Renderer.InitSkinForm(this);
            base.Padding = DefaultPadding;
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmSkyray
            // 
            this.ClientSize = new System.Drawing.Size(386, 181);
            this.Name = "FrmSkyray";
            this.ResumeLayout(false);

        }

        #region ISkyrayStyle 成员
        public void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:
//SkinFormColorTable.CaptionActive = Color.FromArgb(255, 96, 177, 254);
                    SkinFormColorTable.CaptionActive = Color.FromArgb(80, 128, 218);
                    SkinFormColorTable.Border = Color.FromArgb(80, 128, 218);
                    SkinFormColorTable.InnerFill = Color.FromArgb(255, 80, 128, 218);
                    this.Refresh();
                    break;
                case Style.Office2007Sliver:
                    SkinFormColorTable.CaptionActive = Color.FromArgb(199, 203, 209);
                    SkinFormColorTable.Border = Color.FromArgb(199, 203, 209);
                    SkinFormColorTable.InnerFill = Color.FromArgb(255, 199, 203, 209);
                    this.Refresh();
                    break;

                default: break;
            }
        }

        private Style _Style = Style.Office2007Blue;

        [DefaultValue(Style.Office2007Blue)]
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
