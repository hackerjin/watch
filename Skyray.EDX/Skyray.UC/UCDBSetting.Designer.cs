namespace Skyray.UC
{
    partial class UCDBSetting
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
            base.Dispose(disposing);

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbDBIp = new Skyray.Controls.LabelW();
            this.textIP = new Skyray.Controls.TextBoxW();
            this.lbDBSource = new Skyray.Controls.LabelW();
            this.lbDBUser = new Skyray.Controls.LabelW();
            this.lbDBPwd = new Skyray.Controls.LabelW();
            this.textDB = new Skyray.Controls.TextBoxW();
            this.textUser = new Skyray.Controls.TextBoxW();
            this.textPwd = new Skyray.Controls.TextBoxW();
            this.btDBOk = new Skyray.Controls.ButtonW();
            this.btDBDel = new Skyray.Controls.ButtonW();
            this.chkDB = new Skyray.Controls.CheckBoxW();
            this.SuspendLayout();
            // 
            // lbDBIp
            // 
            this.lbDBIp.AutoSize = true;
            this.lbDBIp.BackColor = System.Drawing.Color.Transparent;
            this.lbDBIp.Location = new System.Drawing.Point(86, 76);
            this.lbDBIp.Name = "lbDBIp";
            this.lbDBIp.Size = new System.Drawing.Size(65, 12);
            this.lbDBIp.TabIndex = 0;
            this.lbDBIp.Text = "网络地址：";
            // 
            // textIP
            // 
            this.textIP.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textIP.Location = new System.Drawing.Point(203, 76);
            this.textIP.Name = "textIP";
            this.textIP.Size = new System.Drawing.Size(100, 21);
            this.textIP.Style = Skyray.Controls.Style.Office2007Blue;
            this.textIP.TabIndex = 1;
            // 
            // lbDBSource
            // 
            this.lbDBSource.AutoSize = true;
            this.lbDBSource.BackColor = System.Drawing.Color.Transparent;
            this.lbDBSource.Location = new System.Drawing.Point(86, 127);
            this.lbDBSource.Name = "lbDBSource";
            this.lbDBSource.Size = new System.Drawing.Size(53, 12);
            this.lbDBSource.TabIndex = 2;
            this.lbDBSource.Text = "数据库：";
            // 
            // lbDBUser
            // 
            this.lbDBUser.AutoSize = true;
            this.lbDBUser.BackColor = System.Drawing.Color.Transparent;
            this.lbDBUser.Location = new System.Drawing.Point(86, 181);
            this.lbDBUser.Name = "lbDBUser";
            this.lbDBUser.Size = new System.Drawing.Size(53, 12);
            this.lbDBUser.TabIndex = 3;
            this.lbDBUser.Text = "用户名：";
            // 
            // lbDBPwd
            // 
            this.lbDBPwd.AutoSize = true;
            this.lbDBPwd.BackColor = System.Drawing.Color.Transparent;
            this.lbDBPwd.Location = new System.Drawing.Point(86, 238);
            this.lbDBPwd.Name = "lbDBPwd";
            this.lbDBPwd.Size = new System.Drawing.Size(41, 12);
            this.lbDBPwd.TabIndex = 4;
            this.lbDBPwd.Text = "密码：";
            // 
            // textDB
            // 
            this.textDB.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textDB.Location = new System.Drawing.Point(203, 127);
            this.textDB.Name = "textDB";
            this.textDB.Size = new System.Drawing.Size(100, 21);
            this.textDB.Style = Skyray.Controls.Style.Office2007Blue;
            this.textDB.TabIndex = 5;
            // 
            // textUser
            // 
            this.textUser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textUser.Location = new System.Drawing.Point(203, 181);
            this.textUser.Name = "textUser";
            this.textUser.Size = new System.Drawing.Size(100, 21);
            this.textUser.Style = Skyray.Controls.Style.Office2007Blue;
            this.textUser.TabIndex = 6;
            // 
            // textPwd
            // 
            this.textPwd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textPwd.Location = new System.Drawing.Point(203, 238);
            this.textPwd.Name = "textPwd";
            this.textPwd.Size = new System.Drawing.Size(100, 21);
            this.textPwd.Style = Skyray.Controls.Style.Office2007Blue;
            this.textPwd.TabIndex = 7;
            // 
            // btDBOk
            // 
            this.btDBOk.bSilver = false;
            this.btDBOk.Location = new System.Drawing.Point(88, 319);
            this.btDBOk.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btDBOk.MenuPos = new System.Drawing.Point(0, 0);
            this.btDBOk.Name = "btDBOk";
            this.btDBOk.Size = new System.Drawing.Size(75, 23);
            this.btDBOk.Style = Skyray.Controls.Style.Office2007Blue;
            this.btDBOk.TabIndex = 8;
            this.btDBOk.Text = "确定";
            this.btDBOk.ToFocused = false;
            this.btDBOk.UseVisualStyleBackColor = true;
            this.btDBOk.Click += new System.EventHandler(this.btDBOk_Click);
            // 
            // btDBDel
            // 
            this.btDBDel.bSilver = false;
            this.btDBDel.Location = new System.Drawing.Point(274, 319);
            this.btDBDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btDBDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btDBDel.Name = "btDBDel";
            this.btDBDel.Size = new System.Drawing.Size(75, 23);
            this.btDBDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btDBDel.TabIndex = 9;
            this.btDBDel.Text = "取消";
            this.btDBDel.ToFocused = false;
            this.btDBDel.UseVisualStyleBackColor = true;
            this.btDBDel.Click += new System.EventHandler(this.btDBDel_Click);
            // 
            // chkDB
            // 
            this.chkDB.AutoSize = true;
            this.chkDB.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkDB.Location = new System.Drawing.Point(88, 32);
            this.chkDB.Name = "chkDB";
            this.chkDB.Size = new System.Drawing.Size(84, 16);
            this.chkDB.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkDB.TabIndex = 10;
            this.chkDB.Text = "数据库开关";
            this.chkDB.UseVisualStyleBackColor = true;
            
            // 
            // UCDBSetting
            // 
            this.Controls.Add(this.chkDB);
            this.Controls.Add(this.btDBDel);
            this.Controls.Add(this.btDBOk);
            this.Controls.Add(this.textPwd);
            this.Controls.Add(this.textUser);
            this.Controls.Add(this.textDB);
            this.Controls.Add(this.lbDBPwd);
            this.Controls.Add(this.lbDBUser);
            this.Controls.Add(this.lbDBSource);
            this.Controls.Add(this.textIP);
            this.Controls.Add(this.lbDBIp);
            this.Name = "UCDBSetting";
            this.Size = new System.Drawing.Size(466, 421);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lbDBIp;
        private Skyray.Controls.TextBoxW textIP;
        private Skyray.Controls.LabelW lbDBSource;
        private Skyray.Controls.LabelW lbDBUser;
        private Skyray.Controls.LabelW lbDBPwd;
        private Skyray.Controls.TextBoxW textDB;
        private Skyray.Controls.TextBoxW textUser;
        private Skyray.Controls.TextBoxW textPwd;
        private Skyray.Controls.ButtonW btDBOk;
        private Skyray.Controls.ButtonW btDBDel;
        private Skyray.Controls.CheckBoxW chkDB;
    }
}
