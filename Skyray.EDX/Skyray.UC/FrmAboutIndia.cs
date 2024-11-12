using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.UC
{
    public partial class FrmAboutIndia : Skyray.Language.UCMultiple
    {
        public FrmAboutIndia()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }
    }
}
