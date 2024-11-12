using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common.Component
{
    public class Dp5FastNetDevice:Dp5Interface
    {
        static double[] DP5_Gain = {1.122, 2.491, 3.778, 5.278,
	                                6.556, 8.387, 10.091, 11.322,
	                                14.554, 17.772, 22.402, 30.839,
	                                38.124, 47.468, 66.157, 101.828};

        public Dp5FastNetDevice()
        {
            Dp5Versions = Dp5Version.Dp5_FastNet;
            Dp5Protocol = new DP5Protocol(4096);
            Dp5Protocol.strDp5ConfigValue = string.Empty;
            Dp5Protocol.strDp5ConfigFile = string.Empty;
        }
        public override void OpenDevice()
        {
        }

        public override void CloseDevice()
        {
        }
        public override bool ConnectDevice(string Ip, string Port)
        {
            return Connect(Ip,Port);
        }
        public override bool DisConnectDevice()
        {
            return DisConnect();
        }
        public override bool IsConnected()
        {
            return IsDeviceConected;
        }

        public override bool LoadDP5CfgOther(ref int intPurOn, ref int intFastThreshold, ref string strPeaktime, ref string strFlattop, ref int hv)
        {
            double dblPeaktime = 0;
            double dblFlattop = 0;
            string strhv = string.Empty;
            string strPurOn = string.Empty;

            string path = GetCfgFilePath();
            if (path == string.Empty && !System.IO.File.Exists(path)) return false;
            try
            {
                LoadDP5CfgOther(path, ref strPurOn, ref intFastThreshold, ref dblPeaktime, ref dblFlattop, ref strhv);
                strPeaktime = string.Format("{0:0.00}", dblPeaktime);
                strFlattop = string.Format("{0:0.000}", dblFlattop);
                intPurOn = strPurOn.ToLower().Equals("off") ? 0 : 1;
                hv = Convert.ToInt32(strhv);
                return true;
            }
            catch
            {
              
            }
            return false;
        }


        public override bool SaveDP5CfgOther(int intPurOn, int intFastThreshold, string strPeaktime, string strFlattop, int intVoltage)
        {
            string path = GetCfgFilePath();
            if (path == string.Empty) return false;
            string StrPurOn = intPurOn == 0 ? "OFF" : "ON";
            try
            {
                SaveDP5CfgOther(path, StrPurOn, intFastThreshold, strPeaktime, strFlattop, intVoltage.ToString());
            }
            catch
            {
                return false;
            }
            return true;
        }


        public override bool LoadDP5Cfg()
        {
            string filepath = GetCfgFilePath();
            string fileconfig;
            if(!File.Exists(filepath)) return false;
            else
            {
                GetConfigStringFromFile(filepath,out fileconfig);
                return true ;
            }
        }


        public override bool ChangeCfgFineGain(string FineGain, int SpecLength, int PreTime)
        {
            double fineGain = Convert.ToSingle(FineGain);
            ChangeDP5GainParameters(fineGain, SpecLength);
            return true;
        }

        public override bool GetData(int[] data, ref bool IsSDDEnabled, ref double UsedTime, ref double FastCount, ref double SlowCount)
        {
            long[] ldata;
            int Serial=0;
            double realTime=0;
            bool result = GetSpectrumAndStatus(out ldata, ref UsedTime, ref realTime, ref Serial, ref FastCount, ref SlowCount);
            if (!result || Serial <= 0) IsSDDEnabled = false;
            for (int i = 0; i < ldata.Length && i < data.Length;i++ )
            {
                data[i] = (int)ldata[i];
            }
            return result;
        }

        public override bool PauseDppDate(bool IsPause)
        {
            return PauseTest(IsPause);
        }

        public override bool ClearDppData()
        {

            return ClearData();
        }



        #region  私有

          
            private int SpecLen
            {
                get { return Dp5Protocol==null?2048:Dp5Protocol.SpecLength; }
                set
                {
                    DP5Protocol temp = Dp5Protocol;
                    Dp5Protocol = new DP5Protocol(value);

                    Dp5Protocol.strDp5ConfigValue = string.Empty;
                    Dp5Protocol.strDp5ConfigFile = string.Empty;
                    if (temp!=null)
                    {
                        string[] strs = temp.strDp5ConfigFile.Split(';');
                        for (int i = 0; i < strs.Length; i++)
                        {
                            if (strs[i].Contains("MCAC"))
                            {
                                Dp5Protocol.strDp5ConfigFile += "MCAC=" + value.ToString() + ";";
                            }
                            else
                            {
                                Dp5Protocol.strDp5ConfigFile += strs[i] + ";";
                            }
                        }  
                    }
                    
                }
            }
            
            private int RecieveLen = 0;
            byte[] RecBytes = new byte[520];
            byte[] SpecBytes = new byte[32768];
            DP5Protocol Dp5Protocol;
            
            private Socket ClientSocket=null;
            
            EndPoint RemoteHost;
            private object theLock = new object();
            private bool IsDeviceConected
            {
                get { return ClientSocket == null ? false : ClientSocket.Connected; }
            }

            private bool Connect(string strIp, string strPort)
            {
                lock (theLock)
                {
                    if (ClientSocket != null)
                    {
                        ClientSocket.Close();
                        ClientSocket = null;
                        
                    }
                    try
                    {
                        ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        int IpPort = int.Parse(strPort.Trim() == string.Empty ? strPort.Trim() : "10001");
                        IPEndPoint myhost = new IPEndPoint(IPAddress.Any, IpPort);
                        ClientSocket.Bind(myhost);
                        IPEndPoint remote = new IPEndPoint(IPAddress.Parse(strIp.Trim()), IpPort);
                        RemoteHost = (EndPoint)remote;
                        ClientSocket.BeginConnect(remote, new AsyncCallback(OnConnected), ClientSocket);
                        ClientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
                        ClientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 20480);
                        //ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        //int IpPort = int.Parse(strPort.Trim() == string.Empty ? strPort.Trim() : "10001");
                        //IPEndPoint remote = new IPEndPoint(IPAddress.Parse(strIp.Trim()), IpPort);
                        //RemoteHost = (EndPoint)remote;
                        //ClientSocket.Connect(RemoteHost);
                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        return false;
                    }
                }
                
            }

            private void OnConnected(IAsyncResult ar)
            {
                Socket ss = (Socket)ar.AsyncState;
                try
                {
                    ss.EndConnect(ar);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            private bool DisConnect()
            {
                lock (theLock)
                {
                    if (ClientSocket == null || !ClientSocket.Connected)
                    {
                        return true;
                    }
                    try
                    {
                        ClientSocket.Close();
                        ClientSocket = null;
                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        return false;
                    }
                }
            }

            private bool ReceiveData()
            {
                if (ClientSocket == null || !ClientSocket.Connected || ClientSocket.Available <= 0)
                {
                    return false;
                }
                bool bresult = false;
                int totalLen = 0;
                RecieveLen = 0;
                while (ClientSocket.Available > 0 && RecieveLen < SpecBytes.Length)
                {
                    int nbytes3 = ClientSocket.ReceiveFrom(RecBytes, ref  RemoteHost);
                    Array.Copy(RecBytes, 0, SpecBytes, RecieveLen, nbytes3);
                    RecieveLen += nbytes3;
                }
                int HeaderIndex = Dp5Protocol.HasHeader(SpecBytes, RecieveLen);
                byte[] spec=new byte[RecieveLen-HeaderIndex];
                Array.Copy(SpecBytes, HeaderIndex, spec, 0, RecieveLen-HeaderIndex);
                bresult = Dp5Protocol.ProcessExData(spec);

                return bresult;
            }

            private bool ReceiveDataSpectrum()
            {
                if (ClientSocket == null || !ClientSocket.Connected || ClientSocket.Available <= 0)
                {
                    return false;
                }
                bool bresult = false;
                int totalLen = 0;
                RecieveLen = 0;
                int nbytes3 = ClientSocket.ReceiveFrom(SpecBytes,12360,SocketFlags.None, ref  RemoteHost);
                int HeaderIndex = Dp5Protocol.HasHeader(SpecBytes, nbytes3);
                byte[] temp = new byte[0];
                if (HeaderIndex >= 0)
                {
                    totalLen = SpecBytes[HeaderIndex + 4] * 256 + SpecBytes[HeaderIndex + 5] + 8;
                    RecieveLen = nbytes3 - HeaderIndex;
                    temp = new byte[RecieveLen];
                    Array.Copy(SpecBytes, HeaderIndex, SpecBytes, 0, RecieveLen);
                }
                bresult = Dp5Protocol.ProcessExData(SpecBytes);

                return bresult;
            }

            private bool GetConfigStringFromDpp(out string strConfig)
            {
                lock (theLock)
                {
                    bool bresult = false;
                    strConfig = string.Empty;
                    if (ClientSocket == null || !ClientSocket.Connected)
                    {
                        return false;
                    }
                    byte[] command = Dp5Protocol.GetConfigCommand;
                    int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                    if (nbytes <= 0)
                    {
                        return bresult;
                    }
                    Thread.Sleep(200);
                    bresult = ReceiveData();
                    strConfig = Dp5Protocol.strDp5ConfigValue;
                    return bresult;
                }
            }

            private void GetConfigStringFromFile(string filename,out string strConfig)
            {
                strConfig = string.Empty;
                Dp5Protocol.ReadCfgFromFile(filename);
                strConfig = Dp5Protocol.strDp5ConfigFile; 
            }
            private void SaveConfigStringToFile(string filename, string strConfig)
            {
                Dp5Protocol.strDp5ConfigFile = strConfig ;
                Dp5Protocol.SaveCfgToFile(filename);
            }
            /// <summary>
            /// 加载DP5参数
            /// </summary>
            private void LoadDP5CfgOther(string filename, ref string intPurOn, ref int intFastThreshold, ref double intPeaktime, ref double intFlattop, ref string hv)
            {
                Dp5Protocol.LoadDP5CfgOther(filename, ref intPurOn, ref intFastThreshold, ref intPeaktime, ref intFlattop, ref hv);
            }

            private void SaveDP5CfgOther(string fileName, string purOn, int fastThreshold, string peakingTime, string flatTop, string intVlotage)
            {
                Dp5Protocol.SaveDP5CfgOther(fileName, purOn, fastThreshold, peakingTime, flatTop, intVlotage);
            }

            private bool SetConfigStringToDPP(string strConfig)
            {
                lock (theLock)
                {
                    if (ClientSocket == null || !ClientSocket.Connected)
                    {
                        return false;
                    }
                    if (strConfig != string.Empty)
                    {
                        Dp5Protocol.strDp5ConfigFile = strConfig;
                    }
                    byte[] command = Dp5Protocol.SetConfigCommand;
                    int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                    if (nbytes <= 0)
                    {
                        return false;
                    }
                    Thread.Sleep(50);
                    return ReceiveData();
                }
            }
            private bool SetConfigStringToDPP()
            {
                lock (theLock)
                {
                    if (ClientSocket == null || !ClientSocket.Connected)
                    {
                        return false;
                    }
                    byte[] command = Dp5Protocol.SetConfigCommand;
                    int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                    if (nbytes <= 0)
                    {
                        return false;
                    }
                    Thread.Sleep(50);
                    return ReceiveData();
                }
            }

            private bool GetSpectrumAndStatus(out long[] data, ref double usedTime,ref double realTime,ref int serialNumber,ref double fastCount,ref double slowCount)
            {
                lock (theLock)
                {
                    data = new long[SpecLen];
                    if (ClientSocket == null || !ClientSocket.Connected)
                    {
                        return false;
                    }
                    byte[] command = Dp5Protocol.GetSpectrumCommand;
                    int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                    if (nbytes <= 0)
                    {

                        return false;
                    }
                     Thread.Sleep(300);
                    bool bresult = false;
                    bresult = ReceiveData();
                    //bresult = ReceiveDataSpectrum();
                    if (bresult)
                    {
                        string strStatus;
                        data = Dp5Protocol.SPECTRUM;
                        strStatus = Dp5Protocol.GetStatusString(ref usedTime, ref realTime, ref serialNumber, ref fastCount, ref slowCount);
                    }

                    return bresult;
                }
            }
            private bool GetStatus(out string strStatus)
            {
                lock (theLock)
                {
                    strStatus = string.Empty;
                    if (ClientSocket == null || !ClientSocket.Connected)
                    {
                        return false;
                    }
                    byte[] command = Dp5Protocol.GetStatusCommand;
                    int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                    if (nbytes <= 0)
                    {
                        return false;
                    }
                    Thread.Sleep(50);
                    bool bresult = false;
                    bresult = ReceiveData();
                    if (bresult)
                    {
                        double usedTime = 0;
                        double realTime = 0;
                        int serialNumber = 0;
                        double fastCount = 0;
                        double slowCount = 0;
                        strStatus = Dp5Protocol.GetStatusString(ref usedTime, ref realTime, ref serialNumber, ref fastCount, ref slowCount);
                    }
                    return bresult;
                }
            }
            

            private bool ChangeDP5GainParameters(double FineGain,int SpecLength)
            {
                this.SpecLen = SpecLength;
                Dp5Protocol.strDp5ConfigValue = string.Empty;
                string[] strs = Dp5Protocol==null?new string[0]:Dp5Protocol.strDp5ConfigFile.Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length<=1)
                {
                    return false;
                }
                //int CorseGain=int.Parse(strs.First(wde=>wde.Contains("GAIA")).Split('=')[1]);
                //double totalGain=DP5_Gain[CorseGain-1]*FineGain;
                int num=0;
                for(int i=0;i<DP5_Gain.Length;i++)
                {
                    if(FineGain>DP5_Gain[i])
                    {
                        num=i;
                    }
                }
                double tempFineGain=FineGain/DP5_Gain[num];
                Dp5Protocol.strDp5ConfigFile = string.Empty;
                for (int i = 0; i < strs.Length; i++)
                {
                    //if (strs[i].Contains("GAIF") || strs[i].Contains("GAIA"))
                    if (strs[i].Contains("GAIF"))
                    {
                        //Dp5Protocol.strDp5ConfigFile += "GAIF=" + FineGain.ToString("F3") + ";";
                       //continue;
                        Dp5Protocol.strDp5ConfigFile += "GAIF=" + tempFineGain.ToString("F4") + ";";
                    }
                    else if (strs[i].Contains("GAIN"))
                    {
                        continue;
                    }
                    else if(strs[i].Contains("GAIA"))
                    {
                         Dp5Protocol.strDp5ConfigFile += "GAIA=" + (num+1) + ";";
                    }
                    else if (strs[i].Contains("MCAC"))
                    {
                        Dp5Protocol.strDp5ConfigFile += "MCAC=" + SpecLength.ToString() + ";";
                    }
                    else
                    {
                        Dp5Protocol.strDp5ConfigFile += strs[i] + ";";
                    }
                }
                SetConfigStringToDPP();
                return true;
            }

            //private bool StartTest()
            //{
            //    lock (theLock)
            //    {
            //        bool bresult = false;
            //        byte[] command = Dp5Protocol.EnableMCACommand;
            //        int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
            //        Thread.Sleep(50);
            //        bresult = ReceiveData();
            //        return bresult;
            //    }

            //}

            //private bool StopTest()
            //{
            //    lock (theLock)
            //    {
            //        bool bresult = false;
            //        byte[] command = Dp5Protocol.DisabelMCACommand;
            //        int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
            //        Thread.Sleep(50);
            //        bresult = ReceiveData();
            //        command = Dp5Protocol.ClearSpectrumCommand;
            //        nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
            //        Thread.Sleep(50);
            //        bresult = ReceiveData();
            //        return bresult;
            //    }
            //}

            private bool ClearData()
            {
                lock (theLock)
                {
                    bool bresult = false;
                    byte[] command = Dp5Protocol.DisabelMCACommand;
                    int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                    Thread.Sleep(50);
                    bresult = ReceiveData();
                    command = Dp5Protocol.ClearSpectrumCommand;
                    nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                    Thread.Sleep(50);
                    bresult = ReceiveData();
                    bresult = false;
                    command = Dp5Protocol.EnableMCACommand;
                    nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                    Thread.Sleep(50);
                    bresult = ReceiveData();
                    return bresult;
                }
            }
            private bool PauseTest(bool IsPause)
            {
                lock (theLock)
                {
                    bool bresult = false;
                    if (IsPause)
                    {
                        byte[] command = Dp5Protocol.DisabelMCACommand;
                        int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                        Thread.Sleep(50);
                        bresult = ReceiveData();
                    }
                    else
                    {
                        byte[] command = Dp5Protocol.EnableMCACommand;
                        int nbytes = ClientSocket.SendTo(command, command.Length, SocketFlags.None, RemoteHost);
                        Thread.Sleep(50);
                        bresult = ReceiveData();
                    }
                    return bresult;
                }
            }

        #endregion
    }
}
