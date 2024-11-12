using System;

namespace Skyray.EDX.Common
{
    public class SerialPortException : Exception
    {
        public SerialPortException(string message)
            : base(message)
        {
        }
    }
}
