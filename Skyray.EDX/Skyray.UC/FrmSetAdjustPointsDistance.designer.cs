namespace Skyray.UC
{
    partial class FrmSetAdjustPointsDistance
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
            this.txtDistance = new Skyray.Controls.TextBoxW();
            this.btnAccept = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.lblDistance = new Skyray.Controls.LabelW();
            this.lblDistanceUnit = new Skyray.Controls.LabelW();
            this.SuspendLayout();
            // 
            // txtDistance
            // 
            this.txtDistance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtDistance.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtDistance.Location = new System.Drawing.Point(87, 32);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(69, 21);
            this.txtDistance.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtDistance.TabIndex = 0;
            this.txtDistance.Text = "0";
            this.txtDistance.Leave += new System.EventHandler(this.txtDistance_Leave);
            // 
            // btnAccept
            // 
            this.btnAccept.bSilver = false;
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAccept.Location = new System.Drawing.Point(193, 17);
            this.btnAccept.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAccept.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAccept.TabIndex = 1;
            this.btnAccept.Text = "确  定";
            this.btnAccept.ToFocused = false;
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(193, 46);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取  消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.BackColor = System.Drawing.Color.Transparent;
            this.lblDistance.Location = new System.Drawing.Point(28, 35);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(35, 12);
            this.lblDistance.TabIndex = 2;
            this.lblDistance.Text = "距离:";
            // 
            // lblDistanceUnit
            // 
            this.lblDistanceUnit.AutoSize = true;
            this.lblDistanceUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblDistanceUnit.Location = new System.Drawing.Point(162, 35);
            this.lblDistanceUnit.Name = "lblDistanceUnit";
            this.lblDistanceUnit.Size = new System.Drawing.Size(17, 12);
            this.lblDistanceUnit.TabIndex = 3;
            this.lblDistanceUnit.Text = "mm";
            // 
            // FrmSetAdjustPointsDistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.lblDistanceUnit);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.txtDistance);
            this.Name = "FrmSetAdjustPointsDistance";
            this.Size = new System.Drawing.Size(304, 85);
            this.VisibleChanged += new System.EventHandler(this.FrmSetAdjustPointsDistance_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.TextBoxW txtDistance;
        private Skyray.Controls.ButtonW btnAccept;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.LabelW lblDistance;
        private Skyray.Controls.LabelW lblDistanceUnit;
    }
}