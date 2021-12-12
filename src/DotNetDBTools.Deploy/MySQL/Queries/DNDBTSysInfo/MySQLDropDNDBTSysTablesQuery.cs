﻿using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo
{
    internal class MySQLDropDNDBTSysTablesQuery : SqlTextOnlyQuery
    {
        public override string Sql =>
$@"DROP TABLE `{DNDBTSysTables.DNDBTDbObjects}`;";
    }
}
