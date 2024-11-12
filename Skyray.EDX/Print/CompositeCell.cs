using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceGrid;
using SourceGrid.Cells;
using System.Drawing;
using DevAge.Drawing;
using SourceGrid.Selection;

namespace Skyray.Print
{
    public class CompositeCell:SourceGrid.Cells.Cell
    {

        /// <summary>
        /// 控制器
        /// </summary>
        private static CellControler cellControler = new CellControler();
     /// <summary>
		/// Constructor
		/// </summary>
        public CompositeCell():this(null)
		{
		}

        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cellValue"></param>
        public CompositeCell(object cellValue)
            : base(cellValue)
		{
			View = SourceGrid.Cells.Views.ColumnHeader.Default;
            AddController(SourceGrid.Cells.Controllers.MouseInvalidate.Default);
			ResizeEnabled = true;
            SourceGrid.Cells.Editors.TextBox headerEditor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            headerEditor.EditableMode = SourceGrid.EditableMode.None;//编辑模式
            Editor = headerEditor;//Editor赋值
            //添加控制器
            AddController(cellControler);
		}

        /// <summary>
        /// Gets or sets if enable the resize of the width of the column. This property internally use the Controllers.Resizable.ResizeWidth.
        /// </summary>
        public bool ResizeEnabled
        {
            get { return (FindController(typeof(SourceGrid.Cells.Controllers.Resizable)) == SourceGrid.Cells.Controllers.Resizable.ResizeBoth); }
            set
            {
                if (value == ResizeEnabled)
                    return;

                if (value)
                    AddController(SourceGrid.Cells.Controllers.Resizable.ResizeBoth);
                else
                    RemoveController(SourceGrid.Cells.Controllers.Resizable.ResizeBoth);
            }
        }
    }

    public class CellControler : SourceGrid.Cells.Controllers.ControllerBase
    {

        /// <summary>
        ///单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }
        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnDoubleClick(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnDoubleClick(sender, e);
            (sender.Grid.Tag as PrintCompositeTable).ClearSelection();//清除选择
            sender.StartEdit();//开始编辑
        }
        /// <summary>
        /// 编辑结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnEditEnded(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnEditEnded(sender, e);
            int colIndex = sender.Position.Column;
            //var cols = sender.Grid.Columns as SourceGrid.GridColumns;
            var cols = sender.Grid.Columns as SourceGrid.Grid.GridColumns;
            var col = cols[colIndex];
            sender.Grid.GetCell(sender.Position).View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            var colInfo = col.Tag as CellSize;
            RowInfo info = colInfo.RowsInfo[sender.Position.Row];
            info.CellText = sender.DisplayText;
        }
    }

    [Serializable]
    public class CellSize
    {
        /// <summary>
        /// 列信息
        /// </summary>
        public ColInfo ColumnInfo { get; set; }


        public List<RowInfo> RowsInfo { get; set; }
    }

    [Serializable]
    public class RowInfo
    {
        /// <summary>
        /// 行高度
        /// </summary>
        public int RowHeight { get; set; }

        /// <summary>
        /// 行名称
        /// </summary>
        public int RowName { get; set; }

        public bool NeedReLayout { get; set; }

        public string CellText { get; set; }

        public bool IsMerge { get; set; }

        public int RowSpan { get; set; }

        public int ColumnSpanm { get; set; }

        public string AttrubuteName { get; set; }

        public string Path { get; set; }

        public bool IsEnumerable { get; set; }
    }
}
