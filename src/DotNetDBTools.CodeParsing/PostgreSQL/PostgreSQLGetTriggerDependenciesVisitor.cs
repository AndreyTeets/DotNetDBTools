using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.PostgreSQLParser;

namespace DotNetDBTools.CodeParsing.PostgreSQL;

internal class PostgreSQLGetTriggerDependenciesVisitor : PostgreSQLParserBaseVisitor<object>
{
    private readonly HashSet<Dependency> _dependencies = new();

    public List<Dependency> GetDependencies() => _dependencies.ToList();

    public override object VisitCreate_trigger_statement([NotNull] Create_trigger_statementContext context)
    {
        string executedFunctionName = PostgreSQLHelperMethods.Unquote(
            context.func_name.schema_qualified_name_for_func_name().GetText());
        _dependencies.Add(new Dependency { Type = DependencyType.Function, Name = executedFunctionName });
        return base.VisitCreate_trigger_statement(context);
    }
}
