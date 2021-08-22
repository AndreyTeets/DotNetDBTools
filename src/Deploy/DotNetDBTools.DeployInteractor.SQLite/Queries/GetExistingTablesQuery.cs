﻿using System.Collections.Generic;

namespace DotNetDBTools.DeployInteractor.SQLite.Queries
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
