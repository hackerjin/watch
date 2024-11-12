using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;

namespace Skyray.EDXRFLibrary
{
    public abstract class PreHeatParams : DbObjectModel<PreHeatParams>
    {
        [Auto("管  压")]
        public abstract int TubVoltage { get; set; }
        [Auto("管  流")]
        public abstract int TubCurrent { get; set; }
        [Auto("粗调码")]
        public abstract float Gain { get; set; }
        [Auto("准直器")]
        public abstract int CollimatorIdx { get; set; }
        [Auto("滤光片")]
        public abstract int FilterIdx { get; set; }
        [Auto("细调码(DP5)6144-10240")]//(普通)120(DP5)6400
        public abstract float FineGain { get; set; }
        [Auto("靶  材")]
        public abstract int Target { get; set; }
        [Auto("扫谱预热时间(S)")]
        public abstract int FinalHeatTime { get; set; }
        [Auto("开始预热时间(S)")]
        public abstract int PreHeatTime { get; set; }
        //[Auto("样品腔")]
        //public abstract int Chamber { get; set; }
        [Auto("靶材模式")]
        public abstract TargetMode TargetMode { get; set; }//靶材模式
        [Auto("管流因子")]
        public abstract int CurrentRate { get; set; }//管流比例因子 
        public abstract PreHeatParams Init(int TubVoltage,
                                            int TubCurrent,
                                            float Gain,
                                            float FineGain,
                                            int FilterIdx,
                                            int CollimatorIdx, 
                                            int PreHeatTime, 
                                            int FinalHeatTime, 
                                            int Target, 
                                            TargetMode TargetMode,
                                            int CurrentRate);//, int Chamber);
    }
}
