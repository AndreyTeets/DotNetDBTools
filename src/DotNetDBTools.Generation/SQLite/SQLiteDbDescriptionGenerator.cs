using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation.SQLite
{
    internal static class SQLiteDescriptionSourceGenerator
    {
        public static string GenerateDescription(SQLiteDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
