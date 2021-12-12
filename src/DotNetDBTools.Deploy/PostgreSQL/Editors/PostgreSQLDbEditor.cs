using System;
using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors
{
    internal class PostgreSQLDbEditor : DbEditor<
        PostgreSQLCheckDNDBTSysTablesExistQuery,
        PostgreSQLCreateDNDBTSysTablesQuery,
        PostgreSQLDropDNDBTSysTablesQuery>
    {
        private readonly ITableEditor _tableEditor;
        private readonly IForeignKeyEditor _foreignKeyEditor;

        public PostgreSQLDbEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
            _tableEditor = new PostgreSQLTableEditor(queryExecutor);
            _foreignKeyEditor = new PostgreSQLForeignKeyEditor(queryExecutor);
        }

        public override void PopulateDNDBTSysTables(Database database)
        {
            PostgreSQLDatabase db = (PostgreSQLDatabase)database;
            foreach (PostgreSQLTable table in db.Tables)
            {
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(table.ID, null, DbObjectsTypes.Table, table.Name));
                foreach (Column c in table.Columns)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(c.ID, table.ID, DbObjectsTypes.Column, c.Name, c.GetExtraInfo()));
                PrimaryKey pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(pk.ID, table.ID, DbObjectsTypes.PrimaryKey, pk.Name));
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(uc.ID, table.ID, DbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (CheckConstraint cc in table.CheckConstraints)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(cc.ID, table.ID, DbObjectsTypes.CheckConstraint, cc.Name));
                foreach (Index index in table.Indexes)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(index.ID, table.ID, DbObjectsTypes.Index, index.Name));
                foreach (Trigger trigger in table.Triggers)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(trigger.ID, table.ID, DbObjectsTypes.Trigger, trigger.Name));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(fk.ID, table.ID, DbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (PostgreSQLFunction function in db.Functions)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(function.ID, null, DbObjectsTypes.Function, function.Name));
            foreach (PostgreSQLView view in db.Views)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name));
            foreach (PostgreSQLProcedure procedure in db.Procedures)
                QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(procedure.ID, null, DbObjectsTypes.Procedure, procedure.Name));
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff)
        {
            PostgreSQLDatabaseDiff dbDiff = (PostgreSQLDatabaseDiff)databaseDiff;
            QueryExecutor.BeginTransaction();
            try
            {
                Dictionary<Guid, Table> newDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.NewDatabase.Tables);
                Dictionary<Guid, Table> oldDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.OldDatabase.Tables);

                foreach (PostgreSQLProcedure procedure in dbDiff.ProceduresToDrop)
                    DropProcedure(procedure);
                foreach (PostgreSQLView view in dbDiff.ViewsToDrop)
                    DropView(view);
                foreach (PostgreSQLFunction function in dbDiff.FunctionsToDrop)
                    DropFunction(function);
                foreach (ForeignKey fk in dbDiff.AllForeignKeysToDrop)
                    _foreignKeyEditor.DropForeignKey(fk, oldDbFKToTableMap);
                foreach (PostgreSQLTable table in dbDiff.RemovedTables)
                    _tableEditor.DropTable(table);

                foreach (PostgreSQLTableDiff tableDiff in dbDiff.ChangedTables)
                    _tableEditor.AlterTable(tableDiff);

                foreach (PostgreSQLTable table in dbDiff.AddedTables)
                    _tableEditor.CreateTable(table);
                foreach (ForeignKey fk in dbDiff.AllForeignKeysToCreate)
                    _foreignKeyEditor.CreateForeignKey(fk, newDbFKToTableMap);
                foreach (PostgreSQLFunction function in dbDiff.FunctionsToCreate)
                    CreateFunction(function);
                foreach (PostgreSQLView view in dbDiff.ViewsToCreate)
                    CreateView(view);
                foreach (PostgreSQLProcedure procedure in dbDiff.ProceduresToCreate)
                    CreateProcedure(procedure);
            }
            catch (Exception)
            {
                QueryExecutor.RollbackTransaction();
                throw;
            }
            QueryExecutor.CommitTransaction();
        }

        private void CreateFunction(PostgreSQLFunction function)
        {
            QueryExecutor.Execute(new GenericQuery($"{function.Code}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(function.ID, null, DbObjectsTypes.Function, function.Name));
        }

        private void DropFunction(PostgreSQLFunction function)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP FUNCTION ""{function.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(function.ID));
        }

        private void CreateView(PostgreSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.Code}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name));
        }

        private void DropView(PostgreSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP VIEW ""{view.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateProcedure(PostgreSQLProcedure procedure)
        {
            QueryExecutor.Execute(new GenericQuery($"{procedure.Code}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(procedure.ID, null, DbObjectsTypes.Procedure, procedure.Name));
        }

        private void DropProcedure(PostgreSQLProcedure procedure)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP PROCEDURE ""{procedure.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(procedure.ID));
        }
    }
}
