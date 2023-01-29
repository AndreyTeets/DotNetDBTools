using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.PostgreSQLParser;
using HM = DotNetDBTools.CodeParsing.Core.HelperMethods;

namespace DotNetDBTools.CodeParsing.PostgreSQL;

internal static class PostgreSQLHelperMethods
{
    public static bool TryGetDependency(Data_typeContext context, out Dependency? dependency)
    {
        dependency = null;
        if (context.predefined_type().schema_qualified_name_nontype() is not null)
        {
            string typeName = Unquote(context.predefined_type().schema_qualified_name_nontype().GetText());
            dependency = new Dependency { Type = DependencyType.DataType, Name = typeName };
        }
        return dependency is not null;
    }

    public static bool TryGetDependency(Function_callContext context, out Dependency? dependency)
    {
        dependency = null;
        if (context.schema_qualified_name_for_func_name() is not null)
        {
            string functionName = Unquote(context.schema_qualified_name_for_func_name().GetText());
            if (IsSequenceFunction(functionName))
                dependency = new Dependency { Type = DependencyType.Sequence, Name = GetSequenceName() };
            else
                dependency = new Dependency { Type = DependencyType.FunctionOrProcedure, Name = functionName };
        }
        return dependency is not null;

        string GetSequenceName()
        {
            return PostgreSQLHelperMethods.GetSequenceName(context.vex_or_named_notation()[0].vex());
        }

        bool IsSequenceFunction(string functionName)
        {
            return functionName.ToLower() switch
            {
                "nextval" => true,
                "setval" => true,
                "currval" => true,
                _ => false,
            };
        }
    }

    public static bool TryGetDependency(From_primaryContext context, out Dependency? dependency)
    {
        dependency = null;
        if (context.schema_qualified_name() is not null)
        {
            string tableOrViewName = Unquote(context.schema_qualified_name().GetText());
            dependency = new Dependency { Type = DependencyType.TableOrView, Name = tableOrViewName };
        }
        else if (context.function_call().Any())
        {
            TryGetDependency(context.function_call(0), out dependency);
            if (!dependency.HasValue)
                throw new ParseException($"Failed to get function dependency from [{HM.GetInitialText(context)}]");
        }
        return dependency is not null;
    }

    public static Dependency GetColumnDependency(Indirection_varContext context, out string parentName)
    {
        Stack<string> parts = new();
        parts.Push(Unquote(context.identifier().GetText()));
        if (context.indirection_list() is not null)
        {
            foreach (IndirectionContext item in context.indirection_list().indirection())
                parts.Push(Unquote(item.col_label().GetText()));
        }

        Dependency dependency = new() { Type = DependencyType.Column, Name = parts.Pop() };
        parentName = null;
        if (parts.Count > 0)
            parentName = parts.Pop();
        return dependency;
    }

    public static string GetSequenceName(VexContext funcArgContext)
    {
        string res;
        if (funcArgContext.CAST_EXPRESSION() is not null)
            res = funcArgContext.vex()[0].GetText();
        else
            res = funcArgContext.GetText();
        return res.Replace("'", "");
    }

    public static string RemoveSchemeIfAny(string quotedIdentifier, out string scheme)
    {
        string[] identifierParts = quotedIdentifier.Split('.');
        scheme = identifierParts.Length == 1 ? null : identifierParts[identifierParts.Length - 2];
        return identifierParts[identifierParts.Length - 1];
    }

    public static string Unquote(string quotedIdentifier)
    {
        return quotedIdentifier?.Replace("\"", "");
    }
}
