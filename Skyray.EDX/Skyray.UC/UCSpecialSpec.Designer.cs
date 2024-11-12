namespace Skyray.UC
{
    partial class UCSpecialSpec
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
            this.dgvwSpecalItems = new Skyray.Controls.DataGridViewW();
            this.SpeccialCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecialLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecialPeakLow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecialPeakHigh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.lblBaseLow = new Skyray.Controls.LabelW();
            this.txtBaseLow = new Skyray.Controls.TextBoxW();
            this.lblBaseHigh = new Skyray.Controls.LabelW();
            this.txtBaseHigh = new Skyray.Controls.TextBoxW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwSpecalItems)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvwSpecalItems
            // 
            this.dgvwSpecalItems.AllowUserToAddRows = false;
            this.dgvwSpecalItems.AllowUserToDeleteRows = false;
            this.dgvwSpecalItems.AllowUserToResizeColumns = false;
            this.dgvwSpecalItems.AllowUserToResizeRows = false;
            this.dgvwSpecalItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvwSpecalItems.BackgroundColor = System.Drawing.Color.White;
            this.dgvwSpecalItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwSpecalItems.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwSpecalItems.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwSpecalItems.ColumnHeadersHeight = 24;
            this.dgvwSpecalItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwSpecalItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SpeccialCaption,
            this.SpecialLimit,
            this.SpecialPeakLow,
            this.SpecialPeakHigh});
            this.dgvwSpecalItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwSpecalItems.Location = new System.Drawing.Point(0, 7);
            this.dgvwSpecalItems.MultiSelect = false;
            this.dgvwSpecalItems.Name = "dgvwSpecalItems";
            this.dgvwSpecalItems.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwSpecalItems.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwSpecalItems.RowHeadersVisible = false;
            this.dgvwSpecalItems.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwSpecalItems.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwSpecalItems.RowTemplate.Height = 23;
            this.dgvwSpecalItems.SecondaryLength = 1;
            this.dgvwSpecalItems.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwSpecalItems.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwSpecalItems.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwSpecalItems.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwSpecalItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwSpecalItems.ShowEportContextMenu = true;
            this.dgvwSpecalItems.Size = new System.Drawing.Size(504, 312);
            this.dgvwSpecalItems.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwSpecalItems.TabIndex = 26;
            this.dgvwSpecalItems.ToPrintCols = null;
            this.dgvwSpecalItems.ToPrintRows = null;
            // 
            // SpeccialCaption
            // 
            this.SpeccialCaption.DataPropertyName = "Caption";
            this.SpeccialCaption.HeaderText = "名称";
            this.SpeccialCaption.Name = "SpeccialCaption";
            this.SpeccialCaption.ReadOnly = true;
            // 
            // SpecialLimit
            // 
            this.SpecialLimit.DataPropertyName = "AreaLimit";
            this.SpecialLimit.HeaderText = "阈值";
            this.SpecialLimit.Name = "SpecialLimit";
            this.SpecialLimit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SpecialPeakLow
            // 
            this.SpecialPeakLow.DataPropertyName = "PeakLow";
            this.SpecialPeakLow.HeaderText = "左边界";
            this.SpecialPeakLow.Name = "SpecialPeakLow";
            this.SpecialPeakLow.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SpecialPeakHigh
            // 
            this.SpecialPeakHigh.DataPropertyName = "PeakHigh";
            this.SpecialPeakHigh.HeaderText = "右边界";
            this.SpecialPeakHigh.Name = "SpecialPeakHigh";
            this.SpecialPeakHigh.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(96, 379);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(76, 23);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 30;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(408, 379);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(326, 379);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 29;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(14, 379);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(76, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 27;
            this.btnAdd.Text = "增加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(217, 379);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(76, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 31;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // lblBaseLow
            // 
            this.lblBaseLow.AutoSize = true;
            this.lblBaseLow.BackColor = System.Drawing.Color.Transparent;
            this.lblBaseLow.Location = new System.Drawing.Point(4, 328);
            this.lblBaseLow.Name = "lblBaseLow";
            this.lblBaseLow.Size = new System.Drawing.Size(53, 12);
            this.lblBaseLow.TabIndex = 32;
            this.lblBaseLow.Text = "背景左界";
            // 
            // txtBaseLow
            // 
            this.txtBaseLow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtBaseLow.Location = new System.Drawing.Point(70, 325);
            this.txtBaseLow.Name = "txtBaseLow";
            this.txtBaseLow.Size = new System.Drawing.Size(114, 21);
            this.txtBaseLow.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtBaseLow.TabIndex = 33;
            // 
            // lblBaseHigh
            // 
            this.lblBaseHigh.AutoSize = true;
            this.lblBaseHigh.BackColor = System.Drawing.Color.Transparent;
            this.lblBaseHigh.Location = new System.Drawing.Point(4, 356);
            this.lblBaseHigh.Name = "lblBaseHigh";
            this.lblBaseHigh.Size = new System.Drawing.Size(53, 12);
            this.lblBaseHigh.TabIndex = 34;
            this.lblBaseHigh.Text = "背景右界";
            // 
            // txtBaseHigh
            // 
            this.txtBaseHigh.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtBaseHigh.Location = new System.Drawing.Point(70, 352);
            this.txtBaseHigh.Name = "txtBaseHigh";
            this.txtBaseHigh.Size = new System.Drawing.Size(114, 21);
            this.txtBaseHigh.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtBaseHigh.TabIndex = 35;
            // 
            // UCSpecialSpec
            // 
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.lblBaseHigh);
            this.Controls.Add(this.txtBaseHigh);
            this.Controls.Add(this.lblBaseLow);
            this.Controls.Add(this.txtBaseLow);
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvwSpecalItems);
            this.Name = "UCSpecialSpec";
            this.Size = new System.Drawing.Size(504, 413);
            this.Load += new System.EventHandler(this.UCSpecialSpec_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwSpecalItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvwSpecalItems;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW btnApplication;
        private Skyray.Controls.LabelW lblBaseLow;
        private Skyray.Controls.TextBoxW txtBaseLow;
        private Skyray.Controls.LabelW lblBaseHigh;
        private Skyray.Controls.TextBoxW txtBaseHigh;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpeccialCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecialLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecialPeakLow;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecialPeakHigh;
    }
}
