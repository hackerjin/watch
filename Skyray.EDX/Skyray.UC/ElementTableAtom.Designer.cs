namespace Skyray.UC
{
    partial class ElementTableAtom
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
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(500, 309);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(103, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(608, 309);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(103, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "取消";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ElementTableAtom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Name = "ElementTableAtom";
            this.Size = new System.Drawing.Size(726, 339);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ElemTableGraphic_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ElemTableGraphic_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnClose;
    }
}
