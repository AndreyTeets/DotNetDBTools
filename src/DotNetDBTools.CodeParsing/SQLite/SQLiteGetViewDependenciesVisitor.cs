using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.SQLiteParser;

namespace DotNetDBTools.CodeParsing.SQLite;

internal class SQLiteGetViewDependenciesVisitor : SQLiteParserBaseVisitor<object>
{
    private readonly HashSet<Dependency> _dependencies = new();

    public List<Dependency> GetDependencies() => _dependencies.ToList();

    public override object VisitTable_or_subquery([NotNull] Table_or_subqueryContext context)
    {
        if (SQLiteHelperMethods.TryGetDependency(context, out Dependency? dependency))
            _dependencies.Add(dependency.Value);
        return base.VisitTable_or_subquery(context);
    }
}
