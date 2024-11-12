using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Language
{
    /// <summary>
    /// FieldInfo Model
    /// </summary>
    public sealed class LangTextInfo
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Type Type { get; set; }
        public string PropertyName { get; set; }
        public string Text { get; set; }
        /// <summary>
        /// FieldInfo Model
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <param name="value">对象（一般为控件）实例</param>
        /// <param name="type">类型</param>
        /// <param name="strPropertyName">属性名</param>
        /// <param name="strText">属性值</param>
        public LangTextInfo(string name, object value, Type type, string strPropertyName, string strText)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
            this.PropertyName = strPropertyName;
            this.Text = strText;         
        }
    }
}
