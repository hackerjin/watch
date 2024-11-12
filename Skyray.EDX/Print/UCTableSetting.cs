using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Print
{
    public partial class UCTableSetting : Skyray.Language.UCMultiple
    {
        public DialogResult returnOk = DialogResult.Cancel;
        public int RowCount;
        public int ColCount;

        public UCTableSetting()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            CancelButtonClick();
            this.RowCount = (int)this.numericUpDown1.Value;
            this.ColCount = (int)this.numericUpDown2.Value;
            returnOk = DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelButtonClick();
        }

        public void CancelButtonClick()
        {
            var form = this.ParentForm;
            if (form != null) form.Close();
        }
    }
}
