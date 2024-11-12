using System;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCCoeeParam : Skyray.Language.UCMultiple
    {

        #region Field

        /// <summary>
        /// 当前工作曲线
        /// </summary>
        private WorkCurve workCurveCurrent;

        #endregion

        #region Init
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCCoeeParam()
        {
            InitializeComponent();
            //BindHelper.BindValueToCtrl(cbwIsEscapePeakProcess, workCurveCurrent.CalibrationParam, "IsEscapePeakProcess", true);
        }
        #endregion

        #region Override
        /// <summary>
        /// 重写父类加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void PageLoad(object sender, EventArgs e)
        {
            meCoee.LayoutType = typeof(CalibrationParam);
            //meCoee.LayoutSource = LayoutSource.New.Init(false, true, true, string.Empty, string.Empty, 3, LabelPosition.Left);
            meCoee.LayoutSource = LayoutSource.New.Init(false, true, false, "", "", 3, LabelPosition.Left);
            workCurveCurrent = WorkCurveHelper.WorkCurveCurrent;
            if (workCurveCurrent != null)
            {
                if (workCurveCurrent.CalibrationParam == null)
                {
                    workCurveCurrent.CalibrationParam = CalibrationParam.New.Init(false, 0, 0, false, 0, 0, false, 0, 0, false, 0, 0, false, "1,2,3",false,1,0,0);
                }

                CtrlFactory.BindValue(meCoee.EditControls, workCurveCurrent.CalibrationParam, true);//绑定数据源
            }
            base.PageLoad(sender, e);
        }
        /// <summary>
        /// 设置文本
        /// </summary>
        public override void SetText()
        {
            Skyray.Language.Lang.Model.SetTextProperty(this.meCoee.LabelControls);
        }
        /// <summary>
        /// 保存文本
        /// </summary>
        public override void SaveText()
        {
            Skyray.Language.Lang.Model.SaveTextProperty(this.meCoee.LabelControls);
        }

        #endregion

        #region Events
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//返回主界面或关闭
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            workCurveCurrent.Save();//保存数据
            EDXRFHelper.GotoMainPage(this);
        }

        #endregion

    }
}
