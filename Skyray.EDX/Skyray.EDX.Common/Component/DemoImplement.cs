using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;
using System.Configuration;

namespace Skyray.EDX.Common.Component
{
    public class DemoImplement:DeviceInterface
    {

        private void BeijingTime()
        {
            while (true)
            {
                if (State == DeviceState.Test
                    || State == DeviceState.Init)
                {
                    usedTime++;
                    Thread.Sleep(1000);
                }
            }
        }

        public override void DoTest(object obj)
        {
            //if(WorkCurveHelper.MeasureTimeType== Skyray.EDX.Common.UIHelper.MeasureTimeType.TimeMeasure)
            //TimerStart();
            if (DeviceParam == null || WorkCurveHelper.DemoSpecList == null || WorkCurveHelper.DemoSpecList.Specs.Length == 0)
            {
                PostMessage(OwnerHandle, Wm_NoDemoData, true, (int)usedTime);
                StopFlag = true;
                connect = DeviceConnect.DisConnect;
                this.State = DeviceState.Idel;
                return;
            }
            if (Pump.Exist && DeviceParam.IsVacuum)
            {
                Pump.Open();
                this.format = new MessageFormat(Info.TimePumpOpen, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                this.State = DeviceState.Idel;
                PumpStart();
            }
            MessageFormat format = new MessageFormat(Info.SpectrumTest, 0);
            WorkCurveHelper.specMessage.localMesage.Add(format);
            //Skyray.EDXRFLibrary.Spectrum.SpecEntity tempSpec = WorkCurveHelper.DataAccess.Query(WorkCurveHelper.DemoSpecList.Name).Specs[0];
            Skyray.EDXRFLibrary.Spectrum.SpecEntity tempSpec = WorkCurveHelper.DemoSpecList.Specs[0];
            int[] originalData = Helper.ToInts(tempSpec.SpecData);
            bool DropData = false;
            usedTime = 0;
            var result = from temp in originalData.ToList() select (int)(temp);
            //var result = from temp in originalData.ToList() select (int)(temp * DeviceParam.PrecTime / tempSpec.UsedTime);
            State = DeviceState.Test;

            Thread t = new Thread(new ThreadStart(BeijingTime)); ;
            if (WorkCurveHelper.IsAutoIncrease) { if (!t.IsAlive) t.Start(); }
            int adjustCountAdd = 1;
            bool IsAdjust = true;
            while (usedTime < DeviceParam.PrecTime && !StopFlag)
            {
                Thread.Sleep(1000);
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
                    State = DeviceState.Test;
                    format = new MessageFormat(Info.SpectrumTest, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                if (State == DeviceState.Break)
                    break;
                if (WorkCurveHelper.MeasureTimeType != Skyray.EDX.Common.UIHelper.MeasureTimeType.Normal) Thread.Sleep(new Random().Next(0,3000));
                if ((!DropData) && (usedTime >= DropTime))
                {
                    DropData = true;
                    ClearData();
                    usedTime = 0;
                }
                var resultRatio = from temp in result select (int)(temp * usedTime / DeviceParam.PrecTime);
                //Data = resultRatio.ToArray();//cyq 更改模拟的数据太长超过Data的长度。2013-03-25
                Array.Copy(resultRatio.ToArray(), Data, Data.Length);
                //if(WorkCurveHelper.MeasureTimeType!= Skyray.EDX.Common.UIHelper.MeasureTimeType.TimeMeasure) 
                //    usedTime++;

                if (WorkCurveHelper.MeasureTimeType != Skyray.EDX.Common.UIHelper.MeasureTimeType.TimeMeasure)
                    if (!WorkCurveHelper.IsAutoIncrease) usedTime++;


                Array.Clear(Data, 0, DeviceParam.BeginChann);
                Array.Clear(Data, DeviceParam.EndChann, (int)WorkCurveHelper.DeviceCurrent.SpecLength - DeviceParam.EndChann);
                Data.CopyTo(DataCopy, 0);
               
                #region 调节计数率
                if (usedTime / 5 >= adjustCountAdd)
                    IsAdjust = false;
                if ((DeviceParam.IsAdjustRate) && usedTime > 1 && !IsAdjust)
                {
                    int adjustCount = AdjustCountRate();
                    if (adjustCount == 0)
                    {
                        
                        Msg.Show(string.Format(Info.strAdjustCountFail, WorkCurveHelper.DeviceCurrent.MaxCurrent), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        StopFlag = true;
                        connect = DeviceConnect.DisConnect;
                        break;
                    }
                    else
                        if (adjustCount == 1)
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

                #region 自动校正峰飘
                if (WorkCurveHelper.IPeakCalibration && WorkCurveHelper.IDemarcateTest==0)
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
                Data.CopyTo(DataCopy, 0);
                ReturnVoltage = DeviceParam.TubVoltage;
                ReturnCurrent = DeviceParam.TubCurrent;
                Spec.TubCurrent = ReturnCurrent > 0 ? ReturnCurrent : DeviceParam.TubCurrent;
                Spec.TubVoltage = ReturnVoltage > 0 ? ReturnVoltage : DeviceParam.TubVoltage;
                CountRating = CountRate();
                if(WorkCurveHelper.MeasureTimeType!= Skyray.EDX.Common.UIHelper.MeasureTimeType.TimeMeasure)
                PostMessage(OwnerHandle, WM_ReceiveData, true, (int)usedTime);
            }

            if (WorkCurveHelper.IsAutoIncrease) t.Abort();


            this.format = new MessageFormat(Info.SpectrumEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            //if (WorkCurveHelper.MeasureTimeType == Skyray.EDX.Common.UIHelper.MeasureTimeType.AutoIncrease)
            //    usedTime = WorkCurveHelper.ActureMeasureTime.ConvertToType(usedTime);
            PostMessage(OwnerHandle, WM_EndTest, true, (int)usedTime);
            connect = DeviceConnect.DisConnect;
            this.State = DeviceState.Idel;
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
        public const double InitError = 0.5;//内部0.5的初始化误差道
        public override void DoInitial(object obj)
        {
            usedTime = 0;
            connect = DeviceConnect.Connect;
            double ChannelError = 0;
            bool Successed = false;
           if (WorkCurveHelper.DemoSpecList == null || WorkCurveHelper.DemoSpecList.Specs.Length == 0)
            {
                PostMessage(OwnerHandle, Wm_NoDemoData, true, (int)usedTime);
                StopFlag = true;
                connect = DeviceConnect.DisConnect;
                this.State = DeviceState.Idel;
                return;
            }

           Thread t = new Thread(new ThreadStart(BeijingTime));

           Skyray.EDXRFLibrary.Spectrum.SpecEntity tempSpec =WorkCurveHelper.DataAccess.Query(WorkCurveHelper.DemoSpecList.Name).Specs[0];
           int[] originalData = Helper.ToInts(tempSpec.SpecData);
           var result = from temp in originalData.ToList() select (int)(temp * InitTime / tempSpec.UsedTime);
            do
            {
                ClearData();
                usedTime = 0;
                connect = DeviceConnect.Connect;
                this.format = new MessageFormat(Info.SpectrumInitialize, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);



                if (WorkCurveHelper.IsAutoIncrease) { if (!t.IsAlive) t.Start(); }

                while ((usedTime < InitTime)  && !StopFlag)
                {
                    Thread.Sleep(1000);
                    var resultRatio = from temp in originalData select (int)(temp * usedTime / InitTime);
                    //Data = resultRatio.ToArray();//cyq 更改模拟的数据太长超过Data的长度。2013-03-25
                    Array.Copy(resultRatio.ToArray(), Data, Data.Length);
                    //usedTime++;
                    if (!WorkCurveHelper.IsAutoIncrease) usedTime++;
                    Array.Clear(Data, 0, 50);
                    Array.Clear(Data, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, 50);
                    Spec.SpecData = TabControlHelper.ConvertArrayToString(Data);
                    //Spec.SpecOrignialData = Spec.SpecData;
                    Data.CopyTo(DataCopy, 0);
                    CountRating = CountRate();
                    ReturnVoltage = DeviceParam.TubVoltage;
                    ReturnCurrent = DeviceParam.TubCurrent;
                    if (!WorkCurveHelper.DeviceCurrent.IsAllowOpenCover && ReturnCoverClosed)
                    {
                        Msg.Show(Info.CoverOpen);
                        StopFlag = true;
                        connect = DeviceConnect.DisConnect;
                        break;
                    }
                    PostMessage(OwnerHandle, WM_ReceiveData, true, (int)usedTime);
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
                int tempMove = Convert.ToInt32(ChannelError);
                int[] tempDataMove = new int[originalData.Length];
                for (int i = 0; i < originalData.Length - Math.Abs(tempMove); i++)
                {
                    if (tempMove < 0)
                        tempDataMove[i] = originalData[i + Math.Abs(tempMove)];
                    else
                        tempDataMove[i + Math.Abs(tempMove)] = originalData[i];
                }
                originalData = tempDataMove;
            } while (!Successed && (State != DeviceState.Idel) && !StopFlag);
            this.format = new MessageFormat(Info.InitailizeEnd, 0);

            if (WorkCurveHelper.IsAutoIncrease) t.Abort();
            
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            PostMessage(OwnerHandle, WM_EndInitial, Successed, (int)usedTime);
            RayTube.Close();
            connect = DeviceConnect.DisConnect;
            State = DeviceState.Idel;
        }

        public override void SetOpenCurrent()
        { }
        public override void PreOpenVoltage()
        {
            
        }
        public override string GetDppVersion()
        {
            return string.Empty;
        }
        public override void SetDp5Cfg()
        {
        }
        public override double CountRate()
        {
            double totalCount = 0;
            for (int i = 0; i < Data.Length; i++)
            {
                totalCount += DataCopy[i];
            }
            //TestTotalCount = (int)Math.Round(totalCount, MidpointRounding.AwayFromZero);
            TestTotalCount = totalCount;
            if (usedTime != 0)
            {
                if (currentRateMode == CountingRateModes.Average)
                    totalCount /= usedTime;
                else
                    totalCount = totalCount - m_lastCountRate;
                totalCount = Convert.ToDouble(totalCount.ToString("f2"));
            }
            else
            {
                totalCount = 0;
            }
            m_lastCountRate = totalCount;
            if (totalCount < 0)
                totalCount = 0;
            return totalCount;
        }
    }
}
