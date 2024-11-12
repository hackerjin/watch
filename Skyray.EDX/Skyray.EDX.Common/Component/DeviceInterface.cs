using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;
using System.Threading;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;
using System.Diagnostics;
using Skyray.Dog;
using System.IO;
using Newtonsoft.Json;

namespace Skyray.EDX.Common.Component
{
    public class BoolEventArgs : EventArgs
    {
        public bool Value { get; set; }
        public BoolEventArgs(bool b)
        {
            Value = b;
        }
    }
    public abstract class DeviceInterface
    {
        //public static bool IsIndia = false;
        public event Action<bool, string> OnDievceConnect;
        public Action CheckDog;

        public MatchPlus MatchPlus = MatchPlus.MatchOff;
        /// <summary>
        /// 线程内部 管流
        /// </summary>
        public double ReturnCurrent;
        /// <summary>
        /// 线程内部 真空度
        /// </summary>
        public double ReturnVacuum;
        public double ReturnVacuumDown;
        /// <summary>
        /// 反馈encoder的高度
        /// </summary>
        public double ReturnEncoderHeight;
        /// <summary>
        /// 线圈值
        /// </summary>
        public double ReturnEncoderValue;

        /// <summary>
        /// 总计数
        /// </summary>
        //public int TestTotalCount;
        public double TestTotalCount;
        public double StartVacuum;

        public QualeElement QualeElement;
        /// <summary>
        /// 线程内部 管压
        /// </summary>
        public double ReturnVoltage;

        public double ReturnTemp;

        /// <summary>
        /// 盖子状态 是否关闭
        /// </summary>
        public bool ReturnCoverClosed;

        public double CountRating;

        public bool IsConnect = false;

        public bool IsSpin = false;

        public bool IfAdjustOk = false;

        public bool TestButtonPressed = false;


        public static ushort[] Bytes2Ushorts(byte[] src)
        {
            int len = src.Length;
            byte[] srcPlus = new byte[len + 1];
            src.CopyTo(srcPlus, 0);
            int count = len >> 1;

            if (len % 2 != 0)
            {
                count += 1;
            }

            ushort[] dest = new ushort[count];


            for (int i = 0; i < count; i++)
            {
                dest[i] = (ushort)(srcPlus[i * 2] & 0xff | srcPlus[2 * i + 1] << 8);
            }


            return dest;
        }





        public static void SetReal(ushort[] src, int start, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            ushort[] dest = Bytes2Ushorts(bytes);

            ushort temp = dest[0];
            dest[0] = dest[1];
            dest[1] = temp;
            dest.CopyTo(src, start);
        }

        /// <summary>
        /// 计数率
        /// </summary>
        public double ReturnCountRate
        {
            get
            {

                return CountRating;
                //if (state == DeviceState.Idel)
                //{
                //    return CountRate();
                //}
                //else
                //{
                //    return CountRating;
                //}
            }

        }

        public bool IsCoverOpenedTest = true;

        public bool HasVauccum = false;
        // public Dp5Device dp5Device;
        public Dp5Interface dp5Device;

        public bool bTimenate;

        public int numDev = 0;//原有未用到的变量，现在使用到新6000B仪器的样品杯位置的更新5秒中刷新一次

        public double dblCurrentRate;

        public int[] statu = new int[32];

        public bool IsExistsLock;

        /// <summary>
        /// 预热参数
        /// </summary>
        public PreHeatParams heatParams;

        public bool ExistMagnet;//电磁铁
        public ChamberCellState[] CellStates;//样品室状态
        public static int MaxCountRate; ///仪器所允许的最大计算率
        public IntPtr OwnerHandle;
        public int DropTime;
        public int RecordDropTime;
        public uint totalUsedTime;///<现在已经用掉的系统时间  记录每次暂停累加的系统时间
        public uint currentTestTime;///<网口通讯中已经测试的系统时间  用以暂停的时候用
        public int InitTime = 10;  ///<初始化刷新时间
        //public int AdjustTime = 8;
        public int PumpTime = 5;  ///<抽真空的时间
        public int MaxChann = 2048; ///<最大的道数
        public string Name; ///<设备名称
        public Port port;          ///<!--接口-->

        public double usedTime;       ///<!--测量已用时间（s）-->
        public int pausTime = 30;
        //public bool IsThickOnLine = false;
        //public int ThickOnLineCalcFrequency = 1;
        public double Time1 = 0;
        public int[] Decre1;
        public bool HasRecord = false;
        public double Count1 = 0;

        private bool _isDropTime = true;
        public bool IsDropTime
        {
            get { return _isDropTime; }
            set
            {
                _isDropTime = value;
            }
        }

        //public int AdjustInterval;
        //public int AdjustTime = 5;
        public double UsedTime
        {
            get
            {
                if (state != DeviceState.Idel)
                {
                    return usedTime;
                }
                else
                {
                    return -1;
                }
            }
        }
        private DeviceState state;  ///<!--仪器状态-->
        public DeviceState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                if (value != DeviceState.Idel && connect != DeviceConnect.Connect)
                    connect = DeviceConnect.Connect;
            }
        }
        public int[] Data;

        public int[] DataCopy;

        public int[] tempData;
        public DeviceParameter DeviceParam;
        ///<!--测试条件-->
        public InitParameter InitParam;    ///<!--初始化条件-->
        public XRayTube RayTube;           ///<!--光管-->
        public Detector Detector;          ///<!--探测器-->
        public List<Filter> FilterList;      ///<!--滤光片-->
        public QuantaMotor FilterMotor;   ///<滤光片的电机
        public QuantaMotor CollimatMotor; ///<准直器的电机
        public QuantaMotor ChamberMotor;   ///<样品腔的电机
        public QuantaMotor TargetMotor;    ///靶材电机
        public VacuumPump Pump;  ///<真空泵
        public SpecEntity Spec;
        //public LibertyMotor zMotor; //Z轴电机

        public Shell ShellCover;
        public Lazer IndiaLazer;

        public static List<string> speVacuum = new List<string>() { "EDX3600" };//特殊类型的抽真空仪器

        //实时的峰通道
        public double MaxChannelRealTime = 0;

        private DeviceConnect _conn;
        public double m_lastCountRate;
        public CountingRateModes currentRateMode;

        public DeviceConnect connect
        {
            get { return _conn; }
            set
            {
                _conn = value;
            }
        }


        public MessageFormat format = new MessageFormat();

        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, short wParam, int lParam);
        [DllImport("User32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
        [DllImport("User32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int wMsg, short wParam, int lParam);

        public const int WM_USER = 0x400;
        public const int WM_ReceiveData = WM_USER + 201;
        public const int WM_EndTest = WM_USER + 202;
        public const int WM_EndInitial = WM_USER + 203;
        public const int WM_MoveStop = WM_USER + 204;
        public const int WM_DeviceDisConnect = WM_USER + 205;
        public const int Wm_Connection = WM_USER + 206;
        public const int Wm_PreheatOpenVoltage = WM_USER + 207;
        public const int Wm_PreheatOpenEnd = WM_USER + 208;
        public const int Wm_BlueTeethCaculate = WM_USER + 209;
        public const int Wm_BlueTeethPortError = WM_USER + 210;
        public const int Wm_ProcessInit = WM_USER + 211;
        public const int Wm_NoDemoData = WM_USER + 212;
        public const int Wm_StartTest = WM_USER + 213;
        public const int Wm_RefreshResult = WM_USER + 214;
        public const int Wm_StopTest = WM_USER + 215;
        public const int Wm_Graphic = WM_USER + 216;
        public const int Wm_DeviceError = WM_USER + 217;
        //public const int Wm_Cameral = WM_USER + 217;
        //public const int Wm_MainClient = WM_USER + 218;
        public const int Wm_GrphicHandle = WM_USER + 219;
        public const int Wm_SaveCount = WM_USER + 220;
        public const int Wm_OpenCover = WM_USER + 221;
        public const int Wm_PumpStart = WM_USER + 222;
        public const int Wm_PumpEnd = WM_USER + 223;
        public const int Wm_AutoAscendZ = WM_USER + 224;
        public const int Wm_NextInitializationElem = WM_USER + 225;
        public const int Wm_LoadAllCurve = WM_USER + 226;
        public const int Wm_InitAddCurrent = WM_USER + 227;
        public const int Wm_ContinueCalibrateElem = WM_USER + 228;
        public const int Wm_CalibrateElemFinish = WM_USER + 229;
        public const int Wm_TestMulti = WM_USER + 230;
        public const int Wm_SuspendTest = WM_USER + 231;

       
        public const int CUSTOM_MESSAGE = 0X400 + 2;//自定义消息 显示
        public const int CUSTOM_MESSAGE_HIDE = 0X400 + 3;//自定义消息 隐藏
        public int[] data0 = { };
        private bool stopFlag = false;

        public bool StopFlag
        {
            get { return stopFlag; }
            set
            {
                stopFlag = value;
                if (FilterMotor != null)
                    FilterMotor.stopFlag = value;
                if (CollimatMotor != null)
                    CollimatMotor.stopFlag = value;
                if (ChamberMotor != null)
                    ChamberMotor.stopFlag = value;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public bool PauseStop { get; set; }
        public Form mainForm;
        //public Device device;
        public object obj = new object();

        public FPGAParams FPGAparam;

        public double dp5UsedTime;

        public int MinCurrent;

        public int[] backData;

        public int MaxCurrent;

        public abstract void DoTest(object obj);

        public abstract void DoInitial(object obj);

        public abstract void PreOpenVoltage();

        /// <summary>
        /// 设备管流
        /// </summary>
        public abstract void SetOpenCurrent();

        public virtual string GetDppVersion() { return string.Empty; }

        public void RegisterHoeKeys()
        {
            HotKey.RegisterHotKey(OwnerHandle, 600, KeyModifiers.Ctrl, Keys.C);
            if (WorkCurveHelper.IsCarrayMatchPKSetting && !WorkCurveHelper.IsMatchAlways)
            {
                HotKey.RegisterHotKey(OwnerHandle, 601, KeyModifiers.Shift | KeyModifiers.Ctrl, Keys.P);
                HotKey.RegisterHotKey(OwnerHandle, 602, KeyModifiers.Shift | KeyModifiers.Ctrl, Keys.A);
            }
        }

        public void UnRegisterHotKeys()
        {
            HotKey.UnregisterHotKey(OwnerHandle, 600);
            if (WorkCurveHelper.IsCarrayMatchPKSetting && !WorkCurveHelper.IsMatchAlways)
            {
                HotKey.UnregisterHotKey(OwnerHandle, 601);
                HotKey.UnregisterHotKey(OwnerHandle, 602);
            }
        }

        public void OpenDevice()
        {

            //if (dp5Device != null)
            //{
            //    dp5Device.OpenDevice();
            //    dp5Device.LoadDP5Cfg(AppDomain.CurrentDomain.BaseDirectory + "\\dp5.cfg");
            //}
        }

        public void BeginSound()
        {
            if (WorkCurveHelper.DeviceCurrent.SysConfig != null && WorkCurveHelper.DeviceCurrent.SysConfig.IsTipSound)
            {
                if (System.IO.File.Exists(Application.StartupPath + @"/begin.wav"))
                {
                    System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/begin.wav");
                    sndPlayer.Play();
                }
            }
        }

        public void TestSound()
        {
            if (WorkCurveHelper.DeviceCurrent.SysConfig != null && WorkCurveHelper.DeviceCurrent.SysConfig.IsTipSound)
            {
                if (System.IO.File.Exists(Application.StartupPath + @"/test.wav"))
                {
                    System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/test.wav");
                    sndPlayer.Play();
                }
            }
        }

        public void StopSound()
        {
            if (WorkCurveHelper.DeviceCurrent.SysConfig != null && WorkCurveHelper.DeviceCurrent.SysConfig.IsTipSound)
            {
                if (System.IO.File.Exists(Application.StartupPath + @"/remind.wav"))
                {
                    System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/remind.wav");
                    sndPlayer.Play();
                }
            }
        }

        public void EndSound()
        {
            if (WorkCurveHelper.DeviceCurrent.SysConfig != null && WorkCurveHelper.DeviceCurrent.SysConfig.IsTipSound)
            {
                if (System.IO.File.Exists(Application.StartupPath + @"/finish.wav"))
                {
                    System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/finish.wav");
                    sndPlayer.Play();
                }
            }
        }

        private void BeijingTime()
        {
            while (true)
            {
                PreHeatusedTime++;
                Thread.Sleep(1000);
            }
        }

        public int PreHeatusedTime = 0;
        public virtual void PreHeatProcess()
        {


            if (WorkCurveHelper.DeviceCurrent == null) return;
            //--------dp5加密修改begin
            if (WorkCurveHelper.DeviceCurrent.ComType == ComType.USB
            && WorkCurveHelper.DeviceCurrent.IsDP5
            && port.GetDeviceKeyInfo() == 1)
            {
                port.LockHV(false);
                bool isInSevenDays = false;
                string surPlus = string.Empty;
                int type = (int)HardwareDog.SNConfirm(WorkCurveHelper.snFilePath, out isInSevenDays, out surPlus);
                if (type == -3 || type == -2)
                {
                    if (!MotorInstance.CheckDog()) port.LockHV(true);
                }

            }
            //--------dp5加密修改end

            if (WorkCurveHelper.IsAutoIncrease)
            {
                Thread t = new Thread(new ThreadStart(BeijingTime)); ;
                PreHeatusedTime = 0;
                if (WorkCurveHelper.IsAutoIncrease) { if (!t.IsAlive) t.Start(); }

                DateTime startTime = DateTime.Now;
                TimeSpan diffTime = new TimeSpan();

                while (PreHeatusedTime <= this.heatParams.PreHeatTime && !StopFlag)
                {
                    connect = DeviceConnect.Connect;
                    diffTime = DateTime.Now.Subtract(startTime);
                    Thread.Sleep(50);
                    if ((int)PreHeatusedTime % 300 == 0)
                        PreOpenVoltage();
                    PostMessage(OwnerHandle, Wm_PreheatOpenVoltage, true, (int)PreHeatusedTime);
                }

                if (WorkCurveHelper.IsAutoIncrease) t.Abort();
                this.format = new MessageFormat(Info.OpenVoltagePreHeatEnd, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                this.State = DeviceState.Idel;

                PostMessage(OwnerHandle, Wm_PreheatOpenEnd, true, (int)PreHeatusedTime);


            }
            else
            {
                DateTime startTime = DateTime.Now;
                TimeSpan diffTime = new TimeSpan();
                while (diffTime.TotalSeconds < this.heatParams.PreHeatTime && !StopFlag)
                {
                    connect = DeviceConnect.Connect;
                    diffTime = DateTime.Now.Subtract(startTime);
                    Thread.Sleep(50);
                    if ((int)diffTime.TotalSeconds % 300 == 0)
                        PreOpenVoltage();
                    PostMessage(OwnerHandle, Wm_PreheatOpenVoltage, true, (int)diffTime.TotalSeconds);
                }

                this.format = new MessageFormat(Info.OpenVoltagePreHeatEnd, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                this.State = DeviceState.Idel;

                PostMessage(OwnerHandle, Wm_PreheatOpenEnd, true, (int)diffTime.TotalSeconds);
            }




        }

        public void DoPreHeat(object obj)
        {
            DeviceState currentState = (DeviceState)obj;
            if (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
            {
                if (!IsConnectDevice())
                {
                    this.format = new MessageFormat(Info.ConnectionDevice, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(this.format);
                    PostMessage(OwnerHandle, WM_DeviceDisConnect, 0, (int)usedTime);
                    State = DeviceState.Idel;
                    connect = DeviceConnect.DisConnect;
                    StopFlag = true;
                    return;
                }
            }
            else if (!port.Connect())
            {
                this.format = new MessageFormat(Info.ConnectionDevice, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                PostMessage(OwnerHandle, WM_DeviceDisConnect, 0, (int)usedTime);
                State = DeviceState.Idel;
                connect = DeviceConnect.DisConnect;
                StopFlag = true;
                return;
            }
            if (ExistMagnet)
                Pump.Open();
            PreHeatProcess();
            CloseDevice();
            connect = DeviceConnect.DisConnect;
            //this.format = new MessageFormat(Info.OpenVoltagePreHeatEnd, 0);
            //WorkCurveHelper.specMessage.localMesage.Add(this.format);
            //this.State = DeviceState.Idel;
        }

        public virtual void PumpStart()
        {
            int pumpTime = 0;
            while (pumpTime < DeviceParam.VacuumTime && !StopFlag)
            {
                Thread.Sleep(1000);
                pumpTime++;
                if (WorkCurveHelper.PumpShowProgress)
                    PostMessage(OwnerHandle, Wm_PumpStart, true, pumpTime);
            }
            if (WorkCurveHelper.PumpShowProgress)
                PostMessage(OwnerHandle, Wm_PumpEnd, true, pumpTime);
        }

        public void InitPort()
        {
            if (WorkCurveHelper.type == InterfaceType.Demo)
            {
                port = new DemoPort();
                return;
            }
            if (WorkCurveHelper.type == InterfaceType.Usb)
            {
                port = new UsbPort();
                port.Dll = WorkCurveHelper.DeviceCurrent.PortType;
            }
            else if (WorkCurveHelper.type == InterfaceType.NetWork)
            {

                try
                {
                    COMThreadManager.SafeCall(() =>
                    {
                        port = new NetPort();
                        (port as NetPort).GetConnect -= new EventHandler(port_GetConnect);
                        (port as NetPort).GetConnect += new EventHandler(port_GetConnect);
                        (port as NetPort).DoRecordingToFile = WorkCurveHelper.WriteNetportLog;
                        port.strIp = WorkCurveHelper.DeviceCurrent.FPGAParams.IP;
                        port.uPort = 3333;
                        port.byteIp = Encoding.Default.GetBytes(port.strIp);
                    });

                    MotorOperator.StartPort();
                    MotorOperator.IsResetOk();

                    

                }
                catch (Exception e) { Msg.Show(e.Message); }
                //port.ConnectState = false;
            }
            else if (WorkCurveHelper.type == InterfaceType.Dp5)
            {
                port = new UsbPort();
                port.Dll = WorkCurveHelper.DeviceCurrent.PortType;
                switch (WorkCurveHelper.DeviceCurrent.Dp5Version)
                {
                    case Dp5Version.Dp5_FastNet:
                        dp5Device = new Dp5FastNetDevice();
                        if (!dp5Device.ConnectDevice(WorkCurveHelper.DeviceCurrent.FPGAParams.IP, "10001"))
                        {
                            //MessageFormat message = new MessageFormat(Info.NoDeviceConnect, 0);                 //修正dp5net无法打开软件的bug
                            //WorkCurveHelper.specMessage.localMesage.Add(message);                              //WorkCurveHelper.specMessage还未初始化，否则会报错
                        }
                        break;
                    case Dp5Version.Dp5_CommonUsb:
                    default:
                        dp5Device = new Dp5Device();
                        break;
                }
            }
            else if (WorkCurveHelper.type == InterfaceType.Parllel)
            {
                port = new ParallelPort();
            }
        }

        private bool GetReConnectCondition(int condition, int result)
        {
            switch (condition)
            {
                case 0:
                    return false;
                case 1:
                    return (result == -1 || result == -3);
                case 2:
                    return (result == -2 || result == -3);
                case 3:
                    return (result < 0);
                default:
                    return false;
            }
        }
        void port_GetConnect(object sender, EventArgs e)
        {

            //    Thread thread = new Thread(new ParameterizedThreadStart(DoInitialThings));
            //    thread.Priority = ThreadPriority.BelowNormal;
            //    thread.Start();


            //}


            //private void DoInitialThings(Object obj)
            //{
            int result = port.GetDeviceKeyInfo();
            if (result == 1)
            {
                if (!HardwareDog.IsHardWareDog)
                    MotorInstance.CheckDog(MotorInstance.UpdateKeyFile);
            }
            else if (result < 0)
            {
                if (!(port is NetPort))
                    throw new Exception("Only in netport can this method be called");
                if (GetReConnectCondition(WorkCurveHelper.ReConnectCondition, result))
                {
                    AddMsg(Info.NetDeviceDisConnection);
                    port.ConnectState = false;
                    WorkCurveHelper.UnderReconnecting = true;
                    // if (WorkCurveHelper.CurrentReconnectTime++ < WorkCurveHelper.RetryGetVersionTimes && (Port.ActionReconnect) != null)
                    if (WorkCurveHelper.CurrentReconnectTime++ < WorkCurveHelper.RetryGetVersionTimes)
                    {
                        AddMsg(Info.DeviceConnecting + "(" + WorkCurveHelper.CurrentReconnectTime + ")");
                        port.Connect();

                        //WorkCurveHelper.SafeCall(x, () =>
                        //{
                        //    port.Connect();
                        //});
                        //Port.ActionReconnect();

                    }
                    else
                    {
                        (port as NetPort).CheckWhetherDevInfoGot();
                    }
                    return;
                }
            }

            SetInfo();



            if (WorkCurveHelper.FunctionEnabled("LaserMode")
                && WorkCurveHelper.deviceMeasure.interfacce.ShellCover != null) //如果功能菜单中没有配置激光和保护罩功能, 则不用移动
            {
                if (!WorkCurveHelper.deviceMeasure.interfacce.ShellCover.Close())
                {
                    Msg.Show("防护罩没有移动到位");
                }
            }

            Skyray.EDXRFLibrary.SurfaceSourceLight ss = Skyray.EDXRFLibrary.SurfaceSourceLight.FindAll()[0];
            try
            {
                int v = 0;
                int c = 0;
                int t = 0;
                int va = 0;
                bool bb = false;
                port.GetParams(ref v, ref c, ref t, ref va, ref bb);
                uint vacuum = 0;
                Thread.Sleep(1000);
                port.SetSurfaceSource(ss.FirstLight, ss.SecondLight, ss.ThirdLight, ss.FourthLight);

                Thread.Sleep(1000);
                if (WorkCurveHelper.DeviceCurrent.HasVacuumPump)
                {
                    if (WorkCurveHelper.DeviceCurrent.VacuumPumpType == VacuumPumpType.VacuumSi)
                    {
                        port.GetVacuum(0, out vacuum);
                    }
                    else if (WorkCurveHelper.DeviceCurrent.VacuumPumpType == VacuumPumpType.Atmospheric)
                    {
                        port.GetVacuum(1, out vacuum);
                    }
                    else
                        port.GetVacuum(2, out vacuum);
                    ReturnVacuum = vacuum;
                    WorkCurveHelper.Volumngreen = ReturnVacuum;
                }
                WorkCurveHelper.DppMachineId = GetDppVersion();
                //暂时
                if (WorkCurveHelper.RegisterId != string.Empty)
                {
                    DateTime expDT = new DateTime();
                    bool _isSucess = NewLicense.License.Decrypt(WorkCurveHelper.RegisterId, WorkCurveHelper.DppMachineId.Replace(" ", "").ToUpper(), ref expDT);
                    int _leftDays = (expDT - DateTime.Now).Days;
                    if (_isSucess && _leftDays > 0)
                    {
                        WorkCurveHelper.IsDppValidate = true;
                        PostMessage(OwnerHandle, Wm_LoadAllCurve, true, 0);
                    }
                }
                Console.WriteLine(WorkCurveHelper.DppMachineId);
            }
            catch { }

        }
        private void SetInfo()
        {
            if (!(port is NetPort))
                throw new Exception("Only in netport can this method be called");
            port.ConnectState = true;
            WorkCurveHelper.IsDeviceInfoGot = true;
            WorkCurveHelper.CurrentReconnectTime = 0;
            WorkCurveHelper.UnderReconnecting = false;
            AddMsg(Info.NetDeviceConnection);
            if (Port.ActionAfterDeviceInfoChecked != null)
                Port.ActionAfterDeviceInfoChecked();


        }

        private void AddMsg(string msg)
        {
            WorkCurveHelper.specMessage.localMesage.Add(new MessageFormat(msg, 0));
        }

        public void Initialize(int deviceLength)
        {
            CountRating = 0;
            ExistMagnet = false;
            MaxChann = (int)deviceLength;
            backData = new int[(int)WorkCurveHelper.DeviceCurrent.SpecLength];
            Data = new int[MaxChann];
            DataCopy = new int[MaxChann];
            tempData = new int[MaxChann];
            Name = WorkCurveHelper.DeviceCurrent.Name;
            FPGAparam = WorkCurveHelper.DeviceCurrent.FPGAParams;
            RayTube = new XRayTube(WorkCurveHelper.DeviceCurrent, port);
            State = DeviceState.Idel;
            Pump = new VacuumPump(port);
            FilterMotor = new QuantaMotor(MotorName.OfFilter, port, obj, WorkCurveHelper.DeviceCurrent.FilterSpeed);
            CollimatMotor = new QuantaMotor(MotorName.OfCollimator, port, obj, WorkCurveHelper.DeviceCurrent.CollimatorSpeed);
            ChamberMotor = new QuantaMotor(MotorName.OfChamber, port, obj, WorkCurveHelper.DeviceCurrent.ChamberSpeed);
            TargetMotor = new QuantaMotor(MotorName.OfTarget, port, obj, WorkCurveHelper.DeviceCurrent.TargetSpeed);
            FilterMotor.OnMoveStop += DoMoveStop;
            CollimatMotor.OnMoveStop += DoMoveStop;
            ChamberMotor.OnMoveStop += DoMoveStop;
            TargetMotor.OnMoveStop += DoMoveStop;
            if (Ranges.RangeDictionary.ContainsKey("TubCurrent"))
            {
                RangeInfo rangeInfo = new RangeInfo();
                Ranges.RangeDictionary.TryGetValue("TubCurrent", out rangeInfo);
                MinCurrent = (int)rangeInfo.Min;
            }
            MaxCurrent = WorkCurveHelper.DeviceCurrent.MaxCurrent;
            //AdjustTime = WorkCurveHelper.AdjustTime;
            //AdjustInterval = WorkCurveHelper.AdjustInterval;

        }

        public virtual void InitDevice()
        {
            Initialize((int)WorkCurveHelper.DeviceCurrent.SpecLength);
            string deviceId = ReportTemplateHelper.LoadSpecifiedValue("LazerShell", "DeviceId");
            if (deviceId != null && deviceId.Equals(WorkCurveHelper.DeviceCurrent.Id.ToString()))
            {
                ReportTemplateHelper.SaveSpecifiedValueandCreate("LazerShell", "DeviceId", WorkCurveHelper.DeviceCurrent.Id.ToString());
                string Id = ReportTemplateHelper.LoadSpecifiedValue("LazerShell", "MotorId");
                string speed = ReportTemplateHelper.LoadSpecifiedValue("LazerShell", "Speed");
                try
                {
                    ShellCover = new Shell(port, int.Parse(Id), int.Parse(speed));
                }
                catch (Exception ex)
                { }
                string isManualString = ReportTemplateHelper.LoadSpecifiedValue("LazerShell", "LazerIsManual");
                if (isManualString != null)
                {
                    try
                    {
                        bool isManual = bool.Parse(isManualString);
                        IndiaLazer = new Lazer(port, isManual);
                    }
                    catch (Exception e)
                    { }
                }
            }
        }


        public bool IsConnectDevice()
        {
            if (port is NetPort)
            {
                int Desc = 0;
                return InternetGetConnectedState(Desc, 0);
            }
            else return true;
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(int Description, int ReservedValue);


        //关闭高压、真空泵等
        public void CloseDevice()
        {
            if (port != null)
            {

                if (WorkCurveHelper.IsUseElect && WorkCurveHelper.IsContinueVol)
                {
                    Pump.TClose();
                    port.CloseVoltageLamp();
                    return;
                }
                if (WorkCurveHelper.IsUseElect)
                {
                    Pump.TClose();
                }

                WorkCurveHelper.deviceMeasure.interfacce.IsDropTime = true;
                if (WorkCurveHelper.DeviceCurrent.ComType == ComType.USB && WorkCurveHelper.DeviceCurrent.PortType == DllType.DLL4 && WorkCurveHelper.DeviceCurrent.UsbVersion == UsbVersion.Usb2)
                {
                    port.CloseXRayTubHV();
                }
                if (WorkCurveHelper.DeviceCurrent.HasFilter
                    && WorkCurveHelper.IsCloseShutterAfterTest
                    && WorkCurveHelper.ResponseFilterIndex <= WorkCurveHelper.DeviceCurrent.Filter.Count)
                {
                    FilterMotor.OnMoveStopEnabled = false;
                    FilterMotor.stopFlag = false;
                    FilterMotor.MoveTo(WorkCurveHelper.ResponseFilterIndex);//滤光片归位会触发电机移动结束事件
                }
                else
                {
                    ////测量结束要关闭高压 0324
                    //port.CloseVoltage();
                    ////关闭高压指示灯
                    //port.CloseVoltageLamp();
                    RayTube.Close();
                }

                //state = DeviceState.Idel;
                ReturnCurrent = 0;
                ReturnVoltage = 0;
                #region 3200l
                //if (WorkCurveHelper.Is3200L)
                //{
                //    float range = 0;
                //    //range=计算真空规上下腔压差
                //    uint upvacuum = 0;
                //    uint downvacuum = 0;
                //    port.GetDoubleVacuum(0, out upvacuum, out downvacuum);
                //    range = Math.Abs(upvacuum - downvacuum);
                //    if (range > 3)//压差过大
                //    {

                //    }

                //    //int dir = WorkCurveHelper.DeviceCurrent.MotorY1Direct == 1 ? 0 : 1;
                //    //关光闸
                //    if (port.GetLightShutState(WorkCurveHelper.DeviceCurrent.MotorLightCode) != LightStatus.Close)
                //    {
                //        //port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, dir, 65535, true, 250 - WorkCurveHelper.DeviceCurrent.MotorY1Speed);
                //        //Thread.Sleep(2000);
                //        do
                //        {
                //            port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorLightCode, WorkCurveHelper.DeviceCurrent.MotorLightDirect, 65535, true, 250 - WorkCurveHelper.DeviceCurrent.MotorLightSpeed);
                //            Thread.Sleep(500);
                //        }
                //        while (port.GetLightShutState(WorkCurveHelper.DeviceCurrent.MotorLightCode) != LightStatus.Close);

                //        //电磁铁有效
                //        port.OpenElectromagnet();
                //    }
                //}
                #endregion
            }
            //如果真空泵打开则测试结束后关闭
            if (Pump.IsOpen || ExistMagnet)
            {
                Pump.Close();
            }
        }

        double exceptCount = 0;
        /// <summary>
        /// 计数率
        /// </summary>
        public virtual double CountRate()
        {
            double totalCount = 0;

            double temp = 0;
            for (int i = 0; i < Data.Length; i++)
            {
                totalCount += DataCopy[i];
            }
            if (usedTime - preUsedTime == WorkCurveHelper.AdjustExceptTime)
            {
                exceptCount = totalCount;
            }
            temp = totalCount;
            //TestTotalCount = (int)Math.Round(totalCount, MidpointRounding.AwayFromZero);
            TestTotalCount = totalCount;
            if (usedTime != 0)
            {
                if (currentRateMode == CountingRateModes.Average && usedTime - preUsedTime > 0)
                {
                    if (usedTime - preUsedTime > WorkCurveHelper.AdjustExceptTime)
                    {
                        totalCount = (totalCount - exceptCount) / (usedTime - preUsedTime - WorkCurveHelper.AdjustExceptTime);
                    }
                    else
                    {
                        totalCount = totalCount / (usedTime - preUsedTime);
                    }
                }
                else if (currentRateMode == CountingRateModes.RealTime)
                {
                    totalCount = totalCount - m_lastCountRate;
                }
                totalCount = Convert.ToDouble(totalCount.ToString("f2"));
            }
            else
            {
                totalCount = 0;
            }
            m_lastCountRate = temp;
            if (totalCount < 0)
                totalCount = 0;
            return totalCount;
        }

        /// <summary> 
        /// 把Data中的数据全部设为0；
        /// </summary>
        public virtual void ClearData()
        {
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = 0;
                DataCopy[i] = 0;
                tempData[i] = 0;
            }
            Array.Clear(backData, 0, backData.Length);
        }

        public virtual void StopMotor()
        {
            state = DeviceState.Idel;
            StopFlag = true;
            CloseDevice();

            if (FilterMotor != null && FilterMotor.IsMoving && !WorkCurveHelper.IsCloseShutterAfterTest)
            { FilterMotor.Index = -1; }
            if (CollimatMotor != null && CollimatMotor.IsMoving)
            { CollimatMotor.Index = -1; }
            if (ChamberMotor != null && ChamberMotor.IsMoving)
            { ChamberMotor.Index = -1; }
            if (TargetMotor != null && TargetMotor.IsMoving)
            { TargetMotor.Index = -1; }
            connect = DeviceConnect.DisConnect;

        }


        /// <summary>
        /// 上一次调整正常时间
        /// </summary>
        public double preUsedTime;

        /// <summary>
        /// 开始调节时间
        /// </summary>
        //public double startAdjustTime=0;

        /// <summary>
        /// 标志域，重新开始进行测试
        /// </summary>
        public int isAdjustSucceed = 0;

        public virtual int AdjustInitCountRate()
        {
            if (CountRating != 0)
            {
                int current = InitParam.TubCurrent;
                int tempCurrent = (int)Math.Round(current * (InitParam.MinRate + InitParam.MaxRate) / (CountRating * 2.0));
                if (WorkCurveHelper.AdjustType == 1)
                {
                    if (CountRating < InitParam.MinRate)
                    {
                        isAdjustSucceed = 1;
                        if (usedTime < WorkCurveHelper.AdjustInterval)
                        {
                            if (tempCurrent < (current + 2))
                            {
                                tempCurrent += 2;
                            }
                            current = tempCurrent;
                            if (tempCurrent > MaxCurrent)
                            {
                                InitParam.TubCurrent = MaxCurrent;
                                return 0;
                            }
                            else
                                InitParam.TubCurrent = tempCurrent;
                            preUsedTime = usedTime;
                            ClearData();
                            return 1;
                        }
                        else
                            return 0;
                    }
                    else if (CountRating > InitParam.MaxRate)
                    {
                        isAdjustSucceed = 1;
                        if (usedTime < WorkCurveHelper.AdjustInterval)
                        {
                            if (tempCurrent > (current - 2))
                            {
                                tempCurrent -= 2;
                            }
                            current = tempCurrent;
                            if (tempCurrent < MinCurrent)
                            {
                                InitParam.TubCurrent = MinCurrent;
                                return 0;
                            }
                            else
                                InitParam.TubCurrent = tempCurrent;
                            preUsedTime = usedTime;
                            ClearData();
                            return 1;
                        }
                        else
                            return 0;
                    }
                    else if (isAdjustSucceed == 1)
                    {
                        usedTime = 0;
                        preUsedTime = 0;
                        ClearData();
                        isAdjustSucceed = 2;
                        return 3;
                    }
                }
                else
                {
                    if (CountRating < InitParam.MinRate)
                    {
                        isAdjustSucceed = 1;
                        if (tempCurrent < (current + 2))
                        {
                            tempCurrent += 2;
                        }
                        if (tempCurrent > MaxCurrent)
                        {
                            InitParam.TubCurrent = MaxCurrent;
                            return 0;
                        }
                        else
                        {
                            InitParam.TubCurrent = tempCurrent;
                        }
                        usedTime = 0;
                        preUsedTime = 0;
                        ClearData();
                        return 1;
                    }
                    else if (CountRating > InitParam.MaxRate)
                    {
                        isAdjustSucceed = 1;
                        if (tempCurrent > (current - 2))
                        {
                            tempCurrent -= 2;
                        }
                        if (tempCurrent < MinCurrent)
                        {
                            InitParam.TubCurrent = MinCurrent;
                            return 0;
                        }
                        else
                        {
                            InitParam.TubCurrent = tempCurrent;
                        }
                        usedTime = 0;
                        preUsedTime = 0;
                        ClearData();
                        return 1;
                    }
                    else if (isAdjustSucceed == 1)
                    {
                        isAdjustSucceed = 2;
                        return 3;
                    }
                }
            }
            return 2;
        }

        /// <summary>
        /// 调节计数率 0为调节失败，1为调节成功，2为不做调节， 3为不需要调节
        /// </summary>
        public virtual int AdjustCountRate()
        {
            if (CountRating != 0)
            {
                int current = DeviceParam.TubCurrent;
                int tempCurrent = (int)Math.Round(current * (DeviceParam.MinRate + DeviceParam.MaxRate) / (CountRating * 2.0));
                if (WorkCurveHelper.AdjustType == 1)
                {
                    if (CountRating < DeviceParam.MinRate)
                    {
                        isAdjustSucceed = 1;
                        if (usedTime < WorkCurveHelper.AdjustInterval)
                        {
                            if (tempCurrent < (current + 2))
                            {
                                tempCurrent += 2;
                            }
                            current = tempCurrent;
                            if (tempCurrent > MaxCurrent)
                            {
                                DeviceParam.TubCurrent = MaxCurrent;
                                return 0;
                            }
                            else
                                DeviceParam.TubCurrent = tempCurrent;
                            preUsedTime = usedTime;
                            ClearData();
                            return 1;
                        }
                        else
                            return 0;
                    }
                    else if (CountRating > DeviceParam.MaxRate)
                    {
                        isAdjustSucceed = 1;
                        if (usedTime < WorkCurveHelper.AdjustInterval)
                        {
                            if (tempCurrent > (current - 2))
                            {
                                tempCurrent -= 2;
                            }
                            current = tempCurrent;
                            if (tempCurrent < MinCurrent)
                            {
                                DeviceParam.TubCurrent = MinCurrent;
                                return 0;
                            }
                            else
                                DeviceParam.TubCurrent = tempCurrent;
                            preUsedTime = usedTime;
                            ClearData();
                            return 1;
                        }
                        else
                            return 0;
                    }
                    else if (isAdjustSucceed == 1)
                    {
                        usedTime = 0;
                        preUsedTime = 0;
                        ClearData();
                        isAdjustSucceed = 2;
                        return 3;
                    }
                }
                else
                {
                    if (CountRating < DeviceParam.MinRate)
                    {
                        isAdjustSucceed = 1;
                        if (tempCurrent < (current + 2))
                        {
                            tempCurrent += 2;
                        }
                        if (tempCurrent > MaxCurrent)
                        {
                            DeviceParam.TubCurrent = MaxCurrent;
                            return 0;
                        }
                        else
                        {
                            DeviceParam.TubCurrent = tempCurrent;
                        }
                        usedTime = 0;
                        preUsedTime = 0;
                        ClearData();
                        return 1;
                    }
                    else if (CountRating > DeviceParam.MaxRate)
                    {
                        isAdjustSucceed = 1;
                        if (tempCurrent > (current - 2))
                        {
                            tempCurrent -= 2;
                        }
                        if (tempCurrent < MinCurrent)
                        {
                            DeviceParam.TubCurrent = MinCurrent;
                            return 0;
                        }
                        else
                        {
                            DeviceParam.TubCurrent = tempCurrent;
                        }
                        usedTime = 0;
                        preUsedTime = 0;
                        ClearData();
                        return 1;
                    }
                    else if (isAdjustSucceed == 1)
                    {
                        isAdjustSucceed = 2;
                        return 3;
                    }
                }
            }
            return 2;
        }



        /// 滤光片事件
        /// </summary>
        public void DoMoveStop(object sender, MotorMoveStopEvent e)
        {
            if (!FilterMotor.IsMoving && !CollimatMotor.IsMoving && !ChamberMotor.IsMoving && !TargetMotor.IsMoving && !stopFlag)
            {
                bool bSucceed = !FilterMotor.Exist || FilterMotor.Exist && FilterMotor.IsSucceed;
                bSucceed = bSucceed && (!CollimatMotor.Exist || CollimatMotor.Exist && CollimatMotor.IsSucceed);

                if (!ChamberMotor.IsSucceed)
                {
                    StopFlag = true;
                    CloseDevice();
                    Msg.Show(Info.ChamberIndex + Info.Error);
                }
                else if (!bSucceed)
                {
                    SendMessage(this.OwnerHandle, Wm_DeviceError, 0, (int)logErr.MotorMoveError);
                }
                else
                {
                    SendMessage(this.OwnerHandle, WM_MoveStop, 0, 0);
                }
            }
        }
        /// <summary>
        /// 发送结束命令使测试开始
        /// </summary>
        public void ImmediacyDotest()
        {
            SendMessage(this.OwnerHandle, WM_MoveStop, 0, 0);
        }

        /// <summary>
        /// 移动滤光片和准直器 DoMoveStopCollimat
        /// </summary>
        public virtual void MotorMove(params int[] ChamberIndex)
        {
            //if (CheckDog != null) CheckDog();
            if (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
            {
                if (!IsConnectDevice())
                {
                    SendMessage(this.OwnerHandle, WM_DeviceDisConnect, 0, 0);
                    State = DeviceState.Idel;
                    return;
                }
                RayTube.EnableCoverSwitch(WorkCurveHelper.DeviceCurrent.IsAllowOpenCover);
                double voltage = 0;
                double current = 0;
                int cover = 0;
                port.getParam(out voltage, out current, out cover);
                if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && (cover != 0))
                {
                    SendMessage(this.OwnerHandle, Wm_OpenCover, 0, 0);
                    State = DeviceState.Idel;
                    return;
                }
                RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
                RayTube.Open();
            }
            else
            {
                if (!port.Connect())
                {
                    SendMessage(this.OwnerHandle, WM_DeviceDisConnect, 0, 0);
                    State = DeviceState.Idel;
                    return;
                }
                GetReturnParams();
                if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && ReturnCoverClosed)
                {
                    SendMessage(this.OwnerHandle, Wm_OpenCover, 0, 0);
                    State = DeviceState.Idel;
                    return;
                }
                //--------dp5加密修改begin
                if (WorkCurveHelper.DeviceCurrent.ComType == ComType.USB
                && WorkCurveHelper.DeviceCurrent.IsDP5
                && port.GetDeviceKeyInfo() == 1)
                {
                    port.LockHV(false);
                    bool isInSevenDays = false;
                    string surPlus = string.Empty;
                    int type = (int)HardwareDog.SNConfirm(WorkCurveHelper.snFilePath, out isInSevenDays, out surPlus);
                    // if (type == -3 || type == -2)
                    if (!HardwareDog.IsHardWareDog)
                    {
                        if (!MotorInstance.CheckDog()) port.LockHV(true);
                    }

                }
                //--------dp5加密修改end
                if (!WorkCurveHelper.DeviceCurrent.HasTarget)
                {
                    RayTube.SetXRayTubeParams(DeviceParam.TubCurrent, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
                    RayTube.Open();
                }
            }
            if (!(Pump.Exist && (DeviceParam.IsVacuum || DeviceParam.IsVacuumDegree)))
                Pump.Close();
            connect = DeviceConnect.Connect;
            if (ExistMagnet)//电子锁
            {
                Pump.Open();
            }
            if (ChamberMotor.Exist && ChamberIndex.Length > 0)
            {
                ChamberMotor.IsMoving = true;
            }
            if (FilterMotor.Exist && DeviceParam.TargetMode != TargetMode.TwoTarget)
            {
                FilterMotor.IsMoving = true;
            }
            if (CollimatMotor.Exist)
            {
                CollimatMotor.IsMoving = true;
            }
            if (TargetMotor.Exist && WorkCurveHelper.DeviceCurrent.HasTarget && DeviceParam.TargetMode == TargetMode.TwoTarget)
            {
                TargetMotor.IsMoving = true;
            }
            if (WorkCurveHelper.DeviceCurrent.HasTarget && DeviceParam.TargetMode != TargetMode.TwoTarget)
            {
                port.CloseXRayTubHV();
            }
            if (ChamberMotor.Exist && ChamberIndex.Length > 0)
            {
                if (WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B")) ChamberMotor.Index = -1;
                ChamberMotor.MoveTo(ChamberIndex[0]);
            }
            if (FilterMotor.Exist && DeviceParam.TargetMode != TargetMode.TwoTarget)
            {
                FilterMotor.MoveTo(DeviceParam.FilterIdx);
            }
            if (CollimatMotor.Exist)
            {
                CollimatMotor.MoveTo(DeviceParam.CollimatorIdx);
            }
            if (TargetMotor.Exist && WorkCurveHelper.DeviceCurrent.HasTarget && DeviceParam.TargetMode == TargetMode.TwoTarget)
            {
                port.OpenXRayTubHV();
                TargetMotor.MoveTo(DeviceParam.TargetIdx);
            }
            if (!FilterMotor.Exist && !CollimatMotor.Exist && !TargetMotor.Exist && !ChamberMotor.Exist)
            {
               SendMessage(this.OwnerHandle, WM_MoveStop, 0, 0);
               
            }
        }

        public virtual void InitMotorMove()
        {
            //if (CheckDog != null) CheckDog();
            if (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
            {
                if (!IsConnectDevice())
                {
                    SendMessage(this.OwnerHandle, WM_DeviceDisConnect, 0, 0);
                    State = DeviceState.Idel;
                    return;
                }
                RayTube.EnableCoverSwitch(WorkCurveHelper.DeviceCurrent.IsAllowOpenCover);
                double voltage = 0;
                double current = 0;
                int cover = 0;
                port.getParam(out voltage, out current, out cover);
                if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && (cover != 0))
                {
                    SendMessage(this.OwnerHandle, Wm_OpenCover, 0, 0);
                    State = DeviceState.Idel;
                    return;
                }

                RayTube.SetXRayTubeParams(InitParam.TubCurrent / InitParam.CurrentRate, InitParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)InitParam.TargetMode);
                RayTube.Open();
            }
            else
            {
                if (!port.Connect())
                {
                    SendMessage(this.OwnerHandle, WM_DeviceDisConnect, 0, 0);
                    State = DeviceState.Idel;
                    return;
                }
                GetReturnParams();
                if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && ReturnCoverClosed)
                {
                    SendMessage(this.OwnerHandle, Wm_OpenCover, 0, 0);
                    State = DeviceState.Idel;
                    return;
                }
                //--------dp5加密修改begin
                if (WorkCurveHelper.DeviceCurrent.ComType == ComType.USB
                && WorkCurveHelper.DeviceCurrent.IsDP5
                && port.GetDeviceKeyInfo() == 1)
                {
                    port.LockHV(false);
                    bool isInSevenDays = false;
                    string surPlus = string.Empty;
                    int type = (int)HardwareDog.SNConfirm(WorkCurveHelper.snFilePath, out isInSevenDays, out surPlus);
                    if (!HardwareDog.IsHardWareDog)// if (type == -3 || type == -2)
                    {
                        if (!MotorInstance.CheckDog()) port.LockHV(true);
                    }

                }
                //--------dp5加密修改end
            }
            connect = DeviceConnect.Connect;
            if (ExistMagnet)//电子锁
            {
                Pump.Open();
            }
            if (FilterMotor.Exist && InitParam.TargetMode != TargetMode.TwoTarget)
            {
                FilterMotor.IsMoving = true;
            }
            if (CollimatMotor.Exist)
            {
                CollimatMotor.IsMoving = true;
            }
            if (TargetMotor.Exist && WorkCurveHelper.DeviceCurrent.HasTarget && InitParam.TargetMode == TargetMode.TwoTarget)
            {
                TargetMotor.IsMoving = true;
            }
            if (WorkCurveHelper.DeviceCurrent.HasTarget && InitParam.TargetMode != TargetMode.TwoTarget)
            {
                port.CloseXRayTubHV();
            }
            if (FilterMotor.Exist && InitParam.TargetMode != TargetMode.TwoTarget)
            {
                FilterMotor.MoveTo(InitParam.Filter);
            }
            if (CollimatMotor.Exist)
            {
                CollimatMotor.MoveTo(InitParam.Collimator);
            }
            if (TargetMotor.Exist && WorkCurveHelper.DeviceCurrent.HasTarget && InitParam.TargetMode == TargetMode.TwoTarget)
            {
                port.OpenXRayTubHV();
                TargetMotor.MoveTo(InitParam.Target);
            }
            if (!FilterMotor.Exist && !CollimatMotor.Exist && !TargetMotor.Exist)
            {
                SendMessage(this.OwnerHandle, WM_MoveStop, 0, 0);
            }
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="ChamberCellIndex">样品腔位置索引</param>
        //public void MotorMove(int ChamberCellIndex)
        //{
        //if (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
        //{
        //}
        //else
        //{
        //    if (!port.Connect())
        //    {
        //        SendMessage(this.OwnerHandle, WM_DeviceDisConnect, 0, 0);
        //        return;
        //    }
        //}
        //if (ChamberMotor.Exist)
        //{
        //    ChamberMotor.MoveTo(ChamberCellIndex);
        //}
        //MotorMove();
        //}

        /// <summary>
        /// 软件退出时关闭线程
        /// </summary>
        public void KillThread(bool bCloseAplication)
        {
            //port.Disconnect();
            //if (zMotor != null && zMotor.thread != null)
            //{
            //    zMotor.thread.Abort();
            //}
            if (ChamberMotor != null && ChamberMotor.thread != null)
            {
                ChamberMotor.thread.Abort();
                ChamberMotor.Index = 0;
            }
            if (FilterMotor != null && FilterMotor.thread != null)
            {
                FilterMotor.thread.Abort();
                FilterMotor.Index = 0;
            }
            if (CollimatMotor != null && CollimatMotor.thread != null)
            {
                CollimatMotor.thread.Abort();
                CollimatMotor.Index = 0;
            }
            if (TargetMotor != null && TargetMotor.thread != null)
            {
                TargetMotor.thread.Abort();
            }
            //if (WorkCurveHelper.TargetType == TargetType.SuperTwoTarget)
            //{
            //    this.port.CloseXRayTubHV();
            //}
            if (bCloseAplication)
            {
                if (this.port != null)
                    this.port.FreeLibrary();
            }
        }

        public void KillExe(string killname)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName == killname)
                {
                    item.Kill();
                }
            }

        }

        public void ExitCOMThread()
        {
            if (port is NetPort)
                COMThreadManager.ExitCOMThread();
        }

        /// <summary>
        /// 此函数用于判断某一外部进程是否打开
        /// </summary>
        /// <param name="processName">参数为进程名</param>
        /// <returns>如果打开了，就返回true，没打开，就返回false</returns>
        public bool IsProcessStarted(string processName)
        {
            Process[] temp = Process.GetProcessesByName(processName);
            if (temp.Length > 0) return true;
            else
                return false;
        }

        /// <summary>
        /// 返回第一个符合条件的样品室
        /// </summary>
        /// <param name="state">样品室状态</param>
        /// <returns></returns>
        public int GetCellIndex(ChamberCellState state)
        {
            if (CellStates == null)
            {
                return -1;
            }
            for (int i = 0; i < CellStates.Length; i++)
            {
                if (CellStates[i] == state)
                {
                    return i;
                }
            }
            return -1;
        }


        public virtual void GetReturnParams()
        {
            int intVolgate = 0, intCurrent = 0, iTemp = 0, iVacuum = 0;
            bool iCoverClose = false;
            if (port != null)
                port.GetParams(ref intVolgate, ref intCurrent, ref iTemp, ref iVacuum, ref iCoverClose);

            if (intVolgate < 0)
                intVolgate = 0;
            if (intCurrent < 0)
                intCurrent = 0;
            ReturnVoltage = intVolgate * 50 / 255 * WorkCurveHelper.DeviceCurrent.VoltageScaleFactor;
            ReturnCurrent = intCurrent * 1000 / 255d * WorkCurveHelper.DeviceCurrent.CurrentScaleFactor;
            Calculate(iVacuum);
            if (port != null && port.Dll == DllType.DLL3)
                iCoverClose = true;
            ReturnCoverClosed = !iCoverClose;

            if (State == DeviceState.Test && DeviceParam != null)
            {
                ReturnCurrent = ReturnCurrent * DeviceParam.CurrentRate;
            }
            else if (State == DeviceState.Init && InitParam != null)
            {
                ReturnCurrent = ReturnCurrent * InitParam.CurrentRate;
            }
            else
            {
                ReturnCurrent = 0;
            }
        }

        public string GetChamberIndex()
        {
            if (WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B") && numDev == 0)
            {
                int nsteps = 0;
                int nChambeIndex = -1;
                bool bTrueSteps = false;
                bool bInitialed = false;
                bool bBusy = false;
                if (port != null)
                    port.GetChamberStatus(ref nsteps, ref nChambeIndex, ref bTrueSteps, ref bInitialed, ref bBusy);
                if (nChambeIndex >= 0)
                {
                    ChamberMotor.Index = nChambeIndex;
                    // numDev++;
                }
                return ChamberMotor.Index.ToString();//+ " (" + bTrueSteps.ToString() + ")"
            }
            //else numDev++;
            // numDev = numDev % 5;
            return ChamberMotor.Index.ToString();
        }

        public bool bshowTemp;

        /// <summary>
        /// 计算温度或者真空度
        /// </summary>
        /// <param name="vacuum1">下位机返回的值</param>
        public void Calculate(double vacuum1)
        {
            if (bshowTemp)
            {
                if (vacuum1 > 219)//控制温度在0以上
                {
                    vacuum1 = 219;
                }
                if (vacuum1 < 34)//控制温度在60以下
                {
                    vacuum1 = 34;
                }
                double vacuum = vacuum1;
                ReturnTemp = -65.0 + Math.Sqrt(-4555.0 * Math.Log(vacuum / 550.0)) + 100.5 / (1.0 + Math.Exp(vacuum / 29.0)) - 13.0 * Math.Exp(-Math.Pow((vacuum - 24.0), 2.0) / (1.0 + 23.55 * vacuum)) +
                    3.03355 * (1.0 / (1.0 + 70.0 * Math.Pow((vacuum - 550.0), 2.0) / (700000.0 + 1.0 * vacuum * 1.0 / (1.0 + Math.Exp(vacuum - 530.0))))) + 2.1355 * Math.Exp(-Math.Pow((vacuum - 53.0), 2.0) / 100.0);
                //temp1 = 2.0;
                if (Double.IsInfinity(ReturnTemp))//应该没用了
                {
                    ReturnTemp = 0.0;
                }
                if (ReturnTemp > 60)
                {
                    ReturnTemp = 60;
                }
            }
            else
            {
                double vaccuum = vacuum1;
                double temp1 = 0.0;
                if ((int)WorkCurveHelper.DeviceCurrent.VacuumPumpType == 0)//计算大气压真空度
                {
                    temp1 = (255.0 - vaccuum) * 101.3 / (255.0 - 51.0);
                }
                else if ((int)WorkCurveHelper.DeviceCurrent.VacuumPumpType == 1)
                {
                    temp1 = 67.406 * Math.Exp(-5.0 * 0.8133 * vaccuum / 255.0);
                }
                else
                {
                    #region
                    if (vaccuum >= 240)
                    {
                        temp1 = 23;
                    }
                    else if (vaccuum >= 232)
                    {
                        temp1 = 24;
                    }
                    else if (vaccuum >= 223)
                    {
                        temp1 = 25.5;
                    }
                    else if (vaccuum >= 217)
                    {
                        temp1 = 27;
                    }
                    else if (vaccuum >= 212)
                    {
                        temp1 = 28;
                    }
                    else if (vaccuum >= 205)
                    {
                        temp1 = 29;
                    }
                    else if (vaccuum >= 199)
                    {
                        temp1 = 30;
                    }
                    else if (vaccuum >= 193)
                    {
                        temp1 = 32;
                    }
                    else if (vaccuum >= 185)
                    {
                        temp1 = 34;
                    }
                    else if (vaccuum >= 183)
                    {
                        temp1 = 36;
                    }
                    else if (vaccuum >= 178)
                    {
                        temp1 = 37;
                    }
                    else if (vaccuum >= 175)
                    {
                        temp1 = 38;
                    }
                    else if (vaccuum >= 171)
                    {
                        temp1 = 39;
                    }
                    else if (vaccuum >= 167)
                    {
                        temp1 = 40;
                    }
                    else if (vaccuum >= 162)
                    {
                        temp1 = 42;
                    }
                    else if (vaccuum >= 158)
                    {
                        temp1 = 44;
                    }
                    else if (vaccuum >= 153)
                    {
                        temp1 = 46;
                    }
                    else if (vaccuum >= 149)
                    {
                        temp1 = 48;
                    }
                    else if (vaccuum >= 143)
                    {
                        temp1 = 50;
                    }
                    else if (vaccuum >= 136)
                    {
                        temp1 = 53;
                    }
                    else if (vaccuum >= 133)
                    {
                        temp1 = 58;
                    }
                    else if (vaccuum >= 130)
                    {
                        temp1 = 60;
                    }
                    else if (vaccuum >= 128)
                    {
                        temp1 = 62.5;
                    }
                    else if (vaccuum >= 121)
                    {
                        temp1 = 64;
                    }
                    else if (vaccuum >= 118)
                    {
                        temp1 = 70;
                    }
                    else if (vaccuum >= 117)
                    {
                        temp1 = 73;
                    }
                    else if (vaccuum >= 114)
                    {
                        temp1 = 75;
                    }
                    else if (vaccuum >= 112)
                    {
                        temp1 = 78;
                    }
                    else if (vaccuum >= 111)
                    {
                        temp1 = 80;
                    }
                    else if (vaccuum >= 109)
                    {
                        temp1 = 82;
                    }
                    else if (vaccuum >= 107)
                    {
                        temp1 = 85;
                    }
                    else if (vaccuum >= 105)
                    {
                        temp1 = 87;
                    }
                    else if (vaccuum >= 104)
                    {
                        temp1 = 90;
                    }
                    else if (vaccuum >= 103)
                    {
                        temp1 = 92;
                    }
                    else if (vaccuum >= 102)
                    {
                        temp1 = 93;
                    }
                    else if (vaccuum >= 101)
                    {
                        temp1 = 95;
                    }
                    else if (vaccuum >= 100)
                    {
                        temp1 = 96;
                    }
                    else if (vaccuum >= 99)
                    {
                        temp1 = 98;
                    }
                    else if (vaccuum >= 98)
                    {
                        temp1 = 100;
                    }
                    else if (vaccuum >= 97)
                    {
                        temp1 = 104;
                    }
                    else if (vaccuum >= 96)
                    {
                        temp1 = 108;
                    }
                    else if (vaccuum >= 95)
                    {
                        temp1 = 112;
                    }
                    else if (vaccuum >= 93)
                    {
                        temp1 = 115;
                    }
                    else if (vaccuum >= 92)
                    {
                        temp1 = 120;
                    }
                    else if (vaccuum >= 91)
                    {
                        temp1 = 125;
                    }
                    else if (vaccuum >= 90)
                    {
                        temp1 = 130;
                    }
                    else if (vaccuum >= 88)
                    {
                        temp1 = 135;
                    }
                    else if (vaccuum >= 87)
                    {
                        temp1 = 140;
                    }
                    else if (vaccuum >= 86)
                    {
                        temp1 = 145;
                    }
                    else if (vaccuum >= 84)
                    {
                        temp1 = 150;
                    }
                    else if (vaccuum >= 83)
                    {
                        temp1 = 155;
                    }
                    else if (vaccuum >= 82)
                    {
                        temp1 = 160;
                    }
                    else if (vaccuum >= 80)
                    {
                        temp1 = 165;
                    }
                    else if (vaccuum >= 79)
                    {
                        temp1 = 170;
                    }
                    else if (vaccuum >= 78)
                    {
                        temp1 = 175;
                    }
                    else if (vaccuum >= 77)
                    {
                        temp1 = 180;
                    }
                    else if (vaccuum >= 75)
                    {
                        temp1 = 185;
                    }
                    else if (vaccuum >= 74)
                    {
                        temp1 = 190;
                    }
                    else if (vaccuum >= 73)
                    {
                        temp1 = 195;
                    }
                    else if (vaccuum >= 72)
                    {
                        temp1 = 200;
                    }
                    else if (vaccuum >= 71)
                    {
                        temp1 = 210;
                    }
                    else if (vaccuum >= 70)
                    {
                        temp1 = 220;
                    }
                    else if (vaccuum >= 69)
                    {
                        temp1 = 230;
                    }
                    else if (vaccuum >= 68)
                    {
                        temp1 = 240;
                    }
                    else if (vaccuum >= 67)
                    {
                        temp1 = 250;
                    }
                    else if (vaccuum >= 66)
                    {
                        temp1 = 260;
                    }
                    else if (vaccuum >= 65)
                    {
                        temp1 = 270;
                    }
                    else if (vaccuum >= 64)
                    {
                        temp1 = 280;
                    }
                    else if (vaccuum >= 63)
                    {
                        temp1 = 300;
                    }
                    else if (vaccuum >= 62)
                    {
                        temp1 = 320;
                    }
                    else if (vaccuum >= 61)
                    {
                        temp1 = 340;
                    }
                    else if (vaccuum >= 58)
                    {
                        temp1 = 360;
                    }
                    else
                    {
                        temp1 = 460;
                    }
                    #endregion
                }
                ReturnVacuum = temp1;
            }
        }

        /// <summary>
        /// 计算真空度的相关值  直接读取设备
        /// </summary>
        /// <param name="dblRet"></param>
        /// <returns></returns>
        //public double Vacuum
        //{
        //    get
        //    {
        //        Double dblRet = 0;
        //        int iVacuum = 0;
        //        iVacuum = port.returnVacuum();
        //        if (speVacuum.Contains(this.Name))
        //        {
        //            dblRet = 67.406 * Math.Exp(-5 * 0.8133 * iVacuum / 255);
        //        }
        //        else//仪器不为EDX3600时，使用真空元器件的线性真空值算法                        
        //        {
        //            dblRet = (255 - iVacuum) * 101.3 / (255 - 51);
        //        }
        //        return dblRet;
        //    }
        //}

        //public bool CoverClosed
        //{
        //    get
        //    {
        //        return port.coverClosed;
        //    }
        //}

        public float ReturnFineGain
        {
            get
            {
                if (this.InitParam != null)
                    return this.InitParam.FineGain;
                else
                    return 0f;

            }
        }

        public float ReturnGain
        {
            get
            {
                if (this.InitParam != null)
                    return this.InitParam.Gain;
                else
                    return 0f;

            }
        }

        public bool CalibrationPeakAuto(Atom atom, double energy, double channel, double fixChannel, ref int[] tempData)
        {
            double ChannelError = fixChannel - channel;
            if (Math.Abs(ChannelError) <= WorkCurveHelper.PeakErrorChannel && Math.Abs(ChannelError) >= 0.1)
            {
                int bChannel = DemarcateEnergyHelp.GetChannel(energy);
                if (QualeElement != null)
                {
                    bool goon = false;
                    int[] peakArr = SpecHelper.Find(Helper.ToDoubles(Data), QualeElement.ChannFWHM, QualeElement.WindowWidth, QualeElement.Trh1, QualeElement.ValleyDistance, QualeElement.AreaLimt);
                    for (int i = 0; i < peakArr.Length; i++)
                    {
                        if (Math.Abs(bChannel - peakArr[i]) <= WorkCurveHelper.PeakErrorChannel + 15)
                        {
                            goon = true;
                            break;
                        }
                    }
                    if (!goon)
                    {
                        return false;
                    }
                }

                //float[] fSpec = new float[tempData.Length];
                //Array.Copy(tempData, fSpec, tempData.Length);
                //Spec.spSpectrumCalibrate(fSpec, tempData.Length, 0f, (float)channel, 0f, (float)fixChannel);
                //tempData = Helper.ToInts(fSpec);
                CorrectSpec(channel, ChannelError, 0d, 0d, ref tempData);
                return true;
            }
            return false;
        }

        public void CorrectSpec(double ch1, double deltaCh1, double ch2, double deltaCh2, ref int[] tempData)
        {
            double g = (deltaCh2 - deltaCh1) * 1.0 / (ch2 - ch1);
            double z = (deltaCh1 * ch2 - ch1 * deltaCh2) * 1.0 / (ch2 - ch1);
            int LiI = 0, HiI = 0;
            double Li = 0, Hi = 0, LiF = 0, HiF = 0, fi = 0;

            double[] data = new double[tempData.Length];
            for (int i = 0; i < tempData.Length; ++i)
            {
                Li = z + (1 + g) * i; LiI = (int)Li; LiF = Li - LiI;
                Hi = z + (1 + g) * (1 + i); HiI = (int)Hi; HiF = Hi - HiI;
                if (Li > 0 && Hi > 0 && LiI < tempData.Length && HiI < tempData.Length)
                {
                    if (HiI - LiI > 1)
                    {
                        for (int j = LiI + 1; j <= HiI - 1; ++j)
                            fi += tempData[j];
                    }
                    else
                    {
                        if (HiI.Equals(LiI)) fi = -tempData[LiI];
                        else fi = 0;
                    }
                    data[i] = (1 - LiF) * tempData[LiI] + HiF * tempData[HiI] + fi;
                    fi = 0;
                }
            }
            int[] newdata = new int[tempData.Length];
            for (int i = 0; i < tempData.Length; ++i)
                newdata[i] = (int)(Math.Round(data[i], MidpointRounding.AwayFromZero));
            Array.Copy(newdata, tempData, tempData.Length);
        }

        public void AscendZAutoFunc()
        {
            port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, WorkCurveHelper.AscendStepZ, true, 250 - WorkCurveHelper.DeviceCurrent.MotorZSpeed);
            do
            {
                Thread.Sleep(1000);
                if (StopFlag)
                {
                    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, 0, true, 250 - WorkCurveHelper.DeviceCurrent.MotorZSpeed);
                    return;
                }
            } while (!port.MotorIsIdel(WorkCurveHelper.DeviceCurrent.MotorZCode));
            PostMessage(OwnerHandle, Wm_AutoAscendZ, true, 1);
        }
        public void AscendZAuto()
        {
            Thread thr = new Thread(new ThreadStart(AscendZAutoFunc));
            thr.Start();
        }
        public virtual void SetDp5Cfg()
        {
        }

        //public void SetLaserMode(bool isManual)
        //{
        //    if (port is NetPort)
        //        (port as NetPort).SetLaserMode(isManual);
        //}

        public bool IsTestButtonPressed()
        {
            if (port is NetPort)
            {
                return (port as NetPort).IsTestButtonPressed();
            }
            return false;
        }

        public bool ClearPressButtonInfo()
        {
            if (port is NetPort)
            {
                return (port as NetPort).ClearPressButtonInfo();
            }
            return false;
        }
    }
    public class Shell
    {
        const int STARTINDEX = 12;
        const int ENDINDEX = 13;
        const int Direction = 1;
        public int MotorCode { get; set; }
        public int Speed { get; set; }
        private Port _port;
        delegate bool SuccessDelegate();

        public Shell(Port port, int motorCode, int speed)
        {
            this._port = port;
            this.MotorCode = motorCode;
            this.Speed = speed;
        }

        private bool Open()
        {
            int count = 0;
            bool success = true;
            _port.MotorControl(MotorCode, Direction, 65535, true, Speed);
            if (_port is NetPort)
            {
                while (!(_port as NetPort).MotorAtTouch(STARTINDEX))
                {
                    Thread.Sleep(20);
                    count++;
                    if (count > 20)
                    {
                        success = false;
                        break;
                    }
                }

            }
            return success;
        }

        public bool Close()
        {
            int count = 0;
            bool success = true;
            _port.MotorControl(MotorCode, (Direction == 0) ? 1 : 0, 65535, true, Speed);
            if (_port is NetPort)
            {
                while (!(_port as NetPort).MotorAtTouch(ENDINDEX))
                {
                    Thread.Sleep(20);
                    count++;
                    if (count > 20)
                    {
                        success = false;
                        break;
                    }
                }
            }
            return success;
        }

        public bool OpenAsync()
        {
            SuccessDelegate sd = new SuccessDelegate(Open);
            IAsyncResult result = sd.BeginInvoke(null, null);
            return sd.EndInvoke(result);
        }

        public bool CloseAsync()
        {
            SuccessDelegate sd = new SuccessDelegate(Close);
            IAsyncResult result = sd.BeginInvoke(null, null);
            return sd.EndInvoke(result);
        }

    }

    public class Lazer
    {
        Port _port;
        public bool IsManual { get; set; }
        public Lazer(Port port, bool isManual)
        {
            _port = port;
            IsManual = isManual;
        }

        public void SetLaserMode(bool isManual)
        {
            if (_port is NetPort)
                (_port as NetPort).SetLaserMode(isManual);
        }

        public void Open()
        {
            _port.OpenHeightLaser(true);
        }

        public void Close()
        {
            _port.OpenHeightLaser(false);
        }
    }

    public enum logErr
    {
        DppError = 0,
        MotorMoveError = 1,
        AdjustRateError = 2
    }
}
