using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Skyray.Controls
{
    public class DataGridViewCheckListBoxWEditingControl : CheckedListBoxW, IDataGridViewEditingControl
    {
        #region IDataGridViewEditingControl 成员

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {

        }

        public DataGridView EditingControlDataGridView { get; set; }


        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value.ToString();
            }
        }
        public int EditingControlRowIndex { get; set; }
        public bool EditingControlValueChanged { get; set; }


        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (keyData & Keys.KeyCode)
            {
                //case Keys.Enter:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                    //case Keys.Home:
                    //case Keys.End:
                    //case Keys.PageDown:
                    //case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {

        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        #endregion

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            EditingControlValueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnItemCheck(ice);
        }
    }
}
