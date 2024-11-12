using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common
{
    public class DataConvertHelper
    {
        //public static void LoadImage(XmlDocument xmlDoc, SpecListEntity specList)
        //{
        //    XmlNode xmlElemRoot = xmlDoc.SelectSingleNode("Spectrum");
        //    XmlNode xmlElem = xmlElemRoot.SelectSingleNode("SampleImage");
        //    if (xmlElem != null)
        //    {
        //        byte[] byteData = Convert.FromBase64String(xmlElem.InnerText);
        //        specList.Image = byteData;
        //    }
        //}

        /// <summary>
        /// 载入样品信息
        /// </summary>
        /// <param name="xmlDoc"></param>
        public static void LoadSampleInfo(XmlDocument xmlDoc, SpecListEntity specList, SpecEntity spec)
        {
            XmlNode xmlElemRoot = xmlDoc.SelectSingleNode("Spectrum");
            XmlNode xmlElem = xmlElemRoot.SelectSingleNode("SampleInfo");
            XmlNodeList nodeList = xmlElem.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                switch (node.Name)
                {
                    case "Supplier":
                        specList.Supplier = node.InnerText;
                        break;
                    case "Weight":
                        specList.Weight = Convert.ToSingle(node.InnerText);
                        break;
                    case "Shape":
                        specList.Shape = node.InnerText;
                        break;
                    case "Operator":
                        specList.Operater = node.InnerText;
                        break;
                    case "SpecDate":
                        specList.SpecDate = DateTime.Parse(node.InnerText);
                        break;
                    case "Caption":
                        specList.SampleName = node.InnerText;
                        break;
                    case "UsedTime":
                        spec.UsedTime = Convert.ToInt32(node.InnerText);
                        spec.SpecTime = Convert.ToDouble(node.InnerText);
                        break;
                    case "SpecSummary": //样品描述信息
                        specList.SpecSummary = Convert.ToString(node.InnerText);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
