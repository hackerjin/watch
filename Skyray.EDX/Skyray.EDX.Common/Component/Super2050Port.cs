using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common.Component
{
    public class Super2050Port : UsbPort
    {
        public override void OpenXRayTubHV()
        {
            bXRayTubeSel(1);
        }

        public override void CloseXRayTubHV()
        {
            bXRayTubeSel(0);
        }
    }
}
