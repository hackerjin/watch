using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDX.Common.Component;
using System.Threading;
using Skyray.EDXRFLibrary;
using AForge.Controls;


namespace Skyray.UC
{
    /// <summary>
    /// 摄像头移动调整类
    /// </summary>
    public partial class UCThickCameraControl :Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 定义多点事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralMultiPoint;

        /// <summary>
        /// 定义网格事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralNetWork;

        /// <summary>
        /// 定义固定步长事件
        /// </summary>
        //public event Skyray.UC.EventDelegate.ReturnCameralState OnFixedWalk;

        /// <summary>
        /// 定义单点事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralSinglePoint;

        /// <summary>
        /// 定义移动事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralMove;

        /// <summary>
        /// 定义校正事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralCheck;

        /// <summary>
        /// 定义开始事件
        /// </summary>
        public event Skyray.UC.EventDelegate.CameralOperation OnCameralStart;

        /// <summary>
        /// 定义停止事件
        /// </summary>
        public event Skyray.UC.EventDelegate.CameralOperation OnCameralStop;

        /// <summary>
        /// 定义复位事件
        /// </summary>
        public event Skyray.UC.EventDelegate.CameralOperation OnCameralReset;

        /// <summary>
        /// 测量设备
        /// </summary>
        private MotorInstance motorHelper = new MotorInstance();

        private Device currentVice;

        //private int iScrollZSpeed = 251;


        public Device CurrentDecvice
        {
            get { return currentVice; }
            set
            {
                if (value != null)
                {
                    currentVice = value;
                    DisableControl(value,false);
                    WorkCurveHelper.DeviceCurrent = currentVice;
                    motorHelper.CreateInstance(MoterType.XMotor);
                    motorHelper.CreateInstance(MoterType.YMotor);
                    motorHelper.CreateInstance(MoterType.ZMotor);
                }
            }
        }

        public void DisableControl(Device device,bool bTest)
        {
            if (!device.HasMotorX && !device.HasMotorY)
                this.vScrollXYSpeed.Enabled = false;

            if (!device.HasMotorX || bTest)
            {
                this.btnXAxisLeft.Enabled = false;
                this.btnXAxisRight.Enabled = false;
            }
            else
            {
                this.btnXAxisLeft.Enabled = true;
                this.btnXAxisRight.Enabled = true;
                this.vScrollXYSpeed.Enabled = true;

                if (device.MotorXSpeed > this.vScrollXYSpeed.Maximum)
                    device.MotorXSpeed = this.vScrollXYSpeed.Maximum;

                if (device.MotorXSpeed < this.vScrollXYSpeed.Minimum)
                    device.MotorXSpeed = this.vScrollXYSpeed.Minimum;
                this.vScrollXYSpeed.Value = device.MotorXSpeed;
                //this.lblXYSpeed.Text = Convert.ToString(170 - device.MotorXSpeed);//(this.vScrollXYSpeed.Maximum - this.vScrollXYSpeed.Value + 1).ToString();
                //lblXYSpeedw1.Text = Convert.ToString(170 - device.MotorXSpeed);

                lblXYSpeed.Text = vScrollXYSpeed.Value.ToString();
                lblXYSpeedw1.Text = Convert.ToString(170 - vScrollXYSpeed.Value);
            }

            if (!device.HasMotorY || bTest)
            {
                this.btnYAxisIn.Enabled = false;
                this.btnYAxisOut.Enabled = false;
            }
            else
            {
                this.btnYAxisIn.Enabled = true;
                this.btnYAxisOut.Enabled = true;
                this.vScrollXYSpeed.Enabled = true;
            }

            if (!device.HasMotorZ || bTest)
            {
                this.btnZAxisOpen.Enabled = false;
                this.btnZAxisClose.Enabled = false;
                this.vScrollZSpeed.Enabled = false;
                this.btnAutoFocal.Enabled = false;
                if (device.HasMotorZ)
                    this.chkIsLazerOpen.Enabled = true;
                else this.chkIsLazerOpen.Enabled = false;
            }
            else
            {
                this.btnZAxisOpen.Enabled = true;
                this.btnZAxisClose.Enabled = true;
                this.vScrollZSpeed.Enabled = true;
                this.btnAutoFocal.Enabled = true;
                this.chkIsLazerOpen.Enabled = true;

                if (device.MotorZSpeed > this.vScrollZSpeed.Maximum)
                    device.MotorZSpeed = this.vScrollZSpeed.Maximum;

                if (device.MotorZSpeed < this.vScrollZSpeed.Minimum)
                    device.MotorZSpeed = this.vScrollZSpeed.Minimum;
                this.vScrollZSpeed.Value = device.MotorZSpeed;
                //this.lblZSpeed.Text = Convert.ToString(170 - device.MotorZSpeed);
                //lblZSpeed1.Text = Convert.ToString(170 - device.MotorZSpeed);
                lblZSpeed.Text = vScrollZSpeed.Value.ToString();
                lblZSpeed1.Text = Convert.ToString(170 - vScrollZSpeed.Value);
            }

            if (!this.chkIsLazerOpen.Checked)
                btnAutoFocal.Enabled = false;
            
            this.checkBoxWalk.Enabled = true;
            this.textBoxWInputWalk.Enabled = true;
           

            if (!device.HasMotorZ && !device.HasMotorY && !device.HasMotorX)
            {
               
                this.checkBoxWalk.Enabled = false;
                this.textBoxWInputWalk.Enabled = false;
            }
        }

        public void DisableControlNew(Device device)
        {
            if (!device.HasMotorX && !device.HasMotorY)
                this.vScrollXYSpeed.Enabled = false;

            if (!device.HasMotorX)
            {
                this.btnXAxisLeft.Enabled = false;
                this.btnXAxisRight.Enabled = false;
            }
            else
            {
                this.btnXAxisLeft.Enabled = true;
                this.btnXAxisRight.Enabled = true;
                this.vScrollXYSpeed.Enabled = true;
            }

            if (!device.HasMotorY)
            {
                this.btnYAxisIn.Enabled = false;
                this.btnYAxisOut.Enabled = false;
            }
            else
            {
                this.btnYAxisIn.Enabled = true;
                this.btnYAxisOut.Enabled = true;
                this.vScrollXYSpeed.Enabled = true;
            }

            if (!device.HasMotorZ)
            {
                this.btnZAxisOpen.Enabled = false;
                this.btnZAxisClose.Enabled = false;
                this.vScrollZSpeed.Enabled = false;
                if (this.chkIsLazerOpen.Enabled != false)// 防止btnAutoFocal不停的煽动
                    this.btnAutoFocal.Enabled = false;
                this.chkIsLazerOpen.Enabled = false;
            }
            else
            {
                this.btnZAxisOpen.Enabled = true;
                this.btnZAxisClose.Enabled = true;
                this.vScrollZSpeed.Enabled = true;
                if (this.chkIsLazerOpen.Checked && !this.btnAutoFocal.Enabled)
                    this.btnAutoFocal.Enabled = true;
                else if (!this.chkIsLazerOpen.Checked) this.btnAutoFocal.Enabled = false;
                //this.chkIsLazerOpen.Enabled = true;
            }

          
            this.checkBoxWalk.Enabled = true;
            this.textBoxWInputWalk.Enabled = true;
          
            if (!device.HasMotorZ && !device.HasMotorY && !device.HasMotorX)
            {
               
                this.checkBoxWalk.Enabled = false;
                this.textBoxWInputWalk.Enabled = false;
            }
        }

        public bool Flag
        {
            set
            {
                if (value)
                {
                    this.radioButtonCheck.Enabled = true;
                    this.radioButtonMany.Enabled = true;
                    this.radioButtonMove.Enabled = true;
                    this.radioButtonNetwork.Enabled = true;
                    this.radioButtonSingle.Enabled = true;
                    //if (this.radioButtonCheck.Checked || this.radioButtonMove.Checked || this.radioButtonSingle.Checked)
                    //    this.buttonWStart.Enabled = false;
                    //if (this.radioButtonMany.Checked || this.radioButtonNetwork.Checked)
                    //    this.buttonWStart.Enabled = true;
                }
                else
                {
                    this.radioButtonCheck.Enabled = false;
                    this.radioButtonMany.Enabled = false;
                    this.radioButtonMove.Enabled = false;
                    this.radioButtonNetwork.Enabled = false;
                    this.radioButtonSingle.Enabled = false;

                    //this.buttonWStart.Enabled = false;
                }
            }
        }

        private bool showMenu = false;

        public bool ShowMenu
        {
            get { return showMenu; }
            set 
            {
                //if (this.skyrayCamera1.Opened)
                    this.skyrayCamera1.ShowCameraMenu = true;
                showMenu = value; 
            }
        }

        public bool MoveAttribute
        {
            set { 
                    this.radioButtonMove.Checked = value;
            }
        }

        private SurfaceSourceLight surfaceSource;
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCThickCameraControl()
        {
            InitializeComponent();
            
            this.textBoxWInputWalk.Text = "8000";
            //CameraRefMotor.OnMotorNoMoveEventHandle += new CameraRefMotor.MotorNoMoveEventHandle(CameraRefMotor_OnMotorNoMoveEventHandle);
             this.skyrayCamera1.DoNextTest = StartNextTest;
            if (DifferenceDevice.IsThick && GP.CurrentUser.Role.RoleType == 0)
            {
                chkresetPoint.Visible = true;
                labelx.Visible = true;
                labely.Visible = true;
                buttonShowPosition.Visible = true;
            }
            else
            {
                chkresetPoint.Visible = false;
                chkresetPoint.Checked = false;
            }
            hScrollStepSpeed.Value = Convert.ToInt32(textBoxWInputWalk.Text);

           
        }

        void CameraRefMotor_OnMotorNoMoveEventHandle()
        {
            Msg.Show(Info.SubmitMotorInformation);
        }

        /// <summary>
        /// 规定步长
        /// </summary>
        public int FixedStep
        {
            get
            {
                try
                {
                    int fixedStep = int.Parse(textBoxWInputWalk.Text);
                    if (fixedStep > CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP)
                        fixedStep = CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP;
                    return fixedStep;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// X Y轴速度
        /// </summary>
        public int XYSpeed
        {
            get
            {
                if (int.Parse(lblXYSpeed.Text) > CameraRefMotor.MIN_XYZ_AXES_MOTOR_SPEED_GEAR)
                {
                    return CameraRefMotor.MIN_XYZ_AXES_MOTOR_SPEED_GEAR;
                }
                else if (int.Parse(lblXYSpeed.Text) < CameraRefMotor.MAX_XY_AXES_MOTOR_SPEED_GEAR)
                {
                    return CameraRefMotor.MAX_XY_AXES_MOTOR_SPEED_GEAR;
                }
                else
                {
                    return int.Parse(lblXYSpeed.Text);
                }
            }
        }

        // 升降平台速度
        public int ZSpeed
        {
            get
            {
                if (int.Parse(lblZSpeed.Text) > CameraRefMotor.MIN_XYZ_AXES_MOTOR_SPEED_GEAR)
                {
                    return CameraRefMotor.MIN_XYZ_AXES_MOTOR_SPEED_GEAR;
                }
                else if (int.Parse(lblZSpeed.Text) < CameraRefMotor.MAX_Z_AXES_MOTOR_SPEED_GEAR)
                {
                    return CameraRefMotor.MAX_Z_AXES_MOTOR_SPEED_GEAR;
                }
                else
                {
                    return int.Parse(lblZSpeed.Text);
                    //return iScrollZSpeed - int.Parse(lblZSpeed.Text);
                }
            }
        }


        /// <summary>
        /// 开始连测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWStart_Click(object sender, EventArgs e)
        {
            //CheckDog();
            if (OnCameralStart != null)
            {
                
                DifferenceDevice.interClassMain.bIsCameraStartTest = true;
                OnCameralStart();
                DifferenceDevice.IsConnect = true;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWStop_Click(object sender, EventArgs e)
        {
            if (OnCameralStop != null)
            {
                OnCameralStop();
                DifferenceDevice.IsConnect = false;
            }
        }

        /// <summary>
        /// 复位操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWReset_Click(object sender, EventArgs e)
        {
            if (chkresetPoint.Checked) //设置复位点
            {
                currentVice.MotorResetX = WorkCurveHelper.positionP.X;
                currentVice.MotorResetY = WorkCurveHelper.positionP.Y;
                string sql = "Update Device Set MotorResetX = " + currentVice.MotorResetX + ", MotorResetY = " + currentVice.MotorResetY + " Where Id = " + currentVice.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            }
            else
            {
                MotorReset();
               // MotorAdvance.FocusPointXY(
               //new MotorAdvance.FocusParams(
               //    (MotorDirections)Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect - 1), currentVice.MotorResetX, 250 - WorkCurveHelper.DeviceCurrent.MotorXSpeed, (MotorDirections)Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYCode - 1), currentVice.MotorResetY, 250 - WorkCurveHelper.DeviceCurrent.MotorYSpeed));
        
            }
         }


        public void MotorReset()
        {

            WorkCurveHelper.positionP.X = currentVice.MotorResetX;
            WorkCurveHelper.positionP.Y = currentVice.MotorResetY;
            if (OnCameralReset != null)
                OnCameralReset();
           
        }
        /// <summary>
        /// 单击移动按钮变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonMove_CheckedChanged(object sender, EventArgs e)
        {
            //if (radioButtonMove.Checked)
            //    buttonWStart.Enabled = false;
            if (OnReturnCameralMove != null)
                OnReturnCameralMove(radioButtonMove.Checked);
        }

        /// <summary>
        /// 单击校正按钮变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonCheck_CheckedChanged(object sender, EventArgs e)
        {
            //if (radioButtonCheck.Checked)
            //    buttonWStart.Enabled = false;
            if (OnReturnCameralCheck != null)
                OnReturnCameralCheck(radioButtonCheck.Checked);
        }

        /// <summary>
        /// 单击单点按钮变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonSingle_CheckedChanged(object sender, EventArgs e)
        {
            //if (radioButtonSingle.Checked)
            //    buttonWStart.Enabled = false;
            if (OnReturnCameralSinglePoint != null)
                OnReturnCameralSinglePoint(radioButtonSingle.Checked);
        }

        /// <summary>
        /// 单击多点按钮变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonMany_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMany.Checked)
                buttonWStart.Enabled = true;
            if (OnReturnCameralMultiPoint != null)
                OnReturnCameralMultiPoint(radioButtonMany.Checked);
        }

        /// <summary>
        /// 单击网格按钮变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonNetwork_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNetwork.Checked)
                buttonWStart.Enabled = true;
            if (OnReturnCameralNetWork != null)
                OnReturnCameralNetWork(radioButtonNetwork.Checked);
        }

        private void UCCameraControl_Load(object sender, EventArgs e)
        {
           
            if (DifferenceDevice.IsThick)
            {
                if (WorkCurveHelper.AxisZSpeedMode == 1)
                {
                    panel1.Visible = false;
                    panel2.Visible = true;
                }
                else
                {
                    panel1.Visible = true;
                    panel2.Visible = false;
                }
            }
            RBSlow.Checked = true;
            //int ZSpeedMax  = int.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("XYZMotor", "ZSpeedMax"));
            //int ZSpeedMin = int.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("XYZMotor", "ZSpeedMin"));
            //ZSpeedMax += ZSpeedMin;
            //iScrollZSpeed = ZSpeedMax;
            //vScrollZSpeed.Minimum = ZSpeedMin;
            //vScrollZSpeed.Maximum = ZSpeedMax-1;
            surfaceSource = SurfaceSourceLight.FindAll()[0];
            if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA && surfaceSource != null)
            {
                vWhiteScrollBar.Value = surfaceSource.FirstLight;
                lblWhiteValue.Text = vWhiteScrollBar.Value.ToString();
                // DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.SetSurfaceSource(sour.FirstLight, sour.SecondLight, sour.ThirdLight, sour.FourthLight);
            }
            else
            {
                vWhiteScrollBar.Value = 0;
                lblWhiteValue.Text = vWhiteScrollBar.Value.ToString();
            }
        }

        void UCCameraControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.skyrayCamera1.Close();
        }

        private void btnXAxisLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int value = XYSpeed;
                 if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
            && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorX)
                {
                    if (DifferenceDevice.IsThick)
                    {
                        WorkCurveHelper.positionP.X = WorkCurveHelper.DeviceCurrent.MotorXDirect == 0 ? WorkCurveHelper.positionP.X + FixedStep : WorkCurveHelper.positionP.X - FixedStep;
                        WorkCurveHelper.positionP.X = WorkCurveHelper.positionP.X < 0 ? 0 : WorkCurveHelper.positionP.X;
                        Console.WriteLine("stepx={0},stepy={1}", WorkCurveHelper.positionP.X.ToString(), WorkCurveHelper.positionP.Y.ToString());

                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect, FixedStep, true, value);

                    }
                    else
                    {
                        if (checkBoxWalk.Checked)
                        {
                            WorkCurveHelper.positionP.X = WorkCurveHelper.DeviceCurrent.MotorXDirect == 0 ? WorkCurveHelper.positionP.X + FixedStep : WorkCurveHelper.positionP.X - FixedStep;
                            WorkCurveHelper.positionP.X = WorkCurveHelper.positionP.X < 0 ? 0 : WorkCurveHelper.positionP.X;
                            Console.WriteLine("stepx={0},stepy={1}", WorkCurveHelper.positionP.X.ToString(), WorkCurveHelper.positionP.Y.ToString());

                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect, FixedStep, true, value);

                        }
                        else
                        {
                            //DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect, 9000000, true, value);
                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect, CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, true, value);
                        }
                    }
                }
            }
        }

        private void btnXAxisLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if(!DifferenceDevice.IsThick)
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left)
            {
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorX)
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect, 0, true, WorkCurveHelper.DeviceCurrent.MotorXSpeed);
            }
        }

        private void btnXAxisRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (!DifferenceDevice.IsThick)
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left)
            {
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorX)
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect-1), 0, true, WorkCurveHelper.DeviceCurrent.MotorXSpeed);
            }
        }

        private void btnXAxisRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int value = XYSpeed;
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
           && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorX)
                {
                    if (DifferenceDevice.IsThick)
                    {
                        WorkCurveHelper.positionP.X = Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect - 1) == 0 ? WorkCurveHelper.positionP.X + FixedStep : WorkCurveHelper.positionP.X - FixedStep;
                        WorkCurveHelper.positionP.X = WorkCurveHelper.positionP.X < 0 ? 0 : WorkCurveHelper.positionP.X;
                        Console.WriteLine("stepx={0},stepy={1}", WorkCurveHelper.positionP.X.ToString(), WorkCurveHelper.positionP.Y.ToString());
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect - 1), FixedStep, true, value);
                        
                    }
                    else
                    {
                        if (checkBoxWalk.Checked)
                        {
                            WorkCurveHelper.positionP.X = Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect - 1) == 0 ? WorkCurveHelper.positionP.X + FixedStep : WorkCurveHelper.positionP.X - FixedStep;
                            WorkCurveHelper.positionP.X = WorkCurveHelper.positionP.X < 0 ? 0 : WorkCurveHelper.positionP.X;
                            Console.WriteLine("stepx={0},stepy={1}", WorkCurveHelper.positionP.X.ToString(), WorkCurveHelper.positionP.Y.ToString());
                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect - 1), FixedStep, true, value);
                        }
                        else
                        {
                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect - 1), CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, true, value);
                        }
                    }
                }
            }
        }

        private void btnYAxisIn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                int value = XYSpeed;
                 if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
            && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorY)
                {
                    if (DifferenceDevice.IsThick)
                    {
                        WorkCurveHelper.positionP.Y = WorkCurveHelper.DeviceCurrent.MotorYDirect == 0 ? WorkCurveHelper.positionP.Y + FixedStep : WorkCurveHelper.positionP.Y - FixedStep;
                        WorkCurveHelper.positionP.Y = WorkCurveHelper.positionP.Y < 0 ? 0 : WorkCurveHelper.positionP.Y;
                        Console.WriteLine("stepx={0},stepy={1}", WorkCurveHelper.positionP.X.ToString(), WorkCurveHelper.positionP.Y.ToString());

                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect, FixedStep, true, value);
                      
                    }
                    else
                    {
                        if (checkBoxWalk.Checked)
                        {
                            // WorkCurveHelper.positionP.Y = WorkCurveHelper.positionP.Y - FixedStep > 0 ? WorkCurveHelper.positionP.Y - FixedStep : 0;
                            WorkCurveHelper.positionP.Y = WorkCurveHelper.DeviceCurrent.MotorYDirect == 0 ? WorkCurveHelper.positionP.Y + FixedStep : WorkCurveHelper.positionP.Y - FixedStep;
                            WorkCurveHelper.positionP.Y = WorkCurveHelper.positionP.Y < 0 ? 0 : WorkCurveHelper.positionP.Y;
                            Console.WriteLine("stepx={0},stepy={1}", WorkCurveHelper.positionP.X.ToString(), WorkCurveHelper.positionP.Y.ToString());

                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect, FixedStep, true, value);
                        }
                        else
                        {
                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect, CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, true, value);
                        }
                    }
                }
            }
        }

        private void btnYAxisIn_MouseUp(object sender, MouseEventArgs e)
        {
            if (!DifferenceDevice.IsThick)
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left)
            {
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorY)
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect, 0, true, WorkCurveHelper.DeviceCurrent.MotorYSpeed);
            }
        }

        private void btnYAxisOut_MouseUp(object sender, MouseEventArgs e)
        {
            if (!DifferenceDevice.IsThick)
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left)
            {
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
               && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorY)
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect - 1), 0, true, WorkCurveHelper.DeviceCurrent.MotorYSpeed);
            }
        }

        private void btnYAxisOut_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int value = XYSpeed;
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
            && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorY)
                {
                    if (DifferenceDevice.IsThick)
                    {
                        WorkCurveHelper.positionP.Y = Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect - 1) == 0 ? WorkCurveHelper.positionP.Y + FixedStep : WorkCurveHelper.positionP.Y - FixedStep;
                        WorkCurveHelper.positionP.Y = WorkCurveHelper.positionP.Y < 0 ? 0 : WorkCurveHelper.positionP.Y;
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect - 1), FixedStep, true, value);
                        Console.WriteLine("stepx={0},stepy={1}", WorkCurveHelper.positionP.X.ToString(), WorkCurveHelper.positionP.Y.ToString());
                      
                    }
                    else
                    {
                        if (checkBoxWalk.Checked)
                        {
                            WorkCurveHelper.positionP.Y = Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect - 1) == 0 ? WorkCurveHelper.positionP.Y + FixedStep : WorkCurveHelper.positionP.Y - FixedStep;
                            WorkCurveHelper.positionP.Y = WorkCurveHelper.positionP.Y < 0 ? 0 : WorkCurveHelper.positionP.Y;
                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect - 1), FixedStep, true, value);
                            Console.WriteLine("stepx={0},stepy={1}", WorkCurveHelper.positionP.X.ToString(), WorkCurveHelper.positionP.Y.ToString());
                        }
                        else
                        {
                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect - 1), CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, true, value);
                        }
                    }
                }
            }
        }

        private void btnZAxisOpen_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left)
            {
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, 0, true, WorkCurveHelper.DeviceCurrent.MotorZSpeed);
            }
        }

        private void btnZAxisOpen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int value = ZSpeed;
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
               && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                {
                    if (checkBoxWalk.Checked)
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, FixedStep, true, value);
                    else
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, true, value);

                }
            }
        }

        private void btnZAxisClose_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left)
            {
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1), 0, true, WorkCurveHelper.DeviceCurrent.MotorZSpeed);
            }
        }

        private void btnZAxisClose_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentVice.HasMotorZ)
            {
                int value = (ZSpeed == this.vScrollZSpeed.Minimum)?(ZSpeed+1):ZSpeed;
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
              && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                {
                    if (checkBoxWalk.Checked)
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1), FixedStep, true, value);
                    else
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1), CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, true, value);

                }
            }
        }

        private void btnAutoFocal_Click(object sender, EventArgs e)
        {
            MotorAdvance.AutoTuneHeightSync(ZSpeed);
        }

        private void chkIsLazerOpen_Click(object sender, EventArgs e)
        {
            if (currentVice.HasMotorZ)
            {
                if (chkIsLazerOpen.Checked)
                {
                    if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.connect != DeviceConnect.Connect
                        || DifferenceDevice.interClassMain.deviceMeasure.interfacce.StopFlag)
                        btnAutoFocal.Enabled = true;
                    MotorInstance.s_singleInstanceZ.OpenHeightLazer();
                }
                else
                {
                    btnAutoFocal.Enabled = false;
                    MotorInstance.s_singleInstanceZ.CloseHeightLazer();
                }
            }
        }

        private void vScrollXYSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (lblXYSpeed.Created && (currentVice.HasMotorX || currentVice.HasMotorY))
            {
                lblXYSpeed.Text = vScrollXYSpeed.Value.ToString();
                lblXYSpeedw1.Text = Convert.ToString(170 - vScrollXYSpeed.Value);
                currentVice.MotorXSpeed = XYSpeed;
                currentVice.MotorYSpeed = XYSpeed;
                string sql = "Update Device Set MotorXSpeed = " + XYSpeed + ", MotorYSpeed = "
                            + XYSpeed + " Where Id = " + currentVice.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            }
        }

        private void vScrollZSpeed_ValueChanged(object sender, EventArgs e)
        {
            //if (lblZSpeed.Created && currentVice.HasMotorZ && !DifferenceDevice.Refresh)
            if (lblZSpeed.Created && currentVice.HasMotorZ )
            {
                lblZSpeed.Text = vScrollZSpeed.Value.ToString();
                lblZSpeed1.Text = Convert.ToString(170 - vScrollZSpeed.Value);

                currentVice.MotorZSpeed = ZSpeed;
                string sql = "Update Device Set MotorZSpeed = " + ZSpeed + " Where Id = " + currentVice.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            //skyrayCamera1.SetTestInformation("", false);
        }

        public void SetTestInformation(string s,bool IsBtnShow)
        {
            skyrayCamera1.SetTestInformation(s, IsBtnShow);
        }
        //手动测试的开始下一次测试，更新摄像头信息
        public void StartNextTest()
        {
            DifferenceDevice.interClassMain.StartNextTest();
            string msg = string.Empty;
            if (Info.TestInfoMsg1.Contains("{0}th"))
            {
                switch (DifferenceDevice.interClassMain.currentTestTimes % 10)
                {
                    case 1:
                        msg = Info.TestInfoMsg1;
                        msg = msg.Replace("{0}th", DifferenceDevice.interClassMain.currentTestTimes + "st");
                        msg = msg.Replace("{1}", DifferenceDevice.interClassMain.testDevicePassedParams.MeasureParams.MeasureNumber.ToString());
                        break;
                    case 2:
                        msg = Info.TestInfoMsg1;
                        msg = msg.Replace("{0}th", DifferenceDevice.interClassMain.currentTestTimes + "nd");
                        msg = msg.Replace("{1}", DifferenceDevice.interClassMain.testDevicePassedParams.MeasureParams.MeasureNumber.ToString());
                        break;
                    case 3:
                        msg = Info.TestInfoMsg1;
                        msg = msg.Replace("{0}th", DifferenceDevice.interClassMain.currentTestTimes + "rd");
                        msg = msg.Replace("{1}", DifferenceDevice.interClassMain.testDevicePassedParams.MeasureParams.MeasureNumber.ToString());
                        break;
                    default:
                        msg = string.Format(Info.TestInfoMsg1, DifferenceDevice.interClassMain.currentTestTimes.ToString(),
                                        DifferenceDevice.interClassMain.testDevicePassedParams.MeasureParams.MeasureNumber.ToString());
                        break;

                }
            }
            else msg = string.Format(Info.TestInfoMsg1, DifferenceDevice.interClassMain.currentTestTimes.ToString(),
                                       DifferenceDevice.interClassMain.testDevicePassedParams.MeasureParams.MeasureNumber.ToString());
            SetTestInformation(msg, false);
        }

        void RBFast_CheckedChanged(object sender, System.EventArgs e)
        {
            if (RBFast.Checked&&RBFast.Visible)
            {
                lblZSpeed.Text = Math.Abs(170 - WorkCurveHelper.ZFast).ToString();
            }
        }

        private void RbMd_CheckedChanged(object sender, EventArgs e)
        {
            if (RBMd.Checked&&RBMd.Visible)
            {
                lblZSpeed.Text = Math.Abs(170 - WorkCurveHelper.ZMiddle).ToString();
            }
        }

        private void RBSlow_CheckedChanged(object sender, EventArgs e)
        {
            if (RBSlow.Checked&&RBSlow.Visible)
            {
                lblZSpeed.Text = Math.Abs(170 - WorkCurveHelper.ZSlow).ToString();
            }
        }

        private void skyrayCamera1_FormatChanged(object sender, EventArgs e)
        {
           }

        private void chkIsLazerOpen_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gbxZFocal_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            labelx.Text = WorkCurveHelper.positionP.X.ToString();
            labely.Text = WorkCurveHelper.positionP.Y.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void chkresetPoint_CheckedChanged(object sender, EventArgs e)
        {
            if (chkresetPoint.Checked)
            {
                buttonWReset.Text = Info.SetReset;
            }
            else
            {
                buttonWReset.Text =Info.Reset;
            }
        }

        private void btnZAxisClose_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void hScrollStepSpeed_ValueChanged(object sender, EventArgs e)
        {
            textBoxWInputWalk.Text = hScrollStepSpeed.Value.ToString();
        }

        private void vWhiteScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.lblWhiteValue.Text = vWhiteScrollBar.Value.ToString();
            surfaceSource.FirstLight = ushort.Parse(lblWhiteValue.Text);
            surfaceSource.SecondLight = ushort.Parse(lblWhiteValue.Text);
            surfaceSource.ThirdLight = ushort.Parse(lblWhiteValue.Text);
            surfaceSource.FourthLight = ushort.Parse(lblWhiteValue.Text);
            DifferenceDevice.MediumAccess.SetSurfaceSource(surfaceSource.FirstLight, surfaceSource.SecondLight, surfaceSource.ThirdLight, surfaceSource.FourthLight);
       
        }

        private void btnOpenCamer2_Click(object sender, EventArgs e)
        {
            FrmShowNewCamer f = new FrmShowNewCamer();
            f.videoSource = skyrayCamera1.VideoSource;
            f.TopMost = true;
            FormStartScreen(2, f);
           
        }

        private void FormStartScreen(int screen, Form form)
        {
            if (Screen.AllScreens.Length < screen)
            {
                MessageBox.Show("当前主机连接最多的屏幕是" + Screen.AllScreens.Length + " 个，不能投屏到第" + screen + "个 屏幕！");
                return;
            }
            screen = screen - 1;
            if (form == null)
            {
                form = new Form();
            }
            form.StartPosition = FormStartPosition.CenterScreen;
            Screen s = Screen.AllScreens[screen];
            form.Location = new System.Drawing.Point(s.Bounds.X, s.Bounds.Y);
            form.WindowState = FormWindowState.Maximized;
            form.Size = new Size(s.WorkingArea.Width, s.WorkingArea.Height);

            form.Show();
            form.BringToFront();
        }
      
        
    }
}
