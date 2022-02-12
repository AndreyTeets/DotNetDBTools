using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteCreateDNDBTSysTablesQuery : SqlTextOnlyQuery
{
    public override string Sql =>
$@"CREATE TABLE {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID} BLOB PRIMARY KEY,
    {DNDBTSysTables.DNDBTDbObjects.ParentID} BLOB NULL,
    {DNDBTSysTables.DNDBTDbObjects.Type} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Name} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Code} TEXT NULL
) WITHOUT ROWID;";
}
