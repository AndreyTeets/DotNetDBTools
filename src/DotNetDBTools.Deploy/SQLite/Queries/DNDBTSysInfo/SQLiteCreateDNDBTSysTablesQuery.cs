using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteCreateDNDBTSysTablesQuery : NoParametersQuery
{
    public override string Sql =>
$@"CREATE TABLE [{DNDBTSysTables.DNDBTDbAttributes}]
(
    [{DNDBTSysTables.DNDBTDbAttributes.Version}] INTEGER NOT NULL
);

CREATE TABLE [{DNDBTSysTables.DNDBTDbObjects}]
(
    [{DNDBTSysTables.DNDBTDbObjects.ID}] BLOB PRIMARY KEY,
    [{DNDBTSysTables.DNDBTDbObjects.ParentID}] BLOB NULL,
    [{DNDBTSysTables.DNDBTDbObjects.Type}] TEXT NOT NULL,
    [{DNDBTSysTables.DNDBTDbObjects.Name}] TEXT NOT NULL,
    [{DNDBTSysTables.DNDBTDbObjects.Code}] TEXT NULL
) WITHOUT ROWID;

CREATE TABLE [{DNDBTSysTables.DNDBTScriptExecutions}]
(
    [{DNDBTSysTables.DNDBTScriptExecutions.ID}] BLOB PRIMARY KEY,
    [{DNDBTSysTables.DNDBTScriptExecutions.Type}] TEXT NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.Name}] TEXT NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.Code}] TEXT NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.MinDbVersionToExecute}] INTEGER NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.MaxDbVersionToExecute}] INTEGER NOT NULL,
    [{DNDBTSysTables.DNDBTScriptExecutions.ExecutedOnDbVersion}] INTEGER NOT NULL
) WITHOUT ROWID;";
}
