namespace Skyray.UC
{
    partial class UCCalcResolution
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
            this.lblEnergy = new Skyray.Controls.LabelW();
            this.txtEnergy = new Skyray.Controls.TextBoxW();
            this.txtCenterChannel = new Skyray.Controls.TextBoxW();
            this.lblCenterChannel = new Skyray.Controls.LabelW();
            this.txtResolution = new Skyray.Controls.TextBoxW();
            this.lblResolution = new Skyray.Controls.LabelW();
            this.btnCalc = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.lblSpec = new Skyray.Controls.LabelW();
            this.cboSpecID = new Skyray.Controls.ComboBoxW();
            this.SuspendLayout();
            // 
            // lblEnergy
            // 
            this.lblEnergy.AutoSize = true;
            this.lblEnergy.BackColor = System.Drawing.Color.Transparent;
            this.lblEnergy.Location = new System.Drawing.Point(11, 27);
            this.lblEnergy.Name = "lblEnergy";
            this.lblEnergy.Size = new System.Drawing.Size(29, 12);
            this.lblEnergy.TabIndex = 0;
            this.lblEnergy.Text = "能量";
            // 
            // txtEnergy
            // 
            this.txtEnergy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtEnergy.Location = new System.Drawing.Point(78, 21);
            this.txtEnergy.Name = "txtEnergy";
            this.txtEnergy.Size = new System.Drawing.Size(100, 21);
            this.txtEnergy.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtEnergy.TabIndex = 1;
            // 
            // txtCenterChannel
            // 
            this.txtCenterChannel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtCenterChannel.Location = new System.Drawing.Point(78, 66);
            this.txtCenterChannel.Name = "txtCenterChannel";
            this.txtCenterChannel.Size = new System.Drawing.Size(100, 21);
            this.txtCenterChannel.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtCenterChannel.TabIndex = 3;
            // 
            // lblCenterChannel
            // 
            this.lblCenterChannel.AutoSize = true;
            this.lblCenterChannel.BackColor = System.Drawing.Color.Transparent;
            this.lblCenterChannel.Location = new System.Drawing.Point(11, 72);
            this.lblCenterChannel.Name = "lblCenterChannel";
            this.lblCenterChannel.Size = new System.Drawing.Size(41, 12);
            this.lblCenterChannel.TabIndex = 2;
            this.lblCenterChannel.Text = "中心道";
            // 
            // txtResolution
            // 
            this.txtResolution.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtResolution.Location = new System.Drawing.Point(257, 66);
            this.txtResolution.Name = "txtResolution";
            this.txtResolution.Size = new System.Drawing.Size(100, 21);
            this.txtResolution.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtResolution.TabIndex = 5;
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.BackColor = System.Drawing.Color.Transparent;
            this.lblResolution.Location = new System.Drawing.Point(188, 72);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(41, 12);
            this.lblResolution.TabIndex = 4;
            this.lblResolution.Text = "分辨率";
            // 
            // btnCalc
            // 
            this.btnCalc.bSilver = false;
            this.btnCalc.Location = new System.Drawing.Point(90, 113);
            this.btnCalc.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCalc.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(75, 23);
            this.btnCalc.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCalc.TabIndex = 6;
            this.btnCalc.Text = "计算";
            this.btnCalc.ToFocused = false;
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(226, 113);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblSpec
            // 
            this.lblSpec.AutoSize = true;
            this.lblSpec.BackColor = System.Drawing.Color.Transparent;
            this.lblSpec.Location = new System.Drawing.Point(188, 27);
            this.lblSpec.Name = "lblSpec";
            this.lblSpec.Size = new System.Drawing.Size(29, 12);
            this.lblSpec.TabIndex = 8;
            this.lblSpec.Text = "谱图";
            // 
            // cboSpecID
            // 
            this.cboSpecID.AutoComplete = false;
            this.cboSpecID.AutoDropdown = false;
            this.cboSpecID.BackColorEven = System.Drawing.Color.White;
            this.cboSpecID.BackColorOdd = System.Drawing.Color.White;
            this.cboSpecID.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboSpecID.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboSpecID.ColumnNames = "";
            this.cboSpecID.ColumnWidthDefault = 75;
            this.cboSpecID.ColumnWidths = "";
            this.cboSpecID.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboSpecID.FormattingEnabled = true;
            this.cboSpecID.LinkedColumnIndex = 0;
            this.cboSpecID.LinkedTextBox = null;
            this.cboSpecID.Location = new System.Drawing.Point(257, 22);
            this.cboSpecID.Name = "cboSpecID";
            this.cboSpecID.Size = new System.Drawing.Size(100, 22);
            this.cboSpecID.TabIndex = 10;
            this.cboSpecID.SelectedValueChanged += new System.EventHandler(this.cboSpecID_SelectedValueChanged);
            // 
            // UCCalcResolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.cboSpecID);
            this.Controls.Add(this.lblSpec);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.txtResolution);
            this.Controls.Add(this.lblResolution);
            this.Controls.Add(this.txtCenterChannel);
            this.Controls.Add(this.lblCenterChannel);
            this.Controls.Add(this.txtEnergy);
            this.Controls.Add(this.lblEnergy);
            this.Name = "UCCalcResolution";
            this.Size = new System.Drawing.Size(391, 151);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblEnergy;
        private Skyray.Controls.TextBoxW txtEnergy;
        private Skyray.Controls.TextBoxW txtCenterChannel;
        private Skyray.Controls.LabelW lblCenterChannel;
        private Skyray.Controls.TextBoxW txtResolution;
        private Skyray.Controls.LabelW lblResolution;
        private Skyray.Controls.ButtonW btnCalc;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.LabelW lblSpec;
        private Skyray.Controls.ComboBoxW cboSpecID;
    }
}
