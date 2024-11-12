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
    public partial class UCReportPath : UCMultiple
    {
        public UCReportPath()
        {
            InitializeComponent();
            string path = ReportTemplateHelper.LoadSpecifiedValue("ExcelPath", "Path");
            if (string.IsNullOrEmpty(path))
            {
                this.txtBoxPath.Text = Application.StartupPath + "\\Report";
                ReportTemplateHelper.SaveSpecifiedValueandCreate("ExcelPath", "Path", this.txtBoxPath.Text);
                WorkCurveHelper.ExcelPath = this.txtBoxPath.Text;


            }
            else
                this.txtBoxPath.Text = ReportTemplateHelper.LoadSpecifiedValue("ExcelPath", "Path");
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.txtBoxPath.Text.IsNullOrEmpty())
                return;
            ReportTemplateHelper.SaveSpecifiedValueandCreate("ExcelPath", "Path", this.txtBoxPath.Text);
            WorkCurveHelper.ExcelPath = this.txtBoxPath.Text;
            EDXRFHelper.GotoMainPage(this);
        }

        private void btwCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void btWSelect_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.folderBrowserDialog1.ShowDialog())
            {
                this.txtBoxPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void txtBoxPath_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
