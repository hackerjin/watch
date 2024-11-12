using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Data;
using SourceGrid;
using Skyray.Controls;
using System.ComponentModel;
using System.Drawing.Printing;
using Aspose.Cells;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;

namespace Skyray.Print
{
    /// <summary>
    /// 预览控件
    /// </summary>
    public class PreviewControl
    {
        public PrintPage Page;
        public PreviewControl(PrintPage page)
        {
            Page = page;
        }

        /// <summary>
        /// 导出至PDF文档
        /// </summary>
        public void ExportToPdf()
        {
            try
            {
                ImageExporter imageImporter = new ImageExporter(new PageExporter(Page));
                PDFExporter exporter = new PDFExporter(imageImporter.GetPageBitmaps());
                exporter.Export();
            }
            catch (System.Exception ex)
            {
                SkyrayMsgBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 导出至Excel文档
        /// </summary>
        public void ExportToExcel()
        {
            ExcelExporter exporter = new ExcelExporter(new PageExporter(Page));
            exporter.Export();
        }

        /// <summary>
        /// 直接导出
        /// </summary>
        /// <param name="path">传入字符串型保存路径</param>
        public void DirectExportToExcel(string path)
        {
            ExcelExporter exporter = new ExcelExporter(new PageExporter(Page));
            exporter.DirectExport(path);
        }
        //修改：何晓明 2011-01-17
        //原因：按指定名称保存源数据至EXCEL
        /// <summary>
        /// 直接导出
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">名称，不需指定后缀名默认xls</param>
        public void DirectExportToExcel(string path, string fileName)
        {
            ExcelExporter exporter = new ExcelExporter(new PageExporter(Page));
            exporter.DirectExport(path, fileName);
        }
        #region 打印

        /// <summary>
        /// 打印
        /// </summary>
        public void Print()
        {
            try
            {
                if (PrintHelper.IsPrinterExist())
                {
                    ImageExporter imageImporter = new ImageExporter(new PageExporter(Page));
                    BitmapsPrinter bitmapPrinter = new BitmapsPrinter(imageImporter.GetPageBitmaps(),this.Page);
                    bitmapPrinter.Print();
                }
                else
                {
                    SkyrayMsgBox.Show(PrintInfo.NoPrinter);
                }
            }
            catch (Exception ex)
            {
                SkyrayMsgBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        public void PrintPreview()
        {
            //try
            {
                if (PrintHelper.IsPrinterExist())
                {
                    ImageExporter imageImporter = new ImageExporter(new PageExporter(Page));
                    //修改：何晓明 2011-02-14
                    //原因：增加纸张类型 传入页面信息
                    //BitmapsPrinter bitmapPrinter = new BitmapsPrinter(imageImporter.GetPageBitmaps());
                    BitmapsPrinter bitmapPrinter = new BitmapsPrinter(imageImporter.GetPageBitmaps(),this.Page);
                    //
                    bitmapPrinter.PrintPreview();
                }
                else
                {
                    SkyrayMsgBox.Show(PrintInfo.NoPrinter);
                }
            }
            //catch (Exception ex)
            //{
            //    SkyrayMsgBox.Show(ex.Message);
            //}
        }

        #endregion
    }
}