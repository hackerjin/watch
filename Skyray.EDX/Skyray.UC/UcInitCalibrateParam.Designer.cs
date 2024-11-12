namespace Skyray.UC
{
    partial class UcInitCalibrateParam
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
            this.btWCancel = new Skyray.Controls.ButtonW();
            this.btWSubmit = new Skyray.Controls.ButtonW();
            this.lblInitCount = new Skyray.Controls.LabelW();
            this.lblRadio = new Skyray.Controls.LabelW();
            this.numFirstCount = new Skyray.Controls.NumericTextBox();
            this.numRadio = new Skyray.Controls.TextBoxW();
            this.SuspendLayout();
            // 
            // btWCancel
            // 
            this.btWCancel.bSilver = false;
            this.btWCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btWCancel.Location = new System.Drawing.Point(160, 124);
            this.btWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btWCancel.Name = "btWCancel";
            this.btWCancel.Size = new System.Drawing.Size(75, 25);
            this.btWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWCancel.TabIndex = 9;
            this.btWCancel.Text = "取消";
            this.btWCancel.ToFocused = false;
            this.btWCancel.UseVisualStyleBackColor = true;
            this.btWCancel.Click += new System.EventHandler(this.btWCancel_Click);
            // 
            // btWSubmit
            // 
            this.btWSubmit.bSilver = false;
            this.btWSubmit.Location = new System.Drawing.Point(33, 124);
            this.btWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btWSubmit.Name = "btWSubmit";
            this.btWSubmit.Size = new System.Drawing.Size(75, 25);
            this.btWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWSubmit.TabIndex = 8;
            this.btWSubmit.Text = "确定";
            this.btWSubmit.ToFocused = false;
            this.btWSubmit.UseVisualStyleBackColor = true;
            this.btWSubmit.Click += new System.EventHandler(this.btWSubmit_Click);
            // 
            // lblInitCount
            // 
            this.lblInitCount.AutoSize = true;
            this.lblInitCount.BackColor = System.Drawing.Color.Transparent;
            this.lblInitCount.Location = new System.Drawing.Point(30, 30);
            this.lblInitCount.Name = "lblInitCount";
            this.lblInitCount.Size = new System.Drawing.Size(60, 13);
            this.lblInitCount.TabIndex = 10;
            this.lblInitCount.Text = "First Count:";
            // 
            // lblRadio
            // 
            this.lblRadio.AutoSize = true;
            this.lblRadio.BackColor = System.Drawing.Color.Transparent;
            this.lblRadio.Location = new System.Drawing.Point(32, 86);
            this.lblRadio.Name = "lblRadio";
            this.lblRadio.Size = new System.Drawing.Size(38, 13);
            this.lblRadio.TabIndex = 12;
            this.lblRadio.Text = "Radio:";
            // 
            // numFirstCount
            // 
            this.numFirstCount.Location = new System.Drawing.Point(132, 27);
            this.numFirstCount.Name = "numFirstCount";
            this.numFirstCount.Size = new System.Drawing.Size(100, 20);
            this.numFirstCount.TabIndex = 14;
            // 
            // numRadio
            // 
            this.numRadio.BorderColor = System.Drawing.Color.Empty;
            this.numRadio.Enabled = false;
            this.numRadio.Location = new System.Drawing.Point(132, 78);
            this.numRadio.Name = "numRadio";
            this.numRadio.Size = new System.Drawing.Size(100, 20);
            this.numRadio.Style = Skyray.Controls.Style.Office2007Blue;
            this.numRadio.TabIndex = 15;
            // 
            // UcInitCalibrateParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.numRadio);
            this.Controls.Add(this.numFirstCount);
            this.Controls.Add(this.lblRadio);
            this.Controls.Add(this.lblInitCount);
            this.Controls.Add(this.btWCancel);
            this.Controls.Add(this.btWSubmit);
            this.Name = "UcInitCalibrateParam";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(312, 191);
            this.Load += new System.EventHandler(this.UcInOutSample_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btWCancel;
        private Skyray.Controls.ButtonW btWSubmit;
        private Skyray.Controls.LabelW lblInitCount;
        private Skyray.Controls.LabelW lblRadio;
        private Skyray.Controls.NumericTextBox numFirstCount;
        private Skyray.Controls.TextBoxW numRadio;

    }
}
