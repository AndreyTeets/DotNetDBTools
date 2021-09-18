using DotNetDBTools.Description.Common;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Description.SQLite
{
    public static class SQLiteDescriptionSourceGenerator
    {
        public static string GenerateDescription(SQLiteDatabaseInfo databaseInfo, string dbName)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo, dbName);
        }
    }
}
