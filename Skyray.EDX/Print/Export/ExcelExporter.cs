using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data.SqlClient;

namespace Skyray.Print
{
    /// <summary>
    /// ExcelExporter类，继承自接口IExporter,将数据导出为Excel
    /// </summary>
    public class ExcelExporter : IExporter
    {
        //修改：何晓明 2011-01-14
        //原因：导出EXCEL时根据内容控制列宽
        /// <summary>
        /// 根据内容定义列宽度
        /// </summary>
        private int[] ColumnWidthByContent;
        private Dictionary<CellControl, int[]> dictionary = new Dictionary<CellControl, int[]>();
        //
        /// <summary>
        /// 构造函数
        /// 初始化PageExporter
        /// </summary>
        /// <param name="exporter"></param>
        public ExcelExporter(PageExporter exporter)
        {
            PageExporter = exporter;
        }

        #region 判断文件是否打开

        //修改：何晓明 2011-01-12
        //原因：覆盖正打开的Excel时为系统提示
        /// <summary>
        /// 调用kernel32.dll中_lopen方法打开文件
        /// </summary>
        /// <param name="lpPathName">文件路径名称</param>
        /// <param name="iReadWrite">读取方式</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        /// <summary>
        /// 调用kernel32.dll中CloseHandle方法关闭文件
        /// </summary>
        /// <param name="hObjedt">打开的文件的句柄</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObjedt);
        /// <summary>
        /// 打开方式读写
        /// </summary>
        public const int OF_READWRITE = 2;
        /// <summary>
        /// 共享方式不共享
        /// </summary>
        public const int OF_SHARE_DENY_NONE = 0x40;
        /// <summary>
        /// 打开失败句柄值
        /// </summary>
        public readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        /// <summary>
        /// 判断文件是否占用
        /// </summary>
        /// <param name="strFileName">文件路径名</param>
        /// <returns></returns>
        public bool IsFileUsed(string strFileName)
        {
            bool bFlag;
            IntPtr hHandle = _lopen(strFileName, OF_READWRITE );
            if (hHandle == HFILE_ERROR)
            {
                bFlag = true;
            }
            else
            {
                CloseHandle(hHandle);
                bFlag = false;
            }
            return bFlag;
        }
        //
        #endregion

        #region IExporter 成员
        /// <summary>
        /// PageExporter属性 通过该属性的GetPages()方法可以获取需要导出的页面的页数
        /// </summary>
        public PageExporter PageExporter { get; set; }
        /// <summary>
        /// 实现IExporter接口 完成导出动作
        /// </summary>
        public void Export()
        {
            string fileName = GetFileName();
            if (fileName != string.Empty)
            {
                //修改：何晓明 2011-03-16
                //原因：导出EXCEL另外分页
                //var workBook = CreateWorkBook(PageExporter.GetPages());
                var workBook = CreateWorkBook(PageExporter.GetPagesToExcel());
                //
                if (workBook != null)
                {
                    //修改：何晓明 2011-03-22
                    //原因：覆盖正打开的Excel时为系统提示
                    if (IsFileUsed(fileName)&&File.Exists(fileName))
                    {
                        //MessageBox.Show(PrintInfo.FileUsed+","+PrintInfo.SaveFailed);
                        Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.FileUsed + "," + PrintInfo.SaveFailed, PrintInfo.Tip, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        workBook.Save(fileName);
                        UIHelper.AskToOpenFile(fileName);
                    }
                    //workBook.Save(fileName);
                    //UIHelper.AskToOpenFile(fileName);
                    //
                    
                }
            }
        }
        /// <summary>
        /// 直接导出
        /// </summary>
        /// <param name="path"></param>
        public void DirectExport(string path)
        {
            //修改：何晓明 2010-01-12
            //原因：保存EXCEL无需打开
            //操作：加入异常捕获
            try
            {
                //修改：何晓明 2011-03-16
                //原因：导出EXCEL另外分页
                //var workBook = CreateWorkBook(PageExporter.GetPages());
                var workBook = CreateWorkBook(PageExporter.GetPagesToExcel());
                //
                if (workBook != null)
                {
                    string nowTime = DateTime.Now.ToString("yyyyMMddhhmmss");
                    workBook.Save(path + "\\" + nowTime + ".xls");
                    //修改：何晓明 2010-01-12
                    //原因：保存EXCEL无需打开
                    //MessageBox.Show(PrintInfo.SaveSuccess,"",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    //Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess);         
                    if (!File.Exists(path + "\\" + nowTime + ".xls")) return;
                    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        Help.ShowHelp(null, path + "\\" + nowTime + ".xls");
                    }
                    //Help.ShowHelp(null,path + "\\" + nowTime + ".xls");
                    //
                }
                else
                {
                    Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.PrintTemplateIsNull, PrintInfo.Tip, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveFailed+e.Message);
            }
        }

        //修改：何晓明 2011-10-17
        //原因：按指定名称保存源数据至EXCEL
        /// <summary>
        /// 直接导出
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">文件名，不需指定后缀名默认xls</param>
        public void DirectExport(string path, string fileName)
        {
            try
            {
                //修改：何晓明 2011-03-16
                //原因：导出EXCEL另外分页
                //var workBook = CreateWorkBook(PageExporter.GetPages());
                var workBook = CreateWorkBook(PageExporter.GetPagesToExcel());
                //
                if (workBook != null)
                {
                    workBook.Save(path+"\\"+fileName+".xls");
                    //修改： 何晓明 20110714 打开保存模板
                    //Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess);
                    if (!File.Exists(path + "\\" + fileName + ".xls")) return;

                    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        Help.ShowHelp(null, path + "\\" + fileName + ".xls");
                    }
                    //
                }
                else
                {
                    Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.NoLoadSource, PrintInfo.Tip, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
                    return;
                }
            }
            catch(Exception e)
            {
                Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveFailed+e.Message);
            }
        }

        /// <summary>
        /// 实现IExporter接口 获取导出文件名称
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return UIHelper.GetFileToSaveName("Excel File (*.xls)|*.xls");
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 设置边框样式
        /// </summary>
        /// <param name="style">传入样式信息</param>
        private void SetStyleBorder(Aspose.Cells.Style style)
        {
            style.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        }
        /// <summary>
        /// 设置头样式
        /// </summary>
        /// <param name="cell">传入单元格</param>
        /// <param name="tableStyle">传入表格样式</param>
        private void SetHeaderStyle(Cell cell, ITableStyle tableStyle)
        {
            //var styleHeader = cell.Style;
            var styleHeader = cell.GetDisplayStyle();
            SetStyleBorder(styleHeader);
            styleHeader.ForegroundColor = tableStyle.HeaderBackColor;
            styleHeader.Pattern = BackgroundType.Solid;
            styleHeader.Font.Color = tableStyle.HeaderColor;
            styleHeader.HorizontalAlignment = TextAlignmentType.Center;
            FontToAsposeFont(styleHeader.Font, tableStyle.HeaderFont);
            if (cell.IsMerged) SetMergedRangeStyle(cell);//设置合并的单元格样式
        }
        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="cell">传入单元格</param>
        /// <param name="tableStyle">传入表格样式</param>
        /// <param name="flag">传入是否设置字体颜色</param>
        private void SetCellStyle(Cell cell, ITableStyle tableStyle,bool flag)
        {
            //var style = cell.Style;
            var style = cell.GetDisplayStyle();
            if (flag)
                style.Font.Color = tableStyle.CellColor;
            else
                style.Font.Color = Color.Red;
            //style.HorizontalAlignment = TextAlignmentType.Left | TextAlignmentType.Center;
            style.HorizontalAlignment = TextAlignmentType.Center;
            //
            FontToAsposeFont(style.Font, tableStyle.CellFont);
            SetStyleBorder(style);
            if (cell.IsMerged) SetMergedRangeStyle(cell);//设置合并的单元格样式
        }

        /// <summary>
        /// 设置合并样式
        /// </summary>
        /// <param name="cell">传入单元格</param>
        private void SetMergedRangeStyle(Cell cell)
        {
            var range = cell.GetMergedRange();
            if (range != null)
            {
                StyleFlag flag = new StyleFlag();
                flag.All = true;
                //range.ApplyStyle(cell.Style, flag);
                range.ApplyStyle(cell.GetDisplayStyle(), flag);
            }
        }

        /// <summary>
        /// 创建工作簿 该方法根据打印内容进行分页创建多个Excel的工作簿
        /// </summary>
        /// <param name="workbook">传入workbook</param>
        /// <param name="sheetName">传入worksheet名字</param>
        /// <param name="pageOrientationType"></param>
        /// <returns></returns>
        private Worksheet CreateWorkSheet(Workbook workbook, string sheetName, PageOrientationType pageOrientationType)
        {
            Worksheet sheet = workbook.Worksheets.Add(sheetName);
            sheet.PageSetup.PaperSize = PaperSizeType.PaperA4;
            sheet.IsGridlinesVisible = false;

            sheet.PageSetup.Orientation = pageOrientationType;

            sheet.PageSetup.PrintComments = PrintCommentsType.PrintNoComments;
            sheet.PageSetup.TopMargin =
                sheet.PageSetup.BottomMargin =
                sheet.PageSetup.LeftMargin =
                sheet.PageSetup.RightMargin = 0;

            sheet.PageSetup.HeaderMargin = 0;
            sheet.PageSetup.FooterMargin = 0;

            return sheet;
        }

        /// <summary>
        /// 创建WorkBook
        /// </summary>
        /// <param name="Sheets"></param>
        /// <returns></returns>
        public Workbook CreateWorkBook(List<List<CellControl>> Sheets)
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Clear();
            int pageIndex = 0;
            if (Sheets == null || Sheets.Count == 0)
                return null;
            foreach (var sc in Sheets)
            {
                if (sc.Count == 0) continue;
                pageIndex++;

                var Page = sc[0].PrintCtrl.Panel.Page;
                var orientationType = Page.Dir == PrintDirection.Horizontal ?
                                   PageOrientationType.Portrait : PageOrientationType.Landscape;

                var sheet = CreateWorkSheet(workbook, "Page" + pageIndex, orientationType);
                Cells cells = sheet.Cells;

                var rows = GetRows(sc);
                SetRowHeight(cells, rows);
                var cols = GetCols(sc);
                SetColWidth(cells, cols);

                foreach (var c in sc)
                {
                    foreach (var typ in c.Types)
                    {
                        var cell = GetCell(cells, rows, cols, c, typ);
                        if (typ == CtrlType.None) continue;
                        else
                        {
                            if (cell != null)
                            {
                                switch (typ)
                                {
                                    case CtrlType.Label:
                                        SetLabelCellStyle(cell, c.PrintCtrl);
                                        cell.PutValue(c.PrintCtrl.Text);
                                        break;
                                    case CtrlType.Field:
                                        SetFieldCellStyle(cell, c.PrintCtrl as PrintField);
                                        cell.PutValue((c.PrintCtrl as PrintField).TextValue);
                                        break;
                                    case CtrlType.Image:
                                        var imageCtrl = c.PrintCtrl as PrintImage;
                                        //修改：何晓明 2010-01-14 
                                        //原因：PNG格式LOGO导出背景色变成黑色
                                        //using (var stream = PrintHelper.GetThumbnail(imageCtrl.Image, imageCtrl.ContentRect.Size))
                                        //修改：何晓明 2011-02-21
                                        //原因：图片及边框导出Excel
                                        //using (var stream = PrintHelper.GetThumbnail(imageCtrl.Image, imageCtrl.ContentRect.Size, imageCtrl.Image.RawFormat))
                                        using (var stream = PrintHelper.GetThumbnailWithBorder(imageCtrl.Image,imageCtrl.ContentRect.Size,imageCtrl.ImageBorderColor))
                                        //
                                        {
                                            int endRow = 1;
                                            int endCol = 1;
                                            if (cell.IsMerged)
                                            {
                                                var range = cell.GetMergedRange();
                                                endRow = range.RowCount;
                                                endCol = range.ColumnCount;
                                            }
                                            sheet.Pictures.Add(cell.Row, cell.Column, endRow + cell.Row, endCol + cell.Column, stream);                                            
                                        }
                                        break;
                                    case CtrlType.Grid:

                                        int x = c.ClientRect.X;
                                        int startCol = cell.Column;
                                        var printTable = c.PrintCtrl as PrintTable;

                                        Aspose.Cells.StyleFlag styleFlag = new StyleFlag();
                                        styleFlag.All = true;
                                        //修改：何晓明 2011-01-19
                                        //原因：多表格导出EXCEL时根据内容控制列宽异常                                    
                                        foreach (KeyValuePair<CellControl, int[]> a in dictionary)
                                        {
                                            if (a.Key == c)
                                                ColumnWidthByContent = a.Value;
                                        }
                                        foreach (var i in c.ColIndexs)
                                        {
                                            //修改：何晓明 2011-03-25
                                            //原因：导出EXCEL时根据内容控制列宽
                                            //x += Math.Max(printTable.ColInfos[i].Width, ColumnWidthByContent[i]);
                                            x += Math.Max(0, ColumnWidthByContent[i]);
                                            //x += printTable.ColInfos[i].Width;
                                            //
                                            int colIndex3 = cols.IndexOf(x);
                                            int colSpan = colIndex3 - startCol;

                                            int y = c.ClientRect.Y;
                                            int startRow = cell.Row;

                                            //表头部分
                                            y += printTable.ColHeight;
                                            int rowIndex3 = rows.IndexOf(y);
                                            int rowSpan = rowIndex3 - startRow;

                                            if (colSpan > 1 || rowSpan > 1)
                                                cells.Merge(startRow, startCol, rowSpan, colSpan);

                                            var cellTable = cells[startRow, startCol];
                                            cellTable.PutValue(printTable.Table.Columns[i].Caption);
                                            SetHeaderStyle(cellTable, printTable.TableStyle);

                                            startRow = rowIndex3;
                                            for (int j = c.RowIndexs.Start; j <= c.RowIndexs.End; j++)
                                            {
                                                //修改：何晓明 2011-03-17
                                                //原因：导出EXCEL页面行高不够
                                                y += printTable.RowHeight;
                                                //int h = 0;
                                                //System.Data.DataTable dtTable = printTable.Table;
                                                //for (int iColumn = 0; iColumn < dtTable.Columns.Count; iColumn++)
                                                //{
                                                //    string strValue = dtTable.Rows[j][iColumn].ToString();
                                                //    List<string> lstNewRows = SplitStringByColumn(printTable.CreateGraphics(), strValue, new PrintCell("").View.Font, printTable.ColInfos[iColumn].Width);
                                                //    h = Math.Max(h, lstNewRows.Count);
                                                //}
                                                //y += h * printTable.RowHeight;
                                                //
                                                rowIndex3 = rows.IndexOf(y);
                                                rowSpan = rowIndex3 - startRow;

                                                if (colSpan > 1 || rowSpan > 1)
                                                    cells.Merge(startRow, startCol, rowSpan, colSpan);
                                                cellTable = cells[startRow, startCol];

                                                string sValue = printTable.Table.Rows[j][i].ToString();
                                                cellTable.PutValue(sValue);                                                
                                                bool showSatus = false;
                                                if (printTable.RangeColumns != null && printTable.RangeColumns.Count > 0)
                                                {
                                                    RangeColumns columnsRange = printTable.RangeColumns.Find(w => w.ColumnsName == printTable.Table.Columns[i].ColumnName);
                                                    if ((sValue.IsInt() || sValue.IsNumeric()) && columnsRange != null)
                                                    {
                                                        double showValue = double.Parse(sValue);
                                                        if (columnsRange.maxValue < showValue || columnsRange.minValue > showValue)
                                                        {
                                                            showSatus = true;
                                                            SetCellStyle(cellTable, printTable.TableStyle, false);
                                                        }
                                                    }
                                                }
                                                if (!showSatus)
                                                    SetCellStyle(cellTable, printTable.TableStyle, true);
                                                startRow = rowIndex3;
                                            }
                                            startCol = colIndex3;
                                        }
                                        break;
                                    case CtrlType.ComposeTable:
                                        int xMark = c.ClientRect.X;
                                        int StartCol = cell.Column+1;                                       

                                        var printCompositeTable = c.PrintCtrl as PrintCompositeTable;
                                        Aspose.Cells.StyleFlag styleComposite = new StyleFlag();
                                        styleComposite.All = true;
                                        List<CellSize> cellList = printCompositeTable.CellInfo;

                                        var pt = c.PrintCtrl as PrintCompositeTable;
                                        List<SourceGrid.Grid> listGrid = new List<SourceGrid.Grid>();
                                        for (int k = 0; k < pt.DTable.Count; k++)
                                            listGrid.Add(pt.CreateGrid(k));
                                        for (int iGrid = 0; iGrid < listGrid.Count;iGrid++ )
                                        {
                                            for (int iCol = 0; iCol < listGrid[iGrid].Columns.Count; iCol++)
                                            {
                                                int yMark = c.ClientRect.Y + 15 + iGrid * listGrid[iGrid].Rows.Count*listGrid[iGrid].Rows[0].Height;
                                                int StartRow = cell.Row + 1 + iGrid * listGrid[iGrid].Rows.Count;
                                                xMark += listGrid[iGrid].Columns[iCol].Width;
                                                int Column = cols.IndexOf(xMark);
                                                int ColSpan = Column - StartCol;
                                                for (int iRow = 0; iRow < listGrid[iGrid].Rows.Count; iRow++)                                                
                                                {
                                                    yMark += listGrid[iGrid].Rows[iRow].Height;
                                                    int Row = rows.IndexOf(yMark);
                                                    int RowSpan = Row - StartRow;
                                                    if (ColSpan > 1 || RowSpan > 1)
                                                        cells.Merge(StartRow, StartCol, RowSpan, ColSpan);
                                                    if(listGrid[iGrid][iRow,iCol]!=null&&(listGrid[iGrid][iRow,iCol].ColumnSpan>1||listGrid[iGrid][iRow,iCol].RowSpan>1))
                                                    {
                                                        cells.Merge(StartRow, StartCol, listGrid[iGrid][iRow, iCol].RowSpan, listGrid[iGrid][iRow, iCol].ColumnSpan);
                                                        for (int iMergeRow = 0; iMergeRow < listGrid[iGrid][iRow, iCol].RowSpan;iMergeRow++ )
                                                        {
                                                            for (int iMergeCol = 0; iMergeCol < listGrid[iGrid][iRow, iCol].ColumnSpan;iMergeCol++ )
                                                            {
                                                                listGrid[iGrid][iRow + iMergeRow, iCol + iMergeCol].ColumnSpan = 1;
                                                                listGrid[iGrid][iRow + iMergeRow, iCol + iMergeCol].RowSpan = 1;
                                                            }
                                                        }
                                                    }
                                                    var compositeTableCell = cells[StartRow, StartCol];//cells[cell.Row + 1 + iRow+iGrid*listGrid[iGrid].Rows.Count, cell.Column + 1 + iCol];
                                                    SourceGrid.Grid grid = listGrid[iGrid] as SourceGrid.Grid;
                                                    if (grid[iRow, iCol]!=null)
                                                    compositeTableCell.PutValue(grid[iRow,iCol].Value);
                                                    SetCellStyle(compositeTableCell, pt.TableStyle, true);
                                                    StartRow = Row;
                                                }
                                                StartCol = Column;
                                            }
                                            StartCol = cell.Column + 1;
                                            xMark = c.ClientRect.X;
                                        }
                                        //for (int a = 0; a < cellList.Count; a++)
                                        //{
                                        //    for (int b = 0; b < cellList[a].RowsInfo.Count; b++)
                                        //    {
                                        //        int rowIndex = b + cell.Row + 1;
                                        //        int columnIndex = a + cell.Column + 1;
                                        //        var compositeTableCell = cells[rowIndex, columnIndex];
                                        //        RowInfo cellInfo = printCompositeTable.CellInfo[a].RowsInfo[b];
                                        //        if (cellInfo.RowSpan > 1 || cellInfo.ColumnSpanm > 1)
                                        //            cells.Merge(rowIndex, columnIndex, cellInfo.RowSpan, cellInfo.ColumnSpanm);
                                        //        for (int u = 0; u < printCompositeTable.DTable.Count; u++)
                                        //        {
                                        //            string attributeName = cellList[a].RowsInfo[b].AttrubuteName;
                                        //            if (!cellList[a].RowsInfo[b].IsEnumerable && attributeName != null)
                                        //            {
                                        //                object obj = printCompositeTable.DataSourceList[u];
                                        //                compositeTableCell.PutValue(obj.GetType().GetProperty(attributeName).GetValue(obj, null));
                                        //            }
                                        //            else if (cellList[a].RowsInfo[b].IsEnumerable)
                                        //            {
                                        //                System.Data.DataTable dt = printCompositeTable.DTable[u];
                                        //                for (int row = 0; row < dt.Rows.Count; row++)
                                        //                {
                                        //                    for (int col = 0; col < dt.Columns.Count; col++)
                                        //                    {
                                        //                        compositeTableCell.PutValue(dt.Rows[row][col].ToString());
                                        //                    }
                                        //                }

                                        //            }
                                        //            else
                                        //            {
                                        //                compositeTableCell.PutValue(printCompositeTable.grid[b, a].Value);
                                        //            }
                                        //            compositeTableCell.PutValue(printCompositeTable.grid[b, a].Value);
                                        //            SetCellStyle(compositeTableCell, printCompositeTable.TableStyle, true);
                                        //        }
                                        //    }
                                        //}

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return workbook;
        }
        /// <summary>
        /// 设置行高
        /// </summary>
        /// <param name="cells">传入单元格</param>
        /// <param name="rows">传入行数</param>
        private void SetRowHeight(Cells cells, List<int> rows)
        {
            for (int i = 0; i < rows.Count - 1; i++)
            {
                int height = rows[i + 1] - rows[i];
                cells.SetRowHeightPixel(i, height);
            }
        }
        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="cells">传入单元格</param>
        /// <param name="cols">传入列数</param>
        private void SetColWidth(Cells cells, List<int> cols)
        {
            for (int i = 0; i < cols.Count - 1; i++)
            {
                int width = cols[i + 1] - cols[i];
                cells.SetColumnWidthPixel(i, width);
            }
        }

        /// <summary>
        /// 设置Label单元格样式
        /// </summary>
        /// <param name="cell">传入单元格</param>
        /// <param name="printCtrl">传入PrintCtrl</param>
        private void SetLabelCellStyle(Cell cell, PrintCtrl printCtrl)
        {
            var style = cell.GetDisplayStyle();
            style.Font.Color = printCtrl.TextColor;
            FontToAsposeFont(style.Font, printCtrl.TextFont);
            //cell.Style.Font.Color = printCtrl.TextColor;
            //FontToAsposeFont(cell.Style.Font, printCtrl.TextFont);
        }

        /// <summary>
        /// 设置Field单元格样式
        /// </summary>
        /// <param name="cell">传入单元格</param>
        /// <param name="printField">传入PrintField</param>
        private void SetFieldCellStyle(Cell cell, PrintField printField)
        {
            var style = cell.GetDisplayStyle();
            style.Font.Color = printField.TextColor;
            FontToAsposeFont(style.Font, printField.TextFont);
            //cell.Style.Font.Color = printField.TextValueColor;
            //FontToAsposeFont(cell.Style.Font, printField.TextValueFont);
        }

        /// <summary>
        /// 获得内容矩形
        /// </summary>
        /// <param name="c">传入CellControl类型信息</param>
        /// <param name="typ">传入CtrlType类型信息</param>
        /// <param name="isMerged">传入Bool型是否合并</param>
        /// <returns></returns>
        private Rectangle GetContentRect(CellControl c, CtrlType typ, ref bool isMerged)
        {
            Rectangle rect = Rectangle.Empty;
            switch (typ)
            {
                case CtrlType.Label:
                    rect = c.LabelRect;
                    break;
                case CtrlType.Field:
                    rect = c.FieldRect;
                    break;
                case CtrlType.Image:
                    rect = c.ClientRect;
                    break;
                case CtrlType.Grid:
                case CtrlType.ComposeTable:
                    isMerged = false;
                    rect = c.ClientRect;
                    break;
                default:
                    break;
            }
            return rect;
        }

        /// <summary>
        /// 获取表格
        /// </summary>
        /// <param name="cells">传入单元格</param>
        /// <param name="rows">传入整型列表行数</param>
        /// <param name="cols">传入整型列表列数</param>
        /// <param name="c">传入CellControl类型</param>
        /// <param name="typ"></param>
        /// <returns></returns>
        private Cell GetCell(Cells cells, List<int> rows, List<int> cols, CellControl c, CtrlType typ)
        {
            bool isMerged = true;
            Rectangle rect = GetContentRect(c, typ, ref isMerged);
            Cell cell = null;
            if (!rect.IsEmpty)
            {
                int rowIndex1 = rows.IndexOf(rect.Y);
                if (rowIndex1 != -1)
                {
                    int colIndex1 = cols.IndexOf(rect.X);
                    if (colIndex1 != -1)
                    {
                        cell = cells[rowIndex1, colIndex1];
                        if (isMerged) //合并单元格
                        {
                            int rowIndex2 = rows.IndexOf(rect.Bottom);
                            if (rowIndex2 != -1)
                            {
                                int colIndex2 = cols.IndexOf(rect.Right);
                                if (colIndex2 != -1)
                                {
                                    int colSpan = colIndex2 - colIndex1;
                                    int rowSpan = rowIndex2 - rowIndex1;
                                    if (colSpan > 0 && rowSpan > 0 && (colSpan > 1 || rowSpan > 1))
                                        cells.Merge(rowIndex1, colIndex1, rowSpan, colSpan);
                                }
                            }
                        }
                    }
                }
            }
            return cell;
        }


        /// <summary>
        /// 字体转换
        /// </summary>
        /// <param name="fontAspose"></param>
        /// <param name="font"></param>
        private void FontToAsposeFont(Aspose.Cells.Font fontAspose, System.Drawing.Font font)
        {
            fontAspose.Name = font.Name;
            fontAspose.Size = (int)font.Size;
            fontAspose.IsBold = font.Bold;
            fontAspose.IsItalic = font.Italic;
            fontAspose.IsStrikeout = font.Strikeout;

            if (font.Underline) fontAspose.Underline = FontUnderlineType.Single;
        }

        #region Excel Sheet Split Row And Col
        /// <summary>
        /// 获取列数
        /// </summary>
        /// <param name="sc">传入CellControl类型列表</param>
        /// <returns></returns>
        private List<int> GetCols(List<CellControl> sc)
        {
            List<int> ints = new List<int>();
            ints.Add(0);

            foreach (var c in sc)
            {
                foreach (var typ in c.Types)
                {
                    if (typ == CtrlType.None) continue;
                    if (typ == CtrlType.Grid)
                    {
                        var printTable = (c.PrintCtrl as PrintTable);
                        int w = c.ClientRect.Left;
                        ints.Add(w);
                        //修改：何晓明 2011-01-14
                        //原因：导出EXCEL时根据内容控制列宽
                        Graphics g = printTable.CreateGraphics();
                        ColumnWidthByContent = GetColumWidthByContents(g, printTable);
                        dictionary.Add(c, ColumnWidthByContent);
                        //
                        foreach (var i in c.ColIndexs)
                        {
                            //修改：何晓明 2011-03-25
                            //原因：导出EXCEL时根据内容控制列宽
                            //w += Math.Max(printTable.ColInfos[i].Width, ColumnWidthByContent[i]);
                            w += Math.Max(0, ColumnWidthByContent[i]);
                            //w += printTable.ColInfos[i].Width;
                            //
                            ints.Add(w);
                        }
                    }
                    else if (typ == CtrlType.ComposeTable)
                    {
                        //var printCompositeTable = (c.PrintCtrl as PrintCompositeTable);
                        //int w = c.ClientRect.Left;
                        //ints.Add(w);
                        //List<CellSize> sizeList = printCompositeTable.CellInfo;
                        //foreach (var cellColumn in sizeList)
                        //{
                        //    w += cellColumn.ColumnInfo.Width;
                        //    ints.Add(w);
                        //}
                        var pt = c.PrintCtrl as PrintCompositeTable;
                        var rowWidth = pt.CellInfo[0].ColumnInfo.Width;
                        List<SourceGrid.Grid> listGrid = new List<SourceGrid.Grid>();
                        for (int k = 0; k < pt.DTable.Count; k++)
                            listGrid.Add(pt.CreateGrid(k));
                        int w = c.ClientRect.Left;
                        ints.Add(w);
                        for (int iGrid = 0; iGrid < listGrid.Count; iGrid++)
                        {
                            for (int iCol = 0; iCol < listGrid[iGrid].Columns.Count; iCol++)
                            {
                                w += listGrid[iGrid].Columns[iCol].Width;
                                ints.Add(w);
                            }
                        }
                    }
                    else
                    {
                        var rect = typ == CtrlType.Label ? c.LabelRect
                            : typ == CtrlType.Field ? c.FieldRect : c.ClientRect;
                        ints.Add(rect.Left);
                        ints.Add(rect.Right);
                    }
                }
            }

            HandleOverLengthCell(ref ints, Param.MaxCellWidth);

            return ints;
        }
        /// <summary>
        /// 获取行数
        /// </summary>
        /// <param name="sc">传入CellControl类型列表</param>
        /// <returns></returns>
        private List<int> GetRows(List<CellControl> sc)
        {
            List<int> ints = new List<int>();
            ints.Add(0);
            //修改：何晓明 2011-01-17
            //原因：表格分行显示时导出内容的高度为多行高度
            //bool hasGrid = false;
            //int iGridOffsetY = 0;
            //
            foreach (var c in sc)
            {                
                foreach (var typ in c.Types)
                {                    
                    if (typ == CtrlType.None) continue;
                    if (typ == CtrlType.Grid)
                    {
                        var printTable = c.PrintCtrl as PrintTable;
                        int start = c.RowIndexs.Start;
                        int end = c.RowIndexs.End;

                        //修改：何晓明 2011-01-17
                        //原因：表格分行显示时导出内容的高度为多行高度
                        //hasGrid = true;
                        //ints.Add(c.ClientRect.Top - iGridOffsetY);
                        //int top = c.ClientRect.Top + printTable.ColHeight-iGridOffsetY; //行头
                        //iGridOffsetY += c.ClientRect.Bottom - c.ClientRect.Top - printTable.RowCount * printTable.RowHeight-printTable.ColHeight;
                        ints.Add(c.ClientRect.Top);
                        int top = c.ClientRect.Top + printTable.ColHeight; //行头
                        //                        
                        ints.Add(top);
                        int h = 0;
                        for (int j = start; j <= end; j++)
                        {
                            //修改：何晓明 2010-03-17
                            //原因：导出EXCEL页面行高不够
                            h += printTable.RowHeight;
                            //int count = 0;
                            //System.Data.DataTable dtTable = printTable.Table;
                            //for (int iColumn = 0; iColumn < dtTable.Columns.Count; iColumn++)
                            //{
                            //    string strValue = dtTable.Rows[j][iColumn].ToString();
                            //    List<string> lstNewRows = SplitStringByColumn(printTable.CreateGraphics(), strValue, new PrintCell("").View.Font, printTable.ColInfos[iColumn].Width);
                            //    count = Math.Max(count, lstNewRows.Count);
                            //}
                            //h += count * printTable.RowHeight;
                            //
                            ints.Add(top +h );//逐行增加
                        }
                    }
                    else if (typ == CtrlType.ComposeTable)
                    {
                        //var printComposite = c.PrintCtrl as PrintCompositeTable;
                        //ints.Add(c.ClientRect.Top + 15);
                        //int top = c.ClientRect.Top + 15;
                        //if (printComposite.CellInfo != null && printComposite.CellInfo.Count > 0)
                        //{
                        //    List<RowInfo> rowsInfo = printComposite.CellInfo[0].RowsInfo;
                        //    int h = 0;
                        //    for (int j = 0; j < rowsInfo.Count; j++)
                        //    {
                        //        h += rowsInfo[j].RowHeight;
                        //        ints.Add(top + h);//逐行增加
                        //    }
                        //}

                        var pt = c.PrintCtrl as PrintCompositeTable;
                        var rowHight = pt.CellInfo[0].RowsInfo[0].RowHeight;
                        List<SourceGrid.Grid> listGrid = new List<SourceGrid.Grid>();
                        for (int k = 0; k < pt.DTable.Count; k++)
                            listGrid.Add(pt.CreateGrid(k));
                        int top = c.ClientRect.Top + 15;
                        int h = 0;
                        for (int iGrid = 0; iGrid < listGrid.Count; iGrid++)
                        {                            
                            for (int iRow = 0; iRow < listGrid[iGrid].Rows.Count; iRow++)
                            {                                
                                h += listGrid[iGrid].Rows[iRow].Height;
                                ints.Add(top + h);
                            }
                        }
                    }
                    else
                    {
                        var rect = typ == CtrlType.Label ? c.LabelRect
                            : typ == CtrlType.Field ? c.FieldRect : c.ClientRect;
                        ints.Add(rect.Top);
                        ints.Add(rect.Bottom);
                    }
                }
            }

            HandleOverLengthCell(ref ints, Param.MaxCellHeight);

            return ints;
        }

        private void HandleOverLengthCell(ref List<int> ints, int max)
        {
            List<int> lstTemp = new List<int>();
            for (int i = 0; i < ints.Count - 1; i++)
            {
                int height = ints[i + 1] - ints[i];
                if (height > max)
                {
                    int n = height / max;
                    for (int j = 1; j <= n; j++)
                    {
                        lstTemp.Add(ints[i] + j * max);
                    }
                }
            }
            ints.AddRange(lstTemp);

            ints = ints.Distinct().ToList();
            ints.Sort();
        }

        //修改：何晓明 2011-01-14
        //原因：导出EXCEL时根据内容控制列宽
        private int[] GetColumWidthByContents(Graphics g,PrintTable printTable)
        {
            System.Data.DataTable table = printTable.Table;
            int[] lstColumnContentWidth = new int[table.Columns.Count];
            for (int c = 0; c < table.Columns.Count;c++ )
            {
                lstColumnContentWidth[c] = (int)((int)g.MeasureString(table.Columns[c].Caption, printTable.TableStyle.HeaderFont != null ?
                        printTable.TableStyle.CellFont : new System.Drawing.Font("宋体", 10)).Width*1.25);
                for (int r = 0; r < table.Rows.Count;r++ )
                {
                    string strContent = table.Rows[r][c].ToString();
                    var size= g.MeasureString(strContent ,printTable.TableStyle.CellFont!=null? 
                        printTable.TableStyle.CellFont:new System.Drawing.Font("宋体",10));
                    int iContentWidth = (int)size.Width;
                    lstColumnContentWidth[c] = Math.Max(lstColumnContentWidth[c], iContentWidth);
                }
            }
            return lstColumnContentWidth;
        }
        //
        //修改：何晓明
        //原因：数据内容按宽度分行
        public List<string> SplitStringByColumn(Graphics g, string strValue, System.Drawing.Font font, int width)
        {
            List<string> lstColumnRows = new List<string>();
            string strNewRow = string.Empty;
            using (g)
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
        #endregion

        #endregion
    }
}
