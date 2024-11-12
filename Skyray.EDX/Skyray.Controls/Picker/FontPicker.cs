using System;
using System.Drawing;
using System.Text;

namespace Skyray.Controls
{
    /// <summary>
    /// Represents a Windows picker box that displays Font values.
    /// </summary>
    public class FontPicker : PickerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FontPicker()
            : base(typeof(Font))
        {
            Value = SystemFonts.DefaultFont;
        }

        /// <summary>
        /// Value
        /// </summary>
        public new Font Value
        {
            get
            {
                return (Font)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }
    }
}
