namespace DotNetDBTools.DeployInteractor.SQLite.Queries
{
    internal static class GetExistingTablesQuery
    {
        public static readonly string Sql =
$@"SELECT
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.Type} = '{SQLiteDbObjectsTypes.Table}';";
    }
}
