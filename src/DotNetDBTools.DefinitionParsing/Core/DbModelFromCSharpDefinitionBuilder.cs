using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal abstract class DbModelFromCSharpDefinitionBuilder<
    TDatabase,
    TTable,
    TView,
    TColumn>
    where TDatabase : Database, new()
    where TTable : Table, new()
    where TView : View, new()
    where TColumn : Column, new()
{
    protected readonly IDataTypeMapper DataTypeMapper;
    protected readonly IDbObjectCodeMapper DbObjectCodeMapper;
    protected readonly IDefaultValueMapper DefaultValueMapper;

    protected DbModelFromCSharpDefinitionBuilder(
        IDataTypeMapper dataTypeMapper,
        IDbObjectCodeMapper dbObjectCodeMapper,
        IDefaultValueMapper defaultValueMapper)
    {
        DataTypeMapper = dataTypeMapper;
        DbObjectCodeMapper = dbObjectCodeMapper;
        DefaultValueMapper = defaultValueMapper;
    }

    public Database BuildDatabaseModel(Assembly dbAssembly)
    {
        TDatabase database = new();
        database.Name = DbAssemblyInfoHelper.GetDbName(dbAssembly);
        database.Version = DbAssemblyInfoHelper.GetDbVersion(dbAssembly);
        database.Tables = BuildTableModels(dbAssembly);
        database.Views = BuildViewModels(dbAssembly);
        database.Scripts = BuildScriptModels(dbAssembly);
        BuildAdditionalDbObjects(database, dbAssembly);
        return database;
    }
    protected virtual void BuildAdditionalDbObjects(Database database, Assembly dbAssembly) { }

    private List<TTable> BuildTableModels(Assembly dbAssembly)
    {
        IEnumerable<IBaseTable> tables = GetInstancesOfAllTypesImplementingInterface<IBaseTable>(dbAssembly);
        List<TTable> tableModels = new();
        foreach (IBaseTable table in tables)
        {
            TTable tableModel = new()
            {
                ID = table.ID,
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

    private List<TView> BuildViewModels(Assembly dbAssembly)
    {
        IEnumerable<IBaseView> views = GetInstancesOfAllTypesImplementingInterface<IBaseView>(dbAssembly);
        List<TView> viewModels = new();
        foreach (IBaseView view in views)
        {
            TView viewModel = new()
            {
                ID = view.ID,
                Name = view.GetType().Name,
                CodePiece = DbObjectCodeMapper.MapToCodePiece(view),
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
                ID = script.ID,
                Name = script.GetType().Name,
                Kind = (ScriptKind)Enum.Parse(typeof(ScriptKind), script.Type.ToString()),
                MinDbVersionToExecute = script.MinDbVersionToExecute,
                MaxDbVersionToExecute = script.MaxDbVersionToExecute,
                CodePiece = DbObjectCodeMapper.MapToCodePiece(script),
            };
            scriptModels.Add(scriptModel);
        }
        return scriptModels;
    }

    private List<TColumn> BuildColumnModels(IBaseTable table)
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
                    ID = column.ID,
                    Name = x.Name,
                    DataType = dataTypeModel,
                    Nullable = column.Nullable,
                    Identity = column.Identity,
                    Default = DefaultValueMapper.MapDefaultValue(column),
                };
                BuildAdditionalColumnModelProperties(columnModel, column, table.GetType().Name);
                return columnModel;
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
                    ID = pk.ID,
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
                    ID = uc.ID,
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
                    ID = ck.ID,
                    Name = x.Name,
                    CodePiece = DbObjectCodeMapper.MapToCodePiece(ck),
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
                    ID = fk.ID,
                    Name = x.Name,
                    ThisColumnNames = fk.ThisColumns.ToList(),
                    ReferencedTableName = fk.ReferencedTable,
                    ReferencedTableColumnNames = fk.ReferencedTableColumns.ToList(),
                    OnUpdate = GetOnUpdateActionName(fk),
                    OnDelete = GetOnDeleteActionName(fk),
                };
                BuildAdditionalForeignKeyModelProperties(fkModel, fk, table.GetType().Name);
                return fkModel;
            })
            .ToList();
    }

    protected virtual void BuildAdditionalForeignKeyModelProperties(ForeignKey fkModel, BaseForeignKey fk, string tableName) { }
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
                Index indexModel = new()
                {
                    ID = index.ID,
                    Name = x.Name,
                    Columns = index.Columns.ToList(),
                    IncludeColumns = index.IncludeColumns?.ToList() ?? new List<string>(),
                    Unique = index.Unique,
                };
                BuildAdditionalIndexModelProperties(indexModel, index, table.GetType().Name);
                return indexModel;
            })
            .ToList();
    }
    protected virtual void BuildAdditionalIndexModelProperties(Index indexModel, BaseIndex index, string tableName) { }

    private List<Trigger> BuildTriggerModels(IBaseTable table)
    {
        return table.GetType().GetPropertyOrFieldMembers()
            .Where(x => typeof(BaseTrigger).IsAssignableFrom(x.GetPropertyOrFieldType()))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x =>
            {
                BaseTrigger trigger = (BaseTrigger)x.GetPropertyOrFieldValue(table);
                Trigger triggerModel = new()
                {
                    ID = trigger.ID,
                    Name = x.Name,
                    CodePiece = DbObjectCodeMapper.MapToCodePiece(trigger),
                };
                BuildAdditionalTriggerModelProperties(triggerModel, trigger, table.GetType().Name);
                return triggerModel;
            })
            .ToList();
    }
    protected virtual void BuildAdditionalTriggerModelProperties(Trigger triggerModel, BaseTrigger trigger, string tableName) { }

    protected static IEnumerable<TInterface> GetInstancesOfAllTypesImplementingInterface<TInterface>(Assembly dbAssembly)
    {
        return dbAssembly.GetTypes()
            .Where(x => x.GetInterfaces()
                .Any(y => y == typeof(TInterface)))
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(x => (TInterface)Activator.CreateInstance(x));
    }
}
