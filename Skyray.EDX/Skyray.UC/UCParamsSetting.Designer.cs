namespace Skyray.UC
{
    partial class UCParamsSetting
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMatchLevl = new Skyray.Controls.LabelW();
            this.txtMatchLevel = new Skyray.Controls.TextBoxW();
            this.btnSubmit = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.lblPercent = new Skyray.Controls.LabelW();
            this.SuspendLayout();
            // 
            // lblMatchLevl
            // 
            this.lblMatchLevl.AutoSize = true;
            this.lblMatchLevl.BackColor = System.Drawing.Color.Transparent;
            this.lblMatchLevl.Location = new System.Drawing.Point(17, 26);
            this.lblMatchLevl.Name = "lblMatchLevl";
            this.lblMatchLevl.Size = new System.Drawing.Size(71, 12);
            this.lblMatchLevl.TabIndex = 0;
            this.lblMatchLevl.Text = "匹配度下限:";
            // 
            // txtMatchLevel
            // 
            this.txtMatchLevel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtMatchLevel.Location = new System.Drawing.Point(107, 23);
            this.txtMatchLevel.Name = "txtMatchLevel";
            this.txtMatchLevel.Size = new System.Drawing.Size(163, 21);
            this.txtMatchLevel.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtMatchLevel.TabIndex = 1;
            // 
            // btnSubmit
            // 
            this.btnSubmit.bSilver = false;
            this.btnSubmit.Location = new System.Drawing.Point(107, 58);
            this.btnSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(79, 23);
            this.btnSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.ToFocused = false;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(196, 58);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.BackColor = System.Drawing.Color.Transparent;
            this.lblPercent.Location = new System.Drawing.Point(275, 26);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(23, 12);
            this.lblPercent.TabIndex = 4;
            this.lblPercent.Text = "(%)";
            // 
            // UCParamsSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtMatchLevel);
            this.Controls.Add(this.lblMatchLevl);
            this.Name = "UCParamsSetting";
            this.Size = new System.Drawing.Size(305, 95);
            this.Load += new System.EventHandler(this.UCParamsSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblMatchLevl;
        private Skyray.Controls.TextBoxW txtMatchLevel;
        private Skyray.Controls.ButtonW btnSubmit;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.LabelW lblPercent;
    }
}
