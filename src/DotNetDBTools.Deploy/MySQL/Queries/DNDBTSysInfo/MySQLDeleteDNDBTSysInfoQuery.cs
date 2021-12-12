using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo
{
    internal class MySQLDeleteDNDBTSysInfoQuery : DeleteDNDBTSysInfoQuery
    {
        public MySQLDeleteDNDBTSysInfoQuery(Guid objectID)
            : base(objectID) { }

        protected override string GetSql(Guid objectID)
        {
            string query =
$@"DELETE FROM `{DNDBTSysTables.DNDBTDbObjects}`
WHERE `{DNDBTSysTables.DNDBTDbObjects.ID}` = '{objectID}';";

            return query;
        }
    }
}
