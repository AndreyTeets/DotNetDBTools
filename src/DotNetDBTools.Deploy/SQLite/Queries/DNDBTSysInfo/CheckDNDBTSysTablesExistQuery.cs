using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo
{
    internal class CheckDNDBTSysTablesExistQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    true
FROM sqlite_master
WHERE type = 'table' AND name = '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
