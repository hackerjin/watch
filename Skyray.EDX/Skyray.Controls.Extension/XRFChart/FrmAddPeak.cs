using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;

namespace Skyray.Controls.Extension
{
    public partial class FrmAddPeak : Skyray.Language.MultipleForm
    {
        public string SelectElement { get; set; }

        public XLine Line { get; set; }

        public double CustomChannel { get; set; }

        public FrmAddPeak()
        {
            InitializeComponent();
            string[] str = Enum.GetNames(typeof(XLine));
            cbwLine.Items.AddRange(str);
            cbwLine.SelectedIndex = 0;
        }

        private void btnSelectElement_Click(object sender, EventArgs e)
        {
            ElementTableAtom table = new ElementTableAtom();
            table.ShowDialog();
            if (table.DialogResult == DialogResult.OK)
            {
                SelectElement = table.SelectedItems == null ? null : table.SelectedItems[0];//元素
                this.txtElementSelected.Text = SelectElement;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SelectElement == null || SelectElement == string.Empty)
                return;
            Line = (XLine)cbwLine.SelectedIndex;
            CustomChannel = double.Parse(numCustomChannel.Value.ToString());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
