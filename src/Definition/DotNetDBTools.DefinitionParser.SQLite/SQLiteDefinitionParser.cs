using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.Models.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetDBTools.DefinitionParser.SQLite
{
    public class SQLiteDefinitionParser
    {
        public static SQLiteDatabaseInfo CreateDatabaseInfo(string dbAssemblyPath)
        {
            throw new NotImplementedException();
        }

        public static SQLiteDatabaseInfo CreateDatabaseInfo(Assembly dbAssembly)
        {
            IEnumerable<ITable> tables = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(ITable)))
                .Select(x => (ITable)Activator.CreateInstance(x));

            IEnumerable<IView> views = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(IView)))
                .Select(x => (IView)Activator.CreateInstance(x));

            return new SQLiteDatabaseInfo()
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

        private static List<SQLiteColumnInfo> GetColumnInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(Column).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .Select(x =>
                {
                    Column column = (Column)x.GetPropertyOrFieldValue(table);
                    return new SQLiteColumnInfo()
                    {
                        ID = column.ID,
                        Name = x.Name,
                        DataType = column.Type.GetType().Name,
                        DefaultValue = column.Default,
                    };
                })
                .ToList();

        private static List<SQLiteForeignKeyInfo> GetForeignKeyInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(ForeignKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .Select(x =>
                {
                    ForeignKey foreignKey = (ForeignKey)x.GetPropertyOrFieldValue(table);
                    return new SQLiteForeignKeyInfo()
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
