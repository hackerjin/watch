using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.Language;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    /// <summary>
    /// 计算强度类
    /// </summary>
    public partial class UcInitCalibrateParam : Skyray.Language.UCMultiple
    {

        private double _initcount;
        public double InitCount
        {
            get { return _initcount; }
            set
            {
                _initcount = value;
                numFirstCount.Text = value.ToString();
            }
        }

        private double _initRadio;
        public double InitRadio
        {
            get { return _initRadio ; }
            set
            {
                _initRadio = value;
                numRadio.Text = value.ToString();
            }
        }

        /// <summary>
        /// 计算强度
        /// </summary>
        public UcInitCalibrateParam()
        {
            InitializeComponent();
            //nUpCount.Value = Convert.ToDecimal(WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.InitFistCount);
            //numRadio.Value = Convert.ToDecimal(WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.InitCalibrateRatio);
        }

        private void btWSubmit_Click(object sender, EventArgs e)
        {
            _initRadio = Convert.ToDouble(numRadio.Text);
            _initcount = Convert.ToDouble(numFirstCount.Text);
            this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void UcInOutSample_Load(object sender, EventArgs e)
        {
            
        }

        private void btWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void nUpCount_ValueChanged(object sender, EventArgs e)
        {

        }
       
    }
}
