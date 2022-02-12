using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

internal class MySQLCreateDNDBTSysTablesQuery : SqlTextOnlyQuery
{
    public override string Sql =>
$@"CREATE TABLE `{DNDBTSysTables.DNDBTDbObjects}`
(
    `{DNDBTSysTables.DNDBTDbObjects.ID}` CHAR(36) PRIMARY KEY,
    `{DNDBTSysTables.DNDBTDbObjects.ParentID}` CHAR(36) NULL,
    `{DNDBTSysTables.DNDBTDbObjects.Type}` VARCHAR(32) NOT NULL,
    `{DNDBTSysTables.DNDBTDbObjects.Name}` VARCHAR(256) NOT NULL,
    `{DNDBTSysTables.DNDBTDbObjects.Code}` MEDIUMTEXT NULL
);";
}
