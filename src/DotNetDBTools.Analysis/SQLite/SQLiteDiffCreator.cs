using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite
{
    public static class SQLiteDiffCreator
    {
        public static SQLiteDatabaseDiff CreateDatabaseDiff(SQLiteDatabaseInfo newDatabase, SQLiteDatabaseInfo oldDatabase)
        {
            IEnumerable<SQLiteTableInfo> addedTables = newDatabase.Tables
                .Where(newDbTable => !oldDatabase.Tables.Any(oldDbTable => oldDbTable.ID == newDbTable.ID))
                .PutReferencedFirst()
                .Select(x => (SQLiteTableInfo)x);

            IEnumerable<SQLiteTableInfo> removedTables = oldDatabase.Tables
                .Where(oldDbTable => !newDatabase.Tables.Any(newDbTable => newDbTable.ID == oldDbTable.ID))
                .PutReferencedLast()
                .Select(x => (SQLiteTableInfo)x);

            List<SQLiteTableDiff> changedTables = new();
            foreach (SQLiteTableInfo newDbTable in newDatabase.Tables.PutReferencedFirst())
            {
                SQLiteTableInfo oldDbTable = (SQLiteTableInfo)oldDatabase.Tables.FirstOrDefault(x => x.ID == newDbTable.ID);
                if (oldDbTable is not null)
                {
                    SQLiteTableDiff tableDiff = CreateTableDiff(newDbTable, oldDbTable);
                    changedTables.Add(tableDiff);
                }
            }

            return new SQLiteDatabaseDiff
            {
                NewDatabase = newDatabase,
                OldDatabase = oldDatabase,
                AddedTables = addedTables,
                RemovedTables = removedTables,
                ChangedTables = changedTables,
                AddedViews = new List<SQLiteViewInfo>(),
                RemovedViews = new List<SQLiteViewInfo>(),
                ChangedViews = new List<SQLiteViewDiff>(),
            };
        }

        private static SQLiteTableDiff CreateTableDiff(SQLiteTableInfo newDbTable, SQLiteTableInfo oldDbTable)
        {
            IEnumerable<SQLiteColumnInfo> addedColumns = newDbTable.Columns
                .Where(newDbTableColumn => !oldDbTable.Columns.Any(oldDbTableColum => oldDbTableColum.ID == newDbTableColumn.ID))
                .Select(x => (SQLiteColumnInfo)x);

            IEnumerable<SQLiteColumnInfo> removedColumns = oldDbTable.Columns
                .Where(oldDbTableColumn => !newDbTable.Columns.Any(newDbTableColumn => newDbTableColumn.ID == oldDbTableColumn.ID))
                .Select(x => (SQLiteColumnInfo)x);

            List<SQLiteColumnDiff> changedColumns = new();
            foreach (SQLiteColumnInfo newDbTableColumn in newDbTable.Columns)
            {
                SQLiteColumnInfo oldDbTableColumn = (SQLiteColumnInfo)oldDbTable.Columns.FirstOrDefault(x => x.ID == newDbTableColumn.ID);
                if (oldDbTableColumn is not null)
                {
                    SQLiteColumnDiff tableDiff = CreateColumnDiff(newDbTableColumn, oldDbTableColumn);
                    changedColumns.Add(tableDiff);
                }
            }

            return new SQLiteTableDiff
            {
                NewTable = newDbTable,
                OldTable = oldDbTable,
                AddedColumns = addedColumns,
                RemovedColumns = removedColumns,
                ChangedColumns = changedColumns,
                AddedForeignKeys = new List<SQLiteForeignKeyInfo>(),
                RemovedForeignKeys = new List<SQLiteForeignKeyInfo>(),
                ChangedForeignKeys = new List<SQLiteForeignKeyDiff>(),
            };
        }

        private static SQLiteColumnDiff CreateColumnDiff(SQLiteColumnInfo newDbTableColumn, SQLiteColumnInfo oldDbTableColumn)
        {
            return new SQLiteColumnDiff
            {
                NewColumn = newDbTableColumn,
                OldColumn = oldDbTableColumn,
            };
        }
    }
}
