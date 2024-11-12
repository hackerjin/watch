using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Controls
{
    public class DataGridViewCheckListBoxWColumn : DataGridViewColumn
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public DataGridViewCheckListBoxWColumn()
            : base(new DataGridViewCheckListBoxWCell())
        {
        }

        /// <summary>
        /// 获取或设置用于创建新单元格的模板。
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CheckListBoxW.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewCheckListBoxWCell)))
                {
                    throw new InvalidCastException("Must be a CheckedListBoxWCell");
                }
                base.CellTemplate = value;
            }
        }
        public string[] DataSource { get; set; }
    }
}
