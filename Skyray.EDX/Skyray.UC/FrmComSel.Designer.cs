namespace Skyray.UC
{
    partial class FrmComSel
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
            this.lbSerial = new Skyray.Controls.LabelW();
            this.lbFreq = new Skyray.Controls.LabelW();
            this.lbTimeout = new Skyray.Controls.LabelW();
            this.btnOk = new Skyray.Controls.ButtonW();
            this.btnCanel = new Skyray.Controls.ButtonW();
            this.cboComList = new System.Windows.Forms.ComboBox();
            this.tbFrequency = new System.Windows.Forms.TextBox();
            this.tbTimeout = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbSerial
            // 
            this.lbSerial.AutoSize = true;
            this.lbSerial.BackColor = System.Drawing.Color.Transparent;
            this.lbSerial.Location = new System.Drawing.Point(78, 85);
            this.lbSerial.Name = "lbSerial";
            this.lbSerial.Size = new System.Drawing.Size(29, 12);
            this.lbSerial.TabIndex = 1;
            this.lbSerial.Text = "串口";
            // 
            // lbFreq
            // 
            this.lbFreq.AutoSize = true;
            this.lbFreq.BackColor = System.Drawing.Color.Transparent;
            this.lbFreq.Location = new System.Drawing.Point(78, 134);
            this.lbFreq.Name = "lbFreq";
            this.lbFreq.Size = new System.Drawing.Size(29, 12);
            this.lbFreq.TabIndex = 2;
            this.lbFreq.Text = "频率";
            // 
            // lbTimeout
            // 
            this.lbTimeout.AutoSize = true;
            this.lbTimeout.BackColor = System.Drawing.Color.Transparent;
            this.lbTimeout.Location = new System.Drawing.Point(78, 188);
            this.lbTimeout.Name = "lbTimeout";
            this.lbTimeout.Size = new System.Drawing.Size(53, 12);
            this.lbTimeout.TabIndex = 3;
            this.lbTimeout.Text = "超时时间";
            // 
            // btnOk
            // 
            this.btnOk.bSilver = false;
            this.btnOk.Location = new System.Drawing.Point(67, 248);
            this.btnOk.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOk.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "确定";
            this.btnOk.ToFocused = false;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCanel
            // 
            this.btnCanel.bSilver = false;
            this.btnCanel.Location = new System.Drawing.Point(241, 248);
            this.btnCanel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCanel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCanel.Name = "btnCanel";
            this.btnCanel.Size = new System.Drawing.Size(75, 23);
            this.btnCanel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCanel.TabIndex = 10;
            this.btnCanel.Text = "取消";
            this.btnCanel.ToFocused = false;
            this.btnCanel.UseVisualStyleBackColor = true;
            this.btnCanel.Click += new System.EventHandler(this.btnCanel_Click);
            // 
            // cboComList
            // 
            this.cboComList.FormattingEnabled = true;
            this.cboComList.Location = new System.Drawing.Point(170, 85);
            this.cboComList.Name = "cboComList";
            this.cboComList.Size = new System.Drawing.Size(88, 20);
            this.cboComList.TabIndex = 32;
            // 
            // tbFrequency
            // 
            this.tbFrequency.Location = new System.Drawing.Point(170, 134);
            this.tbFrequency.Name = "tbFrequency";
            this.tbFrequency.Size = new System.Drawing.Size(88, 21);
            this.tbFrequency.TabIndex = 33;
            this.tbFrequency.Text = "9600";
            // 
            // tbTimeout
            // 
            this.tbTimeout.Enabled = false;
            this.tbTimeout.Location = new System.Drawing.Point(170, 188);
            this.tbTimeout.Name = "tbTimeout";
            this.tbTimeout.Size = new System.Drawing.Size(88, 21);
            this.tbTimeout.TabIndex = 34;
            this.tbTimeout.Text = "500";
            // 
            // FrmComSel
            // 
            this.Controls.Add(this.tbTimeout);
            this.Controls.Add(this.tbFrequency);
            this.Controls.Add(this.cboComList);
            this.Controls.Add(this.btnCanel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lbTimeout);
            this.Controls.Add(this.lbFreq);
            this.Controls.Add(this.lbSerial);
            this.Name = "FrmComSel";
            this.Size = new System.Drawing.Size(441, 394);
            this.Load += new System.EventHandler(this.FrmComSel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lbSerial;
        private Skyray.Controls.LabelW lbFreq;
        private Skyray.Controls.LabelW lbTimeout;
        private Skyray.Controls.ButtonW btnOk;
        private Skyray.Controls.ButtonW btnCanel;
        private System.Windows.Forms.ComboBox cboComList;
        private System.Windows.Forms.TextBox tbFrequency;
        private System.Windows.Forms.TextBox tbTimeout;
    }
}
