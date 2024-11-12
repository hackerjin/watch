using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Skyray.EDX.Common.Component
{
   // [StructLayout(LayoutKind.Explicit, Size = 0x5c), Description]
    [StructLayout(LayoutKind.Explicit, Size = 0x60), Description]  
    public struct _DPP_CONFIG_SETTINGS
    {
        [FieldOffset(0)]
        public byte AcqMode;
        [FieldOffset(0x59)]
        public byte AnalogOut;
        [FieldOffset(0x5c)]
        public byte AudibleCounter;
        [FieldOffset(0x5b)]
        public byte AuxOut;
        [FieldOffset(0x39)]
        public byte BaselineOn;
        [FieldOffset(0x38)]
        public byte BLR;
        [FieldOffset(3)]
        public byte BufferSelect;
        [FieldOffset(0x3a)]
        public byte CoarseGain;
        [FieldOffset(0x31)]
        public byte Decimation;
        [FieldOffset(0x49)]
        public byte DetReset;
        [FieldOffset(0x34)]
        public byte FastThreshold;
        [FieldOffset(60)]
        public int FineGain;
        [FieldOffset(50)]
        public byte FlatTop;
        [MarshalAs(UnmanagedType.I4), FieldOffset(0x54)]
        public int HV;
        [FieldOffset(80)]
        public byte HVEnabled;
        [MarshalAs(UnmanagedType.I4), FieldOffset(0x44)]
        public int InputOffset;
        [FieldOffset(0x48)]
        public byte InputPolarity;
        [FieldOffset(2)]
        public byte MCAChannels;
        [FieldOffset(1)]
        public byte MCSTimebase;
        [FieldOffset(90)]
        public byte OutputOffset;
        [MarshalAs(UnmanagedType.I4), FieldOffset(0x40)]
        public int PoleZero;
        [FieldOffset(0x58)]
        public byte PreampPower;
        [MarshalAs(UnmanagedType.I4), FieldOffset(12)]
        public int PresetCount;
        [MarshalAs(UnmanagedType.I4), FieldOffset(8)]
        public int PresetTime;
        [FieldOffset(0x33)]
        public byte PUREnable;
        [FieldOffset(0x37)]
        public byte RTDFast;
        [FieldOffset(0x36)]
        public byte RTDOn;
        [FieldOffset(0x35)]
        public byte RTDSlow;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I4), FieldOffset(0x10)]
        public int[] SCA;
        [FieldOffset(5)]
        public byte SlowThreshold;
        [MarshalAs(UnmanagedType.I4), FieldOffset(0x4c)]
        public int TEC;
        [FieldOffset(0x30)]
        public byte TimeToPeak;
        [FieldOffset(4)]
        public byte TTLGate;
    }

    //[StructLayout(LayoutKind.Explicit, Size = 0x52), Description]
    [StructLayout(LayoutKind.Explicit, Size = 0x58), Description]
    public struct _DPP_STATUS
    {
        [FieldOffset(0x48)]
        public double AccumulationTime;
        [FieldOffset(0x20)]
        public double BoardTemp;
        [FieldOffset(0x19)]
        public byte BootStatus;
        [FieldOffset(0x38)]
        public double FastCount;
        [FieldOffset(8)]
        public double Firmware;
        [FieldOffset(0)]
        public double FPGA;
        [FieldOffset(40)]
        public double HVMonitor;
        [FieldOffset(0x51)]
        public byte MCSDone;
        [FieldOffset(0x52)]
        public byte PresetCountExpired;
        [FieldOffset(0x1a)]
        public byte PwrBtnConfig;
        [FieldOffset(0x10)]
        public double SerialNumber;
        [FieldOffset(0x1c)]
        public byte SetFastThreshDone;
        [FieldOffset(30)]
        public byte SetInputOffsetDone;
        [FieldOffset(0x1d)]
        public byte SetSlowThreshDone;
        [FieldOffset(0x40)]
        public double SlowCount;
        [FieldOffset(0x18)]
        public byte StatDevInd;
        [FieldOffset(80)]
        public byte StatMcaEnabled;
        [FieldOffset(0x1b)]
        public byte SwConfigRcvd;
        [FieldOffset(0x30)]
        public double TECMonitor;
    }

}
