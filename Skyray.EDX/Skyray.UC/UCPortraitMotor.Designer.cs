namespace Skyray.UC
{
    partial class UCPortraitMotor
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
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonStop1 = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonUp
            // 
            this.buttonUp.Image = global::Skyray.UC.Properties.Resources.go_up;
            this.buttonUp.Location = new System.Drawing.Point(12, 9);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(56, 40);
            this.buttonUp.TabIndex = 0;
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Image = global::Skyray.UC.Properties.Resources.down;
            this.buttonDown.Location = new System.Drawing.Point(74, 9);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(56, 40);
            this.buttonDown.TabIndex = 1;
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonDown_MouseDown);
            this.buttonDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonDown_MouseUp);
            // 
            // buttonStop1
            // 
            this.buttonStop1.Image = global::Skyray.UC.Properties.Resources.StopTest;
            this.buttonStop1.Location = new System.Drawing.Point(136, 9);
            this.buttonStop1.Name = "buttonStop1";
            this.buttonStop1.Size = new System.Drawing.Size(56, 40);
            this.buttonStop1.TabIndex = 2;
            this.buttonStop1.UseVisualStyleBackColor = true;
            this.buttonStop1.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Image = global::Skyray.UC.Properties.Resources.退出24;
            this.buttonExit.Location = new System.Drawing.Point(198, 9);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(56, 40);
            this.buttonExit.TabIndex = 3;
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // UCPortraitMotor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonStop1);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Name = "UCPortraitMotor";
            this.Size = new System.Drawing.Size(257, 57);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonStop1;
        private System.Windows.Forms.Button buttonExit;

    }
}
