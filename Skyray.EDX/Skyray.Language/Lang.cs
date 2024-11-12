using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Language
{
    /// <summary>
    /// 多语言实例对象
    /// </summary>
    public class Lang
    {
        /// <summary>
        /// 记录当前语言信息
        /// </summary>
        public static LanguageModel Model;

        public static ToolStripMenuItem LangItem;

        public static Languages GetDefaultLang()
        {
            return Languages.New.Init("中文", "CN", true, true);
        }
    }
}
