namespace Skyray.UC
{
    partial class FrmScanPure
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
            this.lblPureAlertInfo = new System.Windows.Forms.Label();
            this.btnExist = new Skyray.Controls.ButtonW();
            this.btnSubmit = new Skyray.Controls.ButtonW();
            this.btnOpenSpec = new Skyray.Controls.ButtonW();
            this.btnSkip = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // lblPureAlertInfo
            // 
            this.lblPureAlertInfo.AutoSize = true;
            this.lblPureAlertInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPureAlertInfo.Location = new System.Drawing.Point(59, 41);
            this.lblPureAlertInfo.MaximumSize = new System.Drawing.Size(318, 165);
            this.lblPureAlertInfo.Name = "lblPureAlertInfo";
            this.lblPureAlertInfo.Size = new System.Drawing.Size(35, 12);
            this.lblPureAlertInfo.TabIndex = 8;
            this.lblPureAlertInfo.Text = "Test!";
            // 
            // btnExist
            // 
            this.btnExist.bSilver = false;
            this.btnExist.Location = new System.Drawing.Point(254, 92);
            this.btnExist.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnExist.MenuPos = new System.Drawing.Point(0, 0);
            this.btnExist.Name = "btnExist";
            this.btnExist.Size = new System.Drawing.Size(75, 23);
            this.btnExist.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnExist.TabIndex = 7;
            this.btnExist.Text = "退出";
            this.btnExist.ToFocused = false;
            this.btnExist.UseVisualStyleBackColor = true;
            this.btnExist.Click += new System.EventHandler(this.btnExist_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.bSilver = false;
            this.btnSubmit.Location = new System.Drawing.Point(173, 92);
            this.btnSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.ToFocused = false;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnOpenSpec
            // 
            this.btnOpenSpec.bSilver = false;
            this.btnOpenSpec.Location = new System.Drawing.Point(93, 92);
            this.btnOpenSpec.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOpenSpec.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOpenSpec.Name = "btnOpenSpec";
            this.btnOpenSpec.Size = new System.Drawing.Size(75, 23);
            this.btnOpenSpec.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOpenSpec.TabIndex = 10;
            this.btnOpenSpec.Text = "打开谱";
            this.btnOpenSpec.ToFocused = false;
            this.btnOpenSpec.UseVisualStyleBackColor = true;
            this.btnOpenSpec.Click += new System.EventHandler(this.btnOpenSpec_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.bSilver = false;
            this.btnSkip.Location = new System.Drawing.Point(12, 92);
            this.btnSkip.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSkip.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(75, 23);
            this.btnSkip.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSkip.TabIndex = 9;
            this.btnSkip.Text = "跳过";
            this.btnSkip.ToFocused = false;
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // FrmScanPure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 127);
            this.Controls.Add(this.btnOpenSpec);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.lblPureAlertInfo);
            this.Controls.Add(this.btnExist);
            this.Controls.Add(this.btnSubmit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmScanPure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "扫描纯元素谱";
            this.Load += new System.EventHandler(this.FrmScanPure_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPureAlertInfo;
        private Skyray.Controls.ButtonW btnExist;
        private Skyray.Controls.ButtonW btnSubmit;
        private Skyray.Controls.ButtonW btnOpenSpec;
        private Skyray.Controls.ButtonW btnSkip;
    }
}