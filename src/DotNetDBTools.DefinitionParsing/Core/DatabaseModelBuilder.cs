using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core
{
    internal abstract class DatabaseModelBuilder
    {
        protected readonly IDataTypeMapper DataTypeMapper;
        protected readonly IDefaultValueMapper DefaultValueMapper;

        protected DatabaseModelBuilder(
            IDataTypeMapper dataTypeMapper,
            IDefaultValueMapper defaultValueMapper)
        {
            DataTypeMapper = dataTypeMapper;
            DefaultValueMapper = defaultValueMapper;
        }

        protected List<T> BuildTableModels<T>(Assembly dbAssembly)
            where T : Table, new()
        {
            IEnumerable<IBaseTable> tables = GetInstancesOfAllTypesImplementingInterface<IBaseTable>(dbAssembly);
            List<T> tableModels = new();
            foreach (IBaseTable table in tables)
            {
                T tableModel = new()
                {
                    ID = table.ID,
                    Name = table.GetType().Name,
                    Columns = BuildColumnModels(table),
                    PrimaryKey = BuildPrimaryKeyModels(table),
                    UniqueConstraints = BuildUniqueConstraintModels(table),
                    CheckConstraints = new List<CheckConstraint>(),
                    Indexes = new List<Index>(),
                    Triggers = new List<Trigger>(),
                    ForeignKeys = BuildForeignKeyModels(table),
                };
                BuildAdditionalTableModelProperties(tableModel, table);
                tableModels.Add(tableModel);
            }
            return tableModels;
        }

        protected virtual void BuildAdditionalTableModelProperties<T>(T tableModel, IBaseTable table)
            where T : Table, new()
        {
        }

        protected List<T> BuildViewModels<T>(Assembly dbAssembly)
            where T : View, new()
        {
            IEnumerable<IBaseView> views = GetInstancesOfAllTypesImplementingInterface<IBaseView>(dbAssembly);
            List<T> viewModels = new();
            foreach (IBaseView view in views)
            {
                T viewModel = new()
                {
                    ID = view.ID,
                    Name = view.GetType().Name,
                    Code = view.Code,
                };
                BuildAdditionalViewModelProperties(viewModel, view);
                viewModels.Add(viewModel);
            }
            return viewModels;
        }

        protected virtual void BuildAdditionalViewModelProperties<T>(T viewModel, IBaseView view)
            where T : View, new()
        {
        }

        private List<Column> BuildColumnModels(IBaseTable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(BaseColumn).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    BaseColumn column = (BaseColumn)x.GetPropertyOrFieldValue(table);
                    DataType dataTypeModel = DataTypeMapper.MapToDataTypeModel(column.DataType);
                    Column columnModel = new()
                    {
                        ID = column.ID,
                        Name = x.Name,
                        DataType = dataTypeModel,
                        Nullable = column.Nullable,
                        Identity = column.Identity,
                        Default = DefaultValueMapper.MapDefaultValue(column),
                        DefaultConstraintName = column.Default is not null
                            ? column.DefaultConstraintName ?? $"DF_{table.GetType().Name}_{x.Name}"
                            : null,
                    };
                    BuildAdditionalColumnModelProperties(columnModel, column);
                    return columnModel;
                })
                .ToList();

        protected virtual void BuildAdditionalColumnModelProperties(Column columnModel, BaseColumn column)
        {
        }

        private PrimaryKey BuildPrimaryKeyModels(IBaseTable table)
            => table.GetType().GetPropertyOrFieldMembers()
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
                    BuildAdditionalPrimaryKeyModelProperties(pkModel, pk);
                    return pkModel;
                })
                .SingleOrDefault();

        protected virtual void BuildAdditionalPrimaryKeyModelProperties(PrimaryKey pkModel, BasePrimaryKey pk)
        {
        }

        private List<UniqueConstraint> BuildUniqueConstraintModels(IBaseTable table)
            => table.GetType().GetPropertyOrFieldMembers()
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
                    BuildAdditionalUniqueConstraintModelProperties(ucModel, uc);
                    return ucModel;
                })
                .ToList();

        protected virtual void BuildAdditionalUniqueConstraintModelProperties(UniqueConstraint ucModel, BaseUniqueConstraint uc)
        {
        }

        private List<ForeignKey> BuildForeignKeyModels(IBaseTable table)
            => table.GetType().GetPropertyOrFieldMembers()
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
                    BuildAdditionalForeignKeyModelProperties(fkModel, fk);
                    return fkModel;
                })
                .ToList();

        protected virtual void BuildAdditionalForeignKeyModelProperties(ForeignKey fkModel, BaseForeignKey fk)
        {
        }

        protected abstract string GetOnUpdateActionName(BaseForeignKey fk);
        protected abstract string GetOnDeleteActionName(BaseForeignKey fk);

        protected static IEnumerable<TInterface> GetInstancesOfAllTypesImplementingInterface<TInterface>(Assembly dbAssembly)
        {
            return dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(TInterface)))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x => (TInterface)Activator.CreateInstance(x));
        }
    }
}
