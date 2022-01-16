using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors
{
    internal class PostgreSQLIsDependencyOfTablesObjectsEditor
    {
        protected readonly IQueryExecutor QueryExecutor;

        public PostgreSQLIsDependencyOfTablesObjectsEditor(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor;
        }

        public void Rename_RemovedOrChanged_ObjectsThatTablesDependOn_ToTemp_InDbAndInDbDiff(PostgreSQLDatabaseDiff dbDiff)
        {
            foreach (DBObject dbObject in GetOrderedByDependenciesObjectsToDrop(dbDiff))
                RenameDbObjectToTempInDbAndInDbDiff(dbObject);
        }

        public void Create_AddedOrChanged_ObjectsThatTablesDependOn(PostgreSQLDatabaseDiff dbDiff)
        {
            foreach (DBObject dbObject in GetOrderedByDependenciesObjectsToCreate(dbDiff))
                CreateDbObject(dbObject);
        }

        public void Drop_RemovedOrChanged_ObjectsThatTablesDependOn(PostgreSQLDatabaseDiff dbDiff)
        {
            foreach (DBObject dbObject in GetOrderedByDependenciesObjectsToDrop(dbDiff))
                DropDbObject(dbObject);
        }

        private static IEnumerable<DBObject> GetOrderedByDependenciesObjectsToCreate(PostgreSQLDatabaseDiff dbDiff)
        {
            return dbDiff.FunctionsToCreate.Where(x => x.IsSimple).Select(x => (DBObject)x)
                .Concat(dbDiff.AddedCompositeTypes.Select(x => (DBObject)x))
                .Concat(dbDiff.ChangedCompositeTypes.Select(x => (DBObject)x.NewCompositeType))
                .Concat(dbDiff.AddedDomainTypes.Select(x => (DBObject)x))
                .Concat(dbDiff.ChangedDomainTypes.Select(x => (DBObject)x.NewDomainType))
                .Concat(dbDiff.AddedEnumTypes.Select(x => (DBObject)x))
                .Concat(dbDiff.ChangedEnumTypes.Select(x => (DBObject)x.NewEnumType))
                .Concat(dbDiff.AddedRangeTypes.Select(x => (DBObject)x))
                .Concat(dbDiff.ChangedRangeTypes.Select(x => (DBObject)x.NewRangeType))
                .OrderByDependenciesFirst();
        }

        private static IEnumerable<DBObject> GetOrderedByDependenciesObjectsToDrop(PostgreSQLDatabaseDiff dbDiff)
        {
            return dbDiff.FunctionsToDrop.Where(x => x.IsSimple).Select(x => (DBObject)x)
                .Concat(dbDiff.RemovedCompositeTypes.Select(x => (DBObject)x))
                .Concat(dbDiff.ChangedCompositeTypes.Select(x => (DBObject)x.OldCompositeType))
                .Concat(dbDiff.RemovedDomainTypes.Select(x => (DBObject)x))
                .Concat(dbDiff.ChangedDomainTypes.Select(x => (DBObject)x.OldDomainType))
                .Concat(dbDiff.RemovedEnumTypes.Select(x => (DBObject)x))
                .Concat(dbDiff.ChangedEnumTypes.Select(x => (DBObject)x.OldEnumType))
                .Concat(dbDiff.RemovedRangeTypes.Select(x => (DBObject)x))
                .Concat(dbDiff.ChangedRangeTypes.Select(x => (DBObject)x.OldRangeType))
                .OrderByDependenciesLast();
        }

        private void RenameDbObjectToTempInDbAndInDbDiff(DBObject dbObject)
        {
            if (dbObject is PostgreSQLFunction func)
                RenameFunctionToTempInDbAndInDbDiff(func);
            else if (dbObject is DBObject type)
                RenameTypeToTempInDbAndInDbDiff(type);
            else
                throw new InvalidOperationException($"Invalid dbObject type to rename to temp {dbObject.GetType()}");
        }

        private void CreateDbObject(DBObject dbObject)
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

        private void DropDbObject(DBObject dbObject)
        {
            if (dbObject is PostgreSQLFunction func)
                DropFunction(func);
            else if (dbObject is DBObject type)
                DropType(type);
            else
                throw new InvalidOperationException($"Invalid dbObject type to drop {dbObject.GetType()}");
        }

        private void RenameFunctionToTempInDbAndInDbDiff(PostgreSQLFunction func)
        {
            QueryExecutor.Execute(new PostgreSQLRenameFunctionToTempQuery(func));
            func.Name = $"_DNDBTTemp_{func.Name}";
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(func.ID));
        }

        private void RenameTypeToTempInDbAndInDbDiff(DBObject type)
        {
            QueryExecutor.Execute(new PostgreSQLRenameTypeToTempQuery(type));
            type.Name = $"_DNDBTTemp_{type.Name}";
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(type.ID));

            if (type is PostgreSQLDomainType domainType)
            {
                foreach (CheckConstraint ck in domainType.CheckConstraints)
                    QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(ck.ID));
            }
        }

        private void CreateFunction(PostgreSQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($"{func.GetCode()}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(func.ID, null, DbObjectsTypes.Function, func.Name, func.GetCode()));
        }

        private void CreateCompositeType(PostgreSQLCompositeType type)
        {
            QueryExecutor.Execute(new PostgreSQLCreateCompositeTypeQuery(type));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(type.ID, null, DbObjectsTypes.UserDefinedType, type.Name));
        }

        private void CreateDomainType(PostgreSQLDomainType type)
        {
            QueryExecutor.Execute(new PostgreSQLCreateDomainTypeQuery(type));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(type.ID, null, DbObjectsTypes.UserDefinedType, type.Name));
            foreach (CheckConstraint ck in type.CheckConstraints)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(ck.ID, type.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetCode()));
        }

        private void CreateEnumType(PostgreSQLEnumType type)
        {
            QueryExecutor.Execute(new PostgreSQLCreateEnumTypeQuery(type));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(type.ID, null, DbObjectsTypes.UserDefinedType, type.Name));
        }

        private void CreateRangeType(PostgreSQLRangeType type)
        {
            QueryExecutor.Execute(new PostgreSQLCreateRangeTypeQuery(type));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(type.ID, null, DbObjectsTypes.UserDefinedType, type.Name));
        }

        private void DropFunction(PostgreSQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP FUNCTION ""{func.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(func.ID));
        }

        private void DropType(DBObject type)
        {
            QueryExecutor.Execute(new PostgreSQLDropTypeQuery(type));
        }
    }
}
