using System.Windows.Forms;

namespace Skyray.Controls
{
    /// <summary>
    /// Summary description for SplashForm.
    /// </summary>
    public class FrmSplash : System.Windows.Forms.Form
    {
        public Label lblStatusInfo;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public FrmSplash()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public string StatusInfo
        {
            set
            {
                _StatusInfo = value;
                ChangeStatusText();
            }
            get
            {
                return _StatusInfo;
            }
        }

        public void ChangeStatusText()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(this.ChangeStatusText));
                    return;
                }

                lblStatusInfo.Text = _StatusInfo;
            }
            catch
            {
                //	do something here...
            }
        }


        private string _StatusInfo = "";

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblStatusInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStatusInfo
            // 
            this.lblStatusInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusInfo.Font = new System.Drawing.Font("宋体", 10F);
            this.lblStatusInfo.Location = new System.Drawing.Point(46, 60);
            this.lblStatusInfo.Name = "lblStatusInfo";
            this.lblStatusInfo.Size = new System.Drawing.Size(457, 24);
            this.lblStatusInfo.TabIndex = 1;
            this.lblStatusInfo.Text = "系统初始化中...";
            // 
            // FrmSplash
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(528, 336);
            this.Controls.Add(this.lblStatusInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSplash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }
        #endregion
    }
}
