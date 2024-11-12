using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.Language;

namespace Skyray.UC
{
    public partial class FrmHotSet : MultipleForm
    {
        public FrmHotSet()
        {
            InitializeComponent();
            InitComboBox();
        }


        private void InitComboBox()
        {
            this.cmbModifier.Items.AddRange(new object[] { "None", "Control", "Shift", "Alt" });
            //this.cmbKey.Items.AddRange(new object[]{Keys.None,Keys.F1,Keys.F2,Keys.F3,Keys.F4,Keys.F5,Keys.F6,Keys.F7,Keys.F8,Keys.F9,Keys.F10,Keys.F11,Keys.F12});
            this.cmbKey.Items.Add(((Keys)0).ToString());
            for (int i = 65; i < 91; i++)
            {
                this.cmbKey.Items.Add(((Keys)i).ToString());
            }
            for (int j = 112; j < 124; j++)
            {
                this.cmbKey.Items.Add(((Keys)j).ToString());
            }

            if (WorkCurveHelper.SaveHistoryKeys != null && WorkCurveHelper.SaveHistoryKeys.ToString() != "None")
            {
                string[] keys = WorkCurveHelper.SaveHistoryKeys.ToString().Split(',');
                foreach (var key in cmbKey.Items)
                {
                    if (key.ToString() == keys[0].Trim())
                    {
                        cmbKey.SelectedIndex = cmbKey.Items.IndexOf(key);
                        break;
                    }
                }
                if (keys.Length > 1)
                    this.cmbModifier.SelectedIndex = cmbModifier.Items.IndexOf(keys[1].Trim());
            }
            else
            {
                cmbKey.SelectedIndex = 0;
                cmbModifier.SelectedIndex = 0;
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cmbKey.Text == "None")
            {
                WorkCurveHelper.SaveHistoryKeys = (Keys)Enum.Parse(typeof(Keys), cmbModifier.Text);
            }
            else if (cmbModifier.Text == "None")
            {
                WorkCurveHelper.SaveHistoryKeys = (Keys)Enum.Parse(typeof(Keys), cmbKey.Text);
            }
            else
            {
                WorkCurveHelper.SaveHistoryKeys = (Keys)Enum.Parse(typeof(Keys), cmbModifier.Text) | (Keys)Enum.Parse(typeof(Keys), cmbKey.Text);
            }
           
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
