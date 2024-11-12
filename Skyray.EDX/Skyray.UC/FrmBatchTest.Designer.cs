namespace Skyray.UC
{
    partial class FrmBatchTest
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnBTBrowser = new Skyray.Controls.ButtonEx();
            this.pnlOtherInfos = new System.Windows.Forms.Panel();
            this.btnConfirmSingle = new Skyray.Controls.ButtonEx();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.tbxSampleName = new System.Windows.Forms.TextBox();
            this.lblSampleName = new System.Windows.Forms.Label();
            this.tbxSupplier = new System.Windows.Forms.TextBox();
            this.btnBTClear = new Skyray.Controls.ButtonEx();
            this.btnBTStart = new Skyray.Controls.ButtonEx();
            this.btnExportToExcel = new Skyray.Controls.ButtonEx();
            this.dgvContent = new System.Windows.Forms.DataGridView();
            this.grpBTInputExcel = new Skyray.Controls.Grouper();
            this.grpBTSingleInput = new Skyray.Controls.Grouper();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContent)).BeginInit();
            this.grpBTInputExcel.SuspendLayout();
            this.grpBTSingleInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBTBrowser
            // 
            this.btnBTBrowser.BackColor = System.Drawing.Color.Transparent;
            this.btnBTBrowser.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnBTBrowser.EnteredColor = System.Drawing.Color.Khaki;
            this.btnBTBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBTBrowser.FocusedColor = System.Drawing.Color.SteelBlue;
            this.btnBTBrowser.Location = new System.Drawing.Point(94, 41);
            this.btnBTBrowser.Name = "btnBTBrowser";
            this.btnBTBrowser.PressedColor = System.Drawing.Color.Orange;
            this.btnBTBrowser.Radius = 6F;
            this.btnBTBrowser.Size = new System.Drawing.Size(80, 30);
            this.btnBTBrowser.TabIndex = 0;
            this.btnBTBrowser.Text = "打开Excel";
            this.btnBTBrowser.UseVisualStyleBackColor = true;
            this.btnBTBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // pnlOtherInfos
            // 
            this.pnlOtherInfos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlOtherInfos.AutoScroll = true;
            this.pnlOtherInfos.Location = new System.Drawing.Point(3, 78);
            this.pnlOtherInfos.Name = "pnlOtherInfos";
            this.pnlOtherInfos.Size = new System.Drawing.Size(260, 211);
            this.pnlOtherInfos.TabIndex = 11;
            // 
            // btnConfirmSingle
            // 
            this.btnConfirmSingle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfirmSingle.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirmSingle.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnConfirmSingle.EnteredColor = System.Drawing.Color.Khaki;
            this.btnConfirmSingle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmSingle.FocusedColor = System.Drawing.Color.SteelBlue;
            this.btnConfirmSingle.Location = new System.Drawing.Point(94, 304);
            this.btnConfirmSingle.Name = "btnConfirmSingle";
            this.btnConfirmSingle.PressedColor = System.Drawing.Color.Orange;
            this.btnConfirmSingle.Radius = 6F;
            this.btnConfirmSingle.Size = new System.Drawing.Size(80, 30);
            this.btnConfirmSingle.TabIndex = 3;
            this.btnConfirmSingle.Text = "确定";
            this.btnConfirmSingle.UseVisualStyleBackColor = true;
            this.btnConfirmSingle.Click += new System.EventHandler(this.btnConfirmSingle_Click);
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(16, 57);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(47, 12);
            this.lblSupplier.TabIndex = 7;
            this.lblSupplier.Text = "供应商:";
            // 
            // tbxSampleName
            // 
            this.tbxSampleName.Location = new System.Drawing.Point(94, 27);
            this.tbxSampleName.Name = "tbxSampleName";
            this.tbxSampleName.Size = new System.Drawing.Size(144, 21);
            this.tbxSampleName.TabIndex = 1;
            // 
            // lblSampleName
            // 
            this.lblSampleName.AutoSize = true;
            this.lblSampleName.Location = new System.Drawing.Point(16, 32);
            this.lblSampleName.Name = "lblSampleName";
            this.lblSampleName.Size = new System.Drawing.Size(59, 12);
            this.lblSampleName.TabIndex = 8;
            this.lblSampleName.Text = "样品名称:";
            // 
            // tbxSupplier
            // 
            this.tbxSupplier.Location = new System.Drawing.Point(94, 52);
            this.tbxSupplier.Name = "tbxSupplier";
            this.tbxSupplier.Size = new System.Drawing.Size(144, 21);
            this.tbxSupplier.TabIndex = 2;
            // 
            // btnBTClear
            // 
            this.btnBTClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBTClear.BackColor = System.Drawing.Color.Transparent;
            this.btnBTClear.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnBTClear.EnteredColor = System.Drawing.Color.Khaki;
            this.btnBTClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBTClear.FocusedColor = System.Drawing.Color.SteelBlue;
            this.btnBTClear.Location = new System.Drawing.Point(667, 413);
            this.btnBTClear.Name = "btnBTClear";
            this.btnBTClear.PressedColor = System.Drawing.Color.Orange;
            this.btnBTClear.Radius = 6F;
            this.btnBTClear.Size = new System.Drawing.Size(80, 30);
            this.btnBTClear.TabIndex = 6;
            this.btnBTClear.Text = "清空";
            this.btnBTClear.UseVisualStyleBackColor = true;
            this.btnBTClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnBTStart
            // 
            this.btnBTStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBTStart.BackColor = System.Drawing.Color.Transparent;
            this.btnBTStart.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnBTStart.EnteredColor = System.Drawing.Color.Khaki;
            this.btnBTStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBTStart.FocusedColor = System.Drawing.Color.SteelBlue;
            this.btnBTStart.Location = new System.Drawing.Point(279, 413);
            this.btnBTStart.Name = "btnBTStart";
            this.btnBTStart.PressedColor = System.Drawing.Color.Orange;
            this.btnBTStart.Radius = 6F;
            this.btnBTStart.Size = new System.Drawing.Size(80, 30);
            this.btnBTStart.TabIndex = 4;
            this.btnBTStart.Text = "开始";
            this.btnBTStart.UseVisualStyleBackColor = true;
            this.btnBTStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportToExcel.BackColor = System.Drawing.Color.Transparent;
            this.btnExportToExcel.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnExportToExcel.EnteredColor = System.Drawing.Color.Khaki;
            this.btnExportToExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportToExcel.FocusedColor = System.Drawing.Color.SteelBlue;
            this.btnExportToExcel.Location = new System.Drawing.Point(559, 413);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.PressedColor = System.Drawing.Color.Orange;
            this.btnExportToExcel.Radius = 6F;
            this.btnExportToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExportToExcel.TabIndex = 5;
            this.btnExportToExcel.Text = "ToExcel";
            this.btnExportToExcel.UseVisualStyleBackColor = true;
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // dgvContent
            // 
            this.dgvContent.AllowUserToAddRows = false;
            this.dgvContent.AllowUserToDeleteRows = false;
            this.dgvContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvContent.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvContent.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvContent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvContent.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvContent.Location = new System.Drawing.Point(279, 9);
            this.dgvContent.Name = "dgvContent";
            this.dgvContent.ReadOnly = true;
            this.dgvContent.RowHeadersVisible = false;
            this.dgvContent.RowTemplate.Height = 23;
            this.dgvContent.Size = new System.Drawing.Size(468, 401);
            this.dgvContent.TabIndex = 22;
            // 
            // grpBTInputExcel
            // 
            this.grpBTInputExcel.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpBTInputExcel.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpBTInputExcel.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpBTInputExcel.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpBTInputExcel.BorderThickness = 1F;
            this.grpBTInputExcel.BorderTopOnly = false;
            this.grpBTInputExcel.Controls.Add(this.btnBTBrowser);
            this.grpBTInputExcel.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpBTInputExcel.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grpBTInputExcel.GroupImage = null;
            this.grpBTInputExcel.GroupTitle = "Excel导入";
            this.grpBTInputExcel.HeaderRoundCorners = 4;
            this.grpBTInputExcel.Location = new System.Drawing.Point(4, -2);
            this.grpBTInputExcel.Name = "grpBTInputExcel";
            this.grpBTInputExcel.PaintGroupBox = false;
            this.grpBTInputExcel.RoundCorners = 4;
            this.grpBTInputExcel.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpBTInputExcel.ShadowControl = false;
            this.grpBTInputExcel.ShadowThickness = 3;
            this.grpBTInputExcel.Size = new System.Drawing.Size(269, 104);
            this.grpBTInputExcel.TabIndex = 25;
            this.grpBTInputExcel.TextLineSpace = 2;
            this.grpBTInputExcel.TitleLeftSpace = 18;
            // 
            // grpBTSingleInput
            // 
            this.grpBTSingleInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.grpBTSingleInput.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpBTSingleInput.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpBTSingleInput.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpBTSingleInput.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpBTSingleInput.BorderThickness = 1F;
            this.grpBTSingleInput.BorderTopOnly = false;
            this.grpBTSingleInput.Controls.Add(this.tbxSampleName);
            this.grpBTSingleInput.Controls.Add(this.pnlOtherInfos);
            this.grpBTSingleInput.Controls.Add(this.tbxSupplier);
            this.grpBTSingleInput.Controls.Add(this.lblSampleName);
            this.grpBTSingleInput.Controls.Add(this.btnConfirmSingle);
            this.grpBTSingleInput.Controls.Add(this.lblSupplier);
            this.grpBTSingleInput.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpBTSingleInput.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grpBTSingleInput.GroupImage = null;
            this.grpBTSingleInput.GroupTitle = "单个输入";
            this.grpBTSingleInput.HeaderRoundCorners = 4;
            this.grpBTSingleInput.Location = new System.Drawing.Point(4, 109);
            this.grpBTSingleInput.Name = "grpBTSingleInput";
            this.grpBTSingleInput.PaintGroupBox = false;
            this.grpBTSingleInput.RoundCorners = 4;
            this.grpBTSingleInput.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpBTSingleInput.ShadowControl = false;
            this.grpBTSingleInput.ShadowThickness = 3;
            this.grpBTSingleInput.Size = new System.Drawing.Size(269, 343);
            this.grpBTSingleInput.TabIndex = 26;
            this.grpBTSingleInput.TextLineSpace = 2;
            this.grpBTSingleInput.TitleLeftSpace = 18;
            // 
            // FrmBatchTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(750, 455);
            this.Controls.Add(this.btnExportToExcel);
            this.Controls.Add(this.btnBTClear);
            this.Controls.Add(this.btnBTStart);
            this.Controls.Add(this.grpBTSingleInput);
            this.Controls.Add(this.grpBTInputExcel);
            this.Controls.Add(this.dgvContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "FrmBatchTest";
            this.Text = "FrmBatchTest";
            this.Load += new System.EventHandler(this.FrmBatchTest_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmBatchTest_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContent)).EndInit();
            this.grpBTInputExcel.ResumeLayout(false);
            this.grpBTSingleInput.ResumeLayout(false);
            this.grpBTSingleInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonEx btnBTBrowser;
        private System.Windows.Forms.Panel pnlOtherInfos;
        private Skyray.Controls.ButtonEx btnConfirmSingle;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.TextBox tbxSampleName;
        private System.Windows.Forms.Label lblSampleName;
        private System.Windows.Forms.TextBox tbxSupplier;
        private Skyray.Controls.ButtonEx btnBTClear;
        private Skyray.Controls.ButtonEx btnExportToExcel;
        private System.Windows.Forms.DataGridView dgvContent;
        private Skyray.Controls.ButtonEx btnBTStart;
        private Skyray.Controls.Grouper grpBTInputExcel;
        private Skyray.Controls.Grouper grpBTSingleInput;
    }
}