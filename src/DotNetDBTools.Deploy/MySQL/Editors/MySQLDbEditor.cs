using System;
using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Common.Editors;
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
        private readonly IForeignKeyEditor _foreignKeyEditor;

        public MySQLDbEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
            _tableEditor = new MySQLTableEditor(queryExecutor);
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
                foreach (Index index in table.Indexes)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(index.ID, table.ID, DbObjectsTypes.Index, index.Name));
                foreach (Trigger trigger in table.Triggers)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(trigger.ID, table.ID, DbObjectsTypes.Trigger, trigger.Name));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(fk.ID, table.ID, DbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (MySQLFunction function in db.Functions)
                QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(function.ID, null, DbObjectsTypes.Function, function.Name));
            foreach (MySQLView view in db.Views)
                QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name));
            foreach (MySQLProcedure procedure in db.Procedures)
                QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(procedure.ID, null, DbObjectsTypes.Procedure, procedure.Name));
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff)
        {
            MySQLDatabaseDiff dbDiff = (MySQLDatabaseDiff)databaseDiff;

            Dictionary<Guid, Table> newDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.NewDatabase.Tables);
            Dictionary<Guid, Table> oldDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.OldDatabase.Tables);

            foreach (MySQLProcedure procedure in dbDiff.ProceduresToDrop)
                DropProcedure(procedure);
            foreach (MySQLView view in dbDiff.ViewsToDrop)
                DropView(view);
            foreach (MySQLFunction function in dbDiff.FunctionsToDrop)
                DropFunction(function);
            foreach (ForeignKey fk in dbDiff.AllForeignKeysToDrop)
                _foreignKeyEditor.DropForeignKey(fk, oldDbFKToTableMap);
            foreach (MySQLTable table in dbDiff.RemovedTables)
                _tableEditor.DropTable(table);

            foreach (MySQLTableDiff tableDiff in dbDiff.ChangedTables)
                _tableEditor.AlterTable(tableDiff);

            foreach (MySQLTable table in dbDiff.AddedTables)
                _tableEditor.CreateTable(table);
            foreach (ForeignKey fk in dbDiff.AllForeignKeysToCreate)
                _foreignKeyEditor.CreateForeignKey(fk, newDbFKToTableMap);
            foreach (MySQLFunction function in dbDiff.FunctionsToCreate)
                CreateFunction(function);
            foreach (MySQLView view in dbDiff.ViewsToCreate)
                CreateView(view);
            foreach (MySQLProcedure procedure in dbDiff.ProceduresToCreate)
                CreateProcedure(procedure);
        }

        private void CreateFunction(MySQLFunction function)
        {
            QueryExecutor.Execute(new GenericQuery($"{function.CodePiece}"));
            QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(function.ID, null, DbObjectsTypes.Function, function.Name));
        }

        private void DropFunction(MySQLFunction function)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION `{function.Name}`;"));
            QueryExecutor.Execute(new MySQLDeleteDNDBTSysInfoQuery(function.ID));
        }

        private void CreateView(MySQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.CodePiece}"));
            QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name));
        }

        private void DropView(MySQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP VIEW `{view.Name}`;"));
            QueryExecutor.Execute(new MySQLDeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateProcedure(MySQLProcedure procedure)
        {
            QueryExecutor.Execute(new GenericQuery($"{procedure.CodePiece}"));
            QueryExecutor.Execute(new MySQLInsertDNDBTSysInfoQuery(procedure.ID, null, DbObjectsTypes.Procedure, procedure.Name));
        }

        private void DropProcedure(MySQLProcedure procedure)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE `{procedure.Name}`;"));
            QueryExecutor.Execute(new MySQLDeleteDNDBTSysInfoQuery(procedure.ID));
        }
    }
}
