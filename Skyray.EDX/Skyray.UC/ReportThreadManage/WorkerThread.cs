using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Skyray.UC
{
    public class WorkerThread
    {
        /// <summary>
        /// 任务
        /// </summary>
        private long historyRecord_IDTask;//历史记录ID
        private string TempletFileName;
        private string UserName;
        private bool isReportEXCEL = false;
        private bool isReportPDF = false;
        private bool isReportXML = false;
        private bool isContentXML = false;
        private string reportPath;
        private string reportName;
        private ReportWorker reportWorker;

        /// <summary>
        /// 内部线程
        /// </summary>
        private Thread executeThread = null;

        /// <summary>
        /// 当前工作线程对象的管理对象
        /// </summary>
        private WorkerThreadMgr workerMgr;


        private object thisSection = new object();

        public WorkerThread()
        { }

        /// <summary>
        /// 异步执行词汇数据库查询功能
        /// </summary>
        /// <param name="sentenceTask">句子的任务定义</param>
        /// <param name="onComplete">线程管理器</param>
        /// <param name="wordSearch">查询实现类</param>
        /// <param name="languageType">语言类型</param>
        public void ExecuteAsync(ReportWorker reportWorker,long sentenceTask, WorkerThreadMgr onComplete, string TempletFileName, string UserName,
            bool isReportEXCEL, bool isReportPDF, bool isReportXML, bool isContentXML, string reportPath,string reportName)
        {
            System.Diagnostics.Trace.WriteLine("Enter thread + " + DateTime.Now.ToString());

            lock (thisSection)
            {
                this.reportWorker = reportWorker;
                this.workerMgr = onComplete;
                this.historyRecord_IDTask = sentenceTask;
                this.TempletFileName = TempletFileName;
                this.UserName = UserName;
                this.reportPath = reportPath;
                this.reportName = reportName;
                this.isReportEXCEL = isReportEXCEL;
                this.isReportPDF = isReportPDF;
                this.isReportXML = isReportXML;
                this.isContentXML = isContentXML;
                this.executeThread = new Thread(new ThreadStart(ExecuteTread));
                executeThread.ApartmentState = ApartmentState.STA;
                executeThread.Name = "Corpus Words Find Worker Thread";
                executeThread.Start();
            }
        }

        /// <summary>
        /// 线程执行函数
        /// </summary>
        public void ExecuteTread()
        {

            if (historyRecord_IDTask == null)
                return;
            List<long> hisList = new List<long>();
            hisList.Add(historyRecord_IDTask);
            reportWorker.SetReport(false,hisList,
                TempletFileName,
                UserName,
                reportPath,
                reportName,
                isReportEXCEL,
                isReportPDF,
                isReportXML,
                isContentXML,0);

            //ReportWorker reportWorker = new ReportWorker(historyRecord_IDTask,
            //    TempletFileName,
            //    UserName,
            //    reportPath,
            //    reportName,
            //    isReportEXCEL,
            //    isReportPDF,
            //    isReportXML,
            //    isContentXML);



            //ChineseWordsSearch chineseWordsSearch = new ChineseWordsSearch();

            //IList<WordCollection> sentenceTasklist = new List<WordCollection>();
            //sentenceTasklist = chineseWordsSearch.SearchChineseWordFromDataBase(sentenceTask);

            //if (sentenceTasklist == null || sentenceTasklist.Count == 0) { this.workerMgr.Complete(); return; }


            //Word sentenceW = new Word(sentenceTask.Contents, sentenceTask.StartPosition, sentenceTask.Length);
            //this.workerMgr.ReportResults(sentenceW, sentenceTasklist);
            this.workerMgr.Complete();
        }



        /// <summary>
        /// 判断当前的线程是否空闲
        /// </summary>
        public bool ReadyForProcessing
        {
            get
            {
                lock (thisSection)
                {
                    return (executeThread == null || executeThread.ThreadState == ThreadState.Stopped);
                }
            }
        }
    }
}
