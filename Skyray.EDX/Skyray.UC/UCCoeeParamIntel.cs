using System;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    public partial class UCCoeeParamIntel : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 当前工作曲线
        /// </summary>
        private WorkCurve workCurveCurrent=null;
        public WorkCurve WorkCurve
        {
            get
            {
                return workCurveCurrent;
            }
        }

        private CalibrationParam calibrationParam = null;
        public CalibrationParam CalibrationParam
        {
            get
            {
                return calibrationParam;
            }
        }

        public UCCoeeParamIntel()
        {
            InitializeComponent();
            workCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            calibrationParam = workCurveCurrent.CalibrationParam;
        }

        private void UCCoeeParamIntel_Load(object sender, EventArgs e)
        {
            if (calibrationParam == null)
            {
                calibrationParam = CalibrationParam.New.Init(false, 45, 1, false, 2, 1, false, 1, 1, false, 1, 1, false, "",false,1,0,0);
                calibrationParam.WorkCurve = workCurveCurrent;
                calibrationParam.Save();
            }
            BindHelper.BindCheckedToCtrl(chkEscSpec, calibrationParam, "IsEscapePeakProcess", true);
            BindHelper.BindTextToCtrl(dblEscAngle, calibrationParam, "EscapeAngle", true);
            BindHelper.BindTextToCtrl(dblEscFactor, calibrationParam, "EscapeFactor", true);

            BindHelper.BindCheckedToCtrl(chkSumSpec, calibrationParam, "IsSumPeakProcess", true);
            BindHelper.BindTextToCtrl(dblSumFwhm, calibrationParam, "PulseResolution", true);
            BindHelper.BindTextToCtrl(dblSumFactor, calibrationParam, "SumFactor", true);

            BindHelper.BindCheckedToCtrl(chkRemoveBase1, calibrationParam, "IsRemoveBackGroundOne", true);
            BindHelper.BindTextToCtrl(numRemoveBase1Times, calibrationParam, "RemoveFirstTimes", true);

            BindHelper.BindCheckedToCtrl(chkRemoveBase2, calibrationParam, "IsRemoveBackGroundTwo", true);
            BindHelper.BindTextToCtrl(numRemoveBase2Times, calibrationParam, "RemoveSecondTimes", true);
            BindHelper.BindTextToCtrl(dblRemoveBase2Factor, calibrationParam, "RemoveSecondFactor", true);

            BindHelper.BindCheckedToCtrl(chkRemoveBase3, calibrationParam, "IsRemoveBackGroundThree", true);
            BindHelper.BindTextToCtrl(txtRemoveBase3Points, calibrationParam, "BackGroundPoint", true);


            BindHelper.BindCheckedToCtrl(chkRemoveBase4, calibrationParam, "IsRemoveBackGroundFour", true);
            BindHelper.BindTextToCtrl(numRemoveBase4Times, calibrationParam, "RemoveFourTimes", true);
            BindHelper.BindTextToCtrl(dblRemoveBase4Left, calibrationParam, "RemoveFourLeft", true);
            BindHelper.BindTextToCtrl(dblRemoveBase4Right, calibrationParam, "RemoveFourRight", true);



            //this.chkEscSpec.Checked = true;
            //this.chkSumSpec.Checked = true;
            //this.chkRemoveBase1.Checked = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//返回主界面或关闭
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //if (txtRemoveBase3Points.Text != "" && !txtRemoveBase3Points.Text.EndsWith(","))
            //{
            //    calibrationParam.BackGroundPoint += ",";
            //}
            int escape = calibrationParam.IsEscapePeakProcess == true ? 1 : 0;
            int sumPeak = calibrationParam.IsSumPeakProcess == true ? 1 : 0;
            int one = calibrationParam.IsRemoveBackGroundOne == true ? 1 : 0;
            int two = calibrationParam.IsRemoveBackGroundTwo == true ? 1 : 0;
            int three = calibrationParam.IsRemoveBackGroundThree == true ? 1 : 0;
            int four = calibrationParam.IsRemoveBackGroundFour == true ? 1 : 0;
            string sql = "update CalibrationParam set IsEscapePeakProcess =" + escape + ",EscapeAngle=" + calibrationParam.EscapeAngle + ",EscapeFactor=" + calibrationParam.EscapeFactor 
                + ",IsSumPeakProcess=" + sumPeak + ",PulseResolution=" + calibrationParam.PulseResolution + ",SumFactor=" + calibrationParam.SumFactor + ",IsRemoveBackGroundOne=" 
                + one + ",RemoveFirstFactor=" + calibrationParam.RemoveFirstFactor + ",RemoveFirstTimes=" + calibrationParam.RemoveFirstTimes + ",IsRemoveBackGroundTwo="
                + two + ",RemoveSecondTimes=" + calibrationParam.RemoveSecondTimes + ",RemoveSecondFactor="
                + calibrationParam.RemoveSecondFactor + ",IsRemoveBackGroundThree=" + three + ",BackGroundPoint='" + calibrationParam.BackGroundPoint + "'"
                + ",IsRemoveBackGroundFour=" + four + ",RemoveFourTimes='" + calibrationParam.RemoveFourTimes + "',RemoveFourLeft='" + calibrationParam.RemoveFourLeft + "',RemoveFourRight='" + calibrationParam.RemoveFourRight + "'  where Id =" + calibrationParam.Id;
            Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void txtRemoveBase3Points_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int[] points=Helper.ToInts(txtRemoveBase3Points.Text);
            }
            catch (System.Exception )
            {
                Msg.Show(lblRemoveBase3Points.Text);
                txtRemoveBase3Points.Text = "";
            }
        }

        public override void ExcuteEndProcess(params object[] objs)
        {
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(workCurveCurrent.Id);
            if (WorkCurveHelper.MainSpecList != null && WorkCurveHelper.MainSpecList.Specs.Length > 0)
            {
                List<SpecListEntity> listList = new List<SpecListEntity>();
                listList.Add(WorkCurveHelper.MainSpecList);
                DifferenceDevice.interClassMain.OpenWorkSpec(listList, false);
            }
            if(WorkCurveHelper.VirtualSpecList != null && WorkCurveHelper.VirtualSpecList.Count > 0)
                DifferenceDevice.MediumAccess.OpenVirtualWorkSpectrum(WorkCurveHelper.VirtualSpecList);
        }

        private void dblSumFwhm_TextChanged(object sender, EventArgs e)
        {
            if (dblSumFwhm.Text != "" && 
                ((dblSumFwhm.Text.IndexOf(".") != -1 && dblSumFwhm.Text.Substring(0, dblSumFwhm.Text.IndexOf(".")).Length > 3)
                || (dblSumFwhm.Text.IndexOf(".") == -1 && dblSumFwhm.Text.Length > 3))
                ) dblSumFwhm.Text = "999.00";
        }

        private void dblEscAngle_TextChanged(object sender, EventArgs e)
        {
            if ( dblEscAngle.Text != "" &&
                ((dblEscAngle.Text.IndexOf(".") != -1 && dblEscAngle.Text.Substring(0, dblEscAngle.Text.IndexOf(".")).Length > 3)
                || (dblEscAngle.Text.IndexOf(".") == -1 && dblEscAngle.Text.Length > 3))
                ) dblEscAngle.Text = "999.00";
        }
        
        private void chkRemoveBase4_Click(object sender, EventArgs e)
        {
            if (((CheckBox)sender) == chkRemoveBase1 && chkRemoveBase1.Checked)
            {
                chkRemoveBase2.Checked = false;
                chkRemoveBase3.Checked = false;
                chkRemoveBase4.Checked = false;
            }
            if (((CheckBox)sender) == chkRemoveBase2 && chkRemoveBase2.Checked)
            {
                chkRemoveBase1.Checked = false;
                chkRemoveBase3.Checked = false;
                chkRemoveBase4.Checked = false;
            }
            if (((CheckBox)sender) == chkRemoveBase3 && chkRemoveBase3.Checked)
            {
                chkRemoveBase1.Checked = false;
                chkRemoveBase2.Checked = false;
                chkRemoveBase4.Checked = false;
            }
            if (((CheckBox)sender) == chkRemoveBase4 && chkRemoveBase4.Checked)
            {
                chkRemoveBase1.Checked = false;
                chkRemoveBase2.Checked = false;
                chkRemoveBase3.Checked = false;
            }
        }

        private void dblRemoveBase4Left_TextChanged(object sender, EventArgs e)
        {
            int speclength=(int)WorkCurveHelper.DeviceCurrent.SpecLength;

            if (dblRemoveBase4Left.Text != "")
            {
                int temp = 0;
                try
                {
                    temp = int.Parse(this.dblRemoveBase4Left.Text);

                    if (dblRemoveBase4Right.Text!="" && int.Parse(dblRemoveBase4Left.Text) > int.Parse(dblRemoveBase4Right.Text)) this.dblRemoveBase4Left.Text = "1";
                    if(int.Parse(dblRemoveBase4Left.Text)>=speclength) this.dblRemoveBase4Left.Text=speclength.ToString();
                }
                catch 
                {
                    this.dblRemoveBase4Left.Text="1";
                }
            }
        }

        private void dblRemoveBase4Right_TextChanged(object sender, EventArgs e)
        {
            int speclength = (int)WorkCurveHelper.DeviceCurrent.SpecLength;

            if (dblRemoveBase4Right.Text != "")
            {
                int temp = 0;
                try
                {
                    temp = int.Parse(this.dblRemoveBase4Right.Text);

                    //if (dblRemoveBase4Right.Text != "" && int.Parse(dblRemoveBase4Left.Text) > int.Parse(dblRemoveBase4Right.Text)) this.dblRemoveBase4Left.Text = "0";
                    if (int.Parse(dblRemoveBase4Right.Text) >= speclength) this.dblRemoveBase4Right.Text = speclength.ToString();
                }
                catch 
                {
                    this.dblRemoveBase4Right.Text = "1";
                }
            }
        }
    }
}
