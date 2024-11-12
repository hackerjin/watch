using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Skyray.EDX.Common.ReportHelper
{
    public class ContentRang
    {
        private string _Name;///<元素名
        private double _MinValue;///<含量最小值
        private double _MaxValue;///<含量最大值

        public ContentRang()
        {
            _MinValue = 2;
        }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="name">元素名</param>
        /// <param name="minValue">含量最小值</param>
        /// <param name="maxValue">含量最大值</param>
        public ContentRang(string name, double minValue, double maxValue)
        {
            _Name = name;
            _MinValue = minValue;
            _MaxValue = maxValue;
        }

        /// <summary>
        /// 元素名
        /// </summary>
        [XmlElement("Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        /// <summary>
        /// 含量最小值
        /// </summary>
        [XmlElement("MinValue")]
        public double MinValue
        {
            get
            {
                return _MinValue;
            }
            set
            {
                _MinValue = value;
            }
        }

        /// <summary>
        /// 含量最大值
        /// </summary>
        [XmlElement("MaxValue")]
        public double MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                _MaxValue = value;
            }
        }

    };
    /// <summary>
    /// 元素含量范围列表类
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ContentRang))]
    public class ElemsStandard : List<ContentRang>
    {
        private string _SearchElemName = string.Empty;

        /// <summary>
        /// 自定义索引
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ContentRang this[string name]
        {
            get
            {
                ContentRang result = new ContentRang();
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
        /// 判断是否存在同名的元素记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool TheSameName(ContentRang item)
        {
            bool result = false;
            if ((_SearchElemName != "") && (item.Name.Equals(_SearchElemName)))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="item"></param>
        public new void Add(ContentRang item)
        {
            _SearchElemName = item.Name;
            int index = this.FindIndex(TheSameName);
            if (index >= 0)
            {
               Msg.Show("The same element exists!", "Info");
            }
            else
            {
                base.Add(item);
            }
        }

        public bool IsExists(string name)
        {
            _SearchElemName = name;
            bool result = false;
            if (this.FindIndex(TheSameName) >= 0)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 加载所有记录
        /// </summary>
        /// <param name="fileName"></param>
        public void Load(string fileName)
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(ElemsStandard));
            try
            {
                FileStream stream = new FileStream(fileName, FileMode.Open);
                ElemsStandard elems = (ElemsStandard)xmlSer.Deserialize(stream);
                this.Clear();
                this.AddRange(elems);
                stream.Close();
            }
            catch (FileNotFoundException e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// 保存记录
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(ElemsStandard));
            StreamWriter writer = new StreamWriter(fileName);
            XmlSerializerNamespaces xmls = new XmlSerializerNamespaces();
            xmls.Add(string.Empty, string.Empty);
            xmlSer.Serialize(writer, this, xmls);
            writer.Close();
        }

        /// <summary>
        /// 获取元素含量报告输出格式（小于设定最小值时为“ND”，否则保留一位小数）
        /// </summary>
        /// <param name="name">元素名</param>
        /// <param name="content">元素含量</param>
        /// <returns></returns>
        public string GetContentFormat(string name, double content)
        {
            string result = content.ToString("f1");
            if (IsExists(name))
            {
                if (content < this[name].MinValue)
                {
                    result = "ND";
                }

            }
            return result;
        }

    }
}
