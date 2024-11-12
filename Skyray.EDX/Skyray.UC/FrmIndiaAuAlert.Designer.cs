namespace Skyray.UC
{
    partial class FrmIndiaAuAlert
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
            this.lblIndiaAlert = new Skyray.Controls.LabelW();
            this.btnYes = new Skyray.Controls.ButtonW();
            this.btnNo = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // lblIndiaAlert
            // 
            this.lblIndiaAlert.AutoSize = true;
            this.lblIndiaAlert.BackColor = System.Drawing.Color.Transparent;
            this.lblIndiaAlert.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIndiaAlert.Location = new System.Drawing.Point(42, 27);
            this.lblIndiaAlert.Name = "lblIndiaAlert";
            this.lblIndiaAlert.Size = new System.Drawing.Size(213, 12);
            this.lblIndiaAlert.TabIndex = 0;
            this.lblIndiaAlert.Text = "该样品可能为镀金饰品，是否继续？";
            // 
            // btnYes
            // 
            this.btnYes.bSilver = false;
            this.btnYes.Location = new System.Drawing.Point(44, 63);
            this.btnYes.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnYes.MenuPos = new System.Drawing.Point(0, 0);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "是";
            this.btnYes.ToFocused = false;
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.bSilver = false;
            this.btnNo.Location = new System.Drawing.Point(210, 63);
            this.btnNo.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnNo.MenuPos = new System.Drawing.Point(0, 0);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnNo.TabIndex = 2;
            this.btnNo.Text = "否";
            this.btnNo.ToFocused = false;
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // FrmIndiaAuAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(337, 106);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblIndiaAlert);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmIndiaAuAlert";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Warning";
            this.Load += new System.EventHandler(this.FrmIndiaAuAlert_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmIndiaAuAlert_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblIndiaAlert;
        private Skyray.Controls.ButtonW btnYes;
        private Skyray.Controls.ButtonW btnNo;
    }
}