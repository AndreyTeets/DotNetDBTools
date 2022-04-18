using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Editors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL;

internal class MSSQLFactory : Factory<
    MSSQLQueryExecutor,
    MSSQLGenSqlScriptQueryExecutor,
    MSSQLDbEditor,
    MSSQLDbModelFromDBMSProvider>
{
    public MSSQLFactory() : base(DatabaseKind.MSSQL) { }
}
