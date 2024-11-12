using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.EDX.Common
{
    public delegate void CheckBoxClickedHandler(datagridviewCheckboxHeaderEventArgs e);

    public class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        Point checkBoxLocation;
        Size checkBoxSize;
        bool _checked = false;

        public bool Checked
        {
            set
            {
                if (!value)
                {
                    _checked = value;
                    this.DataGridView.InvalidateCell(this);
                }
            }
        }
        Point _cellLocation = new Point();
        System.Windows.Forms.VisualStyles.CheckBoxState _cbState =
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;



        public event CheckBoxClickedHandler OnCheckBoxClicked;
        //public event CheckBoxClickedHandler OnCancelClicked;

        public DatagridViewCheckBoxHeaderCell()
        {
        }

        protected override void Paint(System.Drawing.Graphics graphics,
            System.Drawing.Rectangle clipBounds,
            System.Drawing.Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                dataGridViewElementState, value,
                formattedValue, errorText, cellStyle,
                advancedBorderStyle, paintParts);
            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(graphics,
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            p.X = cellBounds.Location.X +
                (cellBounds.Width / 2) - (s.Width / 2);
            p.Y = cellBounds.Location.Y +
                (cellBounds.Height / 2) - (s.Height / 2);
            _cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;
            if (_checked)
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.CheckedNormal;
            else
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.UncheckedNormal;
            CheckBoxRenderer.DrawCheckBox
            (graphics, checkBoxLocation, _cbState);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
            if (p.X >= checkBoxLocation.X && p.X <=
                checkBoxLocation.X + checkBoxSize.Width
            && p.Y >= checkBoxLocation.Y && p.Y <=
                checkBoxLocation.Y + checkBoxSize.Height)
            {
                _checked = !_checked;
                if (OnCheckBoxClicked != null)
                {
                    if (this.OnCheckBoxClicked != null)
                    {
                        this.OnCheckBoxClicked.Invoke(new datagridviewCheckboxHeaderEventArgs(_checked));
                    }

                    // OnCheckBoxClicked(new DataGridViewCheckBoxHeaderCellEventArgs(_checked));
                    this.DataGridView.InvalidateCell(this);
                }

            }
            base.OnMouseClick(e);
        }
    }
}
