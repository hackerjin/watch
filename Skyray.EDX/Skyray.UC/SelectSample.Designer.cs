namespace Skyray.UC
{
    partial class SelectSample
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.colDlg = new System.Windows.Forms.ColorDialog();
            this.fbdImport = new System.Windows.Forms.FolderBrowserDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.HighPerformanceListView1 = new Skyray.Controls.HighPerformanceListView();
            this.grpSearch = new Skyray.Controls.Grouper();
            this.lblSortType = new Skyray.Controls.LabelW();
            this.btnExportbyResearch = new Skyray.Controls.ButtonW();
            this.cobNameOrder = new System.Windows.Forms.ComboBox();
            this.chkNameOrder = new Skyray.Controls.CheckBoxW();
            this.cobTimeOrder = new System.Windows.Forms.ComboBox();
            this.chkTimeOrder = new Skyray.Controls.CheckBoxW();
            this.lblTo = new Skyray.Controls.LabelW();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.chkDate = new Skyray.Controls.CheckBoxW();
            this.chkName = new Skyray.Controls.CheckBoxW();
            this.btnSearch = new Skyray.Controls.ButtonW();
            this.txtKeywords = new Skyray.Controls.TextBoxW();
            this.panel5 = new System.Windows.Forms.Panel();
            this.xcSpec = new Skyray.Controls.Extension.XRFChart();
            this.grpSpec = new Skyray.Controls.Grouper();
            this.buttonWOtherInfoSet = new Skyray.Controls.ButtonW();
            this.txtSpecDate = new System.Windows.Forms.DateTimePicker();
            this.btnConfirmParamModify = new Skyray.Controls.ButtonW();
            this.txtSummary = new Skyray.Controls.TextBoxW();
            this.lblDescription = new Skyray.Controls.LabelW();
            this.txtColor = new Skyray.Controls.TextBoxW();
            this.lblColor = new Skyray.Controls.LabelW();
            this.txtSampleName = new Skyray.Controls.TextBoxW();
            this.lblSampleName = new Skyray.Controls.LabelW();
            this.lblWeight = new Skyray.Controls.LabelW();
            this.lblSpecDate = new Skyray.Controls.LabelW();
            this.txtShape = new Skyray.Controls.TextBoxW();
            this.lblSupplier = new Skyray.Controls.LabelW();
            this.txtSupplier = new Skyray.Controls.TextBoxW();
            this.lblShape = new Skyray.Controls.LabelW();
            this.txtWeight = new Skyray.Controls.TextBoxW();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExpand = new Skyray.Controls.ButtonW();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new Skyray.Controls.ButtonW();
            this.btnImport = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btWSelectDemo = new Skyray.Controls.ButtonW();
            this.btnExport = new Skyray.Controls.ButtonW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.btnSelect = new Skyray.Controls.ButtonW();
            this.panel4 = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.panel5.SuspendLayout();
            this.grpSpec.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // colDlg
            // 
            this.colDlg.AnyColor = true;
            this.colDlg.FullOpen = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.HighPerformanceListView1);
            this.splitContainer1.Panel1.Controls.Add(this.grpSearch);
            this.splitContainer1.Panel1MinSize = 206;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel5);
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2MinSize = 11;
            this.splitContainer1.Size = new System.Drawing.Size(778, 490);
            this.splitContainer1.SplitterDistance = 206;
            this.splitContainer1.TabIndex = 24;
            // 
            // HighPerformanceListView1
            // 
            this.HighPerformanceListView1.DisplayStartIdx = 0;
            this.HighPerformanceListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HighPerformanceListView1.hScrollValue = 0;
            this.HighPerformanceListView1.ListViewProvider = null;
            this.HighPerformanceListView1.Location = new System.Drawing.Point(0, 174);
            this.HighPerformanceListView1.Name = "HighPerformanceListView1";
            this.HighPerformanceListView1.Size = new System.Drawing.Size(206, 316);
            this.HighPerformanceListView1.TabIndex = 23;
            // 
            // grpSearch
            // 
            this.grpSearch.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpSearch.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpSearch.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpSearch.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpSearch.BorderThickness = 1F;
            this.grpSearch.BorderTopOnly = false;
            this.grpSearch.Controls.Add(this.lblSortType);
            this.grpSearch.Controls.Add(this.btnExportbyResearch);
            this.grpSearch.Controls.Add(this.cobNameOrder);
            this.grpSearch.Controls.Add(this.chkNameOrder);
            this.grpSearch.Controls.Add(this.cobTimeOrder);
            this.grpSearch.Controls.Add(this.chkTimeOrder);
            this.grpSearch.Controls.Add(this.lblTo);
            this.grpSearch.Controls.Add(this.dateTimePickerTo);
            this.grpSearch.Controls.Add(this.dateTimePickerFrom);
            this.grpSearch.Controls.Add(this.chkDate);
            this.grpSearch.Controls.Add(this.chkName);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.txtKeywords);
            this.grpSearch.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSearch.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpSearch.GroupImage = null;
            this.grpSearch.GroupTitle = "查询";
            this.grpSearch.HeaderRoundCorners = 4;
            this.grpSearch.Location = new System.Drawing.Point(0, 0);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.PaintGroupBox = false;
            this.grpSearch.RoundCorners = 4;
            this.grpSearch.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpSearch.ShadowControl = false;
            this.grpSearch.ShadowThickness = 3;
            this.grpSearch.Size = new System.Drawing.Size(206, 174);
            this.grpSearch.TabIndex = 21;
            this.grpSearch.TextLineSpace = 2;
            this.grpSearch.TitleLeftSpace = 18;
            // 
            // lblSortType
            // 
            this.lblSortType.BackColor = System.Drawing.Color.Transparent;
            this.lblSortType.Location = new System.Drawing.Point(6, 100);
            this.lblSortType.Name = "lblSortType";
            this.lblSortType.Size = new System.Drawing.Size(53, 38);
            this.lblSortType.TabIndex = 42;
            this.lblSortType.Text = "排序方式";
            // 
            // btnExportbyResearch
            // 
            this.btnExportbyResearch.bSilver = false;
            this.btnExportbyResearch.Location = new System.Drawing.Point(8, 144);
            this.btnExportbyResearch.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnExportbyResearch.MenuPos = new System.Drawing.Point(0, 0);
            this.btnExportbyResearch.Name = "btnExportbyResearch";
            this.btnExportbyResearch.Size = new System.Drawing.Size(84, 23);
            this.btnExportbyResearch.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnExportbyResearch.TabIndex = 23;
            this.btnExportbyResearch.Text = "按条件导出";
            this.btnExportbyResearch.ToFocused = true;
            this.btnExportbyResearch.UseVisualStyleBackColor = true;
            this.btnExportbyResearch.Click += new System.EventHandler(this.btnExportbyResearch_Click);
            // 
            // cobNameOrder
            // 
            this.cobNameOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cobNameOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobNameOrder.FormattingEnabled = true;
            this.cobNameOrder.Items.AddRange(new object[] {
            "Asc",
            "Desc"});
            this.cobNameOrder.Location = new System.Drawing.Point(146, 118);
            this.cobNameOrder.Name = "cobNameOrder";
            this.cobNameOrder.Size = new System.Drawing.Size(53, 20);
            this.cobNameOrder.TabIndex = 40;
            // 
            // chkNameOrder
            // 
            this.chkNameOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkNameOrder.AutoSize = true;
            this.chkNameOrder.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkNameOrder.Location = new System.Drawing.Point(65, 120);
            this.chkNameOrder.Name = "chkNameOrder";
            this.chkNameOrder.Size = new System.Drawing.Size(48, 16);
            this.chkNameOrder.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkNameOrder.TabIndex = 41;
            this.chkNameOrder.Text = "名称";
            this.chkNameOrder.UseVisualStyleBackColor = true;
            // 
            // cobTimeOrder
            // 
            this.cobTimeOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cobTimeOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobTimeOrder.FormattingEnabled = true;
            this.cobTimeOrder.Items.AddRange(new object[] {
            "Asc",
            "Desc"});
            this.cobTimeOrder.Location = new System.Drawing.Point(146, 94);
            this.cobTimeOrder.Name = "cobTimeOrder";
            this.cobTimeOrder.Size = new System.Drawing.Size(53, 20);
            this.cobTimeOrder.TabIndex = 23;
            // 
            // chkTimeOrder
            // 
            this.chkTimeOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTimeOrder.AutoSize = true;
            this.chkTimeOrder.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkTimeOrder.Location = new System.Drawing.Point(65, 98);
            this.chkTimeOrder.Name = "chkTimeOrder";
            this.chkTimeOrder.Size = new System.Drawing.Size(48, 16);
            this.chkTimeOrder.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkTimeOrder.TabIndex = 39;
            this.chkTimeOrder.Text = "时间";
            this.chkTimeOrder.UseVisualStyleBackColor = true;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.BackColor = System.Drawing.Color.Transparent;
            this.lblTo.Location = new System.Drawing.Point(36, 73);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(17, 12);
            this.lblTo.TabIndex = 35;
            this.lblTo.Text = "至";
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerTo.Location = new System.Drawing.Point(66, 69);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(135, 21);
            this.dateTimePickerTo.TabIndex = 34;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerFrom.Location = new System.Drawing.Point(66, 43);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(135, 21);
            this.dateTimePickerFrom.TabIndex = 33;
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkDate.Location = new System.Drawing.Point(8, 42);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(48, 16);
            this.chkDate.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkDate.TabIndex = 32;
            this.chkDate.Text = "日期";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // chkName
            // 
            this.chkName.AutoSize = true;
            this.chkName.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkName.Location = new System.Drawing.Point(8, 21);
            this.chkName.Name = "chkName";
            this.chkName.Size = new System.Drawing.Size(48, 16);
            this.chkName.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkName.TabIndex = 31;
            this.chkName.Text = "名称";
            this.chkName.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.bSilver = false;
            this.btnSearch.Location = new System.Drawing.Point(135, 144);
            this.btnSearch.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSearch.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(64, 23);
            this.btnSearch.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSearch.TabIndex = 30;
            this.btnSearch.Text = "查询";
            this.btnSearch.ToFocused = false;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtKeywords
            // 
            this.txtKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKeywords.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtKeywords.Location = new System.Drawing.Point(66, 19);
            this.txtKeywords.Name = "txtKeywords";
            this.txtKeywords.Size = new System.Drawing.Size(135, 21);
            this.txtKeywords.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtKeywords.TabIndex = 29;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.xcSpec);
            this.panel5.Controls.Add(this.grpSpec);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(17, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(551, 490);
            this.panel5.TabIndex = 41;
            // 
            // xcSpec
            // 
            this.xcSpec.BoundaryElement = null;
            this.xcSpec.BShowElement = false;
            this.xcSpec.ChangeSpeed = 0.025;
            this.xcSpec.Coeff = null;
            this.xcSpec.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.YellowGreen,
        System.Drawing.Color.YellowGreen};
            this.xcSpec.CurrentChannelCount = 0;
            this.xcSpec.currentInterestArea = null;
            this.xcSpec.Cursor = System.Windows.Forms.Cursors.Default;
            this.xcSpec.DefaultLineHeight = 30;
            this.xcSpec.DEnergy = 0;
            this.xcSpec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xcSpec.EditButtons = System.Windows.Forms.MouseButtons.Right;
            this.xcSpec.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.xcSpec.EnableMoveLine = true;
            this.xcSpec.EnableWheel = true;
            this.xcSpec.FontSize = 12;
            this.xcSpec.HorizontalSynchronization = true;
            this.xcSpec.ICurrentChannel = 0;
            this.xcSpec.InterestAreaEnable = false;
            this.xcSpec.IsActiveMove = true;
            this.xcSpec.IsEnableWheelZoom = false;
            this.xcSpec.IsMainElement = false;
            this.xcSpec.IsShowPeakFlagAuto = false;
            this.xcSpec.IsUseScroll = false;
            this.xcSpec.IUseBase = false;
            this.xcSpec.IUseBoundary = false;
            this.xcSpec.IXMaxChannel = 2048;
            this.xcSpec.IYMaxChannel = 1000;
            this.xcSpec.IZero = false;
            this.xcSpec.LinkButtons = System.Windows.Forms.MouseButtons.Left;
            this.xcSpec.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.xcSpec.Location = new System.Drawing.Point(0, 167);
            this.xcSpec.MultiGraph = false;
            this.xcSpec.Name = "xcSpec";
            this.xcSpec.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.xcSpec.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            this.xcSpec.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)));
            this.xcSpec.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            this.xcSpec.Positions = new float[] {
        0F,
        1F};
            this.xcSpec.ScrollGrace = 0;
            this.xcSpec.ScrollMaxX = 0;
            this.xcSpec.ScrollMaxY = 0;
            this.xcSpec.ScrollMaxY2 = 0;
            this.xcSpec.ScrollMinX = 0;
            this.xcSpec.ScrollMinY = 0;
            this.xcSpec.ScrollMinY2 = 0;
            this.xcSpec.SelectButtons = System.Windows.Forms.MouseButtons.Left;
            this.xcSpec.SelectModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.xcSpec.Size = new System.Drawing.Size(551, 323);
            this.xcSpec.TabIndex = 18;
            this.xcSpec.UnSpecing = true;
            this.xcSpec.VerticalSynchronization = true;
            this.xcSpec.XTitle = "";
            this.xcSpec.YTitle = "";
            this.xcSpec.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
            this.xcSpec.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            this.xcSpec.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            this.xcSpec.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            // 
            // grpSpec
            // 
            this.grpSpec.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpSpec.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpSpec.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpSpec.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpSpec.BorderThickness = 1F;
            this.grpSpec.BorderTopOnly = false;
            this.grpSpec.Controls.Add(this.buttonWOtherInfoSet);
            this.grpSpec.Controls.Add(this.txtSpecDate);
            this.grpSpec.Controls.Add(this.btnConfirmParamModify);
            this.grpSpec.Controls.Add(this.txtSummary);
            this.grpSpec.Controls.Add(this.lblDescription);
            this.grpSpec.Controls.Add(this.txtColor);
            this.grpSpec.Controls.Add(this.lblColor);
            this.grpSpec.Controls.Add(this.txtSampleName);
            this.grpSpec.Controls.Add(this.lblSampleName);
            this.grpSpec.Controls.Add(this.lblWeight);
            this.grpSpec.Controls.Add(this.lblSpecDate);
            this.grpSpec.Controls.Add(this.txtShape);
            this.grpSpec.Controls.Add(this.lblSupplier);
            this.grpSpec.Controls.Add(this.txtSupplier);
            this.grpSpec.Controls.Add(this.lblShape);
            this.grpSpec.Controls.Add(this.txtWeight);
            this.grpSpec.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpSpec.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSpec.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpSpec.GroupImage = null;
            this.grpSpec.GroupTitle = "谱信息";
            this.grpSpec.HeaderRoundCorners = 4;
            this.grpSpec.Location = new System.Drawing.Point(0, 0);
            this.grpSpec.Margin = new System.Windows.Forms.Padding(3, 3, 3, 50);
            this.grpSpec.Name = "grpSpec";
            this.grpSpec.PaintGroupBox = false;
            this.grpSpec.RoundCorners = 4;
            this.grpSpec.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpSpec.ShadowControl = false;
            this.grpSpec.ShadowThickness = 3;
            this.grpSpec.Size = new System.Drawing.Size(551, 167);
            this.grpSpec.TabIndex = 12;
            this.grpSpec.TextLineSpace = 2;
            this.grpSpec.TitleLeftSpace = 18;
            // 
            // buttonWOtherInfoSet
            // 
            this.buttonWOtherInfoSet.bSilver = false;
            this.buttonWOtherInfoSet.Location = new System.Drawing.Point(371, 131);
            this.buttonWOtherInfoSet.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWOtherInfoSet.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWOtherInfoSet.Name = "buttonWOtherInfoSet";
            this.buttonWOtherInfoSet.Size = new System.Drawing.Size(86, 30);
            this.buttonWOtherInfoSet.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWOtherInfoSet.TabIndex = 11;
            this.buttonWOtherInfoSet.Text = "样品其他信息";
            this.buttonWOtherInfoSet.ToFocused = false;
            this.buttonWOtherInfoSet.UseVisualStyleBackColor = true;
            this.buttonWOtherInfoSet.Visible = false;
            this.buttonWOtherInfoSet.Click += new System.EventHandler(this.buttonWOtherInfoSet_Click);
            // 
            // txtSpecDate
            // 
            this.txtSpecDate.Location = new System.Drawing.Point(343, 22);
            this.txtSpecDate.Name = "txtSpecDate";
            this.txtSpecDate.Size = new System.Drawing.Size(114, 21);
            this.txtSpecDate.TabIndex = 10;
            // 
            // btnConfirmParamModify
            // 
            this.btnConfirmParamModify.bSilver = false;
            this.btnConfirmParamModify.Location = new System.Drawing.Point(478, 78);
            this.btnConfirmParamModify.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnConfirmParamModify.MenuPos = new System.Drawing.Point(0, 0);
            this.btnConfirmParamModify.Name = "btnConfirmParamModify";
            this.btnConfirmParamModify.Size = new System.Drawing.Size(56, 37);
            this.btnConfirmParamModify.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnConfirmParamModify.TabIndex = 9;
            this.btnConfirmParamModify.Text = "确认修改 ";
            this.btnConfirmParamModify.ToFocused = false;
            this.btnConfirmParamModify.UseVisualStyleBackColor = true;
            this.btnConfirmParamModify.Click += new System.EventHandler(this.buttonW1_Click);
            // 
            // txtSummary
            // 
            this.txtSummary.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtSummary.Location = new System.Drawing.Point(90, 133);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.Size = new System.Drawing.Size(263, 28);
            this.txtSummary.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtSummary.TabIndex = 8;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Location = new System.Drawing.Point(24, 136);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(29, 12);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "描述";
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.Red;
            this.txtColor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtColor.Location = new System.Drawing.Point(343, 94);
            this.txtColor.Name = "txtColor";
            this.txtColor.ReadOnly = true;
            this.txtColor.Size = new System.Drawing.Size(114, 21);
            this.txtColor.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtColor.TabIndex = 6;
            this.txtColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtColor_MouseClick);
            this.txtColor.BackColorChanged += new System.EventHandler(this.txtColor_BackColorChanged);
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.BackColor = System.Drawing.Color.Transparent;
            this.lblColor.Location = new System.Drawing.Point(248, 98);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(29, 12);
            this.lblColor.TabIndex = 5;
            this.lblColor.Text = "颜色";
            // 
            // txtSampleName
            // 
            this.txtSampleName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtSampleName.Location = new System.Drawing.Point(90, 25);
            this.txtSampleName.Name = "txtSampleName";
            this.txtSampleName.Size = new System.Drawing.Size(114, 21);
            this.txtSampleName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtSampleName.TabIndex = 4;
            // 
            // lblSampleName
            // 
            this.lblSampleName.AutoSize = true;
            this.lblSampleName.BackColor = System.Drawing.Color.Transparent;
            this.lblSampleName.Location = new System.Drawing.Point(24, 29);
            this.lblSampleName.Name = "lblSampleName";
            this.lblSampleName.Size = new System.Drawing.Size(53, 12);
            this.lblSampleName.TabIndex = 3;
            this.lblSampleName.Text = "样品名称";
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.BackColor = System.Drawing.Color.Transparent;
            this.lblWeight.Location = new System.Drawing.Point(24, 98);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(29, 12);
            this.lblWeight.TabIndex = 3;
            this.lblWeight.Text = "重量";
            // 
            // lblSpecDate
            // 
            this.lblSpecDate.AutoSize = true;
            this.lblSpecDate.BackColor = System.Drawing.Color.Transparent;
            this.lblSpecDate.Location = new System.Drawing.Point(248, 29);
            this.lblSpecDate.Name = "lblSpecDate";
            this.lblSpecDate.Size = new System.Drawing.Size(53, 12);
            this.lblSpecDate.TabIndex = 3;
            this.lblSpecDate.Text = "测量日期";
            // 
            // txtShape
            // 
            this.txtShape.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtShape.Location = new System.Drawing.Point(343, 59);
            this.txtShape.Name = "txtShape";
            this.txtShape.Size = new System.Drawing.Size(114, 21);
            this.txtShape.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtShape.TabIndex = 4;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.BackColor = System.Drawing.Color.Transparent;
            this.lblSupplier.Location = new System.Drawing.Point(24, 63);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(41, 12);
            this.lblSupplier.TabIndex = 3;
            this.lblSupplier.Text = "供应商";
            // 
            // txtSupplier
            // 
            this.txtSupplier.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtSupplier.Location = new System.Drawing.Point(90, 59);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(114, 21);
            this.txtSupplier.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtSupplier.TabIndex = 4;
            // 
            // lblShape
            // 
            this.lblShape.AutoSize = true;
            this.lblShape.BackColor = System.Drawing.Color.Transparent;
            this.lblShape.Location = new System.Drawing.Point(248, 63);
            this.lblShape.Name = "lblShape";
            this.lblShape.Size = new System.Drawing.Size(29, 12);
            this.lblShape.TabIndex = 3;
            this.lblShape.Text = "形状";
            // 
            // txtWeight
            // 
            this.txtWeight.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtWeight.Location = new System.Drawing.Point(90, 94);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(114, 21);
            this.txtWeight.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtWeight.TabIndex = 4;
            this.txtWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWeight_KeyPress);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExpand);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(17, 490);
            this.panel3.TabIndex = 40;
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExpand.bSilver = false;
            this.btnExpand.Location = new System.Drawing.Point(3, 225);
            this.btnExpand.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnExpand.MenuPos = new System.Drawing.Point(0, 0);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(14, 23);
            this.btnExpand.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnExpand.TabIndex = 39;
            this.btnExpand.Text = ">>";
            this.btnExpand.ToFocused = false;
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Controls.Add(this.btnDel);
            this.panel1.Controls.Add(this.btWSelectDemo);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(8, 498);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(778, 41);
            this.panel1.TabIndex = 25;
            // 
            // btnClear
            // 
            this.btnClear.bSilver = false;
            this.btnClear.Location = new System.Drawing.Point(662, 10);
            this.btnClear.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClear.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(84, 23);
            this.btnClear.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "清空";
            this.btnClear.ToFocused = false;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnImport
            // 
            this.btnImport.bSilver = false;
            this.btnImport.Location = new System.Drawing.Point(3, 9);
            this.btnImport.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnImport.MenuPos = new System.Drawing.Point(0, 0);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnImport.TabIndex = 19;
            this.btnImport.Text = "导入";
            this.btnImport.ToFocused = false;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Visible = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(572, 10);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(84, 23);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 13;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btWSelectDemo
            // 
            this.btWSelectDemo.bSilver = false;
            this.btWSelectDemo.Location = new System.Drawing.Point(482, 10);
            this.btWSelectDemo.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWSelectDemo.MenuPos = new System.Drawing.Point(0, 0);
            this.btWSelectDemo.Name = "btWSelectDemo";
            this.btWSelectDemo.Size = new System.Drawing.Size(84, 23);
            this.btWSelectDemo.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWSelectDemo.TabIndex = 22;
            this.btWSelectDemo.Text = "模拟";
            this.btWSelectDemo.ToFocused = false;
            this.btWSelectDemo.UseVisualStyleBackColor = true;
            this.btWSelectDemo.Visible = false;
            this.btWSelectDemo.Click += new System.EventHandler(this.btWRatioSubmit_Click);
            // 
            // btnExport
            // 
            this.btnExport.bSilver = false;
            this.btnExport.Location = new System.Drawing.Point(88, 9);
            this.btnExport.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnExport.MenuPos = new System.Drawing.Point(0, 0);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnExport.TabIndex = 20;
            this.btnExport.Text = "导出";
            this.btnExport.ToFocused = false;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Visible = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(302, 10);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.bSilver = false;
            this.btnSelect.Location = new System.Drawing.Point(392, 10);
            this.btnSelect.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSelect.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(84, 23);
            this.btnSelect.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "确定";
            this.btnSelect.ToFocused = false;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.splitContainer1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(8, 8);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(778, 490);
            this.panel4.TabIndex = 26;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SelectSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Name = "SelectSample";
            this.Size = new System.Drawing.Size(794, 547);
            this.Load += new System.EventHandler(this.SelectSample_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.grpSpec.ResumeLayout(false);
            this.grpSpec.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Skyray.Controls.ButtonW btnSelect;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.Grouper grpSpec;
        private Skyray.Controls.TextBoxW txtSampleName;
        private Skyray.Controls.LabelW lblSampleName;
        private Skyray.Controls.LabelW lblWeight;
        private Skyray.Controls.LabelW lblSpecDate;
        private Skyray.Controls.TextBoxW txtShape;
        private Skyray.Controls.LabelW lblSupplier;
        private Skyray.Controls.TextBoxW txtSupplier;
        private Skyray.Controls.LabelW lblShape;
        private Skyray.Controls.TextBoxW txtWeight;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.LabelW lblColor;
        private Skyray.Controls.TextBoxW txtColor;
        private System.Windows.Forms.ColorDialog colDlg;
        private Skyray.Controls.ButtonW btnImport;
        private Skyray.Controls.ButtonW btnExport;
        private System.Windows.Forms.FolderBrowserDialog fbdImport;
        private Skyray.Controls.Grouper grpSearch;
        private Skyray.Controls.LabelW lblTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private Skyray.Controls.CheckBoxW chkDate;
        private Skyray.Controls.CheckBoxW chkName;
        private Skyray.Controls.ButtonW btnSearch;
        private Skyray.Controls.TextBoxW txtKeywords;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private Skyray.Controls.ButtonW btWSelectDemo;
        private Skyray.Controls.CheckBoxW chkTimeOrder;
        private System.Windows.Forms.ComboBox cobNameOrder;
        private Skyray.Controls.CheckBoxW chkNameOrder;
        private System.Windows.Forms.ComboBox cobTimeOrder;
        private Skyray.Controls.LabelW lblSortType;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private Skyray.Controls.ButtonW btnExpand;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private Skyray.Controls.Extension.XRFChart xcSpec;
        private Skyray.Controls.HighPerformanceListView HighPerformanceListView1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Skyray.Controls.ButtonW btnExportbyResearch;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Skyray.Controls.ButtonW btnClear;
        private Skyray.Controls.LabelW lblDescription;
        private Skyray.Controls.ButtonW btnConfirmParamModify;
        private Skyray.Controls.TextBoxW txtSummary;
        private System.Windows.Forms.DateTimePicker txtSpecDate;
        private Skyray.Controls.ButtonW buttonWOtherInfoSet;
    }
}
