﻿using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL.Editors
{
    internal class MySQLDbEditor : DbEditor<
        MySQLCheckDNDBTSysTablesExistQuery,
        MySQLCreateDNDBTSysTablesQuery,
        MySQLDropDNDBTSysTablesQuery>
    {
        private readonly ITableEditor _tableEditor;
        private readonly IIndexEditor _indexEditor;
        private readonly ITriggerEditor _triggerEditor;
        private readonly IForeignKeyEditor _foreignKeyEditor;

        public MySQLDbEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
            _tableEditor = new MySQLTableEditor(queryExecutor);
            _indexEditor = new MySQLIndexEditor(queryExecutor);
            _triggerEditor = new MySQLTriggerEditor(queryExecutor);
            _foreignKeyEditor = new MySQLForeignKeyEditor(queryExecutor);
        }

        public override void PopulateDNDBTSysTables(Database database)
        {
            MySQLDatabase db = (MySQLDatabase)database;
            foreach (MySQLTable table in db.Tables)
            {
                QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(table.ID, null, DbObjectsTypes.Table, table.Name));
                foreach (Column c in table.Columns)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(c.ID, table.ID, DbObjectsTypes.Column, c.Name, c.GetCode()));
                PrimaryKey pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(pk.ID, table.ID, DbObjectsTypes.PrimaryKey, pk.Name));
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(uc.ID, table.ID, DbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (CheckConstraint ck in table.CheckConstraints)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(ck.ID, table.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetCode()));
                foreach (Index idx in table.Indexes)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(idx.ID, table.ID, DbObjectsTypes.Index, idx.Name));
                foreach (Trigger trg in table.Triggers)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(trg.ID, table.ID, DbObjectsTypes.Trigger, trg.Name, trg.GetCode()));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(fk.ID, table.ID, DbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (MySQLFunction func in db.Functions)
                QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(func.ID, null, DbObjectsTypes.Function, func.Name, func.GetCode()));
            foreach (MySQLView view in db.Views)
                QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name, view.GetCode()));
            foreach (MySQLProcedure proc in db.Procedures)
                QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(proc.ID, null, DbObjectsTypes.Procedure, proc.Name, proc.GetCode()));
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff)
        {
            MySQLDatabaseDiff dbDiff = (MySQLDatabaseDiff)databaseDiff;

            _triggerEditor.DropTriggers(dbDiff);
            foreach (MySQLProcedure procedure in dbDiff.ProceduresToDrop)
                DropProcedure(procedure);
            foreach (MySQLView view in dbDiff.ViewsToDrop)
                DropView(view);
            foreach (MySQLFunction function in dbDiff.FunctionsToDrop)
                DropFunction(function);
            _foreignKeyEditor.DropForeignKeys(dbDiff);
            _indexEditor.DropIndexes(dbDiff);
            foreach (MySQLTable table in dbDiff.RemovedTables)
                _tableEditor.DropTable(table);

            foreach (MySQLTableDiff tableDiff in dbDiff.ChangedTables)
                _tableEditor.AlterTable(tableDiff);

            foreach (MySQLTable table in dbDiff.AddedTables)
                _tableEditor.CreateTable(table);
            _indexEditor.CreateIndexes(dbDiff);
            _foreignKeyEditor.CreateForeignKeys(dbDiff);
            foreach (MySQLFunction function in dbDiff.FunctionsToCreate)
                CreateFunction(function);
            foreach (MySQLView view in dbDiff.ViewsToCreate)
                CreateView(view);
            foreach (MySQLProcedure procedure in dbDiff.ProceduresToCreate)
                CreateProcedure(procedure);
            _triggerEditor.CreateTriggers(dbDiff);
        }

        private void CreateFunction(MySQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($"{func.GetCode()}"));
            QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(func.ID, null, DbObjectsTypes.Function, func.Name, func.GetCode()));
        }

        private void DropFunction(MySQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION `{func.Name}`;"));
            QueryExecutor.Execute(new MySQLDeleteDNDBTSysInfoQuery(func.ID));
        }

        private void CreateView(MySQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.GetCode()}"));
            QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name, view.GetCode()));
        }

        private void DropView(MySQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP VIEW `{view.Name}`;"));
            QueryExecutor.Execute(new MySQLDeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateProcedure(MySQLProcedure proc)
        {
            QueryExecutor.Execute(new GenericQuery($"{proc.GetCode()}"));
            QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(proc.ID, null, DbObjectsTypes.Procedure, proc.Name, proc.GetCode()));
        }

        private void DropProcedure(MySQLProcedure proc)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE `{proc.Name}`;"));
            QueryExecutor.Execute(new MySQLDeleteDNDBTSysInfoQuery(proc.ID));
        }
    }
}
