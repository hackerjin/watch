namespace Skyray.Camera
{
    partial class FrmVideoParam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVideoParam));
            this.txtFocusX = new System.Windows.Forms.NumericUpDown();
            this.lblFocusX = new System.Windows.Forms.Label();
            this.lblFocusY = new System.Windows.Forms.Label();
            this.txtFocusY = new System.Windows.Forms.NumericUpDown();
            this.lblScaleUnit = new System.Windows.Forms.Label();
            this.txtScaleUnit = new System.Windows.Forms.NumericUpDown();
            this.lblFociHeight = new System.Windows.Forms.Label();
            this.txtSpotHeight = new System.Windows.Forms.NumericUpDown();
            this.lblFociWidth = new System.Windows.Forms.Label();
            this.txtSpotWidth = new System.Windows.Forms.NumericUpDown();
            this.rbtnSpotShapeEllipse = new System.Windows.Forms.RadioButton();
            this.rbtnSpotShapeRectangle = new System.Windows.Forms.RadioButton();
            this.btnAccept = new System.Windows.Forms.Button();
            this.lblViewHeight = new System.Windows.Forms.Label();
            this.txtViewHeight = new System.Windows.Forms.NumericUpDown();
            this.lblViewWidth = new System.Windows.Forms.Label();
            this.txtViewWidth = new System.Windows.Forms.NumericUpDown();
            this.btnDefault = new System.Windows.Forms.Button();
            this.gbxView = new System.Windows.Forms.GroupBox();
            this.gbxFocus = new System.Windows.Forms.GroupBox();
            this.cboColor = new System.Windows.Forms.ComboBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblSpotShape = new System.Windows.Forms.Label();
            this.gbxSpot = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtFocusX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFocusY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScaleUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpotHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpotWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtViewHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtViewWidth)).BeginInit();
            this.gbxView.SuspendLayout();
            this.gbxFocus.SuspendLayout();
            this.gbxSpot.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFocusX
            // 
            this.txtFocusX.DecimalPlaces = 2;
            this.txtFocusX.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtFocusX.Location = new System.Drawing.Point(94, 24);
            this.txtFocusX.Name = "txtFocusX";
            this.txtFocusX.Size = new System.Drawing.Size(63, 21);
            this.txtFocusX.TabIndex = 1;
            this.txtFocusX.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtFocusX.Leave += new System.EventHandler(this.txtFocusX_TextChanged);
            // 
            // lblFocusX
            // 
            this.lblFocusX.AutoSize = true;
            this.lblFocusX.Location = new System.Drawing.Point(23, 27);
            this.lblFocusX.Name = "lblFocusX";
            this.lblFocusX.Size = new System.Drawing.Size(59, 12);
            this.lblFocusX.TabIndex = 3;
            this.lblFocusX.Text = "X坐标(mm)";
            // 
            // lblFocusY
            // 
            this.lblFocusY.AutoSize = true;
            this.lblFocusY.Location = new System.Drawing.Point(23, 54);
            this.lblFocusY.Name = "lblFocusY";
            this.lblFocusY.Size = new System.Drawing.Size(59, 12);
            this.lblFocusY.TabIndex = 5;
            this.lblFocusY.Text = "Y坐标(mm)";
            // 
            // txtFocusY
            // 
            this.txtFocusY.DecimalPlaces = 2;
            this.txtFocusY.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtFocusY.Location = new System.Drawing.Point(94, 51);
            this.txtFocusY.Name = "txtFocusY";
            this.txtFocusY.Size = new System.Drawing.Size(63, 21);
            this.txtFocusY.TabIndex = 4;
            this.txtFocusY.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtFocusY.Leave += new System.EventHandler(this.txtFocusY_TextChanged);
            // 
            // lblScaleUnit
            // 
            this.lblScaleUnit.AutoSize = true;
            this.lblScaleUnit.Location = new System.Drawing.Point(20, 97);
            this.lblScaleUnit.Name = "lblScaleUnit";
            this.lblScaleUnit.Size = new System.Drawing.Size(77, 12);
            this.lblScaleUnit.TabIndex = 7;
            this.lblScaleUnit.Text = "尺度单位(mm)";
            // 
            // txtScaleUnit
            // 
            this.txtScaleUnit.DecimalPlaces = 2;
            this.txtScaleUnit.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtScaleUnit.Location = new System.Drawing.Point(130, 94);
            this.txtScaleUnit.Name = "txtScaleUnit";
            this.txtScaleUnit.Size = new System.Drawing.Size(63, 21);
            this.txtScaleUnit.TabIndex = 6;
            this.txtScaleUnit.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txtScaleUnit.Leave += new System.EventHandler(this.txtScaleUnit_TextChanged);
            // 
            // lblFociHeight
            // 
            this.lblFociHeight.AutoSize = true;
            this.lblFociHeight.Location = new System.Drawing.Point(23, 61);
            this.lblFociHeight.Name = "lblFociHeight";
            this.lblFociHeight.Size = new System.Drawing.Size(53, 12);
            this.lblFociHeight.TabIndex = 11;
            this.lblFociHeight.Text = "高度(mm)";
            // 
            // txtSpotHeight
            // 
            this.txtSpotHeight.DecimalPlaces = 3;
            this.txtSpotHeight.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSpotHeight.Location = new System.Drawing.Point(104, 58);
            this.txtSpotHeight.Name = "txtSpotHeight";
            this.txtSpotHeight.Size = new System.Drawing.Size(63, 21);
            this.txtSpotHeight.TabIndex = 10;
            this.txtSpotHeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblFociWidth
            // 
            this.lblFociWidth.AutoSize = true;
            this.lblFociWidth.Location = new System.Drawing.Point(23, 34);
            this.lblFociWidth.Name = "lblFociWidth";
            this.lblFociWidth.Size = new System.Drawing.Size(53, 12);
            this.lblFociWidth.TabIndex = 9;
            this.lblFociWidth.Text = "宽度(mm)";
            // 
            // txtSpotWidth
            // 
            this.txtSpotWidth.DecimalPlaces = 3;
            this.txtSpotWidth.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSpotWidth.Location = new System.Drawing.Point(104, 31);
            this.txtSpotWidth.Name = "txtSpotWidth";
            this.txtSpotWidth.Size = new System.Drawing.Size(63, 21);
            this.txtSpotWidth.TabIndex = 8;
            this.txtSpotWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // rbtnSpotShapeEllipse
            // 
            this.rbtnSpotShapeEllipse.AutoSize = true;
            this.rbtnSpotShapeEllipse.Location = new System.Drawing.Point(93, 105);
            this.rbtnSpotShapeEllipse.Name = "rbtnSpotShapeEllipse";
            this.rbtnSpotShapeEllipse.Size = new System.Drawing.Size(71, 16);
            this.rbtnSpotShapeEllipse.TabIndex = 1;
            this.rbtnSpotShapeEllipse.Text = "椭    圆";
            this.rbtnSpotShapeEllipse.UseVisualStyleBackColor = true;
            // 
            // rbtnSpotShapeRectangle
            // 
            this.rbtnSpotShapeRectangle.AutoSize = true;
            this.rbtnSpotShapeRectangle.Checked = true;
            this.rbtnSpotShapeRectangle.Location = new System.Drawing.Point(93, 84);
            this.rbtnSpotShapeRectangle.Name = "rbtnSpotShapeRectangle";
            this.rbtnSpotShapeRectangle.Size = new System.Drawing.Size(71, 16);
            this.rbtnSpotShapeRectangle.TabIndex = 0;
            this.rbtnSpotShapeRectangle.TabStop = true;
            this.rbtnSpotShapeRectangle.Text = "矩    形";
            this.rbtnSpotShapeRectangle.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAccept.Location = new System.Drawing.Point(661, 114);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 13;
            this.btnAccept.Text = "确  定";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.setCameraParam);
            // 
            // lblViewHeight
            // 
            this.lblViewHeight.AutoSize = true;
            this.lblViewHeight.Location = new System.Drawing.Point(20, 62);
            this.lblViewHeight.Name = "lblViewHeight";
            this.lblViewHeight.Size = new System.Drawing.Size(53, 12);
            this.lblViewHeight.TabIndex = 18;
            this.lblViewHeight.Text = "高度(mm)";
            // 
            // txtViewHeight
            // 
            this.txtViewHeight.DecimalPlaces = 2;
            this.txtViewHeight.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtViewHeight.Location = new System.Drawing.Point(130, 59);
            this.txtViewHeight.Name = "txtViewHeight";
            this.txtViewHeight.Size = new System.Drawing.Size(63, 21);
            this.txtViewHeight.TabIndex = 17;
            this.txtViewHeight.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.txtViewHeight.Leave += new System.EventHandler(this.txtViewHeight_TextChanged);
            // 
            // lblViewWidth
            // 
            this.lblViewWidth.AutoSize = true;
            this.lblViewWidth.Location = new System.Drawing.Point(20, 27);
            this.lblViewWidth.Name = "lblViewWidth";
            this.lblViewWidth.Size = new System.Drawing.Size(53, 12);
            this.lblViewWidth.TabIndex = 16;
            this.lblViewWidth.Text = "宽度(mm)";
            // 
            // txtViewWidth
            // 
            this.txtViewWidth.DecimalPlaces = 2;
            this.txtViewWidth.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtViewWidth.Location = new System.Drawing.Point(130, 24);
            this.txtViewWidth.Name = "txtViewWidth";
            this.txtViewWidth.Size = new System.Drawing.Size(63, 21);
            this.txtViewWidth.TabIndex = 15;
            this.txtViewWidth.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.txtViewWidth.Leave += new System.EventHandler(this.txtViewWidth_TextChanged);
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(661, 55);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 19;
            this.btnDefault.Text = "默  认";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // gbxView
            // 
            this.gbxView.Controls.Add(this.lblViewWidth);
            this.gbxView.Controls.Add(this.lblScaleUnit);
            this.gbxView.Controls.Add(this.txtViewWidth);
            this.gbxView.Controls.Add(this.txtScaleUnit);
            this.gbxView.Controls.Add(this.txtViewHeight);
            this.gbxView.Controls.Add(this.lblViewHeight);
            this.gbxView.Location = new System.Drawing.Point(429, 12);
            this.gbxView.Name = "gbxView";
            this.gbxView.Size = new System.Drawing.Size(214, 129);
            this.gbxView.TabIndex = 20;
            this.gbxView.TabStop = false;
            this.gbxView.Text = "画    面";
            // 
            // gbxFocus
            // 
            this.gbxFocus.Controls.Add(this.cboColor);
            this.gbxFocus.Controls.Add(this.lblColor);
            this.gbxFocus.Controls.Add(this.rbtnSpotShapeEllipse);
            this.gbxFocus.Controls.Add(this.lblFocusX);
            this.gbxFocus.Controls.Add(this.txtFocusX);
            this.gbxFocus.Controls.Add(this.rbtnSpotShapeRectangle);
            this.gbxFocus.Controls.Add(this.txtFocusY);
            this.gbxFocus.Controls.Add(this.lblSpotShape);
            this.gbxFocus.Controls.Add(this.lblFocusY);
            this.gbxFocus.Location = new System.Drawing.Point(12, 12);
            this.gbxFocus.Name = "gbxFocus";
            this.gbxFocus.Size = new System.Drawing.Size(185, 161);
            this.gbxFocus.TabIndex = 21;
            this.gbxFocus.TabStop = false;
            this.gbxFocus.Text = "光斑中心";
            // 
            // cboColor
            // 
            this.cboColor.FormattingEnabled = true;
            this.cboColor.Items.AddRange(new object[] {
            "红",
            "橙",
            "黄",
            "绿",
            "青",
            "蓝",
            "紫",
            "黑",
            "白"});
            this.cboColor.Location = new System.Drawing.Point(93, 136);
            this.cboColor.Name = "cboColor";
            this.cboColor.Size = new System.Drawing.Size(71, 20);
            this.cboColor.TabIndex = 11;
            this.cboColor.SelectedIndexChanged += new System.EventHandler(this.setCameraParam);
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(23, 136);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(29, 12);
            this.lblColor.TabIndex = 10;
            this.lblColor.Text = "颜色";
            // 
            // lblSpotShape
            // 
            this.lblSpotShape.AutoSize = true;
            this.lblSpotShape.Location = new System.Drawing.Point(23, 86);
            this.lblSpotShape.Name = "lblSpotShape";
            this.lblSpotShape.Size = new System.Drawing.Size(41, 12);
            this.lblSpotShape.TabIndex = 9;
            this.lblSpotShape.Text = "形  状";
            // 
            // gbxSpot
            // 
            this.gbxSpot.Controls.Add(this.txtSpotWidth);
            this.gbxSpot.Controls.Add(this.lblFociWidth);
            this.gbxSpot.Controls.Add(this.txtSpotHeight);
            this.gbxSpot.Controls.Add(this.lblFociHeight);
            this.gbxSpot.Location = new System.Drawing.Point(212, 12);
            this.gbxSpot.Name = "gbxSpot";
            this.gbxSpot.Size = new System.Drawing.Size(185, 129);
            this.gbxSpot.TabIndex = 22;
            this.gbxSpot.TabStop = false;
            this.gbxSpot.Text = "焦    斑";
            this.gbxSpot.Visible = false;
            // 
            // FrmVideoParam
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(783, 180);
            this.Controls.Add(this.gbxFocus);
            this.Controls.Add(this.gbxView);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.gbxSpot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVideoParam";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "视频参数";
            this.Load += new System.EventHandler(this.FrmVideoParam_Load);
            this.Shown += new System.EventHandler(this.VideoParamForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.txtFocusX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFocusY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScaleUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpotHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpotWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtViewHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtViewWidth)).EndInit();
            this.gbxView.ResumeLayout(false);
            this.gbxView.PerformLayout();
            this.gbxFocus.ResumeLayout(false);
            this.gbxFocus.PerformLayout();
            this.gbxSpot.ResumeLayout(false);
            this.gbxSpot.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFocusX;
        private System.Windows.Forms.Label lblFocusY;
        private System.Windows.Forms.Label lblScaleUnit;
        private System.Windows.Forms.Label lblFociHeight;
        private System.Windows.Forms.Label lblFociWidth;
        private System.Windows.Forms.Button btnAccept;
        public System.Windows.Forms.NumericUpDown txtFocusX;
        public System.Windows.Forms.NumericUpDown txtFocusY;
        public System.Windows.Forms.NumericUpDown txtScaleUnit;
        public System.Windows.Forms.NumericUpDown txtSpotHeight;
        public System.Windows.Forms.NumericUpDown txtSpotWidth;
        public System.Windows.Forms.RadioButton rbtnSpotShapeEllipse;
        public System.Windows.Forms.RadioButton rbtnSpotShapeRectangle;
        private System.Windows.Forms.Label lblViewHeight;
        public System.Windows.Forms.NumericUpDown txtViewHeight;
        private System.Windows.Forms.Label lblViewWidth;
        public System.Windows.Forms.NumericUpDown txtViewWidth;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.GroupBox gbxView;
        private System.Windows.Forms.GroupBox gbxFocus;
        private System.Windows.Forms.GroupBox gbxSpot;
        private System.Windows.Forms.Label lblSpotShape;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.ComboBox cboColor;
    }
}