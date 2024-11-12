using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;


namespace Skyray.EDX.Common.Component
{
    ///// <summary>
    ///// 设备类型
    ///// </summary>
    //public enum DeviceType
    //{
    //    PocketIII,  //手持3代
    //    PortableI   //便携1代
    //}


    /// <summary>
    /// 串行设备
    /// </summary>
    public class SerialDevice
    {
        #region 需要用到的函数
        #region 打开设备
        /// <summary>
        /// 打开设备
        /// </summary>
        public void Open()
        {
            //Debug.WriteLine("***Device.Open");
            //add by chengyangpin at 2007-09-01
            //连接时检测不到端口
            CommTransport.OnNoPortError = NoPortError;
            //连接中设备断电
            CommTransport.OnCutPowerError = CutPowerError;


            // 连接设备
            CommTransport.Connect(m_strPortName, m_nBaudRate);

            // 将命令解析函数设为收到数据后的委托调用函数
            CommTransport.OnDataReceive = CmdDataParse;


            // 连续发生校验错误时调用连接异常
            CommTransport.OnDataError = ConnectError;

            // 开启状态查询线程
            m_bExit = false;
            m_threadStatesCheck = new Thread(StatesCheckFunc);
            m_threadStatesCheck.IsBackground = true;
            m_threadStatesCheck.Start();
        }
        #endregion

        #region 关闭设备
        /// <summary>
        /// 关闭设备
        /// </summary>
        public void Close()
        {
            //Debug.WriteLine("***Device.Close");

            // 关闭状态查询线程
            m_bExit = true;
            if (m_threadStatesCheck != null)
            {
                m_threadStatesCheck.Join(100);
            }

            // 断开连接
            CommTransport.DisConnect();
        }
        #endregion

        #region  复位设备
        /// <summary>
        ///  复位设备
        /// </summary>
        public void ResetDevice()
        {
            //Debug.WriteLine("***Device.ResetDevice");

            Byte cmdID = GetCmdID();
            Byte[] cmd = new Byte[6];

            // 填充命令详情
            cmd[0] = cmdID;
            cmd[1] = CMD_RESET;
            if (MAX_CURRENT == 100)
            {
                cmd[2] = (Byte)(m_nCurrent * 255 / 125);
            }
            else if (MAX_CURRENT == 50)
            {
                cmd[2] = (Byte)(m_nCurrent * 255 * 2 / 125);
            }
            cmd[3] = (Byte)(m_nVoltage * 255 / 50);
            cmd[4] = (Byte)(m_nGain);
            cmd[5] = (Byte)(m_nFineGain);

            // 发送命令
            CommTransport.Talk(cmd);

            // 已测时间清零
            m_nLeftTime = 0;
        }
        #endregion

        #region 开启射线
        /// <summary>
        /// 开启射线
        /// </summary>
        public void XRayOn()
        {
            //Debug.WriteLine("***Device.XRayOn");

            Byte cmdID = GetCmdID();
            Byte[] cmd = new Byte[3];

            // 填充命令详情
            cmd[0] = cmdID;
            cmd[1] = CMD_XRAY;
            cmd[2] = CMD_XRAY_ON;

            // 发送命令
            CommTransport.Talk(cmd);
        }
        #endregion

        #region 关闭射线
        /// <summary>
        /// 关闭射线
        /// </summary>
        public void XRayOff()
        {
            //Debug.WriteLine("***Device.XRayOff");

            Byte cmdID = GetCmdID();
            Byte[] cmd = new Byte[3];

            // 填充命令详情
            cmd[0] = cmdID;
            cmd[1] = CMD_XRAY;
            cmd[2] = CMD_XRAY_OFF;

            // 发送命令
            CommTransport.Talk(cmd);
        }
        #endregion

        #region  让设备工作nSeconds秒，数据保存在缓存中
        /// <summary>
        ///  让设备工作nSeconds秒，数据保存在缓存中
        /// </summary>
        /// <param name="nSeconds"></param>
        public void Work(int nSeconds)
        {
            //Debug.WriteLine("***Device.Work");

            // 测量时间只能在 0 到 255之间
            if ((nSeconds < Byte.MinValue) || (nSeconds > Byte.MaxValue))
                throw new ArgumentOutOfRangeException("nSeconds", "Work time must between 0 to 255");

            Byte cmdID = GetCmdID();
            Byte[] cmd = new Byte[3];

            // 填充命令详情
            cmd[0] = cmdID;
            cmd[1] = CMD_WORK;
            cmd[2] = Convert.ToByte(nSeconds);

            // 发送命令
            CommTransport.Talk(cmd);
        }
        #endregion

        #region 读取设备缓存内数据
        /// <summary>
        ///  读取设备缓存内数据
        /// </summary>
        public void ReadData()
        {
            //Debug.WriteLine("***Device.ReadData");

            Byte cmdID = GetCmdID();
            Byte[] cmd = new Byte[2];

            // 填充命令详情
            cmd[0] = cmdID;
            cmd[1] = CMD_READ;

            // 发送命令
            CommTransport.Talk(cmd);
        }
        #endregion

        #region 暂停
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            //Debug.WriteLine("***Device.Pause");

            Byte cmdID = GetCmdID();
            Byte[] cmd = new Byte[2];

            // 填充命令详情
            cmd[0] = cmdID;
            cmd[1] = CMD_PAUSE;

            // 发送命令
            CommTransport.Talk(cmd);
        }
        #endregion

        #region 继续测量
        /// <summary>
        /// 继续测量
        /// </summary>
        public void Continue()
        {
            //Debug.WriteLine("***Device.Continue");

            Byte cmdID = GetCmdID();
            Byte[] cmd = new Byte[2];

            // 填充命令详情
            cmd[0] = cmdID;
            cmd[1] = CMD_CONTINUE;

            // 发送命令
            CommTransport.Talk(cmd);
        }
        #endregion

        #region 移动电机(准直器、滤光片)

        //手持3代两个电机的移动方向
        private int Motor0Direction;
        private int Motor1Direction;
        //便携1代电机的移动方向
        private int Direction;

        /// <summary>
        /// 移动电机
        /// </summary>
        /// <param name="deviceType">设备类型</param>
        /// <param name="direction">电机移动方向</param>
        /// <param name="step">电机移动步数</param>
        public void MoveMotor(Pocket deviceType, int direction, int step)
        {
            if (deviceType == Pocket.PocketIII)//手持式3代
            {
                Motor0Direction = direction;//变量变为电机0的方向：滤光片
                Motor1Direction = step;//变量变为电机1的方向：准直器

                //电机忙
                while (Motor0Busy || Motor1Busy)
                {
                    Thread.Sleep(50);
                }
                //电机没移好
                if (!Motor0Ready || !Motor1Ready)
                {
                    Byte cmdID = GetCmdID();
                    Byte[] cmd = new Byte[4];

                    // 填充命令详情
                    cmd[0] = cmdID;
                    cmd[1] = CMD_MOTOR;
                    cmd[2] = (Byte)Motor0Direction;
                    cmd[3] = (Byte)Motor1Direction;
                    CommTransport.Talk(cmd);
                }
            }
            else if (deviceType == Pocket.PortableI)//便携式1代
            {
                Direction = direction;
                do
                {
                    Thread.Sleep(1000);
                } while (MotorBusy);//电机忙

                if (!MotorTouchSwitch)//电机没在开关位，先复位
                {
                    Byte cmdID = GetCmdID();
                    int resetStep = 8000;
                    Byte[] cmd = new Byte[5];
                    cmd[0] = cmdID;
                    cmd[1] = CMD_MOTOR;
                    cmd[2] = (Byte)(1 - direction);
                    cmd[3] = (Byte)((resetStep >> 8) & 0xFF);
                    cmd[4] = (Byte)(resetStep & 0xFF);
                    CommTransport.Talk(cmd);
                }

                do
                {
                    Thread.Sleep(2200);
                } while (MotorBusy);//电机忙

                if (MotorTouchSwitch)//电机在开关位
                {
                    Byte cmdID = GetCmdID();
                    Byte[] cmd = new Byte[5];
                    cmd[0] = cmdID;
                    cmd[1] = CMD_MOTOR;
                    cmd[2] = (Byte)direction;
                    cmd[3] = (Byte)((step >> 8) & 0xFF);
                    cmd[4] = (Byte)(step & 0xFF);
                    CommTransport.Talk(cmd);
                }
            }
        }
        /// <summary>
        /// 便携电机在开关处
        /// </summary>
        private bool MotorTouchSwitch
        {
            get
            {
                return (MotorState % 4 == 2 || MotorState % 4 == 3);
            }
        }
        /// <summary>
        /// 便携电机忙
        /// </summary>
        public bool MotorBusy
        {
            get
            {
                return (MotorState % 2 == 1);
            }
        }
        /// <summary>
        /// 手持3代0号电机忙
        /// </summary>
        public bool Motor0Busy
        {
            get
            {
                return (MotorState % 16 == 1);
            }
        }

        /// <summary>
        /// 手持3代1号电机忙
        /// </summary>
        public bool Motor1Busy
        {
            get
            {
                return (MotorState / 16 == 1);
            }
        }

        /// <summary>
        /// 手持3代0号电机位置正确
        /// </summary>
        public bool Motor0Ready
        {
            get
            {
                //0号电机
                return (((Motor0Direction == 0 && MotorState % 16 == 4) || //0号左开关
                    (Motor0Direction == 1 && MotorState % 16 == 2)) &&     //0号右开关
                    MotorState % 16 != 1);                                 //不忙
            }
        }

        /// <summary>
        /// 手持3代1号电机位置正确
        /// </summary>
        public bool Motor1Ready
        {
            get
            {
                //1号电机
                return (((Motor1Direction == 0 && MotorState / 64 == 1) || //1号左开关
                    (Motor1Direction == 1 && MotorState / 32 == 1)) &&     //1号右开关
                    MotorState / 16 != 1);                                 //不忙
            }
        }
        #endregion

        #endregion

        #region 需要用到的属性
        #region 端口号
        /// <summary>
        /// 端口号
        /// </summary>
        public string PortName
        {
            set
            {
                m_strPortName = value;
            }
            get
            {
                return m_strPortName;
            }
        }
        #endregion

        #region 波特率
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            set
            {
                m_nBaudRate = value;
            }
            get
            {
                return m_nBaudRate;
            }
        }
        #endregion

        #region 高压
        /// <summary>
        /// 高压
        /// </summary>
        public int Voltage
        {
            set
            {
                if ((value < MIN_VOLTAGE) || (value > MAX_VOLTAGE))
                {
                    throw new ArgumentOutOfRangeException("value", "Voltage must between 10 to 40");
                }

                m_nVoltage = value;
            }
            get
            {
                return m_nVoltage;
            }
        }
        #endregion

        #region 管流
        /// <summary>
        /// 管流
        /// </summary>
        public int Current
        {
            set
            {
                if ((value < MIN_CURRENT) || (value > MAX_CURRENT))
                {
                    throw new ArgumentOutOfRangeException("value", "Current must between 0 to 100");
                } 
                
                m_nCurrent = value;
            }
            get
            {
                return m_nCurrent;
            }
        }
        #endregion

        #region 放大倍数粗调
        /// <summary>
        /// 放大倍数粗调
        /// </summary>
        public int Gain
        {
            set
            {
                if ((value < MIN_GAIN) || (value > MAX_GAIN))
                {
                    throw new ArgumentOutOfRangeException("value", "Gain must between 0 to 255");
                } 
                
                m_nGain = value;
            }
            get
            {
                return m_nGain;
            }
        }
        #endregion

        #region 放大倍数精调
        /// <summary>
        /// 放大倍数精调
        /// </summary>
        public int FineGain
        {
            set
            {
                if ((value < MIN_GAIN) || (value > MAX_GAIN))
                {
                    throw new ArgumentOutOfRangeException("value", "FineGain must between 0 to 255");
                } 
                
                m_nFineGain = value;
            }
            get
            {
                return m_nFineGain;
            }
        }
        #endregion

        #region 高压回读值
        /// <summary>
        /// 高压回读值
        /// </summary>
        public int VoltageMon
        {
            get
            {
                return m_nVoltageMonitor;
            }
        }
        #endregion

        #region 管流回读值
        /// <summary>
        /// 管流回读值
        /// </summary>
        public int CurrentMon
        {
            get
            {
                 return m_nCurrentMonitor;
            }
        }
        #endregion

        #region 扳机是否触发
        /// <summary>
        /// 扳机是否触发
        /// </summary>
        public bool IsSwitchOn
        {
            get
            {
                return m_IsSwitchOn;
            }
        }
        #endregion

        #region 下位机状态
        /// <summary>
        /// 下位机状态
        /// </summary>
        public int DeviceState
        {
            get
            {
                return m_nState;
            }
        }
        #endregion      

        #region 下位机已经测量的时间
        /// <summary>
        /// 下位机已经测量的时间
        /// </summary>
        public int LeftTime
        {
            get
            {
                return m_nLeftTime;
            }
        }
        #endregion
        #endregion

        #region 私有成员
        #region 下位机状态查询线程
        /// <summary>
        /// 下位机状态查询线程
        /// </summary>
        private void StatesCheckFunc()
        {
            while (!m_bExit)
            {
                GetStates();

                Thread.Sleep(200);
            }

            //Debug.WriteLine("StatesCheckThread exited!");
        }
        #endregion

        #region 发送检查状态指令
        /// <summary>
        /// 发送检查状态指令
        /// </summary>
        private void GetStates()
        {
            // 空闲时间
            if (CommTransport.WaitingDataCount <= NO_MORE_GET_STATE)
            {
                Byte cmdID = GetCmdID();
                Byte[] cmd = new Byte[2];

                // 填充命令详情
                cmd[0] = cmdID;
                cmd[1] = CMD_GET_STATE;

                // 发送命令
                CommTransport.Talk(cmd);
            }
       }
        #endregion

        #region 命令解析
        /// <summary>
        /// 命令解析
        /// </summary>
        /// <param name="buf">收到的命令</param>
        private void CmdDataParse(Byte[] buf)
        {
            bool SwitchState;
            int NewLeftTime;
            switch (buf [0])
            {
                case CMD_READ:
                    data.Initialize();
                    for (int i = 0; i < MAX_CHANNEL; i++)
                    {
                        data[i] = buf[2 * i + 1] * 256 + buf[2 * i + 2];
                    }
                   
                    if (OnDataReceived != null)
                        OnDataReceived(data);
                    break;
                case CMD_GET_STATE:
                    ////Debug.WriteLine("CmdDataParse: CMD_GET_STATE");
                    m_nState = buf[1];
                    NewLeftTime = buf[2];
                    SwitchState = (buf[3] == 1);
                    MotorState = buf[4];
                    //m_nCurrentMonitor = buf[5];

                    if (LeftTime != NewLeftTime)
                    {
                        m_nLeftTime = NewLeftTime;
                        //OnTimeChanged();
                    }

                    if (m_IsSwitchOn != SwitchState)
                    {
                        m_IsSwitchOn = SwitchState;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 获取一个命令ID
        /// <summary>
        ///  获取一个变化的非零命令ID，避免下位机重复执行同一命令，下位机复位后初始化上一命令ID为零
        /// </summary>
        /// <returns>命令ID</returns>
        private Byte GetCmdID()
        {
            if (m_byCmdID == Byte.MaxValue)
            {
                m_byCmdID = 1;
            }
            else
            {
                m_byCmdID++;
            }

            return m_byCmdID;
        }
        #endregion

        #region 设备连接异常
        void ConnectError()
        {
            if (OnConnectError != null)
                OnConnectError();
        }
        #endregion

        //add by chengyangpin at 2007-09-01
        #region 连接时检测不到端口异常
        void NoPortError()
        {
            if (OnNoPortError != null)
                OnNoPortError();
        }
        #endregion

        #region 连接中设备断电异常
        void CutPowerError()
        {
            if (OnCutPowerError != null)
                OnCutPowerError();
        }
        #endregion

        private string m_strPortName = "COM6";      // 端口号
        private const int MAX_CHANNEL = 2048;
        private int m_nBaudRate = 19200;            // 波特率

        private const int MIN_VOLTAGE = 5;         // 最小的高压值
        private const int MAX_VOLTAGE = 40;         // 最大的高压值

        private const int MIN_CURRENT = 0;          // 最小的管流值
        public static int MAX_CURRENT = 100;         // 最大的管流值

        private const int MIN_GAIN = 0;             // 最小的放大倍数
        private const int MAX_GAIN = 255;           // 最大的放大倍数

        private const int NO_MORE_GET_STATE = 1;    // 现有待传数据超过此阈值时不再查询状态

        private Thread m_threadStatesCheck;         // 状态查询线程
        private bool m_bExit;                       // 状态查询线程关闭标记

        private int m_nVoltage = MIN_VOLTAGE;       // 高压
        private int m_nCurrent = MIN_CURRENT;       // 管流
        private int m_nGain = MIN_GAIN;             // 放大倍数粗调
        private int m_nFineGain = MIN_GAIN;         // 放大倍数精调

        private int m_nVoltageMonitor=0;              // 高压回读值
        private int m_nCurrentMonitor=0;              // 管流回读值

        private Byte m_byCmdID = 0;                 // 命令ID，下位机用来判断是否为重发的同一条指令

        private const Byte CMD_XRAY = 0x00;               // 射线控制
        private const Byte CMD_WORK = 0x01;               // 工作命令
        private const Byte CMD_READ = 0x02;               // 读内存命令
        private const Byte CMD_GET_STATE = 0x03;          // 查询状态
        private const Byte CMD_RESET = 0x04;              // 复位设备
        private const Byte CMD_PAUSE = 0x05;              // 暂停设备
        private const Byte CMD_CONTINUE = 0x06;           // 继续测量
        private const Byte CMD_MOTOR = 0x07;              // 电机命令//cyp20080926

        private const Byte CMD_XRAY_ON = 0x00;            // 开射线
        private const Byte CMD_XRAY_OFF = 0x01;           // 开射线

        private bool m_IsSwitchOn = false;                // 扳机状态
        private int MotorState;                           // 电机状态//cyp20080926

        private int m_nState = DEVICE_STOP;               // 下位机状态

        private int m_nLeftTime = 0;                      // 下位机已经测量的时间
        private int[] data = new int[MAX_CHANNEL];        // 暂存谱数据

        public string Name;
        #endregion

        /// <summary>
        /// 设备停止标志位
        /// </summary>
        public const int DEVICE_STOP = 0;
        /// <summary>
        /// 设备准备完毕标志位
        /// </summary>
        public const int DEVICE_READY = 1;
        /// <summary>
        /// 设备正在运行标志位
        /// </summary>
        public const int DEVICE_RUN = 2;
        /// <summary>
        /// 设备暂停标志位
        /// </summary>
        public const int DEVICE_PAUSE = 3;

        #region 需要用到的委托
        /// <summary>
        /// 扳机状态改变委托类
        /// </summary>
        public delegate void SwitchStateChanged();
        /// <summary>
        /// 扳机状态改变委托对象
        /// </summary>
        public SwitchStateChanged OnSwtChanged = null;
        /// <summary>
        /// 已测时间改变委托类
        /// </summary>
        public delegate void MeasuredTimeChanged();
        /// <summary>
        /// 已测时间改变委托对象
        /// </summary>
        public MeasuredTimeChanged OnTimeChanged = null;
        /// <summary>
        /// 收到谱数据委托类
        /// </summary>
        /// <param name="data">谱数据</param>
        public delegate void DataReceived(int[] data);
        /// <summary>
        /// 收到谱数据委托对象
        /// </summary>
        public DataReceived OnDataReceived = null;
        /// <summary>
        /// 连接发生异常委托类，通常是连接后没有信号了
        /// </summary>
        public delegate void ConnectErrorCallback();
        /// <summary>
        /// 连接发生异常委托对象
        /// </summary>
        public ConnectErrorCallback OnConnectError = null;


        /*** add by chengyangpin at 2007-09-01 ***/

        /// <summary>
        /// 连接时检测不到端口委托类
        /// </summary>
        public delegate void NoPortErrorCallback();
        /// <summary>
        /// 连接时检测不到端口委托对象
        /// </summary>
        public NoPortErrorCallback OnNoPortError = null;
        /// <summary>
        /// 连接时断电异常委托类
        /// </summary>
        public delegate void CutPowerErrorCallBack();
        /// <summary>
        /// 连接时断电异常委托对象
        /// </summary>
        public CutPowerErrorCallBack OnCutPowerError = null;

        #endregion

    }
}
