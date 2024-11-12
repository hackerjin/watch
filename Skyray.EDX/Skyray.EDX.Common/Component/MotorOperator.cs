using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Windows.Forms;
namespace Skyray.EDX.Common
{
    public enum MotorAxle
    {
        Reset = 0x81,
        XY = 0x82,
        Y1 = 0x8A,
        Z = 0x86,
        CMDState = 0x22
    }


    public enum CMDState
    {
        None = 0x00,
        StartTest = 0x01,
        StopTest = 0x02,
        Focus = 0x04
    }

    class MotorProtocol
    {
        
        public static byte StartByte = 0xEF; //开始
        public static byte EndByte = 0xFE;   //结束
        public byte CmdByte = 0x22;

        #region  电机命令
        //复位命令
        public static byte[] ResetCmd()
        {
            byte[] newbytes = new byte[5];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0x80;
            newbytes[2] = 0x01;//长度
            newbytes[3] = 0x01;
            newbytes[4] = EndByte;
            return newbytes;
        }


        //查询是否复位成功
        public static byte[] CheckInitCmd()
        {
            byte[] newbytes = new byte[5];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0x81;
            newbytes[2] = 0x01;//长度
            newbytes[3] = 0x00;
            newbytes[4] = EndByte;
            return newbytes;
        }


        //
        /// <summary>
        /// 设置XY移动位置 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static  byte[] SetMotorXYCmd(int X, int Y)
        {
            //计算下发步长
            //int XDistance = (int)(X * 256 * 200 * 5);
            //int YDistance = (int)(Y * 256 * 200 * 5);

            int XDistance = X;
            int YDistance = Y;

            byte[] newbytes = new byte[12];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0x82;
            newbytes[2] = 0x08;//长度
            byte[] byteTmp = intToBytesContainsDir(XDistance); //X轴负半轴为00，正半轴80
            newbytes[3] = byteTmp[3];
            newbytes[4] = byteTmp[2];
            newbytes[5] = byteTmp[1];
            newbytes[6] = byteTmp[0];
            byteTmp = intToBytesContainsDir(YDistance);   //Y轴负半轴为Ox00，正半轴0x80
            newbytes[7] = byteTmp[3];
            newbytes[8] = byteTmp[2];
            newbytes[9] = byteTmp[1];
            newbytes[10] = byteTmp[0];
            newbytes[11] = EndByte;
            return newbytes;
        }


        public static byte[] SetMotorY1Cmd(int y1)
        {
             //int y1Distance = (int)(y1 * 256 * 200 * 5);
            int y1Distance = y1;
             byte[] newbytes = new byte[8];
             newbytes[0] = StartByte;//起始
             newbytes[1] = 0x8A;
             newbytes[2] = 0x04;//长度
             byte[] byteTmp = intToBytesContainsDir(y1Distance); //X轴负半轴为00，正半轴80
             newbytes[3] = byteTmp[3];
             newbytes[4] = byteTmp[2];
             newbytes[5] = byteTmp[1];
             newbytes[6] = byteTmp[0];
             newbytes[7] = EndByte;
             return newbytes;
        }


        public static byte[] SetMotorZCmd(int Z, bool isFast)
        {
            int ZDistance = Z;
            byte[] newbytes = new byte[8];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0x86;
            newbytes[2] = 0x04;//长度
            byte[] byteTmp = intZToBytesContainsDir(ZDistance, isFast); //X轴负半轴为00，正半轴80
            newbytes[3] = byteTmp[3];
            newbytes[4] = byteTmp[2];
            newbytes[5] = byteTmp[1];
            newbytes[6] = byteTmp[0];
            newbytes[7] = EndByte;
            return newbytes;
        }

        public static byte[] SetGetXYCmd()
        {
            
            byte[] newbytes = new byte[5];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0xA0;
            newbytes[2] = 0x01;
            newbytes[3] = 0x00;
            newbytes[4] = EndByte;
            return newbytes;
        }


        public static byte[] SetGetZCmd()
        {

            byte[] newbytes = new byte[5];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0xA1;
            newbytes[2] = 0x01;
            newbytes[3] = 0x00;
            newbytes[4] = EndByte;
            return newbytes;
        }

        public static byte[] SetStopCmd(int motorNo)
        {

            byte[] newbytes = new byte[5];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0x90;
            newbytes[2] = 0x01;
            switch(motorNo)
            {
                case 1:
                    newbytes[3] = 0x01;
                    break;
                case 2:
                    newbytes[3] = 0x02;
                    break;
                case 3:
                    newbytes[3] = 0x04;
                    break;
                case 4:
                    newbytes[3] = 0x08;
                    break;
            }
            newbytes[4] = EndByte;
            return newbytes;
        }


        /// <summary>
        /// XY移动完成返回
        /// </summary>
        /// <returns></returns>
        public  static byte[] SetXYComplete()
        {
            byte[] newbytes = new byte[5];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0x82;
            newbytes[2] = 0x01;//长度
            newbytes[3] = 0x01;
            newbytes[4] = EndByte;
            return newbytes;
        }

        /// <summary>
        /// Y1移动完成返回
        /// </summary>
        /// <returns></returns>
        public static byte[] SetY1Complete()
        {
            byte[] newbytes = new byte[5];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0x8A;
            newbytes[2] = 0x01;//长度
            newbytes[3] = 0x01;
            newbytes[4] = EndByte;
            return newbytes;
        }

        /// <summary>
        /// Z移动完成返回
        /// </summary>
        /// <returns></returns>
        public static byte[] SetZComplete()
        {
            byte[] newbytes = new byte[5];
            newbytes[0] = StartByte;//起始
            newbytes[1] = 0x86;
            newbytes[2] = 0x01;//长度
            newbytes[3] = 0x01;
            newbytes[4] = EndByte;
            return newbytes;
        }


        /// <summary>
        /// int 转byte低位在前    val <0 ,反向， val>0 正向
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] intToBytesContainsDir(int val)
        {
            int value = Math.Abs(val);
            //int转换Byte  
            byte[] src = new byte[4];
            src[0] = (byte)(value & 0xff);
            src[1] = (byte)(value >> 8 & 0xff);
            src[2] = (byte)(value >> 16 & 0xff);
            src[3] = (byte)(value >> 24 & 0xff | ((val > 0) ? 0x80 : 0x00));  //val>为正(向右，向上)
            return src;

        }

        public static byte[] intZToBytesContainsDir(int val,bool isFast)
        {
            int value = Math.Abs(val);
            //int转换Byte  
            byte[] src = new byte[4];
            src[0] = (byte)(value & 0xff);
            src[1] = (byte)(value >> 8 & 0xff);
            src[2] = (byte)(value >> 16 & 0xff);
            if(!isFast)
                src[3] = (byte)(value >> 24 & 0xff | ((val > 0) ? 0x80 : 0x00));  //val>为正(向右，向上)
            else
                src[3] = (byte)(value >> 24 & 0xff | ((val > 0) ? 0xc0 : 0x40));  //val>为正(向右，向上)
            return src;

        }

        #endregion

    }

    public class MotorOperator
    {
        private static SerialPortExecute m_SerialPort = null;

        public static event HandResult SerialPortResult;
        public  static event HandResult SerialPortSend;
        private static string _PortName;
        private static int _Frequency;
        private static int _TimeOut;
        private static byte[] byteReceveCheck = new byte[32767];
        private static List<byte[]> ReceiveDataList = new List<byte[]>();
        private static int MaxRecieveCount = 10;


        public static CMDState returnCmdState = CMDState.None;
        public static CMDState ReturnCMDState
        {
            get
            {
                return returnCmdState;
            }
        }

        public static bool IsSerialPortOpen
        {
            get
            {
                if (m_SerialPort != null) return m_SerialPort.IsOpen;
                return false;
            }
        }


        public static  bool StartPort()
        {
            try
            {
              
                if (m_SerialPort == null)
                {
                    WorkCurveHelper.sPortName = ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/ComSel");
                    WorkCurveHelper.iFrequency = int.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/Frequency"));
                    WorkCurveHelper.iTimeOut = int.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/TimeOut"));

                    m_SerialPort = new SerialPortExecute(WorkCurveHelper.sPortName, WorkCurveHelper.iFrequency);
                    m_SerialPort.Parity = Parity.None;
                    m_SerialPort.StopBits = StopBits.One;
                    m_SerialPort.Handshake = Handshake.None;
                    m_SerialPort.DataBits = 8;
                    m_SerialPort.ReadBufferSize = 100;
                    m_SerialPort.ReceivedBytesThreshold = 1;
                    m_SerialPort.BufferSize = 8;
                    m_SerialPort.ReceiveTimeout = WorkCurveHelper.iTimeOut;
                    m_SerialPort.WriteBufferSize = 100;
                    m_SerialPort.SendInterval = 100;
                    //m_SerialPort.SerialPortResult += new HandResult(SerialPort_Result);
                    m_SerialPort.OnSerialPortReceived += new OnReceivedData(SerialPort_Received);
                    m_SerialPort.OnSeriaPortSend += new OnSendData(SerialPort_Send);
                }
                {
                    m_SerialPort.Start();
                    

                }
                return true;
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show("串口初始化失败，请正确设置串口参数");
                m_SerialPort = null;
                return false;
            }
        }


        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public static bool StopPort()
        {
            try
            {
                if (m_SerialPort != null && m_SerialPort.IsOpen)
                    m_SerialPort.Stop();
                m_SerialPort = null;
                return true;
            }
            catch (Exception ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message);
                return false;
            }
        }

        /// <summary>
        /// 更改端口参数
        /// </summary>
        /// <param name="strPortName"></param>
        /// <param name="iFrequency"></param>
        /// <param name="iTimeOut"></param>
        /// <returns></returns>
        public static bool ChangePortName(string strPortName, int iFrequency, int iTimeOut)
        {
            if (m_SerialPort != null && m_SerialPort.IsOpen)
            {
                return false;
            }
            m_SerialPort = null;
            _PortName = strPortName;
            _Frequency = iFrequency;
            _TimeOut = iTimeOut;
            return true;

        }

        public static  bool IsResetOk()
        {
            if (m_SerialPort != null && m_SerialPort.IsOpen)
            {
                return false;
            }
            byte[] ResetSuccmd = MotorProtocol.CheckInitCmd();
            m_SerialPort.Send(ResetSuccmd);
            System.Threading.Thread.Sleep(1000);
            ResetSuccmd[3] = 0x03;
            if (IsLstRecieveDatasContainsComfirm(ResetSuccmd))
            {
                return true;
            }
            Msg.Show("系统复位失败");
            return false;
        }

        private static object theLock = new object();
        public static void SerialPort_Received(object sender, SerialPortEvents e)
        {
          
            lock (theLock)
            {
                Array.Clear(byteReceveCheck, 0, byteReceveCheck.Length);
                int dataLen = e.BufferData.Length;
                if (dataLen > byteReceveCheck.Length) dataLen = byteReceveCheck.Length;

                Array.Copy(e.BufferData, byteReceveCheck, dataLen);
                int bytecount = dataLen;
                int startEFIndex = 0;
                //int endFEIndex = 0;
                for (int i = 0; i < bytecount; i++)
                {
                    if (byteReceveCheck[i] == MotorProtocol.StartByte)
                    {
                        startEFIndex = i;
                        //取数据长度i+2
                        int dataLenTemp = byteReceveCheck[i + 2];
                        if (dataLenTemp > 10) continue;
                        else if (startEFIndex + dataLenTemp + 4 > bytecount) continue;
                        byte[] byteTemps = new byte[dataLenTemp + 4];
                        Array.Copy(byteReceveCheck, startEFIndex, byteTemps, 0, byteTemps.Length);
                        if (byteTemps[byteTemps.Length - 1] == MotorProtocol.EndByte)
                        {
                            ReceiveDataList.Add(byteTemps);
                            i += byteTemps.Length - 1;

                            //出现按键操作
                            if ( byteReceveCheck[1] == (byte)MotorAxle.CMDState)
                            {
                                returnCmdState = OperatorCMD((CMDState)byteReceveCheck[3]);

                            }
                        }

                    }
                  
                }
            }
        }


        private static void SerialPort_Send(object sender, SerialPortEvents e)
        {
            ReceiveDataList.Clear();
            if (SerialPortSend != null) SerialPortSend(sender, e);
        }


        private static CMDState OperatorCMD(CMDState cmdstate)
        {
            CMDState re;
            switch (cmdstate)
            {
                case CMDState.StartTest:
                    re = CMDState.StartTest;
                    break;
                case CMDState.StopTest:
                    re = CMDState.StopTest;
                    break;
                case CMDState.Focus:
                    re = CMDState.Focus;
                    break;
                default:
                    re = CMDState.None;
                    break;
            }
            return re;
        }



        #region   电机操作

        public static bool MotorOperatorResetThread()
        {

            new System.Threading.Thread(new System.Threading.ThreadStart(MotorOperatorResetReady)).Start();
            return true;

        }
        /// <summary>
        /// 电机复位
        /// </summary>
        /// <param name="motorAxle"></param>
        /// <returns></returns>
        public static void MotorOperatorResetReady()
        {
            WorkCurveHelper.X = WorkCurveHelper.ResetX;
            WorkCurveHelper.Y = WorkCurveHelper.ResetY;
            WorkCurveHelper.Z = WorkCurveHelper.ResetZ;
            WorkCurveHelper.isIn = false;
            if (m_SerialPort == null || !m_SerialPort.IsOpen)
            { return; }
            //复位
            m_SerialPort.Send(MotorProtocol.ResetCmd());
            System.Threading.Thread.Sleep(2000);
            int ReceiveCount = 0;
            int LastReiveLaterCount = 0;
            //检查复位
            byte[] ResetSuccmd = MotorProtocol.CheckInitCmd();
            System.Threading.Thread.Sleep(100);
            
            while (true)
            {
                if (ReceiveDataList.Count > ReceiveCount)
                {
                    ReceiveCount = ReceiveDataList.Count;
                    LastReiveLaterCount = 0;
                }
                if (LastReiveLaterCount > MaxRecieveCount) break;
                ResetSuccmd[3] = 0x03;
                if (IsLstRecieveDatasContainsComfirm(ResetSuccmd))
                { return ; }

                //如果返回发送值，则状态不对，需要等待10s
                ResetSuccmd[3] = 0x00;
                if (IsLstRecieveDatasContainsComfirm(ResetSuccmd))
                {
                    System.Threading.Thread.Sleep(10000);
                    break;
                }
                LastReiveLaterCount++;
            }

            for (int i = 0; i < 3; i++)
            {
                ResetSuccmd = MotorProtocol.CheckInitCmd();
                m_SerialPort.Send(ResetSuccmd);
                System.Threading.Thread.Sleep(1000);
                ResetSuccmd[3] = 0x03;
                if (IsLstRecieveDatasContainsComfirm(ResetSuccmd))
                {
                    return ;
                }
            }
            
        }


        public class Param
        {
            public Point point;
            public ulong moveNo;

            public Param(Point point, ulong moveNo)
            {
                this.point = point;
                this.moveNo = moveNo;
            }


        }

     

        /// <summary>
        /// XY移动完成
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>

        public static bool MotorOperatorXYThread(int X, int Y)
        {


            if (X == 0 && Y == 0)
            {
                WorkCurveHelper.motorMoving = false;
                WorkCurveHelper.goingToDest = false;

                return false;
            }

            if (!WorkCurveHelper.contiMoving)
            {
                if ((X > 0 && WorkCurveHelper.X < X) || (Y < 0 && WorkCurveHelper.Y < -Y))
                {
                    Msg.Show("不可移动超平台范围距离！");
                    WorkCurveHelper.motorMoving = false;
                    WorkCurveHelper.goingToDest = false;

                    return false;
                }

                if ((X < 0 && WorkCurveHelper.X - X > WorkCurveHelper.TabWidthStep) || (Y > 0 && WorkCurveHelper.Y + Y > WorkCurveHelper.TabHeightStep))
                {
                    Msg.Show("不可移动超平台范围距离！");
                    WorkCurveHelper.motorMoving = false;
                    WorkCurveHelper.goingToDest = false;

                    return false;
                }
            }

            if (m_SerialPort == null || !m_SerialPort.IsOpen)
            {
                WorkCurveHelper.motorMoving = false;
                WorkCurveHelper.goingToDest = false;

                return false; 
            }
       

            WorkCurveHelper.moveNo = WorkCurveHelper.moveNo + 1;
            ulong moveNo = WorkCurveHelper.moveNo;

            Thread t = new Thread(new ParameterizedThreadStart(MotorOperatorXY));
            t.Start(new Param(new Point(X, Y),moveNo));
            return true;

        }


        public static void MotorOperatorXY(object param)
        {

            Point p = (Point)((Param)param).point;
            ulong moveNo = (ulong)((Param)param).moveNo;
            int X = p.X;
            int Y = p.Y;
          
            while (moveNo > WorkCurveHelper.nextNo) ;

            int ReadCount = 0;


            m_SerialPort.Send(MotorProtocol.SetMotorXYCmd(X, Y));

            if (WorkCurveHelper.contiMoving)   
                return;
            
            //查询是否完成byte
            byte[] ByteConfirm = MotorProtocol.SetXYComplete();
            while (true)
            {

                Thread.Sleep(1000);
                if (IsLstRecieveDatasContainsComfirm(ByteConfirm))
                {
                    
                    getXY();
    
                    WorkCurveHelper.nextNo = WorkCurveHelper.nextNo + 1;

                    WorkCurveHelper.motorMoving = false;

                    WorkCurveHelper.goingToDest = false;


                    return;
                }
                ReadCount++;
                if (ReadCount > MaxRecieveCount)
                {

                    getXY();

                    
                    
                    WorkCurveHelper.nextNo = WorkCurveHelper.nextNo + 1;
                    WorkCurveHelper.motorMoving = false;
                    WorkCurveHelper.goingToDest = false;


                    break;
                }
            }

        }


        public static bool MotorOperatorZThread(int Z)
        {

            if (Z == 0  || Math.Abs(Z) < 5)
            {
                WorkCurveHelper.motorMoving = false;
                return false;
            }
            if (m_SerialPort == null || !m_SerialPort.IsOpen )

            {
                WorkCurveHelper.motorMoving = false;    
                return false; 
            }
   


            WorkCurveHelper.moveNo = WorkCurveHelper.moveNo + 1;
            ulong moveNo = WorkCurveHelper.moveNo;

            
            Thread t = new Thread(new ParameterizedThreadStart(MotorOperatorZ));
            t.Start(new Param(new Point(Z, WorkCurveHelper.zSpeedMode ? 1 : 0), moveNo));
            return true;

        }



        public static void MotorOperatorZ(object param)
        {


            Point p = (Point)((Param)param).point;
            ulong moveNo = (ulong)((Param)param).moveNo;
            int Z = p.X;
            bool isFast = p.Y == 1 ? true : false;

            while (moveNo > WorkCurveHelper.nextNo) ;

            int ReadCount = 0;
            m_SerialPort.Send(MotorProtocol.SetMotorZCmd(Z, isFast));


            if (WorkCurveHelper.contiMoving)
                return;

            //查询是否完成byte
            byte[] ByteConfirm = MotorProtocol.SetZComplete();
            while (true)
            {
                Thread.Sleep(1000);
                if (IsLstRecieveDatasContainsComfirm(ByteConfirm))
                {
                    WorkCurveHelper.nextNo = WorkCurveHelper.nextNo + 1;
                    WorkCurveHelper.motorMoving = false;

                    return;
                }
                ReadCount++;
                if (ReadCount > MaxRecieveCount)
                {
                    WorkCurveHelper.nextNo = WorkCurveHelper.nextNo + 1;
                    WorkCurveHelper.motorMoving = false;

                    break;
                }
            }

        }



        public static void stopMotorThread(int motorNo)
        {

            if (m_SerialPort == null || !m_SerialPort.IsOpen)
            { return; }

            ulong moveNo = WorkCurveHelper.moveNo;

            Thread t = new Thread(new ParameterizedThreadStart(stopMotor));
            t.Start(new Param(new Point(motorNo, 0), moveNo));
            return;


        }

        public static  bool curDir = true;

        public static void stopMotor(object param)
        {

            Point p = (Point)((Param)param).point;
            int motorNo = p.X;
            ulong moveNo = (ulong)((Param)param).moveNo;

            while (moveNo >= WorkCurveHelper.nextNo) ;

            Thread.Sleep(50);

            m_SerialPort.Send(MotorProtocol.SetStopCmd(motorNo));

            Thread.Sleep(200);

            if (motorNo != 3)
            {
                if (motorNo == 1)
                    curDir = true;
                else
                    curDir = false;
                getXY();
            }
            WorkCurveHelper.contiMoving = false;
            WorkCurveHelper.motorMoving = false;
            

        }



        public static bool MotorOperatorY1Thread(int y)
        {
            if (!WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                return false;

            if (y == 0 )
                return true;

            if (m_SerialPort == null || !m_SerialPort.IsOpen)
            { return false; }

            y = (WorkCurveHelper.DeviceCurrent.MotorSpinDirect == 0 ? y : -y);

            if (WorkCurveHelper.DeviceCurrent.MotorSpinDirect == 0)
            {
                if (y < 0)
                {
                    if (WorkCurveHelper.isIn)
                        return false;
                }
                else
                {
                    if (!WorkCurveHelper.isIn)
                        return false;
                }
            }
            else
            {

                if (y > 0)
                {
                    if (WorkCurveHelper.isIn)
                        return false;
                }
                else
                {
                    if (!WorkCurveHelper.isIn)
                        return false;
                }

            }


            WorkCurveHelper.moveNo = WorkCurveHelper.moveNo + 1;
            ulong moveNo = WorkCurveHelper.moveNo;

            WorkCurveHelper.isIn = !WorkCurveHelper.isIn;                    

            Thread t = new Thread(new ParameterizedThreadStart(MotorOperatorY1));
           
            t.Start(new Param(new Point(y, 0), moveNo));
            return true;

        }


        /// <summary>
        /// Y1移动完成
        /// </summary>
        /// <param name="y1"></param>
        /// <returns></returns>
        public static void MotorOperatorY1(object param)
        {

           
            Point p = (Point)((Param)param).point;
            ulong moveNo = (ulong)((Param)param).moveNo;
            int y = p.X;

            while (moveNo > WorkCurveHelper.nextNo) ;


            int ReadCount = 0;

            byte[] ByteConfirm;
            if(WorkCurveHelper.DeviceCurrent.MotorSpinDirect == 0)
            {
                m_SerialPort.Send(MotorProtocol.SetMotorY1Cmd(y));
                ByteConfirm = MotorProtocol.SetY1Complete();

            }
            else
            {
                m_SerialPort.Send(MotorProtocol.SetMotorXYCmd(0, y));
                ByteConfirm = MotorProtocol.SetXYComplete();

            }

            //查询是否完成byte
            while (true)
            {
                Thread.Sleep(1000);
                if (IsLstRecieveDatasContainsComfirm(ByteConfirm))
                {
                    WorkCurveHelper.nextNo = WorkCurveHelper.nextNo + 1;
                    
                    return;
                }
                ReadCount++;
                if (ReadCount > MaxRecieveCount)
                {
                    WorkCurveHelper.nextNo = WorkCurveHelper.nextNo + 1;
                    break;
                }
            }

        }


        public static void updateLargeViewNow()
        {
            System.Reflection.MethodInfo myMethod1 = WorkCurveHelper.ucCameraType.GetMethod("updateLargeViewNow");
            myMethod1.Invoke(WorkCurveHelper.ucCamera, null);

        }

        public static void getXY()
        {

            if (m_SerialPort == null || !m_SerialPort.IsOpen)
            { return; }

            
            int ReadCount = 0;
            m_SerialPort.Send(MotorProtocol.SetGetXYCmd());
            

            while (true)
            {
                Thread.Sleep(100);

                int X = 0;
                int Y = 0;

                if (getXYSteps(ref X, ref Y))
                {
                    WorkCurveHelper.X = WorkCurveHelper.ResetX - X;
                    WorkCurveHelper.Y = WorkCurveHelper.ResetY + Y;

                    new System.Threading.Thread(new System.Threading.ThreadStart(updateLargeViewNow)).Start();

                    if (WorkCurveHelper.X <= 0 ||  WorkCurveHelper.X >= WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff
                        || WorkCurveHelper.Y <= 0 || WorkCurveHelper.Y >= WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff
                        )
                        Msg.Show("请注意，平台已移动到边界");
                    return;
                }

                ReadCount++;

                if (ReadCount > MaxRecieveCount)
                {
                    break;
                }

            }


        }


        public static void getZ()
        {

            if (m_SerialPort == null || !m_SerialPort.IsOpen)
            { return; }

            int ReadCount = 0;
            m_SerialPort.Send(MotorProtocol.SetGetZCmd());

            while (true)
            {
                Thread.Sleep(200);

                int Z = 0;

                if (getZSteps(ref Z))
                {
                    WorkCurveHelper.Z = WorkCurveHelper.ResetZ - Z;
                    return;
                }

                ReadCount++;

                if (ReadCount > MaxRecieveCount)
                {
                    break;
                }

            }

        }

      

       /// <summary>
       /// 判断返回状态是否一致，true为操作成功
       /// </summary>
       /// <param name="Comfirmdata"></param>
       /// <returns></returns>
        private static bool IsLstRecieveDatasContainsComfirm(byte[] Comfirmdata)
        {
            int ReceiveCount = ReceiveDataList.Count;
            for (int i = 0; i < ReceiveCount; i++)
            {
                byte[] bytsTemp = ReceiveDataList[i];
                //判断接受的信息是否跟发送的信息相同
                bool bEqual = true;
                for (int j = 0; j < Comfirmdata.Length && j < bytsTemp.Length; j++)
                {
                    bEqual = bEqual && (Comfirmdata[j] == bytsTemp[j]);
                }
                if (bEqual) return true;
            }
            return false;
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
        
        private static bool getXYSteps(ref int X , ref int Y)
        {
            int ReceiveCount = ReceiveDataList.Count;
            for (int i = 0; i < ReceiveCount; i++)
            {
                byte[] bytesTemp = ReceiveDataList[i];

                
                if (bytesTemp.Length == 12 && bytesTemp[1] == 0xB0 && bytesTemp[2] == 0x08)
                {
                    Array.Reverse(bytesTemp,3,4);
                    Array.Reverse(bytesTemp, 7, 4);
                    X = BitConverter.ToInt32(bytesTemp,3);
                    Y = BitConverter.ToInt32(bytesTemp, 7);
                    
                    if (WorkCurveHelper.selfCheck)
                    {
                        string path = "电机自检.txt";
                        if (curDir == true)
                            addCheckResult(path,X.ToString());
                        else
                            addCheckResult(path, Y.ToString());
                        WorkCurveHelper.xSteps = X;
                        WorkCurveHelper.ySteps = Y;
                    }
                    
                    return true;
                }
            }

            return false; 
        }



        private static bool getZSteps(ref int Z)
        {
            int ReceiveCount = ReceiveDataList.Count;
            for (int i = 0; i < ReceiveCount; i++)
            {
                byte[] bytesTemp = ReceiveDataList[i];

                if (bytesTemp.Length == 8 && bytesTemp[1] == 0xB1 && bytesTemp[2] == 0x04)
                {
                    Array.Reverse(bytesTemp, 3, 4);
                    Z = BitConverter.ToInt32(bytesTemp, 3);
                    return true;
                }
            }

            return false;
        }


        #endregion

    }
}
