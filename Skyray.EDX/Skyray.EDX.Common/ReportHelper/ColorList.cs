using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

namespace Skyray.EDX.Common.ReportHelper
{
    /// <summary>
    /// 元素颜色
    /// </summary>
    public class ElemColor
    {
        private string elemName;///<元素名
        private Color color;///<元素颜色

        public ElemColor()
        {
            elemName = "";
            color = Color.Blue;
        }

        public ElemColor(string name, Color color)
        {
            this.elemName = name;
            this.color = color;
        }
        public string Name
        {
            get
            {
                return elemName;
            }
            set
            {
                elemName = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

    }

    /// <summary>
    /// 元素颜色列表
    /// </summary>
    public class ColorList : List<ElemColor>
    {
        /// <summary>
        /// 自定义索引
        /// </summary>
        /// <param name="name">元素名</param>
        /// <returns>元素颜色实例</returns>
        public ElemColor this[string name]
        {
            get
            {
                ElemColor result = new ElemColor();
                for (int i = 0; i < this.Count; i++)
                {
                    if (name.Equals(this[i].Name))
                    {
                        result = this[i];
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 加载颜色列表
        /// </summary>
        /// <param name="fileName"></param>
        public void Load(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            XmlNode root = xmlDoc.SelectSingleNode("ElementColors");
            XmlNodeList elemList = root.ChildNodes;

            foreach (XmlNode elemNode in elemList)
            {
                ElemColor elem = new ElemColor();
                XmlNodeList childList = elemNode.ChildNodes;
                foreach (XmlNode child in childList)
                {
                    switch (child.Name)
                    {
                        case "Name":
                            elem.Name = child.InnerText;
                            break;
                        case "Color":
                            elem.Color = Color.FromArgb(Convert.ToInt32(child.InnerText));
                            break;

                    }

                }
                Add(elem);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void Save(string fileName)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("ElementColors");
            for (int i = 0; i < Count; i++)
            {
                writer.WriteStartElement("Elem");
                writer.WriteElementString("Name", this[i].Name);
                writer.WriteElementString("Color", this[i].Color.ToArgb().ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

    }
}
