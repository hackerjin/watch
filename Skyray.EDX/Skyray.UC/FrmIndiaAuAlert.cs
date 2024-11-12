using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class FrmIndiaAuAlert : MultipleForm
    {
       // public DialogResult DialogResult;

        private bool bShowDialog = false;

        public FrmIndiaAuAlert(bool showResult)
        {
            InitializeComponent();
            bShowDialog = showResult;
        }
         

        private void FrmIndiaAuAlert_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Close();
        }

        private void FrmIndiaAuAlert_Load(object sender, EventArgs e)
        {
            if (bShowDialog)
            {
                FrmMeasureResult result = new FrmMeasureResult();
                result.isShowElemFullname = true;
                result.DataSource = WorkCurveHelper.WorkCurveCurrent.ElementList;
                result.SampleName = DifferenceDevice.interClassMain.specList.SampleName;
                result.Weight = DifferenceDevice.interClassMain.specList.Weight.ToString();
                result.TopMost = true;
                result.Show();
            }
        }
    }
}
