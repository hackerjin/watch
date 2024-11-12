using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SourceGrid;
namespace Skyray.Print
{
    public class ImageExporter : IExporter
    {
        public PageExporter PageExporter { get; set; }
        private List<Bitmap> _LstBitmap;

        public List<Bitmap> LstBitmap
        {
            get { return _LstBitmap; }
            set { _LstBitmap = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exporter"></param>
        public ImageExporter(PrintPage page) : this(new PageExporter(page)) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exporter"></param>
        public ImageExporter(PageExporter pageExporter)
        {
            PageExporter = pageExporter;
            _LstBitmap = GetPageBitmaps();
        }

        private bool HasCtrlInBody(List<CellControl> page)
        {
            var printPage = PageExporter.Page;
            return page.Exists(ctrl => ctrl.Point.Y.IsBetween(printPage.HeaderHeight, printPage.MaxY));
        }
        /// <summary>
        /// 获取页面对应的Bitmap集合
        /// </summary>
        /// <returns></returns>
        public List<Bitmap> GetPageBitmaps()
        {
            var lstBitmap = new List<Bitmap>();
            var pages = PageExporter.GetPages();
            if (pages == null || pages.Count == 0)
                return null;
            //int z = 0;
            foreach (var page in pages)
            {
                if (page.Count > 0)//页面内有控件
                {
                    Bitmap bitmap = PageExporter.Page.GetBitmap();
                    int i = 0;
                    foreach (var c in page)
                    {

                        var typ = c.PrintCtrl.GetType();

                        var typ1 = c.PrintCtrl.Type;

                        //修改：2010-12-14 何晓明
                        //原因：Feild字段为空时类似一个Label导致排版错乱
                        //var rect = typ1 == CtrlType.Label ? c.LabelRect
                        // : typ1 == CtrlType.Field ? c.FieldRect : c.ClientRect;
                        var rect = new Rectangle();
                        if (typ1 == CtrlType.Label)
                        {
                            rect = c.LabelRect;
                        }
                        else if (typ1 == CtrlType.Field)
                        {
                            rect = c.FieldRect;
                            if (rect == new Rectangle(new Point(0, 0), new Size(0, 0)))
                            {
                                rect = c.LabelRect;
                            }
                        }
                        else if (typ1 == CtrlType.ComposeTable)
                        {
                            rect = new Rectangle(c.Point, c.Size);
                        }
                        else
                        {
                            rect = c.ClientRect;
                        }
                        //

                        if (typ == typeof(PrintTable))
                        {
                            i++;
                            if (c.Types[0] == CtrlType.Label)
                            {
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    //修改： 2010-12-14 何晓明
                                    //原因：取出列换行时的表头
                                    //if (i < page.Count - 1)
                                    //{
                                   // PointF pointF = new PointF(page[i].ClientRect.Left, page[i].ClientRect.Top);
                                   // if (page[i].Types[0] != CtrlType.Label)
                                   //     g.DrawString(c.PrintCtrl.Text, c.PrintCtrl.TextFont,
                                   //new Pen(c.PrintCtrl.TextColor).Brush, c.Point);

                                    //}
                                }
                            }
                            else
                            {
                                var pt = (c.PrintCtrl as PrintTable);
                                var grid = pt.CreateGrid(c.ColIndexs, c.RowIndexs, false);
                                if (grid != null)
                                {
                                    grid.DrawToBitmap(bitmap, rect);

                                    //修改： 2010-12-14 何晓明
                                    //原因：取出列换行时的表头
                                    List<int> lstColumns = (List<int>)c.ColIndexs;
                                    //修改：何晓明 2011-03-03
                                    //原因：首列隐藏后表头丢失
                                    int iMinColumnNum = 0;
                                    for (int iColumnNum = 0; iColumnNum < pt.ColCount;iColumnNum++ )
                                    {
                                        if (pt.ColInfos[iColumnNum].Visiable == true)
                                        {
                                            iMinColumnNum = iColumnNum;
                                            break;
                                        }
                                    }
                                    
                                    int iStartColumn = lstColumns[0];
                                    if(iStartColumn == iMinColumnNum)
                                    //int iStartColumn = lstColumns[0];
                                    //if (iStartColumn == 0)
                                    //
                                    {
                                        //修改：何晓明 2011-02-21
                                        //原因：表格字体较大时高度大于行高，影响布局
                                        //操作：注释
                                        //Size sizeAdd = new Size();
                                        //sizeAdd.Height = (c.PrintCtrl as PrintTable).RowHeight * (-1);
                                        //sizeAdd.Width = 0;

                                        using (Graphics g = Graphics.FromImage(bitmap))
                                        {
                                            //修改：何晓明 2011-03-21
                                            //原因：表格字体较大时高度大于行高，影响布局 PrintTable设置文字表格间距
                                            Size sizeAdd = new Size();                                            
                                            //sizeAdd.Height =(int) g.MeasureString(c.PrintCtrl.Text, pt.TextFont).Height*(-1);
                                            sizeAdd.Height = (pt.TextVSpace + (int)g.MeasureString(c.PrintCtrl.Text, pt.TextFont).Height) * (-1);
                                            //
                                            sizeAdd.Width = 0;
                                            //
                                            g.DrawString(c.PrintCtrl.Text, c.PrintCtrl.TextFont,
                                       new Pen(c.PrintCtrl.TextColor).Brush, c.Point + sizeAdd);
                                        }
                                    }
                                }
                            }
                                     
                        }
                        else if (typ == typeof(PrintCompositeTable))
                        {
                            if (c.Types[0] != CtrlType.Label)
                            {
                                var pt = c.PrintCtrl as PrintCompositeTable;
                                List<Grid> listGrid = new List<Grid>();
                                for (int k = 0; k < pt.DTable.Count; k++)
                                    listGrid.Add(pt.CreateGrid(k));
                                int z = 0;
                                int LocationX = c.Point.X;
                                int LocationY = c.Point.Y;
                                for (int g = 0; g < c.ClientRect.Height / pt.Height; g++)
                                {
                                    if (z < listGrid.Count)
                                    {
                                        Rectangle rectCompositeTable = new Rectangle(c.ClientRect.Left, c.ClientRect.Top + g * pt.Height, c.ClientRect.Width, listGrid[z].Height);
                                        listGrid[z].DrawToBitmap(bitmap, rectCompositeTable);
                                        z++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            c.PrintCtrl.CtrlToBitmap(bitmap, rect.Top);
                        }
                    }
                    lstBitmap.Add(bitmap);
                }
            }
            return lstBitmap;
        }

        public void Export()
        {
            string fileName = GetFileName();
            if (fileName != string.Empty && _LstBitmap != null && _LstBitmap.Count > 0)
            {
                int count = _LstBitmap.Count;
                var firstBitmap = _LstBitmap[0];
                int h = firstBitmap.Height;
                int w = firstBitmap.Width;

                Bitmap bm = new Bitmap(w, h * count);

                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.Clear(Color.White);
                    for (int i = 0; i < count; i++)
                    {
                        g.DrawImage(_LstBitmap[i], new Point(0, i * h));
                    }
                }
                bm.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                UIHelper.AskToOpenFile(fileName);
            }
        }

        public string GetFileName()
        {
            return UIHelper.GetFileToSaveName("JPEG File (*.jpg)|*.jpg");
        }
    }
}
