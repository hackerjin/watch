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
    public partial class UCWarningSettings : UCMultiple
    {
        public UCWarningSettings()
        {
            InitializeComponent();
        }

        private void UCWarningSettings_Load(object sender, EventArgs e)
        {
            if (WorkCurveHelper.IndiaAuWarning)
                radYes.Checked = true;
            else
                radNo.Checked = true;
            scrMax.Value = (int)WorkCurveHelper.IndiaAuWarningMax;
            numtime.Value = (decimal)WorkCurveHelper.IndiaAuWarningTime;
        }

        private void scrMax_ValueChanged(object sender, EventArgs e)
        {
            lblValue.Text = scrMax.Value + "%";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            WorkCurveHelper.IndiaAuWarning = radYes.Checked ? true : false;
            WorkCurveHelper.IndiaAuWarningMax = scrMax.Value;
            WorkCurveHelper.IndiaAuWarningTime = (int)numtime.Value;
            ReportTemplateHelper.SaveSpecifiedValue("IndiaAuWarning", "Warning", (WorkCurveHelper.IndiaAuWarning ? 1 : 0).ToString());
            ReportTemplateHelper.SaveSpecifiedValue("IndiaAuWarning", "Max", WorkCurveHelper.IndiaAuWarningMax.ToString());
            ReportTemplateHelper.SaveSpecifiedValue("IndiaAuWarning", "WarningTime", WorkCurveHelper.IndiaAuWarningTime.ToString());
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }
    }
}
