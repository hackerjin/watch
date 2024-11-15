namespace Lephone.Data.Builder.Clause
{
    public class ConstCondition : Condition
    {
        private readonly string Condition;

        internal ConstCondition(string Condition)
        {
            this.Condition = Condition;
        }

        public override bool SubClauseNotEmpty
        {
            get { return true; }
        }

        public override string ToSqlText(SqlEntry.DataParameterCollection dpc, Dialect.DbDialect dd)
        {
            return Condition;
        }
    }
}
