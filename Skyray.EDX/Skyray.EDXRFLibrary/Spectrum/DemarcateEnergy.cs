using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDXRFLibrary.Spectrum
{
    public class DemarcateEnergy
    {
            public  string ElementName { get; set; }
            public  XLine Line { get; set; }
            public  double Energy { get; set; }
            public  double Channel { get; set; }
            public DemarcateEnergy(string ElementName, XLine Line, double Energy, double Channel)
            {

            }
    }
}
