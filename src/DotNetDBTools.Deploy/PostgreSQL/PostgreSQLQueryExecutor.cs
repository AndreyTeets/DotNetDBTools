using System.Data.Common;
using Dapper;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL
{
    internal class PostgreSQLQueryExecutor : QueryExecutor
    {
        public PostgreSQLQueryExecutor(DbConnection connection)
            : base(connection)
        {
        }

        protected override void BeforeBeginTransaction(DbConnection connection)
        {
            connection.Execute("SET XACT_ABORT ON;");
        }
    }
}
