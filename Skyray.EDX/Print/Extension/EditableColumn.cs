using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.Print
{
    /// <summary>
    /// 打印模板表头
    /// </summary>
    public class PrintHeader : SourceGrid.Cells.ColumnHeader
    {
        /// <summary>
        /// 控制器
        /// </summary>
        private static PrintHeaderControler printHeaderControler = new PrintHeaderControler();
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static PrintHeader() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="caption"></param>
        public PrintHeader(string caption)
            : base(caption)
        {
            //设置Eidtor
            SourceGrid.Cells.Editors.TextBox headerEditor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            headerEditor.EditableMode = SourceGrid.EditableMode.None;//编辑模式
            Editor = headerEditor;//Editor赋值

            //添加控制器
            AddController(printHeaderControler);

            //移除不可选择控制器
            RemoveController(SourceGrid.Cells.Controllers.Unselectable.Default);
        }
    }
    /// <summary>
    /// 表头控制器
    /// </summary>
    public class PrintHeaderControler : SourceGrid.Cells.Controllers.ControllerBase
    {
        /// <summary>
        ///单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
            (sender.Grid.Tag as PrintTable).ClearSelection();//清除选择
        }
        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnDoubleClick(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnDoubleClick(sender, e);
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
            var colInfo = col.Tag as ColInfo;
            if (sender.DisplayText != colInfo.Caption)
            {
                colInfo.Caption = sender.DisplayText;
                colInfo.NeedReLayout = true;
                col.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
                col.Grid.AutoSizeCells(col.Range);
            }
        }
    }
    /// <summary>
    /// 打印模板表格
    /// </summary>
    public class PrintCell : SourceGrid.Cells.Cell
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static PrintCell()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="caption"></param>
        //public PrintCell(string caption)
        //    : base(caption)
        //{
        //    //Editor = null;
        //    //AddController(SourceGrid.Cells.Controllers.Unselectable.Default);//添加控制器
        //}

        public PrintCell(string caption)
            :base(caption)
        {
            SourceGrid.Cells.Views.Cell view = new SourceGrid.Cells.Views.Cell();
            view.Font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
            //view.Font = S
            view.BackColor = Color.White;
            view.WordWrap = true;
            //view.ForeColor = red;
            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            View = view;
        }
    }

}
