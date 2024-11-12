using System;
using System.Collections.Generic;
using ZedGraph;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.Controls.Extension
{
    [ToolboxBitmap(typeof(XRFChart), "Bitmap.XRFChart.bmp")] //定义在工具箱里显示图标

    public class TrendChart : ZedGraphControl
    {
        #region fields
        private int _growstyle;
        private WorkCurve _workCurve;
        private SpecEntity _spec;
        private bool _changescale;
        private int _defaultcolor;
        private bool _UnSpecing = true;
        private bool _IZero = false;

        private bool _bManualScale;  //yuzhao20150611:手动调节谱图增长

        public bool IsEDXRF = false;
        public bool IZero
        {
            get { return _IZero; }
            set { _IZero = value; }
        }

        private bool _IsActiveMove = true;

        private bool _IsFireHScrollEvent = true;

        private double _CurrentPointX;

        private double _CurrentPointY;

        private int pannelCount = 0;// Add by Strong 2013-3-5 记录画线次数，进行Refresh，防止显卡问题造成白线滞留

        /// <summary>
        /// isMultiGraph
        /// </summary>
        private bool _MultiGraph = false;

        /// <summary>
        /// TextObj,Use for display information before Spec
        /// </summary>
        public TextObj text;

        /// <summary>
        /// UseBoundary
        /// </summary>
        private bool _IUseBoundary;

        /// <summary>
        /// UseBase
        /// </summary>
        private bool _IUseBase;

        /// <summary>
        /// boundaryElement
        /// </summary>
        private string boundaryElement;

        /// <summary>
        /// intersting element area
        /// </summary>
        private List<SpecSplitInfo> lssi;

        /// <summary>
        /// the start point of panning
        /// </summary>
        private Point dragStartPt;

        /// <summary>
        /// XScaleMaxCoordy
        /// </summary>
        private int iXMaxChannel;

        /// <summary>
        /// YScaleMaxCoordy
        /// </summary>
        private int iYMaxChannel;

        /// <summary>
        /// the old pointx of horizontal move
        /// </summary>
        private int horizontalOldValue;

        /// <summary>
        /// the old pointx of vertical move
        /// </summary>
        private int verticalOldValue;


        /// <summary>
        /// is or not use scroll 
        /// </summary>
        private bool isUseScroll = true;

        /// <summary>
        /// ZoomNorrowChangeSpeed
        /// </summary>
        private double _ChangeSpeed = 0.025;

        /// <summary>
        /// defaultElementLineHeight
        /// </summary>
        private int defaultLineHeight;

        /// <summary>
        /// colorspositions
        /// </summary>
        private float[] positions;

        /// <summary>
        /// colors
        /// </summary>
        private Color[] colors;

        /// <summary>
        /// font size
        /// </summary>
        private int fontSize;

        private List<SpecListEntity> _VirtualSpecList;

        /// <summary>
        /// elementPeakFlag
        /// </summary>
        private List<ElementLine> elementPeakFlag;

        /// <summary>
        /// planning
        /// </summary>
        private bool planning = false;
        private bool supportDrag = false;

        /// <summary>
        /// demarcateEnergy
        /// </summary>
        private List<DemarcateEnergy> demarcateEnergys;

        /// <summary>
        /// CalculationPoints
        /// </summary>
        public PointF[] CalculationPoints;

        public PointF[] ScalePoints;

        /// <summary>
        /// CalculationWay
        /// </summary>
        public CalculationWay CalculationWay;

        /// <summary>
        /// atoms
        /// </summary>
        private DbObjectList<Atom> atoms;

        /// <summary>
        /// dEnergy
        /// </summary>
        private double dEnergy;

        /// <summary>
        /// iCurrentChannel
        /// </summary>
        private int iCurrentChannel;

        /// <summary>
        /// bShowElement
        /// </summary>
        private bool bShowElement;

        /// <summary>
        /// enableMoveLine
        /// </summary>
        private bool _enableMoveLine = true;

        /// <summary>
        /// enableWheel
        /// </summary>
        private bool _enableWheel = true;

        /// <summary>
        /// CurrentChannelCount
        /// </summary>
        private int _CurrentChannelCount;

        /// <summary>
        /// readyDeleteDemarcateEnergy
        /// </summary>
        private DemarcateEnergy readyDeleteDE;

        /// <summary>
        /// readyAddDemarcateEnergy
        /// </summary>
        private DemarcateEnergy readyAddDE;

        /// <summary>
        /// mouseX
        /// </summary>
        private int mouseX;

        /// <summary>
        /// mouseY
        /// </summary>
        private int mouseY;

        private double OldXMax;

        private double OldXMin;

        private int iInterest;

        private bool interestAreaEnable = false;

        private double[] coeff;

        /// <summary>
        /// Y放大倍数系数
        /// </summary>
        private int oldYcoeff;
        private bool _IsMainElement;
        private IContainer components;
        public event EventHandler ChannelChanged;
        public event EventHandler OnAddPeakFlag;
        public event EventHandler OnDelPeakFlag;
        public event EventHandler<GraphEventArgs> OnLeftBorder;
        public event EventHandler<GraphEventArgs> OnRightBorder;
        public event EventHandler<GraphEventArgs> OnLeftBase;
        public event EventHandler<GraphEventArgs> OnRightBase;
        public event EventHandler OnVirtualSpec;
        private System.Windows.Forms.VScrollBar scrollBarVertical;
        private System.Windows.Forms.HScrollBar scrollBarHorizontal;

        public event EventHandler<HorizontalEventArgs> ChartHChangedEvent; //yuzhao20150624：水平缩放事件
        #endregion


        #region properties
        public string XTitle
        {
            get { return this.GraphPane.XAxis.Title.Text; }
            set { this.GraphPane.XAxis.Title.Text = value; }
        }
        public string YTitle
        {
            get { return this.GraphPane.YAxis.Title.Text; }
            set { this.GraphPane.YAxis.Title.Text = value; }
        }
        /// <summary>
        /// 可以选择感兴趣区域的标志
        /// </summary>
        public bool InterestAreaEnable
        {
            get { return interestAreaEnable; }
            set
            {
                interestAreaEnable = value;
                if (value == true)
                {
                    if (interestAreaList == null)
                        interestAreaList = new List<SpecSplitInfo>();
                    iInterest = 1;
                    this.Cursor = System.Windows.Forms.Cursors.Cross;
                    newInterestArea = new SpecSplitInfo("", 0, 0, Color.Red);
                }
                else
                {
                    iInterest = 0;
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
        }
        /// <summary>
        /// 新的感兴趣区域
        /// </summary>
        private SpecSplitInfo newInterestArea;
        /// <summary>
        /// 当前感兴趣区域
        /// </summary>
        public SpecSplitInfo currentInterestArea { get; set; }
        /// <summary>
        /// 存放所有感兴趣区域
        /// </summary>
        private List<SpecSplitInfo> interestAreaList;

        /// <summary>
        /// 水平同步
        /// </summary>
        public bool HorizontalSynchronization { get; set; }

        /// <summary>
        /// 垂直同步
        /// </summary>
        public bool VerticalSynchronization { get; set; }

        public bool UnSpecing
        {
            get { return _UnSpecing; }
            set { _UnSpecing = value; }
        }

        public bool IsActiveMove
        {
            get { return _IsActiveMove; }
            set { _IsActiveMove = value; }
        }

        private int MaxValue
        {
            get { return this.scrollBarHorizontal.Maximum - this.scrollBarHorizontal.LargeChange + 1; }
        }

        public bool MultiGraph
        {
            get { return _MultiGraph; }
            set { _MultiGraph = value; }
        }

        public double[] Coeff
        {
            get { return coeff; }
            set { coeff = value; }
        }

        public double ChangeSpeed
        {
            get { return _ChangeSpeed; }
            set { _ChangeSpeed = value; }
        }

        public bool IUseBoundary
        {
            get { return _IUseBoundary; }
            set { _IUseBoundary = value; }
        }

        public bool IsAdmin = true;

        
        private int yRemerber = 1000;

        private PaneList paneList;
        public bool IUseBase
        {
            get { return _IUseBase; }
            set { _IUseBase = value; }
        }
   
        public string BoundaryElement
        {
            get { return boundaryElement; }
            set { boundaryElement = value; }
        }

        [Browsable(false), DefaultValue(null)]
        public List<SpecSplitInfo> Lssi
        {
            get { return lssi; }
            set { lssi = value; }
        }

        public int DefaultLineHeight
        {
            get { return defaultLineHeight; }
            set { defaultLineHeight = value; }
        }

        public Dictionary<int, string[]> AtomNamesDic = new Dictionary<int, string[]>();

        public Dictionary<int, int[]> AtomLinesDic = new Dictionary<int, int[]>();

        public Dictionary<int, List<ElementLine>> ElementLineDic = new Dictionary<int, List<ElementLine>>();

        public Dictionary<int, List<SpecData>> SpecDataDic = new Dictionary<int, List<SpecData>>();

        //public Dictionary<int, List<SpecData>> ViceSpecDataDic = new Dictionary<int, List<SpecData>>(); //拟合后的谱  
        private ButtonW btnLeft;
        private ButtonW btnRight;
        private ButtonW btnUp;
        private ButtonW btnDown;
        private ButtonW btnReduction;

        public bool BtnVisable
        {
            set
            {
                //btnLeft.Visible = value;
                //btnRight.Visible = value;
                //btnUp.Visible = value;
                //btnDown.Visible = value;
                //btnReduction.Visible = value;
            }
        }

        public int CurrentSpecPanel = 0;

        public float[] Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }



        public double DEnergy
        {
            get { return dEnergy; }
            set { dEnergy = value; }
        }

        public int ICurrentChannel
        {
            get { return iCurrentChannel; }
            set
            {
                iCurrentChannel = value;

                if (ChannelChanged != null) ChannelChanged(null, null);
            }
        }

        public bool EnableMoveLine
        {
            get { return _enableMoveLine; }
            set { _enableMoveLine = value; }
        }

        public int CurrentChannelCount
        {
            get { return _CurrentChannelCount; }
            set { _CurrentChannelCount = value; }
        }
        [Browsable(false), DefaultValue(null)]
        public DemarcateEnergy ReadyDeleteDE
        {
            get { return readyDeleteDE; }
            set { readyDeleteDE = value; }
        }
        [Browsable(false), DefaultValue(null)]
        public DemarcateEnergy ReadyAddDE
        {
            get { return readyAddDE; }
            set { readyAddDE = value; }
        }

        /// <summary>
        /// 是否显示峰标志
        /// </summary>
        public bool IsShowPeakFlagAuto { get; set; }

        /// <summary>
        /// XScaleMaxCoordy
        /// </summary>
        public int IXMaxChannel
        {
            get { return iXMaxChannel; }
            set
            {
                iXMaxChannel = value;
                this.scrollBarHorizontal.Maximum = iXMaxChannel;
                this.scrollBarHorizontal.LargeChange = iXMaxChannel + 1;
            }
        }

        /// <summary>
        /// YScaleMaxCoordy
        /// </summary>
        public int IYMaxChannel
        {
            get { return iYMaxChannel; }
            set 
            { 
                iYMaxChannel = value;
                
            
            }
        }

        /// <summary>
        /// is or not use scroll
        /// </summary>
        public bool IsUseScroll
        {
            get { return isUseScroll; }
            set
            {
                isUseScroll = value;
                scrollVisible(value);
                BtnVisable = value;
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        public bool BShowElement
        {
            get { return bShowElement; }
            set
            {
                bShowElement = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// font size
        /// </summary>
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        public bool EnableWheel
        {
            get { return _enableWheel; }
            set { _enableWheel = value; }
        }

        public bool IsMainElement
        {
            get { return _IsMainElement; }
            set { _IsMainElement = value; }
        }

        #endregion

        #region constructor

        public TrendChart()
            : base()
        {
            ////var myPane = base.MasterPane.PaneList[0];
            ////myPane.Title.IsVisible = false;
            ////myPane.XAxis.Title.Text = "";
            ////myPane.XAxis.Title.IsVisible = false;
            ////myPane.YAxis.Title.Text = "";
            ////myPane.YAxis.MajorTic.IsInside = false;
            ////myPane.YAxis.MinorTic.IsInside = false;
            ////myPane.XAxis.MajorTic.IsInside = false;
            ////myPane.XAxis.MinorTic.IsInside = false;
            ////this.SetStyle(ControlStyles.SupportsTransparentBackColor |
            ////              ControlStyles.UserPaint |
            ////              ControlStyles.ResizeRedraw |
            ////              ControlStyles.DoubleBuffer, true);
            ////this.SetStyle(ControlStyles.Opaque, false);
            ////this.InitParam();
            ////myPane.XAxis.Scale.FontSpec.Size = 12f;
            ////myPane.YAxis.Scale.FontSpec.Size = 12f;
            ////myPane.YAxis.Title.FontSpec.Size = 12f;
            ////base.IsEnableWheelZoom = false;
            ////MultiGraph = false;
            ////myPane.XAxis.Scale.BaseTic = 0;
            //////myPane.XAxis.Scale.Max = 2048;
            //////myPane.X2Axis.Scale.Max = 2048;
            ////////myPane.XAxis.Type = AxisType.Date;
            ////////myPane.XAxis.Scale.Format = "HH:mm:ss";
            ////////myPane.X2Axis.Type = AxisType.Date;
            ////////myPane.X2Axis.Scale.Format = "HH:mm:ss";
            ////myPane.YAxis.Scale.Max = 1000;
            ////myPane.Y2Axis.Scale.Max = 1000;
            this.AxisChange();
            this.MouseMoveEvent += new ZedMouseEventHandler(MyZedGraph_MouseMoveEvent);
           // this.MouseDownEvent += new ZedMouseEventHandler(MyZedGraph_MouseDownEvent);
            this.MouseUpEvent += new ZedMouseEventHandler(MyZedGraph_MouseUpEvent);
            this.MouseEnter += new EventHandler(XRFChart_MouseEnter);
            this.MouseLeave += new EventHandler(MyZedGraph_MouseLeave);
            this.MouseWheel += new MouseEventHandler(MyZedGraph_MouseWheel);
            this.SizeChanged += new EventHandler(MyZedGraph_SizeChanged);
            ////this.DockChanged += new EventHandler(MyZedGraph_SizeChanged);
            this.MouseDoubleClick += new MouseEventHandler(MyZedGraph_SizeChanged);
            ////this.MouseClick += new MouseEventHandler(XRFChart_MouseClick);
            this.InitializeComponent();
            positions = new float[] { 0f, 1f };
            colors = new Color[] { Color.YellowGreen, Color.YellowGreen };
            text = new TextObj("", 75, 85, CoordType.ChartFraction, AlignH.Center, AlignV.Center);
            text.Tag = "InfoTitle";
            this.GraphPane.GraphObjList.Add(text);
            paneList = base.MasterPane.PaneList;
            HorizontalSynchronization = true;
            VerticalSynchronization = true;
            //interestAreaList = new List<SpecSplitInfo>();
            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Enabled = false;
            oldYcoeff = 1;
        }

        public void SetAxisXLabel()
        {
            if (ChartHChangedEvent != null)                                       //yuzhao20150624:X轴坐标变化事件
                ChartHChangedEvent(this, new HorizontalEventArgs()
                {
                    XMax = this.MasterPane.PaneList[0].XAxis.Scale.Max,
                    XMin = this.MasterPane.PaneList[0].XAxis.Scale.Min
                });
            if (lssi != null && lssi.Count > 0)
            {
                this.GraphPane.GraphObjList.RemoveAll(obj => obj.Tag.ToString() == "element");
                foreach (SpecSplitInfo ssi in lssi)
                {
                    AddLineAxisXLabel(ssi);
                }
            }
        }

        private void AddLineAxisXLabel(SpecSplitInfo ssi)
        {
            foreach (var pan in this.MasterPane.PaneList)
            {
                var zoom = pan.CalcScaleFactor();
                var lblPos = (ssi.X2 + ssi.X1) / 2;
                var max = pan.XAxis.Scale.Max;
                var min = pan.XAxis.Scale.Min;
                if (lblPos < max && lblPos > min)
                {
                    var dd = (lblPos - min) / (max - min);
                    TextObj tt = new TextObj(ssi.Element, dd, 1.005,
                             CoordType.ChartFraction, AlignH.Left, AlignV.Top);
                    tt.FontSpec.Size = 12 / zoom;
                    tt.FontSpec.Fill.IsVisible = false;
                    tt.FontSpec.IsBold = true;
                    tt.FontSpec.FontColor = ssi.Color;
                    tt.FontSpec.Border.IsVisible = false;
                    tt.Tag = "element";
                    pan.GraphObjList.Add(tt);
                }
            }
            //var zoom = this.GraphPane.CalcScaleFactor();
            //var lblPos = (ssi.X2 + ssi.X1) / 2;
            //var max = this.GraphPane.XAxis.Scale.Max;
            //var min = this.GraphPane.XAxis.Scale.Min;
            //if (lblPos < max && lblPos > min)
            //{
            //    var dd = (lblPos - min) / (max - min);
            //    TextObj tt = new TextObj(ssi.Element, dd, 1.005,
            //             CoordType.ChartFraction, AlignH.Left, AlignV.Top);
            //    tt.FontSpec.Size = 12 / zoom;
            //    tt.FontSpec.Fill.IsVisible = false;
            //    tt.FontSpec.IsBold = true;
            //    tt.FontSpec.FontColor = ssi.Color;
            //    tt.FontSpec.Border.IsVisible = false;
            //    this.GraphPane.GraphObjList.Add(tt);
            //}
        }

        /// <summary>
        /// InitParam
        /// </summary>
        private void InitParam()
        {
            elementPeakFlag = new List<ElementLine>();
            //specDataList = new List<SpecData>();
            ////elementLineList = new List<ElementLine>();
            //_VirtualSpecList = new List<SpecList>();
            IXMaxChannel = 240;
            IYMaxChannel = 1000;
            _ChangeSpeed = 0.25;
            defaultLineHeight = 30;
            fontSize = 12;
            UnSpecing = true;
        }

        /// <summary>
        /// InitializeComponent
        /// </summary>
        private void InitializeComponent()
        {
            this.btnReduction = new Skyray.Controls.ButtonW();
            this.btnDown = new Skyray.Controls.ButtonW();
            this.btnUp = new Skyray.Controls.ButtonW();
            this.btnRight = new Skyray.Controls.ButtonW();
            this.btnLeft = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // btnReduction
            // 
            this.btnReduction.BackColor = System.Drawing.Color.Transparent;
            this.btnReduction.bSilver = false;
            this.btnReduction.Image = global::Skyray.Controls.Extension.Resource.ComputeMatching;
            this.btnReduction.ImageLocation = Skyray.Controls.ButtonW.e_imagelocation.Top;
            this.btnReduction.Location = new System.Drawing.Point(15, 20);
            this.btnReduction.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnReduction.MenuPos = new System.Drawing.Point(0, 0);
            this.btnReduction.Name = "btnReduction";
            this.btnReduction.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnReduction.Size = new System.Drawing.Size(24, 24);
            this.btnReduction.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnReduction.TabIndex = 7;
            this.btnReduction.ToFocused = false;
            this.btnReduction.UseVisualStyleBackColor = false;
            this.btnReduction.Visible = true;
            this.btnReduction.MouseLeave += new System.EventHandler(this.btnUp_MouseLeave);
            this.btnReduction.Click += new System.EventHandler(this.btnReduction_Click);
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.Transparent;
            this.btnDown.bSilver = false;
            this.btnDown.Image = global::Skyray.Controls.Extension.Resource.blue_down;
            this.btnDown.ImageLocation = Skyray.Controls.ButtonW.e_imagelocation.Top;
            this.btnDown.Location = new System.Drawing.Point(102, 81);
            this.btnDown.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDown.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDown.Name = "btnDown";
            this.btnDown.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnDown.Size = new System.Drawing.Size(24, 24);
            this.btnDown.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDown.TabIndex = 6;
            this.btnDown.ToFocused = false;
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Visible = false;
            this.btnDown.MouseLeave += new System.EventHandler(this.btnUp_MouseLeave);
            this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnDown_MouseDown);
            this.btnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.Transparent;
            this.btnUp.bSilver = false;
            this.btnUp.Image = global::Skyray.Controls.Extension.Resource.blue_up;
            this.btnUp.ImageLocation = Skyray.Controls.ButtonW.e_imagelocation.Top;
            this.btnUp.Location = new System.Drawing.Point(102, 31);
            this.btnUp.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnUp.MenuPos = new System.Drawing.Point(0, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnUp.Size = new System.Drawing.Size(24, 24);
            this.btnUp.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnUp.TabIndex = 5;
            this.btnUp.ToFocused = false;
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Visible = false;
            this.btnUp.MouseLeave += new System.EventHandler(this.btnUp_MouseLeave);
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseDown);
            this.btnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // btnRight
            // 
            this.btnRight.BackColor = System.Drawing.Color.Transparent;
            this.btnRight.bSilver = false;
            this.btnRight.Image = global::Skyray.Controls.Extension.Resource.blue_right;
            this.btnRight.ImageLocation = Skyray.Controls.ButtonW.e_imagelocation.Top;
            this.btnRight.Location = new System.Drawing.Point(127, 56);
            this.btnRight.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnRight.MenuPos = new System.Drawing.Point(0, 0);
            this.btnRight.Name = "btnRight";
            this.btnRight.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnRight.Size = new System.Drawing.Size(24, 24);
            this.btnRight.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnRight.TabIndex = 4;
            this.btnRight.ToFocused = false;
            this.btnRight.UseVisualStyleBackColor = false;
            this.btnRight.Visible = false;
            this.btnRight.MouseLeave += new System.EventHandler(this.btnUp_MouseLeave);
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseDown);
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // btnLeft
            // 
            this.btnLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnLeft.bSilver = false;
            this.btnLeft.Image = global::Skyray.Controls.Extension.Resource.blue_left;
            this.btnLeft.ImageLocation = Skyray.Controls.ButtonW.e_imagelocation.Top;
            this.btnLeft.Location = new System.Drawing.Point(77, 56);
            this.btnLeft.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnLeft.MenuPos = new System.Drawing.Point(0, 0);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnLeft.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnLeft.Size = new System.Drawing.Size(24, 24);
            this.btnLeft.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnLeft.TabIndex = 3;
            this.btnLeft.ToFocused = false;
            this.btnLeft.UseVisualStyleBackColor = false;
            this.btnLeft.Visible = false;
            this.btnLeft.MouseLeave += new System.EventHandler(this.btnUp_MouseLeave);
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLeft_MouseDown);
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // TrendChart
            // 
            this.Controls.Add(this.btnReduction);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "TrendChart";
            this.Size = new System.Drawing.Size(444, 297);
            this.Controls.SetChildIndex(this.btnLeft, 0);
            this.Controls.SetChildIndex(this.btnRight, 0);
            this.Controls.SetChildIndex(this.btnUp, 0);
            this.Controls.SetChildIndex(this.btnDown, 0);
            this.Controls.SetChildIndex(this.btnReduction, 0);
            this.ResumeLayout(false);

        }

        protected override void OnCreateControl()
        {
            if (!DesignMode)
                initScroll();
            base.OnCreateControl();
        }

        /// <summary>
        /// InitScroll
        /// </summary>
        private void initScroll()
        {
            this.scrollBarVertical = new VScrollBar();
            this.scrollBarVertical.Name = "scrollBarVertical";
            this.scrollBarVertical.Maximum = 1000;
            this.scrollBarVertical.LargeChange = 10;
            this.scrollBarVertical.Value = this.scrollBarVertical.Maximum - scrollBarVertical.LargeChange + 1;

            this.scrollBarVertical.Visible = true;
            this.scrollBarVertical.Scroll += new ScrollEventHandler(scrollBarVertical_Scroll);
            this.Controls.Add(this.scrollBarVertical);
            this.scrollBarHorizontal = new HScrollBar();

            // this.scrol0lBarHorizontal.LargeChange = this.scrollBarHorizontal.Maximum;

            this.scrollBarHorizontal.Name = "scrollBarHorizontal";
            this.scrollBarHorizontal.Visible = false;
            this.scrollBarHorizontal.Scroll += new ScrollEventHandler(scrollBarHorizontal_Scroll);
            //this.scrollBarHorizontal.Orientation = Skyray.Controls.ScrollBarOrientation.Horizontal;
            this.Controls.Add(this.scrollBarHorizontal);
            this.scrollBarHorizontal.Enabled = false;
            this.scrollBarVertical.Enabled = true;

        }

        #endregion

        #region event

       

        /// <summary>
        /// 鼠标点击，删除/添加峰标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void XRFChart_MouseClick(object sender, MouseEventArgs e)
        {
            this.Focus();
            
        }

        /// <summary>
        /// 鼠标滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyZedGraph_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_enableWheel)
            {
                //X轴
                ////if (e.Delta < 0)//缩小
                ////{
                ////    if (_CurrentPointX > 0)
                ////        hNarrow(_CurrentPointX);
                ////}
                ////else
                ////{
                ////    if (_CurrentPointX > 0)
                ////        hZoom(_CurrentPointX);
                ////    //vZoom(_CurrentPointY);
                ////}

                //Y轴
                if (e.Delta < 0)//缩小
                {
                    if (_CurrentPointY > 0)
                        vNarrow(_CurrentPointY); 
                }
                else
                {
                    if (_CurrentPointY > 0)
                        vZoom(_CurrentPointY);
                    //vZoom(_CurrentPointY);
                }

            }
        }

        public void SetXRange(double dMin, double dMax)
        {
            dMin = dMin < 0 ? 0 : dMin;
            dMax = dMax > iXMaxChannel ? iXMaxChannel : dMax;
            this.MasterPane.PaneList[0].XAxis.Scale.Max = dMax;
            this.MasterPane.PaneList[0].XAxis.Scale.Min = dMin;
            this.MasterPane.PaneList[0].X2Axis.Scale.Max = dMax;
            this.MasterPane.PaneList[0].X2Axis.Scale.Min = dMin;
            RefreshHScrollBarValue();
            SetAxisXLabel();
            this.AxisChange();
            this.Refresh();
        }

        [System.Runtime.InteropServices.DllImport("gdi32", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int GetPixel(int hdc, int X, int y);
        [System.Runtime.InteropServices.DllImport("user32", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int WindowFromPoint(int xPoint, int yPoint);
        [System.Runtime.InteropServices.DllImport("user32", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int GetDC(int hwnd);

        private bool MultiPannelProcess(RectangleF rect, Point pf, int i, int panlId)
        {
            if (rect.Contains(pf))
            {
                Point mousePt = pf;
                double x, y;
                base.MasterPane.PaneList[panlId].ReverseTransform(mousePt, out x, out y);
                //string xStr = x.ToString("0");
                _CurrentPointX = x;
                _CurrentPointY = y;
                try
                {
                    ICurrentChannel = int.Parse(Math.Round(x).ToString());
                }
                catch
                {
                    if (x > iXMaxChannel)
                        ICurrentChannel = iXMaxChannel;
                    else ICurrentChannel = 0;
                }
                if (iCurrentChannel <= 0)
                {
                    iCurrentChannel = 0;
                }
                else if (iCurrentChannel >= iXMaxChannel)
                {
                    iCurrentChannel = iXMaxChannel;
                }

                positionInlist = GetPosition(x);
                if (positionInlist < 0) return false; 
                bool flag = false;
                List<SpecData> specDataList;
                if (SpecDataDic.TryGetValue(i, out specDataList))
                {
                    if (specDataList != null && specDataList.Count > 0)
                    {
                        if (iCurrentChannel >= iXMaxChannel && specDataList.Count > iXMaxChannel)
                        {
                            CurrentChannelCount = int.Parse(Math.Round(specDataList[iXMaxChannel - 1].dY).ToString());
                            flag = true;
                        }
                        else if (specDataList.Count > iCurrentChannel)
                        {
                            CurrentChannelCount = int.Parse(Math.Round(specDataList[iCurrentChannel].dY).ToString());
                            flag = true;
                        }

                    }
                    if (!flag)
                        return true;
                }
                dEnergy = DemarcateEnergyHelp.GetEnergy(iCurrentChannel);
                if (dEnergy <= 0)
                {
                    dEnergy = 0;
                }
                if (planning && supportDrag)
                {
                    HandlePan(mousePt);
                }
                int top = (int)this.MasterPane.PaneList[panlId].Chart.Rect.Top;
                int left = (int)this.MasterPane.PaneList[panlId].Chart.Rect.Left;
                int bottom = (int)this.MasterPane.PaneList[panlId].Chart.Rect.Bottom;
                int right = (int)this.MasterPane.PaneList[panlId].Chart.Rect.Right;
                if (this.MasterPane.PaneList.Count == 1)
                {
                    if ((mouseX > 0) && (mouseY > 0))
                    {
                        // ControlPaint.DrawReversibleLine(PointToScreen(new Point(mouseX, top)), PointToScreen(new Point(mouseX, bottom)), Color.Black); //根据具体颜色的背景画可逆的线
                        mouseX = -1;
                        mouseY = -1;
                        //刷新界面解决白线滞留尝试 2014-04-23
                        this.Refresh();
                    }
                    if ((pf.X > left && pf.X < right) && (pf.Y < bottom && pf.Y > top))
                    {
                        // Add by Strong 2013-3-5 记录画线次数，进行Refresh，防止显卡问题造成白线滞留
                        ++pannelCount;
                        if (pannelCount > 30)
                        {
                            pannelCount = 0;
                            //this.Refresh();
                        }
                        //ControlPaint.DrawReversibleLine(PointToScreen(new Point(pf.X, top)), PointToScreen(new Point(pf.X, bottom)), Color.Black); //画鼠标当前所在点的线
                        using (Graphics g = this.CreateGraphics())
                        {
                            int h = WindowFromPoint(pf.X, pf.Y);
                            int hD = GetDC(h);
                            int c = GetPixel(hD, pf.X, pf.Y);  //取屏幕点颜色
                            if (c != -1 && c != 0)
                            {
                                int red = c % 256;

                                int green = (c / 256) % 256;

                                int blue = c / 256 / 256;
                                g.DrawLine(new Pen(Color.FromArgb(255 - red, 255 - green, 255 - blue)), new Point(pf.X, top), new Point(pf.X, bottom)); //画对比颜色的线
                                Console.WriteLine("取色正常-" + Color.FromArgb(255 - red, 255 - green, 255 - blue).ToString());
                            }
                            else
                            {
                                g.DrawLine(new Pen(Color.FromArgb(this.GraphPane.Chart.Fill.Color.A, 255 - this.GraphPane.Chart.Fill.Color.R, 255 - this.GraphPane.Chart.Fill.Color.G, 255 - this.GraphPane.Chart.Fill.Color.B)), new Point(pf.X, top), new Point(pf.X, bottom));//画对比颜色的线
                                Console.WriteLine("取色不正常-" + Color.FromArgb(this.GraphPane.Chart.Fill.Color.A, 255 - this.GraphPane.Chart.Fill.Color.R, 255 - this.GraphPane.Chart.Fill.Color.G, 255 - this.GraphPane.Chart.Fill.Color.B).ToString());
                            }

                        }
                        //g.DrawLine(new Pen(Color.FromArgb(this.GraphPane.Chart.Fill.Color.A, 255 - this.GraphPane.Chart.Fill.Color.R, 255 - this.GraphPane.Chart.Fill.Color.G, 255 - this.GraphPane.Chart.Fill.Color.B)), new Point(pf.X, top), new Point(pf.X, bottom));
                        mouseX = pf.X;
                        mouseY = pf.Y;

                    }
                }
                return true;
            }
            else
            {
                mouseX = -1;
                mouseY = -1;
            }
            return false;
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        ////bool MyZedGraph_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)
        ////{
        ////    if (_enableMoveLine && _IsActiveMove)
        ////    {
        ////        if (this.MasterPane.PaneList.Count > 1)
        ////        {
        ////            for (int i = 0; i < this.MasterPane.PaneList.Count; i++)
        ////            {
        ////                float scaleFactor = this.MasterPane.CalcScaleFactor();
        ////                RectangleF rect = this.MasterPane.PaneList[i].CalcClientRect(this.CreateGraphics(), scaleFactor);
        ////                Point pf = new Point(e.X, e.Y);
        ////                if (MultiPannelProcess(rect, pf, i, i))
        ////                    break;
        ////            }
        ////        }
        ////        else if (this.MasterPane.PaneList.Count == 1)
        ////        {
        ////            float scaleFactor = this.MasterPane.CalcScaleFactor();
        ////            RectangleF rect = this.MasterPane.PaneList[0].CalcClientRect(this.CreateGraphics(), scaleFactor);
        ////            Point pf = new Point(e.X, e.Y);
        ////            MultiPannelProcess(rect, pf, CurrentSpecPanel - 1, 0);
        ////            //Console.WriteLine("鼠标移动");
        ////        }
        ////    }
        ////    return true;
        ////}

        bool MyZedGraph_MouseUpEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                planning = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            return true;
        }

        bool MyZedGraph_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (InterestAreaEnable)//选择新的区域
                {
                    //Strong 2013-3-2 因WIN8系统会有很多竖线去除不掉，所以在鼠标点击时不画竖线
                    //mouseX = -1;
                    //mouseY = -1;
                    if (iInterest == 1)
                    {
                        newInterestArea.X1 = iCurrentChannel;
                        iInterest++;
                    }
                    else if (iInterest == 2)
                    {
                        newInterestArea.X2 = iCurrentChannel;
                        //选择颜色
                        ColorDialog cd = new ColorDialog();
                        cd.FullOpen = true;
                        cd.Color = Color.Red;
                        if (cd.ShowDialog() == DialogResult.OK)
                        {
                            newInterestArea.Color = cd.Color;
                        }
                        interestAreaList.Add(newInterestArea);
                        SetSpecData(_workCurve, this._VirtualSpecList, _spec, _changescale, _defaultcolor, _growstyle);
                        InterestAreaEnable = false;
                    }
                }
                else//选择当前区域
                {
                    if (interestAreaList == null || interestAreaList.Count == 0)
                    {
                        dragStartPt = e.Location;
                        planning = true;
                        //Strong 2013-3-2 因WIN8系统会有很多竖线去除不掉，所以在鼠标点击时不画竖线
                        //mouseX = -1;
                        //mouseY = -1;
                        return true;
                    }
                    else
                    {
                        foreach (var sp in interestAreaList)
                        {
                            if ((iCurrentChannel >= sp.X1 && iCurrentChannel <= sp.X2) || (iCurrentChannel <= sp.X1 && iCurrentChannel >= sp.X2))
                            {
                                currentInterestArea = sp;
                                SetSpecData(_workCurve, this._VirtualSpecList, _spec, _changescale, _defaultcolor, _growstyle);
                               
                                break;
                            }
                        }
                    }
                    //Strong 2013-3-2 因WIN8系统会有很多竖线去除不掉，所以在鼠标点击时不画竖线
                    //mouseX = -1;
                    //mouseY = -1;
                    this.Refresh();
                }
                //dragStartPt = e.Location;
                //planning = true;
            }
            return true;
        }

        void XRFChart_MouseEnter(object sender, EventArgs e)
        {
            this.Refresh();
        }

        void MyZedGraph_MouseLeave(object sender, EventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// ControlSizeChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyZedGraph_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.SuspendLayout();
                foreach (var pane in this.MasterPane.PaneList)
                {
                    pane.IsFontsScaled = false;
                }
                SetAxisXLabel();
                mouseX = -1;
                mouseY = -1;
               
                ChangeScroll();
                this.ResumeLayout(false);

                if (this.text != null && this.text.Text != string.Empty)
                {
                    WriteInformation(this.text.Text);
                }
            }
            catch (Exception ex)
            {
                Skyray.Controls.SkyrayMsgBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// HorizontalScroll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void scrollBarHorizontal_Scroll(object sender, ScrollEventArgs e)
        {
            if (!_IsFireHScrollEvent) return;
            var offsetX = e.NewValue - e.OldValue;
            var newMin = e.NewValue == 0 ? 0 : base.MasterPane.PaneList[0].XAxis.Scale.Min + offsetX;
            var newMax = e.NewValue == MaxValue ? IXMaxChannel : base.MasterPane.PaneList[0].XAxis.Scale.Max + offsetX;
            base.MasterPane.PaneList[0].XAxis.Scale.Max = newMax;
            base.MasterPane.PaneList[0].XAxis.Scale.Min = newMin;
            OldXMax = newMax;
            OldXMin = newMin;
            SetAxisXLabel();
            this.AxisChange();
            this.Refresh();
            horizontalOldValue = e.NewValue;
        }

        /// <summary>
        /// VerticalScroll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void scrollBarVertical_Scroll(object sender, ScrollEventArgs e)
        {
            Vertical_Scroll(e.NewValue);
        }

        private void Vertical_Scroll(int newValue)
        {
            var value = newValue;
            if (value == 0)
            {
                base.MasterPane.PaneList[0].YAxis.Scale.Max = 20;
            }
            else if (value == (this.scrollBarVertical.Maximum - this.scrollBarVertical.LargeChange + 1))
            {
                base.MasterPane.PaneList[0].YAxis.Scale.Max = iYMaxChannel;
            }
            else if (value > verticalOldValue)
            {
                vNarrow(value);//缩小
            }
            else if (value < verticalOldValue)
            {
                vZoom(value);//放大
            }
            mouseX = -1;
            mouseY = -1;
            this.AxisChange();
            this.Refresh();
            verticalOldValue = scrollBarVertical.Value;
            OldYMax = this.MasterPane.PaneList[0].YAxis.Scale.Max;
        }
        private double OldYMax;
        private void tsmiLeftBorder_Click(object sender, EventArgs e)
        {
            if (OnLeftBorder != null) OnLeftBorder(null,
               new GraphEventArgs { Channel = this.iCurrentChannel });
        }

        private void tsmiRightBorder_Click(object sender, EventArgs e)
        {
            if (OnRightBorder != null) OnRightBorder(null,
                new GraphEventArgs { Channel = this.iCurrentChannel });
        }

        private void tsmiBaseLeft_Click(object sender, EventArgs e)
        {
            if (OnLeftBase != null) OnLeftBase(null,
               new GraphEventArgs { Channel = this.iCurrentChannel });
        }

        private void tsmiBaseRight_Click(object sender, EventArgs e)
        {
            if (OnRightBase != null) OnRightBase(null,
               new GraphEventArgs { Channel = this.iCurrentChannel });
        }

        private void tsmiReduction_Click(object sender, EventArgs e)
        {
            if (iYMaxChannel <= 0 ) return;
            _bManualScale = false;//yuzhao20150611:还原谱图时候取消手动增长模式
            this.MasterPane.PaneList[0].YAxis.Scale.Max = iYMaxChannel;
            this.MasterPane.PaneList[0].YAxis.Scale.Min = 0;
            this.scrollBarVertical.Enabled = true;
            this.scrollBarHorizontal.Enabled = true;
            this.scrollBarVertical.Value = this.scrollBarVertical.Maximum - scrollBarVertical.LargeChange + 1;
            oldYcoeff = 1;
            OldYMax = this.MasterPane.PaneList[0].YAxis.Scale.Max;
            mouseX = -1;
            mouseY = -1;
           
            this.AxisChange();
        }

        public void Reduction()
        {
            tsmiReduction_Click(null, null);
        }
        public void Reduction(bool bChangeX, bool bChangeY)
        {
            if (iYMaxChannel < 0 || iXMaxChannel <= 0 || this.MasterPane.PaneList[0].YAxis.Scale.Max < iYMaxChannel * 2) return;
            _bManualScale = false;           //yuzhao20150611:手动调节谱图增长
            if (bChangeY)
            {
                this.MasterPane.PaneList[0].YAxis.Scale.Max = iYMaxChannel < 100 ? 100 : iXMaxChannel * 1.2;
                this.MasterPane.PaneList[0].YAxis.Scale.Min = 0;
                this.MasterPane.PaneList[0].Y2Axis.Scale.Max = iYMaxChannel < 100 ? 100 : iXMaxChannel * 1.2;
                this.MasterPane.PaneList[0].Y2Axis.Scale.Min = 0;
            }
            if (bChangeX)
            {
                this.MasterPane.PaneList[0].XAxis.Scale.Max = iXMaxChannel;
                this.MasterPane.PaneList[0].XAxis.Scale.Min = 0;
                this.MasterPane.PaneList[0].X2Axis.Scale.Max = iXMaxChannel;
                this.MasterPane.PaneList[0].X2Axis.Scale.Min = 0;
            }
            this.scrollBarVertical.Enabled = true;
            this.scrollBarHorizontal.Enabled = true;
            this.scrollBarVertical.Value = this.scrollBarVertical.Maximum - scrollBarVertical.LargeChange + 1;
            this.scrollBarHorizontal.Value = 0;
            this.scrollBarHorizontal.LargeChange = iXMaxChannel;
            RefreshHScrollBarValue();
            OldYMax = this.MasterPane.PaneList[0].YAxis.Scale.Max;
            mouseX = -1;
            mouseY = -1;
            SetAxisXLabel();
            this.AxisChange();
        }
        private void tsmiBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = this.GraphPane.Chart.Fill.Color;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                this.GraphPane.Chart.Fill = new Fill(cd.Color);
                //this.BackColor = cd.Color;
            }
            mouseX = -1;
            mouseY = -1;
        }

      

      

        private void tsmiDelInterestArea_Click(object sender, EventArgs e)
        {
            interestAreaList.Remove(currentInterestArea);
           
            currentInterestArea = null;
            SetSpecData(_workCurve, this._VirtualSpecList, _spec, _changescale, _defaultcolor, _growstyle);
        }

        #endregion

        #region methods

        /// <summary>
        /// ChangeScroll
        /// </summary>
        private void ChangeScroll()
        {
            this.scrollBarVertical.Location = new System.Drawing.Point(Width - 19, 0);
            this.scrollBarVertical.Size = new System.Drawing.Size(19, Height);
            this.scrollBarHorizontal.Location = new System.Drawing.Point(0, Height - 19);
            this.scrollBarHorizontal.Size = new System.Drawing.Size(Width - 19, 19);
            this.Refresh();
        }

        /// <summary>
        /// SetMoveDataLine
        /// </summary>
        private void SetMoveDataLine()
        {
            this.scrollBarVertical.Enabled = true;
            this.scrollBarHorizontal.Enabled = true;
            this.scrollBarVertical.Value = this.scrollBarVertical.Maximum - scrollBarVertical.LargeChange + 1;
            this.scrollBarHorizontal.Value = 0;
            this.scrollBarHorizontal.LargeChange = this.scrollBarHorizontal.Maximum + 1;
        }

        /// <summary>
        /// paint method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (MultiGraph) return;
            if (bShowElement && AtomNamesDic.Count > 0 && AtomLinesDic.Count > 0)
            {
                this.drawElementLine(e.Graphics);
            }
            if (IsShowPeakFlagAuto)
            {
                this.drawElementPeakFlag(e.Graphics);
            }
            if (CalculationPoints != null)
            { DrawCurve(e.Graphics); }
        }

        /// <summary>
        /// DrawCurve
        /// </summary>
        /// <param name="g"></param>
        private void DrawCurve(Graphics g)
        {
            try
            {
                if (CalculationPoints == null || CalculationPoints.Length == 0)
                { return; }
                float scaleFactor = MasterPane.PaneList[0].CalcScaleFactor();
                base.MasterPane.PaneList[0].XAxis.Scale.FontSpec.Size = 12f / scaleFactor;
                base.MasterPane.PaneList[0].YAxis.Scale.FontSpec.Size = 12f / scaleFactor;
                base.MasterPane.PaneList[0].YAxis.Title.FontSpec.Size = 12f / scaleFactor;
                PointF[] points = new PointF[CalculationPoints.Length];
                if (_IsMainElement)
                {
                    this.GraphPane.XAxis.Scale.Min = 0;
                    this.GraphPane.XAxis.Scale.Max = 2;
                    this.GraphPane.YAxis.Scale.Max = 2;
                    this.GraphPane.YAxis.Scale.Min = 0;
                    return;
                }
                else
                {
                    this.GraphPane.YAxis.Scale.Max = ScalePoints.OrderByDescending(data => data.Y).ToList<PointF>()[0].Y * 1.3;
                    this.GraphPane.YAxis.Scale.Min = ScalePoints.OrderBy(data => data.Y).ToList<PointF>()[0].Y * 0.9;
                    this.GraphPane.XAxis.Scale.Max = ScalePoints.OrderByDescending(data => data.X).ToList<PointF>()[0].X * 1.3;
                    this.GraphPane.XAxis.Scale.Min = ScalePoints.OrderBy(data => data.X).ToList<PointF>()[0].X * 0.9;
                    if (this.GraphPane.YAxis.Scale.Max <= 0) this.GraphPane.YAxis.Scale.Max = 1;
                    if (this.GraphPane.XAxis.Scale.Max <= 0) this.GraphPane.XAxis.Scale.Max = 1;
                }
                this.GraphPane.XAxis.Title.IsVisible = true;
                for (int i = 0; i < CalculationPoints.Length; i++)
                {
                    float xCoordy = this.MasterPane.PaneList[0].XAxis.Scale.Transform(CalculationPoints[i].X);
                    float yCoordy = this.MasterPane.PaneList[0].YAxis.Scale.Transform(CalculationPoints[i].Y);
                    points[i] = new PointF(xCoordy, yCoordy);
                }
                if (CalculationWay == CalculationWay.Insert)
                {
                    //this.GraphPane.YAxis.Scale.Min = 0;
                }
                else if (CalculationWay == CalculationWay.Linear)
                { g.DrawLine(Pens.Red, points[0], points[1]); }
                else if (CalculationWay == CalculationWay.ContentContect)
                { g.DrawLine(Pens.Red, points[0], points[1]); }
                else if (CalculationWay == CalculationWay.IntensityCorrect)
                { g.DrawLine(Pens.Red, points[0], points[1]); }
                else if (CalculationWay == CalculationWay.Conic)
                {

                    coeff = new double[3];

                    if (CalSquareParam(CalculationPoints, coeff))
                    {
                        List<PointF> pTempPointF = new List<PointF>();
                        double dblx = 0;
                        double dbly = 0;
                        double mul = this.GraphPane.XAxis.Scale.Max / 1000;
                        do
                        {
                            dbly = coeff[0] * dblx * dblx + coeff[1] * dblx + coeff[2];

                            float xCoordy = this.MasterPane.PaneList[0].XAxis.Scale.Transform(dblx);
                            float yCoordy = this.MasterPane.PaneList[0].YAxis.Scale.Transform(dbly);
                            pTempPointF.Add(new PointF(xCoordy, yCoordy));

                            dblx += mul;
                        } while (dblx <= this.GraphPane.XAxis.Scale.Max);
                        if (pTempPointF.Count != 0)
                        {
                            try
                            {
                                g.DrawCurve(Pens.Red, pTempPointF.ToArray());
                            }
                            catch (System.Exception)
                            {

                            }
                        }
                    }

                }
                this.AxisChange();
            }
            catch
            { return; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Points"></param>
        /// <param name="coeff"></param>
        /// <returns></returns>
        private bool CalSquareParam(PointF[] Points, double[] coeff)
        {
            double[] abs = new double[Points.Length];
            double[] con = new double[Points.Length];
            for (int i = 0; i < Points.Length; i++)
            {
                abs[i] = Points[i].X;
                con[i] = Points[i].Y;
            }
            try
            {
                CalculateCurve(Points, 2, IZero, coeff);
            }
            catch
            {
                return false;
            }
            if (IZero)
            {
                double a = coeff[0];
                double b = coeff[1];
                coeff[0] = b;
                coeff[1] = a;
            }
            return true;
        }

      

        /// <summary>
        /// set element and line
        /// </summary>
        /// <param name="x">way</param>
        /// <param name="color">linrColor</param>
        /// <param name="height">lineHeight</param>
        /// <param name="name">ElementName</param>
        /// <param name="flag"></param>
        private void setElementLine(double energy, Color color, string name, string flag, List<ElementLine> elementLineList, int key)
        {
            double channel = 0;
            channel = DemarcateEnergyHelp.GetChannel(energy);
            double y = calcYStatPoint(channel, key);
            elementLineList.Add(new ElementLine(channel, y, color, defaultLineHeight, name, flag));
        }

        /// <summary>
        /// setElementPeakFlag
        /// </summary>
        /// <param name="x"></param>
        /// <param name="color"></param>
        /// <param name="name"></param>
        /// <param name="flag"></param>
        private void setElementPeakFlag(double x, Color color, string name, string flag)
        {
            int top = (int)this.MasterPane.PaneList[0].Chart.Rect.Top;
            int bottom = (int)this.MasterPane.PaneList[0].Chart.Rect.Bottom;
            elementPeakFlag.Add(new ElementLine(x, 0, color, bottom - top - 3 * fontSize, name, flag));
        }

        /// <summary>
        /// calcYStatPoint
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double calcYStatPoint(double x, int key)
        {
            double y = 0;
            List<SpecData> specDataList;
            if (SpecDataDic.TryGetValue(key, out specDataList))
            {
                foreach (SpecData sd in specDataList)
                {
                    if (sd.dX - x <= 1 && sd.dX - x >= 0)
                    {
                        y = sd.dY;
                        break;
                    }
                }
            }
            return y;
        }

        /// <summary>
        /// drawVirtualSpec
        /// </summary>
        /// <param name="graphPane"></param>
        private void drawVirtualSpec(GraphPane graphPane, SpecEntity spec)
        {
            foreach (SpecListEntity specList in _VirtualSpecList)
            {
                foreach (SpecEntity sp in specList.Specs)
                {
                    if (sp == null)
                        continue;
                    PointPairList ppl = new PointPairList();
                    LineItem li = graphPane.AddCurve("", ppl, Color.FromArgb(specList.VirtualColor), SymbolType.None);
                    if (!HorizontalSynchronization)
                    {
                        li.IsX2Axis = true;
                    }
                    if (!VerticalSynchronization)
                    {
                        li.IsY2Axis = true;
                    }
                    li.Tag = "Spec";
                    li.Line.IsAntiAlias = true;
                    li.Line.IsOptimizedDraw = true;
                    li.Line.IsSmooth = true;
                    li.Line.SmoothTension = 0.05f;
                    li.Line.StepType = StepType.ForwardStep;
                    int[] datas = sp.SpecDatas;
                    for (int i = 0; i < datas.Length; i++)
                    {
                        ppl.Add(i, datas[i]);
                    }
                }
            }
        }

        /// <summary>
        /// draw Spec
        /// </summary>
        private void drawSpec(GraphPane graphPane, SpecEntity spec)
        {
            graphPane.GraphObjList.RemoveAll(r => r.Tag.ToString() == "element");
            int count = graphPane.CurveList.Count;
            graphPane.CurveList.Clear();
            drawVirtualSpec(graphPane, spec);
            drawInterestArea(graphPane);
            if (Lssi != null && Lssi.Count > 0)
            {
                foreach (SpecSplitInfo ssi in Lssi)
                {
                    PointPairList ppl = new PointPairList();
                    LineItem li = graphPane.AddCurve("", ppl, ssi.Color, SymbolType.None);
                    li.Line.IsAntiAlias = true;
                    li.Line.IsOptimizedDraw = true;
                    li.Line.IsSmooth = true;
                    li.Line.SmoothTension = 0.05f;
                    li.Line.StepType = StepType.ForwardStep;

                    li.Line.Fill = new Fill(ssi.Color);
                    li.Tag = "Spec";
                    List<SpecData> specDataList;
                    if (SpecDataDic.TryGetValue(CurrentSpecPanel - 1, out specDataList))
                    {
                        for (int i = ssi.X1; i <= ssi.X2; i++)
                        {
                            ppl.Add(specDataList[i].dX, specDataList[i].dY);
                        }
                    }
                }
                SetAxisXLabel();//设置坐标轴标签
            }
            else
            {
                PointPairList ppl = new PointPairList();
                LineItem li = graphPane.AddCurve("", ppl, Colors[0], SymbolType.None);
                li.Line.IsAntiAlias = true;
                li.Line.IsOptimizedDraw = true;
                li.Line.IsSmooth = true;
                li.Line.SmoothTension = 0.05f;
                li.Line.StepType = StepType.ForwardStep;
                li.Line.Fill = new Fill(Colors[0]);
                li.Tag = "Spec";
                List<SpecData> specDataList;
                if (SpecDataDic.TryGetValue(CurrentSpecPanel - 1, out specDataList))
                {
                    foreach (SpecData sd in specDataList)
                    {
                        ppl.Add(sd.dX, sd.dY);
                    }
                }
            }

            mouseX = -1;
            mouseY = -1;
        }

        private void drawInterestArea(GraphPane graphPane)
        {
            if (interestAreaList == null || interestAreaList.Count == 0)
                return;
            var info = interestAreaList.Find(interest => interest == currentInterestArea);
            if (info != null)
            {
                PointPairList pp = new PointPairList();
                LineItem line = graphPane.AddCurve("", pp, info.Color, SymbolType.None);
                line.Line.IsAntiAlias = true;
                line.Line.IsOptimizedDraw = true;
                line.Line.IsSmooth = true;
                line.Line.SmoothTension = 0.05f;
                line.Line.StepType = StepType.ForwardStep;
                line.Line.Fill = new Fill(info.Color, Color.Black, Color.White, 45);
                int xb = info.X1 >= info.X2 ? info.X1 : info.X2;
                int xa = info.X1 >= info.X2 ? info.X2 : info.X1;
                List<SpecData> specDataList;
                if (SpecDataDic.TryGetValue(0, out specDataList))
                {
                    for (int i = xa; i <= xb; i++)
                    {
                        pp.Add(specDataList[i].dX, specDataList[i].dY);
                    }
                }
            }
            foreach (SpecSplitInfo ssi in interestAreaList)
            {
                if (ssi == currentInterestArea) continue;
                PointPairList pp = new PointPairList();
                LineItem line = graphPane.AddCurve("", pp, ssi.Color, SymbolType.None);
                line.Line.IsAntiAlias = true;
                line.Line.IsOptimizedDraw = true;
                line.Line.IsSmooth = true;
                line.Line.SmoothTension = 0.05f;
                line.Line.StepType = StepType.ForwardStep;
                line.Line.Fill = new Fill(ssi.Color);
                int xb = ssi.X1 >= ssi.X2 ? ssi.X1 : ssi.X2;
                int xa = ssi.X1 >= ssi.X2 ? ssi.X2 : ssi.X1;
                List<SpecData> specDataList;
                if (SpecDataDic.TryGetValue(0, out specDataList))
                {
                    for (int i = xa; i <= xb; i++)
                    {
                        pp.Add(specDataList[i].dX, specDataList[i].dY);
                    }
                }
            }
        }

        /// <summary>
        /// draw element and line
        /// </summary>
        /// <param name="g"></param>
        private void drawElementLine(Graphics g)
        {
           
            for (int m = 0; m < this.MasterPane.PaneList.Count; m++)
            {
                List<ElementLine> elementLineList;
                if (this.MasterPane.PaneList.Count == 1)
                {
                    if (!ElementLineDic.TryGetValue(CurrentSpecPanel - 1, out elementLineList))
                    { return; }
                }
                else
                {
                    if (!ElementLineDic.TryGetValue(m, out elementLineList))
                    { return; }
                }

                float xCoordy = 0;
                float yCoordy = 0;
                RectangleF rect;
                RectangleF recf;
                for (int i = 0; i < elementLineList.Count; i++)
                {
                    xCoordy = this.MasterPane.PaneList[m].XAxis.Scale.Transform(elementLineList[i].XCoordy);
                    yCoordy = this.MasterPane.PaneList[m].YAxis.Scale.Transform(elementLineList[i].YCoordy);
                    elementLineList[i].fLineEndPoint = yCoordy - elementLineList[i].Height;
                    for (int j = 0; j < i; j++)
                    {
                        float fxCoordy = this.MasterPane.PaneList[m].XAxis.Scale.Transform(elementLineList[j].XCoordy);
                        if (Math.Pow(xCoordy - fxCoordy, 2) < Math.Pow(2.2 * fontSize, 2))
                        {
                            if (j % 5 != 0)
                                elementLineList[i].fLineEndPoint = elementLineList[i].fLineEndPoint - 3 * fontSize;
                        }
                    }
                    float left = this.MasterPane.PaneList[m].Chart.Rect.Left;
                    float right = this.MasterPane.PaneList[m].Chart.Rect.Right;
                    float top = this.MasterPane.PaneList[m].Chart.Rect.Top;
                    float bottom = this.MasterPane.PaneList[m].Chart.Rect.Bottom;
                    if (xCoordy >= right || xCoordy <= left)//左右界外
                    {
                        continue;
                    }
                    if (yCoordy >= bottom)//下界外
                    {
                        if (elementLineList[i].fLineEndPoint < bottom)//界线在线与元素之间
                        {
                            drawLine(g, new Pen(elementLineList[i].Color), xCoordy, bottom, xCoordy, elementLineList[i].fLineEndPoint, i);
                        }
                    }
                    else if (yCoordy <= top)//上界外
                    {
                        continue;
                    }
                    else
                    {
                        if (elementLineList[i].fLineEndPoint < top)
                        {
                            drawLine(g, new Pen(elementLineList[i].Color), xCoordy, yCoordy, xCoordy, top, i);
                            continue;
                        }
                        drawLine(g, new Pen(elementLineList[i].Color), xCoordy, yCoordy, xCoordy, elementLineList[i].fLineEndPoint, i);
                    }
                    rect = getRect(xCoordy - fontSize, elementLineList[i].fLineEndPoint - fontSize, 32, 21, i);
                    recf = getRect(xCoordy - fontSize, elementLineList[i].fLineEndPoint - 2.5f * fontSize, 32, 21, i);
                    using (SolidBrush brush = new SolidBrush(elementLineList[i].Color))
                    {
                        Font font = new Font("Arial", fontSize);
                        if (bottom > elementLineList[i].fLineEndPoint - fontSize && top < elementLineList[i].fLineEndPoint - fontSize)
                        {
                            g.DrawString(elementLineList[i].Flag, font, brush, rect);
                        }
                        if (bottom > (elementLineList[i].fLineEndPoint - 2.5f * fontSize) && top < (elementLineList[i].fLineEndPoint - 2.5f * fontSize))
                        {
                            g.DrawString(elementLineList[i].ElementName, font, brush, recf);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// drawLine
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="coordyFlag"></param>
        private void drawLine(Graphics g, Pen pen, float x, float y, float x2, float y2, int index)
        {
            g.DrawLine(pen, x, y, x2, y2);
        }

        /// <summary>
        /// getStringRect
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="coordyFlag"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private RectangleF getRect(float x, float y, float width, float height, int index)
        {
            RectangleF rect = new RectangleF();
            rect = new RectangleF(x, y, width, height);
            return rect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void drawElementPeakFlag(Graphics g)
        {
            foreach (var pl in this.MasterPane.PaneList)
            {
                RectangleF rect;
                RectangleF recf;
                float xCoordy = 0;
                int left = (int)pl.Chart.Rect.Left;
                int right = (int)pl.Chart.Rect.Right;
                int top = (int)pl.Chart.Rect.Top;
                int bottom = (int)pl.Chart.Rect.Bottom;
                for (int i = 0; i < elementPeakFlag.Count; i++)
                {
                    xCoordy = pl.XAxis.Scale.Transform(elementPeakFlag[i].XCoordy);
                    if (xCoordy >= right || xCoordy <= left)
                    {
                        continue;
                    }
                    g.DrawLine(new Pen(elementPeakFlag[i].Color), xCoordy, bottom, xCoordy, top + 2.5f * fontSize);
                    rect = new RectangleF(xCoordy - fontSize, top + 1.5f * fontSize, 32, 21);
                    recf = new RectangleF(xCoordy - fontSize, top, 32, 21);
                    using (SolidBrush brush = new SolidBrush(elementPeakFlag[i].Color))
                    {
                        Font font = new Font("Arial", fontSize);
                        g.DrawString(elementPeakFlag[i].Flag, font, brush, rect);

                        g.DrawString(elementPeakFlag[i].ElementName, font, brush, recf);
                    }
                }
            }
        }

        /// <summary>
        /// show ScrollBar
        /// </summary>
        private void scrollVisible(bool b)
        {
            this.scrollBarHorizontal.Visible = b;
            this.scrollBarVertical.Visible = b;
            //this.grpRadiation.Visible = b;
            if (base.MasterPane.PaneList.Count > 0)
            {
                if (!b)
                {
                    base.MasterPane.PaneList[0].Margin.Right = 9;
                    base.MasterPane.PaneList[0].Margin.Bottom = 9;
                }
                else
                {
                    base.MasterPane.PaneList[0].Margin.Right = 19;
                    base.MasterPane.PaneList[0].Margin.Bottom = 19;
                }
                this.Refresh();
            }
        }

        private void RefreshHScrollBarValue()
        {
            double xMax = this.MasterPane.PaneList[0].XAxis.Scale.Max;
            double xMin = this.MasterPane.PaneList[0].XAxis.Scale.Min;
            _IsFireHScrollEvent = false;
            int iLargechange = (int)(this.scrollBarHorizontal.Maximum + 1 - (xMin + iXMaxChannel - xMax));
            iLargechange = iLargechange < 0 ? 0 : iLargechange;
            this.scrollBarHorizontal.LargeChange = iLargechange;
            if (xMin >= 0) this.scrollBarHorizontal.Value = (int)xMin;
            _IsFireHScrollEvent = true;
            OldXMax = xMax;
            OldXMin = xMin;
        }

        /// <summary>
        /// 处理鼠标拖动的方法
        /// </summary>
        /// <param name="mousePt"></param>
        /// <returns></returns>
        private Point HandlePan(Point mousePt)
        {
            double num;
            double num2;
            double num3;
            double num4;
            double num6;
            double num7;
            double num8;
            double num9;
            base.MasterPane.PaneList[0].ReverseTransform((PointF)this.dragStartPt, out num, out num3, out num6, out num8);
            base.MasterPane.PaneList[0].ReverseTransform((PointF)mousePt, out num2, out num4, out num7, out num9);
            #region X轴
            if (num2 > num)//鼠标右移
            {
                double max = this.MasterPane.PaneList[0].XAxis.Scale.Max;
                if (base.MasterPane.PaneList[0].XAxis.Scale.Min > 0)//原点大于0
                {
                    this.PanScale(base.MasterPane.PaneList[0].XAxis, num, num2);
                    //横滚动条VALUE变小
                }
                else
                {
                    this.MasterPane.PaneList[0].XAxis.Scale.Min = 0;
                    this.MasterPane.PaneList[0].XAxis.Scale.Max = max;
                }
            }
            else//鼠标左移
            {
                if (base.MasterPane.PaneList[0].XAxis.Scale.Max < iXMaxChannel)//小于最大值
                {
                    this.PanScale(base.MasterPane.PaneList[0].XAxis, num, num2);
                    //横滚动条VALUE变大
                }
                else
                {
                    this.MasterPane.PaneList[0].XAxis.Scale.Max = iXMaxChannel;
                }
            }
            #endregion
            SetAxisXLabel();
            #region Y轴
            if (num7 > num6)//鼠标上移
            {
                //if (base.MasterPane.PaneList[0].YAxis.Scale.Min > 0 && (num7 - num6) < base.MasterPane.PaneList[0].YAxis.Scale.Min)//原点大于0
                //{
                //    this.PanScale(base.MasterPane.PaneList[0].YAxis, num6, num7);
                //}
                //else
                //{
                //    this.MasterPane.PaneList[0].YAxis.Scale.Min = 0;
                //}
                vZoom(ChangeSpeed * 200.0);
            }
            else//鼠标下移
            {
                //if (base.MasterPane.PaneList[0].YAxis.Scale.Max < IYMaxChannel)//小于最大值
                //{
                //    this.PanScale(base.MasterPane.PaneList[0].YAxis, num6, num7);
                //}
                //else
                //{
                //    this.MasterPane.PaneList[0].YAxis.Scale.Max = IYMaxChannel;
                //}
                vNarrow(ChangeSpeed * 200.0);
            }
            #endregion
            SetAxisXLabel();
            RefreshHScrollBarValue();
            //mouseX = -1;
            //mouseY = -1;
            this.Refresh();
            this.AxisChange();
            this.dragStartPt = mousePt;
            return mousePt;

        }

        #region 缩  放

        #region X轴

        /// <summary>
        /// hNarrow
        /// </summary>
        /// <param name="x"></param>
        private void hNarrow(double x)
        {
            double min = MasterPane.PaneList[0].XAxis.Scale.Min;
            double max = MasterPane.PaneList[0].XAxis.Scale.Max;
            double current = x;
            double maxSpeed = ChangeSpeed * (MasterPane.PaneList[0].XAxis.Scale.Max - current) / iXMaxChannel;
            double minSpeed = ChangeSpeed * (current - MasterPane.PaneList[0].XAxis.Scale.Min) / iXMaxChannel;
            if (max < iXMaxChannel && iXMaxChannel >= (max + iXMaxChannel * maxSpeed))
            {
                if (min == 0 && iXMaxChannel >= (max + iXMaxChannel * ChangeSpeed))
                {
                    this.MasterPane.PaneList[0].XAxis.Scale.Max += iXMaxChannel * ChangeSpeed;
                }
                else if (min == 0 && iXMaxChannel < (max + iXMaxChannel * ChangeSpeed))
                {
                    this.MasterPane.PaneList[0].XAxis.Scale.Max = iXMaxChannel;
                }
                else
                {
                    this.MasterPane.PaneList[0].XAxis.Scale.Max += iXMaxChannel * maxSpeed;
                }
            }
            else if (iXMaxChannel < (max + iXMaxChannel * maxSpeed))
            {
                this.MasterPane.PaneList[0].XAxis.Scale.Max = iXMaxChannel;
            }
            if (min > 0 && min >= iXMaxChannel * minSpeed)
            {
                if (max == iXMaxChannel && min > iXMaxChannel * ChangeSpeed)
                {
                    this.MasterPane.PaneList[0].XAxis.Scale.Min -= iXMaxChannel * ChangeSpeed;
                }
                else if (max == iXMaxChannel && min <= iXMaxChannel * ChangeSpeed)
                {
                    this.MasterPane.PaneList[0].XAxis.Scale.Min = 0;
                }
                else
                {
                    this.MasterPane.PaneList[0].XAxis.Scale.Min -= iXMaxChannel * minSpeed;
                }
            }
            else if (min < iXMaxChannel * minSpeed)
            {
                this.MasterPane.PaneList[0].XAxis.Scale.Min = 0;
            }
            //if (!HorizontalSynchronization)
            {
                double min2 = MasterPane.PaneList[0].X2Axis.Scale.Min;
                double max2 = MasterPane.PaneList[0].X2Axis.Scale.Max;
                double step2 = MasterPane.PaneList[0].X2Axis.Scale.Max - MasterPane.PaneList[0].X2Axis.Scale.Min;
                double maxSpeed2 = ChangeSpeed * (MasterPane.PaneList[0].X2Axis.Scale.Max - current) / iXMaxChannel;
                double minSpeed2 = ChangeSpeed * (current - MasterPane.PaneList[0].X2Axis.Scale.Min) / iXMaxChannel;
                if (max2 < iXMaxChannel && iXMaxChannel >= (max2 + iXMaxChannel * maxSpeed))
                {
                    if (min2 == 0 && iXMaxChannel >= (max2 + iXMaxChannel * ChangeSpeed))
                    {
                        this.MasterPane.PaneList[0].X2Axis.Scale.Max += iXMaxChannel * ChangeSpeed;
                    }
                    else if (min2 == 0 && iXMaxChannel < (max2 + iXMaxChannel * ChangeSpeed))
                    {
                        this.MasterPane.PaneList[0].X2Axis.Scale.Max = iXMaxChannel;
                    }
                    else
                    {
                        this.MasterPane.PaneList[0].X2Axis.Scale.Max += iXMaxChannel * maxSpeed2;
                    }
                }
                else if (iXMaxChannel < (max2 + iXMaxChannel * maxSpeed2))
                {
                    this.MasterPane.PaneList[0].X2Axis.Scale.Max = iXMaxChannel;
                }
                if (min2 > 0 && min2 >= iXMaxChannel * minSpeed2)
                {
                    if (max2 == iXMaxChannel && min2 > iXMaxChannel * ChangeSpeed)
                    {
                        this.MasterPane.PaneList[0].X2Axis.Scale.Min -= iXMaxChannel * ChangeSpeed;
                    }
                    else if (max2 == iXMaxChannel && min2 <= iXMaxChannel * ChangeSpeed)
                    {
                        this.MasterPane.PaneList[0].X2Axis.Scale.Min = 0;
                    }
                    else
                    {
                        this.MasterPane.PaneList[0].X2Axis.Scale.Min -= iXMaxChannel * minSpeed2;
                    }
                }
                else if (min2 < iXMaxChannel * minSpeed2)
                {
                    this.MasterPane.PaneList[0].X2Axis.Scale.Min = 0;
                }
            }
            //if (ChartHChangedEvent != null)
            //    ChartHChangedEvent(this, new HorizontalEventArgs()
            //    {
            //        XMax = this.MasterPane.PaneList[0].XAxis.Scale.Max,
            //        XMin = this.MasterPane.PaneList[0].XAxis.Scale.Min
            //    });
            RefreshHScrollBarValue();
            SetAxisXLabel();
            mouseX = -1;
            mouseY = -1;
            this.AxisChange();
            this.Refresh();
        }

        /// <summary>
        /// hZoom
        /// </summary>
        /// <param name="x"></param>
        private void hZoom(double x)
        {
            double current = x;
            double step = MasterPane.PaneList[0].XAxis.Scale.Max - MasterPane.PaneList[0].XAxis.Scale.Min;
            //if (step <= iXMaxChannel * ChangeSpeed) return;
            if (step <= iXMaxChannel * 0.01) return;
            double maxSpeed = ChangeSpeed * (MasterPane.PaneList[0].XAxis.Scale.Max - current) / iXMaxChannel;
            double minSpeed = ChangeSpeed * (current - MasterPane.PaneList[0].XAxis.Scale.Min) / iXMaxChannel;
            this.MasterPane.PaneList[0].XAxis.Scale.Max -= iXMaxChannel * maxSpeed;
            this.MasterPane.PaneList[0].XAxis.Scale.Min += iXMaxChannel * minSpeed;
            //if (!HorizontalSynchronization)
            {
                double step2 = MasterPane.PaneList[0].X2Axis.Scale.Max - MasterPane.PaneList[0].X2Axis.Scale.Min;
                //if (step2 <= iXMaxChannel * ChangeSpeed) return;
                if (step2 <= iXMaxChannel * 0.01) return;
                double maxSpeed2 = ChangeSpeed * (MasterPane.PaneList[0].X2Axis.Scale.Max - current) / iXMaxChannel;
                double minSpeed2 = ChangeSpeed * (current - MasterPane.PaneList[0].X2Axis.Scale.Min) / iXMaxChannel;
                this.MasterPane.PaneList[0].X2Axis.Scale.Max -= iXMaxChannel * maxSpeed2;
                this.MasterPane.PaneList[0].X2Axis.Scale.Min += iXMaxChannel * minSpeed2;
            }
            //if (ChartHChangedEvent != null)
            //    ChartHChangedEvent(this, new HorizontalEventArgs() { XMax = this.MasterPane.PaneList[0].XAxis.Scale.Max,
            //                                                         XMin = this.MasterPane.PaneList[0].XAxis.Scale.Min});
            RefreshHScrollBarValue();
            SetAxisXLabel();
            mouseX = -1;
            mouseY = -1;
            this.AxisChange();
            this.Refresh();
        }

        #endregion

        #region Y轴

        public void vZoom(double coee)
        {
            if (this.scrollBarVertical.Value > 0)
            {
                this.scrollBarVertical.Value = (this.scrollBarVertical.Value - (int)coee) >= this.scrollBarVertical.Minimum ? (this.scrollBarVertical.Value - (int)coee) : this.scrollBarVertical.Minimum;
                Vertical_Scroll(scrollBarVertical.Value);
            }
        }

        /// <summary>
        /// 垂直放大
        /// </summary>
        public void vZoom()
        {
            if (this.scrollBarVertical.Value > 0)
            {
                this.scrollBarVertical.Value--;
                Vertical_Scroll(scrollBarVertical.Value);
            }
        }

        /// <summary>
        /// 垂直放大(滚动条)
        /// </summary>
        /// <param name="Multiples">zoomMultiples</param>
        public void vZoom(int Multiples)
        {
            int max = (IYMaxChannel - 20) * Multiples / (this.scrollBarVertical.Maximum - scrollBarVertical.LargeChange + 1);
            //if (this.MasterPane.PaneList[0].YAxis.Scale.Max <= 20 + (IYMaxChannel - 20) / (this.scrollBarVertical.Maximum - scrollBarVertical.LargeChange + 1))
            //{
            //    max = 20;
            //}
            if (max < 1) max = 1;
            this.MasterPane.PaneList[0].YAxis.Scale.Max = max;
        }


        /// <summary>
        /// 垂直缩小(滚动条)
        /// </summary>
        public void vNarrow(int Multiples)
        {
            var max = (IYMaxChannel - 20) * Multiples / (this.scrollBarVertical.Maximum - scrollBarVertical.LargeChange + 1);
            if (max <= 20)
            {
                max = 20;
            }
            this.MasterPane.PaneList[0].YAxis.Scale.Max = max;
        }


        private void Vertical_Scroll(int newValue, int Ycoeff)
        {
            var value = newValue;
            if (value == 0)
            {
                base.MasterPane.PaneList[0].YAxis.Scale.Max = 20;
            }
            else if (value == (this.scrollBarVertical.Maximum - this.scrollBarVertical.LargeChange + 1))
            {
                base.MasterPane.PaneList[0].YAxis.Scale.Max = iYMaxChannel;
            }
            else if (Ycoeff == 1)
            {
                this.scrollBarVertical.Value = this.scrollBarVertical.Maximum - scrollBarVertical.LargeChange + 1;
                base.MasterPane.PaneList[0].YAxis.Scale.Max = iYMaxChannel;
            }
            else
            {
                this.MasterPane.PaneList[0].YAxis.Scale.Max = IYMaxChannel / Ycoeff;
            }
            mouseX = -1;
            mouseY = -1;
            this.AxisChange();
            this.Refresh();
            verticalOldValue = scrollBarVertical.Value;
            OldYMax = this.MasterPane.PaneList[0].YAxis.Scale.Max;
        }




        /// <summary>
        /// 上升谱
        /// </summary>
        public void UpZoom()
        {
            if (this.scrollBarVertical.Value > 0 && oldYcoeff < 16)
            {

                this.scrollBarVertical.Value = this.scrollBarVertical.Value * oldYcoeff / ++oldYcoeff;
                _bManualScale = true;//yuzhao20150611:手动调节谱图增长
                Vertical_Scroll(scrollBarVertical.Value, oldYcoeff);

            }
        }


        /// <summary>
        /// 下降谱
        /// </summary>
        public void DownZoom()
        {
            if (this.scrollBarVertical.Value > 0 && oldYcoeff > 1)
            {
                this.scrollBarVertical.Value = this.scrollBarVertical.Value * oldYcoeff / --oldYcoeff;
                _bManualScale = true; //yuzhao20150611:手动调节谱图增长
                Vertical_Scroll(scrollBarVertical.Value, oldYcoeff);
            }
            //else if (oldYcoeff <= 1)
            //{
            //    Reduction();
            //}
        }


        /// <summary>
        /// vNarrow
        /// </summary>
        public void vNarrow()
        {
            if (this.scrollBarVertical.Value < (this.scrollBarVertical.Maximum - this.scrollBarVertical.LargeChange + 1))
            {
                this.scrollBarVertical.Value++;
                Vertical_Scroll(scrollBarVertical.Value);
            }
        }

        public void vNarrow(double coee)
        {
            if (this.scrollBarVertical.Value < (this.scrollBarVertical.Maximum - this.scrollBarVertical.LargeChange + 1))
            {
                this.scrollBarVertical.Value = (this.scrollBarVertical.Value + (int)coee) <= this.scrollBarVertical.Maximum ? (this.scrollBarVertical.Value + (int)coee) : this.scrollBarVertical.Maximum;
                Vertical_Scroll(scrollBarVertical.Value);
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// SetSpecData
        /// </summary>
        /// <param name="list"></param>                 
        private void SetSpecData(SpecEntity spec, List<SpecListEntity> virtualSpec, int color, bool ChangeScale, int growstyle)
        {
            this.Positions = new float[2] { 0f, 1f };
            this.Colors = new Color[2] { Color.FromArgb(color), Color.FromArgb(color) };
            this._VirtualSpecList = virtualSpec;
            this.EnableMoveLine = true;
            this.IsUseScroll = true;
            this.EnableWheel = true;
            this.scrollBarVertical.Enabled = true;
            this.scrollBarHorizontal.Enabled = true;
            drawSpec(this.MasterPane.PaneList[0], spec);
            SetScaleMax(spec, virtualSpec, ChangeScale, growstyle);
            this.AxisChange();
            this.Refresh();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="lssi"></param>
        ///// <param name="virtualSpec"></param>
        ///// <param name="color"></param>
        ///// <param name="ChangeScale"></param>
        //private void SetSpecData(SpecEntity spec, List<SpecSplitInfo> lssi, List<SpecListEntity> virtualSpec, int color, bool ChangeScale)
        //{
        //    SetSpecData(spec, virtualSpec, color, ChangeScale);
        //}

        /// <summary>
        /// 坐标自动变化
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ChangeScale"></param>
        private void SetScaleMax(SpecEntity spec, List<SpecListEntity> virtualSpecList, bool ChangeScale, int growStyle)
        {
            List<SpecData> list;
            SpecDataDic.TryGetValue(CurrentSpecPanel - 1, out list);
            double dx = 0;
            double dy = 0;
            List<SpecData> ld = new List<SpecData>();
            if (list != null && list.Count > 0)
            {
                dx = list.OrderByDescending(data => data.dX).ToList<SpecData>()[0].dX;
                dy = list.OrderByDescending(data => data.dY).ToList<SpecData>()[0].dY;//获取最大记数
            }

            foreach (var specList in virtualSpecList)
            {
                foreach (SpecEntity sp in specList.Specs)
                {
                    if (sp == null) continue;
                    List<SpecData> listSpecData = translateSpecToSpecData(sp);
                    SpecData maxY = listSpecData.OrderByDescending(l => l.dY).ToList()[0];
                    ld.Add(maxY);
                }
            }
            double yVirtualMax = ld.Count > 0 ? ld.OrderByDescending(o => o.dY).ToList()[0].dY : 0;
            double yMax = dy >= yVirtualMax ? dy * 1.2 : yVirtualMax * 1.2;
            yMax = yMax > int.MaxValue ? int.MaxValue : yMax;
            if (dx > 0)
                iXMaxChannel = int.Parse(dx.ToString());
            if (yMax > 0)
                //iYMaxChannel = int.Parse(Math.Floor(yMax * 1.2).ToString());
                iYMaxChannel = yMax * 1.2 > int.MaxValue ? int.MaxValue : (int)(yMax * 1.2);
            //if (ChangeScale)      //yuzhao_20150604:增加手动变量控制谱图变化模式
            if (ChangeScale && !_bManualScale)                      //yuzhao_20150604:增加手动变量控制谱图变化模式
            {
                //yMax = dy * 1.2;
                if (IsEDXRF)
                {
                    yMax = dy;
                    if (growStyle == 0)
                    {
                        this.MasterPane.PaneList[0].YAxis.Scale.Max = yMax * 1.2 > 100 ? yMax * 1.2 : 100;
                        this.MasterPane.PaneList[0].Y2Axis.Scale.Max = yMax * 1.2 > 100 ? yMax * 1.2 : 100;
                    }
                    else if (growStyle == 1)
                    {
                        if (yMax > (this.MasterPane.PaneList[0].YAxis.Scale.Max * 0.99) && yRemerber > 0)
                        {
                            this.MasterPane.PaneList[0].YAxis.Scale.Max = yMax / 0.6;
                        }
                        else if (yMax < this.MasterPane.PaneList[0].YAxis.Scale.Max * 0.24 || yRemerber == 0)
                        {
                            this.MasterPane.PaneList[0].YAxis.Scale.Max = (5 * yMax) > 100 ? 5 * yMax : 100;
                        }
                        if (yMax > (this.MasterPane.PaneList[0].Y2Axis.Scale.Max * 0.99) && yRemerber > 0)
                        {
                            this.MasterPane.PaneList[0].Y2Axis.Scale.Max = yMax / 0.6;
                        }
                        else if (yMax < this.MasterPane.PaneList[0].Y2Axis.Scale.Max * 0.24 || yRemerber == 0)
                        {
                            this.MasterPane.PaneList[0].Y2Axis.Scale.Max = (5 * yMax) > 100 ? 5 * yMax : 100;
                        }
                    }

                }
                else
                {
                    yMax = dy * 1.2;
                    if (yMax > (this.MasterPane.PaneList[0].YAxis.Scale.Max / 1.2) && yRemerber > 0)
                    {
                        this.MasterPane.PaneList[0].YAxis.Scale.Max = this.MasterPane.PaneList[0].YAxis.Scale.Max * (yMax / yRemerber);
                    }
                    if (yMax > (this.MasterPane.PaneList[0].Y2Axis.Scale.Max / 1.2) && yRemerber > 0)
                    {
                        this.MasterPane.PaneList[0].Y2Axis.Scale.Max = this.MasterPane.PaneList[0].Y2Axis.Scale.Max * (yMax / yRemerber);
                    }
                }
                //if (yMax > (this.MasterPane.PaneList[0].YAxis.Scale.Max / 1.2) && yRemerber > 0)
                //{
                //    this.MasterPane.PaneList[0].YAxis.Scale.Max = this.MasterPane.PaneList[0].YAxis.Scale.Max * (yMax / yRemerber);
                //}
                //else if (yMax < this.MasterPane.PaneList[0].YAxis.Scale.Max * 0.24 || yRemerber==0)
                //{
                //    this.MasterPane.PaneList[0].YAxis.Scale.Max = (5 * yMax) > 100 ? 5 * yMax : 100;
                //}
                //if (yMax > (this.MasterPane.PaneList[0].Y2Axis.Scale.Max / 1.2) && yRemerber > 0)
                //{
                //    this.MasterPane.PaneList[0].Y2Axis.Scale.Max = this.MasterPane.PaneList[0].Y2Axis.Scale.Max * (yMax / yRemerber);
                //}
                //else if (yMax < this.MasterPane.PaneList[0].Y2Axis.Scale.Max * 0.24 || yRemerber == 0)
                //{
                //    this.MasterPane.PaneList[0].Y2Axis.Scale.Max = (5 * yMax) > 100 ? 5 * yMax : 100;
                //}
                //yRemerber = int.Parse(Math.Floor(yMax).ToString());
                yRemerber = (int)yMax;
                if (IYMaxChannel != 20)
                {
                    try
                    {
                        int VScroll = (int.Parse(Math.Floor(this.MasterPane.PaneList[0].YAxis.Scale.Max).ToString()) * this.scrollBarVertical.Maximum) / (IYMaxChannel - 20) + 1;
                        if (VScroll >= 0 && VScroll <= 991)
                        {
                            this.scrollBarVertical.Value = VScroll;
                        }
                    }
                    catch
                    { }
                }
            }
            else
            {
                if (this.MasterPane.PaneList[0].YAxis.Scale.Max == 1000 && yMax > 0)
                {
                    this.MasterPane.PaneList[0].YAxis.Scale.Max = yMax * 1.2;
                }
                if (this.MasterPane.PaneList[0].Y2Axis.Scale.Max == 1000 && yMax > 0)
                {
                    this.MasterPane.PaneList[0].Y2Axis.Scale.Max = yMax * 1.2;
                }
                if (IYMaxChannel != 20)
                {
                    try
                    {
                        if (OldYMax != 0)
                        {
                            this.MasterPane.PaneList[0].YAxis.Scale.Max = OldYMax;
                        }
                        int VScroll = (int.Parse(Math.Floor(this.MasterPane.PaneList[0].YAxis.Scale.Max).ToString()) * this.scrollBarVertical.Maximum) / (IYMaxChannel - 20) + 1;
                        if (VScroll >= 0 && VScroll <= 991)
                        {
                            this.scrollBarVertical.Value = VScroll;
                        }
                    }
                    catch
                    { }
                }
                this.MasterPane.PaneList[0].XAxis.Scale.Min = OldXMin;
                if (OldXMax != 0)
                {
                    this.MasterPane.PaneList[0].XAxis.Scale.Max = OldXMax;
                    SetAxisXLabel();
                }
            }
            this.MasterPane.PaneList[0].YAxis.Scale.Min = 0;
        }


        public void SetYScaleMax(int ymax)
        {
            if (ymax > 0)
                iYMaxChannel = ymax * 1.2 > int.MaxValue ? int.MaxValue : (int)(ymax * 1.2);
            this.MasterPane.PaneList[0].YAxis.Scale.Max = ymax * 1.2 > 100 ? ymax * 1.2 : 100;
        }



        /// <summary>
        /// 画图主入口
        /// </summary>
        /// <param name="WorkCurve">工作曲线</param>
        /// <param name="specList">实谱对象</param>
        /// <param name="specListVisual">对比谱对象</param>
        /// <param name="g"></param>
        /// <param name="currentSpec">当前正在扫的谱</param>
        public void MultiPanel(WorkCurve WorkCurve, SpecListEntity specListFill, List<SpecListEntity> specListVirtual, SpecEntity currentSpec, int defaultColor, Color backColor)
        {
            Graphics g = this.CreateGraphics();
            paneList = this.MasterPane.PaneList;
            this.MasterPane.PaneList.Clear();
            if (specListFill != null) //包含实谱
            {
                this.MasterPane.SetLayout(g, specListFill.Specs.Length, 1);
                for (int j = 0; j < specListFill.Specs.Length; j++)
                {
                    List<SpecData> listSpec = new List<SpecData>();
                    int[] specDataInt = specListFill.Specs[j].SpecDatas;
                    for (int m = 0; m < specDataInt.Length; m++)
                    {
                        SpecData specData = new SpecData(m, specDataInt[m]);
                        listSpec.Add(specData);
                    }
                    SpecDataDic.Remove(j);
                    SpecDataDic.Add(j, listSpec);
                    Rectangle Rectan = new Rectangle(0, 0, 0, 0);
                    GraphPane myPane = this.CreatePanel(WorkCurve, specListFill.Specs[j], Rectan, Color.FromArgb(specListFill.Color), true, false, specListFill, backColor, specListVirtual);
                }
                if (specListFill.Specs.Length > 1)
                {
                    this.IsUseScroll = false;
                    this.EnableWheel = false;
                }
                else
                {
                    this.EnableMoveLine = true;
                    this.IsUseScroll = true;
                    this.EnableWheel = true;
                    this.SetMoveDataLine();
                }
            }
            else
            {
                this.MasterPane.SetLayout(g, 1, 1);
                Rectangle Rectan = new Rectangle(0, 0, 0, 0);
                GraphPane myPane = this.CreatePanel(WorkCurve, currentSpec, Rectan, Color.FromArgb(defaultColor), true, true, specListFill, backColor, specListVirtual);
                this.EnableMoveLine = true;
                this.IsUseScroll = true;
                this.EnableWheel = true;
                this.SetMoveDataLine();
            }
            this.MasterPane.DoLayout(g);
            this.MasterPane.AxisChange(g);
            g.Dispose();
            foreach (var pane in this.MasterPane.PaneList)
            {
                float scaleFactor = pane.CalcScaleFactor();
                pane.XAxis.Scale.FontSpec.Size = 12f / scaleFactor;
                pane.YAxis.Scale.FontSpec.Size = 12f / scaleFactor;
                pane.YAxis.Title.FontSpec.Size = 12f / scaleFactor;
                TextObj obj = pane.GraphObjList.Find(l => l.Tag.ToString() == "InfoTitle") as TextObj;
            }
            if (specListFill != null && specListFill.Specs.Length == 1 && WorkCurve != null && WorkCurve.ElementList != null && WorkCurve.ElementList.Items.Count > 0)
            {
                int transColor = (specListFill == null || specListFill.Color == 0) ? defaultColor : specListFill.Color;
                this.Lssi = this.GetLssi(WorkCurve, transColor);
                this.SetAxisXLabel();
            }
            this.Refresh();
        }

        /// <summary>
        /// SetSpecData
        /// </summary>
        /// <param name="xrf"></param>
        /// <param name="list"></param>
        /// <param name="workCurve"></param>
        public void SetSpecData(WorkCurve workCurve, List<SpecListEntity> VirtualSpecList, SpecEntity spec, bool ChangeScale, int defaultColor, int growstyle)
        {
            _workCurve = workCurve;
            _spec = spec;
            _changescale = ChangeScale;
            _defaultcolor = defaultColor;
            _growstyle = growstyle;
            try
            {
                removePanel();
                if (workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count == 0)
                {
                    this.Lssi = null;
                    SetSpecData(spec, VirtualSpecList, defaultColor, ChangeScale, growstyle);
                }
                else
                {
                    List<SpecData> specDataList;
                    if (SpecDataDic.TryGetValue(0, out specDataList))
                    {
                        if (spec == null || spec.SpecData == null) return;
                    }
                    if (specDataList == null || specDataList.Count == 0)
                    {
                        specDataList = new List<SpecData>();
                        int[] specDataInt = spec.SpecDatas;
                        for (int i = 0; i < specDataInt.Length; i++)
                        {
                            SpecData specData = new SpecData(i, specDataInt[i]);
                            specDataList.Add(specData);
                        }
                        SpecDataDic.Remove(0);
                        SpecDataDic.Add(0, specDataList);
                    }
                    this.Lssi = GetLssi(workCurve, defaultColor);
                    SetSpecData(spec, VirtualSpecList, defaultColor, ChangeScale, growstyle);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private GraphPane createPanel(Rectangle rect, Color BackColor)
        {
            //Color.FromArgb(33, 105, 102)
            GraphPane myPane = new GraphPane(rect, "", "", "");
            myPane.IsFontsScaled = false;
            myPane.Title.IsVisible = false;
            myPane.YAxis.MajorTic.IsInside = false;
            myPane.YAxis.MinorTic.IsInside = false;
            myPane.XAxis.MajorTic.IsInside = false;
            myPane.XAxis.MinorTic.IsInside = false;
            if (BackColor != null)
            {
                myPane.Chart.Fill = new Fill(BackColor, BackColor, 45.0F);
            }
            else
            {
                myPane.Chart.Fill = new Fill(Color.Transparent, Color.Transparent, 45.0F);
            }
            myPane.GraphObjList.Add(this.text);
            this.MasterPane.Add(myPane);
            return myPane;
        }

        /// <summary>
        /// 创建单个Panel
        /// </summary>
        /// <param name="spec">实谱</param>
        /// <param name="visualSpec">对比谱</param>
        /// <param name="rect"></param>
        /// <param name="color">曲线颜色</param>
        /// <param name="isFill">是否填充</param>
        /// <returns></returns>
        public GraphPane CreatePanel(WorkCurve WorkCurveCurrent, SpecEntity spec, Rectangle rect, Color color, bool isFill, bool isAlone, SpecListEntity specList, Color BackColor, List<SpecListEntity> specListVirtual)
        {
            Dictionary<SpecEntity, int> dicVisual = new Dictionary<SpecEntity, int>();
            for (int i = 0; i < specListVirtual.Count; i++)
            {
                try
                {
                    foreach (var spe in specListVirtual[i].Specs)
                    {
                        dicVisual.Add(spe, specListVirtual[i].VirtualColor);
                    }
                }
                catch (Exception)
                { continue; }
            }
            GraphPane myPane = createPanel(rect, BackColor);
            if (dicVisual.Count != 0)
            {
                double MaxY = 0;
                foreach (var div in dicVisual)
                {
                    PointPairList ppl = new PointPairList();
                    int[] Points = div.Key.SpecDatas;
                    for (int i = 0; i < Points.Length; i++)
                    {
                        double x = i;
                        double y = double.Parse(Points[i].ToString());
                        ppl.Add(x, y);
                    }
                    LineItem VisualCurve = myPane.AddCurve("", ppl, Color.FromArgb(div.Value), SymbolType.None);
                    VisualCurve.Line.IsAntiAlias = true;
                    VisualCurve.Line.IsOptimizedDraw = true;
                    VisualCurve.Line.IsSmooth = true;
                    VisualCurve.Line.SmoothTension = 0.05f;
                    VisualCurve.Line.StepType = StepType.ForwardStep;
                    if (ppl.OrderByDescending(d => d.Y).ToList()[0].Y > MaxY)
                    {
                        MaxY = ppl.OrderByDescending(d => d.Y).ToList()[0].Y;
                    }
                    myPane.X2Axis.Scale.Max = myPane.XAxis.Scale.Max = ppl.OrderByDescending(data => data.X).ToList()[0].X;
                }
            }
            if (!isAlone)
            {
                PointPairList list = new PointPairList();
                int[] arrPoints = spec.SpecDatas;
                for (int i = 0; i < arrPoints.Length; i++)
                {
                    double x = i;
                    double y = double.Parse(arrPoints[i].ToString());
                    list.Add(x, y);
                }
                if (specList.Specs.Length == 1 && WorkCurveCurrent != null && WorkCurveCurrent.ElementList != null && WorkCurveCurrent.ElementList.Items.Count > 0)
                {
                    int transColor = (specList == null || specList.Color == 0) ? color.ToArgb() : specList.Color;
                    lssi = GetLssi(WorkCurveCurrent, transColor);
                    foreach (SpecSplitInfo ssi in lssi)
                    {
                        PointPairList ppl = new PointPairList();
                        LineItem li = myPane.AddCurve("", ppl, ssi.Color, SymbolType.None);
                        li.Line.IsAntiAlias = true;
                        li.Line.IsOptimizedDraw = true;
                        li.Line.IsSmooth = true;
                        li.Line.SmoothTension = 0.05f;
                        li.Line.StepType = StepType.ForwardStep;
                        FontSpec fontSpec = new FontSpec("", 8, ssi.Color, false, false, false);
                        fontSpec.Border.IsVisible = false;
                        li.Label.FontSpec = fontSpec;
                        li.Line.Fill = new Fill(ssi.Color);
                        for (int i = ssi.X1; i <= ssi.X2; i++)
                        {
                            ppl.Add(list[i].X, list[i].Y);
                        }
                    }
                    SetAxisXLabel();
                }
                else
                {
                    LineItem myCurve = myPane.AddCurve("", list, color, SymbolType.None);
                    myCurve.Line.IsAntiAlias = true;
                    myCurve.Line.IsOptimizedDraw = true;
                    myCurve.Line.IsSmooth = true;
                    myCurve.Line.SmoothTension = 0.05f;
                    myCurve.Line.StepType = StepType.ForwardStep;
                    if (isFill)
                    {
                        myCurve.Line.Fill = new Fill(color, color, 180f);
                    }
                }
                if (list.OrderByDescending(data => data.Y).ToList()[0].Y > myPane.YAxis.Scale.Max)
                {
                    //SetScaleMax(list, true);
                }
                myPane.X2Axis.Scale.Max = myPane.XAxis.Scale.Max = list.OrderByDescending(data => data.X).ToList()[0].X;
            }
            return myPane;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workCurve"></param>
        /// <param name="transColor"></param>
        /// <returns></returns>
        private List<SpecSplitInfo> GetLssi(WorkCurve workCurve, int transColor)
        {
            List<SpecSplitInfo> lssi = new List<SpecSplitInfo>();
            int specLength = (int)workCurve.Condition.Device.SpecLength == 0 ? 2048 : (int)workCurve.Condition.Device.SpecLength;
            var items = from c in workCurve.ElementList.Items orderby c.PeakLow select c;
            foreach (CurveElement ce in items)
            {
                SpecSplitInfo splitInfo = new SpecSplitInfo((ce.IsOxide) ? ce.Formula : ce.Caption, ce.PeakLow, ce.PeakHigh, Color.FromArgb(ce.Color));
                lssi.Add(splitInfo);
            }
            int count = lssi.Count - 1;
            if (lssi[0].X1 > 0)//从0到第一道
            {
                SpecSplitInfo splitInfo = new SpecSplitInfo("", 0, lssi[0].X1, Color.FromArgb(transColor));
                lssi.Add(splitInfo);
            }
            for (int i = 0; i < count; i++)
            {
                if (lssi[i + 1].X1 > lssi[i].X2)
                {
                    SpecSplitInfo splitInfo = new SpecSplitInfo("", lssi[i].X2, lssi[i + 1].X1, Color.FromArgb(transColor));
                    lssi.Add(splitInfo);
                }
            }
            if (lssi[count].X2 < specLength - 1)
            {
                SpecSplitInfo splitInfo = new SpecSplitInfo("", lssi[count].X2, specLength - 1, Color.FromArgb(transColor));
                lssi.Add(splitInfo);
            }
            return lssi;
        }

        /// <summary>
        /// removePanel
        /// </summary>
        public void removePanel()
        {
            Graphics g = this.CreateGraphics();
            int panelCount = this.MasterPane.PaneList.Count;
            if (panelCount > 1)
            {
                this.MasterPane.PaneList.RemoveRange(1, this.MasterPane.PaneList.Count - 1);
                this.MasterPane.SetLayout(g, 1, 1);
            }
        }

        public void ClearCurve()
        {
            foreach (var pane in this.MasterPane.PaneList)
            {
                pane.GraphObjList.RemoveAll(r => r.Tag == null);
                pane.CurveList.Clear();
            }
            this.Refresh();
            this.AxisChange();
        }

        /// <summary>
        /// translateSpecToSpecData
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        private List<SpecData> translateSpecToSpecData(SpecEntity spec)
        {
            int[] specDataInt = spec.SpecDatas;
            List<SpecData> listSpecData = new List<SpecData>();
            for (int i = 0; i < specDataInt.Length; i++)
            {
                SpecData specData = new SpecData(i, specDataInt[i]);
                listSpecData.Add(specData);
            }
            return listSpecData;
        }

       

        /// <summary>
        /// clearElementTable
        /// </summary>
        public void clearElementTable()
        {
            this.Refresh();
        }

        /// <summary>
        /// WriteInformation BeforeSpec
        /// </summary>
        /// <param name="Info"></param>
        public void WriteInformation(string Info)
        {
            //float scaleFactor = this.GraphPane.CalcScaleFactor();
            text.IsVisible = true;
            text.IsClippedToChartRect = true;
            text.Text = Info;
            text.FontSpec.Fill.IsVisible = false;
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.IsBold = true;
            //text.FontSpec.Size = 24f;
            text.FontSpec.Size = 24f * ((200 + this.GraphPane.Rect.Width) / Screen.PrimaryScreen.Bounds.Width);
            //text.FontSpec.IsItalic = true;
            text.FontSpec.StringAlignment = StringAlignment.Center;
            //text.Location = new Location(0.25, 0.15, CoordType.ChartFraction);
            Graphics g = this.CreateGraphics();
            SizeF size = g.MeasureString(text.Text, text.FontSpec.GetFont(this.GraphPane.CalcScaleFactor()));
            float d = size.Width / this.GraphPane.Rect.Width / 2;
            text.Location = new Location(0.5 - d, 0.18, CoordType.ChartFraction);
            text.ZOrder = ZOrder.A_InFront;

            mouseX = -1;
            mouseY = -1;
            this.Refresh();
        }

        /// <summary>
        /// StopWriteInformation
        /// </summary>
        public void ClearInformation()
        {
            text.Text = "";
            text.IsVisible = false;
            mouseX = -1;
            mouseY = -1;
            this.Refresh();
        }

        /// <summary>
        /// 显示单张谱
        /// </summary>
        /// <param name="xrf">谱图控件对象</param>
        /// <param name="spec">实谱</param>
        /// <param name="visualSpec">对比谱</param>
        /// <param name="g"></param>
        public void ShowAloneSpec(WorkCurve workCurve, SpecEntity spec, List<SpecListEntity> VirtualSpecList, bool ChangeScale, int defaultColor)
        {
            //List<SpecData> listSpecData = translateSpecToSpecData(spec);
            SetSpecData(workCurve, VirtualSpecList, spec, ChangeScale, defaultColor, _growstyle);
        }

        /// <summary>
        /// ProcessDialogKey
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            //if (_enableWheel && (specDataList.Count > 0 || (_VirtualSpecList != null && _VirtualSpecList.Count > 0)))
            if (_enableWheel)
            {
                switch (keyData)
                {
                    case Keys.Up:
                        this.vZoom();
                        break;
                    case Keys.Down:
                        this.vNarrow();
                        break;
                    case Keys.Left:
                        this.hNarrow(this.MasterPane.PaneList[0].XAxis.Scale.Max);
                        break;
                    case Keys.Right:
                        this.hZoom(this.MasterPane.PaneList[0].XAxis.Scale.Min);
                        break;
                    case Keys.Add:
                        this.UpZoom();
                        break;
                    case Keys.Subtract:
                        this.DownZoom();
                        break;
                    case Keys.Space:
                        tsmiReduction_Click(null, null);
                        this.Refresh();
                        break;
                    default: break;
                }
            }
            return true;
            //return base.ProcessDialogKey(keyData);
            //return base.ProcessDialogKey(keyData);
        }

        #endregion

        #region 拟合曲线

        public bool CalculateCurve(PointF[] Points, int dim, bool izero, double[] x)
        {
            double[] transX = new double[x.Length];
            if (izero)
            {
                double[,] A = new double[Points.Length, dim];
                double[] B = new double[Points.Length];
                int intValid = 0;
                for (int i = 0; i < Points.Length; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        A[intValid, j] = Math.Pow(Points[i].X, j + 1);
                    }
                    B[intValid] = Points[i].Y;
                    intValid++;
                }
                if (intValid < dim)
                {
                    return false;
                }
                MatrixFun.MatrixEquation(Points.Length, dim, A, B, transX);
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = transX[i];
                }
            }
            else
            {
                double[,] A = new double[Points.Length, dim + 1];
                double[] B = new double[Points.Length];
                int intValid = 0;
                for (int i = 0; i < Points.Length; i++)
                {
                    A[intValid, 0] = 1;
                    for (int j = 0; j < dim; j++)
                        A[intValid, j + 1] = Math.Pow(Points[i].X, j + 1);
                    B[intValid] = Points[i].Y;
                    intValid++;
                }
                if (intValid < dim + 1)
                {
                    return false;
                }
                MatrixFun.MatrixEquation(Points.Length, dim + 1, A, B, transX);
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = transX[transX.Length - i - 1];
                }
            }

            return true;
        }


        #endregion

        private void tsmVirtual_Click(object sender, EventArgs e)
        {
            if (OnVirtualSpec != null)
                OnVirtualSpec(null, null);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            switch (DirEnum)
            {
                case DirEnum.Up:
                    this.vZoom();
                    break;
                case DirEnum.Down:
                    this.vNarrow();
                    break;
                case DirEnum.Left:
                    this.hNarrow(this.MasterPane.PaneList[0].XAxis.Scale.Max);
                    break;
                case DirEnum.Right:
                    this.hZoom(this.MasterPane.PaneList[0].XAxis.Scale.Min);
                    break;
            }
        }

        private Timer timer;

        private DirEnum DirEnum;

        private void btnReduction_Click(object sender, EventArgs e)
        {
            tsmiReduction_Click(null, null);
            this.Refresh();
        }

        private void btnUp_MouseLeave(object sender, EventArgs e)
        {
            mouseX = -1;
            mouseY = -1;
            this.Refresh();
        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            DirEnum = DirEnum.Right;
            timer.Enabled = true;
            timer.Start();
        }

        private void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            DirEnum = DirEnum.Up;
            timer.Enabled = true;
            timer.Start();
        }

        private void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            timer.Stop();
            timer.Enabled = false;
        }

        private void btnDown_MouseDown(object sender, MouseEventArgs e)
        {
            DirEnum = DirEnum.Down;
            timer.Enabled = true;
            timer.Start();
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            DirEnum = DirEnum.Left;
            timer.Enabled = true;
            timer.Start();
        }

        private void tsmiUpSpec_Click(object sender, EventArgs e)
        {
            UpZoom();
        }

        private void tsmiDownSpec_Click(object sender, EventArgs e)
        {
            DownZoom();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {

        }


        bool MyZedGraph_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            float scaleFactor = this.MasterPane.CalcScaleFactor();

            RectangleF rect = this.MasterPane.PaneList[0].CalcClientRect(this.CreateGraphics(), scaleFactor);
            Point pf = new Point(e.X, e.Y);
            MultiPannelProcess(rect, pf, 0, 0);
            return true;
        }


        //private bool MultiPannelProcess(RectangleF rect, Point pf, int i, int panlId)
        //{
        //    string tooltip = string.Empty;
        //    if (rect.Contains(pf))
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.Default;

        //        Console.WriteLine("pf value : x=" + pf.X.ToString() + "  y=" + pf.Y.ToString());
        //        Point mousePt = pf;
        //        double x, y;
        //        base.MasterPane.PaneList[panlId].ReverseTransform(mousePt, out x, out y);
        //        tooltip = "(" + x.ToString() + "," + y.ToString() + ")";
        //        Console.WriteLine(tooltip);

        //        positionInlist = GetPosition(x);
        //        if (positionInlist < 0) return false; ;

        //        Console.WriteLine("index=" + positionInlist.ToString() + "value =" + lstvalues[positionInlist].ToString());

        //        PointF p = base.MasterPane.PaneList[panlId].GeneralTransform(lstvalues[positionInlist], 0, 0);

        //        Console.WriteLine("x:" + p.X.ToString("f2") + "y:" + p.Y.ToString("f2"));

        //        if (this.MasterPane.PaneList.Count == 1)
        //        {
        //            if ((mouseX > 0) && (mouseY > 0))
        //            {
        //                // ControlPaint.DrawReversibleLine(PointToScreen(new Point(mouseX, top)), PointToScreen(new Point(mouseX, bottom)), Color.Black); //根据具体颜色的背景画可逆的线
        //                mouseX = -1;
        //                mouseY = -1;
        //                //刷新界面解决白线滞留尝试 2014-04-23
        //                this.Refresh();
        //            }

        //            int top = (int)this.MasterPane.PaneList[panlId].Chart.Rect.Top;
        //            int left = (int)this.MasterPane.PaneList[panlId].Chart.Rect.Left;
        //            int bottom = (int)this.MasterPane.PaneList[panlId].Chart.Rect.Bottom;
        //            int right = (int)this.MasterPane.PaneList[panlId].Chart.Rect.Right;
        //            using (Graphics g = this.CreateGraphics())
        //            {
        //                g.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(pf.X, top), new Point(pf.X, bottom)); //画对比颜色的线

        //                //int h = WindowFromPoint(pf.X, pf.Y);
        //                //int hD = GetDC(h);
        //                //int c = GetPixel(hD, pf.X, pf.Y);  //取屏幕点颜色
        //                //if (c != -1 && c != 0)
        //                //{
        //                //    int red = c % 256;

        //                //    int green = (c / 256) % 256;

        //                //    int blue = c / 256 / 256;
        //                //    g.DrawLine(new Pen(Color.FromArgb(255 - red, 255 - green, 255 - blue)), new Point(pf.X, top), new Point(pf.X, bottom)); //画对比颜色的线
        //                //    Console.WriteLine("取色正常-" + Color.FromArgb(255 - red, 255 - green, 255 - blue).ToString());
        //                //}
        //                //else
        //                //{
        //                //  //  g.DrawLine(new Pen(Color.FromArgb(this.GraphPane.Chart.Fill.Color.A, 255 - this.GraphPane.Chart.Fill.Color.R, 255 - this.GraphPane.Chart.Fill.Color.G, 255 - this.GraphPane.Chart.Fill.Color.B)), new Point(pf.X, top), new Point(pf.X, bottom));//画对比颜色的线
        //                //    Console.WriteLine("取色不正常-" + Color.FromArgb(this.GraphPane.Chart.Fill.Color.A, 255 - this.GraphPane.Chart.Fill.Color.R, 255 - this.GraphPane.Chart.Fill.Color.G, 255 - this.GraphPane.Chart.Fill.Color.B).ToString());
        //                //}

        //            }

        //            mouseX = pf.X;
        //            mouseY = pf.Y;
        //        }
        //    }


        //    return true;
        //}


        private int GetPosition(double position)
        {
            if (lstvalues.Count > 0)
            {
                return lstvalues.Select((d, i) =>
                {
                    return new
                    {
                        Value = d,
                        index = i
                    };
                }).OrderBy(x => Math.Abs(x.Value - position)).First().index;
            }
            else
                return -1;
        }

        public List<double> lstvalues = new List<double>();

        private int positionInlist;
        public int PositionInList
        {
            get { return positionInlist; }

        }
    }

    

}