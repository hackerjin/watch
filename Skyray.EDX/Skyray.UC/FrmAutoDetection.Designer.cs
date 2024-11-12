namespace Skyray.UC
{
    partial class FrmAutoDetection
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDeviceDetection = new System.Windows.Forms.Label();
            this.lblDetectionProgress = new System.Windows.Forms.Label();
            this.lblDetectionResult = new System.Windows.Forms.Label();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.proBarDetection = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblDeviceDetection
            // 
            this.lblDeviceDetection.AutoSize = true;
            this.lblDeviceDetection.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDeviceDetection.Location = new System.Drawing.Point(201, 24);
            this.lblDeviceDetection.Name = "lblDeviceDetection";
            this.lblDeviceDetection.Size = new System.Drawing.Size(94, 21);
            this.lblDeviceDetection.TabIndex = 0;
            this.lblDeviceDetection.Text = "仪器检测";
            // 
            // lblDetectionProgress
            // 
            this.lblDetectionProgress.AutoSize = true;
            this.lblDetectionProgress.Location = new System.Drawing.Point(23, 79);
            this.lblDetectionProgress.Name = "lblDetectionProgress";
            this.lblDetectionProgress.Size = new System.Drawing.Size(53, 12);
            this.lblDetectionProgress.TabIndex = 3;
            this.lblDetectionProgress.Text = "检测进度";
            // 
            // lblDetectionResult
            // 
            this.lblDetectionResult.AutoSize = true;
            this.lblDetectionResult.Location = new System.Drawing.Point(422, 79);
            this.lblDetectionResult.Name = "lblDetectionResult";
            this.lblDetectionResult.Size = new System.Drawing.Size(0, 12);
            this.lblDetectionResult.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(304, 103);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 25);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = " 取 消 ";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(149, 103);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 25);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = " 确 定 ";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // proBarDetection
            // 
            this.proBarDetection.Location = new System.Drawing.Point(94, 71);
            this.proBarDetection.Name = "proBarDetection";
            this.proBarDetection.Size = new System.Drawing.Size(304, 23);
            this.proBarDetection.TabIndex = 20;
            // 
            // FrmAutoDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.ClientSize = new System.Drawing.Size(547, 136);
            this.Controls.Add(this.proBarDetection);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblDetectionResult);
            this.Controls.Add(this.lblDetectionProgress);
            this.Controls.Add(this.lblDeviceDetection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmAutoDetection";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmAutoDetection_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDeviceDetection;
        private System.Windows.Forms.Label lblDetectionProgress;
        private System.Windows.Forms.Label lblDetectionResult;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnOK;
        private System.Windows.Forms.ProgressBar proBarDetection;
    }
}
