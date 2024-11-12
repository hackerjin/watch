namespace Skyray.UC
{
    partial class UCResolveParams
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
            this.lblElements = new System.Windows.Forms.Label();
            this.cbxElement = new System.Windows.Forms.ComboBox();
            this.lblCondition = new System.Windows.Forms.Label();
            this.cbxCondition = new System.Windows.Forms.ComboBox();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnSubmit = new Skyray.Controls.ButtonW();
            this.lbwDeviceParams = new Skyray.Controls.LabelW();
            this.cbxDeviceParamsList = new Skyray.Controls.ComboBoxW();
            this.SuspendLayout();
            // 
            // lblElements
            // 
            this.lblElements.AutoSize = true;
            this.lblElements.Location = new System.Drawing.Point(9, 16);
            this.lblElements.Name = "lblElements";
            this.lblElements.Size = new System.Drawing.Size(41, 12);
            this.lblElements.TabIndex = 0;
            this.lblElements.Text = "元素：";
            // 
            // cbxElement
            // 
            this.cbxElement.FormattingEnabled = true;
            this.cbxElement.Location = new System.Drawing.Point(97, 12);
            this.cbxElement.Name = "cbxElement";
            this.cbxElement.Size = new System.Drawing.Size(122, 20);
            this.cbxElement.TabIndex = 1;
            // 
            // lblCondition
            // 
            this.lblCondition.AutoSize = true;
            this.lblCondition.Location = new System.Drawing.Point(9, 47);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(41, 12);
            this.lblCondition.TabIndex = 2;
            this.lblCondition.Text = "条件：";
            // 
            // cbxCondition
            // 
            this.cbxCondition.FormattingEnabled = true;
            this.cbxCondition.Location = new System.Drawing.Point(97, 44);
            this.cbxCondition.Name = "cbxCondition";
            this.cbxCondition.Size = new System.Drawing.Size(173, 20);
            this.cbxCondition.TabIndex = 3;
            this.cbxCondition.SelectedIndexChanged += new System.EventHandler(this.cbxCondition_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(159, 116);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnSubmit
            // 
            this.btnSubmit.bSilver = false;
            this.btnSubmit.Location = new System.Drawing.Point(60, 116);
            this.btnSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.ToFocused = false;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click_1);
            // 
            // lbwDeviceParams
            // 
            this.lbwDeviceParams.AutoSize = true;
            this.lbwDeviceParams.BackColor = System.Drawing.Color.Transparent;
            this.lbwDeviceParams.Location = new System.Drawing.Point(9, 79);
            this.lbwDeviceParams.Name = "lbwDeviceParams";
            this.lbwDeviceParams.Size = new System.Drawing.Size(65, 12);
            this.lbwDeviceParams.TabIndex = 8;
            this.lbwDeviceParams.Text = "测量条件：";
            // 
            // cbxDeviceParamsList
            // 
            this.cbxDeviceParamsList.AutoComplete = false;
            this.cbxDeviceParamsList.AutoDropdown = false;
            this.cbxDeviceParamsList.BackColorEven = System.Drawing.Color.White;
            this.cbxDeviceParamsList.BackColorOdd = System.Drawing.Color.White;
            this.cbxDeviceParamsList.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cbxDeviceParamsList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cbxDeviceParamsList.ColumnNames = "";
            this.cbxDeviceParamsList.ColumnWidthDefault = 75;
            this.cbxDeviceParamsList.ColumnWidths = "";
            this.cbxDeviceParamsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxDeviceParamsList.FormattingEnabled = true;
            this.cbxDeviceParamsList.LinkedColumnIndex = 0;
            this.cbxDeviceParamsList.LinkedTextBox = null;
            this.cbxDeviceParamsList.Location = new System.Drawing.Point(98, 76);
            this.cbxDeviceParamsList.Name = "cbxDeviceParamsList";
            this.cbxDeviceParamsList.Size = new System.Drawing.Size(121, 22);
            this.cbxDeviceParamsList.TabIndex = 9;
            // 
            // UCResolveParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.cbxDeviceParamsList);
            this.Controls.Add(this.lbwDeviceParams);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.cbxCondition);
            this.Controls.Add(this.lblCondition);
            this.Controls.Add(this.cbxElement);
            this.Controls.Add(this.lblElements);
            this.Name = "UCResolveParams";
            this.Size = new System.Drawing.Size(295, 149);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblElements;
        private System.Windows.Forms.ComboBox cbxElement;
        private System.Windows.Forms.Label lblCondition;
        private System.Windows.Forms.ComboBox cbxCondition;
        private Skyray.Controls.ButtonW btnSubmit;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.LabelW lbwDeviceParams;
        private Skyray.Controls.ComboBoxW cbxDeviceParamsList;

    }
}
