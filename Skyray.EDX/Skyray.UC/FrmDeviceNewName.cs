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
    public partial class FrmDeviceNewName : Skyray.Language.MultipleForm
    {
        public string newDeviceName = string.Empty;

        public FrmDeviceNewName()
        {
            InitializeComponent();
        }

        public FrmDeviceNewName(string name)
        {
            InitializeComponent();
            this.txtNewDeviceName.Text = name;
            this.txtNewDeviceName.Focus();
            this.txtNewDeviceName.SelectAll();
            newDeviceName = txtNewDeviceName.Text;
        }

        public FrmDeviceNewName(string name, string title, string lableText, string txtText)
            : this(txtText)
        {
            this.Text = title;
            this.lblNewDeviceName.Text = lableText;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Skyray.EDX.Common.ValidateHelper.IllegalCheck(this.txtNewDeviceName))
            {
                this.txtNewDeviceName.Focus();
                this.txtNewDeviceName.SelectAll();
                return;
            }
            newDeviceName = this.txtNewDeviceName.Text;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void txtNewDeviceName_Enter(object sender, EventArgs e)
        {
            if (lblNewDeviceName.Text.Equals(Info.NewDeviceName)) return;
            ToolTip tip = new ToolTip();
            tip.IsBalloon = true;
            tip.UseAnimation = true;
            tip.BackColor = Color.Yellow;
            //tip.SetToolTip(txtNewDeviceName, "使用+来实现和值的，如Cl+Br");
            tip.Show(Info.CustomDataTip, txtNewDeviceName, 0, -50);
        }
    }
}
