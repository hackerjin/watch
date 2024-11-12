using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.Controls;
using System.ComponentModel;


namespace Skyray.EDX.Common
{
    public class CtrlHelper
    {
        public static void EnumToComboBox(ComboBoxW comboBox, Type enumType)
        {
            if (enumType.BaseType == typeof(System.Enum))
            {
                foreach (var str in Enum.GetNames(enumType))
                {
                    comboBox.Items.Add(str);
                }
                if (comboBox.Items.Count > 0) comboBox.SelectedIndex = 0;
            }
        }

        public static void EnumDescToComboBox(ComboBoxW comboBox, Type enumType)
        {
            if (enumType.BaseType == typeof(System.Enum))
            {

                foreach (var str in Enum.GetNames(enumType))
                {
                    object[] objs = enumType.GetField(str).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (objs != null && objs.Length > 0)
                    {
                        DescriptionAttribute attr = objs[0] as DescriptionAttribute;
                        comboBox.Items.Add(attr.Description);
                    }
                }
                if (comboBox.Items.Count > 0) comboBox.SelectedIndex = 0;
            }
        }
    }
}
