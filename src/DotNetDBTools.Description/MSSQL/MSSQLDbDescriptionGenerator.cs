using DotNetDBTools.Description.Shared;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Description.MSSQL
{
    public static class MSSQLDescriptionSourceGenerator
    {
        public static string GenerateDescription(MSSQLDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
