using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;

namespace DotNetDBTools.CodeParsing.PostgreSQL;

public class PostgreSQLCodeParser : CodeParser<PostgreSQLParser, PostgreSQLLexer>
{
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
}
