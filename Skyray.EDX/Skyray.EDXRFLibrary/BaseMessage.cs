using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public class BaseMessage
    {
        /// <summary>
        /// 是固定的,还是动态增加的
        /// </summary>
        public bool IsFixed { set; get; }

        /// <summary>
        /// 发送接收容器的位置
        /// </summary>
        public int Position { set; get; }

        /// <summary>
        /// 发送的内容是否有值
        /// </summary>
        public bool IsValidate { set; get; }

        /// <summary>
        /// 容器布局是否为主容器可见
        /// </summary>
        public bool RootVisible { set; get; }

        /// <summary>
        /// 容器的名称
        /// </summary>
        public DataGridViewType type { set; get; }


        public ElementList elementList { set; get; }

        //public FuncType funcType { set; get; }
    }

    public enum DataGridViewType
    {
        Quality,
        Spec,
        Calibration,
        StatisticsInfo,
        TestResult,
        Device,
        TestThick,
        CurveList,
        ThickStatics
    }

    public struct DataGridViewName
    {
        public static string[] strList = { "定性分析", "工作谱", "工作曲线", "统计信息", "测试结果", "设备", "测厚统计", "曲线列表" };
    }
}
