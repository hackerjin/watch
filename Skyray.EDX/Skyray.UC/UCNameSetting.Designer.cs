namespace Skyray.UC
{
    partial class UCNameSetting
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
            this.CombinationNameLab = new System.Windows.Forms.Label();
            this.TBW_ShowText = new Skyray.Controls.TextBoxW();
            this.DownBtn = new Skyray.Controls.ButtonW();
            this.UpBtn = new Skyray.Controls.ButtonW();
            this.LB_ShowOrder = new Skyray.Controls.ListBoxW();
            this.DGV_NameList = new Skyray.Controls.DataGridViewW();
            this.NeedEditName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApplyBtn = new Skyray.Controls.ButtonW();
            this.grouper2 = new Skyray.Controls.Grouper();
            this.CBL_AllItems = new System.Windows.Forms.CheckedListBox();
            this.grouper1 = new Skyray.Controls.Grouper();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_NameList)).BeginInit();
            this.grouper2.SuspendLayout();
            this.grouper1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CombinationNameLab
            // 
            this.CombinationNameLab.AutoSize = true;
            this.CombinationNameLab.Location = new System.Drawing.Point(155, 9);
            this.CombinationNameLab.Name = "CombinationNameLab";
            this.CombinationNameLab.Size = new System.Drawing.Size(89, 12);
            this.CombinationNameLab.TabIndex = 1;
            this.CombinationNameLab.Text = "名称组合显示：";
            // 
            // TBW_ShowText
            // 
            this.TBW_ShowText.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.TBW_ShowText.Location = new System.Drawing.Point(153, 31);
            this.TBW_ShowText.Name = "TBW_ShowText";
            this.TBW_ShowText.Size = new System.Drawing.Size(469, 21);
            this.TBW_ShowText.Style = Skyray.Controls.Style.Office2007Blue;
            this.TBW_ShowText.TabIndex = 11;
            // 
            // DownBtn
            // 
            this.DownBtn.bSilver = false;
            this.DownBtn.Location = new System.Drawing.Point(177, 89);
            this.DownBtn.MaxImageSize = new System.Drawing.Point(0, 0);
            this.DownBtn.MenuPos = new System.Drawing.Point(0, 0);
            this.DownBtn.Name = "DownBtn";
            this.DownBtn.Size = new System.Drawing.Size(63, 23);
            this.DownBtn.Style = Skyray.Controls.Style.Office2007Blue;
            this.DownBtn.TabIndex = 9;
            this.DownBtn.Text = "∨";
            this.DownBtn.ToFocused = false;
            this.DownBtn.UseVisualStyleBackColor = true;
            this.DownBtn.Click += new System.EventHandler(this.DownBtn_Click);
            // 
            // UpBtn
            // 
            this.UpBtn.bSilver = false;
            this.UpBtn.Location = new System.Drawing.Point(177, 39);
            this.UpBtn.MaxImageSize = new System.Drawing.Point(0, 0);
            this.UpBtn.MenuPos = new System.Drawing.Point(0, 0);
            this.UpBtn.Name = "UpBtn";
            this.UpBtn.Size = new System.Drawing.Size(63, 24);
            this.UpBtn.Style = Skyray.Controls.Style.Office2007Blue;
            this.UpBtn.TabIndex = 8;
            this.UpBtn.Text = "∧";
            this.UpBtn.ToFocused = false;
            this.UpBtn.UseVisualStyleBackColor = true;
            this.UpBtn.Click += new System.EventHandler(this.UpBtn_Click);
            // 
            // LB_ShowOrder
            // 
            this.LB_ShowOrder.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.LB_ShowOrder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LB_ShowOrder.FormattingEnabled = true;
            this.LB_ShowOrder.HorizontalScrollbar = true;
            this.LB_ShowOrder.ItemHeight = 17;
            this.LB_ShowOrder.Location = new System.Drawing.Point(17, 21);
            this.LB_ShowOrder.Name = "LB_ShowOrder";
            this.LB_ShowOrder.Size = new System.Drawing.Size(144, 157);
            this.LB_ShowOrder.Style = Skyray.Controls.Style.Office2007Blue;
            this.LB_ShowOrder.TabIndex = 7;
            // 
            // DGV_NameList
            // 
            this.DGV_NameList.AllowUserToAddRows = false;
            this.DGV_NameList.AllowUserToDeleteRows = false;
            this.DGV_NameList.AllowUserToResizeRows = false;
            this.DGV_NameList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.DGV_NameList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_NameList.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.DGV_NameList.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.DGV_NameList.ColumnHeadersHeight = 20;
            this.DGV_NameList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_NameList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NeedEditName});
            this.DGV_NameList.Dock = System.Windows.Forms.DockStyle.Left;
            this.DGV_NameList.Location = new System.Drawing.Point(8, 8);
            this.DGV_NameList.Name = "DGV_NameList";
            this.DGV_NameList.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.DGV_NameList.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.DGV_NameList.ReadOnly = true;
            this.DGV_NameList.RowHeadersVisible = false;
            this.DGV_NameList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.DGV_NameList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.DGV_NameList.RowTemplate.Height = 23;
            this.DGV_NameList.SecondaryLength = 1;
            this.DGV_NameList.SecondaryRowColor1 = System.Drawing.Color.White;
            this.DGV_NameList.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.DGV_NameList.SelectedRowColor1 = System.Drawing.Color.White;
            this.DGV_NameList.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.DGV_NameList.ShowEportContextMenu = false;
            this.DGV_NameList.Size = new System.Drawing.Size(129, 262);
            this.DGV_NameList.Style = Skyray.Controls.Style.Office2007Blue;
            this.DGV_NameList.TabIndex = 0;
            this.DGV_NameList.ToPrintCols = null;
            this.DGV_NameList.ToPrintRows = null;
            this.DGV_NameList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_NameList_CellClick);
            // 
            // NeedEditName
            // 
            this.NeedEditName.HeaderText = "名字";
            this.NeedEditName.Name = "NeedEditName";
            this.NeedEditName.ReadOnly = true;
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.bSilver = false;
            this.ApplyBtn.Location = new System.Drawing.Point(177, 138);
            this.ApplyBtn.MaxImageSize = new System.Drawing.Point(0, 0);
            this.ApplyBtn.MenuPos = new System.Drawing.Point(0, 0);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(63, 23);
            this.ApplyBtn.Style = Skyray.Controls.Style.Office2007Blue;
            this.ApplyBtn.TabIndex = 12;
            this.ApplyBtn.Text = "应用";
            this.ApplyBtn.ToFocused = false;
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // grouper2
            // 
            this.grouper2.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouper2.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouper2.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouper2.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grouper2.BorderThickness = 1F;
            this.grouper2.BorderTopOnly = false;
            this.grouper2.Controls.Add(this.CBL_AllItems);
            this.grouper2.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grouper2.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grouper2.GroupImage = null;
            this.grouper2.GroupTitle = "";
            this.grouper2.HeaderRoundCorners = 4;
            this.grouper2.Location = new System.Drawing.Point(424, 63);
            this.grouper2.Name = "grouper2";
            this.grouper2.PaintGroupBox = false;
            this.grouper2.RoundCorners = 4;
            this.grouper2.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper2.ShadowControl = false;
            this.grouper2.ShadowThickness = 3;
            this.grouper2.Size = new System.Drawing.Size(198, 199);
            this.grouper2.TabIndex = 14;
            this.grouper2.TextLineSpace = 2;
            this.grouper2.TitleLeftSpace = 18;
            // 
            // CBL_AllItems
            // 
            this.CBL_AllItems.FormattingEnabled = true;
            this.CBL_AllItems.HorizontalScrollbar = true;
            this.CBL_AllItems.Location = new System.Drawing.Point(14, 21);
            this.CBL_AllItems.Name = "CBL_AllItems";
            this.CBL_AllItems.Size = new System.Drawing.Size(170, 164);
            this.CBL_AllItems.TabIndex = 13;
            this.CBL_AllItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CBL_AllItems_ItemCheck);
            // 
            // grouper1
            // 
            this.grouper1.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouper1.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouper1.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouper1.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grouper1.BorderThickness = 1F;
            this.grouper1.BorderTopOnly = false;
            this.grouper1.Controls.Add(this.LB_ShowOrder);
            this.grouper1.Controls.Add(this.ApplyBtn);
            this.grouper1.Controls.Add(this.UpBtn);
            this.grouper1.Controls.Add(this.DownBtn);
            this.grouper1.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grouper1.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grouper1.GroupImage = null;
            this.grouper1.GroupTitle = "";
            this.grouper1.HeaderRoundCorners = 4;
            this.grouper1.Location = new System.Drawing.Point(154, 63);
            this.grouper1.Name = "grouper1";
            this.grouper1.PaintGroupBox = false;
            this.grouper1.RoundCorners = 4;
            this.grouper1.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouper1.ShadowControl = false;
            this.grouper1.ShadowThickness = 3;
            this.grouper1.Size = new System.Drawing.Size(255, 199);
            this.grouper1.TabIndex = 14;
            this.grouper1.TextLineSpace = 2;
            this.grouper1.TitleLeftSpace = 18;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "名字";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // UCNameSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grouper2);
            this.Controls.Add(this.TBW_ShowText);
            this.Controls.Add(this.CombinationNameLab);
            this.Controls.Add(this.DGV_NameList);
            this.Controls.Add(this.grouper1);
            this.Name = "UCNameSetting";
            this.Size = new System.Drawing.Size(640, 278);
            this.Load += new System.EventHandler(this.UCNameSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_NameList)).EndInit();
            this.grouper2.ResumeLayout(false);
            this.grouper1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.DataGridViewW DGV_NameList;
        private System.Windows.Forms.Label CombinationNameLab;
        private Skyray.Controls.ListBoxW LB_ShowOrder;
        private Skyray.Controls.ButtonW UpBtn;
        private Skyray.Controls.ButtonW DownBtn;
        private Skyray.Controls.TextBoxW TBW_ShowText;
        private System.Windows.Forms.DataGridViewTextBoxColumn NeedEditName;
        private Skyray.Controls.ButtonW ApplyBtn;
        private Skyray.Controls.Grouper grouper2;
        private System.Windows.Forms.CheckedListBox CBL_AllItems;
        private Skyray.Controls.Grouper grouper1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    }
}