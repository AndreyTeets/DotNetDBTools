using DotNetDBTools.Generation.Core.Description;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.Generation.Agnostic.Description;

internal static class AgnosticDescriptionGenerator
{
    public static string GenerateDescription(AgnosticDatabase database, GenerationOptions options)
    {
        return TablesDescriptionGenerator.Create(database, options);
    }
}
