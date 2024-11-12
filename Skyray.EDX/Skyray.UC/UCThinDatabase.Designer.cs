namespace Skyray.UC
{
    partial class UCThinDatabase
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
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.lbHistorylLine = new Skyray.Controls.LabelW();
            this.grpSelectTimePeriod = new Skyray.Controls.Grouper();
            this.pnlPeriod = new System.Windows.Forms.Panel();
            this.radAllTime = new System.Windows.Forms.RadioButton();
            this.radBefore15Days = new System.Windows.Forms.RadioButton();
            this.dateTimePickerBefore = new System.Windows.Forms.DateTimePicker();
            this.radBeforeCustomTime = new System.Windows.Forms.RadioButton();
            this.radCustomTimePeriod = new System.Windows.Forms.RadioButton();
            this.radBeforeOneMonth = new System.Windows.Forms.RadioButton();
            this.btnStartToClear = new Skyray.Controls.ButtonW();
            this.btnCancelClear = new Skyray.Controls.ButtonW();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblCurrentAndTotal = new System.Windows.Forms.Label();
            this.lblClearTip = new Skyray.Controls.LabelW();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.grpSelectTimePeriod.SuspendLayout();
            this.pnlPeriod.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.CustomFormat = "";
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTo.Location = new System.Drawing.Point(289, 5);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(144, 21);
            this.dateTimePickerTo.TabIndex = 7;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.CustomFormat = "";
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(14, 5);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(148, 21);
            this.dateTimePickerFrom.TabIndex = 6;
            // 
            // lbHistorylLine
            // 
            this.lbHistorylLine.AutoSize = true;
            this.lbHistorylLine.BackColor = System.Drawing.Color.Transparent;
            this.lbHistorylLine.Location = new System.Drawing.Point(214, 10);
            this.lbHistorylLine.Name = "lbHistorylLine";
            this.lbHistorylLine.Size = new System.Drawing.Size(17, 12);
            this.lbHistorylLine.TabIndex = 5;
            this.lbHistorylLine.Text = "至";
            // 
            // grpSelectTimePeriod
            // 
            this.grpSelectTimePeriod.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpSelectTimePeriod.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpSelectTimePeriod.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpSelectTimePeriod.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpSelectTimePeriod.BorderThickness = 1F;
            this.grpSelectTimePeriod.BorderTopOnly = false;
            this.grpSelectTimePeriod.Controls.Add(this.pnlPeriod);
            this.grpSelectTimePeriod.Controls.Add(this.radAllTime);
            this.grpSelectTimePeriod.Controls.Add(this.radBefore15Days);
            this.grpSelectTimePeriod.Controls.Add(this.dateTimePickerBefore);
            this.grpSelectTimePeriod.Controls.Add(this.radBeforeCustomTime);
            this.grpSelectTimePeriod.Controls.Add(this.radCustomTimePeriod);
            this.grpSelectTimePeriod.Controls.Add(this.radBeforeOneMonth);
            this.grpSelectTimePeriod.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpSelectTimePeriod.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpSelectTimePeriod.GroupImage = null;
            this.grpSelectTimePeriod.GroupTitle = "选择时间范围";
            this.grpSelectTimePeriod.HeaderRoundCorners = 4;
            this.grpSelectTimePeriod.Location = new System.Drawing.Point(11, 3);
            this.grpSelectTimePeriod.Name = "grpSelectTimePeriod";
            this.grpSelectTimePeriod.PaintGroupBox = false;
            this.grpSelectTimePeriod.RoundCorners = 4;
            this.grpSelectTimePeriod.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpSelectTimePeriod.ShadowControl = false;
            this.grpSelectTimePeriod.ShadowThickness = 3;
            this.grpSelectTimePeriod.Size = new System.Drawing.Size(531, 180);
            this.grpSelectTimePeriod.TabIndex = 8;
            this.grpSelectTimePeriod.TextLineSpace = 2;
            this.grpSelectTimePeriod.TitleLeftSpace = 18;
            // 
            // pnlPeriod
            // 
            this.pnlPeriod.Controls.Add(this.dateTimePickerTo);
            this.pnlPeriod.Controls.Add(this.lbHistorylLine);
            this.pnlPeriod.Controls.Add(this.dateTimePickerFrom);
            this.pnlPeriod.Location = new System.Drawing.Point(53, 129);
            this.pnlPeriod.Name = "pnlPeriod";
            this.pnlPeriod.Size = new System.Drawing.Size(444, 29);
            this.pnlPeriod.TabIndex = 12;
            this.pnlPeriod.Visible = false;
            // 
            // radAllTime
            // 
            this.radAllTime.AutoSize = true;
            this.radAllTime.Location = new System.Drawing.Point(365, 34);
            this.radAllTime.Name = "radAllTime";
            this.radAllTime.Size = new System.Drawing.Size(71, 16);
            this.radAllTime.TabIndex = 0;
            this.radAllTime.Text = "全部时间";
            this.radAllTime.UseVisualStyleBackColor = true;
            // 
            // radBefore15Days
            // 
            this.radBefore15Days.AutoSize = true;
            this.radBefore15Days.Location = new System.Drawing.Point(215, 34);
            this.radBefore15Days.Name = "radBefore15Days";
            this.radBefore15Days.Size = new System.Drawing.Size(71, 16);
            this.radBefore15Days.TabIndex = 0;
            this.radBefore15Days.Text = "15天之前";
            this.radBefore15Days.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerBefore
            // 
            this.dateTimePickerBefore.CustomFormat = "";
            this.dateTimePickerBefore.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerBefore.Location = new System.Drawing.Point(69, 90);
            this.dateTimePickerBefore.Name = "dateTimePickerBefore";
            this.dateTimePickerBefore.Size = new System.Drawing.Size(148, 21);
            this.dateTimePickerBefore.TabIndex = 6;
            this.dateTimePickerBefore.Visible = false;
            // 
            // radBeforeCustomTime
            // 
            this.radBeforeCustomTime.AutoSize = true;
            this.radBeforeCustomTime.Location = new System.Drawing.Point(53, 71);
            this.radBeforeCustomTime.Name = "radBeforeCustomTime";
            this.radBeforeCustomTime.Size = new System.Drawing.Size(95, 16);
            this.radBeforeCustomTime.TabIndex = 0;
            this.radBeforeCustomTime.Text = "自定义时间前";
            this.radBeforeCustomTime.UseVisualStyleBackColor = true;
            this.radBeforeCustomTime.CheckedChanged += new System.EventHandler(this.radBeforeCustomTime_CheckedChanged);
            // 
            // radCustomTimePeriod
            // 
            this.radCustomTimePeriod.AutoSize = true;
            this.radCustomTimePeriod.Location = new System.Drawing.Point(53, 113);
            this.radCustomTimePeriod.Name = "radCustomTimePeriod";
            this.radCustomTimePeriod.Size = new System.Drawing.Size(119, 16);
            this.radCustomTimePeriod.TabIndex = 0;
            this.radCustomTimePeriod.Text = "自定义时间范围内";
            this.radCustomTimePeriod.UseVisualStyleBackColor = true;
            this.radCustomTimePeriod.CheckedChanged += new System.EventHandler(this.radCustomTimePeriod_CheckedChanged);
            // 
            // radBeforeOneMonth
            // 
            this.radBeforeOneMonth.AutoSize = true;
            this.radBeforeOneMonth.Checked = true;
            this.radBeforeOneMonth.Location = new System.Drawing.Point(53, 34);
            this.radBeforeOneMonth.Name = "radBeforeOneMonth";
            this.radBeforeOneMonth.Size = new System.Drawing.Size(83, 16);
            this.radBeforeOneMonth.TabIndex = 0;
            this.radBeforeOneMonth.TabStop = true;
            this.radBeforeOneMonth.Text = "一个月之前";
            this.radBeforeOneMonth.UseVisualStyleBackColor = true;
            // 
            // btnStartToClear
            // 
            this.btnStartToClear.bSilver = false;
            this.btnStartToClear.Location = new System.Drawing.Point(142, 13);
            this.btnStartToClear.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnStartToClear.MenuPos = new System.Drawing.Point(0, 0);
            this.btnStartToClear.Name = "btnStartToClear";
            this.btnStartToClear.Size = new System.Drawing.Size(75, 23);
            this.btnStartToClear.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnStartToClear.TabIndex = 9;
            this.btnStartToClear.Text = "开始清理";
            this.btnStartToClear.ToFocused = false;
            this.btnStartToClear.UseVisualStyleBackColor = true;
            this.btnStartToClear.Click += new System.EventHandler(this.btnStartToClear_Click);
            // 
            // btnCancelClear
            // 
            this.btnCancelClear.bSilver = false;
            this.btnCancelClear.Location = new System.Drawing.Point(353, 13);
            this.btnCancelClear.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancelClear.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancelClear.Name = "btnCancelClear";
            this.btnCancelClear.Size = new System.Drawing.Size(75, 23);
            this.btnCancelClear.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancelClear.TabIndex = 9;
            this.btnCancelClear.Text = "取消";
            this.btnCancelClear.ToFocused = false;
            this.btnCancelClear.UseVisualStyleBackColor = true;
            this.btnCancelClear.Click += new System.EventHandler(this.btnCancelClear_Click);
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblCurrentAndTotal);
            this.pnlInfo.Controls.Add(this.lblClearTip);
            this.pnlInfo.Controls.Add(this.btnCancelClear);
            this.pnlInfo.Controls.Add(this.progressBar1);
            this.pnlInfo.Controls.Add(this.btnStartToClear);
            this.pnlInfo.Location = new System.Drawing.Point(0, 189);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(557, 116);
            this.pnlInfo.TabIndex = 13;
            // 
            // lblCurrentAndTotal
            // 
            this.lblCurrentAndTotal.Location = new System.Drawing.Point(291, 48);
            this.lblCurrentAndTotal.Name = "lblCurrentAndTotal";
            this.lblCurrentAndTotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblCurrentAndTotal.Size = new System.Drawing.Size(252, 15);
            this.lblCurrentAndTotal.TabIndex = 15;
            this.lblCurrentAndTotal.Text = "0/0";
            // 
            // lblClearTip
            // 
            this.lblClearTip.AutoSize = true;
            this.lblClearTip.BackColor = System.Drawing.Color.Transparent;
            this.lblClearTip.Location = new System.Drawing.Point(10, 97);
            this.lblClearTip.Name = "lblClearTip";
            this.lblClearTip.Size = new System.Drawing.Size(0, 12);
            this.lblClearTip.TabIndex = 13;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar1.Location = new System.Drawing.Point(11, 67);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(531, 22);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 14;
            // 
            // UCThinDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.grpSelectTimePeriod);
            this.Name = "UCThinDatabase";
            this.Size = new System.Drawing.Size(557, 316);
            this.grpSelectTimePeriod.ResumeLayout(false);
            this.grpSelectTimePeriod.PerformLayout();
            this.pnlPeriod.ResumeLayout(false);
            this.pnlPeriod.PerformLayout();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private Skyray.Controls.LabelW lbHistorylLine;
        private Skyray.Controls.Grouper grpSelectTimePeriod;
        private System.Windows.Forms.RadioButton radBefore15Days;
        private System.Windows.Forms.RadioButton radBeforeOneMonth;
        private System.Windows.Forms.RadioButton radAllTime;
        private System.Windows.Forms.RadioButton radBeforeCustomTime;
        private System.Windows.Forms.RadioButton radCustomTimePeriod;
        private System.Windows.Forms.DateTimePicker dateTimePickerBefore;
        private Skyray.Controls.ButtonW btnStartToClear;
        private System.Windows.Forms.Panel pnlPeriod;
        private Skyray.Controls.ButtonW btnCancelClear;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblCurrentAndTotal;
        private Skyray.Controls.LabelW lblClearTip;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
