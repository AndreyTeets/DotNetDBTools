using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;

namespace DotNetDBTools.CodeParsing.PostgreSQL
{
    public class PostgreSQLCodeParser
    {
        private readonly ErrorListener _errorListener = new();

        public List<string> SplitToStatements(string input)
        {
            IParseTree parseTree = Parse(input, x => x.sql());
            PostgreSQLSplitToStatementsVisitor visitor = new();
            parseTree.Accept(visitor);
            return visitor.GetTopLevelStatements();
        }

        public List<Dependency> GetFunctionDependencies(string input)
        {
            IParseTree parseTree = Parse(input, x => x.statement());
            PostgreSQLGetFunctionAttributesVisitor visitor = new();
            visitor.Visit(parseTree);

            IParseTree bodyParseTree = visitor.FunctionLanguage == "SQL"
                ? Parse(visitor.FunctionBody, x => x.statement())
                : Parse(visitor.FunctionBody, x => x.plpgsql_function());
            PostgreSQLGetFunctionDependenciesVisitor bodyVisitor = new();
            bodyVisitor.Visit(bodyParseTree);
            return bodyVisitor.GetDependencies();
        }

        public List<Dependency> GetViewDependencies(string input)
        {
            IParseTree parseTree = Parse(input, x => x.statement());
            PostgreSQLGetViewDependenciesVisitor visitor = new();
            parseTree.Accept(visitor);
            return visitor.GetDependencies();
        }

        private IParseTree Parse(string input, Func<PostgreSQLParser, IParseTree> startRule)
        {
            PostgreSQLParser parser = CreateParser(input);
            IParseTree parseTree = startRule(parser);
            ProcessErrors();
            return parseTree;
        }

        private void ProcessErrors()
        {
            List<string> errors = _errorListener.GetErrors();
            if (errors.Count > 0)
            {
                string errorMsg = string.Join("\n", errors);
                _errorListener.ClearErrors();
                throw new ParseException(errorMsg);
            }
        }

        private PostgreSQLParser CreateParser(string input)
        {
            ICharStream charStream = CharStreams.fromString(input);
            Lexer lexer = new PostgreSQLLexer(charStream);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(_errorListener);

            ITokenStream tokenStream = new CommonTokenStream(lexer);
            PostgreSQLParser parser = new(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(_errorListener);

            return parser;
        }
    }
}
