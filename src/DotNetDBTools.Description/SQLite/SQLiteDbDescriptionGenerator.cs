using DotNetDBTools.Description.Core;
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
