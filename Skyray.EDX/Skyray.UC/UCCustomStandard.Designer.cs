namespace Skyray.UC
{
    partial class UCCustomStandard
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
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gpStand = new System.Windows.Forms.GroupBox();
            this.txbTotalContext = new Skyray.Controls.TextBoxW();
            this.checkTotalValue = new Skyray.Controls.CheckBoxW();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.lblCurrentStandard = new Skyray.Controls.LabelW();
            this.btnDelData = new Skyray.Controls.ButtonW();
            this.btnSelectElement = new Skyray.Controls.ButtonW();
            this.dgvwStandardDatas = new Skyray.Controls.DataGridViewW();
            this.ColElementCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartStandardContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StandardContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StandardThick = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StandardThickMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColUnitThick = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtNewStandard = new Skyray.Controls.TextBoxW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.lbwStandardList = new Skyray.Controls.ListBoxW();
            this.gpCurve = new System.Windows.Forms.GroupBox();
            this.dgvCurveStandardName = new Skyray.Controls.DataGridViewW();
            this.colworkCurve = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStandardName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnAddCustum = new Skyray.Controls.ButtonW();
            this.gpStand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwStandardDatas)).BeginInit();
            this.gpCurve.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurveStandardName)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ElementCaption";
            this.dataGridViewTextBoxColumn1.HeaderText = "元素名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "StartStandardContent";
            this.dataGridViewTextBoxColumn2.FillWeight = 120F;
            this.dataGridViewTextBoxColumn2.HeaderText = "标准含量(ppm)";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 120;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "StandardContent";
            this.dataGridViewTextBoxColumn3.HeaderText = "标准含量(ppm)";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "含量单位";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "StandardThick";
            this.dataGridViewTextBoxColumn5.HeaderText = "标准厚度";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "StandardThickMax";
            this.dataGridViewTextBoxColumn6.HeaderText = "厚度单位";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "厚度单位";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // gpStand
            // 
            this.gpStand.Controls.Add(this.btnAddCustum);
            this.gpStand.Controls.Add(this.txbTotalContext);
            this.gpStand.Controls.Add(this.checkTotalValue);
            this.gpStand.Controls.Add(this.btnApplication);
            this.gpStand.Controls.Add(this.lblCurrentStandard);
            this.gpStand.Controls.Add(this.btnDelData);
            this.gpStand.Controls.Add(this.btnSelectElement);
            this.gpStand.Controls.Add(this.dgvwStandardDatas);
            this.gpStand.Controls.Add(this.txtNewStandard);
            this.gpStand.Controls.Add(this.btnAdd);
            this.gpStand.Controls.Add(this.btnDel);
            this.gpStand.Controls.Add(this.btnCancel);
            this.gpStand.Controls.Add(this.btnOK);
            this.gpStand.Controls.Add(this.lbwStandardList);
            this.gpStand.Location = new System.Drawing.Point(264, 4);
            this.gpStand.Name = "gpStand";
            this.gpStand.Size = new System.Drawing.Size(634, 364);
            this.gpStand.TabIndex = 28;
            this.gpStand.TabStop = false;
            // 
            // txbTotalContext
            // 
            this.txbTotalContext.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txbTotalContext.Location = new System.Drawing.Point(489, 188);
            this.txbTotalContext.Name = "txbTotalContext";
            this.txbTotalContext.Size = new System.Drawing.Size(113, 21);
            this.txbTotalContext.Style = Skyray.Controls.Style.Office2007Blue;
            this.txbTotalContext.TabIndex = 39;
            this.txbTotalContext.TextChanged += new System.EventHandler(this.txbTotalContext_TextChanged);
            // 
            // checkTotalValue
            // 
            this.checkTotalValue.AutoSize = true;
            this.checkTotalValue.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.checkTotalValue.Location = new System.Drawing.Point(489, 165);
            this.checkTotalValue.Name = "checkTotalValue";
            this.checkTotalValue.Size = new System.Drawing.Size(90, 16);
            this.checkTotalValue.Style = Skyray.Controls.Style.Office2007Blue;
            this.checkTotalValue.TabIndex = 38;
            this.checkTotalValue.Text = "总含量(ppm)";
            this.checkTotalValue.UseVisualStyleBackColor = true;
            this.checkTotalValue.CheckedChanged += new System.EventHandler(this.checkTotalValue_CheckedChanged);
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(489, 228);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(113, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 37;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // lblCurrentStandard
            // 
            this.lblCurrentStandard.AutoSize = true;
            this.lblCurrentStandard.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentStandard.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentStandard.Location = new System.Drawing.Point(27, 15);
            this.lblCurrentStandard.Name = "lblCurrentStandard";
            this.lblCurrentStandard.Size = new System.Drawing.Size(83, 12);
            this.lblCurrentStandard.TabIndex = 36;
            this.lblCurrentStandard.Text = "选择当前标准";
            // 
            // btnDelData
            // 
            this.btnDelData.bSilver = false;
            this.btnDelData.Location = new System.Drawing.Point(485, 83);
            this.btnDelData.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelData.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelData.Name = "btnDelData";
            this.btnDelData.Size = new System.Drawing.Size(113, 23);
            this.btnDelData.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelData.TabIndex = 35;
            this.btnDelData.Text = "删除元素";
            this.btnDelData.ToFocused = false;
            this.btnDelData.UseVisualStyleBackColor = true;
            this.btnDelData.Click += new System.EventHandler(this.btnDelData_Click);
            // 
            // btnSelectElement
            // 
            this.btnSelectElement.bSilver = false;
            this.btnSelectElement.Location = new System.Drawing.Point(485, 17);
            this.btnSelectElement.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSelectElement.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSelectElement.Name = "btnSelectElement";
            this.btnSelectElement.Size = new System.Drawing.Size(113, 23);
            this.btnSelectElement.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSelectElement.TabIndex = 34;
            this.btnSelectElement.Text = "选择元素";
            this.btnSelectElement.ToFocused = false;
            this.btnSelectElement.UseVisualStyleBackColor = true;
            this.btnSelectElement.Click += new System.EventHandler(this.btnSelectElement_Click);
            // 
            // dgvwStandardDatas
            // 
            this.dgvwStandardDatas.AllowUserToAddRows = false;
            this.dgvwStandardDatas.AllowUserToResizeColumns = false;
            this.dgvwStandardDatas.AllowUserToResizeRows = false;
            this.dgvwStandardDatas.BackgroundColor = System.Drawing.Color.White;
            this.dgvwStandardDatas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwStandardDatas.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwStandardDatas.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwStandardDatas.ColumnHeadersHeight = 24;
            this.dgvwStandardDatas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwStandardDatas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColElementCaption,
            this.StartStandardContent,
            this.StandardContent,
            this.ColUnit,
            this.StandardThick,
            this.StandardThickMax,
            this.ColUnitThick});
            this.dgvwStandardDatas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwStandardDatas.Location = new System.Drawing.Point(176, 14);
            this.dgvwStandardDatas.Name = "dgvwStandardDatas";
            this.dgvwStandardDatas.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwStandardDatas.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwStandardDatas.RowHeadersVisible = false;
            this.dgvwStandardDatas.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwStandardDatas.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwStandardDatas.RowTemplate.Height = 23;
            this.dgvwStandardDatas.SecondaryLength = 1;
            this.dgvwStandardDatas.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwStandardDatas.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwStandardDatas.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwStandardDatas.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(217)))), ((int)(((byte)(254)))));
            this.dgvwStandardDatas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwStandardDatas.ShowEportContextMenu = true;
            this.dgvwStandardDatas.Size = new System.Drawing.Size(303, 302);
            this.dgvwStandardDatas.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwStandardDatas.TabIndex = 33;
            this.dgvwStandardDatas.ToPrintCols = null;
            this.dgvwStandardDatas.ToPrintRows = null;
            this.dgvwStandardDatas.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwStandardDatas_CellValueChanged);
            // 
            // ColElementCaption
            // 
            this.ColElementCaption.DataPropertyName = "ElementCaption";
            this.ColElementCaption.HeaderText = "元素名称";
            this.ColElementCaption.Name = "ColElementCaption";
            this.ColElementCaption.ReadOnly = true;
            this.ColElementCaption.Width = 80;
            // 
            // StartStandardContent
            // 
            this.StartStandardContent.DataPropertyName = "StartStandardContent";
            this.StartStandardContent.FillWeight = 120F;
            this.StartStandardContent.HeaderText = "标准含量L(ppm)";
            this.StartStandardContent.Name = "StartStandardContent";
            this.StartStandardContent.Width = 120;
            // 
            // StandardContent
            // 
            this.StandardContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StandardContent.DataPropertyName = "StandardContent";
            this.StandardContent.HeaderText = "标准含量(ppm)";
            this.StandardContent.Name = "StandardContent";
            // 
            // ColUnit
            // 
            this.ColUnit.HeaderText = "含量单位";
            this.ColUnit.Name = "ColUnit";
            this.ColUnit.Visible = false;
            // 
            // StandardThick
            // 
            this.StandardThick.DataPropertyName = "StandardThick";
            this.StandardThick.HeaderText = "标准厚度";
            this.StandardThick.Name = "StandardThick";
            this.StandardThick.Visible = false;
            // 
            // StandardThickMax
            // 
            this.StandardThickMax.DataPropertyName = "StandardThickMax";
            this.StandardThickMax.HeaderText = "标准厚度MAX";
            this.StandardThickMax.Name = "StandardThickMax";
            this.StandardThickMax.Visible = false;
            // 
            // ColUnitThick
            // 
            this.ColUnitThick.HeaderText = "厚度单位";
            this.ColUnitThick.Name = "ColUnitThick";
            this.ColUnitThick.Visible = false;
            // 
            // txtNewStandard
            // 
            this.txtNewStandard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtNewStandard.Location = new System.Drawing.Point(176, 330);
            this.txtNewStandard.Name = "txtNewStandard";
            this.txtNewStandard.Size = new System.Drawing.Size(142, 21);
            this.txtNewStandard.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtNewStandard.TabIndex = 31;
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(324, 330);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(91, 21);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 30;
            this.btnAdd.Text = "新建标准";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(27, 330);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(143, 21);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 32;
            this.btnDel.Text = "删除标准";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(489, 294);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(113, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 29;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(489, 261);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(113, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 28;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbwStandardList
            // 
            this.lbwStandardList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.lbwStandardList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbwStandardList.FormattingEnabled = true;
            this.lbwStandardList.HorizontalScrollbar = true;
            this.lbwStandardList.ItemHeight = 21;
            this.lbwStandardList.Location = new System.Drawing.Point(27, 39);
            this.lbwStandardList.Name = "lbwStandardList";
            this.lbwStandardList.Size = new System.Drawing.Size(142, 256);
            this.lbwStandardList.Style = Skyray.Controls.Style.Office2007Blue;
            this.lbwStandardList.TabIndex = 27;
            this.lbwStandardList.SelectedIndexChanged += new System.EventHandler(this.lbwStandardList_SelectedIndexChanged);
            // 
            // gpCurve
            // 
            this.gpCurve.Controls.Add(this.dgvCurveStandardName);
            this.gpCurve.Location = new System.Drawing.Point(11, 4);
            this.gpCurve.Name = "gpCurve";
            this.gpCurve.Size = new System.Drawing.Size(222, 364);
            this.gpCurve.TabIndex = 29;
            this.gpCurve.TabStop = false;
            // 
            // dgvCurveStandardName
            // 
            this.dgvCurveStandardName.AllowUserToAddRows = false;
            this.dgvCurveStandardName.AllowUserToResizeRows = false;
            this.dgvCurveStandardName.BackgroundColor = System.Drawing.Color.White;
            this.dgvCurveStandardName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCurveStandardName.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvCurveStandardName.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvCurveStandardName.ColumnHeadersHeight = 20;
            this.dgvCurveStandardName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCurveStandardName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colworkCurve,
            this.colStandardName});
            this.dgvCurveStandardName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCurveStandardName.Location = new System.Drawing.Point(3, 17);
            this.dgvCurveStandardName.Name = "dgvCurveStandardName";
            this.dgvCurveStandardName.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvCurveStandardName.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvCurveStandardName.RowHeadersVisible = false;
            this.dgvCurveStandardName.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvCurveStandardName.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvCurveStandardName.RowTemplate.Height = 23;
            this.dgvCurveStandardName.SecondaryLength = 1;
            this.dgvCurveStandardName.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvCurveStandardName.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvCurveStandardName.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvCurveStandardName.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvCurveStandardName.ShowEportContextMenu = false;
            this.dgvCurveStandardName.Size = new System.Drawing.Size(216, 344);
            this.dgvCurveStandardName.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvCurveStandardName.TabIndex = 0;
            this.dgvCurveStandardName.ToPrintCols = null;
            this.dgvCurveStandardName.ToPrintRows = null;
            // 
            // colworkCurve
            // 
            this.colworkCurve.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colworkCurve.DataPropertyName = "Name";
            this.colworkCurve.HeaderText = "工作曲线";
            this.colworkCurve.Name = "colworkCurve";
            this.colworkCurve.ReadOnly = true;
            // 
            // colStandardName
            // 
            this.colStandardName.DataPropertyName = "ThickStandardName";
            this.colStandardName.HeaderText = "标准";
            this.colStandardName.Name = "colStandardName";
            this.colStandardName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStandardName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnAddCustum
            // 
            this.btnAddCustum.bSilver = false;
            this.btnAddCustum.Location = new System.Drawing.Point(485, 49);
            this.btnAddCustum.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAddCustum.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAddCustum.Name = "btnAddCustum";
            this.btnAddCustum.Size = new System.Drawing.Size(113, 23);
            this.btnAddCustum.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAddCustum.TabIndex = 40;
            this.btnAddCustum.Text = "添加自定义";
            this.btnAddCustum.ToFocused = false;
            this.btnAddCustum.UseVisualStyleBackColor = true;
            this.btnAddCustum.Click += new System.EventHandler(this.btnAddCustum_Click);
            // 
            // UCCustomStandard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.gpCurve);
            this.Controls.Add(this.gpStand);
            this.Name = "UCCustomStandard";
            this.Size = new System.Drawing.Size(933, 388);
            this.Load += new System.EventHandler(this.CustomStandard_Load);
            this.gpStand.ResumeLayout(false);
            this.gpStand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwStandardDatas)).EndInit();
            this.gpCurve.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurveStandardName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.GroupBox gpStand;
        private Skyray.Controls.TextBoxW txbTotalContext;
        private Skyray.Controls.CheckBoxW checkTotalValue;
        private Skyray.Controls.ButtonW btnApplication;
        private Skyray.Controls.LabelW lblCurrentStandard;
        private Skyray.Controls.ButtonW btnDelData;
        private Skyray.Controls.ButtonW btnSelectElement;
        private Skyray.Controls.DataGridViewW dgvwStandardDatas;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColElementCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartStandardContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn StandardContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn StandardThick;
        private System.Windows.Forms.DataGridViewTextBoxColumn StandardThickMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColUnitThick;
        private Skyray.Controls.TextBoxW txtNewStandard;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ListBoxW lbwStandardList;
        private System.Windows.Forms.GroupBox gpCurve;
        private Skyray.Controls.DataGridViewW dgvCurveStandardName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colworkCurve;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStandardName;
        private Skyray.Controls.ButtonW btnAddCustum;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}
