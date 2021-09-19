using System.Collections.Generic;
using DotNetDBTools.Deploy.Shared;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class GetExistingTablesQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.Type} = '{SQLiteDbObjectsTypes.Table}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
