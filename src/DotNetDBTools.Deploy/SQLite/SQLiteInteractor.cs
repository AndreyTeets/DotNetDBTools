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

        public override DatabaseInfo GetDatabaseModelFromDNDBTSysInfo()
        {
            SQLiteDatabaseInfo databaseInfo = (SQLiteDatabaseInfo)GenerateDatabaseModelFromDBMSSysInfo();

            GetAllDbObjectsFromDNDBTSysInfoQuery.ResultsInterpreter.ReplaceDbModelObjectsIDsWithRecordOnes(
                databaseInfo,
                QueryExecutor.Query<GetAllDbObjectsFromDNDBTSysInfoQuery.DNDBTDbObjectRecord>(
                    new GetAllDbObjectsFromDNDBTSysInfoQuery()));

            return databaseInfo;
        }

        public override DatabaseInfo GenerateDatabaseModelFromDBMSSysInfo()
        {
            Dictionary<string, TableInfo> tables =
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

            return new SQLiteDatabaseInfo(null)
            {
                Tables = tables.Select(x => x.Value),
                Views = new List<ViewInfo>(),
            };
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff)
        {
            SQLiteDatabaseDiff dbDiff = (SQLiteDatabaseDiff)databaseDiff;
            QueryExecutor.BeginTransaction();
            try
            {
                foreach (SQLiteViewInfo view in dbDiff.ViewsToDrop)
                    DropView(view);
                foreach (SQLiteTableInfo table in dbDiff.RemovedTables)
                    DropTable(table);
                foreach (SQLiteTableDiff tableDiff in dbDiff.ChangedTables)
                    AlterTable(tableDiff);
                foreach (SQLiteTableInfo table in dbDiff.AddedTables)
                    CreateTable(table);
                foreach (SQLiteViewInfo view in dbDiff.ViewsToCreate)
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

        public override void PopulateDNDBTSysTables(DatabaseInfo database)
        {
            SQLiteDatabaseInfo db = (SQLiteDatabaseInfo)database;
            foreach (SQLiteTableInfo table in db.Tables)
            {
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, SQLiteDbObjectsTypes.Table, table.Name));
                foreach (ColumnInfo column in table.Columns)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, SQLiteDbObjectsTypes.Column, column.Name));
                PrimaryKeyInfo pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, SQLiteDbObjectsTypes.PrimaryKey, pk.Name));
                foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, SQLiteDbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (CheckConstraintInfo cc in table.CheckConstraints)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, SQLiteDbObjectsTypes.CheckConstraint, cc.Name));
                foreach (IndexInfo index in table.Indexes)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, SQLiteDbObjectsTypes.Index, index.Name));
                foreach (TriggerInfo trigger in table.Triggers)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, SQLiteDbObjectsTypes.Trigger, trigger.Name));
                foreach (ForeignKeyInfo fk in table.ForeignKeys)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, SQLiteDbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (SQLiteViewInfo view in db.Views)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, SQLiteDbObjectsTypes.View, view.Name));
        }

        private void CreateTable(SQLiteTableInfo table)
        {
            QueryExecutor.Execute(new CreateTableQuery(table));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, SQLiteDbObjectsTypes.Table, table.Name));
            foreach (ColumnInfo column in table.Columns)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, SQLiteDbObjectsTypes.Column, column.Name));
            PrimaryKeyInfo pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, SQLiteDbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, SQLiteDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraintInfo cc in table.CheckConstraints)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, SQLiteDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (IndexInfo index in table.Indexes)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, SQLiteDbObjectsTypes.Index, index.Name));
            foreach (TriggerInfo trigger in table.Triggers)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, SQLiteDbObjectsTypes.Trigger, trigger.Name));
            foreach (ForeignKeyInfo fk in table.ForeignKeys)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, SQLiteDbObjectsTypes.ForeignKey, fk.Name));
        }

        private void DropTable(SQLiteTableInfo table)
        {
            QueryExecutor.Execute(new DropTableQuery(table));
            foreach (ForeignKeyInfo fk in table.ForeignKeys)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(fk.ID));
            foreach (TriggerInfo trigger in table.Triggers)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(trigger.ID));
            foreach (IndexInfo index in table.Indexes)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(index.ID));
            foreach (CheckConstraintInfo cc in table.CheckConstraints)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(cc.ID));
            foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            PrimaryKeyInfo pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(pk.ID));
            foreach (ColumnInfo column in table.Columns)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(table.ID));
        }

        private void AlterTable(SQLiteTableDiff tableDiff)
        {
            QueryExecutor.Execute(new AlterTableQuery(tableDiff));

            foreach (ForeignKeyInfo fk in tableDiff.ForeignKeysToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(fk.ID));
            foreach (TriggerInfo trigger in tableDiff.TriggersToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(trigger.ID));
            foreach (IndexInfo index in tableDiff.IndexesToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(index.ID));
            foreach (CheckConstraintInfo cc in tableDiff.CheckConstraintsToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(cc.ID));
            foreach (UniqueConstraintInfo uc in tableDiff.UniqueConstraintsToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            if (tableDiff.PrimaryKeyToDrop is not null)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(tableDiff.PrimaryKeyToDrop.ID));
            foreach (ColumnInfo column in tableDiff.RemovedColumns)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));

            QueryExecutor.Execute(new UpdateDNDBTSysInfoQuery(tableDiff.NewTable.ID, tableDiff.NewTable.Name));
            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
                QueryExecutor.Execute(new UpdateDNDBTSysInfoQuery(columnDiff.NewColumn.ID, columnDiff.NewColumn.Name));

            foreach (ColumnInfo column in tableDiff.AddedColumns)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.Column, column.Name));
            PrimaryKeyInfo pk = tableDiff.PrimaryKeyToCreate;
            if (pk is not null)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraintInfo uc in tableDiff.UniqueConstraintsToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraintInfo cc in tableDiff.CheckConstraintsToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (IndexInfo index in tableDiff.IndexesToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.Index, index.Name));
            foreach (TriggerInfo trigger in tableDiff.TriggersToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.Trigger, trigger.Name));
            foreach (ForeignKeyInfo fk in tableDiff.ForeignKeysToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.ForeignKey, fk.Name));
        }

        private void CreateView(SQLiteViewInfo view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.Code}"));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, SQLiteDbObjectsTypes.View, view.Name));
        }

        private void DropView(SQLiteViewInfo view)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP VIEW {view.Name};"));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(view.ID));
        }
    }
}
