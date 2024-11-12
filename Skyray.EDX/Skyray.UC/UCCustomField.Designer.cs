namespace Skyray.UC
{
    partial class UCCustomField
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
            this.btnClose = new Skyray.Controls.ButtonW();
            this.btnSave = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.dgvwCustom = new Skyray.Controls.DataGridViewW();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColExpression = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnApplication = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCustom)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(318, 163);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(99, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "取消";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.bSilver = false;
            this.btnSave.Location = new System.Drawing.Point(318, 124);
            this.btnSave.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSave.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 23);
            this.btnSave.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "确定";
            this.btnSave.ToFocused = false;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(318, 47);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(99, 23);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 8;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(318, 8);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(99, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "增加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvwCustom
            // 
            this.dgvwCustom.AllowUserToAddRows = false;
            this.dgvwCustom.AllowUserToResizeColumns = false;
            this.dgvwCustom.AllowUserToResizeRows = false;
            this.dgvwCustom.BackgroundColor = System.Drawing.Color.White;
            this.dgvwCustom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwCustom.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwCustom.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwCustom.ColumnHeadersHeight = 24;
            this.dgvwCustom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwCustom.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColExpression});
            this.dgvwCustom.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvwCustom.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwCustom.Location = new System.Drawing.Point(8, 8);
            this.dgvwCustom.MultiSelect = false;
            this.dgvwCustom.Name = "dgvwCustom";
            this.dgvwCustom.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwCustom.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwCustom.RowHeadersVisible = false;
            this.dgvwCustom.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwCustom.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwCustom.RowTemplate.Height = 23;
            this.dgvwCustom.SecondaryLength = 1;
            this.dgvwCustom.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwCustom.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwCustom.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwCustom.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwCustom.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwCustom.ShowEportContextMenu = true;
            this.dgvwCustom.Size = new System.Drawing.Size(304, 260);
            this.dgvwCustom.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwCustom.TabIndex = 12;
            this.dgvwCustom.ToPrintCols = null;
            this.dgvwCustom.ToPrintRows = null;
            this.dgvwCustom.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvwCustom_CellValidating);
            // 
            // ColName
            // 
            this.ColName.DataPropertyName = "Name";
            this.ColName.HeaderText = "名称";
            this.ColName.Name = "ColName";
            this.ColName.Width = 150;
            // 
            // ColExpression
            // 
            this.ColExpression.DataPropertyName = "Expression";
            this.ColExpression.HeaderText = "表达式";
            this.ColExpression.Name = "ColExpression";
            this.ColExpression.Width = 150;
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(318, 85);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(99, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 13;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // UCCustomField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.dgvwCustom);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Name = "UCCustomField";
            this.Size = new System.Drawing.Size(427, 276);
            this.Load += new System.EventHandler(this.UCCustomField_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCustom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.ButtonW btnSave;
        private Skyray.Controls.DataGridViewW dgvwCustom;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColExpression;
        private Skyray.Controls.ButtonW btnApplication;
    }
}
