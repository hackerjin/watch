namespace Skyray.UC
{
    partial class UCDefinePureElem
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
            this.txtElementName = new Skyray.Controls.TextBoxW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.dgvDefineElem = new Skyray.Controls.DataGridViewW();
            this.ColSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnApplication = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefineElem)).BeginInit();
            this.SuspendLayout();
            // 
            // txtElementName
            // 
            this.txtElementName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtElementName.Location = new System.Drawing.Point(11, 13);
            this.txtElementName.Name = "txtElementName";
            this.txtElementName.Size = new System.Drawing.Size(120, 20);
            this.txtElementName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtElementName.TabIndex = 6;
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(272, 376);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(91, 25);
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
            this.btnOK.Location = new System.Drawing.Point(174, 376);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 25);
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
            this.btnDel.Location = new System.Drawing.Point(261, 11);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(102, 25);
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
            this.btnAdd.Location = new System.Drawing.Point(153, 11);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 25);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvDefineElem
            // 
            this.dgvDefineElem.AllowUserToAddRows = false;
            this.dgvDefineElem.AllowUserToDeleteRows = false;
            this.dgvDefineElem.AllowUserToResizeColumns = false;
            this.dgvDefineElem.AllowUserToResizeRows = false;
            this.dgvDefineElem.BackgroundColor = System.Drawing.Color.White;
            this.dgvDefineElem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvDefineElem.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvDefineElem.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvDefineElem.ColumnHeadersHeight = 24;
            this.dgvDefineElem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDefineElem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSupplier,
            this.ColCreateDate});
            this.dgvDefineElem.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDefineElem.Location = new System.Drawing.Point(12, 49);
            this.dgvDefineElem.MultiSelect = false;
            this.dgvDefineElem.Name = "dgvDefineElem";
            this.dgvDefineElem.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvDefineElem.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvDefineElem.RowHeadersVisible = false;
            this.dgvDefineElem.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvDefineElem.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvDefineElem.RowTemplate.Height = 23;
            this.dgvDefineElem.SecondaryLength = 1;
            this.dgvDefineElem.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvDefineElem.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvDefineElem.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvDefineElem.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvDefineElem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDefineElem.ShowEportContextMenu = true;
            this.dgvDefineElem.Size = new System.Drawing.Size(351, 321);
            this.dgvDefineElem.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvDefineElem.TabIndex = 0;
            this.dgvDefineElem.ToPrintCols = null;
            this.dgvDefineElem.ToPrintRows = null;
            // 
            // ColSupplier
            // 
            this.ColSupplier.DataPropertyName = "Name";
            this.ColSupplier.HeaderText = "纯元素名称";
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
            this.btnApplication.Location = new System.Drawing.Point(74, 376);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(86, 25);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 54;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // UCDefinePureElem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.txtElementName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvDefineElem);
            this.Name = "UCDefinePureElem";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(378, 413);
            this.Load += new System.EventHandler(this.UCDefinePureElem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefineElem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvDefineElem;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.TextBoxW txtElementName;
        private Skyray.Controls.ButtonW btnApplication;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCreateDate;
    }
}
