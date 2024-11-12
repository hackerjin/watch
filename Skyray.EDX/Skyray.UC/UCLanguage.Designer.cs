namespace Skyray.UC
{
    partial class UCLanguage
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
            Skyray.Controls.Office2007Renderer office2007Renderer1 = new Skyray.Controls.Office2007Renderer();
            Skyray.Controls.Office2007Renderer office2007Renderer2 = new Skyray.Controls.Office2007Renderer();
            Skyray.Controls.Office2007Renderer office2007Renderer3 = new Skyray.Controls.Office2007Renderer();
            this.dgvLang = new Skyray.Controls.DataGridViewW();
            this.btnAddLang = new Skyray.Controls.ButtonW();
            this.btnSaveLang = new Skyray.Controls.ButtonW();
            this.btnDelLang = new Skyray.Controls.ButtonW();
            this.txtLangName = new Skyray.Controls.TextBoxW();
            this.lblTranslate = new Skyray.Controls.ButtonW();
            this.btnTranslateSelectRow = new Skyray.Controls.ButtonW();
            this.btnImportData = new Skyray.Controls.ButtonW();
            this.containerObject1 = new Skyray.EDX.Common.ContainerObject();
            this.containerObject2 = new Skyray.EDX.Common.ContainerObject();
            this.btWInputExcel = new Skyray.Controls.ButtonW();
            this.btwExcel = new Skyray.Controls.ButtonW();
            this.txtBoxContext = new Skyray.Controls.TextBoxW();
            this.btWSearch = new Skyray.Controls.ButtonW();
            this.containerObject4 = new Skyray.EDX.Common.ContainerObject();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLang)).BeginInit();
            this.containerObject1.SuspendLayout();
            this.containerObject2.SuspendLayout();
            this.containerObject4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvLang
            // 
            this.dgvLang.AllowUserToAddRows = false;
            this.dgvLang.AllowUserToDeleteRows = false;
            this.dgvLang.AllowUserToResizeRows = false;
            this.dgvLang.BackgroundColor = System.Drawing.Color.White;
            this.dgvLang.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvLang.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvLang.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvLang.ColumnHeadersHeight = 24;
            this.dgvLang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLang.Location = new System.Drawing.Point(0, 0);
            this.dgvLang.Name = "dgvLang";
            this.dgvLang.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvLang.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvLang.RowHeadersVisible = false;
            this.dgvLang.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvLang.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvLang.RowTemplate.Height = 23;
            this.dgvLang.SecondaryLength = 1;
            this.dgvLang.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvLang.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvLang.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvLang.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvLang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect;
            this.dgvLang.ShowEportContextMenu = true;
            this.dgvLang.Size = new System.Drawing.Size(377, 637);
            this.dgvLang.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvLang.TabIndex = 0;
            this.dgvLang.ToPrintCols = null;
            this.dgvLang.ToPrintRows = null;
            // 
            // btnAddLang
            // 
            this.btnAddLang.bSilver = false;
            this.btnAddLang.Location = new System.Drawing.Point(66, 91);
            this.btnAddLang.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAddLang.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAddLang.Name = "btnAddLang";
            this.btnAddLang.Size = new System.Drawing.Size(115, 23);
            this.btnAddLang.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAddLang.TabIndex = 1;
            this.btnAddLang.Text = "添加";
            this.btnAddLang.ToFocused = false;
            this.btnAddLang.UseVisualStyleBackColor = true;
            this.btnAddLang.Click += new System.EventHandler(this.btnAddLang_Click);
            // 
            // btnSaveLang
            // 
            this.btnSaveLang.bSilver = false;
            this.btnSaveLang.Location = new System.Drawing.Point(66, 584);
            this.btnSaveLang.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSaveLang.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSaveLang.Name = "btnSaveLang";
            this.btnSaveLang.Size = new System.Drawing.Size(120, 23);
            this.btnSaveLang.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSaveLang.TabIndex = 2;
            this.btnSaveLang.Text = "保存";
            this.btnSaveLang.ToFocused = false;
            this.btnSaveLang.UseVisualStyleBackColor = true;
            this.btnSaveLang.Click += new System.EventHandler(this.btnSaveLang_Click);
            // 
            // btnDelLang
            // 
            this.btnDelLang.bSilver = false;
            this.btnDelLang.Location = new System.Drawing.Point(65, 141);
            this.btnDelLang.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDelLang.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDelLang.Name = "btnDelLang";
            this.btnDelLang.Size = new System.Drawing.Size(115, 23);
            this.btnDelLang.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDelLang.TabIndex = 3;
            this.btnDelLang.Text = "删除";
            this.btnDelLang.ToFocused = false;
            this.btnDelLang.UseVisualStyleBackColor = true;
            this.btnDelLang.Click += new System.EventHandler(this.btnDelLang_Click);
            // 
            // txtLangName
            // 
            this.txtLangName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtLangName.Location = new System.Drawing.Point(66, 42);
            this.txtLangName.Name = "txtLangName";
            this.txtLangName.Size = new System.Drawing.Size(115, 21);
            this.txtLangName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtLangName.TabIndex = 4;
            // 
            // lblTranslate
            // 
            this.lblTranslate.bSilver = false;
            this.lblTranslate.Location = new System.Drawing.Point(66, 304);
            this.lblTranslate.MaxImageSize = new System.Drawing.Point(0, 0);
            this.lblTranslate.MenuPos = new System.Drawing.Point(0, 0);
            this.lblTranslate.Name = "lblTranslate";
            this.lblTranslate.Size = new System.Drawing.Size(114, 23);
            this.lblTranslate.Style = Skyray.Controls.Style.Office2007Blue;
            this.lblTranslate.TabIndex = 5;
            this.lblTranslate.Text = "在线翻译";
            this.lblTranslate.ToFocused = false;
            this.lblTranslate.UseVisualStyleBackColor = true;
            this.lblTranslate.Visible = false;
            this.lblTranslate.Click += new System.EventHandler(this.lblTranslate_Click);
            // 
            // btnTranslateSelectRow
            // 
            this.btnTranslateSelectRow.bSilver = false;
            this.btnTranslateSelectRow.Location = new System.Drawing.Point(66, 197);
            this.btnTranslateSelectRow.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnTranslateSelectRow.MenuPos = new System.Drawing.Point(0, 0);
            this.btnTranslateSelectRow.Name = "btnTranslateSelectRow";
            this.btnTranslateSelectRow.Size = new System.Drawing.Size(114, 23);
            this.btnTranslateSelectRow.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnTranslateSelectRow.TabIndex = 6;
            this.btnTranslateSelectRow.Text = "选中行翻译";
            this.btnTranslateSelectRow.ToFocused = false;
            this.btnTranslateSelectRow.UseVisualStyleBackColor = true;
            this.btnTranslateSelectRow.Visible = false;
            this.btnTranslateSelectRow.Click += new System.EventHandler(this.btnTranslateSelectRow_Click);
            // 
            // btnImportData
            // 
            this.btnImportData.bSilver = false;
            this.btnImportData.Location = new System.Drawing.Point(66, 250);
            this.btnImportData.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnImportData.MenuPos = new System.Drawing.Point(0, 0);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(114, 23);
            this.btnImportData.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnImportData.TabIndex = 6;
            this.btnImportData.Text = "数据导入";
            this.btnImportData.ToFocused = false;
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Visible = false;
            this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
            // 
            // containerObject1
            // 
            this.containerObject1.AutoScroll = true;
            this.containerObject1.BigImage = null;
            this.containerObject1.ContainerAttribute = false;
            this.containerObject1.ContainerLabel = null;
            this.containerObject1.ControlInternal = 0;
            this.containerObject1.Controls.Add(this.dgvLang);
            this.containerObject1.CurrentPanelIndex = 0;
            this.containerObject1.CurrentPlanningNumber = 0;
            this.containerObject1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerObject1.IncludeInnerCoordinate = false;
            this.containerObject1.IsReverseEmbeded = false;
            this.containerObject1.Location = new System.Drawing.Point(0, 0);
            this.containerObject1.Name = "containerObject1";
            this.containerObject1.Name1 = null;
            office2007Renderer1.RoundedEdges = true;
            this.containerObject1.Renderer = office2007Renderer1;
            this.containerObject1.Size = new System.Drawing.Size(377, 637);
            this.containerObject1.SmallImage = null;
            this.containerObject1.Style = Skyray.Controls.Style.Custom;
            this.containerObject1.TabIndex = 7;
            // 
            // containerObject2
            // 
            this.containerObject2.AutoScroll = true;
            this.containerObject2.BigImage = null;
            this.containerObject2.ContainerAttribute = false;
            this.containerObject2.ContainerLabel = null;
            this.containerObject2.ControlInternal = 0;
            this.containerObject2.Controls.Add(this.btWInputExcel);
            this.containerObject2.Controls.Add(this.btwExcel);
            this.containerObject2.Controls.Add(this.txtBoxContext);
            this.containerObject2.Controls.Add(this.btWSearch);
            this.containerObject2.Controls.Add(this.btnSaveLang);
            this.containerObject2.Controls.Add(this.btnDelLang);
            this.containerObject2.Controls.Add(this.btnAddLang);
            this.containerObject2.Controls.Add(this.btnImportData);
            this.containerObject2.Controls.Add(this.txtLangName);
            this.containerObject2.Controls.Add(this.btnTranslateSelectRow);
            this.containerObject2.Controls.Add(this.lblTranslate);
            this.containerObject2.CurrentPanelIndex = 0;
            this.containerObject2.CurrentPlanningNumber = 0;
            this.containerObject2.Dock = System.Windows.Forms.DockStyle.Right;
            this.containerObject2.IncludeInnerCoordinate = false;
            this.containerObject2.IsReverseEmbeded = false;
            this.containerObject2.Location = new System.Drawing.Point(377, 0);
            this.containerObject2.Name = "containerObject2";
            this.containerObject2.Name1 = null;
            office2007Renderer2.RoundedEdges = true;
            this.containerObject2.Renderer = office2007Renderer2;
            this.containerObject2.Size = new System.Drawing.Size(215, 637);
            this.containerObject2.SmallImage = null;
            this.containerObject2.Style = Skyray.Controls.Style.Custom;
            this.containerObject2.TabIndex = 8;
            // 
            // btWInputExcel
            // 
            this.btWInputExcel.bSilver = false;
            this.btWInputExcel.Location = new System.Drawing.Point(66, 543);
            this.btWInputExcel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWInputExcel.MenuPos = new System.Drawing.Point(0, 0);
            this.btWInputExcel.Name = "btWInputExcel";
            this.btWInputExcel.Size = new System.Drawing.Size(120, 23);
            this.btWInputExcel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWInputExcel.TabIndex = 10;
            this.btWInputExcel.Text = "导入Excel";
            this.btWInputExcel.ToFocused = false;
            this.btWInputExcel.UseVisualStyleBackColor = true;
            this.btWInputExcel.Click += new System.EventHandler(this.btWInputExcel_Click);
            // 
            // btwExcel
            // 
            this.btwExcel.bSilver = false;
            this.btwExcel.Location = new System.Drawing.Point(66, 503);
            this.btwExcel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btwExcel.MenuPos = new System.Drawing.Point(0, 0);
            this.btwExcel.Name = "btwExcel";
            this.btwExcel.Size = new System.Drawing.Size(120, 23);
            this.btwExcel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btwExcel.TabIndex = 9;
            this.btwExcel.Text = "导出Excel";
            this.btwExcel.ToFocused = false;
            this.btwExcel.UseVisualStyleBackColor = true;
            this.btwExcel.Click += new System.EventHandler(this.btwExcel_Click);
            // 
            // txtBoxContext
            // 
            this.txtBoxContext.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtBoxContext.Location = new System.Drawing.Point(69, 389);
            this.txtBoxContext.Name = "txtBoxContext";
            this.txtBoxContext.Size = new System.Drawing.Size(112, 21);
            this.txtBoxContext.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtBoxContext.TabIndex = 8;
            // 
            // btWSearch
            // 
            this.btWSearch.bSilver = false;
            this.btWSearch.Location = new System.Drawing.Point(69, 432);
            this.btWSearch.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWSearch.MenuPos = new System.Drawing.Point(0, 0);
            this.btWSearch.Name = "btWSearch";
            this.btWSearch.Size = new System.Drawing.Size(114, 23);
            this.btWSearch.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWSearch.TabIndex = 7;
            this.btWSearch.Text = "查询";
            this.btWSearch.ToFocused = false;
            this.btWSearch.UseVisualStyleBackColor = true;
            this.btWSearch.Click += new System.EventHandler(this.btWSearch_Click);
            // 
            // containerObject4
            // 
            this.containerObject4.AutoScroll = true;
            this.containerObject4.BigImage = null;
            this.containerObject4.ContainerAttribute = false;
            this.containerObject4.ContainerLabel = null;
            this.containerObject4.ControlInternal = 0;
            this.containerObject4.Controls.Add(this.containerObject1);
            this.containerObject4.Controls.Add(this.containerObject2);
            this.containerObject4.CurrentPanelIndex = 0;
            this.containerObject4.CurrentPlanningNumber = 0;
            this.containerObject4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerObject4.IncludeInnerCoordinate = false;
            this.containerObject4.IsReverseEmbeded = false;
            this.containerObject4.Location = new System.Drawing.Point(8, 8);
            this.containerObject4.Name = "containerObject4";
            this.containerObject4.Name1 = null;
            office2007Renderer3.RoundedEdges = true;
            this.containerObject4.Renderer = office2007Renderer3;
            this.containerObject4.Size = new System.Drawing.Size(592, 637);
            this.containerObject4.SmallImage = null;
            this.containerObject4.Style = Skyray.Controls.Style.Custom;
            this.containerObject4.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Excel file (*.xls)|*.xls";
            // 
            // UCLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.containerObject4);
            this.Name = "UCLanguage";
            this.Size = new System.Drawing.Size(608, 653);
            this.Load += new System.EventHandler(this.UCLanguage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLang)).EndInit();
            this.containerObject1.ResumeLayout(false);
            this.containerObject2.ResumeLayout(false);
            this.containerObject2.PerformLayout();
            this.containerObject4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvLang;
        private Skyray.Controls.ButtonW btnAddLang;
        private Skyray.Controls.ButtonW btnSaveLang;
        private Skyray.Controls.ButtonW btnDelLang;
        private Skyray.Controls.TextBoxW txtLangName;
        private Skyray.Controls.ButtonW lblTranslate;
        private Skyray.Controls.ButtonW btnTranslateSelectRow;
        private Skyray.Controls.ButtonW btnImportData;
        private Skyray.EDX.Common.ContainerObject containerObject1;
        private Skyray.EDX.Common.ContainerObject containerObject2;
        private Skyray.EDX.Common.ContainerObject containerObject4;
        private Skyray.Controls.ButtonW btWSearch;
        private Skyray.Controls.TextBoxW txtBoxContext;
        private Skyray.Controls.ButtonW btwExcel;
        private Skyray.Controls.ButtonW btWInputExcel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
