using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary.Define
{
    public abstract class PWDLock : DbObjectModel<PWDLock>
    {
        public abstract string Password { get; set; }

        public abstract PWDLock Init(string Password);
    }
}
