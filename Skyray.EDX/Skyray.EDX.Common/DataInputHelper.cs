using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.Configuration;
using System.Windows.Forms;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.App;
using Skyray.EDX.Common.IApplication;

namespace Skyray.EDX.Common
{
    public class DataInputHelper
    {
        public static void DataInput(string workCurvePath, string dataPath, string devicePath, string dataPrefix, ref bool isPerfection)
        {
            DbContext context = DbEntry.GetContext(dataPrefix);
            context.UsingConnection(delegate
            {
                DataBaseHelper.UpdateDatabase(context);
                Device.DeleteAll();
                Skyray.EDXRFLibrary.Condition.DeleteAll();
                InitParameter.DeleteAll();
                DeviceParameter.DeleteAll();
                //SpecList.DeleteAll();
                //Spec.DeleteAll();
                //WorkCurveHelper.DataAccess.d
                WorkCurve.DeleteAll();
                ElementList.DeleteAll();
                CurveElement.DeleteAll();
                ROHSImplement factory = new ROHSImplement();
                factory.LoadDeviceFactory(devicePath, workCurvePath);
                //factory.RepeatCaculateSampleIntensity(dataPath);Rohs导入不重新计算
            });
            isPerfection = true;
        }

        public static void DataInputXfp2(string workCurvePath, string dataPath, string devicePath, string dataPrefix, ref bool isPerfection,string qualitativePath)
        {
            DbContext context = DbEntry.GetContext(dataPrefix);
            context.UsingConnection(delegate
            {
                DataBaseHelper.UpdateDatabase(context);
                Device.DeleteAll();
                Skyray.EDXRFLibrary.Condition.DeleteAll();
                InitParameter.DeleteAll();
                DeviceParameter.DeleteAll();
                //SpecList.DeleteAll();
                //Spec.DeleteAll();
                WorkCurve.DeleteAll();
                ElementList.DeleteAll();
                CurveElement.DeleteAll();
                XRFImplement factory = new XRFImplement();
                XRFImplement.QualitativePath = qualitativePath;
                factory.LoadDeviceFactory(devicePath, workCurvePath);
            });
            isPerfection = true;
        }

        public static void DataInputXRF(string workCurvePath, string dataPath, string devicePath, string dataPrefix, ref bool isPerfection, string deviceType,string otherXyzPath)
        {
            DbContext context = DbEntry.GetContext(dataPrefix);
            context.UsingConnection(delegate
            {
                DataBaseHelper.UpdateDatabase(context);
                Device.DeleteAll();
                Skyray.EDXRFLibrary.Condition.DeleteAll();
                InitParameter.DeleteAll();
                DeviceParameter.DeleteAll();
                //SpecList.DeleteAll();
                //Spec.DeleteAll();
                WorkCurve.DeleteAll();
                ElementList.DeleteAll();
                CurveElement.DeleteAll();
                XRFDelphiImp.DeviceType = deviceType;
                XRFDelphiImp.OtherXYZPath = otherXyzPath;
                XRFDelphiImp factory = new XRFDelphiImp();
                factory.LoadDeviceFactory(devicePath, workCurvePath);
            });
        }

        public static void DataInputThick(string workCurvePath, string dataPath, string devicePath, string dataPrefix, ref bool isPerfection)
        {
            DbContext context = DbEntry.GetContext(dataPrefix);
            context.UsingConnection(delegate
            {
                DataBaseHelper.UpdateDatabase(context);
                Device.DeleteAll();
                Skyray.EDXRFLibrary.Condition.DeleteAll();
                InitParameter.DeleteAll();
                DeviceParameter.DeleteAll();
                //SpecList.DeleteAll();
                //Spec.DeleteAll();
                WorkCurve.DeleteAll();
                ElementList.DeleteAll();
                CurveElement.DeleteAll();
                FPThickImplement factory = new FPThickImplement();
                factory.LoadDeviceFactory(devicePath, workCurvePath);
            });
            isPerfection = true;
        }

        public static void DataInputThick800(string workCurvePath, string dataPath, string devicePath, string dataPrefix, ref bool isPerfection)
        {
            DbContext context = DbEntry.GetContext(dataPrefix);
            context.UsingConnection(delegate
            {
                DataBaseHelper.UpdateDatabase(context);
                Device.DeleteAll();
                Skyray.EDXRFLibrary.Condition.DeleteAll();
                InitParameter.DeleteAll();
                DeviceParameter.DeleteAll();
                //SpecList.DeleteAll();
                //Spec.DeleteAll();
                WorkCurve.DeleteAll();
                ElementList.DeleteAll();
                CurveElement.DeleteAll();
                Thick800AImplement factory = new Thick800AImplement();
                factory.LoadDeviceFactory(devicePath, workCurvePath);
            });
            isPerfection = true;
        }

        public static void DataInputRoHS3(string workCurvePath, string dataPath, string devicePath, string dataPrefix, ref bool isPerfection)
        {
            DbContext context = DbEntry.GetContext(dataPrefix);
            context.UsingConnection(delegate
            {
                DataBaseHelper.UpdateDatabase(context);
                Device.DeleteAll();
                Skyray.EDXRFLibrary.Condition.DeleteAll();
                InitParameter.DeleteAll();
                DeviceParameter.DeleteAll();
                //SpecList.DeleteAll();
                //Spec.DeleteAll();
                WorkCurve.DeleteAll();
                ElementList.DeleteAll();
                CurveElement.DeleteAll();
                RoHS3Implement factory = new RoHS3Implement();
                factory.LoadDeviceFactory(devicePath, workCurvePath);
                //factory.RepeatCaculateSampleIntensity(dataPath);//重新计算的功能屏蔽 2013-03-22
            });
        }

        public static void DataInputEDXRF(string workCurvePath, string dataPath, string dataPrefix, ref bool isPerfection, int maxChannel)
        {
            DbContext context = DbEntry.GetContext(dataPrefix);
            context.UsingConnection(delegate
            {
                DataBaseHelper.UpdateDatabase(context);
                Device device = Device.FindOne(w => w.IsDefaultDevice == true);
                device.SpecLength = (maxChannel == 1024) ? SpecLength.Min : ((maxChannel == 2048) ? SpecLength.Normal : SpecLength.Max);
                device.Save();
                EDXRFImplement factory = new EDXRFImplement();
                factory.LoadDeviceFactory("", workCurvePath);
            });
        }

        public static SpecListEntity CreateNewSpecListFromOld(string fileName)
        {
            string exePath = System.IO.Path.Combine(
    Application.StartupPath, "UpgradeMage.exe");
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(exePath);

            string software = cfa.AppSettings.Settings["UpgradeMageSoftware"].Value;
            string editType = cfa.AppSettings.Settings["UpgradeMageEditionType"].Value;
            if (software == "") return new SpecListEntity();
            IFactory factory=null;
            SpecListEntity returnList = new SpecListEntity();
            switch (software)
            {
                case "EDXRF":
                    factory = new EDXRFImplement();
                    break;
                case "Rohs":
                    if (editType == "0")
                        factory = new ROHSImplement();
                    else
                        factory = new RoHS3Implement();
                    break;
                case "Thick":
                    if (editType == "0")
                        factory = new FPThickImplement();
                    else
                        factory = new Thick800AImplement();
                    break;
                case "XRF":
                    if (editType == "0")
                        factory = new XRFImplement();
                    else
                        factory = new XRFDelphiImp();
                    break;
            }
            returnList = factory.LoadSpecFactory(fileName);
            return returnList;
        }

      
    }
}
