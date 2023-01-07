using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

internal class MySQLCreateDNDBTSysTablesQuery : NoParametersQuery
{
    public override string Sql =>
$@"CREATE TABLE `{DNDBTSysTables.DNDBTDbAttributes}`
(
    `{DNDBTSysTables.DNDBTDbAttributes.Version}` BIGINT NOT NULL
);

CREATE TABLE `{DNDBTSysTables.DNDBTDbObjects}`
(
    `{DNDBTSysTables.DNDBTDbObjects.ID}` CHAR(36) PRIMARY KEY,
    `{DNDBTSysTables.DNDBTDbObjects.ParentID}` CHAR(36) NULL,
    `{DNDBTSysTables.DNDBTDbObjects.Type}` VARCHAR(32) NOT NULL,
    `{DNDBTSysTables.DNDBTDbObjects.Name}` VARCHAR(256) NOT NULL,
    `{DNDBTSysTables.DNDBTDbObjects.Code}` MEDIUMTEXT NULL
);

CREATE TABLE `{DNDBTSysTables.DNDBTScriptExecutions}`
(
    `{DNDBTSysTables.DNDBTScriptExecutions.ID}` CHAR(36) PRIMARY KEY,
    `{DNDBTSysTables.DNDBTScriptExecutions.Type}` VARCHAR(32) NOT NULL,
    `{DNDBTSysTables.DNDBTScriptExecutions.Name}` VARCHAR(256) NOT NULL,
    `{DNDBTSysTables.DNDBTScriptExecutions.Text}` MEDIUMTEXT NOT NULL,
    `{DNDBTSysTables.DNDBTScriptExecutions.MinDbVersionToExecute}` BIGINT NOT NULL,
    `{DNDBTSysTables.DNDBTScriptExecutions.MaxDbVersionToExecute}` BIGINT NOT NULL,
    `{DNDBTSysTables.DNDBTScriptExecutions.ExecutedOnDbVersion}` BIGINT NOT NULL
);";
}
