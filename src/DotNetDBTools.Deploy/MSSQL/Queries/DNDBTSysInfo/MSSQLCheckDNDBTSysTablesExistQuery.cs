using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

internal class MSSQLCheckDNDBTSysTablesExistQuery : NoParametersQuery
{
    public override string Sql =>
$@"SELECT TOP 1 1 FROM sys.tables WHERE name IN ({DNDBTSysTables.AllTablesForInClause});";
}
