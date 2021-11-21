using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Queries;
using DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL
{
    internal class MySQLInteractor : Interactor
    {
        public MySQLInteractor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
        }

        public override Database GetDatabaseModelFromDNDBTSysInfo()
        {
            MySQLDatabase database = (MySQLDatabase)GenerateDatabaseModelFromDBMSSysInfo();

            GetAllDbObjectsFromDNDBTSysInfoQuery.ResultsInterpreter.ReplaceDbModelObjectsIDsWithRecordOnes(
                database,
                QueryExecutor.Query<GetAllDbObjectsFromDNDBTSysInfoQuery.DNDBTDbObjectRecord>(
                    new GetAllDbObjectsFromDNDBTSysInfoQuery()));

            return database;
        }

        public override Database GenerateDatabaseModelFromDBMSSysInfo()
        {
            Dictionary<string, Table> tables =
                GetColumnsFromDBMSSysInfoQuery.ResultsInterpreter.BuildTablesListWithColumns(
                    QueryExecutor.Query<GetColumnsFromDBMSSysInfoQuery.ColumnRecord>(
                        new GetColumnsFromDBMSSysInfoQuery()));

            GetPrimaryKeysFromDBMSSysInfoQuery.ResultsInterpreter.BuildTablesPrimaryKeys(
                tables,
                QueryExecutor.Query<GetPrimaryKeysFromDBMSSysInfoQuery.PrimaryKeyRecord>(
                    new GetPrimaryKeysFromDBMSSysInfoQuery()));

            GetUniqueConstraintsFromDBMSSysInfoQuery.ResultsInterpreter.BuildTablesUniqueConstraints(
                tables,
                QueryExecutor.Query<GetUniqueConstraintsFromDBMSSysInfoQuery.UniqueConstraintRecord>(
                    new GetUniqueConstraintsFromDBMSSysInfoQuery()));

            GetForeignKeysFromDBMSSysInfoQuery.ResultsInterpreter.BuildTablesForeignKeys(
                tables,
                QueryExecutor.Query<GetForeignKeysFromDBMSSysInfoQuery.ForeignKeyRecord>(
                    new GetForeignKeysFromDBMSSysInfoQuery()));

            return new MySQLDatabase(null)
            {
                Tables = tables.Select(x => x.Value),
                Views = new List<View>(),
                Functions = new List<MySQLFunction>(),
                Procedures = new List<MySQLProcedure>(),
            };
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
                DropForeignKey(fk, oldDbFKToTableMap);
            foreach (MySQLTable table in dbDiff.RemovedTables)
                DropTable(table);

            foreach (MySQLTableDiff tableDiff in dbDiff.ChangedTables)
                AlterTable(tableDiff);

            foreach (MySQLTable table in dbDiff.AddedTables)
                CreateTable(table);
            foreach (ForeignKey fk in dbDiff.AllForeignKeysToCreate)
                CreateForeignKey(fk, newDbFKToTableMap);
            foreach (MySQLFunction function in dbDiff.FunctionsToCreate)
                CreateFunction(function);
            foreach (MySQLView view in dbDiff.ViewsToCreate)
                CreateView(view);
            foreach (MySQLProcedure procedure in dbDiff.ProceduresToCreate)
                CreateProcedure(procedure);
        }

        public override bool DNDBTSysTablesExist()
        {
            return QueryExecutor.QuerySingleOrDefault<bool>(new CheckDNDBTSysTablesExistQuery());
        }

        public override void CreateDNDBTSysTables()
        {
            QueryExecutor.Execute(new CreateDNDBTSysTablesQuery());
        }

        public override void DropDNDBTSysTables()
        {
            QueryExecutor.Execute(new DropDNDBTSysTablesQuery());
        }

        public override void PopulateDNDBTSysTables(Database database)
        {
            MySQLDatabase db = (MySQLDatabase)database;
            foreach (MySQLTable table in db.Tables)
            {
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, MySQLDbObjectsTypes.Table, table.Name));
                foreach (Column column in table.Columns)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, MySQLDbObjectsTypes.Column, column.Name));
                PrimaryKey pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, MySQLDbObjectsTypes.PrimaryKey, pk.Name));
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, MySQLDbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (CheckConstraint cc in table.CheckConstraints)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, MySQLDbObjectsTypes.CheckConstraint, cc.Name));
                foreach (Index index in table.Indexes)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, MySQLDbObjectsTypes.Index, index.Name));
                foreach (Trigger trigger in table.Triggers)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, MySQLDbObjectsTypes.Trigger, trigger.Name));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, MySQLDbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (MySQLFunction function in db.Functions)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(function.ID, null, MySQLDbObjectsTypes.Function, function.Name));
            foreach (MySQLView view in db.Views)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, MySQLDbObjectsTypes.View, view.Name));
            foreach (MySQLProcedure procedure in db.Procedures)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(procedure.ID, null, MySQLDbObjectsTypes.Procedure, procedure.Name));
        }

        private void CreateTable(MySQLTable table)
        {
            QueryExecutor.Execute(new CreateTableQuery(table));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, MySQLDbObjectsTypes.Table, table.Name));
            foreach (Column column in table.Columns)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, MySQLDbObjectsTypes.Column, column.Name));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, MySQLDbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, MySQLDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraint cc in table.CheckConstraints)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, MySQLDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (Index index in table.Indexes)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, MySQLDbObjectsTypes.Index, index.Name));
            foreach (Trigger trigger in table.Triggers)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, MySQLDbObjectsTypes.Trigger, trigger.Name));
        }

        private void DropTable(MySQLTable table)
        {
            QueryExecutor.Execute(new DropTableQuery(table));
            foreach (Trigger trigger in table.Triggers)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(trigger.ID));
            foreach (Index index in table.Indexes)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(index.ID));
            foreach (CheckConstraint cc in table.CheckConstraints)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(cc.ID));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(pk.ID));
            foreach (Column column in table.Columns)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(table.ID));
        }

        private void AlterTable(MySQLTableDiff tableDiff)
        {
            QueryExecutor.Execute(new AlterTableQuery(tableDiff));

            foreach (Trigger trigger in tableDiff.TriggersToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(trigger.ID));
            foreach (Index index in tableDiff.IndexesToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(index.ID));
            foreach (CheckConstraint cc in tableDiff.CheckConstraintsToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(cc.ID));
            foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            if (tableDiff.PrimaryKeyToDrop is not null)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(tableDiff.PrimaryKeyToDrop.ID));
            foreach (Column column in tableDiff.RemovedColumns)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));

            QueryExecutor.Execute(new UpdateDNDBTSysInfoQuery(tableDiff.NewTable.ID, tableDiff.NewTable.Name));
            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
                QueryExecutor.Execute(new UpdateDNDBTSysInfoQuery(columnDiff.NewColumn.ID, columnDiff.NewColumn.Name));

            foreach (Column column in tableDiff.AddedColumns)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, tableDiff.NewTable.ID, MySQLDbObjectsTypes.Column, column.Name));
            PrimaryKey pk = tableDiff.PrimaryKeyToCreate;
            if (pk is not null)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, tableDiff.NewTable.ID, MySQLDbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, tableDiff.NewTable.ID, MySQLDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraint cc in tableDiff.CheckConstraintsToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, tableDiff.NewTable.ID, MySQLDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (Index index in tableDiff.IndexesToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, tableDiff.NewTable.ID, MySQLDbObjectsTypes.Index, index.Name));
            foreach (Trigger trigger in tableDiff.TriggersToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, tableDiff.NewTable.ID, MySQLDbObjectsTypes.Trigger, trigger.Name));
        }

        private void CreateForeignKey(ForeignKey fk, Dictionary<Guid, Table> fkToTableMap)
        {
            QueryExecutor.Execute(new CreateForeignKeyQuery(fk, fkToTableMap[fk.ID].Name));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, fkToTableMap[fk.ID].ID, MySQLDbObjectsTypes.ForeignKey, fk.Name));
        }

        private void DropForeignKey(ForeignKey fk, Dictionary<Guid, Table> fkToTableMap)
        {
            QueryExecutor.Execute(new DropForeignKeyQuery(fk, fkToTableMap[fk.ID].Name));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(fk.ID));
        }

        private void CreateFunction(MySQLFunction function)
        {
            QueryExecutor.Execute(new GenericQuery($"{function.Code}"));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(function.ID, null, MySQLDbObjectsTypes.Function, function.Name));
        }

        private void DropFunction(MySQLFunction function)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION `{function.Name}`;"));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(function.ID));
        }

        private void CreateView(MySQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.Code}"));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, MySQLDbObjectsTypes.View, view.Name));
        }

        private void DropView(MySQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP VIEW `{view.Name}`;"));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateProcedure(MySQLProcedure procedure)
        {
            QueryExecutor.Execute(new GenericQuery($"{procedure.Code}"));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(procedure.ID, null, MySQLDbObjectsTypes.Procedure, procedure.Name));
        }

        private void DropProcedure(MySQLProcedure procedure)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE `{procedure.Name}`;"));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(procedure.ID));
        }
    }
}
