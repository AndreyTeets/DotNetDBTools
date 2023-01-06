using DotNetDBTools.Generation.Core.Description;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL.Description;

internal static class MySQLDescriptionGenerator
{
    public static string GenerateDescription(MySQLDatabase database, GenerationOptions options)
    {
        return TablesDescriptionGenerator.Create(database, options);
    }
}
