namespace Skyray.UC
{
    partial class UCRef
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dgwRefElement = new Skyray.Controls.DataGridViewW();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIsRef = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColRefConf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDistrubThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbwElementList = new Skyray.Controls.ListBoxW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnSave = new Skyray.Controls.ButtonW();
            this.btnApplication = new Skyray.Controls.ButtonW();
            //this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            //this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btWRefTo = new Skyray.Controls.ButtonW();
            this.btnUp = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dgwRefElement)).BeginInit();
            this.SuspendLayout();
            // 
            // dgwRefElement
            // 
            this.dgwRefElement.AllowUserToAddRows = false;
            this.dgwRefElement.AllowUserToResizeColumns = false;
            this.dgwRefElement.AllowUserToResizeRows = false;
            this.dgwRefElement.BackgroundColor = System.Drawing.Color.White;
            this.dgwRefElement.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgwRefElement.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgwRefElement.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgwRefElement.ColumnHeadersHeight = 24;
            this.dgwRefElement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgwRefElement.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColIsRef,
            this.ColRefConf,
            this.ColDistrubThreshold});
            this.dgwRefElement.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgwRefElement.Location = new System.Drawing.Point(132, 17);
            this.dgwRefElement.Name = "dgwRefElement";
            this.dgwRefElement.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgwRefElement.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgwRefElement.RowHeadersVisible = false;
            this.dgwRefElement.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgwRefElement.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgwRefElement.RowTemplate.Height = 23;
            this.dgwRefElement.SecondaryLength = 1;
            this.dgwRefElement.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgwRefElement.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgwRefElement.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgwRefElement.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgwRefElement.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwRefElement.ShowEportContextMenu = true;
            this.dgwRefElement.Size = new System.Drawing.Size(364, 242);
            this.dgwRefElement.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgwRefElement.TabIndex = 20;
            this.dgwRefElement.ToPrintCols = null;
            this.dgwRefElement.ToPrintRows = null;
            this.dgwRefElement.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgwRefElement_CellValidating);
            this.dgwRefElement.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgwRefElement_DataError);
            // 
            // ColName
            // 
            this.ColName.DataPropertyName = "Name";
            this.ColName.HeaderText = "元素名称";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            this.ColName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColName.Width = 120;
            // 
            // ColIsRef
            // 
            this.ColIsRef.DataPropertyName = "IsRef";
            this.ColIsRef.HeaderText = "是否影响";
            this.ColIsRef.Name = "ColIsRef";
            this.ColIsRef.Width = 120;
            // 
            // ColRefConf
            // 
            this.ColRefConf.DataPropertyName = "RefConf";
            this.ColRefConf.HeaderText = "影响系数";
            this.ColRefConf.Name = "ColRefConf";
            this.ColRefConf.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColRefConf.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColRefConf.Width = 120;
            // 
            // ColDistrubThreshold
            // 
            this.ColDistrubThreshold.DataPropertyName = "DistrubThreshold";
            this.ColDistrubThreshold.HeaderText = "干扰阈值";
            this.ColDistrubThreshold.Name = "ColDistrubThreshold";
            this.ColDistrubThreshold.Visible = false;
            this.ColDistrubThreshold.Width = 90;
            // 
            // lbwElementList
            // 
            this.lbwElementList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.lbwElementList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbwElementList.FormattingEnabled = true;
            this.lbwElementList.HorizontalScrollbar = true;
            this.lbwElementList.ItemHeight = 17;
            this.lbwElementList.Location = new System.Drawing.Point(12, 17);
            this.lbwElementList.Name = "lbwElementList";
            this.lbwElementList.Size = new System.Drawing.Size(112, 242);
            this.lbwElementList.Style = Skyray.Controls.Style.Office2007Blue;
            this.lbwElementList.TabIndex = 19;
            this.lbwElementList.SelectedIndexChanged += new System.EventHandler(this.lbwElementList_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(234, 275);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(66, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.bSilver = false;
            this.btnSave.Location = new System.Drawing.Point(138, 275);
            this.btnSave.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSave.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 23);
            this.btnSave.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "确定";
            this.btnSave.ToFocused = false;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(42, 275);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(66, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 21;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // dataGridViewTextBoxColumn1
            // 
            //this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            //this.dataGridViewTextBoxColumn1.HeaderText = "元素名称";
            //this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            //this.dataGridViewTextBoxColumn1.ReadOnly = true;
            //this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            //this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //this.dataGridViewTextBoxColumn1.Width = 120;
            //// 
            //// dataGridViewCheckBoxColumn1
            //// 
            //this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsRef";
            //this.dataGridViewCheckBoxColumn1.HeaderText = "是否影响";
            //this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            //this.dataGridViewCheckBoxColumn1.Width = 120;
            //// 
            //// dataGridViewTextBoxColumn2
            //// 
            //this.dataGridViewTextBoxColumn2.DataPropertyName = "RefConf";
            //this.dataGridViewTextBoxColumn2.HeaderText = "影响系数";
            //this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            //this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            //this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //this.dataGridViewTextBoxColumn2.Width = 120;
            //// 
            //// dataGridViewTextBoxColumn3
            //// 
            //this.dataGridViewTextBoxColumn3.DataPropertyName = "DistrubThreshold";
            //this.dataGridViewTextBoxColumn3.HeaderText = "干扰阈值";
            //this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            //this.dataGridViewTextBoxColumn3.Visible = false;
            //this.dataGridViewTextBoxColumn3.Width = 90;
            // 
            // btWRefTo
            // 
            this.btWRefTo.bSilver = false;
            this.btWRefTo.Location = new System.Drawing.Point(426, 275);
            this.btWRefTo.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWRefTo.MenuPos = new System.Drawing.Point(0, 0);
            this.btWRefTo.Name = "btWRefTo";
            this.btWRefTo.Size = new System.Drawing.Size(66, 23);
            this.btWRefTo.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWRefTo.TabIndex = 22;
            this.btWRefTo.Text = "下一步";
            this.btWRefTo.ToFocused = false;
            this.btWRefTo.UseVisualStyleBackColor = true;
            this.btWRefTo.Click += new System.EventHandler(this.btWRefTo_Click);
            // 
            // btnUp
            // 
            this.btnUp.bSilver = false;
            this.btnUp.Location = new System.Drawing.Point(330, 275);
            this.btnUp.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnUp.MenuPos = new System.Drawing.Point(0, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(66, 23);
            this.btnUp.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnUp.TabIndex = 23;
            this.btnUp.Text = "上一步";
            this.btnUp.ToFocused = false;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // UCRef
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btWRefTo);
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.dgwRefElement);
            this.Controls.Add(this.lbwElementList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Name = "UCRef";
            this.Size = new System.Drawing.Size(508, 318);
            this.Load += new System.EventHandler(this.UCRef_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgwRefElement)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnSave;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ListBoxW lbwElementList;
        private Skyray.Controls.DataGridViewW dgwRefElement;
        private Skyray.Controls.ButtonW btnApplication;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRefConf;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDistrubThreshold;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private Skyray.Controls.ButtonW btWRefTo;
        private Skyray.Controls.ButtonW btnUp;

    }
}
