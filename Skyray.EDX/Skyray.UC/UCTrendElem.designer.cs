namespace Skyray.UC
{
    partial class UCTrendElem
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
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trendChart1 = new Skyray.Controls.Extension.TrendChart();
            this.SuspendLayout();
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // trendChart1
            // 
            this.trendChart1.BoundaryElement = null;
            this.trendChart1.BShowElement = false;
            this.trendChart1.ChangeSpeed = 0.025;
            this.trendChart1.Coeff = null;
            this.trendChart1.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.YellowGreen,
        System.Drawing.Color.YellowGreen};
            this.trendChart1.CurrentChannelCount = 0;
            this.trendChart1.currentInterestArea = null;
            this.trendChart1.Cursor = System.Windows.Forms.Cursors.Default;
            this.trendChart1.DefaultLineHeight = 0;
            this.trendChart1.DEnergy = 0;
            this.trendChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trendChart1.EditButtons = System.Windows.Forms.MouseButtons.Right;
            this.trendChart1.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.trendChart1.EnableMoveLine = true;
            this.trendChart1.EnableWheel = true;
            this.trendChart1.FontSize = 0;
            this.trendChart1.HorizontalSynchronization = true;
            this.trendChart1.ICurrentChannel = 0;
            this.trendChart1.InterestAreaEnable = false;
            this.trendChart1.IsActiveMove = true;
            this.trendChart1.IsMainElement = false;
            this.trendChart1.IsShowPeakFlagAuto = false;
            this.trendChart1.IsUseScroll = true;
            this.trendChart1.IUseBase = false;
            this.trendChart1.IUseBoundary = false;
            this.trendChart1.IXMaxChannel = 0;
            this.trendChart1.IYMaxChannel = 0;
            this.trendChart1.IZero = false;
            this.trendChart1.LinkButtons = System.Windows.Forms.MouseButtons.Left;
            this.trendChart1.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.trendChart1.Location = new System.Drawing.Point(8, 9);
            this.trendChart1.MultiGraph = false;
            this.trendChart1.Name = "trendChart1";
            this.trendChart1.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.trendChart1.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            this.trendChart1.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)));
            this.trendChart1.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            this.trendChart1.Positions = new float[] {
        0F,
        1F};
            this.trendChart1.ScrollGrace = 0;
            this.trendChart1.ScrollMaxX = 0;
            this.trendChart1.ScrollMaxY = 0;
            this.trendChart1.ScrollMaxY2 = 0;
            this.trendChart1.ScrollMinX = 0;
            this.trendChart1.ScrollMinY = 0;
            this.trendChart1.ScrollMinY2 = 0;
            this.trendChart1.SelectButtons = System.Windows.Forms.MouseButtons.Left;
            this.trendChart1.SelectModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.trendChart1.Size = new System.Drawing.Size(584, 382);
            this.trendChart1.TabIndex = 2;
            this.trendChart1.UnSpecing = true;
            this.trendChart1.VerticalSynchronization = true;
            this.trendChart1.XTitle = "x-title";
            this.trendChart1.YTitle = "y-title";
            this.trendChart1.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            this.trendChart1.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            this.trendChart1.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            this.trendChart1.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            this.trendChart1.ZoomStepFraction = 0;
            // 
            // UCTrendElem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.Controls.Add(this.trendChart1);
            this.Name = "UCTrendElem";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(600, 400);
            this.Load += new System.EventHandler(this.UCTrend_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        public Skyray.Controls.Extension.TrendChart trendChart1;

       
    }
}
