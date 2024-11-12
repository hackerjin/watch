namespace Skyray.UC
{
    partial class UCBackUpAndRestore
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
            this.lblBackUpPath = new Skyray.Controls.LabelW();
            this.txtBackUpPath = new Skyray.Controls.TextBoxW();
            this.btnBackUp = new Skyray.Controls.ButtonW();
            this.btnRestore = new Skyray.Controls.ButtonW();
            this.txtRestorePath = new Skyray.Controls.TextBoxW();
            this.lblRestorePath = new Skyray.Controls.LabelW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialogEx1 = new Skyray.Controls.FolderBrowserDialogEx();
            this.chkAutoBackUp = new System.Windows.Forms.CheckBox();
            this.btnCompressDatabase = new Skyray.Controls.ButtonW();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lblHour = new System.Windows.Forms.Label();
            this.lblProbeInterval = new System.Windows.Forms.Label();
            this.gBoxBackup = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.gBoxBackup.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBackUpPath
            // 
            this.lblBackUpPath.AutoSize = true;
            this.lblBackUpPath.BackColor = System.Drawing.Color.Transparent;
            this.lblBackUpPath.Location = new System.Drawing.Point(11, 23);
            this.lblBackUpPath.Name = "lblBackUpPath";
            this.lblBackUpPath.Size = new System.Drawing.Size(65, 12);
            this.lblBackUpPath.TabIndex = 0;
            this.lblBackUpPath.Text = "备份路径：";
            // 
            // txtBackUpPath
            // 
            this.txtBackUpPath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtBackUpPath.Location = new System.Drawing.Point(108, 20);
            this.txtBackUpPath.Name = "txtBackUpPath";
            this.txtBackUpPath.ReadOnly = true;
            this.txtBackUpPath.Size = new System.Drawing.Size(334, 21);
            this.txtBackUpPath.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtBackUpPath.TabIndex = 1;
            // 
            // btnBackUp
            // 
            this.btnBackUp.bSilver = false;
            this.btnBackUp.Location = new System.Drawing.Point(448, 18);
            this.btnBackUp.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnBackUp.MenuPos = new System.Drawing.Point(0, 0);
            this.btnBackUp.Name = "btnBackUp";
            this.btnBackUp.Size = new System.Drawing.Size(42, 23);
            this.btnBackUp.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnBackUp.TabIndex = 2;
            this.btnBackUp.Text = "...";
            this.btnBackUp.ToFocused = false;
            this.btnBackUp.UseVisualStyleBackColor = true;
            this.btnBackUp.Click += new System.EventHandler(this.btnBackUp_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.bSilver = false;
            this.btnRestore.Location = new System.Drawing.Point(448, 56);
            this.btnRestore.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnRestore.MenuPos = new System.Drawing.Point(0, 0);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(42, 23);
            this.btnRestore.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnRestore.TabIndex = 5;
            this.btnRestore.Text = "...";
            this.btnRestore.ToFocused = false;
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // txtRestorePath
            // 
            this.txtRestorePath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtRestorePath.Location = new System.Drawing.Point(108, 58);
            this.txtRestorePath.Name = "txtRestorePath";
            this.txtRestorePath.ReadOnly = true;
            this.txtRestorePath.Size = new System.Drawing.Size(334, 21);
            this.txtRestorePath.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtRestorePath.TabIndex = 4;
            this.txtRestorePath.TextChanged += new System.EventHandler(this.txtRestorePath_TextChanged);
            // 
            // lblRestorePath
            // 
            this.lblRestorePath.AutoSize = true;
            this.lblRestorePath.BackColor = System.Drawing.Color.Transparent;
            this.lblRestorePath.Location = new System.Drawing.Point(11, 61);
            this.lblRestorePath.Name = "lblRestorePath";
            this.lblRestorePath.Size = new System.Drawing.Size(65, 12);
            this.lblRestorePath.TabIndex = 3;
            this.lblRestorePath.Text = "还原路径：";
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(320, 162);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(401, 162);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // folderBrowserDialogEx1
            // 
            this.folderBrowserDialogEx1.Description = "";
            this.folderBrowserDialogEx1.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.folderBrowserDialogEx1.RootFolderPath = "";
            this.folderBrowserDialogEx1.SelectedPath = "";
            this.folderBrowserDialogEx1.ShowNewFolderButton = false;
            // 
            // chkAutoBackUp
            // 
            this.chkAutoBackUp.AutoSize = true;
            this.chkAutoBackUp.Location = new System.Drawing.Point(46, 28);
            this.chkAutoBackUp.Name = "chkAutoBackUp";
            this.chkAutoBackUp.Size = new System.Drawing.Size(72, 16);
            this.chkAutoBackUp.TabIndex = 8;
            this.chkAutoBackUp.Text = "自动备份";
            this.chkAutoBackUp.UseVisualStyleBackColor = true;
            // 
            // btnCompressDatabase
            // 
            this.btnCompressDatabase.bSilver = false;
            this.btnCompressDatabase.Location = new System.Drawing.Point(14, 162);
            this.btnCompressDatabase.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCompressDatabase.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCompressDatabase.Name = "btnCompressDatabase";
            this.btnCompressDatabase.Size = new System.Drawing.Size(104, 23);
            this.btnCompressDatabase.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCompressDatabase.TabIndex = 18;
            this.btnCompressDatabase.Text = "压缩数据库";
            this.btnCompressDatabase.ToFocused = false;
            this.btnCompressDatabase.UseVisualStyleBackColor = true;
            this.btnCompressDatabase.Click += new System.EventHandler(this.btnCompressDatabase_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(323, 26);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(70, 21);
            this.numericUpDown1.TabIndex = 21;
            // 
            // lblHour
            // 
            this.lblHour.AutoSize = true;
            this.lblHour.Location = new System.Drawing.Point(411, 30);
            this.lblHour.Name = "lblHour";
            this.lblHour.Size = new System.Drawing.Size(29, 12);
            this.lblHour.TabIndex = 22;
            this.lblHour.Text = "小时";
            // 
            // lblProbeInterval
            // 
            this.lblProbeInterval.AutoSize = true;
            this.lblProbeInterval.Location = new System.Drawing.Point(170, 30);
            this.lblProbeInterval.Name = "lblProbeInterval";
            this.lblProbeInterval.Size = new System.Drawing.Size(101, 12);
            this.lblProbeInterval.TabIndex = 20;
            this.lblProbeInterval.Text = "自动备份时间间隔";
            // 
            // gBoxBackup
            // 
            this.gBoxBackup.Controls.Add(this.chkAutoBackUp);
            this.gBoxBackup.Controls.Add(this.lblProbeInterval);
            this.gBoxBackup.Controls.Add(this.lblHour);
            this.gBoxBackup.Controls.Add(this.numericUpDown1);
            this.gBoxBackup.Location = new System.Drawing.Point(11, 87);
            this.gBoxBackup.Name = "gBoxBackup";
            this.gBoxBackup.Size = new System.Drawing.Size(479, 69);
            this.gBoxBackup.TabIndex = 17;
            this.gBoxBackup.TabStop = false;
            this.gBoxBackup.Text = "备份设置";
            // 
            // UCBackUpAndRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnCompressDatabase);
            this.Controls.Add(this.gBoxBackup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.txtRestorePath);
            this.Controls.Add(this.lblRestorePath);
            this.Controls.Add(this.btnBackUp);
            this.Controls.Add(this.txtBackUpPath);
            this.Controls.Add(this.lblBackUpPath);
            this.Name = "UCBackUpAndRestore";
            this.Size = new System.Drawing.Size(508, 201);
            this.Load += new System.EventHandler(this.UCBackUpAndRestore_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.gBoxBackup.ResumeLayout(false);
            this.gBoxBackup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblBackUpPath;
        private Skyray.Controls.TextBoxW txtBackUpPath;
        private Skyray.Controls.ButtonW btnBackUp;
        private Skyray.Controls.ButtonW btnRestore;
        private Skyray.Controls.TextBoxW txtRestorePath;
        private Skyray.Controls.LabelW lblRestorePath;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private Skyray.Controls.FolderBrowserDialogEx folderBrowserDialogEx1;
        private System.Windows.Forms.CheckBox chkAutoBackUp;
        private Skyray.Controls.ButtonW btnCompressDatabase;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label lblHour;
        private System.Windows.Forms.Label lblProbeInterval;
        private System.Windows.Forms.GroupBox gBoxBackup;
    }
}
