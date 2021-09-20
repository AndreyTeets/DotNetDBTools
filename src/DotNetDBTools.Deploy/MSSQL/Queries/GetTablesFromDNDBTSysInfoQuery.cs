﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class GetTablesFromDNDBTSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.Type} = '{MSSQLDbObjectsTypes.Table}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
