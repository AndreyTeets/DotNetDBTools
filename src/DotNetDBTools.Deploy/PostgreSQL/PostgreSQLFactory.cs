using System.Data.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.PostgreSQL.Editors;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal class PostgreSQLFactory : IFactory
{
    public IQueryExecutor CreateQueryExecutor(DbConnection connection, Events events)
    {
        return new PostgreSQLQueryExecutor(connection, events);
    }

    public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
    {
        return new PostgreSQLGenSqlScriptQueryExecutor();
    }

    public IDbModelConverter CreateDbModelConverter()
    {
        return new PostgreSQLDbModelConverter();
    }

    public IDbEditor CreateDbEditor(IQueryExecutor queryExecutor)
    {
        return new PostgreSQLDbEditor(queryExecutor);
    }

    public IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IQueryExecutor queryExecutor)
    {
        return new PostgreSQLDbModelFromDBMSProvider(queryExecutor);
    }
}
