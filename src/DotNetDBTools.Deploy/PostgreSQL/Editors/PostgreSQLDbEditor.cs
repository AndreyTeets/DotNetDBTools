using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors
{
    internal class PostgreSQLDbEditor : DbEditor<
        PostgreSQLCheckDNDBTSysTablesExistQuery,
        PostgreSQLCreateDNDBTSysTablesQuery,
        PostgreSQLDropDNDBTSysTablesQuery>
    {
        private readonly ITableEditor _tableEditor;
        private readonly PostgreSQLIsDependencyOfTablesObjectsEditor _isDependencyOfTablesObjectsEditor;
        private readonly PostgreSQLDependsOnTablesObjectsEditor _dependsOnTablesObjectsEditor;

        public PostgreSQLDbEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
            _tableEditor = new PostgreSQLTableEditor(queryExecutor);
            _isDependencyOfTablesObjectsEditor = new PostgreSQLIsDependencyOfTablesObjectsEditor(queryExecutor);
            _dependsOnTablesObjectsEditor = new PostgreSQLDependsOnTablesObjectsEditor(queryExecutor);
        }

        public override void PopulateDNDBTSysTables(Database database)
        {
            PostgreSQLDatabase db = (PostgreSQLDatabase)database;
            InsertUserDefinedTypesInfos(db);
            InsertTablesInfos(db);
            InsertViewsFunctionsProceduresInfos(db);
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff, DeployOptions options)
        {
            PostgreSQLDatabaseDiff dbDiff = (PostgreSQLDatabaseDiff)databaseDiff;
            QueryExecutor.BeginTransaction();
            try
            {
                ApplyDatabaseDiff(dbDiff);
            }
            catch (Exception)
            {
                QueryExecutor.RollbackTransaction();
                throw;
            }
            QueryExecutor.CommitTransaction();
        }

        private void ApplyDatabaseDiff(PostgreSQLDatabaseDiff dbDiff)
        {
            _dependsOnTablesObjectsEditor.DropObjectsThatDependOnTables(dbDiff);

            _isDependencyOfTablesObjectsEditor.Rename_RemovedOrChanged_ObjectsThatTablesDependOn_ToTemp_InDbAndInDbDiff(dbDiff);
            _isDependencyOfTablesObjectsEditor.Create_AddedOrChanged_ObjectsThatTablesDependOn(dbDiff);

            _tableEditor.DropTables(dbDiff);
            _tableEditor.AlterTables(dbDiff);
            _tableEditor.CreateTables(dbDiff);

            _isDependencyOfTablesObjectsEditor.Drop_RemovedOrChanged_ObjectsThatTablesDependOn(dbDiff);

            _dependsOnTablesObjectsEditor.CreateObjectsThatDependOnTables(dbDiff);
        }

        private void InsertUserDefinedTypesInfos(PostgreSQLDatabase db)
        {
            foreach (PostgreSQLCompositeType type in db.CompositeTypes)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(type.ID, null, DbObjectsTypes.UserDefinedType, type.Name));
            foreach (PostgreSQLDomainType type in db.DomainTypes)
            {
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(type.ID, null, DbObjectsTypes.UserDefinedType, type.Name, type.GetCode()));
                foreach (CheckConstraint ck in type.CheckConstraints)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(ck.ID, type.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetCode()));
            }
            foreach (PostgreSQLEnumType type in db.EnumTypes)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(type.ID, null, DbObjectsTypes.UserDefinedType, type.Name));
            foreach (PostgreSQLRangeType type in db.RangeTypes)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(type.ID, null, DbObjectsTypes.UserDefinedType, type.Name));
        }

        private void InsertTablesInfos(PostgreSQLDatabase db)
        {
            foreach (PostgreSQLTable table in db.Tables)
            {
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(table.ID, null, DbObjectsTypes.Table, table.Name));
                foreach (Column c in table.Columns)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(c.ID, table.ID, DbObjectsTypes.Column, c.Name, c.GetCode()));

                PrimaryKey pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(pk.ID, table.ID, DbObjectsTypes.PrimaryKey, pk.Name));

                foreach (UniqueConstraint uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(uc.ID, table.ID, DbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(fk.ID, table.ID, DbObjectsTypes.ForeignKey, fk.Name));
                foreach (CheckConstraint ck in table.CheckConstraints)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(ck.ID, table.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetCode()));
                foreach (Index idx in table.Indexes)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(idx.ID, table.ID, DbObjectsTypes.Index, idx.Name));
                foreach (Trigger trg in table.Triggers)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(trg.ID, table.ID, DbObjectsTypes.Trigger, trg.Name, trg.GetCode()));
            }
        }

        private void InsertViewsFunctionsProceduresInfos(PostgreSQLDatabase db)
        {
            foreach (PostgreSQLView view in db.Views)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name, view.GetCode()));
            foreach (PostgreSQLFunction func in db.Functions)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(func.ID, null, DbObjectsTypes.Function, func.Name, func.GetCode()));
            foreach (PostgreSQLProcedure proc in db.Procedures)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(proc.ID, null, DbObjectsTypes.Procedure, proc.Name, proc.GetCode()));
        }
    }
}
