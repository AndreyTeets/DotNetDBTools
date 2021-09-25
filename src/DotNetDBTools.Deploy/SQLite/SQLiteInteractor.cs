using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite.Queries;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Deploy.SQLite.Queries.SQLiteSysInfo;
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
            List<SQLiteTableInfo> tables = new();
            IEnumerable<string> tablesMetadatas = _queryExecutor.Query<string>(new GetTablesFromDNDBTSysInfoQuery());
            foreach (string tableMetadata in tablesMetadatas)
            {
                SQLiteTableInfo table = SQLiteDbObjectsSerializer.TableFromJson(tableMetadata);
                tables.Add(table);
            }

            return new SQLiteDatabaseInfo(null)
            {
                Tables = tables,
            };
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

        public void ApplyDatabaseDiff(SQLiteDatabaseDiff databaseDiff)
        {
            foreach (SQLiteTableInfo table in databaseDiff.RemovedTables)
                DropTable(table);
            foreach (SQLiteTableDiff tableDiff in databaseDiff.ChangedTables)
                AlterTable(tableDiff);
            foreach (SQLiteTableInfo table in databaseDiff.AddedTables)
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
                string tableMetadata = SQLiteDbObjectsSerializer.TableToJson(table);
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, SQLiteDbObjectsTypes.Table, table.Name, tableMetadata));
            }
        }

        private void CreateTable(SQLiteTableInfo table)
        {
            string tableMetadata = SQLiteDbObjectsSerializer.TableToJson(table);
            _queryExecutor.Execute(new CreateTableQuery(table, tableMetadata));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, SQLiteDbObjectsTypes.Table, table.Name, tableMetadata));
        }

        private void DropTable(SQLiteTableInfo table)
        {
            _queryExecutor.Execute(new DropTableQuery(table));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(table.ID));
        }

        private void AlterTable(SQLiteTableDiff tableDiff)
        {
            string newTableMetadata = SQLiteDbObjectsSerializer.TableToJson((SQLiteTableInfo)tableDiff.NewTable);
            _queryExecutor.Execute(new AlterTableQuery(tableDiff, newTableMetadata));
            _queryExecutor.Execute(new UpdateDNDBTSysInfoQuery(tableDiff.NewTable.ID, tableDiff.NewTable.Name, newTableMetadata));
        }
    }
}
