using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

internal class MSSQLCheckDNDBTSysTablesExistQuery : SqlTextOnlyQuery
{
    public override string Sql =>
$@"SELECT TOP 1 1 FROM sys.tables WHERE name = '{DNDBTSysTables.DNDBTDbObjects}';";
}
