using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;


namespace Skyray.EDXRFLibrary
{
    public abstract class Shape : DbObjectModel<Shape>
    {
        [Length(ColLength.ShapeName)]
        public abstract string Name { get; set; }
        [Length(ColLength.Date)]
        public abstract string CreateDate { get; set; }

        public abstract Shape Init(string Name, string CreateDate);
    }
}
