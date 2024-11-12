using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.Component;

namespace Skyray.EDX.Common
{
    public delegate void VacummEmpty();

    public class VacuumAdjust
    {
        public DeviceParameter DeviceParams;
        public DeviceInterface DeviceInterface;
        public double MoveStationVisible;
        public VacummEmpty OnVacummEmpty;

        public VacuumAdjust()
        { }
        
        public void StartVacumm()
        {
            Thread thread = new Thread(new ThreadStart(VacummEmptyRun));
            thread.Start();
        }

        public void VacummEmptyRun()
        {
            if (DeviceParams != null && (DeviceParams.IsVacuum
                       || DeviceParams.IsVacuumDegree) && WorkCurveHelper.DeviceCurrent.HasVacuumPump)
            {

                while (Math.Abs(WorkCurveHelper.Volumngreen - DeviceInterface.ReturnVacuum) > MoveStationVisible)
                {
                    Thread.Sleep(50);
                }
            }
            if (OnVacummEmpty != null)
                OnVacummEmpty();
        }
    }
}
