using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class DatabaseExistsQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    true
FROM sqlite_master
WHERE type = 'table' AND name = '{DNDBTSystemTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
