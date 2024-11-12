using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SourceGrid;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Drawing2D;
using SourceGrid.Selection;
using DevAge.Drawing;
using SourceGrid.Cells;
using SourceGrid.Cells.Views;
namespace Skyray.Print
{
    /// <summary>
    /// 表格控件
    /// </summary>
    public class PrintTable : PrintCtrl
    {
        /// <summary>
        /// 表格间距
        /// </summary>
        private int _TableSpace = 0;

        public int TableSpace
        {
            get { return _TableSpace; }
            set { _TableSpace = value; }
        }
        /// <summary>
        /// 最小高度
        /// </summary>
        private int _minHeight = 0;

        private List<IndexInfo> _ints;
        /// <summary>
        /// 列分行起始索引集合
        /// </summary>
        public List<IndexInfo> Ints
        {
            get { return _ints; }
        }

        /// <summary>
        /// 最小高度
        /// </summary>
        public int MinHeight
        {
            get { return _minHeight; }
        }

        /// <summary>
        /// 表格大小
        /// </summary>
        private Size _TableSize;//表格大小
        /// <summary>
        /// 表格大小
        /// </summary>
        public Size TableSize
        {
            get { return _TableSize; }
        }

        private int _RowHeight = 0;
        /// <summary>
        ///行高度
        /// </summary>
        public int RowHeight
        {
            get { return _RowHeight; }
            set
            {
                _RowHeight = value;
                if (Param.ChangeDataSourceValue) NodeInfo.RowHeight = value;
            }
        }



        /// <summary>
        /// 列高度
        /// </summary>
        private int _ColHeight;

        public int ColHeight
        {
            get { return _ColHeight; }
            set
            {
                _ColHeight = value;
                if (Param.ChangeDataSourceValue) NodeInfo.ColHeight = value;
            }
        }

        /// <summary>
        /// 表格自动大小
        /// </summary>
        public bool TableAutoSize { get; set; }

        /// <summary>
        /// 表格列属性集合
        /// </summary>
        private ColInfo[] _ColInfos;

        /// <summary>
        /// 表格列属性集合
        /// </summary>
        public ColInfo[] ColInfos
        {
            get { return _ColInfos; }
            set
            {
                _ColInfos = value;
                if (Param.ChangeDataSourceValue) NodeInfo.ColSetInfos = value;
            }
        }

        /// <summary>
        /// 加载完毕事件重载
        /// </summary>
        public override void InitFinished()
        {
            CalcTableSize();
            base.InitFinished();            
            //               
            //修改：2010-12-20 何晓明
            //修改：表格内数据自动换行
            //SetDateTableCounts();
            RePaint();
            //
        }

        /// <summary>
        /// 表格样式
        /// </summary>
        public ITableStyle TableStyle { get; set; }

        /// <summary>
        /// 列总数
        /// </summary>
        public int ColCount { get; set; }

        /// <summary>
        /// 行总数
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// 表格
        /// </summary>
        private DataTable _Table;


        public List<RangeColumns> RangeColumns { get; set; }

        /// <summary>
        /// 表格
        /// </summary>
        public DataTable Table
        {
            get { return _Table; }
            set
            {
                _Table = value;
                ColCount = _Table.Columns.Count;//列总数 
                RowCount = _Table.Rows.Count;
            }
        }

        /// <summary>
        /// 表格Grid集合
        /// </summary>
        private List<Grid> Grids = new List<Grid>();

        /// <summary>
        /// 右键菜单
        /// </summary>
        private ContextMenuStrip cms = new ContextMenuStrip();

        /// <summary>
        /// 清除选择
        /// </summary>
        public void ClearSelection()
        {
            Grids.ForEach(grid => grid.Selection.ResetSelection(true));
        }


        /// <summary>
        ///构造默认的列属性集合
        /// </summary>
        /// <returns></returns>
        private void SetColInfos()
        {
            int colCount = _Table.Columns.Count; //列总数     
            var colInfosNew = new ColInfo[colCount];   //列设置集合      
            using (Graphics g = this.CreateGraphics())
            {
                for (int i = 0; i < colCount; i++)
                {
                    var col = _Table.Columns[i];
                    ColInfo info = null;
                    if (_ColInfos != null)
                    {
                        info = _ColInfos.FirstOrDefault(colInfo => colInfo != null && colInfo.Name == col.ColumnName);
                    }
                    if (info == null)
                    {
                        info = new ColInfo
                        {
                            CellFont = TableStyle.CellFont,
                            ExceptionColor = Color.Red,
                            HeaderFont = TableStyle.HeaderFont,
                            Visiable = true,
                            Name = col.ColumnName,
                            Expression = string.Empty
                        };

                        var sizeHeader = g.MeasureString(col.Caption, info.HeaderFont); //设计状态列最小宽度
                        _ColHeight = Math.Max((int)sizeHeader.Height + 12, ColHeight);
                        int headerWidth = (int)(sizeHeader.Width.GetNearInt()*1.2);  //列最小宽度

                        var sizeCell = g.MeasureString(Param.ValueFormat, info.CellFont);
                        _RowHeight = Math.Max((int)sizeCell.Height + 9, RowHeight);
                        int cellWidth = sizeCell.Width.GetNearInt()+10; //设计状态单元格最小宽度                   

                        //修改：2010-12-20 何晓明
                        //原因：统一初始列宽                           
                        //info.Width = Math.Max(headerWidth, cellWidth) + 8;
                        info.Width = 75;
                        //
                        if (!IsDesignMode) { }//非设计模式，该列宽度为本列所有格子的最大宽度
                    }
                    info.Caption = col.Caption;//列标题
                    colInfosNew[i] = info;
                }
            }
            _ColInfos = colInfosNew;
        }

        /// <summary>
        /// 计算表格大小
        /// </summary>
        private void CalcTableSize()
        {
            SetColInfos();
            cms.Items.Clear();
            int _TableWidth = 0;
            foreach (var colInfo in _ColInfos)
            {
                if (colInfo.Visiable) _TableWidth += colInfo.Width;
                var tsmi = new ToolStripMenuItem(colInfo.Caption);
                tsmi.CheckOnClick = true;
                tsmi.CheckStateChanged += new EventHandler(tsmi_CheckStateChanged);
                tsmi.Checked = colInfo.Visiable;
                cms.Items.Add(tsmi);
            }
            var _TableHeight = ColHeight + RowHeight * (IsDesignMode ? 1 : RowCount);//表格高度

            _TableSize = new Size(_TableWidth, _TableHeight);
        }

        /// <summary>
        /// Checked状态改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tsmi_CheckStateChanged(object sender, EventArgs e)
        {
            var clickItem = sender as ToolStripMenuItem;

            int index = cms.Items.IndexOf(clickItem);
            if (index != -1)
            {
                _ColInfos[index].Visiable = clickItem.Checked;
                RePaint();
            }
        }

        /// <summary>
        /// 行分页
        /// </summary>
        /// <param name="MaxY"></param>
        /// <param name="StartY"></param>
        /// <returns></returns>
        public List<IndexInfo> GetRowIndex(int MaxY, int StartY)
        {
            List<IndexInfo> SplitRowInfos = new List<IndexInfo>();
            int height0 = MaxY - StartY;//本页还剩余高度
            int n0 = (height0 - ColHeight) / RowHeight;//本页能显示行数

            //修改：2010-12-21 何晓明
            //原因：数据内容无法自动换行
            //需求：周巧
            //操作：注释
            //if (n0 > 0)
            //{
            //    int nTemp = n0 > RowCount ? RowCount : n0;
            //    SplitRowInfos.Add(new IndexInfo(0, nTemp - 1));
            //}
            //else
            //{
            //    n0 = 0;//本页一行数据都显示不全
            //}
            //int nRow = (MaxY - Panel.Page.HeaderHeight - ColHeight) / RowHeight;//每页能显示的行总数
            //int n1 = (RowCount - n0) % nRow;//最后一页行数
            //int nPage = (RowCount - n0) / nRow;//除去第一页，共分页数           

            //for (int i = 0; i < nPage; i++)
            //{
            //    int startrow = i * nRow + n0;
            //    int endrow = startrow + nRow - 1;
            //    SplitRowInfos.Add(new IndexInfo(startrow, endrow));
            //}

            //if (n1 > 0)//非整页
            //{
            //    int startrow = nPage * nRow + n0;
            //    int endrow = startrow + n1 - 1;
            //    SplitRowInfos.Add(new IndexInfo(startrow, endrow));
            //}
            //修改：2010-12-21 何晓明
            //原因：数据内容无法自动换行
            //需求：周巧
            //操作：新增
            //按内容分行后没列的行数
            int[] iColumnNewRows = new int[RowCount];
            for (int iTableRow = 0; iTableRow < RowCount; iTableRow++)
            {
                for (int iTableColumn = 0; iTableColumn < ColCount; iTableColumn++)
                {
                    string strContent = Table.Rows[iTableRow][iTableColumn].ToString();
                    int iColumnWidth = this.ColInfos[iTableColumn].Width;
                    int iCellRows = SplitStringByColumn(strContent, new PrintCell("").View.Font, iColumnWidth).Count;
                    //获取每一列在每一行按内容分行的最大行数
                    iColumnNewRows[iTableRow] = iCellRows > iColumnNewRows[iTableRow] ? iCellRows : iColumnNewRows[iTableRow];
                }
            }
            //所有行按列内容分行后的总行数
            int iColumnNewRowsCount = 0;

            for (int iTableRow = 0; iTableRow < RowCount; iTableRow++)
            {
                //按内容分行后每列的行数累加
                iColumnNewRowsCount += iColumnNewRows[iTableRow];
            }
            //当前所在行
            int iCurrentRow = 0;
            int iCurrentColumnRowsCount = 0;
            if (n0 > 0)
            {
                //如果内容分行后行数在首页剩余的行数内，全部放置
                if (iColumnNewRowsCount <= n0)
                {
                    SplitRowInfos.Add(new IndexInfo(0, RowCount-1));
                    return SplitRowInfos;
                }
                else
                {
                    //如果内容分行后行数不在首页剩余的行数内，寻找首页放置行数
                    for (; iCurrentRow < RowCount && iCurrentColumnRowsCount <= n0; iCurrentRow++)
                    {
                        iCurrentColumnRowsCount += iColumnNewRows[iCurrentRow];
                    }
                    //找到从零开始到可以放置的行数
                    SplitRowInfos.Add(new IndexInfo(0, iCurrentRow - 1));
                }
            }
            else
            {
                iCurrentRow = 0;
            }
            //计算每页能显示的行总数
            int iRowsPerPage = (MaxY - Panel.Page.HeaderHeight - ColHeight) / RowHeight;

            //所有行数分页
            iCurrentColumnRowsCount = 0;
            int iRowsStart = iCurrentRow;
            for (; iCurrentRow < RowCount; iCurrentRow++)
            {
                if (iCurrentColumnRowsCount <= iRowsPerPage)
                {
                    iCurrentColumnRowsCount += iColumnNewRows[iCurrentRow];
                }
                else
                {
                    SplitRowInfos.Add(new IndexInfo(iRowsStart, iCurrentRow - 1));
                    iCurrentColumnRowsCount = 0;
                    iRowsStart = iCurrentRow;
                }
                if (iCurrentRow == RowCount - 1 && iCurrentColumnRowsCount <= iRowsPerPage)
                {
                    SplitRowInfos.Add(new IndexInfo(iRowsStart, iCurrentRow));
                }
            }
            //

            return SplitRowInfos;
        }
        //修改：何晓明 2011-03-17
        //原因：导出EXCEL另外分页
        public List<IndexInfo> GetRowIndexToExcel(int MaxY, int StartY)
        {
            List<IndexInfo> SplitRowInfos = new List<IndexInfo>();
            int height0 = MaxY - StartY;//本页还剩余高度
            int n0 = (height0 - ColHeight) / RowHeight;//本页能显示行数

            //修改：2010-12-21 何晓明
            //原因：数据内容无法自动换行
            //需求：周巧
            //操作：注释
            if (n0 > 0)
            {
                int nTemp = n0 > RowCount ? RowCount : n0;
                SplitRowInfos.Add(new IndexInfo(0, nTemp - 1));
            }
            else
            {
                n0 = 0;//本页一行数据都显示不全
            }
            int nRow = (MaxY - Panel.Page.HeaderHeight - ColHeight) / RowHeight;//每页能显示的行总数
            int n1 = (RowCount - n0) % nRow;//最后一页行数
            int nPage = (RowCount - n0) / nRow;//除去第一页，共分页数           

            for (int i = 0; i < nPage; i++)
            {
                int startrow = i * nRow + n0;
                int endrow = startrow + nRow - 1;
                SplitRowInfos.Add(new IndexInfo(startrow, endrow));
            }

            if (n1 > 0)//非整页
            {
                int startrow = nPage * nRow + n0;
                int endrow = startrow + n1 - 1;
                SplitRowInfos.Add(new IndexInfo(startrow, endrow));
            }

            return SplitRowInfos;
        }
        ///// <summary>
        ///// 行分页
        ///// </summary>
        ///// <param name="MaxY"></param>
        ///// <param name="StartY"></param>
        ///// <returns></returns>
        //public List<SplitRowInfo> SplitTableRow(int MaxY, int StartY)
        //{
        //    List<SplitRowInfo> SplitRowInfos = new List<SplitRowInfo>();

        //    int height0 = MaxY - StartY;//本页还剩余高度

        //    int n0 = (height0 - ColHeight) / RowHeight;//本页能显示行数

        //    if (n0 > 0)
        //    {
        //        int nTemp = n0 > RowCount ? RowCount : n0;
        //        SplitRowInfos.Add(new SplitRowInfo
        //        {
        //            SplitTableType = SplitTableType.First,
        //            Indexs = new IndexInfo
        //            {
        //                Start = 0,
        //                End = nTemp - 1
        //            }
        //        });
        //    }
        //    else
        //    {
        //        n0 = 0;//本页一行数据都显示不全
        //    }

        //    int nRow = (MaxY - Panel.Page.HeaderHeight - ColHeight) / RowHeight;//每页能显示的行总数

        //    int n1 = (RowCount - n0) % nRow;//最后一页行数

        //    int nPage = (RowCount - n0) / nRow;//除去第一页，共分页数           

        //    for (int i = 0; i < nPage; i++)
        //    {
        //        int startrow = i * nRow + n0;
        //        int endrow = startrow + nRow - 1;
        //        SplitRowInfos.Add(new SplitRowInfo
        //        {
        //            SplitTableType = SplitTableType.Center,
        //            Indexs = new IndexInfo
        //            {
        //                Start = startrow,
        //                End = endrow
        //            }
        //        });
        //    }

        //    if (n1 > 0)//非整页
        //    {
        //        //nPage++;//分页增加
        //        int startrow = nPage * nRow + n0;
        //        int endrow = startrow + n1 - 1;
        //        SplitRowInfos.Add(new SplitRowInfo
        //        {
        //            SplitTableType = SplitTableType.End,
        //            Indexs = new IndexInfo
        //            {
        //                Start = startrow,
        //                End = endrow
        //            }
        //        });
        //    }

        //    return SplitRowInfos;
        //}

        /// <summary>
        /// 获取列分行起始索引集合
        /// </summary>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public List<IndexInfo> GetColSplits(int maxWidth)
        {
            List<IndexInfo> lst = new List<IndexInfo>();
            int startIndex = 0;
            foreach (var index in SplitCol(maxWidth))
            {
                lst.Add(new IndexInfo { Start = startIndex, End = index });
                startIndex = index;
            }
            if (startIndex < ColCount)
                lst.Add(new IndexInfo { Start = startIndex, End = ColCount });
            return lst;
        }

        /// <summary>
        /// 根据控件宽度拆分列
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> SplitCol(int maxWidth)
        {
            if (_TableSize.Width > maxWidth)
            {
                int colCount = _Table.Columns.Count;//总列数
                if (colCount > 0)
                {
                    if (_ColInfos[0].Width < maxWidth)
                    {
                        int totalWidth = 0;//总宽度
                        int j = 0;//记录开始换行的列索引

                        while (j + 1 < colCount)
                        {
                            for (int i = j; i < colCount; i++)
                            {
                                j = i;
                                totalWidth += _ColInfos[i].Visiable ? _ColInfos[i].Width : 0;
                                if (totalWidth >= maxWidth)
                                {
                                    yield return j;
                                    totalWidth = 0;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 对表格进行排版
        /// </summary>
        private void LayoutGrid()
        {
            base.Controls.Clear();
            int x = BaseRect.Left;
            int y = BaseRect.Top + TextVSpace + TextSize.Height - 2;
            int offsety = RowHeight + ColHeight + TableSpace - 1;
            Grid grid = null;
            for (int i = 0; i < _ints.Count; i++)
            {
                grid = this.CreateGrid(_ints[i], new IndexInfo { Start = 0, End = 0 }, true);
                grid.Location = new Point(x, y);
                y += offsety;
                base.Controls.Add(grid);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="panel"></param>
        public PrintTable(PrintPanel panel)
            : base(panel, true)
        {
            TableStyle = new TableStyle1();
            this.ContextMenuStrip = cms;
            //this.ContextMenuStrip = null;
        }


        /// <summary>
        /// 设置大小事件重载
        /// </summary>
        public override void SetSize()
        {
            if (base.Width == 0)
            {
                int minWidth = Math.Max(TextSize.Width, _TableSize.Width) + _DragImage.Width + Sqare.Width;
                minWidth = Math.Min(minWidth, MaximumSize.Width);
                base.Width = minWidth;//最小宽度  
                base.MinimumSize = new Size(Param.MinTableWidth, 20);
            }
            _ints = GetColSplits(ContentRect.Width);
            _minHeight = GetMinHeight();
            _minHeight = Math.Min(_minHeight, MaximumSize.Height);
            base.Height = _minHeight;
            LayoutGrid();
            Invalidate();
        }

        /// <summary>
        /// 重绘事件重载
        /// </summary>
        public override void RePaint()
        {
            Grids.Clear();
            base.RePaint();
        }

        /// <summary>
        /// 获取最小高度
        /// </summary>
        /// <returns></returns> 
        private int GetMinHeight()
        {
            return BaseRect.Top + TextSize.Height + TextVSpace
                + _ints.Count * _TableSize.Height
                + TableSpace * (_ints.Count - 1)
                + 4;
        }

        /// <summary>
        /// 创建Grid对象
        /// </summary>
        /// <returns></returns>
        private Grid CreateNewGrid()
        {
            Grid grid = new Grid();
            grid.BackColor = Color.White;
            grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            grid.Tag = this;
            Grids.Add(grid);
            return grid;
        }

        /// <summary>
        /// 创建Grid对象 输出时调用
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="isDesignMode"></param>
        /// <returns></returns>
        public Grid CreateGrid(IEnumerable<int> colIndex, IndexInfo rowIndex, bool isDesignMode)
        {
            if (colIndex == null || colIndex.Count() == 0)
                return null;
            int colCount = colIndex.Count();
            int rowCount = rowIndex.End - rowIndex.Start + 2;//总行数
            var grid = CreateNewGrid();
            grid.RowsCount = rowCount;//总行数
            grid.ColumnsCount = colCount;//总列数
            grid.FixedRows = 1;//表头不滚动
            IsDesignMode = false;
            int currentRowIndex = 0;
            int width = 0, height = 0;
            List<int> tempInt = colIndex.ToList();

            //修改：2010-12-20 何晓明
            //原因：数据内容无法自动换行
            int[] RowsHeight = new int[rowIndex.Count];
            //
            for (int i = 0; i < colCount; i++)
            {
                var c = colIndex.ElementAt(i);
                var info = _ColInfos[c];
                grid[0, i] = new PrintHeader(info.Caption);//表头  
                //if (Table.Columns[tempInt[i]] != null)
                    //grid.Columns[i].Name = Table.Columns[tempInt[i]].ColumnName;
                grid.Columns[i].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
                for (int j = rowIndex.Start; j <= rowIndex.End; j++)//数据
                {
                    currentRowIndex = j - rowIndex.Start;
                    string sValue = isDesignMode ? Param.ValueFormat : Table.Rows[j][c].ToString();

                    //修改：2010-12-20 何晓明
                    //原因：数据内容无法自动换行                    
                    //var cell = grid[currentRowIndex + 1, i];
                    //grid.Rows[currentRowIndex + 1].Height = _RowHeight;//行高度
                    var cell = new PrintCell(sValue);
                    int iWidth = this.ColInfos[i].Width;
                    string strList = string.Empty;
                    List<string> lstRows = SplitStringByColumn(sValue, cell.View.Font, iWidth);//SplitStringByColumn(sValue, 8, iWidth);//
                    int iNewRows = lstRows.Count;
                    for (int item = 0; item < lstRows.Count; item++)
                    {
                        strList += lstRows[item].ToString();
                    }
                    sValue = strList;
                    //cell.Value = sValue;                    
                    //grid[currentRowIndex + 1, i] = cell;
                    grid[currentRowIndex + 1, i] = new PrintCell(isDesignMode ? Param.ValueFormat :
                    sValue);
                    //每行获取最高行高
                    RowsHeight[j - rowIndex.Start] = iNewRows > RowsHeight[j - rowIndex.Start] ? iNewRows : RowsHeight[j - rowIndex.Start];
                    grid.Rows[currentRowIndex + 1].Height = _RowHeight * RowsHeight[j - rowIndex.Start];//行高度
                  
                    //
                    
                }
               
                grid.Columns[i].Width = info.Width;
                width += info.Width;
            }

            //修改：2010-12-21 何晓明
            //原因：数据内容无法自动换行
            rowCount = 0;
            for (int iColumnRows = 0; iColumnRows < rowIndex.Count;iColumnRows++ )
            {
                rowCount += RowsHeight[iColumnRows];
            }
            //grid.RowsCount = rowCount +1;//总行数
            //

            grid.Rows[0].Height = _ColHeight;//表头高度
            SetGridSytle(grid, TableStyle);//设置表格样式
            //修改：2010-12-21 何晓明
            //原因：数据内容无法自动换行
            //height = RowHeight * (rowCount - 1) + ColHeight;//总高度
            height = RowHeight * ( rowCount + 1) + ColHeight;
            //
            grid.Size = new Size(BaseRect.Width, height);
            return grid;
        }

        /// <summary>
        /// 创建Grid对象 界面显示时调用
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="isDesignMode"></param>
        /// <returns></returns>
        public Grid CreateGrid(IndexInfo colIndex, IndexInfo rowIndex, bool isDesignMode)
        {
            
            int rowCount = rowIndex.End - rowIndex.Start + 2;//总行数

            var grid = CreateNewGrid();

            grid.RowsCount = rowCount;//总行数
            grid.ColumnsCount = colIndex.End - colIndex.Start;//总列数

            grid.FixedRows = 1;//表头不滚动
            int currentColIndex = 0;
            int currentRowIndex = 0;
            int width = 0, height = 0;
            
            //修改：2010-12-20 何晓明
            //原因：数据内容无法自动换行
            int[] RowsHeight = new int[rowIndex.Count];

            for (int i = colIndex.Start; i < colIndex.End; i++)
            {
                //修改：何晓明 2011-03-03
                //原因：表格内容为空时不显示
                try
                {
                    string strValue = Table.Rows[0][i].ToString();//第一行是否存在
                }
                catch
                { 
                    break;
                }
                //
                currentColIndex = i - colIndex.Start;
                grid[0, currentColIndex] = new PrintHeader(_ColInfos[i].Caption);//表头               
                grid.Columns[currentColIndex].Tag = _ColInfos[i];//列与列属性对象绑定
                _ColInfos[i].GridColInfo = grid.Columns[currentColIndex];

                for (int j = rowIndex.Start; j <= rowIndex.End; j++)//数据
                {
                    currentRowIndex = j - rowIndex.Start;
                    string sValue = Table.Rows[j][i].ToString();
                    //grid[currentRowIndex + 1, currentColIndex] = new PrintCell(isDesignMode ? Param.ValueFormat :
                    //sValue);
                    //修改：2010-12-20 何晓明
                    //原因：数据内容无法自动换行                    
                    //var cell = grid[currentRowIndex + 1, i];
                    //grid.Rows[currentRowIndex + 1].Height = _RowHeight;//行高度
                    var cell = new PrintCell(sValue);
                    int iWidth = this.ColInfos[i].Width;
                    string strList = string.Empty;
                    List<string> lstRows = SplitStringByColumn(sValue, cell.View.Font, iWidth);//SplitStringByColumn(sValue, 8, iWidth);//
                    int iNewRows = lstRows.Count;
                    for (int item = 0; item < lstRows.Count; item++)
                    {
                        strList += lstRows[item].ToString();
                    }
                    sValue = strList;
                    //cell.Value = sValue;
                    //grid[currentRowIndex + 1, i] = cell;
                    grid[currentRowIndex + 1, currentColIndex] = new PrintCell(isDesignMode ? Param.ValueFormat :
                    sValue);
                    //每行获取最高行高
                    //RowsHeight[j - rowIndex.Start] = iNewRows > RowsHeight[j - rowIndex.Start] ? iNewRows : RowsHeight[j - rowIndex.Start];
                    //grid.Rows[currentRowIndex + 1].Height = _RowHeight * RowsHeight[j - rowIndex.Start];//行高度
                    grid.Rows[currentRowIndex + 1].Height = _RowHeight;//行高度
                    
                }
                grid.Columns[currentColIndex].Visible = _ColInfos[i].Visiable;
                //修改：2010-12-27 何晓明
                //原因：表格删除第一列后丢失表格左边框修正
                //需求：周巧
                //grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                //
                grid.Columns[currentColIndex].Width = _ColInfos[i].Width;
                grid.Columns[currentColIndex].MinimalWidth = Param.MinimalColWidth;
                grid.Columns[currentColIndex].MaximalWidth = Param.MaximalColWidth;
                width += _ColInfos[i].Width;
            }
            grid.Rows[0].Height = _ColHeight;//表头高度
            SetGridSytle(grid, TableStyle);//设置表格样式
            //修改：2010-12-20 何晓明
            //原因：数据内容无法自动换行 
            //height = RowHeight * (rowCount - 1) + ColHeight;//总高度
            //修改：2010-12-27 何晓明
            //原因：表格删除第一列后丢失表格左边框修正后不至于出现滚动条
            //需求：周巧
            double dOffsetRowRowHeight = 0.1 * RowHeight;
            int iOffsetRowHeight = dOffsetRowRowHeight.GetNearInt();
            height = RowHeight * (rowCount - 1) + iOffsetRowHeight + ColHeight;//总高度
            //
            grid.Size = new Size(BaseRect.Width, height);
            RegistEvent(grid);

            return grid;
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="grid"></param>
        private void RegistEvent(Grid grid)
        {
            ((ColumnInfoCollection)grid.Columns).ColumnWidthChanged += new ColumnInfoEventHandler(PrintTable_ColumnWidthChanged);
            grid.MouseUp += new MouseEventHandler(grid_MouseUp);
        }

        void grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Cursor.Current == Cursors.VSplit)
                {
                    RePaint();
                }
            }
        }

        private void PrintTable_ColumnWidthChanged(object sender, ColumnInfoEventArgs e)
        {
            var colInfo = e.Column.Tag as ColInfo;
            colInfo.Width = e.Column.Width;
            if (colInfo.NeedReLayout) RePaint();
            colInfo.NeedReLayout = false;
        }

        private void SetCellView(ICell cell, IView view,bool flag,ITableStyle style)
        {
            //bool showNormal = false;
            //if (!flag && RangeColumns != null && RangeColumns.Count > 0 && cell != null && cell.Value != null)
            //{
            //    //RangeColumns columnsRange = RangeColumns.Find(w => w.ColumnsName == cell.Column.Name);
            //    RangeColumns columnsRange = RangeColumns.Find(w => w.ColumnsName == cell.Grid.Name);
            //    string sValue = cell.Value.ToString();
            //    if ((sValue.IsInt() || sValue.IsNumeric()) && columnsRange != null)
            //    {
            //        double showValue = double.Parse(sValue);
            //        if (columnsRange.maxValue < showValue || columnsRange.minValue > showValue)
            //        {
            //            var borderLine = new BorderLine(style.BorderColor, 1);
            //            var viewCell = new SourceGrid.Cells.Views.Cell();
            //            viewCell.Font = style.CellFont;
            //            viewCell.ForeColor = Color.Red;
            //            viewCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            //            showNormal = true;
            //            viewCell.Border = new RectangleBorder(borderLine, borderLine);
            //            cell.View = viewCell;
            //        }
            //    }
            //}
            if ( cell != null)
                cell.View = view;
        }

        private void SetGridSytle(Grid grid, ITableStyle tableStyle)
        {
            var typ = tableStyle.GetType();
            var borderLine = new BorderLine(tableStyle.BorderColor, 1);
            if (typ == typeof(TableStyle1))
            {
                var selection = grid.Selection as SelectionBase;

                selection.BackColor = TableStyle.SelectBackColor;
                selection.FocusBackColor = TableStyle.SelectBackColor;

                var border = selection.Border;
                border.SetWidth(TableStyle.SelectBorderWidth);
                border.SetColor(TableStyle.SelectBorderColor);
                border.SetDashStyle(TableStyle.SelectBorderStyle);

                selection.Border = border;

                var view0 = PrintHelper.GetCellView(tableStyle, true);
                var view1 = PrintHelper.GetCellView(tableStyle, true);
                var view2 = PrintHelper.GetCellView(tableStyle, false);
                var view3 = PrintHelper.GetCellView(tableStyle, false);

                view0.Border = new RectangleBorder(borderLine);
                //修改：何晓明 2011-01-13
                //原因：设计视图下隐藏第一列后续列无左边线
                //view1.Border = new RectangleBorder(borderLine, borderLine, BorderLine.NoBorder, borderLine);//无左边线
                view1.Border = new RectangleBorder(borderLine,borderLine,borderLine,borderLine);
                //
                view2.Border = new RectangleBorder(BorderLine.NoBorder, borderLine, borderLine, borderLine);//无上边线              
                view3.Border = new RectangleBorder(borderLine, borderLine);//无上及左边线;

                SetCellView(grid[0, 0], view0, true, tableStyle);

                for (int i = 1; i < grid.ColumnsCount; i++)
                {
                    SetCellView(grid[0, i], view1, true, tableStyle);                    
                }

                for (int i = 1; i < grid.RowsCount; i++)//左第一列
                {
                    SetCellView(grid[i, 0], view2, true, tableStyle);
                }

                for (int i = 1; i < grid.RowsCount; i++)
                {
                    for (int j = 1; j < grid.ColumnsCount; j++)
                    {
                        //修改：何晓明 2011-01-13
                        //原因：设计视图下隐藏第一列后续列无左边线
                        SetCellView(grid[i, j], view2, false, tableStyle);  
                        //SetCellView(grid[i, j], view3, false, tableStyle);   
                        //
                    }
                }
               
            }
        }

        //修改：2010-12-20 何晓明
        //原因：数据内容按宽度分行
        public static List<string> SplitStringByColumn(string strValue, int PointPerByte,int width)
        {
            int byteCount = 0;
            List<string> lstColumnRows = new List<string>();
            string strNewRow = string.Empty;
            if (string.IsNullOrEmpty(strValue))
            {
                lstColumnRows.Add(strNewRow);
                return lstColumnRows;
            }
            for (int i = 0; i < strValue.Length;i++ )
            {
                string strTemp = strValue.Substring(i, 1);                
                byteCount+=Encoding.Default.GetByteCount(strTemp);

                if ((byteCount+1) * PointPerByte <= width)
                {
                    strNewRow += strTemp;
                }
                else
                {

                    strNewRow += "\n";
                    strValue=strValue.Insert(byteCount-1, "\n");
                    lstColumnRows.Add(strNewRow);
                    byteCount = 0;
                    strNewRow = string.Empty;
                }
                if (i == strValue.Length - 1 && byteCount * PointPerByte <= width)
                    lstColumnRows.Add(strNewRow);
            }
            lstColumnRows.TrimExcess();
            return lstColumnRows;
        }

        private List<string> SplitStringByColumn(string strValue, Font font, int width)
        {
            List<string> lstColumnRows = new List<string>();            
            string strNewRow = string.Empty;
            if (string.IsNullOrEmpty(strValue))
            {
                lstColumnRows.Add(strNewRow);
                return lstColumnRows;
            }
            using (Graphics g =this.CreateGraphics() )
            {
                for (int i = 0; i < strValue.Length; i++)
                {
                    string strTemp = strValue.Substring(i, 1);
                    int iStringPosition =g.MeasureString(strNewRow+strTemp,font).Width.GetNearInt()+20;
                    //修改：何晓明 2010-12-23 打印输入换行符时分行异常
                    //if (iStringPosition <= width)
                    if (iStringPosition <= width&&strTemp!="\n")
                    //
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

        private void SetDateTableCounts()
        {
            //记录DataTable行数
            int iTableCount = 0;
            //记录行可分行数
            int iRowNewCount = 0;
            //单元格分行数
            int iCellRowCount = 0;
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                for (int j = 0; j < Table.Columns.Count; j++)
                {
                    //单元格中每个数据
                    string strValue = this.Table.Rows[i][j].ToString();
                    //列宽
                    int iWidth = this.ColInfos[j].Width;
                    List<string> lstNewRow = SplitStringByColumn(strValue, 8, iWidth);
                    iCellRowCount = lstNewRow.Count;
                    if (iCellRowCount > iRowNewCount)
                    {
                        iRowNewCount = iCellRowCount;
                    }

                }
                iTableCount += iRowNewCount;
            }
            RowCount = iTableCount;
            //
        }

        private void SplitRowsToPage(int RowCount, int iCurrentRow, int iRowsPerPage, int[] iColumnNewRows, List<IndexInfo> SplitRowInfos)
        {
            int iCurrentColumnRowsCount = 0;
            int iRowsStart = iCurrentRow;
            for (; iCurrentRow < RowCount; iCurrentRow++)
            {
                if (iCurrentColumnRowsCount <= iRowsPerPage)
                {
                    iCurrentColumnRowsCount += iColumnNewRows[iCurrentRow];
                }
                else
                {
                    SplitRowInfos.Add(new IndexInfo(iRowsStart, iCurrentRow - 1));
                }
                if (iCurrentRow == RowCount-1)
                {
                    return;
                }
            }
            if (iCurrentRow == RowCount - 1)
            {
                return;
            }
        }
    }
}
