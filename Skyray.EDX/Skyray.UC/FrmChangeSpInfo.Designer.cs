namespace Skyray.UC
{
    partial class FrmChangeSpInfo
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonWSubmit = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(354, 371);
            this.panel1.TabIndex = 0;
            // 
            // buttonWSubmit
            // 
            this.buttonWSubmit.bSilver = false;
            this.buttonWSubmit.Location = new System.Drawing.Point(368, 310);
            this.buttonWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.Name = "buttonWSubmit";
            this.buttonWSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWSubmit.TabIndex = 17;
            this.buttonWSubmit.Text = "确定";
            this.buttonWSubmit.ToFocused = false;
            this.buttonWSubmit.UseVisualStyleBackColor = true;
            this.buttonWSubmit.Click += new System.EventHandler(this.buttonWSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(368, 356);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmChangeSpInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonWSubmit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panel1);
            this.Name = "FrmChangeSpInfo";
            this.Size = new System.Drawing.Size(454, 387);
            this.Load += new System.EventHandler(this.FrmChangeSpInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Skyray.Controls.ButtonW buttonWSubmit;
        private Skyray.Controls.ButtonW btnCancel;
    }
}