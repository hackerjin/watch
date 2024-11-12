using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    [Serializable]
    public class MenuConfig
    {
        /// <summary>
        /// 显示项的名称
        /// </summary>
        public string ItemName { set; get; }

        /// <summary>
        /// 是否显示在主界面中
        /// </summary>
        public bool ShowInMain { set; get; }

        /// <summary>
        /// 是否显示在工具栏
        /// </summary>
        public bool ShowInTools { set; get; }

        /// <summary>
        ///显示项的类型
        /// </summary>
        public int FlagType { set; get; }

        /// <summary>
        /// 改项是否显示
        /// </summary>
        public bool Enable { set; get; }

        /// <summary>
        /// 显示项的文本
        /// </summary>
        public string ItemText { set; get; }

        //修改： 何晓明 20110713 增加快捷键
        /// <summary>
        /// 快捷键
        /// </summary>
        public string ShortcutKeys { set; get; }
        //

        public bool EnableControl { get; set; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public MenuConfig()
        { }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="showInMain"></param>
        /// <param name="showInTools"></param>
        /// <param name="flagType"></param>
        /// <param name="enable"></param>
        /// <param name="itemText"></param>
        public MenuConfig(string itemName, bool showInMain, bool showInTools, 
            int flagType, bool enable, string itemText)
        {
            this.ItemName = itemName;
            this.ShowInMain = showInMain;
            this.ShowInTools = showInTools;
            this.FlagType = flagType;
            this.Enable = enable;
            this.ItemText = itemText;
        }

    }
}
