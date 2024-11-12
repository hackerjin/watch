using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.Print
{
    /// <summary>
    /// PDF导出类
    /// </summary>
    public class PDFExporter : IExporter
    {
        /// <summary>
        /// Bitmap集合属性
        /// </summary>
        public List<Bitmap> _lstBitmaps;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bts"></param>
        public PDFExporter(List<Bitmap> bts)
        {
            _lstBitmaps = bts;
        }

        #region IExporter 成员

        /// <summary>
        /// 实现IExport接口成员 导出文件 
        /// </summary>
        public void Export()
        {
            string fileName = GetFileName();
            if (fileName != string.Empty)
            {
                if (_lstBitmaps != null)
                {
                    var pdfDoc = GetPDFDoc();
                    if (pdfDoc != null)
                    {
                        pdfDoc.Save(fileName);
                        UIHelper.AskToOpenFile(fileName);
                    }
                }
            }
        }

        public void SavePDF(string fileName)
        {
            if (_lstBitmaps != null)
            {
                var pdfDoc = GetPDFDoc();
                if (pdfDoc != null)
                {
                    pdfDoc.Save(fileName);
                    //UIHelper.AskToOpenFile(fileName);
                }
            }
        }

        private Aspose.Pdf.Pdf GetPDFDocNew()
        {
            Aspose.Pdf.Pdf pdf = new Aspose.Pdf.Pdf();
            Aspose.Pdf.Section sec = pdf.Sections.Add();

            Aspose.Pdf.Image image = new Aspose.Pdf.Image(sec);
            image.ImageInfo.SystemImage = _lstBitmaps[0];
            image.ImageInfo.ImageFileType = Aspose.Pdf.ImageFileType.Bmp;
            sec.Paragraphs.Add(image);
            pdf.Sections.Add(sec);

            return pdf;
        }


        /// <summary>
        /// 实现IExport接口成员 获取文件名
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return UIHelper.GetFileToSaveName("PDF File (*.pdf)|*.pdf");
        }

        #endregion

        /// <summary>
        /// 生成PDF文档
        /// </summary>
        /// <returns></returns>
        private Aspose.Pdf.Pdf GetPDFDoc()
        {
            //创建PDF文档并设置大小
            Aspose.Pdf.Pdf pdf1 = new Aspose.Pdf.Pdf();

            pdf1.PageSetup.PageWidth = Aspose.Pdf.PageSize.A4Width;
            pdf1.PageSetup.PageHeight = Aspose.Pdf.PageSize.A4Height;
            //pdf1.DestinationType = Aspose.Pdf.DestinationType.FitWidth;

            Aspose.Pdf.Image image1;

            //PDF页面
            Aspose.Pdf.Section sec1;
            Bitmap newBm = null;
            foreach (Bitmap bm in _lstBitmaps)
            {
                if (bm.Width > Aspose.Pdf.PageSize.A4Width)
                    pdf1.PageSetup.PageWidth = bm.Width;
                    //newBm = ScaleBitmap(bm, (int)Aspose.Pdf.PageSize.A4Width, (int)(bm.Height * Aspose.Pdf.PageSize.A4Width / bm.Width));
                else if (bm.Height > Aspose.Pdf.PageSize.A4Height)
                    pdf1.PageSetup.PageHeight = bm.Height;
                    //newBm = ScaleBitmap(bm, (int)(bm.Width * Aspose.Pdf.PageSize.A4Height / bm.Height), (int)Aspose.Pdf.PageSize.A4Height);
                //else
                //    newBm = bm;
                //新增PDF页面
                sec1 = pdf1.Sections.Add();
                //设置边距
                sec1.PageInfo.Margin.Left = 0;
                sec1.PageInfo.Margin.Right = 0;
                sec1.PageInfo.Margin.Top = 0;
                sec1.PageInfo.Margin.Bottom = 0;

                //定义PDF格式的Image对象
                image1 = new Aspose.Pdf.Image(sec1);
                image1.IsFirstParagraph = true;
                image1.IsFirstParagraphInColumn = true;
                image1.IsImageFitToRowHeight = true;
                //设置页面相关参数
                image1.ImageInfo.SystemImage = bm;
                image1.ImageInfo.ImageFileType = Aspose.Pdf.ImageFileType.Bmp;
                image1.ImageInfo.Alignment = Aspose.Pdf.AlignmentType.Center;

                //将图像添加至页面
                sec1.Paragraphs.Add(image1);
            }
            return pdf1;
        }

        public static Bitmap ScaleBitmap(Bitmap bitmapSource, int newWidth, int newHeight)
        {
            Bitmap newBitmap = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(newBitmap);
            //设置高质量插值法   
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; 
            //设置高质量,低速度呈现平滑程度   
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; 
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality; 
            //消除锯齿 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawImage(bitmapSource, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, bitmapSource.Width, bitmapSource.Height), GraphicsUnit.Pixel);
            g.Dispose();
            bitmapSource.Dispose();
            return newBitmap;
        }
    }
}
