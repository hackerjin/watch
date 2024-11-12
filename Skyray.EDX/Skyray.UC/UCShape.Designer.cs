namespace Skyray.UC
{
    partial class UCShape
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
            this.txtShape = new Skyray.Controls.TextBoxW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.dgvShape = new Skyray.Controls.DataGridViewW();
            this.ColShape = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnApplication = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShape)).BeginInit();
            this.SuspendLayout();
            // 
            // txtShape
            // 
            this.txtShape.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtShape.Location = new System.Drawing.Point(12, 12);
            this.txtShape.Name = "txtShape";
            this.txtShape.Size = new System.Drawing.Size(122, 21);
            this.txtShape.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtShape.TabIndex = 12;
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(276, 353);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(91, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "取消";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(167, 353);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(89, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(262, 10);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(105, 23);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 9;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(152, 10);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvShape
            // 
            this.dgvShape.AllowUserToAddRows = false;
            this.dgvShape.AllowUserToDeleteRows = false;
            this.dgvShape.AllowUserToResizeColumns = false;
            this.dgvShape.AllowUserToResizeRows = false;
            this.dgvShape.BackgroundColor = System.Drawing.Color.White;
            this.dgvShape.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvShape.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvShape.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvShape.ColumnHeadersHeight = 24;
            this.dgvShape.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvShape.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColShape,
            this.ColCreateDate});
            this.dgvShape.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvShape.Location = new System.Drawing.Point(12, 50);
            this.dgvShape.MultiSelect = false;
            this.dgvShape.Name = "dgvShape";
            this.dgvShape.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvShape.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvShape.RowHeadersVisible = false;
            this.dgvShape.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvShape.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvShape.RowTemplate.Height = 23;
            this.dgvShape.SecondaryLength = 1;
            this.dgvShape.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvShape.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvShape.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvShape.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvShape.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvShape.ShowEportContextMenu = true;
            this.dgvShape.Size = new System.Drawing.Size(355, 296);
            this.dgvShape.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvShape.TabIndex = 7;
            this.dgvShape.ToPrintCols = null;
            this.dgvShape.ToPrintRows = null;
            // 
            // ColShape
            // 
            this.ColShape.DataPropertyName = "Name";
            this.ColShape.HeaderText = "形状名称";
            this.ColShape.Name = "ColShape";
            this.ColShape.ReadOnly = true;
            this.ColShape.Width = 160;
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
            this.btnApplication.Location = new System.Drawing.Point(62, 353);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(86, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 53;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // UCShape
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.txtShape);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvShape);
            this.Name = "UCShape";
            this.Size = new System.Drawing.Size(385, 385);
            this.Load += new System.EventHandler(this.Shape_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShape)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.TextBoxW txtShape;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.DataGridViewW dgvShape;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColShape;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCreateDate;
        private Skyray.Controls.ButtonW btnApplication;
    }
}
