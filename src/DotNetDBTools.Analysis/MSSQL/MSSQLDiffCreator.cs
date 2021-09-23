using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL
{
    public static class MSSQLDiffCreator
    {
        public static MSSQLDatabaseDiff CreateDatabaseDiff(MSSQLDatabaseInfo newDatabase, MSSQLDatabaseInfo oldDatabase)
        {
            List<MSSQLTableInfo> addedTables = newDatabase.Tables
                .Where(newDbTable => !oldDatabase.Tables.Any(oldDbTable => oldDbTable.ID == newDbTable.ID))
                .PutReferencedFirst()
                .Select(x => (MSSQLTableInfo)x)
                .ToList();
            List<MSSQLTableInfo> removedTables = oldDatabase.Tables
                .Where(oldDbTable => !newDatabase.Tables.Any(newDbTable => newDbTable.ID == oldDbTable.ID))
                .PutReferencedLast()
                .Select(x => (MSSQLTableInfo)x)
                .ToList();
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

            List<MSSQLUserDefinedTypeInfo> addedUserDefinedTypes = newDatabase.UserDefinedTypes
                .Where(newType => !oldDatabase.UserDefinedTypes.Any(oldType => oldType.ID == newType.ID))
                .ToList();
            List<MSSQLUserDefinedTypeInfo> removedUserDefinedTypes = oldDatabase.UserDefinedTypes
                .Where(oldType => !newDatabase.UserDefinedTypes.Any(newType => newType.ID == oldType.ID))
                .ToList();
            List<MSSQLUserDefinedTypeDiff> changedUserDefinedTypes = new();
            foreach (MSSQLUserDefinedTypeInfo newType in newDatabase.UserDefinedTypes)
            {
                MSSQLUserDefinedTypeInfo oldType = oldDatabase.UserDefinedTypes.FirstOrDefault(x => x.ID == newType.ID);
                if (oldType is not null)
                {
                    MSSQLUserDefinedTypeDiff udtDiff = new()
                    {
                        NewUserDefinedType = newType,
                        OldUserDefinedType = oldType,
                    };
                    changedUserDefinedTypes.Add(udtDiff);
                }
            }

            MSSQLDatabaseDiff databaseDiff = new()
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
                ChangedUserDefinedTypes = changedUserDefinedTypes,
            };

            ForeignKeysHelper.BuildAllForeignKeysToBeDroppedAndAdded(databaseDiff);
            return databaseDiff;
        }

        private static MSSQLTableDiff CreateTableDiff(MSSQLTableInfo newDbTable, MSSQLTableInfo oldDbTable)
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
                if (oldDbTableColumn is not null)
                {
                    ColumnDiff columnDiff = new ColumnDiff
                    {
                        NewColumn = newDbTableColumn,
                        OldColumn = oldDbTableColumn,
                    };
                    changedColumns.Add(columnDiff);
                }
            }

            PrimaryKeyInfo addedPrimaryKey = newDbTable.PrimaryKey;
            PrimaryKeyInfo removedPrimaryKey = oldDbTable.PrimaryKey;

            List<UniqueConstraintInfo> addedUniqueConstraints = newDbTable.UniqueConstraints
                .Where(newDbTableUC => !oldDbTable.UniqueConstraints.Any(oldDbTableUC => oldDbTableUC.ID == newDbTableUC.ID))
                .ToList();
            List<UniqueConstraintInfo> removedUniqueConstraints = oldDbTable.UniqueConstraints
                .Where(oldDbTableUC => !newDbTable.UniqueConstraints.Any(newDbTableUC => newDbTableUC.ID == oldDbTableUC.ID))
                .ToList();
            foreach (UniqueConstraintInfo newDbTableUC in newDbTable.UniqueConstraints)
            {
                UniqueConstraintInfo oldDbTableUC = oldDbTable.UniqueConstraints.SingleOrDefault(x => x.ID == newDbTableUC.ID);
                if (oldDbTableUC is not null)
                {
                    addedUniqueConstraints.Add(newDbTableUC);
                    removedUniqueConstraints.Add(oldDbTableUC);
                }
            }

            List<ForeignKeyInfo> addedForeignKeys = newDbTable.ForeignKeys
                .Where(newDbTableFK => !oldDbTable.ForeignKeys.Any(oldDbTableFK => oldDbTableFK.ID == newDbTableFK.ID))
                .ToList();
            List<ForeignKeyInfo> removedForeignKeys = oldDbTable.ForeignKeys
                .Where(oldDbTableFK => !newDbTable.ForeignKeys.Any(newDbTableFK => newDbTableFK.ID == oldDbTableFK.ID))
                .ToList();
            foreach (ForeignKeyInfo newDbTableFK in newDbTable.ForeignKeys)
            {
                ForeignKeyInfo oldDbTableFK = oldDbTable.ForeignKeys.SingleOrDefault(x => x.ID == newDbTableFK.ID);
                if (oldDbTableFK is not null)
                {
                    addedForeignKeys.Add(newDbTableFK);
                    removedForeignKeys.Add(oldDbTableFK);
                }
            }

            return new MSSQLTableDiff
            {
                NewTable = newDbTable,
                OldTable = oldDbTable,
                AddedColumns = addedColumns,
                RemovedColumns = removedColumns,
                ChangedColumns = changedColumns,
                AddedPrimaryKey = addedPrimaryKey,
                RemovedPrimaryKey = removedPrimaryKey,
                AddedUniqueConstraints = addedUniqueConstraints,
                RemovedUniqueConstraints = removedUniqueConstraints,
                AddedForeignKeys = addedForeignKeys,
                RemovedForeignKeys = removedForeignKeys,
            };
        }
    }
}
