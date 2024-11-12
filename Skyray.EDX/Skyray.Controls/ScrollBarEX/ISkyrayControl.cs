using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;

namespace Skyray.Controls
{
    public interface ISkyrayControl
    {
        void DrawBackground(Graphics g, Rectangle rect, ScrollBarOrientation orientation);
        void DrawTrack(Graphics g, Rectangle rect, ScrollBarState state, ScrollBarOrientation orientation);
        void DrawThumb(Graphics g, Rectangle rect, ScrollBarState state, ScrollBarOrientation orientation);
        void DrawThumbGrip(Graphics g, Rectangle rect, ScrollBarOrientation orientation);
        void DrawArrowButton(Graphics g, Rectangle rect, ScrollBarArrowButtonState state, bool arrowUp, ScrollBarOrientation orientation);
    }
}
