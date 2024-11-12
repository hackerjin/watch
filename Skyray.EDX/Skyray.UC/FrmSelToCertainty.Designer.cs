namespace Skyray.UC
{
    partial class FrmSelToCertainty
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
            this.lblElemName = new Skyray.Controls.LabelW();
            this.txtSampleUncertainty = new Skyray.Controls.TextBoxW();
            this.lblSmpUncertainty = new Skyray.Controls.LabelW();
            this.comboBoxWElems = new Skyray.Controls.ComboBoxW();
            this.comboBoxWSimSmps = new Skyray.Controls.ComboBoxW();
            this.lblSimilarSmp = new Skyray.Controls.LabelW();
            this.txtStandUncertainty = new Skyray.Controls.TextBoxW();
            this.lblStandUncertainty = new Skyray.Controls.LabelW();
            this.btnClickMe = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // lblElemName
            // 
            this.lblElemName.AutoSize = true;
            this.lblElemName.BackColor = System.Drawing.Color.Transparent;
            this.lblElemName.Location = new System.Drawing.Point(29, 9);
            this.lblElemName.Name = "lblElemName";
            this.lblElemName.Size = new System.Drawing.Size(65, 12);
            this.lblElemName.TabIndex = 1;
            this.lblElemName.Text = "元素全名称";
            // 
            // txtSampleUncertainty
            // 
            this.txtSampleUncertainty.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtSampleUncertainty.Enabled = false;
            this.txtSampleUncertainty.Location = new System.Drawing.Point(31, 84);
            this.txtSampleUncertainty.Name = "txtSampleUncertainty";
            this.txtSampleUncertainty.Size = new System.Drawing.Size(91, 21);
            this.txtSampleUncertainty.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtSampleUncertainty.TabIndex = 2;
            // 
            // lblSmpUncertainty
            // 
            this.lblSmpUncertainty.AutoSize = true;
            this.lblSmpUncertainty.BackColor = System.Drawing.Color.Transparent;
            this.lblSmpUncertainty.Location = new System.Drawing.Point(29, 69);
            this.lblSmpUncertainty.Name = "lblSmpUncertainty";
            this.lblSmpUncertainty.Size = new System.Drawing.Size(101, 12);
            this.lblSmpUncertainty.TabIndex = 3;
            this.lblSmpUncertainty.Text = "未知样不确定度：";
            // 
            // comboBoxWElems
            // 
            this.comboBoxWElems.AutoComplete = false;
            this.comboBoxWElems.AutoDropdown = false;
            this.comboBoxWElems.BackColorEven = System.Drawing.Color.White;
            this.comboBoxWElems.BackColorOdd = System.Drawing.Color.White;
            this.comboBoxWElems.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.comboBoxWElems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.comboBoxWElems.ColumnNames = "";
            this.comboBoxWElems.ColumnWidthDefault = 75;
            this.comboBoxWElems.ColumnWidths = "";
            this.comboBoxWElems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxWElems.FormattingEnabled = true;
            this.comboBoxWElems.LinkedColumnIndex = 0;
            this.comboBoxWElems.LinkedTextBox = null;
            this.comboBoxWElems.Location = new System.Drawing.Point(31, 24);
            this.comboBoxWElems.Name = "comboBoxWElems";
            this.comboBoxWElems.Size = new System.Drawing.Size(107, 22);
            this.comboBoxWElems.TabIndex = 91;
            this.comboBoxWElems.SelectedValueChanged += new System.EventHandler(this.comboBoxWElems_SelectedValueChanged);
            // 
            // comboBoxWSimSmps
            // 
            this.comboBoxWSimSmps.AutoComplete = false;
            this.comboBoxWSimSmps.AutoDropdown = false;
            this.comboBoxWSimSmps.BackColorEven = System.Drawing.Color.White;
            this.comboBoxWSimSmps.BackColorOdd = System.Drawing.Color.White;
            this.comboBoxWSimSmps.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.comboBoxWSimSmps.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.comboBoxWSimSmps.ColumnNames = "";
            this.comboBoxWSimSmps.ColumnWidthDefault = 75;
            this.comboBoxWSimSmps.ColumnWidths = "";
            this.comboBoxWSimSmps.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxWSimSmps.FormattingEnabled = true;
            this.comboBoxWSimSmps.LinkedColumnIndex = 0;
            this.comboBoxWSimSmps.LinkedTextBox = null;
            this.comboBoxWSimSmps.Location = new System.Drawing.Point(15, 158);
            this.comboBoxWSimSmps.Name = "comboBoxWSimSmps";
            this.comboBoxWSimSmps.Size = new System.Drawing.Size(107, 22);
            this.comboBoxWSimSmps.TabIndex = 93;
            this.comboBoxWSimSmps.Visible = false;
            this.comboBoxWSimSmps.SelectedValueChanged += new System.EventHandler(this.comboBoxWSimSmps_SelectedValueChanged);
            // 
            // lblSimilarSmp
            // 
            this.lblSimilarSmp.AutoSize = true;
            this.lblSimilarSmp.BackColor = System.Drawing.Color.Transparent;
            this.lblSimilarSmp.Location = new System.Drawing.Point(17, 143);
            this.lblSimilarSmp.Name = "lblSimilarSmp";
            this.lblSimilarSmp.Size = new System.Drawing.Size(89, 12);
            this.lblSimilarSmp.TabIndex = 92;
            this.lblSimilarSmp.Text = "类似标样名称：";
            this.lblSimilarSmp.Visible = false;
            // 
            // txtStandUncertainty
            // 
            this.txtStandUncertainty.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtStandUncertainty.Location = new System.Drawing.Point(139, 158);
            this.txtStandUncertainty.Name = "txtStandUncertainty";
            this.txtStandUncertainty.Size = new System.Drawing.Size(97, 21);
            this.txtStandUncertainty.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtStandUncertainty.TabIndex = 94;
            this.txtStandUncertainty.Visible = false;
            // 
            // lblStandUncertainty
            // 
            this.lblStandUncertainty.AutoSize = true;
            this.lblStandUncertainty.BackColor = System.Drawing.Color.Transparent;
            this.lblStandUncertainty.Location = new System.Drawing.Point(136, 143);
            this.lblStandUncertainty.Name = "lblStandUncertainty";
            this.lblStandUncertainty.Size = new System.Drawing.Size(89, 12);
            this.lblStandUncertainty.TabIndex = 95;
            this.lblStandUncertainty.Text = "标样不确定度：";
            this.lblStandUncertainty.Visible = false;
            // 
            // btnClickMe
            // 
            this.btnClickMe.bSilver = false;
            this.btnClickMe.Location = new System.Drawing.Point(122, 82);
            this.btnClickMe.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClickMe.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClickMe.Name = "btnClickMe";
            this.btnClickMe.Size = new System.Drawing.Size(28, 23);
            this.btnClickMe.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClickMe.TabIndex = 96;
            this.btnClickMe.Text = "...";
            this.btnClickMe.ToFocused = false;
            this.btnClickMe.UseVisualStyleBackColor = true;
            this.btnClickMe.Click += new System.EventHandler(this.btnClickMe_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(139, 188);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "打印";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(31, 188);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "保存报告";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmSelToCertainty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(261, 223);
            this.Controls.Add(this.btnClickMe);
            this.Controls.Add(this.txtStandUncertainty);
            this.Controls.Add(this.comboBoxWSimSmps);
            this.Controls.Add(this.comboBoxWElems);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblSmpUncertainty);
            this.Controls.Add(this.txtSampleUncertainty);
            this.Controls.Add(this.lblElemName);
            this.Controls.Add(this.lblSimilarSmp);
            this.Controls.Add(this.lblStandUncertainty);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelToCertainty";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择参数";
            this.Load += new System.EventHandler(this.FrmSelToCertainty_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblElemName;
        private Skyray.Controls.TextBoxW txtSampleUncertainty;
        private Skyray.Controls.LabelW lblSmpUncertainty;
        private Skyray.Controls.ComboBoxW comboBoxWElems;
        private Skyray.Controls.ComboBoxW comboBoxWSimSmps;
        private Skyray.Controls.LabelW lblSimilarSmp;
        private Skyray.Controls.TextBoxW txtStandUncertainty;
        private Skyray.Controls.LabelW lblStandUncertainty;
        private Skyray.Controls.ButtonW btnClickMe;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnOK;
    }
}