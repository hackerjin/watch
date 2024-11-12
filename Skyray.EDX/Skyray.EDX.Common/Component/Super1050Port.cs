using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    public class Super1050Port : UsbPort
    {
        public override void OpenXRayTubHV()
        {
            base.OpenPump();
        }

        public override void CloseXRayTubHV()
        {
            base.ClosePump();
        }

    }
}
