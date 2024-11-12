using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Print
{
    /// <summary>
    /// 命令定义
    /// </summary>
    internal enum Cmd
    {
        /// <summary>
        /// 左对齐
        /// </summary>
        AlignLeft,
        /// <summary>
        /// 右对齐
        /// </summary>
        AlignRight,
        /// <summary>
        /// 顶端对齐
        /// </summary>
        AlignTop,
        /// <summary>
        /// 底部对齐
        /// </summary>
        AlignBottom,
        /// <summary>
        /// 标签对齐
        /// </summary>
        AlignLabel,
        /// <summary>
        /// 相同大小
        /// </summary>
        SameSize,
        /// <summary>
        /// 相同宽度
        /// </summary>
        SameWidth,
        /// <summary>
        /// 相同高度
        /// </summary>
        SameHeight,
        /// <summary>
        /// 移除垂直间距
        /// </summary>
        RemoveVSpace,
        /// <summary>
        /// 移除水平间距
        /// </summary>
        RemoveHSpace
    }
    /// <summary>
    /// 鼠标样式定义
    /// </summary>
    internal enum RESIZE_BORDER
    {
        /// <summary>
        /// 无
        /// </summary>
        RB_NONE = 0,
        /// <summary>
        /// 顶端
        /// </summary>
        RB_TOP = 1,
        /// <summary>
        /// 右侧
        /// </summary>
        RB_RIGHT = 2,
        /// <summary>
        /// 底部
        /// </summary>
        RB_BOTTOM = 3,
        /// <summary>
        /// 左侧
        /// </summary>
        RB_LEFT = 4,
        /// <summary>
        /// 左上
        /// </summary>
        RB_TOPLEFT = 5,
        /// <summary>
        /// 右上
        /// </summary>
        RB_TOPRIGHT = 6,
        /// <summary>
        /// 左下
        /// </summary>
        RB_BOTTOMLEFT = 7,
        /// <summary>
        /// 右下
        /// </summary>
        RB_BOTTOMRIGHT = 8,
        /// <summary>
        /// 全方向
        /// </summary>
        RB_SIZEALL = 9
    }

    /// <summary>
    /// 控件类型
    /// </summary>
    public enum CtrlType
    {
        /// <summary>
        /// 空值
        /// </summary>
        None,
        /// <summary>
        /// 标签
        /// </summary>
        Label,
        /// <summary>
        /// 字段
        /// </summary>
        Field,
        /// <summary>
        /// 名称
        /// </summary>
        Image,
        /// <summary>
        /// 表格
        /// </summary>
        Grid,
        /// <summary>
        /// 线条
        /// </summary>
        Line,
        /// <summary>
        /// 表头
        /// </summary>
        Header,
        /// <summary>
        /// 页尾
        /// </summary>
        Footer,
        /// <summary>
        /// Body
        /// </summary>
        Body,
        /// <summary>
        /// 页面
        /// </summary>
        Page,

        ComposeTable
    }

    public enum CompositeContextType
    {
        Label,
        Entity,
        Original
    }

    /// <summary>
    /// 表格边框样式
    /// </summary>
    internal enum BorderType
    {
        //None,
        /// <summary>
        /// 样式1
        /// </summary>
        Style1,
        //Style2
    }
    /// <summary>
    /// 拆分类型
    /// </summary>
    public enum SplitTableType
    {
        /// <summary>
        /// 首页面
        /// </summary>
        First,//首页面
        /// <summary>
        /// 中间页面
        /// </summary>
        Center,//中间页面
        /// <summary>
        /// 尾页
        /// </summary>
        End//尾页
    }

    /// <summary>
    /// 设计页面大小
    /// </summary>
    public enum PageSize
    {
        /// <summary>
        /// 最小
        /// </summary>
        Smallest,
        /// <summary>
        /// 较小
        /// </summary>
        Smaller,
        /// <summary>
        /// 中等
        /// </summary>
        Middle,
        /// <summary>
        /// 较大
        /// </summary>
        Larger,
        /// <summary>
        /// 最大
        /// </summary>
        Largest
    }
    /// <summary>
    /// 打开模式
    /// </summary>
    public enum OpenTemplateMode
    {
        /// <summary>
        /// 仅打开
        /// </summary>
        Open,
        /// <summary>
        /// 打开并预览
        /// </summary>
        OpenAndPreview,
        /// <summary>
        /// 打开并打印
        /// </summary>
        OpenAndPrint
    }
}
