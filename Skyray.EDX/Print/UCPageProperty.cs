using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Skyray.Print
{
    /// <summary>
    /// 页面事件
    /// </summary>
    public partial class UCPageProperty :Skyray.Language.UCMultiple
    {
        private PrintPage _Page;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="page"></param>
        public UCPageProperty(PrintPage page)
            : this()
        {
            _Page = page;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCPageProperty()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSavePageSet_Click(object sender, EventArgs e)
        {
            if (_Page == null) return;
            //页面边距
            _Page.PrintMargin = new Padding((int)numLeft.Value,
                                            (int)numTop.Value,
                                            (int)numRight.Value,
                                            (int)numBottom.Value);
            //打印方向
            _Page.Dir = (PrintDirection)cboPrintDir.SelectedIndex;

            //纸张类型
            //修改：何晓明 2011-02-14 
            //原因：增加打印纸张
            _Page.PaperSize = PaperSize.A4;//修改：何晓明 2010-02-22 增加打印机及打印纸张之维持页面不变//(PaperSize)cboPaperSize.SelectedIndex;

            //修改：何晓明 2011-02-22
            //原因：增加打印机及打印纸张
            SetDefaultPrinter(cboPrinter.Text);
            _Page.strPaperSize = cboPaperSize.Text;
            _Page.strDefaultPrinter = cboPrinter.Text;
            //

            //设计器大小
            _Page.PageSize = (PageSize)cboPageSize.SelectedIndex;

            _Page.SetPrintPageParam();
            
            CancelButtonClick();//Cancel
        }
        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelPageSet_Click(object sender, EventArgs e)
        {
            CancelButtonClick();
        }
        /// <summary>
        /// 取消事件
        /// </summary>
        public virtual void CancelButtonClick()
        {
            var form = this.ParentForm;
            if (form != null) form.Close();
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCPageProperty_Load(object sender, EventArgs e)
        {
            if (_Page == null) return;
            //var items = Enum.GetNames(typeof(PaperSize));
            //修改：何晓明 2011-02-15
            //原因：纸张类型按打印机纸张类型确定
            PrintDocument doc = new PrintDocument();
            //var test = doc.PrinterSettings.PaperSizes;
            //foreach (System.Drawing.Printing.PaperSize item in test)
            //    cboPaperSize.Items.Add(item.PaperName);//纸张大小            
            //cboPaperSize.Items.RemoveAt(cboPaperSize.Items.Count - 1);
            //

            //修改：何晓明 2011-02-22
            //原因：增加打印机及打印纸张
            string strDefaultPrinter = doc.PrinterSettings.PrinterName;
            foreach (string strPrinter in PrinterSettings.InstalledPrinters)
            {
                cboPrinter.Items.Add(strPrinter);
                if (strPrinter == strDefaultPrinter)
                {
                    cboPrinter.SelectedIndex = cboPrinter.Items.IndexOf(strDefaultPrinter);
                }
            }
            //页面加载
            cboPrinter_SelectedIndexChanged(sender, e);
            //

            var items = Enum.GetNames(typeof(PrintDirection));
            foreach (var item in items) cboPrintDir.Items.Add(item);//打印方向

            items = Enum.GetNames(typeof(PageSize));
            foreach (var item in items) cboPageSize.Items.Add(item);//页面大小           

            numLeft.Value = _Page.PrintMargin.Left;
            numRight.Value = _Page.PrintMargin.Right;
            numTop.Value = _Page.PrintMargin.Top;
            numBottom.Value = _Page.PrintMargin.Bottom;

            //修改：何晓明 2011-02-22
            //原因：增加打印机及打印纸张
            //cboPaperSize.SelectedIndex = (int)_Page.PaperSize;
            cboPaperSize.SelectedIndex = cboPaperSize.Items.IndexOf(_Page.strPaperSize);
            if (cboPaperSize.Items.IndexOf("A4") != -1 && cboPaperSize.Text.Trim() == "")
            {
                cboPaperSize.SelectedIndex = cboPaperSize.Items.IndexOf("A4");
            }
            //
            cboPrintDir.SelectedIndex = (int)_Page.Dir;
            cboPageSize.SelectedIndex = (int)_Page.PageSize;
        }
        /// <summary>
        /// 选择索引改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboPaperSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //修改：何晓明 2011-02-15
            //原因：纸张类型按打印机纸张类型确定
            //PaperSize paperSize = (PaperSize)cboPaperSize.SelectedIndex;            
            //var size = PrintHelper.GetPaperSize(paperSize);
            //lblPaperSize.Text = size.Width + "*" + size.Height;

            //numLeft.Maximum = numRight.Maximum = size.Width / 6;
            //numTop.Maximum =
            //    _Page.ShowHeader ? _Page.Header.Height - 30 : size.Height / 6;
            //numBottom.Maximum =
            //   _Page.ShowFooter ? _Page.Footer.Height - 30 : size.Height / 6;

            //修改：何晓明 2011-02-22
            //原因：增加打印机及打印纸张
            string strPaperSize = _Page.strPaperSize;
            var size = PrintHelper.GetPaperSize(strPaperSize);
            lblPaperSize.Text = size.Width + "*" + size.Height;

            numLeft.Maximum = numRight.Maximum = size.Width / 6;
            numTop.Maximum =
                _Page.ShowHeader ? _Page.Header.Height - 30 : size.Height / 6;
            numBottom.Maximum =
               _Page.ShowFooter ? _Page.Footer.Height - 30 : size.Height / 6;
        }


        //修改：何晓明 2011-02-22
        //原因：增加打印机及打印纸张
        /// <summary>
        /// 打印机选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboPaperSize.Items.Clear();
            PrintDocument doc = new PrintDocument();
            doc.PrinterSettings.PrinterName = cboPrinter.Text;
            foreach (System.Drawing.Printing.PaperSize ps in doc.PrinterSettings.PaperSizes)
            {
                cboPaperSize.Items.Add(ps.PaperName);
                if (ps.PaperName.Contains("自"))
                {
                    cboPaperSize.Items.Remove(ps.PaperName);
                }
                //if (cboPaperSize.Items.IndexOf("A4") != -1 && cboPaperSize.Text.Trim() == "")
                //{
                //    cboPaperSize.SelectedIndex = cboPaperSize.Items.IndexOf("A4");
                //}
            }            
        }
        /// <summary>
        /// 设置默认打印机
        /// </summary>
        /// <param name="printerName">打印机名称</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("winspool.drv", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string printerName);
        //
    }
}