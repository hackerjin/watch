namespace Skyray.UC
{
    partial class FrmStyle
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.numOKFontSize = new Skyray.Controls.NumricUpDownW();
            this.labelW5_J = new Skyray.Controls.LabelW();
            this.lblOkBackColor = new System.Windows.Forms.Label();
            this.label3_J = new System.Windows.Forms.Label();
            this.lblOkForeColor = new System.Windows.Forms.Label();
            this.labelW3_J = new Skyray.Controls.LabelW();
            this.panelOK = new System.Windows.Forms.Panel();
            this.lblOK = new Skyray.Controls.LabelW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.numNGFontSize = new Skyray.Controls.NumricUpDownW();
            this.labelW6_J = new Skyray.Controls.LabelW();
            this.lblNGBackColor = new System.Windows.Forms.Label();
            this.label4_J = new System.Windows.Forms.Label();
            this.lblNGForeColor = new System.Windows.Forms.Label();
            this.labelW4_J = new Skyray.Controls.LabelW();
            this.panelNG = new System.Windows.Forms.Panel();
            this.lblNG = new Skyray.Controls.LabelW();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOKFontSize)).BeginInit();
            this.panelOK.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNGFontSize)).BeginInit();
            this.panelNG.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(8, 8);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnApplication);
            this.splitContainer1.Panel1.Controls.Add(this.numOKFontSize);
            this.splitContainer1.Panel1.Controls.Add(this.labelW5_J);
            this.splitContainer1.Panel1.Controls.Add(this.lblOkBackColor);
            this.splitContainer1.Panel1.Controls.Add(this.label3_J);
            this.splitContainer1.Panel1.Controls.Add(this.lblOkForeColor);
            this.splitContainer1.Panel1.Controls.Add(this.labelW3_J);
            this.splitContainer1.Panel1.Controls.Add(this.panelOK);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.numNGFontSize);
            this.splitContainer1.Panel2.Controls.Add(this.labelW6_J);
            this.splitContainer1.Panel2.Controls.Add(this.lblNGBackColor);
            this.splitContainer1.Panel2.Controls.Add(this.label4_J);
            this.splitContainer1.Panel2.Controls.Add(this.lblNGForeColor);
            this.splitContainer1.Panel2.Controls.Add(this.labelW4_J);
            this.splitContainer1.Panel2.Controls.Add(this.panelNG);
            this.splitContainer1.Size = new System.Drawing.Size(493, 388);
            this.splitContainer1.SplitterDistance = 246;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(154, 362);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(75, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 7;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // numOKFontSize
            // 
            this.numOKFontSize.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numOKFontSize.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numOKFontSize.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numOKFontSize.Location = new System.Drawing.Point(104, 317);
            this.numOKFontSize.Name = "numOKFontSize";
            this.numOKFontSize.Size = new System.Drawing.Size(65, 21);
            this.numOKFontSize.TabIndex = 6;
            this.numOKFontSize.Value = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.numOKFontSize.ValueChanged += new System.EventHandler(this.numOKFontSize_ValueChanged);
            // 
            // labelW5_J
            // 
            this.labelW5_J.AutoSize = true;
            this.labelW5_J.BackColor = System.Drawing.Color.Transparent;
            this.labelW5_J.Location = new System.Drawing.Point(12, 319);
            this.labelW5_J.Name = "labelW5_J";
            this.labelW5_J.Size = new System.Drawing.Size(53, 12);
            this.labelW5_J.TabIndex = 5;
            this.labelW5_J.Text = Skyray.EDX.Common.Info.FontSize;
            // 
            // lblOkBackColor
            // 
            this.lblOkBackColor.BackColor = System.Drawing.Color.Green;
            this.lblOkBackColor.Location = new System.Drawing.Point(102, 286);
            this.lblOkBackColor.Name = "lblOkBackColor";
            this.lblOkBackColor.Size = new System.Drawing.Size(67, 19);
            this.lblOkBackColor.TabIndex = 4;
            this.lblOkBackColor.Click += new System.EventHandler(this.lblOkBackColor_Click);
            // 
            // label3_J
            // 
            this.label3_J.AutoSize = true;
            this.label3_J.Location = new System.Drawing.Point(12, 287);
            this.label3_J.Name = "label3_J";
            this.label3_J.Size = new System.Drawing.Size(53, 12);
            this.label3_J.TabIndex = 3;
            this.label3_J.Text = Skyray.EDX.Common.Info.BackColor;
            // 
            // lblOkForeColor
            // 
            this.lblOkForeColor.BackColor = System.Drawing.Color.Black;
            this.lblOkForeColor.Location = new System.Drawing.Point(102, 256);
            this.lblOkForeColor.Name = "lblOkForeColor";
            this.lblOkForeColor.Size = new System.Drawing.Size(67, 19);
            this.lblOkForeColor.TabIndex = 2;
            this.lblOkForeColor.Click += new System.EventHandler(this.lblOkForeColor_Click);
            // 
            // labelW3_J
            // 
            this.labelW3_J.AutoSize = true;
            this.labelW3_J.BackColor = System.Drawing.Color.Transparent;
            this.labelW3_J.Location = new System.Drawing.Point(12, 259);
            this.labelW3_J.Name = "labelW3_J";
            this.labelW3_J.Size = new System.Drawing.Size(53, 12);
            this.labelW3_J.TabIndex = 1;
            this.labelW3_J.Text = Skyray.EDX.Common.Info.ForeColor;
            // 
            // panelOK
            // 
            this.panelOK.BackColor = System.Drawing.Color.Green;
            this.panelOK.Controls.Add(this.lblOK);
            this.panelOK.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOK.Location = new System.Drawing.Point(0, 0);
            this.panelOK.Name = "panelOK";
            this.panelOK.Size = new System.Drawing.Size(246, 245);
            this.panelOK.TabIndex = 0;
            // 
            // lblOK
            // 
            this.lblOK.BackColor = System.Drawing.Color.Transparent;
            this.lblOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOK.Font = new System.Drawing.Font("宋体", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOK.Location = new System.Drawing.Point(0, 0);
            this.lblOK.Name = "lblOK";
            this.lblOK.Size = new System.Drawing.Size(246, 245);
            this.lblOK.TabIndex = 0;
            this.lblOK.Text = "OK";
            this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(13, 362);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // numNGFontSize
            // 
            this.numNGFontSize.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numNGFontSize.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numNGFontSize.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numNGFontSize.Location = new System.Drawing.Point(97, 317);
            this.numNGFontSize.Name = "numNGFontSize";
            this.numNGFontSize.Size = new System.Drawing.Size(70, 21);
            this.numNGFontSize.TabIndex = 7;
            this.numNGFontSize.Value = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.numNGFontSize.ValueChanged += new System.EventHandler(this.numNGFontSize_ValueChanged);
            // 
            // labelW6_J
            // 
            this.labelW6_J.AutoSize = true;
            this.labelW6_J.BackColor = System.Drawing.Color.Transparent;
            this.labelW6_J.Location = new System.Drawing.Point(11, 319);
            this.labelW6_J.Name = "labelW6_J";
            this.labelW6_J.Size = new System.Drawing.Size(53, 12);
            this.labelW6_J.TabIndex = 6;
            this.labelW6_J.Text = Skyray.EDX.Common.Info.FontSize;
            // 
            // lblNGBackColor
            // 
            this.lblNGBackColor.BackColor = System.Drawing.Color.Red;
            this.lblNGBackColor.Location = new System.Drawing.Point(95, 287);
            this.lblNGBackColor.Name = "lblNGBackColor";
            this.lblNGBackColor.Size = new System.Drawing.Size(72, 19);
            this.lblNGBackColor.TabIndex = 5;
            this.lblNGBackColor.Click += new System.EventHandler(this.lblNGBackColor_Click);
            // 
            // label4_J
            // 
            this.label4_J.AutoSize = true;
            this.label4_J.Location = new System.Drawing.Point(11, 287);
            this.label4_J.Name = "label4_J";
            this.label4_J.Size = new System.Drawing.Size(53, 12);
            this.label4_J.TabIndex = 4;
            this.label4_J.Text = Skyray.EDX.Common.Info.BackColor;
            // 
            // lblNGForeColor
            // 
            this.lblNGForeColor.BackColor = System.Drawing.Color.Black;
            this.lblNGForeColor.Location = new System.Drawing.Point(95, 259);
            this.lblNGForeColor.Name = "lblNGForeColor";
            this.lblNGForeColor.Size = new System.Drawing.Size(72, 19);
            this.lblNGForeColor.TabIndex = 3;
            this.lblNGForeColor.Click += new System.EventHandler(this.lblNGForeColor_Click);
            // 
            // labelW4_J
            // 
            this.labelW4_J.AutoSize = true;
            this.labelW4_J.BackColor = System.Drawing.Color.Transparent;
            this.labelW4_J.Location = new System.Drawing.Point(8, 259);
            this.labelW4_J.Name = "labelW4_J";
            this.labelW4_J.Size = new System.Drawing.Size(53, 12);
            this.labelW4_J.TabIndex = 2;
            this.labelW4_J.Text = Skyray.EDX.Common.Info.ForeColor;
            // 
            // panelNG
            // 
            this.panelNG.BackColor = System.Drawing.Color.Red;
            this.panelNG.Controls.Add(this.lblNG);
            this.panelNG.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNG.Location = new System.Drawing.Point(0, 0);
            this.panelNG.Name = "panelNG";
            this.panelNG.Size = new System.Drawing.Size(243, 245);
            this.panelNG.TabIndex = 0;
            // 
            // lblNG
            // 
            this.lblNG.BackColor = System.Drawing.Color.Transparent;
            this.lblNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNG.Font = new System.Drawing.Font("宋体", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNG.Location = new System.Drawing.Point(0, 0);
            this.lblNG.Name = "lblNG";
            this.lblNG.Size = new System.Drawing.Size(243, 245);
            this.lblNG.TabIndex = 0;
            this.lblNG.Text = "NG";
            this.lblNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmStyle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmStyle";
            this.Size = new System.Drawing.Size(509, 404);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numOKFontSize)).EndInit();
            this.panelOK.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numNGFontSize)).EndInit();
            this.panelNG.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelOK;
        private Skyray.Controls.LabelW lblOK;
        private System.Windows.Forms.Panel panelNG;
        private Skyray.Controls.LabelW lblNG;
        private Skyray.Controls.LabelW labelW3_J;
        private Skyray.Controls.LabelW labelW4_J;
        private System.Windows.Forms.Label label3_J;
        private System.Windows.Forms.Label lblOkForeColor;
        private System.Windows.Forms.Label label4_J;
        private System.Windows.Forms.Label lblNGForeColor;
        private System.Windows.Forms.Label lblOkBackColor;
        private System.Windows.Forms.Label lblNGBackColor;
        private Skyray.Controls.LabelW labelW5_J;
        private Skyray.Controls.LabelW labelW6_J;
        private Skyray.Controls.NumricUpDownW numOKFontSize;
        private Skyray.Controls.NumricUpDownW numNGFontSize;
        private Skyray.Controls.ButtonW btnApplication;
        private Skyray.Controls.ButtonW btnOK;

    }
}