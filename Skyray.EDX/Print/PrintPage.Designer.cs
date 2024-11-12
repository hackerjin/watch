namespace Skyray.Print
{
    partial class PrintPage
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
            this.splitterTop = new Skyray.Controls.MTSplitter();
            this.splitterBottom = new Skyray.Controls.MTSplitter();
            this.PnlDesign = new Skyray.Print.PrintPanel();
            this.PnlFooter = new Skyray.Print.PrintPanel();
            this.PnlHeader = new Skyray.Print.PrintPanel();
            this.SuspendLayout();
            // 
            // splitterTop
            // 
            this.splitterTop.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.splitterTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterTop.Location = new System.Drawing.Point(1, 96);
            this.splitterTop.Name = "splitterTop";
            this.splitterTop.Size = new System.Drawing.Size(432, 1);
            this.splitterTop.SplitterBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.splitterTop.SplitterDarkColor = System.Drawing.Color.White;
            this.splitterTop.SplitterLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.splitterTop.SplitterPaintAngle = 90F;
            this.splitterTop.SplitterPointCount = 0;
            this.splitterTop.TabIndex = 1;
            this.splitterTop.TabStop = false;
            // 
            // splitterBottom
            // 
            this.splitterBottom.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.splitterBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterBottom.Location = new System.Drawing.Point(1, 516);
            this.splitterBottom.Name = "splitterBottom";
            this.splitterBottom.Size = new System.Drawing.Size(432, 1);
            this.splitterBottom.SplitterBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.splitterBottom.SplitterDarkColor = System.Drawing.Color.White;
            this.splitterBottom.SplitterLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.splitterBottom.SplitterPaintAngle = 90F;
            this.splitterBottom.SplitterPointCount = 0;
            this.splitterBottom.TabIndex = 3;
            this.splitterBottom.TabStop = false;
            this.splitterBottom.Visible = false;
            // 
            // PnlDesign
            // 
            this.PnlDesign.AllowDrop = true;
            this.PnlDesign.BackColor = System.Drawing.Color.White;
            this.PnlDesign.CtrlMargin = new System.Windows.Forms.Padding(0);
            this.PnlDesign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlDesign.Location = new System.Drawing.Point(1, 97);
            this.PnlDesign.Name = "PnlDesign";
            this.PnlDesign.Page = null;
            this.PnlDesign.ShowFooterCorner = true;
            this.PnlDesign.ShowHeaderCorner = false;
            this.PnlDesign.Size = new System.Drawing.Size(432, 419);
            this.PnlDesign.TabIndex = 5;
            this.PnlDesign.Text = "printPanel3";
            this.PnlDesign.Type = Skyray.Print.CtrlType.Body;
            // 
            // PnlFooter
            // 
            this.PnlFooter.AllowDrop = true;
            this.PnlFooter.BackColor = System.Drawing.Color.White;
            this.PnlFooter.CtrlMargin = new System.Windows.Forms.Padding(0);
            this.PnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlFooter.Location = new System.Drawing.Point(1, 517);
            this.PnlFooter.Name = "PnlFooter";
            this.PnlFooter.Page = null;
            this.PnlFooter.ShowFooterCorner = true;
            this.PnlFooter.ShowHeaderCorner = false;
            this.PnlFooter.Size = new System.Drawing.Size(432, 101);
            this.PnlFooter.TabIndex = 2;
            this.PnlFooter.Text = "printPanel2";
            this.PnlFooter.Type = Skyray.Print.CtrlType.Footer;
            this.PnlFooter.Visible = false;
            // 
            // PnlHeader
            // 
            this.PnlHeader.AllowDrop = true;
            this.PnlHeader.BackColor = System.Drawing.Color.White;
            this.PnlHeader.CtrlMargin = new System.Windows.Forms.Padding(0);
            this.PnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlHeader.Location = new System.Drawing.Point(1, 1);
            this.PnlHeader.Name = "PnlHeader";
            this.PnlHeader.Page = null;
            this.PnlHeader.ShowFooterCorner = false;
            this.PnlHeader.ShowHeaderCorner = true;
            this.PnlHeader.Size = new System.Drawing.Size(432, 95);
            this.PnlHeader.TabIndex = 0;
            this.PnlHeader.Text = "printPanel1";
            this.PnlHeader.Type = Skyray.Print.CtrlType.Header;
            // 
            // PrintPage
            // 
            this.Controls.Add(this.PnlDesign);
            this.Controls.Add(this.splitterBottom);
            this.Controls.Add(this.PnlFooter);
            this.Controls.Add(this.splitterTop);
            this.Controls.Add(this.PnlHeader);
            this.Padding = new System.Windows.Forms.Padding(1, 1, 3, 3);
            this.Size = new System.Drawing.Size(436, 621);
            this.ResumeLayout(false);

        }

        #endregion

        private PrintPanel PnlHeader;
        private Skyray.Controls.MTSplitter splitterTop;
        private PrintPanel PnlFooter;
        private Skyray.Controls.MTSplitter splitterBottom;
        private PrintPanel PnlDesign;
    }
}
