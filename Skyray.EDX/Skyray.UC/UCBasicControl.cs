using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    public partial class UCBasicControl : UserControl
    {
        public UCBasicControl()
        {
            InitializeComponent();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            this.Load += new EventHandler(UCBasicControl_Load);
        }

        void UCBasicControl_Load(object sender, EventArgs e)
        {
            this.SizeChanged += new EventHandler(UCBasicControl_SizeChanged);
        }

        void UCBasicControl_SizeChanged(object sender, EventArgs e)
        {
            this.buttonW5.Location =
                new Point((int)(this.buttonW4.Location.X + (this.buttonW3.Location.X - this.buttonW4.Location.X) / 2f),
                    (int)(this.buttonW4.Location.Y + (this.buttonW1.Location.Y - this.buttonW4.Location.Y) / 2f)
                    );
        }

        public bool ShowFlag
        {
            set
            {
                if (value)
                {
                    this.buttonW1.Enabled = value;
                    this.buttonW2.Enabled = value;
                    this.buttonW3.Enabled = value;
                    this.buttonW4.Enabled = value;
                    this.buttonW5.Enabled = value;
                }
                else
                {
                    this.buttonW3.Enabled = !value;
                    this.buttonW1.Enabled = value;
                    this.buttonW2.Enabled = value;
                    this.buttonW4.Enabled = value;
                    this.buttonW5.Enabled = value;
                }
            }
        }

        public bool NotShowFlag
        {
            set
            {
                if (value)
                {
                    this.buttonW1.Enabled = value;
                    this.buttonW2.Enabled = value;
                    this.buttonW3.Enabled = value;
                    this.buttonW4.Enabled = value;
                    this.buttonW5.Enabled = value;
                }
                else
                {
                    this.buttonW3.Enabled = value;
                    this.buttonW1.Enabled = value;
                    this.buttonW2.Enabled = value;
                    this.buttonW4.Enabled = value;
                    this.buttonW5.Enabled = value;
                }
            }
        }


        private void buttonW1_Click(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition != null && WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count > 0)
            {
                FrmMeasureSetting returnNew = new FrmMeasureSetting();
                WorkCurveHelper.OpenUC(returnNew,true,Info.MeasureTime,true);
            }
        }

       

        private void buttonW2_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GetReturnSpectrum(true, false);
        }

        private void buttonW3_Click(object sender, EventArgs e)
        {
            UCVirtualSelect selectVirtual = new UCVirtualSelect();
            WorkCurveHelper.OpenUC(selectVirtual, false, Info.OpenVirtualSpec,true);
        }

        private void buttonW4_Click(object sender, EventArgs e)
        {
            UCCurve returnControl = new UCCurve();
            WorkCurveHelper.OpenUC(returnControl, true, Info.OpenCurve,true);
        }

        private void buttonW5_Click(object sender, EventArgs e)
        {
            DifferenceDevice.MediumAccess.ExcuteCaculate();
        }

        private void buttonW1_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.RemoveAll();
            this.toolTip1.SetToolTip(this.buttonW1, Info.SettingMeasureTime);
        }

        private void buttonW2_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.RemoveAll();
            this.toolTip1.SetToolTip(this.buttonW2, Info.OpenWorkSpec);
        }

        private void buttonW3_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.RemoveAll();
            this.toolTip1.SetToolTip(this.buttonW3, Info.OpenVirtualSpec);
        }

        private void buttonW4_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.RemoveAll();
            this.toolTip1.SetToolTip(this.buttonW4, Info.OpenCurve);
        }

        private void buttonW5_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.RemoveAll();
            this.toolTip1.SetToolTip(this.buttonW5, Info.Caculate);
        }
    }
}
