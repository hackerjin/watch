using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    /// <summary>
    /// 能量，中心道
    /// </summary>
    public partial class UCCalcResolution : Skyray.Language.UCMultiple
    {
        private SpecListEntity CalSpecList = new SpecListEntity();
        private int specLength = 0;
        private double rFactor = 1;
        private double ResolveCount;
        /// <summary>
        /// 能量，中心道
        /// </summary>
        public UCCalcResolution()
        {
            InitializeComponent();
            cboSpecID.Items.Add("1");
            cboSpecID.Items.Add("2");
            cboSpecID.Items.Add("3");
        }

        /// <summary>
        /// 能量，中心道
        /// </summary>
        public UCCalcResolution(SpecListEntity specList,Device device,double resolveFactor,double resolvelimit)
        {
            InitializeComponent();
            specLength = (int)device.SpecLength;
            CalSpecList = specList;
            CalSpecList.SpecType = specList.SpecType;
            rFactor = resolveFactor;
            ResolveCount = resolvelimit;
            for (int i = 0; i < specList.Specs.Length;i++ )
            {
                cboSpecID.Items.Add((i + 1).ToString());
            }
            if (cboSpecID.Items.Count>=0)
            {
                
                cboSpecID.SelectedIndex = 0;
                this.txtEnergy.Text = ((SpecHelper.GetHighSpecChannel(0, specLength - 1, CalSpecList.Specs[0].SpecDatas) * DemarcateEnergyHelp.k1 + DemarcateEnergyHelp.k0) * 1000).ToString("f2") + " ev";
            }
        }

        private void btnCalc_Click(object sender, System.EventArgs e)
        {
            if (CalSpecList == null || CalSpecList.Specs.Length<=0)
            {
                return;
            }
            int specId = int.Parse(cboSpecID.Text)-1;
            int[] spec= CalSpecList.Specs[specId].SpecDatas;
            int low=0;
            int high=0;
            double ch = SpecHelper.FitChannOfMaxValue(0, specLength-1, spec, ref low, ref high);
            //double halfValue = spec[(int)ch] * 1.0 / 2;
            //double halfValue = SpecHelper.GuassValueOfMaxValue(0, specLength - 1, spec, ref low, ref high)/2;
            double slope = DemarcateEnergyHelp.k1 * 1000;
            //求半高宽的精确边界
            //if ((low - 1) < 0 || low < 0 || (high - 1) < 0 || high < 0) return;
            //double L = low + (halfValue - spec[low]) / (spec[low + 1] - spec[low]);
            //double H = high - (halfValue - spec[high]) / (spec[high - 1] - spec[high]);
            //txtResolution.Text = ((H - L) * slope).ToString("f1");
            txtResolution.Text = (((SpecHelper.GuassValueOfMaxValue(0, specLength - 1, spec, ref low, ref high) * slope * rFactor) > ResolveCount) ? (SpecHelper.GuassValueOfMaxValue(0, specLength - 1, spec, ref low, ref high) * slope * rFactor) : ResolveCount).ToString("F1");
            txtCenterChannel.Text = ch.ToString("f1");
        }

        private void cboSpecID_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if (CalSpecList == null || CalSpecList.Specs.Length <= 0)
            {
                return;
            }
            int specId =  int.Parse(cboSpecID.Text)-1;
            this.txtEnergy.Text = ((SpecHelper.GetHighSpecChannel(0, specLength - 1, CalSpecList.Specs[specId].SpecDatas) * DemarcateEnergyHelp.k1 + DemarcateEnergyHelp.k0) * 1000).ToString("f2") + " ev";
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }
    }
}
