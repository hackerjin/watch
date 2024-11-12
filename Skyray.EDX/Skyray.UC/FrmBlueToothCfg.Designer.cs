namespace Skyray.UC
{
    partial class FrmBlueToothCfg
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
            this.btnCon = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.lblBlueSetName = new Skyray.Controls.LabelW();
            this.lblstate = new Skyray.Controls.LabelW();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbmDevices = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCon
            // 
            this.btnCon.Location = new System.Drawing.Point(233, 85);
            this.btnCon.Name = "btnCon";
            this.btnCon.Size = new System.Drawing.Size(93, 28);
            this.btnCon.TabIndex = 2;
            this.btnCon.Text = "连接";
            this.btnCon.UseVisualStyleBackColor = true;
            this.btnCon.Click += new System.EventHandler(this.btnCon_Click);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(233, 49);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(93, 29);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "搜索";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lblBlueSetName
            // 
            this.lblBlueSetName.AutoSize = true;
            this.lblBlueSetName.BackColor = System.Drawing.Color.Transparent;
            this.lblBlueSetName.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBlueSetName.Location = new System.Drawing.Point(24, 30);
            this.lblBlueSetName.Name = "lblBlueSetName";
            this.lblBlueSetName.Size = new System.Drawing.Size(77, 14);
            this.lblBlueSetName.TabIndex = 6;
            this.lblBlueSetName.Text = "蓝牙设备：";
            // 
            // lblstate
            // 
            this.lblstate.AutoSize = true;
            this.lblstate.BackColor = System.Drawing.Color.Transparent;
            this.lblstate.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblstate.Location = new System.Drawing.Point(12, 134);
            this.lblstate.Name = "lblstate";
            this.lblstate.Size = new System.Drawing.Size(151, 14);
            this.lblstate.TabIndex = 7;
            this.lblstate.Text = "                  ";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(233, 119);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(93, 29);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbmDevices
            // 
            this.cbmDevices.FormattingEnabled = true;
            this.cbmDevices.Location = new System.Drawing.Point(61, 54);
            this.cbmDevices.Name = "cbmDevices";
            this.cbmDevices.Size = new System.Drawing.Size(166, 20);
            this.cbmDevices.TabIndex = 12;
            // 
            // FrmBlueToothCfg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 173);
            this.Controls.Add(this.cbmDevices);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblstate);
            this.Controls.Add(this.lblBlueSetName);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnCon);
            this.Name = "FrmBlueToothCfg";
            this.Text = "蓝牙设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCon;
        private System.Windows.Forms.Button btnScan;
        private Skyray.Controls.LabelW lblBlueSetName;
        private Skyray.Controls.LabelW lblstate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cbmDevices;
        

    }
}