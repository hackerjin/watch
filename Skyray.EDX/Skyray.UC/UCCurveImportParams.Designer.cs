namespace Skyray.UC
{
    partial class UCCurveImportParams
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbWCurvePath = new Skyray.Controls.LabelW();
            this.textBoxW1 = new Skyray.Controls.TextBoxW();
            this.btnSelectPath = new Skyray.Controls.ButtonW();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblCurveName = new Skyray.Controls.LabelW();
            this.lblConditionName = new Skyray.Controls.LabelW();
            this.grpSettingParams = new Skyray.Controls.Grouper();
            this.chbCurveReName = new Skyray.Controls.CheckBoxW();
            this.txbReNameConditon = new Skyray.Controls.TextBoxW();
            this.txbCurveReName = new Skyray.Controls.TextBoxW();
            this.btnSubmit = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.rdoModifyCondition = new System.Windows.Forms.RadioButton();
            this.rdoUseOldCondition = new System.Windows.Forms.RadioButton();
            this.rdoNotLoadCondition = new System.Windows.Forms.RadioButton();
            this.rdoManControl = new System.Windows.Forms.RadioButton();
            this.grpConditin = new Skyray.Controls.Grouper();
            this.grpSettingParams.SuspendLayout();
            this.grpConditin.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbWCurvePath
            // 
            this.lbWCurvePath.AutoSize = true;
            this.lbWCurvePath.BackColor = System.Drawing.Color.Transparent;
            this.lbWCurvePath.Location = new System.Drawing.Point(38, 38);
            this.lbWCurvePath.Name = "lbWCurvePath";
            this.lbWCurvePath.Size = new System.Drawing.Size(89, 12);
            this.lbWCurvePath.TabIndex = 0;
            this.lbWCurvePath.Text = "曲线导入路径：";
            // 
            // textBoxW1
            // 
            this.textBoxW1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textBoxW1.Location = new System.Drawing.Point(133, 32);
            this.textBoxW1.Name = "textBoxW1";
            this.textBoxW1.ReadOnly = true;
            this.textBoxW1.Size = new System.Drawing.Size(207, 21);
            this.textBoxW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.textBoxW1.TabIndex = 1;
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.bSilver = false;
            this.btnSelectPath.Location = new System.Drawing.Point(346, 30);
            this.btnSelectPath.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSelectPath.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(26, 23);
            this.btnSelectPath.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSelectPath.TabIndex = 2;
            this.btnSelectPath.Text = "...";
            this.btnSelectPath.ToFocused = false;
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "XML file (*.xml)|*.xml";
            // 
            // lblCurveName
            // 
            this.lblCurveName.AutoSize = true;
            this.lblCurveName.BackColor = System.Drawing.Color.Transparent;
            this.lblCurveName.Location = new System.Drawing.Point(62, 70);
            this.lblCurveName.Name = "lblCurveName";
            this.lblCurveName.Size = new System.Drawing.Size(65, 12);
            this.lblCurveName.TabIndex = 3;
            this.lblCurveName.Text = "曲线名称：";
            // 
            // lblConditionName
            // 
            this.lblConditionName.AutoSize = true;
            this.lblConditionName.BackColor = System.Drawing.Color.Transparent;
            this.lblConditionName.Location = new System.Drawing.Point(62, 98);
            this.lblConditionName.Name = "lblConditionName";
            this.lblConditionName.Size = new System.Drawing.Size(65, 12);
            this.lblConditionName.TabIndex = 5;
            this.lblConditionName.Text = "条件名称：";
            // 
            // grpSettingParams
            // 
            this.grpSettingParams.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpSettingParams.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpSettingParams.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpSettingParams.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpSettingParams.BorderThickness = 1F;
            this.grpSettingParams.BorderTopOnly = false;
            this.grpSettingParams.Controls.Add(this.chbCurveReName);
            this.grpSettingParams.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpSettingParams.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpSettingParams.GroupImage = null;
            this.grpSettingParams.GroupTitle = "曲线重名";
            this.grpSettingParams.HeaderRoundCorners = 4;
            this.grpSettingParams.Location = new System.Drawing.Point(51, 181);
            this.grpSettingParams.Name = "grpSettingParams";
            this.grpSettingParams.PaintGroupBox = false;
            this.grpSettingParams.RoundCorners = 4;
            this.grpSettingParams.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpSettingParams.ShadowControl = false;
            this.grpSettingParams.ShadowThickness = 3;
            this.grpSettingParams.Size = new System.Drawing.Size(200, 62);
            this.grpSettingParams.TabIndex = 7;
            this.grpSettingParams.TextLineSpace = 2;
            this.grpSettingParams.TitleLeftSpace = 18;
            // 
            // chbCurveReName
            // 
            this.chbCurveReName.AutoSize = true;
            this.chbCurveReName.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chbCurveReName.Checked = true;
            this.chbCurveReName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbCurveReName.Location = new System.Drawing.Point(11, 29);
            this.chbCurveReName.Name = "chbCurveReName";
            this.chbCurveReName.Size = new System.Drawing.Size(96, 16);
            this.chbCurveReName.Style = Skyray.Controls.Style.Office2007Blue;
            this.chbCurveReName.TabIndex = 0;
            this.chbCurveReName.Text = "重名曲线覆盖";
            this.chbCurveReName.UseVisualStyleBackColor = true;
            // 
            // txbReNameConditon
            // 
            this.txbReNameConditon.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txbReNameConditon.Location = new System.Drawing.Point(133, 96);
            this.txbReNameConditon.Name = "txbReNameConditon";
            this.txbReNameConditon.Size = new System.Drawing.Size(172, 21);
            this.txbReNameConditon.Style = Skyray.Controls.Style.Office2007Blue;
            this.txbReNameConditon.TabIndex = 3;
            // 
            // txbCurveReName
            // 
            this.txbCurveReName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txbCurveReName.Location = new System.Drawing.Point(133, 65);
            this.txbCurveReName.Name = "txbCurveReName";
            this.txbCurveReName.Size = new System.Drawing.Size(173, 21);
            this.txbCurveReName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txbCurveReName.TabIndex = 2;
            // 
            // btnSubmit
            // 
            this.btnSubmit.bSilver = false;
            this.btnSubmit.Location = new System.Drawing.Point(316, 257);
            this.btnSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmit.TabIndex = 8;
            this.btnSubmit.Text = "确认";
            this.btnSubmit.ToFocused = false;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(421, 256);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rdoModifyCondition
            // 
            this.rdoModifyCondition.AutoSize = true;
            this.rdoModifyCondition.Location = new System.Drawing.Point(17, 40);
            this.rdoModifyCondition.Name = "rdoModifyCondition";
            this.rdoModifyCondition.Size = new System.Drawing.Size(95, 16);
            this.rdoModifyCondition.TabIndex = 2;
            this.rdoModifyCondition.TabStop = true;
            this.rdoModifyCondition.Text = "覆盖原有条件";
            this.rdoModifyCondition.UseVisualStyleBackColor = true;
            // 
            // rdoUseOldCondition
            // 
            this.rdoUseOldCondition.AutoSize = true;
            this.rdoUseOldCondition.Location = new System.Drawing.Point(17, 21);
            this.rdoUseOldCondition.Name = "rdoUseOldCondition";
            this.rdoUseOldCondition.Size = new System.Drawing.Size(95, 16);
            this.rdoUseOldCondition.TabIndex = 3;
            this.rdoUseOldCondition.TabStop = true;
            this.rdoUseOldCondition.Text = "使用原有条件";
            this.rdoUseOldCondition.UseVisualStyleBackColor = true;
            // 
            // rdoNotLoadCondition
            // 
            this.rdoNotLoadCondition.AutoSize = true;
            this.rdoNotLoadCondition.Location = new System.Drawing.Point(103, 137);
            this.rdoNotLoadCondition.Name = "rdoNotLoadCondition";
            this.rdoNotLoadCondition.Size = new System.Drawing.Size(95, 16);
            this.rdoNotLoadCondition.TabIndex = 4;
            this.rdoNotLoadCondition.TabStop = true;
            this.rdoNotLoadCondition.Text = "重名禁止导入";
            this.rdoNotLoadCondition.UseVisualStyleBackColor = true;
            this.rdoNotLoadCondition.CheckedChanged += new System.EventHandler(this.rdoNotLoadCondition_CheckedChanged);
            // 
            // rdoManControl
            // 
            this.rdoManControl.AutoSize = true;
            this.rdoManControl.Location = new System.Drawing.Point(103, 159);
            this.rdoManControl.Name = "rdoManControl";
            this.rdoManControl.Size = new System.Drawing.Size(95, 16);
            this.rdoManControl.TabIndex = 10;
            this.rdoManControl.TabStop = true;
            this.rdoManControl.Text = "重名特殊处理";
            this.rdoManControl.UseVisualStyleBackColor = true;
            this.rdoManControl.CheckedChanged += new System.EventHandler(this.rdoManControl_CheckedChanged);
            // 
            // grpConditin
            // 
            this.grpConditin.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpConditin.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpConditin.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpConditin.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpConditin.BorderThickness = 1F;
            this.grpConditin.BorderTopOnly = false;
            this.grpConditin.Controls.Add(this.rdoUseOldCondition);
            this.grpConditin.Controls.Add(this.rdoModifyCondition);
            this.grpConditin.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpConditin.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpConditin.GroupImage = null;
            this.grpConditin.GroupTitle = "条件重名";
            this.grpConditin.HeaderRoundCorners = 4;
            this.grpConditin.Location = new System.Drawing.Point(267, 181);
            this.grpConditin.Name = "grpConditin";
            this.grpConditin.PaintGroupBox = false;
            this.grpConditin.RoundCorners = 4;
            this.grpConditin.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpConditin.ShadowControl = false;
            this.grpConditin.ShadowThickness = 3;
            this.grpConditin.Size = new System.Drawing.Size(226, 62);
            this.grpConditin.TabIndex = 11;
            this.grpConditin.TextLineSpace = 2;
            this.grpConditin.TitleLeftSpace = 18;
            // 
            // UCCurveImportParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpConditin);
            this.Controls.Add(this.rdoManControl);
            this.Controls.Add(this.rdoNotLoadCondition);
            this.Controls.Add(this.txbReNameConditon);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txbCurveReName);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.grpSettingParams);
            this.Controls.Add(this.lblConditionName);
            this.Controls.Add(this.lblCurveName);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.textBoxW1);
            this.Controls.Add(this.lbWCurvePath);
            this.Name = "UCCurveImportParams";
            this.Size = new System.Drawing.Size(504, 286);
            this.grpSettingParams.ResumeLayout(false);
            this.grpSettingParams.PerformLayout();
            this.grpConditin.ResumeLayout(false);
            this.grpConditin.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lbWCurvePath;
        private Skyray.Controls.TextBoxW textBoxW1;
        private Skyray.Controls.ButtonW btnSelectPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Skyray.Controls.LabelW lblCurveName;
        private Skyray.Controls.LabelW lblConditionName;
        private Skyray.Controls.Grouper grpSettingParams;
        private Skyray.Controls.ButtonW btnSubmit;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.CheckBoxW chbCurveReName;
        private Skyray.Controls.TextBoxW txbReNameConditon;
        private Skyray.Controls.TextBoxW txbCurveReName;
        private System.Windows.Forms.RadioButton rdoUseOldCondition;
        private System.Windows.Forms.RadioButton rdoModifyCondition;
        private System.Windows.Forms.RadioButton rdoNotLoadCondition;
        private System.Windows.Forms.RadioButton rdoManControl;
        private Skyray.Controls.Grouper grpConditin;
    }
}
