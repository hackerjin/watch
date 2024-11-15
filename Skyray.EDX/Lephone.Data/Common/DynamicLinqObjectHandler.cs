﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Lephone.Util;

namespace Lephone.Data.Common
{
    public class DynamicLinqObjectHandler : FlyweightBase<Type, DynamicLinqObjectHandler>
    {
        protected override DynamicLinqObjectHandler CreateInst(Type t)
        {
            return new DynamicLinqObjectHandler(t);
        }

        public DynamicLinqObjectHandler()
        {
        }

        private readonly ConstructorInfo _constructor;
        private MemberHandler[] _members;
        private List<string> _jar;

        public DynamicLinqObjectHandler(Type type)
        {
            _constructor = type.GetConstructors(ClassHelper.AllFlag)[0];
        }

        public bool IsJarNotInitialized
        {
            get { return _jar == null; }
        }

        public void Init(List<string> jar)
        {
            if(_jar == null)
            {
                _jar = jar;
            }
        }

        public void Init(ObjectInfo oi)
        {
            if(_members == null)
            {
                lock (this)
                {
                    if(_members == null)
                    {
                        var ms = new List<MemberHandler>(oi.Fields);
                        _members = _jar.Select(name => ms.Find(p => p.Name == name)).ToList().ToArray();
                    }
                }
            }
        }

        public MemberHandler[] GetMembers()
        {
            return _members;
        }

        public object CreateObject(IDataReader dr, bool useIndex)
        {
            var args = _jar.Select((t, i) => dr[i]).ToList();
            object obj = _constructor.Invoke(args.ToArray());
            return obj;
        }
    }
}
