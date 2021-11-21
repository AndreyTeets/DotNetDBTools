using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo
{
    internal class CreateDNDBTSysTablesQuery : IQuery
    {
        public string Sql =>
$@"CREATE TABLE `{DNDBTSysTables.DNDBTDbObjects}`
(
    `{DNDBTSysTables.DNDBTDbObjects.ID}` CHAR(36) PRIMARY KEY,
    `{DNDBTSysTables.DNDBTDbObjects.ParentID}` CHAR(36) NULL,
    `{DNDBTSysTables.DNDBTDbObjects.Type}` VARCHAR(32) NOT NULL,
    `{DNDBTSysTables.DNDBTDbObjects.Name}` VARCHAR(256) NOT NULL,
    `{DNDBTSysTables.DNDBTDbObjects.ExtraInfo}` MEDIUMTEXT NULL
);";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
