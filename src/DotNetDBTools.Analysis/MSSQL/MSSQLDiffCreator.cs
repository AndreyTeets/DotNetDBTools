using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL
{
    public static partial class MSSQLDiffCreator
    {
        private static readonly DbObjectsEqualityComparer s_dbObjectsEqualityComparer = new();

        public static MSSQLDatabaseDiff CreateDatabaseDiff(MSSQLDatabaseInfo newDatabase, MSSQLDatabaseInfo oldDatabase)
        {
            List<MSSQLTableInfo> addedTables = newDatabase.Tables
                .Where(newDbTable => !oldDatabase.Tables.Any(oldDbTable => oldDbTable.ID == newDbTable.ID))
                .Select(x => (MSSQLTableInfo)x)
                .ToList();
            List<MSSQLTableInfo> removedTables = oldDatabase.Tables
                .Where(oldDbTable => !newDatabase.Tables.Any(newDbTable => newDbTable.ID == oldDbTable.ID))
                .Select(x => (MSSQLTableInfo)x)
                .ToList();
            List<MSSQLTableDiff> changedTables = new();
            foreach (MSSQLTableInfo newDbTable in newDatabase.Tables)
            {
                MSSQLTableInfo oldDbTable = (MSSQLTableInfo)oldDatabase.Tables.FirstOrDefault(x => x.ID == newDbTable.ID);
                if (oldDbTable is not null &&
                    !s_dbObjectsEqualityComparer.Equals(newDbTable, oldDbTable))
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
                if (oldType is not null &&
                    !s_dbObjectsEqualityComparer.Equals(newType, oldType))
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
                ViewsToCreate = new List<MSSQLViewInfo>(),
                ViewsToDrop = new List<MSSQLViewInfo>(),
                AddedUserDefinedTypes = addedUserDefinedTypes,
                RemovedUserDefinedTypes = removedUserDefinedTypes,
                ChangedUserDefinedTypes = changedUserDefinedTypes,
                UserDefinedTableTypesToCreate = new List<MSSQLUserDefinedTableTypeInfo>(),
                UserDefinedTableTypesToDrop = new List<MSSQLUserDefinedTableTypeInfo>(),
                FunctionsToCreate = new List<MSSQLFunctionInfo>(),
                FunctionsToDrop = new List<MSSQLFunctionInfo>(),
                ProceduresToCreate = new List<MSSQLProcedureInfo>(),
                ProceduresToDrop = new List<MSSQLProcedureInfo>(),
            };

            ForeignKeysHelper.BuildAllForeignKeysToBeDroppedAndCreated(databaseDiff);
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

            return new MSSQLTableDiff
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
