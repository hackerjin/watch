using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Skyray.Print
{
    /// <summary>
    /// 参数信息
    /// </summary>
    public class Param
    {
        public static bool ChangeDataSourceValue = false;
        /// <summary>
        /// 虚线Pen
        /// </summary>
        public static Pen DashPen = PrintHelper.GetPen(Color.Black, DashStyle.Dash, 1);
        /// <summary>
        /// 边角Pen
        /// </summary>
        public static Pen CornerPen = PrintHelper.GetPen(Color.DarkGray, DashStyle.Solid, 1);
        /// <summary>
        /// 默认字体
        /// </summary>
        public static Font PrintDefaultFont = SystemFonts.DefaultFont;
        /// <summary>
        /// 三角模式
        /// </summary>
        public static bool ThreeRect = false;
        /// <summary>
        /// 图片最小大小
        /// </summary>
        public static Size ImageMinSize = new Size(12, 12);//图片最小宽高

        /// <summary>
        /// 字段最大宽度
        /// </summary>
        public static int FieldMaxWidth = 350;

        /// <summary>
        /// 表格最小宽度
        /// </summary>
        public static int MinTableWidth = 50;
        /// <summary>
        /// 列最大宽度
        /// </summary>
        public static int MaximalColWidth = 400;
        /// <summary>
        /// 列最小宽度
        /// </summary>
        public static int MinimalColWidth = 30;
        /// <summary>
        /// 行最大高度
        /// </summary>
        public static int MaximalRowHeight = 300;
        /// <summary>
        /// 行最小高度
        /// </summary>
        public static int MinimalRowHeight = 20;
        /// <summary>
        /// 模板设计状态，预览字符串值
        /// </summary>
        public static string ValueFormat = "       ";
        /// <summary>
        /// 新分页后，控件顶端间距
        /// </summary>
        public static int TrunPageHeaderSpace = 5;
        /// <summary>
        /// 是否校正控件位置
        /// </summary>
        public static bool AdjustCtrlPos = true;
        ///// <summary>
        ///// 续表画刷
        ///// </summary>
        //public static Pen ContinueTablePen = PrintHelper.GetPen(Color.Black, DashStyle.Solid, 1);
        /// <summary>
        /// 续表字体
        /// </summary>
        public static Font ContinueTableDrawFont = SystemFonts.DefaultFont;
        /// <summary>
        /// 续表颜色
        /// </summary>
        public static Brush ContinueTableDrawBrush = Brushes.DarkRed;
        /// <summary>
        /// TreeNodeInfo 默认编辑状态
        /// </summary>
        public static bool TreeNodeInfoEditState = true;

        public static bool DrawPartInfo = false;

        public const int MaxCellHeight = 409;

        public const int MaxCellWidth = 2000;
    }

    /// <summary>
    /// 表格样式
    /// </summary>
    [Serializable]
    public class TableStyle1 : ITableStyle
    {
        #region ITableStyle 成员

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 表头字体
        /// </summary>
        public Font HeaderFont { get; private set; }


        /// <summary>
        /// 表头字体颜色
        /// </summary>
        public Color HeaderBackColor { get; private set; }

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor { get; private set; }
        /// <summary>
        /// 单元格字体
        /// </summary>
        public Font CellFont { get; private set; }

        /// <summary>
        /// 单元格颜色
        /// </summary>
        public Color CellColor { get; private set; }



        /// <summary>
        /// 表头颜色
        /// </summary>
        public Color HeaderColor { get; private set; }
        ///// <summary>
        ///// 单元格视图
        ///// </summary>
        //public SourceGrid.Cells.Views.Cell CellView { get; private set; }

        ///// <summary>
        ///// 表头视图
        ///// </summary>
        //public SourceGrid.Cells.Views.Cell HeaderView { get; private set; }

        /// <summary>
        /// 选择背景色
        /// </summary>
        public Color SelectBackColor { get; private set; }

        /// <summary>
        /// 选择边框颜色
        /// </summary>
        public Color SelectBorderColor { get; private set; }

        /// <summary>
        /// 选择边框宽度
        /// </summary>
        public int SelectBorderWidth { get; private set; }

        /// <summary>
        /// 选择边框样式
        /// </summary>
        public System.Drawing.Drawing2D.DashStyle SelectBorderStyle { get; private set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        public TableStyle1()
        {

            HeaderFont = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold);  //表头字体
            CellFont = new Font(SystemFonts.DefaultFont.FontFamily, 9);//单元格字体

            CellColor = Color.Black;//单元格颜色
            HeaderColor = Color.Black;//表头颜色
            BorderColor = Color.Black;//边框颜色
            HeaderBackColor = Color.Silver;

            SelectBorderColor = Color.DarkBlue;//选择框颜色
            SelectBackColor = Color.FromArgb(100, 100, 100, 100);//选择背景颜色
            SelectBorderWidth = 0;//选择边框宽度
            SelectBorderStyle = DashStyle.Solid;//选择边框样式
        }

        #endregion
    }

    /// <summary>
    /// 表格样式接口定义类
    /// </summary>
    public interface ITableStyle
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 表头字体
        /// </summary>
        Font HeaderFont { get; }
        /// <summary>
        /// 表头颜色
        /// </summary>
        Color HeaderColor { get; }

        /// <summary>
        /// 边框颜色
        /// </summary>
        Color BorderColor { get; }
        /// <summary>
        /// 表头背景颜色
        /// </summary>
        Color HeaderBackColor { get; }
        /// <summary>
        /// 单元格字体
        /// </summary>
        Font CellFont { get; }
        /// <summary>
        /// 单元格颜色
        /// </summary>
        Color CellColor { get; }

        /// <summary>
        /// 选择框颜色
        /// </summary>
        Color SelectBorderColor { get; }
        /// <summary>
        /// 选择框背景色
        /// </summary>
        Color SelectBackColor { get; }

        /// <summary>
        /// 选择框宽度
        /// </summary>
        int SelectBorderWidth { get; }
        /// <summary>
        /// 选择框样式
        /// </summary>
        System.Drawing.Drawing2D.DashStyle SelectBorderStyle { get; }

        ///// <summary>
        ///// 单元格视图
        ///// </summary>
        //SourceGrid.Cells.Views.Cell CellView { get; }
        ///// <summary>
        ///// 表格视图
        ///// </summary>
        //SourceGrid.Cells.Views.Cell HeaderView { get; }
    }
    /// <summary>
    /// 纸张大小定义
    /// </summary>
    public enum PaperSize
    {
        //修改：何晓明 2011-02-12
        //原因：增加纸张类型
        A3,
        /// <summary>
        /// A4纸张大小
        /// </summary>
        A4,
        A5,
        A6,
        B4,
        B5,
        B6
    }
    /// <summary>
    /// 打印方向
    /// </summary>
    public enum PrintDirection
    {
        /// <summary>
        /// 水平（竖向）
        /// </summary>
        Horizontal,
        /// <summary>
        /// 垂直（横向）
        /// </summary>
        Vertical
    }
}