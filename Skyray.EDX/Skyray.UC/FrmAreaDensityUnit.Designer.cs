namespace Skyray.UC
{
    partial class FrmAreaDensityUnit
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
            this.dgvUnits = new Skyray.Controls.DataGridViewW();
            this.btnExit = new Skyray.Controls.ButtonW();
            this.btnDelete = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.txtUnitName = new Skyray.Controls.TextBoxW();
            this.lbl = new Skyray.Controls.LabelW();
            this.txtCoefK = new Skyray.Controls.TextBoxW();
            this.lblbaseUnit = new Skyray.Controls.LabelW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnits)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvUnits
            // 
            this.dgvUnits.AllowUserToAddRows = false;
            this.dgvUnits.AllowUserToDeleteRows = false;
            this.dgvUnits.AllowUserToResizeRows = false;
            this.dgvUnits.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUnits.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvUnits.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUnits.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvUnits.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvUnits.ColumnHeadersHeight = 4;
            this.dgvUnits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnits.Location = new System.Drawing.Point(1, 2);
            this.dgvUnits.MultiSelect = false;
            this.dgvUnits.Name = "dgvUnits";
            this.dgvUnits.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvUnits.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvUnits.ReadOnly = true;
            this.dgvUnits.RowHeadersVisible = false;
            this.dgvUnits.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvUnits.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvUnits.RowTemplate.Height = 23;
            this.dgvUnits.SecondaryLength = 1;
            this.dgvUnits.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvUnits.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvUnits.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvUnits.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvUnits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUnits.ShowEportContextMenu = false;
            this.dgvUnits.Size = new System.Drawing.Size(350, 184);
            this.dgvUnits.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvUnits.TabIndex = 0;
            this.dgvUnits.ToPrintCols = null;
            this.dgvUnits.ToPrintRows = null;
            // 
            // btnExit
            // 
            this.btnExit.bSilver = false;
            this.btnExit.Location = new System.Drawing.Point(232, 246);
            this.btnExit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnExit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "退出";
            this.btnExit.ToFocused = false;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.bSilver = false;
            this.btnDelete.Location = new System.Drawing.Point(131, 246);
            this.btnDelete.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelete.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 25);
            this.btnDelete.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.ToFocused = false;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(24, 246);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 25);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtUnitName
            // 
            this.txtUnitName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtUnitName.Location = new System.Drawing.Point(232, 208);
            this.txtUnitName.Name = "txtUnitName";
            this.txtUnitName.Size = new System.Drawing.Size(105, 20);
            this.txtUnitName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtUnitName.TabIndex = 6;
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.BackColor = System.Drawing.Color.Transparent;
            this.lbl.Location = new System.Drawing.Point(212, 211);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(11, 13);
            this.lbl.TabIndex = 7;
            this.lbl.Text = "*";
            // 
            // txtCoefK
            // 
            this.txtCoefK.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtCoefK.Location = new System.Drawing.Point(89, 208);
            this.txtCoefK.Name = "txtCoefK";
            this.txtCoefK.Size = new System.Drawing.Size(117, 20);
            this.txtCoefK.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtCoefK.TabIndex = 8;
            // 
            // lblbaseUnit
            // 
            this.lblbaseUnit.AutoSize = true;
            this.lblbaseUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblbaseUnit.Location = new System.Drawing.Point(12, 211);
            this.lblbaseUnit.Name = "lblbaseUnit";
            this.lblbaseUnit.Size = new System.Drawing.Size(53, 13);
            this.lblbaseUnit.TabIndex = 9;
            this.lblbaseUnit.Text = "(g/m^2) =";
            // 
            // FrmAreaDensityUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 284);
            this.Controls.Add(this.lblbaseUnit);
            this.Controls.Add(this.txtCoefK);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.txtUnitName);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.dgvUnits);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAreaDensityUnit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "自定义面密度单位";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvUnits;
        private Skyray.Controls.ButtonW btnExit;
        private Skyray.Controls.ButtonW btnDelete;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.TextBoxW txtUnitName;
        private Skyray.Controls.LabelW lbl;
        private Skyray.Controls.TextBoxW txtCoefK;
        private Skyray.Controls.LabelW lblbaseUnit;
    }
}