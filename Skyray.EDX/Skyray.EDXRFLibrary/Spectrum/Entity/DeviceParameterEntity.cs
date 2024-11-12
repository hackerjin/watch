using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDXRFLibrary.Spectrum
{
    [Serializable]
    public class DeviceParameterEntity
    {
        public  string Name { get; set; }
        public  int PrecTime { get; set; }
        public  int TubCurrent { get; set; }
        public  int TubVoltage { get; set; }
        public  bool IsFaceTubVoltage { get; set; }//是否使用实际管压
        public  int FaceTubVoltage { get; set; }
        public  int FilterIdx { get; set; }
        public  int CollimatorIdx { get; set; }
        public  int TargetIdx { get; set; }
        public  bool IsVacuum { get; set; }
        public  int VacuumTime { get; set; }
        public  bool IsVacuumDegree { get; set; }
        public  double VacuumDegree { get; set; }
        public  bool IsAdjustRate { get; set; }
        public  double MinRate { get; set; }
        public  double MaxRate { get; set; }
        public  int BeginChann { get; set; }
        public  int EndChann { get; set; }
        public string ParentName { get; set; }
        public  bool IsDistrubAlert { get; set; }//是否干扰报警

        public  bool IsPeakFloat { get; set; }//是否判断峰飘
        public  int PeakFloatLeft { get; set; }//峰飘左界
        public  int PeakFloatRight { get; set; }//峰飘右界
        public  int PeakFloatChannel { get; set; }//峰道址
        public  int PeakFloatError { get; set; }//误差
        public  int PeakCheckTime { get; set; }

        public TargetMode TargetMode { get; set; }//靶材模式
        public int CurrentRate { get; set; }//管流比例因子 

        public DeviceParameterEntity() { }

        public DeviceParameterEntity(
           string Name,
           int PrecTime,
           int TubCurrent,
           int TubVoltage,
           int FilterIdx,
           int CollimatorIdx,
           int TargetIdx,
           bool IsVacuum,
           int VacuumTime,
           bool IsVacuumDegree,
           double VacuumDegree,
           bool IsAdjustRate,
           double MinRate,
           double MaxRate,
           int BeginChann,
           int EndChann,
           bool IsDistrubAlert,
           bool IsPeakFloat,
           int PeakFloatLeft,
           int PeakFloatRight,
           int PeakFloatChannel,
           int PeakFloatError,
           int PeakCheckTime,
           TargetMode TargetMode,
           int CurrentRate
       ){
           this.Name = Name;
           this.PrecTime = PrecTime;
           this.TubCurrent = TubCurrent;
           this.TubVoltage = TubVoltage;
           this.FilterIdx = FilterIdx;
           this.CollimatorIdx = CollimatorIdx;
           this.TargetIdx = TargetIdx;
           this.IsVacuum = IsVacuum;
           this.IsVacuumDegree = IsVacuumDegree;
           this.VacuumDegree = VacuumDegree;
           this.IsAdjustRate = IsAdjustRate;
           this.MinRate = MinRate;
           this.MaxRate = MaxRate;
           this.BeginChann = BeginChann;
           this.EndChann = EndChann;
           this.IsDistrubAlert = IsDistrubAlert;
           this.IsPeakFloat = IsPeakFloat;
           this.PeakFloatLeft = PeakFloatLeft;
           this.PeakFloatRight = PeakFloatRight;
           this.PeakFloatChannel = PeakFloatChannel;
           this.PeakFloatError = PeakFloatError;
           this.PeakCheckTime = PeakCheckTime;
           this.TargetMode = TargetMode;
           this.CurrentRate = CurrentRate;
        }
    }
}
