using System.Windows.Forms;
using System;

namespace Skyray.UC
{
    /// <summary>
    /// 计算强度类
    /// </summary>
    public partial class UCCalcIntensity : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 计算强度
        /// </summary>
        public UCCalcIntensity()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 重写加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void PageLoad(object sender, EventArgs e)
        {
            base.PageLoad(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void btnJoinCurve_Click(object sender, EventArgs e)
        {

        }
    }
}
