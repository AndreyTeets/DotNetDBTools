using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite.Queries;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Deploy.SQLite.Queries.SQLiteSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite
{
    public class SQLiteInteractor
    {
        private readonly IQueryExecutor _queryExecutor;

        public SQLiteInteractor(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public SQLiteDatabaseInfo GetDatabaseModelFromDNDBTSysInfo()
        {
            SQLiteDatabaseInfo databaseInfo = GenerateDatabaseModelFromSQLiteSysInfo();

            GetAllDbObjectsFromDNDBTSysInfoQuery.ResultsInterpreter.ReplaceDbModelObjectsIDsWithRecordOnes(
                databaseInfo,
                _queryExecutor.Query<GetAllDbObjectsFromDNDBTSysInfoQuery.DNDBTDbObjectRecord>(
                    new GetAllDbObjectsFromDNDBTSysInfoQuery()));

            return databaseInfo;
        }

        public SQLiteDatabaseInfo GenerateDatabaseModelFromSQLiteSysInfo()
        {
            Dictionary<string, SQLiteTableInfo> tables =
                GetColumnsFromSQLiteSysInfoQuery.ResultsInterpreter.BuildTablesListWithColumns(
                    _queryExecutor.Query<GetColumnsFromSQLiteSysInfoQuery.ColumnRecord>(
                        new GetColumnsFromSQLiteSysInfoQuery()));

            GetPrimaryKeysFromSQLiteSysInfoQuery.ResultsInterpreter.BuildTablesPrimaryKeys(
                tables,
                _queryExecutor.Query<GetPrimaryKeysFromSQLiteSysInfoQuery.PrimaryKeyRecord>(
                    new GetPrimaryKeysFromSQLiteSysInfoQuery()));

            GetUniqueConstraintsFromSQLiteSysInfoQuery.ResultsInterpreter.BuildTablesUniqueConstraints(
                tables,
                _queryExecutor.Query<GetUniqueConstraintsFromSQLiteSysInfoQuery.UniqueConstraintRecord>(
                    new GetUniqueConstraintsFromSQLiteSysInfoQuery()));

            GetForeignKeysFromSQLiteSysInfoQuery.ResultsInterpreter.BuildTablesForeignKeys(
                tables,
                _queryExecutor.Query<GetForeignKeysFromSQLiteSysInfoQuery.ForeignKeyRecord>(
                    new GetForeignKeysFromSQLiteSysInfoQuery()));

            IEnumerable<GetTablesDefinitionsFromSQLiteSysInfoQuery.TableRecord> tableRecords =
                _queryExecutor.Query<GetTablesDefinitionsFromSQLiteSysInfoQuery.TableRecord>(
                    new GetTablesDefinitionsFromSQLiteSysInfoQuery());
            GetTablesDefinitionsFromSQLiteSysInfoQuery.ResultsInterpreter.BuildTablesConstraintNames(
                tables, tableRecords);
            GetTablesDefinitionsFromSQLiteSysInfoQuery.ResultsInterpreter.ProcessTablesIdentityColumnCandidates(
               tables, tableRecords);

            return new SQLiteDatabaseInfo(null)
            {
                Tables = tables.Select(x => x.Value),
            };
        }

        public void ApplyDatabaseDiff(SQLiteDatabaseDiff dbDiff)
        {
            foreach (SQLiteTableInfo table in dbDiff.RemovedTables)
                DropTable(table);
            foreach (SQLiteTableDiff tableDiff in dbDiff.ChangedTables)
                AlterTable(tableDiff);
            foreach (SQLiteTableInfo table in dbDiff.AddedTables)
                CreateTable(table);
        }

        public bool DNDBTSysTablesExist()
        {
            bool systemTablesExist = _queryExecutor.QuerySingleOrDefault<bool>(new CheckDNDBTSysTablesExistQuery());
            return systemTablesExist;
        }

        public void CreateDNDBTSysTables()
        {
            _queryExecutor.Execute(new CreateDNDBTSysTablesQuery());
        }

        public void DropDNDBTSysTables()
        {
            _queryExecutor.Execute(new DropDNDBTSysTablesQuery());
        }

        public void PopulateDNDBTSysTables(SQLiteDatabaseInfo database)
        {
            foreach (SQLiteTableInfo table in database.Tables)
            {
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, SQLiteDbObjectsTypes.Table, table.Name));
                foreach (ColumnInfo column in table.Columns)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, SQLiteDbObjectsTypes.Column, column.Name));

                PrimaryKeyInfo pk = table.PrimaryKey;
                if (pk is not null)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, SQLiteDbObjectsTypes.PrimaryKey, pk.Name));

                foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, SQLiteDbObjectsTypes.UniqueConstraint, uc.Name));

                foreach (ForeignKeyInfo fk in table.ForeignKeys)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, SQLiteDbObjectsTypes.ForeignKey, fk.Name));
            }
        }

        private void CreateTable(SQLiteTableInfo table)
        {
            _queryExecutor.Execute(new CreateTableQuery(table));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, SQLiteDbObjectsTypes.Table, table.Name));
            foreach (ColumnInfo column in table.Columns)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, SQLiteDbObjectsTypes.Column, column.Name));

            PrimaryKeyInfo pk = table.PrimaryKey;
            if (pk is not null)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, SQLiteDbObjectsTypes.PrimaryKey, pk.Name));

            foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, SQLiteDbObjectsTypes.UniqueConstraint, uc.Name));

            foreach (ForeignKeyInfo fk in table.ForeignKeys)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, SQLiteDbObjectsTypes.ForeignKey, fk.Name));
        }

        private void DropTable(SQLiteTableInfo table)
        {
            _queryExecutor.Execute(new DropTableQuery(table));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(table.ID));
            foreach (ColumnInfo column in table.Columns)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));

            PrimaryKeyInfo pk = table.PrimaryKey;
            if (pk is not null)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(pk.ID));

            foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));

            foreach (ForeignKeyInfo fk in table.ForeignKeys)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(fk.ID));
        }

        private void AlterTable(SQLiteTableDiff tableDiff)
        {
            _queryExecutor.Execute(new AlterTableQuery(tableDiff));

            foreach (ForeignKeyInfo fk in tableDiff.RemovedForeignKeys)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(fk.ID));
            foreach (UniqueConstraintInfo uc in tableDiff.RemovedUniqueConstraints)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            if (tableDiff.RemovedPrimaryKey is not null)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(tableDiff.RemovedPrimaryKey.ID));
            foreach (ColumnInfo column in tableDiff.RemovedColumns)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));

            _queryExecutor.Execute(new UpdateDNDBTSysInfoQuery(tableDiff.NewTable.ID, tableDiff.NewTable.Name));
            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
                _queryExecutor.Execute(new UpdateDNDBTSysInfoQuery(columnDiff.NewColumn.ID, columnDiff.NewColumn.Name));

            foreach (ColumnInfo column in tableDiff.AddedColumns)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.Column, column.Name));
            if (tableDiff.AddedPrimaryKey is not null)
            {
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(
                    tableDiff.AddedPrimaryKey.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.PrimaryKey, tableDiff.AddedPrimaryKey.Name));
            }
            foreach (UniqueConstraintInfo uc in tableDiff.AddedUniqueConstraints)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (ForeignKeyInfo fk in tableDiff.AddedForeignKeys)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, tableDiff.NewTable.ID, SQLiteDbObjectsTypes.ForeignKey, fk.Name));
        }
    }
}
