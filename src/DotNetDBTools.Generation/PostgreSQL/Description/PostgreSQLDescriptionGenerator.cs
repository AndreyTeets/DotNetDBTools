using DotNetDBTools.Generation.Core.Description;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Description;

internal static class PostgreSQLDescriptionGenerator
{
    public static string GenerateDescription(PostgreSQLDatabase database, GenerationOptions options)
    {
        return TablesDescriptionGenerator.Create(database, options);
    }
}
