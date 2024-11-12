using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDX.Common.ReportHelper;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Skyray.EDX.Common;
using System.Data;
using Skyray.EDXRFLibrary.Spectrum;
using System.Runtime.CompilerServices;

namespace Skyray.Print.ReportThreadManage
{
    public class ReportWorker
    {
        
        private long historyRecordID;
        private bool isReportEXCEL = false;
        private bool isReportPDF = false;
        private bool isReportXML = false;
        private bool isContentXML = false;
        private string reportName = "";
        private string templetFileName = "";
        private string userName = "";
        private string reportPath = "";
        public ReportWorker()
        {

        }
        /// <summary>
        /// 设置模板生成 方式
        /// </summary>
        /// <param name="isShow"></param>
        /// <param name="seleHistoryRecordList"></param>
        /// <param name="templetFileName"></param>
        /// <param name="sUserName"></param>
        /// <param name="sReportPath"></param>
        /// <param name="reportName"></param>
        /// <param name="isReportEXCEL"></param>
        /// <param name="isReportPDF"></param>
        /// <param name="isReportXML"></param>
        /// <param name="isContentXML"></param>
        /// <param name="dWeight"></param>
        public void SetReport(bool isShow,List<long> seleHistoryRecordList, string templetFileName,
            string sUserName, string sReportPath, string reportName, bool isReportEXCEL, bool isReportPDF, bool isReportXML, bool isContentXML
            , double dWeight)
        {
            lock (thisSection)
            {
                if (isShow)
                    SetShowReport(seleHistoryRecordList, templetFileName,
                sUserName, sReportPath, reportName, isReportEXCEL, isReportPDF, isReportXML, isContentXML
                , dWeight);
                else
                    SetSingleReport(seleHistoryRecordList[0], templetFileName,
                sUserName, sReportPath, reportName, isReportEXCEL, isReportPDF, isReportXML, isContentXML);
            }
        }

        public void GenerateReportPDF(string ExcelReportPath, string currReportPath, string currReportName)
        {
            GenerateReportPDF(ExcelReportPath, currReportPath, currReportName, false);
        }

        /// <summary>
        /// 根据传输 参数 生成单个文件
        /// </summary>
        /// <param name="historyRecordID"></param>
        /// <param name="templetFileName"></param>
        /// <param name="userName"></param>
        /// <param name="reportPath"></param>
        /// <param name="reportName"></param>
        /// <param name="isReportEXCEL"></param>
        /// <param name="isReportPDF"></param>
        /// <param name="isReportXML"></param>
        /// <param name="isContentXML"></param>
        private void SetSingleReport(long historyRecordID, string templetFileName,
            string userName,string reportPath,string reportName, bool isReportEXCEL, bool isReportPDF, bool isReportXML, bool isContentXML)
        {
            
            this.historyRecordID = historyRecordID;
            this.templetFileName = templetFileName;
            this.userName = userName;
            this.reportName = reportName;
            this.reportPath = reportPath;
            this.isReportEXCEL = isReportEXCEL;
            this.isReportPDF = isReportPDF;
            this.isReportXML = isReportXML;
            this.isContentXML = isContentXML;
           
            string sExcelPath="";
            if (isReportEXCEL || isReportPDF)
            GenerateReportEXCEL(ref sExcelPath);
            if (isReportPDF) GenerateReportPDF(sExcelPath, reportPath, reportName, isReportEXCEL);
            if (isReportXML) GenerateReportXML(historyRecordID, reportPath);
            if (isContentXML) GenerateContentXML(historyRecordID, reportPath);
        }


        private object thisSection = new object();
        private void GenerateReportEXCEL(ref string sExcelPath)
        {
            Report report = new Report();
            

            report.IsAnalyser = true;
            report.dWeight = 0;
            List<HistoryRecord> historyRecordList = new List<HistoryRecord>();
            historyRecordList.AddRange(HistoryRecord.FindBySql("select * from historyrecord where id=" + historyRecordID));

            SpecListEntity hisSpecList = DataBaseHelper.QueryByEdition(historyRecordList[0].SpecListName, historyRecordList[0].FilePath, historyRecordList[0].EditionType);
            WorkCurve hisWorkCurve = WorkCurve.FindById(historyRecordList[0].WorkCurveId);

            if (historyRecordList != null && historyRecordList.Count > 0)
            {
                report.historyRecordid = historyRecordList[0].Id.ToString();
                report.ReadingNo = historyRecordList[0].HistoryRecordCode;
            }

            report.TempletFileName = templetFileName;
            report.Spec = hisSpecList.Specs[0];
            report.InterestElemCount = hisWorkCurve.ElementList.Items.ToList().FindAll(w => w.IsShowElement).Count;


            List<HistoryElement> hisElementList = HistoryElement.FindBySql("select * from historyelement where historyrecord_id=" + historyRecordList[0].Id);
            foreach (CurveElement cuelement in hisWorkCurve.ElementList.Items)
            {
                HistoryElement hisElement = hisElementList.Find(delegate(HistoryElement v) { return v.elementName == cuelement.Caption; });
                if (hisElement != null)
                    cuelement.Content = double.Parse(hisElement.contextelementValue);
            }


            report.Elements = hisWorkCurve.ElementList;
            report.ReportPath = reportPath;
            report.WorkCurveID = hisWorkCurve.Id.ToString();

            report.WorkCurveName = hisWorkCurve.Name;
            report.operateMember = userName;

            //sExcelPath=report.GenerateReportByPDF(reportName, true, ref reportPath, historyRecordList);
            sExcelPath = report.GenerateRetestReport(reportName, true, historyRecordList);
           
        }

        private void GenerateReportPDF(string ExcelReportPath, string currReportPath, string currReportName, bool isCurrReportEXCEL)
        {
            #region ReportPDF
            if (WorkCurveHelper.ReportVkOrVe == 1)
            {
                ET.Application app =new ET.Application();
                ET.Workbooks wbs = app.Workbooks;

                ET._Workbook wb = wbs.Add(ExcelReportPath);

                ET.Worksheet ws = (ET.Worksheet)wb.Worksheets.get_Item(1);
                ws.UsedRange.Select();
                object missing = System.Reflection.Missing.Value;
                ws.UsedRange.Copy(missing);


                IDataObject iData = Clipboard.GetDataObject();
                Bitmap bits = (Bitmap)iData.GetData(DataFormats.Bitmap);
                Bitmap myBitmap = new Bitmap(bits.Width, bits.Height);
                Graphics g = Graphics.FromImage(myBitmap);
                g.DrawImage(bits, 1, 1);
                //myBitmap.Save(file_gif, System.Drawing.Imaging.ImageFormat.Gif);

                List<Bitmap> bitmaplist = new List<Bitmap>();
                bitmaplist.Add(bits);

                //string SaveReportPath = "";
                //string PDFPath = "";
                //string PDFName = "";

                try
                {
                    PDFExporter exporter = new PDFExporter(bitmaplist);

                    string SaveReportPath = currReportPath + "\\" + currReportName;
                    if (!Directory.Exists(currReportPath))
                        Directory.CreateDirectory(currReportPath);

                    exporter.SavePDF(SaveReportPath);

                    if (!isCurrReportEXCEL)
                    {
                        //删除excel文件
                        FileInfo file = new FileInfo(ExcelReportPath);
                        file.Delete();
                    }
                    app.CutCopyMode = ET.ETCutCopyMode.etCut;// Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCut;
                    myBitmap.Dispose();
                    bits.Dispose();
                    wbs.Close(false);



                }
                catch //(System.Exception ex)
                {
                    //SkyrayMsgBox.Show(ex.Message);
                }
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application app =
                new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbooks wbs = app.Workbooks;

                Microsoft.Office.Interop.Excel.Workbook wb = wbs.Add(ExcelReportPath);

                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets.get_Item(1);
                ws.UsedRange.Select();
                object missing = System.Reflection.Missing.Value;
                ws.UsedRange.Copy(missing);

                IDataObject iData = Clipboard.GetDataObject();
                Bitmap bits = (Bitmap)iData.GetData(DataFormats.Bitmap);
                Bitmap myBitmap = new Bitmap(bits.Width, bits.Height);
                Graphics g = Graphics.FromImage(myBitmap);
                g.DrawImage(bits, 1, 1);
                //myBitmap.Save(file_gif, System.Drawing.Imaging.ImageFormat.Gif);

                List<Bitmap> bitmaplist = new List<Bitmap>();
                bitmaplist.Add(bits);

                //string SaveReportPath = "";
                //string PDFPath = "";
                //string PDFName = "";

                try
                {
                    PDFExporter exporter = new PDFExporter(bitmaplist);

                    string SaveReportPath = currReportPath + "\\" + currReportName + ".pdf";
                    if (!Directory.Exists(currReportPath))
                        Directory.CreateDirectory(currReportPath);

                    exporter.SavePDF(SaveReportPath);

                    if (!isCurrReportEXCEL)
                    {
                        //删除excel文件
                        FileInfo file = new FileInfo(ExcelReportPath);
                        file.Delete();
                    }
                    app.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCut;
                    myBitmap.Dispose();
                    bits.Dispose();
                    wbs.Close();



                }
                catch //(System.Exception ex)
                {
                    //SkyrayMsgBox.Show(ex.Message);
                }
            }

            #endregion
        }

        static object lockObject1 = new object();
        static object lockObject2 = new object();
        private void GenerateReportXML(long currHistoryRecordID, string currreportPath)
        {
            string sReportXMLName = "ReportXML.xml";
            //if(System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(10000);
            string xmlContent = "";
            HistoryRecord tt = HistoryRecord.FindById(currHistoryRecordID);
            SpecListEntity temp = DataBaseHelper.QueryByEdition(tt.SpecListName,tt.FilePath,tt.EditionType);
            //List<SpecListEntity> hisSpecList = SpecList.FindBySql("select * from speclist where id in (select speclistid from   historyrecord where id=" + currHistoryRecordID + " )");
            List<WorkCurve> hisWorkCurveList = WorkCurve.FindBySql("select * from workcurve where id in (select workcurveid from   historyrecord where id=" + currHistoryRecordID + " )");


            xmlContent += "<Reoprt>";
            //if(System.Diagnostics.Debugger.IsAttached)
            //    xmlContent+="<ThreadNO>"+System.Threading.Thread.CurrentThread.Name+"</ThreadNO>";
            xmlContent += "<SampleName>" + temp.Name + "</SampleName>";
            xmlContent += "<WorkCurve>" + hisWorkCurveList[0].Name + "</WorkCurve>";
            xmlContent += "<TestDate>" + temp.SpecDate + "</TestDate>";
            xmlContent += "<Description>" + temp.SpecSummary + "</Description>";
            xmlContent += "<Element>";

            List<HistoryElement> HistoryElementList = HistoryElement.FindBySql("select * from historyelement where historyrecord_id=" + currHistoryRecordID);
            foreach (HistoryElement curveelement in HistoryElementList)
            {
                Atom atomn = Atoms.AtomList.Find(s => s.AtomName == curveelement.elementName);
                string atomFullName = (atomn == null) ? "" : atomn.AtomNameEN;
                xmlContent += "<" + atomFullName + ">" + double.Parse(curveelement.contextelementValue).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "</" + atomFullName + ">";
            }
            xmlContent += "</Element>";
            xmlContent += "</Reoprt>";

            if (!Directory.Exists(currreportPath))
                Directory.CreateDirectory(currreportPath);

            string ReportXML = currreportPath + "\\" + sReportXMLName;
            lock (lockObject1)
            {
                FileInfo file = new FileInfo(ReportXML);
                if (file.Exists)
                {
                    StreamReader myreader = File.OpenText(ReportXML);
                    string s = "";
                    s = myreader.ReadToEnd();
                    s = s.Replace("<Data>", "<Data>" + xmlContent);
                    myreader.Close();
                    StreamWriter sw = new StreamWriter(ReportXML, false);
                    sw.Write(s);
                    sw.Flush();
                    sw.Close();


                }
                else
                {
                    xmlContent = "<?xml version='1.0' encoding='utf-8'?><Data>" + xmlContent + "</Data>";
                    StreamWriter sw = new StreamWriter(ReportXML, true);
                    sw.Write(xmlContent);
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        private void GenerateContentXML(long currHistoryRecordID, string currreportPath)
        {
            string sReportXMLName = "ReportContentXML.xml";
            //if(System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(10000);
            string xmlContent = "";
            HistoryRecord tt = HistoryRecord.FindById(currHistoryRecordID);
            SpecListEntity temp = DataBaseHelper.QueryByEdition(tt.SpecListName,tt.FilePath,tt.EditionType);
            //List<SpecList> hisSpecList = SpecList.FindBySql("select * from speclist where id in (select speclistid from   historyrecord where id=" + currHistoryRecordID + " )");
            List<WorkCurve> hisWorkCurveList = WorkCurve.FindBySql("select * from workcurve where id in (select workcurveid from   historyrecord where id=" + currHistoryRecordID + " )");


            xmlContent += "<?xml version='1.0' encoding='utf-8'?><Data>";
            xmlContent += "<SampleName>" + temp.Name + "</SampleName>";
            xmlContent += "<WorkCurve>" + hisWorkCurveList[0].Name + "</WorkCurve>";
            xmlContent += "<TestDate>" + temp.SpecDate + "</TestDate>";
            xmlContent += "<Element>";

            List<HistoryElement> HistoryElementList = HistoryElement.FindBySql("select * from historyelement where historyrecord_id=" + currHistoryRecordID);
            foreach (HistoryElement curveelement in HistoryElementList)
            {
                Atom atomn = Atoms.AtomList.Find(s => s.AtomName == curveelement.elementName);
                string atomFullName = (atomn == null) ? "" : atomn.AtomNameEN;
                xmlContent += "<" + atomFullName + ">" + double.Parse(curveelement.contextelementValue).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "</" + atomFullName + ">";
            }
            xmlContent += "</Element>";

            if (!Directory.Exists(currreportPath))
                Directory.CreateDirectory(currreportPath);

            string ContentXML = currreportPath + "\\" + sReportXMLName;
            lock (lockObject2)
            {
                FileInfo file = new FileInfo(ContentXML);
                if (file.Exists)
                    file.Delete();


                xmlContent += "</Data>";
                StreamWriter sw = new StreamWriter(ContentXML, true);
                sw.Write(xmlContent);
                sw.Flush();
                sw.Close();
            }
        }



        #region 点击获取报表

        /// <summary>
        /// 生成Excel或PDF自动生成单次或多次Xml
        /// </summary>
        /// <param name="seleHistoryRecordList"></param>
        /// <param name="templetFileName"></param>
        /// <param name="sUserName"></param>
        /// <param name="sReportPath"></param>
        /// <param name="reportName"></param>
        /// <param name="isReportEXCEL"></param>
        /// <param name="isReportPDF"></param>
        /// <param name="isReportXML"></param>
        /// <param name="isContentXML"></param>
        /// <param name="dWeight"></param>
        private void SetShowReport(List<long> seleHistoryRecordList, string templetFileName,
            string sUserName, string sReportPath, string reportName, bool isReportEXCEL, bool isReportPDF, bool isReportXML, bool isContentXML
            , double dWeight)
        {
            string sExcelPath = "";
            if (isReportEXCEL || isReportPDF)
                sExcelPath=GetExcelPDFByHistoryRecord(seleHistoryRecordList, templetFileName, sUserName, sReportPath, reportName, dWeight);
            if (isReportPDF ) GenerateReportPDF(sExcelPath, sReportPath, reportName, isReportEXCEL);

            if (seleHistoryRecordList.Count > 1) SetMuialHistoryRecordXml(sReportPath, seleHistoryRecordList);
            else
            {
                GenerateReportXML(seleHistoryRecordList[0], sReportPath);
                GenerateContentXML(seleHistoryRecordList[0], sReportPath);
            }



        }

        private string GetExcelPDFByHistoryRecord(List<long> seleHistoryRecordList, string templetFileName,
            string sUserName, string sReportPath, string reportName
            , double dWeight)
        {
            #region //Old
            //string ExcelReportPath = "";
            //ExcelTemplateParams.GetExcelTemplateParams();
            //if (seleHistoryRecordList != null && seleHistoryRecordList.Count > 0)
            //{
            //    Report report = new Report();

            //    report.IsAnalyser = true;
            //    report.dWeight = dWeight;

            //    string sHisRecordID = "";
            //    foreach (long his in seleHistoryRecordList) sHisRecordID += his + ",";
            //    if (sHisRecordID != "") sHisRecordID = sHisRecordID.Substring(0, sHisRecordID.Length - 1);
            //    List<HistoryRecord> HistoryRecordlist = HistoryRecord.FindBySql("select * from historyrecord where id in (" + sHisRecordID + ")");

            //    List<HistoryRecord> historyRecordList = HistoryRecordlist;

            //    SpecList hisSpecList = SpecList.FindById(historyRecordList[0].SpecListId);
            //    WorkCurve hisWorkCurve = WorkCurve.FindById(historyRecordList[0].WorkCurveId);

            //    if (historyRecordList != null && historyRecordList.Count > 0)
            //    {
            //        report.historyRecordid = historyRecordList[0].Id.ToString();
            //        report.ReadingNo = historyRecordList[0].HistoryRecordCode;
            //    }


            //    report.TempletFileName = templetFileName;


            //    //List<SpecList> reportSpecList = (speclistid == "") ? null : SpecList.FindBySql("select * from speclist where id=" + speclistid);

            //    report.Spec = hisSpecList.Specs[0];
            //    report.InterestElemCount = hisWorkCurve.ElementList.Items.Count;


            //    List<HistoryElement> hisElementList = HistoryElement.FindBySql("select * from historyelement where historyrecord_id=" + historyRecordList[0].Id);
            //    foreach (CurveElement cuelement in hisWorkCurve.ElementList.Items)
            //    {
            //        HistoryElement hisElement = hisElementList.Find(delegate(HistoryElement v) { return v.elementName == cuelement.Caption; });
            //        if (hisElement != null)
            //            cuelement.Content = double.Parse(hisElement.contextelementValue);
            //    }


            //    report.Elements = hisWorkCurve.ElementList;


            //    report.ReportPath = WorkCurveHelper.ExcelPath;
            //    report.WorkCurveID = hisWorkCurve.Id.ToString();

            //    if (WorkCurveHelper.CurrentStandard != null)
            //        report.historyStandID = WorkCurveHelper.CurrentStandard.Id;
            //    else
            //        report.historyStandID = -1;
            //    report.WorkCurveName = hisWorkCurve.Name;
            //    report.operateMember = sUserName;

            //    ExcelReportPath = report.GenerateReportByPDF(reportName, true, ref sReportPath, historyRecordList);
            //}

            //return ExcelReportPath;
            #endregion
            string ExcelReportPath = "";
            ExcelTemplateParams.GetExcelTemplateParams();
            if (seleHistoryRecordList != null && seleHistoryRecordList.Count > 0)
            {
                Report report = new Report();
                report.IsAnalyser = true;
                //report.dWeight = dWeight;

                string sHisRecordID = "";
                foreach (long his in seleHistoryRecordList) sHisRecordID += his + ",";
                if (sHisRecordID != "") sHisRecordID = sHisRecordID.Substring(0, sHisRecordID.Length - 1);
                List<HistoryRecord> HistoryRecordlist = HistoryRecord.FindBySql("select * from historyrecord where id in (" + sHisRecordID + ")");
                HistoryRecordlist = HistoryRecordlist.OrderBy(seleHistoryRecordList, (w) => { return HistoryRecordlist.First(n => n.Id == w); });
                //HistoryRecordlist =(List<HistoryRecord>)HistoryRecordlist.ObjectOrderBy(seleHistoryRecordList, (long w) => { return HistoryRecordlist.First(n => n.Id == w); });                

                List<HistoryRecord> historyRecordList = HistoryRecordlist;
                report.dWeight = historyRecordList.Count > 0 ? historyRecordList[0].Weight : 0;
                SpecListEntity hisSpecList = DataBaseHelper.QueryByEdition(historyRecordList[0].SpecListName, historyRecordList[0].FilePath, historyRecordList[0].EditionType);
                WorkCurve hisWorkCurve = WorkCurve.FindById(historyRecordList[0].WorkCurveId);//历史记录工作曲线 但并不能代表历史可能在历史记录保存之后修改

                if (historyRecordList != null && historyRecordList.Count > 0)
                {
                    report.historyRecordid = historyRecordList[0].Id.ToString();
                    report.ReadingNo = historyRecordList[0].HistoryRecordCode;
                }
                report.TempletFileName = templetFileName;


                //List<SpecList> reportSpecList = (speclistid == "") ? null : SpecList.FindBySql("select * from speclist where id=" + speclistid);

                report.Spec = hisSpecList.Specs[0];
                report.specList = hisSpecList;

                //report.InterestElemCount = hisWorkCurve.ElementList.Items.Count;//历史记录工作曲线的感兴趣元素不一定是当时历史记录生成时的元素 可能后续改动

                List<HistoryElement> hisElementList = HistoryElement.FindBySql("select * from historyelement where historyrecord_id=" + historyRecordList[0].Id);
                report.InterestElemCount = hisElementList.Count;

                //foreach (CurveElement cuelement in hisWorkCurve.ElementList.Items)
                //{
                //    HistoryElement hisElement = hisElementList.Find(delegate(HistoryElement v) { return v.elementName == cuelement.Caption; });
                //    if (hisElement != null)
                //        cuelement.Content = double.Parse(hisElement.contextelementValue);
                //}
                ElementList elementList = ElementList.New;

                //foreach(HistoryElement hisEle in hisElementList)
                //{
                //    CurveElement curEle = CurveElement.New;
                //    curEle.Caption = hisEle.elementName;
                //    double dValue = 0;
                //    double.TryParse(hisEle.contextelementValue,out dValue);
                //    curEle.Content = dValue;
                //    curEle.Intensity = hisEle.CaculateIntensity;
                //    elementList.Items.Add(curEle);
                //}
                //report.Elements = elementList;

                //var workEles = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList();//当前感兴趣元素副本
                //var hisEles = hisElementList;//历史记录副本
                //report.Elements = hisWorkCurve.ElementList;//初始化 报告感兴趣元素列表
                //report.Elements.Items.Clear();//清空所有历史记录元素信息 未排序
                //var eleOrder = from w in workEles
                //               join h in hisEles
                //               on w.Caption equals h.elementName
                //               orderby w.RowsIndex
                //               select w;//排序感兴趣元素 并获取历史记录元素值
                //var eleLeft = hisEles.Except(eleOrder); //历史记录中 不在 感兴趣元素中的元素信息
                //eleOrder.Concat(eleLeft);//合并 历史记录中剩余部分元素信息
                //eleOrder.All(w => { report.Elements.Items.Add(w); return true; });//添加所有排序的感兴趣元素和 历史记录信息
                //report.Elements = hisWorkCurve.ElementList;
                var workEles = WorkCurve.FindById(hisElementList.FirstOrDefault().HistoryRecord.WorkCurveId)
                    .ElementList.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex).ToList();
                foreach (var ele in workEles)
                {
                    HistoryElement hisEle = hisElementList.Find(w => w.elementName == ele.Caption);
                    if (hisEle != null)
                        elementList.Items.Add(hisEle.ToCurveElement());
                }
                foreach (HistoryElement ele in hisElementList)
                {
                    CurveElement curEle = elementList == null ? null : elementList.Items.First(w => w.Caption == ele.elementName);
                    if (curEle == null)
                        elementList.Items.Add(ele.ToCurveElement());
                }
                ElementList lstTemp=WorkCurve.FindById(hisElementList.FirstOrDefault().HistoryRecord.WorkCurveId).ElementList;
                elementList.RhIsLayer = lstTemp.RhIsLayer;
                elementList.LayerElemsInAnalyzer = lstTemp.LayerElemsInAnalyzer == null ? "" : lstTemp.LayerElemsInAnalyzer;
                report.Elements = elementList;
                report.ReportPath = WorkCurveHelper.ExcelPath;
                report.WorkCurveID = hisWorkCurve.Id.ToString();

                if (WorkCurveHelper.CurrentStandard != null)
                    report.historyStandID = WorkCurveHelper.CurrentStandard.Id;
                else
                    report.historyStandID = -1;
                report.WorkCurveName = hisWorkCurve.Name;
                report.operateMember = sUserName;
                report.RetestFileName = templetFileName;
                report.selectLong = seleHistoryRecordList;

                //ExcelReportPath = report.GenerateReportByPDF(reportName, true, ref sReportPath, historyRecordList);
                //ExcelReportPath = report.GenerateRetestReport(reportName, true, historyRecordList);
                ExcelReportPath = report.GenerateRetestReport(reportName, true, seleHistoryRecordList);


            }

            return ExcelReportPath;
        }






        private void SetMuialHistoryRecordXml(string sReportPath, List<long> selectLong)
        {
            #region
            //添加多次测量后的平均值到xml文件中
            string sReportXMLPath = sReportPath + "\\ReportContentXML.xml";


            string hisId = "";
            foreach (long hisRecord in selectLong)
            {
                hisId += hisRecord.ToString() + ",";
            }
            if (hisId != "") hisId = hisId.Substring(0, hisId.Length - 1);
            if (hisId == "") return;

            string xmlContent = "";
            string xmlContentTop = "";

            double dTotalContent = 0;

            HistoryRecord HisRecord = HistoryRecord.FindById(selectLong[0]);

            List<WorkCurve> currWorkCurveList = WorkCurve.FindBySql("select * from   workcurve where id=" + HisRecord.WorkCurveId);

            xmlContent += "<?xml version='1.0' encoding='utf-8'?><Data>";
            xmlContent += "<SampleName>Average</SampleName>";
            xmlContent += "<WorkCurve>" + ((currWorkCurveList.Count > 0) ? currWorkCurveList[0].Name : "") + "</WorkCurve>";
            xmlContent += "<TestDate>" + HisRecord.SpecDate + "</TestDate>";
            xmlContent += "<Element>";





            xmlContentTop += "<Reoprt>";
            xmlContentTop += "<SampleName>Average</SampleName>";
            xmlContentTop += "<WorkCurve>" + ((currWorkCurveList.Count > 0) ? currWorkCurveList[0].Name : "") + "</WorkCurve>";
            xmlContentTop += "<TestDate>" + HisRecord.SpecDate + "</TestDate>";
            xmlContentTop += "<Description></Description>";
            xmlContentTop += "<Element>";
            DataSet ds = Lephone.Data.DbEntry.Context.ExecuteDataset("select elementname from historyelement where historyrecord_id in (" + hisId + ") group by elementname");

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Atom atomn = Atoms.AtomList.Find(s => s.AtomName == row[0].ToString());
                string atomFullName = (atomn == null) ? "" : atomn.AtomNameEN;

                dTotalContent = 0;
                List<HistoryElement> HistoryElementList = HistoryElement.FindBySql("select * from historyelement where historyrecord_id in (" + hisId + ")  and elementname='" + row[0].ToString() + "'");
                foreach (HistoryElement HisElement in HistoryElementList)
                    dTotalContent += double.Parse(double.Parse(HisElement.contextelementValue).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()));
                double value = dTotalContent / selectLong.Count;
                xmlContent += "<" + atomFullName + ">" + value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "</" + atomFullName + ">";
                xmlContentTop += "<" + atomFullName + ">" + value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "</" + atomFullName + ">";
            }
            xmlContent += "</Element>";
            xmlContent += "</Data>";
            xmlContentTop += "</Element>";
            xmlContentTop += "</Reoprt>";

            FileInfo file = new FileInfo(sReportXMLPath);
            if (file.Exists)
                file.Delete();
            StreamWriter sw = new StreamWriter(sReportXMLPath, true);
            sw.Write(xmlContent);
            sw.Flush();
            sw.Close();


            sReportXMLPath = sReportPath + "\\ReportXML.xml";
            FileInfo file1 = new FileInfo(sReportXMLPath);
            if (file1.Exists)
            {
                StreamReader myreader = File.OpenText(sReportXMLPath);
                string s = "";
                s = myreader.ReadToEnd();
                s = s.Replace("<Data>", "<Data>" + xmlContentTop);
                myreader.Close();
                StreamWriter sw1 = new StreamWriter(sReportXMLPath, false);
                sw1.Write(s);
                sw1.Flush();
                sw1.Close();


            }
            else
            {
                xmlContentTop = "<?xml version='1.0' encoding='utf-8'?><Data>" + xmlContentTop + "</Data>";
                StreamWriter sw2 = new StreamWriter(sReportXMLPath, true);
                sw2.Write(xmlContent);
                sw2.Flush();
                sw2.Close();
            }

            #endregion
        }

        #endregion


    }
}
