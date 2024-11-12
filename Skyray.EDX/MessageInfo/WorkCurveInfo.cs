using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;

namespace Skyray.MessageInfo
{
    public class WorkCurveInfo : BaseMessage
    {
        [Auto("Calibration", "校正曲线")]
        public string Calibration { set; get; }

        [Auto("MeasureTime", "测量时间")]
        public string MeasureTime { set; get; }

        [Auto("Voltage", "管压")]
        public string Voltage { set; get; }

        [Auto("Current", "管流")]
        public string Current { set; get; }

        [Auto("Filter", "滤光片")]
        public string Filter { set; get; }

        [Auto("Collimator", "准直器")]
        public string Collimator { set; get; }

        [Auto("VacuumizeByTime", "时间抽真空")]
        public string VacuumizeByTime { set; get; }

        [Auto("AdjustCountRate", "调节计数率")]
        public string AdjustCountRate { set; get; }

        [Auto("MaxCountRate", "最大计数率")]
        public string MaxCountRate { set; get; }

        [Auto("MinCountRate", "最小计数率")]
        public string MinCountRate { set; get; }

        [Auto("InitalElem", "初始化元素")]
        public string InitalElem { set; get; }

        [Auto("InitalChann", "初始化通道")]
        public string InitalChann { set; get; }

        [Auto("MeasureParam", "测量条件")]
        public string MeasureParam { set; get; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public WorkCurveInfo()
        {
            this.Position = 1;
            this.IsFixed = true;
            this.type = DataGridViewType.Calibration;
        }
    }
}
