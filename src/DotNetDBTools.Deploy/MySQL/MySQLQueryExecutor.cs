using System.Data.Common;
using Dapper;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MySQL
{
    internal class MySQLQueryExecutor : QueryExecutor
    {
        public MySQLQueryExecutor(DbConnection connection)
            : base(connection)
        {
        }

        protected override void BeforeBeginTransaction(DbConnection connection)
        {
            connection.Execute("SET XACT_ABORT ON;");
        }
    }
}
