using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// 测量条件
    /// </summary>
    public abstract class ExcitationConditions : DbObjectModel<ExcitationConditions>
    {
        /// <summary>
        /// 靶材原子序数
        /// </summary>
        public abstract int TargetAtomicNumber { get; set; }
        /// <summary>
        /// 靶材射线出射角(°)
        /// </summary>
        public abstract double TargetTakeOffAngle { get; set; }
        /// <summary>
        /// 光管的管压（千伏）
        /// </summary>
        public abstract double TubeVoltage { get; set; }
        /// <summary>
        /// 窗口厚度（毫米）
        /// </summary>
        public abstract double TubeWindowThickness { get; set; }
        /// <summary>
        /// 样品入射角(°)
        /// </summary>
        public abstract double IncidentAngle { get; set; }
        /// <summary>
        /// 样品出射角(°)
        /// </summary>
        public abstract double EmergentAngle { get; set; }
        /// <summary>
        /// 滤光片的原子序数，如果是化合物设为-1
        /// </summary>
        public abstract double PrimaryFilterAtomicNumber { get; set; }
        /// <summary>
        /// 滤光片的厚度（毫米）
        /// </summary>
        public abstract double PrimaryFilterThickness { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>       
        public abstract ExcitationConditions Init(int TargetAtomicNumber,
            double TargetTakeOffAngle,
            double TubeVoltage,
            double TubeWindowThickness,
            double IncidentAngle,
            double EmergentAngle,
            double PrimaryFilterAtomicNumber,
            double PrimaryFilterThickness
            );

    }

}