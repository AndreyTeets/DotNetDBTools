using System.Data;
using Dapper;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL;

internal class MSSQLQueryExecutor : QueryExecutor
{
    public MSSQLQueryExecutor(IDbConnection connection, Events events)
        : base(connection, events)
    {
    }

    protected override void BeforeBeginTransaction(IDbConnection connection)
    {
        connection.Execute("SET XACT_ABORT ON;");
    }
}
