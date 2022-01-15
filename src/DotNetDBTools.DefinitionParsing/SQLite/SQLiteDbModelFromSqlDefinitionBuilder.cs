﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.CodeParsing.SQLite;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParsing.SQLite
{
    internal class SQLiteDbModelFromSqlDefinitionBuilder
    {
        private readonly SQLiteDataTypeMapper _dataTypeMapper = new();
        private readonly SQLiteDefaultValueMapper _defaultValueMapper = new();

        public Database BuildDatabaseModel(Assembly dbAssembly)
        {
            SQLiteDatabase database = new();
            database.Name = DbAssemblyInfoHelper.GetDbName(dbAssembly);

            List<ObjectInfo> dbObjects = ParseDbObjectsFromEmbeddedSqlFiles(dbAssembly);
            database.Tables = BuildTableModels(dbObjects.Where(x => x is TableInfo).Select(x => (TableInfo)x));
            database.Views = BuildViewModels(dbObjects.Where(x => x is ViewInfo).Select(x => (ViewInfo)x));

            Dictionary<string, Table> tableNameToTableMap = database.Tables.ToDictionary(x => x.Name, x => x);
            BuildTablesIndexes(tableNameToTableMap, dbObjects.Where(x => x is IndexInfo).Select(x => (IndexInfo)x));
            BuildTablesTriggers(tableNameToTableMap, dbObjects.Where(x => x is TriggerInfo).Select(x => (TriggerInfo)x));

            return database;
        }

        private static List<ObjectInfo> ParseDbObjectsFromEmbeddedSqlFiles(Assembly dbAssembly)
        {
            IEnumerable<string> embeddedSqlFilesResourcePaths = dbAssembly.GetManifestResourceNames()
                .Where(x => x.EndsWith(".sql", StringComparison.OrdinalIgnoreCase));

            List<ObjectInfo> dbObjects = new();
            SQLiteCodeParser parser = new();
            foreach (string sqlFileResourcePath in embeddedSqlFilesResourcePaths)
            {
                using Stream stream = dbAssembly.GetManifestResourceStream(sqlFileResourcePath);
                using StreamReader sr = new(stream);
                string sqlFileText = sr.ReadToEnd();
                ObjectInfo objectInfo = parser.GetModelFromCreateStatement(sqlFileText);
                dbObjects.Add(objectInfo);
            }
            return dbObjects;
        }

        private List<SQLiteTable> BuildTableModels(IEnumerable<TableInfo> tables)
        {
            List<SQLiteTable> tableModels = new();
            foreach (TableInfo table in tables)
            {
                if (!table.ID.HasValue)
                    throw new Exception($"ID is not declared for table '{table.Name}'");

                SQLiteTable tableModel = new()
                {
                    ID = table.ID.Value,
                    Name = table.Name,
                    Columns = BuildColumnModels(table),
                    PrimaryKey = BuildPrimaryKeyModels(table),
                    UniqueConstraints = BuildUniqueConstraintModels(table),
                    CheckConstraints = BuildCheckConstraintModels(table),
                    ForeignKeys = BuildForeignKeyModels(table),
                    Indexes = new List<Index>(),
                    Triggers = new List<Trigger>(),
                };
                tableModels.Add(tableModel);
            }
            return tableModels.OrderByName();
        }

        private List<SQLiteView> BuildViewModels(IEnumerable<ViewInfo> views)
        {
            List<SQLiteView> viewModels = new();
            foreach (ViewInfo view in views)
            {
                if (!view.ID.HasValue)
                    throw new Exception($"ID is not declared for view '{view.Name}'");

                SQLiteView viewModel = new()
                {
                    ID = view.ID.Value,
                    Name = view.Name,
                    CodePiece = new CodePiece { Code = view.Code.NormalizeLineEndings() },
                };
                viewModels.Add(viewModel);
            }
            return viewModels.OrderByName();
        }

        private List<Column> BuildColumnModels(TableInfo table)
        {
            List<Column> columnModels = new();
            foreach (ColumnInfo column in table.Columns)
            {
                if (!column.ID.HasValue)
                    throw new Exception($"ID is not declared for column '{column.Name}' in table '{table.Name}'");

                Column columnModel = new()
                {
                    ID = column.ID.Value,
                    Name = column.Name,
                    DataType = _dataTypeMapper.GetDataTypeModel(column),
                    Nullable = !column.NotNull,
                    Identity = column.Autoincrement,
                    Default = _defaultValueMapper.GetDefaultValueModel(column),
                };
                columnModels.Add(columnModel);
            }
            return columnModels.OrderByName();
        }

        private PrimaryKey BuildPrimaryKeyModels(TableInfo table)
        {
            ConstraintInfo pk = table.Constraints.SingleOrDefault(x => x.Type == ConstraintType.PrimaryKey);
            if (pk is not null)
            {
                if (!pk.ID.HasValue)
                    throw new Exception($"ID is not declared for primary key '{pk.Name}' in table '{table.Name}'");

                PrimaryKey pkModel = new()
                {
                    ID = pk.ID.Value,
                    Name = pk.Name,
                    Columns = pk.Columns,
                };
                return pkModel;
            }
            return null;
        }

        private List<UniqueConstraint> BuildUniqueConstraintModels(TableInfo table)
        {
            List<UniqueConstraint> ucModels = new();
            foreach (ConstraintInfo uc in table.Constraints.Where(x => x.Type == ConstraintType.Unique))
            {
                if (!uc.ID.HasValue)
                    throw new Exception($"ID is not declared for unique constraint '{uc.Name}' in table '{table.Name}'");

                UniqueConstraint ucModel = new()
                {
                    ID = uc.ID.Value,
                    Name = uc.Name,
                    Columns = uc.Columns,
                };
                ucModels.Add(ucModel);
            }
            return ucModels.OrderByName();
        }

        private List<CheckConstraint> BuildCheckConstraintModels(TableInfo table)
        {
            List<CheckConstraint> ckModels = new();
            foreach (ConstraintInfo ck in table.Constraints.Where(x => x.Type == ConstraintType.Check))
            {
                if (!ck.ID.HasValue)
                    throw new Exception($"ID is not declared for check constraint '{ck.Name}' in table '{table.Name}'");

                CheckConstraint ckModel = new()
                {
                    ID = ck.ID.Value,
                    Name = ck.Name,
                    CodePiece = new CodePiece { Code = ck.Code },
                };
                ckModels.Add(ckModel);
            }
            return ckModels.OrderByName();
        }

        private List<ForeignKey> BuildForeignKeyModels(TableInfo table)
        {
            List<ForeignKey> fkModels = new();
            foreach (ConstraintInfo fk in table.Constraints.Where(x => x.Type == ConstraintType.ForeignKey))
            {
                if (!fk.ID.HasValue)
                    throw new Exception($"ID is not declared for foreign key '{fk.Name}' in table '{table.Name}'");

                ForeignKey fkModel = new()
                {
                    ID = fk.ID.Value,
                    Name = fk.Name,
                    ThisColumnNames = fk.Columns,
                    ReferencedTableName = fk.RefTable,
                    ReferencedTableColumnNames = fk.RefColumns,
                    OnUpdate = fk.UpdateAction?.ToUpper() ?? ForeignKeyActions.NoAction,
                    OnDelete = fk.DeleteAction?.ToUpper() ?? ForeignKeyActions.NoAction,
                };
                fkModels.Add(fkModel);
            }
            return fkModels.OrderByName();
        }

        private void BuildTablesIndexes(
            Dictionary<string, Table> tableNameToTableMap,
            IEnumerable<IndexInfo> indexes)
        {
            foreach (IndexInfo index in indexes.OrderBy(x => x.Name, StringComparer.Ordinal))
            {
                if (!index.ID.HasValue)
                    throw new Exception($"ID is not declared for index '{index.Name}'");

                Index indexModel = new()
                {
                    ID = index.ID.Value,
                    Name = index.Name,
                    Columns = index.Columns,
                    IncludeColumns = new List<string>(),
                    Unique = index.Unique,
                };
                ((List<Index>)tableNameToTableMap[index.Table].Indexes).Add(indexModel);
            }
        }

        private void BuildTablesTriggers(
            Dictionary<string, Table> tableNameToTableMap,
            IEnumerable<TriggerInfo> triggers)
        {
            foreach (TriggerInfo trigger in triggers.OrderBy(x => x.Name, StringComparer.Ordinal))
            {
                if (!trigger.ID.HasValue)
                    throw new Exception($"ID is not declared for trigger '{trigger.Name}'");

                Trigger triggerModel = new()
                {
                    ID = trigger.ID.Value,
                    Name = trigger.Name,
                    CodePiece = new CodePiece { Code = trigger.Code.NormalizeLineEndings() },
                };
                ((List<Trigger>)tableNameToTableMap[trigger.Table].Triggers).Add(triggerModel);
            }
        }
    }
}
