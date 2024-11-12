using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.Data;
using Skyray.Controls;
using Skyray.EDXRFLibrary.Spectrum;
using System.Drawing;

namespace Skyray.UC
{
    /// <summary>
    /// 所有的委托类型
    /// </summary>
    public class EventDelegate
    {
        /// <summary>
        /// 打开工作曲线
        /// </summary>
        //public delegate void InitDeviceParameter();
        public delegate void MeasureResult(int currentTimes, Skyray.EDXRFLibrary.ElementList elementList);
        ///// <summary>
        ///// 添加感兴趣元素
        ///// </summary>
        ///// <param name="currentWorkCurve"></param>
        //public delegate void NotifyAddElements(WorkCurve currentWorkCurve);
        //public delegate void LoadDataSource(ref List<TreeNodeInfo> list);
        /// <summary>
        /// 开始扫描处理
        /// </summary>
        /// <param name="testDevice"></param>
        public delegate void ExcuteTestBackGroudWorker(TestDevicePassedParams testDevice);

        public delegate void ContinuousData(List<DataTable> dt, List<object> obj);

        public delegate void HistoryTemplateSave(string excelSavePath, DataTable dtResult, DataTable dtStatics);

        public delegate void TestResultReport(DataGridViewW dataGrid, string AuK);

        public delegate void VacummEmptyThread();


        ///// <summary>
        ///// 打开工作谱
        ///// </summary>
        ///// <param name="specListCurrent"></param>
        public delegate void CreateSpecs(List<SpecListEntity> specListCurrent);

        /// <summary>
        /// 摄像头当前的状态
        /// </summary>
        /// <param name="checkedState"></param>
        public delegate void ReturnCameralState(bool checkedState);

        /// <summary>
        /// 摄像头的操作
        /// </summary>
        public delegate void CameralOperation();

        public delegate bool PrintTemplateSource();

        //public delegate void SaveChangeStandand();

        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="e"></param>
        public delegate void KeyMouseEvent(EventArgs e);

        public delegate void EventNotifySizeChange(object sender, int length);

        public delegate void EventMoveStationParams(int xyspeed);
        public delegate void EventHandleDialog(bool flag);

       
    }

    public class AutoAnalysisClass
    {
        public Dictionary<int, string[]> strDic { set; get; }
        public Dictionary<int, int[]> lineDic { set; get; }
        public bool bShow { set; get; }

        public AutoAnalysisClass()
        { }

        public AutoAnalysisClass(bool bshow)
        {
            this.bShow = bshow;
        }

        public AutoAnalysisClass(Dictionary<int, string[]> strDic, Dictionary<int, int[]> lineDic, bool bShow)
        {
            this.strDic = strDic;
            this.lineDic = lineDic;
            this.bShow = bShow;
        }
    }
}
