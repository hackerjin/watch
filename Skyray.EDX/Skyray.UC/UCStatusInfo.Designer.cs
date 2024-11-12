namespace Skyray.UC
{
    partial class UCStatusInfo
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
            this.labelSet = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelSupus = new System.Windows.Forms.Label();
            this.labelFinishTime = new System.Windows.Forms.Label();
            this.labelSetingTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelSet
            // 
            this.labelSet.AutoSize = true;
            this.labelSet.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSet.Font = new System.Drawing.Font("SimSun", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelSet.Location = new System.Drawing.Point(8, 8);
            this.labelSet.Name = "labelSet";
            this.labelSet.Size = new System.Drawing.Size(59, 13);
            this.labelSet.TabIndex = 0;
            this.labelSet.Text = "预置时间";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(124, 6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(407, 15);
            this.progressBar1.TabIndex = 2;
            // 
            // labelSupus
            // 
            this.labelSupus.AutoSize = true;
            this.labelSupus.Font = new System.Drawing.Font("SimSun", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSupus.ForeColor = System.Drawing.Color.Blue;
            this.labelSupus.Location = new System.Drawing.Point(539, 6);
            this.labelSupus.Name = "labelSupus";
            this.labelSupus.Size = new System.Drawing.Size(59, 13);
            this.labelSupus.TabIndex = 3;
            this.labelSupus.Text = "剩余时间";
            // 
            // labelFinishTime
            // 
            this.labelFinishTime.AutoSize = true;
            this.labelFinishTime.Location = new System.Drawing.Point(611, 8);
            this.labelFinishTime.Name = "labelFinishTime";
            this.labelFinishTime.Size = new System.Drawing.Size(11, 12);
            this.labelFinishTime.TabIndex = 4;
            this.labelFinishTime.Text = "0";
            // 
            // labelSetingTime
            // 
            this.labelSetingTime.AutoSize = true;
            this.labelSetingTime.Location = new System.Drawing.Point(87, 8);
            this.labelSetingTime.Name = "labelSetingTime";
            this.labelSetingTime.Size = new System.Drawing.Size(11, 12);
            this.labelSetingTime.TabIndex = 5;
            this.labelSetingTime.Text = "0";
            // 
            // UCStatusInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.labelSetingTime);
            this.Controls.Add(this.labelFinishTime);
            this.Controls.Add(this.labelSupus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.labelSet);
            this.Name = "UCStatusInfo";
            this.Size = new System.Drawing.Size(917, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSet;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelSupus;
        private System.Windows.Forms.Label labelFinishTime;
        private System.Windows.Forms.Label labelSetingTime;
    }
}
