namespace Skyray.UC
{
    partial class FrmDeviceNewName
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
            this.lblNewDeviceName = new Skyray.Controls.LabelW();
            this.txtNewDeviceName = new Skyray.Controls.TextBoxW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // lblNewDeviceName
            // 
            this.lblNewDeviceName.AutoSize = true;
            this.lblNewDeviceName.BackColor = System.Drawing.Color.Transparent;
            this.lblNewDeviceName.Location = new System.Drawing.Point(4, 13);
            this.lblNewDeviceName.Name = "lblNewDeviceName";
            this.lblNewDeviceName.Size = new System.Drawing.Size(65, 12);
            this.lblNewDeviceName.TabIndex = 0;
            this.lblNewDeviceName.Text = "新设备名：";
            // 
            // txtNewDeviceName
            // 
            this.txtNewDeviceName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtNewDeviceName.Location = new System.Drawing.Point(98, 10);
            this.txtNewDeviceName.Name = "txtNewDeviceName";
            this.txtNewDeviceName.Size = new System.Drawing.Size(152, 21);
            this.txtNewDeviceName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtNewDeviceName.TabIndex = 1;
            this.txtNewDeviceName.Enter += new System.EventHandler(this.txtNewDeviceName_Enter);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(150, 35);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(46, 23);
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
            this.btnCancel.Location = new System.Drawing.Point(202, 35);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(46, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmDeviceNewName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 64);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtNewDeviceName);
            this.Controls.Add(this.lblNewDeviceName);
            this.Name = "FrmDeviceNewName";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新设备名";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblNewDeviceName;
        private Skyray.Controls.TextBoxW txtNewDeviceName;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
    }
}