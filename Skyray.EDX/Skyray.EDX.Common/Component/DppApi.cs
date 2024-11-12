using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.EDX.Common.Component
{
    public  static class DppApi
    {
        [DllImport("DppApi.dll")]
        public static extern  void CloseDppApi(IntPtr objptr);

        [DllImport("DppApi.dll")]
        public static extern  void CloseUSBDevice(IntPtr objptr);

        [DllImport("DppApi.dll")]
        public static extern  int Get80MHzMode(IntPtr objptr);

        [DllImport("DppApi.dll")]
        public static extern  void GetConfigFromDpp(IntPtr objptr);

        [DllImport("DppApi.dll")]
        public static extern  void GetConfigFromFile(IntPtr objptr, string file, int lenFile, byte deviceType);

        [DllImport("DppApi.dll")]
        public static extern  void GetConfigString(IntPtr objptr, byte[] Config, int lenOfString);

        [DllImport("DppApi.dll")]
        public static extern  int GetDppData(IntPtr objptr, int[] DataBuffer);

        [DllImport("DppApi.dll")]
        public static extern  void GetStatusBuffer(IntPtr objptr, bool isBufferA, int[] Status);

        [DllImport("DppApi.dll")]
        public static extern  void GetStatusString(IntPtr objptr, bool b, byte[] status, int lenOfString);

        [DllImport("DppApi.dll")]
        public static extern  void GetStatusStringFromBuffer(IntPtr objptr, bool isBufferA, [MarshalAs(UnmanagedType.LPStr)] string pszStatus, int cSize, int[] Status);

        [DllImport("DppApi.dll")]
        public static extern  void GetStatusStruct(IntPtr objptr, bool b,ref _DPP_STATUS status);

        [DllImport("DppApi.dll")]
        public static extern  void GetStatusStructFromBuffer(IntPtr objptr, bool isBufferA, ref _DPP_STATUS status, int[] Status);


        [DllImport("DppApi.dll")]
        public static extern  void GetTempConfigSettings(IntPtr objptr, ref _DPP_CONFIG_SETTINGS CfgSet, bool currentupdate);

        [DllImport("DppApi.dll")]
        public static extern  void GetTempConfigString(IntPtr objptr, byte[] s, int cSize);

        [DllImport("DppApi.dll")]
        public static extern  int GetUSBDppDeviceInfo(IntPtr objptr, int device, int[] lSerialNumber);

        [DllImport("DppApi.dll")]
        public static extern  int MonitorUSBDppDevices(IntPtr objptr);

        [DllImport("DppApi.dll")]
        public static extern IntPtr OpenDppApi();

        [DllImport("DppApi.dll")]
        public static extern  int OpenUSBDevice(IntPtr objptr);

        [DllImport("DppApi.dll")]
        public static extern  void PauseDppData(IntPtr objptr, bool b);

        [DllImport("DppApi.dll")]
        public static extern  void SaveConfigToFile(IntPtr objptr, string pszFilename, int cSize);

        [DllImport("DppApi.dll")]
        public static extern  void SendConfigToBuffer(IntPtr objptr);

        [DllImport("DppApi.dll")]
        public static extern  void SendConfigToDpp(IntPtr objptr);

        [DllImport("DppApi.dll")]
        public static extern  void SetFPGAClockDefault(IntPtr objptr, bool b80MHzMode, byte DPPDeviceType);

        [DllImport("DppApi.dll")]
        public static extern  void SetTempConfigSettings(IntPtr objptr, ref _DPP_CONFIG_SETTINGS CfgSet, bool CurrentUpdate);

        [DllImport("DppApi.dll")]
        public static extern void ClearDppData(IntPtr objptr, bool isBufferA);

    }
}
