using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;

namespace Skyray.EDX.Common.Component
{

    public class UsbDeviceInplement : DeviceInterface
    {
        public const double InitError = 0.5;//内部0.5的初始化误差道
        public UsbDeviceInplement()
        {

        }

        public override void InitDevice()
        {
            Initialize(2048);
        }
        public override void SetDp5Cfg()
        {
        }
        public override string GetDppVersion()
        {
            return string.Empty;
        }
        #region DeviceInterface Members

        public override void DoTest(object obj)
        {
            DeviceState currentState = (DeviceState)obj;
            if (!port.Connect())
            {
                this.format = new MessageFormat(Info.ConnectionDevice, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                PostMessage(OwnerHandle, WM_DeviceDisConnect, 0, (int)usedTime);
                State = DeviceState.Idel;
                connect = DeviceConnect.DisConnect;
                StopFlag = true;
                return;
            }
            if (ExistMagnet)
            {
                Pump.Open();
            }
            if (Pump.Exist && DeviceParam.IsVacuum && !HasVauccum && !StopFlag)
            {
                Pump.Open();
                this.format = new MessageFormat(Info.TimePumpOpen, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                PumpStart();
            }
            usedTime = 0;
            IfAdjustOk = false;
            connect = DeviceConnect.Connect;
            bool DropData = false;
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
            RayTube.EnableCoverSwitch(!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover);
            if (DeviceParam != null)
                RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
            RayTube.Open();
            while (PauseStop || !port.GetData(Data))
            {
                if (StopFlag)
                {
                    State = DeviceState.Idel;
                    connect = DeviceConnect.DisConnect;
                    port.CloseVoltage();
                    return;
                }
            }
            MessageFormat format = new MessageFormat(Info.SpectrumTest, 0);
            WorkCurveHelper.specMessage.localMesage.Add(format);
            do
            {
                Thread.Sleep(50);
            }
            while (!port.GetData(Data));
            ClearData();
            Data.CopyTo(DataCopy, 0);
            bool InitBoolean = false;
            preUsedTime = 0;
            bool IsPeakFloatInit = true;
            if (DeviceParam != null)
            {
                //if (WorkCurveHelper.DeviceCurrent.HasMotorY1 && IsSpin && WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))
                //{
                //    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, WorkCurveHelper.DeviceCurrent.MotorY1Direct, 0, true, WorkCurveHelper.DeviceCurrent.MotorY1Speed);
                //}
                while ((usedTime < DeviceParam.PrecTime) && (currentState != DeviceState.Idel) && !StopFlag)
                {
                Begin:
                    RayTube.SetXRayTubeParams(DeviceParam.TubCurrent / DeviceParam.CurrentRate, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);

                    if ((!DropData) && (usedTime >= DropTime) && DropTime > 0)
                    {
                        DropData = true;
                        ClearData();
                        usedTime = 0;
                    }

                    DateTime dtFirst = DateTime.Now;
                    do
                    {
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
                            int[] dropData = new int[Data.Length];
                            while (StopFlag || PauseStop || !port.GetData(dropData))
                            {
                                Thread.Sleep(50);
                            }
                            State = DeviceState.Test;
                            format = new MessageFormat(Info.SpectrumTest, 0);
                            WorkCurveHelper.specMessage.localMesage.Add(format);
                            goto Begin;
                        }
                        Thread.Sleep(50);
                        DateTime dtEnd = DateTime.Now;
                        if ((dtEnd - dtFirst).TotalSeconds > 10)
                        {
                            port.Connect();
                            dtFirst = dtEnd;
                        }
                        if (PauseStop || StopFlag)
                        {
                            port.CloseVoltage();
                            if (StopFlag)
                                break;
                        }
                    }
                    while (StopFlag || PauseStop || !port.GetData(Data));
                    if (State == DeviceState.Break)
                        break;
                    if (!WorkCurveHelper.IsAutoIncrease) usedTime++;
                    for (int j = 0, z = 0; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength && z < Data.Length; j++)
                    {
                        if (WorkCurveHelper.DeviceCurrent.SpecLength == SpecLength.Min)
                        {
                            backData[j] = Data[z] + Data[z + 1];
                            z = z + 2;
                        }
                        else
                        {
                            backData[j] = Data[z];
                            z = z + 1;
                        }
                    }
                    for (int i = 0; i < DeviceParam.BeginChann; i++)
                        backData[i] = 0;
                    for (int j = DeviceParam.EndChann; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength; j++)
                        backData[j] = 0;
                    Spec.SpecData = TabControlHelper.ConvertArrayToString(backData);
                    Spec.TubCurrent = ReturnCurrent;
                    Spec.TubVoltage = ReturnVoltage;
                    backData.CopyTo(DataCopy, 0);
                    GetReturnParams();

                    #region 峰飘提示
                    if (DeviceParam.IsPeakFloat && IsPeakFloatInit && usedTime == DeviceParam.PeakCheckTime)
                    {
                        MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(DeviceParam.PeakFloatLeft, DeviceParam.PeakFloatRight, Spec.SpecDatas);
                        int ChannelErrorp = DeviceParam.PeakFloatChannel - Convert.ToInt32(MaxChannelRealTime);
                        if (Math.Abs(ChannelErrorp) > DeviceParam.PeakFloatError)
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

                    #region 开盖提示
                    if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && ReturnCoverClosed && !StopFlag)
                    {
                       // Msg.Show(Info.CoverOpen);
                        StopFlag = true;
                        connect = DeviceConnect.DisConnect;

                        SendMessage(this.OwnerHandle, Wm_OpenCover, 0, 0);
                        break;
                    }
                    #endregion

                    MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Spec.SpecDatas);
                    CountRating = CountRate();
                    if (!IfAdjustOk)//当自动调节计数率OK后，就不在调节
                    {
                        if ((DeviceParam.IsAdjustRate) && (usedTime % WorkCurveHelper.AdjustTime == 0))
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
                            else if (adjustCount == 3)
                            {
                                IfAdjustOk = true;
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
                        }
                    }
                    PostMessage(OwnerHandle, WM_ReceiveData, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
                }
            }
            //if (WorkCurveHelper.DeviceCurrent.HasMotorY1 && IsSpin && WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))
            //{
            //    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, WorkCurveHelper.DeviceCurrent.MotorY1Direct, 0, false, WorkCurveHelper.DeviceCurrent.MotorY1Speed);
            //}
            this.format = new MessageFormat(Info.SpectrumEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            PostMessage(OwnerHandle, WM_EndTest, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
            connect = DeviceConnect.DisConnect;
            this.State = DeviceState.Idel;
            if (InitBoolean)
                PostMessage(OwnerHandle, Wm_ProcessInit, true, (int)usedTime);
        }
        public override void SetOpenCurrent()
        { }
        public override void PreOpenVoltage()
        {
            RayTube.SetXRayTubeParams(this.heatParams.TubCurrent / this.heatParams.CurrentRate, this.heatParams.TubVoltage, (int)this.heatParams.Gain, (int)this.heatParams.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)heatParams.TargetMode);
            RayTube.Open();
            GetReturnParams();
        }

        public override void DoInitial(object obj)
        {
            DeviceState currentState = (DeviceState)obj;
            if (!port.Connect())
            {
                this.format = new MessageFormat(Info.ConnectionDevice, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                PostMessage(OwnerHandle, WM_DeviceDisConnect, 0, (int)usedTime);
                //if (OnDeviceConnect != null)
                //    OnDeviceConnect(usedTime);
                State = DeviceState.Idel;
                connect = DeviceConnect.DisConnect;
                StopFlag = true;
                return;
            }
            //PostMessage(OwnerHandle, Wm_Connection, 0, usedTime);
            usedTime = 0;
            IfAdjustOk = false;
            connect = DeviceConnect.Connect;
            double rate = 0;
            int preGain = 0;
            double ChannelError = 0;
            double fineGainRate = 6;
            int preFineGain = 0;
            double preChannel = 0;
            bool Successed = false;


            RayTube.EnableCoverSwitch(!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover);
            RayTube.SetXRayTubeParams(InitParam.TubCurrent / InitParam.CurrentRate, InitParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)InitParam.TargetMode);
            RayTube.Open();
            while (PauseStop || !port.GetData(Data))
            {
                if (StopFlag)
                {
                    State = DeviceState.Idel;
                    connect = DeviceConnect.DisConnect;
                    return;
                }
                if (PauseStop)
                    port.CloseVoltage();
            }
            //if (WorkCurveHelper.DeviceCurrent.HasMotorY1 && IsSpin && WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))
            //{
            //    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, WorkCurveHelper.DeviceCurrent.MotorY1Direct, 0, true, WorkCurveHelper.DeviceCurrent.MotorY1Speed);
            //}
            do
            {
                ClearData();
                usedTime = 0;
                preUsedTime = 0;
                connect = DeviceConnect.Connect;
                this.format = new MessageFormat(Info.SpectrumInitialize, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                while ((usedTime < InitTime) && currentState != DeviceState.Idel && !StopFlag)
                {
                Begin:
                    RayTube.SetXRayTubeParams(InitParam.TubCurrent / InitParam.CurrentRate, InitParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)InitParam.TargetMode);
                    do
                    {

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
                            int[] dropData = new int[Data.Length];
                            while (StopFlag || PauseStop || !port.GetData(dropData))
                            {
                                Thread.Sleep(50);
                            }
                            State = DeviceState.Test;
                            format = new MessageFormat(Info.SpectrumInitialize, 0);
                            WorkCurveHelper.specMessage.localMesage.Add(format);
                            goto Begin;
                        }
                        Thread.Sleep(50);
                        if (PauseStop || StopFlag)
                        {
                            port.CloseVoltage();
                            if (StopFlag)
                                break;
                        }
                    }
                    while (StopFlag || PauseStop || !port.GetData(Data));
                    if (!WorkCurveHelper.IsAutoIncrease) usedTime++;

                    for (int j = 0, z = 0; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength && z < Data.Length; j++)
                    {
                        if (WorkCurveHelper.DeviceCurrent.SpecLength == SpecLength.Min)
                        {
                            backData[j] = Data[z] + Data[z + 1];
                            z = z + 2;
                        }
                        else
                        {
                            backData[j] = Data[z];
                            z = z + 1;
                        }
                    }
                    for (int i = 0; i < 50; i++)
                        backData[i] = 0;
                    for (int j = (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength; j++)
                        backData[j] = 0;
                    Spec.SpecData = TabControlHelper.ConvertArrayToString(backData);
                    //Spec.SpecOrignialData = Spec.SpecData;
                    MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Spec.SpecDatas);
                    //Data.CopyTo(DataCopy, 0);
                    backData.CopyTo(DataCopy, 0);
                    CountRating = CountRate();
                    GetReturnParams();
                    if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && ReturnCoverClosed)
                    {
                        //Msg.Show(Info.CoverOpen);
                        StopFlag = true;
                        connect = DeviceConnect.DisConnect;

                        SendMessage(this.OwnerHandle, Wm_OpenCover, 0, 0);
                        break;
                    }
                    #region 调节计数率
                    if (!IfAdjustOk)//当自动调节计数率OK后，就不在调节
                    {
                        if ((InitParam.IsAdjustRate) && (usedTime % WorkCurveHelper.AdjustTime == 0))
                        {
                            int adjustCount = AdjustInitCountRate();
                            if (adjustCount == 0)
                            {
                                if (DialogResult.OK == Msg.Show(string.Format(Info.strAdjustCountFail, WorkCurveHelper.DeviceCurrent.MaxCurrent),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning))
                                {
                                    StopFlag = true;
                                    connect = DeviceConnect.DisConnect;
                                    break;
                                }
                            }
                            else if (adjustCount == 3)
                            {
                                IfAdjustOk = true;
                            }
                        }
                    }
                    #endregion
                    PostMessage(OwnerHandle, WM_ReceiveData, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
                }
                ChannelError = InitParam.Channel - MaxChannelRealTime;//edit by chuyaqin ,比较通道时应用double型。
                //在允许误差的范围内，不用调节
                if (Math.Abs(ChannelError) <= InitParam.ChannelError + InitError)
                {
                    Thread.Sleep(100);
                    Successed = true;
                    break;
                }
                //误差小于5道进行细调
                if (Math.Abs(ChannelError) < 15)
                {
                    if (preFineGain != 0)
                        fineGainRate = preFineGain / ChannelError;
                    if (fineGainRate < 2)
                        fineGainRate = 2;
                    preFineGain = Convert.ToInt32(ChannelError * fineGainRate);
                    InitParam.FineGain += preFineGain;
                }//误差太大无法调节
                else if (Math.Abs(ChannelError) > MaxChann / 2)
                {
                    break;
                }
                else//进行粗调
                {
                    if (preGain != 0)
                        rate = (preChannel - ChannelError) / preGain;
                    else
                        rate = 6;
                    if (rate < 2)
                        rate = 2;
                    preChannel = ChannelError;
                    preGain = Convert.ToInt32(ChannelError / rate);
                    InitParam.Gain += preGain;
                }
                if (InitParam.FineGain <= 0 || InitParam.FineGain >= 250 || InitParam.Gain <= 0 || InitParam.Gain >= 250)
                {
                    Successed = false;
                    break;
                }
            } while (!Successed && (State != DeviceState.Idel) && !StopFlag);
            //if (WorkCurveHelper.DeviceCurrent.HasMotorY1 && IsSpin && WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))
            //{
            //    port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorY1Code, WorkCurveHelper.DeviceCurrent.MotorY1Direct, 0, false, WorkCurveHelper.DeviceCurrent.MotorY1Speed);
            //}
            this.format = new MessageFormat(Info.InitailizeEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            PostMessage(OwnerHandle, WM_EndInitial, Successed, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
            CloseDevice();
            connect = DeviceConnect.DisConnect;
            State = DeviceState.Idel;
        }

        #endregion


    }


}
