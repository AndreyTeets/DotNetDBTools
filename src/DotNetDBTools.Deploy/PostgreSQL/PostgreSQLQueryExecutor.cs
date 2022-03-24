using System.Data.Common;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal class PostgreSQLQueryExecutor : QueryExecutor
{
    public PostgreSQLQueryExecutor(DbConnection connection, Events events)
        : base(connection, events)
    {
    }
}
