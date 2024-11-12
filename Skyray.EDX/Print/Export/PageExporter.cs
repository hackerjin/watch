using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Skyray.Print
{
    /// <summary>
    /// 页面导出类
    /// </summary>
    public class PageExporter
    {
        private PrintPage _Page;
        
        /// <summary>
        /// PrintPage类型属性
        /// </summary>
        public PrintPage Page
        {
            get { return _Page; }
            set { _Page = value; }
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="printPage"></param>
        public PageExporter(PrintPage printPage)
        {
            _Page = printPage;
        }

        #region 私有变量
        
        /// <summary>
        /// 当前游标
        /// </summary>
        private int currentY = 0;
        
        /// <summary>
        /// CellControl类型集合
        /// </summary>
        private List<CellControl> page;

        /// <summary>
        /// CellControl类型集合列表
        /// </summary>
        private List<List<CellControl>> lstPage;

        /// <summary>
        /// CellControl类型数组
        /// </summary>
        private CellControl[] lstCellCtrls;
        #endregion

        #region 私有方法
        
        /// <summary>
        /// 换页
        /// </summary>
        private void TrunPage()
        {
            currentY = _Page.HeaderHeight;
            NewPage();
        }

        private void TrunPage1()
        {
            currentY = Page.Height * PageCurrent;
            if (_Page.ShowHeader)
            {
                foreach (var cellCtrl in _Page.GetHeaderCellControls())
                {
                    AddCellCtrl(page, cellCtrl, cellCtrl.Point.Y + currentY);
                }
                currentY += _Page.HeaderHeight;
            }
            if (_Page.ShowFooter)
            {
                foreach (var cellCtrl in _Page.GetFootCellControls())
                {
                    AddCellCtrl(page, cellCtrl, cellCtrl.Point.Y + (Page.MaxY+Page.FooterHeight) * PageCurrent);
                }
            }
            PageCurrent++;
        }

        /// <summary>
        /// 新建页面
        /// </summary>
        private void NewPage()
        {
            page = new List<CellControl>();
            //修改：何晓明 2011-02-21
            //原因：不显示页眉预览及打印中仍然显示
            //foreach (var cellCtrl in _Page.GetHeaderCellControls())
            //{
            //    AddCellCtrl(page, cellCtrl, cellCtrl.Point.Y);
            //}
            if (_Page.ShowHeader)
            {
                foreach (var cellCtrl in _Page.GetHeaderCellControls())
                {
                    AddCellCtrl(page, cellCtrl, cellCtrl.Point.Y);
                }
            }
            //

            foreach (var cellCtrl in _Page.GetFootCellControls())
            {
                AddCellCtrl(page, cellCtrl, cellCtrl.Point.Y);
            }
            //lstPage.Add(page);
            lstPage.Add(page);
        }

        /// <summary>
        /// 当前游标增加
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool AddCurrentY(int y)
        {
            bool bTurnPage = false;
            int temp = currentY + y;
            if (temp > _Page.MaxY)
            {
                //修改：2010-12-15 何晓明
                //原因：行换页导致中间多出一页空白
                //TrunPage();//换页
                bTurnPage = true;//
            }
            else
            {
                currentY = temp;
            }
            return bTurnPage;
        }

        #endregion

        private List<int> GetVisableCol(ColInfo[] colInfos, IndexInfo colIndexInfo, ref int width)
        {
            List<int> lst = new List<int>();
            for (int i = colIndexInfo.Start; i < colIndexInfo.End; i++)
            {
                if (colInfos[i].Visiable)
                {
                    width += colInfos[i].Width;                    
                    lst.Add(i);
                }
            }
            lst.TrimExcess();
            return lst;
        }

        private int PageCurrent = 1;

        /// <summary>
        /// 页面增加CellControl类
        /// </summary>
        /// <param name="page">传入CellControl集合</param>
        /// <param name="cellCtrl">传入CellControl类</param>
        /// <param name="y"></param>
        private void AddCellCtrl(List<CellControl> page, CellControl cellCtrl, int y)
        {
            var ctrlToDraw = cellCtrl.PrintCtrl;
            var typ = ctrlToDraw.Type;
            bool bHasText = !string.IsNullOrEmpty(ctrlToDraw.Text);

            int left = cellCtrl.Point.X;
            int h = ctrlToDraw.TitleRect.Height;
            var pnt = new Point(left, y);

            int w = ctrlToDraw.TitleRect.Width;

            switch (typ)
            {
                case CtrlType.Field:
                    var fieldCtrl = ctrlToDraw as PrintField;
                    if (!string.IsNullOrEmpty(fieldCtrl.TextValue))
                    {
                        var fieldPoint = new Point(left + fieldCtrl.BaseRect.Width - fieldCtrl.ValueSize.Width, y);
                        var fieldSize = new Size(fieldCtrl.ValueSize.Width, h);
                        cellCtrl.Types[1] = CtrlType.Field;
                        cellCtrl.FieldRect = new Rectangle(fieldPoint, fieldSize);
                    }
                    w = fieldCtrl.TextSize.Width;
                    break;
                case CtrlType.Image:
                    var imageCtrl = ctrlToDraw as PrintImage;
                    if (imageCtrl.Image != null)
                    {
                        int iLeft = PageCurrent*_Page.MaxY - y;
                        if (iLeft < imageCtrl.Height)
                            TrunPage1();
                        cellCtrl.Types[1] = CtrlType.Image;
                        var imagePoint = new Point(left, cellCtrl.RelativeY + currentY + h);
                        var imageSize = new Size(imageCtrl.BaseRect.Width, imageCtrl.ContentRect.Height);
                        cellCtrl.ClientRect = new Rectangle(imagePoint, imageSize);
                    }
                    break;
                case CtrlType.Grid:
                    var tableCtrl = ctrlToDraw as PrintTable;
                    bHasText = bHasText && tableCtrl.Table.Rows.Count > 0;
                    break;
                case CtrlType.ComposeTable:
                    var compositeTable = ctrlToDraw as PrintCompositeTable;
                    bHasText = bHasText && compositeTable.grid.Rows.Count > 0;
                    break;
                default:
                    break;
            }

            if (bHasText)
            {
                cellCtrl.Types[0] = CtrlType.Label;
                var sizeOfText = new Size(w, h);
                cellCtrl.LabelRect = new Rectangle(pnt, sizeOfText);
            }

            page.Add(cellCtrl);
        }

        #region
        /// <summary>
        /// 获取页数
        /// </summary>
        /// <returns></returns>
        public List<List<CellControl>> GetPages()
        {
            lstCellCtrls = _Page.GetBodyCellControls();
            if (lstCellCtrls.Length == 0)
                return null;
            //throw new Exception(PrintInfo.TemplateNoData);

            var groups = _Page.GetGroupInfo(lstCellCtrls);

            currentY = 0;
            lstPage = new List<List<CellControl>>();
            NewPage();
            //修改：何晓明 2011-03-10
            //原因：一个组中表格数量多于一个时出错
            int iGroupTableCount = 0;
            //
            for (int i = 0; i < groups.Count; i++)
            {
                currentY += groups[i].StartY;//开始绘制纵坐标点

                int bottomY = currentY + groups[i].Height;
                //paul 不进行分页 
                if (bottomY > _Page.MaxY * PageCurrent) TrunPage1();
                //if (bottomY > _Page.MaxY) TrunPage();

                var ctrlIndex = groups[i].CtrlIndex;
                CellControl tableCell = null;
                CellControl cellCtrl = null;
                bool flag = false;
                for (int k = ctrlIndex.Start; k < ctrlIndex.End; k++)
                {
                    cellCtrl = lstCellCtrls[k];
                    AddCellCtrl(page, cellCtrl, cellCtrl.RelativeY + currentY);
                    if (cellCtrl.PrintCtrl.Type == CtrlType.Grid)
                    {
                        if (tableCell != null)
                        {
                            //throw new Exception(PrintInfo.GroupErr);
                            flag = true;
                            //Update by HeXiaoMing 2010-12-08
                            //Reason: 拖动两个以上表格时如果先托第二个后托第一个出现分组异常导致空白
                            //break;
                            //
                        }
                        tableCell = cellCtrl;
                    }

                    //修改：何晓明 2011-03-10
                    //原因：一个组中表格数量多于一个时出错
                    #region 表格处理
                    if (tableCell != null && tableCell.PrintCtrl.Type == CtrlType.Grid)
                    {
                        PrintTable tableToDraw = null;
                        if (tableCell != null)
                        {
                            tableToDraw = tableCell.PrintCtrl as PrintTable;
                        }

                        if (tableToDraw == null || tableToDraw.Table.Rows.Count == 0)
                        {
                            currentY += groups[i].Height;//该组控件不包含表格

                        }
                        else
                        {
                            int left = tableCell.Point.X;
                            AddCurrentY(tableCell.RelativeY);//表格起始纵坐标

                            iGroupTableCount++;

                            for (int j = 0; j < tableToDraw.Ints.Count; j++)//列分行
                            {
                                var colInfo = tableToDraw.Ints[j];
                                int width = 0;
                                var visableCol = GetVisableCol(tableToDraw.ColInfos, colInfo, ref width);
                                //修改：何晓明 2011-03-17
                                //原因：导出EXCEL另外分页
                                //var lstRows = tableToDraw.GetRowIndex(_Page.MaxY, currentY);
                                var lstRows = tableToDraw.GetRowIndexToExcel(_Page.MaxY, currentY);
                                //
                                for (int r = 0; r < lstRows.Count; r++)//行分页
                                {
                                    if (r > 0) TrunPage();

                                    var splitRowIndex = lstRows[r];
                                    int h0 = r == 0 && j == 0 ? tableToDraw.TitleRect.Height : 0;
                                    int topGrid = h0 + currentY;

                                    //修改：2010-12-21 何晓明
                                    //原因：表格内数据自动换行
                                    int h = tableToDraw.ColHeight
                                        + splitRowIndex.Count * tableToDraw.RowHeight + h0;
                                    //System.Data.DataTable dtTable = tableToDraw.Table;
                                    //int iRowNewCount = 0;
                                    //int iPageRowCount = 0;
                                    //for (int iRows = splitRowIndex.Start; iRows <= splitRowIndex.End; iRows++)
                                    //{
                                    //    for (int iColumns = 0; iColumns < dtTable.Columns.Count; iColumns++)
                                    //    {
                                    //        string strValue = dtTable.Rows[iRows][iColumns].ToString();
                                    //        List<string> lstNewRows = SplitStringByColumn(strValue, new PrintCell("").View.Font, tableToDraw.ColInfos[j].Width);
                                    //        if (lstNewRows.Count > iRowNewCount)
                                    //        {
                                    //            iRowNewCount = lstNewRows.Count;
                                    //        }
                                    //    }
                                    //    iPageRowCount += iRowNewCount;
                                    //}
                                    //int h = tableToDraw.ColHeight
                                    //    + iPageRowCount * tableToDraw.RowHeight + h0;
                                    //

                                    var point = new Point(left, topGrid);

                                    var size = new Size(width, h);

                                    var cellctrl1 = new CellControl
                                    {
                                        PrintCtrl = tableToDraw,
                                        ColIndexs = visableCol,
                                        Point = point,
                                        Size = size,
                                        RowIndexs = splitRowIndex,
                                        ClientRect = new Rectangle { Location = point, Size = size },
                                        Types = new CtrlType[] { CtrlType.None, CtrlType.Grid }
                                    };

                                    page.Add(cellctrl1);
                                    //修改：2010-12-21 何晓明
                                    //原因：表格内数据自动换行
                                    //if (r != lstRows.Count - 1) h += tableToDraw.TableSpace;
                                    //
                                    if (groups[i].Tables.Count <= 1 || iGroupTableCount == groups[i].Tables.Count)
                                        AddCurrentY(h);
                                    //修改：2010-12-16 何晓明
                                    //原因：当行分页至整好一页时进行列分行时列不分页导致重叠bug修正
                                    if (r == lstRows.Count - 1 && cellctrl1.ClientRect.Bottom > _Page.MaxY)
                                        TrunPage();
                                }
                                AddCurrentY(tableToDraw.TableSpace);
                            }
                            tableCell = null;
                        }
                    }
                    #endregion

                    if (cellCtrl.PrintCtrl.Type == CtrlType.ComposeTable)
                        tableCell = cellCtrl;
                }
                //Update by HeXiaoMing 2011-01-19
                //Reason: 表格重叠时导致分组异常提醒
                if (flag)
                {
                    System.Windows.Forms.MessageBox.Show(PrintInfo.TableVSpaceError, PrintInfo.TableVSpaceTip, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    //continue;
                }

                if (tableCell != null && tableCell.PrintCtrl.Type == CtrlType.ComposeTable)
                {
                    PrintCompositeTable compositeTable = tableCell.PrintCtrl as PrintCompositeTable;
                    compositeTable.ClearSelection();
                    tableCell.Point = new Point(tableCell.Point.X, currentY);
                    int iCompositeTableCount = compositeTable.DTable.Count;
                    int iLeft = _Page.MaxY - currentY;
                    int iPageFirstCount = iLeft / compositeTable.Height;

                    int tempCount = (iCompositeTableCount <= iPageFirstCount ? iCompositeTableCount : iPageFirstCount);
                    var pt = tableCell.PrintCtrl as PrintCompositeTable;
                    var rowHight = pt.CellInfo[0].RowsInfo[0].RowHeight;
                    List<SourceGrid.Grid> listGrid = new List<SourceGrid.Grid>();
                    for (int k = 0; k < pt.DTable.Count; k++)
                        listGrid.Add(pt.CreateGrid(k));
                    int h = 0;
                    for (int iGrid = 0; iGrid < listGrid.Count; iGrid++)
                    {
                        for (int iRow = 0; iRow < listGrid[iGrid].Rows.Count; iRow++)
                        {
                            h += listGrid[iGrid].Rows[iRow].Height;
                            //Size size1 = new Size(compositeTable.Width, h * tempCount + 30);
                            //if (iLeft < size1.Height)
                            //{
                            //    h = 0;
                            //    TrunPage1();
                            //    Point point = new Point(compositeTable.Location.X, currentY);
                            //    var cellctrl1 = new CellControl
                            //    {
                            //        PrintCtrl = compositeTable,
                            //        ClientRect = new Rectangle { Location = point, Size = size1 },
                            //        Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
                            //    };
                            //    page.Add(cellctrl1);
                            //}
                        }
                    }
                    Size size2 = new Size(compositeTable.Width, h * tempCount + 30);
                    Point point2 = new Point(compositeTable.Location.X, currentY);
                    var cellctrl2 = new CellControl
                    {
                        PrintCtrl = compositeTable,
                        ClientRect = new Rectangle { Location = point2, Size = size2 },
                        Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
                    };
                    page.Add(cellctrl2);
                    AddCurrentY(h);
                    int iCountPerPage = (_Page.MaxY - _Page.HeaderHeight) / compositeTable.Height;
                    if (iPageFirstCount < iCompositeTableCount)
                    {
                        int modCount = (iCompositeTableCount - iPageFirstCount) % iCountPerPage;
                        int multCount = (iCompositeTableCount - iPageFirstCount) / iCountPerPage;
                        while (multCount > 0)
                        {
                            TrunPage1();
                            Point point = new Point(compositeTable.Location.X, currentY);
                            Size sizePage = new Size(compositeTable.Width, _Page.MaxY);
                            var cellctrlPage = new CellControl
                            {
                                PrintCtrl = compositeTable,
                                ClientRect = new Rectangle { Location = point, Size = sizePage },
                                Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
                            };
                            page.Add(cellctrlPage);
                            multCount--;
                        }

                        TrunPage1();
                        Point point3 = new Point(compositeTable.Location.X, currentY);
                        Size size3 = new Size(compositeTable.Width, compositeTable.grid.Height * modCount);
                        var cellctrl3 = new CellControl
                        {
                            PrintCtrl = compositeTable,
                            ClientRect = new Rectangle { Location = point3, Size = size3 },
                            Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
                        };
                        page.Add(cellctrl2);
                    }

                }
                else if (tableCell != null && tableCell.PrintCtrl.Type == CtrlType.Grid)
                {

                    //修改：何晓明 2011-03-10
                    //原因：一个组中表格数量多于一个时出错
                    //#region 表格处理

                    //PrintTable tableToDraw = null;
                    //if (tableCell != null)
                    //{
                    //    tableToDraw = tableCell.PrintCtrl as PrintTable;
                    //}

                    //if (tableToDraw == null || tableToDraw.Table.Rows.Count == 0)
                    //{
                    //    currentY += groups[i].Height;//该组控件不包含表格

                    //}
                    //else
                    //{
                    //    int left = tableCell.Point.X;
                    //    AddCurrentY(tableCell.RelativeY);//表格起始纵坐标
                    //    for (int j = 0; j < tableToDraw.Ints.Count; j++)//列分行
                    //    {
                    //        var colInfo = tableToDraw.Ints[j];
                    //        int width = 0;
                    //        var visableCol = GetVisableCol(tableToDraw.ColInfos, colInfo, ref width);

                    //        var lstRows = tableToDraw.GetRowIndex(_Page.MaxY, currentY);
                    //        for (int r = 0; r < lstRows.Count; r++)//行分页
                    //        {
                    //            if (r > 0) TrunPage();

                    //            var splitRowIndex = lstRows[r];
                    //            int h0 = r == 0 && j == 0 ? tableToDraw.TitleRect.Height : 0;
                    //            int topGrid = h0 + currentY;

                    //            //修改：2010-12-21 何晓明
                    //            //原因：表格内数据自动换行
                    //            //int h = tableToDraw.ColHeight
                    //            //    + splitRowIndex.Count * tableToDraw.RowHeight + h0;
                    //            System.Data.DataTable dtTable = tableToDraw.Table;
                    //            int iRowNewCount = 0;
                    //            int iPageRowCount = 0;
                    //            for (int iRows = splitRowIndex.Start; iRows <= splitRowIndex.End; iRows++)
                    //            {
                    //                for (int iColumns = 0; iColumns < dtTable.Columns.Count; iColumns++)
                    //                {
                    //                    string strValue = dtTable.Rows[iRows][iColumns].ToString();
                    //                    List<string> lstNewRows = SplitStringByColumn(strValue, new PrintCell("").View.Font, tableToDraw.ColInfos[j].Width);
                    //                    if (lstNewRows.Count > iRowNewCount)
                    //                    {
                    //                        iRowNewCount = lstNewRows.Count;
                    //                    }
                    //                }
                    //                iPageRowCount += iRowNewCount;
                    //            }
                    //            int h = tableToDraw.ColHeight
                    //                + iPageRowCount * tableToDraw.RowHeight + h0;
                    //            //

                    //            var point = new Point(left, topGrid);

                    //            var size = new Size(width, h);

                    //            var cellctrl1 = new CellControl
                    //            {
                    //                PrintCtrl = tableToDraw,
                    //                ColIndexs = visableCol,
                    //                Point = point,
                    //                Size = size,
                    //                RowIndexs = splitRowIndex,
                    //                ClientRect = new Rectangle { Location = point, Size = size },
                    //                Types = new CtrlType[] { CtrlType.None, CtrlType.Grid }
                    //            };

                    //            page.Add(cellctrl1);
                    //            //修改：2010-12-21 何晓明
                    //            //原因：表格内数据自动换行
                    //            //if (r != lstRows.Count - 1) h += tableToDraw.TableSpace;
                    //            //
                    //            AddCurrentY(h);
                    //            //修改：2010-12-16 何晓明
                    //            //原因：当行分页至整好一页时进行列分行时列不分页导致重叠bug修正
                    //            if (r == lstRows.Count - 1 && cellctrl1.ClientRect.Bottom + _Page.MaxY / 10 > _Page.MaxY)
                    //                TrunPage();
                    //        }
                    //        AddCurrentY(tableToDraw.TableSpace);
                    //    }
                    //}

                    //#endregion
                }
                else
                {
                    //修改：何晓明 2011-03-21
                    //原因：导出EXCEL表格留白过多
                    if (cellCtrl != null && cellCtrl.PrintCtrl.Type != CtrlType.Grid)
                        //
                        currentY += groups[i].Height;
                }
            }
            return lstPage;
        }

        #region 备注
        //public List<List<CellControl>> GetPages()
        //{
        //    lstCellCtrls = _Page.GetBodyCellControls();
        //    if (lstCellCtrls.Length == 0)
        //        return null;
        //    //throw new Exception(PrintInfo.TemplateNoData);

        //    var groups = _Page.GetGroupInfo(lstCellCtrls);

        //    currentY = 0;
        //    lstPage = new List<List<CellControl>>();
        //    NewPage();
        //    //修改：何晓明 2011-03-10
        //    //原因：一个组中表格数量多于一个时出错
        //    int iGroupTableCount = 0;
        //    //
        //    for (int i = 0; i < groups.Count; i++)
        //    {
        //        currentY += groups[i].StartY;//开始绘制纵坐标点

        //        int bottomY = currentY + groups[i].Height;
        //        if (bottomY > _Page.MaxY) TrunPage();

        //        var ctrlIndex = groups[i].CtrlIndex;
        //        CellControl tableCell = null;
        //        CellControl cellCtrl = null;
        //        bool flag = false;
        //        for (int k = ctrlIndex.Start; k < ctrlIndex.End; k++)
        //        {
        //            cellCtrl = lstCellCtrls[k];
        //            AddCellCtrl(page, cellCtrl, cellCtrl.RelativeY + currentY);
        //            if (cellCtrl.PrintCtrl.Type == CtrlType.Grid)
        //            {
        //                if (tableCell != null)
        //                {
        //                    //throw new Exception(PrintInfo.GroupErr);
        //                    flag = true;
        //                    //Update by HeXiaoMing 2010-12-08
        //                    //Reason: 拖动两个以上表格时如果先托第二个后托第一个出现分组异常导致空白
        //                    //break;
        //                    //
        //                }
        //                tableCell = cellCtrl;
        //            }

        //            //修改：何晓明 2011-03-10
        //            //原因：一个组中表格数量多于一个时出错
        //            #region 表格处理
        //            if (tableCell != null && tableCell.PrintCtrl.Type == CtrlType.Grid)
        //            {
        //                PrintTable tableToDraw = null;
        //                if (tableCell != null)
        //                {
        //                    tableToDraw = tableCell.PrintCtrl as PrintTable;
        //                }

        //                if (tableToDraw == null || tableToDraw.Table.Rows.Count == 0)
        //                {
        //                    currentY += groups[i].Height;//该组控件不包含表格

        //                }
        //                else
        //                {
        //                    int left = tableCell.Point.X;
        //                    AddCurrentY(tableCell.RelativeY);//表格起始纵坐标

        //                    iGroupTableCount++;

        //                    for (int j = 0; j < tableToDraw.Ints.Count; j++)//列分行
        //                    {
        //                        var colInfo = tableToDraw.Ints[j];
        //                        int width = 0;
        //                        var visableCol = GetVisableCol(tableToDraw.ColInfos, colInfo, ref width);

        //                        var lstRows = tableToDraw.GetRowIndex(_Page.MaxY, currentY);
        //                        for (int r = 0; r < lstRows.Count; r++)//行分页
        //                        {
        //                            if (r > 0) TrunPage();

        //                            var splitRowIndex = lstRows[r];
        //                            int h0 = r == 0 && j == 0 ? tableToDraw.TitleRect.Height : 0;
        //                            int topGrid = h0 + currentY;

        //                            //修改：2010-12-21 何晓明
        //                            //原因：表格内数据自动换行
        //                            //int h = tableToDraw.ColHeight
        //                            //    + splitRowIndex.Count * tableToDraw.RowHeight + h0;
        //                            System.Data.DataTable dtTable = tableToDraw.Table;
        //                            int iRowNewCount = 0;
        //                            int iPageRowCount = 0;
        //                            for (int iRows = splitRowIndex.Start; iRows <= splitRowIndex.End; iRows++)
        //                            {
        //                                //for (int iColumns = 0; iColumns < dtTable.Columns.Count; iColumns++)
        //                                for (int iColumns = 0; iColumns < visableCol.Count; iColumns++)
        //                                {
        //                                    //string strValue = dtTable.Rows[iRows][iColumns].ToString();
        //                                    //List<string> lstNewRows = SplitStringByColumn(strValue, new PrintCell("").View.Font, tableToDraw.ColInfos[j].Width);
        //                                    string strValue = dtTable.Rows[iRows][visableCol[iColumns]].ToString();
        //                                    List<string> lstNewRows = SplitStringByColumn(strValue, new PrintCell("").View.Font, tableToDraw.ColInfos[visableCol[iColumns]].Width);
        //                                    if (lstNewRows.Count > iRowNewCount)
        //                                    {
        //                                        iRowNewCount = lstNewRows.Count;
        //                                    }
        //                                }
        //                                iPageRowCount += iRowNewCount;
        //                                iRowNewCount = 0;
        //                            }
        //                            int h = tableToDraw.ColHeight
        //                                + iPageRowCount * tableToDraw.RowHeight + h0;
        //                            //int h = tableToDraw.ColHeight + (splitRowIndex.End-splitRowIndex.Start) * tableToDraw.RowHeight + h0;
        //                            //

        //                            var point = new Point(left, topGrid);

        //                            var size = new Size(width, h);

        //                            var cellctrl1 = new CellControl
        //                            {
        //                                PrintCtrl = tableToDraw,
        //                                ColIndexs = visableCol,
        //                                Point = point,
        //                                Size = size,
        //                                RowIndexs = splitRowIndex,
        //                                ClientRect = new Rectangle { Location = point, Size = size },
        //                                Types = new CtrlType[] { CtrlType.None, CtrlType.Grid }
        //                            };

        //                            page.Add(cellctrl1);
        //                            //修改：2010-12-21 何晓明
        //                            //原因：表格内数据自动换行
        //                            //if (r != lstRows.Count - 1) h += tableToDraw.TableSpace;
        //                            //
        //                            if (groups[i].Tables.Count <= 1 || iGroupTableCount == groups[i].Tables.Count)
        //                                AddCurrentY(h);
        //                            //修改：2010-12-16 何晓明
        //                            //原因：当行分页至整好一页时进行列分行时列不分页导致重叠bug修正
        //                            if (r == lstRows.Count - 1 && cellctrl1.ClientRect.Bottom > _Page.MaxY)
        //                                TrunPage();
        //                        }
        //                        AddCurrentY(tableToDraw.TableSpace);
        //                    }
        //                    //tableCell = null;
        //                }
        //            }
        //            #endregion

        //            if (cellCtrl.PrintCtrl.Type == CtrlType.ComposeTable)
        //                tableCell = cellCtrl;
        //        }
        //        //Update by HeXiaoMing 2011-01-19
        //        //Reason: 表格重叠时导致分组异常提醒
        //        //if (flag)
        //        //{
        //        //    System.Windows.Forms.MessageBox.Show(PrintInfo.TableVSpaceError, PrintInfo.TableVSpaceTip, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        //        //    //continue;
        //        //}

        //        if (tableCell != null && tableCell.PrintCtrl.Type == CtrlType.ComposeTable)
        //        {
        //            PrintCompositeTable compositeTable = tableCell.PrintCtrl as PrintCompositeTable;
        //            compositeTable.ClearSelection();
        //            tableCell.Point = new Point(tableCell.Point.X, currentY);
        //            int iCompositeTableCount = compositeTable.DTable.Count;
        //            int iLeft = _Page.MaxY - currentY;
        //            int iPageFirstCount = iLeft / compositeTable.Height;
        //            Point point = new Point(compositeTable.Location.X, currentY);
        //            int tempCount = (iCompositeTableCount <= iPageFirstCount ? iCompositeTableCount : iPageFirstCount);
        //            Size size1 = new Size(compositeTable.Width, compositeTable.Height * tempCount);
        //            var cellctrl1 = new CellControl
        //            {
        //                PrintCtrl = compositeTable,
        //                ClientRect = new Rectangle { Location = point, Size = size1 },
        //                Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
        //            };
        //            page.Add(cellctrl1);
        //            AddCurrentY(compositeTable.Height * tempCount);

        //            int iCountPerPage = (_Page.MaxY - _Page.HeaderHeight) / compositeTable.Height;
        //            if (iPageFirstCount < iCompositeTableCount)
        //            {
        //                int modCount = (iCompositeTableCount - iPageFirstCount) % iCountPerPage;
        //                int multCount = (iCompositeTableCount - iPageFirstCount) / iCountPerPage;
        //                while (multCount > 0)
        //                {
        //                    TrunPage();
        //                    point = new Point(compositeTable.Location.X, currentY);
        //                    Size sizePage = new Size(compositeTable.Width, _Page.MaxY);
        //                    var cellctrlPage = new CellControl
        //                    {
        //                        PrintCtrl = compositeTable,
        //                        ClientRect = new Rectangle { Location = point, Size = sizePage },
        //                        Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
        //                    };
        //                    page.Add(cellctrlPage);
        //                    multCount--;
        //                }

        //                TrunPage();
        //                point = new Point(compositeTable.Location.X, currentY);
        //                Size size2 = new Size(compositeTable.Width, compositeTable.Height * modCount);
        //                var cellctrl2 = new CellControl
        //                {
        //                    PrintCtrl = compositeTable,
        //                    ClientRect = new Rectangle { Location = point, Size = size2 },
        //                    Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
        //                };
        //                page.Add(cellctrl2);
        //                AddCurrentY(compositeTable.Height * modCount);
        //            }

        //        }
        //        else if (tableCell != null && tableCell.PrintCtrl.Type == CtrlType.Grid)
        //        {

        //            //修改：何晓明 2011-03-10
        //            //原因：一个组中表格数量多于一个时出错
        //            //#region 表格处理

        //            //PrintTable tableToDraw = null;
        //            //if (tableCell != null)
        //            //{
        //            //    tableToDraw = tableCell.PrintCtrl as PrintTable;
        //            //}

        //            //if (tableToDraw == null || tableToDraw.Table.Rows.Count == 0)
        //            //{
        //            //    currentY += groups[i].Height;//该组控件不包含表格

        //            //}
        //            //else
        //            //{
        //            //    int left = tableCell.Point.X;
        //            //    AddCurrentY(tableCell.RelativeY);//表格起始纵坐标
        //            //    for (int j = 0; j < tableToDraw.Ints.Count; j++)//列分行
        //            //    {
        //            //        var colInfo = tableToDraw.Ints[j];
        //            //        int width = 0;
        //            //        var visableCol = GetVisableCol(tableToDraw.ColInfos, colInfo, ref width);

        //            //        var lstRows = tableToDraw.GetRowIndex(_Page.MaxY, currentY);
        //            //        for (int r = 0; r < lstRows.Count; r++)//行分页
        //            //        {
        //            //            if (r > 0) TrunPage();

        //            //            var splitRowIndex = lstRows[r];
        //            //            int h0 = r == 0 && j == 0 ? tableToDraw.TitleRect.Height : 0;
        //            //            int topGrid = h0 + currentY;

        //            //            //修改：2010-12-21 何晓明
        //            //            //原因：表格内数据自动换行
        //            //            //int h = tableToDraw.ColHeight
        //            //            //    + splitRowIndex.Count * tableToDraw.RowHeight + h0;
        //            //            System.Data.DataTable dtTable = tableToDraw.Table;
        //            //            int iRowNewCount = 0;
        //            //            int iPageRowCount = 0;
        //            //            for (int iRows = splitRowIndex.Start; iRows <= splitRowIndex.End; iRows++)
        //            //            {
        //            //                for (int iColumns = 0; iColumns < dtTable.Columns.Count; iColumns++)
        //            //                {
        //            //                    string strValue = dtTable.Rows[iRows][iColumns].ToString();
        //            //                    List<string> lstNewRows = SplitStringByColumn(strValue, new PrintCell("").View.Font, tableToDraw.ColInfos[j].Width);
        //            //                    if (lstNewRows.Count > iRowNewCount)
        //            //                    {
        //            //                        iRowNewCount = lstNewRows.Count;
        //            //                    }
        //            //                }
        //            //                iPageRowCount += iRowNewCount;
        //            //            }
        //            //            int h = tableToDraw.ColHeight
        //            //                + iPageRowCount * tableToDraw.RowHeight + h0;
        //            //            //

        //            //            var point = new Point(left, topGrid);

        //            //            var size = new Size(width, h);

        //            //            var cellctrl1 = new CellControl
        //            //            {
        //            //                PrintCtrl = tableToDraw,
        //            //                ColIndexs = visableCol,
        //            //                Point = point,
        //            //                Size = size,
        //            //                RowIndexs = splitRowIndex,
        //            //                ClientRect = new Rectangle { Location = point, Size = size },
        //            //                Types = new CtrlType[] { CtrlType.None, CtrlType.Grid }
        //            //            };

        //            //            page.Add(cellctrl1);
        //            //            //修改：2010-12-21 何晓明
        //            //            //原因：表格内数据自动换行
        //            //            //if (r != lstRows.Count - 1) h += tableToDraw.TableSpace;
        //            //            //
        //            //            AddCurrentY(h);
        //            //            //修改：2010-12-16 何晓明
        //            //            //原因：当行分页至整好一页时进行列分行时列不分页导致重叠bug修正
        //            //            if (r == lstRows.Count - 1 && cellctrl1.ClientRect.Bottom + _Page.MaxY / 10 > _Page.MaxY)
        //            //                TrunPage();
        //            //        }
        //            //        AddCurrentY(tableToDraw.TableSpace);
        //            //    }
        //            //}

        //            //#endregion
        //        }
        //        else
        //        {
        //            currentY += groups[i].Height;
        //        }
        //    }
        //    return lstPage;
        //}

        #endregion

        #endregion
        //修改：何晓明 2011-03-16
        //原因：导出EXCEL另外分页
        public List<List<CellControl>> GetPagesToExcel()
        {
            lstCellCtrls = _Page.GetBodyCellControls();
            if (lstCellCtrls.Length == 0)
                return null;
            //throw new Exception(PrintInfo.TemplateNoData);

            var groups = _Page.GetGroupInfo(lstCellCtrls);

            currentY = 0;
            lstPage = new List<List<CellControl>>();
            NewPage();
            //修改：何晓明 2011-03-10
            //原因：一个组中表格数量多于一个时出错
            int iGroupTableCount = 0;
            //
            for (int i = 0; i < groups.Count; i++)
            {
                currentY += groups[i].StartY;//开始绘制纵坐标点

                int bottomY = currentY + groups[i].Height;
                //paul 不进行分页 
                if (bottomY > _Page.MaxY*PageCurrent) TrunPage1();
                //if (bottomY > _Page.MaxY) TrunPage();

                var ctrlIndex = groups[i].CtrlIndex;
                CellControl tableCell = null;
                CellControl cellCtrl = null;
                bool flag = false;
                for (int k = ctrlIndex.Start; k < ctrlIndex.End; k++)
                {
                    cellCtrl = lstCellCtrls[k];
                    AddCellCtrl(page, cellCtrl, cellCtrl.RelativeY + currentY);
                    if (cellCtrl.PrintCtrl.Type == CtrlType.Grid)
                    {
                        if (tableCell != null)
                        {
                            //throw new Exception(PrintInfo.GroupErr);
                            flag = true;
                            //Update by HeXiaoMing 2010-12-08
                            //Reason: 拖动两个以上表格时如果先托第二个后托第一个出现分组异常导致空白
                            //break;
                            //
                        }
                        tableCell = cellCtrl;
                    }

                    //修改：何晓明 2011-03-10
                    //原因：一个组中表格数量多于一个时出错
                    #region 表格处理
                    if (tableCell != null && tableCell.PrintCtrl.Type == CtrlType.Grid)
                    {
                        PrintTable tableToDraw = null;
                        if (tableCell != null)
                        {
                            tableToDraw = tableCell.PrintCtrl as PrintTable;
                        }

                        if (tableToDraw == null || tableToDraw.Table.Rows.Count == 0)
                        {
                            currentY += groups[i].Height;//该组控件不包含表格

                        }
                        else
                        {
                            int left = tableCell.Point.X;
                            AddCurrentY(tableCell.RelativeY);//表格起始纵坐标

                            iGroupTableCount++;

                            for (int j = 0; j < tableToDraw.Ints.Count; j++)//列分行
                            {
                                var colInfo = tableToDraw.Ints[j];
                                int width = 0;
                                var visableCol = GetVisableCol(tableToDraw.ColInfos, colInfo, ref width);
                                //修改：何晓明 2011-03-17
                                //原因：导出EXCEL另外分页
                                //var lstRows = tableToDraw.GetRowIndex(_Page.MaxY, currentY);
                                var lstRows = tableToDraw.GetRowIndexToExcel(_Page.MaxY, currentY);
                                //
                                for (int r = 0; r < lstRows.Count; r++)//行分页
                                {
                                    if (r > 0) TrunPage();

                                    var splitRowIndex = lstRows[r];
                                    int h0 = r == 0 && j == 0 ? tableToDraw.TitleRect.Height : 0;
                                    int topGrid = h0 + currentY;

                                    //修改：2010-12-21 何晓明
                                    //原因：表格内数据自动换行
                                    int h = tableToDraw.ColHeight
                                        + splitRowIndex.Count * tableToDraw.RowHeight + h0;
                                    //System.Data.DataTable dtTable = tableToDraw.Table;
                                    //int iRowNewCount = 0;
                                    //int iPageRowCount = 0;
                                    //for (int iRows = splitRowIndex.Start; iRows <= splitRowIndex.End; iRows++)
                                    //{
                                    //    for (int iColumns = 0; iColumns < dtTable.Columns.Count; iColumns++)
                                    //    {
                                    //        string strValue = dtTable.Rows[iRows][iColumns].ToString();
                                    //        List<string> lstNewRows = SplitStringByColumn(strValue, new PrintCell("").View.Font, tableToDraw.ColInfos[j].Width);
                                    //        if (lstNewRows.Count > iRowNewCount)
                                    //        {
                                    //            iRowNewCount = lstNewRows.Count;
                                    //        }
                                    //    }
                                    //    iPageRowCount += iRowNewCount;
                                    //}
                                    //int h = tableToDraw.ColHeight
                                    //    + iPageRowCount * tableToDraw.RowHeight + h0;
                                    //

                                    var point = new Point(left, topGrid);

                                    var size = new Size(width, h);

                                    var cellctrl1 = new CellControl
                                    {
                                        PrintCtrl = tableToDraw,
                                        ColIndexs = visableCol,
                                        Point = point,
                                        Size = size,
                                        RowIndexs = splitRowIndex,
                                        ClientRect = new Rectangle { Location = point, Size = size },
                                        Types = new CtrlType[] { CtrlType.None, CtrlType.Grid }
                                    };

                                    page.Add(cellctrl1);
                                    //修改：2010-12-21 何晓明
                                    //原因：表格内数据自动换行
                                    //if (r != lstRows.Count - 1) h += tableToDraw.TableSpace;
                                    //
                                    if (groups[i].Tables.Count <= 1 || iGroupTableCount == groups[i].Tables.Count)
                                        AddCurrentY(h);
                                    //修改：2010-12-16 何晓明
                                    //原因：当行分页至整好一页时进行列分行时列不分页导致重叠bug修正
                                    if (r == lstRows.Count - 1 && cellctrl1.ClientRect.Bottom > _Page.MaxY)
                                        TrunPage();
                                }
                                AddCurrentY(tableToDraw.TableSpace);
                            }
                            tableCell = null;
                        }
                    }
                    #endregion

                    if (cellCtrl.PrintCtrl.Type == CtrlType.ComposeTable)
                        tableCell = cellCtrl;
                }
                //Update by HeXiaoMing 2011-01-19
                //Reason: 表格重叠时导致分组异常提醒
                if (flag)
                {
                    System.Windows.Forms.MessageBox.Show(PrintInfo.TableVSpaceError, PrintInfo.TableVSpaceTip, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    //continue;
                }

                if (tableCell != null && tableCell.PrintCtrl.Type == CtrlType.ComposeTable)
                {
                    PrintCompositeTable compositeTable = tableCell.PrintCtrl as PrintCompositeTable;
                    compositeTable.ClearSelection();
                    tableCell.Point = new Point(tableCell.Point.X, currentY);
                    int iCompositeTableCount = compositeTable.DTable.Count;
                    int iLeft = _Page.MaxY - currentY;
                    int iPageFirstCount = iLeft / compositeTable.Height;
                 
                    int tempCount = (iCompositeTableCount <= iPageFirstCount ? iCompositeTableCount : iPageFirstCount);
                    var pt = tableCell.PrintCtrl as PrintCompositeTable;
                    var rowHight = pt.CellInfo[0].RowsInfo[0].RowHeight;
                    List<SourceGrid.Grid> listGrid = new List<SourceGrid.Grid>();
                    for (int k = 0; k < pt.DTable.Count; k++)
                        listGrid.Add(pt.CreateGrid(k));
                    int h = 0;
                    for (int iGrid = 0; iGrid < listGrid.Count; iGrid++)
                    {
                        for (int iRow = 0; iRow < listGrid[iGrid].Rows.Count; iRow++)
                        {
                            h += listGrid[iGrid].Rows[iRow].Height;
                            //Size size1 = new Size(compositeTable.Width, h * tempCount + 30);
                            //if (iLeft < size1.Height)
                            //{
                            //    h = 0;
                            //    TrunPage1();
                            //    Point point = new Point(compositeTable.Location.X, currentY);
                            //    var cellctrl1 = new CellControl
                            //    {
                            //        PrintCtrl = compositeTable,
                            //        ClientRect = new Rectangle { Location = point, Size = size1 },
                            //        Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
                            //    };
                            //    page.Add(cellctrl1);
                            //}
                        }
                    }
                    Size size2 = new Size(compositeTable.Width, h * tempCount + 30);
                    Point point2 = new Point(compositeTable.Location.X, currentY);
                    var cellctrl2 = new CellControl
                    {
                        PrintCtrl = compositeTable,
                        ClientRect = new Rectangle { Location = point2, Size = size2 },
                        Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
                    };
                    page.Add(cellctrl2);
                    AddCurrentY(h);
                    int iCountPerPage = (_Page.MaxY - _Page.HeaderHeight) / compositeTable.Height;
                    if (iPageFirstCount < iCompositeTableCount)
                    {
                        int modCount = (iCompositeTableCount - iPageFirstCount) % iCountPerPage;
                        int multCount = (iCompositeTableCount - iPageFirstCount) / iCountPerPage;
                        while (multCount > 0)
                        {
                            TrunPage1();
                            Point point = new Point(compositeTable.Location.X, currentY);
                            Size sizePage = new Size(compositeTable.Width, _Page.MaxY);
                            var cellctrlPage = new CellControl
                            {
                                PrintCtrl = compositeTable,
                                ClientRect = new Rectangle { Location = point, Size = sizePage },
                                Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
                            };
                            page.Add(cellctrlPage);
                            multCount--;
                        }

                       TrunPage1();
                        Point point3 = new Point(compositeTable.Location.X, currentY);
                        Size size3 = new Size(compositeTable.Width,compositeTable.grid.Height*modCount);
                        var cellctrl3 = new CellControl
                        {
                            PrintCtrl = compositeTable,
                            ClientRect = new Rectangle { Location = point3, Size = size3 },
                            Types = new CtrlType[] { CtrlType.None, CtrlType.ComposeTable }
                        };
                        page.Add(cellctrl2);
                    }

                }
                else if (tableCell != null && tableCell.PrintCtrl.Type == CtrlType.Grid)
                {

                    //修改：何晓明 2011-03-10
                    //原因：一个组中表格数量多于一个时出错
                    //#region 表格处理

                    //PrintTable tableToDraw = null;
                    //if (tableCell != null)
                    //{
                    //    tableToDraw = tableCell.PrintCtrl as PrintTable;
                    //}

                    //if (tableToDraw == null || tableToDraw.Table.Rows.Count == 0)
                    //{
                    //    currentY += groups[i].Height;//该组控件不包含表格

                    //}
                    //else
                    //{
                    //    int left = tableCell.Point.X;
                    //    AddCurrentY(tableCell.RelativeY);//表格起始纵坐标
                    //    for (int j = 0; j < tableToDraw.Ints.Count; j++)//列分行
                    //    {
                    //        var colInfo = tableToDraw.Ints[j];
                    //        int width = 0;
                    //        var visableCol = GetVisableCol(tableToDraw.ColInfos, colInfo, ref width);

                    //        var lstRows = tableToDraw.GetRowIndex(_Page.MaxY, currentY);
                    //        for (int r = 0; r < lstRows.Count; r++)//行分页
                    //        {
                    //            if (r > 0) TrunPage();

                    //            var splitRowIndex = lstRows[r];
                    //            int h0 = r == 0 && j == 0 ? tableToDraw.TitleRect.Height : 0;
                    //            int topGrid = h0 + currentY;

                    //            //修改：2010-12-21 何晓明
                    //            //原因：表格内数据自动换行
                    //            //int h = tableToDraw.ColHeight
                    //            //    + splitRowIndex.Count * tableToDraw.RowHeight + h0;
                    //            System.Data.DataTable dtTable = tableToDraw.Table;
                    //            int iRowNewCount = 0;
                    //            int iPageRowCount = 0;
                    //            for (int iRows = splitRowIndex.Start; iRows <= splitRowIndex.End; iRows++)
                    //            {
                    //                for (int iColumns = 0; iColumns < dtTable.Columns.Count; iColumns++)
                    //                {
                    //                    string strValue = dtTable.Rows[iRows][iColumns].ToString();
                    //                    List<string> lstNewRows = SplitStringByColumn(strValue, new PrintCell("").View.Font, tableToDraw.ColInfos[j].Width);
                    //                    if (lstNewRows.Count > iRowNewCount)
                    //                    {
                    //                        iRowNewCount = lstNewRows.Count;
                    //                    }
                    //                }
                    //                iPageRowCount += iRowNewCount;
                    //            }
                    //            int h = tableToDraw.ColHeight
                    //                + iPageRowCount * tableToDraw.RowHeight + h0;
                    //            //

                    //            var point = new Point(left, topGrid);

                    //            var size = new Size(width, h);

                    //            var cellctrl1 = new CellControl
                    //            {
                    //                PrintCtrl = tableToDraw,
                    //                ColIndexs = visableCol,
                    //                Point = point,
                    //                Size = size,
                    //                RowIndexs = splitRowIndex,
                    //                ClientRect = new Rectangle { Location = point, Size = size },
                    //                Types = new CtrlType[] { CtrlType.None, CtrlType.Grid }
                    //            };

                    //            page.Add(cellctrl1);
                    //            //修改：2010-12-21 何晓明
                    //            //原因：表格内数据自动换行
                    //            //if (r != lstRows.Count - 1) h += tableToDraw.TableSpace;
                    //            //
                    //            AddCurrentY(h);
                    //            //修改：2010-12-16 何晓明
                    //            //原因：当行分页至整好一页时进行列分行时列不分页导致重叠bug修正
                    //            if (r == lstRows.Count - 1 && cellctrl1.ClientRect.Bottom + _Page.MaxY / 10 > _Page.MaxY)
                    //                TrunPage();
                    //        }
                    //        AddCurrentY(tableToDraw.TableSpace);
                    //    }
                    //}

                    //#endregion
                }
                else
                {
                    //修改：何晓明 2011-03-21
                    //原因：导出EXCEL表格留白过多
                    if(cellCtrl!=null&&cellCtrl.PrintCtrl.Type!= CtrlType.Grid)
                    //
                    currentY += groups[i].Height;
                }
            }
            return lstPage;
        }
        //修改：何晓明
        //原因：数据内容按宽度分行
        public List<string> SplitStringByColumn(string strValue, Font font, int width)
        {
            List<string> lstColumnRows = new List<string>();
            string strNewRow = string.Empty;
            using (Graphics g = Graphics.FromImage(Properties.Resources.LvDou))
            {
                for (int i = 0; i < strValue.Length; i++)
                {
                    string strTemp = strValue.Substring(i, 1);
                    int iStringPosition = g.MeasureString(strNewRow + strTemp, font).Width.GetNearInt() + 20;
                    //修改：何晓明 2010-12-23 打印输入换行符时分行异常
                    //if (iStringPosition <= width)
                    if (iStringPosition <= width && strTemp != "\n")
                    {
                        strNewRow += strTemp;
                    }
                    else
                    {
                        if (i > 0)
                        {
                            //修改：何晓明 2010-12-23 打印输入换行符时分行异常
                            //strNewRow += "\n";
                            //strValue = strValue.Insert(i - 1, "\n");

                            strNewRow += "\n";
                            if (strTemp != "\n")
                            {
                                strValue = strValue.Insert(i - 1, "\n");
                            }
                            lstColumnRows.Add(strNewRow);
                            strNewRow = string.Empty;
                        }
                    }
                    if (i == strValue.Length - 1 && g.MeasureString(strNewRow, font).Width.GetNearInt() <= width)
                        lstColumnRows.Add(strNewRow);
                }
            }
            lstColumnRows.TrimExcess();
            return lstColumnRows;

        }
    }
}
