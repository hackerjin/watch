using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Print
{
    public class DirectPrintLibcs
    {
        public static string currentTemplatePath = string.Empty;


        public static List<TreeNodeInfo> lst = new List<TreeNodeInfo>();


        /// <summary>
        /// 添加本地数据源
        /// </summary>
        /// <param name="ucPrint">打印模板控件</param>
        public static void LoadLocalSource(UCPrint ucPrint)
        {
            List<TreeNodeInfo> lstLocalSource = PrintHelper.FileToObj<List<TreeNodeInfo>>(AppDomain.CurrentDomain.BaseDirectory + "LocalDataSource.tni");
            if (lstLocalSource != null)
            {
                foreach (TreeNodeInfo tni in lstLocalSource)
                {
                    if (ucPrint.DataSource.Find(w => w.Name == tni.Name && w.Type == tni.Type) == null)
                    {
                        ucPrint.DataSource.Add(tni);
                    }
                }
            }
        }

        /// <summary>
        /// 根据模板打印
        /// </summary>
        /// <param name="docName">模板的路径及名称</param>
        /// <param name="listInfo">数据源对象</param>
        /// <param name="excelPath">excel缓存路径</param>
        public static void LoadTemplate(string docName, string excelPath)
        {
            UCPrint ucPrint = new UCPrint();
            ucPrint.DataSource = lst;
            LoadLocalSource(ucPrint);
            ucPrint.OpenTemplate(PrintHelper.FileToObj<TemplateSource>(docName));
            ucPrint.previewControl1 = new PreviewControl(ucPrint.Page);
            ucPrint.previewControl1.DirectExportToExcel(excelPath);
        }

        /// <summary>
        ///  根据模板打印
        /// </summary>
        /// <param name="listInfo">数据源对象</param>
        /// <param name="excelPath">excel缓存路径</param>
        public static void LoadTemplate(string excelPath)
        {
            UCPrint ucPrint = new UCPrint();
            ucPrint.DataSource = lst;
            LoadLocalSource(ucPrint);
            ucPrint.OpenTemplate(ucPrint.SelectTemplate());
            ucPrint.previewControl1 = new PreviewControl(ucPrint.Page);
            ucPrint.previewControl1.DirectExportToExcel(excelPath);
        }

        //修改：何晓明 2011-01-17 
        //原因：按指定名称保存源数据至EXCEL
        /// <summary>
        /// 根据模板打印
        /// </summary>
        /// <param name="listInfo">数据源对象</param>
        /// <param name="excelPath">excel缓存路径</param>
        /// <param name="fileName">excel名称，不需指定后缀名默认xls</param>
        public static void LoadTemplate(string docName, string excelPath, string fileName)
        {
            UCPrint ucPrint = new UCPrint();
            ucPrint.DataSource = lst;
            LoadLocalSource(ucPrint);
            ucPrint.OpenTemplate(PrintHelper.FileToObj<TemplateSource>(docName));
            ucPrint.previewControl1 = new PreviewControl(ucPrint.Page);
            ucPrint.previewControl1.DirectExportToExcel(excelPath, fileName);
        }

        /// <summary>
        /// 直接打印
        /// </summary>
        /// <param name="strFilenName">文件路径</param>
        /// <param name="lstInfo">数据源对象</param>
       public static void DirectPrint(string strFileName)
       {
            UCPrint ucPrint = new UCPrint();
            ucPrint.DataSource = lst;
            LoadLocalSource(ucPrint);
            ucPrint.OpenTemplate(PrintHelper.FileToObj<TemplateSource>(strFileName));
            ucPrint.previewControl1 = new PreviewControl(ucPrint.Page);
            ucPrint.previewControl1.Print();
        }
        //修改：何晓明 2011-03-14
        //原因：直接打开打印模板
        /// <summary>
        /// 直接打开模板
        /// </summary>
        /// <param name="strFileName">文件名称</param>
        /// <param name="lstInfo">数据源</param>
        /// <returns></returns>
       public static void DirectOpenTemplate(string strFileName)
       {
           UCPrint ucPrint = new UCPrint();
           ucPrint.DataSource = lst;
           LoadLocalSource(ucPrint);
           ucPrint.OnNotifyPathChange += new UCPrint.NotifyPathChange(ucPrint_OnNotifyPathChange);
           ucPrint.OpenTemplate(PrintHelper.FileToObj<TemplateSource>(strFileName));
           ucPrint.CurrentTemplateName = strFileName;
           //修改：何晓明 2011-03-15
           //原因：函数打开窗口允许最大最小化
           PrintHelper.OpenUC(new System.Windows.Forms.Form(), ucPrint, true, strFileName.Substring(Math.Max(strFileName.LastIndexOf('\\') + 1, 0)));
           //PrintHelper.OpenUC(ucPrint, strFileName.Substring(Math.Max( strFileName.LastIndexOf('\\')+1,0)));
       }
       //修改：朱庆华 2010-03-14
       //原因：获取控件更改模板路径后的值
       static void ucPrint_OnNotifyPathChange(string strPath)
       {
           currentTemplatePath = strPath;
       }
       //
    }
}
