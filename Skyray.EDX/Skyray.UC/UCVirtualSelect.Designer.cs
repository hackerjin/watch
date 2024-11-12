namespace Skyray.UC
{
    partial class UCVirtualSelect
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
            this.btnSubmitRate = new Skyray.Controls.ButtonW();
            this.btnDelete = new Skyray.Controls.ButtonW();
            this.btnNew = new Skyray.Controls.ButtonW();
            this.panel1 = new Skyray.Controls.XPander.Panel();
            this.dataGridVirtual = new Skyray.Controls.DataGridViewW();
            this.SpecListName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TubVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TubCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataCellFilter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnRatioVisual = new Skyray.Controls.ButtonW();
            this.btnCacel = new Skyray.Controls.ButtonW();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVirtual)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSubmitRate
            // 
            this.btnSubmitRate.bSilver = false;
            this.btnSubmitRate.Location = new System.Drawing.Point(763, 187);
            this.btnSubmitRate.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmitRate.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmitRate.Name = "btnSubmitRate";
            this.btnSubmitRate.Size = new System.Drawing.Size(99, 27);
            this.btnSubmitRate.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmitRate.TabIndex = 4;
            this.btnSubmitRate.Text = "比例对比谱";
            this.btnSubmitRate.ToFocused = false;
            this.btnSubmitRate.UseVisualStyleBackColor = true;
            this.btnSubmitRate.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.bSilver = false;
            this.btnDelete.Location = new System.Drawing.Point(763, 61);
            this.btnDelete.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelete.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(99, 27);
            this.btnDelete.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.ToFocused = false;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnNew
            // 
            this.btnNew.bSilver = false;
            this.btnNew.Location = new System.Drawing.Point(763, 11);
            this.btnNew.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnNew.MenuPos = new System.Drawing.Point(0, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(99, 27);
            this.btnNew.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "添加";
            this.btnNew.ToFocused = false;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // panel1
            // 
            this.panel1.AssociatedSplitter = null;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.CaptionFont = new System.Drawing.Font("SimSun", 12.5F, System.Drawing.FontStyle.Bold);
            this.panel1.CaptionHeight = 27;
            this.panel1.Controls.Add(this.dataGridVirtual);
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Image = null;
            this.panel1.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.MinimumSize = new System.Drawing.Size(27, 27);
            this.panel1.Name = "panel1";
            this.panel1.PanelStyle = Skyray.Controls.XPander.PanelStyle.Office2007Blue;
            this.panel1.ShowCaptionbar = false;
            this.panel1.Size = new System.Drawing.Size(749, 264);
            this.panel1.Style = Skyray.Controls.Style.Office2007Blue;
            this.panel1.TabIndex = 1;
            this.panel1.Text = "panel1";
            this.panel1.ToolTipTextCloseIcon = null;
            this.panel1.ToolTipTextExpandIconPanelCollapsed = null;
            this.panel1.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // dataGridVirtual
            // 
            this.dataGridVirtual.AllowUserToAddRows = false;
            this.dataGridVirtual.AllowUserToResizeRows = false;
            this.dataGridVirtual.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dataGridVirtual.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridVirtual.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dataGridVirtual.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dataGridVirtual.ColumnHeadersHeight = 20;
            this.dataGridVirtual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridVirtual.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SpecListName,
            this.SpecColor,
            this.SpecDate,
            this.SpecTime,
            this.TubVoltage,
            this.TubCurrent,
            this.dataCellFilter,
            this.ColumnEnable});
            this.dataGridVirtual.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridVirtual.Location = new System.Drawing.Point(0, 0);
            this.dataGridVirtual.Name = "dataGridVirtual";
            this.dataGridVirtual.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dataGridVirtual.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dataGridVirtual.RowHeadersVisible = false;
            this.dataGridVirtual.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dataGridVirtual.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridVirtual.RowTemplate.Height = 23;
            this.dataGridVirtual.SecondaryLength = 1;
            this.dataGridVirtual.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dataGridVirtual.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dataGridVirtual.SelectedRowColor1 = System.Drawing.Color.White;
            this.dataGridVirtual.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dataGridVirtual.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridVirtual.ShowEportContextMenu = false;
            this.dataGridVirtual.Size = new System.Drawing.Size(749, 261);
            this.dataGridVirtual.Style = Skyray.Controls.Style.Office2007Blue;
            this.dataGridVirtual.TabIndex = 0;
            this.dataGridVirtual.ToPrintCols = null;
            this.dataGridVirtual.ToPrintRows = null;
            this.dataGridVirtual.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridVirtual_CellDoubleClick);
            this.dataGridVirtual.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridVirtual_CellPainting);
            // 
            // SpecListName
            // 
            this.SpecListName.HeaderText = "谱名称";
            this.SpecListName.Name = "SpecListName";
            this.SpecListName.Width = 205;
            // 
            // SpecColor
            // 
            this.SpecColor.HeaderText = "颜色";
            this.SpecColor.Name = "SpecColor";
            this.SpecColor.ReadOnly = true;
            this.SpecColor.Width = 80;
            // 
            // SpecDate
            // 
            this.SpecDate.HeaderText = "日期";
            this.SpecDate.Name = "SpecDate";
            this.SpecDate.Width = 80;
            // 
            // SpecTime
            // 
            this.SpecTime.HeaderText = "时间(s)";
            this.SpecTime.Name = "SpecTime";
            this.SpecTime.Width = 80;
            // 
            // TubVoltage
            // 
            this.TubVoltage.HeaderText = "管压(Kv)";
            this.TubVoltage.Name = "TubVoltage";
            this.TubVoltage.Width = 80;
            // 
            // TubCurrent
            // 
            this.TubCurrent.HeaderText = "管流(uA)";
            this.TubCurrent.Name = "TubCurrent";
            this.TubCurrent.Width = 80;
            // 
            // dataCellFilter
            // 
            this.dataCellFilter.HeaderText = "滤光片";
            this.dataCellFilter.Name = "dataCellFilter";
            this.dataCellFilter.Width = 80;
            // 
            // ColumnEnable
            // 
            this.ColumnEnable.HeaderText = "激活";
            this.ColumnEnable.Name = "ColumnEnable";
            this.ColumnEnable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnEnable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColumnEnable.Width = 60;
            // 
            // btnRatioVisual
            // 
            this.btnRatioVisual.bSilver = false;
            this.btnRatioVisual.Location = new System.Drawing.Point(763, 108);
            this.btnRatioVisual.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnRatioVisual.MenuPos = new System.Drawing.Point(0, 0);
            this.btnRatioVisual.Name = "btnRatioVisual";
            this.btnRatioVisual.Size = new System.Drawing.Size(99, 27);
            this.btnRatioVisual.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnRatioVisual.TabIndex = 5;
            this.btnRatioVisual.Text = "原始对比谱";
            this.btnRatioVisual.ToFocused = false;
            this.btnRatioVisual.UseVisualStyleBackColor = true;
            this.btnRatioVisual.Click += new System.EventHandler(this.btnRatioVisual_Click);
            // 
            // btnCacel
            // 
            this.btnCacel.bSilver = false;
            this.btnCacel.Location = new System.Drawing.Point(763, 234);
            this.btnCacel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCacel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCacel.Name = "btnCacel";
            this.btnCacel.Size = new System.Drawing.Size(99, 27);
            this.btnCacel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCacel.TabIndex = 6;
            this.btnCacel.Text = "取消";
            this.btnCacel.ToFocused = false;
            this.btnCacel.UseVisualStyleBackColor = true;
            this.btnCacel.Click += new System.EventHandler(this.btnCacel_Click);
            // 
            // UCVirtualSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCacel);
            this.Controls.Add(this.btnRatioVisual);
            this.Controls.Add(this.btnSubmitRate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.panel1);
            this.Name = "UCVirtualSelect";
            this.Size = new System.Drawing.Size(873, 280);
            this.Load += new System.EventHandler(this.UCVirtualSelect_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVirtual)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dataGridVirtual;
        private Skyray.Controls.XPander.Panel panel1;
        private Skyray.Controls.ButtonW btnNew;
        private Skyray.Controls.ButtonW btnDelete;
        private Skyray.Controls.ButtonW btnSubmitRate;
        private Skyray.Controls.ButtonW btnRatioVisual;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private Skyray.Controls.ButtonW btnCacel;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecListName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn TubVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn TubCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataCellFilter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnEnable;
    }
}
