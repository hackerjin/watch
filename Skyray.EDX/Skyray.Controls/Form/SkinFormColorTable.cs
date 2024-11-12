using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Skyray.Controls
{
    public class SkinFormColorTable
    {
        //private static readonly Color _captionActive =
        private static Color _captionActive =
            Color.FromArgb(80, 128, 218);
        //Color.FromArgb(255, 35, 72, 132);

        private static readonly Color _captionDeactive =
           //Color.FromArgb(150, 175, 210, 255);
        Color.FromArgb(75, 120, 200);
        private static readonly Color _captionText =
            Color.FromArgb(40, 111, 152);
        private static Color _border =
            //Color.FromArgb(255, 175, 210, 255);

        Color.FromArgb(80, 128, 218);
        private static readonly Color _innerBorder =
         Color.FromArgb(255, 255, 255, 255);

        private static readonly Color _back = Color.Transparent;

        private static readonly Color _controlBoxActive =
             Color.FromArgb(255, 175, 210, 255);
        private static readonly Color _controlBoxDeactive =
             Color.FromArgb(150, 175, 210, 255);

        private static readonly Color _controlBoxHover =
            Color.FromArgb(37, 114, 151);
        private static readonly Color _controlBoxPressed =
           Color.FromArgb(27, 84, 111);
        private static readonly Color _controlCloseBoxHover =
            Color.FromArgb(213, 66, 22);
        private static readonly Color _controlCloseBoxPressed =
            Color.FromArgb(171, 53, 17);
        private static readonly Color _controlBoxInnerBorder =
            Color.FromArgb(128, 250, 250, 250);

        private static Color _innerFill = Color.FromArgb(255, 115, 152, 227);

        public static Color InnerFill
        {
            get { return SkinFormColorTable._innerFill; }
            set { SkinFormColorTable._innerFill = value; }
        }

        //public virtual Color CaptionActive
        //{
        //    get { return _captionActive; }
        //}
        public static Color CaptionActive
        {
            get { return SkinFormColorTable._captionActive; }
            set { SkinFormColorTable._captionActive = value; }
        }

        public virtual Color CaptionDeactive
        {
            get { return _captionDeactive; }
        }

        public virtual Color CaptionText
        {
            get { return _captionText; }
        }

        public static Color Border
        {
            get { return SkinFormColorTable._border; }
            set { SkinFormColorTable._border = value; }
        }

        //public virtual Color Border
        //{
        //    get { return _border; }
        //}

        public virtual Color InnerBorder
        {
            get { return _innerBorder; }
        }

        public virtual Color Back
        {
            get { return _back; }
        }

        public virtual Color ControlBoxActive
        {
            get { return _controlBoxActive; }
        }

        public virtual Color ControlBoxDeactive
        {
            get { return _controlBoxDeactive; }
        }

        public virtual Color ControlBoxHover
        {
            get { return _controlBoxHover; }
        }

        public virtual Color ControlBoxPressed
        {
            get { return _controlBoxPressed; }
        }

        public virtual Color ControlCloseBoxHover
        {
            get { return _controlCloseBoxHover; }
        }

        public virtual Color ControlCloseBoxPressed
        {
            get { return _controlCloseBoxPressed; }
        }

        public virtual Color ControlBoxInnerBorder
        {
            get { return _controlBoxInnerBorder; }
        }
    }
}
