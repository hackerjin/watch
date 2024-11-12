using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace Skyray.EDX.Common.OutExcel
{
    public class VkExcel
    {
        private Excel.Application excelApp = null;
        private Excel.Workbook excelWorkbook = null;
        private Excel.Sheets excelSheets = null;
        private Excel.Worksheet excelWorksheet = null;

        private static object vk_missing = System.Reflection.Missing.Value;

        private static object vk_visible = true;
        private static object vk_false = false;
        private static object vk_true = true;

        private bool vk_app_visible = false;

        private object vk_filename;

        #region OPEN WORKBOOK VARIABLES
        private object vk_update_links = 0;
        private object vk_read_only = vk_true;
        private object vk_format = 1;
        private object vk_password = vk_missing;
        private object vk_write_res_password = vk_missing;
        private object vk_ignore_read_only_recommend = vk_true;
        private object vk_origin = vk_missing;
        private object vk_delimiter = vk_missing;
        private object vk_editable = vk_false;
        private object vk_notify = vk_false;
        private object vk_converter = vk_missing;
        private object vk_add_to_mru = vk_false;
        private object vk_local = vk_false;
        private object vk_corrupt_load = vk_false;
        #endregion

        #region CLOSE WORKBOOK VARIABLES
        private object vk_save_changes = vk_false;
        private object vk_route_workbook = vk_false;
        #endregion

        public VkExcel()
        {
            this.startExcel();
        }


        public VkExcel(bool visible)
        {
            this.vk_app_visible = visible;
            this.startExcel();
        }

        /// <summary>
        /// Vahe Karamian - 03/04/2005 - Start Excel Application
        /// </summary>
        #region 开启Excel
        private void startExcel()
        {
            if (this.excelApp == null)
            {
                this.excelApp = new Excel.ApplicationClass();
            }

            this.excelApp.ScreenUpdating = false;
            this.excelApp.DisplayAlerts = false;
            // Make Excel Visible
            this.excelApp.Visible = this.vk_app_visible;
        }
        #endregion


        #region 关闭Excel
        public void stopExcel()
        {
            if (this.excelApp != null)
            {
                this.excelApp.ScreenUpdating = true;
                Process[] pProcess;
                pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
                pProcess[0].Kill();
            }
        }
        #endregion


        #region 打开模板
        public string OpenFile(string fileName, string password)
        {
            vk_filename = fileName;

            if (password.Length > 0)
            {
                vk_password = password;
            }

            try
            {
                // Open a workbook in Excel
                //this.excelWorkbook = this.excelApp.Workbooks.Open(
                //    fileName, vk_update_links, vk_read_only, vk_format, vk_password,
                //    vk_write_res_password, vk_ignore_read_only_recommend, vk_origin,
                //    vk_delimiter, vk_editable, vk_notify, vk_converter, vk_add_to_mru,
                //    vk_local, vk_corrupt_load);
                this.excelWorkbook = this.excelApp.Workbooks.Open(
                fileName, vk_missing, vk_missing, vk_missing, vk_missing,
                vk_missing, vk_missing, vk_missing,
                vk_missing, vk_missing, vk_missing, vk_missing, vk_missing,
                vk_missing, vk_missing);
                GetExcelSheets();
            }
            catch (Exception e)
            {
                this.CloseFile();
                return e.Message;
            }
            return "OK";
        }
        #endregion

        public void CloseFile()
        {
            excelWorkbook.Close(vk_save_changes, vk_filename, vk_route_workbook);
        }

        /// <summary>
        /// 查找文本所在的区域（多个）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<Excel.Range> FindText(string text)
        {
            List<Excel.Range> rngList = new List<Excel.Range>();
            object oText = text.Trim().ToUpper();
            Excel.Range rngFoundFirst = null;
            if (excelWorksheet != null)
            {
                Excel.Range rang = (Excel.Range)excelWorksheet.UsedRange;
                Excel.Range rngFound = rang.Find(oText, vk_missing, vk_missing, vk_missing, vk_missing, Excel.XlSearchDirection.xlNext, vk_missing, vk_missing, vk_missing);
                // if (rngFound != null && rngFound.Cells.Rows.Count >= 1 && rngFound.Cells.Columns.Count >= 1)
                while (rngFound != null)
                {
                    if (rngFoundFirst == null)
                    {
                        rngFoundFirst = rngFound;
                    }
                    else if (rngFound.get_Address(vk_missing, vk_missing, Excel.XlReferenceStyle.xlA1, vk_missing, vk_missing) == rngFoundFirst.get_Address(vk_missing, vk_missing, Excel.XlReferenceStyle.xlA1, vk_missing, vk_missing))
                    {
                        break;
                    }
                    rngList.Add(rngFound);
                    rngFound = rang.FindNext(rngFound);
                    // return true;
                }
            }
            return rngList;
        }

        /// <summary>
        /// 替代文本
        /// </summary>
        /// <param name="whatText"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public bool ReplaceText(string whatText, object replacement)
        {
            object oText = whatText.Trim().ToUpper();
            if (excelWorksheet != null)
            {
                Excel.Range rang = (Excel.Range)excelWorksheet.UsedRange;
                return rang.Replace(oText, replacement, vk_missing, vk_missing, vk_missing, vk_missing, vk_missing, vk_missing);
            }
            return false;
        }

        /// <summary>
        /// 查找文本所在的区域（一个）
        /// </summary>
        /// <param name="whatText"></param>
        /// <returns></returns>
        public Excel.Range FindRang(string whatText)
        {
            Excel.Range rngResult = null;
            object oText = whatText.Trim().ToUpper();
            if (excelWorksheet != null)
            {
                Excel.Range rang = (Excel.Range)excelWorksheet.UsedRange;
                Excel.Range rngFound = rang.Find(oText, vk_missing, vk_missing, vk_missing, vk_missing, Excel.XlSearchDirection.xlNext, vk_missing, vk_missing, vk_missing);
                if (rngFound != null)
                {
                    rngResult = rngFound;
                    if ((bool)rngFound.MergeCells)
                    {
                        rngResult = rngFound.MergeArea;
                    }
                }
            }
            return rngResult;
        }

        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="rang"></param>
        /// <param name="fileName"></param>
        public void InsertPicture(Excel.Range rang, string fileName)
        {
            rang.Select();
            Excel.Pictures pics = (Excel.Pictures)excelWorksheet.Pictures(vk_missing);
            pics.Insert(fileName, vk_missing);
        }

        /// <summary>
        ///  插入图片
        /// </summary>
        /// <param name="rang"></param>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InsertPicture(Excel.Range rang, string fileName, float width, float height)
        {
            rang.Select();
            float picLeft = Convert.ToSingle(rang.Left);
            float picTop = Convert.ToSingle(rang.Top);
            excelWorksheet.Shapes.AddPicture(fileName, Office.MsoTriState.msoFalse, Office.MsoTriState.msoCTrue, picLeft, picTop, width, height);
        }

        /// <summary>
        ///  插入图片
        /// </summary>
        /// <param name="rang"></param>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InsertPicture(Excel.Range rang, Image image, int width, int height)
        {
            rang.Select();
            Graphics g = Graphics.FromImage(image);
            int w = Convert.ToInt32(width * g.DpiX / 72.0);
            int h = Convert.ToInt32(height * g.DpiY / 72.0);
            // w = Convert.ToInt32(1.0 * image.Width / image.Height * h);
            Bitmap bmp = new Bitmap(image, w - 2, h - 2);

            System.Windows.Forms.Clipboard.SetDataObject(bmp, true);
            excelWorksheet.Paste(rang, bmp);
            //float picLeft = Convert.ToSingle(rang.Left);
            //float picTop = Convert.ToSingle(rang.Top);
            //excelWorksheet.Shapes.AddPicture(fileName, Office.MsoTriState.msoFalse, Office.MsoTriState.msoCTrue, picLeft, picTop, width, height);
        }

        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="rang">插入区域</param>
        /// <param name="srcBitmap">图片源</param>
        public void InsertPicture(Excel.Range rang, Bitmap srcBitmap)
        {
            rang.Select();
            System.Windows.Forms.Clipboard.SetDataObject(srcBitmap, true);
            excelWorksheet.Paste(rang, srcBitmap);
        }

        /// <summary>
        /// 保护工作表
        /// </summary>
        /// <param name="passWord">密码</param>
        public void Protect(string passWord)
        {
            excelWorksheet.Protect(passWord, vk_missing, vk_missing, vk_missing, vk_true, vk_missing, vk_missing, vk_missing,
                vk_missing, vk_missing, vk_missing, vk_missing, vk_missing, vk_missing, vk_missing, vk_missing);
        }


        /// <summary>
        /// 保存报告
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAs(string fileName)
        {
            excelWorkbook.SaveAs(fileName, vk_missing, vk_missing, vk_missing, vk_missing, vk_missing, Excel.XlSaveAsAccessMode.xlExclusive, vk_missing, vk_missing, vk_missing, vk_missing, vk_missing);
        }

        /// <summary>
        /// 打印报告
        /// </summary>
        public void PrintReport()
        {
            excelWorksheet.PrintOut(vk_missing, vk_missing, vk_missing, vk_false, vk_missing, vk_false, vk_false, vk_missing);
        }

        #region 获取工作表
        public void GetExcelSheets()
        {
            if (this.excelWorkbook != null)
            {
                excelSheets = excelWorkbook.Worksheets;
                excelWorksheet = (Excel.Worksheet)excelSheets[1];
            }
        }
        #endregion
    }
}
