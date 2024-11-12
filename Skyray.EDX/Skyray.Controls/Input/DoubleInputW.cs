using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Drawing;

namespace Skyray.Controls
{
    public class DoubleInputW : TextBoxW,ISkyrayStyle
    {
        private int _decimalDigits = 2;
        private string _matchString = string.Empty;
        private string _formatString = string.Empty;

        private const int WM_CHAR = 0x0102;
        private const int WM_PASTE = 0X302;

        private static readonly string MatchStringFormat = @"^[0-9]+(.[0-9]{{1,{0}}})?$";

        public DoubleInputW()
            : base()
        {
        }

        [Description("小数的位数。")]
        [DefaultValue(2)]
        public int DecimalDigits
        {
            get { return _decimalDigits; }
            set 
            {
                if (_decimalDigits != value)
                {
                    _decimalDigits = value;
                    _matchString = string.Empty;
                    _formatString = string.Empty;
                }
            }
        }

        [Browsable(false)]
        public decimal Number
        {
            get 
            {
                if (string.IsNullOrEmpty(Text))
                {
                    return 0;
                }
                return decimal.Parse(Text);
            }
        }

        private string MatchString
        {
            get
            {
                if (_matchString == string.Empty)
                {
                    _matchString = string.Format(
                       MatchStringFormat,
                       _decimalDigits.ToString());
                }
                return _matchString;
            }
        }

        private string FormatString
        {
            get
            {
                if (_formatString == string.Empty)
                {
                    _formatString = "f" + DecimalDigits.ToString();
                }
                return _formatString;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (!string.IsNullOrEmpty(Text))
            {
                Text = Number.ToString(FormatString);
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PASTE:
                    IDataObject iData = Clipboard.GetDataObject();
                    if (iData.GetDataPresent(DataFormats.Text))//粘贴的内容是否是文本
                    {
                        string str;
                        str = (String)iData.GetData(DataFormats.Text);
                        if (Regex.IsMatch(str, MatchString)) //文本内容是不是数字
                        {
                            break;
                        }
                    }
                    m.Result = IntPtr.Zero;
                    return;
                case WM_CHAR:
                    int keyChar = m.WParam.ToInt32();
                    bool charIsNumber = (keyChar > 47 && keyChar < 58);  //数字
                    bool charIsCommand =
                        keyChar == 8 ||                                //退格
                        keyChar == 26 ||                               //撤销
                        keyChar == 3 || keyChar == 22 || keyChar == 24;//拷贝、粘贴、剪切
                    //bool charIsDot = keyChar == 46;
                    bool charIsDot = false;
                    if (_decimalDigits != 0 && keyChar == 46)
                    {
                        charIsDot = true;
                    }
                    int dotIndex = Text.IndexOf('.');
                    if (charIsNumber)
                    {
                        if (dotIndex != -1)
                        {
                            if (SelectionStart > dotIndex)
                            {
                                if (Text.Length - dotIndex > DecimalDigits)
                                {
                                    m.WParam = IntPtr.Zero;
                                }
                            }
                        }
                    }
                    else if (charIsDot)
                    {
                        if (dotIndex != -1)
                        {
                            if (SelectionStart > dotIndex ||
                                dotIndex > SelectionStart + SelectionLength)
                            {
                                m.WParam = IntPtr.Zero;
                            }
                        }
                        else
                        {
                            if (Text.Length - SelectionStart > DecimalDigits)
                            {
                                m.WParam = IntPtr.Zero;
                            }
                        }
                    }
                    else if (!charIsCommand)
                    {
                        m.WParam = IntPtr.Zero;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        #region ISkyrayStyle 成员
        public new void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:
                    base.BorderColor = Color.FromArgb(121, 153, 194);
                    this.Refresh();
                    break;
                case Style.Office2007Sliver:
                    base.BorderColor = Color.FromArgb(111, 112, 116);
                    this.Refresh();
                    break;

                default: break;
            }
        }

        private Style _Style = Style.Office2007Blue;

        [DefaultValue(Style.Office2007Blue)]
        public new Style Style
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
