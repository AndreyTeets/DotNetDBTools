using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation.SQLite;

internal static class SQLiteDescriptionGenerator
{
    public static string GenerateDescription(SQLiteDatabase database, GenerationOptions options)
    {
        return TablesDescriptionGenerator.Create(database, options);
    }
}
