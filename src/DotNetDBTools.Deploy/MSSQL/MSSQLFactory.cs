using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Editors;

namespace DotNetDBTools.Deploy.MSSQL;

internal class MSSQLFactory : Factory<
    MSSQLQueryExecutor,
    MSSQLGenSqlScriptQueryExecutor,
    MSSQLDbModelConverter,
    MSSQLDbEditor,
    MSSQLDbModelFromDBMSProvider>
{
}
