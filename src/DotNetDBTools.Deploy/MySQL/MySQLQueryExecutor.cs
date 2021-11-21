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
    }
}
