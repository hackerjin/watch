namespace Skyray.UC
{
    partial class UCPureSpecLib
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
            this.panel1 = new Skyray.Controls.XPander.Panel();
            this.dgvPureData = new Skyray.Controls.DataGridViewW();
            this.ColSampleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrentCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colElemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvwElement = new Skyray.Controls.DataGridViewW();
            this.btbDel = new Skyray.Controls.ButtonW();
            this.btnok = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioThird = new System.Windows.Forms.RadioButton();
            this.radioConic = new System.Windows.Forms.RadioButton();
            this.radioInsert = new System.Windows.Forms.RadioButton();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOutPut = new Skyray.Controls.ButtonW();
            this.btnImput = new Skyray.Controls.ButtonW();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chkShowPure = new Skyray.Controls.CheckBoxW();
            this.ColElement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpression = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPureData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwElement)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AssociatedSplitter = null;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.CaptionFont = new System.Drawing.Font("SimSun", 12.5F, System.Drawing.FontStyle.Bold);
            this.panel1.CaptionHeight = 27;
            this.panel1.Controls.Add(this.dgvPureData);
            this.panel1.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(65)))), ((int)(((byte)(118)))));
            this.panel1.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.panel1.CustomColors.CaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(164)))), ((int)(((byte)(224)))));
            this.panel1.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            this.panel1.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(222)))));
            this.panel1.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(203)))), ((int)(((byte)(136)))));
            this.panel1.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.ContentGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(190)))), ((int)(((byte)(245)))));
            this.panel1.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.panel1.CustomColors.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Image = null;
            this.panel1.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panel1.Location = new System.Drawing.Point(307, 12);
            this.panel1.MinimumSize = new System.Drawing.Size(27, 27);
            this.panel1.Name = "panel1";
            this.panel1.PanelStyle = Skyray.Controls.XPander.PanelStyle.Office2007Blue;
            this.panel1.ShowCaptionbar = false;
            this.panel1.Size = new System.Drawing.Size(610, 434);
            this.panel1.Style = Skyray.Controls.Style.Office2007Blue;
            this.panel1.TabIndex = 1;
            this.panel1.Text = "panel1";
            this.panel1.ToolTipTextCloseIcon = null;
            this.panel1.ToolTipTextExpandIconPanelCollapsed = null;
            this.panel1.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // dgvPureData
            // 
            this.dgvPureData.AllowUserToAddRows = false;
            this.dgvPureData.AllowUserToResizeRows = false;
            this.dgvPureData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvPureData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPureData.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvPureData.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvPureData.ColumnHeadersHeight = 21;
            this.dgvPureData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPureData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSampleName,
            this.colHeight,
            this.colCurrentCount,
            this.colTotalCount,
            this.colUsedTime,
            this.colElemName,
            this.colCurrent});
            this.dgvPureData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPureData.Location = new System.Drawing.Point(0, 0);
            this.dgvPureData.Name = "dgvPureData";
            this.dgvPureData.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvPureData.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvPureData.RowHeadersVisible = false;
            this.dgvPureData.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvPureData.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvPureData.RowTemplate.Height = 23;
            this.dgvPureData.SecondaryLength = 1;
            this.dgvPureData.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvPureData.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvPureData.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvPureData.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvPureData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPureData.ShowEportContextMenu = false;
            this.dgvPureData.Size = new System.Drawing.Size(610, 434);
            this.dgvPureData.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvPureData.TabIndex = 0;
            this.dgvPureData.ToPrintCols = null;
            this.dgvPureData.ToPrintRows = null;
            this.dgvPureData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPureData_CellContentClick);
            // 
            // ColSampleName
            // 
            this.ColSampleName.DataPropertyName = "Name";
            this.ColSampleName.HeaderText = "谱名称";
            this.ColSampleName.Name = "ColSampleName";
            this.ColSampleName.Width = 150;
            // 
            // colHeight
            // 
            this.colHeight.DataPropertyName = "Height";
            this.colHeight.HeaderText = "高度";
            this.colHeight.Name = "colHeight";
            this.colHeight.Width = 70;
            // 
            // colCurrentCount
            // 
            this.colCurrentCount.DataPropertyName = "CurrentUnifyCount";
            this.colCurrentCount.HeaderText = "计数";
            this.colCurrentCount.Name = "colCurrentCount";
            // 
            // colTotalCount
            // 
            this.colTotalCount.DataPropertyName = "TotalCount";
            this.colTotalCount.HeaderText = "计数率";
            this.colTotalCount.Name = "colTotalCount";
            // 
            // colUsedTime
            // 
            this.colUsedTime.DataPropertyName = "UsedTime";
            this.colUsedTime.HeaderText = "测量时间";
            this.colUsedTime.Name = "colUsedTime";
            this.colUsedTime.Visible = false;
            this.colUsedTime.Width = 80;
            // 
            // colElemName
            // 
            this.colElemName.DataPropertyName = "ElementName";
            this.colElemName.HeaderText = "元素";
            this.colElemName.Name = "colElemName";
            this.colElemName.Width = 80;
            // 
            // colCurrent
            // 
            this.colCurrent.DataPropertyName = "Current";
            this.colCurrent.HeaderText = "管流";
            this.colCurrent.Name = "colCurrent";
            // 
            // dgvwElement
            // 
            this.dgvwElement.AllowUserToAddRows = false;
            this.dgvwElement.AllowUserToResizeColumns = false;
            this.dgvwElement.AllowUserToResizeRows = false;
            this.dgvwElement.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvwElement.BackgroundColor = System.Drawing.Color.White;
            this.dgvwElement.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwElement.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwElement.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwElement.ColumnHeadersHeight = 21;
            this.dgvwElement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvwElement.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColElement,
            this.colExpression});
            this.dgvwElement.Location = new System.Drawing.Point(22, 12);
            this.dgvwElement.MultiSelect = false;
            this.dgvwElement.Name = "dgvwElement";
            this.dgvwElement.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwElement.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwElement.RowHeadersVisible = false;
            this.dgvwElement.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwElement.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwElement.RowTemplate.Height = 23;
            this.dgvwElement.SecondaryLength = 1;
            this.dgvwElement.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwElement.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwElement.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwElement.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwElement.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwElement.ShowEportContextMenu = true;
            this.dgvwElement.Size = new System.Drawing.Size(279, 434);
            this.dgvwElement.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwElement.TabIndex = 21;
            this.dgvwElement.ToPrintCols = null;
            this.dgvwElement.ToPrintRows = null;
            this.dgvwElement.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwElement_CellValueChanged);
            this.dgvwElement.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvwElement_CellBeginEdit);
            this.dgvwElement.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvwElement_CellMouseDown);
            this.dgvwElement.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwElement_CellEndEdit);
            this.dgvwElement.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvwElement_CellMouseDoubleClick);
            this.dgvwElement.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwElement_CellContentClick);
            // 
            // btbDel
            // 
            this.btbDel.bSilver = false;
            this.btbDel.Location = new System.Drawing.Point(929, 267);
            this.btbDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btbDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btbDel.Name = "btbDel";
            this.btbDel.Size = new System.Drawing.Size(92, 45);
            this.btbDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btbDel.TabIndex = 22;
            this.btbDel.Text = "删除";
            this.btbDel.ToFocused = false;
            this.btbDel.UseVisualStyleBackColor = true;
            this.btbDel.Click += new System.EventHandler(this.btbDel_Click);
            // 
            // btnok
            // 
            this.btnok.bSilver = false;
            this.btnok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnok.Location = new System.Drawing.Point(929, 330);
            this.btnok.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnok.MenuPos = new System.Drawing.Point(0, 0);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(92, 45);
            this.btnok.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnok.TabIndex = 23;
            this.btnok.Text = "确定";
            this.btnok.ToFocused = false;
            this.btnok.UseVisualStyleBackColor = true;
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(930, 396);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 45);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioThird);
            this.groupBox1.Controls.Add(this.radioConic);
            this.groupBox1.Controls.Add(this.radioInsert);
            this.groupBox1.Location = new System.Drawing.Point(863, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(54, 42);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // radioThird
            // 
            this.radioThird.AutoSize = true;
            this.radioThird.Location = new System.Drawing.Point(16, 54);
            this.radioThird.Name = "radioThird";
            this.radioThird.Size = new System.Drawing.Size(49, 17);
            this.radioThird.TabIndex = 2;
            this.radioThird.TabStop = true;
            this.radioThird.Text = "三次";
            this.radioThird.UseVisualStyleBackColor = true;
            this.radioThird.CheckedChanged += new System.EventHandler(this.radioThird_CheckedChanged);
            // 
            // radioConic
            // 
            this.radioConic.AutoSize = true;
            this.radioConic.Location = new System.Drawing.Point(15, 19);
            this.radioConic.Name = "radioConic";
            this.radioConic.Size = new System.Drawing.Size(49, 17);
            this.radioConic.TabIndex = 1;
            this.radioConic.TabStop = true;
            this.radioConic.Text = "二次";
            this.radioConic.UseVisualStyleBackColor = true;
            this.radioConic.CheckedChanged += new System.EventHandler(this.radioConic_CheckedChanged);
            // 
            // radioInsert
            // 
            this.radioInsert.AutoSize = true;
            this.radioInsert.Location = new System.Drawing.Point(64, 19);
            this.radioInsert.Name = "radioInsert";
            this.radioInsert.Size = new System.Drawing.Size(49, 17);
            this.radioInsert.TabIndex = 0;
            this.radioInsert.TabStop = true;
            this.radioInsert.Text = "插值";
            this.radioInsert.UseVisualStyleBackColor = true;
            this.radioInsert.Visible = false;
            this.radioInsert.CheckedChanged += new System.EventHandler(this.radioInsert_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(929, 206);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(92, 45);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 26;
            this.btnAdd.Text = "增加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "谱名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Height";
            this.dataGridViewTextBoxColumn2.HeaderText = "高度";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "TotalCount";
            this.dataGridViewTextBoxColumn3.HeaderText = "计数率";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "UsedTime";
            this.dataGridViewTextBoxColumn4.HeaderText = "测量时间";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "ElementName";
            this.dataGridViewTextBoxColumn5.HeaderText = "元素";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Current";
            this.dataGridViewTextBoxColumn6.HeaderText = "元素";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Current";
            this.dataGridViewTextBoxColumn7.HeaderText = "公式";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.HeaderText = "公式";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "公式";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 138;
            // 
            // btnOutPut
            // 
            this.btnOutPut.bSilver = false;
            this.btnOutPut.Location = new System.Drawing.Point(929, 146);
            this.btnOutPut.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOutPut.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOutPut.Name = "btnOutPut";
            this.btnOutPut.Size = new System.Drawing.Size(92, 45);
            this.btnOutPut.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOutPut.TabIndex = 27;
            this.btnOutPut.Text = "导出";
            this.btnOutPut.ToFocused = false;
            this.btnOutPut.UseVisualStyleBackColor = true;
            this.btnOutPut.Click += new System.EventHandler(this.btnOutPut_Click);
            // 
            // btnImput
            // 
            this.btnImput.bSilver = false;
            this.btnImput.Location = new System.Drawing.Point(929, 89);
            this.btnImput.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnImput.MenuPos = new System.Drawing.Point(0, 0);
            this.btnImput.Name = "btnImput";
            this.btnImput.Size = new System.Drawing.Size(92, 45);
            this.btnImput.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnImput.TabIndex = 28;
            this.btnImput.Text = "导入";
            this.btnImput.ToFocused = false;
            this.btnImput.UseVisualStyleBackColor = true;
            this.btnImput.Click += new System.EventHandler(this.btnImput_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Excel File(*.xls)|*.xls";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Excel File(*.xls)|*.xls";
            // 
            // chkShowPure
            // 
            this.chkShowPure.AutoSize = true;
            this.chkShowPure.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkShowPure.Location = new System.Drawing.Point(934, 36);
            this.chkShowPure.Name = "chkShowPure";
            this.chkShowPure.Size = new System.Drawing.Size(91, 17);
            this.chkShowPure.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkShowPure.TabIndex = 29;
            this.chkShowPure.Text = "checkBoxW1";
            this.chkShowPure.UseVisualStyleBackColor = true;
            // 
            // ColElement
            // 
            this.ColElement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColElement.HeaderText = "元素";
            this.ColElement.Name = "ColElement";
            this.ColElement.ReadOnly = true;
            this.ColElement.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colExpression
            // 
            this.colExpression.HeaderText = "公式";
            this.colExpression.Name = "colExpression";
            // 
            // UCPureSpecLib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkShowPure);
            this.Controls.Add(this.btnImput);
            this.Controls.Add(this.btnOutPut);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.btbDel);
            this.Controls.Add(this.dgvwElement);
            this.Controls.Add(this.panel1);
            this.Name = "UCPureSpecLib";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(1046, 491);
            this.Load += new System.EventHandler(this.UCPureSpecLib_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPureData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwElement)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.XPander.Panel panel1;
        private Skyray.Controls.DataGridViewW dgvwElement;
        private Skyray.Controls.DataGridViewW dgvPureData;
        private Skyray.Controls.ButtonW btbDel;
        private Skyray.Controls.ButtonW btnok;
        private Skyray.Controls.ButtonW btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioConic;
        private System.Windows.Forms.RadioButton radioInsert;
        private Skyray.Controls.ButtonW btnAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.RadioButton radioThird;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSampleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsedTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colElemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrent;
        private Skyray.Controls.ButtonW btnOutPut;
        private Skyray.Controls.ButtonW btnImput;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Skyray.Controls.CheckBoxW chkShowPure;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColElement;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpression;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    }
}
