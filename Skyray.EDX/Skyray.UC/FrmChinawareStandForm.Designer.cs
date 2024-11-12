namespace Skyray.UC
{
    partial class FrmChinawareStandForm
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
            this.dataGridViewW = new Skyray.Controls.DataGridViewW();
            this.StandardColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.confidenceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExit = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewW
            // 
            this.dataGridViewW.AllowUserToAddRows = false;
            this.dataGridViewW.AllowUserToDeleteRows = false;
            this.dataGridViewW.AllowUserToResizeRows = false;
            this.dataGridViewW.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dataGridViewW.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewW.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dataGridViewW.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dataGridViewW.ColumnHeadersHeight = 20;
            this.dataGridViewW.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewW.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StandardColumn,
            this.confidenceColumn});
            this.dataGridViewW.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewW.MultiSelect = false;
            this.dataGridViewW.Name = "dataGridViewW";
            this.dataGridViewW.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dataGridViewW.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dataGridViewW.ReadOnly = true;
            this.dataGridViewW.RowHeadersVisible = false;
            this.dataGridViewW.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dataGridViewW.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewW.RowTemplate.Height = 23;
            this.dataGridViewW.SecondaryLength = 1;
            this.dataGridViewW.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dataGridViewW.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dataGridViewW.SelectedRowColor1 = System.Drawing.Color.White;
            this.dataGridViewW.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dataGridViewW.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewW.ShowEportContextMenu = false;
            this.dataGridViewW.Size = new System.Drawing.Size(309, 438);
            this.dataGridViewW.Style = Skyray.Controls.Style.Office2007Blue;
            this.dataGridViewW.TabIndex = 0;
            this.dataGridViewW.ToPrintCols = null;
            this.dataGridViewW.ToPrintRows = null;
            this.dataGridViewW.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewW_CellDoubleClick);
            // 
            // StandardColumn
            // 
            this.StandardColumn.HeaderText = "陶瓷类型";
            this.StandardColumn.Name = "StandardColumn";
            this.StandardColumn.ReadOnly = true;
            this.StandardColumn.Width = 200;
            // 
            // confidenceColumn
            // 
            this.confidenceColumn.HeaderText = "可信度(%)";
            this.confidenceColumn.Name = "confidenceColumn";
            this.confidenceColumn.ReadOnly = true;
            // 
            // btnExit
            // 
            this.btnExit.bSilver = false;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(327, 426);
            this.btnExit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnExit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "退出";
            this.btnExit.ToFocused = false;
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // FrmChinawareStandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 460);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.dataGridViewW);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChinawareStandForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "分析结果";
            this.Shown += new System.EventHandler(this.FrmChinawareStandForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dataGridViewW;
        private Skyray.Controls.ButtonW btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn StandardColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn confidenceColumn;

    }
}