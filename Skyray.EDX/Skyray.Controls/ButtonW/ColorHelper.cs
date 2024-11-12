using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Skyray.Controls
{
    public class ColorHelper
    {
        //default
        public static Color[] colors0 = new Color[]
        {
            Color.FromArgb(200,223,237,255),  
            Color.FromArgb(200,211,231,255),
            Color.FromArgb(200,207,228,255),
            Color.FromArgb(200,175,210,255),
            Color.FromArgb(200,101,147,207)
        };
        //hover
        public static Color[] colors1 = new Color[]
        {
        //    Color.FromArgb(200,255,255,178),
        //    Color.FromArgb(200,255,214,108),
        //    Color.FromArgb(200,255,214,78),
        //    Color.FromArgb(200,255,255,178),
        //     Color.FromArgb(255,196,177,118)
            Color.FromArgb(255,255,253,238),
            Color.FromArgb(255,255,237,172),
            Color.FromArgb(255,255,224,131),
            Color.FromArgb(255,255,229,155),
            Color.FromArgb(255,196,177,118)
        };
        //press
        public static Color[] colors2 = new Color[]
        {
            //Color.FromArgb(200,255,178,100),
            //Color.FromArgb(200,255,128,30),
            //Color.FromArgb(200,255,128,0),
            //Color.FromArgb(200,255,178,100),
            // Color.FromArgb(255,128,64,0)
            Color.FromArgb(255,255,236,212),
            Color.FromArgb(255,255,198,125),
            Color.FromArgb(255,255,182,88),
            Color.FromArgb(255,255,218,114),
            Color.FromArgb(255,128,64,0)
        };
        //sliver
        public static Color[] colors3 = new Color[]
        {
            //Color.FromArgb(248, 248, 248),
            //Color.FromArgb(218, 219, 231),
            //Color.FromArgb(208, 209, 221),
            //Color.FromArgb(199, 203, 209),
            //Color.FromArgb(111, 112, 116)
            Color.FromArgb(200, 234, 237, 249),
            Color.FromArgb(200, 228, 232, 243),
            Color.FromArgb(200, 225, 229, 240),
            Color.FromArgb(200, 213, 217, 227),
            Color.FromArgb(255,111, 112, 116)
        };

        public static Color[] colors4 = new Color[]
        {
            Color.FromArgb(255,255,255,178),
            Color.FromArgb(255,255,214,108),
            Color.FromArgb(255,255, 189, 105),
            Color.FromArgb(255,255,255,178),
            Color.FromArgb(255,196,177,118)
        };

        public static Color[] colorFormCaption = new Color[] 
        {
            Color.FromArgb(255,96,177,254),  
            Color.FromArgb(255,125,199,255),
            Color.FromArgb(255,129,202,255),
            Color.FromArgb(255,140,210,255),
            Color.FromArgb(255,101,147,207)
        };

        public static ColorBlend GetBlend3()
        {
            ColorBlend mix = new ColorBlend();
            Color[] colors = null;
            colors = ColorHelper.colors4;
            mix.Colors = new Color[] { colors[0], colors[2], colors[3] };
            mix.Positions = new float[] { 0.0F, 0.5F, 1.0F };
            return mix;
        }
        public static ColorBlend GetBlend4()
        {
            ColorBlend mix = new ColorBlend();
            Color[] colors = null;
            colors = ColorHelper.colors1;
            mix.Colors = new Color[] { colors[0], colors[1],colors[2], colors[3] };
            mix.Positions = new float[] { 0.0F, 0.3F,0.35F, 1.0F };
            return mix;
        }

        public static ColorBlend GetBlendSliver()
        {
            ColorBlend mix = new ColorBlend();
            Color[] colors = null;
            colors = ColorHelper.colors3;
            mix.Colors = new Color[] { colors[0], colors[1], colors[2], colors[3] };
            mix.Positions = new float[] { 0.0F, 0.3F, 0.35F, 1.0F };
            return mix;
        }

        public static ColorBlend GetBlendBlue()
        {
            ColorBlend mix = new ColorBlend();
            Color[] colors = null;
            colors = ColorHelper.colors0;
            mix.Colors = new Color[] { colors[0], colors[1], colors[2], colors[3] };
            mix.Positions = new float[] { 0.0F, 0.3F, 0.35F, 1.0F };
            return mix;
        }
    }
}
