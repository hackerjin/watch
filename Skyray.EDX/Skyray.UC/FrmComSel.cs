using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class FrmComSel : Skyray.Language.UCMultiple
    {
        public string sPortName;
        public int iFrequency;
        public int iTimeOut;

        public FrmComSel()
        {
            InitializeComponent();
            string[] portlist = SerialPort.GetPortNames();
            foreach (string ss in portlist)
            {
                cboComList.Items.Add(ss);
                if (sPortName != string.Empty && ss.Equals(sPortName)) cboComList.SelectedItem = ss;
            }
        }
        

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cboComList.SelectedItem == null)
            {
                MessageBox.Show("请选择串口名");
                return;
            }
            WorkCurveHelper.sPortName = cboComList.SelectedItem.ToString();
            WorkCurveHelper.iFrequency = Convert.ToInt32(tbFrequency.Text);
            WorkCurveHelper.iTimeOut = Convert.ToInt32(tbTimeout.Text);

            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/ComSel", WorkCurveHelper.sPortName);
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/Frequency", WorkCurveHelper.iFrequency.ToString());
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/TimeOut", WorkCurveHelper.iTimeOut.ToString());
            MotorOperator.StopPort();
            MotorOperator.StartPort();

            EDXRFHelper.GotoMainPage(this);
           
            
        }

        private void btnCanel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);

        }

        private void FrmComSel_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < cboComList.Items.Count; i++)
            {
                if (cboComList.Items[i].ToString() == WorkCurveHelper.sPortName)
                {
                    cboComList.SelectedItem = cboComList.Items[i];
                }

            }
            
        }

       

       

        
    }
}
