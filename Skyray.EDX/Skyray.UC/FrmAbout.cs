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
    public partial class FrmAbout : Skyray.Language.UCMultiple
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void linkLabelWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", linkLabelWeb.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.ParentForm != null) this.ParentForm.Close();
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            WorkCurveHelper.deviceMeasure.interfacce.Pump.TOpen();//电磁阀置1
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WorkCurveHelper.deviceMeasure.interfacce.Pump.TClose();//电磁阀置1
        }
    }
}
