using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.Language;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.Component;

namespace Skyray.UC
{
    public partial class UCChamberMove : UCMultiple
    {
        private QuantaMotor ChamberMotor;
        private List<Chamber> listChamber = new List<Chamber>();
        public static UCChamberMove chamberMotor;

        public static UCChamberMove CreateSingleInstance()
        {
            if (chamberMotor == null)
                chamberMotor = new UCChamberMove();
            return chamberMotor;
        }

        public override void OpenUC(bool flag, string TitleName, bool isModel, bool noneStyle)
        {
            if (this.IsSignlObject) return;
            Form form = new Form();
            form.BackColor = Color.White;
            form.Text = Info.MoveWorkStation;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.ShowInTaskbar = false;
            int padSpace = 0;
            form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
            form.FormClosing += (s, ex) =>
            {
                this.IsSignlObject = false;
                chamberMotor = null;
            };
            form.Controls.Add(this);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.ClientSize = new Size(this.Width + padSpace * 2, this.Height + padSpace * 2);
            form.ShowIcon = false;
            form.TopMost = true;
            this.Dock = DockStyle.Fill;
            form.StartPosition = FormStartPosition.CenterScreen;
            if (isModel)
            {
                form.ShowDialog();
            }
            else
            {
                form.Show();
            }
            //form.Show();
            this.IsSignlObject = true;
        }

        public UCChamberMove()
        {
            InitializeComponent();
            if (WorkCurveHelper.DeviceCurrent.HasChamber && WorkCurveHelper.DeviceCurrent.Chamber.Count > 0)
            {
                listChamber = WorkCurveHelper.DeviceCurrent.Chamber.ToList();
                foreach (Chamber tempBer in listChamber)
                    this.comboBox1.Items.Add(tempBer.Num);
                ChamberMotor = new QuantaMotor(MotorName.OfChamber, WorkCurveHelper.deviceMeasure.interfacce.port, new object(),
                   WorkCurveHelper.DeviceCurrent.ChamberSpeed);
                if (WorkCurveHelper.DeviceCurrent.HasChamber)
                {
                    ChamberMotor.Exist = true;
                    ChamberMotor.Target = new int[listChamber.Count];
                    ChamberMotor.ID = WorkCurveHelper.DeviceCurrent.ChamberElectricalCode;
                    ChamberMotor.DirectionFlag = WorkCurveHelper.DeviceCurrent.ChamberElectricalDirect;
                    ChamberMotor.DefSpeed = WorkCurveHelper.DeviceCurrent.ChamberSpeed;
                    ChamberMotor.OnMoveStop += new EventHandler<MotorMoveStopEvent>(ChamberMotor_OnMoveStop);
                    int cout = 0;
                    foreach (Chamber chmber in listChamber)
                    {
                        ChamberMotor.Target[cout] = chmber.Step;
                        cout++;
                    }
                }
                this.comboBox1.SelectedIndex = 0;
            }
        }

        void ChamberMotor_OnMoveStop(object sender, MotorMoveStopEvent e)
        {
            MessageFormat format = new MessageFormat();
            format = new MessageFormat(Info.ChamberMotorEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(format);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listChamber != null && listChamber.Count > 0)
            {
                Chamber ber = listChamber.Find(w => w.Num == int.Parse(this.comboBox1.Text));
                if (ber != null)
                    this.lblStep.Text = ber.Step.ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            //if (!WorkCurveHelper.deviceMeasure.interfacce.port.is)
            //{
            //    //MessageFormat message = new MessageFormat(Info.NoDeviceConnect, 0);
            //    //WorkCurveHelper.specMessage.localMesage.Add(message);
            //    //EDXRFHelper.GotoMainPage(this);
            //    //return;
            //    Msg.Show(Info.NoDeviceConnect);
            //}
            if (listChamber != null && listChamber.Count > 0)
            {
                Chamber ber = listChamber.Find(w => w.Num == int.Parse(this.comboBox1.Text));
                if (ber != null)
                {
                    if (WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))
                    {
                        ChamberMotor.Index = -1;
                    }
                    ChamberMotor.MoveTo(ber.Num);
                    WorkCurveHelper.deviceMeasure.interfacce.ChamberMotor.Index=ChamberMotor.Index;
                }
                lblChamberIndex.Text = Info.ChamberIndex + ":" + ChamberMotor.Index;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //if (!WorkCurveHelper.deviceMeasure.interfacce.port.Connect())
            //{
            //    //MessageFormat message = new MessageFormat(Info.NoDeviceConnect, 0);
            //    //WorkCurveHelper.specMessage.localMesage.Add(message);
            //    //EDXRFHelper.GotoMainPage(this);
            //    //return;
            //    Msg.Show(Info.NoDeviceConnect);
            //}
            //WorkCurveHelper.deviceMeasure.interfacce.port.ResetChamber(WorkCurveHelper.DeviceCurrent.ChamberElectricalCode, WorkCurveHelper.DeviceCurrent.ChamberElectricalDirect, WorkCurveHelper.DeviceCurrent.ChamberSpeed);
            //ChamberMotor.Index = WorkCurveHelper.DeviceCurrent.ChamberMaxNum;
            //lblChamberIndex.Text = Info.ChamberIndex + ":" + ChamberMotor.Index;
            //if (listChamber != null && listChamber.Count > 0)
            //{
            //    Chamber ber = listChamber.Find(w => w.Num == 1);
            //    if (ber != null)
            //    {
            //        if (WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))
            //        {
            //            ChamberMotor.Index = -1;
            //        }
            //        ChamberMotor.MoveTo(ber.Num);
            //        WorkCurveHelper.deviceMeasure.interfacce.ChamberMotor.Index = ChamberMotor.Index;
            //    }
            //    lblChamberIndex.Text = Info.ChamberIndex + ":" + ChamberMotor.Index;
            //}

            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ResetChamber));
            thread.IsBackground = true;
            thread.Priority = System.Threading.ThreadPriority.Highest;
            thread.Name = this.Name.ToString();
            thread.Start();
            
        }
        private delegate void FlushClient();
        private void ResetChamber()
        {
            
            int i = WorkCurveHelper.deviceMeasure.interfacce.port.ResetChamber(WorkCurveHelper.DeviceCurrent.ChamberElectricalCode, WorkCurveHelper.DeviceCurrent.ChamberElectricalDirect, WorkCurveHelper.DeviceCurrent.ChamberSpeed);
            WorkCurveHelper.deviceMeasure.interfacce.ChamberMotor.Index = i;
            FlushClient fc=delegate(){ChamberMotor.Index= i;lblChamberIndex.Text = Info.ChamberIndex + ":" + ChamberMotor.Index;};
            lblChamberIndex.Invoke(fc);
            
        }
    }
}
