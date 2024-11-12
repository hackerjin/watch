using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.UC
{
    public partial class FrmNewPointsName : Skyray.Language.MultipleForm
    {

        public string Name
        {
            set
            {
                txtMultiName.Text = value;
            }
            get
            {

                return txtMultiName.Text == null ? "default" : txtMultiName.Text;
            }
        }
        public FrmNewPointsName()
        {
            InitializeComponent();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {

        }

        private void FrmNewPointsName_Load(object sender, EventArgs e)
        {
            txtMultiName.Text = "default";
        }
    }
}
