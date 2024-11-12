namespace Skyray.UC
{
    partial class UCHistoryRecordContinuousList
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
            this.components = new System.ComponentModel.Container();
            this.textBoxWInputName = new Skyray.Controls.TextBoxW();
            this.buttonWSearch = new Skyray.Controls.ButtonW();
            this.dataGridViewW1 = new Skyray.Controls.DataGridViewW();
            this.lbHistorylLine = new Skyray.Controls.LabelW();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.butOK = new Skyray.Controls.ButtonW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.btWJoinData = new Skyray.Controls.ButtonW();
            this.checkLuSu = new Skyray.Controls.CheckBoxW();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxWInputName
            // 
            this.textBoxWInputName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textBoxWInputName.Location = new System.Drawing.Point(116, 11);
            this.textBoxWInputName.Name = "textBoxWInputName";
            this.textBoxWInputName.Size = new System.Drawing.Size(120, 21);
            this.textBoxWInputName.Style = Skyray.Controls.Style.Office2007Blue;
            this.textBoxWInputName.TabIndex = 6;
            // 
            // buttonWSearch
            // 
            this.buttonWSearch.bSilver = false;
            this.buttonWSearch.Location = new System.Drawing.Point(256, 11);
            this.buttonWSearch.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWSearch.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWSearch.Name = "buttonWSearch";
            this.buttonWSearch.Size = new System.Drawing.Size(60, 23);
            this.buttonWSearch.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWSearch.TabIndex = 2;
            this.buttonWSearch.Text = "查 询";
            this.buttonWSearch.ToFocused = false;
            this.buttonWSearch.UseVisualStyleBackColor = true;
            this.buttonWSearch.Click += new System.EventHandler(this.buttonWSearch_Click_1);
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
            this.dataGridViewW1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewW1.Location = new System.Drawing.Point(12, 45);
            this.dataGridViewW1.MultiSelect = false;
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
            this.dataGridViewW1.Size = new System.Drawing.Size(628, 318);
            this.dataGridViewW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.dataGridViewW1.TabIndex = 0;
            this.dataGridViewW1.ToPrintCols = null;
            this.dataGridViewW1.ToPrintRows = null;
            this.dataGridViewW1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewW1_MouseClick);
            // 
            // lbHistorylLine
            // 
            this.lbHistorylLine.AutoSize = true;
            this.lbHistorylLine.BackColor = System.Drawing.Color.Transparent;
            this.lbHistorylLine.Location = new System.Drawing.Point(19, 15);
            this.lbHistorylLine.Name = "lbHistorylLine";
            this.lbHistorylLine.Size = new System.Drawing.Size(65, 12);
            this.lbHistorylLine.TabIndex = 7;
            this.lbHistorylLine.Text = "按名称查询";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // butOK
            // 
            this.butOK.bSilver = false;
            this.butOK.Location = new System.Drawing.Point(473, 11);
            this.butOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.butOK.MenuPos = new System.Drawing.Point(0, 0);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(60, 23);
            this.butOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.butOK.TabIndex = 8;
            this.butOK.Text = "打 印";
            this.butOK.ToFocused = false;
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(545, 11);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(60, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "取消";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btWJoinData
            // 
            this.btWJoinData.bSilver = false;
            this.btWJoinData.Location = new System.Drawing.Point(393, 11);
            this.btWJoinData.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWJoinData.MenuPos = new System.Drawing.Point(0, 0);
            this.btWJoinData.Name = "btWJoinData";
            this.btWJoinData.Size = new System.Drawing.Size(70, 23);
            this.btWJoinData.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWJoinData.TabIndex = 10;
            this.btWJoinData.Text = "数据源";
            this.btWJoinData.ToFocused = false;
            this.btWJoinData.UseVisualStyleBackColor = true;
            this.btWJoinData.Visible = false;
            this.btWJoinData.Click += new System.EventHandler(this.btWJoinData_Click);
            // 
            // checkLuSu
            // 
            this.checkLuSu.AutoSize = true;
            this.checkLuSu.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.checkLuSu.Location = new System.Drawing.Point(340, 15);
            this.checkLuSu.Name = "checkLuSu";
            this.checkLuSu.Size = new System.Drawing.Size(48, 16);
            this.checkLuSu.Style = Skyray.Controls.Style.Office2007Blue;
            this.checkLuSu.TabIndex = 11;
            this.checkLuSu.Text = "卤素";
            this.checkLuSu.UseVisualStyleBackColor = true;
            // 
            // UCHistoryRecordContinuousList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.checkLuSu);
            this.Controls.Add(this.btWJoinData);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbHistorylLine);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.dataGridViewW1);
            this.Controls.Add(this.textBoxWInputName);
            this.Controls.Add(this.buttonWSearch);
            this.Name = "UCHistoryRecordContinuousList";
            this.Size = new System.Drawing.Size(651, 379);
            this.Load += new System.EventHandler(this.UCHistoryRecordContinuousList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewW1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.DataGridViewW dataGridViewW1;
        private Skyray.Controls.ButtonW buttonWSearch;
        private Skyray.Controls.TextBoxW textBoxWInputName;
        private Skyray.Controls.LabelW lbHistorylLine;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private Skyray.Controls.ButtonW butOK;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.ButtonW btWJoinData;
        private Skyray.Controls.CheckBoxW checkLuSu;
    }
}
