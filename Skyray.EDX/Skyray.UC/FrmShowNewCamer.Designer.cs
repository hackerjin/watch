namespace Skyray.UC
{
    partial class FrmShowNewCamer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmShowNewCamer));
            this.skyrayCamera1 = new Skyray.Camera.SkyrayCamera();
            this.SuspendLayout();
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
            this.skyrayCamera1.FociX = 40;
            this.skyrayCamera1.FociY = 30;
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
            this.skyrayCamera1.MoveRateX = 24;
            this.skyrayCamera1.MoveRateY = 24;
            this.skyrayCamera1.Name = "skyrayCamera1";
            this.skyrayCamera1.RuleUnit = 5;
            this.skyrayCamera1.Size = new System.Drawing.Size(710, 436);
            this.skyrayCamera1.TabIndex = 0;
            this.skyrayCamera1.Text = "skyrayCamera1";
            this.skyrayCamera1.VideoIndex = 0;
            this.skyrayCamera1.VideoSource = null;
            this.skyrayCamera1.ViewHeight = 5;
            this.skyrayCamera1.ViewWidth = 5;
            // 
            // FrmShowNewCamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 436);
            this.Controls.Add(this.skyrayCamera1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmShowNewCamer";
            this.Text = "FrmShowNewCamer";
            this.Load += new System.EventHandler(this.FrmShowNewCamer_Load);
            this.Leave += new System.EventHandler(this.FrmShowNewCamer_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Camera.SkyrayCamera skyrayCamera1;


    }
}