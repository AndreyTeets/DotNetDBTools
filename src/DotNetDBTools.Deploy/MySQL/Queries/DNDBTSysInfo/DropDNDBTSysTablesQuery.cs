﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo
{
    internal class DropDNDBTSysTablesQuery : IQuery
    {
        public string Sql =>
$@"DROP TABLE `{DNDBTSysTables.DNDBTDbObjects}`;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}