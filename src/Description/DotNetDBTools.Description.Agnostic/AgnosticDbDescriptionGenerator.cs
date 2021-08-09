using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.Description.Agnostic
{
    public static class AgnosticDbDescriptionGenerator
    {
        public static string GenerateDescription(AgnosticDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}
