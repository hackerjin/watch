namespace Skyray.UC
{
    partial class FrmSampleInfo
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
            this.grpSample = new Skyray.Controls.Grouper();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtDescription = new Skyray.Controls.TextBoxW();
            this.lblDescription = new Skyray.Controls.LabelW();
            this.buttonWOk = new Skyray.Controls.ButtonW();
            this.buttonWCancel = new Skyray.Controls.ButtonW();
            this.pnlLoss = new System.Windows.Forms.Panel();
            this.txtLoss = new Skyray.Controls.TextBoxW();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLoss = new Skyray.Controls.LabelW();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxElementNameChild = new System.Windows.Forms.ComboBox();
            this.lblSampleName = new Skyray.Controls.LabelW();
            this.txtSampleName = new Skyray.Controls.TextBoxW();
            this.lblShape = new Skyray.Controls.LabelW();
            this.comboBoxWShape = new Skyray.Controls.ComboBoxW();
            this.lblSupplier = new Skyray.Controls.LabelW();
            this.txtWeight = new Skyray.Controls.TextBoxW();
            this.comboBoxWSupplier = new Skyray.Controls.ComboBoxW();
            this.lblWeight = new Skyray.Controls.LabelW();
            this.grpSample.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlLoss.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSample
            // 
            this.grpSample.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpSample.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpSample.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpSample.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpSample.BorderThickness = 1F;
            this.grpSample.BorderTopOnly = false;
            this.grpSample.Controls.Add(this.panel3);
            this.grpSample.Controls.Add(this.pnlLoss);
            this.grpSample.Controls.Add(this.panel1);
            this.grpSample.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpSample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSample.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpSample.GroupImage = null;
            this.grpSample.GroupTitle = "";
            this.grpSample.HeaderRoundCorners = 4;
            this.grpSample.Location = new System.Drawing.Point(6, 6);
            this.grpSample.Name = "grpSample";
            this.grpSample.PaintGroupBox = false;
            this.grpSample.RoundCorners = 4;
            this.grpSample.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpSample.ShadowControl = false;
            this.grpSample.ShadowThickness = 3;
            this.grpSample.Size = new System.Drawing.Size(509, 247);
            this.grpSample.TabIndex = 10;
            this.grpSample.TextLineSpace = 2;
            this.grpSample.TitleLeftSpace = 18;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtDescription);
            this.panel3.Controls.Add(this.lblDescription);
            this.panel3.Controls.Add(this.buttonWOk);
            this.panel3.Controls.Add(this.buttonWCancel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 127);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(509, 120);
            this.panel3.TabIndex = 16;
            // 
            // txtDescription
            // 
            this.txtDescription.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtDescription.Location = new System.Drawing.Point(76, 3);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(377, 59);
            this.txtDescription.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtDescription.TabIndex = 9;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Location = new System.Drawing.Point(21, 9);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(29, 12);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "描述";
            // 
            // buttonWOk
            // 
            this.buttonWOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWOk.bSilver = false;
            this.buttonWOk.Location = new System.Drawing.Point(247, 84);
            this.buttonWOk.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWOk.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWOk.Name = "buttonWOk";
            this.buttonWOk.Size = new System.Drawing.Size(75, 23);
            this.buttonWOk.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWOk.TabIndex = 10;
            this.buttonWOk.Text = "确定";
            this.buttonWOk.ToFocused = false;
            this.buttonWOk.UseVisualStyleBackColor = true;
            this.buttonWOk.Click += new System.EventHandler(this.buttonWOk_Click);
            // 
            // buttonWCancel
            // 
            this.buttonWCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWCancel.bSilver = false;
            this.buttonWCancel.Location = new System.Drawing.Point(378, 84);
            this.buttonWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWCancel.Name = "buttonWCancel";
            this.buttonWCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWCancel.TabIndex = 11;
            this.buttonWCancel.Text = "取消";
            this.buttonWCancel.ToFocused = false;
            this.buttonWCancel.UseVisualStyleBackColor = true;
            this.buttonWCancel.Click += new System.EventHandler(this.buttonWCancel_Click);
            // 
            // pnlLoss
            // 
            this.pnlLoss.Controls.Add(this.txtLoss);
            this.pnlLoss.Controls.Add(this.label1);
            this.pnlLoss.Controls.Add(this.lblLoss);
            this.pnlLoss.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLoss.Location = new System.Drawing.Point(0, 94);
            this.pnlLoss.Name = "pnlLoss";
            this.pnlLoss.Size = new System.Drawing.Size(509, 33);
            this.pnlLoss.TabIndex = 15;
            // 
            // txtLoss
            // 
            this.txtLoss.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtLoss.Location = new System.Drawing.Point(76, 6);
            this.txtLoss.Name = "txtLoss";
            this.txtLoss.Size = new System.Drawing.Size(110, 21);
            this.txtLoss.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtLoss.TabIndex = 22;
            this.txtLoss.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLoss_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "%";
            // 
            // lblLoss
            // 
            this.lblLoss.AutoSize = true;
            this.lblLoss.BackColor = System.Drawing.Color.Transparent;
            this.lblLoss.Location = new System.Drawing.Point(21, 11);
            this.lblLoss.Name = "lblLoss";
            this.lblLoss.Size = new System.Drawing.Size(41, 12);
            this.lblLoss.TabIndex = 21;
            this.lblLoss.Text = "烧失量";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBoxElementNameChild);
            this.panel1.Controls.Add(this.lblSampleName);
            this.panel1.Controls.Add(this.txtSampleName);
            this.panel1.Controls.Add(this.lblShape);
            this.panel1.Controls.Add(this.comboBoxWShape);
            this.panel1.Controls.Add(this.lblSupplier);
            this.panel1.Controls.Add(this.txtWeight);
            this.panel1.Controls.Add(this.comboBoxWSupplier);
            this.panel1.Controls.Add(this.lblWeight);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(509, 94);
            this.panel1.TabIndex = 14;
            // 
            // comboBoxElementNameChild
            // 
            this.comboBoxElementNameChild.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxElementNameChild.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxElementNameChild.DisplayMember = "Name";
            this.comboBoxElementNameChild.FormattingEnabled = true;
            this.comboBoxElementNameChild.Location = new System.Drawing.Point(76, 30);
            this.comboBoxElementNameChild.Name = "comboBoxElementNameChild";
            this.comboBoxElementNameChild.Size = new System.Drawing.Size(156, 20);
            this.comboBoxElementNameChild.TabIndex = 13;
            this.comboBoxElementNameChild.ValueMember = "Id";
            this.comboBoxElementNameChild.Visible = false;
            // 
            // lblSampleName
            // 
            this.lblSampleName.AutoSize = true;
            this.lblSampleName.BackColor = System.Drawing.Color.Transparent;
            this.lblSampleName.Location = new System.Drawing.Point(21, 33);
            this.lblSampleName.Name = "lblSampleName";
            this.lblSampleName.Size = new System.Drawing.Size(53, 12);
            this.lblSampleName.TabIndex = 0;
            this.lblSampleName.Text = "样品名称";
            // 
            // txtSampleName
            // 
            this.txtSampleName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtSampleName.Location = new System.Drawing.Point(79, 30);
            this.txtSampleName.Name = "txtSampleName";
            this.txtSampleName.Size = new System.Drawing.Size(127, 21);
            this.txtSampleName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtSampleName.TabIndex = 1;
            // 
            // lblShape
            // 
            this.lblShape.AutoSize = true;
            this.lblShape.BackColor = System.Drawing.Color.Transparent;
            this.lblShape.Location = new System.Drawing.Point(265, 36);
            this.lblShape.Name = "lblShape";
            this.lblShape.Size = new System.Drawing.Size(29, 12);
            this.lblShape.TabIndex = 2;
            this.lblShape.Text = "形状";
            // 
            // comboBoxWShape
            // 
            this.comboBoxWShape.AutoComplete = false;
            this.comboBoxWShape.AutoDropdown = false;
            this.comboBoxWShape.BackColorEven = System.Drawing.Color.White;
            this.comboBoxWShape.BackColorOdd = System.Drawing.Color.White;
            this.comboBoxWShape.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.comboBoxWShape.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.comboBoxWShape.ColumnNames = "";
            this.comboBoxWShape.ColumnWidthDefault = 75;
            this.comboBoxWShape.ColumnWidths = "";
            this.comboBoxWShape.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxWShape.FormattingEnabled = true;
            this.comboBoxWShape.LinkedColumnIndex = 0;
            this.comboBoxWShape.LinkedTextBox = null;
            this.comboBoxWShape.Location = new System.Drawing.Point(307, 30);
            this.comboBoxWShape.Name = "comboBoxWShape";
            this.comboBoxWShape.Size = new System.Drawing.Size(135, 22);
            this.comboBoxWShape.TabIndex = 3;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.BackColor = System.Drawing.Color.Transparent;
            this.lblSupplier.Location = new System.Drawing.Point(21, 72);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(41, 12);
            this.lblSupplier.TabIndex = 4;
            this.lblSupplier.Text = "供应商";
            // 
            // txtWeight
            // 
            this.txtWeight.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtWeight.Location = new System.Drawing.Point(307, 66);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(135, 21);
            this.txtWeight.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtWeight.TabIndex = 7;
            this.txtWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWeight_KeyPress);
            // 
            // comboBoxWSupplier
            // 
            this.comboBoxWSupplier.AutoComplete = false;
            this.comboBoxWSupplier.AutoDropdown = false;
            this.comboBoxWSupplier.BackColorEven = System.Drawing.Color.White;
            this.comboBoxWSupplier.BackColorOdd = System.Drawing.Color.White;
            this.comboBoxWSupplier.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.comboBoxWSupplier.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.comboBoxWSupplier.ColumnNames = "";
            this.comboBoxWSupplier.ColumnWidthDefault = 75;
            this.comboBoxWSupplier.ColumnWidths = "";
            this.comboBoxWSupplier.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxWSupplier.FormattingEnabled = true;
            this.comboBoxWSupplier.LinkedColumnIndex = 0;
            this.comboBoxWSupplier.LinkedTextBox = null;
            this.comboBoxWSupplier.Location = new System.Drawing.Point(76, 66);
            this.comboBoxWSupplier.Name = "comboBoxWSupplier";
            this.comboBoxWSupplier.Size = new System.Drawing.Size(156, 22);
            this.comboBoxWSupplier.TabIndex = 5;
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.BackColor = System.Drawing.Color.Transparent;
            this.lblWeight.Location = new System.Drawing.Point(265, 72);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(29, 12);
            this.lblWeight.TabIndex = 6;
            this.lblWeight.Text = "重量";
            // 
            // FrmSampleInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(521, 259);
            this.Controls.Add(this.grpSample);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSampleInfo";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "样品信息";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmSampleInfo_Load);
            this.grpSample.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pnlLoss.ResumeLayout(false);
            this.pnlLoss.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.Grouper grpSample;
        private Skyray.Controls.TextBoxW txtDescription;
        private Skyray.Controls.LabelW lblDescription;
        private Skyray.Controls.TextBoxW txtWeight;
        private Skyray.Controls.LabelW lblWeight;
        private Skyray.Controls.ComboBoxW comboBoxWSupplier;
        private Skyray.Controls.LabelW lblSupplier;
        private Skyray.Controls.ComboBoxW comboBoxWShape;
        private Skyray.Controls.LabelW lblShape;
        private Skyray.Controls.TextBoxW txtSampleName;
        private Skyray.Controls.LabelW lblSampleName;
        private Skyray.Controls.ButtonW buttonWCancel;
        private Skyray.Controls.ButtonW buttonWOk;
        private System.Windows.Forms.ComboBox comboBoxElementNameChild;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlLoss;
        private Skyray.Controls.TextBoxW txtLoss;
        private System.Windows.Forms.Label label1;
        private Skyray.Controls.LabelW lblLoss;
    }
}