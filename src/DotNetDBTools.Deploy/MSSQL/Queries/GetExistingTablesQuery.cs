using System.Collections.Generic;
using DotNetDBTools.Deploy.Shared;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class GetExistingTablesQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    {DNDBTSystemTables.DNDBTDbObjects.Metadata}
FROM {DNDBTSystemTables.DNDBTDbObjects}
WHERE {DNDBTSystemTables.DNDBTDbObjects.Type} = '{MSSQLDbObjectsTypes.Table}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
