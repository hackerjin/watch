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
    public partial class UCDBSetting : Skyray.Language.UCMultiple
    {
        private string strPath = Application.StartupPath + "\\DBConnection.ini";

        //从文件中获取参数设置到文本框中
        public UCDBSetting()
        {
            InitializeComponent();

            System.Text.StringBuilder tempbuilder = new System.Text.StringBuilder(255);


            Skyray.API.WinMethod.GetPrivateProfileString("Param", "DataSource", "127.0.0.1", tempbuilder, 255, strPath);
            string DBName = tempbuilder.ToString();

            Skyray.API.WinMethod.GetPrivateProfileString("Param", "InitialCatalog", "EDXRFDB", tempbuilder, 255, strPath);
            string IP = tempbuilder.ToString();

            Skyray.API.WinMethod.GetPrivateProfileString("Param", "UserID", "sa", tempbuilder, 255, strPath);
            string User = tempbuilder.ToString();

            Skyray.API.WinMethod.GetPrivateProfileString("Param", "password", "123", tempbuilder, 255, strPath);
            string Pwd = tempbuilder.ToString();

            textIP.Text = DBName;

            textDB.Text = IP;

            textUser.Text = User;

            textPwd.Text = Pwd;

            string isDBOpen = ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/IsDBOpen");
            if (isDBOpen == "False")
                chkDB.Checked = false;
            else
                chkDB.Checked = true;


        }

        //保存文本框中的参数到本地文件中
        private void btDBOk_Click(object sender, EventArgs e)
        {

            Skyray.API.WinMethod.WritePrivateProfileString("Param", "DataSource", textIP.Text, strPath);

            Skyray.API.WinMethod.WritePrivateProfileString("Param", "InitialCatalog", textDB.Text, strPath);

            Skyray.API.WinMethod.WritePrivateProfileString("Param", "UserID", textUser.Text, strPath);

            Skyray.API.WinMethod.WritePrivateProfileString("Param", "password", textPwd.Text, strPath);

            WorkCurveHelper.IsDBOpen = chkDB.Checked;

            UCHistoryRecord temp = (UCHistoryRecord)WorkCurveHelper.ucHistoryRecord1;

            if (WorkCurveHelper.IsDBOpen)
                temp.btnExportSQL1.Visible = true;
            else
                temp.btnExportSQL1.Visible = false;

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/IsDBOpen", WorkCurveHelper.IsDBOpen.ToString());

            EDXRFHelper.GotoMainPage(this);


        }

        private void btDBDel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);

        }


    }
}
