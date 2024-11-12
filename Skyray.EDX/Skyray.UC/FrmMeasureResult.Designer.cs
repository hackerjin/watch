namespace Skyray.UC
{
    partial class FrmMeasureResult
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
            this.labelResultSample = new System.Windows.Forms.Label();
            this.textBoxSampleName = new System.Windows.Forms.TextBox();
            this.buttonMeasureSubmit = new System.Windows.Forms.Button();
            this.buttonMeasureCancel = new System.Windows.Forms.Button();
            this.dataGridViewW1 = new Skyray.Controls.DataGridViewW();
            this.ElementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Intensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Context = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBoxWeight = new System.Windows.Forms.CheckBox();
            this.textBoxResultWeight = new System.Windows.Forms.TextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.labelInformation = new System.Windows.Forms.Label();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnWriteReport = new System.Windows.Forms.Button();
            this.chKShowKValue = new Skyray.Controls.CheckBoxW();
            this.btnPrintReport = new System.Windows.Forms.Button();
            this.lblContentBit = new Skyray.Controls.LabelW();
            this.cobBoxShowBit = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNewCurve = new System.Windows.Forms.Button();
            this.btnJoinCurve = new System.Windows.Forms.Button();
            this.btnExportToSQL = new System.Windows.Forms.Button();
            this.btnHotKey = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelResultSample
            // 
            this.labelResultSample.AutoSize = true;
            this.labelResultSample.Location = new System.Drawing.Point(3, 17);
            this.labelResultSample.Name = "labelResultSample";
            this.labelResultSample.Size = new System.Drawing.Size(53, 12);
            this.labelResultSample.TabIndex = 0;
            this.labelResultSample.Text = "样品名称";
            // 
            // textBoxSampleName
            // 
            this.textBoxSampleName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBoxSampleName.Location = new System.Drawing.Point(56, 13);
            this.textBoxSampleName.Name = "textBoxSampleName";
            this.textBoxSampleName.ReadOnly = true;
            this.textBoxSampleName.Size = new System.Drawing.Size(80, 21);
            this.textBoxSampleName.TabIndex = 1;
            // 
            // buttonMeasureSubmit
            // 
            this.buttonMeasureSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMeasureSubmit.Location = new System.Drawing.Point(343, 327);
            this.buttonMeasureSubmit.Name = "buttonMeasureSubmit";
            this.buttonMeasureSubmit.Size = new System.Drawing.Size(68, 23);
            this.buttonMeasureSubmit.TabIndex = 3;
            this.buttonMeasureSubmit.Text = "确定";
            this.buttonMeasureSubmit.UseVisualStyleBackColor = true;
            this.buttonMeasureSubmit.Visible = false;
            this.buttonMeasureSubmit.Click += new System.EventHandler(this.buttonMeasureSubmit_Click);
            // 
            // buttonMeasureCancel
            // 
            this.buttonMeasureCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMeasureCancel.Location = new System.Drawing.Point(341, 327);
            this.buttonMeasureCancel.Name = "buttonMeasureCancel";
            this.buttonMeasureCancel.Size = new System.Drawing.Size(68, 23);
            this.buttonMeasureCancel.TabIndex = 4;
            this.buttonMeasureCancel.Text = "确定";
            this.buttonMeasureCancel.UseVisualStyleBackColor = true;
            this.buttonMeasureCancel.Click += new System.EventHandler(this.buttonMeasureCancel_Click);
            // 
            // dataGridViewW1
            // 
            this.dataGridViewW1.AllowUserToAddRows = false;
            this.dataGridViewW1.AllowUserToDeleteRows = false;
            this.dataGridViewW1.AllowUserToResizeColumns = false;
            this.dataGridViewW1.AllowUserToResizeRows = false;
            this.dataGridViewW1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewW1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewW1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dataGridViewW1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewW1.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dataGridViewW1.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dataGridViewW1.ColumnHeadersHeight = 20;
            this.dataGridViewW1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewW1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ElementName,
            this.Intensity,
            this.Context});
            this.dataGridViewW1.Location = new System.Drawing.Point(6, 43);
            this.dataGridViewW1.Name = "dataGridViewW1";
            this.dataGridViewW1.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dataGridViewW1.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dataGridViewW1.ReadOnly = true;
            this.dataGridViewW1.RowHeadersVisible = false;
            this.dataGridViewW1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dataGridViewW1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewW1.RowTemplate.Height = 23;
            this.dataGridViewW1.SecondaryLength = 1;
            this.dataGridViewW1.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dataGridViewW1.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dataGridViewW1.SelectedRowColor1 = System.Drawing.Color.White;
            this.dataGridViewW1.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dataGridViewW1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewW1.ShowEportContextMenu = false;
            this.dataGridViewW1.Size = new System.Drawing.Size(327, 308);
            this.dataGridViewW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.dataGridViewW1.TabIndex = 2;
            this.dataGridViewW1.ToPrintCols = null;
            this.dataGridViewW1.ToPrintRows = null;
            this.dataGridViewW1.SelectionChanged += new System.EventHandler(this.dataGridViewW1_SelectionChanged);
            // 
            // ElementName
            // 
            this.ElementName.HeaderText = "元素";
            this.ElementName.Name = "ElementName";
            this.ElementName.ReadOnly = true;
            this.ElementName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Intensity
            // 
            this.Intensity.HeaderText = "强度";
            this.Intensity.Name = "Intensity";
            this.Intensity.ReadOnly = true;
            this.Intensity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Intensity.Visible = false;
            // 
            // Context
            // 
            this.Context.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Context.HeaderText = "含量";
            this.Context.Name = "Context";
            this.Context.ReadOnly = true;
            this.Context.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // checkBoxWeight
            // 
            this.checkBoxWeight.AutoSize = true;
            this.checkBoxWeight.Location = new System.Drawing.Point(139, 15);
            this.checkBoxWeight.Name = "checkBoxWeight";
            this.checkBoxWeight.Size = new System.Drawing.Size(48, 16);
            this.checkBoxWeight.TabIndex = 5;
            this.checkBoxWeight.Text = "重量";
            this.checkBoxWeight.UseVisualStyleBackColor = true;
            this.checkBoxWeight.CheckedChanged += new System.EventHandler(this.checkBoxWeight_CheckedChanged);
            // 
            // textBoxResultWeight
            // 
            this.textBoxResultWeight.Location = new System.Drawing.Point(195, 12);
            this.textBoxResultWeight.Name = "textBoxResultWeight";
            this.textBoxResultWeight.Size = new System.Drawing.Size(80, 21);
            this.textBoxResultWeight.TabIndex = 6;
            this.textBoxResultWeight.TextChanged += new System.EventHandler(this.textBoxResultWeight_TextChanged);
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(277, 17);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(11, 12);
            this.labelUnit.TabIndex = 7;
            this.labelUnit.Text = "g";
            // 
            // labelInformation
            // 
            this.labelInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInformation.AutoSize = true;
            this.labelInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInformation.ForeColor = System.Drawing.Color.Red;
            this.labelInformation.Location = new System.Drawing.Point(128, 365);
            this.labelInformation.Name = "labelInformation";
            this.labelInformation.Size = new System.Drawing.Size(0, 31);
            this.labelInformation.TabIndex = 8;
            this.labelInformation.Visible = false;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrint.Location = new System.Drawing.Point(341, 289);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(68, 23);
            this.buttonPrint.TabIndex = 9;
            this.buttonPrint.Text = "保存报告";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Element";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Intensity";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Content";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 102;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Weight";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // btnWriteReport
            // 
            this.btnWriteReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWriteReport.Location = new System.Drawing.Point(341, 211);
            this.btnWriteReport.Name = "btnWriteReport";
            this.btnWriteReport.Size = new System.Drawing.Size(68, 23);
            this.btnWriteReport.TabIndex = 10;
            this.btnWriteReport.Text = "写入报告";
            this.btnWriteReport.UseVisualStyleBackColor = true;
            this.btnWriteReport.Visible = false;
            this.btnWriteReport.Click += new System.EventHandler(this.btnWriteReport_Click);
            // 
            // chKShowKValue
            // 
            this.chKShowKValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chKShowKValue.AutoSize = true;
            this.chKShowKValue.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chKShowKValue.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chKShowKValue.ForeColor = System.Drawing.Color.Red;
            this.chKShowKValue.Location = new System.Drawing.Point(6, 374);
            this.chKShowKValue.Name = "chKShowKValue";
            this.chKShowKValue.Size = new System.Drawing.Size(103, 18);
            this.chKShowKValue.Style = Skyray.Controls.Style.Office2007Blue;
            this.chKShowKValue.TabIndex = 16;
            this.chKShowKValue.Text = "是否显示K值";
            this.chKShowKValue.UseVisualStyleBackColor = true;
            this.chKShowKValue.Visible = false;
            this.chKShowKValue.CheckedChanged += new System.EventHandler(this.chKShowKValue_CheckedChanged);
            // 
            // btnPrintReport
            // 
            this.btnPrintReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintReport.Location = new System.Drawing.Point(341, 250);
            this.btnPrintReport.Name = "btnPrintReport";
            this.btnPrintReport.Size = new System.Drawing.Size(68, 23);
            this.btnPrintReport.TabIndex = 10;
            this.btnPrintReport.Text = "打印";
            this.btnPrintReport.UseVisualStyleBackColor = true;
            this.btnPrintReport.Click += new System.EventHandler(this.btnPrintReport_Click);
            // 
            // lblContentBit
            // 
            this.lblContentBit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContentBit.AutoSize = true;
            this.lblContentBit.BackColor = System.Drawing.Color.Transparent;
            this.lblContentBit.Location = new System.Drawing.Point(339, 133);
            this.lblContentBit.Name = "lblContentBit";
            this.lblContentBit.Size = new System.Drawing.Size(65, 12);
            this.lblContentBit.TabIndex = 17;
            this.lblContentBit.Text = "数据位数：";
            // 
            // cobBoxShowBit
            // 
            this.cobBoxShowBit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cobBoxShowBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobBoxShowBit.FormattingEnabled = true;
            this.cobBoxShowBit.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cobBoxShowBit.Location = new System.Drawing.Point(339, 148);
            this.cobBoxShowBit.Name = "cobBoxShowBit";
            this.cobBoxShowBit.Size = new System.Drawing.Size(70, 20);
            this.cobBoxShowBit.TabIndex = 18;
            this.cobBoxShowBit.SelectedIndexChanged += new System.EventHandler(this.cobBoxShowBit_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnHotKey);
            this.panel1.Controls.Add(this.btnExportToSQL);
            this.panel1.Controls.Add(this.btnNewCurve);
            this.panel1.Controls.Add(this.btnJoinCurve);
            this.panel1.Controls.Add(this.cobBoxShowBit);
            this.panel1.Controls.Add(this.lblContentBit);
            this.panel1.Controls.Add(this.chKShowKValue);
            this.panel1.Controls.Add(this.btnPrintReport);
            this.panel1.Controls.Add(this.btnWriteReport);
            this.panel1.Controls.Add(this.buttonPrint);
            this.panel1.Controls.Add(this.labelInformation);
            this.panel1.Controls.Add(this.labelUnit);
            this.panel1.Controls.Add(this.textBoxResultWeight);
            this.panel1.Controls.Add(this.checkBoxWeight);
            this.panel1.Controls.Add(this.buttonMeasureCancel);
            this.panel1.Controls.Add(this.buttonMeasureSubmit);
            this.panel1.Controls.Add(this.dataGridViewW1);
            this.panel1.Controls.Add(this.textBoxSampleName);
            this.panel1.Controls.Add(this.labelResultSample);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(419, 404);
            this.panel1.TabIndex = 19;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnNewCurve
            // 
            this.btnNewCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewCurve.Location = new System.Drawing.Point(343, 43);
            this.btnNewCurve.Name = "btnNewCurve";
            this.btnNewCurve.Size = new System.Drawing.Size(68, 23);
            this.btnNewCurve.TabIndex = 20;
            this.btnNewCurve.Text = "新建曲线";
            this.btnNewCurve.UseVisualStyleBackColor = true;
            this.btnNewCurve.Click += new System.EventHandler(this.btnNewCurve_Click);
            // 
            // btnJoinCurve
            // 
            this.btnJoinCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJoinCurve.Location = new System.Drawing.Point(343, 12);
            this.btnJoinCurve.Name = "btnJoinCurve";
            this.btnJoinCurve.Size = new System.Drawing.Size(68, 23);
            this.btnJoinCurve.TabIndex = 19;
            this.btnJoinCurve.Text = "参与曲线";
            this.btnJoinCurve.UseVisualStyleBackColor = true;
            this.btnJoinCurve.Click += new System.EventHandler(this.btnJoinCurve_Click);
            // 
            // btnExportToSQL
            // 
            this.btnExportToSQL.Location = new System.Drawing.Point(341, 207);
            this.btnExportToSQL.Name = "btnExportToSQL";
            this.btnExportToSQL.Size = new System.Drawing.Size(70, 23);
            this.btnExportToSQL.TabIndex = 21;
            this.btnExportToSQL.Text = "导入SQL";
            this.btnExportToSQL.UseVisualStyleBackColor = true;
            this.btnExportToSQL.Visible = false;
            this.btnExportToSQL.Click += new System.EventHandler(this.btnExportSQL_Click);
            // 
            // btnHotKey
            // 
            this.btnHotKey.Location = new System.Drawing.Point(343, 72);
            this.btnHotKey.Name = "btnHotKey";
            this.btnHotKey.Size = new System.Drawing.Size(70, 23);
            this.btnHotKey.TabIndex = 22;
            this.btnHotKey.Text = "热键";
            this.btnHotKey.UseVisualStyleBackColor = true;
            this.btnHotKey.Visible = false;
            this.btnHotKey.Click += new System.EventHandler(this.btnHotKey_Click);
            // 
            // FrmMeasureResult
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(419, 404);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMeasureResult";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "测量结果";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMeasureResult_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelResultSample;
        private System.Windows.Forms.TextBox textBoxSampleName;
        private Skyray.Controls.DataGridViewW dataGridViewW1;
        private System.Windows.Forms.Button buttonMeasureSubmit;
        private System.Windows.Forms.Button buttonMeasureCancel;
        private System.Windows.Forms.CheckBox checkBoxWeight;
        private System.Windows.Forms.TextBox textBoxResultWeight;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.Label labelInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button btnWriteReport;
        private Skyray.Controls.CheckBoxW chKShowKValue;
        private System.Windows.Forms.Button btnPrintReport;
        private Skyray.Controls.LabelW lblContentBit;
        private System.Windows.Forms.ComboBox cobBoxShowBit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ElementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Intensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Context;
        private System.Windows.Forms.Button btnNewCurve;
        private System.Windows.Forms.Button btnJoinCurve;
        private System.Windows.Forms.Button btnExportToSQL;
        private System.Windows.Forms.Button btnHotKey;
    }
}