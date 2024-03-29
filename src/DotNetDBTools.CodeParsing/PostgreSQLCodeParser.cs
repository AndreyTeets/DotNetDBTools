﻿using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.CodeParsing.PostgreSQL;

namespace DotNetDBTools.CodeParsing;

public class PostgreSQLCodeParser : CodeParser<PostgreSQLParser, PostgreSQLLexer>
{
    /// <inheritdoc />
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

                triggerInfoRes.CreateStatement = $"{expectedCreateFunctionStatement}{triggerInfoRes.CreateStatement}";
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
    /// Parses the list of tables|views|functions referenced in the provided create view statement.
    /// </summary>
    public List<Dependency> GetViewDependencies(string createViewStatement)
    {
        IParseTree parseTree = Parse(createViewStatement, x => x.create_view_statement());
        PostgreSQLGetViewDependenciesVisitor visitor = new();
        parseTree.Accept(visitor);
        return visitor.GetDependencies();
    }

    /// <summary>
    /// Parses the list of tables|views|functions|procedures referenced in the provided create function statement.
    /// </summary>
    public List<Dependency> GetFunctionDependencies(string createFunctionStatement, out string language)
    {
        IParseTree parseTree = Parse(createFunctionStatement, x => x.create_function_statement());
        PostgreSQLGetFunctionAttributesVisitor visitor = new();
        visitor.Visit(parseTree);
        language = visitor.FunctionLanguage;

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
    /// Parses the list of tables|views|functions|procedures referenced in the provided create procedure statement.
    /// </summary>
    public List<Dependency> GetProcedureDependencies(string createProcedureStatement, out string language)
    {
        return GetFunctionDependencies(createProcedureStatement, out language);
    }

    /// <summary>
    /// Parses the name of executed function in the provided create trigger statement.
    /// </summary>
    public List<Dependency> GetTriggerDependencies(string createTriggerStatement)
    {
        IParseTree parseTree = Parse(createTriggerStatement, x => x.create_trigger_statement());
        PostgreSQLGetTriggerDependenciesVisitor visitor = new();
        parseTree.Accept(visitor);
        return visitor.GetDependencies();
    }

    /// <summary>
    /// Parses the list of functions|sequences referenced in the provided expression statement.
    /// </summary>
    public List<Dependency> GetExpressionDependencies(string expressionStatement)
    {
        IParseTree parseTree = Parse(expressionStatement, x => x.vex());
        PostgreSQLGetExpressionDependenciesVisitor visitor = new();
        parseTree.Accept(visitor);
        return visitor.GetDependencies();
    }
}
