using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;


namespace Skyray.EDX.Common.Library
{
    public class DephiHelper
    {
        public static unsafe string GetSpecInfo(string filePath,out int UsedTime)
        {
            UsedTime = 0;
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            BinaryReader FS = new BinaryReader(File.Open(filePath, FileMode.Open));
            int[] data = new int[2048];
            DeviceParameter deviceParams = DeviceParameter.New;
            string str = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                int temp = FS.ReadInt32();
                str += temp + ",";
            }
            OldDeviceParam oldParms = new OldDeviceParam();
            byte[] bytesParams = FS.ReadBytes(48);
            fixed (byte* bp = &bytesParams[0])
            {
                oldParms = (OldDeviceParam)Marshal.PtrToStructure(new IntPtr(bp), typeof(OldDeviceParam));
            }
            byte[] b = oldParms.MinValue;
            byte[] bc = new byte[8];
            bc[0] = 0;
            bc[1] = 0;
            bc[2] = 0;
            bc[3] = 0;
            bc[4] = b[0];
            bc[5] = b[1];
            bc[6] = b[2];
            bc[7] = b[3];
            deviceParams.MinRate = BitConverter.ToDouble(bc, 0);
            deviceParams.MaxRate = oldParms.MaxValue;
            deviceParams.PrecTime = oldParms.PrecTime;
            deviceParams.TubVoltage = oldParms.TubVoltage;
            deviceParams.TubCurrent = oldParms.TubCurrent;
            deviceParams.BeginChann = oldParms.BeginChann;
            deviceParams.EndChann = oldParms.EndChann;
            deviceParams.FilterIdx = oldParms.FilterIdx;
            deviceParams.CollimatorIdx = oldParms.CollimatorIdx;
            deviceParams.IsAdjustRate = oldParms.AdjustRated;
            deviceParams.IsVacuum = oldParms.Vacuumed;
            UsedTime = deviceParams.PrecTime;
            FS.Close();
            return str;
        }

        public static unsafe bool GetDeviceParamsFromSpec(FileInfo filePath, SpecListEntity specList)
        {
            bool result = true;
            if (!System.IO.File.Exists(filePath.FullName))
            {
                return false;
            }
            BinaryReader FS = new BinaryReader(File.Open(filePath.FullName, FileMode.Open));
            try
            {
                int[] data = new int[2048];
                SpecEntity spec = new SpecEntity();
                DeviceParameterEntity deviceParams = new DeviceParameterEntity();
                string str = string.Empty;
                for (int i = 0; i < data.Length; i++)
                {
                    int temp = FS.ReadInt32();
                    str += temp + ",";
                }
                spec.SpecData = str;
                OldDeviceParam oldParms = new OldDeviceParam();
                byte[] bytesParams = FS.ReadBytes(48);
                fixed (byte* bp = &bytesParams[0])
                {
                    oldParms = (OldDeviceParam)Marshal.PtrToStructure(new IntPtr(bp), typeof(OldDeviceParam));
                }
                byte[] b = oldParms.MinValue;
                byte[] bc = new byte[8];
                bc[0] = 0;
                bc[1] = 0;
                bc[2] = 0;
                bc[3] = 0;
                bc[4] = b[0];
                bc[5] = b[1];
                bc[6] = b[2];
                bc[7] = b[3];
                deviceParams.MinRate = BitConverter.ToDouble(bc, 0);
                deviceParams.MaxRate = oldParms.MaxValue;
                deviceParams.PrecTime = oldParms.PrecTime;
                deviceParams.TubVoltage = oldParms.TubVoltage;
                deviceParams.TubCurrent = oldParms.TubCurrent;
                deviceParams.BeginChann = oldParms.BeginChann;
                deviceParams.EndChann = oldParms.EndChann;
                deviceParams.FilterIdx = oldParms.FilterIdx;
                deviceParams.CollimatorIdx = oldParms.CollimatorIdx;
                deviceParams.IsAdjustRate = oldParms.AdjustRated;
                deviceParams.IsVacuum = oldParms.Vacuumed;

                spec.TubCurrent = oldParms.TubCurrent;
                spec.TubVoltage = oldParms.TubVoltage;
                spec.Name = filePath.Name;

                byte[] specParams = FS.ReadBytes(1612);
                OldSpecParam oldSpec = new OldSpecParam();
                fixed (byte* bp = &specParams[0])
                {
                    oldSpec = (OldSpecParam)Marshal.PtrToStructure(new IntPtr(bp), typeof(OldSpecParam));
                }
                specList.SampleName = oldSpec.Caption;
                specList.Operater = oldSpec.Operator;
                specList.SpecDate = string.IsNullOrEmpty(oldSpec.SpecDate)?DateTime.Now:DateTime.Parse(oldSpec.SpecDate);
                specList.Weight = string.IsNullOrEmpty(oldSpec.Weight) ? 0 : Convert.ToDouble(oldSpec.Weight);
                specList.SpecSummary = oldSpec.Remark;
                specList.Supplier = oldSpec.Supplier;
                spec.UsedTime = oldSpec.UsedTime;
                spec.SpecTime = spec.UsedTime;
                spec.IsSmooth = false;
                Condition conditon = null;
                Device device = Device.FindOne(w => w.IsDefaultDevice == true);
                if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition != null)
                {
                    device = WorkCurveHelper.DeviceCurrent;
                    conditon = WorkCurveHelper.WorkCurveCurrent.Condition;
                }
                else
                    conditon = device.Conditions[0];
                spec.DeviceParameter =  conditon.DeviceParamList[0].ConvertFrom();
                spec.RemarkInfo = "";
                specList.Name = filePath.Name;
                specList.SpecType = SpecType.StandSpec;
                specList.InitParam = Default.GetInitParameter(device.SpecLength).ConvertToNewEntity();
                specList.DemarcateEnergys = Default.ConvertFormOldToNew(new List<DemarcateEnergy>(),device.SpecLength);
                specList.Specs = new SpecEntity[1];
                specList.Specs[0] = spec;
                if (filePath.Exists)
                {
                    FileInfo[] jpgInfo = filePath.Directory.GetFiles(specList.Name + ".jpeg");
                    if (jpgInfo != null && jpgInfo.Length > 0)
                        jpgInfo[0].CopyTo(WorkCurveHelper.SaveSamplePath, true);
                }
            }
            catch { result = false; }
            finally { FS.Close(); }
            return result;
        }

         public static unsafe bool GetFromXRFSpec(FileInfo filePath, SpecListEntity specList)
        {
            bool result = true;
            if (!System.IO.File.Exists(filePath.FullName))
            {
                return false;
            }
            BinaryReader FS = new BinaryReader(File.Open(filePath.FullName, FileMode.Open));
            try
            {
                int[] data = new int[2048];
                DeviceParameterEntity deviceParams = new DeviceParameterEntity();
                string str = string.Empty;
                for (int i = 0; i < data.Length; i++)
                {
                    int temp = FS.ReadInt32();
                    str += temp + ",";
                }
                XRFCommonDeviceParam oldParms = new XRFCommonDeviceParam();
                byte[] bytesParams = FS.ReadBytes(80);
                fixed (byte* bp = &bytesParams[0])
                {
                    oldParms = (XRFCommonDeviceParam)Marshal.PtrToStructure(new IntPtr(bp), typeof(XRFCommonDeviceParam));
                }
                byte[] b = oldParms.MinValue;
                byte[] bc = new byte[8];
                bc[0] = 0;
                bc[1] = 0;
                bc[2] = 0;
                bc[3] = 0;
                bc[4] = b[0];
                bc[5] = b[1];
                bc[6] = b[2];
                bc[7] = b[3];
                deviceParams.MinRate = BitConverter.ToDouble(bc, 0);
                deviceParams.MaxRate = oldParms.MaxValue;
                deviceParams.PrecTime = oldParms.PrecTime;
                deviceParams.TubVoltage = (int)oldParms.TubVoltage;
                deviceParams.TubCurrent = oldParms.TubCurrent;
                deviceParams.BeginChann = oldParms.BeginChann;
                deviceParams.EndChann = oldParms.EndChann;
                deviceParams.FilterIdx = oldParms.FilterIdx;
                deviceParams.CollimatorIdx = oldParms.CollimatorIdx;
                deviceParams.IsAdjustRate = oldParms.AdjustRated;
                deviceParams.IsVacuum = oldParms.Vacuumed;

                Condition conditon = null;
                Device device = Device.FindOne(w => w.IsDefaultDevice == true);
                if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition != null)
                {
                    device = WorkCurveHelper.DeviceCurrent;
                    conditon = WorkCurveHelper.WorkCurveCurrent.Condition;
                }
                else
                    conditon = device.Conditions[0];
                //spec.TubCurrent = oldParms.TubCurrent;
                //spec.TubVoltage = (int)oldParms.TubVoltage;
                //spec.Name = filePath.Name;

                byte[] specParams = FS.ReadBytes(1612);
                XRFCommonSpecParam oldSpec = new XRFCommonSpecParam();
                fixed (byte* bp = &specParams[0])
                {
                    oldSpec = (XRFCommonSpecParam)Marshal.PtrToStructure(new IntPtr(bp), typeof(XRFCommonSpecParam));
                }
                specList.SampleName = oldSpec.Caption;
                specList.Operater = oldSpec.Operator;
                specList.SpecDate = string.IsNullOrEmpty(oldSpec.SpecDate) ? DateTime.Now : DateTime.Parse(oldSpec.SpecDate);
                specList.Weight = string.IsNullOrEmpty(oldSpec.Weight) ? 0 : Convert.ToDouble(oldSpec.Weight);
                specList.SpecSummary = oldSpec.Remark;
                specList.Supplier = oldSpec.Supplier;
                //spec.UsedTime = oldSpec.UsedTime;
                //spec.SpecTime = spec.UsedTime;
                //spec.DeviceParameter = deviceParams;
                //spec.RemarkInfo = "";
                specList.Name = filePath.Name;
                specList.SpecType = SpecType.StandSpec;
                specList.InitParam = Default.GetInitParameter(device.SpecLength).ConvertToNewEntity();
                specList.DemarcateEnergys = Default.ConvertFormOldToNew(new List<DemarcateEnergy>(), device.SpecLength);
                int[] ints = Helper.ToInts(str);
               
                if (conditon.DeviceParamList != null && conditon.DeviceParamList.Count > 0)
                {
                    specList.Specs = new SpecEntity[conditon.DeviceParamList.Count];
                    int k = 0;
                    foreach (DeviceParameter tep in conditon.DeviceParamList)
                    {
                        int[] deviceParamsData = new int[(int)device.SpecLength];
                        Array.Copy(ints, tep.BeginChann, deviceParamsData, tep.BeginChann, tep.EndChann - tep.BeginChann);
                        for (int i = 0; i < deviceParamsData.Length; i++)
                            deviceParamsData[i] = (int)(deviceParamsData[i] * (float)tep.PrecTime / conditon.DeviceParamList[0].PrecTime);
                        SpecEntity spec = new SpecEntity(specList.SampleName, Helper.ToStrs(deviceParamsData), tep.PrecTime, tep.PrecTime, tep.TubVoltage, tep.TubCurrent, "");
                        spec.DeviceParameter = new DeviceParameterEntity(tep.Name, tep.PrecTime, tep.TubCurrent, tep.TubVoltage, tep.FilterIdx, tep.CollimatorIdx, tep.TargetIdx, tep.IsVacuum, tep.VacuumTime, tep.IsVacuumDegree, tep.VacuumDegree, tep.IsAdjustRate, tep.MinRate, tep.MaxRate, tep.BeginChann, tep.EndChann, tep.IsDistrubAlert, tep.IsPeakFloat, tep.PeakFloatLeft, tep.PeakFloatRight, tep.PeakFloatChannel, tep.PeakFloatError, tep.PeakCheckTime, tep.TargetMode, tep.CurrentRate);
                        spec.IsSmooth = false;
                        specList.Specs[k] = spec;
                        k++;
                    }
                }
                if (filePath.Exists)
                {
                    FileInfo[] jpgInfo = filePath.Directory.GetFiles(specList.Name + ".jpeg");
                    if (jpgInfo != null && jpgInfo.Length > 0)
                        jpgInfo[0].CopyTo(WorkCurveHelper.SaveSamplePath, true);
                }
            }
            catch { result = false; }
            finally 
            { 
                FS.Close(); 
            }
            return result;
        }

         public static unsafe bool GetFromXRFCopperSpec(FileInfo filePath, SpecListEntity specList)
         {
             bool result = true;
             if (!System.IO.File.Exists(filePath.FullName))
             {
                 return false;
             }
             BinaryReader FS = new BinaryReader(File.Open(filePath.FullName, FileMode.Open));
             try
             {
                 int[] data = new int[2048];
                 //SpecEntity spec = new SpecEntity();
                 DeviceParameterEntity deviceParams = new DeviceParameterEntity();
                 string str = string.Empty;
                 for (int i = 0; i < data.Length; i++)
                 {
                     int temp = FS.ReadInt32();
                     str += temp + ",";
                 }
                
                 XRFCopperDeviceParam oldParms = new XRFCopperDeviceParam();
                 byte[] bytesParams = FS.ReadBytes(56);
                 fixed (byte* bp = &bytesParams[0])
                 {
                     oldParms = (XRFCopperDeviceParam)Marshal.PtrToStructure(new IntPtr(bp), typeof(XRFCopperDeviceParam));
                 }
                 byte[] b = oldParms.MinValue;
                 byte[] bc = new byte[8];
                 bc[0] = 0;
                 bc[1] = 0;
                 bc[2] = 0;
                 bc[3] = 0;
                 bc[4] = b[0];
                 bc[5] = b[1];
                 bc[6] = b[2];
                 bc[7] = b[3];
                 deviceParams.MinRate = BitConverter.ToDouble(bc, 0);
                 deviceParams.MaxRate = oldParms.MaxValue;
                 deviceParams.PrecTime = oldParms.PrecTime;
                 deviceParams.TubVoltage = (int)oldParms.TubVoltage;
                 deviceParams.TubCurrent = oldParms.TubCurrent;
                 deviceParams.BeginChann = oldParms.BeginChann;
                 deviceParams.EndChann = oldParms.EndChann;
                 deviceParams.FilterIdx = oldParms.FilterIdx;
                 deviceParams.CollimatorIdx = oldParms.CollimatorIdx;
                 deviceParams.IsAdjustRate = oldParms.AdjustRated;
                 deviceParams.IsVacuum = oldParms.Vacuumed;

                 //spec.TubCurrent = oldParms.TubCurrent;
                 //spec.TubVoltage = (int)oldParms.TubVoltage;
                 //spec.Name = filePath.Name;
                 Condition conditon = null;
                 Device device = Device.FindOne(w => w.IsDefaultDevice == true);
                 if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition != null)
                 {
                     device = WorkCurveHelper.DeviceCurrent;
                     conditon = WorkCurveHelper.WorkCurveCurrent.Condition;
                 }
                 else
                     conditon = device.Conditions[0];

                 byte[] specParams = FS.ReadBytes(1612);
                 XRFCopperSpecParam oldSpec = new XRFCopperSpecParam();
                 fixed (byte* bp = &specParams[0])
                 {
                     oldSpec = (XRFCopperSpecParam)Marshal.PtrToStructure(new IntPtr(bp), typeof(XRFCopperSpecParam));
                 }
                 specList.SampleName = oldSpec.Caption;
                 specList.Operater = oldSpec.Operator;
                 specList.SpecDate = string.IsNullOrEmpty(oldSpec.SpecDate) ? DateTime.Now : DateTime.Parse(oldSpec.SpecDate);
                 specList.Weight = string.IsNullOrEmpty(oldSpec.Weight) ? 0 : Convert.ToDouble(oldSpec.Weight);
                 specList.SpecSummary = oldSpec.Remark;
                 specList.Supplier = oldSpec.Supplier;
                 //spec.UsedTime = oldSpec.UsedTime;
                 //spec.SpecTime = spec.UsedTime;
                 //spec.DeviceParameter = deviceParams;
                 //spec.RemarkInfo = "";
                 specList.Name = filePath.Name;
                 specList.SpecType = SpecType.StandSpec;
                 specList.InitParam = Default.GetInitParameter(device.SpecLength).ConvertToNewEntity();
                 specList.DemarcateEnergys = Default.ConvertFormOldToNew(new List<DemarcateEnergy>(), device.SpecLength);
                 int[] ints = Helper.ToInts(str);
               
                 if (conditon.DeviceParamList != null && conditon.DeviceParamList.Count > 0)
                 {
                     specList.Specs = new SpecEntity[conditon.DeviceParamList.Count];
                     int k = 0;
                     foreach (DeviceParameter tep in conditon.DeviceParamList)
                     {
                         int[] deviceParamsData = new int[(int)device.SpecLength];
                         Array.Copy(ints, tep.BeginChann, deviceParamsData, tep.BeginChann, tep.EndChann - tep.BeginChann);
                         for (int i = 0; i < deviceParamsData.Length; i++)
                             deviceParamsData[i] = (int)(deviceParamsData[i] * (float)tep.PrecTime / conditon.DeviceParamList[0].PrecTime);
                         SpecEntity spec = new SpecEntity(specList.SampleName, Helper.ToStrs(deviceParamsData), tep.PrecTime, tep.PrecTime, tep.TubVoltage, tep.TubCurrent, "");
                         spec.DeviceParameter = new DeviceParameterEntity(tep.Name, tep.PrecTime, tep.TubCurrent, tep.TubVoltage, tep.FilterIdx, tep.CollimatorIdx, tep.TargetIdx, tep.IsVacuum, tep.VacuumTime, tep.IsVacuumDegree, tep.VacuumDegree, tep.IsAdjustRate, tep.MinRate, tep.MaxRate, tep.BeginChann, tep.EndChann, tep.IsDistrubAlert, tep.IsPeakFloat, tep.PeakFloatLeft, tep.PeakFloatRight, tep.PeakFloatChannel, tep.PeakFloatError, tep.PeakCheckTime, tep.TargetMode, tep.CurrentRate);
                         spec.IsSmooth = false;
                         specList.Specs[k] = spec;
                         k++;
                     }
                 }
                 if (filePath.Exists)
                 {
                     FileInfo[] jpgInfo = filePath.Directory.GetFiles(specList.Name + ".jpeg");
                     if (jpgInfo != null && jpgInfo.Length > 0)
                         jpgInfo[0].CopyTo(WorkCurveHelper.SaveSamplePath, true);
                 }

             }
             catch { result = false; }
             finally { FS.Close(); }
             return result;
         }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OldDeviceParam
    {
        [MarshalAs(UnmanagedType.I4)]
        public int PrecTime;                //预置测量时间
        [MarshalAs(UnmanagedType.I4)]
        public int TubCurrent;              //管流
        [MarshalAs(UnmanagedType.I4)]
        public int TubVoltage;              //管压
        [MarshalAs(UnmanagedType.I4)]
        public int FilterIdx;              //滤光片
        [MarshalAs(UnmanagedType.I4)]
        public int CollimatorIdx;  //准直器
        [MarshalAs(UnmanagedType.Bool)]
        public bool Vacuumed;       //是否抽真空
        [MarshalAs(UnmanagedType.Bool)]
        public bool AdjustRated;    //是否调节计数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] MinValue;
        [MarshalAs(UnmanagedType.R8)]
        public double MaxValue;

        [MarshalAs(UnmanagedType.I4)]
        public int BeginChann;             //起始道址
        [MarshalAs(UnmanagedType.I4)]
        public int EndChann;       //结束道址
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct OldSpecParam
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string cap;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Caption;                //预置测量时间

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string supp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Supplier;   //管流

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string wei;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Weight;              //管压

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string bat;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Batch;              //滤光片

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string oper;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Operator;  //准直器

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string sepcD;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string SpecDate;       //是否抽真空

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string specT;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string SpecTime;    //是否调节计数

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string remar;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Remark;                 //最小计数率
        [MarshalAs(UnmanagedType.I4)]
        public int UsedTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XRFCommonDeviceParam
    {
        [MarshalAs(UnmanagedType.I4)]
        public int PrecTime;                //预置测量时间
        [MarshalAs(UnmanagedType.I4)]
        public int TubCurrent;              //管流
        [MarshalAs(UnmanagedType.I4)]
        public int FilterIdx;              //滤光片
        [MarshalAs(UnmanagedType.I4)]
        public int Scale;    
        [MarshalAs(UnmanagedType.I4)]
        public int CollimatorIdx;  //准直器

        [MarshalAs(UnmanagedType.R8)]
        public double TubVoltage;             
        [MarshalAs(UnmanagedType.Bool)]
        public bool Vacuumed;       //是否抽真空
        [MarshalAs(UnmanagedType.Bool)]
        public bool Magneted;
        [MarshalAs(UnmanagedType.I4)]
        public int PumpTime;
        [MarshalAs(UnmanagedType.R8)]
        public double PumpVacuum;
        [MarshalAs(UnmanagedType.I4)]
        public int PreVacuumMode;
        [MarshalAs(UnmanagedType.Bool)]
        public bool AdjustRated;    //是否调节计数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] MinValue;
        [MarshalAs(UnmanagedType.R8)]
        public double MaxValue;
        [MarshalAs(UnmanagedType.I4)]
        public int BeginChann;             //起始道址
        [MarshalAs(UnmanagedType.I4)]
        public int EndChann;       //结束道址
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XRFCopperDeviceParam
    {
        [MarshalAs(UnmanagedType.I4)]
        public int PrecTime;                //预置测量时间
        [MarshalAs(UnmanagedType.I4)]
        public int TubCurrent;              //管流
        [MarshalAs(UnmanagedType.I4)]
        public int TubVoltage;              //管压
        [MarshalAs(UnmanagedType.I4)]
        public int FilterIdx;              //滤光片
        [MarshalAs(UnmanagedType.I4)]
        public int CollimatorIdx;  //准直器
        [MarshalAs(UnmanagedType.Bool)]
        public bool Vacuumed;       //是否抽真空
        [MarshalAs(UnmanagedType.I4)]
        public int PumpTime;     
        [MarshalAs(UnmanagedType.Bool)]
        public bool AdjustRated;    //是否调节计数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] MinValue;
        [MarshalAs(UnmanagedType.R8)]
        public double MaxValue;

        [MarshalAs(UnmanagedType.I4)]
        public int BeginChann;             //起始道址
        [MarshalAs(UnmanagedType.I4)]
        public int EndChann;       //结束道址
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct XRFCommonSpecParam
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string cap;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Caption;                //预置测量时间

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string supp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Supplier;   //管流

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string wei;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Weight;              //管压

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string shp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Shape;              //滤光片

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string oper;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Operator;  //准直器

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string sepcD;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string SpecDate;       //是否抽真空

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string specT;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string SpecTime;    //是否调节计数

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string remar;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Remark;                 //最小计数率
        [MarshalAs(UnmanagedType.I4)]
        public int UsedTime;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct XRFCopperSpecParam
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string cap;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Caption;                //预置测量时间

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string supp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Supplier;   //管流

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string wei;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Weight;              //管压

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string shp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Shape;              //滤光片

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string oper;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Operator;  //准直器

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string sepcD;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string SpecDate;       //是否抽真空

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string specT;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string SpecTime;    //是否调节计数

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string remar;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string Remark;                 //最小计数率
        [MarshalAs(UnmanagedType.I4)]
        public int UsedTime;
    }
}
