using DotNetDBTools.Description.Shared;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Description.SQLite
{
    public static class SQLiteDescriptionSourceGenerator
    {
        public static string GenerateDescription(SQLiteDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
