using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public abstract partial class EncodeXY : DbObjectModel<EncodeXY>
    {
        public abstract double X { get; set; }
        public abstract double Y { get; set; }
        public abstract EncodeXY Init(double X, double Y);
    }
  
}
