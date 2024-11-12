using Lephone.Data.Definition;
using System;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 探测器结构
    /// </summary>
    [Serializable]
    [Auto("探测器")]
    public abstract class Detector : DbObjectModel<Detector>
    {
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }

        /// <summary>
        /// 探测器类型，Si半导体，Xe计数管
        /// </summary>
        [Auto("探测器类型")]
        public abstract DetectorType Type { get; set; }

        /// <summary>
        /// 峰的能量位置（eV）
        /// </summary>
        [Auto("能量")]
        public abstract double Energy { get; set; }

        /// <summary>
        /// 峰的半高宽，即能量分辨率
        /// </summary>
        [Auto("分辨率")]
        public abstract double Fwhm { get; set; }

        /// <summary>
        /// rohs用的拟合系数
        /// </summary>
        public double FixGaussDelta;//rohs用的拟合系数 （HalfWidth/PeakChannel(ag通道)） cyq

        public abstract Detector Init(DetectorType Type, double Energy, double Fwhm);

    }

    /// <summary>
    /// 探测器类型
    /// </summary>
    [Serializable]
    public enum DetectorType
    {
        Si,//Si探测器
        Xe,//Xe探测器
        //Dp5
    }



    /// <summary>
    /// 光管
    /// </summary>
    [Serializable]
    [Auto("光管")]
    public abstract class Tubes : DbObjectModel<Tubes>
    {
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }

        [Auto("靶材原子序号")]
        public abstract int AtomNum { get; set; }
        [Auto("靶材角度")]
        public abstract int Angel { get; set; }
        [Auto("窗口厚度(mm)")]
        public abstract double Thickness { get; set; }
        [Auto("窗口材料"), Length(ColLength.Material)]
        public abstract string Material { get; set; }
        [Auto("入射角(度)")]
        public abstract int Incident { get; set; }
        [Auto("出射角(度)")]
        public abstract int Exit { get; set; }
        [Auto("窗口原子序号")]
        public abstract int MaterialAtomNum { get; set; }

        //[Auto("管压")]
        public abstract double TubVoltage { get; set; }
        //[Auto("因子")]
        public abstract double PercentVoltage { get; set; }

        public abstract Tubes Init(int AtomNum, int Angel, double Thickness, string Material, int Incident, int Exit, int MaterialAtomNum);
    }
    /// <summary>
    /// 准直器电机
    /// </summary>
    [Serializable]
    public abstract class Collimator : DbObjectModel<Collimator>
    {
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }

        public abstract int Num { get; set; }//序号/位置

        public abstract double Diameter { get; set; }//直径

        public abstract int Step { get; set; }//步长

        public abstract Collimator Init(int Num, double Diameter, int Step);
    }
    /// <summary>
    /// 滤光片电机
    /// </summary>
    [Serializable]
    public abstract class Filter : DbObjectModel<Filter>
    {
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }

        public abstract int Num { get; set; }

        public abstract int Step { get; set; }

        [AllowNull]
        public abstract string Caption { get; set; }

        public abstract int AtomNum { get; set; }

        public abstract double FilterThickness { get; set; }

        public abstract Filter Init(int Num, int Step, string Caption, double FilterThickness);
    }
    /// <summary>
    /// 样品腔
    /// </summary>
    [Serializable]
    public abstract class Chamber : DbObjectModel<Chamber>
    {
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }

        public abstract int Num { get; set; }

        public abstract int Step { get; set; }

        public abstract int StepCoef1 { get; set; }//针对6000B添加偏移2014-10-14
        public abstract int StepCoef2 { get; set; }//针对6000B添加偏移
        public abstract Chamber Init(int Num, int Step, int StepCoef1, int StepCoef2);
    }
    /// <summary>
    /// 靶材
    /// </summary>
    [Serializable]
    public abstract class Target : DbObjectModel<Target>
    {
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }

        public abstract int Num { get; set; }

        public abstract int Step { get; set; }
        [AllowNull]
        public abstract string Caption { get; set; }

        public abstract int AtomNum { get; set; }

        public abstract double TargetThickness { get; set; }

        public abstract Target Init(int Num, int Step, string Caption, double TargetThickness);
    }
    [Serializable]
    public abstract class FPGAParams : DbObjectModel<FPGAParams>
    {
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }
        [Auto("基线恢复运行标志")]
        public abstract OFFON BaseResume { get; set; }//<基线恢复运行标志
        [Auto("基线恢复门限使用标志")]
        public abstract OFFON BaseLimit { get; set; }//<基线恢复门限使用标志
        [Auto("堆积叛弃功能运行标志")]
        public abstract OFFON HeapUP { get; set; }//<堆积叛弃功能运行标志
        [Auto("运行时钟")]
        public abstract int Rate { get; set; }//<运行时钟
        [Auto("梯形上升时间寄存器")]
        public abstract double PeakingTime { get; set; }//<梯形上升时间寄存器  

        public int SendPeakTime//下发的PeakingTime
        {
            get
            {
                //float temp = 0.0f;
                //temp = (float)PeakingTime;
                //if (PeakingTime <= 6.4)
                //{
                //    temp = temp / 0.8f - 1;
                //}
                //else if (PeakingTime <= 12.8)
                //{
                //    temp = temp / 1.6f - 1;
                //}
                //else if (PeakingTime <= 25.6)
                //{
                //    temp = temp / 3.2f - 1;
                //}
                //else if (PeakingTime <= 51.2)
                //{
                //    temp = temp / 6.4f - 1;
                //}
                //else
                //{
                //    temp = temp / 12.8f - 1;
                //}
                //return temp;
                float temp = 0.0f;
                temp = (float)PeakingTime;
                if (PeakingTime <= 6.4)
                {
                    temp = temp / 0.8f - 1;
                }
                else if (PeakingTime <= 12.8)
                {
                    temp = temp / 1.6f + 3;
                }
                else if (PeakingTime <= 25.6)
                {
                    temp = temp / 3.2f + 7;
                }
                else if (PeakingTime <= 51.2)
                {
                    temp = temp / 6.4f + 11;
                }
                else
                {
                    temp = temp / 12.8f + 15;
                }
                return (int)Math.Round(temp, MidpointRounding.AwayFromZero);
            }
        }
        [Auto("梯形顶宽时间寄存器")]
        public abstract double FlatTop { get; set; }//<梯形顶宽时间寄存器  赋值的时候要确保PeakingTime 已被赋值

        public int SendFlatTop//下发的FlatTop
        {
            get
            {
                double temp = 0.0;
                temp = FlatTop;
                if (PeakingTime <= 6.4)
                {
                    temp = temp / 0.2 - 1;
                }
                else if (PeakingTime <= 12.8)
                {
                    temp = temp / 0.4 - 1;
                }
                else if (PeakingTime <= 25.6)
                {
                    temp = temp / 0.8 - 1;
                }
                else if (PeakingTime <= 51.2)
                {
                    temp = temp / 1.6 - 1;
                }
                else
                {
                    temp = temp / 3.2 - 1;
                }
                temp = Math.Round(temp, MidpointRounding.AwayFromZero);
                return (int)temp;
            }
        }
        [Auto("慢成形门限值")]
        public abstract int SlowLimit { get; set; }//<慢成形门限值
        //public abstract bool AllowHighVoltage;  ///<是否允许开盖启动高压
        
        public abstract double ShowPeakingTime { get; set; }
        
        public abstract double ShowFlatTop { get; set; }

        [Auto("IP地址")]
        public abstract string IP { get; set; }//IP地址
        [Auto("快成型门限值")]
        public abstract int FastLimit { get; set; }
        [Auto("截距")]
        public abstract double Intercept { get; set; }

        public abstract FPGAParams Init(OFFON BaseResume, OFFON BaseLimit, OFFON HeapUP, int Rate, double PeakingTime, double FlatTop, int SlowLimit, double ShowPeakingTime, double ShowFlatTop, string IP, int FastLimit, double Intercept);
    }

    /// <summary>
    /// 仪器设备
    /// </summary>
    [Serializable]
    public abstract class Device : DbObjectModel<Device>
    {
        /// <summary>
        /// 包含测量条件
        /// </summary>  
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<Condition> Conditions { get; set; }
        /// <summary>
        /// 包含探测器结构
        /// </summary>
        [HasOne(OrderBy = "Id")]
        public abstract Detector Detector { get; set; }
        /// <summary>
        /// 包含系统配置
        /// </summary>
        [HasOne(OrderBy = "Id")]
        public abstract SysConfig SysConfig { get; set; }
        /// <summary>
        /// 包含光管信息
        /// </summary>
        [HasOne(OrderBy = "Id")]
        public abstract Tubes Tubes { get; set; }
        /// <summary>
        /// 包含FPGA参数信息
        /// </summary>
        [HasOne(OrderBy = "Id")]
        public abstract FPGAParams FPGAParams { get; set; }
        /// <summary>
        /// 包含准直器信息
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<Collimator> Collimators { get; set; }
        /// <summary>
        /// 包含滤光片信息
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<Filter> Filter { get; set; }
        /// <summary>
        /// 包含样品腔信息
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<Chamber> Chamber { get; set; }
        /// <summary>
        /// 包含样品腔信息
        /// </summary>
        [HasMany(OrderBy = "Id")]
        public abstract HasMany<Target> Target { get; set; }
        /// <summary>
        /// 仪器名称
        /// </summary>
        [Length(ColLength.DeviceName)]
        public abstract string Name { get; set; }
        /// <summary>
        /// 是否有滤光片电机
        /// </summary>
        public abstract bool HasFilter { get; set; }
        ///<summary>
        ///是否有准直器电机
        ///</summary>
        public abstract bool HasCollimator { get; set; }
        /// <summary>
        /// 是否有样品腔电机
        /// </summary>
        public abstract bool HasChamber { get; set; }
        /// <summary>
        /// 是否有靶材电机
        /// </summary>
        public abstract bool HasTarget { get; set; }
        [Auto("谱长度")]
        public abstract SpecLength SpecLength { get; set; }
        [Auto("管压上限")]
        public abstract int MaxVoltage { get; set; }
        [Auto("管流上限")]
        public abstract int MaxCurrent { get; set; }
        [Auto("是否支持DP5")]
        public abstract bool IsDP5 { get; set; }
        
        [Auto("通信类型")]
        public abstract ComType ComType { get; set; }
        [Auto("端口号")]
        public abstract int ComNum { get; set; }
        [Auto("波特率")]
        public abstract int Bits { get; set; }
        [Auto("Pocket类型")]
        public abstract Pocket Pocket { get; set; }
        [Auto("管压比例因子")]
        public abstract double VoltageScaleFactor { get; set; }
       
        [Auto("管流比例因子")]
        public abstract double CurrentScaleFactor { get; set; }

        [Auto("是否有真空泵")]
        public abstract bool HasVacuumPump { get; set; }
        [Auto("真空度显示方式")]
        public abstract VacuumPumpType VacuumPumpType { get; set; }
        [Auto("是否有电磁铁")]
        public abstract bool HasElectromagnet { get; set; }
        [Auto("是否允许开盖")]
        public abstract bool IsAllowOpenCover { get; set; }

        [Auto("是否开启仪器自动检测")]
        public abstract bool IsAutoDetection { get; set; }

        public abstract int CollimatorElectricalCode { get; set; }//准直器编号
        public abstract int CollimatorElectricalDirect { get; set; }//方向
        public abstract int CollimatorMaxNum { get; set; }//最大编号
        public abstract int CollimatorSpeed { get; set; }//速度
        public abstract int FilterElectricalCode { get; set; }//滤光片编号
        public abstract int FilterElectricalDirect { get; set; }//方向
        public abstract int FilterMaxNum { get; set; }//最大编号
        public abstract int FilterSpeed { get; set; }//速度
        public abstract int ChamberElectricalCode { get; set; }//样品腔编号
        public abstract int ChamberElectricalDirect { get; set; }//方向
        public abstract int ChamberMaxNum { get; set; }//最大编号
        public abstract int ChamberSpeed { get; set; }//速度

        public abstract int TargetElectricalCode { get; set; }//靶材编号
        public abstract int TargetElectricalDirect { get; set; }//方向
        public abstract int TargetMaxNum { get; set; }//最大编号
        public abstract int TargetSpeed { get; set; }//速度

        public abstract bool HasMotorX { get; set; }//是否有X轴电机
        public abstract int MotorXCode { get; set; }//X轴电机编号
        public abstract int MotorXDirect { get; set; }//X轴电机方向
        public abstract int MotorXSpeed { get; set; }//X轴电机速度
        public abstract int MotorXMaxStep { get; set; }//X轴电机最大步长
        public abstract bool HasMotorY { get; set; }//是否有Y轴电机
        public abstract int MotorYCode { get; set; }//Y轴电机编号
        public abstract int MotorYDirect { get; set; }//Y轴电机方向
        public abstract int MotorYSpeed { get; set; }//Y轴电机速度
        public abstract int MotorYMaxStep { get; set; }//Y轴电机最大步长
        public abstract bool HasMotorZ { get; set; }//是否有Z轴电机
        public abstract int MotorZCode { get; set; }//Z轴电机编号
        public abstract int MotorZDirect { get; set; }//Z轴电机方向
        public abstract int MotorZSpeed { get; set; }//Z轴电机速度
        public abstract int MotorZMaxStep { get; set; }//Z轴电机最大步长
        public abstract double MotorZDutyRatioUp { get; set; }
        public abstract double MotorZDutyRatioDown { get; set; }
        public abstract DllType PortType { get; set; }//通讯类型
        public abstract UsbVersion UsbVersion { get; set; } //usb版本
        public abstract bool IsPassward { get; set; } //Dll是否加密

        [Length(ColLength.DeviceID)]
        public abstract string DeviceID { get; set; }//设备ID
        public abstract bool IsDefaultDevice { get; set; }//是否默认设备
        public abstract bool HasMotorSpin { get; set; }//是否有Y1电机
        public abstract int MotorSpinCode { get; set; }//Y1电机编号
        public abstract int MotorSpinDirect { get; set; }//Y1电机方向
        public abstract int MotorSpinSpeed { get; set; }//Y1电机速度
        public abstract int MotorSPinMaxStep { get; set; }//Y1电机最大步长
        public abstract bool HasMotorLight { get; set; }//是否有光栅电机
        public abstract int MotorLightDirect { get; set; }//光栅电机方向
        public abstract int MotorLightCode { get; set; }//光栅电机编号
        public abstract int MotorLightSpeed { get; set; }//光栅电机速度
        public abstract int MotorLightMaxStep { get; set; }//光栅电机最大步长
        public abstract int MotorResetX { get; set; }  //移动平台复位点X位置
        public abstract int MotorResetY { get; set; }  //移动平台复位点Y位置
        public abstract string EncoderFormula { get; set; }

        [Auto("DP5类型")]
        public abstract Dp5Version Dp5Version { get; set; }

        public abstract Device Init(string Name,
            //bool HasVacuumPump,
            //bool HasElectromagnet,
             string deviceId,
             DllType PortType,
             double VoltageScaleFactor,
             double CurrentScaleFactor,
             string DeviceID,
             SpecLength SpecLength,
             int CollimatorElectricalCode,
             int FilterElectricalCode,
             int ChamberElectricalCode,
             int CollimatorSpeed, int FilterSpeed, int ChamberSpeed,
            int MotorXSpeed, int MotorYSpeed, int MotorZSpeed, int MaxVoltage, int MaxCurrent, int TargetElectricalCode, int TargetSpeed,
            int MotorResetX, int MotorResetY,string EncoderFormula
            );

    }

}
