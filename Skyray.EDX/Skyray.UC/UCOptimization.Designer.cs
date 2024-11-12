namespace Skyray.UC
{
    partial class UCOptimization
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
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.lbwOptimization = new Skyray.Controls.ListBoxW();
            this.dgvwOptimization = new Skyray.Controls.DataGridViewW();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.btWOptimizeTo = new Skyray.Controls.ButtonW();
            this.btWUp = new Skyray.Controls.ButtonW();
            this.comBoxOptimizationType = new Skyray.Controls.ComboBoxW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptimiztionValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptExpression = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptimiztionMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptimiztionMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptimiztionRange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptimiztionFactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsJoinIntensity = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwOptimization)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(113, 281);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(76, 25);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 23;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(359, 281);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(277, 281);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 25);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 22;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(31, 281);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(76, 25);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 20;
            this.btnAdd.Text = "增加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbwOptimization
            // 
            this.lbwOptimization.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.lbwOptimization.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbwOptimization.FormattingEnabled = true;
            this.lbwOptimization.HorizontalScrollbar = true;
            this.lbwOptimization.ItemHeight = 17;
            this.lbwOptimization.Location = new System.Drawing.Point(8, 39);
            this.lbwOptimization.Name = "lbwOptimization";
            this.lbwOptimization.Size = new System.Drawing.Size(125, 225);
            this.lbwOptimization.Style = Skyray.Controls.Style.Office2007Blue;
            this.lbwOptimization.TabIndex = 24;
            this.lbwOptimization.SelectedIndexChanged += new System.EventHandler(this.lbwOptimization_SelectedIndexChanged);
            // 
            // dgvwOptimization
            // 
            this.dgvwOptimization.AllowUserToAddRows = false;
            this.dgvwOptimization.AllowUserToDeleteRows = false;
            this.dgvwOptimization.AllowUserToResizeColumns = false;
            this.dgvwOptimization.AllowUserToResizeRows = false;
            this.dgvwOptimization.BackgroundColor = System.Drawing.Color.White;
            this.dgvwOptimization.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwOptimization.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwOptimization.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwOptimization.ColumnHeadersHeight = 24;
            this.dgvwOptimization.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwOptimization.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OptimiztionValue,
            this.OptExpression,
            this.OptimiztionMin,
            this.OptimiztionMax,
            this.OptimiztionRange,
            this.OptimiztionFactor,
            this.IsJoinIntensity});
            this.dgvwOptimization.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwOptimization.Location = new System.Drawing.Point(139, 39);
            this.dgvwOptimization.MultiSelect = false;
            this.dgvwOptimization.Name = "dgvwOptimization";
            this.dgvwOptimization.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwOptimization.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwOptimization.RowHeadersVisible = false;
            this.dgvwOptimization.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwOptimization.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwOptimization.RowTemplate.Height = 23;
            this.dgvwOptimization.SecondaryLength = 1;
            this.dgvwOptimization.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwOptimization.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwOptimization.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwOptimization.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwOptimization.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwOptimization.ShowEportContextMenu = true;
            this.dgvwOptimization.Size = new System.Drawing.Size(401, 225);
            this.dgvwOptimization.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwOptimization.TabIndex = 25;
            this.dgvwOptimization.ToPrintCols = null;
            this.dgvwOptimization.ToPrintRows = null;
            this.dgvwOptimization.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvwOptimization_CellValidating);
            this.dgvwOptimization.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvwOptimization_DataError);
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(195, 281);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(76, 25);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 26;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // btWOptimizeTo
            // 
            this.btWOptimizeTo.bSilver = false;
            this.btWOptimizeTo.Location = new System.Drawing.Point(521, 281);
            this.btWOptimizeTo.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWOptimizeTo.MenuPos = new System.Drawing.Point(0, 0);
            this.btWOptimizeTo.Name = "btWOptimizeTo";
            this.btWOptimizeTo.Size = new System.Drawing.Size(75, 25);
            this.btWOptimizeTo.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWOptimizeTo.TabIndex = 27;
            this.btWOptimizeTo.Text = "下一步";
            this.btWOptimizeTo.ToFocused = false;
            this.btWOptimizeTo.UseVisualStyleBackColor = true;
            this.btWOptimizeTo.Click += new System.EventHandler(this.btWOptimizeTo_Click);
            // 
            // btWUp
            // 
            this.btWUp.bSilver = false;
            this.btWUp.Location = new System.Drawing.Point(440, 282);
            this.btWUp.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWUp.MenuPos = new System.Drawing.Point(0, 0);
            this.btWUp.Name = "btWUp";
            this.btWUp.Size = new System.Drawing.Size(75, 25);
            this.btWUp.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWUp.TabIndex = 28;
            this.btWUp.Text = "上一步";
            this.btWUp.ToFocused = false;
            this.btWUp.UseVisualStyleBackColor = true;
            this.btWUp.Click += new System.EventHandler(this.btWUp_Click);
            // 
            // comBoxOptimizationType
            // 
            this.comBoxOptimizationType.AutoComplete = false;
            this.comBoxOptimizationType.AutoDropdown = false;
            this.comBoxOptimizationType.BackColorEven = System.Drawing.Color.White;
            this.comBoxOptimizationType.BackColorOdd = System.Drawing.Color.White;
            this.comBoxOptimizationType.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.comBoxOptimizationType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.comBoxOptimizationType.ColumnNames = "";
            this.comBoxOptimizationType.ColumnWidthDefault = 75;
            this.comBoxOptimizationType.ColumnWidths = "";
            this.comBoxOptimizationType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comBoxOptimizationType.FormattingEnabled = true;
            this.comBoxOptimizationType.LinkedColumnIndex = 0;
            this.comBoxOptimizationType.LinkedTextBox = null;
            this.comBoxOptimizationType.Location = new System.Drawing.Point(8, 11);
            this.comBoxOptimizationType.Name = "comBoxOptimizationType";
            this.comBoxOptimizationType.Size = new System.Drawing.Size(121, 21);
            this.comBoxOptimizationType.TabIndex = 29;
            this.comBoxOptimizationType.SelectedIndexChanged += new System.EventHandler(this.comBoxOptimizationType_SelectedIndexChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "OptimiztionValue";
            this.dataGridViewTextBoxColumn1.HeaderText = "值";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 90;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "OptimiztionRange";
            this.dataGridViewTextBoxColumn2.HeaderText = "范围(%)";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "OptimiztionFactor";
            this.dataGridViewTextBoxColumn3.HeaderText = "因子";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // OptimiztionValue
            // 
            this.OptimiztionValue.DataPropertyName = "OptimiztionValue";
            this.OptimiztionValue.HeaderText = "值";
            this.OptimiztionValue.Name = "OptimiztionValue";
            this.OptimiztionValue.Width = 90;
            // 
            // OptExpression
            // 
            this.OptExpression.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OptExpression.DataPropertyName = "OptExpression";
            this.OptExpression.HeaderText = "公式";
            this.OptExpression.Name = "OptExpression";
            this.OptExpression.Visible = false;
            // 
            // OptimiztionMin
            // 
            this.OptimiztionMin.DataPropertyName = "OptimiztionMin";
            this.OptimiztionMin.HeaderText = "范围-";
            this.OptimiztionMin.Name = "OptimiztionMin";
            // 
            // OptimiztionMax
            // 
            this.OptimiztionMax.DataPropertyName = "OptimiztionMax";
            this.OptimiztionMax.HeaderText = "范围+";
            this.OptimiztionMax.Name = "OptimiztionMax";
            // 
            // OptimiztionRange
            // 
            this.OptimiztionRange.DataPropertyName = "OptimiztionRange";
            this.OptimiztionRange.HeaderText = "范围(%)";
            this.OptimiztionRange.Name = "OptimiztionRange";
            this.OptimiztionRange.Visible = false;
            // 
            // OptimiztionFactor
            // 
            this.OptimiztionFactor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OptimiztionFactor.DataPropertyName = "OptimiztionFactor";
            this.OptimiztionFactor.HeaderText = "因子";
            this.OptimiztionFactor.Name = "OptimiztionFactor";
            // 
            // IsJoinIntensity
            // 
            this.IsJoinIntensity.DataPropertyName = "IsJoinIntensity";
            this.IsJoinIntensity.HeaderText = "介入";
            this.IsJoinIntensity.Name = "IsJoinIntensity";
            this.IsJoinIntensity.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsJoinIntensity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsJoinIntensity.Visible = false;
            this.IsJoinIntensity.Width = 60;
            // 
            // UCOptimization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.comBoxOptimizationType);
            this.Controls.Add(this.btWUp);
            this.Controls.Add(this.btWOptimizeTo);
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.dgvwOptimization);
            this.Controls.Add(this.lbwOptimization);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnAdd);
            this.Name = "UCOptimization";
            this.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Size = new System.Drawing.Size(607, 317);
            this.Load += new System.EventHandler(this.UCOptimization_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvwOptimization)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ListBoxW lbwOptimization;
        private Skyray.Controls.DataGridViewW dgvwOptimization;
        private Skyray.Controls.ButtonW btnApplication;
        private Skyray.Controls.ButtonW btWOptimizeTo;
        private Skyray.Controls.ButtonW btWUp;
        private Skyray.Controls.ComboBoxW comBoxOptimizationType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptimiztionValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptExpression;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptimiztionMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptimiztionMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptimiztionRange;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptimiztionFactor;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsJoinIntensity;
    }
}
