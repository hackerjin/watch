namespace Skyray.UC
{
    partial class MatchForm
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
            this.dgvwMatch = new Skyray.Controls.DataGridViewW();
            this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCurveName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubmitButton = new Skyray.Controls.ButtonW();
            this.CancelButton = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwMatch)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvwMatch
            // 
            this.dgvwMatch.AllowUserToAddRows = false;
            this.dgvwMatch.AllowUserToDeleteRows = false;
            this.dgvwMatch.AllowUserToResizeColumns = false;
            this.dgvwMatch.AllowUserToResizeRows = false;
            this.dgvwMatch.BackgroundColor = System.Drawing.Color.White;
            this.dgvwMatch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvwMatch.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwMatch.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwMatch.ColumnHeadersHeight = 20;
            this.dgvwMatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvwMatch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColID,
            this.ColCurveName,
            this.ColMatch});
            this.dgvwMatch.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvwMatch.Location = new System.Drawing.Point(10, 10);
            this.dgvwMatch.Name = "dgvwMatch";
            this.dgvwMatch.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwMatch.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwMatch.ReadOnly = true;
            this.dgvwMatch.RowHeadersVisible = false;
            this.dgvwMatch.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwMatch.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwMatch.RowTemplate.Height = 23;
            this.dgvwMatch.SecondaryLength = 1;
            this.dgvwMatch.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwMatch.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwMatch.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwMatch.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwMatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwMatch.ShowEportContextMenu = false;
            this.dgvwMatch.Size = new System.Drawing.Size(254, 244);
            this.dgvwMatch.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwMatch.TabIndex = 0;
            this.dgvwMatch.ToPrintCols = null;
            this.dgvwMatch.ToPrintRows = null;
            // 
            // ColID
            // 
            this.ColID.HeaderText = "ID";
            this.ColID.Name = "ColID";
            this.ColID.ReadOnly = true;
            this.ColID.Visible = false;
            // 
            // ColCurveName
            // 
            this.ColCurveName.HeaderText = "曲线名称";
            this.ColCurveName.Name = "ColCurveName";
            this.ColCurveName.ReadOnly = true;
            this.ColCurveName.Width = 150;
            // 
            // ColMatch
            // 
            this.ColMatch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColMatch.HeaderText = "匹配度";
            this.ColMatch.Name = "ColMatch";
            this.ColMatch.ReadOnly = true;
            this.ColMatch.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SubmitButton
            // 
            this.SubmitButton.bSilver = false;
            this.SubmitButton.Location = new System.Drawing.Point(281, 11);
            this.SubmitButton.MaxImageSize = new System.Drawing.Point(0, 0);
            this.SubmitButton.MenuPos = new System.Drawing.Point(0, 0);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(75, 23);
            this.SubmitButton.Style = Skyray.Controls.Style.Office2007Blue;
            this.SubmitButton.TabIndex = 1;
            this.SubmitButton.Text = "确定";
            this.SubmitButton.ToFocused = false;
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.bSilver = false;
            this.CancelButton.Location = new System.Drawing.Point(281, 48);
            this.CancelButton.MaxImageSize = new System.Drawing.Point(0, 0);
            this.CancelButton.MenuPos = new System.Drawing.Point(0, 0);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.Style = Skyray.Controls.Style.Office2007Blue;
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "取消";
            this.CancelButton.ToFocused = false;
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // MatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(376, 264);
            this.ControlBox = false;
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.dgvwMatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatchForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "匹配";
            ((System.ComponentModel.ISupportInitialize)(this.dgvwMatch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvwMatch;
        private Skyray.Controls.ButtonW SubmitButton;
        private new Skyray.Controls.ButtonW CancelButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCurveName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMatch;

    }
}