using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract partial class CustomField : DbObjectModel<CustomField>
    {
        [BelongsTo, DbColumn("ElementList_Id")]
        public abstract ElementList ElementList { get; set; }

        [LengthAttribute(ColLength.CustomFieldName)]
        public abstract string Name { get; set; }

        [LengthAttribute(ColLength.CustomFieldExpression)]
        public abstract string Expression { get; set; }

        public abstract CustomField Init(string Name, string Expression);
    }
}
