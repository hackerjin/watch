﻿using System;
namespace Lephone.Data
{
    [Serializable]
	public class ASC
	{
		public string Key;

		public ASC(string Key)
		{
			this.Key = Key;
		}

		public virtual string ToString(Dialect.DbDialect dd)
		{
			return dd.QuoteForColumnName( Key ) + " ASC";
		}

		public static explicit operator ASC (string Key)
		{
			return new ASC(Key);
		}
	}
}
