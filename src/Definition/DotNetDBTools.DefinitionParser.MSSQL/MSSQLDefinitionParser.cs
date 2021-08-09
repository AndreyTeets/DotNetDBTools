using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Models.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetDBTools.DefinitionParser.MSSQL
{
    public class MSSQLDefinitionParser
    {
        public static MSSQLDatabaseInfo CreateDatabaseInfo(string dbAssemblyPath)
        {
            throw new NotImplementedException();
        }

        public static MSSQLDatabaseInfo CreateDatabaseInfo(Assembly dbAssembly)
        {
            IEnumerable<ITable> tables = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(ITable)))
                .Select(x => (ITable)Activator.CreateInstance(x));

            IEnumerable<IView> views = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(IView)))
                .Select(x => (IView)Activator.CreateInstance(x));

            IEnumerable<IFunction> functions = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(IFunction)))
                .Select(x => (IFunction)Activator.CreateInstance(x));

            return new MSSQLDatabaseInfo()
            {
                Tables = GetTableInfos(tables),
                Views = GetViewInfos(views),
                Functions = GetFunctionInfos(functions),
            };
        }

        private static List<MSSQLTableInfo> GetTableInfos(IEnumerable<ITable> tables)
        {
            List<MSSQLTableInfo> tableInfos = new();
            foreach (ITable table in tables)
            {
                MSSQLTableInfo tableInfo = new()
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

        private static List<MSSQLViewInfo> GetViewInfos(IEnumerable<IView> views)
        {
            List<MSSQLViewInfo> viewInfos = new();
            foreach (IView view in views)
            {
                MSSQLViewInfo viewInfo = new()
                {
                    ID = view.ID,
                    Name = view.GetType().Name,
                    Code = view.Code,
                };
                viewInfos.Add(viewInfo);
            }
            return viewInfos;
        }

        private static List<MSSQLFunctionInfo> GetFunctionInfos(IEnumerable<IFunction> functions)
        {
            List<MSSQLFunctionInfo> functionInfos = new();
            foreach (IFunction function in functions)
            {
                MSSQLFunctionInfo functionInfo = new()
                {
                    ID = function.ID,
                    Name = function.GetType().Name,
                    Code = function.Code,
                };
                functionInfos.Add(functionInfo);
            }
            return functionInfos;
        }

        private static List<MSSQLColumnInfo> GetColumnInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(Column).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .Select(x =>
                {
                    Column column = (Column)x.GetPropertyOrFieldValue(table);
                    return new MSSQLColumnInfo()
                    {
                        ID = column.ID,
                        Name = x.Name,
                        DataType = column.Type.GetType().Name,
                        DefaultValue = column.Default,
                    };
                })
                .ToList();

        private static List<MSSQLForeignKeyInfo> GetForeignKeyInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(ForeignKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .Select(x =>
                {
                    ForeignKey foreignKey = (ForeignKey)x.GetPropertyOrFieldValue(table);
                    return new MSSQLForeignKeyInfo()
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
