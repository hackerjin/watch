using Skyray.EDX;
namespace Skyray.UC
{
    partial class FrmCondition
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnCancelSave = new Skyray.Controls.ButtonW();
            this.btnSaveNew = new Skyray.Controls.ButtonW();
            this.dgvwTestConList = new Skyray.Controls.DataGridViewW();
            this.btnDelTestCondition = new Skyray.Controls.ButtonW();
            this.btnAddTestCondition = new Skyray.Controls.ButtonW();
            this.lblConditionList = new Skyray.Controls.LabelW();
            this.cboConditionList = new Skyray.Controls.ComboBoxW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnAddCondition = new Skyray.Controls.ButtonW();
            this.btnDelCondition = new Skyray.Controls.ButtonW();
            this.txtConditionName = new Skyray.Controls.TextBoxW();
            this.lblTestCondition = new System.Windows.Forms.Label();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.meInit = new Skyray.EDX.Common.ModelEditor();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrecTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TubVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsFaceTubVoltage = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FaceTubVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TubCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilterIdx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollimatorIdx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTargetIdx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTargetMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColCurrentRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIsVacuum = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.VacuumTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIsVacuumDegree = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.VacuumDegree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIsAdjustRate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MinRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBeginChann = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColEndChann = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIsDistrubAlert = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColIsPeakFloat = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PeakCheckTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPeakFloatLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPeakFloatRight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPeakFloatChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPeakFloatError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Thickness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwTestConList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancelSave
            // 
            this.btnCancelSave.bSilver = false;
            this.btnCancelSave.Location = new System.Drawing.Point(452, 12);
            this.btnCancelSave.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancelSave.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancelSave.Name = "btnCancelSave";
            this.btnCancelSave.Size = new System.Drawing.Size(60, 23);
            this.btnCancelSave.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancelSave.TabIndex = 45;
            this.btnCancelSave.Text = "取消";
            this.btnCancelSave.ToFocused = false;
            this.btnCancelSave.UseVisualStyleBackColor = true;
            this.btnCancelSave.Visible = false;
            this.btnCancelSave.Click += new System.EventHandler(this.btnCancelSave_Click);
            // 
            // btnSaveNew
            // 
            this.btnSaveNew.bSilver = false;
            this.btnSaveNew.Location = new System.Drawing.Point(383, 12);
            this.btnSaveNew.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSaveNew.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSaveNew.Name = "btnSaveNew";
            this.btnSaveNew.Size = new System.Drawing.Size(60, 23);
            this.btnSaveNew.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSaveNew.TabIndex = 45;
            this.btnSaveNew.Text = "保存";
            this.btnSaveNew.ToFocused = false;
            this.btnSaveNew.UseVisualStyleBackColor = true;
            this.btnSaveNew.Visible = false;
            this.btnSaveNew.Click += new System.EventHandler(this.btnSaveNew_Click);
            // 
            // dgvwTestConList
            // 
            this.dgvwTestConList.AllowUserToAddRows = false;
            this.dgvwTestConList.AllowUserToDeleteRows = false;
            this.dgvwTestConList.AllowUserToResizeColumns = false;
            this.dgvwTestConList.AllowUserToResizeRows = false;
            this.dgvwTestConList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvwTestConList.BackgroundColor = System.Drawing.Color.White;
            this.dgvwTestConList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwTestConList.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwTestConList.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwTestConList.ColumnHeadersHeight = 24;
            this.dgvwTestConList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwTestConList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.PrecTime,
            this.TubVoltage,
            this.IsFaceTubVoltage,
            this.FaceTubVoltage,
            this.TubCurrent,
            this.FilterIdx,
            this.CollimatorIdx,
            this.ColTargetIdx,
            this.ColTargetMode,
            this.ColCurrentRate,
            this.ColIsVacuum,
            this.VacuumTime,
            this.ColIsVacuumDegree,
            this.VacuumDegree,
            this.ColIsAdjustRate,
            this.MinRate,
            this.MaxRate,
            this.ColBeginChann,
            this.ColEndChann,
            this.ColIsDistrubAlert,
            this.ColIsPeakFloat,
            this.PeakCheckTime,
            this.ColPeakFloatLeft,
            this.ColPeakFloatRight,
            this.ColPeakFloatChannel,
            this.ColPeakFloatError,
            this.Thickness});
            this.dgvwTestConList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwTestConList.Location = new System.Drawing.Point(11, 272);
            this.dgvwTestConList.MultiSelect = false;
            this.dgvwTestConList.Name = "dgvwTestConList";
            this.dgvwTestConList.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwTestConList.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwTestConList.RowHeadersVisible = false;
            this.dgvwTestConList.RowHeadersWidth = 21;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwTestConList.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvwTestConList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwTestConList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwTestConList.RowTemplate.Height = 23;
            this.dgvwTestConList.SecondaryLength = 1;
            this.dgvwTestConList.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwTestConList.SecondaryRowColor2 = System.Drawing.Color.White;
            this.dgvwTestConList.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwTestConList.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwTestConList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwTestConList.ShowEportContextMenu = true;
            this.dgvwTestConList.Size = new System.Drawing.Size(936, 166);
            this.dgvwTestConList.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwTestConList.TabIndex = 50;
            this.dgvwTestConList.ToPrintCols = null;
            this.dgvwTestConList.ToPrintRows = null;
            this.dgvwTestConList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwTestConList_CellValueChanged);
            this.dgvwTestConList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvwTestConList_CellValidating);
            this.dgvwTestConList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwTestConList_CellClick);
            this.dgvwTestConList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwTestConList_CellContentClick);
            // 
            // btnDelTestCondition
            // 
            this.btnDelTestCondition.bSilver = false;
            this.btnDelTestCondition.Font = new System.Drawing.Font("SimSun", 9F);
            this.btnDelTestCondition.Location = new System.Drawing.Point(855, 249);
            this.btnDelTestCondition.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelTestCondition.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelTestCondition.Name = "btnDelTestCondition";
            this.btnDelTestCondition.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnDelTestCondition.Size = new System.Drawing.Size(75, 20);
            this.btnDelTestCondition.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelTestCondition.TabIndex = 48;
            this.btnDelTestCondition.Text = "删除";
            this.btnDelTestCondition.ToFocused = false;
            this.btnDelTestCondition.UseVisualStyleBackColor = true;
            this.btnDelTestCondition.Click += new System.EventHandler(this.btnDelTestCondition_Click);
            // 
            // btnAddTestCondition
            // 
            this.btnAddTestCondition.bSilver = false;
            this.btnAddTestCondition.Font = new System.Drawing.Font("SimSun", 9F);
            this.btnAddTestCondition.Location = new System.Drawing.Point(774, 249);
            this.btnAddTestCondition.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAddTestCondition.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAddTestCondition.Name = "btnAddTestCondition";
            this.btnAddTestCondition.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnAddTestCondition.Size = new System.Drawing.Size(75, 20);
            this.btnAddTestCondition.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAddTestCondition.TabIndex = 47;
            this.btnAddTestCondition.Text = "增加";
            this.btnAddTestCondition.ToFocused = false;
            this.btnAddTestCondition.UseVisualStyleBackColor = true;
            this.btnAddTestCondition.Click += new System.EventHandler(this.btnAddTestCondition_Click);
            // 
            // lblConditionList
            // 
            this.lblConditionList.AutoSize = true;
            this.lblConditionList.BackColor = System.Drawing.Color.Transparent;
            this.lblConditionList.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConditionList.Location = new System.Drawing.Point(13, 17);
            this.lblConditionList.Name = "lblConditionList";
            this.lblConditionList.Size = new System.Drawing.Size(57, 12);
            this.lblConditionList.TabIndex = 3;
            this.lblConditionList.Text = "条件列表";
            // 
            // cboConditionList
            // 
            this.cboConditionList.AutoComplete = false;
            this.cboConditionList.AutoDropdown = false;
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
            this.cboConditionList.Location = new System.Drawing.Point(106, 13);
            this.cboConditionList.Name = "cboConditionList";
            this.cboConditionList.Size = new System.Drawing.Size(130, 22);
            this.cboConditionList.TabIndex = 2;
            this.cboConditionList.SelectedIndexChanged += new System.EventHandler(this.cboConditionList_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(855, 11);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancelCondition_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(748, 11);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(92, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOKCondition_Click);
            // 
            // btnAddCondition
            // 
            this.btnAddCondition.bSilver = false;
            this.btnAddCondition.Location = new System.Drawing.Point(314, 16);
            this.btnAddCondition.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAddCondition.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAddCondition.Name = "btnAddCondition";
            this.btnAddCondition.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnAddCondition.Size = new System.Drawing.Size(60, 19);
            this.btnAddCondition.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAddCondition.TabIndex = 0;
            this.btnAddCondition.Text = "添加";
            this.btnAddCondition.ToFocused = false;
            this.btnAddCondition.UseVisualStyleBackColor = true;
            this.btnAddCondition.Click += new System.EventHandler(this.btnAddCondition_Click);
            // 
            // btnDelCondition
            // 
            this.btnDelCondition.bSilver = false;
            this.btnDelCondition.Location = new System.Drawing.Point(252, 16);
            this.btnDelCondition.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelCondition.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelCondition.Name = "btnDelCondition";
            this.btnDelCondition.ShowBase = Skyray.Controls.ButtonW.e_showbase.No;
            this.btnDelCondition.Size = new System.Drawing.Size(60, 19);
            this.btnDelCondition.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelCondition.TabIndex = 0;
            this.btnDelCondition.Text = "删除";
            this.btnDelCondition.ToFocused = false;
            this.btnDelCondition.UseVisualStyleBackColor = true;
            this.btnDelCondition.Click += new System.EventHandler(this.btnDelCondition_Click);
            // 
            // txtConditionName
            // 
            this.txtConditionName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtConditionName.Location = new System.Drawing.Point(244, 14);
            this.txtConditionName.Name = "txtConditionName";
            this.txtConditionName.Size = new System.Drawing.Size(135, 21);
            this.txtConditionName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtConditionName.TabIndex = 20;
            this.txtConditionName.Visible = false;
            // 
            // lblTestCondition
            // 
            this.lblTestCondition.AutoSize = true;
            this.lblTestCondition.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold);
            this.lblTestCondition.Location = new System.Drawing.Point(12, 256);
            this.lblTestCondition.Name = "lblTestCondition";
            this.lblTestCondition.Size = new System.Drawing.Size(57, 12);
            this.lblTestCondition.TabIndex = 51;
            this.lblTestCondition.Text = "测量条件";
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(638, 11);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(92, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 52;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // meInit
            // 
            this.meInit.DataSource = null;
            this.meInit.GroupTitle = "";
            this.meInit.LabelPosition = Skyray.EDX.Common.LabelPosition.Top;
            this.meInit.LayoutType = null;
            this.meInit.Location = new System.Drawing.Point(6, 41);
            this.meInit.Name = "meInit";
            this.meInit.Size = new System.Drawing.Size(946, 202);
            this.meInit.SLayoutType = null;
            this.meInit.TabIndex = 46;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColName.DataPropertyName = "Name";
            this.ColName.HeaderText = "名称";
            this.ColName.Name = "ColName";
            this.ColName.Width = 54;
            // 
            // PrecTime
            // 
            this.PrecTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.PrecTime.DataPropertyName = "PrecTime";
            this.PrecTime.HeaderText = "测量时间";
            this.PrecTime.Name = "PrecTime";
            this.PrecTime.Width = 78;
            // 
            // TubVoltage
            // 
            this.TubVoltage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.TubVoltage.DataPropertyName = "TubVoltage";
            this.TubVoltage.HeaderText = "管压";
            this.TubVoltage.Name = "TubVoltage";
            this.TubVoltage.Width = 54;
            // 
            // IsFaceTubVoltage
            // 
            this.IsFaceTubVoltage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IsFaceTubVoltage.DataPropertyName = "IsFaceTubVoltage";
            this.IsFaceTubVoltage.HeaderText = "实际管压";
            this.IsFaceTubVoltage.Name = "IsFaceTubVoltage";
            this.IsFaceTubVoltage.Width = 84;
            // 
            // FaceTubVoltage
            // 
            this.FaceTubVoltage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.FaceTubVoltage.DataPropertyName = "FaceTubVoltage";
            this.FaceTubVoltage.HeaderText = "实际管压值";
            this.FaceTubVoltage.Name = "FaceTubVoltage";
            this.FaceTubVoltage.Width = 90;
            // 
            // TubCurrent
            // 
            this.TubCurrent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.TubCurrent.DataPropertyName = "TubCurrent";
            this.TubCurrent.HeaderText = "管流";
            this.TubCurrent.Name = "TubCurrent";
            this.TubCurrent.Width = 54;
            // 
            // FilterIdx
            // 
            this.FilterIdx.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.FilterIdx.DataPropertyName = "FilterIdx";
            this.FilterIdx.HeaderText = "滤光片";
            this.FilterIdx.Name = "FilterIdx";
            this.FilterIdx.Width = 66;
            // 
            // CollimatorIdx
            // 
            this.CollimatorIdx.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.CollimatorIdx.DataPropertyName = "CollimatorIdx";
            this.CollimatorIdx.HeaderText = "准直器";
            this.CollimatorIdx.Name = "CollimatorIdx";
            this.CollimatorIdx.Width = 66;
            // 
            // ColTargetIdx
            // 
            this.ColTargetIdx.DataPropertyName = "TargetIdx";
            this.ColTargetIdx.HeaderText = "靶材";
            this.ColTargetIdx.Name = "ColTargetIdx";
            this.ColTargetIdx.Width = 54;
            // 
            // ColTargetMode
            // 
            this.ColTargetMode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColTargetMode.DataPropertyName = "TargetMode";
            this.ColTargetMode.HeaderText = "靶材模式";
            this.ColTargetMode.Name = "ColTargetMode";
            this.ColTargetMode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColTargetMode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColTargetMode.Width = 84;
            // 
            // ColCurrentRate
            // 
            this.ColCurrentRate.DataPropertyName = "CurrentRate";
            this.ColCurrentRate.HeaderText = "管流因子";
            this.ColCurrentRate.Name = "ColCurrentRate";
            this.ColCurrentRate.Width = 78;
            // 
            // ColIsVacuum
            // 
            this.ColIsVacuum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColIsVacuum.DataPropertyName = "IsVacuum";
            this.ColIsVacuum.HeaderText = "抽真空方式1";
            this.ColIsVacuum.Name = "ColIsVacuum";
            this.ColIsVacuum.Width = 88;
            // 
            // VacuumTime
            // 
            this.VacuumTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.VacuumTime.DataPropertyName = "VacuumTime";
            this.VacuumTime.HeaderText = "抽真空时间";
            this.VacuumTime.Name = "VacuumTime";
            this.VacuumTime.Width = 90;
            // 
            // ColIsVacuumDegree
            // 
            this.ColIsVacuumDegree.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColIsVacuumDegree.DataPropertyName = "IsVacuumDegree";
            this.ColIsVacuumDegree.HeaderText = "抽真空方式2";
            this.ColIsVacuumDegree.Name = "ColIsVacuumDegree";
            this.ColIsVacuumDegree.Width = 110;
            // 
            // VacuumDegree
            // 
            this.VacuumDegree.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.VacuumDegree.DataPropertyName = "VacuumDegree";
            this.VacuumDegree.HeaderText = "真空度";
            this.VacuumDegree.Name = "VacuumDegree";
            this.VacuumDegree.Width = 66;
            // 
            // ColIsAdjustRate
            // 
            this.ColIsAdjustRate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColIsAdjustRate.DataPropertyName = "IsAdjustRate";
            this.ColIsAdjustRate.HeaderText = "调节计数率";
            this.ColIsAdjustRate.Name = "ColIsAdjustRate";
            this.ColIsAdjustRate.Width = 71;
            // 
            // MinRate
            // 
            this.MinRate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MinRate.DataPropertyName = "MinRate";
            this.MinRate.HeaderText = "最小计数率";
            this.MinRate.Name = "MinRate";
            this.MinRate.Width = 90;
            // 
            // MaxRate
            // 
            this.MaxRate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MaxRate.DataPropertyName = "MaxRate";
            this.MaxRate.HeaderText = "最大计数率";
            this.MaxRate.Name = "MaxRate";
            this.MaxRate.Width = 90;
            // 
            // ColBeginChann
            // 
            this.ColBeginChann.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColBeginChann.DataPropertyName = "BeginChann";
            this.ColBeginChann.HeaderText = "初始道";
            this.ColBeginChann.Name = "ColBeginChann";
            this.ColBeginChann.Width = 66;
            // 
            // ColEndChann
            // 
            this.ColEndChann.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColEndChann.DataPropertyName = "EndChann";
            this.ColEndChann.HeaderText = "结束道";
            this.ColEndChann.Name = "ColEndChann";
            this.ColEndChann.Width = 66;
            // 
            // ColIsDistrubAlert
            // 
            this.ColIsDistrubAlert.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColIsDistrubAlert.DataPropertyName = "IsDistrubAlert";
            this.ColIsDistrubAlert.HeaderText = "干扰报警";
            this.ColIsDistrubAlert.Name = "ColIsDistrubAlert";
            this.ColIsDistrubAlert.Visible = false;
            this.ColIsDistrubAlert.Width = 59;
            // 
            // ColIsPeakFloat
            // 
            this.ColIsPeakFloat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColIsPeakFloat.DataPropertyName = "IsPeakFloat";
            this.ColIsPeakFloat.HeaderText = "判断峰飘";
            this.ColIsPeakFloat.Name = "ColIsPeakFloat";
            this.ColIsPeakFloat.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColIsPeakFloat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColIsPeakFloat.Width = 78;
            // 
            // PeakCheckTime
            // 
            this.PeakCheckTime.DataPropertyName = "PeakCheckTime";
            this.PeakCheckTime.HeaderText = "判断开始时间";
            this.PeakCheckTime.Name = "PeakCheckTime";
            this.PeakCheckTime.Width = 102;
            // 
            // ColPeakFloatLeft
            // 
            this.ColPeakFloatLeft.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColPeakFloatLeft.DataPropertyName = "PeakFloatLeft";
            this.ColPeakFloatLeft.HeaderText = "峰飘左界";
            this.ColPeakFloatLeft.Name = "ColPeakFloatLeft";
            this.ColPeakFloatLeft.Width = 78;
            // 
            // ColPeakFloatRight
            // 
            this.ColPeakFloatRight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColPeakFloatRight.DataPropertyName = "PeakFloatRight";
            this.ColPeakFloatRight.HeaderText = "峰飘右界";
            this.ColPeakFloatRight.Name = "ColPeakFloatRight";
            this.ColPeakFloatRight.Width = 78;
            // 
            // ColPeakFloatChannel
            // 
            this.ColPeakFloatChannel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColPeakFloatChannel.DataPropertyName = "PeakFloatChannel";
            this.ColPeakFloatChannel.HeaderText = "峰道址";
            this.ColPeakFloatChannel.Name = "ColPeakFloatChannel";
            this.ColPeakFloatChannel.Width = 66;
            // 
            // ColPeakFloatError
            // 
            this.ColPeakFloatError.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColPeakFloatError.DataPropertyName = "PeakFloatError";
            this.ColPeakFloatError.HeaderText = "误差道";
            this.ColPeakFloatError.Name = "ColPeakFloatError";
            this.ColPeakFloatError.Width = 66;
            // 
            // Thickness
            // 
            this.Thickness.DataPropertyName = "Thickness";
            this.Thickness.HeaderText = "窗口厚度(mm)";
            this.Thickness.Name = "Thickness";
            this.Thickness.Width = 102;
            // 
            // FrmCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.lblTestCondition);
            this.Controls.Add(this.btnAddTestCondition);
            this.Controls.Add(this.btnDelTestCondition);
            this.Controls.Add(this.dgvwTestConList);
            this.Controls.Add(this.meInit);
            this.Controls.Add(this.btnCancelSave);
            this.Controls.Add(this.btnSaveNew);
            this.Controls.Add(this.lblConditionList);
            this.Controls.Add(this.cboConditionList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnAddCondition);
            this.Controls.Add(this.btnDelCondition);
            this.Controls.Add(this.txtConditionName);
            this.Name = "FrmCondition";
            this.Size = new System.Drawing.Size(958, 449);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwTestConList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ButtonW btnDelCondition;
        private Skyray.Controls.ButtonW btnAddCondition;
        private Skyray.Controls.ComboBoxW cboConditionList;
        private Skyray.Controls.LabelW lblConditionList;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.TextBoxW txtConditionName;
        private Skyray.Controls.ButtonW btnSaveNew;
        private Skyray.Controls.ButtonW btnCancelSave;
        private Skyray.Controls.ButtonW btnDelTestCondition;
        private Skyray.Controls.ButtonW btnAddTestCondition;
        private Skyray.EDX.Common.ModelEditor meInit;
        private Skyray.Controls.DataGridViewW dgvwTestConList;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.Label lblTestCondition;
        private Skyray.Controls.ButtonW btnApplication;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrecTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn TubVoltage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsFaceTubVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn FaceTubVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn TubCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilterIdx;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollimatorIdx;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTargetIdx;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColTargetMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCurrentRate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsVacuum;
        private System.Windows.Forms.DataGridViewTextBoxColumn VacuumTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsVacuumDegree;
        private System.Windows.Forms.DataGridViewTextBoxColumn VacuumDegree;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsAdjustRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBeginChann;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColEndChann;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsDistrubAlert;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsPeakFloat;
        private System.Windows.Forms.DataGridViewTextBoxColumn PeakCheckTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPeakFloatLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPeakFloatRight;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPeakFloatChannel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPeakFloatError;
        private System.Windows.Forms.DataGridViewTextBoxColumn Thickness;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn5;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn6;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        //private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn5;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
    }
}