namespace Skyray.UC
{
    partial class FrmHotSet
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
            this.grouper1 = new Skyray.Controls.Grouper();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.cmbModifier = new Skyray.Controls.ComboBoxW();
            this.btnSet = new Skyray.Controls.ButtonW();
            this.cmbKey = new Skyray.Controls.ComboBoxW();
            this.grouper1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grouper1
            // 
            this.grouper1.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouper1.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouper1.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouper1.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grouper1.BorderThickness = 1F;
            this.grouper1.BorderTopOnly = false;
            this.grouper1.Controls.Add(this.btnOK);
            this.grouper1.Controls.Add(this.cmbModifier);
            this.grouper1.Controls.Add(this.btnSet);
            this.grouper1.Controls.Add(this.cmbKey);
            this.grouper1.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grouper1.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grouper1.GroupImage = null;
            this.grouper1.GroupTitle = "热键设置";
            this.grouper1.HeaderRoundCorners = 4;
            this.grouper1.Location = new System.Drawing.Point(33, 21);
            this.grouper1.Name = "grouper1";
            this.grouper1.PaintGroupBox = false;
            this.grouper1.RoundCorners = 4;
            this.grouper1.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper1.ShadowControl = false;
            this.grouper1.ShadowThickness = 3;
            this.grouper1.Size = new System.Drawing.Size(192, 138);
            this.grouper1.TabIndex = 7;
            this.grouper1.TextLineSpace = 2;
            this.grouper1.TitleLeftSpace = 18;
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(104, 103);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbModifier
            // 
            this.cmbModifier.AutoComplete = false;
            this.cmbModifier.AutoDropdown = false;
            this.cmbModifier.BackColorEven = System.Drawing.Color.White;
            this.cmbModifier.BackColorOdd = System.Drawing.Color.White;
            this.cmbModifier.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cmbModifier.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cmbModifier.ColumnNames = "";
            this.cmbModifier.ColumnWidthDefault = 75;
            this.cmbModifier.ColumnWidths = "";
            this.cmbModifier.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbModifier.FormattingEnabled = true;
            this.cmbModifier.LinkedColumnIndex = 0;
            this.cmbModifier.LinkedTextBox = null;
            this.cmbModifier.Location = new System.Drawing.Point(39, 32);
            this.cmbModifier.Name = "cmbModifier";
            this.cmbModifier.Size = new System.Drawing.Size(121, 22);
            this.cmbModifier.TabIndex = 1;
            // 
            // btnSet
            // 
            this.btnSet.bSilver = false;
            this.btnSet.Location = new System.Drawing.Point(14, 103);
            this.btnSet.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSet.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSet.TabIndex = 0;
            this.btnSet.Text = "设置";
            this.btnSet.ToFocused = false;
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // cmbKey
            // 
            this.cmbKey.AutoComplete = false;
            this.cmbKey.AutoDropdown = false;
            this.cmbKey.BackColorEven = System.Drawing.Color.White;
            this.cmbKey.BackColorOdd = System.Drawing.Color.White;
            this.cmbKey.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cmbKey.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cmbKey.ColumnNames = "";
            this.cmbKey.ColumnWidthDefault = 75;
            this.cmbKey.ColumnWidths = "";
            this.cmbKey.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbKey.FormattingEnabled = true;
            this.cmbKey.LinkedColumnIndex = 0;
            this.cmbKey.LinkedTextBox = null;
            this.cmbKey.Location = new System.Drawing.Point(39, 60);
            this.cmbKey.Name = "cmbKey";
            this.cmbKey.Size = new System.Drawing.Size(121, 22);
            this.cmbKey.TabIndex = 2;
            // 
            // FrmHotSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 170);
            this.Controls.Add(this.grouper1);
            this.Name = "FrmHotSet";
            this.Text = "热键";
            this.TopMost = true;
            this.grouper1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.Grouper grouper1;
        private Skyray.Controls.ComboBoxW cmbModifier;
        private Skyray.Controls.ButtonW btnSet;
        private Skyray.Controls.ComboBoxW cmbKey;
        private Skyray.Controls.ButtonW btnOK;
    }
}