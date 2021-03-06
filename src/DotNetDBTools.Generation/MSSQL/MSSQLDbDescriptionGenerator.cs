using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL;

internal static class MSSQLDescriptionGenerator
{
    public static string GenerateDescription(MSSQLDatabase database, GenerationOptions options)
    {
        return TablesDescriptionGenerator.Create(database, options);
    }
}
