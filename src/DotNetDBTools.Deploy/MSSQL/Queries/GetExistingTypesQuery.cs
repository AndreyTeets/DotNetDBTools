using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class GetExistingTypesQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    {DNDBTSystemTables.DNDBTDbObjects.Metadata}
FROM {DNDBTSystemTables.DNDBTDbObjects}
WHERE {DNDBTSystemTables.DNDBTDbObjects.Type} = '{MSSQLDbObjectsTypes.UserDefinedType}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
