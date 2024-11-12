namespace Skyray.UC
{
    partial class UCCaculateParamSet
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
            this.btWSubmit = new Skyray.Controls.ButtonW();
            this.btWCancel = new Skyray.Controls.ButtonW();
            this.grouper2 = new Skyray.Controls.Grouper();
            this.numlimitvalue = new Skyray.Controls.NumricUpDownW();
            this.lblcLimit = new Skyray.Controls.LabelW();
            this.numkvalue = new Skyray.Controls.NumricUpDownW();
            this.lblkvalue = new Skyray.Controls.LabelW();
            this.lblbvalue = new Skyray.Controls.LabelW();
            this.numbvalue = new Skyray.Controls.NumricUpDownW();
            this.numAvalue = new Skyray.Controls.NumricUpDownW();
            this.lblavalue = new Skyray.Controls.LabelW();
            this.lblnvalue = new Skyray.Controls.LabelW();
            this.numNvalue = new Skyray.Controls.NumricUpDownW();
            this.numDensityCoef = new Skyray.Controls.NumricUpDownW();
            this.grouper2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numlimitvalue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numkvalue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numbvalue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvalue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNvalue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityCoef)).BeginInit();
            this.SuspendLayout();
            // 
            // btWSubmit
            // 
            this.btWSubmit.bSilver = false;
            this.btWSubmit.Location = new System.Drawing.Point(41, 274);
            this.btWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btWSubmit.Name = "btWSubmit";
            this.btWSubmit.Size = new System.Drawing.Size(75, 25);
            this.btWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWSubmit.TabIndex = 4;
            this.btWSubmit.Text = "确定";
            this.btWSubmit.ToFocused = false;
            this.btWSubmit.UseVisualStyleBackColor = true;
            this.btWSubmit.Click += new System.EventHandler(this.btWSubmit_Click);
            // 
            // btWCancel
            // 
            this.btWCancel.bSilver = false;
            this.btWCancel.Location = new System.Drawing.Point(165, 274);
            this.btWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btWCancel.Name = "btWCancel";
            this.btWCancel.Size = new System.Drawing.Size(75, 25);
            this.btWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWCancel.TabIndex = 5;
            this.btWCancel.Text = "取消";
            this.btWCancel.ToFocused = false;
            this.btWCancel.UseVisualStyleBackColor = true;
            this.btWCancel.Click += new System.EventHandler(this.btWCancel_Click);
            // 
            // grouper2
            // 
            this.grouper2.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouper2.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouper2.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouper2.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grouper2.BorderThickness = 1F;
            this.grouper2.BorderTopOnly = false;
            this.grouper2.Controls.Add(this.numlimitvalue);
            this.grouper2.Controls.Add(this.lblcLimit);
            this.grouper2.Controls.Add(this.numkvalue);
            this.grouper2.Controls.Add(this.lblkvalue);
            this.grouper2.Controls.Add(this.lblbvalue);
            this.grouper2.Controls.Add(this.numbvalue);
            this.grouper2.Controls.Add(this.numAvalue);
            this.grouper2.Controls.Add(this.lblavalue);
            this.grouper2.Controls.Add(this.lblnvalue);
            this.grouper2.Controls.Add(this.numNvalue);
            this.grouper2.Controls.Add(this.numDensityCoef);
            this.grouper2.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grouper2.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grouper2.GroupImage = null;
            this.grouper2.GroupTitle = "计算因子";
            this.grouper2.HeaderRoundCorners = 4;
            this.grouper2.Location = new System.Drawing.Point(41, 12);
            this.grouper2.Name = "grouper2";
            this.grouper2.PaintGroupBox = false;
            this.grouper2.RoundCorners = 4;
            this.grouper2.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper2.ShadowControl = false;
            this.grouper2.ShadowThickness = 3;
            this.grouper2.Size = new System.Drawing.Size(199, 232);
            this.grouper2.TabIndex = 7;
            this.grouper2.TextLineSpace = 2;
            this.grouper2.TitleLeftSpace = 18;
            // 
            // numlimitvalue
            // 
            this.numlimitvalue.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numlimitvalue.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numlimitvalue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numlimitvalue.DecimalPlaces = 3;
            this.numlimitvalue.Location = new System.Drawing.Point(67, 190);
            this.numlimitvalue.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numlimitvalue.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.numlimitvalue.Name = "numlimitvalue";
            this.numlimitvalue.Size = new System.Drawing.Size(94, 20);
            this.numlimitvalue.TabIndex = 12;
            // 
            // lblcLimit
            // 
            this.lblcLimit.AutoSize = true;
            this.lblcLimit.BackColor = System.Drawing.Color.Transparent;
            this.lblcLimit.Location = new System.Drawing.Point(10, 192);
            this.lblcLimit.Name = "lblcLimit";
            this.lblcLimit.Size = new System.Drawing.Size(27, 13);
            this.lblcLimit.TabIndex = 11;
            this.lblcLimit.Text = "limit:";
            // 
            // numkvalue
            // 
            this.numkvalue.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numkvalue.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numkvalue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numkvalue.DecimalPlaces = 3;
            this.numkvalue.Location = new System.Drawing.Point(67, 118);
            this.numkvalue.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numkvalue.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.numkvalue.Name = "numkvalue";
            this.numkvalue.Size = new System.Drawing.Size(94, 20);
            this.numkvalue.TabIndex = 10;
            // 
            // lblkvalue
            // 
            this.lblkvalue.AutoSize = true;
            this.lblkvalue.BackColor = System.Drawing.Color.Transparent;
            this.lblkvalue.Location = new System.Drawing.Point(10, 120);
            this.lblkvalue.Name = "lblkvalue";
            this.lblkvalue.Size = new System.Drawing.Size(16, 13);
            this.lblkvalue.TabIndex = 9;
            this.lblkvalue.Text = "k:";
            // 
            // lblbvalue
            // 
            this.lblbvalue.AutoSize = true;
            this.lblbvalue.BackColor = System.Drawing.Color.Transparent;
            this.lblbvalue.Location = new System.Drawing.Point(10, 161);
            this.lblbvalue.Name = "lblbvalue";
            this.lblbvalue.Size = new System.Drawing.Size(16, 13);
            this.lblbvalue.TabIndex = 7;
            this.lblbvalue.Text = "b:";
            // 
            // numbvalue
            // 
            this.numbvalue.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numbvalue.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numbvalue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numbvalue.DecimalPlaces = 3;
            this.numbvalue.Location = new System.Drawing.Point(67, 157);
            this.numbvalue.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numbvalue.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.numbvalue.Name = "numbvalue";
            this.numbvalue.Size = new System.Drawing.Size(94, 20);
            this.numbvalue.TabIndex = 8;
            // 
            // numAvalue
            // 
            this.numAvalue.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numAvalue.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numAvalue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numAvalue.DecimalPlaces = 3;
            this.numAvalue.Location = new System.Drawing.Point(67, 42);
            this.numAvalue.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numAvalue.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.numAvalue.Name = "numAvalue";
            this.numAvalue.Size = new System.Drawing.Size(94, 20);
            this.numAvalue.TabIndex = 6;
            // 
            // lblavalue
            // 
            this.lblavalue.AutoSize = true;
            this.lblavalue.BackColor = System.Drawing.Color.Transparent;
            this.lblavalue.Location = new System.Drawing.Point(10, 44);
            this.lblavalue.Name = "lblavalue";
            this.lblavalue.Size = new System.Drawing.Size(16, 13);
            this.lblavalue.TabIndex = 5;
            this.lblavalue.Text = "a:";
            // 
            // lblnvalue
            // 
            this.lblnvalue.AutoSize = true;
            this.lblnvalue.BackColor = System.Drawing.Color.Transparent;
            this.lblnvalue.Location = new System.Drawing.Point(10, 82);
            this.lblnvalue.Name = "lblnvalue";
            this.lblnvalue.Size = new System.Drawing.Size(16, 13);
            this.lblnvalue.TabIndex = 0;
            this.lblnvalue.Text = "n:";
            // 
            // numNvalue
            // 
            this.numNvalue.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numNvalue.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numNvalue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numNvalue.DecimalPlaces = 3;
            this.numNvalue.Location = new System.Drawing.Point(67, 80);
            this.numNvalue.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numNvalue.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.numNvalue.Name = "numNvalue";
            this.numNvalue.Size = new System.Drawing.Size(94, 20);
            this.numNvalue.TabIndex = 1;
            // 
            // numDensityCoef
            // 
            this.numDensityCoef.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numDensityCoef.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numDensityCoef.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numDensityCoef.DecimalPlaces = 1;
            this.numDensityCoef.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numDensityCoef.Location = new System.Drawing.Point(67, 80);
            this.numDensityCoef.Name = "numDensityCoef";
            this.numDensityCoef.Size = new System.Drawing.Size(94, 20);
            this.numDensityCoef.TabIndex = 3;
            this.numDensityCoef.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numDensityCoef.Visible = false;
            // 
            // UCCaculateParamSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grouper2);
            this.Controls.Add(this.btWCancel);
            this.Controls.Add(this.btWSubmit);
            this.Name = "UCCaculateParamSet";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(270, 322);
            this.grouper2.ResumeLayout(false);
            this.grouper2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numlimitvalue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numkvalue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numbvalue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvalue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNvalue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityCoef)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btWSubmit;
        private Skyray.Controls.ButtonW btWCancel;
        private Skyray.Controls.Grouper grouper2;
        private Skyray.Controls.LabelW lblnvalue;
        private Skyray.Controls.NumricUpDownW numNvalue;
        private Skyray.Controls.NumricUpDownW numDensityCoef;
        private Skyray.Controls.LabelW lblavalue;
        private Skyray.Controls.NumricUpDownW numAvalue;
        private Skyray.Controls.NumricUpDownW numlimitvalue;
        private Skyray.Controls.LabelW lblcLimit;
        private Skyray.Controls.NumricUpDownW numkvalue;
        private Skyray.Controls.LabelW lblkvalue;
        private Skyray.Controls.LabelW lblbvalue;
        private Skyray.Controls.NumricUpDownW numbvalue;
    }
}
