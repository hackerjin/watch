using System;
using System.Collections.Generic;
using System.Text;
using SourceGrid;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace Skyray.Print
{
    public static class SourceGridHelper
    {

        /// <summary>
        /// 获取整形数组中最小值
        /// </summary>
        /// <param name="intArray"></param>
        /// <returns></returns>
        public static int GetMinValue(int[] intArray)
        {
            int intCount = intArray.Length;
            int intMin = intArray[0];
            for (int i = 1; i < intArray.Length; i++)
            {
                if (intMin > intArray[i])
                {
                    intMin = intArray[i];
                }
            }
            return intMin;
        }

        /// <summary>
        /// 获取整形数组中最大值
        /// </summary>
        /// <param name="intArray"></param>
        /// <returns></returns>
        public static int GetMaxValue(int[] intArray)
        {
            int intCount = intArray.Length;
            int intMax = intArray[0];
            for (int i = 1; i < intArray.Length; i++)
            {
                if (intMax < intArray[i])
                {
                    intMax = intArray[i];
                }
            }
            return intMax;
        }

        /// <summary>
        /// 取消合并单元格
        /// </summary>
        /// <param name="grid"></param>
        public static void UnMergeCells(Grid grid)
        {
            RangeRegion rangeRegion = grid.Selection.GetSelectionRegion();
            foreach (SourceGrid.Range range in rangeRegion)
            {
                foreach (Position ps in range.GetCellsPositions())
                {

                    if (grid[ps.Row, ps.Column] != null)
                    {
                        int intColSpan = grid[ps.Row, ps.Column].ColumnSpan;
                        int intRowSpan = grid[ps.Row, ps.Column].RowSpan;
                        if (intColSpan > 1)
                        {
                            grid[ps.Row, ps.Column].ColumnSpan = 1;
                        }
                        if (intRowSpan > 1)
                        {
                            grid[ps.Row, ps.Column].RowSpan = 1;
                        }
                        for (int i = 0; i < intRowSpan; i++)
                        {
                            for (int j = 1; j < intColSpan; j++)
                            {
                                grid[ps.Row + i, ps.Column + j] = new SourceGrid.Cells.Cell(string.Empty, typeof(string));
                                grid.InvalidateCell(grid[ps.Row + i, ps.Column + j]);
                            }
                        }
                    }
                    else
                    {
                        grid[ps.Row, ps.Column] = new SourceGrid.Cells.Cell(string.Empty, typeof(string));
                    }
                    grid.InvalidateCell(grid[ps.Row, ps.Column]);
                }
            }

        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="grid"></param>
        public static void MergeCells(Grid grid)
        {
            RangeRegion rangeRegion = grid.Selection.GetSelectionRegion();
            SourceGrid.Cells.ICell cell;
            foreach (SourceGrid.Range range in rangeRegion)
            {
                cell = grid[range.Start.Row, range.Start.Column];
                foreach (Position ps in range.GetCellsPositions())
                {
                    grid[ps.Row, ps.Column] = null;
                    //grid[ps.Row, ps.Column].Tag = string.Empty;
                }
                grid[range.Start.Row, range.Start.Column] = cell;

                grid[range.Start.Row, range.Start.Column].RowSpan = range.RowsCount;
                grid[range.Start.Row, range.Start.Column].ColumnSpan = range.ColumnsCount;
                grid[range.Start.Row, range.Start.Column].Tag = "MergedCell[" + range.RowsCount + "," + range.ColumnsCount + "]";
                grid.InvalidateCell(grid[range.Start.Row, range.Start.Column]);
            }
        }

        


        public static DataTable GridToDataTable(SourceGrid.Grid grid)
        {
            DataTable dtbl = new DataTable();
            for (int c = 0; c < grid.ColumnsCount; c++)
            {
                string strColName = grid[0, c].DisplayText;
                dtbl.Columns.Add(strColName);
            }

            int intColCount = dtbl.Columns.Count;
            for (int r = 1; r < grid.RowsCount; r++)
            {
                string[] strs = new string[intColCount];

                for (int c = 0; c < intColCount; c++)
                {
                    strs[c] = grid[r, c].DisplayText;
                }
                dtbl.Rows.Add(strs);
            }
            dtbl.AcceptChanges();
            return dtbl;
        }


        /// <summary>
        /// 全边框表格处理
        /// </summary>
        /// <param name="grid"></param>
        public static void AddCompleteBorder(SourceGrid.Grid grid)
        {
            SourceGrid.Cells.Views.Cell cellViewFirst = (SourceGrid.Cells.Views.Cell)grid[0, 0].View.Clone();
            cellViewFirst.Border = new DevAge.Drawing.RectangleBorder(DevAge.Drawing.BorderLine.Black1Width, DevAge.Drawing.BorderLine.Black1Width,
                DevAge.Drawing.BorderLine.Black1Width, DevAge.Drawing.BorderLine.Black1Width);
            grid[0, 0].View = cellViewFirst;

            for (int r = 1; r < grid.RowsCount; r++)
            {
                SourceGrid.Cells.Views.Cell cellView = (SourceGrid.Cells.Views.Cell)grid[r, 0].View.Clone();
                cellView.Border = new DevAge.Drawing.RectangleBorder(DevAge.Drawing.BorderLine.NoBorder, DevAge.Drawing.BorderLine.Black1Width,
                    DevAge.Drawing.BorderLine.Black1Width, DevAge.Drawing.BorderLine.Black1Width);

                grid[r, 0].View = cellView;
            }
        }


        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="intRowCount"></param>
        /// <param name="intColCount"></param>
        public static SourceGrid.Grid CreateSourceGrid(SourceGrid.Grid grid, int intRowCount, int intColCount)
        {
            grid.Redim(intRowCount, intColCount);
            grid.FixedColumns = 1;
            grid.FixedRows = 1;
            for (int r = grid.FixedRows; r < grid.RowsCount; r++)
            {
                grid[r, 0] = new SourceGrid.Cells.RowHeader(r);
                grid[r, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            }
            for (int c = grid.FixedColumns; c < grid.ColumnsCount; c++)
            {
                SourceGrid.Cells.ColumnHeader header = new SourceGrid.Cells.ColumnHeader(GetColCaption(c));
                header.AutomaticSortEnabled = false;
                header.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                grid[0, c] = header;
            }
            grid[0, 0] = new SourceGrid.Cells.Header();

            for (int i = grid.FixedRows; i < intRowCount; i++)
            {
                for (int j = grid.FixedColumns; j < intColCount; j++)
                {
                    grid[i, j] = new SourceGrid.Cells.Cell(string.Empty, typeof(string));
                    grid[i, j].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                }
            }
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.MinimumWidth = 60;
            grid.MinimumHeight = 24;
            grid.AutoSizeCells();
            grid.AutoStretchColumnsToFitWidth = true;
            grid.Columns.StretchToFit();
            grid.AutoStretchRowsToFitHeight = true;
            grid.Rows.StretchToFit();

            grid.Controller.AddController(new KeyDeleteController());

            return grid;
        }

        /// <summary>
        /// 创建单元格边框
        /// </summary>
        /// <param name="grid"></param>
        public static void CreateCellBorderAll(SourceGrid.Grid grid)
        {
            SourceGrid.Cells.Views.Cell cellview;
            SourceGrid.Cells.ICell cell;
            foreach (Position position in grid.Selection.GetSelectionRegion().GetCellsPositions())
            {
                cellview = (SourceGrid.Cells.Views.Cell)grid[position.Row, position.Column].View.Clone();
                cell = grid[position.Row, position.Column];
                cellview.Border = CreateCellBorder(cell);
                cell.View = cellview;
                grid.InvalidateCell(position);
            }
        }

        /// <summary>
        /// 键盘Delete事件
        /// </summary>
        public class KeyDeleteController : SourceGrid.Cells.Controllers.ControllerBase
        {
            public override void OnKeyDown(SourceGrid.CellContext sender, KeyEventArgs e)
            {
                base.OnKeyDown(sender, e);

                if (e.KeyCode == Keys.Delete)
                {
                    SourceGrid.Grid grid = (SourceGrid.Grid)sender.Grid;
                    PositionCollection positionCollection = grid.Selection.GetSelectionRegion().GetCellsPositions();
                    SourceGrid.Cells.ICell cell;
                    foreach (Position position in positionCollection)
                    {
                        if (grid[position.Row, position.Column] != null)
                        {
                            cell = grid[position.Row, position.Column];
                            //cell = new SourceGrid.Cells.Cell(string.Empty, typeof(string));
                            for (int i = 0; i < cell.RowSpan; i++)
                            {
                                for (int j = 0; j < cell.ColumnSpan; j++)
                                {
                                    grid[position.Row + i, position.Column + j] = new SourceGrid.Cells.Cell(string.Empty, typeof(string));
                                }
                            }
                            grid.InvalidateCell(position);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取表格列名称
        /// </summary>
        /// <param name="p_Col"></param>
        /// <returns></returns>
        public static string GetColCaption(int p_Col)
        {
            int l_NumLap = ((p_Col - 1) / 26);
            int l_Remainder = (p_Col) - (l_NumLap * 26);
            string l_tmp = "";
            if (l_NumLap > 0)
                l_tmp += GetColCaption(l_NumLap);

            l_tmp += new string((char)('A' + l_Remainder - 1), 1);
            return l_tmp;
        }

        /// <summary>
        /// 更改文本对齐方式
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="textAlign"></param>
        public static void UpdateTextAlign(Grid grid, DevAge.Drawing.ContentAlignment textAlign)
        {
            RangeRegion rangeRegion = grid.Selection.GetSelectionRegion();
            foreach (SourceGrid.Range range in rangeRegion)
            {
                foreach (Position ps in range.GetCellsPositions())
                {
                    grid[ps.Row, ps.Column].View.TextAlignment = textAlign;
                    grid.InvalidateCell(ps);
                }
            }
        }

        /// <summary>
        /// 更改单元格字体
        /// </summary>
        /// <param name="grid"></param>
        public static void UpdateFont(Grid grid)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowColor = true;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (Position position in grid.Selection.GetSelectionRegion().GetCellsPositions())
                {
                    grid[position.Row, position.Column].View.Font = fontDialog.Font;
                    grid[position.Row, position.Column].View.ForeColor = fontDialog.Color;
                    grid.InvalidateCell(grid[position.Row, position.Column]);
                }
            }
        }

        /// <summary>
        /// 删除单元格
        /// </summary>
        /// <param name="grid"></param>
        public static void DeleteCells(Grid grid)
        {
            RangeRegion rangeRegion = grid.Selection.GetSelectionRegion();
            SourceGrid.Cells.ICell cell;
            foreach (SourceGrid.Range range in rangeRegion)
            {
                foreach (Position position in range.GetCellsPositions())
                {

                    if (grid[position.Row, position.Column] != null)
                    {
                        cell = grid[position.Row, position.Column];
                        //cell = new SourceGrid.Cells.Cell(string.Empty, typeof(string));
                        for (int i = 0; i < cell.RowSpan; i++)
                        {
                            for (int j = 0; j < cell.ColumnSpan; j++)
                            {
                                grid[position.Row + i, position.Column + j] = new SourceGrid.Cells.Cell(string.Empty, typeof(string));
                            }
                        }
                        grid.InvalidateCell(position);
                    }
                }
            }

        }

        /// <summary>
        /// 删除单元格边框
        /// </summary>
        /// <param name="grid"></param>
        public static void DeleteCellsBorder(Grid grid)
        {
            RangeRegion rangeRegion = grid.Selection.GetSelectionRegion();
            //SourceGrid.Cells.ICell cell;
            foreach (SourceGrid.Range range in rangeRegion)
            {
                foreach (Position position in range.GetCellsPositions())
                {
                    if (grid[position.Row, position.Column] != null)
                    {
                        grid[position.Row, position.Column].View.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.LightGray, 1),
                                                                    new DevAge.Drawing.BorderLine(Color.LightGray, 1));
                        grid.InvalidateCell(position);
                    }
                }
            }
        }

        /// <summary>
        /// 建立边框
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static DevAge.Drawing.RectangleBorder CreateCellBorder(SourceGrid.Cells.ICell cell)
        {
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder();
            DevAge.Drawing.RectangleBorder border;
            //左边框
            if (cell.Column.Index > cell.Grid.FixedColumns)
            {
                border = (DevAge.Drawing.RectangleBorder)cell.Grid[cell.Row.Index, cell.Column.Index - 1].View.Border;
                //if (border.Right.Color.IsEmpty&&border.Right.Width==0&&border.Right.DashStyle==System.Drawing.Drawing2D.DashStyle.Solid)
                if (border.Right.Color != Color.Black)
                {
                    cellBorder.Left = DevAge.Drawing.BorderLine.Black1Width;
                }
            }
            else
            {
                cellBorder.Left = DevAge.Drawing.BorderLine.Black1Width;
            }
            //右边框
            if (cell.Grid.ColumnsCount > cell.Column.Index + 1)
            {
                border = (DevAge.Drawing.RectangleBorder)cell.Grid[cell.Row.Index, cell.Column.Index + 1 + cell.ColumnSpan - 1].View.Border;
                //if (border.Left.Color.IsEmpty&&border.Left.Width==0&&border.Left.DashStyle==System.Drawing.Drawing2D.DashStyle.Solid)
                if (border.Left.Color != Color.Black)
                {
                    cellBorder.Right = DevAge.Drawing.BorderLine.Black1Width;
                }
            }
            else
            {
                cellBorder.Right = DevAge.Drawing.BorderLine.Black1Width;
            }
            //上边框
            if (cell.Row.Index > cell.Grid.FixedRows)
            {
                border = (DevAge.Drawing.RectangleBorder)cell.Grid[cell.Row.Index - 1, cell.Column.Index].View.Border;
                //if (border.Bottom.Color.IsEmpty&&border.Bottom.Width==0&&border.Bottom.DashStyle==System.Drawing.Drawing2D.DashStyle.Solid)
                if (border.Bottom.Color != Color.Black)
                {
                    cellBorder.Top = DevAge.Drawing.BorderLine.Black1Width;
                }
            }
            else
            {
                cellBorder.Top = DevAge.Drawing.BorderLine.Black1Width;
            }
            //下边框
            if (cell.Grid.RowsCount > cell.Row.Index + 1)
            {
                border = (DevAge.Drawing.RectangleBorder)cell.Grid[cell.Row.Index + 1 + cell.RowSpan - 1, cell.Column.Index].View.Border;
                if (border.Top.Color != Color.Black)
                {
                    cellBorder.Bottom = DevAge.Drawing.BorderLine.Black1Width;
                }
            }
            else
            {
                cellBorder.Bottom = DevAge.Drawing.BorderLine.Black1Width;
            }

            return cellBorder;
        }
        /// <summary>
        /// 单元格边框样式
        /// </summary>
        public enum CellBorderStyle
        {
            All,//全部
            Top,//顶部
            Left,//左边
            Bottom,//底部
            Right//右边
        }

    }
}
