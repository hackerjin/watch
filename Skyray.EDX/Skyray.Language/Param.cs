using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Skyray.Language
{
    public class ColLength
    {
        public const int FullName = 100;
        public const int ShortName = 100;
        public const int LanguageDataValue = 100;
        public const int LanguageDataKey = 100;
    }

    public class Param
    {
#if DEBUG
        public static bool SaveTextToDB = true;
#else
        public static bool SaveTextToDB = false;
#endif
       // public static bool SaveTextToDB = true;
        /// <summary>
        /// 反射字段类型
        /// </summary>
        public static BindingFlags RefFlag = BindingFlags.Instance
                                            | BindingFlags.DeclaredOnly
                                            | BindingFlags.NonPublic;

        public static string[] HeaderTextProperty = new string[] { "HeaderTextProperty" };

        public static string[] TextProperty = new string[] { "Text" };
        /// <summary>
        /// 需反射属性
        /// </summary> 
        public static string[] PropertyNames =
            new string[]{ 
                "Text",
                "ToolTip", 
                "ToolTipText", 
                "ToolTipTitle",
                "HeaderText", 
                "GroupTitle",
            "Value"};
        /// <summary>
        /// 语言切换时排除的类型集合
        /// </summary>
        /// <returns></returns>
        public static string[] ExcludeTypes = new string[]
        { 
            "System.Windows.Forms.TextBox",
             "System.ComponentModel.IContainer",
            "System.Windows.Forms.MenuStrip",             
            //"System.Windows.Forms.TabControl",
             "Skyray.Controls.TextBoxW",
            "Skyray.Controls.MenuStripW",
            //"Skyray.Controls.TabControlW",
            "Skyray.Controls.ListBoxW",
              "Skyray.Controls.ComboBoxW",
              "Skyray.Controls.NumricUpDownW",  
              "Skyray.Print.TreeViewAdvEx",
              "Skyray.Controls.FontPicker",
              "System.Windows.Forms.ComboBox",
              "Skyray.Controls.ColorPicker",
              "Skyray.Controls.DashStylePicker",
              
              "Skyray.Controls.ToolStripW",
              "System.Windows.Forms.DateTimePicker",
              "Skyray.Controls.MenuStripW",
              "Skyray.Controls.TabPage"
       };
    }
}
