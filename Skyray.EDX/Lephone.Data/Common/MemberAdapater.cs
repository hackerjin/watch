﻿using System;
using System.Reflection;
using Lephone.Util;

namespace Lephone.Data.Common
{
    public abstract class MemberAdapter
    {
        #region MemberAdapter Impl

        internal class FieldAdapter : MemberAdapter
        {
            protected FieldInfo fi;

            public FieldAdapter(FieldInfo fi)
            {
                this.fi = fi;
            }

            public override Type MemberType
            {
                get
                {
                    return fi.FieldType;
                }
            }

            public override void SetValue(object obj, object value)
            {
                fi.SetValue(obj, value);
            }

            public override object GetValue(object obj)
            {
                return fi.GetValue(obj);
            }

            public override T[] GetAttributes<T>(bool inherit)
            {
                return ClassHelper.GetAttributes<T>(fi, inherit);
            }

            public override T GetAttribute<T>(bool inherit)
            {
                return ClassHelper.GetAttribute<T>(fi, inherit);
            }

            public override bool HasAttribute<T>(bool inherit)
            {
                return ClassHelper.HasAttribute<T>(fi, inherit);
            }

            public override string Name
            {
                get { return fi.Name; }
            }

            public override bool IsProperty
            {
                get { return false; }
            }

            public override MemberInfo GetMemberInfo()
            {
                return fi;
            }

            public override void EmitSet(ILBuilder il)
            {
                il.SetField(fi);
            }

            public override void EmitGet(ILBuilder il)
            {
                il.LoadField(fi);
            }
        }

        internal class PropertyAdapter : MemberAdapter
        {
            protected PropertyInfo pi;

            public PropertyAdapter(PropertyInfo pi)
            {
                this.pi = pi;
            }

            public override Type MemberType
            {
                get { return pi.PropertyType; }
            }

            public override void SetValue(object obj, object value)
            {
                pi.SetValue(obj, value, null);
            }

            public override object GetValue(object obj)
            {
                return pi.GetValue(obj, null);
            }

            public override T[] GetAttributes<T>(bool inherit)
            {
                return ClassHelper.GetAttributes<T>(pi, inherit);
            }

            public override T GetAttribute<T>(bool inherit)
            {
                return ClassHelper.GetAttribute<T>(pi, inherit);
            }

            public override bool HasAttribute<T>(bool inherit)
            {
                return ClassHelper.HasAttribute<T>(pi, inherit);
            }

            public override string Name
            {
                get { return pi.Name; }
            }

            public override bool IsProperty
            {
                get { return true; }
            }

            public override MemberInfo GetMemberInfo()
            {
                return pi;
            }

            public override void EmitSet(ILBuilder il)
            {
                il.CallVirtual(pi.GetSetMethod());
            }

            public override void EmitGet(ILBuilder il)
            {
                il.CallVirtual(pi.GetGetMethod());
            }
        }

        internal class UnsignedPropertyAdapter : PropertyAdapter
        {
            public UnsignedPropertyAdapter(PropertyInfo pi)
                : base(pi)
            {
            }

            public override void SetValue(object obj, object value)
            {
                if (pi.PropertyType == typeof(ulong))
                {
                    pi.SetValue(obj, (ulong)(long)value, null);
                }
                else if (pi.PropertyType == typeof(uint))
                {
                    pi.SetValue(obj, (uint)(int)value, null);
                }
                else if (pi.PropertyType == typeof(ushort))
                {
                    pi.SetValue(obj, (ushort)(short)value, null);
                }
            }
        }

        internal class UnsignedFieldAdapter : FieldAdapter
        {
            public UnsignedFieldAdapter(FieldInfo fi)
                : base(fi)
            {
            }

            public override void SetValue(object obj, object value)
            {
                if (fi.FieldType == typeof(ulong))
                {
                    fi.SetValue(obj, (ulong)(long)value);
                }
                else if (fi.FieldType == typeof(uint))
                {
                    fi.SetValue(obj, (uint)(int)value);
                }
                else if (fi.FieldType == typeof(ushort))
                {
                    fi.SetValue(obj, (ushort)(short)value);
                }
            }
        }

        #endregion

        public abstract bool IsProperty { get; }
        public abstract string Name { get; }
        public abstract T[] GetAttributes<T>(bool inherit) where T : Attribute;
        public abstract T GetAttribute<T>(bool inherit) where T : Attribute;
        public abstract bool HasAttribute<T>(bool inherit) where T : Attribute;
        public abstract Type MemberType { get; }
        public abstract void SetValue(object obj, object value);
        public abstract object GetValue(object obj);
        public abstract MemberInfo GetMemberInfo();
        public abstract void EmitSet(ILBuilder il);
        public abstract void EmitGet(ILBuilder il);

        public static MemberAdapter NewObject(FieldInfo fi)
        {
            if (fi.FieldType == typeof(ulong) || fi.FieldType == typeof(uint) || fi.FieldType == typeof(ushort))
            {
                return new UnsignedFieldAdapter(fi);
            }
            return new FieldAdapter(fi);
        }

        public static MemberAdapter NewObject(PropertyInfo pi)
        {
            if (pi.PropertyType == typeof(ulong) || pi.PropertyType == typeof(uint) || pi.PropertyType == typeof(ushort))
            {
                return new UnsignedPropertyAdapter(pi);
            }
            return new PropertyAdapter(pi);
        }
    }
}
