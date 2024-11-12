using Skyray.Controls;
namespace Skyray.UC
{
    partial class FrmLogon
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLogon = new Skyray.Controls.ButtonW();
            this.btnExit = new Skyray.Controls.ButtonW();
            this.lblUser = new Skyray.Controls.LabelW();
            this.lblPwd = new Skyray.Controls.LabelW();
            this.txtPwd = new Skyray.Controls.TextBoxW();
            this.cboUser = new Skyray.Controls.ComboBoxW();
            this.btnActive = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // btnLogon
            // 
            this.btnLogon.BackColor = System.Drawing.Color.Transparent;
            this.btnLogon.bSilver = false;
            this.btnLogon.Enabled = false;
            this.btnLogon.Location = new System.Drawing.Point(316, 26);
            this.btnLogon.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnLogon.MenuPos = new System.Drawing.Point(0, 0);
            this.btnLogon.Name = "btnLogon";
            this.btnLogon.Size = new System.Drawing.Size(80, 23);
            this.btnLogon.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnLogon.TabIndex = 1;
            this.btnLogon.Text = "登录";
            this.btnLogon.ToFocused = false;
            this.btnLogon.UseVisualStyleBackColor = false;
            this.btnLogon.Click += new System.EventHandler(this.btnLogon_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.bSilver = false;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(316, 88);
            this.btnExit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnExit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 23);
            this.btnExit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "退出";
            this.btnExit.ToFocused = false;
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Location = new System.Drawing.Point(59, 28);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(41, 12);
            this.lblUser.TabIndex = 5;
            this.lblUser.Text = "用户名";
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.BackColor = System.Drawing.Color.Transparent;
            this.lblPwd.Location = new System.Drawing.Point(59, 97);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(29, 12);
            this.lblPwd.TabIndex = 6;
            this.lblPwd.Text = "密码";
            // 
            // txtPwd
            // 
            this.txtPwd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtPwd.Location = new System.Drawing.Point(145, 90);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(121, 21);
            this.txtPwd.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtPwd.TabIndex = 0;
            this.txtPwd.UseSystemPasswordChar = true;
            // 
            // cboUser
            // 
            this.cboUser.AutoComplete = false;
            this.cboUser.AutoDropdown = false;
            this.cboUser.BackColorEven = System.Drawing.Color.White;
            this.cboUser.BackColorOdd = System.Drawing.Color.White;
            this.cboUser.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboUser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboUser.ColumnNames = "";
            this.cboUser.ColumnWidthDefault = 75;
            this.cboUser.ColumnWidths = "";
            this.cboUser.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboUser.FormattingEnabled = true;
            this.cboUser.LinkedColumnIndex = 0;
            this.cboUser.LinkedTextBox = null;
            this.cboUser.Location = new System.Drawing.Point(145, 28);
            this.cboUser.Name = "cboUser";
            this.cboUser.Size = new System.Drawing.Size(121, 22);
            this.cboUser.TabIndex = 3;
            // 
            // btnActive
            // 
            this.btnActive.BackColor = System.Drawing.Color.Transparent;
            this.btnActive.bSilver = false;
            this.btnActive.Location = new System.Drawing.Point(316, 56);
            this.btnActive.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnActive.MenuPos = new System.Drawing.Point(0, 0);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(80, 23);
            this.btnActive.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnActive.TabIndex = 7;
            this.btnActive.Text = "授权";
            this.btnActive.ToFocused = false;
            this.btnActive.UseVisualStyleBackColor = false;
            this.btnActive.Click += new System.EventHandler(this.btnActive_Click);
            // 
            // FrmLogon
            // 
            this.AcceptButton = this.btnLogon;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(494, 141);
            this.Controls.Add(this.btnActive);
            this.Controls.Add(this.cboUser);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogon);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统登录";
            this.Load += new System.EventHandler(this.FrmLogon_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ButtonW btnLogon;
        private ButtonW btnExit;
        private LabelW lblUser;
        private LabelW lblPwd;
        private TextBoxW txtPwd;
        private ComboBoxW cboUser;
        private ButtonW btnActive;

    }
}
