namespace Skyray.UC
{
    partial class UCCalcIntensity
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
            this.lblSampleName = new Skyray.Controls.LabelW();
            this.txtSampleName = new Skyray.Controls.TextBoxW();
            this.dgvwCalcIntensity = new Skyray.Controls.DataGridViewW();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnJoinCurve = new Skyray.Controls.ButtonW();
            this.btnClose = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCalcIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSampleName
            // 
            this.lblSampleName.AutoSize = true;
            this.lblSampleName.BackColor = System.Drawing.Color.Transparent;
            this.lblSampleName.Location = new System.Drawing.Point(16, 16);
            this.lblSampleName.Name = "lblSampleName";
            this.lblSampleName.Size = new System.Drawing.Size(53, 12);
            this.lblSampleName.TabIndex = 0;
            this.lblSampleName.Text = "样品名称";
            // 
            // txtSampleName
            // 
            this.txtSampleName.BackColor = System.Drawing.Color.White;
            this.txtSampleName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtSampleName.Location = new System.Drawing.Point(91, 11);
            this.txtSampleName.Name = "txtSampleName";
            this.txtSampleName.ReadOnly = true;
            this.txtSampleName.Size = new System.Drawing.Size(130, 21);
            this.txtSampleName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtSampleName.TabIndex = 1;
            // 
            // dgvwCalcIntensity
            // 
            this.dgvwCalcIntensity.AllowUserToAddRows = false;
            this.dgvwCalcIntensity.AllowUserToResizeColumns = false;
            this.dgvwCalcIntensity.AllowUserToResizeRows = false;
            this.dgvwCalcIntensity.BackgroundColor = System.Drawing.Color.White;
            this.dgvwCalcIntensity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvwCalcIntensity.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwCalcIntensity.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwCalcIntensity.ColumnHeadersHeight = 20;
            this.dgvwCalcIntensity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvwCalcIntensity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgvwCalcIntensity.Location = new System.Drawing.Point(14, 44);
            this.dgvwCalcIntensity.Name = "dgvwCalcIntensity";
            this.dgvwCalcIntensity.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwCalcIntensity.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwCalcIntensity.RowHeadersVisible = false;
            this.dgvwCalcIntensity.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwCalcIntensity.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwCalcIntensity.RowTemplate.Height = 23;
            this.dgvwCalcIntensity.SecondaryLength = 1;
            this.dgvwCalcIntensity.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwCalcIntensity.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwCalcIntensity.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwCalcIntensity.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwCalcIntensity.ShowEportContextMenu = true;
            this.dgvwCalcIntensity.Size = new System.Drawing.Size(362, 160);
            this.dgvwCalcIntensity.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwCalcIntensity.TabIndex = 2;
            this.dgvwCalcIntensity.ToPrintCols = null;
            this.dgvwCalcIntensity.ToPrintRows = null;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "元素";
            this.Column1.Name = "Column1";
            this.Column1.Width = 180;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "强度";
            this.Column2.Name = "Column2";
            this.Column2.Width = 180;
            // 
            // btnJoinCurve
            // 
            this.btnJoinCurve.bSilver = false;
            this.btnJoinCurve.Location = new System.Drawing.Point(82, 217);
            this.btnJoinCurve.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnJoinCurve.MenuPos = new System.Drawing.Point(0, 0);
            this.btnJoinCurve.Name = "btnJoinCurve";
            this.btnJoinCurve.Size = new System.Drawing.Size(89, 23);
            this.btnJoinCurve.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnJoinCurve.TabIndex = 3;
            this.btnJoinCurve.Text = "参与做曲线";
            this.btnJoinCurve.ToFocused = false;
            this.btnJoinCurve.UseVisualStyleBackColor = true;
            this.btnJoinCurve.Click += new System.EventHandler(this.btnJoinCurve_Click);
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(235, 217);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "退出";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // UCCalcIntensity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnJoinCurve);
            this.Controls.Add(this.dgvwCalcIntensity);
            this.Controls.Add(this.txtSampleName);
            this.Controls.Add(this.lblSampleName);
            this.Name = "UCCalcIntensity";
            this.Size = new System.Drawing.Size(386, 251);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCalcIntensity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblSampleName;
        private Skyray.Controls.TextBoxW txtSampleName;
        private Skyray.Controls.DataGridViewW dgvwCalcIntensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private Skyray.Controls.ButtonW btnJoinCurve;
        private Skyray.Controls.ButtonW btnClose;
    }
}
