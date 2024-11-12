using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    [Serializable]
    public class SaveSerialize
    {
        public ContainObjectInformation[] UserDataInfo { get; set; }

        public List<TabPageInforControl> tabPageConType { get; set; }

        /// <summary>
        /// 记录配置工具中生成的Datagridviw的名称
        /// </summary>
        public List<string> DataGridViewWName { set; get; }

        public List<SplitSetingObject> listSplit { set; get; }

        public int Width { set; get; }

        public int Height { set; get; }

        public SaveSerialize()
        {
            //listSplit = new List<SplitSetingObject>();
            //DataGridViewWName = new 
        }
    }

    [Serializable]
    public class TabPageInforControl
    {
        /// <summary>
        /// tabPage的文本内容
        /// </summary>
        public string TabPageText { set; get; }

        /// <summary>
        /// tabpage嵌入的类型
        /// </summary>
        public string TabPageContainerType { set; get; }

        /// <summary>
        /// tabPage的名称
        /// </summary>
        public string TabPageName { set; get; }

        /// <summary>
        /// 嵌入对象的名称
        /// </summary>
        public string EmbededObjectName { set; get; }

        /// <summary>
        /// 嵌入对象的位置
        /// </summary>
        public int EmbededObjectPosition { set; get; }

        /// <summary>
        /// 嵌入对象的方向
        /// </summary>
        public int EmbededOrient { set; get; }


        public string parentName { get; set; }

        public bool IsRecurve { get; set; }

        /// <summary>
        /// 0为skyray控件，1为用户控件，2为一般控件
        /// </summary>
        public int IsUserControl { set; get; }

        public int PartitionNumber { get; set; }

        //public PartitionStyle Style { get; set; }

        public int OrigianalNumber { get; set; }

        public Dictionary<string, TabPlanningOrient> controlType { get; set; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public TabPageInforControl()
        {
            IsUserControl = 2;
            controlType = new Dictionary<string, TabPlanningOrient>();
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="tabPageText"></param>
        /// <param name="tabPageContainerType"></param>
        /// <param name="tabPageName"></param>
        /// <param name="embededName"></param>
        public TabPageInforControl(string tabPageText, string tabPageContainerType, string tabPageName, string embededName)
        {
            this.TabPageText = tabPageText;
            this.TabPageContainerType = tabPageContainerType;
            this.TabPageName = tabPageName;
            this.EmbededObjectName = embededName;
        }
    }

    [Serializable]
    public class TabPlanningOrient
    {
        public string ControlTypeName { get; set; }
        public int IsUserControl { get; set; }
        public Dictionary<string, TabPageInforControl> RecurseTabControl { get; set; }
        public TabPlanningOrient(string controlTypeName, int isUserControl)
        {
            this.ControlTypeName = controlTypeName;
            this.IsUserControl = isUserControl;
            RecurseTabControl = new Dictionary<string, TabPageInforControl>();
        }
        public TabPlanningOrient()
        { }
    }

    /// <summary>
    /// 数据容器的对象的携带的信息
    /// </summary>
    public class EmbededObject
    {
        /// <summary>
        /// 容器的位置
        /// </summary>
        public int ObjectPosition { set; get; }

        /// <summary>
        /// 容器的方向
        /// </summary>
        public int ObjectOrient { set; get; }


        public string IdentifierName { get; set; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public EmbededObject()
        { }
    }
}
