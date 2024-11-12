namespace Skyray.UC
{
    partial class UCWorkRegionSetting
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
            this.btnDelete = new Skyray.Controls.ButtonW();
            this.dgvWorkRegion = new Skyray.Controls.DataGridViewW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.cboLikeWorkRegionName = new Skyray.Controls.ComboBoxW();
            this.txtWorkRegionName = new Skyray.Controls.TextBoxW();
            this.lblWorkRegionName = new Skyray.Controls.LabelW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBoxWSimilarWorkgion = new Skyray.Controls.CheckBoxW();
            this.ColWorkRegionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkRegion)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.bSilver = false;
            this.btnDelete.Location = new System.Drawing.Point(366, 231);
            this.btnDelete.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelete.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "删除";
            this.btnDelete.ToFocused = false;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dgvWorkRegion
            // 
            this.dgvWorkRegion.AllowUserToAddRows = false;
            this.dgvWorkRegion.AllowUserToDeleteRows = false;
            this.dgvWorkRegion.AllowUserToResizeRows = false;
            this.dgvWorkRegion.BackgroundColor = System.Drawing.Color.White;
            this.dgvWorkRegion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvWorkRegion.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvWorkRegion.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvWorkRegion.ColumnHeadersHeight = 20;
            this.dgvWorkRegion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkRegion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColWorkRegionName});
            this.dgvWorkRegion.Location = new System.Drawing.Point(24, 35);
            this.dgvWorkRegion.MultiSelect = false;
            this.dgvWorkRegion.Name = "dgvWorkRegion";
            this.dgvWorkRegion.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvWorkRegion.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvWorkRegion.ReadOnly = true;
            this.dgvWorkRegion.RowHeadersVisible = false;
            this.dgvWorkRegion.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvWorkRegion.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvWorkRegion.RowTemplate.Height = 23;
            this.dgvWorkRegion.SecondaryLength = 1;
            this.dgvWorkRegion.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvWorkRegion.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvWorkRegion.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvWorkRegion.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvWorkRegion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWorkRegion.ShowEportContextMenu = false;
            this.dgvWorkRegion.Size = new System.Drawing.Size(221, 237);
            this.dgvWorkRegion.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvWorkRegion.TabIndex = 6;
            this.dgvWorkRegion.ToPrintCols = null;
            this.dgvWorkRegion.ToPrintRows = null;
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(459, 231);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关闭";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(273, 231);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "增加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cboLikeWorkRegionName
            // 
            this.cboLikeWorkRegionName.AutoComplete = false;
            this.cboLikeWorkRegionName.AutoDropdown = false;
            this.cboLikeWorkRegionName.BackColorEven = System.Drawing.Color.White;
            this.cboLikeWorkRegionName.BackColorOdd = System.Drawing.Color.White;
            this.cboLikeWorkRegionName.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboLikeWorkRegionName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboLikeWorkRegionName.ColumnNames = "";
            this.cboLikeWorkRegionName.ColumnWidthDefault = 75;
            this.cboLikeWorkRegionName.ColumnWidths = "";
            this.cboLikeWorkRegionName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboLikeWorkRegionName.FormattingEnabled = true;
            this.cboLikeWorkRegionName.LinkedColumnIndex = 0;
            this.cboLikeWorkRegionName.LinkedTextBox = null;
            this.cboLikeWorkRegionName.Location = new System.Drawing.Point(271, 170);
            this.cboLikeWorkRegionName.Name = "cboLikeWorkRegionName";
            this.cboLikeWorkRegionName.Size = new System.Drawing.Size(263, 22);
            this.cboLikeWorkRegionName.TabIndex = 3;
            // 
            // txtWorkRegionName
            // 
            this.txtWorkRegionName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtWorkRegionName.Location = new System.Drawing.Point(271, 81);
            this.txtWorkRegionName.Name = "txtWorkRegionName";
            this.txtWorkRegionName.Size = new System.Drawing.Size(263, 21);
            this.txtWorkRegionName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtWorkRegionName.TabIndex = 1;
            // 
            // lblWorkRegionName
            // 
            this.lblWorkRegionName.AutoSize = true;
            this.lblWorkRegionName.BackColor = System.Drawing.Color.Transparent;
            this.lblWorkRegionName.Location = new System.Drawing.Point(269, 44);
            this.lblWorkRegionName.Name = "lblWorkRegionName";
            this.lblWorkRegionName.Size = new System.Drawing.Size(101, 12);
            this.lblWorkRegionName.TabIndex = 0;
            this.lblWorkRegionName.Text = "新建工作区名称：";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "工作区名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 220;
            // 
            // checkBoxWSimilarWorkgion
            // 
            this.checkBoxWSimilarWorkgion.AutoSize = true;
            this.checkBoxWSimilarWorkgion.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.checkBoxWSimilarWorkgion.Location = new System.Drawing.Point(271, 129);
            this.checkBoxWSimilarWorkgion.Name = "checkBoxWSimilarWorkgion";
            this.checkBoxWSimilarWorkgion.Size = new System.Drawing.Size(120, 16);
            this.checkBoxWSimilarWorkgion.Style = Skyray.Controls.Style.Office2007Blue;
            this.checkBoxWSimilarWorkgion.TabIndex = 8;
            this.checkBoxWSimilarWorkgion.Text = "相似工作区名称：";
            this.checkBoxWSimilarWorkgion.UseVisualStyleBackColor = true;
            // 
            // ColWorkRegionName
            // 
            this.ColWorkRegionName.DataPropertyName = "Caption";
            this.ColWorkRegionName.HeaderText = "工作区名称";
            this.ColWorkRegionName.Name = "ColWorkRegionName";
            this.ColWorkRegionName.ReadOnly = true;
            this.ColWorkRegionName.Width = 220;
            // 
            // UCWorkRegionSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.checkBoxWSimilarWorkgion);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dgvWorkRegion);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cboLikeWorkRegionName);
            this.Controls.Add(this.txtWorkRegionName);
            this.Controls.Add(this.lblWorkRegionName);
            this.Name = "UCWorkRegionSetting";
            this.Size = new System.Drawing.Size(548, 297);
            this.Load += new System.EventHandler(this.UCWorkRegionSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkRegion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblWorkRegionName;
        private Skyray.Controls.TextBoxW txtWorkRegionName;
        private Skyray.Controls.ComboBoxW cboLikeWorkRegionName;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.DataGridViewW dgvWorkRegion;
        private Skyray.Controls.ButtonW btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private Skyray.Controls.CheckBoxW checkBoxWSimilarWorkgion;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColWorkRegionName;
    }
}
