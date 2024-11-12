namespace Skyray.UC
{
    partial class FrmUser
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
            this.buttonWCancel = new Skyray.Controls.ButtonW();
            this.dataGridViewUser = new Skyray.Controls.DataGridViewW();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Right = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.buttonWDelete = new Skyray.Controls.ButtonW();
            this.buttonWAdd = new Skyray.Controls.ButtonW();
            this.buttonWSubmit = new Skyray.Controls.ButtonW();
            this.btnApplication = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUser)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonWCancel
            // 
            this.buttonWCancel.bSilver = false;
            this.buttonWCancel.Location = new System.Drawing.Point(413, 212);
            this.buttonWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWCancel.Name = "buttonWCancel";
            this.buttonWCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWCancel.TabIndex = 2;
            this.buttonWCancel.Text = "取消";
            this.buttonWCancel.ToFocused = false;
            this.buttonWCancel.UseVisualStyleBackColor = true;
            this.buttonWCancel.Click += new System.EventHandler(this.buttonWCancel_Click);
            // 
            // dataGridViewUser
            // 
            this.dataGridViewUser.AllowUserToAddRows = false;
            this.dataGridViewUser.AllowUserToResizeColumns = false;
            this.dataGridViewUser.AllowUserToResizeRows = false;
            this.dataGridViewUser.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewUser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewUser.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dataGridViewUser.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dataGridViewUser.ColumnHeadersHeight = 24;
            this.dataGridViewUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.UserName,
            this.Password,
            this.Right});
            this.dataGridViewUser.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewUser.Location = new System.Drawing.Point(8, 8);
            this.dataGridViewUser.Name = "dataGridViewUser";
            this.dataGridViewUser.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dataGridViewUser.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dataGridViewUser.RowHeadersVisible = false;
            this.dataGridViewUser.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dataGridViewUser.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewUser.RowTemplate.Height = 23;
            this.dataGridViewUser.SecondaryLength = 1;
            this.dataGridViewUser.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dataGridViewUser.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dataGridViewUser.SelectedRowColor1 = System.Drawing.Color.White;
            this.dataGridViewUser.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dataGridViewUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUser.ShowEportContextMenu = true;
            this.dataGridViewUser.Size = new System.Drawing.Size(486, 195);
            this.dataGridViewUser.Style = Skyray.Controls.Style.Office2007Blue;
            this.dataGridViewUser.TabIndex = 0;
            this.dataGridViewUser.ToPrintCols = null;
            this.dataGridViewUser.ToPrintRows = null;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Visible = false;
            // 
            // UserName
            // 
            this.UserName.DataPropertyName = "User";
            this.UserName.HeaderText = "用户名";
            this.UserName.Name = "UserName";
            this.UserName.Width = 150;
            // 
            // Password
            // 
            this.Password.DataPropertyName = "Password";
            this.Password.HeaderText = "密码";
            this.Password.Name = "Password";
            this.Password.Width = 150;
            // 
            // Right
            // 
            this.Right.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Right.DataPropertyName = "Right";
            this.Right.HeaderText = "权限";
            this.Right.Name = "Right";
            this.Right.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Right.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // buttonWDelete
            // 
            this.buttonWDelete.bSilver = false;
            this.buttonWDelete.Location = new System.Drawing.Point(116, 212);
            this.buttonWDelete.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWDelete.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWDelete.Name = "buttonWDelete";
            this.buttonWDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonWDelete.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWDelete.TabIndex = 3;
            this.buttonWDelete.Text = "删除";
            this.buttonWDelete.ToFocused = false;
            this.buttonWDelete.UseVisualStyleBackColor = true;
            this.buttonWDelete.Click += new System.EventHandler(this.buttonWDelete_Click);
            // 
            // buttonWAdd
            // 
            this.buttonWAdd.bSilver = false;
            this.buttonWAdd.Location = new System.Drawing.Point(17, 212);
            this.buttonWAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWAdd.Name = "buttonWAdd";
            this.buttonWAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonWAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWAdd.TabIndex = 4;
            this.buttonWAdd.Text = "添加";
            this.buttonWAdd.ToFocused = false;
            this.buttonWAdd.UseVisualStyleBackColor = true;
            this.buttonWAdd.Click += new System.EventHandler(this.buttonWAdd_Click);
            // 
            // buttonWSubmit
            // 
            this.buttonWSubmit.bSilver = false;
            this.buttonWSubmit.Location = new System.Drawing.Point(314, 212);
            this.buttonWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.Name = "buttonWSubmit";
            this.buttonWSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWSubmit.TabIndex = 3;
            this.buttonWSubmit.Text = "确定";
            this.buttonWSubmit.ToFocused = false;
            this.buttonWSubmit.UseVisualStyleBackColor = true;
            this.buttonWSubmit.Click += new System.EventHandler(this.buttonWSubmit_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(215, 212);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(75, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 5;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            // 
            // FrmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.buttonWAdd);
            this.Controls.Add(this.buttonWDelete);
            this.Controls.Add(this.dataGridViewUser);
            this.Controls.Add(this.buttonWCancel);
            this.Controls.Add(this.buttonWSubmit);
            this.Name = "FrmUser";
            this.Size = new System.Drawing.Size(504, 247);
            this.Load += new System.EventHandler(this.FrmUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.DataGridViewW dataGridViewUser;
        private Skyray.Controls.ButtonW buttonWCancel;
        private Skyray.Controls.ButtonW buttonWDelete;
        private Skyray.Controls.ButtonW buttonWAdd;
        private Skyray.Controls.ButtonW buttonWSubmit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private new System.Windows.Forms.DataGridViewComboBoxColumn Right;
        private Skyray.Controls.ButtonW btnApplication;

    }
}