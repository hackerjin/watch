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
    public partial class FrmTip : Form
    {
        public  FrmTip()
        {
            InitializeComponent();
        }
        public FrmTip(string text, int UsedTime)
        {
            InitializeComponent();
            WaitTime = UsedTime;
            this.labelW1.Text = text;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Enabled = true;
            
        }
        private Timer timer = null;
        private int WaitTime = 0;
        private int AlreadyWaitTime = 0;
        public void ShowDialog(string text)
        {
            this.labelW1.Text = text;
            this.ShowDialog();
        }
        private void timer_Tick(object source, EventArgs e)
        {
            //timer.Enabled = false;
            AlreadyWaitTime++;
            if (AlreadyWaitTime >= WaitTime)
            {
                timer.Enabled = false;
                this.Close();
            }
           // timer.Enabled = true;
        }
    }

    public class TipWindow
    {
        static FrmTip frmTip;

        static TipWindow()
        {
        }

        public static void ShowDialog(string text)
        {
            if (frmTip == null)
               frmTip = new FrmTip();
            frmTip.ShowDialog(text);
        }
        delegate void updateDelegate();
        public static void Dispose()
        {
            frmTip.Invoke(new updateDelegate(() => {
                if (frmTip != null)
                {
                    frmTip.Dispose();
                    frmTip.Close();
                }
            }));
        }
    }
}
