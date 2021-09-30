using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.Generation.Agnostic
{
    internal static class AgnosticDescriptionSourceGenerator
    {
        public static string GenerateDescription(AgnosticDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
