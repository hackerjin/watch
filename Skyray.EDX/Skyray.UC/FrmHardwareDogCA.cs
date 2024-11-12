using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class FrmHardwareDogCA : Skyray.Language.MultipleForm
    {
        private int type = -4;  //type为0，为可以进入主程序；其他均不能进入      
        public FrmHardwareDogCA(int Type)
        {
            InitializeComponent();
            type = Type;
            this.txtDevSN.Text = HardwareDog.Sn;
            this.txtDevState.Text = HardwareDog.Deadline;                     
            this.ControlBox = false;
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            HardwareDog.SNActive(this.txtActiveDecode.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            HardwareDog.SNClose();
            if (type == 0||type==-1)
            {
                
                this.Close();
                
            }
            else
            {
                System.Environment.Exit(0);
            }
            
        }

        //private void cbSNItem_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.cbDevItem.SelectedIndex == 1 || this.cbDevItem.SelectedIndex == 2)
        //    {
        //        this.btnActive.Enabled = false;
        //    }
        //    else
        //    {
        //        this.btnActive.Enabled = true;
        //    }
        //}

        private void buttonW1_Click(object sender, EventArgs e)
        {
            HardwareDog.GetVersion();
            textBox4.Text = Info.HVer + ":" + HardwareDog.Hwver.ToString() + " ; " + Info.SVer + ":" + HardwareDog.Swver.ToString();
            HardwareDog.GetRealTime();
            textBox2.Text = HardwareDog.RealTime.ToString();
            HardwareDog.GetEndTime();
            textBox3.Text = HardwareDog.EndTime.ToString();
            HardwareDog.GetPartSN();
            textBox5.Text = HardwareDog.PartSNs[0].ToString();
            textBox6.Text = HardwareDog.PartSNs[1].ToString();
            textBox7.Text = HardwareDog.PartSNs[2].ToString();
        }
        const string SNSTRING = "授权码为：";
        private void btnselect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "读取授权文件";
            ofd.Filter = "(*.txt)|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] allLines = System.IO.File.ReadAllLines(ofd.FileName, System.Text.Encoding.Default);
                    foreach (string str in allLines)
                    {
                        if (str.Contains(SNSTRING))
                        {
                            this.txtActiveDecode.Text = str.Split('：')[1];
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
