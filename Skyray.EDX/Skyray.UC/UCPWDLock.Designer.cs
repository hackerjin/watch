namespace Skyray.UC
{
    partial class UCPWDLock
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
            this.lblPWDLock = new Skyray.Controls.LabelW();
            this.txtPWDLock = new Skyray.Controls.TextBoxW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // lblPWDLock
            // 
            this.lblPWDLock.AutoSize = true;
            this.lblPWDLock.BackColor = System.Drawing.Color.Transparent;
            this.lblPWDLock.Location = new System.Drawing.Point(49, 55);
            this.lblPWDLock.Name = "lblPWDLock";
            this.lblPWDLock.Size = new System.Drawing.Size(53, 12);
            this.lblPWDLock.TabIndex = 0;
            this.lblPWDLock.Text = "密  码：";
            // 
            // txtPWDLock
            // 
            this.txtPWDLock.BorderColor = System.Drawing.Color.Empty;
            this.txtPWDLock.Location = new System.Drawing.Point(134, 50);
            this.txtPWDLock.Name = "txtPWDLock";
            this.txtPWDLock.PasswordChar = '*';
            this.txtPWDLock.Size = new System.Drawing.Size(157, 21);
            this.txtPWDLock.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtPWDLock.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(70, 105);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(203, 105);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // UCPWDLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtPWDLock);
            this.Controls.Add(this.lblPWDLock);
            this.Name = "UCPWDLock";
            this.Size = new System.Drawing.Size(349, 158);
            this.Load += new System.EventHandler(this.UCPWDLock_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblPWDLock;
        private Skyray.Controls.TextBoxW txtPWDLock;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
    }
}
