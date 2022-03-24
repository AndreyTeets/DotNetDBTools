using System.Data.Common;
using Dapper;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite;

internal class SQLiteQueryExecutor : QueryExecutor
{
    public SQLiteQueryExecutor(DbConnection connection, Events events)
        : base(connection, events)
    {
    }

    protected override void BeforeBeginTransaction(DbConnection connection)
    {
        connection.Execute("PRAGMA foreign_keys=off;");
    }
}
