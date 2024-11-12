using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Skyray.EDXRFLibrary;
using System.IO;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using Skyray.EDX.Common.Library;

namespace Skyray.EDX.Common.App
{
    public class XRFDelphiImp : IFactory
    {
        Skyray.EDX.Common.DelphiConvert.XRF rohsFile = new Skyray.EDX.Common.DelphiConvert.XRF();
        private long subByte = 4;
        public static string DeviceType = string.Empty;
        public static string OtherXYZPath = string.Empty;

        #region IFactory 成员

        public void LoadDeviceFactory(string directoryDev, string curvedir)
        {
            if (string.IsNullOrEmpty(directoryDev))
                return;
            try
            {
                DirectoryInfo sampleDir = new DirectoryInfo(Environment.CurrentDirectory + "\\Image\\SampleImage\\EDXX");
                if (sampleDir.Exists)
                    sampleDir.Delete(true);
            }
            catch { }
            Device device = Device.New.Init("", "", DllType.DLL4, 1, 1, "", SpecLength.Normal, 1, 0, 2, 120, 120, 120, 120, 120, 120, 50, 600, 3, 120,0,0,"x");
            device.IsDefaultDevice = true;
            device.Name = "Default";
            device.DeviceID = "";
            device.SpecLength = SpecLength.Normal;
            device.VoltageScaleFactor = 1;
            device.CurrentScaleFactor = 1;
            StringBuilder temp = new StringBuilder(255);
            int size = 255;
            TxtFile.GetPrivateProfileString("System", "SmoothTimes", "0", temp, size, directoryDev + "\\parameter.ini");
            string exePath = System.IO.Path.Combine(
   Environment.CurrentDirectory, "Skyray.XRF.exe");
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(exePath);
            cfa.AppSettings.Settings["SmoothTimes"].Value = temp.ToString();
            cfa.Save();
            string strParamsPath = string.Empty;
            string strDllIndex = string.Empty;
            if (DeviceType == "1")
            {
                TxtFile.GetPrivateProfileString("System", "DeviceType", "0", temp, size, directoryDev + "\\parameter.ini");
                int deviceType = Convert.ToInt32(temp.ToString());
                switch (deviceType)
                {
                    case 0:
                        strDllIndex = "3";
                        device.PortType = DllType.DLL3;
                        strParamsPath = directoryDev + "\\XRFMotorParameter.ini";
                        break;
                    case 1:
                        device.PortType = DllType.DLL3;
                        strDllIndex = "3";
                        strParamsPath = directoryDev + "\\EDX1800BMotorParameter.ini";
                        break;
                    case 2:
                        strDllIndex = "4";
                        device.PortType = DllType.DLL4;
                        strParamsPath = directoryDev + "\\EDX3600MotorParameter.ini";
                        break;
                    case 3:
                        strDllIndex = "4";
                        device.PortType = DllType.DLL4;
                        strParamsPath = directoryDev + "\\EDX3600HMotorParameter.ini";
                        break;
                    case 4:
                        strDllIndex = "4";
                        device.PortType = DllType.DLL4;
                        strParamsPath = directoryDev + "\\EDX3600BMotorParameter.ini";
                        break;
                    case 5:
                        strDllIndex = "4";
                        device.PortType = DllType.DLL4;
                        strParamsPath = directoryDev + "\\EDX3200SMotorParameter.ini";
                        break;
                    case 6:
                        strDllIndex = "4";
                        device.PortType = DllType.DLL4;
                        strParamsPath = directoryDev + "\\EDX1800BMotorParameter.ini";
                        break;
                    case 7:
                        strDllIndex = "4";
                        device.PortType = DllType.DLL4;
                        strParamsPath = directoryDev + "\\XRFMotorParameter.ini";
                        break;
                    default:
                        strParamsPath = directoryDev + "\\XRFMotorParameter.ini";
                        strDllIndex = "4";
                        device.PortType = DllType.DLL4;
                        break;
                }
                rohsFile.ReadFileCollimator(strParamsPath, device);
                rohsFile.ReadFileFilter(strParamsPath, device);
                rohsFile.IsExistMoveAis(directoryDev + "\\MotorParameter.ini", device);
            }
            else
            {
                rohsFile.ReadFileCollimator(directoryDev + "\\MotorParameter.ini", device);
                rohsFile.ReadFileFilter(directoryDev + "\\MotorParameter.ini", device);
                rohsFile.IsExistMoveAis(OtherXYZPath + "\\MotorParameter.ini", device);
            }
            rohsFile.ReadFileRaytube(directoryDev, device);
            rohsFile.ReadFileDetector(directoryDev, device);
            TxtFile.GetPrivateProfileString("System", "Processor", "0", temp, size, directoryDev + "\\Parameter.ini");
            if (Convert.ToInt32(temp.ToString()) == 0)
            {
                device.ComType = ComType.USB;
            }
            else
            {
                device.ComType = ComType.USB;
                device.IsDP5 = true;
                device.Dp5Version = Dp5Version.Dp5_CommonUsb;
            }
            #region 是否加密
            DirectoryInfo info = new DirectoryInfo(directoryDev);
            FileInfo[] filesDll = info.GetFiles("TRUSBDev" + strDllIndex + ".dll");
            if (filesDll != null && filesDll.Length > 0)
            {
                DirectoryInfo otherDir = new DirectoryInfo(Environment.CurrentDirectory);
                FileInfo[] otherDirDll = otherDir.GetFiles("TRUSBDev.Dll" + strDllIndex + ".Usb*.dll");
                if (otherDirDll != null && otherDirDll.Length > 0)
                {
                    FileInfo tempInfo = otherDirDll.ToList().Find(w => Math.Abs((w.Length - filesDll[0].Length) / 1024) < subByte);
                    if (tempInfo != null)
                    {
                        string splitUsb = tempInfo.Name.Replace("TRUSBDev.Dll" + strDllIndex + ".Usb", "").Replace(".dll", "");
                        string[] usbTypes = splitUsb.Split('.');
                        if (usbTypes != null && usbTypes.Length == 2)
                        {
                            device.UsbVersion = (usbTypes[0] == "1") ? UsbVersion.Usb1 : UsbVersion.Usb2;
                            device.IsPassward = (usbTypes[1] == "1") ? false : true;
                        }
                    }
                }
            }
            #endregion
            device.Save();
            DirectoryInfo curveInfo = new DirectoryInfo(curvedir);
            FileInfo[] curveFiles = curveInfo.GetFiles("*.xml");
            if (curveFiles == null || curveFiles.Length == 0)
                return;
            foreach (FileInfo tempCurve in curveFiles)
            {
                Console.Write("XRF工作曲线" + tempCurve.Name + "导入！\r\n");
                LoadCurveFactory(device, tempCurve);
            }
           
        }

        private void LoadCurveFactory(Device device, FileInfo curveFile)
        {
            //构造曲线从xml文件中
            upgradeImport.CurveImport(curveFile.FullName, device, false, true, string.Empty, string.Empty);
        }

        ImputData upgradeImport = new ImputData();
        public void LoadCurveFactory(string directoryDev, List<Skyray.EDXRFLibrary.Condition> conditions)
        {
        }
        public SpecListEntity LoadSpecFactory(string fileName)
        {
            SpecListEntity entity = new SpecListEntity();
            if (DephiHelper.GetFromXRFSpec(new FileInfo(fileName), entity))
                return entity;
            if (DephiHelper.GetFromXRFCopperSpec(new FileInfo(fileName), entity))
                return entity;
            return null;
        }

        public void RepeatCaculateSampleIntensity(string dataPath)
        {

        }
        #endregion
    }
}
