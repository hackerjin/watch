using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.Component;
using System.Windows.Forms;

namespace Skyray.EDX.Common
{

    /// <summary>
    /// 样品室状态
    /// </summary>
    public enum ChamberCellState
    {
        Invalid,
        Waitting,
        Testing,
        Finish
    }




    /// <summary>
    /// 计数率的计算模式
    /// </summary>
    public enum CountingRateModes
    {
        /// <summary>
        /// 计算平均计数率
        /// </summary>
        Average,
        /// <summary>
        /// 计算实时计数率
        /// </summary>
        RealTime
    }

    /// <summary>
    /// 测量模式
    /// </summary>
    public enum OptMode
    {
        /// <summary>
        /// 测量
        /// </summary>
        Test,
        /// <summary>
        /// 初始化
        /// </summary>
        Initalize,
        /// <summary>
        /// 能量刻度
        /// </summary>
        Demarcate,
        /// <summary>
        /// 匹配
        /// </summary>
        Matching,
        /// <summary>
        /// 智能模式
        /// </summary>
        Explore,

        /// <summary>
        /// 标样
        /// </summary>
        StandSample,

        SpecialOnlySave,

        RohsMatching,

        /// <summary>
        /// 多点测量模式
        /// </summary>
        ProgrammableTest,

        /// <summary>
        /// 未知只存，不进行计算
        /// </summary>
        UnknownSave,

        PureSample,

        /// <summary>
        /// 预热
        /// </summary>
        PreHeat,

        /// <summary>
        /// 测量分辨率
        /// </summary>
        Resolve,

        CurveCalibrate,

        BaseCalibrate,   //基材校正

        Detection,

        CalFPGAIntercept

    }

    public enum Demarcate
    {
        None,
        Test,
        Initialize
    }
   
    /// <summary>
    /// 仪器状态
    /// </summary>
    public enum DeviceState
    {
        Idel, Pause, Test, Init, PreHeat, Resume, Motoring, Break
    }

    public enum DeviceConnect
    {
        DisConnect,Connect
    }

    public enum InterfaceType
    {
        Usb,
        NetWork,
        Dp5,
        BlueTeeth,
        Parllel,
        Demo
    }


    public enum CoverState
    { 
        None,
        OpenCover,
        CloseCover

    }

    //public enum TargetType
    //{
    //    SuperOneTarget = 0,
    //    SuperTwoTarget = 1
    //}

    public enum MachineType
    {
        Super1050 = 1050,
        Super2050 = 2050,
        Super2400 = 2400
    }

    /// <summary>
    /// 设备测试事件
    /// </summary>
    public class DeviceEvent : EventArgs
    {
        public int UsedTime;
        public bool InitSucceed;
        public DeviceEvent(int usedTime)
        {
            UsedTime = usedTime;
            InitSucceed = true;
        }
        public DeviceEvent(bool initSucceed)
        {
            InitSucceed = initSucceed;
        }
    }

    /// <summary>
    /// 设备类
    /// </summary>
    public class DeviceMeasure
    {
        public DeviceInterface interfacce;

        private object theLock = new object();

        public Thread deviceThread;

        public void CreateInitalize()
        {
            ContructDeviceType();
            if (WorkCurveHelper.deviceMeasure != null
             && WorkCurveHelper.deviceMeasure.interfacce != null
             && WorkCurveHelper.deviceMeasure.interfacce.dp5Device != null
             && WorkCurveHelper.deviceMeasure.interfacce.dp5Device.IsConnected())
            {
                WorkCurveHelper.deviceMeasure.interfacce.dp5Device.DisConnectDevice();
            }
            if (interfacce != null && interfacce.port != null) interfacce.port.FreeLibrary(); 
            interfacce = MotorInstance.CreateStoneInstance(WorkCurveHelper.type);
            interfacce.InitPort();
            interfacce.InitDevice();
        }

        /// <summary>
        /// 初始化设备类型
        /// </summary>
        private void ContructDeviceType()
        {
            if (WorkCurveHelper.DeviceCurrent == null)
                return;
            if (WorkCurveHelper.DemoInstance)
                WorkCurveHelper.type = InterfaceType.Demo;
            else if (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
                WorkCurveHelper.type = InterfaceType.NetWork;
            else if (WorkCurveHelper.DeviceCurrent.ComType == ComType.BlueTooth)
                WorkCurveHelper.type = InterfaceType.BlueTeeth;
            else if (WorkCurveHelper.DeviceCurrent.ComType == ComType.Parallel)
                WorkCurveHelper.type = InterfaceType.Parllel;
            else if (WorkCurveHelper.DeviceCurrent.IsDP5)
                WorkCurveHelper.type = InterfaceType.Dp5;
            else
                WorkCurveHelper.type = InterfaceType.Usb;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Test()
        {
            //Thread.Sleep(500);
            //if (interfacce.State != DeviceState.Idel)
            //    return;
            lock (theLock)
            {
                if (interfacce != null && !interfacce.StopFlag)
                {
                  
                    interfacce.State = DeviceState.Test;
                    MessageFormat format = new MessageFormat(Info.SpectrumTest, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                    deviceThread = new Thread(new ParameterizedThreadStart(interfacce.DoTest));
                    deviceThread.Priority = ThreadPriority.Highest;
                    //deviceThread.IsBackground = true;
                    deviceThread.Start(interfacce.State);
                }
            }
        }

        /// <summary>
        /// 开始预热
        /// </summary>
        public void StartPreHeat()
        {
            //if (interfacce.State != DeviceState.Idel)
            //    return;
            lock (theLock)
            {
                if (interfacce != null && !interfacce.StopFlag)
                {
                    interfacce.usedTime = 0;
                    interfacce.State = DeviceState.PreHeat;
                    MessageFormat format = new MessageFormat(Info.OpenVoltagePreHeat, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                    deviceThread = new Thread(new ParameterizedThreadStart(interfacce.DoPreHeat));
                    deviceThread.Priority = ThreadPriority.Highest;
                    //deviceThread.IsBackground = true;
                    deviceThread.Start(interfacce.State);
                }
            }
        }

        
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            //if (this.interfacce.IndiaLazer != null && WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && interfacce.IndiaLazer.IsManual)
            //{
            //    this.interfacce.IndiaLazer.Open();
            //}
            //if (interfacce.ShellCover != null)
            //{
            //    if (!this.interfacce.ShellCover.CloseAsync())
            //    {
            //        Msg.Show("防护罩没有移动到位");
            //        return;
            //    }
            //}
            interfacce.StopMotor();
            //MessageFormat format = new MessageFormat((WorkCurveHelper.sTestStopInfo == string.Empty) ? Info.SpectrumStop : WorkCurveHelper.sTestStopInfo, 0);
            MessageFormat format = new MessageFormat(Info.SpectrumStop,0);
            WorkCurveHelper.specMessage.localMesage.Add(format);
            
            //WorkCurveHelper.sTestStopInfo = string.Empty;//周大福客户需要以北京时间为基准进行扫描，如果系统走时时间等于条件中的时间，则停止测试，显示的形式为测试完毕
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            //if (interfacce.State != DeviceState.Idel)
            //{
            //    return;
            //}
            lock (theLock)
            {
                if (interfacce != null && !interfacce.StopFlag)
                {
                    interfacce.State = DeviceState.Init;
                    MessageFormat format = new MessageFormat(Info.SpectrumInitialize, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                    deviceThread = new Thread(new ParameterizedThreadStart(interfacce.DoInitial));
                    deviceThread.Priority = ThreadPriority.Highest;
                    //deviceThread.IsBackground = true;
                    deviceThread.Start(interfacce.State);
                }
            }
        }
    }

}
