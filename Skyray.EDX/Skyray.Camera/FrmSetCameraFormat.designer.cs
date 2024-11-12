namespace Skyray.Camera
{
    partial class FrmSetCameraFormat
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
            this.cboCameraFormats = new System.Windows.Forms.ComboBox();
            this.lblCameraSize = new System.Windows.Forms.Label();
            this.lblVideoDev = new System.Windows.Forms.Label();
            this.cboVideoDev = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboVideoCapabilities = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboCameraFormats
            // 
            this.cboCameraFormats.FormattingEnabled = true;
            this.cboCameraFormats.Location = new System.Drawing.Point(120, 54);
            this.cboCameraFormats.Name = "cboCameraFormats";
            this.cboCameraFormats.Size = new System.Drawing.Size(184, 20);
            this.cboCameraFormats.TabIndex = 0;
            this.cboCameraFormats.Visible = false;
            this.cboCameraFormats.SelectedIndexChanged += new System.EventHandler(this.cboCameraFormats_SelectedIndexChanged);
            // 
            // lblCameraSize
            // 
            this.lblCameraSize.AutoSize = true;
            this.lblCameraSize.Location = new System.Drawing.Point(41, 55);
            this.lblCameraSize.Name = "lblCameraSize";
            this.lblCameraSize.Size = new System.Drawing.Size(53, 12);
            this.lblCameraSize.TabIndex = 2;
            this.lblCameraSize.Text = "视频大小";
            this.lblCameraSize.Visible = false;
            // 
            // lblVideoDev
            // 
            this.lblVideoDev.AutoSize = true;
            this.lblVideoDev.Location = new System.Drawing.Point(41, 20);
            this.lblVideoDev.Name = "lblVideoDev";
            this.lblVideoDev.Size = new System.Drawing.Size(53, 12);
            this.lblVideoDev.TabIndex = 3;
            this.lblVideoDev.Text = "视频设备";
            // 
            // cboVideoDev
            // 
            this.cboVideoDev.FormattingEnabled = true;
            this.cboVideoDev.Location = new System.Drawing.Point(120, 19);
            this.cboVideoDev.Name = "cboVideoDev";
            this.cboVideoDev.Size = new System.Drawing.Size(184, 20);
            this.cboVideoDev.TabIndex = 4;
            this.cboVideoDev.SelectedValueChanged += new System.EventHandler(this.cboVideoDev_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "像素";
            // 
            // cboVideoCapabilities
            // 
            this.cboVideoCapabilities.FormattingEnabled = true;
            this.cboVideoCapabilities.Location = new System.Drawing.Point(120, 91);
            this.cboVideoCapabilities.Name = "cboVideoCapabilities";
            this.cboVideoCapabilities.Size = new System.Drawing.Size(184, 20);
            this.cboVideoCapabilities.TabIndex = 5;
            this.cboVideoCapabilities.SelectedValueChanged += new System.EventHandler(this.cboVideoCapabilities_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(266, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 7;
            // 
            // FrmSetCameraFormat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(362, 183);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboVideoCapabilities);
            this.Controls.Add(this.cboVideoDev);
            this.Controls.Add(this.lblVideoDev);
            this.Controls.Add(this.lblCameraSize);
            this.Controls.Add(this.cboCameraFormats);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSetCameraFormat";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "视频格式";
            this.Load += new System.EventHandler(this.FrmSetCameraFormat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox cboCameraFormats;
        private System.Windows.Forms.Label lblCameraSize;
        private System.Windows.Forms.Label lblVideoDev;
        private System.Windows.Forms.ComboBox cboVideoDev;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cboVideoCapabilities;
        private System.Windows.Forms.Label label2;
    }
}