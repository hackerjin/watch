using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDXRFLibrary.Spectrum
{
    [Serializable]
    public class DemarcateEnergyEntity
    {
        public  string ElementName { get; set; }
        public  XLine Line { get; set; }
        public  double Energy { get; set; }
        public  double Channel { get; set; }

        public DemarcateEnergyEntity() { }
        public DemarcateEnergyEntity(string ElementName, XLine Line, double Energy, double Channel)
        {
            this.ElementName = ElementName;
            this.Line = Line;
            this.Energy = Energy;
            this.Channel = Channel;
        }
        public DemarcateEnergy ConvertFrom()
        {
            DemarcateEnergy energy = DemarcateEnergy.New.Init(this.ElementName, this.Line, this.Energy, this.Channel);
            return energy;
        }
    }
}
