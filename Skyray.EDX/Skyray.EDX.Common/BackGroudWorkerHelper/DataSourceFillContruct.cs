using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

using Lephone.Data.Definition;
using Lephone.Data.Common;
using Lephone.Util;
using Skyray.Controls;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common
{
    public class DataSourceFillContruct
    {
        private MessageInterface messageProcess;
        public event UpdateStatesInformation onStatesInformation;

        /// <summary>
        /// 根据指定的排列方向对容器进行布局初始化
        /// </summary>
        /// <param name="sendContext">发送的任务信息</param>
        /// <param name="orientType">容器的方向</param>
        /// <param name="dataGridView">指定的容器</param>
        /// <param name="isFixed">是否固定</param>
        public void GetSendContextInfo(BaseMessage sendContext, bool orientType, DataGridViewW dataGridView, bool isFixed)
        {
             if (sendContext == null || dataGridView == null)
                return;
             GetInstanceInterface(sendContext.type);
             messageProcess.ContructDataContainer(sendContext, isFixed, orientType, dataGridView);
        }

        /// <summary>
        /// 对相应的容器进行充值
        /// </summary>
        /// <param name="sendContext">发送的内容</param>
        /// <param name="orientType">容器的方向</param>
        /// <param name="isFixed">是否可动态增加</param>
        /// <param name="dataGridView">待填充的容器</param>
        public void FillValueBySendContext(BaseMessage sendContext, bool orientType, bool isFixed, DataGridViewW dataGridView,bool flag,ElementList list)
        {
            if (sendContext == null || dataGridView == null)
                return;
            GetInstanceInterface(sendContext.type);
            messageProcess.RecordElementValusInfo(flag, orientType, isFixed, sendContext, dataGridView, list);
        }

        /// <summary>
        /// 根据容器的类型实例化对象
        /// </summary>
        /// <param name="type"></param>
        private void GetInstanceInterface(DataGridViewType type)
        {
            switch (type)
            {
                case DataGridViewType.StatisticsInfo:
                    messageProcess = new StaticsProcess();
                    break;
                case DataGridViewType.TestResult:
                    messageProcess = new MeasureRecordProcess();
                    messageProcess.onUpdateStatesInformation += new UpdateStatesInformation(messageProcess_onUpdateStatesInformation);
                    break;
                case DataGridViewType.Calibration:
                    messageProcess = new WorkCurveProcess();
                    break;
                case DataGridViewType.Device:
                    messageProcess = new DeviceProcess();
                    break;
                case DataGridViewType.Quality:
                    messageProcess = new QualitativeProcess();
                    break;
                case DataGridViewType.Spec:
                    messageProcess = new WorkSpecProcess();
                    break;
                case DataGridViewType.TestThick:
                    messageProcess = new ThickCaculateProcess();
                    messageProcess.onUpdateStatesInformation+=new UpdateStatesInformation(messageProcess_onUpdateStatesInformation);
                    break;
                case DataGridViewType.CurveList:
                    messageProcess = new CurveProcess();
                    break;
                case DataGridViewType.ThickStatics:
                    messageProcess = new ThickStaticsProcess();
                    break;
                default:
                    break;
            }
        }

        private void messageProcess_onUpdateStatesInformation(DataGridViewW datagridView,ElementList list,int currentTimes)
        {
            if (onStatesInformation != null)
                onStatesInformation(datagridView, list, currentTimes);
        }
    }

    public class PackageBackgroudPassingParams
    {
        public BaseMessage PassingInstance { set; get; }

        public DataSourceFillContruct ExcuteInstance { set; get; }


        public PackageBackgroudPassingParams(BaseMessage passingInstance, DataSourceFillContruct excuteInstance)
        {
            this.PassingInstance = passingInstance;
            this.ExcuteInstance = excuteInstance;
        }
    }
}
