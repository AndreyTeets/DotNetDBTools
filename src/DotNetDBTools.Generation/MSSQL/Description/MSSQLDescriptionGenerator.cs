using DotNetDBTools.Generation.Core.Description;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL.Description;

internal static class MSSQLDescriptionGenerator
{
    public static string GenerateDescription(MSSQLDatabase database, GenerationOptions options)
    {
        return TablesDescriptionGenerator.Create(database, options);
    }
}
