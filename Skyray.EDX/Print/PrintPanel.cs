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
        #region ����

        private CtrlType _Type = CtrlType.None;
        public CtrlType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        /// <summary>
        /// ���������������Ŀؼ��߿򼯺�
        /// </summary>
        [Browsable(false)]
        public Dictionary<object, Rectangle> RectsToAdd { get; private set; }

        /// <summary>
        /// ѡ�е��ӿؼ��϶�ʱ���α߿򼯺�
        /// </summary>
        [Browsable(false)]
        public Dictionary<object, Rectangle> Rects { get; private set; }

        /// <summary>
        /// ��ǰѡ��Ŀؼ�
        /// </summary>

        [Browsable(false), DefaultValue(null)]
        public Control CurrentCtrl { get; set; }

        /// <summary>
        /// ��ҳ��ѡ�еĿؼ�
        /// </summary>
        [Browsable(false)]
        public List<PrintCtrl> SelectedCtrls { get; private set; }

        /// <summary>
        /// �ؼ��븸�������
        /// </summary>
        [Browsable(false), DefaultValue(typeof(Padding), "Padding.Empty")]
        public Padding CtrlMargin { get; set; }

        /// <summary>
        /// ҳ������
        /// </summary>
        [Browsable(false)]
        public PrintPage Page { get; set; }


        private bool _ShowHeaderCorner = true;
        /// <summary>
        /// �Ƿ���ʾҳüCorner
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
        /// �Ƿ���ʾҳ��Corner
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

        #region �������ԣ���¼�϶��ؼ�ʱ����ƫ����
        public int OffSetX { get { return _OffSetX; } }
        public int OffSetY { get { return _OffSetY; } }
        #endregion

        #endregion

        #region ˽���ֶ�
        private int _OffSetX = 0;//X��ƫ����
        private int _OffSetY = 0;//Y��ƫ����


        /// <summary>
        /// ��¼��ǰ���״̬
        /// </summary>
        private bool Dragging = false;

        /// <summary>
        /// ��ק��ʼ������
        /// </summary>
        private Point DragStart = Point.Empty;

        /// <summary>
        /// �ؼ������������
        /// </summary>
        private Rectangle CtrlRect = Rectangle.Empty;

        private Region CtrlRegion;

        /// <summary>
        /// �϶��켣����
        /// </summary>
        private Rectangle DragRect = Rectangle.Empty;

        /// <summary>
        /// //�Ƿ�������ק
        /// </summary>
        private bool TreeDragging = false;

        /// <summary>
        /// Ҫ������������Ŀؼ�����
        /// </summary>
        private List<PrintCtrl> LstCtrlToAdd = new List<PrintCtrl>();

        #endregion

        /// <summary>
        /// ���캯��
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
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            this.AllowDrop = true;//������ק
            BackColor = Color.White;//��ɫ����
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
        /// ���캯��
        /// </summary>
        /// <param name="page"></param>
        public PrintPanel(PrintPage page)
            : this()
        {
            Page = page;
        }

        /// <summary>
        /// ˢ���������
        /// </summary>
        public void InvalidateW()
        {
            Invalidate(CtrlRegion, false);
        }

        /// <summary>
        /// �����϶��߽�
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

        #region �¼�����

        /// <summary>
        /// �ͻ��������
        /// </summary>
        public void CreateCtrlRegion()
        {
            CtrlRect = new Rectangle(CtrlMargin.Left + 1, CtrlMargin.Top + 1,
                                     this.ClientSize.Width - CtrlMargin.Left - CtrlMargin.Right - 1,
                                     this.ClientSize.Height - CtrlMargin.Top - CtrlMargin.Bottom - 1);
            CtrlRegion = new Region(CtrlRect);
        }

        /// <summary>
        /// ��������ƶ��¼�
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.Page.CurrentPanel != this) this.Page.CurrentPanel = this; //ҳ�浱ǰPanel
            this.Page.MouseLocation = new Point(e.X, e.Y);//���λ��
            if (Dragging)
            {
                _OffSetX = e.X - DragStart.X;//X��ƫ����
                _OffSetY = e.Y - DragStart.Y;//Y��ƫ����
                DragRect = new Rectangle(DragStart.X, DragStart.Y, _OffSetX, _OffSetY);
                DragRect = PrintHelper.ConvertRect(DragRect);
                CalcRange();
                InvalidateW();
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// �������ſ��¼�
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
        /// ���ػ����¼�
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
        /// ������갴���¼�
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Page.SelectedPanel = this;
            // Page.Parent.Focus();//Tabpage��ý���
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
        /// �Ϸ�������
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
                    if (i != 0) { top += LstCtrlToAdd[i - 1].Height + 2; }//λ�����������
                    LstCtrlToAdd[i].Location = new Point(p.X, p.Y + top);//����λ�ã������
                    RectsToAdd[LstCtrlToAdd[i]] = LstCtrlToAdd[i].Bounds;
                }
            }
            else
                RectsToAdd.Clear();

            Invalidate();
        }

        /// <summary>
        /// ��TreeNodeInfoʵ������PrintCtrl
        /// </summary>
        /// <param name="infoSource">������Դ���ҵ��õ���TreeNodeInfo</param>
        /// <param name="templateInfo">ģ���д�ȡ��treenodeInfo</param>
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
                    c.TextVSpace = infoCurrent.TextVSpace;//�ı���ֱ���
                    //�����ʽ
                    c.TableStyle = Activator.CreateInstance(infoCurrent.TableStyleType) as ITableStyle;
                    c.ColHeight = infoCurrent.ColHeight;//�и߶�
                    c.RowHeight = infoCurrent.RowHeight;//�и߶�
                    c.ColInfos = infoCurrent.ColSetInfos;//����Ϣ����
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
            ctrl.NodeInfo = infoSource;//��¼����Դ

            ctrl.Name = infoSource.Type.ToString() + "." + infoSource.Name;
            ctrl.Text = bGetValueFromTemplateInfo ? templateInfo.Text : infoSource.Text;

            ctrl.TextFont = infoCurrent.TextFont;
            ctrl.TextColor = infoCurrent.TextColor;

            if (bGetValueFromTemplateInfo) ctrl.SaveValue = true;

            if (CurrentCtrl != null) CurrentCtrl.Invalidate();//ˢ��֮ǰ�Ŀؼ�
            CurrentCtrl = ctrl;//���������ӵĿؼ�Ϊ��ǰ�ؼ�

            // ctrl.Selected = true;//Ĭ��ѡ��
            ctrl.InitFinish = true;
            //�޸ģ������� 2011-03-22
            //ԭ�򣺱������Ϊ��ʱ�����ϳ�������
            if (ctrl != null && ctrl.Type == CtrlType.Grid && ((ctrl as PrintTable).Table == null || ((ctrl as PrintTable).Table != null && (ctrl as PrintTable).Table.Rows.Count == 0)))
                ctrl = null;
            //
            if (ctrl != null)
                listPt.Add(ctrl);
            return listPt;
        }

        /// <summary>
        /// ��TreeNodeInfoʵ������PrintCtrl
        /// </summary>
        /// <param name="infoSource">������Դ���ҵ��õ���TreeNodeInfo</param>
        /// <param name="templateInfo">ģ���д�ȡ��treenodeInfo</param>
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
                //�޸ģ������� 2011-03-22
                //ԭ�򣺱������Ϊ��ʱ����ʾ
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
                    c.TextVSpace = infoCurrent.TextVSpace;//�ı���ֱ���
                    //�����ʽ
                    c.TableStyle = Activator.CreateInstance(infoCurrent.TableStyleType) as ITableStyle;
                    c.ColHeight = infoCurrent.ColHeight;//�и߶�
                    c.RowHeight = infoCurrent.RowHeight;//�и߶�   
                    //�޸ģ������� 2011-01-18
                    //ԭ��Table�Զ����������п�
                    Size originalSize = templateInfo.Size;
                    int iNewWidth = 0;
                    bool flag = false;
                    try
                    {
                        DataColumnCollection ColumnsContents = c.Table.Columns;
                        foreach (DataColumn column in ColumnsContents)
                        {
                            //�޸ģ������� 2011-03-18
                            //ԭ�򣺱�����������Ű����
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
                        //�޸ģ������� 2011-03-18
                        //ԭ�򣺱�����������Ű����
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
                    c.ColInfos = infoCurrent.ColSetInfos;//����Ϣ����
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
            ctrl.NodeInfo = infoSource;//��¼����Դ

            ctrl.Name = infoSource.Type.ToString() + "." + infoSource.Name;
            ctrl.Text = bGetValueFromTemplateInfo ? templateInfo.Text : infoSource.Text;

            ctrl.TextFont = infoCurrent.TextFont;
            ctrl.TextColor = infoCurrent.TextColor;

            if (bGetValueFromTemplateInfo) ctrl.SaveValue = true;

            if (CurrentCtrl != null) CurrentCtrl.Invalidate();//ˢ��֮ǰ�Ŀؼ�
            CurrentCtrl = ctrl;//���������ӵĿؼ�Ϊ��ǰ�ؼ�

            // ctrl.Selected = true;//Ĭ��ѡ��
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
                //��ӿؼ�������
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
            SelectedCtrls.ForEach(ctrl => ctrl.Selected = false);//ȡ��֮ǰ��ѡ��
            LstCtrlToAdd.ForEach(ctrl =>
            {
                Controls.Add(ctrl);
                if (Param.AdjustCtrlPos) AdjustPos(ctrl);
                ctrl.Selected = true;
            });

            Page.PropertyObject = LstCtrlToAdd.Count == 1 ? LstCtrlToAdd[0] : null;//���Կؼ�

            LstCtrlToAdd.Clear();
            Invalidate();
            Focus();
            Page.SelectedPanel = this;
        }

        /// <summary>
        /// �����ؼ�λ��
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
        /// �������
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

        #region �Ҽ��˵��¼�

        /// <summary>
        /// ɾ����ѡ�ؼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelSelected_Click(object sender, EventArgs e)
        {
            DelSelectCtrl();
        }

        /// <summary>
        /// ɾ��ȫ���ؼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelAll_Click(object sender, EventArgs e)
        {
            SelectedCtrls.Clear();//���ѡ��ؼ��б�
            Controls.Clear();//��ҳ�����Ƴ����пؼ�           
        }
        /// <summary>
        /// ������Ϣ��Ӧ
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

        #region ҳüҳ��ֱ�ǻ���

        /// <summary>
        /// ����ҳüֱ��
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pnl"></param>
        private void DrawHeaderCorner(Graphics g, PrintPanel pnl)
        {
            g.DrawLine(Param.CornerPen,//��߽Ǻ���
                pnl.CtrlMargin.Left - 20, pnl.CtrlMargin.Top, pnl.CtrlMargin.Left, pnl.CtrlMargin.Top);
            g.DrawLine(Param.CornerPen, //��߽�����
                pnl.CtrlMargin.Left, pnl.CtrlMargin.Top - 20, pnl.CtrlMargin.Left, pnl.CtrlMargin.Top);
            g.DrawLine(Param.CornerPen,  //�ұ߽Ǻ���
                pnl.Width - pnl.CtrlMargin.Right + 20, pnl.CtrlMargin.Top, pnl.Width - pnl.CtrlMargin.Right, pnl.CtrlMargin.Top);
            g.DrawLine(Param.CornerPen, //�ұ߽�����
                pnl.Width - pnl.CtrlMargin.Right, pnl.CtrlMargin.Top - 20, pnl.Width - pnl.CtrlMargin.Right, pnl.CtrlMargin.Top);
        }

        /// <summary>
        /// ����ҳ��ֱ��
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pnl"></param>
        private void DrawFooterCorner(Graphics g, PrintPanel pnl)
        {
            int LeftCenterX = pnl.CtrlMargin.Left;
            int CenterY = pnl.Height - pnl.CtrlMargin.Bottom;
            int RightCenterX = pnl.Width - pnl.CtrlMargin.Right;

            g.DrawLine(Param.CornerPen, LeftCenterX, CenterY, LeftCenterX - 20, CenterY); //��߽Ǻ��� 
            g.DrawLine(Param.CornerPen, LeftCenterX, CenterY + 20, LeftCenterX, CenterY);  //��߽�����
            g.DrawLine(Param.CornerPen, RightCenterX + 20, CenterY, RightCenterX, CenterY);//�ұ߽Ǻ���
            g.DrawLine(Param.CornerPen, RightCenterX, CenterY + 20, RightCenterX, CenterY);//�ұ߽�����
        }

        #endregion

        /// <summary>
        /// ҳ��TreeNode����
        /// </summary>
        public List<TreeNodeInfo> NodeInfos
        {
            get { return GetNodeInfosToSave(); }
        }

        /// <summary>
        /// ��ȡTreeNodeInfo���󼯺ϣ����ڿͻ�����ģ��
        /// </summary>
        /// <returns></returns>
        private List<TreeNodeInfo> GetNodeInfosToSave()
        {
            bool IsBody = this.Type == CtrlType.Body;//�Ƿ�ΪBody
            var nodeInfos = new List<TreeNodeInfo>();

            foreach (PrintCtrl ctrl in Controls)
            {
                var typ = ctrl.Type;
                if (!IsBody && typ == CtrlType.Grid) continue;//����ܴ�����ҳü����ҳβ
                var nodeInfoNew = new TreeNodeInfo();
                nodeInfoNew.Name = ctrl.NodeInfo.Name;//���� 
                nodeInfoNew.Size = ctrl.Size;

                //bool saveValue = !IsBody && ctrl.SaveValue;//�Ƿ���Ҫ����ֵ
                bool saveValue =  ctrl.SaveValue;
                if (typ == CtrlType.Field)
                {
                    var c = ctrl as PrintField;
                    nodeInfoNew.TextValueColor = c.TextValueColor;//��ɫ
                    nodeInfoNew.TextValueFont = c.TextValueFont;
                    if (saveValue) nodeInfoNew.TextValue = c.TextValue;
                    nodeInfoNew.Size = c.Size;
                }
                else if (typ == CtrlType.Grid)
                {
                    var c = ctrl as PrintTable;

                    nodeInfoNew.TextVSpace = c.TextVSpace;//��ֱ���
                    nodeInfoNew.TableStyleType = c.TableStyle.GetType();//�����ʽ
                    nodeInfoNew.ColHeight = c.ColHeight;//�п��
                    //�п��
                    nodeInfoNew.RowHeight = c.RowHeight;//�߶�
                    nodeInfoNew.Size = c.Size;
                    nodeInfoNew.ColSetInfos = c.NodeInfo.TableAutoSize ? null : c.ColInfos;//���������Ϣ
                }
                else if (typ == CtrlType.Image)
                {
                    var c = ctrl as PrintImage;
                    nodeInfoNew.TextVSpace = c.TextVSpace;//��ֱ���
                    nodeInfoNew.ShowPicBorder = c.DrawImageBorder;
                    nodeInfoNew.PicBorderColor = c.ImageBorderColor;
                    nodeInfoNew.Size = c.Size;
                    if (saveValue) nodeInfoNew.Image = c.Image;
                }
                else if (typ == CtrlType.ComposeTable)
                {
                    var c = ctrl as PrintCompositeTable;
                    nodeInfoNew.TextVSpace = c.TextVSpace;//��ֱ���
                    //nodeInfoNew.ShowPicBorder = c.DrawImageBorder;
                    //nodeInfoNew.PicBorderColor = c.ImageBorderColor;
                    nodeInfoNew.Size = c.Size;
                    nodeInfoNew.CellContext = c.CellInfo;
                    //if (saveValue) nodeInfoNew.Image = c.Image;
                }

                nodeInfoNew.Type = ctrl.Type;//�ؼ�����           
                nodeInfoNew.TextFont = ctrl.TextFont;//����
                nodeInfoNew.TextColor = ctrl.TextColor;//������ɫ
                nodeInfoNew.Location = ctrl.Location;//λ��
                // nodeInfoNew.Size = ctrl.Size;//��С
                nodeInfoNew.SaveValue = ctrl.SaveValue;
                if (saveValue) nodeInfoNew.Text = ctrl.Text;//�����ı�
                nodeInfos.Add(nodeInfoNew);
            }

            return nodeInfos;
        }

        /// <summary>
        /// ҳ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPageProperty_Click(object sender, EventArgs e)
        {
            PrintHelper.OpenUC(new UCPageProperty(Page), PrintInfo.PageProperty);
        }

        /// <summary>
        /// ɾ��ѡ��
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
            int maxWidth = Width - CtrlMargin.Left - CtrlMargin.Right; //���߶�
            //�޸ģ������� 2011-03-30
            //ԭ������ֽ������ �ؼ����ߴ�
            //var sizeMax = PrintHelper.GetPaperSize(PaperSize.A4);
            var sizeMax = PrintHelper.GetPaperSize(Page.PaperSize);
            //
            int maxHeight = sizeMax.Height - Page.HeaderHeight - Page.FooterHeight - 5;
            return new Size(maxWidth, maxHeight);
        }
    }
}