using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Skyray.EDX.Common.Component
{
    static class CommTransport
    {
        #region 连接
        /// <summary>
        /// 连接
        /// </summary>
        public static void Connect(string portName, int baudRate)
        {
            // 如果已经连接，则先关闭
            if (IsConnect)
            {
                DisConnect();
                Thread.Sleep(200);
            }

            // 清除原有数据
            Clear(true);

            // 创建连接
            m_commInterface.callBack = new CommInterface.ReceiveCallback(DataParse);

            //add by chengyangpin at 2007-09-01
            m_commInterface.OnNoPortError = NoPortError;
            m_commInterface.OnCutPowerError = CutPowerError;
            try
            {
                m_commInterface.StartUp(portName, baudRate);
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
                    throw new Exception("No port.");
                }
            }

            // 创建后台传输线程
            m_bExit = false;

            m_transportThread = new Thread(TransportFunc);

            m_transportThread.IsBackground = true;
            m_transportThread.Start();
        }
        #endregion

        //add by chengyangpin at 2007-09-01
        #region 连接时检测不到端口异常
        static void NoPortError()
        {
            if (OnNoPortError != null)
                OnNoPortError();
        }
        #endregion

        #region 连接中设备断电异常
        static void CutPowerError()
        {
            if (OnCutPowerError != null)
                OnCutPowerError();
        }
        #endregion

        #region 是否已经建立连接
        /// <summary>
        /// 是否已经建立连接
        /// </summary>
        public static bool IsConnect
        {
            get
            {
                return m_commInterface.IsStartUp;
            }
        }
        #endregion

        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        public static void DisConnect()
        {
            // 关闭传输线程
            m_bExit = true;

            if (m_transportThread != null)
            {
                m_transportThread.Join(100);
            }

            // 关闭连接
            if (m_commInterface != null)
            {
                m_commInterface.ShutDown();
            }
        }
        #endregion

        #region Dispose
        public static void Dispose()
        {
            DisConnect();
        }
        #endregion

        #region 是否还有数据未传输
        /// <summary>
        /// 是否还有数据未传输
        /// </summary>
        public static bool IsBusy
        {
            get
            {
                return (m_talkQueue.Count>0);
            }
        }
        #endregion

        #region 开始一次传输
        /// <summary>
        /// 开始一次传输，一问一答，如连续MAX_ERROR次错误，则调用OnDataError委托
        /// </summary>
        /// <param name="buf">
        /// 问题
        /// </param>
        public static void Talk(Byte[] buf)
        {
            m_talkQueue.Enqueue(buf);
        }
        #endregion

        #region 现有待传数据
        /// <summary>
        /// 现有待传数据
        /// </summary>
        public static int WaitingDataCount
        {
            get
            {
                return m_talkQueue.Count;
            }
        }
        #endregion

        #region 传输线程函数
        /// <summary>
        /// 传输线程函数
        /// </summary>
        private static void TransportFunc()
        {
            while (!m_bExit)
            {
                Thread.Sleep(50);

                if (m_talkQueue.Count == 0)
                {
                    // 没有数据
                    continue;
                }

                try//add by cyp 20081111
                {

                    // 取出传输队列头的数据，为了出错后重传，数据不出队列
                    Byte[] sendBuf = m_talkQueue.Peak();

                    // 打包发送
                    m_commInterface.Write(Packet(sendBuf));

                    // 等待发送完成
                    bool bResult = m_dataRecvEvent.WaitOne(TIME_OUT, false);

                    if (bResult)
                    {
                        // 发送完成
                        //Debug.WriteLine("Sending Successed.");

                        // 此次传输结束
                        Clear(false);

                        // 数据出队列
                        m_talkQueue.Dequeue();//可能出InvalidOperationException异常
                    }
                    else
                    {
                        m_nErrCount++;
                        //Debug.WriteLine("m_nErrCount: " + m_nErrCount);

                        if (m_nErrCount == MAX_ERROR)
                        {
                            // 连续出现MAX_ERROR次错误
                            if (OnDataError != null)
                                OnDataError();
                            else
                                throw new TimeoutException("Device not responding.");

                            // 清除数据
                            Clear(true);
                        }
                    }
                }
                catch
                {
                    if (OnCutPowerError != null)
                        OnCutPowerError();
                    else
                        throw new InvalidOperationException("Transport error.");
                }
            }

            //Debug.WriteLine("TransportThread exited!");
        }
        #endregion

        #region 清除数据
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="bClearQueue">是否清空队列</param>
        private static void Clear(bool bClearQueue)
        {
            // 错误数
            m_nErrCount = 0;
            m_nCheckErrCount = 0;

            if (bClearQueue)
            {
                m_talkQueue.Clear();    // 清空队列
            }
        }
        #endregion

        #region 打包函数，在数据包前加上传输ID，后数据包后面加上校验值
        /// <summary>
        ///  在数据包前加上传输ID
        /// </summary>
        /// <param name="buf">原始数据</param>
        /// <returns>打包后数据</returns>
        private static Byte[] Packet(Byte[] buf)
        {
            // 申请空间
            Byte[] sendBuf = new Byte[2 + buf.Length];

            // 填入传输ID，溢出后归零，不做溢出检查
            sendBuf[0] = unchecked(m_byTransportID++);

            //Debug.WriteLine("sendBuf.TransportID:" + sendBuf[0]);

            // 复制数据
            Byte CheckSum = sendBuf[0];
            for (int i = 0; i < buf.Length; i++)
            {
                sendBuf[i + 1] = buf[i];
                CheckSum ^= buf[i];
            }

            // 填入校验值
            sendBuf[sendBuf.Length - 1] = CheckSum;

            return sendBuf;
        }
        #endregion

        #region 校验函数
        /// <summary>
        /// 校验函数，校验内容：1、TransfportID应与上一次发送时一致；2、异或校验
        /// </summary>
        /// <param name="buf">数据包</param>
        /// <returns>校验正确返回true，否则返回false</returns>
        private static bool Check(Byte[] buf)
        {
            //上一个传输ID
            Byte lastTransportID = buf[0];

            // 测试用，人为加入错误
//             if (lastTransportID%20 == 5)
//             {
//                 return false;
//             }

            ////Debug.Write("   buf.Length:" + buf.Length);
            // TransportID，校验共占两个字节
            if (buf.Length <= 2)
            {
                return false;
            }

            //Debug.WriteLine("   lastTransportID:" + lastTransportID + "   m_byTransportID:" + m_byTransportID);
            // TransportID要匹配
            if (++lastTransportID != m_byTransportID)
            {
                return false;
            }
            ////Debug.WriteLine("buf.Length:"+buf.Length);
            // 求校验码
            Byte code = 0;
            for(int i=0; i<buf.Length; i++)
            {
                code ^= buf[i];
            }
            if (code != 0)
            {
                //Debug.WriteLine("   CheckCode:" + code);
            }
            return (code == 0);
        }
        #endregion

        #region 数据解析
        private static void DataParse(Byte [] buf)
        {
            if (!Check(buf))
            {
                // 数据校验错误，直接返回，等待超时监视线程重发
                m_nCheckErrCount++;
                //Debug.WriteLine(" ***Check Error " + m_nCheckErrCount);
                return;
            }

            // 去掉传输ID和校验值
            Byte[] recvBuf = new Byte[buf.Length - 2];
            Array.Copy(buf, 1, recvBuf, 0, recvBuf.Length);

            //Debug.WriteLine("m_dataRecvEvent.Set");
            m_dataRecvEvent.Set();
            if (OnDataReceive != null)
                OnDataReceive(recvBuf);
        }
        #endregion

        private const int MAX_ERROR = 5;               // 连续出错次数，超过后将调用出错委托
        private static int TIME_OUT = 5000;            // 等待时间，超过将重发，并将错误次数加一
        private static int m_nErrCount = 0;            // 总错误次数，如果正确传输一次将清零
        private static int m_nCheckErrCount = 0;       // 校验错误次数，如果正确传输一次，将清零

        private static ThreadSafeQueue m_talkQueue = new ThreadSafeQueue(10);   // 传输队列，传输成功时数据出队列

        private static CommInterface m_commInterface = new CommInterface();     // 底层串口，进行了转义发送
        
        private static Byte m_byTransportID = 0x00;                             // 传输ID，用于判断应答传输是否匹配
        private static AutoResetEvent m_dataRecvEvent = new AutoResetEvent(false);  // 规定时间内收到正确应答传输则触发

        private static Thread m_transportThread;                                // 传输线程
        private static bool m_bExit;                                            // 传输线程结束标记

        public delegate void DataReceiveCallback(Byte[] buf);
        public static DataReceiveCallback OnDataReceive = null;                 // 委托，正确收到一次应答传输时触发

        public delegate void DataErrorCallback();
        public static DataErrorCallback OnDataError = null;                     // 委托，连续MAX_ERROR错误应答传输时触发

        //add by chengyangpin at 2007-09-01
        public delegate void NoPortErrorCallback();  // 连接端口异常
        public static NoPortErrorCallback OnNoPortError = null;

        public delegate void CutPowerErrorCallBack(); // 连接时断电异常
        public static CutPowerErrorCallBack OnCutPowerError = null;

    }
}
