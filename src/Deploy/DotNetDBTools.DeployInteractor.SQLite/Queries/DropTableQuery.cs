using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DeployInteractor.SQLite.Queries
{
    internal static class DropTableQuery
    {
        public static string Sql(SQLiteTableInfo table)
        {
            string query =
$@"DROP TABLE {table.Name};

DELETE FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.ID} = '{table.ID}';";

            return query;
        }
    }
}
