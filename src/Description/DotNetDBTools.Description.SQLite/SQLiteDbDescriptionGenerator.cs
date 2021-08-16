using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Description.SQLite
{
    public static class SQLiteDbDescriptionGenerator
    {
        public static string GenerateDescription(SQLiteDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
