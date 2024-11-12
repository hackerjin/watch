using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Lephone.Util;
using Lephone.Data.Dialect;
using Lephone.Data.Driver;
using Lephone.Data.Common;
using Lephone.Data.Builder;
using System.Collections;
using Lephone.Data.Definition;

namespace Lephone.Data.SqlEntry
{
    public class DataProvider : IHasConnection
    {
        internal DbDriver m_Driver;

        ConnectionContext IHasConnection.ConnectionProvider
        {
            get { return ConProvider; }
        }

        internal ConnectionContext ConProvider
        {
            get
            {
                if (Scope<ConnectionContext>.Current != null)
                {
                    return Scope<ConnectionContext>.Current;
                }
                return new ConnectionContext(m_Driver);
            }
        }

        protected DataProvider() { }

        public DataProvider(DbDriver driver)
        {
            m_Driver = driver;
        }

        public DbDialect Dialect
        {
            get { return m_Driver.Dialect; }
        }

        public DbDriver Driver
        {
            get { return m_Driver; }
        }

        #region utils

        public List<DbColumnInfo> GetDbColumnInfoList(string tableName)
        {
            string sqlStr = "select * from " + Dialect.QuoteForTableName(tableName) + " where 1<>1";
            var sql = new SqlStatement(CommandType.Text, sqlStr);
            var ret = new List<DbColumnInfo>();
            ExecuteDataReader(sql, CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly, delegate(IDataReader dr)
            {
                DataTable dt = dr.GetSchemaTable();
                foreach (DataRow row in dt.Rows)
                {
                    ret.Add(new DbColumnInfo(row));
                }
            });
            return ret;
        }

        public List<string> GetTableNames()
        {
            var ret = new List<string>();
            DbStructInterface si = Dialect.GetDbStructInterface();
            string userId = Dialect.GetUserId(Driver.ConnectionString);
            NewConnection(delegate
            {
                var c = (DbConnection)ConProvider.Connection;
                foreach (DataRow dr in c.GetSchema(si.TablesTypeName, si.TablesParams).Rows)
                {
                    if (si.FiltrateDatabaseName)
                    {
                        if (!dr["TABLE_SCHEMA"].Equals(c.Database)) { continue; }
                    }
                    if (userId != null)
                    {
                        if (!dr["OWNER"].Equals(userId)) { continue; }
                    }
                    string s = dr[si.TableNameString].ToString();
                    ret.Add(s);
                }
            });
            return ret;
        }

        public IDbBulkCopy GetDbBulkCopy()
        {
            if (Driver is SqlServerDriver)
            {
                if (Scope<ConnectionContext>.Current != null)
                {
                    var c = (SqlConnection)Scope<ConnectionContext>.Current.Connection;
                    return new SqlServerBulkCopy(c);
                }
                throw new DataException("It must have current connection.");
            }
            return new CommonBulkCopy(this);
        }

        #endregion

        #region Execute Sql

        public DataSet ExecuteDataset(SqlStatement sql, Type returnType)
        {
            var ds = (DataSet)ClassHelper.CreateInstance(returnType);
            ExecuteDataset(sql, ds);
            return ds;
        }

        public DataSet ExecuteDataset(SqlStatement sql)
        {
            var ds = new DataSet();
            ExecuteDataset(sql, ds);
            return ds;
        }

        private void ExecuteDataset(SqlStatement sql, DataSet ds)
        {
            UsingConnection(delegate
            {
                using (IDbCommand e = GetDbCommand(sql))
                {
                    IDbDataAdapter d = m_Driver.GetDbAdapter(e);
                    if (Dialect.ExecuteEachLine)
                    {
                        int i = 0;
                        foreach (string s in Split(e.CommandText))
                        {
                            e.CommandText = s;
                            ((DbDataAdapter)d).Fill(ds, 0, DataSetting.MaxRecords, "Table" + i);
                            i++;
                        }
                    }
                    else
                    {
                        d.Fill(ds);
                    }
                    PopulateOutParams(sql, e);
                }
            });
        }

        public int UpdateDataset(SqlStatement selectSql, DataSet ds)
        {
            return UpdateDataset(selectSql, ds, 1);
        }

        public int UpdateDataset(SqlStatement selectSql, DataSet ds, int updateBatchSize)
        {
            int ret = 0;
            UsingConnection(delegate
            {
                var d = (DbDataAdapter)m_Driver.GetDbAdapter(GetDbCommand(selectSql));
                var cb = m_Driver.GetCommandBuilder();
                cb.QuotePrefix = Dialect.OpenQuote.ToString();
                cb.QuoteSuffix = Dialect.CloseQuote.ToString();
                cb.DataAdapter = d;
                d.UpdateBatchSize = updateBatchSize;
                ret = d.Update(ds);
                ds.AcceptChanges();
            });
            return ret;
        }

        public int UpdateDataset(SqlStatement insertSql, SqlStatement updateSql, SqlStatement deleteSql, DataSet ds)
        {
            return UpdateDataset(insertSql, updateSql, deleteSql, ds, 1, false);
        }

        public int UpdateDataset(SqlStatement insertSql, SqlStatement updateSql, SqlStatement deleteSql, DataSet ds, int updateBatchSize)
        {
            return UpdateDataset(insertSql, updateSql, deleteSql, ds, updateBatchSize, true);
        }

        private int UpdateDataset(SqlStatement insertSql, SqlStatement updateSql, SqlStatement deleteSql, DataSet ds, int updateBatchSize, bool throwException)
        {
            int ret = 0;
            UsingConnection(delegate
            {
                IDbDataAdapter d = m_Driver.GetDbAdapter();
                if (insertSql != null)
                {
                    d.InsertCommand = GetDbCommandForUpdate(insertSql);
                }
                if (updateSql != null)
                {
                    d.UpdateCommand = GetDbCommandForUpdate(updateSql);
                }
                if (deleteSql != null)
                {
                    d.DeleteCommand = GetDbCommandForUpdate(deleteSql);
                }
                if (d is DbDataAdapter)
                {
                    ((DbDataAdapter)d).UpdateBatchSize = updateBatchSize;
                }
                else if (throwException)
                {
                    throw new DataException("The DbDataAdapter doesn't support UpdateBatchSize feature.");
                }
                ret = d.Update(ds);
                ds.AcceptChanges();
            });
            return ret;
        }

        private IDbCommand GetDbCommandForUpdate(SqlStatement sql)
        {
            IDbCommand c = GetDbCommand(sql);
            foreach (IDataParameter p in c.Parameters)
            {
                p.SourceColumn = p.ParameterName[0] == Dialect.ParameterPrefix ? p.ParameterName.Substring(1) : p.ParameterName;
            }
            return c;
        }

        public object ExecuteScalar(SqlStatement sql)
        {
            object obj = null;
            UsingConnection(delegate
            {
                using (IDbCommand e = GetDbCommand(sql))
                {
                    if (Dialect.ExecuteEachLine)
                    {
                        ExecuteBeforeLines(e);
                    }
                    obj = e.ExecuteScalar();
                    PopulateOutParams(sql, e);
                }
            });
            return obj;
        }

        public int ExecuteNonQuery(SqlStatement sql)
        {
            int i = 0;
            UsingConnection(delegate
            {
                using (IDbCommand e = GetDbCommand(sql))
                {
                    if (Dialect.ExecuteEachLine)
                    {
                        i = ExecuteBeforeLines(e);
                    }
                    i += e.ExecuteNonQuery();
                    PopulateOutParams(sql, e);
                }
            });
            return i;
        }

        public void ExecuteDataReader(SqlStatement sql, CallbackObjectHandler<IDataReader> callback)
        {
            ExecuteDataReader(sql, CommandBehavior.Default, callback);
        }

        public void ExecuteDataReader(SqlStatement sql, CommandBehavior behavior, CallbackObjectHandler<IDataReader> callback)
        {
            UsingConnection(delegate
            {
                using (IDbCommand e = GetDbCommand(sql))
                {
                    if (Dialect.ExecuteEachLine)
                    {
                        ExecuteBeforeLines(e);
                    }
                    using (IDataReader r = e.ExecuteReader(behavior))
                    {
                        PopulateOutParams(sql, e);
                        callback(r);
                    }
                }
            });
        }

        // It's only for stupid oracle
        internal void ExecuteDataReader(SqlStatement sql, Type returnType, CallbackObjectHandler<IDataReader> callback)
        {
            UsingConnection(delegate
            {
                using (IDbCommand e = GetDbCommand(sql))
                {
                    if (Dialect.ExecuteEachLine)
                    {
                        ExecuteBeforeLines(e);
                    }
                    using (IDataReader r = e.ExecuteReader(CommandBehavior.Default))
                    {
                        PopulateOutParams(sql, e);
                        using (IDataReader dr = Dialect.GetDataReader(r, returnType))
                        {
                            callback(dr);
                        }
                    }
                }
            });
        }

        public IDbCommand GetDbCommand(SqlStatement sql)
        {
            ConnectionContext et = ConProvider;
            IDbCommand e = m_Driver.GetDbCommand(sql, et.Connection);
            if (et.Transaction != null)
            {
                e.Transaction = et.Transaction;
            }
            return e;
        }

        protected void PopulateOutParams(SqlStatement sql, IDbCommand e)
        {
            if (sql.Parameters.UserSetKey && (sql.SqlCommandType == CommandType.StoredProcedure))
            {
                for (int i = 0; i < sql.Parameters.Count; i++)
                {
                    DataParameter p = sql.Parameters[i];
                    if (p.Direction != ParameterDirection.Input)
                    {
                        p.Value = ((IDbDataParameter)e.Parameters[i]).Value;
                    }
                }
            }
        }

        #endregion

        #region Lines plus

        protected int ExecuteBeforeLines(IDbCommand e)
        {
            List<string> al = Split(e.CommandText);
            int ret = 0;
            for (int i = 0; i < al.Count - 1; i++)
            {
                e.CommandText = al[i];
                ret += e.ExecuteNonQuery();
            }
            e.CommandText = al[al.Count - 1];
            return ret;
        }

        private List<string> Split(string cText)
        {
            var ret = new List<string>();
            using (var sr = new StreamReader(new MemoryStream(Encoding.Unicode.GetBytes(cText)), Encoding.Unicode))
            {
                var statement = new StringBuilder();
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    s = s.Trim();
                    if (s.Length > 0)
                    {
                        if (s.Length > 1 && s.Substring(0, 2) == "--") { continue; }
                        if (s[s.Length - 1] == ';')
                        {
                            statement.Append(s.Substring(0, s.Length));
                            if (Dialect.NotSupportPostFix) { statement.Length--; }
                            if (statement.Length != 0)
                            {
                                ret.Add(statement.ToString());
                            }
                            statement = new StringBuilder();
                        }
                        else
                        {
                            statement.Append(s);
                        }
                    }
                }
                if (statement.Length != 0)
                {
                    ret.Add(statement.ToString());
                }
            }
            return ret;
        }

        #endregion

        #region Shortcut

        private static readonly Regex Reg = new Regex("'.*'|\\?", RegexOptions.Compiled);

        public SqlStatement GetSqlStatement(string sqlStr, params object[] os)
        {
            CommandType ct = SqlStatement.GetCommandType(sqlStr);
            if (ct == CommandType.StoredProcedure)
            {
                return new SqlStatement(ct, sqlStr, os);
            }
            var dpc = new DataParameterCollection();
            int start = 0, n = 0;
            var sql = new StringBuilder();
            string pp = Dialect.ParameterPrefix + "p";
            foreach (Match m in Reg.Matches(sqlStr))
            {
                if (m.Length == 1)
                {
                    string pn = pp + n;
                    sql.Append(sqlStr.Substring(start, m.Index - start));
                    sql.Append(pn);
                    start = m.Index + 1;
                    var dp = new DataParameter(pn, os[n]);
                    dpc.Add(dp);
                    n++;
                }
            }
            if (start < sqlStr.Length)
            {
                sql.Append(sqlStr.Substring(start));
            }
            var ret = new SqlStatement(ct, sql.ToString(), dpc);
            return ret;
        }

        public DataSet ExecuteDataset(string sqlCommandText, params object[] os)
        {
            return ExecuteDataset(GetSqlStatement(sqlCommandText, os));
        }

        public object ExecuteScalar(string sqlCommandText, params object[] os)
        {
            return ExecuteScalar(GetSqlStatement(sqlCommandText, os));
        }

        public int ExecuteNonQuery(string sqlCommandText, params object[] os)
        {
            return ExecuteNonQuery(GetSqlStatement(sqlCommandText, os));
        }

        #endregion

        #region IUsingTransaction

        public void UsingTransaction(CallbackVoidHandler callback)
        {
            if (Scope<ConnectionContext>.Current != null)
            {
                callback();
                return;
            }
            NewTransaction(callback);
        }

        public void UsingTransaction(IsolationLevel il, CallbackVoidHandler callback)
        {
            if (Scope<ConnectionContext>.Current != null)
            {
                ConnectionContext et = Scope<ConnectionContext>.Current;
                if (et.IsolationLevel == il)
                {
                    callback();
                    return;
                }
            }
            NewTransaction(callback);
        }

        public void NewTransaction(CallbackVoidHandler callback)
        {
            NewTransaction(IsolationLevel.ReadCommitted, callback);
        }

        public void NewTransaction(IsolationLevel il, CallbackVoidHandler callback)
        {
            NewConnection(delegate
            {
                ConnectionContext cc = ConProvider;
                cc.BeginTransaction(il);
                try
                {
                    OnBeginTransaction();
                    callback();
                    cc.Commit();
                    OnCommittedTransaction();
                }
                catch
                {
                    try
                    {
                        cc.Rollback();
                    }
                    finally
                    {
                        OnTransactionError();
                    }
                    throw;
                }
            });
        }

        protected internal virtual void OnBeginTransaction()
        {
        }

        protected internal virtual void OnCommittedTransaction()
        {
        }

        protected internal virtual void OnTransactionError()
        {
        }

        public void NewConnection(CallbackVoidHandler callback)
        {
            using (var cc = new ConnectionContext(m_Driver))
            {
                using (new Scope<ConnectionContext>(cc))
                {
                    try
                    {
                        callback();
                    }
                    finally
                    {
                        cc.Close();
                    }
                }
            }
        }

        public void UsingConnection(CallbackVoidHandler callback)
        {
            if (Scope<ConnectionContext>.Current != null)
            {
                callback();
                return;
            }
            NewConnection(callback);
        }

        #endregion

        #region  Add By WZW
        /// <summary>
        /// Add By WZW,用于使用Ado.net从DB中获取数据
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ColNames"></param>
        /// <returns></returns>
        public List<object[]> GetColValueListByADO(string TableName, params string[] ColNames)
        {
            int length = ColNames.Length;
            for (int i = 0; i < length; i++)
            {
                ColNames[i] = this.Dialect.QuoteForColumnName(ColNames[i]);
            }
            string cmdText = "select " + string.Join(",", ColNames) + " from " + this.Dialect.QuoteForTableName(TableName);
            List<object[]> list = new List<object[]>();
            using (System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(DbEntry.Context.Driver.ConnectionString))
            {
                using (System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    using (System.Data.OleDb.OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object[] item = new object[length];
                            for (int j = 0; j < length; j++)
                            {
                                item[j] = reader[j];
                            }
                            list.Add(item);
                        }
                    }
                    return list;
                }
            }
        }



        ///// <summary>
        ///// 单张表格快速批量插入数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj"></param>
        //public void FastSaveList<T>(T obj) where T : IEnumerable
        //{
        //    Type t = null;
        //    ObjectInfo oi = null;
        //    InsertStatementBuilder sb;
        //    UsingTransaction(delegate
        //    {
        //        foreach (object o in (IEnumerable)obj)
        //        {
        //            if (t == null) t = o.GetType();
        //            if (oi == null) oi = ObjectInfo.GetInstance(t);

        //            //foreach (var f in oi.RelationFields)
        //            //{
        //            //    var op = f.GetValue(o);
        //            //    var ho = (ILazyLoading)op;
        //            //    if (ho.IsLoaded)
        //            //    {
        //            //        if (f.IsBelongsTo)
        //            //        {
        //            //            var ibelongsto = op as IBelongsTo;
        //            //            if (ibelongsto != null)
        //            //            {
        //            //                var o1 = ho.Read();
        //            //                ibelongsto.ForeignKey = o1.GetType().GetProperty("Id").GetValue(o1, null).ToString();
        //            //            }
        //            //        }
        //            //        else
        //            //        {
        //            //            //其余关系实现
        //            //        }
        //            //    }
        //            //}



        //            sb = oi.Composer.GetInsertStatementBuilder(o);
        //            SqlStatement sql = sb.ToSqlStatement(Dialect);

        //            if (oi.HasSystemKey)
        //            {
        //                sql.SqlCommandText = Dialect.AddIdentitySelectToInsert(sql.SqlCommandText);
        //            }
        //            using (IDbCommand e = GetDbCommand(sql))
        //            {
        //                if (Dialect.ExecuteEachLine)
        //                {
        //                    ExecuteBeforeLines(e);
        //                }
        //                e.ExecuteNonQuery();
        //                PopulateOutParams(sql, e);
        //            }
        //        }
        //    });
        //}


        /// <summary>
        /// 单张表格快速批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public int FastSaveList<T>(T obj, object foreignKey) where T : IEnumerable
        {
            int iCount = 0;
            Type t = null;
            ObjectInfo oi = null;
            MemberHandler k = null;

            UsingTransaction(delegate
            {
                foreach (object o in (IEnumerable)obj)
                {
                    if (t == null)
                    {
                        t = o.GetType();
                        oi = ObjectInfo.GetInstance(t);
                        k = oi.KeyFields[0];
                    }

                    bool isInsert = k.UnsavedValue.Equals(k.GetValue(o));

                    if (isInsert)
                    {
                        for (int i = 0; i < oi.RelationFields.Length; i++)
                        {
                            var f = oi.RelationFields[i];
                            if (f.IsBelongsTo)
                            {
                                var op = f.GetValue(o);
                                var ibelongsto = op as IBelongsTo;

                                if (ibelongsto != null)
                                {
                                    ibelongsto.ForeignKey = foreignKey;
                                }
                            }
                            else
                            {
                                //其余关系实现
                            }
                        }
                    }

                    SqlStatement sql = isInsert ? GetInsertSql(o, oi) : GetUpdateSql(o, oi);

                    if (sql != null)
                    {
                        using (IDbCommand e = GetDbCommand(sql))
                        {
                            if (Dialect.ExecuteEachLine)
                            {
                                ExecuteBeforeLines(e);
                            }
                            iCount += e.ExecuteNonQuery();
                            //PopulateOutParams(sql, e);
                        }
                    }
                }
            });
            return iCount;
        }
        private SqlStatement GetUpdateSql(object o, ObjectInfo oi)
        {
            SqlStatement sql = null;
            var to = o as DbObjectSmartUpdate;
            if (to != null && to.m_UpdateColumns != null)
            {
                if (to.m_UpdateColumns.Count > 0)
                {
                    var iwc = ObjectInfo.GetKeyWhereClause(o);
                    sql = oi.Composer.GetUpdateStatement(Dialect, o, iwc);
                }
            }
            return sql;
        }

        private SqlStatement GetInsertSql(object o, ObjectInfo oi)
        {
            SqlStatement sql = oi.Composer.GetInsertStatementBuilder(o).ToSqlStatement(Dialect);
            if (oi.HasSystemKey)
            {
                sql.SqlCommandText = Dialect.AddIdentitySelectToInsert(sql.SqlCommandText);
            }
            return sql;
        }

        public void FastDeleteList<T>(T obj) where T : IEnumerable
        {

            Type t = null;
            ObjectInfo oi = null;
            MemberHandler k = null;

            UsingTransaction(delegate
            {
                foreach (object o in (IEnumerable)obj)
                {
                    if (t == null)
                    {
                        t = o.GetType();
                        oi = ObjectInfo.GetInstance(t);
                        k = oi.KeyFields[0];
                    }
                    SqlStatement sql = GetDelSql(o, oi);

                    using (IDbCommand e = GetDbCommand(sql))
                    {
                        if (Dialect.ExecuteEachLine)
                        {
                            ExecuteBeforeLines(e);
                        }
                        e.ExecuteNonQuery();
                    }
                }
            });
        }

        private SqlStatement GetDelSql(object o, ObjectInfo oi)
        {
            SqlStatement sql = null;
            var iwc = ObjectInfo.GetKeyWhereClause(o);
            sql = oi.Composer.GetDeleteStatement(Dialect, iwc);
            return sql;
        }

        public int FastSave<T>(object foreignKey, params LineInfo<T>[] lineInfos)
        {
            int iCount = 0;
            Type t = null;
            ObjectInfo oi = null;
            MemberHandler k = null;

            UsingTransaction(delegate
            {
                foreach (LineInfo<T> lineInfo in lineInfos)
                {
                    foreach (object o in lineInfo.Objs)
                    {
                        if (t == null)
                        {
                            t = o.GetType();
                            oi = ObjectInfo.GetInstance(t);
                            k = oi.KeyFields[0];
                        }
                    
                        SqlStatement sql = null;
                        if (lineInfo.IsToDelete)
                        {
                            sql = GetDelSql(o, oi);
                        }
                        else
                        {
                            bool isInsert = k.UnsavedValue.Equals(k.GetValue(o));
                            if (isInsert)
                            {
                                SetRelation(foreignKey, oi, o);
                                sql = GetInsertSql(o, oi);
                            }
                            else
                            {
                                sql = GetUpdateSql(o, oi);
                            }
                        }
                        if (sql != null)
                        {
                            using (IDbCommand e = GetDbCommand(sql))
                            {
                                if (Dialect.ExecuteEachLine)
                                {
                                    ExecuteBeforeLines(e);
                                }
                                iCount += e.ExecuteNonQuery();
                            }
                        }
                    }
                }
            });
            return iCount;
        }

        private static void SetRelation(object foreignKey, ObjectInfo oi, object o)
        {
            for (int i = 0; i < oi.RelationFields.Length; i++)
            {
                var f = oi.RelationFields[i];
                if (f.IsBelongsTo)
                {
                    var op = f.GetValue(o);
                    var ibelongsto = op as IBelongsTo;

                    if (ibelongsto != null)
                    {
                        ibelongsto.ForeignKey = foreignKey;
                    }
                }
                else
                {
                    //其余关系实现
                }
            }
        }

        public class LineInfo<T>
        {
            public bool IsToDelete { get; set; }
            public IEnumerable<T> Objs { get; set; }
        }

        #endregion
    }

}
