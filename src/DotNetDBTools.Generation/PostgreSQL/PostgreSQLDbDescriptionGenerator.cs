using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL;

internal static class PostgreSQLDescriptionGenerator
{
    public static string GenerateDescription(PostgreSQLDatabase database, GenerationOptions options)
    {
        return TablesDescriptionGenerator.Create(database, options);
    }
}
