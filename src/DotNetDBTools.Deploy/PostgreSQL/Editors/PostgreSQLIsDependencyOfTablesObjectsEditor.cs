using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLIsDependencyOfTablesObjectsEditor
{
    protected readonly IQueryExecutor QueryExecutor;

    public PostgreSQLIsDependencyOfTablesObjectsEditor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public void Rename_RemovedOrChanged_ObjectsThatTablesDependOn_ToTemp_InDbAndInDbDiff(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetOrderedByDependenciesObjectsToDrop(dbDiff))
            RenameDbObjectToTempInDbAndInDbDiff(dbObject);
    }

    public void Create_AddedOrChanged_ObjectsThatTablesDependOn(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetOrderedByDependenciesObjectsToCreate(dbDiff))
            CreateDbObject(dbObject);
    }

    public void Drop_RemovedOrChanged_ObjectsThatTablesDependOn(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetOrderedByDependenciesObjectsToDrop(dbDiff))
            DropDbObject(dbObject);
    }

    private static IEnumerable<DbObject> GetOrderedByDependenciesObjectsToCreate(PostgreSQLDatabaseDiff dbDiff)
    {
        return dbDiff.FunctionsToCreate.Where(x => x.IsSimple).Select(x => (DbObject)x)
            .Concat(dbDiff.AddedCompositeTypes.Select(x => (DbObject)x))
            .Concat(dbDiff.ChangedCompositeTypes.Select(x => (DbObject)x.NewCompositeType))
            .Concat(dbDiff.AddedDomainTypes.Select(x => (DbObject)x))
            .Concat(dbDiff.ChangedDomainTypes.Select(x => (DbObject)x.NewDomainType))
            .Concat(dbDiff.AddedEnumTypes.Select(x => (DbObject)x))
            .Concat(dbDiff.ChangedEnumTypes.Select(x => (DbObject)x.NewEnumType))
            .Concat(dbDiff.AddedRangeTypes.Select(x => (DbObject)x))
            .Concat(dbDiff.ChangedRangeTypes.Select(x => (DbObject)x.NewRangeType))
            .OrderByDependenciesFirst();
    }

    private static IEnumerable<DbObject> GetOrderedByDependenciesObjectsToDrop(PostgreSQLDatabaseDiff dbDiff)
    {
        return dbDiff.FunctionsToDrop.Where(x => x.IsSimple).Select(x => (DbObject)x)
            .Concat(dbDiff.RemovedCompositeTypes.Select(x => (DbObject)x))
            .Concat(dbDiff.ChangedCompositeTypes.Select(x => (DbObject)x.OldCompositeType))
            .Concat(dbDiff.RemovedDomainTypes.Select(x => (DbObject)x))
            .Concat(dbDiff.ChangedDomainTypes.Select(x => (DbObject)x.OldDomainType))
            .Concat(dbDiff.RemovedEnumTypes.Select(x => (DbObject)x))
            .Concat(dbDiff.ChangedEnumTypes.Select(x => (DbObject)x.OldEnumType))
            .Concat(dbDiff.RemovedRangeTypes.Select(x => (DbObject)x))
            .Concat(dbDiff.ChangedRangeTypes.Select(x => (DbObject)x.OldRangeType))
            .OrderByDependenciesLast();
    }

    private void RenameDbObjectToTempInDbAndInDbDiff(DbObject dbObject)
    {
        if (dbObject is PostgreSQLFunction func)
            RenameFunctionToTempInDbAndInDbDiff(func);
        else if (dbObject is DbObject type)
            RenameTypeToTempInDbAndInDbDiff(type);
        else
            throw new InvalidOperationException($"Invalid dbObject type to rename to temp {dbObject.GetType()}");
    }

    private void CreateDbObject(DbObject dbObject)
    {
        if (dbObject is PostgreSQLFunction func)
            CreateFunction(func);
        else if (dbObject is PostgreSQLCompositeType compositeType)
            CreateCompositeType(compositeType);
        else if (dbObject is PostgreSQLDomainType domainType)
            CreateDomainType(domainType);
        else if (dbObject is PostgreSQLEnumType enumType)
            CreateEnumType(enumType);
        else if (dbObject is PostgreSQLRangeType rangeType)
            CreateRangeType(rangeType);
        else
            throw new InvalidOperationException($"Invalid dbObject type to create {dbObject.GetType()}");
    }

    private void DropDbObject(DbObject dbObject)
    {
        if (dbObject is PostgreSQLFunction func)
            DropFunction(func);
        else if (dbObject is DbObject type)
            DropType(type);
        else
            throw new InvalidOperationException($"Invalid dbObject type to drop {dbObject.GetType()}");
    }

    private void RenameFunctionToTempInDbAndInDbDiff(PostgreSQLFunction func)
    {
        QueryExecutor.Execute(new PostgreSQLRenameFunctionToTempQuery(func));
        func.Name = $"_DNDBTTemp_{func.Name}";
        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(func.ID));
    }

    private void RenameTypeToTempInDbAndInDbDiff(DbObject type)
    {
        QueryExecutor.Execute(new PostgreSQLRenameTypeToTempQuery(type));
        type.Name = $"_DNDBTTemp_{type.Name}";
        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(type.ID));

        if (type is PostgreSQLDomainType domainType)
        {
            foreach (CheckConstraint ck in domainType.CheckConstraints)
                QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(ck.ID));
        }
    }

    private void CreateFunction(PostgreSQLFunction func)
    {
        QueryExecutor.Execute(new PostgreSQLCreateFunctionQuery(func));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(func.ID, null, DbObjectType.Function, func.Name, func.GetCreateStatement()));
    }

    private void DropFunction(PostgreSQLFunction func)
    {
        QueryExecutor.Execute(new PostgreSQLDropFunctionQuery(func));
        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(func.ID));
    }

    private void CreateCompositeType(PostgreSQLCompositeType type)
    {
        QueryExecutor.Execute(new PostgreSQLCreateCompositeTypeQuery(type));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type.ID, null, DbObjectType.UserDefinedType, type.Name));
    }

    private void CreateDomainType(PostgreSQLDomainType type)
    {
        QueryExecutor.Execute(new PostgreSQLCreateDomainTypeQuery(type));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type.ID, null, DbObjectType.UserDefinedType, type.Name));
        foreach (CheckConstraint ck in type.CheckConstraints)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(ck.ID, type.ID, DbObjectType.CheckConstraint, ck.Name, ck.GetExpression()));
    }

    private void CreateEnumType(PostgreSQLEnumType type)
    {
        QueryExecutor.Execute(new PostgreSQLCreateEnumTypeQuery(type));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type.ID, null, DbObjectType.UserDefinedType, type.Name));
    }

    private void CreateRangeType(PostgreSQLRangeType type)
    {
        QueryExecutor.Execute(new PostgreSQLCreateRangeTypeQuery(type));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(type.ID, null, DbObjectType.UserDefinedType, type.Name));
    }

    private void DropType(DbObject type)
    {
        QueryExecutor.Execute(new PostgreSQLDropTypeQuery(type));
    }
}
