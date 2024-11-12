namespace Skyray.UC
{
    partial class UCGainSetFor3000
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
            this.lblGain = new Skyray.Controls.LabelW();
            this.diGain = new Skyray.Controls.DoubleInputW();
            this.diFineGain = new Skyray.Controls.DoubleInputW();
            this.lblFineGain = new Skyray.Controls.LabelW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.diCurrent = new Skyray.Controls.DoubleInputW();
            this.lblCurrent = new Skyray.Controls.LabelW();
            this.diVoltage = new Skyray.Controls.DoubleInputW();
            this.lblVoltage = new Skyray.Controls.LabelW();
            this.SuspendLayout();
            // 
            // lblGain
            // 
            this.lblGain.AutoSize = true;
            this.lblGain.BackColor = System.Drawing.Color.Transparent;
            this.lblGain.Location = new System.Drawing.Point(215, 35);
            this.lblGain.Name = "lblGain";
            this.lblGain.Size = new System.Drawing.Size(41, 12);
            this.lblGain.TabIndex = 108;
            this.lblGain.Text = "粗调码";
            // 
            // diGain
            // 
            this.diGain.BorderColor = System.Drawing.Color.Empty;
            this.diGain.DecimalDigits = 0;
            this.diGain.Location = new System.Drawing.Point(290, 32);
            this.diGain.Name = "diGain";
            this.diGain.Size = new System.Drawing.Size(106, 21);
            this.diGain.TabIndex = 109;
            // 
            // diFineGain
            // 
            this.diFineGain.BorderColor = System.Drawing.Color.Empty;
            this.diFineGain.DecimalDigits = 0;
            this.diFineGain.Location = new System.Drawing.Point(290, 82);
            this.diFineGain.Name = "diFineGain";
            this.diFineGain.Size = new System.Drawing.Size(106, 21);
            this.diFineGain.TabIndex = 111;
            // 
            // lblFineGain
            // 
            this.lblFineGain.AutoSize = true;
            this.lblFineGain.BackColor = System.Drawing.Color.Transparent;
            this.lblFineGain.Location = new System.Drawing.Point(215, 85);
            this.lblFineGain.Name = "lblFineGain";
            this.lblFineGain.Size = new System.Drawing.Size(41, 12);
            this.lblFineGain.TabIndex = 110;
            this.lblFineGain.Text = "细调码";
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(107, 130);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 112;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(226, 130);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 113;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // diCurrent
            // 
            this.diCurrent.BorderColor = System.Drawing.Color.Empty;
            this.diCurrent.DecimalDigits = 0;
            this.diCurrent.Location = new System.Drawing.Point(85, 82);
            this.diCurrent.Name = "diCurrent";
            this.diCurrent.Size = new System.Drawing.Size(106, 21);
            this.diCurrent.TabIndex = 117;
            this.diCurrent.Validating += new System.ComponentModel.CancelEventHandler(this.diCurrent_Validating);
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrent.Location = new System.Drawing.Point(20, 85);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(41, 12);
            this.lblCurrent.TabIndex = 116;
            this.lblCurrent.Text = "管  流";
            // 
            // diVoltage
            // 
            this.diVoltage.BorderColor = System.Drawing.Color.Empty;
            this.diVoltage.DecimalDigits = 0;
            this.diVoltage.Location = new System.Drawing.Point(85, 32);
            this.diVoltage.Name = "diVoltage";
            this.diVoltage.Size = new System.Drawing.Size(106, 21);
            this.diVoltage.TabIndex = 115;
            this.diVoltage.Validating += new System.ComponentModel.CancelEventHandler(this.diVoltage_Validating);
            // 
            // lblVoltage
            // 
            this.lblVoltage.AutoSize = true;
            this.lblVoltage.BackColor = System.Drawing.Color.Transparent;
            this.lblVoltage.Location = new System.Drawing.Point(20, 35);
            this.lblVoltage.Name = "lblVoltage";
            this.lblVoltage.Size = new System.Drawing.Size(41, 12);
            this.lblVoltage.TabIndex = 114;
            this.lblVoltage.Text = "管  压";
            // 
            // UCGainSetFor3000
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.diCurrent);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.diVoltage);
            this.Controls.Add(this.lblVoltage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.diFineGain);
            this.Controls.Add(this.lblFineGain);
            this.Controls.Add(this.diGain);
            this.Controls.Add(this.lblGain);
            this.Name = "UCGainSetFor3000";
            this.Size = new System.Drawing.Size(426, 175);
            this.Load += new System.EventHandler(this.UCGainSetFor3000_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblGain;
        private Skyray.Controls.DoubleInputW diGain;
        private Skyray.Controls.DoubleInputW diFineGain;
        private Skyray.Controls.LabelW lblFineGain;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.DoubleInputW diCurrent;
        private Skyray.Controls.LabelW lblCurrent;
        private Skyray.Controls.DoubleInputW diVoltage;
        private Skyray.Controls.LabelW lblVoltage;
    }
}
