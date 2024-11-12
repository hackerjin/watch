using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    /// <summary>
    /// add by cyq
    /// </summary>
    public class RayTube
    {
        private const int MaxVollage = 45;///<管压最大值
        private const int MinVollage = 10;///<管压最小值
        private const int MaxCurrent = 600;///<管流最大值
        private const int MinCurrent = 0;///<管流最小值
        private const int MaxGain = 255;///<放大倍数最大值
        private const int MinGain = 0;///<放大倍数最小值
        private const int MaxFineGain = 255;///<放大倍数最大值
        private const int MinFineGain = 0;///<放大倍数最小值
        private const int DueTime = 8;  ///<8秒钟用来设置一次高压
        public int TargetAtomicNumber;///<靶材原子系数
        public double TargetTakeOffAngle;///<靶材角度
        public double WindowThickness;///<铍窗厚度(mm)
                                      ///
        [Length(ColLength.WindowFormula)]
        public string WindowFormula;  ///<窗口物质表达式
        public double IncidentAngle;///<入射角
        public double EmergentAngle;///<出射角
    }
}
