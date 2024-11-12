using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.Controls
{
    public class DataGridViewCheckListBoxWCell : DataGridViewTextBoxCell
    {

        public DataGridViewCheckListBoxWCell()
            : base()
        {
            //this.Style.Format = "d";//格式设置           
        }
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            var ctl = DataGridView.EditingControl as DataGridViewCheckListBoxWEditingControl;
            ctl.DataSource = ((DataGridViewCheckListBoxWColumn)base.OwningColumn).DataSource;
            
            ctl.Value = this.Value.ToString();
        }

        /// <summary>
        /// 编辑类型
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewCheckListBoxWEditingControl);
            }
        }

        /// <summary>
        /// 值类型
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return string.Empty;
            }
        }             

    }

}
