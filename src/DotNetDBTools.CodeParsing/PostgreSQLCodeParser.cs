using System;
using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.CodeParsing.PostgreSQL;

namespace DotNetDBTools.CodeParsing;

public class PostgreSQLCodeParser : CodeParser<PostgreSQLParser, PostgreSQLLexer>
{
    public ObjectInfo GetObjectInfo(string input)
    {
        if (ScriptDeclarationParser.TryParseScriptInfo(input, out ScriptInfo scriptInfo))
            return scriptInfo;
        return PostgreSQLGetObjectInfoHelper.ParseFunction(input);
    }

    public List<Dependency> GetFunctionDependencies(string input)
    {
        IParseTree parseTree = Parse(input, x => x.statement());
        PostgreSQLGetFunctionAttributesVisitor visitor = new();
        visitor.Visit(parseTree);

        IParseTree bodyParseTree;
        if (visitor.FunctionLanguage == "SQL" || visitor.FunctionLanguage is null)
            bodyParseTree = Parse(visitor.FunctionBody, x => x.sql_function_def());
        else if (visitor.FunctionLanguage == "PLPGSQL")
            bodyParseTree = Parse(visitor.FunctionBody, x => x.plpgsql_function_def());
        else
            throw new Exception($"Invalid function language '{visitor.FunctionLanguage}'");

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
