using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

internal class MSSQLCreateDNDBTSysTablesQuery : NoParametersQuery
{
    public override string Sql =>
$@"CREATE TABLE [{DNDBTSysTables.DNDBTDbAttributes}]
(
    [{DNDBTSysTables.DNDBTDbAttributes.Version}] BIGINT NOT NULL
);

CREATE TABLE [{DNDBTSysTables.DNDBTDbObjects}]
(
    [{DNDBTSysTables.DNDBTDbObjects.ID}] UNIQUEIDENTIFIER PRIMARY KEY,
    [{DNDBTSysTables.DNDBTDbObjects.ParentID}] UNIQUEIDENTIFIER NULL,
    [{DNDBTSysTables.DNDBTDbObjects.Type}] NVARCHAR(32) NOT NULL,
    [{DNDBTSysTables.DNDBTDbObjects.Name}] NVARCHAR(256) NOT NULL,
    [{DNDBTSysTables.DNDBTDbObjects.Code}] NVARCHAR(MAX) NULL
);

CREATE TABLE [{DNDBTSysTables.DNDBTScriptExecutions}]
(
    [{DNDBTSysTables.DNDBTScriptExecutions.ID}] UNIQUEIDENTIFIER PRIMARY KEY,
    [{DNDBTSysTables.DNDBTScriptExecutions.Type}] NVARCHAR(32) NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.Name}] NVARCHAR(256) NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.Text}] NVARCHAR(MAX) NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.MinDbVersionToExecute}] BIGINT NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.MaxDbVersionToExecute}] BIGINT NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.ExecutedOnDbVersion}] BIGINT NOT NULL
);";
}
