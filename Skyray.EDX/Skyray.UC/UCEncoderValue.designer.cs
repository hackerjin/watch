namespace Skyray.UC
{
    partial class UCEncoderValue
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblX1 = new Skyray.Controls.LabelW();
            this.btncalc = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.txtX1 = new Skyray.Controls.TextBoxW();
            this.lbly1 = new Skyray.Controls.LabelW();
            this.txtY1 = new Skyray.Controls.TextBoxW();
            this.txtY2 = new Skyray.Controls.TextBoxW();
            this.lbly2 = new Skyray.Controls.LabelW();
            this.txtX2 = new Skyray.Controls.TextBoxW();
            this.lxlX2 = new Skyray.Controls.LabelW();
            this.lblEncoderValue = new Skyray.Controls.LabelW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.txtFormula = new Skyray.Controls.TextBoxW();
            this.dgvEncoder = new Skyray.Controls.DataGridViewW();
            this.colXpoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYpoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radTwoForcedOrigin = new System.Windows.Forms.RadioButton();
            this.radTwoNoForcedOrigin = new System.Windows.Forms.RadioButton();
            this.gpbradiobox = new System.Windows.Forms.GroupBox();
            this.chkOpenorClose = new Skyray.Controls.CheckBoxW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEncoder)).BeginInit();
            this.gpbradiobox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblX1
            // 
            this.lblX1.AutoSize = true;
            this.lblX1.BackColor = System.Drawing.Color.Transparent;
            this.lblX1.Location = new System.Drawing.Point(71, 38);
            this.lblX1.Name = "lblX1";
            this.lblX1.Size = new System.Drawing.Size(17, 12);
            this.lblX1.TabIndex = 0;
            this.lblX1.Text = "x1";
            // 
            // btncalc
            // 
            this.btncalc.bSilver = false;
            this.btncalc.Location = new System.Drawing.Point(375, 157);
            this.btncalc.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btncalc.MenuPos = new System.Drawing.Point(0, 0);
            this.btncalc.Name = "btncalc";
            this.btncalc.Size = new System.Drawing.Size(75, 30);
            this.btncalc.Style = Skyray.Controls.Style.Office2007Blue;
            this.btncalc.TabIndex = 1;
            this.btncalc.Text = "计算";
            this.btncalc.ToFocused = false;
            this.btncalc.UseVisualStyleBackColor = true;
            this.btncalc.Click += new System.EventHandler(this.btncalc_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(375, 207);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtX1
            // 
            this.txtX1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtX1.Location = new System.Drawing.Point(97, 35);
            this.txtX1.Name = "txtX1";
            this.txtX1.Size = new System.Drawing.Size(80, 21);
            this.txtX1.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtX1.TabIndex = 3;
            // 
            // lbly1
            // 
            this.lbly1.AutoSize = true;
            this.lbly1.BackColor = System.Drawing.Color.Transparent;
            this.lbly1.Location = new System.Drawing.Point(212, 38);
            this.lbly1.Name = "lbly1";
            this.lbly1.Size = new System.Drawing.Size(17, 12);
            this.lbly1.TabIndex = 4;
            this.lbly1.Text = "y1";
            // 
            // txtY1
            // 
            this.txtY1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtY1.Location = new System.Drawing.Point(236, 35);
            this.txtY1.Name = "txtY1";
            this.txtY1.Size = new System.Drawing.Size(80, 21);
            this.txtY1.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtY1.TabIndex = 5;
            // 
            // txtY2
            // 
            this.txtY2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtY2.Location = new System.Drawing.Point(180, 108);
            this.txtY2.Name = "txtY2";
            this.txtY2.Size = new System.Drawing.Size(80, 21);
            this.txtY2.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtY2.TabIndex = 9;
            // 
            // lbly2
            // 
            this.lbly2.AutoSize = true;
            this.lbly2.BackColor = System.Drawing.Color.Transparent;
            this.lbly2.Location = new System.Drawing.Point(156, 111);
            this.lbly2.Name = "lbly2";
            this.lbly2.Size = new System.Drawing.Size(17, 12);
            this.lbly2.TabIndex = 8;
            this.lbly2.Text = "y2";
            // 
            // txtX2
            // 
            this.txtX2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtX2.Location = new System.Drawing.Point(182, 76);
            this.txtX2.Name = "txtX2";
            this.txtX2.Size = new System.Drawing.Size(80, 21);
            this.txtX2.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtX2.TabIndex = 7;
            // 
            // lxlX2
            // 
            this.lxlX2.AutoSize = true;
            this.lxlX2.BackColor = System.Drawing.Color.Transparent;
            this.lxlX2.Location = new System.Drawing.Point(156, 78);
            this.lxlX2.Name = "lxlX2";
            this.lxlX2.Size = new System.Drawing.Size(17, 12);
            this.lxlX2.TabIndex = 6;
            this.lxlX2.Text = "x2";
            // 
            // lblEncoderValue
            // 
            this.lblEncoderValue.AutoSize = true;
            this.lblEncoderValue.BackColor = System.Drawing.Color.Transparent;
            this.lblEncoderValue.Location = new System.Drawing.Point(90, 326);
            this.lblEncoderValue.Name = "lblEncoderValue";
            this.lblEncoderValue.Size = new System.Drawing.Size(71, 12);
            this.lblEncoderValue.TabIndex = 10;
            this.lblEncoderValue.Text = "公式 ：  y=";
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(375, 257);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtFormula
            // 
            this.txtFormula.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtFormula.Location = new System.Drawing.Point(163, 323);
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.Size = new System.Drawing.Size(159, 21);
            this.txtFormula.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtFormula.TabIndex = 13;
            // 
            // dgvEncoder
            // 
            this.dgvEncoder.AllowUserToAddRows = false;
            this.dgvEncoder.AllowUserToResizeRows = false;
            this.dgvEncoder.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvEncoder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEncoder.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvEncoder.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvEncoder.ColumnHeadersHeight = 20;
            this.dgvEncoder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEncoder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colXpoint,
            this.colYpoint});
            this.dgvEncoder.Location = new System.Drawing.Point(74, 29);
            this.dgvEncoder.MultiSelect = false;
            this.dgvEncoder.Name = "dgvEncoder";
            this.dgvEncoder.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvEncoder.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvEncoder.RowHeadersVisible = false;
            this.dgvEncoder.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvEncoder.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvEncoder.RowTemplate.Height = 23;
            this.dgvEncoder.SecondaryLength = 1;
            this.dgvEncoder.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvEncoder.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvEncoder.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvEncoder.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvEncoder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEncoder.ShowEportContextMenu = false;
            this.dgvEncoder.Size = new System.Drawing.Size(263, 208);
            this.dgvEncoder.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvEncoder.TabIndex = 14;
            this.dgvEncoder.ToPrintCols = null;
            this.dgvEncoder.ToPrintRows = null;
            this.dgvEncoder.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEncoder_CellContentClick);
            // 
            // colXpoint
            // 
            this.colXpoint.DataPropertyName = "X";
            this.colXpoint.HeaderText = "X";
            this.colXpoint.Name = "colXpoint";
            this.colXpoint.Width = 130;
            // 
            // colYpoint
            // 
            this.colYpoint.DataPropertyName = "Y";
            this.colYpoint.HeaderText = "Y";
            this.colYpoint.Name = "colYpoint";
            this.colYpoint.Width = 130;
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(375, 60);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 30);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "添加";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(375, 108);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 30);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 16;
            this.btnDel.Text = "删除";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "X";
            this.dataGridViewTextBoxColumn1.HeaderText = "X";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 130;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Y";
            this.dataGridViewTextBoxColumn2.HeaderText = "Y";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 130;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Y";
            this.dataGridViewTextBoxColumn3.HeaderText = "Y";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 130;
            // 
            // radTwoForcedOrigin
            // 
            this.radTwoForcedOrigin.AutoSize = true;
            this.radTwoForcedOrigin.Location = new System.Drawing.Point(19, 36);
            this.radTwoForcedOrigin.Name = "radTwoForcedOrigin";
            this.radTwoForcedOrigin.Size = new System.Drawing.Size(107, 16);
            this.radTwoForcedOrigin.TabIndex = 18;
            this.radTwoForcedOrigin.TabStop = true;
            this.radTwoForcedOrigin.Text = "两次强制过原点";
            this.radTwoForcedOrigin.UseVisualStyleBackColor = true;
            this.radTwoForcedOrigin.CheckedChanged += new System.EventHandler(this.radTwoNoForcedOrigin_CheckedChanged);
            // 
            // radTwoNoForcedOrigin
            // 
            this.radTwoNoForcedOrigin.AutoSize = true;
            this.radTwoNoForcedOrigin.Location = new System.Drawing.Point(19, 15);
            this.radTwoNoForcedOrigin.Name = "radTwoNoForcedOrigin";
            this.radTwoNoForcedOrigin.Size = new System.Drawing.Size(119, 16);
            this.radTwoNoForcedOrigin.TabIndex = 17;
            this.radTwoNoForcedOrigin.TabStop = true;
            this.radTwoNoForcedOrigin.Text = "两次不强制过原点";
            this.radTwoNoForcedOrigin.UseVisualStyleBackColor = true;
            this.radTwoNoForcedOrigin.CheckedChanged += new System.EventHandler(this.radTwoNoForcedOrigin_CheckedChanged);
            // 
            // gpbradiobox
            // 
            this.gpbradiobox.Controls.Add(this.radTwoNoForcedOrigin);
            this.gpbradiobox.Controls.Add(this.radTwoForcedOrigin);
            this.gpbradiobox.Location = new System.Drawing.Point(74, 242);
            this.gpbradiobox.Name = "gpbradiobox";
            this.gpbradiobox.Size = new System.Drawing.Size(200, 59);
            this.gpbradiobox.TabIndex = 19;
            this.gpbradiobox.TabStop = false;
            // 
            // chkOpenorClose
            // 
            this.chkOpenorClose.AutoSize = true;
            this.chkOpenorClose.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkOpenorClose.Location = new System.Drawing.Point(375, 29);
            this.chkOpenorClose.Name = "chkOpenorClose";
            this.chkOpenorClose.Size = new System.Drawing.Size(84, 16);
            this.chkOpenorClose.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkOpenorClose.TabIndex = 20;
            this.chkOpenorClose.Text = "编码器开关";
            this.chkOpenorClose.UseVisualStyleBackColor = true;
            // 
            // UCEncoderValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.chkOpenorClose);
            this.Controls.Add(this.gpbradiobox);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvEncoder);
            this.Controls.Add(this.txtFormula);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblEncoderValue);
            this.Controls.Add(this.txtY2);
            this.Controls.Add(this.lbly2);
            this.Controls.Add(this.txtX2);
            this.Controls.Add(this.lxlX2);
            this.Controls.Add(this.txtY1);
            this.Controls.Add(this.lbly1);
            this.Controls.Add(this.txtX1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btncalc);
            this.Controls.Add(this.lblX1);
            this.Name = "UCEncoderValue";
            this.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Size = new System.Drawing.Size(529, 376);
            this.Load += new System.EventHandler(this.UCEncoderValue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEncoder)).EndInit();
            this.gpbradiobox.ResumeLayout(false);
            this.gpbradiobox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.LabelW lblX1;
        private Skyray.Controls.ButtonW btncalc;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.TextBoxW txtX1;
        private Skyray.Controls.LabelW lbly1;
        private Skyray.Controls.TextBoxW txtY1;
        private Skyray.Controls.TextBoxW txtY2;
        private Skyray.Controls.LabelW lbly2;
        private Skyray.Controls.TextBoxW txtX2;
        private Skyray.Controls.LabelW lxlX2;
        private Skyray.Controls.LabelW lblEncoderValue;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.TextBoxW txtFormula;
        private Skyray.Controls.DataGridViewW dgvEncoder;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.ButtonW btnDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colXpoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYpoint;
        private System.Windows.Forms.RadioButton radTwoForcedOrigin;
        private System.Windows.Forms.RadioButton radTwoNoForcedOrigin;
        private System.Windows.Forms.GroupBox gpbradiobox;
        private Skyray.Controls.CheckBoxW chkOpenorClose;



    }
}
