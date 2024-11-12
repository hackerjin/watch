using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Print
{
    /// <summary>
    /// 页面数据源存取类
    /// </summary>
    [Serializable]
    public class TemplateSource
    {
        /// <summary>
        /// Pnls
        /// </summary>
        public List<Panels> PrintPanels { get; set; }
        /// <summary>
        /// 打印边距
        /// </summary>
        public Padding PageMargin { get; set; }
        /// <summary>
        /// 纸张大小
        /// </summary>
        public PaperSize PaperSize { get; set; }

        //修改：何晓明 2011-02-22
        //原因：增加打印机及打印纸张
        /// <summary>
        /// 真实纸张大小跟打印机关联
        /// </summary>
        public string strPaperSize { get; set; }
        //
        /// <summary>
        /// 是否显示页尾
        /// </summary>
        public bool ShowFooter { get; set; }
        /// <summary>
        /// 是否显示表头
        /// </summary>
        public bool ShowHeader { get; set; }
        /// <summary>
        /// 页面大小
        /// </summary>
        public PageSize PageSize { get; set; }
        /// <summary>
        /// 打印方向
        /// </summary>
        public PrintDirection Dir { get; set; }

        public int HeadSplitterPosition { get; set; }

        public int BottomSplitterPosition { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public TemplateSource()
        {
            PrintPanels = new List<Panels>();
        }
    }

    /// <summary>
    /// printpanel集合类
    /// </summary>
    [Serializable]
    public class Panels
    {
        /// <summary>
        /// Pnl名称
        /// </summary>
        public CtrlType PanelType { get; set; }

        /// <summary>
        /// 节点集合
        /// </summary>
        public List<TreeNodeInfo> NodeInfos { get; set; }
    }
}
