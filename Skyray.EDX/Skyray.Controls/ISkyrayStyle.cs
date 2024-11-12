using System;
using System.Collections.Generic;

using System.Text;

namespace Skyray.Controls
{
    public interface ISkyrayStyle
    {
        Style Style { get; set; }
        void SetStyle(Style style);
    }
}
