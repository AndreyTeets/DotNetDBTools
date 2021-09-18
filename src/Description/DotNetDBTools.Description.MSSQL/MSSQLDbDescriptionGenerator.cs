using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Description.MSSQL
{
    public static class MSSQLDbDescriptionGenerator
    {
        public static string GenerateDescription(MSSQLDatabaseInfo databaseInfo, string dbName)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo, dbName);
        }
    }
}
