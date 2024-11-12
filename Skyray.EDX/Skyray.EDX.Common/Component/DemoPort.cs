using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common.Component
{
    public class DemoPort : Port
    {
        public override void OpenXRayTubHV()
        {
            //throw new NotImplementedException();
        }

        public override void CloseXRayTubHV()
        {
            //throw new NotImplementedException();
        }

  

        public override bool IPSettings(string IP, string SubNet, string GateWay, string DNS)
        {
            //throw new NotImplementedException();
            return true;
        }

        public override bool SetSurfaceSource(ushort firstLight, ushort secondLight, ushort thirdLight, ushort fourthLight)
        {
            return true;
        }

        public override bool Connect()
        {
            return true;
        }

        public override bool Disconnect()
        {
            return false;
        }

        public override bool OpenVoltage()
        {
            return true;
        }

        public override bool CloseVoltage()
        {
            return true;
        }

        public override bool OpenPump()
        {
            return true;
        }

        public override bool ClosePump()
        {
            return true;
        }

        public override bool OpenVoltageLamp()
        {
            return true;
        }

        public override bool CloseVoltageLamp()
        {
            return true;
        }

        public override bool GetData(int[] data)
        {
            return true;
        }

        public override bool MotorControl(int index, int dir, int step, bool swtStop, int speedGear)
        {
            return true;
        }
        public override bool MotorIsIdelAll(int X,int Y)
        {
            return true;
        }

        public override bool MotorIsIdel(int index)
        {
            return true;
        }

        public override bool GetMotorInfo(ref int info)
        {
            return true;
        }

        public override bool GetParams(ref int Voltage, ref int Current, ref int Temperature, ref int Vacuum, ref bool IsOpen)
        {
            //throw new NotImplementedException();
            return true;
        }

        public override void setFPGAParam(byte bBaseResume, byte bBaseLimit, byte bHeapUP, byte bRate, byte bCoarse, uint uFine, uint uTime, byte bUPTime, byte bWidthTime, byte bSlowLimit, double dIntercept)
        {
            //return true;
        }

        public override bool SetParam(int tubVoltage, int tubCurrent, int gain, int fineGain)
        {
            return true;
        }

        public override void setParam(double tubVoltage, double tubCurrent)
        {
            //return true;
        }

        public override void getParam(out double uVoltage, out double uCurrent, out int iUncover)
        {
            //throw new NotImplementedException();
            uVoltage = 0;
            uCurrent = 0;
            iUncover = 0;
        }

        public override void AllowUncover(bool allowUncover)
        {
            //throw new NotImplementedException();
        }

        public override int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods)
        {
            return 0;
        }

        public override void OpenElectromagnet()
        {
            //throw new NotImplementedException();
        }

        public override void CloseElectromagnet()
        {
            //throw new NotImplementedException();
        }

        public override void OpenMagnet()
        {
            //throw new NotImplementedException();
        }

        public override void CloseMagnet()
        {
            //throw new NotImplementedException();
        }

        public override void ConsolePrint(string str)
        {
            //throw new NotImplementedException();
        }

        //public override event EventHandler GetConnect;

        public override bool GetVacuum(uint uType, out uint uVacuum)
        {
            uType = 0;
            uVacuum = 0;
            return true;
        }
        public override void Dispose()
        { }

        public override int GetDeviceKeyInfo()
        {
            return 0;
        }

        public override bool OpenDevice()
        {
            return true;
        }
        public override bool GetDoubleVacuum(uint uType, out uint pUpVacuum, out uint pDownVacuum)
        {
            pUpVacuum = 0;
            pDownVacuum = 0;
            return true;
        }

        public override LightStatus GetLightShutState(int index)
        {
            return LightStatus.Fail;
        }

        public override void GetChamberStatus(ref int steps, ref int chambeIndex, ref bool istruestep, ref bool initialed, ref bool isbusy)
        {
           
        }


        public override int ResetChamber(int ID, int DirectionFlag, int DefSpeed)
        {
            return -1;
        }
        public override bool LockHV(bool bLock)
        {
            return false;
        }
        public override void OpenHeightLaser(bool bOpen)
        {
        }
        public override void MoveZAutoMotor(int iSpeed)
        {
        }

        public override void GetSwitch()
        {
        }
    }
}
