namespace Skyray.UC
{
    partial class FrmCheckInCal
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
            this.radioPure = new System.Windows.Forms.RadioButton();
            this.radioConSample = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioCalibratePar = new System.Windows.Forms.RadioButton();
            this.radioInitialPar = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioPure
            // 
            this.radioPure.AutoSize = true;
            this.radioPure.Location = new System.Drawing.Point(59, 59);
            this.radioPure.Name = "radioPure";
            this.radioPure.Size = new System.Drawing.Size(83, 16);
            this.radioPure.TabIndex = 0;
            this.radioPure.TabStop = true;
            this.radioPure.Text = "纯元素校正";
            this.radioPure.UseVisualStyleBackColor = true;
            this.radioPure.CheckedChanged += new System.EventHandler(this.radioPure_CheckedChanged);
            // 
            // radioConSample
            // 
            this.radioConSample.AutoSize = true;
            this.radioConSample.Location = new System.Drawing.Point(59, 28);
            this.radioConSample.Name = "radioConSample";
            this.radioConSample.Size = new System.Drawing.Size(71, 16);
            this.radioConSample.TabIndex = 1;
            this.radioConSample.TabStop = true;
            this.radioConSample.Text = "控样校正";
            this.radioConSample.UseVisualStyleBackColor = true;
            this.radioConSample.CheckedChanged += new System.EventHandler(this.radioConSample_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(45, 105);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(85, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(162, 105);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(85, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioCalibratePar);
            this.groupBox1.Controls.Add(this.radioInitialPar);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(143, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(113, 79);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // radioCalibratePar
            // 
            this.radioCalibratePar.AutoSize = true;
            this.radioCalibratePar.Location = new System.Drawing.Point(19, 51);
            this.radioCalibratePar.Name = "radioCalibratePar";
            this.radioCalibratePar.Size = new System.Drawing.Size(71, 16);
            this.radioCalibratePar.TabIndex = 3;
            this.radioCalibratePar.TabStop = true;
            this.radioCalibratePar.Text = "校正参数";
            this.radioCalibratePar.UseVisualStyleBackColor = true;
            this.radioCalibratePar.CheckedChanged += new System.EventHandler(this.radioCalibratePar_CheckedChanged);
            // 
            // radioInitialPar
            // 
            this.radioInitialPar.AutoSize = true;
            this.radioInitialPar.Location = new System.Drawing.Point(19, 20);
            this.radioInitialPar.Name = "radioInitialPar";
            this.radioInitialPar.Size = new System.Drawing.Size(71, 16);
            this.radioInitialPar.TabIndex = 2;
            this.radioInitialPar.TabStop = true;
            this.radioInitialPar.Text = "初始参数";
            this.radioInitialPar.UseVisualStyleBackColor = true;
            this.radioInitialPar.CheckedChanged += new System.EventHandler(this.radioInitialPar_CheckedChanged);
            // 
            // FrmCheckInCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 140);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.radioConSample);
            this.Controls.Add(this.radioPure);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCheckInCal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "强度偏移校正";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioPure;
        private System.Windows.Forms.RadioButton radioConSample;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioCalibratePar;
        private System.Windows.Forms.RadioButton radioInitialPar;
    }
}