using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.EDX.Common.Library
{
   public class DllExport
    {
        [DllImport("fileConverToXML.dll")]
        public static extern int saveToXML(string path);

        [DllImport("SaveToXMLFor600.dll")]
        public static extern int SaveToXMLFor600(string path);
    }
}
