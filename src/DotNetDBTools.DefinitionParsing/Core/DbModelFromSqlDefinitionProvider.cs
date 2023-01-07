using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal abstract class DbModelFromSqlDefinitionProvider<
    TDatabase,
    TTable,
    TView,
    TIndex,
    TTrigger,
    TColumn>
    : IDbModelFromSqlDefinitionProvider
    where TDatabase : Database, new()
    where TTable : Table, new()
    where TView : View, new()
    where TIndex : Index, new()
    where TTrigger : Trigger, new()
    where TColumn : Column, new()
{
    protected readonly ICodeParser CodeParser;
    private readonly IAnalysisManager _analysisManager = new AnalysisManager();

    protected DbModelFromSqlDefinitionProvider(
        ICodeParser codeParser)
    {
        CodeParser = codeParser;
    }

    public Database CreateDbModel(Assembly dbAssembly)
    {
        List<ObjectInfo> dbObjects = ParseDbObjectsFromEmbeddedSqlFiles(dbAssembly);
        long dbVersion = DbAssemblyInfoHelper.GetDbVersion(dbAssembly);
        return CreateDbModel(dbObjects, dbVersion);
    }

    public Database CreateDbModel(IEnumerable<string> definitionSqlStatements, long dbVersion)
    {
        List<ObjectInfo> dbObjects = ParseDbObjectsFromDefinitionSqlStatements(definitionSqlStatements);
        return CreateDbModel(dbObjects, dbVersion);
    }

    private List<ObjectInfo> ParseDbObjectsFromEmbeddedSqlFiles(Assembly dbAssembly)
    {
        IEnumerable<string> embeddedSqlFilesResourcePaths = dbAssembly.GetManifestResourceNames()
            .Where(x => x.EndsWith(".sql", StringComparison.OrdinalIgnoreCase));

        List<ObjectInfo> dbObjects = new();
        foreach (string sqlFileResourcePath in embeddedSqlFilesResourcePaths)
        {
            using Stream stream = dbAssembly.GetManifestResourceStream(sqlFileResourcePath);
            using StreamReader sr = new(stream);
            string sqlFileText = sr.ReadToEnd();
            ObjectInfo objectInfo = CodeParser.GetObjectInfo(sqlFileText);
            dbObjects.Add(objectInfo);
        }
        return dbObjects;
    }

    private List<ObjectInfo> ParseDbObjectsFromDefinitionSqlStatements(IEnumerable<string> definitionSqlStatements)
    {
        List<ObjectInfo> dbObjects = new();
        foreach (string statement in definitionSqlStatements)
        {
            ObjectInfo objectInfo = CodeParser.GetObjectInfo(statement);
            dbObjects.Add(objectInfo);
        }
        return dbObjects;
    }

    private Database CreateDbModel(List<ObjectInfo> dbObjects, long dbVersion)
    {
        TDatabase database = new()
        {
            Version = dbVersion,
            Tables = BuildTableModels(dbObjects.OfType<TableInfo>()),
            Views = BuildViewModels(dbObjects.OfType<ViewInfo>()),
            Scripts = BuildScriptModels(dbObjects.OfType<ScriptInfo>()),
        };

        Dictionary<string, Table> tableNameToTableMap = database.Tables.ToDictionary(x => x.Name, x => x);
        BuildTablesIndexes(tableNameToTableMap, dbObjects.OfType<IndexInfo>());
        BuildTablesTriggers(tableNameToTableMap, dbObjects.OfType<TriggerInfo>());

        BuildAdditionalDbObjects(database, dbObjects);
        _analysisManager.DoCreateSpecificDbmsDbModelFromDefinitionPostProcessing(database);
        _analysisManager.DoPostProcessing(database);
        _analysisManager.BuildDependencies(database);
        return database;
    }
    protected virtual void BuildAdditionalDbObjects(Database database, List<ObjectInfo> dbObjects) { }

    private List<Table> BuildTableModels(IEnumerable<TableInfo> tables)
    {
        List<Table> tableModels = new();
        foreach (TableInfo table in tables)
        {
            TTable tableModel = new()
            {
                ID = table.ID.Value,
                Name = table.Name,
                Columns = BuildColumnModels(table),
                PrimaryKey = BuildPrimaryKeyModels(table),
                UniqueConstraints = BuildUniqueConstraintModels(table),
                CheckConstraints = BuildCheckConstraintModels(table),
                ForeignKeys = BuildForeignKeyModels(table),
            };
            BuildAdditionalTableModelProperties(tableModel, table);
            tableModels.Add(tableModel);
        }
        return tableModels;
    }
    protected virtual void BuildAdditionalTableModelProperties(TTable tableModel, TableInfo table) { }

    private List<View> BuildViewModels(IEnumerable<ViewInfo> views)
    {
        List<View> viewModels = new();
        foreach (ViewInfo view in views)
        {
            TView viewModel = new()
            {
                ID = view.ID.Value,
                Name = view.Name,
                CreateStatement = new CodePiece { Code = view.CreateStatement.NormalizeLineEndings() },
            };
            BuildAdditionalViewModelProperties(viewModel, view);
            viewModels.Add(viewModel);
        }
        return viewModels;
    }
    protected virtual void BuildAdditionalViewModelProperties(TView viewModel, ViewInfo view) { }

    private List<Script> BuildScriptModels(IEnumerable<ScriptInfo> scripts)
    {
        List<Script> scriptModels = new();
        foreach (ScriptInfo script in scripts)
        {
            Script scriptModel = new()
            {
                ID = script.ID.Value,
                Name = script.Name,
                Kind = (ScriptKind)Enum.Parse(typeof(ScriptKind), script.Type.ToString()),
                MinDbVersionToExecute = script.MinDbVersionToExecute,
                MaxDbVersionToExecute = script.MaxDbVersionToExecute,
                Text = new CodePiece { Code = script.Text.NormalizeLineEndings() },
            };
            scriptModels.Add(scriptModel);
        }
        return scriptModels;
    }

    private List<Column> BuildColumnModels(TableInfo table)
    {
        List<Column> columnModels = new();
        foreach (ColumnInfo column in table.Columns)
        {
            TColumn columnModel = new()
            {
                ID = column.ID.Value,
                Name = column.Name,
                DataType = new DataType { Name = column.DataType },
                NotNull = column.NotNull,
                Identity = column.Identity,
                Default = new CodePiece { Code = column.Default },
            };
            BuildAdditionalColumnModelProperties(columnModel, column, table.Name);
            columnModels.Add(columnModel);
        }
        return columnModels;
    }
    protected virtual void BuildAdditionalColumnModelProperties(TColumn columnModel, ColumnInfo column, string tableName) { }

    private PrimaryKey BuildPrimaryKeyModels(TableInfo table)
    {
        ConstraintInfo pk = table.Constraints.SingleOrDefault(x => x.Type == ConstraintType.PrimaryKey);
        if (pk is not null)
        {
            PrimaryKey pkModel = new()
            {
                ID = pk.ID.Value,
                Name = pk.Name,
                Columns = pk.Columns,
            };
            BuildAdditionalPrimaryKeyModelProperties(pkModel, pk, table.Name);
            return pkModel;
        }
        return null;
    }
    protected virtual void BuildAdditionalPrimaryKeyModelProperties(PrimaryKey pkModel, ConstraintInfo pk, string tableName) { }

    private List<UniqueConstraint> BuildUniqueConstraintModels(TableInfo table)
    {
        List<UniqueConstraint> ucModels = new();
        foreach (ConstraintInfo uc in table.Constraints.Where(x => x.Type == ConstraintType.Unique))
        {
            UniqueConstraint ucModel = new()
            {
                ID = uc.ID.Value,
                Name = uc.Name,
                Columns = uc.Columns,
            };
            BuildAdditionalUniqueConstraintModelProperties(ucModel, uc, table.Name);
            ucModels.Add(ucModel);
        }
        return ucModels;
    }
    protected virtual void BuildAdditionalUniqueConstraintModelProperties(UniqueConstraint ucModel, ConstraintInfo uc, string tableName) { }

    private List<CheckConstraint> BuildCheckConstraintModels(TableInfo table)
    {
        List<CheckConstraint> ckModels = new();
        foreach (ConstraintInfo ck in table.Constraints.Where(x => x.Type == ConstraintType.Check))
        {
            CheckConstraint ckModel = new()
            {
                ID = ck.ID.Value,
                Name = ck.Name,
                Expression = new CodePiece { Code = ck.Expression },
            };
            BuildAdditionalCheckConstraintModelProperties(ckModel, ck, table.Name);
            ckModels.Add(ckModel);
        }
        return ckModels;
    }
    protected virtual void BuildAdditionalCheckConstraintModelProperties(CheckConstraint ckModel, ConstraintInfo ck, string tableName) { }

    private List<ForeignKey> BuildForeignKeyModels(TableInfo table)
    {
        List<ForeignKey> fkModels = new();
        foreach (ConstraintInfo fk in table.Constraints.Where(x => x.Type == ConstraintType.ForeignKey))
        {
            ForeignKey fkModel = new()
            {
                ID = fk.ID.Value,
                Name = fk.Name,
                ThisTableName = table.Name,
                ThisColumnNames = fk.Columns,
                ReferencedTableName = fk.RefTable,
                ReferencedTableColumnNames = fk.RefColumns,
                OnUpdate = fk.UpdateAction?.ToUpper() ?? ForeignKeyActions.NoAction,
                OnDelete = fk.DeleteAction?.ToUpper() ?? ForeignKeyActions.NoAction,
            };
            BuildAdditionalForeignKeyModelProperties(fkModel, fk);
            fkModels.Add(fkModel);
        }
        return fkModels;
    }
    protected virtual void BuildAdditionalForeignKeyModelProperties(ForeignKey fkModel, ConstraintInfo fk) { }

    private void BuildTablesIndexes(
        Dictionary<string, Table> tableNameToTableMap,
        IEnumerable<IndexInfo> indexes)
    {
        foreach (IndexInfo index in indexes.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            TIndex indexModel = new()
            {
                ID = index.ID.Value,
                Name = index.Name,
                TableName = index.Table,
                Columns = index.Columns,
                Unique = index.Unique,
            };
            BuildAdditionalIndexModelProperties(indexModel, index);
            tableNameToTableMap[index.Table].Indexes.Add(indexModel);
        }
    }
    protected virtual void BuildAdditionalIndexModelProperties(Index indexModel, IndexInfo index) { }

    private void BuildTablesTriggers(
        Dictionary<string, Table> tableNameToTableMap,
        IEnumerable<TriggerInfo> triggers)
    {
        foreach (TriggerInfo trigger in triggers.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            TTrigger triggerModel = new()
            {
                ID = trigger.ID.Value,
                Name = trigger.Name,
                TableName = trigger.Table,
                CreateStatement = new CodePiece { Code = trigger.CreateStatement.NormalizeLineEndings() },
            };
            BuildAdditionalTriggerModelProperties(triggerModel, trigger);
            tableNameToTableMap[trigger.Table].Triggers.Add(triggerModel);
        }
    }
    protected virtual void BuildAdditionalTriggerModelProperties(Trigger triggerModel, TriggerInfo trigger) { }
}
