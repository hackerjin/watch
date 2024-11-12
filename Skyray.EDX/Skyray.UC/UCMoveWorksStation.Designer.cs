namespace Skyray.UC
{
    partial class UCMoveWorksStation
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkIsLazerOpen = new Skyray.Controls.CheckBoxW();
            this.buttonCancel = new Skyray.Controls.ButtonW();
            this.gbxZFocal = new System.Windows.Forms.GroupBox();
            this.lblZSpeed = new Skyray.Controls.LabelW();
            this.btnZAxisOpen = new System.Windows.Forms.Button();
            this.btnAutoFocal = new Skyray.Controls.ButtonW();
            this.btnZAxisClose = new System.Windows.Forms.Button();
            this.vScrollZSpeed = new System.Windows.Forms.VScrollBar();
            this.lblZSlow = new Skyray.Controls.LabelW();
            this.lblZFast = new Skyray.Controls.LabelW();
            this.buttonSubmit = new Skyray.Controls.ButtonW();
            this.buttonReset = new Skyray.Controls.ButtonW();
            this.gbxXYTable = new System.Windows.Forms.GroupBox();
            this.btnYAxisOut = new System.Windows.Forms.Button();
            this.btnXAxisRight = new System.Windows.Forms.Button();
            this.lblXYSpeed = new System.Windows.Forms.Label();
            this.btnXAxisLeft = new System.Windows.Forms.Button();
            this.vScrollXYSpeed = new System.Windows.Forms.VScrollBar();
            this.btnYAxisIn = new System.Windows.Forms.Button();
            this.lblXYSlow = new Skyray.Controls.LabelW();
            this.lblXYFast = new Skyray.Controls.LabelW();
            this.groupBoxFixWalk = new System.Windows.Forms.GroupBox();
            this.textBoxWInputWalk = new Skyray.Controls.NumricUpDownW();
            this.checkBoxWalk = new Skyray.Controls.CheckBoxW();
            this.buttonStop = new Skyray.Controls.ButtonW();
            this.panel2.SuspendLayout();
            this.gbxZFocal.SuspendLayout();
            this.gbxXYTable.SuspendLayout();
            this.groupBoxFixWalk.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWInputWalk)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.chkIsLazerOpen);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.gbxZFocal);
            this.panel2.Controls.Add(this.buttonSubmit);
            this.panel2.Controls.Add(this.buttonReset);
            this.panel2.Controls.Add(this.gbxXYTable);
            this.panel2.Controls.Add(this.groupBoxFixWalk);
            this.panel2.Controls.Add(this.buttonStop);
            this.panel2.Location = new System.Drawing.Point(8, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(778, 209);
            this.panel2.TabIndex = 30;
            // 
            // chkIsLazerOpen
            // 
            this.chkIsLazerOpen.AutoSize = true;
            this.chkIsLazerOpen.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkIsLazerOpen.Location = new System.Drawing.Point(662, 79);
            this.chkIsLazerOpen.Name = "chkIsLazerOpen";
            this.chkIsLazerOpen.Size = new System.Drawing.Size(72, 16);
            this.chkIsLazerOpen.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkIsLazerOpen.TabIndex = 19;
            this.chkIsLazerOpen.Text = "高度激光";
            this.chkIsLazerOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkIsLazerOpen.UseVisualStyleBackColor = true;
            this.chkIsLazerOpen.Visible = false;
            this.chkIsLazerOpen.Click += new System.EventHandler(this.chkIsLazerOpen_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.bSilver = false;
            this.buttonCancel.Location = new System.Drawing.Point(662, 147);
            this.buttonCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(88, 24);
            this.buttonCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonCancel.TabIndex = 30;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.ToFocused = false;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // gbxZFocal
            // 
            this.gbxZFocal.Controls.Add(this.lblZSpeed);
            this.gbxZFocal.Controls.Add(this.btnZAxisOpen);
            this.gbxZFocal.Controls.Add(this.btnAutoFocal);
            this.gbxZFocal.Controls.Add(this.btnZAxisClose);
            this.gbxZFocal.Controls.Add(this.vScrollZSpeed);
            this.gbxZFocal.Controls.Add(this.lblZSlow);
            this.gbxZFocal.Controls.Add(this.lblZFast);
            this.gbxZFocal.Location = new System.Drawing.Point(265, 3);
            this.gbxZFocal.Name = "gbxZFocal";
            this.gbxZFocal.Size = new System.Drawing.Size(248, 185);
            this.gbxZFocal.TabIndex = 25;
            this.gbxZFocal.TabStop = false;
            this.gbxZFocal.Text = "Z轴 移动/聚焦";
            // 
            // lblZSpeed
            // 
            this.lblZSpeed.AutoSize = true;
            this.lblZSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblZSpeed.Location = new System.Drawing.Point(222, 87);
            this.lblZSpeed.Name = "lblZSpeed";
            this.lblZSpeed.Size = new System.Drawing.Size(17, 12);
            this.lblZSpeed.TabIndex = 18;
            this.lblZSpeed.Text = "10";
            // 
            // btnZAxisOpen
            // 
            this.btnZAxisOpen.Image = global::Skyray.UC.Properties.Resources.Top;
            this.btnZAxisOpen.Location = new System.Drawing.Point(25, 53);
            this.btnZAxisOpen.Margin = new System.Windows.Forms.Padding(0);
            this.btnZAxisOpen.Name = "btnZAxisOpen";
            this.btnZAxisOpen.Size = new System.Drawing.Size(51, 70);
            this.btnZAxisOpen.TabIndex = 13;
            this.btnZAxisOpen.UseVisualStyleBackColor = true;
            this.btnZAxisOpen.Click += new System.EventHandler(this.btnZAxisOpen_Click);
            this.btnZAxisOpen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnZAxisOpen_MouseDown);
            this.btnZAxisOpen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnZAxisOpen_MouseUp);
            // 
            // btnAutoFocal
            // 
            this.btnAutoFocal.bSilver = false;
            this.btnAutoFocal.Location = new System.Drawing.Point(25, 141);
            this.btnAutoFocal.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoFocal.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAutoFocal.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAutoFocal.Name = "btnAutoFocal";
            this.btnAutoFocal.Size = new System.Drawing.Size(117, 24);
            this.btnAutoFocal.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAutoFocal.TabIndex = 13;
            this.btnAutoFocal.Text = "自动调节";
            this.btnAutoFocal.ToFocused = false;
            this.btnAutoFocal.UseVisualStyleBackColor = true;
            this.btnAutoFocal.Visible = false;
            this.btnAutoFocal.Click += new System.EventHandler(this.btnAutoFocal_Click);
            // 
            // btnZAxisClose
            // 
            this.btnZAxisClose.Image = global::Skyray.UC.Properties.Resources.Bottom;
            this.btnZAxisClose.Location = new System.Drawing.Point(115, 53);
            this.btnZAxisClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnZAxisClose.Name = "btnZAxisClose";
            this.btnZAxisClose.Size = new System.Drawing.Size(51, 70);
            this.btnZAxisClose.TabIndex = 13;
            this.btnZAxisClose.UseVisualStyleBackColor = true;
            this.btnZAxisClose.Click += new System.EventHandler(this.btnZAxisClose_Click);
            this.btnZAxisClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnZAxisClose_MouseDown);
            this.btnZAxisClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnZAxisClose_MouseUp);
            // 
            // vScrollZSpeed
            // 
            this.vScrollZSpeed.Location = new System.Drawing.Point(191, 18);
            this.vScrollZSpeed.Maximum = 154;
            this.vScrollZSpeed.Minimum = 20;
            this.vScrollZSpeed.Name = "vScrollZSpeed";
            this.vScrollZSpeed.Size = new System.Drawing.Size(19, 147);
            this.vScrollZSpeed.TabIndex = 17;
            this.vScrollZSpeed.Value = 150;
            this.vScrollZSpeed.ValueChanged += new System.EventHandler(this.vScrollZSpeed_ValueChanged);
            // 
            // lblZSlow
            // 
            this.lblZSlow.AutoSize = true;
            this.lblZSlow.BackColor = System.Drawing.Color.Transparent;
            this.lblZSlow.Location = new System.Drawing.Point(213, 141);
            this.lblZSlow.Name = "lblZSlow";
            this.lblZSlow.Size = new System.Drawing.Size(17, 12);
            this.lblZSlow.TabIndex = 12;
            this.lblZSlow.Text = "慢";
            this.lblZSlow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblZFast
            // 
            this.lblZFast.AutoSize = true;
            this.lblZFast.BackColor = System.Drawing.Color.Transparent;
            this.lblZFast.Location = new System.Drawing.Point(213, 30);
            this.lblZFast.Name = "lblZFast";
            this.lblZFast.Size = new System.Drawing.Size(17, 12);
            this.lblZFast.TabIndex = 12;
            this.lblZFast.Text = "快";
            this.lblZFast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.bSilver = false;
            this.buttonSubmit.Location = new System.Drawing.Point(662, 101);
            this.buttonSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(88, 24);
            this.buttonSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonSubmit.TabIndex = 29;
            this.buttonSubmit.Text = "确定";
            this.buttonSubmit.ToFocused = false;
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.bSilver = false;
            this.buttonReset.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReset.Image = global::Skyray.UC.Properties.Resources.reset;
            this.buttonReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonReset.ImageLocation = Skyray.Controls.ButtonW.e_imagelocation.Left;
            this.buttonReset.Location = new System.Drawing.Point(543, 147);
            this.buttonReset.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonReset.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.buttonReset.Size = new System.Drawing.Size(88, 24);
            this.buttonReset.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonReset.TabIndex = 28;
            this.buttonReset.Text = "复位";
            this.buttonReset.ToFocused = false;
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // gbxXYTable
            // 
            this.gbxXYTable.Controls.Add(this.btnYAxisOut);
            this.gbxXYTable.Controls.Add(this.btnXAxisRight);
            this.gbxXYTable.Controls.Add(this.lblXYSpeed);
            this.gbxXYTable.Controls.Add(this.btnXAxisLeft);
            this.gbxXYTable.Controls.Add(this.vScrollXYSpeed);
            this.gbxXYTable.Controls.Add(this.btnYAxisIn);
            this.gbxXYTable.Controls.Add(this.lblXYSlow);
            this.gbxXYTable.Controls.Add(this.lblXYFast);
            this.gbxXYTable.Location = new System.Drawing.Point(2, 3);
            this.gbxXYTable.Name = "gbxXYTable";
            this.gbxXYTable.Size = new System.Drawing.Size(257, 185);
            this.gbxXYTable.TabIndex = 16;
            this.gbxXYTable.TabStop = false;
            this.gbxXYTable.Text = "XY轴 移动";
            // 
            // btnYAxisOut
            // 
            this.btnYAxisOut.Image = global::Skyray.UC.Properties.Resources.down;
            this.btnYAxisOut.Location = new System.Drawing.Point(78, 117);
            this.btnYAxisOut.Name = "btnYAxisOut";
            this.btnYAxisOut.Size = new System.Drawing.Size(40, 40);
            this.btnYAxisOut.TabIndex = 19;
            this.btnYAxisOut.UseVisualStyleBackColor = true;
            this.btnYAxisOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYAxisOut_MouseDown);
            this.btnYAxisOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYAxisOut_MouseUp);
            // 
            // btnXAxisRight
            // 
            this.btnXAxisRight.Image = global::Skyray.UC.Properties.Resources.forward;
            this.btnXAxisRight.Location = new System.Drawing.Point(128, 71);
            this.btnXAxisRight.Margin = new System.Windows.Forms.Padding(0);
            this.btnXAxisRight.Name = "btnXAxisRight";
            this.btnXAxisRight.Size = new System.Drawing.Size(40, 40);
            this.btnXAxisRight.TabIndex = 10;
            this.btnXAxisRight.UseVisualStyleBackColor = true;
            this.btnXAxisRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnXAxisRight_MouseDown);
            this.btnXAxisRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnXAxisRight_MouseUp);
            // 
            // lblXYSpeed
            // 
            this.lblXYSpeed.AutoSize = true;
            this.lblXYSpeed.Location = new System.Drawing.Point(229, 87);
            this.lblXYSpeed.Name = "lblXYSpeed";
            this.lblXYSpeed.Size = new System.Drawing.Size(17, 12);
            this.lblXYSpeed.TabIndex = 18;
            this.lblXYSpeed.Text = "10";
            // 
            // btnXAxisLeft
            // 
            this.btnXAxisLeft.Image = global::Skyray.UC.Properties.Resources.back;
            this.btnXAxisLeft.Location = new System.Drawing.Point(26, 71);
            this.btnXAxisLeft.Margin = new System.Windows.Forms.Padding(0);
            this.btnXAxisLeft.Name = "btnXAxisLeft";
            this.btnXAxisLeft.Size = new System.Drawing.Size(40, 40);
            this.btnXAxisLeft.TabIndex = 10;
            this.btnXAxisLeft.UseVisualStyleBackColor = true;
            this.btnXAxisLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnXAxisLeft_MouseDown);
            this.btnXAxisLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnXAxisLeft_MouseUp);
            // 
            // vScrollXYSpeed
            // 
            this.vScrollXYSpeed.Location = new System.Drawing.Point(195, 18);
            this.vScrollXYSpeed.Maximum = 154;
            this.vScrollXYSpeed.Minimum = 80;
            this.vScrollXYSpeed.Name = "vScrollXYSpeed";
            this.vScrollXYSpeed.Size = new System.Drawing.Size(19, 147);
            this.vScrollXYSpeed.TabIndex = 17;
            this.vScrollXYSpeed.Value = 154;
            this.vScrollXYSpeed.ValueChanged += new System.EventHandler(this.vScrollXYSpeed_ValueChanged);
            // 
            // btnYAxisIn
            // 
            this.btnYAxisIn.Image = global::Skyray.UC.Properties.Resources.go_up;
            this.btnYAxisIn.Location = new System.Drawing.Point(78, 26);
            this.btnYAxisIn.Margin = new System.Windows.Forms.Padding(0);
            this.btnYAxisIn.Name = "btnYAxisIn";
            this.btnYAxisIn.Size = new System.Drawing.Size(40, 40);
            this.btnYAxisIn.TabIndex = 10;
            this.btnYAxisIn.UseVisualStyleBackColor = true;
            this.btnYAxisIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYAxisIn_MouseDown);
            this.btnYAxisIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYAxisIn_MouseUp);
            // 
            // lblXYSlow
            // 
            this.lblXYSlow.AutoSize = true;
            this.lblXYSlow.BackColor = System.Drawing.Color.Transparent;
            this.lblXYSlow.Location = new System.Drawing.Point(217, 141);
            this.lblXYSlow.Name = "lblXYSlow";
            this.lblXYSlow.Size = new System.Drawing.Size(17, 12);
            this.lblXYSlow.TabIndex = 12;
            this.lblXYSlow.Text = "慢";
            this.lblXYSlow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblXYFast
            // 
            this.lblXYFast.AutoSize = true;
            this.lblXYFast.BackColor = System.Drawing.Color.Transparent;
            this.lblXYFast.Location = new System.Drawing.Point(217, 33);
            this.lblXYFast.Name = "lblXYFast";
            this.lblXYFast.Size = new System.Drawing.Size(17, 12);
            this.lblXYFast.TabIndex = 12;
            this.lblXYFast.Text = "快";
            this.lblXYFast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxFixWalk
            // 
            this.groupBoxFixWalk.Controls.Add(this.textBoxWInputWalk);
            this.groupBoxFixWalk.Controls.Add(this.checkBoxWalk);
            this.groupBoxFixWalk.Location = new System.Drawing.Point(522, 3);
            this.groupBoxFixWalk.Name = "groupBoxFixWalk";
            this.groupBoxFixWalk.Size = new System.Drawing.Size(243, 66);
            this.groupBoxFixWalk.TabIndex = 26;
            this.groupBoxFixWalk.TabStop = false;
            this.groupBoxFixWalk.Text = "步数设置";
            // 
            // textBoxWInputWalk
            // 
            this.textBoxWInputWalk.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.textBoxWInputWalk.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.textBoxWInputWalk.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.textBoxWInputWalk.Location = new System.Drawing.Point(128, 26);
            this.textBoxWInputWalk.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.textBoxWInputWalk.Name = "textBoxWInputWalk";
            this.textBoxWInputWalk.Size = new System.Drawing.Size(100, 21);
            this.textBoxWInputWalk.TabIndex = 68;
            // 
            // checkBoxWalk
            // 
            this.checkBoxWalk.AutoSize = true;
            this.checkBoxWalk.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.checkBoxWalk.Location = new System.Drawing.Point(25, 29);
            this.checkBoxWalk.Name = "checkBoxWalk";
            this.checkBoxWalk.Size = new System.Drawing.Size(72, 16);
            this.checkBoxWalk.Style = Skyray.Controls.Style.Office2007Blue;
            this.checkBoxWalk.TabIndex = 23;
            this.checkBoxWalk.Text = "固定步数";
            this.checkBoxWalk.UseVisualStyleBackColor = true;
            // 
            // buttonStop
            // 
            this.buttonStop.bSilver = false;
            this.buttonStop.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonStop.Image = global::Skyray.UC.Properties.Resources.StopTest;
            this.buttonStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStop.ImageLocation = Skyray.Controls.ButtonW.e_imagelocation.Left;
            this.buttonStop.ImageOffset = 0;
            this.buttonStop.Location = new System.Drawing.Point(543, 101);
            this.buttonStop.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonStop.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.buttonStop.Size = new System.Drawing.Size(88, 24);
            this.buttonStop.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonStop.TabIndex = 27;
            this.buttonStop.Text = "停止";
            this.buttonStop.ToFocused = false;
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // UCMoveWorksStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.panel2);
            this.Name = "UCMoveWorksStation";
            this.Size = new System.Drawing.Size(797, 217);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gbxZFocal.ResumeLayout(false);
            this.gbxZFocal.PerformLayout();
            this.gbxXYTable.ResumeLayout(false);
            this.gbxXYTable.PerformLayout();
            this.groupBoxFixWalk.ResumeLayout(false);
            this.groupBoxFixWalk.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWInputWalk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox gbxXYTable;
        private System.Windows.Forms.Button btnYAxisOut;
        private System.Windows.Forms.Button btnXAxisRight;
        private System.Windows.Forms.Label lblXYSpeed;
        private System.Windows.Forms.Button btnXAxisLeft;
        private System.Windows.Forms.VScrollBar vScrollXYSpeed;

        private System.Windows.Forms.Button btnYAxisIn;
        private Skyray.Controls.LabelW lblXYSlow;
        private Skyray.Controls.LabelW lblXYFast;
        private Skyray.Controls.CheckBoxW checkBoxWalk;
        private System.Windows.Forms.GroupBox gbxZFocal;
        private Skyray.Controls.CheckBoxW chkIsLazerOpen;
        private Skyray.Controls.ButtonW btnAutoFocal;
        private Skyray.Controls.LabelW lblZSpeed;
        private System.Windows.Forms.Button btnZAxisOpen;
        private System.Windows.Forms.Button btnZAxisClose;
        private System.Windows.Forms.VScrollBar vScrollZSpeed;
        private Skyray.Controls.LabelW lblZSlow;
        private Skyray.Controls.LabelW lblZFast;
        private System.Windows.Forms.GroupBox groupBoxFixWalk;
        private Skyray.Controls.ButtonW buttonStop;
        private Skyray.Controls.ButtonW buttonReset;
        private Skyray.Controls.ButtonW buttonCancel;
        private Skyray.Controls.ButtonW buttonSubmit;
        private Skyray.Controls.NumricUpDownW textBoxWInputWalk;

    }
}
