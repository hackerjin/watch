namespace Skyray.UC
{
    partial class FrmStatisticsResult
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chKShowKValue = new Skyray.Controls.CheckBoxW();
            this.labelInformation = new System.Windows.Forms.Label();
            this.btnPrintReport = new System.Windows.Forms.Button();
            this.buttonReport = new System.Windows.Forms.Button();
            this.buttonStaticsCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvMeasureResult = new Skyray.Controls.DataGridViewW();
            this.dgvStaticsResult = new Skyray.Controls.DataGridViewW();
            this.btnExportToSQL = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasureResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStaticsResult)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(647, 418);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.btnExportToSQL);
            this.panel3.Controls.Add(this.chKShowKValue);
            this.panel3.Controls.Add(this.labelInformation);
            this.panel3.Controls.Add(this.btnPrintReport);
            this.panel3.Controls.Add(this.buttonReport);
            this.panel3.Controls.Add(this.buttonStaticsCancel);
            this.panel3.Location = new System.Drawing.Point(0, 346);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(647, 72);
            this.panel3.TabIndex = 1;
            // 
            // chKShowKValue
            // 
            this.chKShowKValue.AutoSize = true;
            this.chKShowKValue.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chKShowKValue.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chKShowKValue.ForeColor = System.Drawing.Color.Red;
            this.chKShowKValue.Location = new System.Drawing.Point(10, 14);
            this.chKShowKValue.Name = "chKShowKValue";
            this.chKShowKValue.Size = new System.Drawing.Size(103, 18);
            this.chKShowKValue.Style = Skyray.Controls.Style.Office2007Blue;
            this.chKShowKValue.TabIndex = 18;
            this.chKShowKValue.Text = "是否显示K值";
            this.chKShowKValue.UseVisualStyleBackColor = true;
            this.chKShowKValue.CheckedChanged += new System.EventHandler(this.chKShowKValue_CheckedChanged);
            // 
            // labelInformation
            // 
            this.labelInformation.AutoSize = true;
            this.labelInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInformation.ForeColor = System.Drawing.Color.Red;
            this.labelInformation.Location = new System.Drawing.Point(124, 10);
            this.labelInformation.Name = "labelInformation";
            this.labelInformation.Size = new System.Drawing.Size(0, 31);
            this.labelInformation.TabIndex = 17;
            // 
            // btnPrintReport
            // 
            this.btnPrintReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintReport.Location = new System.Drawing.Point(12, 45);
            this.btnPrintReport.Name = "btnPrintReport";
            this.btnPrintReport.Size = new System.Drawing.Size(79, 22);
            this.btnPrintReport.TabIndex = 2;
            this.btnPrintReport.Text = "打印";
            this.btnPrintReport.UseVisualStyleBackColor = true;
            this.btnPrintReport.Click += new System.EventHandler(this.btnPrintReport_Click);
            // 
            // buttonReport
            // 
            this.buttonReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReport.Location = new System.Drawing.Point(109, 45);
            this.buttonReport.Name = "buttonReport";
            this.buttonReport.Size = new System.Drawing.Size(79, 23);
            this.buttonReport.TabIndex = 2;
            this.buttonReport.Text = "保存报告";
            this.buttonReport.UseVisualStyleBackColor = true;
            this.buttonReport.Click += new System.EventHandler(this.buttonReport_Click);
            // 
            // buttonStaticsCancel
            // 
            this.buttonStaticsCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStaticsCancel.Location = new System.Drawing.Point(206, 45);
            this.buttonStaticsCancel.Name = "buttonStaticsCancel";
            this.buttonStaticsCancel.Size = new System.Drawing.Size(78, 23);
            this.buttonStaticsCancel.TabIndex = 1;
            this.buttonStaticsCancel.Text = "确定";
            this.buttonStaticsCancel.UseVisualStyleBackColor = true;
            this.buttonStaticsCancel.Click += new System.EventHandler(this.buttonStaticsCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.dgvMeasureResult);
            this.panel2.Controls.Add(this.dgvStaticsResult);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(647, 348);
            this.panel2.TabIndex = 0;
            // 
            // dgvMeasureResult
            // 
            this.dgvMeasureResult.AllowUserToAddRows = false;
            this.dgvMeasureResult.AllowUserToDeleteRows = false;
            this.dgvMeasureResult.AllowUserToResizeRows = false;
            this.dgvMeasureResult.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvMeasureResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMeasureResult.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvMeasureResult.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvMeasureResult.ColumnHeadersHeight = 4;
            this.dgvMeasureResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMeasureResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvMeasureResult.Location = new System.Drawing.Point(0, 186);
            this.dgvMeasureResult.Name = "dgvMeasureResult";
            this.dgvMeasureResult.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvMeasureResult.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvMeasureResult.ReadOnly = true;
            this.dgvMeasureResult.RowHeadersVisible = false;
            this.dgvMeasureResult.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvMeasureResult.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvMeasureResult.RowTemplate.Height = 23;
            this.dgvMeasureResult.SecondaryLength = 1;
            this.dgvMeasureResult.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvMeasureResult.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvMeasureResult.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvMeasureResult.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvMeasureResult.ShowEportContextMenu = true;
            this.dgvMeasureResult.Size = new System.Drawing.Size(647, 160);
            this.dgvMeasureResult.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvMeasureResult.TabIndex = 1;
            this.dgvMeasureResult.ToPrintCols = null;
            this.dgvMeasureResult.ToPrintRows = null;
            // 
            // dgvStaticsResult
            // 
            this.dgvStaticsResult.AllowUserToAddRows = false;
            this.dgvStaticsResult.AllowUserToDeleteRows = false;
            this.dgvStaticsResult.AllowUserToResizeRows = false;
            this.dgvStaticsResult.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvStaticsResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStaticsResult.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvStaticsResult.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvStaticsResult.ColumnHeadersHeight = 4;
            this.dgvStaticsResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStaticsResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvStaticsResult.Location = new System.Drawing.Point(0, 0);
            this.dgvStaticsResult.Name = "dgvStaticsResult";
            this.dgvStaticsResult.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvStaticsResult.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvStaticsResult.ReadOnly = true;
            this.dgvStaticsResult.RowHeadersVisible = false;
            this.dgvStaticsResult.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvStaticsResult.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvStaticsResult.RowTemplate.Height = 23;
            this.dgvStaticsResult.SecondaryLength = 1;
            this.dgvStaticsResult.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvStaticsResult.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvStaticsResult.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvStaticsResult.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvStaticsResult.ShowEportContextMenu = true;
            this.dgvStaticsResult.Size = new System.Drawing.Size(647, 186);
            this.dgvStaticsResult.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvStaticsResult.TabIndex = 0;
            this.dgvStaticsResult.ToPrintCols = null;
            this.dgvStaticsResult.ToPrintRows = null;
            // 
            // btnExportToSQL
            // 
            this.btnExportToSQL.Location = new System.Drawing.Point(533, 44);
            this.btnExportToSQL.Name = "btnExportToSQL";
            this.btnExportToSQL.Size = new System.Drawing.Size(75, 23);
            this.btnExportToSQL.TabIndex = 19;
            this.btnExportToSQL.Text = "导入SQL";
            this.btnExportToSQL.UseVisualStyleBackColor = true;
            this.btnExportToSQL.Visible = false;
            this.btnExportToSQL.Click += new System.EventHandler(this.btnExportSQL_Click);
            // 
            // FrmStatisticsResult
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(647, 418);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmStatisticsResult";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "统计信息";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmStatisticsResult_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasureResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStaticsResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonStaticsCancel;
        private System.Windows.Forms.Panel panel2;
        private Skyray.Controls.DataGridViewW dgvStaticsResult;
        private System.Windows.Forms.Button buttonReport;
        private Skyray.Controls.DataGridViewW dgvMeasureResult;
        private Skyray.Controls.CheckBoxW chKShowKValue;
        private System.Windows.Forms.Label labelInformation;
        private System.Windows.Forms.Button btnPrintReport;
        private System.Windows.Forms.Button btnExportToSQL;
    }
}