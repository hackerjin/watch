namespace Skyray.UC
{
    partial class FrmHistoryColumnManage
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
            this.components = new System.ComponentModel.Container();
            this.txtAddColmunName = new Skyray.Controls.TextBoxW();
            this.btnClose = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.dgvColumnManage = new Skyray.Controls.DataGridViewW();
            this.toolTipW1 = new Skyray.Controls.ToolTipW(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnManage)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAddColmunName
            // 
            this.txtAddColmunName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtAddColmunName.Location = new System.Drawing.Point(11, 8);
            this.txtAddColmunName.Name = "txtAddColmunName";
            this.txtAddColmunName.Size = new System.Drawing.Size(120, 21);
            this.txtAddColmunName.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtAddColmunName.TabIndex = 60;
            // 
            // btnClose
            // 
            this.btnClose.bSilver = false;
            this.btnClose.Location = new System.Drawing.Point(272, 343);
            this.btnClose.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnClose.MenuPos = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(91, 23);
            this.btnClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnClose.TabIndex = 59;
            this.btnClose.Text = "取消";
            this.btnClose.ToFocused = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(174, 343);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 58;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(261, 6);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(102, 23);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 57;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(153, 6);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 56;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvColumnManage
            // 
            this.dgvColumnManage.AllowUserToAddRows = false;
            this.dgvColumnManage.AllowUserToDeleteRows = false;
            this.dgvColumnManage.AllowUserToResizeColumns = false;
            this.dgvColumnManage.AllowUserToResizeRows = false;
            this.dgvColumnManage.BackgroundColor = System.Drawing.Color.White;
            this.dgvColumnManage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvColumnManage.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvColumnManage.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvColumnManage.ColumnHeadersHeight = 24;
            this.dgvColumnManage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvColumnManage.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvColumnManage.Location = new System.Drawing.Point(12, 41);
            this.dgvColumnManage.MultiSelect = false;
            this.dgvColumnManage.Name = "dgvColumnManage";
            this.dgvColumnManage.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvColumnManage.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvColumnManage.RowHeadersVisible = false;
            this.dgvColumnManage.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvColumnManage.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvColumnManage.RowTemplate.Height = 23;
            this.dgvColumnManage.SecondaryLength = 1;
            this.dgvColumnManage.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvColumnManage.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvColumnManage.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvColumnManage.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvColumnManage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvColumnManage.ShowEportContextMenu = true;
            this.dgvColumnManage.Size = new System.Drawing.Size(351, 296);
            this.dgvColumnManage.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvColumnManage.TabIndex = 55;
            this.dgvColumnManage.ToPrintCols = null;
            this.dgvColumnManage.ToPrintRows = null;
            // 
            // toolTipW1
            // 
            this.toolTipW1.AutoPopDelay = 5000;
            this.toolTipW1.InitialDelay = 500;
            this.toolTipW1.OwnerDraw = true;
            this.toolTipW1.ReshowDelay = 800;
            this.toolTipW1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTipW1_Popup);
            // 
            // FrmHistoryColumnManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 375);
            this.Controls.Add(this.txtAddColmunName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvColumnManage);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmHistoryColumnManage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户列管理";
            this.Load += new System.EventHandler(this.FrmHistoryColumnManage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnManage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.TextBoxW txtAddColmunName;
        private Skyray.Controls.ButtonW btnClose;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnDel;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.DataGridViewW dgvColumnManage;
        private Skyray.Controls.ToolTipW toolTipW1;
    }
}