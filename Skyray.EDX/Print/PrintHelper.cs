using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Globalization;
using SourceGrid;
using Skyray.Controls;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Printing;
using DevAge.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Xml;
using Skyray.Language;

namespace Skyray.Print
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class PrintHelper
    {
        public static string curreEdition = "";//当前软件版本
        /// <summary>
        /// 获取纸张大小
        /// </summary>
        /// <param name="paperSize"></param>
        /// <returns></returns>
        public static Size GetPaperSize(PaperSize paperSize)
        {
            Size size = Size.Empty;//变量定义
            switch (paperSize)
            {
                //修改：何晓明 2011-02-12
                //原因：增加纸张类型 
                case PaperSize.A3:
                    size.Width = 1169;
                    size.Height = 1654;
                    break;
                case PaperSize.A4:
                    size.Width = 738;
                    size.Height = 1126;
                    break;
                case PaperSize.A5:
                    size.Width = 583;
                    size.Height = 827;
                    break;
                case PaperSize.A6:
                    size.Width = 413;
                    size.Height = 583;
                    break;
                case PaperSize.B4:
                    size.Width = 1012;
                    size.Height = 1433;
                    break;
                case PaperSize.B5:
                    size.Width = 717;
                    size.Height = 1012;
                    break;
                case PaperSize.B6:
                    size.Width = 504;
                    size.Height = 717;
                    break;
                default: break;
            }
            return size;//返回
        }
        //修改：何晓明 2011-02-22 
        //原因：增加打印机及打印纸张
        /// <summary>
        /// 获取纸张大小
        /// </summary>
        /// <param name="strPaperSize">纸张大小名称</param>
        /// <returns></returns>
        public static Size GetPaperSize(string strPaperSize)
        {
            Size size = new Size();
            try
            {                
                System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
                foreach (System.Drawing.Printing.PaperSize ps in doc.PrinterSettings.PaperSizes)
                {
                    //找不到对应纸张时默认使用A4纸打开
                    if (ps.PaperName == strPaperSize)
                    {
                        doc.DefaultPageSettings.PaperSize = ps;
                    }
                }
                size = new Size(doc.DefaultPageSettings.PaperSize.Width, doc.DefaultPageSettings.PaperSize.Height);
            }
            catch
            {
                //未安装打印机
                size.Width = 738;
                size.Height = 1126;
            }
            return size;
        }
        //
        /// <summary>
        ///打开用户控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="TitleName"></param>
        public static void OpenUC(UserControl control, string TitleName)
        {
            OpenUC(new Form(), control, false, TitleName);
        }

        /// <summary>
        /// 打开用户控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="flag">是否最大最小化</param>
        public static void OpenUC(Form form, UserControl control, bool flag, string TitleName)
        {
            form.BackColor = Color.White;
            int padSpace = 0;
            form.Padding = new System.Windows.Forms.Padding(padSpace, padSpace, padSpace, padSpace);
            form.Controls.Add(control);
            form.MaximizeBox = flag;
            form.Text = TitleName;
            if (!flag)
            {
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
                form.MaximizeBox = false;//不可以最小化
            }
            //设置客户区域大小
            form.ClientSize = new Size(control.Width + padSpace * 2, control.Height + padSpace * 2);
            control.Dock = DockStyle.Fill;//填充
            form.StartPosition = FormStartPosition.CenterScreen;//起始位置在屏幕中央
            form.ShowInTaskbar = false;
            //修改：何晓明 2011-03-15
            //原因：函数打开窗口允许最大最小化
            form.MinimizeBox = flag;
            //form.MinimizeBox = false;
            //
            form.ShowIcon = false;
            //form.ShowDialog();//显示
            Skyray.XmlLang.UIHelper.OpenModelDlg(form);
        }

        /// <summary>
        /// 获得单元格视图
        /// </summary>
        /// <param name="tableStyle"></param>
        /// <param name="IsHeader"></param>
        /// <returns></returns>
        public static SourceGrid.Cells.Views.Cell GetCellView(ITableStyle tableStyle, bool IsHeader)
        {
            var viewCell = new SourceGrid.Cells.Views.Cell();
            viewCell.Font = IsHeader ? tableStyle.HeaderFont : tableStyle.CellFont;
            viewCell.ForeColor = IsHeader ? tableStyle.HeaderColor : tableStyle.CellColor;
            viewCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            if (IsHeader) viewCell.BackColor = tableStyle.HeaderBackColor;
            return viewCell;
        }

        /// <summary>
        /// 检验打印机是否存在
        /// </summary>
        /// <returns></returns>        
        public static bool IsPrinterExist()
        {
            //检测打印机个数
            return PrinterSettings.InstalledPrinters.Count > 0;
        }

        public static bool IsDefaultPrinterValid()
        {
            bool bBack = false;
            if (!IsPrinterExist()) return false;
            try
            {
              System.Drawing.Printing.PrintDocument doc = new PrintDocument();
             string printerName = doc.PrinterSettings.PrinterName;
             System.Printing.PrintServer ps = new System.Printing.PrintServer(printerName);
             System.Printing.LocalPrintServer lps = new System.Printing.LocalPrintServer();
             var queue = lps.DefaultPrintQueue;
                queue.AddJob();
                bBack = true;
            }
            catch(Exception e)
            {
                bBack = false;
                SkyrayMsgBox.Show(e.Message);
            }

            return bBack;
        }

        /// <summary>
        /// 深度克隆方法，传入对象必须是可以序列化的
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>       
        public static object DeepClone(this object obj)
        {
            object value;
            using (MemoryStream ms = new MemoryStream())//定义内存流
            {
                BinaryFormatter bf = new BinaryFormatter();//二进制序列化对象
                bf.Serialize(ms, obj);//序列化
                ms.Seek(0, SeekOrigin.Begin);               
                value = bf.Deserialize(ms);//反序列化
            }
            return value;
        }

        /// <summary>
        /// 矩形转换，将宽高负值转化为正值
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Rectangle ConvertRect(Rectangle rect)
        {
            if ((rect.Width < 0) && (rect.Height > 0))
            {
                rect = new Rectangle(rect.Left + rect.Width, rect.Top, -rect.Width, rect.Height);
            }
            else if ((rect.Width > 0) && (rect.Height < 0))
            {
                rect = new Rectangle(rect.Left, rect.Top + rect.Height, rect.Width, -rect.Height);
            }
            else if ((rect.Width < 0) && (rect.Height < 0))
            {
                rect = new Rectangle(rect.Left + rect.Width, rect.Top + rect.Height, -rect.Width, -rect.Height);
            }
            return rect;
        }

        /// <summary>
        /// 文件转化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static T FileToObj<T>(string strFileName)
        {
            try
            {
                Stream serializationStream = File.Open(strFileName, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();//二进制序列化器
                object obj = formatter.Deserialize(serializationStream);
                serializationStream.Close();
                return (T)obj;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return default(T);//失败则返回默认值
            }
        }

        /// <summary>
        /// 将对象序列化为文件，如果失败返回false
        /// </summary>
        /// <param name="objToSerialization"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static bool ObjToFile(object objToSerialization, string strFileName)
        {
            bool bSuccess = true;//记录是否保存成功 
            using (MemoryStream serializationStream = new MemoryStream())
            {
                try
                {
                    new BinaryFormatter().Serialize(serializationStream, objToSerialization);
                }
                catch
                {
                    bSuccess = false;
                }
                if (bSuccess)
                {
                    using (FileStream fs = new FileStream(strFileName, FileMode.Create))
                    {
                        serializationStream.WriteTo(fs);
                        fs.Flush();
                        SaveSpecifiedValueandCreate(strFileName);
                    }
                }
            }
            //提示用户
            SkyrayMsgBox.Show(bSuccess ? PrintInfo.SaveSuccess : PrintInfo.SaveFailed);//提示成功
            return bSuccess;
        }

        #region 保存模板时，设置其参数

        public static void SaveSpecifiedValueandCreate(string NewTemplateName)
        {
            //获取当前默认模板参数设置
            string currTemplateName = LoadReportName();

            //获取当前语言
            Languages CurrentLang = Languages.FindOne(l => l.IsCurrentLang == true);
            string Language = CurrentLang.ShortName;
            if (Language.ToLower() == "english") Language = "EN";
            else if (Language.ToLower() == "chinese") Language = "CN";

            //获取当先默认的打印模板的对象参数
            XmlNode currXmlNodes = null;
            getXmlNode(Language, currTemplateName, ref currXmlNodes);

            //判断需要新增的模板名称是否存在template.xml文件中，如果存在，则直接删除
            string newTemplateName = "";
            string[] tname = NewTemplateName.Split('\\');
            newTemplateName = tname[tname.Length-1];
            deleXmlNode(newTemplateName);

            //添加新节点
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\PrintTemplate\\template.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("application/ReportTemplate/" + curreEdition + "/" + Language);
                if (rootNode == null) return;
                XmlElement newxmlElement = doc.CreateElement("TemplateName");
                for (int i = 0; i < currXmlNodes.Attributes.Count;i++ )
                {
                    newxmlElement.SetAttribute(currXmlNodes.Attributes[i].Name, currXmlNodes.Attributes[i].Value); 
                }
                newxmlElement.InnerText = newTemplateName;
                rootNode.AppendChild(newxmlElement);
                doc.Save(path);

            }



        }

        private static void getXmlNode(string CurrentLang, string TemplateName, ref XmlNode findXmlNodes)
        {
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\PrintTemplate\\template.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList rootNodeList = doc.SelectNodes("application/ReportTemplate/" + curreEdition + "/" + CurrentLang);
                if (rootNodeList == null) return;
                foreach(XmlNode xNode in rootNodeList)
                    if (xNode.ChildNodes != null && xNode.ChildNodes.Count > 0 && findXmlNodes == null)
                        foreach(XmlNode childNodes in xNode.ChildNodes)
                            if (childNodes.InnerText == TemplateName)
                            {
                                findXmlNodes = childNodes;
                                return;
                            }              
                
            }
        }

        private static void deleXmlNode(string TemplateName)
        {
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\PrintTemplate\\template.xml";
            if (File.Exists(path))
            {
                Dictionary<int, XmlNode> deleXmlNode = new Dictionary<int,XmlNode>();
                doc.Load(path);
                XmlNodeList rootNodeList = doc.GetElementsByTagName("TemplateName");
                if (rootNodeList == null) return;
                foreach(XmlNode xmlNode in rootNodeList)
                    if (xmlNode.InnerText == TemplateName)
                    {
                        deleXmlNode.Add(deleXmlNode.Count + 1, xmlNode);
                    }
                foreach (int i in deleXmlNode.Keys)
                {
                    XmlNode dxmlNode = deleXmlNode[i];
                    XmlNode PxmlNode = dxmlNode.ParentNode;
                    PxmlNode.RemoveChild(dxmlNode);
                }
                doc.Save(path);

                return;
                
            }
        }

        public static string LoadReportName()
        {
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

        #endregion


        /// <summary>
        /// 获取Pen对象
        /// </summary>
        /// <param name="color"></param>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Pen GetPen(Color color, DashStyle style, int width)
        {
            Pen pen = new Pen(color);
            pen.DashStyle = style;
            pen.Width = width;
            return pen;
        }


        /// <SUMMARY>
        /// 图片无损缩放
        /// </SUMMARY>
        /// <PARAM name="sourceFile">图片源路径</PARAM>
        /// <PARAM name="destFile">缩放后图片输出路径</PARAM>
        /// <PARAM name="destHeight">缩放后图片高度</PARAM>
        /// <PARAM name="destWidth">缩放后图片宽度</PARAM>
        /// <RETURNS></RETURNS>
        public static Stream GetThumbnail(Image imgSource, Size imageSize)
        {
            //MemoryStream ms = new MemoryStream();
            //var image = imgSource.GetThumbnailImage(imageSize.Width, imageSize.Height, null, IntPtr.Zero);
            //image.Save(ms, ImageFormat.Jpeg);
            //return ms;
            MemoryStream ms = new MemoryStream();
            Image newImage = new Bitmap(imageSize.Width, imageSize.Height);
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImage(imgSource, new Rectangle(0, 0, imageSize.Width, imageSize.Height), new Rectangle(0, 0, imgSource.Width, imgSource.Height), GraphicsUnit.Pixel);

            //var image = imgSource.GetThumbnailImage(imageSize.Width, imageSize.Height, null, IntPtr.Zero);
            newImage.Save(ms, ImageFormat.Jpeg);
            newImage.Dispose();
            return ms;
        }

        //修改：何晓明 2010-01-14 
        //原因：PNG格式LOGO导出背景色变成黑色
        /// <SUMMARY>
        /// 图片无损缩放
        /// </SUMMARY>
        /// <PARAM name="sourceFile">图片源路径</PARAM>
        /// <PARAM name="destFile">缩放后图片输出路径</PARAM>
        /// <PARAM name="destHeight">缩放后图片高度</PARAM>
        /// <PARAM name="destWidth">缩放后图片宽度</PARAM>
        /// <param name="imgFormat">图片格式</param>
        /// <RETURNS></RETURNS>
        public static Stream GetThumbnail(Image imgSource, Size imageSize, ImageFormat imgFormat)
        {
            MemoryStream ms = new MemoryStream();
            Image newImage = new Bitmap(imageSize.Width, imageSize.Height);
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImage(imgSource, new Rectangle(0, 0, imageSize.Width, imageSize.Height), new Rectangle(0, 0, imgSource.Width, imgSource.Height), GraphicsUnit.Pixel);
            //修改：何晓明 2010-01-17
            //原因：值不能为空 参数 encoder
            newImage.Save(ms,ImageFormat.Png);
            //newImage.Save(ms, imgFormat);
            //
            newImage.Dispose();
            return ms;
        }
        //修改：何晓明 2010-02-21
        //原因：图片及边框导出Excel
        /// <summary>
        /// 图片无损缩放
        /// </summary>
        /// <param name="imgSource">图片源</param>
        /// <param name="imageSize">图片大小</param>
        /// <param name="borderColor">边框颜色</param>
        /// <returns></returns>
        public static Stream GetThumbnailWithBorder(Image imgSource, Size imageSize, Color borderColor)
        {
            MemoryStream ms = new MemoryStream();
            Image newImage = new Bitmap(imageSize.Width, imageSize.Height);
            Graphics g = Graphics.FromImage(newImage);
            Rectangle rec = new Rectangle(0, 0, imgSource.Width, imgSource.Height);            
            g.DrawImage(imgSource, new Rectangle(0, 0, imageSize.Width-2, imageSize.Height-2), rec, GraphicsUnit.Pixel);
            g.DrawRectangle(PrintHelper.GetPen(borderColor, DashStyle.Solid, 1), new Rectangle (0,0,imageSize.Width-1,imageSize.Height-1));
            newImage.Save(ms, ImageFormat.Png);
            newImage.Dispose();
            return ms;
        }

        //修改：何晓明 2010-02-14
        //原因：增加打印纸张缩放bitmap
        /// <summary>
        /// 缩放bitmap
        /// </summary>
        /// <param name="bitmap">源bitmap</param>
        /// <param name="iNewWidth">新宽度</param>
        /// <param name="iNewHeight">新高度</param>
        /// <returns></returns>
        public static Bitmap GetThumbnailBitmap(Bitmap bitmap,int iNewWidth,int iNewHeight)
        {
            try
            {
                Bitmap b = new Bitmap(iNewWidth, iNewHeight);
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap,new Rectangle(0,0,iNewWidth,iNewHeight),new Rectangle(0,0,bitmap.Width ,bitmap.Height),GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 缩放bitmap集合
        /// </summary>
        /// <param name="lstBitmap">源bitmap集合</param>
        /// <param name="iNewWidth">新宽度</param>
        /// <param name="iNewHeight">新高度</param>
        /// <returns></returns>
        public static List<Bitmap> GetThumbnailBitmaps(List<Bitmap> lstBitmap,int iNewWidth,int iNewHeight)
        {
            try
            {
                List<Bitmap> lstReturn = new List<Bitmap>();
                foreach (Bitmap bitmap in lstBitmap)
                {
                    lstReturn.Add(GetThumbnailBitmap(bitmap,iNewWidth,iNewHeight));
                }
                return lstReturn;
            }
            catch
            {
                return null;
            }
        }
        //
    }
}

