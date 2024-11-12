using System;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    /// <summary>
    /// 分析参数类
    /// </summary>
    public partial class UCAnalysisParam : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 定性分析参数
        /// </summary>
        private QualeElement qualeElement;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCAnalysisParam()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void PageLoad(object sender, EventArgs e)
        {
            Lephone.Data.Common.DbObjectList<QualeElement> lst = QualeElement.FindAll();
            if (lst != null && lst.Count > 0)
            {
                qualeElement = lst[0];
            }
            if (qualeElement == null || lst.Count == 0)
            {
                qualeElement = QualeElement.New.Init(30, 7, 2.5, 2, 10, "");
                qualeElement.Save();
            }
            meAnalysis.LayoutType = typeof(QualeElement);
            meAnalysis.LayoutSource = LayoutSource.New.Init(false, true, true, string.Empty, string.Empty, 2, LabelPosition.Top);

            CtrlFactory.BindValue(meAnalysis.EditControls, qualeElement, true);//绑定控件
            base.PageLoad(sender, e);
        }
        /// <summary>
        /// 设置文本
        /// </summary>
        public override void SetText()
        {
           Skyray.Language.Lang.Model.SetTextProperty(this.meAnalysis.LabelControls);
        }
        /// <summary>
        /// 保存文本
        /// </summary>
        public override void SaveText()
        {
            Skyray.Language.Lang.Model.SaveTextProperty(false, this.meAnalysis.LabelControls);
        }

        #region Events
        /// <summary>
        /// 点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (qualeElement != null)
            {
                qualeElement.Save();
            }
            EDXRFHelper.GotoMainPage(this);
        }

        /// <summary>
        /// 点击取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        #endregion

        private void btnApplication_Click(object sender, EventArgs e)
        {
            if (qualeElement != null)
            {
                qualeElement.Save();
            }
        }
    }
}
