using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Analysis.Extensions.PostgreSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLTypesEditor
{
    protected readonly IQueryExecutor QueryExecutor;

    public PostgreSQLTypesEditor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public void Rename_TypesToDrop_ToTemp(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetTypesToDropOrderedByDependencies(dbDiff))
            RenameTypeToTemp(dbObject);
    }

    public void CreateTypes(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetTypesToCreateOrderedByDependencies(dbDiff))
            CreateType(dbObject);
    }

    public void Drop_RenamedToTemp_TypesToDrop(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (DbObject dbObject in GetTypesToDropOrderedByDependencies(dbDiff))
            Drop_RenamedToTemp_Type(dbObject);
    }

    public void AlterTypes_ExceptDomains_Default_CK(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (PostgreSQLDomainTypeDiff typeDiff in GetStrippedDomainTypeDiffModels(dbDiff.DomainTypesToAlter))
            AlterDomainType(typeDiff);
    }

    public void SetDomainsDefault(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (PostgreSQLDomainTypeDiff typeDiff in GetDomainTypeDiffsForSettingDefault(dbDiff))
            AlterDomainType(typeDiff);
    }

    public void DropDomainsDefault(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (PostgreSQLDomainTypeDiff typeDiff in GetDomainTypeDiffsForDroppingDefault(dbDiff))
            AlterDomainType(typeDiff);
    }

    public void AddDomainsCheckConstraints(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (PostgreSQLDomainTypeDiff typeDiff in GetDomainTypeDiffsForAddingCheckConstraints(dbDiff))
            AlterDomainType(typeDiff);
    }

    public void DropDomainsCheckConstraints(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (PostgreSQLDomainTypeDiff typeDiff in GetDomainTypeDiffsForDroppingCheckConstraints(dbDiff))
            AlterDomainType(typeDiff);
    }

    private static IEnumerable<DbObject> GetTypesToCreateOrderedByDependencies(PostgreSQLDatabaseDiff dbDiff)
    {
        return dbDiff.CompositeTypesToCreate.Select(x => (DbObject)x)
            .Concat(GetStrippedDomainTypeModelsToCreate(dbDiff))
            .Concat(dbDiff.EnumTypesToCreate.Select(x => (DbObject)x))
            .Concat(dbDiff.RangeTypesToCreate.Select(x => (DbObject)x))
            .OrderByDependenciesFirst(x => x.GetDependencies());

        static IEnumerable<DbObject> GetStrippedDomainTypeModelsToCreate(PostgreSQLDatabaseDiff dbDiff)
        {
            List<PostgreSQLDomainType> res = new();
            foreach (PostgreSQLDomainType type in dbDiff.DomainTypesToCreate)
            {
                PostgreSQLDomainType strippedTypeModel = type.CopyModel();
                if (type.Default.DependsOn.Any(IsComplexDependency))
                    strippedTypeModel.Default.Code = null;
                strippedTypeModel.CheckConstraints.RemoveAll(x => x.Expression.DependsOn.Any(IsComplexDependency));
                res.Add(strippedTypeModel);
            }
            return res;
        }
    }

    private static IEnumerable<DbObject> GetTypesToDropOrderedByDependencies(PostgreSQLDatabaseDiff dbDiff)
    {
        return dbDiff.CompositeTypesToDrop.Select(x => (DbObject)x)
            .Concat(dbDiff.DomainTypesToDrop.Select(x => (DbObject)x))
            .Concat(dbDiff.EnumTypesToDrop.Select(x => (DbObject)x))
            .Concat(dbDiff.RangeTypesToDrop.Select(x => (DbObject)x))
            .OrderByDependenciesLast(x => x.GetDependencies());
    }

    private static List<PostgreSQLDomainTypeDiff> GetStrippedDomainTypeDiffModels(IEnumerable<PostgreSQLDomainTypeDiff> typeDiffs)
    {
        List<PostgreSQLDomainTypeDiff> res = new();
        foreach (PostgreSQLDomainTypeDiff typeDiff in typeDiffs)
        {
            PostgreSQLDomainTypeDiff strippedTypeDiffModel = typeDiff.CopyModel();
            strippedTypeDiffModel.DefaultToSet = null;
            strippedTypeDiffModel.DefaultToDrop = null;
            strippedTypeDiffModel.CheckConstraintsToCreate.Clear();
            strippedTypeDiffModel.CheckConstraintsToDrop.Clear();

            if (!AnalysisManager.DiffIsEmpty(strippedTypeDiffModel))
                res.Add(strippedTypeDiffModel);
        }
        return res;
    }

    private static List<PostgreSQLDomainTypeDiff> GetDomainTypeDiffsForSettingDefault(PostgreSQLDatabaseDiff dbDiff)
    {
        List<PostgreSQLDomainTypeDiff> res = new();
        foreach (PostgreSQLDomainType type in dbDiff.DomainTypesToCreate)
        {
            PostgreSQLDomainTypeDiff diffForSettingDefault = type.CreateEmptyDomainTypeDiff();
            if (type.Default.Code != null && type.Default.DependsOn.Any(IsComplexDependency))
            {
                diffForSettingDefault.DefaultToSet = type.Default;
                res.Add(diffForSettingDefault);
            }
        }
        foreach (PostgreSQLDomainTypeDiff typeDiff in dbDiff.DomainTypesToAlter)
        {
            PostgreSQLDomainTypeDiff diffForSettingDefault = new()
            {
                TypeID = typeDiff.TypeID,
                NewTypeName = typeDiff.NewTypeName,
                OldTypeName = typeDiff.NewTypeName,
            };
            diffForSettingDefault.DefaultToSet = typeDiff.DefaultToSet;

            if (diffForSettingDefault.DefaultToSet != null)
                res.Add(diffForSettingDefault);
        }
        return res;
    }

    private static List<PostgreSQLDomainTypeDiff> GetDomainTypeDiffsForDroppingDefault(PostgreSQLDatabaseDiff dbDiff)
    {
        List<PostgreSQLDomainTypeDiff> res = new();
        foreach (PostgreSQLDomainType type in dbDiff.DomainTypesToDrop)
        {
            PostgreSQLDomainTypeDiff diffForDroppingDefault = type.CreateEmptyDomainTypeDiff();
            if (type.Default.Code != null && type.Default.DependsOn.Any(IsComplexDependency))
            {
                diffForDroppingDefault.DefaultToDrop = type.Default;
                res.Add(diffForDroppingDefault);
            }
        }
        foreach (PostgreSQLDomainTypeDiff typeDiff in dbDiff.DomainTypesToAlter)
        {
            PostgreSQLDomainTypeDiff diffForDroppingDefault = new()
            {
                TypeID = typeDiff.TypeID,
                NewTypeName = typeDiff.OldTypeName,
                OldTypeName = typeDiff.OldTypeName,
            };
            diffForDroppingDefault.DefaultToDrop = typeDiff.DefaultToDrop;

            if (diffForDroppingDefault.DefaultToDrop != null)
                res.Add(diffForDroppingDefault);
        }
        return res;
    }

    private static List<PostgreSQLDomainTypeDiff> GetDomainTypeDiffsForAddingCheckConstraints(PostgreSQLDatabaseDiff dbDiff)
    {
        List<PostgreSQLDomainTypeDiff> res = new();
        foreach (PostgreSQLDomainType type in dbDiff.DomainTypesToCreate)
        {
            PostgreSQLDomainTypeDiff diffForAddingCheckConstraints = type.CreateEmptyDomainTypeDiff();
            foreach (CheckConstraint ck in type.CheckConstraints.Where(x => x.Expression.DependsOn.Any(IsComplexDependency)))
                diffForAddingCheckConstraints.CheckConstraintsToCreate.Add(ck);

            if (diffForAddingCheckConstraints.CheckConstraintsToCreate.Count > 0)
                res.Add(diffForAddingCheckConstraints);
        }
        foreach (PostgreSQLDomainTypeDiff typeDiff in dbDiff.DomainTypesToAlter)
        {
            PostgreSQLDomainTypeDiff diffForAddingCheckConstraints = new()
            {
                TypeID = typeDiff.TypeID,
                NewTypeName = typeDiff.NewTypeName,
                OldTypeName = typeDiff.NewTypeName,
            };
            foreach (CheckConstraint ck in typeDiff.CheckConstraintsToCreate)
                diffForAddingCheckConstraints.CheckConstraintsToCreate.Add(ck);

            if (diffForAddingCheckConstraints.CheckConstraintsToCreate.Count > 0)
                res.Add(diffForAddingCheckConstraints);
        }
        return res;
    }

    private static List<PostgreSQLDomainTypeDiff> GetDomainTypeDiffsForDroppingCheckConstraints(PostgreSQLDatabaseDiff dbDiff)
    {
        List<PostgreSQLDomainTypeDiff> res = new();
        foreach (PostgreSQLDomainType type in dbDiff.DomainTypesToDrop)
        {
            PostgreSQLDomainTypeDiff diffForDroppingCheckConstraints = type.CreateEmptyDomainTypeDiff();
            foreach (CheckConstraint ck in type.CheckConstraints.Where(x => x.Expression.DependsOn.Any(IsComplexDependency)))
                diffForDroppingCheckConstraints.CheckConstraintsToDrop.Add(ck);

            if (diffForDroppingCheckConstraints.CheckConstraintsToDrop.Count > 0)
                res.Add(diffForDroppingCheckConstraints);
        }
        foreach (PostgreSQLDomainTypeDiff typeDiff in dbDiff.DomainTypesToAlter)
        {
            PostgreSQLDomainTypeDiff diffForDroppingCheckConstraints = new()
            {
                TypeID = typeDiff.TypeID,
                NewTypeName = typeDiff.OldTypeName,
                OldTypeName = typeDiff.OldTypeName,
            };
            foreach (CheckConstraint ck in typeDiff.CheckConstraintsToDrop)
                diffForDroppingCheckConstraints.CheckConstraintsToDrop.Add(ck);

            if (diffForDroppingCheckConstraints.CheckConstraintsToDrop.Count > 0)
                res.Add(diffForDroppingCheckConstraints);
        }
        return res;
    }

    private void RenameTypeToTemp(DbObject type)
    {
        QueryExecutor.Execute(new PostgreSQLRenameTypeToTempQuery(type));

        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(type.ID));
        if (type is PostgreSQLDomainType domainType)
        {
            foreach (CheckConstraint ck in domainType.CheckConstraints)
                QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(ck.ID));
        }
    }

    private void CreateType(DbObject dbObject)
    {
        if (dbObject is PostgreSQLCompositeType compositeType)
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

    private void Drop_RenamedToTemp_Type(DbObject type)
    {
        DbObject renamedType = type.CopyModel();
        renamedType.Name = $"_DNDBTTemp_{type.Name}";
        QueryExecutor.Execute(new PostgreSQLDropTypeQuery(renamedType));
    }

    private void AlterDomainType(PostgreSQLDomainTypeDiff typeDiff)
    {
        QueryExecutor.Execute(new PostgreSQLAlterDomainTypeQuery(typeDiff));
        QueryExecutor.Execute(new PostgreSQLUpdateDNDBTDbObjectRecordQuery(typeDiff.TypeID, typeDiff.NewTypeName));
        foreach (CheckConstraint ck in typeDiff.CheckConstraintsToDrop)
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(ck.ID));
        foreach (CheckConstraint ck in typeDiff.CheckConstraintsToCreate)
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(ck.ID, typeDiff.TypeID, DbObjectType.CheckConstraint, ck.Name, ck.GetExpression()));
    }

    private static bool IsComplexDependency(DbObject dbObject)
    {
        return dbObject switch
        {
            PostgreSQLSequence x => false,
            PostgreSQLView x => x.CreateStatement.DependsOn.Any(IsComplexDependency),
            PostgreSQLFunction x => x.CreateStatement.DependsOn.Any(IsComplexDependency),
            PostgreSQLProcedure x => x.CreateStatement.DependsOn.Any(IsComplexDependency),
            _ => true,
        };
    }
}
