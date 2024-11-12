namespace Skyray.UC
{
    partial class UCSampleCalSet
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
            this.btnSelectElement = new Skyray.Controls.ButtonW();
            this.dgvwStandardDatas = new Skyray.Controls.DataGridViewW();
            this.txtNewStandard = new Skyray.Controls.TextBoxW();
            this.lblSampleName = new Skyray.Controls.LabelW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwStandardDatas)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectElement
            // 
            this.btnSelectElement.bSilver = false;
            this.btnSelectElement.Location = new System.Drawing.Point(418, 148);
            this.btnSelectElement.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSelectElement.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSelectElement.Name = "btnSelectElement";
            this.btnSelectElement.Size = new System.Drawing.Size(113, 23);
            this.btnSelectElement.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSelectElement.TabIndex = 23;
            this.btnSelectElement.Text = "选择元素";
            this.btnSelectElement.ToFocused = false;
            this.btnSelectElement.UseVisualStyleBackColor = true;
            this.btnSelectElement.Click += new System.EventHandler(this.btnSelectElement_Click);
            // 
            // dgvwStandardDatas
            // 
            this.dgvwStandardDatas.AllowUserToAddRows = false;
            this.dgvwStandardDatas.AllowUserToResizeColumns = false;
            this.dgvwStandardDatas.AllowUserToResizeRows = false;
            this.dgvwStandardDatas.BackgroundColor = System.Drawing.Color.White;
            this.dgvwStandardDatas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwStandardDatas.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwStandardDatas.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwStandardDatas.ColumnHeadersHeight = 24;
            this.dgvwStandardDatas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwStandardDatas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwStandardDatas.Location = new System.Drawing.Point(11, 11);
            this.dgvwStandardDatas.Name = "dgvwStandardDatas";
            this.dgvwStandardDatas.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwStandardDatas.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwStandardDatas.RowHeadersVisible = false;
            this.dgvwStandardDatas.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwStandardDatas.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwStandardDatas.RowTemplate.Height = 23;
            this.dgvwStandardDatas.SecondaryLength = 1;
            this.dgvwStandardDatas.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwStandardDatas.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwStandardDatas.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwStandardDatas.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.dgvwStandardDatas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwStandardDatas.ShowEportContextMenu = true;
            this.dgvwStandardDatas.Size = new System.Drawing.Size(368, 323);
            this.dgvwStandardDatas.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwStandardDatas.TabIndex = 25;
            this.dgvwStandardDatas.ToPrintCols = null;
            this.dgvwStandardDatas.ToPrintRows = null;
            // 
            // txtNewStandard
            // 
            this.txtNewStandard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtNewStandard.Location = new System.Drawing.Point(401, 46);
            this.txtNewStandard.Name = "txtNewStandard";
            this.txtNewStandard.Size = new System.Drawing.Size(139, 21);
            this.txtNewStandard.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtNewStandard.TabIndex = 26;
            // 
            // lblSampleName
            // 
            this.lblSampleName.AutoSize = true;
            this.lblSampleName.BackColor = System.Drawing.Color.Transparent;
            this.lblSampleName.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSampleName.Location = new System.Drawing.Point(385, 29);
            this.lblSampleName.Name = "lblSampleName";
            this.lblSampleName.Size = new System.Drawing.Size(57, 12);
            this.lblSampleName.TabIndex = 27;
            this.lblSampleName.Text = "校正样名";
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(418, 296);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(113, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 29;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(418, 264);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(113, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 28;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // UCSampleCalSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblSampleName);
            this.Controls.Add(this.txtNewStandard);
            this.Controls.Add(this.dgvwStandardDatas);
            this.Controls.Add(this.btnSelectElement);
            this.Name = "UCSampleCalSet";
            this.Size = new System.Drawing.Size(565, 345);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwStandardDatas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btnSelectElement;
        private Skyray.Controls.DataGridViewW dgvwStandardDatas;
        private Skyray.Controls.TextBoxW txtNewStandard;
        private Skyray.Controls.LabelW lblSampleName;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnOK;
    }
}
