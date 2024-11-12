namespace Skyray.UC
{
    partial class UCDetectPointsSetting
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
            this.lbRoiRadius = new Skyray.Controls.LabelW();
            this.lbMaxPixelErr = new Skyray.Controls.LabelW();
            this.btDBOk = new Skyray.Controls.ButtonW();
            this.btDBDel = new Skyray.Controls.ButtonW();
            this.chkDP = new Skyray.Controls.CheckBoxW();
            this.txtRoiRadius = new Skyray.Controls.TextBoxW();
            this.txtMaxPixelErr = new Skyray.Controls.TextBoxW();
            this.lbMaxDetectNum = new Skyray.Controls.LabelW();
            this.txtMaxDetectNum = new Skyray.Controls.TextBoxW();
            this.SuspendLayout();
            // 
            // lbRoiRadius
            // 
            this.lbRoiRadius.AutoSize = true;
            this.lbRoiRadius.BackColor = System.Drawing.Color.Transparent;
            this.lbRoiRadius.Location = new System.Drawing.Point(86, 102);
            this.lbRoiRadius.Name = "lbRoiRadius";
            this.lbRoiRadius.Size = new System.Drawing.Size(101, 12);
            this.lbRoiRadius.TabIndex = 0;
            this.lbRoiRadius.Text = "待检颜色摄取半径";
            // 
            // lbMaxPixelErr
            // 
            this.lbMaxPixelErr.AutoSize = true;
            this.lbMaxPixelErr.BackColor = System.Drawing.Color.Transparent;
            this.lbMaxPixelErr.Location = new System.Drawing.Point(86, 163);
            this.lbMaxPixelErr.Name = "lbMaxPixelErr";
            this.lbMaxPixelErr.Size = new System.Drawing.Size(89, 12);
            this.lbMaxPixelErr.TabIndex = 2;
            this.lbMaxPixelErr.Text = "像素值误差上限";
            // 
            // btDBOk
            // 
            this.btDBOk.bSilver = false;
            this.btDBOk.Location = new System.Drawing.Point(88, 307);
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
            this.btDBDel.Location = new System.Drawing.Point(272, 307);
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
            // chkDP
            // 
            this.chkDP.AutoSize = true;
            this.chkDP.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkDP.Location = new System.Drawing.Point(88, 51);
            this.chkDP.Name = "chkDP";
            this.chkDP.Size = new System.Drawing.Size(96, 16);
            this.chkDP.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkDP.TabIndex = 10;
            this.chkDP.Text = "多点检测开关";
            this.chkDP.UseVisualStyleBackColor = true;
            // 
            // txtRoiRadius
            // 
            this.txtRoiRadius.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtRoiRadius.Location = new System.Drawing.Point(194, 102);
            this.txtRoiRadius.Name = "txtRoiRadius";
            this.txtRoiRadius.Size = new System.Drawing.Size(100, 21);
            this.txtRoiRadius.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtRoiRadius.TabIndex = 11;
            // 
            // txtMaxPixelErr
            // 
            this.txtMaxPixelErr.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtMaxPixelErr.Location = new System.Drawing.Point(194, 163);
            this.txtMaxPixelErr.Name = "txtMaxPixelErr";
            this.txtMaxPixelErr.Size = new System.Drawing.Size(100, 21);
            this.txtMaxPixelErr.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtMaxPixelErr.TabIndex = 12;
            // 
            // lbMaxDetectNum
            // 
            this.lbMaxDetectNum.AutoSize = true;
            this.lbMaxDetectNum.BackColor = System.Drawing.Color.Transparent;
            this.lbMaxDetectNum.Location = new System.Drawing.Point(86, 227);
            this.lbMaxDetectNum.Name = "lbMaxDetectNum";
            this.lbMaxDetectNum.Size = new System.Drawing.Size(77, 12);
            this.lbMaxDetectNum.TabIndex = 14;
            this.lbMaxDetectNum.Text = "最大检测个数";
            // 
            // txtMaxDetectNum
            // 
            this.txtMaxDetectNum.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtMaxDetectNum.Location = new System.Drawing.Point(194, 227);
            this.txtMaxDetectNum.Name = "txtMaxDetectNum";
            this.txtMaxDetectNum.Size = new System.Drawing.Size(100, 21);
            this.txtMaxDetectNum.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtMaxDetectNum.TabIndex = 15;
            // 
            // UCDetectPointsSetting
            // 
            this.Controls.Add(this.txtMaxDetectNum);
            this.Controls.Add(this.lbMaxDetectNum);
            this.Controls.Add(this.txtMaxPixelErr);
            this.Controls.Add(this.txtRoiRadius);
            this.Controls.Add(this.chkDP);
            this.Controls.Add(this.btDBDel);
            this.Controls.Add(this.btDBOk);
            this.Controls.Add(this.lbMaxPixelErr);
            this.Controls.Add(this.lbRoiRadius);
            this.Name = "UCDetectPointsSetting";
            this.Size = new System.Drawing.Size(466, 421);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lbRoiRadius;
        private Skyray.Controls.LabelW lbMaxPixelErr;
        private Skyray.Controls.ButtonW btDBOk;
        private Skyray.Controls.ButtonW btDBDel;
        private Skyray.Controls.CheckBoxW chkDP;
        private Skyray.Controls.TextBoxW txtRoiRadius;
        private Skyray.Controls.TextBoxW txtMaxPixelErr;
        private Skyray.Controls.LabelW lbMaxDetectNum;
        private Skyray.Controls.TextBoxW txtMaxDetectNum;
    }
}
