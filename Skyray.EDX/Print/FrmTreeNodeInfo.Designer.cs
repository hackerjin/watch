namespace Skyray.Print
{
    partial class FrmTreeNodeInfo
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboCtrlType = new System.Windows.Forms.ComboBox();
            this.lblCtrlType = new System.Windows.Forms.Label();
            this.btnPicPath = new System.Windows.Forms.Button();
            this.openFileDialogPicture = new System.Windows.Forms.OpenFileDialog();
            this.txtGroup = new System.Windows.Forms.TextBox();
            this.lblGroup = new System.Windows.Forms.Label();
            this.lblImage = new System.Windows.Forms.Label();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(21, 34);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(41, 12);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "名称：";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(68, 31);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(121, 21);
            this.txtName.TabIndex = 2;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(21, 61);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(41, 12);
            this.lblText.TabIndex = 3;
            this.lblText.Text = "文本：";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(68, 58);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(121, 21);
            this.txtText.TabIndex = 4;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 167);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(114, 167);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cboCtrlType
            // 
            this.cboCtrlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCtrlType.FormattingEnabled = true;
            this.cboCtrlType.Location = new System.Drawing.Point(68, 86);
            this.cboCtrlType.Name = "cboCtrlType";
            this.cboCtrlType.Size = new System.Drawing.Size(121, 20);
            this.cboCtrlType.TabIndex = 7;
            this.cboCtrlType.SelectedIndexChanged += new System.EventHandler(this.cboCtrlType_SelectedIndexChanged);
            // 
            // lblCtrlType
            // 
            this.lblCtrlType.AutoSize = true;
            this.lblCtrlType.Location = new System.Drawing.Point(21, 94);
            this.lblCtrlType.Name = "lblCtrlType";
            this.lblCtrlType.Size = new System.Drawing.Size(41, 12);
            this.lblCtrlType.TabIndex = 8;
            this.lblCtrlType.Text = "类别：";
            // 
            // btnPicPath
            // 
            this.btnPicPath.Location = new System.Drawing.Point(68, 112);
            this.btnPicPath.Name = "btnPicPath";
            this.btnPicPath.Size = new System.Drawing.Size(121, 23);
            this.btnPicPath.TabIndex = 9;
            this.btnPicPath.Text = "...";
            this.btnPicPath.UseVisualStyleBackColor = true;
            this.btnPicPath.Click += new System.EventHandler(this.btnPicPath_Click);
            // 
            // openFileDialogPicture
            // 
            this.openFileDialogPicture.FileName = "openFileDialog1";
            // 
            // txtGroup
            // 
            this.txtGroup.Enabled = false;
            this.txtGroup.Location = new System.Drawing.Point(66, 6);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(123, 21);
            this.txtGroup.TabIndex = 11;
            // 
            // lblGroup
            // 
            this.lblGroup.AutoSize = true;
            this.lblGroup.Location = new System.Drawing.Point(21, 9);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(41, 12);
            this.lblGroup.TabIndex = 10;
            this.lblGroup.Text = "组别：";
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.Location = new System.Drawing.Point(21, 117);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(41, 12);
            this.lblImage.TabIndex = 12;
            this.lblImage.Text = "图片：";
            // 
            // txtLabel
            // 
            this.txtLabel.Location = new System.Drawing.Point(70, 140);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(121, 21);
            this.txtLabel.TabIndex = 14;
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(23, 143);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(41, 12);
            this.lblContent.TabIndex = 13;
            this.lblContent.Text = "内容：";
            // 
            // FrmTreeNodeInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(215, 199);
            this.Controls.Add(this.txtLabel);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.txtGroup);
            this.Controls.Add(this.lblGroup);
            this.Controls.Add(this.btnPicPath);
            this.Controls.Add(this.lblCtrlType);
            this.Controls.Add(this.cboCtrlType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTreeNodeInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "节点添加";
            this.Load += new System.EventHandler(this.FrmTreeNodeInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cboCtrlType;
        private System.Windows.Forms.Label lblCtrlType;
        private System.Windows.Forms.Button btnPicPath;
        private System.Windows.Forms.OpenFileDialog openFileDialogPicture;
        private System.Windows.Forms.TextBox txtGroup;
        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.Label lblContent;
    }
}