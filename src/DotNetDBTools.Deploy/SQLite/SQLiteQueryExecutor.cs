using System.Data;
using Dapper;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite;

internal class SQLiteQueryExecutor : QueryExecutor
{
    public SQLiteQueryExecutor(IDbConnection connection, Events events)
        : base(connection, events)
    {
    }

    protected override void BeforeBeginTransaction(IDbConnection connection)
    {
        connection.Execute("PRAGMA foreign_keys=off;");
    }
}
