using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

internal class MSSQLDropDNDBTSysTablesQuery : NoParametersQuery
{
    public override string Sql =>
$@"DROP TABLE [{DNDBTSysTables.DNDBTDbAttributes}];
DROP TABLE [{DNDBTSysTables.DNDBTDbObjects}];
DROP TABLE [{DNDBTSysTables.DNDBTScriptExecutions}];";
}
