//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SourceGrid;
//using SourceGrid.Cells;
//using System.Drawing;
//using System.Data;


//namespace Skyray.EDX
//{
//    public class GridBinder
//    {
//        public static void AddColHeader(Grid grid, params string[] strCols)
//        {
//            int intColCount = strCols.Length;//列总数

//            grid.RowsCount = 1;//设置
//            grid.ColumnsCount = intColCount;

//            grid.FixedColumns = 1;
//            grid.FixedRows = 1;
//            grid.AutoStretchColumnsToFitWidth = true;

//            for (int c = 0; c < intColCount; c++)
//            {
//                SourceGrid.Cells.ColumnHeader colHeader = new ColumnHeader(strCols[c]);//构造表头
//                if (grid.Name == "gridSet" || "gridMain" == grid.Name)
//                {
//                    colHeader.ResizeEnabled = false;
//                }
//                colHeader.AutomaticSortEnabled = false;
//                colHeader.Editor = null;
//                grid[0, c] = colHeader;
//                grid.Columns[c].AutoSizeMode = AutoSizeMode.MinimumSize;
//            }

//            //grid.AutoSizeCells();
//            grid.Columns.StretchToFit();
            

//        }

//        public static void DataRowToGrid(Grid grid, DataTable dtbl)
//        {
//            grid.ColumnsCount = dtbl.Columns.Count;

//            for (int r = 0; r < dtbl.Rows.Count; r++)//赋值
//            {
//                for (int c = 0; c < grid.ColumnsCount; c++)
//                {
//                    SetCell(grid, r + 1, c, dtbl.Rows[r][c]);
//                }
//            }
//            grid.RowsCount = dtbl.Rows.Count + 1;
//        }
//        /// <summary>
//        /// 设置单元格的值
//        /// </summary>
//        /// <param name="grid"></param>
//        /// <param name="r"></param>
//        /// <param name="c"></param>
//        /// <param name="objValue"></param>
//        public static void SetCell(Grid grid, int r, int c, object objValue)
//        {
//            if (grid.RowsCount < r + 1)
//            {
//                grid.RowsCount = r + 1;
//            }
//            if (grid[r, c] == null)
//            {
//                grid[r, c] = new SourceGrid.Cells.Cell(objValue);//单元格不存在构建单元格
//                grid[r, c].View = GetTransparentCellView();
//            }
//            else
//            {
//                grid[r, c].Value = objValue;
//            }
//        }

//        /// <summary>
//        /// 创建透明单元格视图
//        /// </summary>
//        public static SourceGrid.Cells.Views.Cell GetTransparentCellView()
//        {
//            SourceGrid.Cells.Views.Cell cellView = new SourceGrid.Cells.Views.Cell();
//            cellView.BackColor = Color.Transparent;
//            cellView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
//            return cellView;
//        }
//    }
//}
