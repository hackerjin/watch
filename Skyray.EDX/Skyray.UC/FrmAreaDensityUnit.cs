using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lephone.Data.Definition;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;


namespace Skyray.UC
{
    public partial class FrmAreaDensityUnit : Skyray.Language.MultipleForm
    {
        public readonly string[] IllegalChars = new string[] { @"\", "?", ":", "<", ">", "|", "*", "''" };
        public FrmAreaDensityUnit()
        {
            InitializeComponent();
            dgvUnits.Columns.Add(Info.strAreaDensity,Info.strAreaDensity);
            dgvUnits.Columns.Add(Info.Summary, Info.Summary);

            List<AreaDensityUnit> units = AreaDensityUnit.FindAll();
            for (int i = 0; i < units.Count; i++)
            {
               int index = dgvUnits.Rows.Add();
               dgvUnits.Rows[index].Cells[Info.strAreaDensity].Value = units[i].Name;
               dgvUnits.Rows[index].Cells[Info.Summary].Value = "(" + Info.strAreaDensityUnit + ")/"+units[i].cofeK;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUnitName.Text.Trim().IsNullOrEmpty() || !txtCoefK.Text.IsNum()||txtUnitName.Text.Trim().Equals(Info.strAreaDensityUnit)) return;
            foreach (string s in IllegalChars)
            {
                if (txtUnitName.Text.Contains(s)) return;
            }
            AreaDensityUnit newUnit = AreaDensityUnit.New.Init(txtUnitName.Text.Trim(), txtCoefK.Text.Trim(), "");
            List<AreaDensityUnit> units = AreaDensityUnit.FindAll();
            if (units.Find(w => w.Name == newUnit.Name) != null) return;
            newUnit.Save();
            int index = dgvUnits.Rows.Add();
            dgvUnits.Rows[index].Cells[Info.strAreaDensity].Value = newUnit.Name;
            dgvUnits.Rows[index].Cells[Info.Summary].Value = "(" + Info.strAreaDensityUnit + ")/" + newUnit.cofeK ;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUnits.SelectedRows.Count <= 0) return;
            if (Msg.Show(Info.ConfirmDel,"",MessageBoxButtons.OKCancel) != DialogResult.OK) return;
            string strName=dgvUnits.SelectedRows[0].Cells[Info.strAreaDensity].Value.ToString();
            List<AreaDensityUnit> units = AreaDensityUnit.FindAll();
            AreaDensityUnit delUnit = units.Find(w => w.Name == strName);
            if (delUnit != null)
            {
                delUnit.Delete();
            }
            for (int i = dgvUnits.SelectedRows.Count; i > 0; i--)
            {
                dgvUnits.Rows.RemoveAt(dgvUnits.SelectedRows[i-1].Index);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

    }
}
