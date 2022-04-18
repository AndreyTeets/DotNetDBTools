using System.Data;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal class PostgreSQLQueryExecutor : QueryExecutor
{
    public PostgreSQLQueryExecutor(IDbConnection connection, Events events)
        : base(connection, events)
    {
    }
}
