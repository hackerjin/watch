using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Skyray.Language;
using System.Xml.Linq;

namespace Skyray.EDX.Common
{
    public class ReportTemplateHelper
    {
        public static string Edition = "";//Rohs,EDXRF,FPThick,XRF

        #region Excel导出参数
        /// <summary>
        /// excel报告模板导出方式
        /// </summary>
        public static int ExcelModeType = 0;
        /// <summary>
        /// 直接导出excel后，元素是否添加标准库中的标准值
        /// </summary>
        public static int IsOurExcel = 0;
        /// <summary>
        /// excel报告模板导出是否加密
        /// </summary>
        public static bool IsEncryption = false;

        /// <summary>
        /// 使用目前导出方式为1,不存在或者0为老方式进行
        /// </summary>
        public static bool IsCurrentUsed = false;

        /// <summary>
        /// 导出历史记录时关联报告
        /// </summary>
        public static bool IsShowUrlInRecords = false;

        public static string ReportAtomNames = null;
        #endregion

        #region 历史记录参数
        /// <summary>
        /// 历史记录界面是否勾选单位
        /// </summary>
        public static bool ShowUnitType = false;
        /// <summary>
        /// 联测历史记录界面是否勾选单位
        /// </summary>
        public static bool ContinuousShowUnitType = false;
        /// <summary>
        /// 联测历史记录界面是否勾选卤素
        /// </summary>
        public static bool ContinuousLushuType = false;
        /// <summary>
        /// 历史记录界面显示数
        /// </summary>
        public static int ShowNumber = 0;
        /// <summary>
        /// 联测历史记录界面显示数
        /// </summary>
        public static int ContinuousNumber = 0;
        /// <summary>
        /// 历史记录界面允许选择记录个数
        /// </summary>
        public static int SelectRecordNumber = 0;
        public static bool IsShowArea = false;
        public static bool IsShowError = false;
        public static bool IsShowIntensity = false;
        public static bool IsShowUnitColumns = false;
        public static bool IsShowAverageColumns = false;
        public static bool IsShowElementAllName = false;
        public static bool IsOnlyShowScanRecord = false;
        public static bool IsShowNA = false;
        public static bool IsShowClAndBr = false;
        public static double StandardContent = 0;
        public static bool IsShowResole = false;
        public static bool IsSplitScreen = false;
        public static int HistoryItemType = 0;//更改不同曲线对应不同的历史记录设置的需求。0,是所有曲线使用同一个设置。1,是每个曲线单独设置。
        public static int ReoprtxAxisScale = 0;// pf by 150615 记录X轴水平刻度值
        public static int ReportXAxisScaleMax = 0;
        public static int ReportXStep = 200;
        public static bool IsSpecShowElem = false;
        /// <summary>
        /// //历史记录显示真实值
        /// </summary>
        public static bool IsShowRContent = false;  

        public static void GetParameter()
        {
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\AppParams.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList nodeList = doc.SelectNodes("application/OpenHistoryRecordType");
                foreach (XmlNode node in nodeList)
                {
                    XmlNode childNode = node.SelectSingleNode("HistoryRecordShowUnitType");
                    ShowUnitType = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("HistoryRecordContinuousShowUnitType");
                    ContinuousShowUnitType = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("HistoryRecordContinuousLushuType");
                    ContinuousLushuType = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("HistoryRecordShowNumber");
                    ShowNumber = int.Parse(childNode == null ? "0" : childNode.InnerText);
                    childNode = node.SelectSingleNode("HistoryRecordContinuousNumber");
                    ContinuousNumber = int.Parse(childNode == null ? "0" : childNode.InnerText);
                    childNode = node.SelectSingleNode("SelectRecordNumber");
                    WorkCurveHelper.PrintExcelCount = SelectRecordNumber = int.Parse(childNode == null ? "0" : childNode.InnerText);
                    childNode = node.SelectSingleNode("IsShowArea");
                    IsShowArea = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("IsShowError");
                    IsShowError = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("IsShowIntensity");
                    IsShowIntensity = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("IsShowUnitColumns");
                    IsShowUnitColumns = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("IsShowAverageColumns");
                    IsShowAverageColumns = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("IsShowElementAllName");
                    IsShowElementAllName = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("IsOnlyShowScanRecord");
                    IsOnlyShowScanRecord = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("IsShowNA");
                    IsShowNA = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("IsShowClAndBr");
                    IsShowClAndBr = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    StandardContent = node.SelectSingleNode("IsShowClAndBr") == null ? 0 : double.Parse(node.SelectSingleNode("IsShowClAndBr").Attributes["StandardContent"].Value);
                    childNode = node.SelectSingleNode("IsShowResole");
                    IsShowResole = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("HistorySplitScreen");
                    IsSplitScreen = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                    childNode = node.SelectSingleNode("IsShowRContent");
                    IsShowRContent = Convert.ToBoolean(int.Parse(childNode == null ? "0" : childNode.InnerText));
                }

                ///导出excel和报告方式
                nodeList = doc.SelectNodes("application/Excel");
                foreach (XmlNode node in nodeList)
                {
                    ExcelModeType = Convert.ToInt16(node.SelectSingleNode("ExcelModeType") == null ? "0" : node.SelectSingleNode("ExcelModeType").InnerText);
                    IsOurExcel = Convert.ToInt16(node.SelectSingleNode("IsOurExcel") == null ? "0" : node.SelectSingleNode("IsOurExcel").InnerText);
                    IsEncryption = Convert.ToBoolean(int.Parse(node.SelectSingleNode("IsEncryption") == null ? "0" : node.SelectSingleNode("IsEncryption").InnerText));
                    IsCurrentUsed = Convert.ToBoolean(int.Parse(node.SelectSingleNode("IsCurrentUsed") == null ? "0" : node.SelectSingleNode("IsCurrentUsed").InnerText));
                    IsShowUrlInRecords = Convert.ToBoolean(int.Parse(node.SelectSingleNode("IsShowUrlInRecords") == null ? "0" : node.SelectSingleNode("IsShowUrlInRecords").InnerText));
                    ReportAtomNames = node.SelectSingleNode("ReportAtomNames") == null ? null : node.SelectSingleNode("ReportAtomNames").InnerText;
                }
                ///Rohs判定结果判定方式
                nodeList = doc.SelectNodes("application/ResultJudgingType");
                foreach (XmlNode node in nodeList)
                {
                    WorkCurveHelper.iResultJudgingType = Convert.ToInt16(node.InnerText);
                }

                
                XmlDocument doc1 = new XmlDocument();
                //追加历史记录里面，不同的曲线对应不同的历史记录设置
                if (File.Exists(Application.StartupPath + "\\CustomHistory.xml"))
                {
                    doc1.Load(Application.StartupPath + "\\CustomHistory.xml");
                    XmlNode childNode = doc1.SelectSingleNode("application/HistoryItemType");
                    HistoryItemType = childNode == null ? 0 : Convert.ToInt32(childNode.InnerText);
                }
            }
        }
        #endregion

        public static void LoadDirctoryTemplate()
        {
            System.Threading.Thread.Sleep(100);
            NaviItem oldTemplate = WorkCurveHelper.NaviItems.Find(w => w.Name == "OldReport");
            if (oldTemplate != null && oldTemplate.Enabled)
            {
                string path = Application.StartupPath + "\\PrintTemplate\\template.xml";
                try {
                    XmlDocument doc = new XmlDocument();
                    
                    if (File.Exists(path))
                    {
                        oldTemplate.MenuStripItem.DropDownItems.Clear();
                        doc.Load(path);
                        XmlNodeList rootNodeList = doc.SelectNodes("application/ReportTemplate/" + Edition + "/" + WorkCurveHelper.LanguageShortName + "/TemplateName");
                        if (rootNodeList != null)
                        {
                            foreach (XmlNode rootNode in rootNodeList)
                            {
                                if (rootNode.Attributes["IsShowSele"].Value.ToString() != "true") continue;


                                string reportName = rootNode.InnerText;
                                string MulitName=rootNode.Attributes["Mulit"].Value;
                                //验证模板是否真实存在、和多个模板是否存在
                                string TemplatePath=Application.StartupPath + "\\PrintTemplate\\";
                                if (File.Exists(TemplatePath + reportName) && (MulitName == "") ? true : File.Exists(TemplatePath + MulitName))
                                {
                                    ToolStripMenuItem item = new ToolStripMenuItem(reportName);
                                    item.Name = reportName;
                                    item.Click += delegate(object sender, EventArgs e)
                                    {
                                        if (oldTemplate != null)
                                        {
                                            foreach (ToolStripMenuItem enumItem in oldTemplate.MenuStripItem.DropDownItems)
                                                enumItem.Checked = false;
                                            item.Checked = true;
                                            LoadReportName(item, false);
                                        }
                                    };
                                    oldTemplate.MenuStripItem.DropDownItems.Add(item);
                                }
                            }
                            LoadReportName(oldTemplate.MenuStripItem, true);
                        }
                    }
                }
                catch (Exception e)
                {
                    Msg.Show(e.Message);

                }
            }
        }


        public static void LoadReportName(ToolStripMenuItem item, bool style)
        {
            System.Threading.Thread.Sleep(100);
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\AppParams.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                 XmlNode rootNode = doc.SelectSingleNode("application");
                 if (rootNode != null)
                 {
                     XmlNode reportNode = rootNode.SelectSingleNode("Report");
                     XmlNode reportNameNode = reportNode.SelectSingleNode("ReportName");
                       if (style)
                       {
                           string templateName = reportNameNode.InnerText.Trim();
                           ToolStripMenuItem currentItem = GetToolsByName(item, templateName);
                           if (currentItem != null)
                               currentItem.Checked = true;
                           else if (item.DropDownItems.Count > 0)
                           {
                               foreach (ToolStripMenuItem strip in item.DropDownItems)
                               {
                                   strip.Checked = true;
                                   SaveSpecifiedValue("Report", "ReportName", strip.Name);//保存默认模板
                                   break;
                               }
                           }
                       }
                       else
                       {
                           reportNameNode.InnerText = item.Name;
                           doc.Save(path);
                       }
                 }
            }
        }

        public static string LoadReportName()
        {
            System.Threading.Thread.Sleep(100);
            XmlDocument doc = new XmlDocument();
            string reportName = string.Empty;
            string path = Application.StartupPath + "\\AppParams.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/Report/ReportName");
                if (rootNode != null)
                {
                    reportName = rootNode.InnerText;
                }
            }
            return reportName;
        }


      

        public static string LoadSpecifiedValue(string tag, string label)
        {
            System.Threading.Thread.Sleep(100);
            //XmlDocument doc = new XmlDocument();
            //string paramsName = string.Empty;
            //string path = Application.StartupPath + "\\AppParams.xml";
            //if (File.Exists(path))
            //{
            //    doc.Load(path);
            //    XmlNode rootNode = doc.SelectSingleNode("application/" + tag + "/" + label);
            //    if (rootNode != null)
            //    {
            //        paramsName = rootNode.InnerText;
            //    }
            //}
            return LoadSpecifiedValueNoWait(tag, label);
        }

        public static string LoadSpecifiedValueNoWait(string tag, string label)
        {
            XmlDocument doc = new XmlDocument();
            string paramsName = string.Empty;
            string path = Application.StartupPath + "\\AppParams.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/" + tag + "/" + label);
                if (rootNode != null)
                {
                    paramsName = rootNode.InnerText;
                }
            }
            return paramsName;
        }

        public static XmlNode LoadSpecifiedNode(string tag, string label)
        {
            string path = Application.StartupPath + "\\AppParams.xml";
            return LoadSpecifiedNode(tag,label,path);
        }

        private static XmlNode LoadSpecifiedNode(string tag, string label, string path)
        {
            System.Threading.Thread.Sleep(100);
            XmlDocument doc = new XmlDocument();
            XmlNode paramsXmlNode = null;
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = null;
                if (label != null && label.Trim() != string.Empty)
                    rootNode = doc.SelectSingleNode("application/" + tag + "/" + label);
                else
                    rootNode = doc.SelectSingleNode("application/" + tag);
                if (rootNode != null)
                {
                    paramsXmlNode = rootNode;
                }
            }
            return paramsXmlNode;
        }


        public static void SaveAttribute(string tag,string attribute, string value)
        {
            System.Threading.Thread.Sleep(50);
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\Parameter.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlElement node = (XmlElement)doc.SelectSingleNode("Parameter/" + tag );
                if (node != null)
                {
                    node.SetAttribute(attribute, value);
                    doc.Save(path);
                }
            }
        }

        public static void SaveSpecifiedValue(string tag,string label,string value)
        {
            System.Threading.Thread.Sleep(50);
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\AppParams.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/" + tag + "/" + label);
                if (rootNode != null)
                {
                    rootNode.InnerText = value;
                    doc.Save(path);
                }
            }
        }



        public static void SaveSpecifiedValue(string path,string tag, string label, string value)
        {
            System.Threading.Thread.Sleep(50);
            XmlDocument doc = new XmlDocument();
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/" + tag + "/" + label);
                if (rootNode != null)
                {
                    rootNode.InnerText = value;
                    doc.Save(path);
                }
                else
                {
                    //没有则写入节点
                    XmlNode rootNode_application = doc.SelectSingleNode("application");
                    if (rootNode_application != null)
                    {
                        XmlNode tagNode = rootNode_application.SelectSingleNode(tag);
                        if (tagNode != null)
                        {
                            rootNode_application = tagNode;
                            XmlElement newXmlNode_label = doc.CreateElement(label);
                            newXmlNode_label.InnerText = value;
                            rootNode_application.AppendChild(newXmlNode_label);
                        }
                        else
                        {
                            XmlElement newXmlNode_tag = doc.CreateElement(tag);
                            XmlElement newXmlNode_label = doc.CreateElement(label);
                            newXmlNode_label.InnerText = value;
                            newXmlNode_tag.AppendChild(newXmlNode_label);
                            rootNode_application.AppendChild(newXmlNode_tag);
                        }
                        doc.Save(path);
                    }
                }
            }
        }

        public static void SaveSpecifiedValue(XmlDocument doc, string tag, string label, string value)
        {
            if (doc == null)
                return;
            System.Threading.Thread.Sleep(50);
            XmlNode rootNode = doc.SelectSingleNode("application/" + tag + "/" + label);
            if (rootNode != null)
            {
                rootNode.InnerText = value;
            }
            else
            {
                //没有则写入节点
                XmlNode rootNode_application = doc.SelectSingleNode("application");
                if (rootNode_application != null)
                {
                    XmlNode tagNode = rootNode_application.SelectSingleNode(tag);
                    if (tagNode != null)
                    {
                        rootNode_application = tagNode;
                        XmlElement newXmlNode_label = doc.CreateElement(label);
                        newXmlNode_label.InnerText = value;
                        rootNode_application.AppendChild(newXmlNode_label);
                    }
                    else
                    {
                        XmlElement newXmlNode_tag = doc.CreateElement(tag);
                        XmlElement newXmlNode_label = doc.CreateElement(label);
                        newXmlNode_label.InnerText = value;
                        newXmlNode_tag.AppendChild(newXmlNode_label);
                        rootNode_application.AppendChild(newXmlNode_tag);
                    }
                }
            }
        }

        public static void SaveSpecifiedParamsValue(string path, string selectNodePath,string value)
        {
            System.Threading.Thread.Sleep(50);
            XmlDocument doc = new XmlDocument();
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode(selectNodePath);
                if (rootNode != null)
                {
                    rootNode.InnerText = value;
                    doc.Save(path);
                }
               
            }
        }

        public static string LoadSpecifiedParamsValue(string path, string selectNodePath)
        {

            System.Threading.Thread.Sleep(50);
            XmlDocument doc = new XmlDocument();
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode(selectNodePath);
                if (rootNode != null)
                {
                    return rootNode.InnerText;

                }
                else
                    return "None";
            }
            else
                return "None";


        }


        public static void SaveSpecifiedValue(string path, XmlDocument target, string strNodes, Dictionary<string, string> dicValues, bool needSave)
        {
            System.Threading.Thread.Sleep(50);
            XmlDocument doc = new XmlDocument();
            if (target != null)
            {
                doc = target;
            }
            else
            {
                if (File.Exists(path))
                {
                    doc.Load(path);
                }
            }
            if (string.IsNullOrEmpty(strNodes))
                return;
            string[] arrNodes = strNodes.Split('/');
            string currentNode = arrNodes[0];
            XmlNode parent = doc.DocumentElement;
            for (int i = 0; i < arrNodes.Length; i++)
            {
                if (i > 0)
                {
                    currentNode += "/" + arrNodes[i];
                }
                XmlNode rootNode = doc.SelectSingleNode(currentNode);
                if (rootNode == null)
                {
                    XmlElement ele = doc.CreateElement(arrNodes[i]);
                    if (dicValues != null && dicValues.ContainsKey(arrNodes[i]))
                    {
                        ele.InnerText = dicValues[arrNodes[i]];
                    }
                    if (parent != null)
                        parent.AppendChild(ele);
                }
                else
                {
                    if (i == arrNodes.Length - 1 && dicValues != null && dicValues.ContainsKey(arrNodes[i]))
                    {
                        rootNode.InnerText = dicValues[arrNodes[i]];
                    }
                }
                parent = doc.SelectSingleNode(currentNode); ;
            }
            if (needSave && File.Exists(path))
            {
                doc.Save(path);
            }
        }
        //修改历史记录保存的元素顺序-----------------------------begin-----------------------
        public static void SaveHistoryItemSpecifiedValue(string tag, string label, string value)
        {
            string path = Application.StartupPath + "\\CustomHistory.xml";
            SaveSpecifiedValue(path, tag, label, value);
        }
        public static string LoadHistoryItemSpecifiedValue(string tag, string label)
        {
            string path = Application.StartupPath + "\\CustomHistory.xml";
            XmlDocument doc = new XmlDocument();
            string paramsName = string.Empty;
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/" + tag + "/" + label);
                if (rootNode != null)
                {
                    paramsName = rootNode.InnerText;
                }
            }
            return paramsName;
        }

        public static void 
            DeleteHistoryItemSpecifiedValue(string tag)
        {
            System.Threading.Thread.Sleep(50);
            string path = Application.StartupPath + "\\CustomHistory.xml";
            XmlDocument doc = new XmlDocument();
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/" + tag);
                if (rootNode != null)
                {
                    XmlNode rootNode_application = doc.SelectSingleNode("application");
                    rootNode_application.RemoveChild(rootNode);
                    doc.Save(path);
                }

            }
        }

        public static void EiditItemSpecifiedValue(string src_tag, string des_tag)
        {
            System.Threading.Thread.Sleep(50);
            string path = Application.StartupPath + "\\CustomHistory.xml";
            XmlDocument doc = new XmlDocument();
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/" + src_tag);
                if (rootNode != null)
                {
                    XmlNode rootNode_application = doc.SelectSingleNode("application");
                    XmlElement newXmlNode_label = doc.CreateElement(des_tag);
                    foreach (XmlNode xn in rootNode.ChildNodes)
                    {
                        newXmlNode_label.AppendChild(xn.Clone());
                    }
                    rootNode_application.AppendChild(newXmlNode_label);
                    rootNode_application.RemoveChild(rootNode);
                    doc.Save(path);
                }

            }
        }



        public static bool ExistSpecifiedNode(string tag, string label)
        {
            string path = Application.StartupPath + "\\CustomHistory.xml";
            return LoadSpecifiedNode(tag, label, path) != null;
        }

        /// <summary>
        /// 获取自定义历史记录的路径
        /// </summary>
        /// <param name="ItemType">WorkCurveID类型0WorkCurve 1WorkRegion</param>
        /// <param name="historyItemPathSave">应该保存的路径</param>
        /// <returns>目前只能使用的路径</returns>
        public static string GetHistoryItemPath(string WorkCurveID, long DeviceId, int DeviceFunctype,int ItemType, out string historyItemPathSave)
        {
            string selectCurve = (WorkCurveID == "--All--" || WorkCurveID == string.Empty || WorkCurveID == "-1") ? null : WorkCurveID;
            string historyItemPath = "HistoryItem";
            historyItemPathSave = "HistoryItem";
            if (selectCurve != null && ReportTemplateHelper.HistoryItemType == 1)
            {
                historyItemPathSave = "HistoryItem_" + DeviceId+"_"+(ItemType==0?"WorkCurve":"WorkRegion") + "_" + WorkCurveID;
                if (ReportTemplateHelper.ExistSpecifiedNode(historyItemPathSave, null))
                    historyItemPath = historyItemPathSave;

            }
            return historyItemPath;
        }
        //修改历史记录保存的元素顺序-----------------------------end-----------------------
        /// <summary>
        /// //paul 2011-04-26  如果“AppParams.xml”文件中不存在，则新建节点tag节点则新增节点
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="label"></param>
        /// <param name="value"></param>
        public static void SaveSpecifiedValueandCreate(string tag, string label, string value)
        {
            System.Threading.Thread.Sleep(50);
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\AppParams.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/" + tag + "/" + label);
                if (rootNode != null)
                {
                    rootNode.InnerText = value;
                    doc.Save(path);
                }
                else
                {
                    XmlNode rootNode_application = doc.SelectSingleNode("application/" + tag);
                    if (rootNode_application == null)
                    {
                        rootNode_application = doc.SelectSingleNode("application");
                        XmlElement newXmlNode_tag = doc.CreateElement(tag);
                        XmlElement newXmlNode_label = doc.CreateElement(label);
                        newXmlNode_label.InnerText = value;
                        newXmlNode_tag.AppendChild(newXmlNode_label);
                        rootNode_application.AppendChild(newXmlNode_tag);
                    }
                    else
                    {
                        XmlElement newXmlNode_label = doc.CreateElement(label);
                        newXmlNode_label.InnerText = value;
                        rootNode_application.AppendChild(newXmlNode_label);
                    }
                    doc.Save(path);
                }
            }
        }

        public static void CreateSpecifiedValue(string tag, string label, string value)
        {
            System.Threading.Thread.Sleep(50);
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\AppParams.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode_application = doc.SelectSingleNode("application/" + tag);
                if (rootNode_application != null)
                {
                    XmlElement newXmlNode_label = doc.CreateElement(label);
                    newXmlNode_label.InnerText = value;
                    rootNode_application.AppendChild(newXmlNode_label);
                    doc.Save(path);
                }
            }
        }

        public static void ClearSpecifiedTag(string tag)
        {
            System.Threading.Thread.Sleep(50);
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\AppParams.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/" + tag);
                rootNode.RemoveAll();
                doc.Save(path);
            }
        }

        public static string LoadByParameterSpecifiedValue(string tag, string label)
        {
            XmlDocument doc = new XmlDocument();
            string paramsName = string.Empty;
            string path = Application.StartupPath + "\\Parameter.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("Parameter/" + tag + "/" + label);
                if (rootNode != null)
                {
                    paramsName = rootNode.InnerText;
                }
            }
            return paramsName;
        }

        public static ToolStripMenuItem GetToolsByName(ToolStripMenuItem item, string templateName)
        {
            if (item == null || item.DropDownItems.Count == 0)
                return null;
            foreach (ToolStripMenuItem strip in item.DropDownItems)
                if (strip.Name == templateName)
                    return strip;
            return null;
        }

        public static XmlNodeList LoadNodes(string tag, string label)
        {
            string path = Application.StartupPath + "\\AppParams.xml";
            XmlDocument doc = new XmlDocument();
            XmlNodeList paramsXmlNode = null;
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList rootNode = null;
                if (label != null && label.Trim() != string.Empty)
                    rootNode = doc.SelectNodes("application/" + tag + "/" + label);
                else
                    rootNode = doc.SelectNodes("application/" + tag);
                if (rootNode != null)
                {
                    paramsXmlNode = rootNode;
                }
            }
            return paramsXmlNode;
        }

    }
}
