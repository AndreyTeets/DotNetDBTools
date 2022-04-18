using System.Data;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core;

internal abstract class Factory<
    TQueryExecutor,
    TGenSqlScriptQueryExecutor,
    TDbEditor,
    TDbModelFromDBMSProvider>
    : IFactory
    where TQueryExecutor : IQueryExecutor
    where TGenSqlScriptQueryExecutor : IGenSqlScriptQueryExecutor, new()
    where TDbEditor : IDbEditor
    where TDbModelFromDBMSProvider : IDbModelFromDBMSProvider
{
    private readonly DatabaseKind _databaseKind;

    protected Factory(DatabaseKind databaseKind)
    {
        _databaseKind = databaseKind;
    }

    public virtual DatabaseKind GetDatabaseKind()
    {
        return _databaseKind;
    }

    public virtual IQueryExecutor CreateQueryExecutor(IDbConnection connection, Events events)
    {
        return Create<TQueryExecutor>(connection, events);
    }

    public virtual IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
    {
        return new TGenSqlScriptQueryExecutor();
    }

    public virtual IDbEditor CreateDbEditor(IQueryExecutor queryExecutor)
    {
        return Create<TDbEditor>(queryExecutor);
    }

    public virtual IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IQueryExecutor queryExecutor)
    {
        return Create<TDbModelFromDBMSProvider>(queryExecutor);
    }
}
