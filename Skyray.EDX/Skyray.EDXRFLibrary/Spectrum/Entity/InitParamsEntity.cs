using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDXRFLibrary.Spectrum
{
    [Serializable]
    public class InitParamsEntity
    {
        
        public  int TubVoltage { get; set; }//<初始化电压
        public  int TubCurrent { get; set; }//初始化电流、
        public  string ElemName { get; set; }//<用来初始化的元素
        public  float Gain { get; set; }    //<粗调码
        public  float FineGain { get; set; } //<细调码
        public  float ActGain { get; set; }
        public  float ActFineGain { get; set; }
        public  int Channel { get; set; }   //<初始化通道
        public  int Filter { get; set; }    //<滤光片
        public  int Collimator { get; set; }  //准直器
        public  int ChannelError { get; set; }//<初始化误差道
        public  int Target { get; set; }
        public  TargetMode TargetMode { get; set; }
        public  int CurrentRate { get; set; }//管流比例因子
        public  bool IsAdjustRate { get; set; }
        public  double MinRate { get; set; }
        public  double MaxRate { get; set; }
        public bool IsJoinInit { get; set; }
        public InitParamsEntity()
        { }

        public InitParamsEntity(int TubVoltage,
            int TubCurrent,
            float Gain,
            float FineGain,
            float ActGain,
            float ActFineGain,
            int Channel,
            int ChannelError,
            string ElemName,
            int Filter,
            int Collimator,
            int Target,
            TargetMode TargetMode,
            int CurrentRate
            )
        {
            this.TubVoltage = TubVoltage;
            this.TubCurrent = TubCurrent;
            this.Gain = Gain;
            this.FineGain = FineGain;
            this.ActGain = ActGain;
            this.ActFineGain = ActFineGain;
            this.Channel = Channel;
            this.ChannelError = ChannelError;
            this.ElemName = ElemName;
            this.Filter = Filter;
            this.Collimator = Collimator;
            this.Target = Target;
            this.TargetMode = TargetMode;
            this.CurrentRate = CurrentRate;
        }
    }
}
