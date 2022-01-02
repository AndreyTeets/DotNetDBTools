using System;
using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
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
        private readonly IIndexEditor _indexEditor;
        private readonly ITriggerEditor _triggerEditor;
        private readonly IForeignKeyEditor _foreignKeyEditor;
        private readonly PostgreSQLUserDefinedTypesEditor _userDefinedTypesEditor;

        public PostgreSQLDbEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
            _tableEditor = new PostgreSQLTableEditor(queryExecutor);
            _indexEditor = new PostgreSQLIndexEditor(queryExecutor);
            _triggerEditor = new PostgreSQLTriggerEditor(queryExecutor);
            _foreignKeyEditor = new PostgreSQLForeignKeyEditor(queryExecutor);
            _userDefinedTypesEditor = new PostgreSQLUserDefinedTypesEditor(queryExecutor);
        }

        public override void PopulateDNDBTSysTables(Database database)
        {
            PostgreSQLDatabase db = (PostgreSQLDatabase)database;
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
                foreach (CheckConstraint ck in table.CheckConstraints)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(ck.ID, table.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetCode()));
                foreach (Index idx in table.Indexes)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(idx.ID, table.ID, DbObjectsTypes.Index, idx.Name));
                foreach (Trigger trg in table.Triggers)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(trg.ID, table.ID, DbObjectsTypes.Trigger, trg.Name, trg.GetCode()));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(fk.ID, table.ID, DbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (PostgreSQLFunction func in db.Functions)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(func.ID, null, DbObjectsTypes.Function, func.Name, func.GetCode()));
            foreach (PostgreSQLView view in db.Views)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name, view.GetCode()));
            foreach (PostgreSQLProcedure proc in db.Procedures)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(proc.ID, null, DbObjectsTypes.Procedure, proc.Name, proc.GetCode()));
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff)
        {
            PostgreSQLDatabaseDiff dbDiff = (PostgreSQLDatabaseDiff)databaseDiff;
            QueryExecutor.BeginTransaction();
            try
            {
                _triggerEditor.DropTriggers(dbDiff);
                foreach (PostgreSQLProcedure procedure in dbDiff.ProceduresToDrop)
                    DropProcedure(procedure);
                foreach (PostgreSQLView view in dbDiff.ViewsToDrop)
                    DropView(view);
                foreach (PostgreSQLFunction function in dbDiff.FunctionsToDrop)
                    DropFunction(function);
                _foreignKeyEditor.DropForeignKeys(dbDiff);
                _indexEditor.DropIndexes(dbDiff);
                foreach (PostgreSQLTable table in dbDiff.RemovedTables)
                    _tableEditor.DropTable(table);

                _userDefinedTypesEditor.RenameAllUserDefinedTypesToTempInDbAndInDbDiff(dbDiff);
                _userDefinedTypesEditor.CreateAllUserDefinedTypes(dbDiff);
                foreach (PostgreSQLTableDiff tableDiff in dbDiff.ChangedTables)
                    _tableEditor.AlterTable(tableDiff);
                _userDefinedTypesEditor.DropAllUserDefinedTypes(dbDiff);

                foreach (PostgreSQLTable table in dbDiff.AddedTables)
                    _tableEditor.CreateTable(table);
                _indexEditor.CreateIndexes(dbDiff);
                _foreignKeyEditor.CreateForeignKeys(dbDiff);
                foreach (PostgreSQLFunction function in dbDiff.FunctionsToCreate)
                    CreateFunction(function);
                foreach (PostgreSQLView view in dbDiff.ViewsToCreate)
                    CreateView(view);
                foreach (PostgreSQLProcedure procedure in dbDiff.ProceduresToCreate)
                    CreateProcedure(procedure);
                _triggerEditor.CreateTriggers(dbDiff);
            }
            catch (Exception)
            {
                QueryExecutor.RollbackTransaction();
                throw;
            }
            QueryExecutor.CommitTransaction();
        }

        private void CreateFunction(PostgreSQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($"{func.GetCode()}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(func.ID, null, DbObjectsTypes.Function, func.Name, func.GetCode()));
        }

        private void DropFunction(PostgreSQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP FUNCTION ""{func.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(func.ID));
        }

        private void CreateView(PostgreSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.GetCode()}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name, view.GetCode()));
        }

        private void DropView(PostgreSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP VIEW ""{view.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateProcedure(PostgreSQLProcedure proc)
        {
            QueryExecutor.Execute(new GenericQuery($"{proc.GetCode()}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(proc.ID, null, DbObjectsTypes.Procedure, proc.Name, proc.GetCode()));
        }

        private void DropProcedure(PostgreSQLProcedure proc)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP PROCEDURE ""{proc.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(proc.ID));
        }
    }
}
