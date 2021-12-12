using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo
{
    internal class PostgreSQLCreateDNDBTSysTablesQuery : SqlTextOnlyQuery
    {
        public override string Sql =>
$@"CREATE TABLE ""{DNDBTSysTables.DNDBTDbObjects}""
(
    ""{DNDBTSysTables.DNDBTDbObjects.ID}"" UUID PRIMARY KEY,
    ""{DNDBTSysTables.DNDBTDbObjects.ParentID}"" UUID NULL,
    ""{DNDBTSysTables.DNDBTDbObjects.Type}"" VARCHAR(32) NOT NULL,
    ""{DNDBTSysTables.DNDBTDbObjects.Name}"" VARCHAR(256) NOT NULL,
    ""{DNDBTSysTables.DNDBTDbObjects.ExtraInfo}"" TEXT NULL
);";
    }
}
