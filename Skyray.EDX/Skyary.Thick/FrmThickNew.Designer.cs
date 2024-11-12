namespace Skyray.Thick
{
    partial class FrmThickNew
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Skyray.Controls.Office2007Renderer office2007Renderer3 = new Skyray.Controls.Office2007Renderer();
            Skyray.Controls.Office2007Renderer office2007Renderer2 = new Skyray.Controls.Office2007Renderer();
            Skyray.Controls.Office2007Renderer office2007Renderer1 = new Skyray.Controls.Office2007Renderer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmThickNew));
            this.statusStripW1 = new Skyray.Controls.StatusStripW();
            this.toolStripStatusLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslCircle = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssUsedTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslabelMeasureTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsTimeProcessBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tssRemainTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabelSuplusTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.containerObject1 = new Skyray.EDX.Common.ContainerObject();
            this.containerObject3 = new Skyray.EDX.Common.ContainerObject();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControlCurve = new Skyray.Controls.TabControlW();
            this.tbWorkCurve = new System.Windows.Forms.TabPage();
            this.dgvWorkCurve = new Skyray.Controls.DataGridViewW();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.containerObject4 = new Skyray.EDX.Common.ContainerObject();
            this.tabControlInfo = new Skyray.Controls.TabControlW();
            this.tabPageDevice = new System.Windows.Forms.TabPage();
            this.dgvDevice = new Skyray.Controls.DataGridViewW();
            this.ColumnDeviceLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDeviceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbpSpec = new System.Windows.Forms.TabPage();
            this.xrfChart1 = new Skyray.Controls.Extension.XRFChart();
            this.ucCameraControl1 = new Skyray.UC.UCCameraControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.lblShowWorkCurveName = new Skyray.Controls.LabelW();
            this.tabControlW3 = new Skyray.Controls.TabControlW();
            this.tabPageMeasureResult = new System.Windows.Forms.TabPage();
            this.dgvMeasure = new Skyray.Controls.DataGridViewW();
            this.tabPageStatics = new System.Windows.Forms.TabPage();
            this.dgvStatics = new Skyray.Controls.DataGridViewW();
            this.ucHistoryRecord1 = new Skyray.UC.UCHistoryRecord();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStripW1.SuspendLayout();
            this.containerObject1.SuspendLayout();
            this.containerObject3.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControlCurve.SuspendLayout();
            this.tbWorkCurve.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkCurve)).BeginInit();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.containerObject4.SuspendLayout();
            this.tabControlInfo.SuspendLayout();
            this.tabPageDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).BeginInit();
            this.tbpSpec.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.tabControlW3.SuspendLayout();
            this.tabPageMeasureResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasure)).BeginInit();
            this.tabPageStatics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatics)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStripW1
            // 
            this.statusStripW1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelStatus,
            this.tslCircle,
            this.tssUsedTime,
            this.tslabelMeasureTime,
            this.tsTimeProcessBar,
            this.tssRemainTime,
            this.tsLabelSuplusTime});
            this.statusStripW1.Location = new System.Drawing.Point(0, 628);
            this.statusStripW1.Name = "statusStripW1";
            this.statusStripW1.Size = new System.Drawing.Size(1016, 23);
            this.statusStripW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.statusStripW1.TabIndex = 2;
            this.statusStripW1.Text = "statusStripW1";
            this.statusStripW1.TTT = null;
            // 
            // toolStripStatusLabelStatus
            // 
            this.toolStripStatusLabelStatus.Name = "toolStripStatusLabelStatus";
            this.toolStripStatusLabelStatus.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.toolStripStatusLabelStatus.Size = new System.Drawing.Size(100, 18);
            this.toolStripStatusLabelStatus.Text = "设备连接状态";
            // 
            // tslCircle
            // 
            this.tslCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tslCircle.Image = global::Skyray.Thick.Properties.Resources.xsimple_004_red;
            this.tslCircle.Name = "tslCircle";
            this.tslCircle.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.tslCircle.Size = new System.Drawing.Size(36, 18);
            this.tslCircle.Text = "toolStripStatusLabel3";
            // 
            // tssUsedTime
            // 
            this.tssUsedTime.Name = "tssUsedTime";
            this.tssUsedTime.Size = new System.Drawing.Size(68, 18);
            this.tssUsedTime.Text = "测量时间：";
            // 
            // tslabelMeasureTime
            // 
            this.tslabelMeasureTime.Name = "tslabelMeasureTime";
            this.tslabelMeasureTime.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.tslabelMeasureTime.Size = new System.Drawing.Size(41, 18);
            this.tslabelMeasureTime.Text = "0s";
            // 
            // tsTimeProcessBar
            // 
            this.tsTimeProcessBar.Name = "tsTimeProcessBar";
            this.tsTimeProcessBar.Size = new System.Drawing.Size(650, 17);
            // 
            // tssRemainTime
            // 
            this.tssRemainTime.Name = "tssRemainTime";
            this.tssRemainTime.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.tssRemainTime.Size = new System.Drawing.Size(78, 18);
            this.tssRemainTime.Text = "剩余时间：";
            // 
            // tsLabelSuplusTime
            // 
            this.tsLabelSuplusTime.Name = "tsLabelSuplusTime";
            this.tsLabelSuplusTime.Size = new System.Drawing.Size(21, 18);
            this.tsLabelSuplusTime.Text = "0s";

            // 
            // ucCameraControl1
            // 
            this.ucCameraControl1.AutoScroll = true;
            this.ucCameraControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ucCameraControl1.CurrentDecvice = null;
            this.ucCameraControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucCameraControl1.Location = new System.Drawing.Point(0, 0);
            this.ucCameraControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ucCameraControl1.Name = "ucCameraControl1";
            this.ucCameraControl1.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.ucCameraControl1.ShowMenu = false;
            this.ucCameraControl1.Size = new System.Drawing.Size(250, 224);
            this.ucCameraControl1.TabIndex = 3;
            // 
            // containerObject1
            // 
            this.containerObject1.AutoScroll = true;
            this.containerObject1.BigImage = null;
            this.containerObject1.ContainerAttribute = false;
            this.containerObject1.ContainerLabel = null;
            this.containerObject1.ControlInternal = 0;
            this.containerObject1.Controls.Add(this.containerObject3);
            this.containerObject1.CurrentPanelIndex = 0;
            this.containerObject1.CurrentPlanningNumber = 0;
            this.containerObject1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerObject1.IncludeInnerCoordinate = false;
            this.containerObject1.IsReverseEmbeded = false;
            this.containerObject1.Location = new System.Drawing.Point(0, 0);
            this.containerObject1.Name = "containerObject1";
            this.containerObject1.Name1 = null;
            office2007Renderer3.RoundedEdges = true;
            this.containerObject1.Renderer = office2007Renderer3;
            this.containerObject1.Size = new System.Drawing.Size(1016, 628);
            this.containerObject1.SmallImage = null;
            this.containerObject1.Style = Skyray.Controls.Style.Custom;
            this.containerObject1.TabIndex = 1;
            // 
            // containerObject3
            // 
            this.containerObject3.AutoScroll = true;
            this.containerObject3.BigImage = null;
            this.containerObject3.ContainerAttribute = false;
            this.containerObject3.ContainerLabel = null;
            this.containerObject3.ControlInternal = 0;
            this.containerObject3.Controls.Add(this.splitContainer1);
            this.containerObject3.CurrentPanelIndex = 0;
            this.containerObject3.CurrentPlanningNumber = 0;
            this.containerObject3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerObject3.IncludeInnerCoordinate = false;
            this.containerObject3.IsReverseEmbeded = false;
            this.containerObject3.Location = new System.Drawing.Point(0, 0);
            this.containerObject3.Name = "containerObject3";
            this.containerObject3.Name1 = null;
            office2007Renderer2.RoundedEdges = true;
            this.containerObject3.Renderer = office2007Renderer2;
            this.containerObject3.Size = new System.Drawing.Size(1016, 628);
            this.containerObject3.SmallImage = null;
            this.containerObject3.Style = Skyray.Controls.Style.Custom;
            this.containerObject3.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1016, 628);
            this.splitContainer1.SplitterDistance = 590;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControlCurve);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(590, 628);
            this.splitContainer3.SplitterDistance = 196;
            this.splitContainer3.TabIndex = 0;
            // 
            // tabControlCurve
            // 
            this.tabControlCurve.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabControlCurve.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.tabControlCurve.Controls.Add(this.tbWorkCurve);
            this.tabControlCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCurve.Location = new System.Drawing.Point(0, 0);
            this.tabControlCurve.Name = "tabControlCurve";
            this.tabControlCurve.SelectedIndex = 0;
            this.tabControlCurve.ShowTabs = true;
            this.tabControlCurve.Size = new System.Drawing.Size(196, 628);
            this.tabControlCurve.Style = Skyray.Controls.Style.Office2007Blue;
            this.tabControlCurve.TabIndex = 2;
            // 
            // tbWorkCurve
            // 
            this.tbWorkCurve.Controls.Add(this.dgvWorkCurve);
            this.tbWorkCurve.Location = new System.Drawing.Point(4, 26);
            this.tbWorkCurve.Name = "tbWorkCurve";
            this.tbWorkCurve.Padding = new System.Windows.Forms.Padding(3);
            this.tbWorkCurve.Size = new System.Drawing.Size(188, 598);
            this.tbWorkCurve.TabIndex = 0;
            this.tbWorkCurve.Text = "工作曲线";
            this.tbWorkCurve.UseVisualStyleBackColor = true;
            // 
            // dgvWorkCurve
            // 
            this.dgvWorkCurve.AllowUserToAddRows = false;
            this.dgvWorkCurve.AllowUserToResizeRows = false;
            this.dgvWorkCurve.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvWorkCurve.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvWorkCurve.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvWorkCurve.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvWorkCurve.ColumnHeadersHeight = 4;
            this.dgvWorkCurve.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkCurve.ColumnHeadersVisible = false;
            this.dgvWorkCurve.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dgvWorkCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWorkCurve.Location = new System.Drawing.Point(3, 3);
            this.dgvWorkCurve.MultiSelect = false;
            this.dgvWorkCurve.Name = "dgvWorkCurve";
            this.dgvWorkCurve.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvWorkCurve.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvWorkCurve.RowHeadersVisible = false;
            this.dgvWorkCurve.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvWorkCurve.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvWorkCurve.RowTemplate.Height = 23;
            this.dgvWorkCurve.SecondaryLength = 1;
            this.dgvWorkCurve.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvWorkCurve.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvWorkCurve.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvWorkCurve.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvWorkCurve.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWorkCurve.ShowEportContextMenu = false;
            this.dgvWorkCurve.Size = new System.Drawing.Size(182, 592);
            this.dgvWorkCurve.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvWorkCurve.TabIndex = 0;
            this.dgvWorkCurve.ToPrintCols = null;
            this.dgvWorkCurve.ToPrintRows = null;
            this.dgvWorkCurve.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWorkCurve_CellClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer3_Panel1_Paint);
            // 
            // splitContainer4.Panel2
            // 
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.ucCameraControl1);
            this.splitContainer4.Panel2.Controls.Add(this.containerObject4);
            this.splitContainer4.Size = new System.Drawing.Size(390, 628);
            this.splitContainer4.SplitterDistance = 260;
            this.splitContainer4.TabIndex = 1;


            // 
            // containerObject4
            // 
            this.containerObject4.AutoScroll = true;
            this.containerObject4.BigImage = null;
            this.containerObject4.ContainerAttribute = false;
            this.containerObject4.ContainerLabel = null;
            this.containerObject4.ControlInternal = 0;
            this.containerObject4.Controls.Add(this.tabControlInfo);
            this.containerObject4.CurrentPanelIndex = 0;
            this.containerObject4.CurrentPlanningNumber = 0;
            this.containerObject4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerObject4.IncludeInnerCoordinate = false;
            this.containerObject4.IsReverseEmbeded = false;
            this.containerObject4.Location = new System.Drawing.Point(0, 0);
            this.containerObject4.Name = "containerObject4";
            this.containerObject4.Name1 = null;
            office2007Renderer1.RoundedEdges = true;
            this.containerObject4.Renderer = office2007Renderer1;
            this.containerObject4.Size = new System.Drawing.Size(390, 364);
            this.containerObject4.SmallImage = null;
            this.containerObject4.Style = Skyray.Controls.Style.Custom;
            this.containerObject4.TabIndex = 2;
            // 
            // tabControlInfo
            // 
            this.tabControlInfo.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabControlInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.tabControlInfo.Controls.Add(this.tabPageDevice);
            this.tabControlInfo.Controls.Add(this.tbpSpec);
            this.tabControlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlInfo.Location = new System.Drawing.Point(0, 0);
            this.tabControlInfo.Name = "tabControlInfo";
            this.tabControlInfo.SelectedIndex = 0;
            this.tabControlInfo.ShowTabs = true;
            this.tabControlInfo.Size = new System.Drawing.Size(390, 364);
            this.tabControlInfo.Style = Skyray.Controls.Style.Office2007Blue;
            this.tabControlInfo.TabIndex = 2;
            // 
            // tabPageDevice
            // 
            this.tabPageDevice.Controls.Add(this.dgvDevice);
            this.tabPageDevice.Location = new System.Drawing.Point(4, 26);
            this.tabPageDevice.Name = "tabPageDevice";
            this.tabPageDevice.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDevice.Size = new System.Drawing.Size(382, 334);
            this.tabPageDevice.TabIndex = 1;
            this.tabPageDevice.Text = "反馈信息";
            this.tabPageDevice.UseVisualStyleBackColor = true;
            // 
            // dgvDevice
            // 
            this.dgvDevice.AllowUserToResizeRows = false;
            this.dgvDevice.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvDevice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDevice.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvDevice.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvDevice.ColumnHeadersHeight = 4;
            this.dgvDevice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevice.ColumnHeadersVisible = false;
            this.dgvDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDeviceLabel,
            this.ColumnDeviceValue});
            this.dgvDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevice.Location = new System.Drawing.Point(3, 3);
            this.dgvDevice.Name = "dgvDevice";
            this.dgvDevice.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvDevice.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvDevice.ReadOnly = true;
            this.dgvDevice.RowHeadersVisible = false;
            this.dgvDevice.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvDevice.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvDevice.RowTemplate.Height = 23;
            this.dgvDevice.SecondaryLength = 1;
            this.dgvDevice.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvDevice.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvDevice.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvDevice.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvDevice.ShowEportContextMenu = false;
            this.dgvDevice.Size = new System.Drawing.Size(376, 328);
            this.dgvDevice.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvDevice.TabIndex = 1;
            this.dgvDevice.ToPrintCols = null;
            this.dgvDevice.ToPrintRows = null;
            // 
            // ColumnDeviceLabel
            // 
            this.ColumnDeviceLabel.HeaderText = "Column1";
            this.ColumnDeviceLabel.Name = "ColumnDeviceLabel";
            this.ColumnDeviceLabel.ReadOnly = true;
            // 
            // ColumnDeviceValue
            // 
            this.ColumnDeviceValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnDeviceValue.HeaderText = "Column1";
            this.ColumnDeviceValue.Name = "ColumnDeviceValue";
            this.ColumnDeviceValue.ReadOnly = true;
            // 
            // tbpSpec
            // 
            this.tbpSpec.Controls.Add(this.xrfChart1);
            this.tbpSpec.Location = new System.Drawing.Point(4, 26);
            this.tbpSpec.Name = "tbpSpec";
            this.tbpSpec.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSpec.Size = new System.Drawing.Size(382, 334);
            this.tbpSpec.TabIndex = 3;
            this.tbpSpec.Text = "谱图";
            this.tbpSpec.UseVisualStyleBackColor = true;
            // 
            // xrfChart1
            // 
            this.xrfChart1.BoundaryElement = null;
            this.xrfChart1.BShowElement = false;
            this.xrfChart1.ChangeSpeed = 0.025;
            this.xrfChart1.Coeff = null;
            this.xrfChart1.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.YellowGreen,
        System.Drawing.Color.YellowGreen};
            this.xrfChart1.CurrentChannelCount = 0;
            this.xrfChart1.currentInterestArea = null;
            this.xrfChart1.Cursor = System.Windows.Forms.Cursors.Default;
            this.xrfChart1.DefaultLineHeight = 30;
            this.xrfChart1.DEnergy = 0;
            this.xrfChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xrfChart1.EditButtons = System.Windows.Forms.MouseButtons.Right;
            this.xrfChart1.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.xrfChart1.EnableMoveLine = true;
            this.xrfChart1.EnableWheel = true;
            this.xrfChart1.FontSize = 12;
            this.xrfChart1.HorizontalSynchronization = true;
            this.xrfChart1.ICurrentChannel = 0;
            this.xrfChart1.InterestAreaEnable = false;
            this.xrfChart1.IsActiveMove = true;
            this.xrfChart1.IsEnableWheelZoom = false;
            this.xrfChart1.IsMainElement = false;
            this.xrfChart1.IsShowPeakFlagAuto = false;
            this.xrfChart1.IsUseScroll = true;
            this.xrfChart1.IUseBase = false;
            this.xrfChart1.IUseBoundary = false;
            this.xrfChart1.IXMaxChannel = 2048;
            this.xrfChart1.IYMaxChannel = 1000;
            this.xrfChart1.IZero = false;
            this.xrfChart1.LinkButtons = System.Windows.Forms.MouseButtons.Left;
            this.xrfChart1.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.xrfChart1.Location = new System.Drawing.Point(3, 3);
            this.xrfChart1.MultiGraph = false;
            this.xrfChart1.Name = "xrfChart1";
            this.xrfChart1.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.xrfChart1.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            this.xrfChart1.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)));
            this.xrfChart1.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            this.xrfChart1.Positions = new float[] {
        0F,
        1F};
            this.xrfChart1.ScrollGrace = 0;
            this.xrfChart1.ScrollMaxX = 0;
            this.xrfChart1.ScrollMaxY = 0;
            this.xrfChart1.ScrollMaxY2 = 0;
            this.xrfChart1.ScrollMinX = 0;
            this.xrfChart1.ScrollMinY = 0;
            this.xrfChart1.ScrollMinY2 = 0;
            this.xrfChart1.SelectButtons = System.Windows.Forms.MouseButtons.Left;
            this.xrfChart1.SelectModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.xrfChart1.Size = new System.Drawing.Size(376, 328);
            this.xrfChart1.TabIndex = 1;
            this.xrfChart1.UnSpecing = true;
            this.xrfChart1.VerticalSynchronization = true;
            this.xrfChart1.XTitle = "";
            this.xrfChart1.YTitle = "";
            this.xrfChart1.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
            this.xrfChart1.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            this.xrfChart1.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            this.xrfChart1.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ucHistoryRecord1);
            this.splitContainer2.Size = new System.Drawing.Size(422, 628);
            this.splitContainer2.SplitterDistance = 219;
            this.splitContainer2.TabIndex = 1;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.lblShowWorkCurveName);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.tabControlW3);
            this.splitContainer5.Size = new System.Drawing.Size(422, 219);
            this.splitContainer5.SplitterDistance = 64;
            this.splitContainer5.TabIndex = 2;
            // 
            // lblShowWorkCurveName
            // 
            this.lblShowWorkCurveName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.lblShowWorkCurveName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShowWorkCurveName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShowWorkCurveName.Location = new System.Drawing.Point(0, 0);
            this.lblShowWorkCurveName.Name = "lblShowWorkCurveName";
            this.lblShowWorkCurveName.Size = new System.Drawing.Size(422, 64);
            this.lblShowWorkCurveName.TabIndex = 0;
            this.lblShowWorkCurveName.Text = "WorkCurveName";
            this.lblShowWorkCurveName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControlW3
            // 
            this.tabControlW3.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabControlW3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.tabControlW3.Controls.Add(this.tabPageMeasureResult);
            this.tabControlW3.Controls.Add(this.tabPageStatics);
            this.tabControlW3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlW3.Location = new System.Drawing.Point(0, 0);
            this.tabControlW3.Name = "tabControlW3";
            this.tabControlW3.SelectedIndex = 0;
            this.tabControlW3.ShowTabs = true;
            this.tabControlW3.Size = new System.Drawing.Size(422, 151);
            this.tabControlW3.Style = Skyray.Controls.Style.Office2007Blue;
            this.tabControlW3.TabIndex = 6;
            // 
            // tabPageMeasureResult
            // 
            this.tabPageMeasureResult.Controls.Add(this.dgvMeasure);
            this.tabPageMeasureResult.Location = new System.Drawing.Point(4, 26);
            this.tabPageMeasureResult.Name = "tabPageMeasureResult";
            this.tabPageMeasureResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMeasureResult.Size = new System.Drawing.Size(414, 121);
            this.tabPageMeasureResult.TabIndex = 0;
            this.tabPageMeasureResult.Text = "测量结果";
            this.tabPageMeasureResult.UseVisualStyleBackColor = true;
            // 
            // dgvMeasure
            // 
            this.dgvMeasure.AllowUserToResizeRows = false;
            this.dgvMeasure.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvMeasure.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMeasure.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvMeasure.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvMeasure.ColumnHeadersHeight = 4;
            this.dgvMeasure.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMeasure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMeasure.Location = new System.Drawing.Point(3, 3);
            this.dgvMeasure.Name = "dgvMeasure";
            this.dgvMeasure.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvMeasure.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvMeasure.ReadOnly = true;
            this.dgvMeasure.RowHeadersVisible = false;
            this.dgvMeasure.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvMeasure.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvMeasure.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvMeasure.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvMeasure.RowTemplate.Height = 33;
            this.dgvMeasure.SecondaryLength = 1;
            this.dgvMeasure.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvMeasure.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvMeasure.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvMeasure.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvMeasure.ShowEportContextMenu = false;
            this.dgvMeasure.Size = new System.Drawing.Size(408, 115);
            this.dgvMeasure.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvMeasure.TabIndex = 1;
            this.dgvMeasure.ToPrintCols = null;
            this.dgvMeasure.ToPrintRows = null;
            this.dgvMeasure.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMeasure_CellClick);
            // 
            // tabPageStatics
            // 
            this.tabPageStatics.Controls.Add(this.dgvStatics);
            this.tabPageStatics.Location = new System.Drawing.Point(4, 26);
            this.tabPageStatics.Name = "tabPageStatics";
            this.tabPageStatics.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStatics.Size = new System.Drawing.Size(414, 121);
            this.tabPageStatics.TabIndex = 1;
            this.tabPageStatics.Text = "统计信息";
            this.tabPageStatics.UseVisualStyleBackColor = true;
            // 
            // dgvStatics
            // 
            this.dgvStatics.AllowUserToResizeRows = false;
            this.dgvStatics.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvStatics.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStatics.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvStatics.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvStatics.ColumnHeadersHeight = 4;
            this.dgvStatics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStatics.Location = new System.Drawing.Point(3, 3);
            this.dgvStatics.Name = "dgvStatics";
            this.dgvStatics.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvStatics.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvStatics.ReadOnly = true;
            this.dgvStatics.RowHeadersVisible = false;
            this.dgvStatics.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvStatics.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvStatics.RowTemplate.Height = 23;
            this.dgvStatics.SecondaryLength = 1;
            this.dgvStatics.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvStatics.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvStatics.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvStatics.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvStatics.ShowEportContextMenu = false;
            this.dgvStatics.Size = new System.Drawing.Size(408, 115);
            this.dgvStatics.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvStatics.TabIndex = 0;
            this.dgvStatics.ToPrintCols = null;
            this.dgvStatics.ToPrintRows = null;
            // 
            // ucHistoryRecord1
            // 
            this.ucHistoryRecord1.BackColor = System.Drawing.Color.GhostWhite;
            this.ucHistoryRecord1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHistoryRecord1.Location = new System.Drawing.Point(0, 0);
            this.ucHistoryRecord1.Name = "ucHistoryRecord1";
            this.ucHistoryRecord1.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.ucHistoryRecord1.Size = new System.Drawing.Size(422, 405);
            this.ucHistoryRecord1.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // FrmThickNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 651);
            this.Controls.Add(this.containerObject1);
            this.Controls.Add(this.statusStripW1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmThickNew";
            this.Text = "Thick";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Deactivate += new System.EventHandler(this.FrmThick_Deactivate);
            this.Load += new System.EventHandler(this.FrmThick_Load);
            this.SizeChanged += new System.EventHandler(this.FrmThick_SizeChanged);
            this.Shown += new System.EventHandler(this.FrmThick_Shown);
            this.Activated += new System.EventHandler(this.FrmThick_Activated);
            this.Leave += new System.EventHandler(this.FrmThick_Leave);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmThick_FormClosing);
            this.statusStripW1.ResumeLayout(false);
            this.statusStripW1.PerformLayout();
            this.containerObject1.ResumeLayout(false);
            this.containerObject3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.tabControlCurve.ResumeLayout(false);
            this.tbWorkCurve.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkCurve)).EndInit();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.containerObject4.ResumeLayout(false);
            this.tabControlInfo.ResumeLayout(false);
            this.tabPageDevice.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).EndInit();
            this.tbpSpec.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.ResumeLayout(false);
            this.tabControlW3.ResumeLayout(false);
            this.tabPageMeasureResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasure)).EndInit();
            this.tabPageStatics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatics)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.EDX.Common.ContainerObject containerObject1;
        private Skyray.Controls.StatusStripW statusStripW1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStatus;
        private System.Windows.Forms.ToolStripStatusLabel tslCircle;
        private System.Windows.Forms.ToolStripStatusLabel tssUsedTime;
        private System.Windows.Forms.ToolStripStatusLabel tslabelMeasureTime;
        private System.Windows.Forms.ToolStripProgressBar tsTimeProcessBar;
        private System.Windows.Forms.ToolStripStatusLabel tssRemainTime;
        private System.Windows.Forms.ToolStripStatusLabel tsLabelSuplusTime;
        private Skyray.EDX.Common.ContainerObject containerObject3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private Skyray.Controls.TabControlW tabControlW3;
        private System.Windows.Forms.TabPage tabPageMeasureResult;
        private Skyray.Controls.DataGridViewW dgvMeasure;
        private System.Windows.Forms.TabPage tabPageStatics;
        private Skyray.Controls.DataGridViewW dgvStatics;
        private Skyray.Controls.LabelW lblShowWorkCurveName;
        private Skyray.UC.UCCameraControl ucCameraControl1;
        private Skyray.UC.UCHistoryRecord ucHistoryRecord1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Skyray.Controls.TabControlW tabControlCurve;
        private System.Windows.Forms.TabPage tbWorkCurve;
        private Skyray.Controls.DataGridViewW dgvWorkCurve;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private Skyray.EDX.Common.ContainerObject containerObject4;
        private Skyray.Controls.TabControlW tabControlInfo;
        private System.Windows.Forms.TabPage tabPageDevice;
        private Skyray.Controls.DataGridViewW dgvDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeviceLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeviceValue;
        private System.Windows.Forms.TabPage tbpSpec;
        private Skyray.Controls.Extension.XRFChart xrfChart1;
    }
}