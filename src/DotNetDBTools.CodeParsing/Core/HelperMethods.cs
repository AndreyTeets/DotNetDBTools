using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Models;

namespace DotNetDBTools.CodeParsing.Core;

internal static class HelperMethods
{
    public static string GetInitialTextOrNull(ParserRuleContext context)
    {
        if (context is null)
            return null;
        return GetInitialText(context);
    }

    public static string GetInitialText(ParserRuleContext context)
    {
        return context.Start.InputStream.GetText(
            new Interval(context.Start.StartIndex, context.Stop.StopIndex));
    }

    public static string GetStartLineAndPos(ParserRuleContext context)
    {
        return $"(line={context.Start.Line},pos={context.Start.Column})";
    }

    public static void SetObjectID(
        ObjectInfo objectInfo, string displayedObjectInfoIfMissing, string idDeclarationComment)
    {
        if (idDeclarationComment is null)
            throw new ParseException($"ID declaration comment is missing for {displayedObjectInfoIfMissing}");

        objectInfo.ID = GetObjectID(idDeclarationComment);
    }

    private static Guid GetObjectID(string idDeclarationComment)
    {
        string idDeclStr = idDeclarationComment.Trim();
        int prefixLen = "--ID:#{".Length;
        int postfixLen = "}#".Length;
        string idStr = idDeclStr.Substring(prefixLen, idDeclStr.Length - prefixLen - postfixLen);
        if (Guid.TryParse(idStr, out Guid id))
            return id;
        else
            throw new ParseException($"Failed to parse object id from id declaration comment '{idDeclStr}'");
    }
}
