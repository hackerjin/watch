using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.Controls;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.Print
{
    public class BitmapsPrinter
    {
        /// <summary>
        /// 页数整型
        /// </summary>
        private int intPageIndex = 0;
        /// <summary>
        /// Bitmap集合
        /// </summary>
        private List<Bitmap> _lstBitmap;
        /// <summary>
        /// 打印预览
        /// </summary>
        private PrintPreviewDialog _PrintPreviewDlg;

        private PrintDialog printDialog;
        /// <summary>
        /// 打印文档
        /// </summary>
        private PrintDocument _PDC;
        /// <summary>
        /// 打印窗体
        /// </summary>
        private Form _Form;
        /// <summary>
        /// 打印默认设置属性
        /// </summary>
        private bool LandScape
        {
            get { return _PDC.DefaultPageSettings.Landscape; }
            set { _PDC.DefaultPageSettings.Landscape = value; }
        }

        //修改：何晓明 2010-02-14
        //原因：增加打印纸张 选择方向
        private PrintPage page;

        /// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="bitmaps"></param>
        //public BitmapsPrinter(List<Bitmap> bitmaps)
        //{
        //    InitComponent();
        //    _lstBitmap = bitmaps;
        //}


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bitmaps"></param>
        public BitmapsPrinter(List<Bitmap> bitmaps,PrintPage page)
        {
            InitComponent(page);
            _lstBitmap = bitmaps;
            this.page = page;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitComponent(PrintPage page)
        {
            _PDC = new PrintDocument();
            _PDC.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

            _PrintPreviewDlg = new PrintPreviewDialog();

            //修改：何晓明 2011-02-22
            //原因：增加打印机及打印纸张
            //_PDC.PrinterSettings.PrinterName = page.strDefaultPrinter;           
            //

            //修改：何晓明 2010-02-14
            //原因：增加打印纸张 选择纸张
            foreach (System.Drawing.Printing.PaperSize ps in _PDC.PrinterSettings.PaperSizes)
            {
                //修改：何晓明 2011-02-22
                //原因：增加打印机及打印纸张
                //if (ps.PaperName.Equals(page.PaperSize.ToString()))
                if(ps.PaperName.Equals(page.strPaperSize))
                //
                {
                    _PDC.DefaultPageSettings.PaperSize = ps;
                    LandScape = page.Dir == PrintDirection.Vertical ? true : false;
                    break;
                }
            }
            //
            printDialog = new PrintDialog();
            printDialog.AllowSomePages = true;
            printDialog.ShowHelp = true;

           
            _PrintPreviewDlg.Document = _PDC;

            _Form = (_PrintPreviewDlg as Form);
            _Form.MinimizeBox = true;
            _Form.ShowIcon = false;
        }

        /// <summary>
        /// 打印事件 在打印预览时调用生成预览文件及打印动作执行时调用生成打印文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_lstBitmap == null) return;

            //修改：何晓明 2010-02-14
            //原因：增加打印纸张缩放bitmap
            _lstBitmap = PrintHelper.GetThumbnailBitmaps(_lstBitmap, 
                page.Dir == PrintDirection.Vertical ?_PDC.DefaultPageSettings.PaperSize.Height:_PDC.DefaultPageSettings.PaperSize.Width, 
                page.Dir == PrintDirection.Vertical ?_PDC.DefaultPageSettings.PaperSize.Width:_PDC.DefaultPageSettings.PaperSize.Height);
            //
            if (intPageIndex < _lstBitmap.Count)
            {
                e.Graphics.DrawImage(_lstBitmap[intPageIndex], new Point(0, 0));

                intPageIndex++;
                if (intPageIndex != _lstBitmap.Count)
                {
                    e.HasMorePages = true;
                }

                //Update by HeXiaoMing 2010-12-07 17:54
                //Reason: Debug Print Button Active
                if (!e.HasMorePages)
                {
                    intPageIndex = 0;
                }
                //

            }
        }

        #region 公开方法
        /// <summary>
        /// 打印预览
        /// </summary>
        public void PrintPreview()
        {
           //修改：何晓明 2011-02-21
           //原因：打印机异常报错且无法与语言对应
           //操作：异常捕捉
            try
            {
                _PrintPreviewDlg.ShowDialog();
            }
            catch
            {
                Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.PrintExceptionMessage);
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        public void Print()
        {
            //修改：何晓明 2011-02-21
            //原因：打印机异常报错且无法与语言对应
           //操作：异常捕捉
            try
            {
                
                if (printDialog.ShowDialog() != DialogResult.OK)
                    return;
                    _PDC.PrinterSettings = printDialog.PrinterSettings;
                    
                _PDC.Print();
            }
            catch
            {
                Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.PrintExceptionMessage);
            }
        }

        #endregion

    }
}
