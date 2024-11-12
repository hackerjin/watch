namespace Skyray.UC
{
    partial class UCXYZMotor
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
            this.grBText = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblXYSpeedw1 = new System.Windows.Forms.Label();
            this.vScrollXYSpeed = new System.Windows.Forms.VScrollBar();
            this.btnAxisRight = new System.Windows.Forms.Button();
            this.btnAxisLeft = new System.Windows.Forms.Button();
            this.grBText.SuspendLayout();
            this.SuspendLayout();
            // 
            // grBText
            // 
            this.grBText.Controls.Add(this.btnStop);
            this.grBText.Controls.Add(this.lblXYSpeedw1);
            this.grBText.Controls.Add(this.vScrollXYSpeed);
            this.grBText.Controls.Add(this.btnAxisRight);
            this.grBText.Controls.Add(this.btnAxisLeft);
            this.grBText.Location = new System.Drawing.Point(3, 3);
            this.grBText.Name = "grBText";
            this.grBText.Size = new System.Drawing.Size(200, 87);
            this.grBText.TabIndex = 0;
            this.grBText.TabStop = false;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(128, 17);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(40, 54);
            this.btnStop.TabIndex = 22;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblXYSpeedw1
            // 
            this.lblXYSpeedw1.Location = new System.Drawing.Point(168, 38);
            this.lblXYSpeedw1.Name = "lblXYSpeedw1";
            this.lblXYSpeedw1.Size = new System.Drawing.Size(28, 11);
            this.lblXYSpeedw1.TabIndex = 21;
            this.lblXYSpeedw1.Text = "20";
            // 
            // vScrollXYSpeed
            // 
            this.vScrollXYSpeed.Location = new System.Drawing.Point(173, 17);
            this.vScrollXYSpeed.Maximum = 159;
            this.vScrollXYSpeed.Minimum = 20;
            this.vScrollXYSpeed.Name = "vScrollXYSpeed";
            this.vScrollXYSpeed.Size = new System.Drawing.Size(19, 63);
            this.vScrollXYSpeed.TabIndex = 18;
            this.vScrollXYSpeed.Value = 150;
            this.vScrollXYSpeed.ValueChanged += new System.EventHandler(this.vScrollXYSpeed_ValueChanged);
            // 
            // btnAxisRight
            // 
            this.btnAxisRight.Location = new System.Drawing.Point(71, 17);
            this.btnAxisRight.Margin = new System.Windows.Forms.Padding(0);
            this.btnAxisRight.Name = "btnAxisRight";
            this.btnAxisRight.Size = new System.Drawing.Size(40, 54);
            this.btnAxisRight.TabIndex = 12;
            this.btnAxisRight.UseVisualStyleBackColor = true;
            this.btnAxisRight.Click += new System.EventHandler(this.btnAxisRight_Click);
            this.btnAxisRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAxisRight_MouseDown);
            this.btnAxisRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAxisRight_MouseUp);
            // 
            // btnAxisLeft
            // 
            this.btnAxisLeft.Location = new System.Drawing.Point(14, 17);
            this.btnAxisLeft.Margin = new System.Windows.Forms.Padding(0);
            this.btnAxisLeft.Name = "btnAxisLeft";
            this.btnAxisLeft.Size = new System.Drawing.Size(40, 54);
            this.btnAxisLeft.TabIndex = 11;
            this.btnAxisLeft.UseVisualStyleBackColor = true;
            this.btnAxisLeft.Click += new System.EventHandler(this.btnAxisLeft_Click);
            // 
            // UCXYZMotor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grBText);
            this.Name = "UCXYZMotor";
            this.Size = new System.Drawing.Size(207, 93);
            this.Load += new System.EventHandler(this.UCXYZMotor_Load);
            this.VisibleChanged += new System.EventHandler(this.UCXYZMotor_VisibleChanged);
            this.grBText.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grBText;
        private System.Windows.Forms.Button btnAxisLeft;
        private System.Windows.Forms.Button btnAxisRight;
        private System.Windows.Forms.VScrollBar vScrollXYSpeed;
        private System.Windows.Forms.Label lblXYSpeedw1;
        private System.Windows.Forms.Button btnStop;
    }
}
