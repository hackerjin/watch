using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.Controls.Tree;
using System.ComponentModel;
using System.Drawing;
using System.Data;

namespace Skyray.Print
{
    /// <summary>
    /// 节点对应信息（简化设计将字段，表格，图片信息存储于本类）
    /// </summary>
    [Serializable]
    public class TreeNodeInfo
    {
        #region 节点属性

        public bool SaveValue { get; set; }
        /// <summary>
        /// 字段文本与值间距
        /// </summary>
        public int TextValueSpace { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public CtrlType Type { get; set; }


        public bool Parent { get; set; }

        public CompositeContextType contextType { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示标题
        /// </summary>
        private string _Caption = string.Empty;
        public string Caption
        {
            get
            {
                return _Caption == string.Empty ? Text : _Caption;
            }
            set { _Caption = value; }
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 文本值
        /// </summary>
        public string TextValue { get; set; }

        /// <summary>
        /// 文本字体
        /// </summary>
        public Font TextFont { get; set; }


        public List<RangeColumns> RangeColumns { get; set; }

        /// <summary>
        /// 文本值字体
        /// </summary>
        public Font TextValueFont { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// 文本值颜色
        /// </summary>
        public Color TextValueColor { get; set; }

        /// <summary>
        /// 节点关联的图片
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// 节点关联的表格
        /// </summary>
        public DataTable Table { get; set; }


        public List<DataTable> Tables { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<object> ObjSource { get; set; }
        /// <summary>
        /// 所在位置
        /// </summary>
        public Point Location { get; set; }


        /// <summary>
        /// 控件大小
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// 表格行设置信息
        /// </summary>
        public ColInfo[] ColSetInfos { get; set; }

        /// <summary>
        /// 文本垂直间距
        /// </summary>
        public int TextVSpace { get; set; }

        /// <summary>
        /// 是否显示图片边框
        /// </summary>
        public bool ShowPicBorder { get; set; }

        /// <summary>
        /// 图片边框颜色
        /// </summary>
        public Color PicBorderColor { get; set; }

        /// <summary>
        /// 表格样式
        /// </summary>
        public Type TableStyleType { get; set; }

        /// <summary>
        /// 表头高度
        /// </summary>
        public int ColHeight { get; set; }

        /// <summary>
        /// 行高度
        /// </summary>
        public int RowHeight { get; set; }

        /// <summary>
        /// 表格自动宽高
        /// </summary>
        public bool TableAutoSize { get; set; }

        public List<CellSize> CellContext { get; set; }

        /// <summary>
        /// 分组号
        /// </summary>
        public int GroupNumber { get; set; }

        public Type CompositeType { get; set; }


        #endregion

        #region 节点选项

        /// <summary>
        /// 是否显示名称
        /// </summary>
        public bool ShowName { get; set; }

        /// <summary>
        /// 是否启用文本编辑
        /// </summary>
        public bool EnableTextEdit { get; set; }

        /// <summary>
        /// 是否启用文本字体编辑
        /// </summary>
        public bool EnableTextFontEdit { get; set; }

        /// <summary>
        /// 是否启用文本颜色编辑
        /// </summary>
        public bool EnableTextColorEdit { get; set; }

        /// <summary>
        /// 是否启用文本编辑
        /// </summary>
        public bool EnableTextValueEdit { get; set; }

        /// <summary>
        /// 是否启用文本字体编辑
        /// </summary>
        public bool EnableTextValueFontEdit { get; set; }

        /// <summary>
        /// 是否启用文本颜色编辑
        /// </summary>
        public bool EnableTextValueColorEdit { get; set; }



        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node"></param>
        public TreeNodeInfo()
        {
            //TextColor = TextValueColor = -16777216;//默认黑色
            TextColor = TextValueColor = Color.Black;//默认黑色
            TextFont = TextValueFont = Param.PrintDefaultFont;//默认字体
            EnableTextEdit = EnableTextFontEdit
                          = EnableTextColorEdit = EnableTextValueEdit
                          = EnableTextValueFontEdit = EnableTextValueColorEdit
                          = Param.TreeNodeInfoEditState;
            ShowPicBorder = true;
        }
    }

    /// <summary>
    /// 区域列类
    /// </summary>
    public class RangeColumns
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnsName {get;set;}
        /// <summary>
        /// 最小值
        /// </summary>
        public double minValue {get;set;}
        /// <summary>
        /// 最大值
        /// </summary>
        public double maxValue {get;set;}
    }
}
