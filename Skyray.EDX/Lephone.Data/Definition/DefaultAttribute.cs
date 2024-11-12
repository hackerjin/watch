using System;


namespace Lephone.Data.Definition
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DefaultFieldValueAttribute:Attribute
    {
        private readonly string _value;

		public string Value
		{
			get { return _value; }
		}

        public DefaultFieldValueAttribute(string vale)
		{
			_value = vale;
		}
    }
}
