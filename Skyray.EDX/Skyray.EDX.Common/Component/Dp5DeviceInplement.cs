using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using Lephone.Data.Common;

namespace Skyray.EDX.Common.Component
{
    public class Dp5DeviceInplement : DeviceInterface
    {
        //const float RATIO = 65.65f;
        
        public Dp5DeviceInplement()
        {
            DbObjectList<QualeElement> QualeElementlIST = QualeElement.FindAll();
            if (QualeElementlIST.Count > 0)
            { QualeElement = QualeElementlIST[0]; }
        }
        public override void SetDp5Cfg()
        {
            if (dp5Device._Dp5Versions == Dp5Version.Dp5_CommonUsb) return;
            dp5Device.OpenDevice();
            dp5Device.LoadDP5Cfg();
            dp5Device.ChangeCfgFineGain(InitParam.FineGain.ToString(), (int)WorkCurveHelper.DeviceCurrent.SpecLength, DeviceParam.PrecTime);
            dp5Device.CloseDevice();
        }

        public override void DoTest(object obj)
        {
            //连接
            if (!port.Connect())
            {
                State = DeviceState.Idel;
                SendMessage(this.OwnerHandle, WM_DeviceDisConnect, 0, 0);
                return;
            }
            Decre1 = new int[Data.Length];
            Time1 = 0;
            usedTime = 0;
            HasRecord = false;
            Count1 = 0;
            //设定仪器参数
            bTimenate = false;
            //测试前先抽真空
            if (Pump.Exist && DeviceParam.IsVacuum && !HasVauccum && !StopFlag)
            {
                Pump.Open();
                this.format = new MessageFormat(Info.TimePumpOpen, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                PumpStart();
            }
            if (Pump.Exist && DeviceParam.IsVacuumDegree && !HasVauccum && !StopFlag)
            {
                Pump.Open();
                this.format = new MessageFormat(Info.SpacePumpOpen, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                do
                {
                    GetReturnParams();

                } while (ReturnVacuum > DeviceParam.VacuumDegree && !StopFlag);
            }
            dp5Device.OpenDevice();
            if (dp5Device._Dp5Versions == Dp5Version.Dp5_CommonUsb)
            {
                numDev = ((Dp5Device)dp5Device).MonitorDevice();
                if (numDev == 0)
                {
                    State = DeviceState.Idel;
                    SendMessage(this.OwnerHandle, WM_DeviceDisConnect, 0, 0);
                    return;
                }
                dp5Device.LoadDP5Cfg();
                dp5Device.ChangeCfgFineGain(InitParam.FineGain.ToString(), (int)WorkCurveHelper.DeviceCurrent.SpecLength, DeviceParam.PrecTime);
            }
            //dp5Device.SetDPPAPIOpen();
            //numDev = dp5Device.MonitorDevice();
            //if (numDev == 0)
            //{
            //    State = DeviceState.Idel;
            //    SendMessage(this.OwnerHandle, WM_DeviceDisConnect, 0, 0);
            //    return;
            //}
            //dp5Device.LoadDP5Cfg();
            //dp5Device.ChangeCfgFineGain(InitParam.FineGain.ToString(), (int)WorkCurveHelper.DeviceCurrent.SpecLength, DeviceParam.PrecTime);
            dp5Device.PauseDppDate(true);
            dp5Device.ClearDppData();
            dp5Device.PauseDppDate(false);
            RayTube.EnableCoverSwitch(!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover);
            RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
            RayTube.Open();
            bool dropData = false;
            
            //int adjustCount = 0;//调节计数率的次数，大于10次测调节失败，结束测量
            State = DeviceState.Test;//仪器连接上后才改变其状态 0324
            connect = DeviceConnect.Connect;
            MessageFormat format = new MessageFormat(Info.SpectrumTest, 0);
            WorkCurveHelper.specMessage.localMesage.Add(format);
            base.ClearData();
            Data.CopyTo(DataCopy, 0);
            bool InitBoolean = false;
            bool IsPeakFloatInit = true;
            bool IsAdjust = true;
            int adjustCountAdd = 1;
            int numFailFromSDD = 0;
            while ((usedTime < DeviceParam.PrecTime) && (State != DeviceState.Idel) && !StopFlag&&!bTimenate)
            {
               
                if (State == DeviceState.Pause)
                {
                    dp5Device.PauseDppDate(true);
                    format = new MessageFormat(Info.PauseStop, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                while (State == DeviceState.Pause)
                {
                    Thread.Sleep(200);
                }
                if (State == DeviceState.Resume)
                {
                    dp5Device.PauseDppDate(false);
                    State = DeviceState.Test;
                    format = new MessageFormat(Info.SpectrumTest, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                Thread.Sleep(500);
                if (State == DeviceState.Break)
                    break;
                RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
                
                bool isSddEnabled=true;
                double FastCount=0;
                double SlowCount=0;

                if (!dp5Device.GetData(Data, ref isSddEnabled, ref usedTime, ref FastCount, ref SlowCount))
                {
                    numFailFromSDD++;
                    if (numFailFromSDD > 10)
                    {
                        bTimenate = true;
                    }
                }
                else
                {
                    if ((!dropData) && (usedTime >= DropTime) && usedTime > 0 && DropTime > 0)
                    {
                        dropData = true;
                        ClearData();
                        Time1 = 0;
                        Array.Clear(Decre1, 0, Decre1.Length);
                        HasRecord = false;
                        Count1 = 0;
                        continue;
                    }
                    for (int i = 0; i < DeviceParam.BeginChann; i++)
                        Data[i] = 0;
                    for (int j = DeviceParam.EndChann; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength; j++)
                        Data[j] = 0;

                    Data.CopyTo(DataCopy, 0);
                    #region 自动校正峰飘3
                    if (WorkCurveHelper.IPeakCalibration && WorkCurveHelper.IDemarcateTest == 0)
                    {
                        int[] smoothData = new int[DataCopy.Length];
                        DataCopy.CopyTo(smoothData, 0);
                        int smoothTime = int.Parse(ConfigurationSettings.AppSettings["SmoothTimes"]);
                        MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(DeviceParam.BeginChann, DeviceParam.EndChann, Helper.Smooth(smoothData, smoothTime));
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
                    MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Spec.SpecDatas);
                    //TestTotalCount = (int)Math.Round(SlowCount, MidpointRounding.AwayFromZero);
                    TestTotalCount = SlowCount;
                    CountRating = CountRate();
                    if (!HasRecord)
                    {
                        DataCopy.CopyTo(Decre1, 0);
                        Time1 = usedTime;
                        Count1 = SlowCount;
                        HasRecord = true;
                    }
                    GetReturnParams();

                    #region
                    if (DeviceParam.IsPeakFloat && IsPeakFloatInit && usedTime >= DeviceParam.PeakCheckTime)
                    {
                        MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(DeviceParam.PeakFloatLeft, DeviceParam.PeakFloatRight, Spec.SpecDatas);
                        int ChannelError = DeviceParam.PeakFloatChannel - Convert.ToInt32(MaxChannelRealTime);
                        if (Math.Abs(ChannelError) > DeviceParam.PeakFloatError)
                        {
                            if (DialogResult.OK == Msg.Show(Info.PeakMove, MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                            {
                                StopFlag = true;
                                connect = DeviceConnect.DisConnect;
                                InitBoolean = true;
                                break;
                            }
                            else
                            {
                                IsPeakFloatInit = false;
                            }
                        }
                    }
                    #endregion
                    if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && ReturnCoverClosed)
                    {
                        CloseDevice();
                        Msg.Show(Info.CoverOpen);
                        StopFlag = true;
                        connect = DeviceConnect.DisConnect;
                        break;
                    }
                    #region 中达高压锁
                    if (IsExistsLock && !StopFlag)
                    {
                        int intVolgate = 0, intCurrent = 0, iTemp = 0, iVacuum = 0;
                        bool iCoverClose = false;
                        if (port != null)
                            port.GetParams(ref intVolgate, ref intCurrent, ref iTemp, ref iVacuum, ref iCoverClose);
                        if (iVacuum == 0)
                        {
                            Msg.Show(Info.VoltageInfo);
                            StopFlag = true;
                            Pump.Close();
                            connect = DeviceConnect.DisConnect;
                            break;
                        }
                    }
                    #endregion
                    if (usedTime / WorkCurveHelper.AdjustTime >= adjustCountAdd)
                        IsAdjust = false;
                    if ((DeviceParam.IsAdjustRate) && usedTime > 1 && !IsAdjust)
                    {
                        int adjustCount = AdjustCountRate();
                        if (adjustCount == 0)
                        {
                            string sSimilarCurve = ReportTemplateHelper.LoadSpecifiedValue("EDX3000", "SimilarCurve");
                            if (sSimilarCurve == "1")
                            {
                                MatchPlus = MatchPlus.MatchOn;
                            }
                            else
                            {
                                Msg.Show(string.Format(Info.strAdjustCountFail, WorkCurveHelper.DeviceCurrent.MaxCurrent), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            StopFlag = true;
                            connect = DeviceConnect.DisConnect;
                            break;
                        }
                        else if (adjustCount == 1)
                        {
                            IsAdjust = true;
                            adjustCountAdd = 1;
                            //yuzhao20150609_begin:yuzhao20150609_begin:根据戴晓玲需求更改为保存调节计数率后的条件
                            Condition cd = Condition.FindOne(c => c.Id == WorkCurveHelper.WorkCurveCurrent.Condition.Id && c.Device.Id == WorkCurveHelper.DeviceCurrent.Id);
                            if (cd != null)
                            {
                                cd.DeviceParamList[0].TubCurrent = DeviceParam.TubCurrent;
                                WorkCurveHelper.WorkCurveCurrent.Condition = cd;
                                cd.Save();
                            }
                            //yuzhao20150609_end
                        }
                        else
                        {
                            IsAdjust = true;
                            adjustCountAdd++;
                        }
                   }

                }
                   PostMessage(OwnerHandle, WM_ReceiveData, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));

             }
            for (int i = 0; i < DataCopy.Length; i++)
            {
                if (usedTime - Time1 == 0)
                    break;
                double newChannelData = (DataCopy[i] - Decre1[i]);// *DeviceParam.PrecTime / (usedTime - Time1);
                DataCopy[i] = (int)Math.Round(newChannelData, MidpointRounding.AwayFromZero);
            }
            usedTime -= Time1;
            Spec.SpecData = TabControlHelper.ConvertArrayToString(DataCopy);
            Spec.TubCurrent = ReturnCurrent;
            Spec.TubVoltage = ReturnVoltage;
            Spec.UsedTime = usedTime;
            dp5Device.PauseDppDate(true);
            dp5Device.ClearDppData();
            dp5Device.CloseDevice();
            this.format = new MessageFormat(Info.SpectrumEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            PostMessage(OwnerHandle, WM_EndTest, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
            State = DeviceState.Idel;
            connect = DeviceConnect.DisConnect;
            if (InitBoolean)
                PostMessage(OwnerHandle, Wm_ProcessInit, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
        }

        public override void ClearData()
        {
            base.ClearData();
            dp5Device.PauseDppDate(true);
            dp5Device.ClearDppData();
            dp5Device.PauseDppDate(false);
        }


        public override int AdjustCountRate()
        {
            double rate;
            rate = CountRate();
            if (rate != 0)
            {
                int current = RayTube.Current;
                int tempCurrent = current;
                tempCurrent = rate < DeviceParam.MinRate ? (int)Math.Round(current * DeviceParam.MinRate / rate) : current;
                tempCurrent = rate > DeviceParam.MaxRate ? (int)Math.Round(current * DeviceParam.MaxRate / rate) : tempCurrent;
                if (rate < DeviceParam.MinRate)
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
                        DeviceParam.TubCurrent = tempCurrent * DeviceParam.CurrentRate;
                    ClearData();
                    Count1 = 0;
                    Time1 = 0;
                    usedTime = 0;
                    return 1;
                }
                else if (rate > DeviceParam.MaxRate)
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
                        DeviceParam.TubCurrent = tempCurrent * DeviceParam.CurrentRate;
                    usedTime = 0;
                    Time1 = 0;
                    Count1 = 0;
                    ClearData();
                    return 1;
                }
            }
            return 2;
        }

        public override double CountRate()
        {
            double totalCount = 0;
            if (usedTime > 0)
            {
                if (currentRateMode == CountingRateModes.Average && usedTime - Time1>0)
                    totalCount = (TestTotalCount - Count1) / (usedTime - Time1);
                else
                    totalCount = (TestTotalCount - Count1) - m_lastCountRate;

            }
            m_lastCountRate = totalCount;
            if (totalCount < 0)
                totalCount = 0;
            return totalCount;
        }
        public override void SetOpenCurrent()
        {
            RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
           
        }
        public override void PreOpenVoltage()
        {
            //dp5Device.OpenDevice();
            //dp5Device.LoadDP5Cfg(AppDomain.CurrentDomain.BaseDirectory + "\\dp5.cfg");
            //dp5Device.SetBeforeTesting((int)heatParams.FineGain, heatParams.PreHeatTime);
            //设定仪器参数
            RayTube.SetXRayTubeParams(heatParams.TubCurrent, heatParams.TubVoltage, (int)heatParams.Gain, (int)heatParams.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)heatParams.TargetMode);
            RayTube.Open();
        }
        public override string GetDppVersion()
        {
            return string.Empty;
        }
        public override int AdjustInitCountRate()
        {
            double rate;
            rate = CountRate();
            if (rate != 0)
            {
                int current = RayTube.Current;
                int tempCurrent = current;
                tempCurrent = rate < InitParam.MinRate ? (int)Math.Round(current * InitParam.MinRate / rate) : current;
                tempCurrent = rate > InitParam.MaxRate ? (int)Math.Round(current * InitParam.MaxRate / rate) : tempCurrent;
                if (rate < InitParam.MinRate)
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
                        InitParam.TubCurrent = tempCurrent * InitParam.CurrentRate;
                    ClearData();
                    usedTime = 0;
                    return 1;
                }
                else if (rate > InitParam.MaxRate)
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
                        InitParam.TubCurrent = tempCurrent * InitParam.CurrentRate;
                    usedTime = 0;
                    ClearData();
                    return 1;
                }
            }
            return 2;
        }


        public override void DoInitial(object obj)
        {
            //连接
            if (!port.Connect())
            {
                SendMessage(this.OwnerHandle, WM_DeviceDisConnect, 0, 0);
                State = DeviceState.Idel;
                return;
            }
            //PostMessage(OwnerHandle, Wm_Connection, 0, usedTime);
            //int rate = 0;
            //int preGain = 0;
            double RATIO = 210;
            List<double> CatchChannels = new List<double>();//用于计算RATIO
            int preFinGain = 0;
            double channelErr = 0;
            double preChannelError = 0;
            double fineGainRate =8;
            bool Successed = false;
            Time1 = 0;
            List<int> lstFineGain = new List<int>();//记录调节的精调
            dp5Device.OpenDevice();
            dp5Device.LoadDP5Cfg();
            dp5Device.ChangeCfgFineGain(InitParam.FineGain.ToString(), (int)WorkCurveHelper.DeviceCurrent.SpecLength, DeviceParam.PrecTime);
            dp5Device.PauseDppDate(true);
            dp5Device.ClearDppData();
            dp5Device.PauseDppDate(false);
            //设定仪器参数
            RayTube.EnableCoverSwitch(!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover);
            RayTube.SetXRayTubeParams(InitParam.TubCurrent / InitParam.CurrentRate, InitParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)InitParam.TargetMode);
            RayTube.Open();
            bTimenate = false;
            base.ClearData();
            Data.CopyTo(DataCopy, 0);
            int numFailFromSDD = 0;
            do
            {
                usedTime = 0;
                connect = DeviceConnect.Connect;

                bool IsAdjust = true;
                int adjustCountAdd = 1;

                while ((usedTime < InitTime) && (State != DeviceState.Idel) && !StopFlag&& !bTimenate)
                {
                    if (State == DeviceState.Pause)
                    {
                        dp5Device.PauseDppDate(true);
                        format = new MessageFormat(Info.PauseStop, 0);
                        WorkCurveHelper.specMessage.localMesage.Add(format);
                    }
                    while (State == DeviceState.Pause)
                    {
                        Thread.Sleep(200);
                    }
                    if (State == DeviceState.Resume)
                    {
                        dp5Device.PauseDppDate(false);
                        State = DeviceState.Test;
                        format = new MessageFormat(Info.SpectrumInitialize, 0);
                        WorkCurveHelper.specMessage.localMesage.Add(format);
                    }
                    Thread.Sleep(500);
                    RayTube.SetXRayTubeParams(InitParam.TubCurrent / InitParam.CurrentRate, InitParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)InitParam.TargetMode);

                    bool isSddEnabled=true;
                    double FastCount=0;
                    double SlowCount=0;

                    if (!dp5Device.GetData(Data, ref isSddEnabled, ref usedTime, ref FastCount, ref SlowCount))
                    {
                        numFailFromSDD++;
                        if (numFailFromSDD > 10)
                        {
                            bTimenate = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 50; i++)
                             Data[i] = 0;
                        for (int j = (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength; j++)
                             Data[j] = 0;
                        Spec.SpecData = TabControlHelper.ConvertArrayToString(Data);
                        Spec.TubCurrent = ReturnCurrent;
                        Spec.TubVoltage = ReturnVoltage;
                        Data.CopyTo(DataCopy, 0);
                        //TestTotalCount=(int)Math.Round(SlowCount, MidpointRounding.AwayFromZero);
                        TestTotalCount = SlowCount;
                        CountRating = CountRate();
                        GetReturnParams();
                        if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && ReturnCoverClosed)
                        {
                            CloseDevice();
                            Msg.Show(Info.CoverOpen);
                            StopFlag = true;
                            connect = DeviceConnect.DisConnect;
                            break;
                        }
                        if (IsExistsLock && ReturnVoltage == 0 && !StopFlag)
                        {
                            Msg.Show(Info.VoltageInfo);
                            StopFlag = true;
                            connect = DeviceConnect.DisConnect;
                            break;
                        }

                        #region 调节计数率
                        if (usedTime / WorkCurveHelper.AdjustTime >= adjustCountAdd)
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
                            }
                            else
                            {
                                IsAdjust = true;
                                adjustCountAdd++;
                            }
                        }

                        #endregion
                    }
                    PostMessage(OwnerHandle, WM_ReceiveData, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
                }
                Thread.Sleep(200);
                MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Spec.SpecDatas);
                channelErr = InitParam.Channel - MaxChannelRealTime;
                Console.WriteLine("channelErr:" + channelErr);
                Console.WriteLine("preFinGain:" + preFinGain);
                Console.WriteLine("preChannelError:" + preChannelError);
                Console.WriteLine("MaxChannelRealTime:" + MaxChannelRealTime);
                if (CatchChannels.Count < 2 && dp5Device.Dp5Versions != Dp5Version.Dp5_CommonUsb)
                {
                    CatchChannels.Add(MaxChannelRealTime);
                }
                if (dp5Device.Dp5Versions != Dp5Version.Dp5_CommonUsb && CatchChannels.Count >= 2)
                {
                    RATIO = Math.Abs(CatchChannels[1] - CatchChannels[0]);
                }
                //modify by chuyaqin
                Successed = (Math.Abs(channelErr) <= InitParam.ChannelError + 0.9);
                if (Successed)
                {
                    break;
                }
                else if (CatchChannels.Count < 2 && dp5Device.Dp5Versions != Dp5Version.Dp5_CommonUsb)
                {

                    dp5Device.PauseDppDate(true);
                    dp5Device.ClearDppData();
                    dp5Device.ChangeCfgFineGain((InitParam.FineGain + 1).ToString(), (int)WorkCurveHelper.DeviceCurrent.SpecLength, DeviceParam.PrecTime);
                    dp5Device.PauseDppDate(false);
                    continue;
                }
                else
                {
                    if (Math.Abs(channelErr) > MaxChann / 2)
                    {
                        break;
                    }
                    if (dp5Device.Dp5Versions == Dp5Version.Dp5_CommonUsb)
                    {
                        if (Math.Abs(channelErr) >= 5)
                        {
                            if (preFinGain != 0)
                            {
                                if (preChannelError == channelErr)
                                {

                                }
                                else
                                {
                                    fineGainRate = 1.0 * preFinGain / (preChannelError - channelErr);

                                }
                            }
                            if (fineGainRate < 1)
                            {
                                fineGainRate = 1;
                            }
                            preFinGain = (int)Math.Round(channelErr * fineGainRate);
                            InitParam.FineGain += preFinGain;
                        }
                        else
                        {
                            fineGainRate /= 1.5;
                            if (fineGainRate < 1)
                            {
                                fineGainRate = 1;
                            }
                            preFinGain = (int)Math.Round(channelErr * fineGainRate);
                            InitParam.FineGain += preFinGain;
                        }
                    }
                    else
                    {
                        InitParam.FineGain += (float)(channelErr / RATIO);
                    }
                    Console.WriteLine("fineGainRate:" + fineGainRate);
                    Console.WriteLine("FineGain:" + InitParam.FineGain);
                    Console.WriteLine("\n");
                    preChannelError = channelErr;
                }
                //Successed = (Math.Abs(channelErr) <= InitParam.ChannelError+0.5);modify by chuyaqin
                //dp5Device.SetBeforeTesting((int)InitParam.FineGain, DeviceParam.PrecTime);
                dp5Device.PauseDppDate(true);
                dp5Device.ClearDppData();
                //InitParam.FineGain = 8;
                dp5Device.ChangeCfgFineGain(InitParam.FineGain.ToString(), (int)WorkCurveHelper.DeviceCurrent.SpecLength, DeviceParam.PrecTime);
                dp5Device.PauseDppDate(false);
            } while (!Successed && (State != DeviceState.Idel) && !StopFlag);//0819
            this.format = new MessageFormat(Info.InitailizeEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            dp5Device.PauseDppDate(true);
            dp5Device.ClearDppData();
            dp5Device.CloseDevice();
            CloseDevice();
            PostMessage(OwnerHandle, WM_EndInitial, Successed, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
            //if (ExistMagnet)
            //{
            //    Pump.Close();
            //    port.CloseVoltage();
            //    port.CloseVoltageLamp();
            //}
            State = DeviceState.Idel;
            connect = DeviceConnect.DisConnect;
        }


 
    }
}