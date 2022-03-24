using System.Data.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MSSQL.Editors;

namespace DotNetDBTools.Deploy.MSSQL;

internal class MSSQLFactory : IFactory
{
    public IQueryExecutor CreateQueryExecutor(DbConnection connection, Events events)
    {
        return new MSSQLQueryExecutor(connection, events);
    }

    public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
    {
        return new MSSQLGenSqlScriptQueryExecutor();
    }

    public IDbModelConverter CreateDbModelConverter()
    {
        return new MSSQLDbModelConverter();
    }

    public IDbEditor CreateDbEditor(IQueryExecutor queryExecutor)
    {
        return new MSSQLDbEditor(queryExecutor);
    }

    public IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IQueryExecutor queryExecutor)
    {
        return new MSSQLDbModelFromDBMSProvider(queryExecutor);
    }
}
