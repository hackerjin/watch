using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Skyray.Controls.Tree;
using System.Threading;
using Skyray.Print.Properties;
using System.IO;
using Skyray.Controls;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using Skyray.EDX.Common;
using Skyray.EDX.Common.ReportHelper;


namespace Skyray.Print
{
    public partial class UCPrint : Skyray.Language.UCMultiple
    {
        #region 公开属性
        public string curreEdition = "";//当前软件版本

        //修改：朱庆华 2010-03-14
        //原因：获取控件更改模板路径后的值
        public delegate void NotifyPathChange( string strPath);
        public event NotifyPathChange OnNotifyPathChange;
        //修改：何晓明 2011-03-02
        //原因：获取当前模板名称
        /// <summary>
        /// 当前打印模板名称
        /// </summary>
        private string _CurrentTemplateName = string.Empty;
        public string CurrentTemplateName
        {
            get { return _CurrentTemplateName; }
            set { _CurrentTemplateName = value;
            if (OnNotifyPathChange != null)
                OnNotifyPathChange(value);
                if(this.Parent!=null)
            ( this.Parent as System.Windows.Forms.Form).Text = value.Substring(Math.Max(value.LastIndexOf("\\" )+1,0));
            }
        }

        //
        /// <summary>
        /// 获取页面预览图片
        /// </summary>
        /// <returns></returns>
        public List<Bitmap> GetBitmaps()
        {
            return new ImageExporter(page).LstBitmap;
        }

        /// <summary>
        /// 属性面板
        /// </summary>
        public UCProperty PropertyPanel
        {
            get { return this.ucProperty; }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public List<TreeNodeInfo> DataSource { get; set; }

        /// <summary>
        /// 是否显示属性框
        /// </summary>
        public bool ShowPropertyPanel
        {
            get { return pnlProperty.Visible; }
            set
            {
                pnlProperty.Visible = value;
            }
        }

        /// <summary>
        /// 是否显示左侧数据源
        /// </summary>
        public bool ShowSourcePanel
        {
            get { return pnlSource.Visible; }
            set
            {
                pnlSource.Visible = value;
            }
        }

        /// <summary>
        /// 是否显示状态栏
        /// </summary>
        public bool ShowStatusBar
        {
            get { return statusStripWStatus.Visible; }
            set
            {
                statusStripWStatus.Visible = value;
            }
        }

        /// <summary>
        /// 是否显示菜单栏
        /// </summary>
        public bool ShowMenuBar
        {
            get { return menuStripW1.Visible; }
            set
            {
                menuStripW1.Visible = value;
            }
        }

        /// <summary>
        /// 是否显示工具栏
        /// </summary>
        public bool ShowToolBar
        {
            get { return toolStripW1.Visible; }
            set
            {
                toolStripW1.Visible = value;
            }
        }


        public PrintPage Page
        {
            get { return page; }
        }
        #endregion

        #region 公开方法

        /// <summary>
        /// 导出至PDF文档
        /// </summary>
        public void ExportToPDF()
        {
            previewControl1.ExportToPdf();
        }

        /// <summary>
        /// 清除属性面板选择
        /// </summary>
        public void ClearProperty()
        {
            this.ucProperty.PropertyObject = null;
        }

        /// <summary>
        /// 打开模板
        /// </summary>
        /// <param name="source"></param>
        public void OpenTemplate(TemplateSource source)
        {
            OpenTemplate(source, OpenTemplateMode.Open);
        }

        /// <summary>
        /// 打开模板
        /// </summary>
        public void OpenTemplate(TemplateSource source, OpenTemplateMode openMode)
        {
            if (source != null)
            {
                SourceToPage(source);//模板至Page对象
                //if (ShowPreviewPage)
                //{
                //    RefreshTabPagePreview();
                //}
                switch (openMode)
                {
                    case OpenTemplateMode.OpenAndPreview:
                        previewControl1.PrintPreview();
                        break;
                    case OpenTemplateMode.OpenAndPrint:
                        previewControl1.Print();
                        break;
                }
            }
        }

        /// <summary>
        /// 获取页面数据源
        /// </summary>
        /// <returns></returns>
        public TemplateSource GetTemplateSource()
        {
            return page.GetTemplateSource();
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="templateFile"></param>
        public void PrintPreview(TemplateSource source)
        {
            OpenTemplate(source, OpenTemplateMode.OpenAndPreview);
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="templateFile"></param>
        public void PrintPreview(string templateFile)
        {
            OpenTemplate(PrintHelper.FileToObj<TemplateSource>(templateFile),
                OpenTemplateMode.OpenAndPreview);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="templateFile"></param>
        public void Print(TemplateSource source)
        {
            OpenTemplate(source, OpenTemplateMode.OpenAndPrint);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="templateFile"></param>
        public void Print(string templateFile)
        {
            OpenTemplate(PrintHelper.FileToObj<TemplateSource>(templateFile),
                OpenTemplateMode.OpenAndPrint);
        }



        #endregion

        #region 公开事件
        public event EventHandler ScrollerScroll;
        public event EventHandler SaveTemplateEvent;
        #endregion

        #region 私有变量
        /// <summary>
        /// 打印预览对象
        /// </summary>
        public PreviewControl previewControl1;


        //public Object RefectionObj;

        //public DataTable dataTable;
        #endregion

        #region 构造器

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCPrint()
        {
            //Set Style
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.UserPaint
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.ResizeRedraw, true);

            InitializeComponent();
            Init();//Init

            SetLocalization();

            page.UCPrint = this;

            //ShowPreviewPage = false;
            
            this.page.SendToBack();
        }

        /// <summary>
        /// Set Locallization information
        /// </summary>
        private void SetLocalization()
        {
            //this.tabPagePreview.Text = PrintInfo.PagePriview;
            //this.tabPageDesign.Text = PrintInfo.DesignPage;
        }
        private void SetVScrollBar()
        {
            int h = pnlPage.Height - hScrollBar1.Height - 5 - 5;//最多显示的高度
            //修改：何晓明 2011-02-22
            //原因：增加打印机及打印纸张
            //int largeChange = page.Height - h;
            int largeChange = Math.Max(page.Height - h,0);
            //
            vScrollBar1.Maximum = largeChange * (4 + (int)page.PageSize);
            vScrollBar1.LargeChange = largeChange;
            page.Top = 5;
            vScrollBar1.Value = 0;
        }
        private void SetHScrollBar()
        {
            int w = pnlPage.Width - vScrollBar1.Width - 1 - 1;
            int largeChange = page.Width - w;
            hScrollBar1.Maximum = largeChange * 2;
            hScrollBar1.LargeChange = largeChange;
            page.Left = GetPageLeftPoint();
            hScrollBar1.Value = 0;
        }
        private int GetMaxHeight()
        {
            return pnlPage.Height - 5 - 5 - (hScrollBar1.Visible ? hScrollBar1.Height : 0);
        }
        private int GetMaxWidth()
        {
            return pnlPage.Width - 1 - 1 - (this.vScrollBar1.Visible ? vScrollBar1.Width : 0);
        }
        private bool IsHScrollBarVisable()
        {
            return page.Width > GetMaxWidth();
        }
        private bool IsVScrollBarVisable()
        {
            return page.Height > GetMaxHeight();
        }
        private int GetPageLeftPoint()
        {
            return Math.Max(0, (pnlPage.Width - (vScrollBar1.Visible ? vScrollBar1.Width : 0) - page.Width) / 2);
        }
        /// <summary>
        /// 页面初始化函数
        /// </summary>
        private void Init()
        {
            previewControl1 = new PreviewControl(page);

            this.pnlPage.SizeChanged += (sender, e) =>
            {
                page.Left = GetPageLeftPoint();
                vScrollBar1.Visible = IsVScrollBarVisable();//是否显示竖向滚动条           
                if (vScrollBar1.Visible) SetVScrollBar();

                hScrollBar1.Visible = IsHScrollBarVisable();
                if (hScrollBar1.Visible) SetHScrollBar();
            };

            this.page.SizeChanged += (sender, e) => { SetVScrollBar(); };

            this.page.OnSelectCtrlChanged += delegate(object sender, EventArgs e)
            {
                ucProperty.PropertyObject = sender;//控件属性
            };
            //状态栏实时显示鼠标所在区域，及其大小和鼠标坐标          
            this.page.OnMouseLocationChanged += delegate(object sender, EventArgs e)
            {
                this.tssMousePoint.Text = this.page.MouseLocation.ToString();
                this.tssArea.Text = this.page.CurrentPanel.Type.ToString();
                this.tssAreaSize.Text = this.page.CurrentPanel.Size.ToString();
                //修改：何晓明 2011-02-28
                //原因：增加纸张显示
                this.tssPapperSizeValue.Text = this.page.strPaperSize;
                //
            };

            page.SetPrintPageParam();
        }

        #endregion

        #region 页面加载事件，线程加载左侧数据源树形
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCPrint_Load(object sender, EventArgs e)
        {
            InitTree();
        }

        /// <summary>
        /// 初始化tree
        /// </summary>
        public void InitTree()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                BeginInvoke((MethodInvoker)delegate
                {

                    //Application.DoEvents();
                    //pnlSource.Text = PrintInfo.DataLoading;//数据加载中..
                    treeSource.SelectionMode = TreeSelectionMode.MultiSameParent;
                    FrmTreeNodeInfo frm = new FrmTreeNodeInfo();
                    frm.LoadLocalDataSource();
                    if (frm.lstTreeNodeInfos != null)
                    //DataSource.AddRange(frm.lstTreeNodeInfos);
                    {
                        foreach (TreeNodeInfo tni in frm.lstTreeNodeInfos)
                        {
                            if(DataSource.Find(w=>w.Name==tni.Name&&w.Type==tni.Type)==null)
                            {
                                DataSource.Add(tni);
                            }
                        }
                    }
                    treeSource.ReportDataSource = DataSource;                    
                    treeSource.ExpandAll();
                    treeSource.ItemDrag -= new ItemDragEventHandler(treeSource_ItemDrag);
                    treeSource.ItemDrag += new ItemDragEventHandler(treeSource_ItemDrag);
                    pnlSource.Text = PrintInfo.Source;//数据源
                });
            });
        }

        void treeSource_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var info1 = (treeSource.SelectedNode.Tag as Node).Tag as TreeNodeInfo;
            //if (info1 != null && info1.Type != CtrlType.None)
                treeSource.DoDragDropSelectedNodes(DragDropEffects.Move);
        }
        #endregion

        #region 菜单栏事件

        /// <summary>
        /// 保存模板事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiSaveTreeSource_Click(object sender, EventArgs e)
        {
            SaveTemplate();
        }

        /// <summary>
        /// 是否显示表头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiShowHeader_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            tsmiShowHeader.Checked = ctsmiShowHeader.Checked = page.ShowHeader = item.Checked;

        }

        /// <summary>
        /// 是否显示页脚
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiShowFooter_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            tsmiShowFooter.Checked = ctsmiShowFooter.Checked = page.ShowFooter = item.Checked;
        }

        /// <summary>
        ///打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPrint_Click(object sender, EventArgs e)
        {
            previewControl1.Print();
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPriview_Click(object sender, EventArgs e)
        {
            previewControl1.PrintPreview();
        }

        /// <summary>
        /// 打开模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiOpenTemplate_Click(object sender, EventArgs e)
        {
            //修改：何晓明 2010-12-28
            //原因：打开模板时偶有异常
            //需求：陈春花
            try
            {
                OpenTemplate(SelectTemplate());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //OpenTemplate(SelectTemplate());

        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAbout_Click(object sender, EventArgs e)
        {

            SkyrayMsgBox.Show(PrintInfo.SystemVersion);
        }

        /// <summary>
        /// 导出至模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiExportToPDF_Click(object sender, EventArgs e)
        {
            ExportToPDF();
        }

        #endregion

        #region 键盘消息处理
        /// <summary>
        /// 键盘消息处理
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (page.SelectedPanel != null)
                page.SelectedPanel.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 从数据源中查找节点信息
        /// </summary>
        /// <param name="nodeInfo"></param>
        /// <returns></returns>
        private TreeNodeInfo GetTreeNodeInfo(TreeNodeInfo nodeInfo)
        {
            if (DataSource == null) return null;
            return DataSource.Find(info => info.Name == nodeInfo.Name && info.Type == nodeInfo.Type);
        }
        /// <summary>
        /// 获取模板文件夹
        /// </summary>
        /// <returns></returns>
        private DirectoryInfo GetTemplateFolder()
        {
            DirectoryInfo info = new DirectoryInfo(Application.StartupPath + "\\PrintTemplate\\");
            if (!info.Exists) info.Create();
            return info;
        }
        /// <summary>
        /// 从Source对象给Page对象赋值
        /// </summary>
        /// <param name="source"></param>
        private void SourceToPage(TemplateSource source)
        {
            page.Header.Controls.Clear();//Header控件清除
            page.Footer.Controls.Clear();//Footer控件清除
            page.Body.Controls.Clear();//Body控件清除

            page.PaperSize = source.PaperSize;//PaperSize
            page.PrintMargin = new Padding(source.PageMargin.Left,
                                           source.PageMargin.Top,
                                           source.PageMargin.Right,
                                           source.PageMargin.Bottom);
            page.Dir = source.Dir;//dir
            page.PageSize = source.PageSize;//PageSize
            //修改：何晓明 2011-02-22
            //原因：增加打印机及打印纸张
            page.strPaperSize = source.strPaperSize==null?"A4":source.strPaperSize;
            //
            page.SetHeadSplitterPos = source.HeadSplitterPosition;
            page.SetFootSplitterPos = source.BottomSplitterPosition;
            tsmiShowHeader.Checked = page.ShowHeader = source.ShowHeader;//ShowHeader
            tsmiShowFooter.Checked = page.ShowFooter = source.ShowFooter;//ShowFooter

            page.SetPrintPageParam();//设置参数

            PrintCtrl printCtrl = null;//打印控件
            PrintPanel printPanel = null;//记录当前PrintPanel控件
            foreach (var pnl in source.PrintPanels)
            {
                if (pnl.PanelType == CtrlType.Header)
                    printPanel = page.Header;
                else if (pnl.PanelType == CtrlType.Footer)
                    printPanel = page.Footer;
                else
                    printPanel = page.Body;

                foreach (var nodeinfo in pnl.NodeInfos)
                {
                    var nodeInfoFromSource = GetTreeNodeInfo(nodeinfo);
                    if (nodeInfoFromSource == null) continue;

                    printCtrl = printPanel.CreatePrintCtrl(nodeInfoFromSource, nodeinfo);

                    if (printCtrl == null)
                        continue;
                    
                    printCtrl.Location = nodeinfo.Location;
                    printCtrl.Size = nodeinfo.Size;
                    //修改：何晓明 2011-03-18
                    //原因：表格因隐藏列排版错误 表格列分行打开模板时无效
                    if (printCtrl.Type == CtrlType.Grid)
                        printCtrl.SetSize();
                    //
                    int w = printCtrl.Width;
                    int left = printCtrl.Left;
                    printPanel.AdjustPos(printCtrl);//模版元素位置矫正
                    printPanel.Controls.Add(printCtrl);
                }
            }
        }
        /// <summary>
        /// 选择模板控件
        /// </summary>
        /// <returns></returns>
        public TemplateSource SelectTemplate()
        {
            string seleTemplate = ReportTemplateHelper.LoadSpecifiedValue("Report", "ReportName");
            var diag = new OpenFileDialog();
            diag.InitialDirectory = GetTemplateFolder().FullName;
            diag.Filter = PrintInfo.TemplateFile + "(*" + seleTemplate + ")|*"+seleTemplate ;
            //diag.Filter = PrintInfo.TemplateFile + "(*.template)|*.template";
            diag.RestoreDirectory = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                //修改：何晓明 2011-03-02
                //原因：获取当前模板名称
                this.CurrentTemplateName = diag.FileName;
                //
                return PrintHelper.FileToObj<TemplateSource>(diag.FileName);
            }
            return null;
        }
        /// <summary>
        /// 保存模板
        /// </summary>
        private void SaveTemplate()
        {
            try
            {
                DirectoryInfo info = GetTemplateFolder();
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.InitialDirectory = info.FullName;
                saveDlg.RestoreDirectory = true;
                saveDlg.Filter = "模板文件(*.template)|*.template";
                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    PrintHelper.curreEdition = curreEdition;
                    PrintHelper.ObjToFile(page.GetTemplateSource(), saveDlg.FileName);
                    if (SaveTemplateEvent != null)
                    {
                        SaveTemplateEvent(null, null);
                    }
                    //修改：何晓明 2011-03-02
                    //原因：获取当前模板名称
                    this.CurrentTemplateName = saveDlg.FileName;

                    //
                }
            }
            catch (Exception ex) { SkyrayMsgBox.Show(ex.Message); }//异常处理
        }

        #endregion

        private void tabControlMain_Selected(object sender, TabControlEventArgs e)
        {
            //if (e.TabPage == tabPagePreview)
            {
                RefreshTabPagePreview();
            }
        }

        private void RefreshTabPagePreview()
        {
            //tabPagePreview.Controls.Clear();
            //var paperSize = PrintHelper.GetPaperSize(page.PaperSize);
            //if (page.Dir == PrintDirection.Vertical)
            //{
            //    paperSize = new Size(paperSize.Height, paperSize.Width);
            //}
            //var lstPage = previewControl1.GetPages();
            //if (lstPage == null || lstPage.Count == 0) return;

            //Point pt = page.Location;
            //foreach (var p in lstPage)
            //{
            //    p.Location = pt;
            //    pt = new Point(pt.X, pt.Y + paperSize.Height + 20);
            //    tabPagePreview.Controls.Add(p);
            //}
        }

        #region Page右键菜单
        /// <summary>
        /// 页面属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPageProperty_Click(object sender, EventArgs e)
        {
            var test = new UCPageProperty(page);
            Skyray.XmlLang.UIHelper.LanguageHandler(test);
            PrintHelper.OpenUC(test, PrintInfo.PageProperty);
            //Skyray.Language.Xml.ObjCache.lstPtr.Add(test.Handle);
            //Skyray.Language.Xml.UIHelper.OpenModelDlg(null);
        }

        /// <summary>
        /// 删除选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelSelect_Click(object sender, EventArgs e)
        {
            foreach (PrintCtrl ctrl in page.SelectedPanel.SelectedCtrls.ToArray())
            {
                page.SelectedPanel.Controls.Remove(ctrl);
            }
        }
        /// <summary>
        /// 删除全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelAll_Click(object sender, EventArgs e)
        {
            page.SelectedPanel.Controls.Clear();
        }

        #endregion

        #region Ctrl右键菜单
        #region 右键菜单

        private void tsmiAlignLeft_Click(object sender, EventArgs e)
        {
            DoCmd(Cmd.AlignLeft);
        }

        private void tsmiAlignRight_Click(object sender, EventArgs e)
        {
            DoCmd(Cmd.AlignRight);
        }

        private void tsmiAlignTop_Click(object sender, EventArgs e)
        {
            DoCmd(Cmd.AlignTop);
        }

        private void tsmiAlignBottom_Click(object sender, EventArgs e)
        {
            DoCmd(Cmd.AlignBottom);
        }

        private void tsmiSameWidth_Click(object sender, EventArgs e)
        {
            DoCmd(Cmd.SameWidth);
        }

        private void tsmiSameHeight_Click(object sender, EventArgs e)
        {
            DoCmd(Cmd.SameHeight);
        }

        private void tsmiSameSize_Click(object sender, EventArgs e)
        {
            DoCmd(Cmd.SameSize);
        }

        /// <summary>
        /// 值对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAlignLabel_Click(object sender, EventArgs e)
        {
            var fields = page.SelectedPanel.SelectedCtrls.OfType<PrintField>();
            var x = fields.Max(ctrl => ctrl.ValueTextStartX);
            foreach (var ctrl in fields) ctrl.Width += x - ctrl.ValueTextStartX;
        }

        private void tsmiRemoveVSpace_Click(object sender, EventArgs e)
        {
            var sortedCtrls = (from ctrl in page.SelectedPanel.SelectedCtrls
                               orderby ctrl.Top //根据控件的顶坐标Y排列
                               select ctrl).ToArray();
            for (int i = 1; i < sortedCtrls.Length; i++)
                sortedCtrls[i].Top = sortedCtrls[i - 1].Bottom + 1; //垂直堆叠
        }

        private void tsmiRemoveHSpace_Click(object sender, EventArgs e)
        {
            var sortedCtrls = (from ctrl in page.SelectedPanel.SelectedCtrls
                               orderby ctrl.Left //根据控件的顶坐标X排列
                               select ctrl).ToArray();
            for (int i = 1; i < sortedCtrls.Length; i++)
                sortedCtrls[i].Left = sortedCtrls[i - 1].Right + 1;//水平堆叠
        }

        private void tsmiBringToFront_Click(object sender, EventArgs e)
        {
            foreach (var ctrl in page.SelectedPanel.SelectedCtrls.ToArray())
                ctrl.BringToFront();
        }

        private void tsmiSendToBack_Click(object sender, EventArgs e)
        {
            foreach (var ctrl in page.SelectedPanel.SelectedCtrls.ToArray())
                ctrl.SendToBack();
        }

        private void tsmiDelSelected_Click(object sender, EventArgs e)
        {
            page.SelectedPanel.DelSelectCtrl();
        }

        private void DoCmd(Cmd cmd)
        {
            page.SelectedPanel.SelectedCtrls.ForEach(ctrl =>
            {
                if (ctrl != page.SelectedPanel.CurrentCtrl)
                {
                    switch (cmd)
                    {
                        case Cmd.AlignLeft:
                            ctrl.Left = page.SelectedPanel.CurrentCtrl.Left;
                            break;
                        case Cmd.AlignRight:
                            ctrl.Left += page.SelectedPanel.CurrentCtrl.Right - ctrl.Right;
                            break;
                        case Cmd.AlignTop:
                            ctrl.Top = page.SelectedPanel.CurrentCtrl.Top;
                            break;
                        case Cmd.AlignBottom:
                            ctrl.Top += page.SelectedPanel.CurrentCtrl.Bottom - ctrl.Bottom;
                            break;
                        case Cmd.SameWidth:
                            ctrl.Width = page.SelectedPanel.CurrentCtrl.Width;
                            break;
                        case Cmd.SameHeight:
                            ctrl.Height = page.SelectedPanel.CurrentCtrl.Height;
                            break;
                        case Cmd.SameSize:
                            ctrl.Size = page.SelectedPanel.CurrentCtrl.Size;
                            break;
                        default: break;
                    }
                }
                if (Param.AdjustCtrlPos) page.SelectedPanel.AdjustPos(ctrl);
            });
        }

        #endregion

        #endregion

        public void ShowCmsPage(Point p)
        {
            tsmiDelAll.Visible = page.SelectedPanel.Controls.Count > 0;
            tsmiDelSelect.Visible = page.SelectedPanel.SelectedCtrls.Count > 0;
            tss2.Visible = tsmiDelAll.Visible || tsmiDelSelect.Visible;
            cmsPage.Show(p);
        }

        public void ShowCmsCtrl(Point p)
        {
            if (page.SelectedPanel == null) return;
            tsmiDelSelected.Visible = page.SelectedPanel.SelectedCtrls.Count > 0;
            tsmiSpace.Visible = tsmiSize.Visible =
                tsmiAlign.Visible = page.SelectedPanel.SelectedCtrls.Count > 1;
            tsmiAlignLabel.Visible =
                page.SelectedPanel.SelectedCtrls.TrueForAll(
                ctrl => ctrl.Type == CtrlType.Field);
            //修改：何晓明 2011-01-14
            //原因：增加页眉页脚居中对齐
            //需求：裴志祥
            //tsmiAlignCenter.Visible = (page.SelectedPanel.Type == CtrlType.Header ||
            //                           page.SelectedPanel.Type == CtrlType.Footer) &&
            //                           page.SelectedPanel.SelectedCtrls.Count > 0;
            tsmiAlignCenter.Visible = page.SelectedPanel.SelectedCtrls.Count > 0;
            //
            cmsCtrl.Show(p);
        }

        private void tsmiExportToExcel_Click(object sender, EventArgs e)
        {
            previewControl1.ExportToExcel();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            var d = e.NewValue / (vScrollBar1.Maximum - vScrollBar1.LargeChange + 1d);
            //Console.WriteLine("Value:" + e.NewValue + "Max:" + vScrollBar1.Maximum + " SmallChange:" + vScrollBar1.SmallChange + " Large:" + vScrollBar1.LargeChange);
            int h = pnlPage.Height - (hScrollBar1.Visible ? hScrollBar1.Height : 0) - 5 - 5;//最多显示的高度
            var offSetY = d * (page.Height - h);
            page.Top = 5 - offSetY.GetNearInt();

            if (this.ScrollerScroll != null) ScrollerScroll(null, null);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            var d = e.NewValue / (hScrollBar1.Maximum - hScrollBar1.LargeChange + 1d);
            //Console.WriteLine("Value:" + e.NewValue + "Max:" + vScrollBar1.Maximum + " SmallChange:" + vScrollBar1.SmallChange + " Large:" + vScrollBar1.LargeChange);
            int h = pnlPage.Width - (vScrollBar1.Visible ? vScrollBar1.Width : 0) - 1 - 1;//最多显示的高度
            var offSetX = d * (page.Width - h);
            page.Left = 1 - offSetX.GetNearInt();
        }

        private void tlsInsertT_Click(object sender, EventArgs e)
        {
            //var test = new UCTableSetting();
            //Skyray.XmlLang.UIHelper.LanguageHandler(test);
            //PrintHelper.OpenUC(test, PrintInfo.InsertTable);
            //if (test.returnOk == DialogResult.Yes)
            //{
            //    PrintCompositeTable table = new PrintCompositeTable(this.page.Body, test.RowCount, test.ColCount);

            //}
        }
        //修改：何晓明 2011-01-14
        //原因：增加页眉页脚居中对齐
        //需求：裴志祥
        private void tsmiAlignCenter_Click(object sender, EventArgs e)
        {
            List<PrintCtrl> ctrls = page.SelectedPanel.SelectedCtrls;
            int iMinLeft = page.Width;
            int iMaxRight = 0;
            int iPageCenter = 0;
            int iCtrlsWidth = 0;
            int iOffsetX = 0;
            foreach (PrintCtrl ctrl in ctrls)
            {
                iMinLeft = iMinLeft <= ctrl.Left ? iMinLeft : ctrl.Left;
                iMaxRight = iMaxRight >= ctrl.Right ? iMaxRight : ctrl.Right;
            }
            iCtrlsWidth = iMaxRight - iMinLeft;
            iPageCenter = (page.Width - page.Margin.Left - page.Margin.Right)/2;
            iOffsetX = iPageCenter - iCtrlsWidth / 2 - iMinLeft;
            foreach ( PrintCtrl ctrl in ctrls )
            {
                ctrl.Left = ctrl.Left + iOffsetX;
            }
        }
        //
    }
}