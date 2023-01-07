using System;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core;

public static class HelperExtensions
{
    public static string GetCode(this Column column) => column.Default.Code;
    public static string GetCode(this CheckConstraint ck) => ck.Expression.Code;
    public static string GetCode(this Trigger trg) => trg.CodePiece.Code;
    public static string GetCode(this View view) => view.CodePiece.Code;
    public static string GetCode(this Script script) => script.CodePiece.Code;

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
