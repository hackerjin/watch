using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCComputeIntensity : Skyray.Language.UCMultiple
    {
        private ElementList _elementList;

        private string _sampleName;
        private bool _isExplore;

        public ElementList elementList
        {
            set
            {
                if (value != null)
                {

                    foreach (CurveElement element in value.Items)
                    {
                        if (element.Caption == "Fe" && (FpWorkCurve.thickMode == ThickMode.NiCuNiFe || FpWorkCurve.thickMode == ThickMode.NiCuNiFe2))
                            this.dgvElementIntensity.Rows.Add("Ni2", element.Intensity.ToString());
                        else
                            this.dgvElementIntensity.Rows.Add(element.Caption, element.Intensity.ToString());
                    }

                    _elementList = value;
                }
            }
        }

        public string sampleName
        {
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    this.textBoxSampleName.Text = value;
                    _sampleName = value;
                }
            }
        }
        public bool IsExplore
        {
            set 
            {
                _isExplore = value;
                this.btnJoinCurve.Visible = !_isExplore;
            }
        }
        public UCComputeIntensity()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_elementList != null)
            {
                foreach (var element in _elementList.Items)
                {
                    StandSample sample = element.Samples.ToList().Find(s => s.SampleName == _sampleName);
                    if (sample == null)
                    {
                        double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(element.Caption.ToUpper())).AtomDensity;
                        StandSample ss = StandSample.New.Init(_sampleName, DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderValue.ToString("f2"), DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderHeight.ToString("f2"), element.Intensity.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()), "0", "0", false, element.Caption, 0, 0, "", dblDensity, "0");
                        element.Samples.Add(ss);
                        element.Save();
                    }
                    else
                    {
                        sample.X = element.Intensity.ToString("f"+WorkCurveHelper.SoftWareContentBit.ToString());
                        sample.Save();
                    }
                }
            }
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }
    }
}
