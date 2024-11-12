using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.UC;
using Skyray.Language;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Skyray.EDX.Common;
namespace Skyray.UC
{
    public partial class FrmBlueToothCfg : Skyray.Language.UCMultiple
    {
        public FrmBlueToothCfg()
        {
            InitializeComponent();
          
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            lblstate.Text = Info.BlueScaning;
            Application.DoEvents();
            cbmDevices.DataSource = DifferenceDevice.interClassMain.bluePrint.ScanDevices();
            cbmDevices.DisplayMember = "DeviceName";
            cbmDevices.ValueMember = "DeviceAddress";
            lblstate.Text = Info.BlueScanComplete;
            Application.DoEvents();
        }

        private void btnCon_Click(object sender, EventArgs e)
        {
            DifferenceDevice.interClassMain.bluePrint.SelectDevice((BluetoothAddress)cbmDevices.SelectedValue);
            WorkCurveHelper.PrintName = cbmDevices.SelectedValue.ToString();
            ReportTemplateHelper.SaveSpecifiedValueandCreate("Report", "PrintName",  cbmDevices.SelectedValue.ToString());
            if (DifferenceDevice.interClassMain.bluePrint.Connect())
            {
                lblstate.Text = Info.ConnectDevice;
            }
            else
            {
                lblstate.Text = Info.NoBlueTooth;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }


       

       
    }
}
