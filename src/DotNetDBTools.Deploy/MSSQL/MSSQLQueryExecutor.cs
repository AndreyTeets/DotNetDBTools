using System;
using System.Data.Common;
using Dapper;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL
{
    internal class MSSQLQueryExecutor : QueryExecutor
    {
        public MSSQLQueryExecutor(Func<DbConnection> createDbConnection) : base(
            createDbConnection: createDbConnection)
        {
        }

        protected override void BeforeBeginTransaction(DbConnection connection)
        {
            connection.Execute("SET XACT_ABORT ON;");
        }
    }
}
