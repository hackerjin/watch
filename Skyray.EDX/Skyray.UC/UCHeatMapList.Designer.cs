namespace Skyray.UC
{
    partial class UCHeatMapList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgCurveElem = new Skyray.Controls.DataGridViewW();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.dgvAllPoint = new Skyray.Controls.DataGridViewW();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.xPanderPanelList1 = new Skyray.Controls.XPander.XPanderPanelList();
            this.xPanderPanel1 = new Skyray.Controls.XPander.XPanderPanel();
            this.dgLine = new Skyray.Controls.DataGridViewW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xPanderPanel2 = new Skyray.Controls.XPander.XPanderPanel();
            this.dgCol = new Skyray.Controls.DataGridViewW();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ucTrendElem1 = new Skyray.UC.UCTrendElem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCurveElem)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllPoint)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.xPanderPanelList1.SuspendLayout();
            this.xPanderPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLine)).BeginInit();
            this.xPanderPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCol)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(8, 7);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgCurveElem);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(930, 539);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgCurveElem
            // 
            this.dgCurveElem.AllowUserToAddRows = false;
            this.dgCurveElem.AllowUserToDeleteRows = false;
            this.dgCurveElem.AllowUserToResizeRows = false;
            this.dgCurveElem.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgCurveElem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgCurveElem.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgCurveElem.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgCurveElem.ColumnHeadersHeight = 20;
            this.dgCurveElem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCurveElem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dgCurveElem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCurveElem.Location = new System.Drawing.Point(0, 0);
            this.dgCurveElem.Name = "dgCurveElem";
            this.dgCurveElem.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgCurveElem.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgCurveElem.ReadOnly = true;
            this.dgCurveElem.RowHeadersVisible = false;
            this.dgCurveElem.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgCurveElem.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgCurveElem.RowTemplate.Height = 23;
            this.dgCurveElem.SecondaryLength = 1;
            this.dgCurveElem.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgCurveElem.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgCurveElem.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgCurveElem.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgCurveElem.ShowEportContextMenu = false;
            this.dgCurveElem.Size = new System.Drawing.Size(150, 539);
            this.dgCurveElem.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgCurveElem.TabIndex = 0;
            this.dgCurveElem.ToPrintCols = null;
            this.dgCurveElem.ToPrintRows = null;
            this.dgCurveElem.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCurveElem_CellClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "元素";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
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
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(776, 539);
            this.splitContainer2.SplitterDistance = 288;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.dgvAllPoint);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel1);
            this.splitContainer3.Size = new System.Drawing.Size(776, 288);
            this.splitContainer3.SplitterDistance = 320;
            this.splitContainer3.TabIndex = 0;
            // 
            // dgvAllPoint
            // 
            this.dgvAllPoint.AllowUserToAddRows = false;
            this.dgvAllPoint.AllowUserToDeleteRows = false;
            this.dgvAllPoint.AllowUserToResizeRows = false;
            this.dgvAllPoint.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAllPoint.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvAllPoint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAllPoint.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvAllPoint.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvAllPoint.ColumnHeadersHeight = 4;
            this.dgvAllPoint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAllPoint.Location = new System.Drawing.Point(0, 0);
            this.dgvAllPoint.Name = "dgvAllPoint";
            this.dgvAllPoint.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvAllPoint.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvAllPoint.ReadOnly = true;
            this.dgvAllPoint.RowHeadersVisible = false;
            this.dgvAllPoint.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvAllPoint.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvAllPoint.RowTemplate.Height = 23;
            this.dgvAllPoint.SecondaryLength = 1;
            this.dgvAllPoint.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvAllPoint.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvAllPoint.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvAllPoint.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvAllPoint.ShowEportContextMenu = false;
            this.dgvAllPoint.Size = new System.Drawing.Size(320, 288);
            this.dgvAllPoint.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvAllPoint.TabIndex = 1;
            this.dgvAllPoint.ToPrintCols = null;
            this.dgvAllPoint.ToPrintRows = null;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 288);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.xPanderPanelList1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.ucTrendElem1);
            this.splitContainer4.Size = new System.Drawing.Size(776, 247);
            this.splitContainer4.SplitterDistance = 60;
            this.splitContainer4.TabIndex = 3;
            // 
            // xPanderPanelList1
            // 
            this.xPanderPanelList1.CaptionStyle = Skyray.Controls.XPander.CaptionStyle.Normal;
            this.xPanderPanelList1.Controls.Add(this.xPanderPanel1);
            this.xPanderPanelList1.Controls.Add(this.xPanderPanel2);
            this.xPanderPanelList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPanderPanelList1.GradientBackground = System.Drawing.Color.Empty;
            this.xPanderPanelList1.Location = new System.Drawing.Point(0, 0);
            this.xPanderPanelList1.Name = "xPanderPanelList1";
            this.xPanderPanelList1.PanelColors = null;
            this.xPanderPanelList1.Size = new System.Drawing.Size(60, 247);
            this.xPanderPanelList1.TabIndex = 1;
            this.xPanderPanelList1.Text = "xPanderPanelList1";
            // 
            // xPanderPanel1
            // 
            this.xPanderPanel1.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanel1.Controls.Add(this.dgLine);
            this.xPanderPanel1.CustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.xPanderPanel1.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xPanderPanel1.CustomColors.CaptionCheckedGradientBegin = System.Drawing.Color.Empty;
            this.xPanderPanel1.CustomColors.CaptionCheckedGradientEnd = System.Drawing.Color.Empty;
            this.xPanderPanel1.CustomColors.CaptionCheckedGradientMiddle = System.Drawing.Color.Empty;
            this.xPanderPanel1.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanel1.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.xPanderPanel1.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanel1.CustomColors.CaptionPressedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(188)))), ((int)(((byte)(235)))));
            this.xPanderPanel1.CustomColors.CaptionPressedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(188)))), ((int)(((byte)(235)))));
            this.xPanderPanel1.CustomColors.CaptionPressedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(188)))), ((int)(((byte)(235)))));
            this.xPanderPanel1.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(215)))), ((int)(((byte)(243)))));
            this.xPanderPanel1.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(215)))), ((int)(((byte)(243)))));
            this.xPanderPanel1.CustomColors.CaptionSelectedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(215)))), ((int)(((byte)(243)))));
            this.xPanderPanel1.CustomColors.CaptionSelectedText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.CustomColors.FlatCaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanel1.CustomColors.FlatCaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanel1.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.xPanderPanel1.Expand = true;
            this.xPanderPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel1.Image = null;
            this.xPanderPanel1.Name = "xPanderPanel1";
            this.xPanderPanel1.PanelStyle = Skyray.Controls.XPander.PanelStyle.Default;
            this.xPanderPanel1.Size = new System.Drawing.Size(60, 222);
            this.xPanderPanel1.Style = Skyray.Controls.Style.Office2007Blue;
            this.xPanderPanel1.TabIndex = 0;
            this.xPanderPanel1.Text = "行";
            this.xPanderPanel1.ToolTipTextCloseIcon = null;
            this.xPanderPanel1.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanel1.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // dgLine
            // 
            this.dgLine.AllowUserToAddRows = false;
            this.dgLine.AllowUserToDeleteRows = false;
            this.dgLine.AllowUserToResizeRows = false;
            this.dgLine.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgLine.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgLine.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgLine.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgLine.ColumnHeadersHeight = 4;
            this.dgLine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLine.ColumnHeadersVisible = false;
            this.dgLine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dgLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgLine.Location = new System.Drawing.Point(1, 25);
            this.dgLine.Name = "dgLine";
            this.dgLine.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgLine.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgLine.ReadOnly = true;
            this.dgLine.RowHeadersVisible = false;
            this.dgLine.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgLine.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgLine.RowTemplate.Height = 23;
            this.dgLine.SecondaryLength = 1;
            this.dgLine.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgLine.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgLine.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgLine.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgLine.ShowEportContextMenu = false;
            this.dgLine.Size = new System.Drawing.Size(58, 197);
            this.dgLine.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgLine.TabIndex = 0;
            this.dgLine.ToPrintCols = null;
            this.dgLine.ToPrintRows = null;
            this.dgLine.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgLine_CellClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // xPanderPanel2
            // 
            this.xPanderPanel2.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanel2.Controls.Add(this.dgCol);
            this.xPanderPanel2.CustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.xPanderPanel2.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xPanderPanel2.CustomColors.CaptionCheckedGradientBegin = System.Drawing.Color.Empty;
            this.xPanderPanel2.CustomColors.CaptionCheckedGradientEnd = System.Drawing.Color.Empty;
            this.xPanderPanel2.CustomColors.CaptionCheckedGradientMiddle = System.Drawing.Color.Empty;
            this.xPanderPanel2.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanel2.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.xPanderPanel2.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanel2.CustomColors.CaptionPressedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(188)))), ((int)(((byte)(235)))));
            this.xPanderPanel2.CustomColors.CaptionPressedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(188)))), ((int)(((byte)(235)))));
            this.xPanderPanel2.CustomColors.CaptionPressedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(188)))), ((int)(((byte)(235)))));
            this.xPanderPanel2.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(215)))), ((int)(((byte)(243)))));
            this.xPanderPanel2.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(215)))), ((int)(((byte)(243)))));
            this.xPanderPanel2.CustomColors.CaptionSelectedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(215)))), ((int)(((byte)(243)))));
            this.xPanderPanel2.CustomColors.CaptionSelectedText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.CustomColors.FlatCaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanel2.CustomColors.FlatCaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanel2.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.xPanderPanel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanel2.Image = null;
            this.xPanderPanel2.Name = "xPanderPanel2";
            this.xPanderPanel2.PanelStyle = Skyray.Controls.XPander.PanelStyle.Default;
            this.xPanderPanel2.Size = new System.Drawing.Size(60, 25);
            this.xPanderPanel2.Style = Skyray.Controls.Style.Office2007Blue;
            this.xPanderPanel2.TabIndex = 1;
            this.xPanderPanel2.Text = "列";
            this.xPanderPanel2.ToolTipTextCloseIcon = null;
            this.xPanderPanel2.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanel2.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // dgCol
            // 
            this.dgCol.AllowUserToAddRows = false;
            this.dgCol.AllowUserToDeleteRows = false;
            this.dgCol.AllowUserToResizeRows = false;
            this.dgCol.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgCol.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgCol.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgCol.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgCol.ColumnHeadersHeight = 4;
            this.dgCol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCol.ColumnHeadersVisible = false;
            this.dgCol.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2});
            this.dgCol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCol.Location = new System.Drawing.Point(1, 25);
            this.dgCol.Name = "dgCol";
            this.dgCol.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgCol.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgCol.ReadOnly = true;
            this.dgCol.RowHeadersVisible = false;
            this.dgCol.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgCol.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgCol.RowTemplate.Height = 23;
            this.dgCol.SecondaryLength = 1;
            this.dgCol.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgCol.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgCol.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgCol.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgCol.ShowEportContextMenu = false;
            this.dgCol.Size = new System.Drawing.Size(58, 0);
            this.dgCol.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgCol.TabIndex = 1;
            this.dgCol.ToPrintCols = null;
            this.dgCol.ToPrintRows = null;
            this.dgCol.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCol_CellClick);
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // ucTrendElem1
            // 
            this.ucTrendElem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.ucTrendElem1.ColsH = 0;
            this.ucTrendElem1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTrendElem1.Location = new System.Drawing.Point(0, 0);
            this.ucTrendElem1.Name = "ucTrendElem1";
            this.ucTrendElem1.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ucTrendElem1.RowsH = 0;
            this.ucTrendElem1.Size = new System.Drawing.Size(712, 247);
            this.ucTrendElem1.TabIndex = 1;
            // 
            // UCHeatMapList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.splitContainer1);
            this.Name = "UCHeatMapList";
            this.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Size = new System.Drawing.Size(946, 553);
            this.Load += new System.EventHandler(this.UCHeatMapList_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCurveElem)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllPoint)).EndInit();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.xPanderPanelList1.ResumeLayout(false);
            this.xPanderPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgLine)).EndInit();
            this.xPanderPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.SplitContainer splitContainer2;
        private Skyray.Controls.DataGridViewW dgCurveElem;
        public System.Windows.Forms.SplitContainer splitContainer3;
        private Skyray.Controls.DataGridViewW dgvAllPoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.SplitContainer splitContainer4;
        private UCTrendElem ucTrendElem1;
        private Skyray.Controls.XPander.XPanderPanelList xPanderPanelList1;
        private Skyray.Controls.XPander.XPanderPanel xPanderPanel1;
        private Skyray.Controls.DataGridViewW dgLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private Skyray.Controls.XPander.XPanderPanel xPanderPanel2;
        private Skyray.Controls.DataGridViewW dgCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;

    }
}
