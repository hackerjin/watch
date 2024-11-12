namespace Skyray.UC
{
    partial class UCMoveNetWorkFun
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
            this.radioButtonSingle = new System.Windows.Forms.RadioButton();
            this.radioButtonCheck = new System.Windows.Forms.RadioButton();
            this.radioButtonNetwork = new System.Windows.Forms.RadioButton();
            this.radioButtonMany = new System.Windows.Forms.RadioButton();
            this.radioButtonMove = new System.Windows.Forms.RadioButton();
            this.buttonWReset = new System.Windows.Forms.Button();
            this.buttonWStop = new System.Windows.Forms.Button();
            this.buttonWStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioButtonSingle
            // 
            this.radioButtonSingle.AutoSize = true;
            this.radioButtonSingle.Location = new System.Drawing.Point(160, 10);
            this.radioButtonSingle.Name = "radioButtonSingle";
            this.radioButtonSingle.Size = new System.Drawing.Size(47, 16);
            this.radioButtonSingle.TabIndex = 25;
            this.radioButtonSingle.TabStop = true;
            this.radioButtonSingle.Text = "单点";
            this.radioButtonSingle.UseVisualStyleBackColor = true;
            this.radioButtonSingle.CheckedChanged += new System.EventHandler(this.radioButtonSingle_CheckedChanged);
            // 
            // radioButtonCheck
            // 
            this.radioButtonCheck.AutoSize = true;
            this.radioButtonCheck.Location = new System.Drawing.Point(89, 10);
            this.radioButtonCheck.Name = "radioButtonCheck";
            this.radioButtonCheck.Size = new System.Drawing.Size(47, 16);
            this.radioButtonCheck.TabIndex = 24;
            this.radioButtonCheck.TabStop = true;
            this.radioButtonCheck.Text = "校正";
            this.radioButtonCheck.UseVisualStyleBackColor = true;
            this.radioButtonCheck.CheckedChanged += new System.EventHandler(this.radioButtonCheck_CheckedChanged);
            // 
            // radioButtonNetwork
            // 
            this.radioButtonNetwork.AutoSize = true;
            this.radioButtonNetwork.Location = new System.Drawing.Point(302, 10);
            this.radioButtonNetwork.Name = "radioButtonNetwork";
            this.radioButtonNetwork.Size = new System.Drawing.Size(47, 16);
            this.radioButtonNetwork.TabIndex = 27;
            this.radioButtonNetwork.TabStop = true;
            this.radioButtonNetwork.Text = "网格";
            this.radioButtonNetwork.UseVisualStyleBackColor = true;
            this.radioButtonNetwork.CheckedChanged += new System.EventHandler(this.radioButtonNetwork_CheckedChanged);
            // 
            // radioButtonMany
            // 
            this.radioButtonMany.AutoSize = true;
            this.radioButtonMany.Location = new System.Drawing.Point(231, 10);
            this.radioButtonMany.Name = "radioButtonMany";
            this.radioButtonMany.Size = new System.Drawing.Size(47, 16);
            this.radioButtonMany.TabIndex = 26;
            this.radioButtonMany.TabStop = true;
            this.radioButtonMany.Text = "多点";
            this.radioButtonMany.UseVisualStyleBackColor = true;
            this.radioButtonMany.CheckedChanged += new System.EventHandler(this.radioButtonMany_CheckedChanged);
            // 
            // radioButtonMove
            // 
            this.radioButtonMove.AutoSize = true;
            this.radioButtonMove.Location = new System.Drawing.Point(18, 10);
            this.radioButtonMove.Name = "radioButtonMove";
            this.radioButtonMove.Size = new System.Drawing.Size(47, 16);
            this.radioButtonMove.TabIndex = 23;
            this.radioButtonMove.TabStop = true;
            this.radioButtonMove.Text = "移动";
            this.radioButtonMove.UseVisualStyleBackColor = true;
            this.radioButtonMove.CheckedChanged += new System.EventHandler(this.radioButtonMove_CheckedChanged);
            // 
            // buttonWReset
            // 
            this.buttonWReset.Location = new System.Drawing.Point(465, 7);
            this.buttonWReset.Name = "buttonWReset";
            this.buttonWReset.Size = new System.Drawing.Size(95, 23);
            this.buttonWReset.TabIndex = 30;
            this.buttonWReset.Text = "复位";
            this.buttonWReset.UseVisualStyleBackColor = true;
            this.buttonWReset.Visible = false;
            this.buttonWReset.Click += new System.EventHandler(this.buttonWReset_Click);
            // 
            // buttonWStop
            // 
            this.buttonWStop.Location = new System.Drawing.Point(562, 7);
            this.buttonWStop.Name = "buttonWStop";
            this.buttonWStop.Size = new System.Drawing.Size(95, 23);
            this.buttonWStop.TabIndex = 29;
            this.buttonWStop.Text = "停止";
            this.buttonWStop.UseVisualStyleBackColor = true;
            this.buttonWStop.Click += new System.EventHandler(this.buttonWStop_Click);
            // 
            // buttonWStart
            // 
            this.buttonWStart.Location = new System.Drawing.Point(368, 7);
            this.buttonWStart.Name = "buttonWStart";
            this.buttonWStart.Size = new System.Drawing.Size(95, 23);
            this.buttonWStart.TabIndex = 28;
            this.buttonWStart.Text = "开始";
            this.buttonWStart.UseVisualStyleBackColor = true;
            this.buttonWStart.Click += new System.EventHandler(this.buttonWStart_Click);
            // 
            // UCMoveNetWorkFun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonWReset);
            this.Controls.Add(this.buttonWStop);
            this.Controls.Add(this.buttonWStart);
            this.Controls.Add(this.radioButtonSingle);
            this.Controls.Add(this.radioButtonCheck);
            this.Controls.Add(this.radioButtonNetwork);
            this.Controls.Add(this.radioButtonMany);
            this.Controls.Add(this.radioButtonMove);
            this.Name = "UCMoveNetWorkFun";
            this.Size = new System.Drawing.Size(675, 37);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonSingle;
        private System.Windows.Forms.RadioButton radioButtonCheck;
        private System.Windows.Forms.RadioButton radioButtonNetwork;
        private System.Windows.Forms.RadioButton radioButtonMany;
        private System.Windows.Forms.RadioButton radioButtonMove;
        private System.Windows.Forms.Button buttonWReset;
        private System.Windows.Forms.Button buttonWStop;
        private System.Windows.Forms.Button buttonWStart;
    }
}
