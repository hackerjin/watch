using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lephone.Data.Common;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.Controls;

namespace Skyray.UC
{
    public partial class FrmCurveReName : Skyray.Language.MultipleForm
    {
        public string newDeviceName = string.Empty;
        private DbObjectList<WorkCurve> lstWorkCurve;

        public FrmCurveReName()
        {
            InitializeComponent();
        }

        public FrmCurveReName(string name, DbObjectList<WorkCurve> lstWorkCurve)
        {
            InitializeComponent();
            this.txtNewDeviceName.Text = name;
            this.txtNewDeviceName.Focus();
            this.txtNewDeviceName.SelectAll();
            newDeviceName = txtNewDeviceName.Text;
            this.lstWorkCurve = lstWorkCurve;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Skyray.EDX.Common.ValidateHelper.IllegalCheck(this.txtNewDeviceName) )
            {
                this.txtNewDeviceName.Focus();
                this.txtNewDeviceName.SelectAll();
                return;
            }

            if (this.lstWorkCurve.Find(w => w.Name == this.txtNewDeviceName.Text 
                && w.Condition.Device.Id == WorkCurveHelper.DeviceCurrent.Id) != null
                && newDeviceName != this.txtNewDeviceName.Text)
            {
                SkyrayMsgBox.Show(Info.ExistName);
                this.txtNewDeviceName.Focus();
                this.txtNewDeviceName.SelectAll();
                return;
            }
            newDeviceName = this.txtNewDeviceName.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
