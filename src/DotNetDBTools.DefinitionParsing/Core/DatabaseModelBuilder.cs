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
            where T : TableInfo, new()
        {
            IEnumerable<IBaseTable> tables = GetInstancesOfAllTypesImplementingInterface<IBaseTable>(dbAssembly);
            List<T> tableInfos = new();
            foreach (IBaseTable table in tables)
            {
                T tableInfo = new()
                {
                    ID = table.ID,
                    Name = table.GetType().Name,
                    Columns = BuildColumnModels(table),
                    PrimaryKey = BuildPrimaryKeyModels(table),
                    UniqueConstraints = BuildUniqueConstraintModels(table),
                    CheckConstraints = new List<CheckConstraintInfo>(),
                    Indexes = new List<IndexInfo>(),
                    Triggers = new List<TriggerInfo>(),
                    ForeignKeys = BuildForeignKeyModels(table),
                };
                BuildAdditionalTableModelProperties(tableInfo, table);
                tableInfos.Add(tableInfo);
            }
            return tableInfos;
        }

        protected virtual void BuildAdditionalTableModelProperties<T>(T tableInfo, IBaseTable table)
            where T : TableInfo, new()
        {
        }

        protected List<T> BuildViewModels<T>(Assembly dbAssembly)
            where T : ViewInfo, new()
        {
            IEnumerable<IBaseView> views = GetInstancesOfAllTypesImplementingInterface<IBaseView>(dbAssembly);
            List<T> viewInfos = new();
            foreach (IBaseView view in views)
            {
                T viewInfo = new()
                {
                    ID = view.ID,
                    Name = view.GetType().Name,
                    Code = view.Code,
                };
                BuildAdditionalViewModelProperties(viewInfo, view);
                viewInfos.Add(viewInfo);
            }
            return viewInfos;
        }

        protected virtual void BuildAdditionalViewModelProperties<T>(T viewInfo, IBaseView view)
            where T : ViewInfo, new()
        {
        }

        private List<ColumnInfo> BuildColumnModels(IBaseTable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(BaseColumn).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    BaseColumn column = (BaseColumn)x.GetPropertyOrFieldValue(table);
                    DataTypeInfo dataTypeInfo = DataTypeMapper.GetDataTypeInfo(column.DataType);
                    ColumnInfo columnInfo = new()
                    {
                        ID = column.ID,
                        Name = x.Name,
                        DataType = dataTypeInfo,
                        Nullable = column.Nullable,
                        Identity = column.Identity,
                        Default = DefaultValueMapper.MapDefaultValue(column),
                        DefaultConstraintName = column.Default is not null
                            ? column.DefaultConstraintName ?? $"DF_{table.GetType().Name}_{x.Name}"
                            : null,
                    };
                    BuildAdditionalColumnModelProperties(columnInfo, column);
                    return columnInfo;
                })
                .ToList();

        protected virtual void BuildAdditionalColumnModelProperties(ColumnInfo columnInfo, BaseColumn column)
        {
        }

        private PrimaryKeyInfo BuildPrimaryKeyModels(IBaseTable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(BasePrimaryKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    BasePrimaryKey pk = (BasePrimaryKey)x.GetPropertyOrFieldValue(table);
                    PrimaryKeyInfo pkInfo = new()
                    {
                        ID = pk.ID,
                        Name = x.Name,
                        Columns = pk.Columns.ToList(),
                    };
                    BuildAdditionalPrimaryKeyModelProperties(pkInfo, pk);
                    return pkInfo;
                })
                .SingleOrDefault();

        protected virtual void BuildAdditionalPrimaryKeyModelProperties(PrimaryKeyInfo pkInfo, BasePrimaryKey pk)
        {
        }

        private List<UniqueConstraintInfo> BuildUniqueConstraintModels(IBaseTable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(BaseUniqueConstraint).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    BaseUniqueConstraint uc = (BaseUniqueConstraint)x.GetPropertyOrFieldValue(table);
                    UniqueConstraintInfo ucInfo = new()
                    {
                        ID = uc.ID,
                        Name = x.Name,
                        Columns = uc.Columns.ToList(),
                    };
                    BuildAdditionalUniqueConstraintModelProperties(ucInfo, uc);
                    return ucInfo;
                })
                .ToList();

        protected virtual void BuildAdditionalUniqueConstraintModelProperties(UniqueConstraintInfo ucInfo, BaseUniqueConstraint uc)
        {
        }

        private List<ForeignKeyInfo> BuildForeignKeyModels(IBaseTable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(BaseForeignKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    BaseForeignKey fk = (BaseForeignKey)x.GetPropertyOrFieldValue(table);
                    ForeignKeyInfo fkInfo = new()
                    {
                        ID = fk.ID,
                        Name = x.Name,
                        ThisColumnNames = fk.ThisColumns.ToList(),
                        ReferencedTableName = fk.ReferencedTable,
                        ReferencedTableColumnNames = fk.ReferencedTableColumns.ToList(),
                        OnUpdate = GetOnUpdateActionName(fk),
                        OnDelete = GetOnDeleteActionName(fk),
                    };
                    BuildAdditionalForeignKeyModelProperties(fkInfo, fk);
                    return fkInfo;
                })
                .ToList();

        protected virtual void BuildAdditionalForeignKeyModelProperties(ForeignKeyInfo fkInfo, BaseForeignKey fk)
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
