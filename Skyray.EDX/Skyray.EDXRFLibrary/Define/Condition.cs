using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class Condition : DbObjectModel<Condition>,ICloneable
    {
        /// <summary>
        /// 所属设备
        /// </summary>
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }
        /// <summary>
        /// 包含曲线
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<WorkCurve> WorkCurves { get; set; }
        /// <summary>
        /// 包含谱
        /// </summary>
        //[HasMany(OrderBy = "Id")]
        //public abstract HasMany<SpecList> SpecLists { get; set; }
        /// <summary>
        /// 包含能量刻度
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<DemarcateEnergy> DemarcateEnergys { get; set; }
        /// <summary>
        /// 包含初始化条件
        /// </summary>
        [HasOne(OrderBy = "Id")]
        public abstract InitParameter InitParam { get; set; }
        /// <summary>
        /// 包含测试条件
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<DeviceParameter> DeviceParamList { get; set; }
        /// <summary>
        /// 测试环境的名称
        /// </summary>
        [Length(ColLength.ConditionName)]
        public abstract string Name { get; set; }

        public abstract ConditionType Type { set; get; }

        public abstract Condition Init(string Name);


        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }


        public byte[] Serialize()
        {
            return BaseObject.Serialize(this);
        }
       
        #endregion
    }
    [Serializable]
    public abstract class InitParameter : DbObjectModel<InitParameter>
    {
        /// <summary>
        /// 所属测量条件
        /// </summary>
        [BelongsTo, DbColumn("Condition_Id")]
        public abstract Condition Condition { get; set; }

        [Auto("管  压")]
        public abstract int TubVoltage { get; set; }//<初始化电压

        [Auto("管  流")]
        public abstract int TubCurrent { get; set; }//初始化电流、

        [Auto("初始化元素"), Length(ColLength.ElementName)]
        public abstract string ElemName { get; set; }//<用来初始化的元素

        [Auto("粗  调  码")]
        public abstract float Gain { get; set; }    //<粗调码

        [Auto("细  调  码")]
        public abstract float FineGain { get; set; } //<细调码

        //实际粗调码
        public abstract float ActGain { get; set; }
        //实际细调码
        public abstract float ActFineGain { get; set; }

        [Auto("初始化通道")]
        public abstract int Channel { get; set; }   //<初始化通道

        [Auto("滤  光  片")]
        public abstract int Filter { get; set; }    //<滤光片

        [Auto("准  直  器")]
        public abstract int Collimator { get; set; }  //准直器

        [Auto("误  差  道")]
        public abstract int ChannelError { get; set; }//<初始化误差道

        [Auto("靶  材")]
        public abstract int Target {get;set;}

        [Auto("靶材模式")]
        public abstract TargetMode TargetMode { get; set; }

        [Auto("管流因子")]
        public abstract int CurrentRate { get; set; }//管流比例因子

        [Auto("是否调节计数")]
        public abstract bool IsAdjustRate { get; set; }
        [Auto("最小计数率")]
        public abstract double MinRate { get; set; }
        [Auto("最大计数率")]
        public abstract double MaxRate { get; set; }
        [Auto("是否参与初始化")]
        public abstract bool IsJoinInit { get; set; }
       // [Auto("放大倍数公式")]
        public abstract string ExpressionFineGain { get; set; }

        [Auto("初始化计数率")]
        public abstract double InitFistCount { get; set; }

       // [Auto("初始化校正值")]
        public abstract double InitCalibrateRatio { get; set; }

        //[Auto("样  品  腔")]
        //public abstract int Chamber { get; set; } //样品腔

        public abstract InitParameter Init(int TubVoltage,
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
            int CurrentRate,
            string ExpressionFineGain,
            double InitCalibrateRatio
            );

        public InitParamsEntity ConvertToNewEntity()
        {
            InitParamsEntity entity = new InitParamsEntity(this.TubVoltage, this.TubCurrent, this.Gain, this.FineGain, this.ActGain, this.ActFineGain, this.Channel, this.ChannelError, this.ElemName, this.Filter, this.Collimator, this.Target, this.TargetMode, this.CurrentRate);
            return entity;
        }
    }
    [Serializable]
    public abstract class DeviceParameter : DbObjectModel<DeviceParameter>
    {
        /// <summary>
        /// 所属测量条件
        /// </summary>
        [BelongsTo, DbColumn("Condition_Id")]
        public abstract Condition Condition { get; set; }
        /// <summary>
        /// 包含子谱信息
        /// </summary>
        //[HasMany(OrderBy = "Id")]
        //public abstract HasMany<Spec> Specs { set; get; }

        [Auto("名称"), Length(ColLength.DeviceParameterName)]
        public abstract string Name { get; set; }

        [Auto("测量时间")]
        public abstract int PrecTime { get; set; }
        [Auto("管流")]
        public abstract int TubCurrent { get; set; }
        [Auto("管压")]
        public abstract int TubVoltage { get; set; }
        public abstract bool IsFaceTubVoltage { get; set; }//是否使用实际管压
        [Auto("实际管压")]
        public abstract int FaceTubVoltage { get; set; }
        [Auto("滤光片")]
        public abstract int FilterIdx { get; set; }
        [Auto("准直器")]
        public abstract int CollimatorIdx { get; set; }
        [Auto("靶材")]
        public abstract int TargetIdx { get; set; }
        [Auto("是否抽真空")]
        public abstract bool IsVacuum { get; set; }
        [Auto("抽真空时间")]
        public abstract int VacuumTime { get; set; }
        [Auto("真空度抽真空")]
        public abstract bool IsVacuumDegree { get; set; }
        [Auto("真空度")]
        public abstract double VacuumDegree { get; set; }
        [Auto("是否调节计数")]
        public abstract bool IsAdjustRate { get; set; }
        [Auto("最小計數率")]
        public abstract double MinRate { get; set; }
        [Auto("最大計數率")]
        public abstract double MaxRate { get; set; }
        [Auto("窗口厚度(mm)")]
        public abstract double Thickness { get; set; }


        public abstract int BeginChann { get; set; }
        public abstract int EndChann { get; set; }

        public abstract bool IsDistrubAlert { get; set; }//是否干扰报警

        public abstract bool IsPeakFloat { get; set; }//是否判断峰飘
        public abstract int PeakFloatLeft { get; set; }//峰飘左界
        public abstract int PeakFloatRight { get; set; }//峰飘右界
        public abstract int PeakFloatChannel { get; set; }//峰道址
        public abstract int PeakFloatError { get; set; }//误差
        public abstract int PeakCheckTime { get; set; }

        public abstract TargetMode TargetMode { get; set; }//靶材模式
        //[DbColumn(
        public abstract int CurrentRate { get; set; }//管流比例因子 

        public abstract DeviceParameter Init(
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
        );

        public DeviceParameterEntity ConvertFrom()
        {
            DeviceParameterEntity entity = new DeviceParameterEntity(this.Name, this.PrecTime, this.TubCurrent, this.TubVoltage, this.FilterIdx, this.CollimatorIdx, this.TargetIdx, this.IsVacuum, this.VacuumTime, this.IsVacuumDegree, this.VacuumDegree, this.IsAdjustRate, this.MinRate, this.MaxRate, this.BeginChann, this.EndChann, this.IsDistrubAlert, this.IsPeakFloat, this.PeakFloatLeft, this.PeakFloatRight, this.PeakFloatChannel, this.PeakFloatError, this.PeakCheckTime, this.TargetMode, this.CurrentRate);
            return entity;
        }
    }
}
