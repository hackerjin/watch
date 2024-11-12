namespace Skyray.UC
{
    partial class UCComputeIntensity
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
            this.labelResultSample = new System.Windows.Forms.Label();
            this.textBoxSampleName = new System.Windows.Forms.TextBox();
            this.dgvElementIntensity = new Skyray.Controls.DataGridViewW();
            this.btnJoinCurve = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ElementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Intensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BgIntensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvElementIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // labelResultSample
            // 
            this.labelResultSample.AutoSize = true;
            this.labelResultSample.Location = new System.Drawing.Point(12, 19);
            this.labelResultSample.Name = "labelResultSample";
            this.labelResultSample.Size = new System.Drawing.Size(41, 12);
            this.labelResultSample.TabIndex = 1;
            this.labelResultSample.Text = "谱名称";
            // 
            // textBoxSampleName
            // 
            this.textBoxSampleName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBoxSampleName.Location = new System.Drawing.Point(14, 34);
            this.textBoxSampleName.Name = "textBoxSampleName";
            this.textBoxSampleName.ReadOnly = true;
            this.textBoxSampleName.Size = new System.Drawing.Size(202, 21);
            this.textBoxSampleName.TabIndex = 2;
            // 
            // dgvElementIntensity
            // 
            this.dgvElementIntensity.AllowUserToAddRows = false;
            this.dgvElementIntensity.AllowUserToDeleteRows = false;
            this.dgvElementIntensity.AllowUserToResizeColumns = false;
            this.dgvElementIntensity.AllowUserToResizeRows = false;
            this.dgvElementIntensity.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvElementIntensity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvElementIntensity.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvElementIntensity.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvElementIntensity.ColumnHeadersHeight = 20;
            this.dgvElementIntensity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvElementIntensity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ElementName,
            this.Intensity,
            this.BgIntensity});
            this.dgvElementIntensity.Location = new System.Drawing.Point(14, 58);
            this.dgvElementIntensity.Name = "dgvElementIntensity";
            this.dgvElementIntensity.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvElementIntensity.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvElementIntensity.ReadOnly = true;
            this.dgvElementIntensity.RowHeadersVisible = false;
            this.dgvElementIntensity.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvElementIntensity.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvElementIntensity.RowTemplate.Height = 23;
            this.dgvElementIntensity.SecondaryLength = 1;
            this.dgvElementIntensity.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvElementIntensity.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvElementIntensity.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvElementIntensity.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvElementIntensity.ShowEportContextMenu = false;
            this.dgvElementIntensity.Size = new System.Drawing.Size(276, 208);
            this.dgvElementIntensity.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvElementIntensity.TabIndex = 3;
            this.dgvElementIntensity.ToPrintCols = null;
            this.dgvElementIntensity.ToPrintRows = null;
            // 
            // btnJoinCurve
            // 
            this.btnJoinCurve.Location = new System.Drawing.Point(296, 190);
            this.btnJoinCurve.Name = "btnJoinCurve";
            this.btnJoinCurve.Size = new System.Drawing.Size(115, 23);
            this.btnJoinCurve.TabIndex = 4;
            this.btnJoinCurve.Text = "参与工作曲线";
            this.btnJoinCurve.UseVisualStyleBackColor = true;
            this.btnJoinCurve.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(296, 243);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ElementName
            // 
            this.ElementName.HeaderText = "元素";
            this.ElementName.Name = "ElementName";
            this.ElementName.ReadOnly = true;
            this.ElementName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // Bgtensity
            // 
            this.BgIntensity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BgIntensity.HeaderText = "背景强度";
            this.BgIntensity.Name = "Bgtensity";
            this.BgIntensity.ReadOnly = true;
            this.BgIntensity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Intensity
            // 
            this.Intensity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Intensity.HeaderText = "强度";
            this.Intensity.Name = "Intensity";
            this.Intensity.ReadOnly = true;
            this.Intensity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UCComputeIntensity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnJoinCurve);
            this.Controls.Add(this.dgvElementIntensity);
            this.Controls.Add(this.textBoxSampleName);
            this.Controls.Add(this.labelResultSample);
            this.Name = "UCComputeIntensity";
            this.Size = new System.Drawing.Size(422, 279);
            ((System.ComponentModel.ISupportInitialize)(this.dgvElementIntensity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelResultSample;
        private System.Windows.Forms.TextBox textBoxSampleName;
        private Skyray.Controls.DataGridViewW dgvElementIntensity;
        private System.Windows.Forms.Button btnJoinCurve;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ElementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Intensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn BgIntensity;
    }
}