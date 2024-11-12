using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common.Component
{
    class BlueTeethPort:Port
    {
        public override bool Connect()
        {
            throw new NotImplementedException();
        }

        public override bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public override bool OpenVoltage()
        {
            throw new NotImplementedException();
        }

        public override bool CloseVoltage()
        {
            throw new NotImplementedException();
        }

        public override bool OpenPump()
        {
            throw new NotImplementedException();
        }

        public override bool ClosePump()
        {
            throw new NotImplementedException();
        }

        public override bool OpenVoltageLamp()
        {
            throw new NotImplementedException();
        }

        public override bool CloseVoltageLamp()
        {
            throw new NotImplementedException();
        }

        public override bool GetData(int[] data)
        {
            throw new NotImplementedException();
        }

        public override bool MotorControl(int index, int dir, int step, bool swtStop, int speedGear)
        {
            throw new NotImplementedException();
        }

        public override bool MotorIsIdel(int index)
        {
            throw new NotImplementedException();
        }

        public override bool GetMotorInfo(ref int info)
        {
            throw new NotImplementedException();
        }

        public override bool GetParams(ref int Voltage, ref int Current, ref int Temperature, ref int Vacuum, ref bool IsOpen)
        {
            throw new NotImplementedException();
        }

        public override void setFPGAParam(byte bBaseResume, byte bBaseLimit, byte bHeapUP, byte bRate, byte bCoarse, uint uFine, uint uTime, byte bUPTime, byte bWidthTime, byte bSlowLimit)
        {
            throw new NotImplementedException();
        }

        public override bool SetParam(int tubVoltage, int tubCurrent, int gain, int fineGain)
        {
            throw new NotImplementedException();
        }

        public override void setParam(ushort tubVoltage, ushort tubCurrent)
        {
            throw new NotImplementedException();
        }

        public override void getParam(out ushort uVoltage, out ushort uCurrent, out int iUncover)
        {
            throw new NotImplementedException();
        }

        public override void AllowUncover(bool allowUncover)
        {
            throw new NotImplementedException();
        }

        public override int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods)
        {
            throw new NotImplementedException();
        }

        public override void OpenHeightLaser()
        {
            throw new NotImplementedException();
        }

        public override void CloseHeightLaser()
        {
            throw new NotImplementedException();
        }

        public override void OpenMagnet()
        {
            throw new NotImplementedException();
        }

        public override void CloseMagnet()
        {
            throw new NotImplementedException();
        }

        public override void ConsolePrint(string str)
        {
            throw new NotImplementedException();
        }
    }
}
