namespace Skyray.UC
{
    partial class UCIPSetting
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
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.lblIP = new Skyray.Controls.LabelW();
            this.txtIP = new Skyray.Controls.IPTextBox();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.txtSubnet = new Skyray.Controls.IPTextBox();
            this.lblSubNet = new Skyray.Controls.LabelW();
            this.txtGateWay = new Skyray.Controls.IPTextBox();
            this.lblGateWay = new Skyray.Controls.LabelW();
            this.txtDNS = new Skyray.Controls.IPTextBox();
            this.lblDNS = new Skyray.Controls.LabelW();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(205, 197);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 25);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = " 取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.buttonW2_Click);
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.BackColor = System.Drawing.Color.Transparent;
            this.lblIP.Location = new System.Drawing.Point(30, 43);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(29, 13);
            this.lblIP.TabIndex = 1;
            this.lblIP.Text = "IP：";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(103, 38);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(180, 23);
            this.txtIP.TabIndex = 10;
            this.txtIP.ToolTipText = "";
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(100, 197);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 25);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtSubnet
            // 
            this.txtSubnet.Location = new System.Drawing.Point(103, 79);
            this.txtSubnet.Name = "txtSubnet";
            this.txtSubnet.Size = new System.Drawing.Size(180, 23);
            this.txtSubnet.TabIndex = 12;
            this.txtSubnet.ToolTipText = "";
            // 
            // lblSubNet
            // 
            this.lblSubNet.AutoSize = true;
            this.lblSubNet.BackColor = System.Drawing.Color.Transparent;
            this.lblSubNet.Location = new System.Drawing.Point(30, 84);
            this.lblSubNet.Name = "lblSubNet";
            this.lblSubNet.Size = new System.Drawing.Size(67, 13);
            this.lblSubNet.TabIndex = 11;
            this.lblSubNet.Text = "子网掩码：";
            // 
            // txtGateWay
            // 
            this.txtGateWay.Location = new System.Drawing.Point(103, 118);
            this.txtGateWay.Name = "txtGateWay";
            this.txtGateWay.Size = new System.Drawing.Size(180, 23);
            this.txtGateWay.TabIndex = 14;
            this.txtGateWay.ToolTipText = "";
            // 
            // lblGateWay
            // 
            this.lblGateWay.AutoSize = true;
            this.lblGateWay.BackColor = System.Drawing.Color.Transparent;
            this.lblGateWay.Location = new System.Drawing.Point(30, 123);
            this.lblGateWay.Name = "lblGateWay";
            this.lblGateWay.Size = new System.Drawing.Size(43, 13);
            this.lblGateWay.TabIndex = 13;
            this.lblGateWay.Text = "网关：";
            // 
            // txtDNS
            // 
            this.txtDNS.Location = new System.Drawing.Point(103, 157);
            this.txtDNS.Name = "txtDNS";
            this.txtDNS.Size = new System.Drawing.Size(180, 23);
            this.txtDNS.TabIndex = 16;
            this.txtDNS.ToolTipText = "";
            // 
            // lblDNS
            // 
            this.lblDNS.AutoSize = true;
            this.lblDNS.BackColor = System.Drawing.Color.Transparent;
            this.lblDNS.Location = new System.Drawing.Point(30, 162);
            this.lblDNS.Name = "lblDNS";
            this.lblDNS.Size = new System.Drawing.Size(42, 13);
            this.lblDNS.TabIndex = 15;
            this.lblDNS.Text = "DNS：";
            // 
            // UCIPSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.txtDNS);
            this.Controls.Add(this.lblDNS);
            this.Controls.Add(this.txtGateWay);
            this.Controls.Add(this.lblGateWay);
            this.Controls.Add(this.txtSubnet);
            this.Controls.Add(this.lblSubNet);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblIP);
            this.Name = "UCIPSetting";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(317, 234);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblIP;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.IPTextBox txtIP;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.IPTextBox txtSubnet;
        private Skyray.Controls.LabelW lblSubNet;
        private Skyray.Controls.IPTextBox txtGateWay;
        private Skyray.Controls.LabelW lblGateWay;
        private Skyray.Controls.IPTextBox txtDNS;
        private Skyray.Controls.LabelW lblDNS;
    }
}
