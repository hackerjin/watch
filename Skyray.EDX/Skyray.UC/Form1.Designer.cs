namespace Skyray.UC
{
    partial class Form1
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
            this.listViewWSpecList = new Skyray.Controls.ListViewW();
            this.txtAddColmunName = new Skyray.Controls.TextBoxW();
            this.button1 = new System.Windows.Forms.Button();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.textBoxW1 = new Skyray.Controls.TextBoxW();
            this.SuspendLayout();
            // 
            // listViewWSpecList
            // 
            this.listViewWSpecList.Dock = System.Windows.Forms.DockStyle.Top;
            this.listViewWSpecList.Location = new System.Drawing.Point(0, 0);
            this.listViewWSpecList.Name = "listViewWSpecList";
            this.listViewWSpecList.Scrollable = false;
            this.listViewWSpecList.Size = new System.Drawing.Size(582, 147);
            this.listViewWSpecList.Style = Skyray.Controls.Style.Office2007Blue;
            this.listViewWSpecList.TabIndex = 23;
            this.listViewWSpecList.UseCompatibleStateImageBehavior = false;
            this.listViewWSpecList.View = System.Windows.Forms.View.List;
            this.listViewWSpecList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewWSpecList_ItemSelectionChanged);
            // 
            // txtAddColmunName
            // 
            this.txtAddColmunName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtAddColmunName.Location = new System.Drawing.Point(34, 182);
            this.txtAddColmunName.Name = "txtAddColmunName";
            this.txtAddColmunName.Size = new System.Drawing.Size(120, 21);
            this.txtAddColmunName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtAddColmunName.TabIndex = 61;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(200, 182);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 62;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(0, 153);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(582, 18);
            this.hScrollBar1.TabIndex = 63;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // textBoxW1
            // 
            this.textBoxW1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textBoxW1.Location = new System.Drawing.Point(373, 184);
            this.textBoxW1.Name = "textBoxW1";
            this.textBoxW1.Size = new System.Drawing.Size(120, 21);
            this.textBoxW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.textBoxW1.TabIndex = 64;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 226);
            this.Controls.Add(this.textBoxW1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtAddColmunName);
            this.Controls.Add(this.listViewWSpecList);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ListViewW listViewWSpecList;
        private Skyray.Controls.TextBoxW txtAddColmunName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private Skyray.Controls.TextBoxW textBoxW1;

    }
}