using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public partial class FrmSelectStandard : Form
    {
        public CustomStandard CurrentStandard { get; set; }
        public FrmSelectStandard()
        {
            InitializeComponent();
            Load += new EventHandler(FrmSelectStandard_Load);
        }

        void FrmSelectStandard_Load(object sender, EventArgs e)
        {
            dataGridViewW1.SelectionChanged += new EventHandler(dataGridViewW1_SelectionChanged);
            foreach (CustomStandard standard in CustomStandard.FindAll())
            {   
                int index = dataGridViewW1.Rows.Add();
                dataGridViewW1.Rows[index].Cells[0].Value = standard.StandardName;
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < standard.StandardDatas.Count; i++)
                    strBuilder.Append(standard.StandardDatas[i].ElementCaption + "=" + standard.StandardDatas[i].StandardContent + ",");
                dataGridViewW1.Rows[index].Cells[1].Value = strBuilder.ToString();
            }
        }

        void dataGridViewW1_SelectionChanged(object sender, EventArgs e)
        {
            CurrentStandard = CustomStandard.FindAll()[dataGridViewW1.SelectedRows[0].Index];
        }
    }
}
