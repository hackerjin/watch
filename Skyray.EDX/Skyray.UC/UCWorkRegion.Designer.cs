namespace Skyray.UC
{
    partial class UCWorkRegion
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
            this.dataViewWorkregion = new Skyray.Controls.DataGridViewW();
            this.ColumnWorkRegion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControlW1 = new Skyray.Controls.TabControlW();
            this.tbPageWorkregion = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.dataViewWorkregion)).BeginInit();
            this.tabControlW1.SuspendLayout();
            this.tbPageWorkregion.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewWorkregion
            // 
            this.dataViewWorkregion.AllowUserToAddRows = false;
            this.dataViewWorkregion.AllowUserToDeleteRows = false;
            this.dataViewWorkregion.AllowUserToResizeRows = false;
            this.dataViewWorkregion.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dataViewWorkregion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataViewWorkregion.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dataViewWorkregion.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dataViewWorkregion.ColumnHeadersHeight = 4;
            this.dataViewWorkregion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataViewWorkregion.ColumnHeadersVisible = false;
            this.dataViewWorkregion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnWorkRegion});
            this.dataViewWorkregion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewWorkregion.Location = new System.Drawing.Point(3, 3);
            this.dataViewWorkregion.Name = "dataViewWorkregion";
            this.dataViewWorkregion.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dataViewWorkregion.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dataViewWorkregion.ReadOnly = true;
            this.dataViewWorkregion.RowHeadersVisible = false;
            this.dataViewWorkregion.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dataViewWorkregion.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataViewWorkregion.RowTemplate.Height = 23;
            this.dataViewWorkregion.SecondaryLength = 1;
            this.dataViewWorkregion.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dataViewWorkregion.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dataViewWorkregion.SelectedRowColor1 = System.Drawing.Color.White;
            this.dataViewWorkregion.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dataViewWorkregion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataViewWorkregion.ShowEportContextMenu = false;
            this.dataViewWorkregion.Size = new System.Drawing.Size(269, 222);
            this.dataViewWorkregion.Style = Skyray.Controls.Style.Office2007Blue;
            this.dataViewWorkregion.TabIndex = 0;
            this.dataViewWorkregion.ToPrintCols = null;
            this.dataViewWorkregion.ToPrintRows = null;
            this.dataViewWorkregion.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataViewWorkregion_CellClick);
            // 
            // ColumnWorkRegion
            // 
            this.ColumnWorkRegion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnWorkRegion.HeaderText = "Column1";
            this.ColumnWorkRegion.Name = "ColumnWorkRegion";
            this.ColumnWorkRegion.ReadOnly = true;
            // 
            // tabControlW1
            // 
            this.tabControlW1.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabControlW1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.tabControlW1.Controls.Add(this.tbPageWorkregion);
            this.tabControlW1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlW1.Location = new System.Drawing.Point(0, 0);
            this.tabControlW1.Name = "tabControlW1";
            this.tabControlW1.SelectedIndex = 0;
            this.tabControlW1.ShowTabs = true;
            this.tabControlW1.Size = new System.Drawing.Size(283, 257);
            this.tabControlW1.Style = Skyray.Controls.Style.Office2007Blue;
            this.tabControlW1.TabIndex = 1;
            // 
            // tbPageWorkregion
            // 
            this.tbPageWorkregion.Controls.Add(this.dataViewWorkregion);
            this.tbPageWorkregion.Location = new System.Drawing.Point(4, 25);
            this.tbPageWorkregion.Name = "tbPageWorkregion";
            this.tbPageWorkregion.Padding = new System.Windows.Forms.Padding(3);
            this.tbPageWorkregion.Size = new System.Drawing.Size(275, 228);
            this.tbPageWorkregion.TabIndex = 0;
            this.tbPageWorkregion.Text = "工作区";
            this.tbPageWorkregion.UseVisualStyleBackColor = true;
            // 
            // UCWorkRegion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlW1);
            this.Name = "UCWorkRegion";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.Size = new System.Drawing.Size(283, 257);
            this.Load += new System.EventHandler(this.UCWorkRegion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataViewWorkregion)).EndInit();
            this.tabControlW1.ResumeLayout(false);
            this.tbPageWorkregion.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dataViewWorkregion;
        private Skyray.Controls.TabControlW tabControlW1;
        private System.Windows.Forms.TabPage tbPageWorkregion;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWorkRegion;
    }
}
