using System.Data;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MySQL;

internal class MySQLQueryExecutor : QueryExecutor
{
    public MySQLQueryExecutor(IDbConnection connection, Events events)
        : base(connection, events)
    {
    }
}
