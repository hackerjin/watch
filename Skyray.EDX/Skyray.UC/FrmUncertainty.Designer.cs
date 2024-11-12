namespace Skyray.UC
{
    partial class FrmUncertainty
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
            this.btnPrintReport = new Skyray.Controls.ButtonW();
            this.btnReport = new Skyray.Controls.ButtonW();
            this.lblcertainty = new Skyray.Controls.LabelW();
            this.SuspendLayout();
            // 
            // btnPrintReport
            // 
            this.btnPrintReport.bSilver = false;
            this.btnPrintReport.Location = new System.Drawing.Point(172, 281);
            this.btnPrintReport.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnPrintReport.MenuPos = new System.Drawing.Point(0, 0);
            this.btnPrintReport.Name = "btnPrintReport";
            this.btnPrintReport.Size = new System.Drawing.Size(76, 33);
            this.btnPrintReport.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnPrintReport.TabIndex = 6;
            this.btnPrintReport.Text = "打印";
            this.btnPrintReport.ToFocused = false;
            this.btnPrintReport.UseVisualStyleBackColor = true;
            this.btnPrintReport.Click += new System.EventHandler(this.btnPrintReport_Click);
            // 
            // btnReport
            // 
            this.btnReport.bSilver = false;
            this.btnReport.Location = new System.Drawing.Point(34, 281);
            this.btnReport.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnReport.MenuPos = new System.Drawing.Point(0, 0);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 33);
            this.btnReport.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnReport.TabIndex = 7;
            this.btnReport.Text = "保存报告";
            this.btnReport.ToFocused = false;
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // lblcertainty
            // 
            this.lblcertainty.AutoSize = true;
            this.lblcertainty.BackColor = System.Drawing.Color.Transparent;
            this.lblcertainty.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblcertainty.Location = new System.Drawing.Point(12, 40);
            this.lblcertainty.Name = "lblcertainty";
            this.lblcertainty.Size = new System.Drawing.Size(56, 14);
            this.lblcertainty.TabIndex = 8;
            this.lblcertainty.Text = "labelW1";
            // 
            // FrmUncertainty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 326);
            this.Controls.Add(this.lblcertainty);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnPrintReport);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUncertainty";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = Skyray.EDX.Common.Info.Uncertainty;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btnPrintReport;
        private Skyray.Controls.ButtonW btnReport;
        private Skyray.Controls.LabelW lblcertainty;
    }
}