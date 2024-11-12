using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;

namespace Skyray.MessageInfo
{
    public class WorkSpec : BaseMessage
    {
        [Auto("SampleName", "样品名称")]
        public string SampleName { set; get; }

        [Auto("Supplier", "供应商")]
        public string Supplier { set; get; }

        [Auto("Weight", "重量")]
        public string Weight { set; get; }

        [Auto("Shape", "形状")]
        public string Shape { set; get; }

        [Auto("MeasureDate", "测量日期")]
        public string MeasureDate { set; get; }

        [Auto("MeasureTime", "测量时间")]
        public string MeasureTime { set; get; }

        [Auto("Operator", "操作员")]
        public string Operator { set; get; }

        [Auto("Current", "管流")]
        public string Current { set; get; }

        [Auto("Voltage", "管压")]
        public string Voltage { set; get; }

        [Auto("Filter", "滤光片")]
        public string Filter { set; get; }

        [Auto("Collimator", "准直器")]
        public string Collimator { set; get; }

        [Auto("Channel", "通道")]
        public string Channel { set; get; }

        [Auto("Summary", "描述")]
        public string Summary { set; get; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public WorkSpec()
        {
            this.Position = 3;
            this.IsFixed = true;
            this.type = DataGridViewType.Spec;
        }
    }
}
