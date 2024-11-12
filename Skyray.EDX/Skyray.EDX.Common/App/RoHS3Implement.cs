using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using System.IO;
using System.Configuration;
using Skyray.EDX.Common;
using System.Xml;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using Skyray.EDX.Common.DelphiConvert;
using Skyray.EDX.Common.Library;

namespace Skyray.EDX.Common.App
{
    public class RoHS3Implement : IFactory
    {
        private ImputData data = new ImputData();
        public ROHS3 rohsFile = new ROHS3();

        private long subByte = 4;
        #region IFactory 成员

        public void LoadDeviceFactory(string directoryDev, string curvedir)
        {
            if (string.IsNullOrEmpty(directoryDev))
                return;
            string exePath = System.IO.Path.Combine(
    Environment.CurrentDirectory, "Skyray.RoHS.exe");
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(exePath);
            cfa.AppSettings.Settings["SmoothTimes"].Value = "3";
            cfa.Save();
            try
            {
                DirectoryInfo sampleDir = new DirectoryInfo(Environment.CurrentDirectory + "\\Image\\SampleImage\\EDXR");
                if (sampleDir.Exists)
                    sampleDir.Delete(true);
                sampleDir.Create();
            }
            catch { }
            Device device = Device.New.Init("", "", DllType.DLL4, 1, 1, "", SpecLength.Normal, 1, 0, 2, 120, 120, 120, 120, 120, 120, 50, 600, 3, 120, 0, 0, "x");
            device.IsDefaultDevice = true;
            device.Name = "Default";
            device.DeviceID = "";
            device.SpecLength = SpecLength.Normal;
            device.VoltageScaleFactor = 1;
            device.CurrentScaleFactor = 1;
            rohsFile.ReadFileCollimator(directoryDev, device);
            rohsFile.ReadFileFilter(directoryDev, device);
            rohsFile.ReadFileRaytube(directoryDev, device);
            rohsFile.ReadFileDetector(directoryDev, device);
            rohsFile.IsExistMoveAis(directoryDev, device);
            StringBuilder temp = new StringBuilder(255);
            int size = 255;
            TxtFile.GetPrivateProfileString("Device", "Interface", "1", temp, size, directoryDev + "\\Configure\\Parameter.ini");
            if (Convert.ToInt32(temp.ToString()) == 1)
            {
                device.ComType = ComType.USB;
                TxtFile.GetPrivateProfileString("System", "bDp5", "0", temp, size, directoryDev + "\\Configure\\Parameter.ini");
                if (Convert.ToInt32(temp.ToString()) == 1)
                    device.IsDP5 = true;
                else
                    device.IsDP5 = false;
                device.Dp5Version = Dp5Version.Dp5_CommonUsb;
                TxtFile.GetPrivateProfileString("System", "DLLType", "4", temp, size, directoryDev + "\\Configure\\Parameter.ini");
                string strDllIndex = string.Empty;
                if (Convert.ToInt32(temp.ToString()) == 3)
                {
                    device.PortType = DllType.DLL3;
                    strDllIndex = "3";
                }
                else
                {
                    device.PortType = DllType.DLL4;
                    strDllIndex = "4";
                }
                #region 是否加密
                DirectoryInfo info = new DirectoryInfo(directoryDev);
                FileInfo[] filesDll = info.GetFiles("TRUSBDev"+strDllIndex+".dll");
                if (filesDll != null && filesDll.Length > 0)
                {
                    DirectoryInfo otherDir = new DirectoryInfo(Environment.CurrentDirectory);
                    FileInfo[] otherDirDll = otherDir.GetFiles("TRUSBDev.Dll" + strDllIndex + ".Usb*.dll");
                    if (otherDirDll != null && otherDirDll.Length > 0)
                    {
                        FileInfo tempInfo =  otherDirDll.ToList().Find(w => Math.Abs((w.Length - filesDll[0].Length)/1024) < subByte);
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
            }
            else
                device.ComType = ComType.Parallel;
             device.Save();
             DirectoryInfo curveInfo = new DirectoryInfo(curvedir);
             FileInfo[] curveFiles = curveInfo.GetFiles("*.xml");
             if (curveFiles == null || curveFiles.Length == 0)
                 return;
             foreach (FileInfo tempCurve in curveFiles)
             {
                 Console.Write("RoHS曲线" + tempCurve.Name + "导入！\r\n");
                 LoadCurveFactory(device, tempCurve);
             }
        }

        private void LoadCurveFactory(Device device, FileInfo curveFile)
        {
            //构造曲线从xml文件中
            data.CurveImport(curveFile.FullName,device,true,true,string.Empty,string.Empty);
        }

        public void LoadCurveFactory(string directoryDev, List<Skyray.EDXRFLibrary.Condition> conditions)
        {

        }

        public void RepeatCaculateSampleIntensity(string dataPath)
        {
            List<SpecListEntity> list = new List<SpecListEntity>();
            DirectoryInfo info = new DirectoryInfo(dataPath);
            List<FileInfo> files = FileFoundHelper.GetDirAllFiles(dataPath, "spe");
            var listCuve = WorkCurve.FindAll().ToList().FindAll(delegate(WorkCurve tt) { return tt.Condition.Device.IsDefaultDevice == true; });

            Helper.ExperienceCoeffs.LoadExperienceCoeffs(AppDomain.CurrentDomain.BaseDirectory + "Parameter.xml");
            string exePath = System.IO.Path.Combine(
      Environment.CurrentDirectory, "Skyray.RoHS.exe");
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(exePath);
            try
            {
                Skyray.EDXRFLibrary.SpecHelper.SmoothTimes = int.Parse(cfa.AppSettings.Settings["SmoothTimes"].Value);
            }
            catch { };
            foreach (var temp in listCuve)
            {
                if (temp.ElementList != null && temp.ElementList.Items.Count > 0)
                {
                    StandSamplesIntensity(temp, files);
                }
                temp.Save();
            }
        }

        private void StandSamplesIntensity(WorkCurve curve, List<FileInfo> files)
        {
            if (curve.ElementList != null && curve.ElementList.Items.Count > 0)
            {
                foreach (CurveElement ttElement in curve.ElementList.Items.ToList())
                {
                    ttElement.Samples.ToList().ForEach(w =>
                    {
                        FileInfo tt = files.Find(t => Path.GetFileNameWithoutExtension(t.Name) == w.SampleName);
                        if (tt != null)
                        {
                            curve.CaculateIntensity(LoadSpecFactory(tt.FullName));
                            w.X = ttElement.Intensity.ToString();
                        }
                    });
                }
            }
        }

        public SpecListEntity LoadSpecFactory(string fileName)
        {
            FileStream specStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader read = new BinaryReader(specStream);
            try
            {
                SpecListEntity specList = new SpecListEntity();
                SpecEntity spec = new SpecEntity();
                spec.IsSmooth = true;
                specList.Name = Path.GetFileNameWithoutExtension(fileName);
                specList.SampleName = specList.Name;
                spec.TubVoltage = read.ReadUInt16();
                spec.TubCurrent = read.ReadUInt16();
                byte[] spedate = new byte[8];
                read.Read(spedate, 0, 8);
                specList.SpecDate = DateTime.Now;//DateTime.FromBinary(BitConverter.ToInt64(spedate,0)).ToString("yyyy-MM-dd");
                spec.UsedTime = read.ReadUInt16();
                spec.SpecTime = spec.UsedTime;
                int FilterId = read.ReadInt32();
                char[] strLotNo = read.ReadChars(51);
                char[] strSup = read.ReadChars(51);
                specList.Supplier = string.Empty; //new string(strSup);
                byte[] data = read.ReadBytes(2048 * 4);
                for (int i = 0; i < 2048; i++)
                {
                    spec.SpecData += BitConverter.ToInt32(data, i * 4) + ",";
                }
                specList.Operater = "";
                spec.Name = specList.Name;
                Device device = Device.FindOne(w => w.IsDefaultDevice == true);
                if (device != null && device.Conditions.Count > 0)
                    specList.DemarcateEnergys = Default.ConvertFormOldToNew(device.Conditions[0].DemarcateEnergys, device.SpecLength);
                else
                {
                    if (WorkCurveHelper.DeviceCurrent==null)
                    {
                        Msg.Show(Info.NotFindDevice);
                        return null;
                    }
                    device = WorkCurveHelper.DeviceCurrent;
                    List<DemarcateEnergy> listDemarcate = new List<DemarcateEnergy>();
                    listDemarcate.Add(Default.GetDemarcateEnergyAg(device.SpecLength));
                    listDemarcate.Add(Default.GetDemarcateEnergyCu(device.SpecLength));
                    specList.DemarcateEnergys = Default.ConvertFormOldToNew(listDemarcate, device.SpecLength);
                }
                specList.InitParam = Default.GetInitParameter(device.SpecLength).ConvertToNewEntity();
                specList.Specs = new SpecEntity[1];
                specList.Specs[0] = spec;
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    FileInfo[] jpgInfo = file.Directory.GetFiles(specList.Name + ".jpeg");
                    if (jpgInfo != null && jpgInfo.Length > 0)
                        jpgInfo[0].CopyTo(WorkCurveHelper.SaveSamplePath, true);
                }
                return specList;
            }
            catch (Exception)
            {
                Msg.Show(Info.ErrFileFormat);
            }
            finally
            {
                read.Close();
            }
            return null;
        }

        #endregion
    }
}
