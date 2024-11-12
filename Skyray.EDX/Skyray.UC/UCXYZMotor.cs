using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common.Component;
using Skyray.EDX.Common;
using System.Runtime.InteropServices;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public partial class UCXYZMotor : Skyray.Language.UCMultiple
    {

        private string _grpText;

        public string GrpText
        {
            get { return _grpText; }
            set { _grpText = value;
            this.grBText.Text = value;
            }
        }

        private Image _leftImage;

        public Image LeftImage
        {
            get { return _leftImage; }
            set { _leftImage = value;
            this.btnAxisLeft.Image = value;
            }
        }

        private Image _rightImage;

        public Image RightImage
        {
            get { return _rightImage; }
            set { _rightImage = value;
            this.btnAxisRight.Image = value;
            }
        }

        private Image _stopImag;

        public Image StopImag
        {
            get { return _stopImag; }
            set { _stopImag = value;
            this.btnStop.Image = value;
            }
        }
   

        private bool _showStop;

        public bool ShowStop
        {
            get { return _showStop; }
            set { _showStop = value; }
        }

        private bool _enableDisable;

        public bool EnableDisable
        {
            get { return _enableDisable; }
            set { _enableDisable = value;
            this.grBText.Enabled = value;
            }
        }

        private bool _isShowTools;

        public bool IsShowTools
        {
            get { return _isShowTools; }
            set { _isShowTools = value; }
        }

        private MoterType _moterType;

        public MoterType MoterType
        {
            get { return _moterType; }
            set { _moterType = value; }
        }

        private int _motorStep;

        public int MotorStep
        {
            get { return _motorStep; }
            set { _motorStep = value; }
        }

        private bool _isShowScroll;

        public bool IsShowScroll
        {
            get { return _isShowScroll; }
            set { _isShowScroll = value;
            this.vScrollXYSpeed.Visible = value;
            this.lblXYSpeedw1.Visible = value;
            }
        }

        public UCXYZMotor()
        {
            InitializeComponent();
            this.btnAxisLeft.Image = this._leftImage;
            this.btnAxisRight.Image = this._rightImage;
            this.grBText.Enabled = this._enableDisable;
            this.grBText.Text = this.GrpText;
            this._motorStep = CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP;
        }

        public void RefrenshEnable()
        {
            if (WorkCurveHelper.DeviceCurrent == null)
                return;
            if (_moterType == MoterType.XMotor && WorkCurveHelper.DeviceCurrent.HasMotorX)
            {
                this.grBText.Enabled = true;
            }
            else if (_moterType == MoterType.YMotor && WorkCurveHelper.DeviceCurrent.HasMotorY)
            {
                this.grBText.Enabled = true;
            }
            else if (_moterType == MoterType.ZMotor && WorkCurveHelper.DeviceCurrent.HasMotorZ)
            {
                this.grBText.Enabled = true;
            }
            else
            {
                this.grBText.Enabled = false;
            }
        }

        private void UCXYZMotor_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            RefrenshEnable();
        }
        bool IsResetting = false;
        private void btnAxisRight_MouseUp(object sender, MouseEventArgs e)
        {
            
            if (e.Button != MouseButtons.Left)
                return;
            if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                    && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null)
            {
                switch (MoterType)
                {
                    case MoterType.XMotor:
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect-1), 0, true,250- WorkCurveHelper.DeviceCurrent.MotorXSpeed);
                        break;
                    case MoterType.YMotor:
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect-1),0, true, 250-WorkCurveHelper.DeviceCurrent.MotorYSpeed);
                        break;
                    case MoterType.ZMotor:
                        if (WorkCurveHelper.DoorCloseType == 0)
                        {
                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1), 0, true, 250 - WorkCurveHelper.DeviceCurrent.MotorZSpeed);

                            //查询盖子有没有盖上
                            if (WorkCurveHelper.DeviceCurrent.HasChamber
                                && WorkCurveHelper.DeviceCurrent.Chamber.ToList().Count >= WorkCurveHelper.DeviceCurrent.ChamberMaxNum
                                && WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))
                            {
                                if (IsResetting) return;
                                IsResetting = true;
                                //DifferenceDevice.interClassMain.deviceMeasure.interfacce.GetReturnParams();
                                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(DoChamberReset));
                                thread.IsBackground = true;
                                thread.Priority = System.Threading.ThreadPriority.Highest;
                                thread.Name = this.Name.ToString();
                                thread.Start();
                            }
                        }
                        break;

                }
            }
        }

        private void DoChamberReset()
        {           
           
            try
            {
                //System.Threading.Thread.Sleep(500);
                if (WorkCurveHelper.type == InterfaceType.NetWork)
                {
                    double voltage = 0;
                    double current = 0;
                    int cover = 0;
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.getParam(out voltage, out current, out cover);
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnCoverClosed = cover != 0;
                }
                else DifferenceDevice.interClassMain.deviceMeasure.interfacce.GetReturnParams();
                if (!DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnCoverClosed)
                {
                   // System.Threading.Thread.Sleep(500);
                    int steps = 0;
                    int chamindex = -1;
                    bool istruestep = true;
                    bool isinitialed = false;
                    bool isbusy = false;
                    WorkCurveHelper.deviceMeasure.interfacce.port.GetChamberStatus(ref steps, ref chamindex, ref istruestep, ref isinitialed, ref isbusy);
                    if (istruestep)
                    {
                        IsResetting = false;
                        return;
                    }
                    //System.Threading.Thread.Sleep(500);
                    int i = WorkCurveHelper.deviceMeasure.interfacce.port.ResetChamber(WorkCurveHelper.DeviceCurrent.ChamberElectricalCode, WorkCurveHelper.DeviceCurrent.ChamberElectricalDirect, WorkCurveHelper.DeviceCurrent.ChamberSpeed);
                    WorkCurveHelper.deviceMeasure.interfacce.ChamberMotor.Index = i;
                }
            }
            catch
            {}
            IsResetting = false;
        }

        private void btnAxisRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                    && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null)
            {
                switch (MoterType)
                {
                    case MoterType.XMotor:
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect-1), this.MotorStep, true,250- WorkCurveHelper.DeviceCurrent.MotorXSpeed);
                        break;
                    case MoterType.YMotor:
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect-1), this.MotorStep, true, 250-WorkCurveHelper.DeviceCurrent.MotorYSpeed);
                        break;
                    case MoterType.ZMotor:
                        
                          DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1), this.MotorStep, true, 250 - WorkCurveHelper.DeviceCurrent.MotorZSpeed);
                        //查询盖子有没有盖上6000B------未修改 6000B仪器只适用0
                        ////查询盖子有没有盖上
                        //if (WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))
                        //{
                        //    DifferenceDevice.interClassMain.deviceMeasure.interfacce.GetReturnParams();
                        //    if (!DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnCoverClosed)
                        //    {
                        //        WorkCurveHelper.deviceMeasure.interfacce.port.ResetChamber(WorkCurveHelper.DeviceCurrent.ChamberElectricalCode, WorkCurveHelper.DeviceCurrent.ChamberElectricalDirect, WorkCurveHelper.DeviceCurrent.ChamberSpeed);
                        //    }
                        //}
                            break;

                }
            }
        }

        private void vScrollXYSpeed_ValueChanged(object sender, EventArgs e)
        {
            lblXYSpeedw1.Text = vScrollXYSpeed.Value.ToString();
            lblXYSpeedw1.Text = Convert.ToString(170 - vScrollXYSpeed.Value);
            int xyValue = 170 - vScrollXYSpeed.Value;
            if ((this._moterType == MoterType.XMotor&&WorkCurveHelper.DeviceCurrent.HasMotorX) || (this.MoterType == MoterType.YMotor && WorkCurveHelper.DeviceCurrent.HasMotorY))
            {
                WorkCurveHelper.DeviceCurrent.MotorXSpeed = xyValue;
                WorkCurveHelper.DeviceCurrent.MotorYSpeed = xyValue;
                string sql = "Update Device Set MotorXSpeed = " + xyValue + ", MotorYSpeed = "
                            + xyValue + " Where Id = " + WorkCurveHelper.DeviceCurrent.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            }
            else if (this._moterType == MoterType.ZMotor && WorkCurveHelper.DeviceCurrent.HasMotorZ)
            {
                WorkCurveHelper.DeviceCurrent.MotorZSpeed = xyValue;
                string sql = "Update Device Set MotorZSpeed = " + xyValue + " Where Id = " + WorkCurveHelper.DeviceCurrent.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            }
        }

        public static UCXYZMotor xMotor;
        public static UCXYZMotor yMotor;
        public static UCXYZMotor zMotor;

        public static void CreateSingleInstance(MoterType type)
        {
            switch (type)
            {
                case MoterType.XMotor:
                    if (xMotor == null)
                        xMotor = new UCXYZMotor();
                    break;
                case MoterType.YMotor:
                    if (yMotor == null)
                        yMotor = new UCXYZMotor();
                    break;
                case MoterType.ZMotor:
                    if (zMotor == null)
                        zMotor = new UCXYZMotor();
                    break;
            }
        }

        static int height = 0;
        public override void OpenUC(bool flag, string TitleName, bool isModel, bool noneStyle)
        {
            if (this.IsSignlObject) return;
            Form form = new Form();
            form.BackColor = Color.White;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.ShowInTaskbar = false;
            int padSpace = 0;
            form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
            form.FormClosing += (s, ex) =>
            {
                this.IsSignlObject = false;
                switch (this.MoterType)
                {
                    case MoterType.XMotor:
                        xMotor = null;
                        break;
                    case MoterType.YMotor:
                        yMotor = null;
                        break;
                    case MoterType.ZMotor:
                        zMotor = null;
                        break;
                }
            };
            form.Controls.Add(this);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.ClientSize = new Size(this.Width + padSpace * 2, this.Height + padSpace * 2);
            form.ShowIcon = false;
            form.TopMost = true;
            this.Dock = DockStyle.Fill;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, height);
            if (xMotor != null || yMotor != null || zMotor != null)
                height += this.Height+20;
            if (height > Screen.PrimaryScreen.WorkingArea.Height-this.Height)
                height = Screen.PrimaryScreen.WorkingArea.Height-this.Height;
            form.Show();
            this.IsSignlObject = true;
        }

        private void UCXYZMotor_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void btnAxisLeft_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                   && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null)
            {
                switch (MoterType)
                {
                    case MoterType.XMotor:
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect, this.MotorStep, true, 250-WorkCurveHelper.DeviceCurrent.MotorXSpeed);
                        break;
                    case MoterType.YMotor:
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect, this.MotorStep, true, 250-WorkCurveHelper.DeviceCurrent.MotorYSpeed);
                        break;
                    case MoterType.ZMotor:
                       // Msg.Show(DifferenceDevice.interClassMain.deviceMeasure.interfacce.Pump.Exist.ToString());
                       // Msg.Show(DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnVacuum.ToString() + ";" + WorkCurveHelper.VacummNo.ToString());

                        if (WorkCurveHelper.SetCoverOpen == 1)
                        {
                            if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnVacuum < WorkCurveHelper.VacummNo)
                            {
                                Msg.Show(Info.VacummLess);
                            }
                            else
                            {
                                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, this.MotorStep, true, 250 - WorkCurveHelper.DeviceCurrent.MotorZSpeed);
                            }
                           

                        }
                        else
                        {
                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, this.MotorStep, true, 250 - WorkCurveHelper.DeviceCurrent.MotorZSpeed);

                        }
                        break;

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                 && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null)
            {
                switch (MoterType)
                {
                    case MoterType.XMotor:
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect, 0, true, 250-WorkCurveHelper.DeviceCurrent.MotorXSpeed);
                        break;
                    case MoterType.YMotor:
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect,0, true, 250-WorkCurveHelper.DeviceCurrent.MotorYSpeed);
                        break;
                    case MoterType.ZMotor:
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, 0, true, 250-WorkCurveHelper.DeviceCurrent.MotorZSpeed);
                        break;

                }
            }
        }

        private void btnAxisRight_Click(object sender, EventArgs e)
        {

        }
    }
}
