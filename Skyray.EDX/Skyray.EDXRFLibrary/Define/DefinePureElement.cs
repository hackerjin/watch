using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;


namespace Skyray.EDXRFLibrary
{
    public abstract class DefinePureElement : DbObjectModel<DefinePureElement>
    {
        [LengthAttribute(ColLength.SupplierName)]
        public abstract string Name { get; set; }

        [LengthAttribute(ColLength.Date)]
        public abstract string CreateDate { get; set; }

        public abstract DefinePureElement Init(string Name, string CreateDate);
    }
}
