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
        if (context.predefined_type().schema_qualified_name_nontype() != null)
        {
            string typeName = Unquote(context.predefined_type().schema_qualified_name_nontype().GetText());
            dependency = new Dependency { Type = DependencyType.DataType, Name = typeName };
        }
        return dependency != null;
    }

    public static bool TryGetDependency(Function_callContext context, out Dependency? dependency)
    {
        dependency = null;
        if (context.schema_qualified_name_for_func_name() != null)
        {
            string functionName = Unquote(context.schema_qualified_name_for_func_name().GetText());
            if (functionName.ToLower() == "nextval")
                dependency = new Dependency { Type = DependencyType.Sequence, Name = GetSequenceName() };
            else
                dependency = new Dependency { Type = DependencyType.Function, Name = functionName };
        }
        return dependency != null;

        string GetSequenceName()
        {
            return PostgreSQLHelperMethods.GetSequenceName(context.vex_or_named_notation()[0].vex());
        }
    }

    public static bool TryGetDependency(From_primaryContext context, out Dependency? dependency)
    {
        dependency = null;
        if (context.schema_qualified_name() != null)
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
        return dependency != null;
    }

    public static string GetSequenceName(VexContext funcArgContext)
    {
        string res;
        if (funcArgContext.CAST_EXPRESSION() != null)
            res = funcArgContext.vex()[0].GetText();
        else
            res = funcArgContext.GetText();
        return res.Replace("'", "");
    }

    public static string Unquote(string quotedIdentifier)
    {
        return quotedIdentifier.Replace("\"", "");
    }
}
