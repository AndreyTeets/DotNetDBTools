using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParser.SQLite
{
    public class SQLiteDefinitionParser
    {
        public static SQLiteDatabaseInfo CreateDatabaseInfo(string dbAssemblyPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            return CreateDatabaseInfo(dbAssembly);
        }

        public static SQLiteDatabaseInfo CreateDatabaseInfo(Assembly dbAssembly)
        {
            IEnumerable<ITable> tables = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(ITable)))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x => (ITable)Activator.CreateInstance(x));

            IEnumerable<IView> views = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(IView)))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x => (IView)Activator.CreateInstance(x));

            return new SQLiteDatabaseInfo(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = GetTableInfos(tables),
                Views = GetViewInfos(views),
            };
        }

        private static List<SQLiteTableInfo> GetTableInfos(IEnumerable<ITable> tables)
        {
            List<SQLiteTableInfo> tableInfos = new();
            foreach (ITable table in tables)
            {
                SQLiteTableInfo tableInfo = new()
                {
                    ID = table.ID,
                    Name = table.GetType().Name,
                    Columns = GetColumnInfos(table),
                    PrimaryKey = GetPrimaryKeyInfo(table),
                    UniqueConstraints = GetUniqueConstraintsInfos(table),
                    ForeignKeys = GetForeignKeyInfos(table),
                };
                tableInfos.Add(tableInfo);
            }
            return tableInfos;
        }

        private static List<SQLiteViewInfo> GetViewInfos(IEnumerable<IView> views)
        {
            List<SQLiteViewInfo> viewInfos = new();
            foreach (IView view in views)
            {
                SQLiteViewInfo viewInfo = new()
                {
                    ID = view.ID,
                    Name = view.GetType().Name,
                    Code = view.Code,
                };
                viewInfos.Add(viewInfo);
            }
            return viewInfos;
        }

        private static List<ColumnInfo> GetColumnInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(Column).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    Column column = (Column)x.GetPropertyOrFieldValue(table);
                    DataTypeInfo dataTypeInfo = SQLiteDataTypeMapper.GetDataTypeInfo(column.DataType);
                    return new ColumnInfo()
                    {
                        ID = column.ID,
                        Name = x.Name,
                        DataType = dataTypeInfo,
                        Nullable = column.Nullable,
                        Identity = column.Identity,
                        Default = SQLiteDefaultValueMapper.MapDefaultValue(column),
                    };
                })
                .ToList();

        private static PrimaryKeyInfo GetPrimaryKeyInfo(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(PrimaryKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    PrimaryKey primaryKey = (PrimaryKey)x.GetPropertyOrFieldValue(table);
                    return new PrimaryKeyInfo()
                    {
                        ID = primaryKey.ID,
                        Name = x.Name,
                        Columns = primaryKey.Columns.ToList(),
                    };
                })
                .SingleOrDefault();

        private static List<UniqueConstraintInfo> GetUniqueConstraintsInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(UniqueConstraint).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    UniqueConstraint uniqueConstraint = (UniqueConstraint)x.GetPropertyOrFieldValue(table);
                    return new UniqueConstraintInfo()
                    {
                        ID = uniqueConstraint.ID,
                        Name = x.Name,
                        Columns = uniqueConstraint.Columns.ToList(),
                    };
                })
                .ToList();

        private static List<ForeignKeyInfo> GetForeignKeyInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(ForeignKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    ForeignKey foreignKey = (ForeignKey)x.GetPropertyOrFieldValue(table);
                    return new ForeignKeyInfo()
                    {
                        ID = foreignKey.ID,
                        Name = x.Name,
                        ThisTableName = table.GetType().Name,
                        ThisColumnNames = foreignKey.ThisColumns.ToList(),
                        ReferencedTableName = foreignKey.ReferencedTable,
                        ReferencedTableColumnNames = foreignKey.ReferencedTableColumns.ToList(),
                        OnDelete = foreignKey.OnDelete.ToString(),
                        OnUpdate = foreignKey.OnUpdate.ToString(),
                    };
                })
                .ToList();
    }
}
