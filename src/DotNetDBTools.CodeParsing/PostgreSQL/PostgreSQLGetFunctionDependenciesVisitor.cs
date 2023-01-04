using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.PostgreSQLParser;

namespace DotNetDBTools.CodeParsing.PostgreSQL;

internal class PostgreSQLGetFunctionDependenciesVisitor : PostgreSQLParserBaseVisitor<object>
{
    private readonly HashSet<Dependency> _dependencies = new();

    public List<Dependency> GetDependencies() => _dependencies.ToList();

    public override object VisitFunction_call([NotNull] Function_callContext context)
    {
        if (context.schema_qualified_name_for_func_name() != null)
        {
            string functionName = Unquote(context.schema_qualified_name_for_func_name().GetText());
            _dependencies.Add(new Dependency { Type = DependencyType.Function, Name = functionName });
        }
        return base.VisitFunction_call(context);
    }

    public override object VisitFrom_primary([NotNull] From_primaryContext context)
    {
        if (context.schema_qualified_name() != null)
        {
            string tableOrViewName = Unquote(context.schema_qualified_name().GetText());
            _dependencies.Add(new Dependency { Type = DependencyType.TableOrView, Name = tableOrViewName });
        }
        return base.VisitFrom_primary(context);
    }

    public override object VisitInsert_stmt([NotNull] Insert_stmtContext context)
    {
        string tableName = Unquote(context.schema_qualified_name().GetText());
        _dependencies.Add(new Dependency { Type = DependencyType.TableOrView, Name = tableName });
        return base.VisitInsert_stmt(context);
    }

    public override object VisitUpdate_stmt([NotNull] Update_stmtContext context)
    {
        string tableName = Unquote(context.schema_qualified_name().GetText());
        _dependencies.Add(new Dependency { Type = DependencyType.TableOrView, Name = tableName });
        return base.VisitUpdate_stmt(context);
    }

    public override object VisitDelete_stmt([NotNull] Delete_stmtContext context)
    {
        string tableName = Unquote(context.schema_qualified_name().GetText());
        _dependencies.Add(new Dependency { Type = DependencyType.TableOrView, Name = tableName });
        return base.VisitDelete_stmt(context);
    }

    private static string Unquote(string quotedIdentifier)
    {
        return quotedIdentifier.Replace("\"", "");
    }
}
