using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.Controls;

namespace Skyray.UC
{
    public partial class FrmCheckInCal : MultipleForm
    {
        private int _calibrateType = 2;//1,纯元素 2,控样
        private int _InitialOrCalibrate = 1;//1,Initial 2,Calibrate
        public int CalibrateType
        {
            get { return _calibrateType; }
        }
        public int InitialOrCalibrate
        {
            get { return _InitialOrCalibrate; }
        }
        public FrmCheckInCal()
        {
            InitializeComponent();
            switch (_calibrateType)
            {
                case 1:
                    radioPure.Checked = true;
                    break;
                case 2:
                    radioConSample.Checked = true;
                    radioInitialPar.Checked = true;
                    break;
               default:
                    radioConSample.Checked = true;
                    radioInitialPar.Checked = true;
                    break;
            }
        }
        public FrmCheckInCal(int  iCalType)
        {
            InitializeComponent();
            _calibrateType = iCalType;
            switch (_calibrateType)
            {
                case 1:
                    radioPure.Checked = true;
                    break;
                case 2:
                    radioConSample.Checked = true;
                    radioInitialPar.Checked = true;
                    break;
                default:
                    radioConSample.Checked = true;
                    radioInitialPar.Checked = true;
                    break;
            }
        }
        private void radioPure_CheckedChanged(object sender, EventArgs e)
        {
            if (radioPure.Checked)
            {
                _calibrateType = 1;
                groupBox1.Visible = false;
            }
        }

        private void radioConSample_CheckedChanged(object sender, EventArgs e)
        {
            if (radioConSample.Checked)
            {
                _calibrateType = 2;
                groupBox1.Visible = true;
                if (radioInitialPar.Checked) _InitialOrCalibrate = 1;
                else _InitialOrCalibrate = 2;

            }
        }

        private void radioInitialPar_CheckedChanged(object sender, EventArgs e)
        {
            if (radioInitialPar.Checked) _InitialOrCalibrate = 1;
        }

        private void radioCalibratePar_CheckedChanged(object sender, EventArgs e)
        {
            if (radioCalibratePar.Checked) _InitialOrCalibrate = 2;
        }
    }
}
