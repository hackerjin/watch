namespace Skyray.UC
{
    partial class UCQualityResult
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
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.dgvQualityResult = new Skyray.Controls.DataGridViewW();
            this.QualityElementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QualityLines = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQualityResult)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(58, 284);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(158, 283);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "打印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dgvQualityResult
            // 
            this.dgvQualityResult.AllowUserToAddRows = false;
            this.dgvQualityResult.AllowUserToDeleteRows = false;
            this.dgvQualityResult.AllowUserToResizeColumns = false;
            this.dgvQualityResult.AllowUserToResizeRows = false;
            this.dgvQualityResult.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvQualityResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvQualityResult.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvQualityResult.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvQualityResult.ColumnHeadersHeight = 20;
            this.dgvQualityResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQualityResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.QualityElementName,
            this.QualityLines});
            this.dgvQualityResult.Location = new System.Drawing.Point(12, 9);
            this.dgvQualityResult.Name = "dgvQualityResult";
            this.dgvQualityResult.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvQualityResult.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvQualityResult.ReadOnly = true;
            this.dgvQualityResult.RowHeadersVisible = false;
            this.dgvQualityResult.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvQualityResult.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvQualityResult.RowTemplate.Height = 23;
            this.dgvQualityResult.SecondaryLength = 1;
            this.dgvQualityResult.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvQualityResult.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvQualityResult.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvQualityResult.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvQualityResult.ShowEportContextMenu = false;
            this.dgvQualityResult.Size = new System.Drawing.Size(226, 268);
            this.dgvQualityResult.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvQualityResult.TabIndex = 3;
            this.dgvQualityResult.ToPrintCols = null;
            this.dgvQualityResult.ToPrintRows = null;
            // 
            // QualityElementName
            // 
            this.QualityElementName.HeaderText = "元素";
            this.QualityElementName.Name = "QualityElementName";
            this.QualityElementName.ReadOnly = true;
            // 
            // QualityLines
            // 
            this.QualityLines.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.QualityLines.HeaderText = "线系";
            this.QualityLines.Name = "QualityLines";
            this.QualityLines.ReadOnly = true;
            // 
            // UCQualityResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvQualityResult);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnSubmit);
            this.Name = "UCQualityResult";
            this.Size = new System.Drawing.Size(250, 310);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQualityResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnPrint;
        private Skyray.Controls.DataGridViewW dgvQualityResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn QualityElementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn QualityLines;
    }
}
