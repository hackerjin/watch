namespace Skyray.UC
{
    partial class UCSysConfig
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.radHasSound = new System.Windows.Forms.RadioButton();
            this.gpSound = new Skyray.Controls.Grouper();
            this.radNoSound = new System.Windows.Forms.RadioButton();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.lblMenuFont = new Skyray.Controls.LabelW();
            this.comFontType = new Skyray.Controls.ComboBoxW();
            this.lblShape = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.txtFontStyle = new System.Windows.Forms.TextBox();
            this.lblFaceSize = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lblToolsSize = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.btnDefault = new Skyray.Controls.ButtonW();
            this.gpSound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // radHasSound
            // 
            this.radHasSound.AutoSize = true;
            this.radHasSound.Location = new System.Drawing.Point(20, 26);
            this.radHasSound.Name = "radHasSound";
            this.radHasSound.Size = new System.Drawing.Size(35, 16);
            this.radHasSound.TabIndex = 1;
            this.radHasSound.TabStop = true;
            this.radHasSound.Text = "有";
            this.radHasSound.UseVisualStyleBackColor = true;
            // 
            // gpSound
            // 
            this.gpSound.BackgroundColor = System.Drawing.Color.Transparent;
            this.gpSound.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.gpSound.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.gpSound.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.gpSound.BorderThickness = 1F;
            this.gpSound.BorderTopOnly = false;
            this.gpSound.Controls.Add(this.radNoSound);
            this.gpSound.Controls.Add(this.radHasSound);
            this.gpSound.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.gpSound.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.gpSound.GroupImage = null;
            this.gpSound.GroupTitle = "测试提示音";
            this.gpSound.HeaderRoundCorners = 4;
            this.gpSound.Location = new System.Drawing.Point(11, 11);
            this.gpSound.Name = "gpSound";
            this.gpSound.PaintGroupBox = false;
            this.gpSound.RoundCorners = 4;
            this.gpSound.ShadowColor = System.Drawing.Color.DarkGray;
            this.gpSound.ShadowControl = false;
            this.gpSound.ShadowThickness = 3;
            this.gpSound.Size = new System.Drawing.Size(469, 56);
            this.gpSound.TabIndex = 2;
            this.gpSound.TextLineSpace = 2;
            this.gpSound.TitleLeftSpace = 18;
            // 
            // radNoSound
            // 
            this.radNoSound.AutoSize = true;
            this.radNoSound.Location = new System.Drawing.Point(168, 26);
            this.radNoSound.Name = "radNoSound";
            this.radNoSound.Size = new System.Drawing.Size(35, 16);
            this.radNoSound.TabIndex = 2;
            this.radNoSound.TabStop = true;
            this.radNoSound.Text = "无";
            this.radNoSound.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(300, 273);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(403, 272);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMenuFont
            // 
            this.lblMenuFont.AutoSize = true;
            this.lblMenuFont.BackColor = System.Drawing.Color.Transparent;
            this.lblMenuFont.Location = new System.Drawing.Point(12, 84);
            this.lblMenuFont.Name = "lblMenuFont";
            this.lblMenuFont.Size = new System.Drawing.Size(41, 12);
            this.lblMenuFont.TabIndex = 6;
            this.lblMenuFont.Text = "字体：";
            // 
            // comFontType
            // 
            this.comFontType.AutoComplete = false;
            this.comFontType.AutoDropdown = false;
            this.comFontType.BackColorEven = System.Drawing.Color.White;
            this.comFontType.BackColorOdd = System.Drawing.Color.White;
            this.comFontType.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.comFontType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.comFontType.ColumnNames = "";
            this.comFontType.ColumnWidthDefault = 75;
            this.comFontType.ColumnWidths = "";
            this.comFontType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comFontType.FormattingEnabled = true;
            this.comFontType.LinkedColumnIndex = 0;
            this.comFontType.LinkedTextBox = null;
            this.comFontType.Location = new System.Drawing.Point(14, 99);
            this.comFontType.Name = "comFontType";
            this.comFontType.Size = new System.Drawing.Size(121, 22);
            this.comFontType.TabIndex = 7;
            // 
            // lblShape
            // 
            this.lblShape.AutoSize = true;
            this.lblShape.Location = new System.Drawing.Point(177, 84);
            this.lblShape.Name = "lblShape";
            this.lblShape.Size = new System.Drawing.Size(41, 12);
            this.lblShape.TabIndex = 8;
            this.lblShape.Text = "字形：";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(182, 126);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 88);
            this.listBox1.TabIndex = 9;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.Click += new System.EventHandler(this.listBox1_Click);
            // 
            // txtFontStyle
            // 
            this.txtFontStyle.Location = new System.Drawing.Point(182, 101);
            this.txtFontStyle.Name = "txtFontStyle";
            this.txtFontStyle.Size = new System.Drawing.Size(120, 21);
            this.txtFontStyle.TabIndex = 10;
            // 
            // lblFaceSize
            // 
            this.lblFaceSize.AutoSize = true;
            this.lblFaceSize.Location = new System.Drawing.Point(340, 84);
            this.lblFaceSize.Name = "lblFaceSize";
            this.lblFaceSize.Size = new System.Drawing.Size(47, 12);
            this.lblFaceSize.TabIndex = 11;
            this.lblFaceSize.Text = "字号：";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(342, 102);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(136, 21);
            this.numericUpDown1.TabIndex = 13;
            // 
            // lblToolsSize
            // 
            this.lblToolsSize.AutoSize = true;
            this.lblToolsSize.Location = new System.Drawing.Point(14, 234);
            this.lblToolsSize.Name = "lblToolsSize";
            this.lblToolsSize.Size = new System.Drawing.Size(101, 12);
            this.lblToolsSize.TabIndex = 14;
            this.lblToolsSize.Text = "工具栏图标大小：";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(121, 232);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown2.TabIndex = 15;
            // 
            // btnDefault
            // 
            this.btnDefault.bSilver = false;
            this.btnDefault.Location = new System.Drawing.Point(197, 272);
            this.btnDefault.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDefault.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDefault.TabIndex = 16;
            this.btnDefault.Text = "默认";
            this.btnDefault.ToFocused = false;
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // UCSysConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.lblToolsSize);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.lblFaceSize);
            this.Controls.Add(this.txtFontStyle);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lblShape);
            this.Controls.Add(this.comFontType);
            this.Controls.Add(this.lblMenuFont);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gpSound);
            this.Name = "UCSysConfig";
            this.Size = new System.Drawing.Size(489, 304);
            this.Load += new System.EventHandler(this.UCSysConfig_Load);
            this.gpSound.ResumeLayout(false);
            this.gpSound.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radHasSound;
        private Skyray.Controls.Grouper gpSound;
        private System.Windows.Forms.RadioButton radNoSound;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.LabelW lblMenuFont;
        private Skyray.Controls.ComboBoxW comFontType;
        private System.Windows.Forms.Label lblShape;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox txtFontStyle;
        private System.Windows.Forms.Label lblFaceSize;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label lblToolsSize;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private Skyray.Controls.ButtonW btnDefault;
    }
}
