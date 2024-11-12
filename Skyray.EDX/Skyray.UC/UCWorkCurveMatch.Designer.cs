namespace Skyray.UC
{
    partial class UCWorkCurveMatch
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
            Skyray.Controls.Office2007Renderer office2007Renderer1 = new Skyray.Controls.Office2007Renderer();
            Skyray.Controls.Office2007Renderer office2007Renderer2 = new Skyray.Controls.Office2007Renderer();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tlsNewMatch = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsRemoveMatch = new System.Windows.Forms.ToolStripMenuItem();
            this.containerObject2 = new Skyray.EDX.Common.ContainerObject();
            this.dgvMatchData = new Skyray.Controls.DataGridViewW();
            this.containerObject1 = new Skyray.EDX.Common.ContainerObject();
            this.chBSelectTotal = new Skyray.Controls.CheckBoxW();
            this.btnUp = new Skyray.Controls.ButtonW();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnSubmit = new Skyray.Controls.ButtonW();
            this.ColMatch = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColMatchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.containerObject2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatchData)).BeginInit();
            this.containerObject1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsNewMatch,
            this.tlsRemoveMatch});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 48);
            // 
            // tlsNewMatch
            // 
            this.tlsNewMatch.Name = "tlsNewMatch";
            this.tlsNewMatch.Size = new System.Drawing.Size(94, 22);
            this.tlsNewMatch.Text = "新建";
            this.tlsNewMatch.Click += new System.EventHandler(this.tlsNewMatch_Click);
            // 
            // tlsRemoveMatch
            // 
            this.tlsRemoveMatch.Name = "tlsRemoveMatch";
            this.tlsRemoveMatch.Size = new System.Drawing.Size(94, 22);
            this.tlsRemoveMatch.Text = "移除";
            this.tlsRemoveMatch.Click += new System.EventHandler(this.tlsRemoveMatch_Click);
            // 
            // containerObject2
            // 
            this.containerObject2.AutoScroll = true;
            this.containerObject2.BigImage = null;
            this.containerObject2.ContainerAttribute = false;
            this.containerObject2.ContainerLabel = null;
            this.containerObject2.ControlInternal = 0;
            this.containerObject2.Controls.Add(this.dgvMatchData);
            this.containerObject2.CurrentPanelIndex = 0;
            this.containerObject2.CurrentPlanningNumber = 0;
            this.containerObject2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerObject2.IncludeInnerCoordinate = false;
            this.containerObject2.IsReverseEmbeded = false;
            this.containerObject2.Location = new System.Drawing.Point(8, 8);
            this.containerObject2.Name = "containerObject2";
            this.containerObject2.Name1 = null;
            office2007Renderer1.RoundedEdges = true;
            this.containerObject2.Renderer = office2007Renderer1;
            this.containerObject2.Size = new System.Drawing.Size(415, 363);
            this.containerObject2.SmallImage = null;
            this.containerObject2.Style = Skyray.Controls.Style.Custom;
            this.containerObject2.TabIndex = 2;
            // 
            // dgvMatchData
            // 
            this.dgvMatchData.AllowUserToAddRows = false;
            this.dgvMatchData.AllowUserToResizeRows = false;
            this.dgvMatchData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvMatchData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMatchData.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvMatchData.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvMatchData.ColumnHeadersHeight = 20;
            this.dgvMatchData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMatchData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColMatch,
            this.ColMatchName});
            this.dgvMatchData.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvMatchData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMatchData.Location = new System.Drawing.Point(0, 0);
            this.dgvMatchData.Name = "dgvMatchData";
            this.dgvMatchData.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvMatchData.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvMatchData.RowHeadersVisible = false;
            this.dgvMatchData.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvMatchData.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvMatchData.RowTemplate.Height = 23;
            this.dgvMatchData.SecondaryLength = 1;
            this.dgvMatchData.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvMatchData.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvMatchData.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvMatchData.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvMatchData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMatchData.ShowEportContextMenu = false;
            this.dgvMatchData.Size = new System.Drawing.Size(415, 363);
            this.dgvMatchData.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvMatchData.TabIndex = 0;
            this.dgvMatchData.ToPrintCols = null;
            this.dgvMatchData.ToPrintRows = null;
            this.dgvMatchData.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvMatchData_CellPainting);
            this.dgvMatchData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMatchData_CellContentClick);
            // 
            // containerObject1
            // 
            this.containerObject1.AutoScroll = true;
            this.containerObject1.BigImage = null;
            this.containerObject1.ContainerAttribute = false;
            this.containerObject1.ContainerLabel = null;
            this.containerObject1.ControlInternal = 0;
            this.containerObject1.Controls.Add(this.chBSelectTotal);
            this.containerObject1.Controls.Add(this.btnUp);
            this.containerObject1.Controls.Add(this.btnApplication);
            this.containerObject1.Controls.Add(this.btnCancel);
            this.containerObject1.Controls.Add(this.btnSubmit);
            this.containerObject1.CurrentPanelIndex = 0;
            this.containerObject1.CurrentPlanningNumber = 0;
            this.containerObject1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.containerObject1.IncludeInnerCoordinate = false;
            this.containerObject1.IsReverseEmbeded = false;
            this.containerObject1.Location = new System.Drawing.Point(8, 371);
            this.containerObject1.Name = "containerObject1";
            this.containerObject1.Name1 = null;
            office2007Renderer2.RoundedEdges = true;
            this.containerObject1.Renderer = office2007Renderer2;
            this.containerObject1.Size = new System.Drawing.Size(415, 36);
            this.containerObject1.SmallImage = null;
            this.containerObject1.Style = Skyray.Controls.Style.Custom;
            this.containerObject1.TabIndex = 1;
            // 
            // chBSelectTotal
            // 
            this.chBSelectTotal.AutoSize = true;
            this.chBSelectTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.chBSelectTotal.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chBSelectTotal.Location = new System.Drawing.Point(3, 11);
            this.chBSelectTotal.Name = "chBSelectTotal";
            this.chBSelectTotal.Size = new System.Drawing.Size(84, 16);
            this.chBSelectTotal.Style = Skyray.Controls.Style.Office2007Blue;
            this.chBSelectTotal.TabIndex = 4;
            this.chBSelectTotal.Text = "全选匹配谱";
            this.chBSelectTotal.UseVisualStyleBackColor = false;
            this.chBSelectTotal.Click += new System.EventHandler(this.chBSelectTotal_Click);
            // 
            // btnUp
            // 
            this.btnUp.bSilver = false;
            this.btnUp.Location = new System.Drawing.Point(336, 7);
            this.btnUp.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnUp.MenuPos = new System.Drawing.Point(0, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "上一步";
            this.btnUp.ToFocused = false;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(93, 7);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(75, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 2;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(255, 7);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.bSilver = false;
            this.btnSubmit.Location = new System.Drawing.Point(174, 7);
            this.btnSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSubmit.TabIndex = 0;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.ToFocused = false;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // ColMatch
            // 
            this.ColMatch.HeaderText = "是否匹配";
            this.ColMatch.Name = "ColMatch";
            // 
            // ColMatchName
            // 
            this.ColMatchName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColMatchName.HeaderText = "匹配谱";
            this.ColMatchName.Name = "ColMatchName";
            this.ColMatchName.ReadOnly = true;
            // 
            // UCWorkCurveMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.containerObject2);
            this.Controls.Add(this.containerObject1);
            this.Name = "UCWorkCurveMatch";
            this.Size = new System.Drawing.Size(431, 415);
            this.Load += new System.EventHandler(this.UCWorkCurveMatch_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.containerObject2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatchData)).EndInit();
            this.containerObject1.ResumeLayout(false);
            this.containerObject1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dgvMatchData;
        private Skyray.EDX.Common.ContainerObject containerObject1;
        private Skyray.EDX.Common.ContainerObject containerObject2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tlsNewMatch;
        private System.Windows.Forms.ToolStripMenuItem tlsRemoveMatch;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnSubmit;
        private Skyray.Controls.ButtonW btnApplication;
        private Skyray.Controls.ButtonW btnUp;
        private Skyray.Controls.CheckBoxW chBSelectTotal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColMatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMatchName;
    }
}
