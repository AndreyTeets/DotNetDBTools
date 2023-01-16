﻿using System.Collections.Generic;
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

    public override object VisitData_type([NotNull] Data_typeContext context)
    {
        if (PostgreSQLHelperMethods.TryGetDependency(context, out Dependency? dependency))
            _dependencies.Add(dependency.Value);
        return base.VisitData_type(context);
    }

    public override object VisitFunction_call([NotNull] Function_callContext context)
    {
        if (PostgreSQLHelperMethods.TryGetDependency(context, out Dependency? dependency))
            _dependencies.Add(dependency.Value);
        return base.VisitFunction_call(context);
    }

    public override object VisitFrom_primary([NotNull] From_primaryContext context)
    {
        if (PostgreSQLHelperMethods.TryGetDependency(context, out Dependency? dependency))
            _dependencies.Add(dependency.Value);
        return base.VisitFrom_primary(context);
    }
}
