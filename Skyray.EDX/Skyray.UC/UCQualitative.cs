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

namespace Skyray.UC
{
    public delegate void CellClickElement(string elementName);

    public partial class UCQualitative : Skyray.Language.UCMultiple
    {
        public object DataSource { get; set; }

        public bool Enable
        {
            set
            {
                this.dataGridViewW2.Enabled = value;
            }
        }

        //public event EventDelegate.NotifyAddElements onAddElements;
        public event CellClickElement OnCellClick;

        public UCQualitative()
        {
            InitializeComponent();
            //this.dataGridViewW1.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
            //this.dataGridViewW1.DefaultCellStyle.BackColor = System.Drawing.Color.Beige;

            this.dataGridViewW2.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
            this.dataGridViewW2.DefaultCellStyle.BackColor = System.Drawing.Color.Beige;
            this.dataGridViewW3.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
            this.dataGridViewW3.DefaultCellStyle.BackColor = System.Drawing.Color.Beige;
        }

        private void dataGridViewW2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridViewW3.Rows.Clear();
            string elementName = this.dataGridViewW2.CurrentCell.Value.ToString();
            CurveElement element = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.First(w => w.Caption == elementName);
            foreach (ElementRef str in element.ElementRefs)
            {
                if (str.IsRef)
                    this.dataGridViewW3.Rows.Add(str.Name);
            }
            if (OnCellClick != null)
                OnCellClick(elementName);
        }

        private void ToolStripMenuItemEditElement_Click(object sender, EventArgs e)
        {
            UCElement ucElement = new UCElement();
            WorkCurveHelper.OpenUC(ucElement, true, Info.EditElement,true);
        }
    }
}
