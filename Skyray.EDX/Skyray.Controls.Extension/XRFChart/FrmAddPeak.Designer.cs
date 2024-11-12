namespace Skyray.Controls.Extension
{
    partial class FrmAddPeak
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
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.lblElementSeleted = new System.Windows.Forms.Label();
            this.lblLine = new System.Windows.Forms.Label();
            this.btnSel = new Skyray.Controls.ButtonW();
            this.txtElementSelected = new Skyray.Controls.TextBoxW();
            this.cbwLine = new Skyray.Controls.ComboBoxW();
            this.lblCustomChannel = new System.Windows.Forms.Label();
            this.numCustomChannel = new Skyray.Controls.NumricUpDownW();
            ((System.ComponentModel.ISupportInitialize)(this.numCustomChannel)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(76, 125);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(83, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(221, 125);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblElementSeleted
            // 
            this.lblElementSeleted.AutoSize = true;
            this.lblElementSeleted.Location = new System.Drawing.Point(25, 34);
            this.lblElementSeleted.Name = "lblElementSeleted";
            this.lblElementSeleted.Size = new System.Drawing.Size(29, 12);
            this.lblElementSeleted.TabIndex = 2;
            this.lblElementSeleted.Text = "元素";
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(200, 34);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(29, 12);
            this.lblLine.TabIndex = 3;
            this.lblLine.Text = "线系";
            // 
            // btnSel
            // 
            this.btnSel.bSilver = false;
            this.btnSel.Location = new System.Drawing.Point(169, 30);
            this.btnSel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSel.Name = "btnSel";
            this.btnSel.Size = new System.Drawing.Size(24, 19);
            this.btnSel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSel.TabIndex = 20;
            this.btnSel.Text = "...";
            this.btnSel.ToFocused = false;
            this.btnSel.UseVisualStyleBackColor = true;
            this.btnSel.Click += new System.EventHandler(this.btnSelectElement_Click);
            // 
            // txtElementSelected
            // 
            this.txtElementSelected.BackColor = System.Drawing.Color.White;
            this.txtElementSelected.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtElementSelected.Location = new System.Drawing.Point(94, 29);
            this.txtElementSelected.Name = "txtElementSelected";
            this.txtElementSelected.ReadOnly = true;
            this.txtElementSelected.Size = new System.Drawing.Size(100, 21);
            this.txtElementSelected.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtElementSelected.TabIndex = 19;
            this.txtElementSelected.Click += new System.EventHandler(this.btnSelectElement_Click);
            // 
            // cbwLine
            // 
            this.cbwLine.AutoComplete = false;
            this.cbwLine.AutoDropdown = false;
            this.cbwLine.BackColorEven = System.Drawing.Color.White;
            this.cbwLine.BackColorOdd = System.Drawing.Color.White;
            this.cbwLine.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cbwLine.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cbwLine.ColumnNames = "";
            this.cbwLine.ColumnWidthDefault = 75;
            this.cbwLine.ColumnWidths = "";
            this.cbwLine.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbwLine.FormattingEnabled = true;
            this.cbwLine.LinkedColumnIndex = 0;
            this.cbwLine.LinkedTextBox = null;
            this.cbwLine.Location = new System.Drawing.Point(249, 29);
            this.cbwLine.Name = "cbwLine";
            this.cbwLine.Size = new System.Drawing.Size(98, 22);
            this.cbwLine.TabIndex = 21;
            // 
            // lblCustomChannel
            // 
            this.lblCustomChannel.AutoSize = true;
            this.lblCustomChannel.Location = new System.Drawing.Point(24, 84);
            this.lblCustomChannel.Name = "lblCustomChannel";
            this.lblCustomChannel.Size = new System.Drawing.Size(65, 12);
            this.lblCustomChannel.TabIndex = 22;
            this.lblCustomChannel.Text = "自定义通道";
            // 
            // numCustomChannel
            // 
            this.numCustomChannel.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numCustomChannel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numCustomChannel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numCustomChannel.DecimalPlaces = 1;
            this.numCustomChannel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numCustomChannel.Location = new System.Drawing.Point(94, 78);
            this.numCustomChannel.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numCustomChannel.Name = "numCustomChannel";
            this.numCustomChannel.Size = new System.Drawing.Size(100, 21);
            this.numCustomChannel.TabIndex = 24;
            // 
            // FrmAddPeak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(380, 167);
            this.Controls.Add(this.numCustomChannel);
            this.Controls.Add(this.lblCustomChannel);
            this.Controls.Add(this.cbwLine);
            this.Controls.Add(this.btnSel);
            this.Controls.Add(this.txtElementSelected);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.lblElementSeleted);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddPeak";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加峰标识";
            ((System.ComponentModel.ISupportInitialize)(this.numCustomChannel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private System.Windows.Forms.Label lblElementSeleted;
        private System.Windows.Forms.Label lblLine;
        private Skyray.Controls.ButtonW btnSel;
        private Skyray.Controls.TextBoxW txtElementSelected;
        private Skyray.Controls.ComboBoxW cbwLine;
        private System.Windows.Forms.Label lblCustomChannel;
        private NumricUpDownW numCustomChannel;
    }
}