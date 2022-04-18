using System.Data.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Core.Editors;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core;

internal abstract class Factory<
    TQueryExecutor,
    TGenSqlScriptQueryExecutor,
    TDbModelConverter,
    TDbEditor,
    TDbModelFromDBMSProvider>
    : IFactory
    where TQueryExecutor : IQueryExecutor
    where TGenSqlScriptQueryExecutor : IGenSqlScriptQueryExecutor, new()
    where TDbModelConverter : IDbModelConverter, new()
    where TDbEditor : IDbEditor
    where TDbModelFromDBMSProvider : IDbModelFromDBMSProvider
{
    public virtual IQueryExecutor CreateQueryExecutor(DbConnection connection, Events events)
    {
        return Create<TQueryExecutor>(connection, events);
    }

    public virtual IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
    {
        return new TGenSqlScriptQueryExecutor();
    }

    public virtual IDbModelConverter CreateDbModelConverter()
    {
        return new TDbModelConverter();
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
