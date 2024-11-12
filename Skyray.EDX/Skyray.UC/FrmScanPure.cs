using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.UC
{
    public partial class FrmScanPure : Skyray.Language.MultipleForm
    {
        private string lblString=string.Empty;
        public string TestInfo
        {
            get { return lblString; }
            set { lblString = value; }
        }
        public int CalibrateType
        {
            set {
                    if (value == 2)
                    {
                        btnSkip.Visible = false;
                       // btnOpenSpec.Visible = false;
                    }
            }
        } 
        public FrmScanPure()
        {
            InitializeComponent();
        }

        private void FrmScanPure_Load(object sender, EventArgs e)
        {
            lblPureAlertInfo.Text = lblString;
        }

        public void RefreshText(string Info)
        {
            lblString = Info;
            lblPureAlertInfo.Text = lblString;
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOpenSpec_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnExist_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

    }
}
