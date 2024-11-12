using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.EDX.Common.IApplication;

namespace Skyray.EDX.Common.App
{
    public class EDXRFImplement : IFactory
    {

        public static string usbType;

        #region IFactory 成员

        public void LoadDeviceFactory(string directoryDev, string curvedir)
        {
            Device device = Device.FindOne(w => w.IsDefaultDevice == true);
            DirectoryInfo curveInfo = new DirectoryInfo(curvedir);
            FileInfo[] curveFiles = curveInfo.GetFiles("*.xml");
            if (curveFiles == null || curveFiles.Length == 0)
                return;
            foreach (FileInfo tempCurve in curveFiles)
            {
                Console.Write("贵金属工作曲线" + tempCurve.Name + "导入！\r\n");
                LoadCurveFactory(device, tempCurve);
            }
        }

        ImputData upgradeImport = new ImputData();

        private void LoadCurveFactory(Device device, FileInfo curveFile)
        {
            //构造曲线从xml文件中
            upgradeImport.CurveImport(curveFile.FullName, device, true, true, string.Empty, string.Empty);
        }

        public void LoadCurveFactory(string directoryDev, List<Skyray.EDXRFLibrary.Condition> conditions)
        {


        }

        public Skyray.EDXRFLibrary.Spectrum.SpecListEntity LoadSpecFactory(string fileName)
        {
            return upgradeImport.SampleImport(fileName);
        }

        public void RepeatCaculateSampleIntensity(string dataPath)
        {

        }
        #endregion
    }
}
