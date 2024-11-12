namespace Skyray.UC
{
    partial class UCSingStand
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
            this.textBoxW2 = new Skyray.Controls.TextBoxW();
            this.lblfuhao = new Skyray.Controls.LabelW();
            this.textBoxW1 = new Skyray.Controls.TextBoxW();
            this.lblfloorTxt = new Skyray.Controls.LabelW();
            this.SuspendLayout();
            // 
            // textBoxW2
            // 
            this.textBoxW2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textBoxW2.Location = new System.Drawing.Point(134, 4);
            this.textBoxW2.Name = "textBoxW2";
            this.textBoxW2.Size = new System.Drawing.Size(52, 20);
            this.textBoxW2.Style = Skyray.Controls.Style.Office2007Blue;
            this.textBoxW2.TabIndex = 26;
            // 
            // lblfuhao
            // 
            this.lblfuhao.AutoSize = true;
            this.lblfuhao.BackColor = System.Drawing.Color.Transparent;
            this.lblfuhao.Location = new System.Drawing.Point(114, 9);
            this.lblfuhao.Name = "lblfuhao";
            this.lblfuhao.Size = new System.Drawing.Size(14, 13);
            this.lblfuhao.TabIndex = 25;
            this.lblfuhao.Text = "~";
            // 
            // textBoxW1
            // 
            this.textBoxW1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textBoxW1.Location = new System.Drawing.Point(56, 5);
            this.textBoxW1.Name = "textBoxW1";
            this.textBoxW1.Size = new System.Drawing.Size(52, 20);
            this.textBoxW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.textBoxW1.TabIndex = 24;
            // 
            // lblfloorTxt
            // 
            this.lblfloorTxt.AutoSize = true;
            this.lblfloorTxt.BackColor = System.Drawing.Color.Transparent;
            this.lblfloorTxt.Location = new System.Drawing.Point(11, 8);
            this.lblfloorTxt.Name = "lblfloorTxt";
            this.lblfloorTxt.Size = new System.Drawing.Size(43, 13);
            this.lblfloorTxt.TabIndex = 23;
            this.lblfloorTxt.Text = "第一层";
            // 
            // UCSingStand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.textBoxW2);
            this.Controls.Add(this.lblfuhao);
            this.Controls.Add(this.textBoxW1);
            this.Controls.Add(this.lblfloorTxt);
            this.Name = "UCSingStand";
            this.Size = new System.Drawing.Size(195, 28);
            this.Load += new System.EventHandler(this.UCSingStand_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.TextBoxW textBoxW2;
        private Skyray.Controls.LabelW lblfuhao;
        private Skyray.Controls.TextBoxW textBoxW1;
        private Skyray.Controls.LabelW lblfloorTxt;

    }
}
