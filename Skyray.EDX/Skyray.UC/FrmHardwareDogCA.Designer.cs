namespace Skyray.UC
{
    partial class FrmHardwareDogCA
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
            this.txtActiveDecode = new System.Windows.Forms.TextBox();
            this.lblActiveDecode = new System.Windows.Forms.Label();
            this.btnActive = new Skyray.Controls.ButtonW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.lblDevSN = new System.Windows.Forms.Label();
            this.lblPlanTelNum = new Skyray.Controls.LabelW();
            this.lblPlanTel = new Skyray.Controls.LabelW();
            this.lblPlanEmailAD = new Skyray.Controls.LabelW();
            this.lblPlanEmail = new Skyray.Controls.LabelW();
            this.richTextPlan = new System.Windows.Forms.RichTextBox();
            this.txtDevSN = new System.Windows.Forms.TextBox();
            this.txtDevState = new System.Windows.Forms.TextBox();
            this.lblDevState = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblRealTime = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.lblVer = new System.Windows.Forms.Label();
            this.btnVerify = new Skyray.Controls.ButtonW();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.lbldSN = new System.Windows.Forms.Label();
            this.lblgSN = new System.Windows.Forms.Label();
            this.lblXSN = new System.Windows.Forms.Label();
            this.btnSelect = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // txtActiveDecode
            // 
            this.txtActiveDecode.Location = new System.Drawing.Point(105, 21);
            this.txtActiveDecode.Name = "txtActiveDecode";
            this.txtActiveDecode.Size = new System.Drawing.Size(207, 21);
            this.txtActiveDecode.TabIndex = 19;
            // 
            // lblActiveDecode
            // 
            this.lblActiveDecode.AutoSize = true;
            this.lblActiveDecode.Location = new System.Drawing.Point(22, 24);
            this.lblActiveDecode.Name = "lblActiveDecode";
            this.lblActiveDecode.Size = new System.Drawing.Size(65, 12);
            this.lblActiveDecode.TabIndex = 18;
            this.lblActiveDecode.Text = "重授权码：";
            // 
            // btnActive
            // 
            this.btnActive.bSilver = false;
            this.btnActive.Location = new System.Drawing.Point(236, 347);
            this.btnActive.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnActive.MenuPos = new System.Drawing.Point(0, 0);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(75, 23);
            this.btnActive.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnActive.TabIndex = 21;
            this.btnActive.Text = "授权";
            this.btnActive.ToFocused = false;
            this.btnActive.UseVisualStyleBackColor = true;
            this.btnActive.Click += new System.EventHandler(this.btnActive_Click);
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(23, 347);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "关闭";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblDevSN
            // 
            this.lblDevSN.AutoSize = true;
            this.lblDevSN.Location = new System.Drawing.Point(22, 61);
            this.lblDevSN.Name = "lblDevSN";
            this.lblDevSN.Size = new System.Drawing.Size(65, 12);
            this.lblDevSN.TabIndex = 23;
            this.lblDevSN.Text = "设备名称：";
            // 
            // lblPlanTelNum
            // 
            this.lblPlanTelNum.AutoSize = true;
            this.lblPlanTelNum.BackColor = System.Drawing.Color.Transparent;
            this.lblPlanTelNum.Location = new System.Drawing.Point(136, 397);
            this.lblPlanTelNum.Name = "lblPlanTelNum";
            this.lblPlanTelNum.Size = new System.Drawing.Size(89, 12);
            this.lblPlanTelNum.TabIndex = 27;
            this.lblPlanTelNum.Text = "(0512)57017022";
            // 
            // lblPlanTel
            // 
            this.lblPlanTel.AutoSize = true;
            this.lblPlanTel.BackColor = System.Drawing.Color.Transparent;
            this.lblPlanTel.Location = new System.Drawing.Point(22, 397);
            this.lblPlanTel.Name = "lblPlanTel";
            this.lblPlanTel.Size = new System.Drawing.Size(65, 12);
            this.lblPlanTel.TabIndex = 26;
            this.lblPlanTel.Text = "联系电话：";
            // 
            // lblPlanEmailAD
            // 
            this.lblPlanEmailAD.AutoSize = true;
            this.lblPlanEmailAD.BackColor = System.Drawing.Color.Transparent;
            this.lblPlanEmailAD.Location = new System.Drawing.Point(136, 430);
            this.lblPlanEmailAD.Name = "lblPlanEmailAD";
            this.lblPlanEmailAD.Size = new System.Drawing.Size(155, 12);
            this.lblPlanEmailAD.TabIndex = 29;
            this.lblPlanEmailAD.Text = "pmc@skyray-instrument.com";
            // 
            // lblPlanEmail
            // 
            this.lblPlanEmail.AutoSize = true;
            this.lblPlanEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblPlanEmail.Location = new System.Drawing.Point(22, 430);
            this.lblPlanEmail.Name = "lblPlanEmail";
            this.lblPlanEmail.Size = new System.Drawing.Size(65, 12);
            this.lblPlanEmail.TabIndex = 28;
            this.lblPlanEmail.Text = "联系邮箱：";
            // 
            // richTextPlan
            // 
            this.richTextPlan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextPlan.Location = new System.Drawing.Point(24, 463);
            this.richTextPlan.Name = "richTextPlan";
            this.richTextPlan.ReadOnly = true;
            this.richTextPlan.Size = new System.Drawing.Size(288, 55);
            this.richTextPlan.TabIndex = 30;
            this.richTextPlan.Text = "请将设备名称发到上述邮箱，并注明贵司全称，请在邮件发出后拨打联系电话确认，我司收到邮件后会发送一个解码文件给您！";
            // 
            // txtDevSN
            // 
            this.txtDevSN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDevSN.Location = new System.Drawing.Point(105, 58);
            this.txtDevSN.Name = "txtDevSN";
            this.txtDevSN.ReadOnly = true;
            this.txtDevSN.Size = new System.Drawing.Size(206, 14);
            this.txtDevSN.TabIndex = 31;
            // 
            // txtDevState
            // 
            this.txtDevState.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDevState.Location = new System.Drawing.Point(105, 91);
            this.txtDevState.Name = "txtDevState";
            this.txtDevState.ReadOnly = true;
            this.txtDevState.Size = new System.Drawing.Size(206, 14);
            this.txtDevState.TabIndex = 33;
            // 
            // lblDevState
            // 
            this.lblDevState.AutoSize = true;
            this.lblDevState.Location = new System.Drawing.Point(22, 94);
            this.lblDevState.Name = "lblDevState";
            this.lblDevState.Size = new System.Drawing.Size(65, 12);
            this.lblDevState.TabIndex = 32;
            this.lblDevState.Text = "设备状态：";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(105, 126);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(206, 14);
            this.textBox2.TabIndex = 37;
            // 
            // lblRealTime
            // 
            this.lblRealTime.AutoSize = true;
            this.lblRealTime.Location = new System.Drawing.Point(22, 129);
            this.lblRealTime.Name = "lblRealTime";
            this.lblRealTime.Size = new System.Drawing.Size(65, 12);
            this.lblRealTime.TabIndex = 36;
            this.lblRealTime.Text = "实际时间：";
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(105, 164);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(206, 14);
            this.textBox3.TabIndex = 39;
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Location = new System.Drawing.Point(22, 166);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(65, 12);
            this.lblEndTime.TabIndex = 38;
            this.lblEndTime.Text = "过期时间：";
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(105, 196);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(206, 14);
            this.textBox4.TabIndex = 41;
            // 
            // lblVer
            // 
            this.lblVer.AutoSize = true;
            this.lblVer.Location = new System.Drawing.Point(22, 196);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(65, 12);
            this.lblVer.TabIndex = 40;
            this.lblVer.Text = "设备版本：";
            // 
            // btnVerify
            // 
            this.btnVerify.bSilver = false;
            this.btnVerify.Location = new System.Drawing.Point(123, 347);
            this.btnVerify.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnVerify.MenuPos = new System.Drawing.Point(0, 0);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(75, 23);
            this.btnVerify.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnVerify.TabIndex = 48;
            this.btnVerify.Text = "验证";
            this.btnVerify.ToFocused = false;
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.buttonW1_Click);
            // 
            // textBox7
            // 
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox7.Location = new System.Drawing.Point(106, 299);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(206, 14);
            this.textBox7.TabIndex = 47;
            // 
            // textBox6
            // 
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Location = new System.Drawing.Point(105, 262);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(206, 14);
            this.textBox6.TabIndex = 45;
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Location = new System.Drawing.Point(105, 228);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(206, 14);
            this.textBox5.TabIndex = 35;
            // 
            // lbldSN
            // 
            this.lbldSN.AutoSize = true;
            this.lbldSN.Location = new System.Drawing.Point(22, 230);
            this.lbldSN.Name = "lbldSN";
            this.lbldSN.Size = new System.Drawing.Size(53, 12);
            this.lbldSN.TabIndex = 34;
            this.lbldSN.Text = "多道SN：";
            // 
            // lblgSN
            // 
            this.lblgSN.AutoSize = true;
            this.lblgSN.Location = new System.Drawing.Point(22, 264);
            this.lblgSN.Name = "lblgSN";
            this.lblgSN.Size = new System.Drawing.Size(53, 12);
            this.lblgSN.TabIndex = 44;
            this.lblgSN.Text = "高压SN：";
            // 
            // lblXSN
            // 
            this.lblXSN.AutoSize = true;
            this.lblXSN.Location = new System.Drawing.Point(23, 302);
            this.lblXSN.Name = "lblXSN";
            this.lblXSN.Size = new System.Drawing.Size(59, 12);
            this.lblXSN.TabIndex = 46;
            this.lblXSN.Text = "X射线SN：";
            // 
            // btnSelect
            // 
            this.btnSelect.bSilver = false;
            this.btnSelect.Location = new System.Drawing.Point(315, 20);
            this.btnSelect.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSelect.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(46, 23);
            this.btnSelect.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSelect.TabIndex = 49;
            this.btnSelect.Text = "选择";
            this.btnSelect.ToFocused = false;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnselect_Click);
            // 
            // FrmHardwareDogCA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(364, 536);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.lblXSN);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.lblgSN);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.lblVer);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.lblEndTime);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.lblRealTime);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.lbldSN);
            this.Controls.Add(this.txtDevState);
            this.Controls.Add(this.lblDevState);
            this.Controls.Add(this.txtDevSN);
            this.Controls.Add(this.richTextPlan);
            this.Controls.Add(this.lblPlanEmailAD);
            this.Controls.Add(this.lblPlanEmail);
            this.Controls.Add(this.lblPlanTelNum);
            this.Controls.Add(this.lblPlanTel);
            this.Controls.Add(this.lblDevSN);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnActive);
            this.Controls.Add(this.txtActiveDecode);
            this.Controls.Add(this.lblActiveDecode);
            this.Name = "FrmHardwareDogCA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "授权";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtActiveDecode;
        private System.Windows.Forms.Label lblActiveDecode;
        private Skyray.Controls.ButtonW btnActive;
        private Skyray.Controls.ButtonW btnClose;
        private System.Windows.Forms.Label lblDevSN;
      
        private Skyray.Controls.LabelW lblPlanTelNum;
        private Skyray.Controls.LabelW lblPlanTel;
        private Skyray.Controls.LabelW lblPlanEmailAD;
        private Skyray.Controls.LabelW lblPlanEmail;
        private System.Windows.Forms.RichTextBox richTextPlan;
        private System.Windows.Forms.TextBox txtDevSN;
        private System.Windows.Forms.TextBox txtDevState;
        private System.Windows.Forms.Label lblDevState;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblRealTime;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label lblVer;
        private Skyray.Controls.ButtonW btnVerify;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label lbldSN;
        private System.Windows.Forms.Label lblgSN;
        private System.Windows.Forms.Label lblXSN;
        private Skyray.Controls.ButtonW btnSelect;
    }
}