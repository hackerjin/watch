namespace Skyray.UC
{
    partial class UCExpunction
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
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.dgvwExpunction = new Skyray.Controls.DataGridViewW();
            this.ColElement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColKA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColKb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwExpunction)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(203, 302);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(400, 302);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvwExpunction
            // 
            this.dgvwExpunction.AllowUserToAddRows = false;
            this.dgvwExpunction.AllowUserToDeleteRows = false;
            this.dgvwExpunction.AllowUserToResizeRows = false;
            this.dgvwExpunction.BackgroundColor = System.Drawing.Color.White;
            this.dgvwExpunction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvwExpunction.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwExpunction.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwExpunction.ColumnHeadersHeight = 20;
            this.dgvwExpunction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvwExpunction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColElement,
            this.ColKA,
            this.ColKb,
            this.ColLa,
            this.ColLb,
            this.ColLr,
            this.ColLe});
            this.dgvwExpunction.Location = new System.Drawing.Point(3, 3);
            this.dgvwExpunction.Name = "dgvwExpunction";
            this.dgvwExpunction.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwExpunction.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwExpunction.RowHeadersVisible = false;
            this.dgvwExpunction.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwExpunction.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwExpunction.RowTemplate.Height = 23;
            this.dgvwExpunction.SecondaryLength = 1;
            this.dgvwExpunction.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwExpunction.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwExpunction.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwExpunction.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwExpunction.ShowEportContextMenu = false;
            this.dgvwExpunction.Size = new System.Drawing.Size(673, 273);
            this.dgvwExpunction.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwExpunction.TabIndex = 2;
            this.dgvwExpunction.ToPrintCols = null;
            this.dgvwExpunction.ToPrintRows = null;
            this.dgvwExpunction.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvwExpunction_CellValidating);
            this.dgvwExpunction.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvwExpunction_DataError);
            // 
            // ColElement
            // 
            this.ColElement.DataPropertyName = "ElementName";
            this.ColElement.HeaderText = "元素";
            this.ColElement.Name = "ColElement";
            this.ColElement.ReadOnly = true;
            this.ColElement.Width = 52;
            // 
            // ColKA
            // 
            this.ColKA.DataPropertyName = "Ka";
            this.ColKA.HeaderText = "Ka";
            this.ColKA.Name = "ColKA";
            // 
            // ColKb
            // 
            this.ColKb.DataPropertyName = "Kb";
            this.ColKb.HeaderText = "Kb";
            this.ColKb.Name = "ColKb";
            // 
            // ColLa
            // 
            this.ColLa.DataPropertyName = "La";
            this.ColLa.HeaderText = "La";
            this.ColLa.Name = "ColLa";
            // 
            // ColLb
            // 
            this.ColLb.DataPropertyName = "Lb";
            this.ColLb.HeaderText = "Lb";
            this.ColLb.Name = "ColLb";
            // 
            // ColLr
            // 
            this.ColLr.DataPropertyName = "Lr";
            this.ColLr.HeaderText = "Lr";
            this.ColLr.Name = "ColLr";
            // 
            // ColLe
            // 
            this.ColLe.DataPropertyName = "Le";
            this.ColLe.HeaderText = "Le";
            this.ColLe.Name = "ColLe";
            // 
            // UCExpunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.dgvwExpunction);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "UCExpunction";
            this.Size = new System.Drawing.Size(679, 343);
            this.Load += new System.EventHandler(this.UCExpunction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwExpunction)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.DataGridViewW dgvwExpunction;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColElement;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColKA;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColKb;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLa;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLb;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLe;
    }
}
