namespace Skyray.UC
{
    partial class UCCoeeParam
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
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.meCoee = new Skyray.EDX.Common.ModelEditor();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(410, 286);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(234, 286);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(92, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // meCoee
            // 
            this.meCoee.DataSource = null;
            this.meCoee.GroupTitle = "";
            this.meCoee.LabelPosition = Skyray.EDX.Common.LabelPosition.Top;
            this.meCoee.LayoutType = null;
            this.meCoee.Location = new System.Drawing.Point(4, 3);
            this.meCoee.Name = "meCoee";
            this.meCoee.Size = new System.Drawing.Size(925, 273);
            this.meCoee.SLayoutType = null;
            this.meCoee.TabIndex = 5;
            // 
            // UCCoeeParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.meCoee);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "UCCoeeParam";
            this.Size = new System.Drawing.Size(932, 331);
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.EDX.Common.ModelEditor meCoee;
    }
}
