using DotNetDBTools.Description.Common;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.Description.Agnostic
{
    public static class AgnosticDescriptionSourceGenerator
    {
        public static string GenerateDescription(AgnosticDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
