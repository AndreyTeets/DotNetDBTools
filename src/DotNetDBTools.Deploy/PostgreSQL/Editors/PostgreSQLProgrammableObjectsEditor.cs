using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Analysis.Extensions.PostgreSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLProgrammableObjectsEditor
{
    protected readonly IQueryExecutor QueryExecutor;

    public PostgreSQLProgrammableObjectsEditor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public void Rename_SimpleDepsProgrammableObjectsToDrop_ToTemp(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetProgrammableObjectsToDropOrderedByDependencies(dbDiff, DepsKind.Simple))
            RenameProgrammableObjectToTemp(dbObject);
    }

    public void CreateSimpleDepsProgrammableObjects(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetProgrammableObjectsToCreateOrderedByDependencies(dbDiff, DepsKind.Simple))
            CreateProgrammableObject(dbObject);
    }

    public void Drop_RenamedToTemp_SimpleDepsProgrammableObjectsToDrop(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetProgrammableObjectsToDropOrderedByDependencies(dbDiff, DepsKind.Simple))
            Drop_RenamedToTemp_ProgrammableObject(dbObject);
    }

    public void CreateComplexDepsProgrammableObjects(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetProgrammableObjectsToCreateOrderedByDependencies(dbDiff, DepsKind.Complex))
            CreateProgrammableObject(dbObject);
    }

    public void DropComplexDepsProgrammableObjects(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetProgrammableObjectsToDropOrderedByDependencies(dbDiff, DepsKind.Complex))
            DropProgrammableObject(dbObject);
    }

    private static IEnumerable<DbObject> GetProgrammableObjectsToCreateOrderedByDependencies(
        PostgreSQLDatabaseDiff dbDiff, DepsKind depsKind)
    {
        return dbDiff.ViewsToCreate.Select(x => (DbObject)x)
            .Concat(dbDiff.FunctionsToCreate.Select(x => (DbObject)x))
            .Concat(dbDiff.ProceduresToCreate.Select(x => (DbObject)x))
            .Where(x => depsKind == DepsKind.Simple ? !IsComplexDepsObject(x) : IsComplexDepsObject(x))
            .OrderByDependenciesFirst(x => x.GetDependencies());
    }

    private static IEnumerable<DbObject> GetProgrammableObjectsToDropOrderedByDependencies(
        PostgreSQLDatabaseDiff dbDiff, DepsKind depsKind)
    {
        return dbDiff.ViewsToDrop.Select(x => (DbObject)x)
            .Concat(dbDiff.FunctionsToDrop.Select(x => (DbObject)x))
            .Concat(dbDiff.ProceduresToDrop.Select(x => (DbObject)x))
            .Where(x => depsKind == DepsKind.Simple ? !IsComplexDepsObject(x) : IsComplexDepsObject(x))
            .OrderByDependenciesLast(x => x.GetDependencies());
    }

    private void RenameProgrammableObjectToTemp(DbObject dbObject)
    {
        QueryExecutor.Execute(new PostgreSQLRenameProgrammableObjectToTempQuery(dbObject));
        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(dbObject.ID));
    }

    private void CreateProgrammableObject(DbObject dbObject)
    {
        if (dbObject is PostgreSQLView view)
            CreateView(view);
        else if (dbObject is PostgreSQLFunction func)
            CreateFunction(func);
        else if (dbObject is PostgreSQLProcedure proc)
            CreateProcedure(proc);
        else
            throw new InvalidOperationException($"Invalid dbObject type to create {dbObject.GetType()}");
    }

    private void Drop_RenamedToTemp_ProgrammableObject(DbObject dbObject)
    {
        DbObject renamedDbObject = dbObject.CopyModel();
        renamedDbObject.Name = $"_DNDBTTemp_{dbObject.Name}";

        if (renamedDbObject is PostgreSQLView view)
            DropView(view);
        else if (renamedDbObject is PostgreSQLFunction func)
            DropFunction(func);
        else if (renamedDbObject is PostgreSQLProcedure proc)
            DropProcedure(proc);
        else
            throw new InvalidOperationException($"Invalid dbObject type to drop {renamedDbObject.GetType()}");
    }

    private void DropProgrammableObject(DbObject dbObject)
    {
        if (dbObject is PostgreSQLView view)
            DropView(view);
        else if (dbObject is PostgreSQLFunction func)
            DropFunction(func);
        else if (dbObject is PostgreSQLProcedure proc)
            DropProcedure(proc);
        else
            throw new InvalidOperationException($"Invalid dbObject type to drop {dbObject.GetType()}");

        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(dbObject.ID));
    }

    private void CreateView(PostgreSQLView view)
    {
        QueryExecutor.Execute(new CreateViewQuery(view));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(view.ID, null, DbObjectType.View, view.Name, view.GetCreateStatement()));
    }

    private void DropView(PostgreSQLView view)
    {
        QueryExecutor.Execute(new DropViewQuery(view));
    }

    private void CreateFunction(PostgreSQLFunction func)
    {
        QueryExecutor.Execute(new PostgreSQLCreateFunctionQuery(func));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(func.ID, null, DbObjectType.Function, func.Name, func.GetCreateStatement()));
    }

    private void DropFunction(PostgreSQLFunction func)
    {
        QueryExecutor.Execute(new PostgreSQLDropFunctionQuery(func));
    }

    private void CreateProcedure(PostgreSQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"{proc.GetCreateStatement()}"));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(proc.ID, null, DbObjectType.Procedure, proc.Name, proc.GetCreateStatement()));
    }

    private void DropProcedure(PostgreSQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($@"DROP PROCEDURE ""{proc.Name}"";"));
    }

    private static bool IsComplexDepsObject(DbObject dbObject)
    {
        return dbObject switch
        {
            PostgreSQLSequence x => false,
            PostgreSQLView x => x.CreateStatement.DependsOn.Any(IsComplexDepsObject),
            PostgreSQLFunction x => x.CreateStatement.DependsOn.Any(IsComplexDepsObject),
            PostgreSQLProcedure x => x.CreateStatement.DependsOn.Any(IsComplexDepsObject),
            _ => true,
        };
    }

    private enum DepsKind
    {
        Simple = 1,
        Complex = 2,
    }
}
