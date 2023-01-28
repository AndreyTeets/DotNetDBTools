using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Analysis;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal abstract class DbModelFromCSharpDefinitionProvider<
    TDatabase,
    TTable,
    TView,
    TIndex,
    TTrigger,
    TColumn>
    : IDbModelFromDefinitionProvider
    where TDatabase : Database, new()
    where TTable : Table, new()
    where TView : View, new()
    where TIndex : Index, new()
    where TTrigger : Trigger, new()
    where TColumn : Column, new()
{
    protected readonly IDataTypeMapper DataTypeMapper;
    protected readonly IDbObjectCodeMapper DbObjectCodeMapper;
    protected readonly IDefaultValueMapper DefaultValueMapper;
    private readonly IAnalysisManager _analysisManager = new AnalysisManager();

    protected DbModelFromCSharpDefinitionProvider(
        IDataTypeMapper dataTypeMapper,
        IDbObjectCodeMapper dbObjectCodeMapper,
        IDefaultValueMapper defaultValueMapper)
    {
        DataTypeMapper = dataTypeMapper;
        DbObjectCodeMapper = dbObjectCodeMapper;
        DefaultValueMapper = defaultValueMapper;
    }

    public Database CreateDbModel(Assembly dbAssembly)
    {
        TDatabase database = new()
        {
            Version = DbAssemblyInfoHelper.GetDbVersion(dbAssembly),
            Tables = BuildTableModels(dbAssembly),
            Views = BuildViewModels(dbAssembly),
            Scripts = BuildScriptModels(dbAssembly),
        };
        BuildAdditionalDbObjects(database, dbAssembly);
        if (database.Kind != DatabaseKind.Agnostic)
            _analysisManager.DoCreateSpecificDbmsDbModelFromDefinitionPostProcessing(database);
        _analysisManager.DoPostProcessing(database);
        if (database.Kind != DatabaseKind.Agnostic)
            _analysisManager.BuildDependencies(database);
        return database;
    }
    protected virtual void BuildAdditionalDbObjects(Database database, Assembly dbAssembly) { }

    private List<Table> BuildTableModels(Assembly dbAssembly)
    {
        IEnumerable<IBaseTable> tables = GetInstancesOfAllTypesImplementingInterface<IBaseTable>(dbAssembly);
        List<Table> tableModels = new();
        foreach (IBaseTable table in tables)
        {
            TTable tableModel = new()
            {
                ID = table.DNDBT_OBJECT_ID,
                Name = table.GetType().Name,
                Columns = BuildColumnModels(table),
                PrimaryKey = BuildPrimaryKeyModels(table),
                UniqueConstraints = BuildUniqueConstraintModels(table),
                CheckConstraints = BuildCheckConstraintModels(table),
                Indexes = BuildIndexModels(table),
                Triggers = BuildTriggerModels(table),
                ForeignKeys = BuildForeignKeyModels(table),
            };
            BuildAdditionalTableModelProperties(tableModel, table);
            tableModels.Add(tableModel);
        }
        return tableModels;
    }
    protected virtual void BuildAdditionalTableModelProperties(TTable tableModel, IBaseTable table) { }

    private List<View> BuildViewModels(Assembly dbAssembly)
    {
        IEnumerable<IBaseView> views = GetInstancesOfAllTypesImplementingInterface<IBaseView>(dbAssembly);
        List<View> viewModels = new();
        foreach (IBaseView view in views)
        {
            TView viewModel = new()
            {
                ID = view.DNDBT_OBJECT_ID,
                Name = view.GetType().Name,
                CreateStatement = DbObjectCodeMapper.MapToCodePiece(view),
            };
            BuildAdditionalViewModelProperties(viewModel, view);
            viewModels.Add(viewModel);
        }
        return viewModels;
    }
    protected virtual void BuildAdditionalViewModelProperties(TView viewModel, IBaseView view) { }

    private List<Script> BuildScriptModels(Assembly dbAssembly)
    {
        IEnumerable<IBaseScript> scripts = GetInstancesOfAllTypesImplementingInterface<IBaseScript>(dbAssembly);
        List<Script> scriptModels = new();
        foreach (IBaseScript script in scripts)
        {
            Script scriptModel = new()
            {
                ID = script.DNDBT_OBJECT_ID,
                Name = script.GetType().Name,
                Kind = (ScriptKind)Enum.Parse(typeof(ScriptKind), script.Type.ToString()),
                MinDbVersionToExecute = script.MinDbVersionToExecute,
                MaxDbVersionToExecute = script.MaxDbVersionToExecute,
                Text = DbObjectCodeMapper.MapToCodePiece(script),
            };
            scriptModels.Add(scriptModel);
        }
        return scriptModels;
    }

    private List<Column> BuildColumnModels(IBaseTable table)
    {
        return table.GetType().GetPropertyOrFieldMembers()
            .Where(x => typeof(BaseColumn).IsAssignableFrom(x.GetPropertyOrFieldType()))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x =>
            {
                BaseColumn column = (BaseColumn)x.GetPropertyOrFieldValue(table);
                DataType dataTypeModel = DataTypeMapper.MapToDataTypeModel(column.DataType);
                TColumn columnModel = new()
                {
                    ID = column.DNDBT_OBJECT_ID,
                    Name = x.Name,
                    DataType = dataTypeModel,
                    NotNull = column.NotNull,
                    Identity = column.Identity,
                    Default = DefaultValueMapper.MapToDefaultValueModel(column.Default),
                };
                BuildAdditionalColumnModelProperties(columnModel, column, table.GetType().Name);
                return (Column)columnModel;
            })
            .ToList();
    }
    protected virtual void BuildAdditionalColumnModelProperties(TColumn columnModel, BaseColumn column, string tableName) { }

    private PrimaryKey BuildPrimaryKeyModels(IBaseTable table)
    {
        return table.GetType().GetPropertyOrFieldMembers()
            .Where(x => typeof(BasePrimaryKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x =>
            {
                BasePrimaryKey pk = (BasePrimaryKey)x.GetPropertyOrFieldValue(table);
                PrimaryKey pkModel = new()
                {
                    ID = pk.DNDBT_OBJECT_ID,
                    Name = x.Name,
                    Columns = pk.Columns.ToList(),
                };
                BuildAdditionalPrimaryKeyModelProperties(pkModel, pk, table.GetType().Name);
                return pkModel;
            })
            .SingleOrDefault();
    }
    protected virtual void BuildAdditionalPrimaryKeyModelProperties(PrimaryKey pkModel, BasePrimaryKey pk, string tableName) { }

    private List<UniqueConstraint> BuildUniqueConstraintModels(IBaseTable table)
    {
        return table.GetType().GetPropertyOrFieldMembers()
            .Where(x => typeof(BaseUniqueConstraint).IsAssignableFrom(x.GetPropertyOrFieldType()))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x =>
            {
                BaseUniqueConstraint uc = (BaseUniqueConstraint)x.GetPropertyOrFieldValue(table);
                UniqueConstraint ucModel = new()
                {
                    ID = uc.DNDBT_OBJECT_ID,
                    Name = x.Name,
                    Columns = uc.Columns.ToList(),
                };
                BuildAdditionalUniqueConstraintModelProperties(ucModel, uc, table.GetType().Name);
                return ucModel;
            })
            .ToList();
    }
    protected virtual void BuildAdditionalUniqueConstraintModelProperties(UniqueConstraint ucModel, BaseUniqueConstraint uc, string tableName) { }

    private List<CheckConstraint> BuildCheckConstraintModels(IBaseTable table)
    {
        return table.GetType().GetPropertyOrFieldMembers()
            .Where(x => typeof(BaseCheckConstraint).IsAssignableFrom(x.GetPropertyOrFieldType()))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x =>
            {
                BaseCheckConstraint ck = (BaseCheckConstraint)x.GetPropertyOrFieldValue(table);
                CheckConstraint ckModel = new()
                {
                    ID = ck.DNDBT_OBJECT_ID,
                    Name = x.Name,
                    Expression = DbObjectCodeMapper.MapToCodePiece(ck),
                };
                BuildAdditionalCheckConstraintModelProperties(ckModel, ck, table.GetType().Name);
                return ckModel;
            })
            .ToList();
    }
    protected virtual void BuildAdditionalCheckConstraintModelProperties(CheckConstraint ckModel, BaseCheckConstraint ck, string tableName) { }

    private List<ForeignKey> BuildForeignKeyModels(IBaseTable table)
    {
        return table.GetType().GetPropertyOrFieldMembers()
            .Where(x => typeof(BaseForeignKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x =>
            {
                BaseForeignKey fk = (BaseForeignKey)x.GetPropertyOrFieldValue(table);
                ForeignKey fkModel = new()
                {
                    ID = fk.DNDBT_OBJECT_ID,
                    Name = x.Name,
                    ThisColumnNames = fk.ThisColumns.ToList(),
                    ReferencedTableName = fk.ReferencedTable,
                    ReferencedTableColumnNames = fk.ReferencedTableColumns.ToList(),
                    OnUpdate = GetOnUpdateActionName(fk),
                    OnDelete = GetOnDeleteActionName(fk),
                };
                BuildAdditionalForeignKeyModelProperties(fkModel, fk);
                return fkModel;
            })
            .ToList();
    }

    protected virtual void BuildAdditionalForeignKeyModelProperties(ForeignKey fkModel, BaseForeignKey fk) { }
    protected abstract string GetOnUpdateActionName(BaseForeignKey fk);
    protected abstract string GetOnDeleteActionName(BaseForeignKey fk);
    protected string MapFKActionNameFromDefinitionToModel(string fkDefinitionActionName)
    {
        return fkDefinitionActionName switch
        {
            "NoAction" => ForeignKeyActions.NoAction,
            "Restrict" => ForeignKeyActions.Restrict,
            "Cascade" => ForeignKeyActions.Cascade,
            "SetDefault" => ForeignKeyActions.SetDefault,
            "SetNull" => ForeignKeyActions.SetNull,
            _ => throw new Exception($"Invalid foreign key definition action name '{fkDefinitionActionName}'"),
        };
    }

    private List<Index> BuildIndexModels(IBaseTable table)
    {
        return table.GetType().GetPropertyOrFieldMembers()
            .Where(x => typeof(BaseIndex).IsAssignableFrom(x.GetPropertyOrFieldType()))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x =>
            {
                BaseIndex index = (BaseIndex)x.GetPropertyOrFieldValue(table);
                TIndex indexModel = new()
                {
                    ID = index.DNDBT_OBJECT_ID,
                    Name = x.Name,
                    Columns = GetIndexColumns(index),
                    Unique = index.Unique,
                };
                BuildAdditionalIndexModelProperties(indexModel, index);
                return (Index)indexModel;
            })
            .ToList();
    }
    protected virtual List<string> GetIndexColumns(BaseIndex index)
    {
        return index.Columns.ToList();
    }
    protected virtual void BuildAdditionalIndexModelProperties(Index indexModel, BaseIndex index) { }

    private List<Trigger> BuildTriggerModels(IBaseTable table)
    {
        return table.GetType().GetPropertyOrFieldMembers()
            .Where(x => typeof(BaseTrigger).IsAssignableFrom(x.GetPropertyOrFieldType()))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x =>
            {
                BaseTrigger trigger = (BaseTrigger)x.GetPropertyOrFieldValue(table);
                TTrigger triggerModel = new()
                {
                    ID = trigger.DNDBT_OBJECT_ID,
                    Name = x.Name,
                    CreateStatement = DbObjectCodeMapper.MapToCodePiece(trigger),
                };
                BuildAdditionalTriggerModelProperties(triggerModel, trigger);
                return (Trigger)triggerModel;
            })
            .ToList();
    }
    protected virtual void BuildAdditionalTriggerModelProperties(Trigger triggerModel, BaseTrigger trigger) { }

    protected static IEnumerable<TInterface> GetInstancesOfAllTypesImplementingInterface<TInterface>(Assembly dbAssembly)
    {
        return dbAssembly.GetTypes()
            .Where(x => x.GetInterfaces()
                .Any(y => y == typeof(TInterface)))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x => (TInterface)Activator.CreateInstance(x));
    }
}
