namespace Skyray.Camera
{
    partial class FrmScreentCamer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmScreentCamer));
            this.videoSourcePlayer1 = new AForge.Controls.VideoSourcePlayer();
            this.skyrayCamera1 = new Skyray.Camera.SkyrayCamera();
            this.SuspendLayout();
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoSourcePlayer1.Location = new System.Drawing.Point(0, 0);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(598, 402);
            this.videoSourcePlayer1.TabIndex = 0;
            this.videoSourcePlayer1.Text = "videoSourcePlayer1";
            this.videoSourcePlayer1.VideoSource = null;
            // 
            // skyrayCamera1
            // 
            this.skyrayCamera1.AutoSaveSamplePic = false;
            this.skyrayCamera1.BackColor = System.Drawing.Color.LightCyan;
            this.skyrayCamera1.CameraFormat = null;
            this.skyrayCamera1.ContiTestPoints = ((System.Collections.ArrayList)(resources.GetObject("skyrayCamera1.ContiTestPoints")));
            this.skyrayCamera1.DeviceName = null;
            this.skyrayCamera1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skyrayCamera1.FociIndex = 0;
            this.skyrayCamera1.FociX = 0;
            this.skyrayCamera1.FociY = 0;
            this.skyrayCamera1.FormatHight = 0;
            this.skyrayCamera1.FormatWidth = 0;
            this.skyrayCamera1.IsAdminUse = null;
            this.skyrayCamera1.IsShowTestPoints = true;
            this.skyrayCamera1.IsWidthHeightChecked = false;
            this.skyrayCamera1.IsXRF = null;
            this.skyrayCamera1.Location = new System.Drawing.Point(0, 0);
            this.skyrayCamera1.Mode = Skyray.Camera.SkyrayCamera.CameraMode.Move;
            this.skyrayCamera1.Mode3000D = Skyray.Camera.SkyrayCamera.CameraMode.Move;
            this.skyrayCamera1.MouseModifyFoci = false;
            this.skyrayCamera1.MoveRateFocus = new System.Drawing.Point(0, 0);
            this.skyrayCamera1.MoveRateStepX = 0;
            this.skyrayCamera1.MoveRateStepY = 0;
            this.skyrayCamera1.MoveRateTargetExpect = new System.Drawing.Point(0, 0);
            this.skyrayCamera1.MoveRateTargetReal = new System.Drawing.Point(0, 0);
            this.skyrayCamera1.MoveRateX = 0;
            this.skyrayCamera1.MoveRateY = 0;
            this.skyrayCamera1.Name = "skyrayCamera1";
            this.skyrayCamera1.RuleUnit = 0;
            this.skyrayCamera1.Size = new System.Drawing.Size(598, 402);
            this.skyrayCamera1.TabIndex = 1;
            this.skyrayCamera1.Text = "skyrayCamera1";
            this.skyrayCamera1.VideoIndex = 0;
            this.skyrayCamera1.VideoSource = null;
            this.skyrayCamera1.ViewHeight = 5;
            this.skyrayCamera1.ViewWidth = 5;
            // 
            // FrmScreentCamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 402);
            this.Controls.Add(this.skyrayCamera1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmScreentCamer";
            this.Text = "FrmScreentCamer";
            this.Load += new System.EventHandler(this.FrmScreentCamer_Load);
            this.ResumeLayout(false);

        }
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer1;
        #endregion
        private SkyrayCamera skyrayCamera1;
    }
}