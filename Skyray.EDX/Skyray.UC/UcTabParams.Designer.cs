namespace Skyray.UC
{
    partial class UcTabParams
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
            this.btWCancel = new Skyray.Controls.ButtonW();
            this.btWSubmit = new Skyray.Controls.ButtonW();
            this.lblInOutDis = new Skyray.Controls.LabelW();
            this.numInOutDis = new Skyray.Controls.NumricUpDownW();
            this.lblTabResetHeight = new Skyray.Controls.LabelW();
            this.numTabResetHeight = new Skyray.Controls.NumricUpDownW();
            this.lblTwoCameraDis = new Skyray.Controls.LabelW();
            this.numTwoCameraDis = new Skyray.Controls.NumricUpDownW();
            this.lblTestDis = new Skyray.Controls.LabelW();
            this.numTestDis = new Skyray.Controls.NumricUpDownW();
            this.lblLargeViewFormula = new Skyray.Controls.LabelW();
            this.numSquareCoeff = new Skyray.Controls.NumricUpDownW();
            this.lblSquareCoeff = new Skyray.Controls.LabelW();
            this.numBaseCoeff = new Skyray.Controls.NumricUpDownW();
            this.lblBaseCoeff = new Skyray.Controls.LabelW();
            this.numMultiCoeff = new Skyray.Controls.NumricUpDownW();
            this.lblHeightWidthRatio = new Skyray.Controls.LabelW();
            this.lblMultiCoeff = new Skyray.Controls.LabelW();
            this.numHeightWidthRatio = new Skyray.Controls.NumricUpDownW();
            ((System.ComponentModel.ISupportInitialize)(this.numInOutDis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTabResetHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTwoCameraDis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTestDis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSquareCoeff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBaseCoeff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMultiCoeff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeightWidthRatio)).BeginInit();
            this.SuspendLayout();
            // 
            // btWCancel
            // 
            this.btWCancel.bSilver = false;
            this.btWCancel.Location = new System.Drawing.Point(350, 493);
            this.btWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btWCancel.Name = "btWCancel";
            this.btWCancel.Size = new System.Drawing.Size(75, 23);
            this.btWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWCancel.TabIndex = 9;
            this.btWCancel.Text = "取消";
            this.btWCancel.ToFocused = false;
            this.btWCancel.UseVisualStyleBackColor = true;
            this.btWCancel.Click += new System.EventHandler(this.btWCancel_Click);
            // 
            // btWSubmit
            // 
            this.btWSubmit.bSilver = false;
            this.btWSubmit.Location = new System.Drawing.Point(89, 493);
            this.btWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btWSubmit.Name = "btWSubmit";
            this.btWSubmit.Size = new System.Drawing.Size(75, 23);
            this.btWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWSubmit.TabIndex = 8;
            this.btWSubmit.Text = "确定";
            this.btWSubmit.ToFocused = false;
            this.btWSubmit.UseVisualStyleBackColor = true;
            this.btWSubmit.Click += new System.EventHandler(this.btWSubmit_Click);
            // 
            // lblInOutDis
            // 
            this.lblInOutDis.AutoSize = true;
            this.lblInOutDis.BackColor = System.Drawing.Color.Transparent;
            this.lblInOutDis.Location = new System.Drawing.Point(87, 137);
            this.lblInOutDis.Name = "lblInOutDis";
            this.lblInOutDis.Size = new System.Drawing.Size(65, 12);
            this.lblInOutDis.TabIndex = 10;
            this.lblInOutDis.Text = "进出样距离";
            // 
            // numInOutDis
            // 
            this.numInOutDis.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numInOutDis.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numInOutDis.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numInOutDis.DecimalPlaces = 2;
            this.numInOutDis.Location = new System.Drawing.Point(273, 137);
            this.numInOutDis.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numInOutDis.Name = "numInOutDis";
            this.numInOutDis.Size = new System.Drawing.Size(103, 21);
            this.numInOutDis.TabIndex = 11;
            this.numInOutDis.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblTabResetHeight
            // 
            this.lblTabResetHeight.AutoSize = true;
            this.lblTabResetHeight.BackColor = System.Drawing.Color.Transparent;
            this.lblTabResetHeight.Location = new System.Drawing.Point(87, 76);
            this.lblTabResetHeight.Name = "lblTabResetHeight";
            this.lblTabResetHeight.Size = new System.Drawing.Size(77, 12);
            this.lblTabResetHeight.TabIndex = 12;
            this.lblTabResetHeight.Text = "平台复位高度";
            // 
            // numTabResetHeight
            // 
            this.numTabResetHeight.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numTabResetHeight.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numTabResetHeight.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numTabResetHeight.DecimalPlaces = 2;
            this.numTabResetHeight.Location = new System.Drawing.Point(273, 76);
            this.numTabResetHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTabResetHeight.Name = "numTabResetHeight";
            this.numTabResetHeight.Size = new System.Drawing.Size(103, 21);
            this.numTabResetHeight.TabIndex = 14;
            this.numTabResetHeight.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblTwoCameraDis
            // 
            this.lblTwoCameraDis.AutoSize = true;
            this.lblTwoCameraDis.BackColor = System.Drawing.Color.Transparent;
            this.lblTwoCameraDis.Location = new System.Drawing.Point(87, 199);
            this.lblTwoCameraDis.Name = "lblTwoCameraDis";
            this.lblTwoCameraDis.Size = new System.Drawing.Size(125, 12);
            this.lblTwoCameraDis.TabIndex = 15;
            this.lblTwoCameraDis.Text = "远景与近景摄像头距离";
            // 
            // numTwoCameraDis
            // 
            this.numTwoCameraDis.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numTwoCameraDis.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numTwoCameraDis.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numTwoCameraDis.DecimalPlaces = 2;
            this.numTwoCameraDis.Location = new System.Drawing.Point(273, 199);
            this.numTwoCameraDis.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTwoCameraDis.Name = "numTwoCameraDis";
            this.numTwoCameraDis.Size = new System.Drawing.Size(103, 21);
            this.numTwoCameraDis.TabIndex = 16;
            this.numTwoCameraDis.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblTestDis
            // 
            this.lblTestDis.AutoSize = true;
            this.lblTestDis.BackColor = System.Drawing.Color.Transparent;
            this.lblTestDis.Location = new System.Drawing.Point(87, 265);
            this.lblTestDis.Name = "lblTestDis";
            this.lblTestDis.Size = new System.Drawing.Size(137, 12);
            this.lblTestDis.TabIndex = 17;
            this.lblTestDis.Text = "近景摄像头与测试点距离";
            // 
            // numTestDis
            // 
            this.numTestDis.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numTestDis.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numTestDis.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numTestDis.DecimalPlaces = 2;
            this.numTestDis.Location = new System.Drawing.Point(273, 265);
            this.numTestDis.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTestDis.Name = "numTestDis";
            this.numTestDis.Size = new System.Drawing.Size(103, 21);
            this.numTestDis.TabIndex = 18;
            this.numTestDis.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblLargeViewFormula
            // 
            this.lblLargeViewFormula.AutoSize = true;
            this.lblLargeViewFormula.BackColor = System.Drawing.Color.Transparent;
            this.lblLargeViewFormula.Location = new System.Drawing.Point(87, 321);
            this.lblLargeViewFormula.Name = "lblLargeViewFormula";
            this.lblLargeViewFormula.Size = new System.Drawing.Size(125, 12);
            this.lblLargeViewFormula.TabIndex = 19;
            this.lblLargeViewFormula.Text = "远景图像大小公式系数";
            // 
            // numSquareCoeff
            // 
            this.numSquareCoeff.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numSquareCoeff.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numSquareCoeff.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numSquareCoeff.DecimalPlaces = 2;
            this.numSquareCoeff.Location = new System.Drawing.Point(217, 359);
            this.numSquareCoeff.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSquareCoeff.Name = "numSquareCoeff";
            this.numSquareCoeff.Size = new System.Drawing.Size(65, 21);
            this.numSquareCoeff.TabIndex = 20;
            this.numSquareCoeff.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblSquareCoeff
            // 
            this.lblSquareCoeff.AutoSize = true;
            this.lblSquareCoeff.BackColor = System.Drawing.Color.Transparent;
            this.lblSquareCoeff.Location = new System.Drawing.Point(87, 359);
            this.lblSquareCoeff.Name = "lblSquareCoeff";
            this.lblSquareCoeff.Size = new System.Drawing.Size(65, 12);
            this.lblSquareCoeff.TabIndex = 24;
            this.lblSquareCoeff.Text = "二次项系数";
            // 
            // numBaseCoeff
            // 
            this.numBaseCoeff.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numBaseCoeff.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numBaseCoeff.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numBaseCoeff.DecimalPlaces = 2;
            this.numBaseCoeff.Location = new System.Drawing.Point(217, 405);
            this.numBaseCoeff.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numBaseCoeff.Name = "numBaseCoeff";
            this.numBaseCoeff.Size = new System.Drawing.Size(65, 21);
            this.numBaseCoeff.TabIndex = 25;
            this.numBaseCoeff.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblBaseCoeff
            // 
            this.lblBaseCoeff.AutoSize = true;
            this.lblBaseCoeff.BackColor = System.Drawing.Color.Transparent;
            this.lblBaseCoeff.Location = new System.Drawing.Point(87, 407);
            this.lblBaseCoeff.Name = "lblBaseCoeff";
            this.lblBaseCoeff.Size = new System.Drawing.Size(41, 12);
            this.lblBaseCoeff.TabIndex = 26;
            this.lblBaseCoeff.Text = "基系数";
            // 
            // numMultiCoeff
            // 
            this.numMultiCoeff.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMultiCoeff.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMultiCoeff.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMultiCoeff.DecimalPlaces = 2;
            this.numMultiCoeff.Location = new System.Drawing.Point(479, 359);
            this.numMultiCoeff.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMultiCoeff.Name = "numMultiCoeff";
            this.numMultiCoeff.Size = new System.Drawing.Size(65, 21);
            this.numMultiCoeff.TabIndex = 27;
            this.numMultiCoeff.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblHeightWidthRatio
            // 
            this.lblHeightWidthRatio.AutoSize = true;
            this.lblHeightWidthRatio.BackColor = System.Drawing.Color.Transparent;
            this.lblHeightWidthRatio.Location = new System.Drawing.Point(321, 407);
            this.lblHeightWidthRatio.Name = "lblHeightWidthRatio";
            this.lblHeightWidthRatio.Size = new System.Drawing.Size(89, 12);
            this.lblHeightWidthRatio.TabIndex = 28;
            this.lblHeightWidthRatio.Text = "远景图像高宽比";
            // 
            // lblMultiCoeff
            // 
            this.lblMultiCoeff.AutoSize = true;
            this.lblMultiCoeff.BackColor = System.Drawing.Color.Transparent;
            this.lblMultiCoeff.Location = new System.Drawing.Point(321, 359);
            this.lblMultiCoeff.Name = "lblMultiCoeff";
            this.lblMultiCoeff.Size = new System.Drawing.Size(65, 12);
            this.lblMultiCoeff.TabIndex = 29;
            this.lblMultiCoeff.Text = "一次项系数";
            // 
            // numHeightWidthRatio
            // 
            this.numHeightWidthRatio.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numHeightWidthRatio.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numHeightWidthRatio.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numHeightWidthRatio.DecimalPlaces = 2;
            this.numHeightWidthRatio.Location = new System.Drawing.Point(479, 407);
            this.numHeightWidthRatio.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numHeightWidthRatio.Name = "numHeightWidthRatio";
            this.numHeightWidthRatio.Size = new System.Drawing.Size(65, 21);
            this.numHeightWidthRatio.TabIndex = 30;
            this.numHeightWidthRatio.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // UcTabParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.numHeightWidthRatio);
            this.Controls.Add(this.lblMultiCoeff);
            this.Controls.Add(this.lblHeightWidthRatio);
            this.Controls.Add(this.numMultiCoeff);
            this.Controls.Add(this.lblBaseCoeff);
            this.Controls.Add(this.numBaseCoeff);
            this.Controls.Add(this.lblSquareCoeff);
            this.Controls.Add(this.numSquareCoeff);
            this.Controls.Add(this.lblLargeViewFormula);
            this.Controls.Add(this.numTestDis);
            this.Controls.Add(this.lblTestDis);
            this.Controls.Add(this.numTwoCameraDis);
            this.Controls.Add(this.lblTwoCameraDis);
            this.Controls.Add(this.numTabResetHeight);
            this.Controls.Add(this.lblTabResetHeight);
            this.Controls.Add(this.numInOutDis);
            this.Controls.Add(this.lblInOutDis);
            this.Controls.Add(this.btWCancel);
            this.Controls.Add(this.btWSubmit);
            this.Name = "UcTabParams";
            this.Size = new System.Drawing.Size(598, 527);
            this.Load += new System.EventHandler(this.UcTabParams_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numInOutDis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTabResetHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTwoCameraDis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTestDis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSquareCoeff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBaseCoeff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMultiCoeff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeightWidthRatio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btWCancel;
        private Skyray.Controls.ButtonW btWSubmit;
        private Skyray.Controls.LabelW lblInOutDis;
        private Skyray.Controls.NumricUpDownW numInOutDis;
        private Skyray.Controls.LabelW lblTabResetHeight;
        private Skyray.Controls.NumricUpDownW numTabResetHeight;
        private Skyray.Controls.LabelW lblTwoCameraDis;
        private Skyray.Controls.NumricUpDownW numTwoCameraDis;
        private Skyray.Controls.LabelW lblTestDis;
        private Skyray.Controls.NumricUpDownW numTestDis;
        private Skyray.Controls.LabelW lblLargeViewFormula;
        private Skyray.Controls.NumricUpDownW numSquareCoeff;
        private Skyray.Controls.LabelW lblSquareCoeff;
        private Skyray.Controls.NumricUpDownW numBaseCoeff;
        private Skyray.Controls.LabelW lblBaseCoeff;
        private Skyray.Controls.NumricUpDownW numMultiCoeff;
        private Skyray.Controls.LabelW lblHeightWidthRatio;
        private Skyray.Controls.LabelW lblMultiCoeff;
        private Skyray.Controls.NumricUpDownW numHeightWidthRatio;

    }
}
