using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;
using static DotNetDBTools.CodeParsing.Generated.PostgreSQLParser;

namespace DotNetDBTools.CodeParsing.PostgreSQL
{
    internal class PostgreSQLSplitToStatementsVisitor : PostgreSQLParserBaseVisitor<object>
    {
        private readonly List<string> _statements = new();

        public List<string> GetTopLevelStatements() => _statements;

        public override object VisitStatement([NotNull] StatementContext context)
        {
            _statements.Add(GetInitialText(context));
            return null;
        }

        private static string GetInitialText(ParserRuleContext context)
        {
            return context.Start.InputStream.GetText(
                new Interval(context.Start.StartIndex, context.Stop.StopIndex));
        }
    }
}
