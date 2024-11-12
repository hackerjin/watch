using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;

namespace Skyray.EDX.Common.UIHelper
{
    public class XmlHelper
    {
        public static void GenerateXmlFile( string xmlFilePath)
        {
            string strFilePath = xmlFilePath;
            if (!File.Exists(strFilePath))
            {
                XmlDocument doc = new XmlDocument();
                XmlElement rootElement = doc.CreateElement("UI");
                doc.AppendChild(rootElement);

                XmlElement FormXRF = AddChildNode(doc, rootElement, "FormXRF");
                AddChildNode(doc, FormXRF, "Style", "Office2007Blue");
                AddChildNode(doc, AddChildNode(doc, FormXRF, "containerObject2"), "Size", "240,695");
                AddChildNode(doc, AddChildNode(doc, FormXRF, "containerObject6"), "Size", "240,196");
                AddChildNode(doc, AddChildNode(doc, FormXRF, "containerObject8"), "Size", "762,300");

                XmlElement FormRohs = AddChildNode(doc, rootElement, "FormRohs");
                AddChildNode(doc, FormRohs, "Style", "Office2007Blue");
                AddChildNode(doc, AddChildNode(doc, FormRohs, "containerObject2"), "Size", "270,693");
                AddChildNode(doc, AddChildNode(doc, FormRohs, "containerObject5"), "Size", "270,109");
                AddChildNode(doc, AddChildNode(doc, FormRohs, "containerObject13"), "Size", "727,289");
                AddChildNode(doc, AddChildNode(doc, FormRohs, "containerObject9"), "Size", "270,237");

                XmlElement FrmThick = AddChildNode(doc, rootElement, "FrmThick");
                AddChildNode(doc, FrmThick, "Style", "Office2007Blue");
                AddChildNode(doc, AddChildNode(doc, FrmThick, "containerObject2"), "Size", "240,693");
                AddChildNode(doc, AddChildNode(doc, FrmThick, "containerObject6"), "Size", "270,196");
                AddChildNode(doc, AddChildNode(doc, FrmThick, "containerObject8"), "Size", "760,300");

                XmlElement Frm3000 = AddChildNode(doc, rootElement, "Frm3000");
                AddChildNode(doc, Frm3000, "Style", "Office2007Blue");
                AddChildNode(doc, AddChildNode(doc, Frm3000, "containerObjectLeft"), "Size", "213,647");
                AddChildNode(doc, AddChildNode(doc, Frm3000, "containerObjectButtonsAndDrop"), "Size", "213,112");
                AddChildNode(doc, AddChildNode(doc, Frm3000, "containerObjectSpecAndTubeStatus"), "Size", "213,196");
                AddChildNode(doc, AddChildNode(doc, Frm3000, "containerObjectCurve"), "Size", "213,72");
                AddChildNode(doc, AddChildNode(doc, Frm3000, "containerObjectEnergy"), "Size", "213,70");
                AddChildNode(doc, AddChildNode(doc, Frm3000, "containerObjectButtons"), "Size", "112,112");

                doc.Save(strFilePath);
            }
        }
        private static XmlElement AddChildNode(XmlDocument doc, XmlNode parentNode, string childNodeName)
        {
            XmlElement childNode = doc.CreateElement(childNodeName);
            parentNode.AppendChild(childNode);
            return childNode;
        }
        private static XmlElement AddChildNode(XmlDocument doc, XmlNode parentNode, string childNodeName, string childNodeValue)
        {
            XmlElement element = AddChildNode(doc, parentNode, childNodeName);
            element.InnerText = childNodeValue;
            return element;
        }

        public static void RecordContainerValue(XmlDocument doc,string parentForm,string containerNodeName,ContainerObject container)
        {
            XmlNode size = GetContainerSizeNode(doc, parentForm, containerNodeName);
            if (container != null && size != null)
                size.InnerText = container.Width.ToString() + "," + container.Height.ToString();
        }
        public static void RecordContainerValue(XmlDocument doc, string parentForm, string containerNodeName,System.Windows.Forms.SplitContainer container)
        {
            XmlNode size = GetContainerSizeNode(doc, parentForm, containerNodeName);
            if (container != null && size != null)
                size.InnerText = container.SplitterDistance.ToString();
        }
        public static void RecordStyleValue(XmlDocument doc,string parentForm,string styleText)
        {
            XmlNode style = GetStyleNode(doc, parentForm);
            style.InnerText = styleText;
        }
        private static XmlNode GetContainerSizeNode(XmlDocument doc, string parentForm, string containerNodeName)
        {
            XmlNode firstLevel = doc.FirstChild;
            XmlNode xmlForm = firstLevel.SelectSingleNode(parentForm);
            if (xmlForm == null)
                return null;
            XmlNode xmlContainer = xmlForm.SelectSingleNode(containerNodeName);
            if (xmlContainer == null)
                return null;
            XmlNode size = xmlContainer.SelectSingleNode("Size");
            return size;
        }
        private static XmlNode GetStyleNode(XmlDocument doc, string parentForm)
        {
            XmlNode firstLevel = doc.FirstChild;
            XmlNode xmlForm = firstLevel.SelectSingleNode(parentForm);
            XmlNode style = xmlForm.SelectSingleNode("Style");
            return style;
        }
        public static void SetContainerValue(XmlDocument doc,string parentForm,string containerNodeName,ContainerObject container)
        {
            XmlNode size = GetContainerSizeNode(doc, parentForm, containerNodeName);
            if (container !=null && size != null)
                container.Size = ConvertStringToSize(size.InnerText);
        }
        public static void SetContainerValue(XmlDocument doc, string parentForm, string containerNodeName, System.Windows.Forms.SplitContainer container)
        {
            XmlNode size = GetContainerSizeNode(doc, parentForm, containerNodeName);
            if (container != null && size != null)
                container.SplitterDistance = Convert.ToInt32(size.InnerText);
        }
        public static Skyray.Controls.Style SetStyleValue(XmlDocument doc, string parentForm)
        {
            XmlNode styleNode = GetStyleNode(doc, parentForm);
            Skyray.Controls.Style style = (Skyray.Controls.Style)Enum.Parse(typeof(Skyray.Controls.Style), styleNode.InnerText);
            return style;
        }
        private static Size ConvertStringToSize(string value)
        {
            int[] ints = SplitStringToIntArray(value);
            Size size = new Size(ints[0], ints[1]);
            return size;
        }
        private static int[] SplitStringToIntArray(string value)
        {
            string[] str =  value.Split(',');
            int[] ints = new int[str.Length];
            for (int i = 0; i < str.Length;i++ )
            {
                //int.TryParse(str[i], out ints[i]);
                ints[i] = int.Parse(str[i]);
            }
            return ints;
        }

        public static MeasureTimeType GetMeasureTimeType()
        {
            System.Threading.Thread.Sleep(100);
            try
            {
                XDocument doc = XDocument.Load(Environment.CurrentDirectory + "\\AppParams.xml");
                return GetMeasureTimeType(doc);
            }
            catch
            {
                return default(MeasureTimeType);
            }
        }
        public static MeasureTimeType GetMeasureTimeType(XDocument doc)
        {
            MeasureTimeType mBack = MeasureTimeType.Normal;
            var MesureTime = doc.Element("application").Element("TestParams").Element("MeasureTime");
            if (MesureTime != null)
            {
                if (MesureTime.Attribute("MeasureTimeType") == null)
                {
                    MesureTime.Add(new XAttribute("MeasureTimeType", MeasureTimeType.Normal.ConvertToType<int>().ToString()));
                    doc.Save(Environment.CurrentDirectory + "\\AppParams.xml");
                }
                mBack = MesureTime.Attribute("MeasureTimeType").Value.ConvertToType<int>().ConvertToType(mBack);
            }
            return mBack;
        }
    }
    public enum MeasureTimeType
    {
        Normal,
        AutoIncrease,
        TimeMeasure,
    }
}
