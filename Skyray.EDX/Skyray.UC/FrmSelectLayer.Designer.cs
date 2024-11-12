namespace Skyray.UC
{
    partial class FrmSelectLayer
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
            this.radSingle = new System.Windows.Forms.RadioButton();
            this.radMultiLayer = new System.Windows.Forms.RadioButton();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.grpSelect = new System.Windows.Forms.GroupBox();
            this.grpSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // radSingle
            // 
            this.radSingle.AutoSize = true;
            this.radSingle.Location = new System.Drawing.Point(49, 35);
            this.radSingle.Name = "radSingle";
            this.radSingle.Size = new System.Drawing.Size(47, 16);
            this.radSingle.TabIndex = 0;
            this.radSingle.TabStop = true;
            this.radSingle.Text = "单层";
            this.radSingle.UseVisualStyleBackColor = true;
            this.radSingle.CheckedChanged += new System.EventHandler(this.radSingle_CheckedChanged);
            // 
            // radMultiLayer
            // 
            this.radMultiLayer.AutoSize = true;
            this.radMultiLayer.Location = new System.Drawing.Point(164, 35);
            this.radMultiLayer.Name = "radMultiLayer";
            this.radMultiLayer.Size = new System.Drawing.Size(47, 16);
            this.radMultiLayer.TabIndex = 1;
            this.radMultiLayer.TabStop = true;
            this.radMultiLayer.Text = "多层";
            this.radMultiLayer.UseVisualStyleBackColor = true;
            this.radMultiLayer.CheckedChanged += new System.EventHandler(this.radMultiLayer_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(49, 78);
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
            this.btnCancel.Location = new System.Drawing.Point(164, 78);
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
            // grpSelect
            // 
            this.grpSelect.Controls.Add(this.radMultiLayer);
            this.grpSelect.Controls.Add(this.btnCancel);
            this.grpSelect.Controls.Add(this.radSingle);
            this.grpSelect.Controls.Add(this.btnOK);
            this.grpSelect.Location = new System.Drawing.Point(16, 9);
            this.grpSelect.Name = "grpSelect";
            this.grpSelect.Size = new System.Drawing.Size(282, 111);
            this.grpSelect.TabIndex = 4;
            this.grpSelect.TabStop = false;
            // 
            // FrmSelectLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(312, 135);
            this.Controls.Add(this.grpSelect);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectLayer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择层级";
            this.grpSelect.ResumeLayout(false);
            this.grpSelect.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radSingle;
        private System.Windows.Forms.RadioButton radMultiLayer;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private System.Windows.Forms.GroupBox grpSelect;
    }
}