using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.SqlEntry;
using Lephone.Data;
using Lephone.Data.Common;
using System.Collections;
using Lephone.Data.Builder;

namespace Skyray.EDX.Common
{
    public class DBHelper
    {
        /// <summary>
        /// 压缩数据库
        /// </summary>
        public static void CompressDB()
        {
            SqlStatement sql = new SqlStatement("VACUUM");
            sql.SqlCommandType = System.Data.CommandType.Text;
            DbEntry.Context.ExecuteNonQuery(sql);
            DbEntry.GetContext("Lang").ExecuteNonQuery(sql);
        }
    }
}
