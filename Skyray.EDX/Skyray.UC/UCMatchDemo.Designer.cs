namespace Skyray.UC
{
    partial class UCMatchDemo
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
            this.richTextMathDemoInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextMathDemoInfo
            // 
            this.richTextMathDemoInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextMathDemoInfo.Location = new System.Drawing.Point(0, 0);
            this.richTextMathDemoInfo.Name = "richTextMathDemoInfo";
            this.richTextMathDemoInfo.Size = new System.Drawing.Size(430, 328);
            this.richTextMathDemoInfo.TabIndex = 0;
            this.richTextMathDemoInfo.Text = "";
            // 
            // UCMatchDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextMathDemoInfo);
            this.Name = "UCMatchDemo";
            this.Size = new System.Drawing.Size(430, 328);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextMathDemoInfo;
    }
}
