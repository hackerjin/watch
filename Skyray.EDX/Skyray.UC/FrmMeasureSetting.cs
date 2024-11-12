using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public partial class FrmMeasureSetting : Skyray.Language.UCMultiple
    {
        private bool _isSetMatchTime;
        public FrmMeasureSetting()
        {
            InitializeComponent();
            LoadDataSource();
        }

        public FrmMeasureSetting(bool isSetMatchTime)
        {
            InitializeComponent();
            _isSetMatchTime = isSetMatchTime;
            LoadDataSource();
        }

        public void LoadDataSource()
        {
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition != null && WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count > 0)
                this.numricUpDownW1.Value = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].PrecTime;
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition != null && WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count > 0)
            {
                try
                {
                    int measureTime =  Convert.ToInt32(this.numricUpDownW1.Value.ToString());
                    DeviceParameter tempDevice = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0];
                    string sql = "Update DeviceParameter Set PrecTime = " + measureTime + " Where Id = " + tempDevice.Id;
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    if (_isSetMatchTime)                                  //yuzhaomodify:修改时间可以修改匹配时间
                    {
                        Condition conditionMatch = Condition.FindOne(w => w.Type == ConditionType.Match && w.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                        string sql2 = "Update DeviceParameter Set PrecTime = " + measureTime + " Where Id = " + conditionMatch.DeviceParamList[0].Id;
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql2);
                    }

                    WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
                    this.dialogResult = DialogResult.OK;
                }
                catch { }
            }
            EDXRFHelper.GotoMainPage(this);
           
        }

        public override void ExcuteEndProcess(params object[] objs)
        {
            DifferenceDevice.interClassMain.progressInfo.MeasureTime = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].PrecTime.ToString() + "s";
            DifferenceDevice.interClassMain.progressInfo.SurplusTime = "0s";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }


    }
}
