using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;
using XRFNetLib;
using System.Threading;
using System.Runtime.InteropServices;
using Skyray.Key;
using System.IO;
using Skyray.Dog;

namespace Skyray.EDX.Common.Component
{
    public class NetPort : Port, INetworkCallBack, IDetectorCallBack
    {
        public string strSubNet = "255.255.255.0";
        public string strGetWay = "192.168.2.1";
        public string strDNS = "192.168.1.2";
        public double TempVoltage = 0;
        public double TempCurrent = 0;
        public int TempUncover = 0;
        public uint TempVacuum = 0;
        public bool DoRecordingToFile = false;
        private bool DoConsoleWriteLine = false;
        private static readonly object lockGet = new object();
        private static readonly object recordLocker = new object();
    
        private static XRFNetLib.XRFRay xrfRay;

        private static XRFNetLib.Detector detector;
        private static Apparatus ss;
        private static Electromagnet electromagnetClass;

        public static readonly XRFNetworkClass netWork = new XRFNetworkClass();


        public NetPort()
        {
            netWork.SetCallback(this);
            //dataLength = (int)WorkCurveHelper.DeviceCurrent.SpecLength;
            //specData = new int[dataLength];
            dataLength = 4096;
            specData = new int[dataLength];
        }


        public override bool IPSettings(string IP, string SubNet, string GateWay, string DNS)
        {
            lock (lockGet)
            {
                this.strIp = IP;
                this.strSubNet = SubNet;
                this.strGetWay = GateWay;
                this.strDNS = DNS;
                byte[] byteIp = System.Text.Encoding.Default.GetBytes(strIp);
                byte[] byteSubNet = System.Text.Encoding.Default.GetBytes(strSubNet);
                byte[] byteGetWay = System.Text.Encoding.Default.GetBytes(strGetWay);
                byte[] byteDNS = System.Text.Encoding.Default.GetBytes(strDNS);
                try
                {
                    netWork.GetApparatus().Config(ref byteIp[0], ref byteSubNet[0], ref byteGetWay[0], ref byteDNS[0]);
                    RecordInfo("IPSettings成功！");
                }
                catch(Exception ex)
                {
                    RecordInfo("IPSettings失败！" + ex.ToString());
                    return false;
                }
                return true;
            }
        }

        public override void Dispose()
        {
            //throw new NotImplementedException();
        }

        public override bool Connect()
        {
            //byte[] byteIp = System.Text.Encoding.Default.GetBytes(strIp);

            //byte[] byteIp = Encoding.Default.GetBytes(strIp);
            lock (lockGet)
            {
                try
                {
                    COMThreadManager.SafeCall(() =>
                    {
                        RecordInfo("Connect开始！");
                        netWork.Disconnect();
                        bool isInSevenDays = false;
                        string surPlus = string.Empty;
                        HardwareDog.SNClose();
                       
                        HardwareDog.CheckHardwareDog(WorkCurveHelper.snFilePath, false, null);
                        HardwareDog.SNClose();
                       
                        Thread.Sleep(200);
                        netWork.Connect(ref byteIp[0], uPort);
                        RecordInfo("Connect成功！");

                       
                        //此处为针对edxv机器上的近景摄像头的特殊处理，对其他机型没有影响
                        Thread.Sleep(500);
                        netWork.CloseSmallCamera();
                        Thread.Sleep(2000);
                        netWork.OpenSmallCamera();
                        Thread.Sleep(1200);
                        netWork.CloseSmallCamera();
                        
                       

                    });
                    
                }
                catch (Exception ex)
                { 
                    RecordInfo("Connect失败！" + ex.ToString());
                    MessageBox.Show("Connect Exception: " + ex.Message);
                    return false;
                }
                return true;
            }
        }

        
       

        public override bool Disconnect()
        {
            lock (lockGet)
            {
                COMThreadManager.SafeCall(() =>
                {
                    RecordInfo("Disconnect开始");
                    netWork.Disconnect();
                    HardwareDog.SNClose();
                    RecordInfo("Disconnect成功");
                }, 3000);
                return true;
            }
        }


       
        public override bool OpenVoltage()
        {
            lock (lockGet)
            {
                //HighVoltage highVoltage = netWork.GetHighVotage();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    xrfRay.OpenHighVoltage();
                    
                    RecordInfo("开高压成功！");
                    //Console.WriteLine("开高压成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("开高压失败！");
                    RecordInfo("开高压失败！" + ex.ToString());
                    return false;
                }
                return true;
            }
        }

        public override bool CloseVoltage()
        {
            lock (lockGet)
            {
                //HighVoltage highVoltage = netWork.GetHighVotage();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    xrfRay.CloseHighVoltage();
                    //Console.WriteLine("关高压成功！");
                    
                    RecordInfo("关高压成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("关高压失败！");
                    RecordInfo("关高压失败！" + ex.ToString());
                    return false;
                }
                return true;
            }
        }

        public override bool OpenPump()
        {
            lock (lockGet)
            {
                //XRFNetLib.VacuumPump vacuumPump = netWork.GetVacuumPump();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    xrfRay.OpenVacuumPump();
                    RecordInfo("开真空泵成功！");
                }
                catch (Exception ex)
                {
                    RecordInfo("开真空泵失败！" + ex.ToString());
                    return false;
                }
                return true;
            }
        }

        public override bool GetVacuum(uint uType, out uint uVacuum)
        {
            lock (lockGet)
            {
                //XRFNetLib.VacuumPump vacuumPump = netWork.GetVacuumPump();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    uVacuum = TempVacuum;
                    //Console.WriteLine("获取真空度");
                    xrfRay.GetVacuum(uType, out uVacuum, 2000);
                    //Console.WriteLine("获取真空度成功：" + uVacuum.ToString());
                    TempVacuum = uVacuum;
                    RecordInfo("获取真空度成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("获取真空度失败！");
                    RecordInfo("获取真空度失败！" + ex.ToString());
                    Log.Error("vacuumPump GetVacuum  " + "error");
                    uVacuum = TempVacuum;
                    return false;
                }
                return true;
            }
        }

        public override bool ClosePump()
        {
            lock (lockGet)
            {
                //XRFNetLib.VacuumPump vacuumPump = netWork.GetVacuumPump();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    xrfRay.CloseVacuumPump();
                    RecordInfo("关真空泵成功！");
                }
                catch (Exception ex)
                {
                    RecordInfo("关真空泵失败！" + ex.ToString());
                    return false;
                }
                return true;
            }
        }

        public override bool OpenVoltageLamp()
        {
            lock (lockGet)
            {
                //HighVoltage highVoltage = netWork.GetHighVotage();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    xrfRay.OpenPilotLamp();
                    //Console.WriteLine("开高压灯成功！");
                    RecordInfo("开高压灯成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("开高压灯失败！");
                    RecordInfo("开高压灯失败！" + ex.ToString());
                    return false;
                }
                return true;
            }
        }

        public override bool CloseVoltageLamp()
        {
            lock (lockGet)
            {
                //HighVoltage highVoltage = netWork.GetHighVotage();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    xrfRay.ClosePilotLamp();
                    //Console.WriteLine("关高压灯成功！");
                    RecordInfo("关高压灯成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("关高压灯失败！");
                    RecordInfo("关高压灯失败！" + ex.ToString());
                    return false;
                }
                return true;
            }
        }

        public override bool GetData(int[] data)
        {
            lock (lockGet)
            {
                RecordInfo("GetData开始");
                //XRFNetLib.Detector detector = netWork.GetDetector();
                if(detector == null)
                    detector = netWork.GetDetector();
                detector.SetCallback(this);
                detector.BeginRead();
                RecordInfo("GetData结束");
                return true;
            }
        }

        public override bool MotorControl(int index, int dir, int step, bool swtStop, int speedGear)
        {
            //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
            //try
            //{
            //    iMotorControl.Move((byte)index, (byte)dir, (ushort)step, Convert.ToByte(!swtStop), (byte)speedGear);
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
            //return true;
            lock (lockGet)
            {
                //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    ushort Coef2 = 0;
                    ushort Coef1 = 0;
                    //if (WorkCurveHelper.DeviceCurrent.MotorZCode == index)
                    //{
                    //    double dutyRatio = 0;
                    //    if (dir == 0)
                    //        dutyRatio = WorkCurveHelper.DeviceCurrent.MotorZDutyRatioUp;
                    //    else
                    //        dutyRatio = WorkCurveHelper.DeviceCurrent.MotorZDutyRatioDown;
                    //    Coef1 = (ushort)(dutyRatio * 65535);

                    //}
                    ////2014-10-14 追加6000B网口的偏移
                    //else if (WorkCurveHelper.DeviceCurrent.HasChamber && WorkCurveHelper.DeviceCurrent.ChamberElectricalCode == index)
                    //{
                    //    Chamber chtemp = WorkCurveHelper.DeviceCurrent.Chamber.ToList().Find(w => w.Step == step);
                    //    if (chtemp == null) chtemp = WorkCurveHelper.DeviceCurrent.Chamber.ToList().Find(w => w.Num == WorkCurveHelper.DeviceCurrent.ChamberMaxNum);
                    //    Coef1 = (ushort)(chtemp != null ? chtemp.StepCoef1 : 0);
                    //    Coef2 = (ushort)(chtemp != null ? chtemp.StepCoef2 : 0);
                    //}

                    byte[] contbyte = BitConverter.GetBytes(step);
                    Coef1 = BitConverter.ToUInt16(contbyte, 2);
                    Coef2 = BitConverter.ToUInt16(contbyte, 0);
                    xrfRay.MotorMove((byte)index, (byte)dir, (ushort)step, Convert.ToByte(!swtStop), (byte)speedGear, Coef1, Coef2);
                    RecordInfo("MotorControlMove成功+ " + speedGear.ToString());
                }
                catch (Exception ex)
                {
                    RecordInfo("MotorControlMove失败" + ex.ToString());
                    return false;
                }
                return true;
            }
        }
        //光电开关
        public bool MotorAtTouch(int index)
        {
            try
            {
                //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                if (xrfRay == null)
                    xrfRay = netWork.GetRayTube();
                short pInfo = 0;
                xrfRay.GetSwitchInfo(out pInfo, 2000);
                pInfo = (short)(32767 + pInfo);
                RecordInfo("MotorAtTouch:"+(pInfo & (0x01 << index)));
                if ((pInfo & (0x01 << index)) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                RecordInfo("MotorAtTouch失败! " + ex.ToString());
                Log.Error("MotorControl GetSwitch " + "error");
                return true;
            }

        }

        public override bool MotorIsIdelAll(int X,int Y)
        {
            lock (lockGet)
            {
                try
                {
                    //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    byte bValue = 0;
                    xrfRay.GetMotorState(out bValue, 2000);
                    //Console.WriteLine("MotorIsIdel " + bValue);
                    RecordInfo("MotorIsIdel:" + bValue);
                    if (((bValue & (0x01 << X)) == 0) && ((bValue & (0x01 << Y))== 0))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    //RecordInfo("MotorIsIdel成功");
                }
                catch (Exception ex)
                {
                    RecordInfo("MotorIsIdel失败!" + ex.ToString());
                    Log.Error("MotorControl GetState " + "error");
                    return false;
                }
            }
        }

        public override bool MotorIsIdel(int index)
        {
            lock (lockGet)
            {
                try
                {
                    //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    byte bValue = 0;
                    xrfRay.GetMotorState(out bValue, 2000);
                    //Console.WriteLine("MotorIsIdel " + bValue);
                    RecordInfo("MotorIsIdel:" + bValue);
                    if ((bValue & (0x01 << index)) == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    //RecordInfo("MotorIsIdel成功");
                }
                catch (Exception ex)
                {
                    RecordInfo("MotorIsIdel失败!" + ex.ToString());
                    Log.Error("MotorControl GetState " + "error");
                    return false;
                }
            }
        }

        public override bool GetMotorInfo(ref int info)
        {
            return true;
        }

        public override bool GetParams(ref int Voltage, ref int Current, ref int Temperature, ref int Vacuum, ref bool IsOpen)
        {
            return true;
        }

        public override void setFPGAParam(byte bBaseResume, byte bBaseLimit, byte bHeapUP, byte bRate, byte bCoarse, uint uFine, uint uTime, byte bUPTime, byte bWidthTime, byte bSlowLimit, double dIntercept)
        {
            lock (lockGet)
            {
                //XRFNetLib.Detector detector = netWork.GetDetector();
                if(detector == null)
                    detector = netWork.GetDetector();
                try
                {
                    detector.Config(bBaseResume, bBaseLimit, bHeapUP, bRate, bCoarse, uFine, uTime, bUPTime, bWidthTime, bSlowLimit, dIntercept);
                    RecordInfo("设置FPGA参数成功！");
                    //Console.WriteLine("设置FPGA参数成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("设置FPGA参数失败！");
                    RecordInfo("设置FPGA参数失败！" + ex.ToString());
                    Log.Error("detector SetConfig " + "error");
                }
            }
        }

        public override bool SetParam(int tubVoltage, int tubCurrent, int gain, int fineGain)
        {
            return true;
        }

        public override void setParam(double tubVoltage, double tubCurrent)
        {
            lock (lockGet)
            {
                //XRFRay xrfRay = netWork.GetRayTube();
                if(xrfRay == null)
                    xrfRay = netWork.GetRayTube();
                try
                {
                    xrfRay.SetConfig(Convert.ToUInt16((tubVoltage * 4095.0 / 50.0) / 2), Convert.ToUInt16((tubCurrent * 4095.0 / 1000.0) / 2));
                    //Console.WriteLine("设置管压管流成功！");
                    RecordInfo("设置管压管流成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("设置管压管流失败！");
                    RecordInfo("设置管压管流失败！" + ex.ToString());
                    Log.Error("xrfRay SetVoltage  " + "error");
                }
            }
        }

        public override void getParam(out double uVoltage, out double uCurrent, out int iUncover)
        {
            lock (lockGet)
            {
                //XRFRay xrfRay = netWork.GetRayTube();
                if(xrfRay == null)
                    xrfRay = netWork.GetRayTube();
                uVoltage = TempVoltage;
                uCurrent = TempCurrent;
                iUncover = TempUncover;
                ushort ushVol, ushCur;
                try
                {
                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    //sw.Start();
                    xrfRay.GetConfig(out ushVol, out ushCur, out iUncover, 2000);
                    //sw.Stop();
                    //string showtime = sw.Elapsed.TotalMilliseconds.ToString();
                    //Console.WriteLine("启动占用时间--：{0}ms", sw.Elapsed.TotalMilliseconds);

                    uVoltage = (ushVol / 4095.0) * 50;
                    uCurrent = (ushCur / 4095.0) * 1000;
                    //uVoltage = (uVoltage / 4095.0) * 50;
                    //uCurrent = (uCurrent / 4095.0) * 1000;
                    TempVoltage = uVoltage;
                    TempCurrent = uCurrent;
                    TempUncover = iUncover;
                    //Console.WriteLine("读取管压管流成功！" + TempVoltage + "," + TempCurrent + "," + TempUncover);
                    RecordInfo("读取管压管流成功！" + TempVoltage + "," + TempCurrent + "," + TempUncover);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("读取管压管流失败！");
                    RecordInfo("读取管压管流失败！" + ex.ToString());
                    uVoltage = TempVoltage;
                    uCurrent = TempCurrent;
                    iUncover = TempUncover;
                    Log.Error("xrfRay GetVoltage  " + "error");
                }
            }
        }

        public override void AllowUncover(bool allowUncover)
        {
            lock (lockGet)
            {
                try 
	            {	        
		            //HighVoltage highVoltage = netWork.GetHighVotage();
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    if (allowUncover)
                    {
                        xrfRay.AllowUncover();
                    }
                    else
                    {
                        xrfRay.ForbidUncover();
                    }
                    RecordInfo("AllowUncover成功！");
	            }
	            catch (Exception ex)
	            {
                    //Console.WriteLine("AllowUncover执行异常！");
                    RecordInfo("AllowUncover执行异常！" + ex.ToString());
	            }

             }
        }

        //public override int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods)
        //{
        //   return KeyAPI.GetKeyInfo(company, mode, serialNum, ref LeftSencods);
        //}
        public override int GetDeviceKeyInfo()//0-不加密;1-加密;-1-没有取到信息
        {
            lock (lockGet)
            {
                int? res = null;
                uint puse = 1;
                uint timeout = 4000;
                uint[] pVersion = { 1, 1, 1, 1 };
                string sUse = string.Empty;
                string sVersion = string.Empty;
                try
                {
                    //Apparatus ss = netWork.GetApparatus();
                    if(ss == null)
                        ss = netWork.GetApparatus();
                    ss.CheckVersion(out puse, timeout);
                    sUse = puse.ToString();
                    RecordInfo("加密板ss.CheckVersion成功！值为" + sUse);
                }
                catch(Exception ex)
                {
                    res = -1;
                    puse = 0;//没有获取到信息,目前是不加密
                    sUse = "";
                    RecordInfo("加密板ss.CheckVersion失败！" + ex.ToString());
                    Log.Error("Arm CheckVersion  " + "error");
                }
                try
                {
                    //XRFRay xrfRay = netWork.GetRayTube();
                    if(xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    xrfRay.CheckVersion(pVersion, timeout);
                    foreach (var a in pVersion)
                    {
                       sVersion += a.ToString();
                    }
                    if (sVersion == "45445836") sVersion = "SuccessVersion";
                    RecordInfo("接口板xrfRay.CheckVersion成功！值为"+sVersion);
                }
                catch(Exception ex)
                {
                    res = res.HasValue ? (res.Value + (-2)) : -2;
                    sVersion = "";
                    RecordInfo("接口板xrfRay.CheckVersion失败！" + ex.ToString());
                    Log.Error("IFB CheckVersion  " + "error");
                }
                using (FileStream fileStream = new FileStream(Application.StartupPath + @"\Version.txt", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fileStream))
                    {
                        sw.WriteLine("ARMVersion:" + sUse + "\r\n" + "IFBVersion:" + sVersion);
                    }
                }
                if (sVersion != "SuccessVersion") return -1;
                return res.HasValue ? res.Value : (int)puse;
            }
        }

        #region 获取加密狗信息

        [DllImport("TRUSBDev.Dll4.Usb2.0.dll", EntryPoint = "GetKeyInfo", CharSet = CharSet.Ansi)]
        private static extern int GetKeyInfo4(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods);

        public override int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods)
        {
            return GetKeyInfo4(company, mode, serialNum, ref LeftSencods);
        }

        #endregion

        public override void OpenElectromagnet()
        {
            lock (lockGet)
            {
                RecordInfo("OpenElectromagnet开始！");
                //HeightLaser heightLaser = netWork.GetHeightLaser();
                if (xrfRay == null)
                    xrfRay = netWork.GetRayTube();
                xrfRay.OpenElectromagnet();
                RecordInfo("OpenElectromagnet成功！");
            }
        }

        public override void CloseElectromagnet()
        {
            lock (lockGet)
            {
                RecordInfo("CloseElectromagnet开始！");
                //HeightLaser heightLaser = netWork.GetHeightLaser();
                if (xrfRay == null)
                    xrfRay = netWork.GetRayTube();
                xrfRay.CloseElectromagnet();
                RecordInfo("CloseElectromagnet成功！");
            }
        }

        public override void OpenMagnet()
        {
            lock (lockGet)
            {
                RecordInfo("OpenMagnet开始！");
                //Electromagnet electromagnetClass = netWork.GetElectromagnet();
                if(electromagnetClass == null)
                    electromagnetClass = netWork.GetElectromagnet();
                electromagnetClass.Open();
                RecordInfo("OpenMagnet成功！");
            }
        }

        public override void CloseMagnet()
        {
            lock (lockGet)
            {
                RecordInfo("CloseMagnet开始！");
                //Electromagnet electromagnetClass = netWork.GetElectromagnet();
                if(electromagnetClass == null)
                    electromagnetClass = netWork.GetElectromagnet();
                electromagnetClass.Close();
                RecordInfo("CloseMagnet成功！");
            }
        }

        public override void ConsolePrint(string str)
        {
            lock (lockGet)
            {
                netWork.ShowConsole(1);
            }
        }

        #region INetworkCallBack 成员
        public int ConnectFailedCount = 0;
        public void OnDisconnect()
        {

            new Action(() =>
            {
                lock (lockGet)
                {
                    RecordInfo("OnDisconnect开始！");
                    ConnectState = false;
                    WorkCurveHelper.specMessage.localMesage.Add(new MessageFormat(Info.NetDeviceDisConnection, 0));
                    if (WorkCurveHelper.UnderReconnecting && (WorkCurveHelper.CurrentReconnectTime++ < WorkCurveHelper.RetryGetVersionTimes))
                    {
                        WorkCurveHelper.specMessage.localMesage.Add(new MessageFormat(Info.DeviceConnecting + "(" + WorkCurveHelper.CurrentReconnectTime + ")", 0));
                        Connect();
                        return;
                    }
                    HardwareDog.SNClose();
                    CheckWhetherDevInfoGot();
                    RecordInfo("OnDisconnect结束！");
                }
            }).BeginInvoke(null, null);
            
        }

        public void OnRemoteDisconnected()
        {
            lock (lockGet)
            {
                MessageFormat message = new MessageFormat(Info.NetDeviceDisConnection, 0);
                WorkCurveHelper.specMessage.localMesage.Add(message);
                ConnectState = false;
            }
        }

        public void OnRemoteServerDisconnected()
        {
            lock (lockGet)
            {
                MessageFormat message = new MessageFormat(Info.NetDeviceDisConnection, 0);
                WorkCurveHelper.specMessage.localMesage.Add(message);
                ConnectState = false;
            }
        }

        #endregion

        #region IDetectorCallBack 成员

        public void OnRead(uint uQuickCount, uint uSlowCount, uint uSystemTime, uint uLocalTime, DetectData pData)
        {
            lock (lockGet)
            { 
                ushort[] dataTemp = new ushort[specData.Length];
                for (int i = 0; i < dataTemp.Length; i++)
                {
                    pData.At((ushort)i, out dataTemp[i]);
                    specData[i] += dataTemp[i];
                }
                this.uQuickCount = uQuickCount;
                this.uSlowCount = uSlowCount;
                this.uSystemTime = uSystemTime;
                this.uLoaclTime = uLocalTime;
                if (pData != null)
                {
                    Marshal.ReleaseComObject(pData);
                    pData = null;
                }
                NetReadOK = true;
            }
        }

        #endregion
        public event EventHandler GetConnect;
        #region INetworkCallBack 成员

        public void OnConnected()
        {
            new Action(() =>
            {
                RecordInfo("OnConnected线程开始！");
                lock (lockGet)
                {
                    //ConnectState = true; //判断是否连接上的逻辑修改为: 连接上并且可以读取到版本号, 所以放到成功读取版本号后赋值
                    if (GetConnect != null) GetConnect(null, null);
                }
                RecordInfo("OnConnected线程结束！");
            }).BeginInvoke(null, null);
        }

        public void OnRemoteConnected()
        {
            //throw new NotImplementedException();
        }

        #endregion

        public override void OpenXRayTubHV()
        {
            lock (lockGet)
            {
                RecordInfo("OpenXRayTubHV开始！");
                //XRFRay xrfRay = netWork.GetRayTube();
                if(xrfRay == null)
                    xrfRay = netWork.GetRayTube();
                xrfRay.bXRayTubeSel(1);
                RecordInfo("OpenXRayTubHV结束！");
            }
        }

        public override void CloseXRayTubHV()
        {
            lock (lockGet)
            {
                RecordInfo("CloseXRayTubHV开始！");
                //XRFRay xrfRay = netWork.GetRayTube();
                if(xrfRay == null)
                    xrfRay = netWork.GetRayTube();
                xrfRay.bXRayTubeSel(0);
                RecordInfo("CloseXRayTubHV结束！");
            }
        }

        public override bool SetSurfaceSource(ushort firstLight, ushort secondLight, ushort thirdLight, ushort fourthLight)
        {
            lock (lockGet)
            {
                //XRFNetLib.SurfaceSource surfaceSource = netWork.GetSurfaceSource();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    xrfRay.SetSurfaceLight(firstLight, secondLight, thirdLight, fourthLight);
                    RecordInfo("设置面光源成功！");
                }
                catch(Exception ex)
                {
                    RecordInfo("设置面光源失败！" + ex.ToString());
                    Log.Error("surfaceSource SetLight  " + "error");
                    return false;
                }
                return true;
            }
        }

        //public override event EventHandler GetConnect;

        public override bool OpenDevice()
        {
            return true;
        }

        private uint tempupvacuum = 0;
        private uint tempdownvacuum = 0;
        public override bool GetDoubleVacuum(uint uType, out uint pUpVacuum, out uint pDownVacuum)
        {
            lock (lockGet)
            {
                //XRFNetLib.VacuumPump vacuumPump = netWork.GetVacuumPump();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    //if (xrfRay == null)
                    //    xrfRay = netWork.GetRayTube();
                    xrfRay.GetDoubleVacuum(uType, out pUpVacuum, out pDownVacuum, 2000);
                    //vacuumPump.GetDoubleVacuum(ref pUpVacuum, ref pDownVacuum, 300);
                    tempupvacuum = pUpVacuum;
                    tempdownvacuum = pDownVacuum;
                    RecordInfo("GetDoubleVacuum成功！");
                }
                catch (Exception ex)
                {
                    pUpVacuum = tempupvacuum;
                    pDownVacuum = tempdownvacuum;
                    RecordInfo("GetDoubleVacuum失败！" + ex.ToString());
                    Log.Error("vacuumPump GetDoubleVacuum  " + "error");
                    return false;
                }
                return true;
            }
        }

        public override LightStatus GetLightShutState(int index)
        {
            lock (lockGet)
            {
                try
                {
                    //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    byte bValue = 0;
                    xrfRay.GetMotorState(out bValue, 2000);
                    if ((bValue & (0x01 << (index + 1))) == 0)
                    {
                        if ((bValue & (0x01 << (index + 2))) == 0)
                            return LightStatus.Middle;
                        else
                            return LightStatus.Close;
                    }
                    else
                    {
                        if ((bValue & (0x01 << (index + 2))) == 0)
                            return LightStatus.Open;
                        else
                            return LightStatus.Middle;
                    }
                    RecordInfo("GetLightShutState成功！");
                }
                catch (Exception ex)
                {
                    RecordInfo("GetLightShutState失败！" +ex.ToString());
                    Log.Error("iMotorControl GetState  " + "error");
                    return LightStatus.Fail;
                }
            }
        }

        public override void GetChamberStatus(ref int steps, ref int chambeIndex, ref bool istruestep, ref bool initialed, ref bool isbusy)
        {
            lock (lockGet)
            {
                //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    byte[] pstate = new byte[2];
                    xrfRay.GetChamberState(pstate, (uint)pstate.Length, 2000);
                    steps = 0;
                    chambeIndex = pstate[0];
                    istruestep = (pstate[1] == 0);
                    isbusy = false;
                    RecordInfo("GetChamberStatus成功！");
                }
                catch (Exception ex)
                {
                    RecordInfo("GetChamberStatus失败！" + ex.ToString());
                    Log.Error("iMotorControl GetChamberState  " + "error");
                }
            }
        }

        public void SetLaserMode(bool isManual)
        {
            lock (lockGet)
            {
                //XRFNetLib.HeightLaser heightLaser = netWork.GetHeightLaser();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    byte[] pstate = new byte[1];
                    xrfRay.SetLaserMode((byte)(isManual ? 1 : 0));
                    RecordInfo("SetLaserMode成功！");
                }
                catch (Exception ex)
                {
                    RecordInfo("SetLaserMode失败！" + ex.ToString());
                    Log.Error("heightLaser SetLaserMode  " + "error");
                }
            }
        }

        public override int ResetChamber(int ID, int DirectionFlag, int DefSpeed)
        {
            lock (lockGet)
            {
                RecordInfo("ResetChamber开始！");
                int chamindex = -1;
                MotorControl(ID, DirectionFlag, WorkCurveHelper.DeviceCurrent.Chamber[WorkCurveHelper.DeviceCurrent.ChamberMaxNum - 1].Step + 1, true, DefSpeed);
                int i = 0;
                do
                {
                    Thread.Sleep(1000);
                    if (i > 30)
                    {
                        MotorControl(ID, DirectionFlag, 0, true, DefSpeed);
                        break;
                    }
                    i++;
                } while (!MotorIsIdel(ID));
                int steps = 0;
                bool istruestep = true;
                bool isinitialed = false;
                bool isbusy = false;
                GetChamberStatus(ref steps, ref chamindex, ref istruestep, ref isinitialed, ref isbusy);
                RecordInfo("ResetChamber成功！");
                return chamindex;
            } 
        }
        public override bool LockHV(bool bLock)
        {
            return false;
        }
        public override void OpenHeightLaser(bool bOpen)
        {
            lock (lockGet)
            {
                //XRFNetLib.HeightLaser heightLaser = netWork.GetHeightLaser();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    byte[] pstate = new byte[1];
                    xrfRay.OpenLaserControl((byte)(bOpen ? 1 : 0));
                    RecordInfo("OpenHeightLaser成功！");
                }
                catch (Exception ex)
                {
                    RecordInfo("OpenHeightLaser失败！" + ex.ToString());
                    Log.Error("heightLaser GetHeightLaser  " + "error");
                }
            }
        }

        public override void GetSwitch()
        {
            try
            {
                //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                if (xrfRay == null)
                    xrfRay = netWork.GetRayTube();
                short value = 0;
                xrfRay.GetSwitchInfo(out value, 2000);
               

            }
            catch (Exception ex)
            {
               
            }

        }
      

        public override void MoveZAutoMotor(int iSpeed)
        {
            lock (lockGet)
            {
                //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                try
                {
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    byte[] pstate = new byte[1];
                    xrfRay.MoveZAutoMotor((byte)iSpeed);
                    RecordInfo("MoveZAutoMotor成功！");
                }
                catch (Exception ex)
                {
                    RecordInfo("MoveZAutoMotor失败！" + ex.ToString());
                    Log.Error("iMotorControl MoveZAutoMotor  " + "error");
                }
            }
        }
        byte[] byteGetDMCABytes = null;
        public void GetDMCADatas()
        {
            lock (lockGet)
            {
                //XRFNetLib.Detector detector = netWork.GetDetector();
                try
                {
                    if (detector == null)
                        detector = netWork.GetDetector();
                    short intGetBytesLen = 0;
                    if (byteGetDMCABytes == null)
                        byteGetDMCABytes = new byte[16390];
                    else Array.Clear(byteGetDMCABytes, 0, byteGetDMCABytes.Length);
                    detector.GetDMCADatas(2000, byteGetDMCABytes, ref intGetBytesLen);

                    this.uQuickCount = (uint)((byteGetDMCABytes[48] * 256 + byteGetDMCABytes[49]) * 65536 + (byteGetDMCABytes[50] * 256 + byteGetDMCABytes[51]));
                    this.uSlowCount = (uint)((byteGetDMCABytes[52] * 256 + byteGetDMCABytes[53]) * 65536 + (byteGetDMCABytes[54] * 256 + byteGetDMCABytes[55]));
                    this.uSystemTime = (uint)((byteGetDMCABytes[56] * 256 + byteGetDMCABytes[57]) * 65536 + (byteGetDMCABytes[58] * 256 + byteGetDMCABytes[59]));
                    this.uLoaclTime = (uint)((byteGetDMCABytes[60] * 256 + byteGetDMCABytes[61]) * 65536 + (byteGetDMCABytes[62] * 256 + byteGetDMCABytes[63]));

                    int DataStartIndex = 64;
                    for (int i = 0; i < dataLength; i++)
                    {
                        DataStartIndex=64+2*i;
                        specData[i] += (ushort)(byteGetDMCABytes[DataStartIndex] * 256 + byteGetDMCABytes[DataStartIndex + 1]);
                    }
                    //Console.WriteLine("时间：" + this.uSystemTime / 1000);
                    RecordInfo("DMCAData成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("设置Dp5FPGA参数失败！");
                    RecordInfo("DMCAData失败！" + ex.ToString());
                    Log.Error("detector GetDMCADatas  " + "error");
                }
            }

        }

        public  void SetDpp100Parameter(byte[] byteDp5Cmd, short intCmdLen)
        {
            lock (lockGet)
            {
                //XRFNetLib.Detector detector = netWork.GetDetector();
                try
                {
                    if(detector == null)
                        detector = netWork.GetDetector();
                    detector.SetDpp100Parameter(byteDp5Cmd, intCmdLen);
                    RecordInfo("SetDpp100Parameter成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("设置Dp5FPGA参数失败！");
                    RecordInfo("SetDpp100Parameter失败！" +ex.ToString());
                    Log.Error("detector SetDpp100Parameter  " + "error");
                }
            }
        }
        public void GetDpp100Datas(byte[] byteDp5Cmd, short intDp5CmdLen, byte[] byteGetBytes, ref short intGetBytesLen, uint uTimeOut)
        {
            lock (lockGet)
            {
                //XRFNetLib.Detector detector = netWork.GetDetector();
                try
                {
                    if(detector == null)
                        detector = netWork.GetDetector();
                    detector.GetDpp100Datas(byteDp5Cmd, intDp5CmdLen, 2000, byteGetBytes, ref intGetBytesLen);
                    RecordInfo("GetDpp100Data成功！");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("设置Dp5FPGA参数失败！");
                    RecordInfo("GetDpp100Data失败！" + ex.ToString());
                    Log.Error("detector GetDpp100Datas  " + "error");
                }
            }
        }

        #region 一键测试功能
        public bool IsTestButtonPressed()
        {
            lock (lockGet)
            {
                try
                {
                    //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                    if (xrfRay == null)
                        xrfRay = netWork.GetRayTube();
                    //if (xrfRay == null)
                    //    xrfRay = netWork.GetRayTube();
                    byte tmpValue = 0;
                    xrfRay.GetPushButtonInfo(out tmpValue, 800);
                   // RecordInfo("GetPushButtonInfo成功！");
                    Thread.Sleep(100);
                    return tmpValue == 0xFF;
                }
                catch (Exception ex)
                {
                    //RecordInfo("GetPushButtonInfo失败！" + ex.ToString());
                    Log.Error("iMotorControl GetPushButtonInfo  " + "error");
                    return false;
                }
            }

        }

        public bool ClearPressButtonInfo()
        {
            try
            {
                //XRFNetLib.MotorControl iMotorControl = netWork.GetMotor();
                if (xrfRay == null)
                    xrfRay = netWork.GetRayTube();
                //if (xrfRay == null)
                //    xrfRay = netWork.GetRayTube();
                xrfRay.ClearPushButtonInfo();
                RecordInfo("ClearPushButtonInfo成功！");
                return true;
            }
            catch (Exception ex)
            {
                RecordInfo("ClearPushButtonInfo失败！" + ex.ToString());
                Log.Error("iMotorControl ClearPushButtonInfo  " + "error");
                return false;
            }
        }
        #endregion

        public void CheckWhetherDevInfoGot()
        {
            WorkCurveHelper.CurrentReconnectTime = 0;
            WorkCurveHelper.UnderReconnecting = false;
            if (Port.ActionAfterDeviceInfoChecked != null)
                Port.ActionAfterDeviceInfoChecked();
            if (!WorkCurveHelper.IsDeviceInfoGot)
            {
                if (Port.ActionAfterDeviceInfoNotGot == null) //必须要实现该接口, 关闭主窗体
                {
                    throw new ArgumentNullException("ActionAfterDeviceInfoNotGot");
                }
                new Action(() =>
                {
                    WorkCurveHelper.Ewh.WaitOne(10000);
                    Port.ActionAfterDeviceInfoNotGot();
                }).BeginInvoke(null, null);
            }
            else
            {
                MessageFormat message = new MessageFormat(Info.NetDeviceDisConnection, 0);
                WorkCurveHelper.specMessage.localMesage.Add(message);
            }
        }

        private void RecordInfo(string content)
        {
            if (DoRecordingToFile)
            {
                lock (recordLocker)
                {
                    try
                    {
                        using (FileStream fileStream = new FileStream(Application.StartupPath + @"\NetPortRecord.txt", FileMode.Append))
                        {
                            using (StreamWriter sw = new StreamWriter(fileStream))
                            {
                                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + ": " + content);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("RecordInfo Exception: " + ex.ToString());
                    }
                    
                }
               
            }

            if (DoConsoleWriteLine)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + ": " + content);
            }
            
        }
    }
}
