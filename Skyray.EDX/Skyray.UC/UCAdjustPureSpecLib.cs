using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Skyray.Language;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.Controls;

namespace Skyray.UC
{
    public partial class UCAdjustPureSpecLib : UCMultiple
    {
        public UCAdjustPureSpecLib()
        {
            InitializeComponent();
           
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            double result =0;
            bool isdouble = double.TryParse(txtAdjustCoeff.Text, out result) ;
            WorkCurveHelper.PureAdjustCoeff = result;
            if (!isdouble)
            {
                WorkCurveHelper.PureAdjustCoeff = 1;
            }
            ReportTemplateHelper.SaveSpecifiedValueandCreate("EncoderCoeff", "PureAdjustCoeff", WorkCurveHelper.PureAdjustCoeff.ToString());
       
            
        }

        private void UCAdjustPureSpecLib_Load(object sender, EventArgs e)
        {
            txtAdjustCoeff.Text = WorkCurveHelper.PureAdjustCoeff.ToString();
        }

    }
}
