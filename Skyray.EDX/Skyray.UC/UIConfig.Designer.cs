namespace Skyray.UC
{
    partial class UIConfig
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
            this.dataGridViewWContainObjects = new Skyray.Controls.DataGridViewW();
            this.ColObjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.containerObject1 = new Skyray.EDX.Common.ContainerObject();
            this.grb_ChildControl = new System.Windows.Forms.GroupBox();
            this.chkVisible = new Skyray.Controls.CheckBoxW();
            this.btnSet = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWContainObjects)).BeginInit();
            this.containerObject1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewWContainObjects
            // 
            this.dataGridViewWContainObjects.AllowUserToAddRows = false;
            this.dataGridViewWContainObjects.AllowUserToDeleteRows = false;
            this.dataGridViewWContainObjects.AllowUserToOrderColumns = true;
            this.dataGridViewWContainObjects.AllowUserToResizeRows = false;
            this.dataGridViewWContainObjects.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dataGridViewWContainObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewWContainObjects.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dataGridViewWContainObjects.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dataGridViewWContainObjects.ColumnHeadersHeight = 20;
            this.dataGridViewWContainObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWContainObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColObjectName});
            this.dataGridViewWContainObjects.Location = new System.Drawing.Point(11, 11);
            this.dataGridViewWContainObjects.Name = "dataGridViewWContainObjects";
            this.dataGridViewWContainObjects.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dataGridViewWContainObjects.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dataGridViewWContainObjects.ReadOnly = true;
            this.dataGridViewWContainObjects.RowHeadersVisible = false;
            this.dataGridViewWContainObjects.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dataGridViewWContainObjects.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewWContainObjects.RowTemplate.Height = 23;
            this.dataGridViewWContainObjects.SecondaryLength = 1;
            this.dataGridViewWContainObjects.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dataGridViewWContainObjects.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dataGridViewWContainObjects.SelectedRowColor1 = System.Drawing.Color.White;
            this.dataGridViewWContainObjects.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dataGridViewWContainObjects.ShowEportContextMenu = false;
            this.dataGridViewWContainObjects.Size = new System.Drawing.Size(249, 313);
            this.dataGridViewWContainObjects.Style = Skyray.Controls.Style.Office2007Blue;
            this.dataGridViewWContainObjects.TabIndex = 0;
            this.dataGridViewWContainObjects.ToPrintCols = null;
            this.dataGridViewWContainObjects.ToPrintRows = null;
            this.dataGridViewWContainObjects.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewWContainObjects_CellClick);
            // 
            // ColObjectName
            // 
            this.ColObjectName.DataPropertyName = "Name1";
            this.ColObjectName.HeaderText = "容器名";
            this.ColObjectName.Name = "ColObjectName";
            this.ColObjectName.ReadOnly = true;
            this.ColObjectName.Width = 220;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "容器名";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 220;
            // 
            // containerObject1
            // 
            this.containerObject1.AutoScroll = true;
            this.containerObject1.BigImage = null;
            this.containerObject1.ContainerAttribute = false;
            this.containerObject1.ContainerLabel = null;
            this.containerObject1.ControlInternal = 0;
            this.containerObject1.Controls.Add(this.grb_ChildControl);
            this.containerObject1.Controls.Add(this.chkVisible);
            this.containerObject1.Controls.Add(this.btnSet);
            this.containerObject1.Controls.Add(this.btnCancel);
            this.containerObject1.Controls.Add(this.btnOK);
            this.containerObject1.CurrentPanelIndex = 0;
            this.containerObject1.CurrentPlanningNumber = 0;
            this.containerObject1.IncludeInnerCoordinate = false;
            this.containerObject1.IsReverseEmbeded = false;
            this.containerObject1.Location = new System.Drawing.Point(266, 11);
            this.containerObject1.Name = "containerObject1";
            this.containerObject1.Name1 = "containerObject1";
            office2007Renderer1.RoundedEdges = true;
            this.containerObject1.Renderer = office2007Renderer1;
            this.containerObject1.Size = new System.Drawing.Size(126, 313);
            this.containerObject1.SmallImage = null;
            this.containerObject1.Style = Skyray.Controls.Style.Custom;
            this.containerObject1.TabIndex = 5;
            // 
            // grb_ChildControl
            // 
            this.grb_ChildControl.BackColor = System.Drawing.Color.Transparent;
            this.grb_ChildControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grb_ChildControl.Location = new System.Drawing.Point(5, 3);
            this.grb_ChildControl.Name = "grb_ChildControl";
            this.grb_ChildControl.Size = new System.Drawing.Size(114, 166);
            this.grb_ChildControl.TabIndex = 5;
            this.grb_ChildControl.TabStop = false;
            this.grb_ChildControl.Text = "明细";
            // 
            // chkVisible
            // 
            this.chkVisible.AutoSize = true;
            this.chkVisible.BackColor = System.Drawing.Color.Transparent;
            this.chkVisible.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkVisible.Location = new System.Drawing.Point(34, 175);
            this.chkVisible.Name = "chkVisible";
            this.chkVisible.Size = new System.Drawing.Size(48, 16);
            this.chkVisible.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkVisible.TabIndex = 1;
            this.chkVisible.Text = "可见";
            this.chkVisible.UseVisualStyleBackColor = false;
            // 
            // btnSet
            // 
            this.btnSet.bSilver = false;
            this.btnSet.Location = new System.Drawing.Point(22, 207);
            this.btnSet.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSet.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(85, 23);
            this.btnSet.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSet.TabIndex = 4;
            this.btnSet.Text = "设置";
            this.btnSet.ToFocused = false;
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(22, 236);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(22, 265);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // UIConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.containerObject1);
            this.Controls.Add(this.dataGridViewWContainObjects);
            this.Name = "UIConfig";
            this.Size = new System.Drawing.Size(400, 340);
            this.Load += new System.EventHandler(this.UIConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWContainObjects)).EndInit();
            this.containerObject1.ResumeLayout(false);
            this.containerObject1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dataGridViewWContainObjects;
        private Skyray.Controls.CheckBoxW chkVisible;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnSet;
        private Skyray.EDX.Common.ContainerObject containerObject1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColObjectName;
        private System.Windows.Forms.GroupBox grb_ChildControl;
    }
}
