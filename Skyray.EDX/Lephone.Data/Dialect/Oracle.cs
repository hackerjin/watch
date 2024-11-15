using System;
using System.Data;
using Lephone.Data.SqlEntry;
using Lephone.Data.Builder;
using Lephone.Data.Common;
using Lephone.Util.Logging;

namespace Lephone.Data.Dialect
{
    public class Oracle : SequencedDialect
    {
        public Oracle()
        {
            TypeNames[DataType.String] = "CLOB";
            TypeNames[DataType.DateTime] = "TIMESTAMP";
            TypeNames[DataType.Date] = "DATE";
            TypeNames[DataType.Time] = "DATE"; //TODO: Is it right?
            TypeNames[DataType.Boolean] = "NUMBER(1,0)";

            TypeNames[DataType.Byte] = "NUMBER(3,0)";
            TypeNames[DataType.SByte] = "";
            TypeNames[DataType.Decimal] = "NUMBER(19,5)";
            TypeNames[DataType.Double] = "DOUBLE PRECISION";
            TypeNames[DataType.Single] = "FLOAT(24)";

            TypeNames[DataType.Int32] = "NUMBER(10,0)";
            TypeNames[DataType.UInt32] = "NUMBER(10,0)";
            TypeNames[DataType.Int64] = "NUMBER(20,0)";
            TypeNames[DataType.UInt64] = "NUMBER(20,0)";
            TypeNames[DataType.Int16] = "NUMBER(5,0)";
            TypeNames[DataType.UInt16] = "NUMBER(5,0)";

            TypeNames[DataType.Binary] = "BLOB";

            TypeNames[typeof(string)] = "VARCHAR2";
        }

        public override string DbNowString
        {
            get { return "SYSDATE"; }
        }

        public override string GetUserId(string connectionString)
        {
            string [] ss = connectionString.Split(';');
            foreach (string s in ss)
            {
                string[] ms = s.Split('=');
                if (ms[0].Trim().ToLower() == "user id")
                {
                    return ms[1].Trim().ToUpper();
                }
            }
            return null;
        }

        public override IDataReader GetDataReader(IDataReader dr, Type returnType)
        {
            return new StupidDataReader(dr, returnType);
        }

        public override bool NotSupportPostFix
        {
            get { return true; }
        }

        public override string GetSelectSequenceSql(string tableName)
        {
            return string.Format("SELECT {0}_SEQ.NEXTVAL FROM DUAL", tableName.ToUpper());
        }

        public override bool NeedCommitCreateFirst
        {
            get { return false; }
        }

        public override bool SupportDirctionOfEachColumnInIndex
        {
            get { return false; }
        }

        public override string IdentityColumnString
        {
            get { return "NOT NULL"; }
        }

        public override string GetCreateSequenceString(string tableName)
        {
            return string.Format("CREATE SEQUENCE {0}_SEQ INCREMENT BY 1;\n", tableName.ToUpper());
        }

        protected override string QuoteSingle(string name)
        {
            return base.QuoteSingle(name.ToUpper());
        }

        public override string NullString
        {
            get { return ""; }
        }

        public override bool ExecuteEachLine
        {
            get { return true; }
        }

        public override void ExecuteDropSequence(DataProvider dp, string tableName)
        {
            string sql = string.Format("DROP SEQUENCE {0}_SEQ;\n", tableName.ToUpper());
            Logger.SQL.Trace(sql);
            dp.ExecuteNonQuery(sql);
        }

        protected override SqlStatement GetPagedSelectSqlStatement(SelectStatementBuilder ssb)
        {
            SqlStatement sql = base.GetNormalSelectSqlStatement(ssb);
            sql.SqlCommandText = string.Format("SELECT * FROM ( SELECT ROW_.*, ROWNUM ROWNUM_ FROM ( {0} ) ROW_ WHERE ROWNUM <= {1} ) WHERE ROWNUM_ >= {2}",
                sql.SqlCommandText, ssb.Range.EndIndex, ssb.Range.StartIndex);
            return sql;
        }

        public override char ParameterPrefix
        {
            get { return ':'; }
        }

        public override string GenIndexName(string n)
        {
            return GenIndexName(n, 30);
        }
    }
}
