using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    public abstract class ContinuousResult : DbObjectModel<ContinuousResult>
    {
        public abstract string SampleName { get; set; }

        public abstract string Supplier { get; set; }

        public abstract DateTime? TestDate { get; set; }

        public abstract long HistoryId { get; set; }

        public abstract int TestTime { get; set; }

        public abstract long ContinuousNumber { get; set; }

        [Auto("Info.HistoryRecordCode")]
        public abstract string HistoryRecordCode { get; set; }//流水号

    }
}
