using Skyray.Controls;
using System.Windows.Forms;
using System.Drawing;
using System;
namespace Skyray.Print
{
    partial class UCPrint
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlClient = new System.Windows.Forms.Panel();
            this.pnlPage = new System.Windows.Forms.Panel();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.page = new Skyray.Print.PrintPage();
            this.pnlProperty = new Skyray.Controls.XPander.Panel();
            this.ucProperty = new Skyray.Print.UCProperty();
            this.mtSplitter1 = new Skyray.Controls.MTSplitter();
            this.pnlSource = new Skyray.Controls.XPander.Panel();
            this.treeSource = new Skyray.Print.TreeViewAdvEx();
            this.toolStripW1 = new Skyray.Controls.ToolStripW();
            this.statusStripWStatus = new Skyray.Controls.StatusStripW();
            this.tssPageAreaText = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssArea = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssPapperSizeText = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssPapperSizeValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssPageSizeText = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssAreaSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssMousePointText = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssMousePoint = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStripW1 = new Skyray.Controls.MenuStripW();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveTreeSource = new System.Windows.Forms.ToolStripMenuItem();
            this.报表导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportToPDF = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPriview = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTable = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsInsertT = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowHeader = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowFooter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsPage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctsmiShowHeader = new System.Windows.Forms.ToolStripMenuItem();
            this.ctsmiShowFooter = new System.Windows.Forms.ToolStripMenuItem();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDelSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tss2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPageProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCtrl = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDelSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlign = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlignLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlignRight = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlignTop = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlignBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlignLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSize = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSameWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSameHeight = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSameSize = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSpace = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRemoveVSpace = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRemoveHSpace = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBringToFront = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSendToBack = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlignCenter = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlClient.SuspendLayout();
            this.pnlPage.SuspendLayout();
            this.pnlProperty.SuspendLayout();
            this.pnlSource.SuspendLayout();
            this.statusStripWStatus.SuspendLayout();
            this.menuStripW1.SuspendLayout();
            this.cmsPage.SuspendLayout();
            this.cmsCtrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlClient
            // 
            this.pnlClient.BackColor = System.Drawing.Color.Lavender;
            this.pnlClient.Controls.Add(this.pnlPage);
            this.pnlClient.Controls.Add(this.pnlProperty);
            this.pnlClient.Controls.Add(this.mtSplitter1);
            this.pnlClient.Controls.Add(this.pnlSource);
            this.pnlClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlClient.Location = new System.Drawing.Point(0, 24);
            this.pnlClient.Name = "pnlClient";
            this.pnlClient.Padding = new System.Windows.Forms.Padding(2);
            this.pnlClient.Size = new System.Drawing.Size(1012, 658);
            this.pnlClient.TabIndex = 3;
            // 
            // pnlPage
            // 
            this.pnlPage.Controls.Add(this.vScrollBar1);
            this.pnlPage.Controls.Add(this.hScrollBar1);
            this.pnlPage.Controls.Add(this.page);
            this.pnlPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPage.Location = new System.Drawing.Point(174, 2);
            this.pnlPage.Name = "pnlPage";
            this.pnlPage.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.pnlPage.Size = new System.Drawing.Size(597, 654);
            this.pnlPage.TabIndex = 6;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.LargeChange = 5;
            this.vScrollBar1.Location = new System.Drawing.Point(577, 3);
            this.vScrollBar1.Maximum = 10;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(18, 633);
            this.vScrollBar1.TabIndex = 8;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(6, 634);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(569, 18);
            this.hScrollBar1.TabIndex = 9;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // page
            // 
            this.page.BackColor = System.Drawing.Color.LightGray;
            this.page.Dir = Skyray.Print.PrintDirection.Horizontal;
            this.page.Location = new System.Drawing.Point(6, 3);
            this.page.MouseLocation = new System.Drawing.Point(0, 0);
            this.page.Name = "page";
            this.page.Padding = new System.Windows.Forms.Padding(1, 1, 3, 3);
            this.page.PageSize = Skyray.Print.PageSize.Smallest;
            this.page.PaperSize = Skyray.Print.PaperSize.A4;
            this.page.PrintMargin = new System.Windows.Forms.Padding(50);
            this.page.Size = new System.Drawing.Size(388, 421);
            this.page.SizeOfPaper = new System.Drawing.Size(0, 0);
            this.page.TabIndex = 0;
            // 
            // pnlProperty
            // 
            this.pnlProperty.AssociatedSplitter = null;
            this.pnlProperty.BackColor = System.Drawing.Color.Transparent;
            this.pnlProperty.CaptionFont = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold);
            this.pnlProperty.CaptionHeight = 22;
            this.pnlProperty.Controls.Add(this.ucProperty);
            this.pnlProperty.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.pnlProperty.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.pnlProperty.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.pnlProperty.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pnlProperty.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.pnlProperty.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlProperty.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.pnlProperty.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.pnlProperty.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.pnlProperty.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.pnlProperty.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.pnlProperty.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pnlProperty.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.pnlProperty.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlProperty.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlProperty.Image = null;
            this.pnlProperty.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlProperty.Location = new System.Drawing.Point(771, 2);
            this.pnlProperty.MinimumSize = new System.Drawing.Size(22, 22);
            this.pnlProperty.Name = "pnlProperty";
            this.pnlProperty.PanelStyle = Skyray.Controls.XPander.PanelStyle.Office2007Blue;
            this.pnlProperty.ShowExpandIcon = true;
            this.pnlProperty.Size = new System.Drawing.Size(239, 654);
            this.pnlProperty.Style = Skyray.Controls.Style.Office2007Blue;
            this.pnlProperty.TabIndex = 1;
            this.pnlProperty.Text = "属性";
            this.pnlProperty.ToolTipTextCloseIcon = null;
            this.pnlProperty.ToolTipTextExpandIconPanelCollapsed = null;
            this.pnlProperty.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // ucProperty
            // 
            this.ucProperty.BackColor = System.Drawing.Color.White;
            this.ucProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucProperty.Location = new System.Drawing.Point(1, 23);
            this.ucProperty.Name = "ucProperty";
            this.ucProperty.Padding = new System.Windows.Forms.Padding(8);
            this.ucProperty.Page = null;
            this.ucProperty.Size = new System.Drawing.Size(237, 630);
            this.ucProperty.TabIndex = 0;
            this.ucProperty.Visible = false;
            // 
            // mtSplitter1
            // 
            this.mtSplitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.mtSplitter1.Location = new System.Drawing.Point(172, 2);
            this.mtSplitter1.Name = "mtSplitter1";
            this.mtSplitter1.Size = new System.Drawing.Size(2, 654);
            this.mtSplitter1.SplitterBorderColor = System.Drawing.Color.Transparent;
            this.mtSplitter1.SplitterDarkColor = System.Drawing.Color.Transparent;
            this.mtSplitter1.SplitterLightColor = System.Drawing.Color.Transparent;
            this.mtSplitter1.SplitterPaintAngle = 90F;
            this.mtSplitter1.TabIndex = 1;
            this.mtSplitter1.TabStop = false;
            // 
            // pnlSource
            // 
            this.pnlSource.AssociatedSplitter = null;
            this.pnlSource.BackColor = System.Drawing.Color.Transparent;
            this.pnlSource.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
            this.pnlSource.CaptionHeight = 24;
            this.pnlSource.Controls.Add(this.treeSource);
            this.pnlSource.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.pnlSource.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.pnlSource.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.pnlSource.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pnlSource.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.pnlSource.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlSource.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.pnlSource.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.pnlSource.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.pnlSource.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.pnlSource.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.pnlSource.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pnlSource.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.pnlSource.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSource.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlSource.Image = null;
            this.pnlSource.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlSource.Location = new System.Drawing.Point(2, 2);
            this.pnlSource.MinimumSize = new System.Drawing.Size(24, 24);
            this.pnlSource.Name = "pnlSource";
            this.pnlSource.PanelStyle = Skyray.Controls.XPander.PanelStyle.Office2007Blue;
            this.pnlSource.ShowExpandIcon = true;
            this.pnlSource.Size = new System.Drawing.Size(170, 654);
            this.pnlSource.Style = Skyray.Controls.Style.Office2007Blue;
            this.pnlSource.TabIndex = 1;
            this.pnlSource.Text = "数据加载中...";
            this.pnlSource.ToolTipTextCloseIcon = null;
            this.pnlSource.ToolTipTextExpandIconPanelCollapsed = null;
            this.pnlSource.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // treeSource
            // 
            this.treeSource.AllowDrop = true;
            this.treeSource.AutoItemDrag = false;
            this.treeSource.BackColor = System.Drawing.SystemColors.Window;
            this.treeSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeSource.DefaultToolTipProvider = null;
            this.treeSource.DisplayDraggingNodes = true;
            this.treeSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeSource.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeSource.HighlightDropPosition = false;
            this.treeSource.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeSource.Location = new System.Drawing.Point(1, 25);
            this.treeSource.Model = null;
            this.treeSource.Name = "treeSource";
            this.treeSource.ReportDataSource = null;
            this.treeSource.RowHeight = 18;
            this.treeSource.SelectedNode = null;
            this.treeSource.SelfLevelEdit = false;
            this.treeSource.Size = new System.Drawing.Size(168, 628);
            this.treeSource.TabIndex = 0;
            this.treeSource.Text = "treeSource";
            // 
            // toolStripW1
            // 
            this.toolStripW1.Location = new System.Drawing.Point(0, 24);
            this.toolStripW1.Name = "toolStripW1";
            this.toolStripW1.Size = new System.Drawing.Size(1012, 25);
            this.toolStripW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.toolStripW1.TabIndex = 2;
            this.toolStripW1.Text = "toolStripW1";
            this.toolStripW1.Visible = false;
            // 
            // statusStripWStatus
            // 
            this.statusStripWStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssPageAreaText,
            this.tssArea,
            this.tssPapperSizeText,
            this.tssPapperSizeValue,
            this.tssPageSizeText,
            this.tssAreaSize,
            this.tssMousePointText,
            this.tssMousePoint});
            this.statusStripWStatus.Location = new System.Drawing.Point(0, 682);
            this.statusStripWStatus.Name = "statusStripWStatus";
            this.statusStripWStatus.Size = new System.Drawing.Size(1012, 22);
            this.statusStripWStatus.Style = Skyray.Controls.Style.Office2007Blue;
            this.statusStripWStatus.TabIndex = 0;
            this.statusStripWStatus.Text = "statusStripWStatus";
            this.statusStripWStatus.TTT = null;
            // 
            // tssPageAreaText
            // 
            this.tssPageAreaText.Name = "tssPageAreaText";
            this.tssPageAreaText.Size = new System.Drawing.Size(35, 17);
            this.tssPageAreaText.Text = "区域 ";
            // 
            // tssArea
            // 
            this.tssArea.Name = "tssArea";
            this.tssArea.Size = new System.Drawing.Size(41, 17);
            this.tssArea.Text = "Header";
            // 
            // tssPapperSizeText
            // 
            this.tssPapperSizeText.Name = "tssPapperSizeText";
            this.tssPapperSizeText.Size = new System.Drawing.Size(29, 17);
            this.tssPapperSizeText.Text = "纸张";
            // 
            // tssPapperSizeValue
            // 
            this.tssPapperSizeValue.Name = "tssPapperSizeValue";
            this.tssPapperSizeValue.Size = new System.Drawing.Size(17, 17);
            this.tssPapperSizeValue.Text = "A4";
            // 
            // tssPageSizeText
            // 
            this.tssPageSizeText.Name = "tssPageSizeText";
            this.tssPageSizeText.Size = new System.Drawing.Size(29, 17);
            this.tssPageSizeText.Text = "大小";
            // 
            // tssAreaSize
            // 
            this.tssAreaSize.Name = "tssAreaSize";
            this.tssAreaSize.Size = new System.Drawing.Size(47, 17);
            this.tssAreaSize.Text = "600,400";
            // 
            // tssMousePointText
            // 
            this.tssMousePointText.Name = "tssMousePointText";
            this.tssMousePointText.Size = new System.Drawing.Size(77, 17);
            this.tssMousePointText.Text = "鼠标当前位置";
            // 
            // tssMousePoint
            // 
            this.tssMousePoint.Name = "tssMousePoint";
            this.tssMousePoint.Size = new System.Drawing.Size(47, 17);
            this.tssMousePoint.Text = "X=0;Y=0";
            // 
            // menuStripW1
            // 
            this.menuStripW1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiTable,
            this.tsmiSet,
            this.tsmiHelp});
            this.menuStripW1.Location = new System.Drawing.Point(0, 0);
            this.menuStripW1.Name = "menuStripW1";
            this.menuStripW1.Size = new System.Drawing.Size(1012, 24);
            this.menuStripW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.menuStripW1.TabIndex = 1;
            this.menuStripW1.Text = "menuStripW1";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpenTemplate,
            this.tsmiSaveTreeSource,
            this.报表导出ToolStripMenuItem,
            this.toolStripSeparator1,
            this.tsmiPriview,
            this.tsmiPrint});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(41, 20);
            this.tsmiFile.Text = "文件";
            // 
            // tsmiOpenTemplate
            // 
            this.tsmiOpenTemplate.Name = "tsmiOpenTemplate";
            this.tsmiOpenTemplate.Size = new System.Drawing.Size(118, 22);
            this.tsmiOpenTemplate.Text = "打开模板";
            this.tsmiOpenTemplate.Click += new System.EventHandler(this.tsmiOpenTemplate_Click);
            // 
            // tsmiSaveTreeSource
            // 
            this.tsmiSaveTreeSource.Name = "tsmiSaveTreeSource";
            this.tsmiSaveTreeSource.Size = new System.Drawing.Size(118, 22);
            this.tsmiSaveTreeSource.Tag = "ToNaviItem";
            this.tsmiSaveTreeSource.Text = "存为模板";
            this.tsmiSaveTreeSource.Click += new System.EventHandler(this.tsmiSaveTreeSource_Click);
            // 
            // 报表导出ToolStripMenuItem
            // 
            this.报表导出ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExportToExcel,
            this.tsmiExportToPDF});
            this.报表导出ToolStripMenuItem.Name = "报表导出ToolStripMenuItem";
            this.报表导出ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.报表导出ToolStripMenuItem.Text = "报表导出";
            // 
            // tsmiExportToExcel
            // 
            this.tsmiExportToExcel.Name = "tsmiExportToExcel";
            this.tsmiExportToExcel.Size = new System.Drawing.Size(124, 22);
            this.tsmiExportToExcel.Text = "Excel格式";
            this.tsmiExportToExcel.Click += new System.EventHandler(this.tsmiExportToExcel_Click);
            // 
            // tsmiExportToPDF
            // 
            this.tsmiExportToPDF.Name = "tsmiExportToPDF";
            this.tsmiExportToPDF.Size = new System.Drawing.Size(124, 22);
            this.tsmiExportToPDF.Text = "Pdf 格式";
            this.tsmiExportToPDF.Visible = false;
            this.tsmiExportToPDF.Click += new System.EventHandler(this.tsmiExportToPDF_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(115, 6);
            // 
            // tsmiPriview
            // 
            this.tsmiPriview.Name = "tsmiPriview";
            this.tsmiPriview.Size = new System.Drawing.Size(118, 22);
            this.tsmiPriview.Text = "打印预览";
            this.tsmiPriview.Click += new System.EventHandler(this.tsmiPriview_Click);
            // 
            // tsmiPrint
            // 
            this.tsmiPrint.Name = "tsmiPrint";
            this.tsmiPrint.Size = new System.Drawing.Size(118, 22);
            this.tsmiPrint.Text = "打印";
            this.tsmiPrint.Click += new System.EventHandler(this.tsmiPrint_Click);
            // 
            // tsmiTable
            // 
            this.tsmiTable.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsInsertT});
            this.tsmiTable.Name = "tsmiTable";
            this.tsmiTable.Size = new System.Drawing.Size(41, 20);
            this.tsmiTable.Text = "表格";
            this.tsmiTable.Visible = false;
            // 
            // tlsInsertT
            // 
            this.tlsInsertT.Name = "tlsInsertT";
            this.tlsInsertT.Size = new System.Drawing.Size(118, 22);
            this.tlsInsertT.Text = "插入表格";
            this.tlsInsertT.Click += new System.EventHandler(this.tlsInsertT_Click);
            // 
            // tsmiSet
            // 
            this.tsmiSet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiShowHeader,
            this.tsmiShowFooter});
            this.tsmiSet.Name = "tsmiSet";
            this.tsmiSet.Size = new System.Drawing.Size(41, 20);
            this.tsmiSet.Text = "设置";
            // 
            // tsmiShowHeader
            // 
            this.tsmiShowHeader.Checked = true;
            this.tsmiShowHeader.CheckOnClick = true;
            this.tsmiShowHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiShowHeader.Name = "tsmiShowHeader";
            this.tsmiShowHeader.Size = new System.Drawing.Size(94, 22);
            this.tsmiShowHeader.Text = "页眉";
            this.tsmiShowHeader.Click += new System.EventHandler(this.tsmiShowHeader_Click);
            // 
            // tsmiShowFooter
            // 
            this.tsmiShowFooter.CheckOnClick = true;
            this.tsmiShowFooter.Name = "tsmiShowFooter";
            this.tsmiShowFooter.Size = new System.Drawing.Size(94, 22);
            this.tsmiShowFooter.Text = "页脚";
            this.tsmiShowFooter.Click += new System.EventHandler(this.tsmiShowFooter_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAbout});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(41, 20);
            this.tsmiHelp.Text = "帮助";
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(94, 22);
            this.tsmiAbout.Text = "关于";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // cmsPage
            // 
            this.cmsPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctsmiShowHeader,
            this.ctsmiShowFooter,
            this.tss1,
            this.tsmiDelSelect,
            this.tsmiDelAll,
            this.tss2,
            this.tsmiPageProperty});
            this.cmsPage.Name = "contextMenuStrip1";
            this.cmsPage.Size = new System.Drawing.Size(153, 148);
            // 
            // ctsmiShowHeader
            // 
            this.ctsmiShowHeader.Checked = true;
            this.ctsmiShowHeader.CheckOnClick = true;
            this.ctsmiShowHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ctsmiShowHeader.Name = "ctsmiShowHeader";
            this.ctsmiShowHeader.Size = new System.Drawing.Size(152, 22);
            this.ctsmiShowHeader.Text = "页眉";
            this.ctsmiShowHeader.Click += new System.EventHandler(this.tsmiShowHeader_Click);
            // 
            // ctsmiShowFooter
            // 
            this.ctsmiShowFooter.CheckOnClick = true;
            this.ctsmiShowFooter.Name = "ctsmiShowFooter";
            this.ctsmiShowFooter.Size = new System.Drawing.Size(152, 22);
            this.ctsmiShowFooter.Text = "页脚";
            this.ctsmiShowFooter.Click += new System.EventHandler(this.tsmiShowFooter_Click);
            // 
            // tss1
            // 
            this.tss1.Name = "tss1";
            this.tss1.Size = new System.Drawing.Size(149, 6);
            this.tss1.Visible = false;
            // 
            // tsmiDelSelect
            // 
            this.tsmiDelSelect.Name = "tsmiDelSelect";
            this.tsmiDelSelect.Size = new System.Drawing.Size(152, 22);
            this.tsmiDelSelect.Text = "删除选择";
            this.tsmiDelSelect.Click += new System.EventHandler(this.tsmiDelSelect_Click);
            // 
            // tsmiDelAll
            // 
            this.tsmiDelAll.Name = "tsmiDelAll";
            this.tsmiDelAll.Size = new System.Drawing.Size(152, 22);
            this.tsmiDelAll.Text = "删除全部";
            this.tsmiDelAll.Click += new System.EventHandler(this.tsmiDelAll_Click);
            // 
            // tss2
            // 
            this.tss2.Name = "tss2";
            this.tss2.Size = new System.Drawing.Size(149, 6);
            this.tss2.Visible = false;
            // 
            // tsmiPageProperty
            // 
            this.tsmiPageProperty.Name = "tsmiPageProperty";
            this.tsmiPageProperty.Size = new System.Drawing.Size(152, 22);
            this.tsmiPageProperty.Text = "页面属性";
            this.tsmiPageProperty.Click += new System.EventHandler(this.tsmiPageProperty_Click);
            // 
            // cmsCtrl
            // 
            this.cmsCtrl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDelSelected,
            this.tsmiAlign,
            this.tsmiSize,
            this.tsmiSpace,
            this.tsmiBringToFront,
            this.tsmiSendToBack,
            this.tsmiAlignCenter});
            this.cmsCtrl.Name = "contextMenuStrip1";
            this.cmsCtrl.Size = new System.Drawing.Size(119, 158);
            // 
            // tsmiDelSelected
            // 
            this.tsmiDelSelected.Name = "tsmiDelSelected";
            this.tsmiDelSelected.Size = new System.Drawing.Size(118, 22);
            this.tsmiDelSelected.Text = "删除选择";
            this.tsmiDelSelected.Click += new System.EventHandler(this.tsmiDelSelected_Click);
            // 
            // tsmiAlign
            // 
            this.tsmiAlign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAlignLeft,
            this.tsmiAlignRight,
            this.tsmiAlignTop,
            this.tsmiAlignBottom,
            this.tsmiAlignLabel});
            this.tsmiAlign.Name = "tsmiAlign";
            this.tsmiAlign.Size = new System.Drawing.Size(118, 22);
            this.tsmiAlign.Text = "对齐";
            // 
            // tsmiAlignLeft
            // 
            this.tsmiAlignLeft.Name = "tsmiAlignLeft";
            this.tsmiAlignLeft.Size = new System.Drawing.Size(118, 22);
            this.tsmiAlignLeft.Text = "左对齐";
            this.tsmiAlignLeft.Click += new System.EventHandler(this.tsmiAlignLeft_Click);
            // 
            // tsmiAlignRight
            // 
            this.tsmiAlignRight.Name = "tsmiAlignRight";
            this.tsmiAlignRight.Size = new System.Drawing.Size(118, 22);
            this.tsmiAlignRight.Text = "右对齐";
            this.tsmiAlignRight.Click += new System.EventHandler(this.tsmiAlignRight_Click);
            // 
            // tsmiAlignTop
            // 
            this.tsmiAlignTop.Name = "tsmiAlignTop";
            this.tsmiAlignTop.Size = new System.Drawing.Size(118, 22);
            this.tsmiAlignTop.Text = "顶端对齐";
            this.tsmiAlignTop.Click += new System.EventHandler(this.tsmiAlignTop_Click);
            // 
            // tsmiAlignBottom
            // 
            this.tsmiAlignBottom.Name = "tsmiAlignBottom";
            this.tsmiAlignBottom.Size = new System.Drawing.Size(118, 22);
            this.tsmiAlignBottom.Text = "底端对齐";
            this.tsmiAlignBottom.Click += new System.EventHandler(this.tsmiAlignBottom_Click);
            // 
            // tsmiAlignLabel
            // 
            this.tsmiAlignLabel.Name = "tsmiAlignLabel";
            this.tsmiAlignLabel.Size = new System.Drawing.Size(118, 22);
            this.tsmiAlignLabel.Text = "标签对齐";
            this.tsmiAlignLabel.Click += new System.EventHandler(this.tsmiAlignLabel_Click);
            // 
            // tsmiSize
            // 
            this.tsmiSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSameWidth,
            this.tsmiSameHeight,
            this.tsmiSameSize});
            this.tsmiSize.Name = "tsmiSize";
            this.tsmiSize.Size = new System.Drawing.Size(118, 22);
            this.tsmiSize.Text = "形状";
            // 
            // tsmiSameWidth
            // 
            this.tsmiSameWidth.Name = "tsmiSameWidth";
            this.tsmiSameWidth.Size = new System.Drawing.Size(106, 22);
            this.tsmiSameWidth.Text = "等宽";
            this.tsmiSameWidth.Click += new System.EventHandler(this.tsmiSameWidth_Click);
            // 
            // tsmiSameHeight
            // 
            this.tsmiSameHeight.Name = "tsmiSameHeight";
            this.tsmiSameHeight.Size = new System.Drawing.Size(106, 22);
            this.tsmiSameHeight.Text = "等高";
            this.tsmiSameHeight.Click += new System.EventHandler(this.tsmiSameHeight_Click);
            // 
            // tsmiSameSize
            // 
            this.tsmiSameSize.Name = "tsmiSameSize";
            this.tsmiSameSize.Size = new System.Drawing.Size(106, 22);
            this.tsmiSameSize.Text = "等大小";
            this.tsmiSameSize.Click += new System.EventHandler(this.tsmiSameSize_Click);
            // 
            // tsmiSpace
            // 
            this.tsmiSpace.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRemoveVSpace,
            this.tsmiRemoveHSpace});
            this.tsmiSpace.Name = "tsmiSpace";
            this.tsmiSpace.Size = new System.Drawing.Size(118, 22);
            this.tsmiSpace.Text = "间距";
            // 
            // tsmiRemoveVSpace
            // 
            this.tsmiRemoveVSpace.Name = "tsmiRemoveVSpace";
            this.tsmiRemoveVSpace.Size = new System.Drawing.Size(142, 22);
            this.tsmiRemoveVSpace.Text = "移除垂直间距";
            this.tsmiRemoveVSpace.Click += new System.EventHandler(this.tsmiRemoveVSpace_Click);
            // 
            // tsmiRemoveHSpace
            // 
            this.tsmiRemoveHSpace.Name = "tsmiRemoveHSpace";
            this.tsmiRemoveHSpace.Size = new System.Drawing.Size(142, 22);
            this.tsmiRemoveHSpace.Text = "移除水平间距";
            this.tsmiRemoveHSpace.Click += new System.EventHandler(this.tsmiRemoveHSpace_Click);
            // 
            // tsmiBringToFront
            // 
            this.tsmiBringToFront.Name = "tsmiBringToFront";
            this.tsmiBringToFront.Size = new System.Drawing.Size(118, 22);
            this.tsmiBringToFront.Text = "置顶";
            this.tsmiBringToFront.Click += new System.EventHandler(this.tsmiBringToFront_Click);
            // 
            // tsmiSendToBack
            // 
            this.tsmiSendToBack.Name = "tsmiSendToBack";
            this.tsmiSendToBack.Size = new System.Drawing.Size(118, 22);
            this.tsmiSendToBack.Text = "置底";
            this.tsmiSendToBack.Click += new System.EventHandler(this.tsmiSendToBack_Click);
            // 
            // tsmiAlignCenter
            // 
            this.tsmiAlignCenter.Name = "tsmiAlignCenter";
            this.tsmiAlignCenter.Size = new System.Drawing.Size(118, 22);
            this.tsmiAlignCenter.Text = "居中";
            this.tsmiAlignCenter.Click += new System.EventHandler(this.tsmiAlignCenter_Click);
            // 
            // UCPrint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.pnlClient);
            this.Controls.Add(this.statusStripWStatus);
            this.Controls.Add(this.menuStripW1);
            this.Controls.Add(this.toolStripW1);
            this.Name = "UCPrint";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.Size = new System.Drawing.Size(1012, 704);
            this.Load += new System.EventHandler(this.UCPrint_Load);
            this.pnlClient.ResumeLayout(false);
            this.pnlPage.ResumeLayout(false);
            this.pnlProperty.ResumeLayout(false);
            this.pnlSource.ResumeLayout(false);
            this.statusStripWStatus.ResumeLayout(false);
            this.statusStripWStatus.PerformLayout();
            this.menuStripW1.ResumeLayout(false);
            this.menuStripW1.PerformLayout();
            this.cmsPage.ResumeLayout(false);
            this.cmsCtrl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private MenuStripW menuStripW1;
        private StatusStripW statusStripWStatus;
        private ToolStripW toolStripW1;
        private ToolStripMenuItem tsmiPrint;
        private ToolStripMenuItem tsmiPriview;
        private ToolStripMenuItem tsmiSaveTreeSource;
        private ToolStripMenuItem tsmiFile;
        private Panel pnlClient;
        private ToolStripMenuItem tsmiSet;

        private Skyray.Controls.MTSplitter mtSplitter1;
        private PrintPage page;
        private ToolStripStatusLabel tssMousePointText;
        private ToolStripStatusLabel tssPageSizeText;
        private ToolStripStatusLabel tssPageAreaText;
        private ToolStripStatusLabel tssArea;
        private ToolStripStatusLabel tssAreaSize;
        private ToolStripStatusLabel tssMousePoint;
        private Skyray.Controls.XPander.Panel pnlSource;
        private TreeViewAdvEx treeSource;
        private Panel pnlPage;
        private Skyray.Controls.XPander.Panel pnlProperty;
        private UCProperty ucProperty;
        private ToolStripMenuItem tsmiOpenTemplate;
        private ToolStripMenuItem tsmiHelp;
        private ToolStripMenuItem tsmiAbout;
        private ToolStripSeparator toolStripSeparator1;

        private System.Windows.Forms.ToolStripMenuItem tsmiShowHeader;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowFooter;
        private ContextMenuStrip cmsPage;
        private ToolStripMenuItem ctsmiShowHeader;
        private ToolStripMenuItem ctsmiShowFooter;
        private ToolStripSeparator tss1;
        private ToolStripMenuItem tsmiDelSelect;
        private ToolStripMenuItem tsmiDelAll;
        private ToolStripSeparator tss2;
        private ToolStripMenuItem tsmiPageProperty;
        private ContextMenuStrip cmsCtrl;
        private ToolStripMenuItem tsmiDelSelected;
        private ToolStripMenuItem tsmiAlign;
        private ToolStripMenuItem tsmiAlignLeft;
        private ToolStripMenuItem tsmiAlignRight;
        private ToolStripMenuItem tsmiAlignTop;
        private ToolStripMenuItem tsmiAlignBottom;
        private ToolStripMenuItem tsmiAlignLabel;
        private ToolStripMenuItem tsmiSize;
        private ToolStripMenuItem tsmiSameWidth;
        private ToolStripMenuItem tsmiSameHeight;
        private ToolStripMenuItem tsmiSameSize;
        private ToolStripMenuItem tsmiSpace;
        private ToolStripMenuItem tsmiRemoveVSpace;
        private ToolStripMenuItem tsmiRemoveHSpace;
        private ToolStripMenuItem tsmiBringToFront;
        private ToolStripMenuItem tsmiSendToBack;
        private ToolStripMenuItem 报表导出ToolStripMenuItem;
        private ToolStripMenuItem tsmiExportToExcel;
        private ToolStripMenuItem tsmiExportToPDF;
        private VScrollBar vScrollBar1;
        private HScrollBar hScrollBar1;
        private ToolStripMenuItem tsmiTable;
        private ToolStripMenuItem tlsInsertT;
        private ToolStripMenuItem tsmiAlignCenter;
        private ToolStripStatusLabel tssPapperSizeText;
        private ToolStripStatusLabel tssPapperSizeValue;

    }
}
