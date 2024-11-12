using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;

namespace Skyray.DataConverter.Input
{
    public class RoHS3DataInput
    {

        public static void CreateRoHS3(string deviceName, string workCurvePath)
        {
            Device defaultDevice = Device.FindOne(w => w.IsDefaultDevice == true);
            
        }
    }
}
