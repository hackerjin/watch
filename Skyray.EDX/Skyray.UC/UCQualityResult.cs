using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCQualityResult : Skyray.Language.UCMultiple
    {
        public UCQualityResult()
        {
            InitializeComponent();
        }

        public UCQualityResult(string[] elementName, int[] lines):
            this()
        {
            if (elementName.Length == lines.Length)
            {
                for (int i = 0; i < elementName.Length; i++)
                {
                    this.dgvQualityResult.Rows.Add(elementName[i], lines[i] == 0 ? "K" : "L");
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<int> colIndex = new List<int>();
            foreach (DataGridViewColumn column in this.dgvQualityResult.Columns)
            {
                if (column.Visible)
                {
                    colIndex.Add(column.Index);
                }
            }
            List<int> rowIndex = new List<int>();
            foreach (DataGridViewRow row in this.dgvQualityResult.Rows)
            {
                rowIndex.Add(row.Index);
            }

            this.dgvQualityResult.ToPrintCols = colIndex;
            this.dgvQualityResult.ToPrintRows = rowIndex;
            this.dgvQualityResult.PDC = new PrintDocument();
            this.dgvQualityResult.PDC.DocumentName = Info.AnalysisReport;
            this.dgvQualityResult.Print(true);
        }
    }
}
