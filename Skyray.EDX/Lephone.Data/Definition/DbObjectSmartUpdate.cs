﻿using System;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Collections.Generic;
using Lephone.Data.Common;

namespace Lephone.Data.Definition
{
    [Serializable]
    [XmlRoot("DbObject")]
    public class DbObjectSmartUpdate : DbObjectBase, IXmlSerializable
    {
        [Exclude]
        internal protected Dictionary<string, object> m_UpdateColumns;

        [Exclude]
        internal bool m_InternalInit;

        protected void m_InitUpdateColumns()
        {
            m_UpdateColumns = new Dictionary<string, object>();
        }

        protected internal void m_ColumnUpdated(string columnName)
        {
            if (m_UpdateColumns != null && !m_InternalInit)
            {
                m_UpdateColumns[columnName] = 1;
            }
        }

        public XmlSchema GetSchema()
        {
            //ObjectInfo Info = ObjectInfo.GetInstance(this.GetType());
            //XmlSchema xs = new XmlSchema();
            //XmlSchemaComplexType xct = new XmlSchemaComplexType();
            //xct.Name = "DbObject";
            //XmlSchemaSequence xss = new XmlSchemaSequence();
            //foreach (MemberHandler mh in Info.SimpleFields)
            //{
            //    XmlSchemaElement xe = new XmlSchemaElement();
            //    xe.Name = mh.MemberInfo.Name;
            //    xe.ElementSchemaType = XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Int);
            //    xss.Items.Add(xe);
            //}
            //xct.ContentModel = xss;
            //xs.Items.Add(xct);
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            //var oi = ObjectInfo.GetInstance(GetType());
            //foreach (MemberHandler mh in oi.SimpleFields)
            //{
            //    var ns = reader.ReadElementString(mh.MemberInfo.Name);
            //    object o = ClassHelper.ChangeType(ns, mh.FieldType);
            //    mh.SetValue(this, o);
            //}
            throw new NotImplementedException(); // can not create instance of abstract class...
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var oi = ObjectInfo.GetInstance(GetType());
            foreach (MemberHandler mh in oi.SimpleFields)
            {
                object o = mh.GetValue(this);
                if (o != null)
                {
                    writer.WriteElementString(mh.MemberInfo.Name, o.ToString());
                }
            }
        }

        public virtual void Save()
        {
            DbEntry.Save(this);
        }

        public virtual void Delete()
        {
            DbEntry.Delete(this);
        }

        public virtual ValidateHandler Validate()
        {
            var v = new ValidateHandler();
            v.ValidateObject(this);
            return v;
        }

        public virtual bool IsValid()
        {
            return Validate().IsValid;
        }
    }
}
