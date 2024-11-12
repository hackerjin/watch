namespace Skyray.UC
{
    partial class FrmAuthorization
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
            this.btnRequst = new Skyray.Controls.ButtonW();
            this.txtRequest = new Skyray.Controls.TextBoxW();
            this.lblShowRemark = new Skyray.Controls.LabelW();
            this.lblRequest = new Skyray.Controls.LabelW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // btnRequst
            // 
            this.btnRequst.bSilver = false;
            this.btnRequst.Location = new System.Drawing.Point(367, 52);
            this.btnRequst.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnRequst.MenuPos = new System.Drawing.Point(0, 0);
            this.btnRequst.Name = "btnRequst";
            this.btnRequst.Size = new System.Drawing.Size(105, 44);
            this.btnRequst.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnRequst.TabIndex = 0;
            this.btnRequst.Text = "生成授权请求";
            this.btnRequst.ToFocused = false;
            this.btnRequst.UseVisualStyleBackColor = true;
            this.btnRequst.Click += new System.EventHandler(this.btnRequst_Click);
            // 
            // txtRequest
            // 
            this.txtRequest.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtRequest.Location = new System.Drawing.Point(29, 52);
            this.txtRequest.Multiline = true;
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(292, 84);
            this.txtRequest.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtRequest.TabIndex = 1;
            // 
            // lblShowRemark
            // 
            this.lblShowRemark.AutoEllipsis = true;
            this.lblShowRemark.BackColor = System.Drawing.Color.Transparent;
            this.lblShowRemark.Location = new System.Drawing.Point(29, 164);
            this.lblShowRemark.Name = "lblShowRemark";
            this.lblShowRemark.Size = new System.Drawing.Size(283, 69);
            this.lblShowRemark.TabIndex = 2;
            this.lblShowRemark.Text = "请点击“生成授权请求”，将生成的授权请求码，\r\n\r\n将生成的授权码发送至天瑞客服用于获取授权文件。";
            this.lblShowRemark.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRequest
            // 
            this.lblRequest.AutoSize = true;
            this.lblRequest.BackColor = System.Drawing.Color.Transparent;
            this.lblRequest.Location = new System.Drawing.Point(29, 33);
            this.lblRequest.Name = "lblRequest";
            this.lblRequest.Size = new System.Drawing.Size(79, 13);
            this.lblRequest.TabIndex = 3;
            this.lblRequest.Text = "授权请求码：";
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(377, 176);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(105, 44);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "关闭";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // FrmAuthorization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblRequest);
            this.Controls.Add(this.lblShowRemark);
            this.Controls.Add(this.txtRequest);
            this.Controls.Add(this.btnRequst);
            this.Name = "FrmAuthorization";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(493, 251);
            this.Load += new System.EventHandler(this.FrmAuthorization_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btnRequst;
        private Skyray.Controls.TextBoxW txtRequest;
        private Skyray.Controls.LabelW lblShowRemark;
        private Skyray.Controls.LabelW lblRequest;
        private Skyray.Controls.ButtonW btnOK;

    }
}