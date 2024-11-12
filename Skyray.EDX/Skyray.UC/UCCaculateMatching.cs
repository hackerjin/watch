using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    public partial class UCCaculateMatching :Skyray.Language.UCMultiple
    {
        public UCCaculateMatching()
        {
            InitializeComponent();
        }

        private void btnOpenSpec_Click(object sender, EventArgs e)
        {
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false,false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            if (this.dgvSpecMatching.CurrentCell != null)
            {
                this.dgvSpecMatching.CurrentCell.Value = returnResult[0].Name;
                this.dgvSpecMatching.CurrentCell.Tag = returnResult[0];
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.dgvSpecMatching.Rows.Add();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvSpecMatching.CurrentCell != null)
            {
                int rowIndex = this.dgvSpecMatching.CurrentCell.RowIndex;
                this.dgvSpecMatching.Rows.RemoveAt(rowIndex);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        private void btnCacel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCaculateMatching_Click(object sender, EventArgs e)
        {
            if (this.dgvSpecMatching.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dgvSpecMatching.Rows)
                {
                    try
                    {
                        SpecEntity spec = (row.Cells["OriginalSpec"].Tag as SpecListEntity).Specs[0];
                        SpecEntity comparSpec = (row.Cells["CompareSpec"].Tag as SpecListEntity).Specs[0];
                        row.Cells["MatchLevel"].Value = TabControlHelper.Matching(spec.SpecDatac, 5, 0, comparSpec.SpecDatac);
                    }
                    catch { };
                }
            }
        }
    }
}
