using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using Skyray.Controls.Tree;
using System.ComponentModel;
using System.Data;
using SourceGrid;
using System.Collections;
namespace Skyray.Print
{
    public partial class PrintPanel : Control
    {
        #region 属性

        private CtrlType _Type = CtrlType.None;
        public CtrlType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        /// <summary>
        /// 即将添加至设计器的控件边框集合
        /// </summary>
        [Browsable(false)]
        public Dictionary<object, Rectangle> RectsToAdd { get; private set; }

        /// <summary>
        /// 选中的子控件拖动时矩形边框集合
        /// </summary>
        [Browsable(false)]
        public Dictionary<object, Rectangle> Rects { get; private set; }

        /// <summary>
        /// 当前选择的控件
        /// </summary>

        [Browsable(false), DefaultValue(null)]
        public Control CurrentCtrl { get; set; }

        /// <summary>
        /// 本页面选中的控件
        /// </summary>
        [Browsable(false)]
        public List<PrintCtrl> SelectedCtrls { get; private set; }

        /// <summary>
        /// 控件与父容器间距
        /// </summary>
        [Browsable(false), DefaultValue(typeof(Padding), "Padding.Empty")]
        public Padding CtrlMargin { get; set; }

        /// <summary>
        /// 页面容器
        /// </summary>
        [Browsable(false)]
        public PrintPage Page { get; set; }


        private bool _ShowHeaderCorner = true;
        /// <summary>
        /// 是否显示页眉Corner
        /// </summary>
        public bool ShowHeaderCorner
        {
            get { return _ShowHeaderCorner; }
            set
            {
                _ShowHeaderCorner = value;
                base.Invalidate();
            }
        }

        private bool _ShowFooterCorner = true;
        /// <summary>
        /// 是否显示页脚Corner
        /// </summary>
        public bool ShowFooterCorner
        {
            get { return _ShowFooterCorner; }
            set
            {
                _ShowFooterCorner = value;
                base.Invalidate();
            }
        }

        #region 备用属性，记录拖动控件时坐标偏移量
        public int OffSetX { get { return _OffSetX; } }
        public int OffSetY { get { return _OffSetY; } }
        #endregion

        #endregion

        #region 私有字段
        private int _OffSetX = 0;//X轴偏移量
        private int _OffSetY = 0;//Y轴偏移量


        /// <summary>
        /// 记录当前鼠标状态
        /// </summary>
        private bool Dragging = false;

        /// <summary>
        /// 拖拽开始点坐标
        /// </summary>
        private Point DragStart = Point.Empty;

        /// <summary>
        /// 控件所在区域矩形
        /// </summary>
        private Rectangle CtrlRect = Rectangle.Empty;

        private Region CtrlRegion;

        /// <summary>
        /// 拖动轨迹矩形
        /// </summary>
        private Rectangle DragRect = Rectangle.Empty;

        /// <summary>
        /// //是否正在拖拽
        /// </summary>
        private bool TreeDragging = false;

        /// <summary>
        /// 要增加至设计器的控件集合
        /// </summary>
        private List<PrintCtrl> LstCtrlToAdd = new List<PrintCtrl>();

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public PrintPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.ResizeRedraw, true);
            InitializeComponent();
            Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.AllowDrop = true;//允许拖拽
            BackColor = Color.White;//白色背景
            SelectedCtrls = new List<PrintCtrl>();
            Rects = new Dictionary<object, Rectangle>();
            RectsToAdd = new Dictionary<object, Rectangle>();
            this.SizeChanged += (sender, e) => CreateCtrlRegion();
            this.ControlRemoved += (sender, e) =>
            {
                SelectedCtrls.Clear();
                if (SelectedCtrls.Count == 0) Page.PropertyObject = this;
            };
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="page"></param>
        public PrintPanel(PrintPage page)
            : this()
        {
            Page = page;
        }

        /// <summary>
        /// 刷新设计区域
        /// </summary>
        public void InvalidateW()
        {
            Invalidate(CtrlRegion, false);
        }

        /// <summary>
        /// 计算拖动边界
        /// </summary>
        private void CalcRange()
        {
            if (DragRect.Left < CtrlMargin.Left)
                DragRect = new Rectangle(CtrlMargin.Left + 1,
                                         DragRect.Top,
                                         DragRect.Width - CtrlMargin.Left + DragRect.Left,
                                         DragRect.Height);

            if (DragRect.Top < CtrlMargin.Top)
                DragRect = new Rectangle(DragRect.Left,
                                         CtrlMargin.Top + 1,
                                         DragRect.Width,
                                         DragRect.Height - CtrlMargin.Top + DragRect.Top);

            if (DragRect.Height + DragRect.Top > this.Height - CtrlMargin.Bottom)
            {
                DragRect = new Rectangle(DragRect.Left, DragRect.Top, DragRect.Width, this.Height - DragRect.Top - CtrlMargin.Bottom - 1);
            }

            if (DragRect.Width + DragRect.Left > this.Width - CtrlMargin.Right)
            {
                DragRect = new Rectangle(DragRect.Left, DragRect.Top, this.Width - CtrlMargin.Right - DragRect.Left - 1, DragRect.Height);
            }
        }

        #region 事件重载

        /// <summary>
        /// 客户设计区域
        /// </summary>
        public void CreateCtrlRegion()
        {
            CtrlRect = new Rectangle(CtrlMargin.Left + 1, CtrlMargin.Top + 1,
                                     this.ClientSize.Width - CtrlMargin.Left - CtrlMargin.Right - 1,
                                     this.ClientSize.Height - CtrlMargin.Top - CtrlMargin.Bottom - 1);
            CtrlRegion = new Region(CtrlRect);
        }

        /// <summary>
        /// 重载鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.Page.CurrentPanel != this) this.Page.CurrentPanel = this; //页面当前Panel
            this.Page.MouseLocation = new Point(e.X, e.Y);//鼠标位置
            if (Dragging)
            {
                _OffSetX = e.X - DragStart.X;//X轴偏移量
                _OffSetY = e.Y - DragStart.Y;//Y轴偏移量
                DragRect = new Rectangle(DragStart.X, DragStart.Y, _OffSetX, _OffSetY);
                DragRect = PrintHelper.ConvertRect(DragRect);
                CalcRange();
                InvalidateW();
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// 重载鼠标放开事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Page.UCPrint.ShowCmsPage(this.PointToScreen(e.Location));
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _OffSetX = _OffSetY = 0;
                this.Dragging = false;
                base.Capture = false;

                foreach (PrintCtrl ctrl in base.Controls)
                {
                    bool isToBeSelected = DragRect.IntersectsWith(new Rectangle(ctrl.Location, ctrl.Size));
                    if (isToBeSelected) ctrl.Selected = true;
                }

                this.Page.PropertyObject = SelectedCtrls.Count == 1 ? SelectedCtrls[0] : null;
                this.DragRect = Rectangle.Empty;
                this.InvalidateW();
            }
        }


        /// <summary>
        /// 重载绘制事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Rects != null && Rects.Count > 0) Rects.ToList().ForEach(r => e.Graphics.DrawRectangle(Param.DashPen, r.Value));
            if (RectsToAdd != null && RectsToAdd.Count > 0) RectsToAdd.ToList().ForEach(r => e.Graphics.DrawRectangle(Param.DashPen, r.Value));
            if (DragRect != Rectangle.Empty) e.Graphics.DrawRectangle(Param.DashPen, DragRect);

            if (_ShowHeaderCorner) DrawHeaderCorner(e.Graphics, this);
            if (_ShowFooterCorner) DrawFooterCorner(e.Graphics, this);
        }

        /// <summary>
        /// 重载鼠标按下事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Page.SelectedPanel = this;
            // Page.Parent.Focus();//Tabpage获得焦点
            Focus();
            _OffSetX = _OffSetY = 0;
            if (e.Button == MouseButtons.Left)
            {
                this.Dragging = base.Capture = true;
                this.DragStart = new Point(e.X, e.Y);

                if (Keys.Control == Control.ModifierKeys) { return; }
                foreach (PrintCtrl myCtrl in this.SelectedCtrls.ToArray())
                    if (myCtrl.Selected) myCtrl.Selected = false;
            }
        }

        /// <summary>
        /// 拖放至区域
        /// </summary>
        /// <param name="drgevent"></param>
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            var p = PointToClient(new Point(drgevent.X, drgevent.Y));
            bool b = CtrlRect.Contains(p);
            if (b)
            {
                if (CtrlType.Body != Type && LstCtrlToAdd.OfType<PrintTable>().Count() > 0)
                    b = false;
            }
            drgevent.Effect = b ? DragDropEffects.Move : DragDropEffects.None;
            if (drgevent.Effect == DragDropEffects.Move)
            {
                int top = 0;
                for (int i = 0; i < LstCtrlToAdd.Count; i++)
                {
                    if (i != 0) { top += LstCtrlToAdd[i - 1].Height + 2; }//位置纵坐标递增
                    LstCtrlToAdd[i].Location = new Point(p.X, p.Y + top);//设置位置，左对齐
                    RectsToAdd[LstCtrlToAdd[i]] = LstCtrlToAdd[i].Bounds;
                }
            }
            else
                RectsToAdd.Clear();

            Invalidate();
        }

        /// <summary>
        /// 由TreeNodeInfo实例创建PrintCtrl
        /// </summary>
        /// <param name="infoSource">从数据源查找到得到的TreeNodeInfo</param>
        /// <param name="templateInfo">模板中存取的treenodeInfo</param>
        /// <returns></returns>
        public List<PrintCtrl> CreatePrintCtrl(Node infoS, TreeNodeInfo templateInfo)
        {
            List<PrintCtrl> listPt = new List<PrintCtrl>();
            PrintCtrl ctrl = null;
            TreeNodeInfo infoSource = infoS.Tag as TreeNodeInfo;
            var infoCurrent = templateInfo == null ? infoSource : templateInfo;
            bool bGetValueFromTemplateInfo = templateInfo != null && templateInfo.SaveValue;
            if (infoSource.Type == CtrlType.Label)
            {
                ctrl = new PrintCtrl(this);
            }
            else if (infoSource.Type == CtrlType.Field)
            {
                if (!infoSource.Parent)
                {
                    var field = new PrintField(this);
                    field.TextValue = bGetValueFromTemplateInfo ? templateInfo.TextValue : infoSource.TextValue;
                    field.TextValueColor = infoCurrent.TextValueColor;
                    field.TextValueFont = infoCurrent.TextValueFont;
                    ctrl = field;
                }
                else
                {
                    if (infoS.Nodes.Count == 0)
                        return null;
                    foreach (Node childNode in infoS.Nodes)
                    {
                        List<PrintCtrl> prt = CreatePrintCtrl(childNode, null);
                        listPt.AddRange(prt);
                    }
                    return listPt;
                }
            }
            else if (infoSource.Type == CtrlType.Grid)
            {
                var c = new PrintTable(this);
                c.Table = infoSource.Table;
                c.RangeColumns = infoSource.RangeColumns;
                if (templateInfo != null)
                {
                    c.TextVSpace = infoCurrent.TextVSpace;//文本垂直间距
                    //表格样式
                    c.TableStyle = Activator.CreateInstance(infoCurrent.TableStyleType) as ITableStyle;
                    c.ColHeight = infoCurrent.ColHeight;//列高度
                    c.RowHeight = infoCurrent.RowHeight;//行高度
                    c.ColInfos = infoCurrent.ColSetInfos;//列信息集合
                }
                ctrl = c;
            }
            else if (infoSource.Type == CtrlType.Image)
            {
                var c = new PrintImage(this);
                c.Image = bGetValueFromTemplateInfo ? templateInfo.Image : infoSource.Image;
                c.DrawImageBorder = infoCurrent.ShowPicBorder;
                c.ImageBorderColor = infoCurrent.PicBorderColor;
                c.TextVSpace = infoCurrent.TextVSpace;
                ctrl = c;
            }
            else if (infoSource.Type == CtrlType.ComposeTable)
            {
                if (infoSource.contextType == CompositeContextType.Original)
                {
                    var c = new PrintCompositeTable(this);
                    c.DTable = infoSource.Tables;
                    c.DataSourceList = infoSource.ObjSource;
                    ctrl = c;
                }
            }
            ctrl.Type = infoSource.Type;
            ctrl.NodeInfo = infoSource;//记录数据源

            ctrl.Name = infoSource.Type.ToString() + "." + infoSource.Name;
            ctrl.Text = bGetValueFromTemplateInfo ? templateInfo.Text : infoSource.Text;

            ctrl.TextFont = infoCurrent.TextFont;
            ctrl.TextColor = infoCurrent.TextColor;

            if (bGetValueFromTemplateInfo) ctrl.SaveValue = true;

            if (CurrentCtrl != null) CurrentCtrl.Invalidate();//刷新之前的控件
            CurrentCtrl = ctrl;//设置新增加的控件为当前控件

            // ctrl.Selected = true;//默认选中
            ctrl.InitFinish = true;
            //修改：何晓明 2011-03-22
            //原因：表格数据为空时不能拖出到界面
            if (ctrl != null && ctrl.Type == CtrlType.Grid && ((ctrl as PrintTable).Table == null || ((ctrl as PrintTable).Table != null && (ctrl as PrintTable).Table.Rows.Count == 0)))
                ctrl = null;
            //
            if (ctrl != null)
                listPt.Add(ctrl);
            return listPt;
        }

        /// <summary>
        /// 由TreeNodeInfo实例创建PrintCtrl
        /// </summary>
        /// <param name="infoSource">从数据源查找到得到的TreeNodeInfo</param>
        /// <param name="templateInfo">模板中存取的treenodeInfo</param>
        /// <returns></returns>
        public PrintCtrl CreatePrintCtrl(TreeNodeInfo infoSource, TreeNodeInfo templateInfo)
        {
            PrintCtrl ctrl = null;
            var infoCurrent = templateInfo == null ? infoSource : templateInfo;
            bool bGetValueFromTemplateInfo = templateInfo != null && templateInfo.SaveValue;
            if (infoSource.Type == CtrlType.Label)
            {
                ctrl = new PrintCtrl(this);
            }
            else if (infoSource.Type == CtrlType.Field)
            {
                //if (!infoSource.Parent)
                //{
                var field = new PrintField(this);
                field.TextValue = bGetValueFromTemplateInfo ? templateInfo.TextValue : infoSource.TextValue;
                field.TextValueColor = infoCurrent.TextValueColor;
                field.TextValueFont = infoCurrent.TextValueFont;
                ctrl = field;
                //}
                //else
                //{
                //    if (infoS.Nodes.Count == 0)
                //        return null;
                //    foreach (Node childNode in infoS.Nodes)
                //    {
                //        List<PrintCtrl> prt = CreatePrintCtrl(childNode, null);
                //        listPt.AddRange(prt);
                //    }
                //    return listPt;
                //}
            }
            else if (infoSource.Type == CtrlType.Grid)
            {
                var c = new PrintTable(this);
                //修改：何晓明 2011-03-22
                //原因：表格内容为空时不显示
                if (infoSource.Table == null)
                {
                    //infoSource.Text = "";
                    return null;
                }
                else if (infoSource.Table.Rows.Count == 0)
                {
                    //infoSource.Text = "";
                    return null;
                }
                //
                c.Table = infoSource.Table;
                if (templateInfo != null)
                {
                    c.TextVSpace = infoCurrent.TextVSpace;//文本垂直间距
                    //表格样式
                    c.TableStyle = Activator.CreateInstance(infoCurrent.TableStyleType) as ITableStyle;
                    c.ColHeight = infoCurrent.ColHeight;//列高度
                    c.RowHeight = infoCurrent.RowHeight;//行高度   
                    //修改：何晓明 2011-01-18
                    //原因：Table自动调整新增列宽
                    Size originalSize = templateInfo.Size;
                    int iNewWidth = 0;
                    bool flag = false;
                    try
                    {
                        DataColumnCollection ColumnsContents = c.Table.Columns;
                        foreach (DataColumn column in ColumnsContents)
                        {
                            //修改：何晓明 2011-03-18
                            //原因：表格因隐藏列排版错误
                            ColInfo colinfo = infoCurrent.ColSetInfos.FirstOrDefault(w => w.Name == column.ColumnName);
                            if (colinfo == null)
                            {
                                iNewWidth += 75;
                                flag = true;
                            }
                            else if (colinfo.Visiable == true)
                            {
                                iNewWidth += colinfo.Width;
                            }
                            //
                            //ColInfo colinfo = infoCurrent.ColSetInfos.FirstOrDefault(w => w.Name == column.ColumnName && w.Visiable == true);
                            //if (colinfo != null)
                            //    iNewWidth += colinfo.Width;
                            //else
                            //{
                            //    iNewWidth += 75;
                            //    templateInfo.Size = new Size(iNewWidth + 15, originalSize.Height + iNewWidth / Page.Width * infoCurrent.RowHeight + infoCurrent.ColHeight);
                            //}

                        }
                        //修改：何晓明 2011-03-18
                        //原因：表格因隐藏列排版错误
                        if (flag)
                        {
                            templateInfo.Size = new Size(iNewWidth + 20, originalSize.Height + iNewWidth / Page.Width * infoCurrent.RowHeight + infoCurrent.ColHeight);
                        }
                        //
                    }
                    catch (Exception)
                    {

                    }
                    //
                    c.ColInfos = infoCurrent.ColSetInfos;//列信息集合
                }
                ctrl = c;
            }
            else if (infoSource.Type == CtrlType.Image)
            {
                var c = new PrintImage(this);
                c.Image = bGetValueFromTemplateInfo ? templateInfo.Image : infoSource.Image;
                c.DrawImageBorder = infoCurrent.ShowPicBorder;
                c.ImageBorderColor = infoCurrent.PicBorderColor;
                c.TextVSpace = infoCurrent.TextVSpace;
                ctrl = c;
            }
            else if (infoSource.Type == CtrlType.ComposeTable)
            {
                var c = new PrintCompositeTable(this);
                c.CellInfo = infoCurrent.CellContext;
                c.DTable = infoSource.Tables;
                c.DataSourceList = infoSource.ObjSource;
                ctrl = c;
            }

            ctrl.Type = infoSource.Type;
            ctrl.NodeInfo = infoSource;//记录数据源

            ctrl.Name = infoSource.Type.ToString() + "." + infoSource.Name;
            ctrl.Text = bGetValueFromTemplateInfo ? templateInfo.Text : infoSource.Text;

            ctrl.TextFont = infoCurrent.TextFont;
            ctrl.TextColor = infoCurrent.TextColor;

            if (bGetValueFromTemplateInfo) ctrl.SaveValue = true;

            if (CurrentCtrl != null) CurrentCtrl.Invalidate();//刷新之前的控件
            CurrentCtrl = ctrl;//设置新增加的控件为当前控件

            // ctrl.Selected = true;//默认选中
            ctrl.InitFinish = true;
            return ctrl;
        }

        /// <summary>
        /// OnDragEnter
        /// </summary>
        /// <param name="drgevent"></param>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (TreeDragging) return;
            RectsToAdd.Clear();
            LstCtrlToAdd.Clear();

            var p = PointToClient(new Point(drgevent.X, drgevent.Y));
            var SelectedNodes = (TreeNodeAdv[])drgevent.Data.GetData(typeof(TreeNodeAdv[]));

            foreach (var node in SelectedNodes)
            {
                //添加控件至容器
                LstCtrlToAdd.AddRange(CreatePrintCtrl(node.Tag as Node, null));
            }
            TreeDragging = true;
        }
        /// <summary>
        /// OnDragLeave
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragLeave(EventArgs e)
        {
            ClearData();
        }
        /// <summary>
        /// OnDragDrop
        /// </summary>
        /// <param name="drgevent"></param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            TreeDragging = false;
            RectsToAdd.Clear();
            SelectedCtrls.ForEach(ctrl => ctrl.Selected = false);//取消之前的选择
            LstCtrlToAdd.ForEach(ctrl =>
            {
                Controls.Add(ctrl);
                if (Param.AdjustCtrlPos) AdjustPos(ctrl);
                ctrl.Selected = true;
            });

            Page.PropertyObject = LstCtrlToAdd.Count == 1 ? LstCtrlToAdd[0] : null;//属性控件

            LstCtrlToAdd.Clear();
            Invalidate();
            Focus();
            Page.SelectedPanel = this;
        }

        /// <summary>
        /// 调整控件位置
        /// </summary>
        /// <param name="ctrl"></param>
        public void AdjustPos(Control ctrl)
        {
            int maxX = Math.Max(CtrlMargin.Left, Width - CtrlMargin.Right - ctrl.Width);
            if (ctrl.Left > maxX) ctrl.Left = maxX;
            if (ctrl.Left < CtrlMargin.Left) ctrl.Left = CtrlMargin.Left;

            int maxY = Math.Max(CtrlMargin.Top, Height - CtrlMargin.Bottom - ctrl.Height);
            if (ctrl.Top > maxY) ctrl.Top = maxY;
            if (ctrl.Top < CtrlMargin.Top) ctrl.Top = CtrlMargin.Top;
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearData()
        {
            TreeDragging = false;
            RectsToAdd.Clear();
            LstCtrlToAdd.ForEach(ctrl => SelectedCtrls.Remove(ctrl));
            LstCtrlToAdd.Clear();
            Rects.Clear();
            Invalidate();
        }

        #endregion

        #region 右键菜单事件

        /// <summary>
        /// 删除所选控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelSelected_Click(object sender, EventArgs e)
        {
            DelSelectCtrl();
        }

        /// <summary>
        /// 删除全部控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelAll_Click(object sender, EventArgs e)
        {
            SelectedCtrls.Clear();//清空选择控件列表
            Controls.Clear();//从页面中移除所有控件           
        }
        /// <summary>
        /// 键盘消息响应
        /// </summary>
        /// <param name="key"></param>
        public void ProcessKey(Keys key)
        {
            if (!Focused) return;

            if (SelectedCtrls.Count > 0)
            {
                switch (key)
                {
                    case Keys.Up:
                        SelectedCtrls.ForEach(ctrl =>
                        {
                            int MinY;
                            if (ctrl.Panel.Type == CtrlType.Header) MinY = CtrlMargin.Top;
                            else if (ctrl.Panel.Type == CtrlType.Body) MinY = Page.ShowHeader ? 0 : CtrlMargin.Top;
                            else MinY = 0;
                            if (ctrl.Top > MinY) ctrl.Top -= 1;
                        });
                        break;
                    case Keys.Down:
                        SelectedCtrls.ForEach(ctrl =>
                        {
                            int MaxY;
                            if (ctrl.Panel.Type == CtrlType.Header) MaxY = Page.Header.Height - ctrl.Height;
                            else if (ctrl.Panel.Type == CtrlType.Body) MaxY = Page.Body.Height - (Page.ShowFooter ? 0 : Page.Body.CtrlMargin.Bottom) - ctrl.Height;
                            else MaxY = Page.Footer.Height - Page.Footer.CtrlMargin.Bottom - ctrl.Height;
                            if (ctrl.Top < MaxY) ctrl.Top += 1;
                        });
                        break;
                    case Keys.Left://LeftLeft
                        SelectedCtrls.ForEach(ctrl =>
                        {
                            int MinX = Page.Body.CtrlMargin.Left;
                            if (ctrl.Left > MinX) ctrl.Left -= 1;
                        });
                        break;
                    case Keys.Right://Right
                        SelectedCtrls.ForEach(ctrl =>
                        {
                            int MaxX = Page.Width - Page.Body.CtrlMargin.Right - ctrl.Width;
                            if (ctrl.Left < MaxX) ctrl.Left += 1;
                        });
                        break;
                    case Keys.Delete://Delete
                        foreach (var ctrl in SelectedCtrls.ToArray())
                        {
                            this.Controls.Remove(ctrl);
                        }
                        break;

                    default: break;//default
                }
            }
        }

        #endregion

        #region 页眉页脚直角绘制

        /// <summary>
        /// 绘制页眉直角
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pnl"></param>
        private void DrawHeaderCorner(Graphics g, PrintPanel pnl)
        {
            g.DrawLine(Param.CornerPen,//左边角横线
                pnl.CtrlMargin.Left - 20, pnl.CtrlMargin.Top, pnl.CtrlMargin.Left, pnl.CtrlMargin.Top);
            g.DrawLine(Param.CornerPen, //左边角竖线
                pnl.CtrlMargin.Left, pnl.CtrlMargin.Top - 20, pnl.CtrlMargin.Left, pnl.CtrlMargin.Top);
            g.DrawLine(Param.CornerPen,  //右边角横线
                pnl.Width - pnl.CtrlMargin.Right + 20, pnl.CtrlMargin.Top, pnl.Width - pnl.CtrlMargin.Right, pnl.CtrlMargin.Top);
            g.DrawLine(Param.CornerPen, //右边角竖线
                pnl.Width - pnl.CtrlMargin.Right, pnl.CtrlMargin.Top - 20, pnl.Width - pnl.CtrlMargin.Right, pnl.CtrlMargin.Top);
        }

        /// <summary>
        /// 绘制页脚直角
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pnl"></param>
        private void DrawFooterCorner(Graphics g, PrintPanel pnl)
        {
            int LeftCenterX = pnl.CtrlMargin.Left;
            int CenterY = pnl.Height - pnl.CtrlMargin.Bottom;
            int RightCenterX = pnl.Width - pnl.CtrlMargin.Right;

            g.DrawLine(Param.CornerPen, LeftCenterX, CenterY, LeftCenterX - 20, CenterY); //左边角横线 
            g.DrawLine(Param.CornerPen, LeftCenterX, CenterY + 20, LeftCenterX, CenterY);  //左边角竖线
            g.DrawLine(Param.CornerPen, RightCenterX + 20, CenterY, RightCenterX, CenterY);//右边角横线
            g.DrawLine(Param.CornerPen, RightCenterX, CenterY + 20, RightCenterX, CenterY);//右边角竖线
        }

        #endregion

        /// <summary>
        /// 页面TreeNode集合
        /// </summary>
        public List<TreeNodeInfo> NodeInfos
        {
            get { return GetNodeInfosToSave(); }
        }

        /// <summary>
        /// 获取TreeNodeInfo对象集合，用于客户保存模板
        /// </summary>
        /// <returns></returns>
        private List<TreeNodeInfo> GetNodeInfosToSave()
        {
            bool IsBody = this.Type == CtrlType.Body;//是否为Body
            var nodeInfos = new List<TreeNodeInfo>();

            foreach (PrintCtrl ctrl in Controls)
            {
                var typ = ctrl.Type;
                if (!IsBody && typ == CtrlType.Grid) continue;//表格不能存在于页眉或者页尾
                var nodeInfoNew = new TreeNodeInfo();
                nodeInfoNew.Name = ctrl.NodeInfo.Name;//名称 
                nodeInfoNew.Size = ctrl.Size;

                //bool saveValue = !IsBody && ctrl.SaveValue;//是否需要保存值
                bool saveValue =  ctrl.SaveValue;
                if (typ == CtrlType.Field)
                {
                    var c = ctrl as PrintField;
                    nodeInfoNew.TextValueColor = c.TextValueColor;//颜色
                    nodeInfoNew.TextValueFont = c.TextValueFont;
                    if (saveValue) nodeInfoNew.TextValue = c.TextValue;
                    nodeInfoNew.Size = c.Size;
                }
                else if (typ == CtrlType.Grid)
                {
                    var c = ctrl as PrintTable;

                    nodeInfoNew.TextVSpace = c.TextVSpace;//垂直间距
                    nodeInfoNew.TableStyleType = c.TableStyle.GetType();//表格样式
                    nodeInfoNew.ColHeight = c.ColHeight;//列宽度
                    //列宽度
                    nodeInfoNew.RowHeight = c.RowHeight;//高度
                    nodeInfoNew.Size = c.Size;
                    nodeInfoNew.ColSetInfos = c.NodeInfo.TableAutoSize ? null : c.ColInfos;//表格设置信息
                }
                else if (typ == CtrlType.Image)
                {
                    var c = ctrl as PrintImage;
                    nodeInfoNew.TextVSpace = c.TextVSpace;//垂直间距
                    nodeInfoNew.ShowPicBorder = c.DrawImageBorder;
                    nodeInfoNew.PicBorderColor = c.ImageBorderColor;
                    nodeInfoNew.Size = c.Size;
                    if (saveValue) nodeInfoNew.Image = c.Image;
                }
                else if (typ == CtrlType.ComposeTable)
                {
                    var c = ctrl as PrintCompositeTable;
                    nodeInfoNew.TextVSpace = c.TextVSpace;//垂直间距
                    //nodeInfoNew.ShowPicBorder = c.DrawImageBorder;
                    //nodeInfoNew.PicBorderColor = c.ImageBorderColor;
                    nodeInfoNew.Size = c.Size;
                    nodeInfoNew.CellContext = c.CellInfo;
                    //if (saveValue) nodeInfoNew.Image = c.Image;
                }

                nodeInfoNew.Type = ctrl.Type;//控件类型           
                nodeInfoNew.TextFont = ctrl.TextFont;//字体
                nodeInfoNew.TextColor = ctrl.TextColor;//字体颜色
                nodeInfoNew.Location = ctrl.Location;//位置
                // nodeInfoNew.Size = ctrl.Size;//大小
                nodeInfoNew.SaveValue = ctrl.SaveValue;
                if (saveValue) nodeInfoNew.Text = ctrl.Text;//保存文本
                nodeInfos.Add(nodeInfoNew);
            }

            return nodeInfos;
        }

        /// <summary>
        /// 页面属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPageProperty_Click(object sender, EventArgs e)
        {
            PrintHelper.OpenUC(new UCPageProperty(Page), PrintInfo.PageProperty);
        }

        /// <summary>
        /// 删除选择
        /// </summary>
        public void DelSelectCtrl()
        {
            foreach (PrintCtrl ctrl in SelectedCtrls.ToArray())
            {
                this.Controls.Remove(ctrl);
            }
        }

        public Size GetInnerCtrlMaxSize()
        {
            int maxWidth = Width - CtrlMargin.Left - CtrlMargin.Right; //最大高度
            //修改：何晓明 2011-03-30
            //原因：增加纸张类型 控件最大尺寸
            //var sizeMax = PrintHelper.GetPaperSize(PaperSize.A4);
            var sizeMax = PrintHelper.GetPaperSize(Page.PaperSize);
            //
            int maxHeight = sizeMax.Height - Page.HeaderHeight - Page.FooterHeight - 5;
            return new Size(maxWidth, maxHeight);
        }
    }
}