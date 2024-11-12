namespace Skyray.UC
{
    partial class UCStop
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
            this.btnStopTest = new Skyray.Controls.ButtonW();
            this.btnSuspendTest = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // btnStopTest
            // 
            this.btnStopTest.bSilver = false;
            this.btnStopTest.Location = new System.Drawing.Point(52, 35);
            this.btnStopTest.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnStopTest.MenuPos = new System.Drawing.Point(0, 0);
            this.btnStopTest.Name = "btnStopTest";
            this.btnStopTest.Size = new System.Drawing.Size(75, 23);
            this.btnStopTest.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnStopTest.TabIndex = 2;
            this.btnStopTest.Text = "终止测试";
            this.btnStopTest.ToFocused = false;
            this.btnStopTest.UseVisualStyleBackColor = true;
            this.btnStopTest.Click += new System.EventHandler(this.btnStopTest_Click);
            // 
            // btnSuspendTest
            // 
            this.btnSuspendTest.bSilver = false;
            this.btnSuspendTest.Location = new System.Drawing.Point(227, 35);
            this.btnSuspendTest.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSuspendTest.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSuspendTest.Name = "btnSuspendTest";
            this.btnSuspendTest.Size = new System.Drawing.Size(75, 23);
            this.btnSuspendTest.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSuspendTest.TabIndex = 3;
            this.btnSuspendTest.Text = "暂停测试";
            this.btnSuspendTest.ToFocused = false;
            this.btnSuspendTest.UseVisualStyleBackColor = true;
            this.btnSuspendTest.Click += new System.EventHandler(this.btnSuspendTest_Click);
            // 
            // UCStop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(356, 91);
            this.Controls.Add(this.btnSuspendTest);
            this.Controls.Add(this.btnStopTest);
            this.Name = "UCStop";
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnStopTest;
        private Skyray.Controls.ButtonW btnSuspendTest;
    }
}
