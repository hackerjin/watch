namespace Skyray.UC
{
    partial class UCOtherInfoSet
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
            this.buttonWOK = new Skyray.Controls.ButtonW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lab_InfoSele = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonWOK
            // 
            this.buttonWOK.bSilver = false;
            this.buttonWOK.Location = new System.Drawing.Point(88, 327);
            this.buttonWOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWOK.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWOK.Name = "buttonWOK";
            this.buttonWOK.Size = new System.Drawing.Size(75, 23);
            this.buttonWOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWOK.TabIndex = 2;
            this.buttonWOK.Text = "确定";
            this.buttonWOK.ToFocused = false;
            this.buttonWOK.UseVisualStyleBackColor = true;
            this.buttonWOK.Click += new System.EventHandler(this.buttonWOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(187, 327);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(68, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 42;
            this.btnClose.Text = "取消";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lab_InfoSele);
            this.panel1.Location = new System.Drawing.Point(11, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 310);
            this.panel1.TabIndex = 43;
            // 
            // lab_InfoSele
            // 
            this.lab_InfoSele.AutoSize = true;
            this.lab_InfoSele.Location = new System.Drawing.Point(10, 12);
            this.lab_InfoSele.Name = "lab_InfoSele";
            this.lab_InfoSele.Size = new System.Drawing.Size(53, 12);
            this.lab_InfoSele.TabIndex = 0;
            this.lab_InfoSele.Text = "信息填写";
            // 
            // UCOtherInfoSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.buttonWOK);
            this.Name = "UCOtherInfoSet";
            this.Size = new System.Drawing.Size(298, 361);
            this.Load += new System.EventHandler(this.UCOtherInfoSet_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW buttonWOK;
        private Skyray.Controls.ButtonW btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lab_InfoSele;
    }
}
