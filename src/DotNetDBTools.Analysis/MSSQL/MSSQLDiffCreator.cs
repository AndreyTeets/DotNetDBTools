using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Shared;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL
{
    public static class MSSQLDiffCreator
    {
        public static MSSQLDatabaseDiff CreateDatabaseDiff(MSSQLDatabaseInfo newDatabase, MSSQLDatabaseInfo oldDatabase)
        {
            IEnumerable<MSSQLTableInfo> addedTables = newDatabase.Tables
                .Where(newDbTable => !oldDatabase.Tables.Any(oldDbTable => oldDbTable.ID == newDbTable.ID))
                .PutReferencedFirst()
                .Select(x => (MSSQLTableInfo)x);

            IEnumerable<MSSQLTableInfo> removedTables = oldDatabase.Tables
                .Where(oldDbTable => !newDatabase.Tables.Any(newDbTable => newDbTable.ID == oldDbTable.ID))
                .PutReferencedLast()
                .Select(x => (MSSQLTableInfo)x);

            List<MSSQLTableDiff> changedTables = new();
            foreach (MSSQLTableInfo newDbTable in newDatabase.Tables.PutReferencedFirst())
            {
                MSSQLTableInfo oldDbTable = (MSSQLTableInfo)oldDatabase.Tables.FirstOrDefault(x => x.ID == newDbTable.ID);
                if (oldDbTable is not null)
                {
                    MSSQLTableDiff tableDiff = CreateTableDiff(newDbTable, oldDbTable);
                    changedTables.Add(tableDiff);
                }
            }

            IEnumerable<MSSQLUserDefinedTypeInfo> addedUserDefinedTypes = newDatabase.UserDefinedTypes
                .Where(newType => !oldDatabase.UserDefinedTypes.Any(oldType => oldType.ID == newType.ID));

            IEnumerable<MSSQLUserDefinedTypeInfo> removedUserDefinedTypes = oldDatabase.UserDefinedTypes
                .Where(oldType => !newDatabase.UserDefinedTypes.Any(newType => newType.ID == oldType.ID));

            return new MSSQLDatabaseDiff
            {
                NewDatabase = newDatabase,
                OldDatabase = oldDatabase,
                AddedTables = addedTables,
                RemovedTables = removedTables,
                ChangedTables = changedTables,
                AddedViews = new List<MSSQLViewInfo>(),
                RemovedViews = new List<MSSQLViewInfo>(),
                ChangedViews = new List<MSSQLViewDiff>(),
                AddedFunctions = new List<MSSQLFunctionInfo>(),
                RemovedFunctions = new List<MSSQLFunctionInfo>(),
                ChangedFunctions = new List<MSSQLFunctionDiff>(),
                AddedUserDefinedTypes = addedUserDefinedTypes,
                RemovedUserDefinedTypes = removedUserDefinedTypes,
                ChangedUserDefinedTypes = new List<MSSQLUserDefinedTypeDiff>(),
            };
        }

        private static MSSQLTableDiff CreateTableDiff(MSSQLTableInfo newDbTable, MSSQLTableInfo oldDbTable)
        {
            IEnumerable<MSSQLColumnInfo> addedColumns = newDbTable.Columns
                .Where(newDbTableColumn => !oldDbTable.Columns.Any(oldDbTableColum => oldDbTableColum.ID == newDbTableColumn.ID))
                .Select(x => (MSSQLColumnInfo)x);

            IEnumerable<MSSQLColumnInfo> removedColumns = oldDbTable.Columns
                .Where(oldDbTableColumn => !newDbTable.Columns.Any(newDbTableColumn => newDbTableColumn.ID == oldDbTableColumn.ID))
                .Select(x => (MSSQLColumnInfo)x);

            List<MSSQLColumnDiff> changedColumns = new();
            foreach (MSSQLColumnInfo newDbTableColumn in newDbTable.Columns)
            {
                MSSQLColumnInfo oldDbTableColumn = (MSSQLColumnInfo)oldDbTable.Columns.FirstOrDefault(x => x.ID == newDbTableColumn.ID);
                if (oldDbTableColumn is not null)
                {
                    MSSQLColumnDiff tableDiff = CreateColumnDiff(newDbTableColumn, oldDbTableColumn);
                    changedColumns.Add(tableDiff);
                }
            }

            return new MSSQLTableDiff
            {
                NewTable = newDbTable,
                OldTable = oldDbTable,
                AddedColumns = addedColumns,
                RemovedColumns = removedColumns,
                ChangedColumns = changedColumns,
                AddedForeignKeys = new List<MSSQLForeignKeyInfo>(),
                RemovedForeignKeys = new List<MSSQLForeignKeyInfo>(),
                ChangedForeignKeys = new List<MSSQLForeignKeyDiff>(),
            };
        }

        private static MSSQLColumnDiff CreateColumnDiff(MSSQLColumnInfo newDbTableColumn, MSSQLColumnInfo oldDbTableColumn)
        {
            return new MSSQLColumnDiff
            {
                NewColumn = newDbTableColumn,
                OldColumn = oldDbTableColumn,
            };
        }
    }
}
