namespace Skyray.UC
{
    partial class UCChinawareDataBase
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
            this.btnChinawareDataBase = new Skyray.Controls.ButtonW();
            this.txtFileName = new Skyray.Controls.TextBoxW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.lblCount = new Skyray.Controls.LabelW();
            this.lblStumer = new Skyray.Controls.LabelW();
            this.txtCount = new Skyray.Controls.TextBoxW();
            this.txtStumer = new Skyray.Controls.TextBoxW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chKChinaware = new Skyray.Controls.CheckBoxW();
            this.SuspendLayout();
            // 
            // btnChinawareDataBase
            // 
            this.btnChinawareDataBase.bSilver = false;
            this.btnChinawareDataBase.Location = new System.Drawing.Point(22, 14);
            this.btnChinawareDataBase.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnChinawareDataBase.MenuPos = new System.Drawing.Point(0, 0);
            this.btnChinawareDataBase.Name = "btnChinawareDataBase";
            this.btnChinawareDataBase.Size = new System.Drawing.Size(75, 23);
            this.btnChinawareDataBase.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnChinawareDataBase.TabIndex = 0;
            this.btnChinawareDataBase.Text = "数据库";
            this.btnChinawareDataBase.ToFocused = false;
            this.btnChinawareDataBase.UseVisualStyleBackColor = true;
            this.btnChinawareDataBase.Click += new System.EventHandler(this.btnChinawareDataBase_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtFileName.Location = new System.Drawing.Point(128, 14);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(203, 21);
            this.txtFileName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtFileName.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(337, 12);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.BackColor = System.Drawing.Color.Transparent;
            this.lblCount.Location = new System.Drawing.Point(22, 53);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(77, 12);
            this.lblCount.TabIndex = 3;
            this.lblCount.Text = "牌号显示个数";
            // 
            // lblStumer
            // 
            this.lblStumer.AutoSize = true;
            this.lblStumer.BackColor = System.Drawing.Color.Transparent;
            this.lblStumer.Location = new System.Drawing.Point(22, 83);
            this.lblStumer.Name = "lblStumer";
            this.lblStumer.Size = new System.Drawing.Size(53, 12);
            this.lblStumer.TabIndex = 4;
            this.lblStumer.Text = "赝品阈值";
            // 
            // txtCount
            // 
            this.txtCount.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtCount.Location = new System.Drawing.Point(128, 50);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(203, 21);
            this.txtCount.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtCount.TabIndex = 5;
            this.txtCount.Text = "5";
            // 
            // txtStumer
            // 
            this.txtStumer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtStumer.Location = new System.Drawing.Point(128, 80);
            this.txtStumer.Name = "txtStumer";
            this.txtStumer.Size = new System.Drawing.Size(203, 21);
            this.txtStumer.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtStumer.TabIndex = 6;
            this.txtStumer.Text = "0.95";
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(337, 48);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chKChinaware
            // 
            this.chKChinaware.AutoSize = true;
            this.chKChinaware.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chKChinaware.Location = new System.Drawing.Point(24, 110);
            this.chKChinaware.Name = "chKChinaware";
            this.chKChinaware.Size = new System.Drawing.Size(108, 16);
            this.chKChinaware.Style = Skyray.Controls.Style.Office2007Blue;
            this.chKChinaware.TabIndex = 8;
            this.chKChinaware.Text = "是否启用数据库";
            this.chKChinaware.UseVisualStyleBackColor = true;
            this.chKChinaware.Visible = false;
            this.chKChinaware.CheckedChanged += new System.EventHandler(this.chKChinaware_CheckedChanged);
            // 
            // UCChinawareDataBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.chKChinaware);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtStumer);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.lblStumer);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btnChinawareDataBase);
            this.Name = "UCChinawareDataBase";
            this.Size = new System.Drawing.Size(426, 109);
            this.Load += new System.EventHandler(this.UCChinawareDataBase_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btnChinawareDataBase;
        private Skyray.Controls.TextBoxW txtFileName;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.LabelW lblCount;
        private Skyray.Controls.LabelW lblStumer;
        private Skyray.Controls.TextBoxW txtCount;
        private Skyray.Controls.TextBoxW txtStumer;
        private Skyray.Controls.ButtonW btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Skyray.Controls.CheckBoxW chKChinaware;
    }
}
