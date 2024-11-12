using System;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;


namespace Skyray.EDX.Common.Component
{
    /// <summary>
    /// ������·�㣬ʵ��ת���շ���
    /// </summary>
    public class CommInterface
    {
        #region ���캯��
        /// <summary>
        /// Ĭ��ʵ��������
        /// </summary>
        public CommInterface()
        {
            m_sendQueue = new ThreadSafeQueue(10);
            m_port = new SerialPort();
        }
        #endregion

        #region ������·
        /// <summary>
        /// ������·
        /// </summary>
        /// <param name="portName">�˿ں�</param>
        /// <param name="baudRate">������</param>
        public void StartUp(string portName, int baudRate)
        {
            ShutDown();                 // ����Ѿ��򿪣��ȹر�
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

            m_sendQueue.Clear();          // ��շ��Ͷ���
            m_bExit = false;
            m_sendThread = new Thread(new ThreadStart(SendFunc));
            m_sendThread.IsBackground = true;
            m_sendThread.Start();         // ���������߳�

            m_receiveThread = new Thread(new ThreadStart(ReceiveFunc));
            m_receiveThread.IsBackground = true;
            m_receiveThread.Start();
        }
        #endregion

        #region �ر���·
        /// <summary>
        /// �ر���·
        /// </summary>
        public void ShutDown()
        {
            // �ȴ������̡߳������߳̽���
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

        #region �����ݷ��뷢�Ͷ���
        /// <summary>
        /// �����ݷ��뷢�Ͷ���
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
        /// �׳�Send��Recv���쳣(cyp20081027)
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

        #region �����߳�
        /// <summary>
        /// �����߳�
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
                        Byte[] buf = m_sendQueue.Dequeue();//����Ų���� cyp 20081027

                        // SOF
                        m_port.Write(EOF_BUF, 0, EOF_BUF.Length);

                        // ����
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

        #region �����߳�
        /// <summary>
        /// �����߳�
        /// </summary>
        private void ReceiveFunc()
        {
            bool bIsEsc = false;                                // ��һ���ַ���ת���ַ�
            Byte[] receiveBuf = new Byte[COMM_MAX_PACKET_SIZE]; // ���ջ���
            int nIndex = 0;                                     // ���ջ����α�

            while (!m_bExit)
            {
                try
                {
                    if (m_port.BytesToRead > 0)//cyp��BytesToRead�����׳��쳣
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
		                        if (bIsEsc == true)	// ��һ���ַ�Ϊת���ַ�
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

        #region ��·�Ƿ��Ѿ�����
        /// <summary>
        /// ��·�Ƿ��Ѿ�����
        /// </summary>
        public bool IsStartUp
        {
            get
            {
                return m_port.IsOpen;
            }
        }
        #endregion

        private const Byte EOF = 0xFE;                      // ֡ͷ֡β
        private const Byte ESC = 0xAA;                      // ת���ַ�
        private const Byte ESC_EOF = 0xC0;
        private const Byte ESC_ESC = 0x75;

        private const int COMM_MAX_PACKET_SIZE = 4200;      // ���ݰ��������

        private SerialPort m_port;                          // ����

        /// <summary>
        /// ί���࣬�յ�һ֡����ʱ����
        /// </summary>
        /// <param name="buf"></param>
        public delegate void ReceiveCallback(Byte[] buf);
        /// <summary>
        /// ί�ж����յ�һ֡����ʱ����
        /// </summary>
        public ReceiveCallback callBack;

        ThreadSafeQueue m_sendQueue;                        // ���Ͷ���
        Thread m_sendThread;                                // �����߳�

        Thread m_receiveThread;                             // �����߳�

        private bool m_bExit;                               // ���ڹرձ��

        //add by chengyangpin at 2007-09-01
        /// <summary>
        /// ί���ࣺ�����Ҳ����˿ڷ����쳣
        /// </summary>
        public delegate void NoPortErrorCallback();
        /// <summary>
        /// ί�ж��������Ҳ����˿ڷ����쳣
        /// </summary>
        public NoPortErrorCallback OnNoPortError = null;
        /// <summary>
        /// ί���ࣺ����ʱ�ϵ��쳣
        /// </summary>
        public delegate void CutPowerErrorCallBack();
        /// <summary>
        /// ί�ж�������ʱ�ϵ��쳣
        /// </summary>
        public CutPowerErrorCallBack OnCutPowerError = null;

    }

}