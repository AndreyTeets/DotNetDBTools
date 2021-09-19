using System.Collections.Generic;
using DotNetDBTools.Deploy.Shared;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class GetExistingTablesQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    {DNDBTSystemTables.DNDBTDbObjects.Metadata}
FROM {DNDBTSystemTables.DNDBTDbObjects}
WHERE {DNDBTSystemTables.DNDBTDbObjects.Type} = '{SQLiteDbObjectsTypes.Table}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
