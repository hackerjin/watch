using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;

namespace Skyray.MessageInfo
{
    public class DeviceInfo : BaseMessage
    {
        [Auto("Current", "管流")]
        public string Current { set; get; }

        [Auto("Voltage", "管压")]
        public string Voltage { set; get; }

        [Auto("CoarseCode", "粗调")]
        public string CoarseCode { set; get; }

        [Auto("FineCode", "精调")]
        public string FineCode { set; get; }

        [Auto("CountRate", "计数率")]
        public string CountRate { set; get; }

        [Auto("MeasureTime", "测量时间")]
        public string MeasureTime { set; get; }

        [Auto("Channel", "峰通道")]
        public string Channel { set; get; }

        [Auto("Count", "次数")]
        public string Count { set; get; }

        [Auto("VacuumDegree", "真空度")]
        public string VacuumDegree { set; get; }

        public DeviceInfo()
        {
            this.Position = 0;
            this.IsFixed = true;
            this.type = DataGridViewType.Device;
        }
    }
}
