namespace Skyray.UC
{
    partial class UCCaculateMatching
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
            this.dgvSpecMatching = new Skyray.Controls.DataGridViewW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.btnDelete = new Skyray.Controls.ButtonW();
            this.btnOpenSpec = new Skyray.Controls.ButtonW();
            this.btnCaculateMatching = new Skyray.Controls.ButtonW();
            this.btnCacel = new Skyray.Controls.ButtonW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompareSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MatchLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpecMatching)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSpecMatching
            // 
            this.dgvSpecMatching.AllowUserToAddRows = false;
            this.dgvSpecMatching.AllowUserToResizeRows = false;
            this.dgvSpecMatching.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvSpecMatching.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSpecMatching.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvSpecMatching.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvSpecMatching.ColumnHeadersHeight = 20;
            this.dgvSpecMatching.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSpecMatching.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OriginalSpec,
            this.CompareSpec,
            this.MatchLevel});
            this.dgvSpecMatching.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvSpecMatching.Location = new System.Drawing.Point(8, 8);
            this.dgvSpecMatching.Name = "dgvSpecMatching";
            this.dgvSpecMatching.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvSpecMatching.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvSpecMatching.RowHeadersVisible = false;
            this.dgvSpecMatching.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvSpecMatching.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvSpecMatching.RowTemplate.Height = 23;
            this.dgvSpecMatching.SecondaryLength = 1;
            this.dgvSpecMatching.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvSpecMatching.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvSpecMatching.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvSpecMatching.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvSpecMatching.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvSpecMatching.ShowEportContextMenu = false;
            this.dgvSpecMatching.Size = new System.Drawing.Size(470, 229);
            this.dgvSpecMatching.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvSpecMatching.TabIndex = 0;
            this.dgvSpecMatching.ToPrintCols = null;
            this.dgvSpecMatching.ToPrintRows = null;
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(488, 133);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 25);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.bSilver = false;
            this.btnDelete.Location = new System.Drawing.Point(488, 170);
            this.btnDelete.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelete.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 25);
            this.btnDelete.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "移除";
            this.btnDelete.ToFocused = false;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnOpenSpec
            // 
            this.btnOpenSpec.bSilver = false;
            this.btnOpenSpec.Location = new System.Drawing.Point(488, 59);
            this.btnOpenSpec.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOpenSpec.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOpenSpec.Name = "btnOpenSpec";
            this.btnOpenSpec.Size = new System.Drawing.Size(75, 25);
            this.btnOpenSpec.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOpenSpec.TabIndex = 5;
            this.btnOpenSpec.Text = "打开谱";
            this.btnOpenSpec.ToFocused = false;
            this.btnOpenSpec.UseVisualStyleBackColor = true;
            this.btnOpenSpec.Click += new System.EventHandler(this.btnOpenSpec_Click);
            // 
            // btnCaculateMatching
            // 
            this.btnCaculateMatching.bSilver = false;
            this.btnCaculateMatching.Location = new System.Drawing.Point(488, 96);
            this.btnCaculateMatching.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCaculateMatching.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCaculateMatching.Name = "btnCaculateMatching";
            this.btnCaculateMatching.Size = new System.Drawing.Size(75, 25);
            this.btnCaculateMatching.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCaculateMatching.TabIndex = 6;
            this.btnCaculateMatching.Text = "计算";
            this.btnCaculateMatching.ToFocused = false;
            this.btnCaculateMatching.UseVisualStyleBackColor = true;
            this.btnCaculateMatching.Click += new System.EventHandler(this.btnCaculateMatching_Click);
            // 
            // btnCacel
            // 
            this.btnCacel.bSilver = false;
            this.btnCacel.Location = new System.Drawing.Point(488, 207);
            this.btnCacel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCacel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCacel.Name = "btnCacel";
            this.btnCacel.Size = new System.Drawing.Size(75, 25);
            this.btnCacel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCacel.TabIndex = 2;
            this.btnCacel.Text = "取消";
            this.btnCacel.ToFocused = false;
            this.btnCacel.UseVisualStyleBackColor = true;
            this.btnCacel.Click += new System.EventHandler(this.btnCacel_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "当前谱";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 190;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "对比谱";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 190;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "匹配度";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // OriginalSpec
            // 
            this.OriginalSpec.HeaderText = "当前谱";
            this.OriginalSpec.Name = "OriginalSpec";
            this.OriginalSpec.Width = 190;
            // 
            // CompareSpec
            // 
            this.CompareSpec.HeaderText = "对比谱";
            this.CompareSpec.Name = "CompareSpec";
            this.CompareSpec.Width = 190;
            // 
            // MatchLevel
            // 
            this.MatchLevel.HeaderText = "匹配度";
            this.MatchLevel.Name = "MatchLevel";
            this.MatchLevel.Width = 80;
            // 
            // UCCaculateMatching
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCaculateMatching);
            this.Controls.Add(this.btnOpenSpec);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnCacel);
            this.Controls.Add(this.dgvSpecMatching);
            this.Name = "UCCaculateMatching";
            this.Size = new System.Drawing.Size(577, 245);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpecMatching)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvSpecMatching;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW btnDelete;
        private Skyray.Controls.ButtonW btnOpenSpec;
        private Skyray.Controls.ButtonW btnCaculateMatching;
        private Skyray.Controls.ButtonW btnCacel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompareSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatchLevel;
    }
}
