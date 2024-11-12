namespace Skyray.UC
{
    partial class FrmSuperModel
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
            this.btnOK = new Skyray.Controls.ButtonW();
            this.radOneTarget = new System.Windows.Forms.RadioButton();
            this.radTwoTarget = new System.Windows.Forms.RadioButton();
            this.rad2050 = new System.Windows.Forms.RadioButton();
            this.rad2400 = new System.Windows.Forms.RadioButton();
            this.pnlMachine = new System.Windows.Forms.Panel();
            this.pnlTarget = new System.Windows.Forms.Panel();
            this.pnlMachine.SuspendLayout();
            this.pnlTarget.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(96, 157);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(126, 26);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // radOneTarget
            // 
            this.radOneTarget.AutoSize = true;
            this.radOneTarget.Location = new System.Drawing.Point(24, 32);
            this.radOneTarget.Name = "radOneTarget";
            this.radOneTarget.Size = new System.Drawing.Size(59, 16);
            this.radOneTarget.TabIndex = 2;
            this.radOneTarget.TabStop = true;
            this.radOneTarget.Text = "一次靶";
            this.radOneTarget.UseVisualStyleBackColor = true;
            this.radOneTarget.CheckedChanged += new System.EventHandler(this.radOneTarget_CheckedChanged);
            // 
            // radTwoTarget
            // 
            this.radTwoTarget.AutoSize = true;
            this.radTwoTarget.Location = new System.Drawing.Point(24, 82);
            this.radTwoTarget.Name = "radTwoTarget";
            this.radTwoTarget.Size = new System.Drawing.Size(59, 16);
            this.radTwoTarget.TabIndex = 3;
            this.radTwoTarget.TabStop = true;
            this.radTwoTarget.Text = "二次靶";
            this.radTwoTarget.UseVisualStyleBackColor = true;
            this.radTwoTarget.CheckedChanged += new System.EventHandler(this.radTwoTarget_CheckedChanged);
            // 
            // rad2050
            // 
            this.rad2050.AutoSize = true;
            this.rad2050.Location = new System.Drawing.Point(27, 32);
            this.rad2050.Name = "rad2050";
            this.rad2050.Size = new System.Drawing.Size(77, 16);
            this.rad2050.TabIndex = 5;
            this.rad2050.TabStop = true;
            this.rad2050.Text = "Super2050";
            this.rad2050.UseVisualStyleBackColor = true;
            this.rad2050.CheckedChanged += new System.EventHandler(this.rad2050_CheckedChanged);
            // 
            // rad2400
            // 
            this.rad2400.AutoSize = true;
            this.rad2400.Location = new System.Drawing.Point(27, 77);
            this.rad2400.Name = "rad2400";
            this.rad2400.Size = new System.Drawing.Size(77, 16);
            this.rad2400.TabIndex = 6;
            this.rad2400.TabStop = true;
            this.rad2400.Text = "Super2400";
            this.rad2400.UseVisualStyleBackColor = true;
            this.rad2400.CheckedChanged += new System.EventHandler(this.rad2400_CheckedChanged);
            // 
            // pnlMachine
            // 
            this.pnlMachine.Controls.Add(this.rad2400);
            this.pnlMachine.Controls.Add(this.rad2050);
            this.pnlMachine.Location = new System.Drawing.Point(22, 26);
            this.pnlMachine.Name = "pnlMachine";
            this.pnlMachine.Size = new System.Drawing.Size(133, 123);
            this.pnlMachine.TabIndex = 7;
            // 
            // pnlTarget
            // 
            this.pnlTarget.Controls.Add(this.radOneTarget);
            this.pnlTarget.Controls.Add(this.radTwoTarget);
            this.pnlTarget.Location = new System.Drawing.Point(171, 26);
            this.pnlTarget.Name = "pnlTarget";
            this.pnlTarget.Size = new System.Drawing.Size(133, 123);
            this.pnlTarget.TabIndex = 8;
            // 
            // FrmSuperModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 213);
            this.Controls.Add(this.pnlTarget);
            this.Controls.Add(this.pnlMachine);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSuperModel";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择模式";
            this.Load += new System.EventHandler(this.FrmSuperModel_Load);
            this.pnlMachine.ResumeLayout(false);
            this.pnlMachine.PerformLayout();
            this.pnlTarget.ResumeLayout(false);
            this.pnlTarget.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnOK;
        private System.Windows.Forms.RadioButton radOneTarget;
        private System.Windows.Forms.RadioButton radTwoTarget;
        private System.Windows.Forms.RadioButton rad2050;
        private System.Windows.Forms.RadioButton rad2400;
        private System.Windows.Forms.Panel pnlMachine;
        private System.Windows.Forms.Panel pnlTarget;
    }
}