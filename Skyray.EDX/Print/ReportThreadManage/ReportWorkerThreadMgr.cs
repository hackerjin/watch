using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Skyray.Print.ReportThreadManage
{
    public class ReportWorkerThreadMgr
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

        /// <summary>
        /// 所有运行线程的运行结果
        /// </summary>
        //private Dictionary<Word, IList<WordCollection>> results = new Dictionary<Word, IList<WordCollection>>();



        public ReportWorkerThreadMgr(EventWaitHandle completeEvent)
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

        ///// <summary>
        ///// 回调函数，让运行线程将得到的结构保持在管理类中
        ///// </summary>
        ///// <param name="w"></param>
        ///// <param name="dic"></param>
        //public void ReportResults(Word w, IList<WordCollection> dic)
        //{
        //    lock (thisSection)
        //        this.results.Add(w, dic);
        //}


       
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

        ///// <summary>
        ///// 最终任务运行的结构
        ///// </summary>
        //public Dictionary<Word, IList<WordCollection>> Results
        //{
        //    get
        //    {
        //        lock (thisSection)
        //            return this.results;
        //    }
        //}


       
        /// <summary>
        /// 开始执行异步任务执行
        /// </summary>
        /// <param name="worker">执行任务线程</param>
        /// <param name="sentenceTask">任务</param>
        /// <param name="wordSearch">查询实现</param>
        /// <param name="languageType">语言类型</param>
        public void StartWorker(ReportWorkerThread worker, bool isShow, List<long> seleHistoryRecordList, string templetFileName,
            string sUserName, string sReportPath, string reportName, bool isReportEXCEL, bool isReportPDF, bool isReportXML, bool isContentXML
            , double dWeight)
        {
            lock (thisSection)
                runningWorkerCount++;

            worker.ExecuteAsync(this, isShow, seleHistoryRecordList, templetFileName, sUserName, sReportPath,
                reportName, isReportEXCEL, isReportPDF, isReportXML, isContentXML, dWeight);
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
