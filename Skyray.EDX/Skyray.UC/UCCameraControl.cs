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
using AForge.Video.DirectShow;
using AForge.Controls;
using Skyray.EDX.Common.UIHelper;
using System.Xml;
using XRFNetLib;
using System.Reflection;
using System.IO;
using System.Collections;
namespace Skyray.UC
{
    /// <summary>
    /// 摄像头移动调整类
    /// </summary>
    public partial class UCCameraControl : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 定义多点事件
        /// </summary>
        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralMultiPoint;

        public event Skyray.UC.EventDelegate.ReturnCameralState OnReturnCameralMatrix;
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
        /// 定义出样事件
        /// </summary>
        public event Skyray.UC.EventDelegate.CameralOperation OnCameralOutSample;



        private readonly XRFNetworkClass netWork = new XRFNetworkClass();


        /// <summary>
        /// 测量设备
        /// </summary>
        private MotorInstance motorHelper = new MotorInstance();

        private Device currentVice;

        private bool visible = false;
        //private int iScrollZSpeed = 251;


        public bool isprogramMode = false;

        public Device CurrentDecvice
        {
            get { return currentVice; }
            set
            {
                if (value != null)
                {
                    currentVice = value;
                    DisableControl(value, false);
                    WorkCurveHelper.DeviceCurrent = currentVice;
                    motorHelper.CreateInstance(MoterType.XMotor);
                    motorHelper.CreateInstance(MoterType.YMotor);
                    motorHelper.CreateInstance(MoterType.ZMotor);
                }
            }
        }




        public int SetScorllValue
        {
            set
            {
                if (value != null)
                {
                    this.smallCameraLightBar.Value = value;
                }
            }
        }



        public void DisableControl(Device device, bool bTest)
        {
            containerCamer1.Width = skyrayCamera1.CapVideoWidth;

            

            if (!device.HasMotorX && !device.HasMotorY && !device.HasMotorZ)
            {
                btnSearchfocus.Visible = false;
                trackBarZMotor.Visible = false;
                gbxXYTable.Visible = false;
                gbxZFocal.Visible = false;
                grpdefine.Visible = false;
                labelx.Visible = false;
                labely.Visible = false;
                btnSearchfocus.Visible = false;
            }
            else
            {
                if (WorkCurveHelper.IsShowXYMove) //&&GP.CurrentUser.Role.RoleType == 0 
                {
                    gbxXYTable.Visible = true;
                    gbxZFocal.Visible = true;
                    
                }
                else
                {
                    gbxXYTable.Visible = false;
                    gbxZFocal.Visible = false;
                    buttonWStart.Location = new Point(2, 20);
                    buttonWStop.Location = new Point(2, 145);
                    grpdefine.Location = new Point(2, 263);
                }
                btnSearchfocus.Visible = true;
                trackBarZMotor.Visible = true;
                grpdefine.Visible = true;
                btnSearchfocus.Visible = true;
            }
            gbxXYTable.Visible = true;
            grpdefine.Visible = false;

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
                    this.chkLaser.Enabled = true;
                else this.chkLaser.Enabled = false;
            }
            else
            {
                this.btnZAxisOpen.Enabled = true;
                this.btnZAxisClose.Enabled = true;
                this.vScrollZSpeed.Enabled = true;
                this.btnAutoFocal.Enabled = true;
                this.chkLaser.Enabled = true;

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

            if (!this.chkLaser.Checked)
                btnAutoFocal.Enabled = false;
            this.buttonStop.Enabled = true;
            this.buttonReset.Enabled = true;
            this.chkFixedXY.Enabled = true;
            this.fixedXYValue.Enabled = true;
            this.buttonSubmit.Enabled = true;

            if (!device.HasMotorZ && !device.HasMotorY && !device.HasMotorX)
            {
                this.buttonStop.Enabled = false;
                // this.buttonSubmit.Enabled = false;
                this.buttonReset.Enabled = false;
                this.chkFixedXY.Enabled = false;
                this.fixedXYValue.Enabled = false;
            }
          
            btnSearchfocus.Visible = false;
            trackBarZMotor.Visible = false;
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
                if (this.chkLaser.Enabled != false)// 防止btnAutoFocal不停的煽动
                    this.btnAutoFocal.Enabled = false;
                this.chkLaser.Enabled = false;
            }
            else
            {
                this.btnZAxisOpen.Enabled = true;
                this.btnZAxisClose.Enabled = true;
                this.vScrollZSpeed.Enabled = true;
                if (this.chkLaser.Checked && !this.btnAutoFocal.Enabled)
                    this.btnAutoFocal.Enabled = true;
                else if (!this.chkLaser.Checked) this.btnAutoFocal.Enabled = false;
                //this.chkIsLazerOpen.Enabled = true;
            }

            //if (!this.chkIsLazerOpen.Checked)
            //    btnAutoFocal.Enabled = false;
            this.buttonStop.Enabled = true;
            this.buttonReset.Enabled = true;
            //this.checkBoxWalk.Enabled = true;
            this.fixedXYValue.Enabled = true;
            this.buttonSubmit.Enabled = true;

            if (!device.HasMotorZ && !device.HasMotorY && !device.HasMotorX)
            {
                this.buttonStop.Enabled = false;
                //this.buttonSubmit.Enabled = false;
                this.buttonReset.Enabled = false;
                this.chkFixedXY.Enabled = false;
                this.fixedXYValue.Enabled = false;
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
                    this.radioButtonMatrix.Enabled = true;
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
                    this.radioButtonMatrix.Enabled = false;



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
            set
            {
                this.radioButtonMove.Checked = value;
            }
        }

        public System.Windows.Forms.GroupBox GbxEncoderMove
        {
            get
            {
                return this.gbxEncoderMove;
                
            }

            set
            {
                this.gbxEncoderMove = value;
            }

        }

        public Skyray.Controls.CheckBoxW ChkRetest
        {
            get
            {


                return this.chkRetest;
            }

            set
            {

                this.chkRetest = value;
            }
        }
        public Skyray.Controls.CheckBoxW ChkTestDemo
        {

            get
            {

                return this.chkTestDemo;
            }

            set
            {
                this.chkTestDemo = value;

            }
        }
        public Skyray.Controls.CheckBoxW ChkFixedXY
        {

            get
            {

                return this.chkFixedXY;
            }

            set
            {
                this.chkFixedXY = value;

            }
        }
        public Skyray.Controls.CheckBoxW ChkFixedZ
        {
            get
            {

                return this.chkFixedZ;
            }

            set
            {
                this.chkFixedZ = value;

            }
        }



        public Skyray.Controls.CheckBoxW ChkLazer
        {
            get
            {
                return this.chkLaser;
            }

            set
            {
                this.chkLaser = value;

            }


        }
        public Skyray.Controls.TextBoxW InputWalk
        {
            get
            {
                return this.fixedXYValue;
            }

            set
            {
                this.fixedXYValue = value;

            }

        }

        private SurfaceSourceLight surfaceSource;


        /// <summary>
        /// 构造函数
        /// </summary>
        public UCCameraControl()
        {
            InitializeComponent();

            
            btnSearchfocus.Parent = this.skyrayCamera1;
            trackBarZMotor.Parent = this.skyrayCamera1;
           

            DifferenceDevice.interClassMain.OnSateChanged += new EventHandler<BoolEventArgs>(interClassMain_OnSateChanged);
            timerM.Interval = 50;
            timerM.Tick += new EventHandler(timerM_Tick);//定时器事件
            this.skyrayCamera1.DoNextTest = StartNextTest;
            chkAutoInOutSample.Checked = WorkCurveHelper.bOpenOutSample;
            chkFixedXY.Checked = WorkCurveHelper.FixedXY;
            chkFixedZ.Checked = WorkCurveHelper.FixedZ;

            if (!WorkCurveHelper.zFixable)
            {
                this.chkFixedZ.Visible = false;
                this.chkFixedZ.Checked = false;
                this.fixedZValue.Visible = false;

            }
            
            if (DifferenceDevice.IsThick && GP.CurrentUser.Role.RoleType == 0)
            {
                chkresetPoint.Visible = false;
                chkresetPoint.Checked = false;
                buttonWReset.Visible = false;
                buttonShowPosition.Visible = false;
                radioButtonCheck.Visible = true;
            }
            else
            {
                chkresetPoint.Visible = false;
                chkresetPoint.Checked = false;
                buttonWReset.Visible = false;
                buttonShowPosition.Visible = false;
                radioButtonCheck.Visible = false;

            }
            this.fixedXYValue.Text = "50";
           
            timerCurrent = new System.Windows.Forms.Timer();
            timerCurrent.Tick += new EventHandler(timerCurrent_Tick);//定时器事件
            timerCurrent.Interval = 50;
            timerCurrent.Enabled = false;
            DifferenceDevice.CurCameraControl = this;
            WorkCurveHelper.camera = this.skyrayCamera1;
            WorkCurveHelper.cameraType = typeof(Skyray.Camera.SkyrayCamera);
            WorkCurveHelper.thickType = typeof(Thick);
            WorkCurveHelper.ucCamera = this;
            WorkCurveHelper.ucCameraType = typeof(UCCameraControl);

            initLargeView();

         
        }


        public VideoSourcePlayer largeViewCamera = null;

        private void initLargeView()
        {

            int smallCameraIndex = int.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml", "Camera/CaptureIndex")) - 1;

            int largeCameraIndex = int.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml", "Camera/LargeCameraCaptureIndex")) - 1;


            if (smallCameraIndex == largeCameraIndex)
            {
                Msg.Show("远景摄像头与近景摄像头冲突！");
                return;
            }


            if (largeCameraIndex == -1)
                return;

            this.largeViewCamera = new VideoSourcePlayer();

            this.largeViewCamera.Visible = false;

            
            
            int capVideoWidth = 640;
            int capVideoHeight = 480;

            int MaxResolution = 2000000;

            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            VideoCaptureDevice videoSourceDevice = null;

            try
            {
                if (videoDevices.Count > 0)
                {

                    videoSourceDevice = new VideoCaptureDevice(videoDevices[largeCameraIndex].MonikerString);//连接摄像头。

                    videoSourceDevice.SetCameraProcAmp(VideoProcAmpProperty.Gain, 0, VideoProcAmpFlags.Auto);

                    if (!videoDevices[largeCameraIndex].Name.Contains("Daheng"))
                    {
                        foreach (VideoCapabilities vcb in videoSourceDevice.VideoCapabilities)
                        {
                            if (capVideoWidth == vcb.FrameSize.Width && capVideoHeight == vcb.FrameSize.Height)
                            {
                                videoSourceDevice.VideoResolution = vcb;
                                break;
                            }
                        }
                        if (videoSourceDevice.VideoResolution == null)
                        {
                            foreach (VideoCapabilities vcb in videoSourceDevice.VideoCapabilities)
                            {
                                if (vcb.FrameSize.Width * vcb.FrameSize.Height <= MaxResolution)
                                {
                                    videoSourceDevice.VideoResolution = vcb;

                                    break;
                                }
                            }

                        }
                    }


                }
            }
            catch
            {
                this.largeViewCamera = null;
                return;
            }
            this.largeViewCamera.VideoSource = videoSourceDevice;
            this.largeViewCamera.Start();
        }


        public void programMode()
        {


            if (!this.isprogramMode)
            {


                MethodInfo myMethod = WorkCurveHelper.curFrmThickType.GetMethod("showHide");
                myMethod.Invoke(WorkCurveHelper.curFrmThick, new object[] { false });



                //gbxEncoderMove为pnl的子控件，但是又需要单独根据配置决定是否显示
                this.gbxEncoderMove.Visible = WorkCurveHelper.hasMotorEncoder;

                this.largeViewPictureBox.Visible = true;
                this.splitContainer1.SplitterDistance = 250;
                this.isprogramMode = true;


                this.skyrayCamera1.tsmiEditMultiPoint_Click(0, null);
                new System.Threading.Thread(new System.Threading.ThreadStart(updateLargeViewNow)).Start();

            }
            else
            {


                MethodInfo myMethod = WorkCurveHelper.curFrmThickType.GetMethod("showHide");
                myMethod.Invoke(WorkCurveHelper.curFrmThick, new object[] { true });


                //gbxEncoderMove为pnl的子控件，但是又需要单独根据配置决定是否显示
                this.gbxEncoderMove.Visible = WorkCurveHelper.hasMotorEncoder;

                this.largeViewPictureBox.Visible = false;
                this.splitContainer1.SplitterDistance = 390;
                this.isprogramMode = false;
                this.skyrayCamera1.tsmiEditMultiPoint_Click(null, null);


            }



        }


       

        void CameraRefMotor_OnMotorNoMoveEventHandle()
        {
            Msg.Show(Info.SubmitMotorInformation);
        }

       
        public float FixedXYValue
        {
            get
            {
                try
                {
                    float fixedXYValue = 0;
                    if (chkFixedXY.Checked)
                    {

                       fixedXYValue = float.Parse(this.fixedXYValue.Text);
                       if (!WorkCurveHelper.FixedDisXY)
                           fixedXYValue = fixedXYValue < 5 ? 5 : fixedXYValue;
                       return fixedXYValue;
                       
                    }
                    else
                        return 0;
                    
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }



        public float FixedZValue
        {
            get
            {
                try
                {
                    float fixedZValue = 0;
                    if (chkFixedZ.Checked)
                    {

                       fixedZValue = float.Parse(this.fixedZValue.Text);
                       if (!WorkCurveHelper.FixedDisZ)
                           fixedZValue = fixedZValue < 5 ? 5 : fixedZValue;
                       return fixedZValue;
                       
                    }
                    else
                        return 0;
                    
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
        public void buttonWStart_Click(object sender, EventArgs e)
        {

            //CheckDog();
            if (OnCameralStart != null)
            {
                if (this.radioButtonCheck.Checked || this.radioButtonMove.Checked || this.radioButtonSingle.Checked)
                {
                    DifferenceDevice.interClassMain.bIsCameraStartTest = false;
                }
                else
                    DifferenceDevice.interClassMain.bIsCameraStartTest = true;

                OnCameralStart();

            }
        }

       
        //收到modbus信号，启动测试
        public void buttonWStart_Modbus()
        {
            if (ModbusTcp.slave.DataStore.CoilDiscretes[2])
            {
                Msg.Show("上次测量数据尚未通过modbusTcp读取");
                WorkCurveHelper.dataStore.CoilDiscretes[1] = false;
                WorkCurveHelper.startDoing = 0;
                return;
            }

            //CheckDog();
            if (OnCameralStart != null)
            {
                if (this.radioButtonCheck.Checked || this.radioButtonMove.Checked || this.radioButtonSingle.Checked)
                {
                    DifferenceDevice.interClassMain.bIsCameraStartTest = false;
                }
                else
                    DifferenceDevice.interClassMain.bIsCameraStartTest = true;



                OnCameralStart();
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

        //收到modbus信号，中断测试
        public void buttonWStop_Modbus()
        {
            if (!ModbusTcp.slave.DataStore.CoilDiscretes[1])
            {
                Msg.Show("无进行中的modbusTcp开启的测试");
                WorkCurveHelper.stopDoing = 0;
                return;
            }
            if (WorkCurveHelper.stopDoing > 1)
            {
                Msg.Show("正在通过modbusTcp停止测试，不可重复停止测试");
                return;

            }

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

                WorkCurveHelper.bMotorRestart = false;
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
        public void radioButtonMany_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMany.Checked)
            {
                buttonWStart.Enabled = true;
            }
            else
            {


                chkEditMulti.Checked = true;
                btnEditMulit_Click(null, null);

                skyrayCamera1.IsShowCenter = false;
            }
            if (OnReturnCameralMultiPoint != null)
                OnReturnCameralMultiPoint(radioButtonMany.Checked);
        }

        public void radioButtonMatrix_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMatrix.Checked)
            {
                buttonWStart.Enabled = true;
                //多点时固定步长
                //  btnEditMulit.Visible = true;
            }
            else
            {


                chkEditMulti.Checked = true;
                btnEditMulit_Click(null, null);

                //   btnEditMulit.Visible = false;
                skyrayCamera1.IsShowCenter = false;
            }
            if (OnReturnCameralMatrix != null)
                OnReturnCameralMatrix(radioButtonMatrix.Checked);
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
            //vScrollZSpeed.Minimum = 0;
            //vScrollZSpeed.Maximum = 0;
            //if (this.skyrayCamera1.Opened)
            //this.mtSplitter1.SplitPosition =  this.skyrayCamera1.CapVideoWidth;
            //else
            //    this.mtSplitter1.SplitPosition =  this.skyrayCamera1.Width - 40;

            this.fixedXYValue.Text = "4";
            this.fixedZValue.Text = "4";

            if (DifferenceDevice.IsThick)
            {
                if (WorkCurveHelper.AxisZSpeedMode == 1)
                {
                    panelZ1.Visible = false;
                    panelZ2.Visible = true;
                }
                else
                {
                    panelZ1.Visible = true;
                    panelZ2.Visible = false;

                }
            }
            RBSlow.Checked = true;


            if (WorkCurveHelper.zSpeedMode)
            {
                this.rbZLow.Checked = false;
                this.rbZFast.Checked = true;
            }
            else
            {
                this.rbZLow.Checked = true;
                this.rbZFast.Checked = false;
            }
            //int ZSpeedMax  = int.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("XYZMotor", "ZSpeedMax"));
            //int ZSpeedMin = int.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("XYZMotor", "ZSpeedMin"));
            //ZSpeedMax += ZSpeedMin;
            //iScrollZSpeed = ZSpeedMax;
            //vScrollZSpeed.Minimum = ZSpeedMin;
            //vScrollZSpeed.Maximum = ZSpeedMax-1;
            surfaceSource = SurfaceSourceLight.FindAll()[0];
            if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA && surfaceSource != null)
            {
                smallCameraLightBar.Value = surfaceSource.FirstLight;
                lblSmallCameraLight.Text = smallCameraLightBar.Value.ToString();
                // DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.SetSurfaceSource(sour.FirstLight, sour.SecondLight, sour.ThirdLight, sour.FourthLight);
            }
            else
            {
                smallCameraLightBar.Value = 1000;
                lblSmallCameraLight.Text = smallCameraLightBar.Value.ToString();
            }
            

            WorkCurveHelper.testDemo = bool.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/testDemo"));
            if (WorkCurveHelper.testDemo)
                this.chkTestDemo.Checked = true;
            else
                this.chkTestDemo.Checked = false;

            this.skyrayCamera1.CapVideoWidth = 640;
            this.skyrayCamera1.CapVideoHeight = 480;
            this.skyrayCamera1.Open();
            if (this.skyrayCamera1.selectedDeviceIndex == 0)
                Thread.Sleep(2000);

            this.gbxEncoderMove.Visible = WorkCurveHelper.hasMotorEncoder;
            

            initXY();
            getLargeView();


        }



        //在更新远景图像之前，需要先更新当前远景图像在虚拟盘上的位置，因此需调用getXY，这里不将getXY在此函数中调用，因为此函数只有在Load以及进样时调用
        //且在Load时，还要对largeViewX/Y进行特殊处理
        public void getLargeView()
        {

            WorkCurveHelper.waitMoveStop();

            MotorOperator.getZ();

            if (this.largeViewCamera != null)
            {
                WorkCurveHelper.largeViewCatchHeight = WorkCurveHelper.Z;

                while (this.largeViewCamera.GetCurrentVideoFrame() == null) ;
                this.largeView = this.largeViewCamera.GetCurrentVideoFrame();

                // 创建新的Bitmap对象并指定目标大小
                Bitmap scaledImage = new Bitmap(imageWidth, imageHeight);

                using (Graphics graphics = Graphics.FromImage(scaledImage))
                {

                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        graphics.FillRectangle(brush, 0, 0, imageWidth, imageHeight);
                    }
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    float leftX = (WorkCurveHelper.largeViewX / WorkCurveHelper.XCoeff - WorkCurveHelper.largeViewTabWidth / 2.0f) / WorkCurveHelper.TabWidth * imageWidth;
                    float leftY = (WorkCurveHelper.largeViewY / WorkCurveHelper.YCoeff - WorkCurveHelper.largeViewTabHeight / 2.0f) / WorkCurveHelper.TabHeight * imageHeight;

                    graphics.DrawImage((Image)this.largeView.Clone(), leftX, leftY, targetWidth, targetHeight);


                }
                
                this.largeViewPictureBox.Image = scaledImage;
            }
            else
            {

                float colGap = cellSize / WorkCurveHelper.TabWidth * imageWidth;
                float rowGap = cellSize / WorkCurveHelper.TabHeight * imageHeight;


                // 创建新的Bitmap对象并指定目标大小
                Bitmap scaledImage = new Bitmap(imageWidth, imageHeight);


                using (Graphics graphics = Graphics.FromImage(scaledImage))
                {

                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        graphics.FillRectangle(brush, 0, 0, imageWidth, imageHeight);
                    }

                    using (Pen gridPen = new Pen(Color.Black, 1))
                    {
                        // 绘制水平网格线
                        for (int row = 1; row < Math.Ceiling(WorkCurveHelper.TabHeight / cellSize); row++)
                        {
                            graphics.DrawLine(gridPen, 0, row * rowGap, imageWidth, row * rowGap);
                        }

                        // 绘制垂直网格线
                        for (int col = 1; col < Math.Ceiling(WorkCurveHelper.TabWidth / cellSize); col++)
                        {
                            graphics.DrawLine(gridPen, col * colGap, 0, col * colGap, imageHeight);
                        }


                    }


                }
                this.largeViewPictureBox.Image = scaledImage;

            }

        }


        //在Load时特殊处理largeViewX/Y
        private void initXY()
        {
            MotorOperator.getXY();

            if (this.largeViewCamera != null)
            {
                WorkCurveHelper.largeViewX = WorkCurveHelper.X;
                WorkCurveHelper.largeViewY = WorkCurveHelper.Y + WorkCurveHelper.TwoCameraDis * WorkCurveHelper.YCoeff;
            }
        }

    

        private Image largeView = null;

        private static int imageWidth
        {
            get
            {
               if (WorkCurveHelper.DeviceCurrent != null)
               {
                   if (WorkCurveHelper.DeviceCurrent.HasMotorSpin || WorkCurveHelper.DeviceCurrent.MotorYMaxStep < 300)
                       return 600;
                   else
                       return 600; //复位点与最外点的距离
               }
               else
                   return 0;


            }
        }


        private static int imageHeight
        {

            get
            {
                if (WorkCurveHelper.DeviceCurrent != null)
                {
                    if (WorkCurveHelper.DeviceCurrent.HasMotorSpin || WorkCurveHelper.DeviceCurrent.MotorYMaxStep < 300)
                        return 600;
                    else
                        return 800; //复位点与最外点的距离
                }
                else
                    return 0;

            }

        }
        private static float cellSize = 25;
      
        private static float targetWidth 
        {
            get
            {

                return WorkCurveHelper.largeViewTabWidth / WorkCurveHelper.TabWidth * imageWidth;
            }

        }

        private static float targetHeight
        {
            get
            {
                return WorkCurveHelper.largeViewTabHeight / WorkCurveHelper.TabHeight * imageHeight;
            }
        }

        private Mutex largeViewMutex = new Mutex();


        //此函数在发生运动之后更新全景图像上的当前位置，因此通常在调用运动函数之后紧接着调用此函数
        //但是，此处不将此函数在运动函数之中调用，否则关联性太强
        public void updateLargeViewNow()
        {


            WorkCurveHelper.waitMoveStop();

            if (this.largeViewCamera != null && this.largeView != null)
            {

                float curPixelX = (WorkCurveHelper.X / WorkCurveHelper.XCoeff / WorkCurveHelper.TabWidth * imageWidth);
                float curPixelY = (WorkCurveHelper.Y / WorkCurveHelper.YCoeff / WorkCurveHelper.TabHeight * imageHeight);

                //当前点到平台可移动范围的距离
                float curDisLeft = WorkCurveHelper.TabWidth - (WorkCurveHelper.X / WorkCurveHelper.XCoeff);
                float curDisTop = WorkCurveHelper.TabHeight - (WorkCurveHelper.Y / WorkCurveHelper.YCoeff);
                float curDisRight = WorkCurveHelper.X / WorkCurveHelper.XCoeff;
                float curDisBot = WorkCurveHelper.Y / WorkCurveHelper.YCoeff;


                //此处添加互斥锁，防止多个更新线程同时访问largeView
                this.largeViewMutex.WaitOne();

                // 创建新的Bitmap对象并指定目标大小
                Bitmap scaledImage = new Bitmap(imageWidth, imageHeight);

                using (Graphics graphics = Graphics.FromImage(scaledImage))
                {

                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        graphics.FillRectangle(brush, 0, 0, imageWidth, imageHeight);
                    }

                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    float leftX = (WorkCurveHelper.largeViewX / WorkCurveHelper.XCoeff - WorkCurveHelper.largeViewTabWidth / 2.0f) / WorkCurveHelper.TabWidth * imageWidth;
                    float leftY = (WorkCurveHelper.largeViewY / WorkCurveHelper.YCoeff - WorkCurveHelper.largeViewTabHeight / 2.0f) / WorkCurveHelper.TabHeight * imageHeight;

                    graphics.DrawImage((Image)this.largeView.Clone(), leftX, leftY, targetWidth, targetHeight);

                    // 设置绘制质量
                    System.Drawing.Pen pen = new System.Drawing.Pen(Color.Red, 2);

                    graphics.DrawRectangle(pen, curPixelX - 10, curPixelY - 8, 20, 16);

                    using (SolidBrush brush = new SolidBrush(Color.Red))
                    {
                        int center = 80;
                        int size = 50;
                        int disToCenter = 15;
                        Font font = new Font("TabDis", 10, FontStyle.Regular);

                        Rectangle rect = new Rectangle(center - (int)(size / 2.0), center - disToCenter, size, size);

                        graphics.DrawString(curDisTop.ToString("f2"), font, brush, rect);

                        rect = new Rectangle(center - disToCenter - size, center - 40, size, size);

                        graphics.DrawString("高度", font, brush, rect);

                        rect = new Rectangle(center - (int)(size / 2.0), center - disToCenter - 30, size, size);

                        graphics.DrawString((WorkCurveHelper.largeViewCatchHeight / WorkCurveHelper.ZCoeff).ToString("f2"), font, brush, rect);

                        rect = new Rectangle(center - disToCenter - size, center, size, size);
                        graphics.DrawString(curDisLeft.ToString("f2"), font, brush, rect);


                        rect = new Rectangle(center + disToCenter, center, size, size);
                        graphics.DrawString(curDisRight.ToString("f2"), font, brush, rect);


                        rect = new Rectangle(center - (int)(size / 2.0), center + disToCenter, size, size);
                        graphics.DrawString(curDisBot.ToString("f2"), font, brush, rect);


                    }



                    System.Drawing.Pen pointsPen = new System.Drawing.Pen(Color.Blue, 2);

                    int index = this.skyrayCamera1.alTempTestPoints.Count - this.skyrayCamera1.alContiTestPoints.Count;

                    float X, Y;

                    int crossSize = 10; // 十字架的大小
                    Pen crossPen = new Pen(Color.Blue, 4);
                    for (int i = 0; i < this.skyrayCamera1.alContiTestPoints.Count; i++)
                    {

                        X = ((Point)this.skyrayCamera1.alTempTestPoints[index + i]).X;
                        Y = ((Point)this.skyrayCamera1.alTempTestPoints[index + i]).Y;

                        float locX = 0;
                        float locY = 0;

                        locX = X / WorkCurveHelper.XCoeff / WorkCurveHelper.TabWidth * imageWidth;

                        locY = Y / WorkCurveHelper.YCoeff / WorkCurveHelper.TabHeight * imageHeight;


                        // 画水平线
                        graphics.DrawLine(crossPen, locX - crossSize / 2, locY, locX + crossSize / 2, locY);
                        // 画垂直线
                        graphics.DrawLine(crossPen, locX, locY - crossSize / 2, locX, locY + crossSize / 2);


                    }

                }
                this.largeViewPictureBox.Image = scaledImage;

                this.largeViewMutex.ReleaseMutex();

            }
            else
            {
                float curPixelX = (WorkCurveHelper.X / WorkCurveHelper.XCoeff / WorkCurveHelper.TabWidth * imageWidth);
                float curPixelY = (WorkCurveHelper.Y / WorkCurveHelper.YCoeff / WorkCurveHelper.TabHeight * imageHeight);

                //当前点到平台可移动范围的距离
                float curDisLeft = WorkCurveHelper.TabWidth - (WorkCurveHelper.X / WorkCurveHelper.XCoeff);
                float curDisTop = WorkCurveHelper.TabHeight - (WorkCurveHelper.Y / WorkCurveHelper.YCoeff);
                float curDisRight = WorkCurveHelper.X / WorkCurveHelper.XCoeff;
                float curDisBot = WorkCurveHelper.Y / WorkCurveHelper.YCoeff;
                float colGap = cellSize / WorkCurveHelper.TabWidth * imageWidth;
                float rowGap = cellSize / WorkCurveHelper.TabHeight * imageHeight;

                this.largeViewMutex.WaitOne();

                // 创建新的Bitmap对象并指定目标大小
                Bitmap scaledImage = new Bitmap(imageWidth, imageHeight);


                using (Graphics graphics = Graphics.FromImage(scaledImage))
                {




                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        graphics.FillRectangle(brush, 0, 0, imageWidth, imageHeight);
                    }

                    using (Pen gridPen = new Pen(Color.Black, 1))
                    {

                        // 绘制水平网格线
                        for (int row = 1; row < Math.Ceiling(WorkCurveHelper.TabHeight / cellSize); row++)
                        {
                            graphics.DrawLine(gridPen, 0, row * rowGap, imageWidth, row * rowGap);
                        }

                        // 绘制垂直网格线
                        for (int col = 1; col < Math.Ceiling(WorkCurveHelper.TabWidth / cellSize); col++)
                        {
                            graphics.DrawLine(gridPen, col * colGap, 0, col * colGap, imageHeight);
                        }


                    }


                    using (Pen posPen = new Pen(Color.Red, 2))
                    {

                        graphics.DrawRectangle(posPen, curPixelX - 10, curPixelY - 8, 20, 16);

                    }


                    using (SolidBrush brush = new SolidBrush(Color.Red))
                    {
                        int center = 80;
                        int size = 50;
                        int disToCenter = 15;
                        Font font = new Font("TabDis", 10, FontStyle.Regular);

                        Rectangle rect = new Rectangle(center - (int)(size / 2.0), center - disToCenter, size, size);

                        graphics.DrawString(curDisTop.ToString("f2"), font, brush, rect);


                        rect = new Rectangle(center - disToCenter - size, center, size, size);
                        graphics.DrawString(curDisLeft.ToString("f2"), font, brush, rect);


                        rect = new Rectangle(center + disToCenter, center, size, size);
                        graphics.DrawString(curDisRight.ToString("f2"), font, brush, rect);


                        rect = new Rectangle(center - (int)(size / 2.0), center + disToCenter, size, size);
                        graphics.DrawString(curDisBot.ToString("f2"), font, brush, rect);


                    }

                    System.Drawing.Pen pointsPen = new System.Drawing.Pen(Color.Blue, 2);

                    int index = this.skyrayCamera1.alTempTestPoints.Count - this.skyrayCamera1.alContiTestPoints.Count;

                    float X, Y;
                    int crossSize = 10; // 十字架的大小
                    Pen crossPen = new Pen(Color.Blue, 4);
                    for (int i = 0; i < this.skyrayCamera1.alContiTestPoints.Count; i++)
                    {

                        X = ((Point)this.skyrayCamera1.alTempTestPoints[index + i]).X;
                        Y = ((Point)this.skyrayCamera1.alTempTestPoints[index + i]).Y;

                        float locX = 0;
                        float locY = 0;

                        locX = X / WorkCurveHelper.XCoeff / WorkCurveHelper.TabWidth * imageWidth;

                        locY = Y / WorkCurveHelper.YCoeff / WorkCurveHelper.TabHeight * imageHeight;


                        // 画水平线
                        graphics.DrawLine(crossPen, locX - crossSize / 2, locY, locX + crossSize / 2, locY);
                        // 画垂直线
                        graphics.DrawLine(crossPen, locX, locY - crossSize / 2, locX, locY + crossSize / 2);


                    }


                }
                this.largeViewPictureBox.Image = scaledImage;

                this.largeViewMutex.ReleaseMutex();

            }

            WorkCurveHelper.motorMoving = false;


        }


       

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (e.X > imageWidth || e.Y > imageHeight || WorkCurveHelper.motorMoving || this.skyrayCamera1.Mode == Skyray.Camera.SkyrayCamera.CameraMode.Cell)
                    return;
                
                WorkCurveHelper.motorMoving = true;

                int curPixelX = (int)(WorkCurveHelper.X / WorkCurveHelper.XCoeff / WorkCurveHelper.TabWidth * imageWidth);
                int curPixelY = (int)(WorkCurveHelper.Y / WorkCurveHelper.YCoeff / WorkCurveHelper.TabHeight * imageHeight);

                //获取平台移动步数
                int stepX = (int)((e.X - curPixelX) / (float)imageWidth * WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff);
                int stepY = (int)((e.Y - curPixelY) / (float)imageHeight * WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff);




                this.skyrayCamera1.IsShowCenter = false;

               

                MotorOperator.MotorOperatorXYThread(-stepX, stepY);


         
            }
            else
            {

                if (this.largeViewCamera == null)
                {
                    this.skyrayCamera1.largeCameraSelect = true;
                    this.skyrayCamera1.tsmiCameraFormat_Click(null, null);
                    this.skyrayCamera1.largeCameraSelect = false;
                    initLargeView();
                    initXY();
                    getLargeView();
                    new System.Threading.Thread(new System.Threading.ThreadStart(((UCCameraControl)WorkCurveHelper.ucCamera).updateLargeViewNow)).Start();
                }
            }

        }





        private void tabResetBut_Click(object sender, EventArgs e)
        {
            
            bool flag = false;
            this.buttonWStart.Enabled = flag;
            this.buttonWStop.Enabled = flag;
            this.tabResetBut.Enabled = flag;
            this.btnZAxisOpen.Enabled = flag;
            this.btnZAxisClose.Enabled = flag;
            this.gbxXYTable.Enabled = flag;

            MotorOperator.MotorOperatorResetThread();

            Thread.Sleep(28000);

            flag = true;
            this.buttonWStart.Enabled = flag;
            this.buttonWStop.Enabled = flag;
            this.tabResetBut.Enabled = flag;
            this.btnZAxisOpen.Enabled = flag;
            this.btnZAxisClose.Enabled = flag;
            this.gbxXYTable.Enabled = flag;
            WorkCurveHelper.X = WorkCurveHelper.ResetX;
            WorkCurveHelper.Y = WorkCurveHelper.ResetY;
            WorkCurveHelper.Z = WorkCurveHelper.ResetZ;

            if (DifferenceDevice.CurCameraControl.largeViewCamera != null)
            {
                ((Thick)WorkCurveHelper.curThick).OUTSample();
                ((Thick)WorkCurveHelper.curThick).INSample();
            }
        }



        void UCCameraControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timers != null)
                timers.Enabled = false;
            if (timerCurrent != null)
                timerCurrent.Enabled = false;
            this.skyrayCamera1.Close();
        }

        public static void addCheckResult(string filePath, string data)
        {
            try
            {
                // 检查文件是否存在
                if (!File.Exists(filePath))
                {
                    // 文件不存在，创建文件
                    File.Create(filePath).Close();
                }


                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(data);

                }
            }
            catch
            {
                return;
            }
        }

        public static void addSteps(string filePath, string line1,string line2)
        {

            try
            {
                // 检查文件是否存在
                if (!File.Exists(filePath))
                {
                    // 文件不存在，创建文件
                    File.Create(filePath).Close();
                }


                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.WriteLine(line1);
                    sw.WriteLine(line2);

                }
            }
            catch
            {
                return;

            }
        }

        public void selfCheck()
        {

            string path =  "电机自检.txt";
            WorkCurveHelper.selfCheck = true;
            WorkCurveHelper.loopTestDemo = bool.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/loopTestDemo"));
            this.buttonWStop.Enabled = true;
            
            int curXSteps = 0;
            int curYSteps = 0;
            do
            {
                curXSteps = 0;
                curYSteps = 0;

                addCheckResult(path, System.DateTime.Now.ToString());
                int sleepTime = 15000;

                //左检
                WorkCurveHelper.contiMoving = true;
                MotorDirections dirX = WorkCurveHelper.DeviceCurrent.MotorXDirect == 0 ? MotorDirections.Negative : MotorDirections.Positive;

                if (dirX == MotorDirections.Negative)
                    MotorOperator.MotorOperatorXYThread((int)(-1000 * WorkCurveHelper.XCoeff), 0);
                else
                    MotorOperator.MotorOperatorXYThread((int)(1000 * WorkCurveHelper.XCoeff), 0);
                MotorOperator.stopMotorThread(1);
                Thread.Sleep(sleepTime);
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;

                Thread.Sleep(1000);
                curXSteps += Math.Abs( WorkCurveHelper.xSteps);
                
                //右检
                WorkCurveHelper.contiMoving = true;
                dirX = WorkCurveHelper.DeviceCurrent.MotorXDirect == 0 ? MotorDirections.Positive : MotorDirections.Negative;

                if (dirX == MotorDirections.Negative)
                    MotorOperator.MotorOperatorXYThread((int)(-1000 * WorkCurveHelper.XCoeff), 0);
                else
                    MotorOperator.MotorOperatorXYThread((int)(1000 * WorkCurveHelper.XCoeff), 0);
                MotorOperator.stopMotorThread(1);
                Thread.Sleep(sleepTime);
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;


                Thread.Sleep(1000);
                curXSteps += Math.Abs( WorkCurveHelper.xSteps);

                //上检
                WorkCurveHelper.contiMoving = true;
                MotorDirections dirY = WorkCurveHelper.DeviceCurrent.MotorYDirect == 0 ? MotorDirections.Positive : MotorDirections.Negative;

                if (dirY == MotorDirections.Negative)
                    MotorOperator.MotorOperatorXYThread(0, (int)(-1000 * WorkCurveHelper.YCoeff));
                else
                    MotorOperator.MotorOperatorXYThread(0, (int)(1000 * WorkCurveHelper.YCoeff));
                MotorOperator.stopMotorThread(2);
                Thread.Sleep(sleepTime);
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;

                Thread.Sleep(1000);
                curYSteps += Math.Abs(WorkCurveHelper.ySteps);
                //下检
                WorkCurveHelper.contiMoving = true;
                dirY = WorkCurveHelper.DeviceCurrent.MotorYDirect == 0 ? MotorDirections.Negative : MotorDirections.Positive;

                if (dirY == MotorDirections.Negative)
                    MotorOperator.MotorOperatorXYThread(0, (int)(-1000 * WorkCurveHelper.YCoeff));
                else
                    MotorOperator.MotorOperatorXYThread(0, (int)(1000 * WorkCurveHelper.YCoeff));
                MotorOperator.stopMotorThread(2);
                Thread.Sleep(sleepTime);
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;
                Thread.Sleep(1000);
                curYSteps += Math.Abs( WorkCurveHelper.ySteps);

               

            }
            while (WorkCurveHelper.loopTestDemo);
            
            WorkCurveHelper.xSteps = curXSteps;
            WorkCurveHelper.ySteps = curYSteps;

            addSteps("电机自检步长.txt", WorkCurveHelper.xSteps.ToString(), WorkCurveHelper.ySteps.ToString());
            WorkCurveHelper.loopTestDemo = false;
            WorkCurveHelper.selfCheck = false;

            Thread.Sleep(2000);
            Environment.Exit(0);
            

        }



        
        private void btnXAxisLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (WorkCurveHelper.contiMoving)
            {
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;
            }
        }

        private void btnXAxisRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (WorkCurveHelper.contiMoving)
            {
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;
            }   
        }


        private void btnXAxisLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.skyrayCamera1.IsShowCenter = false;
                int value = XYSpeed;
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
           && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorX)
                {

                    if (WorkCurveHelper.motorMoving)
                    {
                        return;
                    }
                    else
                    {
                        WorkCurveHelper.motorMoving = true;
                    }


                    MotorDirections dirX = WorkCurveHelper.DeviceCurrent.MotorXDirect==0 ? MotorDirections.Negative : MotorDirections.Positive;
                    float moveValue = dirX == MotorDirections.Negative ? -FixedXYValue : FixedXYValue;

                    

                    if (this.chkFixedXY.Checked)
                    {

                        if (WorkCurveHelper.FixedDisXY)
                        {

                          

                            bool flag = MotorOperator.MotorOperatorXYThread((int)(moveValue * WorkCurveHelper.XCoeff), 0);
                          
                            
                           
                        }
                        else
                        {



                            bool flag = MotorOperator.MotorOperatorXYThread((int)(moveValue), 0);
                            
                        }

                        

                    }
                    else
                    {

                        

                        WorkCurveHelper.contiMoving = true;
                        if (dirX == MotorDirections.Negative)
                            MotorOperator.MotorOperatorXYThread((int)(-WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff), 0);
                        else
                            MotorOperator.MotorOperatorXYThread((int)(WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff), 0);

                        MotorOperator.stopMotorThread(1);

                        

                    }

                }
            }
        }



        private void btnXAxisRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.skyrayCamera1.IsShowCenter = false;
                int value = XYSpeed;
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
           && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorX)
                {
                    if (WorkCurveHelper.motorMoving)
                    {
                        
                        return;
                    }
                    else
                    {
                        WorkCurveHelper.motorMoving = true;
                    }
                    MotorDirections dirX = WorkCurveHelper.DeviceCurrent.MotorXDirect == 0 ? MotorDirections.Positive : MotorDirections.Negative;
                    float moveValue = dirX == MotorDirections.Negative ? -FixedXYValue : FixedXYValue;



                    if (this.chkFixedXY.Checked)
                    {

                        if (WorkCurveHelper.FixedDisXY)
                        {

                            bool flag = MotorOperator.MotorOperatorXYThread((int)(moveValue * WorkCurveHelper.XCoeff), 0);
                        }
                        else
                        {


                            bool flag = MotorOperator.MotorOperatorXYThread((int)(moveValue), 0);

                           

                        }

                    }
                    else
                    {

                        WorkCurveHelper.contiMoving = true;

                        if (dirX == MotorDirections.Negative)
                            MotorOperator.MotorOperatorXYThread((int)(-WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff), 0);
                        else
                            MotorOperator.MotorOperatorXYThread((int)(WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff), 0);

                        MotorOperator.stopMotorThread(1);
                    }

                        
                        

                }
            }
        }



     


        private void btnYAxisIn_MouseUp(object sender, MouseEventArgs e)
        {

            if (WorkCurveHelper.contiMoving)
            {
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;
            }
            

         
        }

        private void btnYAxisOut_MouseUp(object sender, MouseEventArgs e)
        {

            if (WorkCurveHelper.contiMoving)
            {
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;
            }
            

        }

        private void btnYAxisIn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.skyrayCamera1.IsShowCenter = false;
                int value = XYSpeed;
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
           && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorY)
                {

                    if (WorkCurveHelper.motorMoving)
                        return;
                    else
                    {
                        WorkCurveHelper.motorMoving = true;
                    }

                    MotorDirections dirY = WorkCurveHelper.DeviceCurrent.MotorYDirect == 0 ? MotorDirections.Positive : MotorDirections.Negative;
                    float moveValue = dirY == MotorDirections.Negative ? -FixedXYValue : FixedXYValue;


                 

                    if (this.chkFixedXY.Checked)
                    {
                        if (WorkCurveHelper.FixedDisXY)
                        {




                            bool flag = MotorOperator.MotorOperatorXYThread(0, (int)(moveValue * WorkCurveHelper.YCoeff));


                          

                        }
                        else
                        {



                            bool flag = MotorOperator.MotorOperatorXYThread(0, (int)(moveValue));

                        }
                    }
                    else
                    {


                        WorkCurveHelper.contiMoving = true;

                        if (dirY == MotorDirections.Negative)
                            MotorOperator.MotorOperatorXYThread(0, (int)(-WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff));
                        else
                            MotorOperator.MotorOperatorXYThread(0, (int)(WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff));

                        MotorOperator.stopMotorThread(2);

                    }
                }
            }
        }



      

        private void btnYAxisOut_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.skyrayCamera1.IsShowCenter = false;
                int value = XYSpeed;
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
            && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorY)
                {

                    if (WorkCurveHelper.motorMoving)
                        return;
                    else
                    {
                        WorkCurveHelper.motorMoving = true;
                    }


                    MotorDirections dirY = WorkCurveHelper.DeviceCurrent.MotorYDirect == 0 ? MotorDirections.Negative : MotorDirections.Positive;
                    float moveValue = dirY == MotorDirections.Negative ? -FixedXYValue : FixedXYValue;


                    if (this.chkFixedXY.Checked)
                    {
                        if (WorkCurveHelper.FixedDisXY)
                        {

                            bool flag = MotorOperator.MotorOperatorXYThread(0, (int)(moveValue * WorkCurveHelper.YCoeff));
                            

                        }
                        else
                        {


                            bool flag = MotorOperator.MotorOperatorXYThread(0, (int)(moveValue));

                           


                        }
                    }
                    else
                    {
                      


                 
                        WorkCurveHelper.contiMoving = true;

                        if (dirY == MotorDirections.Negative)
                            MotorOperator.MotorOperatorXYThread(0, (int)(-WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff));
                        else
                            MotorOperator.MotorOperatorXYThread(0, (int)(WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff));

                        MotorOperator.stopMotorThread(2);

                    }
                }
            }
        }


        

     
        private void btnZAxisOpen_MouseUp(object sender, MouseEventArgs e)
        {

            if (WorkCurveHelper.contiMoving)
            {
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;
            }
        }

        private void btnZAxisClose_MouseUp(object sender, MouseEventArgs e)
        {
            if (WorkCurveHelper.contiMoving)
            {
                WorkCurveHelper.nextNo++;
                WorkCurveHelper.contiMoving = false;
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

                    if (WorkCurveHelper.motorMoving)
                        return;
                    else
                    {
                        WorkCurveHelper.motorMoving = true;
                    }


                

                    if (this.chkFixedZ.Checked)
                    {
                        if (WorkCurveHelper.FixedDisZ)
                        {

                            bool flag = MotorOperator.MotorOperatorZThread((int)(-FixedZValue * WorkCurveHelper.ZCoeff));

                           
                        }
                        else
                        {
                            bool flag = MotorOperator.MotorOperatorZThread((int)(-FixedZValue));

                            
                        }

                    }
                    else
                    {
                        WorkCurveHelper.contiMoving = true;
                        MotorOperator.MotorOperatorZThread((int)(-140 * WorkCurveHelper.ZCoeff));
                        MotorOperator.stopMotorThread(3);
                        
                    }
                    
                }
            }
        }

       

        private void btnZAxisClose_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentVice.HasMotorZ)
            {
                int value = (ZSpeed == this.vScrollZSpeed.Minimum) ? (ZSpeed + 1) : ZSpeed;
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
              && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                {
                    if (WorkCurveHelper.motorMoving)
                        return;
                    else
                    {
                        WorkCurveHelper.motorMoving = true;
                    }


                    if (this.chkFixedZ.Checked)
                    {
                        if (WorkCurveHelper.FixedDisZ)
                        {
                            bool flag = MotorOperator.MotorOperatorZThread((int)(FixedZValue * WorkCurveHelper.ZCoeff));
                            
                        }
                        else
                        {
                            bool flag = MotorOperator.MotorOperatorZThread((int)(FixedZValue));
                           
                        }
                    }
                    else
                    {
                        WorkCurveHelper.contiMoving = true;
                        MotorOperator.MotorOperatorZThread((int)(140 * WorkCurveHelper.ZCoeff));
                        MotorOperator.stopMotorThread(3);
                        
                    }
                }
            }
        }




        private void btnAutoFocal_Click(object sender, EventArgs e)
        {
            int zspeed = int.Parse(lblZSpeed1.Text);
            MotorAdvance.AutoTuneHeightSync(zspeed);
        }

        private void chkIsLazerOpen_Click(object sender, EventArgs e)
        {
            if (currentVice.HasMotorZ)
            {
                if (chkLaser.Checked)
                {
                    if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.connect != DeviceConnect.Connect
                        || DifferenceDevice.interClassMain.deviceMeasure.interfacce.StopFlag)
                        btnAutoFocal.Enabled = true;
                    if (WorkCurveHelper.bCustomDevice || WorkCurveHelper.bHeightLayer)
                        MotorInstance.s_singleInstanceZ.OpenHeightLazer();
                    else
                        WorkCurveHelper.deviceMeasure.interfacce.Pump.TOpen();

                    // MotorInstance.s_singleInstanceZ.OpenHeightLazer();
                }
                else
                {
                    btnAutoFocal.Enabled = false;
                    if (WorkCurveHelper.bCustomDevice || WorkCurveHelper.bHeightLayer)
                        MotorInstance.s_singleInstanceZ.CloseHeightLazer();
                    else
                        WorkCurveHelper.deviceMeasure.interfacce.Pump.TClose();

                    // MotorInstance.s_singleInstanceZ.CloseHeightLazer();
                }
            }
        }

        public void AutoCheckLazer()
        {
            if (currentVice.HasMotorZ)
            {
                if (chkLaser.Checked)
                {
                    if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.connect != DeviceConnect.Connect
                        || DifferenceDevice.interClassMain.deviceMeasure.interfacce.StopFlag)
                        btnAutoFocal.Enabled = true;
                    if (WorkCurveHelper.bCustomDevice || WorkCurveHelper.bHeightLayer)
                        MotorInstance.s_singleInstanceZ.OpenHeightLazer();
                    else
                        WorkCurveHelper.deviceMeasure.interfacce.Pump.TOpen();

                    // MotorInstance.s_singleInstanceZ.OpenHeightLazer();
                }
                else
                {
                    btnAutoFocal.Enabled = false;
                    if (WorkCurveHelper.bCustomDevice || WorkCurveHelper.bHeightLayer)
                        MotorInstance.s_singleInstanceZ.CloseHeightLazer();
                    else
                        WorkCurveHelper.deviceMeasure.interfacce.Pump.TClose();

                    // MotorInstance.s_singleInstanceZ.CloseHeightLazer();
                }
            }
        }



        private void vScrollXYSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (lblXYSpeed.Created && (currentVice.HasMotorX || currentVice.HasMotorY))
            {
                lblXYSpeed.Text = vScrollXYSpeed.Value.ToString();
                lblXYSpeedw1.Text = Convert.ToString((170 - vScrollXYSpeed.Value) > 21 ? 170 - vScrollXYSpeed.Value : 21);
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
            if (lblZSpeed.Created && currentVice.HasMotorZ)
            {
                lblZSpeed.Text = vScrollZSpeed.Value.ToString();
                lblZSpeed1.Text = Convert.ToString((170 - vScrollZSpeed.Value) > 21 ? 170 - vScrollZSpeed.Value : 21);// Convert.ToString(170 - vScrollZSpeed.Value);

                currentVice.MotorZSpeed = ZSpeed;
                string sql = "Update Device Set MotorZSpeed = " + ZSpeed + " Where Id = " + currentVice.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
           
        }
        
        public void SetTestInformation(string s, bool IsBtnShow)
        {
            skyrayCamera1.SetTestInformation(s, IsBtnShow);
        }

        public void SetFocusText(string text)
        {
            this.btnAutoFoucus.Text = text;
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
            if (RBFast.Checked && RBFast.Visible)
            {
                lblZSpeed.Text = lblZSpeed1.Text = Math.Abs(170 - WorkCurveHelper.ZFast).ToString();
                currentVice.MotorZSpeed = ZSpeed;
                string sql = "Update Device Set MotorZSpeed = " + ZSpeed + " Where Id = " + currentVice.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            }
        }

        private void RbMd_CheckedChanged(object sender, EventArgs e)
        {
            if (RBMd.Checked && RBMd.Visible)
            {
                lblZSpeed.Text = lblZSpeed1.Text = Math.Abs(170 - WorkCurveHelper.ZMiddle).ToString();

                currentVice.MotorZSpeed = ZSpeed;
                string sql = "Update Device Set MotorZSpeed = " + ZSpeed + " Where Id = " + currentVice.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            }
        }

        private void RBSlow_CheckedChanged(object sender, EventArgs e)
        {
            if (RBSlow.Checked && RBSlow.Visible)
            {
                lblZSpeed.Text = lblZSpeed1.Text = Math.Abs(170 - WorkCurveHelper.ZSlow).ToString();
                currentVice.MotorZSpeed = ZSpeed;
                string sql = "Update Device Set MotorZSpeed = " + ZSpeed + " Where Id = " + currentVice.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            }
        }

        private void skyrayCamera1_FormatChanged(object sender, EventArgs e)
        {
            // this.mtSplitter1.SplitPosition = this.skyrayCamera1.CapVideoWidth;
        }

        private void chkIsLazerOpen_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gbxZFocal_Enter(object sender, EventArgs e)
        {

        }

        private void buttonShowPosition_Click(object sender, EventArgs e)
        {
            labelx.Text = WorkCurveHelper.positionP.X.ToString();
            labely.Text = WorkCurveHelper.positionP.Y.ToString();
        }

        private void chkresetPoint_CheckedChanged(object sender, EventArgs e)
        {
            if (chkresetPoint.Checked)
            {
                WorkCurveHelper.bXYMotorSetp = true;
                buttonWReset.Text = Info.SetReset;
            }
            else
            {
                WorkCurveHelper.bXYMotorSetp = false;
                buttonWReset.Text = Info.Reset;
            }
        }

        private void btnZAxisClose_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }



        private void smallCameraLightBar_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (visible)
                {
                    visible = false;
                    if (surfaceSource == null)
                        surfaceSource = SurfaceSourceLight.FindAll()[0];
                    this.lblSmallCameraLight.Text = smallCameraLightBar.Value.ToString();
                    surfaceSource.FirstLight = ushort.Parse(lblSmallCameraLight.Text);               
                    surfaceSource.Save();
                    DifferenceDevice.MediumAccess.SetSurfaceSource(surfaceSource.FirstLight, surfaceSource.SecondLight, surfaceSource.ThirdLight, surfaceSource.FourthLight);
                    visible = true;
                }
            }
            catch
            {
            }
            finally
            {
                visible = true;
            }
        }


        private void largeCameraLightBar_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (visible)
                {
                    visible = false;
                    if (surfaceSource == null)
                        surfaceSource = SurfaceSourceLight.FindAll()[0];
                    this.lblLargeCameraLight.Text = largeCameraLightBar.Value.ToString();                   
                    surfaceSource.ThirdLight = ushort.Parse(lblLargeCameraLight.Text);
                    surfaceSource.Save();
                    DifferenceDevice.MediumAccess.SetSurfaceSource(surfaceSource.FirstLight, surfaceSource.SecondLight, surfaceSource.ThirdLight, surfaceSource.FourthLight);
                    visible = true;
                }
            }
            catch
            {
            }
            finally
            {
                visible = true;
            }
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

        void interClassMain_OnSateChanged(object sender, Skyray.EDX.Common.Component.BoolEventArgs e)
        {
            TestStartAfterControlState(e.Value);
        }

        private void TestStartAfterControlState(bool flag)
        {
            this.BeginInvoke(new Action(() =>
            {
                this.buttonWStart.Enabled = flag;
                this.buttonWStop.Enabled = !flag;
                this.tabResetBut.Enabled = flag;
                this.btnZAxisOpen.Enabled = flag;
                this.btnZAxisClose.Enabled = flag;

            }));


        }


        private void textBoxWInputWalk_Leave(object sender, EventArgs e)
        {
            int count = 21;
            bool isCount = Int32.TryParse(fixedXYValue.Text, out count);
            if (count < 21)
            {
                count = 21;
            }
        }

        private void chkIsLazerOpen_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void btnOutInSample_Click(object sender, EventArgs e)
        {
            int value = XYSpeed;
            if (btnOutInSample.Text == Info.OutSample)
            {
                OutSample();
                btnOutInSample.Text = Info.InSample;
            }
            else
            {
                InSample();
                btnOutInSample.Text = Info.OutSample;
            }
        }

        public void InSample()
        {
         
        }

        public void OutSample()
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (btnShowCamerPoint.Text == Info.ShowDefinition)
            {
                Skyray.EDX.Common.CameraRefMotor.IsShowDefine = true;
                skyrayCamera1.FocusSize = (WorkCurveHelper.FocusArea == 1) ? 2 : ((WorkCurveHelper.FocusArea == 2) ? 4 : 8);
                skyrayCamera1.CamerCurrentX = double.Parse(textBox2.Text);
                skyrayCamera1.CamerCurrentY = double.Parse(textBox3.Text);
                btnShowCamerPoint.Text = Info.StopDefinition;
                timerCurrent.Enabled = true;
            }
            else
            {
                Skyray.EDX.Common.CameraRefMotor.IsShowDefine = false;
                btnShowCamerPoint.Text = Info.ShowDefinition;
                labelCurrent.Text = "...";
                if (timerCurrent != null)
                {
                    timerCurrent.Stop();
                    timerCurrent.Enabled = false;
                }
            }
            // Bitmap bmp = skyrayCamera1.GetCamerImage;

            //WorkCurveHelper.cx = double.Parse(textBox2.Text) ;
            //WorkCurveHelper.cy = double.Parse(textBox3.Text);
            //Skyray.EDX.Common.CameraRefMotor.IsTransLocked = true;
            //Skyray.EDX.Common.CameraRefMotor.IsReceiveLocked = false;
            //Thread.Sleep(50);
            //if (Skyray.EDX.Common.Component.MotorAdvance.CameraBitmap != null)
            //{
            //    double current = MotorAdvance.GetMaxandCurrentDefi(Skyray.EDX.Common.Component.MotorAdvance.CameraBitmap, WorkCurveHelper.cx,WorkCurveHelper.cy);
            //    labelCurrent.Text = current.ToString();
            //}
            //Skyray.EDX.Common.CameraRefMotor.IsTransLocked = false;


        }
        private void timerCurrent_Tick(object source, EventArgs e)
        {
            labelCurrent.Text = skyrayCamera1.CamerCurrent.ToString("f0");
            Skyray.EDX.Common.CameraRefMotor.ShowCurrentDefin = skyrayCamera1.CamerCurrent;
        }
        private System.Windows.Forms.Timer timerCurrent;

        private void btnAutoFoucus_Click(object sender, EventArgs e)
        {
            if (btnAutoFoucus.Text == Info.AutoFocus)
            {
                WorkCurveHelper.cx = double.Parse(textBox2.Text);
                WorkCurveHelper.cy = double.Parse(textBox3.Text);
                btnAutoFoucus.Text = Info.StopTest;
                timers = new System.Windows.Forms.Timer();
                timers.Interval = 1000;
                timers.Tick += new EventHandler(timer_Tick);//定时器事件
                timers.Start();


                MotorAdvance.AutoTuneHeightSync(ZSpeed);
            }
            else
            {

                if (timers != null)
                    timers.Stop();
                MotorAdvance.CancelAllStop();
                btnAutoFoucus.Text = Info.AutoFocus;
            }


        }

        System.Windows.Forms.Timer timers;

        private void button2_Click(object sender, EventArgs e)
        {
            timerCurrent.Enabled = false;
        }

        private void timer_Tick(object source, EventArgs e)
        {
            if (MotorAdvance.isFinishFocus == true)
            {
                btnSearchfocus.Image = global::Skyray.UC.Properties.Resources.eyes;
                //btnAutoFoucus.Text = Info.AutoFocus;
                timers.Stop();
                if (MotorAdvance.outFocusRange)
                {
                    timers.Stop();
                    timers.Dispose();
                    timers = null;
                    Msg.Show(Info.NotFocus);
                }
            }
            else
            {
                btnSearchfocus.Image = global::Skyray.UC.Properties.Resources.searcheyes;

            }
        }


        private void btnYAxisOut_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSearchfocus_Click(object sender, EventArgs e)
        {
            if (MotorAdvance.isFinishFocus == true)
            {
                btnSearchfocus.Image = global::Skyray.UC.Properties.Resources.searcheyes;
                WorkCurveHelper.cx = double.Parse(textBox2.Text);
                WorkCurveHelper.cy = double.Parse(textBox3.Text);
                MotorAdvance.FociBMPX = skyrayCamera1.FociBmpX;
                MotorAdvance.FociBMPY = skyrayCamera1.FociBmpY;
                btnAutoFoucus.Text = Info.StopTest;
                timers = new System.Windows.Forms.Timer();
                timers.Interval = 1000;
                timers.Tick += new EventHandler(timer_Tick);//定时器事件
                timers.Start();
                MotorAdvance.AutoTuneHeightSync(ZSpeed);
            }
            else
            {
                if (timers != null)
                    timers.Stop();
                btnSearchfocus.Image = global::Skyray.UC.Properties.Resources.eyes;

                MotorAdvance.CancelAllStop();
                // btnAutoFoucus.Text = Info.AutoFocus;
            }
        }

        private void trackBarZMotor_MouseDown(object sender, MouseEventArgs e)
        {
            int x, bias = 8;
            if (e.Y <= bias)
                x = 0;
            else if (e.Y >= trackBarZMotor.Height - bias)
                x = trackBarZMotor.Height - bias * 2;
            else
                x = e.Y - bias;
            trackBarZMotor.Value = Convert.ToInt32(x * Convert.ToDouble(trackBarZMotor.Maximum
            - trackBarZMotor.Minimum) / (trackBarZMotor.Height - bias * 2) + trackBarZMotor.Minimum) * -1;
            Console.WriteLine("track bar x: " + x.ToString());
            if (e.Button == MouseButtons.Left && currentVice.HasMotorZ)
            {
                // timerM.Enabled = true;
                int direction = Convert.ToInt32(trackBarZMotor.Value) > 0 ? WorkCurveHelper.DeviceCurrent.MotorZDirect : Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1);
                int speed = ZspeedReturn(Math.Abs(Convert.ToInt32(trackBarZMotor.Value)));// 170 - Math.Abs(Convert.ToInt32(trackBarZMotor.Value)) ;
                ZmotorMove(speed, direction);
            }
            if (e.Button == MouseButtons.Right && currentVice.HasMotorZ)
            {
                //int direction = Convert.ToInt32(trackBarZMotor.Value) > 0 ? WorkCurveHelper.DeviceCurrent.MotorZDirect : Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1);
                int direction = e.Y < 80 ? WorkCurveHelper.DeviceCurrent.MotorZDirect : Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1);

                int speed = 170 - Math.Abs(WorkCurveHelper.ZSlow);// 170 - Math.Abs(Convert.ToInt32(trackBarZMotor.Value)) ;
                ZmotorMove(speed, direction);
            }
        }

        private int ZspeedReturn(int zvalue)
        {
            int speed = 0;
            if (zvalue <= 10)
                speed = WorkCurveHelper.ZSlow;
            else if (zvalue <= 20)
                speed = WorkCurveHelper.ZMiddle;
            else
                speed = WorkCurveHelper.ZFast;

            speed = 170 - speed;
            return speed;
        }

        System.Windows.Forms.Timer timerM = new System.Windows.Forms.Timer();//计时器
        int timeout = 0;//超时时间
        void timerM_Tick(object sender, EventArgs e)
        {
            timeout++;
            if (timeout == 5)
            {
                int direction = Convert.ToInt32(trackBarZMotor.Value) > 0 ? WorkCurveHelper.DeviceCurrent.MotorZDirect : Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1);
                int speed = ZspeedReturn(Math.Abs(Convert.ToInt32(trackBarZMotor.Value)));// 170 - Math.Abs(Convert.ToInt32(trackBarZMotor.Value)) ;
                ZmotorMove(speed, direction);
            }
        }


        private void ZmotorMove(int speed, int direction)
        {
            if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
          && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
            {
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, direction, CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, true, speed);
            }
        }
        private void trackBarZMotor_MouseUp(object sender, MouseEventArgs e)
        {
            timerM.Enabled = false;
            timeout = 0;
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                int direction = Convert.ToInt32(trackBarZMotor.Value) > 0 ? WorkCurveHelper.DeviceCurrent.MotorZDirect : Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1);
                // int speed = 170 - Math.Abs(Convert.ToInt32(trackBarZMotor.Value));
                int speed = ZspeedReturn(Math.Abs(Convert.ToInt32(trackBarZMotor.Value)));
                if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                    DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, direction, 0, true, speed);
            }
            trackBarZMotor.Value = 0;
        }

        private void skyrayCamera1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void skyrayCamera1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void chkAutoInOutSample_CheckedChanged(object sender, EventArgs e)
        {
            WorkCurveHelper.bOpenOutSample = chkAutoInOutSample.Checked;
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/bOpenOutSample", WorkCurveHelper.bOpenOutSample.ToString());

        }

        private void btnEditMulit_Click(object sender, EventArgs e)
        {

            chkEditMulti.Checked = !chkEditMulti.Checked;
            if (chkEditMulti.Checked)
            {
                btnEditMulit.Text = Info.Complete;
            }
            else
            {

                btnEditMulit.Text = Info.Edit;
            }
        }

        private void groupboxPoint_Enter(object sender, EventArgs e)
        {

        }

        private void chkEditMulti_CheckedChanged(object sender, EventArgs e)
        {

        }

     

  
        private void testDemoChk_CheckedChanged(object sender, EventArgs e)
        {

            if (chkTestDemo.Checked)
                WorkCurveHelper.testDemo = true;
            else
                WorkCurveHelper.testDemo = false;

            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/testDemo", WorkCurveHelper.testDemo.ToString());
          
        }

        private void tabResetBut_Enter(object sender, EventArgs e)
        {
            this.Select();
        }

     

        private void panelZ1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblWhitepalance_Click(object sender, EventArgs e)
        {

        }

       

        private void gbxZFocal_Enter_1(object sender, EventArgs e)
        {

        }

        private void rbZLow_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbZLow.Checked)
                WorkCurveHelper.zSpeedMode = false;
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/zSpeedMode", WorkCurveHelper.zSpeedMode.ToString());

        }

        private void rbZFast_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbZFast.Checked)
                WorkCurveHelper.zSpeedMode = true;
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/zSpeedMode", WorkCurveHelper.zSpeedMode.ToString());

        }

      
        private void chkFixedXY_CheckedChanged(object sender, EventArgs e)
        {
            WorkCurveHelper.FixedXY = this.chkFixedXY.Checked;
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/FixedXY", WorkCurveHelper.FixedXY.ToString());

        }


        private void chkFixedZ_CheckedChanged(object sender, EventArgs e)
        {
            WorkCurveHelper.FixedZ = this.chkFixedZ.Checked;
            ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/FixedZ", WorkCurveHelper.FixedZ.ToString());

        }

      




        private void encoderMoveUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (!chkFixedXY.Checked)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.encoderMotorCode, 1, (int)(WorkCurveHelper.encoderCoeff * 10000), true, 1);
            else
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.encoderMotorCode, 1, (int)(int.Parse(this.fixedXYValue.Text)), true, 1);

        }


        private void encoderMoveUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (!chkFixedXY.Checked)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.encoderMotorCode, 1, 0, true, 1);
            
        }

        private void encoderMoveDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (!chkFixedXY.Checked)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.encoderMotorCode, 0, (int)(WorkCurveHelper.encoderCoeff * 10000), true, 1);
            else
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.encoderMotorCode, 0, (int)(int.Parse(this.fixedXYValue.Text)), true, 1);

        }

        private void encoderMoveDown_MouseUp(object sender, MouseEventArgs e)
        {

            if (!chkFixedXY.Checked)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.encoderMotorCode, 0, 0, true, 1);

        }


        private void largeViewPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
           
            using (Graphics graphics = Graphics.FromImage(this.largeViewPictureBox.Image))
            {

                using (Brush brush = new SolidBrush(Color.White))
                {
                    graphics.FillRectangle(brush, 0, 0, 150, 25);
                }


                
                float curDisLeft = WorkCurveHelper.TabWidth * ( 1- (float)e.Location.X /imageWidth);
                float curDisTop = WorkCurveHelper.TabHeight * (1 - (float)e.Location.Y / imageHeight);


                graphics.DrawString(curDisLeft.ToString("f2") + "      " + curDisTop.ToString("f2"), new Font("loc", 12, FontStyle.Regular), Brushes.Blue, new PointF(15,0));
            }

            this.largeViewPictureBox.Invalidate(new Rectangle(0, 0, 150, 25));
        }

        private void btnXAxisLeft_Click(object sender, EventArgs e)
        {

        }

       

     

        

    }


}
