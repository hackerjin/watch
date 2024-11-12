namespace Skyray.UC
{
    partial class UCSurfaceSource
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
            Skyray.Controls.Office2007BlueRenderer office2007BlueRenderer11 = new Skyray.Controls.Office2007BlueRenderer();
            Skyray.Controls.Office2007BlueRenderer office2007BlueRenderer12 = new Skyray.Controls.Office2007BlueRenderer();
            Skyray.Controls.Office2007BlueRenderer office2007BlueRenderer13 = new Skyray.Controls.Office2007BlueRenderer();
            Skyray.Controls.Office2007BlueRenderer office2007BlueRenderer14 = new Skyray.Controls.Office2007BlueRenderer();
            Skyray.Controls.Office2007BlueRenderer office2007BlueRenderer15 = new Skyray.Controls.Office2007BlueRenderer();
            this.lblSurfaceSourceOne = new Skyray.Controls.LabelW();
            this.lblSurfaceSourceTwo = new Skyray.Controls.LabelW();
            this.scrSurfaceSourceOne = new Skyray.Controls.ScrollBarEx();
            this.scrSurfaceSourceTwo = new Skyray.Controls.ScrollBarEx();
            this.btnSet = new Skyray.Controls.ButtonW();
            this.lblValueOne = new Skyray.Controls.LabelW();
            this.lblValueTwo = new Skyray.Controls.LabelW();
            this.grpMain = new Skyray.Controls.Grouper();
            this.lblValueAll = new Skyray.Controls.LabelW();
            this.lblSurfaceSourceAll = new Skyray.Controls.LabelW();
            this.scrSurfaceSourceAll = new Skyray.Controls.ScrollBarEx();
            this.scrSurfaceSourceThird = new Skyray.Controls.ScrollBarEx();
            this.lblValueFourth = new Skyray.Controls.LabelW();
            this.lblSurfaceSourceThird = new Skyray.Controls.LabelW();
            this.lblValueThird = new Skyray.Controls.LabelW();
            this.lblSurfaceSourceFourth = new Skyray.Controls.LabelW();
            this.scrSurfaceSourceFourth = new Skyray.Controls.ScrollBarEx();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.grpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSurfaceSourceOne
            // 
            this.lblSurfaceSourceOne.AutoSize = true;
            this.lblSurfaceSourceOne.BackColor = System.Drawing.Color.Transparent;
            this.lblSurfaceSourceOne.Location = new System.Drawing.Point(22, 36);
            this.lblSurfaceSourceOne.Name = "lblSurfaceSourceOne";
            this.lblSurfaceSourceOne.Size = new System.Drawing.Size(83, 12);
            this.lblSurfaceSourceOne.TabIndex = 0;
            this.lblSurfaceSourceOne.Text = "设置面光源1：";
            // 
            // lblSurfaceSourceTwo
            // 
            this.lblSurfaceSourceTwo.AutoSize = true;
            this.lblSurfaceSourceTwo.BackColor = System.Drawing.Color.Transparent;
            this.lblSurfaceSourceTwo.Location = new System.Drawing.Point(22, 82);
            this.lblSurfaceSourceTwo.Name = "lblSurfaceSourceTwo";
            this.lblSurfaceSourceTwo.Size = new System.Drawing.Size(83, 12);
            this.lblSurfaceSourceTwo.TabIndex = 1;
            this.lblSurfaceSourceTwo.Text = "设置面光源2：";
            // 
            // scrSurfaceSourceOne
            // 
            this.scrSurfaceSourceOne.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(185)))), ((int)(((byte)(235)))));
            this.scrSurfaceSourceOne.Location = new System.Drawing.Point(151, 31);
            this.scrSurfaceSourceOne.Maximum = 4095;
            this.scrSurfaceSourceOne.Name = "scrSurfaceSourceOne";
            this.scrSurfaceSourceOne.Orientation = Skyray.Controls.ScrollBarOrientation.Horizontal;
            this.scrSurfaceSourceOne.Renderer = office2007BlueRenderer11;
            this.scrSurfaceSourceOne.Size = new System.Drawing.Size(200, 19);
            this.scrSurfaceSourceOne.TabIndex = 2;
            this.scrSurfaceSourceOne.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrSurfaceSource_Scroll);
            // 
            // scrSurfaceSourceTwo
            // 
            this.scrSurfaceSourceTwo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(185)))), ((int)(((byte)(235)))));
            this.scrSurfaceSourceTwo.Location = new System.Drawing.Point(151, 78);
            this.scrSurfaceSourceTwo.Maximum = 4095;
            this.scrSurfaceSourceTwo.Name = "scrSurfaceSourceTwo";
            this.scrSurfaceSourceTwo.Orientation = Skyray.Controls.ScrollBarOrientation.Horizontal;
            this.scrSurfaceSourceTwo.Renderer = office2007BlueRenderer12;
            this.scrSurfaceSourceTwo.Size = new System.Drawing.Size(200, 19);
            this.scrSurfaceSourceTwo.TabIndex = 3;
            this.scrSurfaceSourceTwo.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrSurfaceSource_Scroll);
            // 
            // btnSet
            // 
            this.btnSet.bSilver = false;
            this.btnSet.Location = new System.Drawing.Point(85, 301);
            this.btnSet.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSet.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(108, 23);
            this.btnSet.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSet.TabIndex = 4;
            this.btnSet.Text = "设置";
            this.btnSet.ToFocused = false;
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // lblValueOne
            // 
            this.lblValueOne.AutoSize = true;
            this.lblValueOne.BackColor = System.Drawing.Color.Transparent;
            this.lblValueOne.Location = new System.Drawing.Point(368, 36);
            this.lblValueOne.Name = "lblValueOne";
            this.lblValueOne.Size = new System.Drawing.Size(11, 12);
            this.lblValueOne.TabIndex = 5;
            this.lblValueOne.Text = "1";
            // 
            // lblValueTwo
            // 
            this.lblValueTwo.AutoSize = true;
            this.lblValueTwo.BackColor = System.Drawing.Color.Transparent;
            this.lblValueTwo.Location = new System.Drawing.Point(368, 84);
            this.lblValueTwo.Name = "lblValueTwo";
            this.lblValueTwo.Size = new System.Drawing.Size(11, 12);
            this.lblValueTwo.TabIndex = 6;
            this.lblValueTwo.Text = "2";
            // 
            // grpMain
            // 
            this.grpMain.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpMain.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpMain.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpMain.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpMain.BorderThickness = 1F;
            this.grpMain.BorderTopOnly = false;
            this.grpMain.Controls.Add(this.lblValueAll);
            this.grpMain.Controls.Add(this.lblSurfaceSourceAll);
            this.grpMain.Controls.Add(this.scrSurfaceSourceAll);
            this.grpMain.Controls.Add(this.scrSurfaceSourceThird);
            this.grpMain.Controls.Add(this.lblValueFourth);
            this.grpMain.Controls.Add(this.lblSurfaceSourceThird);
            this.grpMain.Controls.Add(this.lblValueThird);
            this.grpMain.Controls.Add(this.lblSurfaceSourceFourth);
            this.grpMain.Controls.Add(this.scrSurfaceSourceFourth);
            this.grpMain.Controls.Add(this.scrSurfaceSourceOne);
            this.grpMain.Controls.Add(this.lblValueTwo);
            this.grpMain.Controls.Add(this.lblSurfaceSourceOne);
            this.grpMain.Controls.Add(this.lblValueOne);
            this.grpMain.Controls.Add(this.lblSurfaceSourceTwo);
            this.grpMain.Controls.Add(this.scrSurfaceSourceTwo);
            this.grpMain.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpMain.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grpMain.GroupImage = null;
            this.grpMain.GroupTitle = "";
            this.grpMain.HeaderRoundCorners = 4;
            this.grpMain.Location = new System.Drawing.Point(11, 11);
            this.grpMain.Name = "grpMain";
            this.grpMain.PaintGroupBox = false;
            this.grpMain.RoundCorners = 4;
            this.grpMain.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpMain.ShadowControl = false;
            this.grpMain.ShadowThickness = 3;
            this.grpMain.Size = new System.Drawing.Size(418, 260);
            this.grpMain.TabIndex = 7;
            this.grpMain.TextLineSpace = 2;
            this.grpMain.TitleLeftSpace = 18;
            // 
            // lblValueAll
            // 
            this.lblValueAll.AutoSize = true;
            this.lblValueAll.BackColor = System.Drawing.Color.Transparent;
            this.lblValueAll.Location = new System.Drawing.Point(368, 221);
            this.lblValueAll.Name = "lblValueAll";
            this.lblValueAll.Size = new System.Drawing.Size(11, 12);
            this.lblValueAll.TabIndex = 15;
            this.lblValueAll.Text = "5";
            // 
            // lblSurfaceSourceAll
            // 
            this.lblSurfaceSourceAll.AutoSize = true;
            this.lblSurfaceSourceAll.BackColor = System.Drawing.Color.Transparent;
            this.lblSurfaceSourceAll.Location = new System.Drawing.Point(22, 219);
            this.lblSurfaceSourceAll.Name = "lblSurfaceSourceAll";
            this.lblSurfaceSourceAll.Size = new System.Drawing.Size(89, 12);
            this.lblSurfaceSourceAll.TabIndex = 13;
            this.lblSurfaceSourceAll.Text = "设置全部光源：";
            // 
            // scrSurfaceSourceAll
            // 
            this.scrSurfaceSourceAll.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(185)))), ((int)(((byte)(235)))));
            this.scrSurfaceSourceAll.Location = new System.Drawing.Point(151, 215);
            this.scrSurfaceSourceAll.Maximum = 4095;
            this.scrSurfaceSourceAll.Name = "scrSurfaceSourceAll";
            this.scrSurfaceSourceAll.Orientation = Skyray.Controls.ScrollBarOrientation.Horizontal;
            this.scrSurfaceSourceAll.Renderer = office2007BlueRenderer13;
            this.scrSurfaceSourceAll.Size = new System.Drawing.Size(200, 19);
            this.scrSurfaceSourceAll.TabIndex = 14;
            this.scrSurfaceSourceAll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrSurfaceSourceAll_Scroll);
            // 
            // scrSurfaceSourceThird
            // 
            this.scrSurfaceSourceThird.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(185)))), ((int)(((byte)(235)))));
            this.scrSurfaceSourceThird.Location = new System.Drawing.Point(151, 123);
            this.scrSurfaceSourceThird.Maximum = 4095;
            this.scrSurfaceSourceThird.Name = "scrSurfaceSourceThird";
            this.scrSurfaceSourceThird.Orientation = Skyray.Controls.ScrollBarOrientation.Horizontal;
            this.scrSurfaceSourceThird.Renderer = office2007BlueRenderer14;
            this.scrSurfaceSourceThird.Size = new System.Drawing.Size(200, 19);
            this.scrSurfaceSourceThird.TabIndex = 9;
            this.scrSurfaceSourceThird.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrSurfaceSource_Scroll);
            // 
            // lblValueFourth
            // 
            this.lblValueFourth.AutoSize = true;
            this.lblValueFourth.BackColor = System.Drawing.Color.Transparent;
            this.lblValueFourth.Location = new System.Drawing.Point(368, 176);
            this.lblValueFourth.Name = "lblValueFourth";
            this.lblValueFourth.Size = new System.Drawing.Size(11, 12);
            this.lblValueFourth.TabIndex = 12;
            this.lblValueFourth.Text = "4";
            // 
            // lblSurfaceSourceThird
            // 
            this.lblSurfaceSourceThird.AutoSize = true;
            this.lblSurfaceSourceThird.BackColor = System.Drawing.Color.Transparent;
            this.lblSurfaceSourceThird.Location = new System.Drawing.Point(22, 128);
            this.lblSurfaceSourceThird.Name = "lblSurfaceSourceThird";
            this.lblSurfaceSourceThird.Size = new System.Drawing.Size(83, 12);
            this.lblSurfaceSourceThird.TabIndex = 7;
            this.lblSurfaceSourceThird.Text = "设置面光源3：";
            // 
            // lblValueThird
            // 
            this.lblValueThird.AutoSize = true;
            this.lblValueThird.BackColor = System.Drawing.Color.Transparent;
            this.lblValueThird.Location = new System.Drawing.Point(368, 128);
            this.lblValueThird.Name = "lblValueThird";
            this.lblValueThird.Size = new System.Drawing.Size(11, 12);
            this.lblValueThird.TabIndex = 11;
            this.lblValueThird.Text = "3";
            // 
            // lblSurfaceSourceFourth
            // 
            this.lblSurfaceSourceFourth.AutoSize = true;
            this.lblSurfaceSourceFourth.BackColor = System.Drawing.Color.Transparent;
            this.lblSurfaceSourceFourth.Location = new System.Drawing.Point(22, 174);
            this.lblSurfaceSourceFourth.Name = "lblSurfaceSourceFourth";
            this.lblSurfaceSourceFourth.Size = new System.Drawing.Size(83, 12);
            this.lblSurfaceSourceFourth.TabIndex = 8;
            this.lblSurfaceSourceFourth.Text = "设置面光源4：";
            // 
            // scrSurfaceSourceFourth
            // 
            this.scrSurfaceSourceFourth.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(185)))), ((int)(((byte)(235)))));
            this.scrSurfaceSourceFourth.Location = new System.Drawing.Point(151, 170);
            this.scrSurfaceSourceFourth.Maximum = 4095;
            this.scrSurfaceSourceFourth.Name = "scrSurfaceSourceFourth";
            this.scrSurfaceSourceFourth.Orientation = Skyray.Controls.ScrollBarOrientation.Horizontal;
            this.scrSurfaceSourceFourth.Renderer = office2007BlueRenderer15;
            this.scrSurfaceSourceFourth.Size = new System.Drawing.Size(200, 19);
            this.scrSurfaceSourceFourth.TabIndex = 10;
            this.scrSurfaceSourceFourth.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrSurfaceSource_Scroll);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(239, 301);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // UCSurfaceSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpMain);
            this.Controls.Add(this.btnSet);
            this.Name = "UCSurfaceSource";
            this.Size = new System.Drawing.Size(432, 335);
            this.Load += new System.EventHandler(this.UCSurfaceSource_Load);
            this.grpMain.ResumeLayout(false);
            this.grpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.LabelW lblSurfaceSourceOne;
        private Skyray.Controls.LabelW lblSurfaceSourceTwo;
        private Skyray.Controls.ScrollBarEx scrSurfaceSourceOne;
        private Skyray.Controls.ScrollBarEx scrSurfaceSourceTwo;
        private Skyray.Controls.ButtonW btnSet;
        private Skyray.Controls.LabelW lblValueOne;
        private Skyray.Controls.LabelW lblValueTwo;
        private Skyray.Controls.Grouper grpMain;
        private Skyray.Controls.LabelW lblValueAll;
        private Skyray.Controls.LabelW lblSurfaceSourceAll;
        private Skyray.Controls.ScrollBarEx scrSurfaceSourceAll;
        private Skyray.Controls.ScrollBarEx scrSurfaceSourceThird;
        private Skyray.Controls.LabelW lblValueFourth;
        private Skyray.Controls.LabelW lblSurfaceSourceThird;
        private Skyray.Controls.LabelW lblValueThird;
        private Skyray.Controls.LabelW lblSurfaceSourceFourth;
        private Skyray.Controls.ScrollBarEx scrSurfaceSourceFourth;
        private Skyray.Controls.ButtonW btnOK;
    }
}
