using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;

namespace Skyray.UC
{
    /// <summary>
    /// 设置调整的距离
    /// </summary>
    public partial class FrmSetAdjustPointsDistance : UCMultiple
    {
        /// <summary>
        /// 
        /// </summary>
        private double dblBakDistance = 0.0;

        /// <summary>
        /// 调整距离
        /// </summary>
        public event EventDelegate.CameralOperation OnadjustDistance;

        /// <summary>
        /// 画面两点对应的真实距离
        /// </summary>
        public double Distance
        {
            get
            {
                double d;
                double.TryParse(txtDistance.Text, out d);
                return d;
            }
            set
            {
                txtDistance.Text = Math.Abs(value).ToString();
            }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public FrmSetAdjustPointsDistance()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取用户所填的距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDistance_Leave(object sender, EventArgs e)
        {
            double d;
            double.TryParse(txtDistance.Text, out d);
            
            txtDistance.Text = Math.Abs(d).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSetAdjustPointsDistance_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                dblBakDistance = Distance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        /// <summary>
        /// 单击接受按钮处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
            if (OnadjustDistance != null)
                OnadjustDistance();
        }
    }
}
