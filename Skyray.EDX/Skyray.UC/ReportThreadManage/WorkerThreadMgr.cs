using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Skyray.UC
{
    public class WorkerThreadMgr
    {
        /// <summary>
        /// 外部传入的完成事件
        /// </summary>
        private EventWaitHandle complete;

        /// <summary>
        /// 内部状态的监控事件
        /// </summary>
        private AutoResetEvent workderCompleteEvent = new AutoResetEvent(false);

        /// <summary>
        /// 当前管理的Worker的数目
        /// </summary>
        private int runningWorkerCount = 0;

        private object thisSection = new object();





        public WorkerThreadMgr(EventWaitHandle completeEvent)
        {
            this.complete = completeEvent;
        }

        /// <summary>
        /// 监测任务是否处理结束
        /// </summary>
        public void Complete()
        {
            lock (thisSection)
            {
                runningWorkerCount--;
                workderCompleteEvent.Set();

                if (runningWorkerCount < 1)
                {
                    complete.Set();
                }
            }
        }





        /// <summary>
        /// 当前运行的线程数目
        /// </summary>
        public int WorkerCount
        {
            get
            {
                lock (thisSection)
                    return runningWorkerCount;
            }
        }




        /// <summary>
        /// 开始执行异步任务执行
        /// </summary>
        /// <param name="worker">执行任务线程</param>
        /// <param name="sentenceTask">任务</param>
        /// <param name="wordSearch">查询实现</param>
        /// <param name="languageType">语言类型</param>
        public void StartWorker(ReportWorker reportWorker,WorkerThread worker, long historyRecordTask, string TempletFileName, string UserName,
            bool isReportEXCEL, bool isReportPDF, bool isReportXML, bool isContentXML, string reportPath, string reportName)
        {
            lock (thisSection)
                runningWorkerCount++;

            worker.ExecuteAsync(reportWorker,historyRecordTask, this, TempletFileName, UserName,
                isReportEXCEL, isReportPDF, isReportXML, isContentXML, reportPath, reportName);
        }

        /// <summary>
        /// 阻塞当前线程，等待空闲的线程
        /// </summary>
        public void WaitForAvailableWorker()
        {
            WaitHandle[] handles = new WaitHandle[2] { workderCompleteEvent, complete };
            WaitHandle.WaitAny(handles, TimeSpan.FromSeconds(10), false);
        }
    }
}
