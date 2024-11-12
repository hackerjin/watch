using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Skyray.EDX.Common.Component
{
    public class ParallelDeviceImplement : DeviceInterface
    {
        public const double InitError = 0.5;//内部0.5的初始化误差道
        [DllImport("kernel32.dll")]
        private static extern int Beep(int dwFreq, int dwDuration);

        public override void SetDp5Cfg()
        {
        }
        public override string GetDppVersion()
        {
            return string.Empty;
        }
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
            usedTime = 0;
            ClearData();
            connect = DeviceConnect.Connect;
            bool DropData = false;
            RayTube.SetXRayTubeParams(DeviceParam.TubCurrent, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
            //port.SetParam(DeviceParam.TubVoltage, DeviceParam.TubCurrent, (int)InitParam.Gain, (int)InitParam.FineGain);
            MessageFormat format = new MessageFormat(Info.SpectrumTest, 0);
            WorkCurveHelper.specMessage.localMesage.Add(format);
            Data.CopyTo(DataCopy, 0);
            bool InitBoolean = false;
            bool IsPeakFloatInit = true;
            preUsedTime = 0;
            if (DeviceParam != null)
            {
                while ((usedTime < DeviceParam.PrecTime) && (currentState != DeviceState.Idel) && !StopFlag)
                {
                    //port.SetParam(DeviceParam.TubVoltage, DeviceParam.TubCurrent, (int)InitParam.Gain, (int)InitParam.FineGain);
                    if ((!DropData) && (usedTime >= DropTime) && DropTime > 0)
                    {
                        DropData = true;
                        ClearData();
                        usedTime = 0;
                        RayTube.SetXRayTubeParams(DeviceParam.TubCurrent, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
                        //port.SetParam(DeviceParam.TubVoltage, DeviceParam.TubCurrent, (int)InitParam.Gain, (int)InitParam.FineGain);
                    }
                    while (StopFlag || PauseStop || !port.GetData(Data))
                    {
                        Thread.Sleep(50);
                        if (PauseStop || StopFlag)
                        {
                            port.CloseVoltage();
                            if (StopFlag)
                                break;
                        }
                    }
                    Beep(0X7FF, 100);
                    
                    usedTime++;
                    for (int i = 0; i < DeviceParam.BeginChann; i++)
                        Data[i] = 0;
                    for (int j = DeviceParam.EndChann; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength; j++)
                        Data[j] = 0;
                    Spec.SpecData = TabControlHelper.ConvertArrayToString(Data);
                    Data.CopyTo(DataCopy, 0);
                    ReturnVoltage = DeviceParam.TubVoltage;
                    ReturnCurrent = DeviceParam.TubCurrent;
                    if (DeviceParam.IsPeakFloat && IsPeakFloatInit && usedTime == DeviceParam.PeakCheckTime)
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
                    CountRating = CountRate();
                    MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Spec.SpecDatas);
                    if ((DeviceParam.IsAdjustRate) && (usedTime % 5 == 0))
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
                            RayTube.SetXRayTubeParams(DeviceParam.TubCurrent, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
                            //port.SetParam(DeviceParam.TubVoltage, DeviceParam.TubCurrent, (int)InitParam.Gain, (int)InitParam.FineGain);
                        }
                    }
                    PostMessage(OwnerHandle, WM_ReceiveData, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
                }
            }
            this.format = new MessageFormat(Info.SpectrumEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            PostMessage(OwnerHandle, WM_EndTest, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
            connect = DeviceConnect.DisConnect;
            this.State = DeviceState.Idel;
            if (InitBoolean)
                PostMessage(OwnerHandle, Wm_ProcessInit, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
        }
        public override void SetOpenCurrent()
        { }
        public override void DoInitial(object obj)
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
            usedTime = 0;
            connect = DeviceConnect.Connect;
            double rate = 0;
            int preGain = 0;
            double ChannelError = 0;
            double fineGainRate = 6;
            int preFineGain = 0;
            double preChannel = 0;

            bool Successed = false;
            //port.SetParam(InitParam.TubVoltage, InitParam.TubCurrent, (int)InitParam.Gain, (int)InitParam.FineGain);
            //while (StopFlag || PauseStop || !port.GetData(Data))
            //{
            //    if (StopFlag)
            //    {
            //        State = DeviceState.Idel;
            //        connect = DeviceConnect.DisConnect;
            //        return;
            //    }
            //    if (PauseStop)
            //        port.CloseVoltage();
            //}
            do
            {
                RayTube.SetXRayTubeParams(DeviceParam.TubCurrent, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
                //port.SetParam(InitParam.TubVoltage, InitParam.TubCurrent, (int)InitParam.Gain, (int)InitParam.FineGain);
                ClearData();
                usedTime = 0;
                connect = DeviceConnect.Connect;
                this.format = new MessageFormat(Info.SpectrumInitialize, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                while ((usedTime < InitTime) && currentState != DeviceState.Idel && !StopFlag)
                {
                    //port.SetParam(InitParam.TubVoltage, InitParam.TubCurrent, (int)InitParam.Gain, (int)InitParam.FineGain);
                    while (StopFlag || PauseStop || !port.GetData(Data))
                    {
                        Thread.Sleep(50);
                        if (PauseStop || StopFlag)
                        {
                            port.CloseVoltage();
                            if (StopFlag)
                                break;
                        }
                    }
                    Beep(0X7FF, 100);
                    usedTime++;
                    for (int i = 0; i < 50; i++)
                        Data[i] = 0;
                    for (int j = (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength; j++)
                        Data[j] = 0;
                    Spec.SpecData = TabControlHelper.ConvertArrayToString(Data);
                    Data.CopyTo(DataCopy, 0);
                    CountRating = CountRate();
                    ReturnVoltage = InitParam.TubVoltage;
                    ReturnCurrent = InitParam.TubCurrent;
                    PostMessage(OwnerHandle, WM_ReceiveData, true, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
                }
                MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Spec.SpecDatas);
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
                    preGain = Convert.ToInt32(ChannelError / rate);
                    InitParam.Gain += preGain;
                }
                if (InitParam.FineGain <= 0 || InitParam.FineGain >= 250 || InitParam.Gain <= 0 || InitParam.Gain >= 250)
                {
                    Successed = false;
                    break;
                }
            } while (!Successed && (State != DeviceState.Idel) && !StopFlag);
            this.format = new MessageFormat(Info.InitailizeEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            PostMessage(OwnerHandle, WM_EndInitial, Successed, (int)Math.Round(usedTime, MidpointRounding.AwayFromZero));
            connect = DeviceConnect.DisConnect;
            State = DeviceState.Idel;
        }

        public override void PreOpenVoltage()
        {
            RayTube.SetXRayTubeParams(DeviceParam.TubCurrent, DeviceParam.TubVoltage, (int)InitParam.Gain, (int)InitParam.FineGain, WorkCurveHelper.DeviceCurrent.HasTarget, (int)DeviceParam.TargetMode);
            //port.SetParam(this.heatParams.TubVoltage, this.heatParams.TubCurrent, (int)this.heatParams.Gain, (int)this.heatParams.FineGain);
            ReturnVoltage = heatParams.TubVoltage;
            ReturnCurrent = heatParams.TubCurrent;
        }
    }
}
