namespace Skyray.UC
{
    partial class UCCaculateBits
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
            this.lbwThickBits = new Skyray.Controls.LabelW();
            this.nUpThick = new Skyray.Controls.NumricUpDownW();
            this.lbwContentBits = new Skyray.Controls.LabelW();
            this.nUpContent = new Skyray.Controls.NumricUpDownW();
            this.btWSubmit = new Skyray.Controls.ButtonW();
            this.btWCancel = new Skyray.Controls.ButtonW();
            this.grouper1 = new Skyray.Controls.Grouper();
            this.grouper2 = new Skyray.Controls.Grouper();
            this.comboBoxLayers = new System.Windows.Forms.ComboBox();
            this.lblLayer = new Skyray.Controls.LabelW();
            this.labelW3 = new Skyray.Controls.LabelW();
            this.lblDensity = new Skyray.Controls.LabelW();
            this.numDensity = new Skyray.Controls.NumricUpDownW();
            this.lblDensityCoef = new Skyray.Controls.LabelW();
            this.numDensityCoef = new Skyray.Controls.NumricUpDownW();
            ((System.ComponentModel.ISupportInitialize)(this.nUpThick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpContent)).BeginInit();
            this.grouper1.SuspendLayout();
            this.grouper2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityCoef)).BeginInit();
            this.SuspendLayout();
            // 
            // lbwThickBits
            // 
            this.lbwThickBits.AutoSize = true;
            this.lbwThickBits.BackColor = System.Drawing.Color.Transparent;
            this.lbwThickBits.Location = new System.Drawing.Point(19, 42);
            this.lbwThickBits.Name = "lbwThickBits";
            this.lbwThickBits.Size = new System.Drawing.Size(53, 12);
            this.lbwThickBits.TabIndex = 0;
            this.lbwThickBits.Text = "镀层精度";
            // 
            // nUpThick
            // 
            this.nUpThick.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.nUpThick.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.nUpThick.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.nUpThick.Location = new System.Drawing.Point(96, 36);
            this.nUpThick.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nUpThick.Name = "nUpThick";
            this.nUpThick.Size = new System.Drawing.Size(103, 21);
            this.nUpThick.TabIndex = 1;
            this.nUpThick.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // lbwContentBits
            // 
            this.lbwContentBits.AutoSize = true;
            this.lbwContentBits.BackColor = System.Drawing.Color.Transparent;
            this.lbwContentBits.Location = new System.Drawing.Point(17, 90);
            this.lbwContentBits.Name = "lbwContentBits";
            this.lbwContentBits.Size = new System.Drawing.Size(53, 12);
            this.lbwContentBits.TabIndex = 2;
            this.lbwContentBits.Text = "成分精度";
            // 
            // nUpContent
            // 
            this.nUpContent.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.nUpContent.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.nUpContent.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.nUpContent.Location = new System.Drawing.Point(96, 84);
            this.nUpContent.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nUpContent.Name = "nUpContent";
            this.nUpContent.Size = new System.Drawing.Size(103, 21);
            this.nUpContent.TabIndex = 3;
            this.nUpContent.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // btWSubmit
            // 
            this.btWSubmit.bSilver = false;
            this.btWSubmit.Location = new System.Drawing.Point(135, 150);
            this.btWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btWSubmit.Name = "btWSubmit";
            this.btWSubmit.Size = new System.Drawing.Size(75, 23);
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
            this.btWCancel.Location = new System.Drawing.Point(261, 150);
            this.btWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btWCancel.Name = "btWCancel";
            this.btWCancel.Size = new System.Drawing.Size(75, 23);
            this.btWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWCancel.TabIndex = 5;
            this.btWCancel.Text = "取消";
            this.btWCancel.ToFocused = false;
            this.btWCancel.UseVisualStyleBackColor = true;
            this.btWCancel.Click += new System.EventHandler(this.btWCancel_Click);
            // 
            // grouper1
            // 
            this.grouper1.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouper1.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouper1.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouper1.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grouper1.BorderThickness = 1F;
            this.grouper1.BorderTopOnly = false;
            this.grouper1.Controls.Add(this.lbwThickBits);
            this.grouper1.Controls.Add(this.nUpThick);
            this.grouper1.Controls.Add(this.lbwContentBits);
            this.grouper1.Controls.Add(this.nUpContent);
            this.grouper1.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grouper1.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grouper1.GroupImage = null;
            this.grouper1.GroupTitle = "计算精度";
            this.grouper1.HeaderRoundCorners = 4;
            this.grouper1.Location = new System.Drawing.Point(11, 11);
            this.grouper1.Name = "grouper1";
            this.grouper1.PaintGroupBox = false;
            this.grouper1.RoundCorners = 4;
            this.grouper1.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper1.ShadowControl = false;
            this.grouper1.ShadowThickness = 3;
            this.grouper1.Size = new System.Drawing.Size(214, 123);
            this.grouper1.TabIndex = 6;
            this.grouper1.TextLineSpace = 2;
            this.grouper1.TitleLeftSpace = 18;
            // 
            // grouper2
            // 
            this.grouper2.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouper2.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouper2.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouper2.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grouper2.BorderThickness = 1F;
            this.grouper2.BorderTopOnly = false;
            this.grouper2.Controls.Add(this.comboBoxLayers);
            this.grouper2.Controls.Add(this.lblLayer);
            this.grouper2.Controls.Add(this.labelW3);
            this.grouper2.Controls.Add(this.lblDensity);
            this.grouper2.Controls.Add(this.numDensity);
            this.grouper2.Controls.Add(this.lblDensityCoef);
            this.grouper2.Controls.Add(this.numDensityCoef);
            this.grouper2.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grouper2.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grouper2.GroupImage = null;
            this.grouper2.GroupTitle = "计算因子";
            this.grouper2.HeaderRoundCorners = 4;
            this.grouper2.Location = new System.Drawing.Point(245, 11);
            this.grouper2.Name = "grouper2";
            this.grouper2.PaintGroupBox = false;
            this.grouper2.RoundCorners = 4;
            this.grouper2.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper2.ShadowControl = false;
            this.grouper2.ShadowThickness = 3;
            this.grouper2.Size = new System.Drawing.Size(214, 123);
            this.grouper2.TabIndex = 7;
            this.grouper2.TextLineSpace = 2;
            this.grouper2.TitleLeftSpace = 18;
            // 
            // comboBoxLayers
            // 
            this.comboBoxLayers.Location = new System.Drawing.Point(67, 37);
            this.comboBoxLayers.Name = "comboBoxLayers";
            this.comboBoxLayers.Size = new System.Drawing.Size(94, 20);
            this.comboBoxLayers.TabIndex = 13;
            this.comboBoxLayers.SelectedIndexChanged += new System.EventHandler(this.comboBoxLayers_SelectedIndexChanged);
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            this.lblLayer.BackColor = System.Drawing.Color.Transparent;
            this.lblLayer.Location = new System.Drawing.Point(10, 41);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(29, 12);
            this.lblLayer.TabIndex = 5;
            this.lblLayer.Text = "膜层";
            // 
            // labelW3
            // 
            this.labelW3.AutoSize = true;
            this.labelW3.BackColor = System.Drawing.Color.Transparent;
            this.labelW3.Location = new System.Drawing.Point(164, 93);
            this.labelW3.Name = "labelW3";
            this.labelW3.Size = new System.Drawing.Size(53, 12);
            this.labelW3.TabIndex = 4;
            this.labelW3.Text = "(g/cm^3)";
            // 
            // lblDensity
            // 
            this.lblDensity.AutoSize = true;
            this.lblDensity.BackColor = System.Drawing.Color.Transparent;
            this.lblDensity.Location = new System.Drawing.Point(9, 94);
            this.lblDensity.Name = "lblDensity";
            this.lblDensity.Size = new System.Drawing.Size(53, 12);
            this.lblDensity.TabIndex = 0;
            this.lblDensity.Text = "膜层密度";
            // 
            // numDensity
            // 
            this.numDensity.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numDensity.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numDensity.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numDensity.DecimalPlaces = 3;
            this.numDensity.Location = new System.Drawing.Point(67, 89);
            this.numDensity.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numDensity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numDensity.Name = "numDensity";
            this.numDensity.Size = new System.Drawing.Size(94, 21);
            this.numDensity.TabIndex = 1;
            this.numDensity.ValueChanged += new System.EventHandler(this.numDensity_ValueChanged);
            // 
            // lblDensityCoef
            // 
            this.lblDensityCoef.AutoSize = true;
            this.lblDensityCoef.BackColor = System.Drawing.Color.Transparent;
            this.lblDensityCoef.Location = new System.Drawing.Point(10, 94);
            this.lblDensityCoef.Name = "lblDensityCoef";
            this.lblDensityCoef.Size = new System.Drawing.Size(29, 12);
            this.lblDensityCoef.TabIndex = 2;
            this.lblDensityCoef.Text = "因子";
            this.lblDensityCoef.Visible = false;
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
            this.numDensityCoef.Location = new System.Drawing.Point(67, 89);
            this.numDensityCoef.Name = "numDensityCoef";
            this.numDensityCoef.Size = new System.Drawing.Size(94, 21);
            this.numDensityCoef.TabIndex = 3;
            this.numDensityCoef.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numDensityCoef.Visible = false;
            // 
            // UCCaculateBits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grouper2);
            this.Controls.Add(this.grouper1);
            this.Controls.Add(this.btWCancel);
            this.Controls.Add(this.btWSubmit);
            this.Name = "UCCaculateBits";
            this.Size = new System.Drawing.Size(470, 182);
            ((System.ComponentModel.ISupportInitialize)(this.nUpThick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpContent)).EndInit();
            this.grouper1.ResumeLayout(false);
            this.grouper1.PerformLayout();
            this.grouper2.ResumeLayout(false);
            this.grouper2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensityCoef)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.LabelW lbwThickBits;
        private Skyray.Controls.NumricUpDownW nUpThick;
        private Skyray.Controls.LabelW lbwContentBits;
        private Skyray.Controls.NumricUpDownW nUpContent;
        private Skyray.Controls.ButtonW btWSubmit;
        private Skyray.Controls.ButtonW btWCancel;
        private Skyray.Controls.Grouper grouper1;
        private Skyray.Controls.Grouper grouper2;
        private Skyray.Controls.LabelW lblDensity;
        private Skyray.Controls.NumricUpDownW numDensity;
        private Skyray.Controls.LabelW lblDensityCoef;
        private Skyray.Controls.NumricUpDownW numDensityCoef;
        private Skyray.Controls.LabelW labelW3;
        private Skyray.Controls.LabelW lblLayer;
        private System.Windows.Forms.ComboBox comboBoxLayers;
    }
}
