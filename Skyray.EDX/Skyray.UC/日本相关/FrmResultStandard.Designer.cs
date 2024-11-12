namespace Skyray.UC
{
    partial class FrmResultStandard
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
            this.dgvStandard = new Skyray.Controls.DataGridViewW();
            this.CurveNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CaptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MinColmn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayTextColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.buttonWSubmit = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStandard)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvStandard
            // 
            this.dgvStandard.AllowUserToAddRows = false;
            this.dgvStandard.AllowUserToResizeRows = false;
            this.dgvStandard.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvStandard.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStandard.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvStandard.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvStandard.ColumnHeadersHeight = 20;
            this.dgvStandard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStandard.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CurveNameColumn,
            this.CaptionColumn,
            this.MinColmn,
            this.MaxColumn,
            this.DisplayTextColumn});
            this.dgvStandard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStandard.Location = new System.Drawing.Point(0, 0);
            this.dgvStandard.MultiSelect = false;
            this.dgvStandard.Name = "dgvStandard";
            this.dgvStandard.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvStandard.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvStandard.RowHeadersVisible = false;
            this.dgvStandard.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvStandard.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvStandard.RowTemplate.Height = 23;
            this.dgvStandard.SecondaryLength = 1;
            this.dgvStandard.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvStandard.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvStandard.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvStandard.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvStandard.ShowEportContextMenu = false;
            this.dgvStandard.Size = new System.Drawing.Size(568, 457);
            this.dgvStandard.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvStandard.TabIndex = 0;
            this.dgvStandard.ToPrintCols = null;
            this.dgvStandard.ToPrintRows = null;
            // 
            // CurveNameColumn
            // 
            this.CurveNameColumn.DataPropertyName = "CurveName";
            this.CurveNameColumn.HeaderText = Skyray.EDX.Common.Info.CurveName;
            this.CurveNameColumn.Name = "CurveNameColumn";
            this.CurveNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CurveNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CaptionColumn
            // 
            this.CaptionColumn.DataPropertyName = "Caption";
            this.CaptionColumn.HeaderText = Skyray.EDX.Common.Info.ElementName;
            this.CaptionColumn.Name = "CaptionColumn";
            this.CaptionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CaptionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MinColmn
            // 
            this.MinColmn.DataPropertyName = "Min";
            this.MinColmn.HeaderText = Skyray.EDX.Common.Info.MinValue + "(%)";
            this.MinColmn.Name = "MinColmn";
            // 
            // MaxColumn
            // 
            this.MaxColumn.DataPropertyName = "Max";
            this.MaxColumn.HeaderText = Skyray.EDX.Common.Info.MaxValue + "(%)";
            this.MaxColumn.Name = "MaxColumn";
            // 
            // DisplayTextColumn
            // 
            this.DisplayTextColumn.DataPropertyName = "DisplayText";
            this.DisplayTextColumn.HeaderText = Skyray.EDX.Common.Info.Display;
            this.DisplayTextColumn.Name = "DisplayTextColumn";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(8, 8);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvStandard);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnApplication);
            this.splitContainer1.Panel2.Controls.Add(this.buttonWSubmit);
            this.splitContainer1.Panel2.Controls.Add(this.btnDel);
            this.splitContainer1.Panel2.Controls.Add(this.btnAdd);
            this.splitContainer1.Size = new System.Drawing.Size(568, 500);
            this.splitContainer1.SplitterDistance = 457;
            this.splitContainer1.TabIndex = 1;
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(257, 9);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(75, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 3;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // buttonWSubmit
            // 
            this.buttonWSubmit.bSilver = false;
            this.buttonWSubmit.Location = new System.Drawing.Point(380, 9);
            this.buttonWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.Name = "buttonWSubmit";
            this.buttonWSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWSubmit.TabIndex = 2;
            this.buttonWSubmit.Text = "确定";
            this.buttonWSubmit.ToFocused = false;
            this.buttonWSubmit.UseVisualStyleBackColor = true;
            this.buttonWSubmit.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(139, 9);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 1;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(29, 9);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.HeaderText = Skyray.EDX.Common.Info.CurveName;
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.HeaderText = Skyray.EDX.Common.Info.ElementName;
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = Skyray.EDX.Common.Info.MinValue;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = Skyray.EDX.Common.Info.MaxValue;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = Skyray.EDX.Common.Info.Display;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // FrmResultStandard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmResultStandard";
            this.Size = new System.Drawing.Size(584, 516);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStandard)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvStandard;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW buttonWSubmit;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurveNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CaptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinColmn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisplayTextColumn;
        private Skyray.Controls.ButtonW btnApplication;
    }
}