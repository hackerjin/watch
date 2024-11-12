using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Skyray.Controls
{
    public abstract class PanelRender
    {
        public abstract Region CreateRegion(Panel form);

        public abstract void InitSkinForm(Panel form);
    }
}
