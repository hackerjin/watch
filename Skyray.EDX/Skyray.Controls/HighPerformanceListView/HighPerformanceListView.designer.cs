namespace Skyray.Controls
{
    partial class HighPerformanceListView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HighPerformanceListView));
            this.listView = new System.Windows.Forms.ListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.hScrollBar = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.BackColor = System.Drawing.Color.GhostWhite;
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.OwnerDraw = true;
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(214, 151);
            this.listView.SmallImageList = this.imageList;
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            this.listView.Resize += new System.EventHandler(this.ListView_Resize);
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView_ColumnClick);
            //this.listView.Click += new System.EventHandler(this.listView_Click);
            this.listView.MouseUp += new System.Windows.Forms.MouseEventHandler(listView_MouseUp);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "down.png");
            this.imageList.Images.SetKeyName(1, "up.png");
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.hScrollBar.LargeChange = 1;
            this.hScrollBar.Location = new System.Drawing.Point(197, 0);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(17, 151);
            this.hScrollBar.TabIndex = 1;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // HighPerformanceListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hScrollBar);
            this.Controls.Add(this.listView);
            this.Name = "HighPerformanceListView";
            this.Size = new System.Drawing.Size(214, 151);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.VScrollBar hScrollBar;
    }
}
