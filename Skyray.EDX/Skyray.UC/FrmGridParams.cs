using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;

namespace Skyray.UC
{
    /// <summary>
    /// 网格移动参数
    /// </summary>
    public partial class FrmGridParams : UCMultiple
    {
        /// <summary>
        /// 行中的点数
        /// </summary>
        private int bkPointRowCount;

        /// <summary>
        /// 列中的点数
        /// </summary>
        private int bkPointColCount;

        /// <summary>
        /// 行间的距离
        /// </summary>
        private double bkPointRowDistance;

        /// <summary>
        /// 列间的距离
        /// </summary>
        private double bkPointColDistance;

        /// <summary>
        /// 多点事件
        /// </summary>
        public event EventDelegate.CameralOperation OnSetMultiPoint;

        /// <summary>
        /// 行测量点数
        /// </summary>
        public int PointRowCount
        {
            get
            {
                return Convert.ToInt32(this.numRowPoint.Value);
            }
            set
            {
                if (value < 0)
                {
                    numRowPoint.Value = 0;
                }
                else
                {
                    numRowPoint.Value = Convert.ToDecimal(value);
                }
            }
        }

        /// <summary>
        /// 列测量点数
        /// </summary>
        public int PointColCount
        {
            get
            {
                return Convert.ToInt32(this.numColumnPoint.Value);
            }
            set
            {
                if (value < 0)
                {
                    numColumnPoint.Value = 0;
                }
                else
                {
                    numColumnPoint.Value = Convert.ToDecimal(value);
                }
            }
        }

        /// <summary>
        /// 行测量点距
        /// </summary>
        public double PointRowDistance
        {
            get
            {
                double d;
                double.TryParse(txtRowPointDistance.Text, out d);
                return d;
            }
            set
            {
                txtRowPointDistance.Text = value.ToString();
            }
        }

        /// <summary>
        /// 列测量点距
        /// </summary>
        public double PointColDistance
        {
            get
            {
                double d;
                double.TryParse(txtColumnPointDistance.Text, out d);
                return d;
            }
            set
            {
                txtColumnPointDistance.Text = value.ToString();
            }
        }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public FrmGridParams()
        {
            InitializeComponent();
            bkPointRowCount = 10;
            bkPointColCount = 10;
            bkPointRowDistance = 10;
            bkPointColDistance = 10;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWSubmit_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);

            if (OnSetMultiPoint != null)
                OnSetMultiPoint();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWCancel_Click(object sender, EventArgs e)
        {
            // 恢复备份值
            PointRowCount = bkPointRowCount;
            PointColCount = bkPointColCount;
            PointRowDistance = bkPointRowDistance;
            PointColDistance = bkPointColDistance;
            EDXRFHelper.GotoMainPage(this);
          
        }

        /// <summary>
        /// 鼠标离开处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxW1_Leave(object sender, EventArgs e)
        {
            double d;
            double.TryParse(txtRowPointDistance.Text, out d);
            if (d < 0)
            {
                txtRowPointDistance.Text = (-d).ToString();
            }
        }

        /// <summary>
        /// 鼠标离开处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxW2_Leave(object sender, EventArgs e)
        {
            double d;
            double.TryParse(txtColumnPointDistance.Text, out d);
            if (d < 0)
            {
                txtColumnPointDistance.Text = (-d).ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmGridParams_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // 备份参数
                bkPointRowCount = PointRowCount;
                bkPointColCount = PointColCount;
                bkPointRowDistance = PointRowDistance;
                bkPointColDistance = PointColDistance;
                
                


                //RefreshUI();
            } 
        }
    }
}
