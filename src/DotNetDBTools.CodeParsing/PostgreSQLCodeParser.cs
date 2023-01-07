using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.CodeParsing.PostgreSQL;

namespace DotNetDBTools.CodeParsing;

public class PostgreSQLCodeParser : CodeParser<PostgreSQLParser, PostgreSQLLexer>
{
    public override ObjectInfo GetObjectInfo(string createStatement)
    {
        if (ScriptDeclarationParser.TryParseScriptInfo(createStatement, out ScriptInfo scriptInfo))
            return scriptInfo;
        else if (ParseTriggerInfoWhenTwoStatements(createStatement, out TriggerInfo triggerInfo))
            return triggerInfo;
        return ParseObjectInfo<PostgreSQLGetObjectInfoVisitor>(createStatement, x => x.dndbt_sqldef_create_statement());

        bool ParseTriggerInfoWhenTwoStatements(string input, out TriggerInfo triggerInfo)
        {
            List<string> statements = PostgreSQLStatementsSplitter.Split(input);
            if (statements.Count == 2)
            {
                string expectedCreateFunctionStatement = statements[0];
                string expectedCreateTriggerStatement = statements[1];

                ObjectInfo objectInfo = ParseObjectInfo<PostgreSQLGetObjectInfoVisitor>(
                    expectedCreateTriggerStatement, x => x.dndbt_sqldef_create_statement());
                if (objectInfo is not TriggerInfo triggerInfoRes)
                    throw new ParseException($"Trigger object code contains 2 statements and second one is not a trigger\ninput=[{input}]");

                triggerInfoRes.Code = $"{expectedCreateFunctionStatement}{triggerInfoRes.Code}";
                triggerInfo = triggerInfoRes;
                return true;
            }
            else
            {
                triggerInfo = null;
                return false;
            }
        }
    }

    /// <summary>
    /// Parses the list of functions|views|tables referenced in the provided create function statement.
    /// </summary>
    public List<Dependency> GetFunctionDependencies(string createFunctionStatement)
    {
        IParseTree parseTree = Parse(createFunctionStatement, x => x.create_function_statement());
        PostgreSQLGetFunctionAttributesVisitor visitor = new();
        visitor.Visit(parseTree);

        IParseTree bodyParseTree;
        if (visitor.FunctionLanguage == "SQL" || visitor.FunctionLanguage is null)
            bodyParseTree = Parse(visitor.FunctionBody, x => x.sql_function_def());
        else if (visitor.FunctionLanguage == "PLPGSQL")
            bodyParseTree = Parse(visitor.FunctionBody, x => x.plpgsql_function_def());
        else
            throw new ParseException($"Invalid function language '{visitor.FunctionLanguage}'");

        PostgreSQLGetFunctionDependenciesVisitor bodyVisitor = new();
        bodyVisitor.Visit(bodyParseTree);
        return bodyVisitor.GetDependencies();
    }

    /// <summary>
    /// Parses the list of functions|views|tables referenced in the provided create view statement.
    /// </summary>
    public List<Dependency> GetViewDependencies(string createViewStatement)
    {
        IParseTree parseTree = Parse(createViewStatement, x => x.create_view_statement());
        PostgreSQLGetViewDependenciesVisitor visitor = new();
        parseTree.Accept(visitor);
        return visitor.GetDependencies();
    }
}
