using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class CreateSystemTablesQuery : IQuery
    {
        public string Sql =>
$@"CREATE TABLE {DNDBTSystemTables.DNDBTDbObjects}
(
    {DNDBTSystemTables.DNDBTDbObjects.ID} BLOB PRIMARY KEY,
    {DNDBTSystemTables.DNDBTDbObjects.Type} TEXT NOT NULL,
    {DNDBTSystemTables.DNDBTDbObjects.Name} TEXT NOT NULL,
    {DNDBTSystemTables.DNDBTDbObjects.Metadata} TEXT NOT NULL
) WITHOUT ROWID;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
