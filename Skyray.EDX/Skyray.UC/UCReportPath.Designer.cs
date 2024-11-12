namespace Skyray.UC
{
    partial class UCReportPath
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
            this.lbwReportPath = new Skyray.Controls.LabelW();
            this.btnSubmit = new Skyray.Controls.ButtonW();
            this.btwCancel = new Skyray.Controls.ButtonW();
            this.txtBoxPath = new Skyray.Controls.TextBoxW();
            this.btWSelect = new Skyray.Controls.ButtonW();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // lbwReportPath
            // 
            this.lbwReportPath.AutoSize = true;
            this.lbwReportPath.BackColor = System.Drawing.Color.Transparent;
            this.lbwReportPath.Location = new System.Drawing.Point(14, 26);
            this.lbwReportPath.Name = "lbwReportPath";
            this.lbwReportPath.Size = new System.Drawing.Size(91, 13);
            this.lbwReportPath.TabIndex = 0;
            this.lbwReportPath.Text = "报告保存路径：";
            // 
            // btnSubmit
            // 
            this.btnSubmit.bSilver = false;
            this.btnSubmit.Location = new System.Drawing.Point(110, 56);
            this.btnSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 25);
            this.btnSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.ToFocused = false;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btwCancel
            // 
            this.btwCancel.bSilver = false;
            this.btwCancel.Location = new System.Drawing.Point(207, 56);
            this.btwCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btwCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btwCancel.Name = "btwCancel";
            this.btwCancel.Size = new System.Drawing.Size(75, 25);
            this.btwCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btwCancel.TabIndex = 2;
            this.btwCancel.Text = "取消";
            this.btwCancel.ToFocused = false;
            this.btwCancel.UseVisualStyleBackColor = true;
            this.btwCancel.Click += new System.EventHandler(this.btwCancel_Click);
            // 
            // txtBoxPath
            // 
            this.txtBoxPath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtBoxPath.Location = new System.Drawing.Point(110, 21);
            this.txtBoxPath.Name = "txtBoxPath";
            this.txtBoxPath.ReadOnly = true;
            this.txtBoxPath.Size = new System.Drawing.Size(192, 20);
            this.txtBoxPath.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtBoxPath.TabIndex = 3;
            this.txtBoxPath.TextChanged += new System.EventHandler(this.txtBoxPath_TextChanged);
            // 
            // btWSelect
            // 
            this.btWSelect.bSilver = false;
            this.btWSelect.Location = new System.Drawing.Point(305, 21);
            this.btWSelect.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWSelect.MenuPos = new System.Drawing.Point(0, 0);
            this.btWSelect.Name = "btWSelect";
            this.btWSelect.Size = new System.Drawing.Size(17, 25);
            this.btWSelect.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWSelect.TabIndex = 4;
            this.btWSelect.Text = "...";
            this.btWSelect.ToFocused = false;
            this.btWSelect.UseVisualStyleBackColor = true;
            this.btWSelect.Click += new System.EventHandler(this.btWSelect_Click);
            // 
            // UCReportPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btWSelect);
            this.Controls.Add(this.txtBoxPath);
            this.Controls.Add(this.btwCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lbwReportPath);
            this.Name = "UCReportPath";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(333, 85);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lbwReportPath;
        private Skyray.Controls.ButtonW btnSubmit;
        private Skyray.Controls.ButtonW btwCancel;
        private Skyray.Controls.TextBoxW txtBoxPath;
        private Skyray.Controls.ButtonW btWSelect;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}
