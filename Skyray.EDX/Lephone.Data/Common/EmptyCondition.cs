namespace Lephone.Data.Common
{
    public class EmptyCondition : Condition
    {
        public override bool SubClauseNotEmpty
        {
            get { return false; }
        }

        public override string ToSqlText(SqlEntry.DataParameterCollection dpc, Dialect.DbDialect dd)
        {
            return null;
        }
    }
}
