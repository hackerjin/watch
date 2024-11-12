using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Controls
{
    public class CheckedListBoxW : CheckedListBox
    {
        public string Value
        {
            get
            {
                return GetCheckedItemsString();
            }
            set
            {
                SetCheckedItems(value);
            }
        }
        

        private void SetCheckedItems(string values)
        {
            string[] strs = values.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);//拆分
            for (int j = 0; j < Items.Count; j++)
                SetItemChecked(j, strs.Contains(Items[j].ToString()));//设置Checked属性
        }

        private string GetCheckedItemsString()
        {
            List<string> lst = new List<string>();
            foreach (var item in base.CheckedItems)
            {
                lst.Add(item.ToString());
            }
            return string.Join(",", lst.ToArray());
        }
    }
}
