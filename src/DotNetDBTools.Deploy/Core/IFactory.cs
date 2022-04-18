using System.Data;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core;

internal interface IFactory
{
    public DatabaseKind GetDatabaseKind();
    public IQueryExecutor CreateQueryExecutor(IDbConnection connection, Events events);
    public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor();
    public IDbEditor CreateDbEditor(IQueryExecutor queryExecutor);
    public IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IQueryExecutor queryExecutor);
}
