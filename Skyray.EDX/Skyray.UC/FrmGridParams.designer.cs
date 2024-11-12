namespace Skyray.UC
{
    partial class FrmGridParams
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
            this.grouperRow = new Skyray.Controls.Grouper();
            this.lblRowUnit = new System.Windows.Forms.Label();
            this.txtRowPointDistance = new Skyray.Controls.TextBoxW();
            this.lblRowPointDistance = new Skyray.Controls.LabelW();
            this.numRowPoint = new Skyray.Controls.NumricUpDownW();
            this.lblRowPoint = new Skyray.Controls.LabelW();
            this.grouperColumn = new Skyray.Controls.Grouper();
            this.lblColumnUnit = new System.Windows.Forms.Label();
            this.txtColumnPointDistance = new Skyray.Controls.TextBoxW();
            this.lblColumnPointDistance = new Skyray.Controls.LabelW();
            this.numColumnPoint = new Skyray.Controls.NumricUpDownW();
            this.lblColumnPoint = new Skyray.Controls.LabelW();
            this.buttonWSubmit = new Skyray.Controls.ButtonW();
            this.buttonWCancel = new Skyray.Controls.ButtonW();
            this.grouperRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRowPoint)).BeginInit();
            this.grouperColumn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numColumnPoint)).BeginInit();
            this.SuspendLayout();
            // 
            // grouperRow
            // 
            this.grouperRow.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouperRow.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouperRow.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouperRow.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grouperRow.BorderThickness = 1F;
            this.grouperRow.BorderTopOnly = false;
            this.grouperRow.Controls.Add(this.lblRowUnit);
            this.grouperRow.Controls.Add(this.txtRowPointDistance);
            this.grouperRow.Controls.Add(this.lblRowPointDistance);
            this.grouperRow.Controls.Add(this.numRowPoint);
            this.grouperRow.Controls.Add(this.lblRowPoint);
            this.grouperRow.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grouperRow.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grouperRow.GroupImage = null;
            this.grouperRow.GroupTitle = "行上的点";
            this.grouperRow.HeaderRoundCorners = 4;
            this.grouperRow.Location = new System.Drawing.Point(21, 13);
            this.grouperRow.Name = "grouperRow";
            this.grouperRow.PaintGroupBox = false;
            this.grouperRow.RoundCorners = 4;
            this.grouperRow.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouperRow.ShadowControl = false;
            this.grouperRow.ShadowThickness = 3;
            this.grouperRow.Size = new System.Drawing.Size(327, 57);
            this.grouperRow.TabIndex = 0;
            this.grouperRow.TextLineSpace = 2;
            this.grouperRow.TitleLeftSpace = 18;
            // 
            // lblRowUnit
            // 
            this.lblRowUnit.AutoSize = true;
            this.lblRowUnit.Location = new System.Drawing.Point(301, 30);
            this.lblRowUnit.Name = "lblRowUnit";
            this.lblRowUnit.Size = new System.Drawing.Size(17, 12);
            this.lblRowUnit.TabIndex = 4;
            this.lblRowUnit.Text = "mm";
            // 
            // txtRowPointDistance
            // 
            this.txtRowPointDistance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtRowPointDistance.Location = new System.Drawing.Point(206, 27);
            this.txtRowPointDistance.Name = "txtRowPointDistance";
            this.txtRowPointDistance.Size = new System.Drawing.Size(88, 21);
            this.txtRowPointDistance.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtRowPointDistance.TabIndex = 3;
            // 
            // lblRowPointDistance
            // 
            this.lblRowPointDistance.AutoSize = true;
            this.lblRowPointDistance.BackColor = System.Drawing.Color.Transparent;
            this.lblRowPointDistance.Location = new System.Drawing.Point(143, 30);
            this.lblRowPointDistance.Name = "lblRowPointDistance";
            this.lblRowPointDistance.Size = new System.Drawing.Size(41, 12);
            this.lblRowPointDistance.TabIndex = 2;
            this.lblRowPointDistance.Text = "点距：";
            // 
            // numRowPoint
            // 
            this.numRowPoint.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numRowPoint.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numRowPoint.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numRowPoint.Location = new System.Drawing.Point(51, 28);
            this.numRowPoint.Name = "numRowPoint";
            this.numRowPoint.Size = new System.Drawing.Size(86, 21);
            this.numRowPoint.TabIndex = 1;
            // 
            // lblRowPoint
            // 
            this.lblRowPoint.AutoSize = true;
            this.lblRowPoint.BackColor = System.Drawing.Color.Transparent;
            this.lblRowPoint.Location = new System.Drawing.Point(4, 30);
            this.lblRowPoint.Name = "lblRowPoint";
            this.lblRowPoint.Size = new System.Drawing.Size(41, 12);
            this.lblRowPoint.TabIndex = 0;
            this.lblRowPoint.Text = "点数：";
            // 
            // grouperColumn
            // 
            this.grouperColumn.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouperColumn.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouperColumn.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouperColumn.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grouperColumn.BorderThickness = 1F;
            this.grouperColumn.BorderTopOnly = false;
            this.grouperColumn.Controls.Add(this.lblColumnUnit);
            this.grouperColumn.Controls.Add(this.txtColumnPointDistance);
            this.grouperColumn.Controls.Add(this.lblColumnPointDistance);
            this.grouperColumn.Controls.Add(this.numColumnPoint);
            this.grouperColumn.Controls.Add(this.lblColumnPoint);
            this.grouperColumn.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grouperColumn.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grouperColumn.GroupImage = null;
            this.grouperColumn.GroupTitle = "列上的点";
            this.grouperColumn.HeaderRoundCorners = 4;
            this.grouperColumn.Location = new System.Drawing.Point(21, 76);
            this.grouperColumn.Name = "grouperColumn";
            this.grouperColumn.PaintGroupBox = false;
            this.grouperColumn.RoundCorners = 4;
            this.grouperColumn.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouperColumn.ShadowControl = false;
            this.grouperColumn.ShadowThickness = 3;
            this.grouperColumn.Size = new System.Drawing.Size(327, 64);
            this.grouperColumn.TabIndex = 1;
            this.grouperColumn.TextLineSpace = 2;
            this.grouperColumn.TitleLeftSpace = 18;
            // 
            // lblColumnUnit
            // 
            this.lblColumnUnit.AutoSize = true;
            this.lblColumnUnit.Location = new System.Drawing.Point(301, 33);
            this.lblColumnUnit.Name = "lblColumnUnit";
            this.lblColumnUnit.Size = new System.Drawing.Size(17, 12);
            this.lblColumnUnit.TabIndex = 5;
            this.lblColumnUnit.Text = "mm";
            // 
            // txtColumnPointDistance
            // 
            this.txtColumnPointDistance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtColumnPointDistance.Location = new System.Drawing.Point(206, 30);
            this.txtColumnPointDistance.Name = "txtColumnPointDistance";
            this.txtColumnPointDistance.Size = new System.Drawing.Size(88, 21);
            this.txtColumnPointDistance.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtColumnPointDistance.TabIndex = 3;
            // 
            // lblColumnPointDistance
            // 
            this.lblColumnPointDistance.AutoSize = true;
            this.lblColumnPointDistance.BackColor = System.Drawing.Color.Transparent;
            this.lblColumnPointDistance.Location = new System.Drawing.Point(143, 30);
            this.lblColumnPointDistance.Name = "lblColumnPointDistance";
            this.lblColumnPointDistance.Size = new System.Drawing.Size(41, 12);
            this.lblColumnPointDistance.TabIndex = 2;
            this.lblColumnPointDistance.Text = "点距：";
            // 
            // numColumnPoint
            // 
            this.numColumnPoint.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numColumnPoint.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numColumnPoint.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numColumnPoint.Location = new System.Drawing.Point(51, 28);
            this.numColumnPoint.Name = "numColumnPoint";
            this.numColumnPoint.Size = new System.Drawing.Size(86, 21);
            this.numColumnPoint.TabIndex = 1;
            // 
            // lblColumnPoint
            // 
            this.lblColumnPoint.AutoSize = true;
            this.lblColumnPoint.BackColor = System.Drawing.Color.Transparent;
            this.lblColumnPoint.Location = new System.Drawing.Point(4, 30);
            this.lblColumnPoint.Name = "lblColumnPoint";
            this.lblColumnPoint.Size = new System.Drawing.Size(41, 12);
            this.lblColumnPoint.TabIndex = 0;
            this.lblColumnPoint.Text = "点数：";
            // 
            // buttonWSubmit
            // 
            this.buttonWSubmit.bSilver = false;
            this.buttonWSubmit.Location = new System.Drawing.Point(354, 24);
            this.buttonWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.Name = "buttonWSubmit";
            this.buttonWSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWSubmit.TabIndex = 2;
            this.buttonWSubmit.Text = "确定";
            this.buttonWSubmit.ToFocused = false;
            this.buttonWSubmit.UseVisualStyleBackColor = true;
            this.buttonWSubmit.Click += new System.EventHandler(this.buttonWSubmit_Click);
            // 
            // buttonWCancel
            // 
            this.buttonWCancel.bSilver = false;
            this.buttonWCancel.Location = new System.Drawing.Point(354, 60);
            this.buttonWCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWCancel.Name = "buttonWCancel";
            this.buttonWCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonWCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWCancel.TabIndex = 3;
            this.buttonWCancel.Text = "取消";
            this.buttonWCancel.ToFocused = false;
            this.buttonWCancel.UseVisualStyleBackColor = true;
            this.buttonWCancel.Click += new System.EventHandler(this.buttonWCancel_Click);
            // 
            // FrmGridParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.buttonWCancel);
            this.Controls.Add(this.buttonWSubmit);
            this.Controls.Add(this.grouperColumn);
            this.Controls.Add(this.grouperRow);
            this.Name = "FrmGridParams";
            this.Size = new System.Drawing.Size(446, 159);
            this.grouperRow.ResumeLayout(false);
            this.grouperRow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRowPoint)).EndInit();
            this.grouperColumn.ResumeLayout(false);
            this.grouperColumn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numColumnPoint)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.Controls.Grouper grouperRow;
        private Skyray.Controls.TextBoxW txtRowPointDistance;
        private Skyray.Controls.LabelW lblRowPointDistance;
        private Skyray.Controls.NumricUpDownW numRowPoint;
        private Skyray.Controls.LabelW lblRowPoint;
        private Skyray.Controls.Grouper grouperColumn;
        private Skyray.Controls.TextBoxW txtColumnPointDistance;
        private Skyray.Controls.LabelW lblColumnPointDistance;
        private Skyray.Controls.NumricUpDownW numColumnPoint;
        private Skyray.Controls.LabelW lblColumnPoint;
        private Skyray.Controls.ButtonW buttonWSubmit;
        private Skyray.Controls.ButtonW buttonWCancel;
        private System.Windows.Forms.Label lblRowUnit;
        private System.Windows.Forms.Label lblColumnUnit;
    }
}