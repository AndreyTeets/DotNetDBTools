using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo
{
    internal class PostgreSQLDropDNDBTSysTablesQuery : SqlTextOnlyQuery
    {
        public override string Sql =>
$@"DROP TABLE ""{DNDBTSysTables.DNDBTDbObjects}"";";
    }
}
