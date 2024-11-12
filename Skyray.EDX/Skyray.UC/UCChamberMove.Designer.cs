namespace Skyray.UC
{
    partial class UCChamberMove
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
            this.lblChamberMove = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblStep = new System.Windows.Forms.Label();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblChamberIndex = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblChamberMove
            // 
            this.lblChamberMove.AutoSize = true;
            this.lblChamberMove.Location = new System.Drawing.Point(38, 42);
            this.lblChamberMove.Name = "lblChamberMove";
            this.lblChamberMove.Size = new System.Drawing.Size(41, 12);
            this.lblChamberMove.TabIndex = 0;
            this.lblChamberMove.Text = "样品杯";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(98, 39);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblStep
            // 
            this.lblStep.AutoSize = true;
            this.lblStep.BackColor = System.Drawing.Color.GhostWhite;
            this.lblStep.Location = new System.Drawing.Point(277, 42);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(0, 12);
            this.lblStep.TabIndex = 2;
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(86, 94);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(75, 23);
            this.btnMove.TabIndex = 3;
            this.btnMove.Text = "移动";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(193, 94);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblChamberIndex
            // 
            this.lblChamberIndex.AutoSize = true;
            this.lblChamberIndex.Location = new System.Drawing.Point(38, 73);
            this.lblChamberIndex.Name = "lblChamberIndex";
            this.lblChamberIndex.Size = new System.Drawing.Size(89, 12);
            this.lblChamberIndex.TabIndex = 5;
            this.lblChamberIndex.Text = "样品杯当前杯位";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(255, 37);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "复位";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // UCChamberMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblChamberIndex);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblChamberMove);
            this.Name = "UCChamberMove";
            this.Size = new System.Drawing.Size(368, 125);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblChamberMove;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblChamberIndex;
        private System.Windows.Forms.Button btnReset;
    }
}
