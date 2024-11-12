namespace Skyray.UC
{
    partial class FrmMeasureSetting
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
            this.labelInputTime = new System.Windows.Forms.Label();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numricUpDownW1 = new Skyray.Controls.NumricUpDownW();
            ((System.ComponentModel.ISupportInitialize)(this.numricUpDownW1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInputTime
            // 
            this.labelInputTime.AutoSize = true;
            this.labelInputTime.Location = new System.Drawing.Point(51, 33);
            this.labelInputTime.Name = "labelInputTime";
            this.labelInputTime.Size = new System.Drawing.Size(161, 12);
            this.labelInputTime.TabIndex = 0;
            this.labelInputTime.Text = "输入测量时间：（0～9999s）";
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(131, 87);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonSubmit.TabIndex = 2;
            this.buttonSubmit.Text = "确定";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(234, 87);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // numricUpDownW1
            // 
            this.numricUpDownW1.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numricUpDownW1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numricUpDownW1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numricUpDownW1.Location = new System.Drawing.Point(53, 48);
            this.numricUpDownW1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numricUpDownW1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numricUpDownW1.Name = "numricUpDownW1";
            this.numricUpDownW1.Size = new System.Drawing.Size(143, 21);
            this.numricUpDownW1.TabIndex = 5;
            this.numricUpDownW1.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // FrmMeasureSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numricUpDownW1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.labelInputTime);
            this.Name = "FrmMeasureSetting";
            this.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Size = new System.Drawing.Size(323, 118);
            ((System.ComponentModel.ISupportInitialize)(this.numricUpDownW1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInputTime;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.Button buttonCancel;
        private Skyray.Controls.NumricUpDownW numricUpDownW1;
    }
}