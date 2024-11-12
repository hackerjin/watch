namespace Skyray.UC
{
    partial class UCSelectSpecType
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
            this.btnSubmit = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.lblselectType = new Skyray.Controls.LabelW();
            this.cbxSelectType = new Skyray.Controls.ComboBoxW();
            this.chkExport = new Skyray.Controls.CheckBoxW();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.bSilver = false;
            this.btnSubmit.Location = new System.Drawing.Point(154, 57);
            this.btnSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 21);
            this.btnSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.ToFocused = false;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(254, 57);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 21);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblselectType
            // 
            this.lblselectType.AutoSize = true;
            this.lblselectType.BackColor = System.Drawing.Color.Transparent;
            this.lblselectType.Location = new System.Drawing.Point(19, 15);
            this.lblselectType.Name = "lblselectType";
            this.lblselectType.Size = new System.Drawing.Size(89, 12);
            this.lblselectType.TabIndex = 5;
            this.lblselectType.Text = "选择谱的版本：";
            // 
            // cbxSelectType
            // 
            this.cbxSelectType.AutoComplete = false;
            this.cbxSelectType.AutoDropdown = false;
            this.cbxSelectType.BackColorEven = System.Drawing.Color.White;
            this.cbxSelectType.BackColorOdd = System.Drawing.Color.White;
            this.cbxSelectType.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cbxSelectType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cbxSelectType.ColumnNames = "";
            this.cbxSelectType.ColumnWidthDefault = 75;
            this.cbxSelectType.ColumnWidths = "";
            this.cbxSelectType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxSelectType.FormattingEnabled = true;
            this.cbxSelectType.LinkedColumnIndex = 0;
            this.cbxSelectType.LinkedTextBox = null;
            this.cbxSelectType.Location = new System.Drawing.Point(113, 12);
            this.cbxSelectType.Name = "cbxSelectType";
            this.cbxSelectType.Size = new System.Drawing.Size(121, 22);
            this.cbxSelectType.TabIndex = 6;
            this.cbxSelectType.SelectedIndexChanged += new System.EventHandler(this.cbxSelectType_SelectedIndexChanged);
            // 
            // chkExport
            // 
            this.chkExport.AutoSize = true;
            this.chkExport.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkExport.Location = new System.Drawing.Point(265, 14);
            this.chkExport.Margin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.chkExport.Name = "chkExport";
            this.chkExport.Size = new System.Drawing.Size(84, 16);
            this.chkExport.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkExport.TabIndex = 39;
            this.chkExport.Text = "Fp格式导出";
            this.chkExport.UseVisualStyleBackColor = true;
            // 
            // UCSelectSpecType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkExport);
            this.Controls.Add(this.cbxSelectType);
            this.Controls.Add(this.lblselectType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Name = "UCSelectSpecType";
            this.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Size = new System.Drawing.Size(369, 88);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btnSubmit;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.LabelW lblselectType;
        private Skyray.Controls.ComboBoxW cbxSelectType;
        private Skyray.Controls.CheckBoxW chkExport;

    }
}
