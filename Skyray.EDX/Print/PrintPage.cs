using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Print
{
    /// <summary>
    /// 打印模板页面类
    /// </summary>
    //[Serializable]
    public partial class PrintPage : ContainerControl
    {
        /// <summary>
        /// 打印模板页面类构造函数
        /// </summary>
        public PrintPage()
        {
            //设置缓冲模式
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.UserPaint
                    | ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.ResizeRedraw, true);

            InitializeComponent();//设计器的初始化组件事件
            Iint();//初始化事件
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Iint()
        {
            //这里代码需要重构？

            PnlHeader.Page = PnlDesign.Page = PnlFooter.Page = this;
            CalcPanelPadding(PnlHeader, PnlFooter, PnlDesign);//设置控件与容器的边距 

            //页眉大小改变事件注册
            PnlHeader.SizeChanged += (sender, e) =>
            {
                CalcPanelPadding(PnlHeader, PnlDesign);
                foreach (Control ctrl in PnlHeader.Controls)
                    PnlHeader.AdjustPos(ctrl);//调整位置
            };
            //大小改变事件
            PnlFooter.SizeChanged += (sender, e) =>
            {
                CalcPanelPadding(PnlFooter, PnlDesign);//计算页边距
                foreach (Control ctrl in PnlFooter.Controls)
                    PnlFooter.AdjustPos(ctrl);//调整位置
            };

            //大小改变事件
            this.SizeChanged += (sender, e) =>
            {
                int width = this.Width - this.Padding.Left - this.Padding.Right;
                PnlHeader.MinimumSize = new Size(width, PnlHeader.CtrlMargin.Top + 30);
                PnlHeader.MaximumSize = new Size(width, this.Height / 3);

                PnlFooter.MinimumSize = new Size(width, PnlFooter.CtrlMargin.Bottom + 30);
                PnlFooter.MaximumSize = new Size(width, this.Height / 3);

                this.Invalidate();
            };
        }

        #region 属性
        //修改：何晓明 2011-02-22
        //原因：增加打印机及打印纸张        
        public string strPaperSize = "A4";
        public string strDefaultPrinter = string.Empty;
        //
        public int MaxY
        {
            get
            {
                return (Dir == PrintDirection.Vertical ?
                SizeOfPaper.Width : SizeOfPaper.Height)
                - (ShowHeader ? PnlHeader.Height : 0) - FooterHeight;
            }
        }

        public int HeaderHeight
        {
            get { return ShowHeader ? PnlHeader.Height : PrintMargin.Top; }
        }

        public int FooterHeight
        {
            get { return ShowFooter ? PnlFooter.Height : PrintMargin.Bottom; }


        }

        public int SetHeadSplitterPos
        {
            set
            {
                this.splitterTop.SplitPosition = value;
            }
        }

        public int SetFootSplitterPos
        {
            set
            {
                this.splitterBottom.SplitPosition = value;
            }
        }

        /// <summary>
        /// 鼠标位置改变事件
        /// </summary>
        public event EventHandler OnMouseLocationChanged;

        /// <summary>
        /// 记录鼠标位置
        /// </summary>
        private Point _MouseLocation = Point.Empty;

        /// <summary>
        /// 当前鼠标位置
        /// </summary>
        [Browsable(false), DefaultValue(typeof(Point), "Point.Empty")]
        public Point MouseLocation
        {
            get { return _MouseLocation; }
            set
            {
                _MouseLocation = value;
                if (OnMouseLocationChanged != null) OnMouseLocationChanged(null, null);
            }
        }

        /// <summary>
        /// 当前鼠标所在区域类型
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public PrintPanel CurrentPanel { get; set; }

        /// <summary>
        /// 当前鼠标点击的区域Panel
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public PrintPanel SelectedPanel { get; set; }


        [Browsable(false), DefaultValue(null)]
        public UCPrint UCPrint { get; set; }

        /// <summary>
        /// 打印页边距
        /// </summary>
        private Padding _PrintMargin = new Padding(50, 50, 50, 50);

        private int _minNum = 10;

        /// <summary>
        /// 打印页边距
        /// </summary>
        [Browsable(false)]
        public Padding PrintMargin
        {
            get { return _PrintMargin; }
            set
            {
                _PrintMargin = value;
                CalcPanelPadding(Header, Footer, Body);
            }
        }

        /// <summary>
        /// 纸张大小
        /// </summary>
        public Size SizeOfPaper { get; set; }

        /// <summary>
        /// 纸张大小
        /// </summary>
        public PaperSize PaperSize { get; set; }

        /// <summary>
        /// 打印方向
        /// </summary>
        public PrintDirection Dir { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        public PageSize PageSize { get; set; }

        /// <summary>
        /// 设置页面参数
        /// </summary>
        public void SetPrintPageParam()
        {
            //修改：何晓明 2011-02-14
            //原因：增加纸张类型 界面保持原样
            //SizeOfPaper = PrintHelper.GetPaperSize(PaperSize.A4);
            //SizeOfPaper = PrintHelper.GetPaperSize(this.PaperSize);
            //

            //修改：何晓明 2011-02-22
            //原因：增加打印机及打印纸张 更改页面大小          
            System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
            strDefaultPrinter = doc.PrinterSettings.PrinterName;
            SizeOfPaper = PrintHelper.GetPaperSize(strPaperSize);
            //

            Size size = Size.Empty;
            int K = (int)PageSize + 1;
            bool b = Dir == PrintDirection.Vertical;

            int width = b ? SizeOfPaper.Height : SizeOfPaper.Width;
            int height = b ? SizeOfPaper.Width : SizeOfPaper.Height;

            size.Width = width;
            size.Height = height * K;

            Size = size;

            CalcCtrlMaxSize(PnlDesign, PnlFooter, PnlFooter);
        }

        private bool _ShowHeader = true;
        /// <summary>
        /// 显示页眉
        /// </summary>
        [Browsable(false), DefaultValue(true)]
        public bool ShowHeader
        {
            get
            {
                return _ShowHeader;
            }
            set
            {
                if (value != _ShowHeader)
                {
                    _ShowHeader = value;
                    splitterTop.Visible = value;
                    PnlHeader.Visible = value;
                    PnlDesign.ShowHeaderCorner = !value;
                    CalcPanelPadding(PnlDesign);

                    AdjustBodyCtrlPosAfterShowHeaderChanged();

                    PnlDesign.CreateCtrlRegion();
                }
            }
        }
        private void AdjustBodyCtrlPosAfterShowHeaderChanged()
        {
            foreach (Control ctrl in PnlDesign.Controls)
            {
                int top = 0;
                if (_ShowHeader)
                {
                    top = ctrl.Top - PrintMargin.Top;
                }
                else
                {
                    top = ctrl.Top + PrintMargin.Top;
                }
                int maxTop = Height - HeaderHeight - FooterHeight - ctrl.Height - 8;

                ctrl.Top = top < maxTop ? top : maxTop;
            }
        }
        private void AdjustBodyCtrlPosAfterShowFooterChanged()
        {
            foreach (Control ctrl in PnlDesign.Controls)
            {
                int top = 0;
                if (_ShowFooter && ctrl.Top + FooterHeight + ctrl.Height + 8 > Height)
                {
                    top = ctrl.Top - (PnlFooter.Height - PrintMargin.Top);
                }
                else
                {
                    top = ctrl.Top;
                }
                int maxTop = Height - HeaderHeight - FooterHeight - ctrl.Height - 8;

                ctrl.Top = top < maxTop ? top : maxTop;
            }
        }

        private bool _ShowFooter = false;
        /// <summary>
        /// 显示页脚
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool ShowFooter
        {
            get { return _ShowFooter; }
            set
            {
                if (value != _ShowFooter)
                {
                    _ShowFooter = value;
                    splitterBottom.Visible = value;
                    PnlFooter.Visible = value;
                    PnlDesign.ShowFooterCorner = !value;
                    CalcPanelPadding(PnlDesign);
                    AdjustBodyCtrlPosAfterShowFooterChanged();
                    PnlDesign.CreateCtrlRegion();
                }
            }
        }

        /// <summary>
        /// 页眉
        /// </summary>
        public PrintPanel Header
        {
            get { return PnlHeader; }
        }

        /// <summary>
        /// 页脚
        /// </summary>
        public PrintPanel Footer
        {
            get { return PnlFooter; }
        }

        /// <summary>
        /// 设计主体区域
        /// </summary>
        public PrintPanel Body
        {
            get { return PnlDesign; }
        }

        #endregion

        /// <summary>
        /// 计算容器内控件最大宽度
        /// </summary>
        /// <param name="panels"></param>
        private void CalcCtrlMaxSize(params PrintPanel[] panels)
        {
            foreach (var panel in panels)
            {
                var sizeMax = panel.GetInnerCtrlMaxSize();
                foreach (PrintCtrl ctrl in panel.Controls)
                {
                    ctrl.MaximumSize = sizeMax;                    
                }
            }
        }
        

        /// <summary>
        /// 计算pnl边距
        /// </summary>
        /// <param name="pnls"></param>
        public void CalcPanelPadding(params PrintPanel[] pnls)
        {
            foreach (var pnl in pnls) CalcPanelPadding(pnl);
        }

        /// <summary>
        /// 计算pnl边距
        /// </summary>
        /// <param name="panel"></param>
        private void CalcPanelPadding(PrintPanel panel)
        {
            int left = PrintMargin.Left;
            int right = PrintMargin.Right;
            int top = 0;
            int bottom = 0;
            switch (panel.Type)
            {
                case CtrlType.Header://Header
                    top = PrintMargin.Top;
                    if (top > panel.Height - _minNum)
                    {
                        top = panel.Height - _minNum;
                        PrintMargin = new Padding(
                            PrintMargin.Left,
                            top,
                            PrintMargin.Right,
                            PrintMargin.Bottom);
                    }
                    break;
                case CtrlType.Body://Body
                    top = _ShowHeader ? 0 : PrintMargin.Top;
                    bottom = _ShowFooter ? 0 : PrintMargin.Bottom;
                    break;
                case CtrlType.Footer://Footer
                    bottom = PrintMargin.Bottom;
                    if (bottom > panel.Height - _minNum)
                    {
                        bottom = panel.Height - _minNum;
                        PrintMargin = new Padding(
                             PrintMargin.Left,
                             PrintMargin.Top,
                             PrintMargin.Right,
                             bottom);
                    }
                    break;
            }
            panel.CtrlMargin = new Padding(left, top, right, bottom);//设置
            panel.Invalidate(false);//刷新
        }

        #region 自定义选择事件
        public event EventHandler OnSelectCtrlChanged;
        public event EventHandler OnSelectCtrlChangeing;
        private Control _PropertyOBJ;
        /// <summary>
        /// 当前控件
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public Control PropertyObject
        {
            get { return _PropertyOBJ; }
            set
            {                
                if (_PropertyOBJ != value)
                {
                    if (OnSelectCtrlChangeing != null) OnSelectCtrlChangeing(_PropertyOBJ, null);//即将改变
                    _PropertyOBJ = value;
                    if (OnSelectCtrlChanged != null) OnSelectCtrlChanged(_PropertyOBJ, null);//选择改变
                    //_PropertyOBJ.Focus();
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取页面数据源
        /// </summary>
        /// <returns></returns>
        public TemplateSource GetTemplateSource()
        {
            TemplateSource source = new TemplateSource();
            source.PaperSize = this.PaperSize;
            source.PageMargin = PrintMargin;
            source.ShowFooter = _ShowFooter;
            source.ShowHeader = _ShowHeader;
            source.PageSize = PageSize;
            //修改：何晓明 2011-02-22
            //原因：增加打印机及打印纸张
            source.strPaperSize = strPaperSize;
            //
            source.HeadSplitterPosition = this.splitterTop.SplitPosition;
            source.BottomSplitterPosition = this.splitterBottom.SplitPosition;
            source.Dir = Dir;


            if (_ShowHeader)
                source.PrintPanels.Add(new Panels { PanelType = CtrlType.Header, NodeInfos = Header.NodeInfos });
            if (_ShowFooter)
                source.PrintPanels.Add(new Panels { PanelType = CtrlType.Footer, NodeInfos = Footer.NodeInfos });
            source.PrintPanels.Add(new Panels { PanelType = CtrlType.Body, NodeInfos = Body.NodeInfos });

            return source;
        }
    }
}
