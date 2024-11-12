namespace Skyray.UC
{
    partial class UcInOutSample
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
            this.lblYInSample = new Skyray.Controls.LabelW();
            this.nUpInSample = new Skyray.Controls.NumricUpDownW();
            ((System.ComponentModel.ISupportInitialize)(this.nUpInSample)).BeginInit();
            this.SuspendLayout();
            // 
            // btWCancel
            // 
            this.btWCancel.bSilver = false;
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
            // lblYInSample
            // 
            this.lblYInSample.AutoSize = true;
            this.lblYInSample.BackColor = System.Drawing.Color.Transparent;
            this.lblYInSample.Location = new System.Drawing.Point(30, 57);
            this.lblYInSample.Name = "lblYInSample";
            this.lblYInSample.Size = new System.Drawing.Size(77, 13);
            this.lblYInSample.TabIndex = 10;
            this.lblYInSample.Text = "in sample step:";
            // 
            // nUpInSample
            // 
            this.nUpInSample.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.nUpInSample.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.nUpInSample.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.nUpInSample.Location = new System.Drawing.Point(132, 57);
            this.nUpInSample.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.nUpInSample.Name = "nUpInSample";
            this.nUpInSample.Size = new System.Drawing.Size(103, 20);
            this.nUpInSample.TabIndex = 11;
            this.nUpInSample.Value = new decimal(new int[] {
            48000,
            0,
            0,
            0});
            // 
            // UcInOutSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.nUpInSample);
            this.Controls.Add(this.lblYInSample);
            this.Controls.Add(this.btWCancel);
            this.Controls.Add(this.btWSubmit);
            this.Name = "UcInOutSample";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(312, 191);
            this.Load += new System.EventHandler(this.UcInOutSample_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nUpInSample)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btWCancel;
        private Skyray.Controls.ButtonW btWSubmit;
        private Skyray.Controls.LabelW lblYInSample;
        private Skyray.Controls.NumricUpDownW nUpInSample;

    }
}
