using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.UnitTests.CodeParsing.Base;
using static DotNetDBTools.CodeParsing.Generated.PostgreSQLParser;

namespace DotNetDBTools.UnitTests.CodeParsing.PostgreSQL;

public class PostgreSQLGrammarTests : BaseGrammarTests<PostgreSQLParser, PostgreSQLLexer>
{
    protected override string TestDataDir => "../../../TestData/PostgreSQL/Grammar/examples";
    protected override Func<PostgreSQLParser, IParseTree> ListOfStatementsStartRule => x => x.sql();
    protected override Action<TestCodeParser, IParseTree> DoAdditionalParsing => (x, y) => ParseAllBodies(x, y);
    private Func<PostgreSQLParser, IParseTree> SqlFunctionBodyStartRule => x => x.sql_function_def();
    private Func<PostgreSQLParser, IParseTree> PlPgSqlFunctionBodyStartRule => x => x.plpgsql_function_def();
    private Func<PostgreSQLParser, IParseTree> AnonymousBlockBodyStartRule => x => x.function_block();
    private Func<PostgreSQLParser, IParseTree> ExecuteBodyStartRule => x => x.sql();

    private void ParseAllBodies(TestCodeParser parser, IParseTree parseTree, string prevLinesStackTrace = "")
    {
        TestExtractBodiesVisitor visitor = new();
        visitor.Visit(parseTree);

        foreach ((string lang, string text, string line) body in visitor.Bodies)
        {
            if (body.text is null)
            {
                throw new AdditionalParseException(
$"Function body is null (lang={body.lang} (line={CurrentLinesStackTrace(body.line)})");
            }

            try
            {
                if (body.lang == "SQL" || body.lang is null)
                {
                    IParseTree bodyParseTree = parser.ParseToTree(body.text, SqlFunctionBodyStartRule);
                    ParseAllBodies(parser, bodyParseTree, CurrentLinesStackTrace(body.line));
                }
                else if (body.lang == "PLPGSQL")
                {
                    IParseTree bodyParseTree = parser.ParseToTree(body.text, PlPgSqlFunctionBodyStartRule);
                    ParseAllBodies(parser, bodyParseTree, CurrentLinesStackTrace(body.line));
                }
                else if (body.lang == "AnonymousBlock")
                {
                    IParseTree bodyParseTree = parser.ParseToTree(body.text, AnonymousBlockBodyStartRule);
                    ParseAllBodies(parser, bodyParseTree, CurrentLinesStackTrace(body.line));
                }
                else if (body.lang == "ExecuteBody")
                {
                    IParseTree bodyParseTree = parser.ParseToTree(body.text, ExecuteBodyStartRule);
                    ParseAllBodies(parser, bodyParseTree, CurrentLinesStackTrace(body.line));
                }
                else if (body.lang != "INTERNAL" && body.lang != "C")
                {
                    throw new AdditionalParseException(
$"Invalid lang={body.lang} (line={CurrentLinesStackTrace(body.line)})");
                }
            }
            catch (ParseException ex)
            {
                throw new AdditionalParseException(
$"Failed to parse body(lang={body.lang}, line={CurrentLinesStackTrace(body.line)}): {ex.Message}");
            }
        }

        string CurrentLinesStackTrace(string bodyLine) => prevLinesStackTrace == ""
            ? bodyLine
            : $"{prevLinesStackTrace}->{bodyLine}";
    }

    private class TestExtractBodiesVisitor : PostgreSQLParserBaseVisitor<object>
    {
        public List<(string lang, string text, string line)> Bodies { get; set; } = new();

        public override object VisitCreate_function_statement([NotNull] Create_function_statementContext context)
        {
            string lang = null;
            string text = null;
            string line = $"{context.Start.Line}";

            if (context.function_body() is not null)
                text = HelperMethods.GetInitialText(context.function_body());

            foreach (Function_actions_commonContext actContext in context.function_actions_common())
            {
                if (actContext.function_def() is not null)
                {
                    if (actContext.function_def().character_string(0).BeginDollarStringConstant() is not null)
                        text = ExtractTextFromDollarConstant(actContext.function_def().character_string(0));
                    else
                        text = ExtractTextFromSimpleStringConstant(actContext.function_def().character_string(0));
                }
                else if (actContext.lang_name is not null)
                {
                    lang = actContext.lang_name.GetText().ToUpper();
                    if (actContext.lang_name.Character_String_Literal() is not null)
                        lang = lang.Substring(1, lang.Length - 2).Replace("''", "'");
                }
            }

            Bodies.Add((lang, text, line));

            return base.VisitCreate_function_statement(context);
        }

        public override object VisitAnonymous_block([NotNull] Anonymous_blockContext context)
        {
            string lang = "AnonymousBlock";
            string text;
            string line = $"{context.Start.Line}";
            if (context.character_string(0).BeginDollarStringConstant() is not null)
                text = ExtractTextFromDollarConstant(context.character_string(0));
            else
                text = ExtractTextFromSimpleStringConstant(context.character_string(0));
            Bodies.Add((lang, text, line));

            return base.VisitAnonymous_block(context);
        }

        public override object VisitExecute_stmt([NotNull] Execute_stmtContext context)
        {
            Character_stringContext str = context.vex()?.value_expression_primary()?.unsigned_value_specification()?.character_string();
            if (str is not null)
            {
                string lang = "ExecuteBody";
                string text;
                string line = $"{context.Start.Line}";
                if (str.BeginDollarStringConstant() is not null)
                    text = ExtractTextFromDollarConstant(str);
                else
                    text = ExtractTextFromSimpleStringConstant(str);
                Bodies.Add((lang, text, line));
            }

            return base.VisitExecute_stmt(context);
        }

        private static string ExtractTextFromDollarConstant(Character_stringContext context)
        {
            string dollarConstant = context.BeginDollarStringConstant().GetText();
            string quotedFuncBody = context.GetText();
            return quotedFuncBody.Substring(dollarConstant.Length, quotedFuncBody.Length - 2 * dollarConstant.Length);
        }

        private static string ExtractTextFromSimpleStringConstant(Character_stringContext context)
        {
            string quotedFuncBody = context.GetText();
            return quotedFuncBody.Substring(1, quotedFuncBody.Length - 2).Replace("''", "'");
        }
    }
}
