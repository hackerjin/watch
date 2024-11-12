using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Skyray.EDXRFLibrary.Define
{
    [Serializable]
    public class TitleIco
    {
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private Icon ico;

        public Icon Ico
        {
            get { return ico; }
            set { ico = value; }
        }
    }
}
