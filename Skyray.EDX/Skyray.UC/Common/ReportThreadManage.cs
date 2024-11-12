using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDX.Common.ReportHelper;
using System.Xml;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.Print.ReportThreadManage;






namespace Skyray.UC
{
    public class ReportThreadManage
    {
        public List<HistoryRecord> historyRecordList = new List<HistoryRecord>();
        private bool isReportEXCEL = false;
        private bool isReportPDF = false;
        private bool isReportXML = false;
        private bool isContentXML = false;
        private ReportWorker SetreportWorker = new ReportWorker();

        private AutoResetEvent moreWork = new AutoResetEvent(false);

        public ReportThreadManage()
        {
        }

        #region 备注

        //public void GetHistoryRecordReportTasksThread()//未使用
        //{
        //    WorkerThread[] reportWorker = new WorkerThread[5];
        //    for (int i = 0; i < reportWorker.Length; i++)
        //    {
        //        reportWorker[i] = new WorkerThread();
        //    }
        //    WorkerThreadMgr reportWorkerThreadMgr = new WorkerThreadMgr(moreWork);
        //    bool b = true;

        //    //获取模板名称
        //    string TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
        //    string UserName = FrmLogon.userName;
        //    XmlNode LoadSpecifiedNode = ReportTemplateHelper.LoadSpecifiedNode("Report", "IsAutoSaveReport");

        //    foreach (XmlAttribute xmlatt in LoadSpecifiedNode.Attributes)
        //        if (xmlatt.Name == "ReportEXCEL") isReportEXCEL = (xmlatt.Value.ToString() == "1") ? true : false;
        //        else if (xmlatt.Name == "ReportPDF") isReportPDF = (xmlatt.Value.ToString() == "1") ? true : false;
        //        else if (xmlatt.Name == "ReportXML") isReportXML = (xmlatt.Value.ToString() == "1") ? true : false;
        //        else if (xmlatt.Name == "ContentXML") isContentXML = (xmlatt.Value.ToString() == "1") ? true : false;
        //    string ReportPath = (WorkCurveHelper.ExcelPath.IsNullOrEmpty()) ? Application.StartupPath + "\\Report" : WorkCurveHelper.ExcelPath; ;


        //    while (b)
        //    {
        //        foreach (WorkerThread worker in reportWorker)
        //        {
        //            if (worker.ReadyForProcessing && historyRecordList.Count > 0)//判断当前线程时候处于空闲和是否存在需要分析的对象
        //            {
        //                HistoryRecord currHistoryRecord = historyRecordList[0];
        //                long hisRecordid = currHistoryRecord.Id;
        //                historyRecordList.Remove(currHistoryRecord);
        //                HistoryRecord tt = HistoryRecord.FindById(hisRecordid);
        //                SpecListEntity temp = DataBaseHelper.QueryByEdition(tt.SpecListName, tt.FilePath, tt.EditionType);
        //                //List<SpecList> hisSpecList = SpecList.FindBySql("select * from speclist where id in (select speclistid from   historyrecord where id=" + hisRecordid + " )");
        //                if (temp == null) continue;

        //                string reportName = temp.Name;
        //                reportWorkerThreadMgr.StartWorker(SetreportWorker, worker, hisRecordid, TempletFileName, UserName,
        //                    isReportEXCEL, isReportPDF, isReportXML, isContentXML, ReportPath, reportName);
        //                moreWork.Reset();
        //            }

        //            if (historyRecordList.Count == 0) { b = false; return; }


        //        }
        //    }
        //}

        //public void GetRecordReport(object obj)//自动保存
        //{
        //    WorkCurveHelper.PopThreadList();//多线程按顺序输出 何晓明 20120310
        //    //if (System.Diagnostics.Debugger.IsAttached)
        //    //    Thread.Sleep(5000-int.Parse(WorkCurveHelper.lsThread.Last().Name)*1000);
        //    string sSpecListid = obj.ToString();
        //    if (sSpecListid.IndexOf(",") == -1)
        //    {
        //        //获取模板名称
        //        string TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
        //        string UserName = FrmLogon.userName;
        //        XmlNode LoadSpecifiedNode = ReportTemplateHelper.LoadSpecifiedNode("Report", "IsAutoSaveReport");

        //        foreach (XmlAttribute xmlatt in LoadSpecifiedNode.Attributes)
        //            if (xmlatt.Name == "ReportEXCEL") isReportEXCEL = (xmlatt.Value.ToString() == "1") ? true : false;
        //            else if (xmlatt.Name == "ReportPDF") isReportPDF = (xmlatt.Value.ToString() == "1") ? true : false;
        //            else if (xmlatt.Name == "ReportXML") isReportXML = (xmlatt.Value.ToString() == "1") ? true : false;
        //            else if (xmlatt.Name == "ContentXML") isContentXML = (xmlatt.Value.ToString() == "1") ? true : false;
        //        string ReportPath = (WorkCurveHelper.ExcelPath.IsNullOrEmpty()) ? Application.StartupPath + "\\Report" : WorkCurveHelper.ExcelPath; ;

        //        SpecListEntity currSpecList = WorkCurveHelper.DataAccess.Query(obj.ToString());


        //        string reportName = currSpecList.Name;

        //        List<long> HistoryRecordList = new List<long>();
        //        List<HistoryRecord> historyRecordList = HistoryRecord.FindBySql("select * from historyrecord where   samplename like '" + currSpecList.SampleName + "%' order by caculateTime desc  LIMIT 1 ");
        //        if (historyRecordList.Count == 0) return;
        //        HistoryRecordList.Add(historyRecordList[0].Id);

        //        SetreportWorker.SetReport(false, HistoryRecordList, TempletFileName, FrmLogon.userName, ReportPath, reportName,
        //            isReportEXCEL, isReportPDF, isReportXML, isContentXML, 0);
        //    }
        //    else
        //    {
        //        string[] sSpecList = sSpecListid.Split(',');
        //        List<long> HistoryRecordList = new List<long>();
        //        //for (int i = 0; i < sSpecList.Length - 1; i++) HistoryRecordList.Add(long.Parse(sSpecList[i]));
        //        for (int i = 0; i < sSpecList.Length; i++) HistoryRecordList.Add(long.Parse(sSpecList[i]));
        //        GetHistoryRecordReport(HistoryRecordList, 0, true, true);

        //    }
        //}


        #endregion
        /// <summary>
        /// 历史记录中 印度版 按模板保存
        /// </summary>
        /// <param name="HistoryRecordList"></param>
        /// <param name="dWeight"></param>
        /// <param name="isCurrEXCEL"></param>
        /// <param name="isCurrDPF"></param>
        /// <returns></returns>
        public string GetHistoryRecordReport(List<long> HistoryRecordList,double dWeight,bool isCurrEXCEL,bool isCurrDPF)
        {
            //获取模板名称
            string TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ((HistoryRecordList.Count>1)?ExcelTemplateParams.ManyTimeTemplate: ExcelTemplateParams.OneTimeTemplate);
            string UserName = FrmLogon.userName;
            XmlNode LoadSpecifiedNode = ReportTemplateHelper.LoadSpecifiedNode("Report", "IsAutoSaveReport");

            foreach (XmlAttribute xmlatt in LoadSpecifiedNode.Attributes)
                if (xmlatt.Name == "ReportEXCEL") isReportEXCEL = (xmlatt.Value.ToString() == "1") ? true : false;
                else if (xmlatt.Name == "ReportPDF") isReportPDF = (xmlatt.Value.ToString() == "1") ? true : false;
                else if (xmlatt.Name == "ReportXML") isReportXML = (xmlatt.Value.ToString() == "1") ? true : false;
                else if (xmlatt.Name == "ContentXML") isContentXML = (xmlatt.Value.ToString() == "1") ? true : false;
            string ReportPath = (WorkCurveHelper.ExcelPath.IsNullOrEmpty()) ? Application.StartupPath + "\\Report" : WorkCurveHelper.ExcelPath;

            string sReturnPath = "";
            isReportEXCEL = false;
            isReportPDF = false;
            if (isCurrEXCEL) { isReportEXCEL = true; }
            if (isCurrDPF) { isReportPDF = true; }

            

            string reportName="";
            if (HistoryRecordList.Count == 1)
            {
                //List<SpecList> hisSpecList = SpecList.FindBySql("select * from speclist where id in (select speclistid from   historyrecord where id=" + HistoryRecordList[0] + " )");

                HistoryRecord tt = HistoryRecord.FindById(HistoryRecordList[0]);
                SpecListEntity temp = DataBaseHelper.QueryByEdition(tt.SpecListName,tt.FilePath,tt.EditionType);
                if (temp ==null) return "";
                reportName = temp.Name;
            }
            else
            reportName = "Average";
            //生成各式报告
            SetreportWorker.SetReport(true, HistoryRecordList, TempletFileName, FrmLogon.userName, ReportPath, reportName,
                isReportEXCEL, isReportPDF, isReportXML, isContentXML, dWeight);

            if (isCurrEXCEL) { sReturnPath = ReportPath + "\\" + reportName + ".xls"; } else { sReturnPath = ReportPath + "\\" + reportName + ".pdf"; }

            return sReturnPath;
        }




        public string GetPDFReportJapan(string excelPath,string reportName)   //yuzhao20140219:保存pdf命名与excel一致
        {
            string reportPath = Application.StartupPath + "\\Report\\";
            SetreportWorker.GenerateReportPDF(excelPath, reportPath, reportName);
            return reportPath + reportName;
        }
        
    }


    public class ReportThreadManageNew
    {
        public static Queue<string> lowTask = new Queue<string>();
        private static AutoResetEvent moreWork = new AutoResetEvent(false);

        public static void DoWork()
        {
            System.Windows.Forms.Timer StopRectTimer = new System.Windows.Forms.Timer();
            StopRectTimer.Tick += new EventHandler(timer1_Tick);
            StopRectTimer.Interval = 100;
            StopRectTimer.Enabled = true;

            for (int i = 0; i < reportWorker.Length; i++)
            {
                reportWorker[i] = new ReportWorkerThread();
            }
            
        }

        private static ReportWorkerThread[] reportWorker = new ReportWorkerThread[3];
        private static ReportWorkerThreadMgr reportWorkerThreadMgr = new ReportWorkerThreadMgr(moreWork);
        private static bool isReportEXCEL = false;
        private static bool isReportPDF = false;
        private static bool isReportXML = false;
        private static bool isContentXML = false;
        private static void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

                foreach (ReportWorkerThread worker in reportWorker)
                {
                    if (worker.ReadyForProcessing && lowTask.Count > 0)
                    {
                        // 从队列中获取URL
                        string sSpecListid = (string)lowTask.Dequeue();

                        string[] sSpecList = sSpecListid.Split(',');
                        List<long> HistoryRecordList = new List<long>();
                        for (int i = 0; i < sSpecList.Length; i++) HistoryRecordList.Add(long.Parse(sSpecList[i]));


                        string TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ((HistoryRecordList.Count > 1) ? ExcelTemplateParams.ManyTimeTemplate : ExcelTemplateParams.OneTimeTemplate);
                        string UserName = FrmLogon.userName;
                        XmlNode LoadSpecifiedNode = ReportTemplateHelper.LoadSpecifiedNode("Report", "IsAutoSaveReport");

                        foreach (XmlAttribute xmlatt in LoadSpecifiedNode.Attributes)
                            if (xmlatt.Name == "ReportEXCEL") isReportEXCEL = (xmlatt.Value.ToString() == "1") ? true : false;
                            else if (xmlatt.Name == "ReportPDF") isReportPDF = (xmlatt.Value.ToString() == "1") ? true : false;
                            else if (xmlatt.Name == "ReportXML") isReportXML = (xmlatt.Value.ToString() == "1") ? true : false;
                            else if (xmlatt.Name == "ContentXML") isContentXML = (xmlatt.Value.ToString() == "1") ? true : false;
                        string ReportPath = (WorkCurveHelper.ExcelPath.IsNullOrEmpty()) ? Application.StartupPath + "\\Report" : WorkCurveHelper.ExcelPath;

                        string reportName = "";
                        if (HistoryRecordList.Count == 1)
                        {
                            HistoryRecord tt = HistoryRecord.FindById(HistoryRecordList[0]);
                            SpecListEntity temp = DataBaseHelper.QueryByEdition(tt.SpecListName, tt.FilePath, tt.EditionType);
                            if (temp == null) continue;
                            reportName = temp.Name;
                        }
                        else
                            reportName = "Average";


                        reportWorkerThreadMgr.StartWorker(worker, true, HistoryRecordList, TempletFileName, FrmLogon.userName, ReportPath, reportName,
                isReportEXCEL, isReportPDF, isReportXML, isContentXML, 0);
                        moreWork.Reset();
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // 线程被放弃
            }
        }

        
    }
}
