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
    public partial class UcInOutSample : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 计算强度
        /// </summary>
        public UcInOutSample()
        {
            InitializeComponent();
            nUpInSample.Value = WorkCurveHelper.InSampleSetp;
            
        }

        private void btWSubmit_Click(object sender, EventArgs e)
        {
            WorkCurveHelper.InSampleSetp = Convert.ToInt32(nUpInSample.Value);
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/InSampleSetp", WorkCurveHelper.InSampleSetp.ToString());
    
            EDXRFHelper.GotoMainPage(this);
        }

        private void UcInOutSample_Load(object sender, EventArgs e)
        {
            
        }

        private void btWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }
       
    }
}
