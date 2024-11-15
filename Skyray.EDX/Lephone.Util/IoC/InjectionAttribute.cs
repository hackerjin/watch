﻿using System;

namespace Lephone.Util.IoC
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class InjectionAttribute : Attribute
    {
        public string Name { get; set; }

        public InjectionAttribute()
            : this(SimpleContainer.DefaultName)
        {
        }

        public InjectionAttribute(string name)
        {
            this.Name = name;
        }
    }
}
