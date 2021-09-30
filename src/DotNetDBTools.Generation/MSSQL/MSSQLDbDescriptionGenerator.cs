using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL
{
    internal static class MSSQLDescriptionSourceGenerator
    {
        public static string GenerateDescription(MSSQLDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
