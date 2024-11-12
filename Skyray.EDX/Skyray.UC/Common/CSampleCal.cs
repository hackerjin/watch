using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Skyray.UC
{
    public class CSampleCalData
    {
        public string ElemCaption; //元素
        public float ElemContent; //含量 
        public float Error;//误差 %单位
        public CSampleCalData(string element, float content, float error)
        {
            ElemCaption = element;
            ElemContent = content;
            Error = error;
        }
    }
    public class CSampleCal
    {
        public List<CSampleCalData> ListSampleDatas = null;
        public string ControlSampleName = string.Empty;
        public bool LoadDatasFromFile(string strFileName)
        {
            if (!File.Exists(strFileName)) return false;
            if (ListSampleDatas == null) ListSampleDatas = new List<CSampleCalData>();
            ListSampleDatas.Clear();
            try
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load(strFileName);
                System.Xml.XmlNode nodeTemp = xmlDoc.SelectSingleNode("ConSampleSetting/ControlSampleName");
                ControlSampleName = nodeTemp != null ? nodeTemp.InnerText : string.Empty;
                nodeTemp = xmlDoc.SelectSingleNode("ConSampleSetting/ControlSpNode");
                if (nodeTemp != null)
                {
                    foreach (System.Xml.XmlNode node in nodeTemp.ChildNodes)
                    {
                        CSampleCalData cscd = new CSampleCalData("",0,0);
                        cscd.ElemCaption = node.Attributes["ElemCaption"].Value;
                        cscd.ElemContent = Convert.ToSingle(node.Attributes["ElemContent"].Value);
                        cscd.Error = Convert.ToSingle(node.Attributes["ElemError"].Value);
                        ListSampleDatas.Add(cscd);
                    }
 
                }
                xmlDoc.Load(strFileName);
            }
            catch
            {
                ListSampleDatas = null;
                ControlSampleName = string.Empty;
                return false;
            }
            return true;
        }


        public bool SaveDatasToFile(string strFileName)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.XmlDeclaration newDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(newDec);
            System.Xml.XmlElement ParInfo1 = xmlDoc.CreateElement("ConSampleSetting");
            xmlDoc.AppendChild(ParInfo1);
            System.Xml.XmlElement ParInfo = xmlDoc.CreateElement("ControlSampleName");
            ParInfo.InnerText = ControlSampleName;
            ParInfo1.AppendChild(ParInfo);

            System.Xml.XmlElement ParInfoNode = xmlDoc.CreateElement("ControlSpNode");
            ParInfo1.AppendChild(ParInfoNode);
            foreach (CSampleCalData cscd in ListSampleDatas)
            {
                System.Xml.XmlElement nodeInfoTemp = xmlDoc.CreateElement("SpNode");
                ParInfoNode.AppendChild(nodeInfoTemp);
                 System.Xml.XmlAttribute xab = xmlDoc.CreateAttribute("ElemCaption");
                xab.Value = cscd.ElemCaption;
                nodeInfoTemp.Attributes.Append(xab);

                xab = xmlDoc.CreateAttribute("ElemContent");
                xab.Value = cscd.ElemContent.ToString();
                nodeInfoTemp.Attributes.Append(xab);

                xab = xmlDoc.CreateAttribute("ElemError");
                xab.Value = cscd.Error.ToString();
                nodeInfoTemp.Attributes.Append(xab);

            }
            xmlDoc.Save(strFileName);
            return true;
        }

        public bool IsCSampleCalEnabled
        {
            get { return ListSampleDatas != null && ControlSampleName != null && ListSampleDatas.Count > 0; }
        }

    }
}
