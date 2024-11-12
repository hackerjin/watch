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
    public partial class UCSysSettings : Skyray.Language.UCMultiple
    {
        public UCSysSettings()
        {
            InitializeComponent();
            chkThrInit.Checked = WorkCurveHelper.bShowInitParam;
            chkLightShutter.Checked = WorkCurveHelper.IsUseElect;
            chkPureEleUni.Checked = WorkCurveHelper.IsPureElemCurrentUnify;
            chkFixedDisXY.Checked = WorkCurveHelper.FixedDisXY;
            chkFixedDisZ.Checked = WorkCurveHelper.FixedDisZ;
            chkMultiReset.Checked = WorkCurveHelper.multiReset;
            chkFilterReset.Checked = WorkCurveHelper.filterReset;
            chkloopTestDemo.Checked = WorkCurveHelper.loopTestDemo;
            chkNormalizeSteps.Checked = WorkCurveHelper.normalizeSteps;

            string curMode = ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/FPThick/AxisZSpeedMode");
            if (curMode == "0")
                chkThrSpeeds.Checked = false;
            else
                chkThrSpeeds.Checked = true;
            if (WorkCurveHelper.matrixMode == "dotDot")
                comboMatrixMode.SelectedIndex = 0;
            else if (WorkCurveHelper.matrixMode == "matrixDot")
                comboMatrixMode.SelectedIndex = 1;
            else if (WorkCurveHelper.matrixMode == "dotMatrix")
                comboMatrixMode.SelectedIndex = 2; 

        }

        private void btnSysOk_Click(object sender, EventArgs e)
        {
            WorkCurveHelper.bShowInitParam = chkThrInit.Checked;
            WorkCurveHelper.IsUseElect = chkLightShutter.Checked;
            WorkCurveHelper.IsPureElemCurrentUnify=  chkPureEleUni.Checked;
            WorkCurveHelper.FixedDisXY = chkFixedDisXY.Checked;
            WorkCurveHelper.FixedDisZ = chkFixedDisZ.Checked;
            WorkCurveHelper.multiReset = chkMultiReset.Checked;
            WorkCurveHelper.filterReset = chkFilterReset.Checked;
            WorkCurveHelper.loopTestDemo = chkloopTestDemo.Checked;
            WorkCurveHelper.normalizeSteps = chkNormalizeSteps.Checked;

            UCCameraControl curCamera = DifferenceDevice.CurCameraControl;

            if (chkFixedDisXY.Checked)
            {
                if (Language.Lang.Model.CurrentLang.FullName == "中文")
                {
                    curCamera.ChkFixedXY.Text = "固定距离(XY)";
                }
                else if (Language.Lang.Model.CurrentLang.FullName == "English")
                    curCamera.ChkFixedXY.Text = "FixedDis(XY)";
                else
                    curCamera.ChkFixedXY.Text = "固定距離(XY)";

            }
            else
            {
                if (Language.Lang.Model.CurrentLang.FullName == "中文")
                {
                    curCamera.ChkFixedXY.Text = "固定步长(XY)";

                }
                else if (Language.Lang.Model.CurrentLang.FullName == "English")
                    curCamera.ChkFixedXY.Text = "FixedStep(XY)";
                else
                    curCamera.ChkFixedXY.Text = "固定步長(XY)";

            }



            if (chkFixedDisZ.Checked)
            {
                if (Language.Lang.Model.CurrentLang.FullName == "中文")
                {
                    curCamera.ChkFixedZ.Text = "固定距离(Z)";
                }
                else if (Language.Lang.Model.CurrentLang.FullName == "English")
                    curCamera.ChkFixedZ.Text = "FixedDis(Z)";
                else
                    curCamera.ChkFixedZ.Text = "固定距離(Z)";

            }
            else
            {
                if (Language.Lang.Model.CurrentLang.FullName == "中文")
                {
                    curCamera.ChkFixedZ.Text = "固定步长(Z)";

                }
                else if (Language.Lang.Model.CurrentLang.FullName == "English")
                    curCamera.ChkFixedZ.Text = "FixedStep(Z)";
                else
                    curCamera.ChkFixedZ.Text = "固定步長(Z)";

            }
         
            if(WorkCurveHelper.FixedDisXY)
                curCamera.fixedXYValue.Text = "4";


            if (WorkCurveHelper.FixedDisZ)
                curCamera.fixedZValue.Text = "4";

            if(chkThrSpeeds.Checked)
                WorkCurveHelper.AxisZSpeedMode = 1;
            else
                WorkCurveHelper.AxisZSpeedMode = 0;


            if (DifferenceDevice.IsThick)
            {
                if (WorkCurveHelper.AxisZSpeedMode == 1)
                {
                    curCamera.Controls.Find("panelZ1", true)[0].Visible = false;
                    curCamera.Controls.Find("panelZ2", true)[0].Visible = true;

                }
                else
                {
                    curCamera.Controls.Find("panelZ1", true)[0].Visible = true;
                    curCamera.Controls.Find("panelZ2", true)[0].Visible = false;
                }
            }

            if (comboMatrixMode.SelectedIndex == 0)
            {
                WorkCurveHelper.matrixMode = "dotDot";
            }
            else if (comboMatrixMode.SelectedIndex == 1)
            {
                WorkCurveHelper.matrixMode = "matrixDot";
            }
            else if (comboMatrixMode.SelectedIndex == 2)
            {
                WorkCurveHelper.matrixMode = "dotMatrix";
            }


            if (DifferenceDevice.CurCameraControl.skyrayCamera1.Mode == Skyray.Camera.SkyrayCamera.CameraMode.dotDot ||
                DifferenceDevice.CurCameraControl.skyrayCamera1.Mode == Skyray.Camera.SkyrayCamera.CameraMode.dotMatrix ||
                DifferenceDevice.CurCameraControl.skyrayCamera1.Mode == Skyray.Camera.SkyrayCamera.CameraMode.matrixDot)
            {
                if (comboMatrixMode.SelectedIndex == 0)
                {
                    DifferenceDevice.CurCameraControl.skyrayCamera1.Mode = Skyray.Camera.SkyrayCamera.CameraMode.dotDot;
                }
                else if (comboMatrixMode.SelectedIndex == 1)
                {
                    DifferenceDevice.CurCameraControl.skyrayCamera1.Mode = Skyray.Camera.SkyrayCamera.CameraMode.matrixDot;
                }
                else if (comboMatrixMode.SelectedIndex == 2)
                {
                    DifferenceDevice.CurCameraControl.skyrayCamera1.Mode = Skyray.Camera.SkyrayCamera.CameraMode.dotMatrix;
                }

            }

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/bShowInitParam", WorkCurveHelper.bShowInitParam.ToString());
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/IsUserElect", WorkCurveHelper.IsUseElect.ToString());
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/IsPureElemCurrentUnify", WorkCurveHelper.IsPureElemCurrentUnify.ToString());
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/FPThick/AxisZSpeedMode", WorkCurveHelper.AxisZSpeedMode.ToString());
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/matrixMode", WorkCurveHelper.matrixMode);
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/multiReset", WorkCurveHelper.multiReset.ToString());
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/filterReset", WorkCurveHelper.filterReset.ToString());
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/loopTestDemo", WorkCurveHelper.loopTestDemo.ToString());
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/normalizeSteps", WorkCurveHelper.normalizeSteps.ToString());

          
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnSysDel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }



    }
}
