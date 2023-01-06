using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

internal class PostgreSQLCreateDNDBTSysTablesQuery : NoParametersQuery
{
    public override string Sql =>
$@"CREATE TABLE ""{DNDBTSysTables.DNDBTDbAttributes}""
(
    ""{DNDBTSysTables.DNDBTDbAttributes.Version}"" BIGINT NOT NULL
);

CREATE TABLE ""{DNDBTSysTables.DNDBTDbObjects}""
(
    ""{DNDBTSysTables.DNDBTDbObjects.ID}"" UUID PRIMARY KEY,
    ""{DNDBTSysTables.DNDBTDbObjects.ParentID}"" UUID NULL,
    ""{DNDBTSysTables.DNDBTDbObjects.Type}"" VARCHAR(32) NOT NULL,
    ""{DNDBTSysTables.DNDBTDbObjects.Name}"" VARCHAR(256) NOT NULL,
    ""{DNDBTSysTables.DNDBTDbObjects.Code}"" TEXT NULL
);

CREATE TABLE ""{DNDBTSysTables.DNDBTScriptExecutions}""
(
    ""{DNDBTSysTables.DNDBTScriptExecutions.ID}"" UUID PRIMARY KEY,
    ""{DNDBTSysTables.DNDBTScriptExecutions.Type}"" VARCHAR(32) NOT NULL,
    ""{DNDBTSysTables.DNDBTScriptExecutions.Name}"" VARCHAR(256) NOT NULL,
    ""{DNDBTSysTables.DNDBTScriptExecutions.Code}"" TEXT NOT NULL,
    ""{DNDBTSysTables.DNDBTScriptExecutions.MinDbVersionToExecute}"" BIGINT NOT NULL,
    ""{DNDBTSysTables.DNDBTScriptExecutions.MaxDbVersionToExecute}"" BIGINT NOT NULL,
    ""{DNDBTSysTables.DNDBTScriptExecutions.ExecutedOnDbVersion}"" BIGINT NOT NULL
);";
}
