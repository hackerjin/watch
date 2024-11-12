using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary.Define;
using Lephone.Data.Common;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCStop : Skyray.Language.MultipleForm
    {
       
        public UCStop()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.DialogResult = DialogResult.None;
        }

        private void btnStopTest_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnSuspendTest_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }




    }
}
