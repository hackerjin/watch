namespace Skyray.UC
{
    partial class UCPureElementsTable
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
            this.dataGridViewW1 = new Skyray.Controls.DataGridViewW();
            this.buttonWAdd = new Skyray.Controls.ButtonW();
            this.buttonWRemove = new Skyray.Controls.ButtonW();
            this.buttonWSubmit = new Skyray.Controls.ButtonW();
            this.buttonWCancel = new Skyray.Controls.ButtonW();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ElementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ka = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Kb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.La = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewW1
            // 
            this.dataGridViewW1.AllowUserToAddRows = false;
            this.dataGridViewW1.AllowUserToDeleteRows = false;
            this.dataGridViewW1.AllowUserToResizeColumns = false;
            this.dataGridViewW1.AllowUserToResizeRows = false;
            this.dataGridViewW1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewW1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewW1.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dataGridViewW1.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dataGridViewW1.ColumnHeadersHeight = 24;
            this.dataGridViewW1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewW1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ElementName,
            this.Ka,
            this.Kb,
            this.La,
            this.Lb});
            this.dataGridViewW1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewW1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewW1.Location = new System.Drawing.Point(8, 8);
            this.dataGridViewW1.Name = "dataGridViewW1";
            this.dataGridViewW1.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dataGridViewW1.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
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
            this.dataGridViewW1.ShowEportContextMenu = true;
            this.dataGridViewW1.Size = new System.Drawing.Size(503, 554);
            this.dataGridViewW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.dataGridViewW1.TabIndex = 1;
            this.dataGridViewW1.ToPrintCols = null;
            this.dataGridViewW1.ToPrintRows = null;
            this.dataGridViewW1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewW1_CellValidating);
            this.dataGridViewW1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewW1_CellEndEdit);
            // 
            // buttonWAdd
            // 
            this.buttonWAdd.bSilver = false;
            this.buttonWAdd.Location = new System.Drawing.Point(517, 9);
            this.buttonWAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWAdd.Name = "buttonWAdd";
            this.buttonWAdd.Size = new System.Drawing.Size(94, 23);
            this.buttonWAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWAdd.TabIndex = 0;
            this.buttonWAdd.Text = "添加";
            this.buttonWAdd.ToFocused = false;
            this.buttonWAdd.UseVisualStyleBackColor = true;
            this.buttonWAdd.Click += new System.EventHandler(this.buttonWAdd_Click);
            // 
            // buttonWRemove
            // 
            this.buttonWRemove.bSilver = false;
            this.buttonWRemove.Location = new System.Drawing.Point(517, 52);
            this.buttonWRemove.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWRemove.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWRemove.Name = "buttonWRemove";
            this.buttonWRemove.Size = new System.Drawing.Size(94, 23);
            this.buttonWRemove.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWRemove.TabIndex = 1;
            this.buttonWRemove.Text = "移除";
            this.buttonWRemove.ToFocused = false;
            this.buttonWRemove.UseVisualStyleBackColor = true;
            this.buttonWRemove.Click += new System.EventHandler(this.buttonWRemove_Click);
            // 
            // buttonWSubmit
            // 
            this.buttonWSubmit.bSilver = false;
            this.buttonWSubmit.Location = new System.Drawing.Point(519, 497);
            this.buttonWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.Name = "buttonWSubmit";
            this.buttonWSubmit.Size = new System.Drawing.Size(94, 23);
            this.buttonWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWSubmit.TabIndex = 2;
            this.buttonWSubmit.Text = "确定";
            this.buttonWSubmit.ToFocused = false;
            this.buttonWSubmit.UseVisualStyleBackColor = true;
            this.buttonWSubmit.Click += new System.EventHandler(this.buttonWSubmit_Click);
            // 
            // buttonWCancel
            // 
            this.buttonWCancel.bSilver = false;
            this.buttonWCancel.Location = new System.Drawing.Point(519, 537);
            this.buttonWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWCancel.Name = "buttonWCancel";
            this.buttonWCancel.Size = new System.Drawing.Size(94, 23);
            this.buttonWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWCancel.TabIndex = 3;
            this.buttonWCancel.Text = "取消";
            this.buttonWCancel.ToFocused = false;
            this.buttonWCancel.UseVisualStyleBackColor = true;
            this.buttonWCancel.Click += new System.EventHandler(this.buttonWCancel_Click);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // ElementName
            // 
            this.ElementName.HeaderText = "名称";
            this.ElementName.Name = "ElementName";
            this.ElementName.ReadOnly = true;
            // 
            // Ka
            // 
            this.Ka.HeaderText = "Ka";
            this.Ka.Name = "Ka";
            // 
            // Kb
            // 
            this.Kb.HeaderText = "Kb";
            this.Kb.Name = "Kb";
            // 
            // La
            // 
            this.La.HeaderText = "La";
            this.La.Name = "La";
            // 
            // Lb
            // 
            this.Lb.HeaderText = "Lb";
            this.Lb.Name = "Lb";
            // 
            // UCPureElementsTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.buttonWRemove);
            this.Controls.Add(this.buttonWCancel);
            this.Controls.Add(this.buttonWAdd);
            this.Controls.Add(this.buttonWSubmit);
            this.Controls.Add(this.dataGridViewW1);
            this.Name = "UCPureElementsTable";
            this.Size = new System.Drawing.Size(621, 570);
            this.Load += new System.EventHandler(this.UCPureElementsTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW buttonWRemove;
        private Skyray.Controls.ButtonW buttonWAdd;
        private Skyray.Controls.DataGridViewW dataGridViewW1;
        private Skyray.Controls.ButtonW buttonWSubmit;
        private Skyray.Controls.ButtonW buttonWCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ElementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ka;
        private System.Windows.Forms.DataGridViewTextBoxColumn Kb;
        private System.Windows.Forms.DataGridViewTextBoxColumn La;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lb;

    }
}
