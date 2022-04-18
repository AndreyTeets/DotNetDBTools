using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.PostgreSQLParser;

namespace DotNetDBTools.CodeParsing.PostgreSQL;

internal class PostgreSQLGetViewDependenciesVisitor : PostgreSQLParserBaseVisitor<object>
{
    private readonly HashSet<Dependency> _dependencies = new();

    public List<Dependency> GetDependencies() => _dependencies.ToList();

    public override object VisitFrom_primary([NotNull] From_primaryContext context)
    {
        if (context.schema_qualified_name() != null)
        {
            string tableOrViewName = Unquote(context.schema_qualified_name().GetText());
            _dependencies.Add(new Dependency { Type = DependencyType.TableOrView, Name = tableOrViewName });
        }
        else if (context.function_call().Any())
        {
            string functionName = Unquote(context.function_call(0).schema_qualified_name_nontype().GetText());
            _dependencies.Add(new Dependency { Type = DependencyType.Function, Name = functionName });
        }
        return base.VisitFrom_primary(context);
    }

    private static string Unquote(string quotedIdentifier)
    {
        return quotedIdentifier.Replace("\"", "");
    }
}
