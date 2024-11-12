namespace Skyray.UC
{
    partial class RegisterForm
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
            this.licenseTextBox = new System.Windows.Forms.TextBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.serialTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // licenseTextBox
            // 
            this.licenseTextBox.Location = new System.Drawing.Point(72, 58);
            this.licenseTextBox.Name = "licenseTextBox";
            this.licenseTextBox.Size = new System.Drawing.Size(331, 21);
            this.licenseTextBox.TabIndex = 11;
            // 
            // exitButton
            // 
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(328, 97);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 10;
            this.exitButton.Text = "退出";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(222, 97);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(75, 23);
            this.registerButton.TabIndex = 9;
            this.registerButton.Text = "注册";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "注册码：";
            // 
            // serialTextBox
            // 
            this.serialTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.serialTextBox.Location = new System.Drawing.Point(72, 25);
            this.serialTextBox.Name = "serialTextBox";
            this.serialTextBox.ReadOnly = true;
            this.serialTextBox.Size = new System.Drawing.Size(331, 21);
            this.serialTextBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "序列号：";
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 137);
            this.Controls.Add(this.licenseTextBox);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.serialTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "注册"; 
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox licenseTextBox;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serialTextBox;
        private System.Windows.Forms.Label label1;
    }
}