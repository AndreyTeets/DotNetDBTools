using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL;

internal static class MySQLDescriptionGenerator
{
    public static string GenerateDescription(MySQLDatabase database, GenerationOptions options)
    {
        return TablesDescriptionGenerator.Create(database, options);
    }
}
