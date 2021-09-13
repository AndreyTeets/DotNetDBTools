using System.Collections.Generic;

namespace DotNetDBTools.DeployInteractor.SQLite.Queries
{
    internal class DatabaseExistsQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    true
FROM sqlite_master
WHERE type = 'table' AND name = '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
