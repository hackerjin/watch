using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.Language;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public partial class UCSelectSpecType : UCMultiple
    {
        private bool _bExport = false;
        public bool IsExport
        {
            get { return _bExport; }
        }
        public UCSelectSpecType()
        {
            InitializeComponent();
            LoadSelectTypeForEdition();
            this.cbxSelectType.SelectedIndex = 0;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            _bExport = chkExport.Checked;
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void LoadSelectTypeForEdition()
        {
            if (DifferenceDevice.IsXRF)
            {
               this.cbxSelectType.Items.AddRange(new string[]{"XFP2","XRF"});
            }
            else if (DifferenceDevice.IsRohs)
            {
                this.cbxSelectType.Items.AddRange(new string[] {"ROHS3", "ROHS4"});
            }
            else if (DifferenceDevice.IsThick)
            {
                this.cbxSelectType.Items.AddRange(new string[] {"FPThick","Thick800A"});
            }
            else if (DifferenceDevice.IsAnalyser)
            {
                this.cbxSelectType.Items.AddRange(new string[] { "EDXRF", "FPThick" });//修改可以打开镀层谱
            }
        }

        private void cbxSelectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbxSelectType.SelectedItem == null || string.IsNullOrEmpty(this.cbxSelectType.SelectedItem.ToString()))
                return;
            WorkCurveHelper.EditionType = (TotalEditionType)Enum.Parse(typeof(TotalEditionType), this.cbxSelectType.SelectedItem.ToString());
        }
    }
}
