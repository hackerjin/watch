using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using System.Xml;
using Lephone.Data;
using Lephone.Data.Definition;
using System.IO;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDX.Common.IApplication;
using Skyray.EDX.Common.Library;

namespace Skyray.EDX.Common.CSharpConvert
{
    public class XRFReadFile : IDevice, ISpectrums, IWorkCurve
    {
        #region IDevice Members 设备信息

        public void ReadFileCollimator(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode rootNode = doc.SelectSingleNode("Motor");
            XmlNode node = rootNode.SelectSingleNode("DirectionFlag");
            device.CollimatorElectricalDirect = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("ID");
            device.CollimatorElectricalCode = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("DefSpeed");
            if (node != null)
            {
                device.CollimatorSpeed = Convert.ToInt32(node.InnerText);
            }
            node = rootNode.SelectSingleNode("TargetCount");
            int targetCount = Convert.ToInt32(node.InnerText);
            if (targetCount > 0)
            {
                int[] Target = new int[targetCount];
                for (int i = 0; i < targetCount; ++i)
                {
                    Collimator filter = Collimator.New;
                    filter.Num = i;
                    node = rootNode.SelectSingleNode("Target" + i.ToString());
                    filter.Step = Convert.ToInt32(node.InnerText);
                    device.Collimators.Add(filter);
                }
            }
        }

        public void ReadFileFilter(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
           
            XmlNode rootNode = doc.SelectSingleNode("Motor");
            XmlNode node = rootNode.SelectSingleNode("DirectionFlag");
            device.FilterElectricalDirect = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("ID");
            device.FilterElectricalCode = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("DefSpeed");
            if (node != null)
            {
                device.FilterSpeed = Convert.ToInt32(node.InnerText);
            }
            node = rootNode.SelectSingleNode("TargetCount");
            int targetCount = Convert.ToInt32(node.InnerText);
            if (targetCount > 0)
            {
                int[] Target = new int[targetCount];
                for (int i = 0; i < targetCount; ++i)
                {
                    Filter filter = Filter.New;
                    filter.Num = i;
                    node = rootNode.SelectSingleNode("Target" + i.ToString());
                    filter.Step = Convert.ToInt32(node.InnerText);
                    device.Filter.Add(filter);
                }
            }
        }

        public void ReadFileDetector(string filePaht, Device device)
        {
            if (!System.IO.File.Exists(filePaht))
            {
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePaht);
            Detector detector = Detector.New;
            XmlNode xmlRootElem = xmlDoc.SelectSingleNode("Device");
            XmlNode xmlElem = xmlRootElem.SelectSingleNode("Detector");
            XmlNode xmlElemSub = xmlElem.SelectSingleNode("DetectorType");
            detector.Type = (DetectorType)Enum.Parse(typeof(DetectorType), xmlElemSub.InnerText);
            xmlElemSub = xmlElem.SelectSingleNode("Energy");
            detector.Energy = Convert.ToDouble(xmlElemSub.InnerText);
            xmlElemSub = xmlElem.SelectSingleNode("Fwhm");
            detector.Fwhm = Convert.ToDouble(xmlElemSub.InnerText);
            device.Detector = detector;


        }

        public void ReadFileChamber(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode rootNode = doc.SelectSingleNode("Motor");
            XmlNode node = rootNode.SelectSingleNode("DirectionFlag");
            device.ChamberElectricalDirect = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("ID");
            device.ChamberElectricalCode = Convert.ToInt32(node.InnerText);
            node = rootNode.SelectSingleNode("DefSpeed");
            if (node != null)
            {
                device.ChamberSpeed = Convert.ToInt32(node.InnerText);
            }
            node = rootNode.SelectSingleNode("TargetCount");
            int targetCount = Convert.ToInt32(node.InnerText);
            if (targetCount > 0)
            {
                int[] Target = new int[targetCount];
                for (int i = 0; i < targetCount; ++i)
                {
                    Chamber filter = Chamber.New;
                    filter.Num = i;
                    node = rootNode.SelectSingleNode("Target" + i.ToString());
                    filter.Step = Convert.ToInt32(node.InnerText);
                    filter.StepCoef1 = 0;
                    filter.StepCoef2 = 0;
                    device.Chamber.Add(filter);
                }
            }
        }

        public void ReadFileRaytube(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            Tubes tubes = Tubes.New;
            XmlNode xmlRootElem = xmlDoc.SelectSingleNode("XRayTub");
            XmlNode xmlElem = xmlRootElem.SelectSingleNode("TargetAtomicNumber");
            tubes.AtomNum = Convert.ToInt32(xmlElem.InnerText);
            xmlElem = xmlRootElem.SelectSingleNode("TargetTakeOffAngle");
            tubes.Angel = Convert.ToInt32(xmlElem.InnerText);
            xmlElem = xmlRootElem.SelectSingleNode("WindowThickness");
            tubes.Thickness = Convert.ToDouble(xmlElem.InnerText);

            xmlElem = xmlRootElem.SelectSingleNode("WindowFormula");
            tubes.Material = xmlElem.InnerText;

            xmlElem = xmlRootElem.SelectSingleNode("IncidentAngle");
            tubes.Incident = Convert.ToInt32(xmlElem.InnerText);

            xmlElem = xmlRootElem.SelectSingleNode("EmergentAngle");
            tubes.Exit = Convert.ToInt32(xmlElem.InnerText);
            device.Tubes = tubes;
        }

        public void IsExistMoveAis(string filePath, Device device)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode rootNode = doc.SelectSingleNode("Motor");

            XmlNode node = rootNode.SelectSingleNode("Exist");
            bool exists = Convert.ToBoolean(node.InnerText);
            if (exists)
            {
                device.HasMotorZ = exists;
                node = rootNode.SelectSingleNode("ID");
                device.MotorZCode = Convert.ToInt32(node.InnerText);

                node = rootNode.SelectSingleNode("DirectionFlag");
                device.MotorZDirect = Convert.ToInt32(node.InnerText);

                node = rootNode.SelectSingleNode("MaxStep");
                device.MotorZMaxStep = Convert.ToInt32(node.InnerText);

                node = rootNode.SelectSingleNode("DefSpeed");
                if (node != null)
                {
                    device.MotorZSpeed = Convert.ToInt32(node.InnerText);
                }
            }
        }

        #endregion

        #region ISpectrums Members 谱信息

        public void GetSpectrum(FileInfo filePath, SpecType type, SpecListEntity specList, List<Device> deviceList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath.FullName);
            DeviceParameterEntity deviceParams = new DeviceParameterEntity();
            string deviceName = ReadHelper.LoadDeviceInfo(xmlDoc, deviceParams);
            SpecEntity spec = new SpecEntity();
            ReadHelper.LoadSampleInfo(xmlDoc, specList,spec);
           
            XmlNode xmlElemRoot = xmlDoc.SelectSingleNode("Spectrum");
            XmlNode xmlElem = xmlElemRoot.SelectSingleNode("Data");
            char[] sep = new char[] { ',' };
            string[] dataStr = xmlElem.InnerText.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            string str = string.Empty;
            for (int i = 0; i < dataStr.Length; ++i)
            {
                str += Convert.ToInt32(dataStr[i])+",";
            }
            spec.SpecData = str;
            spec.TubCurrent = deviceParams.TubCurrent;
            spec.TubVoltage = deviceParams.TubVoltage;
            spec.Name = filePath.Name;
            
            specList.Name = filePath.Name;
            specList.SpecType = type;
            if (deviceList == null || deviceList.Count == 0)
                return;
            Device device = deviceList.Find(w => w.Name == deviceName);
            spec.RemarkInfo = "";
            spec.DeviceParameter = deviceParams;
            specList.Specs = new SpecEntity[1];
            specList.Specs[0] = spec;
            specList.InitParam = device.Conditions[0].InitParam.ConvertToNewEntity();
            WorkCurveHelper.DataAccess.Save(specList);
            ReadHelper.LoadImage(xmlDoc, specList,1);
        }

        #endregion

        #region IWorkCurve Members 工作曲线信息

        public ElementList GetIntestedElementList(string filePath,string workCurveDir)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            return ReadHelper.loadElementList(filePath, workCurveDir);
        }

        public List<Skyray.EDXRFLibrary.Condition> ReadFileCondition(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            return ReadHelper.LoadCondition(filePath);
        }

        public Compounds ReadFileCompounds(string filePath)
        {
            throw new NotImplementedException();
        }

        public void ReadElementsSample(string filePath, CurveElement elements)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            ReadHelper.LoadElementSamples(filePath, elements);
        }

        public List<DemarcateEnergy> GetConditionDemarcate(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            return ReadHelper.LoadDemarcateEnergy(filePath);
        }

        public QualeElement GetQualeElement(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            return ReadHelper.LoadQualeElement(filePath);
        }

        public void CreateCurve(WorkCurve curve, string filePath,List<Skyray.EDXRFLibrary.Condition> conditions)
        {
            if (curve == null || !System.IO.File.Exists(filePath))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode rootNode = doc.SelectSingleNode("WorkCurve");

            XmlNode node = rootNode.SelectSingleNode("Type");
            curve.CalcType = (node.InnerText == "EC" ? CalcType.EC : CalcType.FP);

            node = rootNode.SelectSingleNode("ConditionID");
            if (node.InnerText == "Intellectualized")
                node.InnerText = Info.IntelligentCondition;
            //else
            //{
            //    int i = 0;
            //}
            curve.Condition = conditions.Find(w => w.Name == node.InnerText);
            curve.FuncType = FuncType.XRF;
            curve.ConditionName = curve.Condition.Name;
        }
        #endregion


       

    }
}
