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
    public partial class FrmSuperModel : Skyray.Language.MultipleForm
    {
        public FrmSuperModel()
        {
            InitializeComponent();
        }

        private void radTwoTarget_CheckedChanged(object sender, EventArgs e)
        {
            if (radTwoTarget.Checked)
            {
                //WorkCurveHelper.TargetType = TargetType.SuperTwoTarget;
            }
        }

        private void radOneTarget_CheckedChanged(object sender, EventArgs e)
        {
            if (radOneTarget.Checked)
            {
                //WorkCurveHelper.TargetType = TargetType.SuperOneTarget;
            }
        }

        //private void rad1050_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rad1050.Checked)
        //    {
        //        WorkCurveHelper.MachineType = MachineType.Super1050;
        //    }
        //}

        private void rad2050_CheckedChanged(object sender, EventArgs e)
        {
            if (rad2050.Checked)
            {
                //WorkCurveHelper.MachineType = MachineType.Super2050;
            }
        }

        private void rad2400_CheckedChanged(object sender, EventArgs e)
        {
            if (rad2400.Checked)
            {
                //WorkCurveHelper.MachineType = MachineType.Super2400;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
            //InitSysParams.WriteParameter(AppDomain.CurrentDomain.BaseDirectory + "Parameter.xml");
            //this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FrmSuperModel_Load(object sender, EventArgs e)
        {
            //if (WorkCurveHelper.MachineType == MachineType.Super2400)
            //{
            //    rad2400.Checked = true;
            //}
            //else //if (WorkCurveHelper.MachineType == MachineType.Super2050)
            //{
            //    rad2050.Checked = true;
            //}
            ////else
            ////{
            ////    rad1050.Checked = true;
            ////}
            //if (WorkCurveHelper.TargetType == TargetType.SuperTwoTarget)
            //{
            //    radTwoTarget.Checked = true;
            //}
            //else
            //{
            //    radOneTarget.Checked = true;
            //}
        }

    }
}
