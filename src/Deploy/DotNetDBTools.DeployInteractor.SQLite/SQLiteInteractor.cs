﻿using System.Collections.Generic;
using DotNetDBTools.DeployInteractor.SQLite.Queries;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    public class SQLiteInteractor
    {
        private readonly IQueryExecutor _queryExecutor;

        public SQLiteInteractor(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public void UpdateDatabase(SQLiteDatabaseDiff databaseDiff)
        {
            foreach (SQLiteTableInfo table in databaseDiff.AddedTables)
                CreateTable(table);
            foreach (SQLiteTableInfo table in databaseDiff.RemovedTables)
                DropTable(table);
            foreach (SQLiteTableDiff tableDiff in databaseDiff.ChangedTables)
                AlterTable(tableDiff);
        }

        public bool DatabaseExists()
        {
            bool databaseExists = _queryExecutor.QuerySingleOrDefault<bool>(new DatabaseExistsQuery());
            return databaseExists;
        }

        public void CreateEmptyDatabase()
        {
            _queryExecutor.Execute(new CreateEmptyDatabaseQuery());
        }

        public SQLiteDatabaseInfo GetExistingDatabase()
        {
            SQLiteDatabaseInfo databaseInfo = new();
            List<SQLiteTableInfo> tables = new();
            IEnumerable<string> tablesMetadatas = _queryExecutor.Query<string>(new GetExistingTablesQuery());
            foreach (string tableMetadata in tablesMetadatas)
            {
                SQLiteTableInfo table = SQLiteDbObjectsSerializer.TableFromJson(tableMetadata);
                tables.Add(table);
            }
            databaseInfo.Tables = tables;
            return databaseInfo;
        }

        private void CreateTable(SQLiteTableInfo table)
        {
            string tableMetadata = SQLiteDbObjectsSerializer.TableToJson(table);
            _queryExecutor.Execute(new CreateTableQuery(table, tableMetadata));
        }

        private void DropTable(SQLiteTableInfo table)
        {
            _queryExecutor.Execute(new DropTableQuery(table));
        }

        private void AlterTable(SQLiteTableDiff tableDiff)
        {
            string newTableMetadata = SQLiteDbObjectsSerializer.TableToJson(tableDiff.NewTable);
            _queryExecutor.Execute(new AlterTableQuery(tableDiff, newTableMetadata));
        }
    }
}
