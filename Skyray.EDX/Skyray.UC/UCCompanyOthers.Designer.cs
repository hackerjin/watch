namespace Skyray.UC
{
    partial class UCCompanyOthers
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
            this.lblLabel = new System.Windows.Forms.Label();
            this.cbxInpoutItem = new System.Windows.Forms.ComboBox();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.btnDelete = new Skyray.Controls.ButtonW();
            this.btnSubmit = new Skyray.Controls.ButtonW();
            this.btnCacel = new Skyray.Controls.ButtonW();
            this.dgvValues = new Skyray.Controls.DataGridViewW();
            this.dgvSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLabel
            // 
            this.lblLabel.AutoSize = true;
            this.lblLabel.Location = new System.Drawing.Point(12, 14);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(53, 12);
            this.lblLabel.TabIndex = 0;
            this.lblLabel.Text = "输入项：";
            // 
            // cbxInpoutItem
            // 
            this.cbxInpoutItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxInpoutItem.FormattingEnabled = true;
            this.cbxInpoutItem.Location = new System.Drawing.Point(14, 29);
            this.cbxInpoutItem.Name = "cbxInpoutItem";
            this.cbxInpoutItem.Size = new System.Drawing.Size(276, 20);
            this.cbxInpoutItem.TabIndex = 1;
            this.cbxInpoutItem.SelectedIndexChanged += new System.EventHandler(this.cbxInpoutItem_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(304, 59);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.bSilver = false;
            this.btnDelete.Location = new System.Drawing.Point(304, 88);
            this.btnDelete.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelete.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "删除";
            this.btnDelete.ToFocused = false;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.bSilver = false;
            this.btnSubmit.Location = new System.Drawing.Point(304, 126);
            this.btnSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmit.TabIndex = 7;
            this.btnSubmit.Text = "确认";
            this.btnSubmit.ToFocused = false;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCacel
            // 
            this.btnCacel.bSilver = false;
            this.btnCacel.Location = new System.Drawing.Point(304, 155);
            this.btnCacel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCacel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCacel.Name = "btnCacel";
            this.btnCacel.Size = new System.Drawing.Size(75, 23);
            this.btnCacel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCacel.TabIndex = 8;
            this.btnCacel.Text = "取消";
            this.btnCacel.ToFocused = false;
            this.btnCacel.UseVisualStyleBackColor = true;
            this.btnCacel.Click += new System.EventHandler(this.btnCacel_Click);
            // 
            // dgvValues
            // 
            this.dgvValues.AllowUserToAddRows = false;
            this.dgvValues.AllowUserToResizeColumns = false;
            this.dgvValues.AllowUserToResizeRows = false;
            this.dgvValues.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvValues.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvValues.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvValues.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvValues.ColumnHeadersHeight = 4;
            this.dgvValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvValues.ColumnHeadersVisible = false;
            this.dgvValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvSelect,
            this.dgvId,
            this.dgvValueColumn});
            this.dgvValues.Location = new System.Drawing.Point(14, 59);
            this.dgvValues.MultiSelect = false;
            this.dgvValues.Name = "dgvValues";
            this.dgvValues.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvValues.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvValues.RowHeadersVisible = false;
            this.dgvValues.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvValues.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvValues.RowTemplate.Height = 23;
            this.dgvValues.SecondaryLength = 1;
            this.dgvValues.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvValues.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvValues.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvValues.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvValues.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvValues.ShowEportContextMenu = false;
            this.dgvValues.Size = new System.Drawing.Size(276, 125);
            this.dgvValues.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvValues.TabIndex = 9;
            this.dgvValues.ToPrintCols = null;
            this.dgvValues.ToPrintRows = null;
            this.dgvValues.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValues_CellEndEdit);
            this.dgvValues.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValues_CellClick);
            // 
            // dgvSelect
            // 
            this.dgvSelect.HeaderText = "dgvSelct";
            this.dgvSelect.Name = "dgvSelect";
            this.dgvSelect.Width = 30;
            // 
            // dgvId
            // 
            this.dgvId.HeaderText = "dgvId";
            this.dgvId.Name = "dgvId";
            this.dgvId.Visible = false;
            // 
            // dgvValueColumn
            // 
            this.dgvValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvValueColumn.HeaderText = "dgvValueColumn";
            this.dgvValueColumn.Name = "dgvValueColumn";
            // 
            // UCCompanyOthers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.dgvValues);
            this.Controls.Add(this.btnCacel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cbxInpoutItem);
            this.Controls.Add(this.lblLabel);
            this.Name = "UCCompanyOthers";
            this.Size = new System.Drawing.Size(392, 198);
            this.Load += new System.EventHandler(this.UCCompanyOthers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.ComboBox cbxInpoutItem;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW btnDelete;
        private Skyray.Controls.ButtonW btnSubmit;
        private Skyray.Controls.ButtonW btnCacel;
        private Skyray.Controls.DataGridViewW dgvValues;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvValueColumn;
    }
}
