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
    public partial class UCSels : MultipleForm
    {
        public List<string> Sels = null;
        public UCSels()
        {
            InitializeComponent();
        }
        public string SelectString = string.Empty;
        private void labeltemp_Click(object sender, EventArgs e)
        {
            SelectString=((Skyray.Controls.LabelW)sender).Text;
            this.DialogResult = DialogResult.OK;
        }

        private void UCSels_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            if (Sels.Count > 0)
            {
                int i = 0;
                int locationX = 8;
                int locationY = 8;
                int row=5,col=1;
                foreach (var sel in Sels)
                {
                    Skyray.Controls.LabelW labeltemp = new LabelW();
                    labeltemp.FlatStyle = FlatStyle.Popup;
                    labeltemp.TextAlign = ContentAlignment.MiddleCenter;
                    labeltemp.BorderStyle = BorderStyle.FixedSingle;
                    labeltemp.Text = sel;
                    labeltemp.BackColor=Color.FromArgb(175, 210, 255);
                    labeltemp.Size = new Size(60, 24);
                    labeltemp.Font = new Font(this.Font, FontStyle.Bold);
                    this.Controls.Add(labeltemp);
                    labeltemp.Click+=new EventHandler(labeltemp_Click);
                    labeltemp.Location = new Point(locationX, locationY);
                    if (SelectString != null && SelectString.Equals(sel))
                    {
                        labeltemp.Focus();
                        labeltemp.BackColor = Color.Orange;
                    }
                    i++;
                    locationY = i % 5 * 30 + 8;
                    locationX = i / 5 * 70 + 8;
                }
                col=i/5+1;
                this.Size = new Size(col * 70 + 16, row * 30 + 32);
            }
        }
    }
}
