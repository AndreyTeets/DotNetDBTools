using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLDependsOnTablesObjectsEditor
{
    private readonly IIndexEditor _indexEditor;
    private readonly ITriggerEditor _triggerEditor;
    private readonly IForeignKeyEditor _foreignKeyEditor;

    protected readonly IQueryExecutor QueryExecutor;

    public PostgreSQLDependsOnTablesObjectsEditor(IQueryExecutor queryExecutor)
    {
        _indexEditor = new PostgreSQLIndexEditor(queryExecutor);
        _triggerEditor = new PostgreSQLTriggerEditor(queryExecutor);
        _foreignKeyEditor = new PostgreSQLForeignKeyEditor(queryExecutor);
        QueryExecutor = queryExecutor;
    }

    public void CreateObjectsThatDependOnTables(PostgreSQLDatabaseDiff dbDiff)
    {
        _indexEditor.CreateIndexes(dbDiff);
        _foreignKeyEditor.CreateForeignKeys(dbDiff);
        foreach (DbObject dbObject in GetOrderedByDependenciesObjectsToCreate(dbDiff))
            CreateDbObject(dbObject);
        _triggerEditor.CreateTriggers(dbDiff);
    }

    public void DropObjectsThatDependOnTables(PostgreSQLDatabaseDiff dbDiff)
    {
        _triggerEditor.DropTriggers(dbDiff);
        foreach (DbObject dbObject in GetOrderedByDependenciesObjectsToDrop(dbDiff))
            DropDbObject(dbObject);
        _foreignKeyEditor.DropForeignKeys(dbDiff);
        _indexEditor.DropIndexes(dbDiff);
    }

    private static IEnumerable<DbObject> GetOrderedByDependenciesObjectsToCreate(PostgreSQLDatabaseDiff dbDiff)
    {
        return dbDiff.FunctionsToCreate.Where(x => !x.IsSimple).Select(x => (DbObject)x)
            .Concat(dbDiff.ViewsToCreate.Select(x => (DbObject)x))
            .Concat(dbDiff.ProceduresToCreate.Select(x => (DbObject)x))
            .OrderByDependenciesFirst();
    }

    private static IEnumerable<DbObject> GetOrderedByDependenciesObjectsToDrop(PostgreSQLDatabaseDiff dbDiff)
    {
        return dbDiff.FunctionsToDrop.Where(x => !x.IsSimple).Select(x => (DbObject)x)
            .Concat(dbDiff.ViewsToDrop.Select(x => (DbObject)x))
            .Concat(dbDiff.ProceduresToDrop.Select(x => (DbObject)x))
            .OrderByDependenciesLast();
    }

    private void CreateDbObject(DbObject dbObject)
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

    private void DropDbObject(DbObject dbObject)
    {
        if (dbObject is PostgreSQLView view)
            DropView(view);
        else if (dbObject is PostgreSQLFunction func)
            DropFunction(func);
        else if (dbObject is PostgreSQLProcedure proc)
            DropProcedure(proc);
        else
            throw new InvalidOperationException($"Invalid dbObject type to drop {dbObject.GetType()}");
    }

    private void CreateView(PostgreSQLView view)
    {
        QueryExecutor.Execute(new CreateViewQuery(view));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(view.ID, null, DbObjectType.View, view.Name, view.GetCode()));
    }

    private void DropView(PostgreSQLView view)
    {
        QueryExecutor.Execute(new DropViewQuery(view));
        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(view.ID));
    }

    private void CreateFunction(PostgreSQLFunction func)
    {
        QueryExecutor.Execute(new PostgreSQLCreateFunctionQuery(func));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(func.ID, null, DbObjectType.Function, func.Name, func.GetCode()));
    }

    private void DropFunction(PostgreSQLFunction func)
    {
        QueryExecutor.Execute(new PostgreSQLDropFunctionQuery(func));
        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(func.ID));
    }

    private void CreateProcedure(PostgreSQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($"{proc.GetCode()}"));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(proc.ID, null, DbObjectType.Procedure, proc.Name, proc.GetCode()));
    }

    private void DropProcedure(PostgreSQLProcedure proc)
    {
        QueryExecutor.Execute(new GenericQuery($@"DROP PROCEDURE ""{proc.Name}"";"));
        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(proc.ID));
    }
}
