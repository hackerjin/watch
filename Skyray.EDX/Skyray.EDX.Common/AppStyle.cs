using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Skyray.EDX.Common
{
    public class AppStyle
    {
        //public Size Size { get; set; }

        public bool IsMaxinum { get; set; }

        public bool IsMininum { get; set; }

        public bool IsControlBox { get; set; }

        public string ApplicationName { get; set; }

        public FormWindowState State { get; set; }

        public bool ShowStatusBar { get; set; }

        public bool ShowToolStrip { get; set; }

        public bool ShowMenuBar { get; set; }



    }
}
