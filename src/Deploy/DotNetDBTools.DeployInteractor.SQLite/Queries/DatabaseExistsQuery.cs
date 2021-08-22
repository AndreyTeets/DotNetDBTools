namespace DotNetDBTools.DeployInteractor.SQLite.Queries
{
    internal static class DatabaseExistsQuery
    {
        public static readonly string Sql =
$@"SELECT
    true
FROM sqlite_master 
WHERE type = 'table' AND name = '{DNDBTSysTables.DNDBTDbObjects}';";
    }
}
