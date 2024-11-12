namespace Skyray.UC
{
    partial class UCNodeNameOrPath
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
            this.lblNodeName = new System.Windows.Forms.Label();
            this.txtNodeName = new System.Windows.Forms.TextBox();
            this.lblNodePath = new System.Windows.Forms.Label();
            this.txtNodePath = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExplore = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // lblNodeName
            // 
            this.lblNodeName.AutoSize = true;
            this.lblNodeName.Location = new System.Drawing.Point(27, 13);
            this.lblNodeName.Name = "lblNodeName";
            this.lblNodeName.Size = new System.Drawing.Size(65, 12);
            this.lblNodeName.TabIndex = 0;
            this.lblNodeName.Text = "节点名称：";
            // 
            // txtNodeName
            // 
            this.txtNodeName.Location = new System.Drawing.Point(98, 10);
            this.txtNodeName.Name = "txtNodeName";
            this.txtNodeName.Size = new System.Drawing.Size(100, 21);
            this.txtNodeName.TabIndex = 1;
            // 
            // lblNodePath
            // 
            this.lblNodePath.AutoSize = true;
            this.lblNodePath.Location = new System.Drawing.Point(27, 42);
            this.lblNodePath.Name = "lblNodePath";
            this.lblNodePath.Size = new System.Drawing.Size(65, 12);
            this.lblNodePath.TabIndex = 2;
            this.lblNodePath.Text = "节点路径：";
            // 
            // txtNodePath
            // 
            this.txtNodePath.Location = new System.Drawing.Point(98, 39);
            this.txtNodePath.Name = "txtNodePath";
            this.txtNodePath.ReadOnly = true;
            this.txtNodePath.Size = new System.Drawing.Size(166, 21);
            this.txtNodePath.TabIndex = 3;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(139, 65);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 4;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(237, 65);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExplore
            // 
            this.btnExplore.Location = new System.Drawing.Point(270, 37);
            this.btnExplore.Name = "btnExplore";
            this.btnExplore.Size = new System.Drawing.Size(28, 23);
            this.btnExplore.TabIndex = 6;
            this.btnExplore.Text = "..";
            this.btnExplore.UseVisualStyleBackColor = true;
            this.btnExplore.Click += new System.EventHandler(this.btnExplore_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // UCNodeNameOrPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExplore);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtNodePath);
            this.Controls.Add(this.lblNodePath);
            this.Controls.Add(this.txtNodeName);
            this.Controls.Add(this.lblNodeName);
            this.Name = "UCNodeNameOrPath";
            this.Size = new System.Drawing.Size(323, 94);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNodeName;
        private System.Windows.Forms.TextBox txtNodeName;
        private System.Windows.Forms.Label lblNodePath;
        private System.Windows.Forms.TextBox txtNodePath;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExplore;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
