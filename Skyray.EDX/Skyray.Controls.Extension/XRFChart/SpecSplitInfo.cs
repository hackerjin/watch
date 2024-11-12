using System.Drawing;

namespace Skyray.Controls.Extension
{
    public class SpecSplitInfo
    {
        public string Element;
        public int X1;
        public int X2;
        public Color Color;
        public SpecSplitInfo(string element, int x1, int x2, Color color)
        {
            this.Element = element;
            this.X1 = x1;
            this.X2 = x2;
            this.Color = color;
        }
    }
}
