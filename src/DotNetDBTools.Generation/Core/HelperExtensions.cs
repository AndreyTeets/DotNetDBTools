using System;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core;

public static class HelperExtensions
{
    public static string GetDefault(this Column column) => column.Default?.Code;
    public static string GetExpression(this CheckConstraint ck) => ck.Expression.Code;
    public static string GetCreateStatement(this Trigger trg) => trg.CreateStatement.Code;
    public static string GetCreateStatement(this View view) => view.CreateStatement.Code;
    public static string GetText(this Script script) => script.Text.Code;

    // TODO refactor objects code to separate table DNDBTDbObjectsCode(ObjectID,CodeType,CodeText,PK(ObjectID,CodeType))
    public static string GetCode(this Index index) => index is Models.PostgreSQL.PostgreSQLIndex idx
        ? idx.Expression?.Code
        : null;

    public static string AppendSemicolonIfAbsent(this string val)
    {
        if (!val.EndsWith(";", StringComparison.Ordinal))
            return $"{val};";
        else
            return val;
    }

    public static string NormalizeLineEndings(this string value)
    {
        return value.Replace("\r\n", "\n").Trim();
    }
}
