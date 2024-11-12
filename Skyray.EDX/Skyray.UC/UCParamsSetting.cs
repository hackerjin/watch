using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCParamsSetting :UCMultiple
    {
        public UCParamsSetting()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            double temp = 0d;
            try
            {
                temp = double.Parse(this.txtMatchLevel.Text);
            }
            catch (Exception ex) { Msg.Show(ex.Message);
                                   return;
            }
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "MatchLevel", temp.ToString());
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void UCParamsSetting_Load(object sender, EventArgs e)
        {
            string str = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "MatchLevel");
            if (!string.IsNullOrEmpty(str))
                this.txtMatchLevel.Text = str;
        }
    }
}
