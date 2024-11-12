using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Skyray.EDXRFLibrary
{
    public class ExperienceCoeffs
    {
        public double PbCoeffA = 1;
        public double PbCoeffB = 0;
        public double CdCoeffA = 1;
        public double CdCoeffB = 0;
        public double PbLimit = 1000;
        public static double ErrCoeffA = 0.11;
        public static double ErrCoeffB = 30;
        public static double ContRatio = 35;

        public double CdSnCoeff = -0.0004;
        public double PbabCoeff = 0.7;
        public int SnLeft = 0;
        public int SnRight = 0;
        public double HalfWidth;
        public int PeakChannel;
        public bool IsShowND = false;

        //加载是否优化
        //public bool IGetGradeContent = false;

        /// <summary>
        /// 加载强度计算的经验系数
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadExperienceCoeffs(string fileName)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNode xmlTemp = doc.SelectSingleNode("Parameter/System/PbCoeffA");
            PbCoeffA = Convert.ToDouble(xmlTemp.InnerText);
            xmlTemp = doc.SelectSingleNode("Parameter/System/PbCoeffB");
            PbCoeffB = Convert.ToDouble(xmlTemp.InnerText);
            xmlTemp = doc.SelectSingleNode("Parameter/System/CdCoeffA");
            CdCoeffA = Convert.ToDouble(xmlTemp.InnerText);
            xmlTemp = doc.SelectSingleNode("Parameter/System/CdCoeffB");
            CdCoeffB = Convert.ToDouble(xmlTemp.InnerText);
            xmlTemp = doc.SelectSingleNode("Parameter/System/PbLimit");
            PbLimit = Convert.ToDouble(xmlTemp.InnerText);
            xmlTemp = doc.SelectSingleNode("Parameter/System/CdSnCoeff");
            CdSnCoeff = Convert.ToDouble(xmlTemp.InnerText);
            xmlTemp = doc.SelectSingleNode("Parameter/System/PbabCoeff");
            PbabCoeff = Convert.ToDouble(xmlTemp.InnerText);

            xmlTemp = doc.SelectSingleNode("Parameter/System/HalfWidth");
            HalfWidth = Convert.ToDouble(xmlTemp.InnerText);
            xmlTemp = doc.SelectSingleNode("Parameter/System/PeakChannel");
            PeakChannel = Convert.ToInt32(xmlTemp.InnerText);
            XmlNodeList list = doc.SelectNodes("Parameter/Element");
            foreach (XmlNode doNode in list)
            {
                if (doNode.SelectSingleNode("ElementName").InnerText.Equals("Sn"))
                {
                    SnLeft = int.Parse(doNode.SelectSingleNode("Left").InnerText);
                    SnRight = int.Parse(doNode.SelectSingleNode("Right").InnerText);
                }
            }
            
        }

        //public void LoadGradeCoeffs(string fileName)
        //{
        //    try
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(fileName);
        //        XmlNode xmlTemp = doc.SelectSingleNode("Parameter/IGetGradeContent");
        //        IGetGradeContent = Convert.ToBoolean(Convert.ToInt32(xmlTemp.InnerText));
        //    }
        //    catch 
        //    {

        //        IGetGradeContent = false;
        //    }
        //}
    }
}
