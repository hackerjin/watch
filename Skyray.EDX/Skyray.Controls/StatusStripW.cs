using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Skyray.Controls
{
    public class StatusStripW : StatusStrip, ISkyrayStyle
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class TT 
        {
            public int A { get; set; }
            public int B { get; set; }
            public TT(int a,int b)
            {
                A = a;
                B = b;
            }
        }

        public TT TTT { get; set; }


        public StatusStripW()
        {
            this.Renderer = new Office2007Renderer();
        }
        #region ISkyrayStyle 成员

        public void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:
                    this.Renderer = new Office2007Renderer();
                    break;
                case Style.Office2007Sliver:
                    this.Renderer = new Office2007Renderer(new Office2007SilverColorTable());
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
