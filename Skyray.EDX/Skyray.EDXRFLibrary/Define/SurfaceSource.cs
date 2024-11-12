using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class SurfaceSourceLight : DbObjectModel<SurfaceSourceLight>
    {
        public abstract ushort FirstLight { get; set; }

        public abstract ushort SecondLight { get; set; }

        public abstract ushort ThirdLight { get; set; }

        public abstract ushort FourthLight { get; set; }

        public abstract SurfaceSourceLight Init(ushort FirstLight, ushort SecondLight, ushort ThirdLight, ushort FourthLight);
    }
}
