using System;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;


namespace Skyray.EDX.Common.Component
{
    /// <summary>
    /// 串口链路层，实现转义收发，
    /// </summary>
    public class CommInterface
    {
        #region 构造函数
        /// <summary>
        /// 默认实例构造器
        /// </summary>
        public CommInterface()
        {
            m_sendQueue = new ThreadSafeQueue(10);
            m_port = new SerialPort();
        }
        #endregion

        #region 开启链路
        /// <summary>
        /// 开启链路
        /// </summary>
        /// <param name="portName">端口号</param>
        /// <param name="baudRate">波特率</param>
        public void StartUp(string portName, int baudRate)
        {
            ShutDown();                 // 如果已经打开，先关闭
            m_port.PortName = portName;
            m_port.BaudRate = baudRate;

            //add by chengyangpin at 2007-09-01
            try
            {
                m_port.Open();
            }
            catch
            {
                if (OnNoPortError != null)
                {
                    OnNoPortError();
                    return;
                }
                else
                {
                    throw new Exception("Can not find the port!");
                }
            }

            m_sendQueue.Clear();          // 清空发送队列
            m_bExit = false;
            m_sendThread = new Thread(new ThreadStart(SendFunc));
            m_sendThread.IsBackground = true;
            m_sendThread.Start();         // 启动发送线程

            m_receiveThread = new Thread(new ThreadStart(ReceiveFunc));
            m_receiveThread.IsBackground = true;
            m_receiveThread.Start();
        }
        #endregion

        #region 关闭链路
        /// <summary>
        /// 关闭链路
        /// </summary>
        public void ShutDown()
        {
            // 等待发送线程、接收线程结束
            m_bExit = true;

            if (IsStartUp)
            {
                m_receiveThread.Join(100);
                m_sendThread.Join(100);

                try//add by cyp 20081111
                {
                    m_port.Close();
                }
                catch
                {
                    CatchSendRecvEx();
                    return;
                    throw new InvalidOperationException("Close error.");
                }
            }

        }
        #endregion

        #region 将数据放入发送队列
        /// <summary>
        /// 将数据放入发送队列
        /// </summary>
        /// <param name="buf"></param>
        public void Write(Byte [] buf)
        {
            if (buf.Length != 0)
            {
                m_sendQueue.Enqueue(buf);
            }
        }
        #endregion

        /// <summary>
        /// 抛出Send和Recv的异常(cyp20081027)
        /// </summary>
        private void CatchSendRecvEx()
        {
            m_bExit = true;
            if (OnCutPowerError != null)
            {
                OnCutPowerError();
                return;
            }
            else
            {
                throw new Exception("Device power error.");
            }
        }

        #region 发送线程
        /// <summary>
        /// 发送线程
        /// </summary>
        private void SendFunc()
        {
            Byte[] EOF_BUF = new Byte[1]{EOF};
            Byte[] ESC_EOF_BUF = new Byte[2]{ESC, ESC_EOF};
            Byte[] ESC_ESC_BUF = new Byte[2]{ESC, ESC_ESC};
            Byte code;

            while (!m_bExit)
            {
                if (m_sendQueue.Count > 0)
                {
                    try
                    {
                        Byte[] buf = m_sendQueue.Dequeue();//从上挪进来 cyp 20081027

                        // SOF
                        m_port.Write(EOF_BUF, 0, EOF_BUF.Length);

                        // 数据
                        for (int i = 0; i < buf.Length; i++ )
                        {
                            code = (Byte)buf.GetValue(i);

                            switch (code)
                            {
                                case EOF:
                                    m_port.Write(ESC_EOF_BUF, 0, ESC_EOF_BUF.Length);
                                    break;
                                case ESC:
                                    m_port.Write(ESC_ESC_BUF, 0, ESC_ESC_BUF.Length);
                                    break;
                                default:
                                    m_port.Write(buf, i, 1);
                                    break;
                            }
                        }

                        // EOF
                        m_port.Write(EOF_BUF, 0, EOF_BUF.Length);
                    }
                    catch
                    {
                        CatchSendRecvEx();
                    }
                    finally
                    {

                    }


                }
                else
                {
                    Thread.Sleep(50);
                }
            }

            //Debug.WriteLine("SendThread exited!");
        }
        #endregion

        #region 接收线程
        /// <summary>
        /// 接收线程
        /// </summary>
        private void ReceiveFunc()
        {
            bool bIsEsc = false;                                // 上一个字符是转义字符
            Byte[] receiveBuf = new Byte[COMM_MAX_PACKET_SIZE]; // 接收缓存
            int nIndex = 0;                                     // 接收缓存游标

            while (!m_bExit)
            {
                try
                {
                    if (m_port.BytesToRead > 0)//cyp：BytesToRead容易抛出异常
                    {
                        Byte code = (Byte)m_port.ReadByte();
                        switch (code)
                        {
                            case EOF:
                                if ((nIndex > 0) && (callBack != null))
                                {
                                    Byte [] dataBuf = new Byte[nIndex];
                                    Array.Copy(receiveBuf, dataBuf, nIndex);
                                    if (callBack != null)
                                        callBack(dataBuf);
                                }
                                bIsEsc = false;
		                        nIndex = 0;
                                break;

                            case ESC:
		                        bIsEsc = true;
    	                        break;

                            default:
		                        if (bIsEsc == true)	// 上一个字符为转义字符
		                        {
			                        if (code == ESC_EOF)		// ESC_EOF
			                        {
				                        receiveBuf[nIndex] = EOF;
				                        if (nIndex<COMM_MAX_PACKET_SIZE) nIndex++;
			                        }
			                        else// if (temp_data == COMM_ESC_ESC)	// ESC_ESC
			                        {
				                        receiveBuf[nIndex] = ESC;
				                        if (nIndex<COMM_MAX_PACKET_SIZE) nIndex++;
                                    }

                                    bIsEsc = false;
		                        }
		                        else
		                        {
				                    receiveBuf[nIndex] = code;
				                    if (nIndex<COMM_MAX_PACKET_SIZE) nIndex++;
		                        }

                                break;
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }                 
                }
                catch
                {
                    CatchSendRecvEx();
                }          
            }

            //Debug.WriteLine("ReceiveThread exited!");
        }
        #endregion

        #region 链路是否已经建立
        /// <summary>
        /// 链路是否已经建立
        /// </summary>
        public bool IsStartUp
        {
            get
            {
                return m_port.IsOpen;
            }
        }
        #endregion

        private const Byte EOF = 0xFE;                      // 帧头帧尾
        private const Byte ESC = 0xAA;                      // 转义字符
        private const Byte ESC_EOF = 0xC0;
        private const Byte ESC_ESC = 0x75;

        private const int COMM_MAX_PACKET_SIZE = 4200;      // 数据包最大容量

        private SerialPort m_port;                          // 串口

        /// <summary>
        /// 委托类，收到一帧数据时触发
        /// </summary>
        /// <param name="buf"></param>
        public delegate void ReceiveCallback(Byte[] buf);
        /// <summary>
        /// 委托对象，收到一帧数据时触发
        /// </summary>
        public ReceiveCallback callBack;

        ThreadSafeQueue m_sendQueue;                        // 发送队列
        Thread m_sendThread;                                // 发送线程

        Thread m_receiveThread;                             // 接收线程

        private bool m_bExit;                               // 串口关闭标记

        //add by chengyangpin at 2007-09-01
        /// <summary>
        /// 委托类：连接找不到端口发生异常
        /// </summary>
        public delegate void NoPortErrorCallback();
        /// <summary>
        /// 委托对象：连接找不到端口发生异常
        /// </summary>
        public NoPortErrorCallback OnNoPortError = null;
        /// <summary>
        /// 委托类：连接时断电异常
        /// </summary>
        public delegate void CutPowerErrorCallBack();
        /// <summary>
        /// 委托对象：连接时断电异常
        /// </summary>
        public CutPowerErrorCallBack OnCutPowerError = null;

    }

}