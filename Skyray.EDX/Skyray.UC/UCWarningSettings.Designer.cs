namespace Skyray.UC
{
    partial class UCWarningSettings
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
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.lblWarning = new Skyray.Controls.LabelW();
            this.grpWarningSettings = new Skyray.Controls.Grouper();
            this.labelW5 = new Skyray.Controls.LabelW();
            this.numtime = new System.Windows.Forms.NumericUpDown();
            this.radNo = new System.Windows.Forms.RadioButton();
            this.radYes = new System.Windows.Forms.RadioButton();
            this.scrMax = new System.Windows.Forms.HScrollBar();
            this.lblValue = new Skyray.Controls.LabelW();
            this.lblTime = new Skyray.Controls.LabelW();
            this.lblLimit = new Skyray.Controls.LabelW();
            this.grpWarningSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numtime)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(106, 163);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(258, 163);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.BackColor = System.Drawing.Color.Transparent;
            this.lblWarning.Location = new System.Drawing.Point(36, 39);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(53, 12);
            this.lblWarning.TabIndex = 2;
            this.lblWarning.Text = "是否报警";
            // 
            // grpWarningSettings
            // 
            this.grpWarningSettings.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpWarningSettings.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpWarningSettings.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpWarningSettings.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpWarningSettings.BorderThickness = 1F;
            this.grpWarningSettings.BorderTopOnly = false;
            this.grpWarningSettings.Controls.Add(this.labelW5);
            this.grpWarningSettings.Controls.Add(this.numtime);
            this.grpWarningSettings.Controls.Add(this.radNo);
            this.grpWarningSettings.Controls.Add(this.radYes);
            this.grpWarningSettings.Controls.Add(this.scrMax);
            this.grpWarningSettings.Controls.Add(this.lblValue);
            this.grpWarningSettings.Controls.Add(this.lblTime);
            this.grpWarningSettings.Controls.Add(this.lblLimit);
            this.grpWarningSettings.Controls.Add(this.lblWarning);
            this.grpWarningSettings.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpWarningSettings.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpWarningSettings.GroupImage = null;
            this.grpWarningSettings.GroupTitle = "报警设置";
            this.grpWarningSettings.HeaderRoundCorners = 4;
            this.grpWarningSettings.Location = new System.Drawing.Point(11, 12);
            this.grpWarningSettings.Name = "grpWarningSettings";
            this.grpWarningSettings.PaintGroupBox = false;
            this.grpWarningSettings.RoundCorners = 4;
            this.grpWarningSettings.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpWarningSettings.ShadowControl = false;
            this.grpWarningSettings.ShadowThickness = 3;
            this.grpWarningSettings.Size = new System.Drawing.Size(417, 139);
            this.grpWarningSettings.TabIndex = 3;
            this.grpWarningSettings.TextLineSpace = 2;
            this.grpWarningSettings.TitleLeftSpace = 18;
            // 
            // labelW5
            // 
            this.labelW5.AutoSize = true;
            this.labelW5.BackColor = System.Drawing.Color.Transparent;
            this.labelW5.Location = new System.Drawing.Point(318, 102);
            this.labelW5.Name = "labelW5";
            this.labelW5.Size = new System.Drawing.Size(11, 12);
            this.labelW5.TabIndex = 11;
            this.labelW5.Text = "S";
            // 
            // numtime
            // 
            this.numtime.Location = new System.Drawing.Point(127, 97);
            this.numtime.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numtime.Name = "numtime";
            this.numtime.Size = new System.Drawing.Size(177, 21);
            this.numtime.TabIndex = 10;
            // 
            // radNo
            // 
            this.radNo.AutoSize = true;
            this.radNo.Location = new System.Drawing.Point(239, 35);
            this.radNo.Name = "radNo";
            this.radNo.Size = new System.Drawing.Size(35, 16);
            this.radNo.TabIndex = 9;
            this.radNo.TabStop = true;
            this.radNo.Text = "否";
            this.radNo.UseVisualStyleBackColor = true;
            // 
            // radYes
            // 
            this.radYes.AutoSize = true;
            this.radYes.Location = new System.Drawing.Point(128, 35);
            this.radYes.Name = "radYes";
            this.radYes.Size = new System.Drawing.Size(35, 16);
            this.radYes.TabIndex = 8;
            this.radYes.TabStop = true;
            this.radYes.Text = "是";
            this.radYes.UseVisualStyleBackColor = true;
            // 
            // scrMax
            // 
            this.scrMax.LargeChange = 1;
            this.scrMax.Location = new System.Drawing.Point(127, 68);
            this.scrMax.Name = "scrMax";
            this.scrMax.Size = new System.Drawing.Size(177, 16);
            this.scrMax.TabIndex = 7;
            this.scrMax.ValueChanged += new System.EventHandler(this.scrMax_ValueChanged);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.BackColor = System.Drawing.Color.Transparent;
            this.lblValue.Location = new System.Drawing.Point(318, 70);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(11, 12);
            this.lblValue.TabIndex = 6;
            this.lblValue.Text = "%";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Location = new System.Drawing.Point(36, 102);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(53, 12);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "报警时限";
            // 
            // lblLimit
            // 
            this.lblLimit.AutoSize = true;
            this.lblLimit.BackColor = System.Drawing.Color.Transparent;
            this.lblLimit.Location = new System.Drawing.Point(36, 70);
            this.lblLimit.Name = "lblLimit";
            this.lblLimit.Size = new System.Drawing.Size(53, 12);
            this.lblLimit.TabIndex = 4;
            this.lblLimit.Text = "报警门限";
            // 
            // UCWarningSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.grpWarningSettings);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "UCWarningSettings";
            this.Size = new System.Drawing.Size(438, 200);
            this.Load += new System.EventHandler(this.UCWarningSettings_Load);
            this.grpWarningSettings.ResumeLayout(false);
            this.grpWarningSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numtime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.LabelW lblWarning;
        private Skyray.Controls.Grouper grpWarningSettings;
        private System.Windows.Forms.HScrollBar scrMax;
        private Skyray.Controls.LabelW lblValue;
        private Skyray.Controls.LabelW lblTime;
        private Skyray.Controls.LabelW lblLimit;
        private System.Windows.Forms.NumericUpDown numtime;
        private System.Windows.Forms.RadioButton radNo;
        private System.Windows.Forms.RadioButton radYes;
        private Skyray.Controls.LabelW labelW5;
    }
}
