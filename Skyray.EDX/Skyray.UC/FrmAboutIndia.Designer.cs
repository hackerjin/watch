namespace Skyray.UC
{
    partial class FrmAboutIndia
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnOk = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Skyray.UC.Properties.Resources.pingzi;
            this.panel1.Location = new System.Drawing.Point(17, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(249, 177);
            this.panel1.TabIndex = 0;
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAppName.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblAppName.Location = new System.Drawing.Point(12, 191);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(220, 25);
            this.lblAppName.TabIndex = 1;
            this.lblAppName.Text = "EDXRF X-Ray Analyzer";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVersion.ForeColor = System.Drawing.Color.Red;
            this.lblVersion.Location = new System.Drawing.Point(14, 220);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(150, 17);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "EDXRF 2.0.71.140208";
            // 
            // btnOk
            // 
            this.btnOk.bSilver = false;
            this.btnOk.Location = new System.Drawing.Point(99, 249);
            this.btnOk.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOk.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 21);
            this.btnOk.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "确定";
            this.btnOk.ToFocused = false;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmAboutIndia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblAppName);
            this.Controls.Add(this.panel1);
            this.Name = "FrmAboutIndia";
            this.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Size = new System.Drawing.Size(283, 280);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblVersion;
        private Skyray.Controls.ButtonW btnOk;
    }
}
