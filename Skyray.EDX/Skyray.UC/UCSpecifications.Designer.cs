namespace Skyray.UC
{
    partial class UCSpecifications
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
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnSaveNew = new Skyray.Controls.ButtonW();
            this.txtCategoryName = new Skyray.Controls.TextBoxW();
            this.btnAddCategory = new Skyray.Controls.ButtonW();
            this.btnDelCategory = new Skyray.Controls.ButtonW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.btnSave = new Skyray.Controls.ButtonW();
            this.btnDeleteElement = new Skyray.Controls.ButtonW();
            this.btnAddElement = new Skyray.Controls.ButtonW();
            this.dgvElements = new Skyray.Controls.DataGridViewW();
            this.ColElementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExampleMaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExampleMinValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteExample = new Skyray.Controls.ButtonW();
            this.btnAddExample = new Skyray.Controls.ButtonW();
            this.dgvExample = new Skyray.Controls.DataGridViewW();
            this.ColExampleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColExampleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCreateExampleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cboCategory = new Skyray.Controls.ComboBoxW();
            this.lblCategory = new Skyray.Controls.LabelW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvElements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExample)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(484, 15);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 47;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancelSave_Click);
            // 
            // btnSaveNew
            // 
            this.btnSaveNew.bSilver = false;
            this.btnSaveNew.Location = new System.Drawing.Point(415, 15);
            this.btnSaveNew.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSaveNew.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSaveNew.Name = "btnSaveNew";
            this.btnSaveNew.Size = new System.Drawing.Size(60, 23);
            this.btnSaveNew.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSaveNew.TabIndex = 48;
            this.btnSaveNew.Text = "保存";
            this.btnSaveNew.ToFocused = false;
            this.btnSaveNew.UseVisualStyleBackColor = true;
            this.btnSaveNew.Visible = false;
            this.btnSaveNew.Click += new System.EventHandler(this.btnSaveNew_Click);
            // 
            // txtCategoryName
            // 
            this.txtCategoryName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtCategoryName.Location = new System.Drawing.Point(263, 16);
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.Size = new System.Drawing.Size(146, 21);
            this.txtCategoryName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtCategoryName.TabIndex = 58;
            this.txtCategoryName.Visible = false;
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.bSilver = false;
            this.btnAddCategory.Location = new System.Drawing.Point(344, 17);
            this.btnAddCategory.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAddCategory.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnAddCategory.Size = new System.Drawing.Size(60, 19);
            this.btnAddCategory.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAddCategory.TabIndex = 11;
            this.btnAddCategory.Text = "增加";
            this.btnAddCategory.ToFocused = false;
            this.btnAddCategory.UseVisualStyleBackColor = true;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // btnDelCategory
            // 
            this.btnDelCategory.bSilver = false;
            this.btnDelCategory.Enabled = false;
            this.btnDelCategory.Location = new System.Drawing.Point(282, 17);
            this.btnDelCategory.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelCategory.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelCategory.Name = "btnDelCategory";
            this.btnDelCategory.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnDelCategory.Size = new System.Drawing.Size(60, 19);
            this.btnDelCategory.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelCategory.TabIndex = 10;
            this.btnDelCategory.Text = "删除";
            this.btnDelCategory.ToFocused = false;
            this.btnDelCategory.UseVisualStyleBackColor = true;
            this.btnDelCategory.Click += new System.EventHandler(this.btnDelCategory_Click);
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(419, 397);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "关闭";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.bSilver = false;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(215, 397);
            this.btnSave.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSave.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "保存";
            this.btnSave.ToFocused = false;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDeleteElement
            // 
            this.btnDeleteElement.bSilver = false;
            this.btnDeleteElement.Enabled = false;
            this.btnDeleteElement.Location = new System.Drawing.Point(504, 58);
            this.btnDeleteElement.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDeleteElement.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDeleteElement.Name = "btnDeleteElement";
            this.btnDeleteElement.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteElement.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDeleteElement.TabIndex = 7;
            this.btnDeleteElement.Text = "删除";
            this.btnDeleteElement.ToFocused = false;
            this.btnDeleteElement.UseVisualStyleBackColor = true;
            this.btnDeleteElement.Click += new System.EventHandler(this.btnDeleteElement_Click);
            // 
            // btnAddElement
            // 
            this.btnAddElement.bSilver = false;
            this.btnAddElement.Enabled = false;
            this.btnAddElement.Location = new System.Drawing.Point(368, 58);
            this.btnAddElement.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAddElement.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAddElement.Name = "btnAddElement";
            this.btnAddElement.Size = new System.Drawing.Size(75, 23);
            this.btnAddElement.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAddElement.TabIndex = 6;
            this.btnAddElement.Text = "增加";
            this.btnAddElement.ToFocused = false;
            this.btnAddElement.UseVisualStyleBackColor = true;
            this.btnAddElement.Click += new System.EventHandler(this.btnAddElement_Click);
            // 
            // dgvElements
            // 
            this.dgvElements.AllowUserToAddRows = false;
            this.dgvElements.AllowUserToDeleteRows = false;
            this.dgvElements.AllowUserToResizeRows = false;
            this.dgvElements.BackgroundColor = System.Drawing.Color.White;
            this.dgvElements.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvElements.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvElements.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvElements.ColumnHeadersHeight = 20;
            this.dgvElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvElements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColElementName,
            this.ExampleMaxValue,
            this.ExampleMinValue});
            this.dgvElements.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvElements.Location = new System.Drawing.Point(368, 96);
            this.dgvElements.MultiSelect = false;
            this.dgvElements.Name = "dgvElements";
            this.dgvElements.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvElements.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvElements.RowHeadersVisible = false;
            this.dgvElements.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvElements.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvElements.RowTemplate.Height = 23;
            this.dgvElements.SecondaryLength = 1;
            this.dgvElements.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvElements.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvElements.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvElements.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvElements.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvElements.ShowEportContextMenu = false;
            this.dgvElements.Size = new System.Drawing.Size(321, 280);
            this.dgvElements.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvElements.TabIndex = 5;
            this.dgvElements.ToPrintCols = null;
            this.dgvElements.ToPrintRows = null;
            this.dgvElements.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvElements_CellValidating);
            // 
            // ColElementName
            // 
            this.ColElementName.DataPropertyName = "ElementName";
            this.ColElementName.HeaderText = "元素名称";
            this.ColElementName.Name = "ColElementName";
            this.ColElementName.ReadOnly = true;
            // 
            // ExampleMaxValue
            // 
            this.ExampleMaxValue.DataPropertyName = "MaxValue";
            this.ExampleMaxValue.HeaderText = "最大值";
            this.ExampleMaxValue.Name = "ExampleMaxValue";
            // 
            // ExampleMinValue
            // 
            this.ExampleMinValue.DataPropertyName = "MinValue";
            this.ExampleMinValue.HeaderText = "最小值";
            this.ExampleMinValue.Name = "ExampleMinValue";
            // 
            // btnDeleteExample
            // 
            this.btnDeleteExample.bSilver = false;
            this.btnDeleteExample.Enabled = false;
            this.btnDeleteExample.Location = new System.Drawing.Point(158, 58);
            this.btnDeleteExample.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDeleteExample.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDeleteExample.Name = "btnDeleteExample";
            this.btnDeleteExample.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteExample.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDeleteExample.TabIndex = 4;
            this.btnDeleteExample.Text = "删除";
            this.btnDeleteExample.ToFocused = false;
            this.btnDeleteExample.UseVisualStyleBackColor = true;
            this.btnDeleteExample.Click += new System.EventHandler(this.btnDeleteExample_Click);
            // 
            // btnAddExample
            // 
            this.btnAddExample.bSilver = false;
            this.btnAddExample.Enabled = false;
            this.btnAddExample.Location = new System.Drawing.Point(20, 58);
            this.btnAddExample.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAddExample.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAddExample.Name = "btnAddExample";
            this.btnAddExample.Size = new System.Drawing.Size(75, 23);
            this.btnAddExample.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAddExample.TabIndex = 3;
            this.btnAddExample.Text = "增加";
            this.btnAddExample.ToFocused = false;
            this.btnAddExample.UseVisualStyleBackColor = true;
            this.btnAddExample.Click += new System.EventHandler(this.btnAddExample_Click);
            // 
            // dgvExample
            // 
            this.dgvExample.AllowUserToAddRows = false;
            this.dgvExample.AllowUserToDeleteRows = false;
            this.dgvExample.AllowUserToResizeRows = false;
            this.dgvExample.BackgroundColor = System.Drawing.Color.White;
            this.dgvExample.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvExample.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvExample.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvExample.ColumnHeadersHeight = 20;
            this.dgvExample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExample.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColExampleName,
            this.ColExampleType,
            this.ColCreateExampleDate});
            this.dgvExample.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvExample.Location = new System.Drawing.Point(20, 96);
            this.dgvExample.MultiSelect = false;
            this.dgvExample.Name = "dgvExample";
            this.dgvExample.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvExample.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvExample.RowHeadersVisible = false;
            this.dgvExample.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvExample.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvExample.RowTemplate.Height = 23;
            this.dgvExample.SecondaryLength = 1;
            this.dgvExample.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvExample.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvExample.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvExample.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvExample.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExample.ShowEportContextMenu = false;
            this.dgvExample.Size = new System.Drawing.Size(321, 280);
            this.dgvExample.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvExample.TabIndex = 2;
            this.dgvExample.ToPrintCols = null;
            this.dgvExample.ToPrintRows = null;
            this.dgvExample.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExample_CellContentClick);
            // 
            // ColExampleName
            // 
            this.ColExampleName.DataPropertyName = "ExampleName";
            this.ColExampleName.HeaderText = "规格名称";
            this.ColExampleName.Name = "ColExampleName";
            // 
            // ColExampleType
            // 
            this.ColExampleType.DataPropertyName = "ExampleType";
            this.ColExampleType.HeaderText = "规格类型";
            this.ColExampleType.Name = "ColExampleType";
            // 
            // ColCreateExampleDate
            // 
            this.ColCreateExampleDate.DataPropertyName = "CreateExampleDate";
            this.ColCreateExampleDate.HeaderText = "创建日期";
            this.ColCreateExampleDate.Name = "ColCreateExampleDate";
            this.ColCreateExampleDate.ReadOnly = true;
            // 
            // cboCategory
            // 
            this.cboCategory.AutoComplete = false;
            this.cboCategory.AutoDropdown = false;
            this.cboCategory.BackColorEven = System.Drawing.Color.White;
            this.cboCategory.BackColorOdd = System.Drawing.Color.White;
            this.cboCategory.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboCategory.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboCategory.ColumnNames = "";
            this.cboCategory.ColumnWidthDefault = 75;
            this.cboCategory.ColumnWidths = "";
            this.cboCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.LinkedColumnIndex = 0;
            this.cboCategory.LinkedTextBox = null;
            this.cboCategory.Location = new System.Drawing.Point(88, 15);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(169, 22);
            this.cboCategory.TabIndex = 1;
            this.cboCategory.SelectedIndexChanged += new System.EventHandler(this.cboCategory_SelectedIndexChanged);
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.BackColor = System.Drawing.Color.Transparent;
            this.lblCategory.Location = new System.Drawing.Point(22, 20);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(53, 12);
            this.lblCategory.TabIndex = 0;
            this.lblCategory.Text = "当前类别";
            // 
            // UCSpecifications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveNew);
            this.Controls.Add(this.txtCategoryName);
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.btnDelCategory);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDeleteElement);
            this.Controls.Add(this.btnAddElement);
            this.Controls.Add(this.dgvElements);
            this.Controls.Add(this.btnDeleteExample);
            this.Controls.Add(this.btnAddExample);
            this.Controls.Add(this.dgvExample);
            this.Controls.Add(this.cboCategory);
            this.Controls.Add(this.lblCategory);
            this.Name = "UCSpecifications";
            this.Size = new System.Drawing.Size(709, 446);
            this.Load += new System.EventHandler(this.UCSpecifications_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvElements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExample)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblCategory;
        private Skyray.Controls.ComboBoxW cboCategory;
        private Skyray.Controls.DataGridViewW dgvExample;
        private Skyray.Controls.ButtonW btnAddExample;
        private Skyray.Controls.ButtonW btnDeleteExample;
        private Skyray.Controls.DataGridViewW dgvElements;
        private Skyray.Controls.ButtonW btnDeleteElement;
        private Skyray.Controls.ButtonW btnAddElement;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.ButtonW btnSave;
        private Skyray.Controls.ButtonW btnAddCategory;
        private Skyray.Controls.ButtonW btnDelCategory;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnSaveNew;
        private Skyray.Controls.TextBoxW txtCategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColExampleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColExampleType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCreateExampleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColElementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExampleMaxValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExampleMinValue;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}
