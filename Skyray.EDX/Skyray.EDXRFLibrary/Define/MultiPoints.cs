using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class MultiPoints : DbObjectModel<MultiPoints>
    {
        [BelongsTo, DbColumn("Device_Id")]
        public abstract Device Device { get; set; }

        public abstract string Name { get; set; }

        public abstract int Number { get; set; }

        public abstract int X { get; set; }

        public abstract int Y { get; set; }

        public abstract MultiPoints Init(
            string Name,
            int Number,
            int X,
            int Y
            );

    }


}
