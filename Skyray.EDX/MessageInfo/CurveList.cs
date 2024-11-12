using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;

namespace Skyray.MessageInfo
{
    public class CurveItem
    {
        [Auto("Calibration", "校正曲线")]
        public string Calibration { get; set; }

        [Auto("CalibrationType", "曲线类型")]
        public string CalibrationType { get; set; }

        [Auto("CalibrationCondition", "测试条件")]
        public string ConditionName { get; set; }

        public long Id { get; set; }

        public CurveItem(string calibration, string calibrationType, string conditionName, long id)
        {
            this.Calibration = calibration;
            this.CalibrationType = calibrationType;
            this.ConditionName = conditionName;
            this.Id = id;
        }

    }


    public class Curve : BaseMessage
    {
        /// <summary>
        /// 统计结果
        /// </summary>
        public List<CurveItem> curveResult { set; get; }

        //[Auto("行数")]
        //public int rowNumber { set; get; }
        public int ConditionType { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Curve()
        {
            this.IsFixed = false;
            //this.rowNumber = 1;
            this.Position = 7;
            ConditionType = -1;
            this.type = DataGridViewType.CurveList;
            curveResult = new List<CurveItem>();
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="staticsResult"></param>
        /// <param name="rowNumber"></param>
        public Curve(List<CurveItem> curveResult, int type)
            : this()
        {
            this.curveResult = curveResult;
            //this.rowNumber = rowNumber;
            this.ConditionType = type;
        }
    }
}
