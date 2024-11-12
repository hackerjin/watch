using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Skyray.Print.ReportThreadManage
{
    public class ReportWorkerThread
    {
        /// <summary>
        /// 内部任务列表
        /// </summary>
        //private string historyRecordID;

        /// <summary>
        /// 内部线程
        /// </summary>
        private Thread executeThread = null;

        /// <summary>
        /// 当前工作线程对象的管理对象
        /// </summary>
        private ReportWorkerThreadMgr workerMgr;

        ///// <summary>
        ///// 具体查询的实现
        ///// </summary>
        //private WordSearch wordSearch;

        //private string languageType;
        private object thisSection = new object();

        public ReportWorkerThread()
        {
            
        }

        private List<long> seleHistoryRecordList;
        private bool isShow;
        private string templetFileName;
        private string sUserName;
        private string sReportPath;
        private string reportName;
        private bool isReportEXCEL;
        private bool isReportPDF;
        private bool isReportXML;
        private bool isContentXML;
        private double dWeight;

        /// <summary>
        /// 异步执行词汇数据库查询功能
        /// </summary>
        /// <param name="sentenceTask">句子的任务定义</param>
        /// <param name="onComplete">线程管理器</param>
        /// <param name="wordSearch">查询实现类</param>
        /// <param name="languageType">语言类型</param>
        public void ExecuteAsync(ReportWorkerThreadMgr onComplete, bool isShow, List<long> seleHistoryRecordList, string templetFileName,
            string sUserName, string sReportPath, string reportName, bool isReportEXCEL, bool isReportPDF, bool isReportXML, bool isContentXML
            , double dWeight)
        {
            //System.Diagnostics.Trace.WriteLine("Enter thread + " + DateTime.Now.ToString());

            lock (thisSection)
            {
                this.workerMgr = onComplete;
                this.isShow = isShow;
                this.templetFileName = templetFileName;
                this.sUserName = sUserName;
                this.sReportPath = sReportPath;
                this.reportName = reportName;
                this.isReportEXCEL = isReportEXCEL;
                this.isReportPDF = isReportPDF;
                this.isReportXML = isReportXML;
                this.isContentXML = isContentXML;
                this.dWeight = dWeight;
                this.seleHistoryRecordList = seleHistoryRecordList;
                this.executeThread = new Thread(new ThreadStart(ExecuteTread));
                //this.executeThread.ApartmentState = System.Threading.ApartmentState.STA;
                this.executeThread.SetApartmentState(ApartmentState.STA);
                executeThread.Start();
            }
        }

        /// <summary>
        /// 线程执行函数
        /// </summary>
        public void ExecuteTread()
        {

            if (seleHistoryRecordList == null || seleHistoryRecordList.Count<=0)
                return;


            //System.Diagnostics.Trace.WriteLine("Enter thread + " + seleHistoryRecordList.Count.ToString());

            ReportWorker SetreportWorker = new ReportWorker();
            SetreportWorker.SetReport(this.isShow, this.seleHistoryRecordList, this.templetFileName, this.sUserName, this.sReportPath, this.reportName,
                    this.isReportEXCEL, this.isReportPDF, this.isReportXML, this.isContentXML, this.dWeight);

            //System.Diagnostics.Trace.WriteLine("Enter thread + " + historyRecordID.ToString());

            //List<WordCollection> newlist = new List<WordCollection>();

            //ChineseWordsSearch chineseWordsSearch = new ChineseWordsSearch();

            //IList<WordCollection> sentenceTasklist =new List<WordCollection>();
            //sentenceTasklist = chineseWordsSearch.SearchChineseWordFromDataBase(sentenceTask);

            //if (sentenceTasklist == null || sentenceTasklist.Count == 0) { this.workerMgr.Complete(); return; }


            //Word sentenceW = new Word(sentenceTask.Contents, sentenceTask.StartPosition, sentenceTask.Length);
            //this.workerMgr.ReportResults(sentenceW,sentenceTasklist);
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
