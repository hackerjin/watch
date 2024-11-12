using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data.Definition;

using Lephone.Util;

namespace Skyray.EDXRFLibrary
{
    public abstract class SpecElementComposition : DbObjectModel<SpecElementComposition>
    {
        public new abstract int Id { set; get; }

        public abstract int ElementId { set; get; }

        public abstract int SpecId { set; get; }

        [Length(ColLength.ElementValue)]
        public abstract string ElementValue { set; get; }

        public abstract SpecElementComposition Init(int id, int elementId, int specId, string elementValue);
    }
}
