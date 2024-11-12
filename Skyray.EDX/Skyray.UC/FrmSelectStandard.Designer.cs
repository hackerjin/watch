namespace Skyray.UC
{
    partial class FrmSelectStandard
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
            this.dataGridViewW1 = new Skyray.Controls.DataGridViewW();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnConfirm = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.standardNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detailsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewW1
            // 
            this.dataGridViewW1.AllowUserToAddRows = false;
            this.dataGridViewW1.AllowUserToResizeRows = false;
            this.dataGridViewW1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dataGridViewW1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewW1.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dataGridViewW1.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dataGridViewW1.ColumnHeadersHeight = 20;
            this.dataGridViewW1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewW1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.standardNameColumn,
            this.detailsColumn});
            this.dataGridViewW1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewW1.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewW1.Name = "dataGridViewW1";
            this.dataGridViewW1.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dataGridViewW1.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dataGridViewW1.ReadOnly = true;
            this.dataGridViewW1.RowHeadersVisible = false;
            this.dataGridViewW1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dataGridViewW1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewW1.RowTemplate.Height = 23;
            this.dataGridViewW1.SecondaryLength = 1;
            this.dataGridViewW1.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dataGridViewW1.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dataGridViewW1.SelectedRowColor1 = System.Drawing.Color.White;
            this.dataGridViewW1.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dataGridViewW1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewW1.ShowEportContextMenu = false;
            this.dataGridViewW1.Size = new System.Drawing.Size(490, 212);
            this.dataGridViewW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.dataGridViewW1.TabIndex = 0;
            this.dataGridViewW1.ToPrintCols = null;
            this.dataGridViewW1.ToPrintRows = null;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridViewW1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(490, 212);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnConfirm);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 212);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(490, 41);
            this.panel2.TabIndex = 2;
            // 
            // btnConfirm
            // 
            this.btnConfirm.bSilver = false;
            this.btnConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfirm.Location = new System.Drawing.Point(115, 10);
            this.btnConfirm.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnConfirm.MenuPos = new System.Drawing.Point(0, 0);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.ToFocused = false;
            this.btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(258, 10);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // standardNameColumn
            // 
            this.standardNameColumn.HeaderText = "标准库名称";
            this.standardNameColumn.Name = "standardNameColumn";
            this.standardNameColumn.ReadOnly = true;
            // 
            // detailsColumn
            // 
            this.detailsColumn.HeaderText = "详细信息";
            this.detailsColumn.Name = "detailsColumn";
            this.detailsColumn.ReadOnly = true;
            this.detailsColumn.Width = 380;
            // 
            // FrmSelectStandard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 253);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectStandard";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "请选择当前测试的标准样品！";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dataGridViewW1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnConfirm;
        private System.Windows.Forms.DataGridViewTextBoxColumn standardNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn detailsColumn;
    }
}