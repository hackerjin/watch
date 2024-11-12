using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;

namespace Skyray.Controls.Extension
{
    public class ElementS
    {
        /// <summary>
        /// ElementName
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// FontSize
        /// </summary>
        public float FontSize { get; set; }

        /// <summary>
        /// Coordy
        /// </summary>
        public double Coordy { get; set; }

        /// <summary>
        /// FontColor
        /// </summary>
        public Color FontColor { get; set; }

        public ElementS(string elementName, float fontSize, Color fontColor, double coordy)
        {
            this.ElementName = elementName;
            this.FontSize = fontSize;
            this.FontColor = fontColor;
            this.Coordy = coordy;
        }
    }
}
