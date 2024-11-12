namespace Skyray.UC
{
    partial class UCCoeeParamIntel
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
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.grpEscSpec = new Skyray.Controls.Grouper();
            this.dblEscFactor = new Skyray.Controls.DoubleInputW();
            this.lblEscFactor = new Skyray.Controls.LabelW();
            this.dblEscAngle = new Skyray.Controls.DoubleInputW();
            this.lblEscAngle = new Skyray.Controls.LabelW();
            this.chkEscSpec = new Skyray.Controls.CheckBoxW();
            this.grpSumSpec = new Skyray.Controls.Grouper();
            this.dblSumFactor = new Skyray.Controls.DoubleInputW();
            this.lblSumFactor = new Skyray.Controls.LabelW();
            this.dblSumFwhm = new Skyray.Controls.DoubleInputW();
            this.lblSumFwhm = new Skyray.Controls.LabelW();
            this.chkSumSpec = new Skyray.Controls.CheckBoxW();
            this.grpRemoveBase = new Skyray.Controls.Grouper();
            this.labelW1Line = new Skyray.Controls.LabelW();
            this.dblRemoveBase4Right = new Skyray.Controls.DoubleInputW();
            this.dblRemoveBase4Left = new Skyray.Controls.DoubleInputW();
            this.lblRange = new Skyray.Controls.LabelW();
            this.numRemoveBase4Times = new Skyray.Controls.NumricUpDownW();
            this.lblRemoveBase4Times = new Skyray.Controls.LabelW();
            this.chkRemoveBase4 = new Skyray.Controls.CheckBoxW();
            this.txtRemoveBase3Points = new Skyray.Controls.TextBoxW();
            this.lblRemoveBase3Points = new Skyray.Controls.LabelW();
            this.dblRemoveBase2Factor = new Skyray.Controls.DoubleInputW();
            this.lblRemoveBase2Factor = new Skyray.Controls.LabelW();
            this.numRemoveBase2Times = new Skyray.Controls.NumricUpDownW();
            this.lblRemoveBase2Times = new Skyray.Controls.LabelW();
            this.numRemoveBase1Times = new Skyray.Controls.NumricUpDownW();
            this.lblRemoveBase1Times = new Skyray.Controls.LabelW();
            this.chkRemoveBase3 = new Skyray.Controls.CheckBoxW();
            this.chkRemoveBase2 = new Skyray.Controls.CheckBoxW();
            this.chkRemoveBase1 = new Skyray.Controls.CheckBoxW();
            this.grpEscSpec.SuspendLayout();
            this.grpSumSpec.SuspendLayout();
            this.grpRemoveBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoveBase4Times)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoveBase2Times)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoveBase1Times)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(368, 414);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(262, 415);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(92, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpEscSpec
            // 
            this.grpEscSpec.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpEscSpec.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpEscSpec.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpEscSpec.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpEscSpec.BorderThickness = 1F;
            this.grpEscSpec.BorderTopOnly = false;
            this.grpEscSpec.Controls.Add(this.dblEscFactor);
            this.grpEscSpec.Controls.Add(this.lblEscFactor);
            this.grpEscSpec.Controls.Add(this.dblEscAngle);
            this.grpEscSpec.Controls.Add(this.lblEscAngle);
            this.grpEscSpec.Controls.Add(this.chkEscSpec);
            this.grpEscSpec.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpEscSpec.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpEscSpec.GroupImage = null;
            this.grpEscSpec.GroupTitle = "逃逸峰";
            this.grpEscSpec.HeaderRoundCorners = 4;
            this.grpEscSpec.Location = new System.Drawing.Point(19, 4);
            this.grpEscSpec.Name = "grpEscSpec";
            this.grpEscSpec.PaintGroupBox = false;
            this.grpEscSpec.RoundCorners = 4;
            this.grpEscSpec.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpEscSpec.ShadowControl = false;
            this.grpEscSpec.ShadowThickness = 3;
            this.grpEscSpec.Size = new System.Drawing.Size(443, 87);
            this.grpEscSpec.TabIndex = 8;
            this.grpEscSpec.TextLineSpace = 2;
            this.grpEscSpec.TitleLeftSpace = 18;
            // 
            // dblEscFactor
            // 
            this.dblEscFactor.BorderColor = System.Drawing.Color.Empty;
            this.dblEscFactor.Location = new System.Drawing.Point(321, 37);
            this.dblEscFactor.Name = "dblEscFactor";
            this.dblEscFactor.Size = new System.Drawing.Size(87, 21);
            this.dblEscFactor.TabIndex = 4;
            // 
            // lblEscFactor
            // 
            this.lblEscFactor.AutoSize = true;
            this.lblEscFactor.BackColor = System.Drawing.Color.Transparent;
            this.lblEscFactor.Location = new System.Drawing.Point(301, 18);
            this.lblEscFactor.Name = "lblEscFactor";
            this.lblEscFactor.Size = new System.Drawing.Size(29, 12);
            this.lblEscFactor.TabIndex = 3;
            this.lblEscFactor.Text = "因子";
            // 
            // dblEscAngle
            // 
            this.dblEscAngle.BorderColor = System.Drawing.Color.Empty;
            this.dblEscAngle.Location = new System.Drawing.Point(177, 37);
            this.dblEscAngle.Name = "dblEscAngle";
            this.dblEscAngle.Size = new System.Drawing.Size(87, 21);
            this.dblEscAngle.TabIndex = 2;
            this.dblEscAngle.TextChanged += new System.EventHandler(this.dblEscAngle_TextChanged);
            // 
            // lblEscAngle
            // 
            this.lblEscAngle.AutoSize = true;
            this.lblEscAngle.BackColor = System.Drawing.Color.Transparent;
            this.lblEscAngle.Location = new System.Drawing.Point(154, 18);
            this.lblEscAngle.Name = "lblEscAngle";
            this.lblEscAngle.Size = new System.Drawing.Size(29, 12);
            this.lblEscAngle.TabIndex = 1;
            this.lblEscAngle.Text = "角度";
            // 
            // chkEscSpec
            // 
            this.chkEscSpec.AutoSize = true;
            this.chkEscSpec.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkEscSpec.Location = new System.Drawing.Point(37, 39);
            this.chkEscSpec.Name = "chkEscSpec";
            this.chkEscSpec.Size = new System.Drawing.Size(48, 16);
            this.chkEscSpec.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkEscSpec.TabIndex = 0;
            this.chkEscSpec.Text = "处理";
            this.chkEscSpec.UseVisualStyleBackColor = true;
            // 
            // grpSumSpec
            // 
            this.grpSumSpec.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpSumSpec.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpSumSpec.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpSumSpec.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpSumSpec.BorderThickness = 1F;
            this.grpSumSpec.BorderTopOnly = false;
            this.grpSumSpec.Controls.Add(this.dblSumFactor);
            this.grpSumSpec.Controls.Add(this.lblSumFactor);
            this.grpSumSpec.Controls.Add(this.dblSumFwhm);
            this.grpSumSpec.Controls.Add(this.lblSumFwhm);
            this.grpSumSpec.Controls.Add(this.chkSumSpec);
            this.grpSumSpec.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpSumSpec.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpSumSpec.GroupImage = null;
            this.grpSumSpec.GroupTitle = "和峰";
            this.grpSumSpec.HeaderRoundCorners = 4;
            this.grpSumSpec.Location = new System.Drawing.Point(19, 91);
            this.grpSumSpec.Name = "grpSumSpec";
            this.grpSumSpec.PaintGroupBox = false;
            this.grpSumSpec.RoundCorners = 4;
            this.grpSumSpec.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpSumSpec.ShadowControl = false;
            this.grpSumSpec.ShadowThickness = 3;
            this.grpSumSpec.Size = new System.Drawing.Size(443, 84);
            this.grpSumSpec.TabIndex = 9;
            this.grpSumSpec.TextLineSpace = 2;
            this.grpSumSpec.TitleLeftSpace = 18;
            // 
            // dblSumFactor
            // 
            this.dblSumFactor.BorderColor = System.Drawing.Color.Empty;
            this.dblSumFactor.Location = new System.Drawing.Point(322, 39);
            this.dblSumFactor.Name = "dblSumFactor";
            this.dblSumFactor.Size = new System.Drawing.Size(87, 21);
            this.dblSumFactor.TabIndex = 9;
            // 
            // lblSumFactor
            // 
            this.lblSumFactor.AutoSize = true;
            this.lblSumFactor.BackColor = System.Drawing.Color.Transparent;
            this.lblSumFactor.Location = new System.Drawing.Point(301, 20);
            this.lblSumFactor.Name = "lblSumFactor";
            this.lblSumFactor.Size = new System.Drawing.Size(29, 12);
            this.lblSumFactor.TabIndex = 8;
            this.lblSumFactor.Text = "因子";
            // 
            // dblSumFwhm
            // 
            this.dblSumFwhm.BorderColor = System.Drawing.Color.Empty;
            this.dblSumFwhm.Location = new System.Drawing.Point(178, 39);
            this.dblSumFwhm.Name = "dblSumFwhm";
            this.dblSumFwhm.Size = new System.Drawing.Size(87, 21);
            this.dblSumFwhm.TabIndex = 7;
            this.dblSumFwhm.TextChanged += new System.EventHandler(this.dblSumFwhm_TextChanged);
            // 
            // lblSumFwhm
            // 
            this.lblSumFwhm.AutoSize = true;
            this.lblSumFwhm.BackColor = System.Drawing.Color.Transparent;
            this.lblSumFwhm.Location = new System.Drawing.Point(154, 20);
            this.lblSumFwhm.Name = "lblSumFwhm";
            this.lblSumFwhm.Size = new System.Drawing.Size(101, 12);
            this.lblSumFwhm.TabIndex = 6;
            this.lblSumFwhm.Text = "脉冲对分辨率(ms)";
            // 
            // chkSumSpec
            // 
            this.chkSumSpec.AutoSize = true;
            this.chkSumSpec.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkSumSpec.Location = new System.Drawing.Point(37, 41);
            this.chkSumSpec.Name = "chkSumSpec";
            this.chkSumSpec.Size = new System.Drawing.Size(48, 16);
            this.chkSumSpec.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkSumSpec.TabIndex = 5;
            this.chkSumSpec.Text = "处理";
            this.chkSumSpec.UseVisualStyleBackColor = true;
            // 
            // grpRemoveBase
            // 
            this.grpRemoveBase.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpRemoveBase.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpRemoveBase.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpRemoveBase.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpRemoveBase.BorderThickness = 1F;
            this.grpRemoveBase.BorderTopOnly = false;
            this.grpRemoveBase.Controls.Add(this.labelW1Line);
            this.grpRemoveBase.Controls.Add(this.dblRemoveBase4Right);
            this.grpRemoveBase.Controls.Add(this.dblRemoveBase4Left);
            this.grpRemoveBase.Controls.Add(this.lblRange);
            this.grpRemoveBase.Controls.Add(this.numRemoveBase4Times);
            this.grpRemoveBase.Controls.Add(this.lblRemoveBase4Times);
            this.grpRemoveBase.Controls.Add(this.chkRemoveBase4);
            this.grpRemoveBase.Controls.Add(this.txtRemoveBase3Points);
            this.grpRemoveBase.Controls.Add(this.lblRemoveBase3Points);
            this.grpRemoveBase.Controls.Add(this.dblRemoveBase2Factor);
            this.grpRemoveBase.Controls.Add(this.lblRemoveBase2Factor);
            this.grpRemoveBase.Controls.Add(this.numRemoveBase2Times);
            this.grpRemoveBase.Controls.Add(this.lblRemoveBase2Times);
            this.grpRemoveBase.Controls.Add(this.numRemoveBase1Times);
            this.grpRemoveBase.Controls.Add(this.lblRemoveBase1Times);
            this.grpRemoveBase.Controls.Add(this.chkRemoveBase3);
            this.grpRemoveBase.Controls.Add(this.chkRemoveBase2);
            this.grpRemoveBase.Controls.Add(this.chkRemoveBase1);
            this.grpRemoveBase.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpRemoveBase.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpRemoveBase.GroupImage = null;
            this.grpRemoveBase.GroupTitle = "本底";
            this.grpRemoveBase.HeaderRoundCorners = 4;
            this.grpRemoveBase.Location = new System.Drawing.Point(19, 176);
            this.grpRemoveBase.Name = "grpRemoveBase";
            this.grpRemoveBase.PaintGroupBox = false;
            this.grpRemoveBase.RoundCorners = 4;
            this.grpRemoveBase.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpRemoveBase.ShadowControl = false;
            this.grpRemoveBase.ShadowThickness = 3;
            this.grpRemoveBase.Size = new System.Drawing.Size(443, 230);
            this.grpRemoveBase.TabIndex = 10;
            this.grpRemoveBase.TextLineSpace = 2;
            this.grpRemoveBase.TitleLeftSpace = 18;
            // 
            // labelW1Line
            // 
            this.labelW1Line.AutoSize = true;
            this.labelW1Line.BackColor = System.Drawing.Color.Transparent;
            this.labelW1Line.Location = new System.Drawing.Point(347, 199);
            this.labelW1Line.Name = "labelW1Line";
            this.labelW1Line.Size = new System.Drawing.Size(11, 12);
            this.labelW1Line.TabIndex = 24;
            this.labelW1Line.Text = "-";
            // 
            // dblRemoveBase4Right
            // 
            this.dblRemoveBase4Right.BorderColor = System.Drawing.Color.Empty;
            this.dblRemoveBase4Right.DecimalDigits = 0;
            this.dblRemoveBase4Right.Location = new System.Drawing.Point(360, 196);
            this.dblRemoveBase4Right.Name = "dblRemoveBase4Right";
            this.dblRemoveBase4Right.Size = new System.Drawing.Size(62, 21);
            this.dblRemoveBase4Right.TabIndex = 23;
            this.dblRemoveBase4Right.TextChanged += new System.EventHandler(this.dblRemoveBase4Right_TextChanged);
            // 
            // dblRemoveBase4Left
            // 
            this.dblRemoveBase4Left.BorderColor = System.Drawing.Color.Empty;
            this.dblRemoveBase4Left.DecimalDigits = 0;
            this.dblRemoveBase4Left.Location = new System.Drawing.Point(290, 196);
            this.dblRemoveBase4Left.Name = "dblRemoveBase4Left";
            this.dblRemoveBase4Left.Size = new System.Drawing.Size(53, 21);
            this.dblRemoveBase4Left.TabIndex = 21;
            this.dblRemoveBase4Left.TextChanged += new System.EventHandler(this.dblRemoveBase4Left_TextChanged);
            // 
            // lblRange
            // 
            this.lblRange.AutoSize = true;
            this.lblRange.BackColor = System.Drawing.Color.Transparent;
            this.lblRange.Location = new System.Drawing.Point(277, 177);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(119, 12);
            this.lblRange.TabIndex = 20;
            this.lblRange.Text = "扣本底4范围(1-谱长)";
            // 
            // numRemoveBase4Times
            // 
            this.numRemoveBase4Times.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numRemoveBase4Times.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numRemoveBase4Times.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numRemoveBase4Times.Location = new System.Drawing.Point(178, 196);
            this.numRemoveBase4Times.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numRemoveBase4Times.Name = "numRemoveBase4Times";
            this.numRemoveBase4Times.Size = new System.Drawing.Size(87, 21);
            this.numRemoveBase4Times.TabIndex = 19;
            // 
            // lblRemoveBase4Times
            // 
            this.lblRemoveBase4Times.AutoSize = true;
            this.lblRemoveBase4Times.BackColor = System.Drawing.Color.Transparent;
            this.lblRemoveBase4Times.Location = new System.Drawing.Point(151, 177);
            this.lblRemoveBase4Times.Name = "lblRemoveBase4Times";
            this.lblRemoveBase4Times.Size = new System.Drawing.Size(101, 12);
            this.lblRemoveBase4Times.TabIndex = 18;
            this.lblRemoveBase4Times.Text = "扣本底4次数(1-3)";
            // 
            // chkRemoveBase4
            // 
            this.chkRemoveBase4.AutoSize = true;
            this.chkRemoveBase4.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkRemoveBase4.Location = new System.Drawing.Point(37, 198);
            this.chkRemoveBase4.Name = "chkRemoveBase4";
            this.chkRemoveBase4.Size = new System.Drawing.Size(126, 16);
            this.chkRemoveBase4.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkRemoveBase4.TabIndex = 17;
            this.chkRemoveBase4.Text = "扣本底4(基线校正)";
            this.chkRemoveBase4.UseVisualStyleBackColor = true;
            this.chkRemoveBase4.Click += new System.EventHandler(this.chkRemoveBase4_Click);
            // 
            // txtRemoveBase3Points
            // 
            this.txtRemoveBase3Points.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtRemoveBase3Points.Location = new System.Drawing.Point(177, 153);
            this.txtRemoveBase3Points.Name = "txtRemoveBase3Points";
            this.txtRemoveBase3Points.Size = new System.Drawing.Size(227, 21);
            this.txtRemoveBase3Points.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtRemoveBase3Points.TabIndex = 16;
            this.txtRemoveBase3Points.TextChanged += new System.EventHandler(this.txtRemoveBase3Points_TextChanged);
            // 
            // lblRemoveBase3Points
            // 
            this.lblRemoveBase3Points.AutoSize = true;
            this.lblRemoveBase3Points.BackColor = System.Drawing.Color.Transparent;
            this.lblRemoveBase3Points.Location = new System.Drawing.Point(152, 137);
            this.lblRemoveBase3Points.Name = "lblRemoveBase3Points";
            this.lblRemoveBase3Points.Size = new System.Drawing.Size(173, 12);
            this.lblRemoveBase3Points.TabIndex = 15;
            this.lblRemoveBase3Points.Text = "背景点(用\",\"相隔,参考:1,2,3)";
            // 
            // dblRemoveBase2Factor
            // 
            this.dblRemoveBase2Factor.BorderColor = System.Drawing.Color.Empty;
            this.dblRemoveBase2Factor.Location = new System.Drawing.Point(322, 102);
            this.dblRemoveBase2Factor.Name = "dblRemoveBase2Factor";
            this.dblRemoveBase2Factor.Size = new System.Drawing.Size(87, 21);
            this.dblRemoveBase2Factor.TabIndex = 14;
            // 
            // lblRemoveBase2Factor
            // 
            this.lblRemoveBase2Factor.AutoSize = true;
            this.lblRemoveBase2Factor.BackColor = System.Drawing.Color.Transparent;
            this.lblRemoveBase2Factor.Location = new System.Drawing.Point(299, 84);
            this.lblRemoveBase2Factor.Name = "lblRemoveBase2Factor";
            this.lblRemoveBase2Factor.Size = new System.Drawing.Size(29, 12);
            this.lblRemoveBase2Factor.TabIndex = 13;
            this.lblRemoveBase2Factor.Text = "因子";
            // 
            // numRemoveBase2Times
            // 
            this.numRemoveBase2Times.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numRemoveBase2Times.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numRemoveBase2Times.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numRemoveBase2Times.Location = new System.Drawing.Point(177, 102);
            this.numRemoveBase2Times.Name = "numRemoveBase2Times";
            this.numRemoveBase2Times.Size = new System.Drawing.Size(87, 21);
            this.numRemoveBase2Times.TabIndex = 12;
            // 
            // lblRemoveBase2Times
            // 
            this.lblRemoveBase2Times.AutoSize = true;
            this.lblRemoveBase2Times.BackColor = System.Drawing.Color.Transparent;
            this.lblRemoveBase2Times.Location = new System.Drawing.Point(152, 84);
            this.lblRemoveBase2Times.Name = "lblRemoveBase2Times";
            this.lblRemoveBase2Times.Size = new System.Drawing.Size(71, 12);
            this.lblRemoveBase2Times.TabIndex = 11;
            this.lblRemoveBase2Times.Text = "扣本底2次数";
            // 
            // numRemoveBase1Times
            // 
            this.numRemoveBase1Times.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numRemoveBase1Times.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numRemoveBase1Times.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numRemoveBase1Times.Location = new System.Drawing.Point(177, 49);
            this.numRemoveBase1Times.Name = "numRemoveBase1Times";
            this.numRemoveBase1Times.Size = new System.Drawing.Size(87, 21);
            this.numRemoveBase1Times.TabIndex = 10;
            // 
            // lblRemoveBase1Times
            // 
            this.lblRemoveBase1Times.AutoSize = true;
            this.lblRemoveBase1Times.BackColor = System.Drawing.Color.Transparent;
            this.lblRemoveBase1Times.Location = new System.Drawing.Point(151, 31);
            this.lblRemoveBase1Times.Name = "lblRemoveBase1Times";
            this.lblRemoveBase1Times.Size = new System.Drawing.Size(71, 12);
            this.lblRemoveBase1Times.TabIndex = 9;
            this.lblRemoveBase1Times.Text = "扣本底1次数";
            // 
            // chkRemoveBase3
            // 
            this.chkRemoveBase3.AutoSize = true;
            this.chkRemoveBase3.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkRemoveBase3.Location = new System.Drawing.Point(37, 155);
            this.chkRemoveBase3.Name = "chkRemoveBase3";
            this.chkRemoveBase3.Size = new System.Drawing.Size(114, 16);
            this.chkRemoveBase3.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkRemoveBase3.TabIndex = 8;
            this.chkRemoveBase3.Text = "扣本底3(折线法)";
            this.chkRemoveBase3.UseVisualStyleBackColor = true;
            this.chkRemoveBase3.Click += new System.EventHandler(this.chkRemoveBase4_Click);
            // 
            // chkRemoveBase2
            // 
            this.chkRemoveBase2.AutoSize = true;
            this.chkRemoveBase2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkRemoveBase2.Location = new System.Drawing.Point(37, 104);
            this.chkRemoveBase2.Name = "chkRemoveBase2";
            this.chkRemoveBase2.Size = new System.Drawing.Size(126, 16);
            this.chkRemoveBase2.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkRemoveBase2.TabIndex = 7;
            this.chkRemoveBase2.Text = "扣本底2(低分辨率)";
            this.chkRemoveBase2.UseVisualStyleBackColor = true;
            this.chkRemoveBase2.Click += new System.EventHandler(this.chkRemoveBase4_Click);
            // 
            // chkRemoveBase1
            // 
            this.chkRemoveBase1.AutoSize = true;
            this.chkRemoveBase1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkRemoveBase1.Location = new System.Drawing.Point(37, 51);
            this.chkRemoveBase1.Name = "chkRemoveBase1";
            this.chkRemoveBase1.Size = new System.Drawing.Size(126, 16);
            this.chkRemoveBase1.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkRemoveBase1.TabIndex = 6;
            this.chkRemoveBase1.Text = "扣本底1(高分辨率)";
            this.chkRemoveBase1.UseVisualStyleBackColor = true;
            this.chkRemoveBase1.Click += new System.EventHandler(this.chkRemoveBase4_Click);
            // 
            // UCCoeeParamIntel
            // 
            this.Controls.Add(this.grpRemoveBase);
            this.Controls.Add(this.grpSumSpec);
            this.Controls.Add(this.grpEscSpec);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "UCCoeeParamIntel";
            this.Size = new System.Drawing.Size(481, 446);
            this.Load += new System.EventHandler(this.UCCoeeParamIntel_Load);
            this.grpEscSpec.ResumeLayout(false);
            this.grpEscSpec.PerformLayout();
            this.grpSumSpec.ResumeLayout(false);
            this.grpSumSpec.PerformLayout();
            this.grpRemoveBase.ResumeLayout(false);
            this.grpRemoveBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoveBase4Times)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoveBase2Times)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoveBase1Times)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.Grouper grpEscSpec;
        private Skyray.Controls.Grouper grpSumSpec;
        private Skyray.Controls.Grouper grpRemoveBase;
        private Skyray.Controls.CheckBoxW chkEscSpec;
        private Skyray.Controls.DoubleInputW dblEscAngle;
        private Skyray.Controls.LabelW lblEscAngle;
        private Skyray.Controls.LabelW lblEscFactor;
        private Skyray.Controls.DoubleInputW dblEscFactor;
        private Skyray.Controls.DoubleInputW dblSumFactor;
        private Skyray.Controls.LabelW lblSumFactor;
        private Skyray.Controls.DoubleInputW dblSumFwhm;
        private Skyray.Controls.LabelW lblSumFwhm;
        private Skyray.Controls.CheckBoxW chkSumSpec;
        private Skyray.Controls.CheckBoxW chkRemoveBase3;
        private Skyray.Controls.CheckBoxW chkRemoveBase2;
        private Skyray.Controls.CheckBoxW chkRemoveBase1;
        private Skyray.Controls.DoubleInputW dblRemoveBase2Factor;
        private Skyray.Controls.LabelW lblRemoveBase2Factor;
        private Skyray.Controls.NumricUpDownW numRemoveBase2Times;
        private Skyray.Controls.LabelW lblRemoveBase2Times;
        private Skyray.Controls.NumricUpDownW numRemoveBase1Times;
        private Skyray.Controls.LabelW lblRemoveBase1Times;
        private Skyray.Controls.TextBoxW txtRemoveBase3Points;
        private Skyray.Controls.LabelW lblRemoveBase3Points;
        private Skyray.Controls.CheckBoxW chkRemoveBase4;
        private Skyray.Controls.DoubleInputW dblRemoveBase4Right;
        private Skyray.Controls.DoubleInputW dblRemoveBase4Left;
        private Skyray.Controls.LabelW lblRange;
        private Skyray.Controls.NumricUpDownW numRemoveBase4Times;
        private Skyray.Controls.LabelW lblRemoveBase4Times;
        private Skyray.Controls.LabelW labelW1Line;
    }
}
