namespace Skyray.Camera
{
    partial class FrmEditMultiPoints
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEditMultiPoints));
            this.cboMultiNames = new System.Windows.Forms.ComboBox();
            this.lblmultiName = new System.Windows.Forms.Label();
            this.btnAccept = new System.Windows.Forms.Button();
            this.dgvMultiDatas = new System.Windows.Forms.DataGridView();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnDeletePoint = new System.Windows.Forms.Button();
            this.btnAddPoint = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultiDatas)).BeginInit();
            this.SuspendLayout();
            // 
            // cboMultiNames
            // 
            this.cboMultiNames.FormattingEnabled = true;
            this.cboMultiNames.Location = new System.Drawing.Point(119, 33);
            this.cboMultiNames.Name = "cboMultiNames";
            this.cboMultiNames.Size = new System.Drawing.Size(175, 20);
            this.cboMultiNames.TabIndex = 0;
            this.cboMultiNames.SelectedIndexChanged += new System.EventHandler(this.cboMultiNames_SelectedIndexChanged);
            // 
            // lblmultiName
            // 
            this.lblmultiName.AutoSize = true;
            this.lblmultiName.Location = new System.Drawing.Point(26, 33);
            this.lblmultiName.Name = "lblmultiName";
            this.lblmultiName.Size = new System.Drawing.Size(71, 12);
            this.lblmultiName.TabIndex = 1;
            this.lblmultiName.Text = "Multi Name:";
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(415, 210);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 21);
            this.btnAccept.TabIndex = 2;
            this.btnAccept.Text = "Confirm";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dgvMultiDatas
            // 
            this.dgvMultiDatas.AllowUserToDeleteRows = false;
            this.dgvMultiDatas.AllowUserToResizeColumns = false;
            this.dgvMultiDatas.AllowUserToResizeRows = false;
            this.dgvMultiDatas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMultiDatas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMultiDatas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMultiDatas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumber,
            this.colX,
            this.colY,
            this.colZ});
            this.dgvMultiDatas.Location = new System.Drawing.Point(28, 68);
            this.dgvMultiDatas.MultiSelect = false;
            this.dgvMultiDatas.Name = "dgvMultiDatas";
            this.dgvMultiDatas.RowHeadersVisible = false;
            this.dgvMultiDatas.RowTemplate.Height = 23;
            this.dgvMultiDatas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMultiDatas.Size = new System.Drawing.Size(381, 261);
            this.dgvMultiDatas.TabIndex = 15;
            this.dgvMultiDatas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMultiDatas_CellClick);
            this.dgvMultiDatas.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvMultiDatas_EditingControlShowing);
            // 
            // colNumber
            // 
            this.colNumber.HeaderText = "Number";
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            // 
            // colX
            // 
            this.colX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colX.HeaderText = "X";
            this.colX.Name = "colX";
            // 
            // colY
            // 
            this.colY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colY.HeaderText = "Y";
            this.colY.Name = "colY";
            // 
            // colZ
            // 
            this.colZ.HeaderText = "Z";
            this.colZ.Name = "colZ";
            this.colZ.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(415, 170);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 21);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(415, 33);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 18;
            this.btnDel.Text = "Delete";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnDeletePoint
            // 
            this.btnDeletePoint.Location = new System.Drawing.Point(415, 91);
            this.btnDeletePoint.Name = "btnDeletePoint";
            this.btnDeletePoint.Size = new System.Drawing.Size(75, 21);
            this.btnDeletePoint.TabIndex = 19;
            this.btnDeletePoint.Text = "Delete";
            this.btnDeletePoint.UseVisualStyleBackColor = true;
            this.btnDeletePoint.Click += new System.EventHandler(this.btnDeletePoint_Click);
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.Location = new System.Drawing.Point(415, 130);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(75, 21);
            this.btnAddPoint.TabIndex = 20;
            this.btnAddPoint.Text = "Ìí¼Óµã";
            this.btnAddPoint.UseVisualStyleBackColor = true;
            this.btnAddPoint.Click += new System.EventHandler(this.btnAddPoint_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Number";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 68;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "X";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "Y";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Z";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // FrmEditMultiPoints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(544, 370);
            this.Controls.Add(this.btnAddPoint);
            this.Controls.Add(this.btnDeletePoint);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvMultiDatas);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.lblmultiName);
            this.Controls.Add(this.cboMultiNames);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEditMultiPoints";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit MultiPoints";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmEditMultiPoints_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEditMultiPoints_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultiDatas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboMultiNames;
        private System.Windows.Forms.Label lblmultiName;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnDeletePoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Button btnAddPoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        public System.Windows.Forms.DataGridView dgvMultiDatas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colZ;

    }
}