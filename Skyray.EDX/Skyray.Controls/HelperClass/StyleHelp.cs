using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Skyray.Controls
{
    public class StyleHelp
    {
        public static void DrawArc(Rectangle re, GraphicsPath pa, int radius, e_groupPos _grouppos)
        {
            int _radiusX0Y0 = radius,
                _radiusXFY0 = radius,
                _radiusX0YF = radius,
                _radiusXFYF = radius;

            switch (_grouppos)
            {
                case e_groupPos.Left:
                    _radiusXFY0 = 1; _radiusXFYF = 1;
                    break;
                case e_groupPos.Center:
                    _radiusX0Y0 = 1; _radiusX0YF = 1; _radiusXFY0 = 1; _radiusXFYF = 1;
                    break;
                case e_groupPos.Right:
                    _radiusX0Y0 = 1; _radiusX0YF = 1;
                    break;
                case e_groupPos.Top:
                    _radiusX0YF = 1; _radiusXFYF = 1;
                    break;
                case e_groupPos.Bottom:
                    _radiusX0Y0 = 1; _radiusXFY0 = 1;
                    break;
            }
            pa.AddArc(re.X, re.Y, _radiusX0Y0, _radiusX0Y0, 180, 90);
            pa.AddArc(re.Width - _radiusXFY0, re.Y, _radiusXFY0, _radiusXFY0, 270, 90);
            pa.AddArc(re.Width - _radiusXFYF, re.Height - _radiusXFYF, _radiusXFYF, _radiusXFYF, 0, 90);
            pa.AddArc(re.X, re.Height - _radiusX0YF, _radiusX0YF, _radiusX0YF, 90, 90);
            pa.CloseFigure();
        }
    }

    public enum e_groupPos
    {
        None, Left, Center, Right, Top, Bottom
    }
}
