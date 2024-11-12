using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
namespace Skyray.Controls
{
    public class ToolStripW : ToolStrip,ISkyrayStyle
    {
        public ToolStripW()
        {
            //this.Renderer = new Office2007Renderer();
        }

        #region ISkyrayStyle 成员

        public void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:
                    this.Renderer = new Office2007Renderer();
                    this.Refresh();
                    break;
                case Style.Office2007Sliver:
                    this.Renderer = new Office2007Renderer(new Office2007SilverColorTable());
                    this.Refresh();
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
