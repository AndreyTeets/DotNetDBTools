using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL;

internal static class PostgreSQLDescriptionGenerator
{
    public static string GenerateDescription(PostgreSQLDatabase database)
    {
        return TablesDescriptionGenerator.Create(database);
    }
}
