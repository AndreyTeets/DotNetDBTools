using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors
{
    internal class PostgreSQLUserDefinedTypesEditor
    {
        protected readonly IQueryExecutor QueryExecutor;

        public PostgreSQLUserDefinedTypesEditor(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor;
        }

        public void RenameAllUserDefinedTypesToTempInDbAndInDbDiff(PostgreSQLDatabaseDiff dbDiff)
        {
            foreach (PostgreSQLCompositeType type in dbDiff.RemovedCompositeTypes.Concat(dbDiff.ChangedCompositeTypes.Select(x => x.OldCompositeType)))
                RenameTypeToTempInDbAndInDbDiff(type);
            foreach (PostgreSQLDomainType type in dbDiff.RemovedDomainTypes.Concat(dbDiff.ChangedDomainTypes.Select(x => x.OldDomainType)))
                RenameTypeToTempInDbAndInDbDiff(type);
            foreach (PostgreSQLEnumType type in dbDiff.RemovedEnumTypes.Concat(dbDiff.ChangedEnumTypes.Select(x => x.OldEnumType)))
                RenameTypeToTempInDbAndInDbDiff(type);
            foreach (PostgreSQLRangeType type in dbDiff.RemovedRangeTypes.Concat(dbDiff.ChangedRangeTypes.Select(x => x.OldRangeType)))
                RenameTypeToTempInDbAndInDbDiff(type);
        }

        public void CreateAllUserDefinedTypes(PostgreSQLDatabaseDiff dbDiff)
        {
            foreach (PostgreSQLCompositeType type in dbDiff.AddedCompositeTypes.Concat(dbDiff.ChangedCompositeTypes.Select(x => x.NewCompositeType)))
                CreateCompositeType(type);
            foreach (PostgreSQLDomainType type in dbDiff.AddedDomainTypes.Concat(dbDiff.ChangedDomainTypes.Select(x => x.NewDomainType)))
                CreateDomainType(type);
            foreach (PostgreSQLEnumType type in dbDiff.AddedEnumTypes.Concat(dbDiff.ChangedEnumTypes.Select(x => x.NewEnumType)))
                CreateEnumType(type);
            foreach (PostgreSQLRangeType type in dbDiff.AddedRangeTypes.Concat(dbDiff.ChangedRangeTypes.Select(x => x.NewRangeType)))
                CreateRangeType(type);
        }

        public void DropAllUserDefinedTypes(PostgreSQLDatabaseDiff dbDiff)
        {
            foreach (PostgreSQLCompositeType type in dbDiff.RemovedCompositeTypes.Concat(dbDiff.ChangedCompositeTypes.Select(x => x.OldCompositeType)))
                DropType(type);
            foreach (PostgreSQLDomainType type in dbDiff.RemovedDomainTypes.Concat(dbDiff.ChangedDomainTypes.Select(x => x.OldDomainType)))
                DropType(type);
            foreach (PostgreSQLEnumType type in dbDiff.RemovedEnumTypes.Concat(dbDiff.ChangedEnumTypes.Select(x => x.OldEnumType)))
                DropType(type);
            foreach (PostgreSQLRangeType type in dbDiff.RemovedRangeTypes.Concat(dbDiff.ChangedRangeTypes.Select(x => x.OldRangeType)))
                DropType(type);
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
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(ck.ID, type.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetExtraInfo()));
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

        private void DropType(DBObject type)
        {
            QueryExecutor.Execute(new PostgreSQLDropTypeQuery(type));
        }
    }
}
