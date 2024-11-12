using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;

namespace Skyray.UC
{
    public partial class UCNodeNameOrPath : UCMultiple
    {

        public string nodeName { get; set; }
        public string nodePath { get; set; }
        private List<FileAppSerialize> currentSeailize = null;
        

        public UCNodeNameOrPath()
        {
            InitializeComponent();
        }

        public UCNodeNameOrPath(List<FileAppSerialize> lst):this()
        {
            currentSeailize = lst;
        }

        private void btnExplore_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.openFileDialog1.ShowDialog())
                this.txtNodePath.Text = this.openFileDialog1.FileName;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Skyray.EDX.Common.ValidateHelper.IllegalCheck(this.txtNodeName)||currentSeailize.Exists(w=>w.NodeName.ToLower()==this.txtNodeName.Text.ToLower())||string.IsNullOrEmpty(this.txtNodePath.Text))
                return;
            nodeName = this.txtNodeName.Text;
            nodePath = this.txtNodePath.Text;
            this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }
    }
}
