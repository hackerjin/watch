using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using System.Reflection;

namespace Skyray.UC
{
    public partial class UCSmartGraphic : UserControl
    {
        private int _mode;

        public int Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
            }
        }

        public bool Flag
        {
            set
            {
                if (value)
                {
                    //换成开始图片
                    this.buttonW1.Image = global::Skyray.UC.Properties.Resources.ssb;
                    toolTip1.SetToolTip(buttonW1, Info.Start);
                    toolTip1.ReshowDelay = 0;
                    //this.buttonW1.Enabled = true;
                    this.buttonW2.Enabled = false;
                }
                else
                {
                    this.buttonW1.Image = global::Skyray.UC.Properties.Resources.paau;
                    toolTip1.SetToolTip(buttonW1, Info.PauseStop);
                    toolTip1.ReshowDelay = 0;
                    //this.buttonW1. PauseStop
                    //换成暂停图片
                    //this.buttonW1.Enabled = false;
                    this.buttonW2.Enabled = true;
                }
            }
        }
        public UCSmartGraphic()
        {
            InitializeComponent();
            string info = Info.Start;
            toolTip1.SetToolTip(buttonW1, info);
            toolTip1.ReshowDelay = 0;
            this.buttonW2.Enabled = false;
        }

        public void ChangeScale(double xMAx, double xMin) //yuzhao20150624:X轴坐标变化事件
        {

            smartGraphic1.ChangeShadowRect(xMin, xMAx);
        }

        private void buttonW1_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.Idel)
            {
                if (WorkCurveHelper.WorkCurveCurrent == null)
                {
                    Msg.Show(Info.WarningTestContext, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (DifferenceDevice.IsAnalyser && WorkCurveHelper.ButtonDirectRun) 
                {
                    NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == "TestSetting");
                    if (item != null) 
                    {
                        MethodInfo miHandler = item.GetType().GetMethod("ProcessCommon", BindingFlags.NonPublic | BindingFlags.Instance);
                        miHandler.Invoke(item, new object[] { item.MenuStripItem, null });
                        return;
                    }
                }
                NewWorkSpec returnNew = NewWorkSpec.GetInstance(Mode);
                WorkCurveHelper.OpenUC(returnNew, true, Info.Start,true);
            }
            else if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.Pause)
            {
                //换成开始图片
                this.buttonW1.Image = global::Skyray.UC.Properties.Resources.paau;
                toolTip1.SetToolTip(buttonW1, Info.PauseStop);
                toolTip1.ReshowDelay = 0;
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.State = DeviceState.Resume;
            }
            else if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.State == DeviceState.Motoring)
            { }
            else
            {
                this.buttonW1.Image = global::Skyray.UC.Properties.Resources.ssb;
                //换成开始图片
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.State = DeviceState.Pause;
                toolTip1.SetToolTip(buttonW1, Info.Continue);
                toolTip1.ReshowDelay = 0;
            }

            DifferenceDevice.interClassMain.AutoIncrease();
            
        }

        

        private void buttonW2_Click(object sender, EventArgs e)
        {
            //换成开始图片
            this.buttonW1.Image = global::Skyray.UC.Properties.Resources.ssb;
            toolTip1.SetToolTip(buttonW1, Info.Start);
            toolTip1.ReshowDelay = 0;
            //this.buttonW1.Enabled = true;
            this.buttonW2.Enabled = false;
            DifferenceDevice.MediumAccess.TestStop();
        }

        private void buttonW1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!buttonW1.Enabled)
                return;
            //string info = Info.Start;
            //toolTip1.SetToolTip(buttonW1, info);
            //toolTip1.ReshowDelay = 0;

        }

        private void buttonW2_MouseMove(object sender, MouseEventArgs e)
        {
            if (!buttonW2.Enabled)
                return;
            string info = Info.StopTest;
            toolTip1.SetToolTip(buttonW2, info);
            toolTip1.ReshowDelay = 0;
        }
    }
}
