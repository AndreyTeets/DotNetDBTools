using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo
{
    internal class CreateDNDBTSysTablesQuery : IQuery
    {
        public string Sql =>
$@"CREATE TABLE {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID} BLOB PRIMARY KEY,
    {DNDBTSysTables.DNDBTDbObjects.ParentID} BLOB NULL,
    {DNDBTSysTables.DNDBTDbObjects.Type} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Name} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.ExtraInfo} TEXT NULL
) WITHOUT ROWID;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
