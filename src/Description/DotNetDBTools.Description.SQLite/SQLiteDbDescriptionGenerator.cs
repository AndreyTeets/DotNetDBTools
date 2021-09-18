using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Description.SQLite
{
    public static class SQLiteDbDescriptionGenerator
    {
        public static string GenerateDescription(SQLiteDatabaseInfo databaseInfo, string dbName)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo, dbName);
        }
    }
}
