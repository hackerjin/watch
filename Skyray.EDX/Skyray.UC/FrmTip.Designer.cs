namespace Skyray.UC
{
    partial class FrmTip
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
            this.labelW1 = new Skyray.Controls.LabelW();
            this.SuspendLayout();
            // 
            // labelW1
            // 
            this.labelW1.BackColor = System.Drawing.Color.Transparent;
            this.labelW1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelW1.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelW1.Location = new System.Drawing.Point(0, 0);
            this.labelW1.Name = "labelW1";
            this.labelW1.Size = new System.Drawing.Size(600, 80);
            this.labelW1.TabIndex = 0;
            this.labelW1.Text = "请稍等...";
            this.labelW1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmTip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 80);
            this.Controls.Add(this.labelW1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTip";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmTip";
            this.TransparencyKey = System.Drawing.Color.White;
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.LabelW labelW1;
    }
}