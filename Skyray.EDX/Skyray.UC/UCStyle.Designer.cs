namespace Skyray.UC
{
    partial class UCStyle
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
            this.containerObject1 = new Skyray.EDX.Common.ContainerObject();
            this.btwCancel = new Skyray.Controls.ButtonW();
            this.btWSubmit = new Skyray.Controls.ButtonW();
            this.lbStyleList = new Skyray.Controls.LabelW();
            this.cbStyleList = new Skyray.Controls.ComboBoxW();
            this.containerObject1.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerObject1
            // 
            this.containerObject1.AutoScroll = true;
            this.containerObject1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.containerObject1.BigImage = null;
            this.containerObject1.ContainerAttribute = false;
            this.containerObject1.ContainerLabel = null;
            this.containerObject1.ControlInternal = 0;
            this.containerObject1.Controls.Add(this.btwCancel);
            this.containerObject1.Controls.Add(this.btWSubmit);
            this.containerObject1.Controls.Add(this.lbStyleList);
            this.containerObject1.Controls.Add(this.cbStyleList);
            this.containerObject1.CurrentPanelIndex = 0;
            this.containerObject1.CurrentPlanningNumber = 0;
            this.containerObject1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerObject1.IncludeInnerCoordinate = false;
            this.containerObject1.IsReverseEmbeded = false;
            this.containerObject1.Location = new System.Drawing.Point(8, 8);
            this.containerObject1.Name = "containerObject1";
            this.containerObject1.Size = new System.Drawing.Size(331, 103);
            this.containerObject1.SmallImage = null;
            this.containerObject1.Style = Skyray.Controls.Style.Custom;
            this.containerObject1.TabIndex = 2;
            // 
            // btwCancel
            // 
            this.btwCancel.bSilver = false;
            this.btwCancel.Location = new System.Drawing.Point(197, 76);
            this.btwCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btwCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btwCancel.Name = "btwCancel";
            this.btwCancel.Size = new System.Drawing.Size(75, 23);
            this.btwCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btwCancel.TabIndex = 5;
            this.btwCancel.Text = "取消";
            this.btwCancel.ToFocused = false;
            this.btwCancel.UseVisualStyleBackColor = true;
            this.btwCancel.Click += new System.EventHandler(this.btwCancel_Click);
            // 
            // btWSubmit
            // 
            this.btWSubmit.bSilver = false;
            this.btWSubmit.Location = new System.Drawing.Point(96, 76);
            this.btWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.btWSubmit.Name = "btWSubmit";
            this.btWSubmit.Size = new System.Drawing.Size(75, 23);
            this.btWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.btWSubmit.TabIndex = 4;
            this.btWSubmit.Text = "确定";
            this.btWSubmit.ToFocused = false;
            this.btWSubmit.UseVisualStyleBackColor = true;
            this.btWSubmit.Click += new System.EventHandler(this.btWSubmit_Click);
            // 
            // lbStyleList
            // 
            this.lbStyleList.AutoSize = true;
            this.lbStyleList.BackColor = System.Drawing.Color.Transparent;
            this.lbStyleList.Location = new System.Drawing.Point(25, 33);
            this.lbStyleList.Name = "lbStyleList";
            this.lbStyleList.Size = new System.Drawing.Size(53, 12);
            this.lbStyleList.TabIndex = 3;
            this.lbStyleList.Text = "风格列表";
            // 
            // cbStyleList
            // 
            this.cbStyleList.AutoComplete = false;
            this.cbStyleList.AutoDropdown = false;
            this.cbStyleList.BackColorEven = System.Drawing.Color.White;
            this.cbStyleList.BackColorOdd = System.Drawing.Color.White;
            this.cbStyleList.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cbStyleList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cbStyleList.ColumnNames = "";
            this.cbStyleList.ColumnWidthDefault = 75;
            this.cbStyleList.ColumnWidths = "";
            this.cbStyleList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbStyleList.FormattingEnabled = true;
            this.cbStyleList.LinkedColumnIndex = 0;
            this.cbStyleList.LinkedTextBox = null;
            this.cbStyleList.Location = new System.Drawing.Point(96, 30);
            this.cbStyleList.Name = "cbStyleList";
            this.cbStyleList.Size = new System.Drawing.Size(121, 22);
            this.cbStyleList.TabIndex = 2;
            // 
            // UCStyle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.containerObject1);
            this.Name = "UCStyle";
            this.Size = new System.Drawing.Size(347, 119);
            this.containerObject1.ResumeLayout(false);
            this.containerObject1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Skyray.EDX.Common.ContainerObject containerObject1;
        private Skyray.Controls.ButtonW btwCancel;
        private Skyray.Controls.ButtonW btWSubmit;
        private Skyray.Controls.LabelW lbStyleList;
        private Skyray.Controls.ComboBoxW cbStyleList;

    }
}
