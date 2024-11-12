namespace Skyray.UC
{
    partial class UCMultiSet
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
            this.groupboxPoint = new System.Windows.Forms.GroupBox();
            this.btnSave = new Skyray.Controls.ButtonW();
            this.btnReback = new Skyray.Controls.ButtonW();
            this.lstMultiNames = new Skyray.Controls.ListBoxW();
            this.btnDelPoint = new Skyray.Controls.ButtonW();
            this.btnAddPoint = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.dgvMultiDatas = new Skyray.Controls.DataGridViewW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupboxPoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultiDatas)).BeginInit();
            this.SuspendLayout();
            // 
            // groupboxPoint
            // 
            this.groupboxPoint.Controls.Add(this.btnSave);
            this.groupboxPoint.Controls.Add(this.btnReback);
            this.groupboxPoint.Controls.Add(this.lstMultiNames);
            this.groupboxPoint.Controls.Add(this.btnDelPoint);
            this.groupboxPoint.Controls.Add(this.btnAddPoint);
            this.groupboxPoint.Controls.Add(this.btnDel);
            this.groupboxPoint.Controls.Add(this.btnAdd);
            this.groupboxPoint.Controls.Add(this.dgvMultiDatas);
            this.groupboxPoint.Location = new System.Drawing.Point(0, 11);
            this.groupboxPoint.Name = "groupboxPoint";
            this.groupboxPoint.Size = new System.Drawing.Size(513, 367);
            this.groupboxPoint.TabIndex = 44;
            this.groupboxPoint.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.bSilver = false;
            this.btnSave.Location = new System.Drawing.Point(397, 103);
            this.btnSave.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSave.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 23);
            this.btnSave.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSave.TabIndex = 55;
            this.btnSave.Text = "保存";
            this.btnSave.ToFocused = false;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReback
            // 
            this.btnReback.bSilver = false;
            this.btnReback.Location = new System.Drawing.Point(397, 148);
            this.btnReback.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnReback.MenuPos = new System.Drawing.Point(0, 0);
            this.btnReback.Name = "btnReback";
            this.btnReback.Size = new System.Drawing.Size(88, 23);
            this.btnReback.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnReback.TabIndex = 54;
            this.btnReback.Text = "回零位";
            this.btnReback.ToFocused = false;
            this.btnReback.UseVisualStyleBackColor = true;
            this.btnReback.Click += new System.EventHandler(this.btnReback_Click);
            // 
            // lstMultiNames
            // 
            this.lstMultiNames.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.lstMultiNames.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstMultiNames.FormattingEnabled = true;
            this.lstMultiNames.HorizontalScrollbar = true;
            this.lstMultiNames.ItemHeight = 17;
            this.lstMultiNames.Location = new System.Drawing.Point(27, 13);
            this.lstMultiNames.Name = "lstMultiNames";
            this.lstMultiNames.Size = new System.Drawing.Size(104, 276);
            this.lstMultiNames.Style = Skyray.Controls.Style.Office2007Blue;
            this.lstMultiNames.TabIndex = 53;
            this.lstMultiNames.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstMultiNames_MouseClick);
            this.lstMultiNames.SelectedIndexChanged += new System.EventHandler(this.lstMultiNames_SelectedIndexChanged);
            // 
            // btnDelPoint
            // 
            this.btnDelPoint.bSilver = false;
            this.btnDelPoint.Location = new System.Drawing.Point(397, 55);
            this.btnDelPoint.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelPoint.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelPoint.Name = "btnDelPoint";
            this.btnDelPoint.Size = new System.Drawing.Size(88, 23);
            this.btnDelPoint.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelPoint.TabIndex = 50;
            this.btnDelPoint.Text = "删除点";
            this.btnDelPoint.ToFocused = false;
            this.btnDelPoint.UseVisualStyleBackColor = true;
            this.btnDelPoint.Click += new System.EventHandler(this.btnDelPoint_Click);
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.bSilver = false;
            this.btnAddPoint.Location = new System.Drawing.Point(397, 13);
            this.btnAddPoint.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAddPoint.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(88, 23);
            this.btnAddPoint.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAddPoint.TabIndex = 49;
            this.btnAddPoint.Text = "添加点";
            this.btnAddPoint.ToFocused = false;
            this.btnAddPoint.UseVisualStyleBackColor = true;
            this.btnAddPoint.Click += new System.EventHandler(this.btnAddPoint_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(27, 331);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(104, 23);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 48;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(27, 298);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 47;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvMultiDatas
            // 
            this.dgvMultiDatas.AllowUserToAddRows = false;
            this.dgvMultiDatas.AllowUserToResizeRows = false;
            this.dgvMultiDatas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMultiDatas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvMultiDatas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMultiDatas.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvMultiDatas.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvMultiDatas.ColumnHeadersHeight = 21;
            this.dgvMultiDatas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMultiDatas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNumber,
            this.colX,
            this.colY});
            this.dgvMultiDatas.Location = new System.Drawing.Point(158, 13);
            this.dgvMultiDatas.MultiSelect = false;
            this.dgvMultiDatas.Name = "dgvMultiDatas";
            this.dgvMultiDatas.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvMultiDatas.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvMultiDatas.RowHeadersVisible = false;
            this.dgvMultiDatas.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvMultiDatas.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvMultiDatas.RowTemplate.Height = 23;
            this.dgvMultiDatas.SecondaryLength = 1;
            this.dgvMultiDatas.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvMultiDatas.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvMultiDatas.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvMultiDatas.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvMultiDatas.ShowEportContextMenu = false;
            this.dgvMultiDatas.Size = new System.Drawing.Size(215, 339);
            this.dgvMultiDatas.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvMultiDatas.TabIndex = 44;
            this.dgvMultiDatas.ToPrintCols = null;
            this.dgvMultiDatas.ToPrintRows = null;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Number";
            this.dataGridViewTextBoxColumn1.HeaderText = "Number";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 71;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "X";
            this.dataGridViewTextBoxColumn2.HeaderText = "X";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 71;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Y";
            this.dataGridViewTextBoxColumn3.HeaderText = "Y";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 72;
            // 
            // ColumnNumber
            // 
            this.ColumnNumber.HeaderText = "Number";
            this.ColumnNumber.Name = "ColumnNumber";
            // 
            // colX
            // 
            this.colX.HeaderText = "X";
            this.colX.Name = "colX";
            // 
            // colY
            // 
            this.colY.HeaderText = "Y";
            this.colY.Name = "colY";
            // 
            // UCMultiSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.groupboxPoint);
            this.Name = "UCMultiSet";
            this.Size = new System.Drawing.Size(531, 389);
            this.groupboxPoint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultiDatas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupboxPoint;
        private Skyray.Controls.ButtonW btnDelPoint;
        private Skyray.Controls.ButtonW btnAddPoint;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.DataGridViewW dgvMultiDatas;
        private Skyray.Controls.ListBoxW lstMultiNames;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private Skyray.Controls.ButtonW btnReback;
        private Skyray.Controls.ButtonW btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colY;
    }
}
