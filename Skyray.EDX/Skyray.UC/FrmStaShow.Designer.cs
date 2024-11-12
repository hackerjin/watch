namespace Skyray.UC
{
    partial class FrmStaShow
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
            this.dgvSta = new Skyray.Controls.DataGridViewW();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSta)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSta
            // 
            this.dgvSta.AllowUserToResizeRows = false;
            this.dgvSta.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
            this.dgvSta.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSta.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvSta.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvSta.ColumnHeadersHeight = 4;
            this.dgvSta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSta.Location = new System.Drawing.Point(0, 0);
            this.dgvSta.Name = "dgvSta";
            this.dgvSta.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvSta.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvSta.RowHeadersVisible = false;
            this.dgvSta.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvSta.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvSta.RowTemplate.Height = 23;
            this.dgvSta.SecondaryLength = 1;
            this.dgvSta.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvSta.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvSta.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvSta.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvSta.ShowEportContextMenu = false;
            this.dgvSta.Size = new System.Drawing.Size(504, 271);
            this.dgvSta.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvSta.TabIndex = 0;
            this.dgvSta.ToPrintCols = null;
            this.dgvSta.ToPrintRows = null;
            // 
            // FrmStaShow
            // 
            this.ClientSize = new System.Drawing.Size(504, 271);
            this.Controls.Add(this.dgvSta);
            this.Name = "FrmStaShow";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Skyray.Controls.DataGridViewW dgvSta;
    }
}
