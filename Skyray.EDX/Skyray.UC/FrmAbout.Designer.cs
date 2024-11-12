namespace Skyray.UC
{
    partial class FrmAbout
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
            this.linkLabelWeb = new System.Windows.Forms.LinkLabel();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.lblHomePage = new Skyray.Controls.LabelW();
            this.lblFax = new Skyray.Controls.LabelW();
            this.lblAbout = new Skyray.Controls.LabelW();
            this.lblFaxNum = new Skyray.Controls.LabelW();
            this.lblTelNum = new Skyray.Controls.LabelW();
            this.lblTel = new Skyray.Controls.LabelW();
            this.lblVersion = new Skyray.Controls.LabelW();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkLabelWeb
            // 
            this.linkLabelWeb.AutoSize = true;
            this.linkLabelWeb.Location = new System.Drawing.Point(111, 33);
            this.linkLabelWeb.Name = "linkLabelWeb";
            this.linkLabelWeb.Size = new System.Drawing.Size(169, 13);
            this.linkLabelWeb.TabIndex = 2;
            this.linkLabelWeb.TabStop = true;
            this.linkLabelWeb.Text = "http://www.skyray-instrument.com";
            this.linkLabelWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWeb_LinkClicked);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(209, 72);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(86, 25);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblHomePage
            // 
            this.lblHomePage.AutoSize = true;
            this.lblHomePage.BackColor = System.Drawing.Color.Transparent;
            this.lblHomePage.Location = new System.Drawing.Point(5, 35);
            this.lblHomePage.Name = "lblHomePage";
            this.lblHomePage.Size = new System.Drawing.Size(55, 13);
            this.lblHomePage.TabIndex = 1;
            this.lblHomePage.Text = "公司主页";
            // 
            // lblFax
            // 
            this.lblFax.AutoSize = true;
            this.lblFax.BackColor = System.Drawing.Color.Transparent;
            this.lblFax.Location = new System.Drawing.Point(5, 86);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(43, 13);
            this.lblFax.TabIndex = 1;
            this.lblFax.Text = "传真：";
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.BackColor = System.Drawing.Color.Transparent;
            this.lblAbout.Location = new System.Drawing.Point(5, 9);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(115, 13);
            this.lblAbout.TabIndex = 0;
            this.lblAbout.Text = "EDXRF 2.0.71.140208";
            // 
            // lblFaxNum
            // 
            this.lblFaxNum.AutoSize = true;
            this.lblFaxNum.BackColor = System.Drawing.Color.Transparent;
            this.lblFaxNum.Location = new System.Drawing.Point(110, 83);
            this.lblFaxNum.Name = "lblFaxNum";
            this.lblFaxNum.Size = new System.Drawing.Size(85, 13);
            this.lblFaxNum.TabIndex = 0;
            this.lblFaxNum.Text = "(0512)57017001";
            // 
            // lblTelNum
            // 
            this.lblTelNum.AutoSize = true;
            this.lblTelNum.BackColor = System.Drawing.Color.Transparent;
            this.lblTelNum.Location = new System.Drawing.Point(110, 59);
            this.lblTelNum.Name = "lblTelNum";
            this.lblTelNum.Size = new System.Drawing.Size(85, 13);
            this.lblTelNum.TabIndex = 0;
            this.lblTelNum.Text = "(0512)57017888";
            // 
            // lblTel
            // 
            this.lblTel.AutoSize = true;
            this.lblTel.BackColor = System.Drawing.Color.Transparent;
            this.lblTel.Location = new System.Drawing.Point(5, 61);
            this.lblTel.Name = "lblTel";
            this.lblTel.Size = new System.Drawing.Size(43, 13);
            this.lblTel.TabIndex = 0;
            this.lblTel.Text = "电话：";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVersion.Location = new System.Drawing.Point(112, 30);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(143, 14);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "EDXRF NapcoV.01PC";
            this.lblVersion.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(-16, 102);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(126, 101);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.linkLabelWeb);
            this.Controls.Add(this.lblHomePage);
            this.Controls.Add(this.lblFax);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.lblFaxNum);
            this.Controls.Add(this.lblTelNum);
            this.Controls.Add(this.lblTel);
            this.Name = "FrmAbout";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(343, 126);
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblTel;
        private Skyray.Controls.LabelW lblFax;
        private Skyray.Controls.LabelW lblAbout;
        private Skyray.Controls.LabelW lblHomePage;
        private System.Windows.Forms.LinkLabel linkLabelWeb;
        private Skyray.Controls.LabelW lblTelNum;
        private Skyray.Controls.LabelW lblFaxNum;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.LabelW lblVersion;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}