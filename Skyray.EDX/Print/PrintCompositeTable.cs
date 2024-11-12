using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Views;
using System.Drawing;
using DevAge.Drawing;
using SourceGrid.Selection;
using System.Windows.Forms;
using System.Reflection;

namespace Skyray.Print
{
    public class PrintCompositeTable : PrintCtrl
    {
        /// <summary>
        /// 行数
        /// </summary>
        private int _rowCount;

        public int RowCount
        {
            get { return _rowCount; }
            set { _rowCount = value; }
        }

        /// <summary>
        /// 列数
        /// </summary>
        private int _colCount;

        public int ColCount
        {
            get { return _colCount; }
            set { _colCount = value; }
        }

        public ContextMenuStrip cms = new ContextMenuStrip();


        private List<DataTable> _dTable;

        public List<DataTable> DTable
        {
            get { return _dTable; }
            set { _dTable = value; }
        }


        public List<object> DataSourceList { get; set; }

        /// <summary>
        /// 表格间距
        /// </summary>
        private int _TableSpace = 0;

        public int TableSpace
        {
            get { return _TableSpace; }
            set { _TableSpace = value; }
        }

        private PrintPanel pannel;


        private Size _TableSize;

        /// <summary>
        /// 表格样式
        /// </summary>
        public ITableStyle TableStyle { get; set; }

        private List<CellSize> _cellInfo;

        public List<CellSize> CellInfo
        {
            get { return _cellInfo; }
            set
            {
                _cellInfo = value;
                if (value != null && value.Count > 0)
                {
                    columnCount = value.Count;
                    rowsCount = value[0].RowsInfo.Count;
                }
            }
        }

        private int colWidth = 80;

        private int rowHeight = 30;

        private int rowsCount = 5;

        private int columnCount = 7;

        public Grid grid;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        public PrintCompositeTable(PrintPanel panel)
            : base(panel, true)
        {
            TableStyle = new TableStyle1();
            this.pannel = panel;
            CellInfo = new List<CellSize>();
            this.ContextMenuStrip = cms;
            CreateMenuStrip();
            InitContructTable();

        }

        private void InitContructTable()
        {
            for (int i = 0; i < columnCount; i++)
            {
                var test = new CellSize();
                ColInfo info = new ColInfo();
                info.Width = colWidth;
                List<RowInfo> rowList = new List<RowInfo>();
                for (int j = 0; j < rowsCount; j++)
                {
                    RowInfo jf = new RowInfo();
                    jf.RowHeight = rowHeight;
                    rowList.Add(jf);
                }
                test.ColumnInfo = info;
                test.RowsInfo = rowList;
                CellInfo.Add(test);
            }
            LayoutGrid();
        }


        #region 上下文事件
        private void CreateMenuStrip()
        {
            string[] temp = { PrintInfo.TopInsertRow, PrintInfo.BottomInsertRow, PrintInfo.MergeCell, PrintInfo.DeleteRow,PrintInfo.LeftInsertColumn,
                                PrintInfo.RightInsertColumn,PrintInfo.DeleteCurrentColumn,PrintInfo.SpecifiedDataSource };
            for (int i = 0; i < temp.Length; i++)
            {
                var tsmi = new ToolStripMenuItem(temp[i]);
                tsmi.Click += new EventHandler(tsmi_Click);
                cms.Items.Add(tsmi);
            }
        }

        private void tsmi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem vs = sender as ToolStripMenuItem;
            if (vs.Text == PrintInfo.TopInsertRow)
                TopInsertRow();
            else if (vs.Text == PrintInfo.BottomInsertRow)
                BottomInsertRow();
            else if (vs.Text == PrintInfo.MergeCell)
                MergeCell();
            else if (vs.Text == PrintInfo.DeleteRow)
                DeleteRow();
            else if (vs.Text == PrintInfo.LeftInsertColumn)
                LeftInsertColumn();
            else if (vs.Text == PrintInfo.RightInsertColumn)
                RightInsertColumn();
            else if (vs.Text == PrintInfo.DeleteCurrentColumn)
                DeleteCurrentColumn();
            else if (vs.Text == PrintInfo.SpecifiedDataSource)
            {
                RecurseTreeObj objForm = new RecurseTreeObj();
                objForm.Obj = DataSourceList[0].GetType(); ;
                objForm.SelectDataEnd += delegate(object data, EventArgs ec)
                {
                    RangeRegion region = grid.Selection.GetSelectionRegion();
                    foreach (Position position in region.GetCellsPositions())
                    {
                        grid[position.Row, position.Column].Value = (data as TreeNode).Text;
                        grid[position.Row, position.Column].View.ForeColor = Color.Red;
                        CellInfo[position.Column].RowsInfo[position.Row].AttrubuteName = (data as TreeNode).Text;
                        CellInfo[position.Column].RowsInfo[position.Row].CellText = (data as TreeNode).Text;
                        CellInfo[position.Column].RowsInfo[position.Row].Path = (data as TreeNode).FullPath;
                        if ((data as TreeNode).Tag != null && (bool)(data as TreeNode).Tag)
                            CellInfo[position.Column].RowsInfo[position.Row].IsEnumerable = true;
                    }
                };
                PrintHelper.OpenUC(objForm, PrintInfo.SpecifiedDataSource);
            }
        }

        private void DeleteCurrentColumn()
        {
            if (this.grid != null)
            {
                int[] columnIndex = grid.Selection.GetSelectionRegion().GetColumnsIndex();
                if (columnIndex.Length == 0)
                    return;
                int column = columnIndex[0];
                grid.Width -= grid.Columns[column].Width;
                base.Width -= grid.Columns[column].Width;
                grid.Columns.Remove(column);
                CellInfo.RemoveAt(column);
            }
        }

        private void RightInsertColumn()
        {
            if (this.grid != null)
            {
                int[] columnIndex = grid.Selection.GetSelectionRegion().GetColumnsIndex();
                if (columnIndex.Length == 0)
                    return;
                int column = columnIndex[columnIndex.Length - 1] + 1;
                grid.Columns.Insert(column);
                CellSize sizeColumn = new CellSize();
                ColInfo colInfo = new ColInfo();
                sizeColumn.ColumnInfo = colInfo;
                sizeColumn.RowsInfo = new List<RowInfo>();
                int z = 0;
                while (z < grid.Rows.Count)
                {
                    RowInfo rowInfo = new RowInfo();
                    sizeColumn.RowsInfo.Add(rowInfo);
                    z++;
                }
                CellInfo.Insert(column, sizeColumn);
                grid.Columns[column].Tag = sizeColumn;
                grid.Columns[column].Width = grid.Columns[columnIndex[0]].Width;
                for (int i = 0; i < grid.Rows.Count; i++)
                    grid[i, column] = new CompositeCell();
                SetGridSytle(grid, this.TableStyle, true);
                grid.Selection.FocusColumn(column);
                grid.Width += grid.Columns[column].Width;
                base.Width += grid.Columns[column].Width;
            }
        }

        private void LeftInsertColumn()
        {
            if (this.grid != null)
            {
                int[] columnIndex = grid.Selection.GetSelectionRegion().GetColumnsIndex();
                if (columnIndex.Length == 0)
                    return;
                int column = columnIndex[0];
                grid.Columns.Insert(column);
                CellSize sizeColumn = new CellSize();
                ColInfo colInfo = new ColInfo();
                sizeColumn.ColumnInfo = colInfo;
                sizeColumn.RowsInfo = new List<RowInfo>();
                int z = 0;
                while (z < grid.Rows.Count)
                {
                    RowInfo rowInfo = new RowInfo();
                    sizeColumn.RowsInfo.Add(rowInfo);
                    z++;
                }
                CellInfo.Insert(column, sizeColumn);
                grid.Columns[column].Tag = sizeColumn;
                if (grid.Columns[columnIndex[0] + 1] != null)
                    grid.Columns[column].Width = grid.Columns[columnIndex[0] + 1].Width;
                for (int i = 0; i < grid.Rows.Count; i++)
                    grid[i, column] = new CompositeCell();
                SetGridSytle(grid, this.TableStyle, true);
                grid.Selection.FocusColumn(column);
                grid.Width += grid.Columns[column].Width;
                base.Width += grid.Columns[column].Width;
            }
        }

        private void DeleteRow()
        {
            if (this.grid != null)
            {
                int[] rowsIndex = grid.Selection.GetSelectionRegion().GetRowsIndex();
                if (rowsIndex.Length == 0)
                    return;
                int row = rowsIndex[0];
                grid.Height -= grid.Rows[row].Height;
                base.Height -= grid.Rows[row].Height;
                grid.Rows.Remove(row);
                foreach (CellSize temp in CellInfo)
                {
                    temp.RowsInfo.RemoveAt(row);
                }
            }
        }

        private void BottomInsertRow()
        {
            if (this.grid != null)
            {
                int[] rowsIndex = grid.Selection.GetSelectionRegion().GetRowsIndex();
                if (rowsIndex.Length == 0)
                    return;
                int row = rowsIndex[0] + 1;
                grid.Rows.Insert(row);
                foreach (CellSize temp in CellInfo)
                {
                    RowInfo rowInfo = new RowInfo();
                    rowInfo.RowHeight = grid.Rows[row].Height;
                    temp.RowsInfo.Insert(row, rowInfo);
                    grid.Rows[row].Tag = temp;
                }
                grid.Rows[row].Height = grid.Rows[rowsIndex[0]].Height;
                for (int i = 0; i < grid.Columns.Count; i++)
                    grid[row, i] = new CompositeCell();
                SetGridSytle(grid, this.TableStyle, true);
                grid.Selection.FocusRow(row);
                grid.Height += grid.Rows[row].Height;
                base.Height += grid.Rows[row].Height;
            }
        }

        private void MergeCell()
        {
            if (this.grid != null)
            {
                RangeRegion rangeRegion = grid.Selection.GetSelectionRegion();
                SourceGrid.Cells.ICell cell;
                foreach (SourceGrid.Range range in rangeRegion)
                {
                    cell = grid[range.Start.Row, range.Start.Column];
                    foreach (Position ps in range.GetCellsPositions())
                    {
                        grid[ps.Row, ps.Column] = null;
                        CellInfo[ps.Column].RowsInfo[ps.Row].IsMerge = true;
                    }
                    grid[range.Start.Row, range.Start.Column] = cell;

                    grid[range.Start.Row, range.Start.Column].RowSpan = range.RowsCount;
                    grid[range.Start.Row, range.Start.Column].ColumnSpan = range.ColumnsCount;
                    CellInfo[range.Start.Column].RowsInfo[range.Start.Row].RowSpan = range.RowsCount;
                    CellInfo[range.Start.Column].RowsInfo[range.Start.Row].ColumnSpanm = range.ColumnsCount;
                    CellInfo[range.Start.Column].RowsInfo[range.Start.Row].IsMerge = false;
                    grid[range.Start.Row, range.Start.Column].Tag = "MergedCell[" + range.RowsCount + "," + range.ColumnsCount + "]";
                    grid.InvalidateCell(grid[range.Start.Row, range.Start.Column]);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void TopInsertRow()
        {
            if (this.grid != null)
            {
                int[] rowsIndex = grid.Selection.GetSelectionRegion().GetRowsIndex();
                if (rowsIndex.Length == 0)
                    return;
                int row = rowsIndex[0];
                grid.Rows.Insert(row);
                foreach (CellSize temp in CellInfo)
                {
                    RowInfo rowInfo = new RowInfo();
                    rowInfo.RowHeight = grid.Rows[row].Height;
                    temp.RowsInfo.Insert(row, rowInfo);
                    grid.Rows[row].Tag = temp;
                }
                if (grid.Rows[rowsIndex[0] + 1] != null)
                    grid.Rows[row].Height = grid.Rows[rowsIndex[0] + 1].Height;
                for (int i = 0; i < grid.Columns.Count; i++)
                    grid[row, i] = new CompositeCell();
                SetGridSytle(grid, this.TableStyle, true);
                grid.Selection.FocusRow(row);
                grid.Height += grid.Rows[row].Height;
                base.Height += grid.Rows[row].Height;
            }
        }
        #endregion

        /// <summary>
        /// 对表格进行排版
        /// </summary>
        private void LayoutGrid()
        {
            base.Controls.Clear();
            int x = BaseRect.Left;
            int y = BaseRect.Top + TextVSpace + TextSize.Height - 2;
            grid = new Grid();

            grid.Tag = this;
            grid.RowsCount = rowsCount;
            grid.ColumnsCount = columnCount;
            grid.Location = new Point(x, y);

            for (int i = 0; i < grid.ColumnsCount; i++)
            {
                for (int j = 0; j < grid.RowsCount; j++)
                {
                    if (!CellInfo[i].RowsInfo[j].IsMerge)
                    {
                        grid.Rows[j].Height = CellInfo[i].RowsInfo[j].RowHeight;
                        grid.Rows[j].Tag = CellInfo[i];
                        var cell = new CompositeCell(CellInfo[i].RowsInfo[j].CellText);
                        grid[j, i] = cell;
                        //grid[j, i] = new SourceGrid.Cells.Image();
                        if (CellInfo[i].RowsInfo[j].RowSpan > 0)
                            grid[j, i].RowSpan = CellInfo[i].RowsInfo[j].RowSpan;
                        if (CellInfo[i].RowsInfo[j].ColumnSpanm > 0)
                            grid[j, i].ColumnSpan = CellInfo[i].RowsInfo[j].ColumnSpanm;
                    }
                }
                grid.Columns[i].Width = CellInfo[i].ColumnInfo.Width;
                grid.Columns[i].Tag = CellInfo[i];
            }
            SetGridSytle(grid, this.TableStyle, true);
            RegistEvent(grid);
            grid.AutoScroll = false;
            this.Controls.Add(grid);
        }

        public Grid CreateGrid(int dataIndex)
        {
            int x = BaseRect.Left;
            int y = BaseRect.Top + TextVSpace + TextSize.Height - 2;
            Grid coloneGrind = new Grid();
            coloneGrind.Tag = this;
            coloneGrind.RowsCount = grid.Rows.Count;
            coloneGrind.ColumnsCount = grid.Columns.Count;
            coloneGrind.Location = new Point(x, y);

            for (int i = 0; i < coloneGrind.ColumnsCount; i++)
            {
                for (int j = 0; j < coloneGrind.RowsCount; j++)
                {
                    if (!CellInfo[i].RowsInfo[j].IsMerge)
                    {
                        coloneGrind.Rows[j].Height = CellInfo[i].RowsInfo[j].RowHeight;
                        coloneGrind.Rows[j].Tag = CellInfo[i];
                        var cell = new CompositeCell(CellInfo[i].RowsInfo[j].CellText);
                        coloneGrind[j, i] = cell;
                        if (CellInfo[i].RowsInfo[j].RowSpan > 0)
                            coloneGrind[j, i].RowSpan = CellInfo[i].RowsInfo[j].RowSpan;
                        if (CellInfo[i].RowsInfo[j].ColumnSpanm > 0)
                            coloneGrind[j, i].ColumnSpan = CellInfo[i].RowsInfo[j].ColumnSpanm;
                    }
                }
                coloneGrind.Columns[i].Width = CellInfo[i].ColumnInfo.Width;
                coloneGrind.Columns[i].Tag = CellInfo[i];
            }
            coloneGrind.Size = this.grid.Size;
            if (CellInfo != null && CellInfo.Count > 0)
            {
                DataTable dt = this.DTable[dataIndex];
                List<int> listInt = new List<int>();
                for (int i = 0; i < CellInfo.Count; i++)
                {
                    ColInfo colInfo = CellInfo[i].ColumnInfo;
                    List<RowInfo> listRow = CellInfo[i].RowsInfo;
                    for (int j = 0; j < listRow.Count; j++)
                    {
                        if (coloneGrind[j, i] == null)
                            continue;
                        coloneGrind[j, i].View.ForeColor = Color.Black;
                        if (!string.IsNullOrEmpty(listRow[j].AttrubuteName))
                        {
                            Object obj = DataSourceList[dataIndex];
                            if (!listRow[j].IsEnumerable)
                            {
                                if (listRow[j].AttrubuteName.Equals("PuPath"))
                                {
                                    coloneGrind[j, i].Image = System.Drawing.Image.FromFile(obj.GetType().GetProperty(listRow[j].AttrubuteName).GetValue(obj, null).ToString());//System.Drawing.Image(new SourceGrid.Cells.Image(obj.GetType().GetProperty(listRow[j].AttrubuteName).GetValue(obj, null)));
                                    coloneGrind[j, i].Value = "";
                                }
                                else
                                {
                                    coloneGrind[j, i].Value = obj.GetType().GetProperty(listRow[j].AttrubuteName).GetValue(obj, null);
                                }
                            }
                        }
                    }
                }
                if (dt != null)
                {
                    int rowIndex = 0;
                    int columnIndex = 0;
                    bool result = false;
                    foreach (CellSize cellRow in CellInfo)
                    {
                        RowInfo cellInfo = cellRow.RowsInfo.Find(w => w.IsEnumerable);
                        if (cellInfo != null)
                        {
                            rowIndex = cellRow.RowsInfo.IndexOf(cellInfo);
                            columnIndex = CellInfo.IndexOf(cellRow);
                            coloneGrind.Height -= cellInfo.RowHeight;
                            result = true;
                            break;
                        }
                    }
                    if (result)
                    {
                        coloneGrind.Rows.Remove(rowIndex);
                        rowIndex--;
                        int falgIndex = rowIndex;
                        for (int z = 0; z < dt.Rows.Count; z++)
                        {
                            rowIndex++;
                            DynamicAddGridByEnumerable(rowIndex, dt.Rows[z], columnIndex, dt.Columns.Count, coloneGrind, falgIndex);
                        }
                    }
                }
            }
            SetGridSytle(coloneGrind, this.TableStyle, false);

            return coloneGrind;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="obj"></param>
        private void DynamicAddGridByEnumerable(int rowIndex, DataRow obj, int startColumns, int dtColumns, Grid backGrid, int falgIndex)
        {
            //if (rowIndex < 1)
            //    return;

            int width = backGrid.Columns[backGrid.Columns.Count - 1].Width;
            if (backGrid.Columns.Count < dtColumns)
            {
                for (int c = backGrid.Columns.Count; c < dtColumns; c++)
                {
                    backGrid.Columns.Insert(c);
                    //grid.Columns.Insert(c);
                    CellSize sizeColumn = new CellSize();
                    ColInfo colInfo = new ColInfo();
                    sizeColumn.ColumnInfo = colInfo;
                    sizeColumn.RowsInfo = new List<RowInfo>();
                    int z = 0;
                    while (z < backGrid.Rows.Count)
                    {
                        RowInfo rowInfo = new RowInfo();
                        sizeColumn.RowsInfo.Add(rowInfo);
                        z++;
                    }
                    CellInfo.Insert(c, sizeColumn);
                    backGrid.Columns[c].Tag = sizeColumn;
                    backGrid.Columns[c].Width = width;
                    for (int i = 0; i < backGrid.Rows.Count; i++)
                        backGrid[i, c] = new CompositeCell();
                    SetGridSytle(backGrid, this.TableStyle, true);
                    backGrid.Selection.FocusColumn(c);
                    //_cellInfo.Add(sizeColumn);
                    backGrid.Width += backGrid.Columns[c].Width;
                    base.Width += backGrid.Columns[c].Width;
                }
            }
            //
            backGrid.Rows.Insert(rowIndex);
            backGrid.Rows[rowIndex].Height = backGrid.Rows[rowIndex].Height;
            //for (int i = startColumns,z=0; i < grid.Columns.Count; i++)
            for (int i = startColumns, z = 0; i < backGrid.Columns.Count; i++)
            {
               // if (!CellInfo[i].RowsInfo[falgIndex].IsMerge)
                {
                    string str = string.Empty;
                    if (z < dtColumns)
                    {
                        str = obj[z].ToString();
                        z++;
                    }
                    backGrid[rowIndex, i] = new CompositeCell(str);
                    //backGrid[rowIndex, i].RowSpan = backGrid[rowIndex-1 , i].RowSpan;
                    //backGrid[rowIndex, i].ColumnSpan = backGrid[rowIndex-1, i].ColumnSpan;
                    backGrid[rowIndex, i].RowSpan = backGrid[rowIndex, i].RowSpan;
                    backGrid[rowIndex, i].ColumnSpan = backGrid[rowIndex, i].ColumnSpan;
                }
            }
            backGrid.Height += backGrid.Rows[rowIndex].Height;
            //base.Height = backGrid.Height;
        }


        /// <summary>
        /// 
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
            int _minHeight = GetMinHeight();
            _minHeight = Math.Min(_minHeight, MaximumSize.Height);
            base.Height = _minHeight;
            Invalidate();
        }

        /// <summary>
        /// 获取最小高度
        /// </summary>
        /// <returns></returns> 
        private int GetMinHeight()
        {
            return BaseRect.Top + TextSize.Height + TextVSpace
                + _TableSize.Height
                + TableSpace * (rowsCount - 1)
                + 4;
        }

        /// <summary>
        /// 加载完毕事件重载
        /// </summary>
        public override void InitFinished()
        {
            LayoutGrid();
            CalcTableSize();
            base.InitFinished();
            RePaint();

        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcTableSize()
        {
            #region
            //if (CellInfo == null || CellInfo.Count == 0)
            //    return;
            //int _TableWidth = 0;
            //var _TableHeight = 0;
            //foreach (var colInfo in CellInfo)
            //{
            //    _TableWidth += colInfo.ColumnInfo.Width;
            //}
            //foreach (var rowInfo in CellInfo[0].RowsInfo)
            //{
            //    _TableHeight += rowInfo.RowHeight;
            //}
            //_TableSize = new Size(_TableWidth, _TableHeight);
            #endregion

            if (CellInfo == null || CellInfo.Count == 0)
                return;
            int _TableWidth = 0;
            var _TableHeight = 0;
            int rowIndex = 0;
            int colIndex = 0;
            foreach (var colInfo in CellInfo)
            {
                _TableWidth += colInfo.ColumnInfo.Width;
                if (colIndex++ >= 7)
                {
                    _TableWidth += 10;
                }
            }
            foreach (var rowInfo in CellInfo[0].RowsInfo)
            {
                _TableHeight += rowInfo.RowHeight;
                if (rowIndex++ >= 5)
                {
                    _TableHeight += 10;
                }
            }
            _TableSize = new Size(_TableWidth, _TableHeight);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="grid"></param>
        private void RegistEvent(Grid grid)
        {
            ((ColumnInfoCollection)grid.Columns).ColumnWidthChanged += new ColumnInfoEventHandler(PrintCompositeTable_ColumnWidthChanged);
            ((RowInfoCollection)grid.Rows).RowHeightChanged += new RowInfoEventHandler(PrintCompositeTable_RowHeightChanged);
            grid.MouseUp += new MouseEventHandler(grid_MouseUp);
        }

        void grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Cursor.Current == Cursors.VSplit)
                {
                    //RePaint();
                }
            }
        }

        private void PrintCompositeTable_RowHeightChanged(object sender, RowInfoEventArgs e)
        {
            var colInfo = e.Row.Tag as CellSize;
            grid.Height += e.Row.Height - colInfo.RowsInfo[e.Row.Index].RowHeight;
            base.Height += e.Row.Height - colInfo.RowsInfo[e.Row.Index].RowHeight;
            colInfo.RowsInfo[e.Row.Index].RowHeight = e.Row.Height;
            if (grid.Height > MaximumSize.Height)
                grid.Height = MaximumSize.Height;
            if (base.Height > MaximumSize.Height)
                base.Height = MaximumSize.Height;
        }

        private void PrintCompositeTable_ColumnWidthChanged(object sender, ColumnInfoEventArgs e)
        {
            var colInfo = e.Column.Tag as CellSize;
            grid.Width += e.Column.Width - colInfo.ColumnInfo.Width;
            base.Width += e.Column.Width - colInfo.ColumnInfo.Width;
            colInfo.ColumnInfo.Width = e.Column.Width;
            if (grid.Width > MaximumSize.Width)
                grid.Width = MaximumSize.Width;
            if (base.Width > MaximumSize.Width)
                base.Width = MaximumSize.Width;
        }

        /// <summary>
        /// 清除选择
        /// </summary>
        public void ClearSelection()
        {
            grid.Selection.ResetSelection(true);
        }

        public override void RePaint()
        {
            base.RePaint();
            CalcTableSize();

            if (grid != null)
                grid.Size = _TableSize;

        }
        List<string> ls = new List<string>();
        private void SetGridSytle(Grid grid, ITableStyle tableStyle, bool flag)
        {
            AddStr();
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
                for (int i = 0; i < grid.RowsCount; i++)
                {
                    for (int j = 0; j < grid.ColumnsCount; j++)
                    {
                        if (grid[i, j] != null && grid[i, j].View.ForeColor != Color.Red)
                        {
                            var view0 = PrintHelper.GetCellView(tableStyle, false);
                            view0.Border = new RectangleBorder(borderLine);
                            if (flag && !string.IsNullOrEmpty(CellInfo[j].RowsInfo[i].AttrubuteName))
                            {
                                view0.ForeColor = Color.Red;
                            }
                            if (!flag && grid[i, j].Value != null && grid[i, j].Value.ToString().Contains("~"))
                            {
                                string strvalue = grid[i, j].Value.ToString();
                                grid[i, j].Value = strvalue.Substring(0, strvalue.IndexOf("~"));
                                if (strvalue.Substring(strvalue.IndexOf("~") + 1).Length > 2)
                                {
                                    view0.ForeColor = Color.FromName(strvalue.Substring(strvalue.IndexOf("~") + 1));
                                }
                            }
                            SetCellView(grid[i, j], view0, false, tableStyle);
                        }
                    }
                }
            }
        }

        private void SetCellView(ICell cell, IView view, bool flag, ITableStyle style)
        {
            if (cell != null)
            {
                cell.View = view;
                if (cell.Value != null && FontBold(cell.Value.ToString()))
                {
                    view.Font = new Font(this.Font, FontStyle.Bold);
                    cell.View = view;
                }
            }
        }
        /// <summary>
        /// 加粗字符串
        /// </summary>
        private void AddStr()
        {
            ls.Clear();
            //ls.Add("样品名称");
            //ls.Add("测量时间");
            //ls.Add("供应商");
            //ls.Add("管压");
            //ls.Add("操作员");
            //ls.Add("管流");
            //ls.Add("批号");
            //ls.Add("仪器型号");
            //ls.Add("待检单号");
            //ls.Add("送测单位");
            //ls.Add("元素");
            //ls.Add("强度");
            //ls.Add("含量(ppm)");
            //ls.Add("误差(ppm)");
            //ls.Add("限定标准");
            //ls.Add("判定");
            //ls.Add("测量日期");
            //ls.Add("工作曲线");
            //ls.Add("仪器型号");
            //ls.Add("总判定");
            //ls.Add("次数");
        }

        private bool FontBold(string str)
        {
            return ls.Contains(str);
        }
    }

}
