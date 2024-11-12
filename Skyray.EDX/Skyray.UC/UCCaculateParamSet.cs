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
    public partial class UCCaculateParamSet : UCMultiple
    {
      
        public UCCaculateParamSet()
        {
            InitializeComponent();

            numAvalue.Value = (decimal)WorkCurveHelper.NiCuNiParam.aValue;
            numNvalue.Value = (decimal)WorkCurveHelper.NiCuNiParam.nValue;
            numkvalue.Value = (decimal)WorkCurveHelper.NiCuNiParam.kValue;
            numbvalue.Value = (decimal)WorkCurveHelper.NiCuNiParam.bValue;
            numlimitvalue.Value = (decimal)WorkCurveHelper.NiCuNiParam.limit;
        }

        private void btWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void btWSubmit_Click(object sender, EventArgs e)
        {
            WorkCurveHelper.NiCuNiParam.aValue = (double)numAvalue.Value;
            WorkCurveHelper.NiCuNiParam.nValue = (double)numNvalue.Value;
            WorkCurveHelper.NiCuNiParam.kValue = (double)numkvalue.Value;
            WorkCurveHelper.NiCuNiParam.bValue = (double)numbvalue.Value;
            WorkCurveHelper.NiCuNiParam.limit = (double)numlimitvalue.Value;
            ReportTemplateHelper.SaveAttribute("DataParam/NiFormulaParam", "aValue", WorkCurveHelper.NiCuNiParam.aValue.ToString());
            ReportTemplateHelper.SaveAttribute("DataParam/NiFormulaParam", "nValue", WorkCurveHelper.NiCuNiParam.nValue.ToString());
            ReportTemplateHelper.SaveAttribute("DataParam/NiFormulaParam", "kValue", WorkCurveHelper.NiCuNiParam.kValue.ToString());
            ReportTemplateHelper.SaveAttribute("DataParam/NiFormulaParam", "bValue", WorkCurveHelper.NiCuNiParam.bValue.ToString());
            ReportTemplateHelper.SaveAttribute("DataParam/NiFormulaParam", "limit", WorkCurveHelper.NiCuNiParam.limit.ToString());
          
          
            EDXRFHelper.GotoMainPage(this);
        }

       



      
    }
}
