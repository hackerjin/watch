using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;

namespace Skyray.EDX.Common.Component
{
    public class BlueTeethImplement : DeviceInterface
    {
        private int _intTimeTested;
        private Pocket DeviceType;
        private int WORK_TIME_INT = GlobalClass.firstTime;
        private int _intSumWorkTime = 0;
        private int _intSumValue;
        private bool bFitCurrent;
        private bool _bEnterFineGain;
        private System.Windows.Forms.Timer timer;
        //private int _intGainToChannel;
        //private int _intFineGainToChannel;
        //private bool _bSaveSpec;
        private bool _bConnected;
        private SerialDevice _serialDevice;
        private bool _bPortError;
        private bool _getData = false;
       // private bool _bIsGoOnTest;
        private int _intTestState;
        private Thread threadMove;
        //private int MoveState;

        public BlueTeethImplement()
        {
            _serialDevice = new SerialDevice();
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 100;
            timer.Start();
        }
        public override void SetOpenCurrent()
        { }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (_serialDevice != null && _serialDevice.Motor0Ready && _serialDevice.Motor1Ready)
            {
                timer.Enabled = false;
                //this.format = new MessageFormat(Info.MotorStopMove, 0);
                //WorkCurveHelper.specMessage.localMesage.Add(this.format);
                SendMessage(OwnerHandle, WM_MoveStop, 0, 0);
                //if (OnMoveStop != null)
                //    OnMoveStop(0);
            }
        }

        public bool Connect()
        {
            //如果没连接上，连接
            if (!_bConnected)
            {
                //连接设备
                _serialDevice.PortName = "COM" + WorkCurveHelper.DeviceCurrent.ComNum.ToString();
                _serialDevice.BaudRate = WorkCurveHelper.DeviceCurrent.Bits;
                _serialDevice.OnNoPortError = CannotFindPortException;
                _serialDevice.OnCutPowerError = CutDevicePowerException;
                _serialDevice.OnConnectError = DeviceException;
                _bPortError = false;
                _bConnected = true;
                _serialDevice.Open();
                //设置委托
                _serialDevice.OnDataReceived = GetData;
            }
            return _bConnected;
        }

        private void GetData(int[] data)
        {
            tempData = data;
            _getData = true;
        }

        //连接仪器的时候找不到端口
        private void CannotFindPortException()
        {
            _serialDevice.OnTimeChanged = null;
            _serialDevice.OnSwtChanged = null;
            _serialDevice.OnDataReceived = null;
            _serialDevice.OnNoPortError = null;
            _serialDevice.OnCutPowerError = null;
            _serialDevice.OnConnectError = null;
            _serialDevice.Close();
            //出错关掉自动重连一次
            _bConnected = false;
            if (!_bPortError)
            {
                MessageFormat format = new MessageFormat(Info.DeviceNoPort, 0);
                WorkCurveHelper.specMessage.localMesage.Add(format);
                _bPortError = true;//端口错误为真
            }
        }

        //设备状态错误委托
        private void DeviceException()
        {
            if (_intTestState == 2)
            {
                //if (_intTimeTested < DeviceParam.PrecTime)
                //    _bIsGoOnTest = true;
            }
            _serialDevice.OnTimeChanged = null;
            _serialDevice.OnSwtChanged = null;
            _serialDevice.OnDataReceived = null;
            _serialDevice.OnNoPortError = null;
            _serialDevice.OnCutPowerError = null;
            _serialDevice.OnConnectError = null;
            _serialDevice.Close();
            //出错关掉自动重连一次
            _bConnected = false;
            Connect();
        }

        //连接中设备断电
        private void CutDevicePowerException()
        {
            _serialDevice.OnTimeChanged = null;
            _serialDevice.OnSwtChanged = null;
            _serialDevice.OnDataReceived = null;
            _serialDevice.OnNoPortError = null;
            _serialDevice.OnCutPowerError = null;
            _serialDevice.OnConnectError = null;
            _serialDevice.Close();
            //出错关掉自动重连一次
            _bConnected = false;
        }

        public override void DoTest(object obj)
        {
            if (!_bConnected)
            {
                Connect();  //连接仪器
            }
            if (_bConnected)
            {
                _intSumWorkTime = 0;
                //_bSaveSpec = true;
                _intSumValue = 0;
                _intTestState = 2;
                _serialDevice.Voltage = DeviceParam.TubVoltage;  //高压
                _serialDevice.Current = DeviceParam.TubCurrent;  //管流
                _serialDevice.Gain = (int)InitParam.Gain;   //粗调
                _serialDevice.FineGain = (int)InitParam.FineGain;   //细调
                int iCount = 0;
                _intTimeTested = 0;
                bool tempBool = false;
                int nThisTime;
                do
                {
                    //设备复位
                    _serialDevice.ResetDevice();
                    //开启射线
                    _serialDevice.XRayOn();
                    //等待设备进入就绪状态
                    Thread.Sleep(1000);
                    while (_serialDevice.DeviceState != SerialDevice.DEVICE_READY && iCount++ < 50)
                    {
                        Thread.Sleep(500);
                    }
                }
                while (iCount > 50);
                ClearData();
                connect = DeviceConnect.Connect;
                do
                {
                    nThisTime = 1;
                    _serialDevice.Work(nThisTime);
                    //已测时间
                    _intTimeTested = _intSumWorkTime + nThisTime - _serialDevice.LeftTime;
                    if (_serialDevice.LeftTime == 0)
                    {
                        _intSumWorkTime += nThisTime;
                        _serialDevice.ReadData();
                        while (!_getData && !StopFlag)
                        {
                            Thread.Sleep(50);
                        }
                        _getData = false;
                        //接收数据
                        for (int x = 0; x < MaxChann; x++)
                        {
                            Data[x] += tempData[x];
                        }
                        for (int i = 0; i < DeviceParam.BeginChann; i++)
                            Data[i] = 0;
                        for (int j = DeviceParam.EndChann; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength; j++)
                            Data[j] = 0;
                        Spec.SpecData = TabControlHelper.ConvertArrayToString((int[])(Data.Clone()));
                        CountRating = CountRate();
                        ReturnCurrent = DeviceParam.TubCurrent;
                        ReturnVoltage = DeviceParam.TubVoltage;
                        //调节计数率
                        if (DeviceParam.IsAdjustRate)
                        {
                            AdjustCountRate();
                        }
                        if (DeviceParam.PrecTime == _intTimeTested)
                        {
                            tempBool = true;
                        }
                    }
                    PostMessage(OwnerHandle, WM_ReceiveData, true, _intTimeTested);
                    //if (OnReceiveData != null)
                    //    OnReceiveData(_intTimeTested);
                    if ((((_intTimeTested - GlobalClass.firstTime) % WORK_TIME_INT == 0)
                                   && (_intTimeTested < DeviceParam.PrecTime)) || (((DeviceParam.PrecTime - _intTimeTested) <= WORK_TIME_INT) && ((DeviceParam.PrecTime - _intTimeTested) > 0)))
                    {
                        //PostMessage(OwnerHandle, Wm_BlueTeethCaculate, true, _intTimeTested);
                    }
                    if (tempBool)
                        break;
                }
                while (!StopFlag);
                _serialDevice.XRayOff();
            }
            this.format = new MessageFormat(Info.SpectrumEnd, 0);
            WorkCurveHelper.specMessage.localMesage.Add(this.format);
            PostMessage(OwnerHandle, WM_EndTest, true, _intTimeTested);
            //if (OnTestEnd != null)
            //    OnTestEnd(_intTimeTested);
            connect = DeviceConnect.DisConnect;
            _intTestState = -1;
            this.State = DeviceState.Idel;
        }

        public override void PreOpenVoltage()
        {
            _serialDevice.Voltage = heatParams.TubVoltage;  //高压
            _serialDevice.Current = heatParams.TubCurrent;  //管流
            _serialDevice.Gain = (int)heatParams.Gain;   //粗调
            _serialDevice.FineGain = (int)heatParams.FineGain;   //细调
            //设备复位
            _serialDevice.ResetDevice();
            //开启射线
            _serialDevice.XRayOn();
            //等待设备进入就绪状态
        }

        //public override void StopMotor()
        //{
        //    StopFlag = true;
        //    _serialDevice.XRayOff();
        //    _bEnterFineGain = false;
        //   // _intGainToChannel = 5;
        //    //_intFineGainToChannel = 5;
        //}

        public override int AdjustCountRate()
        {
            int iCurrent = 0;
            bool bFitMultiple = true;
            if (CountRating < (int)DeviceParam.MinRate || CountRating > (int)DeviceParam.MaxRate)
            {
                //管流为1或50
                if (_serialDevice.Current == 1 || _serialDevice.Current == 50)
                {
                    //调节不成功
                    bFitCurrent = false;
                    if (!bFitMultiple || !bFitCurrent)
                    {
                        _serialDevice.XRayOff();
                        return 0; 
                    }
                }
                //设置管流
                if (CountRating != 0)
                {
                    iCurrent = (int)((double)((DeviceParam.MinRate + DeviceParam.MaxRate) * _serialDevice.Current) / (double)(2 * CountRating));
                    //在范围内，管流就以设置的为准
                    if (iCurrent >= 1 && iCurrent <= 50) _serialDevice.Current = iCurrent;
                }
                if (iCurrent <= 0 || iCurrent > 50)
                {
                    if (iCurrent <= 0)
                    {
                        _serialDevice.Current = 1;
                    }
                    if (iCurrent > 50)
                    {
                        _serialDevice.Current = 50;
                    }
                }
                //扳机松开了就不必再work了
                if ((_intTimeTested < DeviceParam.PrecTime) ||
                    WORK_TIME_INT == DeviceParam.PrecTime)
                {
                    //没有调好，就重新走时调～～
                    _intSumValue = 0;
                    CountRating = 0;
                    _intTimeTested = 0;
                    _intSumWorkTime = 0;
                    //在调切计数率的时候，如果没有调切到位，则一切归零重新开始。
                    //pfby2009-1028
                    WORK_TIME_INT = GlobalClass.firstTime;
                    //判断是否第一次响应
                    //_testTime2 = 0;
                    //_intCountRateOkAgain = 0;   //计数调好之后重走一次
                    //测量状态
                    _intTestState = 2;

                    //清除原谱
                    //Spec.Clear();
                    //删除谱信息
                    //specGraphic.RemoveSpec(0);
                    //specGraphic.Invalidate();

                    int iCount;
                    do
                    {
                        //设备复位
                        _serialDevice.ResetDevice();
                        //如果全部时间只有10秒，则完毕之后会关闭高压，所以要开高压
                        if (WORK_TIME_INT == DeviceParam.PrecTime)
                        {
                            //开启射线
                            _serialDevice.XRayOn();
                        }
                        //等待设备进入就绪状态
                        Thread.Sleep(1000);
                        iCount = 0;
                        while (_serialDevice.DeviceState != SerialDevice.DEVICE_READY && iCount++ < 50)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    while (iCount > 50);
                    //设备工作
                }
            }
            return 2;
        }

        public override void MotorMove(params int[] index)
        {
            MoveMotorByBlueTeeth();
        }

        public override void InitMotorMove()
        {
            MoveMotorByBlueTeeth();
        }

        private void MoveMotorByBlueTeeth()
        {
            if (State != DeviceState.Idel)
                return;
            timer.Enabled = true;
            //this.format = new MessageFormat(Info.MotorMove, 0);
            //WorkCurveHelper.specMessage.localMesage.Add(format);
            threadMove = new Thread(new ThreadStart(MotorMoveProcess));
            threadMove.Priority = ThreadPriority.Highest;
            threadMove.Start();
        }

        private void MotorMoveProcess()
        {
            Connect();
            //手持3代
            if (WorkCurveHelper.DeviceCurrent.Pocket == Pocket.PocketIII)
            {
                DeviceType = Pocket.PocketIII;
                _serialDevice.MoveMotor(Pocket.PocketIII, DeviceParam.CollimatorIdx, DeviceParam.FilterIdx);
            }
            //便携1代
            else if (WorkCurveHelper.DeviceCurrent.Pocket == Pocket.PortableI)
            {
                DeviceType = Pocket.PortableI;
                int step = 0;
                if (DeviceParam.FilterIdx == 0)
                    step = FilterMotor.Target[3];
                if (DeviceParam.FilterIdx == 1 && DeviceParam.CollimatorIdx == 0)
                    step = FilterMotor.Target[2];
                if (DeviceParam.FilterIdx == 1 && DeviceParam.CollimatorIdx == 1)
                    step = FilterMotor.Target[1];
                if (DeviceParam.FilterIdx == 2)
                    step = FilterMotor.Target[0];
                _serialDevice.MoveMotor(Pocket.PortableI, 0, step);
            }
        }

        public override double CountRate()
        {
            double totalCount = 0;
            foreach (int iv in tempData)
            {
                //计算小段时间内总的读数率
                _intSumValue += iv;
            }
            if (_intTimeTested != 0)
                totalCount = _intSumValue / _intTimeTested;//初始化计数率
            else
                totalCount = 0;
            m_lastCountRate = totalCount;
            if (totalCount < 0)
                totalCount = 0;
            return totalCount;
        }

        public override void DoInitial(object obj)
        {
            if (!_bConnected)
            {
                Connect();
            }
            if (_bConnected)
            {
                _serialDevice.Voltage = InitParam.TubVoltage;  //高压
                _serialDevice.Current = InitParam.TubCurrent;  //管流
                _serialDevice.Gain = (int)InitParam.Gain;   //粗调
                _serialDevice.FineGain = (int)InitParam.FineGain;   //细调

                //若是便携式，移动电机时间较长，下面的操作放到tmrMotor里面做
                if (DeviceType == Pocket.PortableI)
                {
                    return;
                }
                int nThisTime = 1;  //时间5s
                int nGain = 0;
                int _intPreChannel = 0;
                int _intPreGain = 0;
                int nFineGain = 0;
                int _intPreFineGain = 0;
                bool _bInitSucceed = false;
                int _intGainToChannel = 5;
                int _intFineGainToChannel = 5;
                _intTimeTested = 0;
                bool bFitMultiple = true;
                _intSumWorkTime = 0;
                //_bSaveSpec = false;
                _intTestState = 3;
                connect = DeviceConnect.Connect;
                do
                {
                    int iCount = 0;
                    _intSumValue = 0;
                    ClearData();
                    do
                    {
                        //设备复位
                        _serialDevice.ResetDevice();
                        //开启射线
                        _serialDevice.XRayOn();
                        //等待设备进入就绪状态
                        Thread.Sleep(1000);
                        iCount = 0;

                        while (_serialDevice.DeviceState != SerialDevice.DEVICE_READY && iCount++ < 50)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    while (iCount > 50);
                    do
                    {
                        _serialDevice.Work(nThisTime);
                        int preTimeTest = _intTimeTested;
                        _intTimeTested = _intSumWorkTime + nThisTime - _serialDevice.LeftTime;
                        if (_serialDevice.LeftTime == 0)
                        {
                            _intSumWorkTime += nThisTime;
                            _serialDevice.ReadData();//读取数据
                            while (!_getData && !StopFlag)
                            {
                                Thread.Sleep(50);
                            }
                            _getData = false;
                            //接收数据
                            for (int x = 0; x < MaxChann; x++)
                            {
                                Data[x] += tempData[x];
                            }
                            for (int i = 0; i < 50; i++)
                                Data[i] = 0;
                            for (int j = (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50; j < (int)WorkCurveHelper.DeviceCurrent.SpecLength; j++)
                                Data[j] = 0;
                            Spec.SpecData = TabControlHelper.ConvertArrayToString((int[])(Data.Clone()));
                            CountRating = CountRate();
                            ReturnCurrent = DeviceParam.TubCurrent;
                            ReturnVoltage = DeviceParam.TubVoltage;
                            PostMessage(OwnerHandle, WM_ReceiveData, true, _intTimeTested);
                            //if (OnReceiveData != null)
                            //    OnReceiveData(usedTime);
                        }
                    }
                    while (_intTimeTested < 10 && !StopFlag);

                    //if ((_intTimeTested -5)%5 == 0)
                    //{
                    double MaxChannelRealTime = SpecHelper.FitChannOfMaxValue(50, (int)WorkCurveHelper.DeviceCurrent.SpecLength - 50, Spec.SpecDatas);
                        //若峰通道调好
                        if ((MaxChannelRealTime >= InitParam.Channel - InitParam.ChannelError - 0.5) &&
                            (MaxChannelRealTime <= InitParam.Channel + InitParam.ChannelError + 0.5))
                        {
                            //if (_intTimeTested <= 5)
                            //{
                            //    _serialDevice.Work(5);
                            //}
                            //else
                            //{
                                //初始化成功
                                _bInitSucceed = true;
                                _bEnterFineGain = false;
                                _intGainToChannel = 5;
                                _intFineGainToChannel = 5;
                                _serialDevice.XRayOff();
                                _intTestState = 0;
                                break;
                                //_bInitSucceed = false;
                            //}
                        }
                        //调节峰通道
                        else if ((MaxChannelRealTime < InitParam.Channel - InitParam.ChannelError - 0.5) || (MaxChannelRealTime > InitParam.Channel + InitParam.ChannelError + 0.5))
                        {
                            _intSumValue = 0;
                            CountRating = 0;
                            _intTimeTested = 0;
                            _intSumWorkTime = 0;
                            //差距较大时调节粗调：直接赋值不行，因为它的属性的set里面会抛出异常
                            if ((((Convert.ToInt32(MaxChannelRealTime) - InitParam.Channel) / _intGainToChannel) != 0) &&
                                !_bEnterFineGain)
                            {
                                nGain = _serialDevice.Gain + (InitParam.Channel - Convert.ToInt32(MaxChannelRealTime)) / _intGainToChannel;
                                if ((Convert.ToInt32(MaxChannelRealTime) != _intPreChannel) && (_serialDevice.Gain != _intPreGain))
                                    _intGainToChannel = (Convert.ToInt32(MaxChannelRealTime) - _intPreChannel) / (_serialDevice.Gain - _intPreGain);
                                _intPreGain = _serialDevice.Gain;
                                if (nGain >= 0 && nGain <= 255)
                                {

                                    _serialDevice.Gain = nGain;
                                }
                                else
                                {
                                    if (nGain < 0)
                                    {
                                        _serialDevice.Gain = 1;
                                    }
                                    else
                                    {
                                        _serialDevice.Gain = 255;
                                    }
                                }
                            }
                            //差距较小就调节细调
                            else if (((Convert.ToInt32(MaxChannelRealTime) - InitParam.Channel) / _intGainToChannel) == 0 ||
                                _bEnterFineGain)
                            {
                                _bEnterFineGain = true;
                                nFineGain = _serialDevice.FineGain + _intFineGainToChannel * (InitParam.Channel - Convert.ToInt32(MaxChannelRealTime));
                                if ((Convert.ToInt32(MaxChannelRealTime) != _intPreChannel) && (_serialDevice.FineGain != _intPreFineGain))
                                    _intFineGainToChannel = (_serialDevice.FineGain - _intPreFineGain) / (Convert.ToInt32(MaxChannelRealTime) - _intPreChannel);
                                if (Convert.ToInt32(MaxChannelRealTime) == _intPreChannel || _intFineGainToChannel == 0)
                                    _intFineGainToChannel = 2;
                                _intPreFineGain = _serialDevice.FineGain;
                                if (nFineGain >= 0 && nFineGain <= 255)
                                {
                                    _serialDevice.FineGain = nFineGain;
                                }
                                else
                                {
                                    if (nFineGain < 0)
                                    {
                                        _serialDevice.FineGain = 1;
                                    }
                                    else
                                    {
                                        _serialDevice.FineGain = 255;
                                    }
                                }
                            }
                            Array.Clear(tempData, 0, MaxChann);
                            _intPreChannel = Convert.ToInt32(MaxChannelRealTime);
                            //若已经为最大或最小值但还是没调好
                            if (_serialDevice.Gain == 1 || _serialDevice.Gain == 255 || _serialDevice.FineGain == 1 || _serialDevice.FineGain == 255)
                            {
                                bFitMultiple = false;
                            }
                            //若超出调节范围，则先设为最大或最小值
                            if (_serialDevice.Gain <= 0) _serialDevice.Gain = 1;
                            if (_serialDevice.Gain >= 255) _serialDevice.Gain = 255;
                            if (_serialDevice.FineGain <= 0) _serialDevice.FineGain = 1;
                            if (_serialDevice.FineGain >= 255) _serialDevice.FineGain = 255;
                        }
                        this.InitParam.Gain = _serialDevice.Gain;
                        this.InitParam.FineGain = _serialDevice.FineGain;
                        bFitCurrent = true;
                        if (!bFitMultiple || !bFitCurrent)
                        {
                            _serialDevice.XRayOff();
                            return;
                        }
                    //}
                    //PostMessage(OwnerHandle, WM_ReceiveData, true, _intTimeTested);
                }
                while (!_bInitSucceed && !StopFlag);
                this.format = new MessageFormat(Info.InitailizeEnd, 0);
                WorkCurveHelper.specMessage.localMesage.Add(this.format);
                PostMessage(OwnerHandle, WM_EndInitial, _bInitSucceed, _intTimeTested);
                //if (OnInitialEnd != null)
                    //OnInitialEnd(_bInitSucceed);
                this.State = DeviceState.Idel;
                _intTestState = -1;
                connect = DeviceConnect.DisConnect;
            }
        }
    }
}
