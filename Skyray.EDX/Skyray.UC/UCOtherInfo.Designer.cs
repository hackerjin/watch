namespace Skyray.UC
{
    partial class UCOtherInfo
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
            this.dgvwCompanyOtherInfo = new Skyray.Controls.DataGridViewW();
            this.ColId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIsShow = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvwOthersListInfo = new Skyray.Controls.DataGridViewW();
            this.ColDataId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIsAcquiescence = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.butDeleteInfo = new Skyray.Controls.ButtonW();
            this.butAddInfo = new Skyray.Controls.ButtonW();
            this.butDeleteName = new Skyray.Controls.ButtonW();
            this.butAddName = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.txtName = new Skyray.Controls.TextBoxW();
            this.txtInfo = new Skyray.Controls.TextBoxW();
            this.comControlType = new Skyray.Controls.ComboBoxW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.labelW2 = new Skyray.Controls.LabelW();
            this.combReportInfo = new Skyray.Controls.ComboBoxW();
            this.butAddReportName = new Skyray.Controls.ButtonW();
            this.comRoportControlType = new Skyray.Controls.ComboBoxW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCompanyOtherInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwOthersListInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvwCompanyOtherInfo
            // 
            this.dgvwCompanyOtherInfo.AllowUserToAddRows = false;
            this.dgvwCompanyOtherInfo.AllowUserToResizeRows = false;
            this.dgvwCompanyOtherInfo.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvwCompanyOtherInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvwCompanyOtherInfo.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwCompanyOtherInfo.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwCompanyOtherInfo.ColumnHeadersHeight = 20;
            this.dgvwCompanyOtherInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvwCompanyOtherInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColId,
            this.ColName,
            this.ColIsShow});
            this.dgvwCompanyOtherInfo.Location = new System.Drawing.Point(5, 85);
            this.dgvwCompanyOtherInfo.Name = "dgvwCompanyOtherInfo";
            this.dgvwCompanyOtherInfo.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwCompanyOtherInfo.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwCompanyOtherInfo.RowHeadersVisible = false;
            this.dgvwCompanyOtherInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwCompanyOtherInfo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwCompanyOtherInfo.RowTemplate.Height = 23;
            this.dgvwCompanyOtherInfo.SecondaryLength = 1;
            this.dgvwCompanyOtherInfo.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwCompanyOtherInfo.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwCompanyOtherInfo.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwCompanyOtherInfo.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwCompanyOtherInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvwCompanyOtherInfo.ShowEportContextMenu = false;
            this.dgvwCompanyOtherInfo.Size = new System.Drawing.Size(164, 259);
            this.dgvwCompanyOtherInfo.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwCompanyOtherInfo.TabIndex = 3;
            this.dgvwCompanyOtherInfo.ToPrintCols = null;
            this.dgvwCompanyOtherInfo.ToPrintRows = null;
            this.dgvwCompanyOtherInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwCompanyOtherInfo_CellClick);
            // 
            // ColId
            // 
            this.ColId.DataPropertyName = "Id";
            this.ColId.HeaderText = "Id";
            this.ColId.Name = "ColId";
            this.ColId.Visible = false;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColName.DataPropertyName = "Name";
            this.ColName.HeaderText = "列名";
            this.ColName.Name = "ColName";
            // 
            // ColIsShow
            // 
            this.ColIsShow.DataPropertyName = "Display";
            this.ColIsShow.HeaderText = "显示";
            this.ColIsShow.Name = "ColIsShow";
            this.ColIsShow.Width = 60;
            // 
            // dgvwOthersListInfo
            // 
            this.dgvwOthersListInfo.AllowUserToAddRows = false;
            this.dgvwOthersListInfo.AllowUserToResizeRows = false;
            this.dgvwOthersListInfo.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvwOthersListInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvwOthersListInfo.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwOthersListInfo.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwOthersListInfo.ColumnHeadersHeight = 20;
            this.dgvwOthersListInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvwOthersListInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColDataId,
            this.ColData,
            this.ColIsAcquiescence});
            this.dgvwOthersListInfo.Location = new System.Drawing.Point(175, 13);
            this.dgvwOthersListInfo.Name = "dgvwOthersListInfo";
            this.dgvwOthersListInfo.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwOthersListInfo.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwOthersListInfo.RowHeadersVisible = false;
            this.dgvwOthersListInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwOthersListInfo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwOthersListInfo.RowTemplate.Height = 23;
            this.dgvwOthersListInfo.SecondaryLength = 1;
            this.dgvwOthersListInfo.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwOthersListInfo.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwOthersListInfo.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwOthersListInfo.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwOthersListInfo.ShowEportContextMenu = false;
            this.dgvwOthersListInfo.Size = new System.Drawing.Size(206, 333);
            this.dgvwOthersListInfo.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwOthersListInfo.TabIndex = 4;
            this.dgvwOthersListInfo.ToPrintCols = null;
            this.dgvwOthersListInfo.ToPrintRows = null;
            this.dgvwOthersListInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwOthersListInfo_CellContentClick);
            // 
            // ColDataId
            // 
            this.ColDataId.DataPropertyName = "Id";
            this.ColDataId.HeaderText = "Id";
            this.ColDataId.Name = "ColDataId";
            this.ColDataId.Visible = false;
            // 
            // ColData
            // 
            this.ColData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColData.DataPropertyName = "ListInfo";
            this.ColData.HeaderText = "信息";
            this.ColData.Name = "ColData";
            // 
            // ColIsAcquiescence
            // 
            this.ColIsAcquiescence.DataPropertyName = "Display";
            this.ColIsAcquiescence.HeaderText = "默认";
            this.ColIsAcquiescence.Name = "ColIsAcquiescence";
            this.ColIsAcquiescence.Width = 60;
            // 
            // butDeleteInfo
            // 
            this.butDeleteInfo.BackColor = System.Drawing.Color.GhostWhite;
            this.butDeleteInfo.bSilver = false;
            this.butDeleteInfo.Location = new System.Drawing.Point(397, 113);
            this.butDeleteInfo.MaxImageSize = new System.Drawing.Point(0, 0);
            this.butDeleteInfo.MenuPos = new System.Drawing.Point(0, 0);
            this.butDeleteInfo.Name = "butDeleteInfo";
            this.butDeleteInfo.Size = new System.Drawing.Size(95, 23);
            this.butDeleteInfo.Style = Skyray.Controls.Style.Office2007Blue;
            this.butDeleteInfo.TabIndex = 10;
            this.butDeleteInfo.Text = "删除信息";
            this.butDeleteInfo.ToFocused = false;
            this.butDeleteInfo.UseVisualStyleBackColor = false;
            this.butDeleteInfo.Click += new System.EventHandler(this.butDeleteInfo_Click);
            // 
            // butAddInfo
            // 
            this.butAddInfo.bSilver = false;
            this.butAddInfo.Location = new System.Drawing.Point(397, 73);
            this.butAddInfo.MaxImageSize = new System.Drawing.Point(0, 0);
            this.butAddInfo.MenuPos = new System.Drawing.Point(0, 0);
            this.butAddInfo.Name = "butAddInfo";
            this.butAddInfo.Size = new System.Drawing.Size(95, 23);
            this.butAddInfo.Style = Skyray.Controls.Style.Office2007Blue;
            this.butAddInfo.TabIndex = 9;
            this.butAddInfo.Text = "新增信息";
            this.butAddInfo.ToFocused = false;
            this.butAddInfo.UseVisualStyleBackColor = true;
            this.butAddInfo.Click += new System.EventHandler(this.butAddInfo_Click);
            // 
            // butDeleteName
            // 
            this.butDeleteName.bSilver = false;
            this.butDeleteName.Location = new System.Drawing.Point(5, 360);
            this.butDeleteName.MaxImageSize = new System.Drawing.Point(0, 0);
            this.butDeleteName.MenuPos = new System.Drawing.Point(0, 0);
            this.butDeleteName.Name = "butDeleteName";
            this.butDeleteName.Size = new System.Drawing.Size(61, 23);
            this.butDeleteName.Style = Skyray.Controls.Style.Office2007Blue;
            this.butDeleteName.TabIndex = 12;
            this.butDeleteName.Text = "删除";
            this.butDeleteName.ToFocused = false;
            this.butDeleteName.UseVisualStyleBackColor = true;
            this.butDeleteName.Click += new System.EventHandler(this.butDeleteName_Click);
            // 
            // butAddName
            // 
            this.butAddName.bSilver = false;
            this.butAddName.Location = new System.Drawing.Point(269, 361);
            this.butAddName.MaxImageSize = new System.Drawing.Point(0, 0);
            this.butAddName.MenuPos = new System.Drawing.Point(0, 0);
            this.butAddName.Name = "butAddName";
            this.butAddName.Size = new System.Drawing.Size(52, 23);
            this.butAddName.Style = Skyray.Controls.Style.Office2007Blue;
            this.butAddName.TabIndex = 11;
            this.butAddName.Text = "新增";
            this.butAddName.ToFocused = false;
            this.butAddName.UseVisualStyleBackColor = true;
            this.butAddName.Click += new System.EventHandler(this.butAddName_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(397, 264);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 40;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(397, 224);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(95, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 39;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(397, 303);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 41;
            this.btnClose.Text = "取消";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtName
            // 
            this.txtName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtName.Location = new System.Drawing.Point(150, 362);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(113, 21);
            this.txtName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtName.TabIndex = 43;
            // 
            // txtInfo
            // 
            this.txtInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtInfo.Location = new System.Drawing.Point(397, 41);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(95, 21);
            this.txtInfo.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtInfo.TabIndex = 44;
            // 
            // comControlType
            // 
            this.comControlType.AutoComplete = false;
            this.comControlType.AutoDropdown = false;
            this.comControlType.BackColorEven = System.Drawing.Color.White;
            this.comControlType.BackColorOdd = System.Drawing.Color.White;
            this.comControlType.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.comControlType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.comControlType.ColumnNames = "";
            this.comControlType.ColumnWidthDefault = 75;
            this.comControlType.ColumnWidths = "";
            this.comControlType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comControlType.FormattingEnabled = true;
            this.comControlType.LinkedColumnIndex = 0;
            this.comControlType.LinkedTextBox = null;
            this.comControlType.Location = new System.Drawing.Point(72, 361);
            this.comControlType.Name = "comControlType";
            this.comControlType.Size = new System.Drawing.Size(72, 22);
            this.comControlType.TabIndex = 45;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "列名";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "Display";
            this.dataGridViewCheckBoxColumn1.HeaderText = "显示";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn3.HeaderText = "Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "InfoData";
            this.dataGridViewTextBoxColumn4.HeaderText = "信息";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.DataPropertyName = "Display";
            this.dataGridViewCheckBoxColumn2.HeaderText = "默认";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Width = 40;
            // 
            // labelW2
            // 
            this.labelW2.AutoSize = true;
            this.labelW2.BackColor = System.Drawing.Color.Transparent;
            this.labelW2.Location = new System.Drawing.Point(8, 8);
            this.labelW2.Name = "labelW2";
            this.labelW2.Size = new System.Drawing.Size(65, 12);
            this.labelW2.TabIndex = 46;
            this.labelW2.Text = "报表列新增";
            // 
            // combReportInfo
            // 
            this.combReportInfo.AutoComplete = false;
            this.combReportInfo.AutoDropdown = false;
            this.combReportInfo.BackColorEven = System.Drawing.Color.White;
            this.combReportInfo.BackColorOdd = System.Drawing.Color.White;
            this.combReportInfo.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.combReportInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.combReportInfo.ColumnNames = "";
            this.combReportInfo.ColumnWidthDefault = 75;
            this.combReportInfo.ColumnWidths = "";
            this.combReportInfo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.combReportInfo.FormattingEnabled = true;
            this.combReportInfo.LinkedColumnIndex = 0;
            this.combReportInfo.LinkedTextBox = null;
            this.combReportInfo.Location = new System.Drawing.Point(5, 55);
            this.combReportInfo.Name = "combReportInfo";
            this.combReportInfo.Size = new System.Drawing.Size(88, 22);
            this.combReportInfo.TabIndex = 47;
            // 
            // butAddReportName
            // 
            this.butAddReportName.bSilver = false;
            this.butAddReportName.Location = new System.Drawing.Point(104, 55);
            this.butAddReportName.MaxImageSize = new System.Drawing.Point(0, 0);
            this.butAddReportName.MenuPos = new System.Drawing.Point(0, 0);
            this.butAddReportName.Name = "butAddReportName";
            this.butAddReportName.Size = new System.Drawing.Size(52, 23);
            this.butAddReportName.Style = Skyray.Controls.Style.Office2007Blue;
            this.butAddReportName.TabIndex = 48;
            this.butAddReportName.Text = "新增";
            this.butAddReportName.ToFocused = false;
            this.butAddReportName.UseVisualStyleBackColor = true;
            this.butAddReportName.Click += new System.EventHandler(this.butAddReportName_Click);
            // 
            // comRoportControlType
            // 
            this.comRoportControlType.AutoComplete = false;
            this.comRoportControlType.AutoDropdown = false;
            this.comRoportControlType.BackColorEven = System.Drawing.Color.White;
            this.comRoportControlType.BackColorOdd = System.Drawing.Color.White;
            this.comRoportControlType.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.comRoportControlType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.comRoportControlType.ColumnNames = "";
            this.comRoportControlType.ColumnWidthDefault = 75;
            this.comRoportControlType.ColumnWidths = "";
            this.comRoportControlType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comRoportControlType.FormattingEnabled = true;
            this.comRoportControlType.LinkedColumnIndex = 0;
            this.comRoportControlType.LinkedTextBox = null;
            this.comRoportControlType.Location = new System.Drawing.Point(5, 27);
            this.comRoportControlType.Name = "comRoportControlType";
            this.comRoportControlType.Size = new System.Drawing.Size(88, 22);
            this.comRoportControlType.TabIndex = 49;
            // 
            // UCOtherInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.comRoportControlType);
            this.Controls.Add(this.butAddReportName);
            this.Controls.Add(this.combReportInfo);
            this.Controls.Add(this.labelW2);
            this.Controls.Add(this.comControlType);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.butDeleteName);
            this.Controls.Add(this.butAddName);
            this.Controls.Add(this.butDeleteInfo);
            this.Controls.Add(this.butAddInfo);
            this.Controls.Add(this.dgvwOthersListInfo);
            this.Controls.Add(this.dgvwCompanyOtherInfo);
            this.Name = "UCOtherInfo";
            this.Size = new System.Drawing.Size(516, 399);
            this.Load += new System.EventHandler(this.UCOtherInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCompanyOtherInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwOthersListInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvwCompanyOtherInfo;
        private Skyray.Controls.DataGridViewW dgvwOthersListInfo;
        private Skyray.Controls.ButtonW butDeleteInfo;
        private Skyray.Controls.ButtonW butAddInfo;
        private Skyray.Controls.ButtonW butDeleteName;
        private Skyray.Controls.ButtonW butAddName;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnApplication;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.TextBoxW txtName;
        private Skyray.Controls.TextBoxW txtInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private Skyray.Controls.ComboBoxW comControlType;
        private Skyray.Controls.LabelW labelW2;
        private Skyray.Controls.ComboBoxW combReportInfo;
        private Skyray.Controls.ButtonW butAddReportName;
        private Skyray.Controls.ComboBoxW comRoportControlType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDataId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColData;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsAcquiescence;
    }
}
