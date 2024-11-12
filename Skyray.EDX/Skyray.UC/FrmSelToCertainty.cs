using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.ReportHelper;

namespace Skyray.UC
{
    public partial class FrmSelToCertainty : MultipleForm
    {
        private WorkCurve _WorkCurveTemp = null;
        public WorkCurve WorkCurve
        {
            get { return _WorkCurveTemp; }
            set { _WorkCurveTemp = value; }
        }
        private CurveElement _currentElement = null;
        public CurveElement Element
        {
            get { return _currentElement; }
        }
        public double StandUncertainty
        {
            get { return double.Parse(txtStandUncertainty.Text.Trim()); }
        }
        public double UnknownUncertainty
        {
            get { return double.Parse(txtSampleUncertainty.Text.Trim()); }
        }
        public FrmSelToCertainty()
        {
            InitializeComponent();
        }
        private List<string> LstSampleUnCerts = new List<string>();
        private void FrmSelToCertainty_Load(object sender, EventArgs e)
        {
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml");
                System.Xml.XmlNode node = doc.SelectSingleNode("application/Uncertainty/SampleUncertaintys");
                string strUns = node != null ? node.InnerText : "0.06";
                LstSampleUnCerts=strUns.Split(',').ToList();
            }
            catch
            {
            }
            comboBoxWElems.Items.Clear();
            if (_WorkCurveTemp == null
                ||_WorkCurveTemp.ElementList==null
                ||_WorkCurveTemp.ElementList.Items.Count<=0) return;
            foreach (var em in _WorkCurveTemp.ElementList.Items)
            {
                comboBoxWElems.Items.Add(em.Caption);
            }
            if (comboBoxWElems.Items.Count > 0) comboBoxWElems.SelectedIndex = 0;
            if (LstSampleUnCerts.Count > 0) txtSampleUncertainty.Text = LstSampleUnCerts[0];
        }

        private void comboBoxWElems_SelectedValueChanged(object sender, EventArgs e)
        {
            comboBoxWSimSmps.Items.Clear();
            _currentElement = null;
            string strElem = comboBoxWElems.SelectedItem.ToString();
            CurveElement ce = _WorkCurveTemp.ElementList.Items.ToList().Find(w => w.Caption.Equals(strElem));
            if (ce == null || ce.Samples.Count <= 0) return;
            _currentElement = ce;
            foreach (var sp in ce.Samples)
            {
                comboBoxWSimSmps.Items.Add(sp.SampleName);
            }
            comboBoxWSimSmps.SelectedIndex = 0;
        }

        private void comboBoxWSimSmps_SelectedValueChanged(object sender, EventArgs e)
        {
            string standSamp = comboBoxWSimSmps.SelectedItem.ToString();
            StandSample sp = _currentElement.Samples.ToList().Find(w => w.SampleName.Equals(standSamp));
            if (sp == null) return;
            txtStandUncertainty.Text = sp.Uncertainty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (comboBoxWElems.SelectedItem.ToString() == string.Empty
                || txtStandUncertainty.Text.Trim() == string.Empty
                || txtSampleUncertainty.Text.Trim() == string.Empty)
                this.DialogResult = DialogResult.Cancel;
            else this.DialogResult = DialogResult.OK;
        }

        private void btnClickMe_Click(object sender, EventArgs e)
        {
            UCSels ucSels = new UCSels();
            ucSels.Sels = LstSampleUnCerts;
            ucSels.SelectString = txtSampleUncertainty.Text;
            ucSels.StartPosition=FormStartPosition.Manual;
            ucSels.Location = new Point(MousePosition.X, MousePosition.Y);
            if (ucSels.ShowDialog() == DialogResult.OK)
                txtSampleUncertainty.Text = ucSels.SelectString;
        }

    }
}
