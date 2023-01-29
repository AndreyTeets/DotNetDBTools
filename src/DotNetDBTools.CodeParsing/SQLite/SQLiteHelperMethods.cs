using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.SQLiteParser;

namespace DotNetDBTools.CodeParsing.SQLite;

internal static class SQLiteHelperMethods
{
    public static bool TryGetDependency(Table_or_subqueryContext context, out Dependency? dependency)
    {
        dependency = null;
        if (context.table_name() is not null)
        {
            string tableOrViewName = Unquote(context.table_name().GetText());
            dependency = new Dependency { Type = DependencyType.TableOrView, Name = tableOrViewName };
        }
        return dependency is not null;
    }

    public static string Unquote(string quotedIdentifier)
    {
        return quotedIdentifier
            .Replace("[", "").Replace("]", "")
            .Replace("`", "")
            .Replace("\"", "");
    }
}
