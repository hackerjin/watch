using System;
using Lephone.Data.Dialect;
using Lephone.Data.SqlEntry;

namespace Lephone.Data.Builder.Clause
{
	[Serializable]
	public class WhereClause : IClause
	{
		private Condition _ic;

		public WhereClause()
		{
		}

		public WhereClause(Condition ic)
		{
			_ic = ic;
		}

		public Condition Conditions
		{
			set { _ic = value; }
			get { return _ic; }
		}

		public string ToSqlText(DataParameterCollection dpc, DbDialect dd)
		{
			if ( _ic != null )
			{
				string s = _ic.ToSqlText(dpc, dd);
                if (s != null)
                {
                    return (s.Length > 0) ? " WHERE " + s : "";
                }
			}
			return "";
		}

		public static implicit operator WhereClause (Condition iwc)
		{
			return new WhereClause(iwc);
		}
	}
}
