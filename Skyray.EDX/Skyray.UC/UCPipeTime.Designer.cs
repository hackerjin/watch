namespace Skyray.UC
{
    partial class UCPipeTime
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
            Skyray.Controls.Office2007Renderer office2007Renderer1 = new Skyray.Controls.Office2007Renderer();
            this.containerObject1 = new Skyray.EDX.Common.ContainerObject();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.containerObject1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // containerObject1
            // 
            this.containerObject1.AutoScroll = true;
            this.containerObject1.BigImage = null;
            this.containerObject1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.containerObject1.ContainerAttribute = false;
            this.containerObject1.ContainerLabel = null;
            this.containerObject1.ControlInternal = 0;
            this.containerObject1.Controls.Add(this.pictureBox1);
            this.containerObject1.CurrentPanelIndex = 0;
            this.containerObject1.CurrentPlanningNumber = 0;
            this.containerObject1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerObject1.IncludeInnerCoordinate = false;
            this.containerObject1.IsReverseEmbeded = false;
            this.containerObject1.Location = new System.Drawing.Point(0, 0);
            this.containerObject1.Name = "containerObject1";
            this.containerObject1.Name1 = null;
            office2007Renderer1.RoundedEdges = true;
            this.containerObject1.Renderer = office2007Renderer1;
            this.containerObject1.Size = new System.Drawing.Size(84, 86);
            this.containerObject1.SmallImage = null;
            this.containerObject1.Style = Skyray.Controls.Style.Custom;
            this.containerObject1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Skyray.UC.Properties.Resources.Deavtive;
            this.pictureBox1.Location = new System.Drawing.Point(16, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // UCPipeTime
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.containerObject1);
            this.Name = "UCPipeTime";
            this.Size = new System.Drawing.Size(84, 86);
            this.containerObject1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private Skyray.EDX.Common.ContainerObject containerObject1;
    }
}
