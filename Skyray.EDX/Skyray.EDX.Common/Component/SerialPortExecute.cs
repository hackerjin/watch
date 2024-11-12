using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 委托处理结果
    /// </summary>
    /// <param name="data"></param>
    public delegate void HandResult(object sender, SerialPortEvents e);

    /// <summary>
    /// 委托处理接收数据完成
    /// </summary>
    /// 
    /// <param name="data"></param>
    public delegate void OnReceivedData(object sender, SerialPortEvents e);

    /// <summary>
    /// 发送数据回调
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnSendData(object sender, SerialPortEvents e);

    public class SerialPortExecute
    {
        #region 成员变量
        /// <summary>
        /// 定义接收缓冲区数据
        /// </summary>
        private List<byte> m_buffer = null;

        delegate void AsyncCallResult(byte[] data);

        delegate void AsyncSend(byte[] data);


        public event HandResult SerialPortResult;

        public event OnReceivedData OnSerialPortReceived;

        public event OnSendData OnSeriaPortSend;
        bool m_isReceiving = false;

        /// <summary>
        /// 串口对象
        /// </summary>
        SerialPort m_serialPort = null;

        Thread m_threadMonitor = null;

        /// <summary>
        /// 结束符号
        /// </summary>
        char m_endChar = '\0';
        //char m_endChar = (char)0xFE;
        public char EndChar
        {
            get { return m_endChar; }
            set { m_endChar = value; }
        }

        /// <summary>
        /// 发送缓冲区大小
        /// </summary>
        int m_writeBufferSize = 1024;
        public int WriteBufferSize
        {
            get { return m_writeBufferSize; }
            set { m_writeBufferSize = value; }
        }

        /// <summary>
        /// 是否启用结束符
        /// </summary>
        bool m_useEndChar = false;
        public bool UseEndChar
        {
            get { return m_useEndChar; }
            set { m_useEndChar = value; }
        }


        /// <summary>
        /// 发送间隔时间
        /// </summary>
        int m_sendInterval = 100;

        public int SendInterval
        {
            get { return m_sendInterval; }
            set { m_sendInterval = value; }
        }

        /// <summary>
        /// 连续接收超时时间
        /// </summary>
        int m_receiveTimeout = 1000;

        public int ReceiveTimeout
        {
            get { return m_receiveTimeout; }
            set { m_receiveTimeout = value; }
        }

        /// <summary>
        /// 最后接收时间
        /// </summary>
        DateTime m_lastReceiveTime;

        public DateTime LastReceiveTime
        {
            get { return m_lastReceiveTime; }
        }

        /// <summary>
        /// 缓冲区接收大小
        /// </summary>
        private int m_bufferSize = 1;

        public int BufferSize
        {
            get { return m_bufferSize; }
            set { m_bufferSize = value; }
        }

        /// <summary>
        /// 串口名称
        /// </summary>
        private string m_portName = "COM1";

        public string PortName
        {
            get { return m_portName; }
            set { m_portName = value; }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        private int m_baudRate = 9600;

        public int BaudRate
        {
            get { return m_baudRate; }
            set { m_baudRate = value; }
        }

        /// <summary>
        /// 停止位长度
        /// </summary>
        private StopBits m_stopBits = StopBits.None;

        public StopBits StopBits
        {
            get { return m_stopBits; }
            set { m_stopBits = value; }
        }

        /// <summary>
        /// 基偶校验位
        /// </summary>
        private Parity m_parity = Parity.None;

        public Parity Parity
        {
            get { return m_parity; }
            set { m_parity = value; }
        }

        /// <summary>
        /// 控制协议
        /// </summary>
        private Handshake m_handshake = Handshake.None;

        public Handshake Handshake
        {
            get { return m_handshake; }
            set { m_handshake = value; }
        }

        /// <summary>
        /// 标准数据长度
        /// </summary>
        private int m_dataBits = 8;

        public int DataBits
        {
            get { return m_dataBits; }
            set { m_dataBits = value; }
        }

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private int m_readBufferSize = 4;

        public int ReadBufferSize
        {
            get { return m_readBufferSize; }
            set { m_readBufferSize = value; }
        }

        /// <summary>
        /// 输入缓冲区中的字节数
        /// </summary>
        private int m_receivedBytesThreshold = 1;

        public int ReceivedBytesThreshold
        {
            get { return m_receivedBytesThreshold; }
            set { m_receivedBytesThreshold = value; }
        }

        /// <summary>
        /// 获取当前是否打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return m_serialPort == null ? false : m_serialPort.IsOpen;
            }
        }
        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        public SerialPortExecute(string portName, int baudRate)
        {
            m_portName = portName;
            m_baudRate = baudRate;
            m_buffer = new List<byte>();
            m_serialPort = new SerialPort(portName, baudRate);
            m_serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
        }


        #region 开启关闭串口
        /// <summary>
        /// 开始接收
        /// </summary>
        public bool Start()
        {
            try
            {
                m_serialPort.Parity = m_parity;
                m_serialPort.StopBits = m_stopBits;
                m_serialPort.Handshake = m_handshake;
                m_serialPort.DataBits = m_dataBits;
                m_serialPort.ReadBufferSize = m_readBufferSize;
                m_serialPort.ReceivedBytesThreshold = m_receivedBytesThreshold;
                m_serialPort.WriteBufferSize = m_writeBufferSize;
                m_serialPort.Open();
                //m_threadMonitor = new Thread(new ThreadStart(SerialPortMonitor));
                //m_threadMonitor.IsBackground = true;
                //m_threadMonitor.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
          //  m_threadMonitor.Abort();
            m_serialPort.Close();
            m_buffer.Clear();
            GC.Collect();
        }
        #endregion


        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SerialPort_DataReceived(object sender, EventArgs e)
        {
            StartReceive();
            Thread.Sleep(200);
            if (!m_serialPort.IsOpen) return;
            int availCount = m_serialPort.BytesToRead;
            //Console.WriteLine("到达字节+" + availCount);
            byte[] bs = new byte[availCount];
            bool bEnd = false;
            for (int i = 0; i < availCount; i++)
            {
                //单个字节读取
                byte b = (byte)m_serialPort.ReadByte();
                if (m_useEndChar && b == m_endChar)
                {
                    //结束接收
                    bEnd = true;
                    break;
                }
                bs[i] = b;
            }
            lock (m_buffer)
                m_buffer.AddRange(bs);
            EndReceive(bs);
            if (bEnd)
                CallEndResult();
        }


        /// <summary>
        /// 开始接收
        /// </summary>
        void StartReceive()
        {
            if (!m_isReceiving)
                m_isReceiving = true;
        }


        /// <summary>
        /// 结束接收
        /// </summary>
        void EndReceive(byte[] bs)
        {
            //设置最后接收时间
            m_lastReceiveTime = DateTime.Now;
            m_isReceiving = false;
            if (OnSerialPortReceived != null
                && bs != null
                && bs.Length != 0)
            {
                AsyncCallResult call = new AsyncCallResult(CalReceived);
                call.BeginInvoke(bs, null, null);
            }
        }


        void CalReceived(byte[] data)
        {
            OnSerialPortReceived(this, new SerialPortEvents(data));
        }



        /// <summary>
        /// 读取结束监控
        /// </summary>
        void SerialPortMonitor()
        {


            while (IsOpen)
            {
                if (!m_isReceiving)
                {
                    if (DateTime.Now.Subtract(m_lastReceiveTime).TotalMilliseconds > m_receiveTimeout && m_buffer.Count != 0)
                    {
                        CallEndResult();
                    }
                }
                Thread.Sleep(100);
            }
        }


        /// <summary>
        /// 处理接收数据完成
        /// </summary>
        void CallEndResult()
        {
            byte[] result = CopyBuffer(true);
            if (result != null && result.Length > 0)
            {
                if (SerialPortResult != null)
                {
                    //异步通知接收完成
                    AsyncCallResult call = new AsyncCallResult(CalResult);
                    call.BeginInvoke(result, null, null);
                }
            }
        }

        void CalResult(byte[] data)
        {
           
        }

        byte[] CopyBuffer(bool isClean)
        {
            byte[] bs = null;
            lock (m_buffer)
            {
                bs = new byte[m_buffer.Count];
                m_buffer.CopyTo(bs);
                if (isClean)
                    m_buffer.Clear();
            }
            return bs;
        }
        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bs"></param>
        public void Send(byte[] bs)
        {
            if (!m_serialPort.IsOpen)
            {
                throw new SerialPortException("串口未连接！");
            }
            //异步发送
            AsyncSend call = new AsyncSend(DoAsyncSend);
            call.BeginInvoke(bs, null, null);
        }



        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="data"></param>
        void DoAsyncSend(byte[] data)
        {
            int s = 0;

            if (data == null || data.Length == 0)
                return;
            if (m_useEndChar)
            {
                Array.Resize<byte>(ref data, data.Length + 1);
                data[data.Length - 1] = BitConverter.GetBytes(m_endChar)[0];
            }
            if (data.Length > m_writeBufferSize)
            {
                //分包发送
                int sendCount = 0;
                while (sendCount < data.Length)
                {
                    byte[] sendBytes;
                    if (sendCount + m_writeBufferSize > data.Length)
                    {
                        sendBytes = new byte[data.Length - sendCount];
                        Array.Copy(data, sendCount == 0 ? 0 : sendCount - 1, sendBytes, 0, sendBytes.Length);
                        sendCount = data.Length;
                    }
                    else
                    {
                        sendBytes = new byte[m_writeBufferSize];
                        Array.Copy(data, sendCount == 0 ? 0 : sendCount - 1, sendBytes, 0, sendBytes.Length);
                        sendCount += m_writeBufferSize;
                    }
                    //发送数据
                    m_serialPort.Write(sendBytes, 0, sendBytes.Length);
                    s += sendBytes.Length;

                    Array.Resize<byte>(ref sendBytes, 0);
                    Thread.Sleep(m_sendInterval);
                }

            }
            else
            {
                //一次发送
                m_serialPort.Write(data, 0, data.Length);
            }

            try
            {
                if (OnSeriaPortSend != null)
                    OnSeriaPortSend(this, new SerialPortEvents(data));
                data = null;
            }
            finally
            {
                data = null;
            }
        

        }
        #endregion

    }
}
