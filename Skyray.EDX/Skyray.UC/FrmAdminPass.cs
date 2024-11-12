using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
namespace Skyray.UC
{
    public partial class FrmAdminPass : Skyray.Language.MultipleForm
    {
        public FrmAdminPass()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void btAdminPass_Click(object sender, EventArgs e)
        {


            if (textAdminPass.Text == "skyray")
                WorkCurveHelper.AdminPass = true;
            else
            {
                WorkCurveHelper.AdminPass = false;
                ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/ChangeUser", false.ToString());
                Msg.Show("管理员密码错误");
            }
            Close();
        }
    }
}
