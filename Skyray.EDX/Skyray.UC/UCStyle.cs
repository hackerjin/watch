using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.Controls;

namespace Skyray.UC
{
    public partial class UCStyle :UCMultiple
    {
        public UCStyle()
        {
            InitializeComponent();
             Type style = typeof(Style);
             foreach (string s in Enum.GetNames(style))
             {
                 if (s == "Office2007Blue" || s == "Office2007Sliver")
                 this.cbStyleList.Items.Add(s);
             }
        }

        private void btWSubmit_Click(object sender, EventArgs e)
        {
            if (this.cbStyleList.Text == "") return;
            Style currentStyle = (Style)Enum.Parse(typeof(Style), this.cbStyleList.Text);
            DifferenceDevice.interClassMain.SetStyle(currentStyle);
            EDXRFHelper.GotoMainPage(this);
        }

        private void btwCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }
    }
}
