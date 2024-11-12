using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using Skyray.EDXRFLibrary;
using System;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 光管类
    /// </summary>
    public class XRayTube
    {
        private int MaxVollage = 50;///<管压最大值
        private int MinVollage = 4;///<管压最小值
        private int MaxCurrent = 600;///<管流最大值
        private int MinCurrent = 0;///<管流最小值
        private int MaxGain = 255;///<放大倍数最大值
        private int MinGain = 0;///<放大倍数最小值
        private int MaxFineGain = 255;///<放大倍数最大值
        private int MinFineGain = 0;///<放大倍数最小值
        //private int DueTime = 8;  ///<8秒钟用来设置一次高压
       // private Timer timer;   ///<定时器，用来维持高压
        //private NetControl.NetControl port;     ///<通讯接口   
        private Port port = null;
           
        public int TargetAtomicNumber;///<靶材原子系数
        public double TargetTakeOffAngle;///<靶材角度

        private int minTargetTakeOffAngle = 0;///<铍窗厚度最小值
        private int maxTargetTakeOffAngle = 360;///<铍窗厚度最大值

        public int MinTargetTakeOffAngle
        {
            get { return minTargetTakeOffAngle; }
            set { minTargetTakeOffAngle = value; }
        }

        public int MaxTargetTakeOffAngle
        {
            get { return maxTargetTakeOffAngle; }
            set { maxTargetTakeOffAngle = value; }
        }

        public double WindowThickness;///<铍窗厚度(mm)

        private int minWindowThickness = 0;///<铍窗厚度最小值

        public int MinWindowThickness
        {
            get { return minWindowThickness; }
            set { minWindowThickness = value; }
        }

        public string WindowFormula;  ///<窗口物质表达式
        public double IncidentAngle;///<入射角

        private int minIncidentAngle = 0;///<入射角最小值
        private int maxIncidentAngle = 360;///<入射角最大值

        public int MinIncidentAngle
        {
            get { return minIncidentAngle; }
            set { minIncidentAngle = value; }
        }

        public int MaxIncidentAngle
        {
            get { return maxIncidentAngle; }
            set { maxIncidentAngle = value; }
        }

        public double EmergentAngle;///<出射角
        private int minEmergentAngle = 0;///<出射角最小值
        private int maxEmergentAngle = 360;///<出射角最大值      

        public int MinEmergentAngle
        {
            get { return minEmergentAngle; }
            set { minEmergentAngle = value; }
        }

        public int MaxEmergentAngle
        {
            get { return maxEmergentAngle; }
            set { maxEmergentAngle = value; }
        }
        private int current;

        public int Current
        {
            get
            {
                return current;
            }
        }

        /// <summary>
        /// 管流比例因子 默认值为1
        /// </summary>
        public double dblCurrentScale = 1;


        public double dblVoltageScale;

        private int voltage ;


        public int Voltage
        {
            get
            {
                return voltage;
            }
        }

      
        private int gain; ///<粗调码

        public int Gain
        {
            get
            {
                return gain;
            }
        }

        /// <summary>
        ///细调码
        /// </summary>
        /// <remarks>细调值</remarks>
        private int fineGain;

        public int FineGain
        {
            get
            {
                return fineGain;
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public XRayTube(Port port)
        {
            this.port = port;
            TargetAtomicNumber = 0;
            TargetTakeOffAngle = 0;
            WindowThickness = 0;
            WindowFormula = "";
            IncidentAngle = 0;
            EmergentAngle = 0;
            dblCurrentScale = 1;
            EqualParamsRange();
        }

        public XRayTube(Device device,Port port)
        {
            this.port = port;
            TargetAtomicNumber = device.Tubes.AtomNum;
            TargetTakeOffAngle = device.Tubes.Angel;
            WindowThickness = device.Tubes.Thickness;
            WindowFormula = device.Tubes.Material;
            IncidentAngle = device.Tubes.Incident;
            EmergentAngle = device.Tubes.Exit;
            dblVoltageScale = 1/device.VoltageScaleFactor;
            dblCurrentScale = 1/device.CurrentScaleFactor;
            EqualParamsRange();
        }

        public void SetXRayTubeParams(int current, int voltage, int gain, int fineGain, bool hasTarget,int targetMode)
        {
            if (current < MinCurrent)
            {
                current = MinCurrent;
            }
            else if (current > MaxCurrent)
            {
                current = MaxCurrent;
            }

            this.current = current;
            if (voltage > MaxVollage)
            {
                voltage = MaxVollage;
            }
            else if (voltage < MinVollage)
            {
                voltage = MinVollage;
            }
            if (hasTarget && targetMode == 1 && voltage < 10)   //二次靶时最小管压为10
            {
                voltage = 10;
            }
            this.voltage = voltage;

            if (gain > MaxGain)
            {
                gain = MaxGain;
            }
            else if (gain < MinGain)
            {
                gain = MinGain;
            }
            this.gain = gain;

            if (fineGain > MaxFineGain)
            {
                fineGain = MaxFineGain;
            }
            else if (fineGain < MinFineGain)
            {
                fineGain = MinFineGain;
            }
            this.fineGain = fineGain;
            port.SetParam((int)Math.Round(voltage * dblVoltageScale, 0, MidpointRounding.AwayFromZero),
                   (int)Math.Round(current * dblCurrentScale, 0, MidpointRounding.AwayFromZero), gain, fineGain);
        }

        public void SetXRayTubeParams(int current, int voltage ,bool HasTarget ,int targetMode)
        {
            
            if (current < MinCurrent)
            {
                current = MinCurrent;
            }
            else if (current > MaxCurrent)
            {
                current = MaxCurrent;
            }

            this.current = current;
            if (voltage > MaxVollage)
            {
                voltage = MaxVollage;
            }
            else if (voltage < MinVollage)
            {
                voltage = MinVollage;
            }
            if (HasTarget && targetMode == 1 && voltage < 10)   //二次靶时最小管压为10
            {
                voltage = 10;
            }
            this.voltage = voltage;

            port.setParam(voltage * dblVoltageScale, current * dblCurrentScale);
        }

        private void EqualParamsRange()
        {
            if (Ranges.RangeDictionary.ContainsKey("TubCurrent"))
            {
                RangeInfo rangeInfo = new RangeInfo();
                Ranges.RangeDictionary.TryGetValue("TubCurrent", out rangeInfo);
                MinCurrent = (int)rangeInfo.Min;
                //MaxCurrent = (int)rangeInfo.Max;
            }
            MaxCurrent = WorkCurveHelper.DeviceCurrent.MaxCurrent;
            if (Ranges.RangeDictionary.ContainsKey("TubVoltage"))
            {
                RangeInfo rangeInfo = new RangeInfo();
                Ranges.RangeDictionary.TryGetValue("TubVoltage", out rangeInfo);
                MinVollage = (int)rangeInfo.Min;
                //MaxVollage = (int)rangeInfo.Max;
            }
            MaxVollage = WorkCurveHelper.DeviceCurrent.MaxVoltage;
            if (Ranges.RangeDictionary.ContainsKey("Gain"))
            {
                RangeInfo rangeInfo = new RangeInfo();
                Ranges.RangeDictionary.TryGetValue("Gain", out rangeInfo);
                MinGain = (int)rangeInfo.Min;
                MaxGain = (int)rangeInfo.Max;
            }

            if (Ranges.RangeDictionary.ContainsKey("FineGain"))
            {
                RangeInfo rangeInfo = new RangeInfo();
                Ranges.RangeDictionary.TryGetValue("FineGain", out rangeInfo);
                MinFineGain = (int)rangeInfo.Min;
                MaxFineGain = (int)rangeInfo.Max;
            }

        }

        /// <summary>
        /// 打开高压
        /// </summary>
        public void Open()
        {
            if (WorkCurveHelper.deviceMeasure.interfacce.IndiaLazer != null && !WorkCurveHelper.deviceMeasure.interfacce.IndiaLazer.IsManual
                && WorkCurveHelper.deviceMeasure.interfacce.IndiaLazer.IsManual)
            {
                WorkCurveHelper.deviceMeasure.interfacce.IndiaLazer.Close();
            }
            if (WorkCurveHelper.FunctionEnabled("LaserMode")
                &&WorkCurveHelper.deviceMeasure.interfacce.ShellCover != null)
            {
                if (!WorkCurveHelper.deviceMeasure.interfacce.ShellCover.OpenAsync())
                {
                    Msg.Show("防护罩没有移动到位");
                    return;
                }
            }
            //if (WorkCurveHelper.IsUseElect)
            //{
            //    WorkCurveHelper.deviceMeasure.interfacce.Pump.TOpen();//电磁阀置1
            //}
            Thread.Sleep(100);
            port.OpenVoltage();
            Thread.Sleep(100);
            port.OpenVoltageLamp();
        }

        /// <summary>
        /// 关闭高压
        /// </summary>
        public void Close()
        {
            Thread.Sleep(100);
            port.CloseVoltage();
            Thread.Sleep(100);
            port.CloseVoltageLamp();
            WorkCurveHelper.deviceMeasure.interfacce.IsDropTime = true; 
            if (WorkCurveHelper.deviceMeasure.interfacce.IndiaLazer != null && !WorkCurveHelper.deviceMeasure.interfacce.IndiaLazer.IsManual
                && WorkCurveHelper.deviceMeasure.interfacce.IndiaLazer.IsManual)
            {
                WorkCurveHelper.deviceMeasure.interfacce.IndiaLazer.Open();
            }
            if (WorkCurveHelper.FunctionEnabled("LaserMode")
                &&WorkCurveHelper.deviceMeasure.interfacce.ShellCover != null 
                &&WorkCurveHelper.deviceMeasure.interfacce.port is Skyray.EDX.Common.Component.NetPort
                &&WorkCurveHelper.deviceMeasure.interfacce.port.ConnectState)
            {
                if (!WorkCurveHelper.deviceMeasure.interfacce.ShellCover.Close())
                {
                    Msg.Show("防护罩没有移动到位");
                    return;
                }
            }
           
           
        }

        public void EnableCoverSwitch(bool allowUncover)
        {
            port.AllowUncover(allowUncover);
        }

        /// <summary>
        /// 保持高压，让高压一直处于开启状态
        /// </summary>
        /// <param name="enabled"></param>
        //public void Remain(bool enabled)
        //{
        //    if (enabled && (timer == null))
        //    {
        //        timer = new Timer(OnTime, null, DueTime * 1000, 0);
        //    }
        //    else if (!enabled && (timer != null))
        //    {
        //        timer.Dispose();
        //    }
        //}

        /// <summary>
        /// 时间Tick函数
        /// </summary>
        /// <param name="state">状态</param>
        //private void OnTime(object state)
        //{
        //     = voltage;
        //}
    }
}
