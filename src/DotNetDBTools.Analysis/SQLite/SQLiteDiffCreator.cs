using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite
{
    public static class SQLiteDiffCreator
    {
        private static readonly DbObjectsEqualityComparer s_dbObjectsEqualityComparer = new();

        public static SQLiteDatabaseDiff CreateDatabaseDiff(SQLiteDatabaseInfo newDatabase, SQLiteDatabaseInfo oldDatabase)
        {
            List<SQLiteTableInfo> addedTables = newDatabase.Tables
                .Where(newDbTable => !oldDatabase.Tables.Any(oldDbTable => oldDbTable.ID == newDbTable.ID))
                .Select(x => (SQLiteTableInfo)x)
                .ToList();
            List<SQLiteTableInfo> removedTables = oldDatabase.Tables
                .Where(oldDbTable => !newDatabase.Tables.Any(newDbTable => newDbTable.ID == oldDbTable.ID))
                .Select(x => (SQLiteTableInfo)x)
                .ToList();
            List<SQLiteTableDiff> changedTables = new();
            foreach (SQLiteTableInfo newDbTable in newDatabase.Tables)
            {
                SQLiteTableInfo oldDbTable = (SQLiteTableInfo)oldDatabase.Tables.FirstOrDefault(x => x.ID == newDbTable.ID);
                if (oldDbTable is not null &&
                    !s_dbObjectsEqualityComparer.Equals(newDbTable, oldDbTable))
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
                ViewsToCreate = new List<SQLiteViewInfo>(),
                ViewsToDrop = new List<SQLiteViewInfo>(),
            };
        }

        private static SQLiteTableDiff CreateTableDiff(SQLiteTableInfo newDbTable, SQLiteTableInfo oldDbTable)
        {
            List<ColumnInfo> addedColumns = newDbTable.Columns
                .Where(newDbTableColumn => !oldDbTable.Columns.Any(oldDbTableColumn => oldDbTableColumn.ID == newDbTableColumn.ID))
                .ToList();
            List<ColumnInfo> removedColumns = oldDbTable.Columns
                .Where(oldDbTableColumn => !newDbTable.Columns.Any(newDbTableColumn => newDbTableColumn.ID == oldDbTableColumn.ID))
                .ToList();
            List<ColumnDiff> changedColumns = new();
            foreach (ColumnInfo newDbTableColumn in newDbTable.Columns)
            {
                ColumnInfo oldDbTableColumn = oldDbTable.Columns.SingleOrDefault(x => x.ID == newDbTableColumn.ID);
                if (oldDbTableColumn is not null &&
                    !s_dbObjectsEqualityComparer.Equals(newDbTableColumn, oldDbTableColumn))
                {
                    ColumnDiff columnDiff = new()
                    {
                        NewColumn = newDbTableColumn,
                        OldColumn = oldDbTableColumn,
                    };
                    changedColumns.Add(columnDiff);
                }
            }

            PrimaryKeyInfo primaryKeyToCreate = null;
            PrimaryKeyInfo primaryKeyToDrop = null;
            if (!s_dbObjectsEqualityComparer.Equals(newDbTable.PrimaryKey, oldDbTable.PrimaryKey))
            {
                primaryKeyToCreate = newDbTable.PrimaryKey;
                primaryKeyToDrop = oldDbTable.PrimaryKey;
            }

            List<UniqueConstraintInfo> uniqueConstraintsToCreate = newDbTable.UniqueConstraints
                .Where(newDbTableUC => !oldDbTable.UniqueConstraints.Any(oldDbTableUC => oldDbTableUC.ID == newDbTableUC.ID))
                .ToList();
            List<UniqueConstraintInfo> uniqueConstraintsToDrop = oldDbTable.UniqueConstraints
                .Where(oldDbTableUC => !newDbTable.UniqueConstraints.Any(newDbTableUC => newDbTableUC.ID == oldDbTableUC.ID))
                .ToList();
            foreach (UniqueConstraintInfo newDbTableUC in newDbTable.UniqueConstraints)
            {
                UniqueConstraintInfo oldDbTableUC = oldDbTable.UniqueConstraints.SingleOrDefault(x => x.ID == newDbTableUC.ID);
                if (oldDbTableUC is not null &&
                    !s_dbObjectsEqualityComparer.Equals(newDbTableUC, oldDbTableUC))
                {
                    uniqueConstraintsToCreate.Add(newDbTableUC);
                    uniqueConstraintsToDrop.Add(oldDbTableUC);
                }
            }

            List<ForeignKeyInfo> foreignKeysToCreate = newDbTable.ForeignKeys
                .Where(newDbTableFK => !oldDbTable.ForeignKeys.Any(oldDbTableFK => oldDbTableFK.ID == newDbTableFK.ID))
                .ToList();
            List<ForeignKeyInfo> foreignKeysToDrop = oldDbTable.ForeignKeys
                .Where(oldDbTableFK => !newDbTable.ForeignKeys.Any(newDbTableFK => newDbTableFK.ID == oldDbTableFK.ID))
                .ToList();
            foreach (ForeignKeyInfo newDbTableFK in newDbTable.ForeignKeys)
            {
                ForeignKeyInfo oldDbTableFK = oldDbTable.ForeignKeys.SingleOrDefault(x => x.ID == newDbTableFK.ID);
                if (oldDbTableFK is not null &&
                    !s_dbObjectsEqualityComparer.Equals(newDbTableFK, oldDbTableFK))
                {
                    foreignKeysToCreate.Add(newDbTableFK);
                    foreignKeysToDrop.Add(oldDbTableFK);
                }
            }

            return new SQLiteTableDiff
            {
                NewTable = newDbTable,
                OldTable = oldDbTable,
                AddedColumns = addedColumns,
                RemovedColumns = removedColumns,
                ChangedColumns = changedColumns,
                PrimaryKeyToCreate = primaryKeyToCreate,
                PrimaryKeyToDrop = primaryKeyToDrop,
                UniqueConstraintsToCreate = uniqueConstraintsToCreate,
                UniqueConstraintsToDrop = uniqueConstraintsToDrop,
                CheckConstraintsToCreate = new List<CheckConstraintInfo>(),
                CheckConstraintsToDrop = new List<CheckConstraintInfo>(),
                ForeignKeysToCreate = foreignKeysToCreate,
                ForeignKeysToDrop = foreignKeysToDrop,
                IndexesToCreate = new List<IndexInfo>(),
                IndexesToDrop = new List<IndexInfo>(),
                TriggersToCreate = new List<TriggerInfo>(),
                TriggersToDrop = new List<TriggerInfo>(),
            };
        }
    }
}
