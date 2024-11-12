namespace Skyray.UC
{
    partial class UCSupplier
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
            this.txtSupplier = new Skyray.Controls.TextBoxW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.dgvSupplier = new Skyray.Controls.DataGridViewW();
            this.ColSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnApplication = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplier)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSupplier
            // 
            this.txtSupplier.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtSupplier.Location = new System.Drawing.Point(11, 12);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(120, 21);
            this.txtSupplier.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtSupplier.TabIndex = 6;
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(272, 347);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(91, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "取消";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(174, 347);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(261, 10);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(102, 23);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(153, 10);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvSupplier
            // 
            this.dgvSupplier.AllowUserToAddRows = false;
            this.dgvSupplier.AllowUserToDeleteRows = false;
            this.dgvSupplier.AllowUserToResizeColumns = false;
            this.dgvSupplier.AllowUserToResizeRows = false;
            this.dgvSupplier.BackgroundColor = System.Drawing.Color.White;
            this.dgvSupplier.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSupplier.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvSupplier.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvSupplier.ColumnHeadersHeight = 24;
            this.dgvSupplier.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSupplier.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSupplier,
            this.ColCreateDate});
            this.dgvSupplier.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSupplier.Location = new System.Drawing.Point(12, 45);
            this.dgvSupplier.MultiSelect = false;
            this.dgvSupplier.Name = "dgvSupplier";
            this.dgvSupplier.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvSupplier.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvSupplier.RowHeadersVisible = false;
            this.dgvSupplier.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvSupplier.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvSupplier.RowTemplate.Height = 23;
            this.dgvSupplier.SecondaryLength = 1;
            this.dgvSupplier.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvSupplier.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvSupplier.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvSupplier.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvSupplier.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSupplier.ShowEportContextMenu = true;
            this.dgvSupplier.Size = new System.Drawing.Size(351, 296);
            this.dgvSupplier.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvSupplier.TabIndex = 0;
            this.dgvSupplier.ToPrintCols = null;
            this.dgvSupplier.ToPrintRows = null;
            // 
            // ColSupplier
            // 
            this.ColSupplier.DataPropertyName = "Name";
            this.ColSupplier.HeaderText = "供应商名称";
            this.ColSupplier.Name = "ColSupplier";
            this.ColSupplier.ReadOnly = true;
            this.ColSupplier.Width = 160;
            // 
            // ColCreateDate
            // 
            this.ColCreateDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColCreateDate.DataPropertyName = "CreateDate";
            this.ColCreateDate.HeaderText = "创建日期";
            this.ColCreateDate.Name = "ColCreateDate";
            this.ColCreateDate.ReadOnly = true;
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(74, 347);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(86, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 54;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // UCSupplier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.txtSupplier);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvSupplier);
            this.Name = "UCSupplier";
            this.Size = new System.Drawing.Size(378, 381);
            this.Load += new System.EventHandler(this.UCSupplier_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplier)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvSupplier;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.TextBoxW txtSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCreateDate;
        private Skyray.Controls.ButtonW btnApplication;
    }
}
