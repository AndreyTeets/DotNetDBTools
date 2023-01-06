using DotNetDBTools.Generation.Core.Description;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation.SQLite.Description;

internal static class SQLiteDescriptionGenerator
{
    public static string GenerateDescription(SQLiteDatabase database, GenerationOptions options)
    {
        return TablesDescriptionGenerator.Create(database, options);
    }
}
