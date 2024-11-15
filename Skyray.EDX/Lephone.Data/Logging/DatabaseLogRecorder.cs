﻿using System;
using Lephone.Util.Logging;

namespace Lephone.Data.Logging
{
    public class DatabaseLogRecorder : ILogRecorder
    {
        public void ProcessLog(LogType type, string source, string name, string message, Exception eException)
        {
            var li = new LephoneLog(type, source, name, message, eException);
            try
            {
                DbEntry.Save(li);
            }
            catch (Exception ex)
            {
                string msg = (eException == null) ? message : message + "\n" + eException;
                ((ILogDirect)Logger.System).Log(type, source, name, msg, ex);
            }
        }
    }
}
