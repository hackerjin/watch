using System;
using System.Collections.Generic;
using System.Text;

namespace Skyray.Controls
{
    [Flags]
    public enum TrackBarOwnerDrawParts
    {
        Channel = 4,
        None = 0,
        Thumb = 2,
        Ticks = 1
    }
}
