using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using Skyray.API;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace Skyray.Controls
{
    public class ComboBoxW : ComboBox, ISkyrayStyle
    {

        #region Fields

        private bool _AutoComplete;
        private bool _AutoDropdown;
        private Color _BackColorEven = Color.White;
        private Color _BackColorOdd = Color.White;
        private string _ColumnNameString = "";
        private int _ColumnWidthDefault = 75;
        private string _ColumnWidthString = "";
        private int _LinkedColumnIndex;
        private TextBox _LinkedTextBox;
        private int _TotalWidth = 0;
        private int _ValueMemberColumnIndex = 0;
        private ControlState _buttonState;
        private Collection<string> _ColumnNames = new Collection<string>();
        private Collection<int> _ColumnWidths = new Collection<int>();
        private Color _baseColor = Color.FromArgb(175, 210, 255);
        private Color _borderColor = Color.FromArgb(121, 153, 194);
        private Color _arrowColor = Color.FromArgb(19, 88, 128);
        private IntPtr _editHandle= IntPtr.Zero;
        private bool _bPainting;
        private bool _bShowColumnName;
        //public event System.EventHandler OpenSearchForm;
        //private bool _EscEnabled = false;

        #endregion

        #region Properties

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [DefaultValue(typeof(bool), "false")]
        public bool BShowColumnName
        {
            get { return _bShowColumnName; }
            set { _bShowColumnName = value; }
        }

        [DefaultValue(typeof(Color), "51, 161, 224")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                if (_baseColor != value)
                {
                    _baseColor = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "51, 161, 224")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "19, 88, 128")]
        public Color ArrowColor
        {
            get { return _arrowColor; }
            set
            {
                if (_arrowColor != value)
                {
                    _arrowColor = value;
                    base.Invalidate();
                }
            }
        }

        internal bool ButtonPressed
        {
            get
            {
                if (IsHandleCreated)
                {
                    return GetComboBoxButtonPressed();
                }
                return false;
            }
        }

        internal ControlState ButtonState
        {
            get { return _buttonState; }
            set
            {
                if (_buttonState != value)
                {
                    _buttonState = value;
                    Invalidate(ButtonRect);
                }
            }
        }

        internal Rectangle ButtonRect
        {
            get
            {
                return GetDropDownButtonRect();
            }
        }

        internal IntPtr EditHandle
        {
            get { return _editHandle; }
        }

        internal Rectangle EditRect
        {
            get
            {
                if (DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    Rectangle rect = new Rectangle(
                        3, 3, Width - ButtonRect.Width - 6, Height - 6);
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        rect.X += ButtonRect.Right;
                    }
                    return rect;
                }
                if (IsHandleCreated && EditHandle != IntPtr.Zero)
                {
                    RECT rcClient = new RECT();
                    WinMethod.GetWindowRect(EditHandle, out rcClient);
                    return RectangleToClient(rcClient.Rect);
                }
                return Rectangle.Empty;
            }
        }

        public bool AutoComplete
        {
            get
            {
                return _AutoComplete;
            }
            set
            {
                _AutoComplete = value;
            }
        }

        public bool AutoDropdown
        {
            get
            {
                return _AutoDropdown;
            }
            set
            {
                _AutoDropdown = value;
            }
        }

        public Color BackColorEven
        {
            get
            {
                return _BackColorEven;
            }
            set
            {
                _BackColorEven = value;
            }
        }

        public Color BackColorOdd
        {
            get
            {
                return _BackColorOdd;
            }
            set
            {
                _BackColorOdd = value;
            }
        }

        public Collection<string> ColumnNameCollection
        {
            get
            {
                return _ColumnNames;
            }
        }

        public string ColumnNames
        {
            get
            {
                return _ColumnNameString;
            }

            set
            {
                // If the column string is blank, leave it blank.
                // The default width will be used for all columns.
                if (!Convert.ToBoolean(value.Trim().Length))
                {
                    _ColumnNameString = "";
                }
                else if (value != null)
                {
                    char[] delimiterChars = { ',', ';', ':' };
                    string[] columnNames = value.Split(delimiterChars);

                    if (!DesignMode)
                    {
                        _ColumnNames.Clear();
                    }

                    // After splitting the string into an array, iterate
                    // through the strings and check that they're all valid.
                    foreach (string s in columnNames)
                    {
                        // Does it have length?
                        if (Convert.ToBoolean(s.Trim().Length))
                        {
                            if (!DesignMode)
                            {
                                _ColumnNames.Add(s.Trim());
                            }
                        }
                        else // The value is blank
                        {
                            throw new NotSupportedException("Column names can not be blank.");
                        }
                    }
                    _ColumnNameString = value;
                }
            }
        }

        public Collection<int> ColumnWidthCollection
        {
            get
            {
                return _ColumnWidths;
            }
        }

        public int ColumnWidthDefault
        {
            get
            {
                return _ColumnWidthDefault;
            }
            set
            {
                _ColumnWidthDefault = value;
            }
        }

        public string ColumnWidths
        {
            get
            {
                return _ColumnWidthString;
            }

            set
            {
                // If the column string is blank, leave it blank.
                // The default width will be used for all columns.
                if (!Convert.ToBoolean(value.Trim().Length))
                {
                    _ColumnWidthString = "";
                }
                else if (value != null)
                {
                    char[] delimiterChars = { ',', ';', ':' };
                    string[] columnWidths = value.Split(delimiterChars);
                    string invalidValue = "";
                    int invalidIndex = -1;
                    int idx = 1;
                    int intValue;

                    // After splitting the string into an array, iterate
                    // through the strings and check that they're all integers
                    // or blanks
                    foreach (string s in columnWidths)
                    {
                        // If it has length, test if it's an integer
                        if (Convert.ToBoolean(s.Trim().Length))
                        {
                            // It's not an integer. Flag the offending value.
                            if (!int.TryParse(s, out intValue))
                            {
                                invalidIndex = idx;
                                invalidValue = s;
                            }
                            else // The value was okay. Increment the item index.
                            {
                                idx++;
                            }
                        }
                        else // The value is a space. Use the default width.
                        {
                            idx++;
                        }
                    }

                    // If an invalid value was found, raise an exception.
                    if (invalidIndex > -1)
                    {
                        string errMsg;

                        errMsg = "Invalid column width '" + invalidValue + "' located at column " + invalidIndex.ToString();
                        throw new ArgumentOutOfRangeException(errMsg);
                    }
                    else // The string is fine
                    {
                        _ColumnWidthString = value;

                        // Only set the values of the collections at runtime.
                        // Setting them at design time doesn't accomplish 
                        // anything and causes errors since the collections 
                        // don't exist at design time.
                        if (!DesignMode)
                        {
                            _ColumnWidths.Clear();
                            foreach (string s in columnWidths)
                            {
                                // Initialize a column width to an integer
                                if (Convert.ToBoolean(s.Trim().Length))
                                {
                                    _ColumnWidths.Add(Convert.ToInt32(s));
                                }
                                else // Initialize the column to the default
                                {
                                    _ColumnWidths.Add(_ColumnWidthDefault);
                                }
                            }

                            // If the column is bound to data, set the column widths
                            // for any columns that aren't explicitly set by the 
                            // string value entered by the programmer
                            if (DataManager != null)
                            {
                                InitializeColumns();
                            }
                        }
                    }
                }
            }
        }

        public new DrawMode DrawMode
        {
            get
            {
                return base.DrawMode;
            }
            set
            {
                if (value != DrawMode.OwnerDrawVariable)
                {
                    throw new NotSupportedException("Needs to be DrawMode.OwnerDrawVariable");
                }
                base.DrawMode = value;
            }
        }

        public new ComboBoxStyle DropDownStyle
        {
            get
            {
                return base.DropDownStyle;
            }
            set
            {
                if (value != ComboBoxStyle.DropDown)
                {
                    throw new NotSupportedException("ComboBoxStyle.DropDown is the only supported style");
                }
                base.DropDownStyle = value;
            }
        }

        public int LinkedColumnIndex
        {
            get
            {
                return _LinkedColumnIndex;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("A column index can not be negative");
                }
                _LinkedColumnIndex = value;
            }
        }

        public TextBox LinkedTextBox
        {
            get
            {
                return _LinkedTextBox;
            }
            set
            {
                _LinkedTextBox = value;

                if (_LinkedTextBox != null)
                {
                    // Set any default properties of the Linked Textbox here
                    _LinkedTextBox.ReadOnly = true;
                    _LinkedTextBox.TabStop = false;
                }
            }
        }

        public int TotalWidth
        {
            get
            {
                return _TotalWidth;
            }
        }

        #endregion

        #region Constructors

        public ComboBoxW()
        {
            DrawMode = DrawMode.OwnerDrawVariable;

            // If all of your boxes will be RightToLeft, uncomment 
            // the following line to make RTL the default.
            //RightToLeft = RightToLeft.Yes;

            // Remove the Context Menu to disable pasting 
            ContextMenu = new ContextMenu();
        }

        private void InitializeColumns()
        {
            if (!Convert.ToBoolean(_ColumnNameString.Length))
            {
                PropertyDescriptorCollection propertyDescriptorCollection = DataManager.GetItemProperties();

                _TotalWidth = 0;
                _ColumnNames.Clear();

                for (int colIndex = 0; colIndex < propertyDescriptorCollection.Count; colIndex++)
                {
                    _ColumnNames.Add(propertyDescriptorCollection[colIndex].Name);

                    // If the index is greater than the collection of explicitly
                    // set column widths, set any additional columns to the default
                    if (colIndex >= _ColumnWidths.Count)
                    {
                        _ColumnWidths.Add(_ColumnWidthDefault);
                    }
                    _TotalWidth += _ColumnWidths[colIndex];
                }
            }
            else
            {
                _TotalWidth = 0;

                for (int colIndex = 0; colIndex < _ColumnNames.Count; colIndex++)
                {
                    // If the index is greater than the collection of explicitly
                    // set column widths, set any additional columns to the default
                    if (colIndex >= _ColumnWidths.Count)
                    {
                        _ColumnWidths.Add(_ColumnWidthDefault);
                    }
                    _TotalWidth += _ColumnWidths[colIndex];
                }

            }

            // Check to see if the programmer is trying to display a column
            // in the linked textbox that is greater than the columns in the 
            // ComboBox. I handle this error by resetting it to zero.
            if (_LinkedColumnIndex >= _ColumnNames.Count)
            {
                _LinkedColumnIndex = 0; // Or replace this with an OutOfBounds Exception
            }
        }

        private void InitializeValueMemberColumn()
        {
            int colIndex = 0;
            foreach (String columnName in _ColumnNames)
            {
                if (String.Compare(columnName, ValueMember, true, CultureInfo.CurrentUICulture) == 0)
                {
                    _ValueMemberColumnIndex = colIndex;
                    break;
                }
                colIndex++;
            }
        }

        #endregion

        #region Override

        protected override void OnMouseMove(MouseEventArgs e)
        {

            base.OnMouseMove(e);
            Point point = e.Location;
            if (ButtonRect.Contains(point))
            {
                ButtonState = ControlState.Hover;
            }
            else
            {
                ButtonState = ControlState.Normal;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            Point point = PointToClient(Cursor.Position);
            if (ButtonRect.Contains(point))
            {
                ButtonState = ControlState.Hover;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ButtonState = ControlState.Normal;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ButtonState = ControlState.Hover;
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);

            InitializeColumns();
            if (_bShowColumnName)
            {
                this.SelectedIndex = 1;
            }
        }

        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            base.OnSelectionChangeCommitted(e);
            if (_bShowColumnName)
            {
                if (this.SelectedIndex == 0)
                {
                    this.SelectedIndex = 1;
                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (DesignMode)
                return;

            e.DrawBackground();

            Rectangle boundsRect = e.Bounds;
            int lastRight = 0;

            Color brushForeColor;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // Item is not selected. Use BackColorOdd & BackColorEven
                Color backColor;
                backColor = Convert.ToBoolean(e.Index % 2) ? _BackColorOdd : _BackColorEven;
                using (SolidBrush brushBackColor = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(brushBackColor, e.Bounds);
                }
                brushForeColor = Color.Black;
            }
            else
            {
                GraphicsPath pa = new GraphicsPath();
                StyleHelp.DrawArc(e.Bounds, pa, 6, e_groupPos.None);
                LinearGradientBrush lgbrush = new LinearGradientBrush(e.Bounds, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                lgbrush.InterpolationColors = ColorHelper.GetBlend4();
                e.Graphics.FillRectangle(lgbrush, e.Bounds);
                Rectangle rect = e.Bounds;
                rect.Width--;
                rect.Height--;
                using (Pen pen = new Pen(Color.FromArgb(255, 255, 224, 131)))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
                // Item is selected. Use ForeColor = White
                //brushForeColor = Color.White;
                brushForeColor = Color.Black;
            }
            if (_bShowColumnName && e.Index == 0)
            {
                GraphicsPath pa = new GraphicsPath();
                StyleHelp.DrawArc(e.Bounds, pa, 6, e_groupPos.None);
                LinearGradientBrush lgbrush = new LinearGradientBrush(e.Bounds, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                lgbrush.InterpolationColors = ColorHelper.GetBlendSliver();
                e.Graphics.FillRectangle(lgbrush, e.Bounds);
                Rectangle rect = e.Bounds;
                rect.Width--;
                rect.Height--;
                using (Pen pen = new Pen(Color.FromArgb(200, 225, 229, 240)))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }

            using (Pen linePen = new Pen(SystemColors.GrayText))
            {
                using (SolidBrush brush = new SolidBrush(brushForeColor))
                {
                    if (!Convert.ToBoolean(_ColumnNames.Count))
                    {
                        e.Graphics.DrawString(Convert.ToString(Items[e.Index]), Font, brush, boundsRect);
                    }
                    else
                    {
                        // If the ComboBox is displaying a RightToLeft language, draw it this way.
                        if (RightToLeft.Equals(RightToLeft.Yes))
                        {
                            // Define a StringFormat object to make the string display RTL.
                            StringFormat rtl = new StringFormat();
                            rtl.Alignment = StringAlignment.Near;
                            rtl.FormatFlags = StringFormatFlags.DirectionRightToLeft;

                            // Draw the strings in reverse order from high column index to zero column index.
                            for (int colIndex = _ColumnNames.Count - 1; colIndex >= 0; colIndex--)
                            {
                                if (Convert.ToBoolean(_ColumnWidths[colIndex]))
                                {
                                    string item = Convert.ToString(FilterItemOnProperty(Items[e.Index], _ColumnNames[colIndex]));

                                    boundsRect.X = lastRight;
                                    boundsRect.Width = (int)_ColumnWidths[colIndex];
                                    lastRight = boundsRect.Right;

                                    // Draw the string with the RTL object.
                                    e.Graphics.DrawString(item, Font, brush, boundsRect, rtl);

                                    if (colIndex > 0)
                                    {
                                        e.Graphics.DrawLine(linePen, boundsRect.Right, boundsRect.Top, boundsRect.Right, boundsRect.Bottom);
                                    }
                                }
                            }
                        }
                        // If the ComboBox is displaying a LeftToRight language, draw it this way.
                        else
                        {
                            // Display the strings in ascending order from zero to the highest column.
                            for (int colIndex = 0; colIndex < _ColumnNames.Count; colIndex++)
                            {
                                if (Convert.ToBoolean(_ColumnWidths[colIndex]))
                                {
                                    string item = Convert.ToString(FilterItemOnProperty(Items[e.Index], _ColumnNames[colIndex]));

                                    boundsRect.X = lastRight;
                                    boundsRect.Width = (int)_ColumnWidths[colIndex];
                                    lastRight = boundsRect.Right;
                                    e.Graphics.DrawString(item, Font, brush, boundsRect);

                                    if (colIndex < _ColumnNames.Count - 1)
                                    {
                                        e.Graphics.DrawLine(linePen, boundsRect.Right, boundsRect.Top, boundsRect.Right, boundsRect.Bottom);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            e.DrawFocusRectangle();
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            if (_TotalWidth > 0)
            {
                if (Items.Count > MaxDropDownItems)
                {
                    // The vertical scrollbar is present. Add its width to the total.
                    // If you don't then RightToLeft languages will have a few characters obscured.
                    this.DropDownWidth = _TotalWidth + SystemInformation.VerticalScrollBarWidth;
                }
                else
                {
                    this.DropDownWidth = _TotalWidth;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            //// Use the Delete or Escape Key to blank out the ComboBox and
            //// allow the user to type in a new value
            //string tr = Text;
            //if ((e.KeyCode == Keys.Delete) ||
            //    (e.KeyCode == Keys.Escape))
            //{
            //    if (_EscEnabled)
            //    {
            //        SelectedIndex = -1;
            //        Text = "";
            //        if (_LinkedTextBox != null)
            //        {
            //            _LinkedTextBox.Text = "";
            //        }
            //    }
            //}
            //else if (e.KeyCode == Keys.F3)
            //{
            //    // Fire the OpenSearchForm Event
            //    if (OpenSearchForm != null)
            //    {
            //        OpenSearchForm(this, System.EventArgs.Empty);
            //    }
            //}
        }

        // Some of the code for OnKeyPress was derived from some VB.NET code  
        // posted by Laurent Muller as a suggested improvement for another control.
        // http://www.codeproject.com/vb/net/autocomplete_combobox.asp?df=100&forumid=3716&select=579095#xx579095xx
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            int idx = -1;
            string toFind;

            DroppedDown = _AutoDropdown;
            if (!Char.IsControl(e.KeyChar))
            {
                if (_AutoComplete)
                {
                    toFind = Text.Substring(0, SelectionStart) + e.KeyChar;
                    idx = FindStringExact(toFind);

                    if (idx == -1)
                    {
                        // An exact match for the whole string was not found
                        // Find a substring instead.
                        idx = FindString(toFind);
                    }
                    else
                    {
                        // An exact match was found. Close the dropdown.
                        DroppedDown = false;
                    }

                    if (idx != -1) // The substring was found.
                    {
                        SelectedIndex = idx;
                        SelectionStart = toFind.Length;
                        SelectionLength = Text.Length - SelectionStart;
                    }
                    else // The last keystroke did not create a valid substring.
                    {
                        // If the substring is not found, cancel the keypress
                        e.KeyChar = (char)0;
                    }
                }
                else // AutoComplete = false. Treat it like a DropDownList by finding the
                // KeyChar that was struck starting from the current index
                {
                    idx = FindString(e.KeyChar.ToString(), SelectedIndex);

                    if (idx != -1)
                    {
                        SelectedIndex = idx;
                    }
                }
            }

            // Do no allow the user to backspace over characters. Treat it like
            // a left arrow instead. The user must not be allowed to change the 
            // value in the ComboBox. 
            if ((e.KeyChar == (char)(Keys.Back)) &&  // A Backspace Key is hit
                (_AutoComplete) &&                   // AutoComplete = true
                (Convert.ToBoolean(SelectionStart))) // And the SelectionStart is positive
            {
                // Find a substring that is one character less the the current selection.
                // This mimicks moving back one space with an arrow key. This substring should
                // always exist since we don't allow invalid selections to be typed. If you're
                // on the 3rd character of a valid code, then the first two characters have to 
                // be valid. Moving back to them and finding the 1st occurrence should never fail.
                toFind = Text.Substring(0, SelectionStart - 1);
                idx = FindString(toFind);

                if (idx != -1)
                {
                    SelectedIndex = idx;
                    SelectionStart = toFind.Length;
                    SelectionLength = Text.Length - SelectionStart;
                }
            }

            // e.Handled is always true. We handle every keystroke programatically.
            e.Handled = true;
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e); //Added after version 1.3 on 01/31/2008

            if (_LinkedTextBox != null)
            {
                if (_LinkedColumnIndex < _ColumnNames.Count)
                {
                    _LinkedTextBox.Text = Convert.ToString(FilterItemOnProperty(SelectedItem, _ColumnNames[_LinkedColumnIndex]));
                }
            }
            ButtonState = ControlState.Normal;
           // base.SelectionStart = 0;
        }

        protected override void OnValueMemberChanged(EventArgs e)
        {
            base.OnValueMemberChanged(e);

            InitializeValueMemberColumn();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WinMsgs.WM_PAINT:
                    WmPaint(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region Windows Message Methods

        private void WmPaint(ref Message m)
        {

            if (base.DropDownStyle == ComboBoxStyle.Simple)
            {
                base.WndProc(ref m);
                return;
            }
            else//if (base.DropDownStyle == ComboBoxStyle.DropDown)
            {
                if (!_bPainting)
                {
                    PAINTSTRUCT ps =
                        new PAINTSTRUCT();

                    _bPainting = true;
                    WinMethod.BeginPaint(m.HWnd, ref ps);

                    RenderComboBox(ref m);

                    WinMethod.EndPaint(m.HWnd, ref ps);
                    _bPainting = false;
                    m.Result = KnownParam.TRUE;
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            //else
            //{
            //    base.WndProc(ref m);
            //    RenderComboBox(ref m);
            //}
        }

        #endregion

        #region Method

        private bool GetComboBoxButtonPressed()
        {
            ComboBoxInfo cbi = GetComboBoxInfo();
            return cbi.stateButton == ComboBoxButtonState.STATE_SYSTEM_PRESSED;
        }

        private Rectangle GetDropDownButtonRect()
        {
            ComboBoxInfo cbi = GetComboBoxInfo();

            return cbi.rcButton.Rect;
        }

        private ComboBoxInfo GetComboBoxInfo()
        {
            ComboBoxInfo cbi = new ComboBoxInfo();
            cbi.cbSize = Marshal.SizeOf(cbi);
            WinMethod.GetComboBoxInfo(base.Handle, ref cbi);
            return cbi;
        }

        #endregion

        #region Render Methods

        private void RenderComboBox(ref Message m)
        {
            Rectangle rect = new Rectangle(Point.Empty, Size);
            Rectangle buttonRect = ButtonRect;
            ControlState state = ButtonPressed ?
                ControlState.Pressed : ButtonState;
            using (Graphics g = Graphics.FromHwnd(m.HWnd))
            {
                RenderComboBoxBackground(g, rect, buttonRect);
                RenderConboBoxDropDownButton(g, ButtonRect, state);
                RenderConboBoxBorder(g, rect);
            }
        }

        private void RenderConboBoxBorder(
            Graphics g, Rectangle rect)
        {
            Color borderColor = base.Enabled ?
                _borderColor : SystemColors.ControlDarkDark;
            using (Pen pen = new Pen(borderColor))
            {
                rect.Width--;
                rect.Height--;
                g.DrawRectangle(pen, rect);
            }
        }

        private void RenderComboBoxBackground(
            Graphics g,
            Rectangle rect,
            Rectangle buttonRect)
        {
            Color backColor = base.Enabled ?
                base.BackColor : SystemColors.Control;
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                buttonRect.Inflate(-1, -1);
                rect.Inflate(-1, -1);
                using (Region region = new Region(rect))
                {
                    region.Exclude(buttonRect);
                    region.Exclude(EditRect);
                    g.FillRegion(brush, region);
                }
            }

            //using (SolidBrush brush = new SolidBrush(Color.Black))
            //{
            //    buttonRect.Inflate(-1, -1);
            //    rect.Inflate(-1, -1);
            //    using (Region region = new Region(rect))
            //    {
            //        //region.Exclude(buttonRect);
            //        //region.Exclude(EditRect);
            //        g.FillRegion(brush, region);
            //    }
            //}
        }

        private void RenderConboBoxDropDownButton(
            Graphics g,
            Rectangle buttonRect,
            ControlState state)
        {
            Color baseColor;
            Color backColor = Color.FromArgb(160, 250, 250, 250);
            Color borderColor = base.Enabled ?
                _borderColor : SystemColors.ControlDarkDark;
            Color arrowColor = base.Enabled ?
                _arrowColor : SystemColors.ControlDarkDark;
            Rectangle rect = buttonRect;

            if (base.Enabled)
            {
                switch (state)
                {
                    case ControlState.Hover:
                        //baseColor = RenderHelper.GetColor(_baseColor, 0, -33, -22, -13);

                        baseColor = Color.FromArgb(255, 214, 108);//自定义
                        borderColor = Color.FromArgb(196, 177, 118);//自定义

                        break;
                    case ControlState.Pressed:
                        //baseColor = RenderHelper.GetColor(_baseColor, 0, -65, -47, -25);
                        baseColor = Color.FromArgb(255, 189, 105);//自定义
                        borderColor = Color.FromArgb(196, 177, 118);//自定义

                        break;
                    default:
                        baseColor = _baseColor;
                        break;
                }
            }
            else
            {
                baseColor = SystemColors.ControlDark;
            }

            rect.Inflate(-1, -1);

            RenderScrollBarArrowInternal(
                g,
                rect,
                baseColor,
                borderColor,
                backColor,
                arrowColor,
                RoundStyle.All,
                true,
                false,
                ArrowDirection.Down,
                LinearGradientMode.Vertical);
        }

        internal void RenderScrollBarArrowInternal(
           Graphics g,
           Rectangle rect,
           Color baseColor,
           Color borderColor,
           Color innerBorderColor,
           Color arrowColor,
           RoundStyle roundStyle,
           bool drawBorder,
           bool drawGlass,
           ArrowDirection arrowDirection,
           LinearGradientMode mode)
        {
            RenderHelper.RenderBackgroundInternal(
               g,
               rect,
               baseColor,
               borderColor,
               innerBorderColor,
               roundStyle,
               1,
               .25F,
               drawBorder,
               drawGlass,
               mode);

            using (SolidBrush brush = new SolidBrush(arrowColor))
            {
                RenderArrowInternal(
                    g,
                    rect,
                    arrowDirection,
                    brush);
            }
        }

        internal void RenderArrowInternal(
            Graphics g,
            Rectangle dropDownRect,
            ArrowDirection direction,
            Brush brush)
        {
            Point point = new Point(
                dropDownRect.Left + (dropDownRect.Width / 2),
                dropDownRect.Top + (dropDownRect.Height / 2));
            Point[] points = null;
            switch (direction)
            {
                case ArrowDirection.Left:
                    points = new Point[] { 
                        new Point(point.X + 2, point.Y - 3), 
                        new Point(point.X + 2, point.Y + 3), 
                        new Point(point.X - 1, point.Y) };
                    break;

                case ArrowDirection.Up:
                    points = new Point[] { 
                        new Point(point.X - 3, point.Y + 2), 
                        new Point(point.X + 3, point.Y + 2), 
                        new Point(point.X, point.Y - 2) };
                    break;

                case ArrowDirection.Right:
                    points = new Point[] {
                        new Point(point.X - 2, point.Y - 3), 
                        new Point(point.X - 2, point.Y + 3), 
                        new Point(point.X + 1, point.Y) };
                    break;

                default:
                    points = new Point[] {
                        new Point(point.X - 2, point.Y - 1), 
                        new Point(point.X + 3, point.Y - 1), 
                        new Point(point.X, point.Y + 2) };
                    break;
            }
            g.FillPolygon(brush, points);
        }

        #endregion

        #region ISkyrayStyle 成员
        public void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:
                    _baseColor = Color.FromArgb(175, 210, 255);//自定义
                    _borderColor = Color.FromArgb(121, 153, 194);//自定义
                    _arrowColor = Color.FromArgb(19, 88, 128);
                    this.Refresh();
                    break;
                case Style.Office2007Sliver:
                    _baseColor = Color.FromArgb(199, 203, 209);//自定义
                    _borderColor = Color.FromArgb(111, 112, 116);//自定义
                    _arrowColor = Color.FromArgb(111, 112, 116);
                    this.Refresh();
                    break;

                default: break;
            }
        }

        private Style _Style = Style.Office2007Blue;

        [DefaultValue(Style.Office2007Blue)]
        public Style Style
        {
            get
            {
                return _Style;
            }
            set
            {
                _Style = value;
                SetStyle(_Style);
            }
        }

        #endregion

    }
}
