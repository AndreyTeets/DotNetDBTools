using DotNetDBTools.Description.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Description.MSSQL
{
    internal static class MSSQLDescriptionSourceGenerator
    {
        public static string GenerateDescription(MSSQLDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
