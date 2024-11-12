namespace Skyray.Camera
{
    partial class SkyrayCamera
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
            this.components = new System.ComponentModel.Container();
            this.cmsCamera = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCameraProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCameraFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCameraParam = new System.Windows.Forms.ToolStripMenuItem();
            this.AutoSaveSamplePicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGraphy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowAllTestPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveMultiPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveOutMultiPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEditMultiPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelCurrentFlag = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelLastFlag = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelAllFlag = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSetMoveRate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiX = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMoveRateValueX = new System.Windows.Forms.ToolStripTextBox();
            this.tsmiY = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMoveRateValueY = new System.Windows.Forms.ToolStripTextBox();
            this.tsmiCoeeGraphWidthHeight = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAdjustOrient = new System.Windows.Forms.ToolStripMenuItem();
            this.lblCameraInfo = new System.Windows.Forms.Label();
            this.savegraph = new System.Windows.Forms.SaveFileDialog();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblTestInfomation = new System.Windows.Forms.Label();
            this.cmsCamera.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsCamera
            // 
            this.cmsCamera.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpen,
            this.tsmiCameraProperty,
            this.tsmiCameraFormat,
            this.tsmiCameraParam,
            this.AutoSaveSamplePicToolStripMenuItem,
            this.tsmiGraphy,
            this.tsmiClose,
            this.tsmiShowAllTestPoint,
            this.tsmiSaveMultiPoint,
            this.tsmiSaveOutMultiPoint,
            this.tsmiEditMultiPoint,
            this.tsmiDelCurrentFlag,
            this.tsmiDelLastFlag,
            this.tsmiDelAllFlag,
            this.tsmiSetMoveRate,
            this.tsmiCoeeGraphWidthHeight,
            this.tsmAdjustOrient});
            this.cmsCamera.Name = "cmsCamera";
            this.cmsCamera.Size = new System.Drawing.Size(185, 378);
            this.cmsCamera.Opening += new System.ComponentModel.CancelEventHandler(this.cmsCamera_Opening);
            // 
            // tsmiOpen
            // 
            this.tsmiOpen.Name = "tsmiOpen";
            this.tsmiOpen.Size = new System.Drawing.Size(184, 22);
            this.tsmiOpen.Text = "打开";
            this.tsmiOpen.Click += new System.EventHandler(this.tsmiOpen_Click);
            // 
            // tsmiCameraProperty
            // 
            this.tsmiCameraProperty.Name = "tsmiCameraProperty";
            this.tsmiCameraProperty.Size = new System.Drawing.Size(184, 22);
            this.tsmiCameraProperty.Text = "视频源";
            this.tsmiCameraProperty.Click += new System.EventHandler(this.tsmiCameraProperty_Click);
            // 
            // tsmiCameraFormat
            // 
            this.tsmiCameraFormat.Name = "tsmiCameraFormat";
            this.tsmiCameraFormat.Size = new System.Drawing.Size(184, 22);
            this.tsmiCameraFormat.Text = "视频格式";
            this.tsmiCameraFormat.Click += new System.EventHandler(this.tsmiCameraFormat_Click);
            // 
            // tsmiCameraParam
            // 
            this.tsmiCameraParam.Name = "tsmiCameraParam";
            this.tsmiCameraParam.Size = new System.Drawing.Size(184, 22);
            this.tsmiCameraParam.Text = "视频参数";
            this.tsmiCameraParam.Click += new System.EventHandler(this.tsmiCameraParam_Click);
            // 
            // AutoSaveSamplePicToolStripMenuItem
            // 
            this.AutoSaveSamplePicToolStripMenuItem.Name = "AutoSaveSamplePicToolStripMenuItem";
            this.AutoSaveSamplePicToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.AutoSaveSamplePicToolStripMenuItem.Text = "自动保存样品图";
            this.AutoSaveSamplePicToolStripMenuItem.Click += new System.EventHandler(this.AutoSaveSamplePicToolStripMenuItem_Click);
            // 
            // tsmiGraphy
            // 
            this.tsmiGraphy.Name = "tsmiGraphy";
            this.tsmiGraphy.Size = new System.Drawing.Size(184, 22);
            this.tsmiGraphy.Text = "抓图";
            this.tsmiGraphy.Click += new System.EventHandler(this.tsmiGraphy_Click);
            // 
            // tsmiClose
            // 
            this.tsmiClose.Name = "tsmiClose";
            this.tsmiClose.Size = new System.Drawing.Size(184, 22);
            this.tsmiClose.Text = "关闭";
            this.tsmiClose.Click += new System.EventHandler(this.tsmiClose_Click);
            // 
            // tsmiShowAllTestPoint
            // 
            this.tsmiShowAllTestPoint.Name = "tsmiShowAllTestPoint";
            this.tsmiShowAllTestPoint.Size = new System.Drawing.Size(184, 22);
            this.tsmiShowAllTestPoint.Text = "选择测量点";
            this.tsmiShowAllTestPoint.MouseHover += new System.EventHandler(this.tsmiShowAllTestPoint_MouseHover);
            // 
            // tsmiSaveMultiPoint
            // 
            this.tsmiSaveMultiPoint.Name = "tsmiSaveMultiPoint";
            this.tsmiSaveMultiPoint.Size = new System.Drawing.Size(184, 22);
            this.tsmiSaveMultiPoint.Text = "保存测试点";
            this.tsmiSaveMultiPoint.Click += new System.EventHandler(this.tsmiSaveMultiPoint_Click);
            // 
            // tsmiSaveOutMultiPoint
            // 
            this.tsmiSaveOutMultiPoint.Name = "tsmiSaveOutMultiPoint";
            this.tsmiSaveOutMultiPoint.Size = new System.Drawing.Size(184, 22);
            this.tsmiSaveOutMultiPoint.Text = "保存外测量点";
            this.tsmiSaveOutMultiPoint.Click += new System.EventHandler(this.tsmiSaveOutMultiPoint_Click);
            // 
            // tsmiEditMultiPoint
            // 
            this.tsmiEditMultiPoint.Name = "tsmiEditMultiPoint";
            this.tsmiEditMultiPoint.Size = new System.Drawing.Size(184, 22);
            this.tsmiEditMultiPoint.Text = "编辑测量点";
            this.tsmiEditMultiPoint.Click += new System.EventHandler(this.tsmiEditMultiPoint_Click);
            // 
            // tsmiDelCurrentFlag
            // 
            this.tsmiDelCurrentFlag.Name = "tsmiDelCurrentFlag";
            this.tsmiDelCurrentFlag.Size = new System.Drawing.Size(184, 22);
            this.tsmiDelCurrentFlag.Text = "删除当前测量点";
            this.tsmiDelCurrentFlag.Click += new System.EventHandler(this.tsmiDelCurrentFlag_Click);
            // 
            // tsmiDelLastFlag
            // 
            this.tsmiDelLastFlag.Name = "tsmiDelLastFlag";
            this.tsmiDelLastFlag.Size = new System.Drawing.Size(184, 22);
            this.tsmiDelLastFlag.Text = "删除最后一个标注点";
            this.tsmiDelLastFlag.Click += new System.EventHandler(this.tsmiDelLastFlag_Click);
            // 
            // tsmiDelAllFlag
            // 
            this.tsmiDelAllFlag.Name = "tsmiDelAllFlag";
            this.tsmiDelAllFlag.Size = new System.Drawing.Size(184, 22);
            this.tsmiDelAllFlag.Text = "删除所有测量点";
            this.tsmiDelAllFlag.Click += new System.EventHandler(this.tsmiDelAllFlag_Click);
            // 
            // tsmiSetMoveRate
            // 
            this.tsmiSetMoveRate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiX,
            this.tsmiY});
            this.tsmiSetMoveRate.Name = "tsmiSetMoveRate";
            this.tsmiSetMoveRate.Size = new System.Drawing.Size(184, 22);
            this.tsmiSetMoveRate.Text = "设置移动比例";
            this.tsmiSetMoveRate.VisibleChanged += new System.EventHandler(this.tsmiSetMoveRate_VisibleChanged);
            // 
            // tsmiX
            // 
            this.tsmiX.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMoveRateValueX});
            this.tsmiX.Name = "tsmiX";
            this.tsmiX.Size = new System.Drawing.Size(96, 22);
            this.tsmiX.Text = "X轴";
            // 
            // tsmiMoveRateValueX
            // 
            this.tsmiMoveRateValueX.Name = "tsmiMoveRateValueX";
            this.tsmiMoveRateValueX.Size = new System.Drawing.Size(100, 23);
            this.tsmiMoveRateValueX.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tsmiMoveRateValueX_KeyUp);
            // 
            // tsmiY
            // 
            this.tsmiY.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMoveRateValueY});
            this.tsmiY.Name = "tsmiY";
            this.tsmiY.Size = new System.Drawing.Size(96, 22);
            this.tsmiY.Text = "Y轴";
            // 
            // tsmiMoveRateValueY
            // 
            this.tsmiMoveRateValueY.Name = "tsmiMoveRateValueY";
            this.tsmiMoveRateValueY.Size = new System.Drawing.Size(100, 23);
            this.tsmiMoveRateValueY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tsmiMoveRateValueY_KeyUp);
            // 
            // tsmiCoeeGraphWidthHeight
            // 
            this.tsmiCoeeGraphWidthHeight.Name = "tsmiCoeeGraphWidthHeight";
            this.tsmiCoeeGraphWidthHeight.Size = new System.Drawing.Size(184, 22);
            this.tsmiCoeeGraphWidthHeight.Text = "校正画面宽高";
            this.tsmiCoeeGraphWidthHeight.Click += new System.EventHandler(this.tsmiCoeeGraphWidthHeight_Click);
            // 
            // tsmAdjustOrient
            // 
            this.tsmAdjustOrient.Name = "tsmAdjustOrient";
            this.tsmAdjustOrient.Size = new System.Drawing.Size(184, 22);
            this.tsmAdjustOrient.Text = "调整方向";
            // 
            // lblCameraInfo
            // 
            this.lblCameraInfo.AutoSize = true;
            this.lblCameraInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblCameraInfo.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCameraInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblCameraInfo.Location = new System.Drawing.Point(139, 145);
            this.lblCameraInfo.Name = "lblCameraInfo";
            this.lblCameraInfo.Size = new System.Drawing.Size(0, 14);
            this.lblCameraInfo.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(307, 254);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(66, 28);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblTestInfomation
            // 
            this.lblTestInfomation.AutoSize = true;
            this.lblTestInfomation.BackColor = System.Drawing.Color.Transparent;
            this.lblTestInfomation.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestInfomation.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblTestInfomation.Location = new System.Drawing.Point(161, 304);
            this.lblTestInfomation.Name = "lblTestInfomation";
            this.lblTestInfomation.Size = new System.Drawing.Size(0, 19);
            this.lblTestInfomation.TabIndex = 3;
            // 
            // SkyrayCamera
            // 
            this.BackColor = System.Drawing.Color.LightCyan;
            this.Controls.Add(this.lblTestInfomation);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblCameraInfo);
            this.Size = new System.Drawing.Size(377, 324);
            this.Resize += new System.EventHandler(this.SkyrayCamera_Resize);
            this.cmsCamera.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsCamera;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmiCameraProperty;
        private System.Windows.Forms.ToolStripMenuItem tsmiCameraFormat;
        private System.Windows.Forms.ToolStripMenuItem tsmiCameraParam;
        private System.Windows.Forms.ToolStripMenuItem tsmiClose;
        public System.Windows.Forms.ToolStripMenuItem tsmiDelLastFlag;
        public System.Windows.Forms.ToolStripMenuItem tsmiDelAllFlag;
        private System.Windows.Forms.ToolStripMenuItem tsmiSetMoveRate;
        private System.Windows.Forms.ToolStripMenuItem tsmiCoeeGraphWidthHeight;
        private System.Windows.Forms.ToolStripMenuItem tsmiX;
        private System.Windows.Forms.ToolStripMenuItem tsmiY;
        private System.Windows.Forms.ToolStripTextBox tsmiMoveRateValueX;
        private System.Windows.Forms.ToolStripTextBox tsmiMoveRateValueY;
        private System.Windows.Forms.Label lblCameraInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiGraphy;
        private System.Windows.Forms.SaveFileDialog savegraph;
        private System.Windows.Forms.ToolStripMenuItem tsmAdjustOrient;
        private System.Windows.Forms.ToolStripMenuItem AutoSaveSamplePicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelCurrentFlag;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblTestInfomation;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowAllTestPoint;
        public System.Windows.Forms.ToolStripMenuItem tsmiSaveMultiPoint;
        public System.Windows.Forms.ToolStripMenuItem tsmiSaveOutMultiPoint;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditMultiPoint;
    }
}
