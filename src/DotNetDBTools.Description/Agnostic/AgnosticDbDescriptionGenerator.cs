using DotNetDBTools.Description.Core;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.Description.Agnostic
{
    internal static class AgnosticDescriptionSourceGenerator
    {
        public static string GenerateDescription(AgnosticDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
