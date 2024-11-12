using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using Lephone.Data;
using Lephone.Data.Common;
using Lephone.Data.Definition;
using Lephone.Data.SqlEntry;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Define;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
namespace Skyray.EDX.Common.Component
{
    public class NetPortImplement : DeviceInterface
    {
        private QualeElementOperation QualeElementOperation;
        private NewNetPortProtocol newNetPortProtocol = null;
        private NetPortType _netPortType = NetPortType.DMCA;
        public NetPortType NetPortType
        {
            get { return _netPortType; }
            set
            {
                _netPortType = value;
                if (_netPortType == NetPortType.DPP100) newNetPortProtocol = new NewNetPortProtocol(4096);
                else newNetPortProtocol = null;
            }
        }


        private double[] data;

        private int[] peakPositions;

        private int cover = 0;

        private Thread thread = null;

        private BackgroundWorker getParamsWorker = null;

        private bool ThreadRunning = false;

        private double pauseTime = 0;

        private double calcTimeCountRate = 0;
        public override void GetReturnParams()
        {
            //if (State != DeviceState.Idel
            //    || ThreadRunning) return;//2014-11-07 在取谱或不存在真空泵的情况下不执行取真空度的操作，每秒创建线程耗费资源

            //Action action = this.GetPar;
            //AsyncCallback callback = ar =>
            //    {
            //        ThreadRunning = false;
            //    };
            //action.BeginInvoke(callback,null);


            //if (State != DeviceState.Idel
            //    || thread != null) return;//2014-11-07 在取谱或不存在真空泵的情况下不执行取真空度的操作，每秒创建线程耗费资源
            //thread = new Thread(new ThreadStart(GetPar));
            //thread.IsBackground = true;
            //thread.Priority = ThreadPriority.Highest;
            //thread.Start();

            if (port == null
                || !((port as NetPort).ConnectState)
                || State != DeviceState.Idel)
                // || WorkCurveHelper.TestOnButtonPressedEnabled)

                //||
                //  (!(WorkCurveHelper.TestOnCoverClosedEnabled
                //  || WorkCurveHelper.TestOnButtonPressedEnabled
                //  || (WorkCurveHelper.DeviceCurrent.HasVacuumPump && Pump != null && Pump.Exist))))
                return;
            if (getParamsWorker == null)
            {
                getParamsWorker = new BackgroundWorker();
                getParamsWorker.DoWork -= getParamsWorker_DoWork;
                getParamsWorker.DoWork += getParamsWorker_DoWork;
                Console.WriteLine("getparam new");
            }
            if (getParamsWorker.IsBusy)
            {
                Console.WriteLine("getparam busy");
                return;
            }
            getParamsWorker.RunWorkerAsync();
        }
        public override void SetDp5Cfg()
        {
            if (_netPortType != NetPortType.DPP100 || newNetPortProtocol == null) return;
            if (!IsConnectDevice() || !port.ConnectState)
            {
                Msg.Show(Info.NetDeviceDisConnection);
                StopFlag = true;
                connect = DeviceConnect.DisConnect;
                PostMessage(OwnerHandle, WM_EndTest, true, (int)usedTime);
                return;
            }
            newNetPortProtocol.ReadCfgFromFile(AppDomain.CurrentDomain.BaseDirectory + "Dpp100.cfg");
            newNetPortProtocol.PresetTestTime = DeviceParam.PrecTime;
            newNetPortProtocol.FineGain = InitParam.FineGain;
            byte[] temps = newNetPortProtocol.SetConfigCommand;
            ((NetPort)port).SetDpp100Parameter(temps, (short)temps.Length);
            Thread.Sleep(300);
            temps = newNetPortProtocol.DisabelMCACommand;
            ((NetPort)port).SetDpp100Parameter(temps, (short)temps.Length);
            Thread.Sleep(200);
            temps = newNetPortProtocol.ClearSpectrumCommand;
            ((NetPort)port).SetDpp100Parameter(temps, (short)temps.Length);
            Thread.Sleep(200);
        }

        object objj = new object();
        private void GetPar()
        {
            if (port != null && (port as NetPort).ConnectState && State == DeviceState.Idel)
            {

                if (WorkCurveHelper.bCustomDevice || WorkCurveHelper.bOpenOutSample || (WorkCurveHelper.IsUseElect && WorkCurveHelper.IsContinueVol))
                {
                    double dVolgate = 0d, dCurrent = 0d;
                    int iCover = 0;
                    port.getParam(out dVolgate, out dCurrent, out iCover); //icover 1是仪器盖子已打开。
                    ReturnCoverClosed = iCover != 0;    //returncoverclosed =true 盖子打开了
                }

                //double dVolgate = 0d, dCurrent = 0d;
                //int iCover = 0;
                ////System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                ////sw.Start();
                //port.getParam(out dVolgate, out dCurrent, out iCover); //icover 1是仪器盖子已打开。
                //ReturnCoverClosed = iCover != 0;    //returncoverclosed =true 盖子打开了
                //ReturnVoltage = dVolgate * WorkCurveHelper.DeviceCurrent.VoltageScaleFactor;
                //double currentrate = 1;
                //if (InitParam != null ) currentrate = InitParam.CurrentRate;
                //ReturnCurrent = dCurrent * currentrate * WorkCurveHelper.DeviceCurrent.CurrentScaleFactor;

                //  // 返回线圈值  0x26  
                uint upvacu = 0;
                uint downvacu = 0;
                if (WorkCurveHelper.isShowEncoder)
                {
                    Thread.Sleep(20);
                    port.GetDoubleVacuum((uint)0, out upvacu, out downvacu);   //upvacu*65535
                    Console.WriteLine("upvacu:" + upvacu.ToString() + " downvacu:" + downvacu.ToString());
                    ReturnVacuumDown = (double)upvacu;

                    if (ReturnVacuumDown > 0) //线圈值
                    {
                        GetEncoderValue(ref ReturnEncoderValue, ref ReturnEncoderHeight);
                        ReturnEncoderValue = Math.Round(ReturnEncoderValue, 1);
                    }
                    else
                        ReturnEncoderValue = 0;
                }
                else
                    ReturnEncoderValue = 0;

                // Thread.Sleep(200);

                if (WorkCurveHelper.TestOnButtonPressedEnabled && !WorkCurveHelper.DeviceCurrent.HasChamber)//一键测试信息
                {
                    Thread.Sleep(20);
                    TestButtonPressed = (port as NetPort).IsTestButtonPressed();
                }
                Console.WriteLine("TestButtonPressed:" + TestButtonPressed.ToString());

            }


        }

        private void GetEncoderValue(ref double retEncodervalue, ref double retEncoderHeiht)
        {
            string[] Encodervalue = new string[1];
            string[] EncoderX = new string[1] { "x" };
            //double retEncodervalue = 0;
            Encodervalue[0] = ReturnVacuumDown.ToString();
            if (WorkCurveHelper.DeviceCurrent.EncoderFormula != "x")
            {
                // Encodervalue[0] = ReturnVacuumDown.ToString();
                // EncoderX[0] = "x";
                TabControlHelper.CustomFieldByFortum(WorkCurveHelper.DeviceCurrent.EncoderFormula, EncoderX, Encodervalue, 0, out retEncodervalue);
                if (retEncodervalue < 0) retEncodervalue = 0;

            }
            else
            {
                retEncodervalue = ReturnVacuumDown;
                if (retEncodervalue < 0) retEncodervalue = 0;
            }
            TabControlHelper.CustomFieldByFortum("0.0147*x", EncoderX, Encodervalue, 0, out retEncoderHeiht);
            if (retEncoderHeiht < 0) retEncoderHeiht = 0;
        }


        //private double GetEncoderValue()
        //{
        //    string[] Encodervalue = new string[1];
        //    string[] EncoderX = new string[1];
        //    double retEncodervalue = 0;

        //    if (WorkCurveHelper.DeviceCurrent.EncoderFormula != "x")
        //    {
        //        Encodervalue[0] = ReturnVacuumDown.ToString();
        //        EncoderX[0] = "x";
        //        TabControlHelper.CustomFieldByFortum(WorkCurveHelper.DeviceCurrent.EncoderFormula, EncoderX, Encodervalue, 0, out retEncodervalue);
        //    }
        //    else
        //    {
        //        retEncodervalue = ReturnVacuumDown;
        //    }

        //    return retEncodervalue;
        //}


        public NetPortImplement()
        {
            DbObjectList<QualeElement> QualeElementlIST = QualeElement.FindAll();
            if (QualeElementlIST.Count > 0)
            { QualeElement = QualeElementlIST[0]; }
            QualeElementOperation = new QualeElementOperation();
        }

        void getParamsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GetPar();
        }

        /// <summary>
        /// 扫谱过程
        /// </summary>
        /// <param name="obj"></param>

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


        
        public override void DoTest(object obj)
        {
            WorkCurveHelper.waitMoveStop();

            RayTube.EnableCoverSwitch(WorkCurveHelper.DeviceCurrent.IsAllowOpenCover);
            RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
            RayTube.Open();
            WorkCurveHelper.dataStore.InputDiscretes[2] = true;
            FPGAparam = WorkCurveHelper.DeviceCurrent.FPGAParams;
            if (!IsConnectDevice() || !port.ConnectState)
            {
                Msg.Show(Info.NetDeviceDisConnection);
                StopFlag = true;
                connect = DeviceConnect.DisConnect;
                PostMessage(OwnerHandle, WM_DeviceDisConnect, true, (int)usedTime);
                return;
            }
            if (WorkCurveHelper.IsUseElect)
            {
                WorkCurveHelper.deviceMeasure.interfacce.Pump.TOpen();//电磁阀置1
            }
            bool bDppErrorInTest = false;

            //port.ConnectState = true;
            uint utype = 1;
            if (WorkCurveHelper.DeviceCurrent.VacuumPumpType == VacuumPumpType.VacuumSi)
            { utype = 0; }
            else if (WorkCurveHelper.DeviceCurrent.VacuumPumpType == VacuumPumpType.Atmospheric)
            { utype = 1; }
            else
                utype = 2;
            uint ava = 0;

            cover = 0;
            DeviceState currentState = (DeviceState)obj;
            double voltage = 0;
            double current = 0;
            Decre1 = new int[Data.Length];
            Time1 = 0;
            usedTime = 0;
            HasRecord = false;
            CountRating = 0;
            Count1 = 0;
            double limit = (int)(WorkCurveHelper.FastLimit * 6.4 / WorkCurveHelper.DeviceCurrent.FPGAParams.PeakingTime);
            #region  //未整理逻辑混乱
            if (WorkCurveHelper.Is3200L)
            {
                LightStatus lightStatus = LightStatus.Middle;
                do
                {
                    lightStatus = port.GetLightShutState(WorkCurveHelper.DeviceCurrent.MotorLightCode);
                }
                while (lightStatus == LightStatus.Fail);

                #region 抽真空处理
                if (Pump.Exist && (DeviceParam.IsVacuum || DeviceParam.IsVacuumDegree) && !HasVauccum)//抽真空
                {
                    if (lightStatus == LightStatus.Close || lightStatus == LightStatus.Middle)////如果光闸关闭
                    {
                        float range = 0;
                        //range=计算真空规上下腔压差
                        uint upvacuum = 0;
                        uint downvacuum = 0;
                        port.GetDoubleVacuum(utype, out upvacuum, out downvacuum);
                        range = Math.Abs(upvacuum - downvacuum);
                        if (range > 3)//压差过大
                        {
                            Pump.Open();
                            do
                            {
                                //range=计算真空规上下腔压差
                                port.GetDoubleVacuum(utype, out upvacuum, out downvacuum);
                                range = Math.Abs(upvacuum - downvacuum);
                                Thread.Sleep(900);
                            } while (range <= 3);
                        }
                        else//打开光闸
                        {
                            //port.电磁铁失效
                            //port.打开光闸
                            port.CloseElectromagnet();
                            int dir = WorkCurveHelper.DeviceCurrent.MotorLightDirect == 1 ? 0 : 1;
                            do
                            {
                                port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorLightCode, dir, 65535, true, 250 - WorkCurveHelper.DeviceCurrent.MotorLightSpeed);
                                Thread.Sleep(500);
                            }
                            while (port.GetLightShutState(WorkCurveHelper.DeviceCurrent.MotorLightCode) != LightStatus.Open);
                        }
                    }
                    //安全起见，是否再调用一次打开光闸？
                    //正式抽真空过程
                    if (DeviceParam.IsVacuum)//按时间抽真空
                    {
                        Pump.Open();
                        this.format = new MessageFormat(Info.TimePumpOpen, 0);
                        WorkCurveHelper.specMessage.localMesage.Add(this.format);
                        int pumpTime = 0;
                        while (pumpTime < DeviceParam.VacuumTime && !StopFlag)
                        {
                            Thread.Sleep(900);
                            uint upvacuum = 0;
                            uint downvacuum = 0;
                            port.GetDoubleVacuum(utype, out upvacuum, out downvacuum);
                            ReturnVacuum = upvacuum;
                            ReturnVacuumDown = downvacuum;
                            //port.GetVacuum(utype, out ava);
                            pumpTime++;
                            if (WorkCurveHelper.PumpShowProgress)
                                PostMessage(OwnerHandle, Wm_PumpStart, true, pumpTime);
                        }
                        if (WorkCurveHelper.PumpShowProgress)
                            PostMessage(OwnerHandle, Wm_PumpEnd, true, pumpTime);
                    }
                    if (DeviceParam.IsVacuumDegree)//按真空度抽真空
                    {
                        Pump.Open();
                        this.format = new MessageFormat(Info.SpacePumpOpen, 0);
                        WorkCurveHelper.specMessage.localMesage.Add(this.format);
                        do
                        {
                            uint upvacuum = 0;
                            uint downvacuum = 0;
                            port.GetDoubleVacuum(utype, out upvacuum, out downvacuum);
                            ReturnVacuum = upvacuum;
                            ReturnVacuumDown = downvacuum;
                            //port.GetVacuum(utype, out ava);
                            //ReturnVacuum = ava;
                            Thread.Sleep(900);
                        } while (ReturnVacuum > DeviceParam.VacuumDegree);
                    }
                }
                #endregion
                #region 不抽真空处理
                else//不抽真空
                {
                    if (lightStatus == LightStatus.Close || lightStatus == LightStatus.Middle)////如果光闸关闭
                    {
                        float range = 0;
                        //range=计算真空规上下腔压差
                        uint upvacuum = 0;
                        uint downvacuum = 0;
                        port.GetDoubleVacuum(utype, out upvacuum, out downvacuum);
                        range = Math.Abs(upvacuum - downvacuum);
                        if (range > 3)//压差过大
                        {
                            //this.format = new MessageFormat("上下腔压差过大，请手动放气。", 0);
                            //WorkCurveHelper.specMessage.localMesage.Add(this.format);
                            Msg.Show("上下腔压差过大，请手动放气。");
                            StopFlag = true;
                        }
                        else//打开光闸
                        {
                            //port.电磁铁失效
                            //port.打开光闸
                            port.CloseElectromagnet();
                            int dir = WorkCurveHelper.DeviceCurrent.MotorLightDirect == 0 ? 1 : 0;
                            do
                            {
                                port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorLightCode, dir, 65535, true, 250 - WorkCurveHelper.DeviceCurrent.MotorLightSpeed);
                                Thread.Sleep(500);
                            }
                            while (port.GetLightShutState(WorkCurveHelper.DeviceCurrent.MotorLightCode) != LightStatus.Open);
                        }
                    }
                    //安全起见，是否再调用一次打开光闸？
                }
                #endregion

            }
            else
            {
                if (Pump.Exist && DeviceParam.IsVacuum && !HasVauccum && !StopFlag)//按时间抽真空
                {
                    Pump.Open();
                    this.format = new MessageFormat(Info.TimePumpOpen, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(this.format);
                    int pumpTime = 0;
                    while (pumpTime < DeviceParam.VacuumTime && !StopFlag)
                    {
                        Thread.Sleep(900);
                        port.GetVacuum(utype, out ava);
                        ReturnVacuum = ava;
                        pumpTime++;
                        if (WorkCurveHelper.PumpShowProgress)
                            PostMessage(OwnerHandle, Wm_PumpStart, true, pumpTime);
                    }
                    if (WorkCurveHelper.PumpShowProgress)
                        PostMessage(OwnerHandle, Wm_PumpEnd, true, pumpTime);
                }
                if (Pump.Exist && DeviceParam.IsVacuumDegree && !HasVauccum && !StopFlag)//按真空度抽真空
                {
                    Pump.Open();
                    this.format = new MessageFormat(Info.SpacePumpOpen, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(this.format);
                    do
                    {
                        port.GetVacuum(utype, out ava);
                        ReturnVacuum = ava;
                        Thread.Sleep(500);
                    } while (ReturnVacuum > DeviceParam.VacuumDegree && !StopFlag);
                }
            }
            #endregion




            if (ExistMagnet)//电子锁
            {
                Pump.Open();
            }

            if (!StopFlag)
            {


                pauseTime = 0;
                this.format = new MessageFormat(Info.SpectrumTest, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                //if (WorkCurveHelper.DeviceCurrent.HasMotorY1 && IsSpin)
                //{
                //    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, WorkCurveHelper.DeviceCurrent.MotorY1Direct, 65535, true, WorkCurveHelper.DeviceCurrent.MotorY1Speed);
                //}

                if (_netPortType == NetPortType.DPP100)
                {

                    //禁止
                    DisableDpp100();
                    //设置时间
                    SetTestTimeValueToDpp100(DeviceParam.PrecTime);
                    SetFineToDpp100(InitParam.FineGain);
                    ClearDataDpp100();
                    //启动
                    EnableDpp100();
                }
                else
                {
                    if (WorkCurveHelper.IDemarcateTest == 2)
                    {
                        port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(1 * 65536),
                           (uint)(DeviceParam.PrecTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, 0);
                    }
                    else
                    {
                        port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(InitParam.FineGain * 65536),
                            (uint)(DeviceParam.PrecTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, FPGAparam.Intercept);
                    }
                }


                connect = DeviceConnect.Connect;
                Array.Clear(port.specData, 0, port.specData.Length);
                ClearData();
                State = DeviceState.Test;//仪器连接上后才改变其状态
            }
            bool dropData = false;
            bool IsAdjust = true;
            int adjustCountAdd = 1;
            int recfailedcout = 0;
            int disconnectCount = 0;





            //while ((usedTime + 0.1< DeviceParam.PrecTime) && (State != DeviceState.Idel) && !StopFlag)
            while ((usedTime < DeviceParam.PrecTime) && (State != DeviceState.Idel) && !StopFlag)
            // while ((State != DeviceState.Idel) && !StopFlag)
            {
                if (!IsConnectDevice() || !port.ConnectState)
                {
                    Msg.Show(Info.NetDeviceDisConnection);
                    StopFlag = true;
                    connect = DeviceConnect.DisConnect;
                    break;
                }
                //if (!port.ConnectState || !IsConnectDevice())
                //{
                //    disconnectCount++;
                //    //port.Connect();
                //    Console.WriteLine("扫谱：" + disconnectCount);
                //    Thread.Sleep(500);
                //    continue;
                //}
                disconnectCount = 0;
            Begin:
                //if (WorkCurveHelper.DeviceCurrent.HasMotorY1 && IsSpin)
                //{
                //    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, WorkCurveHelper.DeviceCurrent.MotorY1Direct, 65535, true, WorkCurveHelper.DeviceCurrent.MotorY1Speed);
                //}
                RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
                if ((!dropData) && (usedTime >= RecordDropTime) && RecordDropTime > 0.1 && IsDropTime)
                {
                    dropData = true;
                    IsDropTime = false;
                    ClearData();

                    if (WorkCurveHelper.bCurrentInfluenceGain && WorkCurveHelper.IsFirstInfluenceGain)
                    {
                        try
                        {

                            double outFinGain = 0;
                            calcTimeCountRate = ReturnCountRate;
                            bool isCalcSuccess = ExpressionHelper.ComputeRPN(InitParam.ExpressionFineGain.Replace("x", ReturnCountRate.ToString()), out outFinGain);
                            //TabControlHelper.CustomFieldByFortum(InitParam.ExpressionFineGain, cnrX, cntRate, 0, out outFinGain);  
                            if (isCalcSuccess)
                            {
                                if (outFinGain > 0.75 && outFinGain < 1.25)
                                    InitParam.FineGain = (float)outFinGain;
                            }
                        }
                        catch
                        { }
                    }

                    if (_netPortType == NetPortType.DPP100)
                    {


                        DisableDpp100();
                        ClearDataDpp100();
                        DisableDpp100();
                        SetTestTimeValueToDpp100(DeviceParam.PrecTime);
                        ClearDataDpp100();
                        EnableDpp100();
                    }
                    else
                    {
                        Array.Clear(port.specData, 0, port.specData.Length);
                        if (WorkCurveHelper.IDemarcateTest == 2)
                        {
                            port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(1 * 65536),
                            (uint)(DeviceParam.PrecTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, 0);
                        }
                        else
                        {
                            port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(InitParam.FineGain * 65536),
                            (uint)(DeviceParam.PrecTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, FPGAparam.Intercept);
                        }
                    }
                    Time1 = 0;
                    Array.Clear(Decre1, 0, Decre1.Length);
                    HasRecord = false;
                    Count1 = 0;
                    pauseTime = 0;
                    usedTime = 0;
                    continue;

                }
                #region Dpp100
                if (_netPortType == NetPortType.DPP100)
                {
                    Thread.Sleep(200);
                    if (State == DeviceState.Pause)
                    {
                        DisableDpp100();
                        format = new MessageFormat(Info.PauseStop, 0);
                        WorkCurveHelper.specMessage.localMesage.Add(format);

                    }
                    while (State == DeviceState.Pause)
                    {
                        Thread.Sleep(200);
                    }
                    if (State == DeviceState.Resume)
                    {
                        EnableDpp100();
                        format = new MessageFormat(Info.SpectrumTest, 0);
                        WorkCurveHelper.specMessage.localMesage.Add(format);
                        Thread.Sleep(200);
                    }
                    double localTime = 0;
                    double accTime = 0;
                    double slowCount = 0;
                    double slowCountRate = 0;
                    string status = string.Empty;
                    bool getResult = GetDatasFromDpp100(false, ref localTime, ref accTime, ref slowCount, ref slowCountRate, ref status);
                    if (!getResult)
                    {
                        recfailedcout++;
                        if (recfailedcout > 5)
                        {
                            bDppErrorInTest = true;
                            if (accTime <= 0.0001)
                                break;
                            ////  RayTube.SetXRayTubeParams(0, 0);
                            // // DisableDpp100();
                            ////  ClearDataDpp100();
                            ////  SetDp5Cfg();
                            // // RayTube.SetXRayTubeParams(DeviceParam.TubCurrent, DeviceParam.TubVoltage);
                            //  if (recfailedcout % 5 == 0)
                            //  {
                            //      EnableDpp100();
                            //      continue;
                            //  }
                            //  else if (recfailedcout > 40)
                            //  {
                            //      bDppErrorInTest = true;
                            //      break;
                            //  }
                            //recfailedcout = 0;
                            //continue;
                        }
                        else continue;
                    }
                    else recfailedcout = 0;
                    //Array.Clear(newNetPortProtocol.SPECTRUM, 0, DeviceParam.BeginChann);
                    //Array.Clear(newNetPortProtocol.SPECTRUM, DeviceParam.EndChann, (int)WorkCurveHelper.DeviceCurrent.SpecLength - DeviceParam.EndChann);
                    //newNetPortProtocol.SPECTRUM.CopyTo(Data, 0);
                    //newNetPortProtocol.SPECTRUM.CopyTo(DataCopy, 0);

                    //for (int i = 0; i < Data.Length; i++)
                    //{
                    //    Data[i] = (int)newNetPortProtocol.SPECTRUM[i];
                    //    DataCopy[i] = (int)newNetPortProtocol.SPECTRUM[i];
                    //}
                    int z = 4096 / (int)WorkCurveHelper.DeviceCurrent.SpecLength;

                    for (int i = DeviceParam.BeginChann; i < DeviceParam.EndChann; i++)
                    {
                        int k = z;
                        long temp = 0;
                        while (k-- > 0)
                        {
                            temp += newNetPortProtocol.SPECTRUM[i * z + k];
                        }
                        DataCopy[i] = (int)temp;
                        Data[i] = (int)temp;
                      
                    }


                    if (WorkCurveHelper.IShowQuickInfo)
                    {
                        using (FileStream fileStream = new FileStream(Application.StartupPath + @"\newdp5Data.txt", FileMode.Append))
                        {
                            using (StreamWriter sw = new StreamWriter(fileStream))
                            {
                                // sw.WriteLine(status + "\r\n" + newNetPortProtocol.GetSpectrumString() + "\r\n" + DateTime.Now.ToString() + "\r\n");
                                sw.WriteLine(status + DateTime.Now.ToString() + "\r\n");
                           
                            }
                        }
                    }
                    //TestTotalCount = (int)slowCount;
                    TestTotalCount = slowCount;
                    usedTime = localTime;
                    if (currentRateMode == CountingRateModes.Average && (usedTime - Time1) > 0.1)
                        CountRating = (TestTotalCount - Count1) / (usedTime - pauseTime - Time1);
                    else CountRating = slowCountRate;


                    ushort[] buff = new ushort[2];

                    if (CountRating != 0 && WorkCurveHelper.startDoing == 1)
                    {
                        SetReal(buff, 0, (float)CountRating);
                        WorkCurveHelper.dataStore.HoldingRegisters[1] = buff[0];
                        WorkCurveHelper.dataStore.HoldingRegisters[2] = buff[1];

                    }


                }
                #endregion
                #region DMCA
                else
                {
                    Thread.Sleep(500);
                    ((NetPort)port).GetDMCADatas();
                    //port.GetData(Data);
                    //int ireadCount = 0;
                    //do
                    //{
                    if (State == DeviceState.Pause)
                    {
                        format = new MessageFormat(Info.PauseStop, 0);
                        WorkCurveHelper.specMessage.localMesage.Add(format);
                    }
                    while (State == DeviceState.Pause)
                    {
                        Thread.Sleep(200);
                    }
                    if (State == DeviceState.Resume)
                    {
                        pauseTime += (double)port.uSystemTime / 1000d;
                        State = DeviceState.Test;
                        format = new MessageFormat(Info.SpectrumTest, 0);
                        WorkCurveHelper.specMessage.localMesage.Add(format);
                        if (WorkCurveHelper.IDemarcateTest == 2)
                        {
                            port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(1 * 65536),
                            (uint)((DeviceParam.PrecTime - pauseTime) * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, 0);
                        }
                        else
                        {
                            port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(InitParam.FineGain * 65536),
                            (uint)((DeviceParam.PrecTime - pauseTime) * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, FPGAparam.Intercept);
                        }
                        port.uSystemTime = 0;
                        port.uLoaclTime = 0;
                        port.uSlowCount = 0;
                        port.uQuickCount = 0;
                        goto Begin;
                    }
                    //    ireadCount++;
                    //    Thread.Sleep(50);
                    //}
                    //while (!port.NetReadOK&&ireadCount<100);
                    ////Console.WriteLine(ireadCount);
                    //port.NetReadOK = false;
                    //Array.Clear(port.specData, 0, DeviceParam.BeginChann);
                    //Array.Clear(port.specData, DeviceParam.EndChann, (int)WorkCurveHelper.DeviceCurrent.SpecLength - DeviceParam.EndChann);

                    //port.specData.CopyTo(DataCopy, 0);
                    //port.specData.CopyTo(Data, 0);
                    int z = 4096 / (int)WorkCurveHelper.DeviceCurrent.SpecLength;
                    for (int i = DeviceParam.BeginChann; i < DeviceParam.EndChann; i++)
                    {
                        int k = z;
                        long temp = 0;
                        while (k-- > 0)
                        {
                            temp += port.specData[i * z + k];
                        }
                        DataCopy[i] = (int)temp;
                        Data[i] = (int)temp;
                    }

                    //TestTotalCount = (int)port.uSlowCount;
                    TestTotalCount = port.uSlowCount;
                    usedTime = pauseTime + (double)port.uSystemTime / 1000d;
                    CountRating = CountRate();
                    #region 记录快慢成型数据
                    if (WorkCurveHelper.IShowQuickInfo)
                    {
                        int countData = 0;
                        //int calibrationData = 0;
                        for (int i = 0; i < port.specData.Length; i++)
                        {
                            countData += port.specData[i];
                            //calibrationData += DataCopy[i];
                        }
                        using (FileStream fileStream = new FileStream(Application.StartupPath + @"\ApplicationData.txt", FileMode.Append))
                        {
                            using (StreamWriter sw = new StreamWriter(fileStream))
                            {//+ "校正后计数总和：" + calibrationData
                                sw.WriteLine("测量名称：  " + Spec.Name + "快成型：" + port.uQuickCount.ToString() + "慢成型：" + port.uSlowCount.ToString() + "||" + "计数总和：" + countData + "测量时间：" + (port.uSystemTime / 1000).ToString());
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                if (State == DeviceState.Break)
                    break;

                #region 自动校正峰飘
                if (WorkCurveHelper.IPeakCalibration && WorkCurveHelper.IDemarcateTest == 0)
                {
                    int[] smoothData = new int[DataCopy.Length];
                    DataCopy.CopyTo(smoothData, 0);
                    int smoothTime = int.Parse(ConfigurationSettings.AppSettings["SmoothTimes"]);
                    MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Helper.Smooth(smoothData, smoothTime));
                    if (MaxChannelRealTime > 0)
                    {
                        foreach (Atom atom in Atoms.AtomList)
                        {
                            if (atom.AtomID < 11 || (atom.AtomID > 56 && atom.AtomID < 72) || atom.AtomID > 88)
                                continue;
                            double channel = DemarcateEnergyHelp.GetDoubleChannel(atom.Ka);
                            bool calibration = CalibrationPeakAuto(atom, atom.Kb, channel, MaxChannelRealTime, ref DataCopy);
                            if (calibration)
                            {
                                break;
                            }
                            channel = DemarcateEnergyHelp.GetDoubleChannel(atom.La);
                            calibration = CalibrationPeakAuto(atom, atom.Lb, channel, MaxChannelRealTime, ref DataCopy);
                            if (calibration)
                                break;
                        }
                    }
                    //SpectrumCalibrate(ref DataCopy);
                }
                #endregion

                Spec.SpecData = TabControlHelper.ConvertArrayToString(DataCopy);
                //Spec.SpecOrignialData = Spec.SpecData;
                MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Spec.SpecDatas);
                port.getParam(out voltage, out current, out cover);//获取管压管流
                if (!HasRecord)
                {
                    DataCopy.CopyTo(Decre1, 0);
                    Time1 = usedTime;
                    Count1 = TestTotalCount;
                    HasRecord = true;
                }
                if (Pump.Exist)
                {
                    if (WorkCurveHelper.Is3200L)
                    {
                        uint upvacuum = 0;
                        uint downvacuum = 0;
                        port.GetDoubleVacuum(utype, out upvacuum, out downvacuum);
                        ReturnVacuum = upvacuum;
                        ReturnVacuumDown = downvacuum;
                    }
                    else
                    {
                        port.GetVacuum(utype, out ava);
                        ReturnVacuum = ava;
                    }
                }
                #region 开盖
                if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && (cover != 0))
                {
                    StopFlag = true;
                    connect = DeviceConnect.DisConnect;
                    CloseDevice();
                    State = DeviceState.Idel;
                    SendMessage(this.OwnerHandle, Wm_OpenCover, 0, 0);
                    return;
                }
                else if (WorkCurveHelper.suspendTest)
                {
                    StopFlag = true;
                    connect = DeviceConnect.DisConnect;
                    CloseDevice();
                    State = DeviceState.Idel;
                    SendMessage(this.OwnerHandle, Wm_SuspendTest, 0, 0);
                    return;
                }
                #endregion

                ReturnVoltage = voltage * WorkCurveHelper.DeviceCurrent.VoltageScaleFactor;
                ReturnCurrent = current * DeviceParam.CurrentRate * WorkCurveHelper.DeviceCurrent.CurrentScaleFactor;
                Spec.TubCurrent = ReturnCurrent > 0 ? ReturnCurrent : DeviceParam.TubCurrent;
                Spec.TubVoltage = ReturnVoltage > 0 ? ReturnVoltage : DeviceParam.TubVoltage;

                #region 调节计数率
                if (usedTime / 5 >= adjustCountAdd && !bDppErrorInTest)
                    IsAdjust = false;
                if ((DeviceParam.IsAdjustRate) && usedTime > 1 && !IsAdjust)
                {
                    int adjustCount = AdjustCountRate();
                    //if (adjustCount == 0)
                    //{
                    //    string sSimilarCurve = ReportTemplateHelper.LoadSpecifiedValue("EDX3000", "SimilarCurve");
                    //    if (sSimilarCurve == "1")
                    //    {
                    //        MatchPlus = MatchPlus.MatchOn;
                    //    }
                    //    else
                    //    {
                    //        Msg.Show(string.Format(Info.strAdjustCountFail, WorkCurveHelper.DeviceCurrent.MaxCurrent), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    }
                    //    StopFlag = true;
                    //    connect = DeviceConnect.DisConnect;
                    //    break;
                    //}
                    //else
                    if (adjustCount == 1)
                    {
                        IsAdjust = true;
                        adjustCountAdd = 1;
                        RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);


                        if (_netPortType == NetPortType.DPP100)
                        {
                            DisableDpp100();
                            ClearDataDpp100();
                            EnableDpp100();
                        }
                        else
                        {
                            Array.Clear(port.specData, 0, port.specData.Length);
                            if (WorkCurveHelper.IDemarcateTest == 2)
                            {
                                port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(1 * 65536),
                                (uint)(DeviceParam.PrecTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, 0);
                            }
                            else
                            {
                                port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(InitParam.FineGain * 65536),
                                (uint)(DeviceParam.PrecTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, FPGAparam.Intercept);
                            }
                        }
                        dropData = true;
                        usedTime = 0;

                        ClearData();
                        Time1 = 0;
                        Array.Clear(Decre1, 0, Decre1.Length);
                        HasRecord = false;
                        Count1 = 0;
                        pauseTime = 0;

                        //yuzhao20150609_begin:yuzhao20150609_begin:根据戴晓玲需求更改为保存调节计数率后的条件
                        //Condition cd = Condition.FindOne(c => c.Id == WorkCurveHelper.WorkCurveCurrent.Condition.Id && c.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                        //if (cd != null)
                        //{
                        //    cd.DeviceParamList[0].TubCurrent = DeviceParam.TubCurrent;
                        //    WorkCurveHelper.WorkCurveCurrent.Condition = cd;
                        //    cd.Save();
                        //}
                        if (WorkCurveHelper.WorkCurveCurrent.Condition != null)
                        {
                            WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].TubCurrent = DeviceParam.TubCurrent;
                            WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].Save();
                        }
                        //yuzhao20150609_end

                    }
                    else
                    {
                        IsAdjust = true;
                        adjustCountAdd++;
                    }
                }
                #endregion

                #region 根据快成型门限调节管流
                if (_netPortType != NetPortType.DPP100)
                {
                    double fastAverage = (double)(port.uQuickCount / (port.uSystemTime / 1000f));

                    if (fastAverage > limit && limit > 0 && usedTime > 0)//限制调节时测量时间不能为0
                    {
                        DeviceParam.TubCurrent = (int)(DeviceParam.TubCurrent / (fastAverage / (limit - 2000)));
                        RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
                        if (WorkCurveHelper.IDemarcateTest == 2)
                        {
                            port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(1 * 65536),
                            (uint)(DeviceParam.PrecTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, 0);
                        }
                        else
                        {
                            port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(InitParam.FineGain * 65536),
                            (uint)(DeviceParam.PrecTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, FPGAparam.Intercept);
                        }
                        dropData = true;
                        usedTime = 0;
                        Array.Clear(port.specData, 0, port.specData.Length);
                        ClearData();
                    }
                }
                #endregion
                if (usedTime > 0)//前面有几秒的时间，系统时间都是为0
                {
                    PostMessage(OwnerHandle, WM_ReceiveData, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
                }
                // Console.WriteLine("扫谱时间：" + (port.uSystemTime/1000).ToString());
            }



            //if (WorkCurveHelper.DeviceCurrent.HasMotorY1 && IsSpin)
            //{
            //    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, WorkCurveHelper.DeviceCurrent.MotorY1Direct, 0, true, WorkCurveHelper.DeviceCurrent.MotorY1Speed);
            //    Thread.Sleep(500);
            //}

            //for (int i = 0; i < DataCopy.Length; i++)
            //{
            //    if (usedTime - Time1 == 0)
            //        break;
            //    double newChannelData = (DataCopy[i] - Decre1[i]) * (usedTime / (usedTime - Time1));
            //    DataCopy[i] = (int)Math.Round(newChannelData, MidpointRounding.AwayFromZero);
            //}
            Spec.SpecData = TabControlHelper.ConvertArrayToString(DataCopy);

            using (FileStream fileStream = new FileStream(Application.StartupPath + @"\newdp5Data.txt", FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fileStream))
                {

                    sw.WriteLine(Spec.SpecData);
                }
            }

            Spec.UsedTime = usedTime;

            if (WorkCurveHelper.startDoing == 1)
            {
                ushort[] buff1 = new ushort[2];
                SetReal(buff1, 0, (float)usedTime);
                WorkCurveHelper.dataStore.HoldingRegisters[3] = buff1[0];
                WorkCurveHelper.dataStore.HoldingRegisters[4] = buff1[1];
            }


            State = DeviceState.Idel;
            connect = DeviceConnect.DisConnect;
            this.format = new MessageFormat(Info.SpectrumEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            if (_netPortType == NetPortType.DPP100)
            {
                DisableDpp100();
                ClearDataDpp100();
            }
            else Array.Clear(port.specData, 0, port.specData.Length);
            Thread.Sleep(300);
            //if(bDppErrorInTest&&usedTime<==0.001)
           
            if (WorkCurveHelper.bShowInitParam)
            {
                using (FileStream fileStream = new FileStream(Application.StartupPath + @"\TestInit.txt", FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fileStream))
                    {//+ "校正后计数总和：" + calibrationData
                        sw.WriteLine("计数率: " + calcTimeCountRate.ToString() + " 测量名称：  " + Spec.Name + "放大倍数：" + InitParam.FineGain + " 公式：" + InitParam.ExpressionFineGain);
                    }
                }
            }

            if (bDppErrorInTest)
                PostMessage(OwnerHandle, Wm_DeviceError, (short)(usedTime <= 0.001 ? 0 : 1), (int)logErr.DppError);
            else
            {
                //管流下降至30%
                if (WorkCurveHelper.IsUseElect && WorkCurveHelper.IsContinueVol)
                {
                    RayTube.SetXRayTubeParams(DeviceParam.TubCurrent * 3 / 10 / DeviceParam.CurrentRate, DeviceParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
                }

                
                PostMessage(OwnerHandle, WM_EndTest, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));


                if (WorkCurveHelper.testNum >= 1 )
                {
                    WorkCurveHelper.curDeviceNum++;
                    if(WorkCurveHelper.curDeviceNum % WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count == 0)
                        WorkCurveHelper.testTimes--;            
                }


                if (WorkCurveHelper.IsDBOpen && !StopFlag)
                    SaveCurRecord();
                

                if (WorkCurveHelper.startDoing == 1 && !StopFlag)
                {
                    WorkCurveHelper.dataStore.CoilDiscretes[1] = false;
                    WorkCurveHelper.dataStore.CoilDiscretes[2] = true;
                    WorkCurveHelper.startDoing = 0;
                    ulong no = ulong.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/modbusNo"));
                    no++;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/modbusNo", no.ToString());
                    HistoryRecord lastRecord = HistoryRecord.FindRecent(1)[0];
                    List<HistoryElement> he = HistoryElement.FindBySql("select * from HistoryElement where Historyrecord_Id=" + lastRecord.Id);
                    WorkCurve wk = WorkCurve.FindById(lastRecord.WorkCurveId);
                    string[] eleNames = wk.Name.Split('-').RemoveLast();

                    for (int j = 0; j < he.Count(); j++)
                    {
                        if (!eleNames.Contains(he[j].elementName))
                        {
                            he[j] = null;
                        }
                    }
                    he.RemoveAll(new Predicate<HistoryElement>(isNUll));
                    he.Sort(new Comparison<HistoryElement>(compareEle));
                    int k = 5;
                    for (int j = 0; j < he.Count(); j++)
                    {
                        ushort[] buff = new ushort[2];
                        SetReal(buff, 0, float.Parse(he[j].thickelementValue));
                        WorkCurveHelper.dataStore.HoldingRegisters[k] = buff[0];
                        WorkCurveHelper.dataStore.HoldingRegisters[k + 1] = buff[1];
                        k += 2;
                    }

                }


                if (!StopFlag)
                {
                    if (State == DeviceState.Idel)
                        WorkCurveHelper.dataStore.InputDiscretes[2] = false;
                    else
                        WorkCurveHelper.dataStore.InputDiscretes[2] = true;
                }


            }


        }

        public static bool isNUll(HistoryElement ele)
        {
            return ele == null;

        }
        public static int compareEle(HistoryElement ele, HistoryElement ele1)
        {

            return string.Compare(ele.elementName, ele1.elementName);

        }

        public static long lastId = 0;

        public static System.Data.SqlClient.SqlConnection SqlConn = null;

        private static System.Data.SqlClient.SqlConnection ConnectToSQLDB()
        {
            System.Data.SqlClient.SqlConnection SqlConn = new System.Data.SqlClient.SqlConnection();
            string strPath = Application.StartupPath + "\\DBConnection.ini";
            string contstr = string.Empty;
            System.Text.StringBuilder tempbuilder = new System.Text.StringBuilder(255);
            string strDataSource = string.Empty;
            Skyray.API.WinMethod.GetPrivateProfileString("Param", "DataSource", "Local", tempbuilder, 255, strPath);
            strDataSource = tempbuilder.ToString();
            contstr += "Server=" + strDataSource + ";";

            Skyray.API.WinMethod.GetPrivateProfileString("Param", "InitialCatalog", "EDXRFDB", tempbuilder, 255, strPath);
            contstr += "Initial Catalog=" + tempbuilder.ToString() + ";";

            Skyray.API.WinMethod.GetPrivateProfileString("Param", "UserID", "sa", tempbuilder, 255, strPath);
            contstr += "uid=" + tempbuilder.ToString() + ";";

            Skyray.API.WinMethod.GetPrivateProfileString("Param", "password", "", tempbuilder, 255, strPath);
            contstr += "pwd=" + tempbuilder.ToString();
            try
            {
                SqlConn.ConnectionString = contstr;
                SqlConn.Open();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("无法连接数据库");
                return null;
            }
            return SqlConn;
        }

        private static bool CheckDateBaseSQL()
        {

            string strSql = "";
            strSql += "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TestRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) ";
            strSql += @"

                        CREATE TABLE [dbo].[TestRecord](
	                        [Id] [int] NOT NULL,
	                        [Name] [varchar](100) NOT NULL,
	                        [RecordCord] [varchar](100) ,
	                        [InspectionCode] [varchar](100),
	                        [WorkCurveName] [varchar](100) NOT NULL,
	                        [DeviceName] [varchar](100) NOT NULL,
	                        [SpecDate] [datetime] NOT NULL,
	                        [CountRate] [float] NOT NULL,
	                        [PeakChannel] [float] NOT NULL,
	                        [TubVoltage] [float] NOT NULL,
	                        [TubCurrent] [float] NOT NULL,
                         CONSTRAINT [PK_TestRecord] PRIMARY KEY CLUSTERED 
                        (
	                        [Id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY]

                   
                        ;";

            //检查是否有TestElement表

            strSql += "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TestElement]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) ";
            strSql += @"
                       begin

                        CREATE TABLE [dbo].[TestElement](
	                        [Id] [int] IDENTITY(1,1) NOT NULL,
	                        [TestRecord_Id] [int] NOT NULL,
	                        [ElementName] [varchar](50) NOT NULL,
	                        [ElementValue] [float] NOT NULL,
                         CONSTRAINT [PK_TestElement] PRIMARY KEY CLUSTERED 
                        (
	                        [Id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY] ";


            strSql += @"
                        ALTER TABLE [dbo].[TestElement]  WITH CHECK ADD  CONSTRAINT [FK_TestElement_TestRecord] FOREIGN KEY([TestRecord_Id])
                        REFERENCES [dbo].[TestRecord] ([Id])
                        ON UPDATE CASCADE
                        ON DELETE CASCADE 

                        
                        end; ";





            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = SqlConn;
            try
            {
                cmd.CommandText = strSql;
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("无法创建数据表");
                return false;
            }


        }


        public static void SaveCurRecord()
        {
            if (SqlConn == null)
                SqlConn = ConnectToSQLDB();
            if (SqlConn == null)
                return;

            if (!CheckDateBaseSQL())
            {

                return;
            }
            string strSql = "";


            List<HistoryRecord> records = HistoryRecord.FindBySql(string.Format("select * from HistoryRecord where Id > {0}", lastId));

            //保存一条记录后，将当前已保存Id加1
            lastId += 1;

            List<long> selectLong = new List<long>();
            foreach (var item in records)
            {
                selectLong.Add(item.Id);

            }
            selectLong.Sort();
            //此处先获取样品编号以及送检编号在sqlite中的id
            long sampleId = 0;
            long checkId = 0;
            try
            {

                sampleId = CompanyOthersInfo.FindBySql("select * from CompanyOthersInfo a where  a.Name = '" + "样品编号" + "' ")[0].Id;
                checkId = CompanyOthersInfo.FindBySql("select * from CompanyOthersInfo a where  a.Name = '" + "送检编号" + "' ")[0].Id;
            }
            catch
            {
                Msg.Show("请先添加样品编号以及送检编号信息");
                return;
            }


            for (int i = 0; i < selectLong.Count; i++)
            {

                HistoryRecord hd = HistoryRecord.FindById(selectLong[i]);
                List<HistoryElement> he = HistoryElement.FindBySql("select * from HistoryElement where Historyrecord_Id=" + hd.Id);
                WorkCurve wk = WorkCurve.FindById(hd.WorkCurveId);
                string[] eleNames = wk.Name.Split('-').RemoveLast();

                for (int j = 0; j < he.Count(); j++)
                {
                    if (!eleNames.Contains(he[j].elementName))
                    {
                        he[j] = null;
                    }
                }


                //此处得到了样品的实际编号以及送检的实际编号


                string sampleInfo = "";
                List<HistoryCompanyOtherInfo> temp1 = HistoryCompanyOtherInfo.FindBySql("select * from HistoryCompanyOtherInfo where History_Id = '" + selectLong[i].ToString() + "' and CompanyOthersInfo_Id = '" + sampleId.ToString() + "'");
                if (temp1.Count > 0)
                    sampleInfo = temp1[0].ListInfo;

                string checkInfo = "";

                List<HistoryCompanyOtherInfo> temp2 = HistoryCompanyOtherInfo.FindBySql("select * from HistoryCompanyOtherInfo where History_Id = '" + selectLong[i].ToString() + "' and CompanyOthersInfo_Id = '" + checkId.ToString() + "'");
                if (temp2.Count > 0)
                    checkInfo = temp2[0].ListInfo;






                strSql += string.Format("if not exists (select * from TestRecord where Name = '{0}' and DeviceName = '{1}' and  WorkCurveName = '{2}' ) ", hd.SampleName, hd.DeviceName, wk.Name);

                if (sampleInfo != "" && checkInfo != "")
                {

                    strSql += string.Format(@"
                                INSERT INTO [dbo].[TestRecord]
                                   ([Id]
                                   ,[Name]
                                   ,[RecordCord]
                                   ,[InspectionCode]
                                   ,[WorkCurveName]
                                   ,[DeviceName]
                                   ,[SpecDate]
                                   ,[CountRate]
                                   ,[PeakChannel]
                                   ,[TubVoltage]
                                   ,[TubCurrent])
                             VALUES
                                   ({0}
                                   ,'{1}'
                                   ,'{2}'
                                   ,'{3}'
                                   ,'{4}'
                                   ,'{5}'
                                   ,'{6}'
                                   ,{7}
                                   ,{8}
                                   ,{9}
                                   ,{10})

                              ;", selectLong[i], hd.SampleName, sampleInfo, checkInfo, wk.Name, hd.DeviceName, hd.SpecDate, hd.CountRate, hd.PeakChannel, hd.ActualVoltage, hd.ActualCurrent);

                }

                else
                {
                    strSql += string.Format(@"
                                INSERT INTO [dbo].[TestRecord]
                                   ([Id]
                                   ,[Name]
                                  
                                   ,[WorkCurveName]
                                   ,[DeviceName]
                                   ,[SpecDate]
                                   ,[CountRate]
                                   ,[PeakChannel]
                                   ,[TubVoltage]
                                   ,[TubCurrent])
                             VALUES
                                   ({0}
                                   ,'{1}'
                                  
                              
                                   ,'{2}'
                                   ,'{3}'
                                   ,'{4}'
                                   ,{5}
                                   ,{6}
                                   ,{7}
                                   ,{8})

                              ;", selectLong[i], hd.SampleName, wk.Name, hd.DeviceName, hd.SpecDate, hd.CountRate, hd.PeakChannel, hd.ActualVoltage, hd.ActualCurrent);


                }

                strSql += string.Format("if  exists (select * from TestRecord where Name = '{0}' and DeviceName = '{1}' and  WorkCurveName = '{2}' ) ", hd.SampleName, hd.DeviceName, wk.Name);

                if (sampleInfo != "" && checkInfo != "")
                {

                    strSql += string.Format(@"
                                UPDATE [dbo].[TestRecord]
                               SET [Id] = {0}
                                  ,[Name] = '{1}'
                                  ,[RecordCord] = '{2}'
                                  ,[InspectionCode] = '{3}'
                                  ,[WorkCurveName] = '{4}'
                                  ,[DeviceName] = '{5}'
                                  ,[SpecDate] = '{6}'
                                  ,[CountRate] = {7}
                                  ,[PeakChannel] = {8}
                                  ,[TubVoltage] = {9}
                                  ,[TubCurrent] = {10}
                             WHERE 
                                  Name = '{11}' and DeviceName = '{12}' and WorkCurveName = '{13}'

                              ;", selectLong[i], hd.SampleName, sampleInfo, checkInfo, wk.Name, hd.DeviceName, hd.SpecDate, hd.CountRate, hd.PeakChannel, hd.ActualVoltage, hd.ActualCurrent, hd.SampleName, hd.DeviceName, wk.Name);


                }

                else
                {

                    strSql += string.Format(@"
                                UPDATE [dbo].[TestRecord]
                               SET [Id] = {0}

                                  ,[Name] = '{1}'
                                  
                                  ,[WorkCurveName] = '{2}'
                                  ,[DeviceName] = '{3}'
                                  ,[SpecDate] = '{4}'
                                  ,[CountRate] = {5}
                                  ,[PeakChannel] = {6}
                                  ,[TubVoltage] = {7}
                                  ,[TubCurrent] = {8}
                             WHERE 
                                  Name = '{9}' and DeviceName = '{10}' and WorkCurveName = '{11}'

                              ;", selectLong[i], hd.SampleName, wk.Name, hd.DeviceName, hd.SpecDate, hd.CountRate, hd.PeakChannel, hd.ActualVoltage, hd.ActualCurrent, hd.SampleName, hd.DeviceName, wk.Name);


                }

                for (int k = 0; k < he.Count; k++)
                {
                    if (he[k] != null)
                    {
                        strSql += string.Format("if not exists (select * from TestElement where TestRecord_Id = {0} and  ElementName = '{1}' ) ", selectLong[i], he[k].elementName);

                        strSql += string.Format(@"
                                        INSERT INTO [dbo].[TestElement]
                                       (
                                       [TestRecord_Id]
                                       ,[ElementName]
                                       ,[ElementValue])
                                 VALUES
                                       (
                                       {0}
                                       ,'{1}'
                                       ,{2})
                                      ;", selectLong[i], he[k].elementName, he[k].thickelementValue);

                        strSql += string.Format("if  exists (select * from TestElement where TestRecord_Id = {0} and  ElementName = '{1}' ) ", selectLong[i], he[k].elementName);

                        strSql += string.Format(@"
                               UPDATE [dbo].[TestElement]
                               SET 
                                  [ElementValue] = {0}
                                WHERE
                                    TestRecord_Id = {1} and ElementName = '{2}'
                                     ;", he[k].thickelementValue, selectLong[i], he[k].elementName);


                    }

                }

                try
                {


                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = SqlConn;
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();
                    return;


                }

                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show("保存记录失败");
                    return;
                }



            }
        }
        public override void ClearData()
        {
            base.ClearData();
            if (port != null)
            {
                port.uSystemTime = 0;
                port.uLoaclTime = 0;
                port.uSlowCount = 0;
                port.uQuickCount = 0;
            }
        }


        public override void SetOpenCurrent()
        {
            if (DeviceParam != null)
                RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);

        }

        /// <summary>
        /// 预热开高压
        /// </summary>
        public override void PreOpenVoltage()
        {
            RayTube.SetXRayTubeParams(heatParams.TubCurrent / heatParams.CurrentRate, heatParams.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)heatParams.TargetMode);
            RayTube.Open();

            //port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(heatParams.FineGain * 65536),
            //   (uint)(1), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, FPGAparam.Intercept);
        }

        /// <summary>
        /// 预热扫谱过程
        /// </summary>
        public override void PreHeatProcess()
        {
            DeviceParameter deviceParams = DeviceParameter.New.Init("PreDeviceParams", this.heatParams.PreHeatTime, heatParams.TubCurrent, heatParams.TubVoltage,
               heatParams.FilterIdx, heatParams.CollimatorIdx, heatParams.Target, false, 0, false, 0, false, 0, 0, 50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, false, false, 0, 0, 0, 0, 1, heatParams.TargetMode, heatParams.CurrentRate);
            DeviceParam = deviceParams;
            InitParam = InitParameter.New.Init(heatParams.TubVoltage, heatParams.TubCurrent,
                                                      heatParams.Gain, heatParams.FineGain, 0, 0, 1105, 0, "Ag", heatParams.FilterIdx, heatParams.CollimatorIdx, heatParams.Target, heatParams.TargetMode, heatParams.CurrentRate, "x", 1);
            SetDp5Cfg();
            DateTime startTime = DateTime.Now;
            TimeSpan diffTime = new TimeSpan();
            PreOpenVoltage();
            while (diffTime.TotalSeconds < this.heatParams.PreHeatTime && !StopFlag)
            {
                connect = DeviceConnect.Connect;
                diffTime = DateTime.Now.Subtract(startTime);
                Thread.Sleep(200);
                double voltage = 0;
                double current = 0;
                cover = 0;
                port.getParam(out voltage, out current, out cover);
                ReturnVoltage = voltage * WorkCurveHelper.DeviceCurrent.VoltageScaleFactor;
                ReturnCurrent = current * DeviceParam.CurrentRate * WorkCurveHelper.DeviceCurrent.CurrentScaleFactor;
                if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && (cover != 0))
                {
                    CloseDevice();
                    //Msg.Show(Info.CoverOpen);
                    SendMessage(this.OwnerHandle, Wm_OpenCover, 0, 0);
                    StopFlag = true;
                    connect = DeviceConnect.DisConnect;
                    State = DeviceState.Idel;
                    //break;
                    return;
                }
                Thread.Sleep(500);
                PostMessage(OwnerHandle, Wm_PreheatOpenVoltage, true, (int)diffTime.TotalSeconds);
            }
            State = DeviceState.Idel;
            PostMessage(OwnerHandle, Wm_PreheatOpenEnd, true, (int)diffTime.TotalSeconds);


        }

        public override int AdjustInitCountRate()
        {
            if (CountRating != 0)
            {
                int current = this.InitParam.TubCurrent;
                int tempCurrent = (int)Math.Round(current * (InitParam.MinRate + InitParam.MaxRate) / (CountRating * 2.0));
                if (CountRating < InitParam.MinRate)
                {
                    if (tempCurrent < (current + 4))
                    {
                        tempCurrent += 4;
                    }
                    current = tempCurrent;
                    if (tempCurrent > MaxCurrent)
                    {
                        InitParam.TubCurrent = MaxCurrent;
                        return 0;
                    }
                    else
                        InitParam.TubCurrent = tempCurrent;
                    return 1;
                }
                else if (CountRating > InitParam.MaxRate)
                {
                    if (tempCurrent > (current - 4))
                    {
                        tempCurrent -= 4;
                    }
                    current = tempCurrent;
                    if (tempCurrent < MinCurrent)
                    {
                        InitParam.TubCurrent = MinCurrent;
                        return 0;
                    }
                    else
                        InitParam.TubCurrent = tempCurrent;
                    return 1;
                }
            }
            return 2;
        }

        /// <summary>
        /// 调节计数率
        /// </summary>
        /// <returns></returns>
        public override int AdjustCountRate()
        {
            if (CountRating != 0)
            {
                int current = DeviceParam.TubCurrent;
                int tempCurrent = (int)Math.Round(current * (DeviceParam.MinRate + DeviceParam.MaxRate) / (CountRating * 2.0));
                if (CountRating < DeviceParam.MinRate)
                {
                    if (tempCurrent < (current + 4))
                    {
                        tempCurrent += 4;
                    }
                    current = tempCurrent;
                    if (tempCurrent > MaxCurrent)
                    {
                        DeviceParam.TubCurrent = MaxCurrent;
                        return 0;
                    }
                    else
                    {
                        DeviceParam.TubCurrent = tempCurrent;
                    }
                    return 1;
                }
                else if (CountRating > DeviceParam.MaxRate)
                {
                    if (tempCurrent > (current - 4))
                    {
                        tempCurrent -= 4;
                    }
                    current = tempCurrent;
                    if (tempCurrent < MinCurrent)
                    {
                        DeviceParam.TubCurrent = MinCurrent;
                        return 0;
                    }
                    else
                    {
                        DeviceParam.TubCurrent = tempCurrent;
                    }
                    return 1;
                }
            }
            return 2;
        }

        /// <summary>
        /// 获取计数率
        /// </summary>
        /// <returns></returns>
        public override double CountRate()
        {
            double totalCount = CountRating;
            if (port != null && port.uSystemTime > 0)
            {
                if (currentRateMode == CountingRateModes.Average && (usedTime - pauseTime - Time1) > 0.1)
                    totalCount = (port.uSlowCount - Count1) / (usedTime - pauseTime - Time1);
                else if (port.uSlowCount - Count1 - m_lastCountRate > 0)
                    totalCount = port.uSlowCount - Count1 - m_lastCountRate;
            }
            m_lastCountRate = totalCount;
            // Console.WriteLine(port.uSlowCount + "  " + usedTime);
            if (totalCount < 0)
                totalCount = 0;
            return totalCount;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="obj"></param>
        public override void DoInitial(object obj)
        {
            FPGAparam = WorkCurveHelper.DeviceCurrent.FPGAParams;
            double limit = (int)(WorkCurveHelper.FastLimit * 6.4 / WorkCurveHelper.DeviceCurrent.FPGAParams.PeakingTime);
            double channelErr = 0;
            bool Successed = false;
            double voltage = 0;
            double current = 0;
            Time1 = 0;
            Count1 = 0;
            cover = 0;
            double RATIO = 210;
            int recfailedcout = 0;
            List<double> CatchChannels = new List<double>();//用于计算RATIO
            bool bDppErrorInTest = false;
            if (WorkCurveHelper.IsUseElect)
            {
                WorkCurveHelper.deviceMeasure.interfacce.Pump.TOpen();//电磁阀置1
            }
            #region
            if (WorkCurveHelper.Is3200L)
            {
                uint utype = 1;
                if (WorkCurveHelper.DeviceCurrent.VacuumPumpType == VacuumPumpType.VacuumSi)
                { utype = 0; }
                else if (WorkCurveHelper.DeviceCurrent.VacuumPumpType == VacuumPumpType.Atmospheric)
                { utype = 1; }
                else
                    utype = 2;
                LightStatus lightStatus = LightStatus.Middle;
                do
                {
                    lightStatus = port.GetLightShutState(WorkCurveHelper.DeviceCurrent.MotorLightCode);
                }
                while (lightStatus == LightStatus.Fail);
                if (lightStatus == LightStatus.Close || lightStatus == LightStatus.Middle)////如果光闸关闭
                {
                    float range = 0;
                    //range=计算真空规上下腔压差
                    uint upvacuum = 0;
                    uint downvacuum = 0;
                    port.GetDoubleVacuum(utype, out upvacuum, out downvacuum);
                    range = Math.Abs(upvacuum - downvacuum);
                    if (range > 3)//压差过大
                    {
                        //this.format = new MessageFormat("上下腔压差过大，请手动放气。", 0);
                        //WorkCurveHelper.specMessage.localMesage.Add(this.format);
                        Msg.Show("上下腔压差过大，请手动放气。");
                        StopFlag = true;
                    }
                    else//打开光闸
                    {
                        //port.电磁铁失效
                        //port.打开光闸
                        port.CloseElectromagnet();
                        int dir = WorkCurveHelper.DeviceCurrent.MotorLightDirect == 0 ? 1 : 0;
                        do
                        {
                            port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorLightCode, dir, 65535, true, 250 - WorkCurveHelper.DeviceCurrent.MotorLightSpeed);
                            Thread.Sleep(500);
                        }
                        while (port.GetLightShutState(WorkCurveHelper.DeviceCurrent.MotorLightCode) != LightStatus.Open);
                    }
                }
            }
            #endregion
            // SetDp5Cfg();
            if (!StopFlag)
            {
                port.AllowUncover(WorkCurveHelper.DeviceCurrent.IsAllowOpenCover);
                RayTube.Open();
                connect = DeviceConnect.Connect;
            }
            if (ExistMagnet)//电磁铁 0805
            {
                Pump.Open();
            }
            float fineGainTemp = InitParam.FineGain;
            if (!IsConnectDevice() || !port.ConnectState)
            {
                Msg.Show(Info.NetDeviceDisConnection);
                StopFlag = true;
                connect = DeviceConnect.DisConnect;
                PostMessage(OwnerHandle, WM_DeviceDisConnect, true, (int)usedTime);
                return;
            }
            //if (WorkCurveHelper.DeviceCurrent.HasMotorY1 && IsSpin)
            //{
            //    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, WorkCurveHelper.DeviceCurrent.MotorY1Direct, 65535, true, WorkCurveHelper.DeviceCurrent.MotorY1Speed);
            //}
            do
            {
                usedTime = 0;
                pauseTime = 0;
                CountRating = 0;

                ClearData();
                RayTube.Open();
                RayTube.SetXRayTubeParams(InitParam.TubCurrent / InitParam.CurrentRate, InitParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)InitParam.TargetMode);

                if (_netPortType == NetPortType.DPP100)
                {

                    DisableDpp100();
                    ClearDataDpp100();
                    SetTestTimeValueToDpp100(InitTime);
                    SetFineToDpp100(fineGainTemp);
                    ClearDataDpp100();
                    EnableDpp100();
                }
                else
                {
                    Array.Clear(port.specData, 0, port.specData.Length);
                    port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(InitParam.FineGain * 65536),//初始化的时候粗条设定为50倍
                       (uint)(InitTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, FPGAparam.Intercept);
                }

                bool IsAdjust = true;
                int adjustCountAdd = 1;

                while ((usedTime + 0.1 < InitTime) && (State != DeviceState.Idel) && !StopFlag)
                {
                Begin:
                    #region Dpp100
                    if (_netPortType == NetPortType.DPP100)
                    {
                        Thread.Sleep(200);
                        if (State == DeviceState.Pause)
                        {
                            DisableDpp100();
                            format = new MessageFormat(Info.PauseStop, 0);
                            WorkCurveHelper.specMessage.localMesage.Add(format);

                        }
                        while (State == DeviceState.Pause)
                        {
                            Thread.Sleep(200);
                        }
                        if (State == DeviceState.Resume)
                        {
                            EnableDpp100();
                            format = new MessageFormat(Info.SpectrumInitialize, 0);
                            WorkCurveHelper.specMessage.localMesage.Add(format);
                            Thread.Sleep(200);
                        }

                        double localTime = 0;
                        double accTime = 0;
                        double slowCount = 0;
                        double slowCountRate = 0;
                        string Status = string.Empty;
                        bool getResult = GetDatasFromDpp100(true, ref localTime, ref accTime, ref slowCount, ref slowCountRate, ref Status);
                        if (!getResult)
                        {
                            recfailedcout++;

                            if (recfailedcout > 5)
                            {
                                ////DisableDpp100();
                                ////ClearDataDpp100();
                                ////SetDp5Cfg();
                                ////SetTestTimeValueToDpp100(InitTime);
                                ////SetFineToDpp100(fineGainTemp);
                                //EnableDpp100();
                                //recfailedcout = 0;
                                //continue;
                                bDppErrorInTest = true;
                                if (accTime <= 0.0001)
                                    break;
                            }
                            else continue;
                        }
                        else recfailedcout = 0;
                        //for (int i = 0; i < Data.Length; i++)
                        //{
                        //    Data[i] = (int)newNetPortProtocol.SPECTRUM[i];
                        //    DataCopy[i] = (int)newNetPortProtocol.SPECTRUM[i];
                        //}
                        int z = 4096 / (int)WorkCurveHelper.DeviceCurrent.SpecLength;
                        for (int i = 0; i < Data.Length; i++)
                        {
                            int k = z;
                            long temp = 0;
                            while (k-- > 0)
                            {
                                temp += newNetPortProtocol.SPECTRUM[i * z + k];
                            }
                            DataCopy[i] = (int)temp;
                            Data[i] = (int)temp;
                        }
                        Spec.SpecData = TabControlHelper.ConvertArrayToString(Data);
                        //TestTotalCount = (int)slowCount;
                        TestTotalCount = slowCount;
                        usedTime = localTime;
                        if (currentRateMode == CountingRateModes.Average && usedTime > 0.1)
                            CountRating = TestTotalCount / usedTime;
                        else CountRating = slowCountRate;
                    }
                    #endregion
                    #region DMCA
                    else
                    {
                        Thread.Sleep(500);
                        //port.GetData(Data);
                        //int ireadCount = 0;
                        //do
                        //{
                        ((NetPort)port).GetDMCADatas();
                        if (State == DeviceState.Pause)
                        {
                            format = new MessageFormat(Info.PauseStop, 0);
                            WorkCurveHelper.specMessage.localMesage.Add(format);
                        }
                        while (State == DeviceState.Pause)
                        {
                            Thread.Sleep(200);
                        }
                        if (State == DeviceState.Resume)
                        {
                            pauseTime += (double)port.uSystemTime / 1000d;
                            State = DeviceState.Test;
                            format = new MessageFormat(Info.SpectrumInitialize, 0);
                            WorkCurveHelper.specMessage.localMesage.Add(format);
                            port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(InitParam.FineGain * 65536),
                       (uint)((DeviceParam.PrecTime - pauseTime) * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, FPGAparam.Intercept);
                            port.uSystemTime = 0;
                            port.uLoaclTime = 0;
                            port.uSlowCount = 0;
                            port.uQuickCount = 0;
                            goto Begin;
                        }
                        //   Thread.Sleep(50);
                        //    ireadCount++;
                        //}
                        //while (!port.NetReadOK&&ireadCount<100);
                        //port.NetReadOK = false;
                        //Array.Clear(port.specData, 0, 50);
                        //Array.Clear(port.specData, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, 50);
                        //Spec.SpecData = TabControlHelper.ConvertArrayToString(port.specData);
                        //port.specData.CopyTo(DataCopy, 0);
                        int z = 4096 / (int)WorkCurveHelper.DeviceCurrent.SpecLength;
                        for (int i = 0; i < Data.Length; i++)
                        {
                            int k = z;
                            long temp = 0;
                            while (k-- > 0)
                            {
                                temp += port.specData[i * z + k];
                            }
                            DataCopy[i] = (int)temp;
                            Data[i] = (int)temp;
                        }
                        Spec.SpecData = TabControlHelper.ConvertArrayToString(Data);
                        usedTime = pauseTime + (double)port.uSystemTime / 1000;
                        CountRating = CountRate();
                        if (WorkCurveHelper.IShowQuickInfo)
                        {
                            int countData = 0;
                            //int calibrationData = 0;
                            for (int i = 0; i < port.specData.Length; i++)
                            {
                                countData += port.specData[i];
                                //calibrationData += DataCopy[i];
                            }
                            using (FileStream fileStream = new FileStream(Application.StartupPath + @"\ApplicationData.txt", FileMode.Append))
                            {
                                using (StreamWriter sw = new StreamWriter(fileStream))
                                {//+ "校正后计数总和：" + calibrationData
                                    sw.WriteLine("测量名称：  " + Spec.Name + "快成型：" + port.uQuickCount.ToString() + "慢成型：" + port.uSlowCount.ToString() + "||" + "计数总和：" + countData + "测量时间：" + (port.uSystemTime / 1000).ToString());
                                }
                            }
                        }
                    }
                    #endregion
                    //Console.WriteLine(Spec.SpecData);
                    MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Spec.SpecDatas);
                    port.getParam(out voltage, out current, out cover);
                    if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && (cover != 0))
                    {
                        //Msg.Show(Info.CoverOpen);
                        CloseDevice();
                        StopFlag = true;
                        connect = DeviceConnect.DisConnect;
                        State = DeviceState.Idel;
                        SendMessage(this.OwnerHandle, Wm_OpenCover, 0, 0);
                        return;
                    }

                    ReturnVoltage = voltage * WorkCurveHelper.DeviceCurrent.VoltageScaleFactor;
                    ReturnCurrent = current * InitParam.CurrentRate * WorkCurveHelper.DeviceCurrent.CurrentScaleFactor;

                    #region 调节计数率
                    if (usedTime / 5 >= adjustCountAdd && !bDppErrorInTest)
                        IsAdjust = false;
                    if ((InitParam.IsAdjustRate) && usedTime > 1 && !IsAdjust)
                    {
                        int adjustCount = AdjustInitCountRate();
                        if (adjustCount == 0)
                        {
                            if (DialogResult.OK == Msg.Show(string.Format(Info.strAdjustCountFail, WorkCurveHelper.DeviceCurrent.MaxCurrent), MessageBoxButtons.OK, MessageBoxIcon.Warning))
                            {
                                StopFlag = true;
                                connect = DeviceConnect.DisConnect;
                                break;
                            }
                        }
                        else if (adjustCount == 1)
                        {
                            IsAdjust = true;
                            adjustCountAdd = 1;
                            RayTube.SetXRayTubeParams(InitParam.TubCurrent / InitParam.CurrentRate, InitParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)InitParam.TargetMode);
                            if (_netPortType == NetPortType.DPP100)
                            {
                                DisableDpp100();
                                ClearDataDpp100();
                                EnableDpp100();
                            }
                            else
                            {
                                port.setFPGAParam((byte)FPGAparam.BaseResume, (byte)FPGAparam.BaseLimit, (byte)FPGAparam.HeapUP, (byte)FPGAparam.Rate, (byte)0, (uint)(InitParam.FineGain * 65536),
                           (uint)(InitTime * 1000), (byte)FPGAparam.SendPeakTime, (byte)FPGAparam.SendFlatTop, (byte)FPGAparam.SlowLimit, FPGAparam.Intercept);
                                Array.Clear(port.specData, 0, port.specData.Length);
                            }
                            usedTime = 0;
                            ClearData();
                        }
                        else
                        {
                            IsAdjust = true;
                            adjustCountAdd++;
                        }
                    }
                    #endregion

                    PostMessage(OwnerHandle, WM_ReceiveData, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
                    //Thread.Sleep(500);
                }
                #region 根据快成型门限调节管流
                if (_netPortType != NetPortType.DPP100)
                {
                    double fastAverage = (double)(port.uQuickCount / (port.uSystemTime / 1000f));

                    if (fastAverage > limit && limit > 0)
                    {
                        InitParam.TubCurrent = (int)(InitParam.TubCurrent / (fastAverage / (limit - 2000)));
                        continue;
                    }
                }
                #endregion

                channelErr = InitParam.Channel - MaxChannelRealTime;
                if (_netPortType == NetPortType.DPP100)
                {
                    DisableDpp100();
                    ClearDataDpp100();
                }
                else Array.Clear(port.specData, 0, port.specData.Length);
                //小于允许误差道则不用调节
                if (Math.Abs(channelErr) <= WorkCurveHelper.InitChannelError)
                {
                    Successed = true;
                    InitParam.FineGain = fineGainTemp;
                    break;
                }
                if (CatchChannels.Count < 2)
                {
                    CatchChannels.Add(MaxChannelRealTime);

                }
                if (CatchChannels.Count < 2)
                {
                    fineGainTemp += 0.01f;
                    InitParam.FineGain = fineGainTemp;
                    continue;
                }
                else
                {
                    RATIO = Math.Abs(CatchChannels[1] - CatchChannels[0]);
                }
                if (Math.Abs(channelErr) > MaxChann / 2 || ((fineGainTemp < 0.75 || fineGainTemp > 1.25) && _netPortType == NetPortType.DPP100))
                {
                    break;
                }
                fineGainTemp += (float)(channelErr * 0.01f / RATIO);
                if (double.IsInfinity(fineGainTemp) || double.IsNaN(fineGainTemp) || double.IsNegativeInfinity(fineGainTemp) || double.IsPositiveInfinity(fineGainTemp)) break;
                InitParam.FineGain = fineGainTemp;
                //else
                //{
                //    fineGainTemp *= InitParam.Channel / (float)MaxChannelRealTime;
                //    if (fineGainTemp >= 0.5 && fineGainTemp <= 1.5)
                //    {
                //        InitParam.FineGain = fineGainTemp;
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
                //Successed = (Math.Abs(channelErr) <= WorkCurveHelper.InitChannelError);

            } while (!Successed && (State != DeviceState.Idel) && !StopFlag);
            //if (WorkCurveHelper.DeviceCurrent.HasMotorY1 && IsSpin)
            //{
            //    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, WorkCurveHelper.DeviceCurrent.MotorY1Direct, 0, true, WorkCurveHelper.DeviceCurrent.MotorY1Speed);
            //}

            // CloseDevice();
            State = DeviceState.Idel;
            this.format = new MessageFormat(Info.InitailizeEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            connect = DeviceConnect.DisConnect;
            if (bDppErrorInTest)
                PostMessage(OwnerHandle, Wm_DeviceError, (short)(usedTime <= 0 ? 0 : 1), (int)logErr.DppError);
            else
            {
                //管流下降至30%
                if (WorkCurveHelper.IsUseElect && WorkCurveHelper.IsContinueVol)
                {
                    RayTube.SetXRayTubeParams(InitParam.TubCurrent * 3 / 10 / InitParam.CurrentRate, InitParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)InitParam.TargetMode);
                }
                PostMessage(OwnerHandle, WM_EndInitial, Successed, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
            }
        }

        public void ClearDataDpp100()
        {
            byte[] temps = newNetPortProtocol.ClearSpectrumCommand;
            ((NetPort)port).SetDpp100Parameter(temps, (short)temps.Length);
            Thread.Sleep(200);
        }

        private bool GetDatasFromDpp100(bool bInitial, ref double localTime, ref double accTime, ref double slowCount, ref double slowCountRate, ref string strStatus)
        {
            if (newNetPortProtocol == null) return false;
            //byte[] recDatas = new byte[16390];
            byte[] cmd = new byte[2];
            byte[] temps = newNetPortProtocol.GetStatusCommand;
            Array.Clear(newNetPortProtocol.recDatas, 0, newNetPortProtocol.recDatas.Length);
            cmd[0] = temps[2];
            cmd[1] = temps[3];
            short intrecLen = 0;
            ((NetPort)port).GetDpp100Datas(temps, (short)temps.Length, newNetPortProtocol.recDatas, ref intrecLen, 1000);
            newNetPortProtocol.ProcessExData(cmd, newNetPortProtocol.recDatas);
            strStatus = newNetPortProtocol.GetStatusString(ref accTime, ref localTime, ref slowCount, ref slowCountRate);
            if (localTime <= 0.0001 || localTime > 100000 || slowCount < 0) return false;
            Thread.Sleep(100);
            temps = newNetPortProtocol.GetSpectrumCommand;
            Array.Clear(newNetPortProtocol.recDatas, 0, newNetPortProtocol.recDatas.Length);
            cmd[0] = temps[2];
            cmd[1] = temps[3];
            intrecLen = 0;
            ((NetPort)port).GetDpp100Datas(temps, (short)temps.Length, newNetPortProtocol.recDatas, ref intrecLen, 1000);
            newNetPortProtocol.ProcessExData(cmd, newNetPortProtocol.recDatas);
            //if (!bInitial)
            //{
            //    Array.Clear(newNetPortProtocol.SPECTRUM, 0, DeviceParam.BeginChann);
            //    Array.Clear(newNetPortProtocol.SPECTRUM, DeviceParam.EndChann, (int)WorkCurveHelper.DeviceCurrent.SpecLength - DeviceParam.EndChann);
            //}
            Thread.Sleep(100);
            return true;
        }

        public void EnableDpp100()
        {
            if (newNetPortProtocol == null) return;
            byte[] temps = newNetPortProtocol.EnableMCACommand;
            ((NetPort)port).SetDpp100Parameter(temps, (short)temps.Length);
            Thread.Sleep(200);
        }
        private void SetFineToDpp100(float fineGain)
        {
            if (newNetPortProtocol == null) return;
            newNetPortProtocol.FineGain = fineGain;
            byte[] temps = newNetPortProtocol.SetFineGain;
            ((NetPort)port).SetDpp100Parameter(temps, (short)temps.Length);
            Thread.Sleep(200);
        }
        public void SetTestTimeValueToDpp100(int presetTime)
        {
            if (newNetPortProtocol == null) return;
            newNetPortProtocol.PresetTestTime = presetTime;
            byte[] temps = newNetPortProtocol.SetStopValue;
            ((NetPort)port).SetDpp100Parameter(temps, (short)temps.Length);
            Thread.Sleep(200);
        }
        public void DisableDpp100()
        {
            if (newNetPortProtocol == null) return;
            byte[] temps = newNetPortProtocol.DisabelMCACommand;
            ((NetPort)port).SetDpp100Parameter(temps, (short)temps.Length);
            Thread.Sleep(200);
        }

        public override string GetDppVersion()
        {
            if (_netPortType != NetPortType.DPP100 || newNetPortProtocol == null) return string.Empty;
            if (!IsConnectDevice() || !port.ConnectState)
            {
                Msg.Show(Info.NetDeviceDisConnection);
                StopFlag = true;
                connect = DeviceConnect.DisConnect;
                //PostMessage(OwnerHandle, WM_EndTest, true, (int)usedTime);
                return string.Empty;
            }
            byte[] temps = newNetPortProtocol.GetHardIDCommand;
            byte[] cmd = new byte[2];
            Array.Clear(newNetPortProtocol.recDatas, 0, newNetPortProtocol.recDatas.Length);
            cmd[0] = temps[2];
            cmd[1] = temps[3];
            short intrecLen = 0;
            ((NetPort)port).GetDpp100Datas(temps, (short)temps.Length, newNetPortProtocol.recDatas, ref intrecLen, 1000);
            newNetPortProtocol.ProcessExData(cmd, newNetPortProtocol.recDatas);
            if (newNetPortProtocol.DppVersion == 0xFF)
            {
                string strErr = Info.Dpp100Error + 1 + " (" + Info.CheckSpiLine + ")";
                WorkCurveHelper.specMessage.localMesage.Add(new MessageFormat(strErr, 0));
            }
            return newNetPortProtocol.Dpp100HID;

        }

        #region 峰飘涉及的方法，暂时没有使用

        /// <summary>
        /// 峰漂校正
        /// </summary>
        private void SpectrumCalibrate(ref int[] array)
        {
            float[] fSpec = new float[array.Length];
            Array.Copy(array, fSpec, array.Length);
            int[] qualeArray = new int[array.Length];
            Array.Copy(array, qualeArray, array.Length);
            int smoothTime = int.Parse(ConfigurationSettings.AppSettings["SmoothTimes"]);
            List<QualedInfo> items = this.Quale(Helper.Smooth(qualeArray, smoothTime)); //找到所有符合条件的峰信息
            if (items.Count > 0)
            {
                items = items.OrderByDescending(d => d.ActualPosition).ToList();
                double actualCH1 = items[0].ActualPosition;
                double armCH1 = items[0].ArmPosition;
                int actualInt = (int)Math.Round(actualCH1, MidpointRounding.AwayFromZero);
                int armInt = (int)Math.Round(armCH1, MidpointRounding.AwayFromZero);
                if (Math.Abs(actualCH1 - armCH1) > WorkCurveHelper.PeakErrorChannel)
                {
                    return;
                }
                if (items.Count == 1)
                {
                    if (actualInt != armInt)
                    {
                        SpecProcess.spSpectrumCalibrate(fSpec, array.Length, 0f, (float)armCH1, 0f, (float)actualCH1);
                    }
                }
                else
                {
                    double actualCH2 = items[items.Count - 1].ActualPosition;
                    double armCH2 = items[items.Count - 1].ArmPosition;
                    int actualInt2 = (int)Math.Round(actualCH2, MidpointRounding.AwayFromZero);
                    int armInt2 = (int)Math.Round(armCH2, MidpointRounding.AwayFromZero);
                    if (!(actualInt == armInt && actualInt2 == armInt2))
                    {
                        SpecProcess.spSpectrumCalibrate(fSpec, array.Length, (float)armCH1, (float)armCH2, (float)actualCH1, (float)actualCH2);
                    }
                }
            }
            array = Helper.ToInts(fSpec);
        }


        public List<QualedInfo> Quale(int[] specData)
        {
            List<QualedInfo> qualedlst = new List<QualedInfo>();
            QualedInfo findElem = new QualedInfo();
            data = Helper.ToDoubles(specData);
            peakPositions = QualeElementOperation.Find(data, QualeElement.ChannFWHM, QualeElement.WindowWidth, 20d, 5d, 100d);
            for (int i = 0; i < peakPositions.Length; ++i)
            {
                findElem = StorelyElement();
                if (!findElem.caption.Equals(string.Empty) && qualedlst.FindAll(q => q.caption == findElem.caption).Count == 0)
                {
                    qualedlst.Add(findElem);
                }
            }
            return qualedlst;
        }

        /// <summary>
        /// 定性分析一个峰位
        /// </summary>
        /// <returns></returns>
        private QualedInfo StorelyElement()
        {
            int position = StorelyPoisition(); //按强度大小依次取一个峰位
            int otherPosition = -1;
            QualedInfo result = new QualedInfo();
            if (position < 0)        //找不到峰，结束
            {
                return result;
            }
            double positionEnergy = DemarcateEnergyHelp.GetEnergy(position);
            string[] atomNames;
            string kaNames = Atoms.GetAtom(positionEnergy, XLine.Ka); //得到该峰位附近的Ka系元素
            if (kaNames.Length > 0)
            {
                atomNames = kaNames.Split(new char[] { ' ' });//假设其为Ka系元素，依次取出，找是否存在Kb峰
                foreach (string atomName in atomNames)
                {
                    if (!atomName.Equals(String.Empty))
                    {
                        Atom atom = Atoms.AtomList.Find(w => w.AtomName == atomName);
                        otherPosition = DemarcateEnergyHelp.GetChannel(atom.Kb); // Kb理论峰位
                        otherPosition = StorelyPoisition(otherPosition); // 找到Kb实际位置
                        if (otherPosition > 0) // 若存在，假设成立
                        {
                            result.caption = atom.AtomName;
                            result.ID = atom.AtomID;
                            result.ActualPosition = GetPrecisePos(data, position);
                            result.ArmPosition = DemarcateEnergyHelp.GetDoubleChannel(atom.Ka);
                            if (Math.Abs(result.ActualPosition - result.ArmPosition) > WorkCurveHelper.PeakErrorChannel)
                            {
                                result.caption = string.Empty;
                            }
                            result.Line = 0;
                            RemoveXLines(atom, XLine.Ka, position, otherPosition);
                            break;
                        }
                    }
                }
            }
            if (otherPosition < 0) //在假设前提下找不到Kb参考峰
            {
                string kbNames = Atoms.GetAtom(positionEnergy, XLine.Kb); //更改假设，设其为Kb系元素
                atomNames = kbNames.Split(new char[] { ' ' });
                foreach (string atomName in atomNames)
                {
                    if (!atomName.Equals(String.Empty))
                    {
                        Atom atom = Atoms.AtomList.Find(w => w.AtomName == atomName);
                        otherPosition = DemarcateEnergyHelp.GetChannel(atom.Ka);
                        otherPosition = StorelyPoisition(otherPosition);
                        if (otherPosition > 0)
                        {
                            result.caption = atom.AtomName;
                            result.ID = atom.AtomID;
                            result.ActualPosition = GetPrecisePos(data, position);
                            result.ArmPosition = DemarcateEnergyHelp.GetDoubleChannel(atom.Kb);
                            if (Math.Abs(result.ActualPosition - result.ArmPosition) > WorkCurveHelper.PeakErrorChannel)
                            {
                                //otherPosition = -1;
                            }
                            result.Line = 1;
                            RemoveXLines(atom, XLine.Kb, otherPosition, position);
                            break;
                        }
                    }
                }
            }
            if (otherPosition < 0)//找L
            {
                string LaNames = Atoms.GetAtom(positionEnergy, XLine.La);
                atomNames = LaNames.Split(new char[] { ' ' });
                foreach (string atomName in atomNames)
                {
                    if (!atomName.Equals(String.Empty))
                    {
                        Atom atom = Atoms.AtomList.Find(w => w.AtomName == atomName);
                        otherPosition = DemarcateEnergyHelp.GetChannel(atom.Lb);
                        otherPosition = StorelyPoisition(otherPosition);
                        if (otherPosition > 0)
                        {
                            result.caption = atom.AtomName;
                            result.ID = atom.AtomID;
                            result.ActualPosition = GetPrecisePos(data, position);
                            result.ArmPosition = DemarcateEnergyHelp.GetDoubleChannel(atom.La);
                            if (Math.Abs(result.ActualPosition - result.ArmPosition) > WorkCurveHelper.PeakErrorChannel)
                            {
                                //otherPosition = -1;
                            }
                            result.Line = 2;
                            RemoveXLines(atom, XLine.La, position, otherPosition);
                            break;
                        }
                    }
                }
            }
            if (otherPosition < 0)
            {
                string LbNames = Atoms.GetAtom(positionEnergy, XLine.Lb);
                atomNames = LbNames.Split(new char[] { ' ' });
                foreach (string atomName in atomNames)
                {
                    if (!atomName.Equals(String.Empty))
                    {
                        Atom atom = Atoms.AtomList.Find(w => w.AtomName == atomName);
                        otherPosition = DemarcateEnergyHelp.GetChannel(atom.La);
                        otherPosition = StorelyPoisition(otherPosition);
                        if (otherPosition > 0)
                        {
                            result.caption = atom.AtomName;
                            result.ID = atom.AtomID;
                            result.ActualPosition = GetPrecisePos(data, position);
                            result.ArmPosition = DemarcateEnergyHelp.GetDoubleChannel(atom.Lb);
                            if (Math.Abs(result.ActualPosition - result.ArmPosition) > WorkCurveHelper.PeakErrorChannel)
                            {
                                //otherPosition = -1;
                            }
                            result.Line = 3;
                            RemoveXLines(atom, XLine.Lb, otherPosition, position);
                            break;
                        }
                    }
                }
            }
            if (otherPosition > 0)
            {
                return result;
            }
            else//孤立峰，删除
            {
                data[position] = 0;
                return new QualedInfo();
            }
        }

        private double GetPrecisePos(double[] SpecData, int intPos)
        {
            int low = intPos;
            int high = intPos;
            for (int i = intPos - 5; i > 1; i--)
            {
                if (SpecData[i] < SpecData[i - 1])
                {
                    low = i;
                    break;
                }
            }
            for (int i = intPos + 5; i < SpecData.Length - 1; i++)
            {
                if (SpecData[i] < SpecData[i + 1])
                {
                    high = i;
                    break;
                }
            }
            double precisePos = SpecHelper.FitChannOfMaxValue(low, high, Helper.ToInts(SpecData));
            return precisePos;
        }

        private void RemoveXLines(Atom atom, XLine line, int pa, int pb)
        {
            Double h1, h2;
            if (line == XLine.Ka || line == XLine.Kb)
            {
                h1 = 15;
                h2 = 3;
            }
            else
            {
                h1 = 11;
                h2 = 8;
            }
            Double theoryRate = h2 / h1;
            if (data[pa] * theoryRate > data[pb])
            {
                data[pa] -= data[pb] / theoryRate;
                data[pb] = 0;
            }
            else
            {
                data[pb] -= data[pa] * theoryRate;
                data[pa] = 0;
            }
        }

        /// <summary>
        /// 找当前最高峰位置
        /// </summary>
        private int StorelyPoisition()
        {
            int storelyPosition = -1;
            for (int i = 0; i < peakPositions.Length; i++)
            {
                if (data[peakPositions[i]] <= 0)
                    continue;
                if (storelyPosition < 0)
                {
                    storelyPosition = peakPositions[i];
                    continue;
                }
                if (data[peakPositions[i]] > data[storelyPosition])
                {
                    storelyPosition = peakPositions[i];
                }
            }
            return storelyPosition;
            //if (data[storelyPosition] > 500) //强度阈值, 过滤小杂峰
            //    return storelyPosition;
            //else
            //    return -1;
        }

        private int StorelyPoisition(int position)
        {
            int storelyPosition = -1;
            int errorChann = 15; //channFWHM / 2;
            for (int i = 0; i < peakPositions.Length; i++)
            {
                if (data[peakPositions[i]] <= 0)
                {
                    continue;
                }
                else if (Math.Abs(peakPositions[i] - position) <= errorChann)
                {
                    if (storelyPosition < 0)
                    {
                        storelyPosition = peakPositions[i];
                    }
                    else if (data[storelyPosition] < peakPositions[i])
                    {
                        storelyPosition = peakPositions[i];
                    }
                }
            }

            return storelyPosition;
        }

        #endregion

    }
    /// <summary>
    /// 定性分析到的峰值信息类
    /// </summary>
    public class QualedInfo
    {
        public int ID;
        public string caption;//元素
        public double ActualPosition; //实际位置

        public double ArmPosition; //理论位置
        public int Line; //线系

        public QualedInfo()
        {
            ID = 0;
            caption = string.Empty;
            ActualPosition = 0;
            ArmPosition = 0;
            Line = -1;
        }
    }

    public enum NetPortType
    {
        DMCA = 0,
        DPP100 = 1
    }

}
