namespace Skyray.UC
{
    partial class UcInCalibrate
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
            this.btnResetHigh = new Skyray.Controls.ButtonW();
            this.btnSubmit = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.dgvIntensitys = new Skyray.Controls.DataGridViewW();
            this.btnSpLCalibrate = new Skyray.Controls.ButtonW();
            this.btnspLMeasure = new Skyray.Controls.ButtonW();
            this.btnSpHCalibrate = new Skyray.Controls.ButtonW();
            this.btnSpHMeasure = new Skyray.Controls.ButtonW();
            this.lblLowSample = new Skyray.Controls.LabelW();
            this.lblHighSample = new Skyray.Controls.LabelW();
            this.grpCalculateType = new Skyray.Controls.Grouper();
            this.txtCoefb = new System.Windows.Forms.TextBox();
            this.txtCoefR = new System.Windows.Forms.TextBox();
            this.txtMeasLowT = new System.Windows.Forms.TextBox();
            this.txtMesuHighT = new System.Windows.Forms.TextBox();
            this.txtCaliLowT = new System.Windows.Forms.TextBox();
            this.txtCaliHighT = new System.Windows.Forms.TextBox();
            this.txtSpLowT = new System.Windows.Forms.TextBox();
            this.txtSpHightT = new System.Windows.Forms.TextBox();
            this.A_B = new System.Windows.Forms.Label();
            this.lblMeasureInT = new System.Windows.Forms.Label();
            this.lblCalibrateInT = new System.Windows.Forms.Label();
            this.lblLowSampleT = new System.Windows.Forms.Label();
            this.lblHighSampleT = new System.Windows.Forms.Label();
            this.lblSampleNameT = new System.Windows.Forms.Label();
            this.lblCaculteType = new System.Windows.Forms.Label();
            this.rdobtnA_B = new System.Windows.Forms.RadioButton();
            this.rdobtnA = new System.Windows.Forms.RadioButton();
            this.btnResetLow = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIntensitys)).BeginInit();
            this.grpCalculateType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnResetHigh
            // 
            this.btnResetHigh.bSilver = false;
            this.btnResetHigh.Location = new System.Drawing.Point(435, 46);
            this.btnResetHigh.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnResetHigh.MenuPos = new System.Drawing.Point(0, 0);
            this.btnResetHigh.Name = "btnResetHigh";
            this.btnResetHigh.Size = new System.Drawing.Size(136, 23);
            this.btnResetHigh.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnResetHigh.TabIndex = 0;
            this.btnResetHigh.Text = "高标重置";
            this.btnResetHigh.ToFocused = false;
            this.btnResetHigh.UseVisualStyleBackColor = true;
            this.btnResetHigh.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.bSilver = false;
            this.btnSubmit.Location = new System.Drawing.Point(438, 483);
            this.btnSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(136, 23);
            this.btnSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.ToFocused = false;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(438, 518);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(136, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvIntensitys
            // 
            this.dgvIntensitys.AllowUserToAddRows = false;
            this.dgvIntensitys.AllowUserToDeleteRows = false;
            this.dgvIntensitys.AllowUserToResizeRows = false;
            this.dgvIntensitys.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIntensitys.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvIntensitys.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvIntensitys.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvIntensitys.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvIntensitys.ColumnHeadersHeight = 4;
            this.dgvIntensitys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIntensitys.Location = new System.Drawing.Point(11, 22);
            this.dgvIntensitys.MultiSelect = false;
            this.dgvIntensitys.Name = "dgvIntensitys";
            this.dgvIntensitys.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvIntensitys.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvIntensitys.ReadOnly = true;
            this.dgvIntensitys.RowHeadersVisible = false;
            this.dgvIntensitys.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvIntensitys.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvIntensitys.RowTemplate.Height = 23;
            this.dgvIntensitys.SecondaryLength = 1;
            this.dgvIntensitys.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvIntensitys.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvIntensitys.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvIntensitys.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvIntensitys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIntensitys.ShowCellErrors = false;
            this.dgvIntensitys.ShowEportContextMenu = false;
            this.dgvIntensitys.ShowRowErrors = false;
            this.dgvIntensitys.Size = new System.Drawing.Size(416, 288);
            this.dgvIntensitys.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvIntensitys.TabIndex = 4;
            this.dgvIntensitys.ToPrintCols = null;
            this.dgvIntensitys.ToPrintRows = null;
            this.dgvIntensitys.SelectionChanged += new System.EventHandler(this.dgvIntensitys_SelectionChanged);
            // 
            // btnSpLCalibrate
            // 
            this.btnSpLCalibrate.bSilver = false;
            this.btnSpLCalibrate.Location = new System.Drawing.Point(435, 185);
            this.btnSpLCalibrate.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSpLCalibrate.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSpLCalibrate.Name = "btnSpLCalibrate";
            this.btnSpLCalibrate.Size = new System.Drawing.Size(136, 23);
            this.btnSpLCalibrate.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSpLCalibrate.TabIndex = 5;
            this.btnSpLCalibrate.Text = "低标标定...";
            this.btnSpLCalibrate.ToFocused = false;
            this.btnSpLCalibrate.UseVisualStyleBackColor = true;
            this.btnSpLCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // btnspLMeasure
            // 
            this.btnspLMeasure.bSilver = false;
            this.btnspLMeasure.Location = new System.Drawing.Point(435, 214);
            this.btnspLMeasure.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnspLMeasure.MenuPos = new System.Drawing.Point(0, 0);
            this.btnspLMeasure.Name = "btnspLMeasure";
            this.btnspLMeasure.Size = new System.Drawing.Size(136, 23);
            this.btnspLMeasure.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnspLMeasure.TabIndex = 6;
            this.btnspLMeasure.Text = "低标测量...";
            this.btnspLMeasure.ToFocused = false;
            this.btnspLMeasure.UseVisualStyleBackColor = true;
            this.btnspLMeasure.Click += new System.EventHandler(this.btnOrignal_Click);
            // 
            // btnSpHCalibrate
            // 
            this.btnSpHCalibrate.bSilver = false;
            this.btnSpHCalibrate.Location = new System.Drawing.Point(435, 75);
            this.btnSpHCalibrate.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSpHCalibrate.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSpHCalibrate.Name = "btnSpHCalibrate";
            this.btnSpHCalibrate.Size = new System.Drawing.Size(136, 23);
            this.btnSpHCalibrate.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSpHCalibrate.TabIndex = 87;
            this.btnSpHCalibrate.Text = "高标标定...";
            this.btnSpHCalibrate.ToFocused = false;
            this.btnSpHCalibrate.UseVisualStyleBackColor = true;
            this.btnSpHCalibrate.Click += new System.EventHandler(this.btnSpHCalibrate_Click);
            // 
            // btnSpHMeasure
            // 
            this.btnSpHMeasure.bSilver = false;
            this.btnSpHMeasure.Location = new System.Drawing.Point(435, 104);
            this.btnSpHMeasure.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSpHMeasure.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSpHMeasure.Name = "btnSpHMeasure";
            this.btnSpHMeasure.Size = new System.Drawing.Size(136, 23);
            this.btnSpHMeasure.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSpHMeasure.TabIndex = 86;
            this.btnSpHMeasure.Text = "高标测量...";
            this.btnSpHMeasure.ToFocused = false;
            this.btnSpHMeasure.UseVisualStyleBackColor = true;
            this.btnSpHMeasure.Click += new System.EventHandler(this.btnSpHMeasure_Click);
            // 
            // lblLowSample
            // 
            this.lblLowSample.AutoSize = true;
            this.lblLowSample.BackColor = System.Drawing.Color.Transparent;
            this.lblLowSample.Location = new System.Drawing.Point(305, 7);
            this.lblLowSample.Name = "lblLowSample";
            this.lblLowSample.Size = new System.Drawing.Size(29, 12);
            this.lblLowSample.TabIndex = 88;
            this.lblLowSample.Text = "低标";
            // 
            // lblHighSample
            // 
            this.lblHighSample.AutoSize = true;
            this.lblHighSample.BackColor = System.Drawing.Color.Transparent;
            this.lblHighSample.Location = new System.Drawing.Point(124, 7);
            this.lblHighSample.Name = "lblHighSample";
            this.lblHighSample.Size = new System.Drawing.Size(29, 12);
            this.lblHighSample.TabIndex = 89;
            this.lblHighSample.Text = "高标";
            // 
            // grpCalculateType
            // 
            this.grpCalculateType.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpCalculateType.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpCalculateType.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpCalculateType.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpCalculateType.BorderThickness = 1F;
            this.grpCalculateType.BorderTopOnly = false;
            this.grpCalculateType.Controls.Add(this.txtCoefb);
            this.grpCalculateType.Controls.Add(this.txtCoefR);
            this.grpCalculateType.Controls.Add(this.txtMeasLowT);
            this.grpCalculateType.Controls.Add(this.txtMesuHighT);
            this.grpCalculateType.Controls.Add(this.txtCaliLowT);
            this.grpCalculateType.Controls.Add(this.txtCaliHighT);
            this.grpCalculateType.Controls.Add(this.txtSpLowT);
            this.grpCalculateType.Controls.Add(this.txtSpHightT);
            this.grpCalculateType.Controls.Add(this.A_B);
            this.grpCalculateType.Controls.Add(this.lblMeasureInT);
            this.grpCalculateType.Controls.Add(this.lblCalibrateInT);
            this.grpCalculateType.Controls.Add(this.lblLowSampleT);
            this.grpCalculateType.Controls.Add(this.lblHighSampleT);
            this.grpCalculateType.Controls.Add(this.lblSampleNameT);
            this.grpCalculateType.Controls.Add(this.lblCaculteType);
            this.grpCalculateType.Controls.Add(this.rdobtnA_B);
            this.grpCalculateType.Controls.Add(this.rdobtnA);
            this.grpCalculateType.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpCalculateType.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grpCalculateType.GroupImage = null;
            this.grpCalculateType.GroupTitle = "";
            this.grpCalculateType.HeaderRoundCorners = 4;
            this.grpCalculateType.Location = new System.Drawing.Point(11, 316);
            this.grpCalculateType.Name = "grpCalculateType";
            this.grpCalculateType.PaintGroupBox = false;
            this.grpCalculateType.RoundCorners = 4;
            this.grpCalculateType.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpCalculateType.ShadowControl = false;
            this.grpCalculateType.ShadowThickness = 3;
            this.grpCalculateType.Size = new System.Drawing.Size(416, 225);
            this.grpCalculateType.TabIndex = 90;
            this.grpCalculateType.TextLineSpace = 2;
            this.grpCalculateType.TitleLeftSpace = 18;
            // 
            // txtCoefb
            // 
            this.txtCoefb.Location = new System.Drawing.Point(244, 192);
            this.txtCoefb.Name = "txtCoefb";
            this.txtCoefb.ReadOnly = true;
            this.txtCoefb.Size = new System.Drawing.Size(102, 21);
            this.txtCoefb.TabIndex = 105;
            // 
            // txtCoefR
            // 
            this.txtCoefR.Location = new System.Drawing.Point(109, 192);
            this.txtCoefR.Name = "txtCoefR";
            this.txtCoefR.ReadOnly = true;
            this.txtCoefR.Size = new System.Drawing.Size(102, 21);
            this.txtCoefR.TabIndex = 104;
            // 
            // txtMeasLowT
            // 
            this.txtMeasLowT.Location = new System.Drawing.Point(244, 159);
            this.txtMeasLowT.Name = "txtMeasLowT";
            this.txtMeasLowT.Size = new System.Drawing.Size(102, 21);
            this.txtMeasLowT.TabIndex = 103;
            // 
            // txtMesuHighT
            // 
            this.txtMesuHighT.Location = new System.Drawing.Point(109, 159);
            this.txtMesuHighT.Name = "txtMesuHighT";
            this.txtMesuHighT.Size = new System.Drawing.Size(102, 21);
            this.txtMesuHighT.TabIndex = 102;
            // 
            // txtCaliLowT
            // 
            this.txtCaliLowT.Location = new System.Drawing.Point(244, 128);
            this.txtCaliLowT.Name = "txtCaliLowT";
            this.txtCaliLowT.Size = new System.Drawing.Size(102, 21);
            this.txtCaliLowT.TabIndex = 101;
            // 
            // txtCaliHighT
            // 
            this.txtCaliHighT.Location = new System.Drawing.Point(109, 131);
            this.txtCaliHighT.Name = "txtCaliHighT";
            this.txtCaliHighT.Size = new System.Drawing.Size(102, 21);
            this.txtCaliHighT.TabIndex = 100;
            // 
            // txtSpLowT
            // 
            this.txtSpLowT.Location = new System.Drawing.Point(244, 101);
            this.txtSpLowT.Name = "txtSpLowT";
            this.txtSpLowT.ReadOnly = true;
            this.txtSpLowT.Size = new System.Drawing.Size(102, 21);
            this.txtSpLowT.TabIndex = 99;
            // 
            // txtSpHightT
            // 
            this.txtSpHightT.Location = new System.Drawing.Point(109, 101);
            this.txtSpHightT.Name = "txtSpHightT";
            this.txtSpHightT.ReadOnly = true;
            this.txtSpHightT.Size = new System.Drawing.Size(102, 21);
            this.txtSpHightT.TabIndex = 98;
            // 
            // A_B
            // 
            this.A_B.AutoSize = true;
            this.A_B.Location = new System.Drawing.Point(20, 195);
            this.A_B.Name = "A_B";
            this.A_B.Size = new System.Drawing.Size(47, 12);
            this.A_B.TabIndex = 97;
            this.A_B.Text = "α-β：";
            // 
            // lblMeasureInT
            // 
            this.lblMeasureInT.AutoSize = true;
            this.lblMeasureInT.Location = new System.Drawing.Point(14, 159);
            this.lblMeasureInT.Name = "lblMeasureInT";
            this.lblMeasureInT.Size = new System.Drawing.Size(65, 12);
            this.lblMeasureInT.TabIndex = 96;
            this.lblMeasureInT.Text = "测量强度：";
            // 
            // lblCalibrateInT
            // 
            this.lblCalibrateInT.AutoSize = true;
            this.lblCalibrateInT.Location = new System.Drawing.Point(14, 131);
            this.lblCalibrateInT.Name = "lblCalibrateInT";
            this.lblCalibrateInT.Size = new System.Drawing.Size(65, 12);
            this.lblCalibrateInT.TabIndex = 95;
            this.lblCalibrateInT.Text = "标定强度：";
            // 
            // lblLowSampleT
            // 
            this.lblLowSampleT.AutoSize = true;
            this.lblLowSampleT.Location = new System.Drawing.Point(256, 79);
            this.lblLowSampleT.Name = "lblLowSampleT";
            this.lblLowSampleT.Size = new System.Drawing.Size(17, 12);
            this.lblLowSampleT.TabIndex = 94;
            this.lblLowSampleT.Text = "低";
            // 
            // lblHighSampleT
            // 
            this.lblHighSampleT.AutoSize = true;
            this.lblHighSampleT.Location = new System.Drawing.Point(128, 79);
            this.lblHighSampleT.Name = "lblHighSampleT";
            this.lblHighSampleT.Size = new System.Drawing.Size(17, 12);
            this.lblHighSampleT.TabIndex = 93;
            this.lblHighSampleT.Text = "高";
            // 
            // lblSampleNameT
            // 
            this.lblSampleNameT.AutoSize = true;
            this.lblSampleNameT.Location = new System.Drawing.Point(14, 104);
            this.lblSampleNameT.Name = "lblSampleNameT";
            this.lblSampleNameT.Size = new System.Drawing.Size(65, 12);
            this.lblSampleNameT.TabIndex = 92;
            this.lblSampleNameT.Text = "标样名称：";
            // 
            // lblCaculteType
            // 
            this.lblCaculteType.AutoSize = true;
            this.lblCaculteType.Location = new System.Drawing.Point(14, 31);
            this.lblCaculteType.Name = "lblCaculteType";
            this.lblCaculteType.Size = new System.Drawing.Size(53, 12);
            this.lblCaculteType.TabIndex = 91;
            this.lblCaculteType.Text = "计算方法";
            // 
            // rdobtnA_B
            // 
            this.rdobtnA_B.AutoSize = true;
            this.rdobtnA_B.Font = new System.Drawing.Font("SimSun", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rdobtnA_B.Location = new System.Drawing.Point(176, 28);
            this.rdobtnA_B.Name = "rdobtnA_B";
            this.rdobtnA_B.Size = new System.Drawing.Size(66, 19);
            this.rdobtnA_B.TabIndex = 1;
            this.rdobtnA_B.TabStop = true;
            this.rdobtnA_B.Text = "α+β";
            this.rdobtnA_B.UseVisualStyleBackColor = true;
            this.rdobtnA_B.Click += new System.EventHandler(this.rdobtnA_B_Click);
            // 
            // rdobtnA
            // 
            this.rdobtnA.AutoSize = true;
            this.rdobtnA.Font = new System.Drawing.Font("SimSun", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rdobtnA.Location = new System.Drawing.Point(101, 28);
            this.rdobtnA.Name = "rdobtnA";
            this.rdobtnA.Size = new System.Drawing.Size(41, 19);
            this.rdobtnA.TabIndex = 0;
            this.rdobtnA.TabStop = true;
            this.rdobtnA.Text = "α";
            this.rdobtnA.UseVisualStyleBackColor = true;
            this.rdobtnA.Click += new System.EventHandler(this.rdobtnA_Click);
            // 
            // btnResetLow
            // 
            this.btnResetLow.bSilver = false;
            this.btnResetLow.Location = new System.Drawing.Point(435, 156);
            this.btnResetLow.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnResetLow.MenuPos = new System.Drawing.Point(0, 0);
            this.btnResetLow.Name = "btnResetLow";
            this.btnResetLow.Size = new System.Drawing.Size(136, 23);
            this.btnResetLow.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnResetLow.TabIndex = 107;
            this.btnResetLow.Text = "低标重置";
            this.btnResetLow.ToFocused = false;
            this.btnResetLow.UseVisualStyleBackColor = true;
            this.btnResetLow.Click += new System.EventHandler(this.btnResetLow_Click);
            // 
            // UcInCalibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnResetLow);
            this.Controls.Add(this.grpCalculateType);
            this.Controls.Add(this.lblHighSample);
            this.Controls.Add(this.lblLowSample);
            this.Controls.Add(this.btnSpHCalibrate);
            this.Controls.Add(this.btnSpHMeasure);
            this.Controls.Add(this.btnspLMeasure);
            this.Controls.Add(this.btnSpLCalibrate);
            this.Controls.Add(this.dgvIntensitys);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnResetHigh);
            this.Name = "UcInCalibrate";
            this.Size = new System.Drawing.Size(618, 574);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIntensitys)).EndInit();
            this.grpCalculateType.ResumeLayout(false);
            this.grpCalculateType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btnResetHigh;
        private Skyray.Controls.ButtonW btnSubmit;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.DataGridViewW dgvIntensitys;
        private Skyray.Controls.ButtonW btnSpLCalibrate;
        private Skyray.Controls.ButtonW btnspLMeasure;
        private Skyray.Controls.ButtonW btnSpHCalibrate;
        private Skyray.Controls.ButtonW btnSpHMeasure;
        private Skyray.Controls.LabelW lblLowSample;
        private Skyray.Controls.LabelW lblHighSample;
        private Skyray.Controls.Grouper grpCalculateType;
        private System.Windows.Forms.RadioButton rdobtnA_B;
        private System.Windows.Forms.RadioButton rdobtnA;
        private System.Windows.Forms.Label lblCaculteType;
        private System.Windows.Forms.Label lblSampleNameT;
        private System.Windows.Forms.Label lblMeasureInT;
        private System.Windows.Forms.Label lblCalibrateInT;
        private System.Windows.Forms.Label lblLowSampleT;
        private System.Windows.Forms.Label lblHighSampleT;
        private System.Windows.Forms.Label A_B;
        private System.Windows.Forms.TextBox txtCoefb;
        private System.Windows.Forms.TextBox txtCoefR;
        private System.Windows.Forms.TextBox txtMeasLowT;
        private System.Windows.Forms.TextBox txtMesuHighT;
        private System.Windows.Forms.TextBox txtCaliLowT;
        private System.Windows.Forms.TextBox txtCaliHighT;
        private System.Windows.Forms.TextBox txtSpLowT;
        private System.Windows.Forms.TextBox txtSpHightT;
        private Skyray.Controls.ButtonW btnResetLow;
    }
}
