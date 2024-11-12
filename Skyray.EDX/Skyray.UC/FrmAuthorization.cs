using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class FrmAuthorization : Skyray.Language.UCMultiple
    {
        public FrmAuthorization()
        {
            InitializeComponent();
        }

     
        private void btnRequst_Click(object sender, EventArgs e)
        {
           // if (WorkCurveHelper.IsDppValidate && WorkCurveHelper.DppMachineId.Length > 0)
            if (WorkCurveHelper.DppMachineId.Length > 0)
                txtRequest.Text = EDXRFHelper.GenerateRandomLetter(4).ToUpper() + WorkCurveHelper.DppMachineId.Replace(" ", "").ToUpper();
            else
                txtRequest.Text = "没有授权码信息！";
        }

        private void FrmAuthorization_Load(object sender, EventArgs e)
        {

        }


    }
}
