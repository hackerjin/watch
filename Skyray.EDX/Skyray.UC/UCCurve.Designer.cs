namespace Skyray.UC
{
    partial class UCCurve
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
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btWCurveToElement = new Skyray.Controls.ButtonW();
            this.btWImport = new Skyray.Controls.ButtonW();
            this.btwExportCurve = new Skyray.Controls.ButtonW();
            this.lblWorkArea = new Skyray.Controls.LabelW();
            this.cboWorkArea = new Skyray.Controls.ComboBoxW();
            this.dgvwCurveList = new Skyray.Controls.DataGridViewW();
            this.btnOpenCurve = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.txtCurveName = new Skyray.Controls.TextBoxW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.lblCurveName = new Skyray.Controls.LabelW();
            this.lblCurveType = new Skyray.Controls.LabelW();
            this.lblTestCondition = new Skyray.Controls.LabelW();
            this.cboConditionList = new Skyray.Controls.ComboBoxW();
            this.cboCurveType = new Skyray.Controls.ComboBoxW();
            this.btnReName = new Skyray.Controls.ButtonW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCalcType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCondition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSimilarCurve = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColIsDefaultWorkCurve = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColMainElement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemarkName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShowInMain = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCurveList)).BeginInit();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "XML file (*.xml)|*.xml";
            // 
            // btWCurveToElement
            // 
            this.btWCurveToElement.bSilver = false;
            this.btWCurveToElement.Location = new System.Drawing.Point(677, 469);
            this.btWCurveToElement.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWCurveToElement.MenuPos = new System.Drawing.Point(0, 0);
            this.btWCurveToElement.Name = "btWCurveToElement";
            this.btWCurveToElement.Size = new System.Drawing.Size(125, 25);
            this.btWCurveToElement.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWCurveToElement.TabIndex = 39;
            this.btWCurveToElement.Text = "下一步";
            this.btWCurveToElement.ToFocused = false;
            this.btWCurveToElement.UseVisualStyleBackColor = true;
            this.btWCurveToElement.Click += new System.EventHandler(this.btWCurveToElement_Click);
            // 
            // btWImport
            // 
            this.btWImport.bSilver = false;
            this.btWImport.Location = new System.Drawing.Point(677, 408);
            this.btWImport.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWImport.MenuPos = new System.Drawing.Point(0, 0);
            this.btWImport.Name = "btWImport";
            this.btWImport.Size = new System.Drawing.Size(125, 25);
            this.btWImport.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWImport.TabIndex = 38;
            this.btWImport.Text = "曲线导入";
            this.btWImport.ToFocused = false;
            this.btWImport.UseVisualStyleBackColor = true;
            this.btWImport.Click += new System.EventHandler(this.btWImport_Click);
            // 
            // btwExportCurve
            // 
            this.btwExportCurve.bSilver = false;
            this.btwExportCurve.Location = new System.Drawing.Point(677, 378);
            this.btwExportCurve.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btwExportCurve.MenuPos = new System.Drawing.Point(0, 0);
            this.btwExportCurve.Name = "btwExportCurve";
            this.btwExportCurve.Size = new System.Drawing.Size(125, 25);
            this.btwExportCurve.Style = Skyray.Controls.Style.Office2007Blue;
            this.btwExportCurve.TabIndex = 37;
            this.btwExportCurve.Text = "曲线导出";
            this.btwExportCurve.ToFocused = false;
            this.btwExportCurve.UseVisualStyleBackColor = true;
            this.btwExportCurve.Click += new System.EventHandler(this.btwExportCurve_Click);
            // 
            // lblWorkArea
            // 
            this.lblWorkArea.AutoSize = true;
            this.lblWorkArea.BackColor = System.Drawing.Color.Transparent;
            this.lblWorkArea.Location = new System.Drawing.Point(676, 212);
            this.lblWorkArea.Name = "lblWorkArea";
            this.lblWorkArea.Size = new System.Drawing.Size(43, 13);
            this.lblWorkArea.TabIndex = 35;
            this.lblWorkArea.Text = "工作区";
            this.lblWorkArea.Visible = false;
            // 
            // cboWorkArea
            // 
            this.cboWorkArea.AutoComplete = true;
            this.cboWorkArea.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboWorkArea.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWorkArea.AutoDropdown = true;
            this.cboWorkArea.BackColorEven = System.Drawing.Color.White;
            this.cboWorkArea.BackColorOdd = System.Drawing.Color.White;
            this.cboWorkArea.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboWorkArea.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboWorkArea.ColumnNames = "";
            this.cboWorkArea.ColumnWidthDefault = 75;
            this.cboWorkArea.ColumnWidths = "";
            this.cboWorkArea.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboWorkArea.FormattingEnabled = true;
            this.cboWorkArea.LinkedColumnIndex = 0;
            this.cboWorkArea.LinkedTextBox = null;
            this.cboWorkArea.Location = new System.Drawing.Point(677, 229);
            this.cboWorkArea.Name = "cboWorkArea";
            this.cboWorkArea.Size = new System.Drawing.Size(126, 21);
            this.cboWorkArea.TabIndex = 36;
            this.cboWorkArea.Visible = false;
            this.cboWorkArea.SelectedIndexChanged += new System.EventHandler(this.cboWorkArea_SelectedIndexChanged);
            // 
            // dgvwCurveList
            // 
            this.dgvwCurveList.AllowUserToAddRows = false;
            this.dgvwCurveList.AllowUserToDeleteRows = false;
            this.dgvwCurveList.AllowUserToResizeRows = false;
            this.dgvwCurveList.BackgroundColor = System.Drawing.Color.White;
            this.dgvwCurveList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwCurveList.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwCurveList.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwCurveList.ColumnHeadersHeight = 24;
            this.dgvwCurveList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwCurveList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColCalcType,
            this.ColCondition,
            this.ColSimilarCurve,
            this.ColIsDefaultWorkCurve,
            this.ColMainElement,
            this.colRemarkName,
            this.colTestTime,
            this.colShowInMain});
            this.dgvwCurveList.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvwCurveList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwCurveList.Location = new System.Drawing.Point(6, 7);
            this.dgvwCurveList.MultiSelect = false;
            this.dgvwCurveList.Name = "dgvwCurveList";
            this.dgvwCurveList.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwCurveList.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwCurveList.RowHeadersVisible = false;
            this.dgvwCurveList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwCurveList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwCurveList.RowTemplate.Height = 23;
            this.dgvwCurveList.SecondaryLength = 1;
            this.dgvwCurveList.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwCurveList.SecondaryRowColor2 = System.Drawing.Color.White;
            this.dgvwCurveList.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwCurveList.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwCurveList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwCurveList.ShowEportContextMenu = true;
            this.dgvwCurveList.Size = new System.Drawing.Size(652, 487);
            this.dgvwCurveList.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwCurveList.TabIndex = 34;
            this.dgvwCurveList.ToPrintCols = null;
            this.dgvwCurveList.ToPrintRows = null;
            this.dgvwCurveList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwCurveList_CellValueChanged);
            this.dgvwCurveList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwCurveList_CellDoubleClick);
            this.dgvwCurveList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvwCurveList_CellValidating);
            this.dgvwCurveList.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvwCurveList_CurrentCellDirtyStateChanged);
            this.dgvwCurveList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwCurveList_CellContentClick);
            // 
            // btnOpenCurve
            // 
            this.btnOpenCurve.bSilver = false;
            this.btnOpenCurve.Location = new System.Drawing.Point(678, 10);
            this.btnOpenCurve.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOpenCurve.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOpenCurve.Name = "btnOpenCurve";
            this.btnOpenCurve.Size = new System.Drawing.Size(126, 25);
            this.btnOpenCurve.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOpenCurve.TabIndex = 33;
            this.btnOpenCurve.Text = "打开";
            this.btnOpenCurve.ToFocused = false;
            this.btnOpenCurve.UseVisualStyleBackColor = true;
            this.btnOpenCurve.Click += new System.EventHandler(this.btnOpenCurve_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(677, 348);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(126, 25);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 31;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // txtCurveName
            // 
            this.txtCurveName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtCurveName.Location = new System.Drawing.Point(677, 91);
            this.txtCurveName.Name = "txtCurveName";
            this.txtCurveName.Size = new System.Drawing.Size(126, 20);
            this.txtCurveName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtCurveName.TabIndex = 25;
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(678, 40);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(126, 25);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 30;
            this.btnAdd.Text = "增加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblCurveName
            // 
            this.lblCurveName.AutoSize = true;
            this.lblCurveName.BackColor = System.Drawing.Color.Transparent;
            this.lblCurveName.Location = new System.Drawing.Point(677, 75);
            this.lblCurveName.Name = "lblCurveName";
            this.lblCurveName.Size = new System.Drawing.Size(55, 13);
            this.lblCurveName.TabIndex = 22;
            this.lblCurveName.Text = "曲线名称";
            // 
            // lblCurveType
            // 
            this.lblCurveType.AutoSize = true;
            this.lblCurveType.BackColor = System.Drawing.Color.Transparent;
            this.lblCurveType.Location = new System.Drawing.Point(677, 165);
            this.lblCurveType.Name = "lblCurveType";
            this.lblCurveType.Size = new System.Drawing.Size(55, 13);
            this.lblCurveType.TabIndex = 23;
            this.lblCurveType.Text = "曲线类型";
            // 
            // lblTestCondition
            // 
            this.lblTestCondition.AutoSize = true;
            this.lblTestCondition.BackColor = System.Drawing.Color.Transparent;
            this.lblTestCondition.Location = new System.Drawing.Point(677, 120);
            this.lblTestCondition.Name = "lblTestCondition";
            this.lblTestCondition.Size = new System.Drawing.Size(55, 13);
            this.lblTestCondition.TabIndex = 24;
            this.lblTestCondition.Text = "测试条件";
            // 
            // cboConditionList
            // 
            this.cboConditionList.AutoComplete = true;
            this.cboConditionList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboConditionList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboConditionList.AutoDropdown = true;
            this.cboConditionList.BackColorEven = System.Drawing.Color.White;
            this.cboConditionList.BackColorOdd = System.Drawing.Color.White;
            this.cboConditionList.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboConditionList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboConditionList.ColumnNames = "";
            this.cboConditionList.ColumnWidthDefault = 75;
            this.cboConditionList.ColumnWidths = "";
            this.cboConditionList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboConditionList.FormattingEnabled = true;
            this.cboConditionList.LinkedColumnIndex = 0;
            this.cboConditionList.LinkedTextBox = null;
            this.cboConditionList.Location = new System.Drawing.Point(677, 137);
            this.cboConditionList.Name = "cboConditionList";
            this.cboConditionList.Size = new System.Drawing.Size(126, 21);
            this.cboConditionList.TabIndex = 26;
            // 
            // cboCurveType
            // 
            this.cboCurveType.AutoComplete = false;
            this.cboCurveType.AutoDropdown = false;
            this.cboCurveType.BackColorEven = System.Drawing.Color.White;
            this.cboCurveType.BackColorOdd = System.Drawing.Color.White;
            this.cboCurveType.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboCurveType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboCurveType.ColumnNames = "";
            this.cboCurveType.ColumnWidthDefault = 75;
            this.cboCurveType.ColumnWidths = "";
            this.cboCurveType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboCurveType.FormattingEnabled = true;
            this.cboCurveType.LinkedColumnIndex = 0;
            this.cboCurveType.LinkedTextBox = null;
            this.cboCurveType.Location = new System.Drawing.Point(678, 183);
            this.cboCurveType.Name = "cboCurveType";
            this.cboCurveType.Size = new System.Drawing.Size(125, 21);
            this.cboCurveType.TabIndex = 28;
            // 
            // btnReName
            // 
            this.btnReName.bSilver = false;
            this.btnReName.Location = new System.Drawing.Point(677, 439);
            this.btnReName.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnReName.MenuPos = new System.Drawing.Point(0, 0);
            this.btnReName.Name = "btnReName";
            this.btnReName.Size = new System.Drawing.Size(125, 25);
            this.btnReName.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnReName.TabIndex = 40;
            this.btnReName.Text = "曲线重名";
            this.btnReName.ToFocused = false;
            this.btnReName.UseVisualStyleBackColor = true;
            this.btnReName.Click += new System.EventHandler(this.btnReName_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "工作曲线";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 160;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CalcType";
            this.dataGridViewTextBoxColumn2.HeaderText = "类型";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ConditionName";
            this.dataGridViewTextBoxColumn3.HeaderText = "测试条件";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 120;
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.DataPropertyName = "SimilarCurveName";
            this.dataGridViewComboBoxColumn1.HeaderText = "相似曲线";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsDefaultWorkCurve";
            this.dataGridViewCheckBoxColumn1.HeaderText = "默认";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "SimilarCurve";
            this.dataGridViewTextBoxColumn4.HeaderText = "相似曲线";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // ColName
            // 
            this.ColName.DataPropertyName = "Name";
            this.ColName.HeaderText = "工作曲线";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            this.ColName.Width = 160;
            // 
            // ColCalcType
            // 
            this.ColCalcType.DataPropertyName = "CalcType";
            this.ColCalcType.HeaderText = "类型";
            this.ColCalcType.Name = "ColCalcType";
            this.ColCalcType.ReadOnly = true;
            this.ColCalcType.Width = 50;
            // 
            // ColCondition
            // 
            this.ColCondition.DataPropertyName = "ConditionName";
            this.ColCondition.HeaderText = "测试条件";
            this.ColCondition.Name = "ColCondition";
            this.ColCondition.ReadOnly = true;
            this.ColCondition.Width = 120;
            // 
            // ColSimilarCurve
            // 
            this.ColSimilarCurve.DataPropertyName = "SimilarCurveName";
            this.ColSimilarCurve.HeaderText = "相似曲线";
            this.ColSimilarCurve.Name = "ColSimilarCurve";
            // 
            // ColIsDefaultWorkCurve
            // 
            this.ColIsDefaultWorkCurve.DataPropertyName = "IsDefaultWorkCurve";
            this.ColIsDefaultWorkCurve.HeaderText = "默认";
            this.ColIsDefaultWorkCurve.Name = "ColIsDefaultWorkCurve";
            this.ColIsDefaultWorkCurve.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColIsDefaultWorkCurve.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColIsDefaultWorkCurve.Width = 50;
            // 
            // ColMainElement
            // 
            this.ColMainElement.DataPropertyName = "MainElements";
            this.ColMainElement.HeaderText = "特征元素";
            this.ColMainElement.Name = "ColMainElement";
            // 
            // colRemarkName
            // 
            this.colRemarkName.DataPropertyName = "RemarkName";
            this.colRemarkName.HeaderText = "备注名称";
            this.colRemarkName.Name = "colRemarkName";
            this.colRemarkName.Visible = false;
            // 
            // colTestTime
            // 
            this.colTestTime.DataPropertyName = "TestTime";
            this.colTestTime.HeaderText = "测量时间";
            this.colTestTime.Name = "colTestTime";
            // 
            // colShowInMain
            // 
            this.colShowInMain.DataPropertyName = "IsShowMain";
            this.colShowInMain.HeaderText = "显示";
            this.colShowInMain.Name = "colShowInMain";
            // 
            // UCCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnReName);
            this.Controls.Add(this.btWCurveToElement);
            this.Controls.Add(this.btWImport);
            this.Controls.Add(this.btwExportCurve);
            this.Controls.Add(this.lblWorkArea);
            this.Controls.Add(this.cboWorkArea);
            this.Controls.Add(this.dgvwCurveList);
            this.Controls.Add(this.btnOpenCurve);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.txtCurveName);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblCurveName);
            this.Controls.Add(this.lblCurveType);
            this.Controls.Add(this.lblTestCondition);
            this.Controls.Add(this.cboConditionList);
            this.Controls.Add(this.cboCurveType);
            this.Name = "UCCurve";
            this.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Size = new System.Drawing.Size(848, 501);
            this.Load += new System.EventHandler(this.UCCurve_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCurveList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.TextBoxW txtCurveName;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.LabelW lblCurveName;
        private Skyray.Controls.LabelW lblCurveType;
        private Skyray.Controls.LabelW lblTestCondition;
        private Skyray.Controls.ComboBoxW cboConditionList;
        private Skyray.Controls.ComboBoxW cboCurveType;
        private Skyray.Controls.ButtonW btnOpenCurve;
        private Skyray.Controls.DataGridViewW dgvwCurveList;
        private Skyray.Controls.LabelW lblWorkArea;
        private Skyray.Controls.ComboBoxW cboWorkArea;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private Skyray.Controls.ButtonW btwExportCurve;
        private Skyray.Controls.ButtonW btWImport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private Skyray.Controls.ButtonW btWCurveToElement;
        private Skyray.Controls.ButtonW btnReName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCalcType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCondition;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColSimilarCurve;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsDefaultWorkCurve;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMainElement;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemarkName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTestTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colShowInMain;
    }
}
