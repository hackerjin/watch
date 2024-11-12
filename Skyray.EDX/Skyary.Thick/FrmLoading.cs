using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Thick
{
    public partial class FrmLoading :System.Windows.Forms.Form
    {
        public string ShowInformation
        {
            set { lblLoading.Text = value; }
        }
        public FrmLoading()
        {
            InitializeComponent();
        }
    }
}
