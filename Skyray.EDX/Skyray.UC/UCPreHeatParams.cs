using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCPreHeatParams : Skyray.Language.UCMultiple
    {
        private PreHeatParams preHeatParams;

        public UCPreHeatParams()
        {
            InitializeComponent();
            Skyray.Language.Lang.Model.LanguageChanged += new EventHandler(Model_LanguageChanged);//语言改变事件（主要用于改变自动生成控件部分的语言）
        }

        private void UCPreHeatParams_Load(object sender, EventArgs e)
        {
            DbObjectList<PreHeatParams> lst = PreHeatParams.FindAll();
            if (lst.Count != 0)
            {
                preHeatParams = lst[0];
            }
            else
            {
                preHeatParams = PreHeatParams.New.Init(20, 100, 60, 120, 1, 1, 180, 900, 1, TargetMode.OneTarget, 1);
                if (WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
                    preHeatParams.Gain = 66f;
            }
            modelPreHeat.Init(typeof(PreHeatParams), LayoutSource.New.Init(false, true, true, string.Empty, string.Empty, 3, LabelPosition.Top), preHeatParams);
            Skyray.Language.Lang.Model.SaveTextProperty(false, this.modelPreHeat.LabelControls);
            Skyray.Language.Lang.Model.SaveProperty(new string[] { "GroupTitle" }, this.modelPreHeat);
            Skyray.Language.Lang.Model.SetTextProperty(false, this.modelPreHeat.LabelControls);
            Skyray.Language.Lang.Model.SetProperty(new string[] { "GroupTitle" }, this.modelPreHeat);
            //if (WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
            //{
            //    cboFPGAGain.Items.Clear();
            //    cboFPGAGain.Items.AddRange(new object[] { 87.9, 46.1, 35.3, 78.5 });
            //    cboFPGAGain.Visible = true;
            //    BindHelper.BindTextToCtrl(cboFPGAGain, preHeatParams, "Gain", true);
            //}
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            preHeatParams.Save();

            DifferenceDevice.interClassMain.AutoIncrease();
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        void Model_LanguageChanged(object sender, EventArgs e)
        {
            Skyray.Language.Lang.Model.SetTextProperty(false, this.modelPreHeat.LabelControls);
        }

        public override void ExcuteEndProcess(params object[] objs)
        {
            DifferenceDevice.MediumAccess.StartPreHeatProcess(preHeatParams);
        }

        private void LoadPreHeatParams()
        {
            try
            {
                int voltage = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("PreHeat", "Voltage"));
                int current = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("PreHeat", "Current"));
                int collimator = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("PreHeat", "Collimator"));
                int filter = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("PreHeat", "Filter"));
                int offset = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("PreHeat", "Offset"));
                int fineGain = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("PreHeat", "FineGain"));
                int measureTime = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("PreHeat", "MeasureTime"));
                int preheatTime = Convert.ToInt32(ReportTemplateHelper.LoadSpecifiedValue("PreHeat", "PreheatTime"));
                preHeatParams = PreHeatParams.New.Init(voltage, current, offset, fineGain, filter, collimator, preheatTime, measureTime, 1, TargetMode.OneTarget, 1);
            }
            catch
            { }
        }
    }
}
