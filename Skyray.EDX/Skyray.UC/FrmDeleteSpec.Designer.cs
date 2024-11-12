namespace Skyray.UC
{
    partial class FrmDeleteSpec
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
            this.comboDeleteSpec = new System.Windows.Forms.ComboBox();
            this.btnDeleteSpec = new Skyray.Controls.ButtonW();
            this.SuspendLayout();
            // 
            // comboDeleteSpec
            // 
            this.comboDeleteSpec.FormattingEnabled = true;
            this.comboDeleteSpec.Items.AddRange(new object[] {
            "1个月前待分析谱数据",
            "2个月前待分析谱数据",
            "3个月前待分析谱数据",
            "半年前待分析谱数据",
            "一年前待分析谱数据",
            "两年前待分析谱数据"});
            this.comboDeleteSpec.Location = new System.Drawing.Point(143, 79);
            this.comboDeleteSpec.Name = "comboDeleteSpec";
            this.comboDeleteSpec.Size = new System.Drawing.Size(178, 20);
            this.comboDeleteSpec.TabIndex = 0;
            // 
            // btnDeleteSpec
            // 
            this.btnDeleteSpec.bSilver = false;
            this.btnDeleteSpec.Location = new System.Drawing.Point(143, 182);
            this.btnDeleteSpec.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDeleteSpec.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDeleteSpec.Name = "btnDeleteSpec";
            this.btnDeleteSpec.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteSpec.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDeleteSpec.TabIndex = 1;
            this.btnDeleteSpec.Text = "删除";
            this.btnDeleteSpec.ToFocused = false;
            this.btnDeleteSpec.UseVisualStyleBackColor = true;
            this.btnDeleteSpec.Click += new System.EventHandler(this.btnDeleteHis_Click);
            // 
            // FrmDeleteSpec
            // 
            this.ClientSize = new System.Drawing.Size(504, 271);
            this.Controls.Add(this.btnDeleteSpec);
            this.Controls.Add(this.comboDeleteSpec);
            this.Name = "FrmDeleteSpec";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboDeleteSpec;
        private Skyray.Controls.ButtonW btnDeleteSpec;

    }
}
