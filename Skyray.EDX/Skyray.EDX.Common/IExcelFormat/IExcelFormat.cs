using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common
{
    public interface IExcelFormat
    {
        bool Execute(List<HistoryRecord> historyList);
    }
}
