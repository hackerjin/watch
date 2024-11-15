using System.Data;
using Lephone.Data.Builder;
using Lephone.Data.SqlEntry;

namespace Lephone.Data.Dialect
{
    public class SqlServer2005 : SqlServer2000
    {
        public override bool SupportsRangeStartIndex
        {
            get { return true; }
        }

        protected override SqlStatement GetPagedSelectSqlStatement(SelectStatementBuilder ssb)
        {
            if (ssb.Order == null || ssb.Keys.Count == 0)
            {
                throw PagedMustHaveOrder;
            }

            const string posName = "__rownumber__";
            var dpc = new DataParameterCollection();
            string sqlString = string.Format(
                "SELECT {7} FROM (SELECT {0}, ROW_NUMBER() OVER ({3}) AS {6} FROM {1} {2}) AS T WHERE T.{6} >= {4} AND T.{6} <= {5}",
                ssb.GetColumns(this),
                ssb.From.ToSqlText(dpc, this),
                ssb.Where.ToSqlText(dpc, this),
                ssb.Order.ToSqlText(dpc, this),
                ssb.Range.StartIndex,
                ssb.Range.EndIndex,
                posName,
                ssb.GetColumns(this, false, true)
                );
            return new TimeConsumingSqlStatement(CommandType.Text, sqlString, dpc);
        }
    }
}
