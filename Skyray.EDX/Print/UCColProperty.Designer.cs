namespace Skyray.Print
{
    partial class UCColProperty
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
            this.groupColSet = new Skyray.Controls.Grouper();
            this.chkHideCol = new Skyray.Controls.CheckBoxW();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lblColWidth = new Skyray.Controls.LabelW();
            this.lblColDisplayName = new Skyray.Controls.LabelW();
            this.textBoxW2 = new Skyray.Controls.TextBoxW();
            this.lblColName = new Skyray.Controls.LabelW();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblHeaderFont = new Skyray.Controls.LabelW();
            this.lblColFont = new Skyray.Controls.LabelW();
            this.lblExpression = new Skyray.Controls.LabelW();
            this.textBoxW1 = new Skyray.Controls.TextBoxW();
            this.lblExceptionFont = new Skyray.Controls.LabelW();
            this.fontPicker3 = new Skyray.Controls.FontPicker();
            this.fontPicker2 = new Skyray.Controls.FontPicker();
            this.fontPicker1 = new Skyray.Controls.FontPicker();
            this.groupColSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupColSet
            // 
            this.groupColSet.BackgroundColor = System.Drawing.Color.Transparent;
            this.groupColSet.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.groupColSet.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.groupColSet.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.groupColSet.BorderThickness = 1F;
            this.groupColSet.BorderTopOnly = false;
            this.groupColSet.Controls.Add(this.chkHideCol);
            this.groupColSet.Controls.Add(this.numericUpDown1);
            this.groupColSet.Controls.Add(this.lblColWidth);
            this.groupColSet.Controls.Add(this.lblColDisplayName);
            this.groupColSet.Controls.Add(this.textBoxW2);
            this.groupColSet.Controls.Add(this.lblColName);
            this.groupColSet.Controls.Add(this.comboBox1);
            this.groupColSet.Controls.Add(this.lblHeaderFont);
            this.groupColSet.Controls.Add(this.lblColFont);
            this.groupColSet.Controls.Add(this.lblExpression);
            this.groupColSet.Controls.Add(this.textBoxW1);
            this.groupColSet.Controls.Add(this.lblExceptionFont);
            this.groupColSet.Controls.Add(this.fontPicker3);
            this.groupColSet.Controls.Add(this.fontPicker2);
            this.groupColSet.Controls.Add(this.fontPicker1);
            this.groupColSet.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.groupColSet.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.groupColSet.GroupImage = null;
            this.groupColSet.GroupTitle = "列设置";
            this.groupColSet.HeaderRoundCorners = 4;
            this.groupColSet.Location = new System.Drawing.Point(3, 3);
            this.groupColSet.Name = "groupColSet";
            this.groupColSet.PaintGroupBox = false;
            this.groupColSet.RoundCorners = 4;
            this.groupColSet.ShadowColor = System.Drawing.Color.DarkGray;
            this.groupColSet.ShadowControl = false;
            this.groupColSet.ShadowThickness = 3;
            this.groupColSet.Size = new System.Drawing.Size(210, 269);
            this.groupColSet.TabIndex = 40;
            this.groupColSet.TextLineSpace = 2;
            this.groupColSet.TitleLeftSpace = 18;
            // 
            // chkHideCol
            // 
            this.chkHideCol.AutoSize = true;
            this.chkHideCol.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkHideCol.Location = new System.Drawing.Point(130, 76);
            this.chkHideCol.Name = "chkHideCol";
            this.chkHideCol.Size = new System.Drawing.Size(72, 16);
            this.chkHideCol.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkHideCol.TabIndex = 44;
            this.chkHideCol.Text = "隐藏本列";
            this.chkHideCol.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(72, 75);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(52, 21);
            this.numericUpDown1.TabIndex = 43;
            // 
            // lblColWidth
            // 
            this.lblColWidth.AutoSize = true;
            this.lblColWidth.BackColor = System.Drawing.Color.Transparent;
            this.lblColWidth.Location = new System.Drawing.Point(12, 79);
            this.lblColWidth.Name = "lblColWidth";
            this.lblColWidth.Size = new System.Drawing.Size(29, 12);
            this.lblColWidth.TabIndex = 42;
            this.lblColWidth.Text = "列宽";
            // 
            // lblColDisplayName
            // 
            this.lblColDisplayName.AutoSize = true;
            this.lblColDisplayName.BackColor = System.Drawing.Color.Transparent;
            this.lblColDisplayName.Location = new System.Drawing.Point(13, 49);
            this.lblColDisplayName.Name = "lblColDisplayName";
            this.lblColDisplayName.Size = new System.Drawing.Size(53, 12);
            this.lblColDisplayName.TabIndex = 39;
            this.lblColDisplayName.Text = "显示名称";
            // 
            // textBoxW2
            // 
            this.textBoxW2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textBoxW2.Location = new System.Drawing.Point(72, 45);
            this.textBoxW2.Name = "textBoxW2";
            this.textBoxW2.Size = new System.Drawing.Size(124, 21);
            this.textBoxW2.Style = Skyray.Controls.Style.Office2007Blue;
            this.textBoxW2.TabIndex = 40;
            // 
            // lblColName
            // 
            this.lblColName.AutoSize = true;
            this.lblColName.BackColor = System.Drawing.Color.Transparent;
            this.lblColName.Location = new System.Drawing.Point(12, 25);
            this.lblColName.Name = "lblColName";
            this.lblColName.Size = new System.Drawing.Size(29, 12);
            this.lblColName.TabIndex = 32;
            this.lblColName.Text = "列名";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(72, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(124, 20);
            this.comboBox1.TabIndex = 38;
            // 
            // lblHeaderFont
            // 
            this.lblHeaderFont.AutoSize = true;
            this.lblHeaderFont.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderFont.Location = new System.Drawing.Point(12, 180);
            this.lblHeaderFont.Name = "lblHeaderFont";
            this.lblHeaderFont.Size = new System.Drawing.Size(53, 12);
            this.lblHeaderFont.TabIndex = 33;
            this.lblHeaderFont.Text = "表头字体";
            // 
            // lblColFont
            // 
            this.lblColFont.AutoSize = true;
            this.lblColFont.BackColor = System.Drawing.Color.Transparent;
            this.lblColFont.Location = new System.Drawing.Point(12, 135);
            this.lblColFont.Name = "lblColFont";
            this.lblColFont.Size = new System.Drawing.Size(29, 12);
            this.lblColFont.TabIndex = 33;
            this.lblColFont.Text = "字体";
            // 
            // lblExpression
            // 
            this.lblExpression.AutoSize = true;
            this.lblExpression.BackColor = System.Drawing.Color.Transparent;
            this.lblExpression.Location = new System.Drawing.Point(12, 109);
            this.lblExpression.Name = "lblExpression";
            this.lblExpression.Size = new System.Drawing.Size(41, 12);
            this.lblExpression.TabIndex = 33;
            this.lblExpression.Text = "表达式";
            // 
            // textBoxW1
            // 
            this.textBoxW1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textBoxW1.Location = new System.Drawing.Point(72, 105);
            this.textBoxW1.Name = "textBoxW1";
            this.textBoxW1.Size = new System.Drawing.Size(124, 21);
            this.textBoxW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.textBoxW1.TabIndex = 35;
            // 
            // lblExceptionFont
            // 
            this.lblExceptionFont.AutoSize = true;
            this.lblExceptionFont.BackColor = System.Drawing.Color.Transparent;
            this.lblExceptionFont.Location = new System.Drawing.Point(12, 225);
            this.lblExceptionFont.Name = "lblExceptionFont";
            this.lblExceptionFont.Size = new System.Drawing.Size(53, 12);
            this.lblExceptionFont.TabIndex = 33;
            this.lblExceptionFont.Text = "异常字体";
            // 
            // fontPicker3
            // 
            this.fontPicker3.BackColor = System.Drawing.SystemColors.Window;
            this.fontPicker3.Context = null;
            this.fontPicker3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fontPicker3.Location = new System.Drawing.Point(12, 196);
            this.fontPicker3.Name = "fontPicker3";
            this.fontPicker3.ReadOnly = false;
            this.fontPicker3.Size = new System.Drawing.Size(184, 21);
            this.fontPicker3.TabIndex = 34;
            this.fontPicker3.Text = "Microsoft Sans Serif, 8.25pt";
            this.fontPicker3.Value = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            // 
            // fontPicker2
            // 
            this.fontPicker2.BackColor = System.Drawing.SystemColors.Window;
            this.fontPicker2.Context = null;
            this.fontPicker2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fontPicker2.Location = new System.Drawing.Point(12, 240);
            this.fontPicker2.Name = "fontPicker2";
            this.fontPicker2.ReadOnly = false;
            this.fontPicker2.Size = new System.Drawing.Size(184, 21);
            this.fontPicker2.TabIndex = 34;
            this.fontPicker2.Text = "Microsoft Sans Serif, 8.25pt";
            this.fontPicker2.Value = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            // 
            // fontPicker1
            // 
            this.fontPicker1.BackColor = System.Drawing.SystemColors.Window;
            this.fontPicker1.Context = null;
            this.fontPicker1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fontPicker1.Location = new System.Drawing.Point(12, 151);
            this.fontPicker1.Name = "fontPicker1";
            this.fontPicker1.ReadOnly = false;
            this.fontPicker1.Size = new System.Drawing.Size(184, 21);
            this.fontPicker1.TabIndex = 34;
            this.fontPicker1.Text = "Microsoft Sans Serif, 8.25pt";
            this.fontPicker1.Value = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            // 
            // UCColProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupColSet);
            this.Name = "UCColProperty";
            this.Size = new System.Drawing.Size(219, 277);
            this.groupColSet.ResumeLayout(false);
            this.groupColSet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.Grouper groupColSet;
        private Skyray.Controls.CheckBoxW chkHideCol;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private Skyray.Controls.LabelW lblColWidth;
        private Skyray.Controls.LabelW lblColDisplayName;
        private Skyray.Controls.TextBoxW textBoxW2;
        private Skyray.Controls.LabelW lblColName;
        private System.Windows.Forms.ComboBox comboBox1;
        private Skyray.Controls.LabelW lblHeaderFont;
        private Skyray.Controls.LabelW lblColFont;
        private Skyray.Controls.LabelW lblExpression;
        private Skyray.Controls.TextBoxW textBoxW1;
        private Skyray.Controls.LabelW lblExceptionFont;
        private Skyray.Controls.FontPicker fontPicker3;
        private Skyray.Controls.FontPicker fontPicker2;
        private Skyray.Controls.FontPicker fontPicker1;
    }
}
