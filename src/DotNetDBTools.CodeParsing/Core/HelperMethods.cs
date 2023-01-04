using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace DotNetDBTools.CodeParsing.Core;

internal static class HelperMethods
{
    public static string GetInitialText(ParserRuleContext context)
    {
        return context.Start.InputStream.GetText(
            new Interval(context.Start.StartIndex, context.Stop.StopIndex));
    }
}
