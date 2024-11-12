using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCGainSetFor3000 : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 当前工作曲线
        /// </summary>
        private WorkCurve workCurveCurrent;

        private bool UserUpdateParams = false;

        public UCGainSetFor3000()
        {
            InitializeComponent();
        }

        private void UCGainSetFor3000_Load(object sender, EventArgs e)
        {
            UserUpdateParams = (ReportTemplateHelper.LoadSpecifiedValue("EDXRFGainSet", "UserUpdateParams") == "1") ? true : false;
            if (GP.CurrentUser.Role.RoleType == 2 && !UserUpdateParams)
            {
                diVoltage.Enabled = false;
                diCurrent.Enabled = false;
            }
            if (WorkCurveHelper.WorkCurveCurrent == null)
                return;
            workCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            if (workCurveCurrent != null)
            {
                this.diGain.Text = workCurveCurrent.Condition.InitParam.Gain.ToString();
                this.diFineGain.Text = workCurveCurrent.Condition.InitParam.FineGain.ToString();
                this.diVoltage.Text = workCurveCurrent.Condition.DeviceParamList[0].TubVoltage.ToString();
                this.diCurrent.Text = workCurveCurrent.Condition.DeviceParamList[0].TubCurrent.ToString();
                //this.diVoltage.Text = workCurveCurrent.Condition.InitParam.TubVoltage.ToString();
                //this.diCurrent.Text = workCurveCurrent.Condition.InitParam.TubCurrent.ToString();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (workCurveCurrent != null)
            {
                workCurveCurrent.Condition.InitParam.Gain = float.Parse(this.diGain.Text);
                workCurveCurrent.Condition.InitParam.FineGain = float.Parse(this.diFineGain.Text);
                workCurveCurrent.Condition.DeviceParamList[0].TubVoltage = int.Parse(this.diVoltage.Text);
                workCurveCurrent.Condition.DeviceParamList[0].TubCurrent = int.Parse(this.diCurrent.Text);
                //workCurveCurrent.Condition.InitParam.TubVoltage = int.Parse(this.diVoltage.Text);
                //workCurveCurrent.Condition.InitParam.TubCurrent = int.Parse(this.diCurrent.Text);
                workCurveCurrent.Condition.Save();
            }
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//回到主界面
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//回到主界面
        }

        private void diVoltage_Validating(object sender, CancelEventArgs e)
        {
            if (int.Parse(this.diVoltage.Text) > WorkCurveHelper.DeviceCurrent.MaxVoltage)
            {
                this.diVoltage.Text = WorkCurveHelper.DeviceCurrent.MaxVoltage.ToString();
                e.Cancel = true;
            }
        }

        private void diCurrent_Validating(object sender, CancelEventArgs e)
        {
            if (int.Parse(this.diCurrent.Text) > WorkCurveHelper.DeviceCurrent.MaxCurrent)
            {
                this.diCurrent.Text = WorkCurveHelper.DeviceCurrent.MaxCurrent.ToString();
                e.Cancel = true;
            }
        }
    }
}
