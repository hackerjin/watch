namespace Skyray.UC
{
    partial class FrmAdminPass
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
            this.textAdminPass = new Skyray.Controls.TextBoxW();
            this.lbAdminPass = new Skyray.Controls.LabelW();
            this.btAdminPass = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // textAdminPass
            // 
            this.textAdminPass.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textAdminPass.Location = new System.Drawing.Point(172, 70);
            this.textAdminPass.Name = "textAdminPass";
            this.textAdminPass.Size = new System.Drawing.Size(108, 21);
            this.textAdminPass.Style = Skyray.Controls.Style.Office2007Blue;
            this.textAdminPass.TabIndex = 0;
            // 
            // lbAdminPass
            // 
            this.lbAdminPass.AutoSize = true;
            this.lbAdminPass.BackColor = System.Drawing.Color.Transparent;
            this.lbAdminPass.Location = new System.Drawing.Point(39, 73);
            this.lbAdminPass.Name = "lbAdminPass";
            this.lbAdminPass.Size = new System.Drawing.Size(113, 12);
            this.lbAdminPass.TabIndex = 1;
            this.lbAdminPass.Text = "请输入管理员密码：";
            // 
            // btAdminPass
            // 
            this.btAdminPass.bSilver = false;
            this.btAdminPass.Location = new System.Drawing.Point(150, 120);
            this.btAdminPass.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btAdminPass.MenuPos = new System.Drawing.Point(0, 0);
            this.btAdminPass.Name = "btAdminPass";
            this.btAdminPass.Size = new System.Drawing.Size(75, 23);
            this.btAdminPass.Style = Skyray.Controls.Style.Office2007Blue;
            this.btAdminPass.TabIndex = 2;
            this.btAdminPass.Text = "确定";
            this.btAdminPass.ToFocused = false;
            this.btAdminPass.UseVisualStyleBackColor = true;
            this.btAdminPass.Click += new System.EventHandler(this.btAdminPass_Click);
            // 
            // FrmAdminPass
            // 
            this.ClientSize = new System.Drawing.Size(395, 161);
            this.Controls.Add(this.btAdminPass);
            this.Controls.Add(this.lbAdminPass);
            this.Controls.Add(this.textAdminPass);
            this.Name = "FrmAdminPass";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.TextBoxW textAdminPass;
        private Skyray.Controls.LabelW lbAdminPass;
        private Skyray.Controls.ButtonW btAdminPass;
    }
}
