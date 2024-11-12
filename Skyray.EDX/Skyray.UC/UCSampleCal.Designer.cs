namespace Skyray.UC
{
    partial class UCSampleCal
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvwStandardDatas = new Skyray.Controls.DataGridViewW();
            this.lblSampleCalInfo = new Skyray.Controls.LabelW();
            this.lblQualifiedInfo = new Skyray.Controls.LabelW();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwStandardDatas)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(499, 188);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(499, 228);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 15);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(582, 26);
            this.progressBar1.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(499, 326);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvwStandardDatas
            // 
            this.dgvwStandardDatas.AllowUserToAddRows = false;
            this.dgvwStandardDatas.AllowUserToResizeColumns = false;
            this.dgvwStandardDatas.AllowUserToResizeRows = false;
            this.dgvwStandardDatas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvwStandardDatas.BackgroundColor = System.Drawing.Color.White;
            this.dgvwStandardDatas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwStandardDatas.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwStandardDatas.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwStandardDatas.ColumnHeadersHeight = 24;
            this.dgvwStandardDatas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwStandardDatas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwStandardDatas.Location = new System.Drawing.Point(8, 51);
            this.dgvwStandardDatas.Name = "dgvwStandardDatas";
            this.dgvwStandardDatas.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwStandardDatas.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwStandardDatas.RowHeadersVisible = false;
            this.dgvwStandardDatas.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwStandardDatas.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwStandardDatas.RowTemplate.Height = 23;
            this.dgvwStandardDatas.SecondaryLength = 1;
            this.dgvwStandardDatas.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwStandardDatas.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwStandardDatas.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwStandardDatas.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.dgvwStandardDatas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwStandardDatas.ShowEportContextMenu = true;
            this.dgvwStandardDatas.Size = new System.Drawing.Size(460, 264);
            this.dgvwStandardDatas.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwStandardDatas.TabIndex = 26;
            this.dgvwStandardDatas.ToPrintCols = null;
            this.dgvwStandardDatas.ToPrintRows = null;
            // 
            // lblSampleCalInfo
            // 
            this.lblSampleCalInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblSampleCalInfo.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSampleCalInfo.Location = new System.Drawing.Point(475, 58);
            this.lblSampleCalInfo.Name = "lblSampleCalInfo";
            this.lblSampleCalInfo.Size = new System.Drawing.Size(112, 114);
            this.lblSampleCalInfo.TabIndex = 28;
            this.lblSampleCalInfo.Text = "校正样名";
            // 
            // lblQualifiedInfo
            // 
            this.lblQualifiedInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblQualifiedInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblQualifiedInfo.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblQualifiedInfo.Location = new System.Drawing.Point(8, 323);
            this.lblQualifiedInfo.Name = "lblQualifiedInfo";
            this.lblQualifiedInfo.Size = new System.Drawing.Size(460, 26);
            this.lblQualifiedInfo.TabIndex = 29;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(499, 277);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // UCSampleCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblQualifiedInfo);
            this.Controls.Add(this.lblSampleCalInfo);
            this.Controls.Add(this.dgvwStandardDatas);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Name = "UCSampleCal";
            this.Size = new System.Drawing.Size(600, 360);
            this.Load += new System.EventHandler(this.UCSampleCal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwStandardDatas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnClose;
        private Skyray.Controls.DataGridViewW dgvwStandardDatas;
        private Skyray.Controls.LabelW lblSampleCalInfo;
        private Skyray.Controls.LabelW lblQualifiedInfo;
        private System.Windows.Forms.Button btnSave;
    }
}
