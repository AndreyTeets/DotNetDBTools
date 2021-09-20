using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.DefinitionParser.Agnostic
{
    public static class AgnosticDefinitionParser
    {
        public static AgnosticDatabaseInfo CreateDatabaseInfo(string dbAssemblyPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            return CreateDatabaseInfo(dbAssembly);
        }

        public static AgnosticDatabaseInfo CreateDatabaseInfo(Assembly dbAssembly)
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

            return new AgnosticDatabaseInfo(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = GetTableInfos(tables),
                Views = GetViewInfos(views),
            };
        }

        private static List<AgnosticTableInfo> GetTableInfos(IEnumerable<ITable> tables)
        {
            List<AgnosticTableInfo> tableInfos = new();
            foreach (ITable table in tables)
            {
                AgnosticTableInfo tableInfo = new()
                {
                    ID = table.ID,
                    Name = table.GetType().Name,
                    Columns = GetColumnInfos(table),
                    ForeignKeys = GetForeignKeyInfos(table),
                };
                tableInfos.Add(tableInfo);
            }
            return tableInfos;
        }

        private static List<AgnosticViewInfo> GetViewInfos(IEnumerable<IView> views)
        {
            List<AgnosticViewInfo> viewInfos = new();
            foreach (IView view in views)
            {
                AgnosticViewInfo viewInfo = new()
                {
                    ID = view.ID,
                    Name = view.GetType().Name,
                    Code = view.Code,
                };
                viewInfos.Add(viewInfo);
            }
            return viewInfos;
        }

        private static List<AgnosticColumnInfo> GetColumnInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(Column).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    Column column = (Column)x.GetPropertyOrFieldValue(table);
                    return new AgnosticColumnInfo()
                    {
                        ID = column.ID,
                        Name = x.Name,
                        DataType = AgnosticDataTypeMapper.GetDataTypeInfo(column.DataType),
                        DefaultValue = column.Default,
                    };
                })
                .ToList();

        private static List<AgnosticForeignKeyInfo> GetForeignKeyInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(ForeignKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    ForeignKey foreignKey = (ForeignKey)x.GetPropertyOrFieldValue(table);
                    return new AgnosticForeignKeyInfo()
                    {
                        //ID = foreignKey.ID,
                        Name = x.Name,
                        ThisColumnNames = foreignKey.ThisColumns.ToList(),
                        ForeignTableName = foreignKey.ForeignTable,
                        ForeignColumnNames = foreignKey.ForeignColumns.ToList(),
                        OnDelete = foreignKey.OnDelete.ToString(),
                        OnUpdate = foreignKey.OnUpdate.ToString(),
                    };
                })
                .ToList();
    }
}
