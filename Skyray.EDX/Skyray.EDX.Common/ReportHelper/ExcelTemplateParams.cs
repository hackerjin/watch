using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Skyray.EDXRFLibrary;
using System.Data.SQLite;
using Lephone.Data;

namespace Skyray.EDX.Common.ReportHelper
{
    public class ExcelTemplateParams
    {
        public static bool TemplateType;
        //public static int iTemplateType=0;//模板类型
        public static string OneTimeTemplate;//单条记录模板名称
        public static string ManyTimeTemplate;//多条模板名称
        public static string ManyTimeElementRowMun;//多条模板时，元素显示的开始行数
        public static string ManyTimeElementValueRowMun;//多条模板时，元素值显示的开始行数
        public static string TotalColumnMun;//excel总列数
        static FileSystemWatcher _FileSystemWatcher;
        //public static bool IsCurrentUsed;

        #region 横排模板信息
        public static string HorizontalManyTimeElementRowMun;//横排多条模板时横排时元素显示的开始行数
        public static string HorizontalManyTimeElementValueRowMun;
        public static string HorizontalTotalColumnMun;//横排总列数
        #endregion

        #region 判定结果值
        public static string PassResults;
        public static string FalseResults;
        public static string STDResults;
        #endregion

        static ExcelTemplateParams()
        {
            _FileSystemWatcher = new FileSystemWatcher();
            _FileSystemWatcher.Path = AppDomain.CurrentDomain.BaseDirectory;// "AppParams.xml";
            _FileSystemWatcher.IncludeSubdirectories = true;
            _FileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite|NotifyFilters.LastAccess|
                NotifyFilters.FileName|NotifyFilters.DirectoryName;
            _FileSystemWatcher.Filter = "*.xml";
            _FileSystemWatcher.Changed += new FileSystemEventHandler(_FileSystemWatcher_Changed);
            _FileSystemWatcher.Deleted += new FileSystemEventHandler(_FileSystemWatcher_Changed);
            _FileSystemWatcher.EnableRaisingEvents = true;
        }

        static void _FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (ReportTemplateHelper.IsCurrentUsed)
            {
                FileInfo file = new FileInfo(e.FullPath);
                System.Threading.Thread.Sleep(100);
                if (e.Name == "ReportPropertiesUsed.xml")
                    File.Delete(file.DirectoryName.TrimEnd('\\', '/') + "/ReportPropertiesAll.xml");
            }
        }

        public static void LoadExcelTemplateParams(string fileName)
        {
            #region  四合一
            //WorkCurveHelper.MeasureTimeType = Skyray.EDX.Common.UIHelper.XmlHelper.GetMeasureTimeType();//获取或更新测量时间模式
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(fileName);
            //OneTimeTemplate = "";
            //ManyTimeTemplate = "";
            //if (ReportTemplateHelper.ExcelModeType == 2 || ReportTemplateHelper.ExcelModeType == 0)
            //{
            //    return;
            //}
            //string sReportPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml//CompanyInfo.xml";
            //XmlDocument xmlDocReport = new XmlDocument();
            //xmlDocReport.Load(sReportPath);
            //string strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.ExcelModeType + "']";
            //XmlNodeList xmlnodelist = xmlDocReport.SelectNodes(strWhere);
            //if (xmlnodelist == null || xmlnodelist.Count == 0) return;

            //OneTimeTemplate = xmlnodelist[0].Attributes["OneTimeTemplate"].Value;
            //ManyTimeTemplate = xmlnodelist[0].Attributes["ManyTimeTemplate"].Value;
            //ManyTimeElementRowMun = xmlnodelist[0].Attributes["ElementRowMun"].Value;
            //ManyTimeElementValueRowMun = xmlnodelist[0].Attributes["ElementValueRowMun"].Value;
            //TotalColumnMun = xmlnodelist[0].Attributes["TotalColumnMun"].Value;
            //try
            //{
            //    HorizontalManyTimeElementRowMun = xmlnodelist[0].Attributes["HorizontalManyTimeElementRowMun"].Value;
            //    HorizontalManyTimeElementValueRowMun = xmlnodelist[0].Attributes["HorizontalManyTimeElementValueRowMun"].Value;
            //    HorizontalTotalColumnMun = xmlnodelist[0].Attributes["HorizontalTotalColumnMun"].Value;
            //}
            //catch
            //{
            //    foreach (XmlNode node in xmlDocReport.ChildNodes)
            //    {
            //        try
            //        {
            //            if (node.Name == "Data")
            //            {
            //                foreach (XmlNode secondNode in node.ChildNodes)
            //                {
            //                    XmlAttribute attHorizontalManyTimeElementRowMun = xmlDocReport.CreateAttribute("HorizontalManyTimeElementRowMun");
            //                    attHorizontalManyTimeElementRowMun.Value = "B8";
            //                    if (secondNode.Attributes["Name"].Value.ToString() == "5")
            //                        attHorizontalManyTimeElementRowMun.Value = "B18";
            //                    secondNode.Attributes.SetNamedItem(attHorizontalManyTimeElementRowMun);
            //                    XmlAttribute attHorizontalManyTimeElementValueRowMun = xmlDocReport.CreateAttribute("HorizontalManyTimeElementValueRowMun");
            //                    attHorizontalManyTimeElementValueRowMun.Value = "9";
            //                    if (secondNode.Attributes["Name"].Value.ToString() == "5")
            //                        attHorizontalManyTimeElementValueRowMun.Value = "19";
            //                    secondNode.Attributes.SetNamedItem(attHorizontalManyTimeElementValueRowMun);
            //                    XmlAttribute attHorizontalTotalColumnMun = xmlDocReport.CreateAttribute("HorizontalTotalColumnMun");
            //                    attHorizontalTotalColumnMun.Value = "10";
            //                    if (secondNode.Attributes["Name"].Value.ToString() == "5")
            //                        attHorizontalTotalColumnMun.Value = "12";
            //                    secondNode.Attributes.SetNamedItem(attHorizontalTotalColumnMun);
            //                }
            //            }
            //        }
            //        catch
            //        { }
            //    }
            //    xmlDocReport.Save(sReportPath);
            //}


            //xmlDoc = null;
            //xmlDocReport = null;
            ////GC.Collect();
            #endregion

            #region  thick

            WorkCurveHelper.MeasureTimeType = Skyray.EDX.Common.UIHelper.MeasureTimeType.Normal;//获取或更新测量时间模式
            string sReportPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml//CompanyInfo.xml";
            XmlDocument xmlDocReport = new XmlDocument();
            xmlDocReport.Load(sReportPath);
            string strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.ExcelModeType + "']";
            XmlNodeList xmlnodelist = xmlDocReport.SelectNodes(strWhere);
            if (xmlnodelist == null || xmlnodelist.Count == 0) return;

            OneTimeTemplate = xmlnodelist[0].Attributes["OneTimeTemplate"].Value;
            ManyTimeTemplate = xmlnodelist[0].Attributes["ManyTimeTemplate"].Value;
            ManyTimeElementRowMun = xmlnodelist[0].Attributes["ElementRowMun"].Value;
            ManyTimeElementValueRowMun = xmlnodelist[0].Attributes["ElementValueRowMun"].Value;
            TotalColumnMun = xmlnodelist[0].Attributes["TotalColumnMun"].Value;
            try
            {
                HorizontalManyTimeElementRowMun = xmlnodelist[0].Attributes["HorizontalManyTimeElementRowMun"].Value;
                HorizontalManyTimeElementValueRowMun = xmlnodelist[0].Attributes["HorizontalManyTimeElementValueRowMun"].Value;
                HorizontalTotalColumnMun = xmlnodelist[0].Attributes["HorizontalTotalColumnMun"].Value;
            }
            catch
            {
                foreach (XmlNode node in xmlDocReport.ChildNodes)
                {
                    try
                    {
                        if (node.Name == "Data")
                        {
                            foreach (XmlNode secondNode in node.ChildNodes)
                            {
                                XmlAttribute attHorizontalManyTimeElementRowMun = xmlDocReport.CreateAttribute("HorizontalManyTimeElementRowMun");
                                attHorizontalManyTimeElementRowMun.Value = "B8";
                                if (secondNode.Attributes["Name"].Value.ToString() == "5")
                                    attHorizontalManyTimeElementRowMun.Value = "B18";
                                secondNode.Attributes.SetNamedItem(attHorizontalManyTimeElementRowMun);
                                XmlAttribute attHorizontalManyTimeElementValueRowMun = xmlDocReport.CreateAttribute("HorizontalManyTimeElementValueRowMun");
                                attHorizontalManyTimeElementValueRowMun.Value = "9";
                                if (secondNode.Attributes["Name"].Value.ToString() == "5")
                                    attHorizontalManyTimeElementValueRowMun.Value = "19";
                                secondNode.Attributes.SetNamedItem(attHorizontalManyTimeElementValueRowMun);
                                XmlAttribute attHorizontalTotalColumnMun = xmlDocReport.CreateAttribute("HorizontalTotalColumnMun");
                                attHorizontalTotalColumnMun.Value = "10";
                                if (secondNode.Attributes["Name"].Value.ToString() == "5")
                                    attHorizontalTotalColumnMun.Value = "12";
                                secondNode.Attributes.SetNamedItem(attHorizontalTotalColumnMun);
                            }
                        }
                    }
                    catch
                    { }
                }
                xmlDocReport.Save(sReportPath);
            }


           
            xmlDocReport = null;
            #endregion
        }
        
        public static void GetExcelTemplateParams()
        {
            if (System.IO.File.Exists(Application.StartupPath + "\\HistoryExcelTemplate\\" + OneTimeTemplate)
                && System.IO.File.Exists(Application.StartupPath + "\\HistoryExcelTemplate\\" + ManyTimeTemplate))
            {
                if(Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang)//中文
                {
                    int Index = OneTimeTemplate.IndexOf("Model") + ("Model").Length;
                    OneTimeTemplate = OneTimeTemplate.GetLeft(OneTimeTemplate.Length - Index) + "CN.xls";

                    if (ManyTimeTemplate != "")
                    {
                        Index = ManyTimeTemplate.IndexOf("Model") + ("Model").Length;
                        ManyTimeTemplate = ManyTimeTemplate.GetLeft(ManyTimeTemplate.Length - Index) + "CN.xls";
                    }
                }
                else if(Skyray.Language.Lang.Model.CurrentLang.FullName == "English")//英文
                {
                    int Index = OneTimeTemplate.IndexOf("Model") + ("Model").Length;
                    OneTimeTemplate = OneTimeTemplate.GetLeft(OneTimeTemplate.Length - Index) + "EN.xls";
                    if (ManyTimeTemplate != "")
                    {
                        Index = ManyTimeTemplate.IndexOf("Model") + ("Model").Length;
                        ManyTimeTemplate = ManyTimeTemplate.GetLeft(ManyTimeTemplate.Length - Index) + "EN.xls";
                    }
                }
                else//日文
                {
                    int Index = OneTimeTemplate.IndexOf("Model") + ("Model").Length;
                    OneTimeTemplate = OneTimeTemplate.GetLeft(OneTimeTemplate.Length - Index) + "JP.xls";
                    if (ManyTimeTemplate != "")
                    {
                        Index = ManyTimeTemplate.IndexOf("Model") + ("Model").Length;
                        ManyTimeTemplate = ManyTimeTemplate.GetLeft(ManyTimeTemplate.Length - Index) + "JP.xls";
                    }
                }
            }
        }



        public static XmlNodeList GetCompanyOtherInfoTitles()
        {
            XmlNodeList xmlnodelist;
            string sReportPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml//CompanyInfo.xml";
            XmlDocument xmlDocReport = new XmlDocument();
            xmlDocReport.Load(sReportPath);

            string strWhere = "";
            if (ReportTemplateHelper.ExcelModeType != 2)
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.ExcelModeType + "']";
            else
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.LoadReportName() + "']";


            xmlnodelist = xmlDocReport.SelectNodes(strWhere);
            return xmlnodelist;
        }



 

        #region 判定结果功能
        public static string strPass = ExcelTemplateParams.PassResults;///<判定样品含量不超标
        public static string strFalse = ExcelTemplateParams.FalseResults;///<判定样品含量超标
        public static string strTBD = ExcelTemplateParams.STDResults;///<判定样品含量  我也不知道是咋样
        public static string TestRetult(string HistoryId, bool isContinuous,out string strCustomStandName)
        {
            strPass = ExcelTemplateParams.PassResults;///<判定样品含量不超标
            strFalse = ExcelTemplateParams.FalseResults;///<判定样品含量超标
            strTBD = ExcelTemplateParams.STDResults;///<判定样品含量  我也不知道是咋样
            strCustomStandName = string.Empty;
            //历史记录元素列表
            List<HistoryElement> HistoryElementlist = HistoryElement.FindBySql((isContinuous)?
                " select * from HistoryElement where HistoryRecord_Id in ( select historyid from continuousresult where continuousnumber in ( "+
                " select continuousnumber from continuousresult  where historyid='" + HistoryId + "' ))":
                " select * from HistoryElement where HistoryRecord_Id='" + HistoryId + "'");

            long historyStandID;
            if (HistoryElementlist.Count > 0)
            { historyStandID = HistoryElementlist[0].customstandard_Id; }
            else if (WorkCurveHelper.CurrentStandard != null)
                historyStandID = WorkCurveHelper.CurrentStandard.Id;
            else { historyStandID = -1; return ""; }

            //判定当前标准类型
            CustomStandard standard = CustomStandard.FindById(historyStandID);
            if (standard == null) return "";
            strCustomStandName = standard.StandardName;
            double dTotalValue = 0;
            //玩具标准
            if (standard.StandardName.ToLower().Contains("toy"))
            {
                dTotalValue = 0;
                foreach (HistoryElement hiselement in HistoryElementlist)
                {
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, true, false);
                    if (sJudgeValue == strTBD) return strTBD;
                }

                if (standard.IsSelectTotal && dTotalValue > standard.TotalContentStandard) return strFalse;
                else return strPass;
            }


            //Rohs标准和其他
            dTotalValue = 0;
            if (WorkCurveHelper.iResultJudgingType != 4)
            {
                //foreach (HistoryElement hiselement in HistoryElementlist.FindAll(w => w.elementName.ToLower() != "br" && w.elementName.ToLower() != "cr"))
                foreach (HistoryElement hiselement in HistoryElementlist)
                {
                    if (hiselement.elementName.ToLower() == "br" || hiselement.elementName.ToLower() == "cr") continue;
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, false, true);//先检查所有元素除br和cr是否存在fail
                    if (sJudgeValue == strFalse) return strFalse;
                    else if (sJudgeValue == strTBD) return strTBD;
                }

                //foreach (HistoryElement hiselement in HistoryElementlist.FindAll(w => w.elementName.ToLower() != "br" && w.elementName.ToLower() != "cr"))
                foreach (HistoryElement hiselement in HistoryElementlist)
                {
                    if (hiselement.elementName.ToLower() == "br" || hiselement.elementName.ToLower() == "cr") continue;
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, false, false);//先检查所有元素除br和cr是否存在TDB
                    if (sJudgeValue == strFalse) return strFalse;
                    else if (sJudgeValue == strTBD) return strTBD;
                }

                //foreach (HistoryElement hiselement in HistoryElementlist.FindAll(w => w.elementName.ToLower() == "br" || w.elementName.ToLower() == "cr"))
                foreach (HistoryElement hiselement in HistoryElementlist)
                {
                    if (hiselement.elementName.ToLower() != "br" || hiselement.elementName.ToLower() != "cr") continue;
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, false, false);
                    if (sJudgeValue == strTBD) return strTBD;
                }

            }
            else
            {
                foreach (HistoryElement hiselement in HistoryElementlist)
                {
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, false, true);
                    if (sJudgeValue == strFalse) return strFalse;
                    else if (sJudgeValue == strTBD) return strTBD;
                }

                foreach (HistoryElement hiselement in HistoryElementlist)
                {
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, false, false);
                    if (sJudgeValue == strFalse) return strFalse;
                    else if (sJudgeValue == strTBD) return strTBD;
                }
            }
            if (standard.IsSelectTotal && dTotalValue > standard.TotalContentStandard) return strFalse;
            else return strPass;
        }

        public static string TestRetult(string HistoryId, ref Dictionary<string, string> DElementRetult)
        {
            strPass = ExcelTemplateParams.PassResults;///<判定样品含量不超标
            strFalse = ExcelTemplateParams.FalseResults;///<判定样品含量超标
            strTBD = ExcelTemplateParams.STDResults;///<判定样品含量  我也不知道是咋样


            List<HistoryElement> HistoryElementlist = new List<HistoryElement>();

            ////判断是否存在连测情况       这条使用时，数据量大时， 数据执行很耗时
            //List<ContinuousResult> continuousResultList = ContinuousResult.FindBySql(" select a.* from continuousresult a " +
            //           " left outer join continuousresult b on b.ContinuousNumber=a.ContinuousNumber " +
            //           " where b.historyid='" + HistoryId + "'");
            string sql = "select * from continuousresult where  continuousnumber = (select continuousnumber from continuousresult where historyid='" + HistoryId + "')";

            List<ContinuousResult> continuousResultList = ContinuousResult.FindBySql(sql);
            if (continuousResultList.Count > 0)
            {
                foreach (ContinuousResult conResult in continuousResultList)
                    HistoryElementlist.AddRange(HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id='" + conResult.HistoryId + "'"));
            }
            else
                HistoryElementlist = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id='" + HistoryId + "'");

           
            long historyStandID;
            if (HistoryElementlist.Count > 0)
            { historyStandID = HistoryElementlist[0].customstandard_Id; }
            else if (WorkCurveHelper.CurrentStandard != null)
                historyStandID = WorkCurveHelper.CurrentStandard.Id;
            else { historyStandID = -1; return ""; }

            //判定当前标准类型
            CustomStandard standard = CustomStandard.FindById(historyStandID);
            if (standard == null) return "";

            double dTotalValue = 0;
            string sJudgeReturnValue = "";
            //玩具标准
            if (standard.StandardName.ToLower().Contains("toy"))
            {
                dTotalValue = 0;
                foreach (HistoryElement hiselement in HistoryElementlist)
                {
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, true, false);
                    if (sJudgeValue == strTBD && sJudgeReturnValue == "") sJudgeReturnValue = strTBD;
                    DElementRetult.Add(hiselement.elementName, sJudgeValue);
                }

                if (sJudgeReturnValue == "")
                {
                    if (standard.IsSelectTotal && dTotalValue > standard.TotalContentStandard) return strFalse;
                    else return strPass;
                }
                else return sJudgeReturnValue;
            }


            //Rohs标准和其他
            dTotalValue = 0;
            if (WorkCurveHelper.iResultJudgingType != 4)
            {

                foreach (HistoryElement hiselement in HistoryElementlist.FindAll(w => w.elementName.ToLower() != "br" && w.elementName.ToLower() != "cr"))
                {
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, false, false);
                    if (sJudgeValue == strFalse && sJudgeReturnValue == "") sJudgeReturnValue = strFalse;
                    else if (sJudgeValue == strTBD && sJudgeReturnValue == "") sJudgeReturnValue = strTBD;

                    if (sJudgeValue == "") sJudgeValue = strPass;
                    DElementRetult.Add(hiselement.elementName, sJudgeValue);
                }
                foreach (HistoryElement hiselement in HistoryElementlist.FindAll(w => w.elementName.ToLower() == "br" || w.elementName.ToLower() == "cr"))
                {
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, false, false);
                    if (sJudgeValue == strTBD && sJudgeReturnValue == "") sJudgeReturnValue = strTBD;
                    if (sJudgeValue == "") sJudgeValue = strPass;
                    if (DElementRetult.ContainsKey(hiselement.elementName)) continue;
                    DElementRetult.Add(hiselement.elementName, sJudgeValue);
                }

            }
            else
            {
               
                foreach (HistoryElement hiselement in HistoryElementlist)
                {
                    string sJudgeValue = JudgeElementStandard(hiselement, standard, ref dTotalValue, false, false);
                    if (sJudgeValue == strFalse) sJudgeReturnValue = strFalse;
                    else if (sJudgeValue == strTBD && sJudgeReturnValue == "") sJudgeReturnValue = strTBD;
                    if (sJudgeValue == "") sJudgeValue = strFalse;
                    DElementRetult.Add(hiselement.elementName, sJudgeValue);
                }
            }

            if (sJudgeReturnValue == "")
            {
                if (standard.IsSelectTotal && dTotalValue > standard.TotalContentStandard) return strFalse;
                else return strPass;
            }
            else return sJudgeReturnValue;
        }



        public static string TestRetultForXRF(string HistoryId, out string strCustomStandName)
        {
            strPass = ExcelTemplateParams.PassResults;///<判定样品含量不超标
            strFalse = ExcelTemplateParams.FalseResults;///<判定样品含量超标
            strTBD = ExcelTemplateParams.STDResults;///<判定样品含量  我也不知道是咋样
            strCustomStandName = string.Empty;

            HistoryRecord hr = HistoryRecord.FindById(long.Parse(HistoryId));
            if (hr == null) return "";
            List<HistoryElement> HistoryElementlist = new List<HistoryElement>();

            HistoryElementlist = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id='" + HistoryId + "'");

            long historyStandID;
            if (HistoryElementlist.Count > 0)
            { historyStandID = HistoryElementlist[0].customstandard_Id; }
            else if (WorkCurveHelper.CurrentStandard != null)
                historyStandID = WorkCurveHelper.CurrentStandard.Id;
            else { historyStandID = -1; return ""; }

            //判定当前标准类型
            CustomStandard standard = CustomStandard.FindById(historyStandID);
            if (standard == null) return "";
            strCustomStandName = standard.StandardName;
            double dTotalValue = 0;

            //Rohs标准和其他
            dTotalValue = 0;
            string sJudgeValue = "";
            string sJudgeReturnValue = "";
            foreach (HistoryElement hiselement in HistoryElementlist)
            {
                double eleContent = 0;
                if (hiselement.unitValue == 1)
                    eleContent = double.Parse(hiselement.contextelementValue) * 10000;
                else if (hiselement.unitValue == 2)
                    eleContent = double.Parse(hiselement.contextelementValue);
                else
                    eleContent = double.Parse(hiselement.contextelementValue) * 1000;
                if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                {
                    StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                    {
                        return string.Compare(w.ElementCaption, hiselement.elementName, true) == 0;
                    });
                    if (standSample != null)
                    {

                        if (standSample.StartStandardContent <= eleContent && eleContent <= standSample.StandardContent)
                        {
                            sJudgeValue = strPass;
                        }
                        else
                        {
                            sJudgeValue = strFalse;
                            sJudgeReturnValue = strFalse;
                        }
                    }
                }

            }
            if (sJudgeReturnValue == "")
                return strPass;
            else
                return sJudgeReturnValue;
        }

     
        public static string TestResultForXrf(string HistoryId, ref Dictionary<string, string> DElementResult)
        {
            strPass = ExcelTemplateParams.PassResults;///<判定样品含量不超标
            strFalse = ExcelTemplateParams.FalseResults;///<判定样品含量超标
            strTBD = ExcelTemplateParams.STDResults;///<判定样品含量  我也不知道是咋样
            List<HistoryElement> HistoryElementlist = new List<HistoryElement>();
            HistoryElementlist = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id='" + HistoryId + "'");
            long historyStandID;
            if (HistoryElementlist.Count > 0)
            { historyStandID = HistoryElementlist[0].customstandard_Id; }
            else if (WorkCurveHelper.CurrentStandard != null)
                historyStandID = WorkCurveHelper.CurrentStandard.Id;
            else { historyStandID = -1; return ""; }

            //判定当前标准类型
            CustomStandard standard = CustomStandard.FindById(historyStandID);
            if (standard == null) return "";

            double dTotalValue = 0;
           
            //Rohs标准和其他
            dTotalValue = 0;
            string sJudgeValue = "";
            string sJudgeReturnValue = "";
            foreach (HistoryElement hiselement in HistoryElementlist)
            {
                double eleContent = 0;
                if (hiselement.unitValue == 1)
                    eleContent = double.Parse(hiselement.contextelementValue) * 10000;
                else if (hiselement.unitValue == 2)
                    eleContent = double.Parse(hiselement.contextelementValue);
                else
                    eleContent = double.Parse(hiselement.contextelementValue) * 1000;
                if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                {
                    StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                    {
                        return string.Compare(w.ElementCaption, hiselement.elementName, true) == 0;
                    });
                    if (standSample != null)
                    {
                        //if (WorkCurveHelper.iResultJudgingType == 4)
                        {
                            if (standSample.StartStandardContent <= eleContent && eleContent <= standSample.StandardContent)
                            {
                                sJudgeValue = strPass;
                            }
                            else
                            {
                                sJudgeValue = strFalse;
                                sJudgeReturnValue = strFalse;
                            }

                        }

                    }
                }
               
                DElementResult.Add(hiselement.elementName, sJudgeValue);


            }
            if (sJudgeReturnValue == "")
                return strPass;
            else
                return sJudgeReturnValue;
        }

        public static string TestRetultForEdx(string HistoryId)
        {
            try
            {
                List<HistoryElement> HistoryElementlist = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id='" + HistoryId + "'");
                string workcurveName = WorkCurve.FindById(HistoryRecord.FindById(long.Parse(HistoryId)).WorkCurveId).Name;
                if (HistoryElementlist.Count > 0)
                {
                    string[] Elements = { "Au", "Pt", "Ag" };
                    IEnumerable<Skyray.EDX.Common.Component.ResultStandard> standardList = WorkCurveHelper.JapanStandard.StandardList.Where<Skyray.EDX.Common.Component.ResultStandard>(wc => wc.CurveName == workcurveName);

                    foreach (HistoryElement element in HistoryElementlist)
                        foreach (Skyray.EDX.Common.Component.ResultStandard standard in standardList)
                        {
                            if (standard.Caption == element.elementName)
                            {
                                if (double.Parse(element.contextelementValue) >= standard.Min && double.Parse(element.contextelementValue) <= standard.Max)
                                {
                                    return standard.DisplayText;
                                }
                            }
                        }
                }
                return "NG";
            }
            catch
            {
                return string.Empty;
            }

        }

        public static string TestRetultForThick(string HistoryId, out string strCustomStandName)
        {
            strPass = ExcelTemplateParams.PassResults;///<判定样品含量不超标
            strFalse = ExcelTemplateParams.FalseResults;///<判定样品含量超标
            strTBD = ExcelTemplateParams.STDResults;///<判定样品含量  我也不知道是咋样
            strCustomStandName = string.Empty;

            List<HistoryElement> HistoryElementlist = new List<HistoryElement>();

            HistoryElementlist = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id='" + HistoryId + "'");
            
            long historyStandID;
            if (HistoryElementlist.Count > 0)
            { historyStandID = HistoryElementlist[0].customstandard_Id; }
            else if (WorkCurveHelper.CurrentStandard != null)
                historyStandID = WorkCurveHelper.CurrentStandard.Id;
            else { historyStandID = -1; return ""; }

            //判定当前标准类型
            CustomStandard standard = CustomStandard.FindById(historyStandID);
            if (standard == null) return "";
            strCustomStandName = standard.StandardName;
            double dTotalValue = 0;

            //Rohs标准和其他
            dTotalValue = 0;
            string sJudgeValue = "";
            string sJudgeReturnValue = "";
            foreach (HistoryElement hiselement in HistoryElementlist)
            {
                double elemThickvalue = 0;
                // historyElemet.thickelementValue = (elements.ThicknessUnit.ToString().Equals("um") ) ? elements.Thickness.ToString() : ((double)elements.Thickness / 0.0254).ToString();//"f" + WorkCurveHelper.ThickBit

                bool trydouble  = double.TryParse(hiselement.thickelementValue, out elemThickvalue);
                if (hiselement.thickunitValue == 1)
                    elemThickvalue = elemThickvalue / 0.0254;
                else
                    elemThickvalue = elemThickvalue;
                //if (hiselement.thickunitValue == 1)
                //    elemThickvalue = double.Parse(hiselement.thickelementValue) / 0.0254;
                //else
                //    elemThickvalue = double.Parse(hiselement.thickelementValue);
                if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                {
                    StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                    {
                        return string.Compare(w.ElementCaption, hiselement.elementName, true) == 0;
                    });
                    if (standSample != null)
                    {

                        if (standSample.StandardThick <= elemThickvalue && elemThickvalue <= standSample.StandardThickMax)
                        {
                            sJudgeValue = strPass;
                        }
                        else
                        {
                            sJudgeValue = strFalse;
                          
                        }
                    }
                }

            }
            return sJudgeValue;
        }

      

        private static string JudgeElementStandard(HistoryElement hiselement, CustomStandard standard, ref double dContentValue, bool istoy, bool isOne)
        {
            strPass = ExcelTemplateParams.PassResults;///<判定样品含量不超标
            strFalse = ExcelTemplateParams.FalseResults;///<判定样品含量超标
            strTBD = ExcelTemplateParams.STDResults;///<判定样品含量  我也不知道是咋样
            string returnValue = "";
            double eleContent = 0;
            if (hiselement.unitValue == 1)
                eleContent = double.Parse(hiselement.contextelementValue) * 10000;
            else if (hiselement.unitValue == 2)
                eleContent = double.Parse(hiselement.contextelementValue);
            else
                eleContent = double.Parse(hiselement.contextelementValue) * 1000;
            double eleerror = 0;
            if (hiselement.unitValue == 1)
                eleerror = hiselement.Error * 10000;
            else if (hiselement.unitValue == 2)
                eleerror = hiselement.Error;
            else
                eleerror = hiselement.Error * 1000;

            dContentValue += eleContent;
            double totalValue = ((WorkCurveHelper.iResultJudgingType == 1) || (WorkCurveHelper.iResultJudgingType == 4)) ? eleContent : eleContent + eleerror;

            if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
            {
                StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                {
                    return string.Compare(w.ElementCaption, hiselement.elementName, true) == 0;
                });
                if (standSample != null)
                {
                    if (istoy && totalValue >= standSample.StandardContent) return strTBD;
                    else if (istoy && totalValue < standSample.StandardContent) return strPass;
                    else if (!istoy)
                    {
                        if (hiselement.elementName.Equals("Cr") || hiselement.elementName.Equals("Br"))
                        {
                            if (WorkCurveHelper.iResultJudgingType == 4)
                            {
                                if (totalValue > standSample.StandardContent) return strFalse;
                                else if (standSample.StartStandardContent <= totalValue && totalValue <= standSample.StandardContent) return strTBD;
                                else if (totalValue < standSample.StartStandardContent) return strPass;
                            }
                            else
                            {
                                if (totalValue >= standSample.StandardContent) return strTBD;
                                else if (totalValue < standSample.StandardContent) return strPass;
                            }
                        }
                        else
                        {
                            if ((WorkCurveHelper.iResultJudgingType == 1 || WorkCurveHelper.iResultJudgingType == 3) && totalValue >= standSample.StandardContent) return strFalse;
                            else if ((WorkCurveHelper.iResultJudgingType == 1 || WorkCurveHelper.iResultJudgingType == 3) && totalValue < standSample.StandardContent) return strPass;
                            else if (WorkCurveHelper.iResultJudgingType == 2 && totalValue >= standSample.StandardContent && isOne) return strFalse;
                            else if (WorkCurveHelper.iResultJudgingType == 2 && standSample.StartStandardContent <= totalValue && totalValue < standSample.StandardContent && !isOne) return strTBD;
                            else if (WorkCurveHelper.iResultJudgingType == 2 && totalValue < standSample.StartStandardContent) return strPass;
                            else if (WorkCurveHelper.iResultJudgingType == 4)
                            {
                                if (totalValue > standSample.StandardContent  && isOne) return strFalse;
                                else if (standSample.StartStandardContent <= totalValue && totalValue <= standSample.StandardContent && !isOne) return strTBD;
                                else if (totalValue < standSample.StartStandardContent) return strPass;

                            }
                        }
                    }
                }
            }

            return returnValue;
        }

        //暂时不用
        //private static string JudgeXRFElementStandard(HistoryElement hiselement, CustomStandard standard)
        //{
        //    strPass = ExcelTemplateParams.PassResults;///<判定样品含量不超标
        //    strFalse = ExcelTemplateParams.FalseResults;///<判定样品含量超标
        //    strTBD = ExcelTemplateParams.STDResults;///<判定样品含量  我也不知道是咋样
        //    string returnValue = "";
        //    double eleContent = 0;
        //    if (hiselement.unitValue == 1)
        //        eleContent = double.Parse(hiselement.contextelementValue) * 10000;
        //    else if (hiselement.unitValue == 2)
        //        eleContent = double.Parse(hiselement.contextelementValue);
        //    else
        //        eleContent = double.Parse(hiselement.contextelementValue) * 1000;

       

        //    if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
        //    {
        //        StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
        //        {
        //            return string.Compare(w.ElementCaption, hiselement.elementName, true) == 0;
        //        });
        //        if (standSample != null)
        //        {
        //            if (WorkCurveHelper.iResultJudgingType == 4)
        //            {
        //                if (eleContent > standSample.StandardContent) return strFalse;
        //                else if (standSample.StartStandardContent <= eleContent && eleContent <= standSample.StandardContent) return strPass;
        //                else if (totalValue < standSample.StartStandardContent) return strFalse;

        //            }


        //        }
        //    }

        //    return returnValue;
        //}


        #endregion

        struct KaratItem
        {
            public KaratItem(double max, double min, string content)
            {
                Max = max;
                Min = min;
                Content = content;
            }
            public double Max;
            public double Min;
            public string Content;
        }
    }
}
