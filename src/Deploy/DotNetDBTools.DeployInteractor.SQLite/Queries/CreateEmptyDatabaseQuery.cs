namespace DotNetDBTools.DeployInteractor.SQLite.Queries
{
    internal static class CreateEmptyDatabaseQuery
    {
        public static readonly string Sql =
$@"CREATE TABLE {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID} BLOB PRIMARY KEY,
    {DNDBTSysTables.DNDBTDbObjects.Type} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Name} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Metadata} TEXT NOT NULL
) WITHOUT ROWID;";
    }
}
