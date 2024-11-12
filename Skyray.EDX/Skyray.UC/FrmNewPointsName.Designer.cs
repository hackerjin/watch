namespace Skyray.UC
{
    partial class FrmNewPointsName
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
            this.txtMultiName = new System.Windows.Forms.TextBox();
            this.lblNewPointName = new System.Windows.Forms.Label();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMultiName
            // 
            this.txtMultiName.Location = new System.Drawing.Point(108, 24);
            this.txtMultiName.Name = "txtMultiName";
            this.txtMultiName.Size = new System.Drawing.Size(209, 20);
            this.txtMultiName.TabIndex = 0;
            // 
            // lblNewPointName
            // 
            this.lblNewPointName.AutoSize = true;
            this.lblNewPointName.Location = new System.Drawing.Point(14, 29);
            this.lblNewPointName.Name = "lblNewPointName";
            this.lblNewPointName.Size = new System.Drawing.Size(67, 13);
            this.lblNewPointName.TabIndex = 1;
            this.lblNewPointName.Text = "新多点名：";
            // 
            // btnAccept
            // 
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAccept.Location = new System.Drawing.Point(215, 57);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 25);
            this.btnAccept.TabIndex = 14;
            this.btnAccept.Text = "确  定";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 57);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取 消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmNewPointsName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(433, 94);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.lblNewPointName);
            this.Controls.Add(this.txtMultiName);
            this.Name = "FrmNewPointsName";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "多点名称";
            this.Load += new System.EventHandler(this.FrmNewPointsName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMultiName;
        private System.Windows.Forms.Label lblNewPointName;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
    }
}