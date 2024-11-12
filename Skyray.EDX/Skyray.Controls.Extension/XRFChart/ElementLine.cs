using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;

namespace Skyray.Controls.Extension
{
    [Serializable]
    public class ElementLine
    {
        /// <summary>
        /// XCoordy
        /// </summary>
        public double XCoordy { get; set; }

        /// <summary>
        /// YCoordy
        /// </summary>
        public double YCoordy { get; set; }

        /// <summary>
        /// line color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// line height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// element name
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// element flag
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// fLineHeight
        /// </summary>
        public float fLineEndPoint { get; set; }

        public ElementLine(double xCoordy, double yCoordy, Color color, int height, string elementName, string flag)
        {
            this.XCoordy = xCoordy;
            this.YCoordy = yCoordy;
            this.Color = color;
            this.Height = height;
            this.ElementName = elementName;
            this.Flag = flag;
        }
    }
}
