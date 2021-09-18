using System.Collections.Generic;
using DotNetDBTools.Deploy.Common;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class GetExistingTypesQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.Type} = '{MSSQLDbObjectsTypes.UserDefinedType}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
