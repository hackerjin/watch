using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;

namespace Skyray.UC
{
    public partial class FrmStatus : MultipleForm
    {
        private bool bContiue = false;
        public bool Continue
        {
            get { return bContiue; }
            set { bContiue = value; }
        }
        public FrmStatus()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < progressBar1.Maximum)
                progressBar1.Value += 1;
            else
            {
                timer1.Enabled = false;
                this.Close();
            }
        }
    }
}
