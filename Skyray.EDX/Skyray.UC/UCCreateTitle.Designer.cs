namespace Skyray.UC
{
    partial class UCCreateTitle
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
            this.lblICO = new Skyray.Controls.LabelW();
            this.lblTitle = new Skyray.Controls.LabelW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.txtTitle = new Skyray.Controls.TextBoxW();
            this.ofdIco = new System.Windows.Forms.OpenFileDialog();
            this.txtICO = new Skyray.Controls.TextBoxW();
            this.btnSelectICO = new Skyray.Controls.ButtonW();
            this.pbICO = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbICO)).BeginInit();
            this.SuspendLayout();
            // 
            // lblICO
            // 
            this.lblICO.AutoSize = true;
            this.lblICO.BackColor = System.Drawing.Color.Transparent;
            this.lblICO.Location = new System.Drawing.Point(51, 50);
            this.lblICO.Name = "lblICO";
            this.lblICO.Size = new System.Drawing.Size(29, 12);
            this.lblICO.TabIndex = 0;
            this.lblICO.Text = "图标";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Location = new System.Drawing.Point(51, 117);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(29, 12);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "标题";
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(75, 170);
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
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(220, 170);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtTitle
            // 
            this.txtTitle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtTitle.Location = new System.Drawing.Point(111, 114);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(159, 21);
            this.txtTitle.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtTitle.TabIndex = 4;
            // 
            // ofdIco
            // 
            this.ofdIco.Filter = "*.ico|";
            // 
            // txtICO
            // 
            this.txtICO.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtICO.Location = new System.Drawing.Point(111, 47);
            this.txtICO.Name = "txtICO";
            this.txtICO.Size = new System.Drawing.Size(159, 21);
            this.txtICO.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtICO.TabIndex = 5;
            // 
            // btnSelectICO
            // 
            this.btnSelectICO.bSilver = false;
            this.btnSelectICO.Location = new System.Drawing.Point(276, 47);
            this.btnSelectICO.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSelectICO.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSelectICO.Name = "btnSelectICO";
            this.btnSelectICO.Size = new System.Drawing.Size(33, 21);
            this.btnSelectICO.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSelectICO.TabIndex = 6;
            this.btnSelectICO.Text = "...";
            this.btnSelectICO.ToFocused = false;
            this.btnSelectICO.UseVisualStyleBackColor = true;
            this.btnSelectICO.Click += new System.EventHandler(this.btnSelectICO_Click);
            // 
            // pbICO
            // 
            this.pbICO.Location = new System.Drawing.Point(315, 39);
            this.pbICO.Name = "pbICO";
            this.pbICO.Size = new System.Drawing.Size(30, 35);
            this.pbICO.TabIndex = 7;
            this.pbICO.TabStop = false;
            // 
            // UCCreateTitle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbICO);
            this.Controls.Add(this.btnSelectICO);
            this.Controls.Add(this.txtICO);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblICO);
            this.Name = "UCCreateTitle";
            this.Size = new System.Drawing.Size(373, 221);
            ((System.ComponentModel.ISupportInitialize)(this.pbICO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblICO;
        private Skyray.Controls.LabelW lblTitle;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.TextBoxW txtTitle;
        private System.Windows.Forms.OpenFileDialog ofdIco;
        private Skyray.Controls.TextBoxW txtICO;
        private Skyray.Controls.ButtonW btnSelectICO;
        private System.Windows.Forms.PictureBox pbICO;
    }
}
