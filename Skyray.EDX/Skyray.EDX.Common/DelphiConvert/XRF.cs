using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.IApplication;

namespace Skyray.EDX.Common.DelphiConvert
{
    public class XRF : IDevice
    {
        #region IDevice 成员

        public void ReadFileCollimator(string filePath, Skyray.EDXRFLibrary.Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            StringBuilder temp = new StringBuilder(255);
            int size = 255;
            TxtFile.GetPrivateProfileString("CollMotor", "Exist", "0", temp, size, filePath);
            if (Convert.ToInt32(temp.ToString()) == 1)
                device.HasCollimator = true;
            TxtFile.GetPrivateProfileString("CollMotor", "MotorID", "1", temp, size, filePath);
            device.CollimatorElectricalCode = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString("CollMotor", "MotorDir", "0", temp, size, filePath);
            device.CollimatorElectricalDirect = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString("CollMotor", "Speed", "0", temp, size, filePath);
            device.CollimatorSpeed = Convert.ToInt32(temp.ToString());
            int colliCount = 0;
            for (int i = 0; i < 8; i++)
            {
                TxtFile.GetPrivateProfileString("CollMotor", "Tag" + (i + 1), "0", temp, size, filePath);
                if (Convert.ToInt32(temp.ToString()) == 0)
                    continue;
                Collimator collimator = Collimator.New;
                collimator.Num = i + 1;
                collimator.Step = Convert.ToInt32(temp.ToString());
                device.Collimators.Add(collimator);
                colliCount++;
            }
            device.CollimatorMaxNum = colliCount;
        }

        public void ReadFileFilter(string filePath, Skyray.EDXRFLibrary.Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            StringBuilder temp = new StringBuilder(255);
            int size = 255;
            TxtFile.GetPrivateProfileString("FilterMotor", "Exist", "0", temp, size, filePath);
            if (Convert.ToInt32(temp.ToString()) == 1)
                device.HasFilter = true;
            TxtFile.GetPrivateProfileString("FilterMotor", "MotorID", "0", temp, size, filePath);
            device.FilterElectricalCode = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString("FilterMotor", "MotorDir", "0", temp, size, filePath);
            device.FilterElectricalDirect = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString("FilterMotor", "Speed", "0", temp, size, filePath);
            device.FilterSpeed = Convert.ToInt32(temp.ToString());
            int filterCount = 0;
            for (int i = 0; i < 8; i++)
            {
                TxtFile.GetPrivateProfileString("FilterMotor", "Tag" + (i + 1), "0", temp, size, filePath);
                if (Convert.ToInt32(temp.ToString()) == 0)
                    continue;
                Filter FilterMotor = Filter.New;
                FilterMotor.Num = i + 1;
                FilterMotor.Step = Convert.ToInt32(temp.ToString());
                device.Filter.Add(FilterMotor);
                filterCount++;
            }
            device.FilterMaxNum = filterCount;
        }

        public void ReadFileDetector(string filePaht, Skyray.EDXRFLibrary.Device device)
        {
            device.Detector = Detector.New.Init(Skyray.EDXRFLibrary.DetectorType.Si, 5.895, 170);
        }

        public void ReadFileChamber(string filePath, Skyray.EDXRFLibrary.Device device)
        {
           
        }

        public void ReadFileRaytube(string filePath, Skyray.EDXRFLibrary.Device device)
        {
            device.Tubes = Tubes.New.Init(74, 19, 1.9, "SiO2", 40, 35, 14);
        }

        public void IsExistMoveAis(string filePath, Skyray.EDXRFLibrary.Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            StringBuilder temp = new StringBuilder(255);
            int size = 255;
            //string caption = "XMotor";
            //TxtFile.GetPrivateProfileString(caption, "Exist", "", temp, size, filePath);
            //device.HasMotorX = (Convert.ToInt32(temp.ToString()) == 1 ? true : false);
            //TxtFile.GetPrivateProfileString(caption, "MotorID", "", temp, size, filePath);
            //device.MotorXCode = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "MotorDir", "", temp, size, filePath);
            //device.MotorXDirect = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "Speed", "", temp, size, filePath);
            //device.MotorXSpeed = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "MaxStep", "", temp, size, filePath);
            //device.MotorXMaxStep = Convert.ToInt32(temp.ToString()); ;

            //caption = "YMotor";
            //TxtFile.GetPrivateProfileString(caption, "Exist", "", temp, size, filePath);
            //device.HasMotorY = (Convert.ToInt32(temp.ToString()) == 1 ? true : false);
            //TxtFile.GetPrivateProfileString(caption, "MotorID", "", temp, size, filePath);
            //device.MotorYCode = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "MotorDir", "", temp, size, filePath);
            //device.MotorYDirect = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "Speed", "", temp, size, filePath);
            //device.MotorYSpeed = Convert.ToInt32(temp.ToString());
            //TxtFile.GetPrivateProfileString(caption, "MaxStep", "", temp, size, filePath);
            //device.MotorXMaxStep = Convert.ToInt32(temp.ToString()); ;

            string caption = "ZMotor";
            TxtFile.GetPrivateProfileString(caption, "Exist", "", temp, size, filePath);
            device.HasMotorZ = (Convert.ToInt32(temp.ToString()) == 1 ? true : false);
            TxtFile.GetPrivateProfileString(caption, "MotorID", "", temp, size, filePath);
            device.MotorZCode = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString(caption, "MotorDir", "", temp, size, filePath);
            device.MotorZDirect = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString(caption, "Speed", "", temp, size, filePath);
            device.MotorZSpeed = Convert.ToInt32(temp.ToString());
            TxtFile.GetPrivateProfileString(caption, "MaxStep", "", temp, size, filePath);
            device.MotorXMaxStep = Convert.ToInt32(temp.ToString()); ;
        }

        #endregion
    }
}
