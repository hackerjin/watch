using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Controls
{
    public class ListViewW : ListView, ISkyrayStyle
    {
        #region ISkyrayStyle 成员
        public void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:

                    break;
                case Style.Office2007Sliver:

                    break;

                default: break;
            }
        }

        private Style _Style = Style.Office2007Blue;
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
