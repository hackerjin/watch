namespace Skyray.UC
{
	partial class FrmDialog
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
            this.buttonWSubmit = new Skyray.Controls.ButtonW();
            this.buttonWCancel = new Skyray.Controls.ButtonW();
            this.lblAlertInfo = new System.Windows.Forms.Label();
            this.vPicBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.vPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonWSubmit
            // 
            this.buttonWSubmit.bSilver = false;
            this.buttonWSubmit.Location = new System.Drawing.Point(139, 81);
            this.buttonWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.Name = "buttonWSubmit";
            this.buttonWSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWSubmit.TabIndex = 0;
            this.buttonWSubmit.Text = "确定";
            this.buttonWSubmit.ToFocused = false;
            this.buttonWSubmit.UseVisualStyleBackColor = true;
            this.buttonWSubmit.Click += new System.EventHandler(this.buttonWSubmit_Click);
            // 
            // buttonWCancel
            // 
            this.buttonWCancel.bSilver = false;
            this.buttonWCancel.Location = new System.Drawing.Point(235, 81);
            this.buttonWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWCancel.Name = "buttonWCancel";
            this.buttonWCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWCancel.TabIndex = 1;
            this.buttonWCancel.Text = "取消";
            this.buttonWCancel.ToFocused = false;
            this.buttonWCancel.UseVisualStyleBackColor = true;
            this.buttonWCancel.Click += new System.EventHandler(this.buttonWCancel_Click);
            // 
            // lblAlertInfo
            // 
            this.lblAlertInfo.AutoSize = true;
            this.lblAlertInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblAlertInfo.Location = new System.Drawing.Point(90, 44);
            this.lblAlertInfo.MaximumSize = new System.Drawing.Size(318, 165);
            this.lblAlertInfo.Name = "lblAlertInfo";
            this.lblAlertInfo.Size = new System.Drawing.Size(35, 12);
            this.lblAlertInfo.TabIndex = 5;
            this.lblAlertInfo.Text = "Test!";
            // 
            // vPicBox
            // 
            this.vPicBox.BackColor = System.Drawing.Color.Transparent;
            this.vPicBox.Location = new System.Drawing.Point(35, 34);
            this.vPicBox.Name = "vPicBox";
            this.vPicBox.Size = new System.Drawing.Size(32, 32);
            this.vPicBox.TabIndex = 6;
            this.vPicBox.TabStop = false;
            // 
            // FrmDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(320, 112);
            this.Controls.Add(this.vPicBox);
            this.Controls.Add(this.lblAlertInfo);
            this.Controls.Add(this.buttonWCancel);
            this.Controls.Add(this.buttonWSubmit);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "dd";
            this.Load += new System.EventHandler(this.FrmDialog_Load);
            this.Shown += new System.EventHandler(this.FrmDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.vPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private Skyray.Controls.ButtonW buttonWSubmit;
        private Skyray.Controls.ButtonW buttonWCancel;
        private System.Windows.Forms.Label lblAlertInfo;
        private System.Windows.Forms.PictureBox vPicBox;
	}
}