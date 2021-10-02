using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite.Queries;
using DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite
{
    internal class SQLiteInteractor : Interactor
    {
        public SQLiteInteractor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
        }

        public override bool DatabaseExists(string databaseName) { return true; }
        public override void CreateDatabase(string databaseName) { }

        public override Database GetDatabaseModelFromDNDBTSysInfo()
        {
            SQLiteDatabase database = (SQLiteDatabase)GenerateDatabaseModelFromDBMSSysInfo();

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

            IEnumerable<GetTablesDefinitionsFromDBMSSysInfoQuery.TableRecord> tableRecords =
                QueryExecutor.Query<GetTablesDefinitionsFromDBMSSysInfoQuery.TableRecord>(
                    new GetTablesDefinitionsFromDBMSSysInfoQuery());
            GetTablesDefinitionsFromDBMSSysInfoQuery.ResultsInterpreter.BuildTablesConstraintNames(
                tables, tableRecords);
            GetTablesDefinitionsFromDBMSSysInfoQuery.ResultsInterpreter.ProcessTablesIdentityColumnCandidates(
               tables, tableRecords);

            return new SQLiteDatabase(null)
            {
                Tables = tables.Select(x => x.Value),
                Views = new List<View>(),
            };
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff)
        {
            SQLiteDatabaseDiff dbDiff = (SQLiteDatabaseDiff)databaseDiff;
            QueryExecutor.BeginTransaction();
            try
            {
                foreach (SQLiteView view in dbDiff.ViewsToDrop)
                    DropView(view);
                foreach (SQLiteTable table in dbDiff.RemovedTables)
                    DropTable(table);
                foreach (SQLiteTableDiff tableDiff in dbDiff.ChangedTables)
                    AlterTable(tableDiff);
                foreach (SQLiteTable table in dbDiff.AddedTables)
                    CreateTable(table);
                foreach (SQLiteView view in dbDiff.ViewsToCreate)
                    CreateView(view);
            }
            catch (Exception)
            {
                QueryExecutor.RollbackTransaction();
                throw;
            }
            QueryExecutor.CommitTransaction();
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
            SQLiteDatabase db = (SQLiteDatabase)database;
            foreach (SQLiteTable table in db.Tables)
            {
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, SQLiteDbObjectsTypes.Table, table.Name));
                foreach (Column column in table.Columns)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, SQLiteDbObjectsTypes.Column, column.Name));
                PrimaryKey pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, SQLiteDbObjectsTypes.PrimaryKey, pk.Name));
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, SQLiteDbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (CheckConstraint cc in table.CheckConstraints)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, SQLiteDbObjectsTypes.CheckConstraint, cc.Name));
                foreach (Index index in table.Indexes)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, SQLiteDbObjectsTypes.Index, index.Name));
                foreach (Trigger trigger in table.Triggers)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, SQLiteDbObjectsTypes.Trigger, trigger.Name));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, SQLiteDbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (SQLiteView view in db.Views)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, SQLiteDbObjectsTypes.View, view.Name));
        }

        private void CreateTable(SQLiteTable table)
        {
            QueryExecutor.Execute(new CreateTableQuery(table));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, SQLiteDbObjectsTypes.Table, table.Name));
            foreach (Column column in table.Columns)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, SQLiteDbObjectsTypes.Column, column.Name));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, SQLiteDbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, SQLiteDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraint cc in table.CheckConstraints)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, SQLiteDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (Index index in table.Indexes)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, SQLiteDbObjectsTypes.Index, index.Name));
            foreach (Trigger trigger in table.Triggers)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, SQLiteDbObjectsTypes.Trigger, trigger.Name));
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, SQLiteDbObjectsTypes.ForeignKey, fk.Name));
        }

        private void DropTable(SQLiteTable table)
        {
            QueryExecutor.Execute(new DropTableQuery(table));
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(fk.ID));
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

        private void AlterTable(SQLiteTableDiff tableDiff)
        {
            QueryExecutor.Execute(new AlterTableQuery(tableDiff));

            foreach (ForeignKey fk in tableDiff.ForeignKeysToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(fk.ID));
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
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.Column, column.Name));
            PrimaryKey pk = tableDiff.PrimaryKeyToCreate;
            if (pk is not null)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraint cc in tableDiff.CheckConstraintsToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (Index index in tableDiff.IndexesToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.Index, index.Name));
            foreach (Trigger trigger in tableDiff.TriggersToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.Trigger, trigger.Name));
            foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.ForeignKey, fk.Name));
        }

        private void CreateView(SQLiteView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.Code}"));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, SQLiteDbObjectsTypes.View, view.Name));
        }

        private void DropView(SQLiteView view)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP VIEW {view.Name};"));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(view.ID));
        }
    }
}
