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
    public partial class UCDetectPointsSetting : Skyray.Language.UCMultiple
    {
        private string strPath = Application.StartupPath + "\\DBConnection.ini";

        //从文件中获取参数设置到文本框中
        public UCDetectPointsSetting()
        {
            InitializeComponent();

            
            chkDP.Checked =bool.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/detectPoints"));

            txtRoiRadius.Text = ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/roiRadius");
            txtMaxPixelErr.Text = ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/maxPixelErr");
            txtMaxDetectNum.Text = ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/maxDetectNum");
        }

        //保存文本框中的参数到本地文件中
        private void btDBOk_Click(object sender, EventArgs e)
        {
           

         

            WorkCurveHelper.detectPoints = this.chkDP.Checked;

            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/detectPoints", WorkCurveHelper.detectPoints.ToString());

            WorkCurveHelper.roiRadius = int.Parse(txtRoiRadius.Text);
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/roiRadius",txtRoiRadius.Text );

            WorkCurveHelper.maxPixelErr = int.Parse(txtMaxPixelErr.Text);
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/maxPixelErr", txtMaxPixelErr.Text);

            WorkCurveHelper.maxDetectNum = int.Parse(txtMaxDetectNum.Text);
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/maxDetectNum", txtMaxDetectNum.Text);

            DifferenceDevice.CurCameraControl.skyrayCamera1.ContiTestPoints.Clear();

            EDXRFHelper.GotoMainPage(this);


        }

        private void btDBDel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);

        }

      


    }
}
